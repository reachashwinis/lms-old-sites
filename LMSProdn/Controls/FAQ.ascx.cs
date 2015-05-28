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
using Com.Arubanetworks.Licensing.Dataaccesslayer;
using Com.Arubanetworks.Licensing.Lib.Data;
using Com.Arubanetworks.Licensing.Lib.Utils;

public partial class Controls_FAQ : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UpdateDataView();
    }

    private void UpdateDataView()
    {
        Certificate objCert = new Certificate();
        UserInfo objUserInfo = new UserInfo();
        objUserInfo = (UserInfo)Session["USER_INFO"];
        DataSet GridData = objCert.GetFAQ(objUserInfo.Role, Session["BRAND"].ToString());
        RptFAQ.DataSource = GridData;
        RptFAQ.DataBind();
    }
}
