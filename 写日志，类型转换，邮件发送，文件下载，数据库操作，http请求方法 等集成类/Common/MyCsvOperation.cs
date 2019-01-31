using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Common
{
    public class MyCsvOperation
    {
        /// <summary>
        /// 生成SCV文件
        /// </summary>
        ///<param name="fileName">生成文件路径+名称</param>
        ///<param name="modeList">生成内容</param>
        ///<param name="size">文件大小</param>
        public static void Write(string fileName, List<string> modeList,out string size)
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs, Encoding.Default))
                {
                    var len = modeList.Count;
                    for (var i = 0; i < len; i++)
                    {
                        sw.WriteLine(modeList[i]);

                    }
                }
            }
            var myFile = new FileInfo(fileName);
            size = GetAutoSizeString(myFile.Length, 2);
        }



        private const double KbCount = 1024;
        private const double MbCount = KbCount * 1024;
        private const double GbCount = MbCount * 1024;
        private const double TbCount = GbCount * 1024;

        /// <summary>
        /// 得到适应的大小
        /// </summary>
        /// <param name="size">文件大小</param>
        /// <param name="roundCount">精度</param>
        /// <returns>string</returns>
        public static string GetAutoSizeString(double size, int roundCount)
        {
            //if (KbCount > size)
            //{
            //    return Math.Round(size / KbCount, roundCount) + "B";
            //}
            if (MbCount > size)
            {
                return Math.Round(size / KbCount, roundCount) + "KB";
            }
            if (GbCount > size)
            {
                return Math.Round(size / MbCount, roundCount) + "MB";
            }
            if (TbCount > size)
            {
                return Math.Round(size / GbCount, roundCount) + "GB";
            }
            return Math.Round(size / TbCount, roundCount) + "TB";
        }
    }
}
