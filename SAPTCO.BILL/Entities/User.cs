using System.Collections.Generic;
namespace SAPTCO.BILL.Entities
{
    public class User
    {
        public User()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
