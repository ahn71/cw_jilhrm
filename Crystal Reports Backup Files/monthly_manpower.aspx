<%@ Page Title="Monthly Manpower Report" Language="C#" MasterPageFile="~/personnel_NestedMaster.Master" AutoEventWireup="true" CodeBehind="monthly_manpower.aspx.cs" Inherits="SigmaERP.personnel.monthly_manpower" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tdWidth {
            width: 400px;
            height: 40px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <div class="row">
        <div class="col-md-12 ">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li><a href="#">/</a></li>
                    <li><a href="/personnel_defult.aspx">Personnel</a></li>
                    <li><a href="#">/</a></li>
                    <li><a href="/personnel/employee_index.aspx">Employee Information</a></li>
                    <li><a href="#">/</a></li>
                    <li><a href="#" class="ds_negevation_inactive Ptactive">Monthly Man Power Status</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
      <div class="row Ptrow">

                <div class="employee_box_header PtBoxheader">
                    <h2>Monthly Man Power Status Report</h2>
                </div>
                <div class="employee_box_body">
                    <div class="employee_box_content">

                        <div class="punishment_against">
                          <h1 runat="server" visible="false" id="WarningMessage" style="color: red; text-align: center"></h1>
                            <table runat="server" visible="true" id="tblGenerateType" class="employee_table">
                                <tr>
                                    <td>Company
                                    </td>
                                    <td>:
                                    </td>
                                    <td class="tdWidth">
                                        <asp:DropDownList ID="ddlCompanyy" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyy_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <%--<asp:CalendarExtender ID="CalendarExtender2" Format=yyyy-MM runat="server" TargetControlID="txtMonthName"></asp:CalendarExtender>--%>                                
                                    </td>
                                </tr>
                                <tr>
                                    <td>Shift
                                    </td>
                                    <td>:
                                    </td>
                                    <td class="tdWidth">
                                        <asp:DropDownList ID="ddlShift" ClientIDMode="Static" CssClass="form-control select_width" Width="98%" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>

                                    <td>Employee Type</td>
                                    <td>:</td>
                                    <td class="tdWidth">
                                        <asp:RadioButtonList runat="server" ID="rblEmpType" RepeatDirection="Horizontal">
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>From Date
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                        <asp:CalendarExtender
                                            ID="TextBoxDate_CalendarExtender" Format="dd-MM-yyyy" runat="server" Enabled="True" TargetControlID="txtDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>To Date
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                        <asp:CalendarExtender
                                            ID="CalendarExtender1" Format="dd-MM-yyyy" runat="server" Enabled="True" TargetControlID="txtFromDate">
                                        </asp:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                </div>

                <div class="punishment_button_area">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                <th> <asp:Button ID="btnPreview" CssClass="css_btn Ptbut" runat="server" ValidationGroup="save" Text="Preview" OnClick="btnPreview_Click"  /></th>
                                <th><asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/personnel/employee_index.aspx" CssClass="css_btn Ptbut" /></th>   
                         </tr>
                    </tbody>
                  </table>
                </div>

        </div>
      </div>
    </div>
     <script type="text/javascript">
         $(document).ready(function () {

             $("#ddlShift").select2();

         });
         function loadcardNo() {
             $("#ddlShift").select2();
         }
         function goToNewTabandWindow(url) {

             window.open(url);
             loadcardNo();

         }
    </script>
</asp:Content>
