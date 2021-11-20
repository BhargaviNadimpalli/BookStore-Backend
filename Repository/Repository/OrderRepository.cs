using Microsoft.Extensions.Configuration;
using Model;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Repository.Repository
{
   public class OrderRepository : IOrderRepository
    {
        public OrderRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        SqlConnection sqlConnection;


        public bool AddOrder(List<CartModel> orderdetails)
        {
            bool res = false;
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {
                    foreach (var order in orderdetails)
                    {
                        SqlCommand sqlCommand = new SqlCommand("AddOrder", sqlConnection);
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlConnection.Open();
                        sqlCommand.Parameters.AddWithValue("@BookId", order.BookID);
                        sqlCommand.Parameters.AddWithValue("@CartId", order.CartID);
                        sqlCommand.Parameters.AddWithValue("@UserId", order.UserId);
                        string date = DateTime.Now.ToString(" dd MMM yyyy");
                        sqlCommand.Parameters.AddWithValue("@OrderDate", date);
                        var returnedSQLParameter = sqlCommand.Parameters.Add("@result", SqlDbType.Int);
                        returnedSQLParameter.Direction = ParameterDirection.Output;
                        sqlCommand.ExecuteNonQuery();
                        int result = (int)returnedSQLParameter.Value;

                        if (result == 1)
                        {
                            res = true;
                            sqlConnection.Close();
                        }
                        else
                        {
                            res = false;
                            break;
                        }
                    }
                    return res;
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
