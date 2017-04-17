using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using System.Net;
using System.Web;

namespace OFrameLibrary.Helpers
{
    public static class PdfHelper
    {
        public static string GetPDFFromHTML(string HTML, string path, string filename)
        {
            using (var document = new Document())
            {
                using (var fileStream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                {
                    var writer = PdfWriter.GetInstance(document, fileStream);
                    document.Open();
                    using (var stringReader = new StringReader(HTML))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);
                    }
                }
            }

            return LocalStorages.Storage + filename;
        }

        public static void WriteDataToPdf(string path, string filename)
        {
            using (var req = new WebClient())
            {
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ClearContent();
                response.ClearHeaders();
                response.Buffer = true;
                response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename);
                var data = req.DownloadData(path + filename);
                response.BinaryWrite(data);
                response.End();
            }
        }
    }
}
