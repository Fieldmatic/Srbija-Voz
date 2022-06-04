using SrbijaVoz.dataGridRecord;
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
    /// Interaction logic for LineSchedulePage.xaml
    /// </summary>
    public partial class LineSchedulePage : Page
    {
        public LineSchedulePage(List<LineScheduleRecord> lineScheduleRecords)
        {
            InitializeComponent();
            LineDataGrid.ItemsSource = lineScheduleRecords;
        }
    }
}
