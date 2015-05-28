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
/// Summary description for DAUser
/// </summary>
namespace Com.Arubanetworks.Licensing.Dataaccesslayer
{
    public class DAUser
    {
        string strSQLConn = string.Empty;
        public DAUser()
        {
            strSQLConn = ConfigurationManager.ConnectionStrings["LMSDB"].ToString();
        }

        public DataSet GetMembers(int intCompanyId, int intPageSize, int intPageNum, string Filter)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CompanyId", SqlDbType.Int); spParam.Value = intCompanyId;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getMembers", new SqlConnection(strSQLConn));
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

        public DataSet GetUserInfo(int intAcctId,string strBrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = intAcctId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar,10); spParam.Value = strBrand;
            lstParam.Add(spParam);
                    
            SqlCommand objComm = new SqlCommand("LMS_getAcctInfoByAcctId", new SqlConnection(strSQLConn));
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

        public DataSet GetUserInfo(string strEmail,string strBrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@email", SqlDbType.VarChar,100); spParam.Value = strEmail;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = strBrand;
            lstParam.Add(spParam);

            //ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getAcctInfoByEmail",lstParam.ToArray());

            SqlCommand objComm = new SqlCommand("LMS_getAcctInfoByEmail", new SqlConnection(strSQLConn));
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


        public DataSet CheckExistingAccounts(string EmailList)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@EmailList", SqlDbType.Text); spParam.Value = EmailList;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_checkExistingAccounts", new SqlConnection(strSQLConn));
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

