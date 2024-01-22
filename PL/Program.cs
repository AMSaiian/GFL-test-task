using DAL.Context;
using Microsoft.EntityFrameworkCore;
using PL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TreeContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Local")));

builder.Services.AddServices();

builder.Services.AddControllersWithViews();

var app = builder.Build();

await app.EnsureCreateDb();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Config}/{action=Index}/{id?}");

app.Run();