using SAPTCO.BILL.Models;
using System;
using System.Linq;

namespace SAPTCO.BILL.Helper
{
    public class EndPointLogger
    {
        public static string LogCheckOut(CheckOutLog model)
        {
            string message = "";

            using (SaptcoContext _db = new SaptcoContext())
            {
                string query = $"EXECUTE SP_CHECKOUTLOG {model.CheckOutId},'{model.Status}','{model.RequestJson}','{model.ResponseJson}','{model.Action}'";
                message = _db.Database.SqlQuery<string>(query).FirstOrDefault();
            }

            return message;
        }

        public static int CheckOutTransaction(DTOCheckOutSP model)
        {
            int result = 0;

            using (SaptcoContext _db = new SaptcoContext())
            {
                string query = $"EXECUTE SP_CHECKOUT '{model.Status}',{model.Amount},'{model.Type}'";
                var identity = _db.Database.SqlQuery<decimal>(query).FirstOrDefault();
                result = Convert.ToInt32(identity);
            }

            return result;

        }
    }
}