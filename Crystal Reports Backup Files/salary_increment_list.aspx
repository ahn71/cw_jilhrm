<%@ Page Title="Salary Increment List" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="salary_increment_list.aspx.cs" Inherits="SigmaERP.personnel.salary_increment_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="salary_incre_list_main_box">               
                 <div class="punishment_box_header">
                    <h2>Salary Increment List</h2>
                </div>
     
                 <div class="salary_incre_list_box_body">  
                          <div class="salary_incre_table_list">
                                <table border="1" class="employee_list_table"  cellspacing="" cellpadding="0">
                                        <tr bgcolor="#ffffff">
                                            <th>Emp Card No</th>
                                            <th>Eff. Date</th>
                                            <th>Eff. Date</th>
                                            <th>Old Basic</th>
                                            <th>New Basic</th>
                                            <th>Old Gross</th>
                                            <th>New Gross</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                              </div>
                     <div class="punishment_button_area">
                         <div class="promotion_list_btn_left">       
                                <table class="emp_button_table">
                                    <tbody>
                                        <tr>
                                            <td><asp:Button ID="Button2" CssClass="back_button" runat="server" Text="Alter" /></td>
                                            <td><asp:Button ID="Button3" CssClass="emp_btn" runat="server" Text="Delete" /></td>
                                     </tr>
                                </tbody>
                              </table>
                        </div>
                         <div class="promotion_list_btn_right">
                             <table class="emp_button_table">
                                    <tbody>
                                        <tr>
                                            <td><asp:Button ID="Button4" CssClass="emp_btn" runat="server" Text="Find" /></td>
                                            <td><asp:Button ID="Button1" CssClass="emp_btn" runat="server" Text="Close" /></td>
                                     </tr>
                                </tbody>
                              </table>
                         </div>
                </div>
             </div>
      </div>
</asp:Content>
