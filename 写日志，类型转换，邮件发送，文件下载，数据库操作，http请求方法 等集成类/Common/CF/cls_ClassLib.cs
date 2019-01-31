using System;
using System.Data;
using System.Collections;
using System.Collections.Concurrent;
using System.Configuration;
using System.Net;


namespace CF
{
    /// <summary>
    /// cls_ClassLib 的摘要说明。
    /// </summary>
    public class cls_ClassLib
    {


        private static string conn_sqlite;
        /// <summary>
        /// 应用程序运行标志
        /// </summary>
        public static bool RunFlag = true;

        /// <summary>
        /// 强联网同步
        /// </summary>
        public static string QiangLianWangSync = ConfigurationManager.AppSettings["QiangLianWangSync"];

        /// <summary>
        /// 弱联网同步
        /// </summary>
        public static string SvmcpSync = ConfigurationManager.AppSettings["SvmcpSync"];

        /// <summary>
        /// 弱联网上行验证
        /// </summary>
        public static string SvmcpMoValidate = ConfigurationManager.AppSettings["SvmcpMoValidate"];



        /// <summary>
        /// 是否可以获取强联网Token值 0为不获取 1为获取
        /// </summary>
        public static bool IsGetToken = ConfigurationManager.AppSettings["IsGetToken"] == "1";


        /// <summary>
        /// 是否执行多线程 0为不获取 1为获取
        /// </summary>
        public static bool IsDoThread = ConfigurationManager.AppSettings["IsDoThread"] == "1";

        public static string OrderStart = ConfigurationManager.AppSettings["OrderStart"] ?? "";


        public static string BaoYueOrderStart = ConfigurationManager.AppSettings["BaoYueOrderStart"] ?? "";

        /// <summary>
        /// 内存SP业务表
        /// </summary>
        public static DataTable dt_tur_ServiceProvider = new DataTable();

        /// <summary>
        /// SP省份配置
        /// </summary>
        public static DataTable dt_tur_SpProvinceConfig = new DataTable();

        /// <summary>
        /// SP分省
        /// </summary>
        public static DataTable dt_tur_SpProvince = new DataTable();

        /// <summary>
        /// 通道列表
        /// </summary>
        public static DataTable dt_tur_R_type = new DataTable();

        /// <summary>
        /// 未知号段查询测试
        /// </summary>
        //public static DataTable dt_tsp_ChinaMobileCode = new DataTable();

        /// <summary>
        /// 通过手机号前三位判断运营商
        /// </summary>
        public static DataTable dt_tbl_net_check = new DataTable();
        /// <summary>
        /// 业务资料
        /// </summary>
        public static DataTable dt_tbl_AppUserRelation = new DataTable();

        /// <summary>
        /// sp与app对应关系
        /// </summary>
        public static DataTable dt_tur_CpAndSpRelation = new DataTable();
        /// <summary>
        /// 线程休眠时间
        /// </summary>
        public static int ThreadSleepTime = 1 * 60 * 1000;

        /// <summary>
        /// IP限制策略
        /// </summary>
        public static ConcurrentDictionary<string, string> ht_Globe_AllowIP = new ConcurrentDictionary<string, string>();
        /// <summary>
        /// 手机黑名单
        /// </summary>
        public static ConcurrentDictionary<string, string> ht_Globe_BlockMobileNo = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// IMSI黑名单
        /// </summary>
        public static ConcurrentDictionary<string, string> ht_Globe_BlockIMSI = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// 1级分割
        /// </summary>
        public static char SPLIT_CHAR_LEVEL1 = (char)0x1A;

        /// <summary> 
        /// 2级分割
        /// </summary>
        public static char SPLIT_CHAR_LEVEL2 = (char)0x19;

        /// <summary>
        /// 3级分割
        /// </summary>
        public readonly static char SPLIT_CHAR_LEVEL3 = (char)0x18;

        /// <summary>
        /// 1级分割
        /// </summary>
        public const string SPLIT_STRING_LEVEL1 = "№Ⅰ";

        /// <summary> 
        /// 2级分割
        /// </summary>
        public const string SPLIT_STRING_LEVEL2 = "№Ⅱ";

        /// <summary>
        /// 3级分割
        /// </summary>
        public const string SPLIT_STRING_LEVEL3 = "№Ⅲ";

        /// <summary>
        /// IP黑名单
        /// </summary>
        public static Hashtable hs_BlockIP;

