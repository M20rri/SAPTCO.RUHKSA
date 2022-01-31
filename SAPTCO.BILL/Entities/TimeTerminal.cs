namespace SAPTCO.BILL.Entities
{
    public class TimeTerminal
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? TerminalID { get; set; }
        public int? TripID { get; set; }
    }
}
