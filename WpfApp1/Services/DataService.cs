
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookStoreWPF.Models;
using BookStoreWPF.ViewModels;

namespace BookStoreWPF.Services
{
    public class DataService
    {
        private ObservableCollection<Book> _books;
        private ObservableCollection<Order> _orders;
        private List<UserViewModel> _users;

        public DataService()
        {
            InitializeTestData();
        }

        // Обновим метод InitializeTestData в DataService.cs
        private void InitializeTestData()
        {
            // Тестовые пользователи
            _users = new List<UserViewModel>
    {
        new UserViewModel { Login = "admin", Role = UserRole.Administrator },
        new UserViewModel { Login = "manager", Role = UserRole.Manager },
        new UserViewModel { Login = "client", Role = UserRole.Client }
    };

            // Тестовые книги
            _books = new ObservableCollection<Book>
    {
        new Book { Id = 1, Title = "Война и мир", Author = "Лев Толстой", Genre = "Классика", Price = 500, Discount = 10 },
        new Book { Id = 2, Title = "Преступление и наказание", Author = "Федор Достоевский", Genre = "Классика", Price = 450, Discount = 0 },
        new Book { Id = 3, Title = "Мастер и Маргарита", Author = "Михаил Булгаков", Genre = "Роман", Price = 600, Discount = 20 },
        new Book { Id = 4, Title = "1984", Author = "Джордж Оруэлл", Genre = "Антиутопия", Price = 350, Discount = 5 },
        new Book { Id = 5, Title = "Гарри Поттер и философский камень", Author = "Джоан Роулинг", Genre = "Фэнтези", Price = 700, Discount = 25 },
        new Book { Id = 6, Title = "Убить пересмешника", Author = "Харпер Ли", Genre = "Роман", Price = 480, Discount = 15 },
        new Book { Id = 7, Title = "Властелин колец", Author = "Дж. Р. Р. Толкин", Genre = "Фэнтези", Price = 850, Discount = 30 }
    };

            // Тестовые заказы
            _orders = new ObservableCollection<Order>
    {
        new Order { Id = 1, UserId = 3, BookId = 1, Quantity = 1, OrderDate = System.DateTime.Now.AddDays(-5), Status = "Выполнен" },
        new Order { Id = 2, UserId = 3, BookId = 3, Quantity = 2, OrderDate = System.DateTime.Now.AddDays(-2), Status = "В обработке" },
        new Order { Id = 3, UserId = 2, BookId = 5, Quantity = 1, OrderDate = System.DateTime.Now.AddDays(-1), Status = "В обработке" },
        new Order { Id = 4, UserId = 1, BookId = 7, Quantity = 3, OrderDate = System.DateTime.Now.AddDays(-7), Status = "Выполнен" }
    };
        }

        public ObservableCollection<Book> GetAllBooks() => _books;

        public ObservableCollection<Order> GetAllOrders() => _orders;

        public UserViewModel AuthenticateUser(string login, string password)
        {
            return _users.FirstOrDefault(u => u.Login == login);
        }

        public void AddBook(Book book)
        {
            book.Id = _books.Count > 0 ? _books.Max(b => b.Id) + 1 : 1;
            _books.Add(book);
        }

        public void UpdateBook(Book book)
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook != null)
            {
                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Genre = book.Genre;
                existingBook.Price = book.Price;
                existingBook.Discount = book.Discount;
            }
        }

        public void DeleteBook(int bookId)
        {
            var book = _books.FirstOrDefault(b => b.Id == bookId);
            if (book != null)
                _books.Remove(book);
        }

        public void AddOrder(Order order)
        {
            order.Id = _orders.Count > 0 ? _orders.Max(o => o.Id) + 1 : 1;
            _orders.Add(order);
        }

        public void UpdateOrder(Order order)
        {
            var existingOrder = _orders.FirstOrDefault(o => o.Id == order.Id);
            if (existingOrder != null)
            {
                existingOrder.BookId = order.BookId;
                existingOrder.Quantity = order.Quantity;
                existingOrder.Status = order.Status;
            }
        }

        public void DeleteOrder(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
                _orders.Remove(order);
        }
    }
}