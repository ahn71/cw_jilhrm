<%@ Page Title="Salary Entry Panel" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="payroll_entry_panel.aspx.cs" Inherits="SigmaERP.payroll.payroll_entry_panel" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <%--<script src="../scripts/jquery-1.8.2.js"></script>--%>

       <script type="text/javascript">

           var oldgridcolor;
           function SetMouseOver(element) {
               oldgridcolor = element.style.backgroundColor;
               element.style.backgroundColor = '#ffeb95';
               element.style.cursor = 'pointer';
               // element.style.textDecoration = 'underline';
           }
           function SetMouseOut(element) {
               element.style.backgroundColor = oldgridcolor;
               // element.style.textDecoration = 'none';

           }


</script>
    <style>
        
/*.gvdisplay1 th:nth-child(1) {
    text-align: center;
}*/
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dasboard</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="/payroll_default.aspx">Payroll</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li>  <a href="/payroll/salary_index.aspx">Salary</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                     <li> <a href="#" class="ds_negevation_inactive Pactive">Salary Entry Panel</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate>
        <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
    </ContentTemplate>
    </asp:UpdatePanel>
  
    
    
    <asp:HiddenField ID="hdfEmpId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdfUserType" Value="1" runat="server" ClientIDMode="Static" />
   

    <asp:HiddenField ID="hfTotlBasci" Value="1" runat="server" ClientIDMode="Static" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >        
        <ContentTemplate>          
        
   
   <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Salary Entry Panel</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">
                <input type="text" class="form-control" visible="false" id="txtFinding" runat="server" style="margin-left: 0px; width: 99%; text-align:center"  placeholder="Search by anythings" />               
                <div class="em_personal_info" id="divEmpPersonnelInfo" style="margin:0px">
                    
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <Triggers>
                            <%--  <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />--%>

                            <%--<asp:AsyncPostBackTrigger ControlID="chkSalaryCount" />

                     <asp:AsyncPostBackTrigger ControlID="chkPFMember" />
                      <asp:AsyncPostBackTrigger ControlID="btnSave" />         --%>
                           
                        </Triggers>
                        <ContentTemplate>

                            <asp:TabContainer ID="tc1" runat="server" CssClass="fancy fancy-green" AutoPostBack="true" OnActiveTabChanged="tc1_ActiveTabChanged" ActiveTabIndex="0">
                                <asp:TabPanel runat="server"  TabIndex="0" ID="tab1" HeaderText="Salary Entry">
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />                                                
                                                <asp:AsyncPostBackTrigger ControlID="chkPFMember" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" />   
                                                                                           
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:HiddenField ID="hfTotalHouseRent" Value="0" runat="server" ClientIDMode="Static" />                                       
                                                <asp:HiddenField ID="hdfSalaryType" Value="Gross" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hdfsaveupdatestatus" Value="Save" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hdfBasic" Value="0" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hdfMedical" Value="0" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hdfhouserent" Value="0" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hdfConveyance" Value="0" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hdfFoodAllowance" Value="0" runat="server" ClientIDMode="Static" />
                                                 <asp:HiddenField ID="hdfTechnical" Value="0" runat="server" ClientIDMode="Static" />
                                                 <asp:HiddenField ID="hdfOthers" Value="0" runat="server" ClientIDMode="Static" />
                                                 <asp:HiddenField ID="hdfPF" Value="0" runat="server" ClientIDMode="Static" />


                                                <asp:HiddenField ID="hfBasicStatus" Value="0" runat="server" ClientIDMode="Static" />                                        
                                                <asp:HiddenField ID="hfMedicalStatus" Value="Gross" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hfFoodStatus" Value="Save" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hfConveyanceStatus" Value="0" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hfTechnicalStatus" Value="0" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hfHouseStatus" Value="0" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hfOthersStatus" Value="0" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hfPFStatus" Value="0" runat="server" ClientIDMode="Static" />
                                                 <asp:HiddenField ID="hfIsGarments" Value="0" runat="server" ClientIDMode="Static" />
                                                 <asp:HiddenField ID="HiddenField10" Value="0" runat="server" ClientIDMode="Static" />
                                                 <asp:HiddenField ID="HiddenField11" Value="0" runat="server" ClientIDMode="Static" />

                                        <table class="em_personal_info_table">
                                            <tr>
                                                 <td>Company<span class="requerd1">*</span>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCompanyList" runat="server" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" >
                                            </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Card No<span class="requerd1">*</span>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlEmpCardNo" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" OnSelectedIndexChanged="ddlEmpCardNo_SelectedIndexChanged"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Emp Type
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID ="lblEmpType" style="font-weight:bold;color:red; background-color:yellow"></asp:Label>
                                                </td>
                                            </tr>
                                             <tr id="salarytype" runat="server" visible="true">

                                                <td>Salary Type
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                  <asp:RadioButtonList ID="rblSalaryType" runat="server" RepeatDirection="Horizontal" Enabled="false"  >
                                                      <asp:ListItem Value="Scale" Text="Scale" Selected="True" ></asp:ListItem>
                                                      <asp:ListItem Value="Gross" Text="Gross"></asp:ListItem>
                                                      <asp:ListItem Value="Gross Scale" Text="Gross Scale"></asp:ListItem>
                                                  </asp:RadioButtonList> 
                                                  
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Payment Type
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="chkPaymentType" runat="server" RepeatColumns="3" AutoPostBack="True" OnSelectedIndexChanged="chkPaymentType_SelectedIndexChanged" >
                                                        <asp:ListItem Value="0" Selected="True">Cash</asp:ListItem>
                                                        <asp:ListItem Value="1">Bank</asp:ListItem>
                                                        <asp:ListItem Value="2">Check</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>PF Member
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td><asp:CheckBox ID="chkPFMember" runat="server" Checked="true" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="chkPFMember_CheckedChanged" /></td>
                                                            
                                                            <td style="padding-left:61px;">Over Time</td>
                                                            <td>:</td>
                                                            <td> 
                                                                <asp:RadioButtonList ID="rblOverTime" runat="server" RepeatColumns="2" AutoPostBack="True" OnSelectedIndexChanged="chkPaymentType_SelectedIndexChanged">
                                                                    <asp:ListItem Value="1" Selected="True">Yes</asp:ListItem>
                                                                    <asp:ListItem Value="0">No</asp:ListItem>
                                                                </asp:RadioButtonList>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                    

                                                </td>
                                            </tr>
                                              <tr>
                                                <td>
                                                   Allow to Edit
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="ckbAllowtoEdit" runat="server" Checked="false" ClientIDMode="Static" AutoPostBack="true" OnCheckedChanged="ckbAllowtoEdit_CheckedChanged"/></td>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trBank">
                                                <td>Bank Name
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlBankList" ClientIDMode="Static" CssClass="form-control select_width"></asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr runat="server" id="trAccount">
                                                <td>Account No
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEmpAccNo" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>Grade
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:DropDownList ID="ddlGrade" runat="server" ClientIDMode="Static" CssClass="form-control select_width"></asp:DropDownList>
                                                </td>
                                            </tr>
                                           
                                            <tr id="trJoiningSalary" runat="server" visible="false">
                                                <td>Joining Salary
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtJoiningSalary" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width">0</asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trBasic">
                                                <td>Basic<asp:Label Visible="true" ID="lblBasic" Font-Bold="true" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtBasic" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" onKeyUp="SalaryCalculation();" AutoComplete="off" ></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr id="trMedical">
                                                <td>Medical <asp:Label Visible="true" ID="lblMedical" Font-Bold="true" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtMedical" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False">0</asp:TextBox>

                                                </td>
                                            </tr>
                                           <tr id="trFood" runat="server" visible="true">
                                                <td>Food<asp:Label Visible="true" ID="lblFood" Font-Bold="true" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtFoodAllowance" runat="server"  ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="false">0</asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr id="trConveyanceAllow" runat="server" visible="true">
                                                <td>Conveyance <asp:Label Visible="true" ID="lblConveyance" Font-Bold="true" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtConveyanceAllow" runat="server"  ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False">0</asp:TextBox>

                                                </td>
                                            </tr>

                                            
                                            <tr runat="server" id="tr1" visible="true">
                                                <td>Technical<asp:Label Visible="true" Font-Bold="true" ID="lblTechnical" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtTechnicalAllow"  runat="server" ClientIDMode="Static" Enabled="false" CssClass="form-control text_box_width">0</asp:TextBox>

                                                </td>
                                            </tr>

                                             <tr id="trHouserent">
                                                <td>House<asp:Label runat="server" Font-Bold="true" ID="lblHouseRent" Text="" ClientIDMode="Static"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtHouseRent" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False">0</asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="tr3">
                                                <td>Others<asp:Label runat="server" Font-Bold="true" ID="lblOthers" Text="" ClientIDMode="Static"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOthers" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False">0</asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="ha" runat="server" visible="false">
                                                <td>Holiday Allow.
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtHolidayAllow" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width">0</asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr id="ta" runat="server" visible="false">
                                                <td>Tiffin Allowance
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtTiffinAllowance" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width">0</asp:TextBox>

                                                </td>
                                            </tr>                                           
                                           
                                            <tr id="la" runat="server" visible="false">
                                                <td>
                                                    <p>Lunch Allowance</p>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtLunchAllowance" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2">0</asp:TextBox>
                                                    <asp:CheckBox ID="chkLunchCount" ClientIDMode="Static" CssClass="empchkLunch" Text="Lunch Count" Checked="false" runat="server" />

                                                </td>
                                            </tr>

                                            <tr runat="server" id="trPFAmount">
                                                <td>PF Amount<asp:Label ID="lblPF" Font-Bold="true" ClientIDMode="Static" Text="" runat="server">0</asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPFAmount" runat="server" ClientIDMode="Static" Width="188px" CssClass="form-control text_box_width_2" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trPfDate" visible="false">
                                                <td>
                                                    <asp:TextBox runat="server" ID="txtPFDate" ClientIDMode="Static"></asp:TextBox></td>
                                            </tr>
                                            <tr id="trel" runat="server" visible="false">
                                                <td>Earned Leave
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEarnedLeave" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width">0</asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trPresentSalary" runat="server">
                                                <td>Gross
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPresentSalary" runat="server" ClientIDMode="Static" onKeyUp="SalaryCalculation();" CssClass="form-control text_box_width" Enabled="false" Font-Bold="True" ForeColor="#009900">0</asp:TextBox>
                                                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers" 
                                TargetControlID="txtPresentSalary" ValidChars=""></asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                            
                                            <tr id="tresd" runat="server" visible="false">
                                                <td>El Start Date
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtElStart" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" Format="d-M-yyyy" runat="server" TargetControlID="txtElStart"></asp:CalendarExtender>
                                                </td>
                                            </tr>
                                            <tr id="bt" runat="server" visible="false">
                                                <td>Bonus Type
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:CheckBoxList ID="chkSlablist" RepeatColumns="2" runat="server">
                                                        <asp:ListItem Selected="False">Slab A</asp:ListItem>
                                                        <asp:ListItem Selected="False">Slab B</asp:ListItem>
                                                    </asp:CheckBoxList>

                                                </td>
                                            </tr>

                                               <tr id="Tr4" runat="server" >
                                                <td>TDS
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtTDS" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width">0</asp:TextBox>
                                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" 
                                TargetControlID="txtTDS" ValidChars=""></asp:FilteredTextBoxExtender>
                                                </td>
                                            </tr>

                                             <tr id="TrAttendanceBouns" runat="server" visible="false">
                                                <td>Atten. Bonus
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtAttenBonus" Enabled="false" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width">0</asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr id="trNightAllowance" runat="server" visible="true">
                                                <td>Night Allowance
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtNightAllowance" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width">0</asp:TextBox>

                                                </td>
                                            </tr>
                                             <tr id="tr2" runat="server" visible="False">
                                                <td>Dormitory Rent
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:DropDownList runat="server" ID="ddlDormitoryRent" ClientIDMode="Static" CssClass="form-control select_width">
                                                        <asp:ListItem Value="0" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Value="150">150</asp:ListItem>
                                                        <asp:ListItem Value="200">200</asp:ListItem>                                                       
                                                    </asp:DropDownList>

                                                </td>
                                            </tr>
                                        </table>
                                        <table class="em_button_table">
                                            <tr>

                                                <th>
                                                    <asp:Button ID="btnNew" Class="Pbutton" runat="server" Text="New" />
                                                </th>                                       

                                                <th>
                                                    <asp:Button ID="btnSave" CssClass="Pbutton" ClientIDMode="Static" runat="server" OnClientClick="return InputValidation();" Text="Submit" OnClick="btnSave_Click" />
                                                </th>

                                                <th>
                                                    <asp:Button ID="btnClose" Class="Pbutton" runat="server" Text="Close" PostBackUrl="~/personnel_defult.aspx" /></th>

                                            </tr>
                                        </table>
                                                </ContentTemplate>
                             </asp:UpdatePanel>
                                    </ContentTemplate>

                                </asp:TabPanel>
                                <asp:TabPanel runat="server" ID="tab2" TabIndex="1" HeaderText="Salary List">
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                                            <ContentTemplate>

                                          <table class="em_personal_info_table">
                                              <tr>
                                                 <td>Company
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCompanyList2" runat="server" OnSelectedIndexChanged="ddlCompanyList2_SelectedIndexChanged" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" >
                                            </asp:DropDownList>
                                                </td>
                                            </tr>
                                          </table>
                                        <asp:GridView runat="server" ID="gvSalaryList" CssClass="gvdisplay1" DataKeyNames="SN" AutoGenerateColumns="false" HeaderStyle-BackColor="#ffa500" HeaderStyle-Height="28px" HeaderStyle-ForeColor="White" PageSize="25" Width="100%"  OnRowCommand="gvSalaryList_RowCommand" OnRowDataBound="gvSalaryList_RowDataBound" >
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>SL</HeaderTemplate>
                                                    <ItemTemplate >
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" ItemStyle-HorizontalAlign="Center" />
                                                 <asp:BoundField DataField="EmpName" HeaderStyle-HorizontalAlign="Left" HeaderText="Name" />
                                                 <asp:BoundField DataField="BasicSalary" HeaderText="Basic" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="MedicalAllownce" HeaderText="Medical" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="HouseRent" HeaderText="House" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="PFAmount" HeaderText="PF.Amount" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="EmpPresentSalary" HeaderText="Gross" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="SalaryCount" HeaderText="S.Count" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:TemplateField HeaderText="Change"  HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                  <ItemTemplate >
                                      <asp:Button ID="btnEdit" runat="server" CommandName="Alter" Width="55px" Height="30px" Font-Bold="true" ForeColor="green" Text="Edit" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                  </ItemTemplate>
                              </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                                  </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>

                                </asp:TabPanel>
                            </asp:TabContainer>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                   

                </div>
            </div>
        </div>
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    <script type="text/javascript">
       
        $(document).ready(function () {            
            //$("#ddlBankList").select2();
            $(document).on("keyup", '.form-control', function () {
                searchTable($(this).val(), 'ContentPlaceHolder1_ContentPlaceHolder1_tc1_tab2_gvSalaryList', '');
            });
            $(document).on("keypress", "body", function (e) {
                if (e.keyCode == 13) e.preventDefault();
                // alert('deafault prevented');

            });
            $("#ddlEmpCardNo").select2();
        });   
        
        function load() {
            $("#ddlEmpCardNo").select2();
            $("#ddlBankList").select2();            
        }
        function Messageshow(messagetype,message)
        {
            showMessage(message, messagetype);
            load();
        }
        function goToNewTab(url)
        {
            $("#ddlEmpCardNo").select2();
            $("#ddlBankList").select2();
            window.open(url);
        }
        

        function SalaryCalculation()
        {
            try
            {
                var basic = 0;
                var medical = 0;
                var food = 0;
                var convayance = 0;
                var technical = 0;
                var houseRent = 0;
                var others = 0;
                var pf = 0;

               
              
                var salary_type = $('#<%=hdfSalaryType.ClientID%>').val().trim();
                if ($('#ckbAllowtoEdit').attr('checked'))
                    return;
                
                if (salary_type.trim() == "Scale")
                {
                    alert("Scall123");
                    basic = $('#txtBasic').val();
                    //-------------------End Basic Allowance Part-----------------------------------------------------------------------------

                    if (('#<%=hfMedicalStatus.ClientID%>').val() = "0") // 0=%
                    {
                        var MP = ($('#<%=lblMedical.ClientID%>').val().trim().length < 0) ? 0 : $('#<%=lblMedical.ClientID%>').val();
                        medical = Math.round(parseFloat(basic) * parseFloat(MP) / 100, 0);
                    }
                    else if (('#<%=hfMedicalStatus.ClientID%>').val() = "1") // 1=৳ 
                        medical = ('#<%=txtMedical.ClientID%>').val();
                    else medical = "0";

                    //-------------------End Medical Allowance Part-----------------------------------------------------------------------------

                    if (('#<%=hfFoodStatus.ClientID%>').val() = "0") // 0=%
                    {
                        var FP = ($('#<%=lblFood.ClientID%>').val().trim().length < 0) ? 0 : $('#<%=lblFood.ClientID%>').val(); 
                        food = Math.round(parseFloat(basic) * parseFloat(FP) / 100, 0);
                    }
                    else if (('#<%=hfFoodStatus.ClientID%>').val() = "1") // 1=৳ 
                        food = ('#<%=txtFoodAllowance.ClientID%>').val();
                    else food = "0";

                    //-------------------End Food Allowance Part-----------------------------------------------------------------------------

                    if (('#<%=hfConveyanceStatus.ClientID%>').val() = "0") // 0=%
                    {
                        var CP = ($('#<%=lblConveyance.ClientID%>').val().trim().length < 0) ? 0 : $('#<%=lblConveyance.ClientID%>').val(); 
                        convayance = Math.round(parseFloat(basic) * parseFloat(CP) / 100, 0);
                    }
                    else if (('#<%=hfConveyanceStatus.ClientID%>').val() = "1") // 1=৳ 
                        convayance = ('#<%=txtConveyanceAllow.ClientID%>').val();
                    else convayance = "0";

                    //-------------------End Conveyance Allowance Part-----------------------------------------------------------------------------

                    if (('#<%=hfTechnicalStatus.ClientID%>').val() = "0") // 0=%
                    {
                        var TP = ($('#<%=lblTechnical.ClientID%>').val().trim().length < 0) ? 0 : $('#<%=lblTechnical.ClientID%>').val(); 
                        technical = Math.round(parseFloat(basic) * parseFloat(TP) / 100, 0);
                    }
                    else if (('#<%=hfTechnicalStatus.ClientID%>').val() = "1") // 1=৳ 
                        technical = ('#<%=txtTechnicalAllow.ClientID%>').val();
                    else technical = "0";

                    //-------------------End Technical Allowance Part-----------------------------------------------------------------------------

                    if (('#<%=hfHouseStatus.ClientID%>').val() = "0") // 0=%
                    {
                        var HP = ($('#<%=lblHouseRent.ClientID%>').val().trim().length < 0) ? 0 : $('#<%=lblHouseRent.ClientID%>').val(); 
                        houseRent = Math.round(parseFloat(basic) * parseFloat(HP) / 100, 0);
                    }
                    else if (('#<%=hfHouseStatus.ClientID%>').val() = "1") // 1=৳ 
                        houseRent = ('#<%=txtHouseRent.ClientID%>').val();
                    else houseRent = "0";

                    //-------------------End House Allowance Part-----------------------------------------------------------------------------

                    if (('#<%=hfOthersStatus.ClientID%>').val() = "0") // 0=%
                    {
                        var OP = ($('#<%=lblOthers.ClientID%>').val().trim().length < 0) ? 0 : $('#<%=lblOthers.ClientID%>').val(); 
                        others = Math.round(parseFloat(basic) * parseFloat(OP) / 100, 0);
                    }
                    else if (('#<%=hfOthersStatus.ClientID%>').val() = "1") // 1=৳ 
                        others = ('#<%=txtOthers.ClientID%>').val();
                    else others = "0";

                    //-------------------End Others Allowance Part-----------------------------------------------------------------------------

                     if (('#<%=hfPFStatus.ClientID%>').val() = "0") // 0=%
                    {
                        var PFP = ($('#<%=lblPF.ClientID%>').val().trim().length < 0) ? 0 : $('#<%=lblPF.ClientID%>').val(); 
                         pf = Math.round(parseFloat(basic) * parseFloat(PFP) / 100, 0);
                    }
                    else if (('#<%=hfPFStatus.ClientID%>').val() = "1") // 1=৳ 
                        pf = ('#<%=txtPFAmount.ClientID%>').val();
                    else pf = "0";

                    //-------------------End Provident Fund Allowance Part-----------------------------------------------------------------------------

                    var TotalSalary = Math.round((parseFloat(basic) + parseFloat(medical) + parseFloat(food) + parseFloat(convayance) + parseFloat(technical) + parseFloat(houseRent) + parseFloat(others)) - parseFloat(pf), 0);
                    $('#txtPresentSalary').val(TotalSalary);

                }

                else if (salary_type == "Gross Scale")
                {
                
                    if ($('#<%=hfIsGarments.ClientID%>').val() == "1")              // for garments salary 
                    {
                      
                        var GetGross = $('#<%=txtPresentSalary.ClientID%>').val();
                        if (GetGross.length == 0) GetGross = 0;
                        var ba = $('#hfBasicStatus').val();
                        if (ba == "0") // 0=%
                        {
                            var BP = ($('#hdfBasic').val().trim().length < 0) ? 0 : $('#hdfBasic').val();
                            basic = Math.round(parseFloat(GetGross) * parseFloat(BP) / 100, 2);
                            $('#txtBasic').val(basic);
                        }
                        else if (ba == "1") // 1=৳ 
                        {
                            basic = ('#<%=txtBasic.ClientID%>').val();
                        }
                        else {
                            basic = "0";
                            $('#txtBasic').val(basic);
                        }

                        if (parseFloat(GetGross) <= 5500) {
                            $('#txtAttenBonus').val(200);
                        }
                        else {
                            $('#txtAttenBonus').val(0);
                        }
                        //-------------------End Basic Allowance Part-----------------------------------------------------------------------------


                        var ma = $('#hfMedicalStatus').val();
                        if (ma == "0") // 0=%
                        {
                            var MP = ($('#hdfMedical').val().trim().length < 0) ? 0 : $('#hdfMedical').val();
                            medical = Math.round(parseFloat(GetGross) * parseFloat(MP) / 100, 2);
                            $('#txtMedical').val(medical);
                        }
                        else if (ma == "1") // 1=৳ 
                        {
                            medical = $('#txtMedical').val();
                        }
                        else {
                            medical = "0";
                            $('#txtMedical').val(medical);
                        }



                        //-------------------End Medical Allowance Part-----------------------------------------------------------------------------
                        var fa = $('#hfFoodStatus').val();
                        if (fa == "0") // 0=%
                        {
                            var FP = ($('#hdfFoodAllowance').val().trim().length < 0) ? 0 : $('#hdfFoodAllowance').val();
                            food = Math.round(parseFloat(GetGross) * parseFloat(FP) / 100, 2);
                            $('#txtFoodAllowance').val(food);
                        }
                        else if (fa == "1") // 1=৳
                        {
                            food = ('#<%=txtFoodAllowance.ClientID%>').val();
                        }

                        else {
                            food = "0";
                            $('#txtMedical').val(medical);
                        }


                        //-------------------End Food Allowance Part-----------------------------------------------------------------------------
                        var ca = $('#hfConveyanceStatus').val();
                        if (ca == "0") // 0=%
                        {
                            var CP = ($('#hdfConveyance').val().trim().length < 0) ? 0 : $('#hdfConveyance').val();
                            convayance = Math.round(parseFloat(GetGross) * parseFloat(CP) / 100, 2);
                            $('#txtConveyanceAllow').val(convayance);
                        }
                        else if (ca == "1") // 1=৳ 
                            convayance = $('#txtConveyanceAllow').val();
                        else {
                            convayance = "0";
                            $('#txtConveyanceAllow').val(convayance);
                        }


                        //-------------------End Conveyance Allowance Part-----------------------------------------------------------------------------                                        

                        var ta = $('#hfTechnicalStatus').val();
                        if (ta == "0") // 0=%
                        {
                            var TP = ($('#hdfTechnical').val().trim().length < 0) ? 0 : $('#hdfTechnical').val();
                            technical = Math.round(parseFloat(GetGross) * parseFloat(TP) / 100, 2);
                            $('#txtTechnicalAllow').va(technical);

                        }
                        else if (ta == "1") // 1=৳ 
                        {
                            technical = $('#txtTechnicalAllow').val();

                        }
                        else {
                            technical = "0";
                            $('#txtTechnicalAllow').val(technical);

                        }

                        //-------------------End Technical Allowance Part-----------------------------------------------------------------------------

                        var ha = $('#hfHouseStatus').val();
                        if (ha == "0") // 0=%
                        {
                            var HP = ($('#hdfhouserent').val().trim().length < 0) ? 0 : $('#hdfhouserent').val();
                            houseRent = Math.round(parseFloat(basic) * parseFloat(HP) / 100, 2);
                            $('#txtHouseRent').val(houseRent);
                        }
                        else if (ha == "1") // 1=৳ 
                            houseRent = $('#txtHouseRent').val();
                        else {
                            houseRent = "0";
                            $('#txtHouseRent').val(houseRent);
                        }

                        //-------------------End House Allowance Part-----------------------------------------------------------------------------

                        var oa = $('#hfOthersStatus').val();
                        if (oa == "0") // 0=%
                        {
                            var OP = ($('#hdfOthers').val().trim().length < 0) ? 0 : $('#hdfOthers').val();
                            others = Math.round(parseFloat(GetGross) * parseFloat(OP) / 100, 2);
                            $('#txtOthers').val(others);
                        }
                        else if (oa == "1") // 1=৳ 
                            others = $('#txtOthers').val();
                        else {
                            others = "0";
                            others = Math.round(parseFloat(GetGross) - (parseFloat(basic) + parseFloat(houseRent) + parseFloat(medical)), 0);
                            if (parseFloat(others) < 0) {
                                houseRent = parseFloat(houseRent) + parseFloat(others);
                                $('#txtHouseRent').val(houseRent);
                                $('#txtOthers').val('0');
                            }
                            else {
                                $('#txtOthers').val(others);
                            }
                            
                        }


                        //-------------------End Others Allowance Part-----------------------------------------------------------------------------
                        var pfa = $('#hfPFStatus').val();
                        if (pfa == "0") // 0=%
                        {
                            var PFP = ($('#hdfPF').val().trim().length < 0) ? 0 : $('#hdfPF').val();
                            pf = Math.round(parseFloat(basic) * parseFloat(PFP) / 100, 0);
                            $('#txtPFAmount').val(pf)
                        }
                        else if (pfa == "1") // 1=৳ 
                            pf = $('#txtPFAmount.ClientID').val();
                        else {
                            pf = "0";
                            $('#txtPFAmount').val(pf)
                        }
                    }

                    if ($('#<%=hfIsGarments.ClientID%>').val() == "0")                              // for others salary or textile salary 
                    {                     
                        var GetGross = $('#<%=txtPresentSalary.ClientID%>').val();
                        if (GetGross.length == 0) GetGross = 0;
                        var ba = $('#hfBasicStatus').val();
                        if (ba == "0") // 0=%
                        {
                            var BP = ($('#hdfBasic').val().trim().length < 0) ? 0 : $('#hdfBasic').val();
                           // basic = parseFloat(GetGross) * parseFloat(BP) / 100;
                            basic = parseFloat(parseFloat(GetGross) * parseFloat(BP) / 100).toFixed(2);
                           // basic = Math.round(parseFloat(GetGross) * parseFloat(BP) / 100, 1);
                            $('#txtBasic').val(basic);
                        }
                        else if (ba == "1") // 1=৳ 
                        {
                            basic = ('#<%=txtBasic.ClientID%>').val();
                        }
                        else {
                            basic = "0";
                            $('#txtBasic').val(basic);
                        }

                        //if (parseFloat(GetGross) <= 5500)
                        //{
                        //    $('#txtAttenBonus').val(200);
                        //}
                        //else
                        //{
                            $('#txtAttenBonus').val(0);
                        //}
                        //-------------------End Basic Allowance Part-----------------------------------------------------------------------------

                  
                        var ma = $('#hfMedicalStatus').val();
                        if (ma == "0") // 0=%
                        {                           
                            var MP = ($('#hdfMedical').val().trim().length < 0) ? 0 : $('#hdfMedical').val();                           
                           // medical = Math.round(parseFloat(GetGross) * parseFloat(MP) / 100, 0);
                            medical = parseFloat(parseFloat(GetGross) * parseFloat(MP) / 100).toFixed(2);
                            $('#txtMedical').val(medical);
                        }
                        else if (ma == "3") // 3=% of basic
                        {
                            var MP = ($('#hdfMedical').val().trim().length < 0) ? 0 : $('#hdfMedical').val();
                           // medical = Math.round(parseFloat(basic) * parseFloat(MP) / 100, 0);
                            medical = parseFloat(parseFloat(basic) * parseFloat(MP) / 100).toFixed(2);

                            $('#txtMedical').val(medical);
                        }
                        else if (ma == "1") // 1=৳ 
                        {
                            medical = $('#txtMedical').val();                          
                        }
                        else {
                            medical = "0";
                            $('#txtMedical').val(medical);
                        }

                           

                        //-------------------End Medical Allowance Part-----------------------------------------------------------------------------
                        var fa = $('#hfFoodStatus').val();
                        if (fa == "0") // 0=%
                        {
                            var FP = ($('#hdfFoodAllowance').val().trim().length < 0) ? 0 : $('#hdfFoodAllowance').val();
                          //  food = Math.round(parseFloat(GetGross) * parseFloat(FP) / 100, 2);
                            food = parseFloat(parseFloat(GetGross) * parseFloat(FP) / 100).toFixed(2);
                            $('#txtFoodAllowance').val(food);
                        }
                       
                        else if (fa == "3") // 3=% of Basic
                        {
                            var FP = ($('#hdfFoodAllowance').val().trim().length < 0) ? 0 : $('#hdfFoodAllowance').val();
                           // food = Math.round(parseFloat(basic) * parseFloat(FP) / 100, 2);
                            food =  parseFloat(parseFloat(basic) * parseFloat(FP) / 100).toFixed(2);
                            $('#txtFoodAllowance').val(food);
                        }
                        else if (fa == "1") // 1=৳
                        {
                            food = ('#<%=txtFoodAllowance.ClientID%>').val();
                        }

                        else {
                            food = "0";
                            $('#txtMedical').val(medical);
                        }
                            

                        //-------------------End Food Allowance Part-----------------------------------------------------------------------------
                        var ca = $('#hfConveyanceStatus').val();
                        if (ca== "0") // 0=%
                        {
                            var CP = ($('#hdfConveyance').val().trim().length < 0) ? 0 : $('#hdfConveyance').val();
                           // convayance = Math.round(parseFloat(GetGross) * parseFloat(CP) / 100, 2);
                            convayance = parseFloat(parseFloat(GetGross) * parseFloat(CP) / 100).toFixed(2);
                            $('#txtConveyanceAllow').val(convayance);
                        }
                        else if (ca == "3") // 3=% of Basic
                        {
                            var CP = ($('#hdfConveyance').val().trim().length < 0) ? 0 : $('#hdfConveyance').val();
                            //convayance = Math.round(parseFloat(basic) * parseFloat(CP) / 100, 2);
                            convayance =parseFloat(parseFloat(basic) * parseFloat(CP) / 100).toFixed(2);
                            $('#txtConveyanceAllow').val(convayance);
                        }
                        else if (ca == "1") // 1=৳ 
                            convayance = $('#txtConveyanceAllow').val();
                        else
                        {
                            convayance = "0";
                            $('#txtConveyanceAllow').val(convayance);
                        }
                            

                        //-------------------End Conveyance Allowance Part-----------------------------------------------------------------------------                                        

                        var ta = $('#hfTechnicalStatus').val();
                        if (ta == "0") // 0=%
                        {
                            var TP = ($('#hdfTechnical').val().trim().length < 0) ? 0 : $('#hdfTechnical').val();
                           // technical = Math.round(parseFloat(GetGross) * parseFloat(TP) / 100, 2);
                            technical = parseFloat(parseFloat(GetGross) * parseFloat(TP) / 100).toFixed(2);
                            $('#txtTechnicalAllow').va(technical);

                        }
                        else if (ta == "3") // 3=% of Basic
                        {
                            var TP = ($('#hdfTechnical').val().trim().length < 0) ? 0 : $('#hdfTechnical').val();
                         //   technical = Math.round(parseFloat(basic) * parseFloat(TP) / 100, 2);
                            technical =parseFloat(parseFloat(basic) * parseFloat(TP) / 100).toFixed(2);
                            $('#txtTechnicalAllow').va(technical);

                        }
                        else if (ta == "1") // 1=৳ 
                        {
                            technical = $('#txtTechnicalAllow').val(); 

                        }                         
                        else
                        {
                            technical = "0";
                            $('#txtTechnicalAllow').val(technical);
                           
                        }
                            
                        //-------------------End Technical Allowance Part-----------------------------------------------------------------------------
                      
                        var ha = $('#hfHouseStatus').val();
                        if (ha == "0") // 0=%
                        {
                            var HP = ($('#hdfhouserent').val().trim().length < 0) ? 0 : $('#hdfhouserent').val();
                           // houseRent = Math.round(parseFloat(GetGross) * parseFloat(HP) / 100, 2);
                            houseRent = parseFloat(parseFloat(GetGross) * parseFloat(HP) / 100).toFixed(2);
                            $('#txtHouseRent').val(houseRent);
                        }
                        else if (ha == "3") // 3=% of Basic
                        {
                            var HP = ($('#hdfhouserent').val().trim().length < 0) ? 0 : $('#hdfhouserent').val();
                            //houseRent = Math.round(parseFloat(basic) * parseFloat(HP) / 100, 2);
                            houseRent =parseFloat(parseFloat(basic) * parseFloat(HP) / 100).toFixed(2);
                            $('#txtHouseRent').val(houseRent);
                        }
                        else if (ha == "1") // 1=৳ 
                            houseRent = $('#txtHouseRent').val();
                        else {
                            houseRent = "0";
                            $('#txtHouseRent').val(houseRent);
                        }

                        //-------------------End House Allowance Part-----------------------------------------------------------------------------

                        var oa = $('#hfOthersStatus').val();
                        if (oa == "0") // 0=%
                        {
                            var OP = ($('#hdfOthers').val().trim().length < 0) ? 0 : $('#hdfOthers').val();
                            others = Math.round(parseFloat(GetGross) * parseFloat(OP) / 100, 2);
                            $('#txtOthers').val(others);
                        }
                        else if (oa == "3") // 3=% of Basic
                        {
                            var OP = ($('#hdfOthers').val().trim().length < 0) ? 0 : $('#hdfOthers').val();
                            //others = Math.round(parseFloat(basic) * parseFloat(OP) / 100, 2);
                            others =parseFloat(parseFloat(basic) * parseFloat(OP) / 100).toFixed(2);
                            $('#txtOthers').val(others);
                        }
                        else if (oa == "1") // 1=৳ 
                            others = $('#txtOthers').val();
                        else
                        {
                            others = "0";
                            //others = Math.round(parseFloat(GetGross) - (parseFloat(basic) + parseFloat(houseRent) + parseFloat(medical)), 0);
                            $('#txtOthers').val(others);
                        }
                            

                        //-------------------End Others Allowance Part-----------------------------------------------------------------------------
                        var pfa = $('#hfPFStatus').val();
                        if (pfa == "0") // 0=%
                        {
                            var PFP = ($('#hdfPF').val().trim().length < 0) ? 0 : $('#hdfPF').val();
                            //pf = Math.round(parseFloat(basic) * parseFloat(PFP) / 100, 2);                           
                            pf = parseFloat(parseFloat(basic) * parseFloat(PFP) / 100).toFixed(2);                        
                            $('#txtPFAmount').val(pf)
                        }
                        else if (pfa == "3") // 3=% of Basic
                        {
                            var PFP = ($('#hdfPF').val().trim().length < 0) ? 0 : $('#hdfPF').val();
                           // pf = Math.round(parseFloat(basic) * parseFloat(PFP) / 100, 2);
                            pf = parseFloat(parseFloat(basic) * parseFloat(PFP) / 100).toFixed(2);   
                            $('#txtPFAmount').val(pf)
                        }
                        else if (pfa == "1") // 1=৳ 
                            pf = $('#txtPFAmount.ClientID').val();
                        else
                        {
                            pf = "0";
                            $('#txtPFAmount').val(pf)
                        }
                                                

                    }

                    //-------------------End Medical Allowance Part---------------------------------------------------------------------------------------
                }

              


            }
            catch (exception)
            { }
        }



        function InputValidation() {
            try {       
               
                if ($('#ddlEmpCardNo option:selected').text().length == 0) {

                    showMessage("warning->Please Select Any Employee");
                    $('#ddlEmpCardNo').focus();
                    return false;
                }
              
                var SalaryCout = $('#<%=chkPaymentType.ClientID %> input:checked').val();
                if (SalaryCout == 1 && ($('#txtBankName').val().trim().length == 0 || $('#txtEmpAccNo').val().trim().length == 0)) {
              
                    showMessage("warning->Please Enter Bank Name and Account Number");
                   $('#txtEmpAccNo').focus();
                            return false;
                }


                if ($('#ddlGrade option:selected').text().length == 0) {

                    showMessage("warning->Please Select Any Grade ");
                    $('#ddlGrade').focus();
                    return false;
                }
                //var type = $('#hdfpresentsalryKeypress').val();                
                //if (type==0)
                //    {
                //    //if ($('#hdfpresentsalryKeypress').val().length == 0) {
                //        showMessage("warning->Please Enter Present Salary");
                //        $('#txtPresentSalary').focus();
                //        return false;
                //    //}
                //}
                
                if ($('#chkPFMember').is(':checked')) {
                    if ($('#txtPFDate').val().trim().length == 0) {
                        showMessage("warning->Please Enter PF Date");
                        $('#txtPFDate').focus();
                        return false;
                    }
                    if ($('#txtPFAmount').val().trim().length == 0) {
                        showMessage("warning->Please Enter PF Amount");
                        $('#txtPFAmount').focus();
                        return false;
                    }
                }
               <%-- var emptype = $('#<%=rblEmpType.ClientID %> input:checked').val();
                if (emptype == 2) {
                    var utype = $('#<%=rblusertype.ClientID %> input:checked').val();
                    if (utype == undefined) {                       
                        if ($('#hdfUserType').val() == "1") {
                            showMessage("warning->Please Select User Type ");
                            return false;
                        }
                    }
                } --%>              
               
                return true;
              

            }
            catch (exception) {

            }
        }

       
        function ShowdivEmpAddress() {
            $('#divEmpInfo').hide();
            $('#div_emp_save').hide();
            $('#divEmpAddress').show();
            $('#divEmpPersonnelInfo').hide();
        }
        function AllHidewithoutEmployeediv() {
            $('#divEmpInfo').show();
            $('#div_emp_save').show();
            $('#divEmpAddress').hide();
            $('#divEmpPersonnelInfo').hide();
            $('#divEmpExperience').hide();
            $('#divEmpEducation').hide();
        }
        function divEmpExperienceList() {
            if ($('#ddlEmpCardNo option:selected').text().length == 0) {
                showMessage('Please Select Employee Card No','warning');
                return false;
            }
           
            return true;
        }
    
          
         
                function SaveSuccess() {
                    showMessage("Successfully saved", "success");
                }
                function UnableSave() {
                    $("#ddlEmpCardNo").select2();
                    showMessage("Unable to save", "error");
                }
                function UpdateSuccess() {
                    showMessage("Successfully Updated", "success");
                }
                function UnableUpdate() {
                    showMessage("Unable to Update", "error");
                }


               
        </script>

</asp:Content>
