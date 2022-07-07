<%@ Page Title="Personnel" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="personnel_defult.aspx.cs" Inherits="SigmaERP.personnel_defult" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
      <div class="row">
            <div class="col-md-12">
                <div class="ds_nagevation_bar" style="border-bottom:none;">
                     <div style="margin-top: 5px">
                           <ul>
                               <li><a href="/default.aspx">Dashboard</a></li>
                               <li> <a class="seperator" href="#">/</a></li>
                               <li> <a href="#" class="ds_negevation_inactive Ptactive">Personnel</a></li>
                           </ul>               
                     </div>
                </div>
             </div>
        </div>
    
      <%--  <img alt="" class="main_images" src="images/hrd.png">--%>
    <div class="main-bg">
   <div class="col-lg-12" style="margin-top:10%">
             <div class="row">

                 <div class=" col-md-4"></div>

                 <div class="col-md-2">
                     <a class="ds_personnel_Basic_Text" href="/personnel/employee_index.aspx"> <img class="image_width_for_module" src="images/common/department.ico" /><br />Employee Information</a>
                     
                 </div>
                 <div class=" col-md-2">
                        <a class="ds_personnel_Basic_Text" href="/personnel/roster_index.aspx"><img class="image_width_for_module" src="images/common/designation.ico" /><br />Roster Configuration</a>
                 </div>             
                 <div class=" col-md-4"></div>
             </div>


</div>
</div>
</asp:Content>
