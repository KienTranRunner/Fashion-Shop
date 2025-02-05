using Amazon.S3;
using fShop.Models;
using fShop.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<fShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;  
    options.Cookie.IsEssential = true;  
});




// Thêm CartService
builder.Services.AddScoped<CartService>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/SignIn/Index"; 
        options.AccessDeniedPath = "/Home/AccessDenied";
    });

builder.Services.AddAuthorization();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(); 



builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); 

app.UseAuthorization();

app.UseSession(); 

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
