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
using SrbijaVoz.database;
using SrbijaVoz.model;

namespace SrbijaVoz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Database database;

        public MainWindow()
        {
            InitializeComponent();
            database = new Database();
            
        }

        public MainWindow(Database db)
        {
            InitializeComponent();
            database = db;
            
        }

        private void LoginEvent(object sender, RoutedEventArgs e)
        {
            foreach (Client client in database.Clients)
            {
                if (client.Username.Equals(Username.Text) && client.Password.Equals(Password.Password))
                {
                    ClientWindow clientWindow = new ClientWindow(database,client);
                    clientWindow.Show();
                    this.Close();
                }
            }

            foreach (Manager manager in database.Managers)
            {
                if (manager.Username.Equals(Username.Text) && manager.Password.Equals(Password.Password))
                {
                    ManagerWindow managerWindow = new ManagerWindow(database, manager);
                    managerWindow.Show();
                    this.Close();
                }
            }
        }


    }
}
