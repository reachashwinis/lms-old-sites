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
using System.Data.Odbc;
using System.Collections.Generic;
using Com.Arubanetworks.Licensing.Lib.Utils;
using Com.Arubanetworks.Licensing.Lib.Data;

/// <summary>
/// Summary description for DACertificate
/// </summary>
namespace Com.Arubanetworks.Licensing.Dataaccesslayer
{
    public class DACertificate
    {
        string strODBCConn = string.Empty;
        string strSQLConn = string.Empty;
        string strCertsConn = string.Empty;
        string strSOFSNConn = string.Empty;

        public DACertificate()
        {
            strODBCConn = ConfigurationManager.ConnectionStrings["KEYGEN"].ToString();
            strSQLConn = ConfigurationManager.ConnectionStrings["LMSDB"].ToString();
            strCertsConn = ConfigurationManager.ConnectionStrings["CERTSDB"].ToString();
            strSOFSNConn = ConfigurationManager.ConnectionStrings["SOFSNDB"].ToString();
        }

        public DataSet GetPartMapEntry(string strCertificateSerialNumber)
        {
            DataSet dsPartMap = new DataSet();
            SqlParameter spParam = new SqlParameter("@certificate_id", SqlDbType.VarChar, 100);
            spParam.Value = strCertificateSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            dsPartMap = SqlHelper.ExecuteDataset(new SqlConnection(strCertsConn), CommandType.StoredProcedure, "dbo.sp_GetSerialNo", lstParam.ToArray());
            if (dsPartMap != null)
            {
                if (dsPartMap.Tables.Count > 0)
                {
                    dsPartMap.Tables[0].TableName = "PART_MAP";
                    if (dsPartMap.Tables[0].Rows.Count > 0)
                        return dsPartMap;
                }
            }
            return null;
        }

        public DataSet GetSerialNumberCertifcateMap(string strSerialNumber)
        {
            DataSet dsPartMap = new DataSet();
            SqlParameter spParam = new SqlParameter("@serial_number", SqlDbType.VarChar, 100);
            spParam.Value = strSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            dsPartMap = SqlHelper.ExecuteDataset(new SqlConnection(strCertsConn), CommandType.StoredProcedure, "dbo.sp_GetCertificate", lstParam.ToArray());
            if (dsPartMap != null)
            {
                if (dsPartMap.Tables.Count > 0)
                {
                    dsPartMap.Tables[0].TableName = "REV_PART_MAP";
                    if (dsPartMap.Tables[0].Rows.Count > 0)
                        return dsPartMap;
                }
            }
            return null;
        }

        public string getAlcatelPartNo(string strPartNo)
        {
            DataSet dsResult = new DataSet();
            SqlParameter spParam = new SqlParameter("@Part_id", SqlDbType.VarChar, 100);
            spParam.Value = strPartNo;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            dsResult = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetAlcatelPartNo", lstParam.ToArray());
            if (dsResult != null)
            {
                if (dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                        return dsResult.Tables[0].Rows[0]["AlcatelRoHSComp3EM"].ToString();
                }
            }
            return string.Empty;
        }

        public bool AddUploadInfo(string strUploadfile, string strSavefile, int FileSize, int UserId)
        {
            SqlParameter spParam = new SqlParameter("@upload_file", SqlDbType.VarChar, 500);
            spParam.Value = strUploadfile;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@saved_file", SqlDbType.VarChar, 500);
            spParam.Value = strSavefile;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@file_size", SqlDbType.Int);
            spParam.Value = FileSize;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@user_id", SqlDbType.Int);
            spParam.Value = UserId;
            lstParam.Add(spParam);

            int Result = SqlHelper.ExecuteNonQuery(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "dbo.sp_AddUploadFileInfo", lstParam.ToArray());
            if (Result > 0)
            {
                return true;
            }
            return false;
        }

        //Added on Jan 28 2014
        public string getQuickConnectPassword(string strUserName)
        {
            DataSet dsQCert = new DataSet();
            SqlParameter spParam = new SqlParameter("@user_name", SqlDbType.VarChar, 100);
            spParam.Value = strUserName;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            dsQCert = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetQCUserName", lstParam.ToArray());
            if (dsQCert != null)
            {
                if (dsQCert.Tables.Count > 0)
                {
                    if (dsQCert.Tables[0].Rows.Count > 0)
                        return dsQCert.Tables[0].Rows[0]["password"].ToString();
                }
            }
            return string.Empty;
        }

        public DataSet GetQCDetails(string userName, string brand)
        {
            SqlParameter spParam = new SqlParameter("@userName", SqlDbType.VarChar, 200); spParam.Value = userName;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = brand;
            lstParam.Add(spParam);

            DataSet dsParts = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getQCDetailsByCompany", lstParam.ToArray());
            return dsParts;
        }

        public string getAWCertificate(string strSerialNumber)
        {
            DataSet dsAWCert = new DataSet();
            SqlParameter spParam = new SqlParameter("@serial_number", SqlDbType.VarChar, 100);
            spParam.Value = strSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            dsAWCert = SqlHelper.ExecuteDataset(new SqlConnection(strCertsConn), CommandType.StoredProcedure, "sp_GetAWCertificate", lstParam.ToArray());
            if (dsAWCert != null)
            {
                if (dsAWCert.Tables.Count > 0)
                {
                    if (dsAWCert.Tables[0].Rows.Count > 0)
                        return dsAWCert.Tables[0].Rows[0]["LicKey"].ToString();
                }
            }
            return string.Empty;
        }

        public string getAWLicenseKey(string strSerialNumber)
        {
            DataSet dsAWCert = new DataSet();
            SqlParameter spParam = new SqlParameter("@serial_number", SqlDbType.VarChar, 100);
            spParam.Value = strSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            dsAWCert = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "sp_GetAWLicenseKey", lstParam.ToArray());
            if (dsAWCert != null)
            {
                if (dsAWCert.Tables.Count > 0)
                {
                    if (dsAWCert.Tables[0].Rows.Count > 0)
                        return dsAWCert.Tables[0].Rows[0]["LicKey"].ToString();
                }
            }
            return string.Empty;
        }

