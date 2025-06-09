using Microsoft.EntityFrameworkCore;
using Serilog;
using ShipeX2.Application.Interfaces;
using ShipeX2.Application.Wrappers;
using ShipeX2.Identity.Context;
using ShipeX2.Identity.Services;
using static ShipeX2.Identity.Context.ApplicationDbContext;

var builder = WebApplication.CreateBuilder(args);

//Serilog Configuration 
builder.Host.UseSerilog(( context, services, configuration ) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});



// For PostgreSQL connnection string 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb")));
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
    options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Configure Cookie-based Authentication
builder.Services.AddAuthentication("auth_token").AddCookie("auth_token", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "auth_token";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".ShipX.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = "ShipX.AntiForgery";
});
builder.Host.UseSerilog();



// Add Scoped Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddScoped<IDynamicDbContextFactory, DynamicDbContextFactory>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<ICarrierListServices, CarrierServices>();
builder.Services.AddScoped<CurrentUser>();



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
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
