using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using URL_Shortener.Data;
using URL_Shortener.Data.Repositories;
using URL_Shortener.Services;
using URL_Shortener.Services.ShorteningAlgorithms;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<UShortDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb")));
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IURLsRepository, URLsRepository>();

builder.Services.AddScoped<IURLShortenAlgorithm, URLShortenSHA256>();

builder.Services.AddScoped<IUserVerificationService, UserVerificationService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IUserSessionService, UserSessionService>();
builder.Services.AddScoped<IURLsManagementService, URLsManagementService>();
builder.Services.AddScoped<IURLsViewingService, URLsViewingService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opt =>
                {
                    opt.LoginPath = builder.Configuration["Cookies:AuthCookie:LoginPath"];
                    opt.LogoutPath = builder.Configuration["Cookies:AuthCookie:LogoutPath"];
                    opt.AccessDeniedPath = builder.Configuration["Cookies:AuthCookie:AccessDeniedPath"];

                    opt.SlidingExpiration = builder.Configuration.GetValue<bool>("Cookies:AuthCookie:SlidingExpiration");
                    opt.ExpireTimeSpan = TimeSpan.FromMinutes(builder.Configuration
                                                  .GetValue<int>("Cookies:AuthCookie:ExpireTimeSpanInMinutes"));

                    opt.Cookie.Name = builder.Configuration["Cookies:AuthCookie:BrowserName"];
                    opt.Cookie.SameSite = builder.Configuration.GetValue<SameSiteMode>("Cookies:AuthCookie:SameSite");
                    opt.Cookie.HttpOnly = builder.Configuration.GetValue<bool>("Cookies:AuthCookie:HttpOnly");
                    opt.Cookie.IsEssential = builder.Configuration.GetValue<bool>("Cookies:AuthCookie:IsEssential");
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
