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

public partial class Controls_AssignAWCertificates : System.Web.UI.UserControl
{
    private DataSet GridData = new DataSet();
    int rowCount = 0;
    protected static Hashtable columnNames = null;
    private static DataTable dtFilCols = null;
    bool addSortDirectionIcon = false;
    DataSet dsLookupValues = new DataSet();
    private string strUser = string.Empty;
    Certificate objCert;
    DataTable dtCertReseller;

    protected ArrayList expressions;
    protected ArrayList orders;
    protected ArrayList columns;
    int intCompanyId = -1;

    private const string NO_CERTS = "No licenses were selected";
    private const string NO_RESELLER = "Choose a reseller for selected Licenses";
    private const string FAILURE_ASSIGN = "Unable to assign Licenses";

    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false;
        string remoteUser = ((UserInfo)Session["USER_INFO"]).GetUserEmail();
        intCompanyId = ((UserInfo)Session["USER_INFO"]).GetUserCompanyId();
        objCert = new Certificate();
        if (!IsPostBack)
        {
            createColumnNamesList();
            gvPR.Attributes["RecordedExpression"] = "";
            gvPR.Attributes["FilterQuery"] = "";

            GridData = objCert.GetUnassignedAWCerts(intCompanyId, gvPR.PageSize, 1, string.Empty);
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
        columnNames.Add("PartDesc", "Description,1");
        columnNames.Add("SerialNumber", "Certificate Id,2");
    }

