using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Windows.Media;
using EngineeringThesis.Core.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LiczbyNaSlowaNETCore;

namespace EngineeringThesis.Core.Utility
{
    public class InvoiceTemplate
    {
        public Invoice Invoice;
        private readonly iTextSharp.text.Font _defaultFont;
        private readonly iTextSharp.text.Font _fontBold;
        private readonly iTextSharp.text.Font _fontMini;
        private readonly iTextSharp.text.Font _bigFontBold;
        private readonly iTextSharp.text.Font _bigFont;
        private readonly PdfPCell _blankCell;
        private readonly PdfPCell _formattedCellToRight;
        private string _grossSum;
        private CultureInfo _culture;

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

            _culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            _culture.NumberFormat.NumberDecimalSeparator = ",";
            _culture.NumberFormat.NumberGroupSeparator = ".";

            _blankCell = new PdfPCell
            {
                Border = 0,
                Padding = 2
            };

            _formattedCellToRight = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Padding = 2,
                Border = 0
            };
        }

        public string CreatePdf(Utility.InvoiceTypeTemplateEnum invoiceType)
        {
            var directory = "Faktury";
            var invoiceDirectory = directory + @"\" + Invoice.InvoiceNumber.Replace("/", "-") + @"\";
            var newLine = new Paragraph("\n");
            bool exists = System.IO.Directory.Exists(directory);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(directory);
            }
            exists = System.IO.Directory.Exists(directory);
            if (exists)
            {
                bool existsInvoice = Directory.Exists(invoiceDirectory);
                if (!existsInvoice)
                {
                    Directory.CreateDirectory(invoiceDirectory);
                }
            }

            string file;
            if (invoiceType == Utility.InvoiceTypeTemplateEnum.Original)
            {
               file = invoiceDirectory + Invoice.InvoiceNumber.Replace("/", "-") + ".pdf";
            }
            else
            {
                file = invoiceDirectory + Invoice.InvoiceNumber.Replace("/", "-") + " - Kopia" + ".pdf";
            }
            

            Document document = new Document(iTextSharp.text.PageSize.A4, 50, 50, 70, 10); // 10: Margins Left, Right, Top, Bottom
            PdfWriter.GetInstance(document, new FileStream(file, FileMode.Create));

            document.Open();

            //create invoice title
            if (invoiceType == Utility.InvoiceTypeTemplateEnum.Original)
            {
                document.Add(CreateTitle("Oryginał"));
            }
            else
            {
                document.Add(CreateTitle("Kopia"));
            }
            

            document.Add(newLine);

            //create invoice date, invoice number and city
            document.Add(CreateInvoiceDates());

            document.Add(newLine);

            //create seller and contractor information
            document.Add(CreateInvoiceCustomers());

            document.Add(newLine);

            //create invoice items
            document.Add(CreateInvoiceItemsTable());

            document.Add(CreateInvoiceItems(Invoice.InvoiceItems.ToList()));

            document.Add(CreateInvoiceItemsSummary(Invoice.InvoiceItems.ToList()));

            document.Add(newLine);

            document.Add(CreateInvoiceSummary());

            for (int i = 0; i < 8; i++)
            {
                document.Add(newLine);
            }

            document.Add(CreateCustomersSignature());

            document.Close();

            return file;
        }

        private IElement CreateCustomersSignature()
        {
            PdfPTable table = new PdfPTable(3);

            int[] colWidth = { 20, 50 ,20 };
            table.SetWidths(colWidth);
            table.WidthPercentage = 100;

            Phrase dots = new Phrase("...........................................", _defaultFont);

            PdfPCell formattedCellToLeft = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_LEFT,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 2,
                Border = 0
            };

            PdfPCell formattedCellToRight = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_RIGHT,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 2,
                Border = 0
            };

            PdfPCell dotCell = formattedCellToLeft;
            dotCell.Phrase = dots;
            table.AddCell(dotCell);

            PdfPCell emptyCell = formattedCellToLeft;
            emptyCell.Phrase = new Phrase("", _bigFont);
            table.AddCell(emptyCell);

            dotCell = formattedCellToRight;
            dotCell.Phrase = dots;
            table.AddCell(dotCell);

            PdfPCell contractorCell = formattedCellToLeft;
            contractorCell.Phrase = new Phrase("Podpis osoby upoważnionej \n do odbioru faktury VAT", _defaultFont);
            table.AddCell(contractorCell);

            emptyCell.Phrase = new Phrase("", _bigFont);
            table.AddCell(emptyCell);

            PdfPCell sellerCell = formattedCellToRight;
            sellerCell.Phrase = new Phrase("Podpis osoby upoważnionej \n do wystawienia faktury VAT", _defaultFont);
            table.AddCell(sellerCell);

            return table;
        }

        private IElement CreateInvoiceSummary()
        {
            PdfPTable table = new PdfPTable(2);

            int[] colWidth = { 20,70 };
            table.SetWidths(colWidth);
            table.WidthPercentage = 100;

            PdfPCell formattedCell = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_LEFT,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                Border = 0
            };

            PdfPCell payCellName = formattedCell;
            payCellName.Phrase = new Phrase("Do zapłaty: ", _bigFontBold);
            table.AddCell(payCellName);

            PdfPCell payCell = formattedCell;
            payCell.Phrase = new Phrase(_grossSum, _bigFontBold);
            table.AddCell(payCell);

            PdfPCell inWordsCellName = formattedCell;
            inWordsCellName.Phrase = new Phrase("Słownie: ", _fontMini);
            table.AddCell(inWordsCellName);

            var amountInWords = CreateAmountInWords();
            PdfPCell inWordsCell = formattedCell;
            inWordsCell.Phrase = new Phrase(amountInWords, _fontMini);
            table.AddCell(inWordsCell);

            PdfPCell blank = _blankCell;
            blank.Phrase = new Phrase(" ", _defaultFont);
            table.AddCell(blank);
            table.AddCell(blank);

            PdfPCell paymentTypeCellName = formattedCell;
            paymentTypeCellName.Phrase = new Phrase("Forma płatności: ", _defaultFont);
            table.AddCell(paymentTypeCellName);

            PdfPCell paymentTypeCell = formattedCell;
            paymentTypeCell.Phrase = new Phrase(Invoice.PaymentType.Name, _defaultFont);
            table.AddCell(paymentTypeCell);

            PdfPCell paymentDateCellName = formattedCell;
            paymentDateCellName.Phrase = new Phrase("Zapłacono: ", _defaultFont);
            table.AddCell(paymentDateCellName);

            PdfPCell paymentDateCell = formattedCell;
            if (Invoice.PaymentDate != null)
            {
                paymentDateCell.Phrase = new Phrase(Invoice.PaymentDate.Value.ToString("dd.MM.yyyy"), _defaultFont);
            }
            else
            {
                paymentDateCell.Phrase = new Phrase("Nie zapłacono", _defaultFont);
            }
            table.AddCell(paymentDateCell);

            PdfPCell bankAccountCellName = formattedCell;
            bankAccountCellName.Phrase = new Phrase("Numer konta bankowego: ", _defaultFont);
            table.AddCell(bankAccountCellName);

            PdfPCell bankAccountCell = formattedCell;
            if (Invoice.Seller.BankAccountNumber != null)
            {
                var bankAccount = decimal.Parse(Invoice.Seller.BankAccountNumber);
                bankAccountCell.Phrase = new Phrase($"{bankAccount:## #### #### #### #### #### ####}", _defaultFont);
            }
            else
            {
                bankAccountCell.Phrase = new Phrase("Brak", _defaultFont);
            }
            table.AddCell(bankAccountCell);


            return table;
        }

        private IElement CreateInvoiceItemsSummary(List<InvoiceItem> invoiceItems)
        {
            PdfPTable table = new PdfPTable(10);

            int[] colWidth = { 7, 30, 10, 7, 10, 10, 10, 7, 10, 10 };
            table.SetWidths(colWidth);
            table.WidthPercentage = 100;

            PdfPCell formattedCellCentered = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_CENTER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 4
            };

            for (int i = 0; i < 5; i++)
            {
                table.AddCell(_blankCell);
            }

            PdfPCell sumCellName = new PdfPCell(new Phrase("RAZEM: ", _fontBold))
            {
                VerticalAlignment = Element.ALIGN_CENTER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 4,
                Border = 0
            };
            table.AddCell(sumCellName);

            var sumNet = CalculateNetSum(Invoice.InvoiceItems.ToList());
            PdfPCell sumNetCell = formattedCellCentered;
            sumNetCell.Phrase = new Phrase(sumNet, _fontBold);
            table.AddCell(sumNetCell);

            table.AddCell(_blankCell);

            var vatSum = CalculateVatSum(Invoice.InvoiceItems.ToList());
            PdfPCell vatSumCell = formattedCellCentered;
            vatSumCell.Phrase = new Phrase(vatSum, _fontBold);
            table.AddCell(vatSumCell);

            _grossSum = CalculateGrossSum(Invoice.InvoiceItems.ToList());
            PdfPCell grossSumCell = formattedCellCentered;
            grossSumCell.Phrase = new Phrase(_grossSum, _fontBold);
            table.AddCell(grossSumCell);

            return table;
        }

        private IElement CreateInvoiceItems(List<InvoiceItem> invoiceItems)
        {
            PdfPTable table = new PdfPTable(10);

            int[] colWidth = { 7, 30, 10, 7, 10, 10, 10, 7, 10, 10 };
            table.SetWidths(colWidth);
            table.WidthPercentage = 100;

            PdfPCell formattedCellCentered = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_CENTER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 4
            };

            for (int i = 0; i < invoiceItems.Count; i++)
            {
                PdfPCell lpCellName = formattedCellCentered;
                lpCellName.Phrase = new Phrase((i + 1).ToString(), _defaultFont);
                table.AddCell(lpCellName);

                PdfPCell invoiceItemNameCellName = new PdfPCell(new Phrase(invoiceItems[i].Name, _defaultFont))
                {
                    VerticalAlignment = Element.ALIGN_LEFT,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 4
                };
                table.AddCell(invoiceItemNameCellName);

                PdfPCell pkWiUCellName = formattedCellCentered;
                pkWiUCellName.Phrase = new Phrase(invoiceItems[i].PKWiU, _defaultFont);
                table.AddCell(pkWiUCellName);

                PdfPCell amountCellName = formattedCellCentered;
                amountCellName.Phrase = new Phrase(invoiceItems[i].Amount.ToString(), _defaultFont);
                table.AddCell(amountCellName);

                PdfPCell unitCellName = formattedCellCentered;
                unitCellName.Phrase = new Phrase(invoiceItems[i].Unit, _defaultFont);
                table.AddCell(unitCellName);

                PdfPCell netPriceCellName = formattedCellCentered;
                netPriceCellName.Phrase = new Phrase(invoiceItems[i].NetPrice, _defaultFont);
                table.AddCell(netPriceCellName);

                PdfPCell netSumCellName = formattedCellCentered;
                netSumCellName.Phrase = new Phrase(invoiceItems[i].NetSum, _defaultFont);
                table.AddCell(netSumCellName);

                PdfPCell vatCellName = formattedCellCentered;
                vatCellName.Phrase = new Phrase(invoiceItems[i].VAT.ToString(), _defaultFont);
                table.AddCell(vatCellName);

                PdfPCell vatSumCellName = formattedCellCentered;
                vatSumCellName.Phrase = new Phrase(invoiceItems[i].VATSum, _defaultFont);
                table.AddCell(vatSumCellName);

                PdfPCell grossSumCellName = formattedCellCentered;
                grossSumCellName.Phrase = new Phrase(invoiceItems[i].GrossSum, _defaultFont);
                table.AddCell(grossSumCellName);
            }

            return table;
        }

        private IElement CreateInvoiceItemsTable()
        {
            PdfPTable table = new PdfPTable(10);
            int[] colWidth = { 7, 30, 10,7, 10, 10, 10, 7, 10, 10 };
            table.SetWidths(colWidth);
            table.WidthPercentage = 100;

            PdfPCell formattedCellCentered = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_CENTER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            };

            PdfPCell lpCellName = formattedCellCentered;
            lpCellName.Phrase = new Phrase("Lp.", _fontBold);
            table.AddCell(lpCellName);

            PdfPCell invoiceItemNameCellName = formattedCellCentered;
            invoiceItemNameCellName.Phrase = new Phrase("Nazwa towaru lub usługi", _fontBold);
            table.AddCell(invoiceItemNameCellName);

            PdfPCell pkWiUCellName = formattedCellCentered;
            pkWiUCellName.Phrase = new Phrase("PKWiU", _fontBold);
            table.AddCell(pkWiUCellName);

            PdfPCell amountCellName = formattedCellCentered;
            amountCellName.Phrase = new Phrase("Ilość", _fontBold);
            table.AddCell(amountCellName);

            PdfPCell unitCellName = formattedCellCentered;
            unitCellName.Phrase = new Phrase("j.m.", _fontBold);
            table.AddCell(unitCellName);

            PdfPCell netPriceCellName = formattedCellCentered;
            netPriceCellName.Phrase = new Phrase("Cena netto", _fontBold);
            table.AddCell(netPriceCellName);

            PdfPCell netSumCellName = formattedCellCentered;
            netSumCellName.Phrase = new Phrase("Wartość netto", _fontBold);
            table.AddCell(netSumCellName);

            PdfPCell vatCellName = formattedCellCentered;
            vatCellName.Phrase = new Phrase("VAT [%]", _fontBold);
            table.AddCell(vatCellName);

            PdfPCell vatSumCellName = formattedCellCentered;
            vatSumCellName.Phrase = new Phrase("Wartość VAT", _fontBold);
            table.AddCell(vatSumCellName);

            PdfPCell grossSumCellName = formattedCellCentered;
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

            PdfPCell formattedCell = new PdfPCell
            {
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                Border = 0
            };

            PdfPCell sellerNameCellName = _formattedCellToRight;
            sellerNameCellName.Phrase = new Phrase("SPRZEDAWCA:  ", _fontBold);
            table.AddCell(sellerNameCellName);

            PdfPCell sellerNameCell = formattedCell;
            sellerNameCell.Phrase = new Phrase(Invoice.Seller.Name, _fontBold);
            table.AddCell(sellerNameCell);

            table.AddCell(_blankCell);

            PdfPCell customerNameCellName = _formattedCellToRight;
            customerNameCellName.Phrase = new Phrase("NABYWCA:  ", _fontBold);
            table.AddCell(customerNameCellName);

            PdfPCell customerNameCell = formattedCell;
            customerNameCell.Phrase = new Phrase(Invoice.Contractor.Name, _fontBold);
            table.AddCell(customerNameCell);

            table.AddCell(_blankCell);

            PdfPCell sellerAddressCellName = _formattedCellToRight;
            sellerAddressCellName.Phrase = new Phrase("Adres:  ", _defaultFont);
            table.AddCell(sellerAddressCellName);

            var sellerAddress = CreateAddress(Invoice.Seller);
            PdfPCell sellerAddressCell = formattedCell;
            sellerAddressCell.Phrase = new Phrase(sellerAddress, _defaultFont);
            table.AddCell(sellerAddressCell);

            table.AddCell(_blankCell);

            PdfPCell customerAddressCellName = _formattedCellToRight;
            customerAddressCellName.Phrase = new Phrase("Adres: ", _defaultFont);
            table.AddCell(customerAddressCellName);

            var customerAddress = CreateAddress(Invoice.Contractor);
            PdfPCell customerAddressCell = formattedCell;
            customerAddressCell.Phrase = new Phrase(customerAddress, _defaultFont);
            table.AddCell(customerAddressCell);

            table.AddCell(_blankCell);

            PdfPCell sellerNIPCellName = _formattedCellToRight;
            sellerNIPCellName.Phrase = new Phrase("NIP:  ", _defaultFont);
            table.AddCell(sellerNIPCellName);

            PdfPCell sellerNIPCell = formattedCell;
            sellerNIPCell.Phrase = new Phrase(Invoice.Seller.NIP, _defaultFont);
            table.AddCell(sellerNIPCell);

            table.AddCell(_blankCell);

            PdfPCell customerNIPCellName = _formattedCellToRight;
            customerNIPCellName.Phrase = new Phrase("NIP:  ", _defaultFont);
            table.AddCell(customerNIPCellName);

            PdfPCell customerNIPCell = formattedCell;
            customerNIPCell.Phrase = new Phrase(Invoice.Contractor.NIP, _defaultFont);
            table.AddCell(customerNIPCell);

            table.AddCell(_blankCell);

            return table;
        }

        private IElement CreateInvoiceDates()
        {
            PdfPTable table = new PdfPTable(2);

            int[] colWidth = {70, 15};
            table.SetWidths(colWidth);
            table.WidthPercentage = 100;

            PdfPCell invoiceDateCellName = _formattedCellToRight;
            invoiceDateCellName.Phrase = new Phrase("Data wystawienia: ", _defaultFont);
            table.AddCell(invoiceDateCellName);

            PdfPCell invoiceDateCell = _formattedCellToRight;
            invoiceDateCell.Phrase = new Phrase(Invoice.InvoiceDate.ToString("dd.MM.yyyy"), _defaultFont);
            table.AddCell(invoiceDateCell);

            PdfPCell invoicePaymentDateCellName = _formattedCellToRight;
            invoicePaymentDateCellName.Phrase = new Phrase("Termin płatności: ", _defaultFont);
            table.AddCell(invoicePaymentDateCellName);

            PdfPCell invoicePaymentDateCell = _formattedCellToRight;
            invoicePaymentDateCell.Phrase = new Phrase(Invoice.PaymentDeadline.ToString("dd.MM.yyyy"), _defaultFont);
            table.AddCell(invoicePaymentDateCell);

            PdfPCell invoiceNumberCellName = _formattedCellToRight;
            invoiceNumberCellName.Phrase = new Phrase("Numer faktury VAT: ", _defaultFont);
            table.AddCell(invoiceNumberCellName);

            PdfPCell invoiceNumberCell = _formattedCellToRight;
            invoiceNumberCell.Phrase = new Phrase(Invoice.InvoiceNumber, _defaultFont);
            table.AddCell(invoiceNumberCell);

            PdfPCell invoicePlaceCellName = _formattedCellToRight;
            invoicePlaceCellName.Phrase = new Phrase("Miejsce wystawienia: ", _defaultFont);
            table.AddCell(invoicePlaceCellName);

            PdfPCell invoicePlaceDateCell = _formattedCellToRight;
            invoicePlaceDateCell.Phrase = new Phrase(Invoice.Seller.City, _defaultFont);
            table.AddCell(invoicePlaceDateCell);

            return table;
        }

        private IElement CreateTitle(string invoiceType)
        {
            PdfPTable table = new PdfPTable(1);

            int[] colWidth = { 100 };
            table.SetWidths(colWidth);
            table.WidthPercentage = 100;

            string invoiceTitle = " FAKTURA VAT nr " + Invoice.InvoiceNumber;

            PdfPCell invoiceTitleCell = new PdfPCell(new Phrase(invoiceTitle, _bigFontBold))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 2,
                Border = 0
            };
            table.AddCell(invoiceTitleCell);
            PdfPCell invoiceTypeCell = new PdfPCell(new Phrase(invoiceType, _bigFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 2,
                Border = 0
            };
            table.AddCell(invoiceTypeCell);

            return table;
        }

        private string CreateAddress(Customer customer)
        {
            var address = $"{customer.Street} {customer.StreetNumber} \n{customer.ZipCode} {customer.City}";

            return address;
        }

        private string CalculateGrossSum(List<InvoiceItem> invoiceItems)
        {
            decimal sum = 0;
            foreach (var invoiceItem in invoiceItems)
            {
                sum += Convert.ToDecimal(invoiceItem.GrossSum, _culture);
            }

            return sum.ToString("#.00", new CultureInfo("pl"));
        }

        private string CalculateVatSum(List<InvoiceItem> invoiceItems)
        {
            decimal sum = 0;
            foreach (var invoiceItem in invoiceItems)
            {
                sum += Convert.ToDecimal(invoiceItem.VATSum, _culture);
            }

            return sum.ToString("#.00", new CultureInfo("pl"));
        }

        private string CalculateNetSum(List<InvoiceItem> invoiceItems)
        {
            decimal sum = 0;
            foreach (var invoiceItem in invoiceItems)
            {
                sum += Convert.ToDecimal(invoiceItem.NetSum, _culture);
            }

            return sum.ToString("#.00", new CultureInfo("pl"));
        }

        private string CreateAmountInWords()
        {
            var options = new NumberToTextOptions
            {
                Stems = true,
                Currency = Currency.PLN
            };

            return NumberToText.Convert(Convert.ToDecimal(_grossSum, _culture), options);
        }
    }
}
