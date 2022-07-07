<%@ Page Title="Shift Change Individual" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="shift_change_individual.aspx.cs" Inherits="SigmaERP.payroll.shift_change_individual" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
    	<div class="main_box_header">
            <h2>Shift Change Individual</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="input_division_info">
                    <table class="division_table">
                        <tr>
                            <td>
                                Card Number
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox2" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Effective From
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox5" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Previous Base Shift Type
                            </td>
                            <td>
                                :
                            </td>
                            <td>

                                <asp:TextBox ID="TextBox1" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Current Base Shift Type
                            </td>
                            <td>
                                :
                            </td>
                            <td>

                                <asp:TextBox ID="TextBox3" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Change Shift
                            </td>
                            <td>
                                :
                            </td>
                            <td>

                                <asp:TextBox ID="TextBox4" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="button_area">
                    <table class="button_table">
                        <tr>
                            <th><a href="http://localhost:2374/" class="back_button">Back</a></th>
                            <th><asp:Button ID="Button1"  PostBackUrl="/payroll/shift_change_individual_list_all.aspx" runat="server" Text="List All" CssClass="css_btn" /></th>
                            <th><button class="css_btn" type="button" name="" value="">Save</button></th>
                            <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                            
                        </tr>
                    </table>
                </div>

				
            </div>
        </div>
    </div>
</asp:Content>
