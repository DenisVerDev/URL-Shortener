using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddScoped<IPostsRepository, PostsRepository>();

builder.Services.AddScoped<IURLShortenAlgorithm, URLShortenSHA256>();

builder.Services.AddScoped<IUserVerificationService, UserVerificationService>();
builder.Services.AddScoped<IUsersViewingService, UsersViewingService>();
builder.Services.AddScoped<IUserManagementService, UserManagementService>();
builder.Services.AddScoped<IUserSessionService, UserSessionService>();
builder.Services.AddScoped<IURLsManagementService, URLsManagementService>();
builder.Services.AddScoped<IURLsViewingService, URLsViewingService>();
builder.Services.AddScoped<IPostsViewingService, PostsViewingService>();
builder.Services.AddScoped<IPostsManagementService, PostsManagementService>();

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

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionFeature?.Error;

        var traceId = context.TraceIdentifier;

        logger.LogError(exception, "Unhandled exception. TraceId: {TraceId}, Path: {Path}", traceId, context.Request.Path);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred.",
            Detail = "Contact support with the traceId.",
            Extensions =
            {
                ["traceId"] = traceId
            }
        });
    });
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
