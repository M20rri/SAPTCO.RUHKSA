using System.ComponentModel.DataAnnotations;

namespace SAPTCO.BILL.Entities
{
    public class Paymint
    {
        [Key]
        public int IdPay { get; set; }
        public string NamePay { get; set; }
    }
}
