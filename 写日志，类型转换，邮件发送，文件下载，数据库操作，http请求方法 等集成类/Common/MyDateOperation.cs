using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class MyDateOperation
    {

        /// <summary>   
        /// 计算某日起始日期（礼拜一的日期）   
        /// </summary>   
        /// <param name="someDate">该周中任意一天</param>   
        /// <returns>返回礼拜一日期，后面的具体时、分、秒和传入值相等</returns>   
        public static DateTime GetMondayDate(DateTime someDate)
        {
            var i = someDate.DayOfWeek - DayOfWeek.Monday;
            if (i == -1) i = 6;// i值 > = 0 ，因为枚举原因，Sunday排在最前，此时Sunday-Monday=-1，必须+7=6。   
            var ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Subtract(ts);
        }  

        /// <summary>   
        /// 计算某日结束日期（礼拜日的日期）   
        /// </summary>   
        /// <param name="someDate">该周中任意一天</param>   
        /// <returns>返回礼拜日日期，后面的具体时、分、秒和传入值相等</returns>   
        public static DateTime GetSundayDate(DateTime someDate)
        {
            var i = someDate.DayOfWeek - DayOfWeek.Sunday;
            if (i != 0) i = 7 - i;// 因为枚举原因，Sunday排在最前，相减间隔要被7减。   
            var ts = new TimeSpan(i, 0, 0, 0);
            return someDate.Add(ts);
        }

        /// <summary>   
        /// 获取月初日期
        /// </summary>   
        /// <param name="someDate">该月中任意一天</param>   
        /// <returns>返回月初日期</returns>   
        public static DateTime GetBeginMonthDate(DateTime someDate)
        {
            return new DateTime(someDate.Year, someDate.Month, 1);
        }   

        /// <summary>   
        /// 获取月末日期 
        /// </summary>   
        /// <param name="someDate">该月中任意一天</param>   
        /// <returns>返回月末日期</returns>   
        public static DateTime GetEndMonthDate(DateTime someDate)
        {
            return GetBeginMonthDate(someDate).AddMonths(1).AddDays(-1);
        }


        /// <summary>
        /// json字符串转化成string 例如：90秒--->  00:01:30;
        /// </summary>
        public static string ParseTimeStr(decimal snum)
        {
            var num = (int)snum;
            var hh = (num / 3600).ToString("00");
            var mm = (num % 3600 / 60).ToString("00");
            var ss = (num % 3600 % 60).ToString("00");
            return hh + ":" + mm + ":" + ss;
        }
    }
}
