using SAPTCO.BILL.Entities;
using SAPTCO.BILL.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SAPTCO.BILL
{
    public partial class ScanTicket : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["TicketId"] != null)
                    {
                        int ticketId = Convert.ToInt32(Traversehtml.Decrypt(HttpUtility.UrlDecode(Request.QueryString["TicketId"])));
                        string message = "";
                        if (ticketId > 0)
                        {
                            using (RuhKSAEntities _db = new RuhKSAEntities())
                            {
                                var hyperTicket = _db.HyperTickets.FirstOrDefault(a => a.Id == ticketId);
                                if (hyperTicket.is_used == 0)
                                {
                                    hyperTicket.is_used = 1;
                                    _db.SaveChanges();
                                    message = "تم استخدام التذكره بنجاح";
                                }
                                else
                                {
                                    message = "لا يمكن استخدام التذكره تم استخدمها  من قبل";
                                }
                                Response.Write($"<h1>{message}</h1>");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Response.Write("<h1>Invalid Invoice</h1>");
                }
            }
        }
    }
}