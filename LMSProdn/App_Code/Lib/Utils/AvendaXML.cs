using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for ClearPassXML
/// </summary>
public class AvendaXML
{
    public string email = string.Empty;
    public string password = string.Empty;
    public string message = string.Empty;
    public string warning = string.Empty;
    public string subscription_key = string.Empty;
    public string xmlparse_error = string.Empty;
    public string error = string.Empty;
    public string user_name = string.Empty;
    public string license = string.Empty;
	public AvendaXML()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}
