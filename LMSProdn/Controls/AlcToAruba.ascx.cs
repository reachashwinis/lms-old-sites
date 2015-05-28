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

public partial class Controls_AlcToAruba : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (RdBtnLst1.Items.FindByValue("CertificateId").Selected == true)
            {
                LblCertId1.Text = "Certificate";
            }
            else
            {
                LblCertId1.Text = "Controller";
            }
        }

    }
    protected void Btnnext1_Click(object sender, EventArgs e)
    {
        
        //WizardStepBase parent = WizAlcToAruba.ActiveStep;
        //((RadioButton)parent.FindControl("RdBtnLst1")).
        if (!Page.IsValid)
            return;

        if (RdBtnLst1.Items.FindByValue("CertificateId").Selected == true)
        {
            LoadCertDetails();
        }
        else
        {
            LoadSKUDetails();
        }

        //Session["CERT_INFO"] = dsCertInfo;
        //LoadWizStep2(dt);
    }

    private void LoadWizStep2(DataTable dt)
    {
        try
        {
            WizAlcToAruba.ActiveStepIndex = 1;
            rptCertSNo2.DataSource = dt;
            rptCertSNo2.DataBind();
            Session["CERT_INFO"] = dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void DisplayError(string strExp)
    {
        WizardStepBase parent = WizAlcToAruba.ActiveStep;
        if (strExp == "NO_PART_MAP")
        {
            ((Label)parent.FindControl("LblError1")).Visible = true;
            ((Label)parent.FindControl("LblError1")).Text = Certificate.NO_PART_MAP;
        }
        if (strExp == "YES_ACTIVATED")
        {
            ((Label)parent.FindControl("LblError1")).Visible = true;
            ((Label)parent.FindControl("LblError1")).Text = Certificate.YES_ACTIVATED;
        }
        if (strExp == "NO_CERT_INFO")
        {
            ((Label)parent.FindControl("LblError1")).Visible = true;
            ((Label)parent.FindControl("LblError1")).Text = Certificate.NO_CERT_INFO;
        }
        if (strExp == "NO_ARUBA_PART")
        {
            ((Label)parent.FindControl("LblError1")).Visible = true;
            ((Label)parent.FindControl("LblError1")).Text = Certificate.NO_ALC_PART;
        }

        WizAlcToAruba.ActiveStepIndex = 0;
    }

    protected void BtnActivate2_Click(object sender, EventArgs e)
    {
        string strERROR, LicenseKey;
        strERROR = string.Empty; LicenseKey = string.Empty;
        DACertificate objDACert = new DACertificate();
        DataTable dt = (DataTable)Session["CERT_INFO"];
        Certificate objCertificate = new Certificate();
        KeyGenInput objKgInp = new KeyGenInput();
        objKgInp.Brand = ConfigurationManager.AppSettings["ARUBA_BRAND"].ToString();
        for (int i = 0;i < dt.Rows.Count; i++)
        {
            objKgInp.CertPartId = dt.Rows[i]["ArubaPartId"].ToString();
            if (dt.Rows[i]["certType"].ToString() == ConfigurationManager.AppSettings["CERT_TYPE"].ToString())
            {
                LicenseKey = objCertificate.GenerateArubaCert(objKgInp);
                if (LicenseKey.Equals(string.Empty))
                {
                    dt.Rows[i]["Error"] = "Unable to generate Certificate for " + objKgInp.CertPartId + " <BR>";
                }
                else
                {
                    //add to esitransfertable
                    dt.Rows[i]["ArubaSerialNumber"] = LicenseKey;
                }
            }
            else
            {
                dt.Rows[i]["ArubaSerialNumber"] = dt.Rows[i]["serial_number"].ToString();
            }
        }
          
            if (!objCertificate.UpdateAlcatelCert(dt))
            {
               strERROR += "Unable to add Certitficate: " + LicenseKey + " for Part Id: " + objKgInp.CertPartId + " to LMS  <BR>";
               DisplayError(strERROR);
            }
            else
            {
                LoadWizStepLast(dt);
            }
    }

    private void LoadWizStepLast(DataTable dt)
    {
        bool blResult = false;
        WizAlcToAruba.ActiveStepIndex = 2;
        rptCertConvert3.DataSource = dt;
        rptCertConvert3.DataBind();
        WizardStepBase parent = WizAlcToAruba.ActiveStep;

        ////Call send Email API
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            CertificateMailInfo objCertMailInfo = new CertificateMailInfo();
            UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
            objCertMailInfo.Brand = Session["BRAND"].ToString().ToUpper();
            objCertMailInfo.CertPartId = dt.Rows[i]["part_id"].ToString();
            objCertMailInfo.CertPartDesc = dt.Rows[i]["part_desc"].ToString();
            objCertMailInfo.Email = objUserInfo.Email;
            objCertMailInfo.ArubaPartId = dt.Rows[i]["ArubaPartId"].ToString();
            objCertMailInfo.ArubaPartDesc = dt.Rows[i]["ArubaPartDesc"].ToString();
            objCertMailInfo.ArubaSerialNumber = dt.Rows[i]["ArubaSerialNumber"].ToString();
            objCertMailInfo.Name = objUserInfo.FirstName + " " + objUserInfo.LastName;
            Email objEmail = new Email();
            blResult = objEmail.sendArubaCertInfo(objCertMailInfo);
        }
        if (blResult == true)
        {
            ((Label)parent.FindControl("LblEmail")).Text = "You will Receive an Email with the Aruba Certificate details";
        }
    }


    private void setError(string text)
    {
        LblError2.Text = text;
        LblError2.Visible = true;
    }

    private void LoadCertDetails()
    {
        KeyGenInput objkeygenIp = new KeyGenInput();
        Certificate objCert = new Certificate();
        WizardStepBase parent = WizAlcToAruba.ActiveStep;
        DataSet dsCertInfo = new DataSet();
        DataTable dt = new DataTable();
        DACertificate objDACert = new DACertificate();

        objkeygenIp.CertSerialNumber = ((TextBox)parent.FindControl("TxtCertId1")).Text;
        objkeygenIp.Brand = Session["BRAND"].ToString();

        if (objCert.HasPartMapEntry(objkeygenIp) == false)
        {
            DisplayError("NO_PART_MAP");
            return;
        }

        dsCertInfo = objDACert.GetArubaCertInfoForAlc(objkeygenIp.CertSerialNumber);
        if (dsCertInfo == null)
        {
            DisplayError("NO_CERT_INFO");
            return;
        }
        else
        {
            if (dsCertInfo.Tables[0].Rows[0]["ArubaPartId"].ToString() == string.Empty)
            {
                DisplayError("NO_ARUBA_PART");
                return;
            }

            dt = BuildDataTable(dsCertInfo);
            LoadWizStep2(dt);
            
        }

    }
    private void LoadSKUDetails()
    {
        KeyGenInput objkeygenIp = new KeyGenInput();
        Certificate objCert = new Certificate();
        DACertificate objDACert = new DACertificate();
        DataSet dsSKUInfo = new DataSet();
        DataTable dt = new DataTable();
        WizardStepBase parent = WizAlcToAruba.ActiveStep;
        objkeygenIp.SystemSerialNumber = ((TextBox)parent.FindControl("TxtCertId1")).Text;
        dsSKUInfo = objDACert.GetArubaCertInfoForAlcSKU(objkeygenIp.SystemSerialNumber);
        if (dsSKUInfo == null)
        {
            DisplayError("NO_CERT_INFO");
            return;
        }
        else
        {
            if (dsSKUInfo.Tables[0].Rows[0]["ArubaPartId"].ToString() == string.Empty)
            {
                DisplayError("NO_ARUBA_PART");
                return;
            }
            dt = BuildDataTable(dsSKUInfo);
            LoadWizStep2(dt);
        }
    }

    private DataTable BuildDataTable(DataSet ds)
    {
        DataTable dt = new DataTable();
        dt = ds.Tables[0];
        return dt;
    }
}
