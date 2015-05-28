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
using System.Drawing;
using System.Text;

/// <summary>
/// Summary description for Sort
/// </summary>
namespace Com.Arubanetworks.Licensing.Lib.Utils
{
    public class Sort
    {
        // static declarations
        public static Char[] aSplitterChar = new Char[] { ',' };
        public static int EXPRESSIONS = 0;
        public static int ORDERS = 1;
        public static string TRUE_CASE = "0";
        public static string FALSE_CASE = "1";

        // constant variables
        public const String ORDER_DESC = " desc";
        public const String ORDER_ASC = " asc";
        public const String CONCAT_ORDER_DESC = "desc,";
        public const String CONCAT_ORDER_ASC = "asc,";
        private const String ASCENDING = "Ascending";
        private const String DESCENDING = "Descending";

        /**
         * Reads the current sorting fields and their orders (DESC/ASC) and
         * sets the new sorting fields and their orders. Internally,
         * the orders are reverted if you clicked on a sorted column. Information
         * is stored to and read from the DataGrid's Attributes repository.
         */
        public static void PrepareSortingString(String strNewSort, GridView gridView)
        {
            // Read current settings for fields and orders (DESC/ASC)
            String strSortExpressions = gridView.Attributes["SortingFields"];
            String strSortOrders = gridView.Attributes["SortingOrders"];
            String strNewSortExpressions = "";
            String strNewSortOrders = "";
            // Parse the new sorting string. This string can only be one of 
            // the strings read from the SortExpression attribute for datagrid's
            // columns. It has any of the forms: 
            // "FieldName", "FieldName DESC", "FieldName ASC",
            // "Field1,Field2 DESC", "Field1 ASC,Field2 DESC", "Field1,Field2".
            // The function splits on the comma, trims all strings and 
            // builds up the two string to store in the Attributes repository.

            // Separate field names from the information regarding the order.
            // Use strNewSort to build the order-less string of fields 
            ArrayList list = GetExpressionNOrders(strNewSort);
            ArrayList expressions = (ArrayList)list[EXPRESSIONS];
            ArrayList orders = (ArrayList)list[ORDERS];

            if (expressions != null && orders != null)
            {
                for (int j = 0; j < expressions.Count; j++)
                {
                    strNewSortExpressions += ((string)expressions[j]).Trim() + ",";
                    strNewSortOrders += ((string)orders[j]).Trim() + ",";
                }
            }
            strNewSortExpressions = strNewSortExpressions.Trim(aSplitterChar);
            strNewSortOrders = strNewSortOrders.Trim(aSplitterChar);
            // Compare current sorting fields with the new one. If it is 
            // the same, and we have sorted already on this fields, 
            // then ASC and DESC are inverted in strNewSortOrders.
            if (strSortExpressions == strNewSortExpressions && strSortOrders != "")
            {
                String[] aSortOrders = strSortOrders.Split(aSplitterChar);

                strNewSortOrders = "";
                for (int i = 0; i < aSortOrders.Length; i++)
                {
                    if (aSortOrders[i] == "desc")
                        strNewSortOrders += CONCAT_ORDER_ASC;
                    else
                        strNewSortOrders += CONCAT_ORDER_DESC;
                }
                strNewSortOrders = strNewSortOrders.Trim(aSplitterChar);
            }

            // Stores the sorting settings to the Attributes repository.
            // This information will be retrieved later when the datagrid rebinds
            gridView.Attributes["SortingFields"] = strNewSortExpressions;
            gridView.Attributes["SortingOrders"] = strNewSortOrders;
        }

        /**
         * Returns the string to sort the datagrid's content. The string is 
         * prepared using the information set by PrepareSortingString. It
         * matches the syntax recognized by the datagrid.
         */
        public static String ReadSortingString(GridView gridView)
        {
            StringBuilder returnString = new StringBuilder();
            StringBuilder strSortInfo = new StringBuilder();
            StringBuilder recordedExpression = new StringBuilder();

            // Read current settings for fields and orders (DESC/ASC)
            String strSortExpressions = gridView.Attributes["SortingFields"];
            String strSortOrders = gridView.Attributes["SortingOrders"];

            // Build the string to sort the datagrid			
            //Char[] aSplitterChar = new Char[] {','};
            String[] aSortExpressions = strSortExpressions.Split(aSplitterChar);
            String[] aSortOrders = strSortOrders.Split(aSplitterChar);

            for (int i = 0; i < aSortExpressions.Length; i++)
            {
                strSortInfo.Append(aSortExpressions[i]);
                strSortInfo.Append(" ");
                strSortInfo.Append(aSortOrders[i]);
                strSortInfo.Append(",");
            }
            //disable multiline sort
            //recordedExpression.Append(gridView.Attributes["RecordedExpression"]);
            //recordedExpression.Append(strSortInfo);
            returnString.Append(RemoveIfAlreadySorted(recordedExpression.ToString().Trim(aSplitterChar)));
            returnString.Append(strSortInfo.ToString().Trim());
            //disable multiline sort
           // gridView.Attributes["RecordedExpression"] = returnString.ToString();

            return returnString.ToString().Trim(aSplitterChar);
        }

