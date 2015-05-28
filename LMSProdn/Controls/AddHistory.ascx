<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddHistory.ascx.cs" Inherits="Controls_AddHistory" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border="0"><tr><td style="width: 413px">
<asp:Label ID="LblCaption" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="History Details"
    Width="410px" Font-Italic="True"></asp:Label>
    </td></tr></table>
<asp:Wizard ID="wizController" runat="server" ActiveStepIndex="1" Width="962px" DisplaySideBar="False" Height="254px" >
<WizardSteps>
<asp:WizardStep AllowReturn="False" ID="wStep1" StepType="Start"  Title="List My Controller" runat="server">
<table width="100%" cellpadding="0" cellspacing="0">
<tr>
<td colspan="2" align="right">
<asp:Panel ID="pnlFilterQuery" runat="server">
<asp:LinkButton CssClass="btn"   ID="lnkRemFilter"  runat="server" OnClick="lnkRemfilter_OnClick" Text="Remove Filter :"></asp:LinkButton>&nbsp;<asp:Label ID="lblFilter" runat="server" CssClass="btn"></asp:Label>
</asp:Panel>
<asp:Panel ID="pnlFilterParams" runat="server" DefaultButton="btnGo">
<asp:DropDownList ID="ddlColumns" runat="server" CssClass="ddlist"></asp:DropDownList>&nbsp;<asp:DropDownList ID="ddlOperators" runat="server" CssClass="ddlist"></asp:DropDownList>&nbsp;
<asp:TextBox ID="txtSearch" runat="server" MaxLength="75" Columns="55" CssClass="txt"></asp:TextBox>&nbsp;<asp:ImageButton ID="btnGo" AlternateText="Go" ImageUrl="~/Images/searchLens.gif" runat="server" OnClick="btnGo_OnClick" ValidationGroup="btnGo" ImageAlign="AbsBottom" />
<br /><asp:RequiredFieldValidator ID="rfvSearch" runat="server" ControlToValidate="txtSearch" ErrorMessage="Search text is empty" Display="Dynamic" ValidationGroup="btnGo"></asp:RequiredFieldValidator>
</asp:Panel>
</td>
</tr>
<tr style="width:100%">
<td class="paging" align="left">Total Records:&nbsp;<asp:label id="totalRecords" runat="server"></asp:label>&nbsp;|&nbsp;Page:&nbsp;<asp:label id="currentPage" runat="server"></asp:label>&nbsp;
of&nbsp;<asp:label id="totalPages" runat="server"></asp:label>&nbsp;&nbsp;<asp:LinkButton  ID="lnkClearSort" runat="server" Text="Clear Sort"  OnClick="lnkClearSort_Click"></asp:LinkButton>
</td>
<td class="paging" align="right">
<asp:imagebutton id="btnFirst" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navFirst.gif" CommandArgument="0" AlternateText="Go to the first page" EnableViewState="False"></asp:imagebutton>
<asp:LinkButton   ID="lnkFirst" runat="server" OnClick="linkButton_Click" CommandArgument="0" Text="first"></asp:LinkButton> |
<asp:imagebutton id="btnPrev" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navPrevious.gif" CommandArgument="prev" AlternateText="Previous page" EnableViewState="False"></asp:imagebutton>
<asp:LinkButton   ID="lnkPrev" runat="server" OnClick="linkButton_Click" CommandArgument="prev" Text="previous"></asp:LinkButton> | <asp:LinkButton  ID="lnlNext" runat="server" OnClick="linkButton_Click" CommandArgument="next" Text="next"></asp:LinkButton>
<asp:imagebutton id="btnNext" onclick="PagerButtonClick" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/navNext.gif" CommandArgument="next" AlternateText="Next page" EnableViewState="False"></asp:imagebutton>
| <asp:LinkButton  ID="lnlLast" runat="server" OnClick="linkButton_Click" CommandArgument="last" Text="last"></asp:LinkButton>
<asp:ImageButton id="btnLast" runat="server" ImageUrl="~/Images/navLast.gif" AlternateText="Go to the last page" CommandArgument="last" OnClick="PagerButtonClick" ImageAlign="AbsMiddle" EnableViewState="False" /> |
<asp:LinkButton  ID="lnkDload" runat="server" OnClick="lnkDload_OnClick" CommandArgument="all" Text="Download"></asp:LinkButton>

