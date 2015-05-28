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

public partial class Controls_CreateAirwaveEvalCert : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect(ConfigurationManager.AppSettings["NEWCREATEAWEVAL_URL"], true);        
    }
}

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    Response.Redirect(ConfigurationManager.AppSettings["NEWCREATEAWEVAL_URL"], true);
        //lblErr.Visible = false;
        //if (!IsPostBack)
        //{
        //    //load Dept list
        //    Certificate objCert = new Certificate();
        //    DataSet dsEvalparts = objCert.LoadAirwaveEvalParts(Session["BRAND"].ToString());
        //    UIHelper.PrepareAndBindListWithoutPlease(ddlEvalPart, dsEvalparts.Tables[0], "MIX", "VAL", true);
        //}
   // }
    //protected void btnCreate_OnClick(object sender, EventArgs e)
    //{
    //    if (!Page.IsValid)
    //        return;
    //    bool retVal;
    //    retVal = false;
    //    bool blRestrict = false;
    //    Certificate objCert = new Certificate();
    //    AirwaveKeyProcessor objAirwvKeyProcess = new AirwaveKeyProcessor();
    //    AirwaveCertInfo objAirwaveCertInfo = new AirwaveCertInfo();
    //    User objUser = new User();
    //    string strAirwvEval = string.Empty;
    //    string strOrg = string.Empty;
    //    string strExpiry = string.Empty;
    //    string strERROR = string.Empty;
    //    string strPassword = string.Empty;
    //    string strFName = string.Empty;
    //    string strLname = string.Empty;        
           
    //    if (!((UserInfo)Session["USER_INFO"]).GetImpersonateUserRole().Contains(ConfigurationManager.AppSettings["ARUBA_ROLE"].ToString()))
    //    {
    //        blRestrict = RestrictGenerateAirwEvalCert(ddlEvalPart.SelectedValue,txtEmail.Text,Session["BRAND"].ToString());
    //    }

    //    if (blRestrict == false)
    //    {
    //        strOrg = Server.UrlEncode(TxtOrg.Text);
    //        strAirwvEval = objCert.GenerateAirwEvalCert(ddlEvalPart.SelectedValue.ToString(), strOrg);
    //        if (strAirwvEval.Contains("Error"))
    //        {
    //            setError(lblErr, Certificate.FAILURE_EVALCERT);
    //            return;
    //        }
    //        else
    //        {
    //            strERROR = objUser.IsEmailExists(txtEmail.Text, true);
    //            if (strERROR == string.Empty)
    //            {
    //                if (!objUser.IsRestrictedDomain(txtEmail.Text))
    //                {
    //                    if ((txtEmail.Text.ToLower().Contains("@arubanetworks.com")))
    //                    {
    //                        if (!objUser.isArubaEmp(txtEmail.Text))
    //                        {
    //                            strPassword = Membership.GeneratePassword(15, 5);
    //                        }
    //                        //else if (!objUser.IsWindowsLoginIDSSO(txtEmail.Text, Session["APPID"].ToString()))
    //                        //{
    //                        //    strPassword = Membership.GeneratePassword(15, 5);
    //                        //}
    //                    }
    //                    else
    //                    {
    //                        strPassword = Membership.GeneratePassword(15, 5);
    //                    }

    //                    UserInfo objLicUser = new UserInfo();
    //                    int len = txtSoldTo.Text.IndexOf(" ");
    //                    if (len <= 0)
    //                    {
    //                        strFName = txtSoldTo.Text.Trim();
    //                        strLname = txtSoldTo.Text.Trim();
    //                    }
    //                    else
    //                    {
    //                        int strLen = txtSoldTo.Text.Length - len;
    //                        strFName = txtSoldTo.Text.Substring(0, len);
    //                        strLname = txtSoldTo.Text.Substring(len, strLen);
    //                    }
    //                    objLicUser.FirstName = strFName.Trim();
    //                    objLicUser.LastName = strLname.Trim();
    //                    objLicUser.Email = txtEmail.Text.Trim();
    //                    objLicUser.CompanyName = TxtOrg.Text.Trim();
    //                    objLicUser.Role = "customer";
    //                    objLicUser.Status = "ACTIVE";
    //                    objLicUser.Comments = "AIRW";
    //                    objLicUser.Phone = string.Empty;
    //                    objLicUser.Brand = Session["BRAND"].ToString();
    //                    objLicUser.Password = strPassword;
    //                    objLicUser.IPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString();
    //                    objLicUser.CompanyId = -1;

    //                    if (!objUser.AddUser(objLicUser))
    //                    {
    //                        strPassword = string.Empty;
    //                    }
    //                }
    //            }

    //            objAirwvKeyProcess.TheKey = strAirwvEval;
    //            bool blInsert = objCert.AddAirwvEvalCertificate(ddlEvalPart.SelectedValue, strAirwvEval, txtEmail.Text, TxtOrg.Text, Session["BRAND"].ToString(), ConfigurationManager.AppSettings["CERTTYPE"].ToString());
    //            //send email notification to email address of recipient
    //            // Added by Ashwini            
    //            objAirwaveCertInfo.Brand = (Session["BRAND"].ToString().ToUpper());
    //            objAirwaveCertInfo.CertId = strAirwvEval;
    //            objAirwaveCertInfo.PackageDesc = ddlEvalPart.SelectedItem.ToString();
    //            objAirwaveCertInfo.Package = ddlEvalPart.SelectedValue;
    //            objAirwaveCertInfo.Email = txtEmail.Text;
    //            objAirwaveCertInfo.Name = txtSoldTo.Text;
    //            strExpiry = objAirwaveCertInfo.ExpiryDate;
    //            objAirwaveCertInfo.Password = strPassword;

    //            Email objEmail = new Email();
    //            string strCCEmail = ((UserInfo)Session["USER_INFO"]).GetUserEmail();
    //            retVal = objEmail.sendAirwvEvalCertInfo(objAirwaveCertInfo, strCCEmail);
    //            if (retVal == true)
    //            {
    //                // LblMail.Text = "email is sent to " + objcgInfo.Email;
    //                ((Literal)wizAirwave.FindControl("LiteralCert")).Text = strAirwvEval.Replace("\n", "<BR>");

    //                wizAirwave.ActiveStepIndex = 1;
    //            }
    //            else
    //            {
    //                // LblMail.Text = "email is sent to " + objcgInfo.Email;
    //                ((Literal)wizAirwave.FindControl("LiteralCert")).Text = strAirwvEval.Replace("\n", "<BR>");
    //                wizAirwave.ActiveStepIndex = 1;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        setError(lblErr, Certificate.MAX_AIRW_EVAL_COUNT);
    //    }

    //}
    //protected void cvEmail_OnValidate(object source, ServerValidateEventArgs args)
    //{

    //}

    //private void setError(Label lbl, string text)
    //{
    //    lbl.Text = text;
    //    lbl.Visible = true;
    //}

    //private bool RestrictGenerateAirwEvalCert(string strPartNo, string strEmail, string strBrand)
    //{
    //   Certificate objCert = new Certificate();
    //   return objCert.RestrictGenerateAirwEvalCert(strPartNo, strEmail, strBrand);
    //}
