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
using System.Text.RegularExpressions;

public partial class Controls_CreateClpEval : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LblError.Visible = false;
        UserInfo objUser = new UserInfo();
        objUser = (UserInfo)Session["USER_INFO"];
        TxtEmail.Text = objUser.GetUserEmail();
        TxtName.Text = objUser.GetUserFirstName() + " " + objUser.GetUserLastName();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;

        Certificate objCert = new Certificate();
        UserInfo objUser = new UserInfo();
        objUser = (UserInfo)Session["USER_INFO"];
        AmigopodCertInfo objAmigopodCertinfo = new AmigopodCertInfo();
        Email objMail = new Email();
        Company objComp = new Company();
        AmigopodXML objAmigopodXML = new AmigopodXML();
        string subscriptionKey = string.Empty;
        string meth = string.Empty;
        string do_guestconnect = "1";
        string do_policy_manager = "1";
        string do_enterprise = "1";
        string category = ConfigurationManager.AppSettings["CLP_EVAL_CAT"].ToString();
        string disabled = ConfigurationManager.AppSettings["CLP_EVAL_DAYS"].ToString();
        string policy_manager_licenseType = ConfigurationManager.AppSettings["CLP_LIC_TYPE"].ToString();
        string enterprise_licenseType = ConfigurationManager.AppSettings["CLP_LIC_TYPE"].ToString();
        string strPolicy = string.Empty;
        string strEnterprise = string.Empty;
        string strUserId = string.Empty;
        string strPassword = string.Empty;
        string expDate;
        try
        {
            ////Validation begins.            
            //// Check whether eval license is already generated in LMS or not.      
            if (objCert.IsClpEvalGenerated(TxtCompany.Text))
            {
                setError(LblError, "The evaluation license is already generated for the company : " + TxtCompany.Text.Trim());
                return;
            }

            if (!((UserInfo)Session["USER_INFO"]).GetImpersonateUserRole().Contains(ConfigurationManager.AppSettings["ARUBA_ROLE"].ToString()))
            {
                //Restrict the user not to generate the evaluation license for the same user
                if (objCert.RestrictGenerateClpEvalCert(TxtCompany.Text.Trim(), TxtEmail.Text.Trim(), Session["BRAND"].ToString()))
                {
                    setError(LblError, "You have exceeded the maximum number of times you can generate Eval license. Please contact Aruba support team for further assistance.");
                    return;
                }                
            }

            //// Validation done.

            string strCompany = TxtCompany.Text.Trim().Replace("&", "%26");
            strCompany = TxtCompany.Text.Trim().Replace(" ", "%20");
            //string strCompany = Server.UrlEncode(TxtCompany.Text.Trim());
            //return;
            objAmigopodXML = objCert.GenerateClsEval(strCompany, category, TxtEmail.Text.Trim(), do_enterprise, do_policy_manager, do_guestconnect, disabled, policy_manager_licenseType, enterprise_licenseType);
            subscriptionKey = objAmigopodXML.subscription_key;
            strPolicy = objAmigopodXML.policy_manager;
            strEnterprise = objAmigopodXML.enterprise;
            strUserId = objAmigopodXML.user_name;
            strPassword = objAmigopodXML.password;
            expDate = objAmigopodXML.expiry_time;

            if (objAmigopodXML.error != "0")
            {
                if (objAmigopodXML.error.Contains("Cannot update customer") || objAmigopodXML.error.Contains("name already in use."))
                {
                    new Log().logDebug(meth, "Message : " + objAmigopodXML.message + " ;Error : " + objAmigopodXML.error + ";" + objAmigopodXML.xmlparse_error + ";" + objAmigopodXML.warning + " ; Sub Key: " + subscriptionKey);
                    setError(LblError, "Cannot generate the Subscription ID: Customer name already in use.");

                    objMail.sendAmigopodErrorMessage("Message : " + objAmigopodXML.message + " <br /> " + " Error/Warning from WS : " + objAmigopodXML.error + " <br /> " + objAmigopodXML.xmlparse_error + "<br />" + objAmigopodXML.warning, "GenerateSubKey", TxtCompany.Text.Trim(), subscriptionKey);
                    return;
                }
                else
                {
                    new Log().logDebug(meth, "Message : " + objAmigopodXML.message + " ;Error : " + objAmigopodXML.error + ";" + objAmigopodXML.xmlparse_error + ";" + objAmigopodXML.warning + " ; Sub Key: " + subscriptionKey);
                    setError(LblError, "Application could not process your request.The error occurred has been logged");

                    objMail.sendAmigopodErrorMessage("Message : " + objAmigopodXML.message + " <br /> " + " Error/Warning from WS : " + objAmigopodXML.error + " <br /> " + objAmigopodXML.xmlparse_error + "<br />" + objAmigopodXML.warning, "GenerateSubKey", TxtCompany.Text.Trim(), subscriptionKey);
                    return;
                }
            }
            else if (objAmigopodXML.subscription_key != string.Empty && objAmigopodXML.error == "0")
            {
                //Insert details to Db.
                bool isActivated = objCert.InsertClpEvalInfo(subscriptionKey, objUser.AcctId, objUser.GetUserAcctId(), TxtCompany.Text.Trim(), TxtEmail.Text.Trim(), Convert.ToDouble(expDate), strPolicy, strEnterprise, strUserId, strPassword, Session["BRAND"].ToString());

                if (isActivated)
                {
                    //send mail
                    objAmigopodCertinfo.Subscriptionkey = subscriptionKey;
                    objAmigopodCertinfo.ExpiryDate = UIHelper.UnixTimeStampToDateTime(Convert.ToDouble(expDate));
                    objAmigopodCertinfo.Brand = Session["BRAND"].ToString().ToUpper();
                    objAmigopodCertinfo.UserName = objAmigopodXML.user_name;
                    objAmigopodCertinfo.Password = objAmigopodXML.password;
                    objAmigopodCertinfo.Name = TxtName.Text.Trim() + " (" + TxtEmail.Text + ")";
                    objAmigopodCertinfo.Email = TxtEmail.Text.Trim();
                    objAmigopodCertinfo.PolicyLic = objAmigopodXML.policy_manager;
                    objAmigopodCertinfo.EnterpriseLic = objAmigopodXML.enterprise;
                    objAmigopodCertinfo.CompanyName = TxtCompany.Text.Trim();

                    bool blSend = objMail.sendClsEvalKey(objAmigopodCertinfo, Session["BRAND"].ToString().ToUpper());
                    if (blSend == false)
                    {
                        LblInfo.ForeColor = System.Drawing.Color.Red;
                        LblInfo.Text = "System failed to send the mail to " + TxtEmail.Text.Trim() + ". Please contact the Support for further help.";
                    }
                    ((Literal)WizGenerate.FindControl("LiteralSubKey")).Text = objAmigopodXML.subscription_key;
                    ((Literal)WizGenerate.FindControl("LiteralPolicyLic")).Text = objAmigopodXML.policy_manager;
                    ((Literal)WizGenerate.FindControl("LiteralEnterLic")).Text = objAmigopodXML.enterprise;
                    WizGenerate.ActiveStepIndex = 1;
                }
                else
                {
                    new Log().logInfo(meth, "Unable to insert the eval activation key into db." + TxtCompany.Text);
                    setError(LblError, "Application could not process your request.The error occurred has been logged"); objMail.sendAmigopodErrorMessage("Unable to insert the eval activation key into db.", "GenerateClpEval", TxtCompany.Text, string.Empty);
                }
            }
            else
            {
                new Log().logDebug(meth, "Message : " + objAmigopodXML.message + " ;Error : " + objAmigopodXML.error + ";" + objAmigopodXML.xmlparse_error + ";" + objAmigopodXML.warning + " ; Company: " + TxtCompany.Text);
                setError(LblError, "Application could not process your request.The error occurred has been logged"); objMail.sendAmigopodErrorMessage("Message : " + objAmigopodXML.message + " <br /> " + " Error/Warning from WS : " + objAmigopodXML.error + " <br /> " + objAmigopodXML.xmlparse_error + "<br />" + objAmigopodXML.warning, "GenerateClpEval", TxtCompany.Text, subscriptionKey);
            }
        }
        catch (Exception ex)
        {
            new Log().logDebug(meth, ex.ToString());
            objMail.sendAmigopodErrorMessage(ex.ToString(), "GenerateClpEval", TxtCompany.Text, subscriptionKey);
            setError(LblError, "Application could not process your request.The error occurred has been logged");
        }
    }

    private string IsValidEmail(string strEmail)
    {
        if (Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$") == false)
        {
            return strEmail;
        }
        else
        {
            return string.Empty;
        }
    }

    protected void CustmValidateEmail_OnValidate(object source, ServerValidateEventArgs args)
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

    private void setError(Label LblError, string strError)
    {
        LblError.Text = strError;
        LblError.Visible = true;
    }
}
