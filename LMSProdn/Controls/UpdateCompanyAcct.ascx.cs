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

public partial class Controls_UpdateCompanyAcct : System.Web.UI.UserControl
{
    Company objCompany;
    Lookup objLookup;
    UserInfo objUserInfo;
    User objUser;
   
    int intAcctId = -1;
    private string ACCT_MODE = "ACCT_MODE";
    DataSet dsLookupValues;

    protected void Page_Load(object sender, EventArgs e)
    {
        objUser= new User();
        objCompany = new Company();
        objLookup = new Lookup();
        lblError.Visible = false;
        objUserInfo = (UserInfo)Session["USER_INFO"];
        if (!IsPostBack)
        {
            //load lookupValues
            dsLookupValues = (DataSet)Cache[Lookup.LOOKUP_TBL];
            if (dsLookupValues == null)
            {
                dsLookupValues = objLookup.LoadLookupValues(((UserInfo)Session["USER_INFO"]).GetUserEmail(),Session["BRAND"].ToString());
                Cache.Insert(Lookup.LOOKUP_TBL, dsLookupValues, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }          

            DataSet dsCompany = objCompany.GetCompanyListAll(Session["BRAND"].ToString());

            UIHelper.PrepareAndBindListWithoutPlease(ddlCompany, dsCompany.Tables[0], "CompanyName", "CompanyId", true);

            //if ((Request.UrlReferrer.ToString().ToLower().Contains("/pages/UpdateCompanyAcct.aspx")))
            //{ 
            //modify account
                Session[ACCT_MODE] = "modify";
                if (Session[User.ACCT_ID] != null)
                {
                    intAcctId = Int32.Parse(Session[User.ACCT_ID].ToString());

                    if (intAcctId >= 0)
                    {
                        LoadUserInformation();
                    }
                }
                else
                {
                    setError("Unable to fetch User Info. Please try again.", lblError);
                }
            
            //}
            
        }
     
    }

    private void LoadUserInformation()
    {

        DataSet dsUser = objUser.GetAccountInfo(intAcctId,Session["BRAND"].ToString());
        if (dsUser == null)
        {
            setError("Unable to fetch User Information. Please try again", lblError);
        }
        else
        {
            DataRow dr = dsUser.Tables[0].Rows[0];
            //UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.ACCT_ID, hdnAcctId);
            UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.FIRST_NAME, txtFName);
            UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.LAST_NAME, txtLName);
            UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.EMAIL, txtEmail);
            UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.USER_ENTERED_COMPANY, txtCompany);                        
            UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.COMPANY_ID, ddlCompany);
            //UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.COMMENTS, TxtComments);            
            txtEmail.ReadOnly = true;
            txtFName.ReadOnly = true;
            txtLName.ReadOnly = true;
            ViewState["UserCompanyId"] = dr[User.COMPANY_ID].ToString();
            ViewState["Userid"] = dr[User.ACCT_ID].ToString();
            if ((Request.UrlReferrer.ToString().ToLower().Contains("/pages/companyaccounts.aspx")))
            {
                ddlCompany.Enabled = false;
            }
            else
            {
                ddlCompany.Enabled = true;
            }

        } 
    }

    private void setError(string text, Label lbl)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }

    public void btnUpdate_OnClick(object sender, EventArgs args)
    {
        string strFName = txtFName.Text.Trim();
        string strLname = txtLName.Text.Trim();
        string strEmail = txtEmail.Text.Trim().ToLower();
        string strCompanyName = txtCompany.Text.Trim();
        string strComments = TxtComments.Text;
        string strBrand = Session["BRAND"].ToString();
        Email objEmail = new Email();
        AccountMailInfo objLgm = new AccountMailInfo();
        int AcctId =  objUserInfo.AcctId;
        UserInfo objLicUser = new UserInfo();

        string strERROR = string.Empty;

        if (ddlCompany.SelectedValue.Equals(string.Empty))
            strERROR += "Please select company <BR>";

        if (strCompanyName.Equals(string.Empty))
            strERROR += "Company Name is mandatory.<BR>";

        int intUserCompanyid = Int32.Parse(ViewState["UserCompanyId"].ToString());
        int intUserId = Int32.Parse(ViewState["Userid"].ToString());
        int intSelCompanyid = objCompany.GetCompanyID(ddlCompany.SelectedItem.ToString(), Session["BRAND"].ToString());
        if ((intUserCompanyid == -1) || (intUserCompanyid == intSelCompanyid))
        {
            intUserCompanyid = intSelCompanyid;
        }

        else 
        {
            //nov
            Certificate objCert = new Certificate();
            DataSet dsCert = objCert.GetActiveCertsByAcct(intUserId, 1, 1, "", Session["BRAND"].ToString());
            if (ViewState["confirm"].ToString() == "true" && (dsCert != null || dsCert.Tables[0].Rows.Count > 0))
            {                
                setError("Certificates are activated by the User.Do you want to continue?", lblError);
                ViewState["confirm"] = "true";
                btnUpdate.Text = "Confirm";
                return;
            }
            else
            {
                intUserCompanyid = intSelCompanyid;
            }
        }

        if (!strERROR.Equals(string.Empty))
        {
            setError(strERROR, lblError);
            return;
        }

        objLicUser.Email = strEmail;
        objLicUser.FirstName = strFName;
        objLicUser.LastName = strLname;
        objLicUser.CompanyName = strCompanyName;
        objLicUser.Brand = strBrand;
        objLicUser.AcctId = AcctId;
        //objLicUser.CompanyId = Int32.Parse(ddlCompany.SelectedValue);
        //objLicUser.CompanyId = objCompany.GetCompanyID(objLicUser.CompanyName,Session["BRAND"].ToString());
        //objLicUser.CompanyId = objCompany.GetCompanyID(ddlCompany.SelectedItem.ToString(), Session["BRAND"].ToString());
        objLicUser.CompanyId = intUserCompanyid;
        objLicUser.Comments = strComments;
        bool retVal = objUser.UpdateAccount(objLicUser);
        if (retVal == true)
        {
            setError("Account Details are Updated successfully", LblSucc);
            //send mail
            objLgm.Brand = objLicUser.Brand.ToUpper();
            objLgm.Email = objLicUser.Email;
            objLgm.Name = objLicUser.FirstName + objLicUser.LastName;
            objLgm.CompanyName = objLicUser.CompanyName;
            //objEmail.sendCompanyAssignedInfo(objLgm);
        }
        else
        {
            setError("Unable to Update Account Details", lblError);
        }

    }
}

