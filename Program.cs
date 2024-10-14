using EntityFrameworkCoreCRUD.Data;
using EntityFrameworkCoreCRUD.Service;
using EntityFrameworkCoreCRUD.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
/*using EntityFrameworkCoreCRUD.Services;
*/
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<BuyerService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<CarService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderDetailService>();


// Add services to the container.
builder.Services.AddControllersWithViews();
// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
// add
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
