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
using System.Text.RegularExpressions;


public partial class Accounts_CreateAccount : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Redirect(ConfigurationManager.AppSettings["NEWREGACCT_URL"], true);
        }
    }

//    protected void Page_Load(object sender, EventArgs e)
//    {
//        if (!IsPostBack)
//        {
//            wizNewAcct.ActiveStepIndex = 0;
//        }
//    }

//    protected void btnVerifyCert_OnClick(object sender, EventArgs e)
//    {
//        if (!Page.IsValid)
//        {
//            wizNewAcct.ActiveStepIndex = 0;
//            return;
//        }
//        wizNewAcct.ActiveStepIndex = 1;
//    }

//    protected void btnNext_OnClick(object sender, EventArgs e)
//    {
//        if (!Page.IsValid)
//        {
//            wizNewAcct.ActiveStepIndex = 1;
//            return;
//        }

//        if (hdnDuplicate.Value == "true")
//        {
//            divPwd.Visible = false;
//            rfvConfirm.Enabled = false;
//            rfvPwd.Enabled = false;
//            cmpPassword.Enabled = false;
//            spanDuplicate.Visible = false;
//        }
//        wizNewAcct.ActiveStepIndex = 2;

//    }

//    protected void cvVerifyCert_OnServerValidate(object sender, ServerValidateEventArgs e)
//    {
//        Certificate objCert = new Certificate();
//        string partType = string.Empty;
//        string partId = string.Empty;
//        DataSet ds = objCert.GetCertInfo(txtCertId.Text.Trim());
//        if (ds == null)
//        {
//            e.IsValid = false;
//            ((CustomValidator)sender).ErrorMessage = "Invalid certificate";
//            return;

//        }
//        else
//        {
//            partId = ds.Tables[0].Rows[0]["part_id"].ToString();
//            partType = objCert.getPartType(partId,Session["BRAND"].ToString());
//            if (partType == ConfigurationManager.AppSettings["RFP_TYPE"].ToString() || partType == ConfigurationManager.AppSettings["ECS_TYPE"].ToString())
//                return;            
//            else if (partType == ConfigurationManager.AppSettings["AW_TYPE"].ToString() || partType == ConfigurationManager.AppSettings["AMG_TYPE"].ToString() || partType == ConfigurationManager.AppSettings["ALE_TYPE"].ToString())
//            {
//                e.IsValid = true;
//                //return;
//            } 
//            else
//            {
//                KeyGenInput objKeyGenIp = new KeyGenInput();
//                objKeyGenIp.CertSerialNumber = txtCertId.Text.Trim();
//                objKeyGenIp.Brand = Session["BRAND"].ToString();

//                KeyGen.ArubaLicensing objKeygen = new KeyGen.ArubaLicensing();
//                KeyGen.ComplexType1 objKGInp = new KeyGen.ComplexType1();
//                objKeygen.Url = ConfigurationManager.AppSettings["KeyGen.URL"];
//                string decCertType = string.Empty;

//                if (string.Empty.Equals(objKeyGenIp.CertSerialNumber)
//                   || string.Empty.Equals(objKeyGenIp.Brand))
//                    return;

//                objKGInp.brand = objKeyGenIp.Brand;
//                objKGInp.cert = objKeyGenIp.CertSerialNumber;
//                objKGInp.acctid = "3374";
//                objKGInp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
//                objKGInp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString(); 
//                decCertType = objKeygen.decodeCert(objKGInp);
//                if (decCertType.ToLower().Contains("unknown")) //returns "unknown part" if not  valid cert id
//                {
//                    e.IsValid = false;
//                    ((CustomValidator)sender).ErrorMessage = "Invalid certificate";
//                    return;
//                }
//            }
//        }

