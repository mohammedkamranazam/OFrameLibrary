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
            using (Document document = new Document())
            {
                var writer = PdfWriter.GetInstance(document, new FileStream(Path.Combine(path, filename), FileMode.Create));
                document.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, new StringReader(HTML));
            }

            return LocalStorages.Storage + filename;
        }

        public static void WriteDataToPdf(string path, string filename)
        {
            var req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
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