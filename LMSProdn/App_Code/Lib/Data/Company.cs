using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Com.Arubanetworks.Licensing.Dataaccesslayer;
using Com.Arubanetworks.Licensing.Lib.Utils;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Company
/// </summary>
namespace Com.Arubanetworks.Licensing.Lib.Data
{
    public class Company
    {
        public const string COMPANY_ID = "company_id";
        public const string COMPANY_NAME = "company_name";
        public const string COMPANY_TYPE = "type";
        public const string COMPANY_BRAND = "brand";
        public const string PARENT_COMPANY_ID = "ParentCompanyId";


        public const string LINKS_TYPE = "distributor_reseller";

        #region ErrorCodes
        public const string NO_COMPANY_INFO = "This Company does not exist";
        public const string DUP_COMPANY_NAME = "This Company is already present in the system";
        public const string FAILURE_ADD_COMPANY="Unable to add this Company!";
        public const string FAILURE_EDIT_COMPANY = "Unable to edit this Company!";
        public const string FAILURE_IS_MY_DIST= "This reseller does not belong to you!";
        public const string FAILURE_REMOVE_MEMBER = "Unable to remove this member!";
        public const string NO_DIST_SELECTED = "Please select Distributor Ids";
        #endregion

        private DACompany daoComp;
       
        private string strSQLConn;

       public Company()
        {
            daoComp = new DACompany();
            strSQLConn = ConfigurationManager.ConnectionStrings["LMSDB"].ToString();
           
        }

        public DataSet GetCompanyListAll(string strBrand)
        {
            return daoComp.GetCompanyListAll(strBrand);
        }

        public DataSet GetCompanyInfo(int intCompanyId)
        {
            return daoComp.GetCompanyInfo(intCompanyId);
        }

        public DataSet GetDistributorInfo(int intCompanyId)
        {
            return daoComp.GetDistributorInfo(intCompanyId);
        }
        
        public DataSet GetCompanyList(int intPageSize, int intPageNum, string Filter, string Brand, string Type,int intUserCompanyId)
        {
            DataSet ds = new DataSet();
            switch (Type)
            {
                    
                case CompanyType.Customer:
                case CompanyType.Distributor:
                    ds= daoComp.GetCompanyList(intPageSize, intPageNum, Filter, Brand, Type);
                    break;
                case CompanyType.Reseller:
                    ds= daoComp.GetResellerList(intPageSize, intPageNum, Filter, Brand, Type, intUserCompanyId);
                    break;

            
            }
            return ds;            
        }

        public DataSet GetDistributorList(int intPageSize, int intPageNum, string Filter, string Brand)
        {
            return daoComp.GetDistributorList(intPageSize, intPageNum, Filter,Brand);
                   
        }
        public DataSet GetNonDistributorIds(int intCompanyId)
        {
            return daoComp.GetNonDistributorIds(intCompanyId);

        }

        public DataSet CheckDistinctCompanyName(string strNewCompanyName)
        {
            return daoComp.CheckDistinctCompanyName(strNewCompanyName);
        }

        public bool AddCompany(string CompanyName,string Brand,string Type,int ParentCompanyId)
        {
            string meth = "AddCompany:Company";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
            DACertificate daoCert = new DACertificate();
            int noOfRowsAffected=-1;
            try
            {
                int CompanyId = 0;
                CompanyId = daoComp.AddCompanyInfo(CompanyName, Brand, Type, objTran);
                if (CompanyId>0)
                {
                    if (Type == CompanyType.Reseller)
                    {
                        noOfRowsAffected = daoCert.AssignCertificate(ParentCompanyId, CompanyId, LINKS_TYPE,objTran);//to include relation in links table
                        if (noOfRowsAffected > 0)
                            retVal = true;
                        else
                            retVal = false;

                    }
                    if (retVal == true)
                    {
                        //call insert into customer Master to include this company too
                        retVal = true;
                        //if (callToCustomerMasterAddAPI == true)
                        //{

                        //    retVal = true;
                        //}
                        //else
                        //{
                        //    retVal = false;
                        //}
                    }
                }
                else
                {
                    retVal = false;
                }

                if (retVal == true)
                {
                    objTran.Commit();
                }
                else 
                {
                    objTran.Rollback();
                }
            }
            catch (Exception ex)
            {
                objTran.Rollback();
                new Log().logException(meth, ex);
                retVal = false;
            }
            finally
            {
                objTran.Dispose();
                if (objSConn != null && objSConn.State == ConnectionState.Open)
                    objSConn.Close();
            }
            return retVal;
        
        }

