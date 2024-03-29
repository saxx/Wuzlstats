using Microsoft.EntityFrameworkCore;
using Wuzlstats;
using Wuzlstats.Hubs;
using Wuzlstats.Models;
using Wuzlstats.ViewModels.Hubs;
using Wuzlstats.ViewModels.Home;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddSingleton<AppSettings>();
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(opts =>
{
    opts.EnableDetailedErrors = true;
}).AddNewtonsoftJsonProtocol();
builder.Services.AddTransient<ReloadPlayersViewModel>();
builder.Services.AddTransient<GamesViewModel>();
builder.Services.AddTransient<LeagueStatisticsViewModel>();
builder.Services.AddDbContext<Db>(opts =>
{
    var config = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();

    var connectionString = config.GetConnectionString("statsdb");
    if (connectionString != null && connectionString.Contains("%LOCALAPPDATA%"))
    {
        connectionString = connectionString.Replace("%LOCALAPPDATA%",
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
    }
    opts.UseSqlite(connectionString);
});

builder.Services.AddWebOptimizer(pipeline =>
{
    var sourceFiles = new[] {
        "js/app.autoPageReload.js",
        "js/app.config.js",
        "js/app.leagueMenuBar.js",
        "js/app.leagueStatistics.js",
        "js/app.playerStatistics.js",
        "js/app.score.js",
        "js/app.signalr.js",
        "js/app.teamStatistics.js",
    };
    pipeline.AddJavaScriptBundle("/js/app.bundle.js", sourceFiles);
    pipeline.AddFiles("text/css", "/css/*");
}, options =>
{
    options.EnableCaching = false;
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
app.UseWebOptimizer();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.MapHub<ApiHub>("/apiHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await UpdateDatabase(app);
app.Run();

async Task UpdateDatabase(IHost host)
{
    await using var scope = host.Services.CreateAsyncScope();
    var ctx = scope.ServiceProvider.GetRequiredService<Db>();
    await ctx.Database.MigrateAsync();
}
