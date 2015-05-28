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
using System.Text;

public partial class UpdateMyEmailId : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnEnter_OnClick(object sender, EventArgs e)
    {
        User objUserModule = new User();
        User objUser = new User();
        UserInfo objUserInfo = new UserInfo();
        bool blResult, blRestrict;
        blRestrict = objUser.IsRestrictedDomain(txtEmail.Text);
        if (blRestrict == true)
        {
            lblError.Text = "This Email Id is found in our list of Restricted Domain";
            return;
        }
        objUserInfo = objUser.GetUserInfo(txtEmail.Text, Session["BRAND"].ToString());
        if (objUserInfo != null)
        {
            lblError.Text = "This Email Id already Exists.";
            return;
        }

        blResult = objUserModule.UpdateUserEmailId(txtEmail.Text, Session["EMail"].ToString());
        if (blResult == true)
        {
            LblSuccess.Text = "Your Email Id is updated successfully." + "<A href = 'Login.aspx'> Please Login here.</A>";
            lblError.Text = string.Empty;
        }
        else
        {
            lblError.Text = "Failed to Update your Email Id.Please contact TAC team";
        }
    }

    protected void Page_Error(object sender, EventArgs args)
    {
        Exception objExcep = Server.GetLastError();
        Server.ClearError();
        new Log().logException(sender.ToString(), objExcep);
        Response.Redirect("Error.aspx", true);
    }
}
