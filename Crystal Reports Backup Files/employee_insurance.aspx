<%@ Page Title="Employee Insurance" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="employee_insurance.aspx.cs" Inherits="SigmaERP.personnel.employee_insurance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="worker_id__main_box">
        <div class="punishment_box_header">
            <h2>Employee List</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content">
        <div class="punishment_against">
        <table class="employee_table">                     
                  <tr>
                        <td width="35%">
                            <asp:RadioButton ID="RadioButton1" Class="" runat="server" Text="New Recruitment" />
                        </td>
                        <td>
                            <asp:RadioButton ID="RadioButton2" runat="server" Text="Separation" />
                        </td>
                   </tr>
            </table>
            </div>

                <div class="punishment_against">
                    
                  <table class="employee_table">                 
                        <tbody>
                     <tr>
                            <td>
                                Month Name
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                               <asp:DropDownList ID="DropDownList2" ClientIDMode="Static" CssClass="form-control select_width" runat="server">
                                    <asp:ListItem>-select-</asp:ListItem>
                                    <asp:ListItem>January</asp:ListItem>
                                    <asp:ListItem>February</asp:ListItem>
                                    <asp:ListItem>March</asp:ListItem>
                                   <asp:ListItem>April</asp:ListItem>
                                    <asp:ListItem>May</asp:ListItem>
                                    <asp:ListItem>June</asp:ListItem>
                                    <asp:ListItem>July</asp:ListItem>
                                   <asp:ListItem>August</asp:ListItem>
                                    <asp:ListItem>September</asp:ListItem>
                                    <asp:ListItem>October</asp:ListItem>
                                    <asp:ListItem>November</asp:ListItem>
                                    <asp:ListItem>December</asp:ListItem>
                                </asp:DropDownList>
                                
                            </td>
                        </tr>          
                      
                    </tbody>
                  </table>
                </div>
                <div class="punishment_button_area">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                <th><button value="" name="" type="button" class="back_button">Preview</button></th>
                                <th><button value="" name="" type="button" class="emp_btn">Close</button></th>   
                         </tr>
                    </tbody>
                  </table>
                </div>

        </div>
      </div>
    </div>
</asp:Content>
