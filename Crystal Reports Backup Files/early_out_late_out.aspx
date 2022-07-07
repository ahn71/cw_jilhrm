<%@ Page Title="Early In-Out" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="early_out_late_out.aspx.cs" Inherits="SigmaERP.All_Report.Attendance.early_out_late_out" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .division_table_leave1 {
            width: 100%;
        }

            .division_table_leave1 tr {
                height: 40px;
            }
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
                    <li><a href="#" class="ds_negevation_inactive Mactive">Early In-Out</a></li>
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
            <h2>Early In-Out</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">

                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" />
                        
                        <asp:AsyncPostBackTrigger ControlID="ddlShiftName" />
                    </Triggers>
                    <ContentTemplate>
               

                <div class="bonus_generation" style="width:61%; margin:0px auto;">                
<h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center">You Have Not Any Access Permission!</h1>

                    <table runat="server" visible="true" id="tblGenerateType"  class="division_table_leave1">
                           <tr >
                       <td></td>
                            <td></td>
                               <td></td>
                               <td></td>
                               <td></td>
                            <td colspan="2">
                                <asp:RadioButtonList runat="server" ID="rblShiftNumber" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblShiftNumber_SelectedIndexChanged">
                                    <asp:ListItem Value="20" Selected="True" Text="Last 20"></asp:ListItem>
                                     <asp:ListItem Value="50"  Text="Last 50"></asp:ListItem>
                                     <asp:ListItem Value="00"  Text="All"></asp:ListItem>
                                </asp:RadioButtonList>  
                            </td>
  
                        </tr>
                        <tr id="trCompanyName" runat="server" visible="true">
                            <td>                               
                                Company
                            </td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static" CssClass="form-control select_width" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged">
                                    </asp:DropDownList>                       
                            </td>
                            <td>
                                Shift
                            </td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList ID="ddlShiftName" runat="server" ClientIDMode="Static" CssClass="form-control select_width" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlShiftName_SelectedIndexChanged" >
                               </asp:DropDownList> 
                            </td>
  
                        </tr>
                            <tr>
                            <td>                               
                                Date
                            </td>
                            <td>:</td>
                            <td>
                                  <asp:TextBox ID="txtFromDate"  runat="server" PlaceHolder="Click For Calander" ClientIDMode="Static" CssClass="form-control text_box_width_import" ></asp:TextBox>
                               <asp:CalendarExtender Format="dd-MM-yyyy" ID="txtFromDate_CalendarExtender" runat="server" TargetControlID="txtFromDate">
                               </asp:CalendarExtender>                       
                            </td>
                                <td>To Date</td>
                                <td>:</td>
                                <td><asp:TextBox ID="txtToDate"  runat="server" PlaceHolder="Click For Calander" ClientIDMode="Static" CssClass="form-control text_box_width_import" ></asp:TextBox>
                               <asp:CalendarExtender ID="txtToDate_CalendarExtender" Format="dd-MM-yyyy" runat="server" TargetControlID="txtToDate">
                               </asp:CalendarExtender></td>
                             
                        </tr>
                        <tr>
                            <td>Report Type</td>
                            <td>:</td>
                           <td>
                               <asp:RadioButtonList ID="rblReportType" runat="server" RepeatDirection="Horizontal" Font-Bold="true" >
                                   <asp:ListItem Text="Early Out" Value="0" Selected="True"></asp:ListItem>
                                   <asp:ListItem Text="Late Out" Value="1"></asp:ListItem>
                               </asp:RadioButtonList>
                           </td>
                             <td>
                                Card No
                            </td>
                            <td>:</td>
                            <td>
                                 <asp:TextBox ID="txtCardNo"  runat="server" PlaceHolder=" For Individual" ClientIDMode="Static" CssClass="form-control text_box_width_import" ></asp:TextBox>
                                        
                            </td>
                                <td> <asp:LinkButton ID="lnkNew" runat="server" Text="New" OnClientClick="InputBoxNew()"></asp:LinkButton></td>
                        </tr>
                      
                    </table>                     
                      <br /> 
                    <center>
                            <table>
                                   <tr>                             
                            <td >
                                <asp:RadioButtonList runat="server" ID="rblEmpType" AutoPostBack="true" RepeatDirection="Horizontal" >
                                </asp:RadioButtonList>
                            </td>
                         </tr>
                            </table>  
                        </center>  
                </div>
             
                 <div id="workerlist" runat="server" class="id_card" style="background-color:white; width:61%;">
                            <div class="id_card_left EilistL">
                                <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata EilistCec" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div class="id_card_center EilistC">
                                <table style="margin-top:0px;" class="employee_table">                      
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
                                <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata EilistCec"  ClientIDMode="Static" runat="server"></asp:ListBox>
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
        function load() {
            $("#ddlShiftName").select2();
        }
        function goToNewTabandWindow(url) {
            window.open(url);
            load();
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

        //function goToNewTabandWindow(url) {
        //    window.open(url);

        //}

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

        function InputBoxNew()
        {
            $('#txtCardNo').val('');
        }

    </script>
</asp:Content>
