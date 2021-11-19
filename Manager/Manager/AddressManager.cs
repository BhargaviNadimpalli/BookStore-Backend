using Manager.Interface;
using Model;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Manager
{
    public class AddressManager : IAddressManager
    {
        private readonly IAddressRepository repository;
        public AddressManager(IAddressRepository repository)
        {
            this.repository = repository;
        }
        public bool AddAddress(AddressModel addressDetails)
        {
            try
            {
                return this.repository.AddAddress(addressDetails);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool EditAddress(AddressModel addressDetails)
        {
            try
            {
                return this.repository.EditAddress(addressDetails);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<AddressModel> GetUserAddress(int userId)
        {
            try
            {
                return this.repository.GetUserAddress(userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool RemoveUserAddress(int addressId)
        {
            try
            {
                return this.repository.RemoveUserAddress(addressId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
