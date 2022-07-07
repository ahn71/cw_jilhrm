<%@ Page Title="Out Duty Report" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="outduty_report.aspx.cs" Inherits="SigmaERP.attendance.outduty_report" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .division_table_leave1 {
            width: 100%;
        }

        .division_table_leave1 tr {
                height: 35px;
        }        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li>/</li>
                    <li><a href="/attendance_default.aspx">Attendance</a></li>
                    <li>/</li>
                    <li><a href="#" class="ds_negevation_inactive Mactive">Out Duty Report</a></li>
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
        </Triggers>
        <ContentTemplate>
            <div class="main_box Mbox">
         <div class="main_box_header MBoxheader">
                    
                    <h2>Out Duty Report</h2>
                </div>
               <%-- <div class="main_box_body">
                    <div class="main_box_content">
                        <div style="text-align:-moz-center" class="bonus_generation">--%>
                <div class="employee_box_body">
                    <div class="employee_box_content">

                <div class="bonus_generation outduty-report-outer">                  

<h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center"></h1>
                    <table runat="server" visible="true" id="tblGenerateType" class="division_table_leave1">
                        <tr>
                            <td>
                                <div class="col-md-12 outduty-report">
                                    <div class="row" id="trCompanyName" runat="server" visible="false">
                                        <div class="col-md-12">
                                            <div class="form-group row">
                                                <label class="col-md-2" for="email">Company <span class="colon-list">:</span></label>
                                                <div class="col-md-10">
                                                    <asp:DropDownList ID="ddlCompany" runat="server" AutoPostBack="true" ClientIDMode="Static" CssClass="form-control select_width" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                            </asp:DropDownList>
                                                 </div>
                                             </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="row form-group">
                                                <label class="col-md-3" for="txtFromDate">From Date <span class="colon-list">:</span></label>
                                                <div class="col-md-7">
                                                     <asp:TextBox ID="txtFromDate" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width key-off" autocomplete="off"></asp:TextBox>
                                                    <asp:CalendarExtender
                                                        ID="CalendarExtender1" Format="dd-MM-yyyy" runat="server" Enabled="True" TargetControlID="txtFromDate">
                                                    </asp:CalendarExtender>
                                                 </div>
                                                <div class="col-md-2"></div>
                                             </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group row">
                                                <label class="col-md-3" for="txtToDate">To Date <span class="colon-list">:</span></label>
                                                <div class="col-md-7">
                                                     <asp:TextBox ID="txtToDate" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width key-off" autocomplete="off"></asp:TextBox>
                                                    <asp:CalendarExtender
                                                        ID="TextBoxDate_CalendarExtender" Format="dd-MM-yyyy" runat="server" Enabled="True" TargetControlID="txtToDate">
                                                    </asp:CalendarExtender>
                                                 </div>
                                                <div class="col-md-2"></div>
                                             </div>
                                        </div>
                                        <div class="col-md-6 hidden-xs hidden-sm">
                                            <div class="form-group row">
                                                <label class=" col-md-3" for="rblEmpType">Employee Type <span class="colon-list">:</span></label>
                                                <div class="col-md-7">
                                                    <asp:RadioButtonList runat="server" ID="rblEmpType"  RepeatDirection="Horizontal">
                                            </asp:RadioButtonList>
                                                 </div>
                                                <div class="col-md-2"></div>
                                             </div>
                                        </div>
                                        <div class="col-md-6 hidden-xs hidden-sm">
                                            <div class="form-group row">
                                                <label class="col-md-3" for="txtCardNo"> Card No <span class="colon-list">:</span></label>
                                                <div class="col-md-5">
                                                      <asp:TextBox ID="txtCardNo" ClientIDMode="Static" runat="server" PlaceHolder=" For Individual" CssClass="form-control text_box_width"></asp:TextBox>
                                                 </div>
                                                <div class="col-md-2">
                                                    <asp:LinkButton ID="lnkNew" class="btn btn-primary" Text="New" runat="server" OnClientClick="InputBoxNew()"></asp:LinkButton>
                                                </div>
                                             </div>
                                        </div>
                                    </div>
                                  </div>
                                 </td>
                               </tr>
                    </table>
                </div>
                      <div id="workerlist" runat="server" class="id_card outduty-report-outer">
                            <div class="id_card_left EilistL">
                                <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata EilistCec" style="height:270px !important" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div class="id_card_center EilistC" >
                                <table style="margin-top:60px;" class="employee_table">                    
                              <tr>
                                    <td >
                                        <asp:Button ID="btnAddItem" Class="arrow_button" runat="server" Text=">" OnClick="btnAddItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnAddAllItem" Class="arrow_button" runat="server" Text=">>" OnClick="btnAddAllItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveItem" Class="arrow_button" runat="server" Text="<" OnClick="btnRemoveItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveAllItem" Class="arrow_button" runat="server" Text="<<" OnClick="btnRemoveAllItem_Click"  />
                                    </td>
                               </tr>
                        </table>
                    </div>
                     <div class="id_card_right EilistR">
                                <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata EilistCec"  style="height:270px !important"  ClientIDMode="Static" runat="server"></asp:ListBox>
                            </div>
                </div>
                        <div class="job_card_button_area">                           
                            <asp:Button ID="btnPreview" CssClass="Mbutton" runat="server" ValidationGroup="save"  Text="Preview" OnClick="btnPreview_Click" />
                            &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="Mbutton" />
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

        
         function InputBoxNew()
         {
           
             $('#txtCardNo').val('');
         }
        
    </script>
</asp:Content>