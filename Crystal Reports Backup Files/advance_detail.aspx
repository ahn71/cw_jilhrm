<%@ Page Title="Advance Detail" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="advance_detail.aspx.cs" Inherits="SigmaERP.payroll.advance_detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="main_box">
    	<div class="main_box_header">
            <h2>Loan / Advance Details</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="advance_Details_box1">
                    <table class="advance_Details_box1_table">
                        <tr>
                            <td>
                                <asp:RadioButton ID="RadioButton1" runat="server" /> Loan
                            </td>
                            <td>
                                <asp:RadioButton ID="RadioButton2" runat="server" /> Advance
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="advance_Details_box2">
                    <fieldset>
                        <legend>Employee</legend>
                            <table class="advance_Details_box2_table">
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RadioButton3" runat="server" /> All
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="RadioButton4" runat="server" /> Individual
                                    </td>
                                </tr>
                            </table>
                    </fieldset>
                </div>
                <div class="advance_Details_box3">
                    <fieldset>
                        <legend>Process</legend>
                            <table class="advance_Details_box3_table">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="CheckBox1" runat="server" /> Processed
                                </td>
                            </tr>
                                <tr>
                                <td>
                                    <asp:CheckBox ID="CheckBox2" runat="server" /> UnProcessed
                                </td>
                            </tr>
                            </table>
                    </fieldset>
                </div>
                <div class="job_card_button_area">
                    <button class="css_btn" type="button" name="" value="">Preview</button> &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
