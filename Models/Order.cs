using System.Collections.ObjectModel;

namespace Models
{
    public class Order : Entity
    {
        private DateTime dateTime;

        public DateTime DateTime
        {
            get => dateTime;
            set
            {
                dateTime = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
    }
}