        public DataSet CheckPendingAccounts(string EmailList)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@EmailList", SqlDbType.Text); spParam.Value = EmailList;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_checkPendingAccounts", new SqlConnection(strSQLConn));
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

        public DataSet CheckPendingGroupAccounts(string EmailList)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@EmailList", SqlDbType.Text); spParam.Value = EmailList;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_checkPendingGroupAccounts", new SqlConnection(strSQLConn));
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

        public DataSet AddPendingAccounts(string Email, int CompanyId, string Brand)
        {
            //string retVal = string.Empty;
            DataSet retVal;
            SqlParameter spParam = new SqlParameter("@CompanyId", SqlDbType.Int); spParam.Value = CompanyId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
                   

            spParam = new SqlParameter("@Email", SqlDbType.VarChar, 100); spParam.Value = Email;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Brand", SqlDbType.VarChar, 20); spParam.Value = Brand;
            lstParam.Add(spParam);

            retVal = SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_AddPendingAccount", lstParam.ToArray());
            return retVal;
        }

        public DataSet GetUngroupedAccounts(string strCompanyType, int intPageSize, int intPageNum, string Filter)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CompanyType", SqlDbType.VarChar, 50); spParam.Value = strCompanyType;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getUngroupedAccounts", new SqlConnection(strSQLConn));
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

        public DataSet GetAccountsList( int intPageSize, int intPageNum, string Filter,string strBrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Brand", SqlDbType.VarChar, 20); spParam.Value = strBrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getAccountsList", new SqlConnection(strSQLConn));
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

        public DataSet GetUnAssignedAccounts(int intPageSize, int intPageNum, string Filter, string strBrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Brand", SqlDbType.VarChar, 20); spParam.Value = strBrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_GetUnAssignedAccounts", new SqlConnection(strSQLConn));
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

        public DataSet GetCompanyAccounts(int intPageSize, int intPageNum, string Filter, string strBrand,string companyid)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Brand", SqlDbType.VarChar, 20); spParam.Value = strBrand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CompanyId", SqlDbType.VarChar, 4); spParam.Value = companyid;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getCompanyAccounts", new SqlConnection(strSQLConn));
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

        public bool GroupCurrentAccounts(int intCompanyId, string CommaSeperatedAcctIds)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@CompanyId", SqlDbType.Int); spParam.Value = intCompanyId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CommaSepAcctIds", SqlDbType.Text); spParam.Value = CommaSeperatedAcctIds;
            lstParam.Add(spParam);

            int intRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_groupCurrentAccounts", lstParam.ToArray());
            if (intRowsAffected == 0)
                return false;
            else
                return true;
        }


        public bool AddUser(UserInfo objLicUser)
        {
            string meth="AddUser";
            bool retVal=true;
          
            try
            {
                SqlParameter spParam = new SqlParameter("@cust_type", SqlDbType.VarChar, 20); spParam.Value = objLicUser.Role;
                List<SqlParameter> lstParam = new List<SqlParameter>();
                lstParam.Add(spParam);


                spParam = new SqlParameter("@company", SqlDbType.VarChar, 255); spParam.Value = objLicUser.CompanyName;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@firstname", SqlDbType.VarChar, 100); spParam.Value = objLicUser.FirstName;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@lastname", SqlDbType.VarChar, 100); spParam.Value = objLicUser.LastName;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@email", SqlDbType.VarChar, 255); spParam.Value = objLicUser.Email;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@password", SqlDbType.VarChar, 255); spParam.Value = objLicUser.Password;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@phone", SqlDbType.VarChar, 50); spParam.Value = objLicUser.Phone;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = objLicUser.Brand;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@status", SqlDbType.VarChar, 10); spParam.Value = objLicUser.Status;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@company_id", SqlDbType.Int); spParam.Value = objLicUser.CompanyId;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@Comments", SqlDbType.VarChar,255); spParam.Value = objLicUser.Comments;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@IsCompanyAdmin", SqlDbType.Int); spParam.Value = objLicUser.isCompanyAdmin;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = objLicUser.AcctId;
                lstParam.Add(spParam);

                string StrMenuId = ConfigurationManager.AppSettings["COMPANYADMIN_MENU"].ToString();
                string[] arrPageInfo = StrMenuId.Split('|');
                spParam = new SqlParameter("@MenuId", SqlDbType.Int); spParam.Value = Int32.Parse(arrPageInfo[0]);
                lstParam.Add(spParam);
                
                spParam = new SqlParameter("@MenuId1", SqlDbType.Int); spParam.Value = Int32.Parse(arrPageInfo[1]);
                lstParam.Add(spParam);
                
                int NoOfRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_addAcctInfo", lstParam.ToArray());
                 
                if (NoOfRowsAffected == 0)
                     retVal = false;

            }
            catch (Exception ex)
            {
                new Log().logException(meth, ex);
                retVal = false;
            }

            return retVal;
        }

        public bool IsUserCompanyAdmin(string strEmail)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@Email", SqlDbType.VarChar,200); spParam.Value = strEmail;
            lstParam.Add(spParam);

            DataSet dsCompanyAdmin = SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_IsCompanyAdmin", lstParam.ToArray());
            if (dsCompanyAdmin.Tables.Count > 0)
            {
                if (dsCompanyAdmin.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }


        public bool UpdateAccount(UserInfo objLicUser)
        {
            string meth = "AddUser";
            bool retVal = true;

            try
            {
                SqlParameter spParam = new SqlParameter("@cust_type", SqlDbType.VarChar, 20); spParam.Value = objLicUser.Role;
                List<SqlParameter> lstParam = new List<SqlParameter>();
                lstParam.Add(spParam);


                spParam = new SqlParameter("@company", SqlDbType.VarChar, 255); spParam.Value = objLicUser.CompanyName;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@firstname", SqlDbType.VarChar, 100); spParam.Value = objLicUser.FirstName;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@lastname", SqlDbType.VarChar, 100); spParam.Value = objLicUser.LastName;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@email", SqlDbType.VarChar, 255); spParam.Value = objLicUser.Email;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@phone", SqlDbType.VarChar, 50); spParam.Value = objLicUser.Phone;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@status", SqlDbType.VarChar, 10); spParam.Value = objLicUser.Status;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@company_id", SqlDbType.Int); spParam.Value = Int32.Parse(objLicUser.CompanyId.ToString());
                lstParam.Add(spParam);

                spParam = new SqlParameter("@Comments", SqlDbType.VarChar,255); spParam.Value = objLicUser.Comments;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@IsCompanyAdmin", SqlDbType.Bit); spParam.Value = objLicUser.isCompanyAdmin;
                lstParam.Add(spParam);

                spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = Int32.Parse(objLicUser.AcctId.ToString());
                lstParam.Add(spParam);

                string StrMenuId = ConfigurationManager.AppSettings["COMPANYADMIN_MENU"].ToString();
                string[] arrPageInfo = StrMenuId.Split('|');
                spParam = new SqlParameter("@MenuId", SqlDbType.Int); spParam.Value = Int32.Parse(arrPageInfo[0]);
                lstParam.Add(spParam);

                spParam = new SqlParameter("@MenuId1", SqlDbType.Int); spParam.Value = Int32.Parse(arrPageInfo[1]);
                lstParam.Add(spParam);

                int NoOfRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_UpdateAccount", lstParam.ToArray());

                if (NoOfRowsAffected == 0)
                    retVal = false;
            }
            catch (Exception ex)
            {
                new Log().logException(meth, ex);
                retVal = false;

            }

            return retVal;
        }

        public bool IsOnlyAdmin(string strEmail, int CompanyId)
        {
            bool retVal = false;
            DataSet ds = new DataSet(); 
            SqlParameter spParam = new SqlParameter("@Email", SqlDbType.VarChar, 255); spParam.Value = strEmail;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@CompanyId", SqlDbType.Int); spParam.Value = CompanyId;
            lstParam.Add(spParam);

            ds = SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_CheckIsOnlyAdmin", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
                retVal = true;

            return retVal;
        }



        public bool UpdateAccountStatus(string strEmail, string strStatus)
        {
            bool retVal = false;
            SqlParameter spParam = new SqlParameter("@Email", SqlDbType.VarChar, 255); spParam.Value = strEmail;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@Status", SqlDbType.VarChar, 10); spParam.Value = strStatus;
            lstParam.Add(spParam);

            int NoOfRowsupdated = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_UpdateAccountStatus", lstParam.ToArray());
            if (NoOfRowsupdated > 0)
                retVal = true;

            return retVal;
        }

        public DataSet GetPendingAccount(string strActivationCode)
        {

            SqlParameter spParam = new SqlParameter("@ActivationCode", SqlDbType.VarChar, 255); spParam.Value = strActivationCode;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
          
            DataSet ds =SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_getPendingAcctInfo", lstParam.ToArray());
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            ds = null;

            return ds;
        }

        public bool RemovePendingAccount(string strActivationCode)
        {
            bool retVal=false;
            SqlParameter spParam = new SqlParameter("@ActivationCode", SqlDbType.VarChar, 255); spParam.Value = strActivationCode;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            int NoOfRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_deletePendingAccount", lstParam.ToArray());

            if (NoOfRowsAffected == 1)
                retVal = true;

            return retVal;
           
        }

        public bool UpdateUserEmailId(string strEmailId,string strOldEmailId)
        {
            bool retVal = false;
            SqlParameter spParam = new SqlParameter("@NewEmail", SqlDbType.VarChar, 255); spParam.Value = strEmailId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@OldEmail", SqlDbType.VarChar, 255); spParam.Value = strOldEmailId;
            lstParam.Add(spParam);

            int NoOfRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_UpdateEmail", lstParam.ToArray());

            if (NoOfRowsAffected == 1)
                retVal = true;

            return retVal;
        }

    }
}
