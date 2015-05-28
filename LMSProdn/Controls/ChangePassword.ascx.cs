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
using System.Drawing;
using System.Web.Caching;
using System.Collections.Generic;

public partial class Controls_ChangePassword : System.Web.UI.UserControl
{
    //UserInfo objUserInfo;
    //User objUser = new User();

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect(ConfigurationManager.AppSettings["NEWCHPW_URL"], true);
    }

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    lblError.Visible=false;
    //    lblSuccess.Visible=false;
    //    objUserInfo = (UserInfo)Session["USER_INFO"];
    //    string email = objUserInfo.GetUserEmail();

    //    if (email.ToLower().Contains("@arubanetworks.com") && objUser.IsWindowsLoginIDSSO(email, Session["APPID"].ToString()))
    //        pnlNonAruba.Visible = false;
    //    else
    //        pnlAruba.Visible = false;

    //}
    //protected void btnChgPwd_OnClick(object sender, EventArgs args)
    //{
    //    string strERROR = string.Empty;
    //    string strSUCCESS = string.Empty;

    //    if (!Page.IsValid)
    //        return;

    //    if (txtCurrPass.Text.Length > 0 && txtNewPass.Text.Length > 0 )
    //    {
    //        if (txtNewPass.Text.Contains(" "))
    //        {
    //            strERROR = "Password should not contain blanks";
    //            setError (strERROR,lblError);
    //            return;
    //        }

    //        if(!txtNewPass.Text.Equals(txtConfPass.Text))
    //        {
    //            strERROR = "Passwords do not match";
    //            setError (strERROR,lblError);
    //            return;
    //        }

    //        User objUser = new User();
    //        if (objUser.ChangePassword(objUserInfo.GetUserEmail(), txtCurrPass.Text, txtNewPass.Text))
    //        {
    //            //mail
    //            Email objEmail = new Email();
    //            AccountMailInfo objLgm = new AccountMailInfo();
    //            objLgm.Email = objUserInfo.GetUserEmail();
    //            objLgm.Password = txtNewPass.Text;
    //            objLgm.Brand = Session["BRAND"].ToString().ToUpper();
    //            objEmail.sendChangePasswordNotification(objLgm);
    //            strSUCCESS = "Password changed successfully.";
    //        }
    //        else
    //        {
    //            strERROR = "Unable to change password";
    //        }
    //    }
    //    else
    //    { 
    //    strERROR="All fields are mandatory";
        
    //    }
    //    setError(strERROR,lblError);
    //    setError(strSUCCESS,lblSuccess);

    //}
    //private void setError(string text, Label lbl)
    //{
    //    lbl.Text = text;
    //    lbl.Visible = true;
    //}

}
