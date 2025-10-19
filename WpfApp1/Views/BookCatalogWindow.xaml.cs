using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BookStoreWPF.Models;
using BookStoreWPF.Services;
using BookStoreWPF.ViewModels;

namespace BookStoreWPF.Views
{
    public partial class BookCatalogWindow : Window
    {
        private readonly DataService _dataService;
        private readonly UserViewModel _currentUser;
        private ObservableCollection<Book> _allBooks;
        private ObservableCollection<Book> _filteredBooks;

        public BookCatalogWindow(UserViewModel user)
        {
            InitializeComponent();
            _dataService = new DataService();
            _currentUser = user;

            LoadBooks();
            ApplyUserRestrictions();
        }

        private void LoadBooks()
        {
            _allBooks = _dataService.GetAllBooks();
            _filteredBooks = new ObservableCollection<Book>(_allBooks);
            booksGrid.ItemsSource = _filteredBooks;
        }

        private void ApplyUserRestrictions()
        {
           
            if (_currentUser.Role == UserRole.Guest || _currentUser.Role == UserRole.Client)
            {
                controlPanel.Visibility = Visibility.Collapsed;
            }
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

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void CmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void CmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_allBooks == null) return;

            var filtered = _allBooks.AsEnumerable();

            // Поиск
            if (txtSearch != null && !string.IsNullOrEmpty(txtSearch.Text))
            {
                string searchText = txtSearch.Text.ToLower();
                filtered = filtered.Where(b =>
                    b.Title.ToLower().Contains(searchText) ||
                    b.Author.ToLower().Contains(searchText) ||
                    b.Genre.ToLower().Contains(searchText));
            }

            // Фильтрация по жанру
            if (cmbFilter?.SelectedItem is ComboBoxItem genreItem)
            {
                string selectedGenre = genreItem.Content.ToString();
                if (selectedGenre != "Все жанры")
                {
                    filtered = filtered.Where(b => b.Genre == selectedGenre);
                }
            }

            // Сортировка
            if (cmbSort?.SelectedItem is ComboBoxItem sortItem)
            {
                string sortOption = sortItem.Content.ToString();
                switch (sortOption)
                {
                    case "По названию (А-Я)":
                        filtered = filtered.OrderBy(b => b.Title);
                        break;
                    case "По названию (Я-А)":
                        filtered = filtered.OrderByDescending(b => b.Title);
                        break;
                    case "По цене (возрастание)":
                        filtered = filtered.OrderBy(b => b.FinalPrice);
                        break;
                    case "По цене (убывание)":
                        filtered = filtered.OrderByDescending(b => b.FinalPrice);
                        break;
                }
            }

            _filteredBooks.Clear();
            foreach (var book in filtered)
            {
                _filteredBooks.Add(book);
            }
        }
    }
}
