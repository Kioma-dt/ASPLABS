using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web_453503_Avramenko.UI.HelperClasses;
using Web_453503_Avramenko.UI.Models;
using Web_453503_Avramenko.UI.Services.Authentication;
using Web_453503_Avramenko.UI.Services.FileService;

namespace Web_453503_Avramenko.UI.Controllers;

public class AccountController(
    IHttpContextAccessor httpContextAccessor,
    HttpClient httpClient,
    ITokenAccessor tokenAccessor,
    IOptions<KeycloakData> options,
    IFileService fileService)
    : Controller
{
    public async Task Login()
    {
        await HttpContext.ChallengeAsync(
            "keycloak",
            new AuthenticationProperties { RedirectUri = Url.Action("Index", "Home") });
    }
    
    [HttpPost]
    public async Task Logout()
    {
        await
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync("keycloak",
            new AuthenticationProperties { RedirectUri = Url.Action("Index", "Home")
            });
    }
    
    public IActionResult Register()
    {
        return View(new RegisterUserViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserViewModel user)
    {
        if (ModelState.IsValid)
        {
            if (user is null)
            {
                return BadRequest();
            }

            try
            {
                await tokenAccessor.SetAuthorizationHeaderAsync(httpClient, true);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
            
            var avatarUrl = "/images/default-profile-picture.jpeg";

            if (user.Avatar is not null)
            {
                avatarUrl = await fileService.SaveFileAsync(user.Avatar);
            }

            var newUser = new CreateUserModel();
            newUser.Attributes.Add("avatar", avatarUrl);
            newUser.Email = user.Email;
            newUser.Username = user.Email;
            newUser.Credentials.Add(new UserCredentials(){Value = user.Password});
            
            var requestUri =
                $"{options.Value.Host}/admin/realms/{options.Value.Realm}/users";
            
            var serializationOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            
            var json = JsonSerializer.Serialize(newUser, serializationOptions);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await httpClient.PostAsync(requestUri, content);
            
            if (response.IsSuccessStatusCode)
            {
                return Redirect(Url.Action("Index", "Home"));
            }
            else return BadRequest(response.StatusCode);
        }
        return View(user);
    }
    
    class CreateUserModel
    {
        public Dictionary<string, string> Attributes { get; set; } = new ();
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Enabled { get; set; } =  true;
        public bool EmailVerified { get; set; } = true;
        public List<UserCredentials> Credentials { get; set; } = new();
    }
    class UserCredentials
    {
        public string Type { get; set; } = "password";
        public bool Temporary { get; set; } = false;
        public string Value { get; set; }
    }
}