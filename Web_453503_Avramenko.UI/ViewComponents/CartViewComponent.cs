using Microsoft.AspNetCore.Mvc;
using Web_453503_Avramenko.Domain.Cart;
using Web_453503_Avramenko.UI.Models;

namespace Web_453503_Avramenko.UI.ViewComponents;

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
            TotalWeight = _cart.TotalWeight
        };
        return View(viewModel);
    }
}