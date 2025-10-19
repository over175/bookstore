
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BookStoreWPF.Models;
using BookStoreWPF.Services;
using BookStoreWPF.ViewModels;

namespace BookStoreWPF.Views
{
    public partial class OrdersWindow : Window
    {
        private readonly DataService _dataService;
        private readonly UserViewModel _currentUser;
        private ObservableCollection<Order> _allOrders;
        private ObservableCollection<Order> _filteredOrders;

        public OrdersWindow(UserViewModel user)
        {
            InitializeComponent();
            _dataService = new DataService();
            _currentUser = user;
            LoadOrders();
        }

        private void LoadOrders()
        {
            _allOrders = _dataService.GetAllOrders();
            _filteredOrders = new ObservableCollection<Order>(_allOrders);
            ordersGrid.ItemsSource = _filteredOrders;
        }

        private void CmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_allOrders == null) return;

            var filtered = _allOrders.AsEnumerable();

            // Фильтрация по статусу
            if (cmbStatusFilter?.SelectedItem is ComboBoxItem statusItem)
            {
                string selectedStatus = statusItem.Content.ToString();
                if (selectedStatus != "Все статусы")
                {
                    filtered = filtered.Where(o => o.Status == selectedStatus);
                }
            }

            _filteredOrders.Clear();
            foreach (var order in filtered)
            {
                _filteredOrders.Add(order);
            }
        }
    }
}
