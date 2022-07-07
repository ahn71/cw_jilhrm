<%@ Page Title="Bonus" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="bonus_index.aspx.cs" Inherits="SigmaERP.payroll.bonus_index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <div class="row">
            <div class="col-md-12">
               <div class="ds_nagevation_bar" style="border-bottom:none;">
                   
                           <ul>
                               <li><a href="/default.aspx">Dasboard</a></li>
                               <li> <a class="seperator" href="#">/</a></li>
                               <li> <a href="/payroll_default.aspx">Payroll</a></li>
                               <li> <a class="seperator" href="#">/</a></li>
                               <li> <a href="#" class="ds_negevation_inactive Pactive">Bouns</a></li>
                           </ul>               
                    
                </div>
             </div>
        </div>
    
      <%--  <img alt="" class="main_/imagess" src="/imagess/hrd.png">--%>
    <div style="background: transparent url('../../images/glossy-2.jpg') repeat scroll 0% 0%; position: absolute; width: 100%; left: 0px; height: 1200%;">
         <div class="col-lg-12" style="margin-top:10%">
             <div class="row">

                 <div class=" col-md-2"></div>

                 <div class="col-md-2" title="Bonus Setup">
                     <a class="ds_Settings_Basic_Text Pbox" href="/payroll/bonus_setup.aspx"> <img class="images_width_for_module" src="/images/common/qualification.ico" /><br /> Bonus Setup</a>
                     
                 </div>
                 <div class=" col-md-2" title="Bonus Month Setup">
                        <a class="ds_Settings_Basic_Text Pbox" href="/payroll/bonus_monyh_setup.aspx"><img class="images_width_for_module" src="/images/common/qualification.ico" /><br />B. Month Setup</a>
                 </div>
                 <div class=" col-md-2" title="Bonus Generate">
                      <a class="ds_Settings_Basic_Text Pbox" href="/payroll/bonus_generation.aspx"><img class="images_width_for_module" src="/images/common/generate.ico" /><br />B. Generate</a>
                 </div>
                   <div class=" col-md-2" title="Bonus Rise & Fall">
                      <a class="ds_Settings_Basic_Text Pbox" href="/payroll/bonus_increase_decrease.aspx"><img class="images_width_for_module" src="/images/common/risefall.ico" /><br />B. Rise & Fall</a>
                 </div>
                 <div class=" col-md-2"></div>
             </div>

               <div class="row">

                 <div class=" col-md-2"></div>

                 <div class=" col-md-2" title="Bonus Sheet">
                      <a class="ds_Settings_Basic_Text Pbox" href="/payroll/bonus_sheet_Report.aspx"><img class="images_width_for_module" src="/images/common/businesstype.ico" /><br />Bonus Sheet</a> 
                    
                 </div>
                 <div class=" col-md-2" title="Bonus Summary">
                     <a class="ds_Settings_Basic_Text Pbox" href="/payroll/bonus_summary_report.aspx"><img class="images_width_for_module" src="/images/common/businesstype.ico" /><br />B. Summary</a> 
                 
                 </div>
                 <div class=" col-md-2" title="Bonus Miss Sheet">

                     <a class="ds_Settings_Basic_Text Pbox" href="/payroll/bonus_miss_sheet_Report.aspx"><img class="images_width_for_module" src="/images/common/businesstype.ico" /><br />B. Miss Sheet</a>
                 </div>
                   <%--<div class=" col-md-2" title="Employee Profile">
                       <img class="image_width_for_module" style="width: 86%" src="../images/common/blankImageForManu.png" /><br />
                   </div>--%>
                   <div class=" col-md-2" title="Employee Profile">
                       <a class="ds_Settings_Basic_Text Pbox" style="height: 155px;" href="#"></a>
                   </div>
                 <div class=" col-md-2"></div>
             </div>
</div>
        </div>
</asp:Content>
