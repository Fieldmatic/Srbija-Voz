using SrbijaVoz.database;
using SrbijaVoz.dataGridRecord;
using SrbijaVoz.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using Line = SrbijaVoz.model.Line;

namespace SrbijaVoz.managerWindows
{
    /// <summary>
    /// Interaction logic for CreateLineSchedule.xaml
    /// </summary>
    public partial class CreateLineSchedule : Window
    {
        public Database Database { get; set; }

        public Delegate Create;

        public CreateLineSchedule(Database database, List<LineRecord> lineRecords)
        {
            InitializeComponent();
            this.DataContext = this;
            Database = database;
            LineDataGrid.ItemsSource = lineRecords;
        }

        private List<int> GetCheckBoxValues()
        {
            List<int> values = new();
            if (Cb1.IsChecked == true) values.Add(1);
            if (Cb2.IsChecked == true) values.Add(2);
            if (Cb3.IsChecked == true) values.Add(3);
            if (Cb4.IsChecked == true) values.Add(4);
            if (Cb5.IsChecked == true) values.Add(5);
            if (Cb6.IsChecked == true) values.Add(6);
            if (Cb7.IsChecked == true) values.Add(0);
            return values;
        }

        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            LineRecord lineRecord = (LineRecord)LineDataGrid.SelectedItem;
            if (lineRecord == null)
            {
                MessageBox.Show("Niste izabrali nijednu liniju!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            List<int> days = GetCheckBoxValues();
            if (days.Count == 0)
            {
                MessageBox.Show("Niste izabrali nijedan dan!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            Line line = Database.Lines.Find(l => l.Id.Equals(lineRecord.Id));
            Database.LineSchedules.Add(new LineSchedule(line, days, new Dictionary<DateTime, List<int>>()));
            MessageBox.Show("Novi red vožnje uspešno dodat.",
                                "Dodavanje novog reda vožnje",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            Create.DynamicInvoke();
            this.Close();
        }
    }
}
