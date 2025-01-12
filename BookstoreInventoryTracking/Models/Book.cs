using System.ComponentModel;

namespace BookstoreInventoryTracking.Models
{
    public class Book
    {
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public string Isbn = string.Empty;
        public string Name = string.Empty;
        public string Author = string.Empty;
        public string Location = string.Empty;
        public double Price = 0.0;
        public int Quantity = 0;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}