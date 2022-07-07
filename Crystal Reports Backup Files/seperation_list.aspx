<%@ Page Title="Seperation List" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="seperation_list.aspx.cs" Inherits="SigmaERP.personnel.seperation_sheet_list" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="salary_incre_list_main_box">               
                 <div class="punishment_box_header">
                    <h2>Seperation List</h2>
                </div>
     
                 <div class="salary_incre_list_box_body">  
                          <div class="salary_incre_table_list">
                                <table border="1" class="employee_list_table"  cellspacing="" cellpadding="0">
                                        <tr bgcolor="#ffffff">
                                            <th>Emp Card No</th>
                                            <th>Eff. Date</th>
                                            <th>Eff. Date</th>
                                            <th>Old Basic</th>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td></td>
                                            <td></td>
                                            <td></td>
                                        </tr>
                                    </table>
                              </div>

                  <div class="punishment_button_area">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                <th><asp:Button ID="Button1" CssClass="back_button" runat="server" Text="Alter" /></th>
                                <th><asp:Button ID="Button2" CssClass="emp_btn" runat="server" Text="Delete" /></th>
                                <th><asp:Button ID="Button3" CssClass="emp_btn" runat="server" Text="Find" /></th>   
                                <th><asp:Button ID="Button4" CssClass="emp_btn" runat="server" Text="Close" /></th>  
                         </tr>
                    </tbody>
                  </table>
                </div>
                  
                       
                </div>
      </div>
</asp:Content>
