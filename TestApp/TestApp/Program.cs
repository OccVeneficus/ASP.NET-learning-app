using TestApp.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEntityFrameworkSqlite().AddDbContext<TreeDbContext>();
using (var client = new TreeDbContext(builder.Configuration))
{
    var isCreated = client.Database.EnsureCreated();
    if (isCreated)
    {
        client.SeedDb();
    }
}
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
