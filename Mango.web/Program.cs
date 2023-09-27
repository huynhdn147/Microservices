using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<ICouponservice, Couponservice>();
builder.Services.AddHttpClient<IAuthservice, Authservice>();
builder.Services.AddHttpClient<IProductservice, ProductService>();
builder.Services.AddHttpClient<ICartservice, Cartservice>();
DS.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"];
DS.AuthAPIBase = builder.Configuration["ServiceUrls:AuthAPI"];
DS.ProductAPIBase = builder.Configuration["ServiceUrls:ProductAPI"];

DS.ShoppingCartAPIBase = builder.Configuration["ServiceUrls:ShoppingCartAPI"];
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<ICouponservice, Couponservice>();
builder.Services.AddScoped<IProductservice, ProductService>();
builder.Services.AddScoped<IAuthservice,Authservice>();
builder.Services.AddScoped<ICartservice, Cartservice>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.ExpireTimeSpan = TimeSpan.FromHours(10);
        option.LoginPath = "/Auth/Login";
        option.AccessDeniedPath = "/Auth/AccessDenied";
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
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
