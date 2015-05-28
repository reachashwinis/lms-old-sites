<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CompanyAccounts.ascx.cs" Inherits="Controls_CompanyAccounts" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border=0><tr><td>
<asp:Label ID="LblCaption" runat="server" CssClass="lblHeader" Font-Bold="True" ForeColor="#0000C0" Text="My company accounts"
    Width="298px" Font-Italic="True" Font-Size="Small"></asp:Label>
    </td></tr></table>
<table width="100%" cellpadding="0" cellspacing="0">
<tr>
<td colspan="2" align="right">
<asp:Panel ID="pnlFilterQuery" runat="server">
<asp:LinkButton CssClass="btn"   ID="lnkRemFilter"  runat="server" OnClick="lnkRemfilter_OnClick" Text="Remove Filter :"></asp:LinkButton>&nbsp;<asp:Label ID="lblFilter" runat="server" CssClass="btn"></asp:Label></asp:Panel>
<asp:Panel ID="pnlFilterParams" runat="server" DefaultButton="btnGo">
<asp:DropDownList ID="ddlColumns" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>&nbsp;<asp:DropDownList ID="ddlOperators" runat="server" AutoPostBack="false" CssClass="ddlist"></asp:DropDownList>&nbsp;
<asp:TextBox ID="txtSearch" runat="Server" MaxLength="50" Columns="40" CssClass="txt"></asp:TextBox>&nbsp;<asp:ImageButton ID="btnGo" AlternateText="Go" ImageUrl="~/Images/searchLens.gif" runat="server" OnClick="btnGo_OnClick" ValidationGroup="btnGo" ImageAlign="AbsBottom" />
<br /><asp:RequiredFieldValidator ID="rfvSearch" runat="server" ControlToValidate="txtSearch" ErrorMessage="Search text is empty" Display="dynamic" ValidationGroup="btnGo"></asp:RequiredFieldValidator></asp:Panel>
</td>
</tr>
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
<asp:ImageButton id="btnLast" runat="server" ImageUrl="~/Images/navLast.gif" AlternateText="Go to the last page" CommandArgument="last" OnClick="PagerButtonClick" ImageAlign="AbsMiddle" EnableViewState="false" />
</td>
</tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0"  style="background-color:White">
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
        PageSize="50 " AllowPaging="false" 
        PagerSettings-Visible="false" 
        OnSorting="gvPR_Sort" OnRowDataBound="ItemCellsUpdate" 
        EmptyDataText="No accounts found"
        EmptyDataRowStyle-CssClass="AlternatingRowStyle"
         >
            <Columns>
                <asp:BoundField DataField="firstname" HeaderText="First Name" SortExpression="firstname" />
               <asp:BoundField DataField="lastname" HeaderText="Last Name" SortExpression="lastname" /> 
                           <asp:TemplateField HeaderText="Email" SortExpression="Email">
            <ItemTemplate>
            <asp:LinkButton ID="lnkEmail" runat="server" OnCommand="lnkEmail_OnCommand"  CommandArgument='<%# DataBinder.Eval(Container.DataItem, "AcctId") %>' Text='<%# DataBinder.Eval(Container.DataItem, "Email") %>'></asp:LinkButton>
           </ItemTemplate>
            </asp:TemplateField>
                <asp:BoundField DataField="AccountType" HeaderText="Account Type"  SortExpression="AccountType" />
               <asp:BoundField DataField="Phone" HeaderText="Phone"  SortExpression="" /> 
               <asp:BoundField DataField="Company" HeaderText="User-entered company"  SortExpression="Company" /> 
               <asp:BoundField DataField="assigned_company" HeaderText="Aruba-assigned company"  SortExpression="assigned_company" /> 
               <asp:BoundField DataField="Brand" HeaderText="Brand"  SortExpression="Brand" /> 
               <asp:BoundField DataField="CreatedOn" HeaderText="Created On"  SortExpression="CreatedOn" /> 
               <asp:BoundField DataField="Status" HeaderText="Status"  SortExpression="Status" /> 
            </Columns>
        </asp:GridView>
        </td>
        </tr>
        </table>
        <asp:Label ID="lblErr" CssClass="lblError" runat="server"></asp:Label>