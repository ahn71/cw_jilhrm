<%@ Page Title="Employee Information" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="employee_index.aspx.cs" Inherits="SigmaERP.personnel.EmpInfo_Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <div class="row">
            <div class="col-md-12">
                <div class="ds_nagevation_bar" style="border-bottom:none;">
                   
                           <ul>
                               <li><a href="/default.aspx">Dasboard</a></li>
                               <li> <a class="seperator" href="#">/</a></li>
                               <li> <a href="/personnel_defult.aspx">Personnel</a></li>
                               <li> <a class="seperator" href="#">/</a></li>
                               <li> <a href="#" class="ds_negevation_inactive Ptactive">Employee Information</a></li>
                           </ul>               
                    
                </div>
             </div>
        </div>
    <div style="background: transparent url('../../images/glossy-2.jpg') repeat scroll 0% 0%; position: absolute; width: 100%; left: 0px; height: 1200%;">
    <div class="col-lg-12" style="margin-top: 10%">
        <div class="row">


            <div class=" col-md-2"></div>
            <div class="col-md-2" title="New Employee Entry" runat="server" id="pEmployee" >
                <a class="ds_personnel_Basic_Text" href="/personnel/employee.aspx">
                    <img class="image_width_for_module" src="../images/common/addemployee.ico"/><br />
                    Employee Entry Panel</a>

            </div>
            <div class=" col-md-2" title="All Employee Details" runat="server" id="pEmployeeList">
                <a class="ds_personnel_Basic_Text" href="/personnel/employee_list.aspx">
                    <img class="image_width_for_module" src="../images/common/businesstype.ico" /><br />
                    All Employee List</a>
            </div>
            <div class=" col-md-2" title="Employee Profile" runat="server" id="pEmployeeProfile">
                <a class="ds_personnel_Basic_Text" href="/personnel/employee_profile.aspx">
                    <img class="image_width_for_module" src="../images/common/grade.ico" /><br />
                    Employee Profile Report</a>
            </div>
            <div class=" col-md-2" title="Employee List Report" runat="server" id="pEmployeeListReport">
                <a class="ds_personnel_Basic_Text" href="/personnel/employee_information.aspx">
                    <img class="image_width_for_module" src="../images/common/businesstype.ico" /><br />
                    Employee List Report</a>
            </div>
            <div class=" col-md-2"></div>
        </div>



        <div class="row">

            <div class=" col-md-2"></div>

            <div class=" col-md-2" title="Employee Seperation" runat="server" id="pSeperation">
                <a class="ds_personnel_Basic_Text" href="/personnel/separation.aspx">
                    <img class="image_width_for_module" src="../images/common/add document.ico" /><br />
                    Seperation Entry Panel</a>

            </div>
            <div class=" col-md-2" title="Employee Seperation Report" runat="server" id="pSeperationListReport">

                <a class="ds_personnel_Basic_Text" href="/personnel/seperation_sheet.aspx">
                    <img class="image_width_for_module" src="../images/common/businesstype.ico" /><br />
                    Seperation List Report</a>
            </div>
            
            <div class=" col-md-2" title="Provident Fund Report" runat="server" id="pManPowerStatusReport">

                <a class="ds_personnel_Basic_Text" href="/personnel/man_power_status.aspx">
                    <img class="image_width_for_module" src="../images/common/businesstype.ico" /><br />
                    Man Power Status Report</a>
            </div>
             <div class=" col-md-2" title="Provident Fund Report" runat="server" id="pMonthlyManPowerReport">

                <a class="ds_personnel_Basic_Text" href="/personnel/monthly_manpower.aspx">
                    <img class="image_width_for_module" src="../images/common/businesstype.ico" /><br />
                    Monthly Man Power Report</a>
            </div>
            <div class=" col-md-2"></div>
        </div>
        <div class="row">

            <div class=" col-md-2"></div>
            <div class=" col-md-2" title="Blood Group Report" runat="server" id="pEmpContactListReport">

                <a class="ds_personnel_Basic_Text" href="/personnel/EmpContactReport.aspx">
                    <img class="image_width_for_module" src="../images/common/Allowance.ico" /><br />
                    Contact List Report </a>
            </div>
            <div class=" col-md-2" title="Staff ID Card" runat="server" id="pEmpIDCardReport">
                <a class="ds_personnel_Basic_Text" href="/personnel/staff_id_card.aspx">
                    <img class="image_width_for_module" src="../images/common/employee detail.ico" /><br />
                      Emp. ID Card Report</a>

            </div>
          <%--  <div class=" col-md-2" title="Worker ID Card">

                <a class="ds_personnel_Basic_Text" href="/personnel/worker_id_card.aspx">
                    <img class="image_width_for_module" src="../images/common/employee detail.ico" /><br />
                    Worker ID Card Report</a>
            </div>--%>
            <div class=" col-md-2" title="Blood Group Report" runat="server" id="pBloodGroupReport">
                <a class="ds_personnel_Basic_Text" href="/personnel/blood_group.aspx">
                    <img class="image_width_for_module" src="../images/common/add document.ico" /><br />
                    Blood Group Report</a>
            </div>           
            <div class=" col-md-2"></div>
        </div>
        </div>
        </div>
</asp:Content>
