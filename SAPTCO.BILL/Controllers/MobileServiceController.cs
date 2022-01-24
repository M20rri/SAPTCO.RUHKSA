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
using System.Net.Http;
using System.Net.Http.Headers;
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
                using (SaptcoContext _db = new SaptcoContext())
                {
                    string query = $"EXECUTE SP_Hotel 1";
                    var _hotels = _db.Database.SqlQuery<ServiceVM>(query).ToList();
                    _currentHotel = _hotels.FirstOrDefault(a => a.id == model.ServiceId);
                }
                #endregion

                model.UnitPrice = _currentHotel.cost;
                model.Merchand.Amount = _currentHotel.cost * model.Quantity;

                merchand = model.Merchand;

                #region Apply [SP_CHECKOUT] Stored Procedure
                {

                    DTOCheckOutSP _checkount = new DTOCheckOutSP
                    {
                        Amount = merchand.Amount,
                        Status = "Prepare CheckOut",
                        Type = merchand.PaymentMethod
                    };

                    using (SaptcoContext _db = new SaptcoContext())
                    {
                        string query = $"EXECUTE SP_CHECKOUT '{_checkount.Status}',{_checkount.Amount},'{_checkount.Type}',{model.Quantity},{model.UnitPrice},'{model.Merchand.GivenName}','{model.Merchand.Phone}'";
                        checkOut = _db.Database.SqlQuery<DTOCheckAmountRes>(query).FirstOrDefault();
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

                    DTOCheckOutSP _checkount = new DTOCheckOutSP
                    {
                        Amount = merchand.Amount,
                        Status = "Payment Form",
                        Type = merchand.PaymentMethod
                    };

                    using (SaptcoContext _db = new SaptcoContext())
                    {
                        string query = $"EXECUTE SP_EDITCHECKOUT {checkOut.Id},'{_checkount.Status}'";
                        checkOut.Id = _db.Database.SqlQuery<int>(query).FirstOrDefault();
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

                DTOCheckOutSP _checkount = new DTOCheckOutSP
                {
                    Amount = Convert.ToDouble(result.amount),
                    Status = result.SAPTCOResult.code,
                    Type = model.PaymentMethod
                };

                using (SaptcoContext _db = new SaptcoContext())
                {
                    string query = $"EXECUTE SP_EDITCHECKOUT {result.merchantTransactionId},'{_checkount.Status}'";
                    int checkOutId = _db.Database.SqlQuery<int>(query).FirstOrDefault();
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

            DTOCheckOutSP _checkount = new DTOCheckOutSP
            {
                Amount = Convert.ToDouble(model.Amount),
                Status = "Hook Notify",
                Type = model.PaymentMethod
            };

            using (SaptcoContext _db = new SaptcoContext())
            {
                string query = $"EXECUTE SP_EDITCHECKOUT {model.MerchantTransactionId},'{_checkount.Status}'";
                int checkOutId = _db.Database.SqlQuery<int>(query).FirstOrDefault();

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
            List<HyperPayInvoice> hyperPayTickets = new List<HyperPayInvoice>();
            List<string> tickets = new List<string>();
            int no_tickets = 0;
            using (SaptcoContext _db = new SaptcoContext())
            {
                try
                {
                    #region Ticket Step
                    {
                        string query = $"EXECUTE SP_PRINTHYPERPAYTICKET {id}";
                        hyperPayTickets = _db.Database.SqlQuery<HyperPayInvoice>(query).ToList();
                        no_tickets = hyperPayTickets.Count;
                        string trRows = DrawTicketDetails(hyperPayTickets.FirstOrDefault());

                        #region Create Ticket 

                        foreach (var hyperPayTicket in hyperPayTickets)
                        {

                            ticket = Traversehtml.CreateHyperPayNoteCreditNoteTicket(trRows, hyperPayTicket);

                            string qrDate = hyperPayTicket.CreatedAt;
                            string qrDateTime = string.Concat(qrDate, "T", hyperPayTicket.CreatedTime);
                            string sellerName = Helpers.GetTLV(1, "Saudi Public Transport Company");
                            string VATRegistrationNumber = Helpers.GetTLV(2, "300004441600003");
                            string timeStamp = Helpers.GetTLV(3, qrDateTime);
                            string invoiceTotalWithVAT = Helpers.GetTLV(4, hyperPayTicket.TotalIncludingVat.ToString());
                            string VATTotal = Helpers.GetTLV(5, hyperPayTicket.VatAmount.ToString());

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
                            PdfDocument doc = converter.ConvertHtmlString(ticket, "");


                            // save pdf document

                            byte[] bytes = doc.Save();



                            string invoiceQueryParam = HttpUtility.UrlEncode(Traversehtml.Encrypt(hyperPayTicket.InvoiceId.ToString()));
                            billResponse = $"{ConfigurationManager.AppSettings["BASE_URL"]}/DownloadTicket.aspx?invId={invoiceQueryParam}";


                            // save in database

                            using (SqlConnection _cn = new SqlConnection(ConfigurationManager.ConnectionStrings["billConstr"].ToString()))
                            {
                                using (SqlCommand _cmd = new SqlCommand("SP_PAYMMENT_TICKET", _cn))
                                {
                                    _cmd.CommandType = CommandType.StoredProcedure;
                                    _cn.Open();
                                    _cmd.Parameters.Add("@INVID", SqlDbType.Int).Value = id;
                                    _cmd.Parameters.Add("@DOWNLOADURL", SqlDbType.NVarChar).Value = billResponse;
                                    _cmd.Parameters.Add("@TICKETID", SqlDbType.Int).Value = hyperPayTicket.InvoiceId;
                                    _cmd.Parameters.Add("@BILL", SqlDbType.VarBinary).Value = bytes;
                                    var rdr = _cmd.ExecuteReader();
                                    while (rdr.Read())
                                    {
                                        response.code = Convert.ToInt32(rdr[0]);
                                        response.message = rdr[1].ToString();
                                    }
                                    _cn.Close();
                                }
                            }
                        }


                        #endregion

                    }
                    #endregion

                    #region Invoice Step
                    {
                        string query = $"EXECUTE SP_PRINTHYPERPAYINVOICE {id}";
                        hyperPayInvoice = _db.Database.SqlQuery<HyperPayInvoice>(query).FirstOrDefault();
                        string customerPhone = "";
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


                        using (SqlConnection _cn = new SqlConnection(ConfigurationManager.ConnectionStrings["billConstr"].ToString()))
                        {
                            using (SqlCommand _cmd = new SqlCommand("SP_PAYMMENT_INVOICE", _cn))
                            {
                                _cmd.CommandType = CommandType.StoredProcedure;
                                _cn.Open();
                                _cmd.Parameters.Add("@INVID", SqlDbType.Int).Value = id;
                                _cmd.Parameters.Add("@DOWNLOADURL", SqlDbType.NVarChar).Value = billResponse;
                                _cmd.Parameters.Add("@BILL", SqlDbType.VarBinary).Value = bytes;
                                var rdr = _cmd.ExecuteReader();
                                while (rdr.Read())
                                {
                                    response.code = Convert.ToInt32(rdr[0]);
                                    response.message = rdr[1].ToString();
                                    customerPhone = rdr[2].ToString();
                                }
                                _cn.Close();
                            }
                        }


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
            List<DTOTickeUrls> ticketsUrls = new List<DTOTickeUrls>();

            SqlConnection _cn = new SqlConnection(ConfigurationManager.ConnectionStrings["billConstr"].ToString());
            string query = $"EXECUTE SP_DOWNLOADTICKET {id}";

            DTODownloadDocs docs = new DTODownloadDocs();

            SqlCommand cmd = new SqlCommand(query, _cn);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            _cn.Close();


            docs = ds.Tables[1].AsEnumerable().Select(QdataRow => new DTODownloadDocs
            {
                InvoiceUrl = QdataRow.Field<string>("DownloadUrl"),
                TicketUrl = ds.Tables[0].AsEnumerable().Select(AdataRow => new DTOTickeUrls
                {
                    Id = AdataRow.Field<int>("Id"),
                    Url = AdataRow.Field<string>("Url")
                }).ToList()
            }).FirstOrDefault();

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

        private string DrawTicketDetails(HyperPayInvoice item)
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

    }
}