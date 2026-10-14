using Web_453503_Avramenko.Domain.Entities;

namespace Web_453503_Avramenko.Domain.Cart;

public class Cart
{
    public Dictionary<Guid, CartItem> CartItems { get; set; } = new();

    public virtual void AddToCart(Pet pet)
    {
        if (CartItems.ContainsKey(pet.Id))
        {
            CartItems[pet.Id].Quantity++;
        }
        else
        {
            CartItems[pet.Id] = new CartItem
            {
                Pet = pet,
                Quantity = 1
            };
        }
    }

    public virtual void RemoveFromCart(Guid id)
    {
        CartItems.Remove(id);
    }

    public virtual void ClearAll()
    {
        CartItems.Clear();
    }
    
    public int Count => CartItems.Sum(c => c.Value.Quantity);
    public double TotalWeight => CartItems.Sum(c => c.Value.Quantity * c.Value.Pet.Weight);
}