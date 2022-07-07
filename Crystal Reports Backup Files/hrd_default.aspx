<%@ Page Title="Settings" Language="C#" MasterPageFile="~/hrd_nested.master" AutoEventWireup="true" CodeBehind="hrd_default.aspx.cs" Inherits="SigmaERP.hrd_default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="row">
            <div class="col-md-12">
                <div class="ds_nagevation_bar" style="border-bottom:0;">
                     <div style="margin-top: 5px">
                           <ul>
                               <li><a href="/default.aspx">Dasboard</a></li>
                               <li> <a class="seperator" href="/hrd_default.aspx">/</a></li>
                               <li> <a href="/hrd_default.aspx" class="ds_negevation_inactive Ractive">Settings</a></li>
                           </ul>               
                     </div>
                </div>
             </div>
        </div>
    
      <%--  <img alt="" class="main_images" src="images/hrd.png">--%>
        <div style="background: transparent url('../../images/glossy-2.jpg') repeat scroll 0% 0%; position: absolute; width: 100%; left: 0px; height: 1000%;">
         <div class="col-lg-12" style="margin-top:10%">
             <div class="row">

                 <div class=" col-md-2"></div>

                 <div class="col-md-2">
                     <a class="ds_Settings_Basic_Text" href="/hrd/department.aspx"> <img class="image_width_for_module" src="images/common/department.ico" /><br /> Department</a>
                     
                 </div>
                 <div class=" col-md-2">
                        <a class="ds_Settings_Basic_Text" href="/hrd/designation.aspx"><img class="image_width_for_module" src="images/common/designation.ico" /><br />Designation</a>
                 </div>
                 <div class=" col-md-2">
                      <a class="ds_Settings_Basic_Text" href="/hrd/grade_config.aspx"><img class="image_width_for_module" src="images/common/grade.ico" /><br />Grade</a>
                 </div>
                   <div class=" col-md-2">
                      <a class="ds_Settings_Basic_Text" href="/hrd/shift_config.aspx"><img class="image_width_for_module" src="images/common/Class Schedule.ico" /><br />Shift</a>
                 </div>
                 <div class=" col-md-2"></div>
             </div>

               <div class="row">

                 <div class=" col-md-2"></div>

                 <div class=" col-md-2  ">
                      <a class="ds_Settings_Basic_Text" href="/hrd/qualification.aspx"><img class="image_width_for_module" src="images/common/qualification.ico" /><br />Qualification</a> 
                    
                 </div>
                 <div class=" col-md-2  ">
                     <a class="ds_Settings_Basic_Text" href="/hrd/religion.aspx"><img class="image_width_for_module" src="images/common/religion.ico" /><br />Religion</a> 
                 
                 </div>
                 <div class=" col-md-2  ">

                     <a class="ds_Settings_Basic_Text" href="/hrd/district_Config.aspx"><img class="image_width_for_module" src="images/common/distric add.ico" /><br />District</a>
                 </div>
                   <div class=" col-md-2  ">

                    <a class="ds_Settings_Basic_Text" href="/hrd/Thana.aspx"><img class="image_width_for_module" src="images/common/thana add.ico" /><br />Thana</a> 
                 </div>
                 <div class=" col-md-2"></div>
             </div>
    
               <div class="row">

                 <div class=" col-md-2"></div>

                 <div class=" col-md-2  ">
                     <a class="ds_Settings_Basic_Text" href="/hrd/allowancesetup.aspx"><img class="image_width_for_module" src="images/common/add document.ico" /><br />Stamp Deduction</a> 
                   
                 </div>
                 <div class=" col-md-2  ">

                      <a class="ds_Settings_Basic_Text" href="/hrd/line_config.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Line/Group</a> 
                 </div>
                 <div class=" col-md-2  ">

                      <a class="ds_Settings_Basic_Text" href="/hrd/CompanyInfo.aspx"><img class="image_width_for_module" src="images/common/company.ico" /><br />Company</a> 
                 </div>
                   <div class=" col-md-2  ">

                     <a class="ds_Settings_Basic_Text" href="/hrd/others_settings.aspx"><img class="image_width_for_module" src="images/common/others.ico" /><br />Others</a> 
                 </div>
                 <div class=" col-md-2"></div>
             </div>
    
       
                  <div class="row">

                 <div class=" col-md-2"></div>

                 <div class=" col-md-2  ">
                     <a class="ds_Settings_Basic_Text" href="/hrd/floorConfig.aspx"><img class="image_width_for_module" src="images/common/add document.ico" /><br />Floor</a> 
                   
                 </div>
                 <div class=" col-md-2  ">

                      <a class="ds_Settings_Basic_Text" href="/hrd/business_type.aspx"><img class="image_width_for_module" src="images/common/businesstype.ico" /><br />Business Type</a> 
                 </div>
                <%-- <div class=" col-md-2  ">

                      <a class="ds_Settings_Basic_Text" href="/hrd/CompanyInfo.aspx"><img class="image_width_for_module" src="images/common/company.ico" /><br />Company</a> 
                 </div>
                   <div class=" col-md-2  ">

                     <a class="ds_Settings_Basic_Text" href="/hrd/others_settings.aspx"><img class="image_width_for_module" src="images/common/others.ico" /><br />Others</a> 
                 </div>--%>
                 <div class=" col-md-2"></div>
               </div>
    

</div></div>
</asp:Content>
