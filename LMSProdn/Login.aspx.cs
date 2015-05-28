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
using System.Text;


public partial class LoginPage : Com.Arubanetworks.Licensing.Lib.BasePage
{
    public string DestPage;
    protected void Page_Load(object sender, EventArgs e)
    {
        //lgnLogin.Visible = !IsSiteClosed();
        //spClosed.Visible = IsSiteClosed();
        CheckSiteClosed();
        Response.Redirect("~/Validate.aspx");
        if (lgnLogin.Visible)
        {
            lgnLogin.Focus();
        }
        //if (!IsSessionExpired())
        //Response.Redirect(ConfigurationManager.AppSettings["DEFAULT_URL"],false);
    }
    protected void lgnLogin_Authenticate(object sender, AuthenticateEventArgs args)
    {
        //UserInfo objLicUser = new UserInfo();
        //User objUserModule = new User();
        //string errMessage = string.Empty;
        //string UserIPAddr = Request.ServerVariables["REMOTE_HOST"].ToString();
        //Email objEmails = new Email();

        //errMessage = objUserModule.AuthenticateUser(lgnLogin.UserName, lgnLogin.Password, UserIPAddr);
        //objLicUser = objUserModule.GetUserInfo(lgnLogin.UserName, Session["BRAND"].ToString());
        //if (errMessage.Equals(string.Empty))
        //{
        //    if (objLicUser != null)
        //    {
        //        if (!objLicUser.Status.ToUpper().Equals("ACTIVE"))
        //         {
        //             args.Authenticated = false;
        //             lgnLogin.FailureText = "Your account is not active";
        //         }
        //         else if (objLicUser.Role.Equals(string.Empty))
        //         {
        //             args.Authenticated = false;
        //             lgnLogin.FailureText = "Your account does not have access setup";
        //         }
        //         else if (objLicUser.Status.ToUpper().Equals("ACTIVE") && objUserModule.IsInactiveAccount(lgnLogin.UserName, ConfigurationManager.AppSettings["LMSAPP_ID"].ToString(), true))
        //         {
        //             AccountMailInfo objAccountInfo = new AccountMailInfo();
        //             objAccountInfo.ActivationCode = objUserModule.getActivationCode(lgnLogin.UserName,Session["BRAND"].ToString());
        //             objAccountInfo.Brand = Session["BRAND"].ToString().ToUpper();
        //             objAccountInfo.Email = objLicUser.Email;
        //             objAccountInfo.Name = objLicUser.FirstName + " " + objLicUser.LastName;                    
        //             objEmails.sendActivationInfo(objAccountInfo);
        //             lgnLogin.FailureText = "Your account needs to be validated due to no activity.  Please confirm to reinstate via the activation email sent to your email ID.";
        //         }
        //         else
        //         {
        //             Session["User_Info"] = objLicUser;
        //             //Session["EMail"] = objLicUser.Email;
        //             args.Authenticated = true;
        //             string strInfo = "Success :-- UserName : " + lgnLogin.UserName + " | DateTime : " + DateTime.Now.ToString()+"  |IP: "+Request.ServerVariables["REMOTE_HOST"].ToString();
        //             new Log().logInfo("Login.lgnLogin_Authenticate", strInfo);
        //             bool retVal = objUserModule.UpdateLastLogin(lgnLogin.UserName, Session["APPID"].ToString());                   
        //         }
        //     }
        //     else
        //     {
        //         if (objUserModule.isArubaEmp(lgnLogin.UserName) && Session["BRAND"].ToString() == ConfigurationManager.AppSettings["ARUBA_BRAND"].ToString())
        //         {
        //             objLicUser = objUserModule.GetEmpInfo(lgnLogin.UserName);
        //             Session["User_Info"] = objLicUser;
        //             args.Authenticated = true;
        //             string strInfo = "Success :-- UserName : " + lgnLogin.UserName + " | DateTime : " + DateTime.Now.ToString() + "  |IP: " + Request.ServerVariables["REMOTE_HOST"].ToString();
        //             new Log().logInfo("Login.lgnLogin_Authenticate", strInfo);
        //         }
        //         else
        //         {
        //             lgnLogin.FailureText = "Your account may not be not active or it does not exists!!";
        //             args.Authenticated = false;
        //         }                
        //     }
        //}
        //else
        //{
        //    if (objLicUser != null)
        //    {
        //        if (errMessage.Contains("IP"))
        //        {
        //            if (ConfigurationManager.AppSettings["IP_REQUIRED"].ToString() == "YES")
        //            {
        //                //send email
        //                Email objEmail = new Email();
        //                AccountMailInfo objLgm = new AccountMailInfo();
        //                objLgm.Email = objLicUser.Email;
        //                objLgm.Brand = Session["BRAND"].ToString().ToUpper();
        //                objLgm.Name = objLicUser.FirstName + " " + objLicUser.LastName;
        //                objLgm.IPAddress = Request.ServerVariables["REMOTE_HOST"].ToString();
        //                objLgm.ActivationCode = objUserModule.getIPAddressCode(objLgm.Email.ToString());
        //                bool blResult = objEmail.sendIPActivationInfo(objLgm);
        //                if (blResult == true)
        //                {
        //                    lgnLogin.FailureText = errMessage;
        //                }
        //                else
        //                {
        //                    lgnLogin.FailureText = "You are trying to Login with Different IP Address. For furhter detail,Please Contact TAC team";
        //                }
        //            }
        //            else if (objLicUser.Status.ToUpper().Equals("ACTIVE") && objUserModule.IsInactiveAccount(lgnLogin.UserName, ConfigurationManager.AppSettings["LMSAPP_ID"].ToString(), true))
        //            {
        //                AccountMailInfo objAccountInfo = new AccountMailInfo();
        //                objAccountInfo.ActivationCode = objUserModule.getActivationCode(lgnLogin.UserName, Session["BRAND"].ToString());
        //                objAccountInfo.Brand = Session["BRAND"].ToString().ToUpper();
        //                objAccountInfo.Email = objLicUser.Email;
        //                objAccountInfo.Name = objLicUser.FirstName + " " + objLicUser.LastName;
        //                objEmails.sendActivationInfo(objAccountInfo);
        //                lgnLogin.FailureText = "Your account needs to be validated due to no activity.  Please confirm to reinstate via the activation email sent to your email ID.";
        //            }
        //            else
        //            {
        //                Session["User_Info"] = objLicUser;
        //                //Session["EMail"] = objLicUser.Email;
        //                args.Authenticated = true;
        //                string strInfo = "Success :-- UserName : " + lgnLogin.UserName + " | DateTime : " + DateTime.Now.ToString() + "  |IP: " + Request.ServerVariables["REMOTE_HOST"].ToString();
        //                new Log().logInfo("Login.lgnLogin_Authenticate", strInfo);
        //                bool retVal = objUserModule.UpdateLastLogin(lgnLogin.UserName, Session["APPID"].ToString());
        //            }

        //        }

        //        else if (errMessage.Contains("Restricted Domain"))
        //        {
        //            if (ConfigurationManager.AppSettings["RESTRICTED_DOMAIN"].ToString() == "YES")
        //            {
        //                Session["EMail"] = objLicUser.Email;
        //                string strPath = ConfigurationManager.AppSettings["UPDATE_DOMAIN_URL"];
        //                Response.Redirect(strPath, true);
        //            }
        //            else
        //            {
        //                Session["User_Info"] = objLicUser;
        //                //Session["EMail"] = objLicUser.Email;
        //                args.Authenticated = true;
        //                string strInfo = "Success :-- UserName : " + lgnLogin.UserName + " | DateTime : " + DateTime.Now.ToString() + "  |IP: " + Request.ServerVariables["REMOTE_HOST"].ToString();
        //                new Log().logInfo("Login.lgnLogin_Authenticate", strInfo);
        //                bool retVal = objUserModule.UpdateLastLogin(lgnLogin.UserName, Session["APPID"].ToString());
        //            }
        //        }
        //        else
        //        {
        //            //Response.Write(errMessage.ToString());
        //            args.Authenticated = false;
        //            lgnLogin.FailureText = errMessage;
        //        }
        //    }
        //    else
        //    {
        //        lgnLogin.FailureText = "Invalid Login/password";
        //        args.Authenticated = false;
        //    }
        //}
    }

    new protected void Page_Error(object sender, EventArgs args)
    {
        Exception objExcep = Server.GetLastError();
        Server.ClearError();
        new Log().logException(sender.ToString(), objExcep);
        lgnLogin.FailureText = "Login processing failed.Please contact administrator";

    }
}
