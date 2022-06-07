using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        public List<Station> Stations { get; set; }

        public Database Database { get; set; }

        public LinePage(List<LineRecord> lineRecords, Database database, ManagerWindow managerWindow)
        {
            InitializeComponent();
            LineDataGrid.ItemsSource = lineRecords;
            Database = database;
            Stations = Database.Stations;

            managerWindow.CommandBindings.Clear();
            managerWindow.InitializeManagerShortcuts();
            InitializeLinePageShortcuts(managerWindow);
        }

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
            var form = new UpdateLine(Database, lineRecord);
            form.ShowDialog();
        }

        private void DeleteLine_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DeleteLine_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LineRecord lineRecord = (LineRecord)LineDataGrid.SelectedItem;
            if (lineRecord == null) return;

            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da obrišete liniju?",
                                "Brisanje linije",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
                return;
            else if (result == MessageBoxResult.Yes)
            {
                Line line = Database.Lines.Where(l => l.Id.Equals(lineRecord.Id)).First();
                Database.Lines.Remove(line);
                MessageBox.Show("Linija uspešn obrisana!",
                                "Brisanje linije",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }
    }
}
