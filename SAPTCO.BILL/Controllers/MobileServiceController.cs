using Newtonsoft.Json;
using SAPTCO.BILL.Models;
using SelectPdf;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using log4net;
using System.Collections.Generic;
using System.Text;
using System.Web;
using SAPTCO.BILL.Helper;
using RestSharp;
using SAPTCO.BILL.Entities;
using System.Net;
using SAPTCO.BILL.sa.saptco.services;

namespace SAPTCO.BILL.Controllers
{
    public class MobileServiceController : Controller
    {

        private static ILog _log { get; set; } = LogManager.GetLogger(typeof(MobileServiceController));

        // Login
        [HttpPost]
        public JsonResult Auth(UserVM model)
        {
            _log.Info("Login");

            ResponseMessageVM response = new ResponseMessageVM();
            try
            {
                using (SaptcoContext _db = new SaptcoContext())
                {
                    string query = $"EXECUTE SP_LOGIN '{model.username}','{model.password}'";
                    var currentUser = _db.Database.SqlQuery<UserVM>(query).FirstOrDefault();
                    response = currentUser is null ? new ResponseMessageVM
                    {
                        code = 0,
                        message = "invalid credentials"
                    } : new ResponseMessageVM
                    {
                        code = 1,
                        message = JsonConvert.SerializeObject(currentUser)
                    };
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // Load Services
        [HttpGet]
        public JsonResult Services()
        {
            _log.Info("Load Services");
            List<ServiceVM> serives = new List<ServiceVM>();
            using (SaptcoContext _db = new SaptcoContext())
            {
                try
                {

                    string query = $"EXECUTE SP_SERVICES";
                    serives = _db.Database.SqlQuery<ServiceVM>(query).ToList();
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                }
                return Json(serives, JsonRequestBehavior.AllowGet);
            }
        }

        // Load trip
        [HttpGet]
        public JsonResult trips()
        {
            _log.Info("Load trips");
            List<TripVM> trips = new List<TripVM>();
            using (SaptcoContext _db = new SaptcoContext())
            {
                try
                {

                    string query = $"EXECUTE SP_GetTrip";
                    trips = _db.Database.SqlQuery<TripVM>(query).ToList();
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                }
                return Json(trips, JsonRequestBehavior.AllowGet);
            }
        }

        // Load reminder
        [HttpGet]
        public JsonResult reminder(int id)
        {
            _log.Info("Load reminder");
            int reminder = 0;
            using (SaptcoContext _db = new SaptcoContext())
            {
                try
                {

                    string query = $"EXECUTE SP_GetRemainingTrips {id}";
                    reminder = _db.Database.SqlQuery<int>(query).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                }
                return Json(reminder, JsonRequestBehavior.AllowGet);
            }
        }

        // Load paymint
        [HttpGet]
        public JsonResult paymints()
        {
            _log.Info("Load paymint");
            List<PaymintVM> Paymint = new List<PaymintVM>();
            using (SaptcoContext _db = new SaptcoContext())
            {
                try
                {

                    string query = $"EXECUTE SP_GetPay";
                    Paymint = _db.Database.SqlQuery<PaymintVM>(query).ToList();
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                }
                return Json(Paymint, JsonRequestBehavior.AllowGet);
            }
        }


        // Load paymint
        [HttpPost]
        public JsonResult Hotels(Hotel hotels)
        {
            _log.Info("Load Hotels");
            List<ServiceVM> Hotels = new List<ServiceVM>();
            using (SaptcoContext _db = new SaptcoContext())
            {
                try
                {

                    string query = $"EXECUTE SP_Hotel  {hotels.serviceId}";
                    Hotels = _db.Database.SqlQuery<ServiceVM>(query).ToList();
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                }
                return Json(Hotels, JsonRequestBehavior.AllowGet);
            }
        }

        // Load time
        [HttpPost]
        public JsonResult Time(Hotel hotels)
        {
            _log.Info("Load Times");
            List<ServiceVM> Times = new List<ServiceVM>();
            using (SaptcoContext _db = new SaptcoContext())
            {
                try
                {

                    string query = $"EXECUTE SP_Times {hotels.serviceId}";
                    Times = _db.Database.SqlQuery<ServiceVM>(query).ToList();
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                }
                return Json(Times, JsonRequestBehavior.AllowGet);
            }
        }

        // TimeTrip
        [HttpPost]
        public JsonResult TimeTrip(Hotel hotels)
        {
            _log.Info("Load TimeTrip");
            ServiceVM Times = new ServiceVM();

            using (SaptcoContext _db = new SaptcoContext())
            {
                try
                {
                    string query = $"EXECUTE SP_TimeTrip {hotels.serviceId}";
                    Times = _db.Database.SqlQuery<ServiceVM>(query).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                }
                return Json(Times, JsonRequestBehavior.AllowGet);
            }
        }



        // Load Terminal
        [HttpGet]
        public JsonResult Terminal()
        {
            _log.Info("Load Terminal");
            List<ServiceVM> Terminals = new List<ServiceVM>();
            using (SaptcoContext _db = new SaptcoContext())
            {
                try
                {

                    string query = $"EXECUTE SP_Terminal";
                    Terminals = _db.Database.SqlQuery<ServiceVM>(query).ToList();
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                }
                return Json(Terminals, JsonRequestBehavior.AllowGet);
            }
        }

        // Load Report
        [HttpPost]
        public JsonResult Report(trip trips)
        {
            _log.Info("Load report");
            List<ReportVM> report = new List<ReportVM>();

            List<listSupject> supjects = new List<listSupject>();
            List<listCustomer> customer = new List<listCustomer>();

            using (SaptcoContext _db = new SaptcoContext())
            {
                var temp = "";
                try
                {
                    DateTime dd = DateTime.Parse(trips.date);

                    string query = $"EXECUTE SP_Report {trips.idtrip} , '{dd}', {trips.terminalId}";
                    supjects = _db.Database.SqlQuery<listSupject>(query).ToList();

                    for (int i = 0; i < supjects.Count; i++)
                    {
                        string query2 = $"EXECUTE SP_Reportsup {trips.idtrip},{supjects[i].Id}, '{dd}' , {trips.terminalId}";
                        customer = _db.Database.SqlQuery<listCustomer>(query2).ToList();

                        temp = temp + "<tr><td class='bg-secondary text-light fw-bold'>اسم الفندق</td><td colspan='3' class='bg-secondary text-light fw-bold'>" + supjects[i].Name + "</td></tr>";
                        for (int i2 = 0; i2 < customer.Count; i2++)
                        {
                            int x = i2 + 1;
                            temp = temp + "<tr><td>" + x + "</td><td>" + customer[i2].name + "</td><td>" + customer[i2].Phone + "</td><td>" + customer[i2].countTiket + "</td></tr>";
                        }
                    }

                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                }
                return Json(temp, JsonRequestBehavior.AllowGet);
            }
        }

        // Transact
        [HttpPost]
        public ActionResult Transact(TransVM model)
        {
            _log.Info("Load Transact");
            string billResponse = "";
            string invoice = "";

            using (SaptcoContext _db = new SaptcoContext())
            {
                string query2 = $"EXECUTE SP_GetCountInTrip {model.IdTrip}";
                var total = _db.Database.SqlQuery<int>(query2).FirstOrDefault();
                int countTiket2 = model.countTiket;
                total = total + countTiket2;
                if (total < 48)
                {
                    try
                    {

                        InvoiceVM invoiceBill = new InvoiceVM();
                        {
                            string query = $"EXECUTE SP_REGTRANS {model.userId},{model.serviceId},'{model.customer.phone}','{model.customer.nationalId}',N'{model.customer.name}',{model.IdTrip},{model.IdPay},{model.countTiket},{model.idTerminal},{model.idTime}";
                            invoiceBill = _db.Database.SqlQuery<InvoiceVM>(query).FirstOrDefault();

                            invoice = Traversehtml.CreditNote(new PDFVM(invoiceBill.id, invoiceBill.createdAt, invoiceBill.cost, invoiceBill.withVatTotal, invoiceBill.taxValue, invoiceBill.taxValue, invoiceBill.withoutVatTotal, invoiceBill.detail, invoiceBill.createdTime, invoiceBill.CustomerName, invoiceBill.Line, invoiceBill.StartingPoint, invoiceBill.Hotel, invoiceBill.TripTime, invoiceBill.Tel, invoiceBill.StartingPointEn, invoiceBill.HotelEn, invoiceBill.countTiket));
                        }

                        #region Create Invoice 

                        string qrDate = invoiceBill.timeStamp.ToString("yyyy-MM-dd");
                        string qrDateTime = string.Concat(qrDate, "T", invoiceBill.createdTime);
                        string sellerName = Helpers.GetTLV(1, "Saudi Public Transport Company");
                        string VATRegistrationNumber = Helpers.GetTLV(2, "300004441600003");
                        string timeStamp = Helpers.GetTLV(3, qrDateTime);
                        string invoiceTotalWithVAT = Helpers.GetTLV(4, invoiceBill.withVatTotal);
                        string VATTotal = Helpers.GetTLV(5, invoiceBill.taxValue);

                        string qrHexa = string.Concat(sellerName, VATRegistrationNumber, timeStamp, invoiceTotalWithVAT, VATTotal);
                        string TLVBase64 = Helpers.FromHexaToBase64(qrHexa);

                        Traversehtml.GenerateQRCode(TLVBase64);
                        string invoiceQueryParam2 = HttpUtility.UrlEncode(Traversehtml.Encrypt(invoiceBill.id));

                        string requestedUrl2 = $"{ConfigurationManager.AppSettings["BASE_URL"]}/Download.aspx?invoiceUsed={invoiceQueryParam2}";

                        Traversehtml.ToImage(requestedUrl2);



                        string pdf_page_size = "A4";
                        PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                        pdf_page_size, true);



                        string pdf_orientation = "Portrait";
                        PdfPageOrientation pdfOrientation =
                        (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);

                        int webPageWidth = 1024;
                        int webPageHeight = 0;



                        // instantiate a html to pdf converter object
                        HtmlToPdf converter = new HtmlToPdf();



                        // set converter options
                        converter.Options.PdfPageSize = pageSize;
                        converter.Options.PdfPageOrientation = pdfOrientation;
                        converter.Options.WebPageWidth = webPageWidth;
                        converter.Options.WebPageHeight = webPageHeight;



                        // create a new pdf document converting an url
                        PdfDocument doc = converter.ConvertHtmlString(invoice, "");


                        // save pdf document

                        byte[] bytes = doc.Save();

                        // save in database

                        {

                            using (SqlConnection _cn = new SqlConnection(ConfigurationManager.ConnectionStrings["billConstr"].ToString()))
                            {
                                using (SqlCommand _cmd = new SqlCommand("SP_INVOICE", _cn))
                                {
                                    _cmd.CommandType = CommandType.StoredProcedure;
                                    _cn.Open();
                                    _cmd.Parameters.Add("@INVID", SqlDbType.Int).Value = invoiceBill.id;
                                    _cmd.Parameters.Add("@BILL", SqlDbType.VarBinary).Value = bytes;
                                    _cmd.ExecuteNonQuery();
                                    _cn.Close();
                                }
                            }

                        }

                        #endregion

                        #region Send SMS

                        string invoiceQueryParam = HttpUtility.UrlEncode(Traversehtml.Encrypt(invoiceBill.id));

                        string requestedUrl = $"{ConfigurationManager.AppSettings["BASE_URL"]}/Download.aspx?invId={invoiceQueryParam}";

                        billResponse = requestedUrl;

                        NotificationWS _Proxy = new NotificationWS() { Url = "http://services.saptco.sa/Notifications/NotificationWS.asmx" };

                        int senderID = Convert.ToInt32(ConfigurationManager.AppSettings["SENDERID"]);


                        StringBuilder msg = new StringBuilder();
                        msg.AppendLine("عزيزي العميل");
                        msg.AppendLine("تم اصدار فاتورة للاطلاع عليها يرجى الضغط على الرابط ادناه");
                        msg.AppendLine(requestedUrl);

                        bool isSendSMS = _Proxy.SendSMS(model.customer.phone, msg.ToString(), senderID);

                        #endregion

                        #region Create Ticket
                        int countTiket = model.countTiket;
                        for (int i = 0; i < countTiket; i++)
                        {
                            string query3 = $"EXECUTE SP_Create_Ticket {invoiceBill.id}";
                            var total3 = _db.Database.SqlQuery<tickets>(query3).FirstOrDefault();
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex.Message);
                    }

                    return Json(billResponse, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }




            }
        }



        // convert payment to EF
        [HttpPost]
        public JsonResult CheckOut(DTOCheckOutPush model)
        {
            _log.Info("Load CheckOut");
            DTOCheckOutResponse result = new DTOCheckOutResponse();
            DTOMerchand merchand = new DTOMerchand();
            ServiceVM _currentHotel = new ServiceVM();
            DTOCheckAmountRes checkOut = new DTOCheckAmountRes();
            string creatFormResponse = string.Empty;
            try
            {

                #region Get Price
                using (RuhKSAEntities _db = new RuhKSAEntities())
                {
                    _currentHotel = _db.SubServices.Where(a => a.ServiceId == 1).Select(r => new ServiceVM
                    {
                        id = r.Id,
                        cost = r.Cost.Value,
                        name = r.Name,
                    }).FirstOrDefault();
                }
                #endregion

                model.UnitPrice = _currentHotel.cost;
                model.Merchand.Amount = _currentHotel.cost * model.Quantity;

                merchand = model.Merchand;

                #region Apply [SP_CHECKOUT] Stored Procedure
                {

                    using (RuhKSAEntities _db = new RuhKSAEntities())
                    {
                        MerchandTransaction merchandTransactions = new MerchandTransaction
                        {
                            STATUS = "Prepare CheckOut",
                            TYPE = merchand.PaymentMethod,
                            AMOUNT = merchand.Amount,
                            VATAMOUNT = merchand.Amount * .15,
                            TOTALAMOUNT = (merchand.Amount * .15) + merchand.Amount,
                            ARRIVALTIME = model.ArrivalTime,
                            CREATEDON = DateTime.Now,
                            CUSTOMERNAME = model.Merchand.GivenName,
                            CUSTOMERPHONE = model.Merchand.Phone,
                            PRICE = model.UnitPrice,
                            FROMPOINT = model.FromPoint,
                            QUANTITY = model.Quantity,
                            TOPOINT = model.ToPoint,
                            VATPERCENTAGE = 15
                        };

                        _db.MerchandTransactions.Add(merchandTransactions);
                        _db.SaveChanges();
                        checkOut.Id = merchandTransactions.ID;
                        checkOut.Amount = merchandTransactions.TOTALAMOUNT;


                        #region Add Tickets

                        for (int i = 0; i < merchandTransactions.QUANTITY; i++)
                        {
                            _db.HyperTickets.Add(new HyperTicket { InvoiceId = checkOut.Id, CreatedAt = DateTime.Now, is_used = 0 });
                            _db.SaveChanges();
                        }

                        #endregion

                    }
                }
                #endregion


                merchand.MerchantTransactionId = checkOut.Id;
                merchand.Amount = Math.Floor(checkOut.Amount);

                #region Create CheckOut
                {
                    var client = new RestClient(ConfigurationManager.AppSettings["checkoutAPI"]);
                    client.Timeout = -1;
                    string body = JsonConvert.SerializeObject(merchand);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", ConfigurationManager.AppSettings["HyperPayLogin"]);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    result = JsonConvert.DeserializeObject<DTOCheckOutResponse>(response.Content);

                    #region Log
                    EndPointLogger.LogCheckOut(new CheckOutLog { CheckOutId = checkOut.Id, RequestJson = JsonConvert.SerializeObject(model), ResponseJson = response.Content, Status = response.StatusCode.ToString(), Action = "Create CheckOut" });
                    #endregion
                }
                #endregion

                #region Apply [SP_CHECKOUT] Stored Procedure
                {

                    using (RuhKSAEntities _db = new RuhKSAEntities())
                    {
                        // get current MerchandTransaction
                        var currentMerchandTransaction = _db.MerchandTransactions.FirstOrDefault(a => a.ID == checkOut.Id);
                        currentMerchandTransaction.STATUS = "Payment Form";
                        _db.SaveChanges();
                    }
                }
                #endregion

                #region Create Payment Form
                {
                    var client = new RestClient(ConfigurationManager.AppSettings["PaymentFormAPI"]);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", ConfigurationManager.AppSettings["HyperPayLogin"]);
                    request.AddHeader("Content-Type", "application/json");
                    DTOSumbitPayment body = new DTOSumbitPayment { CheckoutId = result.id };
                    request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    creatFormResponse = response.Content;

                    #region Log
                    EndPointLogger.LogCheckOut(new CheckOutLog { CheckOutId = checkOut.Id, RequestJson = JsonConvert.SerializeObject(body), ResponseJson = response.Content, Status = response.StatusCode.ToString(), Action = "Create Payment Form" });
                    #endregion

                }
                #endregion

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }
            return Json(new { creatFormResponse, result.id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PaymentStatus(DTOPaymentStatus model)
        {
            DTOPaymentStatusInfo result = new DTOPaymentStatusInfo();
            try
            {
                var client = new RestClient(ConfigurationManager.AppSettings["PaymentStatusAPI"]);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", ConfigurationManager.AppSettings["HyperPayLogin"]);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                result = JsonConvert.DeserializeObject<DTOPaymentStatusInfo>(response.Content);


                using (RuhKSAEntities _db = new RuhKSAEntities())
                {
                    // get current MerchandTransaction
                    var currentMerchandTransaction = _db.MerchandTransactions.FirstOrDefault(a => a.ID == Convert.ToInt32(result.merchantTransactionId));
                    currentMerchandTransaction.STATUS = result.SAPTCOResult.code;
                    _db.SaveChanges();
                }


                #region Log
                EndPointLogger.LogCheckOut(new CheckOutLog { CheckOutId = Convert.ToInt32(result.merchantTransactionId), RequestJson = JsonConvert.SerializeObject(model), ResponseJson = response.Content.Replace("'", "\""), Status = response.StatusCode.ToString(), Action = "Get Payment Status" });
                #endregion

            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult HookNotofication(HookNotify model)
        {

            using (RuhKSAEntities _db = new RuhKSAEntities())
            {
                // get current MerchandTransaction
                var currentMerchandTransaction = _db.MerchandTransactions.FirstOrDefault(a => a.ID == Convert.ToInt32(model.MerchantTransactionId));
                currentMerchandTransaction.STATUS = "Hook Notify";
                _db.SaveChanges();
            }

            #region Log
            EndPointLogger.LogCheckOut(new CheckOutLog { CheckOutId = Convert.ToInt32(model.MerchantTransactionId), RequestJson = JsonConvert.SerializeObject(model), Status = HttpStatusCode.OK.ToString(), Action = "Hook Notifications" });
            #endregion

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // MobileService/GenerateInvoice/1
        [HttpGet]
        public JsonResult GenerateInvoice(int id)
        {
            string billResponse = "";
            string invoice = "";
            string ticket = "";
            ResponseMessageVM response = new ResponseMessageVM();
            HyperPayInvoice hyperPayInvoice = new HyperPayInvoice();
            List<HyperPayTicket> hyperPayTickets = new List<HyperPayTicket>();
            List<string> tickets = new List<string>();
            int no_tickets = 0;
            string customerPhone = "";
            using (RuhKSAEntities _db = new RuhKSAEntities())
            {
                try
                {
                    #region Ticket Step
                    {
                        hyperPayTickets = (from MT in _db.MerchandTransactions
                                                        join HT in _db.HyperTickets on MT.ID equals HT.InvoiceId
                                                        where MT.ID == id
                                                        select new HyperPayTicket
                                                        {
                                                            TicketNo = HT.Id.ToString(),
                                                            Quantity = 1,
                                                            UnitPrice = MT.PRICE.Value,
                                                            CreatedAt = MT.CREATEDON.ToString(),
                                                            CreatedTime = MT.CREATEDON.ToString(),
                                                            VatPercentage = MT.VATPERCENTAGE,
                                                            VatAmount = MT.PRICE * .15,
                                                            TotalBeforeVat = MT.PRICE.Value,
                                                            TotalIncludingVat = (MT.PRICE) + (MT.PRICE * .15),
                                                            Customer = MT.CUSTOMERNAME,
                                                            Phone = MT.CUSTOMERPHONE,
                                                            From = MT.FROMPOINT,
                                                            To = MT.TOPOINT,
                                                            ArrivalTime = MT.ARRIVALTIME
                                                        }).ToList();

                        no_tickets = hyperPayTickets.Count;
                        string trRows = DrawTicketDetails(hyperPayTickets);

                        #region Create Ticket 

                        ticket = Traversehtml.CreateHyperPayNoteCreditNoteTicket(trRows);

                        string pdf_page_size = "A4";
                        PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                        pdf_page_size, true);



                        string pdf_orientation = "Portrait";
                        PdfPageOrientation pdfOrientation =
                        (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);

                        int webPageWidth = 1024;
                        int webPageHeight = 0;



                        // instantiate a html to pdf converter object
                        HtmlToPdf converter = new HtmlToPdf();



                        // set converter options
                        converter.Options.PdfPageSize = pageSize;
                        converter.Options.PdfPageOrientation = pdfOrientation;
                        converter.Options.WebPageWidth = webPageWidth;
                        converter.Options.WebPageHeight = webPageHeight;



                        // create a new pdf document converting an url
                        PdfDocument doc = converter.ConvertHtmlString(ticket, "");


                        // save pdf document

                        byte[] bytes = doc.Save();

                        string invoiceQueryParam = HttpUtility.UrlEncode(Traversehtml.Encrypt(hyperPayTickets.FirstOrDefault().TicketNo.ToString()));
                        billResponse = $"{ConfigurationManager.AppSettings["BASE_URL"]}/DownloadTicket.aspx?invId={invoiceQueryParam}";

                        // save in database

                        var spHyperTickets = _db.HyperTickets.Where(a => a.InvoiceId == id).ToList();
                        foreach (var item in spHyperTickets)
                        {
                            item.Bill = bytes;
                            item.DownloadUrl = billResponse;
                            _db.SaveChanges();
                        }

                        response = new ResponseMessageVM { code = 1, message = "SUCCESS" };
                        #endregion

                    }
                    #endregion

                    #region Invoice Step
                    {
                        hyperPayInvoice = (from MT in _db.MerchandTransactions
                                           where MT.ID == id
                                           select new HyperPayInvoice
                                           {
                                               InvoiceId = MT.ID,
                                               Quantity = 1,
                                               UnitPrice = MT.PRICE.Value,
                                               CreatedAt = MT.CREATEDON.ToString(),
                                               CreatedTime = MT.CREATEDON.ToString(),
                                               VatPercentage = MT.VATPERCENTAGE,
                                               VatAmount = MT.PRICE * .15,
                                               TotalBeforeVat = MT.PRICE.Value,
                                               TotalIncludingVat = (MT.PRICE) + (MT.PRICE * .15)
                                           }).FirstOrDefault();

                        string trRows = DrawInvoiceDetails(hyperPayInvoice);
                        string trSecion = DrawInvoiceSection(id);


                        invoice = Traversehtml.CreateHyperPayNoteCreditNote(trRows, trSecion, hyperPayInvoice);


                        #region Create Invoice 


                        string qrDate = hyperPayInvoice.CreatedAt;
                        string qrDateTime = string.Concat(qrDate, "T", hyperPayInvoice.CreatedTime);
                        string sellerName = Helpers.GetTLV(1, "Saudi Public Transport Company");
                        string VATRegistrationNumber = Helpers.GetTLV(2, "300004441600003");
                        string timeStamp = Helpers.GetTLV(3, qrDateTime);
                        string invoiceTotalWithVAT = Helpers.GetTLV(4, hyperPayInvoice.TotalIncludingVat.ToString());
                        string VATTotal = Helpers.GetTLV(5, hyperPayInvoice.VatAmount.ToString());

                        string qrHexa = string.Concat(sellerName, VATRegistrationNumber, timeStamp, invoiceTotalWithVAT, VATTotal);
                        string TLVBase64 = Helpers.FromHexaToBase64(qrHexa);

                        Traversehtml.GenerateQRCode(TLVBase64);

                        string pdf_page_size = "A4";
                        PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                        pdf_page_size, true);



                        string pdf_orientation = "Portrait";
                        PdfPageOrientation pdfOrientation =
                        (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);

                        int webPageWidth = 1024;
                        int webPageHeight = 0;



                        // instantiate a html to pdf converter object
                        HtmlToPdf converter = new HtmlToPdf();



                        // set converter options
                        converter.Options.PdfPageSize = pageSize;
                        converter.Options.PdfPageOrientation = pdfOrientation;
                        converter.Options.WebPageWidth = webPageWidth;
                        converter.Options.WebPageHeight = webPageHeight;



                        // create a new pdf document converting an url
                        PdfDocument doc = converter.ConvertHtmlString(invoice, "");


                        // save pdf document

                        byte[] bytes = doc.Save();

                        #endregion

                        // save in database


                        string invoiceQueryParam = HttpUtility.UrlEncode(Traversehtml.Encrypt(id.ToString()));
                        billResponse = $"{ConfigurationManager.AppSettings["BASE_URL"]}/PaymentDownload.aspx?invId={invoiceQueryParam}";

                        _db.TicketPaymentPDFs.Add(new TicketPaymentPDF
                        {
                            InvoiceId = id,
                            Bill = bytes,
                            CreatedAt = DateTime.Now,
                            is_used = 0,
                            DownloadUrl = billResponse
                        });
                        _db.SaveChanges();

                        response = new ResponseMessageVM { code = 1, message = "SUCCESS" };

                        #region Send SMS
                        {
                            NotificationWS _Proxy = new NotificationWS() { Url = "http://services.saptco.sa/Notifications/NotificationWS.asmx" };
                            int senderID = Convert.ToInt32(ConfigurationManager.AppSettings["SENDERID"]);


                            StringBuilder msg = new StringBuilder();
                            msg.AppendLine("عزيزي العميل");
                            msg.AppendLine("تم اصدار فاتورة للاطلاع عليها يرجى الضغط على الرابط ادناه");
                            msg.AppendLine(billResponse);

                            bool isSendSMS = _Proxy.SendSMS(customerPhone, msg.ToString(), senderID);
                        }
                        #endregion
                    }
                    #endregion


                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                }
                return Json(new { response, url = billResponse }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GenerateTicketTable(int id)
        {
            DTODownloadDocs docs = new DTODownloadDocs();
            using (RuhKSAEntities _db = new RuhKSAEntities())
            {
                // tickets
                docs.TicketUrl = _db.HyperTickets.Where(a => a.InvoiceId == id).Select(r => new DTOTickeUrls
                {
                    Id = r.Id,
                    Url = r.DownloadUrl
                }).FirstOrDefault();

                // invocie
                docs.InvoiceUrl = _db.TicketPaymentPDFs.FirstOrDefault(a => a.InvoiceId == id).DownloadUrl;
            }

            return Json(docs, JsonRequestBehavior.AllowGet);
        }


        private string DrawInvoiceSection(int invoiceNumber)
        {
            return $"<tr>" +
                $"<td style='width: 33%; text-align: right; font-size: 1.1em; font-weight: bold;'>رقم الفاتورة</td>" +
                $"<td style='width: 33%; text-align: center;'>{invoiceNumber}</td>" +
                $"<td style='width: 33%; text-align: left; font-weight: bold;'>Invoice No</td>" +
                $"</tr>";
        }

        private string DrawInvoiceDetails(HyperPayInvoice item)
        {
            string result = "<tbody>";
            result += $"<tr>" +
                    $"<td style='border: 1px solid #cccccc; padding: 8px;'>{item.Description}</td>" +
                    $"<td style='border: 1px solid #cccccc;'>وحدة</td>" +
                    $"<td style='border: 1px solid #cccccc;direction:ltr;'>{item.Quantity}</td>" +
                    $"<td style='border: 1px solid #cccccc;direction:ltr;'>{item.UnitPrice}</td>" +
                    $"<td style='border: 1px solid #cccccc;direction:ltr;'>{item.TotalBeforeVat}</td>" +
                    $"<td style='border: 1px solid #cccccc;'>{item.VatPercentage}</td>" +
                    $"<td style='border: 1px solid #cccccc;direction:ltr;'>{item.VatAmount}</td>" +
                    $"<td style='border: 1px solid #cccccc;direction:ltr;'>{item.TotalIncludingVat}</td>" +
                    $"</tr>";

            result += "</tbody>" +
                "<tfoot style='background: #a57c35; color: #ffffff;'>";

            result += $"<tr>" +
                      $"<td style='border: 1px solid #cccccc; padding: 8px;'>الإجمالي</td>" +
                      $"<td style='border: 1px solid #cccccc;'></td>" +
                      $"<td style='border: 1px solid #cccccc;'></td>" +
                      $"<td style='border: 1px solid #cccccc;'></td>" +
                      $"<td style='border: 1px solid #cccccc;direction:ltr;'>{item.TotalBeforeVat}</td>" +
                      $"<td style='border: 1px solid #cccccc;'></td>" +
                      $"<td style='border: 1px solid #cccccc;direction:ltr;'>{item.VatAmount}</td>" +
                      $"<td style='border: 1px solid #cccccc;direction:ltr;'>{item.TotalIncludingVat}</td>" +
                      $"</tr>" +
                      $"</tfoot>";

            return result;
        }


        private string DrawTicketDetails(List<HyperPayTicket> items)
        {

            string backgroundUrl = $"{ConfigurationManager.AppSettings["BASE_URL"]}/assets/ticket-footer-bg.png";
            string saptcoLogo = Server.MapPath("~/assets/logo.png");

            string result = "";
            foreach (var item in items)
            {


                string TicketId = HttpUtility.UrlEncode(Traversehtml.Encrypt(item.TicketNo));
                string ticketImageUrl = Server.MapPath($"~/Tickets/Ticekt_{item.TicketNo}.png");
                string ticketScanUrl = $"{ConfigurationManager.AppSettings["BASE_URL"]}/ScanTicket.aspx?TicketId={TicketId}";
                Traversehtml.GenerateQRCodeTicket(ticketScanUrl, ticketImageUrl);

                result += $"<div style='width: 680px;height: 300px;font-family: sans-serif;background: #f8f5ee url(\"{backgroundUrl}\") bottom center no-repeat; " +
                "border-radius: 1em;border-top: 0.25em solid #a57c35;border-bottom: 0.25em solid #766e64;box-shadow: 0 0 5px rgba(0, 0, 0, 0.2); margin: 2.4em auto;'>";
                result += "<div style='width: 70px; height: 80px; background: #ffffff; border-radius: 50%; float: left; margin-top: 110px; margin-left: -35px; box-shadow: inset -1px 0 #dfdfdf;'></div>";
                result += "<div style='width: 70px; height: 80px; background: #ffffff; border-radius: 50%; float: right; margin-top: 110px; margin-right: -35px; box-shadow: -1px 0 #dfdfdf;'></div>";


                result += "<div style='padding: 0 3.5em;'>";
                result += "<h1 style='margin-bottom: 0.2em; font-size: 2.2em; display: inline-block;'>SAPTCO bus lines</h1>";
                result += $"<img src='{saptcoLogo}' style='float: right; margin-top: 1em;' width='210px;' />";
                result += "</div>";

                result += "<div style='padding: 1em 3.5em; height: 100px;'>";


                result += "<div style='width: 35%; float: left;'>";
                result += "<h5 style='color: #a57c35; margin-top: 0.75em; margin-bottom: 0; padding-bottom: 0;'>Name</h5>";
                result += $"<h5 style='margin-top: 0.25em; margin-bottom: 0.25em;'>{item.Customer}</h5>";
                result += "</div>";

                result += "<div style='width: 35%; float: left; text-align: center;'>";
                result += "<h5 style='color: #a57c35; margin-top: 0.75em; margin-bottom: 0; padding-bottom: 0;'>Ticket Type</h5>";
                result += "<h5 style='padding-top: 0; margin-top: 0.25em; margin-bottom: 0.25em;'>ONEWAY</h5>";
                result += "</div>";


                result += "<div style='width: 30%; float: left;'>";
                result += "<div style='display: 50%; float: left;'>";
                result += "<h5 style='color: #a57c35; margin-top: 0.75em; margin-bottom: 0; padding-bottom: 0;'>Issued by</h5>";
                result += "<h5 style='margin-top: 0.25em; margin-bottom: 0.25em;'>SAPTCO</h5>";
                result += "</div></div>";



                result += "<div style='width: 70%; float: left;'>";
                result += "<div style='display: 33%; float: left;'>";
                result += "<h5 style='color: #a57c35; margin-top: 0.75em; margin-bottom: 0; padding-bottom: 0;'>From</h5>";
                result += $"<h5 style='margin-top: 0.25em; margin-bottom: 0.75em;'>{item.From}</h5>";
                result += "</div>";


                result += "<div style='display: 33%; float: left; margin-left: 2em;'>";
                result += "<h5 style='color: #a57c35; margin-top: 0.75em; margin-bottom: 0; padding-bottom: 0;'>Date</h5>";
                result += $"<h5 style='margin-top: 0.25em; margin-bottom: 0.75em;'>{item.CreatedAt}</h5>";
                result += "</div>";


                result += "<div style='display: 33%; float: left; margin-left: 2em;'>";
                result += "<h5 style='color: #a57c35; margin-top: 0.75em; margin-bottom: 0; padding-bottom: 0;'>Time</h5>";
                result += $"<h5 style='margin-top: 0.25em; margin-bottom: 0.75em;'>{item.ArrivalTime}</h5>";
                result += "</div></div>";


                result += "<div style='width: 30%; float: right;'>";
                result += "<div style='display: 50%; float: left;'>";
                result += "<h5 style='color: #a57c35; margin-top: 0.75em; margin-bottom: 0; padding-bottom: 0;'>Ticket number</h5>";
                result += $"<h4 style='margin-top: 0.5em; margin-bottom: 0.5em;'>{Traversehtml.MakeIntoSequence(Convert.ToInt32(item.TicketNo), 9, "SA")}</h4>";
                result += $"<img style='display: block; margin-top: 0.25em;' src='{ticketImageUrl}' width='90px;'  />";
                result += " </div> </div>";

                result += "<div style='width: 70%; float: left;'>";
                result += "<h5 style='color: #a57c35; margin-top: 0em; margin-bottom: 0; padding-bottom: 0;'>To</h5>";
                result += $"<h5 style='margin-top: 0.25em;'>{item.To}</h5>";
                result += "</div>";

                result += "</div>";
                result += "</div>";

            }



            return result;
        }

    }
}