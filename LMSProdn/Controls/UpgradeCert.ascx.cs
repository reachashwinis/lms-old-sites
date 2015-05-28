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
using Com.Arubanetworks.Licensing.Dataaccesslayer;
using System.Drawing;
using System.Web.Caching;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Data.OleDb;

public partial class Controls_UpgradeCert : System.Web.UI.UserControl
{
    string StrIfile = string.Empty;
    string strFru = string.Empty;
    string strOfile = string.Empty;
    string strMarg = string.Empty;
    string errorInfo = String.Empty;
    string strIDir = string.Empty;
    string strODir = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private bool LoadTextFile(string textFilePath, DataSet dataToLoad, out string errorInfo)
    {
        errorInfo = String.Empty;

        try
        {
            string textFileFolder = (new System.IO.FileInfo(textFilePath)).DirectoryName;
            string textConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                                            "Data Source=" + textFileFolder + ";" +
                                            "Extended Properties=\"text;\";";
            //from using System.Data.OleDb:
            OleDbConnection textConnection = new OleDbConnection(textConnectionString);

            textConnection.Open();

            textFilePath = (new System.IO.FileInfo(textFilePath)).Name;
            textFilePath = textFilePath.Substring(0, 9);
            string selectCommand = "select * from " + textFilePath ;

            //open command:
            OleDbCommand textOpenCommand = new OleDbCommand(selectCommand);
            textOpenCommand.Connection = textConnection;

            OleDbDataAdapter textDataAdapter = new OleDbDataAdapter(textOpenCommand);

            int rows = textDataAdapter.Fill(dataToLoad);

            textConnection.Close();
            textConnection.Dispose();

            bool blResult = LoadDataToDB(dataToLoad);
            if (blResult == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex_load_text_file)
        {
            errorInfo = ex_load_text_file.Message;
            return false;
        }
    }

    protected void BtnSubmit_Click(object sender, EventArgs e)
    {
        Certificate objCert = new Certificate();
        strFru = TxtCtrl.Text;
        strFru = strFru.TrimEnd();
        strFru = strFru.TrimStart();
        strIDir = "UpgradeIFile/";
        strODir = "UpgradeOFile/";
        FtpWebRequest request;
        FtpWebResponse response;
        DataSet dsActInfo = objCert.GetCertsByFru(strFru, Session["BRAND"].ToString());
        if (dsActInfo == null || dsActInfo.Tables[0].Rows.Count <= 0)
        {
            LblError.Text = "No License keys associated to this Controller";
            return;
        }
        string[] rowString = new string[dsActInfo.Tables[0].Rows.Count];
        int i = 0;
        foreach (DataRow dr in dsActInfo.Tables[0].Rows)
        {
            rowString[i] = dr.ItemArray.GetValue(0).ToString() + "\t";
            rowString[i] += dr.ItemArray.GetValue(1).ToString();
            i++;
        }

        strMarg = dsActInfo.Tables[1].Rows[0]["marg"].ToString();
        string filename = "D:/UpgData/" + strIDir + strFru + "_" + strMarg + ".txt";
        System.IO.File.WriteAllLines(filename, rowString);

        //FTP 
        StrIfile =  strFru + "_" + strMarg + ".txt";
        strOfile = strFru + "_" + strMarg + ".txt";
        request = (FtpWebRequest)FtpWebRequest.Create("ftp://10.6.9.17/offline_quote_tool/" + strIDir + StrIfile);
        request.Method = WebRequestMethods.Ftp.UploadFile;
        request.Credentials = new NetworkCredential("jtran-server\\ftpuser", "Confident*9");
        request.UsePassive = true;
        request.UseBinary = false;
        request.KeepAlive = false;

        StreamReader objReader = new StreamReader(filename);
        byte[] fileContents = File.ReadAllBytes(filename);
        objReader.Close();
        request.ContentLength = fileContents.Length;

        Stream objWriter = request.GetRequestStream();
        objWriter.Write(fileContents, 0, fileContents.Length);
        objWriter.Close();
        response = (FtpWebResponse)request.GetResponse();
        Response.Write(response.StatusDescription);

        UpgradeCertInput objKeyGenUpIp = new UpgradeCertInput(Session["BRAND"].ToString(), StrIfile, strFru, strOfile, strMarg);

        string strStatus = objCert.GenerateUpgradeCerts(objKeyGenUpIp);
        if (strStatus != string.Empty)
        {
            //FileStream outputStream = new FileStream("D:/UpgData/" + "\\" + strOfile, FileMode.Create);
            FileStream outputStream = new FileStream("D:/UpgData/" + strODir + "\\" + strOfile, FileMode.Create);
            request = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://10.6.9.17/offline_quote_tool/" + strODir + strOfile));
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.UseBinary = false;
            request.Credentials = new NetworkCredential("jtran-server\\ftpuser", "Confident*9");
            response = (FtpWebResponse)request.GetResponse();
            Stream ftpStream = response.GetResponseStream();
            long cl = response.ContentLength;
            int bufferSize = 2048;
            int readCount;
            byte[] buffer = new byte[bufferSize];
            readCount = ftpStream.Read(buffer, 0, bufferSize);
            while (readCount > 0)
            {
                outputStream.Write(buffer, 0, readCount);
                readCount = ftpStream.Read(buffer, 0, bufferSize);
            }

            ftpStream.Close();
            outputStream.Close();
            response.Close();
        }

        ////open text file into Dataset:
        //string textFilePath = @"D:/UpgData/" + "\\" + strOfile;

        //DataSet dataTextFile = new DataSet("textfile");
        //if (!LoadTextFile(textFilePath, dataTextFile, out errorInfo))
        //{
        //    LblError.Text = "Failed to load text file:\n" + errorInfo;
        //    return;
        //}
        //else
        //{
        //    LblError.Text = "File Loaded:\nTables:" + dataTextFile.Tables.Count.ToString() + "\nRows:" + dataTextFile.Tables[0].Rows.Count.ToString();
        //}

    }

    public bool LoadDataToDB(DataSet dsData)
    {
        Certificate objCert = new Certificate();
        strFru = TxtCtrl.Text;
        bool blResult = objCert.LoadDataToDB(dsData,strFru,Session["BRAND"].ToString());
        return blResult;
    }

}
 