</td>
</tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0"  style="background-color:White">
<tr style="width:100%">
<td colspan="2" align="left">
        <asp:GridView ID="gvPR" runat="server" AutoGenerateColumns="False" GridLines="None" 
         CellSpacing="1" CellPadding="1"
         Width="100%" BorderWidth="0px"
        AllowSorting ="True"  
        PageSize="30" 
        OnSorting="gvPR_Sort" OnRowDataBound="ItemCellsUpdate" 
        EmptyDataText="No Certificates found">
            <Columns>
            <asp:TemplateField>
             <ItemTemplate>
           <asp:CheckBox ID="chkFru" runat="server" /><input type="hidden" id="hdnFruId" runat="server" 
           value='<%# DataBinder.Eval(Container.DataItem, "pk_cert_id") %>' />
           </ItemTemplate>
           </asp:TemplateField>
                <asp:BoundField DataField="serial_number" HeaderText="Serial No"/>
                <asp:BoundField DataField="part_id" HeaderText="Part Number" />
                <asp:BoundField DataField="part_desc" HeaderText="Part Desc"/>
                <asp:BoundField DataField="so_id" HeaderText="So ID"/>
                <asp:BoundField DataField="sold_to_cust" HeaderText="Sold To Cust"/>
                <asp:BoundField DataField="sold_to_name" HeaderText="Sold To Name"/>
                <asp:BoundField DataField="ship_to_cust" HeaderText="Ship To Cust"/>
                <asp:BoundField DataField="ship_to_name" HeaderText="Ship To Name"/>
                <asp:BoundField DataField="bill_to_cust" HeaderText="Bill To Cust"/>
                <asp:BoundField DataField="bill_to_name" HeaderText="Bill To Name"/>
                <asp:BoundField DataField="email" HeaderText="Added By"/>
                <asp:BoundField DataField="add_ts" HeaderText="Added On"/>
            </Columns>
            <AlternatingRowStyle CssClass="AlternatingRowStyle" />
            <EmptyDataRowStyle CssClass="AlternatingRowStyle" />
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="HeaderStyle" HorizontalAlign="Center" />
            <PagerSettings Visible="False" />
            <RowStyle CssClass="RowStyle" />
        </asp:GridView>
        </td> </tr>
        </table>
        <asp:Label ID="lblCertErr" runat="server"></asp:Label>
        <table><tr>
        <td><asp:Button ID="BtnUpdate" runat="server" Text="Update Location" 
        ValidationGroup="StepFirst" OnClick="Update_Onclick"  CssClass="btn" Width="150px"/><br />
        </td></tr></table>
        </asp:WizardStep>
<asp:WizardStep AllowReturn="False" ID="wStep2" StepType="Complete" Title="Enter System Info" runat="server">
<table width="100%" cellpadding="0" cellspacing="0"  style="background-color:White">
<tr style="width:100%">
<td colspan="2" align="left">
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" GridLines="None" 
         CellSpacing="1" CellPadding="1"
         Width="100%" BorderWidth="0px" PageSize="50">
            <Columns>
                <asp:BoundField DataField="serial_number" HeaderText="Serial No"/>
                <asp:BoundField DataField="part_id" HeaderText="Part Number" />
                <asp:BoundField DataField="part_desc" HeaderText="Part Desc"/>
                <asp:BoundField DataField="so_id" HeaderText="So ID"/>
                <asp:BoundField DataField="sold_to_cust" HeaderText="Sold To Cust"/>
                <asp:BoundField DataField="sold_to_name" HeaderText="Sold To Name"/>
                <asp:BoundField DataField="ship_to_cust" HeaderText="Ship To Cust"/>
                <asp:BoundField DataField="ship_to_name" HeaderText="Ship To Name"/>
                <asp:BoundField DataField="bill_to_cust" HeaderText="Bill To Cust"/>
                <asp:BoundField DataField="bill_to_name" HeaderText="Bill To Name"/>
                <asp:BoundField DataField="email" HeaderText="Added By"/>
                <asp:BoundField DataField="add_ts" HeaderText="Added On"/>
            <asp:TemplateField>
             <ItemTemplate>
                <asp:TextBox ID="TxtHistory" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "History") %>'/> 
            </ItemTemplate>
            </asp:TemplateField>
            </Columns>
            <AlternatingRowStyle CssClass="AlternatingRowStyle" />
            <EmptyDataRowStyle CssClass="AlternatingRowStyle" />
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="HeaderStyle" HorizontalAlign="Center" />
            <PagerSettings Visible="False" />
            <RowStyle CssClass="RowStyle" />
        </asp:GridView>
        </td> </tr>
<tr>
<td style="width: 353px">
<asp:Button ID="btnsubmit" runat="server" Text="Submit" ValidationGroup="StepLast" OnClick="Submit_Onclick"  CssClass="btn" Width="70px"/><br />
</td>
</tr>
<tr>
<td style="width: 353px; height: 21px">
<asp:Label ID="lblSystemErr" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
<asp:Label ID="lblSuccess" runat="server" ForeColor="Green" Font-Bold="True"></asp:Label>
</td>
</tr>
</table>
</asp:WizardStep>
</WizardSteps>
<StartNavigationTemplate>
</StartNavigationTemplate>
<StepNavigationTemplate>
</StepNavigationTemplate>
<FinishNavigationTemplate>
</FinishNavigationTemplate>
    <SideBarTemplate>
        <asp:DataList ID="SideBarList" runat="server">
            <ItemTemplate>
                <asp:LinkButton ID="SideBarButton" runat="server"></asp:LinkButton>
            </ItemTemplate>
            <SelectedItemStyle Font-Bold="True" />
        </asp:DataList>
    </SideBarTemplate>
</asp:Wizard>
