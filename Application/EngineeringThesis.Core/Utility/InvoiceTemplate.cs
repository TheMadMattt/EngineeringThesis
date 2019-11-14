using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using EngineeringThesis.Core.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LiczbyNaSlowaNETCore;

namespace EngineeringThesis.Core.Utility
{
    public class InvoiceTemplate
    {
        private readonly Invoice _invoice;
        private Utility.InvoiceTemplateStruct _template;
        private readonly Font _defaultFont;
        private readonly Font _fontBold;
        private readonly Font _fontMini;
        private readonly Font _bigFontBold;
        private readonly Font _bigFont;
        private readonly PdfPCell _blankCell;
        private readonly PdfPCell _formattedCellToRight;
        private string _grossSum;
        private readonly CultureInfo _culture;

        public InvoiceTemplate(Invoice invoice)
        {
            _invoice = invoice;

            BaseFont regularFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
            BaseFont boldFont = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, BaseFont.EMBEDDED);
            _defaultFont = new Font(regularFont, 9);
            _fontBold = new Font(boldFont, 9);
            _fontMini = new Font(regularFont, 7);
            _bigFontBold = new Font(boldFont, 14);
            _bigFont = new Font(regularFont, 14);

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

        public string CreatePdf(Utility.InvoiceTemplateStruct template)
        {
            _template = template;
            var directory = "Faktury";
            var invoiceDirectory = directory + "/" + _invoice.InvoiceNumber.Replace("/", "-") + @"\";
            var newLine = new Paragraph("\n");
            bool exists = Directory.Exists(directory);
            if (!exists)
            {
                Directory.CreateDirectory(directory);
            }
            exists = Directory.Exists(directory);
            if (exists)
            {
                bool existsInvoice = Directory.Exists(invoiceDirectory);
                if (!existsInvoice)
                {
                    Directory.CreateDirectory(invoiceDirectory);
                }
            }

            string file;
            if (_template.InvoiceType == Utility.InvoiceTypeTemplateEnum.Original)
            {
                if (_template.InvoiceTitle == Utility.InvoiceTypeTemplateEnum.Proforma)
                {
                    file = invoiceDirectory + _invoice.InvoiceNumber.Replace("/", "-") + "-proforma" + ".pdf";
                }
                else if (_template.InvoiceTitle == Utility.InvoiceTypeTemplateEnum.Correction)
                {
                    file = invoiceDirectory + _invoice.InvoiceNumber.Replace("/", "-") + "-korekta" + ".pdf";
                }
                else
                {
                    file = invoiceDirectory + _invoice.InvoiceNumber.Replace("/", "-") + ".pdf";
                }

            }
            else
            {
                if (_template.InvoiceTitle == Utility.InvoiceTypeTemplateEnum.Proforma)
                {
                    file = invoiceDirectory + _invoice.InvoiceNumber.Replace("/", "-") + "-proforma-Kopia" + ".pdf";
                }
                else if (_template.InvoiceTitle == Utility.InvoiceTypeTemplateEnum.Correction)
                {
                    file = invoiceDirectory + _invoice.InvoiceNumber.Replace("/", "-") + "-korekta-Kopia" + ".pdf";
                }
                else
                {
                    file = invoiceDirectory + _invoice.InvoiceNumber.Replace("/", "-") + "-Kopia" + ".pdf";
                }
            }

            string invoiceTitle;
            if (_template.InvoiceTitle == Utility.InvoiceTypeTemplateEnum.Proforma)
            {
                invoiceTitle = " FAKTURA PRO FORMA nr " + _invoice.InvoiceNumber;
            }
            else if (_template.InvoiceTitle == Utility.InvoiceTypeTemplateEnum.Correction)
            {
                invoiceTitle = " Korekta do FAKTURY VAT nr " + _invoice.InvoiceNumber;
            }
            else
            {
                invoiceTitle = " FAKTURA VAT nr " + _invoice.InvoiceNumber;
            }


            Document document = new Document(PageSize.A4, 50, 50, 70, 10);
            PdfWriter.GetInstance(document, new FileStream(file, FileMode.Create));

            document.Open();

            //create invoice title
            document.Add(_template.InvoiceType == Utility.InvoiceTypeTemplateEnum.Original
                ? CreateTitle("Oryginał", invoiceTitle)
                : CreateTitle("Kopia", invoiceTitle));


            document.Add(newLine);

            //create invoice date, invoice number and city
            document.Add(CreateInvoiceDates());

            document.Add(newLine);

            //create seller and contractor information
            document.Add(CreateInvoiceCustomers());

            document.Add(newLine);

            //create invoice items
            document.Add(CreateInvoiceItemsTable());

            document.Add(CreateInvoiceItems(_invoice.InvoiceItems.ToList()));

            document.Add(CreateInvoiceItemsSummary());

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

            int[] colWidth = { 20, 50, 20 };
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

            PdfPCell dotCell;
            PdfPCell emptyCell = formattedCellToLeft;
            if (_template.PickupPersonCheckBox)
            {
                dotCell = formattedCellToLeft;
                dotCell.Phrase = dots;
                table.AddCell(dotCell);
            }
            else
            {
                emptyCell.Phrase = new Phrase("", _bigFont);
                table.AddCell(emptyCell);
            }
            

            emptyCell.Phrase = new Phrase("", _bigFont);
            table.AddCell(emptyCell);

            if (_template.InvoicePersonCheckBox)
            {
                dotCell = formattedCellToRight;
                dotCell.Phrase = dots;
                table.AddCell(dotCell);
            }
            else
            {
                emptyCell.Phrase = new Phrase("", _bigFont);
                table.AddCell(emptyCell);
            }

            if (_template.PickupPersonCheckBox)
            {
                PdfPCell contractorCell = formattedCellToLeft;
                contractorCell.Phrase = new Phrase("Podpis osoby upoważnionej \n do odbioru faktury VAT", _defaultFont);
                table.AddCell(contractorCell);
            }
            else
            {
                emptyCell.Phrase = new Phrase("", _bigFont);
                table.AddCell(emptyCell);
            }

            emptyCell.Phrase = new Phrase("", _bigFont);
            table.AddCell(emptyCell);

            if (_template.InvoicePersonCheckBox)
            {
                PdfPCell sellerCell = formattedCellToRight;
                sellerCell.Phrase = new Phrase("Podpis osoby upoważnionej \n do wystawienia faktury VAT", _defaultFont);
                table.AddCell(sellerCell);
            }
            else
            {
                emptyCell.Phrase = new Phrase("", _bigFont);
                table.AddCell(emptyCell);
            }
            

            return table;
        }

