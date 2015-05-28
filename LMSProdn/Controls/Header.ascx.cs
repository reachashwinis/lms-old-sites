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

public partial class Controls_Header : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["USER_INFO"]==null)
            Response.Redirect(ConfigurationManager.AppSettings["LOGIN_URL"],true);

        UserInfo objUserInfo = new UserInfo();
        objUserInfo = (UserInfo)Session["USER_INFO"];
        lblUserName.Text = objUserInfo.GetUserEmail();
 
    }
    protected void imgLogout_Click(object sender, ImageClickEventArgs e)
    {
        UserInfo objLicUser = new UserInfo();
        objLicUser = (UserInfo)Session["USER_INFO"];
        if (objLicUser.IsImpersonate == true)
        {
            objLicUser.IsImpersonate = false;
            objLicUser.ImpersonateEmail = string.Empty;
            objLicUser.ImpersonateUserRole = string.Empty;
            Session["USER_INFO"] = objLicUser;
            Response.Redirect(ConfigurationManager.AppSettings["DEFAULT_URL"].ToString(), true);
        }
        else
        {
            Session.Abandon();
            Session.Clear();
            Response.Redirect(ConfigurationManager.AppSettings["LOGIN_URL"], true);
        }
    
    }
}
