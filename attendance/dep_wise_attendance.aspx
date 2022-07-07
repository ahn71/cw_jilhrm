<%@ Page Title="Department Wise Attendance" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="dep_wise_attendance.aspx.cs" Inherits="SigmaERP.attendance.dep_wise_attendance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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
        .auto-style1 {
            width: 426px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
            <asp:AsyncPostBackTrigger ControlID="ddlDivision" />
            <asp:AsyncPostBackTrigger ControlID="rbAttendanceList" />
        </Triggers>
        <ContentTemplate>
    <div class="main_box">
        <div class="main_box_header">
            <h2>Manualy Department wise Attendance</h2>
        </div>
        <div class="main_box_body">
            <div class="main_box_content">


                <div class="attendance_option">
                     
                    <fieldset>
                        <legend>Option</legend>
                        <table>
                            <tr>
                                <td class="auto-style1">
                                    

                                     <asp:RadioButtonList ID="rbEmpType" runat="server" RepeatDirection="Horizontal">
                                     </asp:RadioButtonList>
                                     

                                   

                                     <asp:RadioButtonList ID="rbAttendanceList" runat="server" RepeatDirection="Horizontal" style="padding-left: 151px; float:right; margin-top: -19px;" AutoPostBack="True" OnSelectedIndexChanged="rbAttendanceList_SelectedIndexChanged" >
                            <asp:ListItem Selected="True" Value="0">Current Date</asp:ListItem>
                            <asp:ListItem Value="1">Multiple</asp:ListItem>
                        </asp:RadioButtonList>
                                </td>
                                
                            </tr>
                        </table>
                       

                    </fieldset>
                </div>
                <div class="input_division_info">
                    
                    <table class="division_table">
                      
                       <tr>
                                <td>Division</td>
                                <td>: </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlDivision" runat="server" ClientIDMode="Static" CssClass="form-control select_width" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                       
                                    </asp:DropDownList>
                                </td>
                            </tr>
                          
                       <tr>
                                <td>Department</td>
                                <td>: </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlDepartment" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
                                       
                                    </asp:DropDownList>
                                </td>
                            </tr>
                       <%-- <tr>
                            <td>Month Id
                            </td>
                            <td>:
                            </td>
                            <td colspan="3">

                                <asp:DropDownList ID="ddlMonthID" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
                                    
                                </asp:DropDownList>
                                                   <asp:RequiredFieldValidator InitialValue="" ID="RequiredFieldValidator13" Display="Dynamic" 
    ValidationGroup="save" runat="server" ControlToValidate="ddlMonthID"
    Text="*" ErrorMessage="ErrorMessage"></asp:RequiredFieldValidator>
                            </td>
                        </tr>--%>
                        <caption>
                            .
                            <tr>
                                <td>Atten. Status </td>
                                <td>: </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlAttenStatus" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
                                        <asp:ListItem>-Select-</asp:ListItem>
                                        <asp:ListItem>Present</asp:ListItem>
                                        <asp:ListItem>Absent</asp:ListItem>
                                        <asp:ListItem>Late</asp:ListItem>
                                        <asp:ListItem>Casula Leave</asp:ListItem>
                                        <asp:ListItem>Sick Leave</asp:ListItem>
                                        <asp:ListItem>Maternity Leave</asp:ListItem>
                                        <asp:ListItem>Weekend</asp:ListItem>
                                        <asp:ListItem>Holiday</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>From Date </td>
                                <td>: </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtFromDate" runat="server" autocomplete="off" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MM-yyyy" TargetControlID="txtFromDate">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>To Date</td>
                                <td>: </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MM-yyyy" TargetControlID="txtToDate">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>Time In </td>
                                <td>: </td>
                                <td style="width: 40px;">
                                    <asp:TextBox ID="txtInTimeHr" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtInTimeHr" ErrorMessage="*" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align: center">&nbsp;: </td>
                                <td>
                                    <asp:TextBox ID="txtInTimeMin" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtInTimeMin" ErrorMessage="*" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Time Out for Lunch </td>
                                <td>: </td>
                                <td style="width: 40px;">
                                    <asp:TextBox ID="txtOutForLunchHr" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtOutForLunchHr" ErrorMessage="*" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align: center">&nbsp;: </td>
                                <td>
                                    <asp:TextBox ID="txtOutForLunchMin" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtOutForLunchMin" ErrorMessage="*" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Time In after Lunch </td>
                                <td>: </td>
                                <td style="width: 40px;">
                                    <asp:TextBox ID="txtInAfterLunchHr" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtInAfterLunchHr" ErrorMessage="*" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align: center">&nbsp;: </td>
                                <td>
                                    <asp:TextBox ID="txtInAfterLunchMin" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtInAfterLunchMin" ErrorMessage="*" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Time Out </td>
                                <td>: </td>
                                <td style="width: 40px;">
                                    <asp:TextBox ID="txtTimeOutHr" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtTimeOutHr" ErrorMessage="*" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align: center">&nbsp;: </td>
                                <td>
                                    <asp:TextBox ID="txtTimeOutMin" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="0"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtTimeOutMin" ErrorMessage="*" ForeColor="Red" ValidationGroup="save"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </caption>
                    </table>
                </div>
                <div class="button_area">
                    <table class="button_table">
                        <tr>
                            
                            <th>
                                <asp:Button ID="btnShow" CssClass="css_btn" runat="server" Text="List All" PostBackUrl="~/attendance/attendance_list.aspx" /></th>
                           <th> <asp:Button ID="btnSave" CssClass="css_btn" runat="server" Text="Save" OnClientClick="return InputValidationBasket() ;" ClientIDMode="Static" OnClick="btnSave_Click"/></th>
                            <th>
                               <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                                </th>
                            <th>
                                 <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold;float:left; margin-top:0px"><p style="margin-top: 0px;">Wait attendance&nbsp; processing</p> </span> 
                                        <img style="width:26px;height:26px;cursor:pointer; float:left" src="/images/wait.gif"  />  
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </th>
                        </tr>
                    </table>
                </div>

                <div class="dataTables_wrapper">
                            <asp:GridView ID="gvAttendance" runat="server" AutoGenerateColumns="False"
                                CellPadding="4" Height="13px" AllowPaging="True" ForeColor="#333333" Width="100%"
                                PageSize="6">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Card No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmpCardNo" runat="server" Text='<%# Eval("EmpCardNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAttenStatus" runat="server" Text='<%# Eval("AttenStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="In Hr">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTimeInHr" runat="server" Text='<%# Eval("TimeInHr") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="InMin">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTimeInMin" runat="server" Text='<%# Eval("TimeInMin") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Out Hr">
                                        <ItemTemplate>
                                            <asp:Label ID="TimeOutHr" runat="server" Text='<%# Eval("TimeOutHr") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="Out Min">
                                        <ItemTemplate>
                                            <asp:Label ID="TimeOutMin" runat="server" Text='<%# Eval("TimeOutMin") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" CommandArgument='<%# Eval("AttId") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>


                                            <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
                                                OnClientClick="return confirm('Are you sure, you want to delete the record?')"
                                                CommandArgument='<%# Eval("AttId") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                </Columns>
                                <EditRowStyle BackColor="#7C6F57" />
                                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                                <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#E3EAEB" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                <SortedDescendingHeaderStyle BackColor="#15524A" />
                            </asp:GridView>
                            <%--<div runat="server" id="divPunismentList" style="width: 500px; height: 599px;"></div>--%>
                        </div>



            </div>
             

        </div>
    </div>

                    </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function InputValidationBasket() {
            try {
                if ($('#txtEmpCardNo').val().trim().length == 0) {
                    showMessage('Please type the emp cardno', 'error');
                    $('#txtEmpCardNo').focus(); return false;
                }
                if ($('#ddlAttenStatus').val() == "-Select-") {
                    showMessage('Please select a valid attendance status', 'error');
                    $('#txtEmpCardNo').focus(); return false;
                }
                if ($('#txtInDate').val().trim().length == 0) {
                    showMessage('Please select att date', 'error');
                    $('#txtInDate').focus(); return false;
                }
                return true;
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }
    </script>
</asp:Content>
