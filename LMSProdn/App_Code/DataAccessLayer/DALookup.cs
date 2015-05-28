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
using Com.Arubanetworks.Licensing.Lib.Utils;
using Com.Arubanetworks.Licensing.Lib.Data;
using System.Collections.Generic;
/// <summary>
/// Summary description for DALookup
/// </summary>
namespace Com.Arubanetworks.Licensing.Dataaccesslayer
{
    public class DALookup
    {
        private string strDbConn = string.Empty;
        public DALookup()
        {
            strDbConn = ConfigurationManager.ConnectionStrings["LMSDB"].ConnectionString;
        }

        public DataSet LoadMenuItems(string strUserId, int usrAcctId, string strBrand)
        {
            DataSet dsMenu = new DataSet();
            SqlConnection objSconn = new SqlConnection(strDbConn);
            SqlParameter spUserRole = new SqlParameter("@RoleTitle", SqlDbType.VarChar, 15);
            SqlParameter spAcctid = new SqlParameter("@AcctId", SqlDbType.Int, 15);
            SqlParameter spBrand = new SqlParameter("@brand", SqlDbType.VarChar, 10);
            spUserRole.Value = strUserId.Trim();
            spAcctid.Value = usrAcctId;
            spBrand.Value = strBrand.ToString();
            dsMenu = SqlHelper.ExecuteDataset(objSconn, CommandType.StoredProcedure, "LMS_GetMenuItems", spUserRole, spAcctid, spBrand);
            dsMenu.Tables[0].TableName = Lookup.MODULES_TBL;
            dsMenu.Tables[1].TableName = Lookup.MENUS_TBL;
            objSconn.Dispose();
            return dsMenu;
        }
        public DataSet LoadLookupValues(string strUserId, string strBrand)
        {
            DataSet dsLookup = new DataSet();
            SqlConnection objSconn = new SqlConnection(strDbConn);
            SqlParameter spUserNameId = new SqlParameter("@UserId", SqlDbType.VarChar, 30);
            SqlParameter spUserNameBrand = new SqlParameter("@brand", SqlDbType.VarChar, 10);
            spUserNameId.IsNullable = false;
            spUserNameId.Value = strUserId;
            spUserNameBrand.Value = strBrand;
            dsLookup = SqlHelper.ExecuteDataset(objSconn, CommandType.StoredProcedure, "LMS_GetLookupValues", spUserNameId, spUserNameBrand);
            dsLookup.Tables[0].TableName = Lookup.LOOKUP_TBL;

            objSconn.Dispose();
            return dsLookup;
        }

        public DataSet LoadCertParts(string PartId)
        {
            DataSet dsLookup = new DataSet();
            SqlConnection objSconn = new SqlConnection(strDbConn);
            SqlParameter spUserName = new SqlParameter("@PartId", SqlDbType.VarChar, 50);
            spUserName.IsNullable = false;
            spUserName.Value = PartId;
            dsLookup = SqlHelper.ExecuteDataset(objSconn, CommandType.StoredProcedure, "LMS_GetCertParts", spUserName);
            dsLookup.Tables[0].TableName = Lookup.CERTPARTS_TBL;

            objSconn.Dispose();
            return dsLookup;
        }

        public DataSet LoadRFPparts()
        {
            DataSet dsLookup = new DataSet();
            SqlConnection objSconn = new SqlConnection(strDbConn);
            dsLookup = SqlHelper.ExecuteDataset(objSconn, CommandType.StoredProcedure, "LMS_GetRFPparts");
            dsLookup.Tables[0].TableName = Lookup.RFPPARTS_TBL;

            objSconn.Dispose();
            return dsLookup;
        }

        public DataSet LoadCertParts(string PartId, string CertCode)
        {
            DataSet dsLookup = new DataSet();
            SqlConnection objSconn = new SqlConnection(strDbConn);
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@PartId", SqlDbType.VarChar, 50);
            spParam.IsNullable = false;
            spParam.Value = PartId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CertCode", SqlDbType.VarChar, 50);
            spParam.IsNullable = false;
            spParam.Value = CertCode;
            lstParam.Add(spParam);


            dsLookup = SqlHelper.ExecuteDataset(objSconn, CommandType.StoredProcedure, "LMS_GetCertParts", lstParam.ToArray());
            dsLookup.Tables[0].TableName = Lookup.CERTPARTS_TBL;

            objSconn.Dispose();
            return dsLookup;
        }

