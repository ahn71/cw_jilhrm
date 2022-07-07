<%@ Page Title="Salary Increment" Language="C#" MasterPageFile="~/personnel_NestedMaster.Master" AutoEventWireup="true" CodeBehind="salary_increment.aspx.cs" Inherits="SigmaERP.personnel.salary_increment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style>
       .gvRight th {
           text-align:center;
       }
   </style>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <div class="row">
                  <div class="col-md-12 ds_nagevation_bar">
               <div style="margin-top: 5px">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                       <li> <a class="seperator" href="#"></a>/</li>
                       <li> <a href="/payroll_default.aspx">Payroll</a></li>   
                       <li> <a class="seperator" href="#"></a>/</li>                  
                        <li> <a href="/payroll/salary_index.aspx">Salary</a></li>
                        <li> <a class="seperator" href="#"></a>/</li>
                       <li> <a href="#" class="ds_negevation_inactive Pactive">Salary Increment</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hfSaveStatus" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfEmpSN" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfBasicPercentage" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfOthersAllowance" ClientIDMode="Static" runat="server" Value="0" />
    <asp:HiddenField ID="hfHouseAmount" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfHouseRentPercentage" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfDeleteStatus" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfEmpId" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="upSave" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upupdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="updelete" runat="server" ClientIDMode="Static" />

    <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Salary Increment</h2>
        </div>   
        
         <asp:TabContainer ID="tc1" runat="server" CssClass="fancy fancy-green" AutoPostBack="true" >
                                <asp:TabPanel runat="server"  TabIndex="0" ID="tab1" HeaderText="Salary Entry">
                                    
                                    </asp:TabPanel>

                                    </asp:TabContainer>
        
        
             
    	<div class="main_box_body Pbody">
            
            <div class="main_box_content"  style="overflow:hidden">
                <div class="salary_increment_left_area" style="width:52%">
                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>                       
                        <asp:AsyncPostBackTrigger ControlID="ddlCompany" />
                        <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:HiddenField ID="hfSalaryType" ClientIDMode="Static" runat="server" />
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
                        
                            <div class="punishment_against">
                                <table class="employee_table">
                                    <tbody>
                                        <tr id="trCompany" runat="server" >
                                            <td>Company
                                            </td>
                                            <td>:
                                            </td>
                                            <td id="td1" runat="server" clientidmode="Static" style="font-size: 16px">
                                                <asp:DropDownList ID="ddlCompany" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>                                       
                                        <tr>
                                            <td>Employee Card No
                                            </td>
                                            <td>:
                                            </td>
                                            <td id="tdCardNo" runat="server" clientidmode="Static" style="font-size: 16px"> 
                                                <table class="employee_table">
                                                    <tr>
                                                        <td style="width:60%"><asp:DropDownList ID="ddlEmpCardNo" runat="server" AutoPostBack="true" ClientIDMode="Static"  CssClass="form-control select_width" OnSelectedIndexChanged="ddlEmpCardNo_SelectedIndexChanged"></asp:DropDownList></td>
                                                        <td style="width:20%"><asp:Button ID="btnIncrementInfo" ClientIDMode="Static" runat="server" Text="Increment Status" CssClass="Pbutton"  OnClick="btnIncrementInfo_Click" /> </td>
                                                    </tr>
                                                </table>                                                                                                                             
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Employee Name
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmpName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Last Increment Date
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLastIncrementDate" autocomplete="off" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtLastIncrementDate">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Last Increment Amount
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLastIncrementAmount" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Increment Amount
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIncrementAmount" runat="server" ClientIDMode="Static" onKeyUp="SalaryCalculation();" autocomplete="off" CssClass="form-control text_box_width"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="TextBox1_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" TargetControlID="txtIncrementAmount" FilterType="Numbers">
                                                </asp:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <!--ST-->
                           <div class="promo_middle_box">
                                <div class="promo_box_left">
                                    <fieldset >
                                        <legend>P<b>resent Salary Structure</b>
                                        </legend>
                                        <table class="employee_table">
                                            <tr>
                                                <td>Grade
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                     
                                                    <asp:TextBox ID="txtPresentGrade" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width_2" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Basic<asp:Label Visible="true" ID="lblBasic" Font-Bold="true" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPresentBasic" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width_2" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Medical<asp:Label Visible="true" ID="lblMedical" Font-Bold="true" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPresentMedical" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" Enabled="False"></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr id="trFood" runat="server" >
                                                <td>Food<asp:Label Visible="true" ID="lblFood" Font-Bold="true" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPresentFoodAllowance" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width_2" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trConveyance" runat="server" >
                                                <td>Conveyance<asp:Label Visible="true" ID="lblConveyance" Font-Bold="true" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPresentConveyance" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width_2" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="tr1" visible="false">
                                                <td>Technical<asp:Label Visible="true" Font-Bold="true" ID="lblTechnical" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtTechnicalAllow"  runat="server" ClientIDMode="Static" Enabled="false" CssClass="form-control text_box_width">0</asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    House<asp:Label runat="server" Font-Bold="true" ID="lblHouseRent" Text="" ClientIDMode="Static"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPresentHouseRent" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr runat="server" visible="false">
                                                <td>Others <asp:Label runat="server" Font-Bold="true" ID="lblOthers" Text="" ClientIDMode="Static"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOthers" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False">0</asp:TextBox>
                                                </td>
                                            </tr>                                             
                                            <tr id="trPFAmount" runat="server">
                                                <td>PF Amount<asp:Label ID="lblPF"  runat="server" ClientIDMode="Static" Font-Bold="true" Text="">0</asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPFAmount" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Gross Salary
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPresentGrossSalary" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td>TDS
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTDS" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                                <!--RT   -->
                                <div class="promo_box_right">
                                    <fieldset style="padding: 0px !important">
                                        <legend>
                                            <b>New Salary Structure</b>
                                        </legend>
                                        <table class="employee_table">
                                            <tr>
                                                <td>Grade
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:DropDownList ID="ddlGrade" runat="server" ClientIDMode="Static" CssClass="form-control select_width"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Basic<asp:Label Visible="true" ID="lblbasicnew" Font-Bold="true" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNewBasic" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width_2" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Medical<asp:Label Visible="true" ID="lblmedicalnew" Font-Bold="true" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNewMedical" runat="server" ClientIDMode="Static" onkeypress="return totalSalaryCalculation(event)" CssClass="form-control text_box_width_2" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trNewFood" runat="server" >
                                                <td>Food<asp:Label Visible="true" ID="lblfoodnew" Font-Bold="true" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNewFoodAllowance" onkeypress="return totalSalaryCalculation(event)" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width_2" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trNewConveyance" runat="server">
                                                <td>Conveyance<asp:Label Visible="true" ID="lblconveyancenew" Font-Bold="true" ClientIDMode="Static" Text="" runat="server"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNewConveyance" onkeypress="return totalSalaryCalculation(event)" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width_2" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>House<asp:Label runat="server" Font-Bold="true" ID="lblhouserentnew" Text="" ClientIDMode="Static"></asp:Label>
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNewHouseRent" runat="server" onkeypress="return totalSalaryCalculation(event)" ClientIDMode="Static" CssClass="form-control text_box_width_2" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr id="tr2" runat="server" visible="false">
                                                <td>Others
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNewOthers" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False">0</asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="tr6" runat="server" visible="false">
                                                <td>Others
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNewTransportOthers" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False">0</asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id="trNewPF" runat="server">
                                                <td>PF Amount
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNewPF" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" Enabled="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Gross Salary
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNewGross" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>TDS
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNewTDS" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </div>
                            <div class="punishment_against">
                                <table class="employee_table">
                                    <tbody>
                                        <tr>
                                            <td>Increment Order Ref Number
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIncrementOrderRefNumber" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Increment Order Ref Date
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIncrementOrderRefDate" autocomplete="off" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>

                                                <asp:CalendarExtender ID="txtIncrementOrderRefDate_CalendarExtender" Format="d-M-yyyy" runat="server" TargetControlID="txtIncrementOrderRefDate">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Effective From
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEffectiveFrom" runat="server" ClientIDMode="Static" autocomplete="off" CssClass="form-control text_box_width"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtEffectiveFrom_CalendarExtender" Format="MM-yyyy" runat="server" TargetControlID="txtEffectiveFrom">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Remarks
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRemarks" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                            </td>
                                        </tr>

                                    </tbody>
                                </table>

                            </div>
                       <div class="emp_button_area" style="width: 426px">
                                <table class="promo_button_table">
                                    <tbody>
                                        <tr>
                                            <th>
                                                <asp:Button ID="btnSave" CssClass="Pbutton" ClientIDMode="Static" runat="server" Text="Save" OnClientClick="return InputValidationBasket();" OnClick="btnSave_Click" /></th>
                                            <th>
                                                <asp:Button ID="btnClear" CssClass="Pbutton" runat="server" ClientIDMode="Static" Text="Clear"  OnClick="btnClear_Click" /></th>
                                            <th><a class="Pbutton" href="../default.aspx">Close</a></th>
                                            <th>
                                                <%-- <asp:Button ID="btnDelete" ClientIDMode="Static" CssClass="emp_btn" runat="server" Text="Delete" OnClick="btnDelete_Click"  /></th>--%>
                                            </th>
                                            <th>
                                                <asp:Button ID="btnComplain" runat="server" Text="Complain" Visible="false" CssClass="css_btn" OnClick="btnComplain_Click" /></th>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                      

                    </ContentTemplate>
                </asp:UpdatePanel>
                      </div>
                <div class="salary_increment_right_area" style="width:48%">
                    <asp:UpdatePanel runat="server" ID="dipUpdate" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" />
                            <%--   <asp:AsyncPostBackTrigger ControlID="btnSearch" />--%>
                        </Triggers>
                        <ContentTemplate>
                            <div runat="server" id="divPagelist" visible="true" style="height: 782px; overflow: scroll">
                                <%-- <div id="divSalaryIncrementList" runat="server" style="height: 599px;"></div>--%>
                                <asp:GridView ID="divSalaryIncrementList" CssClass="gvRight" runat="server" Width="100%" AutoGenerateColumns="False" DataKeyNames="SN,EmpId" HeaderStyle-BackColor="#ffa500" HeaderStyle-Height="28px" HeaderStyle-ForeColor="white" HeaderStyle-Font-Size="11px" HeaderStyle-Font-Bold="true" AllowPaging="True" OnRowCommand="divSalaryIncrementList_RowCommand" OnRowDeleting="divSalaryIncrementList_RowDeleting" >

                                    <HeaderStyle BackColor="#ffa500" Font-Bold="True" Font-Size="11px" ForeColor="White" Height="28px"></HeaderStyle>
                                    <PagerStyle CssClass="gridview" Height="20px" />
                                    <RowStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:BoundField DataField="SN" HeaderText="SN" Visible="false" ItemStyle-Height="28px">
                                            <ItemStyle Height="28px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EmpId" HeaderText="EmpId" Visible="false" ItemStyle-Height="28px">
                                            <ItemStyle Height="28px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CompanyName" HeaderStyle-HorizontalAlign="Left" HeaderText="Company Name" Visible="false" ItemStyle-Width="100px" ItemStyle-Height="28px" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                                            <ItemStyle HorizontalAlign="Left" Height="28px" Width="100px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" Visible="true" ItemStyle-Width="120px" ItemStyle-Height="28px">
                                            <ItemStyle Height="28px" Width="120px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IncrementAmount" HeaderText="Increment" Visible="true" ItemStyle-Width="120px" ItemStyle-Height="28px">
                                            <ItemStyle Height="28px" Width="120px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EmpPresentSalary" HeaderText="Salary" Visible="true" ItemStyle-Width="120px" ItemStyle-Height="28px">
                                            <ItemStyle Height="28px" Width="120px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EffectiveMonth" HeaderText="Effective Month" Visible="true" ItemStyle-Width="150px" ItemStyle-Height="28px">
                                            <ItemStyle Height="28px" Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EmpType" HeaderText="EmpType" Visible="false" ItemStyle-Width="84px" ItemStyle-Height="28px">
                                            <ItemStyle Height="28px" Width="84px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="DateofUpdate" HeaderText="DateofUpdate" Visible="false" ItemStyle-Width="84px" ItemStyle-Height="28px">
                                            <ItemStyle Height="28px" Width="84px"></ItemStyle>
                                        </asp:BoundField>                                     
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="deleterow" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Delete" Font-Bold="true" ForeColor="Red" OnClientClick="return confirm('Are you sure to delete?');"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                    </Columns>
                                </asp:GridView>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        $(document).ready(function () {
            $(document).on("keypress", "body", function (e)
            {
                if (e.keyCode== 13) e.preventDefault();
                // alert('deafault prevented');

            });
        });
        $(document).ready(function () {
            load();
            $("#txtIncrementAmount").keydown(function (e) {
                // Allow: backspace, delete, tab, escape, enter and .
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                    // Allow: Ctrl+A
                    (e.keyCode == 65 && e.ctrlKey === true) ||
                    // Allow: home, end, left, right, down, up
                    (e.keyCode >= 35 && e.keyCode <= 40)) {
                    // let it happen, don't do anything
                    return;
                }
                // Ensure that it is a number and stop the keypress
                if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });
        });
       

        function InputValidationBasket() {
            try {
                if ($('#txtEmpCardNo').val().trim().length < 8) {
                    showMessage("warning->Please type or select valid card no ");
                    $('#txtEmpCardNo').focus();
                    return false;
                }
                else if ($('#txtIncrementAmount').val().trim().length == 0) {
                    showMessage("warning->Please type the increment amount ");
                    $('#txtIncrementAmount').focus();
                    return false;
                }
                else if ($('#txtNewGross').val().trim().length == 0) {
                    showMessage("warning->Please type the increment amount then press enter ");
                    $('#txtNewGross').focus();
                    return false;
                }
                else if ($('#txtIncrementOrderRefNumber').val().trim().length == 0) {
                    showMessage("warning->Please type the  increment order reference number ");
                    $('#txtIncrementOrderRefNumber').focus();
                    return false;
                }
                else if ($('#txtIncrementOrderRefDate').val().trim().length == 0) {
                    showMessage("warning->Please select the  increment order reference date ");
                    $('#txtIncrementOrderRefDate').focus();
                    return false;
                }
                else if ($('#txtEffectiveFrom').val().trim().length == 0) {
                    showMessage("warning->Please select the  effective date ");
                    $('#txtEffectiveFrom').focus();
                    return false;
                }
            }
            catch (exception) {

            }
        }

        function ClearInputBox() {
            try {         
                $('#txtEmpName').val('');

                $('#txtLastIncrementDate').val('');
                $('#txtLastIncrementAmount').val('');
                $('#txtIncrementAmount').val('');
                $('#txtPresentBasic').val('');
                $('#txtPresentMedical').val('');
                $('#txtPresentHouseRent').val('');
                $('#txtPresentConveyance').val('');
                $('#txtPresentGrossSalary').val('');
                $('#txtNewBasic').val('');
                $('#txtNewMedical').val('');
                $('#txtNewHouseRent').val('');

                $('#txtPresentFoodAllowance').val('');
                $('#txtNewFoodAllowance').val('');

                $('#txtTransportOthers').val('');
                $('#txtNewTransportOthers').val('');

                $('#txtNewConveyance').val('');
                $('#txtNewGross').val('');
                $('#txtIncrementOrderRefNumber').val('');
                $('#txtIncrementOrderRefDate').val('');
                $('#txtEffectiveFrom').val('');
                $('#txtRemarks').val('');
                $('#hfSaveStatus').val('Save');
                $('#btnSave').val("Save");
                $('#ddlEmpCardNo').show();
                $('#txtEmpCardNo').show();
                $('#btnSearch').show();
                $('#hfDeleteStatus').val('0');
                $("#ddlEmpCardNo").val('0');
                $('#txtNewPF').val('');
                $('#txtPFAmount').val('');
                $('#txtNewTDS').val('');
                $('#txtTDS').val('');
                load();
            }
            catch (exception) {

            }
        }

        function totalSalaryCalculation(e) {
            try {
                if (e.keyCode == 13) {
                    if ($("#txtIncrementAmount").val() == "") {
                        showMessage("Enter Increment Amount", "error");
                        $("#txtIncrementAmount").focus();
                        return false;
                    }
                    //$('#hdfpresentsalryKeypress').val('Press');
                    SalaryCalculation();
                }
            }
            catch (exception) {

            }
        }
      
        //------------- Salary Calculation by Nayem---------------
        function SalaryCalculation() {
            try {
                var basic = 0;
                var medical = 0;
                var food = 0;
                var convayance = 0;
                var technical = 0;
                var houseRent = 0;
                var others = 0;
                var pf = 0;
                var TransportAndOther = 0;
                var GetGross =parseFloat( $('#<%=txtPresentGrossSalary.ClientID%>').val()) + parseFloat($('#<%=txtIncrementAmount.ClientID%>').val());

                $('#txtNewGross').val(GetGross);
                var salary_type = $('#<%=hdfSalaryType.ClientID%>').val().trim();

                if (salary_type == "Gross Scale") {                  

                    if ($('#<%=hfIsGarments.ClientID%>').val() == "0")                              // for others salary or textile salary 
                    {
                        //alert('jakson');
                        var GetGross = $('#<%=txtNewGross.ClientID%>').val();
                        if (GetGross.length == 0) GetGross = 0;
                        var ba = $('#hfBasicStatus').val();
                        if (ba == "0") // 0=%
                        {
                            var BP = ($('#hdfBasic').val().trim().length < 0) ? 0 : $('#hdfBasic').val();
                           // basic = parseFloat(GetGross) * parseFloat(BP) / 100;
                            basic = parseFloat(parseFloat(GetGross) * parseFloat(BP) / 100).toFixed(2);
                           // basic = Math.round(parseFloat(GetGross) * parseFloat(BP) / 100, 1);
                            $('#txtNewBasic').val(basic);
                            
                        }
                        else if (ba == "1") // 1=৳ 
                        {
                            basic = ('#<%=txtNewBasic.ClientID%>').val();
                        }
                        else {
                            basic = "0";
                            $('#txtNewBasic').val(basic);
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
                            $('#txtNewMedical').val(medical);
                        }
                        else if (ma == "3") // 3=% of basic
                        {
                            var MP = ($('#hdfMedical').val().trim().length < 0) ? 0 : $('#hdfMedical').val();
                           // medical = Math.round(parseFloat(basic) * parseFloat(MP) / 100, 0);
                            medical = parseFloat(parseFloat(basic) * parseFloat(MP) / 100).toFixed(2);

                            $('#txtNewMedical').val(medical);
                        }
                        else if (ma == "1") // 1=৳ 
                        {
                            medical = $('#txtNewMedical').val();                          
                        }
                        else {
                            medical = "0";
                            $('#txtNewMedical').val(medical);
                        }

                           

                        //-------------------End Medical Allowance Part-----------------------------------------------------------------------------
                        var fa = $('#hfFoodStatus').val();
                        if (fa == "0") // 0=%
                        {
                            var FP = ($('#hdfFoodAllowance').val().trim().length < 0) ? 0 : $('#hdfFoodAllowance').val();
                          //  food = Math.round(parseFloat(GetGross) * parseFloat(FP) / 100, 2);
                            food = parseFloat(parseFloat(GetGross) * parseFloat(FP) / 100).toFixed(2);
                            $('#txtNewFoodAllowance').val(food);
                        }
                       
                        else if (fa == "3") // 3=% of Basic
                        {
                            var FP = ($('#hdfFoodAllowance').val().trim().length < 0) ? 0 : $('#hdfFoodAllowance').val();
                           // food = Math.round(parseFloat(basic) * parseFloat(FP) / 100, 2);
                            food =  parseFloat(parseFloat(basic) * parseFloat(FP) / 100).toFixed(2);
                            $('#txtNewFoodAllowance').val(food);
                        }
                        else if (fa == "1") // 1=৳
                        {
                            food = ('#<%=txtNewFoodAllowance.ClientID%>').val();
                        }

                        else {
                            food = "0";
                            $('#txtNewFoodAllowance').val(medical);
                        }
                            

                        //-------------------End Food Allowance Part-----------------------------------------------------------------------------
                        var ca = $('#hfConveyanceStatus').val();
                        if (ca== "0") // 0=%
                        {
                            var CP = ($('#hdfConveyance').val().trim().length < 0) ? 0 : $('#hdfConveyance').val();
                           // convayance = Math.round(parseFloat(GetGross) * parseFloat(CP) / 100, 2);
                            convayance = parseFloat(parseFloat(GetGross) * parseFloat(CP) / 100).toFixed(2);
                            $('#txtNewConveyance').val(convayance);
                        }
                        else if (ca == "3") // 3=% of Basic
                        {
                            var CP = ($('#hdfConveyance').val().trim().length < 0) ? 0 : $('#hdfConveyance').val();
                            //convayance = Math.round(parseFloat(basic) * parseFloat(CP) / 100, 2);
                            convayance =parseFloat(parseFloat(basic) * parseFloat(CP) / 100).toFixed(2);
                            $('#txtNewConveyance').val(convayance);
                        }
                        else if (ca == "1") // 1=৳ 
                            convayance = $('#txtNewConveyance').val();
                        else
                        {
                            convayance = "0";
                            $('#txtNewConveyance').val(convayance);
                        }
                            

                        //-------------------End Conveyance Allowance Part-----------------------------------------------------------------------------                                        

                        var ta = $('#hfTechnicalStatus').val();
                        if (ta == "0") // 0=%
                        {
                            var TP = ($('#hdfTechnical').val().trim().length < 0) ? 0 : $('#hdfTechnical').val();
                           // technical = Math.round(parseFloat(GetGross) * parseFloat(TP) / 100, 2);
                            technical = parseFloat(parseFloat(GetGross) * parseFloat(TP) / 100).toFixed(2);
                            $('#txtNewTechnicalAllow').va(technical);

                        }
                        else if (ta == "3") // 3=% of Basic
                        {
                            var TP = ($('#hdfTechnical').val().trim().length < 0) ? 0 : $('#hdfTechnical').val();
                         //   technical = Math.round(parseFloat(basic) * parseFloat(TP) / 100, 2);
                            technical =parseFloat(parseFloat(basic) * parseFloat(TP) / 100).toFixed(2);
                            $('#txtNewTechnicalAllow').va(technical);

                        }
                        else if (ta == "1") // 1=৳ 
                        {
                            technical = $('#txtNewTechnicalAllow').val(); 

                        }                         
                        else
                        {
                            technical = "0";
                            $('#txtNewTechnicalAllow').val(technical);
                           
                        }
                            
                        //-------------------End Technical Allowance Part-----------------------------------------------------------------------------
                      
                        var ha = $('#hfHouseStatus').val();
                        if (ha == "0") // 0=%
                        {
                            var HP = ($('#hdfhouserent').val().trim().length < 0) ? 0 : $('#hdfhouserent').val();
                           // houseRent = Math.round(parseFloat(GetGross) * parseFloat(HP) / 100, 2);
                            houseRent = parseFloat(parseFloat(GetGross) * parseFloat(HP) / 100).toFixed(2);
                            $('#txtNewHouseRent').val(houseRent);
                        }
                        else if (ha == "3") // 3=% of Basic
                        {
                            var HP = ($('#hdfhouserent').val().trim().length < 0) ? 0 : $('#hdfhouserent').val();
                            //houseRent = Math.round(parseFloat(basic) * parseFloat(HP) / 100, 2);
                            houseRent =parseFloat(parseFloat(basic) * parseFloat(HP) / 100).toFixed(2);
                            $('#txtNewHouseRent').val(houseRent);
                        }
                        else if (ha == "1") // 1=৳ 
                            houseRent = $('#txtNewHouseRent').val();
                        else {
                            houseRent = "0";
                            $('#txtNewHouseRent').val(houseRent);
                        }

                        //-------------------End House Allowance Part-----------------------------------------------------------------------------

                        var oa = $('#hfOthersStatus').val();
                        if (oa == "0") // 0=%
                        {
                            var OP = ($('#hdfOthers').val().trim().length < 0) ? 0 : $('#hdfOthers').val();
                            others = Math.round(parseFloat(GetGross) * parseFloat(OP) / 100, 2);
                            $('#txtNewOthers').val(others);
                        }
                        else if (oa == "3") // 3=% of Basic
                        {
                            var OP = ($('#hdfOthers').val().trim().length < 0) ? 0 : $('#hdfOthers').val();
                            //others = Math.round(parseFloat(basic) * parseFloat(OP) / 100, 2);
                            others =parseFloat(parseFloat(basic) * parseFloat(OP) / 100).toFixed(2);
                            $('#txtNewOthers').val(others);
                        }
                        else if (oa == "1") // 1=৳ 
                            others = $('#txtNewOthers').val();
                        else
                        {
                            others = "0";
                            //others = Math.round(parseFloat(GetGross) - (parseFloat(basic) + parseFloat(houseRent) + parseFloat(medical)), 0);
                            $('#txtNewOthers').val(others);
                        }
                            

                        //-------------------End Others Allowance Part-------------------
                        var pfa = $('#hfPFStatus').val();
                        if (pfa == "0") // 0=%
                        {
                            var PFP = ($('#hdfPF').val().trim().length < 0) ? 0 : $('#hdfPF').val();
                            //pf = Math.round(parseFloat(basic) * parseFloat(PFP) / 100, 2);                           
                            pf = parseFloat(parseFloat(basic) * parseFloat(PFP) / 100).toFixed(2);                        
                            $('#txtNewPF').val(pf)
                        }
                        else if (pfa == "3") // 3=% of Basic
                        {
                            var PFP = ($('#hdfPF').val().trim().length < 0) ? 0 : $('#hdfPF').val();
                           // pf = Math.round(parseFloat(basic) * parseFloat(PFP) / 100, 2);
                            pf = parseFloat(parseFloat(basic) * parseFloat(PFP) / 100).toFixed(2);   
                            $('#txtNewPF').val(pf)
                        }
                        else if (pfa == "1") // 1=৳ 
                            pf = $('#txtNewPF.ClientID').val();
                        else
                        {
                            pf = "0";
                            $('#txtNewPF').val(pf)
                        }

                    }

                    //-------------------End Medical Allowance Part---------------------------------------------------------------------------------------
                }




            }
            catch (exception)
            { }
        }
        //------------------End-----------------------------------
        function editSalaryIncrement(getId) {
            try {

                
                $('#ddlEmpType').hide();
                $('#tdEmpType').text($('#r_' + getId + ' td:nth-child(4)').html());
                
                jx.load('/ajax.aspx?id=' + getId + '&todo=getSalaryIncrementInfo' + '&amount= ' + 0 + '&status=' + status + ' ', function (data) {

                    var getInfo = data.split("_");

                    alert(getId);
                    $('#hfEmpId').val(getInfo[0]);
                    $('#tdCardNo').text(getInfo[1]);
                    $('#ddlEmpCardNo').hide();
                    $('#txtEmpCardNo').hide();
                    $('#btnSearch').hide();
                    $('#txtEmpName').val(getInfo[2]);
                    $('#hfSalaryType').val(getInfo[3]);
                    $('#txtLastIncrementDate').val(getInfo[4]);
                    $('#txtLastIncrementAmount').val(getInfo[5]);
               

                    if (getInfo[3] == "Scale") {

                        $('#txtPresentBasic').val(getInfo[6]);
                        $('#txtPresentMedical').val(getInfo[7]);
                        $('#txtPresentHouseRent').val(getInfo[8]);
                        $('#txtPresentConveyance').val(getInfo[9]);
                        $('#txtPresentFoodAllowance').val(getInfo[17]);
                    }
                    else{
                        $('#txtPresentBasic').val('');
                        $('#txtPresentMedical').val('');
                        $('#txtPresentHouseRent').val('');
                        $('#txtPresentConveyance').val('');
                        $('#txtPresentFoodAllowance').val('');
                    }
                    $('#txtPresentGrossSalary').val(getInfo[10]);
                    $('#txtIncrementOrderRefNumber').val(getInfo[12]);
                    $('#txtIncrementOrderRefDate').val(getInfo[13]);
                    $('#txtEffectiveFrom').val(getInfo[14]);
                    $('#txtRemarks').val(getInfo[15]);
                    $('#hfEmpSN').val(getInfo[16]);
                  
                    $('#btnSave').val("View");
                    $('#hfSaveStatus').val("Update");
                });
            }
            catch (exception) {

            }
        }

        function getDeleteMessage() {
            try {
                var isdelete = confirm("Do you want to delete ?");
                if (isdelete) $('#hfDeleteStatus').val('1');
                else $('#hfDeleteStatus').val('0');

            }
            catch (exception) {
                showMessage(exception);
            }
        }

        function CloseWindowt() {
            window.close();
        }

        function goToNewTabandWindow(url) {
            load();
            window.open(url);

        }
        function load() {
            $("#ddlEmpCardNo").select2();           
        }

    </script>
</asp:Content>
