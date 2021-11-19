using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Interface
{
   public interface ICartManager
    {
        public bool AddToCart(CartModel details);

        public List<CartModel> GetCart(int userId);

        public bool UpdateCart(CartModel details);

        public bool DeleteCart(int cartId);


    }
}
