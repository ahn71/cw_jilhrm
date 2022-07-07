<%@ Page Title="Employee PF" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="employee_pf.aspx.cs" Inherits="SigmaERP.payroll.employee_pf" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="main_box">
    	<div class="main_box_header">
            <h2>Employee PF</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="employee_pf_box1">
                    <table class="employee_pf_box1_table">
                        <tr>
                            <td>
                                Form Date : 
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList1"  CssClass="form-control select_width" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                To Date : 
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList2"  CssClass="form-control select_width" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="job_card_button_area">
                    <button class="css_btn" type="button" name="" value="">Preview</button> &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
