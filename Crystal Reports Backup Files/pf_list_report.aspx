
<%@ Page Title="Financial Information" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="pf_list_report.aspx.cs" Inherits="SigmaERP.personnel.pf_list_report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12 ds_nagevation_bar">
            <div style="margin-top: 5px">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li><a href="#">/</a></li>
                    <li><a href="/personnel_defult.aspx">Personnel</a></li>
                    <li><a href="#">/</a></li>
                    <li><a href="/personnel/employee_index.aspx">Employee Information</a></li>
                    <li><a href="#">/</a></li>
                    <li><a href="#" class="ds_negevation_inactive">Financial Info Report</a></li>
                </ul>
            </div>

        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
           <asp:AsyncPostBackTrigger ControlID="ddlCompany" />
           <asp:AsyncPostBackTrigger ControlID="ddlShiftList" />  
            <asp:AsyncPostBackTrigger ControlID="rblReportType" />         
        </Triggers>
        <ContentTemplate>
            <div class="row Ptrow">
                <div class="employee_box_header PtBoxheader">
                    <h2>Financial Information Report</h2>
                </div>
                <div class="employee_box_body">
                    <div class="employee_box_content">
                        <div class="punishment_against1">
                            <div class="punishment_against1">

                                <h1 runat="server" visible="false" id="WarningMessage" style="color: red; text-align: center">You Have Not Any Access Permission!</h1>
                                <table runat="server" id="tblGenerateType" class="punishment_against">
                                    <tr id="trCompanyName" runat="server" visible="true">
                                        <td>Company
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true" ClientIDMode="Static" CssClass="form-control select_width" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td>Shift
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:DropDownList ID="ddlShiftList" runat="server" AutoPostBack="true" ClientIDMode="Static" CssClass="form-control select_width" OnSelectedIndexChanged="ddlShiftList_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table class="punishment_against">
                                    <tbody>
                                        <tr>
                                            <td>Employee Type</td>
                                            <td>:</td>
                                            <td>
                                                <asp:RadioButtonList runat="server" ID="rblEmpType" ClientIDMode="Static" RepeatDirection="Horizontal" Font-Bold="true">
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Report Type</td>
                                            <td>:</td>
                                            <td>
                                                <asp:RadioButtonList runat="server" ID="rblReportType" ClientIDMode="Static" RepeatDirection="Horizontal" Font-Bold="true">
                                                    <asp:ListItem Selected="True" Value="PF">PF List</asp:ListItem>
                                                    <asp:ListItem Value="1">Bank Salary List</asp:ListItem>
                                                    <asp:ListItem Value="0">Cash Salay List</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div id="divDepartmentList" runat="server" class="id_card" style="background-color: white; width: 61%;">
                                <div class="id_card_left EilistL">
                                    <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata EilistCec" SelectionMode="Multiple"></asp:ListBox>
                                </div>
                                <div class="id_card_center EilistC">
                                    <table style="margin-top: 0px;" class="employee_table">
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
                                <div class="id_card_right EilistR">
                                    <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata EilistCec" ClientIDMode="Static" runat="server"></asp:ListBox>
                                </div>
                            </div>
                            <div class="punishment_button_area">
                                <table class="emp_button_table">
                                    <tbody>
                                        <tr>
                                            <th>
                                                <asp:Button ID="btnPreview" CssClass="css_btn Ptbut" runat="server" ValidationGroup="save" OnClientClick="return InputValidation();" Text="Preview" OnClick="btnPreview_Click" />
                                            <th>
                                                <asp:Button ID="btnClose" CssClass="css_btn Ptbut" Text="Close" PostBackUrl="~/default.aspx" runat="server" /></th>
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

         //function InputBoxNew()
         //{
           
         //    $('#txtCardNo').val('');
         //}
         //function InputValidation()
         //{
         //    if (validateText('txtDate', 1, 100, "Please Select Attendance Date ") == false) return false;
         //    return true;             
         //}
    </script>
</asp:Content>
