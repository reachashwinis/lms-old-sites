<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/LoginTemplate.master"
CodeFile="CSSProvisioning.aspx.cs" Inherits="CSSProvision_CSSProvisioning" Title="Licensing System | CSS Account" %>
<asp:Content ID="Content1" ContentPlaceHolderID="phLoginBody" Runat="Server">
<table width="100%" border="0">
 <tr style="width:100%">
        <td colspan="2"><asp:ValidationSummary ID="vsSubmit" runat="server" ValidationGroup="vgSubmit" HeaderText="Please correct the following errors:" 
        EnableClientScript="true" DisplayMode="BulletList" ShowMessageBox="false" ShowSummary="true" CssClass="lblError" />
               
       </td>        
    </tr>
<tr>
<td>
Company <Font Color="Red" size="2">*</Font>
</td>
<td>
<asp:TextBox ID="txtCompanyName" runat="server" MaxLength="50" Columns="60" CssClass="txt" TextMode="SingleLine"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" ControlToValidate="txtCompanyName" ValidationGroup="vgSubmit" ErrorMessage="Company Name is mandatory" Display="None"></asp:RequiredFieldValidator>
</td>
</tr>

<tr>
<td>
Customer Website URL <Font Color="Red" size="2">*</Font>
</td>
<td>
<asp:TextBox ID="txtWebsite" runat="server" MaxLength="100" Columns="60" CssClass="txt" TextMode="SingleLine"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvWebsite" runat="server" ControlToValidate="txtWebsite" ValidationGroup="vgSubmit" ErrorMessage="Website is mandatory" Display="None"></asp:RequiredFieldValidator>
</td>
</tr>
   <tr>
<td>
First Name <Font Color="Red" size="2">*</Font>
</td>
<td>
<asp:TextBox ID="txtFName" runat="server" MaxLength="50" Columns="60" CssClass="txt" TextMode="SingleLine"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvFName" runat="server" ControlToValidate="txtFName" ValidationGroup="vgSubmit" ErrorMessage="First Name is mandatory" Display="None"></asp:RequiredFieldValidator>
</td>
</tr>
   <tr>
