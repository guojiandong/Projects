using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SpyTool
{
    public class UtilHelper
    {
        public static Dictionary<string, string> MessageTypeDic = new Dictionary<string, string>();

        /// <summary>
        /// 初始化 信息类型
        /// </summary>
        public static void InitMessageType()
        {

//            [A=255, R=216, G=30, B=6]  红色
//            [A = 255, R = 18, G = 150, B = 219] 蓝色
//            [A = 255, R = 244, G = 234, B = 42] 黄色
//            [A = 255, R = 255, G = 106, B = 51] 红色
//            [A = 255, R = 112, G = 112, B = 112] 灰色
//            [A = 255, R = 26, G = 250, B = 41] 绿色

            MessageTypeDic.Add(Color.FromArgb(216, 30, 6).ToString(), "深红");
            MessageTypeDic.Add(Color.FromArgb(18, 150, 219).ToString(), "蓝色");
            MessageTypeDic.Add(Color.FromArgb(244, 234, 42).ToString(), "黄色");
            MessageTypeDic.Add(Color.FromArgb(255, 106, 51).ToString(), "浅红色");
            MessageTypeDic.Add(Color.FromArgb(112, 112, 112).ToString(), "灰色");
            MessageTypeDic.Add(Color.FromArgb(26, 250, 41).ToString(), "绿色");
        }

        /// <summary>
        /// 抓取整个屏幕
        /// </summary>
        /// <returns></returns>
        public static Bitmap CaptureScreen()
        {
            Size screenSize = Screen.PrimaryScreen.Bounds.Size;
            return CaptureScreen(0, 0, screenSize.Width, screenSize.Height);
        }

        /// <summary>
        /// 根据尺寸截屏
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap CaptureScreen(int x, int y, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(new Point(x, y), new Point(0, 0), bmp.Size);
                g.Dispose();
            }
            //bit.Save(@"capture2.png");
            return bmp;
        }

        /// <summary>
        /// 逐像素对比 比较耗
        /// </summary>
        public static Color ImageCompareArrayChanged(Bitmap oldImage, Bitmap newImage, out bool isChanged)
        {
            Color resultColor = Color.White;
            isChanged = false;
            string firstPixel;
            string secondPixel;
            if (oldImage.Width == newImage.Width && oldImage.Height == newImage.Height)
            {
                for (int i = 0; i < oldImage.Width; i++)
                {
                    for (int j = 0; j < newImage.Height; j++)
                    {
                        firstPixel = oldImage.GetPixel(i, j).ToString();
                        secondPixel = newImage.GetPixel(i, j).ToString();
                        if (firstPixel != secondPixel)
                        {
                            isChanged = true;
                            resultColor = newImage.GetPixel(i, j);
                            return resultColor;
                        }
                    }
                }
            }
            return resultColor;
        }

        /// <summary>
        /// 逐像素对比 比较耗
        /// </summary>
        public static Color ImageCompareArray1Changed(Bitmap oldImage, Bitmap newImage, out bool isChanged)
        {
            Color resultColor = Color.White;
            isChanged = false;
            Color firstPixel;
            Color secondPixel;
            if (oldImage.Width == newImage.Width && oldImage.Height == newImage.Height)
            {
                for (int i = 0; i < oldImage.Width; i++)
                {
                    for (int j = 0; j < newImage.Height; j++)
                    {
                        firstPixel = oldImage.GetPixel(i, j);
                        secondPixel = newImage.GetPixel(i, j);
                        if (Math.Abs(firstPixel.R - secondPixel.R) > 50 || Math.Abs(firstPixel.G - secondPixel.G) > 50 || Math.Abs(firstPixel.B - secondPixel.B) > 50)
                        {
                            isChanged = true;
                            resultColor = newImage.GetPixel(i, j);
                            return resultColor;
                        }
                    }
                }
            }
            return resultColor;
        }

        /// <summary>
        /// 将图片转化为string再比较  Image 固定大小 
        /// </summary>
        /// <param name="firstImage"></param>
        /// <param name="secondImage"></param>
        /// <returns></returns>
        public static bool BitmapCompareStringChanged(Bitmap firstImage, Bitmap secondImage)
        {
            MemoryStream ms = new MemoryStream();
            firstImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            String firstBitmap = Convert.ToBase64String(ms.ToArray());
            ms.Position = 0;

            secondImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            String secondBitmap = Convert.ToBase64String(ms.ToArray());

            if (firstBitmap.Equals(secondBitmap))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary> 
        /// 计算相对位置的RGB是否一致  一点检测
        /// </summary>
        /// <param name="_old"></param>
        /// <param name="_new"></param>
        /// <returns></returns>
        public static bool ImageComparePixelChanged(Image _old ,Image _new)
        {
            int _old_width = _old.Width;
            int _old_heigt = _old.Height;
            int _new_width = _new.Width;
            int _new_heigt = _new.Height;



            int old_color1 = ((new Bitmap(_old)).GetPixel(_old_width / 5, _old_heigt /5)).ToArgb();
            int new_color1 = ((new Bitmap(_new)).GetPixel(_new_width / 5, _new_heigt / 5)).ToArgb();

            if (old_color1 != new_color1)
                return true;

            return false;
        }

        /// <summary>
        /// 计算相对位置的RGB是否一致 五点矫正
        /// </summary>
        /// <param name="_old"></param>
        /// <param name="_new"></param>
        /// <returns></returns>
        public static bool ImageCompareMultiPixelChanged(Image _old, Image _new)
        {
            int _old_width = _old.Width;
            int _old_heigt = _old.Height;
            int _new_width = _new.Width;
            int _new_heigt = _new.Height;

            Vertor2 old_center_point = new Vertor2(_old_width / 2, _old_heigt / 2);
            Vertor2 new_center_point = new Vertor2(_new_width / 2, _new_heigt / 2);

            // 中心点上下左右各偏移10%Width  10%Height
            List<Vertor2> oldVerterList = new List<Vertor2>() {
                new Vertor2(old_center_point),// center
                new Vertor2(old_center_point.x - _old_width / 10 ,old_center_point.y),// Left
                new Vertor2(old_center_point.x ,old_center_point.y + _old_heigt / 10),// Top
                new Vertor2(old_center_point.x + _old_width / 10 ,old_center_point.y ),// Right
                new Vertor2(old_center_point.x ,old_center_point.y - _old_heigt / 10),// Botom
            };

            List<Vertor2> newVerterList = new List<Vertor2>() {
                new Vertor2(old_center_point),// center
                new Vertor2(new_center_point.x - _new_width / 10 ,new_center_point.y),// Left
                new Vertor2(new_center_point.x ,new_center_point.y + _new_heigt / 10),// Top
                new Vertor2(new_center_point.x + _new_width / 10 ,new_center_point.y ),// Right
                new Vertor2(new_center_point.x ,new_center_point.y - _new_heigt / 10),// Botom
            };

            for (int i= 0;i< newVerterList.Count;i++)
            {
                int old_color1 = ((new Bitmap(_old)).GetPixel(oldVerterList[i].x, oldVerterList[i].y)).ToArgb();
                int new_color1 = ((new Bitmap(_new)).GetPixel(newVerterList[i].x, newVerterList[i].y)).ToArgb();
               // Console.WriteLine("old_color1 = "+ old_color1 + " new_color1 = "+ new_color1);
                if (old_color1 != new_color1)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 将图片转化为string再比较  适用于背景图片大小不变的控件
        /// </summary>
        /// <param name="firstImage"></param>
        /// <param name="secondImage"></param>
        /// <returns></returns>
        public static bool ImageCompareStringChanged(Image firstImage, Image secondImage)
        {
            MemoryStream ms = new MemoryStream();
            firstImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            String firstBitmap = Convert.ToBase64String(ms.ToArray());
            ms.Position = 0;

            secondImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            String secondBitmap = Convert.ToBase64String(ms.ToArray());

            if (firstBitmap.Equals(secondBitmap))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 选取图片中某一点对比 性能较高 可靠性？
        /// </summary>
        public static Color ImageComparePointChanged(Bitmap oldImage, Bitmap newImage, out bool isChanged)
        {
            isChanged = false;
            Color resultColor = Color.White;
            if (oldImage.Width == newImage.Width && oldImage.Height == newImage.Height)
            {
                // 选取 中间点对比即可
                int center_i = newImage.Width / 2;
                int center_j = newImage.Height / 2;

                Color newColor = newImage.GetPixel(center_i, center_j);
                resultColor = newColor;
                string firstPixel = oldImage.GetPixel(center_i, center_j).ToString();
                string secondPixel = newColor.ToString();
                if (firstPixel != secondPixel)
                {
                    isChanged = true;
                }

            }
            return resultColor;
        }

        /// <summary>
        /// 从文件中加载Bitmap
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Bitmap ReadImageFile(string path)
        {
            if (!File.Exists(path))
                return null;

            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = 0;
            filelength = (int)fs.Length; //获得文件长度 
            Byte[] image = new Byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            System.Drawing.Image result = System.Drawing.Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="strSql"></param>
        public static void ExecuteInsert(string strSql)
        {
            MySqlConnection conn = null;
            string constructorString = "server=172.17.201.229;User Id=dbproxy;password= luxshare.auto.dbproxy.pwd;Database=w1_pdmis";
            try
            {
                conn = new MySqlConnection(constructorString);
                conn.Open();

                MySqlCommand mycmd = new MySqlCommand();
                mycmd.Connection = conn;
                mycmd.CommandType = CommandType.Text;
                mycmd.CommandText = strSql;
                mycmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("SqlHelper - ExecuteInsert Error: " + ex.Message + " Sql :" + strSql);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 获取图片的主色调 获取图片的所有像素计算
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Color GetMajorColor1(Bitmap bitmap)
        {
            //色调的总和
            var sum_hue = 0d;
            //色差的阈值
            var threshold = 30;
            //计算色调总和
            for (int h = 0; h < bitmap.Height; h++)
            {
                for (int w = 0; w < bitmap.Width; w++)
                {
                    var hue = bitmap.GetPixel(w, h).GetHue();
                    sum_hue += hue;
                }
            }
            var avg_hue = sum_hue / (bitmap.Width * bitmap.Height);

            //色差大于阈值的颜色值
            var rgbs = new List<Color>();
            for (int h = 0; h < bitmap.Height; h++)
            {
                for (int w = 0; w < bitmap.Width; w++)
                {
                    var color = bitmap.GetPixel(w, h);
                    var hue = color.GetHue();
                    //如果色差大于阈值，则加入列表
                    if (Math.Abs(hue - avg_hue) > threshold)
                    {
                        rgbs.Add(color);
                    }
                }
            }
            if (rgbs.Count == 0)
                return Color.Black;
            //计算列表中的颜色均值，结果即为该图片的主色调
            int sum_r = 0, sum_g = 0, sum_b = 0;
            foreach (var rgb in rgbs)
            {
                sum_r += rgb.R;
                sum_g += rgb.G;
                sum_b += rgb.B;
            }
            return Color.FromArgb(sum_r / rgbs.Count,
                sum_g / rgbs.Count,
                sum_b / rgbs.Count);
        }

        /// <summary>
        /// 获取 8x8 的缩略图
        /// </summary>
        /// <param name="SourceImg"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image ReduceSize(Image SourceImg, int width = 8, int height = 8)
        {
            Image image = SourceImg.GetThumbnailImage(width, height, () =>
            {
                return false;
            }, IntPtr.Zero);
            return image;
        }

        /// <summary>
        /// 颜色差异度
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns></returns>
        public static double GetDifference(Color color1, Color color2)
        {
            double y1 = 0.299 * color1.R + 0.587 * color1.G + 0.114 * color1.B;
            double u1 = -0.14713 * color1.R - 0.28886 * color1.G + 0.436 * color1.B;
            double v1 = 0.615 * color1.R - 0.51498 * color1.G - 0.10001 * color1.B;
            double y2 = 0.299 * color2.R + 0.587 * color2.G + 0.114 * color2.B;
            double u2 = -0.14713 * color2.R - 0.28886 * color2.G + 0.436 * color2.B;
            double v2 = 0.615 * color2.R - 0.51498 * color2.G - 0.10001 * color2.B;
            return System.Math.Sqrt((y1 - y2) * (y1 - y2) + (u1 - u2) * (u1 - u2) + (v1 - v2) * (v1 - v2));
        }

        /// <summary>
        /// 二位坐标
        /// </summary>
        public class Vertor2
        {
            public int x { get; set; }
            public int y { get; set; }

            public Vertor2(int _x, int _y)
            {
                this.x = _x;
                this.y = _y;
            }

            public Vertor2( Vertor2 other)
            {
                this.x = other.x;
                this.y = other.y;
            }
        }

        /// <summary>
        /// 获取图片的主色调  五点取样法 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static string GetMajorColor(Bitmap bitmap)
        {
            List<Vertor2> listColors = new List<Vertor2>();
            Dictionary<string, int> colorDic = new Dictionary<string, int>();

            int delta = bitmap.Width / 5;
            Color color = Color.Wheat;

            int Center_i = bitmap.Height / 2;
            int Center_j = bitmap.Width / 2;

            listColors.Add(new Vertor2(Center_i, Center_j)); //Center
            listColors.Add(new Vertor2(Center_i - delta, Center_j)); // Right
            listColors.Add(new Vertor2(Center_i , Center_j + delta));//Top
            listColors.Add(new Vertor2(Center_i + delta, Center_j));// Left
            listColors.Add(new Vertor2(Center_i, Center_j - delta));//Botom

            for (int i = 0;i< 5;i++)
            {
                Color co = bitmap.GetPixel(listColors[i].x, listColors[i].y);
                if (!colorDic.ContainsKey(co.ToString()))
                {
                    colorDic.Add(co.ToString(), 1);
                }
                else
                {
                    colorDic[co.ToString()]++;
                }
            }

            var dicSort = from objDic in colorDic orderby objDic.Value descending select objDic;

            foreach (KeyValuePair<string, int> kvp in dicSort)
            {
                return MessageTypeDic[kvp.Key];
            }
            return null;
        }

        /// <summary>
        /// 获取图片的主色调  五点取样法 
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static string GetMajorColorImage(Image image)
        {
            List<Vertor2> listColors = new List<Vertor2>();
            Dictionary<string, int> colorDic = new Dictionary<string, int>();

            int delta = image.Width / 5;
            Color color = Color.Wheat;

            int Center_i = image.Height / 2;
            int Center_j = image.Width / 2;

            listColors.Add(new Vertor2(Center_i, Center_j)); //Center
            listColors.Add(new Vertor2(Center_i - delta, Center_j)); // Right
            listColors.Add(new Vertor2(Center_i, Center_j + delta));//Top
            listColors.Add(new Vertor2(Center_i + delta, Center_j));// Left
            listColors.Add(new Vertor2(Center_i, Center_j - delta));//Botom

            for (int i = 0; i < 5; i++)
            {
                Color co = new Bitmap(image).GetPixel(listColors[i].x, listColors[i].y);
                if (!colorDic.ContainsKey(co.ToString()))
                {
                    colorDic.Add(co.ToString(), 1);
                }
                else
                {
                    colorDic[co.ToString()]++;
                }
            }

            var dicSort = from objDic in colorDic orderby objDic.Value descending select objDic;

            foreach (KeyValuePair<string, int> kvp in dicSort)
            {
                return MessageTypeDic[kvp.Key];
            }
            return null;
        }

        public static float GetResult(int[] firstNum, int[] scondNum)
        {
            if (firstNum.Length != scondNum.Length)
            {
                return 0;
            }
            else
            {
                float result = 0;
                int j = firstNum.Length;
                for (int i = 0; i < j; i++)
                {
                    result += 1 - GetAbs(firstNum[i], scondNum[i]);
                   // Console.WriteLine(i + "----" + result);
                }
                return result / j;
            }
        }

        //计算相减后的绝对值
        private static float GetAbs(int firstNum, int secondNum)
        {
            float abs = Math.Abs((float)firstNum - (float)secondNum);
            float result = Math.Max(firstNum, secondNum);
            if (result == 0)
                result = 1;
            return abs / result;
        }

        public static int[] GetHisogram(Bitmap img)
        {
            BitmapData data = img.LockBits(new System.Drawing.Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int[] histogram = new int[256];
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                int remain = data.Stride - data.Width * 3;
                for (int i = 0; i < histogram.Length; i++)
                    histogram[i] = 0;
                for (int i = 0; i < data.Height; i++)
                {
                    for (int j = 0; j < data.Width; j++)
                    {
                        int mean = ptr[0] + ptr[1] + ptr[2];
                        mean /= 3;
                        histogram[mean]++;
                        ptr += 3;
                    }
                    ptr += remain;
                }
            }
            img.UnlockBits(data);
            return histogram;
        }

        public static Dictionary<string, int> DicCount = new Dictionary<string, int>();

        public static bool CompareColorChanged(List<string> checkedColorList, List<string> ignoredColorList, Bitmap bitmap, string old_color, out string new_color)
        {
            DicCount.Clear();
            new_color = string.Empty;
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    new_color = ColorTranslator.ToHtml(color);
                    if (!ignoredColorList.Contains(new_color) && checkedColorList.Contains(new_color) && !old_color.Equals(new_color))
                    {
                        Console.WriteLine(" old_color = " + old_color + " new_color = " + new_color);
                        return true;
                    }
                }
            }
            return false;
        }

        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            object result = null;
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    result = xmldes.Deserialize(sr);
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);

            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }
        #endregion

        //string xml = XmlUtil.Serializer(typeof(List<BaseHeader>), baseHeaders);
        //using (FileStream fsWrite = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
        //{
        //    byte[] buffer = Encoding.Default.GetBytes(xml);
        //    fsWrite.Write(buffer, 0, buffer.Length);
        //}
        //MessageBox.Show("保存成功");
    }
}
