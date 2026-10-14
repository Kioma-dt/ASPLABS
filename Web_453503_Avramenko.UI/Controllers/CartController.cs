using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_453503_Avramenko.Domain.Cart;
using Web_453503_Avramenko.UI.Services.PetService;

namespace Web_453503_Avramenko.UI.Controllers;

public class CartController
    : Controller
{
    private readonly IPetService _petService;
    private readonly Cart _cart;

    public CartController(
        IPetService petService,
        Cart cart)
    {
        _petService = petService;
        _cart = cart;
    }

    public IActionResult Index()
    {
        return View(_cart);
    }
    
    [Authorize]
    [Route("[controller]/add/{id:guid}")]
    public async Task<ActionResult> Add(Guid id, string returnUrl)
    {
        var data = await _petService.GetPetByIdAsync(id);
        if (data.IsSuccessfully)
        {
            _cart.AddToCart(data.Data);
        }
        
        return Redirect(returnUrl);
    }

    [Authorize]
    [Route("[controller]/remove/{id:guid}")]
    public IActionResult Remove(Guid id)
    {
        _cart.RemoveFromCart(id);
        return RedirectToAction(nameof(Index));
    }
}