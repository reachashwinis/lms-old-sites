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
using System.Reflection;


public partial class Controls_ImportMyLegacyClpLic : System.Web.UI.UserControl
{
    Email objemail = new Email();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void BtnNext1_Click(object sender, EventArgs e)
    {
        Certificate objCert = new Certificate();
        DataTable dt = new DataTable();
        UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
        bool blError = false;
        try
        {
            //if (!objCert.isValidClpSubscription(TxtSubKey1.Text.Trim()))
            //{
            if (!objCert.IsLegacySubscription(TxtSubKey1.Text.Trim()))
            {
                LblError1.Text = "The Subscription ID is not valid.";
                WizImportlegacySub.ActiveStepIndex = 0;
                LblError1.Visible = true;
                return;
            }
            else if (objCert.IsEvaluationSubKey(TxtSubKey1.Text.Trim()))
            {
                LblError1.Text = "You can not import this Subscription ID as this is of evaluation type.";
                WizImportlegacySub.ActiveStepIndex = 0;
                LblError1.Visible = true;
                return;
            }
            else if (objCert.IsAmigopodSubscription(TxtSubKey1.Text.Trim()))
            {
                LblError1.Text = "You can not import this Subscription ID as this is of Legacy ClearPass type. Please Use <i> My Clearpass (Legacy                       guest)</i> to import this Subscription ID to LMS.";
                WizImportlegacySub.ActiveStepIndex = 0;
                LblError1.Visible = true;
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                dt = objCert.BuildAmgCertInfo();
                DataRow dr = dt.NewRow();
                Subscription objSub = objCert.GetDetailsAmigopodSubscription(TxtSubKey1.Text.Trim(), "1");

                if (objSub.so != TxtSoId1.Text.Trim())
                {
                    LblError1.Text = "This Subscription ID is not generated for Sales Order ID " + TxtSoId1.Text.Trim() + ". Please enter the correct Sales Order";
                    WizImportlegacySub.ActiveStepIndex = 0;
                    LblError1.Visible = true;
                    return;
                }

                //if (objSub.so == "" || objSub.so == "0")
                //{
                //    objSub.so = "55555";
                //}
                if (objSub.po == "" || objSub.po == "0")
                {
                    objSub.po = "Lgcy4444";
                }

                if (objSub.expiretime == "0")
                {
                    objSub.expiretime = string.Empty;
                }
                if (objSub.create_time == "0" || objSub.create_time == "0000-00-00 00:00:00")
                {
                    objSub.create_time = string.Empty;
                }

                dr["SubscriptionKey"] = objSub.subscription_key;
                dr["Organization"] = objSub.name;
                dr["po_id"] = objSub.po;
                dr["so_id"] = objSub.so;
                dr["license_count"] = objSub.license;
                dr["onBoard_count"] = objSub.on_board_license;
                dr["expire_time"] = objSub.expiretime;
                dr["user_name"] = objSub.username;
                dr["password"] = objSub.password;
                dr["sms_credit"] = objSub.sms_credit;
                dr["sms_handler"] = objSub.sms_handler;
                dr["email"] = objSub.email;
                dr["create_time"] = objSub.create_time;
                dr["HA"] = objSub.high_availability;
                dr["advertising"] = objSub.adv_feature;
                dr["category"] = objSub.category;
                dr["sms_count"] = objSub.sms_count;
                if (objSub.subscription_key != objSub.high_avaialbility_key)
                {
                    dr["HASubscriptionKey"] = objSub.high_avaialbility_key;
                }
                if (dr["expire_time"].ToString() != string.Empty)
                {
                    if (Convert.ToDateTime(dr["expire_time"].ToString()) <= System.DateTime.Today)
                    {
                        dr["Error"] = "This Subscription ID has crossed the expiry date of support. To extend the expiration, please contact your local Aruba Networks representative, or contact Aruba Support at <A>support@arubanetworks.com </A>";
                        blError = false;
                    }
                }
                dt.Rows.Add(dr);

                DataTable dtClp = objCert.BuildClpCertInfo();
                for (int indexJ = 0; indexJ < objSub.lstClsLicense.Count; indexJ++)
                {
                    DataRow drClp = dtClp.NewRow();
                    if (objSub.lstClsLicense[indexJ].expiry_date == "0")
                    {
                        objSub.lstClsLicense[indexJ].expiry_date = string.Empty;
                    }
                    if (objSub.lstClsLicense[indexJ].created_date == "0" || objSub.lstClsLicense[indexJ].created_date == "0000-00-00 00:00:00")
                    {
                        objSub.lstClsLicense[indexJ].created_date = string.Empty;
                    }
                    drClp["part_id"] = objSub.lstClsLicense[indexJ].part_id;
                    drClp["part_desc"] = objSub.lstClsLicense[indexJ].part_desc;
                    drClp["expire_time"] = objSub.lstClsLicense[indexJ].expiry_date;
                    drClp["so_id"] = objSub.so;
                    drClp["po_id"] = objSub.po;
                    drClp["create_time"] = objSub.lstClsLicense[indexJ].created_date;
                    drClp["version"] = objSub.lstClsLicense[indexJ].version;
                    drClp["LicenseKey"] = objSub.lstClsLicense[indexJ].license_key;
                    drClp["subscription_key"] = objSub.subscription_key;
                    drClp["cust_name"] = objSub.name;
                    drClp["num_users"] = objSub.lstClsLicense[indexJ].num_users;
                    drClp["user_name"] = objSub.username;
                    drClp["password"] = objSub.password;

                    if (objCert.IsClpLicenseImported(drClp["LicenseKey"].ToString(), drClp["part_id"].ToString()))
                    {
                        drClp["IsImported"] = "yes";
                    }
                    else
                    {
                        drClp["IsImported"] = "no";
                    }

                    if (objSub.lstClsLicense[indexJ].version != string.Empty)
                    {
                        dtClp.Rows.Add(drClp);
                    }
                }

                ds.Tables.Add(dt);
                ds.Tables.Add(dtClp);

                if (blError == true)
                {
                    LblError1.Text = "Error is occurred.";
                    WizImportlegacySub.ActiveStepIndex = 0;
                    LblError1.Visible = true;
                }
                else
                {
                    LoadWizStep2(ds);
                }
            }
            //}
            //else
            //{
            //    LblError1.Text = "This subscription ID already exists!";
            //    WizImportlegacySub.ActiveStepIndex = 0;
            //    LblError1.Visible = true;
            //    return;
            //}
        }
        catch (Exception ex)
        {
            new Log().logException("ImportSubscription", ex);
            LblError1.Text = "Application could not process your request.The error occurred has been logged";
            LblError1.Visible = true;
            objemail.sendAmigopodErrorMessage(ex.Message, "ImportSubscription", "", TxtSubKey1.Text);
        }
    }

