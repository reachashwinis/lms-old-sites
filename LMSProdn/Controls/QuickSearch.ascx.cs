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
using System.Text.RegularExpressions;

public partial class Controls_QuickSearch : System.Web.UI.UserControl
{

    private DataSet GridData = new DataSet();
    int rowCount = 0;
    protected static Hashtable columnNames = null;
    bool addSortDirectionIcon = false;
    bool showSOID;
    Certificate objCert;

    protected ArrayList expressions;
    protected ArrayList orders;
    protected ArrayList columns;
    string strCommaSepCertIds = string.Empty;
    Email objEmail;

    protected void Page_Load(object sender, EventArgs e)
    {
        objCert = new Certificate();
        objEmail = new Email();

        if (!IsPostBack)
        {
            divRes.Visible = false;
            //set Grid params

            createColumnNamesList();
            gvPR.Attributes["RecordedExpression"] = "";
            gvPR.Attributes["FilterQuery"] = "";
            lblErr.Visible = false;
            LblSearchErr.Visible = false;
            Session["Filter"] = null;
        }
        else
        { 
        //check for this value
            if (Session["FILTER"] == null || Session["FILTER"].ToString().Equals(string.Empty))
            {
            //search hasnt started so dont show results
                divRes.Visible = false;
                return;
            }
            else 
            {
                divRes.Visible = true;            
            }
        }
        if (chkHistory.Checked == true)
        {
            btnSendMail.Enabled = false;
        }
        else
        {
            btnSendMail.Enabled = true;
        }
        lblErr.Visible = false;
        LblSearchErr.Visible = false;
        LblResult.Text = string.Empty;        
    }