        public DataSet LoadCertPartsQA(string PartId, string CertCode, string version, string strUser)
        {
            DataSet dsLookup = new DataSet();
            SqlConnection objSconn = new SqlConnection(strDbConn);
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@PartId", SqlDbType.VarChar, 50);
            spParam.IsNullable = false;
            spParam.Value = PartId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CertCode", SqlDbType.VarChar, 50);
            spParam.IsNullable = false;
            spParam.Value = CertCode;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@version", SqlDbType.VarChar, 20);
            spParam.IsNullable = false;
            spParam.Value = version;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@User", SqlDbType.VarChar, 150);
            spParam.IsNullable = false;
            spParam.Value = strUser;
            lstParam.Add(spParam);

            dsLookup = SqlHelper.ExecuteDataset(objSconn, CommandType.StoredProcedure, "LMS_GetCertPartsQA", lstParam.ToArray());
            dsLookup.Tables[0].TableName = Lookup.CERTPARTS_TBL;

            objSconn.Dispose();
            return dsLookup;
        }


        public DataSet LoadCertPartsQA(string PartId, string version, string strUser)
        {
            DataSet dsLookup = new DataSet();
            SqlConnection objSconn = new SqlConnection(strDbConn);
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@PartId", SqlDbType.VarChar, 50);
            spParam.IsNullable = false;
            spParam.Value = PartId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@version", SqlDbType.VarChar, 20);
            spParam.IsNullable = false;
            spParam.Value = version;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@User", SqlDbType.VarChar, 200);
            spParam.IsNullable = false;
            spParam.Value = strUser;
            lstParam.Add(spParam);


            dsLookup = SqlHelper.ExecuteDataset(objSconn, CommandType.StoredProcedure, "LMS_GetQACertPartsByVersion", lstParam.ToArray());
            dsLookup.Tables[0].TableName = Lookup.CERTPARTS_TBL;

            objSconn.Dispose();
            return dsLookup;
        }

        public DataSet LoadLegacyCertParts(string PartId)
        {
            DataSet dsLookup = new DataSet();
            SqlConnection objSconn = new SqlConnection(strDbConn);
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@PartId", SqlDbType.VarChar, 50);
            spParam.IsNullable = false;
            spParam.Value = PartId;
            lstParam.Add(spParam);

            dsLookup = SqlHelper.ExecuteDataset(objSconn, CommandType.StoredProcedure, "LMS_GetLegacyCertParts", lstParam.ToArray());
            dsLookup.Tables[0].TableName = Lookup.CERTPARTS_TBL;

            objSconn.Dispose();
            return dsLookup;
        }

        public DataSet LoadLegacyCertParts(string PartId, string strCertPart)
        {
            SqlConnection objSconn = new SqlConnection(strDbConn);
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@CertCode", SqlDbType.VarChar, 100); spParam.Value = strCertPart;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@PartId", SqlDbType.VarChar, 100); spParam.Value = PartId;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(objSconn, CommandType.StoredProcedure, "LMS_GetLegacyParts", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            else
            {
                return null;
            }
        }

        public DataSet LoadEvalParts(string site, string strVersion)
        {
            DataSet dsLookup = new DataSet();
            SqlConnection objSconn = new SqlConnection(strDbConn);
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@brand", SqlDbType.VarChar, 30);
            spParam.IsNullable = false;
            spParam.Value = site;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@version", SqlDbType.VarChar, 500);
            spParam.IsNullable = false;
            spParam.Value = strVersion;
            lstParam.Add(spParam);

            dsLookup = SqlHelper.ExecuteDataset(objSconn, CommandType.StoredProcedure, "LMS_LoadEvalParts", lstParam.ToArray());
            dsLookup.Tables[0].TableName = Lookup.EVALPARTS_TBL;

            objSconn.Dispose();
            return dsLookup;
        }

