using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Com.Arubanetworks.Licensing.Lib;
using Com.Arubanetworks.Licensing.Lib.Data;

public partial class Pages_Default : BasePage 
{
    protected string prefixControlId = "control_";
    protected void Page_Load(object sender, EventArgs e)
    {
        string ModId = string.Empty;
        string tabId = string.Empty;
        try
        {
            runPagePreReqs();
            if (Request.Params["tabID"] != null)
            {
                tabId = Request.Params["tabID"];
            }
            if (!string.Empty.Equals(tabId))
            {
                if (tabId.ToUpper() == "HOME")
                {
                    ModId = Session["PREV_MODULE"].ToString();
                    LoadModDescription(ModId);
                }
                plTab.Controls.Clear();
                System.Web.UI.Control ctlUserControl = new System.Web.UI.Control();
                ctlUserControl = LoadControl(@Request.ApplicationPath + "/Controls/" + tabId + ".ascx");
                //Response.Write(@Request.ApplicationPath + "/Controls/" + tabId + ".ascx");
                ctlUserControl.ID = prefixControlId + tabId;
                plTab.Controls.Add(ctlUserControl);
            }
            else
            {
                if(Session["USER_INFO"]!=null)
                    Response.Redirect(ConfigurationManager.AppSettings["DEFAULT_URL"], true);
                else
                    Response.Redirect(ConfigurationManager.AppSettings["LOGIN_URL"], true);
            }
        }
        catch (Exception exp)
        {
            ModId = string.Empty;
        }             
    }

    private void LoadModDescription(string strModId)
    {
        Certificate objCert = new Certificate();
        string strModDesc = string.Empty;
        strModDesc = objCert.getModuleDescription(strModId);
        LitrModDesc.Text = strModDesc;
    }
}