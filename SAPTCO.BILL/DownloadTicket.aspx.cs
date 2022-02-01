using SAPTCO.BILL.Entities;
using SAPTCO.BILL.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SAPTCO.BILL
{
    public partial class DownloadTicket : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["invId"] != null)
                    {
                        int invId = Convert.ToInt32(Traversehtml.Decrypt(HttpUtility.UrlDecode(Request.QueryString["invId"])));

                        if (invId > 0)
                        {
                            byte[] bytes;

                            using (RuhKSAEntities _db = new RuhKSAEntities())
                            {
                                bytes = _db.HyperTickets.FirstOrDefault(a => a.Id == invId).Bill;
                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.ContentType = "application/pdf";
                                Response.AppendHeader("Content-Disposition", $"attachment; filename={invId}.pdf");
                                Response.BinaryWrite(bytes);
                                Response.Flush();
                                Response.End();
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