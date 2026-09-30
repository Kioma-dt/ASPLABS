using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Web_453503_Avramenko.API.Data;
using Web_453503_Avramenko.API;
using Web_453503_Avramenko.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connStr = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connStr));

builder.Services.AddMediatR(conf =>
    conf.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddControllers();

var authServer = builder.Configuration
    .GetSection("AuthServer")
    .Get<AuthServerData>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
    {
        o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";

        o.Authority = $"{authServer.Host}/realms/{authServer.Realm}";

        o.Audience = "account";
        o.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
});

var app = builder.Build();

//! await DbInitializer.SeedData(app);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();
app.MapIEndpoints();

app.MapPetEndpoints();

app.Run();