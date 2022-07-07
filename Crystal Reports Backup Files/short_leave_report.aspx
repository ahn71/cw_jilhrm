<%@ Page Title="Short Leave Report" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="short_leave_report.aspx.cs" Inherits="SigmaERP.personnel.short_leave_report" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="main_box">
        <div class="main_content_box_header">
            <h2>Short Leave Report</h2>
        </div>

        <div class="main_content_box_body">
            <div class="box_main_content">
                <div class="status_form">
                    <div class="short_leave_select_form">
                        <table>
                            <tr>
                                <td>From Date :</td>
                                <td>
                                    <asp:TextBox ID="txtFromDate" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:ImageButton runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/images/comon/Calendar_scheduleHS.png" TabIndex="4"
                                    ID="imgFromDate"></asp:ImageButton>
                                <cc1:CalendarExtender runat="server" Format="dd-MMM-yyyy"
                                    PopupButtonID="imgFromDate" Enabled="True"
                                    TargetControlID="txtFromDate" ID="CExtApplicationDate">
                                </cc1:CalendarExtender>

                                    <asp:RequiredFieldValidator ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator2" runat="server"  ControlToValidate="txtFromDate" ErrorMessage="*"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate = "txtFromDate" ValidationGroup="save"
    ValidationExpression = "^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$"
    runat="server" ErrorMessage="Invalid format.">
    </asp:RegularExpressionValidator>

                                </td>
                                <td></td>
                                <td></td>
                            </tr>

                            <tr>
                                <td>To Date :</td>
                                <td>
                                    <asp:TextBox ID="txtToDate" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>

                                <asp:ImageButton runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/images/comon/Calendar_scheduleHS.png" TabIndex="4"
                                    ID="imgToDate"></asp:ImageButton>
                                <cc1:CalendarExtender runat="server" Format="dd-MMM-yyyy"
                                    PopupButtonID="imgToDate" Enabled="True"
                                    TargetControlID="txtToDate" ID="CalendarExtender1">
                                </cc1:CalendarExtender>

                                    <asp:RequiredFieldValidator ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator1" runat="server"  ControlToValidate="txtToDate" ErrorMessage="*"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate = "txtToDate" ValidationGroup="save"
    ValidationExpression = "^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$"
    runat="server" ErrorMessage="Invalid format.">
    </asp:RegularExpressionValidator>

                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                        <br />
                    </div>
                    <fieldset>
                        <legend><b>Select Option</b></legend>
                        <table>
                            <tr>
                                <td align="right">
                                    <asp:RadioButton ID="RadioButton2" runat="server" />
                                    <label for="male">Summary</label>

                                </td>
                                <td width="100px">
                                <td align="right">
                                    <asp:RadioButton ID="RadioButton1" runat="server" />
                                    <label for="male">Details</label>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                        <br />
                    </fieldset>
                    <br />

                    <div class="border">
                    </div>
                    <div class="button_middle_status" style="width:270px">
                        <table>
                            <tbody>

                                <tr>
                                    <td>
                                        <asp:Button ID="btnPreview" runat="server" Text="Prewiew" ValidationGroup="save"
                                            CssClass="back_button" OnClick="btnPreview_Click" /></td>
                                    <td>
                                        <asp:Button ID="Button3" runat="server" PostBackUrl="~/default.aspx" Text="Close" CssClass="css_btn" />
                                    </td>
                                </tr>

                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
