<%@ Page Title="Mult Card Salary Sheet" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="mult_card_salary_sheet.aspx.cs" Inherits="SigmaERP.payroll.mult_card_salary_sheet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
    	<div class="main_box_header">
            <h2>Mult Card Salary Sheet</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="mult_card_salary_sheet_box1">
                    <div class="mult_card_salary_sheet_box1_left">
                        <table class="mult_card_salary_sheet_box1_left_table">
                            <tr>
                                <td>Month Name : </td>
                                <td><asp:DropDownList ID="DropDownList1" ClientIDMode="Static" CssClass="form-control select_width" runat="server"></asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td>Division Name : </td>
                                <td><asp:DropDownList ID="DropDownList2" ClientIDMode="Static" CssClass="form-control select_width" runat="server"></asp:DropDownList></td>
                            </tr>
                        </table>
                    </div>
                    <div class="mult_card_salary_sheet_box1_right">
                        <fieldset>
                            <legend>Selection</legend>
                            <table class="mult_card_salary_sheet_box1_right_table">
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RadioButton1" runat="server" /> English

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RadioButton2" runat="server" /> Bangla
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </div>
                <div class="annual_leave_payment_box2">
                    <div class="annual_leave_payment_box2_left">
                        <asp:ListBox ID="ListBox1" Width="260" Height="146" runat="server"></asp:ListBox>
                    </div>
                    <div class="mult_card_salary_sheet_box2_middle">
                        <button class="next_button" type="button" name="" value=""> > </button><br />
                        <button class="next_button" type="button" name="" value=""> >> </button><br />
                        <button class="next_button" type="button" name="" value=""> < </button><br />
                        <button class="next_button" type="button" name="" value=""> << </button>
                    </div>
                    <div class="annual_leave_payment_box2_right">
                        <asp:ListBox ID="ListBox2" Width="260" Height="146" runat="server"></asp:ListBox>
                    </div>
                </div>
                <div class="job_card_button_area">
                    <button class="exclude_button" type="button" name="" value="">Exclude Card</button>
                    <button class="css_btn" type="button" name="" value="">Preview</button> &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                </div>
                <div class="annual_leave_payment_box3">
                    <div class="annual_leave_payment_box3_left">
                        <asp:ListBox ID="ListBox3" Width="260" Height="80" runat="server"></asp:ListBox>
                    </div>
                    <div class="annual_leave_payment_box3_middle">
                        <button class="next_button" type="button" name="" value=""> > </button><br />
                        <button class="next_button" type="button" name="" value=""> < </button>
                    </div>
                    <div class="annual_leave_payment_box3_right">
                        <asp:ListBox ID="ListBox4" Width="260" Height="80" runat="server"></asp:ListBox>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
