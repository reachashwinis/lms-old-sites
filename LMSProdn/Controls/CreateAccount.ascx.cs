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

public partial class Controls_CreateAccount : System.Web.UI.UserControl
{
    //Company objCompany;
    //Lookup objLookup;
    //UserInfo objUserInfo;
    //User objUser;
   
    //int intAcctId = -1;
    //private string ACCT_MODE = "ACCT_MODE";
    //DataSet dsLookupValues;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Redirect(ConfigurationManager.AppSettings["NEWCREATEACCT_URL"], true);
    }

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    objUser= new User();
    //    objCompany = new Company();
    //    objLookup = new Lookup();
    //    lblError.Visible = false;
    //    lblResetErr.Visible = false;
    //    objUserInfo = (UserInfo)Session["USER_INFO"];
    //    if (!IsPostBack)
    //    {
    //        //load lookupValues
    //        dsLookupValues = (DataSet)Cache[Lookup.LOOKUP_TBL];
    //        if (dsLookupValues == null)
    //        {
    //            dsLookupValues = objLookup.LoadLookupValues(((UserInfo)Session["USER_INFO"]).GetUserEmail(),Session["BRAND"].ToString());
    //            Cache.Insert(Lookup.LOOKUP_TBL, dsLookupValues, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
    //        }

    //        UIHelper.PrepareAndBindLookupFromAllWithoutPlease(ddlCustType, dsLookupValues.Tables[Lookup.LOOKUP_TBL], "ROLES", "TXT", "VAL", false);
    //        switch (objUserInfo.GetUserRole())
    //        {
    //            case UserType.ArubaCA:
    //            ddlCustType.Items.Remove(ddlCustType.Items.FindByValue(UserType.ArubaGod));
    //            break;
    //        }

    //        DataSet dsCompany = objCompany.GetCompanyListAll(Session["BRAND"].ToString());

    //        UIHelper.PrepareAndBindListWithoutPlease(ddlCompany, dsCompany.Tables[0], "CompanyName", "CompanyId", true);
            

    //        Session[ACCT_MODE] = "add";
    //        ClearControls();
    //        if ((Request.UrlReferrer.ToString().ToLower().Contains("/pages/accounts.aspx")) || (Request.UrlReferrer.ToString().ToLower().Contains("/pages/companyaccounts.aspx")))
    //        { 
    //        //modify account
    //            Session[ACCT_MODE] = "modify";
    //            if (Session[User.ACCT_ID] != null)
    //            {
    //                intAcctId = Int32.Parse(Session[User.ACCT_ID].ToString());

    //                if (intAcctId <= 0)
    //                {
    //                    Session[ACCT_MODE] = "add";
    //                }
    //                else
    //                {
    //                    LoadUserInformation();
    //                }
    //            }
    //            else
    //            {
    //                Session[ACCT_MODE] = "add";
    //            }
            
    //        }
            
    //    }
     

    //    setScreenMode(Session[ACCT_MODE].ToString());
    //}
    //private void setScreenMode(String mode)
    //{
    //    if (mode == "add")
    //    {
    //        btnUpdate.Visible = false;
    //        btnCreate.Visible = true;
    //        pnlRstPwd.Visible = false;
            
    //    }
    //    else
    //    {
    //        btnCreate.Visible = false;
    //        btnUpdate.Visible = true;
    //        pnlRstPwd.Visible = true;

    //        if ((objUserInfo.GetUserRole().Equals(UserType.ArubaGod) || (objUserInfo.GetUserRole().Equals(UserType.ArubaCA))) && objUserInfo.GetUserEmail().ToLower().Contains("@arubanetworks.com"))
    //        {
    //            pnlArubaCa.Visible = true;

    //            if ((!objUser.IsWindowsLoginIDSSO(txtEmail.Text, Session["APPID"].ToString())))
    //                pnlRstPwd.Visible = true;
    //            else
    //                pnlRstPwd.Visible = false;

    //        }
                 
    //    }
        
    //}

    //private void LoadUserInformation()
    //{

    //    DataSet dsUser = objUser.GetAccountInfo(intAcctId,Session["BRAND"].ToString());
    //    if (dsUser == null)
    //    {
    //        Session[ACCT_MODE] = "add";
    //        setScreenMode(Session[ACCT_MODE].ToString());
    //    }
    //    else
    //    {
    //        DataRow dr = dsUser.Tables[0].Rows[0];
    //        //UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.ACCT_ID, hdnAcctId);
    //        UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.FIRST_NAME, txtFName);
    //        UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.LAST_NAME, txtLName);
    //        UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.EMAIL, txtEmail);
    //        UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.USER_ENTERED_COMPANY, txtCompany);
    //        UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.PHONE, txtPhone);
    //        UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.STATUS, ddlStatus);
    //        UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.COMPANY_ID, ddlCompany);
    //        UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.USER_TYPE, ddlCustType);
    //        UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.COMMENTS, TxtComments);
    //        UIHelper.SetDataColumnOrControl(UIHelper.AssignDirection.Control, dr, User.ISCOMPANYADMIN, ChkIsAdmin);
    //        txtEmail.ReadOnly = true;
    //        ViewState["UserCompanyId"] = dr[User.COMPANY_ID].ToString();
    //        ViewState["Userid"] = dr[User.ACCT_ID].ToString();
    //        if ((Request.UrlReferrer.ToString().ToLower().Contains("/pages/companyaccounts.aspx")))
    //        {
    //            ddlCompany.Enabled = false;
    //            ddlCustType.Enabled = false;
    //        }
    //        else
    //        {
    //            ddlCompany.Enabled = true;
    //            ddlCustType.Enabled = true;
    //        }

    //    } 
    //}

    //private void ClearControls()
    //{
    //    txtFName.Text = string.Empty;
    //    txtLName.Text = string.Empty;
    //    txtEmail.Text = string.Empty;
    //    txtPhone.Text = string.Empty;
    //    txtCompany.Text = string.Empty;
    //    TxtComments.Text = string.Empty;
    //    ddlCustType.ClearSelection();
    //    UIHelper.SetDefaultValue(ddlCustType, UserType.Customer);
    //    ddlCompany.ClearSelection();
    //    ddlStatus.ClearSelection();
    //    UIHelper.SetDefaultValue(ddlStatus,"Active");
    
    //}

    //private void setError(string text, Label lbl)
    //{
    //    lbl.Text = text;
    //    lbl.Visible = true;
    //}

    //public void btnAdd_OnClick(object sender, EventArgs args)
    //{
    //    string strFName = txtFName.Text.Trim();
    //    string strLname = txtLName.Text.Trim();
    //    string strEmail = txtEmail.Text.Trim().ToLower();
    //    string strCompanyName = txtCompany.Text.Trim();
    //    string strStatus = ddlStatus.SelectedValue;
    //    string strCustType = ddlCustType.SelectedValue;
    //    string strBrand = Session["BRAND"].ToString();
    //    string strComments = TxtComments.Text;
        
    //    string strERROR= string.Empty;

    //    if (strFName.Equals(string.Empty))
    //        strERROR += "First Name is mandatory. <BR>";

    //    if (strLname.Equals(string.Empty))
    //        strERROR += "Last Name is mandatory.<BR>";

    //    if (strEmail.Equals(string.Empty))
    //        strERROR += "Email is mandatory.<BR>";

    //    if (strCompanyName.Equals(string.Empty))
    //        strERROR += "Company Name is mandatory.<BR>";
       
    //    if (objUser.IsRestrictedDomain(strEmail))
    //    {
    //        strERROR += "Email Id is found in our list of Restricted Domain.<BR>";
    //    }

    //    if (ChkIsAdmin.Checked == true)
    //    {
    //        if (ddlCompany.SelectedValue.ToString() != string.Empty)
    //        {
    //            if (objUser.IsOnlyAdmin(strEmail, Int32.Parse(ddlCompany.SelectedValue)))
    //            {
    //                strERROR += "Company Admin already exists for " + ddlCompany.SelectedItem + " .<BR>";
    //            }
    //        }
    //        else
    //        {
    //            strERROR += "Company information is required for Company Admin.Please select company!!<BR>";
    //        }
    //    }

    //    //check for user ID in accounts and pending table

    //    strERROR += objUser.IsEmailExists(strEmail, true);

    //    if (!strERROR.Equals(string.Empty))
    //    {
    //        setError(strERROR, lblError);
    //        return;
    //    }
 
    //    string strPassword = string.Empty;
    //    if ((txtEmail.Text.ToLower().Contains("@arubanetworks.com")))
    //    {
    //        if (!objUser.isArubaEmp(txtEmail.Text))
    //        {
    //            strPassword = Membership.GeneratePassword(15, 5);
    //        }
    //        //else if (!objUser.IsWindowsLoginIDSSO(txtEmail.Text, Session["APPID"].ToString()))
    //        //{
    //        //    strPassword = Membership.GeneratePassword(15, 5);
    //        //}
    //    }
    //    else
    //    {
    //        strPassword = Membership.GeneratePassword(15, 5);
    //    }

    //     UserInfo objLicUser = new UserInfo();

    //     objLicUser.FirstName= strFName;
    //     objLicUser.LastName= strLname;
    //     objLicUser.Email= strEmail;
    //     objLicUser.CompanyName= strCompanyName;
    //     objLicUser.Role = ddlCustType.SelectedValue;
    //     objLicUser.Status= ddlStatus.SelectedValue;
    //     objLicUser.Phone= txtPhone.Text.Trim();
    //     objLicUser.Brand= strBrand;
    //     objLicUser.Password= strPassword;
    //     objLicUser.Comments = strComments;
    //     objLicUser.isCompanyAdmin = ChkIsAdmin.Checked;
    //     objLicUser.IPAddress = Request.ServerVariables["REMOTE_ADDR"].ToString();
    //     objLicUser.AcctId = objUserInfo.AcctId;
         
    //    if (ddlCompany.SelectedValue.Equals(string.Empty))
    //        objLicUser.CompanyId = -1;
    //    else
    //        objLicUser.CompanyId = Int32.Parse(ddlCompany.SelectedValue);

    //    if (objUser.AddUser(objLicUser))
    //    {
    //        Response.Redirect(ConfigurationManager.AppSettings["LISTACCOUNTS_URL"], true);
    //    }
    //    else
    //    {
    //        strERROR = "Unable to create account";
    //        setError(strERROR, lblError);       
    //    }
        
    //}

    //public void btnUpdate_OnClick(object sender, EventArgs args)
    //{
    //    string strFName = txtFName.Text.Trim();
    //    string strLname = txtLName.Text.Trim();
    //    string strEmail = txtEmail.Text.Trim().ToLower();
    //    string strCompanyName = txtCompany.Text.Trim();
    //    string strStatus = ddlStatus.SelectedValue;
    //    string strCustType = ddlCustType.SelectedValue;
    //    string strComments = TxtComments.Text;
    //    string strBrand = Session["BRAND"].ToString();
    //    int AcctId =  objUserInfo.AcctId;
    //    UserInfo objLicUser = new UserInfo();

    //    string strERROR = string.Empty;

    //    if (strFName.Equals(string.Empty))
    //        strERROR += "First Name is mandatory. <BR>";

    //    if (strLname.Equals(string.Empty))
    //        strERROR += "Last Name is mandatory.<BR>";

    //    if (strCompanyName.Equals(string.Empty))
    //        strERROR += "Company Name is mandatory.<BR>";

    //    if (ChkIsAdmin.Checked == true)
    //    {
    //        if (objUser.IsOnlyAdmin(strEmail, Int32.Parse(ddlCompany.SelectedValue)))
    //        {
    //            strERROR += "Company Admin already exists for " + ddlCompany.SelectedItem + " .<BR>";
    //        }
    //    }

    //    int intUserCompanyid = Int32.Parse(ViewState["UserCompanyId"].ToString());
    //    int intUserId = Int32.Parse(ViewState["Userid"].ToString());
    //    int intSelCompanyid = objCompany.GetCompanyID(ddlCompany.SelectedItem.ToString(), Session["BRAND"].ToString());
    //    if ((intUserCompanyid == -1) || (intUserCompanyid == intSelCompanyid))
    //    {
    //        intUserCompanyid = intSelCompanyid;
    //    }

    //    else 
    //    {
    //        //nov
    //        Certificate objCert = new Certificate();
    //        DataSet dsCert = objCert.GetActiveCertsByAcct(intUserId, 1, 1, "", Session["BRAND"].ToString());
    //        if (dsCert != null)
    //        {
    //            if (dsCert.Tables[0].Rows.Count > 0)
    //            {
    //                strERROR += "Certificates are activated by the User.Can not move the user to the selected company.";
    //            }
    //            else
    //            {
    //                intUserCompanyid = intSelCompanyid;
    //            }
    //        }
    //        else
    //        {
    //            intUserCompanyid = intSelCompanyid;
    //        }
    //    }

    //    if (!strERROR.Equals(string.Empty))
    //    {
    //        setError(strERROR, lblError);
    //        return;
    //    }

    //    objLicUser.Email = strEmail;
    //    objLicUser.FirstName = strFName;
    //    objLicUser.LastName = strLname;
    //    objLicUser.CompanyName = strCompanyName;
    //    objLicUser.Phone = txtPhone.Text;
    //    objLicUser.Status = strStatus;
    //    objLicUser.Role = strCustType;
    //    objLicUser.Brand = strBrand;
    //    objLicUser.AcctId = AcctId;
    //    //objLicUser.CompanyId = Int32.Parse(ddlCompany.SelectedValue);
    //    //objLicUser.CompanyId = objCompany.GetCompanyID(objLicUser.CompanyName,Session["BRAND"].ToString());
    //    //objLicUser.CompanyId = objCompany.GetCompanyID(ddlCompany.SelectedItem.ToString(), Session["BRAND"].ToString());
    //    objLicUser.CompanyId = intUserCompanyid;
    //    objLicUser.Comments = strComments;
    //    objLicUser.isCompanyAdmin = ChkIsAdmin.Checked;
    //    bool retVal = objUser.UpdateAccount(objLicUser);
    //    if (retVal == true)
    //    {
    //        setError("Account Details are Updated successfully", LblSucc);
    //    }
    //    else
    //    {
    //        setError("Unable to Update Account Details", lblError);
    //    }

    //}

    //public void btnResetPwd_OnClick(object sender, EventArgs args)
    //{
    //    string strERROR = string.Empty;
    //    string strSUCCESS = string.Empty;
    //    if (!Page.IsValid)
    //        return;

    //    if (txtNewPass.Text.Length > 0)
    //    {
    //        if (txtNewPass.Text.Contains(" "))
    //        {
    //            strERROR = "Password should not contain blanks";
    //            setError(strERROR, lblError);
    //            return;
    //        }

    //        User objUser = new User();
    //        if (objUser.ResetPassword(txtEmail.Text, txtNewPass.Text))
    //        {
    //            //mail
    //            Email objEmail = new Email();
    //            AccountMailInfo objLgm = new AccountMailInfo();
    //            objLgm.Email = txtEmail.Text;
    //            objLgm.Password = txtNewPass.Text;
    //            objLgm.Brand = Session["BRAND"].ToString().ToUpper();
    //             bool retVal = objEmail.sendLoginInfo(objLgm);
    //             if (retVal)
    //            {
    //                strSUCCESS = "Password changed successfully.";
    //            }
    //            else
    //            {
    //                strSUCCESS = "Password changed successfully.Failed to Send Mail";
    //            }
    //        }
    //        else
    //        {
    //            strERROR = "Unable to change password";
    //        }
    //    }
    //    else
    //    {
    //        strERROR = "Please Enter Password";

    //    }
    //    setError(strERROR, lblError);
    //    setError(strSUCCESS, LblSuccess);
    //}


    //protected void ddlCustType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (!ddlCustType.SelectedItem.ToString().Contains("Super Admin") || !ddlCustType.SelectedItem.ToString().Contains("Customer Advocacy"))
    //    {
    //        ChkIsAdmin.Enabled = true;
    //    }

    //}
}
