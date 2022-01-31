using System.Collections.Generic;

namespace SAPTCO.BILL.Entities
{
    public class SubService
    {
        public SubService()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Cost { get; set; }
        public int? ServiceId { get; set; }
        public string NameEn { get; set; }

        public virtual Service Service { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
