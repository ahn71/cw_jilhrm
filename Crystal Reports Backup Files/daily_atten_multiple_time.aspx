<%@ Page Title="Daily Atten Multiple Time" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="daily_atten_multiple_time.aspx.cs" Inherits="SigmaERP.attendance.daily_atten_multiple_time" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnPreview" />
            <asp:AsyncPostBackTrigger ControlID="ddlMonthID" />
        </Triggers>
        <ContentTemplate>
    <div class="main_box">
    	<div class="main_box_header">
            <h2>Daily Attendance Multiple Time</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="job_card_box1">
                    <table class="job_card_table">
                        <tr>
                            <td>
                                <asp:RadioButton ID="RadioButton1" runat="server"></asp:RadioButton> Month ID
                            </td>
                            <td>
                                <asp:RadioButton ID="RadioButton2" runat="server"></asp:RadioButton> Individual Card
                            </td>
                            <td>
                                <asp:RadioButton ID="RadioButton3" runat="server"></asp:RadioButton> Division
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="job_card_box2">
                    <table>
                        <tr>
                            <td>Month ID :</td>
                            <td> 

                                <asp:DropDownList ID="ddlMonthID" runat="server" CssClass="form-control select_width">
                                    
                                </asp:DropDownList>
                                                   <asp:RequiredFieldValidator InitialValue="" ID="RequiredFieldValidator13" Display="Dynamic" 
    ValidationGroup="save" runat="server" ControlToValidate="ddlMonthID"
    Text="*" ErrorMessage="ErrorMessage"></asp:RequiredFieldValidator>

                            </td>
                            <td>Card ID :</td>
                            <td> <asp:TextBox ID="txtCardNo" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                            <asp:RequiredFieldValidator InitialValue="" ID="RequiredFieldValidator1" Display="Dynamic" 
    ValidationGroup="save" runat="server" ControlToValidate="txtCardNo"
    Text="*" ErrorMessage="ErrorMessage"></asp:RequiredFieldValidator>
                        </tr>
                    </table>
                </div>

<%--                <div class="job_card_box3">
                    <div class="job_card_left">
                        <asp:ListBox ID="ListBox1" Width="270" Height="146" runat="server"></asp:ListBox>
                    </div>
                    <div class="job_card_middle">
                        <button class="next_button" type="button" name="" value=""> > </button><br />
                        <button class="next_button" type="button" name="" value=""> >> </button><br />
                        <button class="next_button" type="button" name="" value=""> < </button><br />
                        <button class="next_button" type="button" name="" value=""> << </button>
                    </div>
                    <div class="job_card_right">
                        <asp:ListBox ID="ListBox2" Width="270" Height="146" runat="server"></asp:ListBox>
                    </div>
                </div>--%>

                <div class="job_card_button_area">
                    <asp:Button ID="btnPreview" CssClass="css_btn"  ValidationGroup="save" runat="server" Text="Preview"/>
                    &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/attendance_default.aspx" CssClass="css_btn" />
                </div>
            </div>
        </div>
    </div>

            
                    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
