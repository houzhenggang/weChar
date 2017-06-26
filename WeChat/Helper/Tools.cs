using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace WeChat.Helper
{
    public class Tools
    {

        /// <summary>
        /// Sha1
        /// </summary>
        /// <param name="orgStr"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string Sha1(string orgStr, string encode = "UTF-8")
        {
            var sha1 = new SHA1Managed();
            var sha1bytes = System.Text.Encoding.GetEncoding(encode).GetBytes(orgStr);
            byte[] resultHash = sha1.ComputeHash(sha1bytes);
            string sha1String = BitConverter.ToString(resultHash).ToLower();
            sha1String = sha1String.Replace("-", "");
            return sha1String;
        }
        #region 获取Json字符串某节点的值
        /// <summary>
        /// 获取Json字符串某节点的值
        /// </summary>
        public static string GetJsonValue(string jsonStr, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                key = "\"" + key.Trim('"') + "\"";
                int index = jsonStr.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截<span id="3_nwp" style="width: auto; height: auto; float: none;"><a id="3_nwl" href="http://cpro.baidu.com/cpro/ui/uijs.php?c=news&cf=5&ch=0&di=128&fv=17&jk=17f5c9861aa0f402&k=%B6%BA%BA%C5&k0=%B6%BA%BA%C5&kdi0=0&luki=2&n=10&p=baidu&q=00007110_cpr&rb=0&rs=1&seller_id=1&sid=2f4a01a86c9f517&ssp2=1&stid=0&t=tpclicked3_hc&tu=u1704338&u=http%3A%2F%2Fwww%2Edaxueit%2Ecom%2Farticle%2F5631%2Ehtml&urlid=0" target="_blank" mpid="3" style="text-decoration: none;"><span style="color:#0000ff;font-size:14px;width:auto;height:auto;float:none;">逗号</span></a></span>，若是最后一个，截“｝”号，取最小值
                    int end = jsonStr.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonStr.IndexOf('}', index);
                    }

                    result = jsonStr.Substring(index, end - index);
                    result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
                }
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 创建时间戳
        /// </summary>
        /// <returns></returns>
        public static long CreatenTimestamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        private static string[] strs = new string[]
                                 {
                                  "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z",
                                  "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
                                 };
        /// <summary>
        /// 创建随机字符串
        /// 本代码来自开源微信SDK项目：
        /// </summary>
        /// <returns></returns>
        public static string CreatenNonce_str()
        {
            Random r = new Random();
            var sb = new StringBuilder();
            var length = strs.Length;
            for (int i = 0; i < 15; i++)
            {
                sb.Append(strs[r.Next(length - 1)]);
            }
            return sb.ToString();
        }

        //中奖算法
        //public static int getGameWinningLevel(List<TurntablePrzie> list)
        //{

        //    // 中奖等级：未中奖   
        //    int winningLevel = -1;
        //    //如果未定义数据之间返回未中奖
        //    if (list == null || list.Count <= 0)
        //    {
        //        return winningLevel;
        //    }
        //    // 中奖随机号   
        //    int randomWinningNo = 0;
        //    int[] args = new int[list.Count * 2];
        //    Random xjzwx = new Random();
        //    //随机生成一个6位数整数
        //    int temp = (int)Math.Round(xjzwx.NextDouble() * 1000000000) % 1000000;
        //    int j = 0;

        //    for (int i = 0; i < list.Count(); i++)
        //    {
        //        //获取中奖概率
        //        double tmpWinningPro = list[i].Probability;
        //        //
        //        if (j == 0)
        //        {
        //            args[j] = randomWinningNo;
        //        }
        //        else
        //        {
        //            args[j] = args[j - 1] + 1;
        //        }
        //        args[j + 1] = args[j] + (int)Math.Round(tmpWinningPro * 10000) - 1;

        //        if (temp >= args[j] && temp <= args[j + 1])
        //        {
        //            winningLevel = i;//返回中奖奖品在list中的位置
        //            return winningLevel;
        //        }
        //        j += 2;
        //    }
        //    return winningLevel;
        //}


        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth"></param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>

        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);

            ImageFormat tFormat = iSource.RawFormat;

            int sW = 0, sH = 0;

            //按比例缩放

            Size tem_size = new Size(iSource.Width, iSource.Height);



            if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
            {

                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {

                    sW = dWidth;

                    sH = (dWidth * tem_size.Height) / tem_size.Width;

                }

                else
                {

                    sH = dHeight;

                    sW = (tem_size.Width * dHeight) / tem_size.Height;

                }

            }

            else
            {

                sW = tem_size.Width;

                sH = tem_size.Height;

            }

            Bitmap ob = new Bitmap(dWidth, dHeight);

            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);

            g.CompositingQuality = CompositingQuality.HighQuality;

            g.SmoothingMode = SmoothingMode.HighQuality;

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();

            //以下代码为保存图片时，设置压缩质量

            EncoderParameters ep = new EncoderParameters();

            long[] qy = new long[1];

            qy[0] = flag;//设置压缩的比例1-100

            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);

            ep.Param[0] = eParam;

            try
            {

                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();

                ImageCodecInfo jpegICIinfo = null;

                for (int x = 0; x < arrayICI.Length; x++)
                {

                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {

                        jpegICIinfo = arrayICI[x];

                        break;

                    }

                }

                if (jpegICIinfo != null)
                {

                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径

                }

                else
                {

                    ob.Save(dFile, tFormat);

                }

                return true;

            }

            catch
            {

                return false;

            }

            finally
            {

                iSource.Dispose();

                ob.Dispose();

            }



        }

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <returns></returns>
        public static bool CompressImages(string pathOriginal, string pathDestination)
        {

            var files = Directory.GetFiles(pathOriginal);
            bool bos = false;

            foreach (var file in files)
            {
                string strImgPathOriginal = pathOriginal + Path.GetFileName(file).ToString().Trim();
                if (System.IO.File.Exists(strImgPathOriginal))
                { //原始图片存在

                    string strImgPath = pathDestination + Path.GetFileName(file).ToString().Trim();
                    if (!System.IO.File.Exists(strImgPath))
                    { //压缩图片不存在

                        //压缩图片
                        bos = GetPicThumbnail(strImgPathOriginal, strImgPath, 200, 200, 100);
                        if (!bos)
                        {
                            break;
                        }
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片Img</param>
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">高度</param>
        /// <param name="dWidth"></param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns></returns>

        public static bool GetPicThumbnails(System.Drawing.Image iSource, string dFile, int dHeight, int dWidth, int flag)
        {
            //System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);

            ImageFormat tFormat = iSource.RawFormat;

            int sW = 0, sH = 0;

            int temp = 0;
            //按比例缩放
            if (iSource.Width > iSource.Height)
            {
                temp = iSource.Height;
            }
            else
            {

                temp = iSource.Width;
            }



            Size tem_size = new Size(temp, temp);


            if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
            {

                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {

                    sW = dWidth;

                    sH = (dWidth * tem_size.Height) / tem_size.Width;

                }

                else
                {

                    sH = dHeight;

                    sW = (tem_size.Width * dHeight) / tem_size.Height;

                }

            }

            else
            {

                sW = tem_size.Width;

                sH = tem_size.Height;

            }

            Bitmap ob = new Bitmap(dWidth, dHeight);

            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);

            g.CompositingQuality = CompositingQuality.HighQuality;

            g.SmoothingMode = SmoothingMode.HighQuality;

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();

            //以下代码为保存图片时，设置压缩质量

            EncoderParameters ep = new EncoderParameters();

            long[] qy = new long[1];

            qy[0] = flag;//设置压缩的比例1-100

            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);

            ep.Param[0] = eParam;

            try
            {

                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();

                ImageCodecInfo jpegICIinfo = null;

                for (int x = 0; x < arrayICI.Length; x++)
                {

                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {

                        jpegICIinfo = arrayICI[x];

                        break;

                    }

                }

                if (jpegICIinfo != null)
                {

                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径

                }

                else
                {

                    ob.Save(dFile, tFormat);

                }

                return true;

            }

            catch
            {

                return false;

            }

            finally
            {

                iSource.Dispose();

                ob.Dispose();

            }



        }


        /// <summary> 
        /// 按照指定大小缩放图片，但是为了保证图片宽高比将对图片从中心进行截取，达到图片不被拉伸的效果
        /// </summary> 
        /// <param name="srcImage"></param> 
        /// <param name="iWidth"></param> 
        /// <param name="iHeight"></param> 
        /// <returns></returns> 
        public static Bitmap SizeImageWithOldPercent(Image srcImage, int iWidth, int iHeight)
        {
            try
            {
                // 要截取图片的宽度（临时图片） 
                int newW = srcImage.Width;
                // 要截取图片的高度（临时图片） 
                int newH = srcImage.Height;
                // 截取开始横坐标（临时图片） 
                int newX = 0;
                // 截取开始纵坐标（临时图片） 
                int newY = 0;
                // 截取比例（临时图片） 
                double whPercent = 1;
                whPercent = ((double)iWidth / (double)iHeight) * ((double)srcImage.Height / (double)srcImage.Width);
                if (whPercent > 1)
                {
                    // 当前图片宽度对于要截取比例过大时 
                    newW = int.Parse(Math.Round(srcImage.Width / whPercent).ToString());
                }
                else if (whPercent < 1)
                {
                    // 当前图片高度对于要截取比例过大时 
                    newH = int.Parse(Math.Round(srcImage.Height * whPercent).ToString());
                }
                if (newW != srcImage.Width)
                {
                    // 宽度有变化时，调整开始截取的横坐标 
                    newX = Math.Abs(int.Parse(Math.Round(((double)srcImage.Width - newW) / 2).ToString()));
                }
                else if (newH == srcImage.Height)
                {
                    // 高度有变化时，调整开始截取的纵坐标 
                    newY = Math.Abs(int.Parse(Math.Round(((double)srcImage.Height - (double)newH) / 2).ToString()));
                }
                // 取得符合比例的临时文件 
                Bitmap cutedImage = CutImage(srcImage, newX, newY, newW, newH);
                // 保存到的文件 
                Bitmap b = new Bitmap(iWidth, iHeight);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量 
                g.InterpolationMode = InterpolationMode.Default;
                g.DrawImage(cutedImage, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(0, 0, cutedImage.Width, cutedImage.Height), GraphicsUnit.Pixel);
                g.Dispose();
                //return b;

                return cutedImage;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary> 
        /// 剪裁 -- 用GDI+ 
        /// </summary> 
        /// <param name="b">原始Bitmap</param> 
        /// <param name="StartX">开始坐标X</param> 
        /// <param name="StartY">开始坐标Y</param> 
        /// <param name="iWidth">宽度</param> 
        /// <param name="iHeight">高度</param> 
        /// <returns>剪裁后的Bitmap</returns> 
        public static Bitmap CutImage(Image b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }
            int w = b.Width;
            int h = b.Height;
            if (StartX >= w || StartY >= h)
            {
                // 开始截取坐标过大时，结束处理 
                return null;
            }
            if (StartX + iWidth > w)
            {
                // 宽度过大时只截取到最大大小 
                iWidth = w - StartX;
            }
            if (StartY + iHeight > h)
            {
                // 高度过大时只截取到最大大小 
                iHeight = h - StartY;
            }
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight);
                Graphics g = Graphics.FromImage(bmpOut);
                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 切原始图片
        /// </summary>
        /// <param name="b"></param>
        /// <param name="StartX"></param>
        /// <param name="StartY"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <returns></returns>
        public static Bitmap CutImageOriginal(Image b)
        {
            Image imageSource = b;
            int orgWidth = imageSource.Width;
            int orgHight = imageSource.Height;

            int width = 0;
            int height = 0;

            if (orgWidth > orgHight)
            {
                width = orgHight;
                height = orgHight;
            }
            else
            {
                width = orgWidth;
                height = orgWidth;
            }

            Rectangle cropArea = new Rectangle();

            int x = orgWidth / 2 - width / 2;//（145是从中间开始向两边截图的宽度，可以自定义）
            int y = orgHight / 2 - height / 2;

            cropArea.X = x;
            cropArea.Y = y;
            cropArea.Width = width;
            cropArea.Height = height;

            Bitmap bmpImage = new Bitmap(imageSource);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);

            return bmpCrop;
        }

        /// <summary>
        /// 生成两个数之间的随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static int GetRandNum(int min, int max)
        {
            Random r = new Random(Guid.NewGuid().GetHashCode());
            return r.Next(min, max);
        }

        ///<summary>返回两个经纬度坐标点的距离（单位：米） by Alex.Y</summary>
        ///<param name="Longtiude">来源坐标经度Y</param>
        ///<param name="Latitude">来源坐标经度X</param>
        ///<param name="Longtiude2">目标坐标经度Y</param>
        ///<param name="Latitude2">目标坐标经度X</param>
        ///<returns>返回距离（米）</returns>
        public static double getMapDistance(double Longtiude, double Latitude, double Longtiude2, double Latitude2)
        {

            var lat1 = Latitude;
            var lon1 = Longtiude;
            var lat2 = Latitude2;
            var lon2 = Longtiude2;
            var earthRadius = 6371; //appxoximate radius in miles  6378.137

            var factor = Math.PI / 180.0;
            var dLat = (lat2 - lat1) * factor;
            var dLon = (lon2 - lon1) * factor;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1 * factor)
              * Math.Cos(lat2 * factor) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double d = earthRadius * c * 1000;

            return d;

        }
    }
}