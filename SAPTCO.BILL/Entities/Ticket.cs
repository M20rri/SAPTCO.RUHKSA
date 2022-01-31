using System;
namespace SAPTCO.BILL.Entities
{
    public class Ticket
    {
        public int TicketID { get; set; }
        public int? BillID { get; set; }
        public DateTime? CDate { get; set; }
    }
}
