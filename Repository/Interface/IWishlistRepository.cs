using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interface
{
   public interface IWishlistRepository
    {
        public bool AddToWishList(WishlistModel wishListmodel);

        public bool RemoveFromWishList(int wishListId);

        public List<WishlistModel> GetWishList(int userId);

    }
}
