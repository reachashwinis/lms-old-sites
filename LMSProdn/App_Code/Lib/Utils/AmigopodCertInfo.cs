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
/// Summary description for AmigopodCertInfo
/// </summary>
public class AmigopodCertInfo
{
    public string Email;
    public string CertId;
    public string Subscriptionkey;
    public string Name;
    public string SerialNumber;
    public string Brand;
    public string SoId;
    public string PoId;
    public string PartId;
    public DateTime ExpiryDate;
    public string HASubKey = "High Availability is not avaialble for this Subscription ID";
    public string HACertid;
    public string HACertSerialNo;
    public string guestLicCertId;
    public string guestLicCertSerialNo;
    public string onBoardCertId;
    public string onBoardSerialNo;
    public string AdvCertId;
    public string AdvSerialNo;
    public string UserName;
    public string Password;
    public string PolicyLic;
    public string EnterpriseLic;
    public string CompanyName;
    public string LicenseCount;
    public List<clpCertInfo> lstClsLicense = new List<clpCertInfo>();

    public string getString()
    {
        return "Email:" + Email + " CertId:" + CertId + " SubscriptionKey:" + Subscriptionkey + " Name:" + Name + " SerailaNumber:" + SerialNumber + " Brand:" + Brand + " SoId:" + SoId + " PoId:" + PoId + " ExpiryDate:" + ExpiryDate;
    }
}

public class clpCertInfo
{
    public string clpCertId = string.Empty;
    public string clpPartId = string.Empty;
    public string clpLicKey = string.Empty;
    public string clpCreatedDate = string.Empty;
    public string clpExpiryDate = string.Empty;
    public string clpVersion = string.Empty;
    public string clpCertSerialNo = string.Empty;
    public string subscription_key = string.Empty;
    public string clpPartDesc = string.Empty;
}
