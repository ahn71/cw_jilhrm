<%@ Page Title="Daily Summary" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="daily_summary.aspx.cs" Inherits="SigmaERP.attendance.daily_summary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
        </Triggers>
        <ContentTemplate>
            <div class="main_box">
                <div class="main_box_header">
                    <h2>Daily Summary</h2>
                </div>
                <div class="main_box_body">
                    <div class="main_box_content">
                        <div class="overtime_report_box3">
                            <p>
                                Date : 
                       Date : 
                        <asp:TextBox ID="dptDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:ImageButton runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/images/comon/Calendar_scheduleHS.png" TabIndex="4"
                                    ID="imgDate"></asp:ImageButton>
                                <cc1:CalendarExtender runat="server" Format="dd-MMM-yyyy"
                                    PopupButtonID="imgDate" Enabled="True"
                                    TargetControlID="dptDate" ID="CExtApplicationDate">
                                </cc1:CalendarExtender>
                                <asp:RequiredFieldValidator ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator3" runat="server" ControlToValidate="dptDate" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="dptDate" ValidationGroup="save"
                                    ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$"
                                    runat="server" ErrorMessage="Invalid format.">
                                </asp:RegularExpressionValidator>
                            </p>
                        </div>
                        <div class="job_card_button_area">
                            <asp:Button ID="btnPreview" CssClass="css_btn"  ValidationGroup="save" runat="server" Text="Preview"/>
                            &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
