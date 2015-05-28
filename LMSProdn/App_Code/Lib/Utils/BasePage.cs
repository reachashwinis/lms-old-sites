using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Com.Arubanetworks.Licensing.Lib.Data;


/// <summary>
/// BasePage for all Licensing ASPX pagess
/// </summary>
namespace Com.Arubanetworks.Licensing.Lib
{
    public class BasePage : System.Web.UI.Page
    {

        public BasePage()
        {

        }

        public bool IsSessionExpired()
        {
            if (Session["USER_INFO"] == null || Session["BRAND"] == null || Session["APPID"] == null)
            {
                return true;
            }
            return false;

        }

        public void CheckSiteClosed()
        {
            if (IsSiteClosed())
            {
                Response.Redirect(ConfigurationManager.AppSettings["SITE_CLOSED_URL"], true);
            }

        }

        public bool IsSiteClosed()
        {
            if (ConfigurationManager.AppSettings["SITE_OPEN"].ToLower().Equals("false"))
            {
                return true;
            }
            return false;
        }

        public void runPagePreReqs()
        {
            if (IsSessionExpired())
            {
                Response.Redirect(ConfigurationManager.AppSettings["LOGIN_URL"], true);
            }
            if (IsSiteClosed())
            {
                Response.Redirect(ConfigurationManager.AppSettings["SITE_CLOSED_URL"], true);
            }
                    
        }

        protected void Page_Error(object sender, EventArgs args)
        {
            Exception objExcep = Server.GetLastError();
            Server.ClearError();
            new Lib.Utils.Log().logException(sender.ToString(), objExcep);
            Response.Redirect(ConfigurationManager.AppSettings["ERROR_URL"], true);
        }

        public void checkAccessToPage(string PageType, int PageId, string role,string strUser,int AcctId)
        {
            Dataaccesslayer.DALookup objDALookup = new Com.Arubanetworks.Licensing.Dataaccesslayer.DALookup();
            if (objDALookup.IsPageVisible(PageType, PageId, role,AcctId) == false)
            {
                new Lib.Utils.Log().logInfo("FORCE_ACCESS", strUser + " tried to access PageId:" + PageId + " PageType:" + PageType);
                Response.Redirect(ConfigurationManager.AppSettings["ACCESS_DENIED_URL"], true);
            }
        }
        public void checkAuthPersonnel(string PageName, string strUser)
        {
            Dataaccesslayer.DALookup objDALookup = new Com.Arubanetworks.Licensing.Dataaccesslayer.DALookup();
            if (objDALookup.IsAuthorisedPersonnel(PageName, strUser) == false)
            {
                new Lib.Utils.Log().logInfo("FORCE_ACCESS", strUser + " tried to access Page, PageCode:" + PageName );
                Response.Redirect(ConfigurationManager.AppSettings["ACCESS_DENIED_URL"], true);
            }
        }
        public void checkForReferredPage()
        {
            bool retVal = true;
            if (Request.UrlReferrer == null || Request.UrlReferrer.ToString().Equals(string.Empty))
            {
                retVal = false;
            }
            else
            {
                if (!Request.UrlReferrer.ToString().Contains(Request.ServerVariables["SERVER_NAME"]))
                {
                    retVal=false;
                
                }
            }
            if(retVal==false)
            {
            Response.Redirect(ConfigurationManager.AppSettings["DEFAULT_URL"],true);
            }
        }

    }
}


