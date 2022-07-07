<%@ Page Title="User Account" Language="C#" MasterPageFile="~/Tools_Nested.Master" AutoEventWireup="true" CodeBehind="CreateAccount.aspx.cs" Inherits="SigmaERP.ControlPanel.CreateAccount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../style/oitlStyle.css" rel="stylesheet" />
    <link href="../style/reg_style.css" rel="stylesheet" />

    <style type="text/css">
        .auto-style2 {
            padding: 7px;
            width: 117px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <div style="margin-top: 5px">
                    <ul>
                        <li><a href="/default.aspx">Dasboard</a></li>
                        <li>/</li>
                        <li><a href="/tools_default.aspx">Tools</a></li>
                        <li>/</li>
                        <li><a href="#" class="ds_negevation_inactive Tactive">User Account</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="create_account_main_box Mbox">
        <div class="employee_box_header TBoxheader">
            <h2>New User</h2>
        </div>
        <div class="punishment_bottom_header">

            <p>
                <asp:LinkButton ID="lnkLogin" runat="server" PostBackUrl="~/ControlPanel/Login.aspx">Login</asp:LinkButton>
            </p>
        </div>
        <div class="employee_box_body Tbody" style="width: 100%">

            <div class="create_account_content">

                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <%-- <asp:AsyncPostBackTrigger ControlID="btnSearch" />--%>
                        <%--<asp:AsyncPostBackTrigger ControlID="ckbLeaveAuthority" />--%>
                    </Triggers>
                    <ContentTemplate>
                        <asp:HiddenField ID="hfSalaryType" ClientIDMode="Static" runat="server" />
                        <div class="input_division_info">
                            <%--<asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>--%>
                            <table class="division_table">

                                <tr>
                                    <td>Company</td>
                                    <td>:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control select_width" Width="330px" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Emoloyee</td>
                                    <td>:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlEmpList" ClientIDMode="Static" runat="server" CssClass="form-control select_width" Width="330px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td>User Name</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtUsername" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="320px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Password</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtPassword" ClientIDMode="Static" TextMode="Password" runat="server" CssClass="form-control text_box_width" Width="320px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Con. Password</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtConfirmPassword" ClientIDMode="Static" runat="server" TextMode="Password" CssClass="form-control text_box_width" Width="320px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Email</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtEmail" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="320px"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td>Type</td>
                                    <td>:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control select_width" Width="330px">
                                            
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="ckbLeaveAuthority" Text=" If Leave Authority" AutoPostBack="true" OnCheckedChanged="ckbLeaveAuthority_CheckedChanged" />
                                    </td>
                                </tr>
                                <%--</table>
                              <br />
                              <table class="division_table" runat="server" >    --%>
                                <asp:Panel runat="server" ID="pLeaveAuthority" Visible="false">


                                    <tr>
                                        <td>Position</td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="txtOrder" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="320px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FT1" runat="server" FilterType="Numbers"
                                                TargetControlID="txtOrder" ValidChars="">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Authority</td>
                                        <td>:</td>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rblLeaveAuthority" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="true" Value="1">Forword</asp:ListItem>
                                                <asp:ListItem Value="2">Approved</asp:ListItem>
                                                <asp:ListItem Value="0">Both</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Emp Type</td>
                                        <td>:</td>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rblEmpType" RepeatDirection="Horizontal">
                                            </asp:RadioButtonList></td>
                                    </tr>
                                    <tr>
                                        <td>Athorise area</td>
                                        <td>:</td>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rblOnlyDepartment" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
                                                <asp:ListItem Value="1">Only Depertment</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>

                            <%--</ContentTemplate>
                                </asp:UpdatePanel>--%>
                        </div>


                        <div class="button_area Rbutton_area">
                            <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server" ClientIDMode="Static" UpdateMode="Conditional">
                                 <Triggers>
                                     <asp:AsyncPostBackTrigger ControlID="btnSave" />
                                     <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                                 </Triggers>

                                 <ContentTemplate>--%>

                            <asp:Button ID="btnSave" ClientIDMode="Static" CssClass="Tbutton" runat="server" Text="Create" OnClientClick="return InputValidationBasket();" OnClick="btnSave_Click" />
                            <asp:Button ID="btnClear" ClientIDMode="Static" CssClass="Tbutton" runat="server" Text="Clear" />

                            <%--</ContentTemplate>
                                  </asp:UpdatePanel>--%>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>


    <script type="text/javascript">
        $(document).ready(function () {
            load();
           
        });
        function load() {
            $("#ddlEmpList").select2();
        }
        function InputValidationBasket() {
            try {

                if ($('#txtLastName').val().trim().length <= 0) {
                    showMessage('Last name required minimum 3 characters', 'error');
                    $('#txtLastName').focus(); return false;
                }
                else if ($('#txtUsername').val().trim().length <= 3) {
                    showMessage(' name required minimum 4 characters ', 'error');
                    $('#txtUsername').focus(); return false;
                }
                else if ($('#txtPassword').val().trim().length == 0) {
                    showMessage('User password required minimum 4 characters', 'error');
                    $('#txtPassword').focus(); return false;
                }
                else if ($('#txtConfirmPassword').val().trim().length == 0) {
                    showMessage('Please type confirm password', 'error');
                    $('#txtConfirmPassword').focus(); return false;
                }
                else if ($('#txtPassword').val() != $('#txtConfirmPassword').val()) {
                    showMessage('Dose not match confirm password with main password', 'error');
                    $('#txtConfirmPassword').focus(); return false;
                }

                else if ($('#txtEmail').val().trim().length == 0) {
                    showMessage('Please type the Valid email address', 'error');
                    $('#txtEmail').focus(); return false;
                }
                else if ($('#ddlUserType').val().trim().length == 0) {
                    showMessage('Please select the user type', 'error');
                    $('#ddlUserType').focus(); return false;
                }

                return true;
            }
            catch (exception) {
                showMessage(exception, 'error')
            }
        }

        function ClearInputBox() {
            try {
                load();
                $("#ddlEmpList").val("0");
                $('#txtFirstName').val('');
                $('#txtLastName').val('');
                $('#txtUsername').val('');
                $('#txtPassword').val('');
                $('#txtConfirmPassword').val('');
                $('#txtEmail').val('');
                $('#ddlUserType').val('User');
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }

    </script>
</asp:Content>
