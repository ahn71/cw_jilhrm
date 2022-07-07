<%@ Page Title="Monthly Night Allowance" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="monthly_night_allowance.aspx.cs" Inherits="SigmaERP.payroll.monthly_night_allowance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
    	<div class="main_box_header">
            <h2>Holiday / Night Allowance</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="festival_bonus_box1">
                    <fieldset>
                        <legend>
                            Select an Option
                        </legend>
                        <table class="festival_bonus_box1_table">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="RadioButton1" runat="server" /> Indivdual 

                                </td>
                                <td>
                                    <asp:DropDownList ID="DropDownList1" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="RadioButton2" runat="server" /> Worker 
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButton ID="RadioButton3" runat="server" /> Staff 
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div class="festival_bonus_box2">
                    <table class="ot_payment_sheet_box1_table">
                        <tr>
                            <td>
                                Month ID : 
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList3" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Division Name : 
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList2" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                            </td>

                        </tr>
                    </table>
                </div>

                <div class="job_card_box3">

                    <div class="job_card_left">
                        <asp:ListBox ID="ListBox1" Width="270"  Height="146" runat="server"></asp:ListBox>
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
                    <button class="css_btn" type="button" name="" value="">Preview</button> &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
