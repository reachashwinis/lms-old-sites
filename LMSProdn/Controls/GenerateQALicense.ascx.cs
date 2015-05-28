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
using System.Runtime.Remoting.Messaging;


public partial class Controls_GenerateQALicense : System.Web.UI.UserControl
{
    DataSet dsLookupValues;
    private string ERR_SERIALNUM_LEN = "The serial number must be 9 or 10 characters";
    private string ERR_MACID_LEN = "The MAC ID must be an Alphanumeric characters in format XX:XX:XX:XX:XX:XX.";
    private string ERR_COUNT_NO = "Please enter only number";
    private string ERR_PASSPHRASE_LEN = "Passphrase is mandatory and must be of 35 characters.";
    private string ERR_PASSPHRASE_REQ = "Please enter Passphrase for this controller.";
    private string ERR_PASSPHRASE_INVALID = "Invalid Passphrase for this serial Number.";
    UserInfo objUserInfo;
    Lookup objLookup;
    delegate string MyAsyncQADelegate(string certPartid, string SerialNum, string PartId, string CustCode, int SystemId, string ServIpAddr, string strSession, string strVersion, string strFruPart, string strPassphrase);

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
                dsLookupValues = objLookup.LoadLookupValues(((UserInfo)Session["USER_INFO"]).GetUserEmail(), Session["BRAND"].ToString());
                Cache.Insert(Lookup.LOOKUP_TBL, dsLookupValues, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }

            UIHelper.PrepareAndBindLookupFromAllWithoutPlease(ddlPartId, dsLookupValues.Tables[0], "QA_LIC_SYSTEM", "TXT", "VAL", true);
            ddlCertPart.Items.Insert(0, new ListItem("[Select System Part ID]", ""));

