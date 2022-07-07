<%@ Page Title="Festival Bonus" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="festival_bonus.aspx.cs" Inherits="SigmaERP.payroll.festival_bonus" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="main_box">
    	<div class="main_box_header">
            <h2>&nbsp;Festival Bonus Sheet</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                       <asp:AsyncPostBackTrigger ControlID="rblSelectEmployeeType" />
                        <asp:AsyncPostBackTrigger ControlID="ddlDivisionName" />
                    </Triggers>
                    <ContentTemplate>
                       
                <div class="monthly_salary_sheet_box1">
                    <fieldset style="margin-left: -82px;">
                        <legend>Selection</legend>
                            <table>
                                <tr>
                                    <td>
                                         Bonus Type
                                    </td>
                                    <td>
                                       
                                      
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMonthID"  ClientIDMode="Static" CssClass="form-control select_width" OnChange="getSalaryMonth();" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                                
                                 
                            
                    </fieldset>
                </div>
                <div class="monthly_salary_sheet_box2">
                    <div class="monthly_salary_sheet_box2_left">
                        <fieldset>
                            <legend>Select Employee Type</legend>
                                <table>
                                    <tr>
                                         <td>
                                            <asp:RadioButtonList ID="rbEmpTypeList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbEmpTypeList_SelectedIndexChanged"></asp:RadioButtonList>

                                        </td>
                                    </tr>
                                </table>
                        </fieldset>
                    </div>
                    <div class="monthly_salary_sheet_box2_right">
                        <fieldset style="height: 115px;">
                            <legend>Select Language</legend>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rbLanguage" runat="server" AutoPostBack="True">
                                                <asp:ListItem Value="0">Bangla</asp:ListItem>
                                                <asp:ListItem Value="1" Selected="True">English</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                        </fieldset>
                    </div>
                    
                </div>
                <div class="monthly_salary_sheet_box3">
                    <fieldset>
                        <legend>Selected Option</legend>
                        <table style="margin-left: 246px;">
                            <tr>
                               
                                      <td>
                                            <asp:RadioButtonList ID="rblSelectEmployeeType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblSelectEmployeeType_SelectedIndexChanged" RepeatDirection="Horizontal"></asp:RadioButtonList>
                                        </td>
                               
                            </tr>
                        </table>
                            <table class="monthly_salary_sheet_box3_table">
                                 
                                    <tr>
                                        <td></td>
                                        <td>
                                            Individual Card
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCardNo" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            Division Name
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDivisionName" CssClass="form-control select_width" runat="server" OnSelectedIndexChanged="ddlDivisionName_SelectedIndexChanged" AutoPostBack="True" ></asp:DropDownList>
                                        </td>
                                    </tr>
                            </table>
                    </fieldset>
                </div>
                                        <div class="job_card_box3">
                    <div class="job_card_left">
                        <asp:ListBox ID="lstAll" Width="270"  Height="146" runat="server"></asp:ListBox>
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
                    <div class="job_card_right">
                        <asp:ListBox ID="lstSelected" Width="270" Height="146" runat="server"></asp:ListBox>
                    </div>
                </div>
                        </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                    <Triggers>

                    </Triggers>
                    <ContentTemplate>

                   
                <div class="job_card_button_area">
                    <asp:Button ID="btnPreview" CssClass="css_btn" runat="server" Text="Preview" OnClick="btnPreview_Click"/>
                    <asp:Button ID="btnBankBonusList" CssClass="css_btn" runat="server" Text="Bank Bonus List" OnClick="btnBankBonusList_Click" />
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                </div>

                 </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>

    <script type="text/javascript">
        function getSalaryMonth() {

            var val = document.getElementById('ddlMonthID').value;
            document.getElementById('txtMonthId').value = val;

        }

        function CloseWindowt() {
            window.close();
        }

        function goToNewTabandWindow(url) {
            window.open(url);

        }

    </script>
</asp:Content>
