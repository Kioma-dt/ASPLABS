using Microsoft.EntityFrameworkCore;
using Web_453503_Avramenko.API.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connStr = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connStr));

builder.Services.AddControllers();

var app = builder.Build();

await DbInitializer.SeedData(app);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();

app.Run();