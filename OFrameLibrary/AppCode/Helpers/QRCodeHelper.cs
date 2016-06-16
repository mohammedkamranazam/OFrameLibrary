using QRCoder;
using System.Drawing;
using System.IO;

namespace OFrameLibrary.Helpers
{
    public static class QRCodeHelper
    {
        public static void GenerateQRCode(string code, string path)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeGenerator.QRCode qrCode = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.M);
            System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            imgBarCode.Height = 100;
            imgBarCode.Width = 100;
            using (Bitmap bitMap = qrCode.GetGraphic(20))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    //byte[] byteImage = ms.ToArray();
                    //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    //return imgBarCode.ImageUrl;
                    var img = Image.FromStream(ms);
                    img.Save(path);
                    img.Dispose();
                }
            }
        }
    }
}