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

                        if (ticketId > 0)
                        {
                            using (SaptcoContext _db = new SaptcoContext())
                            {
                                string ticketQueryUsed = $"EXECUTE SP_TICKETUSED {ticketId}";
                                string message = _db.Database.SqlQuery<string>(ticketQueryUsed).FirstOrDefault();
                                Response.Write($"<h1>{message}</h1>");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<h1>Invalid Invoice</h1>");
                }
            }
        }
    }
}