        public bool AddCompany(string CompanyName,string Brand,string Type,int ParentCompanyId,string[] DistIds)
        {
            string meth = "AddCompany:Distributor";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
            DACertificate daoCert = new DACertificate();
       
            try
            {
                int CompanyId = 0;
                CompanyId = daoComp.AddCompanyInfo(CompanyName, Brand, Type, objTran);
                if (CompanyId>0)
                {
                    if (Type == CompanyType.Distributor)
                    {
                        //to make sure there are no repetitions
                        daoComp.RemoveCompanyDistLink(CompanyId,objTran);
                        for (int i = 0; i < DistIds.Length; i++)
                        {
                            if (!daoComp.AddCompanyDistLink(CompanyId, DistIds[i], objTran))//to include relation in distributor_ids table
                            {
                                retVal = false;
                                break;
                            }
                            

                        }
                        
                    }
                    if (retVal == true)
                    {
                        //call insert into customer Master to include this company too
                        retVal = true;
                        //if (callToCustomerMasterAddAPI == true)
                        //{

                        //    retVal = true;
                        //}
                        //else
                        //{
                        //    retVal = false;
                        //}
                    }
                }
                else
                {
                    retVal = false;
                }

                if (retVal == true)
                {
                    objTran.Commit();
                }
                else 
                {
                    objTran.Rollback();
                }
            }
            catch (Exception ex)
            {
                objTran.Rollback();
                new Log().logException(meth, ex);
                retVal = false;
            }
            finally
            {
                objTran.Dispose();
                if (objSConn != null && objSConn.State == ConnectionState.Open)
                    objSConn.Close();
            }
            return retVal;
        
        }


        public bool EditCompany(int CompanyId,string CompanyName,string Brand, string Type)
        {
            string meth = "EditCompany:Company";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
              try
            {
                
                if(daoComp.UpdateCompanyInfo(CompanyId,CompanyName, Brand, Type, objTran))
                {
                    
                    if (retVal == true)
                    {
                        //call insert into customer Master to include this company too
                        retVal = true;
                        //if (callToCustomerMasterAddAPI == true)
                        //{

                        //    retVal = true;
                        //}
                        //else
                        //{
                        //    retVal = false;
                        //}
                    }
                }
                else
                {
                    retVal = false;
                }

                if (retVal == true)
                {
                    objTran.Commit();
                }
                else
                {
                    objTran.Rollback();
                }
            }
            catch (Exception ex)
            {
                objTran.Rollback();
                new Log().logException(meth, ex);
                retVal = false;
            }
            finally
            {
                objTran.Dispose();
                if (objSConn != null && objSConn.State == ConnectionState.Open)
                    objSConn.Close();
            }
            return retVal;

        }
        public bool EditCompany(int CompanyId, string CompanyName, string Brand, string Type,string[] DistIds)
        {
            string meth = "EditCompany:Distributor";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
            try
            {

                if (daoComp.UpdateCompanyInfo(CompanyId, CompanyName, Brand, Type, objTran))
                {
                    if (Type == CompanyType.Distributor)
                    {
                        //to make sure there are no repetitions
                        daoComp.RemoveCompanyDistLink(CompanyId, objTran);
                        for (int i = 0; i < DistIds.Length; i++)
                        {
                            if (!daoComp.AddCompanyDistLink(CompanyId, DistIds[i], objTran))//to include relation in distributor_ids table
                            {
                                retVal = false;
                                break;
                            }


                        }

                    }
                    if (retVal == true)
                    {
                        //call insert into customer Master to include this company too
                        retVal = true;
                        //if (callToCustomerMasterAddAPI == true)
                        //{

                        //    retVal = true;
                        //}
                        //else
                        //{
                        //    retVal = false;
                        //}
                    }
                }
                else
                {
                    retVal = false;
                }

                if (retVal == true)
                {
                    objTran.Commit();
                }
                else
                {
                    objTran.Rollback();
                }
            }
            catch (Exception ex)
            {
                objTran.Rollback();
                new Log().logException(meth, ex);
                retVal = false;
            }
            finally
            {
                objTran.Dispose();
                if (objSConn != null && objSConn.State == ConnectionState.Open)
                    objSConn.Close();
            }
            return retVal;

        }


        public bool RemoveMember(int AcctId)
        {

            return daoComp.RemoveMember(AcctId);

        }

        public int GetCompanyID(string CompanyName,string strBrand)
        {
            return daoComp.GetCompanyID(CompanyName,strBrand);
        }

       

        public bool IsMyDistributor(int DistId, int ResellerId)
        {
            return daoComp.IsMyDistributor(DistId, ResellerId);
        }



    }
    public sealed class CompanyType
    {
        public const string Customer = "customer";
        public const string Distributor = "distributor";
        public const string Reseller = "reseller";
    }
}