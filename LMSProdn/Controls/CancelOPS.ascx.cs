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

public partial class Controls_CancelOPS : System.Web.UI.UserControl
{
    UserInfo objUserInfo;
    Lookup objLookup;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Visible = false;
        lblSuccess.Visible = false;
        btnConfirm.Visible = false;
        objUserInfo = (UserInfo)(Session["USER_INFO"]);
        if (ConfigurationManager.AppSettings["SPL_PRIV"].Contains(objUserInfo.GetUserEmail()))
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
        if (!Page.IsValid)
            return;

        Certificate objCertificate = new Certificate();
        DataSet dsTemp;
        string sysId = string.Empty;
        string sysSerialNumber = string.Empty;
        hdnCertId.Value = string.Empty;
        string[] arr;
        string strSNum = txtSNum.Text.Trim();
        string strNoSerialNo = string.Empty;

        if (strSNum.ToString() != string.Empty)
        {

             arr = strSNum.Split(',');
             for (int i = 0 ; i < arr.Length ; i++)
            {
                if (arr[i].Length < 10)
                    arr[i] = arr[i].ToUpper();

                if (arr[i].Length > 0)
                {
                    dsTemp = objCertificate.GetOPCertInfo(arr[i]);
                    if (dsTemp == null)
                    {
                        strNoSerialNo = strNoSerialNo + arr[i] + ",";
                    }
                    else
                    {
                        hdnCertId.Value += arr[i] + ",";
                    }
                }
            }
            if (strNoSerialNo.EndsWith(","))
            {
                int occ = strNoSerialNo.LastIndexOf(',');
                strNoSerialNo = strNoSerialNo.Substring(0, occ);
            }
            string hdnValue = hdnCertId.Value;
            if (hdnValue.EndsWith(","))
            {
                int occ = hdnValue.LastIndexOf(',');
                hdnValue = hdnValue.Substring(0, occ);
                hdnCertId.Value = hdnValue;
            }
             
            if (strNoSerialNo.Length > 0)
            {

                if (chkOverride.Checked == true)
                {
                    btnCancel.Visible = false;
                    btnConfirm.Visible = true;
                }
                else
                {    
                    strERROR = strNoSerialNo + " serial number does not exist in OPS Activations<BR>";
                }

             }
            else
            {
                strSUCCESS = hdnCertId.Value + " Serial Number found. Please confirm to continue.<BR>";
                txtSNum.ReadOnly = true;
                btnConfirm.Visible = true;
                btnCancel.Visible = false;
            }
                  
        }
        else
        {
           strERROR += "Serial Numbers are mandatory";
        }

            setError(strERROR, lblError);
            setError(strSUCCESS, lblSuccess);
 }

    protected void btnConfirm_OnClick(object sender, EventArgs args)
    {

        btnCancel.Visible = false;
        string strERROR = string.Empty;
        string strSUCCESS = string.Empty;
        string strSuccSlno = string.Empty;
        string strFailSlno = string.Empty;
        if (!hdnCertId.Value.Trim().Equals(string.Empty))
        {

            Certificate objCert = new Certificate();
            string[] arr;
            string hdnValue = hdnCertId.Value;
            arr = hdnValue.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                if (objCert.CancelOPSActivations(arr[i].Trim().ToString(), objUserInfo.GetUserEmail()))
                {
                    strSuccSlno = strSuccSlno + arr[i] + ",";
                }
                else
                {
                    strFailSlno = strFailSlno + arr[i] + ",";
                }
            }
            if (strSuccSlno != string.Empty)
            {
                if (strSuccSlno.EndsWith(","))
                {
                    int occ = strSuccSlno.LastIndexOf(',');
                    strSuccSlno = strSuccSlno.Substring(0, occ);
                }
                strSUCCESS += strSuccSlno + " Serial numbers removed from OPSActivations";
            }
            else
            {
                if (strFailSlno.EndsWith(","))
                {
                    int occ = strFailSlno.LastIndexOf(',');
                    strFailSlno = strFailSlno.Substring(0, occ);
                }
                strERROR += strFailSlno + " Serial numbers were Unable to remove";
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