        private IElement CreateInvoiceSummary()
        {
            PdfPTable table = new PdfPTable(2);

            int[] colWidth = { 20, 70 };
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
            paymentTypeCell.Phrase = new Phrase(_invoice.PaymentType.Name, _defaultFont);
            table.AddCell(paymentTypeCell);

            if (_invoice.PaymentDate != null)
            {
                PdfPCell paymentDateCellName = formattedCell;
                paymentDateCellName.Phrase = new Phrase("Zapłacono: ", _defaultFont);
                table.AddCell(paymentDateCellName);

                PdfPCell paymentDateCell = formattedCell;

                paymentDateCell.Phrase = new Phrase(_invoice.PaymentDate?.ToString("dd.MM.yyyy"), _defaultFont);

                table.AddCell(paymentDateCell);
            }
            else
            {
                PdfPCell paymentDateCellName = formattedCell;
                paymentDateCellName.Phrase = new Phrase("Termin płatności: ", _defaultFont);
                table.AddCell(paymentDateCellName);

                PdfPCell paymentDateCell = formattedCell;
                paymentDateCell.Phrase = new Phrase("Termin zapłaty upływa dnia: " + _invoice.PaymentDeadline.ToString("dd.MM.yyyy"), _defaultFont);
                table.AddCell(paymentDateCell);
            }

            PdfPCell bankAccountCellName = formattedCell;
            bankAccountCellName.Phrase = new Phrase("Numer konta bankowego: ", _defaultFont);
            table.AddCell(bankAccountCellName);

            PdfPCell bankAccountCell = formattedCell;
            bankAccountCell.Phrase = _invoice.Seller.BankAccountNumber != null ? new Phrase(_invoice.Seller.BankAccountNumber, _defaultFont) : new Phrase("Brak", _defaultFont);
            table.AddCell(bankAccountCell);

            blank.Phrase = new Phrase(" ", _defaultFont);
            table.AddCell(blank);
            table.AddCell(blank);

            if (_template.CommentsCheckBox)
            {
                PdfPCell commentsCellName = formattedCell;
                commentsCellName.Phrase = new Phrase("Uwagi do faktury: ", _fontBold);
                table.AddCell(commentsCellName);

                PdfPCell commentsCell = formattedCell;
                if (_invoice.Comments != null)
                {
                    commentsCell.Phrase = new Phrase(_invoice.Comments, _defaultFont);
                }
                else
                {
                    commentsCell.Phrase = new Phrase("Brak", _defaultFont);
                }
                table.AddCell(commentsCell);
            }

            return table;
        }

        private IElement CreateInvoiceItemsSummary()
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

            var sumNet = CalculateNetSum(_invoice.InvoiceItems.ToList());
            PdfPCell sumNetCell = formattedCellCentered;
            sumNetCell.Phrase = new Phrase(sumNet, _fontBold);
            table.AddCell(sumNetCell);

            table.AddCell(_blankCell);

            var vatSum = CalculateVatSum(_invoice.InvoiceItems.ToList());
            PdfPCell vatSumCell = formattedCellCentered;
            vatSumCell.Phrase = new Phrase(vatSum, _fontBold);
            table.AddCell(vatSumCell);

            _grossSum = CalculateGrossSum(_invoice.InvoiceItems.ToList());
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
            int[] colWidth = { 7, 30, 10, 7, 10, 10, 10, 7, 10, 10 };
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

