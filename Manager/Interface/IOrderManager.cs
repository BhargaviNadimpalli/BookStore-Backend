using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Interface
{
    public interface IOrderManager
    {
        public bool AddOrder(List<CartModel> orderdetails);
    }
}
