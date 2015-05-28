using Com.Arubanetworks.Licensing.Lib.Data;
using Com.Arubanetworks.Licensing.Lib.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Validate : System.Web.UI.Page
{
    public void SetSession()
    {
        Session["BRAND"] = ConfigurationManager.AppSettings["ARUBA_BRAND"];
        Uri uriPath = Request.Url;
        string refpage = uriPath.AbsoluteUri;
        string strpath = Request.ApplicationPath;
        if (refpage.ToLower().Contains(ConfigurationManager.AppSettings["ALCATEL_WEB_SERVER"]))
        {
            // whichSite = "alcatel";
            Session["BRAND"] = ConfigurationManager.AppSettings["ALCATEL_BRAND"];
            Session["APPID"] = ConfigurationManager.AppSettings["ALCAPP_ID"];
            //altText = "Alcatel Networks";
        }
        else
        {
            // whichSite = "aruba";
            Session["BRAND"] = ConfigurationManager.AppSettings["ARUBA_BRAND"];
            Session["APPID"] = ConfigurationManager.AppSettings["LMSAPP_ID"];
            // altText = "Aruba Networks";
        }

    }

    public string ApplicationUrl
    {
        get
        {

            String sAbsUri = Request.Url.AbsoluteUri;
            String sRawUrl = Request.RawUrl;


            if (Request.ApplicationPath == "/")
                return sAbsUri.Substring(0, sAbsUri.Length -
           sRawUrl.Length);
            else
                return sAbsUri.Substring(0, sAbsUri.Length -
           sRawUrl.Length) + Request.ApplicationPath;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["USER_INFO"] != null)
        {
            // if user session is valid 
            RedirectUser();
        }

        if (Request != null && Request.Form["REF"] != null && !string.IsNullOrEmpty(Request.Form["REF"].ToString()))
        {
            string REF = Request.Form["REF"].ToString();
            WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings["SSO_RefID_Url"] + REF);

            request.Headers.Add("ping.instanceId", ConfigurationManager.AppSettings["SSO_InstanceID"]);
            String encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["SSO_Username"] + ":" + ConfigurationManager.AppSettings["SSO_Password"]));
            request.Headers.Add("Authorization", "Basic " + encoded);

            WebResponse response = request.GetResponse();

            System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());

            string JSonResponse = sr.ReadToEnd().Trim();
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

            dynamic obj = serializer.Deserialize(JSonResponse, typeof(object));

            if (obj == null)
            {
                Response.Redirect("~/Error.aspx");
            }

            string userEmail = obj.subject;
            var memberOf = obj.memberof;

            //JSONSSOResponse logJson = JavaScriptConvert.DeserializeObject<JSONSSOResponse>(JSonResponse);

            //if (logJson == null || string.IsNullOrEmpty(logJson.subject))
            //{
            //    Response.Redirect("~/Error.aspx");
            //}

            if (memberOf != null)
            {
                string strpageInfo = ConfigurationManager.AppSettings["PING_ALLOWED_ROLES"].ToString();
                string[] arrMemberInfo = strpageInfo.Split('|');

                bool validMember = false;
                if (arrMemberInfo != null && arrMemberInfo.Length > 0)
                {
                    foreach (string item in arrMemberInfo)
                    {
                        if (memberOf is System.Collections.Generic.List<object>)
                        {
                            List<object> ReqInfo = memberOf;
                            foreach (var reqItem in ReqInfo)
                            {
                                if (item.ToUpper().Equals(reqItem.ToString().ToUpper()))
                                {
                                    validMember = true;
                                    break;
                                }
                            }
                        }
                        else if (memberOf is string)
                        {
                            if (item.ToUpper().Equals(memberOf.ToUpper()))
                            {
                                validMember = true;
                                break;
                            }
                        }
                    }
                }

                if (!validMember)
                {
                    UnauthorizedAccessException ex = new UnauthorizedAccessException(userEmail + "un authorised access.");
                    new Log().logException("Login.lgnLogin_Authenticate", ex);
                    Session["ErrorInfo"] = "You are not authorized to use the system. Please contact administrator.";
                    Response.Redirect("~/Error.aspx");
                }
            }
            else
            {
                //Session["Error"] = "You are not authorized to use the system. Please contact administrator.";
                Response.Redirect("~/Error.aspx");
            }

            SetSession();

            AuthReturn validationresponse = Authenticate(userEmail);

            if (validationresponse != null)
            {
                if (!string.IsNullOrEmpty(validationresponse.redirectUrl))
                {
                    Response.Redirect(validationresponse.redirectUrl, false);
                }
                if (!string.IsNullOrEmpty(validationresponse.retMessage))
                {
                    Session["ErrorInfo"] = validationresponse.retMessage;
                    Response.Redirect("~/Error.aspx", false);
                }
                if (validationresponse.Authenticated == true)
                {
                    string arbPromo = ConfigurationManager.AppSettings["ARUBA_PROMO_ROLE"].ToString();
                    UserInfo roleType = new UserInfo();
                    roleType = (UserInfo)Session["USER_INFO"];
                    string numberOfRoles = roleType.GetUserRole();
                    if (numberOfRoles.Contains(arbPromo))
                    {
                        string promoUrl = ConfigurationManager.AppSettings["AIRWAVE_PROMO_URL"];
                        Response.Redirect(promoUrl);
                        return;

                    }

                    string url = ConfigurationManager.AppSettings["DEFAULT_URL"];
                    if (!string.IsNullOrEmpty(validationresponse.redirectUrl))
                    {
                        url = validationresponse.redirectUrl;
                    }
                    if (Session["UserRedirectpage"] != null)
                    {
                        url = Session["UserRedirectpage"].ToString();
                        Session["UserRedirectpage"] = null;
                    }


                    Response.Redirect(url, false);
                }
                else
                {
                    if (validationresponse.Authenticated == false && string.IsNullOrEmpty(validationresponse.retMessage))
                    {
                        Session["ErrorInfo"] = "Authentication falied. please try again.";
                        Response.Redirect("~/Error.aspx", false);
                    }
                }
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
        else
        {

            string redirecturl = "/";
            if (Session["UserRedirectpage"] != null)
            {
                redirecturl = Session["UserRedirectpage"].ToString();
            }
            else if (IsPostBack && HttpContext.Current.Request.UrlReferrer != null)
            {
                redirecturl = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            RedirectSSOLogin(redirecturl);


        }
    }

    protected void RedirectUser()
    {
        string arbPromo = ConfigurationManager.AppSettings["ARUBA_PROMO_ROLE"].ToString();
        UserInfo roleType = new UserInfo();
        roleType = (UserInfo)Session["USER_INFO"];
        string numberOfRoles = roleType.GetUserRole();
        if (numberOfRoles.Contains(arbPromo))
        {
            string promoUrl = ConfigurationManager.AppSettings["AIRWAVE_PROMO_URL"];
            Response.Redirect(promoUrl);
            return;

        }

        string url = ConfigurationManager.AppSettings["DEFAULT_URL"];

        if (Session["UserRedirectpage"] != null)
        {
            url = Session["UserRedirectpage"].ToString();
            Session["UserRedirectpage"] = null;
        }


        Response.Redirect(url, true);
    }

    public void RedirectSSOLogin(string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = "/";
        }

        string Url = ConfigurationManager.AppSettings["SSO_SERVER"] + HttpUtility.UrlEncode(ApplicationUrl + "/Validate.aspx") + (HttpUtility.UrlEncode("?DestUrl=" + returnUrl));
        Response.Redirect(Url, false);
    }

    protected AuthReturn Authenticate(string userName)
    {
        bool Authenticated = false;
        string retMessage = string.Empty;
        string redirectUrl = string.Empty;

        AuthReturn retObj = new AuthReturn();
        try
        {
            UserInfo objLicUser = new UserInfo();
            User objUserModule = new User();
            string errMessage = string.Empty;
            string UserIPAddr = Request.ServerVariables["REMOTE_HOST"].ToString();
            Email objEmails = new Email();

            //added by chetan - needed to run the app localhost
            if (UserIPAddr == "::1")
            {
                UserIPAddr = "10.1.1.1";
            }

            errMessage = objUserModule.AuthenticateUser(userName, UserIPAddr);
            objLicUser = objUserModule.GetUserInfo(userName, Session["BRAND"].ToString());

            if (errMessage.Equals(string.Empty))
            {
                if (objLicUser != null)
                {
                    if (!objLicUser.Status.ToUpper().Equals("ACTIVE"))
                    {
                        Authenticated = false;
                        retMessage = "Your account is not active";
                    }
                    else if (objLicUser.Role.Equals(string.Empty))
                    {
                        Authenticated = false;
                        retMessage = "Your account does not have access setup";
                    }
                    else if (objLicUser.Status.ToUpper().Equals("ACTIVE") && objUserModule.IsInactiveAccount(userName, ConfigurationManager.AppSettings["LMSAPP_ID"].ToString(), true))
                    {
                        AccountMailInfo objAccountInfo = new AccountMailInfo();
                        objAccountInfo.ActivationCode = objUserModule.getActivationCode(userName, Session["BRAND"].ToString());
                        objAccountInfo.Brand = Session["BRAND"].ToString().ToUpper();
                        objAccountInfo.Email = objLicUser.Email;
                        objAccountInfo.Name = objLicUser.FirstName + " " + objLicUser.LastName;
                        objEmails.sendActivationInfo(objAccountInfo);
                        retMessage = "Your account needs to be validated due to no activity.  Please confirm to reinstate via the activation email sent to your email ID.";
                    }
                    else
                    {
                        Session["USER_INFO"] = objLicUser;


                        //Session["EMail"] = objLicUser.Email;
                        Authenticated = true;
                        string strInfo = "Success :-- UserName : " + userName + " | DateTime : " + DateTime.Now.ToString() + "  |IP: " + Request.ServerVariables["REMOTE_HOST"].ToString();
                        new Log().logInfo("Login.lgnLogin_Authenticate", strInfo);
                        bool retVal = objUserModule.UpdateLastLogin(userName, Session["APPID"].ToString());
                    }
                }
                else
                {
                    if (objUserModule.isArubaEmp(userName) && Session["BRAND"].ToString() == ConfigurationManager.AppSettings["ARUBA_BRAND"].ToString())
                    {
                        objLicUser = objUserModule.GetEmpInfo(userName);
                        Session["USER_INFO"] = objLicUser;
                        // args.Authenticated = true;
                        string strInfo = "Success :-- UserName : " + userName + " | DateTime : " + DateTime.Now.ToString() + "  |IP: " + Request.ServerVariables["REMOTE_HOST"].ToString();
                        new Log().logInfo("Login.lgnLogin_Authenticate", strInfo);
                    }
                    else
                    {
                        // lgnLogin.FailureText = "Your account may not be not active or it does not exists!!";
                        // args.Authenticated = false;
                    }
                }
            }
            else
            {
                if (objLicUser != null)//invalid action, test and  handle this 
                {
                    if (errMessage.Contains("IP"))
                    {
                        if (ConfigurationManager.AppSettings["IP_REQUIRED"].ToString() == "YES")
                        {
                            //send email
                            Email objEmail = new Email();
                            AccountMailInfo objLgm = new AccountMailInfo();
                            objLgm.Email = objLicUser.Email;
                            objLgm.Brand = Session["BRAND"].ToString().ToUpper();
                            objLgm.Name = objLicUser.FirstName + " " + objLicUser.LastName;
                            objLgm.IPAddress = Request.ServerVariables["REMOTE_HOST"].ToString();
                            objLgm.ActivationCode = objUserModule.getIPAddressCode(objLgm.Email.ToString());
                            bool blResult = objEmail.sendIPActivationInfo(objLgm);
                            if (blResult == true)
                            {
                                retMessage = errMessage;
                            }
                            else
                            {
                                retMessage = "You are trying to Login with Different IP Address. For furhter detail,Please Contact TAC team";
                            }
                        }
                        else if (objLicUser.Status.ToUpper().Equals("ACTIVE") && objUserModule.IsInactiveAccount(userName, ConfigurationManager.AppSettings["LMSAPP_ID"].ToString(), true))
                        {
                            AccountMailInfo objAccountInfo = new AccountMailInfo();
                            objAccountInfo.ActivationCode = objUserModule.getActivationCode(userName, Session["BRAND"].ToString());
                            objAccountInfo.Brand = Session["BRAND"].ToString().ToUpper();
                            objAccountInfo.Email = objLicUser.Email;
                            objAccountInfo.Name = objLicUser.FirstName + " " + objLicUser.LastName;
                            objEmails.sendActivationInfo(objAccountInfo);
                            // lgnLogin.FailureText = "Your account needs to be validated due to no activity.  Please confirm to reinstate via the activation email sent to your email ID.";
                        }
                        else
                        {
                            Session["USER_INFO"] = objLicUser;

                            //Session["EMail"] = objLicUser.Email;
                            Authenticated = true;
                            string strInfo = "Success :-- UserName : " + userName + " | DateTime : " + DateTime.Now.ToString() + "  |IP: " + Request.ServerVariables["REMOTE_HOST"].ToString();
                            new Log().logInfo("Login.lgnLogin_Authenticate", strInfo);
                            bool retVal = objUserModule.UpdateLastLogin(userName, Session["APPID"].ToString());
                        }

                    }

                    else if (errMessage.Contains("Restricted Domain"))
                    {
                        if (ConfigurationManager.AppSettings["RESTRICTED_DOMAIN"].ToString() == "YES")
                        {
                            Session["EMail"] = objLicUser.Email;
                            string strPath = ConfigurationManager.AppSettings["UPDATE_DOMAIN_URL"];
                            //Response.Redirect(strPath, true);
                            redirectUrl = strPath;
                        }
                        else
                        {
                            Session["USER_INFO"] = objLicUser;
                            //Session["EMail"] = objLicUser.Email;
                            Authenticated = true;
                            string strInfo = "Success :-- UserName : " + userName + " | DateTime : " + DateTime.Now.ToString() + "  |IP: " + Request.ServerVariables["REMOTE_HOST"].ToString();
                            new Log().logInfo("Login.lgnLogin_Authenticate", strInfo);
                            bool retVal = objUserModule.UpdateLastLogin(userName, Session["APPID"].ToString());
                        }
                    }
                    else
                    {
                        //Response.Write(errMessage.ToString());
                        Authenticated = false;
                        retMessage = errMessage;
                    }
                }
                else
                {
                    retMessage = "Invalid Login/password";
                    Authenticated = false;
                }
            }
        }
        catch (Exception)
        {
            retObj = null;
            throw;
        }

        retObj.redirectUrl = redirectUrl;
        retObj.retMessage = retMessage;
        retObj.Authenticated = Authenticated;

        return retObj;
    }
}

public class AuthReturn
{
    public AuthReturn()
    { }

    public bool Authenticated = false;
    public string retMessage = string.Empty;
    public string redirectUrl = string.Empty;

}
public class JSONSSOResponse
{
    public JSONSSOResponse()
    { }
    public string authnCtx = string.Empty;
    public string partnerEntityID = string.Empty;
    public string subject = string.Empty;
    public string instanceId = string.Empty;
    public string sessionid = string.Empty;
    public string authnInst = string.Empty;
    public dynamic memberof;
}