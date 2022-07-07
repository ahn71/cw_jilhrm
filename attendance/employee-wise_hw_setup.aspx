<%@ Page Title="Employee Wise W/H Setup" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="employee-wise_hw_setup.aspx.cs" Inherits="SigmaERP.attendance.employee_wise_hw_setup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .gv th {
            text-align:center;
        }
         .gv th:nth-child(2),td:nth-child(2) {
             padding-left:3px;
            text-align:left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li>/</li>
                    <li><a href="/attendance_default.aspx">Attendance</a></li>
                    <li>/</li>
                    <li><a href="#" class="ds_negevation_inactive Mactive">Employee Wise Holyday and Weekend Setup</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">

        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />

            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
        </Triggers>
        <ContentTemplate>

            <div class="main_box Mbox">
                <div class="main_box_header MBoxheader">
                    <h2>Employee Wise Holyday and Weekend Setup</h2>
                </div>
                <div class="employee_box_body">
                    <div class="employee_box_content">
                        <div class="input_division_info">
                            <table class="division_table">
                                <tr runat="server">
                                    <td>Company Name</td>
                                    <td>:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlCompanyList" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Card No
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCardNo" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="77%"></asp:TextBox> 
                                        <asp:Button Text="Find" ID="btnFind" runat="server" CssClass="Mbutton" style="float:right; margin-top:-41px"  OnClick="btnFind_Click" />                                       
                                        <asp:Label ID="lblEmpName" runat="server" Text="Name"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Day
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlWeekend" ClientIDMode="Static" CssClass="form-control select_width"  runat="server">
                                             <asp:ListItem>Friday</asp:ListItem>
                                              <asp:ListItem>Saturday</asp:ListItem>
                                              <asp:ListItem>Sunday</asp:ListItem>
                                             <asp:ListItem>Monday</asp:ListItem>
                                              <asp:ListItem>Tuesday</asp:ListItem>
                                             <asp:ListItem>Wednesday</asp:ListItem>
                                              <asp:ListItem>Thursday</asp:ListItem>
                                         </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>From Date
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                        <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                            PopupButtonID="imgDate" Enabled="True"
                                            TargetControlID="txtFromDate" ID="CExtApplicationDate">
                                        </asp:CalendarExtender>
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValida" runat="server"
                                        ControlToValidate="txtDate" ValidationExpression="^(([1-9])|(0[1-9])|(1[0-2]))\/((0[1-9])|([1-31]))\/((19|20)\d\d)$" Display="Dynamic" ValidationGroup="save" SetFocusOnError="true" ErrorMessage="invalid date">*</asp:RegularExpressionValidator>--%>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>To Date
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                        <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                            PopupButtonID="imgDate" Enabled="True"
                                            TargetControlID="txtToDate" ID="CalendarExtender1">
                                        </asp:CalendarExtender>



                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValida" runat="server"
                                        ControlToValidate="txtDate" ValidationExpression="^(([1-9])|(0[1-9])|(1[0-2]))\/((0[1-9])|([1-31]))\/((19|20)\d\d)$" Display="Dynamic" ValidationGroup="save" SetFocusOnError="true" ErrorMessage="invalid date">*</asp:RegularExpressionValidator>--%>
                                    
                                    </td>
                                </tr>

                                <tr runat="server">
                                    <td>Type</td>
                                    <td>:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlType" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="false">

                                            <asp:ListItem Text="Weekend" Value="Weekend"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td>Discription
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDescription" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                        
                                    </td>
                                </tr>


                            </table>
                        </div>

                        <div class="button_area" style="">
                            <table>

                                <tr>
                                    <td>
                                        <asp:Button ID="btnSave" CssClass="Mbutton" ValidationGroup="save" runat="server" Text="Save" OnClick="btnSave_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="Mbutton" OnClick="btnClear_Click" />
                                    </td>
                                    <td>
                                        <asp:Button ID="Button3" runat="server" PostBackUrl="~/attendance_default.aspx" Text="Close" CssClass="Mbutton" />
                                    </td>
                                </tr>

                            </table>
                        </div>

                        <div class="show_division_info">
                            <%--<div id="ShiftConfig" class="datatables_wrapper" runat="server" style="width:100%; height:auto; max-height:500px;overflow:auto;overflow-x:hidden;"></div>--%>
                            <asp:GridView ID="gvHoliday" runat="server" Width="100%" CssClass="gv" AutoGenerateColumns="false" DataKeyNames="HWCode" HeaderStyle-BackColor="#2b5e4e" HeaderStyle-Height="28px" HeaderStyle-ForeColor="white" HeaderStyle-Font-Size="14px" HeaderStyle-Font-Bold="true" AllowPaging="True" PageSize="20" OnRowDataBound="gvHoliday_RowDataBound" OnRowCommand="gvHoliday_RowCommand" OnRowDeleting="gvHoliday_RowDeleting" OnPageIndexChanging="gvHoliday_PageIndexChanging">
                                <RowStyle HorizontalAlign="Center" />
                                <PagerStyle CssClass="gridview" />
                                <Columns>

                                    <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" ItemStyle-Height="28px" />

                                    <asp:BoundField DataField="EmpName" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />

                                    <asp:BoundField DataField="DptName" HeaderText="Department" ItemStyle-Height="28px" />

                                    <asp:BoundField DataField="FromDate" HeaderText="From Date" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ToDate" HeaderText="To Date" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="HWType" HeaderText="Type" ItemStyle-Height="28px" />
                                    <asp:BoundField DataField="HWDayName" HeaderText="Day Name" ItemStyle-Height="28px" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" ItemStyle-Height="28px" />

                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkAlter" runat="server" CommandName="Alter" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Edit" Font-Bold="true" ForeColor="Green"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Delete" Font-Bold="true" ForeColor="Red" OnClientClick="return confirm('Are you sure to delete?');"></asp:LinkButton>
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
 

    </script>
</asp:Content>
