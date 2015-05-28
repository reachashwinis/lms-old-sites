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


public partial class Accounts_ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect(ConfigurationManager.AppSettings["NEWFORGOTPW_URL"], true);
    }
    //protected void btnForgotPwd_OnClick(object sender, EventArgs e)
    //{
    //    User objUser = new User();
    //    bool flg = objUser.IsWindowsLoginIDSSO(txtEmail.Text.ToLower(),Session["APPID"].ToString());
    //    if (txtEmail.Text.ToLower().Contains("@arubanetworks.com") && objUser.IsWindowsLoginIDSSO(txtEmail.Text.ToLower(), Session["APPID"].ToString()))
    //    {
    //        setError("You cannot reset password for - "+txtEmail.Text+ " here.",lblError);
    //        return;
    //    }
    //    string strError = objUser.IsEmailExists(txtEmail.Text.Trim(), false);
    //    if (!strError.Equals(string.Empty))
    //    {
    //        if (objUser.IsEmailExistsSSO(txtEmail.Text.Trim()))
    //        {
    //            string strPassword = Membership.GeneratePassword(15,5);
    //            bool retVal = objUser.ResetPassword(txtEmail.Text.Trim(), strPassword);
    //            if (retVal)
    //            { 
    //            //send email
    //                AccountMailInfo objLgm = new AccountMailInfo();
    //                objLgm.Email = txtEmail.Text.Trim();
    //                objLgm.Password = strPassword;
    //                objLgm.Brand = Session["BRAND"].ToString().ToUpper();
    //                Email objEmail = new Email();
    //                bool result = objEmail.sendLoginInfo(objLgm);
    //                //setError(strPassword, lblError);

    //                new Log().logInfo("Reset Password","Password reset for :"+txtEmail.Text.Trim()+" | IP: "+Request.ServerVariables["REMOTE_HOST"].ToString()+" |On: "+DateTime.Now.ToString());
    //                LblSuccess.Visible = true;
    //                LblSuccess.Text = "Password is changed!. Email will be sent to " +txtEmail.Text.Trim() + " now";
    //            }
    //            else
    //            {
    //            setError("Unable to reset your password",lblError);
    //            }
    //        }
    //        else
    //        {
    //            setError("Unable to find Info for "+txtEmail.Text, lblError);          
    //        }

    //    }
    //    else
    //    {
    //        setError("Invalid LoginID", lblError);
    //    }
    
    //}
    //private void setError(string text, Label lbl)
    //{
    //    lbl.Text = text;
    //    lbl.Visible = true;
    //}

    //protected void Page_Error(object sender, EventArgs args)
    //{
    //    Exception objExcep = Server.GetLastError();
    //    Server.ClearError();
    //    new Log().logException(sender.ToString(), objExcep);
    //    Response.Redirect("Error.aspx", true);
    //}
}
