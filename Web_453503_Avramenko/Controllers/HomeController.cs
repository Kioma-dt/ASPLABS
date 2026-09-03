using Microsoft.AspNetCore.Mvc;

namespace Web_453503_Avramenko.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}