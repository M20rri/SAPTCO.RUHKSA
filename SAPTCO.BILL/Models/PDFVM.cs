using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using ZXing;
using System.Text;
using System.Security.Cryptography;
using System;
using ZXing.QrCode;
using ZXing.Common;
using System.Collections.Generic;

namespace SAPTCO.BILL.Models
{
    public class InvoiceVM
    {
        public string id { get; set; }
        public DateTime timeStamp { get; set; }
        public string createdAt { get; set; }
        public string createdTime { get; set; }
        public string cost { get; set; }
        public string countTiket { get; set; }
        public string taxValue { get; set; }
        public string withVatTotal { get; set; }
        public string withoutVatTotal { get; set; }
        public string detail { get; set; }
        public string CustomerName { get; set; }
        public string Line { get; set; }
        public string StartingPoint { get; set; }
        public string Hotel { get; set; }
        public string TripTime { get; set; }
        public string Tel { get; set; }
        public string StartingPointEn { get; set; }
        public string HotelEn { get; set; }
    }

    public class tickets
    {
        public int TicketID { get; set; }
        public int BillID { get; set; }
        public DateTime CDate { get; set; }



    }

    public class PDFVM
    {
        public string billNumber { get; set; } = "100351";

        public string reserveNumber { get; set; } = "26999146";

        public string billCreateAt { get; set; } = "01-11-2021";

        public string billCreateTime { get; set; } = "15:44:00";

        public string vATNumber { get; set; } = "300004441600003";

        public string withVatTotal { get; set; } = "-219.0 ";

        public string quantity { get; set; } = "1";

        public string vatPercentage { get; set; } = "15.0";

        public string vatValue { get; set; } = "20";

        public string unitPrice { get; set; } = "190.43";

        public string detail { get; set; } = "02166629079";

        public string withoutVatTotal { get; set; } = "- 190.43";

        public string taxValue { get; set; } = "-28.57";

        public string CustomerName { get; set; } = "Medhat Ali";
        public string Line { get; set; } = "1";
        public string StartingPoint { get; set; } = "صاله 1";
        public string Hotel { get; set; } = "فندق برج رافال";
        public string TripTime { get; set; } = "14:00";
        public string Tel { get; set; } = "05696969696";
        public string StartingPointEn { get; set; } = "Terminal 1";
        public string HotelEn { get; set; } = "Burj Rafal Riyadh";



        public PDFVM(string _billNumber, string _billCreateAt, string _unitPrice, string _withVatTotal, string _vatValue, string _taxValue, string _withoutVatTotal, string _detail, string _billCreateTime, string _CustomerName, string _Line, string _StartingPoint, string _Hotel, string _TripTime, string _Tel, string _StartingPointEn, string _HotelEn, string _countTicket)
        {
            billNumber = _billNumber;
            billCreateAt = _billCreateAt;
            unitPrice = _unitPrice;
            withVatTotal = _withVatTotal;
            vatValue = _vatValue;
            taxValue = _taxValue;
            withoutVatTotal = _withoutVatTotal;
            detail = _detail;
            billCreateTime = _billCreateTime;
            CustomerName = _CustomerName;
            Line = _Line;
            StartingPoint = _StartingPoint;
            Hotel = _Hotel;
            TripTime = _TripTime;
            Tel = _Tel;
            StartingPointEn = _StartingPointEn;
            HotelEn = _HotelEn;
            quantity = _countTicket;
        }
    }

