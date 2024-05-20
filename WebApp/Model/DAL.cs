using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebApp.Model
{
    public class DAL
    {
        public List<Users> GetUsers(IConfiguration configuration)
        {
            List<Users> listUsers = new List<Users>();
            using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("DBCS").ToString()))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [BikeStores].[production].[products]", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Users user = new Users();
                        user.product_id = dt.Rows[i]["product_id"] != DBNull.Value ? (int?)Convert.ToInt32(dt.Rows[i]["product_id"]) : null;
                        user.product_name = dt.Rows[i]["product_name"] != DBNull.Value ? Convert.ToString(dt.Rows[i]["product_name"]) : null;
                        user.list_price = dt.Rows[i]["list_price"] != DBNull.Value ? (decimal?)Convert.ToDecimal(dt.Rows[i]["list_price"]) : null;
                        user.model_year = dt.Rows[i]["model_year"] != DBNull.Value ? (int?)Convert.ToInt32(dt.Rows[i]["model_year"]) : null;
                        user.brand_id = dt.Rows[i]["brand_id"] != DBNull.Value ? (int?)Convert.ToInt32(dt.Rows[i]["brand_id"]) : null;
                        listUsers.Add(user);
                    }
                }
            }
            return listUsers;
        }

        public Users GetUser(int productId, IConfiguration configuration)
        {
            Users user = null;
            using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("DBCS").ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [BikeStores].[production].[products] WHERE product_id = @ProductID", con);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    user = new Users
                    {
                        product_id = reader["product_id"] != DBNull.Value ? (int?)Convert.ToInt32(reader["product_id"]) : null,
                        product_name = reader["product_name"] != DBNull.Value ? Convert.ToString(reader["product_name"]) : null,
                        list_price = reader["list_price"] != DBNull.Value ? (decimal?)Convert.ToDecimal(reader["list_price"]) : null,
                        model_year = reader["model_year"] != DBNull.Value ? (int?)Convert.ToInt32(reader["model_year"]) : null,
                        brand_id = reader["brand_id"] != DBNull.Value ? (int?)Convert.ToInt32(reader["brand_id"]) : null
                    };
                }
                con.Close();
            }
            return user;
        }

        public int AddUser(Users user, IConfiguration configuration)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("DBCS").ToString()))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO [BikeStores].[production].[products] (product_name, list_price, model_year, brand_id) " +
                    "VALUES (@ProductName, @ListPrice, @ModelYear, @BrandId)", con);

                cmd.Parameters.AddWithValue("@ProductName", user.product_name);
                cmd.Parameters.AddWithValue("@ListPrice", user.list_price);
                cmd.Parameters.AddWithValue("@ModelYear", user.model_year); // handle nullable value
                cmd.Parameters.AddWithValue("@BrandId", user.brand_id);

                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();
            }
            return i;
        }

        public int UpdateUser(Users user, IConfiguration configuration)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("DBCS").ToString()))
            {
                SqlCommand cmd = new SqlCommand("UPDATE [BikeStores].[production].[products] SET product_name = @ProductName, list_price = @ListPrice, model_year = @ModelYear, brand_id = @BrandId WHERE product_id = @ProductID", con);

                cmd.Parameters.AddWithValue("@ProductName", user.product_name);
                cmd.Parameters.AddWithValue("@ListPrice", user.list_price);
                cmd.Parameters.AddWithValue("@ModelYear", user.model_year);
                cmd.Parameters.AddWithValue("@BrandId", user.brand_id);
                cmd.Parameters.AddWithValue("@ProductID", user.product_id);

                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();
            }
            return i;
        }

        public int DeleteUser(int productId, IConfiguration configuration)
        {
            int i = 0;
            using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("DBCS").ToString()))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM [BikeStores].[production].[products] WHERE product_id = @ProductID", con);
                cmd.Parameters.AddWithValue("@ProductID", productId);

                con.Open();
                i = cmd.ExecuteNonQuery();
                con.Close();
            }
            return i;
        }
    }
}
