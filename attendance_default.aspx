<%@ Page Title="Attendance" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="attendance_default.aspx.cs" Inherits="SigmaERP.attendance_default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar" style="border-bottom: none;">
                <div style="margin-top: 5px">
                    <ul>
                        <li><a href="/default.aspx">Dashboard</a></li>
                        <li><a class="seperator" href="/hrd_default.aspx">/</a></li>
                        <li><a href="#" class="ds_negevation_inactive Mactive">Attendance</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <div class="main-bg">
        <div class="col-lg-12" style="margin-top: 10%">
            <div class="row">

                <div class=" col-md-2"></div>

                <div class="col-md-2" title="Attendance Month Setup" runat="server" id="pMonthSetup">
                    <a class="ds_attendance_Basic_Text" href="/attendance/monthly_setup.aspx">
                        <img class="image_width_for_module" src="images/common/grade.ico" /><br />
                        Month Setup </a>

                </div>
                <div class=" col-md-2" title="Machine Data Import" runat="server" id="pShrinkData">
                    <a class="ds_attendance_Basic_Text" href="/attendance/import_data_ahg.aspx">
                        <img class="image_width_for_module" src="images/common/generate.ico" /><br />
                        Shrink Data</a>
                </div>
                <%--<div class="col-md-2" title="Daily Logout Setup" >
                     <a class="ds_attendance_Basic_Text" href="/attendance/monthly_logout_setup.aspx"> <img class="image_width_for_module" src="images/common/grade.ico" /><br /> Logout Setup</a>
                     
                 </div>
                 <div class=" col-md-2" title="Attendance Late Deduction">
                        <a class="ds_attendance_Basic_Text" href="/attendance/late_deduction.aspx"><img class="image_width_for_module" src="images/common/qualification.ico" /><br />Late Deduction</a>
                 </div>
                   <div class=" col-md-2" title="Attendance Late Deduction">
                        <a class="ds_attendance_Basic_Text" href="/attendance/employee-wise_hw_setup.aspx"><img class="image_width_for_module" src="images/common/qualification.ico" /><br />Emp Weekend</a>
                 </div>--%>
                <div class=" col-md-2" title="Attendance Manually Count " runat="server" id="pManuallyCount">
                    <a class="ds_attendance_Basic_Text" href="/attendance/attendance.aspx">
                        <img class="image_width_for_module" src="images/common/application.ico" /><br />
                        Manually Count</a>
                </div>
                <div class=" col-md-2" title="All Attendance List" runat="server" id="pAttendanceList">
                    <a class="ds_attendance_Basic_Text" href="/attendance/attendance_list.aspx">
                        <img class="image_width_for_module" src="images/common/businesstype.ico" /><br />
                        Attendance List</a>
                </div>
                <div class=" col-md-2"></div>
            </div>
            <div class="row">
                <div class=" col-md-2"></div>
                <div class=" col-md-2" title="Daily Attendance Summary" runat="server" id="pAttSummary">
                    <a class="ds_attendance_Basic_Text" href="/attendance/attendance_summary.aspx">
                        <img class="image_width_for_module" src="images/common/businesstype.ico" /><br />
                        Att Summary </a>

                </div>
                <div class=" col-md-2 " title="Daily In-Out Report" runat="server" id="pAttInOutReport">
                    <a class="ds_attendance_Basic_Text" href="/attendance/daily_movement.aspx">
                        <img class="image_width_for_module" src="images/common/businesstype.ico" /><br />
                        In-Out Report</a>
                </div>
                <div class=" col-md-2 " title="Todays Attendance Stutus" runat="server" id="pAttManualReport">
                    <a class="ds_attendance_Basic_Text" href="/attendance/daily_manualAttendance_report.aspx">
                        <img class="image_width_for_module" src="images/common/businesstype.ico" /><br />
                        Manual Report</a>
                </div>
                <%--<div class=" col-md-2 " title="Daily Early Late Out Report">
                    <a class="ds_attendance_Basic_Text" href="/attendance/early_out_late_out.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Early In-Out</a> 
                 </div>--%>

                <div class=" col-md-2" title="Monthly Attendance Status" runat="server" id="pAttMonthlyStatus">
                    <a class="ds_attendance_Basic_Text" href="/attendance/monthly_in_out_report.aspx">
                        <img class="image_width_for_module" src="images/common/add document.ico" /><br />
                        Monthly Status</a>
                </div>
                <div class=" col-md-2"></div>
            </div>

            <div class="row">
                <div class=" col-md-2"></div>
                <div class=" col-md-2 " title="Out Duty" runat="server" id="pOutDuty">
                    <a class="ds_attendance_Basic_Text" href="/attendance/out_duty.aspx">
                        <img class="image_width_for_module" src="images/common/businesstype.ico" /><br />
                        Out Duty</a>
                </div>
                  <div class=" col-md-2 " title="Out Duty" runat="server" id="pOutDutyList">
                    <a class="ds_attendance_Basic_Text" href="/attendance/out_duty_list.aspx">
                        <img class="image_width_for_module" src="images/common/businesstype.ico" /><br />
                        Out Duty List</a>
                </div>
                  <div class=" col-md-2 " title="Out Duty" runat="server" id="pOutDutyReport">
                    <a class="ds_attendance_Basic_Text" href="/attendance/outduty_report.aspx">
                        <img class="image_width_for_module" src="images/common/businesstype.ico" /><br />
                        Out Duty Report</a>
                </div>
                <div class=" col-md-2 " title="Manpower Wise Attendance Report" runat="server" id="pAttManpowerWise" visible="false">
                    <a class="ds_attendance_Basic_Text" href="/attendance/attendance_summary_manpower.aspx">
                        <img class="image_width_for_module" src="images/common/businesstype.ico" /><br />
                        Manpower Wise Attendance</a>
                </div>
                <div class=" col-md-2 " title="Overtime Report" runat="server" id="pOverTimeReport">
                    <a class="ds_attendance_Basic_Text" href="/attendance/overtime_report.aspx">
                        <img class="image_width_for_module" src="images/common/businesstype.ico" /><br />
                        Overtime Report</a>
                </div>
                <%-- <div class=" col-md-2 " title="Job Card">
                      <a class="ds_attendance_Basic_Text" href="/attendance/job_card_with_summary.aspx"><img class="image_width_for_module" src="images/common/application.ico" /><br />Job Card</a> 
                 </div> --%>
                <div class=" col-md-2"></div>
            </div>




        </div>
    </div>
</asp:Content>
