using System;
namespace SAPTCO.BILL.Entities
{
    public class TicketPaymentPDF
    {
        public int Id { get; set; }
        public int? InvoiceId { get; set; }
        public byte[] Bill { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? is_used { get; set; }
        public string DownloadUrl { get; set; }
    }
}
