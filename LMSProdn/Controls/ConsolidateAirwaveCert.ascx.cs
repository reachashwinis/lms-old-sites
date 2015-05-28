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
public partial class Controls_ConsolidateAirwaveCert : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        setError(string.Empty, false);
    }
    protected void BtnNext_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;

        Certificate objCert = new Certificate();
        AirwaveCertInfo objAirwaveCertInfo = new AirwaveCertInfo();
        AirwaveKeyProcessor objAirwaveKeyProcessor1 = new AirwaveKeyProcessor();
        AirwaveKeyProcessor objAirwaveKeyProcessor2 = new AirwaveKeyProcessor();
        UserInfo objUser = new UserInfo();
        objUser = (UserInfo)Session["USER_INFO"];
        string strConsLic = string.Empty;
        string strOrg = string.Empty;
        Email objEmail = new Email();

        if (!objCert.GetAirwaveCert(TxtCertId1.Text,Session["BRAND"].ToString()))
        {
            setError(TxtCertId1.Text + " does not exist.", true);
            return;
        }

        if (!objCert.GetAirwaveCert(TxtCertId2.Text,Session["BRAND"].ToString()))
        {
            setError(TxtCertId2.Text + " does not exist.", true);
            return;
        }

        bool blIsActCert1 = objCert.IsAirwaveCertActivated(TxtCertId1.Text);
        if (blIsActCert1 == false)
        {
            setError(TxtCertId1.Text + " is not activated", true);
            return;
        }

        bool blIsActCert2 = objCert.IsAirwaveCertActivated(TxtCertId2.Text);
        if (blIsActCert2 == false)
        {
            setError(TxtCertId2.Text + " is not activated", true);
            return;
        }

        objAirwaveKeyProcessor1.TheKey = objCert.GetAirwaveCertInfoForIP(TxtCertId1.Text);
        string strIPAddress1 = objAirwaveKeyProcessor1.IPAddress;
        string strOrg1 = objAirwaveKeyProcessor1.Organization;
        bool blRapid1 = objAirwaveKeyProcessor1.RAPIDs;
        bool blVisualRF1 = objAirwaveKeyProcessor1.VisualRF;
        int intAPCount1 = objAirwaveKeyProcessor1.APCount;
        string strPart1 = objAirwaveKeyProcessor1.Product;
        string strPackage1 = objAirwaveKeyProcessor1.Package;

        objAirwaveKeyProcessor2.TheKey = objCert.GetAirwaveCertInfoForIP(TxtCertId2.Text);
        string strIPAddress2 = objAirwaveKeyProcessor2.IPAddress;
        string strOrg2 = objAirwaveKeyProcessor2.Organization;
        bool blRapid2 = objAirwaveKeyProcessor2.RAPIDs;
        bool blVisualRF2 = objAirwaveKeyProcessor2.VisualRF;
        int intAPCount2 = objAirwaveKeyProcessor2.APCount;
        string strPart2 = objAirwaveKeyProcessor2.Product;
        string strPackage2 = objAirwaveKeyProcessor2.Package;

        if (strPackage1.Contains("EVAL"))
        {
            setError("This " + strPackage1 + " is evaluation certificate", true);
            return;
        }

        if (strPackage2.Contains("EVAL"))
        {
            setError("This " + strPackage2 + " is evaluation certificate", true);
            return;
        }

        string strSerialNo = objAirwaveKeyProcessor1.SerialNumber + ":" + objAirwaveKeyProcessor2.SerialNumber;

        if (blRapid2 == true)
        {
            blRapid2 = true;
        }

        if (blVisualRF2 == true)
        {
            blVisualRF1 = true;
        }

        if (strIPAddress1 == strIPAddress2)
        {
            if (objCert.IsAirwaveCertConsolidated(TxtCertId1.Text, TxtCertId2.Text,strIPAddress1))
            {
                setError(TxtCertId1.Text + "," + TxtCertId2.Text + " are already consolidated", true);
                return;
            }
            int intAPCount = intAPCount1 + intAPCount2;
            strOrg = Server.UrlEncode(Txtorg.Text);
            strConsLic = objCert.ConsolidateAirwvLic(strPart1, strOrg, strIPAddress1, blRapid1, blVisualRF1, intAPCount, strSerialNo);
        }
        else
        {
            setError("License key mismatch!.The IP address should be same to consolidate the license", true);
            return;
        }

        if (strConsLic == string.Empty)
        {
            setError("Unable to consolidate the License key!.", true);
            return;
        }
        else
        {
            if (objCert.InsertConsolidateAirwveActKey(strConsLic, strIPAddress1, TxtCertId1.Text, TxtCertId2.Text, Txtorg.Text, objUser.GetUserAcctId()))
            {
                objAirwaveCertInfo.Package = objAirwaveKeyProcessor1.Package;
                objAirwaveCertInfo.PackageDesc = objAirwaveKeyProcessor2.Package;
                objAirwaveCertInfo.Name = objUser.FirstName + " " + objUser.LastName;
                objAirwaveCertInfo.Email = objUser.GetUserEmail();
                objAirwaveCertInfo.IPAddress = strIPAddress1;
                objAirwaveCertInfo.CertId = TxtCertId1.Text + "<BR>" + TxtCertId2.Text;
                objAirwaveCertInfo.Brand = Session["BRAND"].ToString().ToUpper();
                objAirwaveCertInfo.Activationkey = strConsLic;
                objEmail.sendAirwaveConsolidateActInfo(objAirwaveCertInfo);
                ((Literal)WizConsolidate.FindControl("LiteralLickey")).Text = strConsLic.Replace("\n", "<BR>");
                WizConsolidate.ActiveStepIndex = 1;
            }
            else
            {
                setError("Unable to consolidate the License key!.", true);
                return;
            }
        }
    }
    private void setError(string strCaption, bool blVisible)
    {
        LblErr.Visible = blVisible;
        LblErr.Text = strCaption;
    }

    protected void CustomValid_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (TxtCertId1.Text == string.Empty || TxtCertId2.Text == string.Empty)
        {
            args.IsValid = false;
            CustomValid.ErrorMessage = "Certificate Id are mandatory.";
            return;
        }

        if (Txtorg.Text == string.Empty)
        {
            args.IsValid = false;
            CustomValid.ErrorMessage = "Organization is mandatory.";
            return;
        }
        if (TxtCertId1.Text == TxtCertId2.Text)
        {
            args.IsValid = false;
            CustomValid.ErrorMessage = "You are Entering same Certificate Id!!.";
            return;
        }
        args.IsValid = true;
    }
}
