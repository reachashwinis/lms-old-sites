using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;


/// <summary>
/// Summary description for Subscription
/// </summary>
public class Subscription
{
    public string create_time;
    public string email;
    public string error;
    public string expiretime;
    public string license;
    public string message;
    public string name;
    public string password;
    public string po;
    public string sms_credit;
    public string sms_handler;
    public string so;
    public string subscription_key;
    public string username;
    public string warning;
    public string xmlparse_error;
    public string high_availability;
    public string on_board_license;
    public string adv_feature;
    public string category;
    public string sms_count;
    public string high_avaialbility_key;
    public string sku_id;
    public string cust_id;
    //public clsLicense[] ArrclsLicense = new clsLicense[10];
    public List<clsLicense> lstClsLicense = new List<clsLicense>();

	public Subscription()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public string getString()
    {
        return "subscription_key:" + subscription_key + " user_count:" + license;
    }
}

public class clsLicense
{
    public string part_id = string.Empty;
    public string license_key = string.Empty;
    public string created_date = string.Empty;
    public string expiry_date = string.Empty;
    public string version = string.Empty;
    public string orig_sku = string.Empty;
    public string sku_id = string.Empty;
    public string subscription_key = string.Empty;
    public string so_id = string.Empty;
    public string po_id = string.Empty;
    public string cust_name = string.Empty;
    public string num_users = string.Empty;
    public string part_desc = string.Empty;
}
