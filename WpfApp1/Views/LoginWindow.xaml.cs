using System.Windows;
using BookStoreWPF.Services;
using BookStoreWPF.ViewModels;

namespace BookStoreWPF.Views
{
    public partial class LoginWindow : Window
    {
        public UserViewModel CurrentUser { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Password;

            var dataService = new DataService();
            var user = dataService.AuthenticateUser(login, password);

            if (user != null)
            {
                CurrentUser = user;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = new UserViewModel { Login = "Гость", Role = UserRole.Guest };
            this.DialogResult = true;
            this.Close();
        }
    }
}