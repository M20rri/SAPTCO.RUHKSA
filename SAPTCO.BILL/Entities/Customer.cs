using System.Collections.Generic;
namespace SAPTCO.BILL.Entities
{
    public class Customer
    {
        public Customer()
        {
            this.Transactions = new HashSet<Transaction>();
        }
    
        public int Id { get; set; }
        public string Phone { get; set; }
        public string NationalId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
