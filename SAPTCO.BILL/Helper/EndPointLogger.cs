using SAPTCO.BILL.Entities;
using SAPTCO.BILL.Models;
using System;

namespace SAPTCO.BILL.Helper
{
    public class EndPointLogger
    {
        public static string LogCheckOut(CheckOutLog model)
        {
            string message = "";

            using (RuhKSAEntities _db = new RuhKSAEntities())
            {
                _db.CheckoutLogs.Add(new CheckoutLog
                {
                    CHECKOUTID = model.CheckOutId,
                    STATUS = model.Status,
                    REQUESTJSON = model.RequestJson,
                    RESPONSEJSON= model.ResponseJson,
                    ACTION = model.Action,
                    CREATEDON = DateTime.Now
                });
                _db.SaveChanges();
                message = "Success";
            }

            return message;
        }
    }
}