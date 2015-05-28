using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class LoginTemplate : System.Web.UI.MasterPage
{
    public string whichSite = "aruba";
    public string altText = "Aruba Networks";
    protected void Page_Load(object sender, EventArgs e)
    {
         Uri uriPath = Request.Url;
         string refpage = uriPath.AbsoluteUri;
         if (refpage.ToLower().Contains(ConfigurationManager.AppSettings["ALCATEL_WEB_SERVER"].ToString()))
        {
            whichSite = "alcatel";
            Session["BRAND"] = ConfigurationManager.AppSettings["ALCATEL_BRAND"];
            Session["APPID"] = ConfigurationManager.AppSettings["ALCAPP_ID"];
            altText = "Alcatel Networks";
        }
        else
        {
            whichSite = "aruba";
            Session["BRAND"] = ConfigurationManager.AppSettings["ARUBA_BRAND"];
            Session["APPID"] = ConfigurationManager.AppSettings["LMSAPP_ID"];
            altText = "Aruba Networks";
        }
        

    }
}
