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
using System.Web.SessionState;

public partial class Controls_AllAmigopod : System.Web.UI.UserControl
{
    DataSet dsAmgSubscription = new DataSet();
    Certificate objCert;
    int intAcctId = -1;
    int rowCount = 0;
    protected static Hashtable columnNames = null;
    string meth = "AllAmigopod";
    DataSet dsLookupValues = new DataSet();
    Email objEmail = new Email();
    bool addSortDirectionIcon = false;

    protected ArrayList expressions;
    protected ArrayList orders;
    protected ArrayList columns;
        
    protected void Page_Load(object sender, EventArgs e)
    {
        string remoteUser = ((UserInfo)Session["USER_INFO"]).GetUserEmail();
        intAcctId = ((UserInfo)Session["USER_INFO"]).GetUserAcctId();
        objCert = new Certificate();

        if (!IsPostBack)
        {
            try
            {
                createColumnNamesList();
                grdSubscription.Attributes["RecordedExpression"] = "";
                grdSubscription.Attributes["FilterQuery"] = "";
                setDefaultSortingPattern();
                bindData(1, "");

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
            catch (Exception ex)
            {
                new Log().logException(meth+"_Page_Load", ex);
                objEmail.sendAmigopodErrorMessage(ex.Message, "AllAmigopod_Page_Load", "", "");
            }
        }
        ShowFilterParams();
    }

    public void bindData(int pageIndex, string filter)
    {

        try
        {
            dsAmgSubscription = objCert.GetAmigopodSubscriptions(grdSubscription.PageSize, pageIndex, filter, Session["BRAND"].ToString());
            if (dsAmgSubscription != null)
            {
                rowCount = Int32.Parse(dsAmgSubscription.Tables[1].Rows[0]["TotalRecords"].ToString());
                DataView dataView = new DataView(dsAmgSubscription.Tables[0]);
                string sortString = Sort.ReadSortingString(grdSubscription);
                dataView.Sort = sortString;
                grdSubscription.DataSource = dataView;
                //grdSubscription.DataSource = dsAmgSubscription.Tables[0];
                grdSubscription.DataBind();
              
                updatePageStatus();
            }
        }
        catch (Exception ex)
        {
            new Log().logException(meth+"_bindData", ex);
            objEmail.sendAmigopodErrorMessage(ex.Message, "AllAmigopod_bindData", "", "");
        }
    }

    public void bindUpgradeData(string subscription, GridView grdView)
    {

        try
        {
            DataSet dsAmgUpgrade = objCert.GetAmigopodUpgradeDetails(subscription, Session["BRAND"].ToString());
            if (dsAmgUpgrade != null)
            {
                grdView.DataSource = dsAmgUpgrade.Tables[0];
                grdView.DataBind();
                for (int i = 0; i < grdView.Rows.Count; i++)
                {
                    string cert_id = grdView.Rows[i].Cells[3].Text;
                    if (isHA(cert_id))
                        grdView.Rows[i].Cells[0].Text = "<font color=red>*</font>";
                }
            }
        }
        catch (Exception ex)
        {
            new Log().logException(meth+"_bindUpgradeData", ex);
            objEmail.sendAmigopodErrorMessage(ex.Message, "AllAmigopod_bindUpgradeData", "", "");
        }
    }

    protected void grdSubscription_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AlertToExpiry(sender, e);
                GridView grdDetails = (GridView)e.Row.FindControl("grdSubscriptionDetails");
                string subscription = e.Row.Cells[4].Text;
                bindUpgradeData(subscription, grdDetails);
                grdDetails.Visible = true;
            }
            else if(e.Row.RowType ==DataControlRowType.Header)
            {
                ItemCellsUpdate(sender, e);
            }
        }
        catch (Exception ex)
        {
            new Log().logException(meth+"_grdSubscription_RowDataBound", ex);
            objEmail.sendAmigopodErrorMessage(ex.Message, "AllAmigopod_grdSubscription_RowDataBound", "", "");
        }
    }

    protected void linkButton_Click(object sender, EventArgs e)
    {
        string arg = ((LinkButton)sender).CommandArgument;
        _PagerButtonClick(arg);
    }
    protected void lnkClearSort_Click(object sender, EventArgs e)
    {
        // sortDescription.Text = "";
        addSortDirectionIcon = false;
        grdSubscription.Attributes["RecordedExpression"] = "";
        setDefaultSortingPattern();
        bindData(Int32.Parse(currentPage.Text), GetFilter());
    }
    protected void PagerButtonClick(object sender, EventArgs e)
    {
        string arg = ((ImageButton)sender).CommandArgument;
        _PagerButtonClick(arg);
    }

    private void _PagerButtonClick(string arg)
    {
        int CurrentPage = Int32.Parse(currentPage.Text);
        int Totalpages = Int32.Parse(totalPages.Text);
        Certificate objCert = new Certificate();
        string Filter = GetFilter();
        int TotalRecords = Int32.Parse(totalRecords.Text);
        if (!Filter.Trim().Equals(string.Empty))
        {
            if (!GetCertTypeToView().Equals(string.Empty))
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
                    bindData(CurrentPage + 1, Filter);
                }
                break;
            case ("prev"):

                if (CurrentPage > 1)
                {

                    bindData(CurrentPage - 1, Filter);
                }
                break;
            case ("last"):

                if (CurrentPage != Totalpages && Totalpages > 1)
                {
                    bindData(Totalpages, Filter);
                }
                break;
            case ("all"):

                dsAmgSubscription = objCert.GetAmigopodSubscriptionByAcct(intAcctId, grdSubscription.PageSize, CurrentPage, Filter, Session["BRAND"].ToString());
                //DatasettoExcel.ConvertXML(GridData, Response);
                DatasettoExcel.ExportToTextFile(dsAmgSubscription, ConfigurationManager.AppSettings["DELIMITER"].ToString(), ((UserInfo)Session["USER_INFO"]).GetUserEmail(), Response);
                break;
            case ("allEx"):
                dsAmgSubscription = objCert.GetAmigopodSubscriptionByAcct(intAcctId, grdSubscription.PageSize, CurrentPage, Filter, Session["BRAND"].ToString());
                DatasettoExcel.Convert(dsAmgSubscription, Response);
                break;
            default:
                {
                    bindData(1, Filter);
                }
                break;
        }
    }

    private string GetFilter()
    {
        if (grdSubscription.Attributes["FilterQuery"] == null || grdSubscription.Attributes["FilterQuery"].Equals(string.Empty))
        {
            grdSubscription.Attributes["FilterQuery"] = string.Empty;
            return string.Empty;
        }
        string[] arrFilter = grdSubscription.Attributes["FilterQuery"].Split('^');
        return UIHelper.GetFilterSql(arrFilter[0], arrFilter[1], arrFilter[2]);
    }

    private void Setfilter()
    {
        if (!txtSearch.Text.Trim().Equals(string.Empty))
        {
            grdSubscription.Attributes["FilterQuery"] = ddlColumns.SelectedValue + "^" + ddlOperators.SelectedValue + "^" + txtSearch.Text.Trim();
            lblFilter.Text = ddlColumns.SelectedItem.Text + "  " + ddlOperators.SelectedItem.Text + "  " + txtSearch.Text.Trim();
        }

    }

    private string GetCertTypeToView()
    {
        string strViewCerts = string.Empty;
        //switch (ddlCertType.SelectedValue)
        //{
        //    case "ALL_CERTS":
        //        strViewCerts = " (Part_Id NOT LIKE '%-EVAL%' or Part_Id LIKE '%-EVAL%')";
        //        break;
        //    case "PERM_CERTS":
        //        strViewCerts = "Part_Id NOT LIKE '%-EVAL%'";
        //        break;
        //    case "EVAL_CERTS":
        //        strViewCerts = "Part_Id LIKE '%-EVAL%'";
        //        break;

        //}
        return strViewCerts;

    }

    private void updatePageStatus()
    {
        if (rowCount > 0)
        {
            currentPage.Text = dsAmgSubscription.Tables[1].Rows[0]["PageNumber"].ToString();
            totalPages.Text = dsAmgSubscription.Tables[1].Rows[0]["TotalPages"].ToString();
            totalRecords.Text = rowCount + "";
        }
        else
        {
            currentPage.Text = "1";
            totalPages.Text = "1";
            totalRecords.Text = "0";
        }
    }

    protected void GetDetails_OnCommand(object sender, CommandEventArgs e)
    {
        Session["subscription"] = ((LinkButton)sender).CommandArgument;
        Response.Redirect("~/Pages/SubscriptionDetails.aspx?key=all");
    }
    protected void lnkRemfilter_OnClick(object sender, EventArgs e)
    {
        grdSubscription.Attributes["FilterQuery"] = string.Empty;
        _PagerButtonClick("first");
        ShowFilterParams();
        lblFilter.Text = string.Empty;
        txtSearch.Text = string.Empty;
        ddlColumns.ClearSelection();
        ddlOperators.ClearSelection();
    }
    protected void btnGo_OnClick(object sender, ImageClickEventArgs e)
    {
        Setfilter();
        _PagerButtonClick("first");
        ShowFilterParams();

    }

    private void createColumnNamesList()
    {
        columnNames = new Hashtable();
        columnNames.Add("", "Upgrade,0");
        columnNames.Add("part_id", "License P/N,1");
        columnNames.Add("part_desc", "Description,2");
        columnNames.Add("CertId", "Certificate Id,3");
        columnNames.Add("subscription", "Activation Key,4");
        columnNames.Add("ActivatedBy", "Activated By,5");
        columnNames.Add("ActivatedOn", "Activated On,6");
        columnNames.Add("ExpiryDate", "Expiry Date,6");
        columnNames.Add("Company Name", "companyName,7");
    }

    private void BindFilterColumns()
    {
        string[] arrFilCols = { "Part Name|part_id",
                                "Description|part_desc",
                                "Certificate Id|CertId",
                                "subscription|subscription_key",
                                "Activated By|email",
                                "Company Name|companyName"
                                };
        DataTable dt = UIHelper.GetTableFromStringArray(arrFilCols, '|');
        UIHelper.PrepareAndBindListWithoutPlease(ddlColumns, dt, "TXT", "VAL", false);

    }

    private void ShowFilterParams()
    {
        bool blnShow = false; ;
        if (grdSubscription.Attributes["FilterQuery"] == null || grdSubscription.Attributes["FilterQuery"].Equals(string.Empty))
        {
            grdSubscription.Attributes["FilterQuery"] = string.Empty;
            blnShow = true;
        }
        pnlFilterParams.Visible = blnShow;
        pnlFilterQuery.Visible = !pnlFilterParams.Visible;
    }
    protected void lnkDload_OnClick(object sender, EventArgs e)
    {
        string arg = ((LinkButton)sender).CommandArgument;
        _PagerButtonClick(arg);
    }
    protected void lnkDloadEx_OnClick(object sender, EventArgs e)
    {
        string arg = ((LinkButton)sender).CommandArgument;
        _PagerButtonClick(arg);
    }

    private bool isHA(string cert_id)
    {
        bool isHa = false;
        DataSet dsAmigopodLookup = objCert.GetAmigoppodLookInfo(cert_id, Session["BRAND"].ToString());
        string partType = string.Empty;

        if (dsAmigopodLookup.Tables[0].Rows.Count > 0)
        {
            partType = dsAmigopodLookup.Tables[0].Rows[0]["part_type"].ToString();
        }

        if (partType == ConfigurationManager.AppSettings["CLEARPASS_HA"].ToString())
            isHa = true;
        return isHa;
    }
    protected void grdSubscription_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (grdSubscription.Rows.Count > 0)
        {
            ArrayList list = null; expressions = null; orders = null; columns = null;
            addSortDirectionIcon = true;
            if (grdSubscription.Attributes["defaultSort"] == Sort.TRUE_CASE)
            {
                grdSubscription.Attributes["RecordedExpression"] = "";
                grdSubscription.Attributes["defaultSort"] = Sort.FALSE_CASE;
            }
            
            Sort.PrepareSortingString(e.SortExpression, grdSubscription);
            list = Sort.GetExpressionNOrders(Sort.ReadSortingString(grdSubscription));
            expressions = (ArrayList)list[Sort.EXPRESSIONS];
            orders = (ArrayList)list[Sort.ORDERS];
            columns = Sort.GetColumnNum(columnNames.GetEnumerator(), expressions);

            bindData(Int32.Parse(currentPage.Text), GetFilter());
            
        }
    }

    private void setDefaultSortingPattern()
    {
        grdSubscription.Attributes["defaultSort"] = Sort.TRUE_CASE;
        string defaultPattern = "part_desc ASC";
        Sort.PrepareSortingString(defaultPattern, grdSubscription);
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
                        icon.Text = (((String)orders[ii]).Equals(Sort.ORDER_ASC)) ? "  Λ" : "  V";
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

    protected void AlertToExpiry(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[7].Text != "" || e.Row.Cells[7].Text != null)
            {
                DateTime expiryDate = Convert.ToDateTime(e.Row.Cells[7].Text);
                if (expiryDate <= DateTime.Now)
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                    LinkButton lnkButton = (LinkButton)e.Row.FindControl("lnkBtnDetails");
                    lnkButton.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        
    }

}
