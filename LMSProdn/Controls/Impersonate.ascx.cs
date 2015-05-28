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

public partial class Controls_Impersonate : System.Web.UI.UserControl
{
    private const string MPS_ERROR = "You are trying to impersonate as yourself!";
    private const string USER_NOT_FOUND_ERROR = "Invalid User!";
    private const string ARUBA_ERROR = "Impersonating Aruba employees is not allowed!";
    private const string BRAND_ERROR = "This user does not belong to this site.";
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnImpersonate_OnClick(object sender, EventArgs args)
    {
        lblError.Visible=false;
        string strImp = txtImpEmail.Text.Trim().ToLower();
        User modUser = new User();
        UserInfo objLicUser = (UserInfo)Session["USER_INFO"];
        UserInfo objImpUser = (UserInfo)Session["USER_INFO"];
        
        if (strImp.Equals(objLicUser.Email))
        {
            setError(MPS_ERROR);
            return;
        }

        
        objImpUser = modUser.GetUserInfo(strImp,Session["BRAND"].ToString());
         if (objImpUser == null)
         {
             setError(USER_NOT_FOUND_ERROR);
             return;
         }
        
        if (!objLicUser.Brand.Equals(objImpUser.Brand))
         { 
             setError(BRAND_ERROR);
             return;
         }

         if (objImpUser.Role.Contains("aruba") && !objLicUser.Role.Equals(ConfigurationManager.AppSettings["ADMIN_ROLE"].ToString()))
         {
             setError(ARUBA_ERROR);
             return;
         }

        //if all above rules pass then set impersonate props
         objLicUser.ImpersonateEmail = objImpUser.Email;
         objLicUser.ImpersonateUserRole = objImpUser.Role;
         objLicUser.ImpersonateUserId = objImpUser.AcctId;
         objLicUser.ImpersonateUserCompanyId = objImpUser.CompanyId;
         objLicUser.ImpersonateFirstname = objImpUser.FirstName;
         objLicUser.ImpersonateLastname = objImpUser.LastName;
         objLicUser.IsImpersonate = true;
         Session["USER_INFO"] = objLicUser;
         Cache.Remove("MENU_ITEMS");
         Response.Redirect(ConfigurationManager.AppSettings["DEFAULT_URL"].ToString(), true);


    
    }
    private void setError(string strErrorTxt)
    {
        lblError.Text = strErrorTxt;
        lblError.Visible = true;
    }
}
