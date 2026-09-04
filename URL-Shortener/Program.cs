using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data;
using URL_Shortener.Data.Repositories;
using URL_Shortener.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<UShortDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb")));
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IURLsRepository, URLsRepository>();

builder.Services.AddScoped<IUserVerificationService, UserVerificationService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IUserSessionService, UserSessionService>();
builder.Services.AddScoped<IURLsManagementService, URLsManagementService>();
builder.Services.AddScoped<IURLsViewingService, URLsViewingService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.LoginPath = "/Login";
                    opt.LogoutPath = "/Logout";
                    opt.AccessDeniedPath = "/";
                    
                    opt.SlidingExpiration = true;
                    opt.ExpireTimeSpan = TimeSpan.FromMinutes(30);

                    opt.Cookie.Name = "UrlShortenerAuthCookie";
                    opt.Cookie.SameSite = SameSiteMode.Lax; // for now
                    opt.Cookie.HttpOnly = true;
                    opt.Cookie.IsEssential = true;
                });

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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
