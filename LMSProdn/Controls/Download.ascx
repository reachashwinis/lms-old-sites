<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Download.ascx.cs" Inherits="Controls_Download" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table>
<tr>
<td style="width: 703px">
    <asp:GridView ID="GrdDownload" runat="server"
     Width="100%" EmptyDataText="No Documents to download" AutoGenerateColumns="False"
     CellSpacing="1" CellPadding="1" BorderWidth="0px" PageSize="30">
            <Columns>
             <asp:TemplateField>
           <ItemTemplate>
           <asp:LinkButton CssClass="btn"  ID="lnkDownload" runat="server" 
           CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Name") %>' 
           OnCommand="GotoSystem_OnCommand"   CommandName="DOWNLOAD"  Text="Download" />
           </ItemTemplate>
           </asp:TemplateField>
                <asp:BoundField DataField="name" HeaderText="Name"/>
                <asp:BoundField DataField="Description" HeaderText="Remarks"/>
                <asp:BoundField DataField="Size" HeaderText="Size"/>
            </Columns>
            <AlternatingRowStyle CssClass="AlternatingRowStyle" />
            <EmptyDataRowStyle CssClass="AlternatingRowStyle" />
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="HeaderStyle" HorizontalAlign="Center" />
            <PagerSettings Visible="False" />
            <RowStyle CssClass="RowStyle" />
    </asp:GridView>
</td></tr>
</table>
