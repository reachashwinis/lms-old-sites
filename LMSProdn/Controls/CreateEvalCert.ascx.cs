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
using System.Drawing;
using System.Web.Caching;
using System.Collections.Generic;

public partial class Controls_CreateEvalCert : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible=false;
        string brand = ((UserInfo)Session["USER_INFO"]).Brand;
        if (!IsPostBack)
        {
            //load eval list
           Lookup objLookup = new Lookup();
           string strVersion = GetCertTypeToView();
           DataSet dsEvalparts = objLookup.LoadEvalParts(brand,strVersion);
           UIHelper.PrepareAndBindListWithoutPlease(ddlEvalPart, dsEvalparts.Tables[Lookup.EVALPARTS_TBL], "MIX", "VAL", true); 
        }
    }
    protected void btnCreate_OnClick(object sender, EventArgs args)
    {
        if (!Page.IsValid)
            return;
        bool retVal;
        retVal = false;
        UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
        Certificate objCert = new Certificate();
        KeyGenInput obgKGIp = new KeyGenInput(ddlEvalPart.SelectedValue, ((UserInfo)Session["USER_INFO"]).Brand);
        string serialNumber = objCert.GenerateEvalCertificate(obgKGIp);
        if(serialNumber.Equals(string.Empty))
        {
        setError(lblErr,Certificate.FAILURE_EVALCERT);
        return;
        }
        else
        {
        objCert.AddEvalCertificate(ddlEvalPart.SelectedValue,serialNumber,txtSoldTo.Text,Session["BRAND"].ToString(),ConfigurationManager.AppSettings["CERTTYPE"].ToString());
        //send email notification to email address of recipient
        // Added by Ashwini
        CertificateMailInfo objcgInfo = new CertificateMailInfo();
        objcgInfo.Brand = (Session["BRAND"].ToString().ToUpper());
        objcgInfo.CertId = serialNumber;
        objcgInfo.CertPartDesc = ddlEvalPart.SelectedItem.ToString();
        objcgInfo.CertPartId = ddlEvalPart.SelectedValue;
        objcgInfo.Email = txtEmail.Text;
        objcgInfo.Name = txtSoldTo.Text;
        objcgInfo.CCEmail = objUserInfo.Email;
        Email objEmail = new Email();
        retVal = objEmail.sendEvalCertInfo(objcgInfo);
        LblMail.Text = string.Empty;
        LblMail.Visible = true;
        LblCertId.Visible = true;
        LblCaption.Visible = true;
        LblCaption.Text = "Eval Certificate ID";
        LblCertId.Text = serialNumber.ToString();
        if (retVal == true)
        {
            LblMail.Text = "email is sent to " + objcgInfo.Email;
        }
       }
    
    }
    protected void cvEmail_OnValidate(object sender, ServerValidateEventArgs args)
    {
        //User modUser = new User();
        //if (modUser.GetUserInfo(txtEmail.Text.Trim(),Session["BRAND"].ToString()) == null)
        //    args.IsValid = false;
        //else
        //    args.IsValid = true;
    
    }

     private void setError(Label lbl, string text)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }
    protected void ddlCertVersion_SelectedIndexChanged(object sender, EventArgs e)
    {
        Lookup objLookup = new Lookup();
        string strVersion = GetCertTypeToView();
        DataSet dsEvalparts = objLookup.LoadEvalParts(Session["BRAND"].ToString(), strVersion);
        UIHelper.PrepareAndBindListWithoutPlease(ddlEvalPart, dsEvalparts.Tables[Lookup.EVALPARTS_TBL], "MIX", "VAL", true);
        if (ddlCertVersion.Text.ToUpper().Contains("POST"))
            setError(LblCaption, "Certificates Which can activated on controller running on AOS5.0");
        else
            setError(LblCaption, "Certificates Which Can be activated on controller running on AOS3x");
    }

    private string GetCertTypeToView()
    {
        string strViewCerts = string.Empty;
        switch (ddlCertVersion.SelectedValue)
        {
            case "PRE":
                strViewCerts = " version like '%3%'";
                break;
            case "POST":
                strViewCerts = " version like '%5%'";
                break;
        }
        return strViewCerts;
    }
}
