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
    public class BookRepository : IBookRepository
    {
        public IConfiguration Configuration { get; }
        public BookRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        SqlConnection sqlConnection;
        public bool AddBook(BookModel bookmodel)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                { 
                    
                    SqlCommand sqlCommand = new SqlCommand("[dbo].[SpAddBook]", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@BookName", bookmodel.BookName);
                    sqlCommand.Parameters.AddWithValue("@AuthorName", bookmodel.AuthorName);
                    sqlCommand.Parameters.AddWithValue("@Price", bookmodel.Price);
                    sqlCommand.Parameters.AddWithValue("@originalPrice", bookmodel.OriginalPrice);
                    sqlCommand.Parameters.AddWithValue("@BookDescription", bookmodel.BookDescription);
                    sqlCommand.Parameters.AddWithValue("@Image", bookmodel.Image);
                    sqlCommand.Parameters.AddWithValue("@Rating", bookmodel.Rating);
                    sqlCommand.Parameters.AddWithValue("@BookCount", bookmodel.BookCount);
                    int result = sqlCommand.ExecuteNonQuery();
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

        public BookModel GetBook(int bookId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("[dbo].[SpGetBook]", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@BookId", bookId);
                    BookModel booksModel = new BookModel();
                    SqlDataReader read = sqlCommand.ExecuteReader();
                    if (read.Read())
                    {
                        booksModel.AuthorName = read["AuthorName"].ToString();
                        booksModel.BookName = read["BookName"].ToString();
                        booksModel.BookDescription = read["BookDescription"].ToString();
                        booksModel.Price = Convert.ToInt32(read["Price"]);
                        booksModel.BookCount = Convert.ToInt32(read["BookCount"]);
                        booksModel.Rating = Convert.ToInt32(read["Rating"]);
                        booksModel.Image = read["Image"].ToString();
                        booksModel.OriginalPrice = Convert.ToInt32(read["OriginalPrice"]);
                       
                    }
                    return booksModel;
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

        public bool EditBook(BookModel bookmodel)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));

            using (sqlConnection)

                try
                {

                    SqlCommand sqlCommand = new SqlCommand("[dbo].[UpdateBook]", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@BookId", bookmodel.BookId);
                    sqlCommand.Parameters.AddWithValue("@BookName", bookmodel.BookName);
                    sqlCommand.Parameters.AddWithValue("@AuthorName", bookmodel.AuthorName);
                    sqlCommand.Parameters.AddWithValue("@Price", bookmodel.Price);
                    sqlCommand.Parameters.AddWithValue("@Rating", bookmodel.Rating);
                    sqlCommand.Parameters.AddWithValue("@BookCount", bookmodel.BookCount);
                    sqlCommand.Parameters.AddWithValue("@BookDescription", bookmodel.BookDescription);
                    sqlCommand.Parameters.AddWithValue("@Image", bookmodel.Image);
                    sqlCommand.Parameters.AddWithValue("@originalPrice", bookmodel.OriginalPrice);
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

        public bool RemoveBook(int bookId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {

                    SqlCommand sqlCommand = new SqlCommand("[dbo].[SPRemoveBook]", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();


                    sqlCommand.Parameters.AddWithValue("@BookId", bookId);
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

        public List<BookModel> GetAllBooks()
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {

                    SqlCommand sqlCommand = new SqlCommand("[dbo].[GetAllBooks]", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();

                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<BookModel> bookList = new List<BookModel>();
                        while (reader.Read())
                        {
                            BookModel booksModel = new BookModel();
                            booksModel.BookId = Convert.ToInt32(reader["Id"]);
                            booksModel.AuthorName = reader["AuthorName"].ToString();
                            booksModel.BookName = reader["BookName"].ToString();
                            booksModel.BookDescription = reader["BookDescription"].ToString();
                            booksModel.Price = Convert.ToInt32(reader["Price"]);
                            booksModel.Image = reader["Image"].ToString();
                            booksModel.OriginalPrice = Convert.ToInt32(reader["OriginalPrice"]);
                            booksModel.BookCount = Convert.ToInt32(reader["BookCount"]);
                            booksModel.Rating = Convert.ToInt32(reader["Rating"]);

                            bookList.Add(booksModel);
                        }
                        return bookList;
                    }
                    else
                    {
                        return null;
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
        public bool AddCustomerFeedBack(FeedBackModel feedbackmodel)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {

                    SqlCommand sqlCommand = new SqlCommand("SpAddFeedback", sqlConnection);
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@BookId", feedbackmodel.bookId);
                    sqlCommand.Parameters.AddWithValue("@UserId", feedbackmodel.userId);
                    sqlCommand.Parameters.AddWithValue("@Rating", feedbackmodel.rating);
                    sqlCommand.Parameters.AddWithValue("@FeedBack", feedbackmodel.feedback);


                    int result = sqlCommand.ExecuteNonQuery();

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

        public List<FeedBackModel> GetCustomerFeedBack(int bookid)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            try
            {
                SqlCommand sqlCommand = new SqlCommand("StoreProcedurGetCustomerFeedback", sqlConnection);

                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();


                sqlCommand.Parameters.AddWithValue("@bookid", bookid);
                List<FeedBackModel> feedbackList = new List<FeedBackModel>();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                   
                    while (reader.Read())
                    {
                        FeedBackModel feedbackdetails = new FeedBackModel();
                        feedbackdetails.userId = reader.GetInt32(0);
                        feedbackdetails.customerName = reader.GetString("FullName");
                        feedbackdetails.feedback = reader.GetString("Feedback");
                        feedbackdetails.rating = reader.GetDouble("Rating");
                        feedbackList.Add(feedbackdetails);
                    }

                }
                return feedbackList;
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
