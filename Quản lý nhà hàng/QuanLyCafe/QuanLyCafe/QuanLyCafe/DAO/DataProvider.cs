using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyCafe.DAO
{
   
    public class DataProvider
    {
        private static DataProvider instance;

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            set { DataProvider.instance = value; }
        }
        private DataProvider()
        {

        }
        private string strcon = "Data Source=ADMIN\\SQLEXPRESS;Initial Catalog=QuanLyQuanCafe;Integrated Security=True";
        public DataTable ExecuteQuery(string query,object[] paramater=null)
        {
            DataTable dtb = new DataTable();
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                if (paramater != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, paramater[i]);
                            i++;
                            
                        }
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtb);
                con.Close();
            }
            return dtb;
        }

        public int ExecuteNonQuery(string query, object[] paramater = null)
        {
            int data = 0;
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                if (paramater != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, paramater[i]);
                            i++;

                        }
                    }
                }
                data = cmd.ExecuteNonQuery();
                con.Close();
            }
            return data;
        }

        public object ExecuteScalar(string query, object[] paramater = null)
        {
            object data = 0;
            using (SqlConnection con = new SqlConnection(strcon))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                if (paramater != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, paramater[i]);
                            i++;

                        }
                    }
                }
                data = cmd.ExecuteScalar();
                con.Close();
            }
            return data;
        }
    }

}
