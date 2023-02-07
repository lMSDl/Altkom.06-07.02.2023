

using DAL;
using Microsoft.EntityFrameworkCore;
using Models;
using System.ComponentModel;
using System.Threading;

var contextOptions = new DbContextOptionsBuilder<Context>()
                        .UseSqlServer(@"Server=(local)\SQLEXPRESS;Database=EFCore;Integrated Security=true")
                        //Włączenie śledzenia zmian na podstawie proxy - wymaga specjalnego tworzenia obiektów (context.CreateProxy) i virtualizacji właściwości encji
                        //.UseChangeTrackingProxies()
                        //Włączenie opóźnionego ładowania - wymaga wirtualizacji właściwości referencji
                        .UseLazyLoadingProxies()
                        .LogTo(x => Console.WriteLine(x))
                        .Options;

using (var context = new Context(contextOptions))
{
    context.Database.EnsureDeleted();
    context.Database.Migrate();
}

for (int i = 0; i < 20; i++)
{

    using (var context = new Context(contextOptions))
    {

    var order = new Order() { Type = (OrderType)(i%3)};
    var product = new Product() { Name = "Kapusta " + i, Details = new ProductDetails { Height = i, Weight = 10 * 1, Width = 12 +i } };
    order.Products.Add(product);
    context.Add(order);
    context.SaveChanges();
    }
}

using (var context = new Context(contextOptions))
{
    //odczyt wartości ShadowProperty
    var product = context.Set<Product>().Include(x => x.Details).Where(x => EF.Property<int>(x, "OrderId") == 2).First();
    //zapis wartości ShadowProperty
    context.Entry(product).Property("OrderId").CurrentValue = 3;

    product.Name = "Sałata";
    context.SaveChanges();
    //Procedures(context);

    var orderSummaries = context.Set<OrderSummary>().ToList();


}








static void ChangeTracker(Context context)
{
    //wyłączenie automatycznego wykrywania zmian
    //AutoDetectChanges działa w przypadku wywołania Entries, Local, SaveChanges
    //context.ChangeTracker.AutoDetectChangesEnabled = false;



    var order = new Order();
    var product = new Product() { Name = "Kapusta", Price = 15/*, Id = 1*/ };
    order.Products.Add(product);



    /*var order = context.CreateProxy<Order>();
    var product = context.CreateProxy<Product>(x => { x.Name = "Kapusta"; x.Price = 15;*//*, Id = 1 *//*});
    order.Products.Add(product);*/

    Console.WriteLine("Order przed dodaniem do kontekstu: " + context.Entry(order).State);
    Console.WriteLine("Product przed dodaniem do kontekstu: " + context.Entry(product).State);

    /*context.Attach(product);

    Console.WriteLine("Order po dodanu produktu do kontekstu: " + context.Entry(order).State);
    Console.WriteLine("Product po dodanu produktu do kontekstu: " + context.Entry(product).State);*/

    context.Attach(order);


    Console.WriteLine("Order po dodanu orderu do kontekstu: " + context.Entry(order).State);
    Console.WriteLine("Product po dodanu orderu do kontekstu: " + context.Entry(product).State);

    context.SaveChanges();

    Console.WriteLine("Order po dodanu do bazy danych: " + context.Entry(order).State);
    Console.WriteLine("Product po dodanu do bazy danych: " + context.Entry(product).State);

    order.DateTime = DateTime.Now;

    Console.WriteLine("Order po zmianie daty: " + context.Entry(order).State);
    Console.WriteLine("Product po zmianie daty: " + context.Entry(product).State);

    context.Remove(product);

    Console.WriteLine("Order po zgłoszeniu usunięcia produktu: " + context.Entry(order).State);
    Console.WriteLine("Product po zgłoszeniu usunięcia produktu: " + context.Entry(product).State);

    WriteDebugView(context);

    context.SaveChanges();

    Console.WriteLine("Order po wykonaniu na bazie danych: " + context.Entry(order).State);
    Console.WriteLine("Product po wykonaniu na bazie danych: " + context.Entry(product).State);

    product.Name = "";
    Console.WriteLine("Product po zmianie nazwy: " + context.Entry(product).State);


    for (int i = 0; i < 3; i++)
    {
        order = new Order() { DateTime = DateTime.Now.AddMinutes(-i * 64) };
        order.Products = new System.Collections.ObjectModel.ObservableCollection<Product>
            (Enumerable.Range(1, new Random(i).Next(2, 10))
            .Select(x => new Product { Name = x.ToString(), Price = x * 0.32f })
            .ToList());

        context.Add(order);
    }

    WriteDebugView(context);
    context.SaveChanges();
    WriteDebugView(context);

    order.DateTime = DateTime.Now;
    order.Products.First().Name = "samochodzik";
    WriteDebugView(context);

    //Ręcznie uruchomienie wykrywania zmian
    //context.ChangeTracker.DetectChanges();
    //WriteDebugView(context);


    var products = new List<Product>()
{
    new Product() { Name = "P1", Order = new Order{ Id = 60, DateTime = DateTime.Now } },
    new Product() { Name = "P2", Order = new Order{} }
};

    foreach (var p in products)
    {

        context.ChangeTracker.TrackGraph(p, x =>
        {
            if (x.Entry.IsKeySet)
                x.Entry.State = EntityState.Modified;
            else
                x.Entry.State = EntityState.Added;

            x.Entry.Properties.Where(x => x.Metadata.ClrType == typeof(DateTime)).ToList().ForEach(x => x.IsModified = false);
        });

        Console.WriteLine("Product: " + context.Entry(p).State);
        Console.WriteLine("Product order: " + context.Entry(p.Order).State);
    }

    WriteDebugView(context);





    static void WriteDebugView(Context context)
    {
        Console.WriteLine("-----");
        Console.WriteLine(context.ChangeTracker.DebugView.ShortView);
        Console.WriteLine("-----");
        Console.WriteLine(context.ChangeTracker.DebugView.LongView);
        Console.WriteLine("-----");
    }
}

