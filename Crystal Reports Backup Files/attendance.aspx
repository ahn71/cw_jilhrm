<%@ Page Title="Manually Single Attendance" Language="C#" MasterPageFile="~/attendance_nested.Master" AutoEventWireup="true" CodeBehind="attendance.aspx.cs" Inherits="SigmaERP.attendance.attendance" %>
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
            $(document).ready(function () {                
                loadMessage();
            });
     </script>
   
    <style type="text/css">
    .modalBackground
    {
        background-color: Black;
        filter: alpha(opacity=90);
        opacity: 0.5;
    }
    .modalPopup
    {
        background-color: #FFFFFF;
        border-width: 1px;
        border-style: solid;
        border-color: black;
        padding-top: 10px;
        padding-left: 10px;
        width: 600px;
        height: 200px;
       
    }
    .TDLeftGrid /*to label the left TD*/
         { 
            VERTICAL-ALIGN: TOP; 
            TEXT-ALIGN: LEFT;
            text-indent:3px; 
            FONT-WEIGHT: NONE; 
            FONT-SIZE: 9pt; 
            COLOR: #7C201E; 
            FONT-FAMILY: Arial,Times New Roman,Verdana,sans-serif; 
            TEXT-DECORATION:NONE; 	   
            BACKGROUND:#D8DBDF;
         }  
   #lab label{
       padding:0 7px!important;
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
                    <li><a href="#" class="ds_negevation_inactive Mactive">Manually Single Attendance</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <asp:UpdatePanel ID="uplMessage" runat="server">
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
     <asp:HiddenField ID="hdAttendance" ClientIDMode="Static" runat="server" Value="" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="hdAttendance" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
            
            
            <asp:AsyncPostBackTrigger ControlID="rblAttendanceCountType" />
            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
        </Triggers>
        <ContentTemplate>
    <div class="main_box Mbox">
        <div class="main_box_header MBoxheader">
            <h2>Manually Single Attendance</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content">
                
       
    
                <div class="input_division_info">
                    
                 <center> 
                     
                     <div id="divFindInfo" runat="server" style="font-weight:bold"></div>
                     
                       <table >
                        <tr>
                                <td>Company<span class="requerd1">*</span></td>
                                <td>:</td>
                                <td colspan="3"> 
                                    <asp:DropDownList ID="ddlCompanyList" runat="server" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" Width="98%"  >
                                    </asp:DropDownList>
                                </td>
                               
                               
                            </tr> 
                 <tr runat="server" visible="false">
                                <td>Date Range<span class="requerd1">*</span></td>
                                <td>:</td>
                                <td colspan="2">
                                    <asp:RadioButtonList ID="rblAttendanceCountType" runat="server" RepeatDirection="Horizontal"  AutoPostBack="True" OnSelectedIndexChanged="rblAttendanceCountType_SelectedIndexChanged" >
                                     <asp:ListItem Selected="True" Value="Single">Single</asp:ListItem>
                                    
                                     </asp:RadioButtonList>
                                </td>
                                <td></td>
                                <td>
                                    
                                </td>
                                 <td>                           
                                </td>
                                
                            </tr>
                            <tr id="trFromDate" runat="server" >
                                <td id="tdFromDate" runat="server">From Date <span class="requerd1">*</span></td>
                                <td>: </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtFromDate" runat="server" autocomplete="off" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MM-yyyy" TargetControlID="txtFromDate">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr id="trToDate" runat="server" visible="false" >
                                <td>To Date<span class="requerd1">*</span></td>
                                <td>: </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MM-yyyy" TargetControlID="txtToDate">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                                 
                        <tr>
                            <td>Card No<span class="requerd1">*</span>
                            </td>
                            <td>:
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtEmpCardNo" ClientIDMode="Static"  runat="server" CssClass="form-control text_box_width" Width="95%" MaxLength="13" ></asp:TextBox> 
                             
                             </td>
                          <td>
                              <asp:Button ID="btnFindEmpInfo" runat="server" Text="Find " CssClass="Mbutton" Width="60px" Height="37px" OnClientClick="return InputValidation();"  OnClick="btnFindEmpInfo_Click"></asp:Button>
                          </td>

                        </tr>
                     
                     <tr>
                        <td></td>
                         <td></td>
                         
                     </tr>  
                                 
                        <caption>                            
                            <tr>
                                <td>Status <span class="requerd1">*</span></td>
                                <td>: </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlAttendanceTemplate" runat="server" ClientIDMode="Static" CssClass="form-control select_width">                                     
                                        <asp:ListItem Value="AC"> Activities</asp:ListItem>
                                        <asp:ListItem Value="WH">WHO</asp:ListItem>    
                                        <asp:ListItem Value="Lv">Leave</asp:ListItem>               
                                    </asp:DropDownList>
                                </td>
                                <td id="lab">
                              <asp:CheckBox runat="server" ID="ckbOutPunch"  Text="Out Duty" AutoPostBack="true" OnCheckedChanged="ckbOutPunch_CheckedChanged"/>
                                    <asp:CheckBox runat="server" ID="ckbTraining"  Text="Training" AutoPostBack="true" OnCheckedChanged="ckbTraining_CheckedChanged"/>
                          </td</tr>
                            <tr runat="server" id="trReferencId" visible="false">
                                <td>Reference ID <span class="requerd1">*</span></td>
                                <td>: </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtReferencId" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                </td>                              
                            </tr>                                                          
                            <tr>
                                <td>In Time<span class="requerd1">*</span> </td>
                                <td>:</td>
                                <td>                                    
                                     <asp:TextBox ID="txtInHur" runat="server"  ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00" style=" text-align: center; font-weight: bold;width:60px;float:left;"></asp:TextBox>
                                    </td>
                                <td> 
                                     <asp:TextBox ID="txtInMin" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00" style=" text-align: center; font-weight: bold;width:60px"></asp:TextBox>
                                    </td>
                                <td> 
                                     <asp:DropDownList ID="ddlInTimeAMPM" runat="server" CssClass="attend_select_min" Width="67px" style="float:left;">
                                      <asp:ListItem Value="AM">AM</asp:ListItem>
                                      <asp:ListItem Value="PM">PM</asp:ListItem>
                                    </asp:DropDownList>                          
                                </td>                        
                                
                            </tr>
                            <tr id="trStartLunch" runat="server"  visible="false">
                                <td>Time Out for Lunch </td>
                                <td>: </td>
                                <td style="width: 40px;">
                                    <asp:TextBox ID="txtOutForLunchHr" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtOutForLunchHr" ErrorMessage="*" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align: center">&nbsp;: </td>
                                <td>
                                    <asp:TextBox ID="txtOutForLunchMin" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtOutForLunchMin" ErrorMessage="*" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr tr="trEndLunch" runat="server"  visible="false">
                                <td>Time In after Lunch </td>
                                <td>: </td>
                                <td style="width: 40px;">
                                    <asp:TextBox ID="txtInAfterLunchHr" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtInAfterLunchHr" ErrorMessage="*" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align: center">&nbsp;: </td>
                                <td>
                                    <asp:TextBox ID="txtInAfterLunchMin" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtInAfterLunchMin" ErrorMessage="*" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr >
                               <td>Out Time <span class="requerd1">*</span></td>
                                <td>: </td>
                                <td>
                                    <asp:TextBox ID="txtOutHur" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00" style=" text-align: center; font-weight: bold; width:60px"></asp:TextBox>
                                    </td>
                                <td> 
                                    <asp:TextBox ID="txtOutMin" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00" style=" text-align: center; font-weight: bold; width:60px"></asp:TextBox>
                                    </td>
                                <td> 
                                    <asp:DropDownList ID="ddlOutTimeAMPM" runat="server" CssClass="attend_select_min" Width="67px">
                                        <asp:ListItem Value="AM" Selected="True">AM</asp:ListItem>    
                                        <asp:ListItem Value="PM" >PM</asp:ListItem>                                                                            
                                    </asp:DropDownList>
                                </td>
                            </tr>
                                   <tr  >
                                <td>Remark</td>
                                <td>: </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" ClientIDMode="Static" CssClass="form-control text_box_width" Height="35px"></asp:TextBox>
                                  
                                </td>
                            </tr>
                        </caption>
                    </table></center>
                </div>
                <div class="button_area" style="margin-top:10px;">
                    <table class="button_table">
                        <tr>
                            <th>
                                
                                <asp:Button ID="btnShow" CssClass="Mbutton" runat="server" Text="List All" PostBackUrl="~/attendance/attendance_list.aspx" />

                            </th>
                            <th>
                                <asp:Button ID="btnSave" CssClass="Mbutton" runat="server" Text="Save" OnClientClick="return InputValidationBasket();"  OnClick="btnSave_Click"/>
                            </th>
                            <th>
                                <asp:Button ID="btnClear" CssClass="Mbutton" runat="server" Text="Clear" OnClick="btnClear_Click" />
                           <th> 
                               <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/attendance_default.aspx" CssClass="Mbutton" />
                           </th>
                                <th>
                                    <asp:Button Visible="false" runat="server" ID="btnComplain" Text="Complain" CssClass="css_btn" OnClick="btnComplain_Click" />
                                <%--<asp:Button ID="btnComplain" runat="server" Text="Complain" CssClass="css_btn" >--%>
                            </th>
                        
                        </tr>
                    </table>
                </div>
                 <div class="dataTables_wrapper">
                    <asp:GridView ID="gvAttendance" runat="server" AllowPaging="True" style="font-size:13px" AutoGenerateColumns="False" DataKeyNames="EmpCardNo" CellPadding="4" ForeColor="#333333" Height="13px"  PageSize="1500" Width="100%" OnPageIndexChanging="gvAttendance_PageIndexChanging" OnRowDataBound="gvAttendance_RowDataBound">
                        <PagerStyle CssClass="gridview" Height="20px" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>

                             <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hideSubId" runat="server" 
                                            Value='<%# DataBinder.Eval(Container.DataItem, "EmpCardNo")%>' />
                                        <%# Container.DataItemIndex+1%>
                                    </ItemTemplate>

                                    <ItemStyle HorizontalAlign="Center" ForeColor="green" />
                                </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Card No">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("EmpCardNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("EmpName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Department">
                                <ItemTemplate>
                                    <asp:Label ID="lblDptName" runat="server" Text='<%# Eval("DptName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Designation">
                                <ItemTemplate>
                                    <asp:Label ID="lblDsgName" runat="server" Text='<%# Eval("DsgName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblATTDate" runat="server" Text='<%# Eval("AttDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="In Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblIntime" ForeColor="Green" Font-Bold="true" runat="server" Text='<%# Eval("Intime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Out Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label Font-Bold="true" ID="lblOuttime" ForeColor="Red" runat="server" Text='<%# Eval("Outtime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblATTStatus" runat="server" Text='<%# Eval("ATTStatus") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                          
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#27235C" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F8FAFA" />
                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                        <SortedDescendingCellStyle BackColor="#D4DFE1" />
                        <SortedDescendingHeaderStyle BackColor="#15524A" />
                    </asp:GridView>
                    <ul runat="server" id="ulAttMissingLog" visible="false" >
                        <li><a target="_blank" href="attendance_missing_log.aspx">attendance missing log</a></li>
                    </ul>
                </div>
            </div>
             

        </div>
    </div>

                    </ContentTemplate>
    </asp:UpdatePanel>   
    <script type="text/javascript">
        function InputValidation()
        {
            alter('Hello!');
            if ($('#txtEmpCardNo').val().trim().length <5) {
                showMessage('Please type valid card no', 'error');
                $('#txtEmpCardNo').focus(); return false;
            }
            if ($('#txtFromDate').val().trim().length <1) {
                showMessage('Please type valid Date', 'error');
                $('#txtFromDate').focus(); return false;
            }
            return true;
        }

        function InputValidationBasket() {
            try {
                alert();
                if ($('#ddlDivisionName').val() == "s") {
                    showMessage('Please select division', 'error');
                    $('#ddlDivisionName').focus(); return false;
                }

                if ($('#ddlShiftName').val()=="s") {
                    showMessage('Please select shift', 'error');
                    $('#ddlShiftName').focus(); return false;
                }

                if ($('#txtEmpCardNo').val().trim().length <11) {
                    showMessage('Please type the emp cardno', 'error');
                    $('#txtEmpCardNo').focus(); return false;
                }
                
                if ($('#ddlAttendanceTemplate').val()=="s") {
                    showMessage('Please select attendance status', 'error');
                    $('#txtInDate').focus(); return false;
                }
  
                if ($('#rblAttendanceCountType input:checked').val() == "Single") {

                    if ($('#txtFromDate').val().trim().length < 10) {
                        showMessage('Please select date', 'error');
                        $('#txtFromDate').focus(); return false;
                    }
                }

                else {
                    if ($('#txtFromDate').val().trim().length < 10) {
                        showMessage('Please select from date', 'error');
                        $('#txtFromDate').focus(); return false;
                    }
                    if ($('#txtToDate').val().trim().length < 10) {
                        showMessage('Please select to date', 'error');
                        $('#txtToDate').focus(); return false;
                    }
                }
                alert(parseInt($('#txtInHur').val().trim());
                if (parseInt($('#txtInHur').val().trim()) == 0) {
                    showMessage('Please type valid start hour', 'error');
                    $('#txtInHur').focus(); return false;
                }

                return true;
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }

        function InputValidationBasket2()
        {
            try
            {
                alert();
                if ($('#txtEmpCardNo').val().trim().length<4)
                {
                    alert("01");
                    showMessage('Please type valid card no.', 'error');
                    $('#txtEmpCardNo').focus(); return false;
                }
                return true;
            }
            catch(exception)
            {
            
            }

        }
    </script>
</asp:Content>
