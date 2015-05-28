<%@ Control Language="C#" AutoEventWireup="true" CodeFile="QuickSearch.ascx.cs" Inherits="Controls_QuickSearch" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<asp:Panel ID="pnlDefault" runat="server" DefaultButton="btnSearch" Width="585px">
<table>
<tr>
<td colspan="2">
    <asp:Label ID="LblCaption" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Quick search"
        Width="125px" CssClass="lblHeader" Font-Italic="True" Font-Size="Small"></asp:Label></td>
</tr>
<tr>
<td><asp:Label ID="LblSearchBy" runat="server" Text="Search By:"></asp:Label>
</td>
<td>
<asp:DropDownList ID="ddlTerms" runat="server" AutoPostBack="True" CssClass="ddlist" Width="147px" OnSelectedIndexChanged="ddlTerms_SelectedIndexChanged">
                  <asp:ListItem Text="Certificate ID" Value="CERT"></asp:ListItem>  
                  <asp:ListItem Text="Sales Order #" Value="SOID"></asp:ListItem> 
                  <asp:ListItem Text="Cust PO #" Value="POID"></asp:ListItem> 
                  <asp:ListItem Text="Reseller/End User PO #" Value="ENDPOID"></asp:ListItem> 
                  <asp:ListItem Text="Controller Serial Number" Value="FRU"></asp:ListItem>
                  <asp:ListItem Text="Certificate Serial Number" Value="LCERT"></asp:ListItem>
</asp:DropDownList>
</td>
</tr>
<tr>
<td><asp:Label ID="LblSearch" runat="server" Text="Enter search critieria:"></asp:Label> 
</td>
<td>
 <asp:TextBox ID="txtSearch" runat="server" Columns="100" CssClass="txt" Width="314px"> </asp:TextBox>
</td>
</tr>
<tr>
   <td> <asp:LinkButton ID="LnkAdvSearch" runat="server" OnClick="LnkAdvSearch_Click">Advance Search</asp:LinkButton> </td>
</tr>
    <asp:Panel ID="PanelAdvSearch" runat="server" Visible="false">
    <tr><td>
        <asp:Label ID="LblSoldTo" runat="server" Text="Sold To:"></asp:Label></td>
        <td> 
            <asp:TextBox ID="TxtSoldTo" Columns="10" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
        <td>
            <asp:Label ID="LblSoldToName" runat="server" Text="Sold To Name:"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TxtSoldToName" Columns="50" runat="server"></asp:TextBox>
        </td>
        </tr>
        <tr>
        <td>
            <asp:Label ID="LblShipTo" runat="server" Text="Ship To:"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TxtShipTo" Columns="10" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
        <td>
            <asp:Label ID="LblShipToName" runat="server" Text="Ship To Name:"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TxtShipToName" Columns="50" runat="server"></asp:TextBox>
        </td>
        </tr>
        <tr>
        <td>
            <asp:Label ID="LblBillTo" runat="server" Text="Bill To:"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TxtBillTo" Columns="10" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
        <td>
            <asp:Label ID="LblBillToName" runat="server" Text="Bill To Name:"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="TxtBillToName" Columns="50" runat="server"></asp:TextBox>
        </td>
        </tr>
        <tr><td>
            <asp:Label ID="LblOrderDate" runat="server" Text="Order Date(mm/dd/yyyy):"></asp:Label></td>
            <td>
                <asp:TextBox ID="TxtOrderDate" runat="server"></asp:TextBox>
            </td>
        </tr>
     <tr>
    <td><asp:Label ID="LblFilterType" runat="server" Text="Filter Type:"></asp:Label></td>
    <td colspan="2">
        <asp:DropDownList ID="DdlFilterType" runat="server" CssClass="ddlist" AutoPostBack="true" OnSelectedIndexChanged="DdlFilterType_SelectedIndexChanged">
        <asp:ListItem Text="Any one of the above" Value="ANY" Selected="true"></asp:ListItem>
        <asp:ListItem Text="All of the above" Value="ALL"></asp:ListItem>
        </asp:DropDownList>
    </td>
    </tr>   
    <tr>
    <td>
        <asp:Label ID="LblHelp" Width="400px" runat="server" Font-Bold="true" Text="Gives the resultset based on any one of the entered filter criteria."></asp:Label>
    </td>
    </tr>
    </asp:Panel>
 <tr>
