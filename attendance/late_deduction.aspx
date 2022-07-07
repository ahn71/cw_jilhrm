<%@ Page Title="Late Deduction" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="late_deduction.aspx.cs" Inherits="SigmaERP.personnel.late_deduction" %>
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
        #ContentPlaceHolder1_MainContent_gvLateDeductionTypeList th {
            text-align:center;
        }
        #ContentPlaceHolder1_MainContent_gvLateDeductionTypeList th:first-child,td:first-child {
            text-align:left;
            padding-left:3px;
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
                    <li><a href="#" class="ds_negevation_inactive Mactive">Late Deduction</a></li>
                </ul>
            </div>
        </div>
    </div>
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
     <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>

     <div class="main_box Mbox">
         <div class="main_box_header MBoxheader">
            <h2>Late Deduction</h2>
        </div>
        <div class="main_box_body_leave">
            <div class="main_box_content_leave" id="divElementContainer" runat="server">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                       
                        <asp:AsyncPostBackTrigger ControlID="btnSave" />
                        <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="input_division_info_leave">
                            <table class="division_table_leave">
                               
                                <tbody>

                                     <tr id="trCompanyName" runat="server">
                                        <td>Company Name</td>
                                        <td>:</td>
                                        <td>
                                        <asp:DropDownList ID="ddlCompanyList" ClientIDMode="Static"   CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" >              
                                         </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td>Leave Id
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLeaveId" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False"></asp:TextBox>

                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td>Leave Name
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlLeaveTypes" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
                                            <asp:ListItem Value="s">Select</asp:ListItem>
                                            <asp:ListItem Value="c/l">Casual Leave</asp:ListItem>
                                            <asp:ListItem Value="s/l">Sick Leave</asp:ListItem>
                                            <asp:ListItem Value="a/l">Annual Leave</asp:ListItem>
                                            <asp:ListItem Value="m/l">Maternity Leave</asp:ListItem>
                                            <asp:ListItem Value="op/l">Official Purpose Leave</asp:ListItem>
                                            <asp:ListItem Value="o/l">Others Leave</asp:ListItem>
                                            </asp:DropDownList>
                                          
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Late Days
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="txtLateDays" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>

                                            

                                            <%--<asp:RequiredFieldValidator  Type="Integer" ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator2" runat="server"  ControlToValidate="txtLeaveDays" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            No. Of Deduction Days
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="txtNoOfDeductionDays" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align:top">Notes
                                        </td>
                                        <td style="vertical-align:top">:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="txtNotes" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" TextMode="MultiLine"></asp:TextBox>
                                           

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Deduction Allowed
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="IsDeductionAllowed" CssClass="form-control text_box_width" runat="server" Checked="true" Enabled="false" />


                                        </td>
                                    </tr>
                

                                </tbody>
                            </table>
                        </div>
                        <div class="punishment_button_area">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                <th><asp:Button ID="btnSave" ValidationGroup="save" CssClass="Mbutton" runat="server" Text="Save" OnClick="btnSave_Click"  /></th>
                                <th><asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="Mbutton" OnClick="btnClear_Click" /></th>
                                <th><asp:Button ID="Button3" runat="server" Text="Close" CssClass="Mbutton" PostBackUrl="~/attendance_default.aspx" OnClick="Page_Load" /></th>                                   
                         </tr>
                    </tbody>
                  </table>
                </div>
                      <%--  <div class="list_button" style="width:216px;padding-top: 10px;">
                            <table>
                                <tbody>
                                    <tr>                                        
                                        <td>
                                            <asp:Button ID="btnSave" ValidationGroup="save" CssClass="Mbutton" runat="server" Text="Save" OnClick="btnSave_Click"  />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="Mbutton" OnClick="btnClear_Click" />
                                        </td>
                                        <td>
                                            <asp:Button ID="Button3" runat="server" Text="Close" CssClass="Mbutton" PostBackUrl="~/attendance_default.aspx" OnClick="Page_Load" />
                                        </td>                                                                        
                                    </tr>
                                </tbody>
                            </table>
                        </div>--%>
                        <div >
                            

                            <asp:GridView ID="gvLateDeductionTypeList" runat="server" DataKeyNames="LDId,CompanyId,Notes,ShortName" AllowPaging="True" AutoGenerateColumns="False" Width="100%" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" OnRowCommand="gvLateDeductionTypeList_RowCommand" OnRowDeleting="gvLateDeductionTypeList_RowDeleting" OnRowDataBound="gvLateDeductionTypeList_RowDataBound" >
                                <RowStyle HorizontalAlign="Center" />
                                <Columns>
                                    

                                    <asp:BoundField DataField="LeaveName" HeaderText="Name"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                    <asp:BoundField DataField="LateDays" HeaderText="Late Days"  />
                                   
                                    <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkStatus" runat="server" Checked='<%#bool.Parse(Eval("IsDeduction").ToString())%>' CssClass="largerCheckbox" Enabled="false"  />

                                                    </ItemTemplate> 
                                                    <ItemStyle HorizontalAlign="Center" Width="45px" />
                                                </asp:TemplateField>
                                    <asp:BoundField DataField="NoDeductionDays" HeaderText="Deduction Days"   />
                                    
                                    <asp:BoundField DataField="Year" HeaderText="Year"   />

                                     <asp:TemplateField HeaderText="Edit" ItemStyle-Width="100px">
                                       <ItemTemplate  >
                                            <asp:Button ID="btnAlter" runat="server" ControlStyle-CssClass="btnForAlterInGV"  Text="Edit" CommandName="Alter" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                       </ItemTemplate>
                                   </asp:TemplateField>
                                    <%--<asp:ButtonField CommandName="Alter"   ControlStyle-CssClass="btnForAlterInGV"  HeaderText="Alter" ButtonType="Button" Text="Alter" ItemStyle-Width="80px"/>--%>
                                   
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="100px">
                                       <ItemTemplate  >
                                            <asp:Button ID="btnDelete" runat="server" ControlStyle-CssClass="btnForDeleteInGV"  Text="Delete" CommandName="Delete" CommandArgument='<%#((GridViewRow)Container).RowIndex%>'  OnClientClick="return confirm('Are you sure to delete ?')" />
                                       </ItemTemplate>
                                   </asp:TemplateField>
                                     
                                </Columns>
                                <HeaderStyle BackColor="#2b5e4e" Height="28px" />
                            </asp:GridView>
                            
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>

 

</asp:Content>
