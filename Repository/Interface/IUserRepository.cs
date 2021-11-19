using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interface
{
    public interface IUserRepository
    {
        public bool Register(RegisterModel userDetails);

        public string Login(LoginModel login);


        public string GenerateToken(string userName);

        public DataResponseModel ForgetPassword(string Email);

        public bool ResetPassword(ResetPasswordModel resetPassword);

    }
}
