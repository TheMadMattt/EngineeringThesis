using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EngineeringThesis.Core.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace EngineeringThesis.Core.Utility
{
    public class InvoiceTemplate
    {
        public Invoice Invoice;
        private iTextSharp.text.Font fonts;
        private iTextSharp.text.Font font;
        private iTextSharp.text.Font font_mini;
        private iTextSharp.text.Font napis_big;
        private iTextSharp.text.Font napis_big_n;

        public InvoiceTemplate(Invoice invoice)
        {
            Invoice = invoice;

            BaseFont bfComic = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            BaseFont bfComicBD = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, BaseFont.EMBEDDED);
            fonts = new iTextSharp.text.Font(bfComic, 8);
            font = new iTextSharp.text.Font(bfComicBD, 9);
            font_mini = new iTextSharp.text.Font(bfComic, 7);
            napis_big = new iTextSharp.text.Font(bfComicBD, 14);
            napis_big_n = new iTextSharp.text.Font(bfComic, 14);
        }

        public void createPDF(string title)
        {
            var file = "Faktury/" + Invoice.InvoiceNumber.Replace("/", "-") + ".pdf";

            Document document = new Document(iTextSharp.text.PageSize.A4, 50, 50, 70, 10); // 10: Margins Left, Right, Top, Bottom
            PdfWriter.GetInstance(document, new FileStream(file, FileMode.Create));

            document.Open();

            document.Add(CreateTitle(title));

            document.Close();
        }

        public PdfPTable CreateTitle(string title)
        {
            PdfPTable table = new PdfPTable(1);

            int[] titleWidth = new int[] { 100 };
            table.SetWidths(titleWidth);
            table.WidthPercentage = 100;

            string napis = " FAKTURA VAT nr " + Invoice.InvoiceNumber;

            PdfPCell cell = new PdfPCell(new Phrase(napis, napis_big))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 2,
                Border = 0
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(title, napis_big_n))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 2,
                Border = 0
            };
            table.AddCell(cell);

            return table;
        }
    }
}
