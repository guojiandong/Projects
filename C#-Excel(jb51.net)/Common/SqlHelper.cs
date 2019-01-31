using Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Common
{
    class SqlHelper
    {
        public static Dictionary<string, string> dic_tb_subject;
        public static Dictionary<string, string> dic_tb_userline;
        public static Dictionary<string, string> dic_tb_process_best;
        public static Dictionary<string, string> dic_tb_process_norm;
        public static Dictionary<string, string> dic_tb_process_id_code;
        public static Dictionary<string, int> dayNight;
        public static Dictionary<string, ProcessEntity> dic_processEntity;

        public static Dictionary<string, string> dic_tb_subject_best;
        public static Dictionary<string, string> dic_tb_subject_norm;
        public static Dictionary<string, string> dic_tb_line_workday;

        // 初始化数据
        public static void InitData()
        {
            // 初始化 SubjectDic
            string subject_sql = "SELECT  id, name, processId  FROM tb_subject";
            dic_tb_subject = ExecuteSelect(subject_sql, 3);

            //初始化 UserLineDic
            string userline_sql = "SELECT  lineId,code FROM tb_userline";
            dic_tb_userline = ExecuteSelect(userline_sql, 2);

            //初始化 theBest
            string thebest_sql = "SELECT  theBest, id FROM tb_process";
            dic_tb_process_best = ExecuteSelect(thebest_sql, 2);

            //初始化 theNorm
            string thenorm_sql = "SELECT  theNorm, id FROM tb_process";
            dic_tb_process_norm = ExecuteSelect(thenorm_sql, 2);

            //初始化 tb_process
            string the_process_id_code_sql = "SELECT  id, auth FROM tb_process";
            dic_tb_process_id_code = ExecuteSelect(the_process_id_code_sql, 2);

            // 初始化 dic_tb_subject_best
            string the_subject_best_sql = "SELECT  best,name,processId  FROM tb_subject";
            dic_tb_subject_best = ExecuteSelect(the_subject_best_sql, 3);

            // 初始化 dic_tb_subject_best
            string the_subject_norm_sql = "SELECT  norm,name,processId  FROM tb_subject";
            dic_tb_subject_norm = ExecuteSelect(the_subject_norm_sql, 3);

            //初始化 dic_lineWorkDay
            string the_line_workday_sql = "SELECT classId,line, month  FROM tb_lineWorkday";
            dic_tb_line_workday = ExecuteSelect(the_line_workday_sql, 3);

            //初始化 ClassId 白夜班
            dayNight = InitDayNight();
        }

        // 查询数据 MySqlConnection
        // [0] Value  [1] + [2]+ ..Key
        public static Dictionary<string, string> ExecuteSelect(string strSql, int column)
        {
            List<string> tempColumn = new List<string>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            MySqlDataReader reader = null;
            MySqlConnection conn = null;
            string constructorString = "server=172.17.201.229;User Id=dbproxy;password= luxshare.auto.dbproxy.pwd;Database=w1_pdmis";
            try
            {
                conn = new MySqlConnection(constructorString);
                conn.Open();

                MySqlCommand mycmd = new MySqlCommand(strSql, conn);
                reader = mycmd.ExecuteReader();
                while (reader.Read())
                {
                    tempColumn.Clear();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        tempColumn.Add(reader[i].ToString().Replace(" ", ""));
                    }
                    if (tempColumn.Count > 2)
                        dic.Add(tempColumn[1].ToLower() + "_" + tempColumn[2].ToLower(), tempColumn[0]);
                    else
                        dic.Add(tempColumn[1].ToLower(), tempColumn[0]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SqlHelper - ExecuteSelect Error: " + ex.Message + " Sql :" + strSql);
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
            return dic;
        }

        // Batch Select
        public static void ExecuteBatchSelect(string strSql)
        {
            MySqlDataReader reader = null;
            MySqlConnection conn = null;
            string key = "CA005";
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT  code, lineId FROM tb_userline where code in");
            sql.Append("(");
            sql.Append("'CA001");
            sql.Append("','");
            sql.Append("CA002");
            sql.Append("','");
            sql.Append("CA005'");
            sql.Append(")");
            strSql = sql.ToString();
            string constructorString = "server=172.17.201.229;User Id=dbproxy;password= luxshare.auto.dbproxy.pwd;Database=w1_pdmis";
            try
            {
                conn = new MySqlConnection(constructorString);
                conn.Open();

                MySqlCommand mycmd = new MySqlCommand(strSql, conn);
                reader = mycmd.ExecuteReader();
                while (reader.Read())
                {
                    string code = reader[0].ToString();
                    string userlineid = reader[1].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                reader.Close();
                conn.Close();
            }
        }

        // 初始化白夜班
        public static Dictionary<string, int> InitDayNight()
        {
            Dictionary<string, int> dayNight = new Dictionary<string, int>();
            return dayNight;
        }

        //插入数据
        public static void ExecuteInsert(string strSql)
        {
            MySqlConnection conn = null;
            string constructorString = "server=172.17.201.229;User Id=dbproxy;password= luxshare.auto.dbproxy.pwd;Database=w1_pdmis";
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
                Console.WriteLine("SqlHelper - ExecuteInsert Error: " + ex.Message + " Sql :"+ strSql);
            }
            finally
            {
                conn.Close();
            }
        }

        //关闭连接
        public static void CloseConnection()
        {
        }

        #region Set 赋值函数

        public static void GetSubjectId(string processId, string name )
        {

        }

        public static void GetClassId(ref QualityEntity entity)
        {

        }

        public static void GetUserLineId(ref QualityEntity entity)
        {

        }

        public static int GetTheBest(string code)
        {
            ProcessEntity entity;
            if (dic_processEntity.TryGetValue(code, out entity))
            {
                return entity.TheBest;
            }
            return -1;
        }

        public static int GetTheNorm(string code)
        {
            ProcessEntity entity;
            if (dic_processEntity.TryGetValue(code, out entity))
            {
                return entity.TheNorm;
            }
            return -1;
        }
        
        #endregion
    }
}
