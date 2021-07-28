using System;
using Spire.Pdf;
using System.Drawing;
using System.Threading;
using Spire.Pdf.HtmlConverter;
using Newtonsoft.Json;
using System.IO;
using Spire.Pdf.Graphics;

namespace PDF
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static void InitializationApplication()
        {
            // Initialize log4net.
            log4net.Config.XmlConfigurator.Configure();

            log.Info("Start execution...");
        }

        // Method that creates PDF from Wikipedia page.
        private static void FromWikipediaToPDF(PdfDocument doc, PdfPageSettings setting, PdfHtmlLayoutFormat htmlLayoutFormat)
        {
            // Take content from url to convert into PDF.
            String url = "https://www.wikipedia.org/";

            Thread thread = new Thread(() =>
            {
                doc.LoadFromHTML(url, false, false, false, setting, htmlLayoutFormat);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            log.Info("Saving the PDF...");

            // Save PDF to file.
            doc.SaveToFile("output-wiki.pdf");
            doc.Close();
        }
       
        // Method that creates PDF from File.
        private static void FromFileToPDF(PdfDocument doc, String fileName)
        {
            // Open and Read file.
            string text = File.ReadAllText(fileName);

            // Add section to PDF.
            PdfSection section = doc.Sections.Add();
            // Add Page to PDF.
            PdfPageBase page = section.Pages.Add();

            // Setting font, format, brush and widget.
            PdfFont font = new PdfFont(PdfFontFamily.Helvetica, 11);
            PdfStringFormat format = new PdfStringFormat();
            format.LineSpacing = 20f;

            PdfBrush brush = PdfBrushes.Black;
            PdfTextWidget textWidget = new PdfTextWidget(text, font, brush);

            float y = 0;

            PdfTextLayout textLayout = new PdfTextLayout();
            textLayout.Break = PdfLayoutBreakType.FitPage;
            textLayout.Layout = PdfLayoutType.Paginate;

            RectangleF bounds = new RectangleF(new PointF(0, y), page.Canvas.ClientSize);
            textWidget.StringFormat = format;
            textWidget.Draw(page, bounds, textLayout);

            // Save to PDF.
            doc.SaveToFile("TxtToPDf.pdf", FileFormat.PDF);
            doc.Close();
        }

        // Method that creates a JSON example.
        private static String CreateJson(String name, String surname, int years, String city)
        {
            ProvaJSON simplePerson = new ProvaJSON();
            simplePerson.name = name;
            simplePerson.surname = surname;
            simplePerson.years = years;
            simplePerson.city = city;

            return JsonConvert.SerializeObject(simplePerson);
        }

        static void Main(string[] args)
        {
            InitializationApplication();

            // Create a PDF document.
            PdfDocument doc = new PdfDocument();
            PdfPageSettings setting = new PdfPageSettings();

            // Setting size.
            setting.Size = new SizeF(1000, 1000);
            setting.Margins = new Spire.Pdf.Graphics.PdfMargins(20);

            PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();
            htmlLayoutFormat.IsWaiting = true;

            log.Info("Creating PDF from Wikipedia link...");

            FromWikipediaToPDF(doc, setting, htmlLayoutFormat);

            // Write Error Log to File.
            try
            {
                throw new Exception("This is an error message...");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Write all Log to .log file.
            Console.Read();

            log.Info("Opening PDF from Wikipedia link...");

            // Launching the PDF file.
            System.Diagnostics.Process.Start("output-wiki.pdf");

            // Create PDF from JSON
            // It is more convenient and faster to use the "aspose.cells" library but i cannot use it.
            // So it is better to switch from JSON to file and from file to PDF because there isn't JSON -> PDF method.
            // products.aspose.com/cells/net/conversion/json-to-pdf/
            ProvaJSON textJSON = (ProvaJSON) JsonConvert.DeserializeObject<ProvaJSON>(CreateJson("Giovanni", "Poggi", 24, "Milano"));
            String fileName = "SampleText.txt";

            log.Info("Creating Text File from String...");

            // Append text to an existing file named "SampleText.txt".
            using (StreamWriter outputFile = new StreamWriter(fileName, false))
            {
                outputFile.WriteLine(textJSON.ToString());
            }

            log.Info("Creating PDF from File...");

            FromFileToPDF(doc, fileName);

            log.Info("End");
        }
    }
}
