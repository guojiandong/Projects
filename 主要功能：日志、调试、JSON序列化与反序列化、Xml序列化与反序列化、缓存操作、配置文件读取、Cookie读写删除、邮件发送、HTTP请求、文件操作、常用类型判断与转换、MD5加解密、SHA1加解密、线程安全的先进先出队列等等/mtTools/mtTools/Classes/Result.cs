
namespace mtTools
{
    /// <summary>
    /// 简单操作返回类
    /// </summary>
    public class Result
    {

        /// <summary>
        /// 返回代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 附加消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 附加内容
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// 简单操作返回类
        /// </summary>
        public Result()
        {
            Code = 0;
            Message = "";
            Content = null;
        }

        /// <summary>
        /// 简单操作返回类
        /// </summary>
        /// <param name="code">返回代码</param>
        /// <param name="message">附加消息</param>
        public Result(int code, string message)
        {
            Code = code;
            Message = message;
            Content = null;
        }

        /// <summary>
        /// 简单操作返回类
        /// </summary>
        /// <param name="code">返回代码</param>
        /// <param name="message">附加消息</param>
        /// <param name="content">附加内容</param>
        public Result(int code, string message, object content)
        {
            Code = code;
            Message = message;
            Content = content;
        }

    }
}
