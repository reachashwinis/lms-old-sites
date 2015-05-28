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

public partial class Pages_MyControllers : Com.Arubanetworks.Licensing.Lib.BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        runPagePreReqs();
        string strpageInfo = ConfigurationManager.AppSettings["LISTSMYCONT_AUTH"].ToString();
        string[] arrPageInfo = strpageInfo.Split('|');
        UserInfo objUserInfo = (UserInfo)(Session["USER_INFO"]);
        checkAccessToPage(arrPageInfo[1], Int32.Parse(arrPageInfo[0]), objUserInfo.GetUserRole(), objUserInfo.GetUserEmail(), objUserInfo.GetUserAcctId());
    }
}
