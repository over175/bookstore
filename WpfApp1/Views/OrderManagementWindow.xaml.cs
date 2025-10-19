
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BookStoreWPF.Models;
using BookStoreWPF.Services;

namespace BookStoreWPF.Views
{
    public partial class OrderManagementWindow : Window
    {
        private readonly DataService _dataService;
        private ObservableCollection<Order> _orders;

        public OrderManagementWindow()
        {
            InitializeComponent();
            _dataService = new DataService();
            LoadOrders();
        }

        private void LoadOrders()
        {
            _orders = _dataService.GetAllOrders();
            ordersGrid.ItemsSource = _orders;
            UpdateStatus($"Загружено заказов: {_orders.Count}");
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new OrderEditWindow();
            if (editWindow.ShowDialog() == true)
            {
                _dataService.AddOrder(editWindow.Order);
                LoadOrders();
                UpdateStatus("Заказ успешно добавлен");
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = ordersGrid.SelectedItem as Order;
            if (selectedOrder == null)
            {
                MessageBox.Show("Выберите заказ для редактирования", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editWindow = new OrderEditWindow(selectedOrder);
            if (editWindow.ShowDialog() == true)
            {
                _dataService.UpdateOrder(editWindow.Order);
                LoadOrders();
                UpdateStatus("Заказ успешно обновлен");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = ordersGrid.SelectedItem as Order;
            if (selectedOrder == null)
            {
                MessageBox.Show("Выберите заказ для удаления", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить заказ #{selectedOrder.Id}?",
                "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _dataService.DeleteOrder(selectedOrder.Id);
                LoadOrders();
                UpdateStatus("Заказ успешно удален");
            }
        }

        private void UpdateStatus(string message)
        {
            txtStatus.Text = message;
        }
    }
}