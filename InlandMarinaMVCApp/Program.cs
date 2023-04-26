using Microsoft.EntityFrameworkCore;
using InlandMarinaData;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(opt => opt.LoginPath = "/Account/Login"); // add to use cookies authentication
                                                                    // login page: Account controller, Login method

builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(90);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
}); // add before AddControllersWithViews to use session state object

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

app.UseAuthentication();

app.UseAuthorization();

app.UseSession(); // add before MapControllerRoute to use session state object

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
