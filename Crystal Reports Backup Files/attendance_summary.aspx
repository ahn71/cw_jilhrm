<%@ Page Title="Attendance Summary" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="attendance_summary.aspx.cs" Inherits="SigmaERP.attendance.attendance_summary" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .rb label {
            margin-left:5px;
            padding-right:8px;
        }
        .header{
            background-color:#27235c;
        }
        .gvdisplay th:nth-child(2),th:nth-child(3),th:nth-child(4),th:nth-child(5),th:nth-child(6){
            text-align:center;
        }
        .select_width1 {
            width: 96%;
            margin: 3px;
        } 
        .ajax__calendar_years{
            width:135px !important;
        }
        .ajax__calendar_body{
            width:135px !important;
            height:121px !important;
        }     
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>   
     <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
     <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li>/</li>
                    <li><a href="/attendance_default.aspx">Attendance</a></li>
                    <li>/</li>
                    <li><a href="#" class="ds_negevation_inactive Mactive">Attendance Summary</a></li>
                </ul>
            </div>
        </div>
    </div>
    <div class="main_box Mbox">
    	<div class="main_box_header MBoxheader">
            <h2>Attendance Summary</h2>
        </div>
    	<div class="employee_box_body">
        	<div class="employee_box_content">

                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                          <asp:AsyncPostBackTrigger ControlID="btnPrintPreview" />
                        <%--<asp:AsyncPostBackTrigger ControlID="ddlShiftName" />--%>
                        <%--<asp:AsyncPostBackTrigger ControlID="rblDptType" />--%>
                        <%--<asp:AsyncPostBackTrigger ControlID="rblReportType" />--%>
                        <asp:AsyncPostBackTrigger ControlID="rblPrintType" />
                    </Triggers>
                    <ContentTemplate>
                <div class="bonus_generation" style="width:61%; margin:0px auto;">
                    <h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center"></h1>
                     <div visible="true" runat="server" id="tblGenerateType" class="bonus_generation_table super_admin_option">
                        <div id="trForCompanyList" runat="server" class="form-inline" >
                            <div class="div_table_row" style="display:none" >
                            
                             <span class="cell colon_sym"></span>                            
                           <asp:RadioButtonList ID="rblReportType"  runat="server" RepeatDirection="Vertical" AutoPostBack="true" >
                                <asp:ListItem Selected="True" Value="0" Text=""></asp:ListItem>                    
                                </asp:RadioButtonList>
                               Daily Attendance Summary                      
                         </div>   
                          <%--  <div class="row">
                                 <div class=" col-md-8"></div>
                                <div class=" col-md-4">
                                     <div class="form-group att_shift">                                 
                                 <asp:RadioButtonList runat="server" ID="rblShiftNumber" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblShiftNumber_SelectedIndexChanged">
                                    <asp:ListItem Value="20" Selected="True" Text="Last 20"></asp:ListItem>
                                     <asp:ListItem Value="50"  Text="Last 50"></asp:ListItem>
                                     <asp:ListItem Value="00"  Text="All"></asp:ListItem>
                                </asp:RadioButtonList> 
                            </div>
                                </div>

                            </div>--%>

                            <div class="row">
                                <div class=" col-md-7">
                                    <div class="form-group att_shift">
                                        <label id="trComT" runat="server" style="width:80px">Company</label>
                                        <span id="trComR" runat="server">
                                            <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static" CssClass="form-control select_width" Width="256px" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                </div>

                                <div class=" col-md-5">
                                <div class="form-group att_date_picker">
                                    <label>Date</label>
                                    <asp:TextBox ID="txtFromDate" Width="174" runat="server" PlaceHolder="Click For Calander" CssClass="form-control select_width"></asp:TextBox>
                                    <asp:CalendarExtender Format="dd-MM-yyyy" ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate">
                                    </asp:CalendarExtender>
                                </div>
                                    </div>
                            </div>
                            <div class="row">
                                <div class=" col-md-7">
                                    <div class="form-group att_shift">
                                        <label style="width:80px">Shift</label>
                                        <asp:DropDownList ID="ddlShiftName" runat="server" ClientIDMode="Static" CssClass="form-control select_width" Width="256px" >
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                           
                            <div class="form-group" runat="server" visible="false">
                                <label>Card No</label>
                                <label>&nbsp;</label>
                                <asp:TextBox runat="server" ID="txtCardNo" CssClass="form-control select_width" Width="174px" ClientIDMode="Static" PlaceHolder="Card No" MaxLength="4"></asp:TextBox>
                            </div>
                            <div class="form-group" runat="server" visible="false">
                                <label>&nbsp;</label>
                                 <asp:LinkButton runat="server" ID="lnkNew" Text="New" ClientIDMode="Static" style="position: relative; top: 7px;" OnClientClick="inputBoxClear()"></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    
                </div>
                        <%--
                <div class="bonus_generation" style="width:61%; margin:0px auto;">
                   <div class="form-inline">
                      <div class="form-group">
                            <label>From Date</label>
                            <asp:TextBox ID="txtFromDate" Width="174"  runat="server" PlaceHolder="Click For Calander" CssClass="form-control select_width"></asp:TextBox>
                            <asp:CalendarExtender Format="dd-MM-yyyy" ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate">
                            </asp:CalendarExtender>
                        </div>
                        <div class="form-group">
                            <label></label>
                            <asp:TextBox Enabled="false" ID="dptDate" Width="174"   runat="server" PlaceHolder="Click For Calander"  CssClass="form-control select_width" Visible="False"></asp:TextBox>
                               <asp:CalendarExtender Format="dd-MM-yyyy" ID="txtFromDate_CalendarExtender" runat="server" TargetControlID="dptDate">
                               </asp:CalendarExtender>
                        </div>  
                        <div runat="server" id="divOptions" class="form-group" >
                            <asp:RadioButtonList ID="rblDptType"  runat="server" AutoPostBack="true" CssClass="rb" OnSelectedIndexChanged="rblDptType_SelectedIndexChanged" RepeatDirection="Horizontal" >
                                   <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
                                   <asp:ListItem Value="1">Partial</asp:ListItem>
                               </asp:RadioButtonList>
                        </div>  
                   </div>
                </div>
                        --%>
                <div class="id_card" style="background-color:white; width:61%;" id="divDepartmentList" visible="false" runat="server" >
                     <%--<div id="id_card" runat="server" class="id_card" style="background-color:white; width:61%;"  >--%>
                            <div class="id_card_left EilistL">
                                <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata EilistCec" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div class="id_card_center EilistC">
                                <table style="margin-top:0px;" class="employee_table">  
                        <asp:Button ID="Button1" Class="arrow_button" runat="server" Text=">" OnClick="Button1_Click"  />
                        <asp:Button ID="btnAddAllItem" Class="arrow_button" runat="server" Text=">>" OnClick="btnAddAllItem_Click"  />
                        <asp:Button ID="Button2" Class="arrow_button" runat="server" Text="<" OnClick="Button2_Click"  />
                        <asp:Button ID="btnRemoveAllItem" Class="arrow_button" runat="server" Text="<<" OnClick="btnRemoveAllItem_Click"  />
                                   
                    </div>
                   <div class="id_card_right EilistR">
                                <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata EilistCec"  ClientIDMode="Static" runat="server"></asp:ListBox>
                            </div>
                </div>
                        <div id="divDepartment" visible="false" runat="server" class="bonus_generation" style="width:61%; margin:0px auto;">
                    <div class="daily_absence_report_left">
                        <caption>
                            <p>
                                Available Departments</p>
                            <asp:ListBox ID="lstEmployees" runat="server" AutoPostBack="True" Height="146" SelectionMode="Multiple" Width="260"></asp:ListBox>
                        </caption>
                    </div>
                    <div class="daily_absence_report_middle">

                        <asp:Button ID="btnadditem" CssClass="next_button" runat="server" Text=">" />
                        <caption>
                            <br />
                            <asp:Button ID="btnaddall" runat="server" CssClass="next_button" Text="&gt;&gt;" />
                            <br />
                            <asp:Button ID="btnremoveitem" runat="server" CssClass="next_button" Text="&lt;" />
                            <br />
                            <asp:Button ID="btnremoveall" runat="server" CssClass="next_button" Text="&lt;&lt;" />
                        </caption>
                    </div>
                    <div class="daily_absence_report_right">
                        <caption>
                            <p>
                                Selected Department/s</p>
                            <asp:ListBox ID="lstSelectedEmployees" runat="server" Height="146" SelectionMode="Multiple" Width="260"></asp:ListBox>
                        </caption>

                    </div>
                </div>
             
                   <div class="bonus_generation" style="width:61%; margin:0px auto;">
                       <center>
                           <asp:RadioButtonList runat="server" ID="rblDptORGrp"  RepeatDirection="Horizontal" CssClass="rb"  >
                                    <asp:ListItem Text="By Depertment" Value="Dpt" Selected="True" ></asp:ListItem>
                                     <asp:ListItem Text="By Section" Value="Grp"></asp:ListItem>
                                </asp:RadioButtonList>       
                           </center>                                      
                   <table>

                       <tr>
                         
                                <td>
                                   <asp:Button ID="btnSearch" runat="server" CssClass="Mbutton" OnClick="btnSearch_Click" Text="Search" />
                               </td>
                           <td>
                                   <asp:Button ID="btnPrintPreview" runat="server" CssClass="Mbutton"  Text="Print Preview" OnClick="btnPrintPreview_Click" />
                               </td>  
                           <td>
                               <asp:RadioButtonList runat="server" ID="rblPrintType"  RepeatDirection="Horizontal" CssClass="rb" AutoPostBack="True" OnSelectedIndexChanged="rblPrintType_SelectedIndexChanged" >
                                    <asp:ListItem Text="For View" Value="View" Selected="True" ></asp:ListItem>
                                     <asp:ListItem Text="For Print" Value="Print"></asp:ListItem>
                                </asp:RadioButtonList>
                           </td>                            
                           <td>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static">
                            <ProgressTemplate>
                                <span style="font-family: 'Times New Roman'; font-size: 20px; margin-top: -14px; color: green; font-weight: bold; float: left">
                                    <p>Wait...&nbsp;</p>
                                </span>
                                <img style="width: 26px; height: 26px; cursor: pointer; float: left; margin-top: -13px;" src="/images/wait.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                           </td> 
                       </tr>            
                   </table>
                            
                       </div>
                        </ContentTemplate>
                </asp:UpdatePanel>
                <div class="bonus_generation" style="width:61%; margin:0px auto;">
          <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional" >
              <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                  <asp:AsyncPostBackTrigger ControlID="rblPrintType" />
              </Triggers>
              <ContentTemplate>
                   <div id="divsummary" class="datatables_wrapper" runat="server" style="width:100%; height:auto; max-height:500px;overflow:auto;overflow-x:hidden;"></div>
                                 
              </ContentTemplate>
          </asp:UpdatePanel>
                    </div>
                </div>
                    

<%--                         <div >
                             <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                 <Triggers>
                                     <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                                     <asp:AsyncPostBackTrigger ControlID="rblDptType" />
                                 </Triggers>
                                 <ContentTemplate>

                                
                                        </ContentTemplate>
                                 </asp:UpdatePanel>
                </div>--%>

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

        function inputBoxClear()
        {
            $('#txtCardNo').val('');
        }

    </script>    
</asp:Content>
