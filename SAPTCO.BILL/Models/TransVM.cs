namespace SAPTCO.BILL.Models
{
    public class TransVM
    {
        public int userId { get; set; }
        public int serviceId { get; set; }
        
        public int SubServiceId { get; set; }
        public int IdTrip { get; set; }

        public int IdPay { get; set; }
        public CustomerVM customer { get; set; }

        public int countTiket { get; set; }
        public int idTerminal { get; set; }
        public int idTime { get; set; }



    }
}