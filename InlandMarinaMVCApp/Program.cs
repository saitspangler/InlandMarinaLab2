using Microsoft.EntityFrameworkCore;
using InlandMarinaData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//get the connection string from appsettings.json

builder.Services.AddDbContext<InlandMarinaContext>(options => options.UseSqlServer(
builder.Configuration.GetConnectionString("InlandMarinaContext")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
