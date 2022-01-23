using SAPTCO.BILL.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SAPTCO.BILL
{
    public partial class PaymentDownload : Page
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
                                string qry = $"EXECUTE SP_PRINTPAYMENTINVOICE {invId}";
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
                    else if (Request.QueryString["invoiceUsed"] != null)
                    {
                        int invId = Convert.ToInt32(Traversehtml.Decrypt(HttpUtility.UrlDecode(Request.QueryString["invoiceUsed"])));

                        if (invId > 0)
                        {
                            //byte[] bytes;

                            using (SaptcoContext _db = new SaptcoContext())
                            {
                                string qry3 = $"EXECUTE SP_BillIsUsed {invId}";
                                int isUsed = _db.Database.SqlQuery<int>(qry3).FirstOrDefault();
                                if (isUsed == 0)
                                {
                                    string qry2 = $"EXECUTE SP_INVOICEUsed {invId}";
                                    var x = _db.Database.SqlQuery<string>(qry2).FirstOrDefault();
                                    Response.Write("<h1>تم استخدام التذكره بنجاح </h1>");
                                }
                                else
                                {
                                    Response.Write("<h1>لا يمكن استخدام التذكره تم استخدمها  من قبل </h1>");
                                }
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