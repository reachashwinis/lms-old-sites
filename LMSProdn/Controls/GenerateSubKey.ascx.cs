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

public partial class Controls_GenerateSubKey : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;
        string subscriptionKey = string.Empty;
        string soId = string.Empty;
        string poId = string.Empty;
        DateTime expDate;
        int licenseCount = 0;
        int company = 0;
        string company_name = string.Empty;
        string email = string.Empty;
        string Name = string.Empty;
        bool isBaseSku = false;
        bool isOnBoard = false;

        bool isActivated = false;
        string cert_id = string.Empty;
        string category = string.Empty;
        string meth = "btnSubmit_click_GenerateSubscription";

        string strSKUId = ConfigurationManager.AppSettings["AMIGOPOD_SKU_ID"].ToString();
        string do_guestconnect = "0";

        string type = ConfigurationManager.AppSettings["CLEARPASS_ONBOARD"].ToString();

        Certificate objCert = new Certificate();
        UserInfo objUser = new UserInfo();
        objUser = (UserInfo)Session["USER_INFO"];
        AmigopodCertInfo objAmigopodCertinfo = new AmigopodCertInfo();
        Email objMail = new Email();
        Company objComp = new Company();
        AmigopodXML objAmigopodXML = new AmigopodXML();
        cert_id = txtCertId.Text.Trim();
        
        try {
            
            //Get the cert details from certGen db
            DataSet dsAmg = objCert.getAmigopodCertFromCertGen(cert_id, Session["BRAND"].ToString());
            if (dsAmg.Tables[0].Rows.Count > 0)
                expDate = Convert.ToDateTime(dsAmg.Tables[0].Rows[0]["expiration_date"].ToString());
            else
            {
                new Log().logInfo(meth, "Certificate could not found in certgen db: "+cert_id);
                lblErr.Text = "This Certificate does not exist.";
                lblErr.Visible = true;
                objMail.sendAmigopodErrorMessage("Certificate could not found in certgen db: "+cert_id, "GenerateSubKey", cert_id, "");
                return;
            }

            //get the details of certificate from db
            DataSet dsAmigopodCert = objCert.getAmigopodCertDetails(cert_id, Session["BRAND"].ToString());

            //check if certificate exists
            if (dsAmigopodCert.Tables[0].Rows.Count <= 0)
            {
                lblErr.Text = "This Certificate does not exist.";
                lblErr.Visible = true;
                return;
            }

            //Check whether already generated the subscription ID
            if (objCert.IsAmigoppodCertActivated(cert_id, ConfigurationManager.AppSettings["CLEARPASS_ACTIVATION"].ToString()) || objCert.IsAmigoppodCertActivated(cert_id, ConfigurationManager.AppSettings["CLEARPASS_IMPORT"].ToString()))
            {
                lblErr.Text = "The Subscription ID for this Certificate is already generated";
                lblErr.Visible = true;
                return;
            }

            string partType = string.Empty;
            //to check the business rule - verify the certificate id is that of base clearpass sku
            DataSet dsAmigopodLookup = objCert.GetAmigoppodLookInfo(cert_id, Session["BRAND"].ToString());
            if (dsAmigopodLookup.Tables[0].Rows.Count > 0)
            {
                isBaseSku = Convert.ToBoolean(dsAmigopodLookup.Tables[0].Rows[0]["isBase_sku"]);
                partType = dsAmigopodLookup.Tables[0].Rows[0]["part_type"].ToString();
            }
            
            if (!isBaseSku)
            {
                //to check whether the sku is that of OnBoard, If yes, we need to generate a free sub key 
                //with one fifth of onboard count as guest count
                if (partType != type)
                {
                    lblErr.Text = "The certificate id is not compatible to generate Subscription ID.";
                    lblErr.Visible = true;
                    return;
                }
                else
                    isOnBoard = true;
            }
            if (dsAmigopodCert.Tables[0].Rows.Count > 0)
            {
                soId = dsAmigopodCert.Tables[0].Rows[0]["so_id"].ToString();
                poId = dsAmigopodCert.Tables[0].Rows[0]["cust_po_id"].ToString();
                if (!isOnBoard) //changed for OnBoard Sub key : 24/08/2012
                {
                    licenseCount = Convert.ToInt32(dsAmigopodLookup.Tables[0].Rows[0]["license_count"]);
                    category = dsAmigopodLookup.Tables[0].Rows[0]["category"].ToString();
                }
                else
                {
                    licenseCount = Convert.ToInt32(dsAmigopodLookup.Tables[0].Rows[0]["license_count"])/5;
                    category = ConfigurationManager.AppSettings["AMG_SW"].ToString();
                }

                company = objUser.GetUserCompanyId();
                email = objUser.GetUserEmail();
                Name = objUser.GetUserFirstName() + " " + objUser.GetUserLastName();

                DataSet dsCompany = objComp.GetCompanyInfo(company);
                if (dsCompany!=null && dsCompany.Tables[0].Rows.Count > 0)
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
                    objMail.sendAmigopodErrorMessage("Could not find company info of the user: " + email, "GenerateSubKey", cert_id, "");
                    return;
                }
                company_name = company_name + " " + soId;
                string incrementor = objCert.GetCompanyIncrementor(company_name);
                company_name = company_name + "_" + incrementor;

                //changed for OnBoard Sub key : 24/08/2012 
                //commented to chnage the logic of free sub key
                /*if (isOnBoard)
                {
                    soId = ConfigurationManager.AppSettings["AMG_SO_ID"].ToString();
                }*/

                objAmigopodXML = objCert.GenerateAmigopodSubscription(cert_id, category, soId, poId, expDate, licenseCount, Server.UrlEncode(company_name), email, do_guestconnect, strSKUId);
                subscriptionKey = objAmigopodXML.subscription_key;
            }
            if (objAmigopodXML.error != "0")
            {
                new Log().logDebug(meth, "Message : "+objAmigopodXML.message+" ;Error : " + objAmigopodXML.error + ";" + objAmigopodXML.xmlparse_error + ";" + objAmigopodXML.warning + " ; Sub Key: " + subscriptionKey);
                lblErr.Text = "Application could not process your request.The error occurred has been logged";

                //objMail.sendAmigopodErrorMessage("Message : " + objAmigopodXML.message + " ; Error : " + objAmigopodXML.error, "GenerateSubKey",cert_id,"");
                objMail.sendAmigopodErrorMessage("Message : " + objAmigopodXML.message+" <br /> "+" Error/Warning from WS : " + objAmigopodXML.error + " <br /> " + objAmigopodXML.xmlparse_error + "<br />" + objAmigopodXML.warning, "GenerateSubKey", cert_id, subscriptionKey);
                lblErr.Visible = true;
                return;
            }
            else if (objAmigopodXML.subscription_key != string.Empty && objAmigopodXML.error == "0")
            {
                //changed for OnBoard Sub key : 24/08/2012
                //generare certificate id for the newly generated sub key and insert into db
                if (isOnBoard)
                {
                    //cert_id is passing for the pupose of logging error/exception
                    isActivated = objCert.GenerateFreeSubkey(subscriptionKey, cert_id, ConfigurationManager.AppSettings["CLEARPASS_ACTIVATION"].ToString(), objUser.GetUserAcctId(), company, company_name, objUser.AcctId, expDate.ToString(),email,soId, poId);
                }
                else
                {
                    isActivated = objCert.InsertAmigopodActkey(subscriptionKey, cert_id, "ACTIVATION", objUser.GetUserAcctId(), company, company_name, "Generating Subscription ID", objUser.AcctId, expDate.ToString());
                }
                if (isActivated)
                {
                    //send mail
                    objAmigopodCertinfo.Subscriptionkey = subscriptionKey;
                    objAmigopodCertinfo.SoId = soId;
                    objAmigopodCertinfo.PoId = poId;
                    objAmigopodCertinfo.Name = Name;
                    objAmigopodCertinfo.Email = email;
                    objAmigopodCertinfo.ExpiryDate = expDate;
                    objAmigopodCertinfo.Brand = Session["BRAND"].ToString().ToUpper();

                    bool blSend = objMail.sendAmigopodSubscriptionInfo(objAmigopodCertinfo, category, objAmigopodXML.email, objAmigopodXML.password);
                    if (objAmigopodXML.message != "")
                    {
                        new Log().logInfo(meth, "Message : " + objAmigopodXML.message  + " ; Sub Key: " + subscriptionKey);
                    }
                    else if (objAmigopodXML.warning != "" || objAmigopodXML.error != "" || objAmigopodXML.xmlparse_error !="")
                    {
                        new Log().logDebug(meth, "Error : " + objAmigopodXML.error + ";" + objAmigopodXML.xmlparse_error + ";" + objAmigopodXML.warning + " ; Sub Key: " + subscriptionKey);
                        objMail.sendAmigopodErrorMessage("Error/Warning from WS : " + objAmigopodXML.error + " <br /> " + objAmigopodXML.xmlparse_error + "<br />" + objAmigopodXML.warning, "GenerateSubKey", cert_id, subscriptionKey);
                    }
                    ((Literal)wizActivate.FindControl("LiteralAct")).Text = subscriptionKey.Replace("\n", "<BR>");
                    if (isOnBoard)
                    {
                        //((Literal)wizActivate.FindControl("lblOnBoardMessage")).Text = "Please use the following Subscription ID to add OnBoard license count using  <b>Add OnBoard License</b> feature.";
                        ((Literal)wizActivate.FindControl("lblOnBoardMessage")).Visible = true;
                    }
                    else
                    {
                        ((Literal)wizActivate.FindControl("lblOnBoardMessage")).Visible = false;
                    }
                    wizActivate.ActiveStepIndex = 1;
                }
                else
                {
                    new Log().logInfo(meth, "Unable to insert the activation key into db." + subscriptionKey);
                    lblErr.Text = "Application could not process your request.The error occurred has been logged";
                    lblErr.Visible = true;
                    objMail.sendAmigopodErrorMessage("Unable to insert the activation key into db." , "GenerateSubKey", cert_id,subscriptionKey);
                }
            }
            else
            {
                new Log().logDebug(meth, "Message : " + objAmigopodXML.message + " ;Error : " + objAmigopodXML.error + ";" + objAmigopodXML.xmlparse_error + ";" + objAmigopodXML.warning + " ; Sub Key: " + subscriptionKey);
                lblErr.Text = "Application could not process your request.The error occurred has been logged";
                lblErr.Visible = true;
                objMail.sendAmigopodErrorMessage("Message : " + objAmigopodXML.message + " <br /> " + " Error/Warning from WS : " + objAmigopodXML.error + " <br /> " + objAmigopodXML.xmlparse_error + "<br />" + objAmigopodXML.warning, "GenerateSubKey", cert_id, subscriptionKey);
            }
        }
        catch(Exception ex)
        {
            new Log().logException(meth, ex);
            lblErr.Text = "Application could not process your request.The error occurred has been logged";
            lblErr.Visible = true;
            objMail.sendAmigopodErrorMessage(ex.Message, "GenerateSubKey",cert_id,"");
        }
    }
}
