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

public partial class Accounts_ActivateAccountIP : System.Web.UI.Page
{
    string strIPAddress;
    string strAppID;
    protected void Page_Load(object sender, EventArgs e)
    {
        Uri objUri = Request.Url;
        string strPath = objUri.AbsoluteUri;

        if (strPath.ToLower().Contains(ConfigurationManager.AppSettings["ALCATEL_WEB_SERVER"]))
        {
            strAppID = ConfigurationManager.AppSettings["ALCAPP_ID"].ToString();
        }
        else
        {
            strAppID = ConfigurationManager.AppSettings["LMSAPP_ID"].ToString();
        }

        strIPAddress = Request.ServerVariables["REMOTE_HOST"].ToString();

        if (!IsPostBack)
        {
            if (Request.QueryString["k"] != null && !Request.QueryString["k"].ToString().Equals(string.Empty))
            {

                spanActCode.Visible = false;

                //update acct
                string strIPActivationCode = Request.QueryString["k"].ToString();

                ProcessActivation(strIPActivationCode, strIPAddress);

                //show span
            }
            else
            {
                spanActCode.Visible = true;
                spanActive.Visible = false;
                //setError("Activation Code not found", lblError);
            }

        }

    }
    private void setError(string text, Label lbl)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }

    protected void btnActivate_OnClick(object sender, EventArgs args)
    {
        if (!Page.IsValid)
            return;
        string strIPActivationCode = TxtUserId.Text;
        ProcessActivation(strIPActivationCode, strIPAddress);
    }

    private void ProcessActivation(string strIPActivationCode, string strIPAddress)
    {
        User objUser = new User();

        if (objUser.UpdateIPAddress(strIPActivationCode, strIPAddress))
        {
            lblError.Visible = false;
            spanActive.Visible = true;
            spanActCode.Visible = false;
            LblDisplay.Visible = true;
        }
        else
        {
            spanActive.Visible = false;
            setError("Unable to Update IP Address", lblError);
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
