<%@ Page Title="Daily Out Time Missing" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="out_time_missiong.aspx.cs" Inherits="SigmaERP.attendance.out_time_missiong" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnPreview" />
            <asp:AsyncPostBackTrigger ControlID="rdoWise" />
            
        </Triggers>
        <ContentTemplate>

            <div class="main_box">
                <div class="main_box_header">
                    <h2>Daily Out Time Missing Report</h2>
                </div>
                <div class="main_box_body">
                    <div class="main_box_content">
                        <div class="late_report_box1">
                            <p>
                                Date : 
                        <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:ImageButton runat="server" AlternateText="Click to show calendar"
                                    ImageUrl="~/images/comon/Calendar_scheduleHS.png" TabIndex="4"
                                    ID="imgInDate"></asp:ImageButton>
                                <cc1:CalendarExtender runat="server" Format="yyyy-MM-dd"
                                    PopupButtonID="imgInDate" Enabled="True"
                                    TargetControlID="txtDate" ID="CExtApplicationDate">
                                </cc1:CalendarExtender>

                                <asp:RequiredFieldValidator ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDate" ErrorMessage="*"></asp:RequiredFieldValidator>
                               <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtDate" ValidationGroup="save"
                                    ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$"
                                    runat="server" ErrorMessage="Invalid format.">
                                </asp:RegularExpressionValidator>--%>

                            </p>
                        </div>
                        <div class="late_report_box2">
                            <table class="table_late_report">
                                <tr>
                                    <td>
                                        
                                        <asp:RadioButtonList ID="rdoWise" AutoPostBack="true" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdoWise_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="List All" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Division"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        
                                    </td>
                                    
                                </tr>
                            </table>

                        </div>
                        <div class="late_report_box3">
                            <p>
                                Division Name : 
                       <asp:DropDownList ID="ddlDivision" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlDevision_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
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

                        <div class="late_report_button">
                            <asp:Button ID="btnPreview" CssClass="css_btn" runat="server" ValidationGroup="save" Text="Preview" OnClick="btnPreview_Click"  />
                           
                            &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
