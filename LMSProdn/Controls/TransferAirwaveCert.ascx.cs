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

public partial class Controls_TransferAirwaveCert : System.Web.UI.UserControl
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
    DataSet dsCertInfo;
    DataSet dsSystemInfo;
    CheckBox chk;
    private const string CONST_TRANSFER = "transfer";

    protected void Page_Load(object sender, EventArgs e)
    {
        string remoteUser = ((UserInfo)Session["USER_INFO"]).GetUserEmail();
        intAcctId = ((UserInfo)Session["USER_INFO"]).GetUserAcctId();
        lblActKey.Visible = false;
        lblSystemErr.Visible = false;
        lblCertErr.Visible = false;
        btnTransfer.Enabled = true;

        objCert = new Certificate();
        if (!IsPostBack)
        {
            wizActivate.ActiveStepIndex = 0;
            lblCertErr.Visible = false;
            createColumnNamesList();
            gvPR.Attributes["RecordedExpression"] = "";
            gvPR.Attributes["FilterQuery"] = "";

            GridData = objCert.GetAirwaveCertByAcct(intAcctId, gvPR.PageSize, 1, string.Empty, Session["BRAND"].ToString());
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
        columnNames.Add("PartId", "License P/N,0");
        columnNames.Add("PartDesc", "Description,1");
        columnNames.Add("CertId", "Certificate Id,2");
        columnNames.Add("IPAddress", "IP Address,3");
        columnNames.Add("Organization", "Organization,4");
        columnNames.Add("ActCode", "Activation Key,5");
        columnNames.Add("ActivatedOn", "Activated On,6");
    }

    private void BindFilterColumns()
    {
        string[] arrFilCols = { "Part Name|PartId",
                                "Description|PartDesc",
                                "Certificate Id|CertId",
                                "IP Address|IPAddress",
                                "Organization|Organization",
                                "Activation Key|ActCode"
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
                    GridData = objCert.GetAirwaveCertByAcct(intAcctId, gvPR.PageSize, 1, string.Empty, Session["BRAND"].ToString());
                    UpdateDataView();
                }
                break;
            case ("prev"):

                if (CurrentPage > 1)
                {

                    GridData = objCert.GetAirwaveCertByAcct(intAcctId, gvPR.PageSize, 1, string.Empty, Session["BRAND"].ToString());
                    UpdateDataView();
                }
                break;
            case ("last"):

                if (CurrentPage != Totalpages && Totalpages > 1)
                {
                    GridData = objCert.GetAirwaveCertByAcct(intAcctId, gvPR.PageSize, 1, string.Empty, Session["BRAND"].ToString());
                    UpdateDataView();
                }
                break;
            default:
                //if (Totalpages >= 1)
                {
                    GridData = objCert.GetAirwaveCertByAcct(intAcctId, gvPR.PageSize, 1, string.Empty, Session["BRAND"].ToString());
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

            GridData = objCert.GetAirwaveCertByAcct(intAcctId, gvPR.PageSize, 1, string.Empty, Session["BRAND"].ToString());
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
        GridData = objCert.GetAirwaveCertByAcct(intAcctId, gvPR.PageSize, 1, string.Empty, Session["BRAND"].ToString());

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

    protected void GotoSystem_OnCommand(object sender, EventArgs args)
    {
        DataTable dt = objCert.BuildAirwaveCertInfo();
        string strCertid = ((LinkButton)sender).CommandArgument.ToString();
        DataRow dr = dt.NewRow();

        //get Cert info

        dsCertInfo = objCert.GetAirwaveCertInfoForIP(Int32.Parse(strCertid));
        if (dsCertInfo.Tables[0].Rows.Count <= 0)
        {
            setError(lblCertErr, Certificate.NO_CERT_INFO);
            return;
        }
        //To keep tab on number of transfers-ash
        if (!((UserInfo)Session["USER_INFO"]).GetImpersonateUserRole().Contains(ConfigurationManager.AppSettings["ARUBA_ROLE"].ToString()))
        {
            if (objCert.CheckAWTransferCount(((UserInfo)Session["USER_INFO"]).GetUserAcctId(), Int32.Parse(strCertid), CONST_TRANSFER))
            {
                setError(lblCertErr, Certificate.MAX_AIRW_COUNT);
                return;
            }
        }

        if (!objCert.CheckAirwaveOwnership(((UserInfo)Session["USER_INFO"]).GetUserAcctId(), Int32.Parse(strCertid)))
        {
            setError(lblCertErr, Certificate.OWNER_FAIL);
            return;
        }

        for (int i = 0; i < dsCertInfo.Tables[0].Rows.Count; i++)
        {

            dr = dt.NewRow();
            dr["pk_cert_id"] = Int32.Parse(dsCertInfo.Tables[0].Rows[i]["pk_cert_id"].ToString());
            dr["serial_number"] = dsCertInfo.Tables[0].Rows[i]["serial_number"].ToString();
            dr["ActivationKey"] = dsCertInfo.Tables[0].Rows[i]["ActivationKey"].ToString();
            dr["part_id"] = dsCertInfo.Tables[0].Rows[i]["part_id"].ToString();
            dr["part_desc"] = dsCertInfo.Tables[0].Rows[i]["Part_desc"].ToString();
            dr["Organization"] = dsCertInfo.Tables[0].Rows[i]["Organization"].ToString();
            dr["IPAddress"] = dsCertInfo.Tables[0].Rows[i]["IPAddress"].ToString();
            dr["so_id"] = dsCertInfo.Tables[0].Rows[i]["so_id"].ToString();
            dr["IsConsolidated"] = dsCertInfo.Tables[0].Rows[i]["IsConsolidated"].ToString();
            dr["ConsolidateIP"] = dsCertInfo.Tables[0].Rows[i]["ConsolidateIP"].ToString();
            dr["Lserial_number"] = dsCertInfo.Tables[0].Rows[i]["Lserial_number"].ToString();

            dt.Rows.Add(dr);
        }

        lblSystem.Text = "Please enter IP Address of the system";

        LblCertInfo.Text = "Following Licenses(s) are Activated for <BR>" + " IP Address " +
        dr["IPAddress"].ToString() + " Please select to transfer the license(s) for following IP Address.";
        Session["CERT_INFO"] = dt;
        wizActivate.ActiveStepIndex = 1;
        GrdVw2.DataSource = dt;
        GrdVw2.DataBind();
        for (int i = 0; i < GrdVw2.Rows.Count; i++)
        {
            chk = (CheckBox)(GrdVw2.Rows[i].FindControl("chkCert"));
            if (strCertid == dsCertInfo.Tables[0].Rows[i]["pk_cert_id"].ToString())
            {             
                chk.Checked = true;
            }
            if (bool.Parse(dsCertInfo.Tables[0].Rows[i]["IsConsolidated"].ToString()) == true)
            {
                chk.Checked = true;
                chk.Enabled = false;
            }
        }
    }

    protected void TransferLic_Onclick(object sender, EventArgs args)
    {
        string method = "TransferLic_Onclick";
        string strSystemIP = txtSystem.Text.Trim();
        int APCount = 0;
        DataTable dt = (DataTable)Session["CERT_INFO"];
        UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
        DataTable dtCopy = objCert.BuildAirwaveCertInfo();
        DACertificate daoCert = new DACertificate();
        AirwaveKeyProcessor objAirwaveKeyProcessor = new AirwaveKeyProcessor();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string strSysPartId = string.Empty;
            string strActivationCode = string.Empty;
            string strCertId = string.Empty;
            chk = (CheckBox)(GrdVw2.Rows[i].FindControl("chkCert"));
            if (chk.Checked == true)
            {
                strCertId = dt.Rows[i]["serial_number"].ToString();

                if (daoCert.IsAirwaveCertActivated(strCertId, strSystemIP))
                {
                    setError(lblSystemErr, Certificate.YES_ACTIVATED_SYSTEM);
                    return;
                }

                strSysPartId = dt.Rows[i]["part_id"].ToString();
                //if (strSysPartId.Contains("EXP") || strSysPartId.Contains("EXF") || strSysPartId.Contains("AW-K12"))
                //{
                    objAirwaveKeyProcessor.TheKey = objCert.getAWLicenseKey(strCertId);
                    if (objAirwaveKeyProcessor.TheKey != string.Empty)
                    {
                        APCount = objAirwaveKeyProcessor.APCount;
                    }
                    else
                    {
                        APCount = 0;
                    }
                //}

                //get activation code

                strActivationCode = objCert.GenerateAirwaveActivation(dt.Rows[i]["so_id"].ToString(), strSystemIP, dt.Rows[i]["part_id"].ToString(), dt.Rows[i]["Organization"].ToString(), APCount, dt.Rows[i]["Lserial_number"].ToString());

                if (strActivationCode == String.Empty || (strActivationCode == null) || strActivationCode.Contains("Error"))
                {
                    setError(lblSystemErr, Certificate.FAILURE_KEYGEN);
                    new Log().logSystemError(method, Certificate.FAILURE_KEYGEN + ":CertId: " + dt.Rows[i]["serial_number"].ToString()
                                                                          + " IP Address:" + dt.Rows[i]["IPAddress"].ToString());
                    return;

                }
                else
                {
                    dt.Rows[i]["ActivationKey"] = strActivationCode;
                    dt.Rows[i]["IPAddress"] = strSystemIP;
                    dtCopy.LoadDataRow(dt.Rows[i].ItemArray, false);
                }
            }
        }


        if (objCert.UpdateAirwaveActivationInfo(dtCopy, objUserInfo.GetUserAcctId()))
        {
            for (int index = 0; index < dtCopy.Rows.Count; index++ )
            {
                dtCopy.Rows[index]["ActivationKey"] = dtCopy.Rows[index]["ActivationKey"].ToString().Replace("\n", "<BR>");
            }
            LoadWizSteplast(dtCopy);
        }
        else
        {
            setError(lblSystemErr, Certificate.PERSISTENCE_ISSUE);
        }

    }

    private void setError(Label lbl, string text)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }

    private void LoadWizSteplast(DataTable dt)
    {
        wizActivate.ActiveStepIndex = 2;
        Session["CERT_INFO"] = dt;
        Grdvw3.DataSource = dt;
        Grdvw3.DataBind();
        LblSysInfo.Text = "System IP Address : " + dt.Rows[0]["IPAddress"].ToString();
    }
}