        /**
         * Removes any already exisiting sort expression for the current requested column.
         * Last expression concatenated is the current requested string.
         * @param	sortString		Current sort string
         * @return		Newly created string.
         */
        private static string RemoveIfAlreadySorted(string sortString)
        {
            StringBuilder returnString = new StringBuilder();
            ArrayList list = GetExpressionNOrders(sortString);
            ArrayList expressions = (ArrayList)list[EXPRESSIONS];
            ArrayList orders = (ArrayList)list[ORDERS];

            int counter = expressions.Count;
            string lastExpression = ""; // wip.KRG : change the code to an iterator so that if an expression with multiple
            // column are involved, then lastExpression variable will be for muliple columns.
            if (counter > 0)
            {
                lastExpression = expressions[counter - 1].ToString();
            }
            for (int ii = 0; ii < counter; ii++)
            {
                if (!((expressions[ii].ToString().Trim()).Equals(lastExpression)))
                {
                    returnString.Append(expressions[ii]);
                    returnString.Append(" ");
                    returnString.Append(orders[ii]);
                    returnString.Append(",");
                }
            }
            return returnString.ToString().Trim();
        }

        /**
         * Returns the expressions and orders.
         * @param	recordedExpression		Current recorded expression
         * @return		Array list containing expressions and orders	
         */
        public static ArrayList GetExpressionNOrders(string recordedExpression)
        {
            ArrayList returnList = new ArrayList();
            ArrayList expressions = new ArrayList();
            ArrayList orders = new ArrayList();
            String[] sortExpressions = recordedExpression.Split(aSplitterChar);

            for (int i = 0; i < sortExpressions.Length; i++)
            {
                String tmp1 = sortExpressions[i].ToLower().Trim();
                int nPos = 0;
                string order = "";
                if (tmp1.EndsWith(ORDER_DESC))
                {
                    order = ORDER_DESC;
                    nPos = tmp1.LastIndexOf(ORDER_DESC);
                }
                else if (tmp1.EndsWith(ORDER_ASC))
                {
                    order = ORDER_ASC;
                    nPos = tmp1.LastIndexOf(ORDER_ASC);
                }
                else
                {
                    order = ORDER_ASC;
                    nPos = tmp1.Length;
                }
                string expression = tmp1.Substring(0, nPos).Trim();
                expressions.Add(expression);
                orders.Add(order);
            }
            returnList.Add(expressions);
            returnList.Add(orders);
            return returnList;
        }

        /**
         * Creates a descriptive string out of the provided sort string.
         * @param	sortString	String containing the expression and the orders.
         * @param	table		Contains the list of all the expressions and their user understable conversion.
         * @return	descriptive string.
         */
        public static string GetDescriptiveSortString(string sortString, Hashtable table)
        {
            StringBuilder returnString = new StringBuilder();
            System.Collections.IDictionaryEnumerator enumerator = null;
            if (table != null)
            {
                enumerator = table.GetEnumerator();
                String[] sortExpressions = sortString.Split(aSplitterChar);
                for (int i = 0; i < sortExpressions.Length; i++)
                {
                    enumerator.Reset();
                    returnString.Append(" | ");
                    String tmp1 = sortExpressions[i].ToLower().Trim();
                    int nPos = 0;
                    string order = "";
                    if (tmp1.EndsWith(ORDER_DESC))
                    {
                        order = DESCENDING;
                        nPos = tmp1.LastIndexOf(ORDER_DESC);
                    }
                    else if (tmp1.EndsWith(ORDER_ASC))
                    {
                        order = ASCENDING;
                        nPos = tmp1.LastIndexOf(ORDER_ASC);
                    }
                    string expression = tmp1.Substring(0, nPos).Trim();
                    if (expression != "")
                    {
                        while (enumerator.MoveNext())
                        {
                            if ((enumerator.Key).ToString().ToLower().Trim().Equals(expression))
                            {
                                String[] valueTokens = ((String)enumerator.Value).Split(aSplitterChar);
                                if (valueTokens.Length >= 0)
                                {
                                    returnString.Append(valueTokens[0]);
                                    returnString.Append(" SORTED AS : ");
                                    returnString.Append(order);
                                }
                            }
                        }
                    }
                }
            }
            return returnString.ToString();
        }

        /**
         * Returns the column numbers for all the given expressions sent as arraylist parameter.
         * @param	enumerator		Enumerator containing the Column names, column numbers of a
         *                          data grid defined in the datagrid's .ascx class
         * @param	expression		All the expressions for which column number is requested.
         * @returns		Arraylist containing the column numbers for the requested expressions.
         */
        public static ArrayList GetColumnNum(IDictionaryEnumerator enumerator, ArrayList expressions)
        {
            ArrayList columns = new ArrayList();
            for (int ii = 0; ii < expressions.Count; ii++)
            {
                enumerator.Reset();
                while (enumerator.MoveNext())
                {
                    if ((enumerator.Key).ToString().ToLower().Trim().Equals(((String)expressions[ii]).ToLower().Trim()))
                    {
                        String[] columnNum = new String[2];
                        columnNum = ((String)enumerator.Value).Split(Sort.aSplitterChar);
                        columns.Add(columnNum[1]);
                    }
                }
            }
            return columns;
        }

        /**
         * Returns a Label as icon for displaying it on the datagrid.
         * @returns		A label for display.
         */
        public static Label getSortIconLabel()
        {
           Label sortGlyph = new Label();
           // sortGlyph.Font.Name = "Webdings";
            sortGlyph.Font.Bold = true;
            sortGlyph.Font.Size = FontUnit.XSmall;
            sortGlyph.ForeColor = Color.Green;
            return sortGlyph;
        }

    }
}
