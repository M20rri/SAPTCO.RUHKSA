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

                            using (SaptcoContext _db = new SaptcoContext())
                            {
                                string qry = $"EXECUTE SP_PRINTTICKET {invId}";
                                bytes = _db.Database.SqlQuery<byte[]>(qry).FirstOrDefault();
                            }

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
                catch (Exception ex)
                {
                    Response.Write("<h1>Invalid Invoice</h1>");
                }
            }
        }
    }
}