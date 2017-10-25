using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sf
{
    public class DBHelper
    {
        //string connectionString = "Data Source=DESKTOP-FLL68BC\SQLEXPRESS;Initial Catalog=CanToolApp;Integrated Security=True";
        //数据库连接字符串
        private static string SQL_CONN_STR = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;

        public static SqlConnection conn = new SqlConnection(SQL_CONN_STR);

        #region 【执行Select方法】
        //public DataSet GetDataSet(string sql)
        //{
        //    conn.Open();
        //    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
        //    SqlCommand cmd = new SqlCommand(sql, conn);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);
        //    return ds;
        //}
        #endregion

        #region [仿写方法]
        public DataSet GetDataSet(string sql)
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            SqlCommand cmd = new SqlCommand(sql, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            return ds;
        }
        #endregion


        #region[获取Message的ID]
        //根据message 获取 signal 的 ID
        public int SelectIdInMessage(string name)
        {

            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(name, conn);
            SqlCommand cmd = new SqlCommand(name, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            int  result = int.Parse(ds.Tables[0].Rows[0][0].ToString());
            conn.Close();

            
            return result;

        }

        //public int SelectIdInMessage(string name)
        //{
           
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = conn;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandText = "SelectIdInMessage";
        //        cmd.Parameters.AddWithValue("@MeaasgeName", name);
        //        int result = Convert.ToInt32(cmd.ExecuteScalar());
        //        return result;
           
        //}
        #endregion
    }
}