        /// <summary>
        /// 全局随机数序列,避免由于cpu时钟太快导致取出相同的随机数
        /// </summary>
        public static Random GlobeRandom = new Random();

        /// <summary>
        /// 旅游数据库连接串
        /// </summary>
        public static string CONN_XLC = System.Configuration.ConfigurationManager.AppSettings["conn_xlc"];

        /// <summary>
        /// 是否使用proxy
        /// </summary>
        public static bool IsUseProxy = System.Configuration.ConfigurationManager.AppSettings["IsUseProxy"] == "1" ? true : false;

        /// <summary>
        /// ProxyIP列表
        /// </summary>
        public static DataTable dt_ProxyIPList = new DataTable();

        /// <summary>
        /// 小说数据库连接串
        /// </summary>
        //public static string CONN_BOOK = "User ID=web5ku;Password=#aspchery342*~;data source=124.237.121.54;database=BookService;Enlist=true;"
        //        + "Pooling=true;Max Pool Size=512;Min Pool Size=0;Connection Lifetime=300;packet size=1000;";

        /// <summary>
        /// 小说数据库连接串
        /// </summary>
        public static string CONN_IFree = System.Configuration.ConfigurationManager.AppSettings["CONN_IFree"];

        /// <summary>
        /// 小游戏数据库连接串
        /// </summary>
        public static string CONN_GAME = System.Configuration.ConfigurationManager.AppSettings["conn_game"];

        /// <summary>
        /// 斗地主
        /// </summary>
        public static string CONN_DDZ = System.Configuration.ConfigurationManager.AppSettings["CONN_DDZ"];

        /// <summary>
        /// 法律条文数据库
        /// </summary>
        public static string CONN_LAW = System.Configuration.ConfigurationManager.AppSettings["conn_law"];

        /// <summary>
        /// 律网综合数据库
        /// </summary>
        public static string CONN_LAWCOMMON = System.Configuration.ConfigurationManager.AppSettings["conn_lawcommon"];

        /// <summary>
        /// SNSPT数据库
        /// </summary>
        public static string CONN_SNSPT = System.Configuration.ConfigurationManager.AppSettings["CONN_SNSPT"];

        /// <summary>
        /// WapDb数据库
        /// </summary>
        public static string CONN_WAP = System.Configuration.ConfigurationManager.AppSettings["CONN_WAP"];

        /// <summary>
        /// SNSPT数据库
        /// </summary>
        public static string CONN_EP = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];


        /// <summary>
        /// SNSPT数据库
        /// </summary>
        public static string ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>
        /// 三合一使用Ep数据库
        /// </summary>
        public static string CONN_EPDB = System.Configuration.ConfigurationManager.AppSettings["CONN_EPDB"];

        /// <summary>
        /// SDK更新地址
        /// </summary>
        public static string SdkUpdateUrl = System.Configuration.ConfigurationManager.AppSettings["SdkUpdateUrl"];


        /// <summary>
        /// MMMarketXml
        /// </summary>
        public static string CONN_MMSyncAppOrder = System.Configuration.ConfigurationManager.AppSettings["CONN_MMSyncAppOrder"];

        /// <summary>
        /// 发送短信获取手机号码端口
        /// </summary>
        public static string MZ12114Port = System.Configuration.ConfigurationManager.AppSettings["MZ12114Port"];
        /// <summary>
        /// 发送短信获取手机号码内容
        /// </summary>
        public static string MZ12114Contents = System.Configuration.ConfigurationManager.AppSettings["MZ12114Contents"];

        /// <summary>
        /// 亿科同步订单Url
        /// </summary>
        public static string YiKeOrderDeliverUrl = System.Configuration.ConfigurationManager.AppSettings["YiKeOrderDeliverUrl"];

        /// <summary>
        /// 亿科同步订单报告Url
        /// </summary>
        public static string YiKeOrderReportUrl = System.Configuration.ConfigurationManager.AppSettings["YiKeOrderReportUrl"];

        /// <summary>
        /// 得途Wo+同步地址
        /// </summary>
        public static string DetuWo = System.Configuration.ConfigurationManager.AppSettings["DetuWo"];

        /// <summary>
        /// 势至文化Wo+同步地址
        /// </summary>
        public static string ShiZhiWo = System.Configuration.ConfigurationManager.AppSettings["ShiZhiWo"];

