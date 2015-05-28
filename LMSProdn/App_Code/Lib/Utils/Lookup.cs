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
using System.Collections.Generic;
using Com.Arubanetworks.Licensing.Dataaccesslayer;



/// <summary>
/// Summary description for Lookup
/// </summary>
namespace Com.Arubanetworks.Licensing.Lib.Utils
{
    public class Lookup
    {
      
        public const string CERT_TYPE="CERT_TYPE";
        public const string MODULES_TBL = "MODULES_TBL";
        public const string MENUS_TBL = "MENUS_TBL";
        public const string LOOKUP_TBL = "LOOKUP_TBL";
        public const string EVALPARTS_TBL = "LOOKUP_TBL";
        public const string CERTPARTS_TBL = "CERTPARTS_TBL";
        public const string RFPPARTS_TBL = "RFPPARTS_TBL";
        private string strDbConn = string.Empty;
        public Lookup()
        {
            strDbConn = ConfigurationManager.ConnectionStrings["LMSDB"].ConnectionString;

        }

        public DataSet LoadMenuItems(string strUserRole, int usrAcctId,string strBrand)
        {
            DataSet dsLookup = new DataSet();
            DALookup objDALookup = new DALookup();
            dsLookup = objDALookup.LoadMenuItems(strUserRole, usrAcctId,strBrand);
            return dsLookup;

        }

        public DataSet LoadLookupValues(string strUserID,string strBrand)
        {
            DataSet dsLookup = new DataSet();
            DALookup objDALookup = new DALookup();
            dsLookup = objDALookup.LoadLookupValues(strUserID,strBrand);
            return dsLookup;

        }

        public DataSet LoadEvalParts(string site,string strVersion)
        {
            DataSet dsLookup = new DataSet();
            DALookup objDALookup = new DALookup();
            dsLookup = objDALookup.LoadEvalParts(site,strVersion);
            return dsLookup;

        }

        public string GetLookupText(string strLookupValue,string strLookupType)
        {
            DALookup objDALookup = new DALookup();
            return objDALookup.GetLookupText(strLookupValue, strLookupType);

        }


        public static string GetTimeStamp()
        {
            string date = DateTime.Now.ToShortDateString().Replace("/", "");
            string time = DateTime.Now.ToShortTimeString().Replace(":", "").Replace(" ", "");
            return date + time;
        }

        public DataSet LoadCertParts(string PartId)
        {
            DALookup objDALookup = new DALookup();
            return objDALookup.LoadCertParts(PartId);
        }

        public DataSet LoadRFPParts()
        {
            DALookup objDALookup = new DALookup();
            return objDALookup.LoadRFPparts();
        }

        public DataSet LoadCertParts(string PartId, string CertCode)
        {
            DALookup objDALookup = new DALookup();
            return objDALookup.LoadCertParts(PartId,CertCode);
        }

        public DataSet LoadCertPartsQA(string PartId, string version,string strUser)
        {
            DALookup objDALookup = new DALookup();
            return objDALookup.LoadCertPartsQA(PartId, version,strUser);
        }

        public DataSet LoadLegacyCertParts(string PartId)
        {
            DALookup objDALookup = new DALookup();
            return objDALookup.LoadLegacyCertParts(PartId);
        }

        public DataSet LoadLegacyCertParts(string PartId,string strCertPart)
        {
            DALookup objDALookup = new DALookup();
            return objDALookup.LoadLegacyCertParts(PartId, strCertPart);
        }
       
        public DataSet LoadCertPartsQA(string PartId, string CertCode,string version,string strUserId)
        {
            DALookup objDALookup = new DALookup();
            return objDALookup.LoadCertPartsQA(PartId, CertCode, version, strUserId);
        }

        public DataSet GetConfigData(int intPageSize,int intPageNum,string strFilter)
        {
            DALookup objDALookup = new DALookup();
            return objDALookup.GetConfigData(intPageSize, intPageNum, strFilter);
        }

        public bool IsExistsConfigData(string strSerialNumber, string strMacAddr)
        {
            DALookup objDALookup = new DALookup();
            return objDALookup.CheckConfigData(strSerialNumber, strMacAddr);
           
        
        }

        public bool AddConfigData(string strSerialNum, string strMacAddr, string strIPAddr, string strUser)
        {
            string meth = "AddConfigData";
            DALookup objDALookup = new DALookup();
            DACertificate objDACert = new DACertificate();
            SqlConnection objSConn = new SqlConnection(strDbConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;

            try
            {
                if (objDALookup.AddConfigData(strSerialNum, strMacAddr, objTran))
                {
                    string sql = "AddConfigData(" + strSerialNum + "," + strMacAddr + ")";
                    if (!objDACert.AddTransactionRecord(strUser, sql, strIPAddr, meth, objTran))
                    {
                        retVal = false;
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

    }

}