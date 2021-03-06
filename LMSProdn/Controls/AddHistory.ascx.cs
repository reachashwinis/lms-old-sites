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
using Com.Arubanetworks.Licensing.Dataaccesslayer;
using System.Drawing;
using System.Web.Caching;
using System.Collections.Generic;

public partial class Controls_AddHistory : System.Web.UI.UserControl
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
    int intAcctId = -1;
    string acctType = string.Empty;
    DataSet dsInfo;

    protected void Page_Load(object sender, EventArgs e)
    {
        string remoteUser = ((UserInfo)Session["USER_INFO"]).GetUserEmail();
        intAcctId = ((UserInfo)Session["USER_INFO"]).GetUserAcctId();
        lblSuccess.Visible = false;
        lblSystemErr.Visible = false;
        lblCertErr.Visible = false;
        btnsubmit.Enabled = true;

        objCert = new Certificate();
        if (!IsPostBack)
        {
            wizController.ActiveStepIndex = 0;
            lblCertErr.Visible = false;
            createColumnNamesList();
            gvPR.Attributes["RecordedExpression"] = "";
            gvPR.Attributes["FilterQuery"] = "";

            GridData = objCert.GetAllSerialNo(intAcctId, gvPR.PageSize, 1, string.Empty, Session["BRAND"].ToString());
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
        columnNames.Add("Fru", "System S/N,0");
        columnNames.Add("SystemPartId", "System P/N,1");
        columnNames.Add("SystemDesc", "System Desc,2");
        columnNames.Add("Location", "System Location,3");
    }

    private void BindFilterColumns()
    {

        string[] arrFilCols = { "System S/N|Fru", 
                                "System P/N|System_Part_Id",
                                "System Desc|System_Desc",
                                "System Location|Location"
                                };
        DataTable dt = UIHelper.GetTableFromStringArray(arrFilCols, '|');
        UIHelper.PrepareAndBindListWithoutPlease(ddlColumns, dt, "TXT", "VAL", false);

    }

    private void setDefaultSortingPattern()
    {
        gvPR.Attributes["defaultSort"] = Sort.TRUE_CASE;
        string defaultPattern = "Fru ASC";
        Sort.PrepareSortingString(defaultPattern, gvPR);
    }

    private void UpdateDataView()
    {

        DataSet myDataSet = GridData;//vivek:sp
        if (myDataSet != null)
        {
            rowCount = Int32.Parse(myDataSet.Tables[1].Rows[0]["TotalRecords"].ToString());
            DataView dataView = new DataView(myDataSet.Tables[0]);
            //string sortString = Sort.ReadSortingString(gvPR);
            //dataView.Sort = sortString;
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

    public void lnkDload_OnClick(object sender, EventArgs e)
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
                    GridData = objCert.GetAllSerialNo(intAcctId, gvPR.PageSize, CurrentPage + 1, Filter,Session["BRAND"].ToString());
                    UpdateDataView();
                }
                break;
            case ("prev"):

                if (CurrentPage > 1)
                {

                    GridData = objCert.GetAllSerialNo(intAcctId, gvPR.PageSize, CurrentPage - 1, Filter, Session["BRAND"].ToString());
                    UpdateDataView();
                }
                break;
            case ("last"):

                if (CurrentPage != Totalpages && Totalpages > 1)
                {
                    GridData = objCert.GetAllSerialNo(intAcctId, gvPR.PageSize, Totalpages, Filter, Session["BRAND"].ToString());
                    UpdateDataView();
                }
                break;
            case ("all"):
                int TotalRecords = Int32.Parse(totalRecords.Text);
                GridData = objCert.GetAllSerialNo(intAcctId, TotalRecords, 1, Filter, Session["BRAND"].ToString());
                //DatasettoExcel.ConvertXML(GridData, Response);
                DatasettoExcel.ExportToTextFile(GridData, ConfigurationManager.AppSettings["DELIMITER"].ToString(), ((UserInfo)Session["USER_INFO"]).GetUserEmail(), Response);
                break;
            default:
                //if (Totalpages >= 1)
                {
                    GridData = objCert.GetAllSerialNo(intAcctId, gvPR.PageSize, 1, Filter, Session["BRAND"].ToString());
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

            GridData = objCert.GetAllSerialNo(intAcctId, gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter(), Session["BRAND"].ToString());
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
        GridData = objCert.GetAllSerialNo(intAcctId, gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter(), Session["BRAND"].ToString());

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

    protected void Submit_Onclick(object sender, EventArgs args)
    {
        DataTable dt = (DataTable)Session["FRU_INFO"];
        for (int i = 0; i < GridView2.Rows.Count; i++)
        {
            TextBox txtBx = (TextBox)(GridView2.Rows[i].Cells[3].FindControl("Txtloc"));
            dt.Rows[i]["Location"] = txtBx.Text; 
        }
        bool retVal = objCert.UpdateLocation(dt, ((UserInfo)Session["USER_INFO"]).GetUserAcctId());
        if (retVal == false)
        {
            setError(lblSystemErr, "Unable to update Location");
        }
        else
        {
            setError(lblSuccess, "Location is Updated Successfully");
        }
    }


    protected void Update_Onclick(object sender, EventArgs args)
    {
        DataTable dt = objCert.BuildCertInfo();
        CheckBox chk;
        for (int i = 0; i < gvPR.Rows.Count; i++)
        {
            chk = (CheckBox)(gvPR.Rows[i].Cells[0].FindControl("chkFru"));
            if (chk.Checked == true)
            {
                DataRow dr = dt.NewRow();
                HtmlInputHidden hdn = (HtmlInputHidden)(gvPR.Rows[i].Cells[0].FindControl("hdnFruId"));
                string strFru = hdn.Value;
                dr[Certificate.SYS_SERIAL_NUMBER] = strFru;

                //get Fru info

                dsInfo = objCert.GetFruInfo(strFru);
                if (dsInfo == null)
                {
                    setError(lblCertErr, Certificate.NO_SYS_INFO);
                    return;
                }
                dr[Certificate.SYS_PART_ID] = dsInfo.Tables[0].Rows[0]["part_id"].ToString();
                dr[Certificate.SYS_PART_DESC] = dsInfo.Tables[0].Rows[0]["Part_desc"].ToString();
                dr[Certificate.SYS_SERIAL_NUMBER] = dsInfo.Tables[0].Rows[0]["Serial_Number"].ToString();
                dr[Certificate.LOC] = dsInfo.Tables[0].Rows[0]["Location"].ToString();
                dt.Rows.Add(dr);
            }
        }
        LoadWizStep2(dt);
    }

    private void setError(Label lbl, string text)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }


    private void LoadWizStep2(DataTable dt)
    {
        try
        {
            wizController.ActiveStepIndex = 1;
            GridView2.DataSource = dt;
            GridView2.DataBind();
            Session["FRU_INFO"] = dt;
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
}

