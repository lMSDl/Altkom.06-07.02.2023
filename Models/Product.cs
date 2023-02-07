using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Product : Entity
    {
        private ILazyLoader _lazyLoader;

        public Product()
        {
        }

        public Product(ILazyLoader lazyLoader)
        {
            _lazyLoader = lazyLoader;
        }

        private string n_name = string.Empty;
        private Order? order;

        public string Name
        {
            get => n_name;
            set
            {
                n_name = value;
                OnPropertyChanged();
            }
        }
        public float Price { get; set; }

        //ShadowProperty dla referencji
        //public int OrderId { get; set; }
        public virtual Order? Order { get => _lazyLoader?.Load(this, ref order) ?? order; set => order = value; }

        //Odpowiednik IsRowVersion z konfiguracji
        //[Timestamp]
        public byte[] Timestamp { get; set; }

        public string Description { get; }

        public virtual ProductDetails? Details { get; set; }
    }
}