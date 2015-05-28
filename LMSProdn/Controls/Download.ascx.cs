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
using Com.Arubanetworks.Licensing.Dataaccesslayer;
using Com.Arubanetworks.Licensing.Lib.Data;
using Com.Arubanetworks.Licensing.Lib.Utils;
using System.IO;
using System.Globalization;

public partial class Controls_Download : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataSet dsDoc = new DataSet();
            string strFile = ConfigurationManager.AppSettings["DOWNLOAD_PATH"].ToString() + ConfigurationManager.AppSettings["XML_FILE"].ToString();
            dsDoc.ReadXml(strFile);
            GrdDownload.DataSource = dsDoc.Tables[0].DefaultView;
            GrdDownload.DataBind();
        }
    }

    protected void GotoSystem_OnCommand(object sender, EventArgs args)
    {
        string strSource = string.Empty;
        try
        {
            UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
            string strDocname = ((LinkButton)sender).CommandArgument.ToString();
            strDocname = strDocname.Replace("\r\n", "");
            strSource = ConfigurationManager.AppSettings["DOWNLOAD_PATH"].ToString() + Path.GetFileName(strDocname);
            new Log().logInfo("GotoSystem_onCommand", strSource);
            FileInfo file = new FileInfo(strSource);
            //strSource = Server.MapPath(Path.GetFileName(strDocname));
            //new Log().logInfo("GotoSystem_onCommand", strSource);
            //Response.Write(strSource);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/octet-stream";
            //Response.AppendHeader("Content-Disposition", "attachment; filename=AMP-6.4.7.zip");
            Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + strDocname + "");
            Response.TransmitFile(strSource);
            UpdateDownloadLog(objUserInfo.GetUserAcctId(), strDocname, Session["BRAND"].ToString());
            //Response.Flush();
            //Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            //Response.Close();
        }
        catch (Exception e)
        {
            new Log().logException(sender.ToString(), e);
            Response.Redirect(ConfigurationManager.AppSettings["ERROR_URL"], true);
        }
    }

    protected void Page_Error(object sender, EventArgs args)
    {
        Exception objExcep = Server.GetLastError();
        Server.ClearError();
        new Log().logException(sender.ToString(), objExcep);
        Response.Redirect(ConfigurationManager.AppSettings["ERROR_URL"], true);
    }

    private bool UpdateDownloadLog(int acctId,  string strFileName, string brand)
    {
        Certificate objCert = new Certificate();
        bool blResult = objCert.UpdateDownloadLog(acctId, strFileName,brand);
        return blResult;
    }
}
