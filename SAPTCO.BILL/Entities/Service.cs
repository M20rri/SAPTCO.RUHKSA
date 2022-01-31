using System.Collections.Generic;
namespace SAPTCO.BILL.Entities
{
    public class Service
    {
        public Service()
        {
            this.SubServices = new HashSet<SubService>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SubService> SubServices { get; set; }
    }
}
