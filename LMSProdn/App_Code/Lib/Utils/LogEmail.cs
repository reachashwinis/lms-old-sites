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


namespace Com.Arubanetworks.Licensing.Lib.Utils
{
    /// <summary>
    /// Summary description for LogEmail
    /// </summary>
    public class LogEmail
    {
        private string strSQLConn = string.Empty;

        public LogEmail()
        {
            strSQLConn = ConfigurationManager.ConnectionStrings["LMSDB"].ToString();
        }

        public void LogMailDetails(String fromName, String fromAdd, String toline, String cc, String bcc,String subj, String htmlmsg, String attachment,String activity)
        {
            write(fromName, fromAdd, toline, cc, bcc,subj, htmlmsg,attachment,activity);
        }

        private void write(String fromName, String fromAdd, String toline, String cc, String bcc, String subj, String htmlmsg, String attachment,String activity)
        {

            SqlConnection objconn = new SqlConnection(strSQLConn);

            try
            {
                List<SqlParameter> lstParam = new List<SqlParameter>();

                SqlParameter spParam = new SqlParameter("@FromUser", SqlDbType.Text); spParam.Value = fromAdd;
                lstParam.Add(spParam);
                spParam = new SqlParameter("@ToUser", SqlDbType.Text); spParam.Value = toline;
                lstParam.Add(spParam);
                spParam = new SqlParameter("@ccUser", SqlDbType.VarChar, 200); spParam.Value = cc;
                lstParam.Add(spParam);
                spParam = new SqlParameter("@bccUser", SqlDbType.VarChar, 200); spParam.Value = bcc;
                lstParam.Add(spParam);
                spParam = new SqlParameter("@Subj", SqlDbType.VarChar, 200); spParam.Value = subj;
                lstParam.Add(spParam);
                spParam = new SqlParameter("@Msg", SqlDbType.VarChar ,4000); spParam.Value = htmlmsg;
                lstParam.Add(spParam);
                spParam = new SqlParameter("@activity", SqlDbType.VarChar, 50); spParam.Value = activity;
                lstParam.Add(spParam);
                SqlHelper.ExecuteNonQuery(objconn, CommandType.StoredProcedure, "LMS_addEmail", lstParam.ToArray());
            }

            catch (Exception e)
            {
                Console.Out.WriteLine("ATTENTION: ERROR IN LOG PROCESSING <" + e + ">");
            }
            finally
            {
                if (objconn.State == ConnectionState.Open)
                    objconn.Close();
            }

            objconn.Dispose();
        }
    }
}
