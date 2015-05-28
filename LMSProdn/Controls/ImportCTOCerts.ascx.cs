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

public partial class Controls_ImportCTOCerts : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErr.Visible = false;
    }

    private void setError(Label lbl, string text)
    {
        lbl.Text = text;
        lbl.Visible = true;
    }

    protected void cvCertId_OnServerValidate(object sender, ServerValidateEventArgs args)
    {
        if (chkyes.Checked == false)
        {
            string strCertId = txtCertId.Text;
            if (strCertId == string.Empty)
            {
                ((CustomValidator)sender).ErrorMessage = "Please Enter Certificate Id";
                args.IsValid = false;
            }
            else
            {
                Certificate objCert = new Certificate();
                if (objCert.IsCTOedCert(strCertId))
                    args.IsValid = true;
                else
                {
                    ((CustomValidator)sender).ErrorMessage = Certificate.NO_CERT_INFO;
                    args.IsValid = false;
                }
            }
        }
    }

    protected void cvFruId_OnServerValidate(object sender, ServerValidateEventArgs args)
    {
        if (chkyes.Checked == true)
        {
            string strFruId = txtFru.Text;
            if (strFruId == string.Empty)
            {
                ((CustomValidator)sender).ErrorMessage = "Please Enter Controller Serial Number";
                args.IsValid = false;
            }
            else
            {
                Certificate objCert = new Certificate();
                if (objCert.IsCTOedPOECert(strFruId))
                    args.IsValid = true;
                else
                {
                    ((CustomValidator)sender).ErrorMessage = Certificate.NO_FRU_INFO;
                    args.IsValid = false;

                }
            }
        }
    }

    protected void btnImport_OnClick(object sender, EventArgs args)
    {
        if (!Page.IsValid)
            return;

        Certificate objCert = new Certificate();
        if (chkyes.Checked == true)
        {
            if (objCert.ImportCTOedPOECerts(txtFru.Text, ((UserInfo)Session["USER_INFO"]).GetUserAcctId()))
            {
                setError(lblErr, Certificate.SUCCESS_IMPORTCTOCERTS);
                lblErr.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                setError(lblErr, Certificate.FAILURE_IMPORTCTOCERTS);
                lblErr.ForeColor = System.Drawing.Color.Red;
            }

        }
        else
        {
            if (objCert.ImportCTOedCerts(txtCertId.Text, ((UserInfo)Session["USER_INFO"]).GetUserAcctId()))
            {
                setError(lblErr, Certificate.SUCCESS_IMPORTCTOCERTS);
                lblErr.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                setError(lblErr, Certificate.FAILURE_IMPORTCTOCERTS);
                lblErr.ForeColor = System.Drawing.Color.Red;
            }
        }
        
    }
    protected void chkyes_CheckedChanged(object sender, EventArgs e)
    {
        if (chkyes.Checked == true)
        {
            txtCertId.Enabled = false;
            txtFru.Enabled = true;
        }
        else
        {
            txtCertId.Enabled = true;
            txtFru.Enabled = false;
        }
    }
}
