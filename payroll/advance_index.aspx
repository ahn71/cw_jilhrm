<%@ Page Title="Advance" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="advance_index.aspx.cs" Inherits="SigmaERP.payroll.advance_index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <div class="row">
            <div class="col-md-12">
               <div class="ds_nagevation_bar" style="border-bottom:none;">
                    
                           <ul>
                               <li><a href="/default.aspx">Dasboard</a></li>
                               <li> <a class="seperator" href="/hrd_default.aspx">/</a></li>
                               <li> <a href="/payroll_default.aspx">Payroll</a></li>
                               <li> <a class="seperator" href="/hrd_default.aspx">/</a></li>
                               <li> <a href="/hrd_default.aspx" class="ds_negevation_inactive Pactive">Advance</a></li>
                           </ul>               
                   
                </div>
             </div>
        </div>
    
      <%--  <img alt="" class="main_images" src="images/hrd.png">--%>
    <div class="main-bg">
         <div class="col-lg-12" style="margin-top:10%">
             <div class="row">

                 <div class=" col-md-2"></div>

                 <div class="col-md-2" title="Advance Entry">
                     <a class="ds_Settings_Basic_Text Pbox" href="/payroll/advance.aspx"> <img class="image_width_for_module" src="/images/common/advanceentry.ico" /><br /> Advance Entry Panel</a>
                     
                 </div>
                 <div class=" col-md-2" title="Advance Setting">
                        <a class="ds_Settings_Basic_Text Pbox" href="/payroll/advancsetting.aspx"><img class="image_width_for_module" src="/images/common/generate.ico" /><br />Advance Settings</a>
                 </div>
                 <div class=" col-md-2" title="Advance Reprot">
                      <a class="ds_Settings_Basic_Text Pbox" href="/payroll/advance_info.aspx"><img class="image_width_for_module" src="/images/common/advancereport.ico" /><br />Advance Reports</a>
                 </div>
                 <div class=" col-md-2" title="Employee Profile">
                     <a class="ds_Settings_Basic_Text Pbox" style="height: 155px;" href="#"></a>
                 </div>
                 <%-- <div class=" col-md-2" title="Employee Profile">
                       <img class="image_width_for_module" style="width: 86%" src="../images/common/blankImageForManu.png" /><br />
                   </div>--%>
                 <div class=" col-md-2"></div>
             </div>
</div>
        </div>
</asp:Content>
