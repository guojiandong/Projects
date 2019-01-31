using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ashx
{
    /// <summary>
    /// ProcessHandler 的摘要说明
    /// </summary>
    public class ProcessHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            string sql = "select * from tb_work_progress;";
            System.Data.DataTable data_table = SqlLite.ExecuteSelect(sql);
            ExcelHelper.ExportReminder(data_table, ref result);

            // 2.返回操作结果
            bool isSuccess = false;
            if (result.Length == 0)
            {
                isSuccess = true;
            }
            var rs = new { success = isSuccess, msg = result.ToString() };
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            context.Response.ContentType = "text/plain";
            context.Response.Write(js.Serialize(rs)); // 返回Json格式的内容
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}