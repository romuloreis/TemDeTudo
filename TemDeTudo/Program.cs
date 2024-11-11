using Microsoft.EntityFrameworkCore;
using TemDeTudo.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TemDeTudoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TemDeTudoContext") ?? throw new InvalidOperationException("Connection string 'TemDeTudoContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

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
