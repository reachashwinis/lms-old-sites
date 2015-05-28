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

public partial class Controls_ClpSubscriptionDet : System.Web.UI.UserControl
{
    Certificate objCert;
    DataSet dsDetails = new DataSet();
    string urlDirection = string.Empty;
    Email objEmail = new Email();

    protected void Page_Load(object sender, EventArgs e)
    {
        urlDirection = Request.QueryString["key"];
        objCert = new Certificate();
        LblCaption.Text = "Subscription Details ";
        string subscription = Session["subscription"].ToString();
        GetDetails(subscription,"1");
    }

    private void GetDetails(string subscription,string p)
    {
        Subscription sub = objCert.GetDetailsAmigopodSubscription(subscription,p);

        if (sub != null)
        {

            try
            {
                tblUpgrade.Rows[0].Cells[2].Text = subscription;
                tblUpgrade.Rows[1].Cells[2].Text = sub.po;
                tblUpgrade.Rows[2].Cells[2].Text = sub.so;
                if (sub.expiretime != null && sub.expiretime != "")
                    //tblUpgrade.Rows[3].Cells[2].Text = (new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(Convert.ToDouble(sub.expiretime))).ToShortDateString();
                tblUpgrade.Rows[3].Cells[2].Text = sub.expiretime.ToString();
                tblUpgrade.Rows[4].Cells[2].Text = sub.license;
                tblUpgrade.Rows[5].Cells[2].Text = sub.name;
                tblUpgrade.Rows[6].Cells[2].Text = sub.username;
                tblUpgrade.Rows[7].Cells[2].Text = sub.password;
                tblUpgrade.Rows[8].Cells[2].Text = sub.email;
                //tblUpgrade.Rows[9].Cells[2].Text = sub.sms_credit;
                //tblUpgrade.Rows[10].Cells[2].Text = sub.sms_handler;
                tblUpgrade.Rows[9].Cells[2].Text = sub.create_time;
                //tblUpgrade.Rows[13].Cells[2].Text = sub.on_board_license;
                //tblUpgrade.Rows[14].Cells[2].Text = sub.adv_feature;

                //string ha_sub_key = string.Empty;
                //ha_sub_key = GetHASubscription(subscription);
                //if (!(ha_sub_key.Equals(string.Empty)))
                //{
                //    tblUpgrade.Rows[12].Visible = true;
                //    tblUpgrade.Rows[12].Cells[2].Text = ha_sub_key;
                //}
                //else
                //{
                //    tblUpgrade.Rows[12].Visible = false;
                //    tblUpgrade.Rows[12].Cells[2].Text = "";
                //}
            }
            catch (Exception ex)
            {
                new Log().logException("GetClpSubscriptionDetails", ex);
                objEmail.sendAmigopodErrorMessage(ex.Message, "GetClpSubscriptionDetails", "", subscription);
            }
        }
    }
    protected void lnkBtnBack_Click(object sender, EventArgs e)
    {
        if (urlDirection == "all")
            Response.Redirect("~/Pages/AllClearPassCerts.aspx");
        else
            Response.Redirect("~/Pages/MyClearPassCerts.aspx");
    }
    //private string GetHASubscription(string subscription)
    //{
    //    string ha_sbu_key = string.Empty;
    //    string type = ConfigurationManager.AppSettings["CLEARPASS_HA"].ToString();
    //    ha_sbu_key = objCert.GetHaSubscription(subscription,type);
    //    return ha_sbu_key;
    //}
}

