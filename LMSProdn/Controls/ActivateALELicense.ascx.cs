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
using Com.Arubanetworks.Licensing.Lib.Utils;
using Com.Arubanetworks.Licensing.Lib.Data;
using Com.Arubanetworks.Licensing.Dataaccesslayer;
using System.Net;
using System.Text.RegularExpressions;


public partial class Controls_ActivateALELicense : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false;
    }
    
    protected void lnkEULA_Click(object sender, EventArgs e)
    {
        RdBtnLst2.SelectedValue = "yes";
        //Response.Redirect("EULA.pdf", true);
        ScriptManager.RegisterStartupScript(this, sender.GetType(), "HTML", "window.open('EULA_aruba_Airwave.aspx','NewWindow','height=900,width=900,top=50,left=150,status=yes,toolbar=no, menubar=no,location=no,resizable=yes,scrollbars=no')", true);
    }
    protected void btnALEActivate_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;

        bool blAddActkey = false;
        string meth = "btnALEActivate_Click";
        string strActivationKey = string.Empty;
        string strOrg = string.Empty;
        Certificate objCert = new Certificate();
        string strBrand = string.Empty;
        string strSerialNo = string.Empty;
        string strPartId = string.Empty;
        string strPartDesc = string.Empty;
        string strPackage = string.Empty;
        int APCount = 0;
        string strQty = string.Empty;
        Email objMail = new Email();
        string strOrderId = string.Empty; string strProduct = string.Empty; string strProductDesc = string.Empty;
        ALECertInfo objALECertInfo = new ALECertInfo();
        UserInfo objUser = new UserInfo();
        objUser = (UserInfo)Session["USER_INFO"];
        
        try
        {
            //Check for Certificate
            DataSet dsALECert = objCert.getALECertDet(TxtEvalKey.Text, Session["BRAND"].ToString());

            if (dsALECert.Tables[0].Rows.Count <= 0)
            {
                lblErr.Text = "This Certificate does not exist.";
                lblErr.Visible = true;
                return;
            }

            //Check for Activation
            if (objCert.IsALECertActivated(TxtEvalKey.Text.Trim()))
            {
                lblErr.Text = "This Certificate is already activated";
                lblErr.Visible = true;
                return;
            }

            //if (isIP(txtSNum.Text.Trim()))
            //{
            //    lblErr.Text = "This IP address is invalid";
            //    lblErr.Visible = true;
            //    return;
            //}

            //if (!IsAddressValid(txtSNum.Text.Trim()))
            //{
            //    lblErr.Text = "This IP address is invalid";
            //    lblErr.Visible = true;
            //    return;
            //}

            if (!isValidIPAddress(txtSNum.Text.Trim()))
            {
                lblErr.Text = "This IP address is invalid";
                lblErr.Visible = true;
                return;
            }

            DataSet dsALECertGen = objCert.getALECertGen(TxtEvalKey.Text);
            if (dsALECert.Tables[0].Rows.Count > 0)
            {
                strOrderId = dsALECertGen.Tables[0].Rows[0]["SoId"].ToString();
                strPartId = dsALECertGen.Tables[0].Rows[0]["PartNumber"].ToString();
                strPartDesc = dsALECertGen.Tables[0].Rows[0]["PartDesc"].ToString();
                strBrand = dsALECertGen.Tables[0].Rows[0]["Brand"].ToString();
                strSerialNo = dsALECertGen.Tables[0].Rows[0]["SerialNumber"].ToString();
                strProduct = dsALECertGen.Tables[0].Rows[0]["Product"].ToString();
                strPackage = dsALECertGen.Tables[0].Rows[0]["Package"].ToString();
                strQty = dsALECertGen.Tables[0].Rows[0]["Qty"].ToString();
                  
                    if (Int32.Parse(strQty) >= 0)
                    {
                        APCount = Int32.Parse(strQty);
                    }
                    else
                    {
                        APCount = 0;
                        lblErr.Text = "This Certificate does not exist.";
                        lblErr.Visible = true;
                        return;
                    }
                }
            else
            {
                lblErr.Text = "This Certificate does not exist.";
                lblErr.Visible = true;
                return;
            }

            if (strBrand == Session["BRAND"].ToString())
            {
                strOrg = Server.UrlEncode(txtSoId.Text);
                strActivationKey = objCert.GenerateALEActivation(strOrderId, txtSNum.Text, strPartId, strOrg, APCount, strSerialNo, strProduct, strPackage);
            }
            else
            {
                lblErr.Text = "Invalid Certificate";
                lblErr.Visible = true;
                return;
            }
            
            if (strActivationKey.Contains("Error"))
            {
                new Log().logInfo("btnALEActivate_Click", strActivationKey);
                lblErr.Text = "Unable to generate License key. Please contact Support for further help.";
                lblErr.Visible = true;
                return;
            }
            else
            {
                blAddActkey = objCert.InsertALEActkey(strActivationKey, txtSNum.Text, TxtEvalKey.Text, txtSoId.Text, objUser.GetUserAcctId(),objUser.AcctId);
                if (blAddActkey == true)
                {
                    //send mail
                    objALECertInfo.Activationkey = strActivationKey;
                    objALECertInfo.Brand = Session["BRAND"].ToString().ToUpper();
                    objALECertInfo.CertId = TxtEvalKey.Text;
                    objALECertInfo.Email = objUser.GetUserEmail();
                    objALECertInfo.IPAddress = txtSNum.Text;
                    objALECertInfo.Name = objUser.FirstName + " " + objUser.LastName;
                    objALECertInfo.Package = strPartId;
                    objALECertInfo.PackageDesc = strPartDesc;
                    bool blSend = objMail.sendALEActivationInfo(objALECertInfo);                    
                    ((Literal)wizActivate.FindControl("LiteralAct")).Text = strActivationKey.Replace("\n", "<BR>");
                     wizActivate.ActiveStepIndex = 1;
                }
                else
                {
                    new Log().logInfo("btnALEActivate_Click", "Unable to insert the activation key.");
                    lblErr.Text = "Application could not process your request. The error has been logged.";
                    lblErr.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            new Log().logException(meth, ex);
            lblErr.Text = "Application could not process your request. The error has been logged.";
            lblErr.Visible = true;
        }
    }

    public bool IsAddressValid(string addrString)
    {
        IPAddress address;
        return IPAddress.TryParse(addrString, out address);
    }

    public bool isIP(string ip)
    {
        Regex reg = new Regex("\b(?:(?:25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\\.){3}(?:25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\b");
        return reg.IsMatch(ip);
    }

    public bool isValidIPAddress(string strIPAddress)
    {
        try
        {
            string[] IPValid = strIPAddress.Split('.');
            for (int i = 0; i < IPValid.Length; i++)
            {
                if (Int32.Parse(IPValid[i]) < 0 || Int32.Parse(IPValid[i]) > 255)
                {
                    return false;
                }
            }
            if (IPValid.Length != 4)
            {
                return false;
            }
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
