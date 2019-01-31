using System;
using System.Drawing;
using System.IO;

namespace SpyTool
{
    class SimilarPhoto
    {
        Image SourceImg;

        public SimilarPhoto(string filePath)
        {
            SourceImg = Image.FromFile(filePath);
        }

        public SimilarPhoto(Stream stream)
        {
            SourceImg = Image.FromStream(stream);
        }

        public SimilarPhoto(Image image)
        {
            SourceImg = image;
        }

        /// <summary>
        /// 适用于Fill 填充满整个控件的PicBoxButton or Button
        /// </summary>
        /// <param name="_old"></param>
        /// <param name="_new"></param>
        /// <returns></returns>
        public static bool IsChanged(Image _old, Image _new)
        {
            SimilarPhoto similarPhoto_old = new SimilarPhoto(_old);
            SimilarPhoto similarPhoto_new = new SimilarPhoto(_new);
            int similar = CalcSimilarDegree(similarPhoto_old.GetHash(), similarPhoto_new.GetHash());
            if (similar < 5)
                return false;
            else
                return true;
        }

        public String GetHash()
        {
            Image image = ReduceSize();
            Byte[] grayValues = ReduceColor(image);
            Byte average = CalcAverage(grayValues);
            String reslut = ComputeBits(grayValues, average);
            return reslut;
        }

        // Step 1 : Reduce size to 8*8
        private Image ReduceSize(int width = 8, int height = 8)
        {
            Image image = SourceImg.GetThumbnailImage(width, height, () =>
            {
                return false;
            }, IntPtr.Zero);
            //image.Save(@"C:\Users\luxshare-ict\Desktop\Test\1" + ".jpg");
            return image;
        }

        //Step 2 : Reduce Color
        private Byte[] ReduceColor(Image image)
        {
            Bitmap bitMap = new Bitmap(image);
            Byte[] grayValues = new Byte[image.Width * image.Height];

            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                {
                    Color color = bitMap.GetPixel(x, y);
                    byte grayValue = (byte)((color.R * 30 + color.G * 59 + color.B * 11) / 100);
                    grayValues[x * image.Width + y] = grayValue;
                }
            return grayValues;
        }

        //Step 3 : Average the colors
        private Byte CalcAverage(byte[] values)
        {
            int sum = 0;
            for (int i = 0; i < values.Length; i++)
                sum += (int)values[i];
            return Convert.ToByte(sum / values.Length);
        }

        //Step 4 : Compute the bits
        private String ComputeBits(byte[] values, byte averageValue)
        {
            char[] result = new char[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] < averageValue)
                    result[i] = '0';
                else
                    result[i] = '1';
            }
            return new String(result);
        }

        // Compare hash
        public static Int32 CalcSimilarDegree(string a, string b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException();
            int count = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    count++;
            }
            return count;
        }
    }
}
