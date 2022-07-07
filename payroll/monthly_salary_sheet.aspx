<%@ Page Title="Monthly Salary Sheet" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="monthly_salary_sheet.aspx.cs" Inherits="SigmaERP.payroll.monthly_salary_sheet" %>
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
                       <li> <a href="#" class="ds_negevation_inactive">Monthly Salary Sheet</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>
    <div class="main_box1">
    	<div class="main_box_header">
            <h2>Monthly Salary Sheet</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <asp:UpdatePanel ID="up1" runat="server" >
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
                                         Month
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMonthId" runat="server" ClientIDMode="Static" CssClass="input"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtMonthId_CalendarExtender" Format="MM-yyyy" runat="server" TargetControlID="txtMonthId">
                                        </asp:CalendarExtender>
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
                                            <asp:DropDownList ID="ddlDivisionName" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDivisionName_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>
                            </table>
                    </fieldset>
                </div>
                        <div class="job_card_box3">
                    <div class="job_card_left">
                        <asp:ListBox ID="lstAll" Width="270" SelectionMode="Multiple"  Height="146" runat="server"></asp:ListBox>
                    </div>
                                <div class="id_card_center">
                        <table class="arowBnt_table">                     
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
                        <asp:ListBox ID="lstSelected" Width="270" SelectionMode="Multiple" Height="146" runat="server"></asp:ListBox>
                    </div>
                </div>
                        </ContentTemplate>
                </asp:UpdatePanel>

              

                   
                <div class="job_card_button_area">
                    <div style="margin:5px; float:left; width: 232px;">
                          <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold;float:left; height: 48px;"> wait processing
                                        <img style="width:26px;height:26px;cursor:pointer; float:left" src="/images/wait.gif"  />
                                          
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                    </div>

                    <asp:Button ID="btnPreview" CssClass="css_btn" runat="server" Text="Preview" OnClick="btnPreview_Click" />
                    <asp:Button ID="btnBankSalaryList" CssClass="css_btn" runat="server" Text="Bank Salary" OnClick="btnBankSalaryList_Click"  />
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                </div>

               
                

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
