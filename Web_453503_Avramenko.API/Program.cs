using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Web_453503_Avramenko.API.Data;
using Web_453503_Avramenko.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connStr = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connStr));

builder.Services.AddMediatR(conf =>
    conf.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddControllers();

var app = builder.Build();

//! await DbInitializer.SeedData(app);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();
app.MapIEndpoints();

app.MapPetEndpoints();

app.Run();