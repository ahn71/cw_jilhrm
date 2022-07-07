<%@ Page Title="Leave Configuration" Language="C#" MasterPageFile="~/leave_nested.Master" AutoEventWireup="true" CodeBehind="LeaveConfig.aspx.cs" Inherits="SigmaERP.personnel.LeaveConfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #ContentPlaceHolder1_MainContent_gvLeaveConfig td, th {
            text-align: center;
        }

            #ContentPlaceHolder1_MainContent_gvLeaveConfig td:nth-child(4), th:nth-child(4) {
                text-align: left;
                padding-left: 3px;
            }

        #ContentPlaceHolder1_MainContent_gvLeaveConfig th:first-child {
            text-align: left;
            padding-left: 3px;
        }

        #ContentPlaceHolder1_MainContent_gvLeaveConfig td:first-child {
            text-align: left;
            padding-left: 3px;
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
                       <li> <a href="/leave_default.aspx">Leave</a></li>
                       <li>/</li>
                       <li> <a href="#" class="ds_negevation_inactive Lactive">Leave Configuration</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
     <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>

    <div class="main_box Lbox">
        <div class="main_box_header_leave LBoxheader">
            <h2>Leave Configuration Panel</h2>
        </div>
        <div class="main_box_body_leave Lbody">
            <div class="main_box_content_leave" id="divElementContainer" runat="server">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                       <asp:AsyncPostBackTrigger ControlID="hdLeaveId" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" />
                        <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="input_division_info">
                            <table class="employee_table">
                                <asp:HiddenField ID="hdLeaveId" ClientIDMode="Static" runat="server" Value="" />
                                <tbody>

                                     <tr id="trCompanyName" runat="server" >
                                        <td>Company Name<span class="requerd1">*</span></td>
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
                                        <td>Leave Name<span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlLeaveTypes" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
                                            <%--<asp:ListItem Value="s">Select</asp:ListItem>
                                            <asp:ListItem Value="c/l">Casual Leave</asp:ListItem>
                                            <asp:ListItem Value="s/l">Sick Leave</asp:ListItem>
                                            <asp:ListItem Value="a/l">Annual Leave</asp:ListItem>
                                            <asp:ListItem Value="m/l">Maternity Leave</asp:ListItem>
                                            <asp:ListItem Value="op/l">Official Purpose Leave</asp:ListItem>
                                            <asp:ListItem Value="o/l">Others Leave</asp:ListItem>
                                            <asp:ListItem Value="sr/l">Short Leave</asp:ListItem>--%>
                                            </asp:DropDownList>                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Leave Days<span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="txtLeaveDays" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"  ></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtLeaveDays"
                                                ErrorMessage="Please Enter Only Numbers" ValidationExpression="^\d+$" ValidationGroup="save"></asp:RegularExpressionValidator>
                                            <%--<asp:RequiredFieldValidator  Type="Integer" ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator2" runat="server"  ControlToValidate="txtLeaveDays" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Leave Nature<span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="txtLeaveNature" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                            <asp:RequiredFieldValidator ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtLeaveNature" ErrorMessage="*"></asp:RequiredFieldValidator>

                                        </td>
                                    </tr>
                                    <tr visible="false" runat="server">
                                        <td>Deduction Allowed
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="IsDeductionAllowed" CssClass="form-control text_box_width" runat="server" />


                                        </td>
                                    </tr>
                

                                </tbody>
                            </table>
                        </div>
                        <div class="button_area Rbutton_area">                          
                            <asp:Button ID="btnShow" CssClass="Lbutton" runat="server" Text="List All" Visible="False" />
                            <asp:Button ID="btnSave" ValidationGroup="save" CssClass="Lbutton" runat="server" Text="Save" OnClick="btnSave_Click" />                                  
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="Lbutton" OnClick="btnClear_Click" />                                   
                            <asp:Button ID="Button3" runat="server" Text="Close" CssClass="Lbutton" PostBackUrl="~/leave_default.aspx" OnClick="Page_Load" />                                     
                            <asp:Button runat="server" ID="btnPreview" Text="Preview" CssClass="Lbutton" OnClick="btnPreview_Click"  />                    
                        </div>
                        <div >
                         
                               
                           <div class="show_division_info">
                            <asp:GridView ID="gvLeaveConfig" runat="server" DataKeyNames="LeaveId,CompanyId" AllowPaging="True" AutoGenerateColumns="False" Width="100%"  HeaderStyle-ForeColor="White" OnRowCommand="gvLeaveConfig_RowCommand" OnRowDeleting="gvLeaveConfig_RowDeleting" OnRowDataBound="gvLeaveConfig_RowDataBound">
                                
                                <Columns>
                                    <asp:BoundField DataField="LeaveId" HeaderText="LeaveId" Visible="false" />

                                    <asp:BoundField DataField="LeaveName" HeaderText="Name"  />
                                    <asp:BoundField DataField="ShortName" HeaderText="Short"  />
                                    <asp:BoundField DataField="LeaveDays" HeaderText="Days"  />
                                    <asp:BoundField DataField="LeaveNature" HeaderText="Nature" />
                                    <asp:BoundField DataField="IsDeductionAllowed" HeaderText="Deduction" Visible="false"/>

                                     <asp:TemplateField HeaderText="Edit" ItemStyle-Width="100px">
                                       <ItemTemplate  >
                                            <asp:Button ID="btnAlter" runat="server" ControlStyle-CssClass="btnForAlterInGV"  Text="Edit" CommandName="Alter" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                       </ItemTemplate>
                                   </asp:TemplateField>
                                    <%--<asp:ButtonField CommandName="Alter"   ControlStyle-CssClass="btnForAlterInGV"  HeaderText="Alter" ButtonType="Button" Text="Alter" ItemStyle-Width="80px"/>--%>
                                   
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="100px">
                                       <ItemTemplate  >
                                            <asp:Button ID="btnDelete" runat="server" ControlStyle-CssClass="btnForDeleteInGV"  Text="Delete" CommandName="Delete" CommandArgument='<%# Eval("LeaveId")%>'  OnClientClick="return confirm('Are you sure to delete ?')" />
                                       </ItemTemplate>
                                   </asp:TemplateField>
                                     
                                </Columns>
                                <HeaderStyle BackColor="#5EC1FF" Height="28px" />
                            </asp:GridView>
                          </div>  
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>

   <script type="text/javascript">
       function goToNewTabandWindow(url) {
           window.open(url);

       }



    </script>
</asp:Content>
