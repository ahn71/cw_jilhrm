<%@ Page Title="Tiffin Bill" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="tiffin_bill.aspx.cs" Inherits="SigmaERP.payroll.tiffin_bill" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="main_box">
    	<div class="main_box_header">
            <h2>Tiffin Bill for Staff</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="tiffin_bill_box1">
                    <table class="tiffin_bill_box1_table">
                        <tr>
                            <td>
                                <asp:RadioButton ID="RadioButton1" runat="server" /> All
                            </td>
                            <td>
                                <asp:RadioButton ID="RadioButton2" runat="server" /> Deptwise
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="tiffin_bill_box2">
                    <p>Month Name : <asp:DropDownList ID="DropDownList1"  CssClass="form-control select_width" runat="server"></asp:DropDownList></p>
                </div>
                <div class="job_card_button_area">
                    <button class="css_btn" type="button" name="" value="">Preview</button> &nbsp;
                   <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
