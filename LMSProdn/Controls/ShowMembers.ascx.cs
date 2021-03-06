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

public partial class Controls_ShowMembers : System.Web.UI.UserControl
{
    private DataSet GridData = new DataSet();
    int rowCount = 0;
    protected static Hashtable columnNames = null;
    bool addSortDirectionIcon = false;
    DataSet dsLookupValues = new DataSet();
    private string strUser = string.Empty;
    User objUser;
    private string strBrand = string.Empty;
    protected ArrayList expressions;
    protected ArrayList orders;
    protected ArrayList columns;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false; 

        if (Session[Company.COMPANY_ID] == null || Session[Company.COMPANY_ID].ToString().Equals(string.Empty))
            Response.Redirect(Request.UrlReferrer.ToString(), true);



        string remoteUser = ((UserInfo)Session["USER_INFO"]).GetUserEmail();
        strBrand = ((UserInfo)Session["USER_INFO"]).Brand;

        objUser = new User();
        if (!IsPostBack)
        {

            Company objCom = new Company();
            DataSet ds = objCom.GetCompanyInfo(Int32.Parse(Session[Company.COMPANY_ID].ToString()));
            if (ds == null)
            {
                Response.Redirect(Request.UrlReferrer.ToString(), true);
            }
            else
            {

                Session[Company.COMPANY_TYPE] = ds.Tables[0].Rows[0][Company.COMPANY_TYPE].ToString();
                string strCompanyType = (Session[Company.COMPANY_TYPE].ToString() == CompanyType.Customer) ? CompanyType.Customer : Session[Company.COMPANY_TYPE].ToString();
                setError(lblCompanyInfo,"Showing members for "+strCompanyType+" :"+ds.Tables[0].Rows[0][Company.COMPANY_NAME].ToString());
            }


            createColumnNamesList();
            gvPR.Attributes["RecordedExpression"] = "";
            gvPR.Attributes["FilterQuery"] = "";

            GridData = objUser.GetMembers(Int32.Parse(Session[Company.COMPANY_ID].ToString()), gvPR.PageSize, 1, string.Empty);
            setDefaultSortingPattern(); // Set a default sorting pattern
            UpdateDataView();


            //load Dept list
            Lookup objLookup = new Lookup();
            dsLookupValues = (DataSet)Cache[Lookup.LOOKUP_TBL];
            if (dsLookupValues == null)
            {
                dsLookupValues = objLookup.LoadLookupValues(remoteUser, Session["BRAND"].ToString());
                Cache.Insert(Lookup.LOOKUP_TBL, dsLookupValues, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }
            UIHelper.PrepareAndBindLookupFromAllWithoutPlease(ddlOperators, dsLookupValues.Tables[Lookup.LOOKUP_TBL], "OPERATORS", "TXT", "VAL", false);
            BindFilterColumns();

        }
        ShowFilterParams();
    }

    private void createColumnNamesList()
    {
        columnNames = new Hashtable();
        columnNames.Add("FirstName", "First Name,0");
        columnNames.Add("LastName", "Last Name,1");
        columnNames.Add("Email", "Email,2");
        columnNames.Add("Phone", "Phone,3");
        columnNames.Add("Company", "User-entered Company,4");
        columnNames.Add("AccountType", "Account Type,5");
        columnNames.Add("Status", "Status,6");
        columnNames.Add("CreatedOn", "Created On,7");



    }

    private void BindFilterColumns()
    {

        string[] arrFilCols = { "First Name|"+User.FIRST_NAME,
                                "Last Name|"+User.LAST_NAME,
                                "Email|"+User.EMAIL,    
                                "Phone|"+User.PHONE,
                                "Account Type|"+User.USER_TYPE,
                                "User-entered Company|"+User.USER_ENTERED_COMPANY,
                                "Status|"+User.STATUS                                
                                };
        DataTable dt = UIHelper.GetTableFromStringArray(arrFilCols, '|');
        UIHelper.PrepareAndBindListWithoutPlease(ddlColumns, dt, "TXT", "VAL", false);

    }

    private void setDefaultSortingPattern()
    {
        gvPR.Attributes["defaultSort"] = Sort.TRUE_CASE;
        string defaultPattern = "LastName ASC";
        Sort.PrepareSortingString(defaultPattern, gvPR);
    }

