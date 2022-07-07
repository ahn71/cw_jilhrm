<%@ Page Title="Shift Configuration" Language="C#" MasterPageFile="~/hrd_nested.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="shift_config.aspx.cs" Inherits="SigmaERP.hrd.shift_config" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../scripts/jquery-1.8.2.js"></script>
        <script type="text/javascript">

            var oldgridcolor;
            function SetMouseOver(element) {
                oldgridcolor = element.style.backgroundColor;
                element.style.backgroundColor = '#ffeb95';
                element.style.cursor = 'pointer';
                // element.style.textDecoration = 'underline';
            }
            function SetMouseOut(element) {
                element.style.backgroundColor = oldgridcolor;
                // element.style.textDecoration = 'none';

            }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row Rrow">
                  <div class="col-md-12 ds_nagevation_bar">
               <div style="margin-top: 5px">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                       <li> <a href="#">/</a></li>
                       <li> <a href="/hrd_default.aspx">Settings</a></li>
                       <li> <a href="#">/</a></li>
                       <li> <a href="#" class="ds_negevation_inactive Ractive">Shift</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:HiddenField ID="hdnUpdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnbtnStage" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upSave" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upupdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="updelete" runat="server" ClientIDMode="Static" />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">

                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" />
                           
                               <asp:AsyncPostBackTrigger ControlID="rblOverTime" />
                                <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                            </Triggers>
                            <ContentTemplate>

              <div class="main_box RBox">
    	<div class="main_box_header RBoxheader">
            <h2>Shift Configuration Panel</h2>
        </div>
    	<div class="main_box_body Rbody">
        	<div class="main_box_content">
                <div class="input_division_info RTable">
                    <table>
                       
                        <tr>
                            <td colspan="4">
                                <span style="text-align:left;display:block;">Company <span class="requerd1">*</span></span>
                                <div style="text-align:left;display:block;">
                               
                                     <asp:DropDownList ID="ddlCompanyList" runat="server" ClientIDMode="Static" CssClass="form-control" style="width:89%" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ></asp:DropDownList>
                                    </div>

                            </td>
                        </tr>
                        <tr>
                           
                            <td>
                                <span style="text-align:left;display:block;">Department <span class="requerd1">*</span></span>
                                <div style="text-align:left;display:block;">
                               
                                     <asp:DropDownList ID="ddlDepartmentList" runat="server" CssClass="form-control select_width" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartmentList_SelectedIndexChanged" ></asp:DropDownList>
                                    </div>

                                

                            </td>
                            <td></td>
                            <td>
                              <span style="text-align:left;display:block;">Shift Name <span class="requerd1">*</span></span>
                            
                                 <div style="margin-top: 0px;">  
                                      <asp:TextBox ID="txtShiftName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                   </div>
                               
                               
                            </td>
                             <asp:CheckBox ID="chkIsInitial" runat="server" Text="Is Initial" style="float: right; position: relative; top: 80px; color: rgb(255, 0, 0); font-weight: bold; right: -22px;" />
                        </tr>

                           <tr style="display:none">
                               
                            <td>
                                <span style="text-align:left;display:block;">From Date <span class="requerd1">*</span></span>
                                <div style="text-align:left;display:block;">
                                <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server"  Format="dd-MM-yyyy" TargetControlID="txtFromDate">
                                </asp:CalendarExtender>
                                </div>

                            </td>
                             
                                 <td></td>
                       
                            <td>
                                 <span style="text-align:left;display:block;">To Date <span class="requerd1">*</span></span>
                                <div style="text-align:left;display:block;">
                                <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Format="dd-MM-yyyy" TargetControlID="txtToDate">
                                </asp:CalendarExtender>
                                </div>
                            </td>
                               
                        </tr>
                        
                      
                        <tr>
                           
                            <td>
                                <table>
                                    <tr>
                                        <td colspan="4">Shift Start Time (HH:MM) <span class="requerd1">*</span></td>
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="txtStartTimeHH" style="text-align:center" runat="server" placeHolder="00" ClientIDMode="Static" CssClass="form-control text_box_width RInput" ></asp:TextBox></td>
                                        <td>MM</td>
                                        <td><asp:TextBox ID="txtStartTimeMM" runat="server" style=" text-align:center" ClientIDMode="Static" CssClass="form-control text_box_width RInput" >00</asp:TextBox></td>
                                        <td>
                                            <asp:DropDownList ID="ddlStartTimeAMPM" CssClass="form-control select_width RSelt" runat="server" ClientIDMode="Static">
                                                <asp:ListItem Value="AM">AM</asp:ListItem>
                                                <asp:ListItem Value="PM">PM</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                           
                             <td></td>
                           
                            <td>
                                <table>
                                    <tr>
                                        <td colspan="4">Start Punch Count Time(HH:MM) <span class="requerd1">*</span></td>
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="txtPunchCountHH" style=" text-align:center" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width RInput" >00</asp:TextBox></td>
                                        <td>MM</td>
                                        <td><asp:TextBox ID="txtPunchCountMM" runat="server" style=" text-align:center" ClientIDMode="Static" CssClass="form-control text_box_width RInput" >00</asp:TextBox></td>
                                        <td>
                                            <asp:DropDownList ID="ddlPunchCountAMPM" CssClass="form-control select_width RSelt" runat="server" ClientIDMode="Static">
                                                <asp:ListItem Value="AM">AM</asp:ListItem>
                                                <asp:ListItem Value="PM">PM</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>

                        <tr>
                            
                            <td>                     
                                <table>
                                    <tr>
                                        <td colspan="4"> Shift End Time(HH:MM) <span class="requerd1">*</span></td>
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="txtEndTimeHH" style=" text-align:center" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width RInput" >00</asp:TextBox></td>
                                        <td>MM</td>
                                        <td><asp:TextBox ID="txtEndTimeMM" style=" text-align:center" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width RInput" >00</asp:TextBox></td>
                                        <td>
                                            <asp:DropDownList ID="ddlEndTimeAMPM" CssClass="form-control select_width RSelt" runat="server" ClientIDMode="Static">
                                                <asp:ListItem Value="AM">AM</asp:ListItem>
                                                <asp:ListItem Value="PM">PM</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                             <td></td>
                           <td>
                                <span style="float:left"> Acceptable Late(MM) <span class="requerd1">*</span></span> <br />
                                <asp:TextBox ID="txtAcceptableLate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" ToolTip="Type Integer Number">0</asp:TextBox>
                            </td>
                        </tr>
                      

                         <tr>
                           
                            <td>
                                <table>
                                    <tr>
                                        <td colspan="4" style="color:red" >Break Start Time (HH:MM) <span class="requerd1">*</span></td>
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="txtBreakSHour" style="text-align:center" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width RInput" >00</asp:TextBox></td>
                                        <td>MM</td>
                                        <td><asp:TextBox ID="txtBreakSMinute" runat="server" style=" text-align:center" ClientIDMode="Static" CssClass="form-control text_box_width RInput" >00</asp:TextBox></td>
                                        <td>
                                            <asp:DropDownList ID="ddlBreakStartTime" CssClass="form-control select_width RSelt" runat="server" ClientIDMode="Static">
                                                <asp:ListItem Value="AM">AM</asp:ListItem>
                                                <asp:ListItem Value="PM">PM</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                           
                             <td></td>
                           
                            <td>
                                <table>
                                    <tr>
                                        <td colspan="4" style="color:red" >Break End Time(HH:MM) <span class="requerd1">*</span></td>
                                    </tr>
                                    <tr>
                                        <td><asp:TextBox ID="txtBreakEndHour" style=" text-align:center" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width RInput" >00</asp:TextBox></td>
                                        <td>MM</td>
                                        <td><asp:TextBox ID="txtBreakEndMinute" runat="server" style=" text-align:center" ClientIDMode="Static" CssClass="form-control text_box_width RInput" >00</asp:TextBox></td>
                                        <td>
                                            <asp:DropDownList ID="ddlBreakEndTime" CssClass="form-control select_width RSelt" runat="server" ClientIDMode="Static">
                                                <asp:ListItem Value="AM">AM</asp:ListItem>
                                                <asp:ListItem Value="PM">PM</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>



                        <tr>
                            <td>
                                 <span style="float:left">Over Time <span class="requerd1">*</span></span> <br />
                                <div style="float:left">
                                  <asp:RadioButtonList ID="rblOverTime" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblOverTime_SelectedIndexChanged">
                                    <asp:ListItem  Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                  </asp:RadioButtonList> 
                                </div>
                            </td>
                             <td></td>
                            <td>
                              <span style="float:left">Acceptable Min as OT <span class="requerd1">*</span></span> <br />
                                <asp:TextBox ID="txtAcceptableOTMin" CssClass="form-control text_box_width"  runat="server" style="text-align:center; font-weight:bold " Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <span style="float:left">Active <span class="requerd1">*</span></span> <br />
                                <asp:RadioButtonList ID="rblActiveInactive" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Selected="True" Value="1">Yes</asp:ListItem>
                                    <asp:ListItem Value="0">No</asp:ListItem>
                                </asp:RadioButtonList> 
                            </td>
                             <td></td>
                            <td>
                                 <span style="float:left">Notes:</span> <br />
                                <asp:TextBox ID="txtNotes" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" ToolTip="Type Integer Number" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                        
                    </table>
                </div>

                    

               <div class="button_area Rbutton_area">
                     <asp:Button ID="btnSave" ClientIDMode="Static" CssClass="Rbutton"  runat="server" Text="Save" OnClientClick="return validateInputs();" OnClick="btnSave_Click"    />
                     <asp:Button ID="btnClear" ClientIDMode="Static" CssClass="Rbutton"  runat="server" Text="Clear" OnClick="btnClear_Click"  />
                     <asp:Button ID="btnClose" ClientIDMode="Static" CssClass="Rbutton" PostBackUrl="~/hrd_default.aspx"  runat="server" Text="Close" />
                </div>

               <div class="show_division_info">

                <%--Share--%>
                
                     <%--<div id="ShiftConfig" class="datatables_wrapper" runat="server" style="width:100%; height:auto; max-height:500px;overflow:auto;overflow-x:hidden;"></div>--%>
                    <asp:GridView ID="gvShiftConfigurationList" runat="server"  Width="100%"  AutoGenerateColumns="false" DataKeyNames="sftId,Notes,DptId"   OnRowCommand="gvShiftConfigurationList_RowCommand" OnRowDeleting="gvShiftConfigurationList_RowDeleting" AllowPaging="True" PageSize="15" OnRowDataBound="gvShiftConfigurationList_RowDataBound" OnPageIndexChanging="gvShiftConfigurationList_PageIndexChanging">
                       <RowStyle HorizontalAlign="Center" Height="30px" />
                        <EditRowStyle Height="28px" />
                        <HeaderStyle BackColor="#0057AE" Height="28px" Font-Size="14px" ForeColor="White"  />
                        <PagerStyle  CssClass="gridview Sgridview" Height="40px" />
                         <Columns>
                            <asp:BoundField DataField="sftId"  HeaderText="ShiftId" Visible="false"  ItemStyle-Height="28px" >
                           
                             <ItemStyle Height="28px" />
                             </asp:BoundField>
                           
                            <asp:BoundField DataField="SftName" HeaderStyle-HorizontalAlign="Left" HeaderText="Shift" Visible="true"  ItemStyle-HorizontalAlign="Left"  >
                             <HeaderStyle HorizontalAlign="Left" />
                             <ItemStyle HorizontalAlign="Left" />
                             </asp:BoundField>
                            <asp:BoundField DataField="DptName" HeaderText="Department" Visible="true"  />
                            
                             <asp:BoundField DataField="StartTime12Fromat" HeaderText="Start Time" Visible="true"  />
                            <asp:BoundField DataField="EndTime12Fomat" HeaderText="Close Time" Visible="true"  />
                            <asp:BoundField DataField="PunchCountTime12Fomat" HeaderText="P Count Time" Visible="true"  />
                            <asp:BoundField DataField="SftAcceptableLate" HeaderText="A.Late" Visible="true"  />
                            <asp:BoundField DataField="OTStatus" HeaderText="OT" Visible="true" ItemStyle-Width="30px" >
                              <ItemStyle Width="30px" />
                             </asp:BoundField>
                              <asp:BoundField DataField="AcceptableTimeAsOT" HeaderText="OTM" Visible="true" ItemStyle-Width="30px" >
                             <ItemStyle Width="30px" />
                             </asp:BoundField>
                            <asp:BoundField DataField="ActiveStatus" HeaderText="Active" Visible="true"  />
                             
                              <asp:TemplateField >
                                  <HeaderTemplate>Initital</HeaderTemplate>
                                  <ItemTemplate>
                                      <asp:CheckBox ID="chkInitialShift" runat="server" Checked='<%#bool.Parse(Eval("IsInitial").ToString())%>' Enabled="false" />
                                  </ItemTemplate>
                              </asp:TemplateField>

                              <asp:BoundField DataField="BreakStartTime" HeaderText="B.Start Time" Visible="true"  />
                            <asp:BoundField DataField="BreakEndTime" HeaderText="B.End Time" Visible="true"/>

                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                     <asp:Button ID="lnkAlter" runat="server" ControlStyle-CssClass="btnForAlterInGV"  Text="Edit" CommandName="Alter" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Delete" >
                                <ItemTemplate>
                                      <asp:Button ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Delete" ControlStyle-CssClass="btnForDeleteInGV" OnClientClick="return confirm('Are you sure to delete?');" />
                                </ItemTemplate>

                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
         
              </div>

				
            </div>
        </div>
    </div>
                                </ContentTemplate>
                </asp:UpdatePanel>
    <script type="text/javascript">
     $('#btnNew').click(function () {
              clear();
          });
          function validateInputs() {
              if (validateText('txtShiftName', 1, 60, 'Enter ShiftName') == false) return false;
              if (validateText('txtEffectiveDate', 1, 60, 'Enter Effective Date') == false) return false;
              if (validateText('txtShiftName', 1, 60, 'Enter ShiftName') == false) return false;
              return true;
          }

          function clear() {
              if ($('#upSave').val() == '0') {

                  $('#btnSave').removeClass('css_btn');
                  $('#btnSave').attr('disabled', 'disabled');
              }
              else {
                  $('#btnSave').addClass('css_btn');
                  $('#btnSave').removeAttr('disabled');
              }

              $('#txtShiftName').val('');
              $('#txtEffectiveDate').val('');
              $('#txtStartTime').val('');
              $('#txtEndTime').val('');
              $('#txtAcceptableLate').val('');
              $('#txtDelayTimeOut').val('');
              $('#btnSave').val('Save');
              $('#hdnbtnStage').val("");
              $('#hdnUpdate').val("");
              $('#txtShiftName').focus();
              $('#btnDelete').removeClass('css_btn');
              $('#btnDelete').attr('disabled', 'disabled');
          }

    </script>
</asp:Content>
