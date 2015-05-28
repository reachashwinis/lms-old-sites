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

public partial class Controls_AddAdvertising : System.Web.UI.UserControl
{
    Certificate objCert = new Certificate();
    string meth = "btnAddAdvertising_Click";
    string type = ConfigurationManager.AppSettings["CLEARPASS_ADV"].ToString();
    Email objemail = new Email();

    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false;
    }

    protected void btnAddAdvertising_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;
        
        string cert_id = txtCertId.Text.Trim();
        string subscriptionKey = txtSubscriptionId.Text.Trim();
        bool blAddAmigopod = false;
        UserInfo objUser = new UserInfo();
        objUser = (UserInfo)Session["USER_INFO"];

        Company objComp = new Company();
        string company_name = string.Empty;

        try {
            //get the details of certificate from db
            DataSet dsAmigopodCert = objCert.getAmigopodCertDetails(cert_id, Session["BRAND"].ToString());

            //check if certificate exists
            if (dsAmigopodCert.Tables[0].Rows.Count <= 0)
            {
                lblErr.Text = "This Certificate does not exist.";
                lblErr.Visible = true;
                return;
            }

            //Check whether already upgraded the certificate
            string import_type = "ADV:" + ConfigurationManager.AppSettings["CLEARPASS_IMPORT"].ToString();

            if (objCert.IsAmigoppodCertActivated(cert_id, type) || objCert.IsAmigoppodCertActivated(cert_id, import_type))
            {
                lblErr.Text = "This advertising feature is already used on some other subscription ";
                lblErr.Visible = true;
                return;
            }
            //check whether the subscription ID is valid
            if (!objCert.isValidAmigopodSubscription(subscriptionKey))
            {
                if (!objCert.IsLegacySubscription(subscriptionKey))
                {
                    lblErr.Text = "The Subscription ID is not valid";
                    lblErr.Visible = true;
                    return;
                }
                else
                {
                    lblErr.Text = "The Subscription ID is generated from leagcy site and needs to be imported to LMS. Please use <I><font style=\"font-weight:bold;color:maroon\"> Import Legacy Cert </font></I> feature avaialble on this site to import the certificate and try again.";
                    lblErr.Visible = true;
                    return;
                }                
            }

            //to check if the advertising feature is already enabled in this subscription
            if (objCert.isEnabledAdvertisingPlugin(subscriptionKey, Session["BRAND"].ToString(),type))
            {
                lblErr.Text = "Advertising feature is already enabled on this subscription";
                lblErr.Visible = true;
                return;
            }

            //Get the lookup info for amigopod SKU
            //to check the business rule - verify the certificate id is that of Advertising License
            DataSet dsAmigopodLookup = objCert.GetAmigoppodLookInfo(cert_id, Session["BRAND"].ToString());
            string part_id = string.Empty;
            string partType = string.Empty;

            if (dsAmigopodLookup.Tables[0].Rows.Count > 0)
            {
                part_id = dsAmigopodLookup.Tables[0].Rows[0]["part_id"].ToString();
                partType = dsAmigopodLookup.Tables[0].Rows[0]["part_type"].ToString();
            }

            if (partType != type)
            {
                lblErr.Text = "The certificate id is not that of an Advertising License";
                lblErr.Visible = true;
                return;
            }

            
            int company = objUser.GetUserCompanyId();

            string  email = objUser.GetUserEmail();
            string  Name = objUser.GetUserFirstName() + " " + objUser.GetUserLastName();
            
            //to get the company name
            DataSet dsCompany = objComp.GetCompanyInfo(company);
            if (dsCompany !=null && dsCompany.Tables[0].Rows.Count > 0)
                company_name = dsCompany.Tables[0].Rows[0]["company_name"].ToString();
            else
            {
                //company info is not available in company table.
                //Getting the comapny name from accounts
                company_name = objCert.GetCompanyInfoByEmail(email);
            }
            if (company_name.Equals(string.Empty))
            {
                //if company name is not set in accounts also then throw error
                new Log().logInfo(meth, "Error : Could not find company info of the user: " + email);
                lblErr.Text = "Application could not process your request.The error occurred has been logged";
                lblErr.Visible = true;
                objemail.sendAmigopodErrorMessage("Could not find company info of the user: " + email, "Add Advertising", cert_id, "");
                return;
            }

            AmigopodXML objAmigopodXML = AddAdvertising(dsAmigopodCert, subscriptionKey, cert_id);
            if (objAmigopodXML.error == "1")
            {
                new Log().logInfo(meth, "Message : " + objAmigopodXML.message + "; Error : " + objAmigopodXML.error+" ;Sub key"+subscriptionKey);
                lblErr.Text = "Application could not process your request.The error occurred has been logged";
                lblErr.Visible = true;
                objemail.sendAmigopodErrorMessage("Message : " + objAmigopodXML.message + "; Error : " + objAmigopodXML.error, "AddAdvertising",cert_id,subscriptionKey);
                return;
            }
            else if (objAmigopodXML.error == "0")
            {
                sendAlert(dsAmigopodCert, subscriptionKey, cert_id, objUser.GetUserAcctId(), company, company_name, objUser.AcctId, Name, email);
            }
            else
            {
                new Log().logInfo(meth, "Message : " + objAmigopodXML.message + "; Error : " + objAmigopodXML.error + " ;Sub key" + subscriptionKey);
                lblErr.Text = "Application could not process your request.The error occurred has been logged";
                lblErr.Visible = true;
                objemail.sendAmigopodErrorMessage("Message : " + objAmigopodXML.message + "; Error : " + objAmigopodXML.error, "AddAdvertising",cert_id,subscriptionKey);
            }
        }
        
        catch (Exception ex)
        {
            new Log().logException(meth, ex);
            lblErr.Text = "Application could not process your request.The error occurred has been logged";
            lblErr.Visible = true;
            objemail.sendAmigopodErrorMessage(ex.Message, "AddAdvertising",cert_id,subscriptionKey);
        }
    }

    private AmigopodXML AddAdvertising(DataSet dsAmigopodCert,string subscriptionkey,string cert_id)
    {
        AmigopodXML objAmigopod = new AmigopodXML();
        if (dsAmigopodCert.Tables[0].Rows.Count > 0)
        {
            objAmigopod = objCert.AddAdvertisingAmigopod(subscriptionkey, cert_id); 
        }
        return objAmigopod;
    }

    private bool isAdvertSKU(string sku)
    {
        bool isValid = false;
        Email objemail = new Email();
        try
        {
            string upgrade = sku.Substring(4, 3);
            if (upgrade == "ADV")
                isValid = true;
        }
        catch (Exception ex)
        {
            new Log().logException("isAdvertSKU", ex);
            objemail.sendAmigopodErrorMessage(ex.Message, "isAdvertSKU","","");
        }

        return isValid;
    }

    private void sendAlert(DataSet dsAmigopodCert, string subscriptionkey, string cert_id,int userId, int company_id,string company_name,int impersonatedById,string name,string email)
    {
        AmigopodCertInfo objAmigopodCertinfo = new AmigopodCertInfo();
        Email objMail = new Email();
        bool isActivated = objCert.InsertAmigopodActkey(subscriptionkey, cert_id, type, userId, company_id,company_name, "Adding advertising feature", impersonatedById,"");
        if (isActivated)
        {
            objAmigopodCertinfo.Subscriptionkey = subscriptionkey;
            objAmigopodCertinfo.SoId = dsAmigopodCert.Tables[0].Rows[0]["so_id"].ToString(); ;
            objAmigopodCertinfo.PoId = dsAmigopodCert.Tables[0].Rows[0]["cust_po_id"].ToString(); ;
            objAmigopodCertinfo.Name = name;
            objAmigopodCertinfo.Email = email;
            objAmigopodCertinfo.PartId = dsAmigopodCert.Tables[0].Rows[0]["part_id"].ToString(); ;
            objAmigopodCertinfo.CertId = cert_id;

            objAmigopodCertinfo.Brand = Session["BRAND"].ToString().ToUpper();
            bool blSend = objMail.sendAmigopodUpgradeInfo(objAmigopodCertinfo);

            ((Literal)wizActivate.FindControl("LiteralAct")).Text = subscriptionkey;
            wizActivate.ActiveStepIndex = 1;
        }
        else
        {
            new Log().logInfo(meth, "Unable to insert the advertising license : "+cert_id+ "in to db. ;sub key "+subscriptionkey);
            lblErr.Text = "Application could not process your request.The error occurred has been logged";
            lblErr.Visible = true;
            objemail.sendAmigopodErrorMessage("Unable to insert the advertising license.", "AddAdvertising", cert_id, subscriptionkey);
        }
    }
}
