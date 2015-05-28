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
using System.Web.Caching;

public partial class controls_topNavHome : System.Web.UI.UserControl
{
    private DataSet dsMenus;
    private bool isCurrentPageSet = false;
    string selectedMod = "-1";
    protected void Page_Load(object sender, EventArgs e)
    {
              
        if (Session["USER_INFO"] == null)
            Response.Redirect(ConfigurationManager.AppSettings["LOGIN_URL"], true);
        else
        {
            UserInfo objLicUser = new UserInfo();
            objLicUser = (UserInfo)Session["USER_INFO"];
            LoadMenuItems(objLicUser.GetUserRole(),objLicUser.GetUserAcctId(),Session["BRAND"].ToString()); //comment for now
            if (IsPostBack)
            {
                getModuleFromUrl();
            }
            populateMenu(selectedMod);
            Label1.Text = objLicUser.FirstName + " " + objLicUser.LastName + "(" + Request.ServerVariables["REMOTE_HOST"].ToString() + ")";
        }	
    }

	protected void BindSubMenu(string ModuleId)
	{       
            DataView dvMenu = dsMenus.Tables[Lookup.MENUS_TBL].DefaultView;
            dvMenu.RowFilter = "ModuleId = " + ModuleId;
            dvMenu.Sort = "SortOrder";
            dlSubMenu.DataSource = dvMenu;
            dlSubMenu.DataBind();
	}

    protected void dlTopMenu_DataBound(object sender, DataListItemEventArgs e)
    {
        if (!isCurrentPageSet)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {                             
                 LinkButton hl = (LinkButton)e.Item.FindControl("lnkMod");
                 if (hl != null)
                 {
                     if ( selectedMod == hl.CommandArgument) //current page
                     {

                         e.Item.CssClass = "topMenuSelectedItem";
                         isCurrentPageSet = true;
                         BindSubMenu(selectedMod);
                         Session["PREV_MODULE"] = selectedMod;
                     }

                 }
            }
        }
    }
                
   protected void lnkMod_OnClick(object sender, EventArgs args)
    {
        selectedMod = ((LinkButton)sender).CommandArgument;
        UserInfo objLicUser = new UserInfo();
        objLicUser = (UserInfo)Session["USER_INFO"];
        LoadMenuItems(objLicUser.GetUserRole(),objLicUser.GetUserAcctId(),Session["BRAND"].ToString()); //comment for now
        populateMenu(selectedMod);
        Response.Redirect("~/Pages/Default.aspx?tabId=Home", true);
        Session["PREV_MODULE"] = selectedMod;      
    }

    private void LoadMenuItems(string strUserRole, int usrAcctId,string strBrand)
    {
        UserInfo objLicUser = (UserInfo)Session["USER_INFO"];
        dsMenus = (DataSet)Cache["MENU_ITEMS_"+objLicUser.GetUserAcctId().ToString()];


        if (dsMenus == null)
        {
            Lookup objLookup = new Lookup();
            dsMenus = objLookup.LoadMenuItems(strUserRole, usrAcctId, Session["BRAND"].ToString());
            Cache.Add("MENU_ITEMS_" + objLicUser.GetUserAcctId().ToString(), dsMenus, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(1), CacheItemPriority.High, null);
        }     

    }

    private void populateMenu(string ModuleId)
    {
        isCurrentPageSet = false;

        if (dsMenus.Tables[Lookup.MODULES_TBL].Rows.Count > 0 && dsMenus.Tables[Lookup.MENUS_TBL].Rows.Count > 0)
        {
            DataView dvModule = dsMenus.Tables[Lookup.MODULES_TBL].DefaultView;
            dvModule.Sort = "SortOrder";

            if (ModuleId != "-1")
            {
                //ModuleId = Session["PREV_MODULE"].ToString();
                selectedMod = ModuleId;
               
            }
            else
            {

                getModuleFromUrl();
            
            }
            dlTopMenu.DataSource = dvModule;
            dlTopMenu.DataBind();
            Session["PREV_MODULE"] = selectedMod;
        }
    
    }

    public void Change_Color(object sender, EventArgs e)
    {
        ((HyperLink)dlSubMenu.FindControl("HyperLink1")).BackColor = System.Drawing.Color.Blue;
    }
    private void getModuleFromUrl()
    {
        string url = Request.FilePath.ToString();
        DataView dvMenus = dsMenus.Tables[Lookup.MENUS_TBL].DefaultView;
        dvMenus.RowFilter = "NavigationUrl = '" + url.Replace(Request.ApplicationPath, "~") + "'";
        if (dvMenus.Count > 0)
        {
            selectedMod = dvMenus[0]["ModuleId"].ToString();
            Session["PREV_MODULE"] = selectedMod;
        }
        else
        {
            if (Session["PREV_MODULE"] != null)
                selectedMod = Session["PREV_MODULE"].ToString();
            else
                selectedMod = "1";//certificate management

            //Session["PREV_MODULE"] = "1";//certificate management
        
        }
    }
}
