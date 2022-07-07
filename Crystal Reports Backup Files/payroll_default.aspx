<%@ Page Title="Payroll" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="payroll_default.aspx.cs" Inherits="SigmaERP.payroll_nested1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    

     <div class="row">
            <div class="col-md-12">
                <div class="ds_nagevation_bar" style="border-bottom:none;">
                     <div style="margin-top: 5px">
                           <ul>
                               <li><a href="/default.aspx">Dasboard</a></li>
                               <li> <a class="seperator" href="#">/</a></li>
                               <li> <a href="#" class="ds_negevation_inactive Pactive">Payroll</a></li>
                           </ul>               
                     </div>
                </div>
             </div>
        </div>
    
      <%--  <img alt="" class="main_images" src="images/hrd.png">--%>
    <div style="background: transparent url('../../images/glossy-2.jpg') repeat scroll 0% 0%; position: absolute; width: 100%; left: 0px; height: 1000%;">
         <div class="col-lg-12" style="margin-top:10%">
             <div class="row">

                 <div class=" col-md-3"></div>

                 <div class="col-md-2">
                     <a class="ds_Settings_Basic_Text Pbox" href="/payroll/bonus_index.aspx"> <img class="image_width_for_module" src="images/common/department.ico" /><br /> Bonus</a>
                     
                 </div>
                 <div class=" col-md-2">
                        <a class="ds_Settings_Basic_Text Pbox" href="/payroll/advance_index.aspx"><img class="image_width_for_module" src="images/common/designation.ico" /><br />Advance</a>
                 </div>
                 <div class=" col-md-2">
                      <a class="ds_Settings_Basic_Text Pbox" href="/payroll/salary_index.aspx"><img class="image_width_for_module" src="images/common/grade.ico" /><br />Salary</a>
                 </div>
                 
                 <div class=" col-md-3"></div>
             </div>
             <div class="row">

                 <div class=" col-md-3"></div>

                 <div class="col-md-2">
                     <a class="ds_Settings_Basic_Text Pbox" href="/pf/pf_index.aspx"> <img class="image_width_for_module" src="images/common/department.ico" /><br /> Provident Fund</a>
                     
                 </div>
                 <div class=" col-md-2">
                        <a class="ds_Settings_Basic_Text Pbox" href="/vat_tax/vat_tax_index.aspx"><img class="image_width_for_module" src="images/common/designation.ico" /><br />Vat&Tax</a>
                 </div>     
                    
                 
                 <div class=" col-md-3"></div>
             </div>


</div></div>

</asp:Content>
