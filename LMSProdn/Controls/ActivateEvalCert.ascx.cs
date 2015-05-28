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

public partial class Controls_ActivateEvalCert : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Btnnext1_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;
        Certificate objCert = new Certificate();
        WizardStepBase parent = WizActivateEval.ActiveStep;
        KeyGenInput objkeygenIp = new KeyGenInput();
        objkeygenIp.CertSerialNumber = ((TextBox)parent.FindControl("TxtCertId1")).Text;
        objkeygenIp.Brand = Session["BRAND"].ToString();
        DataSet dsCertInfo = new DataSet();
        DACertificate objDACert = new DACertificate();

        dsCertInfo = objDACert.GetCertificateInfo(objkeygenIp.CertSerialNumber);
        if (dsCertInfo == null)
        {
            DisplayError("NO_CERT_INFO");
            return;
        }
        else
        {
            if (!dsCertInfo.Tables[0].Rows[0]["Part_id"].ToString().Contains("EVL"))
            {
                DisplayError("NOT_EVAL");
                return;
            }
            else if (!dsCertInfo.Tables[0].Rows[0][dsCertInfo.Tables[0].Columns[13].ColumnName.ToString()].Equals(string.Empty))
            {
                DisplayError("YES_ACTIVATED");
                return;
            }
        }
        DataTable dt = new DataTable();
        DataColumn dc1 = new DataColumn(Certificate.LIC_ID, typeof(System.Int32));
        dt.Columns.Add(dc1);
        DataColumn dc2 = new DataColumn(Certificate.LIC_SERIAL_NUMBER, typeof(System.String));
        dt.Columns.Add(dc2);
        DataColumn dc3 = new DataColumn(Certificate.ACTIVATION_KEY, typeof(System.String));
        dt.Columns.Add(dc3);
        DataColumn dc4 = new DataColumn(Certificate.LIC_PART_ID, typeof(System.String));
        dt.Columns.Add(dc4);
        DataColumn dc5 = new DataColumn(Certificate.LIC_PART_DESC, typeof(System.String));
        dt.Columns.Add(dc5);
        DataColumn dc6 = new DataColumn(Certificate.LIC_ALC_PART_ID_3EM, typeof(System.String));
        dt.Columns.Add(dc6);
        DataColumn dc7 = new DataColumn(Certificate.FORCE_SN_MAC, typeof(System.String));
        dt.Columns.Add(dc7);
        DataColumn dc8 = new DataColumn(Certificate.SYS_ID, typeof(System.String));
        dt.Columns.Add(dc8);
        DataColumn dc9 = new DataColumn(Certificate.SYS_SERIAL_NUMBER, typeof(System.String));
        dt.Columns.Add(dc9);
        DataColumn dc10 = new DataColumn(Certificate.SYS_PART_ID, typeof(System.String));
        dt.Columns.Add(dc10);
        DataColumn dc11 = new DataColumn(Certificate.SYS_PART_DESC, typeof(System.String));
        dt.Columns.Add(dc11);
        DataColumn dc12 = new DataColumn(Certificate.RFP, typeof(System.String));
        dt.Columns.Add(dc12);
        DataColumn dc13 = new DataColumn(Certificate.COMMENTS, typeof(System.String));
        dt.Columns.Add(dc13);

        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);
        dt.Rows[0][Certificate.LIC_ID] = Int32.Parse(dsCertInfo.Tables[0].Rows[0]["pk_cert_id"].ToString());
        dt.Rows[0][Certificate.LIC_SERIAL_NUMBER] = dsCertInfo.Tables[0].Rows[0]["serial_number"].ToString();
        dt.Rows[0][Certificate.ACTIVATION_KEY] = dsCertInfo.Tables[0].Rows[0]["activation_code"].ToString();
        dt.Rows[0][Certificate.LIC_PART_ID] = dsCertInfo.Tables[0].Rows[0]["part_id"].ToString();
        dt.Rows[0][Certificate.LIC_PART_DESC] = dsCertInfo.Tables[0].Rows[0]["part_desc"].ToString();
        dt.Rows[0][Certificate.LIC_ALC_PART_ID_3EM] = dsCertInfo.Tables[0].Rows[0]["AlcatelPartId"].ToString();
        dt.Rows[0][Certificate.FORCE_SN_MAC] = dsCertInfo.Tables[0].Rows[0]["ForceSNOrMAC"].ToString();
        dt.Rows[0][Certificate.RFP] = objCert.getPartType(dt.Rows[0][Certificate.LIC_PART_ID].ToString(),Session["BRAND"].ToString());

        if (!string.Empty.Equals(dt.Rows[0][Certificate.LIC_ALC_PART_ID_3EM].ToString().Trim()))
        {
            dt.Rows[0][Certificate.COMMENTS] = "Please enter MAC or serial number of the System";
        }
        else
        {
            dt.Rows[0][Certificate.COMMENTS] = "Please enter Serial number of the system";
        }
        if (!dt.Rows[0][Certificate.ACTIVATION_KEY].ToString().Equals(string.Empty))
        {
            DisplayError("YES_ACTIVATED");
            return;
        }

        //check for RFP Parts
        if ((dt.Rows[0][Certificate.RFP].ToString().Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString())))
        {
            dt.Rows[0][Certificate.COMMENTS] = "Please enter Mac Address of the system";
            if (!Session["BRAND"].ToString().Contains(ConfigurationManager.AppSettings["ARUBA_BRAND"].ToString()))
            {
                DisplayError("NO_CERT_INFO");
                return;
            }
        }

        if (!dt.Rows[0][Certificate.RFP].ToString().Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString()))
        {
            if (objCert.HasPartMapEntry(objkeygenIp) == false)
            {
                DisplayError("NO_PART_MAP");
                return;
            }
        }

        Session["CERT_INFO"] = dsCertInfo;
        LoadWizStep2(dt);
    }

    private void LoadWizStep2(DataTable dt)
    {
        try
        {
            WizActivateEval.ActiveStepIndex = 1;
            FrmVwSNo2.DataSource = dt;
            FrmVwSNo2.DataBind();
            Session["CERT_INFO"] = dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void LoadWizStep3(DataTable dt)
    {
        try
        {
            WizActivateEval.ActiveStepIndex = 2;
            FrmViewSl3.DataSource = dt;
            FrmViewSl3.DataBind();
            Session["CERT_INFO"] = dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void DisplayError(string strExp)
    {
        WizardStepBase parent = WizActivateEval.ActiveStep;
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

        if (strExp == "NOT_EVAL")
        {
            ((Label)parent.FindControl("LblError1")).Visible = true;
            ((Label)parent.FindControl("LblError1")).Text = Certificate.NOT_EVAL;
        }
        WizActivateEval.ActiveStepIndex = 0;
    }

    protected void BtnActivate2_Click(object sender, EventArgs e)
    {
        string method = "btnActivate_OnClick";
        string pk_cert_id;
        DACertificate daoCert = new DACertificate();
        UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
        DataTable dt = (DataTable)Session["CERT_INFO"];
        if (dt == null || dt.Rows.Count == 0)
        {
            WizActivateEval.ActiveStepIndex = 0;
            return;
        }
        DataSet dsSystemInfo = new DataSet();
        DataSet dsConfigData = new DataSet();
        Certificate objCert = new Certificate();
        string strSystemSerialNumber = string.Empty;
        string strSystemSerialText = string.Empty; //just a copy of User Entered system serial number
        bool hasErrors = false;

        //check if mac exists in esiTransfertable
        strSystemSerialNumber = ((TextBox)FrmVwSNo2.FindControl("TxtSnNo2")).Text.Trim();
        objCert.SerialNumber = strSystemSerialNumber;
        dsSystemInfo = objCert.GetCertInfo(objCert.SerialNumber);

        if (dt.Rows[0][Certificate.RFP].ToString().Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString()))
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            if (!objReg.IsMatch(strSystemSerialNumber))
            {
                setError("Invalid Mac Address");
                hasErrors = true;
                return;
            }
        }
        strSystemSerialText = strSystemSerialNumber;
        bool isMac = false;
        if (!dt.Rows[0][Certificate.LIC_ALC_PART_ID_3EM].ToString().Equals(string.Empty))
        {
            isMac = true;
            if (UIHelper.IsMacAddress(strSystemSerialNumber))
            {
                DataSet dsconfigdata = objCert.GetConfigData(strSystemSerialNumber);
                if (dsconfigdata != null)
                {
                    strSystemSerialNumber = dsconfigdata.Tables[0].Rows[0]["serialnumber"].ToString();
                    // ismac = false; // commmenting for now.!!!!!!!!!!!!!!!!!!!!!!!!!!!
                }
            }
            else
            {
                isMac = false;
            }

            switch (dt.Rows[0][Certificate.FORCE_SN_MAC].ToString())
            {
                case "FORCE_SERIAL":
                    if (isMac)
                    {
                        LblError2.Text = "You must enter a serial number for this certificate";
                        hasErrors = true;
                    }
                    break;
                case "FORCE_MAC":
                    if (!isMac)
                    {
                        LblError2.Text = "You must enter a HW MAC address for this certificate<BR />The MAC address must be in the form xx::xx:xx:xx:xx:xx and must consist of valid hex characters";
                        hasErrors = true;
                    }
                    break;
                default:
                    break;
            }

            if (isMac)
            {
                if (dsSystemInfo == null)
                {
                    //add the MAC to esitransfertable

                    objCert.AddMAC(strSystemSerialNumber, objUserInfo.Brand);
                }

            }
        }  //ends alcatelPArtId - MM check ends


        if (dsSystemInfo == null && (!dt.Rows[0][Certificate.RFP].ToString().Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString())))
        {
            hasErrors = true;
            setError("System Does not exists.You Want to Continue??");
            BtnActivate2.Visible = false;
            BtnCancel2.Visible = true;
            BtnContinue.Visible = true;
            return;
        }
        else
        {
            if ((dt.Rows[0][Certificate.RFP].ToString().Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString())) && (dsSystemInfo == null))
            {
                if (dt.Rows[0][Certificate.LIC_PART_ID].ToString().Contains(ConfigurationManager.AppSettings["RFPSTD"].ToString()))
                {
                    dt.Rows[0][Certificate.SYS_PART_ID] = "RFP-Mobile";
                    dt.Rows[0][Certificate.SYS_PART_DESC] = "RFP Mobile Server";
                }
                else
                {
                    dt.Rows[0][Certificate.SYS_PART_ID] = "RFP-Server";
                    dt.Rows[0][Certificate.SYS_PART_DESC] = "RFP Server";
                }
                pk_cert_id = objCert.AddMacAddress(dt.Rows[0][Certificate.SYS_PART_ID].ToString(), dt.Rows[0][Certificate.SYS_PART_DESC].ToString(), strSystemSerialNumber, Session["BRAND"].ToString());
                dt.Rows[0][Certificate.SYS_ID] = pk_cert_id;
                dt.Rows[0][Certificate.SYS_SERIAL_NUMBER] = strSystemSerialNumber;
            }
            else
            {
                dt.Rows[0][Certificate.SYS_ID] = Int32.Parse(dsSystemInfo.Tables[0].Rows[0]["pk_cert_id"].ToString());
                dt.Rows[0][Certificate.SYS_PART_ID] = dsSystemInfo.Tables[0].Rows[0]["part_id"].ToString();
                dt.Rows[0][Certificate.SYS_PART_DESC] = dsSystemInfo.Tables[0].Rows[0]["part_desc"].ToString();
                dt.Rows[0][Certificate.SYS_SERIAL_NUMBER] = strSystemSerialNumber;
            }
        }

        int SysId = Int32.Parse((dt.Rows[0][Certificate.SYS_ID]).ToString());
        int CertId = Int32.Parse((dt.Rows[0][Certificate.LIC_ID]).ToString());
        if (daoCert.IsCertSystemActivated(CertId, SysId))
        {
            setError(Certificate.YES_ACTIVATED_SYSTEM);
            hasErrors = true;
            return;
        }

        //get activation code

        string strActivationCode = dt.Rows[0][Certificate.ACTIVATION_KEY].ToString();
        if ((string.Empty).Equals(strActivationCode))
        {
            if (dt.Rows[0][Certificate.RFP].ToString().Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString()))
            {
                KeyGenInput objKeyGenIp = new KeyGenInput(dt.Rows[0][Certificate.LIC_SERIAL_NUMBER].ToString(),
                                          dt.Rows[0][Certificate.SYS_SERIAL_NUMBER].ToString(), objUserInfo.Brand);
                strActivationCode = objCert.GenerateRFPAcgtivationKey(objKeyGenIp);

            }
            else
            {
                KeyGenInput objKeyGenIp = new KeyGenInput(dt.Rows[0][Certificate.LIC_SERIAL_NUMBER].ToString(),
                                          dt.Rows[0][Certificate.SYS_SERIAL_NUMBER].ToString(), objUserInfo.Brand);
                strActivationCode = objCert.GenerateActivationKey(objKeyGenIp);
            }

        }

        if (string.Empty.Equals(strActivationCode) || (strActivationCode == null))
        {
            setError(Certificate.FAILURE_KEYGEN);
            new Log().logSystemError(method, Certificate.FAILURE_KEYGEN + ":CertId: " + CertId.ToString() + " SystemId:" + SysId.ToString());
            hasErrors = true;
            return;
        }
        else
        {
            dt.Rows[0][Certificate.ACTIVATION_KEY] = strActivationCode;
        }

        //Session["CERT_INFO"] = dt;

        //do final check to see if rows have activation key

        if (string.Empty.Equals(dt.Rows[0][Certificate.ACTIVATION_KEY].ToString()))
        {
            dt.Rows[0][Certificate.SYS_ERROR] = Certificate.FAILURE_KEYGEN;
            hasErrors = true;
        }
        if (hasErrors)
        {
            FrmVwActivation3.DataSource = dt;
            FrmVwActivation3.DataBind();

        }
        else
        {
            //string strtype = new Lookup().GetLookupText(objUserInfo.GetUserRole(), Lookup.CERT_TYPE); //need to check with ffang
            string strtype = CertType.AccountCertificate;
            if (objUserInfo.GetUserRole().Equals(UserType.Distributor))
                strtype = CertType.DistributorCertificate;
                //Added by Ashwini
                //strtype = CertType.CompanyCertificate;


            if (objCert.AddActivationInfo(dt, objUserInfo.GetUserAcctId(), strtype))
            {
                LoadWizStepLast(dt);
            }
            else
            {
                setError(Certificate.PERSISTENCE_ISSUE);
            }
        }

    }

    private void LoadWizStepLast(DataTable dt)
    {
        WizActivateEval.ActiveStepIndex = 3;
        FrmVwActivation3.DataSource = dt;
        FrmVwActivation3.DataBind();

        ////Call send Email API
        CertificateMailInfo objCertMailInfo = new CertificateMailInfo();
        UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
        objCertMailInfo.ActivationKey = dt.Rows[0][Certificate.ACTIVATION_KEY].ToString();
        objCertMailInfo.Brand = Session["BRAND"].ToString().ToUpper();
        objCertMailInfo.CertId = dt.Rows[0][Certificate.LIC_SERIAL_NUMBER].ToString();
        objCertMailInfo.CertPartDesc = dt.Rows[0][Certificate.LIC_PART_DESC].ToString();
        objCertMailInfo.CertPartId = dt.Rows[0][Certificate.LIC_PART_ID].ToString();
        objCertMailInfo.Email = objUserInfo.Email;
        objCertMailInfo.SysPartDesc = dt.Rows[0][Certificate.SYS_PART_DESC].ToString();
        objCertMailInfo.SysPartId = dt.Rows[0][Certificate.SYS_PART_ID].ToString();
        objCertMailInfo.SysSerialNumber = dt.Rows[0][Certificate.SYS_SERIAL_NUMBER].ToString();
        objCertMailInfo.Name = objUserInfo.FirstName + " " + objUserInfo.LastName;
        Email objEmail = new Email();
        objEmail.sendCertificateActivationInfo(objCertMailInfo);
    }


    private void setError(string text)
    {
        LblError2.Text = text;
        LblError2.Visible = true;
    }

    protected void BtnCancel2_Click(object sender, EventArgs e)
    {
        WizardStepBase parent = WizActivateEval.ActiveStep;
        WizActivateEval.ActiveStepIndex = 0;
        ((TextBox)parent.FindControl("TxtCertId1")).Text = string.Empty;
    }

    protected void BtnContinue_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["CERT_INFO"];
        Certificate objCert = new Certificate();
        dt.Rows[0]["SysSN"] = ((TextBox)FrmVwSNo2.FindControl("TxtSnNo2")).Text.Trim();
        string strSystemSerialNumber = string.Empty;
        strSystemSerialNumber = dt.Rows[0]["SysSN"].ToString();
        if (dt.Rows[0][Certificate.RFP].ToString().Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString()))
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            if (!objReg.IsMatch(strSystemSerialNumber))
            {
                setError("Invalid Mac Address");
                return;
            }
        }
        else
        {
            if (dt.Rows[0][Certificate.SYS_SERIAL_NUMBER].ToString().Length != Int32.Parse(ConfigurationManager.AppSettings["SL_LENGTH"].ToString()))
            {
                int str1 = Int32.Parse(ConfigurationManager.AppSettings["SL_LENGTH"].ToString());
                int str2 = dt.Rows[0][Certificate.SYS_SERIAL_NUMBER].ToString().Length;
                setError("Serial Number Length is Invalid");
                return;
            }
        }
        bool isMac = false;
        if (!dt.Rows[0][Certificate.LIC_ALC_PART_ID_3EM].ToString().Equals(string.Empty))
        {
            isMac = true;
            if (UIHelper.IsMacAddress(strSystemSerialNumber))
            {
                DataSet dsconfigdata = objCert.GetConfigData(strSystemSerialNumber);
                if (dsconfigdata != null)
                {
                    strSystemSerialNumber = dsconfigdata.Tables[0].Rows[0]["serialnumber"].ToString();
                    // ismac = false; // commmenting for now.!!!!!!!!!!!!!!!!!!!!!!!!!!!
                }
            }
            else
            {
                isMac = false;
            }

            switch (dt.Rows[0][Certificate.FORCE_SN_MAC].ToString())
            {
                case "FORCE_SERIAL":
                    if (isMac)
                    {
                        LblError2.Text = "You must enter a serial number for this certificate";
                    }
                    break;
                case "FORCE_MAC":
                    if (!isMac)
                    {
                        LblError2.Text = "You must enter a HW MAC address for this certificate<BR />The MAC address must be in the form xx::xx:xx:xx:xx:xx and must consist of valid hex characters";
                    }
                    break;
                default:
                    break;
            }
        } 
        LoadWizStep3(dt);
    }
    protected void BtnContinue3_Click(object sender, EventArgs e)
    {
        string method;
        method = "BtnContinue3_Click";
        DACertificate daoCert = new DACertificate();
        UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
        DataTable dt = (DataTable)Session["CERT_INFO"];
        if (dt == null || dt.Rows.Count == 0)
        {
            WizActivateEval.ActiveStepIndex = 0;
            return;
        }
        Certificate objCert = new Certificate();
        bool hasErrors = false;

        dt.Rows[0]["SysPartId"] = ((TextBox)FrmViewSl3.FindControl("Txtpartid")).Text.Trim(); 
        //Get Part Description

        dt.Rows[0]["SysPartDesc"] = objCert.getPartDesc(dt.Rows[0]["SysPartId"].ToString(),Session["BRAND"].ToString());

        //get activation code

        string strActivationCode = dt.Rows[0][Certificate.ACTIVATION_KEY].ToString();
        if ((string.Empty).Equals(strActivationCode))
        {
            if (dt.Rows[0][Certificate.RFP].ToString().Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString()))
            {
                KeyGenInput objKeyGenIp = new KeyGenInput(dt.Rows[0][Certificate.LIC_SERIAL_NUMBER].ToString(),
                                          dt.Rows[0][Certificate.SYS_SERIAL_NUMBER].ToString(), objUserInfo.Brand);
                strActivationCode = objCert.GenerateRFPAcgtivationKey(objKeyGenIp);

            }
            else
            {
                KeyGenInput objKeyGenIp = new KeyGenInput(dt.Rows[0][Certificate.LIC_SERIAL_NUMBER].ToString(),
                                          dt.Rows[0][Certificate.SYS_SERIAL_NUMBER].ToString(), objUserInfo.Brand);
                strActivationCode = objCert.GenerateActivationKey(objKeyGenIp);
            }

        }

        if (string.Empty.Equals(strActivationCode) || (strActivationCode == null))
        {
            setError(Certificate.FAILURE_KEYGEN);
            new Log().logSystemError(method, Certificate.FAILURE_KEYGEN + ":CertId: " + dt.Rows[0]["LicPartId"].ToString() + " SystemId:" + dt.Rows[0]["SysPartId"].ToString());
            hasErrors = true;
            return;
        }
        else
        {
            dt.Rows[0][Certificate.ACTIVATION_KEY] = strActivationCode;
        }

       //do final check to see if rows have activation key

        if (string.Empty.Equals(dt.Rows[0][Certificate.ACTIVATION_KEY].ToString()))
        {
            dt.Rows[0][Certificate.SYS_ERROR] = Certificate.FAILURE_KEYGEN;
            hasErrors = true;
        }
        if (hasErrors)
        {
            FrmVwActivation3.DataSource = dt;
            FrmVwActivation3.DataBind();
        }
        else
        {
            if (objCert.AddEvalActivationInfo(dt, objUserInfo.GetUserAcctId(), Request.ServerVariables["REMOTE_HOST"].ToString(),Session["BRAND"].ToString()))
            {
                LoadWizStepLast(dt);
            }
            else
            {
                setError(Certificate.PERSISTENCE_ISSUE);
            }
        }
    }
    protected void BtnCancel3_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["CERT_INFO"];
        LoadWizStep2(dt);
    }
}
