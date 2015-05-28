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
using System.Timers;

public partial class Controls_ActivateCertificate : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {   
        lblDbError.Visible=false; 
    }
    protected void btnNext_OnClick(object sender, EventArgs args)
    {
        
        //Page.Validate();
        if (!Page.IsValid)
            return;

        DataSet dsCertInfo = new DataSet();
        Certificate objCert = new Certificate();
        DataTable dt = objCert.BuildCertInfo();
        DataRow dr;
        int i = 1;
        dr = dt.NewRow();
        if (addCertRow(dr, txtCertificate1, i))
        {
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            i++;
        
        }
        if (addCertRow(dr, txtCertificate2, i))
        {
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            i++;

        }
        if (addCertRow(dr, txtCertificate3, i))
        {
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            i++;

        }
        if (addCertRow(dr, txtCertificate4, i))
        {
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            i++;

        }
        if (addCertRow(dr, txtCertificate5, i))
        {
            dt.Rows.Add(dr);
            dr = dt.NewRow();
            i++;

        }
        if (addCertRow(dr, txtCertificate6, i))
        {
            dt.Rows.Add(dr);
        }
        //check for duplicates
        bool hasErrors = false;
        if(dt.Rows.Count>1)
        {
            for (int oInd = dt.Rows.Count-1; oInd >=0; oInd--)
            {
                for (int iInd = 0; iInd < oInd; iInd++)
                {
                    if (dt.Rows[oInd][Certificate.LIC_SERIAL_NUMBER].ToString().Equals(dt.Rows[iInd][Certificate.LIC_SERIAL_NUMBER].ToString()))
                    {
                        dt.Rows[oInd][Certificate.LIC_ERROR] = Certificate.DUP_CERT_INFO;
                        hasErrors = true;
                    }
                                
                }
                       
            }
        }
        if (hasErrors == false)
        {

            //get certinfos for validation

            for (int j = 0; j < dt.Rows.Count; j++)
            {

                dsCertInfo = objCert.GetCertInfo(dt.Rows[j][Certificate.LIC_SERIAL_NUMBER].ToString());
                if (dsCertInfo == null)
                {
                    dt.Rows[j][Certificate.LIC_ERROR] = Certificate.NO_CERT_INFO;
                    hasErrors = true;
                    continue;

                }

                dt.Rows[j][Certificate.LIC_ID] = Int32.Parse(dsCertInfo.Tables[0].Rows[0]["pk_cert_id"].ToString());
                dt.Rows[j][Certificate.LIC_SERIAL_NUMBER] = dsCertInfo.Tables[0].Rows[0]["serial_number"].ToString();
                dt.Rows[j][Certificate.ACTIVATION_KEY] = dsCertInfo.Tables[0].Rows[0]["activation_code"].ToString();
                dt.Rows[j][Certificate.LIC_PART_ID] = dsCertInfo.Tables[0].Rows[0]["part_id"].ToString();
                dt.Rows[j][Certificate.LIC_PART_DESC] = dsCertInfo.Tables[0].Rows[0]["part_desc"].ToString();
                dt.Rows[j][Certificate.LIC_ALC_PART_ID_3EM] = dsCertInfo.Tables[0].Rows[0]["AlcatelPartId"].ToString();
                dt.Rows[j][Certificate.FORCE_SN_MAC] = dsCertInfo.Tables[0].Rows[0]["ForceSNOrMAC"].ToString();
                dt.Rows[j][Certificate.RFP] = objCert.getPartType(dt.Rows[j][Certificate.LIC_PART_ID].ToString(),Session["BRAND"].ToString());
                dt.Rows[j][Certificate.LOC] = dsCertInfo.Tables[0].Rows[0]["Location"].ToString();
                dt.Rows[j][Certificate.VERSION] = dsCertInfo.Tables[0].Rows[0]["version"].ToString();
                
                if (!string.Empty.Equals(dt.Rows[j][Certificate.LIC_ALC_PART_ID_3EM].ToString().Trim()))
                {
                    dt.Rows[j][Certificate.COMMENTS] = "Please enter MAC or serial number of the System";
                }
                else
                {
                    dt.Rows[j][Certificate.COMMENTS] = "Please enter Serial number of the system";
                }
                if (!dt.Rows[j][Certificate.ACTIVATION_KEY].ToString().Equals(string.Empty))
                {
                    dt.Rows[j][Certificate.LIC_ERROR] = Certificate.YES_ACTIVATED;
                    hasErrors = true;
                    continue;

                }

                if (true)//comment for now...
                {
                    KeyGenInput objkeygenIp = new KeyGenInput();
                    objkeygenIp.CertSerialNumber = dt.Rows[j][Certificate.LIC_SERIAL_NUMBER].ToString();
                    objkeygenIp.Brand = Session["BRAND"].ToString();
                    if (Session["BRAND"].ToString() == ConfigurationManager.AppSettings["ARUBA_BRAND"].ToString() && dt.Rows[j][Certificate.RFP].ToString() == ConfigurationManager.AppSettings["RFP_TYPE"].ToString())
                    {
                        if (objCert.HasPartMapEntry(objkeygenIp) == false)
                        {
                            continue;
                        }
                        else
                        {
                            dt.Rows[j][Certificate.IS_ARUBA_RFP] = "YES";
                        }
                    }
                    else
                    {
                        if (objCert.HasPartMapEntry(objkeygenIp) == false)
                        {
                            dt.Rows[j][Certificate.LIC_ERROR] = Certificate.NO_PART_MAP;
                            hasErrors = true;
                            continue;
                        }
                    }
                }

                //check for RFP Parts
                if ((dt.Rows[j][Certificate.RFP].ToString().Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString())) && dt.Rows[j][Certificate.IS_ARUBA_RFP].ToString() != "YES")
                {
                    dt.Rows[j][Certificate.COMMENTS] = "Please enter MAC Address of the System";
                    if (!Session["BRAND"].ToString().Contains(ConfigurationManager.AppSettings["ARUBA_BRAND"].ToString()))
                    {
                        dt.Rows[j][Certificate.LIC_ERROR] = Certificate.NO_CERT_INFO;
                        hasErrors = true;
                        continue;
                    }
                }

            }
        }
        Session["CERT_INFO"] = dt;
        if (hasErrors == true)
        {
            clearAllCertControls();
            WizardStepBase parent = wizActivate.ActiveStep;
            for (int rIndex = 1; rIndex <= dt.Rows.Count; rIndex++)
            {
                
                ((TextBox)parent.FindControl("txtCertificate" + rIndex.ToString())).Text = dt.Rows[rIndex-1][Certificate.LIC_SERIAL_NUMBER].ToString();
                ((Label)parent.FindControl("lblErr" + rIndex.ToString())).Text = dt.Rows[rIndex-1][Certificate.LIC_ERROR].ToString();
            
            }
            wizActivate.ActiveStepIndex = 0;
            return;

        }
       
        LoadWizStep2(dt);
     
    }

    private void LoadWizStep2(DataTable dt)
    {
        wizActivate.ActiveStepIndex = 1;
        rptCert.DataSource = dt;
        rptCert.DataBind();
        for (int i = 0; i < rptCert.Items.Count; i++)
        {
        if (dt.Rows[i][Certificate.LOC].ToString() == string.Empty)
            {
            ((TextBox)rptCert.Items[i].FindControl("txtLocation")).ReadOnly = false;
            }
        }
    }


    protected void btnActivate_OnClick(object sender, EventArgs args)
    {
        string method = "btnActivate_OnClick";
        string strSerialNumber = string.Empty;
        UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];

        DACertificate daoCert = new DACertificate();
        DataTable dt = (DataTable)Session["CERT_INFO"];
        if (dt == null || dt.Rows.Count == 0)
        {
            wizActivate.ActiveStepIndex = 0;
            return;
        }
        DataSet dsSystemInfo = new DataSet();
        DataSet dsConfigData = new DataSet();
        Certificate objCert = new Certificate();
        bool hasErrors = false;
        string pk_cert_id;
        //check whether 3 and 5 certs are activating on the same controller
        //string strResult = IsSameVersionCertOnCtrl(dt);
        //if (strResult != string.Empty && CheckBox1.Checked == false)
        //{
        //    lblDbError.Text = strResult;
        //    lblDbError.Visible = true;
        //    CheckBox1.Visible = true;
        //    return;
        //}
        for (int i = 0; i < rptCert.Items.Count; i++)
        {
            string strSystemSerialNumber = string.Empty;
            string strSystemSerialText = string.Empty; //just a copy of User Entered system serial number
            string strLocation = string.Empty;
            string strSystemMac = string.Empty;
            strSystemSerialNumber = ((TextBox)rptCert.Items[i].FindControl("txtSNMac")).Text.Trim();
            strSystemSerialText = strSystemSerialNumber;

            strLocation = ((TextBox)rptCert.Items[i].FindControl("txtLocation")).Text.Trim();
            bool isMac = false;

            if (dt.Rows[i][Certificate.RFP].ToString().Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString()) && dt.Rows[i][Certificate.IS_ARUBA_RFP] != "YES")
            {
                Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
                string r = ConfigurationManager.AppSettings["MAC_FORMAT"].ToString();
                if (!objReg.IsMatch(strSystemSerialNumber))
                {
                    dt.Rows[i][Certificate.SYS_ERROR] = "Invalid Mac Address";
                    hasErrors = true;
                    continue;
                }
            }

            if (!dt.Rows[i][Certificate.LIC_ALC_PART_ID_3EM].ToString().Equals(string.Empty))
            {
                isMac = true;
                if (UIHelper.IsMacAddress(strSystemSerialNumber))
                {
                    DataSet dsconfigdata = objCert.GetConfigData(strSystemSerialNumber);
                    if (dsconfigdata != null)
                    {
                        strSystemMac = strSystemSerialNumber;
                        strSystemSerialNumber = dsconfigdata.Tables[0].Rows[0]["serialnumber"].ToString();
                        // ismac = false; // commmenting for now.!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    }
                }
                else
                {
                    isMac = false;
                }

                switch (dt.Rows[i][Certificate.FORCE_SN_MAC].ToString())
                {
                    case "FORCE_SERIAL":
                        if (isMac)
                        {
                            dt.Rows[i][Certificate.SYS_ERROR] = "You must enter a serial number for this certificate";
                            hasErrors = true;
                        }
                        break;
                    case "FORCE_MAC":
                        if (!isMac)
                        {
                            dt.Rows[i][Certificate.SYS_ERROR] = "You must enter a HW MAC address for this certificate<BR />The MAC address must be in the form xx::xx:xx:xx:xx:xx and must consist of valid hex characters";
                            hasErrors = true;
                        }
                        break;
                    default:
                        break;

                }
                if (hasErrors)
                {
                    continue;
                }
                if (isMac)
                {
                    //check if mac exists in esiTransfertable
                    objCert.SerialNumber = strSystemSerialNumber;
                    dsSystemInfo = objCert.GetCertInfo(objCert.SerialNumber);
                    if (dsSystemInfo == null)
                    {
                        //add the MAC to configdata

                        objCert.AddMAC(strSystemSerialNumber, objUserInfo.Brand);
                    }

                }
            }//ends alcatelPArtId - MM check ends
      
            dsSystemInfo = objCert.GetCertInfo(strSystemSerialNumber);
            if ((dsSystemInfo == null) && (!dt.Rows[i][Certificate.RFP].ToString().Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString()) || dt.Rows[i][Certificate.IS_ARUBA_RFP].ToString().Contains("YES")))
            {
                dt.Rows[i][Certificate.SYS_ERROR] = Certificate.NO_SYS_INFO;
                hasErrors = true;
                continue;
            }
            else
            {
                if (dt.Rows[i][Certificate.RFP].ToString() == ConfigurationManager.AppSettings["RFP_TYPE"].ToString() && dt.Rows[i][Certificate.IS_ARUBA_RFP].ToString() != "YES" && dsSystemInfo == null)
                {
                    if (dt.Rows[i][Certificate.LIC_PART_ID].ToString().Contains(ConfigurationManager.AppSettings["RFPSTD"].ToString()))
                    {
                        dt.Rows[i][Certificate.SYS_PART_ID] = "RFP-Mobile";
                        dt.Rows[i][Certificate.SYS_PART_DESC] = "RFP Mobile Server";
                    }
                    else
                    {
                        dt.Rows[i][Certificate.SYS_PART_ID] = "RFP-Server";
                        dt.Rows[i][Certificate.SYS_PART_DESC] = "RFP Server";
                    }
                    pk_cert_id = objCert.AddMacAddress(dt.Rows[i][Certificate.SYS_PART_ID].ToString(), dt.Rows[i][Certificate.SYS_PART_DESC].ToString(), strSystemSerialNumber, Session["BRAND"].ToString());
                    dt.Rows[i][Certificate.SYS_ID] = pk_cert_id;
                    dt.Rows[i][Certificate.SYS_SERIAL_NUMBER] = strSystemSerialNumber;
                }
                else
                {
                    dt.Rows[i][Certificate.SYS_ID] = Int32.Parse(dsSystemInfo.Tables[0].Rows[0]["pk_cert_id"].ToString());
                    dt.Rows[i][Certificate.SYS_PART_ID] = dsSystemInfo.Tables[0].Rows[0]["part_id"].ToString();
                    dt.Rows[i][Certificate.SYS_PART_DESC] = dsSystemInfo.Tables[0].Rows[0]["Part_desc"].ToString();
                    dt.Rows[i][Certificate.SYS_SERIAL_NUMBER] = strSystemSerialNumber;
                }
            }
            dt.Rows[i][Certificate.LOC] = strLocation;
            int SysId = Int32.Parse(dt.Rows[i][Certificate.SYS_ID].ToString());
            int CertId = Int32.Parse(dt.Rows[i][Certificate.LIC_ID].ToString());
            if (daoCert.IsCertSystemActivated(CertId, SysId))
            {
                dt.Rows[i][Certificate.SYS_ERROR] = Certificate.YES_ACTIVATED_SYSTEM;
                hasErrors = true;
                continue;
            }
            // for upgrades
            //int SysId = Int32.Parse((dt.Rows[i][Certificate.SYS_ID]).ToString());
            string UpgradedPartId;
            UpgradedPartId = daoCert.IsControllerUpgraded(SysId);
            if (! (string.Empty).Equals(UpgradedPartId))
            {
                dt.Rows[i][Certificate.SYS_PART_ID] = UpgradedPartId.ToString();
            }
            // Upgrades are done.

            if (dt.Rows[i][Certificate.RFP].ToString() != ConfigurationManager.AppSettings["RFP_TYPE"].ToString() || dt.Rows[i][Certificate.IS_ARUBA_RFP].ToString() == "YES")
            {
                if (!daoCert.IsCertSystemCompatible(dt.Rows[i][Certificate.LIC_PART_ID].ToString(), dt.Rows[i][Certificate.SYS_PART_ID].ToString(), Session["BRAND"].ToString()))
                {
                    dt.Rows[i][Certificate.SYS_ERROR] = Certificate.NO_CERT_SYSTEM_COMPATIBLE;
                    hasErrors = true;
                    continue;
                }
            }

            //get activation code
            string strActivationCode = dt.Rows[i][Certificate.ACTIVATION_KEY].ToString();
            if ((string.Empty).Equals(strActivationCode))
            {
                //Check for RFP Parts
            //RFP-1%%
            if (dt.Rows[i][Certificate.RFP].ToString() == ConfigurationManager.AppSettings["RFP_TYPE"].ToString() && dt.Rows[i][Certificate.IS_ARUBA_RFP].ToString() != "YES")
            {
                KeyGenInput objKeyGenIp = new KeyGenInput(dt.Rows[i][Certificate.LIC_SERIAL_NUMBER].ToString(),
                                 dt.Rows[i][Certificate.SYS_SERIAL_NUMBER].ToString(),objUserInfo.Brand);
                strActivationCode = objCert.GenerateRFPAcgtivationKey(objKeyGenIp);
                
            }
            // Activation of RFP Parts are done here.
            else
            {
                if ((dt.Rows[i][Certificate.LIC_ALC_PART_ID_3EM].ToString() != string.Empty) && (isMac = true) && (dt.Rows[i][Certificate.FORCE_SN_MAC].ToString() == "FORCE_MAC"))
                {
                    KeyGenInput objKeyGenIp = new KeyGenInput(dt.Rows[i][Certificate.LIC_SERIAL_NUMBER].ToString(),
                                                  strSystemMac,objUserInfo.Brand);
                    strActivationCode = objCert.GenerateActivationKey(objKeyGenIp);
                    System.Threading.Thread.Sleep(1500);
                }
                else
                {
                    KeyGenInput objKeyGenIp = new KeyGenInput(dt.Rows[i][Certificate.LIC_SERIAL_NUMBER].ToString(),
                                                                      dt.Rows[i][Certificate.SYS_SERIAL_NUMBER].ToString(),
                                                                       objUserInfo.Brand);
                    strActivationCode = objCert.GenerateActivationKey(objKeyGenIp);
                    System.Threading.Thread.Sleep(1000);
                }
            }

            }


            if (string.Empty.Equals(strActivationCode) || (strActivationCode == null))
            {
                dt.Rows[i][Certificate.SYS_ERROR] = Certificate.FAILURE_KEYGEN;
                new Log().logSystemError(method, Certificate.FAILURE_KEYGEN + ":CertId: " + CertId.ToString() + " SystemId:" + SysId.ToString());
                hasErrors = true;
                continue;

            }
            else
            {
                dt.Rows[i][Certificate.ACTIVATION_KEY] = strActivationCode;
            }

        }

        Session["CERT_INFO"] = dt;

        //do final check to see if all rows have activation key
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            strSerialNumber = strSerialNumber + "," + dt.Rows[i][Certificate.SYS_SERIAL_NUMBER].ToString();
            if (string.Empty.Equals(dt.Rows[i][Certificate.ACTIVATION_KEY].ToString()))
            {
                lblDbError.Visible = true;
                lblDbError.Text = Certificate.FAILURE_KEYGEN;
                hasErrors = true;
            }
        }
        if (hasErrors)
        {
            rptCert.DataSource = dt;
            rptCert.DataBind();
        }
        else
        {
            //string strtype = new Lookup().GetLookupText(objUserInfo.GetUserRole(), Lookup.CERT_TYPE); //need to check with ffang
            string strtype = CertType.AccountCertificate;
            if (objUserInfo.GetUserRole().Equals(UserType.Distributor))
                
                strtype = CertType.DistributorCertificate;
                

            if (objCert.AddActivationInfo(dt, objUserInfo.GetUserAcctId(), strtype))
            {
                if (objCert.UpdateLocation(dt, objUserInfo.GetUserAcctId()))
                {
                    LoadWizStepLast(dt);
                }
                else
                {
                    setError(Certificate.PERSISTENCE_ISSUE);
                }
            }
            else
            {
                setError(Certificate.PERSISTENCE_ISSUE);
            }
        }
        
    }      
        
  
    protected void cvCertBoxes_OnServerValidate(object sender, ServerValidateEventArgs args)
    {
        string temp = txtCertificate1.Text.Trim() + txtCertificate2.Text.Trim() +
         txtCertificate3.Text.Trim() + txtCertificate4.Text.Trim() +
         txtCertificate5.Text.Trim() + txtCertificate6.Text.Trim();
        if (temp.Equals(string.Empty))
        {
            args.IsValid = false;
            return;
        }
        args.IsValid = true;
    
    }

    private bool addCertRow(DataRow dr, TextBox txt,int intRowNum)
    {
        if (txt.Text.Trim().Equals(string.Empty))
            return false;
        else
        {
            dr[Certificate.LIC_SERIAL_NUMBER] = txt.Text.Trim();
            dr[Certificate.ROW_NO] = intRowNum;
            

        }
        return true;
    
    }

    private void clearAllCertControls()
    {
        WizardStepBase parent = wizActivate.ActiveStep;
        for (int i = 1; i < 7; i++)
        {
            ((TextBox)parent.FindControl("txtCertificate" + i.ToString())).Text = string.Empty;
            ((Label)parent.FindControl("lblErr" + i.ToString())).Text = string.Empty;
        
        }
    
    }

    private void LoadWizStepLast(DataTable dt)  
    {
        wizActivate.ActiveStepIndex = 2;
        rptrActInfo.DataSource = dt;
        rptrActInfo.DataBind();

        ////Call send Email API
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            UserInfo objUser = (UserInfo)Session["USER_INFO"];
            CertificateMailInfo objCertMailInfo = new CertificateMailInfo();
            objCertMailInfo.ActivationKey = dt.Rows[i][Certificate.ACTIVATION_KEY].ToString();
            objCertMailInfo.Brand = Session["BRAND"].ToString().ToUpper();
            objCertMailInfo.CertId = dt.Rows[i][Certificate.LIC_SERIAL_NUMBER].ToString();
            objCertMailInfo.CertPartDesc = dt.Rows[i][Certificate.LIC_PART_DESC].ToString();
            objCertMailInfo.CertPartId = dt.Rows[i][Certificate.LIC_PART_ID].ToString();
            objCertMailInfo.Email = objUser.GetUserEmail();
            objCertMailInfo.SysPartDesc = dt.Rows[i][Certificate.SYS_PART_DESC].ToString();
            objCertMailInfo.SysPartId = dt.Rows[i][Certificate.SYS_PART_ID].ToString();
            objCertMailInfo.SysSerialNumber = dt.Rows[i][Certificate.SYS_SERIAL_NUMBER].ToString();
            Email objEmail = new Email();
            objEmail.sendCertificateActivationInfo(objCertMailInfo);
        }
    }

    private void setError(string text)
    {
        lblDbError.Text=text;
        lblDbError.Visible=true;
    }

    private string IsSameVersionCertOnCtrl(DataTable dt)
    {
        ArrayList strSerialNo = new ArrayList();
        Certificate objCert = new Certificate();
        string strResult = string.Empty;
        string strVersion = string.Empty;
        for (int i = 0; i < rptCert.Items.Count; i++)
        {
            //string strCertId = string.Empty;            
            //strCertId = dt.Rows[i][Certificate.LIC_SERIAL_NUMBER].ToString();
            strVersion = dt.Rows[i][Certificate.VERSION].ToString();
            if (!strVersion.Contains("3.5"))
            {
                strSerialNo.Add(((TextBox)rptCert.Items[i].FindControl("txtSNMac")).Text.Trim() + "," + strVersion);
            }
        }
        for (int j = strSerialNo.Count-1; j >= 0; j--)
        {
            for (int k = 0; k < j; k++)
            {
                if (strSerialNo[j].ToString() != strSerialNo[k].ToString())
                {
                    strResult = "You are trying to activate both 3.0 and 5.0 certificates on the same Controller!. Do You still want to Continue?";
                }
            }
        }
        return strResult;
    }
}

