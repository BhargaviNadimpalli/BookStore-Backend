using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model;
using Repository.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;      
using System.Text;

namespace Repository.Repository
{
    public class UserRepository : IUserRepository
    {
        public IConfiguration Configuration { get; }
        public UserRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        SqlConnection sqlConnection;
        public bool Register(RegisterModel userDetails)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));

            try
            {
                using (sqlConnection)
                {
                    string storeprocedure = "SpUserRegisteration";
                    SqlCommand sqlCommand = new SqlCommand(storeprocedure, sqlConnection);
                    userDetails.Password = EncryptPassword(userDetails.Password);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@FullName", userDetails.CustomerName);
                    sqlCommand.Parameters.AddWithValue("@Email", userDetails.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", userDetails.Password);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", userDetails.PhoneNumber);
                    sqlCommand.Parameters.Add("@user", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();

                    var result = sqlCommand.Parameters["@user"].Value;
                    if (!(result is DBNull))
                    {
                        userDetails.CustomerId = Convert.ToInt32(result);
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        public string EncryptPassword(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        public void GetUserDetails(string email)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));

            try
            {
                using (sqlConnection)
                {
                    string storeprocedure = "SELECT * FROM Users WHERE Email = @Email";
                    SqlCommand sqlCommand = new SqlCommand(storeprocedure, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@Email", email);
                    sqlConnection.Open();

                    RegisterModel registerModel = new RegisterModel();
                    SqlDataReader sqlData = sqlCommand.ExecuteReader();
                    if (sqlData.Read())
                    {
                        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                        IDatabase database = connectionMultiplexer.GetDatabase();
                        database.StringSet(key: "FullName", sqlData.GetString("FullName"));
                        database.StringSet(key: "PhoneNumber", sqlData.GetString("PhoneNumber"));
                        database.StringSet(key: "userID", sqlData.GetInt32("_Id").ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }


        public string Login(LoginModel login)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));

            try
            {
                using (sqlConnection)
                {
                    string storeprocedure = "StoredUserLogin";
                    SqlCommand sqlCommand = new SqlCommand(storeprocedure, sqlConnection);
                    login.Password = EncryptPassword(login.Password);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@Email", login.Email);
                    sqlCommand.Parameters.AddWithValue("@Password", login.Password);

                    sqlCommand.Parameters.Add("@user", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();

                    var result = sqlCommand.Parameters["@user"].Value;
                    if (!(result is DBNull))
                    {
                        if (Convert.ToInt32(result) == 2)
                        {
                            GetUserDetails(login.Email);
                            return "Login is Successfull";
                        }
                        return "Incorrect email or password";
                    }
                    return "Login failed";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public string GenerateToken(string userName)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(this.Configuration["SecretKey"]);
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                       new Claim(ClaimTypes.Name, userName)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                };
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
                return handler.WriteToken(token);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
        }

        public DataResponseModel ForgetPassword(string Email)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));

            try
            {

                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[EmailValidity]", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.Add("@user", SqlDbType.Int);
                    cmd.Parameters["@user"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@userId", SqlDbType.Int);
                    cmd.Parameters["@userId"].Direction = ParameterDirection.Output;
                    sqlConnection.Open();
                    cmd.ExecuteNonQuery();
                    var result = cmd.Parameters["@user"].Value;

                    if (result != null && result.Equals(1))
                    {
                        var userId = Convert.ToInt32(cmd.Parameters["@userId"].Value);
                        Random random = new Random();
                        int OTP = random.Next(1000, 9999);
                        this.MSMQSend(OTP);
                        if (this.SendEmail(Email))
                        {
                            return new DataResponseModel() { CustomerId = userId, message = "Otp is send to Email", otp = OTP };
                        }
                        else
                        {
                            return new DataResponseModel() { message = "Failed to sent email" };
                        }
                    }
                    else
                    {
                        return new DataResponseModel() { message = "Invalid emailid" };

                    }
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private MessageQueue QueueDetail()
        {
            MessageQueue messageQueue;
            if (MessageQueue.Exists(@".\Private$\ResetPasswordQueue"))
            {
                messageQueue = new MessageQueue(@".\Private$\ResetPasswordQueue");
            }
            else
            {
                messageQueue = MessageQueue.Create(@".\Private$\ResetPasswordQueue");
            }

            return messageQueue;
        }
        private bool SendEmail(string email)
        {
            string linkToBeSend = this.ReceiveQueue(email);
            if (this.SendMailUsingSMTP(email, linkToBeSend))
            {
                return true;
            }

            return false;
        }

        private void MSMQSend(int otp)
        {
            try
            {
                MessageQueue messageQueue = this.QueueDetail();
                Message message = new Message();
                message.Formatter = new BinaryMessageFormatter();
                message.Body = otp;
                messageQueue.Label = "Otp for password reset";
                messageQueue.Send(message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private string ReceiveQueue(string email)
        {
            ////for reading from MSMQ
            var receiveQueue = new MessageQueue(@".\Private$\ResetPasswordQueue");
            var receiveMsg = receiveQueue.Receive();
            receiveMsg.Formatter = new BinaryMessageFormatter();

            string linkToBeSend = receiveMsg.Body.ToString();
            return linkToBeSend;
        }

        private bool SendMailUsingSMTP(string email, string message)
        {
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            mailMessage.From = new MailAddress("bhargavilatha1999@gmail.com");
            mailMessage.To.Add(new System.Net.Mail.MailAddress(email));
            mailMessage.Subject = "Link to reset your password for BookStore App";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "Here is the otp : " + message;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("bhargavilatha1999@gmail.com", "Bhagi@1234");
            smtp.Send(mailMessage);
            return true;
        }

        public bool ResetPassword(ResetPasswordModel resetPassword)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {
                    //passing query in terms of stored procedure
                    SqlCommand sqlCommand = new SqlCommand("[dbo].[UpdatePassword]", sqlConnection);
                    //passing command type as stored procedure
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    //adding the parameter to the strored procedure
                    var password = this.EncryptPassword(resetPassword.NewPassword);
                    sqlCommand.Parameters.AddWithValue("@UserId", resetPassword.UserId);
                    sqlCommand.Parameters.AddWithValue("@NewPassword", password);
                    sqlCommand.Parameters.Add("@result", SqlDbType.Int);
                    sqlCommand.Parameters["@result"].Direction = ParameterDirection.Output;
                    //checking the result 
                    sqlCommand.ExecuteNonQuery();

                    var result = sqlCommand.Parameters["@result"].Value;
                    if (!(result is DBNull))
                        return true;
                    else
                        return false;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
        }
    }
}
   
