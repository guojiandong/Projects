using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace mtTools
{
    /// <summary>
    /// mtTools v1.0.0.0 示例
    /// </summary>
    public class Demos
    {
        /// <summary>
        /// 示例
        /// </summary>
        public static void Test()
        {

            //FifoTest();

            //ResultTest();


            //CacheHelperTest();

            //ConfigHelperTest();

            //CookieHelperTest();

            //EmailHelperTest(); //不要拿我提供的测试邮箱，乱发垃圾邮件，广告等请自行注册临时邮箱

            //FileHelperTest();

            //HttpHelperTest();

            //LogHelperTest();


            //ConvertHelperTest();

            //DebugHelperTest();

            //EncodeHelperTest();

            //JsonHelperTest();

            //XmlHelperTest();


            StringHelperTest();

        }

        /// <summary>
        /// 线程安全的先进先出队列类 示例
        /// </summary>
        private static void FifoTest()
        {

            var fifo1 = new Fifo<int>(); //最大容量默认为int.MaxValue，初始分配容量默认为0
            var fifo2 = new Fifo<int>(1000); //最大容量默认为int.MaxValue，指定队列初始分配容量为1000
            var fifo3 = new Fifo<int>(2000, 1000); //指定队列最大容量为2000，指定初始分配容易为1000
            var c = fifo2.Count; //列队中元素存在个数
            var m = fifo2.MaxCount; //队列的最大容量

            var ma = fifo3.MaxCount;
            fifo3.ResetMaxCount(3000); //重新设置队列的最大容量
            var mb = fifo3.MaxCount;

            for (var i = 10; i < 25; i++)
            {
                fifo3.Append(i); //元素进队，排队列尾部
            }
            for (; fifo3.Count > 0;)
            {
                var t = fifo3.Pop(); //元素出队，先进先出从前向后移除元素
            }

        }

        /// <summary>
        /// 简单操作返回类 示例
        /// </summary>
        private static void ResultTest()
        {

            var res = new Result();
            res.Code = 1; //默认为0
            res.Message = "操作成功！"; //默认为""
            res.Content = new { userid = 1234, username = "mt" }; //默认为null

            var res2 = new Result(1, "操作成功！"); //Content默认为null
            var res3 = new Result(1, "操作成功！", new { userid = 1234, username = "mt" });

        }

        /// <summary>
        /// 缓存操作类 示例
        /// </summary>
        private static void CacheHelperTest()
        {

            CacheHelper.InsertCache("key1", "values", 3); //插入缓存，3分钟后强制过期
            CacheHelper.InsertCache("key2", "values", new TimeSpan(0, 0, 3, 0)); //插入缓存，3分钟内没有被访问则强制过期，被访问则重新计时

            CacheHelper.InsertCache("key3", new { id = 1, name = "mt" }, 3, new System.Web.Caching.CacheDependency(null, new string[] { "key1", "key2" }), System.Web.Caching.CacheItemPriority.Default, RemoveCacheEvent);
            CacheHelper.InsertCache("key11", new { id = 1, name = "mt" }, 3, new System.Web.Caching.CacheDependency(new string[] { "C:\\Windows\\regedit.exe" }, new string[] { "key1", "key2" }), System.Web.Caching.CacheItemPriority.High, RemoveCacheEvent);

            var obj = CacheHelper.GetCache("key3"); //获取缓存数据
            CacheHelper.RemoveCache("key3"); //移除缓存数据
            obj = CacheHelper.GetCache("key3"); //值为null

            var obj1 = CacheHelper.GetCache("key1");
            var obj2 = CacheHelper.GetCache("key2");
            var obj3 = CacheHelper.GetCache("key11");
            CacheHelper.RemoveMultiCache("ey1"); //删除key包含ey1的所有缓存

            obj1 = CacheHelper.GetCache("key1"); //值为null
            obj2 = CacheHelper.GetCache("key2");
            obj3 = CacheHelper.GetCache("key11"); //值为null
            CacheHelper.RemoveAllCache(); //移除所有缓存
            //CacheHelper.RemoveMultiCache(null); //移除所有缓存，上面方法实际调用为此

            obj1 = CacheHelper.GetCache("key1");
            obj2 = CacheHelper.GetCache("key2"); //值为null
            obj3 = CacheHelper.GetCache("key11");
        }
        /// <summary>
        /// 缓存移除调用此方法
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="reason"></param>
        private static void RemoveCacheEvent(string key, object value, System.Web.Caching.CacheItemRemovedReason reason)
        {
            if (reason == System.Web.Caching.CacheItemRemovedReason.Removed || reason == System.Web.Caching.CacheItemRemovedReason.DependencyChanged)
            {
                if (key == "key3") //判断到期的key并按需进行操作
                {

                }
            }
        }

        /// <summary>
        /// 获取配置文件（App.config或Web.config）中AppSetting配置项值或自定义节点的配置信息  示例
        /// </summary>
        private static void ConfigHelperTest()
        {
            //假如以下配置存在于App.config或Web.config中
            //<appSettings>
            //  <add key="v" value="1.0.0.0" />
            //</appSettings>
            //<configSections>
            //  ... ...
            //  <section name="toolDebug" type="System.Configuration.NameValueSectionHandler"/>
            //</configSections>
            //<!--DebugHelper配置-->
            //<toolDebug>
            //  <!--是否输出调试信息-->
            //  <add key="IsDebug" value="true"/>
            //  <!--调试信息内容格式：@Now-日期;@CodeLocal-出错位置;@Message-消息内容-->
            //  <add key="DebugMessage" value="【@Now】【@CodeLocal】@Message"/>
            //</toolDebug>

            var cf = ConfigHelper.GetCustomNodeSetting("toolDebug");
            var isDebug = cf["IsDebug"]; //true
            var isDebug2 = cf["tIsDebug"]; //tIsDebug不存在：值为null
            var cf2 = ConfigHelper.GetCustomNodeSetting("toolDebug"); //toolDebug不存在：值为CustomNodeSection，只是其中kv.Count==0
            var tDebug = cf2["tDebug"]; //tDebug不存在：值为null

            var appset = ConfigHelper.GetAppSettingValue("v"); //1.0.0.0
            var appset2 = ConfigHelper.GetAppSettingValue("v2"); //值不存在，默认为""
            var appset3 = ConfigHelper.GetAppSettingValue("v2", "2.0.0.0"); //值不存在，值为2.0.0.0
        }

        /// <summary>
        /// 客户端浏览器Cookie操作类 示例
        /// </summary>
        private static void CookieHelperTest()
        {
            CookieHelper.SetCookie("username", "mt", 7); //当前域名（如www.baidu.com）中设置cookie项username为mt

            CookieHelper.SetCookie("username2", "mt", 7, CookieDomain.TopDomain); //当前网站顶级域名（如baidu.com）中设置cookie项username2为mt
            CookieHelper.SetCookie("username3", "mt", 7, CookieDomain.CurrentDomain, "/images/"); //当前网站域名（如www.baidu.com）中的/images/路径下设置cookie项username3为mt

            //取cookie以验证输入
            var obj = CookieHelper.GetCookie("username");
            var obj2 = CookieHelper.GetCookie("username2");
            var obj3 = CookieHelper.GetCookie("username3");

            CookieHelper.RemoveCookie("username2", CookieDomain.TopDomain); //移除cookie时请保持与SetCookie时相同的参数以保证删除成功
            obj2 = CookieHelper.GetCookie("username2");
        }

        /// <summary>
        /// 邮件发送工具类 示例
        /// </summary>
        private static void EmailHelperTest()
        {
            //请于App.config或Web.config中自行配置如下信息，不配置则默认值如下
            //<configSections>
            //  ... ...
            //  <section name="toolEmail" type="System.Configuration.NameValueSectionHandler" />
            //</configSections>
            //<!--EmailHelper配置-->
            //<toolEmail>
            //  <!--邮件发件人名称-->
            //  <add key="EmailName" value="文昌" />
            //  <!--邮件smtp服务器地址-->
            //  <add key="EmailSmtp" value="smtp.126.com" />
            //  <!--邮件地址-->
            //  <add key="EmailAddress" value="mkwuji@126.com" />
            //  <!--邮件密码-->
            //  <add key="EmailPassword" value="mtTools2017" />
            //</toolEmail>

            var isres = EmailHelper.SendEmail("10011@qq.com", "邮件发送", "邮件内容");
            var isres2 = EmailHelper.SendEmail("10011@qq.com", "邮件发送", "邮件内容", "10010@qq.com", "C:\\Windows\\regedit.exe"); //增加抄送和附件
            var sendlist = new List<string>(); //发送人列表
            sendlist.Add("10011@qq.com");
            sendlist.Add("10012@qq.com");
            var ccList = sendlist; //抄送人列表
            var isres3 = EmailHelper.SendEmail(sendlist, "邮件发送", "邮件内容", ccList, "C:\\Windows\\regedit.exe".ToList()); //可发多个用户，抄送多个用户，及多个附件
        }

        /// <summary>
        /// 文本文件操作类 示例
        /// </summary>
        private static void FileHelperTest()
        {
            string filePath = "D:\\dir.txt";
            string filePath2 = "D:\\dir2.txt";
            //读取文本
            string content = FileHelper.ReadText(filePath);
            string content2 = FileHelper.ReadText(filePath2, Encoding.GetEncoding("GB2312"));

            StringBuilder sb = new StringBuilder();
            for (var i = 1; i < 10; i++)
            {
                sb.AppendLine("第" + i + "行！");
            }
            //写入文本
            FileHelper.WriteText(filePath, sb.ToString(), TextWriteType.Covered); //覆盖
            FileHelper.WriteText(filePath2, sb.ToString(), TextWriteType.Append, Encoding.GetEncoding("GB2312")); //追加
            //读取以检查写入
            content = FileHelper.ReadText(filePath);
            content2 = FileHelper.ReadText(filePath2, Encoding.GetEncoding("GB2312"));

            StringBuilder sb2 = new StringBuilder();
            //读取大文本文件
            FileHelper.ReadBigText(filePath, (x) => DebugHelper.WriteLine(x));
            FileHelper.ReadBigText(filePath2, (x) => sb2.AppendLine(x), Encoding.GetEncoding("GB2312"));

            string filePath3 = @"D:\bdlogo.png";
            var bt = HttpHelper.GetUrlByte("https://www.baidu.com/img/bd_logo1.png");
            //流文件读取写入
            FileHelper.WriteFile(filePath3, bt);
            var bt2 = FileHelper.ReadFile(filePath3);
        }

        /// <summary>
        /// HttpWebRequest请求工具类 示例
        /// </summary>
        private static void HttpHelperTest()
        {
            int timeOut = 30;
            string userAgent = "Mozilla/5.0 (Linux; U; Android 7.0; zh-CN) Mobile AppleWebKit/537.36 mtTools Demos V1.0.0.0";
            System.Net.CookieCollection cookies = null;

            //读取网页文本
            var bd = HttpHelper.GetUrlString("https://www.baidu.com");
            var bd2 = HttpHelper.GetUrlString("https://www.baidu.com", timeOut, Encoding.UTF8, userAgent, cookies);

            //下载内容字节流
            var bt = HttpHelper.GetUrlByte("https://www.baidu.com/img/bd_logo1.png");
            var bt2 = HttpHelper.GetUrlByte("https://www.baidu.com/img/bd_logo1.png", timeOut, userAgent, cookies);


            cookies = new System.Net.CookieCollection();
            cookies.Add(new System.Net.Cookie { Name = "BAIDUID", Value = "3F4B33C7D02A5F5F4A1B55978460CDB9:FG=1", Path = "/", Domain = "www.baidu.com" });
            cookies.Add(new System.Net.Cookie("name", "mt", "/", "www.baidu.com"));
            string content = "wd=mtTools";
            //Post提交数据并获取结果页文本
            var res = HttpHelper.PostUrlString("https://www.baidu.com/s", content);
            var res2 = HttpHelper.PostUrlString("https://www.baidu.com/s", content, timeOut, Encoding.UTF8, userAgent, cookies);
        }

        /// <summary>
        /// 日志处理类 示例
        /// </summary>
        private static void LogHelperTest()
        {
            //请于App.config或Web.config中自行配置如下信息，不配置则默认值如下
            //<configSections>
            //  ... ...
            //  <section name="toolLog" type="System.Configuration.NameValueSectionHandler" />
            //</configSections>
            //<!--LogHelper配置-->
            //<toolLog>
            //  <!--日志记录路径：绝对地址"D:\Logs\"或应用程序下目录"Logs/",留空为应用程序根目录下"Logs/"，目录不存在则自动创建-->
            //  <add key="LogPath" value="" />
            //  <!--日志内容格式：@Now-日期;@Message-消息内容-->
            //  <add key="LogMessage" value="【@Now】@Message" />
            //</toolLog>

            var path = LogHelper.appPath; //取config中配置的日志记录路径，可配置为D:\Logs\或Logs/Debug/格式，未配置则为应用程序根目录下"Logs/"

            //yyyyMMdd代表如20180102格式的当天日期字符串
            for (var i = 0; i < 100; i++)
            {
                LogHelper.Logs["Test"].WriteLine("Line:" + i.ToString() + "."); //Test/yyyyMMdd.txt
                LogHelper.Logs["", "Test"].WriteLine("Line:" + i.ToString() + "."); //Test.txt

                LogHelper.Logs["Test2", "Test53"].WriteLine("Line:" + i.ToString() + "."); //Test2/Test53.txt
                LogHelper.Logs["Test2/Test22", "Test54"].WriteLine("Line:" + i.ToString() + "."); //Test2/Test22/Test54.txt

                LogHelper.Logs["Test" + DateTime.Now.ToString("yyyyMMdd"), "Test53"].WriteLine("Line:" + i.ToString() + "."); //TestyyyyMMdd/Test53.txt
                LogHelper.Logs["Test" + DateTime.Now.ToString("yyyyMMdd"), "Test54"].WriteLine("Line:" + i.ToString() + "."); //TestyyyyMMdd/Test54.txt

                LogHelper.Logs["Test3", "Test2/roomid54"].WriteLine("Line:" + i.ToString() + "."); //Test3/Test2/roomid54.txt
                LogHelper.Logs["Test4", "Test2\\roomid54"].WriteLine("Line:" + i.ToString() + "."); //Test4/Test2/roomid54.txt
            }

            //获取日志主为非按日期生成的日志所提供，按日期生成的日志仅能获取当天日志
            //注意：为提升日志写入速度，日志是异步写入的，当程序执行到此时，日志并没有全部写入txt，故取出的日志并非100条
            var logs1 = LogHelper.Logs["Test"].GetAllLog(); //仅当天日志
            var logs2 = LogHelper.Logs["Test2", "Test53"].GetAllLog(); //全部日志
            var logs3 = LogHelper.Logs["", "Test"].GetAllLog(); //全部日志

            //日志片断（日志按调用写入的先后顺序有序记录）：
            //【2016-10-31 15:34:04】Line:0.
            //【2016-10-31 15:34:04】Line:1.
            //【2016-10-31 15:34:04】Line:2.
            //【2016-10-31 15:34:04】Line:3.
            //【2016-10-31 15:34:04】Line:4.
            //【2016-10-31 15:34:04】Line:5.
            //【2016-10-31 15:34:04】Line:6.
        }

        /// <summary>
        /// 类型转换扩展 示例
        /// </summary>
        private static void ConvertHelperTest()
        {
            //DataTable示例类型
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string)); //数据类型为 文本
            DataColumn age = new DataColumn("Age", typeof(Int32));   //数据类型为 整形
            DataColumn Time = new DataColumn("Time", typeof(DateTime)); //数据类型为 时间
            dt.Columns.Add(age);
            dt.Columns.Add(Time);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "张三"; //通过索引赋值
            dr1[1] = 23;
            dr1["Time"] = DateTime.Now;//通过名称赋值
            dt.Rows.Add(dr1);
            dt.Rows.Add("李四", 25, DateTime.Now);//Add你们参数的数据顺序要和dt中的列顺对应

            //DataSet示例类型
            var ds = new DataSet();
            ds.Tables.Add(dt);
            var dt2 = dt.Clone();
            dt2.TableName = "test2"; //DataSet中多DateTable并且是克隆时必须另命名表名
            ds.Tables.Add(dt2);



            //DataTable、DataSet类型转换
            var list = dt.ToList();
            var dts = list.ToDataTable();
            var list2 = ds.ToList();
            var dss = list2.ToDataSet();



            //Stream与byte[]互转
            var bt = HttpHelper.GetUrlByte("https://www.baidu.com/img/bd_logo1.png");
            var stream = bt.ToStream();
            var bt2 = stream.ToByte();
        }

        /// <summary>
        /// 调试输出至文件或调试面板 示例
        /// </summary>
        private static void DebugHelperTest()
        {
            //请于App.config或Web.config中自行配置如下信息，不配置则默认值如下
            //<configSections>
            //  ... ...
            //  <section name="toolDebug" type="System.Configuration.NameValueSectionHandler" />
            //</configSections>
            //<!--DebugHelper配置-->
            //<toolDebug>
            //  <!--是否输出调试信息-->
            //  <add key="IsDebug" value="true" />
            //  <!--调试文件路径及文件前缀：绝对地址"D:\Logs\"或应用程序下目录"Logs/",留空为应用程序根目录下"Logs/Debug/",自动后跟yyyyMMdd.txt为路径，可为"Logs/Debug_，目录不存在则自动创建"-->
            //  <add key="DebugPath" value="" />
            //  <!--调试信息内容格式：@Now-日期;@CodeLocal-出错位置;@Message-消息内容-->
            //  <add key="DebugMessage" value="【@Now】【@CodeLocal】@Message" />
            //</toolDebug>

            var isDebug = DebugHelper.IsDebug; //配置的是否输出调试信息，默认为true
            var debugPath = DebugHelper.DebugPath; //配置的调试文件路径及文件前缀

            try
            {
                "测试的错误信息".WriteLine("DebugHelperTest", "没什么参数或附加记录信息");//输出到输出面板
                "测试的错误信息".WriteFile("DebugHelperTest", "没什么参数或附加记录信息");//输出到Debug文件
                DebugHelper.WriteLine("测试的错误信息", "DebugHelperTest", "没什么参数或附加记录信息");
                DebugHelper.WriteFile("测试的错误信息", "DebugHelperTest", "没什么参数或附加记录信息");

                Convert.ToInt32("abc");
            }
            catch (Exception ex)
            {
                ex.WriteLine("DebugHelperTest", "没什么参数或附加记录信息");
                ex.WriteFile("DebugHelperTest", "没什么参数或附加记录信息");
            }
        }

        /// <summary>
        /// 常用加解密工具类 示例
        /// </summary>
        private static void EncodeHelperTest()
        {
            //MD5
            string str = "MD5加密算法".MD5();
            string str2 = "MD5加密算法".MD5(Encoding.ASCII);
            string str3 = "MD5加密算法".MD5(Encoding.UTF8);

            //SHA1
            string str1 = "SHA1加密算法".SHA1();
            string str12 = "SHA1加密算法".SHA1(Encoding.ASCII);
            string str13 = "SHA1加密算法".SHA1(Encoding.UTF8);
        }

        /// <summary>
        /// Json序列化与反序列化 示例
        /// </summary>
        private static void JsonHelperTest()
        {

            //基础示例类型
            var t1 = new testEntity();
            t1.name = "test1";
            t1.password = "123456";
            t1.Age = 18;
            t1.now = DateTime.Now;
            t1.ip = new IP();
            t1.ip.host = "rock";
            t1.ip.ipv4 = "192.168.1.6";

            var t2 = new testEntity();
            t2.name = "test2";
            t2.password = "123456";
            t2.Age = 18;
            t2.now = DateTime.Now;
            t2.ip = null;

            var list = new List<testEntity>();
            list.Add(t1);
            list.Add(t2);



            //基础示例类型序列化与反序列化
            var s1 = t1.JSONSerialize();
            var s2 = t2.JSONSerialize();
            var s3 = list.JSONSerialize(false);

            var d1 = s1.JSONDeserialize<testEntity>();
            testEntity d2 = s2.JSONDeserialize<testEntity>();
            var d3 = s3.JSONDeserialize<testEntity>();//null：类型不对应
            List<testEntity> b2 = s3.JSONDeserialize<List<testEntity>>();



            //匿名类型及其序列化
            var obj = new { name = "123", pas = "456", a = new { ip = "127.0.0.1", date = DateTime.Now } };
            var st = obj.JSONSerialize();



            //DataTable示例类型
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string)); //数据类型为 文本
            DataColumn age = new DataColumn("Age", typeof(Int32));   //数据类型为 整形
            DataColumn Time = new DataColumn("Time", typeof(DateTime)); //数据类型为 时间
            dt.Columns.Add(age);
            dt.Columns.Add(Time);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "张三"; //通过索引赋值
            dr1[1] = 23;
            dr1["Time"] = DateTime.Now;//通过名称赋值
            dt.Rows.Add(dr1);
            dt.Rows.Add("李四", 25, DateTime.Now);//Add你们参数的数据顺序要和dt中的列顺对应

            //DataTable示例类型序列化与反序列化
            var s4 = dt.JSONSerialize();

            var a3 = s4.JSONDeserialize<DataTable>();//OK
            var a5 = s4.JSONDeserialize<List<Dictionary<string, object>>>();//OK
            var a4 = s4.JSONDeserializeDynamic();//dynamic {object[]}



            //反序列化为动态类型时取值操作示例
            var result = "";

            var m1 = s1.JSONDeserializeDynamic();//dynamic {Dictionary<string,object>}
            if (((Dictionary<string, object>)m1).ContainsKey("name"))
                result += m1["name"].ToString();//test1

            var m2 = s3.JSONDeserializeDynamic();//dynamic {object[]}
            if (m2.Length > 0 && m2[0].Count > 0 && ((Dictionary<string, object>)m2[0]).ContainsKey("name"))//m2[0] dynamic {Dictionary<string,object>}
                result += m2[0]["name"].ToString();//test1

        }

        /// <summary>
        /// Xml序列化与反序列化 示例
        /// </summary>
        private static void XmlHelperTest()
        {

            //示例类型
            var t1 = new testEntity();
            t1.name = "test1";
            t1.password = "123456";
            t1.Age = 18;
            t1.now = DateTime.Now;
            t1.ip = new IP();
            t1.ip.host = "rock";
            t1.ip.ipv4 = "192.168.1.6";

            var t2 = new testEntity();
            t2.name = "test2";
            t2.password = "123456";
            t2.Age = 18;
            t2.now = DateTime.Now;
            t2.ip = null;

            var list = new List<testEntity>();
            list.Add(t1);
            list.Add(t2);



            //Xml序列化
            var xmlt = t1.GetXmlString();
            var xmlt1 = t1.GetXmlString(Encoding.UTF8);
            var xmlt2 = t2.GetXmlString(Encoding.UTF8, true);
            var xmlt3 = t2.GetXmlString(Encoding.UTF8, true, false);
            var xmlt4 = t2.GetXmlString(Encoding.UTF8, true, false, true);
            var xmlt_1 = list.GetXmlString();

            //Xml反序列化
            var ttt1 = xmlt1.GetXmlObj<testEntity>();
            var ttt2 = xmlt2.GetXmlObj<testEntity>();
            var ttt3 = xmlt3.GetXmlObj<testEntity>();
            var ttt4 = xmlt4.GetXmlObj<testEntity>();
            var ttt_1 = xmlt_1.GetXmlObj<List<testEntity>>();



            //GBK格式Xml序列化
            var xmlf1 = t1.GetGBKXmlString();
            var xmlf2 = t1.GetGBKXmlString(Encoding.UTF8);
            var xmlf4 = t2.GetXmlString(Encoding.UTF8, true, false, true);



            //DataTable示例类型
            DataTable dt = new DataTable();
            dt.TableName = "test1"; //xmlHelper操作DataTable时必须有表名否则会报错
            dt.Columns.Add("Name", typeof(string)); //数据类型为 文本
            DataColumn age = new DataColumn("Age", typeof(Int32));   //数据类型为 整形
            DataColumn Time = new DataColumn("Time", typeof(DateTime)); //数据类型为 时间
            dt.Columns.Add(age);
            dt.Columns.Add(Time);
            DataRow dr1 = dt.NewRow();
            dr1[0] = "张三"; //通过索引赋值
            dr1[1] = 23;
            dr1["Time"] = DateTime.Now;//通过名称赋值
            dt.Rows.Add(dr1);
            dt.Rows.Add("李四", 25, DateTime.Now);//Add你们参数的数据顺序要和dt中的列顺对应

            //DateTable的Xml序列化和反序列化
            var dtxml = dt.GetXmlString();
            var dt2 = dtxml.GetXmlObj<DataTable>();
        }

        /// <summary>
        ///  文本操作扩展 常用字符串格式转换 示例
        /// </summary>
        private static void StringHelperTest()
        {
            string str1 = "1234";
            string str2 = "abcd啊啊345bc";
            string str3 = Guid.NewGuid().ToString();
            int? int1 = 4;


            //类型判断与转换，常用类型皆以此命名：仅列举，不作详情测试
            var t1 = str1.IsNullOrEmpty();
            var t2 = str2.IsNullOrWhiteSpace();
            var t3 = str2.IsDateTime();
            var t4 = str1.IsBool();
            var t5 = int1.IsNullOrEmpty(); //0为Empty
            var t6 = str1.IsChinese(); //是否含有中文
            var t7 = str2.IsChinese(); //是否含有中文

            var i7 = str3.IsGuid(); //32位与36位皆可
            var r7 = str3.ToGuid();

            var i8 = str1.IsBool(); //支持数字(数字会转为int后ToBoolean)
            var r8 = str1.ToBool();
            
            var i1 = str1.IsInt();
            var r1 = str1.ToInt();

            var i2 = str1.IsDecimal();
            var r2 = str1.ToDecimal();

            var i3 = str1.IsDouble();
            var r3 = str1.ToDouble();

            var i4 = str1.IsLong();
            var r4 = str1.ToLong();

            var i5 = str1.IsFloat();
            var r5 = str1.ToFloat();

            var i6 = str1.IsByte();
            //var r6 = str1.ToByte();


            //时间转换，兼容时间戳
            var time = DateTime.Now;

            var timeStr = time.ToString("yyyy-MM-dd HH:mm:ss");
            var dt = timeStr.ToDateTime();
            
            var timeUnix = time.ToLong();
            var timeUnix2 = time.ToLong(true);
            var dt1 = timeUnix.ToDateTime();
            var dt2 = timeUnix2.ToDateTime(false);
            var dt3 = timeUnix.ToString().ToDateTime(true);
            var dt4 = timeUnix2.ToString().ToDateTime(true, false);


            //其它扩展操作
            var res3 = 100000.GetRandomString(); //生成随机数
            
            var list = str1.ToList(); //字符串添加至new list<string>中转为此类型list<string>

            //从指定字符串开始截取后面所有字符串
            var s1 = str2.Substring("b");   //前第一个匹配项开始截取
            var s2 = str2.Substring("b", false);    //最后一个匹配项开始截取

            //从指定字符串之后截取指定长度字符串，指定字符串后长度不足则可选全部截取或返回""
            var s3 = str2.Substring("啊啊", 3);
            var s4 = str2.Substring("啊啊", 6); //如果截取的长度不足，默认返回""
            var s5 = str2.Substring("啊啊", 6, true); //为true则截取后面全部字符

            //获取字符串分隔的key value集合
            var strSp = "title:abcd;caption:23453545;content:45678:kkljkk";
            var dic = strSp.GetSplitValue(); //默认以半角分号分隔，半角冒号分隔name与value
            var dic2 = strSp.GetSplitValue(';', ':'); //指定分隔符

            //SubstringZH
            var strTemp = "abcdefgklqkie啊顺罼中地吉基加协 膛加戈 另国中加盟虽别人是j是辊是....,,,,，，，，。。。。！!@#$%*()kfldlkf";
            var st = strTemp.SubstringChinese(25); //截取25个汉字长度的字符串，结尾默认加以省略号
            var st2 = strTemp.SubstringChinese(25, "。。。"); //截取25个汉字长度的字符串，结尾加以。。。
            var st3 = strTemp.SubstringChinese(10, 25); //从第10个字符开始截取25个汉字长度的字符串，结尾默认加以省略号
            var st4 = strTemp.SubstringChinese(10, 35, "。。。"); //从第10个字符开始截取25个汉字长度的字符串，结尾加以。。。

        }

    }


    public class IP
    {
        public string host { get; set; }
        public string ipv4 { get; set; }
    }
    public class testEntity
    {

        public string name { get; set; }
        public string password { get; set; }
        public int Age { get; set; }
        public DateTime now { get; set; }

        public IP ip { get; set; }
    }

}