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

public partial class Controls_Home : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserInfo objUserInfo = (UserInfo)(Session["USER_INFO"]);
        if (objUserInfo.Role == ConfigurationManager.AppSettings["DEFAULT_ROLE"].ToString())
        {
            Response.Redirect(ConfigurationManager.AppSettings["DEFAULT_ROLE_PAGE"].ToString(), true);
        }

    }
}