            int[] colWidth = { 15, 30, 5, 15, 30, 5 };
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
            sellerNameCell.Phrase = new Phrase(_invoice.Seller.Name, _fontBold);
            table.AddCell(sellerNameCell);

            table.AddCell(_blankCell);

            PdfPCell customerNameCellName = _formattedCellToRight;
            customerNameCellName.Phrase = new Phrase("NABYWCA:  ", _fontBold);
            table.AddCell(customerNameCellName);

            PdfPCell customerNameCell = formattedCell;
            customerNameCell.Phrase = new Phrase(_invoice.Contractor.Name, _fontBold);
            table.AddCell(customerNameCell);

            table.AddCell(_blankCell);

            PdfPCell sellerAddressCellName = _formattedCellToRight;
            sellerAddressCellName.Phrase = new Phrase("Adres:  ", _defaultFont);
            table.AddCell(sellerAddressCellName);

            var sellerAddress = CreateAddress(_invoice.Seller);
            PdfPCell sellerAddressCell = formattedCell;
            sellerAddressCell.Phrase = new Phrase(sellerAddress, _defaultFont);
            table.AddCell(sellerAddressCell);

            table.AddCell(_blankCell);

            PdfPCell customerAddressCellName = _formattedCellToRight;
            customerAddressCellName.Phrase = new Phrase("Adres: ", _defaultFont);
            table.AddCell(customerAddressCellName);

            var customerAddress = CreateAddress(_invoice.Contractor);
            PdfPCell customerAddressCell = formattedCell;
            customerAddressCell.Phrase = new Phrase(customerAddress, _defaultFont);
            table.AddCell(customerAddressCell);

            table.AddCell(_blankCell);

            PdfPCell sellerNIPCellName = _formattedCellToRight;
            sellerNIPCellName.Phrase = new Phrase("NIP:  ", _defaultFont);
            table.AddCell(sellerNIPCellName);

            PdfPCell sellerNIPCell = formattedCell;
            sellerNIPCell.Phrase = new Phrase(_invoice.Seller.NIP, _defaultFont);
            table.AddCell(sellerNIPCell);

            table.AddCell(_blankCell);

            PdfPCell customerNIPCellName = _formattedCellToRight;
            customerNIPCellName.Phrase = new Phrase("NIP:  ", _defaultFont);
            table.AddCell(customerNIPCellName);

            PdfPCell customerNIPCell = formattedCell;
            customerNIPCell.Phrase = new Phrase(_invoice.Contractor.NIP, _defaultFont);
            table.AddCell(customerNIPCell);

            table.AddCell(_blankCell);

            return table;
        }

        private IElement CreateInvoiceDates()
        {
            PdfPTable table = new PdfPTable(2);

            int[] colWidth = { 70, 15 };
            table.SetWidths(colWidth);
            table.WidthPercentage = 100;

            PdfPCell invoiceDateCellName = _formattedCellToRight;
            invoiceDateCellName.Phrase = new Phrase("Data sprzedaży: ", _defaultFont);
            table.AddCell(invoiceDateCellName);

            PdfPCell invoiceDateCell = _formattedCellToRight;
            invoiceDateCell.Phrase = new Phrase(_invoice.InvoiceDate.ToString("dd.MM.yyyy"), _defaultFont);
            table.AddCell(invoiceDateCell);

            PdfPCell invoicePaymentDateCellName = _formattedCellToRight;
            invoicePaymentDateCellName.Phrase = new Phrase("Data wystawienia: ", _defaultFont);
            table.AddCell(invoicePaymentDateCellName);

            PdfPCell invoicePaymentDateCell = _formattedCellToRight;
            invoicePaymentDateCell.Phrase = new Phrase(_invoice.InvoiceDate.ToString("dd.MM.yyyy"), _defaultFont);
            table.AddCell(invoicePaymentDateCell);

            PdfPCell invoiceNumberCellName = _formattedCellToRight;
            invoiceNumberCellName.Phrase = new Phrase("Numer faktury VAT: ", _defaultFont);
            table.AddCell(invoiceNumberCellName);

            PdfPCell invoiceNumberCell = _formattedCellToRight;
            invoiceNumberCell.Phrase = new Phrase(_invoice.InvoiceNumber, _defaultFont);
            table.AddCell(invoiceNumberCell);

            PdfPCell invoicePlaceCellName = _formattedCellToRight;
            invoicePlaceCellName.Phrase = new Phrase("Miejsce wystawienia: ", _defaultFont);
            table.AddCell(invoicePlaceCellName);

            PdfPCell invoicePlaceDateCell = _formattedCellToRight;
            invoicePlaceDateCell.Phrase = new Phrase(_invoice.Seller.City, _defaultFont);
            table.AddCell(invoicePlaceDateCell);

            return table;
        }

        private IElement CreateTitle(string invoiceType, string invoiceTitle)
        {
            PdfPTable table = new PdfPTable(1);

            int[] colWidth = { 100 };
            table.SetWidths(colWidth);
            table.WidthPercentage = 100;

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
