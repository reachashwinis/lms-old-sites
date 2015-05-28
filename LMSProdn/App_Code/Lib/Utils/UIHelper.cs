using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.ComponentModel;
using System.Data.Odbc;
using System.Text.RegularExpressions;


/// <summary>
/// Summary description for UIHelper
/// </summary>
namespace Com.Arubanetworks.Licensing.Lib.Utils
{
    public class UIHelper
    {
        public const string DATATXT = "TXT";
        public const string DATAVAL = "VAL";
        private const string PLEASESELTXT = "<Please Select>";
        private const string PLEASESELVAL = "";

        public const string BIT_TRUE = "TRUE";
        public const string BIT_FALSE = "FALSE";
        public const string EQ_SIGN = " = ";
        private const string LIKE_SIGN = " LIKE ";
        public const string SINGLE_QUOTE = "'";
        private const string PERCENT_SIGN = "%";
        public const string DB_AND = " AND ";

        public const string REQ_ERROR = "Error in Request";
        


        public UIHelper()
        {

        }

        public enum AssignDirection : int
        {
            DataColumn,
            Control
        }
        static public void SetDataColumnOrControl(
                UIHelper.AssignDirection setDirection,
                DataRow dr,
                String columeName,
                System.Web.UI.WebControls.WebControl ctl)
        {
            if (setDirection == AssignDirection.Control)
                SetControlFromDataColumn(dr, columeName, ctl);

            else if (setDirection == AssignDirection.DataColumn)
                SetDataColumnFromControl(dr, columeName, ctl);
        }

        static public void SetDataColumnOrControl(
              UIHelper.AssignDirection setDirection,
              DataRow dr,
              String columeName,
              System.Web.UI.HtmlControls.HtmlControl ctl)
        {
            if (setDirection == AssignDirection.Control)
                SetControlFromDataColumn(dr, columeName, ctl);

            else if (setDirection == AssignDirection.DataColumn)
                SetDataColumnFromControl(dr, columeName, ctl);
        }

        static public void SetControlFromDataColumn(
        DataRow dr,
        String columeName,
        System.Web.UI.WebControls.WebControl ctl)
        {
            //  string mn = "SetControlFromDataColumn";
            //Log.log(pn, mn, columeName +" --> " + ctl.ID);
            String t = ctl.GetType().ToString();
            if (t.Equals("System.Web.UI.WebControls.TextBox"))
            {
                ((TextBox)ctl).Text = dr[columeName].ToString().Trim();
            }
            else if (t.Equals("System.Web.UI.WebControls.Label"))
            {
                ((Label)ctl).Text = dr[columeName].ToString().Trim();
            }
            else if (t.Equals("System.Web.UI.WebControls.DropDownList")
               || t.Equals("System.Web.UI.WebControls.RadioButtonList")
                  )
            {
                ListControl ddl = (ListControl)ctl;
                ddl.ClearSelection();
                ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(dr[columeName].ToString()));
            }
            else if (t.Equals("System.Web.UI.WebControls.CheckBoxList")) //for multiselect
            {
                ListControl ddl = (ListControl)ctl;
                ddl.Items[ddl.Items.IndexOf(ddl.Items.FindByValue(dr[columeName].ToString()))].Selected=true;
            }
            else if (t.Equals("System.Web.UI.WebControls.CheckBox"))
            {
                ((CheckBox)ctl).Checked = false;
                if (dr[columeName].ToString().ToUpper().Equals(BIT_TRUE))
                    ((CheckBox)ctl).Checked = true;

            }
            else
            {
                //Log.log(pn, mn, "ERROR: Type unrecognized: " + t + " on " + ctl.ID);
            }
        }

