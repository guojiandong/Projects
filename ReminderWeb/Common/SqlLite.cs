using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common
{
    public static class SqlLite
    {
        //插入数据
        public static void ExecuteInsert(string strSql, ref StringBuilder builder)
        {
            MySqlConnection conn = null;
            string constructorString = "server=172.17.201.229;User Id=dbproxy;password= luxshare.auto.dbproxy.pwd;Database=db_work_progress";
            try
            {
                conn = new MySqlConnection(constructorString);
                conn.Open();

                MySqlCommand mycmd = new MySqlCommand();
                mycmd.Connection = conn;
                mycmd.CommandType = CommandType.Text;
                mycmd.CommandText = strSql;
                mycmd.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                builder.Append("SqlLite - ExecuteInsert Error: " + ex.Message + " Sql :" + strSql);
            }
            finally
            {
                conn.Close();
            }
        }

        //获取数据
        public static DataTable ExecuteSelect(string strSql)
        {
            MySqlConnection conn = null;
            string constructorString = "server=172.17.201.229;User Id=dbproxy;password= luxshare.auto.dbproxy.pwd;Database=db_work_progress;charset=utf8";
            // This will hold the records. 
            DataTable inv = new DataTable();
            try
            {
                conn = new MySqlConnection(constructorString);
                conn.Open();

                MySqlCommand mycmd = new MySqlCommand();
                mycmd.Connection = conn;
                mycmd.CommandType = CommandType.Text;
                mycmd.CommandText = strSql;
                MySqlDataReader dr = mycmd.ExecuteReader();
                inv.Load(dr);
                dr.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }

            return inv;
        }
    }
}
