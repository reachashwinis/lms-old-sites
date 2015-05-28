using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Com.Arubanetworks.Licensing.Dataaccesslayer;
using System.Net;
using Com.Arubanetworks.Licensing.Lib.Data;
using Com.Arubanetworks.Licensing.Lib.Utils;
using System.Web.SessionState;


/// <summary>
/// Summary description for User
/// </summary>
namespace Com.Arubanetworks.Licensing.Lib.Data
{

    public class UserInfo
    {
        public string ImpersonateEmail = string.Empty;
        public bool IsImpersonate = false;
        public string ImpersonateUserRole = string.Empty;
        public int ImpersonateUserId = -1;
        public int ImpersonateUserCompanyId = -1;
        public string ImpersonateFirstname = string.Empty;
        public string ImpersonateLastname = string.Empty;
        public string Role = string.Empty;
        public string FirstName = string.Empty;
        public string LastName = string.Empty;
        public string Password = string.Empty;
        public string Phone = string.Empty;
        public string Email = string.Empty;
        public string CompanyName = string.Empty;
        public int CompanyId = -1;
        public string Status = string.Empty;
        public string AcctActivationCode = string.Empty;
        public int AcctId = -1;
        public string Brand = string.Empty;
        public string Comments = string.Empty;
        public bool isCompanyAdmin = false;
        public string IPAddress;
 
         public UserInfo()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //public void GetValuesFromAuthMod(AuthUser.UserInfo objUserAuth)
        //{
        //    this.AcctId = objUserAuth.AcctId;
        //    this.FirstName = objUserAuth.FirstName;
        //    this.LastName = objUserAuth.LastName;
        //    this.Email = objUserAuth.Email;
        //    this.CompanyId = objUserAuth.CompanyId;
        //    this.CompanyName = objUserAuth.CompanyName;
        //    this.AppId = objUserAuth.AppId;
            
        //    this.AcctActivationCode = objUserAuth.AcctActivationCode;
        //    this.Phone= objUserAuth.Phone;
        //    this.Status = objUserAuth.Status;
        //    this.Password = objUserAuth.Password;

        //}


        public string GetUserRole()
        {
            if (this.IsImpersonate)
            {
                return this.ImpersonateUserRole;
            }
            return this.Role;
        }

        public string GetImpersonateUserRole()
        {
            if (this.IsImpersonate)
            {
                return this.Role;
            }
            return this.Role;
        }

        public  string GetUserEmail()
        {
            if (this.IsImpersonate)
            {
                return this.ImpersonateEmail;
            }
            return this.Email;
        }

        public string GetUserFirstName()
        {
            if (this.IsImpersonate)
            {
                return this.ImpersonateFirstname;
            }
            return this.FirstName;
        }

        public string GetUserLastName()
        {
            if (this.IsImpersonate)
            {
                return this.ImpersonateLastname;
            }
            return this.LastName;
        }

        public int GetUserAcctId()
        {
            if (this.IsImpersonate)
            {
                return this.ImpersonateUserId;
            }
            return this.AcctId;
        }

        public int GetUserCompanyId()
        {
            if (this.IsImpersonate)
            {
                return this.ImpersonateUserCompanyId;
            }
            return this.CompanyId;
        }
    }
    public class User : System.Web.UI.Page
    {
        public string USER_TBL="Users";
        public const string TABLE = "USER_INFO";
        private DAUser daoUser;
        #region ColumnNames
        public const string ACCT_ID="pk_acct_id";
        public const string FIRST_NAME="firstname";
        public const string LAST_NAME="lastname";
        public const string EMAIL ="email";
        public const string USER_ENTERED_COMPANY = "company";
        public const string ARUBA_ASSIGNED_COMPANY = "assigned_company";
        public const string PHONE = "phone";
        public const string PASSWORD = "password";
        public const string STATUS = "status";
        public const string BRAND = "brand";
        public const string CREATED_ON = "add_ts";
        public const string USER_TYPE = "cust_type";
        public const string FULL_NAME = "fullname";
        public const string COMPANY_ID = "company_id";
        public const string COMMENTS = "Comments";
        public const string ISCOMPANYADMIN = "IsCompanyAdmin";
        #endregion

        public enum AccountType
        { PendingAccounts, ExistingAccounts };

        public User()
        {
            daoUser = new DAUser();
            
        }
        //Added by ashwini

