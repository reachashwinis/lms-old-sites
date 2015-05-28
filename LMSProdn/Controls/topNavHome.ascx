<%@ Control Language="C#" AutoEventWireup="true" CodeFile="topNavHome.ascx.cs" Inherits="controls_topNavHome" %>
<table>
<tr><td><asp:Label ID="Label1" runat="server" Text="Label" ForeColor="#C00000" Width="239px"></asp:Label></td></tr></table>
<table cellpadding="0" cellspacing="0" border="0" width="100%">

	<tr>
		<td class="topMenu">
			<asp:DataList id="dlTopMenu" runat="server"
			    CellPadding="0"
				CellSpacing="0"
				RepeatDirection="Horizontal"
				ItemStyle-CssClass="topMenuItem"
				SeparatorStyle-CssClass="topMenuSeparator"
				OnItemDataBound="dlTopMenu_DataBound">
				<ItemTemplate>
				<asp:LinkButton ID="lnkMod" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ModuleTitle") %>'
						CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ModuleId")%>' OnClick="lnkMod_OnClick" CommandName="ChangeMod">
						</asp:LinkButton>				
				</ItemTemplate>	
				<SeparatorTemplate>
					<img id="Img1" src="../Images/MasterPage/images/topMenuSeparator.gif" alt="" border="0" runat="server"/>
				</SeparatorTemplate>
                <ItemStyle CssClass="topMenuItem" />
                <SeparatorStyle CssClass="topMenuSeparator" />
				
			</asp:DataList>
	</tr>
	<tr>
		<td class="subMenu">
			<asp:DataList id="dlSubMenu" runat="server"
				RepeatDirection="Horizontal"
				ItemStyle-CssClass="subMenuItem">
				<ItemTemplate>
					<asp:HyperLink ID="HyperLink1" runat="server" Target='<%# DataBinder.Eval(Container.DataItem, "Target") %>' 
						NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "NavigationUrl") %>' 
						Text='<%# DataBinder.Eval(Container.DataItem, "MenuTitle") %> ' CssClass="subMenuItem"/>
				</ItemTemplate>
				<SeparatorTemplate>
					<img id="Img1" src="../Images/MasterPage/images/topMenuSeparator.gif" alt="" border="0" runat="server"/>
				</SeparatorTemplate>
                <ItemStyle CssClass="subMenuItem" />
                <SelectedItemStyle BorderColor="#FF8080" ForeColor="#0000C0" />
			</asp:DataList>
		</td>
	</tr>	
</table>