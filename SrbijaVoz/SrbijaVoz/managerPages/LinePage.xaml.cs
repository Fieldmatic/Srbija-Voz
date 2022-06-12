using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
using SrbijaVoz.managerWindows.help;
using SrbijaVoz.managerWindows.line;
using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Line = SrbijaVoz.model.Line;

namespace SrbijaVoz.managerWindows
{
    /// <summary>
    /// Interaction logic for LinePage.xaml
    /// </summary>
    public partial class LinePage : Page
    {
        private readonly Database Database;
        public ObservableCollection<LineRecord> LineRecords = new ObservableCollection<LineRecord>();
        private LineRecord? LineRecord;

        public delegate Point GetPosition(IInputElement element);
        private int selectedRowIndex = -1;

        public delegate void RefreshLines();
        public event RefreshLines RefreshLinesListEvent;

        public LinePage(Database database, ManagerWindow managerWindow)
        {
            InitializeComponent();
            Database = database;

            managerWindow.CommandBindings.Clear();
            managerWindow.InitializeManagerShortcuts();
            InitializeLinePageShortcuts(managerWindow);

            InitializeLines();
        }

        #region Shortcuts

        private void InitializeLinePageShortcuts(ManagerWindow managerWindow)
        {
            RoutedCommand addNewLine = new RoutedCommand();
            addNewLine.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            managerWindow.CommandBindings.Add(new CommandBinding(addNewLine, AddLine_Executed));

            RoutedCommand editLine = new RoutedCommand();
            editLine.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));
            managerWindow.CommandBindings.Add(new CommandBinding(editLine, EditLine_Executed));

            RoutedCommand deleteLine = new RoutedCommand();
            deleteLine.InputGestures.Add(new KeyGesture(Key.Delete, ModifierKeys.Shift));
            managerWindow.CommandBindings.Add(new CommandBinding(deleteLine, DeleteLine_Executed));
        }

        private void AddLine_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddLine_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var form = new CreateLine(Database);
            RefreshLinesListEvent += new RefreshLines(InitializeLines);
            form.Create = RefreshLinesListEvent;
            form.ShowDialog();
        }

        private void EditLine_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void EditLine_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LineRecord lineRecord = (LineRecord)LineDataGrid.SelectedItem;
            if (lineRecord == null) return;
            var updateLineWindow = new UpdateLine(Database, lineRecord);
            RefreshLinesListEvent += new RefreshLines(InitializeLines);
            updateLineWindow.Update = RefreshLinesListEvent;
            updateLineWindow.ShowDialog();
        }

        private void DeleteLine_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DeleteLine_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LineRecord lineRecord = (LineRecord)LineDataGrid.SelectedItem;
            if (lineRecord == null) return;
            DeleteLine(lineRecord);
        }


        #endregion

        private void playDemo(object sender, RoutedEventArgs e)
        {
            DemoVideo m = new DemoVideo(@"../../../demo/Linije.mp4");
            m.ShowDialog();
        }

        private void InitializeLines()
        {
            LineRecords = GetLineGridData();
            LineDataGrid.DataContext = LineRecords;
            LineRecord = null;
        }

        private ObservableCollection<LineRecord> GetLineGridData()
        {
            ObservableCollection<LineRecord> lineRecordData = new();
            foreach (Line line in Database.Lines) lineRecordData.Add(new LineRecord(line));
            return lineRecordData;
        }

        private void UpdateLine_Drop(object sender, DragEventArgs e)
        {
            LineRecord = e.Data.GetData("LineRecord") as LineRecord;
            UpdateLine updateLineWindow = new(Database, LineRecord);
            RefreshLinesListEvent += new RefreshLines(InitializeLines);
            updateLineWindow.Update = RefreshLinesListEvent;
            updateLineWindow.Show();

        }

        private void DeleteLine_Drop(object sender, DragEventArgs e)
        {
            LineRecord = e.Data.GetData("LineRecord") as LineRecord;
            DeleteLine(LineRecord);
        }

        private void DeleteLine(LineRecord lineRecord)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da obrišete ovu liniju?",
                                                      "Brisanje Linije",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Line lineForDelete = Database.Lines.Find(item => item.Id == lineRecord.Id);
                Database.Lines.Remove(lineForDelete);
                MessageBox.Show("Linija uspešno obrisana!",
                                "Brisanje linije",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                this.LineDataGrid.DataContext = null;
                InitializeLines();
            }
        }

        private void LineDataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                selectedRowIndex = GetCurrentRowIndex(e.GetPosition);
                if (selectedRowIndex < 0)
                    return;

                LineDataGrid.SelectedIndex = selectedRowIndex;
                if (LineRecords[selectedRowIndex] is not LineRecord selectedEmp)
                    return;
                var dataObject = new DataObject();
                dataObject.SetData("LineRecord", selectedEmp);
                DragDrop.DoDragDrop(LineDataGrid, dataObject, DragDropEffects.Move);
            }
        }

        private int GetCurrentRowIndex(GetPosition pos)
        {
            int curIndex = -1;
            for (int i = 0; i < LineDataGrid.Items.Count; i++)
            {
                DataGridRow itm = GetRowItem(i);
                if (GetMouseTargetRow(itm, pos))
                {
                    curIndex = i;
                    break;
                }
            }
            return curIndex;
        }

        private static bool GetMouseTargetRow(Visual theTarget, GetPosition position)
        {
            Rect rect = VisualTreeHelper.GetDescendantBounds(theTarget);
            Point point = position((IInputElement)theTarget);
            return rect.Contains(point);
        }

        private DataGridRow GetRowItem(int index)
        {
            if (LineDataGrid.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;
            return LineDataGrid.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
        }

        private void ShowLineRoute(object sender, RoutedEventArgs e)
        {
            int ID = (int) ((Button)sender).CommandParameter;
            Line line = Database.GetLineById(ID);
            LineNetworkView lineNetworkView = new LineNetworkView(line);
            lineNetworkView.Show();
        }

        private void LineDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditBtn.IsEnabled = true;
            DeleteBtn.IsEnabled = true;
        }
    }
}
