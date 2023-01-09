using Microsoft.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddImageSharp(
    options =>
    {
        options.Configuration = Configuration.Default;
        options.MemoryStreamManager = new RecyclableMemoryStreamManager();
        options.BrowserMaxAge = TimeSpan.FromHours(12);
        options.CacheMaxAge = TimeSpan.FromDays(30);
        options.CacheHashLength = 8;
    }).Configure<PhysicalFileSystemCacheOptions>(options =>
{
    options.CacheFolder = "imagecache";
});


var app = builder.Build();

app.UseImageSharp();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();