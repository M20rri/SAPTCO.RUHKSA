using System.Data.Entity;

namespace SAPTCO.BILL.Models
{
    public class SaptcoContext : DbContext
    {
        public SaptcoContext() : base("name=billConstr")
        {

        }
    }
}