using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

string basePath = AppDomain.CurrentDomain.BaseDirectory;
string dbPath = $"{basePath}Roulette.db";

builder.Services.AddDbContext<RouletteDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
