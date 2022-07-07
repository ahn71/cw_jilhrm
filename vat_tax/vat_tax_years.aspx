<%@ Page Title="Vat & Tax Years" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="vat_tax_years.aspx.cs" Inherits="SigmaERP.vat_tax.vat_tax_years" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #ContentPlaceHolder1_ContentPlaceHolder1_gvvatraxrateSettings th{
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
                     <li> <a href="#" class="ds_negevation_inactive Pactive">Tax Years Setup</a></li>
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
            <h2>Vat & Tax Years Setup</h2>
        </div>
    	<div class="main_box_body Pbody">
        	<div class="main_box_content">
                  <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />  
             <asp:AsyncPostBackTrigger ControlID="btnNew" />  
            <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" />          
        </Triggers>
        <ContentTemplate>
                <div class="input_division_info">
                   <div class="row">
                        <div class="col-sm-12">
                              <div class="row tbl-controlPanel">
                                <div class="col-sm-4">Company Name<span class="requerd1">*</span></div>
                                <div class="col-sm-8">
                                     <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" AutoPostBack="true" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged" >
                                                                        
                                      </asp:DropDownList>
                                </div>
                              </div>
                             <div class="row tbl-controlPanel">
                                <div class="col-sm-4">From Month<span class="requerd1">*</span></div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtFromMonth" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="MM-yyyy" TargetControlID="txtFromMonth">
                                    </asp:CalendarExtender> 
                                </div>
                              </div>
                              <div class="row tbl-controlPanel">
                                <div class="col-sm-4">To Month<span class="requerd1">*</span></div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtToMonth" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox> 
                                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="MM-yyyy" TargetControlID="txtToMonth">
                                    </asp:CalendarExtender>                               
                                </div>
                              </div>
                              <div class="row tbl-controlPanel">
                                <div class="col-sm-4">Generation Type<span class="requerd1">*</span></div>
                                <div class="col-sm-8">
                                    <asp:DropDownList runat="server" ClientIDMode="Static" ID="ddlGenerateType" CssClass="form-control text_box_width">                                        
                                    </asp:DropDownList>
                             <%--       <asp:TextBox ID="txtOrder"  runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers" 
                                TargetControlID="txtOrder" ValidChars=""></asp:FilteredTextBoxExtender>--%>
                                </div>
                              </div>                               
                              </div>               
                       <%-- </div>--%>
                    </div>

                  
                </div>
                
              
                <div class="button_area Rbutton_area">
                    <a href="#" onclick="window.history.back()" class="Pbutton">Back</a>
                    <asp:Button ID="btnNew" ClientIDMode="Static" CssClass="Pbutton"  runat="server" Text="New" />
                    <asp:Button ID="btnSave" OnClientClick="return validateInputs();"  ClientIDMode="Static" class="Pbutton" runat="server" Text="Save" OnClick="btnSave_Click"/>
                    <asp:Button ID="btnClose" ClientIDMode="Static" CssClass="Pbutton" PostBackUrl="~/hrd_default.aspx"  runat="server" Text="Close" />
                </div>
            </ContentTemplate>
                      </asp:UpdatePanel>

                  <div class="show_division_info">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">    
        <ContentTemplate>
                     <%--<div id="ShiftConfig" class="datatables_wrapper" runat="server" style="width:100%; height:auto; max-height:500px;overflow:auto;overflow-x:hidden;"></div>--%>
                    <asp:GridView ID="gvvatraxrateSettings" runat="server" CssClass="gvdisplay1"  Width="100%" AutoGenerateColumns="False" DataKeyNames="TaxId,CompanyId,OrderNo"  OnRowCommand="gvvatraxrateSettings_RowCommand"  AllowPaging="True" OnRowDataBound="gvvatraxrateSettings_RowDataBound"  >

<HeaderStyle BackColor="#FFA500" Font-Bold="True" Font-Size="14px" ForeColor="White" Height="28px"></HeaderStyle>
                        <PagerStyle CssClass="gridview Sgridview" Height="40px" />

                       <RowStyle HorizontalAlign="Center" />
                         <Columns>                     
  <%--                            <asp:BoundField DataField="CompanyName" HeaderStyle-HorizontalAlign="Left" HeaderText="Company Name" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="Left"  >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>

<ItemStyle HorizontalAlign="Left" Height="28px" ></ItemStyle>
                             </asp:BoundField>--%>
                            <asp:BoundField DataField="FromMonth" HeaderText="From Month" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center"   >
<ItemStyle Height="28px" ></ItemStyle>
                             </asp:BoundField>
                            <asp:BoundField DataField="ToMonth" HeaderText="To Month" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center" >
<ItemStyle Height="28px" ></ItemStyle>
                             </asp:BoundField>  
                              <asp:BoundField DataField="TaxYears" HeaderText="Tax Years" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center"  >
<ItemStyle Height="28px" ></ItemStyle>
                              </asp:BoundField>
                             <asp:BoundField DataField="Type" HeaderText="Generation" Visible="true" ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center">
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
    </script>
</asp:Content>