        public string GetLookupText(string strLookupValue, string strLookupType)
        {
            DataSet dsLookup = new DataSet();
            SqlConnection objSconn = new SqlConnection(strDbConn);
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@LookupValue", SqlDbType.VarChar, 200);
            spParam.IsNullable = false;
            spParam.Value = strLookupValue;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@LookupType", SqlDbType.VarChar, 200);
            spParam.IsNullable = false;
            spParam.Value = strLookupType;
            lstParam.Add(spParam);

            dsLookup = SqlHelper.ExecuteDataset(objSconn, CommandType.StoredProcedure, "LMS_getLookupText", lstParam.ToArray());
            if (dsLookup.Tables[0].Rows.Count > 0)
                return dsLookup.Tables[0].Rows[0]["LookupText"].ToString();

            objSconn.Dispose();
            return string.Empty;
        }

        public bool IsPageVisible(string strPageType, int intPageId, string strRole, int AcctId)
        {
            string strCount = string.Empty;
            SqlConnection objSconn = new SqlConnection(strDbConn);
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@Role", SqlDbType.VarChar, 100);
            spParam.IsNullable = false;
            spParam.Value = strRole;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@PageType", SqlDbType.VarChar, 100);
            spParam.IsNullable = false;
            spParam.Value = strPageType;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@PageId", SqlDbType.Int);
            spParam.IsNullable = false;
            spParam.Value = intPageId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AcctId", SqlDbType.Int);
            spParam.IsNullable = false;
            spParam.Value = AcctId;
            lstParam.Add(spParam);

            strCount = SqlHelper.ExecuteScalar(objSconn, CommandType.StoredProcedure, "LMS_CheckPageAccessLevel", lstParam.ToArray()).ToString();
            if (strCount == null || strCount.Equals("0"))
                return false;
            else
                return true;

        }

        public bool IsAuthorisedPersonnel(string strPageName, string strUserEmail)
        {
            string strCount = string.Empty;
            SqlConnection objSconn = new SqlConnection(strDbConn);
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@WebPageId", SqlDbType.VarChar, 100);
            spParam.IsNullable = false;
            spParam.Value = strPageName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@UserEmail", SqlDbType.VarChar, 100);
            spParam.IsNullable = false;
            spParam.Value = strUserEmail;
            lstParam.Add(spParam);

            strCount = SqlHelper.ExecuteScalar(objSconn, CommandType.StoredProcedure, "LMS_CheckPageAuthPersonnel", lstParam.ToArray()).ToString();
            if (strCount == null || strCount.Equals("0"))
                return false;
            else
                return true;

        }

        public DataSet GetConfigData(int intPageSize, int intPageNum, string Filter)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);


            SqlCommand objComm = new SqlCommand("LMS_getConfigData", new SqlConnection(strDbConn));
            objComm.CommandType = CommandType.StoredProcedure;
            objComm.CommandTimeout = 200;
            SqlParameterCollection prmColl = objComm.Parameters;
            for (int i = 0; i < lstParam.Count; i++)
            {
                prmColl.Add(lstParam[i]);
            }

            SqlDataAdapter objAdap = new SqlDataAdapter(objComm);
            objAdap.Fill(ds);

            return ds;

        }

        public bool CheckConfigData(string strSerialNum, string strMacAddr)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@SerialNumber", SqlDbType.VarChar, 100); spParam.Value = strSerialNum;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@MacAddr", SqlDbType.VarChar, 50); spParam.Value = strMacAddr;
            lstParam.Add(spParam);

            ds = SqlHelper.ExecuteDataset(new SqlConnection(strDbConn), CommandType.StoredProcedure, "LMS_CheckConfigData", lstParam.ToArray());
            if (ds.Tables.Count > 0)
            {
                ds.Tables[0].TableName = "CD_INFO";
                if (Int16.Parse(ds.Tables[0].Rows[0]["CDataCount"].ToString()) > 0)
                    return true;
            }
            return false;

        }

        public bool AddConfigData(string strSerialNum, string strMacAddr, SqlTransaction objTran)
        {
            bool retVal = true;
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@SerialNumber", SqlDbType.VarChar, 100); spParam.Value = strSerialNum;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@MacAddr", SqlDbType.VarChar, 50); spParam.Value = strMacAddr;
            lstParam.Add(spParam);

            int NoOfRowsAffected = Int32.Parse(SqlHelper.ExecuteNonQuery(objTran, CommandType.StoredProcedure, "LMS_AddConfigData", lstParam.ToArray()).ToString());
            if (NoOfRowsAffected == 0)
                retVal = false;

            return retVal;



        }

    }
}