    private void UpdateDataView()
    {

        DataSet myDataSet = GridData;//vivek:sp
        if (myDataSet != null)
        {
            rowCount = Int32.Parse(myDataSet.Tables[1].Rows[0]["TotalRecords"].ToString());
            DataView dataView = new DataView(myDataSet.Tables[0]);
            string sortString = Sort.ReadSortingString(gvPR);
            dataView.Sort = sortString;
            gvPR.DataSource = dataView;
            gvPR.DataBind();
            updatePageStatus();
        }

    }

    private void updatePageStatus()
    {
        if (rowCount > 0)
        {
            currentPage.Text = GridData.Tables[1].Rows[0]["PageNumber"].ToString();
            totalPages.Text = GridData.Tables[1].Rows[0]["TotalPages"].ToString();
            totalRecords.Text = rowCount + "";
        }
        else
        {
            currentPage.Text = "1";
            totalPages.Text = "1";
            totalRecords.Text = "0";
        }
    }

    public void linkButton_Click(object sender, EventArgs e)
    {
        string arg = ((LinkButton)sender).CommandArgument;
        _PagerButtonClick(arg);
    }

    public void PagerButtonClick(Object sender, ImageClickEventArgs e)
    {
        String arg = ((ImageButton)sender).CommandArgument;
        _PagerButtonClick(arg);


    }

    public void _PagerButtonClick(string arg)
    {

        int CurrentPage = Int32.Parse(currentPage.Text);
        int Totalpages = Int32.Parse(totalPages.Text);
        Certificate objCert = new Certificate();
        string Filter = GetFilter();
        switch (arg)
        {
            case ("next"):

                if (CurrentPage < Totalpages)
                {
                    GridData = objUser.GetMembers(Int32.Parse(Session[Company.COMPANY_ID].ToString()), gvPR.PageSize, CurrentPage + 1, Filter); 
                    UpdateDataView();
                }
                break;
            case ("prev"):

                if (CurrentPage > 1)
                {
                    GridData = objUser.GetMembers(Int32.Parse(Session[Company.COMPANY_ID].ToString()), gvPR.PageSize, CurrentPage - 1, Filter); 
                    UpdateDataView();
                }
                break;
            case ("last"):

                if (CurrentPage != Totalpages && Totalpages > 1)
                {
                    GridData = objUser.GetMembers(Int32.Parse(Session[Company.COMPANY_ID].ToString()), gvPR.PageSize, Totalpages, Filter); 
                    UpdateDataView();
                }
                break;
            default:
                //if (Totalpages >= 1)
                {
                    GridData = objUser.GetMembers(Int32.Parse(Session[Company.COMPANY_ID].ToString()), gvPR.PageSize, 1, Filter); 
                    UpdateDataView();
                }
                break;
        }

    }

    public void gvPR_Sort(Object sender, GridViewSortEventArgs e)
    {
        if (gvPR.Rows.Count > 0)
        {
            ArrayList list = null; expressions = null; orders = null; columns = null;
            addSortDirectionIcon = true;
            if (gvPR.Attributes["defaultSort"] == Sort.TRUE_CASE)
            {
                gvPR.Attributes["RecordedExpression"] = "";
                gvPR.Attributes["defaultSort"] = Sort.FALSE_CASE;
            }
            // Sets the new sorting fields and their orders. Internally,
            // the orders are reverted if you clicked on a sorted column.
            Sort.PrepareSortingString(e.SortExpression, gvPR);
            list = Sort.GetExpressionNOrders(Sort.ReadSortingString(gvPR));
            expressions = (ArrayList)list[Sort.EXPRESSIONS];
            orders = (ArrayList)list[Sort.ORDERS];
            columns = Sort.GetColumnNum(columnNames.GetEnumerator(), expressions);

            GridData = objUser.GetMembers(Int32.Parse(Session[Company.COMPANY_ID].ToString()), gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter()); 
            
            // Refreshes the view	
            UpdateDataView();
        }
    }

    public void lnkClearSort_Click(object sender, EventArgs args)
    {

        // sortDescription.Text = "";
        addSortDirectionIcon = false;
        gvPR.Attributes["RecordedExpression"] = "";
        setDefaultSortingPattern();

        GridData = objUser.GetMembers(Int32.Parse(Session[Company.COMPANY_ID].ToString()), gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter()); 
        

        // Refreshes the view	
        UpdateDataView();


    }


