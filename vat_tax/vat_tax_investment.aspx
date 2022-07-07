<%@ Page Title="" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="vat_tax_investment.aspx.cs" Inherits="SigmaERP.vat_tax.vat_tax_investment" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <%--<script src="../scripts/jquery-1.8.2.js"></script>--%>
        <script type="text/javascript">

            var oldgridcolor;
            function SetMouseOver(element) {
                oldgridcolor = element.style.backgroundColor;
                element.style.backgroundColor = '#ffeb95';
                element.style.cursor = 'pointer';
                // element.style.textDecoration = 'underline';
            }
            function SetMouseOut(element) {
                element.style.backgroundColor = oldgridcolor;
                // element.style.textDecoration = 'none';

            }

</script>
    <style>
        #ContentPlaceHolder1_ContentPlaceHolder1_gvvatraxrateSettings th,td {
            text-align:center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row Rrow">
                  <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dasboard</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="/payroll_default.aspx">Payroll</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li>  <a href="/vat_tax/vat_tax_index.aspx">Vat&Tax</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                     <li> <a href="#" class="ds_negevation_inactive Pactive">Vat_Rate Settings</a></li>
                </ul>
            </div>
        </div>
       </div>
  
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:HiddenField ID="hdnUpdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnbtnStage" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upSave" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upupdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="updelete" runat="server" ClientIDMode="Static" />
    <div class="main_box Mbox">
    	<div class="main_box_header PBoxheader">
            <h2>Investment Entry Panel</h2>
        </div>
    	<div class="main_box_body Pbody">
        	<div class="main_box_content">
                  <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />  
             <asp:AsyncPostBackTrigger ControlID="btnNew" />  
            <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" />   
             <asp:AsyncPostBackTrigger ControlID="ddlEmployeeList" />  
            <asp:AsyncPostBackTrigger ControlID="ddlInvstYear" />  
                 
        </Triggers>
        <ContentTemplate>
                <div class="input_division_info">
                   <div class="row">
                        <div class="col-sm-12">
                              <div class="row tbl-controlPanel">
                                <div class="col-sm-8">Company Name<span class="requerd1">*</span></div>
                                <div class="col-sm-4">
                                     <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" AutoPostBack="true" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged" >                                                                        
                                      </asp:DropDownList>
                                </div>
                              </div>
                              <div class="row tbl-controlPanel">
                                <div class="col-sm-8">Investment Year<span class="requerd1">*</span></div>
                                <div class="col-sm-4">
                                     <asp:DropDownList ID="ddlInvstYear" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" AutoPostBack="true" OnSelectedIndexChanged="ddlInvstYear_SelectedIndexChanged" >                                                                        
                                      </asp:DropDownList>
                                </div>
                              </div>
                                    <div class="row tbl-controlPanel">
                                <div class="col-sm-8">Employee<span class="requerd1">*</span></div>
                                <div class="col-sm-4">
                                     <asp:DropDownList ID="ddlEmployeeList" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" AutoPostBack="true" OnSelectedIndexChanged="ddlEmployeeList_SelectedIndexChanged">                                                                        
                                      </asp:DropDownList>
                                </div>
                              </div>
                            
                             <div class="row tbl-controlPanel">
                                <div class="col-sm-8">Life insurance premium (1)<span class="requerd1">*</span></div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtLifeInsurPremium" runat="server" ClientIDMode="Static" Text="0" CssClass="form-control text_box_width"></asp:TextBox>                                
                                </div>
                              </div>
                              <div class="row tbl-controlPanel">
                                <div class="col-sm-8">Contribution to deposit pension scheme (2)<span class="requerd1">*</span></div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtContrDepositPensionScheme" runat="server" ClientIDMode="Static" Text="0" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" 
                                TargetControlID="txtContrDepositPensionScheme" ValidChars=""></asp:FilteredTextBoxExtender>
                                </div>
                              </div>
                              <div class="row tbl-controlPanel">
                                <div class="col-sm-8">Investment in approved savings certificate (3)<span class="requerd1">*</span></div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtInvstInApprSavingsCertificate" runat="server" ClientIDMode="Static" Text="0" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers" 
                                TargetControlID="txtInvstInApprSavingsCertificate" ValidChars=""></asp:FilteredTextBoxExtender>
                                </div>
                              </div>
                               <div class="row tbl-controlPanel">
                                  <div class="col-sm-8">Investment in approved debenture or debenture stock, Stock or Shares (4)<span class="requerd1">*</span></div>
                                  <div class="col-sm-4">
                                    <asp:TextBox ID="txtInvstInApprDebentureOrDebentureStock_StockOrShares" runat="server" ClientIDMode="Static" Text="0" CssClass="form-control text_box_width"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers" 
                                    TargetControlID="txtInvstInApprDebentureOrDebentureStock_StockOrShares" ValidChars=""></asp:FilteredTextBoxExtender>
                                  </div>
                              </div>
                              <div class="row tbl-controlPanel">
                                  <div class="col-sm-8">Contribution to provident fund to which Provident Fund Act, 1925 applies (5)</div>
                                  <div class="col-sm-4">
                                       <asp:TextBox ID="txtContrPFWhichPFAct1925Applies" runat="server" ClientIDMode="Static" Text="0" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers" 
                                TargetControlID="txtContrPFWhichPFAct1925Applies" ValidChars=""></asp:FilteredTextBoxExtender>                             
                              </div>
                              </div>
                             <div class="row tbl-controlPanel">
                                <div class="col-sm-8">Contribution to Super Annuation Fund (6)<span class="requerd1">*</span></div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtContrSuperAnnuationFund" runat="server" ClientIDMode="Static" Text="0" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers" 
                                TargetControlID="txtContrSuperAnnuationFund" ValidChars=""></asp:FilteredTextBoxExtender>
                                </div>
                              </div>
                              <div class="row tbl-controlPanel">
                                <div class="col-sm-8">Contribution to Benevolent Fund and Group Insurance Premium (7)<span class="requerd1">*</span></div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtContrBenevolentFundAndGroupInsurPremium" runat="server" ClientIDMode="Static" Text="0" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Numbers" 
                                TargetControlID="txtContrBenevolentFundAndGroupInsurPremium" ValidChars=""></asp:FilteredTextBoxExtender>
                                </div>
                              </div>
                               <div class="row tbl-controlPanel">
                                  <div class="col-sm-8">Contribution to Zakat Fund (8)<span class="requerd1">*</span></div>
                                  <div class="col-sm-4">
                                    <asp:TextBox ID="txtContrZakatFund" runat="server" ClientIDMode="Static" Text="0" CssClass="form-control text_box_width"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterType="Numbers" 
                                    TargetControlID="txtContrZakatFund" ValidChars=""></asp:FilteredTextBoxExtender>
                                  </div>
                              </div>
                              <div class="row tbl-controlPanel">
                                  <div class="col-sm-8">Others, if any ( give details ) (9)<span class="requerd1">*</span></div>
                                  <div class="col-sm-4">
                                       <asp:TextBox ID="txtOthers" runat="server" ClientIDMode="Static" Text="0" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="Numbers" 
                                TargetControlID="txtOthers" ValidChars=""></asp:FilteredTextBoxExtender>
                             
                              </div>
                              </div>                         
                          
                        </div>
                    </div>                  
                </div>   
                <div class="button_area Rbutton_area">
                    <a href="#" onclick="window.history.back()" class="Pbutton">Back</a>
                    <asp:Button ID="btnNew" ClientIDMode="Static" CssClass="Pbutton"  runat="server" Text="New" OnClick="btnNew_Click" />
                    <asp:Button ID="btnSave" OnClientClick="return validateInputs();"  ClientIDMode="Static" class="Pbutton" runat="server" Text="Save" OnClick="btnSave_Click"/>
                    <asp:Button ID="btnClose" ClientIDMode="Static" CssClass="Pbutton" PostBackUrl="~/hrd_default.aspx"  runat="server" Text="Close" />
                </div>
            </ContentTemplate>
                      </asp:UpdatePanel>
                  <div class="show_division_info">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                     <%--<div id="ShiftConfig" class="datatables_wrapper" runat="server" style="width:100%; height:auto; max-height:500px;overflow:auto;overflow-x:hidden;"></div>--%>
                    <asp:GridView ID="gvvatraxrateSettings" runat="server" CssClass="gvdisplay1"  Width="100%" AutoGenerateColumns="False" DataKeyNames="SL,EmpId"  OnRowCommand="gvvatraxrateSettings_RowCommand" OnRowDataBound="gvvatraxrateSettings_RowDataBound"  AllowPaging="True"  >
