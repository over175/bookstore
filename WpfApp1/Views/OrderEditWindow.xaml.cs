// Views/OrderEditWindow.xaml.cs
using System;
using System.Windows;
using System.Windows.Controls;
using BookStoreWPF.Models;

namespace BookStoreWPF.Views
{
    public partial class OrderEditWindow : Window
    {
        public Order Order { get; private set; }

        public OrderEditWindow()
        {
            InitializeComponent();
            Order = new Order { OrderDate = DateTime.Now, Status = "В обработке" };
            cmbStatus.SelectedIndex = 0;
            Title = "Добавление заказа";
        }

        public OrderEditWindow(Order order)
        {
            InitializeComponent();
            Order = order;
            LoadOrderData();
            Title = "Редактирование заказа";
        }

        private void LoadOrderData()
        {
            txtUserId.Text = Order.UserId.ToString();
            txtBookId.Text = Order.BookId.ToString();
            txtQuantity.Text = Order.Quantity.ToString();

            // Устанавливаем статус
            foreach (ComboBoxItem item in cmbStatus.Items)
            {
                if (item.Content.ToString() == Order.Status)
                {
                    cmbStatus.SelectedItem = item;
                    break;
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                Order.UserId = int.Parse(txtUserId.Text);
                Order.BookId = int.Parse(txtBookId.Text);
                Order.Quantity = int.Parse(txtQuantity.Text);
                Order.Status = (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();

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
            if (!int.TryParse(txtUserId.Text, out int userId) || userId <= 0)
            {
                MessageBox.Show("Введите корректный ID пользователя", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!int.TryParse(txtBookId.Text, out int bookId) || bookId <= 0)
            {
                MessageBox.Show("Введите корректный ID книги", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
