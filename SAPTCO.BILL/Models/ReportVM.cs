using System;
using System.Collections.Generic;

namespace SAPTCO.BILL.Models
{
    public class ReportVM
    {
        public int? IdTrip { get; set; }
        public string NameTrip { get; set; }
        public List<listSupject> listSupjects { get; set; }
    }

    public class listSupject
    { 
        public int? Id { get; set; }
        public string Name { get; set; }
        public List<listCustomer> listCustomers { get; set; }
    }

    public class listCustomer
    { 
        public int? Id { get; set; }
        public string Phone { get; set; }
        public string name { get; set; }
        public int countTiket { get; set; }
    }
    public class trip
    {
        public int idtrip { get; set; }
        public string date { get; set; } = DateTime.Now.ToString();
        public int terminalId { get; set; }
    }

    public class Hotel
    { 
      public int serviceId { get; set; }
    }
}