<%@ Page Title="Monthly In Time Out Time" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="monthly_in_out_report.aspx.cs" Inherits="SigmaERP.attendance.monthly_in_out_report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .division_table_leave1 {
            width: 100%;
        }
          .division_table_leave1 tr {
             height:35px;
          
        }
        /*#ContentPlaceHolder1_MainContent_tblGenerateType tr {
            height:50px;
            border:1px solid silver;
        }*/
        
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
         <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li>/</li>
                    <li><a href="/attendance_default.aspx">Attendance</a></li>
                    <li>/</li>
                    <li><a href="#" class="ds_negevation_inactive Mactive">Monthly Status</a></li>
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
    <div class="main_box Mbox">
        <div class="main_box_header MBoxheader">
            <h2>Monthly Attendance Status Report</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content">

                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" />
                        <asp:AsyncPostBackTrigger ControlID="rblReportType" />                        
                    </Triggers>
                    <ContentTemplate>
                        <div class="bonus_generation outduty-report-outer">
                            <center>                 
<h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center"></h1>
                    
                     <table runat="server" visible="true" id="tblGenerateType"  class="division_table_leave1">                                      
                                <tr id="trForCompanyList" runat="server" class="hidden-xs hidden-sm">
                                <td>Company</td>
                                    <td>&nbsp;:&nbsp;</td>
                                <td>
                                    <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static" CssClass="form-control select_width" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged" >
                                    </asp:DropDownList>
                                </td>   
                                     <td>Generate Type</td>
                                    <td>&nbsp;:&nbsp;</td>        
                                <td><asp:RadioButtonList ID="rblGenerateType" runat="server" RepeatDirection="Horizontal" Font-Bold="true" AutoPostBack="True" OnSelectedIndexChanged="rblGenerateType_SelectedIndexChanged">
                                   <asp:ListItem Text="All" Value="0" Selected="True"></asp:ListItem>
                                   <asp:ListItem Text="Individual" Value="1"></asp:ListItem>
                               </asp:RadioButtonList></td>                                                
                           </tr>                    
                       <tr>
                          <td>Select Month</td>
                      <td>&nbsp;:&nbsp;</td>
                           <td>
                               <asp:DropDownList ID="ddlMonthList" runat="server" ClientIDMode="Static" CssClass="form-control select_width" >
                                    </asp:DropDownList>
                           </td>
                           
                               <td class="hidden-xs hidden-sm">Card No</td>
                                <td class="hidden-xs hidden-sm">&nbsp;:&nbsp;</td>   
                               <td class="hidden-xs hidden-sm">
                               <table>                                 
                                      <td>                                 
                                        <asp:TextBox ID="txtCardNo"  runat="server" PlaceHolder=" For Individual " ClientIDMode="Static" CssClass="form-control text_box_width_import" Enabled="False" ></asp:TextBox>
                                          </td>
                                   <td>                                  
                                        &nbsp;<asp:LinkButton ID="lnkNew" runat="server" Text="New" OnClientClick="InputBoxNew()"></asp:LinkButton>                              
                                    </td>
                               </table>
                            </td>
                               
                           
                          
                           
                       </tr>
                         <br />
                           <tr >
                                <td>Report Type </td>
                                <td>&nbsp;:&nbsp;</td>
                           <td colspan="5" class="chk-table td-xs-hidden">
                               <asp:RadioButtonList ID="rblReportType" runat="server" RepeatDirection="Horizontal" Font-Bold="true" AutoPostBack="True"  >
                                   <asp:ListItem Text="Log in-out" Value="0" Selected="True"></asp:ListItem>
                                   <asp:ListItem Text="Attendance status" Value="1"></asp:ListItem>
                                   <asp:ListItem Text="Attendance status with summary" Value="2"></asp:ListItem>
                                   <asp:ListItem Text="Job Card" Value="3"></asp:ListItem>
                                   <asp:ListItem Text="Only W & H" Value="4"></asp:ListItem>
                               </asp:RadioButtonList>
                           </td>
                            </tr>
                                   <tr runat="server" id="trEmpType" class="hidden-xs hidden-sm"> 
                                       <td>Employee Type</td> 
                                        <td>&nbsp;:&nbsp;</td>                                                     
                            <td colspan="5" >
                                <asp:RadioButtonList runat="server" ID="rblEmpType" AutoPostBack="true" RepeatDirection="Horizontal" >
                                </asp:RadioButtonList>
                            </td>
                         </tr> 
                   </table>
                          </center>
                        </div>
                        <center>
                       <%-- <table>
                            <tr>
                                <td>Report Type </td>
                                <td>&nbsp;:&nbsp;</td>
                           <td>
                               <asp:RadioButtonList ID="rblReportType" runat="server" RepeatDirection="Horizontal" Font-Bold="true" AutoPostBack="True"  >
                                   <asp:ListItem Text="Log in-out" Value="0" Selected="True"></asp:ListItem>
                                   <asp:ListItem Text="Attendance status" Value="1"></asp:ListItem>
                                   <asp:ListItem Text="Attendance summary" Value="2"></asp:ListItem>
                               </asp:RadioButtonList>
                           </td>
                            </tr>
                                   <tr> 
                                       <td>Employee Type</td> 
                                        <td>&nbsp;:&nbsp;</td>                                                     
                            <td >
                                <asp:RadioButtonList runat="server" ID="rblEmpType" AutoPostBack="true" RepeatDirection="Horizontal" >
                                </asp:RadioButtonList>
                            </td>
                         </tr>  
                         
                        </table> --%>                           
                              
                 <div id="workerlist" runat="server" class="id_card" style="background-color:white; width:61%;">
                            <div class="id_card_left EilistL">
                                <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata EilistCec" style="height:270px !important" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div class="id_card_center EilistC">    
                      <table style="margin-top:60px;" class="employee_table">                
                              <tr>
                                    <td >
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
                   <div class="id_card_right EilistR">
                                <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata EilistCec" style="height:270px !important"  ClientIDMode="Static" runat="server"></asp:ListBox>
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
                      <div runat="server" visible="false">
                                <center>                                                               
                                    <asp:RadioButtonList class="rb" ID="rblLanguage" runat="server"  RepeatDirection="Horizontal" >
                                    <asp:ListItem Text="English" Value="EN" Selected="True" ></asp:ListItem>
                                     <asp:ListItem Text="Bangla" Value="BN"></asp:ListItem>
                                </asp:RadioButtonList>
                                    </center>
                            </div>
                    <asp:Button ID="btnPreview" runat="server" CssClass="Mbutton" Text="Preview" OnClientClick="return InputValidationBasket();" OnClick="btnPreview_Click"   />
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/leave_default.aspx" CssClass="Mbutton" />
                </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#ddlShiftName").select2();
        });
        $(document).keyup(function (e) {
            if (e.keyCode == 79) {
                goToNewTabandWindow('/attendance/job_card.aspx');
            }
        });
        function load() {
            $("#ddlShiftName").select2();
        }
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
            load();

        }

        function getSalaryMonth() {

            var val = document.getElementById('ddlMonthID').value;
            document.getElementById('txtMonthId').value = val;

        }

        function CloseWindowt() {
            window.close();
        }

        function goToNewTabandWindow(url) {
            window.open(url);

        }

        function InputBoxNew() {
            $('#txtCardNo').val('');
        }

    </script>
</asp:Content>