    private void createColumnNamesList()
    {
        columnNames = new Hashtable();
        columnNames.Add("part_id", "License P/N,0");
        columnNames.Add("part_desc", "Description,1");
        columnNames.Add("serial_number", "Serial Number,2");
        columnNames.Add("so_id", "Sales Order#,3");
        columnNames.Add("ship_to_cust", "Ship To Cust,4");
        columnNames.Add("sold_to_cust", "Sold To Cust,5");
        columnNames.Add("bill_to_cust", "Bill To Cust,6");
        columnNames.Add("ship_to_name", "Ship To Name,7");
        columnNames.Add("sold_to_name", "Sold To Name,8");
        columnNames.Add("bill_to_name", "Bill To Name,9");       
        columnNames.Add("location", "System Location #,10");
        columnNames.Add("order_date", "Order Date #,11");
        columnNames.Add("version", "Version #,12");
    }

  
    protected void btnSearch_OnClick(object sender, EventArgs args)
    {

        if (!Page.IsValid)
            return;

        //if (txtSearch.Text.Trim().Equals(string.Empty))
        //{
        //    Session["FILTER"] = null;
        //    divRes.Visible = false;
        //    return;
        //}

        if (!TxtOrderDate.Text.Equals(string.Empty))
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["DATE_FORMAT"]);
            if (!objReg.IsMatch(TxtOrderDate.Text.Trim()))
            {
                setError(LblSearchErr, "Invalid Date format");
                return;
            }            
        }
        TxtMail.Text = string.Empty;
        //gather search info
        string Filter = setSearchFilter();
        if (Filter.Trim().Equals(string.Empty))
        {
            Session["FILTER"] = null;
            divRes.Visible = false;
            return;
        }
        string ShowCerts = setShowCerts();
        string brand = Session["BRAND"].ToString();
        showSOID = chkSOID.Checked;

        if (Filter.ToString() != string.Empty)
        {
            divRes.Visible = true;
        }

        ViewState["certType"] = ddlTerms.SelectedValue;
        if (chkHistory.Checked == true)
        {
            GridData = objCert.GetHistorySearchResults(gvPR.PageSize, 1, Filter, ShowCerts, showSOID, brand, ViewState["certType"].ToString());
        }
        else
        {
            GridData = objCert.GetSearchResults(gvPR.PageSize, 1, Filter, ShowCerts, showSOID, brand, ViewState["certType"].ToString());
        }
        setDefaultSortingPattern(); // Set a default sorting pattern
        UpdateDataView();
        LblResult.Visible = true;
        LblResult.Text = "Searched for " + txtSearch.Text + " : " + rowCount + " Records found";
    }

    private string setSearchFilter()
    {        
        string Filter = string.Empty;
        string strFilterType = string.Empty;
        DataSet ds = new DataSet();
        if (DdlFilterType.SelectedValue == "ANY")
        {
            strFilterType = " or ";
        }
        else
        {
            strFilterType = " and ";
        }

        if (txtSearch.Text.Equals(string.Empty))
        {
            Filter = string.Empty;
        }
        else
        {
            switch (ddlTerms.SelectedValue)
            {

                case ("CERT"):
                    Filter = Certificate.CI_SERIAL_NUMBER + UIHelper.EQ_SIGN + UIHelper.SINGLE_QUOTE + UIHelper.GetSQLSafeLiteral(txtSearch.Text.Trim()) + UIHelper.SINGLE_QUOTE;
                    break;
                case ("SOID"):
                    Filter = Certificate.CI_SO_ID + UIHelper.EQ_SIGN + UIHelper.SINGLE_QUOTE + UIHelper.GetSQLSafeLiteral(txtSearch.Text.Trim()) + UIHelper.SINGLE_QUOTE;
                    //Filter = Filter + " or " + Certificate.FRU_SO_ID + UIHelper.EQ_SIGN + UIHelper.SINGLE_QUOTE + UIHelper.GetSQLSafeLiteral(txtSearch.Text.Trim()) + UIHelper.SINGLE_QUOTE;

                    break;
                case ("POID"):
                    Filter = Certificate.CI_PO_ID + UIHelper.EQ_SIGN + UIHelper.SINGLE_QUOTE + UIHelper.GetSQLSafeLiteral(txtSearch.Text.Trim()) + UIHelper.SINGLE_QUOTE;
                    //Filter = Filter + " or " + Certificate.FRU_SO_ID + UIHelper.EQ_SIGN + UIHelper.SINGLE_QUOTE + UIHelper.GetSQLSafeLiteral(txtSearch.Text.Trim()) + UIHelper.SINGLE_QUOTE;

                    break;
                case ("ENDPOID"):
                    Filter = Certificate.CI_ENDUSERPO_ID + UIHelper.EQ_SIGN + UIHelper.SINGLE_QUOTE + UIHelper.GetSQLSafeLiteral(txtSearch.Text.Trim()) + UIHelper.SINGLE_QUOTE;
                    //Filter = Filter + " or " + Certificate.FRU_SO_ID + UIHelper.EQ_SIGN + UIHelper.SINGLE_QUOTE + UIHelper.GetSQLSafeLiteral(txtSearch.Text.Trim()) + UIHelper.SINGLE_QUOTE;

                    break;
                case ("FRU"):
                    Filter = Certificate.CI_SERIAL_NUMBER + UIHelper.EQ_SIGN + UIHelper.SINGLE_QUOTE + UIHelper.GetSQLSafeLiteral(txtSearch.Text.Trim()) + UIHelper.SINGLE_QUOTE;
                    break;
                case ("LCERT"):
                    if (txtSearch.Text.Trim().StartsWith("L"))
                    {
                        ds = objCert.GetSerialNumberCertifcateMap(txtSearch.Text.Trim());
                    }
                    else if (txtSearch.Text.Trim().StartsWith("W"))
                    {
                        ds = objCert.getAWCerDetails(txtSearch.Text.Trim(), Session["BRAND"].ToString());
                    }
                    else if (txtSearch.Text.Trim().StartsWith("E"))
                    {
                        ds = objCert.getALECerDetails(txtSearch.Text.Trim(), Session["BRAND"].ToString());
                    }
                    else 
                    {
                        ds = objCert.getAmgCertDetails(txtSearch.Text.Trim(),Session["BRAND"].ToString());
                    }
                    if (ds != null)
                    {
                        string strCertId = ds.Tables[0].Rows[0]["certificate_id"].ToString();
                        Filter = Certificate.CI_SERIAL_NUMBER + UIHelper.EQ_SIGN + UIHelper.SINGLE_QUOTE + UIHelper.GetSQLSafeLiteral(strCertId.Trim()) + UIHelper.SINGLE_QUOTE;
                    }
                    break;
                    default:
                    break;
            }
        }
        string strAdvSearch = SetAdvSearchFilter(strFilterType);
        if (!strAdvSearch.Equals(string.Empty) && Filter == string.Empty)
        {
            Filter = strAdvSearch;
            Filter = Filter.Substring(0, Filter.LastIndexOf(" ") - 3);
            Session["FILTER"] = Filter;
        }
        else if (strAdvSearch.Equals(string.Empty) && !Filter.Equals(string.Empty))
        {
            Session["FILTER"] = Filter;
        }
        else if (!strAdvSearch.Equals(string.Empty) && !Filter.Equals(string.Empty))
        {
            Filter = Filter + strFilterType + strAdvSearch;
            Filter = Filter.Substring(0, Filter.LastIndexOf(" ") - 3);
            Session["FILTER"] = Filter;
        }
        else if (strAdvSearch.Equals(string.Empty) && Filter.Equals(string.Empty))
        {
            Filter = string.Empty;
            Session["FILTER"] = Filter;
        }        
        return Filter;
    }

    private string SetAdvSearchFilter(string strFilterType)
    {
        string strAdvSearch = string.Empty;
        if (!TxtShipTo.Text.Equals(string.Empty))
        {
            strAdvSearch = strAdvSearch + "ship_to_cust like " + UIHelper.SINGLE_QUOTE + "%" + UIHelper.GetSQLSafeLiteral(TxtShipTo.Text.Trim()) + "%" + UIHelper.SINGLE_QUOTE + strFilterType;
        }
        if (!TxtShipToName.Text.Equals(string.Empty))
        {
            strAdvSearch = strAdvSearch + "ship_to_name like " + UIHelper.SINGLE_QUOTE + "%" + UIHelper.GetSQLSafeLiteral(TxtShipToName.Text.Trim()) + "%" + UIHelper.SINGLE_QUOTE + strFilterType;
        }
        if (!TxtSoldTo.Text.Equals(string.Empty))
        {
            strAdvSearch = strAdvSearch + "sold_to_cust like " + UIHelper.SINGLE_QUOTE + "%" + UIHelper.GetSQLSafeLiteral(TxtSoldTo.Text.Trim()) + "%" + UIHelper.SINGLE_QUOTE + strFilterType;
        }
        if (!TxtSoldToName.Text.Equals(string.Empty))
        {
            strAdvSearch = strAdvSearch + "sold_to_name like " + UIHelper.SINGLE_QUOTE + "%" + UIHelper.GetSQLSafeLiteral(TxtSoldToName.Text.Trim()) + "%" + UIHelper.SINGLE_QUOTE + strFilterType;
        }
        if (!TxtBillTo.Text.Equals(string.Empty))
        {
            strAdvSearch = strAdvSearch + "bill_to_cust like " + UIHelper.SINGLE_QUOTE + "%" + UIHelper.GetSQLSafeLiteral(TxtBillTo.Text.Trim()) + "%" + UIHelper.SINGLE_QUOTE + strFilterType;
        }
        if (!TxtBillToName.Text.Equals(string.Empty))
        {
            strAdvSearch = strAdvSearch + "bill_to_name like " + UIHelper.SINGLE_QUOTE + "%" + UIHelper.GetSQLSafeLiteral(TxtBillToName.Text.Trim()) + "%" + UIHelper.SINGLE_QUOTE + strFilterType;
        }
        if (!TxtOrderDate.Text.Equals(string.Empty))
        {
            strAdvSearch = strAdvSearch + "convert(varchar(12),order_date,101) like " + UIHelper.SINGLE_QUOTE + "%" + UIHelper.GetSQLSafeLiteral(TxtOrderDate.Text.Trim()) + "%" + UIHelper.SINGLE_QUOTE + strFilterType;
        }

        return strAdvSearch;
    }

    private  string setShowCerts()
    {
        bool blnSearchOnlyCerts = chkCertsOnly.Checked;
        string strShowcerts = string.Empty;
        if (blnSearchOnlyCerts)
        {
            strShowcerts= UIHelper.DB_AND + Certificate.CI_TYPE +UIHelper.EQ_SIGN+ UIHelper.SINGLE_QUOTE + RowType.Certificate + UIHelper.SINGLE_QUOTE;
        }
        Session["SHOWCERTS"] = strShowcerts;
        return strShowcerts;
    }

    private string getSearchFilter()
    {
        return Session["FILTER"].ToString();
    }

    private string getShowCerts()
    {
        return Session["SHOWCERTS"].ToString();
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
            TxtMail.Visible = true;
            btnSendMail.Visible = true;  
        }
        else
        {
            currentPage.Text = "1";
            totalPages.Text = "1";
            totalRecords.Text = "0";
            TxtMail.Visible = false;
            btnSendMail.Visible = false;            
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
        string Filter = getSearchFilter();
        string ShowCerts = getShowCerts();
        switch (arg)
        {
            case ("next"):

                if (CurrentPage < Totalpages)
                {
                    if (chkHistory.Checked == true)
                    {
                        GridData = objCert.GetHistorySearchResults(gvPR.PageSize, CurrentPage + 1, Filter, ShowCerts, showSOID, Session["BRAND"].ToString(), ViewState["certType"].ToString());
                    }
                    else
                    {
                        GridData = objCert.GetSearchResults(gvPR.PageSize, CurrentPage + 1, Filter, ShowCerts, showSOID, Session["BRAND"].ToString(), ViewState["certType"].ToString());                        
                    }
                    UpdateDataView();
                }
                break;
            case ("prev"):

                if (CurrentPage > 1)
                {
                    if (chkHistory.Checked == true)
                    {
                        GridData = objCert.GetHistorySearchResults(gvPR.PageSize, CurrentPage - 1, Filter, ShowCerts, showSOID, Session["BRAND"].ToString(), ViewState["certType"].ToString());
                    }
                    else
                    {
                        GridData = objCert.GetSearchResults(gvPR.PageSize, CurrentPage - 1, Filter, ShowCerts, showSOID, Session["BRAND"].ToString(), ViewState["certType"].ToString());
                    }
                    UpdateDataView();
                }
                break;
            case ("last"):

                if (CurrentPage != Totalpages && Totalpages > 1)
                {
                    if (chkHistory.Checked == true)
                    {
                        GridData = objCert.GetHistorySearchResults(gvPR.PageSize, Totalpages, Filter, ShowCerts, showSOID, Session["BRAND"].ToString(), ViewState["certType"].ToString());
                    }
                    else
                    {
                        GridData = objCert.GetSearchResults(gvPR.PageSize, Totalpages, Filter, ShowCerts, showSOID, Session["BRAND"].ToString(), ViewState["certType"].ToString());
                    }
                    UpdateDataView();
                }
                break;
            default:
                if (chkHistory.Checked == true)
                {
                    GridData = objCert.GetHistorySearchResults(gvPR.PageSize, 1, Filter, ShowCerts, showSOID, Session["BRAND"].ToString(), ViewState["certType"].ToString());
                }
                else
                {
                    GridData = objCert.GetSearchResults(gvPR.PageSize, 1, Filter, ShowCerts, showSOID, Session["BRAND"].ToString(), ViewState["certType"].ToString());
                }
                UpdateDataView();
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

            if (chkHistory.Checked == true)
            {
                GridData = objCert.GetHistorySearchResults(gvPR.PageSize, Int32.Parse(currentPage.Text), getSearchFilter(), getShowCerts(), showSOID, Session["BRAND"].ToString(), ViewState["certType"].ToString());
            }
            else
            {
                GridData = objCert.GetSearchResults(gvPR.PageSize, Int32.Parse(currentPage.Text), getSearchFilter(), getShowCerts(), showSOID, Session["BRAND"].ToString(), ViewState["certType"].ToString());
            }
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
        if (chkHistory.Checked == true)
        {
            GridData = objCert.GetHistorySearchResults(gvPR.PageSize, Int32.Parse(currentPage.Text), getSearchFilter(), getShowCerts(), showSOID, Session["BRAND"].ToString(), ViewState["certType"].ToString());
        }
        else
        {
            GridData = objCert.GetSearchResults(gvPR.PageSize, Int32.Parse(currentPage.Text), getSearchFilter(), getShowCerts(), showSOID, Session["BRAND"].ToString(), ViewState["certType"].ToString());
        }

        // Refreshes the view	
        UpdateDataView();
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
                            e.Row.Cells[index+1].Controls.Add(icon);
                        }
                    }
                }
            }
        }
        addSortDirectionIcon = false;
    }

    private void setError(Label lbl, string text)
    {
        lbl.Text = text;
        lbl.Visible = true;
        divRes.Visible = false;
        LblResult.Visible = false;
    }

    protected void ddlTerms_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTerms.SelectedValue == "SOID")
        {
            chkCertsOnly.Enabled = true;
            chkSOID.Enabled = false;
            chkSOID.Checked = false;
        }
        else if (ddlTerms.SelectedValue == "POID")
        {
            chkCertsOnly.Enabled = true;
            chkSOID.Enabled = false;
            chkSOID.Checked = false;
        }
        else if (ddlTerms.SelectedValue == "ENDPOID")
        {
            chkCertsOnly.Enabled = true;
            chkSOID.Enabled = false;
            chkSOID.Checked = false;
        }
        else if(!txtSearch.Text.Equals(string.Empty))
        {
            chkCertsOnly.Enabled = false;
            chkCertsOnly.Checked = false;
            chkSOID.Enabled = true;
        }
  
    }

    protected void btnSendMail_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;
        string strTo = TxtMail.Text.Trim();
        //bool blIsExists = true;
        CheckBox chk;
        for (int i = 0; i < gvPR.Rows.Count; i++)
        {
            chk = (CheckBox)(gvPR.Rows[i].Cells[0].FindControl("chkCert"));
            if (chk.Checked == true)
                strCommaSepCertIds += ((HtmlInputHidden)(gvPR.Rows[i].Cells[0].FindControl("hdnCertId"))).Value + ",";

        }
        //check If contact exists in Expandable or SAP

        if (strCommaSepCertIds.Substring(strCommaSepCertIds.Length - 1, 1) == ",")
            strCommaSepCertIds = strCommaSepCertIds.Substring(0, strCommaSepCertIds.Length - 1);

        DataSet dsCert = objCert.getCertDetails(strCommaSepCertIds);
        DataTable dtCert = dsCert.Tables[0];
        if (objEmail.SendCertsMail(dtCert, strTo, Session["BRAND"].ToString()))
        {
            //if (blIsExists == false)
            //{
                //add to the log history.
                UserInfo objUserInfo = (UserInfo)(Session["USER_INFO"]);
                bool blLog = objCert.LogSentEmail(dtCert, objUserInfo.AcctId, strTo);
                _PagerButtonClick("first");
                setError(lblErr, "Mail is sent to the User " + strTo + " Successfully!");
                lblErr.ForeColor = System.Drawing.Color.Green;
                divRes.Visible = true;
            //}
        }
        else
        {
            setError(lblErr, "Failed to send mails");
            lblErr.ForeColor = System.Drawing.Color.Red;
        }   
    }
    protected void cvNoSelection_OnValidate(object sender, ServerValidateEventArgs args)
    {
        if (gvPR.Rows.Count == 0)
        {
            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = "No Certs were selected";
            return;
        }
        CheckBox chk;
        for (int i = 0; i < gvPR.Rows.Count; i++)
        {
            chk = (CheckBox)(gvPR.Rows[i].Cells[0].FindControl("chkCert"));
            if (chk.Checked == true)
                strCommaSepCertIds += ((HtmlInputHidden)(gvPR.Rows[i].Cells[0].FindControl("hdnCertId"))).Value + ",";

        }
        if (strCommaSepCertIds.Equals(string.Empty))
        {
            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = "No Certificates were selected";
        }

        if (TxtMail.Text.Equals(string.Empty))
        {
            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = "Please enter email Id";
        }
    }
    protected void LnkAdvSearch_Click(object sender, EventArgs e)
    {
        if (PanelAdvSearch.Visible == false)
        {
            PanelAdvSearch.Visible = true;
        }
        else
        {
            PanelAdvSearch.Visible = false;
            TxtSoldTo.Text = string.Empty;
            TxtSoldToName.Text = string.Empty;
            TxtBillTo.Text = string.Empty;
            TxtBillToName.Text = string.Empty;
            TxtShipTo.Text = string.Empty;
            TxtShipToName.Text = string.Empty;
            TxtOrderDate.Text = string.Empty;
         }
    }
    protected void chkHistory_CheckedChanged(object sender, EventArgs e)
    {
        if (chkHistory.Checked == true)
        {
            btnSendMail.Enabled = false;
        }
        else
        {
            btnSendMail.Enabled = true;
        }
    }
    protected void DdlFilterType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DdlFilterType.SelectedValue == "ANY")
        {
            LblHelp.Text = "Gives the resultset based on any one of the above entered filter criteria.";
        }
        else
        {
            LblHelp.Text = "Gives the resultset based on all of the above entered filter criteria.";
        }
    }
}
