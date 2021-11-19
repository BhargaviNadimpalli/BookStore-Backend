using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interface
{
   public interface ICartRepository
    {
       public bool AddToCart(CartModel details);

       public List<CartModel> GetCart(int userId);

        public bool UpdateCart(CartModel details);     

        public bool DeleteCart(int cartId);

    }
}
