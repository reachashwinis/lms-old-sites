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
using Com.Arubanetworks.Licensing.Dataaccesslayer;
using System.Drawing;
using System.Web.Caching;
using System.Collections.Generic;


public partial class Controls_GenLegacyCerts : System.Web.UI.UserControl
{
    DataSet dsLookupValues;
    private string ERR_SERIALNUM_LEN="The serial number must be 9 or 10 characters";
    UserInfo objUserInfo;
    Lookup objLookup;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false;
        lblSuccess.Visible = false;
          objUserInfo = (UserInfo)(Session["USER_INFO"]);
          objLookup = new Lookup();
        if (!IsPostBack)
        {

            dsLookupValues = (DataSet)Cache[Lookup.LOOKUP_TBL];
            if (dsLookupValues == null)
            {
                dsLookupValues = objLookup.LoadLookupValues(((UserInfo)Session["USER_INFO"]).GetUserEmail(),Session["BRAND"].ToString());
                Cache.Insert(Lookup.LOOKUP_TBL, dsLookupValues, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }

            UIHelper.PrepareAndBindLookupFromAllWithoutPlease(ddlPartId, dsLookupValues.Tables[0], "QA_LIC_SYSTEM", "TXT", "VAL", true);
            ddlCertPart.Items.Insert(0, new ListItem("[Select System Part ID]", ""));
        }
        
    }
    protected void ddlPartId_SelectedIndexChanged(object sender, EventArgs args)
    {
        if (ddlPartId.SelectedValue.Equals(string.Empty))
        {
            ddlCertPart.Items.Clear();
            ddlCertPart.Items.Insert(0, new ListItem("[Select System Part ID]", string.Empty));

        }
        else
        {
            if (ddlPartId.SelectedValue.Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString()))
            {
                ddlCertPart.Items.Clear();
                DataSet ds = LoadRFPparts();
                UIHelper.PrepareAndBindListWithoutPlease(ddlCertPart, ds.Tables[Lookup.RFPPARTS_TBL], "TXT", "TXT", true);
                Cache.Insert("RFP_PARTS", ds, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                txtSNum.Text = "Not Required";
                txtSNum.ReadOnly = true;
            }
            else if(ddlPartId.SelectedValue.Contains(ConfigurationManager.AppSettings["ECS_TYPE"].ToString()))
            {

            }
            else
            {
                ddlCertPart.Items.Clear();
                DataSet ds = LoadLegacyCertParts(ddlPartId.SelectedValue);
                UIHelper.PrepareAndBindListWithoutPlease(ddlCertPart, ds.Tables[Lookup.CERTPARTS_TBL], "TXT", "TXT", true);
                Cache.Insert("CERT_PARTS", ds, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                txtSNum.Text = string.Empty;
                txtSNum.ReadOnly = false;
            }
        }
    }

    protected void btnGenQALic_OnClick(object sender, EventArgs args)
    {

        string strERROR = string.Empty;
        string strSUCCESS = string.Empty;
        string CustCode = txtCustCode.Text.Trim().ToUpper();
        string SerialNum = txtSNum.Text.Trim().ToUpper();
        string PartId = ddlPartId.SelectedValue.Trim().ToUpper();
        string certPartid = ddlCertPart.SelectedValue.Trim();
        string PartDesc = string.Empty;
        string strPartDesc = "No Part Desc yet";
        string strVersion = string.Empty;
        int SystemId = -1;
        DataSet dsCertInfo = new DataSet();
        DACertificate daoCert = new DACertificate();

        if (!Page.IsValid)
            return;

        Certificate objCert = new Certificate();
        string partType = objCert.getPartType(ddlCertPart.SelectedValue,Session["BRAND"].ToString());
        if (partType == ConfigurationManager.AppSettings["RFP_TYPE"].ToString() || partType == ConfigurationManager.AppSettings["ECS_TYPE"].ToString())
        {
            ECSGen.ECSGenerators objECS = new ECSGen.ECSGenerators();
            objECS.Url = ConfigurationManager.AppSettings["ECSGen.URL"].ToString();
            string sessId = objECS.getSessionID(ConfigurationManager.AppSettings["QA_SO_ID"].ToString());
            string reqUser = "GEN_QA";
            string strError = "<font color='green' size='medium'>";
            string ecsCertId = objECS.certgen(ddlCertPart.SelectedValue, reqUser, sessId);
            //ecsCertId = "L244367" + "||" + ecsCertId;
            if (ecsCertId.Length == 0)
            {
                strError += "There is a problem generating the certificate for part : " + ecsCertId + "<BR/>";
            }
            else
            {
                if (ecsCertId.Contains("||"))
                {
                    //ecsCertId = ecsCertId.Replace('|', ' ');
                    int index = ecsCertId.LastIndexOf('|');
                    int length = ecsCertId.Length;
                    ecsCertId = ecsCertId.Substring(index + 1, length - index - 1);
                }

                //Get Part Description
                Certificate objCertificate = new Certificate();
                DataSet ds = objCertificate.GetCertPartDesc(ddlCertPart.SelectedValue);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strPartDesc = ds.Tables[0].Rows[0]["description"].ToString();
                    strVersion = ds.Tables[0].Rows[0]["version"].ToString();
                }
                if (!objCertificate.AddQACertificate(ddlCertPart.SelectedValue, strPartDesc, ecsCertId, CustCode, objUserInfo.GetUserEmail(), "99999", objUserInfo.GetUserEmail(), Request.ServerVariables["REMOTE_ADDR"], "GenerateQALicense.aspx", Session["BRAND"].ToString(), strVersion))
                {
                    strERROR += "Unable to add Certitficate: " + ecsCertId + " for Part Id: " + ddlCertPart.SelectedValue + " to LMS  <BR>";
                    setError(strERROR, lblErr);
                }
                else
                {
                    strSUCCESS += "Certificate ID: " + ecsCertId + " was generated for " + ddlCertPart.SelectedValue + "<BR>";
                    setError(strSUCCESS, lblSuccess);
                }
            }

        }
        else
        {
            if (SerialNum.Length > 8 && SerialNum.Length < 11 && PartId.Length > 0 && CustCode.Length > 0)
            {
                Certificate objCertificate = new Certificate();
                DataSet ds = objCertificate.GetFruInfo(SerialNum);
                if (ds == null || ds.Tables[0].Rows.Count <= 0)
                {
                    strERROR += "The serial number " + SerialNum + "  does not exists in the LMS database<BR>";
                    setError(strERROR, lblErr);
                    return;
                }
                else
                {
                    dsCertInfo = objCert.GetCertInfo(SerialNum);
                    SystemId = Int32.Parse(dsCertInfo.Tables[0].Rows[0]["pk_cert_id"].ToString());
                }
            }
                
            // TODO Call GenQA
            string strError = GenAllQACert(certPartid, SerialNum, PartId, CustCode, SystemId);
            if (strError != string.Empty)
            {
                setError(strERROR, lblErr);
            }             
                
        }
     }

