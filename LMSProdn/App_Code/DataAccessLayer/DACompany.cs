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
using Com.Arubanetworks.Licensing.Lib.Utils;
using Com.Arubanetworks.Licensing.Lib.Data;

/// <summary>
/// Summary description for DACompany
/// </summary>

namespace Com.Arubanetworks.Licensing.Dataaccesslayer
{
    public class DACompany
    {
        string strSQLConn = string.Empty;
        public DACompany()
        {
            strSQLConn = ConfigurationManager.ConnectionStrings["LMSDB"].ToString();
        }

        public DataSet GetCompanyInfo(int intCompanyId)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@companyId", SqlDbType.Int);
            spParam.Value = intCompanyId;
            lstParam.Add(spParam);
            DataSet dscompanyInfo = new DataSet();

            dscompanyInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getCompanyInfoById", lstParam.ToArray());
            if (dscompanyInfo.Tables.Count > 0)
            {
                dscompanyInfo.Tables[0].TableName = "COMPANY_INFO";
                if (dscompanyInfo.Tables[0].Rows.Count > 0)
                    return dscompanyInfo;
            }
            return null;
        }
        public DataSet GetCompanyListAll(string strBrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@brand", SqlDbType.VarChar,10); spParam.Value = strBrand;
            lstParam.Add(spParam);
            DataSet dscompanyInfo = new DataSet();

            dscompanyInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getCompanyListAll",lstParam.ToArray());
            if (dscompanyInfo.Tables.Count > 0)
            {
                dscompanyInfo.Tables[0].TableName = "COMPANY_INFO";
                if (dscompanyInfo.Tables[0].Rows.Count > 0)
                    return dscompanyInfo;
            }
            return null;
        }

        public DataSet GetDistributorInfo(int intCompanyId)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@companyId", SqlDbType.Int);
            spParam.Value = intCompanyId;
            lstParam.Add(spParam);
            DataSet dscompanyInfo = new DataSet();

            dscompanyInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getDistributorById", lstParam.ToArray());
            if (dscompanyInfo.Tables.Count > 0)
            {
                dscompanyInfo.Tables[0].TableName = "COMPANY_INFO";
                dscompanyInfo.Tables[1].TableName = "DIST_IDS";
                if (dscompanyInfo.Tables[0].Rows.Count > 0)
                    return dscompanyInfo;
            }
            return null;
        }


        public DataSet GetCompanyList(int intPageSize, int intPageNum, string Filter, string Brand, string Type)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Brand", SqlDbType.VarChar, 20); spParam.Value = Brand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Type", SqlDbType.VarChar, 20); spParam.Value = Type;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getCompanyList", new SqlConnection(strSQLConn));
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
        public DataSet GetDistributorList(int intPageSize, int intPageNum, string Filter, string Brand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Brand", SqlDbType.VarChar, 20); spParam.Value = Brand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getDistributorList", new SqlConnection(strSQLConn));
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

        public DataSet GetResellerList(int intPageSize, int intPageNum, string Filter, string Brand, string Type,int intDistributorId)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Brand", SqlDbType.VarChar, 20); spParam.Value = Brand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Type", SqlDbType.VarChar, 20); spParam.Value = Type;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@DistId", SqlDbType.VarChar, 20); spParam.Value = intDistributorId;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getResellerList", new SqlConnection(strSQLConn));
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



        public DataSet CheckDistinctCompanyName(string CompanyName)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@NewCompanyName", SqlDbType.VarChar,100);
            spParam.Value = CompanyName;
            lstParam.Add(spParam);
            DataSet dscompanyInfo = new DataSet();

            dscompanyInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_checkDistinctCompanyName", lstParam.ToArray());
            return dscompanyInfo;
        }
        public int AddCompanyInfo(string strCompanyName, string strBrand, string strType, SqlTransaction objTrans)
        {
             
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@companyName", SqlDbType.VarChar, 60); spParam.Value = strCompanyName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = strBrand;
            lstParam.Add(spParam);


            spParam = new SqlParameter("@type", SqlDbType.VarChar, 20); spParam.Value = strType;
            lstParam.Add(spParam);



            string temp = SqlHelper.ExecuteScalar(objTrans, CommandType.StoredProcedure, "LMS_addCompanyInfo", lstParam.ToArray()).ToString();

            int CompanyId = 0;
            if (!temp.Equals(string.Empty))
                CompanyId = Int32.Parse(temp);

            return CompanyId;
            
        }

        public bool AddCompanyDistLink(int CompanyId,string DistId, SqlTransaction objTrans)
        {
            bool retval = true;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@companyId", SqlDbType.Int); spParam.Value = CompanyId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@distId", SqlDbType.VarChar, 20); spParam.Value = DistId;
            lstParam.Add(spParam);
            
            int NoOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_addCompanydistLink", lstParam.ToArray());
            if (NoOfRowsAffected > 0)
                retval = true;
            else
                retval = false;

            return retval;

        }

        public bool RemoveCompanyDistLink(int CompanyId,SqlTransaction objTrans)
        {
            bool retval = true;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@companyId", SqlDbType.Int); spParam.Value = CompanyId;
            lstParam.Add(spParam);
                        
            int NoOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_removeCompanyDistLink", lstParam.ToArray());
            if (NoOfRowsAffected > 0)
                retval = true;
            else
                retval = false;

            return retval;

        }


        public bool UpdateCompanyInfo(int intCompanyId,string strCompanyName, string strBrand, string strType, SqlTransaction objTrans)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@companyName", SqlDbType.VarChar, 60); spParam.Value = strCompanyName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = strBrand;
            lstParam.Add(spParam);


            spParam = new SqlParameter("@type", SqlDbType.VarChar, 20); spParam.Value = strType;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@companyId", SqlDbType.Int); spParam.Value = intCompanyId;
            lstParam.Add(spParam);



            int NoOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_updateCompanyInfo", lstParam.ToArray());

            if (NoOfRowsAffected > 0)
                return true;
            else
                return false;
        }

        public bool RemoveMember(int intAcctId)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctId;
            lstParam.Add(spParam);

            int NoOfRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_deleteMember", lstParam.ToArray());

            if (NoOfRowsAffected > 0)
                return true;
            else
                return false;
        }

        public int GetCompanyID(string CompanyName,string strBrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@companyName", SqlDbType.VarChar,200); spParam.Value = CompanyName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = strBrand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_GetCompanyId", lstParam.ToArray());

            if (ds.Tables[0].Rows.Count > 0)
                return Int32.Parse(ds.Tables[0].Rows[0]["Company_Id"].ToString());
            else
                return -1;
        }

        public bool IsMyDistributor(int intDistId,int intResellerId)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@distId", SqlDbType.Int); spParam.Value = intDistId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@resellerId", SqlDbType.Int); spParam.Value = intResellerId;
            lstParam.Add(spParam);


            int NoOfRows = Int32.Parse(SqlHelper.ExecuteScalar(strSQLConn, CommandType.StoredProcedure, "LMS_checkResellerForDistributor", lstParam.ToArray()).ToString());

            if (NoOfRows > 0)
                return true;
            else
                return false;
        }

        public DataSet GetNonDistributorIds(int CompanyId)
        {
            DataSet ds = new DataSet();
            SqlCommand objComm = new SqlCommand("LMS_getNonDistributorIds", new SqlConnection(strSQLConn));
            objComm.CommandType = CommandType.StoredProcedure;
            objComm.CommandTimeout = 200;

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@companyId", SqlDbType.Int); spParam.Value = CompanyId;
            lstParam.Add(spParam);
            SqlParameterCollection prmColl = objComm.Parameters;
            for (int i = 0; i < lstParam.Count; i++)
            {
                prmColl.Add(lstParam[i]);
            }

            SqlDataAdapter objAdap = new SqlDataAdapter(objComm);
            
            objAdap.Fill(ds);

            return ds;

        }

    }
}