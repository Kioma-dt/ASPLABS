using Web_453503.Domain.Entities;

namespace Web_453503.Domain.Cart;

public class CartItem
{
    public Pet? Pet { get; set; }
    public int Quantity { get; set; }
}