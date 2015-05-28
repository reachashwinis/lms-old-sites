<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TransferCert.ascx.cs" Inherits="Controls_TransferCert" %>
<link href="../CSS/Style.css" rel="stylesheet" type="text/css" />
<table border="0"><tr><td>
<asp:Label ID="LblCaption" runat="server" Font-Bold="True" ForeColor="#0000C0" Text="Transfer certificate"
    Width="489px" CssClass="lblHeader" Font-Italic="True" Font-Size="Small"></asp:Label>
    </td></tr></table>
<asp:Wizard ID="wizActivate" runat="server" DisplayCancelButton="false" ActiveStepIndex="2" DisplaySideBar="False" >
<WizardSteps>
<asp:WizardStep AllowReturn="False" ID="wStep1" StepType="Start"  Title="Active Certificates" runat="server">
<table width="100%" cellpadding="0" cellspacing="0">
<tr style="color: Red;width: 100%;font-weight:bold"> <td>
*Note: Transfer of certificate is for RMA purposes only!"
</td></tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0">
<tr>
 <td align="left" style="width: 197px">
View: <asp:DropDownList ID="ddlCertVersion" runat="server" AutoPostBack="True" CssClass="ddlist" OnSelectedIndexChanged="ddlCertVersion_SelectedIndexChanged" Width="73px">
<asp:ListItem Value="PRE">Pre 5.0</asp:ListItem>
<asp:ListItem Selected="True" Value="POST">Post 5.0</asp:ListItem>
</asp:DropDownList>
</td>
<td colspan="2" align="right">
<asp:Panel ID="pnlFilterQuery" runat="server">
<asp:LinkButton CssClass="btn"   ID="lnkRemFilter"  runat="server" OnClick="lnkRemfilter_OnClick" Text="Remove Filter :"></asp:LinkButton>&nbsp;<asp:Label ID="lblFilter" runat="server" CssClass="btn"></asp:Label>
</asp:Panel>
<asp:Panel ID="pnlFilterParams" runat="server" DefaultButton="btnGo">
<asp:DropDownList ID="ddlColumns" runat="server" CssClass="ddlist"></asp:DropDownList>&nbsp;<asp:DropDownList ID="ddlOperators" runat="server" CssClass="ddlist"></asp:DropDownList>&nbsp;
<asp:TextBox ID="txtSearch" runat="server" MaxLength="75" Columns="100" CssClass="txt"></asp:TextBox>&nbsp;<asp:ImageButton ID="btnGo" AlternateText="Go" ImageUrl="~/Images/searchLens.gif" runat="server" OnClick="btnGo_OnClick" ValidationGroup="btnGo" ImageAlign="AbsBottom" />
<br /><asp:RequiredFieldValidator ID="rfvSearch" runat="server" ControlToValidate="txtSearch" ErrorMessage="Search text is empty" Display="Dynamic" ValidationGroup="btnGo"></asp:RequiredFieldValidator>
</asp:Panel>
</td>
</tr>
</table>
<table width="100%" cellpadding="0" cellspacing="0">
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
<asp:ImageButton id="btnLast" runat="server" ImageUrl="~/Images/navLast.gif" AlternateText="Go to the last page" CommandArgument="last" OnClick="PagerButtonClick" ImageAlign="AbsMiddle" EnableViewState="False" />
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
           <asp:LinkButton CssClass="btn"  ID="lnkTfrCert" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CertId") %>' OnCommand="GotoSystem_OnCommand"   CommandName="TRANSFERCERT"  Text="Transfer" />
           </ItemTemplate>
           </asp:TemplateField>
                <asp:BoundField DataField="PartId" HeaderText="Part Number" SortExpression="PartId" />
                <asp:BoundField DataField="PartDesc" HeaderText="Description" SortExpression="PartDesc" />
                <asp:BoundField DataField="SerialNum" HeaderText="Certificate ID" SortExpression="SerialNum" />
                <asp:BoundField DataField="Fru" HeaderText="System Serial Number" SortExpression="Fru"  />
                <asp:BoundField DataField="SystemPartId" HeaderText="System Part Number" SortExpression="SystemPartId"  />
                <asp:BoundField DataField="SystemDesc" HeaderText="System Description" SortExpression="SystemDesc" />
                <asp:BoundField DataField="location" HeaderText="System Location" SortExpression="location" />
                <asp:BoundField DataField="ActCode" HeaderText="Activation Key" SortExpression="ActCode" />
                <asp:BoundField DataField="ActivatedOn" HeaderText="Activated On" ReadOnly="True" SortExpression="ActivatedOn" />
            </Columns>
            <AlternatingRowStyle CssClass="AlternatingRowStyle" />
            <EmptyDataRowStyle CssClass="AlternatingRowStyle" />
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="HeaderStyle" HorizontalAlign="Center" />
            <PagerSettings Visible="False" />
            <RowStyle CssClass="RowStyle" />
        </asp:GridView>
        </td>
        </tr>
        </table>
        <asp:Label ID="lblCertErr"  CssClass="lblError" runat="server"></asp:Label>
        </asp:WizardStep>
<asp:WizardStep AllowReturn="False" ID="wStep2" StepType="Step" Title="Enter System Info" runat="server">
<table width="100%">
<tr><td>
    <asp:Label ID="LblCertInfo" runat="server" Width="497px" Font-Italic="True" ForeColor="Blue"></asp:Label>
