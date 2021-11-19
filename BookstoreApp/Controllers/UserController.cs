using Manager.Interface;
using Microsoft.AspNetCore.Mvc;
using Model;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserManager manager;

        public UserController(IUserManager manager)
        {
            this.manager = manager;

        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterModel userDetails)
        {
            try
            {
                var result = this.manager.Register(userDetails);
                if (result)
                {

                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Added New User Successfully !" });
                }
                else
                {

                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Failed to add new user, Try again" });
                }
            }
            catch (Exception ex)
            {

                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });

            }
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
              try
                {
                   
                    string result = this.manager.Login(login);
                    if (result.Equals("Login is Successfull"))
                    {
                          ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                        IDatabase database = connectionMultiplexer.GetDatabase();

                        Dictionary<string, string> data = new Dictionary<string, string>();
                        data.Add("_Id", database.StringGet("userID"));
                        data.Add("FullName", database.StringGet("FullName"));
                        data.Add("PhoneNumber", database.StringGet("PhoneNumber"));
                        data.Add("Email", login.Email);
                        data.Add("accessToken", this.manager.GenerateToken(login.Email));
                        return this.Ok(new { Status = true, Message = result, result = data });
                    }
                    else if (result.Equals("Invalid Password"))
                    {
                        return this.BadRequest(new ResponseModel<string>() { Status = false, Message = result });
                    }
                    else
                    {
                       
                        return this.BadRequest(new ResponseModel<string>() { Status = false, Message = result });
                    }
                }
                catch (Exception ex)
                {
                   
                    return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
                }
            }
        [HttpPost]
        [Route("forgetPassword")]
        public IActionResult ForgetPassword(string Email)
        {
            try
            {
                var result = this.manager.ForgetPassword(Email);

                if (result.CustomerId > 0)
                {

                    ////Creates a OkResult object that produces an empty Status200OK response.
                    return this.Ok(new ResponseModel<DataResponseModel>() { Status = true, Message = result.message, Data = result });
                }
                else
                {
                    ////Creates an BadRequestResult that produces a Status400BadRequest response.
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = result.message });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut]
        [Route("resetpassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel resetPassword)
        {
            var result = this.manager.ResetPassword(resetPassword);
            try
            {
                if (result)
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Successfully changed password !" });

                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Try again !" });
                }
            }
            catch (Exception e)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = e.Message });
            }

        }
    }
}

