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
    public class CartRepository : ICartRepository
    {
        public IConfiguration Configuration { get; }
        public CartRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        SqlConnection sqlConnection;     
        public bool AddToCart(CartModel details)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("spAddCart", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@BookId", details.BookID);
                    sqlCommand.Parameters.AddWithValue("@UserId", details.UserId);
                    sqlCommand.Parameters.AddWithValue("@NoOfBook", details.BookOrderCount);
                    var returnedSQLParameter = sqlCommand.Parameters.Add("@result", SqlDbType.Int);
                    returnedSQLParameter.Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();
                    var result = (int)returnedSQLParameter.Value;
                    if (result.Equals(1))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
        }

        public List<CartModel> GetCart(int userId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("GetCartItem", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@userId", userId);
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    List<CartModel> cartItems = new List<CartModel>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            CartModel cart = new CartModel();
                            BookModel book = new BookModel();
                            cart.BookID = Convert.ToInt32(reader[0]);
                            book.BookName = reader[1].ToString();
                            book.AuthorName = reader[2].ToString();
                            book.Price = Convert.ToInt32(reader[3]);
                            book.Image = reader[8].ToString();
                            book.OriginalPrice = Convert.ToInt32(reader[4]);
                            book.BookCount = Convert.ToInt32(reader[7]);
                            cart.CartID = Convert.ToInt32(reader[5]);
                            cart.BookOrderCount = Convert.ToInt32(reader[6]);
                            cart.UserId = Convert.ToInt32(reader[9]);
                            cart.Books = book;
                            cartItems.Add(cart);
                        }

                    }
                    return cartItems;
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
        public bool UpdateCart(CartModel details)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("UpdateCart", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@CartId", details.CartID);
                    sqlCommand.Parameters.AddWithValue("@Type", details.type);
                    var returnedSQLParameter = sqlCommand.Parameters.Add("@result", SqlDbType.Int);
                    returnedSQLParameter.Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();
                    var result = (int)returnedSQLParameter.Value;
                    if (result.Equals(1))
                    {
                        return true;
                    }
                    else
                    {
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

        public bool DeleteCart(int cartId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("RemoveFromCart", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@cartId", cartId);
                    var returnedSQLParameter = sqlCommand.Parameters.Add("@result", SqlDbType.Int);
                    returnedSQLParameter.Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();
                    var result = (int)returnedSQLParameter.Value;
                    if (result.Equals(1))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

        }
    }
}
