using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Web_453503_Avramenko.UI;
using Web_453503_Avramenko.UI.Extensions;
using Web_453503_Avramenko.UI.HelperClasses;
using Web_453503_Avramenko.UI.Services.Authentication;
using Web_453503_Avramenko.UI.Services.FileService;
using Web_453503_Avramenko.UI.Services.PetService;


var builder = WebApplication.CreateBuilder(args);

var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

//! builder.RegisterCustomServices();

builder.Services.AddHttpClient<IPetService, ApiPetService>(opt
    => opt.BaseAddress = new Uri(uriData?.ApiUri+"pets/"));
builder.Services.AddHttpClient<ISpeciesService, ApiSpeciesService>(opt 
    => opt.BaseAddress = new Uri(uriData?.ApiUri+"species/"));

builder.Services
    .Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));

var keycloakData = builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "keycloak";
    })
    .AddCookie()
    .AddOpenIdConnect("keycloak", options =>
    {
        options.Authority = $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
        options.ClientId = keycloakData.ClientId;
        options.ClientSecret = keycloakData.ClientSecret;
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.Scope.Add("openid");
        options.SaveTokens = true;
        options.RequireHttpsMetadata = false; 
        options.MetadataAddress =
            $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
    });

builder.Services.AddAuthorization(opt =>
    opt.AddPolicy("admin", p => p.RequireRole("POWER-USER")));

builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();

builder.Services.AddScoped<IFileService, LocalFileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .RequireAuthorization("admin");;

app.Run();