<td colspan="2">
<asp:CheckBox ID="chkCertsOnly" runat="Server" Text="Show Certificates Only" CssClass="ddlist" Enabled="False" />
    <asp:CheckBox ID="chkSOID" runat="server" Text="Show All Records of SO ID" CssClass="ddlist" />
    <asp:CheckBox ID="chkHistory" runat="server" Text="Show history records only" CssClass="ddlist" OnCheckedChanged="chkHistory_CheckedChanged" />   
    </td>
</tr>
<tr>
<td colspan="2">
<asp:Button ID="btnSearch" runat="Server" ValidationGroup="Search" OnClick="btnSearch_OnClick" CssClass="btn" Text="Search" />
    <asp:Label ID="LblSearchErr" runat="server" ForeColor="Red" Width="380px"></asp:Label></td>
</tr>
</table>
    <asp:Label ID="LblResult" runat="server" Font-Bold="True" ForeColor="Green" Visible="False"
        Width="434px"></asp:Label></asp:Panel>
<div id="divRes" runat="server">
<table width="100%" cellpadding="0" cellspacing="0">
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
        PageSize="50" AllowPaging="false" 
        PagerSettings-Visible="false" 
        OnSorting="gvPR_Sort" OnRowDataBound="ItemCellsUpdate" 
        EmptyDataText="No items found matching the search criteria"
        EmptyDataRowStyle-CssClass="AlternatingRowStyle">
            <Columns>
            <asp:TemplateField HeaderText="Send Mail">
           <ItemTemplate>
           <asp:CheckBox ID="chkCert" runat="server" />
           <input type="hidden" id="hdnCertId" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "pk_cert_id") %>' />
           </ItemTemplate>
           </asp:TemplateField>
                <asp:BoundField DataField="part_id" HeaderText="Part Number" SortExpression="part_id" />
                <asp:BoundField DataField="part_desc" HeaderText="Description" SortExpression="part_desc" />
                <asp:BoundField DataField="serial_number" HeaderText="Serial Number/Certificate ID" SortExpression="serial_number" />
                <asp:BoundField DataField="so_id" HeaderText="Sales Order#" SortExpression="so_id" />
                <asp:BoundField DataField="cust_po_id" HeaderText="Cust PO#" SortExpression="so_id" />
                <asp:BoundField DataField="end_user_po" HeaderText="End user/Reseller PO#" SortExpression="so_id" />
                <asp:BoundField DataField="ship_to_cust" HeaderText="Ship To Cust" SortExpression="ship_to_cust"  />
                <asp:BoundField DataField="sold_to_cust" HeaderText="Sold To Cust" SortExpression="sold_to_cust"  />
                <asp:BoundField DataField="bill_to_cust" HeaderText="Bill To Cust" SortExpression="bill_to_cust" />
                <asp:BoundField DataField="ship_to_name" HeaderText="Ship To Name" SortExpression="ship_to_name" />
                <asp:BoundField DataField="sold_to_name" HeaderText="Sold To Name" SortExpression="sold_to_name" />
                <asp:BoundField DataField="bill_to_name" HeaderText="Bill To Name" SortExpression="bill_to_name" />                
                <asp:BoundField DataField="location" HeaderText="System Location" SortExpression="location" />
                <asp:BoundField DataField="order_date" HeaderText="Order Date" SortExpression="order_date" />
                <asp:BoundField DataField="version" HeaderText="Version" SortExpression="version" />
            </Columns>
        </asp:GridView>
        </td>
        </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0">
        <tr><td style="width: 327px; height: 24px;">
           <asp:TextBox ID="TxtMail" runat="server" Width="324px"></asp:TextBox> </td>
           <td style="width: 90px; height: 24px;">
            <asp:Button ID="btnSendMail" runat="server" Text="Send Mail" OnClick="btnSendMail_Click" ValidationGroup="QuickSearch"/></td>
            <td style="height: 24px"><asp:CustomValidator ID="cvNoSelection" Display="dynamic" ValidationGroup="QuickSearch" ErrorMessage="No Certs were selected" runat="server" OnServerValidate="cvNoSelection_OnValidate" ></asp:CustomValidator>
            <asp:Label ID="lblErr" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        </table>

</div>