   protected DataSet LoadLegacyCertParts(string strPartId)
    {
        DataSet ds = new Lookup().LoadLegacyCertParts(strPartId);
        return ds;
    }

    protected DataSet LoadRFPparts()
    {
        DataSet ds = new Lookup().LoadRFPParts();
        return ds;
    }
    private void setError(string text,Label lbl)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }

    protected void cvSerialNumber_OnServerValidate(object sender, ServerValidateEventArgs args)
    { 
        args.IsValid=true;       
        if (!txtSNum.Text.Contains("Not Required") && (txtSNum.Text.Trim().Length < 9 || txtSNum.Text.Trim().Length > 10))
        {
         args.IsValid=false;
         ((CustomValidator)sender).ErrorMessage=ERR_SERIALNUM_LEN;
        }
    
    }

    public string GenAllQACert(string certPartid, string SerialNum, string PartId, string CustCode, int SystemId)
    {
                // Async
                DataSet dsCertParts = new DataSet();
                string strCertPart;
                string strERROR = string.Empty;
                string PartDesc = string.Empty;
                string LicenseKey = string.Empty;
                string ActivationKey = string.Empty;
                string StrLicenseKey = string.Empty;
                string strCertPartId = string.Empty;
                string strCertPartName = string.Empty;
                string strCertPartid = string.Empty;
                DACertificate daoCert = new DACertificate();

                strCertPartName = "<TABLE width='75%'border='1' border='black'>";
                Certificate objCert = new Certificate();
                DataSet dsCertInfo = new DataSet();

                if (certPartid.Contains("(Eval Only)"))
                {
                    strCertPart = "EVAL";
                }
                else if (certPartid.Contains("(All)"))
                {
                    strCertPart = "ALL";
                }
                else if (certPartid.Contains("(Permanent Only)"))
                {
                    strCertPart = "PERM";
                }
                else
                {
                    strCertPart = certPartid;
                }
                dsCertParts = objLookup.LoadLegacyCertParts(PartId, strCertPart);
                KeyGenInput objKgInp = new KeyGenInput();
                objKgInp.Brand = objUserInfo.Brand;
                for (int i = 0; i < dsCertParts.Tables[0].Rows.Count; i++)
                {
                    strCertPartid = string.Empty;
                    string strPartDesc = "No Part Desc yet";
                    string strVersion = string.Empty;
                    DataRow dr = dsCertParts.Tables[0].Rows[i];
                    objKgInp.CertPartId = dsCertParts.Tables[0].Rows[i]["TXT"].ToString();
                    LicenseKey = string.Empty;
                    ActivationKey = string.Empty;
                    strCertPartId = string.Empty;
                    //generate certificate
                    LicenseKey = objCert.GenerateCertificate(objKgInp,Session["BRAND"].ToString());
                    if (LicenseKey.Equals(string.Empty))
                    {
                        strERROR += "Unable to generate Certificate for " + objKgInp.CertPartId + " <BR>";
                    }
                    else
                    {
                        Certificate objCertificate = new Certificate();
                        DataSet ds = objCertificate.GetCertPartDesc(ddlCertPart.SelectedValue);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            strPartDesc = ds.Tables[0].Rows[0]["description"].ToString();
                            strVersion = ds.Tables[0].Rows[0]["version"].ToString();
                        }
                        //add to esitransfertable
                        if (!objCert.AddQACertificate(objKgInp.CertPartId, strPartDesc, LicenseKey, CustCode, objUserInfo.GetUserEmail(), "99999", objUserInfo.GetUserEmail(), Request.ServerVariables["REMOTE_ADDR"], "GenerateQALicense.aspx", Session["BRAND"].ToString(), strVersion))
                        {
                            strERROR += "Unable to add Certitficate: " + LicenseKey + " for Part Id: " + objKgInp.CertPartId + " to LMS  <BR>";
                        }
                        else
                        {
                            dsCertInfo = objCert.GetCertInfo(LicenseKey);
                            int CertId = Int32.Parse(dsCertInfo.Tables[0].Rows[0]["pk_cert_id"].ToString());
                            strCertPartId = dsCertInfo.Tables[0].Rows[0]["part_id"].ToString();
                            if (daoCert.IsCertSystemActivated(CertId, SystemId))
                            {
                                strERROR += "This Certificate : " + LicenseKey + " was already activated  <BR>";
                                break;
                            }
                            //generate Activation Key
                            objKgInp.CertSerialNumber = LicenseKey;
                            objKgInp.SystemSerialNumber = SerialNum;
                            ActivationKey = objCert.GenerateQAActivationKey(objKgInp);
                            if (ActivationKey.Equals(string.Empty))
                            {
                                strERROR += "Unable to Activate Certitficate: " + LicenseKey + " for System: " + SerialNum + " <BR>";
                            }
                            else
                            {

                                //add activation info
                                if (!objCert.AddQAActivationInfo(objUserInfo.AcctId, CertId, SystemId, ActivationKey, CertType.AccountCertificate, Session["BRAND"].ToString()))
                                {
                                    strERROR += "Unable to add Activation Info: Activation Key:<strong> " + ActivationKey + " </strong> Certificate: " + LicenseKey + " for System : " + SerialNum + " to LMS  <BR>";
                                }
                                else
                                {
                                    string strSuccess = ActivationKey + " is generated for the certificate " + LicenseKey + " on controller serial number " + SerialNum;
                                    setError(strSuccess, lblSuccess); 
                                    //send mail
                                    UserInfo objUser = (UserInfo)Session["USER_INFO"];
                                    CertificateMailInfo objCertMailInfo = new CertificateMailInfo();
                                    objCertMailInfo.ActivationKey = ActivationKey;
                                    objCertMailInfo.Brand = Session["BRAND"].ToString().ToUpper();
                                    objCertMailInfo.CertId = objKgInp.CertSerialNumber;
                                    objCertMailInfo.CertPartDesc = "No Part Desc Yet";
                                    objCertMailInfo.CertPartId = objKgInp.CertPartId;
                                    objCertMailInfo.Email = objUser.Email;
                                    objCertMailInfo.SysPartDesc = "No Part Desc Yet";
                                    objCertMailInfo.SysPartId = ddlPartId.SelectedItem.ToString();
                                    objCertMailInfo.SysSerialNumber = objKgInp.SystemSerialNumber;
                                    Email objEmail = new Email();
                                    objEmail.sendCertificateActivationInfo(objCertMailInfo);
                                    //update the Upgrade status
                                    if (daoCert.isUpgradebleCert(objKgInp.CertPartId))
                                    {
                                        if (!objCert.UpdateUpgradeStat(objKgInp.SystemSerialNumber,string.Empty))
                                        {
                                            //send mail
                                            objEmail.UpdateFailureInfo(objKgInp.SystemSerialNumber, ConfigurationManager.AppSettings["BCC_MAIL"].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (strERROR != string.Empty)
                {
                    Email objEmail = new Email();
                    objEmail.sendQAGenrateFailure(strERROR, ConfigurationManager.AppSettings["BCC_MAIL"].ToString());
                }

              return strERROR; 
    }
}