        /// <summary>
        /// SP通道地址
        /// </summary>
        public static string SpChannelInterfaceUrl =
            System.Configuration.ConfigurationManager.AppSettings["SpChannelInterfaceUrl"];

        /// <summary>
        /// SNSPT数据库
        /// </summary>
        public static string CONN_MM = System.Configuration.ConfigurationManager.AppSettings["CONN_MM"];

        //MMUrl
        public static string MMUrl = System.Configuration.ConfigurationManager.AppSettings["MMUrl"];

        //MMUrlSecond
        public static string MMUrlSecond = System.Configuration.ConfigurationManager.AppSettings["MMUrlSecond"];

        /// <summary>
        /// SNSPT数据库
        /// </summary>
        public static string CONN_DM = System.Configuration.ConfigurationManager.AppSettings["CONN_DM"];

        /// <summary>
        /// MM和动漫的请求
        /// </summary>
        public static string MmAndDm = System.Configuration.ConfigurationManager.AppSettings["MmAndDm"];

        /// <summary>
        /// 森林跑跑熊
        /// </summary>
        public static string TeleComUrl = System.Configuration.ConfigurationManager.AppSettings["TeleComUrl"];



        /// <summary>
        /// 英雄出击
        /// </summary>
        public static string YingXiongChujiUrl = System.Configuration.ConfigurationManager.AppSettings["YingXiongChujiUrl"];
        /// <summary>
        ///提交验证码
        /// </summary>
        public static string SubmitVCodeUrl = System.Configuration.ConfigurationManager.AppSettings["SubmitVCodeUrl"];

        /// <summary>
        /// 天翼泡泡破解地址
        /// </summary>
        public static string TianYiPaoPaoUrl = System.Configuration.ConfigurationManager.AppSettings["TianYiPaoPaoUrl"];

        /// <summary>
        /// 天翼泡泡豪华版地址
        /// </summary>
        public static string TianYiPaoPaoHaoHuaUrl = System.Configuration.ConfigurationManager.AppSettings["TianYiPaoPaoHaoHuaUrl"];

        /// <summary>
        /// 森林跑跑熊
        /// </summary>
        public static string MyInterFaceUrl = System.Configuration.ConfigurationManager.AppSettings["MyInterFaceUrl"];

        /// <summary>
        /// 森林跑跑熊扣费通知连接串
        /// </summary>
        public static string CONN_Slppx = System.Configuration.ConfigurationManager.AppSettings["ConnSlppx"];

        /// <summary>
        /// 电信接口数据库
        /// </summary>
        public static string CONN_TeleCom = System.Configuration.ConfigurationManager.AppSettings["CONN_TeleCom"];



        /// <summary>
        /// EP_TeleComs
        /// </summary>
        public static string EP_TeleCom = System.Configuration.ConfigurationManager.AppSettings["EP_TeleCom"];


        /// <summary>
        /// 数据转换数据库PG
        /// </summary>
        public static string datatransfer_pg = System.Configuration.ConfigurationManager.AppSettings["datatransfer_pg"];

        /// <summary>
        /// 数据转换数据库SQL
        /// </summary>
        public static string datatransfer_sql = System.Configuration.ConfigurationManager.AppSettings["datatransfer_sql"];

        /// <summary>
        /// PGSql的Book连接串
        /// </summary>
        public static string CONN_BOOK_PG = System.Configuration.ConfigurationManager.AppSettings["conn_book_pg"];

        /// <summary>
        /// PGSql的Book连接串
        /// </summary>
        public static string CONN_GW = System.Configuration.ConfigurationManager.AppSettings["conn_gw"];

        /// <summary>
        /// 亿部的fgsj连接串
        /// </summary>
        public static string CONN_WFSJ = System.Configuration.ConfigurationManager.AppSettings["CONN_WFSJ"];

        /// <summary>
        /// MDO获取后四位
        /// </summary>
        public static string CONN_RamStr = System.Configuration.ConfigurationManager.AppSettings["CONN_RamStr"];

        /// <summary>
        /// MongoDB用户名
        /// </summary>
        public static string MONGO_USERNAME = System.Configuration.ConfigurationManager.AppSettings["mongo_username"];

        /// <summary>
        /// MongoDB密码
        /// </summary>
        public static string MONGO_PASSWORD = System.Configuration.ConfigurationManager.AppSettings["mongo_password"];