    public void lnkbtnFetch_Click(object sender, ImageClickEventArgs args)
    {
        lnkClearSort_Click(sender, args);

    }

    public void lnkRemfilter_OnClick(object sender, EventArgs args)
    {

        gvPR.Attributes["FilterQuery"] = string.Empty;
        _PagerButtonClick("first");
        ShowFilterParams();
        lblFilter.Text = string.Empty;
        txtSearch.Text = string.Empty;
        ddlColumns.ClearSelection();
        ddlOperators.ClearSelection();
    }
    public void btnGo_OnClick(object sender, ImageClickEventArgs args)
    {
        Setfilter();
        _PagerButtonClick("first");
        ShowFilterParams();

    }



    protected void ItemCellsUpdate(object sender, GridViewRowEventArgs e)
    {

        if (addSortDirectionIcon)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (orders != null && columns != null)
                {
                    for (int ii = 0; ii < orders.Count; ii++)
                    {
                        Label icon = Sort.getSortIconLabel();
                        icon.Text = (((String)orders[ii]).Equals(Sort.ORDER_ASC)) ? "Λ" : "V";
                        int index = Int32.Parse((String)columns[ii]);
                        if (index >= 0)
                        {
                            e.Row.Cells[index].Controls.Add(icon);
                        }
                    }
                }
            }
        }
        addSortDirectionIcon = false;
    }

    private void ShowFilterParams()
    {
        bool blnShow = false; ;
        if (gvPR.Attributes["FilterQuery"].Equals(string.Empty))
        {
            blnShow = true;
        }
        pnlFilterParams.Visible = blnShow;
        pnlFilterQuery.Visible = !pnlFilterParams.Visible;
    }
    private string GetFilter()
    {
        if (gvPR.Attributes["FilterQuery"].Equals(string.Empty))
        {
            return string.Empty;
        }
        string[] arrFilter = gvPR.Attributes["FilterQuery"].Split('^');
        return UIHelper.GetFilterSql(arrFilter[0], arrFilter[1], arrFilter[2]);

    }

    private void Setfilter()
    {
        gvPR.Attributes["FilterQuery"] = ddlColumns.SelectedValue + "^" + ddlOperators.SelectedValue + "^" + txtSearch.Text.Trim();
        lblFilter.Text = ddlColumns.SelectedItem.Text + "  " + ddlOperators.SelectedItem.Text + "  " + txtSearch.Text.Trim();

    }

    protected void lnkRemMember_OnCommand(object sender, EventArgs args)
    {
        Company objCompany = new Company();
        if (((UserInfo)Session["USER_INFO"]).GetUserRole().Equals(UserType.Distributor))
        {
            string strDistId = ((UserInfo)Session["USER_INFO"]).CompanyId.ToString();
            if (strDistId == null || strDistId.Trim() == string.Empty || strDistId.Equals("-1"))
            {
                setError(lblErr, Company.NO_COMPANY_INFO);
                return;
            }
            else
            {
                int intDistId = Int32.Parse(strDistId);
           
                if(Session[Company.COMPANY_ID]==null||Session[Company.COMPANY_ID].ToString().Equals(string.Empty))
                {setError(lblErr,UIHelper.REQ_ERROR);
                    return;
                }
                if (!objCompany.IsMyDistributor(intDistId, Int32.Parse(Session[Company.COMPANY_ID].ToString())))
                {
                    setError(lblErr, Company.FAILURE_IS_MY_DIST);
                    return;
                
                }
            
            }
        
        }

        int intAcctId = Int32.Parse(((LinkButton)sender).CommandArgument.ToString().Trim());
        if (objCompany.RemoveMember(intAcctId))
            _PagerButtonClick("first");
        else
            setError(lblErr, Company.FAILURE_REMOVE_MEMBER);
    }

    private void setError(Label lbl, string text)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }

    protected void lnkAddMembers_OnClick(object sender, EventArgs args)
    {
        Response.Redirect(ConfigurationManager.AppSettings["ADDMEMBERS_URL"], true);
    }
}
