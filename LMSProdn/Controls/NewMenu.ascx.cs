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
using System.Web.SessionState;

public partial class Controls_NewMenu : System.Web.UI.UserControl
{
    private string MENU_TEMPLATE = "<a href=\"{1}\" class=\"menuLink\"target=\"{2}\"  alink=\"#000000\">{0}</a>";
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["USER_INFO"] == null)
        //    Response.Redirect(ConfigurationManager.AppSettings["LOGIN_URL"], true);
        //else
        //{
        //    //if (!IsPostBack)
        //    //{
        //    //    UserInfo objLicUser = new UserInfo();
        //    //    objLicUser = (UserInfo)Session["USER_INFO"];
        //    //    LoadMenuItems(objLicUser.GetUserRole()); //comment for now



        //    //}
        //}
    }

    protected void Page_Init(object sender, EventArgs args)
    {
        if (Session["USER_INFO"] == null)
            Response.Redirect(ConfigurationManager.AppSettings["LOGIN_URL"], true);

        UserInfo objLicUser = new UserInfo();
        objLicUser = (UserInfo)Session["USER_INFO"];
        //Added input parameters acctId,brand - Ashwini
        LoadMenuItems(objLicUser.GetUserRole(), objLicUser.GetUserAcctId(),Session["BRAND"].ToString()); //comment for now
    }
    private void LoadMenuItems(string strUserRole, int usrAcctId,string strBrand)
    {
         DataSet dsMenus = (DataSet)Cache["MENU_ITEMS"];
        bool isActiveTab = false;
        string stBrand = Session["BRAND"].ToString();
        AjaxControlToolkit.TabPanel tbPanel;
        StringBuilder sb;
        ctrlTabs.Controls.Clear();
        if (dsMenus == null)
        {
            Lookup objLookup = new Lookup();
            dsMenus = objLookup.LoadMenuItems(strUserRole, usrAcctId,stBrand);
        }

        if (dsMenus.Tables[Lookup.MODULES_TBL].Rows.Count == 0 || dsMenus.Tables[Lookup.MENUS_TBL].Rows.Count == 0)
        {
            tbPanel = new AjaxControlToolkit.TabPanel();
            tbPanel.HeaderText = "No Items";
            return;

        }
        else
        {
            ctrlTabs.Controls.Clear();
            DataView dvModule = dsMenus.Tables[Lookup.MODULES_TBL].DefaultView;
            dvModule.Sort = "SortOrder";
            DataRowView drvModule ;
            for (int i = 0;i<dvModule.Count;i++)
            {
            drvModule = dvModule[i];
                DataView dvMenu = dsMenus.Tables[Lookup.MENUS_TBL].DefaultView;
                dvMenu.RowFilter = "ModuleId = " + Int32.Parse(drvModule["ModuleId"].ToString());
                dvMenu.Sort = "SortOrder";
                if (dvMenu.Count > 0)
                {
                    //add module node first
                    tbPanel = new AjaxControlToolkit.TabPanel();
                    tbPanel.HeaderText = drvModule["ModuleTitle"].ToString().ToUpper();
                    //tbPanel.CssClass = "CustomTabStyle";
                    
                    //temp = drvModule["ModuleUrl"].ToString();
                    //temp = temp.Replace("~", Request.ApplicationPath.ToString());
                    //temp = string.Format(MENU_TEMPLATE, drvModule["ModuleTitle"].ToString(), temp, drvModule["Target"].ToString());



                    Panel itemsPnl = new Panel();
                    //HtmlAnchor ha;
                    LiteralControl lc;
                    sb = new StringBuilder();
                    for(int j=0;j<dvMenu.Count;j++)
                    {
                        DataRowView drvMenuItem = dvMenu[j];
                        //ha = new HtmlAnchor();
                        

                        string temp;
                        temp = drvMenuItem["NavigationUrl"].ToString();
                        temp = temp.Replace("~", Request.ApplicationPath.ToString());

                        //ha.HRef = temp;
                        //ha.Target = drvMenuItem["Target"].ToString();
                        //ha.InnerText = drvMenuItem["MenuTitle"].ToString() + " ";
                       
                        
                        
                        //itemsPnl.Controls.Add(ha);

                        sb.Append(string.Format(MENU_TEMPLATE, drvMenuItem["MenuTitle"].ToString() + " ", temp, drvMenuItem["Target"].ToString()));
                        
                        if (j < (dvMenu.Count - 1))
                        {
                            sb.Append("<span >  |  </span>");
                             
                        }
                                              
                       
                       //set active tab 
                       if (Request.Url.ToString().IndexOf(drvMenuItem["NavigationUrl"].ToString().Replace("~", "")) > -1)
                       {
                           isActiveTab = true;
                                                 
                       }

                                 
         

                    }
                    lc = new LiteralControl();
                    lc.Text = "<span > "+sb.ToString()+" </span>";
                    
                    itemsPnl.Controls.Add(lc);
                    itemsPnl.Wrap = true;

                    tbPanel.Controls.Add(itemsPnl);
                    ctrlTabs.Controls.Add(tbPanel);
                    if (isActiveTab)
                    {
                        ctrlTabs.ActiveTab = tbPanel;
                      // tbPanel.CssClass = "SelectedTabStyle";
                       //tbPanel.BackColor = System.Drawing.Color.Orange;
                        isActiveTab = false;
                    }
                   
                }
            }


          

        }


    }

    private void _LoadMenuItems(string strUserRole)
    {
        //StringBuilder sb = new StringBuilder();
        //string  temp;
        //DataSet dsMenus = (DataSet)Cache["MENU_ITEMS"];
        //if (dsMenus == null)
        //{
        //    Lookup objLookup = new Lookup();
        //    dsMenus = objLookup.LoadMenuItems(strUserRole);
        //}

        //if (dsMenus.Tables[Lookup.MODULES_TBL].Rows.Count == 0 || dsMenus.Tables[Lookup.MENUS_TBL].Rows.Count == 0)
        //{
        //    MenuItem tn = new MenuItem("No Items");
        //    tn.NavigateUrl = string.Empty;
        //    nmMenu.Items.Add(tn);
        //}
        //else
        //{
        //    nmMenu.Items.Clear();
        //    DataView dvModule = dsMenus.Tables[Lookup.MODULES_TBL].DefaultView;
        //    dvModule.Sort = "SortOrder";
        //    foreach (DataRowView drvModule in dvModule)
        //    {
        //        sb = new StringBuilder();
        //        DataView dvMenu = dsMenus.Tables[Lookup.MENUS_TBL].DefaultView;
        //        dvMenu.RowFilter = "ModuleId = " + Int32.Parse(drvModule["ModuleId"].ToString());
        //        dvMenu.Sort = "SortOrder";
        //        if (dvMenu.Count > 0)
        //        {
        //            //add module node first
        //            //MenuItem tn = new MenuItem(drvModule["ModuleTitle"].ToString());
        //            temp = drvModule["ModuleUrl"].ToString();
        //            temp = temp.Replace("~", Request.ApplicationPath.ToString());
        //            temp = string.Format(MENU_TEMPLATE, drvModule["ModuleTitle"].ToString(), temp, drvModule["Target"].ToString());
        //            MenuItem tn = new MenuItem(temp);
                    
                    
        //            if (drvModule["ModuleUrl"].ToString().Equals(string.Empty))
        //                tn.Selectable = false;

        //            //tn.Target = drvModule["Target"].ToString();
        //            //create menu: child nodes

        //            foreach (DataRowView drvMenuItem in dvMenu)
        //            {
        //                temp = drvMenuItem["NavigationUrl"].ToString();
        //                temp =temp.Replace("~",Request.ApplicationPath.ToString());
        //                sb.Append(string.Format(MENU_TEMPLATE, drvMenuItem["MenuTitle"].ToString() + " ", temp, drvMenuItem["Target"].ToString()));
        //                sb.Append("  |  ");
        //                //MenuItem tnChild = new MenuItem(drvMenuItem["MenuTitle"].ToString() + " ", string.Empty, string.Empty, drvMenuItem["NavigationUrl"].ToString(), string.Empty);
        //                //tnChild.Target = drvMenuItem["Target"].ToString();
        //                //tn.ChildItems.Add(tnChild);

        //            }
        //            MenuItem tnChild = new MenuItem(sb.ToString().Trim().Substring(0, sb.ToString().Trim().Length-1));
        //            tnChild.Selectable = false;

        //            tn.ChildItems.Add(tnChild);
        //            nmMenu.Items.Add(tn);
        //        }
        //    }


        //    //////////////////////////////////////////////////////////////
        //    //tvMenu.Nodes.Clear();
        //    //DataView dvModule = dsMenus.Tables[Lookup.MODULES_TBL].DefaultView;
        //    //dvModule.Sort = "SortOrder";
        //    //foreach (DataRowView drvModule in dvModule)
        //    //{
        //    //    DataView dvMenu = dsMenus.Tables[Lookup.MENUS_TBL].DefaultView;
        //    //    dvMenu.RowFilter = "ModuleId = " + Int32.Parse(drvModule["ModuleId"].ToString());
        //    //    dvMenu.Sort = "SortOrder";
        //    //    //if(dvMenu.Count>=0)
        //    //    //{
        //    //    //add module node first
        //    //    TreeNode tn = new TreeNode(drvModule["ModuleTitle"].ToString());
        //    //    tn.NavigateUrl = drvModule["ModuleUrl"].ToString();
        //    //    tn.Target = drvModule["Target"].ToString();
        //    //    //create menu: child nodes
        //    //    foreach (DataRowView drvMenuItem in dvMenu)
        //    //    {
        //    //        TreeNode tnChild = new TreeNode(drvMenuItem["MenuTitle"].ToString() + " ", string.Empty, string.Empty, drvMenuItem["NavigationUrl"].ToString(), string.Empty);
        //    //        tnChild.Target = drvMenuItem["Target"].ToString();
        //    //        tn.ChildNodes.Add(tnChild);
        //    //    }
        //    //    tvMenu.Nodes.Add(tn);
        //    //    //}
        //    //}

        //}


    }

    private void __LoadMenuItems(string strUserRole)
    {
        //DataSet dsMenus = (DataSet)Cache["MENU_ITEMS"];
        //bool isActiveTab = false;
        //AjaxControlToolkit.TabPanel tbPanel;
        //StringBuilder sb;
        //ctrlTabs.Controls.Clear();
        //if (dsMenus == null)
        //{
        //    Lookup objLookup = new Lookup();
        //    dsMenus = objLookup.LoadMenuItems(strUserRole);
        //}

        //if (dsMenus.Tables[Lookup.MODULES_TBL].Rows.Count == 0 || dsMenus.Tables[Lookup.MENUS_TBL].Rows.Count == 0)
        //{
        //    tbPanel = new AjaxControlToolkit.TabPanel();
        //    tbPanel.HeaderText = "No Items";
        //    return;

        //}
        //else
        //{
        //    ctrlTabs.Controls.Clear();
        //    DataView dvModule = dsMenus.Tables[Lookup.MODULES_TBL].DefaultView;
        //    dvModule.Sort = "SortOrder";
        //    DataRowView drvModule;
        //    for (int i = 0; i < dvModule.Count; i++)
        //    {
        //        drvModule = dvModule[i];
        //        DataView dvMenu = dsMenus.Tables[Lookup.MENUS_TBL].DefaultView;
        //        dvMenu.RowFilter = "ModuleId = " + Int32.Parse(drvModule["ModuleId"].ToString());
        //        dvMenu.Sort = "SortOrder";
        //        if (dvMenu.Count > 0)
        //        {
        //            //add module node first
        //            tbPanel = new AjaxControlToolkit.TabPanel();
        //            tbPanel.HeaderText = drvModule["ModuleTitle"].ToString().ToUpper();
        //            //tbPanel.CssClass = "CustomTabStyle";

        //            //temp = drvModule["ModuleUrl"].ToString();
        //            //temp = temp.Replace("~", Request.ApplicationPath.ToString());
        //            //temp = string.Format(MENU_TEMPLATE, drvModule["ModuleTitle"].ToString(), temp, drvModule["Target"].ToString());



        //            //create menu: child nodes
        //            mnTopLevel = new Menu();

        //            //set CSS Classes for Menu
        //            mnTopLevel.DynamicEnableDefaultPopOutImage = false;
        //            mnTopLevel.StaticEnableDefaultPopOutImage = false;
        //            mnTopLevel.StaticBottomSeparatorImageUrl = "";
        //            mnTopLevel.PathSeparator = '|';

        //            mnTopLevel.DynamicHoverStyle.CssClass = "ActiveMenuItem";
        //            mnTopLevel.DynamicMenuItemStyle.CssClass = "ActiveMenuItem-Hover";
        //            mnTopLevel.StaticMenuItemStyle.CssClass = "ActiveMenuItem-Hover";
        //            mnTopLevel.StaticHoverStyle.CssClass = "ActiveMenuItem";
        //            mnTopLevel.StaticHoverStyle.ForeColor = System.Drawing.Color.White;
        //            mnTopLevel.StaticMenuItemStyle.ForeColor = System.Drawing.Color.White; ;
        //            mnTopLevel.DynamicMenuItemStyle.ForeColor = System.Drawing.Color.White; ;
        //            mnTopLevel.StaticMenuItemStyle.ItemSpacing = 0;
        //            mnTopLevel.StaticMenuItemStyle.HorizontalPadding = 7;
        //            mnTopLevel.DynamicMenuItemStyle.HorizontalPadding = 7;

        //            mnTopLevel.Orientation = Orientation.Horizontal;

        //            foreach (DataRowView drvMenuItem in dvMenu)
        //            {

        //                //temp = drvMenuItem["NavigationUrl"].ToString();
        //                //temp = temp.Replace("~", Request.ApplicationPath.ToString());
        //                // sb.Append(string.Format(MENU_TEMPLATE, drvMenuItem["MenuTitle"].ToString() + " ", temp, drvMenuItem["Target"].ToString()));
        //                ///sb.Append("  |  ");
        //                mnItem = new MenuItem(drvMenuItem["MenuTitle"].ToString() + " ", string.Empty, string.Empty, drvMenuItem["NavigationUrl"].ToString(), string.Empty);
        //                mnItem.Target = drvMenuItem["Target"].ToString();

        //                mnTopLevel.Items.Add(mnItem);
        //                //set active tab 
        //                if (Request.Url.ToString().IndexOf(drvMenuItem["NavigationUrl"].ToString().Replace("~", "")) > -1)
        //                {
        //                    isActiveTab = true;
        //                    mnItem.Selected = true;
        //                    mnTopLevel.BackColor = System.Drawing.Color.Orange;

        //                }
        //                else
        //                {
        //                    mnTopLevel.BackColor = System.Drawing.Color.White;
        //                }


        //            }

        //            tbPanel.Controls.Add(mnTopLevel);
        //            ctrlTabs.Controls.Add(tbPanel);
        //            if (isActiveTab)
        //            {
        //                ctrlTabs.ActiveTab = tbPanel;
        //                // tbPanel.CssClass = "SelectedTabStyle";
        //                //tbPanel.BackColor = System.Drawing.Color.Orange;
        //                isActiveTab = false;
        //            }

        //        }
        //    }




        //}


    }
}
