// Views/BookManagementWindow.xaml.cs
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BookStoreWPF.Models;
using BookStoreWPF.Services;
using BookStoreWPF.Views;

namespace BookStoreWPF.Views
{
    public partial class BookManagementWindow : Window
    {
        private readonly DataService _dataService;
        private ObservableCollection<Book> _books;

        public BookManagementWindow()
        {
            InitializeComponent();
            _dataService = new DataService();
            LoadBooks();
        }

        private void LoadBooks()
        {
            _books = _dataService.GetAllBooks();
            booksGrid.ItemsSource = _books;
            UpdateStatus($"Загружено книг: {_books.Count}");
        }

        private void BooksGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var book = e.Row.DataContext as Book;
            if (book != null && book.Discount > 15)
            {
                e.Row.Background = (System.Windows.Media.SolidColorBrush)FindResource("DiscountBrush");
                e.Row.Foreground = System.Windows.Media.Brushes.White;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new BookEditWindow();
            if (editWindow.ShowDialog() == true)
            {
                _dataService.AddBook(editWindow.Book);
                LoadBooks();
                UpdateStatus("Книга успешно добавлена");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = booksGrid.SelectedItem as Book;
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу для редактирования", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editWindow = new BookEditWindow(selectedBook);
            if (editWindow.ShowDialog() == true)
            {
                _dataService.UpdateBook(editWindow.Book);
                LoadBooks();
                UpdateStatus("Книга успешно обновлена");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedBook = booksGrid.SelectedItem as Book;
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу для удаления", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить книгу \"{selectedBook.Title}\"?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _dataService.DeleteBook(selectedBook.Id);
                LoadBooks();
                UpdateStatus("Книга успешно удалена");
            }
        }

        private void UpdateStatus(string message)
        {
            txtStatus.Text = message;
        }
    }
}
