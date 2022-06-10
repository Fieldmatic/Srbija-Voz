using SrbijaVoz.clientPages;
using SrbijaVoz.database;
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
using System.Windows.Shapes;

namespace SrbijaVoz
{
    /// <summary>
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        private Database Database;
        private Client Client;
        public ClientWindow(Database database, Client client)
        {
            InitializeComponent();
            CenterWindowOnScreen();
            Database = database;
            Client = client;
        }

        private void LogoutEvent(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(Database);
            mainWindow.Show();
            this.Close();

        }

        private void SwitchToDeparturesPage(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new DeparturesPage(Database, Client);

        }

        private void SwitchToReservedTicketsPage(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new ReservedTicketsPage(Database, Client);

        }

        private void SwitchToBoughtTicketsPage(object sender, RoutedEventArgs e)
        {
            DataFrame.Content = new BoughtTicketsPage(Database, Client);

        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}
