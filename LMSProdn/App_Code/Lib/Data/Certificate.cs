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
using System.Web.SessionState;

/// <summary>
/// Summary description for Certificate
/// </summary>
namespace Com.Arubanetworks.Licensing.Lib.Data
{
    public class Certificate : System.Web.UI.Page
    {
        public string CertificateId = string.Empty;
        public string SerialNumber = string.Empty;
        public string PartId = string.Empty;
        private string strPartMap = string.Empty;
        private string strPartMapSerialNumber = string.Empty;
        private string strSQLConn = string.Empty;
        private DACertificate daoCert;
        private DALog daoLog;
        public const string TABLE = "ACT_CERTS";
        public const string TABLECLP = "CLP_CERTS";

        public Email objemail = new Email();
        

        #region Column Names for Cert Activation
        public const string ROW_NO = "RowNo";
        public const string LIC_ID = "LicId";
        public const string LIC_PART_ID = "LicPartId";
        public const string LIC_PART_DESC = "LicPartDesc";
        public const string ACTIVATION_KEY = "ActivationKey";
        public const string LIC_ALC_PART_ID_3EM = "LicAlc3EMPartId";
        public const string LIC_SERIAL_NUMBER = "LicSN";
        public const string LIC_ERROR = "LicError";
        public const string SYS_ID = "SysId";
        public const string SYS_PART_ID = "SysPartId";
        public const string SYS_PART_DESC = "SysPartDesc";
        public const string SYS_ALC_PART_ID_3EM = "SysAlc3EMPartId";
        public const string SYS_SERIAL_NUMBER = "SysSN";
        public const string SYS_ERROR = "SysError";
        public const string COMMENTS = "Comments";
        public const string FORCE_SN_MAC = "ForceSnMac";
        public const string RFP = "RFP";
        public const string LOC = "Location";
        public const string VERSION = "version";
        public const string IS_ARUBA_RFP = "IsArubaRFP";
        
        #endregion


        #region Column Names for searching Certificate Info

        public const string CI_SERIAL_NUMBER="serial_number";
        public const string CI_SO_ID = "so_id";
        public const string CI_TYPE = "certType";
        public const string CI_RESELLER_ID="Reseller_Id";
        public const string CI_CERT_ID = "Cert_Id";
        public const string CI_PO_ID = "cust_po_id";
        public const string CI_ENDUSERPO_ID = "end_user_po";


        #endregion



        #region ErrorCode

        public static  string NO_PART_MAP = "This certificate Id has no Part Map entry!";
                public static string NO_CERT_INFO = "This certificate does not exist";
                public static string DUP_CERT_INFO = "This certificate is a duplicate entry in your input";
                public static string NO_SYS_INFO = "This system does not exist";
                public static string YES_ACTIVATED = "This certificate is already activated!";
                public static string YES_ACTIVATED_SYSTEM = "This certificate is already activated on this system!";
                public static string NO_CERT_SYSTEM_COMPATIBLE = "This certificate is not compatible with this system!";
                public static string SUCCESS_ACTIVATION = "The certificate was successfully activated";
                public static string FAILURE_ACTIVATION = "The certificate was not activated!";
                public static string FAILURE_KEYGEN = "Unable to generate License Key!";
                public static string FAILURE_ADD_ACT_INFO = "Unable to add License Key info!";
                public static string PERSISTENCE_ISSUE = "Error occurred when saving Activation Info!";
                public static string OWNER_FAIL = "This Certificate does not belong to you!";
                public static string MAX_COUNT = "This Certificate is already transferred more than allocated number!Please Contact TAC team for Assistance.";
        public static string EQ_COUNT = "This Certificate has now reached the maximum allocated number. You will not be able to transfer it any more time.";
                public static string MAX_AIRW_COUNT = "You have exceeded the maximum number of times you can transfer this license. Please contact Aruba support team for further assistance.";
                public static string MAX_AIRW_EVAL_COUNT = "You have exceeded the maximum number of times you can generate Eval license. Please contact Aruba support team for further assistance.";
                public static string FAILURE_EVALCERT = "Unable to generate Evaluation certificate";
                public static string FAILURE_IMPORTCTOCERTS = "Unable to import preloaded licenses!";
                public static string SUCCESS_IMPORTCTOCERTS = "Preloaded licenses were imported successfully";
                public static string NOT_EVAL = "This is not Eval Certificate";
                public static string NO_ALC_PART = "No Equivalent Aruba Part found for Alcatel";
                public static string NO_FRU_INFO = "This Controller does not exist";

        #endregion

        public Certificate()
        {
            daoCert = new DACertificate();
            daoLog = new DALog();
            strSQLConn = ConfigurationManager.ConnectionStrings["LMSDB"].ToString();
        }

        public DataTable BuildCertInfo()
        {
            DataTable dtCertInfo = new DataTable(TABLE);
            DataColumnCollection columns;
            columns = dtCertInfo.Columns;

            AddColumn(dtCertInfo, ROW_NO, "System.Int32", false, 0);
            AddColumn(dtCertInfo, LIC_ID, "System.Int32", false, 0);
            AddColumn(dtCertInfo, SYS_ID, "System.Int32", false, 0);

            AddColumnString(dtCertInfo, LIC_PART_ID);
            AddColumnString(dtCertInfo, LIC_ALC_PART_ID_3EM);
            AddColumnString(dtCertInfo, LIC_PART_DESC);
            AddColumnString(dtCertInfo, LIC_ERROR);
            AddColumnString(dtCertInfo, LIC_SERIAL_NUMBER);

            AddColumnString(dtCertInfo,SYS_PART_ID);
            AddColumnString(dtCertInfo, SYS_ALC_PART_ID_3EM);
            AddColumnString(dtCertInfo, SYS_PART_DESC);
            AddColumnString(dtCertInfo, SYS_ERROR);
            AddColumnString(dtCertInfo, SYS_SERIAL_NUMBER);

            AddColumnString(dtCertInfo, ACTIVATION_KEY);
            AddColumnString(dtCertInfo, COMMENTS);
            AddColumnString(dtCertInfo, FORCE_SN_MAC);
            AddColumnString(dtCertInfo, RFP);
            AddColumnString(dtCertInfo, LOC);
            AddColumnString(dtCertInfo, VERSION);
            AddColumnString(dtCertInfo,IS_ARUBA_RFP);

            return dtCertInfo;
        }

        public DataTable BuildAirwaveCertInfo()
        {
            DataTable dtAirwaveCert = new DataTable(TABLE);
            DataColumnCollection columns;
            columns = dtAirwaveCert.Columns;

            AddColumn(dtAirwaveCert, "RowNo", "System.Int32", false, 0);
            AddColumn(dtAirwaveCert, "pk_cert_id", "System.Int32", false, 0);

            AddColumnString(dtAirwaveCert, "part_id");
            AddColumnString(dtAirwaveCert, "part_desc");
            AddColumnString(dtAirwaveCert, "serial_number");
            AddColumnString(dtAirwaveCert, "ActivationKey");
            AddColumnString(dtAirwaveCert, "Organization");
            AddColumnString(dtAirwaveCert, "Error");
            AddColumnString(dtAirwaveCert, "IPAddress");
            AddColumnString(dtAirwaveCert, "so_id");
            AddColumnString(dtAirwaveCert, "IsConsolidated");
            AddColumnString(dtAirwaveCert, "ConsolidateIP");
            AddColumnString(dtAirwaveCert, "Lserial_number");

            return dtAirwaveCert;
        }

        public DataTable BuildAmgCertInfo()
        {
            DataTable dtAmgCert = new DataTable(TABLE);
            DataColumnCollection columns;
            columns = dtAmgCert.Columns;

            AddColumn(dtAmgCert, "RowNo", "System.Int32", false, 0);
            AddColumn(dtAmgCert, "pk_cert_id", "System.Int32", false, 0);

            AddColumnString(dtAmgCert, "SubscriptionKey");
            AddColumnString(dtAmgCert, "part_id");
            AddColumnString(dtAmgCert, "part_desc");
            AddColumnString(dtAmgCert, "serial_number");
            AddColumnString(dtAmgCert, "LSerial_number");
            AddColumnString(dtAmgCert, "Organization");
            AddColumnString(dtAmgCert, "Error");
            AddColumnString(dtAmgCert, "po_id");
            AddColumnString(dtAmgCert, "so_id");
            AddColumnString(dtAmgCert, "license_count");
            AddColumnString(dtAmgCert, "onBoard_count");            
            AddColumnString(dtAmgCert, "expire_time");            
            AddColumnString(dtAmgCert, "user_name");
            AddColumnString(dtAmgCert, "password");
            AddColumnString(dtAmgCert, "sms_credit");
            AddColumnString(dtAmgCert, "sms_handler");
            AddColumnString(dtAmgCert, "email");
            AddColumnString(dtAmgCert, "create_time");
            AddColumnString(dtAmgCert, "HA");
            AddColumnString(dtAmgCert, "advertising");
            AddColumnString(dtAmgCert, "category");
            AddColumnString(dtAmgCert, "sms_count");
            AddColumnString(dtAmgCert, "HASubscriptionKey");
            AddColumnString(dtAmgCert, "HApart_id");
            AddColumnString(dtAmgCert, "HAserial_number");
            AddColumnString(dtAmgCert, "HALserial_number");
            AddColumnString(dtAmgCert, "LicSerialNo");
            AddColumnString(dtAmgCert, "LicCertId");
            AddColumnString(dtAmgCert, "AdvSerialNo");
            AddColumnString(dtAmgCert, "AdvCertId");
            AddColumnString(dtAmgCert, "OnBoardSerialNo");
            AddColumnString(dtAmgCert, "OnBoardCertId");
            AddColumnString(dtAmgCert, "SkuId");
            AddColumnString(dtAmgCert, "CustId");            

            return dtAmgCert;
        }

        public DataTable BuildClpCertInfo()
        {
            DataTable dtClpCert = new DataTable(TABLECLP);
            DataColumnCollection columns;
            columns = dtClpCert.Columns;

            AddColumn(dtClpCert, "RowNo", "System.Int32", false, 0);
            AddColumn(dtClpCert, "pk_cert_id", "System.Int32", false, 0);

            AddColumnString(dtClpCert, "LicenseKey");
            AddColumnString(dtClpCert, "part_id");
            AddColumnString(dtClpCert, "part_desc");
            AddColumnString(dtClpCert, "serial_number");
            AddColumnString(dtClpCert, "LSerial_number");
            AddColumnString(dtClpCert, "Error");
            AddColumnString(dtClpCert, "expire_time");
            AddColumnString(dtClpCert, "user_name");
            AddColumnString(dtClpCert, "password");
            AddColumnString(dtClpCert, "create_time");
            AddColumnString(dtClpCert, "version");
            AddColumnString(dtClpCert, "so_id");
            AddColumnString(dtClpCert, "po_id");
            AddColumnString(dtClpCert, "subscription_key");
            AddColumnString(dtClpCert, "cust_name");
            AddColumnString(dtClpCert, "num_users");
            AddColumnString(dtClpCert, "IsImported");
            return dtClpCert;
        }

        private void AddColumn(DataTable table, string column, string type, bool allowNull, object defaultValue)
        {
            DataColumn columnNew = table.Columns.Add(column);
            columnNew.DataType = System.Type.GetType(type);
            columnNew.AllowDBNull = allowNull;
            columnNew.ColumnName = column;
            columnNew.Caption = column;
            columnNew.DefaultValue = defaultValue;
            columnNew.ColumnMapping = MappingType.Attribute;
        }

        private void AddColumnString(DataTable table, string column)
        {
            DataColumn columnNew = table.Columns.Add(column);
            columnNew.DataType = System.Type.GetType("System.String");
            // use "true" for most columns so that retrieving from database will not
            // fail for rows with incorrect null columns. 
            columnNew.AllowDBNull = true;
            columnNew.ColumnName = column;
            columnNew.Caption = column;
            columnNew.ColumnMapping = MappingType.Attribute;
        }

        public string getQuickConnectPassword(string strUserName)
        {
            return daoCert.getQuickConnectPassword(strUserName);
        }

        public DataSet GetQCDetails(string userName, string brand)
        {
            return daoCert.GetQCDetails(userName, brand);
        }

        public bool HasPartMapEntry(KeyGenInput objKeyGenIp) 
            // ashwini: include "brand" in input after demo eg: see decodeMMScertificate function in this class
        {

            KeyGen.ArubaLicensing objKeygen = new KeyGen.ArubaLicensing();
            KeyGen.ComplexType1 objKGInp = new KeyGen.ComplexType1();
            objKeygen.Url = ConfigurationManager.AppSettings["KeyGen.URL"];
            string decCertType = string.Empty;
            bool blnHasPartMap = false;
            

            if (string.Empty.Equals(objKeyGenIp.CertSerialNumber)
               || string.Empty.Equals(objKeyGenIp.Brand))
                return false;

            objKGInp.brand = objKeyGenIp.Brand;
            objKGInp.cert = objKeyGenIp.CertSerialNumber;
            objKGInp.acctid = "3374";
            objKGInp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
            objKGInp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString();            
            //objKGInp.brand = "aruba";
            //objKGInp.cert = strCertificateSerialNumber;
            decCertType = objKeygen.decodeCert(objKGInp);
            if (decCertType.ToLower().Contains("unknown")) //returns "unknown part" if not  valid cert id
            {
                DataSet ds = daoCert.GetPartMapEntry(objKeyGenIp.CertSerialNumber);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                        {
                            blnHasPartMap = true;
                        }

                    }
                }
            }
            else
            {
                blnHasPartMap = true;
            }