<HeaderStyle BackColor="#FFA500" Font-Bold="True" Font-Size="14px" ForeColor="White" Height="28px"></HeaderStyle>
                        <PagerStyle CssClass="gridview Sgridview" Height="40px" />
                       <RowStyle HorizontalAlign="Center" />                         <Columns>                     
                              <asp:BoundField DataField="InvstYear"  HeaderText="Invst.Year" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center"  >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
<ItemStyle HorizontalAlign="Left" Height="28px" ></ItemStyle>
                             </asp:BoundField>
                            <asp:BoundField DataField="LifeInsurPremium" HeaderText="(1)" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center"   >
<ItemStyle Height="28px" ></ItemStyle>
                             </asp:BoundField>
                            <asp:BoundField DataField="ContrDepositPensionScheme" HeaderText="(2)" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center" >
<ItemStyle Height="28px" ></ItemStyle>
                             </asp:BoundField>  
                              <asp:BoundField DataField="InvstInApprSavingsCertificate" HeaderText="(3)" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center"  >
<ItemStyle Height="28px" ></ItemStyle>
                              </asp:BoundField>
                             <asp:BoundField DataField="InvstInApprDebentureOrDebentureStock_StockOrShares" HeaderText="(4)" Visible="true" ItemStyle-Height="28px">
                                 <ItemStyle Height="28px"></ItemStyle>
                             </asp:BoundField>
                             <asp:BoundField DataField="ContrPFWhichPFAct1925Applies" HeaderText="(5)" Visible="true" ItemStyle-Height="28px">
                                 <ItemStyle Height="28px"></ItemStyle>
                             </asp:BoundField>                     
                                <asp:BoundField DataField="ContrSuperAnnuationFund" HeaderText="(6)" Visible="true" ItemStyle-Height="28px">
                                 <ItemStyle Height="28px"></ItemStyle>
                             </asp:BoundField>
                             <asp:BoundField DataField="ContrBenevolentFundAndGroupInsurPremium" HeaderText="(7)" Visible="true" ItemStyle-Height="28px">
                                 <ItemStyle Height="28px"></ItemStyle>
                             </asp:BoundField> 
                                <asp:BoundField DataField="ContrZakatFund" HeaderText="(8)" Visible="true" ItemStyle-Height="28px">
                                 <ItemStyle Height="28px"></ItemStyle>
                             </asp:BoundField>
                             <asp:BoundField DataField="Others" HeaderText="(9)" Visible="true" ItemStyle-Height="28px">
                                 <ItemStyle Height="28px"></ItemStyle>
                             </asp:BoundField>                           
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAlter" runat="server" CommandName="Alter" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Edit" Font-Bold="true" ForeColor="Green" ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" >
                                <ItemTemplate>
                                     <asp:LinkButton ID="lnkDelete" runat="server" CommandName="deleterow" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Delete" Font-Bold="true" ForeColor="Red" OnClientClick="return confirm('Are you sure to delete?');" ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
         </ContentTemplate>
                           </asp:UpdatePanel>
                </div>				
            </div>
        </div>
    </div>
     <script type="text/javascript">
         $(document).ready(function () {
             $("#ddlEmployeeList").select2();
             $("#ddlInvstYear").select2();
         });
         function load() {
             $("#ddlEmployeeList").select2();
             $("#ddlInvstYear").select2();
            
         }
         //$('#btnNew').click(function () {
         //    clear();
         //});
         function validateInputs() {
             //if (validateText('txtDepartment', 1, 60, 'Enter Department Name') == false) return false;
             //if (validateText('txtDepartmentCode', 1, 2, 'Enter Valid Department Code') == false) return false;
             //if (validateText('txtDepartmentCode', 2, 2, 'Enter Department Code (Must be 2 Character)') == false) return false;
             return true;
         }

         function editDepartment(id) {
             var divsn = $('#r_' + id + ' td:first').html();
             var dropdownlistbox = document.getElementById("dlDivision")
             for (var x = 0; x < dropdownlistbox.length; x++) {
                 if (divsn == dropdownlistbox.options[x].text) {
                     dropdownlistbox.options[x].selected = true;

                 }
             }
             //$('#dlDivision').val(divsn);
             var depName= $('#r_' + id + ' td:nth-child(2)').html();
             $('#txtDepartment').val(depName);
             var depNameB = $('#r_' + id + ' td:nth-child(3)').html();
             $('#txtDepartmentBn').val(depNameB);
             //var depCode = $('#r_' + id + ' td:nth-child(4)').html();
             //$('#txtDepartmentCode').val(depCode);
             var depStatus = $('#r_' + id + ' td:nth-child(4)').html();
             $('#dlStatus').val(depStatus);

             if ($('#updelete').val() == '1') {
                 $('#btnDelete').addClass('css_btn');
                 $('#btnDelete').removeAttr('disabled');
             }
             if ($('#upupdate').val() == '1') {
                 $('#btnSave').val('Update');
                 $('#btnSave').addClass('css_btn');
                 $('#btnSave').removeAttr('disabled');
             }
             else {
                 $('#btnSave').val('Update');
                 $('#btnSave').removeClass('css_btn');
                 $('#btnSave').attr('disabled', 'disabled');
             }
             $('#hdnbtnStage').val(1);
             $('#hdnUpdate').val(id);
         }

         function deleteSuccess() {
             showMessage('Deleted successfully', 'success');
             $('#btnSave').val('Save');
             $('#hdnbtnStage').val("");
             $('#hdnUpdate').val("");
             clear();
         }
         function UpdateSuccess() {
             showMessage('Updated successfully', 'success');
             $('#btnSave').val('Save');
             $('#hdnbtnStage').val("");
             $('#hdnUpdate').val("");
             clear();
         }
         function SaveSuccess() {
             showMessage('Save successfully', 'success');
             $('#btnSave').val('Save');
             $('#hdnbtnStage').val("");
             $('#hdnUpdate').val("");
             clear();
         }


         function clear() {
             if ($('#upSave').val() == '0') {

                 $('#btnSave').removeClass('css_btn');
                 $('#btnSave').attr('disabled', 'disabled');
             }
             else {
                 $('#btnSave').addClass('css_btn');
                 $('#btnSave').removeAttr('disabled');
             }
             
             $('#txtDepartment').val('');
             $('#txtDepartmentBn').val('');
             $('#txtDepartmentCode').val('');
             $('#btnSave').val('Save');
             $('#hdnbtnStage').val("");
             $('#hdnUpdate').val("");
             $('#btnDelete').removeClass('css_btn');
             $('#btnDelete').attr('disabled', 'disabled');
             //$('#dlDivision option:selected').text('---Select---');
         }
         function validateInputs() {
        
             if (validateText('txtLifeInsurPremium', 1, 60, 'Please,fill up this field.') == false) return false;
             if (validateText('txtContrDepositPensionScheme', 1, 60, 'Please,fill up this field.') == false) return false;
             if (validateText('txtInvstInApprSavingsCertificate', 1, 60, 'Please,fill up this field.') == false) return false;
             if (validateText('txtInvstInApprDebentureOrDebentureStock_StockOrShares', 1, 60, 'Please,fill up this field.') == false) return false;
             if (validateText('txtContrPFWhichPFAct1925Applies', 1, 60, 'Please,fill up this field.') == false) return false;
             if (validateText('txtContrSuperAnnuationFund', 1, 60, 'Please,fill up this field.') == false) return false;
             if (validateText('txtContrBenevolentFundAndGroupInsurPremium', 1, 60, 'Please,fill up this field.') == false) return false;
             if (validateText('txtContrZakatFund', 1, 60, 'Please,fill up this field.') == false) return false;
             if (validateText('txtOthers', 1, 60, 'Please,fill up this field.') == false) return false;
             return true;
         }
    </script>
</asp:Content>
