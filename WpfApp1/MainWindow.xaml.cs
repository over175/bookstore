using System.Windows;
using System.Windows.Controls;
using BookStoreWPF.ViewModels;

namespace BookStoreWPF
{
    public partial class MainWindow : Window
    {
        private UserViewModel _currentUser;

        public MainWindow()
        {
            InitializeComponent();

            
            var loginWindow = new Views.LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                _currentUser = loginWindow.CurrentUser;
                SetupMenuForUser();
                ShowWelcomePage();
            }
            else
            {
                
                Application.Current.Shutdown();
            }
        }

        public MainWindow(UserViewModel user)
        {
            InitializeComponent();
            _currentUser = user;
            SetupMenuForUser();
            ShowWelcomePage();
        }

        private void SetupMenuForUser()
        {
            if (_currentUser == null) return;

            
            bool isAdmin = _currentUser.Role == UserRole.Administrator;
            bool canViewOrders = _currentUser.Role == UserRole.Manager || _currentUser.Role == UserRole.Administrator;

            mnuManageBooks.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
            mnuOrders.Visibility = canViewOrders ? Visibility.Visible : Visibility.Collapsed;
            mnuManageOrders.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ShowWelcomePage()
        {
            var welcomeText = new TextBlock
            {
                Text = $"Добро пожаловать, {_currentUser?.Role} {_currentUser?.Login ?? "Гость"}!",
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            MainFrame.Content = welcomeText;
        }

        private void ViewBooks_Click(object sender, RoutedEventArgs e)
        {
            var bookCatalog = new Views.BookCatalogWindow(_currentUser);
            bookCatalog.Owner = this;
            bookCatalog.ShowDialog();
        }

        private void ManageBooks_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser.Role == UserRole.Administrator)
            {
                var bookManagement = new Views.BookManagementWindow();
                bookManagement.Owner = this;
                bookManagement.ShowDialog();
            }
            else
            {
                MessageBox.Show("Недостаточно прав для управления книгами", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ViewOrders_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser.Role == UserRole.Manager || _currentUser.Role == UserRole.Administrator)
            {
                var ordersWindow = new Views.OrdersWindow(_currentUser);
                ordersWindow.Owner = this;
                ordersWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Недостаточно прав для просмотра заказов", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ManageOrders_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser.Role == UserRole.Administrator)
            {
                var orderManagement = new Views.OrderManagementWindow();
                orderManagement.Owner = this;
                orderManagement.ShowDialog();
            }
            else
            {
                MessageBox.Show("Недостаточно прав для управления заказами", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}