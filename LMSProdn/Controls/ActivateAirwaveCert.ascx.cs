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

public partial class Controls_ActivateAirwaveCert : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false;
    }
    protected void btnAirwaveActivate_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;

        bool blAddActkey = false;
        string meth = "btnAirwaveActivate_Click";
        string strActivationKey = string.Empty;
        string strOrg = string.Empty;
        Certificate objCert = new Certificate();
        string strBrand = string.Empty;
        string strSerialNo = string.Empty;
        int APCount = 0;
        Email objMail = new Email();
        string strOrderId = string.Empty; string strProduct = string.Empty; string strProductDesc = string.Empty;
        AirwaveCertInfo objAirwaveCertInfo = new AirwaveCertInfo();
        UserInfo objUser = new UserInfo();
        objUser = (UserInfo)Session["USER_INFO"];
        AirwaveKeyProcessor objAirwaveKeyProcessor = new AirwaveKeyProcessor();
        try
        {
            //Check for Certificate
            DataSet dsAirwaveCert = objCert.getAirwaveCertDet(TxtEvalKey.Text,Session["BRAND"].ToString());

            if (dsAirwaveCert.Tables[0].Rows.Count <= 0)
            {
                lblErr.Text = "This Certificate does not exist.";
                lblErr.Visible = true;
                return;
            }

            //Check for Activation
            if (objCert.IsAirwaveCertActivated(TxtEvalKey.Text.Trim()))
            {
                lblErr.Text = "This Certificate is already activated";
                lblErr.Visible = true;
                return;
            }

            if (!isValidIPAddress(txtSNum.Text.Trim()))
            {
                lblErr.Text = "This IP address is invalid";
                lblErr.Visible = true;
                return;
            }

            if (dsAirwaveCert.Tables[0].Rows.Count > 0)
            {
                strOrderId = dsAirwaveCert.Tables[0].Rows[0]["so_id"].ToString();
                strProduct = dsAirwaveCert.Tables[0].Rows[0]["part_id"].ToString();
                strProductDesc = dsAirwaveCert.Tables[0].Rows[0]["part_desc"].ToString();
                strBrand = dsAirwaveCert.Tables[0].Rows[0]["brand"].ToString();
                strSerialNo = dsAirwaveCert.Tables[0].Rows[0]["Lserial_number"].ToString();

                //if (strProduct.Contains("EXP") || strProduct.Contains("EXF"))
                DataSet dsParts = objCert.getPartDetails(strProduct, ConfigurationManager.AppSettings["ARUBA_BRAND"].ToString());
                if (bool.Parse(dsParts.Tables[0].Rows[0]["IsExp"].ToString()) == true)
                {
                    objAirwaveKeyProcessor.TheKey = objCert.getAWCertificate(TxtEvalKey.Text.Trim());
                    if (objAirwaveKeyProcessor.TheKey != string.Empty)
                    {
                        APCount = objAirwaveKeyProcessor.APCount;
                    }
                    else
                    {
                        APCount = 0;
                        lblErr.Text = "This Certificate does not exist.";
                        lblErr.Visible = true;
                        return;
                    }
                }

                if (strBrand == Session["BRAND"].ToString())
                {
                    strOrg = Server.UrlEncode(txtSoId.Text);
                    strActivationKey = objCert.GenerateAirwaveActivation(strOrderId, txtSNum.Text, strProduct, strOrg, APCount, strSerialNo);
                }
                else
                {
                    lblErr.Text = "Invalid Certificate";
                    lblErr.Visible = true;
                    return;
                }
            }
            if (strActivationKey.Contains("Error"))
            {
                new Log().logInfo("btnAirwaveActivate_Click", strActivationKey);
                lblErr.Text = "Unable to generate License key. Application could not process your request.";
                lblErr.Visible = true;
                return;
            }
            else
            {
                blAddActkey = objCert.InsertAirwaveActkey(strActivationKey, txtSNum.Text, TxtEvalKey.Text, txtSoId.Text, objUser.GetUserAcctId());
                if (blAddActkey == true)
                {
                    //send mail
                    objAirwaveCertInfo.Activationkey = strActivationKey;
                    objAirwaveCertInfo.Brand = Session["BRAND"].ToString().ToUpper();
                    objAirwaveCertInfo.CertId = TxtEvalKey.Text;
                    objAirwaveCertInfo.Email = objUser.GetUserEmail();
                    objAirwaveCertInfo.IPAddress = txtSNum.Text;
                    objAirwaveCertInfo.Name = objUser.FirstName + " " + objUser.LastName;
                    objAirwaveCertInfo.Package = strProduct;
                    objAirwaveCertInfo.PackageDesc = strProductDesc;
                    bool blSend = objMail.sendAirwaveActivationInfo(objAirwaveCertInfo);
                    objAirwaveKeyProcessor.TheKey = strActivationKey;
                    ((Literal)wizActivate.FindControl("LiteralAct")).Text = strActivationKey.Replace("\n", "<BR>");
                    wizActivate.ActiveStepIndex = 1;
                }
                else
                {
                    new Log().logInfo("btnAirwaveActivate_Click", "Unable to insert the activation key.");
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
    protected void lnkEULA_Click(object sender, EventArgs e)
    {
        RdBtnLst2.SelectedValue = "yes";
        //Response.Redirect("EULA.pdf", true);
        ScriptManager.RegisterStartupScript(this, sender.GetType(), "HTML", "window.open('EULA_aruba_Airwave.aspx','NewWindow','height=900,width=900,top=50,left=150,status=yes,toolbar=no, menubar=no,location=no,resizable=yes,scrollbars=no')", true);
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
