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

public partial class Controls_AddQuickConnect : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false;
    }

    protected void btnActivate_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;        
        string cert_id = txtCertId.Text.Trim();              
        string email = string.Empty;        
        string part_id = string.Empty;
        string activation_code = string.Empty;
        string expiry_date = string.Empty;                
        string meth = "btnActivate_Click_AddQuickConnectLicense";
        Certificate objCert = new Certificate();
        UserInfo objUser = new UserInfo();
        Email objemail = new Email();
        objUser = (UserInfo)Session["USER_INFO"];
        AmigopodCertInfo objAmigopodCertinfo = new AmigopodCertInfo();
        Email objMail = new Email();
        Company objComp = new Company();
        string company_name = string.Empty;                
        int pk_cert_id;
        int licenseCount = 0;
        string so_id = string.Empty; string po_id = string.Empty;
        string strExpiryDate = string.Empty;
        string Name = string.Empty;
        QuickConnect objQuickConnect = new QuickConnect();
        string type = ConfigurationManager.AppSettings["QUICKCONNECT_TYPE"].ToString();
        try
        {
            DataSet DsCert = objCert.getClpCertFromCertGen(cert_id, Session["BRAND"].ToString());

            if (DsCert.Tables[0].Rows.Count > 0)
            {
                licenseCount = Convert.ToInt32(DsCert.Tables[0].Rows[0]["Qty"]);
                part_id = DsCert.Tables[0].Rows[0]["part_id"].ToString();
                expiry_date = DsCert.Tables[0].Rows[0]["expiration_date"].ToString();
            }
            else
            {
                lblErr.Text = "This Certificate is not valid.";
                lblErr.Visible = true;
                return;
            }

            //Check whether certificate is already activated
            DataSet dsCertInfo = objCert.GetQuickConnectCertInfo(cert_id, Session["BRAND"].ToString());
            if (dsCertInfo.Tables[0].Rows.Count <= 0)
            {
                lblErr.Text = "This Certificate does not exist.";
                lblErr.Visible = true;
                return;
            }
            else
            {
                activation_code = dsCertInfo.Tables[0].Rows[0]["user_name"].ToString();
                pk_cert_id = Int32.Parse(dsCertInfo.Tables[0].Rows[0]["pk_cert_id"].ToString());
                so_id = dsCertInfo.Tables[0].Rows[0]["so_id"].ToString();
                po_id = dsCertInfo.Tables[0].Rows[0]["cust_po_id"].ToString();

                if (activation_code != string.Empty)
                {
                    lblErr.Text = "This Certificate is already activated.";
                    lblErr.Visible = true;
                    return;
                }
            }

            DataSet dsAmigopodLookup = objCert.GetAmigoppodLookInfo(cert_id, Session["BRAND"].ToString(), string.Empty);
            string partType = string.Empty;

            if (dsAmigopodLookup.Tables[0].Rows.Count > 0)
            {
                partType = dsAmigopodLookup.Tables[0].Rows[0]["part_type"].ToString();
            }

            if (partType != type)
            {
                lblErr.Text = "The certificate ID is not that of QuickConnect License";
                lblErr.Visible = true;
                return;
            }

            email = objUser.GetUserEmail();
            Name = objUser.GetUserFirstName() + " " + objUser.GetUserLastName();

            // If this user activated QuickConnect?
            DataSet dsQuick = objCert.getQuickConnectUserInfo(email);
            if (dsQuick.Tables[0].Rows.Count > 0)
            {
                objQuickConnect.QCInstance_type = "RENEW";
                objQuickConnect.email = email;                
                objQuickConnect.expiryDate = expiry_date;
                objQuickConnect.licenseCount = licenseCount.ToString();
                objQuickConnect.company_name = company_name;
                objQuickConnect.certId = cert_id;
                objQuickConnect.pkCertid = pk_cert_id;
                objQuickConnect.so_id = so_id;
                objQuickConnect.po_id = po_id;
                objQuickConnect.part_id = part_id;
                objQuickConnect.name = Name;
                objQuickConnect.cert_id = txtCertId.Text;
                LoadWizStep2(objQuickConnect); 
            }
            else
            {
                objQuickConnect.QCInstance_type = "NEW";
                objQuickConnect.email = email;                
                objQuickConnect.expiryDate = expiry_date;
                objQuickConnect.licenseCount = licenseCount.ToString();
                objQuickConnect.company_name = company_name;
                objQuickConnect.certId = cert_id;
                objQuickConnect.pkCertid = pk_cert_id;
                objQuickConnect.so_id = so_id;
                objQuickConnect.po_id = po_id;
                objQuickConnect.part_id = part_id;
                objQuickConnect.name = Name;
                objQuickConnect.cert_id = txtCertId.Text;
                LoadWizStep3(objQuickConnect); 
            }          
        }
        catch (Exception ex)
        {
            new Log().logException(meth, ex);
            lblErr.Text = "Application could not process your request.The error occurred has been logged.";
            lblErr.Visible = true;
            objemail.sendAmigopodErrorMessage(ex.Message, "AddQuickConnectLicense", cert_id, string.Empty);
        }
    }

    private void LoadWizStep2(QuickConnect objQuickConnect)
    {
        wizActivate.ActiveStepIndex = 1;
        LitrExpiry1.Text = objQuickConnect.expiryDate;
        LitrCert1.Text = objQuickConnect.cert_id;
        LitrUserCount1.Text = objQuickConnect.licenseCount;
        LitrPartId1.Text = objQuickConnect.part_id;
        Session["QC"] = objQuickConnect;
    }

    private void LoadWizStep3(QuickConnect objQuickConnect)
    {
        wizActivate.ActiveStepIndex = 2;
        if (RdBtnPrompt.SelectedValue == "RENEW")
        {
            PanelSelect.Visible = true;
        }
        else
        {
            PanelAdd.Visible = true;
        }
        LitrExpiry2.Text = objQuickConnect.expiryDate;
        LitrCert2.Text = objQuickConnect.cert_id;
        LitrUserCount2.Text = objQuickConnect.licenseCount;
        LitrPartId2.Text = objQuickConnect.part_id;
        Session["QC"] = objQuickConnect;
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        QuickConnect objQuickConnect = new QuickConnect();
        Certificate objCert = new Certificate();
        objQuickConnect = (QuickConnect) Session["QC"];
        if (RdBtnPrompt.SelectedValue == "RENEW")
        {
            objQuickConnect.QCInstance_type = "RENEW";
            DataSet dsQuick = objCert.getQuickConnectCompanyInfo(objQuickConnect.email);
            if (dsQuick.Tables[0].Rows.Count == 1)
            {
                //LstCompany.DataSource = dsQuick;
                //LstCompany.DataValueField = "company_name";
                //LstCompany.DataTextField = "company_name";
                //LstCompany.DataBind();
                //LstCompany.Items.Insert(0, new ListItem("[Please select Company]", ""));
                LstCompany.Items.Insert(0, new ListItem(dsQuick.Tables[0].Rows[0]["company_name"].ToString(), dsQuick.Tables[0].Rows[0]["company_name"].ToString()));
            }
            else
            {
                LstCompany.DataSource = dsQuick;
                LstCompany.DataValueField = "company_name";
                LstCompany.DataTextField = "company_name";
                LstCompany.DataBind();
                LstCompany.Items.Insert(0, new ListItem("[Please select Company]", ""));
            }
            PanelSelect.Visible = true;
        }
        else
        {
            objQuickConnect.QCInstance_type = "NEW";
            PanelAdd.Visible = true;
        }
        LoadWizStep3(objQuickConnect);

    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    { 
        Certificate objCert = new Certificate();
        QuickConnect objQuickConnect = new QuickConnect();
        objQuickConnect = (QuickConnect) Session["QC"];
        string result = string.Empty;
        bool isActivated;
        UserInfo objUser = new UserInfo();
        objUser = (UserInfo)Session["USER_INFO"];        
        Email objEmail = new Email();
        string meth = "BtnSubmit_Click_AddQuickConnect";
        string Action = string.Empty;
        AmigopodCertInfo objAmigopodCertinfo = new AmigopodCertInfo();
        DateTime ExpiryDate;
        try
        {
            if (objQuickConnect.QCInstance_type == "NEW")
            {
                if (txtCompany.Text.Trim() == string.Empty)
                {
                    LblErr2.Text = "Please enter Company Name";
                    LblErr2.Visible = true;
                    return;
                }
                objQuickConnect.username = Guid.NewGuid().ToString("N").Substring(0, 8);
                objQuickConnect.password = Membership.GeneratePassword(10, 3);
                objQuickConnect.company_name = txtCompany.Text.Trim();
                ExpiryDate = Convert.ToDateTime(objQuickConnect.expiryDate);
                objQuickConnect.expiryDate = ExpiryDate.ToString("yyyy-MM-dd");
                Action = "ADD";
            }
            else
            {
                if (LstCompany.SelectedItem.Value == string.Empty)
                {
                    LblErr2.Text = "Please select Company Name";
                    LblErr2.Visible = true;
                    return;
                }
                DataSet dsQuickbyComp = objCert.getQuickConnectUserInfo(objQuickConnect.email, LstCompany.SelectedItem.Text.Trim());
                objQuickConnect.username = dsQuickbyComp.Tables[0].Rows[0]["user_name"].ToString();
                objQuickConnect.password = dsQuickbyComp.Tables[0].Rows[0]["password"].ToString();
                objQuickConnect.company_name = LstCompany.SelectedItem.Text.Trim();
                ExpiryDate  = Convert.ToDateTime(dsQuickbyComp.Tables[0].Rows[0]["expiry_date"].ToString());
                ExpiryDate = ExpiryDate.AddYears(Int32.Parse(ConfigurationManager.AppSettings["QC_YEAR"].ToString()));                   
                objQuickConnect.expiryDate = ExpiryDate.ToString("yyyy-MM-dd");
                Action = "UPDATE";
            }
            result = objCert.AddQuickConnect(objQuickConnect.certId, objQuickConnect.licenseCount, objQuickConnect.expiryDate, objQuickConnect.name, objQuickConnect.email, Server.UrlEncode(objQuickConnect.company_name), objQuickConnect.username, objQuickConnect.password, Action);            
         if (result.Contains("success"))
         {
             isActivated = objCert.InsertQuickConnect(objQuickConnect.pkCertid, objUser.AcctId, objQuickConnect.company_name, objUser.GetUserAcctId(), Int32.Parse(objQuickConnect.licenseCount), objQuickConnect.expiryDate, objQuickConnect.name, objQuickConnect.email, objQuickConnect.username, objQuickConnect.password);
            if (isActivated)
            {
                //send mail
                objAmigopodCertinfo.UserName = objQuickConnect.username;
                objAmigopodCertinfo.Password = objQuickConnect.password;
                objAmigopodCertinfo.Name = objQuickConnect.name;
                objAmigopodCertinfo.Email = objQuickConnect.email;
                objAmigopodCertinfo.PartId = objQuickConnect.part_id;
                objAmigopodCertinfo.CertId = objQuickConnect.certId;
                objAmigopodCertinfo.SoId = objQuickConnect.so_id;
                objAmigopodCertinfo.PoId = objQuickConnect.po_id;
                objAmigopodCertinfo.LicenseCount = objQuickConnect.licenseCount;
                objAmigopodCertinfo.Brand = Session["BRAND"].ToString().ToUpper();
                objAmigopodCertinfo.ExpiryDate = Convert.ToDateTime(objQuickConnect.expiryDate);
                objAmigopodCertinfo.CompanyName = objQuickConnect.company_name;
                bool blSend = objEmail.sendClsQuickConnect(objAmigopodCertinfo);
                ((Literal)wizActivate.FindControl("LiteralUserName")).Text = objQuickConnect.username;
                ((Literal)wizActivate.FindControl("LiteralPwd")).Text = objQuickConnect.password;
                ((Literal)wizActivate.FindControl("LiteralCount")).Text = objQuickConnect.licenseCount;
                ((Literal)wizActivate.FindControl("LiteralExpry")).Text = objQuickConnect.expiryDate;
                wizActivate.ActiveStepIndex = 3;
             }
            else
            {
                new Log().logInfo(meth, "Unable to insert the QuickConnect license certificate: " + objQuickConnect.certId + " for company: " + objQuickConnect.company_name);
                LblErr1.Text = "Application could not process your request.The error occurred has been logged.";
                LblErr1.Visible = true;
                objEmail.sendAmigopodErrorMessage("Unable to insert the QuickConnect license certificate", "AddQuickConnectLicense", objQuickConnect.certId, objQuickConnect.company_name);
                wizActivate.ActiveStepIndex = 2;
                return;
            }
         }
         else if (result.Contains("errorCode=userPresent"))
            {
                new Log().logInfo(meth, "Error : " + result + "Certifcate id" + objQuickConnect.certId);
                LblErr1.Text = "This user already exists.";
                LblErr1.Visible = true;
                objEmail.sendAmigopodErrorMessage("Error : " + result, "AddQuickConnect", objQuickConnect.certId, string.Empty);
                wizActivate.ActiveStepIndex = 2;
                return;
            }
        else
            {
                new Log().logInfo(meth, "Error : " + result + "Certifcate id" + objQuickConnect.certId);
                LblErr1.Text = "Application could not process your request.The error occurred has been logged.";
                LblErr1.Visible = true;
                objEmail.sendAmigopodErrorMessage("Error : " + result, "AddQuickConnect", objQuickConnect.certId, string.Empty);
                wizActivate.ActiveStepIndex = 2;
                return;
            }
        }
        catch (Exception ex)
        {
            new Log().logException(meth, ex);
            LblErr1.Text = "Application could not process your request.The error occurred has been logged.";
            LblErr1.Visible = true;
            objEmail.sendAmigopodErrorMessage(ex.Message, "AddQuickConnectLicense", objQuickConnect.certId, string.Empty);
        }
    }  
}
