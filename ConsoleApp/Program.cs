

using DAL;
using Microsoft.EntityFrameworkCore;
using Models;

var contextOptions = new DbContextOptionsBuilder<Context>()
                        .UseSqlServer(@"Server=(local)\SQLEXPRESS;Database=EFCore;Integrated Security=true")
                        //Włączenie śledzenia zmian na podstawie proxy - wymaga specjalnego tworzenia obiektów (context.CreateProxy) i virtualizacji właściwości encji
                        //.UseChangeTrackingProxies()
                        .Options;

var context = new Context(contextOptions);
context.Database.EnsureDeleted();
context.Database.EnsureCreated();

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