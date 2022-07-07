<%@ Page Title="Promotion Sheet" Language="C#" MasterPageFile="~/personnel_NestedMaster.Master" AutoEventWireup="true" CodeBehind="promotion_sheet.aspx.cs" Inherits="SigmaERP.personnel.promotion_sheet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div class="row">
                  <div class="col-md-12 ds_nagevation_bar">
               <div style="margin-top: 5px">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                       <li> <a class="seperator" href="#"></a>/</li>
                       <li> <a href="/payroll_default.aspx">Payroll</a></li>   
                       <li> <a class="seperator" href="#"></a>/</li>                  
                        <li> <a href="/payroll/salary_index.aspx">Salary</a></li>
                        <li> <a class="seperator" href="#"></a>/</li>
                       <li> <a href="#" class="ds_negevation_inactive Pactive">Promotion Sheet</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>
   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="up1">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlCompany" />
        </Triggers>
        <ContentTemplate>
          <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Promotion Sheet</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">
                        <div class="punishment_against">
<h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center"></h1>        

                            <table runat="server" visible="true" id="tblGenerateType" class="employee_table">
                                <tr id="trCompanyName" runat="server">
                                    <td>Company
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCompany" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                     <td>
                                         Type
                                    </td>
                                    <td></td>
                                    <td>
                                       <asp:RadioButtonList ID="rbEmpList" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"></asp:RadioButtonList>
                                    </td>
                                </tr>
                                                  
                                    <tr>
                                        <td>Month   
                                        </td>
                                        <td>
                                        </td>
                                        <td >
                                            <asp:DropDownList ID="ddlMonthName" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                               
                            </table>
                        </div>
                        <div id="divdepartment" runat="server" visible="false" class="id_card">
                            <div class="id_card_left">
                                <p>Available Departments</p>
                                <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div class="id_card_center">
                                <table class="employee_table">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnAddItem" Class="arrow_button" runat="server" Text=">" OnClick="btnAddItem_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnAddAllItem" Class="arrow_button" runat="server" Text=">>" OnClick="btnAddAllItem_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnRemoveItem" Class="arrow_button" runat="server" Text="<" OnClick="btnRemoveItem_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnRemoveAllItem" Class="arrow_button" runat="server" Text="<<" OnClick="btnRemoveAllItem_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="id_card_right">
                                <p>Selected Departments</p>
                                <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata" ClientIDMode="Static" runat="server"></asp:ListBox>
                            </div>
                        </div>

                        <div class="punishment_button_area">
                            <table class="emp_button_table">
                                <tbody>
                                    <tr>
                                        <th>
                                            <asp:Button ID="btnpreview" CssClass="Pbutton" runat="server" ClientIDMode="Static" Text="Preview" OnClick="btnpreview_Click" /></th>
                                        <th>
                                            <asp:Button ID="btnClose" CssClass="Pbutton" Text="Close" PostBackUrl="~/personnel_defult.aspx" runat="server" /></th>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function goToNewTabandWindow(url) {
            window.open(url);
        }
    </script>
</asp:Content>
