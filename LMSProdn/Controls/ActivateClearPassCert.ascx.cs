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

public partial class Controls_ActivateClearPassCert : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {       
        Certificate objCert = new Certificate();
        string strSubscriptionKey = string.Empty;
        Email objEmail = new Email();        
        Subscription objSubscription = new Subscription();
        DataSet dsSub = new DataSet();
        DataTable dt = new DataTable();
        strSubscriptionKey = Session["subkey"].ToString();
        try
        {
            if (!IsPostBack)
            {
                lblDbError.Visible = false;
                //Check if subscription ID is valid.
                if (objCert.IsAmigopodSubscription(strSubscriptionKey))
                {
                    setError(LblImportError, "This Subscription ID is of legacy one. Please generate the new Subscription ID from <b> Generate Subscription ID </b> from <b> ClearPass(Legacy Guest) Certificates	 </b> module.");
                    return;
                }
                else
                {
                    dt = FetchClsSubscriptionDetails(strSubscriptionKey);
                    dsSub.Tables.Add(dt.Copy());
                    LoadWizStep2(dsSub);
                }
            }
        }
        catch (Exception ex)
        {
            new Log().logException("FetchClsSubscriptionDetails", ex);
            wizActivate.ActiveStepIndex = 0;
            setError(LblImportError, "Application could not process your request.The error occurred has been logged");
            objEmail.sendAmigopodErrorMessage(ex.Message, "FetchClsSubscriptionDetails", "", strSubscriptionKey);
        }
    }

    protected void btnNext_OnClick(object sender, EventArgs args)
    {
        //Page.Validate();
        if (!Page.IsValid)
            return;
        DataSet ds = (DataSet)Session["SUB_INFO"];
        DataTable dtSubDet = new DataTable();
        dtSubDet = ds.Tables[0];
        dtSubDet.TableName = "SUBINFO";
        if (dtSubDet == null || dtSubDet.Rows.Count == 0)
        {
            wizActivate.ActiveStepIndex = 0;
            return;
        }

        DataSet dsCertInfo = new DataSet();
        Certificate objCert = new Certificate();
        DataTable dt = objCert.BuildCertInfo();
        dt.TableName = "CERTINFO";
        string strQty = string.Empty;
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
        if (dt.Rows.Count > 1)
        {
            for (int oInd = dt.Rows.Count - 1; oInd >= 0; oInd--)
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
                //Get the cert details from certGen db
                DataSet dsClp = objCert.getClpCertFromCertGen(dt.Rows[j][Certificate.LIC_SERIAL_NUMBER].ToString(), Session["BRAND"].ToString());
                if (dsClp.Tables[0].Rows.Count <= 0)
                {
                    dt.Rows[j][Certificate.LIC_ERROR] = Certificate.NO_CERT_INFO;
                    hasErrors = true;
                    continue;
                }

                dsCertInfo = objCert.GetClpCertificateInfo(dt.Rows[j][Certificate.LIC_SERIAL_NUMBER].ToString());
                if (dsCertInfo == null)
                {
                    dt.Rows[j][Certificate.LIC_ERROR] = Certificate.NO_CERT_INFO;
                    hasErrors = true;
                    continue;
                }
                else if (dsCertInfo.Tables[0].Rows[0]["part_id"].ToString().StartsWith("QC"))
                {
                    dt.Rows[j][Certificate.LIC_ERROR] = "This is QuickConnect certificate. Please activate this using <i>Add Quick Connect </i>";
                    hasErrors = true;
                    continue;
                }

                dt.Rows[j][Certificate.LIC_ID] = Int32.Parse(dsCertInfo.Tables[0].Rows[0]["pk_cert_id"].ToString());
                dt.Rows[j][Certificate.LIC_SERIAL_NUMBER] = dsCertInfo.Tables[0].Rows[0]["serial_number"].ToString();
                dt.Rows[j][Certificate.ACTIVATION_KEY] = dsCertInfo.Tables[0].Rows[0]["activation_code"].ToString();
                dt.Rows[j][Certificate.LIC_PART_ID] = dsCertInfo.Tables[0].Rows[0]["part_id"].ToString();
                dt.Rows[j][Certificate.LIC_PART_DESC] = dsCertInfo.Tables[0].Rows[0]["part_desc"].ToString();
                dt.Rows[j][Certificate.LOC] = dsCertInfo.Tables[0].Rows[0]["Location"].ToString();
                dt.Rows[j][Certificate.VERSION] = dsCertInfo.Tables[0].Rows[0]["version"].ToString();

                if (!dt.Rows[j][Certificate.ACTIVATION_KEY].ToString().Equals(string.Empty))
                {
                    dt.Rows[j][Certificate.LIC_ERROR] = Certificate.YES_ACTIVATED;
                    hasErrors = true;
                    continue;
                }

            }
        }

        //Session["SUB_INFO"] = dt;
        DataSet DsSubInfo = new DataSet();
        DsSubInfo.Tables.Add(dtSubDet.Copy());
        DsSubInfo.Tables.Add(dt.Copy());

        if (hasErrors == true)
        {
            clearAllCertControls();
            WizardStepBase parent = wizActivate.ActiveStep;
            for (int rIndex = 1; rIndex <= dt.Rows.Count; rIndex++)
            {
                ((TextBox)parent.FindControl("txtCertificate" + rIndex.ToString())).Text = dt.Rows[rIndex - 1][Certificate.LIC_SERIAL_NUMBER].ToString();
                ((Label)parent.FindControl("lblErr" + rIndex.ToString())).Text = dt.Rows[rIndex - 1][Certificate.LIC_ERROR].ToString();
            }
            wizActivate.ActiveStepIndex = 1;
            return;
        }
        LoadWizStep4(DsSubInfo);
     
    }

    private void LoadWizStep2(DataSet ds)
    {
        Session["SUB_INFO"] = ds;
        wizActivate.ActiveStepIndex = 0;        
        rptSub2.DataSource = ds.Tables[0];
        LblSubkey3.Text = Session["subkey"].ToString();
        rptSub2.DataBind();
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["Error"].ToString().Contains("This Subscription ID is invalid"))
            {
                BtnImport.Enabled = false;
                LblImportError.Text = "The application could not process this Subscription ID. Please contact support for further help.";
                return;
            }
            else
            {
                BtnImport.Enabled = true;
            }
        }
        else
        {
            BtnImport.Enabled = false;
            LblImportError.Text = "The application could not process this Subscription ID. Please contact support for further help.";
            return;
        }
    }

    private void LoadWizStep3(DataSet ds)
    {
        Session["SUB_INFO"] = ds;
        //LblSubkey3.Text = txtSubKey.Text;
        LblSubkey3.Text = Session["subkey"].ToString();
        wizActivate.ActiveStepIndex = 1;        
    }

    private void LoadWizStep4(DataSet DsSubInfo)
    {
        Session["SUB_INFO"] = DsSubInfo;
        rptCert.DataSource = DsSubInfo.Tables["CERTINFO"];
        rptCert.DataBind();
        loadVersion(DsSubInfo.Tables["CERTINFO"]);
        Lblsubkey4.Text = Session["subkey"].ToString();        
        wizActivate.ActiveStepIndex = 2;
    }

    private void loadVersion(DataTable dtCert)
    {
        for (int i=0;i<dtCert.Rows.Count;i++)
        {
            if (dtCert.Rows[i]["Version"].ToString() == "5.6")
            {
              ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).Items.Clear();
              ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).Items.Add("6.X.X.X");
              ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).Items.Add("5.0.X.X");
              ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).SelectedValue = "6.X.X.X";
              ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).Enabled = true;
            }
            else if (dtCert.Rows[i]["Version"].ToString() == "5")
            {
              ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).Items.Clear();
              ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).Items.Add("5.0.X.X");
              ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).SelectedValue = "5.0.X.X";
              ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).Enabled = false;
            }
            else
            {
              ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).Items.Clear();
              ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).Items.Add("6.X.X.X");
              ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).SelectedValue = "6.X.X.X";
              ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).Enabled = false;
            }
        }
    }

    protected void btnActivate_OnClick(object sender, EventArgs args)
    {
        string method = "btnActivate_OnClick";
        UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
        Certificate objCert = new Certificate();
        bool hasErrors = false;
        string strCustId = string.Empty;
        string avenda_sku = "1";
        string strSubKey = string.Empty;

        DACertificate daoCert = new DACertificate();
        DataSet dsSubInfo = (DataSet)Session["SUB_INFO"];
        if (dsSubInfo == null || dsSubInfo.Tables.Count == 0)
        {
            wizActivate.ActiveStepIndex = 0;
            return;
        }

        DataTable dtCert = dsSubInfo.Tables["CERTINFO"];
        DataTable dtSub = dsSubInfo.Tables["SUBINFO"];

        strCustId = dtSub.Rows[0]["CustId"].ToString();
        strSubKey = dtSub.Rows[0]["SubscriptionKey"].ToString(); 

        for (int i = 0; i < rptCert.Items.Count; i++)
        {
            string strVersion = string.Empty;
            string strLocation = string.Empty;
            string strLicenseCount = string.Empty;
            string strLicenseType = string.Empty;
            string strExpYear = string.Empty;
            string strProductSKU = string.Empty;
            string strSKUId = string.Empty;
            string strProductName = string.Empty;
            AvendaXML objAvendaXML = new AvendaXML();
            DataSet DsCert = new DataSet();
            strVersion = ((DropDownList)rptCert.Items[i].FindControl("DrpListVersion")).SelectedValue.Trim();
            strLocation = ((TextBox)rptCert.Items[i].FindControl("txtLocation")).Text.Trim();
            dtCert.Rows[i][Certificate.LOC] = strLocation;
            dtCert.Rows[i][Certificate.VERSION] = strVersion;
            

            // Get the ClearPass SKU details from AmgLookinfo

            DataSet ds = objCert.GetAmigoppodLookInfo(dtCert.Rows[i]["LicSN"].ToString(), Session["BRAND"].ToString());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["part_type"].ToString() == "WORKSPACE")
                    {
                        DsCert = objCert.getClpCertFromCertGen(dtCert.Rows[i]["LicSN"].ToString(), Session["BRAND"].ToString());
                        int Qty1 = Int32.Parse(ds.Tables[0].Rows[0]["license_count"].ToString());
                        int Qty2 = Int32.Parse(DsCert.Tables[0].Rows[0]["Qty"].ToString());
                        int Qty = Qty1 * Qty2;
                        strLicenseCount = Qty.ToString();
                    }
                    else
                    {
                        strLicenseCount = ds.Tables[0].Rows[0]["license_count"].ToString();
                    }
                    strLicenseType = ds.Tables[0].Rows[0]["license_type"].ToString();
                    strExpYear = ds.Tables[0].Rows[0]["exp_year"].ToString();
                    strSKUId = ds.Tables[0].Rows[0]["sku_id"].ToString();
                    strProductName = ds.Tables[0].Rows[0]["product_name"].ToString();
                    strProductSKU = ds.Tables[0].Rows[0]["part_id"].ToString();
                }
                else
                {
                    dtCert.Rows[i][Certificate.LIC_ERROR] = "Unable to fetch ClearPass SKU details";
                    hasErrors = true;
                    continue;
                }
            }
            else
            {
                dtCert.Rows[i][Certificate.LIC_ERROR] = "Unable to fetch ClearPass SKU details";
                hasErrors = true;
                continue;
            }
            
            //get activation code
            string strActivationCode = dtCert.Rows[i][Certificate.ACTIVATION_KEY].ToString();
            if ((string.Empty).Equals(strActivationCode))
            {

                objAvendaXML = objCert.ActivateClpCert(strCustId, avenda_sku, strProductName, strSKUId, strSubKey, strVersion, strProductSKU, strLicenseCount, strLicenseType, dtCert.Rows[i]["LicSN"].ToString(), strExpYear);
            }

            if (string.Empty.Equals(objAvendaXML.license) && objAvendaXML.error != "0")
            {
                dtCert.Rows[i][Certificate.SYS_ERROR] = Certificate.FAILURE_KEYGEN;
                new Log().logSystemError(method, Certificate.FAILURE_KEYGEN + ":CertId: " + dtCert.Rows[i][Certificate.LIC_ID].ToString());
                hasErrors = true;
                continue;
            }
            else
            {
                dtCert.Rows[i][Certificate.ACTIVATION_KEY] = objAvendaXML.license;
            }
        }

        Session["SUB_INFO"] = dsSubInfo;

        //do final check to see if all rows have activation key
        for (int i = 0; i < dtCert.Rows.Count; i++)
        {
            if (string.Empty.Equals(dtCert.Rows[i][Certificate.ACTIVATION_KEY].ToString()))
            {
                lblDbError.Visible = true;
                lblDbError.Text = Certificate.FAILURE_KEYGEN;
                hasErrors = true;
            }
        }
        if (hasErrors)
        {
            rptCert.DataSource = dtCert;
            rptCert.DataBind();
        }
        else
        {
            if (objCert.AddClpActivationInfo(dtCert, objUserInfo.AcctId, objUserInfo.GetUserAcctId(), strSubKey))
            {
                LoadWizStepLast(dsSubInfo);
            }
            else
            {
                setError(LblImportError, Certificate.PERSISTENCE_ISSUE);
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

    private bool addCertRow(DataRow dr, TextBox txt, int intRowNum)
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

    private void LoadWizStepLast(DataSet DsSubAll)  
    {
        Lblsubkey5.Text = Session["subkey"].ToString();
        wizActivate.ActiveStepIndex = 3;
        rptrActInfo.DataSource = DsSubAll.Tables["CERTINFO"];
        rptrActInfo.DataBind();
        ////Call send Email API
        UserInfo objUser = (UserInfo)Session["USER_INFO"];
        Email objEmail = new Email();
        objEmail.sendClpCertActivationInfo(DsSubAll, objUser, Session["BRAND"].ToString().ToUpper());
    }

    private void setError( Label LblDbError, string text)
    {
        LblDbError.Text = text;
        LblDbError.Visible = true;
    }

    private DataTable FetchClsSubscriptionDetails(string strSubscriptionKey)
    {
         //Fetch all the information of subscription ID.
        Certificate objCert = new Certificate();
        Subscription objSubscription = new Subscription();
        DataTable dt = objCert.BuildAmgCertInfo();
        DataRow dr = dt.NewRow();
        objSubscription = objCert.GetDetailsAmigopodSubscription(strSubscriptionKey, "1");

        dr["SubscriptionKey"] = objSubscription.subscription_key;
        dr["Organization"] = objSubscription.name;
        dr["po_id"] = objSubscription.po;
        dr["so_id"] = objSubscription.so;
        dr["expire_time"] = objSubscription.expiretime;
        dr["user_name"] = objSubscription.username;
        dr["password"] = objSubscription.password;
        dr["email"] = objSubscription.email;
        dr["create_time"] = objSubscription.create_time;
        dr["category"] = objSubscription.category;
        dr["SkuId"] = objSubscription.sku_id;
        dr["CustId"] = objSubscription.cust_id;
        dt.Rows.Add(dr);
        if (objSubscription.expiretime == null || objSubscription.expiretime == string.Empty)
        {
            dr["Error"] = "This Subscription ID is invalid";                
        }
        else
        {
            if (Convert.ToDateTime(dr["expire_time"].ToString()) <= System.DateTime.Today)
            {
                dr["Error"] = "This Subscription ID has crossed the expiry date of support. To extend the expiration, please contact your local Aruba Networks representative, or contact Aruba Support at <A>support@arubanetworks.com </A>";
            }
        }            
    return dt;
    }


    protected void  BtnImport_Click(object sender, EventArgs e)
    {
            Certificate objCert = new Certificate();
            UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
            int AcctId = objUserInfo.GetUserAcctId();
            int ImpAcctId = objUserInfo.AcctId;
            DataTable dtResult = new DataTable();
            Email objEmail = new Email();
            string strSubscriptionKey = Session["subkey"].ToString();
            try
            {
                DataSet ds = (DataSet)Session["SUB_INFO"];
                if (ds == null || ds.Tables.Count <= 0)
                {
                    setError(LblImportError, "");
                    objEmail.sendAmigopodErrorMessage("Error in fetching ClearPass Subscription " + strSubscriptionKey, "FetchSubscription", "", strSubscriptionKey);
                    wizActivate.ActiveStepIndex = 0;
                    return;
                }
                else
                {
                    LoadWizStep3(ds);
                }                           
            }
            catch (Exception ex)
            {
                new Log().logException("ImportClpSubscription", ex);
                wizActivate.ActiveStepIndex = 1;
                setError(LblImportError, "Application could not process your request.The error occurred has been logged");
                objEmail.sendAmigopodErrorMessage(ex.Message, "ImportClpSubscription", "", strSubscriptionKey);
            }
    }
    protected void BtnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Pages/MyClearPassSub.aspx");
    }
}