<td>
Last Name <Font Color="Red" size="2">*</Font>
</td>
<td>
<asp:TextBox ID="txtLName" runat="server" MaxLength="50" Columns="60" CssClass="txt" TextMode="SingleLine"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvLName" runat="server" ControlToValidate="txtLName" ValidationGroup="vgSubmit" ErrorMessage="Last Name is mandatory" Display="None"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td>
Email <Font Color="Red" size="2">*</Font>
</td>
<td>
<asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Columns="60" CssClass="txt" TextMode="SingleLine"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ValidationGroup="vgSubmit" ErrorMessage=" Email is mandatory" Display="None"></asp:RequiredFieldValidator>
<asp:CustomValidator ID="cvDupDomain" runat="server" ErrorMessage="Request for Content Security Provisioning has already been placed" OnServerValidate="cvDupDomain_Validate" ValidationGroup="vgSubmit" Display="None"></asp:CustomValidator>
</td>
</tr> 
<tr>
<td>
City <Font Color="Red" size="2">*</Font>
</td>
<td>
<asp:TextBox ID="txtCity" runat="server" MaxLength="50" Columns="60" CssClass="txt" TextMode="SingleLine"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" ValidationGroup="vgSubmit" ErrorMessage="City is mandatory" Display="None"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td>
Country <Font Color="Red" size="2">*</Font>
</td>
<td>
<asp:DropDownList ID="ddlCountry" runat="server" CssClass="ddlist">
<asp:ListItem selected="true" value="">Please select</asp:ListItem>
                           <asp:ListItem value="Afghanistan">Afghanistan </asp:ListItem>
                           <asp:ListItem value="Åland Islands">Åland Islands </asp:ListItem>
                           <asp:ListItem value="Albania">Albania </asp:ListItem>
                           <asp:ListItem value="Algeria">Algeria </asp:ListItem>
                           <asp:ListItem value="American Samoa">American Samoa </asp:ListItem>
                           <asp:ListItem value="Andorra">Andorra </asp:ListItem>
                           <asp:ListItem value="Angola">Angola </asp:ListItem>
                           <asp:ListItem value="Anguilla">Anguilla </asp:ListItem>
                           <asp:ListItem value="Antarctica">Antarctica </asp:ListItem>
                           <asp:ListItem value="Antigua/Barbuda">Antigua/Barbuda </asp:ListItem>
                           <asp:ListItem value="Argentina">Argentina </asp:ListItem>
                           <asp:ListItem value="Armenia">Armenia </asp:ListItem>
                           <asp:ListItem value="Aruba">Aruba </asp:ListItem>
                           <asp:ListItem value="Australia">Australia </asp:ListItem>
                           <asp:ListItem value="Austria">Austria </asp:ListItem>
                           <asp:ListItem value="Azerbaidjan">Azerbaidjan </asp:ListItem>
                           <asp:ListItem value="Bahamas">Bahamas </asp:ListItem>
                           <asp:ListItem value="Bahrain">Bahrain </asp:ListItem>
                           <asp:ListItem value="Bangladesh">Bangladesh </asp:ListItem>
                           <asp:ListItem value="Barbados">Barbados </asp:ListItem>
                           <asp:ListItem value="Belarus">Belarus </asp:ListItem>
                           <asp:ListItem value="Belgium">Belgium </asp:ListItem>
                           <asp:ListItem value="Belize">Belize </asp:ListItem>
                           <asp:ListItem value="Benin">Benin </asp:ListItem>
                           <asp:ListItem value="Bermuda">Bermuda </asp:ListItem>
                           <asp:ListItem value="Bhutan">Bhutan </asp:ListItem>
                           <asp:ListItem value="Bolivia">Bolivia </asp:ListItem>
                           <asp:ListItem value="Bosnia-Herzegovina">Bosnia-Herzegovina </asp:ListItem>
                           <asp:ListItem value="Botswana">Botswana </asp:ListItem>
                           <asp:ListItem value="Bouvet Island">Bouvet Island </asp:ListItem>
                           <asp:ListItem value="Brazil">Brazil </asp:ListItem>
                           <asp:ListItem value="British Indian Ocean Territory">British Indian Ocean Territory </asp:ListItem>
                           <asp:ListItem value="Brunei Darussalam">Brunei Darussalam </asp:ListItem>
                           <asp:ListItem value="Bulgaria">Bulgaria </asp:ListItem>
                           <asp:ListItem value="Burkina Faso">Burkina Faso </asp:ListItem>
                           <asp:ListItem value="Burundi">Burundi </asp:ListItem>
                           <asp:ListItem value="Cambodia">Cambodia </asp:ListItem>
                           <asp:ListItem value="Cameroon">Cameroon </asp:ListItem>
                           <asp:ListItem value="Canada">Canada </asp:ListItem>
                           <asp:ListItem value="Cape Verde">Cape Verde </asp:ListItem>
                           <asp:ListItem value="Cayman Islands">Cayman Islands </asp:ListItem>
                           <asp:ListItem value="Central African Republic">Central African Republic </asp:ListItem>
                           <asp:ListItem value="Chad">Chad </asp:ListItem>
                           <asp:ListItem value="Chile">Chile </asp:ListItem>
                           <asp:ListItem value="China">China </asp:ListItem>
                           <asp:ListItem value="Christmas Island">Christmas Island </asp:ListItem>
                           <asp:ListItem value="Cocos (Keeling) Islands">Cocos (Keeling) Islands </asp:ListItem>
                           <asp:ListItem value="Colombia">Colombia </asp:ListItem>
                           <asp:ListItem value="Comoros">Comoros </asp:ListItem>
                           <asp:ListItem value="Congo">Congo </asp:ListItem>
                           <asp:ListItem value="Congo, the Democratic Republic ">Congo, the Democratic Republic </asp:ListItem>
                           <asp:ListItem value="Cook Islands">Cook Islands </asp:ListItem>
                           <asp:ListItem value="Costa Rica">Costa Rica </asp:ListItem>
                           <asp:ListItem value="Côte d'Ivoire">Côte d'Ivoire </asp:ListItem>
                           <asp:ListItem value="Croatia">Croatia </asp:ListItem>
                           <asp:ListItem value="Cuba">Cuba </asp:ListItem>
                           <asp:ListItem value="Cyprus">Cyprus </asp:ListItem>
                           <asp:ListItem value="Czech Republic">Czech Republic </asp:ListItem>
                           <asp:ListItem value="Denmark">Denmark </asp:ListItem>
                           <asp:ListItem value="Djibouti">Djibouti </asp:ListItem>
                           <asp:ListItem value="Dominica">Dominica </asp:ListItem>
                           <asp:ListItem value="Dominican Republic">Dominican Republic </asp:ListItem>
                           <asp:ListItem value="East Timor">East Timor </asp:ListItem>
                           <asp:ListItem value="Ecuador">Ecuador </asp:ListItem>
                           <asp:ListItem value="Egypt">Egypt </asp:ListItem>
                           <asp:ListItem value="El Salvador">El Salvador </asp:ListItem>
                           <asp:ListItem value="Equatorial Guinea">Equatorial Guinea </asp:ListItem>
                           <asp:ListItem value="Eritrea">Eritrea </asp:ListItem>
                           <asp:ListItem value="Estonia">Estonia </asp:ListItem>
                           <asp:ListItem value="Ethiopia">Ethiopia </asp:ListItem>
                           <asp:ListItem value="Falkland Islands">Falkland Islands </asp:ListItem>
                           <asp:ListItem value="Faroe Islands">Faroe Islands </asp:ListItem>
                           <asp:ListItem value="Fiji">Fiji </asp:ListItem>
                           <asp:ListItem value="Finland">Finland </asp:ListItem>
                           <asp:ListItem value="Former Czechoslovakia">Former Czechoslovakia </asp:ListItem>
                           <asp:ListItem value="Former USSR">Former USSR </asp:ListItem>
                           <asp:ListItem value="France">France </asp:ListItem>
                           <asp:ListItem value="France (European Territory)">France (European Territory) </asp:ListItem>
                           <asp:ListItem value="French Guyana">French Guyana </asp:ListItem>
                           <asp:ListItem value="French Southern Territories">French Southern Territories </asp:ListItem>
                           <asp:ListItem value="Gabon">Gabon </asp:ListItem>
                           <asp:ListItem value="Gambia">Gambia </asp:ListItem>
                           <asp:ListItem value="Georgia">Georgia </asp:ListItem>
                           <asp:ListItem value="Germany">Germany </asp:ListItem>
                           <asp:ListItem value="Ghana">Ghana </asp:ListItem>
                           <asp:ListItem value="Gibraltar">Gibraltar </asp:ListItem>
                           <asp:ListItem value="Great Britain">Great Britain </asp:ListItem>
                           <asp:ListItem value="Greece">Greece </asp:ListItem>
                           <asp:ListItem value="Greenland">Greenland </asp:ListItem>
                           <asp:ListItem value="Grenada">Grenada </asp:ListItem>
                           <asp:ListItem value="Guadeloupe (French)">Guadeloupe (French) </asp:ListItem>
                           <asp:ListItem value="Guam (USA)">Guam (USA) </asp:ListItem>
                           <asp:ListItem value="Guatemala">Guatemala </asp:ListItem>
                           <asp:ListItem value="Guernsey">Guernsey </asp:ListItem>
                           <asp:ListItem value="Guinea">Guinea </asp:ListItem>
                           <asp:ListItem value="Guinea Bissau">Guinea Bissau </asp:ListItem>
                           <asp:ListItem value="Guyana">Guyana </asp:ListItem>
                           <asp:ListItem value="Haiti">Haiti </asp:ListItem>
                           <asp:ListItem value="Heard / McDonald Is">Heard / McDonald Is </asp:ListItem>
                           <asp:ListItem value="Honduras">Honduras </asp:ListItem>
                           <asp:ListItem value="HongKong">Hong Kong </asp:ListItem>
                           <asp:ListItem value="Hungary">Hungary </asp:ListItem>
                           <asp:ListItem value="Iceland">Iceland </asp:ListItem>
                           <asp:ListItem value="India">India </asp:ListItem>
                           <asp:ListItem value="Indonesia">Indonesia </asp:ListItem>
                           <asp:ListItem value="Iran">Iran </asp:ListItem>
                           <asp:ListItem value="Iraq">Iraq </asp:ListItem>
                           <asp:ListItem value="Ireland">Ireland </asp:ListItem>
                           <asp:ListItem value="Israel">Israel </asp:ListItem>
                           <asp:ListItem value="Italy">Italy </asp:ListItem>
                           <asp:ListItem value="Ivory Coast">Ivory Coast</asp:ListItem>
                           <asp:ListItem value="Jamaica">Jamaica </asp:ListItem>
                           <asp:ListItem value="Japan">Japan </asp:ListItem>
                           <asp:ListItem value="Jordan">Jordan </asp:ListItem>
                           <asp:ListItem value="Kazakhstan">Kazakhstan </asp:ListItem>
                           <asp:ListItem value="Kenya">Kenya </asp:ListItem>
                           <asp:ListItem value="Kiribati">Kiribati </asp:ListItem>
                           <asp:ListItem value="Kuwait">Kuwait </asp:ListItem>
                           <asp:ListItem value="Kyrgyzstan">Kyrgyzstan </asp:ListItem>
                           <asp:ListItem value="Kosovo">Kosovo</asp:ListItem>
                           <asp:ListItem value="Laos">Laos </asp:ListItem>
                           <asp:ListItem value="Latvia">Latvia </asp:ListItem>
                           <asp:ListItem value="Lebanon">Lebanon </asp:ListItem>
                           <asp:ListItem value="Lesotho">Lesotho </asp:ListItem>
                           <asp:ListItem value="Liberia">Liberia </asp:ListItem>
                           <asp:ListItem value="Libya">Libya </asp:ListItem>
                           <asp:ListItem value="Liechtenstein">Liechtenstein </asp:ListItem>
                           <asp:ListItem value="Lithuania">Lithuania </asp:ListItem>
                           <asp:ListItem value="Luxembourg">Luxembourg </asp:ListItem>
                           <asp:ListItem value="Macau">Macau </asp:ListItem>
                           <asp:ListItem value="Macedonia">Macedonia </asp:ListItem>
                           <asp:ListItem value="Madagascar">Madagascar </asp:ListItem>
                           <asp:ListItem value="Malawi">Malawi </asp:ListItem>
                           <asp:ListItem value="Malaysia">Malaysia </asp:ListItem>
                           <asp:ListItem value="Maldives">Maldives </asp:ListItem>
                           <asp:ListItem value="Mali">Mali </asp:ListItem>
                           <asp:ListItem value="Malta">Malta </asp:ListItem>
                           <asp:ListItem value="Marshall Islands">Marshall Islands </asp:ListItem>
                           <asp:ListItem value="Martinique (French)">Martinique (French) </asp:ListItem>
                           <asp:ListItem value="Mauritania">Mauritania </asp:ListItem>
                           <asp:ListItem value="Mauritius">Mauritius </asp:ListItem>
                           <asp:ListItem value="Mayotte">Mayotte </asp:ListItem>
                           <asp:ListItem value="Mexico">Mexico </asp:ListItem>
                           <asp:ListItem value="Micronesia">Micronesia </asp:ListItem>
                           <asp:ListItem value="Moldavia">Moldavia </asp:ListItem>
                           <asp:ListItem value="Monaco">Monaco </asp:ListItem>
                           <asp:ListItem value="Mongolia">Mongolia </asp:ListItem>
                           <asp:ListItem value="Montserrat">Montserrat </asp:ListItem>
                           <asp:ListItem value="Montenegro">Montenegro</asp:ListItem>
                           <asp:ListItem value="Morocco">Morocco </asp:ListItem>
                           <asp:ListItem value="Mozambique">Mozambique </asp:ListItem>
                           <asp:ListItem value="Myanmar">Myanmar </asp:ListItem>
                           <asp:ListItem value="Namibia">Namibia </asp:ListItem>
                           <asp:ListItem value="Nauru">Nauru </asp:ListItem>
                           <asp:ListItem value="Nepal">Nepal </asp:ListItem>
                           <asp:ListItem value="Netherlands">Netherlands </asp:ListItem>
                           <asp:ListItem value="Netherlands Antilles">Netherlands Antilles </asp:ListItem>
                           <asp:ListItem value="Neutral Zone">Neutral Zone </asp:ListItem>
                           <asp:ListItem value="New Caledonia (French)">New Caledonia (French) </asp:ListItem>
                           <asp:ListItem value="New Zealand">New Zealand </asp:ListItem>
                           <asp:ListItem value="Nicaragua">Nicaragua </asp:ListItem>
                           <asp:ListItem value="Niger">Niger </asp:ListItem>
                           <asp:ListItem value="Nigeria">Nigeria </asp:ListItem>
                           <asp:ListItem value="Niue">Niue </asp:ListItem>
                           <asp:ListItem value="Norfolk Island">Norfolk Island </asp:ListItem>
                           <asp:ListItem value="North Korea">North Korea </asp:ListItem>
                           <asp:ListItem value="Northern Mariana Is">Northern Mariana Is</asp:ListItem>
                           <asp:ListItem value="Norway">Norway </asp:ListItem>
                           <asp:ListItem value="Oman">Oman </asp:ListItem>
                           <asp:ListItem value="Pakistan">Pakistan </asp:ListItem>
                           <asp:ListItem value="Palau">Palau </asp:ListItem>
                           <asp:ListItem value="Panama">Panama </asp:ListItem>
                           <asp:ListItem value="Papua New Guinea">Papua New Guinea </asp:ListItem>
                           <asp:ListItem value="Paraguay">Paraguay </asp:ListItem>
                           <asp:ListItem value="Peru">Peru </asp:ListItem>
                           <asp:ListItem value="Philippines">Philippines </asp:ListItem>
                           <asp:ListItem value="Pitcairn Island">Pitcairn Island </asp:ListItem>
                           <asp:ListItem value="Poland">Poland </asp:ListItem>
                           <asp:ListItem value="Polynesia (French)">Polynesia (French) </asp:ListItem>
                           <asp:ListItem value="Portugal">Portugal </asp:ListItem>
                           <asp:ListItem value="Puerto Rico">Puerto Rico </asp:ListItem>
                           <asp:ListItem value="Qatar">Qatar </asp:ListItem>
                           <asp:ListItem value="Reunion (French)">Reunion (French) </asp:ListItem>
                           <asp:ListItem value="Romania">Romania </asp:ListItem>
                           <asp:ListItem value="Russian Federation">Russian Federation </asp:ListItem>
                           <asp:ListItem value="Rwanda">Rwanda </asp:ListItem>
                           <asp:ListItem value="Saint Helena">Saint Helena </asp:ListItem>
                           <asp:ListItem value="Saint Kitts &amp; Nevis Anguilla">Saint Kitts &amp; Nevis Anguilla </asp:ListItem>
                           <asp:ListItem value="Saint Lucia">Saint Lucia </asp:ListItem>
                           <asp:ListItem value="Saint Pierre and Miquelon">Saint Pierre and Miquelon </asp:ListItem>
                           <asp:ListItem value="Saint Tome and Principe">Saint Tome and Principe </asp:ListItem>
                           <asp:ListItem value="Saint Vincent &amp; Grenadines">Saint Vincent &amp; Grenadines </asp:ListItem>
                           <asp:ListItem value="Samoa">Samoa </asp:ListItem>
                           <asp:ListItem value="San Marino">San Marino </asp:ListItem>
                           <asp:ListItem value="Saudi Arabia">Saudi Arabia </asp:ListItem>
                           <asp:ListItem value="Senegal">Senegal </asp:ListItem>
                           <asp:ListItem value="Serbia">Serbia </asp:ListItem>
                           <asp:ListItem value="Seychelles">Seychelles </asp:ListItem>
                           <asp:ListItem value="Sierra Leone">Sierra Leone </asp:ListItem>
                           <asp:ListItem value="Singapore">Singapore </asp:ListItem>
                           <asp:ListItem value="Slovak Republic">Slovak Republic </asp:ListItem>
                           <asp:ListItem value="Slovenia">Slovenia </asp:ListItem>
                           <asp:ListItem value="Solomon Islands">Solomon Islands </asp:ListItem>
                           <asp:ListItem value="Somalia">Somalia </asp:ListItem>
                           <asp:ListItem value="South Africa">South Africa </asp:ListItem>
                           <asp:ListItem value="South Korea">South Korea </asp:ListItem>
                           <asp:ListItem value="Spain">Spain </asp:ListItem>
                           <asp:ListItem value="Sri Lanka">Sri Lanka </asp:ListItem>
                           <asp:ListItem value="Sudan">Sudan </asp:ListItem>
                           <asp:ListItem value="Suriname">Suriname </asp:ListItem>
                           <asp:ListItem value="Svalbard/Jan Mayen Is">Svalbard/Jan Mayen Is </asp:ListItem>
                           <asp:ListItem value="Swaziland">Swaziland </asp:ListItem>
                           <asp:ListItem value="Sweden">Sweden </asp:ListItem>
                           <asp:ListItem value="Switzerland">Switzerland </asp:ListItem>
                           <asp:ListItem value="Syrian Arab Republic">Syrian Arab Republic </asp:ListItem>
                           <asp:ListItem value="Tadjikistan">Tadjikistan </asp:ListItem>
                           <asp:ListItem value="Taiwan">Taiwan </asp:ListItem>
                           <asp:ListItem value="Tanzania">Tanzania </asp:ListItem>
                           <asp:ListItem value="Thailand">Thailand </asp:ListItem>
                           <asp:ListItem value="Timor-Leste">Timor-Leste </asp:ListItem>
                           <asp:ListItem value="Togo">Togo </asp:ListItem>
                           <asp:ListItem value="Tokelau">Tokelau </asp:ListItem>
                           <asp:ListItem value="Tonga">Tonga </asp:ListItem>
                           <asp:ListItem value="Trinidad and Tobago">Trinidad and Tobago </asp:ListItem>
                           <asp:ListItem value="Tunisia">Tunisia </asp:ListItem>
                           <asp:ListItem value="Turkey">Turkey </asp:ListItem>
                           <asp:ListItem value="Turkmenistan">Turkmenistan </asp:ListItem>
                           <asp:ListItem value="Turks / Caicos Is">Turks / Caicos Is </asp:ListItem>
                           <asp:ListItem value="Tuvalu">Tuvalu </asp:ListItem>
                           <asp:ListItem value="Uganda">Uganda </asp:ListItem>
                           <asp:ListItem value="Ukraine">Ukraine </asp:ListItem>
                           <asp:ListItem value="United Arab Emirates">United Arab Emirates </asp:ListItem>
                           <asp:ListItem value="United Kingdom">United Kingdom </asp:ListItem>
                           <asp:ListItem value="United States Minor Outlying Islands">United States Minor Outlying Islands</asp:ListItem>
                           <asp:ListItem value="USA">United States </asp:ListItem>
                           <asp:ListItem value="Uruguay">Uruguay </asp:ListItem>
                           <asp:ListItem value="Uzbekistan">Uzbekistan </asp:ListItem>
                           <asp:ListItem value="Vanuatu">Vanuatu </asp:ListItem>
                           <asp:ListItem value="Vatican City State">Vatican City State </asp:ListItem>
                           <asp:ListItem value="Venezuela">Venezuela </asp:ListItem>
                           <asp:ListItem value="Vietnam">Vietnam </asp:ListItem>
                           <asp:ListItem value="Virgin Islands (British)">Virgin Islands (British) </asp:ListItem>
                           <asp:ListItem value="Virgin Islands (USA)">Virgin Islands (USA) </asp:ListItem>
                           <asp:ListItem value="Wallis and Futuna Is">Wallis and Futuna Is</asp:ListItem>
                           <asp:ListItem value="Western Sahara">Western Sahara </asp:ListItem>
                           <asp:ListItem value="Yemen">Yemen </asp:ListItem>
                           <asp:ListItem value="Zaire">Zaire </asp:ListItem>
                           <asp:ListItem value="Zambia">Zambia </asp:ListItem>
                           <asp:ListItem value="Zimbabwe">Zimbabwe </asp:ListItem>
