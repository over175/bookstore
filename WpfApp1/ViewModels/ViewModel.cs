
using System.ComponentModel;

namespace BookStoreWPF.ViewModels
{
    public enum UserRole
    {
        Guest,
        Client,
        Manager,
        Administrator
    }

    public class UserViewModel : INotifyPropertyChanged
    {
        private string _login = "Гость";
        private UserRole _role = UserRole.Guest;

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }

        public UserRole Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
                OnPropertyChanged(nameof(IsAdmin));
                OnPropertyChanged(nameof(CanViewOrders));
            }
        }

        public bool IsAdmin => Role == UserRole.Administrator;

        public bool CanViewOrders => Role == UserRole.Manager || Role == UserRole.Administrator;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}