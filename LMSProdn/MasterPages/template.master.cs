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
using Com.Arubanetworks.Licensing.Lib.Data;
using Com.Arubanetworks.Licensing.Lib.Utils;

public partial class template : System.Web.UI.MasterPage
{
    public string whichSite = "aruba";
    public string altText = "Aruba Networks";
    protected void Page_Load(object sender, EventArgs e)
    {
        UserInfo objLicUser = new UserInfo();
        Session["BRAND"] = ConfigurationManager.AppSettings["ARUBA_BRAND"];
        Uri uriPath = Request.Url;
        string refpage = uriPath.AbsoluteUri;
        string strpath = Request.ApplicationPath;
        if (refpage.ToLower().Contains(ConfigurationManager.AppSettings["ALCATEL_WEB_SERVER"]))
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

        objLicUser = (UserInfo)Session["USER_INFO"];
        if (objLicUser != null)
        {
            if (objLicUser.IsImpersonate == true)
            {
                lnkLogout.Text = "LOGOUT " + objLicUser.ImpersonateEmail;
                lnkLogout.Font.Bold = true;
            }
        }
    }


    protected void lnkLogout_Click(object sender, EventArgs args)
    {
        UserInfo objLicUser = new UserInfo();
        objLicUser = (UserInfo)Session["USER_INFO"];
        if (objLicUser != null)
        {
            if (objLicUser.IsImpersonate == true)
            {
                objLicUser.IsImpersonate = false;
                objLicUser.ImpersonateEmail = string.Empty;
                objLicUser.ImpersonateUserRole = string.Empty;
                Session["USER_INFO"] = objLicUser;
                Cache.Remove("MENU_ITEMS");
                Response.Redirect(ConfigurationManager.AppSettings["DEFAULT_URL"].ToString(), true);
            }
            else
            {
                Session.Abandon();
                Cache.Remove("MENU_ITEMS");
                string LoginUrl = Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + ResolveUrl(ConfigurationManager.AppSettings["LOGIN_URL"].ToString());
                string logouturl = String.Format(ConfigurationManager.AppSettings["PingLogOut_Page"].ToString(), LoginUrl);
                Response.Redirect(logouturl, true);
            }
        }
    }
}
