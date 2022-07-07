<%@ Page Title="Department" Language="C#" MasterPageFile="~/hrd_nested.master" AutoEventWireup="true" CodeBehind="department.aspx.cs" Inherits="SigmaERP.hrd.department" %>
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
    <style>
        #ContentPlaceHolder1_MainContent_divDepartmentList th {
            text-align:center;
        }
         #ContentPlaceHolder1_MainContent_divDepartmentList  th:nth-child(1),td:nth-child(1)  {
            text-align:left;
           padding-left:3px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row Rrow">
                  <div class="col-md-12 ds_nagevation_bar">
               <div style="margin-top: 5px">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                       <li> <a href="#">/</a></li>
                       <li> <a href="/hrd_default.aspx">Settings</a></li>
                       <li> <a href="#">/</a></li>
                       <li> <a href="#" class="ds_negevation_inactive Ractive">Department</a></li>
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
    <div class="main_box RBox">
    	<div class="main_box_header RBoxheader">
            <h2>Department Entry Panel</h2>
        </div>
    	<div class="main_box_body Rbody">
        	<div class="main_box_content">
                  <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />  
             <asp:AsyncPostBackTrigger ControlID="btnNew" />  
            <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" />          
        </Triggers>
        <ContentTemplate>
                <div class="input_division_info">
                    <table class="employee_table">
                        <tr id="trCompanyName"   runat="server">
                            <td>
                               Company Name  
                            </td>
                            <td>:</td>
                            <td>
                               <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" AutoPostBack="true" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged" requerd  >
                                                                        
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Department Name <span class="requerd1">*</span> 
                            </td>
                            <td>:</td>
                            <td>
                                <asp:TextBox ID="txtDepartment" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                বাংলায় 
                            </td>
                            <td>:</td>
                            <td>

                                <asp:TextBox ID="txtDepartmentBn" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Font-Names="SutonnyMJ"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trDptCode">
                            <td>
                                Department Code <span class="requerd1">*</span>  
                            </td>
                            <td>:</td>
                            <td>

                                <asp:TextBox ID="txtDepartmentCode" ClientIDMode="Static" runat="server" MaxLength="2" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="F1" runat="server" FilterType="Numbers" 
                                TargetControlID="txtDepartmentCode" ValidChars=""></asp:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Status <span class="requerd1">*</span> 
                            </td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList ID="dlStatus" ClientIDMode="Static" CssClass="form-control select_width" runat="server">
                                    <asp:ListItem>-select-</asp:ListItem>
                                    <asp:ListItem>Active</asp:ListItem>
                                    <asp:ListItem>InActive</asp:ListItem>
                                </asp:DropDownList>                             
                            </td>
                        </tr>
                    </table>
                </div>
                
              
                <div class="button_area Rbutton_area">
                    <a href="#" onclick="window.history.back()" class="Rbutton">Back</a>
                    <asp:Button ID="btnNew" ClientIDMode="Static" CssClass="Rbutton"  runat="server" Text="New" OnClick="btnNew_Click"/>
                    <asp:Button ID="btnSave" OnClientClick="return validateInputs();" OnClick="btnSave_Click" ClientIDMode="Static" class="Rbutton" runat="server" Text="Save" />
                    <asp:Button ID="btnClose" ClientIDMode="Static" CssClass="Rbutton" PostBackUrl="~/hrd_default.aspx"  runat="server" Text="Close" />
                </div>
            </ContentTemplate>
                      </asp:UpdatePanel>

                  <div class="show_division_info">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">    
        <ContentTemplate>
                     <%--<div id="ShiftConfig" class="datatables_wrapper" runat="server" style="width:100%; height:auto; max-height:500px;overflow:auto;overflow-x:hidden;"></div>--%>
                    <asp:GridView ID="divDepartmentList" runat="server"  Width="100%" AutoGenerateColumns="False" DataKeyNames="SL,CompanyId,DptId"  OnRowCommand="divDepartmentList_RowCommand" OnRowDeleting="divDepartmentList_RowDeleting" AllowPaging="True" OnPageIndexChanging="divDepartmentList_PageIndexChanging" OnRowDataBound="divDepartmentList_RowDataBound" >

<HeaderStyle BackColor="#0057AE" Font-Bold="True" Font-Size="14px" ForeColor="White" Height="28px"></HeaderStyle>
                        <PagerStyle CssClass="gridview Sgridview" Height="40px" />

                       <RowStyle HorizontalAlign="Center" />
                         <Columns>
                            <asp:BoundField DataField="SL"  HeaderText="SL" Visible="false"  ItemStyle-Height="28px" >
<ItemStyle Height="28px"></ItemStyle>
                             </asp:BoundField>
                              <asp:BoundField DataField="CompanyName" HeaderStyle-HorizontalAlign="Left" HeaderText="Company Name" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="Left"  >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>

<ItemStyle HorizontalAlign="Left" Height="28px" ></ItemStyle>
                             </asp:BoundField>
                            <asp:BoundField DataField="DptName" HeaderText="Department" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center"   >
<ItemStyle Height="28px" ></ItemStyle>
                             </asp:BoundField>
                            <asp:BoundField DataField="DptNameBn" HeaderText="বিভাগ" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="fontF"  >
<ItemStyle Height="28px" ></ItemStyle>
                             </asp:BoundField>  
                              <asp:BoundField DataField="DptCode" HeaderText="Code" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center"  >
<ItemStyle Height="28px" ></ItemStyle>
                             </asp:BoundField>                   
                             <asp:BoundField DataField="DptStatus" HeaderText="Dpt Status" Visible="true"  ItemStyle-Height="28px" >                         
<ItemStyle Height="28px" ></ItemStyle>
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
             if (validateText('txtDepartment', 1, 60, 'Enter Department Name') == false) return false;
             if (validateText('txtDepartmentCode', 1, 2, 'Enter Valid Department Code') == false) return false;
             if (validateText('txtDepartmentCode', 2, 2, 'Enter Department Code (Must be 2 Character)') == false) return false;
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
