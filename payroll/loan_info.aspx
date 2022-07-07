<%@ Page Title="" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="loan_info.aspx.cs" Inherits="SigmaERP.payroll.loan_info" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
           <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="main_box">
    	<div class="main_box_header">
            <h2>Laon Reports</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="rblGenerateType" />
                        <asp:AsyncPostBackTrigger ControlID="ddlDivisionName" />
                        <asp:AsyncPostBackTrigger ControlID="rbEmpTypeList" />
                        <asp:AsyncPostBackTrigger ControlID="ddlCardNo" />
                    </Triggers>
                    <ContentTemplate>

                <div class="monthly_salary_sheet_box1">
                    <fieldset style="margin-left: -150px; width: 573px;">
                        <legend>Report Type</legend>
                            <table>
                                <tr>
                                    <td>
                                       
                                    </td>
                                    <td>
                                       <b> <asp:RadioButtonList ID="rblReportType" runat="server" RepeatDirection="Horizontal" style="margin-left: 204px;">

                                            <asp:ListItem Selected="True">Summary</asp:ListItem>
                                            <asp:ListItem>Details</asp:ListItem>

                                        </asp:RadioButtonList></b>
                                    </td>
                                </tr>
                               
                            </table>
                                
                                 
                            
                    </fieldset>
                </div>
                <div class="monthly_salary_sheet_box2">
                    <div class="monthly_salary_sheet_box2_left">
                        <fieldset>
                            <legend>Employee Type</legend>
                                <table>
                                    <tr>
                                         <td>
                                            <asp:RadioButtonList ID="rbEmpTypeList" runat="server" AutoPostBack="True"  RepeatDirection="Horizontal" OnSelectedIndexChanged="rbEmpTypeList_SelectedIndexChanged"></asp:RadioButtonList>

                                        </td>
                                    </tr>
                                </table>
                        </fieldset>
                    </div>
                    <div class="monthly_salary_sheet_box2_right">
                        <fieldset style="height: 49px;">
                            <legend>Generate Option</legend>
                                <div style="float: left; border-right: 1px solid #ccc; height: 50px; margin-top: -19px; padding-top: 20px; padding-right: 13px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblGenerateType" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblGenerateType_SelectedIndexChanged">
                                                <asp:ListItem Value="All">All</asp:ListItem>
                                                <asp:ListItem Value="Certain" Selected="True">Certain</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                                <div style="float:right">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rbAllAdvanceLastAdvance" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblGenerateType_SelectedIndexChanged" style="margin-top: -80px; margin-right: -1px;">
                                                <asp:ListItem Value="All Loan Info" Selected="false">All  Advance Loan Info    </asp:ListItem>
                                                <asp:ListItem Value="Last Loan Info" Selected="True">Last Loan Info    </asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                </div>
                        </fieldset>
                    </div>
                    
                </div>
                <div class="monthly_salary_sheet_box3">
                    <fieldset>
                        <legend>Selected Option</legend>
                        <table style="margin-left: 246px;">
                            <tr>
                            
                                    <td>
                                          
                                        </td>
                               
                            </tr>
                        </table>
                            <table class="monthly_salary_sheet_box3_table">
                                    <tr id="trddlIndividualCardno" runat="server">
                                        <td></td>
                                        <td>
                                            Individual Card
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCardNo" CssClass="form-control select_width" runat="server" OnSelectedIndexChanged="ddlCardNo_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                        </td>
                                    </tr>
                                <tr id="trtxtIndividualCardno" runat="server">
                                    <td>

                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:TextBox CssClass="form-control text_box_width" ID="txtCardNo" runat="server" placeHolder="Type Card No For Find"></asp:TextBox>
                                    </td>
                                </tr>
                                
                                    <tr id="trddlDividison" runat="server" visible="false">
                                        <td></td>
                                        <td>
                                            Division Name
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDivisionName" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDivisionName_SelectedIndexChanged" ></asp:DropDownList>
                                        </td>
                                    </tr>
                            </table>
                    </fieldset>
                </div>
                        <div class="job_card_box3" id="divJobCardpart" runat="server" visible="false">
                    <div class="job_card_left">
                        <asp:ListBox ID="lstAll" Width="270"  Height="146" SelectionMode="Multiple" runat="server"></asp:ListBox>
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
                                        <asp:Button ID="btnAddAllItem" Class="arrow_button" runat="server" Text=">>" OnClick="btnAddAllItem_Click"  />
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

                <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                    <Triggers>

                    </Triggers>
                    <ContentTemplate>

                   
                <div class="job_card_button_area">
                    <div style="margin:5px; float:left; width: 232px;">
                          <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold;float:left; height: 48px;"> wait processing
                                        <img style="width:26px;height:26px;cursor:pointer; float:left" src="/images/wait.gif"  />
                                          
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                    </div>

                    <asp:Button ID="btnPreview" CssClass="css_btn" runat="server" Text="Preview" style=" margin-left:-188px" OnClick="btnPreview_Click" />
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
