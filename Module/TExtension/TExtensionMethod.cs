using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNBackend.Module.TExtension
{
    public static class TExtensionMethod
    {
        #region String => Int; Int => String

        // ---- Parse string => int
        public static int TToInt32(this string obj)
        {
            try
            {
                return Int32.Parse(obj.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string TToString(this int obj)
        {
            try
            {
                return obj.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // ----- Parse Array string => Array int
        public static int[] TToArrayInt32(this string[] obj)
        {
            try
            {
                return Array.ConvertAll(obj, s => int.Parse(s));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string[] TToArrayString(this int[] obj)
        {
            try
            {
                return Array.ConvertAll(obj, s => s.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Setting
        public static string TAppSettings(this string text, string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region JSON
        public static string TObjectToJson(this object obj)
        {
            try
            {
                if (obj != null)
                    return JsonConvert.SerializeObject(obj);
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static T TJsonToObject<T>(this string obj)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region File + Folder
        // ---- Exist
        public static bool TIsExistFile(this string pathFile)
        {
            try
            {
                if (File.Exists(pathFile))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool TIsExistFolder(this string pathFolder)
        {
            try
            {
                if (Directory.Exists(pathFolder))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string TReadAllTextFile(this string pathFile)
        {
            try
            {
                if (pathFile.TIsExistFile())
                    return File.ReadAllText(pathFile);
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Image
        public static string TConvertByteImageToBase64(this byte[] bImage)
        {
            try
            {
                if (bImage != null)
                {
                    string base64String = Convert.ToBase64String(bImage);
                    return @"data:image/jpeg;base64," + base64String;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static byte[] TConvertBase64ToByteImage(this string base64)
        {
            try
            {
                bool isJPG = base64.IndexOf("jpeg;base64,") > 0;
                string strBase64 = isJPG ? base64.Remove(0, "data:image/jpeg;base64,".Length) : base64.Remove(0, "data:image/png;base64,".Length);
                byte[] bytes = System.Convert.FromBase64String(strBase64);
                return bytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Image TConvertBase64ToImage(this string base64)
        {
            try
            {
                byte[] bytes = TConvertBase64ToByteImage(base64);
                if (bytes == null)
                    return null;
                Image img = TConvertByteImageToImage(bytes);
                return img;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Image TConvertByteImageToImage(this byte[] bImage)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream(bImage);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
            catch (Exception ex)
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }
                throw ex;
            }
        }

        public static bool TSaveJpeg(this Image image, string path, int quality)
        {
            try
            {
                if (quality < 0 || quality > 100)
                    throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                ImageCodecInfo jpegCodec = TGetEncoderInfo("image/jpeg");
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;

                image.Save(path, jpegCodec, encoderParams);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static ImageCodecInfo TGetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
        #endregion
    }
}