//        if (partType == ConfigurationManager.AppSettings["AW_TYPE"].ToString())
//        {
//            ds = objCert.GetAccountsForAirwCert(txtCertId.Text.Trim());
//        }
//        //Praveena - for amigopod cert registration
//        else if (partType == ConfigurationManager.AppSettings["AMG_TYPE"].ToString())
//        {
//            ds = objCert.GetAccountsForAmigopodCert(txtCertId.Text.Trim());
//        }
//        else if (partType == ConfigurationManager.AppSettings["ALE_TYPE"].ToString())
//        {
//            ds = objCert.GetAccountsForALECert(txtCertId.Text.Trim());
//        }
//        else
//        {
//            ds = objCert.GetAccountsForCert(txtCertId.Text.Trim(), CertType.AccountCertificate);
//        }
//        if (ds == null)
//        {
//            e.IsValid = true;
//            return;
//        }
//        else
//        {
//            foreach (DataRow dr in ds.Tables[0].Rows)
//            {
//                if (dr["pk_acct_id"].ToString() != ConfigurationManager.AppSettings["CTO_ACCT"])
//                {
//                    e.IsValid = false;
//                    ((CustomValidator)sender).ErrorMessage = "This Certificate is already associated with an account.";
//                    return;
//                }
//            }
//            e.IsValid = true;
//            ((CustomValidator)sender).ErrorMessage = string.Empty;
//        }
//    }

//    protected void cvEmail_OnServerValidate(object sender, ServerValidateEventArgs e)
//    {
//        User objUser = new User();
//        string strEmail = txtEmail.Text.Trim();
//        //   strEmail = "'" + strEmail + "'";

//        if (IsValidEmail(strEmail) == false)
//        {
//            e.IsValid = false;
//            ((CustomValidator)sender).ErrorMessage = "Invalid Email Address.";
//            return;
//        }

//        string strERROR = objUser.IsEmailExists(strEmail, true);
//        if (strERROR.Equals(string.Empty))
//        {
//            e.IsValid = true;
//            if (objUser.IsEmailExistsSSO(strEmail))
//            {
//                hdnDuplicate.Value = "true";

//            }
//            else if (objUser.IsRestrictedDomain(strEmail,true))
//            {
//                e.IsValid = false;
//                ((CustomValidator)sender).ErrorMessage = "This Email ID is found in our list of Restricted Domain";
//                return;
//            }

//        }
//        else
//        {
//            e.IsValid = false;
//            ((CustomValidator)sender).ErrorMessage = strERROR;
//            return;
//        }

//    }

//    protected void btnActivate_OnClick(object sender, EventArgs e)
//    {
//        if (!Page.IsValid)
//        {
//            wizNewAcct.ActiveStepIndex = 2;
//            return;
//        }
//        else
//        {
//            //add activation info
//            UserInfo objLicUser = new UserInfo();

//            objLicUser.FirstName = txtFName.Text.Trim();
//            objLicUser.LastName = txtLName.Text.Trim();
//            objLicUser.Email = txtEmail.Text.Trim(); ;
//            objLicUser.CompanyName = txtCompany.Text.Trim();
//            objLicUser.Role = UserType.Customer;

//            if (hdnDuplicate.Value.Equals("true"))
//                objLicUser.Status = "ACTIVE";
//            else
//                objLicUser.Status = "INACTIVE";

//            objLicUser.Phone = txtPhone.Text.Trim();
//            objLicUser.Brand = Session["BRAND"].ToString();
//            objLicUser.Password = txtPassword.Text;
            
//            User objUser = new User();
//            if (!objUser.AddUser(objLicUser))
//            {
//                setError("Unable to create account", lblError);
//                return;
//            }
//            //move to end step
//            wizNewAcct.ActiveStepIndex = 3;
//        }
//    }
//    private void setError(string text, Label lbl)
//    {
//        lbl.Text = text;
//        lbl.Visible = true;
//    }

//    private bool IsValidEmail(string strEmail)
//    {
//        if (Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") == false)
//        {
//            return false;
//        }
//        else
//        {
//            return true;
//        }
//    }


//    protected void Page_Error(object sender, EventArgs args)
//    {
//        Exception objExcep = Server.GetLastError();
//        Server.ClearError();
//        new Log().logException(sender.ToString(), objExcep);
//        Response.Redirect("Error.aspx", true);
//    }

}
