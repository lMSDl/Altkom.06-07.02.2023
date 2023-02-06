using DAL.Configurations;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DAL
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new OrderConfiguration());
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            //włączenie śledzenia zmian na podstawie notyfikacji (wymaga implementacji INotifyPropertyChanged i/lub INotifyPropertyChanging) oraz wykorzystanie obserwowalnych kolekcji
            modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangedNotifications);
        }
    }
}