            return blnHasPartMap;
        }
        public DataSet GetPartMapEntry(string strCertificateSerialNumber)
        {
            DataSet ds = daoCert.GetPartMapEntry(strCertificateSerialNumber);
            return ds;
        }
        public DataSet GetSerialNumberCertifcateMap(string strSerialNumber)
        {
            DataSet ds = daoCert.GetSerialNumberCertifcateMap(strSerialNumber);
            return ds;
        } 

        public string getAlcatelPartNo(string strPartNo)
        {
            return daoCert.getAlcatelPartNo(strPartNo);
        }

        public bool AddUploadInfo(string strUploadfile, string strSavefile,int FileSize, int User)
        {
            return daoCert.AddUploadInfo(strUploadfile, strSavefile, FileSize, User);
        }

        public string GetPartID(string strSerialNumber)
        {
            string strPartID = daoCert.GetPartID(strSerialNumber);
            return strPartID;
        }

        public DataSet GetIAPInfo(string strSerialNo)
        {
            return daoCert.GetIAPInfo(strSerialNo);
        }

        public DataSet GetCertInfo(string strCertificateSerialNumber)
        {
            return daoCert.GetCertificateInfo(strCertificateSerialNumber);
        }

        public DataSet GetClpCertificateInfo(string strCertId)
        {
            return daoCert.GetClpCertificateInfo(strCertId);
        }

        public DataSet GetMyController(int intAcctId, int intPageSize, int intPageNum, string Filter, string strBrand)
        {
            return daoCert.GetMyController(intAcctId, intPageSize, intPageNum, Filter,strBrand);
        }

        public DataSet GetAllSerialNo(int intAcctid, int intPageSize, int intPageNum, string Filter, string strBrand)
        {
            return daoCert.GetAllSerialNo(intAcctid, intPageSize, intPageNum, Filter, strBrand);
        }

        public bool UpdateDownloadLog(int intAcctId, string strFileName, string strBrand)
        {
            return daoCert.UpdateDownloadLog(intAcctId, strFileName, strBrand);
        }

        public string GetUserEmail(int intAcctId)
        {
            return daoCert.GetUserEmail(intAcctId);
        }

        //public string GetQuickConnectUser(string userName)
        //{
        //    return daoCert.GetQuickConnectUser(userName);
        //}

        public DataSet GetDownLoadsDet(int intAcctid, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetDownLoadsDet(intAcctid, intPageSize, intPageNum, Filter, strbrand);
        }

        public DataSet GetFruInfo(string strFru)
        {
            return daoCert.GetFruInfo(strFru);
        }

        public DataSet GetCertPartDesc(string strpartId)
        {
            return daoCert.GetCertPartDesc(strpartId);
        }

        public string AddMacAddress(string partId, string partdesc, string serialNumber, string brand)
        {
            return daoCert.AddMacAddress(partId, partdesc, serialNumber, brand);
        } 

        public DataSet GetCertInfo(int intCertId)
        {
            return daoCert.GetCertificateInfo(intCertId);
        }

        public DataSet getCertDetails(string strCertId)
        {
            return daoCert.getCertDetails(strCertId);
        }

        public DataSet getAirwaveCertDet(string strAirwaveCert,string strBrand)
        {
            return daoCert.getAirwaveCertDet(strAirwaveCert,strBrand);
        }

        public DataSet getAWCerDetails(string strCertSlNo, string strBrand)
        {
            return daoCert.getAWCerDetails(strCertSlNo, strBrand);
        }

        public DataSet getALECerDetails(string strCertSlNo, string strBrand)
        {
            return daoCert.getALECerDetails(strCertSlNo, strBrand);
        }        

        public DataSet getAmgCertDetails(string strCertSlNo, string strBrand)
        {
            return daoCert.getAmgCertDetails(strCertSlNo, strBrand);
        }

        public string getAWCertificate(string strSerialNumber)
        {
            return daoCert.getAWCertificate(strSerialNumber);
        }

        public string getAWLicenseKey(string strSerialNumber)
        {
            return daoCert.getAWLicenseKey(strSerialNumber);
        }

        public DataSet getPartDetails(string strProduct, string strBrand)
        {
            return daoCert.getPartDetails(strProduct, strBrand);
        }

        public DataSet GetFAQ(string strRole, string strBrand)
        {
            return daoCert.GetFAQ(strRole, strBrand);
        }

        public bool IsAirwaveCertActivated(string strCertId)
        {
            return daoCert.IsAirwaveCertActivated(strCertId);
        }

        //Added by Praveena on June/2014

        public DataSet GetAirwaveCerts(int pageSize, int PageNumber, string filter, string strbrand)
        {
            return daoCert.GetAirwaveCerts(pageSize, PageNumber, filter, strbrand);
        }

        public bool ReassignAirwaveCertificates(int intOldAcctId, int intDestAcctId, int impersonatedBy, string[] arrCerts)
        {
            return daoCert.ReassignAirwaveCertificates(intOldAcctId, intDestAcctId, impersonatedBy, string.Join(",", arrCerts));
        }

        public DataSet GetAirwaveCertInfoForCertId(int certId)
        {
            return daoCert.GetAirwaveCertInfoForCertId(certId);
        }

        public bool GetAirwaveCert(string strCertId, string strBrand)
        {
            DataSet ds = new DataSet();
            ds = daoCert.getAirwaveCertDet(strCertId, strBrand);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsAirwaveCertConsolidated(string strCertId1, string strCertId2, string strIPAddress)
        {
            return daoCert.IsAirwaveCertConsolidated(strCertId1, strCertId2,strIPAddress);
        }

        public string GenerateAirwaveActivation(string strOrder, string strIPAddress, string strPart, string strOrg, int APCount, string strSerialNo)
        {
            AirwavekeyGen.Airwave objAirwave = new AirwavekeyGen.Airwave();
            objAirwave.Url = ConfigurationManager.AppSettings["AIRWAVE_URL"];
            string strActivationKey = objAirwave.GetActivationKey(strOrder, strPart, strOrg, strIPAddress, APCount, strSerialNo);     
            return strActivationKey;
        }

        public string ConsolidateAirwvLic(string strPart, string strOrg, string strIPAddress, bool blRapid, bool blVisualRF, int intAPCount, string strSerialNo)
        {
            AirwavekeyGen.Airwave objAirwave = new AirwavekeyGen.Airwave();
            objAirwave.Url = ConfigurationManager.AppSettings["AIRWAVE_URL"];
            string strNewActivationKey = objAirwave.GetConsolidatedActivationKey(strPart, strOrg, strIPAddress, blRapid, blVisualRF, intAPCount, strSerialNo);
            return strNewActivationKey;
        }

        public DataSet GetAirwaveCertInfoForIP(int CertId)
        {
            return daoCert.GetAirwaveCertInfoForIP(CertId);
        }

        public string GetAirwaveCertInfoForIP(string certId)
        {
            string strActKey = string.Empty;
            DataSet dsResult = daoCert.GetAirwaveCertInfoForIP(certId);
            if (dsResult != null && dsResult.Tables[0].Rows.Count > 0)
            {
                strActKey = dsResult.Tables[0].Rows[0]["ActivationKey"].ToString();
            }
            else
            {
                strActKey = string.Empty;
            }
            return strActKey;
        }

        public bool CheckAirwaveOwnership(int intAcctId, int intCertId)
        {
            return daoCert.CheckAirwaveOwnership(intAcctId, intCertId);
        }

        public bool UpdateAirwaveActivationInfo(DataTable dt, int intAcctId)
        {
            string meth = "UpdateActivationInfo: Airwave Certs";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            string strCertId = string.Empty; string strIPAddress = string.Empty;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
            try
            {
                string strActivationCode = string.Empty;
                int NoofRowsAffected = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strActivationCode = string.Empty;
                    strCertId = dt.Rows[i]["serial_number"].ToString();
                    strIPAddress = dt.Rows[i]["IPAddress"].ToString();
                    strActivationCode = dt.Rows[i]["ActivationKey"].ToString();
                    if (UpdateAirwaveActivationInfo(intAcctId, strCertId, strIPAddress, strActivationCode, objTran))
                        NoofRowsAffected++;
                }

                if (NoofRowsAffected == dt.Rows.Count)
                {
                    objTran.Commit();
                    retVal = true;
                }
                else
                {
                    objTran.Rollback();
                    retVal = false;
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

        public DataSet LoadAirwaveEvalParts(string strBrand)
        {
            return daoCert.LoadAirwaveEvalParts(strBrand);
        }

        public bool AddAirwvEvalCertificate(string strEvalPart, string strEvalKey, string strSoldTo, string strOrg, string strBrand, string strCertType)
        {
            return daoCert.AddAirwvEvalCertificate(strEvalPart, strEvalKey, strSoldTo, strOrg, strBrand, strCertType);
        }

        public bool RestrictGenerateAirwEvalCert(string strPartNo, string strEmail,string strBrand)
        {
            return daoCert.RestrictGenerateAirwEvalCert(strPartNo, strEmail, strBrand);
        }

        public DataSet GetAirwaveCertByAcct(int intAcctId, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetAirwaveCertByAcct(intAcctId, intPageSize, intPageNum, Filter, strbrand);
        }

        public bool UpdateAirwaveActivationInfo(int intAcctId, string strCertId, string strIPAddress, string strActivationCode, SqlTransaction objTran)
        {
            string meth = "Certificate:UpdateActivationInfo";
            bool blnTrnSuccess = true;

            int intNoOfRows = 0;
            try
            {
                intNoOfRows = daoCert.UpdateAirwaveActivationInfo(intAcctId,strCertId, strIPAddress, strActivationCode, objTran);
                if (intNoOfRows > 0)
                {
                    intNoOfRows = daoLog.LogAirwaveTransferInfo(intAcctId, strCertId, strIPAddress, objTran);
                    if (intNoOfRows == 0)
                    {
                        blnTrnSuccess = false;
                    }
                }
                else
                {
                    blnTrnSuccess = false;
                }
            }

            catch (Exception ex)
            {
                new Log().logException(meth, ex);
                blnTrnSuccess = false;
            }

            return blnTrnSuccess;
        }

        public string GenerateAirwEvalCert(string strEvalpart, string strOrg)
        {
            AirwavekeyGen.Airwave objAirwave = new AirwavekeyGen.Airwave();
            objAirwave.Url = ConfigurationManager.AppSettings["AIRWAVE_URL"];
            string strActivationKey = objAirwave.GetEvalKey(strEvalpart, strOrg);
            return strActivationKey;
        }

        public bool InsertAirwaveActkey(string strActivationKey, string strIPAddress, string strEvalkey, string strOrganizationName, int AcctId)
        {
            bool blResult = false;
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            string meth = "InsertAirwaveActkey";
            string strCertId = string.Empty;
            int intNoOfRows = 0;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            try
            {
                blResult = daoCert.InsertAirwaveActkey(strActivationKey, strIPAddress, strEvalkey, strOrganizationName, AcctId, objTran);
                if (blResult == true)
                {
                    intNoOfRows = daoLog.LogAirwaveActivationInfo(AcctId, strEvalkey, strIPAddress, objTran);
                }
                if (intNoOfRows > 0)
                {
                    objTran.Commit();
                }
                else
                {
                    objTran.Rollback();
                }
            }
            catch (Exception e)
            {
                objTran.Rollback();
                new Log().logException(meth, e);
            }
            finally
            {
                objTran.Dispose();
                if (objSConn != null && objSConn.State == ConnectionState.Open)
                    objSConn.Close();
            }
            return blResult;
        }

        public bool InsertConsolidateAirwveActKey(string strActivationKey, string strIPAddress, string strCertId1, string strCertId2, string strOrganizationName, int AcctId)
        {
            bool blResult = false;
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            string meth = "InsertConsolidateAirwveActKey";
            int intNoOfRows1 = 0; int intNoOfRows2 = 0;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            try
            {
                blResult = daoCert.InsertConsolidateAirwveActKey(strActivationKey, strIPAddress, strCertId1, strCertId2, strOrganizationName, AcctId, objTran);
                if (blResult == true)
                {
                    intNoOfRows1 = daoLog.LogAirwaveConsolidationInfo(AcctId, strCertId1, strIPAddress, objTran);
                    intNoOfRows2 = daoLog.LogAirwaveConsolidationInfo(AcctId, strCertId2, strIPAddress, objTran);
                    if (intNoOfRows1 > 0 && intNoOfRows2 > 0)
                    {
                        objTran.Commit();
                    }
                    else
                    {
                        objTran.Rollback();
                    }
                }
            }
            catch (Exception e)
            {
                objTran.Rollback();
                new Log().logException(meth, e);
            }
            finally
            {
                objTran.Dispose();
                if (objSConn != null && objSConn.State == ConnectionState.Open)
                    objSConn.Close();
            }
            return blResult;
        }

        public bool LogSentEmail(DataTable dtCert, int AcctId, string strTo)
        {
            string meth = "LogSentEmail";
            int NoofRowsAffected = 0;
            string strSoId = string.Empty;
            string StrTo = string.Empty;
            string StrCertId = string.Empty;
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
            try
            {
                for (int index = 0; index < dtCert.Rows.Count; index++)
                {
                    strSoId = dtCert.Rows[0]["SO_ID"].ToString();
                    StrCertId = dtCert.Rows[0]["SERIAL_NUMBER"].ToString();
                    //strTo = dtCert.Rows[0]["LICENSE_CONTACT"].ToString();
                    if (daoCert.LogSentEmail(StrCertId, strSoId, strTo, AcctId))
                        NoofRowsAffected++;
                }

                if (NoofRowsAffected == dtCert.Rows.Count)
                {
                    objTran.Commit();
                    retVal = true;
                }
                else
                {
                    objTran.Rollback();
                    retVal = false;
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

        public DataSet GetCertInfoForFru(int intCertId, string strVersion)
        {
            return daoCert.GetCertInfoForFru(intCertId, strVersion);
        }

        public DataSet GetOPCertInfo(string strSerialNumber)
        {
           return daoCert.GetOPCertInfo(strSerialNumber);
        }

        public DataSet GetConfigData(string strMacAdd)
        {
            return daoCert.GetConfigData(strMacAdd);
        }

        public string GetSerialOrMacReq(string strLicPartId, string strBrand)
        {
            return daoCert.GetSerialOrMacReq(strLicPartId, strBrand);
        }

        public DataSet CheckActivationInfo(string strListOfCertIds)
        {
            return daoCert.CheckActivationInfo(strListOfCertIds);
        }

        public bool CancelSerialNumber(int intCertId, string strUserId, string strReason)
        { 
        return daoCert.CancelSerialNumber(intCertId, strUserId, strReason);
        }

        public int getOldestSerialNo(string certValue)
        {
            DataSet dsCert =  daoCert.getOldestSerialNo(certValue);
            if (dsCert != null)
                return Int32.Parse(dsCert.Tables[0].Rows[0]["pk_cert_id"].ToString());
            else
                 return 0;
        }

        public bool CancelOPSActivations(string SerialNo, string strUserId)
        {
            return daoCert.CancelOPSActivations(SerialNo, strUserId);
        }

        public bool AddEvalActivationInfo(DataTable dt, int UserId, string strIPAddress,string brand)
        {
            return daoCert.AddEvalActivationInfo(dt,UserId, strIPAddress,brand);
        }

        public string getPartType(string PartID,string strBrand)
        {
            return daoCert.getPartType(PartID, strBrand);
        }

        public string getPartDesc(string PartID,string brand)
        {
            return daoCert.getPartDesc(PartID,brand);
        }

        public DataSet GetQuickConnectCertInfo(string certId, string brand)
        {
            return daoCert.GetQuickConnectCertInfo(certId, brand);
        }

        public DataSet getQuickConnectUserInfo(string email)
        {
            return daoCert.getQuickConnectUserInfo(email);
        }

        public DataSet getQuickConnectCompanyInfo(string email)
        {
            return daoCert.getQuickConnectCompanyInfo(email);
        }

        public DataSet getQuickConnectUserInfo(string email, string companyName)
        {
            return daoCert.getQuickConnectUserInfo(email, companyName);
        }

        //2014
        public bool isFlexibleLicense(string PartNo)
        {
            return daoCert.isFlexibleLicense(PartNo);
        }

        public bool IsSameVersionCertOnCtrl(string strSerialNo, string strCertid)
        {
            return daoCert.IsSameVersionCertOnCtrl(strSerialNo, strCertid);
        }

        public DataSet GetCertsByFru(string strFru,string brand)
        {
            return daoCert.GetCertsByFru(strFru, brand);
        }

        public bool LoadDataToDB(DataSet dsData,string strFru,string brand)
        {
            string meth = "AddUpgradeCerts";
            int NoofRowsAffected = 0;
            string strLicName = string.Empty;
            string strCert = string.Empty;
            string strActkey = string.Empty;
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
            try
            {
                for (int i = 0; i < dsData.Tables[0].Rows.Count;i++ )
                {
                    strLicName = dsData.Tables[0].Rows[0]["LIC_NAME"].ToString();
                    strCert = dsData.Tables[0].Rows[0]["CERT"].ToString();
                    strActkey = dsData.Tables[0].Rows[0]["ACTIVATION_KEY"].ToString();
                    if (daoCert.LoadDataToDB(strFru, strLicName, strCert, strActkey, brand))
                        NoofRowsAffected++;
                }

                if (NoofRowsAffected == dsData.Tables[0].Rows.Count)
                {
                    objTran.Commit();
                    retVal = true;
                }
                else
                {
                    objTran.Rollback();
                    retVal = false;
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

        public bool AddClpActivationInfo(DataTable dt, int ImpersonatedAcctId, int AcctId, string strSubKey)
        {
            string meth = "AddClpActivationInfo: MulitpleCerts";
            SqlConnection objSConn = new SqlConnection(strSQLConn);        
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
            try
            {               
                int NoofRowsAffected = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string strActivationCode = string.Empty;
                    int intCertId = 0; string strLocation = string.Empty;
                    string strVersion = string.Empty;
                    intCertId = Int32.Parse(dt.Rows[i][Certificate.LIC_ID].ToString());                    
                    strActivationCode = dt.Rows[i][Certificate.ACTIVATION_KEY].ToString();
                    strLocation = dt.Rows[i][Certificate.LOC].ToString();
                    strVersion = dt.Rows[i][Certificate.VERSION].ToString();
                    if (AddClpActivationInfo(AcctId, ImpersonatedAcctId, intCertId, strActivationCode, strLocation, strVersion, strSubKey, objTran))
                        NoofRowsAffected++;
                }

                if (NoofRowsAffected == dt.Rows.Count)
                {
                    objTran.Commit();
                    retVal = true;
                }
                else
                {
                    objTran.Rollback();
                    retVal = false;
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
        
        public bool InsertQuickConnect (int pk_cert_id, int ImpAcctId, string companyName, int AcctId, int licenseCount, string expiry_date, string Name, string email, string username, string Password)
        {

            string meth = "Certificate:AddQuickConnectInfo";
            bool blnTrnSuccess = true;
            int intNoOfRows = 0;
            DACertificate daoCert = new DACertificate();
            DALog daoLog = new DALog();
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();            
            objTran = objSConn.BeginTransaction();
            try
            {
                intNoOfRows = daoCert.InsertQuickConnect(pk_cert_id, ImpAcctId, companyName, AcctId, expiry_date,username, Password, licenseCount,objTran);          
                if (intNoOfRows > 0)
                {
                    intNoOfRows = daoLog.LogClpActivationInfo(AcctId, pk_cert_id, ImpAcctId, objTran);
                    if (intNoOfRows > 0)
                    {
                        objTran.Commit();
                        blnTrnSuccess = true;
                    }
                    else
                    {
                        objTran.Rollback();
                        blnTrnSuccess = false;
                    }
                }
                else
                {
                    objTran.Rollback();
                    blnTrnSuccess = false;
                }
            }

            catch (Exception ex)
            {
                new Log().logException(meth, ex);
                objTran.Rollback();
                blnTrnSuccess = false;
            }

            return blnTrnSuccess;
        }

        public bool AddClpActivationInfo(int AcctId, int ImpAcctId, int intCertId, string strActivationCode, string strLocation, string strVersion,string strSubKey,SqlTransaction objTran)
        {
            
            string meth = "Certificate:AddClpActivationInfo";
            bool blnTrnSuccess = true;
            int intNoOfRows = 0;
            try
            {
                intNoOfRows = daoCert.AddClpActivationInfo(intCertId, ImpAcctId, AcctId, strActivationCode, strLocation, strVersion, strSubKey,objTran);
                if (intNoOfRows > 0)
                {
                    intNoOfRows = daoLog.LogClpActivationInfo(AcctId, intCertId, ImpAcctId, objTran);
                    if (intNoOfRows > 0)
                    {
                        blnTrnSuccess = true;
                    }            
                    else
                    {
                        blnTrnSuccess = false;
                    }               
                }
                else
                {
                    blnTrnSuccess = false;
                }            
            }

            catch (Exception ex)
            {
                new Log().logException(meth, ex);
                blnTrnSuccess = false;
            }
                
            return blnTrnSuccess;
        }

        public bool AddActivationInfo(DataTable dt, int intAcctId,string strType)
        {
            string meth = "AddActivationInfo: MulitpleCerts";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
            try
            {
                
                int intCertId, intSystemId;
                string strActivationCode = string.Empty;
                int NoofRowsAffected = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    intCertId = Int32.Parse(dt.Rows[i][Certificate.LIC_ID].ToString());
                    intSystemId = Int32.Parse(dt.Rows[i][Certificate.SYS_ID].ToString());
                    strActivationCode = dt.Rows[i][Certificate.ACTIVATION_KEY].ToString();
                    if (AddActivationInfo(intAcctId, intCertId, intSystemId, strActivationCode, strType, objTran))
                        NoofRowsAffected++;

                }

                if (NoofRowsAffected == dt.Rows.Count)
                {
                    objTran.Commit();
                    retVal = true;
                }
                else
                {
                    objTran.Rollback();
                    retVal = false;
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

        public bool UpdateLocation(DataTable dt, int intAcctId)
        {
            string meth = "Certificate:UpdateLocation";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
            try
            {

                string strSerialNo,strLocation;
                string strActivationCode = string.Empty;
                string strCertpartid = string.Empty;
                int NoofRowsAffected = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strSerialNo = dt.Rows[i][Certificate.SYS_SERIAL_NUMBER].ToString();
                    strLocation = dt.Rows[i][Certificate.LOC].ToString();
                    strActivationCode = dt.Rows[i][Certificate.ACTIVATION_KEY].ToString();
                    strCertpartid = dt.Rows[i][Certificate.LIC_PART_ID].ToString();
                    //check for version if necessary
                    if (daoCert.isUpgradebleCert(strCertpartid))
                    {
                        if (!daoCert.UpdateUpgradeStat(strSerialNo, objTran))
                        {
                            //send mail
                            Email objEmail = new Email();
                            objEmail.UpdateFailureInfo(strSerialNo, ConfigurationManager.AppSettings["DEV_MAIL"].ToString());
                        }
                    }
                    if (daoCert.UpdateLocation(strSerialNo, strLocation, intAcctId, objTran))
                        NoofRowsAffected++;
                }

                if (NoofRowsAffected == dt.Rows.Count)
                {
                    objTran.Commit();
                    retVal = true;
                }
                else
                {
                    objTran.Rollback();
                    retVal = false;
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

     public bool AddActivationInfo(int intAcctId, int intCertId, int intSystemId, string strActivationCode,string strType ,SqlTransaction objTran)
    {
        string meth = "Certificate:AddActivationInfo";
        bool blnTrnSuccess = true;
      
        int intNoOfRows = 0;
        try
        {
            intNoOfRows = daoCert.AddActivationInfo(intCertId, intSystemId, strActivationCode, objTran);
            if (intNoOfRows > 0)
            {
                intNoOfRows = daoCert.AssignCertificate(intAcctId, intCertId, strType,objTran);
                if (intNoOfRows > 0)
                {
                    intNoOfRows = daoLog.LogActivationInfo(intAcctId, intCertId, intSystemId, objTran);
                    if (intNoOfRows > 0)
                    {
                        //add distributor-cert mapping in links table
                       
                        UserInfo objUserInf = new User().GetUserInfo(intAcctId,Session["BRAND"].ToString());
                        if (objUserInf.GetUserCompanyId() != -1)
                        {
                            if (objUserInf.GetUserRole().Equals(UserType.Distributor))
                            {
                                strType = CertType.CompanyCertificate;
                                intNoOfRows = daoCert.AssignCertificate(objUserInf.GetUserCompanyId(), intCertId, strType, objTran);
                                if (intNoOfRows == 0)
                                {
                                    blnTrnSuccess = false;
                                }
                            }
                        }

                    }
                    else 
                    {
                        blnTrnSuccess = false;
                    }
                }
                else
                {
                    blnTrnSuccess = false;
                }
               

            }
            else
            {
                blnTrnSuccess = false;
            }


            
        }

        catch (Exception ex)
        {
            new Log().logException(meth, ex);
            blnTrnSuccess = false;
        }
        
        
        return blnTrnSuccess;
    
    }

        public bool AddActivationInfo(int intAcctId, int intCertId, int intSystemId, string strActivationCode, string strType)
        {
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            bool retVal = false;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();

             retVal=AddActivationInfo(intAcctId, intCertId, intSystemId, strActivationCode, strType, objTran);

             if (retVal)
            {
                objTran.Commit();
            }

            else
            {
                objTran.Rollback();

            }

            objSConn.Close();
            return retVal;


        }

        public bool AddQAActivationInfo(int intAcctId, int intCertId, int intSystemId, string strActivationCode, string strType, string strSession)
        {
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            bool retVal = false;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();

            retVal = AddQAActivationInfo(intAcctId, intCertId, intSystemId, strActivationCode, strType, strSession, objTran);

            if (retVal)
            {
                objTran.Commit();
            }

            else
            {
                objTran.Rollback();

            }

            objSConn.Close();
            return retVal;
        }

        public bool AddQAActivationInfo(int intAcctId, int intCertId, int intSystemId, string strActivationCode, string strType, string strSession, SqlTransaction objTran)
        {
            string meth = "Certificate:AddActivationInfo";
            bool blnTrnSuccess = true;

            int intNoOfRows = 0;
            try
            {
                intNoOfRows = daoCert.AddActivationInfo(intCertId, intSystemId, strActivationCode, objTran);
                if (intNoOfRows > 0)
                {
                    intNoOfRows = daoCert.AssignCertificate(intAcctId, intCertId, strType, objTran);
                    if (intNoOfRows > 0)
                    {
                        intNoOfRows = daoLog.LogActivationInfo(intAcctId, intCertId, intSystemId, objTran);
                        if (intNoOfRows > 0)
                        {
                            //add distributor-cert mapping in links table

                            UserInfo objUserInf = new User().GetUserInfo(intAcctId, strSession);
                            if (objUserInf.GetUserCompanyId() != -1)
                            {
                                if (objUserInf.GetUserRole().Equals(UserType.Distributor))
                                {
                                    strType = CertType.DistributorCertificate;
                           
                                    intNoOfRows = daoCert.AssignCertificate(objUserInf.GetUserCompanyId(), intCertId, strType, objTran);
                                    if (intNoOfRows == 0)
                                    {
                                        blnTrnSuccess = false;
                                    }
                                }
                            }

                        }
                        else
                        {
                            blnTrnSuccess = false;
                        }
                    }
                    else
                    {
                        blnTrnSuccess = false;
                    }


                }
                else
                {
                    blnTrnSuccess = false;
                }



            }

            catch (Exception ex)
            {
                new Log().logException(meth, ex);
                blnTrnSuccess = false;
            }
            return blnTrnSuccess;
        }

       public bool UpdateUpgradeStat(string srcFru, string destFru)
        {
            return daoCert.UpdateUpgradeStat(srcFru, destFru);
        }

    public bool UpdateActivationInfo(DataTable dt, int intAcctId, string strType)
    {
        string meth = "UpdateActivationInfo: MulitpleCerts";
        SqlConnection objSConn = new SqlConnection(strSQLConn);
        SqlTransaction objTran;
        objSConn.Open();
        objTran = objSConn.BeginTransaction();
        bool retVal = true;
        try
        {

            int intCertId, intSystemId;
            string strActivationCode = string.Empty;
            int NoofRowsAffected = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strActivationCode = string.Empty;
                intCertId = Int32.Parse(dt.Rows[i][Certificate.LIC_ID].ToString());
                intSystemId = Int32.Parse(dt.Rows[i][Certificate.SYS_ID].ToString());
                strActivationCode = dt.Rows[i][Certificate.ACTIVATION_KEY].ToString();
                if (UpdateActivationInfo(intAcctId, intCertId, intSystemId, strActivationCode, strType, objTran))
                    NoofRowsAffected++;

            }

            if (NoofRowsAffected == dt.Rows.Count)
            {
                objTran.Commit();
                retVal = true;
            }
            else
            {
                objTran.Rollback();
                retVal = false;
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

    public bool UpdateActivationInfo(int intAcctId, int intCertId, int intSystemId, string strActivationCode, string strType, SqlTransaction objTran)
    {
        string meth = "Certificate:UpdateActivationInfo";
        bool blnTrnSuccess = true;

        int intNoOfRows = 0;
        try
        {
            intNoOfRows = daoCert.UpdateActivationInfo(intAcctId,intCertId, intSystemId, strActivationCode, objTran);
            if (intNoOfRows > 0)
            {

                intNoOfRows = daoLog.LogTransferInfo(intAcctId, intCertId, intSystemId, objTran);
                if (intNoOfRows == 0)
                {
                    blnTrnSuccess = false;
                }
                if (!((UserInfo)Session["USER_INFO"]).GetImpersonateUserRole().Contains(ConfigurationManager.AppSettings["ARUBA_ROLE"].ToString()))
                {
                    intNoOfRows = daoLog.UpdateTransferCount(intCertId, objTran);
                    if (intNoOfRows == 0)
                    {
                        blnTrnSuccess = false;
                    }
                }
            }
            else
            {
                blnTrnSuccess = false;
            }
        }

        catch (Exception ex)
        {
            new Log().logException(meth, ex);
            blnTrnSuccess = false;
        }

        return blnTrnSuccess;
    }

        public bool AddMAC(string strMAC, string strBrand)
        {
            string meth = "Certificate:AddMAC";
            bool blnSuccess = false;
            
           
            int intNoOfRows = 0;
            try
            {
                intNoOfRows = daoCert.AddMAC(strMAC, strBrand);
                if (intNoOfRows > 0)
                {
                    blnSuccess= true;
                }
                
            }

            catch (Exception ex)
            {
                new Log().logException(meth, ex);
                blnSuccess = false;
            }
               
            
            return blnSuccess;

        }

        public DataSet GetAllCerts(string strEmail,int intPageSize, int intPageNum, string Filter,string strbrand)
        {
            return daoCert.GetAllCerts(strEmail,intPageSize, intPageNum, Filter,strbrand);
        }

        public DataSet GetAllQucikConnectCerts(string strEmail, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetAllQucikConnectCerts(strEmail, intPageSize, intPageNum, Filter, strbrand);
        }
        public DataSet GetUnassignedCerts(int intCompanyId,int intPageSize, int intPageNum, string Filter)
        {
            return daoCert.GetUnassignedCerts(intCompanyId,intPageSize, intPageNum, Filter);
        }
        public DataSet GetUnassignedClpCerts(int intCompanyId, int intPageSize, int intPageNum, string Filter)
        {
            return daoCert.GetUnassignedClpCerts(intCompanyId, intPageSize, intPageNum, Filter);
        }
        public DataSet GetUnassignedAWCerts(int intCompanyId, int intPageSize, int intPageNum, string Filter)
        {
            return daoCert.GetUnassignedAWCerts(intCompanyId, intPageSize, intPageNum, Filter);
        }        
        public DataSet GetAssignedCerts(int intCompanyId, int intPageSize, int intPageNum, string Filter)
        {
            return daoCert.GetAssignedCerts(intCompanyId, intPageSize, intPageNum, Filter);
        }
        public DataSet GetAssignedAWCerts(int intCompanyId, int intPageSize, int intPageNum, string Filter)
        {
            return daoCert.GetAssignedAWCerts(intCompanyId, intPageSize, intPageNum, Filter);
        }
        public DataSet GetAssignedCLPCerts(int intCompanyId, int intPageSize, int intPageNum, string Filter)
        {
            return daoCert.GetAssignedCLPCerts(intCompanyId, intPageSize, intPageNum, Filter);
        }        
        public DataSet GetCertsByAcct(int intAcctId,int intPageSize, int intPageNum, string Filter,string strbrand)
        {
            return daoCert.GetCertsByAcct(intAcctId,intPageSize, intPageNum, Filter,strbrand );
        }

        public DataSet GetQuickConnectCertsByAcct(int intAcctId, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetQuickConnectCertsByAcct(intAcctId, intPageSize, intPageNum, Filter, strbrand);
        }        

        public DataSet GetClpSubByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetClpSubByAcct(intAcctid, intPageSize, intPageNum, Filter, strbrand);
        }

        public DataSet GetAllClpSubscription(int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetAllClpSubscription(intPageSize, intPageNum, Filter, strbrand);
        }

        public DataSet GetClpCertsByAcct(int intAcctId, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetClpCertsByAcct(intAcctId, intPageSize, intPageNum, Filter, strbrand);
        }

        public DataSet GetAllClpCerts(int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetAllClpCerts(intPageSize, intPageNum, Filter, strbrand);
        }

        public DataSet GetFruByAcct(int intAcctId, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetFruByAcct(intAcctId, intPageSize, intPageNum, Filter, strbrand);
        }

        public DataSet GetEvalCertsByAcct(int intAcctId, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetEvalCertsByAcct(intAcctId, intPageSize, intPageNum, Filter, strbrand);
        }

        public DataSet GetRMACertsByAcct(int intAcctId, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetRMACertsByAcct(intAcctId, intPageSize, intPageNum, Filter, strbrand);
        }

        public DataSet GetAllRMACerts(int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetAllRMACerts(intPageSize, intPageNum, Filter, strbrand);
        }
        
        public DataSet GetActiveCertsByAcct(int intAcctId, int intPageSize, int intPageNum, string Filter, string strBrand)
        {
            return daoCert.GetActiveCertsByAcct(intAcctId, intPageSize, intPageNum, Filter,strBrand);
        }

        public DataSet GetActivePermCertsByAcct(int intAcctId, int intPageSize, int intPageNum, string Filter, string strBrand)
        {
            return daoCert.GetActivePermCertsByAcct(intAcctId, intPageSize, intPageNum, Filter, strBrand);
        }
        
        public DataSet GetEvalParts(string strBrand ,int intPageSize, int intPageNum, string Filter)
        {
            return daoCert.GetEvalParts(strBrand,intPageSize, intPageNum, Filter);
        }

        public bool ReassignCertificates(int intOldAcctId,int intDestAcctID,string[] arrCertId,string srcCertype)
        {
            return daoCert.ReassignCertificates(intOldAcctId,intDestAcctID, string.Join(",", arrCertId), srcCertype);
        }

        public DataSet GetAccountsForAirwCert(string strSerialNumber)
        {
            return daoCert.GetAccountsForAirwCert(strSerialNumber);
        }

        public bool UpdateAlcatelCert(DataTable dt)
        {
            bool blResult = false;
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
               DataRow dr = dt.Rows[i];
               blResult = daoCert.UpdateAlcatelCert(dr);
               if (blResult == false)
                {
                    objTran.Rollback();
                    return blResult;
                }
            }
            objTran.Commit();
            return blResult;
        }

        public bool CheckOwnership(int intAcctId, int intCertId, string Type)
        {
            return daoCert.CheckOwnership(intAcctId, intCertId, Type);
        }

        public bool CheckOwnershipNew(int intAcctId, int intCertId, string Type)
        {
            return daoCert.CheckOwnershipNew(intAcctId, intCertId, Type);
        }
        
        public bool CheckPOEOwnership(int intAcctId, int intFruId, string Type)
        {
            return daoCert.CheckPOEOwnership(intAcctId, intFruId, Type);
        }

        public DataSet CheckTransferCount(int intAcctId, int intCertId, string action)
        {
            return daoCert.CheckTransferCount(intAcctId, intCertId, action);
        }

        public DataSet CheckTransferCountNew(int intCertId)
        {
            return daoCert.CheckTransferCountNew(intCertId);
        }        

        public bool CheckAWTransferCount(int intAcctId, int intCertId, string action)
        {
            return daoCert.CheckAWTransferCount(intAcctId, intCertId, action);
        }

        public string GenerateActivationKey(KeyGenInput objKeyGenIp)
        {
            KeyGen.ArubaLicensing objKeygen = new KeyGen.ArubaLicensing();
            objKeygen.Url = ConfigurationManager.AppSettings["KeyGen.URL"];
            KeyGen.ComplexType1 objKGInp = new KeyGen.ComplexType1();
            string strActivationCode = string.Empty;

            if (string.Empty.Equals(objKeyGenIp.CertSerialNumber)
                || string.Empty.Equals(objKeyGenIp.SystemSerialNumber)
                || string.Empty.Equals(objKeyGenIp.Brand))
                return string.Empty;

            objKGInp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
            objKGInp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString(); 
            objKGInp.brand = objKeyGenIp.Brand;
            objKGInp.cert = objKeyGenIp.CertSerialNumber;
            objKGInp.fru = objKeyGenIp.SystemSerialNumber.ToUpper();
            strActivationCode = objKeygen.genActKey(objKGInp);

            return strActivationCode;        
        }

        public string GenerateUpgradeCerts(UpgradeCertInput objUpgradeCertIp)
        {
            KeyGenUp.ArubaLicensing objKeyGenUp = new KeyGenUp.ArubaLicensing();
            objKeyGenUp.Url = ConfigurationManager.AppSettings["KeyGenUp.URL"];
            KeyGenUp.ComplexType3 objKeygenUpIp = new KeyGenUp.ComplexType3();
            objKeygenUpIp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
            objKeygenUpIp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString();
            objKeygenUpIp.brand = objUpgradeCertIp.Brand;
            objKeygenUpIp.fru = objUpgradeCertIp.Fru;
            objKeygenUpIp.ifile = objUpgradeCertIp.Ifile;
            objKeygenUpIp.ofile = objUpgradeCertIp.Ofile;
            objKeygenUpIp.marg = objUpgradeCertIp.Marg;
            string strStatus = objKeyGenUp.UpgradeCertNew(objKeygenUpIp);
            return strStatus;
        }

        public string GenerateQAActivationKey(KeyGenInput objKeyGenIp)
        {
            KeyGenQA.ArubaLicensing objKeygen = new KeyGenQA.ArubaLicensing();
            objKeygen.Url = ConfigurationManager.AppSettings["KeyGenQA.URL"];
            KeyGenQA.ComplexType1 objKGInp = new KeyGenQA.ComplexType1();
            string strActivationCode = string.Empty;

            if (string.Empty.Equals(objKeyGenIp.CertSerialNumber)
                || string.Empty.Equals(objKeyGenIp.SystemSerialNumber)
                || string.Empty.Equals(objKeyGenIp.Brand))
                return string.Empty;

            objKGInp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
            objKGInp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString();
            objKGInp.brand = objKeyGenIp.Brand;
            objKGInp.cert = objKeyGenIp.CertSerialNumber;
            objKGInp.fru = objKeyGenIp.SystemSerialNumber.ToUpper();
            strActivationCode = objKeygen.genActKey(objKGInp);

            return strActivationCode;
        }

        public string GenerateVCQAActivationKey(KeyGenInput objKeyGenIp)
        {
            KeyGenQA.ArubaLicensing objKeygen = new KeyGenQA.ArubaLicensing();
            objKeygen.Url = ConfigurationManager.AppSettings["KeyGenQA.URL"];
            KeyGenQA.ComplexType1 objKGInp = new KeyGenQA.ComplexType1();
            string strActivationCode = string.Empty;

            if (string.Empty.Equals(objKeyGenIp.CertSerialNumber)
                || string.Empty.Equals(objKeyGenIp.SystemSerialNumber)
                || string.Empty.Equals(objKeyGenIp.Brand))
                return string.Empty;

            objKGInp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
            objKGInp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString();
            objKGInp.brand = objKeyGenIp.Brand;
            objKGInp.cert = objKeyGenIp.CertSerialNumber;
            objKGInp.fru = objKeyGenIp.SystemSerialNumber.ToUpper();
            objKGInp.passphrase = objKeyGenIp.PassPhrase;
            strActivationCode = objKeygen.genVCActKey(objKGInp);
            return strActivationCode;
        }

        public string GenerateFlexQAActivationKey(KeyGenInput objKeyGenIp)
        {
            KeyGenQA.ArubaLicensing objKeygen = new KeyGenQA.ArubaLicensing();
            objKeygen.Url = ConfigurationManager.AppSettings["KeyGenQA.URL"];
            KeyGenQA.ComplexType1 objKGInp = new KeyGenQA.ComplexType1();
            string strActivationCode = string.Empty;

            if (string.Empty.Equals(objKeyGenIp.CertSerialNumber)
                || string.Empty.Equals(objKeyGenIp.SystemSerialNumber)
                || string.Empty.Equals(objKeyGenIp.Brand))
                return string.Empty;

            objKGInp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
            objKGInp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString();
            objKGInp.brand = objKeyGenIp.Brand;
            objKGInp.cert = objKeyGenIp.CertSerialNumber;
            objKGInp.fru = objKeyGenIp.SystemSerialNumber.ToUpper();
            strActivationCode = objKeygen.genFlexActKey(objKGInp);

            return strActivationCode;
        }

        public string GenerateRFPAcgtivationKey(KeyGenInput objKeyGenIp)
        {

            RFPGenKey.NetChem objRFPGenKey = new RFPGenKey.NetChem();
            RFPGenKey.ComplexType1 objComplex1 = new RFPGenKey.ComplexType1();
            string strActivationCode = string.Empty;
            
            if (string.Empty.Equals(objKeyGenIp.CertSerialNumber)
                || string.Empty.Equals(objKeyGenIp.SystemSerialNumber)
                || string.Empty.Equals(objKeyGenIp.Brand))
                return string.Empty;

            RFPGenKey.ComplexType1 objInp = new RFPGenKey.ComplexType1();
            RFPGenKey.ComplexType2 objOut = new RFPGenKey.ComplexType2();
            objInp.authname  = ConfigurationManager.AppSettings["RFP_USER"];
            objInp.authpass = ConfigurationManager.AppSettings["RFP_PWD"];
            objInp.acctid = "";
            objInp.brand = objKeyGenIp.Brand;
            objInp.cert = objKeyGenIp.CertSerialNumber;
            objInp.fru = objKeyGenIp.SystemSerialNumber.ToUpper();

            objOut = objRFPGenKey.genKey(objInp);
            string strError = objOut.errmsg;
            strActivationCode = objOut.result;
            if (strActivationCode.Contains("<BR>"))
               strActivationCode = strActivationCode.Replace("<BR>", "  ");
            
            return strActivationCode; 
        }

        public string GenerateCertificate(KeyGenInput objKeyGenIp,String strBrand)
        {
            string strLicenseKey = string.Empty;
            Certificate objCert = new Certificate();
            string partId = objKeyGenIp.CertPartId;
            //string strBrand = Session["BRAND"].ToString();
            string partType = objCert.getPartType(partId, strBrand);
            if (partType == ConfigurationManager.AppSettings["RFP_TYPE"].ToString())
            {                
                RFPGenKey.NetChem objRFPKey = new RFPGenKey.NetChem();
                RFPGenKey.ComplexType1 objInp = new RFPGenKey.ComplexType1();
                RFPGenKey.ComplexType2 objOut = new RFPGenKey.ComplexType2();
                objInp.authname = ConfigurationManager.AppSettings["RFP_USER"];
                objInp.authpass = ConfigurationManager.AppSettings["RFP_PWD"];
                objInp.acctid="";
                objInp.brand=objKeyGenIp.Brand.ToUpper();
                objInp.cert = objKeyGenIp.CertSerialNumber;
                objInp.fru=objKeyGenIp.SystemSerialNumber.ToUpper();
                objOut = objRFPKey.genKey(objInp);
                strLicenseKey = objOut.result;
            
            }
            else if (partType == ConfigurationManager.AppSettings["ECS_TYPE"].ToString())
            {

            }
            else
            {
                KeyGenQA.ArubaLicensing objKeygen = new KeyGenQA.ArubaLicensing();
                objKeygen.Url = ConfigurationManager.AppSettings["KeyGenQA.URL"];
                KeyGenQA.ComplexType1 objKGInp = new KeyGenQA.ComplexType1();
               
                if (string.Empty.Equals(objKeyGenIp.CertPartId)
                    || string.Empty.Equals(objKeyGenIp.Brand))
                    return string.Empty;

                objKGInp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
                objKGInp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString(); 
                objKGInp.brand = objKeyGenIp.Brand;
                objKGInp.cert = objKeyGenIp.CertPartId;
                if (objKeyGenIp.UserCount != string.Empty)
                {
                    objKGInp.count = objKeyGenIp.UserCount;
                    strLicenseKey = objKeygen.getFlexCert(objKGInp);
                }
                else
                {
                    strLicenseKey = objKeygen.getCert(objKGInp);
                }
                

                if (strLicenseKey.Equals("0"))
                    strLicenseKey = string.Empty;
            }
            return strLicenseKey;

        }

        public string GenerateVCQACertificate(KeyGenInput objKeyGenIp, String strBrand)
        {
            string strLicenseKey = string.Empty;
            Certificate objCert = new Certificate();
            string partId = objKeyGenIp.CertPartId;
            //string strBrand = Session["BRAND"].ToString();
            KeyGenQA.ArubaLicensing objKeygen = new KeyGenQA.ArubaLicensing();
            objKeygen.Url = ConfigurationManager.AppSettings["KeyGenQA.URL"];
            KeyGenQA.ComplexType1 objKGInp = new KeyGenQA.ComplexType1();

            if (string.Empty.Equals(objKeyGenIp.CertPartId)
                || string.Empty.Equals(objKeyGenIp.Brand))
                 return string.Empty;

               objKGInp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
               objKGInp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString();
               objKGInp.brand = objKeyGenIp.Brand;
               objKGInp.cert = objKeyGenIp.CertPartId;
               strLicenseKey = objKeygen.getVCQACert(objKGInp);

                if (strLicenseKey.Equals("0"))
                    strLicenseKey = string.Empty;

            return strLicenseKey;
        }
        
        public string GenerateArubaCert(KeyGenInput objKeyGenIp)
        {
            string strLicenseKey = string.Empty;
                KeyGen.ArubaLicensing objKeygen = new KeyGen.ArubaLicensing();
                objKeygen.Url = ConfigurationManager.AppSettings["KeyGen.URL"];
                KeyGen.ComplexType1 objKGInp = new KeyGen.ComplexType1();
               

                if (string.Empty.Equals(objKeyGenIp.CertPartId)
                    || string.Empty.Equals(objKeyGenIp.Brand))
                    return string.Empty;

                objKGInp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
                objKGInp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString(); 
                objKGInp.brand = objKeyGenIp.Brand;
                objKGInp.cert = objKeyGenIp.CertPartId;
                strLicenseKey = objKeygen.getCert(objKGInp);
                if (strLicenseKey.Equals("0"))
                    strLicenseKey = string.Empty;

            return strLicenseKey;
    }

        public string DecodeMMSCertificate(KeyGenInput objKeyGenIp)
        {
           KeyGen.ArubaLicensing objKeygen = new KeyGen.ArubaLicensing();
            objKeygen.Url = ConfigurationManager.AppSettings["KeyGen.URL"];
            KeyGen.ComplexType1 objKGInp = new KeyGen.ComplexType1();
            string decCertType = string.Empty;

            if (string.Empty.Equals(objKeyGenIp.CertSerialNumber)
               || string.Empty.Equals(objKeyGenIp.Brand))
                return string.Empty;

            objKGInp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
            objKGInp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString();
            objKGInp.brand = objKeyGenIp.Brand;
            objKGInp.cert = objKeyGenIp.CertSerialNumber;
            decCertType = objKeygen.decodeCert(objKGInp);
            return decCertType;
        }

        public string DecodePassphrase(KeyGenInput objKeyGenIp)
        {
            string strPassphrase = string.Empty;
            KeyGenQA.ArubaLicensing objKeygen = new KeyGenQA.ArubaLicensing();
            objKeygen.Url = ConfigurationManager.AppSettings["KeyGenQA.URL"];
            KeyGenQA.ComplexType1 objKGInp = new KeyGenQA.ComplexType1();

            if (string.Empty.Equals(objKeyGenIp.SystemSerialNumber)
               || string.Empty.Equals(objKeyGenIp.PassPhrase))
                return string.Empty;                

            objKGInp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
            objKGInp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString();
            objKGInp.fru = objKeyGenIp.SystemSerialNumber;
            objKGInp.passphrase = objKeyGenIp.PassPhrase;
            strPassphrase = objKeygen.decodePassphrase(objKGInp);
            return strPassphrase;
        }

        public string DecodeActivation(KeyGenInput objkeygenIp)
        {
            KeyGen.ArubaLicensing objKeygen = new KeyGen.ArubaLicensing();
            //ActKeyGen.ArubaLicensing objKeygen = new KeyGen.ArubaLicensing();
            objKeygen.Url = ConfigurationManager.AppSettings["KeyGen.ArubaWSTest"];
            KeyGen.ComplexType1 objKGInp = new KeyGen.ComplexType1();
            string decCertType = string.Empty;

            if (string.Empty.Equals(objkeygenIp.CertSerialNumber)
               || string.Empty.Equals(objkeygenIp.Brand) || string.Empty.Equals(objkeygenIp.SystemSerialNumber))
                return string.Empty;

            objKGInp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
            objKGInp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString();
            objKGInp.brand = objkeygenIp.Brand;
            objKGInp.activationcode = objkeygenIp.CertSerialNumber;
            objKGInp.fru = objkeygenIp.SystemSerialNumber.ToUpper();
            decCertType = objKeygen.decodeActivation(objKGInp);
            return decCertType;
        }

        public string GenerateEvalCertificate(KeyGenInput objKeyGenIp)
        { 
        //generate certifcate
            KeyGen.ArubaLicensing objKeygen = new KeyGen.ArubaLicensing();
            objKeygen.Url = ConfigurationManager.AppSettings["KeyGen.URL"];
            KeyGen.ComplexType1 objKGInp = new KeyGen.ComplexType1();
            string strActivationCode = string.Empty;

            if (string.Empty.Equals(objKeyGenIp.CertSerialNumber)
                || string.Empty.Equals(objKeyGenIp.Brand))
                return string.Empty;

            objKGInp.authname = ConfigurationManager.AppSettings["KEYGEN_USER"].ToString();
            objKGInp.authpass = ConfigurationManager.AppSettings["KEYGEN_PASS"].ToString();
            objKGInp.brand = objKeyGenIp.Brand;
            objKGInp.cert = objKeyGenIp.CertSerialNumber;
             strActivationCode = objKeygen.getCert(objKGInp);

            return strActivationCode;
        }

        public bool AddEvalCertificate(string strPartId, string strSerialNum, string strSoldTo, string brand, string certType)
        {
            return daoCert.AddEvalCertificate(strPartId,strSerialNum, ConfigurationManager.AppSettings["SOLD_TO_CUST"].ToString(), strSoldTo, strSoldTo, strSoldTo, brand,certType);
        }

        public string getCertVersion(string strCertpartid)
        {
            return daoCert.getCertVersion(strCertpartid);
        }

        public bool AddQACertificate(string strPartId,string strPartDesc, string strSerialNum, string strSoldTo,string strShipTo,string strSOId,string UserId,string IpAddr,string method,string strBrand,string strVersion)
        {
            string meth = "AddQACertificate";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
            try 
            { 
            if(daoCert.AddSerialNumber(strPartId, strPartDesc, strSerialNum, strSoldTo, strSoldTo, strShipTo, strShipTo,objTran,strBrand,strVersion))
            {
                string sql = "AddSerialNumber("+strPartId+","+ strPartDesc+","+  strSerialNum+","+  strSoldTo+","+  strSoldTo+","+  strShipTo+","+  strShipTo+")";
                if (!daoCert.AddTransactionRecord(UserId, sql, IpAddr, method, objTran))
                {
                     retVal=false;
                }
            
            }
            else
            {
             retVal=false;
            }
            
            if (retVal==true)
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
        public bool UnassignCertificate(int CertId,int ResellerId,int AcctId)
        {
            string meth = "UnassignCertificate:Break Reseller-Cert relation";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
            int noOfRowsaffected = -1;
            string type = CertType.CompanyCertificate;//CertType.ResellerCertificate;
            try
            {
                noOfRowsaffected = daoCert.UnassignCertificate(CertId, ResellerId, type, objTran);
                if (noOfRowsaffected > 0)
                {
                    noOfRowsaffected = daoLog.LogUnassignInfo(AcctId, CertId, ResellerId, objTran);
                    if (noOfRowsaffected == 0)
                    {
                        retVal = false;
                    }
                }
                else
                {
                    retVal = false;
                }

                if (retVal==true)
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

        public bool UnassignAWCertificate(int CertId, int ResellerId, int AcctId)
        {
            string meth = "UnassignCertificate:Break Reseller-Cert relation";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
            int noOfRowsaffected = -1;
            string type = CertType.CompanyCertificate;//CertType.ResellerCertificate;
            try
            {
                noOfRowsaffected = daoCert.UnassignAWCertificate(CertId, ResellerId, type, objTran);
                if (noOfRowsaffected > 0)
                {
                    noOfRowsaffected = daoLog.LogUnassignAWInfo(AcctId, CertId, ResellerId, objTran);
                    if (noOfRowsaffected == 0)
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

        public bool UnassignClpCertificate(int CertId, int ResellerId, int AcctId)
        {
            string meth = "UnassignCertificate:Break Reseller-Cert relation";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool retVal = true;
            int noOfRowsaffected = -1;
            string type = CertType.CompanyCertificate;//CertType.ResellerCertificate;
            try
            {
                noOfRowsaffected = daoCert.UnassignClpCertificate(CertId, ResellerId, type, objTran);
                if (noOfRowsaffected > 0)
                {
                    noOfRowsaffected = daoLog.LogUnassignClpInfo(AcctId, CertId, ResellerId, objTran);
                    if (noOfRowsaffected == 0)
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

        public bool IsCTOedCert(string CertificateSerialNumber)
        {
            return daoCert.IsCTOedCert(CertificateSerialNumber, CertType.AccountCertificate);
        }         
        
        public bool IsCTOedPOECert(string SerialNumber)
        {
            return daoCert.IsCTOedPOECert(SerialNumber, CertType.AccountCertificate);      
        }

        public bool ImportCTOedCerts(string CertificateSerialNumber, int AcctId)
        {
            DataSet ds  =  GetCertInfo(CertificateSerialNumber);
            if (ds == null)
                return false;
            else
            {
                string SOId = ds.Tables[0].Rows[0]["so_id"].ToString();
                string fru = ds.Tables[0].Rows[0]["fru"].ToString();
                return daoCert.ImportCTOedCerts(SOId, AcctId, Int32.Parse(ConfigurationManager.AppSettings["CTO_ACCT"]), fru);
            }
        }

        public bool ImportCTOedPOECerts(string FruSerialNumber, int AcctId)
        {
            DataSet ds = GetCertInfo(FruSerialNumber);
            if (ds == null)
                return false;
            else
            {
                string SOId = ds.Tables[0].Rows[0]["so_id"].ToString();
                return daoCert.ImportCTOedPOECerts(SOId, AcctId, Int32.Parse(ConfigurationManager.AppSettings["CTO_ACCT"]), FruSerialNumber);
            }
        }
        
        public DataSet GetSearchResults(int intPageSize, int intPageNum, string Filter, string ShowCerts, bool showSOID, string brand, string certType)
        {
            return daoCert.GetSearchResults(intPageSize, intPageNum, Filter, ShowCerts, showSOID,brand,certType);
        }

        public DataSet GetHistorySearchResults(int intPageSize, int intPageNum, string Filter, string ShowCerts, bool showSOID, string brand, string certType)
        {
            return daoCert.GetHistorySearchResults(intPageSize, intPageNum, Filter, ShowCerts, showSOID, brand, certType);
        }

        public bool AssignAWCertsToResellers(int AcctId, DataTable dtCertReseller)
        {
            string meth = "AssignCertsToResellers:Add certificate-reseller relation";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool blnTrnSuccess = true;
            int noOfRowsaffected = -1;
            string type = CertType.CompanyCertificate;// CertType.ResellerCertificate;
            try
            {
                for (int i = 0; i < dtCertReseller.Rows.Count; i++)
                {
                    noOfRowsaffected = daoCert.AssignAWCertificate(Int32.Parse(dtCertReseller.Rows[i][CI_RESELLER_ID].ToString()), Int32.Parse(dtCertReseller.Rows[i][CI_CERT_ID].ToString()), type, objTran);
                    if (noOfRowsaffected > 0)
                    {
                        noOfRowsaffected = daoLog.LogAssignAWInfo(AcctId, Int32.Parse(dtCertReseller.Rows[i][CI_CERT_ID].ToString()), Int32.Parse(dtCertReseller.Rows[i][CI_RESELLER_ID].ToString()), objTran);
                        if (noOfRowsaffected == 0)
                        {
                            blnTrnSuccess = false;
                            break;
                        }
                    }
                    else
                    {
                        blnTrnSuccess = false;
                        break;
                    }

                }
                if (blnTrnSuccess)
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
                blnTrnSuccess = false;
            }
            finally
            {
                objTran.Dispose();
                if (objSConn != null && objSConn.State == ConnectionState.Open)
                    objSConn.Close();
            }
            return blnTrnSuccess;
        }

        public bool AssignClpCertsToResellers(int AcctId, DataTable dtCertReseller)
        {
            string meth = "AssignCertsToResellers:Add certificate-reseller relation";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool blnTrnSuccess = true;
            int noOfRowsaffected = -1;
            string type = CertType.CompanyCertificate;// CertType.ResellerCertificate;
            try
            {
                for (int i = 0; i < dtCertReseller.Rows.Count; i++)
                {
                    noOfRowsaffected = daoCert.AssignClpCertificate(Int32.Parse(dtCertReseller.Rows[i][CI_RESELLER_ID].ToString()), Int32.Parse(dtCertReseller.Rows[i][CI_CERT_ID].ToString()), type, objTran);
                    if (noOfRowsaffected > 0)
                    {
                        noOfRowsaffected = daoLog.LogAssignClpInfo(AcctId, Int32.Parse(dtCertReseller.Rows[i][CI_CERT_ID].ToString()), Int32.Parse(dtCertReseller.Rows[i][CI_RESELLER_ID].ToString()), objTran);
                        if (noOfRowsaffected == 0)
                        {
                            blnTrnSuccess = false;
                            break;
                        }
                    }
                    else
                    {
                        blnTrnSuccess = false;
                        break;
                    }

                }
                if (blnTrnSuccess)
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
                blnTrnSuccess = false;
            }
            finally
            {
                objTran.Dispose();
                if (objSConn != null && objSConn.State == ConnectionState.Open)
                    objSConn.Close();
            }
            return blnTrnSuccess;
        }

        public bool AssignCertsToResellers(int AcctId, DataTable dtCertReseller)
        {
            string meth = "AssignCertsToResellers:Add certificate-reseller relation";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            bool blnTrnSuccess = true;
            int noOfRowsaffected = -1;
            string type = CertType.CompanyCertificate;// CertType.ResellerCertificate;
            try
            {
                for (int i = 0; i < dtCertReseller.Rows.Count; i++)
                {
                    noOfRowsaffected = daoCert.AssignCertificate(Int32.Parse(dtCertReseller.Rows[i][CI_RESELLER_ID].ToString()), Int32.Parse(dtCertReseller.Rows[i][CI_CERT_ID].ToString()), type, objTran);
                    if (noOfRowsaffected > 0)
                    {
                        noOfRowsaffected = daoLog.LogAssignInfo(AcctId, Int32.Parse(dtCertReseller.Rows[i][CI_CERT_ID].ToString()), Int32.Parse(dtCertReseller.Rows[i][CI_RESELLER_ID].ToString()), objTran);
                        if (noOfRowsaffected == 0)
                        {
                            blnTrnSuccess = false;
                            break;
                        }
                    }
                    else
                    {
                        blnTrnSuccess = false;
                        break;
                    }
                
                }
                if (blnTrnSuccess)
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
                blnTrnSuccess = false;
            }
            finally
            {
                objTran.Dispose();
                if (objSConn != null && objSConn.State == ConnectionState.Open)
                    objSConn.Close();
            }
            return blnTrnSuccess;
        }

        public DataSet GetCertsAssignedToResellers(int intCompanyId, int intPageSize, int intPageNum, string Filter)
        {
            return daoCert.GetCertsAssignedToResellers(intCompanyId, intPageSize, intPageNum, Filter);
        }

        public DataSet GetClpCertsAssignedToResellers(int intCompanyId, int intPageSize, int intPageNum, string Filter)
        {
            return daoCert.GetClpCertsAssignedToResellers(intCompanyId, intPageSize, intPageNum, Filter);
        }

        public DataSet GetAWCertsAssignedToResellers(int intCompanyId, int intPageSize, int intPageNum, string Filter)
        {
            return daoCert.GetAWCertsAssignedToResellers(intCompanyId, intPageSize, intPageNum, Filter);
        }

        public DataSet GetAccountsForCert(string strSerialNumber, string strCertType)
        {
            return daoCert.GetAccountsForCert(strSerialNumber, strCertType);
        }

        //Added by Ashwini on July/25/2012

        public bool IsSubkeyImported(string subKey)
        {
            return daoCert.IsSubkeyImported(subKey);
        }

        //By Praveena - For Amigopod Integration
        public DataSet getAmigopodCertFromCertGen(string strAmigopodCert, string strBrand)
        {
            return daoCert.getAmigopodCertFromCertGen(strAmigopodCert, strBrand);
        }

        //By Praveena - For Amigopod Integration
        public DataSet getAmigopodCertDetails(string strAmigopodCert, string strBrand)
        {
            return daoCert.getAmigopodCertDet(strAmigopodCert, strBrand);
        }

        //By Praveena - For Amigopod Integration
        public bool IsAmigoppodCertActivated(string strCertId, string actionType)
        {
            return daoCert.IsAmigopodCertActivated(strCertId, actionType);
        }

        //By Praveena - For Amigopod Integration
        public DataSet GetAmigoppodLookInfo(string strCertId, string strBrand)
        {
            return daoCert.getAmigopodLookupInfo(strCertId, strBrand);
        }

        //Added by Ashwini on Mar/25/2013

        public bool IsAvendaSubKeyImported(string strSubkey)
        {
            return daoCert.IsAvendaSubKeyImported(strSubkey);
        }

        public bool IsClpLicenseImported(string ActivationKey, string PartId)
        {
            return daoCert.IsClpLicenseImported(ActivationKey, PartId);
        }

        public DataSet getClpCertFromCertGen(string strAmigopodCert, string strBrand)
        {
            return daoCert.getClpCertFromCertGen(strAmigopodCert, strBrand);
        }

        //Added by Ashwini on 19th/Jan/2014

        public string AddQuickConnect(string cert_id, string licenseCount, string expiry_date,string Name,string email,string company_name, string username, string Password,string Action)
        {
            AmigopodWS.Service objAmigopodWS = new AmigopodWS.Service();
            string status = ConfigurationManager.AppSettings["QUICK_STATUS"].ToString();
            return objAmigopodWS.ProcessQuickConnect(company_name, Name, email, Password, status, username, licenseCount, expiry_date, Action);
        }

        // Added by Ashwini on Mar/17/2013

        public AvendaXML ActivateClpCert(string cust_id, string avenda_sku, string productName, string sku_id, string subscription, string productVersion, string productSKU, string numUsers, string licenseType, string cert_id, string expYear)
        {
            //call amigopod web service to get the Subscription ID for the input.
            string strActivationKey = string.Empty;
            //assign  value to verify email. 
            AvendaXML objAvendaXML = new AvendaXML();

            try
            {
                AmigopodWS.Service objAmigopodWS = new AmigopodWS.Service();
                AmigopodWS.AvendaXML objAvendaResponse = objAmigopodWS.AddAvendaLicense(cust_id, avenda_sku, productName,
                sku_id, subscription, productVersion, productSKU, numUsers, licenseType, expYear);
                objAvendaXML.message = objAvendaResponse.message;
                objAvendaXML.license = objAvendaResponse.subscription;
                objAvendaXML.warning = objAvendaResponse.warning;
                objAvendaXML.error = objAvendaResponse.error;
                objAvendaXML.xmlparse_error = objAvendaResponse.xmlparse_error;
            }
            catch (Exception ex)
            {
                new Log().logException("Activate ClearPass Certificate", ex);
                objemail.sendAmigopodErrorMessage(ex.Message, "ActivateClpCert", cert_id, "");
            }

            return objAvendaXML;
        }

        //By Praveena - For Amigopod Integration
        public AmigopodXML GenerateAmigopodSubscription(string cert_id, string category, string soid, string poid, DateTime expDate, int licence_count, string company_name, string email, string do_guestconnect, string sku_id)
        {
            //call amigopod web service to get the subscription ID for the input.
            string strActivationKey = string.Empty;
            //assign  value to verify email. 
            AmigopodXML objAmigopodXml = new AmigopodXML();

            try
            {
                AmigopodWS.Service objAmigopodWS = new AmigopodWS.Service();
                String expiry = expDate.ToShortDateString();
                AmigopodWS.AmigopodXML objAmgResponse = objAmigopodWS.GetSubscriptionKey(company_name, category, licence_count.ToString(), expiry, email, "0", poid, soid, do_guestconnect, sku_id);

                objAmigopodXml.email = objAmgResponse.username;
                objAmigopodXml.message = objAmgResponse.message;
                objAmigopodXml.password = objAmgResponse.password;
                objAmigopodXml.subscription_key = objAmgResponse.subscription_key;
                objAmigopodXml.warning = objAmgResponse.warning;
                objAmigopodXml.error = objAmgResponse.error;
                objAmigopodXml.xmlparse_error = objAmgResponse.xmlparse_error;
            }
            catch (Exception ex)
            {
                new Log().logException("GenerateAmigopodSubscription", ex);
                objemail.sendAmigopodErrorMessage(ex.Message, "GenerateAmigopodSubscription",cert_id,"");
            }

            return objAmigopodXml;
        }

        //By Praveena - For Amigopod Integration
        public bool InsertAmigopodActkey(string subscriptionKey, string cert_id, string action, int user_account_id, int company_id, string comapny_name, string comments, int impersonatedBy,string expiry_date)
        {
            bool bResult = false;
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            string meth = "InsertAmigopodActkey";
            string CertId = string.Empty;
            int NoOfRows = 0;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            try
            {
                bResult = daoCert.InsertAmigopodActkey(subscriptionKey, cert_id, action, user_account_id, company_id, comapny_name, impersonatedBy, comments, objTran, expiry_date);
                if (bResult == true)
                    objTran.Commit();
                else
                    objTran.Rollback();

            }
            catch (Exception ex)
            {
                objTran.Rollback();
                new Log().logException(meth, ex);
                objemail.sendAmigopodErrorMessage(ex.Message, "InsertAmigopodActkey",cert_id,subscriptionKey);
            }
            finally
            {
                objTran.Dispose();
                if (objSConn != null && objSConn.State == ConnectionState.Open)
                    objSConn.Close();
            }
            return bResult;
        }

        //By Praveena - For Amigopod Integration
        public AmigopodXML UpgradeAmigopodSubscription(string subscription, string license,string certid)
        {
            AmigopodXML objAmigopodXml = new AmigopodXML();

            try
            {
                //call amigopod web service to update the subscription ID .
                AmigopodWS.Service objAmigopodWS = new AmigopodWS.Service();
                AmigopodWS.AmigopodXML objAmgResponse = objAmigopodWS.UpgradeLicense(subscription, license);

                objAmigopodXml.email = objAmgResponse.email;
                objAmigopodXml.message = objAmgResponse.message;
                objAmigopodXml.password = objAmgResponse.password;
                objAmigopodXml.subscription_key = objAmgResponse.subscription_key;
                objAmigopodXml.warning = objAmgResponse.warning;
                objAmigopodXml.error = objAmgResponse.error;
                objAmigopodXml.xmlparse_error = objAmgResponse.xmlparse_error;

            }
            catch (Exception ex)
            {
                new Log().logException("UpgradeAmigopodSubscription", ex);
                objemail.sendAmigopodErrorMessage(ex.Message, "UpgradeAmigopodSubscription",certid,subscription);
            }
            return objAmigopodXml;
        }

        public bool isValidClpSubscription(string subscriptionKey)
        {
            return daoCert.isValidClpSubscription(subscriptionKey);
        }

        //By Praveena - For Amigopod Integration
        public bool isValidAmigopodSubscription(string subscriptionKey)
        {
            return daoCert.isValidSubscription(subscriptionKey);
        }

        //By Praveena - For Amigopod Integration
        public AmigopodXML AddAdvertisingAmigopod(string subscriptionkey, string cert_id)
        {
            AmigopodXML objAmigopodXml = new AmigopodXML();
            try
            {
                //call amigopod web service to update the subscription ID .
                AmigopodWS.Service objAmigopodWS = new AmigopodWS.Service();
                AmigopodWS.AmigopodXML objAmgResponse = objAmigopodWS.AddAdvertising(subscriptionkey, cert_id);

                objAmigopodXml.email = objAmgResponse.email;
                objAmigopodXml.message = objAmgResponse.message;
                objAmigopodXml.password = objAmgResponse.password;
                objAmigopodXml.subscription_key = objAmgResponse.subscription_key;
                objAmigopodXml.warning = objAmgResponse.warning;
                objAmigopodXml.error = objAmgResponse.error;
                objAmigopodXml.xmlparse_error = objAmgResponse.xmlparse_error;
            }
            catch (Exception ex)
            {
                new Log().logException("AddAdvertisingAmigopodSubscription", ex);
                objemail.sendAmigopodErrorMessage(ex.Message, "isValidAmigopodSubscription",cert_id,subscriptionkey);
                
            }
            return objAmigopodXml;
        }

        //By Praveena - For Amigopod Integration
        public DataSet GetAmigopodSubscriptionByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetAmigopodSubscriptionByAcct(intAcctid, intPageSize, intPageNum, Filter, strbrand);
        }

        //By Praveena - For Amigopod Integration
        public DataSet GetAmigopodUpgradeDetails(string subscription, string brand)
        {
            return daoCert.GetAmigopodUpgradeDetails(subscription, brand);
        }

        //By Praveena - For Amigopod Integration
        public bool isEnabledAdvertisingPlugin(string subscriptionKey, string brand, string action_type)
        {
            return daoCert.isEnabledAdvertisingPlugin(subscriptionKey, brand,action_type);
        }

        //Added by Ashwini
        public bool IsLegacySubscription(string subscription)
        {
            Subscription objSub = new Subscription();
           objSub =  GetDetailsAmigopodSubscription(subscription,"1");
           if (objSub.license == string.Empty || objSub.license == null)
           {
               return false;
           }
           return true;
        }

        public bool IsAmigopodSubscription(string subscription)
        {
            Subscription objSub = new Subscription();
            objSub = GetDetailsAmigopodSubscription(subscription, "1");
            for (int i = 0; i < objSub.lstClsLicense.Count; i++)
            {
                if (objSub.lstClsLicense[i].sku_id.Contains(ConfigurationManager.AppSettings["AMIGOPOD_SKU_ID"].ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        //Added by Ashwini
        public bool IsEvaluationSubKey(string subscription)
        {
            Subscription objSub = new Subscription();
            objSub = GetDetailsAmigopodSubscription(subscription, "1");
            if (objSub.category == ConfigurationManager.AppSettings["AMG_EVAL"].ToString())
            {
                return true;
            }            
            return false;
        }

        // Added by Ashwini on 16/3/2013

        public DataTable ImportLegacyAvendaSubKey(DataTable dt, int AcctId, int ImpAcctId)
        {
            string meth = "ImportLegacyAvendaSubKey";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            Certificate objCert = new Certificate();
            DataTable dtResult = objCert.BuildAmgCertInfo();
            try
            {
                int NoofRowsAffected = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AvendaCert objAvendaCert = new AvendaCert(); 
                    string strCategory = string.Empty;                     
                    string strPart = string.Empty; string[] strArrPart; string[] strHAArrPart;
                    objAvendaCert.subscription = dt.Rows[i]["SubscriptionKey"].ToString();
                    objAvendaCert.CompanyName = dt.Rows[i]["Organization"].ToString();
                    objAvendaCert.activated_ts = dt.Rows[i]["create_time"].ToString();
                    objAvendaCert.expiryDate = dt.Rows[i]["expire_time"].ToString();
                    objAvendaCert.email = dt.Rows[i]["email"].ToString();
                    objAvendaCert.so = dt.Rows[i]["so_id"].ToString();
                    objAvendaCert.po = dt.Rows[i]["po_id"].ToString();
                    objAvendaCert.username = dt.Rows[i]["user_name"].ToString();
                    objAvendaCert.password = dt.Rows[i]["password"].ToString();

                    //if (objAvendaCert.expiryDate == string.Empty || objAvendaCert.expiryDate == "0")
                    //{
                    //    objAvendaCert.expiryDate = null;
                    //}
                    //if (objAvendaCert.activated_ts == string.Empty || objAvendaCert.activated_ts == "0")
                    //{
                    //    objAvendaCert.activated_ts = null;
                    //}

                    if (objAvendaCert.num_users == string.Empty)
                    {
                        objAvendaCert.num_users = "0";
                    }

                    strCategory = dt.Rows[i]["category"].ToString();
                    if (strCategory.ToUpper() == ConfigurationManager.AppSettings["AMG_HW"].ToString())
                    {
                        strPart = ConfigurationManager.AppSettings["CLS_HW_BASE"].ToString();
                        strArrPart = strPart.Split('|');
                        objAvendaCert.partId = strArrPart[0];
                        objAvendaCert.partDesc = strArrPart[1];
                    }
                    else
                    {
                        strPart = ConfigurationManager.AppSettings["CLS_SW_BASE"].ToString();
                        strArrPart = strPart.Split('|');
                        objAvendaCert.partId = strArrPart[0];
                        objAvendaCert.partDesc  = strArrPart[1];
                    }

                    DataSet dsCertResult = daoCert.GenerateClsCert(objAvendaCert.so, objAvendaCert.partId, objAvendaCert.expiryDate, Session["BRAND"].ToString(), objAvendaCert.num_users);
                    if (dsCertResult != null)
                    {
                        if (dsCertResult.Tables[0].Rows.Count > 0)
                        {
                            for (int index = 0; index < dsCertResult.Tables[0].Rows.Count; index++)
                            {
                                if (dsCertResult.Tables[0].Rows[index]["part_id"].ToString() == objAvendaCert.partId)
                                {
                                    objAvendaCert.CertId = dsCertResult.Tables[0].Rows[0]["certificate_id"].ToString();
                                    objAvendaCert.SerialNo = dsCertResult.Tables[0].Rows[0]["serial_number"].ToString();
                                }
                            }
                        }
                    }

                    if (!objAvendaCert.CertId.Equals(string.Empty))
                    {
                        if (ImportLegacyClsSubKey(objAvendaCert, AcctId, ImpAcctId, objTran))
                        {
                            DataRow drResult = dtResult.NewRow();
                            drResult["SubscriptionKey"] = objAvendaCert.subscription;
                            drResult["part_id"] = objAvendaCert.partId;
                            drResult["serial_number"] = objAvendaCert.CertId;
                            drResult["LSerial_number"] = objAvendaCert.SerialNo;
                            drResult["so_id"] = objAvendaCert.so;
                            drResult["po_id"] = objAvendaCert.po;
                            drResult["Organization"] = objAvendaCert.CompanyName;
                            drResult["user_name"] = objAvendaCert.username;
                            drResult["Password"] = objAvendaCert.password;
                            drResult["expire_time"] = objAvendaCert.expiryDate;
                            drResult["create_time"] = objAvendaCert.activated_ts;
                            drResult["email"] = objAvendaCert.email;
                            drResult["part_desc"] = objAvendaCert.partDesc; 
                            
                            dtResult.Rows.Add(drResult);
                            NoofRowsAffected++;
                        }
                    }
                }

                if (NoofRowsAffected == dt.Rows.Count)
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
            }
            finally
            {
                objTran.Dispose();
                if (objSConn != null && objSConn.State == ConnectionState.Open)
                    objSConn.Close();
            }
            return dtResult;

        }

        //Added by Ashwini on Jan/4/2014

        public DataTable ImportLegacyAvendaLicKey(DataTable dt, int AcctId, int ImpAcctId)
        {
            string meth = "ImportLegacyAvendaLicKey";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            Certificate objCert = new Certificate();
            DataTable dtResult = objCert.BuildClpCertInfo();
            try
            {
                //Is this license already imported to LMS?
                int countRows = 0;
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    if (dt.Rows[k]["IsImported"] == "yes")
                    {
                        countRows = countRows + 1;
                    }
                }

                if (countRows == dt.Rows.Count)
                {
                    return dtResult;
                }                

                int NoofRowsAffected = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AvendaClpLicense objAvendaClpLicense = new AvendaClpLicense();
                    string strCategory = string.Empty;
                    string strPart = string.Empty; 
                    //string[] strArrPart;

                    if (dt.Rows[i]["IsImported"].ToString() == "1")
                    {
                        continue;
                    }

                    objAvendaClpLicense.license_key = dt.Rows[i]["LicenseKey"].ToString();
                    objAvendaClpLicense.created_date = dt.Rows[i]["create_time"].ToString();
                    objAvendaClpLicense.expiry_date = dt.Rows[i]["expire_time"].ToString();
                    objAvendaClpLicense.part_id = dt.Rows[i]["part_id"].ToString();
                    objAvendaClpLicense.so = dt.Rows[i]["so_id"].ToString();
                    objAvendaClpLicense.po = dt.Rows[i]["po_id"].ToString();
                    objAvendaClpLicense.version = dt.Rows[i]["version"].ToString();
                    objAvendaClpLicense.subscription_key = dt.Rows[i]["subscription_key"].ToString();
                    objAvendaClpLicense.company_name = dt.Rows[i]["cust_name"].ToString();
                    objAvendaClpLicense.num_users = dt.Rows[i]["num_users"].ToString();
                    objAvendaClpLicense.brand = Session["BRAND"].ToString();
                    objAvendaClpLicense.user_name = dt.Rows[i]["user_name"].ToString();
                    objAvendaClpLicense.password = dt.Rows[i]["password"].ToString();

                    //if (objAvendaClpLicense.expiry_date == "0" || objAvendaClpLicense.expiry_date == string.Empty)
                    //{
                    //    objAvendaClpLicense.expiry_date = null;
                    //}

                    //if (objAvendaClpLicense.created_date == "0" || objAvendaClpLicense.created_date == string.Empty)
                    //{
                    //    objAvendaClpLicense.created_date = null;
                    //}

                    if (objAvendaClpLicense.num_users == string.Empty)
                    {
                        objAvendaClpLicense.num_users = "0";
                    }

                    DataSet dsAmgLookup = daoCert.getAmgLookupInfo(objAvendaClpLicense.part_id, Session["BRAND"].ToString());
                    if (dsAmgLookup.Tables.Count > 0)
                    {
                        if (dsAmgLookup.Tables[0].Rows.Count > 0)
                        {
                            objAvendaClpLicense.part_desc = dsAmgLookup.Tables[0].Rows[0]["part_desc"].ToString();
                        }
                    }
                    DataSet dsCertResult = daoCert.GenerateClsCert(objAvendaClpLicense.so, objAvendaClpLicense.part_id, objAvendaClpLicense.expiry_date, Session["BRAND"].ToString(), objAvendaClpLicense.num_users);
                    if (dsCertResult != null)
                    {
                        if (dsCertResult.Tables[0].Rows.Count > 0)
                        {
                            for (int index = 0; index < dsCertResult.Tables[0].Rows.Count; index++)
                            {
                                if (dsCertResult.Tables[0].Rows[index]["part_id"].ToString() == objAvendaClpLicense.part_id)
                                {
                                    objAvendaClpLicense.serial_number = dsCertResult.Tables[0].Rows[0]["certificate_id"].ToString();
                                    objAvendaClpLicense.Lserial_number = dsCertResult.Tables[0].Rows[0]["serial_number"].ToString();
                                }
                            }
                        }
                    }

                    if (!objAvendaClpLicense.serial_number.Equals(string.Empty))
                    {
                        if (ImportLegacyClsLicKey(objAvendaClpLicense, AcctId, ImpAcctId, objTran))
                        {
                            DataRow drResult = dtResult.NewRow();
                            drResult["licenseKey"] = objAvendaClpLicense.license_key;
                            drResult["part_id"] = objAvendaClpLicense.part_id;
                            drResult["part_desc"] = objAvendaClpLicense.part_desc;
                            drResult["serial_number"] = objAvendaClpLicense.serial_number;
                            drResult["LSerial_number"] = objAvendaClpLicense.Lserial_number;
                            drResult["version"] = objAvendaClpLicense.version;
                            drResult["create_time"] = objAvendaClpLicense.created_date;
                            drResult["expire_time"] = objAvendaClpLicense.expiry_date;
                            drResult["so_id"] = objAvendaClpLicense.so;
                            drResult["po_id"] = objAvendaClpLicense.po;
                            drResult["subscription_key"] = objAvendaClpLicense.subscription_key;
                            dtResult.Rows.Add(drResult);
                            NoofRowsAffected++;
                        }
                    }
                }
                objTran.Commit();                   
             
            }
            catch (Exception ex)
            {
                dtResult.Clear();
                objTran.Rollback();                
                new Log().logException(meth, ex);
            }
            finally
            {
                objTran.Dispose();
                if (objSConn != null && objSConn.State == ConnectionState.Open)
                    objSConn.Close();
            }
            return dtResult;

        }

        //Added by Ashwini on July/20/2012
        public DataTable ImportLegacySubKey(DataTable dt, int AcctId, int ImpAcctId)
        {
            string meth = "ImportLegacySubKey";
            SqlConnection objSConn = new SqlConnection(strSQLConn);
            SqlTransaction objTran;
            objSConn.Open();
            objTran = objSConn.BeginTransaction();
            Certificate objCert = new Certificate();
            DataTable dtResult = objCert.BuildAmgCertInfo();
            try
            {
                int NoofRowsAffected = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    AmigopodCert objAmigopodCert = new AmigopodCert(); string strCategory = string.Empty; string strLicenseCount = string.Empty; string strObLicenseCount = string.Empty; string strAdvertise = string.Empty; string strHAPart = string.Empty;
                    string strPart = string.Empty; string[] strArrPart; string[] strHAArrPart;
                    objAmigopodCert.SubKey = dt.Rows[i]["SubscriptionKey"].ToString();
                    objAmigopodCert.OrgName = dt.Rows[i]["Organization"].ToString();
                    objAmigopodCert.activated_ts = dt.Rows[i]["create_time"].ToString();
                    objAmigopodCert.expiryDate = dt.Rows[i]["expire_time"].ToString();
                    objAmigopodCert.email = dt.Rows[i]["email"].ToString();
                    objAmigopodCert.so = dt.Rows[i]["so_id"].ToString();
                    objAmigopodCert.po = dt.Rows[i]["po_id"].ToString();
                    objAmigopodCert.HASubKey = dt.Rows[i]["HASubscriptionKey"].ToString();
                    strLicenseCount = dt.Rows[i]["license_count"].ToString();
                    strObLicenseCount = dt.Rows[i]["onBoard_count"].ToString();
                    strAdvertise = dt.Rows[i]["advertising"].ToString();
                    strCategory = dt.Rows[i]["category"].ToString();                    
                    if (strCategory.ToUpper() == ConfigurationManager.AppSettings["AMG_HW"].ToString())
                    {
                        strPart = ConfigurationManager.AppSettings["AMG_HW_BASE"].ToString();
                        strArrPart = strPart.Split('|');
                        objAmigopodCert.partId = strArrPart[0];
                        objAmigopodCert.partDesc = strArrPart[1];
                        if (!objAmigopodCert.HASubKey.Equals(string.Empty))
                        {
                            strHAPart = ConfigurationManager.AppSettings["AMG_HW_BASER"].ToString();
                            strHAArrPart = strHAPart.Split('|');
                            objAmigopodCert.HAPartId = strHAArrPart[0];
                            objAmigopodCert.HAPartDesc = strHAArrPart[1];
                        }
                    }
                    else
                    {
                        strPart = ConfigurationManager.AppSettings["AMG_SW_BASE"].ToString();
                        strArrPart = strPart.Split('|');
                        objAmigopodCert.partId = strArrPart[0];
                        objAmigopodCert.partDesc = strArrPart[1];
                        if (!objAmigopodCert.HASubKey.Equals(string.Empty))
                        {
                            strHAPart = ConfigurationManager.AppSettings["AMG_SW_BASER"].ToString();
                            strHAArrPart = strHAPart.Split('|');
                            objAmigopodCert.HAPartId = strHAArrPart[0];
                            objAmigopodCert.HAPartDesc = strHAArrPart[1];
                        }
                    }

                    if (Int32.Parse(strLicenseCount) > 0)
                    {
                        Array.Clear(strArrPart, 0, 1);
                        strPart = string.Empty;
                        strPart = ConfigurationManager.AppSettings["AMG_LIC"].ToString();
                        strArrPart = strPart.Split('|');
                        objAmigopodCert.Lic_PartId = strArrPart[0];
                        objAmigopodCert.Lic_PartDesc = strArrPart[1];
                    }

                    if (Int32.Parse(strObLicenseCount) > 0)
                    {
                        Array.Clear(strArrPart, 0, 1);
                        strPart = string.Empty;
                        strPart = ConfigurationManager.AppSettings["AMG_OBLIC"].ToString();
                        strArrPart = strPart.Split('|');
                        objAmigopodCert.OnBoard_LicPartId = strArrPart[0];
                        objAmigopodCert.OnBoard_LicPartDesc = strArrPart[1];
                    }

                    if (!strAdvertise.Equals(string.Empty))
                    {
                        Array.Clear(strArrPart,0,1);
                        strPart = string.Empty;
                        strPart = ConfigurationManager.AppSettings["AMG_ADV"].ToString();
                        strArrPart = strPart.Split('|');
                        objAmigopodCert.Advert_PartId  = strArrPart[0];
                        objAmigopodCert.Advert_PartDesc = strArrPart[1];
                    }

                    //DataSet dsPartDet = daoCert.getAmgPartDet(stLicenseCount, Session["BRAND"].ToString());

                    DataSet dsCertResult = daoCert.GenerateAmgCert(objAmigopodCert.so, objAmigopodCert.partId, objAmigopodCert.expiryDate, Session["BRAND"].ToString(), objAmigopodCert.HAPartId, objAmigopodCert.Lic_PartId, objAmigopodCert.OnBoard_LicPartId, objAmigopodCert.Advert_PartId);
                    if (dsCertResult != null)
                    {
                        if (dsCertResult.Tables[0].Rows.Count > 1)
                        {
                            for (int index = 0; index < dsCertResult.Tables[0].Rows.Count; index++)
                            {
                                if (dsCertResult.Tables[0].Rows[index]["part_id"].ToString() == objAmigopodCert.partId)
                                {
                                    objAmigopodCert.CertId = dsCertResult.Tables[0].Rows[0]["certificate_id"].ToString();
                                    objAmigopodCert.SerialNo = dsCertResult.Tables[0].Rows[0]["serial_number"].ToString();
                                }
                                if (dsCertResult.Tables[0].Rows[index]["part_id"].ToString() == objAmigopodCert.HAPartId)
                                {
                                    objAmigopodCert.HACertId = dsCertResult.Tables[0].Rows[index]["certificate_id"].ToString();
                                    objAmigopodCert.HASerialNo = dsCertResult.Tables[0].Rows[index]["serial_number"].ToString();
                                }
                                if (dsCertResult.Tables[0].Rows[index]["part_id"].ToString() == objAmigopodCert.Lic_PartId)
                                {
                                    objAmigopodCert.Lic_CertId = dsCertResult.Tables[0].Rows[index]["certificate_id"].ToString();
                                    objAmigopodCert.Lic_SerialNo = dsCertResult.Tables[0].Rows[index]["serial_number"].ToString();
                                }
                                if (dsCertResult.Tables[0].Rows[index]["part_id"].ToString() == objAmigopodCert.OnBoard_LicPartId)
                                {
                                    objAmigopodCert.OnBoard_LicCertId = dsCertResult.Tables[0].Rows[index]["certificate_id"].ToString();
                                    objAmigopodCert.OnBoard_LicSerialNo = dsCertResult.Tables[0].Rows[index]["serial_number"].ToString();
                                }
                                if (dsCertResult.Tables[0].Rows[index]["part_id"].ToString() == objAmigopodCert.Advert_PartId)
                                {
                                    objAmigopodCert.Advert_CertId = dsCertResult.Tables[0].Rows[index]["certificate_id"].ToString();
                                    objAmigopodCert.Advert_SerialNo = dsCertResult.Tables[0].Rows[index]["serial_number"].ToString();
                                }
                            }
                        }
                    }

                    if (!objAmigopodCert.CertId.Equals(string.Empty))
                    {
                        if (ImportLegacySubKey(objAmigopodCert,AcctId,ImpAcctId, objTran))
                        {
                            DataRow drResult = dtResult.NewRow();
                            drResult["SubscriptionKey"] = objAmigopodCert.SubKey;
                            drResult["part_id"] = objAmigopodCert.partId;
                            drResult["serial_number"] = objAmigopodCert.CertId;
                            drResult["LSerial_number"] = objAmigopodCert.SerialNo;
                            drResult["HASubscriptionKey"] = objAmigopodCert.HASubKey;
                            drResult["HApart_id"] = objAmigopodCert.HAPartId;
                            drResult["HAserial_number"] = objAmigopodCert.HACertId;
                            drResult["HALserial_number"] = objAmigopodCert.HASerialNo;
                            drResult["so_id"] = objAmigopodCert.so;
                            drResult["po_id"] = objAmigopodCert.po;
                            drResult["Organization"] = objAmigopodCert.OrgName;
                            drResult["LicSerialNo"] = objAmigopodCert.Lic_SerialNo;
                            drResult["LicCertId"] = objAmigopodCert.Lic_CertId;
                            drResult["AdvSerialNo"] = objAmigopodCert.Advert_SerialNo;
                            drResult["AdvCertId"] = objAmigopodCert.Advert_CertId;
                            drResult["OnBoardSerialNo"] = objAmigopodCert.OnBoard_LicSerialNo;
                            drResult["OnBoardCertId"] = objAmigopodCert.OnBoard_LicCertId;

                            dtResult.Rows.Add(drResult);
                            NoofRowsAffected++;
                        }
                    }
                }

                if (NoofRowsAffected == dt.Rows.Count)
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
            }
            finally
            {
                objTran.Dispose();
                if (objSConn != null && objSConn.State == ConnectionState.Open)
                    objSConn.Close();
            }
            return dtResult;
        }

        public bool ImportLegacyClsSubKey(AvendaCert objAvendaCert, int IntAcctId, int ImpActId, SqlTransaction objTran)
        {
            string meth = "Certificate:ImportLegacyClsSubKey";
            bool blnTrnSuccess = true;
            try
            {
                blnTrnSuccess = daoCert.ImportLegacyClsSubKey(objAvendaCert, ConfigurationManager.AppSettings["CLEARPASS_IMPORT"].ToString(), IntAcctId, ImpActId, objTran);
            }

            catch (Exception ex)
            {
                new Log().logException(meth, ex);
                blnTrnSuccess = false;
            }

            return blnTrnSuccess;
        }

        //Added by Ashwini on Jan/4/2014

        public bool ImportLegacyClsLicKey(AvendaClpLicense objAvendaClpLicense, int IntAcctId, int ImpActId, SqlTransaction objTran)
        {
            string meth = "Certificate:ImportLegacyClsLicKey";
            bool blnTrnSuccess = true;
            try
            {
                if (objAvendaClpLicense.part_id.StartsWith("QC"))
                {
                    blnTrnSuccess = daoCert.ImportLegacyClsQCLicKey(objAvendaClpLicense, ConfigurationManager.AppSettings["CLEARPASS_IMPORT"].ToString(), IntAcctId, ImpActId, objTran);
                }
                else
                {
                    blnTrnSuccess = daoCert.ImportLegacyClsLicKey(objAvendaClpLicense, ConfigurationManager.AppSettings["CLEARPASS_IMPORT"].ToString(),                     IntAcctId, ImpActId, objTran);
                }
            }

            catch (Exception ex)
            {
                new Log().logException(meth, ex);
                blnTrnSuccess = false;
            }

            return blnTrnSuccess;
        }

        //Added by Ashwini on July/22/2012
        public bool ImportLegacySubKey(AmigopodCert objAmigopodCert,int IntAcctId,int ImpActId,SqlTransaction objTran)
        {
            string meth = "Certificate:ImportLegacySubKey";
            bool blnTrnSuccess = true;
            try
            {
                blnTrnSuccess = daoCert.ImportLegacySubKey(objAmigopodCert, ConfigurationManager.AppSettings["CLEARPASS_IMPORT"].ToString(), IntAcctId, ImpActId, objTran);
            }

            catch (Exception ex)
            {
                new Log().logException(meth, ex);
                blnTrnSuccess = false;
            }

            return blnTrnSuccess;
        }
       
        //By Praveena - For Amigopod Integration

        //Ashwini modified for ClearPass 
        public Subscription GetDetailsAmigopodSubscription(string subscription,string p)
        {
            Subscription objSubscription = new Subscription();
            try
            {
                //call amigopod web service to update the subscription ID .
                AmigopodWS.Service objAmigopodWS = new AmigopodWS.Service();
                AmigopodWS.AmigopodXML obj = objAmigopodWS.ProcessSubscription(subscription,p);

                objSubscription.subscription_key = obj.subscription_key;
                objSubscription.create_time = obj.create_time;
                objSubscription.expiretime = obj.expiretime;
                objSubscription.license = obj.cnt_guest;
                objSubscription.message = obj.message;
                objSubscription.name = obj.name;
                objSubscription.sms_credit = obj.sms_credit;
                objSubscription.sms_handler = obj.sms_handler;
                objSubscription.so = obj.so;
                objSubscription.po = obj.po;
                objSubscription.email = obj.email;
                objSubscription.error = obj.error;
                objSubscription.username = obj.username;
                objSubscription.password = obj.password;
                objSubscription.high_availability = obj.high_availability_msg;
                objSubscription.on_board_license = obj.cnt_onboard;
                objSubscription.adv_feature = obj.adv_feature_msg;
                objSubscription.category = obj.category;
                objSubscription.sms_count = obj.sms_count;
                objSubscription.high_avaialbility_key = obj.high_availability_key;
                //objSubscription.sku_id = obj.sku_id;
                objSubscription.cust_id = obj.cust_id;
                for (int i = 0;i < obj.ArrclsLicense.GetLength(0);i++)
                {
                    clsLicense objclsLicense = new clsLicense();
                    if (obj.ArrclsLicense[i] == null)
                    {
                        continue;
                    }
                    objclsLicense.part_id = obj.ArrclsLicense[i].part_id;
                    objclsLicense.sku_id = obj.ArrclsLicense[i].sku_id;
                    objclsLicense.orig_sku = obj.ArrclsLicense[i].orig_sku;
                    objclsLicense.version = obj.ArrclsLicense[i].version;
                    objclsLicense.expiry_date = obj.ArrclsLicense[i].expiry_date;
                    objclsLicense.created_date = obj.ArrclsLicense[i].created_date;
                    objclsLicense.license_key = obj.ArrclsLicense[i].license_key;
                    objclsLicense.part_desc = obj.ArrclsLicense[i].part_desc;
                    objSubscription.lstClsLicense.Add(objclsLicense);
                }
            }
            catch (Exception ex)
            {
                new Log().logException("GetDetailsAmigopodSubscription", ex);
                objemail.sendAmigopodErrorMessage(ex.Message, "GetDetailsAmigopodSubscription","",subscription);
            }
            return objSubscription;
        }

        // Added By Ashwini on April/06/2013

        public string getModuleDescription(string strModuleId)
        {
            return daoCert.getModuleDescription(strModuleId);
        }

        //By Praveena - For Amigopod Integration
        public string GetCompanyIncrementor(string company_name)
        {
            return daoCert.GetCompanyIncrementor(company_name);

        }

        //By Praveena - For Amigopod Integration
        public DataSet GetAmigopodSubscriptions(int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            return daoCert.GetAmigopodSubscriptions(intPageSize, intPageNum, Filter, strbrand);
        }

        //By Praveena - For Amigopod Integration
        public DataSet GetAccountsForAmigopodCert(string strSerialNumber)
        {
            return daoCert.GetAccountsForAmigopodCert(strSerialNumber);
        }

        public DataSet GetAccountsForALECert(string strSerialNumber)
        {
            return daoCert.GetAccountsForALECert(strSerialNumber);
        }

        //By Praveena - For Amigopod Integration
        public AmigopodXML AddHighAvailability(string subscriptionKey, string blAvailability,string cert_id)
        {
            AmigopodXML objAmigopodXml = new AmigopodXML();
            try
            {
                //call amigopod web service to update the subscription ID .
                AmigopodWS.Service objAmigopodWS = new AmigopodWS.Service();
                AmigopodWS.AmigopodXML objAmgResponse = objAmigopodWS.AddHighAvailabily(subscriptionKey, blAvailability);

                objAmigopodXml.email = objAmgResponse.email;
                objAmigopodXml.message = objAmgResponse.message;
                objAmigopodXml.password = objAmgResponse.password;
                objAmigopodXml.subscription_key = objAmgResponse.high_availability_key;
                objAmigopodXml.warning = objAmgResponse.warning;
                objAmigopodXml.error = objAmgResponse.error;
                objAmigopodXml.xmlparse_error = objAmgResponse.xmlparse_error;
            }
            catch (Exception ex)
            {
                new Log().logException("AddHighAvailability", ex);
                objemail.sendAmigopodErrorMessage(ex.Message, "AddHighAvailability",cert_id,subscriptionKey);
            }
            return objAmigopodXml;
        }

        //By Praveena - For Amigopod Integration
        public bool UpdateHAActkey(string ha_subscription_key, string prim_subscription_key,int user_id)
        {
            bool bResult = false;
            string meth = "UpdateHAActkey";
            string CertId = string.Empty;
            int NoOfRows = 0;
            try
            {
                bResult = daoCert.UpdateHAKey(ha_subscription_key, prim_subscription_key,user_id);
                
            }
            catch (Exception e)
            {
                new Log().logException(meth, e);
                objemail.sendAmigopodErrorMessage(e.Message, "UpdateHAActkey", "", prim_subscription_key);
            }
            
            return bResult;
        }

        //By Praveena - For Amigopod Integration
        public AmigopodXML AddOnBoardLicense(string subscriptionKey, string p,string cert_id)
        {
            AmigopodXML objAmigopodXml = new AmigopodXML();

            try
            {
                //call amigopod web service to update the subscription ID .
                AmigopodWS.Service objAmigopodWS = new AmigopodWS.Service();
                AmigopodWS.AmigopodXML objAmgResponse = objAmigopodWS.AddOnBoardLicense(subscriptionKey, p);

                objAmigopodXml.email = objAmgResponse.email;
                objAmigopodXml.message = objAmgResponse.message;
                objAmigopodXml.password = objAmgResponse.password;
                objAmigopodXml.subscription_key = objAmgResponse.subscription_key;
                objAmigopodXml.warning = objAmgResponse.warning;
                objAmigopodXml.error = objAmgResponse.error;
                objAmigopodXml.xmlparse_error = objAmgResponse.xmlparse_error;

            }
            catch (Exception ex)
            {
                new Log().logException("AddOnBoardLicense", ex);
                objemail.sendAmigopodErrorMessage(ex.Message, "AddOnBoardLicense",cert_id,subscriptionKey);
            }
            return objAmigopodXml;
        }

        //By Praveena - For Amigopod Integration
        //to get the company name from accounts
        public string GetCompanyInfoByEmail(string email)
        {
            string company = string.Empty;
            try
            {
                company = daoCert.GetCompanyInfoByEmail(email);
            }
            catch (Exception ex) 
            {
               new Log().logException("GetCompanyInfoByEmail", ex);
               objemail.sendAmigopodErrorMessage(ex.Message + " ;email : "+email, "GetCompanyInfoByEmail","","");
            }
            return company;
        }

        public AmigopodXML GenerateClsEval(string name, string category, string email, string do_enterprise, string do_policy_manager, string do_guestconnect, string disabled, string policy_manager_licenseType, string enterprise_licenseType)
        {
            AmigopodXML objAmigopodXML = new AmigopodXML();
            AmigopodWS.Service objAmigopodWS = new AmigopodWS.Service();
            try
            {
                AmigopodWS.AmigopodXML objAmgResponse = objAmigopodWS.ProcessClpEval(name, category, email, do_enterprise, do_policy_manager, do_guestconnect, disabled, policy_manager_licenseType, enterprise_licenseType);
                objAmigopodXML.error = objAmgResponse.error;
                objAmigopodXML.xmlparse_error = objAmgResponse.xmlparse_error;
                objAmigopodXML.policy_manager = objAmgResponse.policy_manager;
                objAmigopodXML.enterprise = objAmgResponse.enterprise;
                objAmigopodXML.subscription_key = objAmgResponse.subscription_key;
                objAmigopodXML.expiry_time = objAmgResponse.expiretime;
                objAmigopodXML.user_name = objAmgResponse.username;
                objAmigopodXML.password = objAmgResponse.password;
            }
            catch (Exception ex)
            {
                new Log().logException("GenerateClsEval", ex);
                objemail.sendAmigopodErrorMessage(ex.Message, "GenerateClsEval", category, "");
            }
            return objAmigopodXML;
        }

        public string GetHaSubscription(string subscription,string action_type)
        {
            string ha_sub_key = string.Empty;
            try {
                ha_sub_key = daoCert.GetHASubscription(subscription,action_type);
            }
            catch (Exception ex)
            {
                new Log().logException("GetHaSubscription", ex);
                objemail.sendAmigopodErrorMessage(ex.Message, "GetHaSubscription", "", subscription);
            }
            return ha_sub_key;
        }

        //By Praveena - For Amigopod Integration
        //changed for OnBoard Sub key : 24/08/2012
        //To genearte and insert free sub key for OnBoard cert.
        public bool GenerateFreeSubkey(string subscriptionKey, string cert_id, string action, int userAcctId, int company, string company_name, int ImpAcctId, string expiry_date, string email,string so, string po)
        {
            string defaultPart = string.Empty;
            string[] arrPart;
            string part_id = string.Empty;
            string part_desc = string.Empty;
            string CertId = string.Empty;
            string SerialNo = string.Empty;

            defaultPart = ConfigurationManager.AppSettings["AMG_SW_OP"].ToString();
            arrPart = defaultPart.Split('|');
            part_id = arrPart[0];
            part_desc = arrPart[1];

            //Commented on 27/08/12 - to change the logic of Free Sub key generation
            /*DataSet dsCertResult = daoCert.GenerateAmgCert(so, part_id, expiry_date, Session["BRAND"].ToString(), "");
            if (dsCertResult != null)
            {
                if ((dsCertResult.Tables[0].Rows.Count == 1))
                {
                    CertId = dsCertResult.Tables[0].Rows[0]["certificate_id"].ToString();
                    SerialNo = dsCertResult.Tables[0].Rows[0]["serial_number"].ToString();
                }
            }
            */

            string meth = "Certificate:GenerateFreeSubkey";
            bool blnTrnSuccess = false;
            //if (!CertId.Equals(string.Empty))
            if (!cert_id.Equals(string.Empty))
            {
                try
                {
                    //blnTrnSuccess = daoCert.GenerateFreeSubKey(subscriptionKey, DateTime.Now.ToString(), userAcctId, company_name, company, ImpAcctId, expiry_date, action, email, so, po,CertId, SerialNo, part_id, part_desc);
                    blnTrnSuccess = daoCert.GenerateFreeSubKey(subscriptionKey, DateTime.Now.ToString(), userAcctId, company_name, company, ImpAcctId, expiry_date, action, email, so, po,cert_id, SerialNo, part_id, part_desc);
                }

                catch (Exception ex)
                {
                    new Log().logException(meth, ex);
                    blnTrnSuccess = false;
                    objemail.sendAmigopodErrorMessage(ex.Message, meth, CertId, subscriptionKey);
                }
            }
            else
            {
                blnTrnSuccess = false;
                objemail.sendAmigopodErrorMessage("Could not generate Certificate Id for free Subscription ID", meth, CertId, subscriptionKey);
            }
            return blnTrnSuccess;
        }

        //changed for OnBoard Sub key : 24/08/2012
        public DataSet GetAmigoppodLookInfo(string strCertId, string strBrand,string ignorePartId)
        {
            return daoCert.getAmigopodLookupInfo(strCertId, strBrand,ignorePartId);
        }

        //Added for License Matrix : By Praveena 24/09/2012
        public DataSet GetLicenseMatrix(int PageSize, int PageNumber, string filter, string strbrand)
        {
            return daoCert.GetLicenseMatrix(PageSize, PageNumber, filter, strbrand);
        }

        // Added by Ashwini on Feb/26/2013

        public DataSet GetClearPassCertDet(string strCertId,string strBrand)
        {
            return daoCert.GetClearPassCertDet(strCertId, strBrand);
        }

        public DataSet IsClsCertActivated(string strCertId)
        {
            return daoCert.IsClsCertActivated(strCertId);
        }

        public AvendaXML GeneratePolicyMgrLicense(string subscription,string DoPolicy,string PolicyUserCount, string PolicyType,string PolicyVersion, string strCertId)
        {
            AvendaXML objAvendaXML = new AvendaXML();
            AmigopodWS.Service objAmigopodWS = new AmigopodWS.Service();
            try
            {
                AmigopodWS.AvendaXML objAmgResponse = objAmigopodWS.AddPolicyManager(subscription, DoPolicy, PolicyUserCount, PolicyType, PolicyVersion);
                objAvendaXML.license = objAmgResponse.license;
                objAvendaXML.error = objAmgResponse.error;
                objAvendaXML.xmlparse_error = objAmgResponse.xmlparse_error;
            }
            catch (Exception ex)
            {
                new Log().logException("GenerateClsPolicyMgr", ex);
                objemail.sendAmigopodErrorMessage(ex.Message, "GenerateClsPolicyMgr", strCertId, "");
            }
             return objAvendaXML;
        }

        // Added by Ashwini on Mar/23/2013 - For ClearPass Integration
        public AvendaXML GenerateClsSubscription(string cert_id, string category, string soid, string poid, DateTime expDate, int licence_count, string company_name, string email, string sku_id, string do_guestconnect)
        {
            //call amigopod web service to get the subscription ID for the input.
            string strActivationKey = string.Empty;
            //assign  value to verify email. 
            AvendaXML objAvendaXML = new AvendaXML();

            try
            {
                AmigopodWS.Service objAmigopodWS = new AmigopodWS.Service();
                String expiry = expDate.ToShortDateString();
                AmigopodWS.AmigopodXML objAmgResponse = objAmigopodWS.GetSubscriptionKey(company_name, category, licence_count.ToString(), expiry, email, "0", poid, soid, do_guestconnect,sku_id);

                objAvendaXML.user_name = objAmgResponse.username;
                objAvendaXML.message = objAmgResponse.message;
                objAvendaXML.password = objAmgResponse.password;
                objAvendaXML.subscription_key = objAmgResponse.subscription_key;
                objAvendaXML.warning = objAmgResponse.warning;
                objAvendaXML.error = objAmgResponse.error;
                objAvendaXML.xmlparse_error = objAmgResponse.xmlparse_error;
            }
            catch (Exception ex)
            {
                new Log().logException("GenerateClsSubscription", ex);
                objemail.sendAmigopodErrorMessage(ex.Message, "GenerateClearPassSubscription", cert_id, "");
            }

            return objAvendaXML;
        }

    public bool InsertClsSubcription(string subscriptionKey, string strCertId, int ImpUserAcctId, string CompanyName, int AcctId, string CustomerEmail, string expDate, string strActKey, string strVersion)
    {
        return daoCert.InsertClsSubcription(subscriptionKey, strCertId, ImpUserAcctId, CompanyName, AcctId, CustomerEmail, expDate, strActKey, strVersion);
    }

    //ALE License
    public DataSet GetALECertByAcct(int intAcctId, int intPageSize, int intPageNum, string Filter, string strbrand)
    {
        return daoCert.GetALECertByAcct(intAcctId, intPageSize, intPageNum, Filter, strbrand);
    }

    public string getALECertificate(string strSerialNumber)
    {
        return daoCert.getALECertificate(strSerialNumber);
    }

    public DataSet getALECertGen(string strSerialNumber)
    {
        return daoCert.getALECertGen(strSerialNumber);
    }

    //ALE License
    public DataSet getALECertDet(string strAleCert, string strBrand)
    {
        return daoCert.getALECertDet(strAleCert, strBrand);
    }

    //ALE License
    public bool IsALECertActivated(string strCertId)
    {
        return daoCert.IsALECertActivated(strCertId);
    }

     public string GenerateALEActivation(string strOrder, string strIPAddress, string strPart, string strOrg, int APCount, string strSerialNo, string strProduct, string strPackage)
    {
        ALEKeyGen.ALE objALE = new ALEKeyGen.ALE();
        objALE.Url = ConfigurationManager.AppSettings["ALE_URL"];
        string strActivationKey = objALE.GetActivationKey(strOrder, strPart,strProduct, strPackage, strOrg, strIPAddress, APCount, strSerialNo);
        return strActivationKey;
    }

    //ALE license
        public bool InsertALEActkey(string strActivationKey, string strIPAddress, string strEvalkey, string strOrganizationName, int AcctId, int ImpAcctId)
    {
        bool blResult = false;
        SqlConnection objSConn = new SqlConnection(strSQLConn);
        SqlTransaction objTran;
        string meth = "InsertALEActkey";
        string strCertId = string.Empty;
        int intNoOfRows = 0;
        objSConn.Open();
        objTran = objSConn.BeginTransaction();
        try
        {
            blResult = daoCert.InsertALEActkey(strActivationKey, strIPAddress, strEvalkey, strOrganizationName, AcctId,  ImpAcctId,objTran);
            if (blResult == true)
            {
                intNoOfRows = daoLog.LogAirwaveActivationInfo(AcctId, strEvalkey, strIPAddress, objTran);
            }
            if (intNoOfRows > 0)
            {
                objTran.Commit();
            }
            else
            {
                objTran.Rollback();
            }
        }
        catch (Exception e)
        {
            objTran.Rollback();
            new Log().logException(meth, e);
        }
        finally
        {
            objTran.Dispose();
            if (objSConn != null && objSConn.State == ConnectionState.Open)
                objSConn.Close();
        }
        return blResult;
    }

    public bool InsertClpEvalInfo(string subscriptionKey, int ImpUserAcctId, int AcctId, string CompanyName, string CustomerEmail, Double expDate, string PolicyLic, string EnterpriseLic, string userName, string Password, string brand)
    {
        return daoCert.InsertClpEvalInfo(subscriptionKey, ImpUserAcctId, AcctId, CompanyName, CustomerEmail, expDate, PolicyLic, EnterpriseLic, userName, Password, brand);
    }

    public bool IsClpEvalGenerated(string strCompanyName)
    {
        return daoCert.IsClpEvalGenerated(strCompanyName);
    }

    public bool RestrictGenerateClpEvalCert(string strCompanyname, string strEmail, string strBrand)
    {
        return daoCert.RestrictGenerateClpEvalCert(strCompanyname, strEmail, strBrand);
    }

    }

    public sealed class QuickConnect
    {
        public string username = string.Empty;
        public string password = string.Empty;
        public string expiryDate = string.Empty;
        public string certId = string.Empty;
        public string licenseCount = string.Empty;
        public string name = string.Empty;
        public string email = string.Empty;
        public string company_name = string.Empty;
        public string so_id = string.Empty;
        public string po_id = string.Empty;
        public int pkCertid = 0;
        public string part_id = string.Empty;
        public string QCInstance_type = string.Empty;
        public string cert_id = string.Empty;
    }

    public sealed class AmigopodCert
    {
        public string SubKey = string.Empty;
        public string OrgName = string.Empty;
        public string activated_ts = string.Empty;
        public string expiryDate = string.Empty;
        public string email = string.Empty;
        public string po = string.Empty;
        public string so = string.Empty;
        public string CertId = string.Empty;
        public string SerialNo = string.Empty;
        public string partId = string.Empty;
        public string partDesc = string.Empty;             
        public string HASubKey = string.Empty;
        public string HACertId = string.Empty;
        public string HAPartId = string.Empty;
        public string HAPartDesc = string.Empty;
        public string HASerialNo = string.Empty;        
        public string Lic_PartId = string.Empty;
        public string Lic_PartDesc = string.Empty;
        public string Lic_CertId = string.Empty;
        public string Lic_SerialNo = string.Empty;
        public string OnBoard_LicPartId = string.Empty;
        public string OnBoard_LicPartDesc = string.Empty;
        public string OnBoard_LicSerialNo = string.Empty;
        public string OnBoard_LicCertId = string.Empty;
        public string Advert_PartId = string.Empty;
        public string Advert_PartDesc = string.Empty;
        public string Advert_SerialNo = string.Empty;
        public string Advert_CertId = string.Empty;
    }

    public sealed class AvendaCert
    {        
        public string CompanyName = string.Empty;
        public string activated_ts = string.Empty;
        public string expiryDate = string.Empty;
        public string ComapnyEmail = string.Empty;
        public string po = string.Empty;
        public string so = string.Empty;
        public string CertId = string.Empty;
        public string SerialNo = string.Empty;
        public string partId = string.Empty;
        public string partDesc = string.Empty;
        public string CompanyId = string.Empty;

        public string email = string.Empty;
        public string password = string.Empty;
        public string message = string.Empty;
        public string warning = string.Empty;
        public string subscription = string.Empty;
        public string xmlparse_error = string.Empty;
        public string license = string.Empty;
        public string error = string.Empty;
        public string name = string.Empty;
        public string expiretime = string.Empty;
        public string username = string.Empty;
        public string create_time = string.Empty;
        public string category = string.Empty;
        public string customer_sku_id = string.Empty;
        public string brand = string.Empty;
        public string version = string.Empty;
        public string num_users = string.Empty;
    }

    public sealed class AvendaClpLicense
    {
        public string part_id = string.Empty;
        public string license_key = string.Empty;
        public string version = string.Empty;
        public string created_date = string.Empty;
        public string expiry_date = string.Empty;
        public string part_desc = string.Empty;
        public string serial_number = string.Empty;
        public string Lserial_number = string.Empty;
        public string email = string.Empty;
        public string so = string.Empty;
        public string po = string.Empty;
        public string subscription_key = string.Empty;
        public string company_name = string.Empty;
        public string brand = string.Empty;
        public string num_users = string.Empty;
        public string user_name = string.Empty;
        public string password = string.Empty;
    }

    public sealed class CertType
        {                       
         public const string  AccountCertificate ="account_certificate";// "account_certificate";
         public const string   ResellerCertificate="account_certificate";//"reseller_certificate";
         public const string DistributorCertificate ="account_certificate";// "distributor_certificate";
         public const string CompanyCertificate = "company_certificate";
        }

    public sealed class KeyGenInput
    {        
        public  string CertSerialNumber = string.Empty;
        public string SystemSerialNumber = string.Empty;
        public string Brand = string.Empty;
        public string CertPartId = string.Empty;
        public string UserCount = string.Empty;
        public string PassPhrase = string.Empty;

        public KeyGenInput(string CertSerialNum,string SystemSerialNum,string UserBrand)
        {
            CertSerialNumber = CertSerialNum;
            SystemSerialNumber = SystemSerialNum;
            Brand = UserBrand;
        }

        public KeyGenInput(string CertSerialNum, string SystemSerialNum, string UserBrand, string Passphrase)
        {
            CertSerialNumber = CertSerialNum;
            SystemSerialNumber = SystemSerialNum;
            Brand = UserBrand;
            PassPhrase = Passphrase;
        }

      /*  public KeyGenInput(string CertSerialNum, string SystemSerialNum, string UserBrand, string CertPartNum)
        {
            CertSerialNumber = CertSerialNum;
            SystemSerialNumber = SystemSerialNum;
            Brand = UserBrand;
            CertPartId = CertPartNum;
        } */

        public KeyGenInput(string CertSerialNum,  string UserBrand)
        {
            CertSerialNumber = CertSerialNum;
            SystemSerialNumber = string.Empty;
            Brand = UserBrand;
        }

        public KeyGenInput()
        {
            CertSerialNumber = string.Empty;
            SystemSerialNumber = string.Empty;
            Brand = string.Empty;
            UserCount = string.Empty;
            PassPhrase = string.Empty;
        }

    }

    public sealed class UpgradeCertInput
    {
        public string Brand = string.Empty;
        public string Ifile = string.Empty;
        public string Fru = string.Empty;
        public string Ofile = string.Empty;
        public string Marg = string.Empty;

        public UpgradeCertInput(string strBrand, string strIfile, string strFru, string strOfile, string strMarg)
        {
            Brand = strBrand;
            Ifile = strIfile;
            Fru = strFru;
            Ofile = strOfile;
            Marg = strMarg;
        }
    }

    public sealed class RowType
    {
        public const string Certificate = "CERT";
        public const string System = "FRU";
    }
}
