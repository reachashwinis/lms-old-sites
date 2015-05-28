using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Summary description for Log
/// </summary>
namespace Com.Arubanetworks.Licensing.Lib.Utils
{
    public class Log
    {
        //Error codes.
        internal const string ERROR = "ERROR";
        internal const string INFO = "INFO";
        internal const string DEBUG = "DEBUG";

        private string strSQLConn = string.Empty;

        public Log()
        {
            strSQLConn = ConfigurationManager.ConnectionStrings["LMSDB"].ToString();
        }

        public void logSystemError(string method, string strExceptionMsg)
        {
            Exception ex = new Exception(strExceptionMsg);
            logException(method, ex);
        }
        public void logException(string method, Exception exceptn)
        {
            write(ERROR,method,exceptn);
        }
        public void logInfo(string method, string strInfo)
        {
            write(INFO, method, strInfo);
        }
        public void logDebug(string method, string strDebugInfo)
        {
            write(DEBUG, method, strDebugInfo);
        }

        private void write(string strType, string strMethod, Exception nested)
        {
            string text = string.Empty;
               if (nested != null)
                {
                    text = "Message :" + nested.Message + Environment.NewLine;
                    text += "StackTrace :" + nested.StackTrace;
                }
                write(strType, strMethod, text);      
        }

        private void write(string strType, string strMethod, string  text)
        {

            SqlConnection objconn = new SqlConnection(strSQLConn);

            try
            {

            
                List<SqlParameter> lstParam = new List<SqlParameter>();

                SqlParameter spParam = new SqlParameter("@Method", SqlDbType.Text); spParam.Value = strMethod;
                lstParam.Add(spParam);
                spParam = new SqlParameter("@Exception", SqlDbType.Text); spParam.Value = text;
                lstParam.Add(spParam);
                spParam = new SqlParameter("@ExcepType", SqlDbType.VarChar, 20); spParam.Value = strType;
                lstParam.Add(spParam);
                SqlHelper.ExecuteNonQuery(objconn, CommandType.StoredProcedure, "LMS_addException", lstParam.ToArray());
            }

            catch (Exception e)
            {
                Console.Out.WriteLine("ATTENTION: ERROR IN LOG PROCESSING <" + e + ">");
            }
            finally
            {
               if(objconn.State==ConnectionState.Open)
                   objconn.Close();
            }

            objconn.Dispose();
        }





    }
}