<%@ Page Title="Job Card With Summary" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="job_card_with_summaryOld.aspx.cs" Inherits="SigmaERP.attendance.job_card_with_summaryOld" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
    	<div class="main_box_header">
            <h2>Job Card With Summary</h2>
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
                            <td> <asp:TextBox ID="TextBox1" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                            <td>Card ID :</td>
                            <td> <asp:TextBox ID="TextBox2" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>
                <div class="job_card_box3">
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
                </div>
                <div class="job_card_button_area">
                    <button class="css_btn" type="button" name="" value="">Preview</button> &nbsp; &nbsp; &nbsp;
                    <button class="css_btn" type="button" name="" value="">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
