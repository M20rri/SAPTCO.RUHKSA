using System.ComponentModel.DataAnnotations;

namespace SAPTCO.BILL.Entities
{
    public class Trip
    {
        [Key]
        public int IdTrip { get; set; }
        public string NameTrip { get; set; }
    }
}
