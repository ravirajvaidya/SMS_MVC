using Microsoft.EntityFrameworkCore;
using SMS_MVC.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Adding the DB context service
builder.Services.AddDbContext<SMSDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Accounts}/{action=Login}/{id?}")
    pattern: "{controller=Classes}/{action=PageAddClass}/{id?}")
    .WithStaticAssets();


app.Run();
