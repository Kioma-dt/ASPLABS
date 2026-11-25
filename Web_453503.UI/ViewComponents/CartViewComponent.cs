using Microsoft.AspNetCore.Mvc;
using Web_453503.Domain.Cart;
using Web_453503.UI.Models;

namespace Web_453503.UI.ViewComponents;

public class CartViewComponent
    : ViewComponent
{
    private readonly Cart _cart;
    
    public CartViewComponent(Cart cart)
    {
        _cart = cart;
    }
    
    public IViewComponentResult Invoke()
    {
        var viewModel = new CartInfoViewModel()
        {
            Count = _cart.Count,
            TotalWeight = Math.Round(_cart.TotalWeight, 3)
        };
        return View(viewModel);
    }
}