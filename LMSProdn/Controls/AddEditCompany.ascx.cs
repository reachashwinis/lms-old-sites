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


public partial class Controls_AddEditCompany : System.Web.UI.UserControl
{
    private Company objCompany;
    private int intCompanyId = -1;
    public string COMPANY_TYPE = string.Empty;//set on paretn ASPX page
    
    protected void Page_Load(object sender, EventArgs e)
    {
        objCompany = new Company();
        btnEdit.Visible = true;
        btnAdd.Visible = true;
        lblErr.Visible = false;

        if(!IsPostBack)
            Session["BACK_URL"] = Request.UrlReferrer.ToString();
            

        if (Session[Company.COMPANY_ID] == null || Session[Company.COMPANY_ID].ToString().Equals(string.Empty))
            btnEdit.Visible = false;
        else
        {
            btnAdd.Visible = false;
            intCompanyId = Int32.Parse(Session[Company.COMPANY_ID].ToString());
            DataSet ds = objCompany.GetCompanyInfo(intCompanyId);
            if (ds == null)
            {
                Session[Company.COMPANY_ID] = null;
                intCompanyId = -1;
                btnAdd.Visible = true;
                btnEdit.Visible = false;
            }
            else
            {
                txtCompany.Text = ds.Tables[0].Rows[0][Company.COMPANY_NAME].ToString().Trim();
                hdnOldName.Value = txtCompany.Text;
            }
                
        }
        
    }

    protected void cvCompany_OnServerValidate(object sender, ServerValidateEventArgs args)
    {
        if (!hdnOldName.Value.Equals(txtCompany.Text))
        {
            DataSet ds = objCompany.CheckDistinctCompanyName(txtCompany.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                args.IsValid = false;
                ((CustomValidator)sender).ErrorMessage = Company.DUP_COMPANY_NAME;

            }
        }
    }

    private void setError(Label lbl, string text)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }

    protected void btnAdd_OnClick(object sender, EventArgs args)
    {
        if (!Page.IsValid)
            return;
        
        if(objCompany.AddCompany(txtCompany.Text.Trim(),((UserInfo)Session["USER_INFO"]).Brand,COMPANY_TYPE,((UserInfo)Session["USER_INFO"]).GetUserCompanyId()))
        {
            Session[Company.COMPANY_ID] = null;
            Response.Redirect(Session["BACK_URL"].ToString(), true);
        }
        else
        {
        setError(lblErr,Company.FAILURE_ADD_COMPANY);
        }
    }

    protected void btnEdit_OnClick(object sender, EventArgs args)
    {
        if (!Page.IsValid)
            return;

        if (objCompany.EditCompany(intCompanyId, txtCompany.Text.Trim(), ((UserInfo)Session["USER_INFO"]).Brand, COMPANY_TYPE))
        {
            Session[Company.COMPANY_ID] = null;
            Response.Redirect(Session["BACK_URL"].ToString(), true);
        }
        else
        {
            setError(lblErr, Company.FAILURE_EDIT_COMPANY);
        }

    }
}
