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

public partial class Accounts_RegisterPromoAcct : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            wizNewAcct.ActiveStepIndex = 0;
        }
    }

    protected void btnVerifyCert_OnClick(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            wizNewAcct.ActiveStepIndex = 0;
            return;
        }

        wizNewAcct.ActiveStepIndex = 1;
    }

    protected void btnNext_OnClick(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            wizNewAcct.ActiveStepIndex = 1;
            return;
        }

        if (hdnDuplicate.Value == "true")
        {
            divPwd.Visible = false;
            rfvConfirm.Enabled = false;
            rfvPwd.Enabled = false;
            cmpPassword.Enabled = false;
            spanDuplicate.Visible = false;
        }
        wizNewAcct.ActiveStepIndex = 2;
    }

    protected void cvVerifyCert_OnServerValidate(object sender, ServerValidateEventArgs e)
    {
        Certificate objCert = new Certificate();
        DataSet ds = objCert.GetIAPInfo(txtCertId.Text.Trim());
        if (ds == null)
        {
            e.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = "Invalid serial Number";
            return;
        }
        else
        {
            e.IsValid = true;
            return;
        }                  
    }

    protected void cvVerifyPromoCode_OnServerValidate(object sender, ServerValidateEventArgs e)
    {
        if (txtPromoCode.Text != "AIRWAVE90")
        {
            e.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = "Invalid Promotion Code.";
            return;
        }
        else
        {
            e.IsValid = true;
            return;
        }                  
    }

    protected void cvEmail_OnServerValidate(object sender, ServerValidateEventArgs e)
    {
        User objUser = new User();
        string strEmail = txtEmail.Text.Trim();
        //   strEmail = "'" + strEmail + "'";
        string strERROR = objUser.IsEmailExists(strEmail, true);
        if (strERROR.Equals(string.Empty))
        {
            e.IsValid = true;
            if (objUser.IsEmailExistsSSO(strEmail))
            {
                hdnDuplicate.Value = "true";

            }
            else if (objUser.IsRestrictedDomain(strEmail,true))
            {
                e.IsValid = false;
                ((CustomValidator)sender).ErrorMessage = "This Email Id is found in our list of Restricted Domain";
            }
        }
        else
        {
            e.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = strERROR;
        }

    }

    protected void btnActivate_OnClick(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            wizNewAcct.ActiveStepIndex = 2;
            return;
        }
        else
        {
            //add activation info
            UserInfo objLicUser = new UserInfo();

            objLicUser.FirstName = txtFName.Text.Trim();
            objLicUser.LastName = txtLName.Text.Trim();
            objLicUser.Email = txtEmail.Text.Trim(); ;
            objLicUser.CompanyName = txtCompany.Text.Trim();
            //objLicUser.Role = UserType.Customer;
            objLicUser.Role = "promo-customer";

            if (hdnDuplicate.Value.Equals("true"))
                objLicUser.Status = "ACTIVE";
            else
                objLicUser.Status = "INACTIVE";

            objLicUser.Phone = txtPhone.Text.Trim();
            objLicUser.Brand = Session["BRAND"].ToString();
            objLicUser.Password = txtPassword.Text;            

            User objUser = new User();
            if (!objUser.AddUser(objLicUser))
            {
                setError("Unable to create account", lblError);
                return;
            }
            //move to end step
            wizNewAcct.ActiveStepIndex = 3;
        }
    }
    private void setError(string text, Label lbl)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }

    protected void Page_Error(object sender, EventArgs args)
    {
        Exception objExcep = Server.GetLastError();
        Server.ClearError();
        new Log().logException(sender.ToString(), objExcep);
        Response.Redirect("Error.aspx", true);
    }
}

