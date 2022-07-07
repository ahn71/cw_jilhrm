<%@ Page Title="Month Setup" Language="C#" MasterPageFile="~/attendance_nested.Master" AutoEventWireup="true" CodeBehind="monthly_setup.aspx.cs" Inherits="SigmaERP.attendance.monthly_setup" %>

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
    <style>
        #ContentPlaceHolder1_MainContent_gvMonthSetup th {
            text-align:center;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row Rrow">
        <div class="col-md-12 ds_nagevation_bar">
            <div style="margin-top: 5px">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li>/</li>
                    <li><a href="/attendance_default.aspx">Attendance</a></li>
                    <li>/</li>
                    <li><a href="#" class="ds_negevation_inactive Mactive">Month Setup</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
           
            <asp:AsyncPostBackTrigger ControlID="hdMonthSetup" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
            <asp:AsyncPostBackTrigger ControlID="gvMonthSetup" />
            <asp:AsyncPostBackTrigger ControlID="btnClear" />
            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
        </Triggers>
        <ContentTemplate>


            <div class="main_box Mbox">
                <div class="main_box_header MBoxheader">
                    <h2>Month Setup</h2>
                </div>
                <div class="main_box_body Mbody">
                    <div class="main_box_content">
                        <div class="input_division_info">
                            <table class="input_division_info">
                                <asp:HiddenField ID="hdMonthSetup" ClientIDMode="Static" runat="server" Value="" />

                                <tr>
                                    <td>
                                        Company<span class="requerd1">*</span>
                                    </td>
                                    <td>:</td>
                                    <td>
                                <asp:DropDownList ID="ddlCompanyList" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" >
                               </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Month ID(mm-yyyy)<span class="requerd1">*</span>
                                    </td>
                                    <td>:
                                    </td>

                                    <td>

                                        <asp:TextBox ID="txtMonthName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                    
                                        <asp:CalendarExtender ID="txtMonthName_CalendarExtender" Format="MM-yyyy" runat="server" TargetControlID="txtMonthName">
                                        </asp:CalendarExtender>
                                    
                                       <%-- <asp:RequiredFieldValidator ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMonthID" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>From Date<span class="requerd1">*</span>
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>

                                        <asp:CalendarExtender ID="txtFromDate_CalendarExtender" Format="dd-MM-yyyy" runat="server" TargetControlID="txtFromDate">
                                        </asp:CalendarExtender>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>To Date<span class="requerd1">*</span>
                                    </td>
                                    <td>:
                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                       
                                        <asp:CalendarExtender ID="txtToDate_CalendarExtender" Format="dd-MM-yyyy" runat="server" TargetControlID="txtToDate">
                                        </asp:CalendarExtender>
                                       
                                        <asp:Button ID="btnCalculation" runat="server" Text="Calculation" OnClick="btnCalculation_Click" />
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td>Total No of Days
                                    </td>
                                    <td>:
                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtTotalNOofDay" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>Total Weekend
                                    </td>
                                    <td>:
                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtTotalWeekend" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                        
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>Total Holiday
                                    </td>
                                    <td>:
                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtTotalHoliday" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>Total Working Days
                                    </td>
                                    <td>:
                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtTotalWorkingDays" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>Expected Payment Date<span class="requerd1">*</span>
                                    </td>
                                    <td>:
                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtExpectedPaymetnDate" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                       
                                        <asp:CalendarExtender ID="txtExpectedPaymetnDate_CalendarExtender" runat="server" TargetControlID="txtExpectedPaymetnDate" Format="dd-MM-yyyy">
                                        </asp:CalendarExtender>
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td>Month Status<span class="requerd1">*</span>
                                    </td>
                                    <td>:
                                    </td>
                                    <td>

                                        <%--<asp:TextBox ID="txtMonthStaus" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>--%>

                                        <asp:DropDownList ID="ddlMonthStatus" CssClass="form-control text_box_width" runat="server" Width="200px" >
                                            <asp:ListItem Value="0" Text=" "></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Inactive"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlMonthStatus" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="button_area Rbutton_area" style="text-align:center">                           
                            <asp:Button ID="btnShow" CssClass="Mbutton" runat="server" Text="List All" Visible="False" />
                            <asp:Button ID="btnSave" CssClass="Mbutton" ValidationGroup="save" runat="server" Text="Save" OnClick="btnSave_Click" />
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="Mbutton" OnClick="btnClear_Click" />
                            <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/attendance_default.aspx" CssClass="Mbutton" />
                            <asp:Button ID="btnPopup" runat="server" Text="Close"  style="display:none" CssClass="Mbutton" />                          
                        </div>

                        <div class="dataTables_wrapper monthly_table_setup show_division_info">
                            <asp:GridView ID="gvMonthSetup" runat="server" AutoGenerateColumns="False" DataKeyNames="MonthId" HeaderStyle-BackColor="#0057AE"
                                CellPadding="4" AllowPaging="True" ForeColor="#333333" Width="100%"
                                PageSize="12" OnRowCommand="gvMonthSetup_RowCommand" OnRowDeleting="gvMonthSetup_RowDeleting" OnRowEditing="gvMonthSetup_RowEditing" OnPageIndexChanging="gvMonthSetup_PageIndexChanging" OnRowDataBound="gvMonthSetup_RowDataBound">
                                <AlternatingRowStyle BackColor="White" />
                                <PagerStyle CssClass="gridview" />
                                <RowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Month">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMonthName" runat="server" Text='<%# Eval("MonthName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From">
                                        <ItemTemplate>
                                            <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("FromDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To">
                                        <ItemTemplate>
                                            <asp:Label ID="lblToDate" runat="server" Text='<%# Eval("ToDate") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Total">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalDays" runat="server" Text='<%# Eval("TotalDays") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Holiday">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalHoliday" runat="server" Text='<%# Eval("TotalHoliday") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Working">
                                        <ItemTemplate >
                                            <asp:Label ID="lblTotalWorkingDays" runat="server" Text='<%# Eval("TotalWorkingDays") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    

                                    <asp:TemplateField HeaderText="Edit">
                                        <ItemTemplate>
                                            <asp:Button ID="lnkEdit" runat="server" Font-Bold="true" Text="Edit" CommandName="Edit"   ControlStyle-CssClass="btnForAlterInGV"  CommandArgument='<%#((GridViewRow)Container).RowIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:Button ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete" ControlStyle-CssClass="btnForDeleteInGV"  
                                                OnClientClick="return confirm('Are you sure, you want to delete the record?')"
                                                CommandArgument='<%# Eval("MonthID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                </Columns>
                          
                                <%--  <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />--%>
                                <HeaderStyle BackColor="#2b5e4e" Height="28px" Font-Bold="True" ForeColor="White" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                 <%-- <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#E3EAEB" />
                               
                             <%--   <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                <SortedDescendingHeaderStyle BackColor="#15524A" /--%>
                            </asp:GridView>
                            <%--<div runat="server" id="divPunismentList" style="width: 500px; height: 599px;"></div>--%>
                        </div>


                    </div>
                </div>
            </div>
            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" Drag="True" 
                            DropShadow="True" PopupControlID="PopupWindow" TargetControlID="btnPopup" CancelControlID="btnCancel" PopupDragHandleControlID="divDrag" CacheDynamicResults="False" Enabled="True" >
                        </asp:ModalPopupExtender>

                        <div style="border-radius: 5px;  border: 2px solid #086A99;border-top:0px; font-weight:bold; width: 380px;background:#ddd;padding:5px;" id="PopupWindow" >
                            <div id="divDrag" class="boxFotter">
                                 <a ID="btnCancel" href="#"><img class="popup_close" src="../images/icon/cancel.png" alt="" style=" display:none" /></a>
                           <cnter> 
                                <h2 style="margin-top: -3px;">Weekend Date</h2>
                           </cnter>
                                
                             </div>

                            <asp:Panel ID="Panel1" runat="server" BackColor="WhiteSmoke">

                             <asp:GridView runat="server" ID="gvWeekendDate" AutoGenerateColumns="false"  HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" Width="220px">
                                 <Columns>
                                     <asp:BoundField HeaderText="Date" DataField="WDate" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" ItemStyle-Width="100px" />
                                     <asp:TemplateField AccessibleHeaderText="Choose" HeaderText="Chosen" ItemStyle-Width="150px"  ItemStyle-HorizontalAlign="center">
                                         <ItemTemplate  >
                                             <asp:CheckBox ID="SelectCheckBox" runat="server" ItemStyle-Width="60px" Checked="true" />
                                           

                                         </ItemTemplate>
                                     </asp:TemplateField>
                                 </Columns>
                             </asp:GridView><br />
                                <asp:Button ID="btnSubmit" Width="80px" Text="OK" runat="server" OnClick="btnSubmit_Click" /><br />
                           </asp:Panel>
                          

                          
                           
                        </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