    public static class Traversehtml
    {
        public static string CreditNote(PDFVM model)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Files/simplified_invoice_barcode.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{billNumber}", model.billNumber);
            body = body.Replace("{reserveNumber}", model.reserveNumber);
            body = body.Replace("{billCreateAt}", model.billCreateAt);
            body = body.Replace("{billCreateTime}", model.billCreateTime);
            body = body.Replace("{vATNumber}", model.vATNumber);
            body = body.Replace("{WithVatTotal}", model.withVatTotal);
            body = body.Replace("{quantity}", model.quantity);
            body = body.Replace("{vatPercentage}", model.vatPercentage);
            body = body.Replace("{unitPrice}", model.unitPrice);
            body = body.Replace("{detail}", model.detail);
            body = body.Replace("{withoutVatTotal}", model.withoutVatTotal);
            body = body.Replace("{taxValue}", model.taxValue);
            body = body.Replace("{vatValue}", model.vatValue);
            body = body.Replace("{QrCode}", ConfigurationManager.AppSettings["BARCODE_URL"].ToString());
            body = body.Replace("{QrCode2}", ConfigurationManager.AppSettings["BARCODE_URL2"].ToString());
            body = body.Replace("{vatValue}", model.vatValue);
            body = body.Replace("{Line}", model.Line);
            body = body.Replace("{StartingPoint}", model.StartingPoint);
            body = body.Replace("{Hotel}", model.Hotel);
            string result = model.TripTime.Substring(0, 5);
            body = body.Replace("{TripTime}", result);
            body = body.Replace("{Tel}", model.Tel);
            body = body.Replace("{StartingPointEn}", model.StartingPointEn);
            body = body.Replace("{HotelEn}", model.HotelEn);
            body = body.Replace("{CustomerName}", model.CustomerName);


            return body;
        }


        public static string CreateHyperPayNoteCreditNote(string trData, string trSecion, HyperPayInvoice model)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Files/HyperPayInvoice.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{billTitleAr}", model.TitleAr);
            body = body.Replace("{billTitleEn}", model.TitleEn);
            body = body.Replace("{billNumber}", model.InvoiceId.ToString());
            body = body.Replace("{billCreateAt}", model.CreatedAt);
            body = body.Replace("{billCreateTime}", model.CreatedTime);
            body = body.Replace("{tbody}", trData);
            body = body.Replace("{tSection}", trSecion);
            body = body.Replace("{withoutVatTotal}", model.TotalBeforeVat.ToString());
            body = body.Replace("{vatValue}", model.VatAmount.ToString());
            body = body.Replace("{WithVatTotal}", model.TotalIncludingVat.ToString());
            body = body.Replace("{QrCode}", ConfigurationManager.AppSettings["BARCODE_URL"].ToString());


            return body;
        }

        public static string CreateHyperPayNoteCreditNoteTicket(string trData)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Files/Ticket.html")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{body}", trData);
            return body;
        }

        public static void ToImage(string code, int size = 180)
        {
            var QCwriter2 = new BarcodeWriter();
            QCwriter2.Format = BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions options = new QrCodeEncodingOptions()
            {
                DisableECI = true,
                CharacterSet = "UTF-8"
            };

            QCwriter2.Options = options;
            var result2 = QCwriter2.Write(code);
            string path2 = HttpContext.Current.Server.MapPath("~/assets/MicrosoftTeams-image1.png");
            var barcodeBitmap2 = new Bitmap(result2);

            using (MemoryStream memory2 = new MemoryStream())
            {
                using (FileStream fs2 = new FileStream(path2, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap2.Save(memory2, ImageFormat.Jpeg);
                    byte[] bytes2 = memory2.ToArray();
                    fs2.Write(bytes2, 0, bytes2.Length);
                }
            }
        }

        public static void GenerateQRCode(string invoiceNumber)
        {
            var QCwriter = new BarcodeWriter();
            QCwriter.Format = BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions options = new QrCodeEncodingOptions()
            {
                DisableECI = true,
                CharacterSet = "UTF-8"
            };

            QCwriter.Options = options;
            var result = QCwriter.Write(invoiceNumber);
            string path = HttpContext.Current.Server.MapPath("~/assets/MicrosoftTeams-image.png");
            var barcodeBitmap = new Bitmap(result);

            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

        }

        public static string Encrypt(string text)
        {
            string EncrptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byKey = System.Text.Encoding.UTF8.GetBytes(EncrptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string str)
        {

            str = str.Replace(" ", "+");
            string DecryptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray = new byte[str.Length];

            byKey = System.Text.Encoding.UTF8.GetBytes(DecryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(str);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }
    }

}