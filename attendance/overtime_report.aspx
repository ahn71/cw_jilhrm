<%@ Page Title="OT Report" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="overtime_report.aspx.cs" Inherits="SigmaERP.attendance.overtime_report1" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li>/</li>
                    <li><a href="/attendance_default.aspx">Attendance</a></li>
                    <li>/</li>
                    <li><a href="#" class="ds_negevation_inactive Mactive">Overtime Report</a></li>
                </ul>
            </div>
        </div>
    </div>
   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
     <asp:UpdatePanel runat="server" ID="up1">
         <Triggers>
             <asp:AsyncPostBackTrigger  ControlID="ddlCompany" />
             <asp:AsyncPostBackTrigger ControlID="rdbDailyOT" />
             <asp:AsyncPostBackTrigger ControlID="rdbMonthlyOT" />            
         </Triggers>
        <ContentTemplate>
     <div class="main_box Mbox">
         <div class="main_box_header MBoxheader">
            <h2>Overtime Report</h2>
        </div>
         <div class="employee_box_body">
                    <div class="employee_box_content">

                <div class="bonus_generation" style="width:61%; margin:0px auto;">

<h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center"></h1>
                    <table runat="server" visible="true" id="tblGenerateType" class="division_table_leave1">
                        <tbody>
                            <tr>
                                <td>Company 
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCompany" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td>Shift 
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlShift" ClientIDMode="Static" CssClass="form-control select_width" runat="server" >
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td>Type</td>
                                <td>:</td>
                                <td>
                                    <asp:RadioButton ID="rdbDailyOT" Class="" runat="server" Text="Daily OT Report" AutoPostBack="True" Checked="True" OnCheckedChanged="rdbDailyOT_CheckedChanged" />

                                    <asp:RadioButton ID="rdbMonthlyOT"  runat="server" Text="Monthly OT" AutoPostBack="True" OnCheckedChanged="rdbMonthlyOT_CheckedChanged" />
                                </td>
                            </tr>
                            <tr id="trDate" runat="server">
                                <td>Date
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <asp:TextBox ID="dptDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" Format="dd-MM-yyyy" runat="server" TargetControlID="dptDate"></asp:CalendarExtender>
                                    <%--<asp:CalendarExtender ID="CalendarExtender2" Format=yyyy-MM runat="server" TargetControlID="txtMonthName"></asp:CalendarExtender>--%>
                                
                                </td>
                            </tr>
                            <tr id="trMonth" runat="server">
                                <td>Month Name
                                </td>
                                <td>:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlMonthName" ClientIDMode="Static" CssClass="form-control select_width" runat="server">
                                    </asp:DropDownList>
                                    <%--<asp:CalendarExtender ID="CalendarExtender2" Format=yyyy-MM runat="server" TargetControlID="txtMonthName"></asp:CalendarExtender>--%>
                                
                                </td>
                            </tr>
                            <tr runat="server" visible="false">
                                  <td>                               
                                Employee Type
                            </td>
                            <td>:</td>
                            <td >
                                <asp:RadioButtonList runat="server" ID="rblEmpType" AutoPostBack="true" RepeatDirection="Horizontal" >
                                </asp:RadioButtonList>
                            </td>
                            </tr>
                            <tr>

                                <td>Card No
                                </td>
                                <td>:</td>
                                <td>
                                    <asp:TextBox ID="txtCardNo" ClientIDMode="Static" runat="server" PlaceHolder=" For Individual" CssClass="form-control text_box_width"></asp:TextBox>

                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkNew" Text="New" Font-Bold="true" ForeColor="Red" runat="server" OnClientClick="InputBoxNew()"></asp:LinkButton></td>
                            </tr>


                        </tbody>
                    </table>
                    </fieldset>
                    </div>
            <div id="workerlist" runat="server" class="id_card" style="background-color:white; width:61%;">
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
                 <div class="daily_absence_report_box3">
                    <p runat="server" id="loadingImg" style="height:2px; margin:0px">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold;float:left"><p>Please Wait</p> </span> 
                                <img style="width:26px;height:26px;cursor:pointer; float:left; margin-left:15px" src="/images/wait.gif"  /> 
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </p>
                </div>
                <div class="punishment_button_area">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                 <th><asp:Button ID="btnPreview" CssClass="Mbutton" runat="server" ClientIDMode="Static" Text="Preview" OnClick="btnPreview_Click"    /></th>
                                <th><asp:Button ID="btnClose" CssClass="Mbutton" Text="Close" PostBackUrl="~/default.aspx" runat="server" /></th> 
                                
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

         function InputBoxNew() {

             $('#txtCardNo').val('');
         }
    </script>
</asp:Content>
