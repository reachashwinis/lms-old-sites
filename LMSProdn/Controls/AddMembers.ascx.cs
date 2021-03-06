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
using System.Text.RegularExpressions;

public partial class Controls_AddMembers : System.Web.UI.UserControl
{
    private DataSet GridData = new DataSet();
    int rowCount = 0;
    protected static Hashtable columnNames = null;
    bool addSortDirectionIcon = false;
    DataSet dsLookupValues = new DataSet();
    private string strUser = string.Empty;
    User objUser;
    Log objLog;
    private string strBrand = string.Empty;
    protected ArrayList expressions;
    protected ArrayList orders;
    protected ArrayList columns;
    string[] arr;
    string strCommaSepAcctIds;
    
    

    #region error codes
    private const string EMPTY_ACCT = "You have not entered email addresses to be grouped";
    private const string NO_EMAIL_ADD = "No Email addresses were found in your entry!";
    private const string INV_EMAILS = "Please correct the invalid email addresses";
    private const string PEN_ACCTS = "These Email Id(s) are already scheduled to group : ";
    private const string ACT_ACCTS = "These Email Id(s) already exists : ";
    private const string FAILURE_GROUP_ACCTS = "Group Accounts failed!";
    private const string SUCCESS_PEND_ACCTS = "You have successfully created these pending accounts.";
    private const string RESTRICTED_DOMAIN = " These Email Id(s) found in our Restricted Domain list : ";
    
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false;
        objUser = new User();
        if (!IsPostBack)
        {
            if (Session[Company.COMPANY_ID] == null || Session[Company.COMPANY_ID].ToString().Equals(string.Empty))
                Response.Redirect(Request.UrlReferrer.ToString(), true);

            Company objCom = new Company();
            DataSet ds = objCom.GetCompanyInfo(Int32.Parse(Session[Company.COMPANY_ID].ToString()));
            if (ds == null)
            {
                setError(lblErr, Company.NO_COMPANY_INFO);
                return;
            }
            else
            {

               string strCompanyType = ds.Tables[0].Rows[0][Company.COMPANY_TYPE].ToString();
               strCompanyType = (strCompanyType == CompanyType.Customer) ? CompanyType.Customer : strCompanyType;
                setError(lblCompanyInfo, "Adding members for " + strCompanyType + " :" + ds.Tables[0].Rows[0][Company.COMPANY_NAME].ToString());
                Session[Company.COMPANY_TYPE] = strCompanyType;
            }

            if (((UserInfo)Session["USER_INFO"]).GetUserRole() == UserType.Distributor)
            {
                rblAccType.Items.RemoveAt(rblAccType.Items.IndexOf(rblAccType.Items.FindByValue("EXIST")));
                rblAccType.SelectedIndex = rblAccType.Items.IndexOf(rblAccType.Items.FindByValue("NEW"));

            }

            wizAddMem.ActiveStepIndex = 0;
        }
        else
        {
            if (wizAddMem.ActiveStepIndex == 1)
            {
                if(rblAccType.SelectedValue=="EXIST")
                    ShowFilterParams();
            }
        
        }
    }
    protected void btnStep1_OnClick(object sender, EventArgs args)
    {
        if (rblAccType.SelectedValue == "NEW")
        {
            pnlNew.Visible = true;
            pnlExist.Visible = false;
            
        }
        else
        {
            pnlNew.Visible = false; 
            pnlExist.Visible = true;
            LoadUngroupedAccounts(Session[Company.COMPANY_TYPE].ToString());//popualate grid
        
        }
        wizAddMem.ActiveStepIndex = 1;
    
    }

    protected void cvNew_OnServerValidate(object sender, ServerValidateEventArgs args)
    {

        args.IsValid = true;
        if (txtNew.Text.Trim().Equals(string.Empty))
        {
            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = EMPTY_ACCT;
            return;
        
        }
        if (!txtNew.Text.Contains("@"))
        {
            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = NO_EMAIL_ADD;
            return;
        }

        
        string strEmailList = txtNew.Text.Trim().Replace(" ","");//remove all spaces
        strEmailList = strEmailList.Replace(Environment.NewLine, "");//remove carriage returns
        //remove empty entries like ",,abc@as.com,,"
        //Regex objRegex = new Regex(",(,)*");
        //strEmailList= objRegex.Replace(strEmailList, ",");

        //remove starting and ending commas if any
        if (strEmailList.Substring(strEmailList.Length - 1, 1) == ",")
            strEmailList = strEmailList.Substring(0, strEmailList.Length - 1);

        if (strEmailList.Substring(0, 1) == ",")
            strEmailList = strEmailList.Substring(1, strEmailList.Length - 1);

        //split email List

        arr = strEmailList.Split(',');
        string strInvalidEmail = string.Empty;
        bool IsFound = false;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].Trim().Equals(string.Empty) || !arr[i].Contains("@"))
            {
                args.IsValid = false;
                ((CustomValidator)sender).ErrorMessage = INV_EMAILS;
                return;

            }
            //Restricted Domain
            if (objUser.IsRestrictedDomain(arr[i].Trim()))
            {
                strInvalidEmail = strInvalidEmail + arr[i] + ",";
                IsFound = true;
            }
        }

        if (IsFound == true)
        {
            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = RESTRICTED_DOMAIN + strInvalidEmail;
            return;
        }

        //check for accounts that are scheduled to be grouped : pending accounts
        strEmailList = UIHelper.SINGLE_QUOTE+strEmailList.Replace(",","','")+UIHelper.SINGLE_QUOTE;
        DataSet dsBadAdds = objUser.CheckWithGroupAccounts(strEmailList, User.AccountType.PendingAccounts);
        string badAdds = string.Empty;
        if (dsBadAdds.Tables.Count != 0 && dsBadAdds.Tables[0].Rows.Count > 0)
        {
            badAdds += dsBadAdds.Tables[0].Rows[0][0].ToString() + ",";
        
        }

       

        if (badAdds.Length > 0)
        {
            if (badAdds.Substring(badAdds.Length - 1, 1) == ",")
                badAdds = badAdds.Substring(0, badAdds.Length - 1);

            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = PEN_ACCTS+badAdds;
            return;
                    
        }

        //check for accounts in accounts table
        dsBadAdds = objUser.CheckWithGroupAccounts(strEmailList, User.AccountType.ExistingAccounts);
        badAdds = string.Empty;
        if (dsBadAdds.Tables.Count != 0 && dsBadAdds.Tables[0].Rows.Count > 0)
        {
            badAdds += dsBadAdds.Tables[0].Rows[0][0].ToString() + ",";

        }

    

        if (badAdds.Length > 0)
        {

            if (badAdds.Substring(badAdds.Length - 1, 1) == ",")
                badAdds = badAdds.Substring(0, badAdds.Length - 1);

            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = ACT_ACCTS + badAdds;
            return;

        }

    }

    protected void btnAccNew_OnClick(object sender, EventArgs args)
    {
        string meth="AddMembers: btnAccNew_OnClick";
        if (!Page.IsValid)
            return;
        bool retval = true;
        if (arr.Length == 0)
        {
            setError(lblErr, NO_EMAIL_ADD);
            return;
        }
        string strType = string.Empty;
        DataSet dsPendingAcc;
        objLog = new Log();
        for (int i = 0; i < arr.Length; i++)
        {
            dsPendingAcc = objUser.AddPendingAccounts(arr[i], Int32.Parse(Session[Company.COMPANY_ID].ToString()), ((UserInfo)Session["USER_INFO"]).Brand);
            if (dsPendingAcc.Tables[0].Rows.Count == 0)
            {
                objLog.logSystemError(meth, "User.AddPendingAccount() failed for :" + arr[i]);
            }
            else
            { 
                //send Email notification
                 Email objEmail = new Email();
                 AccountMailInfo objLgm = new AccountMailInfo();
                    
                 //objLgm.ActivationCode = objLicUser.AcctActivationCode;
                  objLgm.Brand = ((UserInfo)Session["USER_INFO"]).Brand.ToUpper();
                  objLgm.Email = arr[i];
                  objLgm.ActivationCode = dsPendingAcc.Tables[0].Rows[0]["Acctcode"].ToString();
                  strType = dsPendingAcc.Tables[0].Rows[0]["Types"].ToString();
                 //Have to change this
                  switch (strType)
                   {
                    case UserType.Distributor:
                       retval = objEmail.sendDistributorActivationInfo(objLgm);
                        break;
                    case UserType.Reseller:
                        retval = objEmail.sendResellerActivationInfo(objLgm);
                        break;
                     default:
                        retval = objEmail.sendAccountActivationInfo(objLgm);
                        break;
                     }
                    //return retval;
                }
            }
            setError(LblDisplay, SUCCESS_PEND_ACCTS);
    }

    private void LoadUngroupedAccounts(string compType)
    {
        createColumnNamesList();
        gvPR.Attributes["RecordedExpression"] = "";
        gvPR.Attributes["FilterQuery"] = "";

        GridData = objUser.GetUngroupedAccounts(Session[Company.COMPANY_TYPE].ToString(), gvPR.PageSize, 1, string.Empty);
        setDefaultSortingPattern(); // Set a default sorting pattern
        UpdateDataView();


        //load Dept list
        Lookup objLookup = new Lookup();
        dsLookupValues = (DataSet)Cache[Lookup.LOOKUP_TBL];
        if (dsLookupValues == null)
        {
            dsLookupValues = objLookup.LoadLookupValues(((UserInfo)Session["USER_INFO"]).GetUserEmail(),Session["BRAND"].ToString());
            Cache.Insert(Lookup.LOOKUP_TBL, dsLookupValues, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }
        UIHelper.PrepareAndBindLookupFromAllWithoutPlease(ddlOperators, dsLookupValues.Tables[Lookup.LOOKUP_TBL], "OPERATORS", "TXT", "VAL", false);
        BindFilterColumns();
        //ShowFilterParams();
        //some bug had to force
        pnlFilterParams.Visible = true;
        pnlFilterQuery.Visible = false;
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

    private void createColumnNamesList()
    {
        columnNames = new Hashtable();
        columnNames.Add("Fullname", "Name,1");
        columnNames.Add("Email", "Email,2");
        columnNames.Add("Company", "User-entered Company,3");
        columnNames.Add("AccountType", "Account Type,4");
        columnNames.Add("Status", "Status,5");
        columnNames.Add("CreatedOn", "Created On,6");
    }

    private void BindFilterColumns()
    {

        string[] arrFilCols = { "Name|"+User.FULL_NAME,
                                "Email|"+User.EMAIL,    
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
        string defaultPattern = "FullName ASC";
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
                    GridData = objUser.GetUngroupedAccounts(Session[Company.COMPANY_TYPE].ToString(), gvPR.PageSize, CurrentPage + 1, Filter);
                    UpdateDataView();
                }
                break;
            case ("prev"):

                if (CurrentPage > 1)
                {
                    GridData = objUser.GetUngroupedAccounts(Session[Company.COMPANY_TYPE].ToString(), gvPR.PageSize, CurrentPage - 1, Filter);
                    UpdateDataView();
                }
                break;
            case ("last"):

                if (CurrentPage != Totalpages && Totalpages > 1)
                {
                    GridData = objUser.GetUngroupedAccounts(Session[Company.COMPANY_TYPE].ToString(), gvPR.PageSize, Totalpages, Filter);
                    UpdateDataView();
                }
                break;
            default:
                //if (Totalpages >= 1)
                {
                    GridData = objUser.GetUngroupedAccounts(Session[Company.COMPANY_TYPE].ToString(), gvPR.PageSize, 1, Filter);
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

            GridData = objUser.GetUngroupedAccounts(Session[Company.COMPANY_TYPE].ToString(), gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter());

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

        GridData = objUser.GetUngroupedAccounts(Session[Company.COMPANY_TYPE].ToString(), gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter());


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
                        Label icon = new Label();
                        icon.Font.Size = FontUnit.XSmall;
                        icon.ForeColor = Color.Yellow;
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

    protected void cvExist_OnServerValidate(object sender, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (gvPR.Rows.Count == 0)
        {
            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = NO_EMAIL_ADD;
            return;
        }

       
        strCommaSepAcctIds = string.Empty;
        CheckBox chk;
        for (int i = 0; i < gvPR.Rows.Count; i++)
        {
            chk = (CheckBox)(gvPR.Rows[i].Cells[0].FindControl("chkAcct"));
            if (chk.Checked == true)
                strCommaSepAcctIds += ((HtmlInputHidden)(gvPR.Rows[i].Cells[0].FindControl("hdnAcctId"))).Value + ",";

        }
        if (strCommaSepAcctIds.Equals(string.Empty))
        {
            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = NO_EMAIL_ADD;
        }
        else
        {
            //set Acct Ids
            if (strCommaSepAcctIds.Substring(strCommaSepAcctIds.Length - 1, 1) == ",")
                strCommaSepAcctIds = strCommaSepAcctIds.Substring(0, strCommaSepAcctIds.Length - 1);
        }

    }

    protected void btnAccExist_OnClick(object sender, EventArgs args)
    {
        if(!Page.IsValid)
            return;
        if (strCommaSepAcctIds.Trim().Equals(string.Empty))
        {
            setError(lblErr, NO_EMAIL_ADD);
            return;
        }

        if (objUser.GroupCurrentAccounts(Int32.Parse(Session[Company.COMPANY_ID].ToString()), strCommaSepAcctIds))
            Response.Redirect(ConfigurationManager.AppSettings["SHOWMEMBERS_URL"],true);
        else
        {
            setError(lblErr, FAILURE_GROUP_ACCTS);
            return;
        }
        
        

    }

    private void setError(Label lbl, string txt)
    {
        lbl.Text = txt;
        lbl.Visible = true;
    }
}