static void ConcurrencyToken(DbContextOptions<Context> contextOptions)
{
    using (var context = new Context(contextOptions))
    {
        ChangeTracker(context);
    }

    using (var context = new Context(contextOptions))
    {

        var order = context.Set<Order>().First();

        order.DateTime = DateTime.Now;
        context.SaveChanges();

        var product = context.Set<Product>().First();
        product.Price = 15;

        var saved = false;

        do
        {
            try
            {
                context.SaveChanges();
                saved = true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    //wartosci jakie chcemy wprowadzić do bazy
                    var currentValues = entry.CurrentValues;
                    //wartości jakie pamięta context (jakie były pobrane z bazy)
                    var originalValues = entry.OriginalValues;
                    //wartości jakie są aktualnie w bazie danych
                    var databaseValues = entry.GetDatabaseValues();

                    switch (entry.Entity)
                    {
                        case Product:
                            var property = currentValues.Properties.Single(x => x.Name == nameof(Product.Price));
                            var currentValue = (float)currentValues[property];
                            var originalValue = (float)originalValues[property];
                            var databaseValue = (float)databaseValues[property];

                            currentValues[property] = databaseValue + (currentValue - originalValue);
                            break;
                    }

                    entry.OriginalValues.SetValues(databaseValues);
                }

            }
        } while (!saved);

    }
}

static void Transactions(DbContextOptions<Context> contextOptions, bool randomFail = true)
{
    var products = Enumerable.Range(100, 50).Select(x => new Product { Name = $"Product {x}", Price = 1.23f * x }).ToList();
    var orders = Enumerable.Range(0, 5).Select(x => new Order() { DateTime = DateTime.Now.AddMinutes(-3.21 * x) }).ToList();

    using (var context = new Context(contextOptions))
    {
        context.RandomFail = randomFail;

        using var transaction = context.Database.BeginTransaction();

        for (int i = 0; i < 5; i++)
        {
            string savepointName = i.ToString();
            transaction.CreateSavepoint(savepointName);

            try
            {
                var order = orders[i];
                context.Add(order);
                context.SaveChanges();

                var subProducts = products.Skip(i * 10).Take(10).ToList();

                foreach (var product in subProducts)
                {
                    product.Order = order;

                    context.Attach(product);
                    context.SaveChanges();
                }
            }
            catch
            {
                transaction.RollbackToSavepoint(savepointName);
                context.ChangeTracker.Clear();
            }
        }

        transaction.Commit();
    }


    using (var context = new Context(contextOptions))
    {
        using var transaction = context.Database.BeginTransaction();
        var product = context.Set<Product>().First();

        product.Name = "X";
        context.SaveChanges();
        transaction.Commit();
    }
}

static void Loading(DbContextOptions<Context> contextOptions)
{
    Transactions(contextOptions, false);

    using (var context = new Context(contextOptions))
    {
        //Eager loading
        var product = context.Set<Product>().Include(x => x.Order).ThenInclude(x => x.Products).First();
    }

    using (var context = new Context(contextOptions))
    {
        var product = context.Set<Product>().First();
        //Explicit loading
        context.Entry(product).Reference(x => x.Order).Load();
        context.Entry(product.Order).Collection(x => x.Products).Load();
    }

    using (var context = new Context(contextOptions))
    {
        context.Set<Product>().Load();
        context.Set<Order>().Load();

        var product = context.Set<Product>().Local.First();
    }

    Product p = null;
    using (var context = new Context(contextOptions))
    {
        p = context.Set<Product>().First();
        //Lazy loading
        Console.WriteLine(p.Order.DateTime);
    }
}

static void QueryFilter(DbContextOptions<Context> contextOptions)
{
    Transactions(contextOptions, false);

    using (var context = new Context(contextOptions))
    {
        var product = context.Set<Product>().First();

        //product.IsDeleted = true;
        context.Entry(product).Property<bool>("IsDeleted").CurrentValue = true;


        context.SaveChanges();

    }


    using (var context = new Context(contextOptions))
    {
        var product = context.Set<Product>()/*.Where(x => !x.IsDeleted)*/.First();
    }

    using (var context = new Context(contextOptions))
    {
        var order = context.Set<Order>().Include(x => x.Products/*.Where(x => !x.IsDeleted)*/).First();
    }

    using (var context = new Context(contextOptions))
    {
        context.Set<Product>().Load();
        var products = context.Set<Product>().Local.ToList();
    }
}

static void CompiledQuery(DbContextOptions<Context> contextOptions)
{
    Transactions(contextOptions, false);

    using (var context = new Context(contextOptions))
    {


        var orders = Context.GetOrdersByDateRange(context, DateTime.Now.AddDays(-1), DateTime.Now);

    }
    using (var context = new Context(contextOptions))
    {


        var orders = Context.GetOrdersByDateRange(context, DateTime.Now.AddDays(-1), DateTime.Now);

    }
}

static void Procedures(Context context)
{
    //var multiplier = "-1.1; DROP TABLE Products";
    var multiplier = "-1.1";
    //context.Database.ExecuteSqlRaw("EXEC ChangePrice @p0", multiplier);
    context.Database.ExecuteSqlInterpolated($"EXEC ChangePrice {multiplier}");


    var result = context.Set<OrderSummary>().FromSqlRaw("EXEC OrderSummary @p0", 3);
}