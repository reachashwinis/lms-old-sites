using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;

/// <summary>
/// Summary description for DatasettoExcel
/// </summary>
public class DatasettoExcel
{
    public DatasettoExcel()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void Convert(System.Data.DataSet ds, System.Web.HttpResponse response)
    {
        //first let's clean up the response.object
        response.Clear();
        response.Charset = "";
        //set the response mime type for excel
        response.ContentType = "application/vnd.ms-excel";
        response.AddHeader("content-disposition", "attachment; filename=Download" + System.DateTime.Now.ToString("yyyyMMddHHMMss") + ".xls");
        //create a string writer
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        //create an htmltextwriter which uses the stringwriter
        System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
        //instantiate a datagrid
        System.Web.UI.WebControls.DataGrid dg = new System.Web.UI.WebControls.DataGrid();
        //set the datagrid datasource to the dataset passed in
        dg.DataSource = ds.Tables[0];
        //bind the datagrid
        dg.DataBind();
        //tell the datagrid to render itself to our htmltextwriter
        dg.RenderControl(htmlWrite);
        //all that's left is to output the html
        response.Write(stringWrite.ToString());
        response.End();
    }
    public static void ConvertXML(System.Data.DataSet ds, System.Web.HttpResponse response)
    {
        //first let's clean up the response.object
        response.Clear();
        response.Charset = "";
        //set the response mime type for excel
        response.ContentType = "application/text";
        response.AddHeader("Content-Disposition", "attachment; filename=Download" + System.DateTime.Now.ToString("yyyyMMddHHMMss")+ ".xml");
        //create a string writer
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();

        //assume that all Datasets have data only in the tables[0] we will remove all other tables.
        for (int tableIndex = 1;tableIndex <= ds.Tables.Count - 1; tableIndex++ )
        {
            ds.Tables.RemoveAt(tableIndex);
        }
        
        ds.WriteXml(stringWrite,XmlWriteMode.IgnoreSchema);

        //all that's left is to output the html
        if (response.IsClientConnected)
        {
            response.Write(stringWrite.ToString());
        }
        response.End();
    }
    public static void Convert(System.Data.DataSet ds, int TableIndex, System.Web.HttpResponse response)
    {
        //lets make sure a table actually exists at the passed in value
        //if it is not call the base method
        if (TableIndex > ds.Tables.Count - 1)
        {
            Convert(ds, response);
        }
        //we've got a good table so
        //let's clean up the response.object
        response.Clear();
        response.Charset = "";
        //set the response mime type for excel
        response.ContentType = "application/vnd.ms-excel";
        //create a string writer
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        //create an htmltextwriter which uses the stringwriter
        System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
        //instantiate a datagrid
        System.Web.UI.WebControls.DataGrid dg = new System.Web.UI.WebControls.DataGrid();
        //set the datagrid datasource to the dataset passed in
        dg.DataSource = ds.Tables[TableIndex];
        //bind the datagrid
        dg.DataBind();
        //tell the datagrid to render itself to our htmltextwriter
        dg.RenderControl(htmlWrite);
        //all that's left is to output the html
        response.Write(stringWrite.ToString());
        response.End();
    }

    public static void Convert(System.Data.DataSet ds, string TableName, System.Web.HttpResponse response)
    {
        //let's make sure the table name exists
        //if it does not then call the default method
        if (ds.Tables[TableName] == null)
        {
            Convert(ds, response);
        }
        //we've got a good table so
        //let's clean up the response.object
        response.Clear();
        response.Charset = "";
        //set the response mime type for excel
        response.ContentType = "application/vnd.ms-excel";
        //create a string writer
        System.IO.StringWriter stringWrite = new System.IO.StringWriter();
        //create an htmltextwriter which uses the stringwriter
        System.Web.UI.HtmlTextWriter htmlWrite = new System.Web.UI.HtmlTextWriter(stringWrite);
        //instantiate a datagrid
        System.Web.UI.WebControls.DataGrid dg = new System.Web.UI.WebControls.DataGrid();
        //set the datagrid datasource to the dataset passed in
        dg.DataSource = ds.Tables[TableName];
        //bind the datagrid
        dg.DataBind();
        //tell the datagrid to render itself to our htmltextwriter
        dg.RenderControl(htmlWrite);
        //all that's left is to output the html
        response.Write(stringWrite.ToString());
        response.End();
    }

    public static void ExportToTextFile(DataSet GridData, string strFormat, string UserEmail, System.Web.HttpResponse response)
    {
            StringBuilder downloadFile = new StringBuilder();
            string strPath = UserEmail + "_Rep";
            response.Clear();
            response.ContentType = "application/octet-stream";
            response.AppendHeader("Content-Disposition", "attachment; filename=" + strPath + "");            
            response.Charset = "";
            foreach (DataColumn column in GridData.Tables[0].Columns)
            {
                downloadFile.Append("\"" + column.ColumnName.ToString() + "\"\t");
            }
            downloadFile.Append("\r\n");

            foreach (DataRow dr in GridData.Tables[0].Rows)
            {
                foreach (Object Field in dr.ItemArray)
                {
                    downloadFile.Append("\"" + Field.ToString() + "\"\t");
                }
                downloadFile.Append("\r\n");
                //downloadFile.Replace("\t", vbNewLine, downloadFile.Length - 1, 1);
            }

            //File.WriteAllText(strPath, downloadFile.ToString());
            downloadFile = downloadFile.Replace("\"", "");
            response.Write(downloadFile.ToString());
            response.End();

    }
}
