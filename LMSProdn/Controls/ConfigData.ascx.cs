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

public partial class Controls_ConfigData : System.Web.UI.UserControl
{
    private DataSet GridData = new DataSet();
    int rowCount = 0;
    protected static Hashtable columnNames = null;
    bool addSortDirectionIcon = false;
    DataSet dsLookupValues = new DataSet();
    private string strUser = string.Empty;
    private string strBrand = string.Empty;
    Lookup objLookup;
    protected ArrayList expressions;
    protected ArrayList orders;
    protected ArrayList columns;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false;
        strUser = ((UserInfo)Session["USER_INFO"]).GetUserEmail();
        objLookup = new Lookup();
        if (!IsPostBack)
        {


            createColumnNamesList();
            gvPR.Attributes["RecordedExpression"] = "";
            gvPR.Attributes["FilterQuery"] = "";

            GridData = objLookup.GetConfigData(gvPR.PageSize, 1, string.Empty);
            setDefaultSortingPattern(); // Set a default sorting pattern
            UpdateDataView();


            //load Opertors list
            
            dsLookupValues = (DataSet)Cache[Lookup.LOOKUP_TBL];
            if (dsLookupValues == null)
            {
                dsLookupValues = objLookup.LoadLookupValues(strUser,Session["BRAND"].ToString());
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
        columnNames.Add("add_ts", "Added,0");
        columnNames.Add("exp_serial", "Serial Number,1");
        columnNames.Add("value", "MAC Address,2");
    }

    private void BindFilterColumns()
    {

        string[] arrFilCols = { "Serial Number|exp_serial",
                                        "MAC Address|value"            
                                };
        DataTable dt = UIHelper.GetTableFromStringArray(arrFilCols, '|');
        UIHelper.PrepareAndBindListWithoutPlease(ddlColumns, dt, "TXT", "VAL", false);

    }

    private void setDefaultSortingPattern()
    {
        gvPR.Attributes["defaultSort"] = Sort.TRUE_CASE;
        string defaultPattern = "add_ts DESC";
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
                    GridData = objLookup.GetConfigData(gvPR.PageSize, CurrentPage + 1, Filter);
                    UpdateDataView();
                }
                break;
            case ("prev"):

                if (CurrentPage > 1)
                {
                    GridData = objLookup.GetConfigData(gvPR.PageSize, CurrentPage - 1, Filter);
                    UpdateDataView();
                }
                break;
            case ("last"):

                if (CurrentPage != Totalpages && Totalpages > 1)
                {
                    GridData = objLookup.GetConfigData(gvPR.PageSize, Totalpages, Filter);
                    UpdateDataView();
                }
                break;
            default:
                //if (Totalpages >= 1)
                {
                    GridData = objLookup.GetConfigData(gvPR.PageSize, 1, Filter);
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

            GridData = objLookup.GetConfigData(gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter());

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

        GridData = objLookup.GetConfigData(gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter());


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

    protected void btnAdd_OnClick(object sender, EventArgs args)
    { 
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
        string strSerialNum = string.Empty;
        string strMacAddr = string.Empty;

        if (!Page.IsValid)
            return;

        strSerialNum = txtSNum.Text.Trim().ToUpper();
        strMacAddr = txtMacAddr.Text.Trim().ToUpper();

        if (strSerialNum.Length > 0 && strMacAddr.Length > 0)
        {
            //Check to make sure that the mac address follows the correct form
            if (!reg.IsMatch(strMacAddr))
            {
                setError("MAC Address must follow the format xx:xx:xx:xx:xx:xx where x is a hexadecimal character");
            }
            else
            {
                if (objLookup.IsExistsConfigData(strSerialNum, strMacAddr))
                {
                    setError("The serial number/MAC address already exists in the LMS database");
                }
                else 
                {
                    if (objLookup.AddConfigData(strSerialNum, strMacAddr, Request.ServerVariables["REMOTE_ADDR"], strUser))
                    {
                        txtSNum.Text = string.Empty;
                        txtMacAddr.Text = string.Empty;
                        lnkRemfilter_OnClick(this, args);
                        setError("Serial Number is added successfully");
                        lblErr.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        setError("Unable to add Config Data");
                        lblErr.ForeColor = System.Drawing.Color.Red;
                    }
                }

            }
        }
        else
        {
            setError("All fields are mandatory");
        }


    }


    private void setError(string text)
    {
        lblErr.Text = text;
        lblErr.Visible = true;
    }


}
