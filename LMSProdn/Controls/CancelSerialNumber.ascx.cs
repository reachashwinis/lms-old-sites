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

public partial class Controls_CancelSerialNumber : System.Web.UI.UserControl
{
    UserInfo objUserInfo;
    int sNumCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Visible = false;
        lblSuccess.Visible = false;
        btnConfirm.Visible = false;
        objUserInfo = (UserInfo)(Session["USER_INFO"]);
        if (!this.IsPostBack && ConfigurationManager.AppSettings["SPL_PRIV"].Contains(objUserInfo.GetUserEmail()))
        {
            chkOverride.Visible = true;
            chkOverride.Checked = false;
        }
    }

    protected void cvReason_OnServerValidate(object sender, ServerValidateEventArgs args)
    {
        string val = args.Value.Trim();
        string strErr = string.Empty;
        args.IsValid = true;
        if (val.Length == 0)
        {
            strErr = rfvReason.ErrorMessage;
            args.IsValid = false;
        }
        else if (val.Length > 1401)
        {
            strErr = "You can enter atmost 1400 characters";
            args.IsValid = false;
        }

        ((CustomValidator)sender).ErrorMessage = strErr;

    }
    protected void btnCancel_OnClick(object sender, EventArgs args)
    {
        string strERROR = string.Empty;
        string strSUCCESS = string.Empty;
        string strSNum = txtSNum.Text.Trim();
        string Oldpkcertid = string.Empty;
        if (!Page.IsValid)
            return;

        Certificate objCertificate = new Certificate();
        DataSet dsTemp;
        string sysId = string.Empty;
        string sysSerialNumber = string.Empty;
       

        hdnCertId.Value = string.Empty;
        
        if (strSNum.Length < 10)
            strSNum = strSNum.ToUpper();

        if (strSNum.Length > 0 && txtReason.Text.Trim().Length > 0)
        {
            //Added by ashwini
            //string strPartID = objCertificate.GetPartID(strSNum.ToUpper());
            //if (strPartID == string.Empty)
            ////if (strPartID.Contains("3EM") && Session["BRAND"].ToString() != ConfigurationManager.AppSettings["ALCATEL_BRAND"].ToString())
            //{
            //    strERROR += "Serial Number doesnt exists!!<BR>";
            //    setError(strERROR, lblError);
            //    return;
            //}
            //get certificate Id from eng Web if its a L********* id
            if (strSNum.Substring(0, 1).Equals("L"))
            {
                dsTemp = objCertificate.GetSerialNumberCertifcateMap(strSNum);
                if (dsTemp != null)
                {
                    txtSNum.Text = dsTemp.Tables[0].Rows[0]["certificate_id"].ToString();
                }
                else
                {
                    strERROR += "Unable to locate Part Map Entry<BR>";
                }
            }

            dsTemp = objCertificate.GetCertInfo(txtSNum.Text);
            if (dsTemp == null)
            {
                strERROR += "The serial number/certificate Id does not exist in LMS<BR>";
            }
            else
            {
                sNumCount = dsTemp.Tables[0].Rows.Count;
                for (int rInd = 0; rInd < dsTemp.Tables[0].Rows.Count; rInd++)
                {
                    Oldpkcertid += dsTemp.Tables[0].Rows[rInd]["pk_cert_id"].ToString() + ",";
                }
                Oldpkcertid = Oldpkcertid.Substring(0, Oldpkcertid.Length - 1);

                dsTemp = objCertificate.CheckActivationInfo(Oldpkcertid);
                if (dsTemp != null)
                {
                    if (sNumCount > 1)
                    {
                        strERROR += "There are multiple occurrences of the serial number/certificate and one or more of them has already been activated.<BR>";
                    }
                    else
                    {
                        strERROR += "The serial number/certificate has already been activated.";
                        //sysId = dsTemp.Tables[0].Rows[0][Certificate.SYS_ID].ToString();
                        //dsTemp = objCertificate.GetCertInfo(Int32.Parse(sysId));
                        //if (dsTemp != null)
                        //{
                        //    sysSerialNumber = dsTemp.Tables[0].Rows[0]["serial_number"].ToString();
                        //    string certType = dsTemp.Tables[0].Rows[0]["certType"].ToString();
                        //    if (sysSerialNumber.Length < 10 && certType == "CERT")
                        //    {
                        //        strERROR += "<BR>The certificate is tied to S/N: " + sysSerialNumber + " Part: " + dsTemp.Tables[0].Rows[0]["part_desc"].ToString() + "<BR>";
                        //    }
                        //}
                    }

                }

                if (strERROR.Length > 0)
                {
                    if (chkOverride.Checked == true && sNumCount == 1)
                    {
                        hdnCertId.Value = Oldpkcertid;
                        btnCancel.Visible = false;
                        btnConfirm.Visible = true;                   
                    }
                    else if (chkOverride.Checked == true && sNumCount > 1)
                    {
                        hdnCertId.Value = objCertificate.getOldestSerialNo(Oldpkcertid).ToString();
                        btnCancel.Visible = false;
                        btnConfirm.Visible = true; 
                    }
                    chkOverride.Visible = true;
                 }
                else
                {
                    hdnCertId.Value = objCertificate.getOldestSerialNo(Oldpkcertid).ToString();
                    strSUCCESS += "Serial Number/Certificate ID found. Please confirm to continue.<BR>";
                    txtSNum.ReadOnly = true;
                    btnConfirm.Visible = true;
                    btnCancel.Visible = false;
                }
            }

        }
        else
        {
            strERROR += "All fields are mandatory";
        }

        setError(strERROR, lblError);
        setError(strSUCCESS, lblSuccess);
     }

    protected void btnConfirm_OnClick(object sender, EventArgs args)
    {
        
        btnCancel.Visible = false;
        string strERROR = string.Empty;
        string strSUCCESS = string.Empty;
        string strFru = string.Empty;
        string strCertpart = string.Empty;
        bool blUpgrade = false;
        DACertificate daoCert = new DACertificate();

        if (!hdnCertId.Value.Trim().Equals(string.Empty))
        {
            Certificate objCert = new Certificate();
            //update Upgrade status
            DataSet dsCert = objCert.GetCertInfo(Int32.Parse(hdnCertId.Value.Trim()));
            strCertpart = dsCert.Tables[0].Rows[0]["part_id"].ToString();
            strFru = dsCert.Tables[0].Rows[0]["fru"].ToString();
            if (daoCert.isUpgradebleCert(strCertpart))
            {
                blUpgrade = true;
            }

            if (objCert.CancelSerialNumber(Int32.Parse(hdnCertId.Value.Trim()), objUserInfo.GetUserEmail(), txtReason.Text.Trim()))
                {
                    strSUCCESS += "The Serial number/Certificate is removed from LMS";
                    if (blUpgrade == true && strFru != string.Empty)
                    {
                        if (!objCert.UpdateUpgradeStat(strFru, string.Empty))
                        {
                            //send mail
                            Email objEmail = new Email();
                            objEmail.UpdateFailureInfo(hdnCertId.Value.Trim(), ConfigurationManager.AppSettings["BCC_MAIL"].ToString());
                        }
                    }
                }
                else
                {
                    strERROR += "Unable to remove the serial number/certificate<BR>";
                }
        }
        else
        {
            strERROR += "Certificate ID missing";
        }

        setError(strERROR, lblError);
        setError(strSUCCESS, lblSuccess);



    }
    private void setError(string text, Label lbl)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }

}