        /// <summary>
        /// MongoDB连接串
        /// </summary>
        public static string CONN_MONGO = System.Configuration.ConfigurationManager.AppSettings["conn_mongo"];

        /// <summary>
        /// 是否启用缓存
        /// </summary>
        public static bool IsUseCache = System.Configuration.ConfigurationManager.AppSettings["IsUseCache"] == "1" ? true : false;


        /// <summary>
        /// 是否启用内存缓存
        /// </summary>
        public static bool IsUseMemCache = System.Configuration.ConfigurationManager.AppSettings["IsUseMemCache"] == "1" ? true : false;


        public static int MemCacheTime = CF.cls_Common.GetInt(System.Configuration.ConfigurationManager.AppSettings["MemCacheTime"]) > 0 ?
                                                    CF.cls_Common.GetInt(System.Configuration.ConfigurationManager.AppSettings["MemCacheTime"]) : 20;

        /// <summary>
        /// Sqlite数据库连接串
        /// </summary>
        public static string CONN_SQLITE
        {
            get
            {
                if (conn_sqlite == null)
                {
                    conn_sqlite = "Data Source="
                        + System.Web.Hosting.HostingEnvironment.MapPath(System.Configuration.ConfigurationManager.AppSettings["SqlitePath"])
                        + ";Pooling=true;FailIfMissing=false;Compress=True;Journal Mode=Off;";
                }

                return conn_sqlite;
            }

            set
            {
                if (System.Configuration.ConfigurationManager.AppSettings["SqlitePath"] == null)
                {

                    conn_sqlite = "";
                }
                else
                {
                    conn_sqlite = "Data Source="
                        + System.Web.Hosting.HostingEnvironment.MapPath(System.Configuration.ConfigurationManager.AppSettings["SqlitePath"])
                        + ";Pooling=true;FailIfMissing=false;Compress=True;Journal Mode=Off;";
                }
            }
        }

        /// <summary>
        /// 站点域名
        /// </summary>
        public static string SiteDomain = System.Configuration.ConfigurationManager.AppSettings["sitedomain"];

        /// <summary>
        /// Sqlite数据库连接串
        /// </summary>
        public static string CONN_SQLITE_MEM;

        /// <summary>
        /// passport访问口令
        /// </summary>
        public static string AccessKey = System.Configuration.ConfigurationManager.AppSettings["PassportAccessKey"];


        /// <summary>
        /// Common访问口令
        /// </summary>
        public const string CommonKey = "kubaobjadmin12";



        /// <summary>
        /// 用户信息
        /// </summary>
        [Serializable]
        public class UserInfo
        {
            /// <summary>
            /// 用户名
            /// </summary>
            public string UserName = string.Empty;

            /// <summary>
            /// 密码
            /// </summary>
            public string Password = string.Empty;

            /// <summary>
            /// 用户等级
            /// </summary>
            public int UserLevel = 0;

            /// <summary>
            /// 性别 true为男
            /// </summary>
            public bool UserSex = true;

            /// <summary>
            /// 用户编号
            /// </summary>
            public int AssID = 0;

            /// <summary>
            /// 手机号
            /// </summary>
            public string MobileNo = string.Empty;

            /// <summary>
            /// 昵称
            /// </summary>
            public string NiceName = string.Empty;

            /// <summary>
            /// 电子邮件地址
            /// </summary>
            public string Email = string.Empty;

            /// <summary>
            /// 用当前连接的SessionID
            /// </summary>
            public string SessionID = string.Empty;
        }




        /// <summary>
        /// passport传递参数时候进行MD5加密验证的key
        /// </summary>
        public static string PassportMd5Key = "sdfd1230sdsfrewvkdfs";

        /// <summary>
        /// 过虑字符串,系统禁止使用的字符串
        /// </summary>
        public static string HoldKeywords = "FUCK,江泽民,江主席,李登辉,毛主席,毛泽东,周恩来,周总理,李鹏,总理,主席,总统,法轮功,法轮大法,处女膜,龟头,阴茎,生殖器,鸡巴,阴道,操你妈,强奸,乳房,妓,胡锦涛,温家宝,温家保,锦涛,家宝,家保";

        /// <summary>
        /// 网站间参数传递时候用到的3des加密密钥
        /// </summary>
        public readonly static string EncryptNormal_KEY = "keyforkubaonormal3deskey";

