<%@ Page Title="Rejoin List" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="rejoin_list.aspx.cs" Inherits="SigmaERP.personnel.rejoin_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="rejoin_list_main_box">               
                 <div class="punishment_box_header">
                    <h2>Employee Rejoined</h2>
                </div>
     
                 <div class="rejoin_list_box_body">  
                          <div class="rejoin_table_list">
                                <table border="1" class="employee_list_table"  cellspacing="" cellpadding="0">
                                        <tr bgcolor="#ffffff">
                                            <th>Emp Card No</th>
                                            <th>Rejoin Date</th>
                                            <th>Employee Status</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                              </div>
                     <div class="punishment_button_area">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                <th><asp:Button ID="Button4" CssClass="back_button" runat="server" Text="Alter" /></th>
                                <th><asp:Button ID="Button1" CssClass="emp_btn" runat="server" Text="Delete" /></th>
                                <th><asp:Button ID="Button2" CssClass="emp_btn" runat="server" Text="Find" /></th> 
                                <th><asp:Button ID="Button3" CssClass="emp_btn" runat="server" Text="Close" /> </th> 
                         </tr>
                    </tbody>
                  </table>
                </div>
             </div>
      </div>
</asp:Content>
