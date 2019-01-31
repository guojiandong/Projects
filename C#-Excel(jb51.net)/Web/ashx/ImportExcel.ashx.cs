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
            StringBuilder errorMsg = new StringBuilder(); // 错误信息
            //try
            //{
                #region 1.获取Excel文件并转换为一个List集合
                // 1.1存放Excel文件到本地服务器
                HttpPostedFile filePost = context.Request.Files["filed"]; // 获取上传的文件
                string filePath = ExcelHelper.SaveExcelFile(filePost); // 保存文件并获取文件路径

                // 1.2解析文件，存放到一个List集合里,并插入到数据库中
                ExcelHelper.ExcelToEntityList(filePath, ref errorMsg);
                #endregion


                // 2.返回操作结果
                bool isSuccess = false;
                if (errorMsg.Length == 0)
                {
                    isSuccess = true; // 若错误信息成都为空，表示无错误信息
                }
                var rs = new { success = isSuccess,   msg = errorMsg.ToString() /*,  data = enlist */ };
                System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
                context.Response.ContentType = "text/plain";
                context.Response.Write(js.Serialize(rs)); // 返回Json格式的内容
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //    // MesageBox.Show(ex.Message);
            //    // return;
            //}
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