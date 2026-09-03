using Microsoft.AspNetCore.Mvc;

namespace Web_453503_Avramenko.ViewComponents;

public class CartViewComponent
    : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}