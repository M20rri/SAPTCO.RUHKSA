using System;
using System.Collections.Generic;

namespace SAPTCO.BILL.Entities
{
    public class Transaction
    {
        public Transaction()
        {
            this.Bills = new HashSet<Bill>();
        }
    
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? SubServiceId { get; set; }
        public int? CustomerId { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public int? IdTrip { get; set; }
        public int? IdPay { get; set; }
        public int? countTiket { get; set; }
        public int? idTerminal { get; set; }
        public int? idTime { get; set; }
        public int? idTimeOut { get; set; }
        public int? idtripOut { get; set; }
    
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual SubService SubService { get; set; }
        public virtual User User { get; set; }
    }
}
