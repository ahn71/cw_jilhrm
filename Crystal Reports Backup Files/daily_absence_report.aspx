<%@ Page Title="Daily Absence Report" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="daily_absence_report.aspx.cs" Inherits="SigmaERP.attendance.daily_absence_report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnaddall" />
            <asp:AsyncPostBackTrigger ControlID="btnadditem" />
            <asp:AsyncPostBackTrigger ControlID="btnremoveitem" />
            <asp:AsyncPostBackTrigger ControlID="btnremoveall" />
        </Triggers>
            <ContentTemplate>

    <div class="main_box">
        <div class="main_box_header">
            <h2>Daily Absence Report</h2>
        </div>
        <div class="main_box_body">
            <div class="main_box_content">
                <div class="daily_absence_report_box1">
                    <p>
                        Date : 
                        <asp:TextBox ID="dptDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                        <asp:ImageButton runat="server" AlternateText="Click to show calendar"
                            ImageUrl="~/images/comon/Calendar_scheduleHS.png" TabIndex="4"
                            ID="imgDate"></asp:ImageButton>
                        <cc1:calendarextender runat="server" Format="yyyy-MM-dd"
                            PopupButtonID="imgDate" Enabled="True"
                            TargetControlID="dptDate" ID="CExtApplicationDate">
                        </cc1:calendarextender>
                        <asp:RequiredFieldValidator ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator3" runat="server" ControlToValidate="dptDate" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="dptDate" ValidationGroup="save"
                                    ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$"
                                    runat="server" ErrorMessage="Invalid format.">
                                    </asp:RegularExpressionValidator>--%>
                    </p>
                </div>
                <div class="daily_absence_report_box2">
                    <table class="table_daily_absence_report">
                        <tr>

                            <td>
                                <asp:RadioButtonList ID="rdoDept" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rdoDept_SelectedIndexChanged">
                                    <asp:ListItem Text="List All" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Division" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>

                        </tr>
                    </table>

                </div>
                <div class="daily_absence_report_box3">
                    <p>
                        Division Name : 
                       <asp:DropDownList ID="ddlDivision" ClientIDMode="Static" AppendDataBoundItems="True" CssClass="form-control select_width" runat="server" OnTextChanged="ddlDivision_TextChanged" AutoPostBack="true"></asp:DropDownList>
                    </p>
                </div>

                <div class="daily_absence_report_box4">
                    <div class="daily_absence_report_left">
                        <p>Available Departments</p>
                        <asp:ListBox ID="lstEmployees" Width="260" Height="146" runat="server" SelectionMode="Multiple" AutoPostBack="True"></asp:ListBox>
                    </div>
                    <div class="daily_absence_report_middle">

                        <asp:Button ID="btnadditem" CssClass="next_button" runat="server" Text=">" />
                        <br />
                        <asp:Button ID="btnaddall" CssClass="next_button" runat="server" Text=">>"/>
                        <br />
                        <asp:Button ID="btnremoveitem" CssClass="next_button" runat="server" Text="<"/>
                        <br />
                        <asp:Button ID="btnremoveall" CssClass="next_button" runat="server" Text="<<"/>
                    </div>
                    <div class="daily_absence_report_right">
                        <p>Selected Department/s</p>
                        <asp:ListBox ID="lstSelectedEmployees" Width="260" Height="146" runat="server" SelectionMode="Multiple"></asp:ListBox>

                    </div>
                </div>

                <div class="job_card_button_area">
                    <%--<button class="css_btn" type="button" name="" value="">Preview</button>--%>
                            <asp:Button ID="btnPreview" CssClass="css_btn" style="display:none"   ValidationGroup="save" runat="server" Text="Preview" />
                        
                             <asp:Button ID="btnPreviewAbs" CssClass="css_btn" OnClick="btnPreviewAbs_Click" ValidationGroup="save" runat="server" Text="Preview" />
                              &nbsp; &nbsp; &nbsp;
                             <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/attendance_default.aspx" CssClass="css_btn" />

                     </div>
            </div>
        </div>
    </div>

    </ContentTemplate>        
 </asp:UpdatePanel>
</asp:Content>
