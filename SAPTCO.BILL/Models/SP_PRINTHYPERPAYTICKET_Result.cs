namespace SAPTCO.BILL.Models
{
    public class SP_PRINTHYPERPAYTICKET_Result
    {
        public string TicketNo { get; set; }
        public int Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public string CreatedAt { get; set; }
        public string CreatedTime { get; set; }
        public int VatPercentage { get; set; }
        public double? VatAmount { get; set; }
        public double? TotalBeforeVat { get; set; }
        public double? TotalIncludingVat { get; set; }
        public string Customer { get; set; }
        public string Phone { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string ArrivalTime { get; set; }
    }

    public class SP_PRINTHYPERPAYINVOICE_Result
    {
        public int InvoiceId { get; set; }
        public int? Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public string CreatedAt { get; set; }
        public string CreatedTime { get; set; }
        public int VatPercentage { get; set; }
        public double? VatAmount { get; set; }
        public double? TotalBeforeVat { get; set; }
        public double? TotalIncludingVat { get; set; }
    }
}