using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Interface
{
   public interface IWishlistManager
    {
        public bool AddToWishList(WishlistModel wishListmodel);

        public bool RemoveFromWishList(int wishListId);

        public List<WishlistModel> GetWishList(int userId);
    }
}
