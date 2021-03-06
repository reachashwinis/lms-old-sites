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

public partial class Controls_TransferCert : System.Web.UI.UserControl
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
    DataSet dsCertInfo ;
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

            GridData = objCert.GetActivePermCertsByAcct(intAcctId, gvPR.PageSize, 1, GetCertTypeToView(),Session["BRAND"].ToString());
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
        columnNames.Add("PartId", "License P/N,1");
        columnNames.Add("PartDesc", "Description,2");
        columnNames.Add("SerialNum", "Certificate Id,3");
        columnNames.Add("Fru", "System S/N,4");
        columnNames.Add("SystemPartId", "System P/N,5");
        columnNames.Add("SystemDesc", "System Desc,6");
        columnNames.Add("SystemLoc", "System Location,7");
        columnNames.Add("ActCode", "License Key,8");
        columnNames.Add("ActivatedOn", "Activated On,9");
    }

    private void BindFilterColumns()
    {

        string[] arrFilCols = { "License P/N|Part_Id",
                                "Description|Part_Desc",
                                "Certificate Id|Serial_Number",
                                "System S/N|Fru", 
                                "System P/N|System_Part_Id",
                                "System Desc|System_Desc",
                                "License Key|Activation_Code",
                                "System Location|location"
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
                    GridData = objCert.GetActivePermCertsByAcct(intAcctId, gvPR.PageSize, CurrentPage + 1, Filter,Session["BRAND"].ToString());
                    UpdateDataView();
                }
                break;
            case ("prev"):

                if (CurrentPage > 1)
                {

                    GridData = objCert.GetActivePermCertsByAcct(intAcctId, gvPR.PageSize, CurrentPage - 1, Filter, Session["BRAND"].ToString());
                    UpdateDataView();
                }
                break;
            case ("last"):

                if (CurrentPage != Totalpages && Totalpages > 1)
                {
                    GridData = objCert.GetActivePermCertsByAcct(intAcctId, gvPR.PageSize, Totalpages, Filter, Session["BRAND"].ToString());
                    UpdateDataView();
                }
                break;
            default:
                //if (Totalpages >= 1)
                {
                    GridData = objCert.GetActivePermCertsByAcct(intAcctId, gvPR.PageSize, 1, Filter, Session["BRAND"].ToString());
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

            GridData = objCert.GetActivePermCertsByAcct(intAcctId, gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter(),Session["BRAND"].ToString());
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
        GridData = objCert.GetActivePermCertsByAcct(intAcctId, gvPR.PageSize, Int32.Parse(currentPage.Text), GetFilter(),Session["BRAND"].ToString());

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
        DataTable dt = objCert.BuildCertInfo();
        string strVersion = string.Empty;
        string strVer = string.Empty;
         string strCertid  =   ((LinkButton)sender).CommandArgument.ToString();
         DataRow dr = dt.NewRow();
        
        //get Cert info
         switch (ddlCertVersion.SelectedValue)
         {
             case "PRE":
                 strVersion = "%3%";
                 strVer = "Pre 5.0";
                 break;
             case "POST":
                 strVersion = "%5%";
                 strVer = "Post 5.0";
                 break;
         }

         dsCertInfo = objCert.GetCertInfoForFru(Int32.Parse(strCertid), strVersion);
         if (dsCertInfo == null)
         {
             
             setError(lblCertErr, Certificate.NO_CERT_INFO);
             return;

         }
        //To keep tab on number of transfers-ash
         if (!((UserInfo)Session["USER_INFO"]).GetImpersonateUserRole().Contains(ConfigurationManager.AppSettings["ARUBA_ROLE"].ToString()))
         {               
             //int i = Int32.Parse(ConfigurationManager.AppSettings["MAXTRN_COUNT"].ToString());
            // DataSet ds = objCert.CheckTransferCount(((UserInfo)Session["USER_INFO"]).GetUserAcctId(), Int32.Parse(strCertid), CONST_TRANSFER);
             DataSet ds = objCert.CheckTransferCountNew(Int32.Parse(strCertid));
             if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
             {
                 return;
             }
             else if (Int32.Parse(ds.Tables[0].Rows[0]["TransferCount"].ToString()) > Int32.Parse(ConfigurationManager.AppSettings["MAXTRN_COUNT"].ToString()))
             {
                 setError(lblCertErr, Certificate.MAX_COUNT);
                 return;
             }
             //else
             //{
             //    string strFilter = string.Empty;
             //    for (int i = 0; i < dsCertInfo.Tables[0].Rows.Count; i++)
             //    {
             //        bool bl = objCert.CheckTransferCount(((UserInfo)Session["USER_INFO"]).GetUserAcctId(), Int32.Parse(dsCertInfo.Tables[0].Rows[i]["pk_cert_id"].ToString()), CONST_TRANSFER);
             //        if (!bl)
             //        {
             //            strFilter = dsCertInfo.Tables[0].Rows[i]["pk_cert_id"].ToString() + ",";
             //        }
             //        DataView dv = dsCertInfo.Tables[0].DefaultView;
             //        dv.RowFilter = "pk_cert_id in";
             //    }
             //}
         }
         if (CertType.AccountCertificate != ConfigurationManager.AppSettings["TRANSFER_CERT"].ToString())
         {
          if (!objCert.CheckOwnershipNew(((UserInfo)Session["USER_INFO"]).GetUserCompanyId(), Int32.Parse(strCertid), CertType.CompanyCertificate))
            {
             
             setError(lblCertErr, Certificate.OWNER_FAIL);
             return;
            }
         }
        else
        {
         if (!objCert.CheckOwnershipNew(((UserInfo)Session["USER_INFO"]).GetUserAcctId(), Int32.Parse(strCertid), CertType.AccountCertificate))
            {
             
             setError(lblCertErr, Certificate.OWNER_FAIL);
             return;
            }
        }

         for (int i = 0; i < dsCertInfo.Tables[0].Rows.Count; i++)
         {
             dr = dt.NewRow();
             dr[Certificate.LIC_ID] = Int32.Parse(dsCertInfo.Tables[0].Rows[i]["pk_cert_id"].ToString());
             dr[Certificate.LIC_SERIAL_NUMBER] = dsCertInfo.Tables[0].Rows[i]["serial_number"].ToString();
             dr[Certificate.ACTIVATION_KEY] = dsCertInfo.Tables[0].Rows[i]["activation_code"].ToString();
             dr[Certificate.LIC_PART_ID] = dsCertInfo.Tables[0].Rows[i]["part_id"].ToString();
             dr[Certificate.LIC_ALC_PART_ID_3EM] = dsCertInfo.Tables[0].Rows[i]["AlcatelPartId"].ToString();
             dr[Certificate.FORCE_SN_MAC] = dsCertInfo.Tables[0].Rows[i]["ForceSNOrMAC"].ToString();
             dr[Certificate.LIC_PART_DESC] = dsCertInfo.Tables[0].Rows[i]["Part_desc"].ToString();
             dr[Certificate.SYS_PART_DESC] = dsCertInfo.Tables[0].Rows[i]["Fru_desc"].ToString();
             dr[Certificate.SYS_SERIAL_NUMBER] = dsCertInfo.Tables[0].Rows[i]["Fru"].ToString();

             if (!((UserInfo)Session["USER_INFO"]).GetImpersonateUserRole().Contains(ConfigurationManager.AppSettings["ARUBA_ROLE"].ToString()))
             {
                 //int i = Int32.Parse(ConfigurationManager.AppSettings["MAXTRN_COUNT"].ToString());
                 DataSet ds = objCert.CheckTransferCountNew(Int32.Parse(dsCertInfo.Tables[0].Rows[i]["pk_cert_id"].ToString()));
                 if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                 {
                     dr[Certificate.LIC_ERROR] = Certificate.MAX_COUNT;
                     dr[Certificate.COMMENTS] = "ERROR";
                     dsCertInfo.Tables[0].Rows[i]["ErrorType"] = "ERROR";
                 }
                 else if (Int32.Parse(ds.Tables[0].Rows[0]["TransferCount"].ToString()) == Int32.Parse(ConfigurationManager.AppSettings["MAXTRN_COUNT"].ToString()))
                 {
                     dr[Certificate.LIC_ERROR] = "This Certificate has transferred " + ConfigurationManager.AppSettings["MAXTRN_COUNT"].ToString() + " times and has now reached the maximum allocated number. You will not be able to transfer it any more time.";
                     dr[Certificate.COMMENTS] = "WARNING";
                     dsCertInfo.Tables[0].Rows[i]["ErrorType"] = "WARNING";
                 }
                 else if (Int32.Parse(ds.Tables[0].Rows[0]["TransferCount"].ToString()) > Int32.Parse(ConfigurationManager.AppSettings["MAXTRN_COUNT"].ToString()))
                 {
                     dr[Certificate.LIC_ERROR] = Certificate.MAX_COUNT;
                     dr[Certificate.COMMENTS] = "ERROR";
                     dsCertInfo.Tables[0].Rows[i]["ErrorType"] = "ERROR";
                 }
                 else if (Int32.Parse(ds.Tables[0].Rows[0]["TransferCount"].ToString()) == 1)
                 {
                    dr[Certificate.LIC_ERROR] = "This Certificate has transferred " + ds.Tables[0].Rows[0]["TransferCount"].ToString() + " time.";
                    dr[Certificate.COMMENTS] = "WARNING";
                    dsCertInfo.Tables[0].Rows[i]["ErrorType"] = "WARNING";
                 }
                 else if (Int32.Parse(ds.Tables[0].Rows[0]["TransferCount"].ToString()) == 0)
                 {
                     dr[Certificate.LIC_ERROR] = string.Empty;
                     dr[Certificate.COMMENTS] = "WARNING";
                     dsCertInfo.Tables[0].Rows[i]["ErrorType"] = "WARNING";
                 }
                 else
                 {
                     dr[Certificate.LIC_ERROR] = "This Certificate has transferred " + ds.Tables[0].Rows[0]["TransferCount"].ToString() + " times.";
                     dr[Certificate.COMMENTS] = "WARNING";
                     dsCertInfo.Tables[0].Rows[i]["ErrorType"] = "WARNING";
                 }
             }

             dt.Rows.Add(dr);
         }
         if (!string.Empty.Equals(dr[Certificate.FORCE_SN_MAC].ToString()))
         {
             lblSystem.Text = "Please enter MAC of the System";
         }
         else
         {
             lblSystem.Text = "Please enter Serial number of the system";
         }
         LblCertInfo.Text = "Following " + strVer + " Licenses(s) are Activated for <BR>" + " system " +
          dr[Certificate.SYS_SERIAL_NUMBER].ToString() + "(" + dr[Certificate.SYS_PART_DESC].ToString() + ")" + " Please select  to transfer the license(s) for following system.";
         Session["CERT_INFO"] = dt;
         wizActivate.ActiveStepIndex = 1;
         GrdVw2.DataSource = dt;
         GrdVw2.DataBind();
         for (int i = 0; i < GrdVw2.Rows.Count; i++)
         {
             chk = (CheckBox)(GrdVw2.Rows[i].FindControl("chkCert"));
             if (dsCertInfo.Tables[0].Rows[i]["ErrorType"].ToString() == string.Empty || dsCertInfo.Tables[0].Rows[i]["ErrorType"].ToString() == "WARNING")
             {
                 if (strCertid == dsCertInfo.Tables[0].Rows[i]["pk_cert_id"].ToString())
                 {
                     chk.Checked = true;
                     chk.Enabled = true;
                 }
                 else
                 {
                     chk.Enabled = true;
                 }
             }
             else
             {
                 chk.Checked = false;
                 chk.Enabled = false;
             }
         }
    }

    protected void TransferLic_Onclick(object sender, EventArgs args)
    {
        string method = "TransferLic_Onclick";
        string strSystemSerialNumber = txtSystem.Text.Trim();
        string strSystemSerialText = strSystemSerialNumber;
        DataTable dt = (DataTable)Session["CERT_INFO"];
        UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
        DataTable dtCopy = objCert.BuildCertInfo();
        bool blUpgradeVersion = false;
        
        DACertificate daoCert = new DACertificate();
        KeyGenInput objKeyGenIP = new KeyGenInput();
        objKeyGenIP.SystemSerialNumber = strSystemSerialText;
        string srcFru = dt.Rows[0][Certificate.SYS_SERIAL_NUMBER].ToString();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string strSysPartId = string.Empty;
            string strActivationCode = string.Empty;
            string strSystemMac = string.Empty;
            chk = (CheckBox)(GrdVw2.Rows[i].FindControl("chkCert"));
            if (chk.Checked == true)
            {
                objKeyGenIP.CertSerialNumber = dt.Rows[i]["LicSN"].ToString();
                objKeyGenIP.Brand = objUserInfo.Brand.ToString();

                if (true)
                {
                    if (dt.Rows[i][Certificate.LIC_PART_ID].ToString().Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString()))
                    {
                        if (objCert.HasPartMapEntry(objKeyGenIP) == false)
                        {
                            setError(lblSystemErr, Certificate.NO_PART_MAP);
                            return;
                        }
                        else
                        {
                            dt.Rows[i][Certificate.IS_ARUBA_RFP] = "YES";
                            //continue;
                        }
                    }
                }

                //Check for version

                if (blUpgradeVersion == false && daoCert.isUpgradebleCert(dt.Rows[i][Certificate.LIC_PART_ID].ToString()))
                {
                    blUpgradeVersion = true;
                }

                bool isMac = false;
                if (!dt.Rows[i][Certificate.LIC_ALC_PART_ID_3EM].ToString().Equals(string.Empty))
                {
                    isMac = true;
                    if (UIHelper.IsMacAddress(strSystemSerialNumber))
                    {
                        DataSet dsconfigdata = objCert.GetConfigData(strSystemSerialNumber);
                        if (dsconfigdata != null)
                        {
                            strSystemMac = strSystemSerialNumber;
                            strSystemSerialNumber = dsconfigdata.Tables[0].Rows[i]["serialnumber"].ToString();
                            // ismac = false; // commmenting for now.!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        }
                    }
                    else
                    {
                        isMac = false;
                    }

                    switch (dt.Rows[i][Certificate.FORCE_SN_MAC].ToString())
                    {
                        case "FORCE_SERIAL":
                            if (isMac)
                            {
                                setError(lblSystemErr, "You must enter a serial number for this certificate");
                                return;

                            }
                            break;
                        case "FORCE_MAC":
                            if (!isMac)
                            {
                                setError(lblSystemErr, "You must enter a HW MAC address for this certificate<BR />The MAC address must be in the form xx::xx:xx:xx:xx:xx and must consist of valid hex characters");
                                return;
                            }

                            break;
                        default:
                            break;

                    }
                    if (isMac)
                    {
                        //check if mac exists in esiTransfertable
                        objCert.SerialNumber = strSystemSerialNumber;
                        dsSystemInfo = objCert.GetCertInfo(objCert.SerialNumber);
                        if (dsSystemInfo == null)
                        {
                            //add the MAC to esitransfertable
                            objCert.AddMAC(strSystemSerialNumber, objUserInfo.Brand);
                        }

                    }
                }//ends alcatelPArtId - MM check ends

                dsSystemInfo = objCert.GetCertInfo(strSystemSerialNumber);
                if (dsSystemInfo == null)
                {
                    setError(lblSystemErr, Certificate.NO_SYS_INFO);
                    return;
                }
                else
                {
                    dt.Rows[i][Certificate.SYS_ID] = Int32.Parse(dsSystemInfo.Tables[0].Rows[0]["pk_cert_id"].ToString());
                    dt.Rows[i][Certificate.SYS_PART_ID] = dsSystemInfo.Tables[0].Rows[0]["part_id"].ToString();
                    dt.Rows[i][Certificate.SYS_SERIAL_NUMBER] = strSystemSerialNumber;
                }

                int CertId = Int32.Parse(dt.Rows[i][Certificate.LIC_ID].ToString());
                int SysId = Int32.Parse(dt.Rows[i][Certificate.SYS_ID].ToString());
                if (daoCert.IsCertSystemActivated(CertId, SysId))
                {
                    setError(lblSystemErr, Certificate.YES_ACTIVATED_SYSTEM);
                    return;
                }

                // Upgrades 
                string UpgradedPartId;
                UpgradedPartId = daoCert.IsControllerUpgraded(SysId);
                if (!(string.Empty).Equals(UpgradedPartId))
                {
                    dt.Rows[i][Certificate.SYS_PART_ID] = UpgradedPartId.ToString();
                }
                // Upgrades are done.

                if (!daoCert.IsCertSystemCompatible(dt.Rows[i][Certificate.LIC_PART_ID].ToString(), dt.Rows[i][Certificate.SYS_PART_ID].ToString(), Session["BRAND"].ToString()))
                {
                    setError(lblSystemErr, Certificate.NO_CERT_SYSTEM_COMPATIBLE);
                    return;
                }

                //get activation code
                if ((dt.Rows[i][Certificate.LIC_ALC_PART_ID_3EM].ToString() != string.Empty) && (isMac = true) && (dt.Rows[i][Certificate.FORCE_SN_MAC].ToString() == "FORCE_MAC"))
                {
                    KeyGenInput objKeyGenIp = new KeyGenInput(dt.Rows[i][Certificate.LIC_SERIAL_NUMBER].ToString(),
                                                  strSystemMac, objUserInfo.Brand);
                    strActivationCode = objCert.GenerateActivationKey(objKeyGenIp);
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    KeyGenInput objKeyGenIp = new KeyGenInput(dt.Rows[i][Certificate.LIC_SERIAL_NUMBER].ToString(),
                                                                            dt.Rows[i][Certificate.SYS_SERIAL_NUMBER].ToString(),
                                                                             objUserInfo.Brand);
                    strActivationCode = objCert.GenerateActivationKey(objKeyGenIp);
                    System.Threading.Thread.Sleep(1000);
                }

                if (strActivationCode == String.Empty || (strActivationCode == null))
                {
                    setError(lblSystemErr, Certificate.FAILURE_KEYGEN);
                    new Log().logSystemError(method, Certificate.FAILURE_KEYGEN + ":CertId: " + dt.Rows[i][Certificate.LIC_SERIAL_NUMBER].ToString()
                                                                                + " SystemId:" + dt.Rows[i][Certificate.SYS_SERIAL_NUMBER].ToString());
                    return;

                }
                else
                {
                    dt.Rows[i][Certificate.ACTIVATION_KEY] = strActivationCode;
                    dtCopy.LoadDataRow(dt.Rows[i].ItemArray, false);
                }
            }
        }

        //Update Upgrade Status
        if (blUpgradeVersion == true)
        {
            if (!objCert.UpdateUpgradeStat(srcFru, strSystemSerialNumber))
            {
                //send mail
                Email objEmail = new Email();
                objEmail.UpdateFailureInfo(srcFru + "," + strSystemSerialNumber, ConfigurationManager.AppSettings["BCC_MAIL"].ToString());
            }
        }

        string strtype = new Lookup().GetLookupText(objUserInfo.GetUserRole(), Lookup.CERT_TYPE);
        if (objCert.UpdateActivationInfo(dtCopy, objUserInfo.GetUserAcctId(), strtype))
        {
            LoadWizSteplast(dtCopy);
        }
        else
        {
            setError(lblSystemErr, Certificate.PERSISTENCE_ISSUE);
        }
    }

    private void setError(Label lbl,string text)
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
        LblSysInfo.Text = "System Part Number : " + dt.Rows[0]["SysPartId"].ToString() + "<BR/> System Part Description " + dt.Rows[0]["SysPartDesc"].ToString() + "<BR/> System Serial Number" + dt.Rows[0]["SysSN"].ToString();
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
        GridData = objCert.GetActivePermCertsByAcct(intAcctId, gvPR.PageSize, Int32.Parse(currentPage.Text), Filter, Session["BRAND"].ToString());
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
