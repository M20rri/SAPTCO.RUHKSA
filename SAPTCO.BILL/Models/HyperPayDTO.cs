using System.Collections.Generic;
using System.Configuration;

namespace SAPTCO.BILL.Models
{
    public class DTOCheckOutSP
    {
        public string Status { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
    }

    public class DTOCheckAmountRes
    {
        public int Id { get; set; } = 0;
        public double Amount { get; set; }
    }

    public class DTOCheckOutPush
    {
        public DTOMerchand Merchand { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int ServiceId { get; set; }
        public string FromPoint { get; set; }
        public string ToPoint { get; set; }
        public string ArrivalTime { get; set; }
    }

    public class DTOMerchand
    {
        public string PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; } = "SAR";
        public string PaymentType { get; set; } = "DB";
        public int MerchantTransactionId { get; set; }
        public string Email { get; set; } = "awadmg@saptco.com.sa";
        public string Street { get; set; } = "Anas Ibn Malek";
        public string City { get; set; } = "Riyadh";
        public string State { get; set; } = "Aqiq";
        public string Country { get; set; } = "SA";
        public string PostCode { get; set; } = "12211";
        public string GivenName { get; set; } = "Mahmoud";
        public string SureName { get; set; } = "Torri";
        public string Phone { get; set; } = "";
        public string Lang { get; set; } = "ar";
    }

    public class DTOCheckOutResponse
    {
        public Result result { get; set; }
        public string buildNumber { get; set; }
        public string timestamp { get; set; }
        public string ndc { get; set; }
        public string id { get; set; }
    }

    public class Result
    {
        public string code { get; set; }
        public string description { get; set; }
    }

    public class DTOSumbitPayment
    {
        public string CheckoutId { get; set; }
        public string ReturnURL { get; set; } = ConfigurationManager.AppSettings["RedirectUrl"];
    }

    public class DTOPaymentStatus
    {
        public string PaymentMethod { get; set; }
        public string CheckoutId { get; set; }
    }

    public class DTOPaymentStatusInfo
    {
        public string id { get; set; }
        public string paymentType { get; set; }
        public string paymentBrand { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string descriptor { get; set; }
        public string merchantTransactionId { get; set; }
        public Result result { get; set; }
        public Result SAPTCOResult { get; set; }
        public ResultDetails resultDetails { get; set; }
        public ResponseCard card { get; set; }
        public ResponseCustomer customer { get; set; }
        public ThreeDSecure threeDSecure { get; set; }
        public ResponseCustomParameters customParameters { get; set; }
        public ResponseRisk risk { get; set; }
        public string buildNumber { get; set; }
        public string timestamp { get; set; }
        public string ndc { get; set; }
    }

    public class ResultDetails
    {
        public string RiskStatusCode { get; set; }
        public string ResponseCode { get; set; }
        public string RequestId { get; set; }
        public string RiskResponseCode { get; set; }
        public string action { get; set; }
        public string OrderId { get; set; }
    }

    public class ResponseCard
    {
        public string bin { get; set; }
        public string binCountry { get; set; }
        public string last4Digits { get; set; }
        public string holder { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
    }

    public class ResponseCustomer
    {
        public string givenName { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string ip { get; set; }
        public string ipCountry { get; set; }
    }

    public class ThreeDSecure
    {
        public string eci { get; set; }
        public string xid { get; set; }
        public string paRes { get; set; }
    }

    public class ResponseCustomParameters
    {
        public string SHOPPER_EndToEndIdentity { get; set; }
        public string CTPE_DESCRIPTOR_TEMPLATE { get; set; }
    }

    public class ResponseRisk
    {
        public string score { get; set; }
    }

    public class SAPTCOCustomParameters
    {
        public string CustomParameter1 { get; set; }
        public string CustomParameter2 { get; set; }
        public string TicketsURL { get; set; }
    }

    public class HookNotify
    {
        public string CheckoutId { get; set; }
        public int MerchantTransactionId { get; set; }
        public string PaymentId { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentProcessingDate { get; set; }
        public SAPTCOCustomParameters SAPTCOCustomParameters { get; set; }
    }

    public class CheckOutLog
    {
        public int CheckOutId { get; set; }
        public string Status { get; set; }
        public string RequestJson { get; set; }
        public string ResponseJson { get; set; }
        public string Action { get; set; }
    }

    public class HyperPayInvoice
    {
        public string TitleAr { get; set; } = "فاتورة ضريبية مبسطة";
        public string TitleEn { get; set; } = "Simplified Tax Invoice";
        public int InvoiceId { get; set; }
        public string Description { get; set; } = "تذكرة روح السعودية";
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string CreatedAt { get; set; }
        public string CreatedTime { get; set; }
        public double TotalBeforeVat { get; set; }
        public int VatPercentage { get; set; }
        public double VatAmount { get; set; }
        public double TotalIncludingVat { get; set; }
    }

    public class HyperPayTicket
    {
        public string Customer { get; set; }
        public string Phone { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string ArrivalTime { get; set; }
        public string TicketNo { get; set; }

        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public string CreatedAt { get; set; }
        public string CreatedTime { get; set; }
        public double TotalBeforeVat { get; set; }
        public int VatPercentage { get; set; }
        public double VatAmount { get; set; }
        public double TotalIncludingVat { get; set; }
    }

    public class DTOTickeUrls
    {
        public int Id { get; set; }
        public string Url { get; set; }
    }

    public class DTODownloadDocs
    {
        public List<DTOTickeUrls> TicketUrl { get; set; }
        public string InvoiceUrl { get; set; }
    }
}