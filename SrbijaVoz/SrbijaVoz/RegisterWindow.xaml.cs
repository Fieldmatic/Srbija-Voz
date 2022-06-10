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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private Database _database;

        public RegisterWindow(Database database)
        {
            InitializeComponent();
            InitializeShortcut();
            _database = database;
        }

        private void InitializeShortcut()
        {
            RoutedCommand register = new();
            register.InputGestures.Add(new KeyGesture(Key.Enter));
            this.CommandBindings.Add(new CommandBinding(register, RegisterButton_Click));
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string name = Name.Text;
            string surname = Surname.Text;
            string username = Username.Text;
            string password = Password.Password;
            string confirmPassword = ConfirmPassword.Password;
            int type = 0;
            if ((bool)Rb1.IsChecked) type = 1;
            else if ((bool)Rb2.IsChecked) type = 2;

            if (!AllFieldsHaveValue(name, surname, username, password, confirmPassword, type))
                return;

            if (!PasswordsMatch(password, confirmPassword))
                return;

            if (type == 1)
            {
                if (ClientExist(username))
                    return;

                Client newClient = new(username, password, name, surname);
                _database.Clients.Add(newClient);
            }
            else
            {
                if (ManagerExist(username))
                    return;

                Manager newManager = new(username, password, name, surname);
                _database.Managers.Add(newManager);
            }
            MessageBox.Show("Uspešno ste se registrovali!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            MainWindow mw = new(_database);
            mw.Show();
            this.Close();
        }

        private bool ManagerExist(string username)
        {
            Manager? manager = _database.Managers.Find(m => m.Username.Equals(username));
            if (manager != null)
            {
                MessageBox.Show("Korisničko ime već postoji!",
                            "Greška",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                return true;
            }
            return false;
        }

        private bool ClientExist(string username)
        {
            Client? client = _database.Clients.Find(c => c.Username.Equals(username));
            if (client != null)
            {
                MessageBox.Show("Korisničko ime već postoji!",
                            "Greška",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                return true;
            }
            return false;
        }

        private static bool PasswordsMatch(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                MessageBox.Show("Lozinke se ne poklapaju!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private static bool AllFieldsHaveValue(string name, string surname, string username, string password, string confirmPassword, int type)
        {
            if (name == null || surname == null || username == null ||
                password == null || confirmPassword == null || type == 0)
            {
                MessageBox.Show("Sva polja su obavezna!",
                                "Greška",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void Login_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow mw = new(_database);
            mw.Show();
            this.Close();
        }
    }
}
