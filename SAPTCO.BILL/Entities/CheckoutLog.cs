using System;
namespace SAPTCO.BILL.Entities
{
    public class CheckoutLog
    {
        public int ID { get; set; }
        public int CHECKOUTID { get; set; }
        public string STATUS { get; set; }
        public string ACTION { get; set; }
        public string REQUESTJSON { get; set; }
        public string RESPONSEJSON { get; set; }
        public DateTime CREATEDON { get; set; }
    }
}
