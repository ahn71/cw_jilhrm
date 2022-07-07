<%@ Page Title="Employee Bank Info" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="employee_bank_info.aspx.cs" Inherits="SigmaERP.payroll.employee_bank_info" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
    	<div class="main_box_header">
            <h2>Employee Bank Information</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="pay_slip_box1">
                    <fieldset>
                        <legend>Select an Option</legend>
                            <table class="pay_slip_box1_table">
                                <tr>
                                    <td colspan="2"><a href="employee_bank_info.aspx">employee_bank_info.aspx</a>
                                        <asp:RadioButton ID="RadioButton1" runat="server"></asp:RadioButton> All
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RadioButton3" runat="server"></asp:RadioButton> Individual
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownList3" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:RadioButton ID="RadioButton2" runat="server"></asp:RadioButton> Worker
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:RadioButton ID="RadioButton5" runat="server"></asp:RadioButton> Staff
                                    </td>

                                </tr>
                            </table>
                    </fieldset>
                </div>
                <div class="pay_slip_box2">
                    <fieldset>
                        <legend>Month</legend>
                            <p>
                                Month Name : 
                               <asp:DropDownList ID="DropDownList2" ClientIDMode="Static" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                            </p>
                    </fieldset>
                </div>
                <div class="pay_slip_box3">
                    <p>
                        Division : 
                       <asp:DropDownList ID="DropDownList1" ClientIDMode="Static" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                    </p>
                </div>
                <div class="pay_slip_box4">
                    <div class="pay_slip_box4_left">
                        <p>Available Departments</p>
                        <asp:ListBox ID="ListBox1" Width="208" Height="146" runat="server"></asp:ListBox>
                    </div>
                    <div class="pay_slip_box4_middle">
                        <button class="next_button" type="button" name="" value=""> > </button><br />
                        <button class="next_button" type="button" name="" value=""> >> </button><br />
                        <button class="next_button" type="button" name="" value=""> < </button><br />
                        <button class="next_button" type="button" name="" value=""> << </button>
                    </div>
                    <div class="pay_slip_box4_right">
                        <p>Selected Department/s</p>
                        <asp:ListBox ID="ListBox2" Width="208" Height="146" runat="server"></asp:ListBox>
                    </div>
                </div>
                <div class="job_card_button_area">
                    <button class="css_btn" type="button" name="" value="">Preview</button> &nbsp; &nbsp; &nbsp;
                   <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
