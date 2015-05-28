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


public partial class Controls_QCUserPwd : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnVerifyUser_OnClick(object sender, EventArgs e)
    {
        UserInfo objLicUser = new UserInfo();
        User objUserModule = new User();
        string errMessage = string.Empty;
        string UserIPAddr = Request.ServerVariables["REMOTE_HOST"].ToString();        
        errMessage = objUserModule.AuthenticateUser(txtUserName.Text.Trim(), txtPassword.Text.Trim(), UserIPAddr);       
        if (errMessage.Equals(string.Empty))
        {
            wizPwd.ActiveStepIndex = 1;
            LiteralQCUser.Text = Session["qcuser"].ToString();
            //LiteralQCPwd.Text = GetQuickConnectUser(Session["qcuser"].ToString());
        }
        else
        {
            lblError.Text = "Invalid Login/password";
            return;            
        }        
    }

    //private string GetQuickConnectUser(string Username)
    //{
    //    Certificate objCertificate = new Certificate();
    //    return objCertificate.GetQuickConnectUser(Username);         
    //}
}