        public DataSet getPartDetails(string strpartId, string brand)
        {
            SqlParameter spParam = new SqlParameter("@part_id", SqlDbType.VarChar, 100); spParam.Value = strpartId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = brand;
            lstParam.Add(spParam);
            DataSet dsParts = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["CERTSDB"].ConnectionString, CommandType.StoredProcedure, "LMS_getAWPartDet", lstParam.ToArray());
            return dsParts;
        }

        public DataSet getAWCerDetails(string strCertSlNo, string strBrand)
        {
            DataSet dsAWCert = new DataSet();
            SqlParameter spParam = new SqlParameter("@serial_number", SqlDbType.VarChar, 100);
            spParam.Value = strCertSlNo;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20);
            spParam.Value = strBrand;
            lstParam.Add(spParam);

            dsAWCert = SqlHelper.ExecuteDataset(new SqlConnection(strCertsConn), CommandType.StoredProcedure, "sp_GetAWCertDet", lstParam.ToArray());
            if (dsAWCert != null)
            {
                return dsAWCert;
            }
            return null;
        }

        public DataSet getALECerDetails(string strCertSlNo, string strBrand)
        {
            DataSet dsALECert = new DataSet();
            SqlParameter spParam = new SqlParameter("@serial_number", SqlDbType.VarChar, 100);
            spParam.Value = strCertSlNo;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20);
            spParam.Value = strBrand;
            lstParam.Add(spParam);

            dsALECert = SqlHelper.ExecuteDataset(new SqlConnection(strCertsConn), CommandType.StoredProcedure, "sp_GetALECertDet", lstParam.ToArray());
            if (dsALECert != null)
            {
                return dsALECert;
            }
            return null;
        }

        public DataSet getAmgCertDetails(string strCertSlNo, string strBrand)
        {
            DataSet dsAmgCert = new DataSet();
            SqlParameter spParam = new SqlParameter("@serial_number", SqlDbType.VarChar, 100);
            spParam.Value = strCertSlNo;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20);
            spParam.Value = strBrand;
            lstParam.Add(spParam);

            dsAmgCert = SqlHelper.ExecuteDataset(new SqlConnection(strCertsConn), CommandType.StoredProcedure, "sp_GetAmgCertDet", lstParam.ToArray());
            if (dsAmgCert != null)
            {
                return dsAmgCert;
            }
            return null;
        }

        public bool AddEvalActivationInfo(DataTable dt, int UserId, string strIPAddress,string brand)
        {
            bool blResult = false;
            SqlParameter spParam = new SqlParameter("@CertPartId", SqlDbType.VarChar,100);
            spParam.Value = dt.Rows[0]["LicPartId"].ToString();
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 100);
            spParam.Value = dt.Rows[0]["LicSn"].ToString();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@SysPartId", SqlDbType.VarChar, 100);
            spParam.Value = dt.Rows[0]["SysPartId"].ToString();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@Fru", SqlDbType.VarChar, 20);
            spParam.Value = dt.Rows[0]["SysSn"].ToString();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@UserId", SqlDbType.Int);     
            spParam.Value = UserId;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@ActivationKey", SqlDbType.VarChar, 250);
            spParam.Value = dt.Rows[0]["ActivationKey"].ToString();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@IPAddress", SqlDbType.VarChar,20);
            spParam.Value = strIPAddress;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20);
            spParam.Value = brand;
            lstParam.Add(spParam);
            
            DataSet dsEvalcertInfo = new DataSet();
            dsEvalcertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_addEvalActivation", lstParam.ToArray());
            if (dsEvalcertInfo.Tables.Count > 0)
            {
                dsEvalcertInfo.Tables[0].TableName = "EVALCERT_INFO";
                if (dsEvalcertInfo.Tables[0].Rows.Count > 0)
                    blResult = true;
                    return blResult;
            }
            return blResult;
        }

        public string GetPartID(string strSerialNumber)
        {
            SqlParameter spParam = new SqlParameter("@serialnumber", SqlDbType.VarChar, 100);
            spParam.Value = strSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            DataSet dscertInfo = new DataSet();
            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetPartID", lstParam.ToArray());
            if (dscertInfo.Tables.Count > 0)
            {
                dscertInfo.Tables[0].TableName = "CERT_INFO";
                if (dscertInfo.Tables[0].Rows.Count > 0)
                    return dscertInfo.Tables[0].Rows[0]["part_id"].ToString();
            }
            return string.Empty;
        }

        public DataSet GetIAPInfo(string strSerialNo)
        {
            SqlParameter spParam = new SqlParameter("@SerialNumber", SqlDbType.VarChar, 100);
            spParam.Value = strSerialNo;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            DataSet dscertInfo = new DataSet();

            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSOFSNConn), CommandType.StoredProcedure, "getInfoBySerialNumber", lstParam.ToArray());
            if (dscertInfo.Tables.Count > 0)
            {
                dscertInfo.Tables[0].TableName = "FRU_INFO";
                if (dscertInfo.Tables[0].Rows.Count > 0)
                    return dscertInfo;
            }
            return null;
        }

        public DataSet GetCertificateInfo(string strCertificateSerialNumber)
        {
            SqlParameter spParam = new SqlParameter("@SerialNumber", SqlDbType.VarChar, 100);
            spParam.Value = strCertificateSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            DataSet dscertInfo = new DataSet();

            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getCertInfoBySerialNumber", lstParam.ToArray());
            if (dscertInfo.Tables.Count > 0)
            {
                dscertInfo.Tables[0].TableName = "CERT_INFO";
                if (dscertInfo.Tables[0].Rows.Count > 0)
                    return dscertInfo;
            }
            return null;
        }


        public DataSet GetClpCertificateInfo(string strCertificateSerialNumber)
        {
            SqlParameter spParam = new SqlParameter("@SerialNumber", SqlDbType.VarChar, 100);
            spParam.Value = strCertificateSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            DataSet dscertInfo = new DataSet();

            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getClpCertInfoBySerialNumber", lstParam.ToArray());
            if (dscertInfo.Tables.Count > 0)
            {
                dscertInfo.Tables[0].TableName = "CERT_INFO";
                if (dscertInfo.Tables[0].Rows.Count > 0)
                    return dscertInfo;
            }
            return null;
        }

        public DataSet getCertDetails(string strCertId)
        {
            SqlParameter spParam = new SqlParameter("@certId", SqlDbType.VarChar, 1000);
            spParam.Value = strCertId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            DataSet dscertInfo = new DataSet();

            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getCertDetails", lstParam.ToArray());
            if (dscertInfo.Tables.Count > 0)
            {
                dscertInfo.Tables[0].TableName = "CERT_INFO";
                if (dscertInfo.Tables[0].Rows.Count > 0)
                    return dscertInfo;
            }
            return null;
        }

        public DataSet GetArubaCertInfoForAlc(string strCertificateSerialNumber)
        {
            SqlParameter spParam = new SqlParameter("@SerialNumber", SqlDbType.VarChar, 100);
            spParam.Value = strCertificateSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            DataSet dscertInfo = new DataSet();

            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getArubaCertInfoForAlc", lstParam.ToArray());
            if (dscertInfo.Tables.Count > 0)
            {
                dscertInfo.Tables[0].TableName = "CERT_INFO";
                if (dscertInfo.Tables[0].Rows.Count > 0)
                    return dscertInfo;
            }
            return null;
        }

        public DataSet GetArubaCertInfoForAlcSKU(string strSystemSerialNo)
        {
            SqlParameter spParam = new SqlParameter("@SerialNumber", SqlDbType.VarChar, 100);
            spParam.Value = strSystemSerialNo;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            DataSet dscertInfo = new DataSet();

            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getArubaCertInfoForAlcFru", lstParam.ToArray());
            if (dscertInfo.Tables.Count > 0)
            {
                dscertInfo.Tables[0].TableName = "CERT_INFO";
                if (dscertInfo.Tables[0].Rows.Count > 0)
                    return dscertInfo;
            }
            return null;
        }

        public string AddMacAddress(string partId, string partdesc, string serialNumber, string brand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@partId", SqlDbType.VarChar,20); spParam.Value = partId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@partdesc", SqlDbType.VarChar,100); spParam.Value = partdesc;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@serialNumber", SqlDbType.VarChar, 100); spParam.Value = serialNumber;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = brand;
            lstParam.Add(spParam);

            DataSet dscertInfo = new DataSet();
            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_addMacAddress", lstParam.ToArray());
            if (dscertInfo.Tables.Count > 0)
            {
                dscertInfo.Tables[0].TableName = "CERT_INFO";
                if (dscertInfo.Tables[0].Rows.Count > 0)
                    return dscertInfo.Tables[0].Rows[0]["pk_cert_id"].ToString();
            }
            return null;
        } 

        public DataSet GetOPCertInfo(string strSerialNumber)
        {
            SqlParameter spParam = new SqlParameter("@SerialNumber", SqlDbType.VarChar, 800);
            spParam.Value = strSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            DataSet dscertInfo = new DataSet();

            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getOPSCertInfoBySerialNo", lstParam.ToArray());
            if (dscertInfo.Tables.Count > 0)
            {
                dscertInfo.Tables[0].TableName = "CERT_INFO";
                if (dscertInfo.Tables[0].Rows.Count > 0)
                    return dscertInfo;
            }
            return null;
        }

        public DataSet GetSystemInfo(string strSystemSerialNumber)
        {
            return GetCertificateInfo(strSystemSerialNumber);
        }

        public DataSet GetCertificateInfo(int intCertId)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@certId", SqlDbType.Int);
            spParam.Value = intCertId;
            lstParam.Add(spParam);
            DataSet dscertInfo = new DataSet();

            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getCertInfoById", lstParam.ToArray());
            if (dscertInfo.Tables.Count > 0)
            {
                dscertInfo.Tables[0].TableName = "CERT_INFO";
                if (dscertInfo.Tables[0].Rows.Count > 0)
                    return dscertInfo;
            }
            return null;
        }

        public DataSet GetCertInfoForFru(int intCertId, string strVersion)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@certId", SqlDbType.Int);
            spParam.Value = intCertId;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@version", SqlDbType.VarChar,20);
            spParam.Value = strVersion;
            lstParam.Add(spParam);
            DataSet dscertInfo = new DataSet();

            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getCertInfoForFru", lstParam.ToArray());
            if (dscertInfo.Tables.Count > 0)
            {
                dscertInfo.Tables[0].TableName = "CERT_INFO";
                if (dscertInfo.Tables[0].Rows.Count > 0)
                    return dscertInfo;
            }
            return null;
        }

        public DataSet GetSystemInfo(int intSystemId)
        {
            return GetCertificateInfo(intSystemId);
        }


        public DataSet GetConfigData(string strMacAddress)
        {
            SqlParameter spParam = new SqlParameter("@MacAdd", SqlDbType.VarChar, 50); spParam.Value = strMacAddress;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            DataSet dsConfigData = new DataSet();

            dsConfigData = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getConfigDataByMac", lstParam.ToArray());
            if (dsConfigData.Tables.Count > 0)
            {
                dsConfigData.Tables[0].TableName = "CFGDATA_INFO";
                if (dsConfigData.Tables[0].Rows.Count > 0)
                    return dsConfigData;
            }
            return null;

        }

        public string GetSerialOrMacReq(string strCertPartId,string strBrand)
        {
            SqlParameter spParam = new SqlParameter("@CertPartId", SqlDbType.VarChar, 50); spParam.Value = strCertPartId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            DataSet dsConfigData = new DataSet();

            dsConfigData = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getSerialOrMacReq", lstParam.ToArray());
            if (dsConfigData.Tables.Count > 0)
            {
                dsConfigData.Tables[0].TableName = "LOOKUP_INFO";
                if (dsConfigData.Tables[0].Rows.Count > 0)
                    return dsConfigData.Tables[0].Rows[0][0].ToString();
            }
            return string.Empty;
        }

        public bool IsCertSystemActivated(int intCertId, int intSystemId)
        {
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.Int); spParam.Value = intCertId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@SystemId", SqlDbType.Int); spParam.Value = intSystemId;
            lstParam.Add(spParam);
            DataSet dsCertSys = new DataSet();

            dsCertSys = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_checkForFruSystemCombo", lstParam.ToArray());
            if (dsCertSys.Tables.Count > 0)
            {
                dsCertSys.Tables[0].TableName = "CERTSYS_INFO";
                if (dsCertSys.Tables[0].Rows.Count > 0)
                    return true;
            }
            return false;
        }

        //upgrades 
        public string IsControllerUpgraded(int intSystemId)
        {
            //String UpgradpartId;
            SqlParameter spParam = new SqlParameter("@systemId", SqlDbType.Int); spParam.Value = intSystemId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            DataSet dsUpgradedCertSys = new DataSet();
            dsUpgradedCertSys = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_checkForControllerUpgrades", lstParam.ToArray());
            if (dsUpgradedCertSys.Tables.Count > 0)
            {
                dsUpgradedCertSys.Tables[0].TableName = "UPCERTSYS_INFO";
                if (dsUpgradedCertSys.Tables[0].Rows.Count > 0)
                {
                    return dsUpgradedCertSys.Tables[0].Rows[0]["rPart_id"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public bool IsCertSystemCompatible(string CertPartId, string SysPartId, string brand)
        {
            SqlParameter spParam = new SqlParameter("@CertPartId", SqlDbType.VarChar, 50); spParam.Value = CertPartId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@SystemPartId", SqlDbType.VarChar, 50); spParam.Value = SysPartId;
            lstParam.Add(spParam);
            DataSet dsCertSys = new DataSet();
            //Added for Aruba controllers
            if (brand == ConfigurationManager.AppSettings["ARUBA_BRAND"].ToString())
            {
                dsCertSys = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_checkCertSystemCompatible", lstParam.ToArray());
            }
            //Added for Alcatel controllers
            else
            {
                dsCertSys = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_checkCertSysAlcatelCompatible", lstParam.ToArray());
            }
            if (dsCertSys.Tables.Count > 0)
            {
                dsCertSys.Tables[0].TableName = "CERTSYS_INFO";
                if (int.Parse(dsCertSys.Tables[0].Rows[0]["countrow"].ToString()) > 0)
                    return true;
            }
            return false;
        }

        // Added by Ashwini on Mar/18/2013

        public int AddClpActivationInfo(int intCertId, int ImpAcctId, int ActId, string strActivationCode,string strLocation, string strVersion, string strSubKey, SqlTransaction objTrans)
        {
            SqlParameter spParam = new SqlParameter("@certID", SqlDbType.Int); spParam.Value = intCertId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@ImpActId", SqlDbType.Int); spParam.Value = ImpAcctId;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@ActId", SqlDbType.Int); spParam.Value = ActId;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@activationCode", SqlDbType.VarChar, 200); spParam.Value = strActivationCode;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@version", SqlDbType.VarChar, 50); spParam.Value = strVersion;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@Location", SqlDbType.VarChar, 200); spParam.Value = strLocation;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@SubKey", SqlDbType.VarChar, 200); spParam.Value = strSubKey;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_addClpActivationInfo", lstParam.ToArray());

            return noOfRowsAffected;
        }

        public int AddActivationInfo(int intCertId, int intSystemId, string strActivationCode, SqlTransaction objTrans)
        {
            SqlParameter spParam = new SqlParameter("@certID", SqlDbType.Int); spParam.Value = intCertId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@systemId", SqlDbType.Int); spParam.Value = intSystemId;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@activationCode", SqlDbType.VarChar, 200); spParam.Value = strActivationCode;
            lstParam.Add(spParam);


            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_addActivationInfo", lstParam.ToArray());

            return noOfRowsAffected;
        }

        public bool UpdateLocation(string strSerialNo, string strLocation, int intAcctId, SqlTransaction objTran)
        {
            int noOfRowsAffected = 0;
            SqlParameter spParam = new SqlParameter("@serialNo", SqlDbType.VarChar, 100); spParam.Value = strSerialNo;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@location", SqlDbType.VarChar, 100); spParam.Value = strLocation;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctId;
            lstParam.Add(spParam);
            noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTran, CommandType.StoredProcedure, "LMS_updateLocation", lstParam.ToArray());

            if (noOfRowsAffected == 0)
                return false;
            else
                return true;
        }

        public int UpdateActivationInfo(int AcctId, int intCertId, int intSystemId, string strActivationCode, SqlTransaction objTrans)
        {
            SqlParameter spParam = new SqlParameter("@certID", SqlDbType.Int); spParam.Value = intCertId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@systemId", SqlDbType.Int); spParam.Value = intSystemId;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@activationCode", SqlDbType.VarChar, 200); spParam.Value = strActivationCode;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = AcctId;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_updateActivationInfo", lstParam.ToArray());

            return noOfRowsAffected;
        }

        public int AddMAC(string strMAC, string strBrand)
        {
            SqlParameter spParam = new SqlParameter("@MAC", SqlDbType.VarChar, 255); spParam.Value = strMAC;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@Brand", SqlDbType.VarChar, 50); spParam.Value = strBrand;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_addMAC", lstParam.ToArray());

            return noOfRowsAffected;
        }

        public int AssignCertificate(int intAcctId, int intCertID, string strType, SqlTransaction objTrans)
        {
            SqlParameter spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = intAcctId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@CertId", SqlDbType.Int); spParam.Value = intCertID;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Type", SqlDbType.VarChar, 50); spParam.Value = strType;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_assignCertificates", lstParam.ToArray());

            return noOfRowsAffected;
        }

        public int AssignAWCertificate(int intAcctId, int intCertID, string strType, SqlTransaction objTrans)
        {
            SqlParameter spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = intAcctId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@CertId", SqlDbType.Int); spParam.Value = intCertID;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Type", SqlDbType.VarChar, 50); spParam.Value = strType;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_assignAWCertificates", lstParam.ToArray());

            return noOfRowsAffected;
        }

        public int AssignClpCertificate(int intAcctId, int intCertID, string strType, SqlTransaction objTrans)
        {
            SqlParameter spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = intAcctId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@CertId", SqlDbType.Int); spParam.Value = intCertID;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Type", SqlDbType.VarChar, 50); spParam.Value = strType;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_assignClpCertificates", lstParam.ToArray());

            return noOfRowsAffected;
        }

        public bool UpdateUpgradeStat(string srcFru, string destFru)
        {
            SqlParameter spParam = new SqlParameter("@srcFru", SqlDbType.VarChar,20); spParam.Value = srcFru;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@DestFru", SqlDbType.VarChar,20); spParam.Value = destFru;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_UpdateUpgradeStatus", lstParam.ToArray());
            if (noOfRowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateUpgradeStat(string strSerialNo, SqlTransaction objTran)
        {
            SqlParameter spParam = new SqlParameter("@serial_number", SqlDbType.VarChar, 20); spParam.Value = strSerialNo;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTran, CommandType.StoredProcedure, "LMS_UpdateUpgradeCtrlStat", lstParam.ToArray());
            if (noOfRowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public int AssignCertificate(int intAcctId, int intCertID, string strType, SqlConnection objSqlConn)
        {
            SqlParameter spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = intAcctId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@CertId", SqlDbType.Int); spParam.Value = intCertID;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Type", SqlDbType.VarChar, 50); spParam.Value = strType;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objSqlConn, CommandType.StoredProcedure, "LMS_assignCertificates", lstParam.ToArray());

            return noOfRowsAffected;
        }

        public int UnassignCertificate(int intCertID,int intResellerId, string strType, SqlTransaction objTran)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
           
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.Int); spParam.Value = intCertID;
            lstParam.Add(spParam);


             spParam = new SqlParameter("@ParentId", SqlDbType.Int); spParam.Value = intResellerId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Type", SqlDbType.VarChar, 50); spParam.Value = strType;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTran, CommandType.StoredProcedure, "LMS_unassignCertificates", lstParam.ToArray());
            return noOfRowsAffected;
        }

        public int UnassignAWCertificate(int intCertID, int intResellerId, string strType, SqlTransaction objTran)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.Int); spParam.Value = intCertID;
            lstParam.Add(spParam);


            spParam = new SqlParameter("@ParentId", SqlDbType.Int); spParam.Value = intResellerId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Type", SqlDbType.VarChar, 50); spParam.Value = strType;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTran, CommandType.StoredProcedure, "LMS_unassignAWCertificates", lstParam.ToArray());
            return noOfRowsAffected;
        }

        public int UnassignClpCertificate(int intCertID, int intResellerId, string strType, SqlTransaction objTran)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.Int); spParam.Value = intCertID;
            lstParam.Add(spParam);


            spParam = new SqlParameter("@ParentId", SqlDbType.Int); spParam.Value = intResellerId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Type", SqlDbType.VarChar, 50); spParam.Value = strType;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTran, CommandType.StoredProcedure, "LMS_unassignClpCertificates", lstParam.ToArray());
            return noOfRowsAffected;
        }

        //Added by Ashwini on Jan 24/2014
        public DataSet GetAllQucikConnectCerts(string strEmail, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Email", SqlDbType.VarChar, 500); spParam.Value = strEmail;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 500); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_GetAllQuickConnectCerts", new SqlConnection(strSQLConn));
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

        public DataSet GetAllCerts(string strEmail,int intPageSize, int intPageNum, string Filter,string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Email", SqlDbType.VarChar, 500); spParam.Value = strEmail;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 500); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_GetAllCertificates", new SqlConnection(strSQLConn));
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

        public DataSet GetUnassignedCerts(int intCompanyId,int intPageSize, int intPageNum, string Filter)
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

            SqlCommand objComm = new SqlCommand("LMS_GetUnassignedCertificates", new SqlConnection(strSQLConn));
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

        public DataSet GetUnassignedAWCerts(int intCompanyId, int intPageSize, int intPageNum, string Filter)
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

            SqlCommand objComm = new SqlCommand("LMS_getUnassignedAWCertificates", new SqlConnection(strSQLConn));
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

       public DataSet GetUnassignedClpCerts(int intCompanyId, int intPageSize, int intPageNum, string Filter)
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

            SqlCommand objComm = new SqlCommand("LMS_getUnassignedCLPCertificates", new SqlConnection(strSQLConn));
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

        public DataSet GetAssignedCerts(int intCompanyId, int intPageSize, int intPageNum, string Filter)
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

            SqlCommand objComm = new SqlCommand("LMS_GetAssignedCertificates", new SqlConnection(strSQLConn));
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

        public DataSet GetAssignedAWCerts(int intCompanyId, int intPageSize, int intPageNum, string Filter)
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

            SqlCommand objComm = new SqlCommand("LMS_GetAssignedAWCertificates", new SqlConnection(strSQLConn));
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

        public DataSet GetAssignedCLPCerts(int intCompanyId, int intPageSize, int intPageNum, string Filter)
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

            SqlCommand objComm = new SqlCommand("LMS_GetAssignedClpCertificates", new SqlConnection(strSQLConn));
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

        public DataSet GetClpSubByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getClpSubByAcctId", new SqlConnection(strSQLConn));
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

        public DataSet GetAllClpSubscription(int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getAllClpSubscription", new SqlConnection(strSQLConn));
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

        public DataSet GetClpCertsByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getClpCertsByAcctId", new SqlConnection(strSQLConn));
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

        public DataSet GetAllClpCerts(int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getAllClpCerts", new SqlConnection(strSQLConn));
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


        public DataSet GetCertsByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter,string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getAcctCertificatesByAcctId", new SqlConnection(strSQLConn));
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
        //Added by Ashwini on Jan/22/2014
        public DataSet GetQuickConnectCertsByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getQuickConnectCertsByAcctId", new SqlConnection(strSQLConn));
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

        public DataSet GetFruByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getAcctFruByAcctId", new SqlConnection(strSQLConn));
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

        public DataSet GetEvalCertsByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getEvalCertsByAcctId", new SqlConnection(strSQLConn));
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

        public DataSet GetRMACertsByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getRMACertsByAcctId", new SqlConnection(strSQLConn));
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

        public DataSet GetAllRMACerts(int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getAllRMACerts", new SqlConnection(strSQLConn));
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

        public DataSet GetActiveCertsByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter,string strBrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = strBrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getActiveCertificatesByAcctId", new SqlConnection(strSQLConn));
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

        public DataSet GetActivePermCertsByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter, string strBrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = strBrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getActivePermCertificatesByAcctId", new SqlConnection(strSQLConn));
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

        public DataSet GetMyController(int intAcctid, int intPageSize, int intPageNum, string Filter,string strBrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = strBrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_GetMyController", new SqlConnection(strSQLConn));
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

        public DataSet GetAllSerialNo(int intAcctid, int intPageSize, int intPageNum, string Filter, string strBrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = strBrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_GetSerialNo", new SqlConnection(strSQLConn));
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

        public DataSet GetDownLoadsDet(int intAcctid, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            string strSQLConn = strSQLConn = ConfigurationManager.ConnectionStrings["LMSDB"].ToString(); ;
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getDownloadDetails", new SqlConnection(strSQLConn));
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

        public string GetUserEmail(int intAcctId)
        {
            DataSet ds = new DataSet();
            string strSQLConn = strSQLConn = ConfigurationManager.ConnectionStrings["LMSDB"].ToString(); ;
            SqlParameter spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = intAcctId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getUserEmail", new SqlConnection(strSQLConn));
            objComm.CommandType = CommandType.StoredProcedure;
            objComm.CommandTimeout = 200;
            SqlParameterCollection prmColl = objComm.Parameters;
            for (int i = 0; i < lstParam.Count; i++)
            {
                prmColl.Add(lstParam[i]);
            }

            SqlDataAdapter objAdap = new SqlDataAdapter(objComm);
            objAdap.Fill(ds);
            if (ds == null || ds.Tables[0].Rows.Count <= 0)
            {
                return string.Empty;
            }
            else
            {
                return ds.Tables[0].Rows[0]["email"].ToString();
            }
        }

        public bool UpdateDownloadLog(int intAcctid,string strFileName, string strBrand)
        {
            SqlParameter spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = intAcctid;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filename", SqlDbType.VarChar, 200); spParam.Value = strFileName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Brand", SqlDbType.VarChar,20); spParam.Value = strBrand;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_UpdateDownloadLog", lstParam.ToArray());
            if (noOfRowsAffected > 0)
                return true;
            else
                return false;
        }
       
        public DataSet GetFruInfo(string strFru)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@Fru", SqlDbType.VarChar, 100); spParam.Value = strFru;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            SqlCommand objComm = new SqlCommand("LMS_GetFruDetails", new SqlConnection(strSQLConn));
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

        public DataSet GetCertPartDesc(string strPartId)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PartId", SqlDbType.VarChar, 100); spParam.Value = strPartId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            SqlCommand objComm = new SqlCommand("LMS_GetCertPartDesc", new SqlConnection(strSQLConn));
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

        public DataSet GetEvalParts(string strBrand, int intPageSize, int intPageNum, string Filter)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 50); spParam.Value = strBrand;
            lstParam.Add(spParam);


            SqlCommand objComm = new SqlCommand("LMS_getEvalParts", new SqlConnection(strSQLConn));
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


        public bool ReassignCertificates(int intOldAcctId,int intDestAcctID, string CommaSeperatedCertIds, string srcCertType)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@intOldAcctId", SqlDbType.Int); spParam.Value = intOldAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@intDestAcctID", SqlDbType.VarChar, 50); spParam.Value = intDestAcctID;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CertType", SqlDbType.VarChar, 50); spParam.Value = srcCertType;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.Text); spParam.Value = CommaSeperatedCertIds;
            lstParam.Add(spParam);


            int intRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_reassignCerts", lstParam.ToArray());
            if (intRowsAffected == 0)
                return false;
            else
                return true;
        }

        public bool UpdateAlcatelCert(DataRow dr)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@pk_cert_id", SqlDbType.Int); spParam.Value = Int32.Parse(dr["pk_cert_id"].ToString());
            lstParam.Add(spParam);

            spParam = new SqlParameter("@ArubaPartid", SqlDbType.VarChar, 50); spParam.Value = dr["ArubaPartId"].ToString();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@ArubaPartDesc", SqlDbType.VarChar, 100); spParam.Value = dr["ArubaPartDesc"].ToString();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@ArubaSerialNumber", SqlDbType.VarChar, 100); spParam.Value = dr["ArubaSerialNumber"].ToString();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = ConfigurationManager.AppSettings["ARUBA_BRAND"].ToString();
            lstParam.Add(spParam);

            int intRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_UpdateAlcCert", lstParam.ToArray());
            if (intRowsAffected == 0)
                return false;
            else
                return true;
        }

        public bool CheckOwnership(int intAcctid, int intCertId, string srcCertType)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            string ParamName = "@acctId";

            if (!srcCertType.Equals(CertType.AccountCertificate))
                ParamName = "@companyId";

            SqlParameter spParam = new SqlParameter(ParamName, SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@type", SqlDbType.VarChar, 50); spParam.Value = srcCertType;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@certId", SqlDbType.Int); spParam.Value = intCertId;
            lstParam.Add(spParam);


            DataSet ds = SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_checkAcctOwnsCert", lstParam.ToArray());
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return false;
            else
                return true;
        }

        public bool CheckOwnershipNew(int intAcctid, int intCertId, string srcCertType)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            string ParamName = "@acctId";

            if (!srcCertType.Equals(CertType.AccountCertificate))
                ParamName = "@companyId";

            SqlParameter spParam = new SqlParameter(ParamName, SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@type", SqlDbType.VarChar, 50); spParam.Value = CertType.AccountCertificate;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@certId", SqlDbType.Int); spParam.Value = intCertId;
            lstParam.Add(spParam);


            DataSet ds = SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_checkAcctOwnsCert", lstParam.ToArray());
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return false;
            else
                return true;
        }

        public bool CheckPOEOwnership(int intAcctId, int intFruId, string Type)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            string ParamName = "@acctId";

            if (!Type.Equals(CertType.AccountCertificate))
                ParamName = "@companyId";

            SqlParameter spParam = new SqlParameter(ParamName, SqlDbType.Int); spParam.Value = intAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@type", SqlDbType.VarChar, 50); spParam.Value = Type;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@fruId", SqlDbType.Int); spParam.Value = intFruId;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_checkAcctOwnsPOECert", lstParam.ToArray());
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return false;
            else
                return true;
        }

        public DataSet CheckTransferCount(int intAcctId, int intCertId, string strAction)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            string ParamName = "@acctId";

            //if (!srcCertType.Equals(CertType.AccountCertificate))
              //  ParamName = "@companyId";

            SqlParameter spParam = new SqlParameter(ParamName, SqlDbType.Int); spParam.Value = intAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@action", SqlDbType.VarChar, 50); spParam.Value = strAction;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@certId", SqlDbType.Int); spParam.Value = intCertId;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_CheckForTransferCount", lstParam.ToArray());
            return ds;
        }

        public DataSet CheckTransferCountNew(int intCertId)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@certId", SqlDbType.Int); spParam.Value = intCertId;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_CheckForTransferCountNew", lstParam.ToArray());
            return ds;
        }

        public bool CheckAWTransferCount(int intAcctId, int intCertId, string strAction)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            string ParamName = "@acctId";

            //if (!srcCertType.Equals(CertType.AccountCertificate))
            //  ParamName = "@companyId";

            SqlParameter spParam = new SqlParameter(ParamName, SqlDbType.Int); spParam.Value = intAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@action", SqlDbType.VarChar, 50); spParam.Value = strAction;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@certId", SqlDbType.Int); spParam.Value = intCertId;
            lstParam.Add(spParam);


            DataSet ds = SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_CheckForAWTransferCount", lstParam.ToArray());
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return false;
            else
            {
                if (Int32.Parse(ds.Tables[0].Rows[0]["TransferCount"].ToString()) > Int32.Parse(ConfigurationManager.AppSettings["MAXTRN_COUNT"].ToString()))
                    return true;
                else
                    return false;
            }
        }

        public DataSet CheckActivationInfo(string strListOfCertIds)
        {
            
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@ListOfCertId", SqlDbType.Text); spParam.Value = strListOfCertIds;
            lstParam.Add(spParam);

            DataSet ds= SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_CheckActivationInfo", lstParam.ToArray());
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds;
        }

        public bool CancelSerialNumber(int intCertId, string strUserId, string strReason)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.Int); spParam.Value = intCertId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@UserId", SqlDbType.VarChar, 100); spParam.Value = strUserId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Reason", SqlDbType.Text); spParam.Value = strReason;
            lstParam.Add(spParam);

             int IntResult = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_CancelSerialNumber", lstParam.ToArray());

             if (IntResult <= 0)
                 return false;
             else
                 return true;
        
        }

        public DataSet getOldestSerialNo(string certValue)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@ListOfCertId", SqlDbType.VarChar,100); spParam.Value = certValue;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_GetOldestSerialNo", lstParam.ToArray());
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;
            else
                return ds;
        }

        public string getPartType(string PartID, string strBrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@PartID", SqlDbType.VarChar, 50); spParam.Value = PartID;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = strBrand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetPartType", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["TYP"].ToString();
            else
                return null;
        }

        // Added by Ashwini on Jan 20/2014
        public DataSet GetQuickConnectCertInfo(string certId, string brand)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            DataSet ds;
            SqlParameter spParam = new SqlParameter("@SerialNumber", SqlDbType.VarChar, 50); spParam.Value = certId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = brand;
            lstParam.Add(spParam);

            ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetQuickConnectDetBySlNo", lstParam.ToArray()); 
            return ds;

        }

        public DataSet getQuickConnectUserInfo(string email)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            DataSet ds;
            SqlParameter spParam = new SqlParameter("@Email", SqlDbType.VarChar, 250); spParam.Value = email;
            lstParam.Add(spParam);

            ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetQuickConnectUser", lstParam.ToArray());
            return ds;
        }

        public DataSet getQuickConnectCompanyInfo(string email)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            DataSet ds;
            SqlParameter spParam = new SqlParameter("@Email", SqlDbType.VarChar, 250); spParam.Value = email;
            lstParam.Add(spParam);

            ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetQuickConnectCompany", lstParam.ToArray());
            return ds;
        }

        public DataSet getQuickConnectUserInfo(string email, string companyName)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            DataSet ds;
            SqlParameter spParam = new SqlParameter("@Email", SqlDbType.VarChar, 250); spParam.Value = email;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CompanyName", SqlDbType.VarChar, 500); spParam.Value = companyName;
            lstParam.Add(spParam);

            ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetQuickConnectUserByCompany", lstParam.ToArray());
            return ds;
        }

        // 2014

        public bool isFlexibleLicense(string PartNo)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            DataSet ds;
            SqlParameter spParam = new SqlParameter("@PartNo", SqlDbType.VarChar, 250); spParam.Value = PartNo;
            lstParam.Add(spParam);

            ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsFlexibleLic", lstParam.ToArray());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }


        public int InsertQuickConnect(int pk_cert_id, int ImpAcctId, string company_name, int AcctId, string expiry_date, string username, string Password, int licenseCount,SqlTransaction objTran)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@Pk_Cert_id", SqlDbType.Int); spParam.Value = pk_cert_id;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@ImpActId", SqlDbType.Int); spParam.Value = ImpAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CompanyName", SqlDbType.VarChar, 200); spParam.Value = company_name;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@expiryDate", SqlDbType.DateTime); spParam.Value = Convert.ToDateTime(expiry_date);
            lstParam.Add(spParam);

            spParam = new SqlParameter("@ActId", SqlDbType.Int); spParam.Value = AcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@UserName", SqlDbType.VarChar, 200); spParam.Value = username;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Password", SqlDbType.VarChar, 20); spParam.Value = Password;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@licenseCount", SqlDbType.Int); spParam.Value = licenseCount;
            lstParam.Add(spParam);

            int num = SqlHelper.ExecuteNonQuery(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_InsertQuickConnect", lstParam.ToArray());
            return num;

        }

        public int LogQuickConnectInfo(int AcctId, int CertId, int ImpAcctId, SqlTransaction objTran)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();            
            SqlParameter spParam = new SqlParameter("@Acctid", SqlDbType.Int); spParam.Value = AcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Certid", SqlDbType.Int); spParam.Value = CertId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@ImpAcctId", SqlDbType.Int); spParam.Value = ImpAcctId;
            lstParam.Add(spParam);

            int num = SqlHelper.ExecuteNonQuery(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_LogQuickConnectInfo", lstParam.ToArray());
            return num;
        }

        public string getPartDesc(string PartID,string brand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            DataSet ds;
            SqlParameter spParam = new SqlParameter("@PartID", SqlDbType.VarChar, 50); spParam.Value = PartID;
            lstParam.Add(spParam);
            if (brand == ConfigurationManager.AppSettings["ARUBA_BRAND"].ToString())
            {
               ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetArubaPartDesc", lstParam.ToArray());
            }
            else
            {
               ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetAlcPartDesc", lstParam.ToArray());
            }
            if (ds.Tables[0].Rows.Count > 0)
                return ds.Tables[0].Rows[0]["PartDesc"].ToString();
            else
                return null;
        }

        public DataSet GetCertsByFru(string strFru, string brand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            DataSet ds;
            SqlParameter spParam = new SqlParameter("@fru", SqlDbType.VarChar, 50); spParam.Value = strFru;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 50); spParam.Value = brand;
            lstParam.Add(spParam);

            ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetActInfoByFru", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
                return ds;
            else
                return null;
        }

        public bool IsSameVersionCertOnCtrl(string strSerialNo, string strCertid)
        {
            return true;
        }

        public bool LogSentEmail(string StrCertId, string strSoId, string StrTo, int AcctId)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@certId", SqlDbType.VarChar, 150); spParam.Value = StrCertId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@SoId", SqlDbType.VarChar, 50); spParam.Value = strSoId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@contact", SqlDbType.VarChar, 500); spParam.Value = StrTo;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Acctid", SqlDbType.Int); spParam.Value = AcctId;
            lstParam.Add(spParam);

            int result =  SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_LogSentEmail", lstParam.ToArray());
            if (result > 0)
                return true;
            else
                return false;
        }


        public bool LoadDataToDB(string strFru,string strLicName, string strCert, string strActkey,string brand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            DataSet ds;
            SqlParameter spParam = new SqlParameter("@fru", SqlDbType.VarChar, 50); spParam.Value = strFru;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@LicName", SqlDbType.VarChar, 50); spParam.Value = strLicName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Cert", SqlDbType.VarChar, 50); spParam.Value = strCert;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@ActKey", SqlDbType.VarChar, 50); spParam.Value = strActkey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 50); spParam.Value = brand;
            lstParam.Add(spParam);

            ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_InsertCertsToLMS", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
                return true;
            else
                return false;
        }

        public bool CancelOPSActivations(string SerialNo, string strUserId)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@SerialNo", SqlDbType.VarChar, 100); spParam.Value = SerialNo;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@UserID", SqlDbType.VarChar, 100); spParam.Value = strUserId;
            lstParam.Add(spParam);

            int result = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_CancelOPSActivations", lstParam.ToArray());
            if (result > 0)
                return true;
            else
                return false;
        }

        public bool AddEvalCertificate(string PartId, string SerialNumber, string SoldToCust, string SoldToName, string ShipToName
                                        , string BillToName,string brand,string certType)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@partId", SqlDbType.VarChar, 25); spParam.Value = PartId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@serialNumber", SqlDbType.Text); spParam.Value = SerialNumber;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@soldToCust", SqlDbType.VarChar, 50);
            spParam.IsNullable = true;
            if (SoldToCust == null)
                spParam.Value = DBNull.Value;
            else
                spParam.Value = SoldToCust;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@soldToName", SqlDbType.VarChar, 50); spParam.Value = SoldToName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@shipToName", SqlDbType.VarChar, 50); spParam.Value = ShipToName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@billToName", SqlDbType.VarChar, 50); spParam.Value = BillToName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = brand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@certType", SqlDbType.VarChar, 10); spParam.Value = certType;
            lstParam.Add(spParam);

            object retval = SqlHelper.ExecuteScalar(strSQLConn, CommandType.StoredProcedure, "LMS_addEvalCertificate", lstParam.ToArray());
            if (retval == null)
                return false;
            else
            {
                if (Int32.Parse(retval.ToString()) > 0)
                    return true;
                else
                    return false;

            }

        }

        public string getCertVersion(string strCertpart)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@certPartId", SqlDbType.VarChar, 100); spParam.Value = strCertpart;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetCertVersion", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["version"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public bool isUpgradebleCert(string strCertpart)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@PartId", SqlDbType.VarChar, 100); spParam.Value = strCertpart;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsUpgradebleCert", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddSerialNumber(string PartId,string PartDesc, string SerialNumber, string SoldToCust, string SoldToName, string ShipToName
                                   , string BillToName,SqlTransaction objTrans,string strBrand,string strVersion)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@partId", SqlDbType.VarChar, 25); spParam.Value = PartId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@partdesc", SqlDbType.VarChar, 150); spParam.Value = PartDesc;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@serialNumber", SqlDbType.Text); spParam.Value = SerialNumber;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@soldToCust", SqlDbType.VarChar, 50);
            spParam.IsNullable = true;
            if (SoldToCust == null)
                spParam.Value = DBNull.Value;
            else
                spParam.Value = SoldToCust;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@soldToName", SqlDbType.VarChar, 50); spParam.Value = SoldToName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@shipToName", SqlDbType.VarChar, 50); spParam.Value = ShipToName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@billToName", SqlDbType.VarChar, 50); spParam.Value = BillToName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = strBrand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@version", SqlDbType.VarChar, 20); spParam.Value = strVersion;
            lstParam.Add(spParam);

            object retval = SqlHelper.ExecuteScalar(objTrans, CommandType.StoredProcedure, "LMS_addSerialNumber", lstParam.ToArray());
            if (retval == null)
                return false;
            else
            {
                if (Int32.Parse(retval.ToString()) > 0)
                    return true;
                else
                    return false;

            }

        }

        public bool AddTransactionRecord(string UserId, string sql, string ipaddress, string comment,SqlTransaction objTrans)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@uid", SqlDbType.VarChar, 100); spParam.Value = UserId;
            lstParam.Add(spParam);


            spParam = new SqlParameter("@sql", SqlDbType.Text); spParam.Value = sql;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@ipaddr", SqlDbType.VarChar,100); spParam.Value = ipaddress;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@comment", SqlDbType.Text);spParam.Value = comment;
            lstParam.Add(spParam);
            
            object retval = SqlHelper.ExecuteScalar(objTrans, CommandType.StoredProcedure, "LMS_addTransaction", lstParam.ToArray());
            if (retval == null)
                return false;
            else
            {
                if (Int32.Parse(retval.ToString()) > 0)
                    return true;
                else
                    return false;

            }
        }

        public bool IsCTOedCert(string certSerialNo, string strCertType)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@serial_number", SqlDbType.VarChar, 100); spParam.Value = certSerialNo;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@cert_type", SqlDbType.VarChar,50); spParam.Value = strCertType;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetCTOedCert", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsCTOedPOECert(string SerialNumber, string strCertType)    
        {
            
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@serial_number", SqlDbType.VarChar, 100); spParam.Value = SerialNumber;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@cert_type", SqlDbType.VarChar,50); spParam.Value = strCertType;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetCTOedPOECert", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ImportCTOedCerts(string SOId, int NewAcctId, int OldAcctId, string fru)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@soId", SqlDbType.VarChar, 50); spParam.Value = SOId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@ctoAcctId", SqlDbType.Int); spParam.Value = OldAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@newAcctId", SqlDbType.Int); spParam.Value = NewAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@fru", SqlDbType.VarChar,100); spParam.Value = fru;
            lstParam.Add(spParam);

            object retval = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_ImportCTOCerts", lstParam.ToArray());
            if (retval == null)
                return false;
            else
            {
                if (Int32.Parse(retval.ToString()) > 0)
                    return true;
                else
                    return false;

            }
        }

        public bool ImportCTOedPOECerts(string SOId, int NewAcctId, int OldAcctId, string FruSerialNumber)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@soId", SqlDbType.VarChar, 50); spParam.Value = SOId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@ctoAcctId", SqlDbType.Int); spParam.Value = OldAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@newAcctId", SqlDbType.Int); spParam.Value = NewAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@fru", SqlDbType.VarChar,100); spParam.Value = FruSerialNumber;
            lstParam.Add(spParam);

            object retval = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_ImportCTOPOECerts", lstParam.ToArray());
            if (retval == null)
                return false;
            else
            {
                if (Int32.Parse(retval.ToString()) > 0)
                    return true;
                else
                    return false;

            }
        }

        public DataSet GetSearchResults(int intPageSize, int intPageNum, string Filter, string ShowCerts,bool showSOID,string brand,string certType)
        {
            DataSet ds = new DataSet();
            int bitshowSOID = 0;
            if (showSOID == true)
                bitshowSOID = 1;
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@ShowCerts", SqlDbType.VarChar, 500); spParam.Value = ShowCerts;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@showSOID", SqlDbType.Bit); spParam.Value = bitshowSOID;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = brand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@certType", SqlDbType.VarChar, 10); spParam.Value = certType;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_GetSearchResults", new SqlConnection(strSQLConn));
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

        public DataSet GetHistorySearchResults(int intPageSize, int intPageNum, string Filter, string ShowCerts, bool showSOID, string brand, string certType)
        {
            DataSet ds = new DataSet();
            int bitshowSOID = 0;
            if (showSOID == true)
                bitshowSOID = 1;
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@ShowCerts", SqlDbType.VarChar, 500); spParam.Value = ShowCerts;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@showSOID", SqlDbType.Bit); spParam.Value = bitshowSOID;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = brand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@certType", SqlDbType.VarChar, 10); spParam.Value = certType;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_GetSearchHistoryResults", new SqlConnection(strSQLConn));
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

        public DataSet GetCertsAssignedToResellers(int intCompanyId, int intPageSize, int intPageNum, string Filter)
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

            SqlCommand objComm = new SqlCommand("LMS_getCertificatesAssignedToReseller", new SqlConnection(strSQLConn));
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

        public DataSet GetClpCertsAssignedToResellers(int intCompanyId, int intPageSize, int intPageNum, string Filter)
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

            SqlCommand objComm = new SqlCommand("LMS_getClpCertificatesAssignedToReseller", new SqlConnection(strSQLConn));
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

        public DataSet GetAWCertsAssignedToResellers(int intCompanyId, int intPageSize, int intPageNum, string Filter)
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

            SqlCommand objComm = new SqlCommand("LMS_getAWCertificatesAssignedToReseller", new SqlConnection(strSQLConn));
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

        public string GetSerialNo(string strActivationcode)
        {
            SqlParameter spParam = new SqlParameter("@activationcode", SqlDbType.Text); spParam.Value = strActivationcode;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            DataSet dsSerialNo = new DataSet();
            dsSerialNo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getSerialNumber", lstParam.ToArray());
            if (dsSerialNo.Tables.Count > 0)
            {
                dsSerialNo.Tables[0].TableName = "SERIAL_INFO";
                if (dsSerialNo.Tables[0].Rows.Count > 0)
                {
                    return dsSerialNo.Tables[0].Rows[0]["serial_number"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public DataSet GetAccountsForCert(string strSerialNumber, string strCertType)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@serialNumber", SqlDbType.VarChar,100); spParam.Value = strSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@certType", SqlDbType.VarChar, 50); spParam.Value = strCertType;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getAcctforCert", new SqlConnection(strSQLConn));
            objComm.CommandType = CommandType.StoredProcedure;
            objComm.CommandTimeout = 200;
            SqlParameterCollection prmColl = objComm.Parameters;
            for (int i = 0; i < lstParam.Count; i++)
            {
                prmColl.Add(lstParam[i]);
            }

            SqlDataAdapter objAdap = new SqlDataAdapter(objComm);
            objAdap.Fill(ds);

            if (ds.Tables.Count < 0 || ds.Tables[0].Rows.Count < 0)
                ds = null;

            return ds;
        
        }

        public int UpdateAirwaveActivationInfo(int intAcctId, string strCertId, string strSystemIP, string strActivationCode, SqlTransaction objTrans)
        {
            SqlParameter spParam = new SqlParameter("@certID", SqlDbType.VarChar, 100); spParam.Value = strCertId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctId;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@systemIP", SqlDbType.VarChar, 50); spParam.Value = strSystemIP;
            lstParam.Add(spParam);
            strActivationCode = strActivationCode.Replace("\n", "<BR>");
            spParam = new SqlParameter("@activationCode", SqlDbType.VarChar, 7000); spParam.Value = strActivationCode;
            lstParam.Add(spParam);
            
            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_updateAirwaveActivationInfo", lstParam.ToArray());

            return noOfRowsAffected;
        }

        public DataSet GetAirwaveCertInfoForIP(int CertId)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            DataSet dscertInfo = new DataSet();
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.Int);
            spParam.Value = CertId;
            lstParam.Add(spParam);
            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getAirwaveCertInfoForIP", lstParam.ToArray());
            return dscertInfo;
        }

        public DataSet GetAirwaveCertInfoForIP(string CertId)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            DataSet dscertInfo = new DataSet();
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 100);
            spParam.Value = CertId;
            lstParam.Add(spParam);
            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getAirwaveCertInfo", lstParam.ToArray());
            return dscertInfo;
        }

        public bool IsAirwaveCertActivated(string strCertId, string strIPAddress)
        {
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 100); spParam.Value = strCertId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@SystemIP", SqlDbType.VarChar, 30); spParam.Value = strIPAddress;
            lstParam.Add(spParam);
            DataSet dsCertSys = new DataSet();

            dsCertSys = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsAirwaveCertIPCombo", lstParam.ToArray());
            if (dsCertSys.Tables.Count > 0)
            {
                dsCertSys.Tables[0].TableName = "CERTSYS_INFO";
                if (dsCertSys.Tables[0].Rows.Count > 0)
                    return true;
            }
            return false;
        }

        public DataSet GetAirwaveCertByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getAirwaveCertByAcctId", new SqlConnection(strSQLConn));
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

        public DataSet LoadAirwaveEvalParts(string strBrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strBrand;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getAirwaveEvalParts", new SqlConnection(strSQLConn));
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

        public bool CheckAirwaveOwnership(int intAcctId, int intCertId)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();

            SqlParameter spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@certId", SqlDbType.Int); spParam.Value = intCertId;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_checkAcctOwnsAirwaveCert", lstParam.ToArray());
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return false;
            else
                return true;
        }

        public bool InsertAirwaveActkey(string strActivationKey, string strIPAddress, string strEvalkey, string strOrganizationName, int acctId, SqlTransaction objTran)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            strActivationKey = strActivationKey.Replace("\n","<BR>");
            SqlParameter spParam = new SqlParameter("@ActivationKey", SqlDbType.VarChar, 7000); spParam.Value = strActivationKey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@IPAddress", SqlDbType.VarChar, 50); spParam.Value = strIPAddress;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@EvalCertId", SqlDbType.VarChar, 200); spParam.Value = strEvalkey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@OrgName", SqlDbType.VarChar, 2000); spParam.Value = strOrganizationName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = acctId;
            lstParam.Add(spParam);

            int result = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_AddAirwaveActKey", lstParam.ToArray());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool InsertConsolidateAirwveActKey(string strActivationKey, string strIPAddress, string strCertId1, string strCertId2, string strOrganizationName, int AcctId, SqlTransaction objTran)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            strActivationKey = strActivationKey.Replace("\n", "<BR>");
            SqlParameter spParam = new SqlParameter("@ActivationKey", SqlDbType.VarChar, 7000); spParam.Value = strActivationKey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@IPAddress", SqlDbType.VarChar, 50); spParam.Value = strIPAddress;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CertId1", SqlDbType.VarChar, 200); spParam.Value = strCertId1;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CertId2", SqlDbType.VarChar, 200); spParam.Value = strCertId2;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@OrgName", SqlDbType.VarChar, 2000); spParam.Value = strOrganizationName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = AcctId;
            lstParam.Add(spParam);

            int result = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_AddAirwaveConsolidateActKey", lstParam.ToArray());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet getAirwaveCertDet(string strAirwaveCert,string strBrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 100); spParam.Value = strAirwaveCert;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = strBrand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetAirwaveCert", lstParam.ToArray());
            return ds;

        }

        public DataSet GetFAQ(string strRole, string strBrand)
        {
            DataSet dscertInfo = new DataSet();
            SqlParameter spParam = new SqlParameter("@Role", SqlDbType.VarChar, 20);
            spParam.Value = strRole;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Brand", SqlDbType.VarChar, 20);
            spParam.Value = strBrand;
            lstParam.Add(spParam);

            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getFAQ", lstParam.ToArray());
            if (dscertInfo.Tables.Count > 0)
            {
                dscertInfo.Tables[0].TableName = "CERT_INFO";
                if (dscertInfo.Tables[0].Rows.Count > 0)
                    return dscertInfo;
            }
            return null;
        }

        public bool IsAirwaveCertActivated(string strCertId)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 200); spParam.Value = strCertId;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsAirwaveCertActivated", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        internal bool ReassignAirwaveCertificates(int intOldAcctId, int intDestAcctID, int impersonatedBy, string CommaSeperatedCertIds)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@intOldAcctId", SqlDbType.Int); spParam.Value = intOldAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@intDestAcctID", SqlDbType.VarChar, 50); spParam.Value = intDestAcctID;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@impersonatedBy", SqlDbType.VarChar, 50); spParam.Value = impersonatedBy;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.Text); spParam.Value = CommaSeperatedCertIds;
            lstParam.Add(spParam);


            int intRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_reassignAirwaveCerts", lstParam.ToArray());
            if (intRowsAffected == 0)
                return false;
            else
                return true;
        }

        internal DataSet GetAirwaveCertInfoForCertId(object CertId)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            DataSet dscertInfo = new DataSet();
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.Int);
            spParam.Value = CertId;
            lstParam.Add(spParam);
            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getAirwaveCertInfoForCertId", lstParam.ToArray());
            return dscertInfo;
        }

        public bool AddAirwvEvalCertificate(string strEvalPart, string strEvalKey, string strSoldTo, string strOrg, string strBrand, string strCertType)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@part_id", SqlDbType.VarChar, 100); spParam.Value = strEvalPart;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@eval_key", SqlDbType.VarChar, 5000); spParam.Value = strEvalKey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@sold_to", SqlDbType.VarChar, 100); spParam.Value = strSoldTo;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@organization", SqlDbType.VarChar, 200); spParam.Value = strOrg;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = strBrand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@cert_type", SqlDbType.VarChar, 10); spParam.Value = strCertType;
            lstParam.Add(spParam);

            int intRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_AddAirwvEvalCerts", lstParam.ToArray());
            if (intRowsAffected == 0)
                return false;
            else
                return true;

        }

        public bool RestrictGenerateAirwEvalCert(string strPartNo, string strEmail,string strBrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@PartNo", SqlDbType.VarChar, 100); spParam.Value = strPartNo;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Email", SqlDbType.VarChar, 500); spParam.Value = strEmail;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Brand", SqlDbType.VarChar, 20); spParam.Value = strBrand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetAirwaveEvalGenCount", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsAirwaveCertConsolidated(string strCertId1,string strCertId2,string strIPAddress)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@CertId1", SqlDbType.VarChar, 200); spParam.Value = strCertId1;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CertId2", SqlDbType.VarChar, 200); spParam.Value = strCertId2;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@IPAddress", SqlDbType.VarChar, 50); spParam.Value = strIPAddress;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsAirwaveCertConsolidated", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet GetAccountsForAirwCert(string strSerialNumber)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@serialNumber", SqlDbType.VarChar, 100); spParam.Value = strSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getAcctforAirwvCert", new SqlConnection(strSQLConn));
            objComm.CommandType = CommandType.StoredProcedure;
            objComm.CommandTimeout = 200;
            SqlParameterCollection prmColl = objComm.Parameters;
            for (int i = 0; i < lstParam.Count; i++)
            {
                prmColl.Add(lstParam[i]);
            }

            SqlDataAdapter objAdap = new SqlDataAdapter(objComm);
            objAdap.Fill(ds);

            if (ds.Tables.Count < 0 || ds.Tables[0].Rows.Count < 0)
                ds = null;

            return ds;

        }

        //By Praveena - For Amigopod Integration - Details from cert gen data table
        public DataSet getAmigopodCertFromCertGen(string strAmigopodCert, string strBrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@certificate_id", SqlDbType.VarChar, 100); spParam.Value = strAmigopodCert;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = strBrand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strCertsConn), CommandType.StoredProcedure, "sp_GetAmgCertificate", lstParam.ToArray());
            return ds;
        }

        //By Praveena - For Amigopod Integration
        public DataSet getAmigopodCertDet(string strAmigopodCert, string strBrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 100); spParam.Value = strAmigopodCert;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = strBrand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetAmigopodCert", lstParam.ToArray());
            return ds;
        }

        //By Praveena - For Amigopod Integration
        public bool IsAmigopodCertActivated(string strCertId, string actionType)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 200); spParam.Value = strCertId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@actionType", SqlDbType.VarChar, 50); spParam.Value = actionType;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsAmigopodCertActivated", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataSet getClpCertFromCertGen(string strAmigopodCert, string strBrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@certificate_id", SqlDbType.VarChar, 100); spParam.Value = strAmigopodCert;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = strBrand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strCertsConn), CommandType.StoredProcedure, "sp_GetClpCertificate", lstParam.ToArray());
            return ds;
        }

        // Added by Ashwini on Mar/25/2013

        public bool IsAvendaSubKeyImported(string strSubKey)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@SubKey", SqlDbType.VarChar, 200); spParam.Value = strSubKey;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsClpSubKeyActivated", lstParam.ToArray());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        // Added by Ashwini on jan/18/2014
        public bool IsClpLicenseImported(string ActivationKey, string PartId)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@ActKey", SqlDbType.VarChar, 200); spParam.Value = ActivationKey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@PartId", SqlDbType.VarChar, 100); spParam.Value = PartId;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsClpLicenseActivated", lstParam.ToArray());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        //By Praveena - For Amigopod Integration
        public DataSet getAmigopodLookupInfo(string strCertId, string strBrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 200); spParam.Value = strCertId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = strBrand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetAmigopodLookupInfo", lstParam.ToArray());
            return ds;
        }

        //By Praveena - For Amigopod Integration
        public DataSet getAmgLookupInfo(string strPartId, string strBrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@part_id", SqlDbType.VarChar, 200); spParam.Value = strPartId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = strBrand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetAmgLookupInfoByPartid", lstParam.ToArray());
            return ds;
        }

        //By Praveena - For Amigopod Integration
        public bool InsertAmigopodActkey(string subscriptionKey, string cert_id, string action, int acctId, int company_id, string company_name, int impersonatedBy, string comments, SqlTransaction objTran,string expiryDate)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            subscriptionKey = subscriptionKey.Replace("\n", "<BR>");
            SqlParameter spParam = new SqlParameter("@ActivationKey", SqlDbType.VarChar, 7000); spParam.Value = subscriptionKey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 200); spParam.Value = cert_id;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@action", SqlDbType.VarChar, 20); spParam.Value = action;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = acctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@comapny_id", SqlDbType.Int); spParam.Value = company_id;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@comapny_name", SqlDbType.VarChar, 250); spParam.Value = company_name;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@impersonatedBy", SqlDbType.VarChar, 250); spParam.Value = impersonatedBy;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@commnets", SqlDbType.VarChar, 200); spParam.Value = comments;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@expiryDate", SqlDbType.DateTime);
            if (expiryDate != "")
            {
                spParam.Value = Convert.ToDateTime(expiryDate);
            }
            else
            {
                spParam.Value = null;
            }
            lstParam.Add(spParam);

            int result = SqlHelper.ExecuteNonQuery(objTran, CommandType.StoredProcedure, "LMS_AddAmigopodActKey", lstParam.ToArray());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //Added by Ashwini on July/25/2012

        public bool IsSubkeyImported(string strSubKey)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@Subkey", SqlDbType.VarChar, 100); spParam.Value = strSubKey;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsSubKeyImported", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Added by Ashwini on July 24/2012

        public DataSet getAmgPartDet(string stLicenseCount,string brand)
        {
            DataSet ds = new DataSet();

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@LicenseCount", SqlDbType.Int);
            spParam.Value = Int32.Parse(stLicenseCount);
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = brand;
            lstParam.Add(spParam);

            ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetAmgPartDet", lstParam.ToArray());
            return ds;
        }

        //Added by Ashwini on Mar/17/2013
        public bool ImportLegacyClsSubKey(AvendaCert objAvendaCert, string strAction, int IntAcctId, int ImpActId, SqlTransaction objTran)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@SubscriptionKey", SqlDbType.VarChar, 100); spParam.Value = objAvendaCert.subscription;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@activated_ts", SqlDbType.DateTime); 
            if (objAvendaCert.activated_ts == string.Empty || objAvendaCert.activated_ts == "0")
            {
                spParam.Value = null;                
            }
            else
            {
                spParam.Value = Convert.ToDateTime(objAvendaCert.activated_ts);
            }
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = IntAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@comapny_name", SqlDbType.VarChar, 250); spParam.Value = objAvendaCert.CompanyName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@action", SqlDbType.VarChar, 50); spParam.Value = strAction;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@impersonatedBy", SqlDbType.Int); spParam.Value = ImpActId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@email", SqlDbType.VarChar, 200); spParam.Value = objAvendaCert.email;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@po_id", SqlDbType.VarChar, 20); spParam.Value = objAvendaCert.po;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@so_id", SqlDbType.VarChar, 20); spParam.Value = objAvendaCert.so;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@expiry_date", SqlDbType.DateTime);
            if (objAvendaCert.expiryDate == string.Empty || objAvendaCert.expiryDate == "0")
            {
                spParam.Value = null;                
            }
            else
            {
                spParam.Value = Convert.ToDateTime(objAvendaCert.expiryDate);   
            }
            lstParam.Add(spParam);

            spParam = new SqlParameter("@cert_id", SqlDbType.VarChar, 100); spParam.Value = objAvendaCert.CertId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Lserial_number", SqlDbType.VarChar, 20); spParam.Value = objAvendaCert.SerialNo ;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@part_id", SqlDbType.VarChar, 100); spParam.Value = objAvendaCert.partId;
            lstParam.Add(spParam);

            int result = SqlHelper.ExecuteNonQuery(objTran, CommandType.StoredProcedure, "LMS_ImportLegacyClsSubKey", lstParam.ToArray());

            if (result > 0)
            {
                return true;
            }
            return false;
        }

        public bool ImportLegacyClsQCLicKey(AvendaClpLicense objAvendaClpLicense, string strAction, int IntAcctId, int ImpActId, SqlTransaction objTran)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@Licensekey", SqlDbType.VarChar, 200); spParam.Value = objAvendaClpLicense.license_key;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@activated_ts", SqlDbType.DateTime); 
            if (objAvendaClpLicense.created_date == string.Empty || objAvendaClpLicense.created_date == "0")
            {
                spParam.Value = null;
            }
            else
            {
                spParam.Value = Convert.ToDateTime(objAvendaClpLicense.created_date);                
            }
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = IntAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@action", SqlDbType.VarChar, 50); spParam.Value = strAction;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@impersonatedBy", SqlDbType.Int); spParam.Value = ImpActId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@email", SqlDbType.VarChar, 200); spParam.Value = objAvendaClpLicense.email;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@expiry_date", SqlDbType.DateTime);

            if (objAvendaClpLicense.expiry_date == string.Empty || objAvendaClpLicense.expiry_date == "0")
            {
                spParam.Value = null;                
            }
            else
            {
                spParam.Value = Convert.ToDateTime(objAvendaClpLicense.expiry_date);
            }
            lstParam.Add(spParam);

            spParam = new SqlParameter("@cert_id", SqlDbType.VarChar, 100); spParam.Value = objAvendaClpLicense.serial_number;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Lserial_number", SqlDbType.VarChar, 20); spParam.Value = objAvendaClpLicense.Lserial_number;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@part_id", SqlDbType.VarChar, 100); spParam.Value = objAvendaClpLicense.part_id;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@part_descp", SqlDbType.VarChar, 300); spParam.Value = objAvendaClpLicense.part_desc;
            lstParam.Add(spParam);

            //spParam = new SqlParameter("@SubscriptionKey", SqlDbType.VarChar, 200); spParam.Value = objAvendaClpLicense.subscription_key;
            //lstParam.Add(spParam);

            spParam = new SqlParameter("@so_id", SqlDbType.VarChar, 20); spParam.Value = objAvendaClpLicense.so;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@po_id", SqlDbType.VarChar, 20); spParam.Value = objAvendaClpLicense.po;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@comapny_name", SqlDbType.VarChar, 20); spParam.Value = objAvendaClpLicense.company_name;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = objAvendaClpLicense.brand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@license_count", SqlDbType.Int); spParam.Value = Int32.Parse(objAvendaClpLicense.num_users);
            lstParam.Add(spParam);

            spParam = new SqlParameter("@version", SqlDbType.VarChar, 20); spParam.Value = objAvendaClpLicense.version;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@user_name", SqlDbType.VarChar, 100); spParam.Value = objAvendaClpLicense.user_name;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@password", SqlDbType.VarChar, 100); spParam.Value = objAvendaClpLicense.password ;
            lstParam.Add(spParam);

            int result = SqlHelper.ExecuteNonQuery(objTran, CommandType.StoredProcedure, "LMS_ImportLegacyClsQCKey", lstParam.ToArray());

            if (result > 0)
            {
                return true;
            }
            return false;
        }

        // Added by Ashwini on Jan/4/2014

        public bool ImportLegacyClsLicKey(AvendaClpLicense objAvendaClpLicense, string strAction, int IntAcctId, int ImpActId, SqlTransaction objTran)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@Licensekey", SqlDbType.VarChar, 200); spParam.Value = objAvendaClpLicense.license_key;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@activated_ts", SqlDbType.DateTime);
            if (objAvendaClpLicense.created_date == string.Empty || objAvendaClpLicense.created_date == "0")
            {
                spParam.Value = null;
            }
            else
            {
                spParam.Value = Convert.ToDateTime(objAvendaClpLicense.created_date);                
            }                                 
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = IntAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@action", SqlDbType.VarChar, 50); spParam.Value = strAction;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@impersonatedBy", SqlDbType.Int); spParam.Value = ImpActId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@email", SqlDbType.VarChar, 200); spParam.Value = objAvendaClpLicense.email;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@expiry_date", SqlDbType.DateTime);
            if (objAvendaClpLicense.expiry_date == string.Empty || objAvendaClpLicense.expiry_date == "0")
            {
                spParam.Value = null;                                      
            }
            else
            {
                spParam.Value = Convert.ToDateTime(objAvendaClpLicense.expiry_date);            
            }
            lstParam.Add(spParam);           

            spParam = new SqlParameter("@cert_id", SqlDbType.VarChar, 100); spParam.Value = objAvendaClpLicense.serial_number;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Lserial_number", SqlDbType.VarChar, 20); spParam.Value = objAvendaClpLicense.Lserial_number;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@part_id", SqlDbType.VarChar, 100); spParam.Value = objAvendaClpLicense.part_id;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@part_descp", SqlDbType.VarChar, 300); spParam.Value = objAvendaClpLicense.part_desc;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@SubscriptionKey", SqlDbType.VarChar, 200); spParam.Value = objAvendaClpLicense.subscription_key;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@so_id", SqlDbType.VarChar, 20); spParam.Value = objAvendaClpLicense.so;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@po_id", SqlDbType.VarChar, 20); spParam.Value = objAvendaClpLicense.po;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@comapny_name", SqlDbType.VarChar, 20); spParam.Value = objAvendaClpLicense.company_name;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 20); spParam.Value = objAvendaClpLicense.brand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@version", SqlDbType.VarChar, 20); spParam.Value = objAvendaClpLicense.version;
            lstParam.Add(spParam);

            int result = SqlHelper.ExecuteNonQuery(objTran, CommandType.StoredProcedure, "LMS_ImportLegacyClsLicKey", lstParam.ToArray());

            if (result > 0)
            {
                return true;
            }
            return false;
        }


        //Added by Ashwini on July 23/2012
        public bool ImportLegacySubKey(AmigopodCert objAmigopodCert,string strAction,int IntAcctId,int ImpActId,SqlTransaction objTran)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@SubscriptionKey", SqlDbType.VarChar, 100); spParam.Value = objAmigopodCert.SubKey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@activated_ts", SqlDbType.VarChar, 50); spParam.Value = objAmigopodCert.activated_ts;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@action", SqlDbType.VarChar, 20); spParam.Value = strAction;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = IntAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@comapny_name", SqlDbType.VarChar, 250); spParam.Value = objAmigopodCert.OrgName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@impersonatedBy", SqlDbType.Int); spParam.Value = ImpActId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@email", SqlDbType.VarChar, 200); spParam.Value = objAmigopodCert.email;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@po_id", SqlDbType.VarChar, 20); spParam.Value = objAmigopodCert.po;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@so_id", SqlDbType.VarChar, 20); spParam.Value = objAmigopodCert.so;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@expiry_date", SqlDbType.VarChar, 50); spParam.Value = objAmigopodCert.expiryDate ;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@cert_id", SqlDbType.VarChar, 100); spParam.Value = objAmigopodCert.CertId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Lserial_number", SqlDbType.VarChar, 20); spParam.Value = objAmigopodCert.SerialNo;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@HA_subcription", SqlDbType.VarChar, 100); spParam.Value = objAmigopodCert.HASubKey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@part_id", SqlDbType.VarChar, 100); spParam.Value = objAmigopodCert.partId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@HAPart_id", SqlDbType.VarChar, 100); spParam.Value = objAmigopodCert.HAPartId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@HACertId", SqlDbType.VarChar, 100); spParam.Value = objAmigopodCert.HACertId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@HACertSerialNo", SqlDbType.VarChar, 20); spParam.Value = objAmigopodCert.HASerialNo;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@LicPart_id", SqlDbType.VarChar, 100); spParam.Value = objAmigopodCert.Lic_PartId ;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@LicCertId", SqlDbType.VarChar, 100); spParam.Value = objAmigopodCert.Lic_CertId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@LicSerialNo", SqlDbType.VarChar, 20); spParam.Value = objAmigopodCert.Lic_SerialNo;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@OnBoardPart_id", SqlDbType.VarChar, 100); spParam.Value = objAmigopodCert.OnBoard_LicPartId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@OnBoardCertId", SqlDbType.VarChar, 100); spParam.Value = objAmigopodCert.OnBoard_LicCertId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@OnBoardSerialNo", SqlDbType.VarChar, 20); spParam.Value = objAmigopodCert.OnBoard_LicSerialNo;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@AdvPart_id", SqlDbType.VarChar, 100); spParam.Value = objAmigopodCert.Advert_PartId ;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AdvCertId", SqlDbType.VarChar, 100); spParam.Value = objAmigopodCert.Advert_CertId ;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AdvSerialNo", SqlDbType.VarChar, 20); spParam.Value = objAmigopodCert.Advert_SerialNo;
            lstParam.Add(spParam);

            int result = SqlHelper.ExecuteNonQuery(objTran, CommandType.StoredProcedure, "LMS_ImportLegacySubKey", lstParam.ToArray());

            if (result > 0)
            {
                return true;
            }
            return false;
        }

        //Added by Ashwini on 23/July/2012 

        public DataSet GenerateAmgCert(string so, string part_id, string expiryDate, string strBrand, string HAPartId,string strLicPartid,string OnBoardLicPartid,string AdvPartId)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@so_id", SqlDbType.VarChar, 10); spParam.Value = so;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@expiry_date", SqlDbType.VarChar, 50); spParam.Value = expiryDate;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@part_id", SqlDbType.VarChar, 100); spParam.Value = part_id;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = strBrand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@HAPart_id", SqlDbType.VarChar, 100); spParam.Value = HAPartId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@LicPart_id", SqlDbType.VarChar, 100); spParam.Value = strLicPartid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@OnboardPart_id", SqlDbType.VarChar, 100); spParam.Value = OnBoardLicPartid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AdvPart_id", SqlDbType.VarChar, 100); spParam.Value = AdvPartId;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strCertsConn), CommandType.StoredProcedure, "LMS_GetAmgCertInfo", lstParam.ToArray());

            if (ds != null)
            {
                return ds;
            }
            return null;
        }

        public DataSet GenerateClsCert(string so, string part_id, string expiryDate, string strBrand, string LicenseCount)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@so_id", SqlDbType.VarChar, 10); spParam.Value = so;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@expiry_date", SqlDbType.VarChar, 50); spParam.Value = expiryDate;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@part_id", SqlDbType.VarChar, 100); spParam.Value = part_id;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = strBrand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Qty", SqlDbType.Int); spParam.Value = Int32.Parse(LicenseCount);
            lstParam.Add(spParam);    

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strCertsConn), CommandType.StoredProcedure, "LMS_GetClsCertInfo", lstParam.ToArray());

            if (ds.Tables.Count > 0)
            {
                return ds;
            }
            return null;
        }

        public bool isValidClpSubscription(string strSubscriptionKey)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@subscriptionkey", SqlDbType.VarChar, 200); spParam.Value = strSubscriptionKey;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsValidClpSubscription", lstParam.ToArray());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        //By Praveena - For Amigopod Integration
        public bool isValidSubscription(string subscriptionKey)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@subscriptionkey", SqlDbType.VarChar, 200); spParam.Value = subscriptionKey;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsValidAmigopodSubscription", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //By Praveena - For Amigopod Integration
        public DataSet GetAmigopodSubscriptionByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetAmigopodSubscriptionAcctId", lstParam.ToArray());
            return ds;
        }

        //By Praveena - For Amigopod Integration
        internal DataSet GetAmigopodUpgradeDetails(string subscription, string brand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@subscription", SqlDbType.VarChar, 50); spParam.Value = subscription;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = brand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetAmigopodUpgradeDetails", lstParam.ToArray());
            return ds;
        }

        //By Praveena - For Amigopod Integration
        internal bool isEnabledAdvertisingPlugin(string subscription, string brand,string action_type)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@subscription", SqlDbType.VarChar, 50); spParam.Value = subscription;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = brand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@action_type", SqlDbType.VarChar, 10); spParam.Value = action_type;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsAdvertisingEnabled", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Added by ashwini on Apr/06/2013
        public string getModuleDescription(string strModuleId)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@module_id", SqlDbType.Int); spParam.Value = Int32.Parse(strModuleId);
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetModuleDesc", lstParam.ToArray());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["ModuleDesc"].ToString();
                }
            }
            return string.Empty;
        }

        //By Praveena - For Amigopod Integration
        internal string GetCompanyIncrementor(string company_name)
        {
            string incrementor = string.Empty;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@company_name", SqlDbType.VarChar, 250); spParam.Value = company_name;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetCompanyIncrementor", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                incrementor = ds.Tables[0].Rows[0]["incrementor"].ToString();
            }
            return incrementor;
        }
        //By Praveena - For Amigopod Integration
        internal DataSet GetAmigopodSubscriptions(object intPageSize, object intPageNum, object Filter, object strbrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetAmigopodSubscriptions", lstParam.ToArray());
            return ds;
        }

        //To check whether the certificate is already associated with any account
        //By Praveena - For Amigopod Integration
        public DataSet GetAccountsForAmigopodCert(string strSerialNumber)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@serialNumber", SqlDbType.VarChar, 100); spParam.Value = strSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getAcctforAmigopodCert", new SqlConnection(strSQLConn));
            objComm.CommandType = CommandType.StoredProcedure;
            objComm.CommandTimeout = 200;
            SqlParameterCollection prmColl = objComm.Parameters;
            for (int i = 0; i < lstParam.Count; i++)
            {
                prmColl.Add(lstParam[i]);
            }

            SqlDataAdapter objAdap = new SqlDataAdapter(objComm);
            objAdap.Fill(ds);

            if (ds.Tables.Count < 0 || ds.Tables[0].Rows.Count < 0)
                ds = null;

            return ds;

        }

        public DataSet GetAccountsForALECert(string strSerialNumber)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@serialNumber", SqlDbType.VarChar, 100); spParam.Value = strSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getAcctforALECert", new SqlConnection(strSQLConn));
            objComm.CommandType = CommandType.StoredProcedure;
            objComm.CommandTimeout = 200;
            SqlParameterCollection prmColl = objComm.Parameters;
            for (int i = 0; i < lstParam.Count; i++)
            {
                prmColl.Add(lstParam[i]);
            }

            SqlDataAdapter objAdap = new SqlDataAdapter(objComm);
            objAdap.Fill(ds);

            if (ds.Tables.Count < 0 || ds.Tables[0].Rows.Count < 0)
                ds = null;

            return ds;
        }

        //By Praveena - For Amigopod Integration
        internal bool UpdateHAKey(string ha_subscription_key, string prim_subscription_key,int user_id)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter();

            spParam = new SqlParameter("@ha_sub_key", SqlDbType.VarChar, 100); spParam.Value = ha_subscription_key;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@prim_sub_key", SqlDbType.VarChar, 100); spParam.Value = prim_subscription_key;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@user_id", SqlDbType.Int); spParam.Value = user_id;
            lstParam.Add(spParam);

            int result = SqlHelper.ExecuteNonQuery(new SqlConnection(strSQLConn),CommandType.StoredProcedure, "LMS_UpdateHAKey", lstParam.ToArray());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //By Praveena - to get company info from accounts
        internal string GetCompanyInfoByEmail(string email)
        {
            string company = string.Empty;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@email", SqlDbType.VarChar, 100); spParam.Value = email;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getCompanyInfoByEmail", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                company = ds.Tables[0].Rows[0]["company"].ToString();
            }
            return company;
        }

        internal string GetHASubscription(string subscription, string action_type)
        {
            string ha_sub_key = string.Empty;

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@subscription", SqlDbType.VarChar, 100); spParam.Value = subscription;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@actiontype", SqlDbType.VarChar, 10); spParam.Value = action_type;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getHABySubscription", lstParam.ToArray());
            if (ds.Tables[0].Rows.Count > 0)
            {
                ha_sub_key = ds.Tables[0].Rows[0]["subscription_key"].ToString();
            }
            return ha_sub_key;
        }

        //changed for OnBoard Sub key : 24/08/2012
        //To Generate Cert id and insert into db
        public bool GenerateFreeSubKey(string subKey, string activated_ts, int IntAcctId, string OrgName,int CompId, int ImpActId, string expiry_date, string strAction, string email, string so, string po, string strCertId, string strSerialNo, string part_id, string part_desc)
        {

            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@SubscriptionKey", SqlDbType.VarChar, 100); spParam.Value = subKey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@activated_ts", SqlDbType.VarChar, 50); spParam.Value = activated_ts;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@action", SqlDbType.VarChar, 20); spParam.Value = strAction;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = IntAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@comapny_name", SqlDbType.VarChar, 250); spParam.Value = OrgName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@compId", SqlDbType.Int); spParam.Value = CompId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@impersonatedBy", SqlDbType.Int); spParam.Value = ImpActId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@email", SqlDbType.VarChar, 200); spParam.Value = email;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@po_id", SqlDbType.VarChar, 20); spParam.Value = po;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@so_id", SqlDbType.VarChar, 20); spParam.Value = so;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@expiry_date", SqlDbType.VarChar, 50); spParam.Value = expiry_date;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@cert_id", SqlDbType.VarChar, 100); spParam.Value = strCertId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Lserial_number", SqlDbType.VarChar, 20); spParam.Value = strSerialNo;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@part_id", SqlDbType.VarChar, 100); spParam.Value = part_id;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@part_desc", SqlDbType.VarChar, 200); spParam.Value = part_desc;
            lstParam.Add(spParam);

            int result = SqlHelper.ExecuteNonQuery(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GenerateFreeSubKey", lstParam.ToArray());

            if (result > 0)
            {
                return true;
            }
            return false;
        }

        //changed for OnBoard Sub key : 24/08/2012
        public DataSet getAmigopodLookupInfo(string strCertId, string strBrand, string ignorePartID)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 200); spParam.Value = strCertId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = strBrand;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@ignore_part_id", SqlDbType.VarChar, 50); spParam.Value = ignorePartID;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetAmigopodLookupInfo_onBoard", lstParam.ToArray());
            return ds;
        }

        //Added for License Matrix : By Praveena 24/09/2012
        internal DataSet GetLicenseMatrix(int PageSize, int PageNumber, string filter, string strbrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = PageSize;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = PageNumber;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetLicenseMatrix", lstParam.ToArray());
            return ds;
        }

        internal DataSet GetAirwaveCerts(object intPageSize, object intPageNum, object Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getAirwaveCerts", new SqlConnection(strSQLConn));
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
       
        //Added by Ashwini on Feb/26/2013
        internal DataSet GetClearPassCertDet(string strCertId, string strBrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            DataSet dscertInfo = new DataSet();
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.VarChar,100);
            spParam.Value = strCertId;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@brand", SqlDbType.VarChar,20);
            spParam.Value = strBrand;
            lstParam.Add(spParam);
            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getClearPassCertDet", lstParam.ToArray());
            return dscertInfo;
        }

        internal DataSet IsClsCertActivated(string strCertId)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            DataSet dscertInfo = new DataSet();
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 100);
            spParam.Value = strCertId;
            lstParam.Add(spParam);
            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsClearPassCertActivated", lstParam.ToArray());
            return dscertInfo;
        }

        internal bool InsertClsSubcription(string subscriptionKey, string strCertId, int ImpUserAcctId, string CompanyName, int AcctId, string CustomerEmail, string expDate, string strActKey, string strVersion)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@ImpUserAcctId", SqlDbType.Int); spParam.Value = ImpUserAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@subscriptionKey", SqlDbType.VarChar, 100); spParam.Value = subscriptionKey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 100); spParam.Value = strCertId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CustomerEmail", SqlDbType.VarChar, 500); spParam.Value = CustomerEmail;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CustomerName", SqlDbType.VarChar, 500); spParam.Value = CompanyName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@expDate", SqlDbType.DateTime); spParam.Value = Convert.ToDateTime(expDate);
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = AcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@ActivationCode", SqlDbType.VarChar, 100); spParam.Value = strActKey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Version", SqlDbType.VarChar, 20); spParam.Value = strVersion;
            lstParam.Add(spParam);

            int intRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_AddClsSubKey", lstParam.ToArray());
            if (intRowsAffected == 0)
                return false;
            else
                return true;
        }

        public DataSet GetALECertByAcct(int intAcctid, int intPageSize, int intPageNum, string Filter, string strbrand)
        {
            DataSet ds = new DataSet();
            SqlParameter spParam = new SqlParameter("@PageSize", SqlDbType.Int); spParam.Value = intPageSize;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@GetPage", SqlDbType.Int); spParam.Value = intPageNum;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Filter", SqlDbType.VarChar, 500); spParam.Value = Filter;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctid;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar); spParam.Value = strbrand;
            lstParam.Add(spParam);

            SqlCommand objComm = new SqlCommand("LMS_getALECertByAcctId", new SqlConnection(strSQLConn));
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

        //ALE License
        public DataSet getALECertDet(string strAleCert, string strBrand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 100); spParam.Value = strAleCert;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@brand", SqlDbType.VarChar, 10); spParam.Value = strBrand;
            lstParam.Add(spParam);

            DataSet ds = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_GetALECert", lstParam.ToArray());
            return ds;
        }

        public bool IsALECertActivated(string strCertId)
        {
            DataSet dsAWCert = new DataSet();
            SqlParameter spParam = new SqlParameter("@CertId", SqlDbType.VarChar, 100);
            spParam.Value = strCertId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            dsAWCert = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_IsALECertActivated", lstParam.ToArray());
            if (dsAWCert != null)
            {
                if (dsAWCert.Tables.Count > 0)
                {
                    if (dsAWCert.Tables[0].Rows.Count > 0)
                        return true;
                }
            }
            return false;
        }

        //ALE License
        public string getALECertificate(string strSerialNumber)
        {
            DataSet dsAWCert = new DataSet();
            SqlParameter spParam = new SqlParameter("@serial_number", SqlDbType.VarChar, 100);
            spParam.Value = strSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            dsAWCert = SqlHelper.ExecuteDataset(new SqlConnection(strCertsConn), CommandType.StoredProcedure, "sp_GetALECertificate", lstParam.ToArray());
            if (dsAWCert != null)
            {
                if (dsAWCert.Tables.Count > 0)
                {
                    if (dsAWCert.Tables[0].Rows.Count > 0)
                        return dsAWCert.Tables[0].Rows[0]["LicKey"].ToString();
                }
            }
            return string.Empty;
        }

        public DataSet getALECertGen(string strSerialNumber)
        {
            DataSet dsALECert = new DataSet();
            SqlParameter spParam = new SqlParameter("@serial_number", SqlDbType.VarChar, 100);
            spParam.Value = strSerialNumber;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            dsALECert = SqlHelper.ExecuteDataset(new SqlConnection(strCertsConn), CommandType.StoredProcedure, "sp_GetALECertificate", lstParam.ToArray());
            if (dsALECert != null)
            {
                if (dsALECert.Tables.Count > 0)
                {
                    return dsALECert;
                }
                return dsALECert;
            }
            return dsALECert;
        }

        //ALE License
        public bool InsertALEActkey(string strActivationKey, string strIPAddress, string strEvalkey, string strOrganizationName, int acctId, int ImpAcctId, SqlTransaction objTran)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            strActivationKey = strActivationKey.Replace("\n", "<BR>");
            SqlParameter spParam = new SqlParameter("@ActivationKey", SqlDbType.VarChar, 7000); spParam.Value = strActivationKey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@IPAddress", SqlDbType.VarChar, 50); spParam.Value = strIPAddress;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@EvalCertId", SqlDbType.VarChar, 200); spParam.Value = strEvalkey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@OrgName", SqlDbType.VarChar, 2000); spParam.Value = strOrganizationName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = acctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@ImpAcctId", SqlDbType.Int); spParam.Value = ImpAcctId;
            lstParam.Add(spParam);

            int result = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_AddALEActKey", lstParam.ToArray());
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool InsertClpEvalInfo(string subscriptionKey, int ImpUserAcctId, int AcctId, string CompanyName, string CustomerEmail, Double expDate, string PolicyLic, string EnterpriseLic, string userName, string Password, string Brand)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@ImpUserAcctId", SqlDbType.Int); spParam.Value = ImpUserAcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@subscriptionKey", SqlDbType.VarChar, 100); spParam.Value = subscriptionKey;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CustomerEmail", SqlDbType.VarChar, 500); spParam.Value = CustomerEmail;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@CustomerName", SqlDbType.VarChar, 500); spParam.Value = CompanyName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@expDate", SqlDbType.DateTime); spParam.Value = UIHelper.UnixTimeStampToDateTime(expDate);
            lstParam.Add(spParam);

            spParam = new SqlParameter("@AcctId", SqlDbType.Int); spParam.Value = AcctId;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@PolicyLicense", SqlDbType.VarChar, 100); spParam.Value = PolicyLic;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@EntLicense", SqlDbType.VarChar, 100); spParam.Value = EnterpriseLic;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Username", SqlDbType.VarChar, 100); spParam.Value = userName;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Password", SqlDbType.VarChar, 100); spParam.Value = Password;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Brand", SqlDbType.VarChar, 10); spParam.Value = Brand;
            lstParam.Add(spParam);

            int intRowsAffected = SqlHelper.ExecuteNonQuery(strSQLConn, CommandType.StoredProcedure, "LMS_AddClsEvalKey", lstParam.ToArray());
            if (intRowsAffected == 0)
                return false;
            else
                return true;
        }

        public bool IsClpEvalGenerated(string strCompanyName)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            SqlParameter spParam = new SqlParameter("@CompanyName", SqlDbType.VarChar, 500); spParam.Value = strCompanyName;
            lstParam.Add(spParam);

            DataSet dsClpEvalCerts = SqlHelper.ExecuteDataset(strSQLConn, CommandType.StoredProcedure, "LMS_IsCLPEvalGenerated", lstParam.ToArray());

            if (dsClpEvalCerts.Tables.Count > 0)
            {
                if (dsClpEvalCerts.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
            else
                return false;
        }

        public bool RestrictGenerateClpEvalCert(string strCompanyname, string strEmail, string strBrand)
        {
            SqlParameter spParam = new SqlParameter("@CompanyName", SqlDbType.VarChar, 200);
            spParam.Value = strCompanyname;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Email", SqlDbType.VarChar, 200);
            spParam.Value = strEmail;
            lstParam.Add(spParam);

            spParam = new SqlParameter("@Brand", SqlDbType.VarChar, 20);
            spParam.Value = strBrand;
            lstParam.Add(spParam);
            DataSet dscertInfo = new DataSet();

            dscertInfo = SqlHelper.ExecuteDataset(new SqlConnection(strSQLConn), CommandType.StoredProcedure, "LMS_getClpEvalCertCount", lstParam.ToArray());
            if (dscertInfo.Tables.Count > 0)
            {
                dscertInfo.Tables[0].TableName = "CERT_INFO";
                if (Int32.Parse(dscertInfo.Tables[0].Rows[0]["EvalCount"].ToString()) >= Int16.Parse(ConfigurationManager.AppSettings["CLSTRANS_COUNT"]))
                    return true;
            }
            return false;
        }

    }
}