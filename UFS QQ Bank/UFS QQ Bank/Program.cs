using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UFS_QQ_Bank.Data;
using UFS_QQ_Bank.Infrastructure;
using UFS_QQ_Bank.Models;
using UFS_QQ_Bank.Models.ViewModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<IBankAccountRepository, IBankAccountRepository>();
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IPasswordValidator<IdentityUser>, CustomPasswordValidator>();
builder.Services.AddScoped<IUserValidator<IdentityUser>, CustomUserValidator>();
builder.Services.AddScoped<AccountNumberServiceModel>();
//builder.Services.AddDbContext<AppEntityDbContext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<AppEntityDbContext>(options =>options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
var CONSTRING = builder.Configuration.GetConnectionString("DefaultConnection");

//builder.Services.AddDbContext<AppIdentityDbContext>(options =>options.UseSqlite(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddDbContext<AppEntityDbContext>(opts =>
opts.UseSqlServer(CONSTRING, opts =>
{
    opts.EnableRetryOnFailure();
    opts.CommandTimeout(120);
    opts.UseCompatibilityLevel(110);
}));

builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireDigit = false;
    opts.User.RequireUniqueEmail = true;
    
   // opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
}
).AddEntityFrameworkStores<AppEntityDbContext>();



var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

SeedData.EnsurePopulated(app);
//SeedIdentityUser.EnsurePopulated(app);


app.Run();
/* to do thing
 * Account Management(check balance, transfare more, view transaction history and deposit money)
 * notification for user action
 *  drop-database -context AppIdentityDbContext
 * drop-database -context AppEntityDbContext
 * remove-migration -context AppIdentityDbContext
 * remove-migration -context AppEntityDbContext
 * add-migration AppInitialDb -context AppEntityDbContext
 * add-migration AppIdentityDb -context AppIdentityDbContext
 */