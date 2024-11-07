using E_CommerceApi.Models;

namespace E_CommerceApi.DTOs
{
    public class ShoppingCart
    {
        public double totalCarts { get; set; }
        public List<Cart>carts { get; set; }
    }
}
