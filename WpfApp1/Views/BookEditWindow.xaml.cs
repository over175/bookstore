// Views/BookEditWindow.xaml.cs
using System;
using System.Windows;
using System.Windows.Controls;
using BookStoreWPF.Models;

namespace BookStoreWPF.Views
{
    public partial class BookEditWindow : Window
    {
        public Book Book { get; private set; }

        public BookEditWindow()
        {
            InitializeComponent();
            Book = new Book();
            cmbGenre.SelectedIndex = 0;
            Title = "Добавление книги";
        }

        public BookEditWindow(Book book)
        {
            InitializeComponent();
            Book = book;
            LoadBookData();
            Title = "Редактирование книги";
        }

        private void LoadBookData()
        {
            txtTitle.Text = Book.Title;
            txtAuthor.Text = Book.Author;

            // Устанавливаем жанр
            foreach (ComboBoxItem item in cmbGenre.Items)
            {
                if (item.Content.ToString() == Book.Genre)
                {
                    cmbGenre.SelectedItem = item;
                    break;
                }
            }

            txtPrice.Text = Book.Price.ToString();
            txtDiscount.Text = Book.Discount.ToString();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                Book.Title = txtTitle.Text.Trim();
                Book.Author = txtAuthor.Text.Trim();
                Book.Genre = (cmbGenre.SelectedItem as ComboBoxItem)?.Content.ToString();
                Book.Price = decimal.Parse(txtPrice.Text);
                Book.Discount = decimal.Parse(txtDiscount.Text);

                this.DialogResult = true;
                this.Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Введите название книги", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                MessageBox.Show("Введите автора книги", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!decimal.TryParse(txtDiscount.Text, out decimal discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Скидка должна быть от 0 до 100%", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