        /// <summary>
        /// 网站间参数传递时候用到的3des加密密钥
        /// </summary>
        public readonly static string EncryptNormal_IV = "asdf91d&";


        /// <summary>
        /// 被禁止的IP信息
        /// </summary>
        public class BlockIPInfo
        {
            public string Domain { get; set; }
            public string IP { get; set; }
            public int Visit { get; set; }
            public string UA { get; set; }
        }

        public static DataTable dt_ADUrlList = new DataTable();

        /// <summary>
        /// 内存高速缓存
        /// </summary>
        public static CF.cls_Cache MemCache;


        /// <summary>
        /// 内存缓存Key的名字
        /// </summary>
        public static class MemCachekey
        {
            /// <summary>
            /// 双哈希词根
            /// </summary>
            public static string DHT_EtymaWords = "DHT_EtymaWords";

            /// <summary>
            /// 双哈希垃圾分割词
            /// </summary>
            public static string DHT_WasteWords = "DHT_WasteWords";

            /// <summary>
            /// 短信过滤规则
            /// </summary>
            public static string SMSFilterRuleString = "SMSFilterRuleString";

            /// <summary>
            /// 二次短信规则
            /// </summary>
            public static string SMSTwoRuleString = "SMSTwoRuleString";

            /// <summary>
            /// Key: MobileNo value : GWUser
            /// </summary>
            public static string MobileNo_GWUser = "MobileNo_GWUser";
        }

        /// <summary>
        /// 抓取网页的返回结构
        /// </summary>
        public class HttpResult
        {
            /// <summary>
            /// Http状态
            /// </summary>
            public System.Net.HttpStatusCode StatusCode = HttpStatusCode.BadRequest;

            /// <summary>
            /// 内容
            /// </summary>
            public string HtmlContents = "";

            /// <summary>
            /// 用时
            /// </summary>
            public int UseTime = 0;

            /// <summary>
            /// 错误信息
            /// </summary>
            public string ErroMsg = "";

            /// <summary>
            /// 真实地址
            /// </summary>
            public string RealUrl = "";
        }

        /// <summary>
        /// 网站抓取域名速度评分表
        /// </summary>
        public static ConcurrentDictionary<string, int> ht_FastDomain = new ConcurrentDictionary<string, int>();

        /// <summary>
        /// 缓存对象表名
        /// </summary>
        public static class CacheTableName
        {
            //---------------通用的缓存

            /// <summary>
            /// 通用缓存
            /// </summary>
            public static readonly string CacheTable = "cachetable";
        }

        /// <summary>
        /// ProgramWeb中Sdk下载地址
        /// </summary>
        public static string SdkZipName = System.Configuration.ConfigurationManager.AppSettings["SdkZipName"];


        public static string MainServerIp = System.Configuration.ConfigurationManager.AppSettings["MainServerIp"];

        public static string ChuanTongDianXinSyncUrl = System.Configuration.ConfigurationManager.AppSettings["ChuanTongDianXinSyncUrl"];

        public static string IsExcuteStatistics = ConfigurationManager.AppSettings["IsExcuteStatistics"];

        public static string SpId = ConfigurationManager.AppSettings["spId"];

        /// <summary>
        /// 是否同步手机号 1.执行 其他不执行
        /// </summary>
        public static int IsSyncUserPhone = CF.cls_Common.GetInt(ConfigurationManager.AppSettings["IsSyncUserPhone"] ?? "");
        /// <summary>
        /// 是否更新王飞包月的退订时间 1:更新 其他不更新
        /// </summary>
        public static int IsUpdateWangFeiBaoYueCancelDate = CF.cls_Common.GetInt(ConfigurationManager.AppSettings["IsWangFeiBaoYueCancelDate"] ?? "");
        //CONN_Config        

        /// <summary>
        /// 信元3是否执行分省份扣量 1.执行 其他不执行
        /// </summary>
        public static int IsProKou = CF.cls_Common.GetInt(ConfigurationManager.AppSettings["IsProKou"] ?? "");

        /// <summary>
        /// 是否执行调试模式 1.执行 其他不执行
        /// </summary>
        public static int IsTiaoShi = CF.cls_Common.GetInt(ConfigurationManager.AppSettings["IsTiaoShi"] ?? "");
        public class ErrorCode
        {
            public static int CpIdEmptyCode = 1001;
            public static string CpidEmptyMsg = "cpid不能为空";

