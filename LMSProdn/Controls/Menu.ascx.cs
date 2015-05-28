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
using System.Web.Caching;
using Com.Arubanetworks.Licensing.Lib.Data;
using Com.Arubanetworks.Licensing.Lib.Utils;

public partial class Controls_Menu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //txtSearch.Attributes.Clear();
       // txtSearch.Attributes.Add("onfocus", "javascript:clearSearchTxt('" + txtSearch.ClientID + "')");
        //txtSearch.Attributes.Add("onblur", "javascript:setSearchTxt('" + txtSearch.ClientID + "')");
          //  string sUrl = Request.Url.ToString();
          //  string page = sUrl.Substring(sUrl.LastIndexOf("/")+1, sUrl.LastIndexOf(".") - sUrl.LastIndexOf("/"));
          //foreach (TreeNode tvn in TreeView1.Nodes  )
          //{
          //    if (tvn.Value.Equals(Page))
          //    {
          //        tvn.Expand();
          //        break;
          //    }
          //}

        if (Session["USER_INFO"] == null)
            Response.Redirect(ConfigurationManager.AppSettings["LOGIN_URL"], true);
        else
        {
            if (!IsPostBack)
            {
                UserInfo objLicUser = new UserInfo();
                objLicUser = (UserInfo)Session["USER_INFO"];
                LoadMenuItems(objLicUser.GetUserRole(),objLicUser.GetUserAcctId(),Session["BRAND"].ToString()); //comment for now
                
                pnlImpUser.Visible = true;
                if (objLicUser.IsImpersonate)
                {
                    pnlImpUser.Visible = true;
                    lnkImpLogOff.Text = "Logoff " + objLicUser.ImpersonateEmail;
                }
                

            }
         }

        //if (Session["SEARCH_CRITERIA"] != null)
        //{
        //    txtSearch.Text = Session["SEARCH_CRITERIA"].ToString();
        //}


        //this.Page.ClientScript.RegisterHiddenField("__EVENTTARGET", lnkbtnBasicSearch.ClientID.ToString());
        
        
            

    }

    private void LoadMenuItems(string strUserRole, int usrAcctId,string strBrand)
    {
        DataSet dsMenus = (DataSet)Cache["MENU_ITEMS"];
        if (dsMenus == null)
        {
            Lookup objLookup = new Lookup();
            dsMenus = objLookup.LoadMenuItems(strUserRole,usrAcctId,Session["BRAND"].ToString());
        }

        if (dsMenus.Tables[Lookup.MODULES_TBL].Rows.Count == 0 || dsMenus.Tables[Lookup.MENUS_TBL].Rows.Count == 0)
        {
            TreeNode tn = new TreeNode("No Items");
            tn.NavigateUrl = string.Empty;
            tvMenu.Nodes.Add(tn);
        }
        else
        {
            tvMenu.Nodes.Clear();
            DataView dvModule = dsMenus.Tables[Lookup.MODULES_TBL].DefaultView;
            dvModule.Sort="SortOrder";
            foreach (DataRowView drvModule in dvModule)
            {
                DataView dvMenu = dsMenus.Tables[Lookup.MENUS_TBL].DefaultView;
                dvMenu.RowFilter="ModuleId = " + Int32.Parse(drvModule["ModuleId"].ToString());
                dvMenu.Sort="SortOrder";
                //if(dvMenu.Count>=0)
                //{
                //add module node first
                    TreeNode tn = new TreeNode(drvModule["ModuleTitle"].ToString());
                    tn.NavigateUrl = drvModule["ModuleUrl"].ToString();
                    tn.Target = drvModule["Target"].ToString();
                    //create menu: child nodes
                    foreach (DataRowView drvMenuItem in dvMenu)
                    {
                        TreeNode tnChild = new TreeNode(drvMenuItem["MenuTitle"].ToString()+" ", string.Empty, string.Empty, drvMenuItem["NavigationUrl"].ToString(), string.Empty);
                        tnChild.Target = drvMenuItem["Target"].ToString();
                        tn.ChildNodes.Add(tnChild);
                    }
                    tvMenu.Nodes.Add(tn);
               //}
            }
        
        }
        

    }

    protected void lnkbtnFetch_Click(object sender, EventArgs args)
    {
        rfvSearch.Validate();
        if (rfvSearch.IsValid)
        {
            Session["SEARCH_CRITERIA"] = txtSearch.Text.Trim();
            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["SEARCHRESULTSASPX"], true);
        
        }
        else 
        {
            Session["SEARCH_CRITERIA"] = null;
        }

    
    }

    protected void lnkImpLogOff_OnClick(object sender, EventArgs args)
    {
        UserInfo objLicUser = new UserInfo();
        objLicUser = (UserInfo)Session["USER_INFO"];
        objLicUser.IsImpersonate = false;
        objLicUser.ImpersonateEmail = string.Empty;
        objLicUser.ImpersonateUserRole = string.Empty;
        Session["USER_INFO"] = objLicUser;
        Response.Redirect(ConfigurationManager.AppSettings["DEFAULT_URL"].ToString(), true);
            
    }
}