        static public void SetControlFromDataColumn(
        DataRow dr,
        String columeName,
        System.Web.UI.HtmlControls.HtmlControl ctl)
        {
            //  string mn = "SetControlFromDataColumn";
            //Log.log(pn, mn, columeName +" --> " + ctl.ID);
            String t = ctl.GetType().ToString();
            if (t.Equals("System.Web.UI.HtmlControls.HtmlInputHidden"))
            {
                ((HtmlInputHidden)ctl).Value = dr[columeName].ToString().Trim();
            }
            else
            {
                //Log.log(pn, mn, "ERROR: Type unrecognized: " + t + " on " + ctl.ID);
            }
        }


        static public void SetDataColumnFromControl(DataRow dr,
            String columeName,
            System.Web.UI.WebControls.WebControl ctl)
        {

            SetDataColumnFromString(dr, columeName, GetControlValue(ctl));
        }

        static public void SetDataColumnFromControl(DataRow dr,
            String columeName,
            System.Web.UI.HtmlControls.HtmlControl ctl)
        {

            SetDataColumnFromString(dr, columeName, GetControlValue(ctl));
        }

        static public void SetDataColumnFromString(DataRow dr, String columeName, String val)
        {
            // Avoid error with when inserting empty string.
            if (val == null || val.Length == 0)
            {
                if (dr.Table.Columns[columeName].AllowDBNull == true)
                {

                    dr[columeName] = System.DBNull.Value;
                }
                else
                {
                    // Log.log(pn, "SetDataColumnFromString ", "cannot set " + columeName + " to DBNull because AllowDBNull=false");
                }
            }
            else
            {

                dr[columeName] = val;
            }
        }

        static public string PreventTabImageClickExcept(
        string CallerId,
        ControlCollection controlCollection,
        string tabId,
        string prefixImageId,
        string JavascriptMessage)
        {
            string targetImageId = prefixImageId + tabId;
            string controlType;

            IEnumerator childEnum = controlCollection.GetEnumerator();
            while (childEnum.MoveNext())
            {
                System.Web.UI.Control child = (System.Web.UI.Control)childEnum.Current;
                if (child == null || child.ID == null)
                    continue;

                controlType = child.GetType().ToString();
                if (controlType.IndexOf("System.Web.UI.WebControls.Image") >= 0)
                {
                    if (!child.ID.Equals(targetImageId))
                    {
                        Image myimage = (Image)child;
                        String js = String.Format("javascript:alert('{0}'); return false;", JavascriptMessage);
                        myimage.Attributes["onclick"] = js;
                    }
                }
            } //while childEnum.MoveNext()

            return "";
        }