</asp:DropDownList>
<asp:RequiredFieldValidator ID="rfvCountry" runat="server" ControlToValidate="ddlCountry" ValidationGroup="vgSubmit" ErrorMessage="Country is mandatory" Display="None"></asp:RequiredFieldValidator>
</td>
</tr>

<tr>
<td>
No of Employees/Users  <Font Color="Red" size="2">*</Font>
</td>
<td>
<asp:TextBox ID="txtUsers" runat="server" MaxLength="50" Columns="60" CssClass="txt" TextMode="SingleLine"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvUsers" runat="server" ControlToValidate="txtUsers" ValidationGroup="vgSubmit" ErrorMessage="No of Employees/Users is mandatory" Display="None"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="revUsers" runat="server" ControlToValidate="txtUsers"  ErrorMessage="No of Employees/Users : Enter a non-zero positive number"  ValidationExpression="^\d+$" ValidationGroup="vgSubmit" Display="None"></asp:RegularExpressionValidator>
</td>
</tr>
<tr>
<td>
So Id <Font Color="Red" size="2">*</Font>
</td>
<td>
<asp:TextBox ID="txtSoId" runat="server" MaxLength="50" Columns="60" CssClass="txt" TextMode="SingleLine"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvSoId" runat="server" ControlToValidate="txtSoId" ValidationGroup="vgSubmit" ErrorMessage="So Id is Mandatory" Display="None"></asp:RequiredFieldValidator>
</td>
</tr>
<tr>
<td style="text-align:center">
<asp:Button ID="btnSubmit" runat="server" CssClass="btn" OnClick="btnSubmit_Click" Text="Submit" ToolTip="Send Evaluation Request" ValidationGroup="vgSubmit" /> 
</td>
</tr>
<tr>
<td>
&nbsp;
</td>
</tr>
<tr>
<td>
<Font Color="Green" size="2">
<asp:Label  ID="lblSuccess" runat="server" Text="Content Security Provising Request submitted. You will receive an email from Aruba License Management Website after the setup for Content Security is complete." CssClass="lbl" ></asp:Label>
</Font>
</td>
</tr>
</table>
<input type="hidden" id="hdnLeadSource" runat="server" value="Aruba_field_web" />
<input type="hidden" id="hdnLeadOwner" runat="server" value="Jeremy Carlson" />
<input type="hidden" id="hdnPartnerName" runat="server" value="Aruba Networks Inc." />
<input type="hidden" id="hdnPartnerType" runat="server" value="OEM Partner" />
<input type="hidden" id="hdnPartnerContactName" runat="server" value="Ash Chowdappa" />
<input type="hidden" id="hdnPartnerContactEmail" runat="server" value="achowdappa@arubanetworks.com" />
<input type="hidden" id="hdnPartnerPhone" runat="server" value="+14087548003" />
<input type="hidden" id="hdnLeadEmail" runat="server" value="arubahelp@Zscaler.com" />
</asp:Content>



