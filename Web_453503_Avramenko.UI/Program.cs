using Web_453503_Avramenko.UI;
using Web_453503_Avramenko.UI.Extensions;
using Web_453503_Avramenko.UI.Services.PetService;


var builder = WebApplication.CreateBuilder(args);

var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//! builder.RegisterCustomServices();

builder.Services.AddHttpClient<IPetService, ApiPetService>(opt
    => opt.BaseAddress = new Uri(uriData?.ApiUri+"pet"));
builder.Services.AddHttpClient<ISpeciesService, ApiSpeciesService>(opt 
    => opt.BaseAddress = new Uri(uriData?.ApiUri+"species"));

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


app.Run();