            if (ConfigurationManager.AppSettings["SPL_PRIV"].Contains(objUserInfo.GetUserEmail()))
            {
                chkOverride.Visible = true;
                chkOverride.Checked = false;
            }
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
                txtPassphrase.ReadOnly = true;
                txtPassphrase.Text = string.Empty;
            }
            else if (ddlPartId.SelectedValue.Contains(ConfigurationManager.AppSettings["ECS_TYPE"].ToString()))
            {
                txtPassphrase.ReadOnly = true;
                txtPassphrase.Text = string.Empty;
            }
            else if (ddlPartId.SelectedValue.Contains(ConfigurationManager.AppSettings["A9090"].ToString()) || ddlPartId.SelectedValue.Contains(ConfigurationManager.AppSettings["A9000"].ToString())
                || ddlPartId.SelectedValue.Contains(ConfigurationManager.AppSettings["A9980"].ToString()))
            {
                ddlCertPart.Items.Clear();
                DataSet ds = LoadCertParts(ddlPartId.SelectedValue, ddlVersion.SelectedValue, ((UserInfo)Session["USER_INFO"]).GetUserEmail());
                UIHelper.PrepareAndBindListWithoutPlease(ddlCertPart, ds.Tables[Lookup.CERTPARTS_TBL], "TXT", "TXT", true);
                Cache.Insert("CERT_PARTS", ds, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                txtSNum.Text = string.Empty;
                txtPassphrase.ReadOnly = false;
                txtPassphrase.Text = string.Empty;
            }
            else
            {
                ddlCertPart.Items.Clear();
                DataSet ds = LoadCertParts(ddlPartId.SelectedValue, ddlVersion.SelectedValue, ((UserInfo)Session["USER_INFO"]).GetUserEmail());
                UIHelper.PrepareAndBindListWithoutPlease(ddlCertPart, ds.Tables[Lookup.CERTPARTS_TBL], "TXT", "TXT", true);
                Cache.Insert("CERT_PARTS", ds, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                txtSNum.Text = string.Empty;
                txtSNum.ReadOnly = false;
                txtPassphrase.ReadOnly = true;
                txtPassphrase.Text = string.Empty;
            }
        }
    }

    protected void ddlVersion_SelectedIndexChanged(object sender, EventArgs args)
    {
        if (ddlPartId.SelectedValue == "Please Select")
        {
            ddlCertPart.Items.Clear();
            ddlCertPart.Items.Insert(0, new ListItem("[Select System Part ID]", string.Empty));
        }
        else
        {
            ddlCertPart.Items.Clear();
            DataSet ds = LoadCertParts(ddlPartId.SelectedValue, ddlVersion.SelectedValue, ((UserInfo)Session["USER_INFO"]).GetUserEmail());
            UIHelper.PrepareAndBindListWithoutPlease(ddlCertPart, ds.Tables[Lookup.CERTPARTS_TBL], "TXT", "TXT", true);
            Cache.Insert("CERT_PARTS", ds, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            txtSNum.Text = string.Empty;
            txtSNum.ReadOnly = false;
        }

    }

    protected void ddlCertPart_SelectedIndexChanged(object sender, EventArgs args)
    {
        Certificate objCert = new Certificate();
        if (ddlCertPart.SelectedItem.Text.ToUpper().Contains("FLEXIBLE") || objCert.isFlexibleLicense(ddlCertPart.SelectedItem.Text) == true)
        {
            txtCount.ReadOnly = false;
        }
        else
        {
            txtCount.ReadOnly = true;
            txtCount.Text = string.Empty;
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
        string strVersion = ddlVersion.SelectedValue.Trim();
        string PartDesc = string.Empty;
        string strFruPart = string.Empty;
        string strPassphrase = string.Empty;
        int SystemId = -1;
        DataSet dsCertInfo = new DataSet();
        DACertificate daoCert = new DACertificate();

        if (!Page.IsValid)
            return;

        Certificate objCert = new Certificate();
        string partType = objCert.getPartType(ddlCertPart.SelectedValue, Session["BRAND"].ToString());
        if (ddlVersion.SelectedValue == "3" && (partType == ConfigurationManager.AppSettings["RFP_TYPE"].ToString() || partType == ConfigurationManager.AppSettings["ECS_TYPE"].ToString()))
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

                Certificate objCertificate = new Certificate();
                if (!objCertificate.AddQACertificate(ddlCertPart.SelectedValue, "No Part Desc Yet", ecsCertId, CustCode, objUserInfo.GetUserEmail(), "99999", objUserInfo.GetUserEmail(), Request.ServerVariables["REMOTE_ADDR"], "GenerateQALicense.aspx", Session["BRAND"].ToString(), string.Empty))
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
            //if (SerialNum.Length > 8 && SerialNum.Length < 11 && PartId.Length > 0 && CustCode.Length > 0)
            //{
            Certificate objCertificate = new Certificate();
            bool insertSN = true;
            DataSet ds = objCertificate.GetCertInfo(SerialNum);
            if (ds != null)
            {
                if (!ds.Tables[0].Rows[0]["ship_to_cust"].ToString().Contains("ARU"))
                {
                    //strERROR += "The serial number " + SerialNum + "  already exists in the LMS database<BR>";
                    setError("The serial number " + SerialNum + "  was shipped to a different customer in the LMS database<BR> Please Use <I>Remove Serial Number/Certificate from LMS</I> utility to remove this controller from LMS.", lblErr);
                    return;
                }
                else
                {
                    insertSN = false;
                }
            }


            if (insertSN == true && (strERROR.Trim().Length == 0 || chkOverride.Checked == true))
            {
                if (!objCertificate.AddQACertificate(PartId, PartDesc, SerialNum, CustCode, objUserInfo.GetUserEmail(), "99999", objUserInfo.GetUserEmail(), Request.ServerVariables["REMOTE_ADDR"], "GenerateQALicense.aspx", Session["BRAND"].ToString(), string.Empty))
                {
                    setError("Unable to add Serial Number to LMS", lblErr);
                    return;
                }
                strSUCCESS += "Serial number " + SerialNum + " was added to LMS database<BR>";

                dsCertInfo = objCertificate.GetCertInfo(SerialNum);
                SystemId = Int32.Parse(dsCertInfo.Tables[0].Rows[0]["pk_cert_id"].ToString());
            }
            else
            {
                SystemId = Int32.Parse(ds.Tables[0].Rows[0]["pk_cert_id"].ToString());

            }

            //Async calling 
            MyAsyncQADelegate QAasyncDel = new MyAsyncQADelegate(this.GenAllQACert);
            HttpContext objHttpCont = HttpContext.Current;
            string strServIPAddr = objHttpCont.Request.ServerVariables["REMOTE_ADDR"];
            string strSession = Session["BRAND"].ToString();
            if (ddlPartId.SelectedItem.Text.Contains(ConfigurationManager.AppSettings["A9900"].ToString()))
            {
                strFruPart = "A9900";
            }
            else if (ddlPartId.SelectedItem.Text.Contains(ConfigurationManager.AppSettings["A9090"].ToString()))
            {
                strFruPart = "A9090";
                strPassphrase = txtPassphrase.Text;
            }
            else if (ddlPartId.SelectedItem.Text.Contains(ConfigurationManager.AppSettings["A9000"].ToString()))
            {
                strFruPart = "A9000";
                strPassphrase = txtPassphrase.Text;
            }
            else if (ddlPartId.SelectedItem.Text.Contains(ConfigurationManager.AppSettings["A9980"].ToString()))
            {
                strFruPart = "A9980";
                strPassphrase = txtPassphrase.Text;
            }

            IAsyncResult result = QAasyncDel.BeginInvoke(certPartid, SerialNum, PartId, CustCode, SystemId, strServIPAddr, strSession, strVersion, strFruPart, strPassphrase, new AsyncCallback(myAsynCallBackComplete), null);
            setError("Generating Certificate.... An email with all the Activation keys will be sent to your inbox shortly.", lblSuccess);
        }
        //}
    }

    private void myAsynCallBackComplete(IAsyncResult result)
    {
        AsyncResult res = (AsyncResult)result;
        MyAsyncQADelegate QAasyncDel = (MyAsyncQADelegate)res.AsyncDelegate;
        QAasyncDel.EndInvoke(res);
    }

    protected DataSet LoadCertParts(string strPartId)
    {
        DataSet ds = new Lookup().LoadCertParts(strPartId);
        return ds;
    }

    protected DataSet LoadCertParts(string strPartId, string strVersion, string strUser)
    {
        DataSet ds = new Lookup().LoadCertPartsQA(strPartId, strVersion, strUser);
        return ds;
    }

    protected DataSet LoadRFPparts()
    {
        DataSet ds = new Lookup().LoadRFPParts();
        return ds;
    }

    private void setError(string text, Label lbl)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }

    protected void cvCount_OnServerValidate(object sender, ServerValidateEventArgs args)
    {
        if (ddlCertPart.SelectedItem.Text.ToUpper().Contains("FLEXIBLE"))
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtCount.Text, "^([0-9]*)$"))
            {

            }
            else
            {
                args.IsValid = false;
                ((CustomValidator)sender).ErrorMessage = ERR_COUNT_NO;
            }
        }
    }
    protected void cvSerialNumber_OnServerValidate(object sender, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (ddlPartId.SelectedItem.Text.Contains(ConfigurationManager.AppSettings["A9900"].ToString()))
        {
            //if (System.Text.RegularExpressions.Regex.IsMatch(txtSNum.Text, "^[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}$"))
            if (System.Text.RegularExpressions.Regex.IsMatch(txtSNum.Text, "^[0-9a-zA-Z]{2}:[0-9a-zA-Z]{2}:[0-9a-zA-Z]{2}:[0-9a-zA-Z]{2}:[0-9a-zA-Z]{2}:[0-9a-zA-Z]{2}$"))
            {

            }
            else
            {
                args.IsValid = false;
                ((CustomValidator)sender).ErrorMessage = ERR_MACID_LEN;
            }
        }
        else
        {
            if (!txtSNum.Text.Contains("Not Required") && (txtSNum.Text.Trim().Length < 9 || txtSNum.Text.Trim().Length > 10))
            {
                args.IsValid = false;
                ((CustomValidator)sender).ErrorMessage = ERR_SERIALNUM_LEN;
            }
        }
    }

    protected void CvPassphrase_OnServerValidate(object sender, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        Certificate objCert = new Certificate();
        KeyGenInput objKgInp = new KeyGenInput();
        objKgInp.PassPhrase = txtPassphrase.Text.Trim();
        objKgInp.SystemSerialNumber = txtSNum.Text.Trim();
        string strPassPhrase = string.Empty;
        if (ddlPartId.SelectedItem.Text.Contains(ConfigurationManager.AppSettings["A9090"].ToString()) || ddlPartId.SelectedItem.Text.Contains(ConfigurationManager.AppSettings["A9000"].ToString())
        || ddlPartId.SelectedItem.Text.Contains(ConfigurationManager.AppSettings["A9980"].ToString()))
        {
            if (txtPassphrase.Text.Equals(string.Empty))
            {
                args.IsValid = false;
                ((CustomValidator)sender).ErrorMessage = ERR_PASSPHRASE_REQ;
            }
            else if (txtPassphrase.Text.Trim().Length != 35)
            {
                args.IsValid = false;
                ((CustomValidator)sender).ErrorMessage = ERR_PASSPHRASE_LEN;
            }
            else
            {
                strPassPhrase = objCert.DecodePassphrase(objKgInp);
                if (strPassPhrase.ToUpper().Contains("INVALID") || strPassPhrase.Equals(string.Empty))
                {
                    args.IsValid = false;
                    ((CustomValidator)sender).ErrorMessage = ERR_PASSPHRASE_INVALID;
                }
            }
        }
    }

    protected void cvMacId_OnServerValidate(object sender, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (!txtSNum.Text.Contains("Not Required") && (txtSNum.Text.Trim().Length < 12 || txtSNum.Text.Trim().Length > 12))
        {
            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = ERR_MACID_LEN;
        }
    }

    public string GenAllQACert(string certPartid, string SerialNum, string PartId, string CustCode, int SystemId, string ServerIpAdd, string strSession, string strVersion, string strFruPart, string strPassphrase)
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
        bool blUpgrade = false;
        strCertPartName = "<TABLE width='75%'border='1' border='black'>";
        Certificate objCert = new Certificate();
        DataSet dsCertInfo = new DataSet();
        Certificate objCertificate = new Certificate();
        DACertificate daoCert = new DACertificate();
        Email objEmail = new Email();
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
        else if (objCert.isFlexibleLicense(certPartid) == true)
        {
            strCertPart = "FLEX";
        }
        else if (certPartid.Contains("ALL LICENSES (Flexible license Only)"))
        {
            strCertPart = "FLEXALL";
        }
        else
        {
            strCertPart = certPartid;
        }
        dsCertParts = objLookup.LoadCertPartsQA(PartId, strCertPart, strVersion, ((UserInfo)Session["USER_INFO"]).GetUserEmail());
        KeyGenInput objKgInp = new KeyGenInput();
        objKgInp.Brand = objUserInfo.Brand;
        for (int i = 0; i < dsCertParts.Tables[0].Rows.Count; i++)
        {
            strCertPartid = string.Empty;
            DataRow dr = dsCertParts.Tables[0].Rows[i];
            objKgInp.CertPartId = dsCertParts.Tables[0].Rows[i]["TXT"].ToString();
            LicenseKey = string.Empty;
            ActivationKey = string.Empty;
            strCertPartId = string.Empty;
            string strPartDesc = "No Part Desc yet";
            string strCertVersion = string.Empty;
            //generate certificate
            if (strCertPart == "FLEX" || strCertPart == "FLEXALL")
            {
                objKgInp.UserCount = txtCount.Text.Trim();
            }
            if (strPassphrase != string.Empty)
            {
                LicenseKey = objCertificate.GenerateVCQACertificate(objKgInp, Session["BRAND"].ToString());
            }
            else
            {
                LicenseKey = objCertificate.GenerateCertificate(objKgInp, Session["BRAND"].ToString());
            }

            if (LicenseKey.Equals(string.Empty))
            {
                strERROR += "Unable to generate Certificate for " + objKgInp.CertPartId + " <BR>";
            }
            else
            {
                //Certificate objCertificate = new Certificate();
                DataSet ds = objCertificate.GetCertPartDesc(ddlCertPart.SelectedValue);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strPartDesc = ds.Tables[0].Rows[0]["description"].ToString();
                    strCertVersion = ds.Tables[0].Rows[0]["version"].ToString();
                }

                //add to esitransfertable
                if (!objCertificate.AddQACertificate(objKgInp.CertPartId, strPartDesc, LicenseKey, CustCode, objUserInfo.GetUserEmail(), "99999", objUserInfo.GetUserEmail(), ServerIpAdd, "GenerateQALicense.aspx", Session["BRAND"].ToString(), strCertVersion))
                {
                    strERROR += "Unable to add Certitficate: " + LicenseKey + " for Part Id: " + objKgInp.CertPartId + " to LMS  <BR>";
                }
                else
                {
                    dsCertInfo = objCertificate.GetCertInfo(LicenseKey);
                    int CertId = Int32.Parse(dsCertInfo.Tables[0].Rows[0]["pk_cert_id"].ToString());
                    strCertPartId = dsCertInfo.Tables[0].Rows[0]["part_id"].ToString();
                    if (daoCert.IsCertSystemActivated(CertId, SystemId))
                    {
                        strERROR += "This Certificate : " + LicenseKey + " was already activated  <BR>";
                        break;
                    }

                    if (blUpgrade == false && (daoCert.isUpgradebleCert(objKgInp.CertPartId)))
                    {
                        blUpgrade = true;
                    }
                    //generate Activation Key
                    objKgInp.CertSerialNumber = LicenseKey;
                    objKgInp.SystemSerialNumber = SerialNum;
                    if (strCertPart == "FLEX" || strCertPart == "FLEXALL")
                    {
                        ActivationKey = objCertificate.GenerateFlexQAActivationKey(objKgInp);
                    }
                    else
                    {
                        if (strFruPart == "A9000" || strFruPart == "A9090" || strFruPart == "A9980")
                        {
                            objKgInp.PassPhrase = strPassphrase;
                            //strERROR += "Certitficate: " + objKgInp.CertSerialNumber + " System: " + objKgInp.SystemSerialNumber + " Brand: " + objKgInp.Brand + "<BR>";
                            ActivationKey = objCertificate.GenerateVCQAActivationKey(objKgInp);
                        }
                        else
                        {
                            ActivationKey = objCertificate.GenerateQAActivationKey(objKgInp);
                        }
                    }

                    if (ActivationKey.Equals(string.Empty))
                    {
                        strERROR += "Unable to Activate Certitficate: " + LicenseKey + " for System: " + SerialNum + " for passphrase:" + strPassphrase + " for Fru: " + strFruPart + "<BR>";
                    }
                    else
                    {
                        //add activation info
                        if (!objCertificate.AddQAActivationInfo(objUserInfo.AcctId, CertId, SystemId, ActivationKey, CertType.AccountCertificate, Session["BRAND"].ToString()))
                        {
                            strERROR += "Unable to add Activation Info: Activation Key:<strong> " + ActivationKey + " </strong> Certificate: " + LicenseKey + " for System : " + SerialNum + " to LMS  <BR>";
                        }
                        else
                        {
                            //if (!strCertPart.Contains("EVAL") || !strCertPart.Contains("PERM") || !strCertPart.Contains("ALL"))
                            //{
                            //    //send mail
                            //    UserInfo objUser = (UserInfo)Session["USER_INFO"];
                            //    CertificateMailInfo objCertMailInfo = new CertificateMailInfo();
                            //    objCertMailInfo.ActivationKey = ActivationKey;
                            //    objCertMailInfo.Brand = Session["BRAND"].ToString().ToUpper();
                            //    objCertMailInfo.CertId = objKgInp.CertSerialNumber;
                            //    objCertMailInfo.CertPartDesc = "No Part Desc Yet";
                            //    objCertMailInfo.CertPartId = objKgInp.CertPartId;
                            //    objCertMailInfo.Email = objUser.Email;
                            //    objCertMailInfo.SysPartDesc = "No Part Desc Yet";
                            //    objCertMailInfo.SysPartId = ddlPartId.SelectedItem.ToString();
                            //    objCertMailInfo.SysSerialNumber = objKgInp.SystemSerialNumber;
                            //    Email objEmail = new Email();
                            //    objEmail.sendCertificateActivationInfo(objCertMailInfo);
                            //}
                            // if (strCertPart.Contains("EVAL") || strCertPart.Contains("PERM") || strCertPart.Contains("ALL"))
                            // {
                            //strSUCCESS += " Activation Key: <strong>" + ActivationKey + " </strong> for License :" + LicenseKey + "<BR>";
                            //strSUCCESS += "license add " + ActivationKey + "<BR>";
                            StrLicenseKey += "license add " + ActivationKey + "\r\n";
                            strCertPartName += "<TR><TD>" + strCertPartId + "</TD><TD> " + ActivationKey + "</TD></TR>";

                            //}

                            //strSUCCESS += "Certificate ID: " + LicenseKey + " was generated for " + objKgInp.CertPartId + "<BR>";
                            //strSUCCESS += "Activation Key: <strong>" + ActivationKey + "</strong> for License :" + LicenseKey + " System: " + SerialNum + " <BR>";
                            if (ActivationKey == string.Empty)
                            {
                                strERROR += "Certificate ID: " + LicenseKey + " Please activate this certificate using \"Activate Certificate\" <BR>";
                            }
                            if (blUpgrade == true)
                            {
                                if (!objCert.UpdateUpgradeStat(SerialNum.Trim(), string.Empty))
                                {
                                    //send mail
                                    objEmail.UpdateFailureInfo(SerialNum, ConfigurationManager.AppSettings["BCC_MAIL"].ToString());
                                }
                            }

                        }
                    }
                }
            }
        }
        strCertPartName += "" + "</TABLE>";
        UserInfo objUser = (UserInfo)Session["USER_INFO"];
        CertificateMailInfo objCertMailInfo = new CertificateMailInfo();
        objCertMailInfo.Brand = Session["BRAND"].ToString().ToUpper();
        objCertMailInfo.Email = objUser.Email;
        objCertMailInfo.SysPartDesc = "No Part Desc Yet";
        objCertMailInfo.SysPartId = ddlPartId.SelectedItem.ToString();
        objCertMailInfo.SysSerialNumber = objKgInp.SystemSerialNumber;
        objCertMailInfo.ActivationKey = StrLicenseKey;
        objCertMailInfo.CertPartDesc = strCertPartName;
        objCertMailInfo.Passphrase = strPassphrase;
        if (!StrLicenseKey.Equals(string.Empty))
        {
            objEmail.sendQALicenseKey(objCertMailInfo);
        }

        if (strERROR != string.Empty)
        {
            objEmail.sendQAGenrateFailure(strERROR, ConfigurationManager.AppSettings["BCC_MAIL"].ToString());
        }

        return strERROR;
    }
}