        public bool UpdateLastLogin(string strEmail, string APPID)
        {
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            bool retVal = objAuthMod.UpdateLastLogin(strEmail, APPID);
            return retVal;
        }

        public bool ActivateExpiredUser(string strActCode, string APPID)
        {
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            bool retVal = objAuthMod.ActivateExpiredUser(strActCode, APPID);
            return retVal;
        }

        public bool UpdateAccount(UserInfo objLicUser)
        {

            bool retval = false;
            bool retdata = true;
            bool blIsCompanyAdmin = false;
            AuthUser.UserInfo objMasterUser = new AuthUser.UserInfo();
            objMasterUser.FirstName = objLicUser.FirstName;
            objMasterUser.LastName = objLicUser.LastName;
            objMasterUser.CompanyName = objLicUser.CompanyName;
            objMasterUser.Email = objLicUser.Email;
            objMasterUser.Phone = objLicUser.Phone;
            objMasterUser.Status = objLicUser.Status;
            objMasterUser.Comments = objLicUser.Comments;
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            retdata = objAuthMod.UpdateAccount(objMasterUser);

            if (retdata == true)
            {
                blIsCompanyAdmin = daoUser.IsUserCompanyAdmin(objLicUser.Email);
                if (daoUser.UpdateAccount(objLicUser))
                {
                    Email objEmail = new Email();
                    AccountMailInfo objLgm = new AccountMailInfo();
                    if (objLicUser.isCompanyAdmin == true && blIsCompanyAdmin == false)
                    {                       
                        objLgm.Brand = objLicUser.Brand.ToUpper();
                        objLgm.Email = objLicUser.Email;
                        objLgm.IsCompanyAdmin = objLicUser.isCompanyAdmin;
                        objLgm.Name = objLicUser.FirstName + " " + objLicUser.LastName;
                        retval = objEmail.sendCompanyAdminEmail(objLgm);
                    }
                    else if (objLicUser.isCompanyAdmin == false && blIsCompanyAdmin == true)
                    {
                        objLgm.Brand = objLicUser.Brand.ToUpper();
                        objLgm.Email = objLicUser.Email;
                        objLgm.IsCompanyAdmin = objLicUser.isCompanyAdmin;
                        retval = objEmail.sendCompanyAdminDeactivate(objLgm);
                    }
                    retval = true;
                }
            }

            return retval;
        }

        public bool isArubaEmp(string strUserEmail)
        {
            UserInfo objLicUser = new UserInfo();
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            bool retValue = objAuthMod.IsArubaEmp(strUserEmail);
            return retValue;
        }

        public bool IsInactiveAccount(string strUserEmail, string strAppId, bool IsCust)
        {
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            bool retValue = objAuthMod.IsInactiveAccount(strUserEmail, strAppId, IsCust);
            return retValue;
        }

        public string getActivationCode(string strEmail, string strBrand)
        {
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            return objAuthMod.getActivationCode(strEmail,strBrand);
        }

        public UserInfo GetEmpInfo(string strUserEmail)
        {
            UserInfo objLicUser = new UserInfo();
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            AuthUser.UserInfo objAuthUser = objAuthMod.GetArubaEmp(strUserEmail);
            objLicUser.Email = objAuthUser.Email;
            objLicUser.FirstName = objAuthUser.FirstName;
            objLicUser.LastName = objAuthUser.LastName;
            objLicUser.Status = objAuthUser.Status;
            objLicUser.AcctActivationCode = objAuthUser.AcctActivationCode;
            objLicUser.CompanyId = objAuthUser.CompanyId;
            objLicUser.CompanyName = objAuthUser.CompanyName;
            objLicUser.Role = ConfigurationManager.AppSettings["DEFAULT_ROLE"].ToString();

            if (objLicUser == null)
            {
                return null;
            }
            return objLicUser;
        }

        public string getIPAddressCode(string strEmail)
        {
            UserInfo objLicUser = new UserInfo();
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            return  objAuthMod.getIPAddressCode(strEmail);
        }
              
