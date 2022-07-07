<%@ Page Title="Monthly Attendance" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="monthly_attend.aspx.cs" Inherits="SigmaERP.attendance.monthly_attend" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">

    </asp:ScriptManager>   
     <asp:UpdatePanel runat="server" ID="up1">
         <Triggers>
             <asp:AsyncPostBackTrigger ControlID="rdbStaff" />
             <asp:AsyncPostBackTrigger ControlID="rdbWorker" />
             <asp:AsyncPostBackTrigger ControlID="ddlDivision" />

         </Triggers>
        <ContentTemplate>
    <div class="worker_id__main_box">
        <div class="punishment_box_header">
            <h2>Monthly Attendance</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content">
        <div class="punishment_against">
        <table class="employee_table">                     
                  <tr>
                        <td width="27%">
                            <asp:RadioButton ID="rdbStaff" Class="" runat="server" Text="Staff" AutoPostBack="True"  Checked="True" OnCheckedChanged="rdbStaff_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdbWorker" runat="server" Text="Worker" AutoPostBack="True" OnCheckedChanged="rdbWorker_CheckedChanged"  />
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
                               <asp:DropDownList ID="ddlMonthName" ClientIDMode="Static" CssClass="form-control select_width" runat="server"  AutoPostBack="True">
                                   
                                </asp:DropDownList>
                                 <%--<asp:CalendarExtender ID="CalendarExtender2" Format=yyyy-MM runat="server" TargetControlID="txtMonthName"></asp:CalendarExtender>--%>
                                
                            </td>
                        </tr>  
                       <tr>
                            <td>
                                Division
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                               <asp:DropDownList ID="ddlDivision" ClientIDMode="Static" CssClass="form-control select_width" runat="server" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" AutoPostBack="True">
                                   
                                </asp:DropDownList>
                                
                            </td>
                        </tr>           
                      
                    </tbody>
                  </table>
                </div>
                    

                <div class="id_card">
                    <div class="id_card_left">
                        <p>Available Departments</p>
                        <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata" SelectionMode="Multiple"></asp:ListBox>
                    </div>
                    <div class="id_card_center">
                        <table class="employee_table">                     
                              <tr>
                                    <td>
                                        <asp:Button ID="btnAddItem" Class="arrow_button" runat="server" Text=">" OnClick="btnAddItem_Click"   />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnAddAllItem" Class="arrow_button" runat="server" Text=">>" OnClick="btnAddAllItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveItem" Class="arrow_button" runat="server" Text="<" OnClick="btnRemoveItem_Click"   />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveAllItem" Class="arrow_button" runat="server" Text="<<" OnClick="btnRemoveAllItem_Click"  />
                                    </td>
                               </tr>
                        </table>
                    </div>
                    <div class="id_card_right">
                        <p>Selected Departments</p>
                          <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata" ClientIDMode="Static" runat="server"></asp:ListBox>
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
                                 <th><asp:Button ID="btnpreview" CssClass="back_button" runat="server" ClientIDMode="Static" Text="Preview" OnClick="btnpreview_Click"   /></th>
                                <th><asp:Button ID="btnClose" CssClass="css_btn" Text="Close" PostBackUrl="~/default.aspx" runat="server" /></th> 
                                
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
