using HNBackend.Global;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HNBackend.Module.TCaptureScreen
{
    public class TCaptureMain
    {
        public static void CaptureImage(string folderSaveImage, string fileName = "", string extension = "")
        {
            try
            {
                if (string.IsNullOrEmpty(folderSaveImage))
                    throw new Exception("Folder Save Image IsNullOrEmpty.");

                Bitmap bmScreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                var gfxScreenshot = Graphics.FromImage(bmScreen);
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

                if (string.IsNullOrEmpty(extension))
                    extension = ".png";

                string full_path = string.Empty;
                if (string.IsNullOrEmpty(fileName))
                    fileName = string.Format("{0}_{1}{2}", "ScreenShot", TGlobal.CreateGUID(TGUID.TIME), extension);

                if (!Directory.Exists(folderSaveImage))
                    Directory.CreateDirectory(folderSaveImage);

                full_path = string.Format("{0}\\{1}", folderSaveImage, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
