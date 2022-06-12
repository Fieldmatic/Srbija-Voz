using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
using SrbijaVoz.managerWindows.help;
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
    /// Interaction logic for LineSchedulePage.xaml
    /// </summary>
    public partial class LineSchedulePage : Page
    {

        private readonly Database Database;

        public ObservableCollection<LineScheduleRecord> LineScheduleRecords { get; set; }
        private LineScheduleRecord? LineScheduleRecord;

        public delegate Point GetPosition(IInputElement element);
        private int selectedRowIndex = -1;

        public delegate void RefreshLinesSchedules();
        public event RefreshLinesSchedules RefreshLineSchedulesListEvent;

        public LineSchedulePage(Database database, ManagerWindow managerWindow)
        {
            InitializeComponent();
            Database = database;
            LineScheduleRecords = new ObservableCollection<LineScheduleRecord>();

            managerWindow.CommandBindings.Clear();
            managerWindow.InitializeManagerShortcuts();
            InitializeLinePageShortcuts(managerWindow);

            InitializeLineSchedules();
        }

        private void InitializeLineSchedules()
        {
            LineScheduleRecords = GetLineGridData();
            LineScheduleDataGrid.DataContext = LineScheduleRecords;
            LineScheduleRecord = null;
        }

        private ObservableCollection<LineScheduleRecord> GetLineGridData()
        {
            ObservableCollection<LineScheduleRecord> lineScheduleRecordData = new();
            foreach (LineSchedule lineSchedule in Database.LineSchedules)
                lineScheduleRecordData.Add(new LineScheduleRecord(lineSchedule));
            return lineScheduleRecordData;
        }

        private void playDemo(object sender, RoutedEventArgs e)
        {
            DemoVideo m = new DemoVideo(@"../../../demo/RedoviVoznje.mp4");
            m.ShowDialog();
        }

        private void InitializeLinePageShortcuts(ManagerWindow managerWindow)
        {
            RoutedCommand addNewLineSchedule = new RoutedCommand();
            addNewLineSchedule.InputGestures.Add(new KeyGesture(Key.N, ModifierKeys.Control));
            managerWindow.CommandBindings.Add(new CommandBinding(addNewLineSchedule, AddLineSchedule_Executed));

            RoutedCommand editLineSchedule = new RoutedCommand();
            editLineSchedule.InputGestures.Add(new KeyGesture(Key.E, ModifierKeys.Control));
            managerWindow.CommandBindings.Add(new CommandBinding(editLineSchedule, EditLineSchedule_Executed));

            RoutedCommand deleteLineSchedule = new RoutedCommand();
            deleteLineSchedule.InputGestures.Add(new KeyGesture(Key.Delete, ModifierKeys.Shift));
            managerWindow.CommandBindings.Add(new CommandBinding(deleteLineSchedule, DeleteLineSchedule_Executed));

            RoutedCommand openDemo = new();
            openDemo.InputGestures.Add(new KeyGesture(Key.D, ModifierKeys.Control));
            managerWindow.CommandBindings.Add(new CommandBinding(openDemo, playDemo));
        }

        private void AddLineSchedule_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddLineSchedule_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var form = new CreateLineSchedule(Database, GetLineRecords());
            RefreshLineSchedulesListEvent += new RefreshLinesSchedules(InitializeLineSchedules);
            form.Create = RefreshLineSchedulesListEvent;
            form.ShowDialog();
        }

        private void EditLineSchedule_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void EditLineSchedule_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LineScheduleRecord lineScheduleRecord = (LineScheduleRecord)LineScheduleDataGrid.SelectedItem;
            if (lineScheduleRecord == null) return;
            var form = new UpdateLineSchedule(Database, lineScheduleRecord);
            RefreshLineSchedulesListEvent += new RefreshLinesSchedules(InitializeLineSchedules);
            form.Update = RefreshLineSchedulesListEvent;
            form.ShowDialog();
        }

        private void DeleteLineSchedule_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DeleteLineSchedule_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LineScheduleRecord lineScheduleRecord = (LineScheduleRecord)LineScheduleDataGrid.SelectedItem;
            if (lineScheduleRecord == null) return;
            DeleteLineSchedule(lineScheduleRecord);
        }

        private void UpdateLineSchedule_Drop(object sender, DragEventArgs e)
        {
            LineScheduleRecord = e.Data.GetData("LineScheduleRecord") as LineScheduleRecord;
            UpdateLineSchedule updateLineScheduleWindow = new(Database, LineScheduleRecord);
            RefreshLineSchedulesListEvent += new RefreshLinesSchedules(InitializeLineSchedules);
            updateLineScheduleWindow.Update = RefreshLineSchedulesListEvent;
            updateLineScheduleWindow.Show();

        }

        private void DeleteLineSchedule_Drop(object sender, DragEventArgs e)
        {
            LineScheduleRecord = e.Data.GetData("LineScheduleRecord") as LineScheduleRecord;
            DeleteLineSchedule(LineScheduleRecord);
        }

        private void DeleteLineSchedule(LineScheduleRecord lineScheduleRecord)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da obrišete ovaj red vožnje?",
                                                      "Brisanje reda vožnje",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                LineSchedule lineScheduleForDelete = Database.LineSchedules.Find(item => item.Id == lineScheduleRecord.Id);
                Database.LineSchedules.Remove(lineScheduleForDelete);
                MessageBox.Show("Linija uspešno obrisana!",
                                "Brisanje linije",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                this.LineScheduleDataGrid.DataContext = null;
                InitializeLineSchedules();
            }
        }

        private void LineScheduleDataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedRowIndex = GetCurrentRowIndex(e.GetPosition);
            if (selectedRowIndex < 0)
                return;

            LineScheduleDataGrid.SelectedIndex = selectedRowIndex;
            if (LineScheduleRecords[selectedRowIndex] is not LineScheduleRecord selectedEmp)
                return;
            var dataObject = new DataObject();
            dataObject.SetData("LineScheduleRecord", selectedEmp);
            DragDrop.DoDragDrop(LineScheduleDataGrid, dataObject, DragDropEffects.Move);
        }

        private int GetCurrentRowIndex(GetPosition pos)
        {
            int curIndex = -1;
            for (int i = 0; i < LineScheduleDataGrid.Items.Count; i++)
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
            if (LineScheduleDataGrid.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                return null;
            return LineScheduleDataGrid.ItemContainerGenerator.ContainerFromIndex(index) as DataGridRow;
        }

        private List<LineRecord> GetLineRecords()
        {
            List<LineRecord> lineRecordData = new List<LineRecord>();
            foreach (Line line in Database.Lines) lineRecordData.Add(new LineRecord(line));
            return lineRecordData;
        }

        private void LineScheduleDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LineScheduleRecord lineScheduleRecord = (LineScheduleRecord)LineScheduleDataGrid.SelectedItem;
            if (lineScheduleRecord != null)
            {
                EditBtn.IsEnabled = true;
                DeleteBtn.IsEnabled = true;
            }
            else
            {
                EditBtn.IsEnabled = false;
                DeleteBtn.IsEnabled = false;
            }
        }
    }
}
