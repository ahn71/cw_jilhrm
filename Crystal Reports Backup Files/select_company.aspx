<%@ Page Title="Company Name" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="select_company.aspx.cs" Inherits="SigmaERP.tools.select_company" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
    	<div class="main_box_header">
            <h2>Running Company: Company Name</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="select_company">
                    <table class="select_company_table">
                        <tr>
                            <th>Name</th>
                            <th>From Date</th>
                            <th>To Date</th>
                        </tr>
                        <tr>
                            <td>Your Company Name</td>
                            <td>01-02-2012</td>
                            <td>01-02-2014</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </div>


                <div class="late_report_button">
                    <button class="css_btn" type="submit" name="" value="">OK</button> &nbsp; &nbsp;
                    <button class="css_btn" type="button" name="" value="">Close</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
