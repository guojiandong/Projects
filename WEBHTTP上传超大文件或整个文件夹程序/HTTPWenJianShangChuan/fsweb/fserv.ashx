<%@ WebHandler Language="C#" Class="fserv" %>

using System;
using System.Web;
using System.IO;
using System.Text;

public class fserv : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        try
        {
            if (context.Request.Files.Count > 0)
            {
                HttpPostedFile file = context.Request.Files[0];

                string filepath = context.Request["filepath"];
                if (string.IsNullOrEmpty(context.Request["buffMD5"]) && string.IsNullOrEmpty(context.Request["fileMD5"]))
                {
                    string[] path = filepath.Split(new char[] { '\\' });
                    StringBuilder savepath = new StringBuilder(context.Server.MapPath("\\UploadFile\\"));
                    for (int i = 1; i < path.Length; i++)
                    {
                        savepath.Append(path[i] + "\\");
                        DirectoryInfo dir = new DirectoryInfo(savepath.ToString());
                        if (!dir.Exists)
                        {
                            dir.Create();
                        }

                    }
                    file.SaveAs(savepath.ToString() + file.FileName);
                    //file.SaveAs(context.Server.MapPath("\\UploadFile\\" + file.FileName));
                    context.Response.Write("Success\r\n");
                }
                else if (!string.IsNullOrEmpty(context.Request["buffMD5"]) && !string.IsNullOrEmpty(context.Request["fileMD5"]) && string.IsNullOrEmpty(context.Request["endFlag"]))
                {

                    string[] path = filepath.Split(new char[] { '\\' });
                    StringBuilder savepath = new StringBuilder(context.Server.MapPath("\\UploadFile\\"));
                    for (int i = 1; i < path.Length; i++)
                    {
                        savepath.Append(path[i] + "\\");
                        DirectoryInfo dir = new DirectoryInfo(savepath.ToString());
                        if (!dir.Exists)
                        {
                            dir.Create();
                        }

                    }
                    FileInfo fi = new FileInfo(savepath.ToString() + file.FileName + context.Request["fileMD5"]);
                    if (!fi.Exists)
                    {
                        byte[] fileByte = new byte[file.ContentLength];
                        file.InputStream.Read(fileByte, 0, file.ContentLength);
                        FileStream fs = new FileStream(savepath.ToString() + file.FileName + context.Request["fileMD5"], FileMode.CreateNew, FileAccess.Write);
                        fs.Write(fileByte, 0, fileByte.Length);
                        fs.Flush();
                        fs.Close();
                        context.Response.Write("success");
                    }
                    else
                    {
                        byte[] fileByte = new byte[file.ContentLength];
                        file.InputStream.Read(fileByte, 0, file.ContentLength);
                        FileStream fs = new FileStream(savepath.ToString() + file.FileName + context.Request["fileMD5"], FileMode.Append, FileAccess.Write);
                        fs.Write(fileByte, 0, fileByte.Length);
                        fs.Flush();
                        fs.Close();
                        context.Response.Write("success");
                    }
                    //file.SaveAs(savepath.ToString() + file.FileName + context.Request["fileMD5"]);
                }

            }
            else if (!string.IsNullOrEmpty(context.Request["endFlag"]))
            {
                string filename = context.Request["fileName"];
                string filepath = context.Request["path"];
                string[] path = filepath.Split(new char[] { '\\' });
                string fileMD5 = context.Request["fileMD5"];
                StringBuilder savepath = new StringBuilder(context.Server.MapPath("\\UploadFile\\"));
                for (int i = 1; i < path.Length; i++)
                {
                    savepath.Append(path[i] + "\\");


                }
                FileInfo fi = new FileInfo(savepath.ToString() + filename + fileMD5);

                if (fi.Exists)
                {
                    fi.CopyTo(savepath.ToString() + filename);
                    fi.Delete();
                }
                context.Response.Write("success");
            }

        }
        catch(Exception ex)
        {
            context.Response.Write(ex.Message);
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}