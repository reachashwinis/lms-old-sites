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
using System.Reflection;


public partial class Controls_ImportMyLegacySubscription : System.Web.UI.UserControl
{
    Email objemail = new Email();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void BtnNext1_Click(object sender, EventArgs e)
    {
        Certificate objCert = new Certificate();
        DataTable dt = new DataTable();       
        UserInfo objUserInfo = (UserInfo) Session["USER_INFO"];
        bool blError = false;
        try
        {
            if (!objCert.isValidAmigopodSubscription(TxtSubKey1.Text.Trim()))
            {
                if (!objCert.IsLegacySubscription(TxtSubKey1.Text.Trim()))
                {
                    LblError1.Text = "The Subscription ID is not valid.";
                    WizImportlegacySub.ActiveStepIndex = 0;
                    LblError1.Visible = true;
                    return;
                }
                else if (objCert.IsEvaluationSubKey(TxtSubKey1.Text.Trim()))
                {
                    LblError1.Text = "You can not import this Subscription ID as this is of evaluation type.";
                    WizImportlegacySub.ActiveStepIndex = 0;
                    LblError1.Visible = true;
                    return;
                }
                else if (objCert.IsSubkeyImported(TxtSubKey1.Text.Trim()))
                {
                    LblError1.Text = "This Subscription ID is already exists!";
                    WizImportlegacySub.ActiveStepIndex = 0;
                    LblError1.Visible = true;
                    return;
                }
                else
                {
                    dt = objCert.BuildAmgCertInfo();
                    DataRow dr = dt.NewRow();
                    Subscription objSub = objCert.GetDetailsAmigopodSubscription(TxtSubKey1.Text.Trim(), "1");
                    if (objSub.so == "" || objSub.so == "0")
                    {
                        objSub.so = "55555";
                    }
                    if (objSub.po == "" || objSub.po == "0")
                    {
                        objSub.so = "Lgcy4444";
                    }

                    if (objSub.license == "" || objSub.license == "0")
                    {
                        LblError1.Text = "Application could not process your request.The error occurred has been logged";
                        LblError1.Visible = true;
                        objemail.sendAmigopodErrorMessage("Invalid Subscription ID. The license count for the subcription key is zero", "ImportSubscription", "", TxtSubKey1.Text);
                        return;
                    }

                    dr["SubscriptionKey"] = objSub.subscription_key;
                    dr["Organization"] = objSub.name;
                    dr["po_id"] = objSub.po;
                    dr["so_id"] = objSub.so;
                    dr["license_count"] = objSub.license;
                    dr["onBoard_count"] = objSub.on_board_license;
                    dr["expire_time"] = objSub.expiretime;
                    dr["user_name"] = objSub.username;
                    dr["password"] = objSub.password;
                    dr["sms_credit"] = objSub.sms_credit;
                    dr["sms_handler"] = objSub.sms_handler;
                    dr["email"] = objSub.email;
                    dr["create_time"] = objSub.create_time;
                    dr["HA"] = objSub.high_availability;
                    dr["advertising"] = objSub.adv_feature;
                    dr["category"] = objSub.category;
                    dr["sms_count"] = objSub.sms_count;
                    if (objSub.subscription_key != objSub.high_avaialbility_key)
                    {
                        dr["HASubscriptionKey"] = objSub.high_avaialbility_key;
                    }
                    dt.Rows.Add(dr);
                    if (Convert.ToDateTime(dr["expire_time"].ToString()) <= System.DateTime.Today)
                    {
                        dr["Error"] = "This Subscription ID has crossed the expiry date of support. To extend the expiration, please contact your local Aruba Networks representative, or contact Aruba Support at <A>support@arubanetworks.com </A>";
                        blError = false;
                    }
                    if (blError == true)
                    {
                        LblError1.Text = "Error is occurred.";
                        WizImportlegacySub.ActiveStepIndex = 0;
                        LblError1.Visible = true;
                    }
                    else
                    {
                        LoadWizStep2(dt);
                    }
                }
            }
            else
            {
                LblError1.Text = "The Subscription ID already exists!";
                WizImportlegacySub.ActiveStepIndex = 0;
                LblError1.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            new Log().logException("ImportSubscription", ex);
            LblError1.Text = "Application could not process your request.The error occurred has been logged";
            LblError1.Visible = true;
            objemail.sendAmigopodErrorMessage(ex.Message, "ImportSubscription", "", TxtSubKey1.Text);
        }

    }

    private void LoadWizStep2(DataTable dt)
    {
        WizImportlegacySub.ActiveStepIndex = 1;
        rptSub2.DataSource = dt;
        rptSub2.DataBind();
        Session["AMG_CERT"] = dt;
    }

    protected void BtnImport2_Click(object sender, EventArgs e)
    {
        try
        {
            Certificate objCert = new Certificate();
            DataTable dt = (DataTable)Session["AMG_CERT"];
            UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
            int AcctId = objUserInfo.GetUserAcctId();
            int ImpAcctId = objUserInfo.AcctId;
            DataTable dtResult = new DataTable();
            Email objEmail = new Email();
            
            dtResult = objCert.ImportLegacySubKey(dt, AcctId, ImpAcctId);
            if (dtResult.Rows.Count > 0)
            {
                // send mail confirmation
                AmigopodCertInfo objAmigopodCertinfo = new AmigopodCertInfo();
                for (int index=0; index < dtResult.Rows.Count; index++)
                {
                    objAmigopodCertinfo.Subscriptionkey = dtResult.Rows[index]["SubscriptionKey"].ToString();
                    objAmigopodCertinfo.CertId = dtResult.Rows[index]["serial_number"].ToString();                    
                    objAmigopodCertinfo.SerialNumber = dtResult.Rows[index]["Lserial_number"].ToString();
                    objAmigopodCertinfo.SoId = dtResult.Rows[index]["so_id"].ToString();
                    objAmigopodCertinfo.PoId = dtResult.Rows[index]["po_id"].ToString();                    
                    objAmigopodCertinfo.Brand = Session["BRAND"].ToString().ToUpper();
                    objAmigopodCertinfo.Email = objUserInfo.GetUserEmail();
                    objAmigopodCertinfo.Name = objUserInfo.FirstName + " " + objUserInfo.LastName; 
                    objAmigopodCertinfo.HACertid = dtResult.Rows[index]["HAserial_number"].ToString();
                    objAmigopodCertinfo.HACertSerialNo = dtResult.Rows[index]["HALserial_number"].ToString();
                    objAmigopodCertinfo.HASubKey = dtResult.Rows[index]["HASubscriptionKey"].ToString();
                    objAmigopodCertinfo.guestLicCertId = dtResult.Rows[index]["LicCertId"].ToString();
                    objAmigopodCertinfo.guestLicCertSerialNo = dtResult.Rows[index]["LicSerialNo"].ToString();
                    objAmigopodCertinfo.onBoardCertId = dtResult.Rows[index]["OnBoardCertId"].ToString();
                    objAmigopodCertinfo.onBoardSerialNo = dtResult.Rows[index]["OnBoardSerialNo"].ToString();
                    objAmigopodCertinfo.AdvCertId = dtResult.Rows[index]["AdvCertId"].ToString();
                    objAmigopodCertinfo.AdvSerialNo = dtResult.Rows[index]["AdvSerialNo"].ToString();
                    objEmail.sendAmigopodImportSubKeyDet(objAmigopodCertinfo);
                }
                LoadWizStep3(dtResult);
            }
            else
            {
                WizImportlegacySub.ActiveStepIndex = 1;
                LblError2.Text = "Application could not process your request.The error occurred has been logged";
                LblError2.Visible = true;
                objemail.sendAmigopodErrorMessage("Error in Importing Subscription " + TxtSubKey1.Text.Trim(), "ImportSubscription", "", TxtSubKey1.Text);
            }
        }
        catch (Exception ex)
        {
            new Log().logException("ImportSubscription", ex);
            WizImportlegacySub.ActiveStepIndex = 0;
            LblError1.Text = "Application could not process your request.The error occurred has been logged";
            LblError1.Visible = true;
            objemail.sendAmigopodErrorMessage(ex.Message, "ImportSubscription", "", TxtSubKey1.Text);
        }
    }

    private void LoadWizStep3(DataTable dt)
    {
        WizImportlegacySub.ActiveStepIndex = 2;
        rptSub3.DataSource = dt;
        rptSub3.DataBind();
    }

}
