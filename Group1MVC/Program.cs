using BusinessServiceLayer.Interfaces;
using BusinessServiceLayer.Services;
using Group1MVC.Extensions;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IReportService, ReportService>();


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
    pattern: "{controller=Categories}/{action=Index}/{id?}");



// Create a scope and call the service manually
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var dbContext = services.GetRequiredService<FuNewsManagementContext>();
var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    // Migrate changes to the database
    await dbContext.Database.MigrateAsync();

    // Seed the database with data
    await FuNewsManagementContextSeed.SeedAsync(dbContext);
}
catch (Exception ex)
{
    logger.LogError(ex, "A message occured during migration");
}

app.Run();
