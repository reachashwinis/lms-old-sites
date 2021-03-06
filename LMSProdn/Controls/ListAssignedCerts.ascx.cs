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

public partial class Controls_ListAssignedCerts : System.Web.UI.UserControl
{
    private DataSet GridData = new DataSet();
    int rowCount = 0;
    protected static Hashtable columnNames = null;
   
    bool addSortDirectionIcon = false;
    DataSet dsLookupValues = new DataSet();
    private string strUser = string.Empty;
    Certificate objCert;

    protected ArrayList expressions;
    protected ArrayList orders;
    protected ArrayList columns;
    int intCompanyId = -1;

    private const string FAILURE_UNASSIGN = "Unable to unassign certificate";

    protected void Page_Load(object sender, EventArgs e)
    {
        string remoteUser = ((UserInfo)Session["USER_INFO"]).GetUserEmail();
        intCompanyId = ((UserInfo)Session["USER_INFO"]).GetUserCompanyId();
        objCert = new Certificate();
        if (!IsPostBack)
        {
            createColumnNamesList();
            gvPR.Attributes["RecordedExpression"] = "";
            gvPR.Attributes["FilterQuery"] = "";

            GridData = objCert.GetAssignedCerts(intCompanyId, gvPR.PageSize, 1, GetCertTypeToView());
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
        columnNames.Add("Part_Id", "License Part Id,0");
        columnNames.Add("Part_Desc", "Description,1");
        columnNames.Add("Serial_Number", "Certificate Id,2");
        columnNames.Add("Reseller_Name", "Assigned Reseller,3");
        columnNames.Add("Assigned_On", "Assigned On,4");
    }

    private void BindFilterColumns()
    {

        string[] arrFilCols = { 
                                "License Part Id|Part_Id",
                                "Description|Part_Desc",
                                "Certificate Id|Serial_Number",
                                "Assigned Reseller|Reseller_Name",
                                "Assigned On|Assigned_On"
                                };
        DataTable dt = UIHelper.GetTableFromStringArray(arrFilCols, '|');
        UIHelper.PrepareAndBindListWithoutPlease(ddlColumns, dt, "TXT", "VAL", false);

    }
    private void setDefaultSortingPattern()
    {
        gvPR.Attributes["defaultSort"] = Sort.TRUE_CASE;
        string defaultPattern = "Reseller_Name ASC";
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

    public void lnkDload_OnClick(object sender, EventArgs e)
    {
        string arg = ((LinkButton)sender).CommandArgument;
        _PagerButtonClick(arg);
    }

    public void lnkDloadEx_OnClick(object sender, EventArgs e)
    {
        string arg = ((LinkButton)sender).CommandArgument;
        _PagerButtonClick(arg);
    }

    public void _PagerButtonClick(string arg)
    {

        int CurrentPage = Int32.Parse(currentPage.Text);
        int Totalpages = Int32.Parse(totalPages.Text);
        Certificate objCert = new Certificate();
        string Filter = GetFilter();
        int TotalRecords = Int32.Parse(totalRecords.Text);
        if (!Filter.Trim().Equals(string.Empty))
        {
            Filter += " AND " + GetCertTypeToView();
        }
        else
        {
            Filter = GetCertTypeToView();
        }
        switch (arg)
        {
            case ("next"):

                if (CurrentPage < Totalpages)
                {
                    GridData = objCert.GetAssignedCerts(intCompanyId, gvPR.PageSize, CurrentPage + 1, Filter);
                    UpdateDataView();
                }
                break;
            case ("prev"):

                if (CurrentPage > 1)
                {

                    GridData = objCert.GetAssignedCerts(intCompanyId, gvPR.PageSize, CurrentPage - 1, Filter);
                    UpdateDataView();
                }
                break;
            case ("last"):

                if (CurrentPage != Totalpages && Totalpages > 1)
                {
                    GridData = objCert.GetAssignedCerts(intCompanyId, gvPR.PageSize, Totalpages, Filter);
                    UpdateDataView();
                }
                break;
            case ("all"):
                if (gvPR.Attributes["FilterQuery"].Equals(string.Empty))
                {
                    GridData = objCert.GetAssignedCerts(intCompanyId, TotalRecords, 1, Filter);
                }
                else
                {
                    GridData = objCert.GetAssignedCerts(intCompanyId, gvPR.PageSize, 1, Filter);
                }
                //DatasettoExcel.ConvertXML(GridData, Response);
                DatasettoExcel.ExportToTextFile(GridData, ConfigurationManager.AppSettings["DELIMITER"].ToString(), ((UserInfo)Session["USER_INFO"]).GetUserEmail(), Response);
                break;
            case ("allEx"):
                if (gvPR.Attributes["FilterQuery"].Equals(string.Empty))
                {
                    GridData = objCert.GetAssignedCerts(intCompanyId, TotalRecords, 1, Filter);
                }
                else
                {
                    GridData = objCert.GetAssignedCerts(intCompanyId, gvPR.PageSize, 1, Filter);
                }
                DatasettoExcel.Convert(GridData, Response);
                break;
            default:
                //if (Totalpages >= 1)
                {
                    GridData = objCert.GetAssignedCerts(intCompanyId, gvPR.PageSize, 1, Filter);
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

            GridData = objCert.GetAssignedCerts(intCompanyId, gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter());
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
        GridData = objCert.GetAssignedCerts(intCompanyId, gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter());

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

        //add alert before unassign
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnk = (LinkButton)e.Row.Cells[5].FindControl("lnkUnassignCert");
            lnk.Attributes.Add("onClick", "javascript:lnkUnassign_Clicked()");

        
        }
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

    protected void lnkUnassignCert_OnCommand(object sender, EventArgs args)
    { 
    string CertResellerID =((LinkButton)sender).CommandArgument;
    string[] arr = CertResellerID.Split('|');
        //arr[0] - cert ID
        //arr[1] - reseller Id
    if (!objCert.UnassignCertificate(Int32.Parse(arr[0]), Int32.Parse(arr[1]), ((UserInfo)Session["USER_INFO"]).GetUserAcctId()))
        setError(lblErr, FAILURE_UNASSIGN);
    else
        _PagerButtonClick("first");
        
    }

    private void setError(Label lbl,string txt)
    {
        lbl.Text = txt;
        lbl.Visible = false;
    }

    protected void ddlCertVersion_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string Filter = GetCertTypeToView();
        string Filter = GetFilter();
        if (!Filter.Trim().Equals(string.Empty))
        {
            Filter += " AND " + GetCertTypeToView();
        }
        else
        {
            Filter = GetCertTypeToView();
        } 
        GridData = objCert.GetAssignedCerts(intCompanyId, gvPR.PageSize, Int32.Parse(currentPage.Text), Filter);
        // Refreshes the view	
        UpdateDataView();
    }

    private string GetCertTypeToView()
    {
        string strViewCerts = string.Empty;
        switch (ddlCertVersion.SelectedValue)
        {
            case "PRE":
                strViewCerts = " version like '%3%' and sold_to_cust not like '%UPGRADE%'";
                break;
            case "POST":
                strViewCerts = " version like '%5%'";
                break;
        }
        return strViewCerts;
    }
}

