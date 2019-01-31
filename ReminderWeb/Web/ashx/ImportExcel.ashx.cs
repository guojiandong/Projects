using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using Model;
using System.Text;

namespace Web.ashx
{
    /// <summary>
    /// 导入Excel
    /// </summary>
    public class ImportExcel : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            StringBuilder result = new StringBuilder(); // 错误信息
            // 1.提交数据
            HttpRequest Request = context.Request;
            var progress = Request.Params["progress"];
            var project_name = Request.Params["project_name"];
            var deadline = Request.Params["deadline"];
            var remark = Request.Params["remark"];
            var curday = Request.Params["nowday"];
            var name = Request.Params["Name"];

            string sql = "insert into tb_work_progress(ProjectName,Deadline,CurDate,Remark,Progress,Name) values ("+
                "'" + project_name + "',"+
                "'" + deadline + "'," +
                "'" + curday + "'," +
                "'" + remark + "'," +
                "'" + progress + "'," +
                "'" + name + "'" +
                ") on duplicate key update Progress=VALUES(Progress)";
            SqlLite.ExecuteInsert(sql , ref result);

            // 2.返回操作结果
            bool isSuccess = false;
            if (result.Length == 0)
            {
                isSuccess = true;
            }
            var rs = new { success = isSuccess,   msg = result.ToString() };
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