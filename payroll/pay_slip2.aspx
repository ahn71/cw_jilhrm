<%@ Page Title="PaySlip" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="pay_slip2.aspx.cs" Inherits="SigmaERP.payroll.pay_slip" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:ScriptManager ID="ScriptManager1" runat="server">  </asp:ScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnPreview" />
            <asp:AsyncPostBackTrigger ControlID="ddlMonthID" />
            <asp:AsyncPostBackTrigger  ControlID="ddlDivision"/>
        </Triggers>
        <ContentTemplate>

    <div class="main_box">
       <%-- <a href="pay_slip.aspx">pay_slip.aspx</a>--%>
    	<div class="main_box_header">
            <h2>Pay Slip</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="pay_slip_box1">

                    <fieldset runat="server" id="fldEmpType" visible="false">
                        <legend>Emply Type</legend>
                        <asp:RadioButtonList ID="rdoEmpType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rdoEmpType_SelectedIndexChanged" RepeatDirection="Horizontal" Width="50%">
                        <asp:ListItem>Worker</asp:ListItem>
                        <asp:ListItem>Staff</asp:ListItem>
                    </asp:RadioButtonList>
                    </fieldset>
                    

                    <fieldset>
                        <legend>Selection</legend>
                            <table class="pay_slip_box1_table">
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rbEmpTypeList" runat="server"></asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlType" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                                            <asp:ListItem>Individual</asp:ListItem>
                                            <asp:ListItem Selected="True">Worker</asp:ListItem>
                                            <asp:ListItem>Staff</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlIndividualEmp" CssClass="form-control select_width" runat="server" AutoPostBack="True" >
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                
                            </table>
                    </fieldset>
                </div>
                <div class="pay_slip_box2">
                    <fieldset>
                        <legend>Daily OT</legend>
                            <p>
                                Month Name : 
                               <asp:DropDownList ID="ddlMonthID" ClientIDMode="Static" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                            </p>
                    </fieldset>
                </div>
                <div class="pay_slip_box3">
                    <table>
                        <tr>
                            <td>
                                Division : 
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDivision" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                               Language :
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbLanguage" runat="server" AutoPostBack="True" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="0">Bangla</asp:ListItem>
                                                <asp:ListItem Value="1" Selected="True">English</asp:ListItem>
                                            </asp:RadioButtonList>
                           </td> 
                       </tr>
                    </table>
                        
                       
                    
                </div>
                
                <div class="daily_absence_report_box4">
                    <div class="daily_absence_report_left">
                        <p>Available Departments</p>
                        <asp:ListBox ID="lstEmployees" Width="260" Height="146" runat="server" SelectionMode="Multiple" AutoPostBack="True"></asp:ListBox>
                    </div>
                    <div class="daily_absence_report_middle">

                        <asp:Button ID="btnadditem" CssClass="next_button" runat="server" Text=">" />
                        <br />
                        <asp:Button ID="btnaddall" CssClass="next_button" runat="server" Text=">>"/>
                        <br />
                        <asp:Button ID="btnremoveitem" CssClass="next_button" runat="server" Text="<"/>
                        <br />
                        <asp:Button ID="btnremoveall" CssClass="next_button" runat="server" Text="<<"/>
                    </div>
                    <div class="daily_absence_report_right">
                        <p>Selected Department/s</p>
                        <asp:ListBox ID="lstSelectedEmployees" Width="260" Height="146" runat="server" SelectionMode="Multiple"></asp:ListBox>

                    </div>
                    <br />

                    <div style="margin:5px; float:left; width: 232px;">
                          <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold;float:left; height: 48px;"> wait processing
                                        <img style="width:26px;height:26px;cursor:pointer; float:left" src="/images/wait.gif"  />
                                          
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                    </div>

                </div>
                
                 <div class="job_card_button_area">
                    <asp:Button ID="btnPreview" CssClass="css_btn"  ValidationGroup="save" runat="server" Text="Preview" OnClick="btnPreview_Click" />
                    &nbsp; &nbsp; &nbsp;   
                    <asp:Button ID="btnPreview2" CssClass="css_btn"  ValidationGroup="save" runat="server" Text="Pay Slip2" OnClick="btnPreview2_Click"  />
                                   
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                </div>

            </div>
        </div>
    </div>


    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
