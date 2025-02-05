using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace fShop.Models.ViewModels
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<CartItem> GetCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string cartJson = session.GetString(CartSessionKey);
            return cartJson == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);
        }

        public void SaveCart(List<CartItem> cart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            string cartJson = JsonConvert.SerializeObject(cart);
            session.SetString(CartSessionKey, cartJson);
        }

        public void AddToCart(CartItem item)
        {
            var cart = GetCart();
            var existingItem = cart.Find(x => x.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }
            SaveCart(cart);
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            cart.RemoveAll(x => x.ProductId == productId);
            SaveCart(cart);
        }

        public void ClearCart()
        {
            SaveCart(new List<CartItem>());
        }
    }
}