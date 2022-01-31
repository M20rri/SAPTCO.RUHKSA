using System;
namespace SAPTCO.BILL.Entities
{
    public class MerchandTransaction
    {
        public int ID { get; set; }
        public string STATUS { get; set; }
        public int? QUANTITY { get; set; }
        public double? PRICE { get; set; }
        public double? AMOUNT { get; set; }
        public int VATPERCENTAGE { get; set; }
        public double VATAMOUNT { get; set; }
        public double TOTALAMOUNT { get; set; }
        public string TYPE { get; set; }
        public DateTime CREATEDON { get; set; }
        public string CUSTOMERNAME { get; set; }
        public string CUSTOMERPHONE { get; set; }
        public string FROMPOINT { get; set; }
        public string TOPOINT { get; set; }
        public string ARRIVALTIME { get; set; }
    }
}
