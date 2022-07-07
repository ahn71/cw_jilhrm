<%@ Page Title="Vat & Tax Rate Settings" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="vat_rate_settings.aspx.cs" Inherits="SigmaERP.vat_tax.vat_rate_settings" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script src="../scripts/jquery-1.8.2.js"></script>
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
            <h2>Vat & Tax Rate Settings</h2>
        </div>
    	<div class="main_box_body Pbody">
        	<div class="main_box_content">
                  <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />  
             <asp:AsyncPostBackTrigger ControlID="btnNew" />  
            <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" />   
            <asp:AsyncPostBackTrigger ControlID="rblTaxpayerType" />       
        </Triggers>
        <ContentTemplate>
                <div class="input_division_info">
                   <div class="row">
                        <div class="col-sm-12">
                              <div class="row tbl-controlPanel">
                                <div class="col-sm-4">Company Name<span class="requerd1">*</span></div>
                                <div class="col-sm-8">
                                     <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" AutoPostBack="true" >
                                                                        
                                      </asp:DropDownList>
                                </div>
                              </div>
                             <div class="row tbl-controlPanel">
                                <div class="col-sm-4">Slab Name<span class="requerd1">*</span></div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtSlabName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                
                                </div>
                              </div>
                              <div class="row tbl-controlPanel">
                                <div class="col-sm-4">From Taka<span class="requerd1">*</span></div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtFromTaka" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" 
                                TargetControlID="txtFromTaka" ValidChars=""></asp:FilteredTextBoxExtender>
                                </div>
                              </div>
                              <div class="row tbl-controlPanel">
                                <div class="col-sm-4">To Taka<span class="requerd1">*</span></div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txttoTaka" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers" 
                                TargetControlID="txttoTaka" ValidChars=""></asp:FilteredTextBoxExtender>
                                </div>
                              </div>
                               <div class="row tbl-controlPanel">
                                  <div class="col-sm-4">Income Tax Rate(%)<span class="requerd1">*</span></div>
                                  <div class="col-sm-8">
                                    <asp:TextBox ID="txtincometaxrate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers" 
                                    TargetControlID="txtincometaxrate" ValidChars=""></asp:FilteredTextBoxExtender>
                                  </div>
                              </div>
                              <div class="row tbl-controlPanel">
                                  <div class="col-sm-4">Order</div>
                                  <div class="col-sm-8">
                                       <asp:TextBox ID="txtOrder" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers" 
                                TargetControlID="txtOrder" ValidChars=""></asp:FilteredTextBoxExtender>
                             
                              </div>
                              </div>
                             <div class="row tbl-controlPanel">
                                  <div class="col-sm-4">Taxpayer Type</div>
                                  <div class="col-sm-8">
                                      <asp:RadioButtonList runat="server" ID="rblTaxpayerType" AutoPostBack="true" OnSelectedIndexChanged="rblTaxpayerType_SelectedIndexChanged" ClientIDMode="Inherit">
                                        <asp:ListItem Selected="True" Value="0">General</asp:ListItem>
                                        <asp:ListItem Value="1">Women and 65 above aged</asp:ListItem>
                                          <asp:ListItem Value="2">Retarded taxpayers</asp:ListItem>
                                        <asp:ListItem Value="3">Liberation war wounded gazeted freedom fighters</asp:ListItem>
                                      </asp:RadioButtonList>
                             
                              </div>
                              </div>
                            
                          
                        </div>
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
                    <asp:GridView ID="gvvatraxrateSettings" runat="server" CssClass="gvdisplay1"  Width="100%" AutoGenerateColumns="False" DataKeyNames="RSN,CompanyId,Taxpayer"  OnRowCommand="gvvatraxrateSettings_RowCommand" OnRowDataBound="gvvatraxrateSettings_RowDataBound"  AllowPaging="True"  >

<HeaderStyle BackColor="#FFA500" Font-Bold="True" Font-Size="14px" ForeColor="White" Height="28px"></HeaderStyle>
                        <PagerStyle CssClass="gridview Sgridview" Height="40px" />

                       <RowStyle HorizontalAlign="Center" />
                         <Columns>                     
                              <asp:BoundField DataField="CompanyName" HeaderStyle-HorizontalAlign="Left" HeaderText="Company Name" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="Left"  >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
<ItemStyle HorizontalAlign="Left" Height="28px" ></ItemStyle>
                             </asp:BoundField>
                            <asp:BoundField DataField="SlabName" HeaderText="Slab Name" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center"   >
<ItemStyle Height="28px" ></ItemStyle>
                             </asp:BoundField>
                            <asp:BoundField DataField="FromTaka" HeaderText="From Taka" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center" >
<ItemStyle Height="28px" ></ItemStyle>
                             </asp:BoundField>  
                              <asp:BoundField DataField="ToTaka" HeaderText="ToTaka" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center"  >
<ItemStyle Height="28px" ></ItemStyle>
                              </asp:BoundField>
                             <asp:BoundField DataField="IncomeTaxRate" HeaderText="Income Tax Rate" Visible="true" ItemStyle-Height="28px">
                                 <ItemStyle Height="28px"></ItemStyle>
                             </asp:BoundField>
                             <asp:BoundField DataField="RateOrder" HeaderText="Order" Visible="true" ItemStyle-Height="28px">
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
         function validateInputs() {
             if (validateText('txtSlabName', 1, 60, 'Enter Slab Name') == false) return false;
             if (validateText('txtFromTaka', 1, 60, 'Enter From Taka') == false) return false;
             if (validateText('txttoTaka',1, 60, 'Enter To Taka') == false) return false;
             if (validateText('txtincometaxrate', 1, 60, 'Enter Income Tax Rate') == false) return false;
             if (validateText('txtOrder', 1, 60, 'Enter Order No') == false) return false;
             return true;
         }
    </script>
</asp:Content>
