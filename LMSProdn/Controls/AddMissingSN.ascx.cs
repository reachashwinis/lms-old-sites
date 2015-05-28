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
using System.Web.SessionState;

public partial class Controls_AddMissingSN : System.Web.UI.UserControl
{

    DataSet dsLookupValues;
    private string ERR_SERIALNUM_LEN = "The serial number must be 9 or 10 characters";
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

            UIHelper.PrepareAndBindLookupFromAllWithoutPlease(ddlPartId, dsLookupValues.Tables[0], "ADD_HW", "TXT", "VAL", true);

            if (ConfigurationManager.AppSettings["SPL_PRIV"].Contains(objUserInfo.GetUserEmail()))
            {
                chkOverride.Visible = true;
                chkOverride.Checked = false;

            }
        }
    }

    protected void btnAddMissingSn_OnClick(object sender, EventArgs args)
    {
        string strERROR = string.Empty;
        string strSUCCESS = string.Empty;
        string CustCode = txtCustCode.Text.Trim().ToUpper();
        string SerialNum = txtSNum.Text.Trim().ToUpper();
        string PartId = ddlPartId.SelectedValue.Trim().ToUpper();
        string PartDesc = string.Empty;
        DataSet dsCertInfo = new DataSet();
        
        if (!Page.IsValid)
            return;

        if (SerialNum.Length > 8 && SerialNum.Length < 11 && PartId.Length > 0 && CustCode.Length > 0)
        {
            Certificate objCertificate = new Certificate();

            DataSet ds = objCertificate.GetCertInfo(SerialNum);
            if (ds != null)
                strERROR += "The serial number " + SerialNum + "  already exists in the LMS database<BR>";


            if (strERROR.Trim().Length == 0 || chkOverride.Checked == true)
            {
                if (!objCertificate.AddQACertificate(PartId, PartDesc, SerialNum, CustCode, objUserInfo.GetUserEmail(), "99999", objUserInfo.GetUserEmail(), Request.ServerVariables["REMOTE_ADDR"], "AddMissingSN.aspx", Session["BRAND"].ToString(),string.Empty))
                {
                    setError("Unable to add Serial Number to LMS", lblErr);
                    return;
                }
                strSUCCESS += "Serial number " + SerialNum + " was added to LMS database<BR>";
            }

        }
        else 
        {
            strERROR += "All fields are mandatory";
        }

        setError(strERROR, lblErr);
        setError(strSUCCESS, lblSuccess);

    }

    private void setError(string text, Label lbl)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }

    protected void cvSerialNumber_OnServerValidate(object sender, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (txtSNum.Text.Trim().Length < 9 || txtSNum.Text.Trim().Length > 10)
        {
            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = ERR_SERIALNUM_LEN;
        }

    }

}
