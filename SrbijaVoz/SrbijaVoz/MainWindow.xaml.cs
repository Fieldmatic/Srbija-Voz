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
            CenterWindowOnScreen();
            InitializeShortcut();
        }

        public MainWindow(Database db)
        {
            InitializeComponent();
            database = db;
            InitializeShortcut();
        }

        private void InitializeShortcut()
        {
            RoutedCommand login = new RoutedCommand();
            login.InputGestures.Add(new KeyGesture(Key.Enter));
            this.CommandBindings.Add(new CommandBinding(login, LoginEvent));
        }

        private void LoginEvent(object sender, RoutedEventArgs e)
        {
            bool found = false;
            foreach (Client client in database.Clients)
            {
                if (client.Username.Equals(Username.Text) && client.Password.Equals(Password.Password))
                {
                    found = true;
                    ClientWindow clientWindow = new ClientWindow(database,client);
                    clientWindow.Show();
                    this.Close();
                }
            }

            foreach (Manager manager in database.Managers)
            {
                if (manager.Username.Equals(Username.Text) && manager.Password.Equals(Password.Password))
                {
                    found = true;
                    ManagerWindow managerWindow = new ManagerWindow(database, manager);
                    managerWindow.Show();
                    this.Close();
                }
            }
            if (!found)
            {
                MessageBox.Show("Korisnik sa tim korisnickim imenom i lozinkom ne postoji!", "Neuspesan login", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            
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


        private void Register_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RegisterWindow registerWindow = new(database);
            registerWindow.Show();
            this.Close();
        }

    }
}
