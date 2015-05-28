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


public partial class Controls_GenerateQALic : System.Web.UI.UserControl
{
    DataSet dsLookupValues;
    private string ERR_SERIALNUM_LEN="The serial number must be 9 or 10 characters";
    UserInfo objUserInfo;
    Lookup objLookup;

    protected void Page_Load(object sender, EventArgs e)
    {        
        
    }
    protected void ddlPartId_SelectedIndexChanged(object sender, EventArgs args)
    {
        
    }

    protected void btnGenQALic_OnClick(object sender, EventArgs args)
    {
        
    }

    private void myAsynCallBackComplete(IAsyncResult result)
    {

    }

    protected DataSet LoadCertParts(string strPartId)
    {
        DataSet ds = new Lookup().LoadCertParts(strPartId);
        return ds;
    }

    protected DataSet LoadCertParts(string strPartId, string strVersion)
    {
        DataSet ds = new Lookup(). LoadCertPartsQA(strPartId, strVersion,"");
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

    protected void ddlVersion_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}



