﻿using System.Collections.ObjectModel;
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

        public virtual ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
    }
}