</td></tr>
<tr style="width:100%">
<td colspan="2" align="left">
        <asp:GridView ID="GrdVw2" runat="server" AutoGenerateColumns="False" GridLines="None" 
         CellSpacing="1" CellPadding="1"
         Width="100%" BorderWidth="0px"
        PageSize="50">
            <Columns>
            <asp:TemplateField>
               <ItemTemplate>
                <asp:CheckBox CssClass="btn"  ID="chkCert" runat="server"/>
                </ItemTemplate>
           </asp:TemplateField>
                <asp:BoundField DataField="LicpartId" HeaderText="License P/N"/>
                <asp:BoundField DataField="LicPartDesc" HeaderText="Description"/>
                <asp:BoundField DataField="LicSN" HeaderText="Certificate ID"/>
                <asp:BoundField DataField="ActivationKey" HeaderText="Activation Key"/>
                <asp:BoundField DataField="LicError" HeaderText="Error/Warning" ItemStyle-ForeColor="red"/>
            </Columns>
            <AlternatingRowStyle CssClass="AlternatingRowStyle" />
            <EmptyDataRowStyle CssClass="AlternatingRowStyle" />
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="HeaderStyle" HorizontalAlign="Center" />
            <PagerSettings Visible="False" />
            <RowStyle CssClass="RowStyle" />
        </asp:GridView>
        </td>
        </tr>
        <tr> <td> <asp:Label ID="LblCertErr1"  CssClass="lblError" runat="server"></asp:Label></td></tr>
<tr>
<td>
<asp:Label ID="lblSystem" runat="server" Text="Serial Number" CssClass="lbl"></asp:Label>:
</td>
</tr>
<tr>
<td>
<asp:TextBox ID="txtSystem" runat="server" CssClass="txt" Columns="50" MaxLength="50" Width="295px"></asp:TextBox>
</td>
</tr>
<tr>
<td >
<asp:RequiredFieldValidator ID="rfvSystem" runat="server" ControlToValidate="txtSystem" ErrorMessage="System Serial Number/MAC cannot be empty" ValidationGroup="StepLast" CssClass="lblError"></asp:RequiredFieldValidator>
</td>
</tr>
<tr><td>
    <asp:HyperLink ID="HyperLink1" runat="server" Width="297px" NavigateUrl="~/Pages/HowToFindSlNo.aspx" Target="_blank">How do I find my serial number?</asp:HyperLink>
</td></tr>
<tr>
<td>
<asp:Button ID="btnTransfer" runat="server" Text="Transfer" ValidationGroup="StepLast" OnClick="TransferLic_Onclick"  CssClass="btn"/><br />
</td>
</tr>
<tr>
<td>
<asp:Label ID="lblSystemErr" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
<asp:Label ID="lblActKey" runat="server" ForeColor="Green" Font-Bold="True"></asp:Label>
</td>
</tr>
</table>
</asp:WizardStep>
    <asp:WizardStep ID="wStep3" runat="server" AllowReturn="False" StepType="Complete"
        Title="Activation Info">
        <table width="100%" cellpadding="0" cellspacing="0"  style="background-color:White">
        <tr><td style="color: #0000cc; font-style: italic; height: 94px; width: 1592px;">
        Congratulations, you have successfully transfered the following license(s) on the
            below system. These are your license key(s).<br />
        These keys are required to be applied to the system you generated these license for.<br /></td></tr>
                <tr>
<td style="width: 1592px">
<asp:Label ID="LblSysInfo" runat="server" Text="Serial Number" CssClass="lbl" Width="654px" Font-Bold="True" ForeColor="Blue"></asp:Label>
</td>
</tr>

<tr style="width:100%">
<td colspan="2" align="left" style="width: 487px; height: 133px;">
        <asp:GridView ID="Grdvw3" runat="server" AutoGenerateColumns="False" GridLines="None" 
         CellSpacing="1" CellPadding="1"
         Width="100%" BorderWidth="0px"
        PageSize="30" >
            <Columns>
                <asp:BoundField DataField="LicpartId" HeaderText="License P/N" />
                <asp:BoundField DataField="LicPartDesc" HeaderText="Description"/>
                <asp:BoundField DataField="LicSN" HeaderText="Certificate ID" />
                <asp:BoundField DataField="ActivationKey" HeaderText="Activation Key" />
            </Columns>
            <AlternatingRowStyle CssClass="AlternatingRowStyle" />
            <EmptyDataRowStyle CssClass="AlternatingRowStyle" />
            <FooterStyle CssClass="FooterStyle" />
            <HeaderStyle CssClass="HeaderStyle" HorizontalAlign="Center" />
            <PagerSettings Visible="False" />
            <RowStyle CssClass="RowStyle" />
        </asp:GridView>
        </td>
        </tr>
        <tr><td style="color: #0000cc; font-style: italic; height: 94px; width: 1592px;">Please print this page for your records.</td></tr>
        </table>
    </asp:WizardStep>
</WizardSteps>
<StartNavigationTemplate>
</StartNavigationTemplate>
<StepNavigationTemplate>
</StepNavigationTemplate>
<FinishNavigationTemplate>
</FinishNavigationTemplate>
</asp:Wizard>