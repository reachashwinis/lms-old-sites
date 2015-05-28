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

public partial class Controls_HighAvailability : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnHighAvail_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;
        Email objEmail = new Email();
        string cert_id = txtHighAvail.Text.Trim();
        string subscriptionKey = txtSubscriptionId.Text.Trim();

        string soId = string.Empty;
        string poId = string.Empty;
        int licenseCount = 0;
        int company = 0;
        string company_name = string.Empty;
        string email = string.Empty;
        string Name = string.Empty;
        string part_id = string.Empty;
        string category = string.Empty;
        
        string ha_subscription_key = string.Empty;
        string type = ConfigurationManager.AppSettings["CLEARPASS_HA"].ToString();
        bool isActivated = false;

        string meth = "btnHighAvail_click_HighAvailability";
        Certificate objCert = new Certificate();
        UserInfo objUser = new UserInfo();
        objUser = (UserInfo)Session["USER_INFO"];
        AmigopodCertInfo objAmigopodCertinfo = new AmigopodCertInfo();
        Email objMail = new Email();
        Company objComp = new Company();
        AmigopodXML objAmigopodXML = new AmigopodXML();
        try
        {
            //get the details of certificate from db
            DataSet dsAmigopodCert = objCert.getAmigopodCertDetails(cert_id, Session["BRAND"].ToString());

            //check if certificate exists
            if (dsAmigopodCert.Tables[0].Rows.Count <= 0)
            {
                lblErr.Text = "This Certificate does not exist.";
                lblErr.Visible = true;
                return;
            }

            //Check whether already generated the Subscription ID
            string import_type = "HA:" + ConfigurationManager.AppSettings["CLEARPASS_IMPORT"].ToString();
            if (objCert.IsAmigoppodCertActivated(cert_id, type) || objCert.IsAmigoppodCertActivated(cert_id, import_type))
            {
                lblErr.Text = "This Certificate is already used to add High Availability";
                lblErr.Visible = true;
                return;
            }

            //to do - to check whether the subscription ID is valid
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

            //to check the business rule - verify the certificate id is that of High Availability sku
            DataSet dsAmigopodLookup = objCert.GetAmigoppodLookInfo(cert_id, Session["BRAND"].ToString());
            string partType = string.Empty;

            if (dsAmigopodLookup.Tables[0].Rows.Count > 0)
            {
                partType = dsAmigopodLookup.Tables[0].Rows[0]["part_type"].ToString();
            }

            if (partType != type)
            {
                lblErr.Text = "The certificate id is not of the type High Availability and can not be used to enable High Avaialbity on this subscription.";
                lblErr.Visible = true;
                return;
            }

            if (dsAmigopodCert.Tables[0].Rows.Count > 0)
            {

                soId = dsAmigopodCert.Tables[0].Rows[0]["so_id"].ToString();
                poId = dsAmigopodCert.Tables[0].Rows[0]["cust_po_id"].ToString();
                licenseCount = Convert.ToInt32(dsAmigopodLookup.Tables[0].Rows[0]["license_count"]);
                part_id = dsAmigopodLookup.Tables[0].Rows[0]["part_id"].ToString();
                category = dsAmigopodLookup.Tables[0].Rows[0]["category"].ToString();

                company = objUser.GetUserCompanyId();
                email = objUser.GetUserEmail();
                Name = objUser.GetUserFirstName() + " " + objUser.GetUserLastName();


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
                    objMail.sendAmigopodErrorMessage("Could not find company info of the user: " + email, "Add HA", cert_id, "");
                    return;
                }

                objAmigopodXML = objCert.AddHighAvailability(subscriptionKey, "1",cert_id);
                ha_subscription_key = objAmigopodXML.subscription_key;
            }
            if (objAmigopodXML.error != "0")
            {
                new Log().logInfo(meth, " Message : " + objAmigopodXML.message + " ; Error : " + objAmigopodXML.error+" ;Sub Key : "+subscriptionKey);
                lblErr.Text = "Application could not process your request.The error occurred has been logged";
                objEmail.sendAmigopodErrorMessage(" Message : " + objAmigopodXML.message + " ; Error : " + objAmigopodXML.error, "High Availability", cert_id, subscriptionKey);
                lblErr.Visible = true;
                return;
            }
            else if (objAmigopodXML.subscription_key != string.Empty && objAmigopodXML.error == "0")
            {
                isActivated = objCert.InsertAmigopodActkey(ha_subscription_key, cert_id, type, objUser.GetUserAcctId(), company, company_name, "Adding High Availability key", objUser.AcctId,"");
                if (isActivated)
                {
                    //send mail
                    objAmigopodCertinfo.Subscriptionkey = subscriptionKey;
                    objAmigopodCertinfo.SoId = soId;
                    objAmigopodCertinfo.PoId = poId;
                    objAmigopodCertinfo.Name = Name;
                    objAmigopodCertinfo.Email = email;
                    objAmigopodCertinfo.Brand = Session["BRAND"].ToString().ToUpper();

                    bool blSend = objMail.sendAmigopodAddHaInfo(objAmigopodCertinfo, category, objAmigopodXML.email, objAmigopodXML.password,ha_subscription_key);

                    ((Literal)wizActivate.FindControl("LiteralAct")).Text = ha_subscription_key.Replace("\n", "<BR>");
                    wizActivate.ActiveStepIndex = 1;
                }
                else
                {
                    new Log().logInfo(meth, "Unable to add HA to db : " + ha_subscription_key + " ;sub key" + subscriptionKey);
                    lblErr.Text = "Application could not process your request.The error occurred has been logged";
                    objEmail.sendAmigopodErrorMessage("Unable to add HA to db: " + ha_subscription_key, "High Availability", cert_id, subscriptionKey);
                    lblErr.Visible = true;
                }
                isActivated = objCert.UpdateHAActkey(ha_subscription_key, subscriptionKey, objUser.GetUserAcctId());
                if (!isActivated)
                {
                    new Log().logInfo(meth, "Unable to update the HA activation key: " + ha_subscription_key);
                }
            }
            else
            {
                new Log().logInfo(meth, " Message : " + objAmigopodXML.message + " ; Error : " + objAmigopodXML.error + " ;Sub Key : " + subscriptionKey);
                lblErr.Text = "Application could not process your request.The error occurred has been logged";
                objEmail.sendAmigopodErrorMessage(" Message : " + objAmigopodXML.message + " ; Error : " + objAmigopodXML.error, "High Availability", cert_id, subscriptionKey);
                lblErr.Visible = true;
            }
        }
        catch (Exception ex)
        {
            new Log().logException(meth, ex);
            lblErr.Text = "Application could not process your request.The error occurred has been logged";
            lblErr.Visible = true;
            objEmail.sendAmigopodErrorMessage(ex.Message, "High Availability", cert_id, subscriptionKey);
        }
    }

    private bool isHighAvailSKU(string part_id)
    {
        return true;
    }
}
