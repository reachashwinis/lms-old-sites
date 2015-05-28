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
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using System.Xml;

public partial class Controls_BulkActivateCert : System.Web.UI.UserControl
{
    const string INVALID_FILE = "Please upload only tab delimited text file. " + "<A href = 'certs.txt' target='_new' >View File Format here.</A> ";
    const string FILE_SIZE = "Uploaded file size should not exceed 1MB";
    int FileSize;
    protected void Page_Load(object sender, EventArgs e)
    {
        LblURL.Text = INVALID_FILE;
    }
    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
       
    }

    protected void BtnUpload_Click(object sender, EventArgs e)
    {
        bool noError = true;
        string filePath = FileUpload.PostedFile.FileName;
        string ext = filePath.Substring(filePath.LastIndexOf('.') + 1).ToLower();
        if (ext != "txt" && ext != "tab")
        {
            string FileType = FileUpload.PostedFile.ContentType;
            if (!FileType.Contains("text/plain") && !FileType.Contains("application/octet-stream"))
            {
                setError(INVALID_FILE, true);
                noError = false;
                return;
            }
        }     
 
        FileSize = FileUpload.PostedFile.ContentLength;
        if (FileSize > Int32.Parse(ConfigurationManager.AppSettings["FILE_SIZE"].ToString()))
        {
            setError(FILE_SIZE, true);
            noError = false;
            return;
        }

        if (noError == true)
        {
            UploadInputFile();
        }
    }

    private void setError(string strError, bool blIsError)
    {
        if (blIsError)
        {
            LblError.ForeColor = System.Drawing.Color.Red;
            LblError.Text = strError;
        }
        else
        {
            LblError.ForeColor = System.Drawing.Color.Green;
            LblError.Text = strError;
        }
    }

        private void UploadInputFile()
        {
            Certificate objCert = new Certificate();
            if (null != FileUpload.PostedFile)
            {
                string strFileName;
                //const int BUFFER_SIZE = 4064;
                int BUFFER_SIZE = Int32.Parse(ConfigurationManager.AppSettings["FILE_SIZE"].ToString());
                int nBytesRead;
                strFileName = FileUpload.PostedFile.FileName;
                string UploadFileName = System.IO.Path.GetFileName(strFileName);
                UserInfo objUserInfo = (UserInfo)Session["USER_INFO"];
                try
                {
                    Byte[] Buffer = new Byte[BUFFER_SIZE];
                    StringBuilder strUploadedContent = new StringBuilder("");
                    Stream uploadStream = FileUpload.PostedFile.InputStream;
                    nBytesRead = uploadStream.Read(Buffer, 0, BUFFER_SIZE);
                    while (0 != nBytesRead)
                    {
                        strUploadedContent.Append(Encoding.Default.GetString(Buffer, 0, nBytesRead));
                        nBytesRead = uploadStream.Read(Buffer, 0, BUFFER_SIZE);
                    }

                    string strUpload = strUploadedContent.ToString();
                    if (!ParseUploadedDoc(strUpload))
                    {
                        setError("Error occurred. ", true);
                    }
                    else
                    {
                        string strUser = objUserInfo.GetUserAcctId().ToString();
                        string strDate = DateTime.Today.Year.ToString() + ((DateTime.Today.Month.ToString().Length > 1) ? DateTime.Today.Month.ToString() : "0" + DateTime.Today.Month.ToString()) + ((DateTime.Today.Day.ToString().Length > 1) ? DateTime.Today.Day.ToString() : "0" + DateTime.Today.Day.ToString());
                        string strTime = ((DateTime.Now.Hour.ToString().Length > 1) ? DateTime.Now.Hour.ToString() : "0" + DateTime.Now.Hour.ToString()) + ((DateTime.Now.Minute.ToString().Length > 1) ? DateTime.Now.Minute.ToString() : "0" + DateTime.Now.Minute.ToString()) + ((DateTime.Now.Second.ToString().Length > 1) ? DateTime.Now.Second.ToString() : "0" + DateTime.Now.Second.ToString());
                        string FileName = strUser + "_" + strDate + strTime + "_" + Session["BRAND"].ToString() + ".txt";
                        FileUpload.PostedFile.SaveAs(ConfigurationManager.AppSettings["UPLOAD_FILE"].ToString() + FileName);
                        objCert.AddUploadInfo(UploadFileName, FileName, FileSize,Int32.Parse(strUser));
                        setError("System is processing your uploaded file. You will receive an email shortly.", false);
                        uploadStream.Dispose(); ;
                        uploadStream.Close();
                    }

                }
                catch (Exception e)
                {
                    new Log().logException("UploadInputFile", e);
                    setError("The application could not process your file. Please check the file format and try again.", true);
                }
            }
        }

    private bool ParseUploadedDoc(string strUpload)
    {
        bool blresult = true;
        strUpload = strUpload.Replace("\r","");
        char [] delimitedchars = {'\n'};
        string[] splitwords = strUpload.Split(delimitedchars);
        string[] column; 
        StringBuilder InvalidCert = new StringBuilder(); 
        StringBuilder InvalidSerial = new StringBuilder();
        foreach (string word in splitwords)
        {
            column = word.Split('\t');
            if (column[0].Length != 35)
            {
                InvalidCert.Append(column[0].ToString());
                InvalidCert.Append(", ");
                blresult = false;
            }
            if (column[1].Length != 9)
            {
                InvalidSerial.Append(column[1].ToString());
                InvalidSerial.Append(", ");
                blresult = false;
            }
        }
        if (blresult == false)
        {
            string strErrCert = "Invalid Certificate Id(s): " + InvalidCert.ToString();
            strErrCert = strErrCert.Substring(0, strErrCert.Length - 2);
            LblInvalidCert.Text = strErrCert;
            string strErrFru = "Invalid Serial Number(s): " + InvalidSerial.ToString();
            strErrFru = strErrFru.Substring(0, strErrFru.Length - 2);
            LblInvalidFru.Text = strErrFru;
        }
        return blresult;
    }

 }
