<%@ Page Title="Edit Company Information" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="edit_company.aspx.cs" Inherits="SigmaERP.tools.edit_company" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
    	<div class="main_box_header">
            <h2>Edit Company Information</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="edit_company">
                    <table class="edit_company_table">
                        <tr>
                            <td>Company ID : </td>
                            <td><asp:TextBox ID="TextBox2" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Company Name : </td>
                            <td><asp:TextBox ID="TextBox1" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>#CDsbdd : </td>
                            <td><asp:TextBox ID="TextBox3" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Address : </td>
                            <td><asp:TextBox ID="TextBox4" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp; </td>
                            <td><asp:TextBox ID="TextBox5" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>wVkvdc :  </td>
                            <td><asp:TextBox ID="TextBox6" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>&nbsp; </td>
                            <td><asp:TextBox ID="TextBox7" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Country : </td>
                            <td><asp:TextBox ID="TextBox8" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Telephone : </td>
                            <td><asp:TextBox ID="TextBox9" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Fax : </td>
                            <td><asp:TextBox ID="TextBox10" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Default Currency : </td>
                            <td><asp:TextBox ID="TextBox11" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Business Type : </td>
                            <td><asp:TextBox ID="TextBox12" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Financial Year From : </td>
                            <td><asp:TextBox ID="TextBox13" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Financial Year To : </td>
                            <td><asp:TextBox ID="TextBox14" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Multiple Branch : </td>
                            <td><asp:TextBox ID="TextBox15" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>User Access Control : </td>
                            <td><asp:TextBox ID="TextBox16" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Comments : </td>
                            <td><asp:TextBox ID="TextBox17" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox></td>
                        </tr>
                    </table>
                </div>


                <div class="late_report_button">
                    <button class="css_btn" type="submit" name="" value="">Update</button> &nbsp; &nbsp;
                    <button class="css_btn" type="button" name="" value="">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
