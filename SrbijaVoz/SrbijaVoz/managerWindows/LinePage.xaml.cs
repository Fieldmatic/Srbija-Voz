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

namespace SrbijaVoz.managerWindows
{
    /// <summary>
    /// Interaction logic for LinePage.xaml
    /// </summary>
    public partial class LinePage : Page
    {
        public List<Station> Stations { get; set; }

        public Database Database { get; set; }

        public LinePage(List<LineRecord> lineRecords, Database database)
        {
            InitializeComponent();
            LineDataGrid.ItemsSource = lineRecords;
            Database = database;
            Stations = Database.Stations;
        }

        private void AddLine_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddLine_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var form = new AddLineForm(Stations, Database);
            form.ShowDialog();
        }
    }
}
