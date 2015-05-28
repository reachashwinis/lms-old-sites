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
using System.Text;
using System.IO;

public partial class Controls_AllClearPassCerts : System.Web.UI.UserControl
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
    string refVal = "";
   
    protected void Page_Load(object sender, EventArgs e)
    {
        string remoteUser = ((UserInfo)Session["USER_INFO"]).GetUserEmail();
        intAcctId = ((UserInfo)Session["USER_INFO"]).GetUserAcctId();
        objCert = new Certificate();
        
        if (!IsPostBack)
        {
            createColumnNamesList();
            gvPR.Attributes["RecordedExpression"] = "";
            gvPR.Attributes["FilterQuery"] = "";
            string strbrand = Session["BRAND"].ToString();
            
            GridData = objCert.GetAllClpCerts(gvPR.PageSize, 1,GetCertTypeToView(),strbrand);
            setDefaultSortingPattern(); // Set a default sorting pattern
            UpdateDataView();


            //load Dept list
            Lookup objLookup = new Lookup();
            dsLookupValues = (DataSet)Cache[Lookup.LOOKUP_TBL];
            if (dsLookupValues == null)
            {
                dsLookupValues = objLookup.LoadLookupValues(remoteUser,Session["BRAND"].ToString());
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
        columnNames.Add("SubKey", "Subscription ID,0");
        columnNames.Add("PartId", "License P/N,1");
        columnNames.Add("PartDesc", "Description,2");
        columnNames.Add("SerialNum", "Certificate Id,3");
        columnNames.Add("LSerialNum", "Certificate Serial No,4");
        columnNames.Add("SONo", "Sales Order,5");
        columnNames.Add("CustPO", "Purchase Order,6");
        columnNames.Add("EndUserPO", "Reseller/End User PO,7");
        columnNames.Add("Version", "Version,8");
        columnNames.Add("Location", "Location,9");
        columnNames.Add("ActCode", "Activation Key,10");
        columnNames.Add("ActivatedOn", "Activated On,11");
        columnNames.Add("ActivatedBy", "Activated By,12"); 
    }

    private void BindFilterColumns()
    {

        string[] arrFilCols = { "Subscription ID|subscription_key",
                                "License P/N|Part_Id",
                                "Description|Part_Desc",
                                "Certificate Id|Serial_Number",
                                "Certificate Serial No|Lserial_number",
                                "Sales Order|so_id",
                                "Purchase Order|cust_po_id",
                                "End User PO|end_user_po",                                
                                "Version|Version",
                                "Activation Key|Activation_Code",
                                "Location|Location",
                                "Activated By|email",
                                };
        DataTable dt = UIHelper.GetTableFromStringArray(arrFilCols, '|');
        UIHelper.PrepareAndBindListWithoutPlease(ddlColumns, dt, "TXT", "VAL", false);

    }

    private void setDefaultSortingPattern()
    {
        gvPR.Attributes["defaultSort"] = Sort.TRUE_CASE;
        string defaultPattern = "ActivatedOn DESC";
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
                    GridData = objCert.GetAllClpCerts(gvPR.PageSize, CurrentPage + 1, Filter,Session["BRAND"].ToString());
                    UpdateDataView();
                }
                break;
            case ("prev"):

                if (CurrentPage > 1)
                {

                    GridData = objCert.GetAllClpCerts(gvPR.PageSize, CurrentPage - 1, Filter,Session["BRAND"].ToString());
                    UpdateDataView();
                }
                break;
            case ("last"):

                if (CurrentPage != Totalpages && Totalpages > 1)
                {
                    GridData = objCert.GetAllClpCerts(gvPR.PageSize, Totalpages, Filter,Session["BRAND"].ToString());
                    UpdateDataView();
                }
                break;
            case ("all"):
                if (gvPR.Attributes["FilterQuery"].Equals(string.Empty))
                {
                    GridData = objCert.GetAllClpCerts(TotalRecords, 1, Filter, Session["BRAND"].ToString());
                }
                else
                {
                    GridData = objCert.GetAllClpCerts(gvPR.PageSize, 1, Filter, Session["BRAND"].ToString());
                }
                //DatasettoExcel.ConvertXML(GridData, Response);                
                //DatasettoExcel.Convert(GridData, Response);    
                DatasettoExcel.ExportToTextFile(GridData, ConfigurationManager.AppSettings["DELIMITER"].ToString(),((UserInfo)Session["USER_INFO"]).GetUserEmail(),Response);
                break;
            case ("allEx"):
                if (gvPR.Attributes["FilterQuery"].Equals(string.Empty))
                {
                    GridData = objCert.GetAllClpCerts(TotalRecords, 1, Filter, Session["BRAND"].ToString());
                }
                else
                {
                    GridData = objCert.GetAllClpCerts(gvPR.PageSize, 1, Filter, Session["BRAND"].ToString());
                }
                DatasettoExcel.Convert(GridData, Response);    
                break;
            default:
                //if (Totalpages >= 1)
                {
                    GridData = objCert.GetAllClpCerts(gvPR.PageSize, 1, Filter,Session["BRAND"].ToString());
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
            string Filter = GetFilter();
            if (!Filter.Trim().Equals(string.Empty))
            {
                Filter += " AND " + GetCertTypeToView();
            }
            else
            {
                Filter = GetCertTypeToView();
            }
            GridData = objCert.GetAllClpCerts(gvPR.PageSize, Int32.Parse(currentPage.Text), Filter,Session["BRAND"].ToString());
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
        GridData = objCert.GetAllClpCerts(gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter(),Session["BRAND"].ToString());

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
                        icon.Text = (((String)orders[ii]).Equals(Sort.ORDER_ASC)) ? "?" : "V";
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (refVal == e.Row.Cells[0].Text.ToString().Trim())
            {
                //e.Row.Cells[0].Text = "";
            }
            else
            {
                refVal = e.Row.Cells[0].Text.ToString().Trim();

            }
        }
    }

    private void ShowFilterParams()
    {
        bool blnShow = false; ;
        if (gvPR.Attributes["FilterQuery"]==null ||  gvPR.Attributes["FilterQuery"].Equals(string.Empty))
        {
            gvPR.Attributes["FilterQuery"] = string.Empty;
            blnShow = true;
        }
        pnlFilterParams.Visible = blnShow;
        pnlFilterQuery.Visible = !pnlFilterParams.Visible;
    }
    private string GetFilter()
    {
        if (gvPR.Attributes["FilterQuery"] == null || gvPR.Attributes["FilterQuery"].Equals(string.Empty))
        {
            gvPR.Attributes["FilterQuery"] = string.Empty;
            return string.Empty;
        }
        string[] arrFilter = gvPR.Attributes["FilterQuery"].Split('^');
        return UIHelper.GetFilterSql(arrFilter[0], arrFilter[1], arrFilter[2]);

    }

    private void Setfilter()
    {
        if (!txtSearch.Text.Trim().Equals(string.Empty))
        {
            gvPR.Attributes["FilterQuery"] = ddlColumns.SelectedValue + "^" + ddlOperators.SelectedValue + "^" + txtSearch.Text.Trim();
            lblFilter.Text = ddlColumns.SelectedItem.Text + "  " + ddlOperators.SelectedItem.Text + "  " + txtSearch.Text.Trim();
        }

    }

    private string GetCertTypeToView()
    {
        string strViewCerts = string.Empty;
        switch (ddlCertType.SelectedValue) 
        {
            case "ALL_CERTS":
                switch (ddlCertVersion.SelectedValue)
                {
                    case "5.0.X.X":
                        strViewCerts = " (Part_Id NOT LIKE 'SUB%' or Part_Id LIKE 'SUB%') and version like '5%'";
                        break;
                    case "6.X.X.X":
                        strViewCerts = " (Part_Id NOT LIKE 'SUB%' or Part_Id LIKE 'SUB%') and version like '6%'";
                        break;
                }
              break;
            case "PERM_CERTS":
                switch (ddlCertVersion.SelectedValue)
                {
                    case "5.0.X.X":
                        strViewCerts = "Part_Id NOT LIKE 'SUB%' and Part_Id NOT LIKE '%EVL%' and version like '5%'";
                        break;
                    case "6.X.X.X":
                        strViewCerts = "Part_Id NOT LIKE 'SUB%' and Part_Id NOT LIKE '%EVL%' and version like '6%'";
                        break;
                }
             break;
         case "SUB_CERTS":
                switch (ddlCertVersion.SelectedValue)
                {
                    case "5.0.X.X":
                        strViewCerts = "Part_Id LIKE 'SUB%' and version like '5%'";
                        break;
                    case "6.X.X.X":
                        strViewCerts = "Part_Id LIKE 'SUB%' and version like '6%'";
                        break;
                }
                break;
          case "EVAL_CERTS":
                switch (ddlCertVersion.SelectedValue)
                {
                    case "5.0.X.X":
                        strViewCerts = "Part_Id LIKE '%EVL%' and version like '5%'";
                        break;
                    case "6.X.X.X":
                        strViewCerts = "Part_Id LIKE '%EVL%' and version like '6%'";
                        break;
                }
                break;
        }
        return strViewCerts;
    }

    protected void ddlCertType_SelectedIndexChanged(object sender, EventArgs e)
    {

        string Filter = GetFilter();
        if (!Filter.Trim().Equals(string.Empty))
        {
            Filter += " AND " + GetCertTypeToView();
        }
        else
        {
            Filter = GetCertTypeToView();
        } 
        intAcctId = ((UserInfo)Session["USER_INFO"]).GetUserAcctId();
        GridData = objCert.GetAllClpCerts(gvPR.PageSize, Int32.Parse(currentPage.Text), Filter, Session["BRAND"].ToString());
        // Refreshes the view	
        UpdateDataView();
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
        intAcctId = ((UserInfo)Session["USER_INFO"]).GetUserAcctId();
        GridData = objCert.GetAllClpCerts(gvPR.PageSize, Int32.Parse(currentPage.Text), Filter, Session["BRAND"].ToString());
        // Refreshes the view	
        UpdateDataView();
    }

    protected void GetDetails_OnCommand(object sender, CommandEventArgs e)
    {
        Session["subscription"] = ((LinkButton)sender).CommandArgument;
        Response.Redirect("~/Pages/ClpSubscriptionDet.aspx?subkey=");
    }
}