            public static int CpIdErrorCode = 1002;
            public static string CpidErrorMsg = "cpid不存在";


            public static int ImsiEmptyCode = 1003;
            public static string ImsiEmptyMsg = "Imsi有误或为空";

            public static int ImeiEmptyCode = 1004;
            public static string ImeiEmptyMsg = "Imei不能为空";

            public static int FeeErrorCode = 1005;
            public static string FeeErrorMsg = "金额不正确";

            public static int MobileEmptyCode = 1006;
            public static string MobileEmptyMsg = "手机号为空或未解析到手机号";


            public static int AppIdErrorCode = 1007;
            public static string AppIdErrorMsg = "appid不存在";

            public static int FeeIdErrorCode = 1008;
            public static string FeeIdErrorMsg = "计费点不存在";

            public static int SpCmdGetErrorCode = 1009;
            public static string SpCmdGetErrorMsg = "获取指令失败";


            public static int OrderErrorCode = 1010;
            public static string OrderErrorMsg = "订单校验失败";

            public static int ImsiBlackCode = 1011;
            public static string ImsiBlackMsg = "IMSI为黑名单用户";

            public static int OrderIdEmptyCode = 1012;
            public static string OrderIdEmptyMsg = "订单号不能为空";

            public static int CodeEmptyCode = 1013;
            public static string CodeEmptyMsg = "验证码不能为空";

            public static int CpidSidCode = 1014;
            public static string CpidSidMsg = "渠道没有开通该业务";

            public static int CpidDayLimitCode = 1015;
            public static string CpidDayLimitMsg = "渠道日限禁止订购";

            public static int CpidMonthLimitCode = 1016;
            public static string CpidMonthLimitMsg = "渠道月限禁止订购";

            public static int UserBaoYueLimitCode = 1017;
            public static string UserBaoYueLimitMsg = "该用户已经订购该业务";

            public static int ProvinceShieldCode = 1018;
            public static string ProvinceShieldMsg = "该省份已屏蔽";

            public static int FeeEmptyCode = 1019;
            public static string FeeEmptyMsg = "金额不能为空";

            public static int OrderOrProvinceCode = 1020;
            public static string OrderOrProvinceMsg = "未分配指令（不支持的省份）";


            public static int SpDayLimitCode = 1021;
            public static string SpDayLimitMsg = "业务日限禁止订购";

            public static int SpMonthLimitCode = 1022;
            public static string SpMonthLimitMsg = "业务月限禁止订购";

            public static int FeeIdEmptyCode = 1023;
            public static string FeeIdEmptyMsg = "计费点不能为空";


            public static int IpEmptyCode = 2001;
            public static string IpEmptyMsg = "IP不能为空";


            public static int AppversionEmptyCode = 2002;
            public static string AppversionEmptyMsg = "版本号不能为空";

            public static int MacaddressEmptyCode = 2003;
            public static string MacaddressEmptyMsg = "MAC地址不能为空";


            public static int CompanyEmptyCode = 2004;
            public static string CompanyEmptyMsg = "公司名不能为空";

            public static int AppNameEmptyCode = 2005;
            public static string AppNameEmptyMsg = "应用名不能为空";

            public static int AppSubjectEmptyCode = 2006;
            public static string AppSubjectEmptyMsg = "道具名不能为空";

            public static int GetSpCmdError = 2007;
            public static string GetSpCmdErrorMsg = "申请指令失败";

            public static int SpCloseErrorCode = 2008;
            public static string SpCloseErrorMsg = "未配置通道或通道关闭";

            public static int GetVcodeErrorCode = 2009;
            public static string GetVcodeErrorMsg = "获取验证码失败";


            public static int SubVcodeErrorCode = 2010;
            public static string SubVcodeErrorMsg = "提交验证码失败";

            public static int PackgeEmpityCode = 2011;
            public static string PackgeEmpityMsg = "包名不能为空";

            public static int ApkCheckInEmpityCode = 2012;
            public static string ApkCheckInEmpityMsg = "特征码不能为空";

            public static int IccidEmpityCode = 2013;
            public static string IccidEmpityMsg = "手机iccid不能为空";

            public static int HttpEmptyCode = 9999;
            public static string HttpEmptyMsg = "运营商接口返回为空";


            public static int UnkownErrorCode = 9888;
            public static string UnkownErrorMsg = "未知错误";
        }
    }
}
