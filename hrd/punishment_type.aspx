<%@ Page Title="Punishment Type" Language="C#" MasterPageFile="~/hrd_nested.master" AutoEventWireup="true" CodeBehind="punishment_type.aspx.cs" Inherits="SigmaERP.hrd.punishment_type" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:HiddenField ID="hdnUpdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnbtnStage" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upSave" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upupdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="updelete" runat="server" ClientIDMode="Static" />
    <div class="main_box">
    	<div class="main_box_header">
            <h2>Punishment Type</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" />
                                <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                            </Triggers>
                            <ContentTemplate>
                <div class="input_division_info">
                    <table class="division_table">
                        <tr>
                            <td>
                                Punishment Type
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPunishmentType" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="button_area">
                    <table class="button_table">
                        <tr>
                            <th><button id="btnNew" class="css_btn" type="button" name="" value="">New</button></th>
                            <th>
                                <asp:Button ID="btnSave" ClientIDMode="Static" CssClass="css_btn"  runat="server" Text="Save" OnClientClick="return validateInputs();" OnClick="btnSave_Click"    />
                            </th>
                            <th>
                                <asp:Button ID="btnDelete" ClientIDMode="Static" CssClass="css_btn"  runat="server" Text="Delete" OnClick="btnDelete_Click"  />
                            </th>
                            <th> <asp:Button ID="btnClose" ClientIDMode="Static" CssClass="css_btn" PostBackUrl="~/hrd_default.aspx"  runat="server" Text="Close" /></th>
                            
                        </tr>
                    </table>
                </div>
             </ContentTemplate>
                        </asp:UpdatePanel>

				 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" />
                         <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                    </Triggers>
                    <ContentTemplate>
                <div class="show_division_info">
                     <div id="divpunishmenttype" class="datatables_wrapper" runat="server" style="width:100%; height:auto; max-height:500px;overflow:auto;overflow-x:hidden;"></div>
                </div>
                        </ContentTemplate>
                </asp:UpdatePanel>

				
            </div>
        </div>
    </div>
    <script type="text/javascript">

        //$('#dlDivision').change(function () {



        $('#btnNew').click(function () {
            clear();
        });
        function validateInputs() {
            if (validateText('txtPunishmentType', 1, 60, 'Enter Punishment Type') == false) return false;
            return true;
        }

        function editPunishmenttype(id) {
            var divsn = $('#r_' + id + ' td:first').html();
           
            $('#txtPunishmentType').val(divsn);
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

        //function deleteSuccess() {
        //    showMessage('Deleted successfully', 'success');
        //    $('#btnSave').val('Save');
        //    $('#hdnbtnStage').val("");
        //    $('#hdnUpdate').val("");
        //    clear();
        //}
        //function UpdateSuccess() {
        //    showMessage('Updated successfully', 'success');
        //    $('#btnSave').val('Save');
        //    $('#hdnbtnStage').val("");
        //    $('#hdnUpdate').val("");
        //    clear();
        //}
        //function SaveSuccess() {
        //    showMessage('Save successfully', 'success');
        //    $('#btnSave').val('Save');
        //    $('#hdnbtnStage').val("");
        //    $('#hdnUpdate').val("");
        //    clear();
        //}


        function clear() {
            if ($('#upSave').val() == '0') {

                $('#btnSave').removeClass('css_btn');
                $('#btnSave').attr('disabled', 'disabled');
            }
            else {
                $('#btnSave').addClass('css_btn');
                $('#btnSave').removeAttr('disabled');
            }

            $('#txtPunishmentType').val('');
            $('#btnSave').val('Save');
            $('#hdnbtnStage').val("");
            $('#hdnUpdate').val("");
            $('#btnDelete').removeClass('css_btn');
            $('#btnDelete').attr('disabled', 'disabled');
        }

    </script>
</asp:Content>
