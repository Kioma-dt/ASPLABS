using Web_453503_Avramenko.Domain.Entities;

namespace Web_453503_Avramenko.Domain.Cart;

public class CartItem
{
    public Pet? Pet { get; set; }
    public int Quantity { get; set; }
}