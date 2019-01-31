using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;


namespace fs
{
    class Program
    {
        protected static ArrayList FileList = new ArrayList();

        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                if (args.Length == 3)
                {
                    string comPath = args[1];
                    int pos = comPath.LastIndexOf("\\");
                    string zipFile = args[1].Substring(0, pos) + "\\zip.zip";
                    ZipFile.CreateFromDirectory(comPath, zipFile, CompressionLevel.Optimal, true, Encoding.UTF8);

                    int res = 0;
                    while (res == 0)
                    {
                        try
                        {
                            res = UploadOper(args, zipFile);
                        }
                        catch
                        {

                        }
                    }

                    FileInfo file = new FileInfo(zipFile);
                    file.Delete();

                }
                else if (args.Length == 2)
                {
                    ArrayList filelist = GetAllFile(args[1]);
                    if (filelist.Count > 0) {
                        foreach (string fi in filelist)
                        {
                            int res = 0;
                            while (res == 0)
                            {
                                try
                                {
                                    res = UploadOper(args, fi);
                                }
                                catch
                                {

                                }
                            }
                        }

                    }
                }
                else if (args.Length == 4)
                {
                    ArrayList filelist = GetAllFile(args[1]);
                    if (filelist.Count > 0)
                    {
                        foreach (string fi in filelist)
                        {
                            int res = 0;
                            while (res == 0)
                            {
                                try
                                {
                                    res = FileDivUpload(args, fi);
                                }
                                catch
                                {

                                }
                            }

                        }
                    }
                }
            }
            else
            {
                Console.Write("parameter false! \r");
            }
        }

        private static int FileDivUpload(string[] args, string fi)
        {

            FileStream fs = new FileStream(fi, FileMode.Open, FileAccess.Read);
            int pos = fi.LastIndexOf("\\");
            string fileName = fi.Substring(pos + 1);
            long filesize = fs.Length;
            string fileMD5 = "";
            if (filesize > 0)
            {
                long block = filesize / 1024 + 1;
                if (filesize > 1024000)
                {
                    byte[] fileByte = new byte[1024000];
                    fs.Read(fileByte, 0, 1024000);
                    fs.Close();//取1M字节
                    fileMD5 = MD5(fileByte);  //生成文件特征MD5
                }
                else
                {
                    byte[] fileByte = new byte[filesize];
                    fs.Read(fileByte, 0, (int)filesize);
                    fs.Close();
                    fileMD5 = MD5(fileByte);  //生成文件特征MD5
                }

                List<byte[]> aa = new List<byte[]>();
                long sended = 0;
                FileStream fsn = new FileStream(fi, FileMode.Open, FileAccess.Read);
               
                while (filesize - sended > 0)
                {
                    if (filesize - sended > 1024)
                    {
                        byte[] buffsize = new byte[1024];
                        fsn.Seek(sended, SeekOrigin.Begin);
                        fsn.Read(buffsize, 0, 1024);
                      
                        string blockMD5 = MD5(buffsize);
                        int res = 0;
                        while (res == 0)
                        {
                            //

                            aa.Add(buffsize);
                            if (aa.Count > 2000)
                            {
                                //TestSaveBlock(aa, fi, fileName);
                                while (res == 0)
                                {
                                    
                                    res = UploadBlockOper(args, aa, fi, blockMD5, fileMD5);
                                }
                                aa.Clear();
                            }
                           
                            res = 1;
                        }
                        sended += 1024;
                    }
                    else
                    {
                        byte[] buffsize = new byte[filesize - sended];
                        fsn.Seek(sended, SeekOrigin.Begin);
                        fsn.Read(buffsize, 0, buffsize.Length);
                     
                        string blockMD5 = MD5(buffsize);
                        int res = 0;
                        aa.Add(buffsize);
                        while (res == 0)
                        {
                            //

                            while (res == 0)
                            {

                                res = UploadBlockOper(args, aa, fi, blockMD5, fileMD5);
                            }                            //TestSaveBlock(aa, fi, fileName);
                            aa.Clear();
                            res = 1;
                        }
                       
                        sended += buffsize.Length;
                    }
                }
                fsn.Close();
                //string dest = fi.Substring(0, fi.LastIndexOf("\\")) + "\\NEW" + fileName;
                //foreach (byte[] item in aa)
                //{
                //    FileStream fsn2 = new FileStream(dest, FileMode.Append, FileAccess.Write);
                //    fsn2.Write(item, 0, item.Length);
                //    fsn2.Flush();
                //    fsn2.Close();
                //}

                int res2 = 0;
                while (res2 == 0)
                {

                    res2 = UploadEndFlag(args, "end", fi.Substring(0, fi.LastIndexOf("\\")), fileName, fileMD5);
                }
                //fs.Close();
            }
            return 1;

        }

        private static void TestSaveBlock(List<byte[]> aa,string fi,string fileName)
        {
            //string dest = fi.Substring(0, fi.LastIndexOf("\\")) + "\\NEW" + fileName;
            //FileStream fsn2 = new FileStream(dest, FileMode.Append, FileAccess.Write);
            //fsn2.Write(item, 0, item.Length);
            //fsn2.Flush();
            //fsn2.Close();
            string dest = fi.Substring(0, fi.LastIndexOf("\\")) + "\\NEW" + fileName;
            FileStream fsn2 = new FileStream(dest, FileMode.Append, FileAccess.Write);
            foreach (byte[] item in aa)
            {
                fsn2.Write(item, 0, item.Length);               
            }
            fsn2.Flush();
            fsn2.Close();
        }

        private static string MD5(byte[] source)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] resByte = x.ComputeHash(source);
            StringBuilder result = new StringBuilder();
            foreach(byte b in resByte)
            {
                result.Append(b.ToString("x2").ToLower());
            }
            return result.ToString();
        }

        private static int UploadEndFlag(string[] args,string endFlag,string path,string fileName, string fileMD5)
        {
            HttpWebRequest req = WebRequest.Create(args[0]) as HttpWebRequest;
            req.Method = "POST";
            req.Timeout = 3600000;
            string boundary = DateTime.Now.Ticks.ToString("X");
            req.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            StringBuilder sbHeader = new StringBuilder();
            sbHeader.Append(string.Format("Content-Disposition:form-data;name=\"endFlag\"\r\n\r\n{0}", endFlag));
            sbHeader.Append(Encoding.UTF8.GetString(itemBoundaryBytes));
            sbHeader.Append(string.Format("Content-Disposition:form-data;name=\"path\"\r\n\r\n{0}", path));
            sbHeader.Append(Encoding.UTF8.GetString(itemBoundaryBytes));
            sbHeader.Append(string.Format("Content-Disposition:form-data;name=\"fileName\"\r\n\r\n{0}", fileName));
            sbHeader.Append(Encoding.UTF8.GetString(itemBoundaryBytes));
            sbHeader.Append(string.Format("Content-Disposition:form-data;name=\"fileMD5\"\r\n\r\n{0}", fileMD5));
            sbHeader.Append(Encoding.UTF8.GetString(itemBoundaryBytes));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            Stream postStream = req.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);           
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();

            //发送请求并获取相应回应数据
            HttpWebResponse response = req.GetResponse() as HttpWebResponse;

            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);

            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            sr.Close();
            //Console.WriteLine(content + "\r\n");
            if (content == "success")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private static int UploadBlockOper(string[] args, List<byte[]> buffer,string fi,string md5,string fileMD5)
        {
            HttpWebRequest req = WebRequest.Create(args[0]) as HttpWebRequest;
            req.Method = "POST";
            req.Timeout = 3600000;
            string boundary = DateTime.Now.Ticks.ToString("X");
            req.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            int pos = fi.LastIndexOf("\\");
            string fileName = fi.Substring(pos + 1);

            //请求头部信息 
            StringBuilder sbHeader = new StringBuilder();                     
                sbHeader.Append(string.Format("Content-Disposition:form-data;name=\"buffMD5\"\r\n\r\n{0}", md5));
                sbHeader.Append(Encoding.UTF8.GetString(itemBoundaryBytes));
            sbHeader.Append(string.Format("Content-Disposition:form-data;name=\"fileMD5\"\r\n\r\n{0}", fileMD5));
            sbHeader.Append(Encoding.UTF8.GetString(itemBoundaryBytes));
            sbHeader.Append(string.Format("Content-Disposition:form-data;name=\"filepath\"\r\n\r\n{0}", fi.Substring(0, pos)));
            sbHeader.Append(Encoding.UTF8.GetString(itemBoundaryBytes));
            sbHeader.Append(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            Stream postStream = req.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            foreach (byte[] item in buffer)
            {
                postStream.Write(item, 0, item.Length);
            }
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();

            //发送请求并获取相应回应数据
            HttpWebResponse response = req.GetResponse() as HttpWebResponse;

            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);

            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            sr.Close();
            //Console.WriteLine(content + "\r\n");
            if (content == "success")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private static int UploadOper(string[] args, string fi)
        {

            HttpWebRequest req = WebRequest.Create(args[0]) as HttpWebRequest;
            req.Method = "POST";
            req.Timeout = 3600000;
            string boundary = DateTime.Now.Ticks.ToString("X");
            req.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            int pos = fi.LastIndexOf("\\");
            string fileName = fi.Substring(pos + 1);

            //请求头部信息 
            StringBuilder sbHeader = new StringBuilder();
            sbHeader.Append(string.Format("Content-Disposition:form-data;name=\"filepath\"\r\n\r\n{0}", fi.Substring(0, pos)));
            sbHeader.Append(Encoding.UTF8.GetString(itemBoundaryBytes));
            sbHeader.Append(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());
            FileStream fs = new FileStream(fi, FileMode.Open, FileAccess.Read);
            byte[] bArr = new byte[fs.Length];
            fs.Read(bArr, 0, bArr.Length);
            fs.Close();

            Stream postStream = req.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            postStream.Write(bArr, 0, bArr.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();

            //发送请求并获取相应回应数据
            HttpWebResponse response = req.GetResponse() as HttpWebResponse;

            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);

            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            sr.Close();
            Console.WriteLine(content + "\r\n");
            return 1;
        }

        private static ArrayList GetAllFile(string dir)
        {            
            FileInfo targetFileInfo=new FileInfo(dir);
            DirectoryInfo targetdir = new DirectoryInfo(dir);
            if (targetFileInfo.Exists)
            {
                ArrayList fileList=new ArrayList();
                fileList.Add(dir);
                return fileList;
            }
            else if(targetdir.Exists)
            {
                FileInfo[] allFile = targetdir.GetFiles();
                foreach (FileInfo fi in allFile)
                {
                    FileList.Add(fi.DirectoryName + "\\" + fi.Name);
                }
                DirectoryInfo[] alldir = targetdir.GetDirectories();
                foreach (DirectoryInfo di in alldir)
                {
                    GetAllFile(di.FullName);
                }
                return FileList;
            }
            return null;
        }
    }
}
