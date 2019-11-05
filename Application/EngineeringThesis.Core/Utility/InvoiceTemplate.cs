using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Text;
using System.Windows.Media;
using EngineeringThesis.Core.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace EngineeringThesis.Core.Utility
{
    public class InvoiceTemplate
    {
        public Invoice Invoice;
        private iTextSharp.text.Font _defaultFont;
        private iTextSharp.text.Font _fontBold;
        private iTextSharp.text.Font _fontMini;
        private readonly iTextSharp.text.Font _bigFontBold;
        private readonly iTextSharp.text.Font _bigFont;

        public InvoiceTemplate(Invoice invoice)
        {
            Invoice = invoice;

            BaseFont regularFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            BaseFont boldFont = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, BaseFont.EMBEDDED);
            _defaultFont = new iTextSharp.text.Font(regularFont, 9);
            _fontBold = new iTextSharp.text.Font(boldFont, 9);
            _fontMini = new iTextSharp.text.Font(regularFont, 7);
            _bigFontBold = new iTextSharp.text.Font(boldFont, 14);
            _bigFont = new iTextSharp.text.Font(regularFont, 14);
        }

        public string CreatePdf(string title)
        {
            var directory = "Faktury";
            var newLine = new Paragraph("\n");
            bool exists = System.IO.Directory.Exists(directory);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            var file = "Faktury/" + Invoice.InvoiceNumber.Replace("/", "-") + ".pdf";

            Document document = new Document(iTextSharp.text.PageSize.A4, 50, 50, 70, 10); // 10: Margins Left, Right, Top, Bottom
            PdfWriter.GetInstance(document, new FileStream(file, FileMode.Create));

            document.Open();

            //create invoice title
            document.Add(CreateTitle(title));

            document.Add(newLine);

            //create invoice date, invoice number and city
            document.Add(CreateInvoiceDates());

            document.Add(newLine);

            //create seller and contractor information
            document.Add(CreateInvoiceCustomers());

            document.Add(newLine);

            document.Add(CreateInvoiceItems(Invoice.InvoiceItems));

            document.Add(newLine);

            document.Close();

            return file;
        }

        private IElement CreateInvoiceItems(ICollection<InvoiceItem> invoiceInvoiceItems)
        {
            PdfPTable table = new PdfPTable(10);
            int[] colWidth = { 5, 30, 10,7, 10, 10, 10, 7, 10, 10 };
            table.SetWidths(colWidth);
            table.WidthPercentage = 100;

            PdfPCell formattedCell = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_CENTER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            };

            PdfPCell lpCellName = formattedCell;
            lpCellName.Phrase = new Phrase("Lp.", _fontBold);
            table.AddCell(lpCellName);

            PdfPCell invoiceItemNameCellName = formattedCell;
            invoiceItemNameCellName.Phrase = new Phrase("Nazwa towaru lub usługi", _fontBold);
            table.AddCell(invoiceItemNameCellName);

            PdfPCell pkWiUCellName = formattedCell;
            pkWiUCellName.Phrase = new Phrase("PKWiU", _fontBold);
            table.AddCell(pkWiUCellName);

            PdfPCell amountCellName = formattedCell;
            amountCellName.Phrase = new Phrase("Ilość", _fontBold);
            table.AddCell(amountCellName);

            PdfPCell unitCellName = formattedCell;
            unitCellName.Phrase = new Phrase("j.m.", _fontBold);
            table.AddCell(unitCellName);

            PdfPCell netPriceCellName = formattedCell;
            netPriceCellName.Phrase = new Phrase("Cena netto", _fontBold);
            table.AddCell(netPriceCellName);

            PdfPCell netSumCellName = formattedCell;
            netSumCellName.Phrase = new Phrase("Wartość netto", _fontBold);
            table.AddCell(netSumCellName);

            PdfPCell vatCellName = formattedCell;
            vatCellName.Phrase = new Phrase("VAT [%]", _fontBold);
            table.AddCell(vatCellName);

            PdfPCell vatSumCellName = formattedCell;
            vatSumCellName.Phrase = new Phrase("Wartość VAT", _fontBold);
            table.AddCell(vatSumCellName);

            PdfPCell grossSumCellName = formattedCell;
            grossSumCellName.Phrase = new Phrase("Wartość brutto", _fontBold);
            table.AddCell(grossSumCellName);

            return table;
        }

        private IElement CreateInvoiceCustomers()
        {
            PdfPTable table = new PdfPTable(6);

            int[] colWidth = { 20, 20, 10, 20, 20, 10 };
            table.SetWidths(colWidth);
            table.WidthPercentage = 100;
            PdfPCell blankCell = new PdfPCell()
            {
                Border = 0,
                Padding = 2
            };

            PdfPCell sellerNameCellName = new PdfPCell(new Phrase("SPRZEDAWCA:  ", _fontBold))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0
            };
            table.AddCell(sellerNameCellName);

            PdfPCell sellerNameCell = new PdfPCell(new Phrase(Invoice.Seller.Name, _fontBold))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                Border = 0
            };
            table.AddCell(sellerNameCell);

            table.AddCell(blankCell);

            PdfPCell customerNameCellName = new PdfPCell(new Phrase("NABYWCA:  ", _fontBold))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0
            };
            table.AddCell(customerNameCellName);

            PdfPCell customerNameCell = new PdfPCell(new Phrase(Invoice.Contractor.Name, _fontBold))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                Border = 0
            };
            table.AddCell(customerNameCell);

            table.AddCell(blankCell);

            PdfPCell sellerAddressCellName = new PdfPCell(new Phrase("Adres:  ", _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0
            };
            table.AddCell(sellerAddressCellName);

            var sellerAddress = CreateAddress(Invoice.Seller);
            PdfPCell sellerAddressCell = new PdfPCell(new Phrase(sellerAddress, _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                Border = 0
            };
            table.AddCell(sellerAddressCell);

            table.AddCell(blankCell);

            PdfPCell customerAddressCellName = new PdfPCell(new Phrase("Adres: ", _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0
            };
            table.AddCell(customerAddressCellName);

            var customerAddress = CreateAddress(Invoice.Contractor);
            PdfPCell customerAddressCell = new PdfPCell(new Phrase(customerAddress, _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                Border = 0
            };
            table.AddCell(customerAddressCell);

            table.AddCell(blankCell);

            PdfPCell sellerNIPCellName = new PdfPCell(new Phrase("NIP:  ", _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0
            };
            table.AddCell(sellerNIPCellName);

            PdfPCell sellerNIPCell = new PdfPCell(new Phrase(Invoice.Seller.NIP, _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                Border = 0
            };
            table.AddCell(sellerNIPCell);

            table.AddCell(blankCell);

            PdfPCell customerNIPCellName = new PdfPCell(new Phrase("NIP:  ", _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0
            };
            table.AddCell(customerNIPCellName);

            PdfPCell customerNIPCell = new PdfPCell(new Phrase(Invoice.Contractor.NIP, _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                Border = 0
            };
            table.AddCell(customerNIPCell);

            table.AddCell(blankCell);

            return table;
        }

        private string CreateAddress(Customer customer)
        {
             var address = $"{customer.Street} {customer.StreetNumber} \n{customer.ZipCode} {customer.City}";

            return address;
        }

        private IElement CreateInvoiceDates()
        {
            PdfPTable table = new PdfPTable(2);

            int[] colWidth = {70, 15};
            table.SetWidths(colWidth);
            table.WidthPercentage = 100;

            PdfPCell invoiceDateCellName = new PdfPCell(new Phrase("Data wystawienia: ", _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0,
            };
            table.AddCell(invoiceDateCellName);

            PdfPCell invoiceDateCell = new PdfPCell(new Phrase(Invoice.InvoiceDate.ToString("dd.MM.yyyy"), _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0,
            };
            table.AddCell(invoiceDateCell);

            PdfPCell invoicePaymentDateCellName = new PdfPCell(new Phrase("Termin płatności: ", _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0,
            };
            table.AddCell(invoicePaymentDateCellName);

            PdfPCell invoicePaymentDateCell = new PdfPCell(new Phrase(Invoice.PaymentDeadline.ToString("dd.MM.yyyy"), _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0,
            };
            table.AddCell(invoicePaymentDateCell);

            PdfPCell invoiceNumberCellName = new PdfPCell(new Phrase("Numer faktury VAT: ", _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0,
            };
            table.AddCell(invoiceNumberCellName);

            PdfPCell invoiceNumberCell = new PdfPCell(new Phrase(Invoice.InvoiceNumber, _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0,
            };
            table.AddCell(invoiceNumberCell);

            PdfPCell invoicePlaceCellName = new PdfPCell(new Phrase("Miejsce wystawienia: ", _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0,
            };
            table.AddCell(invoicePlaceCellName);

            PdfPCell invoicePlaceDateCell = new PdfPCell(new Phrase(Invoice.Seller.City, _defaultFont))
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0,
            };
            table.AddCell(invoicePlaceDateCell);

            return table;
        }

        public IElement CreateTitle(string title)
        {
            PdfPTable table = new PdfPTable(1);

            int[] colWidth = { 100 };
            table.SetWidths(colWidth);
            table.WidthPercentage = 100;

            string invoiceTitle = " FAKTURA VAT nr " + Invoice.InvoiceNumber;

            PdfPCell cell = new PdfPCell(new Phrase(invoiceTitle, _bigFontBold))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 2,
                Border = 0
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(title, _bigFont))
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
