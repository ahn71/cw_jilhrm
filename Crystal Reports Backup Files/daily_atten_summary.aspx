<%@ Page Title="Daily Atten Summary" Language="C#" MasterPageFile="~/attendance_nested.Master" AutoEventWireup="true" CodeBehind="daily_atten_summary.aspx.cs" Inherits="SigmaERP.attendance.daily_atten_summary" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>   
     <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="main_box">
    	<div class="main_box_header">
            <h2>Daily Attendance Summary Report</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">

                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" />
                        <asp:AsyncPostBackTrigger ControlID="chkForAllCompany" />
                        
                    </Triggers>
                    <ContentTemplate>
                <div class="bonus_generation">
                   

                     <table  class="bonus_generation_table super_admin_option">           
                            
                         <tr id="trForCompanyList" runat="server">
                                <td>Company</td>
                                <td>
                                    <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static" CssClass="form-control select_width" Width="185px" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged" >
                                    </asp:DropDownList>
                                </td>
                             <td></td>
                           <td>
                               <asp:CheckBox ID="chkForAllCompany" runat="server" Text="For All Companies" AutoPostBack="True" OnCheckedChanged="chkForAllCompany_CheckedChanged"   />
                           </td>
                           </tr>
                            </tr>
                         
                                <tr>
                                    
                                    <td>&nbsp; Shift</td>
                                    <td>
                                        <asp:DropDownList ID="ddlShiftName" runat="server" ClientIDMode="Static" CssClass="form-control select_width" Width="185px" AutoPostBack="True" OnSelectedIndexChanged="ddlShiftName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                            </tr>
                         
                     </table>
                    
                </div>
                <div class="payroll_generation_box3">
                   <table>
                       <tr>
                          <td>Date &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                      
                           <td>
                               <asp:TextBox ID="dptDate" Width="174"  runat="server" PlaceHolder="Click For Calander" CssClass="form-control text_box_width_import" ></asp:TextBox>
                               <asp:CalendarExtender Format="dd-MM-yyyy" ID="txtFromDate_CalendarExtender" runat="server" TargetControlID="dptDate">
                               </asp:CalendarExtender>
                           </td>                
                       </tr>
                   </table>
                </div>
                  <div class="job_card_box3" id="divDepartmentList" runat="server" >

                    <div class="job_card_left">
                        <asp:ListBox ID="lstAll" Width="270" SelectionMode="Multiple"  Height="146" runat="server"></asp:ListBox>
                    </div>
                                <div class="id_card_center">
                        <table class="employee_table">                     
                              <tr>
                                    <td>
                                        <asp:Button ID="btnAddItem" Class="arrow_button" runat="server" Text=">" OnClick="btnAddItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnAddAllItem" Class="arrow_button" runat="server" Text=">>" OnClick="btnAddAllItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveItem" Class="arrow_button" runat="server" Text="<" OnClick="btnRemoveItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveAllItem" Class="arrow_button" runat="server" Text="<<" OnClick="btnRemoveAllItem_Click"  />
                                    </td>
                               </tr>
                        </table>
                    </div>
                    <div class="job_card_right">
                        <asp:ListBox ID="lstSelected" Width="270" SelectionMode="Multiple" Height="146" runat="server"></asp:ListBox>
                    </div>
                </div>
                <div class="payroll_generation_button">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold; width:139px; float:left">
                                            <asp:Label runat="server" ID="lblProcess" text="wait processing"></asp:Label>
                                        <img style="width:26px;height:24px;cursor:pointer; margin-right:-56px" src="/images/wait.gif"  />  
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                    <asp:Button ID="btnPreview" runat="server" CssClass="css_btn" Text="Preview" OnClientClick="return InputValidationBasket();" OnClick="btnPreview_Click"   />
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/attendance_default.aspx" CssClass="css_btn" />
                </div>
                
            </div>
                        </ContentTemplate>
                </asp:UpdatePanel>
        </div>
    </div>
     </div>

    <script type="text/javascript">
        function InputValidationBasket() {
            try {
                if ($('#txtGenerateMonth').val().trim().length <= 4) {
                    showMessage('Please select salary month', 'error');
                    $('#txtGenerateMonth').focus(); return false;
                }
                return true;
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }

        function CloseWindowt() {
            window.close();
        }

        function goToNewTabandWindow(url) {
            window.open(url);

        }

    </script>
</asp:Content>
