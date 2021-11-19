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
    public class WishlistRepository : IWishlistRepository
    {
        public IConfiguration Configuration { get; }
        public WishlistRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        SqlConnection sqlConnection;
        public bool AddToWishList(WishlistModel wishListmodel)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {

                    SqlCommand sqlCommand = new SqlCommand("AddtoWishList", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();


                    sqlCommand.Parameters.AddWithValue("@BookId", wishListmodel.BookId);
                    sqlCommand.Parameters.AddWithValue("@UserId", wishListmodel.UserId);


                    var result = sqlCommand.ExecuteNonQuery();

                    if (result > 0)
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

        public bool RemoveFromWishList(int wishListId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {

                    SqlCommand sqlCommand = new SqlCommand("SpRemoveFromWishList", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();


                    sqlCommand.Parameters.AddWithValue("@WishListId", wishListId);

                    sqlCommand.Parameters.Add("@result", SqlDbType.Int);
                    sqlCommand.Parameters["@result"].Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();

                    var result = sqlCommand.Parameters["@result"].Value;
                    if (result.Equals(1))
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

        public List<WishlistModel> GetWishList(int userId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)

                try
                {
                    SqlCommand sqlCommand = new SqlCommand("GetWishlist", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@userId", userId);
                    List<WishlistModel> wishList = new List<WishlistModel>();
                    BookModel booksModel = new BookModel();
                    WishlistModel wishListModel = new WishlistModel();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.Read())
                    {


                        wishListModel.BookId = Convert.ToInt32(reader["BookId"]);
                        booksModel.AuthorName = reader["AuthorName"].ToString();
                        booksModel.BookName = reader["BookName"].ToString();
                        booksModel.Price = Convert.ToInt32(reader["Price"]);
                        booksModel.Image = reader["Image"].ToString();
                        booksModel.OriginalPrice = Convert.ToInt32(reader["OriginalPrice"]);
                        wishListModel.WishListId = Convert.ToInt32(reader["WishListId"]);
                        wishListModel.Books = booksModel;


                    }
                    return wishList;

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
