namespace SAPTCO.BILL.Entities
{
    using System;    
    public class Bill
    {
        public int Id { get; set; }
        public int? InvoiceId { get; set; }
        public byte[] Bill1 { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Is_used { get; set; }
    
        public virtual Transaction Transaction { get; set; }
    }
}
