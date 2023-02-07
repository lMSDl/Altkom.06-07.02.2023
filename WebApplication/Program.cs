using DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<Context>(x => x.UseSqlServer(), 32);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
