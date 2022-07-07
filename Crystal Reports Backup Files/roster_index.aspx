<%@ Page Title="Roster Configuration" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="roster_index.aspx.cs" Inherits="SigmaERP.personnel.roster_index" %>
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
                               <li> <a href="#" class="ds_negevation_inactive Ptactive">Roster Configuration</a></li>
                           </ul>               
                    
                </div>
             </div>
        </div>
    <div style="background: transparent url('../../images/glossy-2.jpg') repeat scroll 0% 0%; position: absolute; width: 100%; left: 0px; height: 1200%;">
    <div class="col-lg-12" style="margin-top: 10%">
        <div class="row">


            <div class=" col-md-2"></div>
            <div class="col-md-2" title="New Employee Entry">
                <a class="ds_personnel_Basic_Text" href="/personnel/ShiftManagement.aspx">
                    <img class="image_width_for_module" src="../images/common/addemployee.ico" /><br />
                    Roster Create &nbsp;&nbsp; Panel</a>
            </div>
            <div class=" col-md-2" title="All Employee Details">
                <a class="ds_personnel_Basic_Text" href="/personnel/shift_roster_extend.aspx">
                    <img class="image_width_for_module" src="../images/common/employee detail.ico" /><br />
                    Roster Extend &nbsp;&nbsp; Panel</a>
            </div>
           <%-- <div class=" col-md-2" title="Employee Profile">
                <a class="ds_personnel_Basic_Text" href="/personnel/shift_roster_remove_single_date_employee.aspx">
                    <img class="image_width_for_module" src="../images/common/grade.ico" /><br />
                      Roster Add & Remove </a>
            </div>--%>
            <div class=" col-md-2" title="Employee List Report">
                <a class="ds_personnel_Basic_Text" href="/personnel/ShiftManageRemove.aspx">
                    <img class="image_width_for_module" src="../images/common/businesstype.ico" /><br />
                    Roster View & Remove</a>
            </div>
             <div class=" col-md-2" title="Roster Transer Panel">
                <a class="ds_personnel_Basic_Text" href="/personnel/roster_transfer.aspx">
                    <img class="image_width_for_module" src="../images/common/employee detail.ico" /><br />
                    Roster Transfer Panel</a>
            </div> 
            
            <div class=" col-md-2"></div>
        </div>

        <div class="row">
             
            <div class=" col-md-2"></div>
             <div class=" col-md-2" title="All Employee Details">
                <a class="ds_personnel_Basic_Text" href="/personnel/roster_missing.aspx">
                    <img class="image_width_for_module" src="../images/common/employee detail.ico" /><br />
                    Roster Missing Panel</a>
            </div> 
            <div class=" col-md-2" title="Employee List Report">
                <a class="ds_personnel_Basic_Text" href="/personnel/FloorAssigne.aspx">
                    <img class="image_width_for_module" src="../images/common/businesstype.ico" /><br />
                   Place Assign  &nbsp;&nbsp; &nbsp;&nbsp;Panel</a>
            </div>
            <div class="col-md-2" title="New Employee Entry">
                <a class="ds_personnel_Basic_Text" href="/personnel/shift_manage_report.aspx">
                    <img class="image_width_for_module" src="../images/common/addemployee.ico" /><br />
                    Roster Manage Report</a>
            </div>
            <div class=" col-md-2" title="All Employee Details">
                <a class="ds_personnel_Basic_Text" href="/personnel/shift_manage_reportByDateRange.aspx">
                    <img class="image_width_for_module" src="../images/common/employee detail.ico" /><br />
                    Roster Report By Date Range</a>
            </div>
            
                     
            <div class="col-md-2"></div>
        </div>

    </div></div>
</asp:Content>
