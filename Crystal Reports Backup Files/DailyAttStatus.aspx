<%@ Page Title="Daily Attendance Status" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="DailyAttStatus.aspx.cs" Inherits="SigmaERP.attendance.DailyAttStatus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
           
        </Triggers>
        <ContentTemplate>
            <div class="main_box">
                <div class="main_box_header">
                    
                    <h2>Daily Attendance Status</h2>
                </div>
                <div class="main_box_body">
                    <div class="main_box_content">
                        <table class="table_daily_absence_report">
                            <tr>
                                <td>
                                    To Date:
                                </td>
                               
                                <td>
                                     <asp:TextBox ID="txtDate"  ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:CalendarExtender
                                                ID="TextBoxDate_CalendarExtender" Format="dd-MM-yyyy" runat="server" Enabled="True" TargetControlID="txtDate">
                                            </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    From Date:
                                </td>
                               
                                <td>
                                    <asp:TextBox ID="txtFromDate"  ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:CalendarExtender
                                                ID="CalendarExtender1" Format="dd-MM-yyyy" runat="server" Enabled="True" TargetControlID="txtFromDate">
                                            </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Atten. Status:
                                </td>
                                <td>
                                     <asp:DropDownList ID="ddlAttStatus" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="False" >
                                          <asp:ListItem>-Select-</asp:ListItem>
                                        <asp:ListItem>Present</asp:ListItem>
                                        <asp:ListItem>Absent</asp:ListItem>
                                        <asp:ListItem>Late</asp:ListItem>
                                        <asp:ListItem>Casula Leave</asp:ListItem>
                                        <asp:ListItem>Sick Leave</asp:ListItem>
                                         <asp:ListItem>Earned Leave</asp:ListItem>
                                        <asp:ListItem>Maternity Leave</asp:ListItem>
                                        <asp:ListItem>Weekend</asp:ListItem>
                                        <asp:ListItem>Holiday</asp:ListItem>
                                     </asp:DropDownList>
                                </td>
                            </tr>

                            </table>
                       
                        </div>
                        <div class="job_card_button_area">
                            <asp:Button ID="btnPreview" CssClass="css_btn" runat="server" ValidationGroup="save" Text="Preview" OnClick="btnPreview_Click"  />
                            &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
     <script type="text/javascript">
         function goToNewTabandWindow(url) {

             window.open(url);

         }

    </script>
</asp:Content>
