using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SMS.Data;
using SMS.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SMSContextConnection") ?? throw new InvalidOperationException("Connection string 'SMSContextConnection' not found.");

builder.Services.AddDbContext<SMSContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<SMSUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<SMSContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
