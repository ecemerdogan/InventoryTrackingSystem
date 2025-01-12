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

        public string Isbn { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double Price { get; set;  } = 0.0;
        public int Quantity { get; set; } = 0;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}