    private void LoadWizStep2(DataSet ds)
    {
        WizImportlegacySub.ActiveStepIndex = 1;
        rptSub2.DataSource = ds.Tables[0];
        rptSub2.DataBind();
        rptSub3.DataSource = ds.Tables[1];
        rptSub3.DataBind();
        Session["AMG_CERT"] = ds;
    }

    protected void BtnImport2_Click(object sender, EventArgs e)
    {
        try
        {
            Certificate objCert = new Certificate();
            DataSet ds = (DataSet)Session["AMG_CERT"];
            UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
            int AcctId = objUserInfo.GetUserAcctId();
            int ImpAcctId = objUserInfo.AcctId;
            DataTable dtResult = new DataTable();
            Email objEmail = new Email();
            DataTable dtSubResult = new DataTable();
            DataSet dsResult = new DataSet();

            if (!objCert.IsAvendaSubKeyImported(TxtSubKey1.Text.Trim()))
            {
                dtSubResult = objCert.ImportLegacyAvendaSubKey(ds.Tables[0], AcctId, ImpAcctId);
            }

            if (ds.Tables[1].Rows.Count > 0)
            {
                dtResult = objCert.ImportLegacyAvendaLicKey(ds.Tables[1], AcctId, ImpAcctId);
            }

            dsResult.Tables.Add(dtSubResult);
            dsResult.Tables.Add(dtResult);

            AmigopodCertInfo objAmigopodCertinfo = new AmigopodCertInfo();
            if (dtSubResult.Rows.Count > 0)
            {
                // send mail confirmation                
                for (int index = 0; index < dtSubResult.Rows.Count; index++)
                {
                    objAmigopodCertinfo.Subscriptionkey = dtSubResult.Rows[index]["SubscriptionKey"].ToString();
                    objAmigopodCertinfo.CertId = dtSubResult.Rows[index]["serial_number"].ToString();
                    objAmigopodCertinfo.SerialNumber = dtSubResult.Rows[index]["Lserial_number"].ToString();
                    objAmigopodCertinfo.SoId = dtSubResult.Rows[index]["so_id"].ToString();
                    objAmigopodCertinfo.PoId = dtSubResult.Rows[index]["po_id"].ToString();
                    objAmigopodCertinfo.UserName = dtSubResult.Rows[index]["user_name"].ToString();
                    objAmigopodCertinfo.Password = dtSubResult.Rows[index]["password"].ToString();
                    objAmigopodCertinfo.ExpiryDate = Convert.ToDateTime(dtSubResult.Rows[index]["expire_time"].ToString());
                    objAmigopodCertinfo.Brand = Session["BRAND"].ToString().ToUpper();
                    objAmigopodCertinfo.Email = objUserInfo.GetUserEmail();
                    objAmigopodCertinfo.Name = objUserInfo.FirstName + " " + objUserInfo.LastName;
                    if (dtResult.Rows.Count > 0)
                    {
                        for (int indexJ = 0; indexJ < dtResult.Rows.Count; indexJ++)
                        {
                            clpCertInfo objclpCertInfo = new clpCertInfo();
                            objAmigopodCertinfo.lstClsLicense.Add(objclpCertInfo);
                            objAmigopodCertinfo.lstClsLicense[indexJ].clpCertId = dtResult.Rows[indexJ]["serial_number"].ToString();
                            objAmigopodCertinfo.lstClsLicense[indexJ].clpCertSerialNo = dtResult.Rows[indexJ]["Lserial_number"].ToString();
                            objAmigopodCertinfo.lstClsLicense[indexJ].clpLicKey = dtResult.Rows[indexJ]["licensekey"].ToString();
                            objAmigopodCertinfo.lstClsLicense[indexJ].clpPartId = dtResult.Rows[indexJ]["part_id"].ToString();
                            objAmigopodCertinfo.lstClsLicense[indexJ].clpCreatedDate = dtResult.Rows[indexJ]["create_time"].ToString();
                            objAmigopodCertinfo.lstClsLicense[indexJ].clpExpiryDate = dtResult.Rows[indexJ]["expire_time"].ToString();
                            objAmigopodCertinfo.lstClsLicense[indexJ].clpVersion = dtResult.Rows[indexJ]["version"].ToString();
                            objAmigopodCertinfo.lstClsLicense[indexJ].clpPartDesc = dtResult.Rows[indexJ]["part_desc"].ToString();
                        }
                        objEmail.sendAvendaImportSubandLicKeyDet(objAmigopodCertinfo);
                    }
                    else
                    {
                        //Need to change this code later..
                        AvendaCert objAvendaCert = new AvendaCert();
                        objAvendaCert.subscription = objAmigopodCertinfo.Subscriptionkey;
                        objAvendaCert.so = objAmigopodCertinfo.SoId;
                        objAvendaCert.po = objAmigopodCertinfo.PoId;
                        objAvendaCert.CertId = objAmigopodCertinfo.CertId;
                        objAvendaCert.SerialNo = objAmigopodCertinfo.SerialNumber;
                        objAvendaCert.brand = objAmigopodCertinfo.Brand.ToUpper();
                        objAvendaCert.email = objAmigopodCertinfo.Email;
                        objAvendaCert.name = objAmigopodCertinfo.Name;
                        objAvendaCert.username = objAmigopodCertinfo.UserName;
                        objAvendaCert.password = objAmigopodCertinfo.Password;
                        objAvendaCert.expiryDate = objAmigopodCertinfo.ExpiryDate.ToString();
                        objEmail.sendAvendaImportSubKeyDet(objAvendaCert);
                    }
                }
                LoadWizStep3(dsResult);
            }
            else if (dtSubResult.Rows.Count <= 0 && dtResult.Rows.Count > 0)
            {
                for (int indexJ = 0; indexJ < dtResult.Rows.Count; indexJ++)
                {
                    clpCertInfo objclpCertInfo = new clpCertInfo();
                    objAmigopodCertinfo.lstClsLicense.Add(objclpCertInfo);
                    objAmigopodCertinfo.lstClsLicense[indexJ].clpCertId = dtResult.Rows[indexJ]["serial_number"].ToString();
                    objAmigopodCertinfo.lstClsLicense[indexJ].clpCertSerialNo = dtResult.Rows[indexJ]["Lserial_number"].ToString();
                    objAmigopodCertinfo.lstClsLicense[indexJ].clpLicKey = dtResult.Rows[indexJ]["licensekey"].ToString();
                    objAmigopodCertinfo.lstClsLicense[indexJ].clpPartId = dtResult.Rows[indexJ]["part_id"].ToString();
                    objAmigopodCertinfo.lstClsLicense[indexJ].clpCreatedDate = dtResult.Rows[indexJ]["create_time"].ToString();
                    objAmigopodCertinfo.lstClsLicense[indexJ].clpExpiryDate = dtResult.Rows[indexJ]["expire_time"].ToString();
                    objAmigopodCertinfo.lstClsLicense[indexJ].clpVersion = dtResult.Rows[indexJ]["version"].ToString();
                    objAmigopodCertinfo.lstClsLicense[indexJ].subscription_key = dtResult.Rows[indexJ]["subscription_key"].ToString();
                    objAmigopodCertinfo.lstClsLicense[indexJ].clpPartDesc = dtResult.Rows[indexJ]["part_desc"].ToString();
                }
                objAmigopodCertinfo.Brand = Session["BRAND"].ToString().ToUpper();
                objAmigopodCertinfo.Email = objUserInfo.GetUserEmail();
                objAmigopodCertinfo.Name = objUserInfo.FirstName + " " + objUserInfo.LastName;
                objEmail.sendAvendaImportLicKeyDet(objAmigopodCertinfo);
                LoadWizStep3(dsResult);
            }
            else
            {
                WizImportlegacySub.ActiveStepIndex = 1;
                LblError2.Text = "This Subscription ID" + TxtSubKey1.Text.Trim() + " and its licenses are already imported to LMS.";
                LblError2.Visible = true;
                objemail.sendAmigopodErrorMessage("Error in Importing Subscription " + TxtSubKey1.Text.Trim(), "ImportClpLicSubscription", "", TxtSubKey1.Text);
            }
        }
        catch (Exception ex)
        {
            new Log().logException("ImportClpLicSubscription", ex);
            WizImportlegacySub.ActiveStepIndex = 0;
            LblError1.Text = "Application could not process your request.The error occurred has been logged";
            LblError1.Visible = true;
            objemail.sendAmigopodErrorMessage(ex.Message, "ImportClpLicSubscription", "", TxtSubKey1.Text);
        }
    }

