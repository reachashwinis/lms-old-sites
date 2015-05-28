
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
using System.Collections.Generic;


public partial class Controls_AddEditDistributors : System.Web.UI.UserControl
{
    private Company objCompany;
    private int intCompanyId = -1;
    public string COMPANY_TYPE = string.Empty;//set on paretn ASPX page
    private string[] arr;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        objCompany = new Company();
        btnEdit.Visible = true;
        btnAdd.Visible = true;
        lblErr.Visible = false;
 

        if (Session[Company.COMPANY_ID] != null && !Session[Company.COMPANY_ID].ToString().Equals(string.Empty))
            intCompanyId = Int32.Parse(Session[Company.COMPANY_ID].ToString());

        if (!IsPostBack)
        {

            Session["BACK_URL"] = Request.UrlReferrer.ToString();
            if (Session[Company.COMPANY_ID] != null && !Session[Company.COMPANY_ID].ToString().Equals(string.Empty))
                intCompanyId = Int32.Parse(Session[Company.COMPANY_ID].ToString());


            DataSet dsNonDist = objCompany.GetNonDistributorIds(intCompanyId);
            UIHelper.PrepareAndBindListWithoutPlease(cblDistIds, dsNonDist.Tables[0], "TXT", "VAL", false);
            cblDistIds.ClearSelection();

            if (intCompanyId == -1)
                btnEdit.Visible = false;
            else
            {
                btnAdd.Visible = false;
                intCompanyId = Int32.Parse(Session[Company.COMPANY_ID].ToString());
                DataSet ds = objCompany.GetDistributorInfo(intCompanyId);
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
          
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        for (int i = 0; i < cblDistIds.Items.Count; i++)//coz there are multiple ids with same value
                        {
                            if (cblDistIds.Items[i].Value.Equals(dr[0].ToString()))
                                cblDistIds.Items[i].Selected = true;
                        }
                        //tempIndex = cblDistIds.Items.IndexOf((cblDistIds.Items.FindByValue(dr[0].ToString())));
                        //if (tempIndex > -1)
                        //{
                        //    cblDistIds.Items[tempIndex].Selected = true;
                        //    tempIndex = -1;
                        //}



                    }
                }

            }
        }
        else
        {
            if (intCompanyId > -1)
            {btnAdd.Visible = false;
                btnEdit.Visible=true;
            }
            else
            {
                btnAdd.Visible = true;
                btnEdit.Visible = false;
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
                return;

            }
        }

        arr = GetDistIds();
        if (arr.Length == 0)
        {
            args.IsValid = false;
            ((CustomValidator)sender).ErrorMessage = Company.NO_DIST_SELECTED;
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


        if (objCompany.AddCompany(txtCompany.Text.Trim(), ((UserInfo)Session["USER_INFO"]).Brand, COMPANY_TYPE, ((UserInfo)Session["USER_INFO"]).GetUserCompanyId(), arr))
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

        if (objCompany.EditCompany(intCompanyId, txtCompany.Text.Trim(), ((UserInfo)Session["USER_INFO"]).Brand, COMPANY_TYPE, arr))
        {
            Session[Company.COMPANY_ID] = null;
            Response.Redirect(Session["BACK_URL"].ToString(), true);
        }
        else
        {
            setError(lblErr, Company.FAILURE_EDIT_COMPANY);
        }

    }

    private string[] GetDistIds()
    {
        List<string> lstDistIds = new List<string>();
        for (int i = 0; i < cblDistIds.Items.Count; i++)
        {
            if (cblDistIds.Items[i].Selected == true)
            {
                if (lstDistIds.IndexOf(cblDistIds.Items[i].Value) == -1)
                {
                    lstDistIds.Add(cblDistIds.Items[i].Value);
                }
            }

        }

        return lstDistIds.ToArray();
    }
}
