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

public partial class Accounts_PendingAccountInfo : System.Web.UI.Page
{
    string strActivationCode = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            wizNewAcct.ActiveStepIndex = 0;
            if (Session["PENDING_ACCT"] == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["LOGIN_URL"], true);
            }
            else
            {

                DataSet ds = (DataSet)Session["PENDING_ACCT"];
                DataRow dr =ds.Tables[0].Rows[0];
                hdnBrand.Value = dr["brand"].ToString();
                hdnCompanyId.Value = dr["company_id"].ToString();
                hdnCustType.Value = dr["cust_type"].ToString();
                txtCompany.Text = dr["company_name"].ToString();
                hdnEmail.Value = dr["email"].ToString();
                strActivationCode = dr["acceptance_code"].ToString();
                hdnActivationCode.Value = strActivationCode;
                hdnDuplicate.Value = dr["Status"].ToString();

                User objUser = new User();
                if (objUser.IsEmailExistsSSO(hdnEmail.Value))
                {
                    hdnDuplicate.Value = "true";
                    txtPassword.Visible = false;
                    txtConfirm.Visible = false;
                    rfvConfirm.Enabled = false;
                    cmpPassword.Enabled = false;
                    divPwd.Visible = false;
                }
                else if (hdnDuplicate.Value == "ACTIVE")
                {
                    hdnDuplicate.Value = "true";
                    txtPassword.Visible = true;
                    txtConfirm.Visible = true;
                    rfvConfirm.Enabled = true;
                    cmpPassword.Enabled = true;
                    divPwd.Visible = true;
                }

            }

        }

    }
    protected void btnActivate_OnClick(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            wizNewAcct.ActiveStepIndex = 0;
            return;
        }
        else
        {
            //add activation info
            UserInfo objLicUser = new UserInfo();

            objLicUser.FirstName = txtFName.Text.Trim();
            objLicUser.LastName = txtLName.Text.Trim();
            objLicUser.Email = hdnEmail.Value;
            objLicUser.CompanyName = txtCompany.Text.Trim();
            objLicUser.Role = hdnCustType.Value;
            if (hdnDuplicate.Value != "true")
            {
                objLicUser.Status = "INACTIVE";

            }
            else
            {

                objLicUser.Status = "ACTIVE";
            }
            
            objLicUser.Phone = txtPhone.Text.Trim();
            objLicUser.Brand = hdnBrand.Value;
            objLicUser.Password = txtPassword.Text;
            objLicUser.CompanyId = Int32.Parse(hdnCompanyId.Value);
            objLicUser.AcctActivationCode = hdnActivationCode.Value;

            User objUser = new User();
            if (!objUser.AddUser(objLicUser))
            {
                setError("Unable to create account", lblError);
                return;
            }
            else
            {               
                if (hdnDuplicate.Value != "true")
                {
                    //send email

                }

                // remove pending account
                if (!objUser.RemovePendingAccount(hdnActivationCode.Value))
                {
                    setError("Unable to remove from pending accounts", lblError);
                    return;
                }
            }
            

            //move to end step
            LoadwizStep2();
        }
    }
    private void setError(string text, Label lbl)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }
    private void LoadwizStep2()
    {
        wizNewAcct.ActiveStepIndex = 1;
        if (hdnDuplicate.Value != "true")
        {
            ((Label)wizNewAcct.FindControl("LblDisplay")).Text = "You have created an account successfully.An Email will be sent to you shortly.Your response will be needed from that email to activate your account";
        }
        else
        {
            ((Label)wizNewAcct.FindControl("LblDisplay")).Text = "You have created an account successfully";
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
