using DAL.Configurations;
using Microsoft.EntityFrameworkCore;
using Models;
using Pluralize.NET.Core;

namespace DAL
{
    public class Context : DbContext
    {
        public static Func<Context, DateTime, DateTime, IEnumerable<Order>> GetOrdersByDateRange { get; } =
            EF.CompileQuery((Context context, DateTime from, DateTime to) =>
                context.Set<Order>()
            .AsNoTracking()
            .Include(x => x.Products)
            .Where(x => x.DateTime >= from)
            .Where(x => x.DateTime <= to));


        public Context() { }
        public Context(DbContextOptions options) : base(options)
        { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfiguration(new OrderConfiguration());
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            //włączenie śledzenia zmian na podstawie notyfikacji (wymaga implementacji INotifyPropertyChanged i/lub INotifyPropertyChanging) oraz wykorzystanie obserwowalnych kolekcji
            //modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangedNotifications);

            modelBuilder.Model.GetEntityTypes()
                .SelectMany(x => x.GetProperties())
                .Where(x => x.Name.EndsWith("PK"))
                .ToList()
                .ForEach(x => {
                    x.IsNullable = false;
                    x.DeclaringEntityType.SetPrimaryKey(x);
                });

            modelBuilder.Model.GetEntityTypes()
                .ToList()
                .ForEach(x =>
                {
                    x.SetTableName(new Pluralizer().Pluralize(x.GetDefaultTableName()));

                    /*var tableName = x.GetDefaultTableName();

                    if (tableName.EndsWith("y"))
                        tableName = tableName.Substring(0, tableName.Length - 1) + "ies";
                    else
                        tableName += "s";

                    x.SetTableName(tableName);*/

                });

            modelBuilder.Model.GetEntityTypes()
                .SelectMany(x => x.GetProperties())
                .Where(x => x.PropertyInfo?.PropertyType == typeof(string))
                .ToList()
                .ForEach(x => {
                    x.IsNullable = true;
                    x.SetColumnName("s_" + x.GetColumnBaseName());
                });
        }


        public bool RandomFail { get; set; }

        public override int SaveChanges()
        {
            if(RandomFail && new Random().Next(1, 25) == 1)
            {
                throw new Exception();
            }

            return base.SaveChanges();
        }
    }
}