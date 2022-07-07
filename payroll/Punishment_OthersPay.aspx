<%@ Page Title="Deductions & Adjustment" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="Punishment_OthersPay.aspx.cs" Inherits="SigmaERP.payroll.Punishment_OthersPay" %>
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
        #ContentPlaceHolder1_ContentPlaceHolder1_tc1_TabPanel1_gvMobileCell th {

            text-align:center;
        }
          #ContentPlaceHolder1_ContentPlaceHolder1_tc1_TabPanel1_gvMobileCell th:nth-child(2) {

            text-align:left;
        }
           #ContentPlaceHolder1_ContentPlaceHolder1_tc1_TabPanel1_gvMobileCell td {

            text-align:center;
        }
              #ContentPlaceHolder1_ContentPlaceHolder1_tc1_TabPanel1_gvMobileCell td:nth-child(2) {

            text-align:left;
        }
        
        .amnt-row .amnt-col {
            width: 32.89%;
            float: left;
        }
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
                     <li> <a href="#" class="ds_negevation_inactive Pactive">Deductions & Adjustment</a></li>
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
 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >        
        <ContentTemplate>          
        
   
   <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Deductions & Adjustment</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">
                <%--<input type="text" class="form-control" visible="false" id="txtFinding" runat="server" style="margin-left: 0px; width: 99%; text-align:center"  placeholder="Search by anythings" />--%>               
                <div class="em_personal_info" id="divEmpPersonnelInfo" style="margin:0px">
                    
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <Triggers>                           
                           
                        </Triggers>
                        <ContentTemplate>

                            <asp:TabContainer ID="tc1" runat="server" CssClass="fancy fancy-green" AutoPostBack="true" OnActiveTabChanged="tc1_ActiveTabChanged" ActiveTabIndex="0">
                                <asp:TabPanel runat="server"  TabIndex="0" ID="TabPanel1" HeaderText="Mobile Cell">
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlCompanyListMobileCell" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNoMobileCell" />                                               
                                               
                                                <asp:AsyncPostBackTrigger ControlID="btnSaveMobileCell" />                                                
                                                                                           
                                            </Triggers>
                                            <ContentTemplate>
                                                

                                        <table class="em_personal_info_table">
                                            <tr>
                                                 <td>Company
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCompanyListMobileCell" runat="server"  OnSelectedIndexChanged="ddlCompanyListMobileCell_SelectedIndexChanged" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" >
                                            </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Card No
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlEmpCardNoMobileCell" ClientIDMode="Static" AutoPostBack="false" CssClass="form-control select_width"></asp:DropDownList>
                                                </td>
                                            </tr>                                         
                                            <tr>
                                                <td>Amount
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtAmountMobileCell" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width">0</asp:TextBox>

                                                </td>
                                            </tr> 
                                             <tr>
                                                <td>Month Name
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtMonthMobileCell" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender1" Format="MM-yyyy" runat="server" TargetControlID="txtMonthMobileCell"></asp:CalendarExtender>
                                                </td>
                                            </tr>                                                                                      
                                        </table>
                                                <br />
                                        <table class="em_button_table">
                                            <tr>

                                              <%--  <th>
                                                    <asp:Button ID="btnNew" Class="Pbutton" runat="server" Text="New" />
                                                </th>   --%>                                    

                                                <th>
                                                    <asp:Button ID="btnSaveMobileCell" CssClass="Pbutton" ClientIDMode="Static" runat="server"  Text="Submit"  OnClick="btnSaveMobileCell_Click"  />
                                                </th>
                                                <th>
                                                    <asp:Button ID="btnSearchMobileCell" CssClass="Pbutton" ClientIDMode="Static" runat="server"  Text="Search" OnClick="btnSearchMobileCell_Click"/>
                                                </th>
                                                <th>
                                                    <asp:Button ID="btnPrintMobileCell" CssClass="Pbutton" ClientIDMode="Static" runat="server"  Text="Print" OnClick="btnPrintMobileCell_Click"/>
                                                </th>
                                                

                                            </tr>
                                        </table>
                                                <br />
                                                  <asp:GridView runat="server" ID="gvMobileCell" HeaderStyle-HorizontalAlign="Center" CssClass="gvdisplay1" DataKeyNames="McId,CompanyId,EmpId" AutoGenerateColumns="false" HeaderStyle-BackColor="#ffa500" HeaderStyle-Height="28px" HeaderStyle-ForeColor="White" PageSize="25" Width="100%"   OnRowCommand="gvMobileCell_RowCommand" >
                                            <Columns>                                                
                                              
                                                 <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" ItemStyle-HorizontalAlign="Center" />
                                                 <asp:BoundField DataField="EmpName" HeaderStyle-HorizontalAlign="Left" HeaderText="Name" />
                                                 
                                                 <asp:BoundField DataField="MobileCell" HeaderText="P.Amount" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="Month" HeaderText="Month" ItemStyle-HorizontalAlign="Center"/>                                                 
                                                <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                <asp:LinkButton ID="lnkAlter" runat="server" CommandName="Alter" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Edit" Font-Bold="true" ForeColor="Green" ></asp:LinkButton>
                                                </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Delete" >
                                                <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="deleterow" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Delete" Font-Bold="true" ForeColor="Red" OnClientClick="return confirm('Are you sure to delete?');" ></asp:LinkButton>
                                                </ItemTemplate>

                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                                </ContentTemplate>
                             </asp:UpdatePanel>
                                    </ContentTemplate>

                                </asp:TabPanel>

                                <asp:TabPanel runat="server"  TabIndex="1" ID="tab1" HeaderText="Advance">
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />                                               
                                               
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" />                                                
                                                                                           
                                            </Triggers>
                                            <ContentTemplate>
                                                

                                        <table class="em_personal_info_table">
                                            <tr>
                                                 <td>Company
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCompanyList" runat="server" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" >
                                            </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Card No
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlEmpCardNo" ClientIDMode="Static" AutoPostBack="false" CssClass="form-control select_width"></asp:DropDownList>
                                                </td>
                                            </tr>                                           
                                                                               
                                            <tr runat="server" visible="false">
                                                <td>Punishment Name
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtpunishment" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"  ></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Amount
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtPAmount" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width">0</asp:TextBox>

                                                </td>
                                            </tr> 
                                             <tr>
                                                <td>Month Name
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtmonthname" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender3" Format="MM-yyyy" runat="server" TargetControlID="txtmonthname"></asp:CalendarExtender>
                                                </td>
                                            </tr>                                                                                      
                                        </table>
                                                
                                        <table class="em_button_table">
                                            <tr>

                                              <%--  <th>
                                                    <asp:Button ID="btnNew" Class="Pbutton" runat="server" Text="New" />
                                                </th>   --%>                                    

                                                <th>
                                                    <asp:Button ID="btnSave" CssClass="Pbutton" ClientIDMode="Static" runat="server" OnClientClick="return validateInputspunishment();" Text="Submit" OnClick="btnSave_Click" />
                                                </th>
                                                <th>
                                                    <asp:Button ID="btnSearch" CssClass="Pbutton" ClientIDMode="Static" runat="server"  Text="Search" OnClick="btnSearch_Click"/>
                                                </th>
                                                <th>
                                                    <asp:Button ID="btnPrint" CssClass="Pbutton" ClientIDMode="Static" runat="server"  Text="Print" OnClick="btnPrint_Click"/>
                                                </th>

                                            </tr>
                                        </table>
                                                <br />
                                                  <asp:GridView runat="server" ID="gvpunishment" CssClass="gvdisplay1" DataKeyNames="PSN,CompanyId,EmpId" AutoGenerateColumns="false" HeaderStyle-BackColor="#ffa500" HeaderStyle-Height="28px" HeaderStyle-ForeColor="White" PageSize="25" Width="100%"  OnRowCommand="gvpunishment_RowCommand" >
                                            <Columns>                                                
                                                <asp:BoundField DataField="CompanyName" HeaderText="Company" ItemStyle-HorizontalAlign="Center" />
                                                 <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" ItemStyle-HorizontalAlign="Center" />
                                                 <asp:BoundField DataField="EmpName" HeaderStyle-HorizontalAlign="Left" HeaderText="Name" />
                                                <%-- <asp:BoundField DataField="PName" HeaderText="P.Name" ItemStyle-HorizontalAlign="Center"/>--%>
                                                 <asp:BoundField DataField="PAmount" HeaderText="Amount" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="MonthName" HeaderText="Month" ItemStyle-HorizontalAlign="Center"/>                                                 
                                                <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                <asp:LinkButton ID="lnkAlter" runat="server" CommandName="Alter" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Edit" Font-Bold="true" ForeColor="Green" ></asp:LinkButton>
                                                </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Delete" >
                                                <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="deleterow" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Delete" Font-Bold="true" ForeColor="Red" OnClientClick="return confirm('Are you sure to delete?');" ></asp:LinkButton>
                                                </ItemTemplate>

                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                                </ContentTemplate>
                             </asp:UpdatePanel>
                                    </ContentTemplate>

                                </asp:TabPanel>
                                <asp:TabPanel runat="server" ID="tab2" TabIndex="2" HeaderText="Other's Pay" >
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                                              <Triggers>                                               
                                                <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo2" />
                                                <asp:AsyncPostBackTrigger ControlID="ddlCompanyList2" />                                               
                                               
                                                <asp:AsyncPostBackTrigger ControlID="btnSave2" />   
                                                                                           
                                            </Triggers>
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
                                              <tr>
                                                <td>Card No
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlEmpCardNo2" ClientIDMode="Static" AutoPostBack="false" CssClass="form-control select_width"></asp:DropDownList>
                                                </td>
                                            </tr>                                           
                                                                               
                                            <tr >
                                                <td>Purpose
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtpurpose" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" onKeyUp="SalaryCalculation();" AutoComplete="off" ></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Amount
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtotherpayAmount" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width">0</asp:TextBox>

                                                </td>
                                            </tr>                                            
                                              <tr>
                                                <td>Active
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td><asp:CheckBox ID="checkActive" runat="server" Checked="true" ClientIDMode="Static" AutoPostBack="false" /></td>
                                                            
                                                           
                                                        </tr>
                                                    </table>
                                                    

                                                </td>
                                            </tr>  
                                          </table>
                                                 <table class="em_button_table">
                                            <tr>

                                                <th>
                                                    <asp:Button ID="Button1" Class="Pbutton" runat="server" Text="New" />
                                                </th>                                       

                                                <th>
                                                    <asp:Button ID="btnSave2" CssClass="Pbutton" ClientIDMode="Static" runat="server" OnClientClick="return validateInputsotherspay();" Text="Submit" OnClick="btnSave2_Click" />
                                                </th>

                                                <th>
                                                    <asp:Button ID="Button3" Class="Pbutton" runat="server" Text="Close" PostBackUrl="~/personnel_defult.aspx" /></th>

                                            </tr>
                                        </table>
                                        <asp:GridView runat="server" ID="gvotherspay" CssClass="gvdisplay1" DataKeyNames="OPSN,CompanyId,EmpId" AutoGenerateColumns="false" HeaderStyle-BackColor="#ffa500" HeaderStyle-Height="28px" HeaderStyle-ForeColor="White" PageSize="25" Width="100%" OnRowCommand="gvotherspay_RowCommand" >
                                            <Columns>
                                                <asp:BoundField DataField="CompanyName" HeaderText="Company" ItemStyle-HorizontalAlign="Center" />
                                                 <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" ItemStyle-HorizontalAlign="Center" />
                                                 <asp:BoundField DataField="EmpName" HeaderStyle-HorizontalAlign="Left" HeaderText="Name" />
                                                 <asp:BoundField DataField="OPpurpose" HeaderText="Purpose" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="OtherPay" HeaderText="Amount" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="IsActive" HeaderText="Enable" ItemStyle-HorizontalAlign="Center"/>                                                 
                                                <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                <asp:LinkButton ID="lnkAlter" runat="server" CommandName="Alter" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Edit" Font-Bold="true" ForeColor="Green" ></asp:LinkButton>
                                                </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Delete" >
                                                <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="deleterow" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Delete" Font-Bold="true" ForeColor="Red" OnClientClick="return confirm('Are you sure to delete?');" ></asp:LinkButton>
                                                </ItemTemplate>

                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                                  </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>

                                </asp:TabPanel>
                                   <asp:TabPanel runat="server" ID="TabPanel2" TabIndex="3" HeaderText="Adjustment" >
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                                              <Triggers>                                               
                                                <asp:AsyncPostBackTrigger ControlID="ddlAddPurposeList"/>
                                                <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo3"/>
                                                <asp:AsyncPostBackTrigger ControlID="ddlCompanyList3"/>                              
                                                <asp:AsyncPostBackTrigger ControlID="btnSave3"/>   
                                                                                           
                                            </Triggers>
                                            <ContentTemplate>

                                          <table class="em_personal_info_table">
                                              <tr>
                                                 <td>Company
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCompanyList3" runat="server" OnSelectedIndexChanged="ddlCompanyList2_SelectedIndexChanged" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" >
                                            </asp:DropDownList>
                                                </td>
                                            </tr>
                                               <tr>
                                                <td>Month
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtAddMonth" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                                    <asp:CalendarExtender ID="CalendarExtender2" Format="MM-yyyy" runat="server" TargetControlID="txtAddMonth"></asp:CalendarExtender>
                                                </td>
                                            </tr> 
                                              <tr>
                                                <td>Employee
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlEmpCardNo3" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" OnSelectedIndexChanged="ddlEmpCardNo3_SelectedIndexChanged"></asp:DropDownList>
                                                </td>
                                            </tr>                                           
                                                                               
                                            <tr >
                                                <td>Purpose
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:DropDownList runat="server" ID="ddlAddPurposeList" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" OnSelectedIndexChanged="ddlAddPurposeList_SelectedIndexChanged" >            
                                                    </asp:DropDownList>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Amount
                                                </td>
                                                <td>:
                                                </td>
                                                <td class="amnt-row">
                                                    <span class="amnt-col"><asp:TextBox ID="txtBasic" runat="server" Enabled="false" ClientIDMode="Static" CssClass="form-control text_box_width" Visible="false">0</asp:TextBox></span>
                                                    <span class="amnt-col"> <asp:TextBox ID="txtDays" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" placeholder="Days" Visible="false" AutoPostBack="true" autocomplete="off" OnTextChanged="txtDays_TextChanged" ></asp:TextBox></span>
                                                    <span class=" amnt-col"> <asp:TextBox ID="txtAddAmount" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width">0</asp:TextBox></span>
                                                </td>
                                            </tr> 
                                              <tr>
                                                  <td>Remarks</td>
                                                  <td>:</td>
                                                  <td>  <asp:TextBox ID="txtAddRemarks" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                                  </td>
                                              </tr>
                                          </table>
                                                 <table class="em_button_table">
                                            <tr>

                                                <th>
                                                    <asp:Button ID="btnAddNew" Class="Pbutton" runat="server" Text="New" />
                                                </th>                                       

                                                <th>
                                                    <asp:Button ID="btnSave3" CssClass="Pbutton" ClientIDMode="Static" runat="server" OnClientClick="return validateInputsAdjustment();" Text="Submit" OnClick="btnSave3_Click" />
                                                </th>
                                            </tr>
                                        </table>
                                        <asp:GridView runat="server" ID="gvAdjustment" CssClass="gvdisplay1" DataKeyNames="SL,CompanyId,EmpId,AdjustmentTypeID" AutoGenerateColumns="false" HeaderStyle-BackColor="#ffa500" HeaderStyle-Height="28px" HeaderStyle-ForeColor="White" PageSize="25" Width="100%" OnRowCommand="gvAdjustment_RowCommand">
                                            <Columns>
                                                <%--<asp:BoundField DataField="CompanyName" HeaderText="Company" ItemStyle-HorizontalAlign="Center" />
                                                 <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" ItemStyle-HorizontalAlign="Center" />
                                                 <asp:BoundField DataField="EmpName" HeaderStyle-HorizontalAlign="Left" HeaderText="Name" />--%>
                                                 <asp:BoundField DataField="Month" HeaderText="Month" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="AdjustmentType" HeaderText="Purpose" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-HorizontalAlign="Center"/>   
                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" ItemStyle-HorizontalAlign="Center"/>
                                                <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                <asp:LinkButton ID="lnkAlter" runat="server" CommandName="Alter" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Edit" Font-Bold="true" ForeColor="Green" ></asp:LinkButton>
                                                </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Delete" >
                                                <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="deleterow" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Delete" Font-Bold="true" ForeColor="Red" OnClientClick="return confirm('Are you sure to delete?');" ></asp:LinkButton>
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
            load();
        });   
        function goToNewTabandWindow(url) {
            window.open(url);

        }
        function load() {
            $("#ddlEmpCardNo").select2();
            $("#ddlEmpCardNo2").select2();
            $("#ddlEmpCardNoMobileCell").select2();
            $("#ddlEmpCardNo3").select2();
        }
        function Messageshow(messagetype,message)
        {
            showMessage(message, messagetype);
            load();
        }
        function goToNewTab(url)
        {
            $("#ddlEmpCardNo").select2();
            $("#ddlEmpCardNo").select2();            
            window.open(url);
        }
        
        function validateInputspunishment() {
            if ($('#ddlCompanyList option:selected').text().length == 0) {
                showMessage("warning->Please Select Company ");
                $('#ddlCompanyList').focus();
                return false;
            }
            if ($('#ddlEmpCardNo option:selected').text().length == 0) {
                showMessage("warning->Please Select Card No ");
                $('#ddlEmpCardNo').focus();
                return false;
            }
            if (validateText('txtpunishment', 1, 60, 'Enter Punishment Name') == false) return false;
            if (validateText('txtPAmount', 1, 6, 'Enter Punishment Amount') == false) return false;
            if (validateText('txtmonthname', 1, 7, 'Select Month Name') == false) return false;
            return true;
        }
        function validateInputsotherspay() {
            if ($('#ddlCompanyList2 option:selected').text().length == 0) {
                showMessage("warning->Please Select Company ");
                $('#ddlCompanyList2').focus();
                return false;
            }

            if ($('#ddlEmpCardNo2 option:selected').text().length == 0) {
                showMessage("warning->Please Select Card No ");
                $('#ddlEmpCardNo2').focus();
                return false;
            }
            if (validateText('txtpurpose', 1, 60, 'Enter Purpose Name') == false) return false;
            if (validateText('txtotherpayAmount', 1, 6, 'Enter Other pay Amount') == false) return false;            
            return true;
        }
      function validateInputsAdjustment() {
            if ($('#ddlCompanyList3 option:selected').text().length == 0) {
                showMessage("warning->Please Select Company ");
                $('#ddlCompanyList3').focus();
                return false;
          }
          if ($('#txtAddMonth').val().length == 0) {
                showMessage("warning->Please Select Month ");
                $('#txtAddMonth').focus();
                return false;
            }
            if ($('#ddlEmpCardNo3 option:selected').text().length == 0) {
                showMessage("warning->Please Select Card No ");
                $('#ddlEmpCardNo3').focus();
                return false;
          }
           if ($('#ddlAddPurposeList option:selected').text().length == 0) {
                showMessage("warning->Please Select Purpose.");
                $('#ddlAddPurposeList').focus();
                $('#ddlAddPurposeList').focus();
                return false;
            }                        
            return true;
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
