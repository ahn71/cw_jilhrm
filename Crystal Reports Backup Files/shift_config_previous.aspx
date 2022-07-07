<%@ Page Title="Shift Configuration Previous" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="shift_config_previous.aspx.cs" Inherits="SigmaERP.hrd.shift_config_previous" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main_box">
    	<div class="main_box_header">
            <h2>Shift Configuration Previous</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="input_division_info">
                    <table class="division_table">
                        <tr>
                            <td>
                                Shift Name
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
                                Effective Date
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownList1" ClientIDMode="Static" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Start Time
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
                                End Time
                            </td>
                            <td>
                                :
                            </td>
                            <td>

                                <asp:TextBox ID="TextBox4" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Acceptable Late
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
                                Delay Time Out
                            </td>
                            <td>
                                :
                            </td>
                            <td>

                                <asp:TextBox ID="TextBox6" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Over Time
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox7" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        
                    </table>
                </div>

                <div class="button_area">
                    <table class="button_table">
                        <tr>
                            <th><asp:Button ID="Button1"  PostBackUrl="/hrd/config_priview_list.aspx" runat="server" Text="List All" CssClass="back_button" /></th>
                            <th><button class="css_btn" type="button" name="" value="">Save</button></th>
                            <th><a class="css_btn" href="../default.aspx" >Close</a></th>
                            
                        </tr>
                    </table>
                </div>

				
            </div>
        </div>
    </div>
</asp:Content>