    private void LoadWizStep3(DataSet ds)
    {
        WizImportlegacySub.ActiveStepIndex = 2;
        rptSub4.DataSource = ds.Tables[0];
        rptSub4.DataBind();
        rptSub5.DataSource = ds.Tables[1];
        rptSub5.DataBind();
    }

    protected void rptSub3_PreRender(object sender, EventArgs e)
    {
        if (rptSub3.Items.Count == 0)
        {
            rptSub3.Visible = false;
            lblNoRecords1.Visible = true;
        }
        else
        {
            rptSub3.Visible = true;
            lblNoRecords1.Visible = false;
        }
    }


    protected void rptSub5_PreRender(object sender, EventArgs e)
    {
        if (rptSub5.Items.Count == 0)
        {
            rptSub5.Visible = false;
            lblNoRecords2.Visible = true;
        }
        else
        {
            rptSub5.Visible = true;
            lblNoRecords2.Visible = false;
        }
    }

    protected void rptSub3_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            HtmlTableRow row = (HtmlTableRow)e.Item.FindControl("TR");
            DataRowView datarow = (DataRowView)e.Item.DataItem;
            if (datarow != null)
            {
                if (datarow.Row["IsImported"].ToString() == "yes")
                {
                    row.Attributes.Add("style", "color:Red;");
                }
            }
        }
    }
}