        public UserInfo GetUserInfo(string strUserEmail,string strBrand)
        {
            //UserInfo objLicUser = new UserInfo();
            //AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            //objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            //AuthUser.UserInfo objAuthUser =  objAuthMod.GetUserInfo(strUserEmail, ConfigurationManager.AppSettings["APP_ID"]);
            //if (objAuthUser != null)
            //{
            //    objLicUser.GetValuesFromAuthMod(objAuthUser);
            //    if (!objLicUser.Status.ToUpper().Equals("ACTIVE"))
            //    {
            //        objLicUser = null;
            //    }

            //}
            //else
            //{
            //    objLicUser = null;
            //}
            //return objLicUser;

            UserInfo objLicUser ;
            DataSet ds = daoUser.GetUserInfo(strUserEmail,strBrand);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                objLicUser = null;
            else
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr[STATUS].ToString().ToUpper().Equals("INACTIVE"))
                    objLicUser = null;
                else
                {
                    objLicUser = new UserInfo();
                    objLicUser.AcctId = Int32.Parse(dr[ACCT_ID].ToString());
                    objLicUser.FirstName = dr[FIRST_NAME].ToString();
                    objLicUser.LastName = dr[LAST_NAME].ToString();
                    objLicUser.Phone = dr[PHONE].ToString();
                    objLicUser.Role = dr[USER_TYPE].ToString();
                    objLicUser.Status = dr[STATUS].ToString();
                    objLicUser.Brand = dr[BRAND].ToString();
                    objLicUser.CompanyName = dr[USER_ENTERED_COMPANY].ToString();
                    objLicUser.CompanyId = Int32.Parse(dr[COMPANY_ID].ToString());
                    objLicUser.Email = strUserEmail;
                }

            }
            return objLicUser;
            
        }

        public UserInfo GetUserInfo(int intUserAcctId,string strBrand)
        {
            DataSet ds = daoUser.GetUserInfo(intUserAcctId,strBrand);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            string strUserEmail = ds.Tables[0].Rows[0][EMAIL].ToString();

            return this.GetUserInfo(strUserEmail,strBrand);
        }

        public DataSet GetAccountInfo(int intUserAcctId,string strBrand)
        {
            DataSet ds = daoUser.GetUserInfo(intUserAcctId,strBrand);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            return ds;

        }

        public string AuthenticateUser(string strUserEmail, string strPassword,string UserIPAddr)
        {
            try
            {
                AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
                objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
                string st = Session["APPID"].ToString();
                AuthUser.AuthResult objAuthResult = objAuthMod.AuthenticateUser(strUserEmail, strPassword, Session["APPID"].ToString(), UserIPAddr);
                string retVal = string.Empty;
                if (!objAuthResult.IsSuccess)
                {
                    retVal = objAuthResult.ErrMessage;
                }
                else
                {
                    retVal = objAuthResult.ErrMessage;
                }
                ////l  AuthUser.UserInfo objAuthUser = objAuthMod.AuthenticateUser(strUserEmail, strPassword, ConfigurationManager.AppSettings["APP_ID"]);
                //  if (objAuthUser != null)
                //  {
                //      //objLicUser.GetValuesFromAuthMod(objAuthUser);
                //      objLicUser  = this.GetUserInfo(strUserEmail);


                //  }
                //  else
                //  {
                //      objLicUser = null;
                //  }
                //  return objLicUser;
                return retVal;
            }
            catch (WebException wex)
                {
                    throw wex;
                }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }

        public string AuthenticateUser(string strUserEmail, string UserIPAddr)
        {
            try
            {
                AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
                objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
                string st = Session["APPID"].ToString();
                AuthUser.AuthResult objAuthResult = objAuthMod.AuthenticateUserbyEmail(strUserEmail, Session["APPID"].ToString(), UserIPAddr);
                string retVal = string.Empty;
                if (!objAuthResult.IsSuccess)
                {
                    retVal = objAuthResult.ErrMessage;
                }
                else
                {
                    retVal = objAuthResult.ErrMessage;
                }
                ////l  AuthUser.UserInfo objAuthUser = objAuthMod.AuthenticateUser(strUserEmail, strPassword, ConfigurationManager.AppSettings["APP_ID"]);
                //  if (objAuthUser != null)
                //  {
                //      //objLicUser.GetValuesFromAuthMod(objAuthUser);
                //      objLicUser  = this.GetUserInfo(strUserEmail);


                //  }
                //  else
                //  {
                //      objLicUser = null;
                //  }
                //  return objLicUser;
                return retVal;
            }
            catch (WebException wex)
            {
                throw wex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetMembers(int intCompanyId, int intPageSize, int intPageNum, string Filter)
        {
            return daoUser.GetMembers(intCompanyId, intPageSize, intPageNum, Filter);
        }

        public DataSet CheckWithAccounts(string EmailList, AccountType enumAccountType)
        {
            DataSet ds = new DataSet();
            switch (enumAccountType)
            { 
                    
                case AccountType.ExistingAccounts:
                    ds= daoUser.CheckExistingAccounts(EmailList);
                    break;
                case AccountType.PendingAccounts:
                    ds= daoUser.CheckPendingAccounts(EmailList);
                    break;
            
            }

            return ds;
        }

        public DataSet CheckWithGroupAccounts(string EmailList, AccountType enumAccountType)
        {
            DataSet ds = new DataSet();
            switch (enumAccountType)
            { 
                    
                case AccountType.ExistingAccounts:
                    ds= daoUser.CheckExistingAccounts(EmailList);
                    break;
                case AccountType.PendingAccounts:
                    ds = daoUser.CheckPendingGroupAccounts(EmailList);
                    break;
            
            }

            return ds;
        }
       

        public DataSet AddPendingAccounts(string Email, int CompanyId, string Brand)
        {
            return daoUser.AddPendingAccounts(Email, CompanyId, Brand);
        }

        public DataSet GetUngroupedAccounts(string strCompanyType, int intPageSize, int intPageNum, string Filter)
        {
            return daoUser.GetUngroupedAccounts(strCompanyType, intPageSize, intPageNum, Filter);
        }

        public bool GroupCurrentAccounts(int intCompanyId, string CommaSeperatedAcctIds)
        {
            return daoUser.GroupCurrentAccounts(intCompanyId, CommaSeperatedAcctIds);
        }

        public bool ChangePassword(string strUserEmail, string strOldPassword, string strNewPassword)
        {

            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            if (objAuthMod.ChangePassword(strUserEmail, strOldPassword, strNewPassword))
                return true;
            else
                return false;
        
        }

        public bool ResetPassword(string strUserEmail,string strNewPassword)
        {

            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            if (objAuthMod.ResetPassword (strUserEmail,strNewPassword))
                return true;
            else
                return false;
        }

        public DataSet GetAccountsList(int intPageSize, int intPageNum, string strFilter, string strBrand)
        {
            return daoUser.GetAccountsList(intPageSize, intPageNum, strFilter,strBrand);
        }

        public DataSet GetUnAssignedAccounts(int intPageSize, int intPageNum, string strFilter, string strBrand)
        {
            return daoUser.GetUnAssignedAccounts(intPageSize, intPageNum, strFilter, strBrand);
        }

        public DataSet GetCompanyAccounts(int intPageSize, int intPageNum, string strFilter, string strBrand, string companyid)
        {
            return daoUser.GetCompanyAccounts(intPageSize, intPageNum, strFilter, strBrand, companyid);
        }

        public bool AddUser(UserInfo objLicUser)
        {
            bool retval = true;
            bool retdata = true;
            DataSet dsResult;
            AuthUser.UserInfo objMasterUser = new AuthUser.UserInfo();
            objMasterUser.FirstName = objLicUser.FirstName;
            objMasterUser.LastName = objLicUser.LastName;
            objMasterUser.CompanyName = objLicUser.CompanyName;
            objMasterUser.Email = objLicUser.Email;
            objMasterUser.Password = objLicUser.Password;
            objMasterUser.Status = objLicUser.Status;
            objMasterUser.AppId = Session["APPID"].ToString();
            objMasterUser.Phone = objLicUser.Phone;
            objMasterUser.Comments = objLicUser.Comments;
            objMasterUser.UserIPAddress = objLicUser.IPAddress;

            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            dsResult = objAuthMod.AddUserInfo(objMasterUser);
            if (dsResult.Tables[0].Rows.Count == 1)
            {
                retdata = true;
            }
            else
            {
                retdata = false;
            }

            if (retdata == true)
            {
                if (daoUser.AddUser(objLicUser))
                {
                    //send email
                    if (objLicUser.AcctActivationCode.ToString() == string.Empty)
                    {
                        Email objEmail = new Email();
                        AccountMailInfo objLgm = new AccountMailInfo();

                        //objLgm.ActivationCode = objLicUser.AcctActivationCode;
                        objLgm.Brand = objLicUser.Brand.ToUpper();
                        objLgm.Email = objLicUser.Email;
                        objLgm.Password = objLicUser.Password;
                        objLgm.IsCompanyAdmin = objLicUser.isCompanyAdmin;
                        objLgm.ActivationCode = dsResult.Tables[0].Rows[0]["AcctCode"].ToString();                        
                        if (objLicUser.Status == "Active" && objLicUser.Comments != "AIRW")
                        {
                            retval = objEmail.sendArubaAccountInfo(objLgm);
                        }
                        else
                        {
                            switch (objLicUser.Role)
                            {
                                case UserType.Distributor:
                                    retval = objEmail.sendDistributorActivationInfo(objLgm);
                                    break;
                                case UserType.Reseller:
                                    retval = objEmail.sendResellerActivationInfo(objLgm);
                                    break;
                                default:
                                    if (objLicUser.Comments != "AIRW")
                                    {
                                        retval = objEmail.sendCaActivationInfo(objLgm);
                                    }
                                    break;
                            }
                        }
                        if (objLgm.IsCompanyAdmin == true)
                        {
                            retval = objEmail.sendCompanyAdminEmail(objLgm);
                        }
                    }
                }
                else
                {
                    retval = false;
                }
            }

            else 
            {
                retval = false;
            }
            return retval;
        }

        public string IsEmailExists(string strUserEmail, bool checkPendingAccounts)
        {

            string strError = string.Empty;

            if (GetUserInfo(strUserEmail,Session["BRAND"].ToString()) == null)
            {
                if (checkPendingAccounts == true)
                {
                    DataSet ds = CheckWithAccounts(strUserEmail, AccountType.PendingAccounts);
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        strError = "This account creation is pending activation by user";
                    }
                }
            }
            else
            {
                strError = "This User Email already exists in the system";
            }

            return strError;

        }

        public bool IsWindowsLoginIDSSO(string strEmail, string APPID)
        {
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            return objAuthMod.IsWindowsLoginID(strEmail, APPID);
        }

        public bool IsEmailExistsSSO(string strEmail)
        {
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            return objAuthMod.IsEmailExists(strEmail);
        }

        public bool IsRestrictedDomain(string strEmail)
        {
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            return objAuthMod.IsRestrictedUser(strEmail); 
        }

        public bool IsRestrictedDomain(string strEmail, bool blIsNewUser)
        {
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            return objAuthMod.IsRestrictedDomain(strEmail, blIsNewUser);
        }

        public bool IsOnlyAdmin(string strEmail, int CompanyId)
        {
            return daoUser.IsOnlyAdmin(strEmail, CompanyId);
        }

        public bool ActivateUser(string strActivationCode,string strAppID)
        {
            bool retVal = false;
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            if (objAuthMod.ActivateUser(strActivationCode, strAppID))
            {
                string strEmail = objAuthMod.GetUserEmail(strActivationCode);
                if (!strEmail.Equals(string.Empty))
                {
                    if (daoUser.UpdateAccountStatus(strEmail, "ACTIVE"))
                    {
                        retVal = true;

                    }
                }

            }

            return retVal;
        
        }

        public bool UpdateIPAddress(string strIPActivationCode, string strIPAddress)
        {
            bool retVal = false;
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            if (objAuthMod.UpdateIPAddress(strIPActivationCode, strIPAddress))
            {
                retVal = true;
            }

            return retVal;

        }

        public DataSet GetPendingAccount(string strActivationCode)
        {
            return daoUser.GetPendingAccount(strActivationCode);
        }

        public bool RemovePendingAccount(string strActivationCode)
        {
            return daoUser.RemovePendingAccount(strActivationCode);
        }

        public bool UpdateUserEmailId (string strEmailId, string strOldEmailId)
        {
            bool retVal = false;
            AuthUser.Authentication objAuthMod = new AuthUser.Authentication();
            objAuthMod.Url = ConfigurationManager.AppSettings["AuthUser.URL"];
            if (objAuthMod.UpdateUserEmailId(strEmailId, strOldEmailId))
            {
                retVal = true;
            }
            if (retVal == true)
            {
                retVal = daoUser.UpdateUserEmailId(strEmailId, strOldEmailId);
            }
            return retVal;
        }

    }
    public sealed class UserType
    {
        public const string Customer = "customer";
        public const string ArubaGod = "aruba-god";
        public const string ArubaCA = "aruba-ca";
        public const string ArubaSales = "aruba-sales";
        public const string Distributor = "distributor";
        public const string Reseller = "reseller";
    }

    


}