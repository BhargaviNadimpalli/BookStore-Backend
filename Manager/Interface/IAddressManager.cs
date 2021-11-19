using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Interface
{
   public interface IAddressManager
    {
        public bool AddAddress(AddressModel addressDetails);

        public bool EditAddress(AddressModel addressDetails);

        public List<AddressModel> GetUserAddress(int userId);

        public bool RemoveUserAddress(int addressId);
    }
}
