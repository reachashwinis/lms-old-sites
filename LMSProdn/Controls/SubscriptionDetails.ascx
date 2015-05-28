<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SubscriptionDetails.ascx.cs" Inherits="Controls_SubscriptionDetails" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border="0" width="100%">
    <tr>
        <td style="width: 80%; height: 21px">
            <asp:Label ID="LblCaption" CssClass="lblHeader" runat="server" Font-Bold="True" ForeColor="#0000C0"
                 Width="472px" Font-Italic="True" Font-Size="Small"></asp:Label>
        </td>
        <td>
            <asp:LinkButton ID="lnkBtnBack" runat="server" OnClick="lnkBtnBack_Click" ><-Back</asp:LinkButton></td>
    </tr>
</table>
<table border="0" width="100%" cellpadding="1" cellspacing ="1" >
    <tr>
        <td id="colUpgrade" runat="server" align="left">
            <Asp:table ID="tblUpgrade" runat="server" CellPadding="1" CellSpacing="1" BorderStyle="NotSet" Width="40%" CssClass="AlternatingRowStyle" Font-Size="small">
               <%-- <Asp:TableHeaderRow BorderColor="Aquamarine">
                    <Asp:TableHeaderCell>Description</Asp:TableHeaderCell>
                    <Asp:TableHeaderCell>:</Asp:TableHeaderCell>
                    <Asp:TableHeaderCell>Value</Asp:TableHeaderCell>
                </Asp:TableHeaderRow>--%>
                <Asp:TableRow ID="rowSubscription" runat="server" >
                    <Asp:TableCell Font-Bold="true">Subscription ID</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell ID="cellSubscription" runat="server"></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow>
                    <Asp:TableCell Font-Bold="true">Purchase Order</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow>
                    <Asp:TableCell Font-Bold="true">Sales Order</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow>
                    <Asp:TableCell Font-Bold="true">Expire Time</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow>
                    <Asp:TableCell Font-Bold="true">License Count</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow>
                    <Asp:TableCell Font-Bold="true">Name</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow>
                    <Asp:TableCell Font-Bold="true">User Name</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow>
                    <Asp:TableCell Font-Bold="true">Password</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow>
                    <Asp:TableCell Font-Bold="true">Email</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow>
                    <Asp:TableCell Font-Bold="true">SMS Credit</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow>
                    <Asp:TableCell Font-Bold="true">SMS Handler</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow>
                    <Asp:TableCell Font-Bold="true">Create Time</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow Visible="false">
                    <Asp:TableCell Font-Bold="true">High Availability</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow>
                    <Asp:TableCell Font-Bold="true">Onboard License Count</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                <Asp:TableRow>
                    <Asp:TableCell Font-Bold="true">Advertising Feature</Asp:TableCell>
                    <Asp:TableCell>:</Asp:TableCell>
                    <Asp:TableCell></Asp:TableCell>
                </Asp:TableRow>
                
            </Asp:table>
        </td>
    </tr>
</table>