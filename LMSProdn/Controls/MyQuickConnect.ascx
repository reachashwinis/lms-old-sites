<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MyQuickConnect.ascx.cs" Inherits="Controls_MyQuickConnect" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
function switchViews(obj, row,imgObj) {
    var div = document.getElementById(obj);
    var img = document.getElementById(imgObj);
    if (div.style.display == "none") 
       {
            div.style.display = "inline";
            img.src="../Images/collation.png";
        } 
    else {
            div.style.display = "none";
            img.src="../Images/expansion.png";
        }
}
</script>
<table border="0" width="100%"><tr><td style="width: 117px">
<asp:Label ID="LblCaption" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="My QuickConnect certificates"
    Width="389px" CssClass="lblHeader" Font-Italic="True" Font-Size="Small"></asp:Label>
    </td>
</tr>
    </table>
<table width="100%" cellpadding="0" cellspacing="0" border="0">
<tr style="width:100%" align="right">
<td align="right">
<asp:Panel ID="pnlFilterQuery" runat="server">
<asp:LinkButton CssClass="btn"   ID="lnkRemFilter"  runat="server" OnClick="lnkRemfilter_OnClick" Text="Remove Filter :"></asp:LinkButton>&nbsp;<asp:Label ID="lblFilter" runat="server" CssClass="btn"></asp:Label></asp:Panel><asp:Panel ID="pnlFilterParams" runat="server" DefaultButton="btnGo">
<asp:DropDownList ID="ddlColumns" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>&nbsp;<asp:DropDownList ID="ddlOperators" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>&nbsp;
<asp:TextBox ID="txtSearch" runat="Server" MaxLength="75" Columns="100" CssClass="txt"></asp:TextBox>&nbsp;<asp:ImageButton ID="btnGo" AlternateText="Go" ImageUrl="~/Images/searchLens.gif" runat="server" OnClick="btnGo_OnClick" ValidationGroup="btnGo" ImageAlign="AbsBottom" /></asp:Panel>
</td>
</tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0" border="0">
<tr style="width:100%">
<td class="paging" align="left">Total Records:&nbsp;<asp:label id="totalRecords" runat="server"></asp:label>&nbsp;|&nbsp;Page:&nbsp;<asp:label id="currentPage" runat="server"></asp:label>&nbsp;
of&nbsp;<asp:label id="totalPages" runat="server"></asp:label>&nbsp;&nbsp;<asp:LinkButton  ID="lnkClearSort" runat="server" Text="Clear Sort"  OnClick="lnkClearSort_Click"></asp:LinkButton>
</td>
<td class="paging" align="right">
<asp:imagebutton id="btnFirst" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navFirst.gif" CommandArgument="0" AlternateText="Go to the first page" EnableViewState="false"></asp:imagebutton>
<asp:LinkButton   ID="lnkFirst" runat="server" OnClick="linkButton_Click" CommandArgument="0" Text="first"></asp:LinkButton> |
<asp:imagebutton id="btnPrev" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navPrevious.gif" CommandArgument="prev" AlternateText="Previous page" EnableViewState="false"></asp:imagebutton>
<asp:LinkButton   ID="lnkPrev" runat="server" OnClick="linkButton_Click" CommandArgument="prev" Text="previous"></asp:LinkButton> | <asp:LinkButton  ID="lnlNext" runat="server" OnClick="linkButton_Click" CommandArgument="next" Text="next"></asp:LinkButton>
<asp:imagebutton id="btnNext" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navNext.gif" CommandArgument="next" AlternateText="Next page" EnableViewState="false"></asp:imagebutton>
| <asp:LinkButton  ID="lnlLast" runat="server" OnClick="linkButton_Click" CommandArgument="last" Text="last"></asp:LinkButton>
<asp:ImageButton id="btnLast" runat="server" ImageUrl="~/Images/navLast.gif" AlternateText="Go to the last page" CommandArgument="last" OnClick="PagerButtonClick" ImageAlign="AbsMiddle" EnableViewState="false" />|
<asp:LinkButton  ID="lnkDload" runat="server" OnClick="lnkDload_OnClick" CommandArgument="all" Text="Download Text"></asp:LinkButton> |
<asp:LinkButton  ID="lnkDloadEx" runat="server" OnClick="lnkDloadEx_OnClick" CommandArgument="allEx" Text="Download Excel"></asp:LinkButton>
</td>
</tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0"  style="background-color:White" border="0">
<tr style="width:100%">
<td colspan="2" align="left">
        <asp:GridView ID="gvPR" runat="server" AutoGenerateColumns="False" GridLines="None" 
         CellSpacing="1" CellPadding="1"
         Width="100%" BorderWidth="0"
         HeaderStyle-CssClass="HeaderStyle"
         HeaderStyle-HorizontalAlign="Center"
        FooterStyle-CssClass="FooterStyle"
        RowStyle-CssClass="RowStyle"
        AlternatingRowStyle-CssClass="AlternatingRowStyle"
        AllowSorting ="true"  
        PageSize="30" AllowPaging="false" 
        PagerSettings-Visible="false" 
        OnSorting="gvPR_Sort" OnRowDataBound="gvPR_RowDataBound" 
        EmptyDataText="No Certificates found"
        EmptyDataRowStyle-CssClass="AlternatingRowStyle">
            <Columns>
             <asp:TemplateField HeaderText="Trans Details">
             <ItemTemplate>      
            <a id="Lnk<%# Eval("UserName") %>" href="javascript:switchViews('div<%# Eval("UserName") %>', 'one','img<%# Eval("UserName") %>');"> 
            <img id="img<%# Eval("UserName") %>" src="../Images/expansion.png" alt="Details"/></a>              
            </ItemTemplate>
            <ItemStyle Width="5%" />
            </asp:TemplateField>
                <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" ItemStyle-Width="5%"  />                
                <asp:TemplateField HeaderText="Send Password" ItemStyle-Width="5%" > 
                <ItemTemplate>
                <asp:LinkButton ID="lnkBtnDetails" runat="server" Text="**********" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "UserName") %>' OnCommand="GetDetails_OnCommand" ToolTip="Send my QuickConnect Credentials"/> </ItemTemplate>                       
                </asp:TemplateField>     
                <asp:BoundField DataField="CompanyName" HeaderText="Company Name" SortExpression="CompanyName" ItemStyle-Width="20%"  />  
                <asp:BoundField DataField="PartId" HeaderText="Part Number" SortExpression="PartId" ItemStyle-Width="5%" />
                <asp:BoundField DataField="PartDesc" HeaderText="Description" SortExpression="PartDesc" ItemStyle-Width="20%" />
                <asp:BoundField DataField="SerialNum" HeaderText="Certificate ID" SortExpression="SerialNum" ItemStyle-Width="10%" />        
                <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="ExpiryDate" ItemStyle-Width="15%" />
                <asp:BoundField DataField="UserCount" HeaderText="User Count" SortExpression="UserCount" ItemStyle-Width="5%" />     
                <asp:BoundField DataField="ActivatedOn" HeaderText="Activated On" SortExpression="ActivatedOn" ItemStyle-Width="10%" />    
                 <asp:TemplateField >
                        <ItemTemplate>
                                <tr width="100%">
                                    <td colspan="8" align="left">
                                    <div id="div<%# Eval("UserName") %>" style="display: none; position: relative; left: 6%;">
                                    <asp:GridView ID="grdDetails" runat="server" EmptyDataText="No details found" AutoGenerateColumns="False" Width="100%" CellSpacing="1" CellPadding="1" BorderWidth="0px" BorderStyle="None" GridLines="None" RowStyle-BackColor="lightgray" >
                <Columns>
                <asp:BoundField DataField="UserName" ItemStyle-Width="5%" />     
                <asp:BoundField DataField="Password" DataFormatString ="**********" ItemStyle-Width="5%"/>     
                <asp:BoundField DataField="CompanyName" ItemStyle-Width="23%"/> 
                <asp:BoundField DataField="PartId" ItemStyle-Wrap="false"/>
                <asp:BoundField DataField="PartDesc" ItemStyle-Width="23%"/>
                <asp:BoundField DataField="SerialNum" ItemStyle-Width="10%"/>          
                <asp:BoundField DataField="ExpiryDate" ItemStyle-Width="15%"/>
                <asp:BoundField DataField="UserCount" ItemStyle-Width="5%"/>     
                <asp:BoundField DataField="ActivatedOn" ItemStyle-Width="10%" />  
                </Columns>
                </asp:GridView>
               </div>
               </td>
               </tr>
               </ItemTemplate>
               </asp:TemplateField>    
            </Columns>
        </asp:GridView>
        </td>
        </tr>
        <tr><td style="color:Red; font-style:italic">
        <asp:Label ID="LblText" Visible="false" runat="server" Text="If you click the password it will be sent to the email on file."></asp:Label>
        </td></tr>
        </table>
