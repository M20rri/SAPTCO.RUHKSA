using System.Data.Entity;
namespace SAPTCO.BILL.Entities
{
    public class RuhKSAEntities : DbContext
    {
        public RuhKSAEntities()
            : base("name=RuhKSAEntities")
        {
        }
        
        public virtual DbSet<CheckoutLog> CheckoutLogs { get; set; }
        public virtual DbSet<HyperTicket> HyperTickets { get; set; }
        public virtual DbSet<MerchandTransaction> MerchandTransactions { get; set; }
        public virtual DbSet<Paymint> Paymints { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<SubService> SubServices { get; set; }
        public virtual DbSet<Terminal> Terminals { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketPaymentPDF> TicketPaymentPDFs { get; set; }
        public virtual DbSet<TimeTerminal> TimeTerminals { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Trip> Trips { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
