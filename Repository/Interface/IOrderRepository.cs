using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interface
{
   public interface IOrderRepository
    {
        public bool AddOrder(List<CartModel> orderdetails);

        public List<OrderModel> GetOrderList(int userId);
    }
}
