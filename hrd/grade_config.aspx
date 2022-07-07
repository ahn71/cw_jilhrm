<%@ Page Title="Grade Configuration" Language="C#" MasterPageFile="~/hrd_nested.master" AutoEventWireup="true" CodeBehind="grade_config.aspx.cs" Inherits="SigmaERP.hrd.grade_config" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="/style/dataTables.css" rel="stylesheet" />
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
          #ContentPlaceHolder1_MainContent_gvGradeList th:nth-child(3),th:nth-child(4),th:nth-child(5),td:nth-child(3) {
            text-align:center;
        }
        #ContentPlaceHolder1_MainContent_gvGradeList th, td {
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
                    <li><a href="#" class="ds_negevation_inactive Ractive">Grade</a></li>
                </ul>
            </div>

        </div>
    </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server">
 <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
     <asp:HiddenField ID="hdnUpdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnbtnStage" runat="server" ClientIDMode="Static" />
     <asp:HiddenField ID="upSave" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upupdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="updelete" runat="server" ClientIDMode="Static" />
        <div class="main_box RBox">
    	<div class="main_box_header RBoxheader">
            <h2>Grade Configuration Panel</h2>
        </div>
    	<div class="main_box_body Rbody">
        	<div class="main_box_content">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" />                        
                        <asp:AsyncPostBackTrigger ControlID="btnClose" />
                        <asp:AsyncPostBackTrigger ControlID="btnNew" />
                        
                    </Triggers>
                    <ContentTemplate>
                <div class="input_division_info">
                    <table class="division_table">
                        <tr>
                            <td>
                                Grade Name <span class="requerd1">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtGradeName" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 বাংলায়
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtGradeBangla" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Font-Names="SutonnyMJ"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Status <span class="requerd1">*</span>
                            </td>
                            <td>
                                :
                            </td>
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
                    <asp:Button ID="btnNew" runat="server" ClientIDMode="Static" CssClass="Rbutton" Text="New" OnClick="btnNew_Click" />
                    <asp:Button ID="btnSave" runat="server" ClientIDMode="Static" CssClass="Rbutton" Text="Save" OnClientClick="return validateInputs();" OnClick="btnSave_Click"/>                         
                    <asp:Button ID="btnClose" Class="Rbutton" runat="server" Text="Close" PostBackUrl="~/hrd_default.aspx" />
                </div>
                     </ContentTemplate>

                </asp:UpdatePanel>
                     <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" />                       
                    </Triggers>
                    <ContentTemplate>
                        <div class="show_division_info">
				<div id="divGradeList" class="datatables_wrapper" runat="server" style="width:100%; height:auto; max-height:500px;overflow:auto;overflow-x:hidden;">
                    <asp:GridView ID="gvGradeList" runat="server" DataKeyNames="SL" AllowPaging="True" AutoGenerateColumns="False" style="width:100%;"   OnPageIndexChanging="gvGradeList_PageIndexChanging" OnRowCommand="gvGradeList_RowCommand" OnRowDeleting="gvGradeList_RowDeleting" OnRowDataBound="gvGradeList_RowDataBound"  >
                                <RowStyle HorizontalAlign="Center" />
                         <PagerStyle CssClass="gridview Sgridview" Height="40px" />
                                <Columns>
                                    

                                    <asp:BoundField DataField="GrdName" HeaderText="Grade Name"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                    <asp:BoundField DataField="GrdNameBangla" HeaderText="Grade(বাংলায়)"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  ItemStyle-CssClass="fontF"/>
                                     <asp:BoundField DataField="GrdStatus" HeaderText=" G.Status"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  />
                                  
                                    
                                    

                                     <asp:TemplateField HeaderText="Edit" ItemStyle-Width="100px">
                                       <ItemTemplate  >
                                            <asp:Button ID="btnAlter" runat="server" ControlStyle-CssClass="btnForAlterInGV"  Text="Edit" CommandName="Alter" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                       </ItemTemplate>
                                   </asp:TemplateField>
                                    <%--<asp:ButtonField CommandName="Alter"   ControlStyle-CssClass="btnForAlterInGV"  HeaderText="Alter" ButtonType="Button" Text="Alter" ItemStyle-Width="80px"/>--%>
                                   
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="100px">
                                       <ItemTemplate  >
                                            <asp:Button ID="btnDelete" runat="server" ControlStyle-CssClass="btnForDeleteInGV"  Text="Delete" CommandName="Delete" CommandArgument='<%#((GridViewRow)Container).RowIndex%>'  OnClientClick="return confirm('Are you sure to delete ?')" />
                                       </ItemTemplate>
                                   </asp:TemplateField>
                                     
                                </Columns>
                                <HeaderStyle BackColor="#0057AE" ForeColor="white" Height="28px" />
                            </asp:GridView>
                            
				</div>
                  </div>
                   </ContentTemplate>

                </asp:UpdatePanel>    
            </div>
        </div>
    </div>
     <script type="text/javascript">
         function getCardNo() {

             var val = document.getElementById('dlDepartment').value;
             document.getElementById('hdfDepartment').value = val;

         }

         function validateInputs() {
             if (validateText('txtGradeName', 1, 60, 'Enter GradeName') == false) return false;
             if ($('dlStatus').val() == '-select-') {
                 showMessage('Please Select Grade Status', 'warning');
                 return false;
             }
             return true;
         }

         function editGrade(id) {
             var Grade = $('#r_' + id + ' td:first').html();
             $('#txtGradeName').val(Grade);
             var GradeB = $('#r_' + id + ' td:nth-child(2)').html();
             $('#txtGradeBangla').val(GradeB);
             var GStatus = $('#r_' + id + ' td:nth-child(3)').html();
             $('#dlStatus').val(GStatus);

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
             $('#btnSave').val('Save');
             $('#hdnbtnStage').val("");
             $('#hdnUpdate').val("");
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
             $('#txtGradeName').val('');
             $('#txtGradeBangla').val('');
             $('#btnSave').val('Save');
             $('#hdnbtnStage').val('');
             $('#hdnUpdate').val('');
             $('#btnDelete').removeClass('css_btn');
             $('#btnDelete').attr('disabled', 'disabled');
         }

    </script>
</asp:Content>
