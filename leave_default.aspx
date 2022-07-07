<%@ Page Title="Leave" Language="C#" MasterPageFile="~/leave_nested.master" AutoEventWireup="true" CodeBehind="leave_default.aspx.cs" Inherits="SigmaERP.leave_default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
            <div class="col-md-12">
                <div class="ds_nagevation_bar" style="border-bottom:none;">
                     <div style="margin-top: 5px">
                           <ul>
                               <li><a href="/default.aspx">Dashboard</a></li>
                               <li> <a class="seperator" href="#">/</a></li>
                               <li> <a href="#" class="ds_negevation_inactive Lactive">Leave</a></li>
                           </ul>               
                     </div>
                </div>
             </div>
        </div>
    <div class="main-bg">
    <div class="col-lg-12" style="margin-top:10%">
             <div class="row">

                 <div class=" col-md-2"></div>

                 <div class="col-md-2" title="Leave Configuration"  runat="server" id="pLeaveConfig">
                     <a class="ds_leave_Basic_Text" href="/leave/LeaveConfig.aspx"> <img class="image_width_for_module" src="images/common/grade.ico" /><br /> Configuration </a>
                     
                 </div>
                 <div class=" col-md-2" title="All Holiday Setup" runat="server" id="pHolidaySetup">
                        <a class="ds_leave_Basic_Text" href="/leave/holyday.aspx"><img class="image_width_for_module" src="images/common/qualification.ico" /><br />Holiday Setup </a>
                 </div>
                 <div class=" col-md-2" title="Leave Application" runat="server" id="pLeaveApplication">
                      <a class="ds_leave_Basic_Text" href="/leave/aplication.aspx"><img class="image_width_for_module" src="images/common/application.ico" /><br />Application</a>
                 </div>
                    <div class=" col-md-2" title="Earn Leave Report" runat="server" id="pShortLeave">
                     <a class="ds_leave_Basic_Text" href="/leave/short_leave.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Short Leave </a> 
                 
                 </div>     
                 <div class=" col-md-2"></div>
             </div>

               <div class="row">
                 <div class=" col-md-2"></div>
                      <div class=" col-md-2" title="Pending Leave Approved" runat="server" id="pLeaveApporval">
                      <a class="ds_leave_Basic_Text" href="/leave/for_approve_leave_list.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Leave Approval</a>
                 </div>
                        <div class=" col-md-2" title="Pending Leave Approved" runat="server" id="pShortLeaveApproval">
                      <a class="ds_leave_Basic_Text" href="/leave/for_approve_shortleave_list.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Short Lv Approval</a>
                 </div>
                      <div class=" col-md-2 " title="All Leave List" runat="server" id="pLeaveList">
                     <a class="ds_leave_Basic_Text" href="/leave/all_leave_list.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />All Leave List</a>
                 </div>
                  <div class=" col-md-2 " title="Yearly Purpose Report" runat="server" id="pLeaveYearlyReport">
                      <a class="ds_leave_Basic_Text" href="/leave/yearly_leaveStatus_report.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Leave Status Reports</a> 
                 </div>
                   <div runat="server" visible="false">
                         <div class=" col-md-2 " title="Leave Balance Report" runat="server" id="pLeaveBalanceReport">
                    <a class="ds_leave_Basic_Text" href="/leave/leave_balance_report.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Balance Report</a> 
                 </div>  
                   </div>
                        
                 <div class=" col-md-2"></div>
             </div>  
        <div class="row">
                 <div class=" col-md-2"></div>
                      <div class=" col-md-2" title="Pending Leave Approved" runat="server" id="Div1">
                      <a class="ds_leave_Basic_Text" href="/leave/earnleavegeneration.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Earnleave Generation</a>
                 </div>                 
                  
                 
                        
                 <div class=" col-md-2"></div>
             </div>


               <div class="row" runat="server" id="row3" visible="false">
                 <div class=" col-md-2"></div>
                          <div class=" col-md-2" title="Official Purpose Report" runat="server" id="pOfficialPurposeLeave">
                     <a class="ds_leave_Basic_Text" href="/leave/company_purpose_leave_report.aspx"><img class="image_width_for_module" src="images/common/add document.ico" /><br />Office Purpose</a>                    
                 </div>
                 <%--<div class=" col-md-2 " title="Yearly Purpose Report" runat="server" id="pLeaveYearlyReport">
                      <a class="ds_leave_Basic_Text" href="/leave/yearly_leaveStatus_report.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Yearly Report</a> 
                 </div>--%>
                         <div class=" col-md-2" title="Earn Leave Generate" runat="server" id="pELGenerate">
                      <a class="ds_leave_Basic_Text" href="/leave/generation.aspx"><img class="image_width_for_module" src="images/common/generate.ico" /><br />EL Generate</a>                     
                 </div>
                 <div class=" col-md-2" title="Earn Leave Report" runat="server" id="pELReport">
                     <a class="ds_leave_Basic_Text" href="/leave/earn_leave_Report.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />EL Report </a> 
                 
                 </div>

                 
                 
              
                 <%--<div class=" col-md-2 " title="Maternity Application">
                      <a class="ds_leave_Basic_Text" href="/leave/MaternityLeaveApplication.aspx"><img class="image_width_for_module" src="images/common/application.ico" /><br />Ma. Application</a> 
                 </div>
                   <div class=" col-md-2 " title="Maternity Voucher">
                     <a class="ds_leave_Basic_Text" href="/leave/maternity.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Ma. Voucher</a> 
                 </div>--%>
                 <div class=" col-md-2"></div>
             </div>
    
       
    

</div></div>

</asp:Content>
