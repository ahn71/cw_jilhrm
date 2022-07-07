<%@ Page Title="Pay Base Shift" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="pay_base_shift.aspx.cs" Inherits="SigmaERP.payroll.pay_base_shift" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="main_box">
    	<div class="main_box_header">
            <h2>Pay Base Shift</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="pay_base_shift_box1">
                    <table class="pay_base_shift_box1_table">
                        <tr>
                            <td>
                                <asp:RadioButton ID="RadioButton1" runat="server" /> Base Shift Emp
                            </td>
                            <td>
                                <asp:RadioButton ID="RadioButton2" runat="server" /> Shift Emp
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="pay_base_shift_box2">
                    <p>Emp Card No : <asp:DropDownList ID="DropDownList1" ClientIDMode="Static" CssClass="form-control select_width" runat="server"></asp:DropDownList></p>
                </div>
                <div class="job_card_button_area">
                    <button class="css_btn" type="button" name="" value="">Preview</button> &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
