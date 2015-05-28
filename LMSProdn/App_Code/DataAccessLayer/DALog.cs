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

/// <summary>
/// Summary description for DALog
/// </summary>
namespace Com.Arubanetworks.Licensing.Dataaccesslayer
{
    public class DALog
    {

        private const string CONST_ACTIVATION = "activate";
        private const string CONST_ASSIGN = "assign";
        private const string CONST_TRANSFER = "transfer";
        private const string CONST_UNASSIGN = "unassign";
        private const string CONST_CONSOLIDATE = "consolidate";

        public DALog()
        {

        }

        public int LogActivationInfo(int intAcctId, int intCertId, int intTargetId, SqlTransaction objTrans)
        {
            return addEvent(CONST_ACTIVATION, intAcctId, intCertId, intTargetId, objTrans);
        }

        public int LogClpActivationInfo(int intAcctId, int IntCertId, int ImpAcctId, SqlTransaction objTrans)
        {
            return addClpEvent(CONST_ACTIVATION, intAcctId, IntCertId, ImpAcctId, objTrans);
        }

        public int LogAirwaveTransferInfo(int intAcctId, string strCertId, string strTargetId, SqlTransaction objTrans)
        {
            return addAirwaveEvent(CONST_TRANSFER, intAcctId, strCertId, strTargetId, objTrans);
        }

        public int LogAirwaveActivationInfo(int intAcctId, string strCertId, string strTargetId, SqlTransaction objTrans)
        {
            return addAirwaveEvent(CONST_ACTIVATION, intAcctId, strCertId, strTargetId, objTrans);
        }

        public int LogAirwaveConsolidationInfo(int intAcctId, string strCertId, string strTargetId, SqlTransaction objTrans)
        {
            return addAirwaveEvent(CONST_CONSOLIDATE, intAcctId, strCertId, strTargetId, objTrans);
        }

        public int LogAssignInfo(int intAcctId, int intCertId, int intTargetId, SqlTransaction objTrans)
        {
            return addEvent(CONST_ASSIGN, intAcctId, intCertId, intTargetId, objTrans);
        }

        public int LogAssignAWInfo(int intAcctId, int intCertId, int intTargetId, SqlTransaction objTrans)
        {
            return addAirwaveEvent(CONST_ASSIGN, intAcctId, intCertId, intTargetId.ToString(), objTrans);
        }

        public int LogAssignClpInfo(int intAcctId, int intCertId, int intTargetId, SqlTransaction objTrans)
        {
            return addClpEvent(CONST_ASSIGN, intAcctId, intCertId, intTargetId, objTrans);
        }

        public int LogTransferInfo(int intAcctId, int intCertId, int intTargetId, SqlTransaction objTrans)
        {
            return addEvent(CONST_TRANSFER, intAcctId, intCertId, intTargetId, objTrans);
        }

        //Added on May
        public int UpdateTransferCount(int intCertId, SqlTransaction objTrans)
        {
            SqlParameter spParam = new SqlParameter("@certID", SqlDbType.Int); spParam.Value = intCertId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_UpdateTransCount", lstParam.ToArray());

            return noOfRowsAffected;
        }

        public int LogUnassignInfo(int intAcctId, int intCertId, int intTargetId, SqlTransaction objTrans)
        {
            return addEvent(CONST_UNASSIGN, intAcctId, intCertId, intTargetId, objTrans);
        }

        public int LogUnassignAWInfo(int intAcctId, int intCertId, int intTargetId, SqlTransaction objTrans)
        {
            return addAirwaveEvent(CONST_UNASSIGN, intAcctId, intCertId, intTargetId.ToString(), objTrans);
        }

        public int LogUnassignClpInfo(int intAcctId, int intCertId, int intTargetId, SqlTransaction objTrans)
        {
            return addClpEvent(CONST_UNASSIGN, intAcctId, intCertId, intTargetId, objTrans);
        }

        //@param  string  $type      the type of event
        //@param  integer $acct_id   PK for the account performing the event
        //@param  integer $cert_id   PK for the certificate being acted upon
        //@param  integer $target_id PK for the event recipient
        private int addEvent(string strAction, int intAcctId, int intCertId, int intTargetId, SqlTransaction objTrans)
        {

            SqlParameter spParam = new SqlParameter("@certID", SqlDbType.Int); spParam.Value = intCertId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctId;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@action", SqlDbType.VarChar, 25); spParam.Value = strAction;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@targetId", SqlDbType.Int);
            spParam.IsNullable = true;
            spParam.Value = intTargetId;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_addEvent", lstParam.ToArray());

            return noOfRowsAffected;
        }

        private int addAirwaveEvent(string strAction, int intAcctId, string strCertId, string strTargetId, SqlTransaction objTrans)
        {

            SqlParameter spParam = new SqlParameter("@certID", SqlDbType.VarChar, 100); spParam.Value = strCertId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctId;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@action", SqlDbType.VarChar, 25); spParam.Value = strAction;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@targetId", SqlDbType.VarChar, 50);
            spParam.IsNullable = true;
            spParam.Value = strTargetId;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_addAirwaveEvent", lstParam.ToArray());

            return noOfRowsAffected;
        }

        private int addAirwaveEvent(string strAction, int intAcctId, int intCertId, string strTargetId, SqlTransaction objTrans)
        {

            SqlParameter spParam = new SqlParameter("@certID", SqlDbType.VarChar, 100); spParam.Value = intCertId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctId;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@action", SqlDbType.VarChar, 25); spParam.Value = strAction;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@targetId", SqlDbType.VarChar, 50);
            spParam.IsNullable = true;
            spParam.Value = strTargetId;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_addAirwaveEventByCertId", lstParam.ToArray());

            return noOfRowsAffected;
        }

        private int addClpEvent(string strAction, int intAcctId, int IntCertId, int ImpAcctId, SqlTransaction objTrans)
        {

            SqlParameter spParam = new SqlParameter("@certID", SqlDbType.VarChar, 100); spParam.Value = IntCertId;
            List<SqlParameter> lstParam = new List<SqlParameter>();
            lstParam.Add(spParam);
            spParam = new SqlParameter("@acctId", SqlDbType.Int); spParam.Value = intAcctId;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@action", SqlDbType.VarChar, 25); spParam.Value = strAction;
            lstParam.Add(spParam);
            spParam = new SqlParameter("@ImpacctId", SqlDbType.Int);
            spParam.IsNullable = true;
            spParam.Value = ImpAcctId;
            lstParam.Add(spParam);

            int noOfRowsAffected = SqlHelper.ExecuteNonQuery(objTrans, CommandType.StoredProcedure, "LMS_addClearPassEvent", lstParam.ToArray());

            return noOfRowsAffected;
        }
    }
}
