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
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using com.arubanetworks.Licensing.LeadSalesforce;
using System.Text;
using System.IO;
using Com.Arubanetworks.Licensing.Lib.Utils;

public partial class Controls_EvalContentSec : System.Web.UI.UserControl
{
    string strDbConn = ConfigurationManager.ConnectionStrings["LMSDB"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblSuccess.Visible = false;
            Clearfields();
        }
    }

    private void Clearfields()
    {
        txtCompanyName.Text = "";
        txtCity.Text = "";
        txtWebsite.Text = "";
        txtFName.Text = "";
        txtLName.Text = "";
        ddlCountry.ClearSelection();
        txtEmail.Text = "";
        txtUsers.Text = "";
        txtState.Text = "";
        TxtSEName.Text = "";
        TxtSEemail.Text = "";
    }

    protected void cvDupDomain_Validate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = true;
        if (GetEvalCSSInfo(txtEmail.Text.Trim().ToLower(), "") > -1)
        {
            args.IsValid = false;
        }

    }

    private int GetEvalCSSInfo(string email, string name)
    {
        DataSet dsEvals = new DataSet();
        List<SqlParameter> arrSPParams = new List<SqlParameter>();
        int retVal = -1;
        SqlParameter SPParam = new SqlParameter("@DomainName", SqlDbType.NVarChar, 100);
        SPParam.IsNullable = false;
        SPParam.Value = email.Split('@')[1];
        arrSPParams.Add(SPParam);

        SPParam = new SqlParameter("@Name", SqlDbType.NVarChar, 100);
        SPParam.Value = name;
        arrSPParams.Add(SPParam);

        dsEvals = SqlHelper.ExecuteDataset(strDbConn, CommandType.StoredProcedure, "LMS_GetEvalCSSRequest", arrSPParams.ToArray());

        if (dsEvals.Tables.Count > 0)
        {
            retVal = Int32.Parse(dsEvals.Tables[0].Rows[0][0].ToString());

        }
        return retVal;

    }

    private int AddEvalCSS()
    {
        int retVal = -1;
        try
        {
            List<SqlParameter> arrSPParams = new List<SqlParameter>();

            SqlParameter SPParam = new SqlParameter("@Company", SqlDbType.NVarChar, 50);
            SPParam.IsNullable = false;
            SPParam.Value = txtCompanyName.Text.Trim();
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@Website", SqlDbType.NVarChar, 100);
            SPParam.Value = txtWebsite.Text.Trim();
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50);
            SPParam.Value = txtFName.Text.Trim();
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@LastName", SqlDbType.NVarChar, 50);
            SPParam.Value = txtLName.Text.Trim();
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@Email", SqlDbType.NVarChar, 50);
            SPParam.Value = txtEmail.Text.Trim();
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@City", SqlDbType.NVarChar, 50);
            SPParam.Value = txtCity.Text.Trim();
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@Country", SqlDbType.NVarChar, 50);
            SPParam.Value = ddlCountry.SelectedValue;
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@EstimatedCloseDate", SqlDbType.DateTime);
            SPParam.Value = DBNull.Value;
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@UserCount", SqlDbType.Int);
            SPParam.Value = Int32.Parse(txtUsers.Text.Trim());
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@PartnerName", SqlDbType.NVarChar, 50);
            SPParam.Value = hdnPartnerName.Value;
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@PartnerType", SqlDbType.NVarChar, 50);
            SPParam.Value = hdnPartnerType.Value;
            arrSPParams.Add(SPParam);


            SPParam = new SqlParameter("@PartnerContactName", SqlDbType.NVarChar, 50);
            SPParam.Value = hdnPartnerContactName.Value;
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@PartnerContactEmail", SqlDbType.NVarChar, 50);
            SPParam.Value = hdnPartnerContactEmail.Value;
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@PartnerPhone", SqlDbType.NVarChar, 50);
            SPParam.Value = hdnPartnerPhone.Value;
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@User_Field_1", SqlDbType.NVarChar, 250);
            SPParam.Value = DBNull.Value;
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@User_Field_2", SqlDbType.NVarChar, 1000);
            SPParam.Value = DBNull.Value;
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@SalesName", SqlDbType.NVarChar, 100);
            SPParam.Value = TxtSEName.Text.Trim();
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@SalesEmail", SqlDbType.NVarChar, 100);
            SPParam.Value = TxtSEemail.Text.Trim();
            arrSPParams.Add(SPParam);

            SPParam = new SqlParameter("@AccountType", SqlDbType.NVarChar, 20);
            SPParam.Value = ConfigurationManager.AppSettings["ACT_TYPE"].ToString();
            arrSPParams.Add(SPParam);

            retVal = Int32.Parse(SqlHelper.ExecuteScalar(strDbConn, CommandType.StoredProcedure, "LMS_AddEvalCssRequest", arrSPParams.ToArray()).ToString());

            return retVal;
        }
        catch (Exception e)
        {
            throw e;
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs args)
    {
        int recnum = -1;
        try
        {
            recnum = AddEvalCSS();
            if (recnum > -1)
            {
                //gen lead
                WebToLead objwebtoLead = new WebToLead("00D70000000KG1j");
                //objwebtoLead.OrganizationID = "00D70000000KG1j";
                objwebtoLead.LeadSource = hdnLeadSource.Value;
                objwebtoLead.Owner = hdnLeadOwner.Value;

                objwebtoLead.FirstName = txtFName.Text.Trim();
                objwebtoLead.LastName = txtLName.Text.Trim();
                objwebtoLead.Company = txtCompanyName.Text.Trim();
                objwebtoLead.WebSite = txtWebsite.Text.Trim();
                objwebtoLead.Email = txtEmail.Text.Trim();
                objwebtoLead.City = txtCity.Text.Trim();
                objwebtoLead.Country = ddlCountry.SelectedValue;
                objwebtoLead.Users = txtUsers.Text.Trim();
                objwebtoLead.EstimatedCloseDate = TxtDelCloseDt.Text.Trim();
                objwebtoLead.SalesName = TxtSEName.Text.Trim();
                objwebtoLead.SalesEmail = TxtSEemail.Text.Trim();
                objwebtoLead.AccountType = ConfigurationManager.AppSettings["ACT_TYPE"].ToString();

                objwebtoLead.PartnerName = hdnPartnerName.Value;
                objwebtoLead.PartnerContactType = hdnPartnerType.Value;
                objwebtoLead.PartnerContactName = hdnPartnerContactName.Value;
                objwebtoLead.PartnerContactEmail = hdnPartnerContactEmail.Value;
                objwebtoLead.PartnerContactPhone = hdnPartnerPhone.Value;
                
                objwebtoLead.DebugEmail = "";
                objwebtoLead.EnableDebug = false;

                objwebtoLead.BuildWebToLeadUrl();
                objwebtoLead.Submit();

                //send  email to jeremy
                string subj = "Request for CSS Eval by :" + txtEmail.Text.Trim();
                string result = "";
                StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplates/LeadCreation.htm"));
                reader = File.OpenText(Server.MapPath("~/MailTemplates/LeadCreation.htm"));
                result = reader.ReadToEnd();
                reader.Close();
                string message = "";
                message = result.Replace("##COMPANY##", txtCompanyName.Text.Trim());
                message = message.Replace("##URL##", txtWebsite.Text.Trim());
                message = message.Replace("##FNAME##", txtFName.Text.Trim());
                message = message.Replace("##LNAME##", txtLName.Text.Trim());
                message = message.Replace("##EMAIL##", txtEmail.Text.Trim());
                message = message.Replace("##CITY##", txtCity.Text.Trim());
                message = message.Replace("##COUNTRY##", ddlCountry.SelectedItem.Text);
                message = message.Replace("##USERS##", txtUsers.Text.Trim());
                message = message.Replace("##SENAME##", TxtSEName.Text.Trim());
                message = message.Replace("##SEEMAIL##", TxtSEemail.Text.Trim());
                message = message.Replace("##ESTDATE##", TxtDelCloseDt.Text.Trim());
                Email objEmail = new Email();
                objEmail.sendMailFromToCcBccSubMsg("licensing@arubanetworks.com", "licensing@arubanetworks.com", hdnLeadEmail.Value, hdnPartnerContactEmail.Value, subj, message.ToString(), ConfigurationManager.AppSettings["BCC_MAIL"]);

                //objEmail.sendMailFromToCcSubMsg("licensing@arubanetworks.com", "licensing@arubanetworks.com", "arao@arubanetworks.com", "arao@arubanetworks.com", subj, message.ToString(), "arao@arubanetworks.com");


                lblSuccess.Visible = true;
                btnSubmit.Visible = false;
            }
        }
        catch (Exception e)
        {
            lblSuccess.Visible = true;
            lblSuccess.ForeColor = System.Drawing.Color.Red;
            lblSuccess.Text = "This Email is already Registered";
        }
    }
}