    private void BindFilterColumns()
    {

        string[] arrFilCols = { "Description|Part_Desc",
                                "Certificate Id|Serial_Number"
                                };
        DataTable dt = UIHelper.GetTableFromStringArray(arrFilCols, '|');
        UIHelper.PrepareAndBindListWithoutPlease(ddlColumns, dt, "TXT", "VAL", false);

    }
    private void setDefaultSortingPattern()
    {
        gvPR.Attributes["defaultSort"] = Sort.TRUE_CASE;
        string defaultPattern = "Part_Desc ASC";
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
        switch (arg)
        {
            case ("next"):

                if (CurrentPage < Totalpages)
                {
                    GridData = objCert.GetUnassignedAWCerts(intCompanyId, gvPR.PageSize, CurrentPage + 1, Filter);
                    UpdateDataView();
                }
                break;
            case ("prev"):

                if (CurrentPage > 1)
                {

                    GridData = objCert.GetUnassignedAWCerts(intCompanyId, gvPR.PageSize, CurrentPage - 1, Filter);
                    UpdateDataView();
                }
                break;
            case ("last"):

                if (CurrentPage != Totalpages && Totalpages > 1)
                {
                    GridData = objCert.GetUnassignedAWCerts(intCompanyId, gvPR.PageSize, Totalpages, Filter);
                    UpdateDataView();
                }
                break;
            case ("all"):
                GridData = objCert.GetUnassignedAWCerts(intCompanyId, gvPR.PageSize, 1, Filter);
                //DatasettoExcel.ConvertXML(GridData, Response);
                DatasettoExcel.ExportToTextFile(GridData, ConfigurationManager.AppSettings["DELIMITER"].ToString(), ((UserInfo)Session["USER_INFO"]).GetUserEmail(), Response);
                break;
            case ("allEx"):
                GridData = objCert.GetUnassignedAWCerts(intCompanyId, gvPR.PageSize, 1, Filter);
                DatasettoExcel.Convert(GridData, Response);
                break;
            default:
                //if (Totalpages >= 1)
                {
                    GridData = objCert.GetUnassignedAWCerts(intCompanyId, gvPR.PageSize, 1, Filter);
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

            GridData = objCert.GetUnassignedAWCerts(intCompanyId, gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter());
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
        GridData = objCert.GetUnassignedAWCerts(intCompanyId, gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter());

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
        //add dropdownlist
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable dt = (DataTable)Cache["RESELLER_LIST" + ((UserInfo)Session["USER_INFO"]).GetUserAcctId().ToString()];
                if (dt == null)
                        {
                            DataSet ds = new Company().GetCompanyList(99999, 1, string.Empty, ((UserInfo)Session["USER_INFO"]).Brand,CompanyType.Reseller,((UserInfo)Session["USER_INFO"]).GetUserCompanyId());
                            dt = ds.Tables[0];
                            // Cache.Insert("ACCT_LIST", ds, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
                            Cache.Insert("RESELLER_LIST"+((UserInfo)Session["USER_INFO"]).GetUserAcctId().ToString(), dt, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10), CacheItemPriority.NotRemovable, null);
                        }
                        //test deptcharge
                        DropDownList ddlTemp = (DropDownList)e.Row.Cells[3].FindControl("ddlReseller");
                        UIHelper.PrepareAndBindListWithoutPlease(ddlTemp, dt, "CompanyName", "CompanyId", true);
                        ddlTemp.Enabled = false;

            //set js functions
                        CheckBox chk = (CheckBox)e.Row.Cells[0].FindControl("chkCert");
                        chk.Attributes.Add("onClick", "javascript:chkCert_Clicked('"+chk.ClientID+"','"+ddlTemp.ClientID+"')");

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
    protected void cvNoSelection_OnValidate(object sender, ServerValidateEventArgs args)
    {
        string strCommaSepCertIds = string.Empty;
        if (gvPR.Rows.Count == 0)
        {
            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = NO_CERTS;
            return;
        }
        dtCertReseller = GetParamsTable();
        CheckBox chk;
        for (int i = 0; i < gvPR.Rows.Count; i++)
        {
            chk = (CheckBox)(gvPR.Rows[i].Cells[0].FindControl("chkCert"));
            if (chk.Checked == true)
            {
                strCommaSepCertIds = ((HtmlInputHidden)(gvPR.Rows[i].Cells[0].FindControl("hdnCertId"))).Value ;
                DropDownList ddlTemp = (DropDownList)gvPR.Rows[i].Cells[3].FindControl("ddlReseller");
                if (ddlTemp.SelectedValue == string.Empty)
                {
                    args.IsValid = false;
                    ((CustomValidator)sender).ErrorMessage = NO_RESELLER;
                    dtCertReseller.Clear();
                    ddlTemp.Enabled = true;
                    return;
                }

                dtCertReseller.Rows.Add(AddRow(dtCertReseller, strCommaSepCertIds, ddlTemp.SelectedValue.ToString()));
            }

        }
        if (dtCertReseller.Rows.Count==0)
        {
            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = NO_CERTS;
        }

    }

    protected void btnAssign_OnClick(object sender, EventArgs args)
    {
        if (!Page.IsValid)
            return;

        if (dtCertReseller ==null ||dtCertReseller.Rows.Count == 0)
        {

            setError(lblErr, NO_CERTS);
            return;
        }
        if (objCert.AssignAWCertsToResellers(((UserInfo)Session["USER_INFO"]).GetUserAcctId(), dtCertReseller))
        {
            //mail feature here
            //UserInfo objUser = (UserInfo)Session["USER_INFO"];
            //CertificateMailInfo objCertMailInfo = new CertificateMailInfo();
            //objCertMailInfo.Email = ((UserInfo)Session["USER_INFO"]).GetUserEmail;
            gvPR.Attributes["FilterQuery"] = string.Empty;
            _PagerButtonClick("first");

        }
        else
        {
            setError(lblErr, FAILURE_ASSIGN);
            return;
        }       
    }

    private void setError(Label lbl, string txt)
    {
        lbl.Text = txt;
        lbl.Visible = true;
    }

    private DataTable GetParamsTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(Certificate.CI_CERT_ID);
        dt.Columns.Add(Certificate.CI_RESELLER_ID);
        return dt;
    }

    private DataRow AddRow(DataTable dt,string CertId, string ResellerId)
    {
        DataRow dr = dt.NewRow();
        dr[Certificate.CI_RESELLER_ID] = ResellerId;
        dr[Certificate.CI_CERT_ID] = CertId;
        return dr;    
    }
}



