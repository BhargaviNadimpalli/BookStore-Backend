using Manager.Interface;
using Model;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository repository;
        public UserManager(IUserRepository repository)
        {
            this.repository = repository;
        }


        public bool Register(RegisterModel RegisterDetails)
        {
            try
            {
                return this.repository.Register(RegisterDetails);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string Login(LoginModel login)
        {
            try
            {
                return this.repository.Login(login);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public string GenerateToken(string userName)
        {
            try
            {
               return this.repository.GenerateToken(userName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataResponseModel ForgetPassword(string Email)
        {
            try
            {
                return this.repository.ForgetPassword(Email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ResetPassword(ResetPasswordModel resetPassword)
        {
            try
            {
                return this.repository.ResetPassword(resetPassword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
