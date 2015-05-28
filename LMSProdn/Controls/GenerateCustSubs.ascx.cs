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

public partial class Controls_GenerateCustSubs : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserInfo objUser = new UserInfo();
        objUser = (UserInfo)Session["USER_INFO"];
        LblError.Visible = false;
        TxtEmail.Text = objUser.GetUserEmail();        
    }

    private void setError(Label LblError, string strError)
    {
        LblError.Text = strError;
        LblError.Visible = true;
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;

        Certificate objCert = new Certificate();
        UserInfo objUser = new UserInfo();
        objUser = (UserInfo)Session["USER_INFO"];
        //AmigopodCertInfo objAmigopodCertinfo = new AmigopodCertInfo();
        Email objMail = new Email();
        Company objComp = new Company();
        //AmigopodXML objAmigopodXML = new AmigopodXML();
        //string strCertId = string.Empty; string strCompany = string.Empty;
        //strCertId = TxtCertId.Text.Trim();
        //strCompany = TxtCustName.Text.Trim();
        string strActCompany = string.Empty;
        AvendaCert objAvendaCert = new AvendaCert();
        AvendaXML objAvendaXML = new AvendaXML();
        bool isBaseSku = false;
        string partType = string.Empty; string category = string.Empty;
        DateTime expDate; string subscriptionKey = string.Empty;
        string meth = string.Empty;
        bool isActivated = false;
        string strLicenseCount = string.Empty;
        string strLicenseType = string.Empty;
        string strSKUId = ConfigurationManager.AppSettings["AVENDA_SKU_ID"].ToString();
        string do_guestconnect = "1";
        string do_policymgr = "1";
        string IsLegacy = string.Empty;
        try
        {
            //Validation begins.

            //Get the cert details from certGen db
            DataSet dsAmg = objCert.getClpCertFromCertGen(TxtCertId.Text.Trim(), Session["BRAND"].ToString());
            if (dsAmg.Tables[0].Rows.Count > 0)
                expDate = Convert.ToDateTime(dsAmg.Tables[0].Rows[0]["expiration_date"].ToString());
            else
            {
                new Log().logInfo(meth, "Certificate could not found in certgen db: " + TxtCertId.Text.Trim());
                setError(LblError, "This Certificate does not exist.");
                objMail.sendAmigopodErrorMessage("Certificate could not found in Certificates db: " + TxtCertId.Text.Trim(), "GenerateSubKey", TxtCertId.Text.Trim(), "");
                return;
            }

            // Check whether Certificates exists in LMS or not.
            DataSet dsClsCert = objCert.GetClearPassCertDet(TxtCertId.Text.Trim(), Session["BRAND"].ToString());

            if (dsClsCert.Tables.Count > 0)
            {
                if (dsClsCert.Tables[0].Rows.Count <= 0)
                {
                    setError(LblError, "This Certificate does not exist!.");
                    return;
                }
            }
            else
            {
                setError(LblError, "This Certificate does not exists.");
                return;
            }

            // Check to see whether the certificate is of base certificate.
            DataSet dsAmigopodLookup = objCert.GetAmigoppodLookInfo(TxtCertId.Text.Trim(), Session["BRAND"].ToString());
            if (dsAmigopodLookup.Tables.Count > 0)
            {
                if (dsAmigopodLookup.Tables[0].Rows.Count > 0)
                {
                    isBaseSku = Convert.ToBoolean(dsAmigopodLookup.Tables[0].Rows[0]["isBase_sku"]);
                    partType = dsAmigopodLookup.Tables[0].Rows[0]["part_type"].ToString();
                    category = dsAmigopodLookup.Tables[0].Rows[0]["category"].ToString();
                    strLicenseCount = dsAmigopodLookup.Tables[0].Rows[0]["license_count"].ToString();
                    strLicenseType = dsAmigopodLookup.Tables[0].Rows[0]["license_type"].ToString();
                    IsLegacy = dsAmigopodLookup.Tables[0].Rows[0]["IsLegacy"].ToString(); 
                }
            }

            if (!isBaseSku)
            {
                setError(LblError, "This Certificate is not of ClearPass appliances.");
                return;
            }

            if (IsLegacy == "true")
            {
                setError(LblError, "This Certificate ID is of legacy one.");
                return;
            }

            // Check if Certificate is used to generate subscription ID for this company.
            DataSet dsActivated = objCert.IsClsCertActivated(TxtCertId.Text.Trim());
            if (dsActivated != null)
            {
                if (dsActivated.Tables.Count > 0)
                {
                    if (dsActivated.Tables[0].Rows.Count > 0)
                    {
                        strActCompany = dsActivated.Tables[0].Rows[0]["customer_name"].ToString();
                        if (strActCompany != string.Empty)
                        {
                            setError(LblError, "This Certificate is already used to generate subscription for " + strActCompany + ".");
                            return;
                        }
                        setError(LblError, "This Certificate is already used to generate subscription.");
                        return;
                    }
                }
            }

            // Validation done.

            // Generate subscription ID.
            objAvendaCert.so = dsClsCert.Tables[0].Rows[0]["so_id"].ToString();
            objAvendaCert.po = dsClsCert.Tables[0].Rows[0]["cust_po_id"].ToString();

            //objAvendaCert.CompanyId = objUser.GetUserCompanyId();
            if (TxtEmail.Text == string.Empty)
            {
                objAvendaCert.email = objUser.GetUserEmail();
            }
            else
            {
                objAvendaCert.email = TxtEmail.Text.Trim();
            }
            objAvendaCert.name = objUser.GetUserFirstName() + " " + objUser.GetUserLastName();
            objAvendaCert.CompanyName = Server.HtmlEncode(TxtCustName.Text.Trim());

            //DataSet dsCompany = objComp.GetCompanyInfo(company);
            //if (dsCompany != null && dsCompany.Tables[0].Rows.Count > 0)
            //{
            //    objAvendaCert.CompanyName = dsCompany.Tables[0].Rows[0]["company_name"].ToString();
            //}
            //else
            //{
            //    //company info is not available in company table.
            //    //Getting the comapny name from accounts
            //    objAvendaCert.CompanyName = objCert.GetCompanyInfoByEmail(email);
            //}
            //if (company_name.Equals(string.Empty))
            //{
            //    //if company name is not set in accounts also then throw error
            //    new Log().logInfo(meth, "Error : Could not find company info of the user: " + email);
            //    lblErr.Text = "Application could not process your request.The error occurred has been logged";
            //    lblErr.Visible = true;
            //    objMail.sendAmigopodErrorMessage("Could not find company info of the user: " + email, "GenerateCustSubKey", cert_id, "");
            //    return;
            //}

            objAvendaXML = objCert.GenerateClsSubscription(TxtCertId.Text.Trim(), category, objAvendaCert.so, objAvendaCert.po, expDate, Int32.Parse("0"), objAvendaCert.CompanyName, objAvendaCert.email, strSKUId, do_guestconnect);
            subscriptionKey = objAvendaXML.subscription_key;

            if (objAvendaXML.error != "0")
            {
                if (objAvendaXML.error.Contains("Cannot update customer") || objAvendaXML.error.Contains("name already in use."))
                {
                    new Log().logDebug(meth, "Message : " + objAvendaXML.message + " ;Error : " + objAvendaXML.error + ";" + objAvendaXML.xmlparse_error + ";" + objAvendaXML.warning + " ; Sub Key: " + subscriptionKey);
                    setError(LblError, "Cannot generate the Subscription ID: Customer name already in use.");

                    objMail.sendAmigopodErrorMessage("Message : " + objAvendaXML.message + " <br /> " + " Error/Warning from WS : " + objAvendaXML.error + " <br /> " + objAvendaXML.xmlparse_error + "<br />" + objAvendaXML.warning, "GenerateSubKey", TxtCertId.Text.Trim(), subscriptionKey);
                    return;
                }
                else
                {
                    new Log().logDebug(meth, "Message : " + objAvendaXML.message + " ;Error : " + objAvendaXML.error + ";" + objAvendaXML.xmlparse_error + ";" + objAvendaXML.warning + " ; Sub Key: " + subscriptionKey);
                    setError(LblError, "Application could not process your request.The error occurred has been logged");

                    objMail.sendAmigopodErrorMessage("Message : " + objAvendaXML.message + " <br /> " + " Error/Warning from WS : " + objAvendaXML.error + " <br /> " + objAvendaXML.xmlparse_error + "<br />" + objAvendaXML.warning, "GenerateSubKey", TxtCertId.Text.Trim(), subscriptionKey);
                    return;
                }
            }
            else if (objAvendaXML.subscription_key != string.Empty && objAvendaXML.error == "0")
            {
                AvendaXML objAvendaPolicyXML = new AvendaXML();
                objAvendaPolicyXML = objCert.GeneratePolicyMgrLicense(objAvendaXML.subscription_key, do_policymgr, strLicenseCount, strLicenseType, DrpListVersion.SelectedValue.ToString(), TxtCertId.Text.Trim());
                if (objAvendaPolicyXML.license != string.Empty && objAvendaPolicyXML.error == "0")
                {
                    //generare certificate id for the newly generated sub key and insert into db
                    isActivated = objCert.InsertClsSubcription(subscriptionKey, TxtCertId.Text.Trim(), objUser.AcctId, objAvendaCert.CompanyName, objUser.GetUserAcctId(), objAvendaCert.email, expDate.ToString(), objAvendaPolicyXML.license, DrpListVersion.SelectedValue.ToString());

                    if (isActivated)
                    {
                        //send mail
                        objAvendaCert.subscription = subscriptionKey;
                        objAvendaCert.expiryDate = expDate.ToString();
                        objAvendaCert.brand = Session["BRAND"].ToString().ToUpper();
                        objAvendaCert.username = objAvendaXML.user_name;
                        objAvendaCert.password = objAvendaXML.password;
                        if (TxtEmail.Text == string.Empty)
                            objAvendaCert.name = objUser.FirstName + " " + objUser.LastName;
                        else
                            objAvendaCert.name = TxtCustName.Text + "(" + TxtEmail.Text + ")";
                        objAvendaCert.category = category;
                        objAvendaCert.license = objAvendaPolicyXML.license;
                        objAvendaCert.version = DrpListVersion.SelectedValue.ToString();
                        //objAvendaCert.email = TxtEmail.Text.Trim();

                        bool blSend = objMail.sendClsSubscriptionInfo(objAvendaCert, Session["BRAND"].ToString());
                        if (blSend == false)
                        {
                            LblInfo.ForeColor = System.Drawing.Color.Red;
                            LblInfo.Text = "System failed to send the mail to " + TxtEmail.Text.Trim() + ". Please contact the Support for further help.";
                        }
                        if (objAvendaXML.message != "")
                        {
                            new Log().logInfo(meth, "Message : " + objAvendaXML.message + " ; Sub Key: " + subscriptionKey);
                        }
                        else if (objAvendaXML.warning != "" || objAvendaXML.error != "" || objAvendaXML.xmlparse_error != "")
                        {
                            new Log().logDebug(meth, "Error : " + objAvendaXML.error + ";" + objAvendaXML.xmlparse_error + ";" + objAvendaXML.warning + " ; Sub Key: " + subscriptionKey);
                            objMail.sendAmigopodErrorMessage("Error/Warning from WS : " + objAvendaXML.error + " <br /> " + objAvendaXML.xmlparse_error + "<br />" + objAvendaXML.warning, "GenerateSubKey", TxtCertId.Text, subscriptionKey);
                        }
                        ((Literal)wizGenerate.FindControl("LiteralAct")).Text = subscriptionKey.Replace("\n", "<BR>");
                        ((Literal)wizGenerate.FindControl("LiteralLickey")).Text = objAvendaPolicyXML.license;                       
                        wizGenerate.ActiveStepIndex = 1;
                    }
                    else
                    {
                        new Log().logInfo(meth, "Unable to insert the activation key into db." + subscriptionKey);
                        setError(LblError, "Application could not process your request.The error occurred has been logged"); objMail.sendAmigopodErrorMessage("Unable to insert the activation key into db.", "GenerateCustSubKey", TxtCertId.Text, subscriptionKey);
                    }
                }
                else
                {
                    new Log().logDebug(meth, "Message : " + objAvendaPolicyXML.message + " ;Error : " + objAvendaPolicyXML.error + ";" + objAvendaPolicyXML.xmlparse_error + ";" + objAvendaPolicyXML.warning + " ; Sub Key: " + subscriptionKey);
                    setError(LblError, "Application could not process your request.The error occurred has been logged"); objMail.sendAmigopodErrorMessage("Message : " + objAvendaPolicyXML.message + " <br /> " + " Error/Warning from WS : " + objAvendaPolicyXML.error + " <br /> " + objAvendaPolicyXML.xmlparse_error + "<br />" + objAvendaPolicyXML.warning, "GenerateCustSubKey", TxtCertId.Text, subscriptionKey);
                }
            }
            else
            {
                new Log().logDebug(meth, "Message : " + objAvendaXML.message + " ;Error : " + objAvendaXML.error + ";" + objAvendaXML.xmlparse_error + ";" + objAvendaXML.warning + " ; Sub Key: " + subscriptionKey);
                setError(LblError, "Application could not process your request.The error occurred has been logged"); objMail.sendAmigopodErrorMessage("Message : " + objAvendaXML.message + " <br /> " + " Error/Warning from WS : " + objAvendaXML.error + " <br /> " + objAvendaXML.xmlparse_error + "<br />" + objAvendaXML.warning, "GenerateCustSubKey", TxtCertId.Text, subscriptionKey);
            }
        }
        catch (Exception ex)
        {
            setError(LblError, "Application could not process the request. The Error has been logged.");
        }
    }
    protected void CustmValidateEmail_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string strResult = IsValidEmail(TxtEmail.Text.Trim());
        if (strResult != string.Empty)
        {
            args.IsValid = false;
            CustmValidateEmail.ErrorMessage = "Invalid Email format " + strResult;
            return;
        }
        args.IsValid = true;
    }

    private string IsValidEmail(string strEmail)
    {        
        //string strInvEmail = string.Empty;
        //string[] strEmailComp = strEmail.Split(',');
        //if (strEmailComp.Length > 0)
        //{
        //    for (int i = 0; i < strEmailComp.Length; i++)
        //    {
        //        // Return true if strIn is in valid e-mail format.
        //        if (Regex.IsMatch(strEmailComp[i].ToString(), @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") == false)
        //        {
        //            strInvEmail = strInvEmail + " ," + strEmailComp[i].ToString();
        //        }
        //    }
        //    return strInvEmail;
        //}
        //else
        //{
        //    if (Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") == false)
        //    {
        //        return strEmail;
        //    }
        //    else
        //    {
        //        return string.Empty;
        //    }
        //}

        if (Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") == false)
        {
            return strEmail;
        }
        else
        {
            return string.Empty;
        }
    }
}
