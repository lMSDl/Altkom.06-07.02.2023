using NetTopologySuite.Geometries;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Order : Entity
    {
        private DateTime dateTime;

        //Odpowiednik IsConcurrencyToken z konfiguracji
        //[ConcurrencyCheck]
        public DateTime DateTime
        {
            get => dateTime;
            set
            {
                dateTime = value;
                OnPropertyChanged();
            }
        }

        public int Number { get; }

        public string? Name { get; set; }
        public OrderType Type { get; set; }

        public Point? DeliveryPoint {get; set;}

        public virtual ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
    }
}