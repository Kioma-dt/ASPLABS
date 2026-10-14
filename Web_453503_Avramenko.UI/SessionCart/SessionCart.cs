using Microsoft.EntityFrameworkCore.Diagnostics;
using Web_453503_Avramenko.Domain.Cart;

namespace Web_453503_Avramenko.UI.SessionCart;

public class SessionCart
    : Cart
{
    private readonly IHttpContextAccessor _accessor;

    public SessionCart(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }
    
    public override void AddToCart(Pet pet)
    {
        base.AddToCart(pet);
        Save();
    }

    public override void ClearAll()
    {
        base.ClearAll();
        Save();
    }

    public override void RemoveFromCart(Guid id)
    {
        base.RemoveFromCart(id);
        Save();
    }

    public static SessionCart GetCart(IServiceProvider provider)
    {
        var accessor = provider.GetRequiredService<IHttpContextAccessor>();

        var session = accessor.HttpContext?.Session
                      ?? throw new InvalidOperationException(
                          "Session is unavailable.");
        var items = session.Get<Dictionary<Guid, CartItem>>("cart");
        
        var cart = new SessionCart(accessor);

        if (items is not null)
        {
            cart.CartItems = items;
        }
        
        return  cart;
    }

    private void Save()
    {
        _accessor.HttpContext.Session.Set("cart", this.CartItems);
    }
}