        static public void SetDefaultValue(System.Web.UI.WebControls.WebControl ctl,string value)
        {
            string t = ctl.GetType().ToString();
            if (t.Equals("System.Web.UI.WebControls.DropDownList")
                   || t.Equals("System.Web.UI.WebControls.RadioButtonList")
                   )
            {
                ListControl ddl = (ListControl)ctl;
                ddl.ClearSelection();
                ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(value));
            }
            else if (t.Equals("System.Web.UI.WebControls.CheckBox"))
            {
                ((CheckBox)ctl).Checked = false;
                if (value.Equals(BIT_TRUE))
                    ((CheckBox)ctl).Checked = true;

            }
        }

        static public string GetControlValue(System.Web.UI.WebControls.WebControl ctl)
        {
            ///string mn = "GetControlValue";
            string val = "";
            String type = ctl.GetType().ToString();
            if (type.Equals("System.Web.UI.WebControls.TextBox"))
            {
                val = ((TextBox)ctl).Text.Trim();
            }
            else if (type.Equals("System.Web.UI.WebControls.Label"))
            {
                val = ((Label)ctl).Text;
            }
            else if (type.Equals("System.Web.UI.WebControls.DropDownList")
                || type.Equals("System.Web.UI.WebControls.RadioButtonList"))
            {
                ListControl ddl = (ListControl)ctl;
                if (ddl.SelectedIndex >= 0)
                {
                    val = ddl.SelectedItem.Value;
                }
            }
            else if (type.Equals("System.Web.UI.WebControls.CheckBoxList"))
            {
                ListControl cbl = (ListControl)ctl;
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    if (cbl.Items[i].Selected == true)
                    {
                        val += cbl.Items[i].Value + ",";
                    }
                }
                if (val.Trim().Length > 0)
                    val = val.Substring(0, val.Length - 1);
            }
            else if (type.Equals("System.Web.UI.WebControls.CheckBox"))
            {
                CheckBox chk = (CheckBox)ctl;
                if (chk.Checked == true)
                    val = BIT_TRUE;
                else
                    val = BIT_FALSE;
            }
            else
            {
                //  Log.log(pn, mn, "ERROR: Type unrecognized: " + type + " on " + ctl.ID);
                return "";
            }

            return val;
        }

        static public string GetControlValue(System.Web.UI.HtmlControls.HtmlControl ctl)
        {
            ///string mn = "GetControlValue";
            string val = "";
            String type = ctl.GetType().ToString();
            if (type.Equals("System.Web.UI.HtmlControls.HtmlInputHidden"))
            {
                val = ((HtmlInputHidden)ctl).Value.Trim();
            }
            else
            {
                //  Log.log(pn, mn, "ERROR: Type unrecognized: " + type + " on " + ctl.ID);
                return "";
            }

            return val;
        }

        public static void LoadControl(ListControl lstCtrl, DataSet ds, string strTblName, bool addPleaseSelectOption)
        {
            lstCtrl.DataSource = ds.Tables[strTblName];
            lstCtrl.DataTextField = DATATXT;
            lstCtrl.DataValueField = DATAVAL;
            lstCtrl.DataBind();
            if (addPleaseSelectOption)
            {
                lstCtrl.Items.Insert(0, new ListItem(PLEASESELTXT, PLEASESELVAL));
            }

        }

        public static string UrlPREdit(System.Web.HttpRequest Request, string recId)
        {
            return Request.ApplicationPath
                + "/Pages/createPR.aspx?tabId=PrDetails&pn="
                + recId;
        }

        public static string renUrlPREdit(System.Web.HttpRequest Request, string recId)
        {
            return Request.ApplicationPath
                + "/Pages/tabs.aspx?pn="
                + recId;
        }

        public static void PrepareAndBindLookupFromAllWithoutPlease(
                System.Web.UI.WebControls.ListControl lstCtrl,
             DataTable dtLookupList,
                String lookupType,
             String dataTextField,
                String dataValueField,
                bool addPleaseSelectOption)
        {
            DataView view = new DataView(dtLookupList);
            view.RowFilter = " LookupType = '" + lookupType + "' ";

            lstCtrl.DataSource = view;
            lstCtrl.DataValueField = dataValueField;
            lstCtrl.DataTextField = dataTextField;
            lstCtrl.DataBind();
            if (addPleaseSelectOption)
            {
                lstCtrl.Items.Insert(0, new ListItem(PLEASESELTXT, PLEASESELVAL));
            }
        }

        public static void RedirectToLoginPage(System.Web.UI.Page pg)
        {
            string strLoginPage = System.Configuration.ConfigurationManager.AppSettings["ASPXLOGINPAGE"];
            pg.Response.Redirect(strLoginPage, true);
        }

        public static string GetUserTime()
        {
            string msg = " UserId: " + System.Web.HttpContext.Current.Session["USER_ID"].ToString() + " Time: " + DateTime.Now.ToLongTimeString();
            return msg;
        }
        public static string GetSQLSafeLiteral(string strUnsafeString)
        { 
        return strUnsafeString.Replace("'","''");
        
        }

        public static string getValfromDDCtrl(GridView gv, int rowIndex, string ctrlId, string TextOrVal)
        {
            string retval = string.Empty;
            GridViewRow gr = gv.Rows[rowIndex];
            DropDownList ddl = (DropDownList)gr.FindControl(ctrlId);
            switch (TextOrVal)
            {
                case "TEXT":
                    retval = ddl.SelectedItem.Text;
                    if (retval.Equals(PLEASESELTXT))
                    {
                        retval = PLEASESELVAL;
                    }
                    break;
                case "VALUE":
                    retval = ddl.SelectedValue;
                    break;


            }
            return retval;

        }

        public static DataSet ConvertOdbcReaderToDataset(OdbcDataReader objOReader)
        {
            DataSet dataSet = new DataSet();
        
                // Create new data table

            DataTable schemaTable = objOReader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    // A query returning records was executed

                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        // Create a column name that is unique in the data table
                        string columnName = (string)dataRow["ColumnName"]; //+ "<C" + i + "/>";
                        // Add the column definition to the data table
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }

                    dataSet.Tables.Add(dataTable);

                    // Fill the data table we just created

                    while (objOReader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < objOReader.FieldCount; i++)
                            dataRow[i] = objOReader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    // No records were returned

                    dataSet = null;
                }
            
            
            return dataSet;
        
        }

        public static bool IsMacAddress(string input)
        {
            Regex objReg = new Regex(ConfigurationManager.AppSettings["MAC_FORMAT"]);
            return objReg.IsMatch(input, 0);
        
        }

        //public static string GetUserRole(Data.UserInfo objUser)
        //{
        //    if (objUser.IsImpersonate)
        //    {
        //        return objUser.ImpersonateUserRole;
        //    }
        //    return objUser.Role;
        //}

        //public static string GetUserEmail(Data.UserInfo objUser)
        //{
        //    if (objUser.IsImpersonate)
        //    {
        //        return objUser.ImpersonateEmail;
        //    }
        //    return objUser.Email;
        //}

        public  static DataTable GetTableFromStringArray(String[] arrString,Char Seperator)
        {
            DataTable dt = new DataTable();
            string[] temp;
            dt.Columns.Add("TXT");
            dt.Columns.Add("VAL");
            DataRow dr;
            for (int i = 0; i < arrString.Length; i++)
            {
                dr = dt.NewRow();
                temp = arrString[i].Split(Seperator);
                dr["TXT"] = temp[0];
                dr["VAL"] = temp[1];
                dt.Rows.Add(dr);
            
            }

            return dt;
        }

        public static void PrepareAndBindListWithoutPlease(
               System.Web.UI.WebControls.ListControl lstCtrl,
            DataTable dtLookupList,
            String dataTextField,
               String dataValueField,
               bool addPleaseSelectOption)
        {
            DataView view = new DataView(dtLookupList);
            lstCtrl.DataSource = view;
            lstCtrl.DataValueField = dataValueField;
            lstCtrl.DataTextField = dataTextField;
            lstCtrl.DataBind();
            if (addPleaseSelectOption)
            {
                lstCtrl.Items.Insert(0, new ListItem(PLEASESELTXT, PLEASESELVAL));
            }
        }

        public static string GetFilterSql(string colName, string Operator, string Search)
        {
            string retval = string.Empty;
            if (Search.Trim().Equals(string.Empty))
            {
                return string.Empty;
            }
            retval = colName;
            Search = UIHelper.GetSQLSafeLiteral(Search);
            switch (Operator)
            { 
                case "EQ":
                    retval += EQ_SIGN + SINGLE_QUOTE + Search + SINGLE_QUOTE;
                    break;
                case"CON":
                    retval += LIKE_SIGN + SINGLE_QUOTE + PERCENT_SIGN + Search + PERCENT_SIGN + SINGLE_QUOTE;
                    break;
                case"BEGW":
                    retval += LIKE_SIGN + SINGLE_QUOTE + Search + PERCENT_SIGN + SINGLE_QUOTE;
                    break;
                case"ENDW":
                    retval += LIKE_SIGN + SINGLE_QUOTE +PERCENT_SIGN+ Search + SINGLE_QUOTE;
                    break;
                default:
                    retval = string.Empty;
                    break;
            }
            return retval;
        
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

    }

}