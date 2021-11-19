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
    public class AddressRepository : IAddressRepository
    {
        public IConfiguration Configuration { get; }
        public AddressRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        SqlConnection sqlConnection;
        public bool AddAddress(AddressModel addressDetails)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));

            using (sqlConnection)

                try
                {

                    SqlCommand sqlCommand = new SqlCommand("SpAddUserAddress", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@Address", addressDetails.Address);
                    sqlCommand.Parameters.AddWithValue("@City", addressDetails.City);
                    sqlCommand.Parameters.AddWithValue("@State", addressDetails.State);
                    sqlCommand.Parameters.AddWithValue("@Type", addressDetails.Type);
                    sqlCommand.Parameters.AddWithValue("@UserId", addressDetails.UserId);

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

        public bool EditAddress(AddressModel addressDetails)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));

            using (sqlConnection)

                try
                {

                    SqlCommand sqlCommand = new SqlCommand("UpdateUserAddress", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@Address", addressDetails.Address);
                    sqlCommand.Parameters.AddWithValue("@City", addressDetails.City);
                    sqlCommand.Parameters.AddWithValue("@State", addressDetails.State);
                    sqlCommand.Parameters.AddWithValue("@Type", addressDetails.Type);
                    sqlCommand.Parameters.AddWithValue("@AddressID", addressDetails.AddressId);
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

        public List<AddressModel> GetUserAddress(int userId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));

            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("GetAddressDetails", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader readData = cmd.ExecuteReader();
                List<AddressModel> userdetaillist = new List<AddressModel>();
                if (readData.HasRows)
                {
                    while (readData.Read())
                    {
                        AddressModel userDetail = new AddressModel();
                        userDetail.AddressId = readData.GetInt32("AddressId");
                        userDetail.Address = readData.GetString("Address");
                        userDetail.City = readData.GetString("City").ToString();
                        userDetail.State = readData.GetString("State");
                        userDetail.Type = readData.GetString("Type");
                        userdetaillist.Add(userDetail);
                    }
                }
                return userdetaillist;
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

        public bool RemoveUserAddress(int addressId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {              

                    SqlCommand sqlCommand = new SqlCommand("RemoveUserAddress", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();


                    sqlCommand.Parameters.AddWithValue("@AddressId", addressId);


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
    }
}
