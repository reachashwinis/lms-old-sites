using System;
using System.Net;
using System.Net.Mail;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Com.Arubanetworks.Licensing.Lib.Data;
using Com.Arubanetworks.Licensing.Dataaccesslayer;
using System.Web.Caching;
using System.IO;
using System.Collections;
using System.Net.Mime;

/// <summary>
/// Summary description for Email
/// </summary>
namespace Com.Arubanetworks.Licensing.Lib.Utils
{
    public class Email 
    {
        //private DAPEmail.Service objEmail;
        bool retVal;
        string bcc = string.Empty;

        public Email()
        {
            //objEmail = new DAPEmail.Service();
            //objEmail.Url = ConfigurationManager.AppSettings["DAPEmail.URL"];  
        }

        public bool sendLoginInfo(AccountMailInfo objLgm)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "You or someone pretending to be you has requested the login information to your Aruba Wireless Networks Licensing web site account.  We have reset your password for security reasons.<BR/>" +
                                   "Your new login info is as follows:<BR/>" +
                                   "Username:" + objLgm.Email + "<BR/>" +
                                   "Password:" + objLgm.Password + "<BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "<BR/><BR/>" +
                                   "If you did not make this request, immediately contact Aruba Wireless Networks at licensing@arubanetworks.com.<BR/>" +
                                   "Aruba Networks Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"];
                    sub = "Your Aruba License Management Website account info";
                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    Message = "You or someone pretending to be you has requested the login information to your Alcatel OmniAccess Licensing web site account.  We have reset your password for security reasons.<BR/>" +
                                    "Your new login info is as follows:<BR/>" +
                                     "Username:" + objLgm.Email + "<BR/>" +
                                   "Password:" + objLgm.Password + "<BR/>" +
                                    ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "<BR/><BR/>" +
                                   "If you did not make this request, immediately contact Alcatel Customer Support at support@ind.alcatel.com.<BR/>" +
                                   "Alcatel Internetworking Licensing Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ALCATEL_SITE_URL"];

                    sub = "Your Alcatel OmniAccess License Management Website account info";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }
            try
            {

                //added by ashwini
                //bool retVal = objEmail.sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
                if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
                {
                    bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                    retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
                }
                else
                {
                    retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
                }

                if (!retVal)
                {
                    new Log().logSystemError("sendLoginInfo", "Mail not sent:" + objLgm.GetString());
                }
                else
                {
                    new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", "", sub, Message, "", "SEND_LOGIN_INFO");
                }

                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool sendAccountActivationInfo(AccountMailInfo objLgm)
        {

            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "Your new Aruba License Management account has been created.<BR/><BR/>" +
                                    "To complete the registration on our site, please click on the link below.<BR/>" +
                                     ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAccount.aspx?k=" + objLgm.ActivationCode + "<BR/>" +
                                    "If you are unable to use the link you will have to enter the following code into the activation page.<BR/>" +
                                    "Code: " + objLgm.ActivationCode + "<BR/>" +
                                    "Page: " + ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAccount.aspx" + "<BR/><BR/>" +
                                   "If you did not create a new account, immediately contact Aruba Wireless Networks at licensing@arubanetworks.com.<BR/>" +
                                   "Aruba Networks Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"];

                    sub = "Aruba License Management Website Activation";

                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    Message = "Your new Alcatel OmniAccess License Management has been created.<BR/><BR/>" +
                                  "To complete the registration on our site, please click on the link below.<BR/>" +
                                   ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAccount.aspx?k=" + objLgm.ActivationCode + "<BR/>" +
                                "If you are unable to use the link you will have to enter the following code into the activation page.<BR/>" +
                                  "Code: " + objLgm.ActivationCode + "<BR/>" +
                                  "Page: " + ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAccount.aspx" + "<BR/><BR/>" +
                                 "If you did not create a new account, immediately contact Alcatel customer support at support@ind.alcatel.com.<BR/>" +
                                 "Alcatel OmniAccess Licensing Webmaster<BR/>" +
                                 ConfigurationManager.AppSettings["ALCATEL_SITE_URL"];

                    sub = "Alcatel OmniAccess License Management Website Activation";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendAccountActivationInfo", "Mail not sent:" + objLgm.GetString());

            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", "", sub, Message, "", "ACCOUNT_ACTIVATION");
            }

            return retVal;

        }

        public bool sendIPActivationInfo(AccountMailInfo objLgm)
        {

            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "You are trying to access Licensing Site with different IP Address.<BR/><BR/>" +
                                    "To ReConfigure the IP Address,Please click on the link below.<BR/>" +
                                     ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAccountIP.aspx?k=" + objLgm.ActivationCode + "<BR/>" +
                                    "If you are unable to use the link you will have to enter the following User Id into the activation page.<BR/>" +
                                    "Code: " + objLgm.ActivationCode + "<BR/>" +
                                    "Page: " + ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAccountIP.aspx" + "<BR/><BR/>" +
                                   "If you did not login to the site, immediately contact Aruba customer support at licensing@arubanetworks.com.<BR/>" +
                                   "Aruba Networks Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"];

                    sub = "Aruba License Management Account";

                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    Message = "You are trying to access Alcatel Licensing Site with different IP Address.<BR/><BR/>" +
                                    "To ReConfigure the IP Address,Please click on the link below.<BR/>" +
                                     ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAccountIP.aspx?k=" + objLgm.ActivationCode + "<BR/>" +
                                    "If you are unable to use the link you will have to enter the following code into the activation page.<BR/>" +
                                    "Code: " + objLgm.ActivationCode + "<BR/>" +
                                    "Page: " + ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAccountIP.aspx" + "<BR/><BR/>" +
                                   "If you did not login to the site, immediately contact Alcatel customer support at support@ind.alcatel.com.<BR/>" +
                                   "Aruba Networks Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ALCATEL_SITE_URL"];

                    sub = "Alcatel OmniAccess License Management Account";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendIPActivationInfo", "Mail not sent:" + objLgm.GetString());

            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", "", sub, Message, "", "IP_ACTIVATION");
            }

            return retVal;

        }

        public bool sendActivationInfo(AccountMailInfo objLgm)
        {

            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "Your account has been temporarily deactivated due to no activity.<BR/><BR/>" +
                              "Please click on below link to reinstate your account.<BR/>" +
                               ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAcct.aspx?k=" + objLgm.ActivationCode.Trim() + "<BR/><BR/>" +
                              "If you are unable to use the above link, please enter the following User ID on the activation page.<BR/><BR/>" +
                               "Code: " + objLgm.ActivationCode + "<BR/>" +
                               "Page: " + ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAcct.aspx" + "<BR/><BR/>" +
                               "If you need further assistance activating the account, please contact Aruba Customer Support at licensing@arubanetworks.com.<BR/><BR/>" +
                               "Aruba Networks Webmaster <BR/>" +
                                ConfigurationManager.AppSettings["ARUBA_SITE_URL"];

                    sub = "Aruba License Management Account";

                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    Message = "Your account has been temporarily deactivated due to no activity.<BR/><BR/>" +
                               "Please click on below link to reinstate your account.<BR/>" +
                               ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAcct.aspx?k=" + objLgm.ActivationCode.Trim() + "<BR/>" +
                               "If you are unable to use the above link, please enter the following User ID on the activation page.<BR/><BR/>" +
                               "Code: " + objLgm.ActivationCode + "<BR/>" +
                               "Page: " + ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAcct.aspx" + "<BR/><BR/>" +
                               "If you need further assistance activating the account,Please contact Alcatel customer support at support@ind.alcatel.com.<BR/>" +
                               "Alcatel Networks Webmaster <BR/>" +
                                ConfigurationManager.AppSettings["ALCATEL_SITE_URL"];

                    sub = "Alcatel OmniAccess License Management Account";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendIPActivationInfo", "Mail not sent:" + objLgm.GetString());

            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", "", sub, Message, "", "IP_ACTIVATION");
            }

            return retVal;

        }

        public bool sendAirwvEvalCertInfo(AirwaveCertInfo objAirwaveCertInfo, string strCCEmail)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objAirwaveCertInfo.Brand)
            {
                case "ARUBA":
                    Message = " Thank you for your request " + objAirwaveCertInfo.Name + " (" + objAirwaveCertInfo.Email + ")" + "<BR/><BR/>" +
                    " This email is your electronic confirmation of your request for an Aruba Networks Evaluation Software License as follows:<BR/>" +
                    "Aruba Part Number: " + objAirwaveCertInfo.Package + "<BR/>" +
                    "Description            : " + objAirwaveCertInfo.PackageDesc + "<BR/>" +
                    "License Key    : <BR><BR><BR>" + objAirwaveCertInfo.CertId.Replace("\n", "<BR>") + "<BR/><BR/>" +
                    "This key can be used to enable the required functionality on Airwave server for 90 days for trial purposes.<BR/><BR/>";
                    Message = Message + "<B>INSTALLING AirWave ON A NEW SERVER: </B><BR><BR>";
                    if (objAirwaveCertInfo.Password == string.Empty)
                    {
                        Message = Message + "1.Login " + ConfigurationSettings.AppSettings["ARUBA_SITE_URL"].ToString() + "<BR><BR>";
                    }
                    else
                    {
                        Message = Message + "1.Login " + ConfigurationSettings.AppSettings["ARUBA_SITE_URL"].ToString() + " by <BR><BR>";
                        Message = Message + "User Name:" + objAirwaveCertInfo.Email + "<BR>";
                        Message = Message + "Password:" + objAirwaveCertInfo.Password + "<BR><BR>";
                    }
                    Message = Message + "2. Download the AirWave software image from the utility <I>My AirWave > Download AirWave Software</I> of " + ConfigurationSettings.AppSettings["ARUBA_SITE_URL"].ToString() + " by right clicking the appropriate link and selecting the <I><B>Save As </I></B> or <I><B>Save Link As </I></B> option saving the file with a .iso ";
                    Message = Message + "extension (iso burning instructions can be found in the AirWave Quick Start Guide available at http://support.arubanetworks.com). <BR><BR>";
                    Message = Message + "3. Burn a CD from the AMP software image.(iso burning instructions can be found in the AirWave Quick Start Guide available at http://support.arubanetworks.com)<BR><BR>";
                    Message = Message + "4. Place the CD in your AirWave server and restart the machine. The computer will boot off of the CD and begin the install phase.<BR>";
                    Message = Message + "<I>(Note: Recommended hardware configurations and complete installation instructions can be found in the User Guide).</I> <BR><BR>";
                    Message = Message + "5. After installing the AirWave Management System software on your server, you will be prompted to enter in a license key.Copy the temporary license key including the 'Begin AMP License Key' and 'End AMP License Key' lines. <BR><BR>";
                    Message = Message + "6.	Navigate to <I>AMP Home-->License page</I> on your AirWave server to enter your permanent AirWave Software License Key.<BR><BR>";
                    Message = Message + "7. Accept the license terms.<BR><BR>";
                    Message = Message + "If you encounter any difficulty during the installation of the AirWave software or at any time during your use of AirWave, please do not hesitate to contact us.<BR><BR>";
                    Message = Message + "If you have any difficulty installing AirWave or have any questions, please contact Aruba Networks Technical Support at ";
                    Message = Message + "support@arubanetworks.com  or call 1-800-943-4526(U.S.A) or +1-408-419-4098 (International)<BR><BR> We look forward to helping you manage your wireless network.<BR><BR>Thank you for being an Aruba customer.<BR><BR><BR>";

                    sub = "Confirmation - Airwave Eval Software License Key";

                    //bcc = "dl-support@arubanetworks.com";

                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    Message = "Thank you for your request " + objAirwaveCertInfo.Name + " (" + objAirwaveCertInfo.Email + ")" + "<BR/><BR/>" +
                                    "This email is your electronic confirmation of your request for an Alcatel OmniAccess WLAN Evaluation Software License as follows:<BR/>" +
                                    "Alcatel Part Number: " + objAirwaveCertInfo.Package + "<BR/>" +
                                    "Description            : " + objAirwaveCertInfo.PackageDesc + "<BR/>" +
                                    "License Key    : <BR>" + objAirwaveCertInfo.CertId.Replace("\n", "<BR>") + "<BR/><BR/>" +
                                    "This key can be used to enable the required functionality in AirWave Server for 90 days for trial purposes.<BR/><BR/>" +
                                    "This electronic copy of the transaction serves as your confirmation. <BR/>" +
                                    "Should you experience difficulties, please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>" +
                                    "Email:      support@ind.alcatel.com<BR/>" +
                                    "Telephone:  1-800-995-2696  (North America)<BR/>" +
                                    "                  1-877-919-9526  (Latin America) <BR/>" +
                                    "                  +33-38-855-6929 (Europe) <BR/>" +
                                    "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>" +
                                    "Thank you for being an Alcatel OmniAccess WLAN Licensing customer.<BR/>" +
                                    "Sent by Alcatel OmniAccess Licensing Manager<BR/>" +
                                    "http://www.alcatel.com/enterprise   <BR/>";

                    sub = "Confirmation - AirWave Eval Software License Key";

                    //bcc="support@ind.alctel.com";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;
            }
            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objCgm.Email, "", sub, Message, bcc);            
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAirwaveCertInfo.Email, strCCEmail, sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[0], objAirwaveCertInfo.Email, strCCEmail, sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendEvalCertInfo", "Mail not sent:" + objAirwaveCertInfo.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAirwaveCertInfo.Email, "", bcc, sub, Message, "", "EVAL_CERT_INFO");
            }
            return retVal;
        }

        public bool sendAirwaveActivationInfo(AirwaveCertInfo objAirwaveCertInfo)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };

            switch (objAirwaveCertInfo.Brand)
            {
                case "ARUBA":
                    {
                        Message = " Thank you for your transaction " + objAirwaveCertInfo.Name + "(" + objAirwaveCertInfo.Email + ")" + ",<BR/><BR/>" +
                       " This email is your electronic confirmation of your Aruba Networks Software License Key generation as follows:<BR/>" +
                       "Aruba Part Number : " + objAirwaveCertInfo.Package + "<BR/>" +
                       "Description             : " + objAirwaveCertInfo.PackageDesc + "<BR/>" +
                       "System IP Address : " + objAirwaveCertInfo.IPAddress + "<BR/>" +
                       "License Key            : <BR>" + objAirwaveCertInfo.Activationkey.Replace("\n", "<BR>") + "<BR/><BR/>" +
                      "This License key can be used only on the Aruba AirWave Wireless Management Suite with the above stated system IP Address to enable the required functionality.<BR/><BR/>" +
                       "To enable the software feature, please login (with Administrator access rights) to your AirWave Server, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                       "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR/>" +
                       "Email:      support@arubanetworks.com <BR/>" +
                       "Telephone:  1-800-943-4526 (USA) <BR/>" +
                        "                 1-408-754-1201 (International) <BR/><BR/>" +
                        "Thank you for being an Aruba Networks customer.<BR/>" +
                        "Sent by Aruba Networks License Manager<BR/>" +
                        "http://www.arubanetworks.com  <BR/>";

                        sub = "Aruba License Activation Info";
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        Message = " Thank you for your transaction " + objAirwaveCertInfo.Name + "(" + objAirwaveCertInfo.Email + ")" + ",<BR/><BR/>" +
                          " This email is your electronic confirmation of your Alcatel OmniAccess Software License Key generation as follows:<BR/>" +
                          "Alcatel Part Number : " + objAirwaveCertInfo.Package + "<BR/>" +
                          "Description             : " + objAirwaveCertInfo.PackageDesc + "<BR/>" +
                          "System IP Address : " + objAirwaveCertInfo.IPAddress + "<BR/>" +
                          "License Key            : <BR>" + objAirwaveCertInfo.Activationkey.Replace("\n", "<BR>") + "<BR/><BR/>" +
                         "This License key can be used only on the Alcatel AirWave Wireless Management Suite with the above stated system IP Address to enable the required functionality.<BR/><BR/>" +
                         "To enable the software feature, please login (with Administrator access rights) to your AirWave Server, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                            "Should you experience difficulties, please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>" +
                           "Email:      support@ind.alcatel.com<BR/>" +
                            "Telephone:  1-800-995-2696  (North America)<BR/>" +
                            "                  1-877-919-9526  (Latin America) <BR/>" +
                            "                  +33-38-855-6929 (Europe) <BR/>" +
                            "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>" +
                            "Thank you for being an Alcatel OmniAccess WLAN Licensing customer.<BR/>" +
                            "Sent by Alcatel OmniAccess Licensing Manager<BR/>" +
                            "http://www.alcatel.com/enterprise   <BR/>";
                        sub = "Alcatel OmniAccess License Activation Info";
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                        break;
                    }
            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, Bcc);
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAirwaveCertInfo.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objAirwaveCertInfo.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendPreparedAccountCreation", "Mail not sent:" + objAirwaveCertInfo.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAirwaveCertInfo.Email, "", bcc, sub, Message, "", "PREPARED_ACCOUNT_CREATION");
            }

            return retVal;
        }

        public bool sendAirwaveConsolidateActInfo(AirwaveCertInfo objAirwaveCertInfo)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };

            switch (objAirwaveCertInfo.Brand)
            {
                case "ARUBA":
                    {
                        Message = " Thank you for your transaction " + objAirwaveCertInfo.Name + "(" + objAirwaveCertInfo.Email + ")" + ",<BR/><BR/>" +
                       " This email is your electronic confirmation of your Aruba Networks Software Consolidated License Key generation as follows:<BR/>" +
                       "Consolidated Aruba Part Number : " + objAirwaveCertInfo.Package + "<BR/>" +
                       "Consolidated Aruba part Number : " + objAirwaveCertInfo.PackageDesc + "<BR/>" +
                       "System IP Address : " + objAirwaveCertInfo.IPAddress + "<BR/>" +
                       "License Key            : " + objAirwaveCertInfo.Activationkey.Replace("\n", "<BR>") + "<BR/><BR/>" +
                       "This Consolidated License key can be used only on the Aruba AirWave Wireless Management Suite with the above stated system IP Address to enable the required functionality.<BR/><BR/>" +
                       "To enable the software feature, please login (with Administrator access rights) to your AirWave Server, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                       "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR/>" +
                       "Email:      support@arubanetworks.com <BR/>" +
                       "Telephone:  1-800-943-4526 (USA) <BR/>" +
                        "                 1-408-754-1201 (International) <BR/><BR/>" +
                        "Thank you for being an Aruba Networks customer.<BR/>" +
                        "Sent by Aruba Networks License Manager<BR/>" +
                        "http://www.arubanetworks.com  <BR/>";

                        sub = "Aruba License Activation Info";
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        Message = " Thank you for your transaction " + objAirwaveCertInfo.Name + "(" + objAirwaveCertInfo.Email + ")" + ",<BR/><BR/>" +
                          " This email is your electronic confirmation of your Alcatel OmniAccess Software Consolidated License Key generation as follows:<BR/>" +
                          "Consolidated Alcatel Part Number : " + objAirwaveCertInfo.Package + "<BR/>" +
                          "Consolidated Alcatel Part Number : " + objAirwaveCertInfo.PackageDesc + "<BR/>" +
                          "System IP Address : " + objAirwaveCertInfo.IPAddress + "<BR/>" +
                          "License Key            : " + objAirwaveCertInfo.Activationkey.Replace("\n", "<BR>") + "<BR/><BR/>" +
                          "This Consolidated License key can be used only on the Alcatel AirWave Wireless Management Suite with the above stated system IP Address to enable the required functionality.<BR/><BR/>" +
                          "To enable the software feature, please login (with Administrator access rights) to your AirWave Server, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                            "Should you experience difficulties, please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>" +
                           "Email:      support@ind.alcatel.com<BR/>" +
                            "Telephone:  1-800-995-2696  (North America)<BR/>" +
                            "                  1-877-919-9526  (Latin America) <BR/>" +
                            "                  +33-38-855-6929 (Europe) <BR/>" +
                            "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>" +
                            "Thank you for being an Alcatel OmniAccess WLAN Licensing customer.<BR/>" +
                            "Sent by Alcatel OmniAccess Licensing Manager<BR/>" +
                            "http://www.alcatel.com/enterprise   <BR/>";

                        sub = "Alcatel OmniAccess License Activation Info";

                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                        break;
                    }
            }
            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, Bcc);
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAirwaveCertInfo.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objAirwaveCertInfo.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendPreparedAccountCreation", "Mail not sent:" + objAirwaveCertInfo.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAirwaveCertInfo.Email, "", bcc, sub, Message, "", "PREPARED_ACCOUNT_CREATION");
            }

            return retVal;
        }

        public bool sendCaActivationInfo(AccountMailInfo objLgm)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "Your new Aruba License Management account has been created.<BR/><BR/>" +
                                    "To complete the registration on our site, please click on the link below.<BR/>" +
                                     ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAccount.aspx?k=" + objLgm.ActivationCode + "<BR/><BR/>" +
                                    "If you are unable to use the link you will have to enter the following code into the activation page.<BR/>" +
                                    "Code: " + objLgm.ActivationCode + "<BR/>" +
                                    "Page: " + ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAccount.aspx" + "<BR/>" +
                                    "Your login info is below.  You may change your password once you have activated and logged in once." +
                                    "Username: " + objLgm.Email + "<BR/>" +
                                    "Password: " + objLgm.Password + "<BR/>" +
                                   ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "<BR/>" +
                                   "Aruba Networks Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"];

                    sub = "Aruba License Management Website Activation";

                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    Message = "Your new Alcatel OmniAccess License Management account has been created.<BR/><BR/>" +
                                  "To complete the registration on our site, please click on the link below.<BR/>" +
                                   ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAccount.aspx?k=" + objLgm.ActivationCode + "<BR/><BR/>" +
                                "If you are unable to use the link you will have to enter the following code into the activation page.<BR/>" +
                                  "Code: " + objLgm.ActivationCode + "<BR/>" +
                                  "Page: " + ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAccount.aspx <BR/>" +
                                  "Your login info is below.  You may change your password once you have activated and logged in once. <BR/>" +
                                    "Username: " + objLgm.Email + "<BR/>" +
                                    "Password: " + objLgm.Password + "<BR/>" +
                                    ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "<BR/>" +
                                 "Alcatel OmniAccess Licensing Webmaster<BR/>" +
                                 ConfigurationManager.AppSettings["ALCATEL_SITE_URL"];

                    sub = "Alcatel OmniAccess License Management Website Activation";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            }


            if (!retVal)
            {
                new Log().logSystemError("sendCaActivationInfo", "Mail not sent:" + objLgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", "", sub, Message, "", "CA_ACTIVATION_INFO");
            }

            return retVal;

        }

        public bool sendCompanyAdminEmail(AccountMailInfo objLgm)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "Your LMS (Licensing Management Server) account has been upgraded to your company's admin account.  With admin privilege, you are able to manage all of your company's accounts <BR/>";
                    Message = Message + "and also transfer/view certificates that your company has activated. Note that the current system allows only one admin account per company.<BR/>";
                    Message = Message + "If you need to transfer your admin privilege to another account in your company, Please contact Aruba Networks Technical Support.<BR/><BR/>";
                    Message = Message + "Aruba Networks Technical Support is available via phone at 1-800-WiFi-LAN or via email at support@arubanetworks.com.<BR/>";
                    Message = Message + "For further information on how to contact us, including phone numbers for international locations,<BR/>";
                    Message = Message + "Please visit  http://www.arubanetworks.com/support/contact_support.php. <BR/><BR/>";
                    Message = Message + "Aruba Networks Customer Advocacy <BR/>";
                    Message = Message + "1344 Crossman Ave. <BR/>";
                    Message = Message + "Sunnyvale, CA 94089 U.S.A. <BR/>";
                    Message = Message + "1-800-WiFiLAN (800-943-4526) <BR/>";
                    Message = Message + "1-408-754-1201 (International)<BR/>";
                    sub = "Your Aruba Networks LMS Company Admin privilege has been activated";
                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    Message = "Congratulations! " + objLgm.Name + "<BR>";
                    Message = Message + " You are now given company Admin privilege. With this privilege, You should be able to manage your company accounts and also should be able to transfer/view all the certificates owned by your company<BR/>";
                    Message = Message + "Please note that only one user is authorized to be a company admin. If you want to assign this privilege to other user, Please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>";
                    Message = Message + "Should you experience difficulties, please contact ";
                    Message = Message + "Email:      support@ind.alcatel.com<BR/>";
                    Message = Message + "Telephone:  1-800-995-2696  (North America)<BR/>";
                    Message = Message + "                  1-877-919-9526  (Latin America) <BR/>";
                    Message = Message + "                  +33-38-855-6929 (Europe) <BR/>";
                    Message = Message + "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>";
                    sub = "Your Alcatel Networks LMS Company Admin Account has been activated";
                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                    break;
            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            }


            if (!retVal)
            {
                new Log().logSystemError("sendCompanyAdminInfo", "Mail not sent:" + objLgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", "", sub, Message, "", "COMPANY_ADMIN_INFO");
            }

            return retVal;
        }

        public bool sendCompanyAdminDeactivate(AccountMailInfo objLgm)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "Note that your LMS (Licensing Management Server) account's admin privilege has been disabled because the admin privilege has been granted to another account. <BR>";
                    Message = Message + "With admin privilege, you are able to manage all of your company's accounts and also transfer/view certificates that your company has activated.<BR/>";
                    Message = Message + "However, the current system allows only one admin account per company.If you believe this was done in error and need to restore your admin privilege, <BR/>";
                    Message = Message + "Please contact Aruba Networks Technical Support.<BR/><BR/>";
                    Message = Message + "Aruba Networks Technical Support is available via phone at 1-800-WiFi-LAN or via email at support@arubanetworks.com.<BR/>";
                    Message = Message + "For further information on how to contact us, including phone numbers for international locations, visit  http://www.arubanetworks.com/support/contact_support.php. <BR/><BR/>";
                    Message = Message + "Aruba Networks Customer Advocacy <BR/>";
                    Message = Message + "1344 Crossman Ave. <BR/>";
                    Message = Message + "Sunnyvale, CA 94089 U.S.A. <BR/>";
                    Message = Message + "1-800-WiFiLAN (800-943-4526) <BR/>";
                    Message = Message + "1-408-754-1201 (International)<BR/>";
                    sub = "Your Aruba Networks LMS Company Admin privilege has been deactivated";
                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    Message = "Greetings! " + objLgm.Name + "<BR>";
                    Message = Message + " Your company Admin privilege. With this privilege, You will no more be able to manage your company accounts and also will not be able to transfer/view all the certificates owned by your company<BR/>";
                    Message = Message + "Please note that only one user is authorized to be a company admin. If you want to assign this privilege back to your account, Please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>";
                    Message = Message + "Should you experience difficulties, please contact ";
                    Message = Message + "Email:      support@ind.alcatel.com<BR/>";
                    Message = Message + "Telephone:  1-800-995-2696  (North America)<BR/>";
                    Message = Message + "                  1-877-919-9526  (Latin America) <BR/>";
                    Message = Message + "                  +33-38-855-6929 (Europe) <BR/>";
                    Message = Message + "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>";
                    sub = "Alcatel OmniAccess License Management Company Admin privilege info";
                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                    break;
            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            }


            if (!retVal)
            {
                new Log().logSystemError("sendCompanyAdminDeactivate", "Mail not sent:" + objLgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", "", sub, Message, "", "COMPANY_ADMIN_DEACTIVATE");
            }

            return retVal;
        }

        public bool sendResellerActivationInfo(AccountMailInfo objLgm)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "Your new Aruba License Management account has been created.<BR/><BR/>" +
                                    "To complete the registration on our site, please click on the link below.<BR/>" +
                                     ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAccount.aspx?k=" + objLgm.ActivationCode + "<BR/><BR/>" +
                                    "If you are unable to use the link you will have to enter the following code into the activation page.<BR/>" +
                                    "Code: " + objLgm.ActivationCode + "<BR/>" +
                                    "Page: " + ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAccount.aspx" + "<BR/>" +
                                    "Your login info is below.  You may change your password once you have activated and logged in once." +
                                    "Username: " + objLgm.Email + "<BR/>" +
                                    "Password: " + objLgm.Password + "<BR/>" +
                                   ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "<BR/>" +
                                   "Aruba Networks Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"];

                    sub = "Aruba License Management Website Activation";

                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    Message = "Your new Alcatel OmniAccess License Management account has been created.<BR/><BR/>" +
                                  "To complete the registration on our site, please click on the link below.<BR/>" +
                                   ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAccount.aspx?k=" + objLgm.ActivationCode + "<BR/><BR/>" +
                                "If you are unable to use the link you will have to enter the following code into the activation page.<BR/>" +
                                  "Code: " + objLgm.ActivationCode + "<BR/>" +
                                  "Page: " + ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAccount.aspx <BR/>" +
                                  "Your login info is below.  You may change your password once you have activated and logged in once. <BR/>" +
                                    "Username: " + objLgm.Email + "<BR/>" +
                                    "Password: " + objLgm.Password + "<BR/>" +
                                    ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "<BR/>" +
                                 "Alcatel OmniAccess Licensing Webmaster<BR/>" +
                                 ConfigurationManager.AppSettings["ALCATEL_SITE_URL"];

                    sub = "Alcatel OmniAccess License Management Website Activation";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendResellerActivationInfo", "Mail not sent:" + objLgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", "", sub, Message, "", "RESELLER_ACTIVATION_INFO");
            }

            return retVal;

        }

        public bool sendDistributorActivationInfo(AccountMailInfo objLgm)
        {

            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "Your new Aruba License Management account has been created.<BR/><BR/>" +
                                    "To complete the registration on our site, please click on the link below.<BR/>" +
                                     ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAccount.aspx?k=" + objLgm.ActivationCode + "<BR/><BR/>" +
                                    "If you are unable to use the link you will have to enter the following code into the activation page.<BR/>" +
                                    "Code: " + objLgm.ActivationCode + "<BR/>" +
                                    "Page: " + ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAccount.aspx" + "<BR/>" +
                                    "Your login info is below.  You may change your password once you have activated and logged in once." +
                                    "Username: " + objLgm.Email + "<BR/>" +
                                    "Password: " + objLgm.Password + "<BR/>" +
                                   ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "<BR/>" +
                                   "Aruba Networks Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"];

                    sub = "Aruba License Management Website Activation";

                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    Message = "Your new Alcatel OmniAccess License Management account has been created.<BR/><BR/>" +
                                  "To complete the registration on our site, please click on the link below.<BR/>" +
                                   ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAccount.aspx?k=" + objLgm.ActivationCode + "<BR/><BR/>" +
                                "If you are unable to use the link you will have to enter the following code into the activation page.<BR/>" +
                                  "Code: " + objLgm.ActivationCode + "<BR/>" +
                                  "Page: " + ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAccount.aspx <BR/>" +
                                  "Your login info is below.  You may change your password once you have activated and logged in once. <BR/>" +
                                    "Username: " + objLgm.Email + "<BR/>" +
                                    "Password: " + objLgm.Password + "<BR/>" +
                                    ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "<BR/>" +
                                 "Alcatel OmniAccess Licensing Webmaster<BR/>" +
                                 ConfigurationManager.AppSettings["ALCATEL_SITE_URL"];

                    sub = "Alcatel OmniAccess License Management Website Activation";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendDistributorActivationInfo", "Mail not sent:" + objLgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", "", sub, Message, "", "DISTRIBUTOR_ACTIVATION_INFO");
            }

            return retVal;
        }

        public bool sendArubaAccountInfo(AccountMailInfo objLgm)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            if (objLgm.Password == string.Empty)
            {
                objLgm.Password = "Please use Windows Password";
            }
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "Your new Aruba License Management account has been created..<BR/><BR/>" +
                                    "Your login info is as follows:<BR/>" +
                                     "Username: " + objLgm.Email + "<BR/>" +
                                    "Password: " + objLgm.Password + "<BR/>" +
                                   ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "<BR/>" +
                                   "Aruba Networks Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"];

                    sub = "Aruba License Management Website Activation";

                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    Message = "Your new Alcatel OmniAccess License Management account has been created.<BR/><BR/>" +
                                    "Your login info is as follows:<BR/>" +
                                    "Username: " + objLgm.Email + "<BR/>" +
                                    "Password: " + objLgm.Password + "<BR/>" +
                                    ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "<BR/>" +
                                 "Alcatel OmniAccess Licensing Webmaster<BR/>" +
                                 ConfigurationManager.AppSettings["ALCATEL_SITE_URL"];

                    sub = "Alcatel OmniAccess License Management Website Activation";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendArubaAccountInfo", "Mail not sent:" + objLgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", "", sub, Message, "", "ARUBA_ACCOUNT_INFO");
            }

            return retVal;

        }

        public bool sendEvalCertInfo(CertificateMailInfo objCgm)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objCgm.Brand)
            {
                case "ARUBA":
                    Message = " Thank you for your request " + objCgm.Name + " (" + objCgm.Email + ")" + "<BR/><BR/>" +
                                    " This email is your electronic confirmation of your request for an Aruba Networks Evaluation Software License as follows:<BR/>" +
                                    "Aruba Part Number: " + objCgm.CertPartId + "<BR/>" +
                                    "Description            : " + objCgm.CertPartDesc + "<BR/>" +
                                    "CERTIFICATE ID    : " + objCgm.CertId + "<BR/><BR/>" +
                                    "This electronic confirmation may be used to activate your Software License Certificate and create an Evaluation License Key that can be applied to your Aruba Networks Mobility Controller platform. This key can be used to enable the required functionality for 90 days (in 30 day increments) for trial purposes.<BR/><BR/>" +
                                    "To activate your License Certificate please have your Mobility Controller (or Supervisor Card in the case of Aruba 5000/6000) serial number available and visit the Aruba License Management Website at:<BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "<BR/><BR/>" +
                                    "If you are a first time user, the Software License Certificate ID number may be used to create a user account at the License Management Website.<BR/><BR/>" +
                                    "This electronic copy of the transaction serves as your confirmation. <BR/>" +
                                    "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR/>" +
                                    "Email:      support@arubanetworks.com <BR/>" +
                                    "Telephone:  1-800-943-4526 (USA) <BR/>" +
                                     "                 1-408-754-1201 (International) <BR/><BR/>" +
                                     "Thank you for being an Aruba Networks customer.<BR/>" +
                                     "Sent by Aruba Networks License Manager<BR/>" +
                                     "http://www.arubanetworks.com  <BR/>";

                    sub = "Confirmation - Eval Software License Certificate";

                    //bcc = "dl-support@arubanetworks.com";

                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    Message = "Thank you for your request " + objCgm.Name + " (" + objCgm.Email + ")" + "<BR/><BR/>" +
                                    "This email is your electronic confirmation of your request for an Alcatel OmniAccess WLAN Evaluation Software License as follows:<BR/>" +
                                    "Alcatel Part Number: " + objCgm.CertPartId + "<BR/>" +
                                    "Description            : " + objCgm.CertPartDesc + "<BR/>" +
                                    "CERTIFICATE ID    :" + objCgm.CertId + "<BR/><BR/>" +
                                    "This electronic confirmation may be used to activate your Software License Certificate and create an Evaluation License Key that can be applied to your Alcatel OmniAccess WLAN switch platform. This key can be used to enable the required functionality for 90 days (in 30 day increments) for trial purposes.<BR/><BR/>" +
                                    "To activate your License Certificate please have your OmniAccess WLAN switch (or Supervisor Card in the case of OmniAccess 6000) serial number available and visit the Alcatel OmniAccess License Management Website at:<BR/>" +
                                    ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "<BR/><BR/>" +
                                    "If you are a first time user, the Software License Certificate ID number may be used to create a user account at the License Management Website.<BR/><BR/>" +
                                    "This electronic copy of the transaction serves as your confirmation. <BR/>" +
                                    "Should you experience difficulties, please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>" +
                                    "Email:      support@ind.alcatel.com<BR/>" +
                                    "Telephone:  1-800-995-2696  (North America)<BR/>" +
                                    "                  1-877-919-9526  (Latin America) <BR/>" +
                                    "                  +33-38-855-6929 (Europe) <BR/>" +
                                    "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>" +
                                    "Thank you for being an Alcatel OmniAccess WLAN Licensing customer.<BR/>" +
                                    "Sent by Alcatel OmniAccess Licensing Manager<BR/>" +
                                    "http://www.alcatel.com/enterprise   <BR/>";

                    sub = "Confirmation - Eval Software License Certificate";

                    //bcc="support@ind.alctel.com";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objCgm.Email, "", sub, Message, bcc);

            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objCgm.Email, objCgm.CCEmail, sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objCgm.Email, objCgm.CCEmail, sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendEvalCertInfo", "Mail not sent:" + objCgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objCgm.Email, "", bcc, sub, Message, "", "EVAL_CERT_INFO");
            }
            return retVal;
        }

        public bool sendAssignedCertInfo(CertificateMailInfo objCgm)
        {

            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objCgm.Brand)
            {
                case "ARUBA":
                    Message = " Thank you for your request " + objCgm.Name + " (" + objCgm.Email + ")" + "<BR/><BR/>" +
                                    " This email is your electronic confirmation of your request for an Aruba Networks Evaluation Software License as follows:<BR/>" +
                                    "Aruba Part Number: " + objCgm.CertPartId + "<BR/>" +
                                    "Description            : " + objCgm.CertPartDesc + "<BR/>" +
                                    "CERTIFICATE ID    : " + objCgm.CertId + "<BR/><BR/>" +
                                    "This electronic confirmation may be used to activate your Software License Certificate and create an Evaluation License Key that can be applied to your Aruba Networks Mobility Controller platform. This key can be used to enable the required functionality for 90 days (in 30 day increments) for trial purposes.<BR/><BR/>" +
                                    "To activate your License Certificate please have your Mobility Controller (or Supervisor Card in the case of Aruba 5000/6000) serial number available and visit the Aruba License Management Website at:<BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "<BR/><BR/>" +
                                    "If you are a first time user, the Software License Certificate ID number may be used to create a user account at the License Management Website.<BR/><BR/>" +
                                    "This electronic copy of the transaction serves as your confirmation. <BR/>" +
                                    "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR/>" +
                                    "Email:      support@arubanetworks.com <BR/>" +
                                    "Telephone:  1-800-943-4526 (USA) <BR/>" +
                                     "                 1-408-754-1201 (International) <BR/><BR/>" +
                                     "Thank you for being an Aruba Networks customer.<BR/>" +
                                     "Sent by Aruba Networks License Manager<BR/>" +
                                     "http://www.arubanetworks.com  <BR/>";

                    sub = "Confirmation - Eval Software License Certificate";

                    //bcc = "dl-support@arubanetworks.com";

                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    Message = "Thank you for your request " + objCgm.Name + " (" + objCgm.Email + ")" + "<BR/><BR/>" +
                                    "This email is your electronic confirmation of your request for an Alcatel OmniAccess WLAN Evaluation Software License as follows:<BR/>" +
                                    "Alcatel Part Number: " + objCgm.CertPartId + "<BR/>" +
                                    "Description            : " + objCgm.CertPartDesc + "<BR/>" +
                                    "CERTIFICATE ID    :" + objCgm.CertId + "<BR/><BR/>" +
                                    "This electronic confirmation may be used to activate your Software License Certificate and create an Evaluation License Key that can be applied to your Alcatel OmniAccess WLAN switch platform. This key can be used to enable the required functionality for 90 days (in 30 day increments) for trial purposes.<BR/><BR/>" +
                                    "To activate your License Certificate please have your OmniAccess WLAN switch (or Supervisor Card in the case of OmniAccess 6000) serial number available and visit the Alcatel OmniAccess License Management Website at:<BR/>" +
                                    ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "<BR/><BR/>" +
                                    "If you are a first time user, the Software License Certificate ID number may be used to create a user account at the License Management Website.<BR/><BR/>" +
                                    "This electronic copy of the transaction serves as your confirmation. <BR/>" +
                                    "Should you experience difficulties, please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>" +
                                    "Email:      support@ind.alcatel.com<BR/>" +
                                    "Telephone:  1-800-995-2696  (North America)<BR/>" +
                                    "                  1-877-919-9526  (Latin America) <BR/>" +
                                    "                  +33-38-855-6929 (Europe) <BR/>" +
                                    "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>" +
                                    "Thank you for being an Alcatel OmniAccess WLAN Licensing customer.<BR/>" +
                                    "Sent by Alcatel OmniAccess Licensing Manager<BR/>" +
                                    "http://www.alcatel.com/enterprise   <BR/>";

                    sub = "Confirmation - Eval Software License Certificate";

                    //bcc="support@ind.alctel.com";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objCgm.Email, "", sub, Message, bcc);

            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objCgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objCgm.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendEvalCertInfo", "Mail not sent:" + objCgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objCgm.Email, "", bcc, sub, Message, "", "EVAL_CERT_INFO");
            }
            return retVal;
        }

        public bool sendArubaCertInfo(CertificateMailInfo objCertMailInfo)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            if (objCertMailInfo.Brand.ToUpper() == ConfigurationManager.AppSettings["ARUBA_BRAND"].ToString().ToUpper())
            {
                Message = " Thank you for your transaction " + objCertMailInfo.Name + "(" + objCertMailInfo.Email + ")," + "<BR/><BR/>" +
                                  " You are successfully Converted your Alcatel Part to Aruba.This Email is your electronic confirmation of your Transaction at " + ConfigurationManager.AppSettings["ALCATEL_SITE_URL"].ToString() +
                "Aruba Part details are as follows:<BR/>" +
                                  "Aruba Part Number : " + objCertMailInfo.ArubaPartId + "<BR/>" +
                                  "Description             : " + objCertMailInfo.ArubaPartDesc + "<BR/>" +
                                  "Serial Number : " + objCertMailInfo.ArubaSerialNumber + "<BR/>" +

                                  "This Certificate can be used for the registration of your account on the Aruba site.<BR>" +
                                  ConfigurationManager.AppSettings["ARUBA_SITE_URL"].ToString() +
                                  "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR/>" +
                                  "Email:      support@arubanetworks.com <BR/>" +
                                  "Telephone:  1-800-943-4526 (USA) <BR/>" +
                                   "                 1-408-754-1201 (International) <BR/><BR/>" +
                                   "Thank you for being an Aruba Networks customer.<BR/>" +
                                   "Sent by Aruba Networks License Manager<BR/>" +
                                   "http://www.arubanetworks.com  <BR/>";
                sub = "Alcatel to Aruba Conversion Info";

                fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objCgm.Email, "", sub, Message, bcc);
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objCertMailInfo.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objCertMailInfo.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendCertificateActivationInfo", "Mail not sent:" + objCertMailInfo.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objCertMailInfo.Email, "", bcc, sub, Message, "", "CERTIFICATE_ACTIVATION_INFO");
            }
            return retVal;
        }


        public bool sendCertificateActivationInfo(CertificateMailInfo objCgm)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            string SYSDESC = "System Serial          : ";
            if (objReg.IsMatch(objCgm.SysSerialNumber))
            {
                SYSDESC = "System MAC Address     : ";
            }


            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            string CertType = "AOS";
            Certificate objCert = new Certificate();
            string partType = objCert.getPartType(objCgm.CertPartId, string.Empty);
            if (partType.Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString()))
                CertType = "RFP";
            else if (partType.Contains(ConfigurationManager.AppSettings["ECS_TYPE"].ToString()))
                CertType = "ECS";
            else
                CertType = "AOS";

            switch (objCgm.Brand)
            {
                case "ARUBA":
                    switch (CertType)
                    {
                        case "AOS":
                            Message = " Thank you for your transaction " + objCgm.Name + "(" + objCgm.Email + ")," + "<BR/><BR/>" +
                                  " This email is your electronic confirmation of your Aruba Networks Software License Key generation as follows:<BR/>" +
                                  "Aruba Part Number : " + objCgm.CertPartId + "<BR/>" +
                                  "Description             : " + objCgm.CertPartDesc + "<BR/>" +
                                  SYSDESC + objCgm.SysSerialNumber + "<BR/>" +
                                  "System Description : " + objCgm.SysPartDesc + "<BR/>" +
                                  "License Key            : " + objCgm.ActivationKey + "<BR/><BR/>" +
                                  "This License key can be used only on the Aruba Networks Mobility Controller with the above stated system serial number to enable the required functionality.<BR/><BR/>" +
                                  "To enable the software feature, please login (with Administrator access rights) to your Mobility Controller, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                                  "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR/>" +
                                  "Email:      support@arubanetworks.com <BR/>" +
                                  "Telephone:  1-800-943-4526 (USA) <BR/>" +
                                   "                 1-408-754-1201 (International) <BR/><BR/>" +
                                   "Thank you for being an Aruba Networks customer.<BR/>" +
                                   "Sent by Aruba Networks License Manager<BR/>" +
                                   "http://www.arubanetworks.com  <BR/>";
                            break;
                        case "RFP":
                            Message = " Thank you for your transaction " + objCgm.Name + "(" + objCgm.Email + ")" + ",<BR/><BR/>" +
                                  " This email is your electronic confirmation of your Aruba Networks Software License Key generation as follows:<BR/>" +
                                  "Aruba Part Number : " + objCgm.CertPartId + "<BR/>" +
                                  "Description             : " + objCgm.CertPartDesc + "<BR/>" +
                                  SYSDESC + objCgm.SysSerialNumber + "<BR/>" +
                                  "System Description : " + objCgm.SysPartDesc + "<BR/>" +
                                  "License Key            : " + objCgm.ActivationKey + "<BR/><BR/>" +
                                  "This License key can be used only on the Aruba Networks Mobility Controller with the above stated system serial number to enable the required functionality.<BR/><BR/>" +
                                  "To enable the software feature, please login (with Administrator access rights) to your Mobility Controller, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                                  "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR/>" +
                                  "Email:      support@arubanetworks.com <BR/>" +
                                  "Telephone:  1-800-943-4526 (USA) <BR/>" +
                                   "                 1-408-754-1201 (International) <BR/><BR/>" +
                                   "Thank you for being an Aruba Networks customer.<BR/>" +
                                   "Sent by Aruba Networks License Manager<BR/>" +
                                   "http://www.arubanetworks.com  <BR/>";
                            break;
                        case "ECS":
                            Message = " Thank you for your transaction " + objCgm.Name + "(" + objCgm.Email + ")" + ",<BR/><BR/>" +
                                   " This email is your electronic confirmation of your Aruba Networks Software License Key generation as follows:<BR/>" +
                                   "Aruba Part Number : " + objCgm.CertPartId + "<BR/>" +
                                   "Description             : " + objCgm.CertPartDesc + "<BR/>" +
                                   SYSDESC + objCgm.SysSerialNumber + "<BR/>" +
                                   "System Description : " + objCgm.SysPartDesc + "<BR/>" +
                                   "License Key            : " + objCgm.ActivationKey + "<BR/><BR/>" +
                                   "This License key can be used only on the Aruba Networks Mobility Controller with the above stated system serial number to enable the required functionality.<BR/><BR/>" +
                                   "To enable the software feature, please login (with Administrator access rights) to your Mobility Controller, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                                   "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR/>" +
                                   "Email:      support@arubanetworks.com <BR/>" +
                                   "Telephone:  1-800-943-4526 (USA) <BR/>" +
                                    "                 1-408-754-1201 (International) <BR/><BR/>" +
                                    "Thank you for being an Aruba Networks customer.<BR/>" +
                                    "Sent by Aruba Networks License Manager<BR/>" +
                                    "http://www.arubanetworks.com  <BR/>";
                            break;
                    }

                    sub = "Aruba License Activation Info";

                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    switch (CertType)
                    {
                        case "AOS":
                            Message = " Thank you for your transaction " + objCgm.Name + "(" + objCgm.Email + ")" + ",<BR/><BR/>" +
                                  " This email is your electronic confirmation of your Alcatel OmniAccess Software License Key generation as follows:<BR/>" +
                                  "Alcatel Part Number : " + objCgm.CertPartId + "<BR/>" +
                                  "Description             : " + objCgm.CertPartDesc + "<BR/>" +
                                  SYSDESC + objCgm.SysSerialNumber + "<BR/>" +
                                  "System Description : " + objCgm.SysPartDesc + "<BR/>" +
                                  "License Key            : " + objCgm.ActivationKey + "<BR/><BR/>" +
                                  "This License key can be used only on the Alcatel OmniAccess WLAN switch with the above stated system serial number to enable the required functionality.<BR/><BR/>" +
                                  "To enable the software feature, please login (with Administrator access rights) to your Mobility Controller, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                                    "Should you experience difficulties, please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>" +
                                   "Email:      support@ind.alcatel.com<BR/>" +
                                    "Telephone:  1-800-995-2696  (North America)<BR/>" +
                                    "                  1-877-919-9526  (Latin America) <BR/>" +
                                    "                  +33-38-855-6929 (Europe) <BR/>" +
                                    "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>" +
                                    "Thank you for being an Alcatel OmniAccess WLAN Licensing customer.<BR/>" +
                                    "Sent by Alcatel OmniAccess Licensing Manager<BR/>" +
                                    "http://www.alcatel.com/enterprise   <BR/>";

                            break;
                        case "RFP":
                            Message = " Thank you for your transaction " + objCgm.Name + "(" + objCgm.Email + ")" + ",<BR/><BR/>" +
                                 " This email is your electronic confirmation of your Alcatel OmniAccess Software License Key generation as follows:<BR/>" +
                                 "Alcatel Part Number : " + objCgm.CertPartId + "<BR/>" +
                                 "Description             : " + objCgm.CertPartDesc + "<BR/>" +
                                 SYSDESC + objCgm.SysSerialNumber + "<BR/>" +
                                 "System Description : " + objCgm.SysPartDesc + "<BR/>" +
                                 "License Key            : " + objCgm.ActivationKey + "<BR/><BR/>" +
                                 "This License key can be used only on the Alcatel OmniAccess WLAN switch with the above stated system serial number to enable the required functionality.<BR/><BR/>" +
                                 "To enable the software feature, please login (with Administrator access rights) to your Mobility Controller, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                                   "Should you experience difficulties, please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>" +
                                  "Email:      support@ind.alcatel.com<BR/>" +
                                   "Telephone:  1-800-995-2696  (North America)<BR/>" +
                                   "                  1-877-919-9526  (Latin America) <BR/>" +
                                   "                  +33-38-855-6929 (Europe) <BR/>" +
                                   "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>" +
                                   "Thank you for being an Alcatel OmniAccess WLAN Licensing customer.<BR/>" +
                                   "Sent by Alcatel OmniAccess Licensing Manager<BR/>" +
                                   "http://www.alcatel.com/enterprise   <BR/>";

                            break;
                        case "ECS":
                            Message = " Thank you for your transaction " + objCgm.Name + "(" + objCgm.Email + ")" + ",<BR/><BR/>" +
                                  " This email is your electronic confirmation of your Alcatel OmniAccess Software License Key generation as follows:<BR/>" +
                                  "Alcatel Part Number : " + objCgm.CertPartId + "<BR/>" +
                                  "Description             : " + objCgm.CertPartDesc + "<BR/>" +
                                  SYSDESC + objCgm.SysSerialNumber + "<BR/>" +
                                  "System Description : " + objCgm.SysPartDesc + "<BR/>" +
                                  "License Key            : " + objCgm.ActivationKey + "<BR/><BR/>" +
                                  "This License key can be used only on the Alcatel OmniAccess WLAN switch with the above stated system serial number to enable the required functionality.<BR/><BR/>" +
                                  "To enable the software feature, please login (with Administrator access rights) to your Mobility Controller, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                                    "Should you experience difficulties, please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>" +
                                   "Email:      support@ind.alcatel.com<BR/>" +
                                    "Telephone:  1-800-995-2696  (North America)<BR/>" +
                                    "                  1-877-919-9526  (Latin America) <BR/>" +
                                    "                  +33-38-855-6929 (Europe) <BR/>" +
                                    "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>" +
                                    "Thank you for being an Alcatel OmniAccess WLAN Licensing customer.<BR/>" +
                                    "Sent by Alcatel OmniAccess Licensing Manager<BR/>" +
                                    "http://www.alcatel.com/enterprise   <BR/>";

                            break;
                    }

                    sub = "Alcatel OmniAccess License Activation Info";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objCgm.Email, "", sub, Message, bcc);
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objCgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objCgm.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendCertificateActivationInfo", "Mail not sent:" + objCgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objCgm.Email, "", bcc, sub, Message, "", "CERTIFICATE_ACTIVATION_INFO");
            }
            return retVal;

        }

        public bool sendQALicenseKey(CertificateMailInfo objCgm)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            string SYSDESC = "System Serial          : ";
            string strPassphrase = "Passphrase  : ";
            if (objReg.IsMatch(objCgm.SysSerialNumber))
            {
                SYSDESC = "System MAC Address     : ";
            }

            if (objCgm.Passphrase != string.Empty)
            {
                strPassphrase = strPassphrase + objCgm.Passphrase;
            }
            else
            {
                strPassphrase = string.Empty;
            }

            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            string CertType = "AOS";
            Certificate objCert = new Certificate();
            string partType = objCert.getPartType(objCgm.CertPartId, string.Empty);
            string strLicenseKey = objCgm.ActivationKey;
            string ccUser = ConfigurationManager.AppSettings["CC_MAIL"].ToString();
            if (partType.Contains(ConfigurationManager.AppSettings["RFP_TYPE"].ToString()))
                CertType = "RFP";
            else if (partType.Contains(ConfigurationManager.AppSettings["ECS_TYPE"].ToString()))
                CertType = "ECS";
            else
                CertType = "AOS";

            switch (objCgm.Brand)
            {
                case "ARUBA":
                    switch (CertType)
                    {
                        case "AOS":
                            Message = " Thank you for your transaction " + objCgm.Name + "(" + objCgm.Email + ")," + "<BR/><BR/>" +
                                  " Please find the attached document of license keys generated for the controller " + SYSDESC + objCgm.SysSerialNumber + "<BR/>" +
                                  "System Description : " + objCgm.SysPartDesc + "<BR/><BR/>" +
                                   strPassphrase + "<BR/><BR/>" +
                                  " Following are the certificate Part Number and its license key <BR/>" +
                                  objCgm.CertPartDesc +
                                  "This License key can be used only on the Aruba Networks Mobility Controller with the above stated system serial number to enable the required functionality.<BR/><BR/>" +
                                  "To enable the software feature, please login (with Administrator access rights) to your Mobility Controller, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                                  "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR/>" +
                                  "Email:      support@arubanetworks.com <BR/>" +
                                  "Telephone:  1-800-943-4526 (USA) <BR/>" +
                                   "                 1-408-754-1201 (International) <BR/><BR/>" +
                                   "Thank you for being an Aruba Networks customer.<BR/>" +
                                   "Sent by Aruba Networks License Manager<BR/>" +
                                   "http://www.arubanetworks.com  <BR/>";
                            break;
                        case "RFP":
                            Message = " Thank you for your transaction " + objCgm.Name + "(" + objCgm.Email + ")" + ",<BR/><BR/>" +
                                  " Please find the attached document of license keys generated for the controller " + SYSDESC + objCgm.SysSerialNumber + "<BR/>" +
                                  "System Description : " + objCgm.SysPartDesc + "<BR/>" +
                                  "This License key can be used only on the Aruba Networks Mobility Controller with the above stated system serial number to enable the required functionality.<BR/><BR/>" +
                                  "To enable the software feature, please login (with Administrator access rights) to your Mobility Controller, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                                  "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR/>" +
                                  "Email:      support@arubanetworks.com <BR/>" +
                                  "Telephone:  1-800-943-4526 (USA) <BR/>" +
                                   "                 1-408-754-1201 (International) <BR/><BR/>" +
                                   "Thank you for being an Aruba Networks customer.<BR/>" +
                                   "Sent by Aruba Networks License Manager<BR/>" +
                                   "http://www.arubanetworks.com  <BR/>";
                            break;
                        case "ECS":
                            Message = " Thank you for your transaction " + objCgm.Name + "(" + objCgm.Email + ")" + ",<BR/><BR/>" +
                                   " Please find the attached document of license keys generated for the controller " + SYSDESC + objCgm.SysSerialNumber + "<BR/>" +
                                  "System Description : " + objCgm.SysPartDesc + "<BR/>" +
                                   "This License key can be used only on the Aruba Networks Mobility Controller with the above stated system serial number to enable the required functionality.<BR/><BR/>" +
                                   "To enable the software feature, please login (with Administrator access rights) to your Mobility Controller, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                                   "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR/>" +
                                   "Email:      support@arubanetworks.com <BR/>" +
                                   "Telephone:  1-800-943-4526 (USA) <BR/>" +
                                    "                 1-408-754-1201 (International) <BR/><BR/>" +
                                    "Thank you for being an Aruba Networks customer.<BR/>" +
                                    "Sent by Aruba Networks License Manager<BR/>" +
                                    "http://www.arubanetworks.com  <BR/>";
                            break;
                    }

                    sub = "Aruba License Activation Info for " + objCgm.SysSerialNumber;

                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                    break;
                case "ALCATEL":
                    switch (CertType)
                    {
                        case "AOS":
                            Message = " Thank you for your transaction " + objCgm.Name + "(" + objCgm.Email + ")" + ",<BR/><BR/>" +
                                  " Please find the attached document of license keys generated for the controller " + SYSDESC + objCgm.SysSerialNumber + "<BR/>" +
                                  "System Description : " + objCgm.SysPartDesc + "<BR/>" +
                                  "This License key can be used only on the Alcatel OmniAccess WLAN switch with the above stated system serial number to enable the required functionality.<BR/><BR/>" +
                                  "To enable the software feature, please login (with Administrator access rights) to your Mobility Controller, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                                    "Should you experience difficulties, please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>" +
                                   "Email:      support@ind.alcatel.com<BR/>" +
                                    "Telephone:  1-800-995-2696  (North America)<BR/>" +
                                    "                  1-877-919-9526  (Latin America) <BR/>" +
                                    "                  +33-38-855-6929 (Europe) <BR/>" +
                                    "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>" +
                                    "Thank you for being an Alcatel OmniAccess WLAN Licensing customer.<BR/>" +
                                    "Sent by Alcatel OmniAccess Licensing Manager<BR/>" +
                                    "http://www.alcatel.com/enterprise   <BR/>";

                            break;
                        case "RFP":
                            Message = " Thank you for your transaction " + objCgm.Name + "(" + objCgm.Email + ")" + ",<BR/><BR/>" +
                                 " Please find the attached document of license keys generated for the controller " + SYSDESC + objCgm.SysSerialNumber + "<BR/>" +
                                  "System Description : " + objCgm.SysPartDesc + "<BR/>" +
                                 "This License key can be used only on the Alcatel OmniAccess WLAN switch with the above stated system serial number to enable the required functionality.<BR/><BR/>" +
                                 "To enable the software feature, please login (with Administrator access rights) to your Mobility Controller, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                                   "Should you experience difficulties, please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>" +
                                  "Email:      support@ind.alcatel.com<BR/>" +
                                   "Telephone:  1-800-995-2696  (North America)<BR/>" +
                                   "                  1-877-919-9526  (Latin America) <BR/>" +
                                   "                  +33-38-855-6929 (Europe) <BR/>" +
                                   "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>" +
                                   "Thank you for being an Alcatel OmniAccess WLAN Licensing customer.<BR/>" +
                                   "Sent by Alcatel OmniAccess Licensing Manager<BR/>" +
                                   "http://www.alcatel.com/enterprise   <BR/>";

                            break;
                        case "ECS":
                            Message = " Thank you for your transaction " + objCgm.Name + "(" + objCgm.Email + ")" + ",<BR/><BR/>" +
                                 " Please find the attached document of license keys generated for the controller " + SYSDESC + objCgm.SysSerialNumber + "<BR/>" +
                                  "System Description : " + objCgm.SysPartDesc + "<BR/>" +
                                  "This License key can be used only on the Alcatel OmniAccess WLAN switch with the above stated system serial number to enable the required functionality.<BR/><BR/>" +
                                  "To enable the software feature, please login (with Administrator access rights) to your Mobility Controller, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                                    "Should you experience difficulties, please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>" +
                                   "Email:      support@ind.alcatel.com<BR/>" +
                                    "Telephone:  1-800-995-2696  (North America)<BR/>" +
                                    "                  1-877-919-9526  (Latin America) <BR/>" +
                                    "                  +33-38-855-6929 (Europe) <BR/>" +
                                    "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>" +
                                    "Thank you for being an Alcatel OmniAccess WLAN Licensing customer.<BR/>" +
                                    "Sent by Alcatel OmniAccess Licensing Manager<BR/>" +
                                    "http://www.alcatel.com/enterprise   <BR/>";

                            break;
                    }

                    sub = "Alcatel OmniAccess License Activation Info for " + objCgm.SysSerialNumber;

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini

            retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objCgm.Email, ccUser, sub, Message, strLicenseKey);

            if (!retVal)
            {
                new Log().logSystemError("sendCertificateActivationInfo", "Mail not sent:" + objCgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objCgm.Email, ccUser, bcc, sub, Message, "", "CERTIFICATE_ACTIVATION_INFO");
            }
            return retVal;
        }

        public bool sendQAGenrateFailure(string strError, string strToAddress)
        {
            string sub = "";
            string[] fromInfo = { "", "" };
            sub = "Aruba QA License Generation Failure";
            fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
            retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], strToAddress, "", sub, strError, "");

            if (!retVal)
            {
                new Log().logSystemError("sendQAGenrateFailure", "Mail not sent:" + strToAddress);
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], strToAddress, "", bcc, sub, strError, "", "CERTIFICATE_ACTIVATION_INFO");
            }
            return retVal;

        }

        public bool UpdateFailureInfo(string strSerialNo, string strToAddress)
        {
            string sub = "";
            string[] fromInfo = { "", "" };
            sub = "Updation to Upgrade Controller " + strSerialNo + " Failed";
            fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
            retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], strToAddress, "", sub, strSerialNo, "");

            if (!retVal)
            {
                new Log().logSystemError("UpdateFailureInfo", "Mail not sent:" + strToAddress);
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], strToAddress, "", bcc, sub, strSerialNo, "", "CERTIFICATE_ACTIVATION_INFO");
            }
            return retVal;
        }

        public bool sendChangePasswordNotification(AccountMailInfo objLgm)
        {

            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "You or someone pretending to be you has changed the password to your Aruba Wireless Networks Licensing web site account. If you did not make this request, immediately contact Aruba Wireless Networks at licensing@arubanetworks.com.<BR/>" +
                                   "If you did do this yourself no further action is required.<BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "<BR/>" +
                                    "Aruba Networks Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"];
                    sub = "Aruba License Management Website password change";
                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');

                    break;
                case "ALCATEL":
                    Message = "You or someone pretending to be you has changed the password to your Alcatel OmniAccess Licensing web site account. If you did not make this request, immediately contact Alcatel OmniAccess customer support at support@ind.alcatel.com.<BR/>" +
                                    "If you did do this yourself no further action is required.<BR/>" +
                                    ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "<BR/>" +
                                   "Alcatel Internetworking Licensing Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ALCATEL_SITE_URL"];

                    sub = "Your Alcatel OmniAccess License Management Website password change";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, Bcc);
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendChangePasswordNotification", "Mail not sent:" + objLgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", bcc, sub, Message, "", "PASSWORD_CHANGE_NOTIFICATION");
            }

            return retVal;
        }

        public bool sendCompanyAssignedInfo(AccountMailInfo objLgm)
        {

            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "Your account " + objLgm.Email + " is assigned to company " + objLgm.CompanyName + "<BR/>" +
                                   "If you requested this change, no further action is required.<BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "<BR/>" +
                                    "Aruba Networks Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"];
                    sub = "Your Aruba license management account assigned company info";
                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');

                    break;
                case "ALCATEL":
                    Message = "Your account " + objLgm.Email + " is assigned to company " + objLgm.CompanyName + "<BR/>" +
                                   "If you requested this change, no further action is required.<BR/>" +
                                    ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "<BR/>" +
                                   "Alcatel Internetworking Licensing Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ALCATEL_SITE_URL"];

                    sub = "Your Alcatel OmniAccess License Management account assigned company info";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, Bcc);
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendCompanyAssignedInfo", "Mail not sent:" + objLgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", bcc, sub, Message, "", "COMPANY_ASSIGEND_NOTIFICATION");
            }

            return retVal;
        }

        public bool sendPreparedAccountInfo(AccountMailInfo objLgm)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "A new Aruba Networks License Management account has been prepared for you.<BR/><BR/>" +
                                   "To complete the process, please click on the link below.<BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAccount.aspx?k=" + objLgm.ActivationCode + " <BR/>" +
                                    "If you are unable to use the link you will have to enter the following code into the prepared account page." +
                                    "Code: " + objLgm.ActivationCode + "<BR/>" +
                                    "Page: " + ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "/Accounts/ActivateAccount.aspx" + "<BR/>" +
                                    "Aruba Networks Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"];
                    sub = "Aruba License Management Website prepared account waiting for you";
                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');

                    break;
                case "ALCATEL":
                    Message = "A new Alcatel OmniAccess License Management account has been prepared for you.<BR/><BR/>" +
                                 "To complete the process, please click on the link below.<BR/>" +
                                  ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAccount.aspx?k=" + objLgm.ActivationCode + " <BR/>" +
                                  "If you are unable to use the link you will have to enter the following code into the prepared account page." +
                                  "Code: " + objLgm.ActivationCode + "<BR/>" +
                                  "Page: " + ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "/Accounts/ActivateAccount.aspx" + "<BR/>" +
                                  "Alcatel Internetworking Licensing Webmaster <BR/>" +
                                  ConfigurationManager.AppSettings["ALCATEL_SITE_URL"];

                    sub = "Alcatel OmniAccess License Management Website prepared account waiting for you";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, Bcc);
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendPreparedAccountInfo", "Mail not sent:" + objLgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", bcc, sub, Message, "", "PREPARED_ACCOUNT_INFO");
            }

            return retVal;
        }

        public bool SendCertsMail(DataTable dtMail, string strTo, string strBrand)
        {
            bool retVal = false;
            bool blHasCerts;
            string Message = string.Empty;
            string sub = string.Empty;
            string[] fromInfo = { "", "" };
            DataTable dtHeader = new DataTable();
            string strAirwaveRec = string.Empty;
            string strAmgRec = string.Empty;
            bool blHasAirwave = false;
            bool blHasAmg = false;
            string strALERec = string.Empty;
            bool blHasALE = false;
            bool blResult = false;
            dtHeader = dtMail.DefaultView.ToTable(true, "so_id");

            for (int i = 0; i < dtHeader.Rows.Count; i++)
            {
                blHasCerts = false;
                Message = string.Empty;
                DataTable dtDet = dtMail.Clone();
                DataRow[] drResult = dtMail.Select("so_id = " + dtHeader.Rows[i]["SO_ID"].ToString());
                foreach (DataRow dr in drResult)
                    dtDet.ImportRow(dr);

                // Message = "Thank you for your order " + dtDet.Rows[0]["license_contact"].ToString() + "<BR>";

                if (strBrand == ConfigurationManager.AppSettings["ARUBA_BRAND"].ToString())
                {
                    Message = " This is the electronic delivery of your requested Aruba Software License(s).<BR>";
                    Message = Message + " The below CERTIFICATE ID(s) are used to create a License Key which is applied to your Aruba Networks Mobility Controller to enable the purchased software license functionality.<BR><BR>";
                    Message = Message + "<span style=\"font-family:Arial;\">";
                    for (int j = 0; j < dtDet.Rows.Count; j++)
                    {
                        if (dtDet.Rows[j]["certType"].ToString() == "CERT")
                        {
                            if (dtDet.Rows[j]["PART_ID"].ToString().Contains("AWMS") || dtDet.Rows[j]["PART_ID"].ToString().Contains("AW"))
                            {
                                blHasAirwave = true;
                                strAirwaveRec = dtDet.Rows[j]["pk_cert_id"].ToString() + "," + strAirwaveRec;
                                continue;
                            }

                            if (dtDet.Rows[j]["PART_ID"].ToString().Contains("AMG") || dtDet.Rows[j]["PART_ID"].ToString().Contains("LIC-CP-OB") || dtDet.Rows[j]["PART_ID"].ToString().StartsWith("CP"))
                            {
                                blHasAmg = true;
                                strAmgRec = dtDet.Rows[j]["pk_cert_id"].ToString() + "," + strAmgRec;
                                continue;
                            }

                            if (dtDet.Rows[j]["PART_ID"].ToString().Contains("ALE"))
                            {
                                blHasALE = true;
                                strALERec = dtDet.Rows[j]["pk_cert_id"].ToString() + "," + strALERec;
                                continue;
                            }

                            Message = Message + "--------------------------------------------------------------------------------<BR><BR>";
                            Message = Message + "<TABLE width=100%>";
                            if (dtDet.Rows[j]["CUST_PO_ID"].ToString() != string.Empty)
                            {
                                Message = Message + "<TR><TD>Customer PO:</TD> <TD>" + dtDet.Rows[j]["CUST_PO_ID"].ToString() + "</TD></TR>";
                            }
                            if (dtDet.Rows[j]["END_USER_PO"].ToString() != string.Empty)
                            {
                                Message = Message + "<TR><TD>Reseller/End User PO:</TD> <TD>" + dtDet.Rows[j]["END_USER_PO"].ToString() + "</TD></TR>";
                            }
                            Message = Message + "<TR><TD>Aruba Sales Order:</TD> <TD>" + dtDet.Rows[j]["SO_ID"].ToString() +
                                    "</TD></TR>";
                            Message = Message + "<TR><TD>Aruba Part Number:</TD> <TD>" + dtDet.Rows[j]["PART_ID"].ToString() +
                                    "</TD></TR>";
                            Message = Message + "<TR><TD>Description: </TD> <TD>" + dtDet.Rows[j]["PART_DESC"].ToString() +
                                    "</TD></TR>";
                            Message = Message + "<TR><TD>CERTIFICATE ID: </TD> <TD>" + dtDet.Rows[j]["SERIAL_NUMBER"].ToString() + "</TD></TR></TABLE>";
                            blHasCerts = true;
                        }
                    }
                    Message = Message + "--------------------------------------------------------------------------------<BR>";
                    Message = Message + "</span>";
                    Message = Message + "To activate your License Certificate please have your Aruba Mobility Controller serial number available and visit the Aruba License Management WEB site at:<BR>" + ConfigurationSettings.AppSettings["ARUBA_SITE_URL"].ToString() + "<BR>";
                    Message = Message + "<BR><BR> There may be a 3-4 hour delay before this information is posted to the Aruba License Management WEB site.";
                    Message = Message + "Unless a hard copy software license certificate was requested for this order, this electronic delivery email will be the only source for the keys to enable this software.<BR><BR>";
                    Message = Message + "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR>";
                    Message = Message + "Email: support@arubanetworks.com <BR> Telephone:1-800-943-4526 (USA)<BR>1-408-754-1201 (International)<BR>BR><BR> Thank you for being an Aruba Networks customer.<BR><BR>Sent by Aruba Networks License Manager<BR><BR> http://www.arubanetworks.com<BR><BR>";
                    if (strAirwaveRec != string.Empty)
                    {
                        strAirwaveRec = strAirwaveRec.Substring(0, (strAirwaveRec.Length - 1));
                        DataTable dtAirwaveDet = dtDet.Clone();
                        DataRow[] drAirwResult = dtDet.Select("pk_cert_id in ( " + strAirwaveRec + " )");
                        foreach (DataRow dr in drAirwResult)
                            dtAirwaveDet.ImportRow(dr);
                        if (dtAirwaveDet.Rows.Count > 0)
                        {
                            blResult = sendAirwaveMail(dtAirwaveDet, strTo, strBrand);
                        }
                    }

                    if (strAmgRec != string.Empty)
                    {
                        strAmgRec = strAmgRec.Substring(0, (strAmgRec.Length - 1));
                        DataTable dtAmgDet = dtDet.Clone();
                        DataRow[] drAmgResult = dtDet.Select("pk_cert_id in ( " + strAmgRec + " )");
                        foreach (DataRow dr in drAmgResult)
                            dtAmgDet.ImportRow(dr);
                        if (dtAmgDet.Rows.Count > 0)
                        {
                            blResult = sendAmigopogMail(dtAmgDet, strTo, strBrand);
                        }
                    }

                    if (strALERec != string.Empty)
                    {
                        strALERec = strALERec.Substring(0, (strALERec.Length - 1));
                        DataTable dtALEDet = dtDet.Clone();
                        DataRow[] drALEResult = dtDet.Select("pk_cert_id in ( " + strALERec + " )");
                        foreach (DataRow dr in drALEResult)
                            dtALEDet.ImportRow(dr);
                        if (dtALEDet.Rows.Count > 0)
                        {
                            blResult = sendALEMail(dtALEDet, strTo, strBrand);
                        }
                    }                    

                    sub = "Copy of Software License Certificate Aruba Sales Order(s) " + dtHeader.Rows[i]["so_id"].ToString();
                    fromInfo[0] = "licensing@arubanetworks.com";
                    fromInfo[1] = "licensing@arubanetworks.com";
                }
                else
                {
                    Message = " This email is your electronic delivery for your recent transaction(s) with Aruba Networks as follows:<BR><BR>";
                    for (int j = 0; j < dtDet.Rows.Count; j++)
                    {
                        if (dtDet.Rows[j]["certType"].ToString() == "CERT")
                        {
                            Message = Message + "--------------------------------------------------------------------------------<BR><BR>";
                            Message = Message + "<TABLE width=100%><TR><TD>Alcatel Sales Order:</TD> <TD>" + dtDet.Rows[j]["SO_ID"].ToString() +
                                    "</TD></TR>";
                            Message = Message + "<TR><TD>Alcatel Part Number:</TD> <TD>" + dtDet.Rows[j]["PART_ID"].ToString() +
                                    "</TD></TR>";
                            Message = Message + "<TR><TD>Description: </TD> <TD>" + dtDet.Rows[j]["PART_DESC"].ToString() +
                                    "</TD></TR>";
                            Message = Message + "<TR><TD>CERTIFICATE ID: </TD> <TD>" + dtDet.Rows[j]["SERIAL_NUMBER"].ToString() +
                                "</TD></TR></TABLE>";
                            blHasCerts = true;
                        }
                    }

                    Message = Message + "--------------------------------------------------------------------------------<BR>";

                    Message = Message + "<BR><BR> This electronic confirmation may be used to activate your Software License Certificate and create a License Key that can be applied to your Alcatel Networks Mobility Controller platform to enable the required functionality.<BR><BR> To activate your License Certificate please have your Mobility Controller (or Supervisor Card in the case of Aruba 5000/6000) serial number available and visit the Aruba License Management WEB site at:<BR><BR> http://licensing.alcateloaw.com <BR><BR> There may be a 3-4 hour delay before this information is posted to the Alcatel License Management WEB site.<BR><BR>If you are a first time user, the Software License Certificate ID may be used to create a user account at the License Management WEB site.<BR><BR> You will receive a hard copy of this certificate shortly. This electronic copy of the transaction(s) serves as your confirmation.<BR><BR> Should you experience difficulties, please contact Alcatel Networks Customer Support at:<BR> Email: support@ind.alcatel.com <BR>Telephone: North America Service and Support: 1-800-995-2696. Latin America Service and Support: 1-877-919-9526. European Service and Support: +33-38-855-6929. Asia Pacific Service and Support: +65-6586-1555/1-818-878-4507 <BR><BR><BR> Thank you for being an Alcatel Networks customer.<BR><BR><BR>Sent by Alcatel License Manager<BR><BR> http://www.alcatel.com<BR><BR>";
                    sub = "Copy of Software License Certificate Alcatel Sales Order(s) " + dtHeader.Rows[i]["so_id"].ToString();
                    fromInfo[0] = "licensing@arubanetworks.com";
                    fromInfo[1] = "licensing@arubanetworks.com";
                }
                try
                {
                    if (blHasCerts == true)
                    {
                        blResult = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], strTo, string.Empty, sub, Message, ConfigurationManager.AppSettings["BCC_MAIL"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    new Log().logException("SendCertConfirmation", ex);
                }
            }
            return blResult;
        }


        public bool sendAirwaveMail(DataTable dtAirwaveDet, string strTo, string strBrand)
        {            
            bool retValAirw = false;
            string MessageAirw = string.Empty;
            string subAirw = string.Empty;
            string[] fromInfoAirw = { "", "" };
            string strUsername = string.Empty;
            string strPassword = string.Empty;
            string strLicenseKey;
            DACertificate objDACertificate = new DACertificate();
            try
            {
                // MessageAirw = "Thank you for your order " + dtAirwaveDet.Rows[0]["SHIP_TO_NAME"].ToString() + "<BR>";

                if (strBrand.Contains(ConfigurationManager.AppSettings["ARUBA_BRAND"].ToString()))
                {
                    MessageAirw = "This is the electronic confirmation of your requested Aruba AirWave Software License(s).<BR>";
                    MessageAirw = MessageAirw + " The below CERTIFICATE ID(s) are used to create a permanent License Key which is applied to enable AirWave on your server.<BR><BR>";
                    //SalesLoginInfo objSalesLoginInfo = new SalesLoginInfo(string.Empty, string.Empty);
                    //objSalesLoginInfo = GetSalesInfo(dtAirwaveDet.Rows[0]["LICENSE_CONTACT"].ToString(), dtAirwaveDet.Rows[0]["SO_ID"].ToString(), ConfigurationSettings.AppSettings["AUTH_KEY"].ToString());
                    //strUsername = objSalesLoginInfo.Login;
                    //strPassword = objSalesLoginInfo.Password;

                    //if (strUsername == string.Empty && strPassword == string.Empty)
                    //{
                    //    return false;
                    //}
                    MessageAirw = MessageAirw + "<span style=\"font-family:Arial;\">";
                    for (int j = 0; j < dtAirwaveDet.Rows.Count; j++)
                    {
                        strLicenseKey = string.Empty;
                        MessageAirw = MessageAirw + "-------------------------------------------------------------------------------------------<BR><BR>";
                        MessageAirw = MessageAirw + "<TABLE width=100%>";
                        if (dtAirwaveDet.Rows[j]["CUST_PO_ID"].ToString() != string.Empty)
                        {
                            MessageAirw = MessageAirw + "<TR><TD>Customer PO:</TD> <TD>" + dtAirwaveDet.Rows[j]["CUST_PO_ID"].ToString() + "</TD></TR>";
                        }
                        if (dtAirwaveDet.Rows[j]["END_USER_PO"].ToString() != string.Empty)
                        {
                            MessageAirw = MessageAirw + "<TR><TD>Reseller/End User PO:</TD> <TD>" + dtAirwaveDet.Rows[j]["END_USER_PO"].ToString() + "</TD></TR>";
                        }
                        MessageAirw = MessageAirw + "<TR><TD>Aruba Sales Order:</TD> <TD>" + dtAirwaveDet.Rows[j]["SO_ID"].ToString() + "</TD></TR>";
                        MessageAirw = MessageAirw + "<TR><TD>Aruba Part Number:</TD> <TD>" + dtAirwaveDet.Rows[j]["PART_ID"].ToString() +
                                "</TD></TR>";
                        MessageAirw = MessageAirw + "<TR><TD>Description: </TD> <TD>" + dtAirwaveDet.Rows[j]["PART_DESC"].ToString() +
                                "</TD></TR>";
                        MessageAirw = MessageAirw + "<TR><TD>CERTIFICATE ID: </TD> <TD>" + dtAirwaveDet.Rows[j]["SERIAL_NUMBER"].ToString() + "</TD></TR>";
                        strLicenseKey = objDACertificate.getAWCertificate(dtAirwaveDet.Rows[j]["SERIAL_NUMBER"].ToString());
                        MessageAirw = MessageAirw + "<TR><TD>Temporary License Key: </TD> <TD> <BR><BR>" + strLicenseKey.Replace("\n", "<BR>") + "</TD></TR></TABLE>";
                    }

                    MessageAirw = MessageAirw + "------------------------------------------------------------------------------------------------<BR>";
                    MessageAirw = MessageAirw + "</span><BR>";
                    MessageAirw = MessageAirw + "<B>IF YOU ARE CURRENTLY USING A DEMO COPY OF AirWave:</B><BR><BR>";
                    MessageAirw = MessageAirw + "1. Copy the license key including the <I>Begin AMP License Key </I> and <I> End AMP License Key </I> lines.<BR><BR>";
                    MessageAirw = MessageAirw + "2. Navigate to AWMS Home-->License page and enter your temporary AirWave Software License Key.<BR><BR>";
                    MessageAirw = MessageAirw + "3. Accept the license terms.<BR><BR>";
                    MessageAirw = MessageAirw + "4.	Follow instructions below to obtain your permanent AirWave license key.<BR><BR><BR>";
                    MessageAirw = MessageAirw + "<B> ACTIVATING YOUR PERMANENT AirWave LICENSE KEY </B> <BR><BR>";
                    MessageAirw = MessageAirw + "To activate your License Certificate please have your server’s permanent IP address available.<BR><BR>";
                    MessageAirw = MessageAirw + "1. Visit the Aruba License Management WEB site at:<BR>" + ConfigurationManager.AppSettings["ARUBA_SITE_URL"].ToString() + "<BR><BR>";
                    MessageAirw = MessageAirw + "<I>Note: There may be a 3-4 hour delay before this information is posted to the Aruba License Management WEB site.";
                    MessageAirw = MessageAirw + "Unless a hard copy software license certificate was requested for this order, this electronic delivery email will be the only source for the keys to enable this software.</I><BR><BR>";
                    MessageAirw = MessageAirw + "2.	Click on the <I>My AirWave-->Activate AirWave Certificate</I> tab. <BR><BR>";
                    MessageAirw = MessageAirw + "3.	Enter your server’s permanent IP Address, Certificate ID, and Organization detail to obtain the permanent license key. <BR><BR>";
                    MessageAirw = MessageAirw + "4.	Copy the license key including the 'Begin AMP License Key' and 'End AMP License Key' lines. <BR><BR>";
                    MessageAirw = MessageAirw + "5.	 Navigate to <I>AWMS Home-->License page</I> and enter your temporary AirWave Software License Key. <BR><BR>";
                    MessageAirw = MessageAirw + "6.	 Accept the license terms. <BR><BR><BR>";
                    MessageAirw = MessageAirw + "<B>INSTALLING AirWave ON A NEW SERVER: </B><BR><BR>";
                    MessageAirw = MessageAirw + "1. Download the AirWave software image from the utility <I>My AirWave > Download AirWave Software</I> of " + ConfigurationManager.AppSettings["ARUBA_SITE_URL"].ToString() + " by right clicking the appropriate link and selecting the <I><B>Save As </I></B> or <I><B>Save Link As </I></B> option saving the file with a .iso ";
                    MessageAirw = MessageAirw + "extension (iso burning instructions can be found in the AirWave Quick Start Guide available at http://support.arubanetworks.com). <BR><BR>";
                    MessageAirw = MessageAirw + "2. Burn a CD from the AMP software image.(iso burning instructions can be found in the AirWave Quick Start Guide available at http://support.arubanetworks.com)<BR><BR>";
                    MessageAirw = MessageAirw + "3. Place the CD in your AirWave server and restart the machine. The computer will boot off of the CD and begin the install phase.<BR>";
                    MessageAirw = MessageAirw + "<I>(Note: Recommended hardware configurations and complete installation instructions can be found in the User Guide).</I> <BR><BR>";
                    MessageAirw = MessageAirw + "4. After installing the AirWave Management System software on your server, you will be prompted to enter in a license key.Copy the temporary license key including the 'Begin AMP License Key' and 'End AMP License Key' lines. <BR><BR>";
                    MessageAirw = MessageAirw + "5.	Navigate to <I>AWMS Home-->License page</I> on your AirWave server to enter your permanent AirWave Software License Key.<BR><BR>";
                    MessageAirw = MessageAirw + "6. Accept the license terms.<BR><BR>";
                    MessageAirw = MessageAirw + "If you encounter any difficulty during the installation of the AWMS software or at any time during your use of AWMS, please do not hesitate to contact us.<BR><BR>";
                    MessageAirw = MessageAirw + "If you have any difficulty installing AirWave or have any questions, please contact Aruba Networks Technical Support at ";
                    MessageAirw = MessageAirw + "support@arubanetworks.com  or call 1-800-943-4526(U.S.A) or +1-408-419-4098 (International)<BR><BR> We look forward to helping you manage your wireless network.<BR><BR>Thank you for being an Aruba customer.<BR><BR><BR>";
                    subAirw = "Copy of Software License Certificate Aruba AirWave Sales Order(s) " + dtAirwaveDet.Rows[0]["so_id"].ToString();
                    //fromInfoAirw[0] = "licensing@arubanetworks.com";
                    //fromInfoAirw[1] = "licensing@arubanetworks.com";
                    fromInfoAirw = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                }
                else
                {
                    MessageAirw = MessageAirw + " This is the electronic delivery confirmation of your purchased Alcatel AirWave Software License(s).<BR>";
                    MessageAirw = MessageAirw + " The below CERTIFICATE ID(s) are used to create a permanent License Key which is applied to enable AirWave on your server.<BR><BR>";
                    //SalesLoginInfo objSalesLoginInfo = new SalesLoginInfo(string.Empty, string.Empty);
                    //objSalesLoginInfo = GetSalesInfo(dtAirwaveDet.Rows[0]["LICENSE_CONTACT"].ToString(), dtAirwaveDet.Rows[0]["SO_ID"].ToString(), ConfigurationSettings.AppSettings["AUTH_KEY"].ToString());
                    //strUsername = objSalesLoginInfo.Login;
                    //strPassword = objSalesLoginInfo.Password;
                    MessageAirw = MessageAirw + "<span style=\"font-family:Arial;\">";
                    for (int j = 0; j < dtAirwaveDet.Rows.Count; j++)
                    {
                        MessageAirw = MessageAirw + "--------------------------------------------------------------------------------------<BR><BR>";
                        MessageAirw = MessageAirw + "<TABLE width=100%><TR><TD>Customer PO:</TD> <TD>" + dtAirwaveDet.Rows[j]["CUST_PO_ID"].ToString() + "</TD></TR>";
                        MessageAirw = MessageAirw + "<TR><TD>Alcatel Sales Order:</TD> <TD>" + dtAirwaveDet.Rows[j]["SO_ID"].ToString() +
                                "</TD></TR>";
                        MessageAirw = MessageAirw + "<TR><TD>Alcatel Part Number:</TD> <TD>" + dtAirwaveDet.Rows[j]["PART_ID"].ToString() +
                                "</TD></TR>";
                        MessageAirw = MessageAirw + "<TR><TD>Description: </TD> <TD>" + dtAirwaveDet.Rows[j]["PART_DESC"].ToString() +
                                "</TD></TR>";
                        MessageAirw = MessageAirw + "<TR><TD>Contact: </TD> <TD>" + dtAirwaveDet.Rows[j]["LICENSE_CONTACT"].ToString() +
                                "</TD></TR>";
                        MessageAirw = MessageAirw + "<TR><TD>CERTIFICATE ID: </TD> <TD>" + dtAirwaveDet.Rows[j]["SERIAL_NUMBER"].ToString() + "</TD></TR></TABLE>";
                        MessageAirw = MessageAirw + "<TR><TD>Temporary License Key: </TD> <TD> <BR><BR>" + dtAirwaveDet.Rows[j]["LICKEY"].ToString().Replace("\n", "<BR>") + "</TD></TR></TABLE>";
                    }

                    MessageAirw = MessageAirw + "-------------------------------------------------------------------------------------------<BR>";
                    MessageAirw = MessageAirw + "</span><BR>";
                    MessageAirw = MessageAirw + "IF YOU ARE CURRENTLY USING A DEMO COPY OF AirWave:</B><BR><BR>";
                    MessageAirw = MessageAirw + "1. Copy the license key including the <I>Begin AMP License Key </I> and <I> End AMP License Key </I> lines.<BR><BR>";
                    MessageAirw = MessageAirw + "2. Navigate to AWMS Home-->License page and enter your temporary AirWave Software License Key.<BR><BR>";
                    MessageAirw = MessageAirw + "3. Accept the license terms.<BR><BR>";
                    MessageAirw = MessageAirw + "4.	Follow instructions below to obtain your permanent AirWave license key.<BR><BR><BR>";
                    MessageAirw = MessageAirw + "<B> ACTIVATING YOUR PERMANENT AirWave LICENSE KEY </B> <BR><BR>";
                    MessageAirw = MessageAirw + "To activate your License Certificate please have your server’s permanent IP address available.<BR><BR>";
                    MessageAirw = MessageAirw + "1. Visit the Alcatel License Management WEB site at:<BR>" + ConfigurationManager.AppSettings["ALCATEL_URL"].ToString() + "<BR>";
                    MessageAirw = MessageAirw + "<I>Note: There may be a 3-4 hour delay before this information is posted to the Alcatel License Management WEB site.";
                    MessageAirw = MessageAirw + "Unless a hard copy software license certificate was requested for this order, this electronic delivery email will be the only source for the keys to enable this software.</I><BR><BR>";
                    MessageAirw = MessageAirw + "2.	Click on the <I>My AirWave-->Activate AirWave Certificate</I> tab. <BR><BR>";
                    MessageAirw = MessageAirw + "3.	Enter your server’s permanent IP Address, Certificate ID, and Organization detail to obtain the permanent license key. <BR><BR>";
                    MessageAirw = MessageAirw + "4.	Copy the license key including the 'Begin AMP License Key' and 'End AMP License Key' lines. <BR><BR>";
                    MessageAirw = MessageAirw + "5.	 Navigate to <I>AWMS Home-->License page</I> and enter your temporary AirWave Software License Key. <BR><BR>";
                    MessageAirw = MessageAirw + "6.	 Accept the license terms. <BR><BR><BR>";
                    MessageAirw = MessageAirw + "<B>INSTALLING AirWave ON A NEW SERVER: </B><BR><BR>";
                    MessageAirw = MessageAirw + "1. Download the AirWave software image from the utility <I>My AirWave > Download AirWave Software</I> of " + ConfigurationManager.AppSettings["ALCATEL_SITE_URL"].ToString() + " by right clicking the appropriate link and selecting the <I><B>Save As </I></B> or <I><B>Save Link As </I></B> option saving the file with a .iso ";
                    MessageAirw = MessageAirw + "extension (iso burning instructions can be found in the AirWave Quick Start Guide available at http://support@ind.alcatel.com). <BR><BR>";
                    MessageAirw = MessageAirw + "2. Burn a CD from the AMP software image.(iso burning instructions can be found in the AirWave Quick Start Guide available at http://support@ind.alcatel.com)<BR><BR>";
                    MessageAirw = MessageAirw + "3. Place the CD in your AirWave server and restart the machine. The computer will boot off of the CD and begin the install phase.<BR><BR>";
                    MessageAirw = MessageAirw + "<I>(Note: Recommended hardware configurations and complete installation instructions can be found in the User Guide).</I> <BR><BR>";
                    MessageAirw = MessageAirw + "4. After installing the AirWave Management System software on your server, you will be prompted to enter in a license key.Copy the temporary license key including the 'Begin AMP License Key' and 'End AMP License Key' lines. <BR><BR>";
                    MessageAirw = MessageAirw + "5.	Navigate to AWMS Home-->License page on your AirWave server to enter your permanent AirWave Software License Key.<BR><BR>";
                    MessageAirw = MessageAirw + "6. Accept the license terms.<BR><BR>";
                    MessageAirw = MessageAirw + "If you encounter any difficulty during the installation of the AWMS software or at any time during your use of AWMS, please do not hesitate to contact us.<BR><BR>";
                    MessageAirw = MessageAirw + "If you have any difficulty installing AirWave or have any questions, please contact Alcatel Networks Technical Support at ";
                    MessageAirw = MessageAirw + "support@ind.alcatel.com  or Call North America Service and Support: 1-800-995-2696. Latin America Service and Support: 1-877-919-9526. European Service and Support: +33-38-855-6929. Asia Pacific Service and Support: +65-6586-1555/1-818-878-4507 <BR> We look forward to helping you manage your wireless network.<BR><BR>Thank you for being an Alcatel customer.<BR><BR><BR>";
                    subAirw = "Copy of Software License Certificate Alcatel AirWave Sales Order(s) " + dtAirwaveDet.Rows[0]["so_id"].ToString();
                    fromInfoAirw = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                    subAirw = "Copy of Software License Certificate Alcatel Sales Order(s) " + dtAirwaveDet.Rows[0]["so_id"].ToString();
                    //fromInfoAirw[0] = "licensing@arubanetworks.com";
                    //fromInfoAirw[1] = "licensing@arubanetworks.com";
                }
                retVal = sendMailFromToCcBccSubMsg(fromInfoAirw[0], fromInfoAirw[1], strTo, string.Empty, subAirw, MessageAirw, ConfigurationManager.AppSettings["BCC_MAIL"].ToString());
            }
            catch (Exception e)
            {
                new Log().logException("SendAirwCertConfirmation", e);
            }
            return retVal;
        }

        public bool sendALEMail(DataTable dtALEDet, string strTo, string strBrand)
        {
            bool retValALE = false;
            string MsgALE = string.Empty;
            string subALE = string.Empty;
            string[] fromInfoALE = { "", "" };
            
            MsgALE = MsgALE + "Thank you for your order. " + "<BR>";

            MsgALE = MsgALE + " This is the electronic delivery confirmation of your purchased ALE product(s).<BR>";
            MsgALE = MsgALE + " The below CERTIFICATE ID(s) will be used to create a Activation Key that you will use to enable your software license functionality.<BR><BR>";

            for (int j = 0; j < dtALEDet.Rows.Count; j++)
            {
                MsgALE = MsgALE + "-----------------------------------------------------------------------------------------------------------------------<BR><BR>";
                MsgALE = MsgALE + "<TABLE width=100%><TR><TD>Customer PO:</TD> <TD>" + dtALEDet.Rows[j]["CUST_PO_ID"].ToString() + "</TD></TR>";
                if (dtALEDet.Rows[j]["END_USER_PO"].ToString() != string.Empty)
                {
                    MsgALE = MsgALE + "<TR><TD>Reseller / End user PO:</TD> <TD>" + dtALEDet.Rows[j]["END_USER_PO"].ToString() + "</TD></TR>";
                }
                MsgALE = MsgALE + "<TR><TD>Aruba Sales Order:</TD> <TD>" + dtALEDet.Rows[j]["SO_ID"].ToString() + "</TD></TR>";
                MsgALE = MsgALE + "<TR><TD>Aruba Part Number:</TD> <TD>" + dtALEDet.Rows[j]["PART_ID"].ToString() + "</TD></TR>";
                MsgALE = MsgALE + "<TR><TD>Description: </TD> <TD>" + dtALEDet.Rows[j]["PART_DESC"].ToString() + "</TD></TR>";
                //MsgALE = MsgALE + "<TR><TD>Contact: </TD> <TD>" + dtAmgDet.Rows[j]["LICENSE_CONTACT"].ToString() + "</TD></TR>";
                MsgALE = MsgALE + "<TR><TD>CERTIFICATE ID: </TD> <TD><span style=\"font-family:Courier New;\">" + dtALEDet.Rows[j]["SERIAL_NUMBER"].ToString() + "</span></TD></TR>";

                MsgALE = MsgALE + "<TR><TD>Serial Number: </TD> <TD>" + dtALEDet.Rows[j]["Lserial_number"].ToString() +
                            "</TD></TR></TABLE>";
            }

            MsgALE = MsgALE + "-----------------------------------------------------------------------------------------------------------------------<BR>";
            MsgALE = MsgALE + "To generate your Subscription ID, please visit the Aruba License Management website at: <BR>" + ConfigurationManager.AppSettings["ARUBA_SITE_URL"].ToString() + "<BR>";
            MsgALE = MsgALE + "<BR><BR> There may be a 3-4 hour delay before this information is posted to the Aruba License Management WEB site.";
            MsgALE = MsgALE + "Unless a hard copy software license certificate was requested for this order, this electronic delivery email will be the only source for the keys to enable this software.<BR><BR>";
            MsgALE = MsgALE + "If you are a first time user, the Software License Certificate ID may be used to create a user account. <BR>";
            MsgALE = MsgALE + "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR>";
            MsgALE = MsgALE + "Email: support@arubanetworks.com <BR> Telephone:1-800-943-4526 (USA)<BR> 1-408-754-1201 (International)<BR><BR><BR> Thank you for being an Aruba Networks customer.<BR><BR>Sent by Aruba Networks License Manager<BR><BR> http://www.arubanetworks.com<BR><BR>";
            subALE = "Copy of Software License Certificate Aruba Sales Order(s) " + dtALEDet.Rows[0]["so_id"].ToString();
            //fromInfo[0] = "licensing@arubanetworks.com";
            //fromInfo[1] = "licensing@arubanetworks.com";
            fromInfoALE = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');            

            try
            {
                retValALE = sendMailFromToCcBccSubMsg(fromInfoALE[0], fromInfoALE[1], strTo, string.Empty, subALE, MsgALE, ConfigurationManager.AppSettings["BCC_MAIL"].ToString());
            }

            catch (Exception ex)
            {
                new Log().logException("SendCertConfirmation", ex);
            }
            return retValALE;
        }

        public bool sendAmigopogMail(DataTable dtAmgDet, string strTo,string strBrand)
        {                        
            bool retValAmg = false;
            string MsgAmg = string.Empty;            
            string subAmg = string.Empty;
            string[] fromInfoAmg = { "", "" };

            
            MsgAmg = MsgAmg + "Thank you for your order. " + "<BR>";

            MsgAmg = MsgAmg + " This is the electronic delivery confirmation of your purchased ClearPass product(s).<BR>";
            MsgAmg = MsgAmg + " The below CERTIFICATE ID(s) will be used to create a Subscription ID that you will use to enable your software license functionality.<BR><BR>";

            for (int j = 0; j < dtAmgDet.Rows.Count; j++)
            {
                MsgAmg = MsgAmg + "-----------------------------------------------------------------------------------------------------------------------<BR><BR>";
                MsgAmg = MsgAmg + "<TABLE width=100%><TR><TD>Customer PO:</TD> <TD>" + dtAmgDet.Rows[j]["CUST_PO_ID"].ToString() + "</TD></TR>";
                if (dtAmgDet.Rows[j]["END_USER_PO"].ToString() != string.Empty)
                {
                    MsgAmg = MsgAmg + "<TR><TD>Reseller / End user PO:</TD> <TD>" + dtAmgDet.Rows[j]["END_USER_PO"].ToString() + "</TD></TR>";
                }
                MsgAmg = MsgAmg + "<TR><TD>Aruba Sales Order:</TD> <TD>" + dtAmgDet.Rows[j]["SO_ID"].ToString() + "</TD></TR>";
                MsgAmg = MsgAmg + "<TR><TD>Aruba Part Number:</TD> <TD>" + dtAmgDet.Rows[j]["PART_ID"].ToString() + "</TD></TR>";
                MsgAmg = MsgAmg + "<TR><TD>Description: </TD> <TD>" + dtAmgDet.Rows[j]["PART_DESC"].ToString() + "</TD></TR>";
                //MsgAmg = MsgAmg + "<TR><TD>Contact: </TD> <TD>" + dtAmgDet.Rows[j]["LICENSE_CONTACT"].ToString() + "</TD></TR>";
                MsgAmg = MsgAmg + "<TR><TD>CERTIFICATE ID: </TD> <TD><span style=\"font-family:Courier New;\">" + dtAmgDet.Rows[j]["SERIAL_NUMBER"].ToString() + "</span></TD></TR>";

                MsgAmg = MsgAmg + "<TR><TD>Serial Number: </TD> <TD>" + dtAmgDet.Rows[j]["Lserial_number"].ToString() +
                            "</TD></TR></TABLE>";
            }

            MsgAmg = MsgAmg + "-----------------------------------------------------------------------------------------------------------------------<BR>";
            MsgAmg = MsgAmg + "To generate your Subscription ID, please visit the Aruba License Management website at: <BR>" + ConfigurationManager.AppSettings["ARUBA_SITE_URL"].ToString() + "<BR>";
            MsgAmg = MsgAmg + "<BR><BR> There may be a 3-4 hour delay before this information is posted to the Aruba License Management WEB site.";
            MsgAmg = MsgAmg + "Unless a hard copy software license certificate was requested for this order, this electronic delivery email will be the only source for the keys to enable this software.<BR><BR>";
            MsgAmg = MsgAmg + "If you are a first time user, the Software License Certificate ID may be used to create a user account. <BR>";
            MsgAmg = MsgAmg + "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR>";
            MsgAmg = MsgAmg + "Email: support@arubanetworks.com <BR> Telephone:1-800-943-4526 (USA)<BR> 1-408-754-1201 (International)<BR><BR><BR> Thank you for being an Aruba Networks customer.<BR><BR>Sent by Aruba Networks License Manager<BR><BR> http://www.arubanetworks.com<BR><BR>";
            subAmg = "Copy of Software License Certificate Aruba Sales Order(s) " + dtAmgDet.Rows[0]["so_id"].ToString();
            //fromInfo[0] = "licensing@arubanetworks.com";
            //fromInfo[1] = "licensing@arubanetworks.com";
            fromInfoAmg = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');            

            try
            {
                retValAmg = sendMailFromToCcBccSubMsg(fromInfoAmg[0], fromInfoAmg[1], strTo, string.Empty, subAmg, MsgAmg, ConfigurationManager.AppSettings["BCC_MAIL"].ToString());
            }

            catch (Exception ex)
            {
                new Log().logException("SendCertConfirmation", ex);
            }
            return retValAmg;
        }        

        public bool sendPreparedAccountCreation(AccountMailInfo objLgm)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            switch (objLgm.Brand)
            {
                case "ARUBA":
                    Message = "Your new Aruba License Management account has been created.<BR/><BR/>" +
                                   "Your login info is as follows:<BR/>" +
                                    "Username: " + objLgm.Email + "<BR/>" +
                                    "Password : ** the password you entered ** <BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"] + "<BR/>" +
                                    "Aruba Networks Webmaster <BR/>" +
                                    ConfigurationManager.AppSettings["ARUBA_SITE_URL"];
                    sub = "Aruba License Management Website account creation";
                    fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');

                    break;
                case "ALCATEL":
                    Message = "Your new Alcatel OmniAccess License Management account has been created.<BR/><BR/>" +
                                  "Your login info is as follows:<BR/>" +
                                   "Username: " + objLgm.Email + "<BR/>" +
                                   "Password : ** the password you entered ** <BR/>" +
                                   ConfigurationManager.AppSettings["ALCATEL_SITE_URL"] + "<BR/>" +
                                   "Alcatel OmniAccess Licensing Webmaster <BR/>" +
                                   ConfigurationManager.AppSettings["ALCATEL_SITE_URL"];
                    sub = "Alcatel OmniAccess License Management Website prepared account waiting for you";

                    fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                    break;

            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, Bcc);
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendPreparedAccountCreation", "Mail not sent:" + objLgm.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objLgm.Email, "", bcc, sub, Message, "", "PREPARED_ACCOUNT_CREATION");
            }

            return retVal;
        }

        //Added by Ashwini

        public bool sendMailFromToCcSubMsg(String fromName, String fromAdd, String toline, String cc, String subj, String htmlmsg, String attachment)
        {
            bool retVal = true;
            Chilkat.MailMan mm = new Chilkat.MailMan();
            Chilkat.Email em = new Chilkat.Email();

            try
            {
                if (subj == "")
                {
                    subj = "An item in queue requires your attention";
                }
                mm.SmtpHost = ConfigurationManager.AppSettings["MAIL_SERVER"];
                mm.SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SERVER_PORT"]);
                mm.StartTLS = true;
                //mm.SmtpUsername = "noreply";
                //mm.SmtpPassword = "n01sh0m3!";

                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };

                mm.SmtpSsl = false;
                mm.UnlockComponent("UFANGMAIL_uEHvvVrp2Rpr");
                em.AddMultipleTo(toline.Replace(";", ","));
                em.AddMultipleCC(cc.Replace(";", ","));
                em.FromName = fromName;
                em.FromAddress = fromAdd;
                em.Subject = subj;
                em.SetHtmlBody(htmlmsg);
                if (attachment.Length > 0)
                {
                    //em.AddFileAttachment(attachment);
                    em.AddStringAttachment("License.txt", attachment);
                }
                retVal = mm.SendEmail(em);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public bool sendMailFromToCcBccSubMsg(string fromName, string fromAdd, string toline, string cc, string subj, string htmlmsg, string bcc)
        {
            bool retVal = true;
            Chilkat.MailMan mm = new Chilkat.MailMan();
            Chilkat.Email em = new Chilkat.Email();

            try
            {
                if (subj == "")
                {
                    subj = "An item in queue requires your attention";
                }
                mm.SmtpHost = ConfigurationManager.AppSettings["MAIL_SERVER"];
                mm.SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SERVER_PORT"]);
                mm.StartTLS = true;
                //mm.SmtpUsername = "noreply";
                //mm.SmtpPassword = "n01sh0m3!";
                mm.UnlockComponent("UFANGMAIL_uEHvvVrp2Rpr");

                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };

                mm.SmtpSsl = false;
                em.AddMultipleTo(toline.Replace(";", ","));
                em.AddMultipleCC(cc.Replace(";", ","));
                em.AddMultipleBcc(bcc.Replace(";", ","));
                em.FromName = fromName;
                em.FromAddress = fromAdd;
                em.Subject = subj;
                em.SetHtmlBody(htmlmsg);
                retVal = mm.SendEmail(em);
                string str = mm.LastErrorText;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        //Added by Ashwini 22/mar/2013

        public bool sendClpCertActivationInfo(DataSet DsSubAll, UserInfo objUser, string strbrand)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            string strCertDetails = "<table>";
            string strSubKey = DsSubAll.Tables["SUBINFO"].Rows[0]["SubscriptionKey"].ToString();
            string strUserName = DsSubAll.Tables["SUBINFO"].Rows[0]["user_name"].ToString();
            string strPassword = DsSubAll.Tables["SUBINFO"].Rows[0]["password"].ToString();
            string strCategory = DsSubAll.Tables["SUBINFO"].Rows[0]["category"].ToString();
            
            string mailTemplateFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AVENDA_LIC_MAIL_TEMPLATE"]);
            Message = File.ReadAllText(mailTemplateFile);
            Message = Message.Replace("##NAME##", objUser.FirstName + " " + objUser.LastName);
            Message = Message.Replace("##USERNAME##", objUser.FirstName + " " + objUser.LastName);
            Message = Message.Replace("##SUBSCRIPTIONID##",strSubKey);

            if (strCategory == "Software")
            {
                Message = Message.Replace("##SoftwareCaption##", "<b>QuickConnect Credentials</b>");
                Message = Message.Replace("##url##", ConfigurationManager.AppSettings["CLS_URL"].ToString());
                Message = Message.Replace("##UserNameCaption##", "User Name");
                Message = Message.Replace("##deliniter##", ":");
                Message = Message.Replace("##usernameval##", strUserName);

                Message = Message.Replace("##PasswordCaption##", "Password");
                Message = Message.Replace("##password##", strPassword);

                Message = Message.Replace("##infoMessage##", "");
            }
            else
            {
                Message = Message.Replace("##SoftwareCaption##", "");
                Message = Message.Replace("##url##", "");

                Message = Message.Replace("##UserNameCaption##", "");
                Message = Message.Replace("##deliniter##", "");
                Message = Message.Replace("##usernameval##", "");

                Message = Message.Replace("##PasswordCaption##", "");
                Message = Message.Replace("##password##", "");

                Message = Message.Replace("##infoMessage##", "");
            }

            for (int i=0;i<DsSubAll.Tables["CERTINFO"].Rows.Count;i++)
            {
                //strCertDetails = strCertDetails + "<tr><td>SO Id: </td><td> " + DsSubAll.Tables["CERTINFO"].Rows[i]["so_id"].ToString()+ "</td></tr>";
                strCertDetails = strCertDetails + "<tr><td style=\"width:30%\">Part No: </td><td style=\"width:70%\"> " + DsSubAll.Tables["CERTINFO"].Rows[i]["LicPartId"].ToString() + "</td></tr>";
                strCertDetails = strCertDetails + "<tr><td style=\"width:30%\">Description: </td><td style=\"width:70%\"> " + DsSubAll.Tables["CERTINFO"].Rows[i]["LicPartDesc"].ToString() + "</td></tr>";
                strCertDetails = strCertDetails + "<tr><td style=\"width:30%\">Certificate Id: </td><td style=\"width:70%\"> " + DsSubAll.Tables["CERTINFO"].Rows[i]["LicSN"].ToString() + "</td></tr>";
                strCertDetails = strCertDetails + "<tr><td style=\"width:30%\">Version: </td><td style=\"width:70%\"> " + DsSubAll.Tables["CERTINFO"].Rows[i]["Version"].ToString() + "</td></tr>";
                strCertDetails = strCertDetails + "<tr><td style=\"width:30%\">Activation Key: </td><td style=\"width:70%\"> " + DsSubAll.Tables["CERTINFO"].Rows[i]["ActivationKey"].ToString() + "</td></tr>";
                strCertDetails = strCertDetails + "<tr><td style=\"width:30%\">Location: </td><td style=\"width:70%\"> " + DsSubAll.Tables["CERTINFO"].Rows[i]["Location"].ToString() + "</td></tr>";
                strCertDetails = strCertDetails + "<tr><td colspan=\"2\">---------------------------------------------------------------------------------------------------------------------------------------------------------</td></tr>";                
            }
            strCertDetails = strCertDetails + "</table>";
            Message = Message.Replace("##CERTTABLE##", strCertDetails);

            switch (strbrand)
            {
                case "ARUBA":
                    {
                        sub = "Aruba ClearPass License Details for Subscription ID: " + strSubKey;
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        sub = "Alcatel ClearPass License Details for Subscription ID: " + strSubKey;
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
            }

            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objUser.GetUserEmail(), "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objUser.GetUserEmail(), "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendAvendaLicenseInfo", "Mail not sent:" + strSubKey);
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objUser.GetUserEmail(), "", bcc, sub, Message, "", "AVENDA_LIC_ACTIVATION_MAIL");
            }
            return retVal;
        }

        public bool sendAvendaImportLicKeyDet(AmigopodCertInfo objAmigopodCertInfo)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            string cert_table = string.Empty;

            string mailTemplateFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AVENDA_IMPLIC_MAIL_TEMPLATE"]);
            Message = File.ReadAllText(mailTemplateFile);
            Message = Message.Replace("##NAME##", objAmigopodCertInfo.Name);
            Message = Message.Replace("##SUBSCRIPTIONID##", objAmigopodCertInfo.Subscriptionkey);
            cert_table = "<table>";
            cert_table = cert_table + "<tr><b><td>Part Id </td><td>Part Desc</td><td>Certificate Id</td><td>Certificate Serial Number</td><td>Activated Date</td><td>Expiry Date</td><td>License Key</td><td>Version</td></b></tr>";
            for (int i = 0; i < objAmigopodCertInfo.lstClsLicense.Count; i++)
            {
                cert_table = cert_table + "<TR>";
                cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpPartId + "</TD>";
                cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpPartDesc + "</TD>";
                cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpCertId + "</TD>";
                cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpCertSerialNo + "</TD>";
                cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpCreatedDate + "</TD>";
                cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpExpiryDate + "</TD>";
                cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpLicKey + "</TD>";
                cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpVersion + "</TD>";
                cert_table = cert_table + "</TR>";
            }
            cert_table = cert_table + "</table>";
            Message = Message.Replace("##CERTTABLE##", cert_table);

            switch (objAmigopodCertInfo.Brand)
            {
                case "ARUBA":
                    {
                        sub = "Aruba ClearPass Account Details: " + objAmigopodCertInfo.Name + " - Sales Order: " + objAmigopodCertInfo.SoId;
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        sub = "Alcatel ClearPass Account Details: " + objAmigopodCertInfo.Name + " - Sales Order: " + objAmigopodCertInfo.SoId;
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
            }

            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendAvendaImportLicKeyDet", "Mail not sent:" + objAmigopodCertInfo.Subscriptionkey);
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", bcc, sub, Message, "", "AVENDA_IMPORT_LIC_MAIL");
            }

            return retVal;
        }

        public bool sendAvendaImportSubandLicKeyDet(AmigopodCertInfo objAmigopodCertInfo)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };
            string cert_table = string.Empty;

            string mailTemplateFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AVENDA_IMPSUB_LIC_MAIL_TEMPLATE"]);
            Message = File.ReadAllText(mailTemplateFile);
            Message = Message.Replace("##NAME##", objAmigopodCertInfo.Name);
            Message = Message.Replace("##SUBSCRIPTIONID##", objAmigopodCertInfo.Subscriptionkey);
            Message = Message.Replace("##CertId##", objAmigopodCertInfo.CertId);
            Message = Message.Replace("##SerialNumber##", objAmigopodCertInfo.SerialNumber);
            Message = Message.Replace("##SALESORDER##", objAmigopodCertInfo.SoId);
            Message = Message.Replace("##PURCHASEORDER##", objAmigopodCertInfo.PoId);
            Message = Message.Replace("##EXPIRYDATE##", objAmigopodCertInfo.ExpiryDate.ToString());
            Message = Message.Replace("##USERNAME##", objAmigopodCertInfo.UserName);
            Message = Message.Replace("##PASSWORD##", objAmigopodCertInfo.Password);
            cert_table = "<table border=\"1\">";
            cert_table = "<tr><b><td>Part No </td><td>Part Description </td><td> Certificate Id </td><td>Certificate Serial No </td><td> Activated Date</td> <td>Expiry Date</td><td>Activation Key </td> <td> Version </td></b></tr>";
            for (int i = 0; i < objAmigopodCertInfo.lstClsLicense.Count; i++)
            {
            cert_table = cert_table + "<TR>";
            cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpPartId + "</TD>";
            cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpPartDesc + "</TD>";
            cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpCertId + "</TD>";
            cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpCertSerialNo + "</TD>";
            cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpCreatedDate + "</TD>";
            cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpExpiryDate + "</TD>";
            cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpLicKey + "</TD>";
            cert_table = cert_table + "<TD>" + objAmigopodCertInfo.lstClsLicense[i].clpVersion + "</TD>";
            cert_table = cert_table + "</TR>";
            }
            cert_table = cert_table + "</table>";
            Message = Message.Replace("##CERTTABLE##", cert_table);

            switch (objAmigopodCertInfo.Brand)
            {
                case "ARUBA":
                    {
                        sub = "Aruba ClearPass Account Details: " + objAmigopodCertInfo.Name + " - Sales Order: " + objAmigopodCertInfo.SoId;
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        sub = "Alcatel ClearPass Account Details: " + objAmigopodCertInfo.Name + " - Sales Order: " + objAmigopodCertInfo.SoId;
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
            }

            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendAvendaImportSubandLicKeyDet", "Mail not sent:" + objAmigopodCertInfo.Subscriptionkey);
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", bcc, sub, Message, "", "AVENDA_SUBSCRIPTION_LIC_IMPORT_MAIL");
            }

            return retVal;
        }

        //Added by Ashwini on Mar/16/2013

        public bool sendAvendaImportSubKeyDet(AvendaCert objAvendaCert)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };

            string mailTemplateFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AVENDA_IMPSUB_MAIL_TEMPLATE"]);
            Message = File.ReadAllText(mailTemplateFile);
            Message = Message.Replace("##NAME##", objAvendaCert.name);
            Message = Message.Replace("##SUBSCRIPTIONID##", objAvendaCert.subscription);
            Message = Message.Replace("##CertId##", objAvendaCert.CertId);
            Message = Message.Replace("##SerialNumber##", objAvendaCert.SerialNo);
            Message = Message.Replace("##SALESORDER##", objAvendaCert.so);
            Message = Message.Replace("##PURCHASEORDER##", objAvendaCert.po);
            Message = Message.Replace("##EXPIRYDATE##", objAvendaCert.expiryDate);
            Message = Message.Replace("##USERNAME##", objAvendaCert.username);
            Message = Message.Replace("##PASSWORD##", objAvendaCert.password);

            switch (objAvendaCert.brand)
            {
                case "ARUBA":
                    {
                        sub = "Aruba ClearPass Account Details: " + objAvendaCert.name + " - Sales Order: " + objAvendaCert.so;
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        sub = "Alcatel ClearPass Account Details: " + objAvendaCert.name + " - Sales Order: " + objAvendaCert.so;
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
            }

            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAvendaCert.email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objAvendaCert.email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendAvendaImportSubKeyDet", "Mail not sent:" + objAvendaCert.subscription);
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAvendaCert.email, "", bcc, sub, Message, "", "AVENDA_SUBSCRIPTION_IMPORT_MAIL");
            }

            return retVal;
        }

        //Added by Ashwini on Aug/02/2012

        public bool sendAmigopodImportSubKeyDet(AmigopodCertInfo objAmigopodCertInfo)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };

            string mailTemplateFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AMG_IMPSUB_MAIL_TEMPLATE"]);
            Message = File.ReadAllText(mailTemplateFile);
            Message = Message.Replace("##NAME##", objAmigopodCertInfo.Name);
            Message = Message.Replace("##SUBSCRIPTIONID##", objAmigopodCertInfo.Subscriptionkey);
            Message = Message.Replace("##CertId##", objAmigopodCertInfo.CertId);
            Message = Message.Replace("##SerialNumber##", objAmigopodCertInfo.SerialNumber);
            Message = Message.Replace("##SALESORDER##", objAmigopodCertInfo.SoId);
            Message = Message.Replace("##PURCHASEORDER##", objAmigopodCertInfo.PoId);
            Message = Message.Replace("##HASUBSCRIPTION##", objAmigopodCertInfo.HASubKey);
            Message = Message.Replace("##HACERTID##", objAmigopodCertInfo.HACertid);
            Message = Message.Replace("##HASERIALNO##", objAmigopodCertInfo.HACertSerialNo);
            Message = Message.Replace("##GuestLicenseCertid##", objAmigopodCertInfo.guestLicCertId);
            Message = Message.Replace("##GuestLicenseSerialNo##", objAmigopodCertInfo.guestLicCertSerialNo);
            Message = Message.Replace("##OnBoardLicenseCertid##", objAmigopodCertInfo.onBoardCertId);
            Message = Message.Replace("##OnBoardLicenseSerialNo##", objAmigopodCertInfo.onBoardSerialNo);
            Message = Message.Replace("##AdvCertId##", objAmigopodCertInfo.AdvCertId);
            Message = Message.Replace("##AdvCertSerialNo##", objAmigopodCertInfo.AdvSerialNo); 
           
            switch (objAmigopodCertInfo.Brand)
            {
                case "ARUBA":
                    {
                        sub = "Aruba ClearPass Account Details: " + objAmigopodCertInfo.Name + " - Sales Order: " + objAmigopodCertInfo.SoId;
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        sub = "Alcatel ClearPass Account Details: " + objAmigopodCertInfo.Name + " - Sales Order: " + objAmigopodCertInfo.SoId;
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
            }

            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendAmigopodImportSubKeyDet", "Mail not sent:" + objAmigopodCertInfo.getString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", bcc, sub, Message, "", "AMIGOPOD_SUBSCRIPTION_IMPORT_MAIL");
            }

            return retVal;
        }


        //By Praveena - For Amigopod Integration
        public bool sendAmigopodSubscriptionInfo(AmigopodCertInfo objAmigopodCertInfo, string category, string username, string password)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };

            string mailTemplateFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AMIGOPOD_SUBSCRIPTION_MAIL_TEMPLATE"]);
            Message = File.ReadAllText(mailTemplateFile);            

            Message = Message.Replace("##UserName##", objAmigopodCertInfo.Name);
            Message = Message.Replace("##SUBSCRIPTIONID##", objAmigopodCertInfo.Subscriptionkey);
            Message = Message.Replace("##SALESORDER##", objAmigopodCertInfo.SoId);
            Message = Message.Replace("##PURCHASEORDER##", objAmigopodCertInfo.PoId);
            if (category == "Software")
            {
                Message = Message.Replace("##SoftwareCaption##", "Virtual Appliance Download Area");
                Message = Message.Replace("##url##", ConfigurationManager.AppSettings["AMG_URL"].ToString());
                Message = Message.Replace("##UserNameCaption##", "User Name");
                Message = Message.Replace("##deliniter##", ":");
                Message = Message.Replace("##usernameval##", username);

                Message = Message.Replace("##PasswordCaption##", "Password");
                Message = Message.Replace("##password##", password);

                Message = Message.Replace("##infoMessage##", "The Quick Start Guide is available on the download page and will step you through initial install.");
            }
            else
            {
                Message = Message.Replace("##SoftwareCaption##", "");
                Message = Message.Replace("##url##", "");

                Message = Message.Replace("##UserNameCaption##", "");
                Message = Message.Replace("##deliniter##", "");
                Message = Message.Replace("##usernameval##", "");

                Message = Message.Replace("##PasswordCaption##", "");
                Message = Message.Replace("##password##", "");

                Message = Message.Replace("##infoMessage##", "");
            }
            switch (objAmigopodCertInfo.Brand)
            {

                case "ARUBA":
                    {
                        sub = "Aruba ClearPass Account Details: " + objAmigopodCertInfo.Name + " - Sales Order: " + objAmigopodCertInfo.SoId;
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        sub = "Alcatel ClearPass Account Details: " + objAmigopodCertInfo.Name + " - Sales Order: " + objAmigopodCertInfo.SoId;
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
            }

            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", sub, Message, bcc); 
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendAmigopodSubscriptionInfo", "Mail not sent:" + objAmigopodCertInfo.getString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", bcc, sub, Message, "", "AMIGOPOD_SUBSCRIPTION_INFO_MAIL");
            }

            return retVal;
        }

        //By Praveena - For Amigopod Integration
        public bool sendAmigopodUpgradeInfo(AmigopodCertInfo objAmigopodCertInfo)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };

            string mailTemplateFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AMIGOPOD_UPGRADE_MAIL_TEMPLATE"]);
            Message = File.ReadAllText(mailTemplateFile);
            Message = Message.Replace("##UserName##", objAmigopodCertInfo.Name);
            Message = Message.Replace("##SUBSCRIPTIONID##", objAmigopodCertInfo.Subscriptionkey);
            Message = Message.Replace("##SALESORDER##", objAmigopodCertInfo.SoId);
            Message = Message.Replace("##PURCHASEORDER##", objAmigopodCertInfo.PoId);
            Message = Message.Replace("##CertId##", objAmigopodCertInfo.CertId);
            Message = Message.Replace("##PartId##", objAmigopodCertInfo.PartId);
            switch (objAmigopodCertInfo.Brand)
            {

                case "ARUBA":
                    {
                        sub = "Aruba ClearPass Upgrade Details: " + objAmigopodCertInfo.Name + " - Sales Order: " + objAmigopodCertInfo.SoId;
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        sub = "Alcatel ClearPass Upgrade Details: " + objAmigopodCertInfo.Name + " - Sales Order: " + objAmigopodCertInfo.SoId;
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
            }

            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendAmigopodSubscriptionInfo", "Mail not sent:" + objAmigopodCertInfo.getString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", bcc, sub, Message, "", "AMIGOPOD_UPGRADE_MAIL_INFO");
            }

            return retVal;
        }

        //By Praveena - For Amigopod Integration
        public void sendAmigopodErrorMessage(string message,string module,string certid,string subkey)
        {
            string Message = "Module : " + module +"</BR>";
            Message = Message + " Cert Id : " + certid + " ; Subscription : " + subkey + "</BR>";
            Message = Message + message;
            string sub = "";
            string[] fromInfo = { "", "" };
            fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');

            string to = ConfigurationManager.AppSettings["AMG_ERROR_MAIL_TO"].ToString();
            string cc = ConfigurationManager.AppSettings["AMG_ERROR_MAIL_CC"].ToString();

            sub = "Amigopod Licensing Exception";

            retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], to, cc, sub, Message, "");
        }

        //By Praveena - For Amigopod Integration
        //to send alert for Add HA 
        public bool sendAmigopodAddHaInfo(AmigopodCertInfo objAmigopodCertInfo, string category, string username, string password,string ha_sub_key)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };

            string mailTemplateFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AMIGOPOD_HA_MAIL_TEMPLATE"]);
            Message = File.ReadAllText(mailTemplateFile);
            Message = Message.Replace("##UserName##", objAmigopodCertInfo.Name);
            Message = Message.Replace("##SUBSCRIPTIONID##", objAmigopodCertInfo.Subscriptionkey);
            Message = Message.Replace("##HA_SUBSCRIPTIONID##", ha_sub_key);
            Message = Message.Replace("##SALESORDER##", objAmigopodCertInfo.SoId);
            switch (objAmigopodCertInfo.Brand)
            {
                case "ARUBA":
                    {
                        sub = "Aruba ClearPass Account Details: " + objAmigopodCertInfo.Name + " - Sales Order: " + objAmigopodCertInfo.SoId;
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        sub = "Alcatel ClearPass Account Details: " + objAmigopodCertInfo.Name + " - Sales Order: " + objAmigopodCertInfo.SoId;
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
            }

            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendAmigopodSubscriptionInfo", "Mail not sent:" + objAmigopodCertInfo.getString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAmigopodCertInfo.Email, "", bcc, sub, Message, "", "AMIGOPOD_SUBSCRIPTION_INFO_MAIL");
            }

            return retVal;
        }

        public bool sendClsSubscriptionInfo(AvendaCert objAvendaCert, string strBrand)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };

            UserInfo objUser = new UserInfo();            

            string mailTemplateFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CLS_SUBSCRIPTION_MAIL_TEMPLATE"]);
            Message = File.ReadAllText(mailTemplateFile);
            Message = Message.Replace("##CompanyName##", objAvendaCert.CompanyName);
            Message = Message.Replace("##Name##", objAvendaCert.name);
            Message = Message.Replace("##SUBSCRIPTIONID##", objAvendaCert.subscription);
            Message = Message.Replace("##SALESORDER##", objAvendaCert.so);
            Message = Message.Replace("##PURCHASEORDER##", objAvendaCert.po);
            Message = Message.Replace("##EXPIRY##", objAvendaCert.expiryDate);
            Message = Message.Replace("##LICENSE##", objAvendaCert.license);
            Message = Message.Replace("##VERSION##", objAvendaCert.version);
            if (objAvendaCert.category == "Software")
            {
                Message = Message.Replace("##SoftwareCaption##", "Virtual Appliance Download Area");
                Message = Message.Replace("##url##", ConfigurationManager.AppSettings["CLS_URL"].ToString());
                Message = Message.Replace("##UserNameCaption##", "User Name");
                Message = Message.Replace("##deliniter##", ":");
                Message = Message.Replace("##usernameval##", objAvendaCert.username);

                Message = Message.Replace("##PasswordCaption##", "Password");
                Message = Message.Replace("##password##", objAvendaCert.password);

                Message = Message.Replace("##infoMessage##", "");
            }
            else
            {
                Message = Message.Replace("##SoftwareCaption##", "");
                Message = Message.Replace("##url##", "");

                Message = Message.Replace("##UserNameCaption##", "");
                Message = Message.Replace("##deliniter##", "");
                Message = Message.Replace("##usernameval##", "");

                Message = Message.Replace("##PasswordCaption##", "");
                Message = Message.Replace("##password##", "");

                Message = Message.Replace("##infoMessage##", "");
            }
            switch (objAvendaCert.brand)
            {

                case "ARUBA":
                    {
                        sub = "Aruba ClearPass Account Details: " + objAvendaCert.CompanyName + " - Sales Order: " + objAvendaCert.so;
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        sub = "Alcatel ClearPass Account Details: " + objAvendaCert.CompanyName + " - Sales Order: " + objAvendaCert.so;
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
            }

            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAvendaCert.email, objUser.GetUserEmail(), sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objAvendaCert.email, objUser.GetUserEmail(), sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendClsSubscriptionInfo", "Mail not sent:" + objAvendaCert.email);
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAvendaCert.email, "", bcc, sub, Message, "", "CLS_SUBSCRIPTION_INFO_MAIL");
            }

            return retVal;
        }

        public bool sendClsEvalKey(AmigopodCertInfo objAmigopodCertinfo, string strBrand)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };

            UserInfo objUser = new UserInfo();

            string mailTemplateFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CLS_EVAL_MAIL_TEMPLATE"]);
            Message = File.ReadAllText(mailTemplateFile);
            Message = Message.Replace("##CompanyName##", objAmigopodCertinfo.CompanyName);
            Message = Message.Replace("##Name##", objAmigopodCertinfo.Name);
            Message = Message.Replace("##SUBSCRIPTIONID##", objAmigopodCertinfo.Subscriptionkey);
            Message = Message.Replace("##POLICYMGR##", objAmigopodCertinfo.PolicyLic);
            Message = Message.Replace("##ENTERPRISE##", objAmigopodCertinfo.EnterpriseLic);
            Message = Message.Replace("##EXPIRY##", objAmigopodCertinfo.ExpiryDate.ToString());

            Message = Message.Replace("##SoftwareCaption##", "Virtual Appliance Download Area");
            Message = Message.Replace("##url##", ConfigurationManager.AppSettings["CLS_URL"].ToString());
            Message = Message.Replace("##UserNameCaption##", "User Name");
            Message = Message.Replace("##deliniter##", ":");
            Message = Message.Replace("##usernameval##", objAmigopodCertinfo.UserName);

            Message = Message.Replace("##PasswordCaption##", "Password");
            Message = Message.Replace("##password##", objAmigopodCertinfo.Password);

            Message = Message.Replace("##infoMessage##", "");

            switch (objAmigopodCertinfo.Brand)
            {
                case "ARUBA":
                    {
                        sub = "Aruba ClearPass Evaluation key for: " + objAmigopodCertinfo.CompanyName;
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        sub = "Alcatel ClearPass Evaluation key for: " + objAmigopodCertinfo.CompanyName;
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
            }
            try
            {
                if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
                {
                    bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                    retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertinfo.Email, objUser.GetUserEmail(), sub, Message, bcc);
                }
                else
                {
                    retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertinfo.Email, objUser.GetUserEmail(), sub, Message, "");
                }

                if (!retVal)
                {
                    new Log().logSystemError("sendClsEvalKey", "Mail not sent:" + objAmigopodCertinfo.Email);
                }
                else
                {
                    new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAmigopodCertinfo.Email, "", bcc, sub, Message, "", "CLS_SUBSCRIPTION_INFO_MAIL");
                }

                return retVal;
            }
            catch (Exception ex)
            {
                retVal = false;
                new Log().logSystemError("sendClsEvalKey", "Mail not sent to:" + objAmigopodCertinfo.Email + " The exception is <BR>" + ex.ToString());
                return retVal;
            }

        }

        //Added by Ashwini on Jan 20/2014

        public bool sendClsQCPassword(AmigopodCertInfo objAmigopodCertinfo)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };

            UserInfo objUser = new UserInfo();

            string mailTemplateFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CLS_QCPWD_MAIL_TEMPLATE"]);
            Message = File.ReadAllText(mailTemplateFile);            
            Message = Message.Replace("##url##", ConfigurationManager.AppSettings["QC_URL"].ToString());            
            Message = Message.Replace("##Name##", objAmigopodCertinfo.Name);                                    
            Message = Message.Replace("##UserNameCaption##", "User Name");
            Message = Message.Replace("##deliniter##", ":");
            Message = Message.Replace("##usernameval##", objAmigopodCertinfo.UserName);
            Message = Message.Replace("##PasswordCaption##", "Password");
            Message = Message.Replace("##password##", objAmigopodCertinfo.Password);                      

            switch (objAmigopodCertinfo.Brand)
            {
                case "ARUBA":
                    {
                        sub = "Aruba ClearPass QuickConnect Login details for: " + objAmigopodCertinfo.Name;
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        sub = "Alcatel ClearPass QuickConnect login details for  " + objAmigopodCertinfo.Name;
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
            }
            try
            {
                if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
                {
                    bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                    retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertinfo.Email, objUser.GetUserEmail(), sub, Message, bcc);
                }
                else
                {
                    retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertinfo.Email, objUser.GetUserEmail(), sub, Message, "");
                }

                if (!retVal)
                {
                    new Log().logSystemError("sendClsQCPassword", "Mail not sent:" + objAmigopodCertinfo.Email);
                }
                else
                {
                    new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAmigopodCertinfo.Email, "", bcc, sub, Message, "", "CLS_QUICKCONNECT_INFO_MAIL");
                }

                return retVal;
            }
            catch (Exception ex)
            {
                retVal = false;
                new Log().logSystemError("sendClsQCPassword", "Mail not sent to:" + objAmigopodCertinfo.Email + " The exception is <BR>" + ex.ToString());
                return retVal;
            }
        }

        public bool sendClsQuickConnect(AmigopodCertInfo objAmigopodCertinfo)
        {
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };

            UserInfo objUser = new UserInfo();

            string mailTemplateFile = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CLS_QC_MAIL_TEMPLATE"]);
            Message = File.ReadAllText(mailTemplateFile);
            Message = Message.Replace("##SoftwareCaption##", "Below are your QuickConnect credentials");
            Message = Message.Replace("##url##", ConfigurationManager.AppSettings["QC_URL"].ToString());
            Message = Message.Replace("##CompanyName##", objAmigopodCertinfo.CompanyName);
            Message = Message.Replace("##Name##", objAmigopodCertinfo.Name);
            Message = Message.Replace("##EXPIRY##", objAmigopodCertinfo.ExpiryDate.ToString("yyyy-MM-dd"));
            Message = Message.Replace("##UserNameCaption##", "User Name");
            Message = Message.Replace("##deliniter##", ":");
            Message = Message.Replace("##usernameval##", objAmigopodCertinfo.UserName);
            Message = Message.Replace("##PasswordCaption##", "Password");
            Message = Message.Replace("##password##", objAmigopodCertinfo.Password);
            Message = Message.Replace("##LICENSECOUNT##", objAmigopodCertinfo.LicenseCount);
            Message = Message.Replace("##SALESORDER##", objAmigopodCertinfo.SoId);
            Message = Message.Replace("##PURCHASEORDER##", objAmigopodCertinfo.PoId);

            Message = Message.Replace("##infoMessage##", "");

            switch (objAmigopodCertinfo.Brand)
            {
                case "ARUBA":
                    {
                        sub = "Aruba ClearPass QuickConnect license for: " + objAmigopodCertinfo.CompanyName;
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        sub = "Alcatel ClearPass QuickConnect license for: " + objAmigopodCertinfo.CompanyName;
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
            }
            try
            {
                if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
                {
                    bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                    retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertinfo.Email, objUser.GetUserEmail(), sub, Message, bcc);
                }
                else
                {
                    retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objAmigopodCertinfo.Email, objUser.GetUserEmail(), sub, Message, "");
                }

                if (!retVal)
                {
                    new Log().logSystemError("sendClsQuickConnect", "Mail not sent:" + objAmigopodCertinfo.Email);
                }
                else
                {
                    new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objAmigopodCertinfo.Email, "", bcc, sub, Message, "", "CLS_QUICKCONNECT_INFO_MAIL");
                }

                return retVal;
            }
            catch (Exception ex)
            {
                retVal = false;
                new Log().logSystemError("sendClsQuickConnect", "Mail not sent to:" + objAmigopodCertinfo.Email + " The exception is <BR>" + ex.ToString());
                return retVal;
            }
        }

        public bool sendALEActivationInfo(ALECertInfo objALECertInfo)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            string Message = "";
            string sub = "";
            string[] fromInfo = { "", "" };

            switch (objALECertInfo.Brand)
            {
                case "ARUBA":
                    {
                        Message = " Thank you for your transaction " + objALECertInfo.Name + "(" + objALECertInfo.Email + ")" + ",<BR/><BR/>" +
                       " This email is your electronic confirmation of your Aruba Networks Software License Key generation as follows:<BR/>" +
                       "Aruba Part Number : " + objALECertInfo.Package + "<BR/>" +
                       "Description             : " + objALECertInfo.PackageDesc + "<BR/>" +
                       "System IP Address : " + objALECertInfo.IPAddress + "<BR/>" +                       
                       "License Key            : <BR>" + objALECertInfo.Activationkey.Replace("\n", "<BR>") + "<BR/><BR/>" +
                      "This License key can be used only on the Aruba Analytics and Location Engine with the above stated system IP Address to enable the required functionality.<BR/><BR/>" +
                       "To enable the software feature, please login (with Administrator access rights) to your ALE Server, copy including --- Begin ALE License Key --- through  --- End ALE License Key --- & paste the key into the \"License Upload\" section under the \"License\" tab of ALE's Web Interface <BR><BR>" + "Should you experience difficulties, please contact Aruba Networks Customer Support at:<BR/>" +
                       "Email:      support@arubanetworks.com <BR/>" +
                       "Telephone:  1-800-943-4526 (USA) <BR/>" +
                        "                 1-408-754-1201 (International) <BR/><BR/>" +
                        "Thank you for being an Aruba Networks customer.<BR/>" +
                        "Sent by Aruba Networks License Manager<BR/>" +
                        "http://www.arubanetworks.com  <BR/>";

                        sub = "Aruba License Activation Info";
                        fromInfo = ConfigurationManager.AppSettings["ARUBA_FROM_EMAIL"].ToString().Split('|');
                        break;
                    }
                case "ALCATEL":
                    {
                        Message = " Thank you for your transaction " + objALECertInfo.Name + "(" + objALECertInfo.Email + ")" + ",<BR/><BR/>" +
                          " This email is your electronic confirmation of your Alcatel OmniAccess Software License Key generation as follows:<BR/>" +
                          "Alcatel Part Number : " + objALECertInfo.Package + "<BR/>" +
                          "Description             : " + objALECertInfo.PackageDesc + "<BR/>" +
                          "System IP Address : " + objALECertInfo.IPAddress + "<BR/>" +
                          "License Key            : <BR>" + objALECertInfo.Activationkey.Replace("\n", "<BR>") + "<BR/><BR/>" +
                         "This License key can be used only on the Alcatel ALE Wireless Management Suite with the above stated system IP Address to enable the required functionality.<BR/><BR/>" +
                         "To enable the software feature, please login (with Administrator access rights) to your ALE Server, copy & paste the key into the \"Add New License Key\" section inside the switch WEB UI and click on \"Add\".<BR/>" +
                            "Should you experience difficulties, please contact Alcatel OmniAccess Licensing Customer Support at:<BR/>" +
                           "Email:      support@ind.alcatel.com<BR/>" +
                            "Telephone:  1-800-995-2696  (North America)<BR/>" +
                            "                  1-877-919-9526  (Latin America) <BR/>" +
                            "                  +33-38-855-6929 (Europe) <BR/>" +
                            "                  +65-6586-1555 / 1-818-878-4507 (Asia PAcific) <BR/><BR/>" +
                            "Thank you for being an Alcatel OmniAccess WLAN Licensing customer.<BR/>" +
                            "Sent by Alcatel OmniAccess Licensing Manager<BR/>" +
                            "http://www.alcatel.com/enterprise   <BR/>";
                        sub = "Alcatel OmniAccess License Activation Info";
                        fromInfo = ConfigurationManager.AppSettings["ALCATEL_FROM_EMAIL"].ToString().Split('|');

                        break;
                    }
            }

            //Added by Ashwini
            //bool retVal = objEmail.sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objLgm.Email, "", sub, Message, Bcc);
            if (ConfigurationManager.AppSettings["BCC_ENABLED"].ToString() == "true")
            {
                bcc = ConfigurationManager.AppSettings["BCC_MAIL"].ToString();
                retVal = sendMailFromToCcBccSubMsg(fromInfo[0], fromInfo[1], objALECertInfo.Email, "", sub, Message, bcc);
            }
            else
            {
                retVal = sendMailFromToCcSubMsg(fromInfo[0], fromInfo[1], objALECertInfo.Email, "", sub, Message, "");
            }

            if (!retVal)
            {
                new Log().logSystemError("sendPreparedAccountCreation", "Mail not sent:" + objALECertInfo.GetString());
            }
            else
            {
                new LogEmail().LogMailDetails(fromInfo[0], fromInfo[1], objALECertInfo.Email, "", bcc, sub, Message, "", "PREPARED_ACCOUNT_CREATION");
            }

            return retVal;
        }
    }

    public class AccountMailInfo
    {
        public string Email;
        public string Password;
        public string Brand;
        public string ActivationCode;
        public string Name;
        public string IPAddress;
        public bool IsCompanyAdmin;
        public string CompanyName;

        public string GetString()
        {
            return "Name:" + Name + "Email:" + Email + " Password:" + Password + " Brand:" + Brand + " ActivationCode:" + ActivationCode + " IP Address:" + IPAddress + "IsCompanyAdmin:" + IsCompanyAdmin + "CompanyName" + CompanyName;
        }

    }

    public class CertificateMailInfo
    {
        public string Email;
        public string CertId;
        public string CertPartId;
        public string CertPartDesc;
        public string SysSerialNumber;
        public string SysPartId;
        public string SysPartDesc;
        public string Brand;
        public string ActivationKey;
        public string Name;
        public string ArubaPartId;
        public string ArubaPartDesc;
        public string ArubaSerialNumber;
        public string CCEmail;
        public string Passphrase;

        public string GetString()
        {
            return "Name:" + Name + "Email:" + Email + " CertId:" + CertId + " Brand:" + Brand + " ActivationKey:" + ActivationKey + " CertPartId:" + CertPartId + " CertPartDesc:" + CertPartDesc + " SysSerialNumber:" + SysSerialNumber + " SysPartId:" + SysPartId + " SysPartDesc:" + SysPartDesc + " Aruba PartId:" + ArubaPartId + " Aruba Part Desc:" + ArubaPartDesc + " Aruba SerialNumber:" + ArubaSerialNumber + "CC Email" + CCEmail + "Passphrase" + Passphrase;
        }
    }

    public class AirwaveCertInfo
    {
        public string Email;
        public string CertId;
        public string Activationkey;
        public string Name;
        public string IPAddress;
        public string SerialNumber;
        public string Package;
        public string Brand;
        public string PackageDesc;
        public string ExpiryDate;
        public string Password;

        public string GetString()
        {
            return "Name:" + Name + "Email:" + Email + " CertId:" + CertId + " Brand:" + Brand + " ActivationKey:" + Activationkey + " SysSerialNumber:" + SerialNumber + " Description:" + PackageDesc + " Expires:" + ExpiryDate + "Password:" + Password;
        }
    }

    public class ALECertInfo
    {
        public string Email;
        public string CertId;
        public string Activationkey;
        public string Name;
        public string IPAddress;
        public string SerialNumber;
        public string Package;
        public string Brand;
        public string PackageDesc;
        public string ExpiryDate;
        public string Password;

        public string GetString()
        {
            return "Name:" + Name + "Email:" + Email + " CertId:" + CertId + " Brand:" + Brand + " ActivationKey:" + Activationkey + " SysSerialNumber:" + SerialNumber + " Description:" + PackageDesc + " Expires:" + ExpiryDate + "Password:" + Password;
        }
    }
}
