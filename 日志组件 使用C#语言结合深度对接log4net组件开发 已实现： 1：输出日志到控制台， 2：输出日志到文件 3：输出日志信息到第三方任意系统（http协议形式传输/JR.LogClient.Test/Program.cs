using JR.LogClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JR.LogClient.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            LogHelper.Logger.Info("消息");
            LogHelper.Log.Info("消息");
            LogHelper.Logger.Warn("警告");
            LogHelper.Log.Warn("警告");
            LogHelper.Logger.Error("异常");
            LogHelper.Log.Error("异常");
            LogHelper.Logger.Fatal("错误");
            LogHelper.Log.Fatal("错误");

            Console.ReadLine();
        }
    }
}
