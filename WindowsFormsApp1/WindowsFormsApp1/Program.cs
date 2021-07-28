using System;
using Spire.Pdf;
using System.Drawing;
using System.Threading;
using Spire.Pdf.HtmlConverter;

namespace PDF
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {
            // Initialize log4net.
            log4net.Config.XmlConfigurator.Configure();

            log.Info("Inizio l'esecuzione");

            //Create a pdf document.
            PdfDocument doc = new PdfDocument();

            PdfPageSettings setting = new PdfPageSettings();

            setting.Size = new SizeF(1000, 1000);
            setting.Margins = new Spire.Pdf.Graphics.PdfMargins(20);

            PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();
            htmlLayoutFormat.IsWaiting = true;

            log.Info("Prendo dal link di Wikipedia l'HTML");

            String url = "https://www.wikipedia.org/";

            Thread thread = new Thread(() =>
            {
                doc.LoadFromHTML(url, false, false, false, setting, htmlLayoutFormat);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            log.Info("Salvo il PDF");

            //Save pdf file.
            doc.SaveToFile("output-wiki.pdf");
            doc.Close();

            //Write Log to File
            try
            {
                throw new Exception("This is test message...");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            Console.Read();

            //Launching the Pdf file.
            System.Diagnostics.Process.Start("output-wiki.pdf");
        }
    }
}
