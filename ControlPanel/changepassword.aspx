<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Tools_Nested.Master" AutoEventWireup="true" CodeBehind="changepassword.aspx.cs" Inherits="SigmaERP.ControlPanel.changepassword" %>
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
    <style type="text/css">
        #divUserAccountInfo {
            width: 652px;
        }
        #ContentPlaceHolder1_MainContent_gvAccountList th,td {
            padding-left:3px;
        }
        #ContentPlaceHolder1_MainContent_gvAccountList th:nth-child(6), th:nth-child(5){
           text-align:center;
        }
        .largerCheckbox
        {
        width: 60px;
        height: 60px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <div style="margin-top: 5px">
                    <ul>
                        <li><a href="/default.aspx">Dasboard</a></li>
                        <li>/</li>
                        <li><a href="/tools_default.aspx">Tools</a></li>
                        <li>/</li>
                        <li><a href="#" class="ds_negevation_inactive Tactive">User Privilege</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    

    <asp:UpdatePanel ID="uplMessage" runat="server">
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>

    <asp:HiddenField ID="hfUserId" runat="server" ClientIDMode="Static" />
          <div id="divChangePasswordMainBox" runat="server" class="create_account_main_box Mbox">
                <div class="employee_box_header TBoxheader">
                    <h2>Change Password</h2>
                </div>
                    <div class="punishment_bottom_header">
                    </div>
              <div class="employee_box_body Tbody" style="width:100%">                 
                      <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                                  <Triggers>
                                      <asp:AsyncPostBackTrigger ControlID="chkShowUserNamePassword" />
                                      <asp:AsyncPostBackTrigger ControlID="btnClear" />
                                      <asp:AsyncPostBackTrigger ControlID="btnSave" />
                                      <asp:AsyncPostBackTrigger ControlID="rblShowType" />
                                      <asp:AsyncPostBackTrigger ControlID="ddlShift" />
                                      <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                                      
                                  </Triggers>
                                  <ContentTemplate>
                    <div  class="change_password_content">
                        <div class="change_password_left_area">
                          <div class="input_division_info">
                            

                                 
                            <table class="division_table">
                           
                                <tr>
                                    <td>Company</td>
                                        <td>:</td>
                                        <td>
                                        <asp:DropDownList  ID="ddlCompanyList" ClientIDMode="Static" Width="330px" Height="30px" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged"   >              
                                         </asp:DropDownList>
                                        </td>  

                                </tr>
                                     <tr style="display:none">
                                  <td>Shift</td>
                                       <td>:</td>
                                      <td>
                                         <asp:DropDownList runat="server" ID="ddlShift" CssClass="form-control text_box_width style" Width="330px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlShift_SelectedIndexChanged"   ></asp:DropDownList>
                                     </td> 
                                </tr>

                                <tr>
                                    <td>Full Name</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtFirstName" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="320px" Enabled="False"></asp:TextBox>
                                    </td>
                                </tr>

                            <%--    <tr>
                                    <td style="vertical-align: text-top;">Last Name</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtLastName" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="320px" Enabled="False"></asp:TextBox>                                      
                                    </td>
                                </tr>--%>
                                      <tr>
                                          <td></td>
                                          <td></td>
                                          <td>
                                        <asp:CheckBox Style="font-weight: bold; color: green; font-family: ms; font-size: 14px;" ID="chkShowUserNamePassword" runat="server" ClientIDMode="Static" Text="Show User Name & Password" AutoPostBack="True" OnCheckedChanged="chkShowUserNamePassword_CheckedChanged" />
                                        <asp:HiddenField ID="hfStatus" runat="server" ClientIDMode="Static" Value="1" /></td>
                                          
                                      </tr>     
                                     <tr>
                                    <td>User Name</td>
                                    <td >:</td>
                                    <td >
                                       <asp:TextBox ID="txtUserName" ClientIDMode="Static" runat="server"  CssClass="form-control text_box_width" Width="320px" Enabled="False" Font-Bold="True" Font-Names="Times New Roman"></asp:TextBox> 
                                    </td>
                                  </tr>
                                     <tr>
                                    <td>Old Password</td>
                                    <td >:</td>
                                    <td>
                                       <asp:TextBox ID="txtPassword" ClientIDMode="Static" runat="server"  CssClass="form-control text_box_width" Width="320px" Enabled="False" Font-Bold="True" Font-Names="Times New Roman"></asp:TextBox>
                                    </td>
                                  </tr>
                                    <tr>
                                    <td>New Password</td>
                                    <td >:</td>
                                    <td>
                                       <asp:TextBox ID="txtNewPassword" ClientIDMode="Static" runat="server"  CssClass="form-control text_box_width" Width="320px" TextMode="Password"></asp:TextBox>
                                    </td>
                                  </tr>
                                     <tr>
                                    <td>Email</td>
                                    <td >:</td>
                                    <td>
                                      <asp:TextBox ID="txtEmail" ClientIDMode="Static" runat="server"  CssClass="form-control text_box_width" Width="320px"></asp:TextBox>
                                    </td>
                                  </tr>   
                    
                                  <tr>
                                    <td>Type</td>
                                    <td >:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control select_width" Width="330px">
                                             <asp:ListItem Value="Master Admin">Master Admin</asp:ListItem>
                                            <asp:ListItem Value="Super Admin">Super Admin</asp:ListItem>
                                            <asp:ListItem Value="Admin">Admin</asp:ListItem>           
                                             <asp:ListItem Value="User">User</asp:ListItem>                                    
                                            <asp:ListItem Value="Viewer">Viewer</asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </td>
                                  </tr> 
                                <tr>
                                    <td>Status</td>
                                    <td >:</td>
                                    <td>
                                        <asp:CheckBox ID="chkStatus" runat="server" ClientIDMode="Static" Text="Is Active ?"/>
                                            
                                    </td>
                                  </tr> 

                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="ckbLeaveAuthority" Text=" If Leave Authority" AutoPostBack="true" OnCheckedChanged="ckbLeaveAuthority_CheckedChanged" />
                                    </td>
                                </tr>
                            
                                <asp:Panel runat="server" ID="pLeaveAuthority" Visible="false">
                                    <tr>
                                        <td>Position</td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="txtOrder" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="320px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FT1" runat="server" FilterType="Numbers"
                                                TargetControlID="txtOrder" ValidChars="">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Authority</td>
                                        <td>:</td>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rblLeaveAuthority" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="true" Value="1">Forword</asp:ListItem>
                                                <asp:ListItem Value="2">Approved</asp:ListItem>
                                                <asp:ListItem Value="0">Both</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Emp Type</td>
                                        <td>:</td>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rblEmpType" RepeatDirection="Horizontal">
                                            </asp:RadioButtonList></td>
                                    </tr>
                                    <tr>
                                        <td>Authorized area</td>
                                        <td>:</td>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rblOnlyDepartment" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
                                                <asp:ListItem Value="1">Only Depertment</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </asp:Panel>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="ckbODAuthority" Text=" If Outduty Authority" AutoPostBack="true" OnCheckedChanged="ckbODAuthority_CheckedChanged"/>
                                    </td>
                                </tr>
                            
                                <asp:Panel runat="server" ID="pODAuthority" Visible="false">
                                    <tr>
                                        <td>Position (OD)</td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="txtOrderOD" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="320px"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                                TargetControlID="txtOrderOD" ValidChars="">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Authority(OD)</td>
                                        <td>:</td>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rblODAuthority" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="true" Value="1">Forword</asp:ListItem>
                                                <asp:ListItem Value="2">Approved</asp:ListItem>
                                                <asp:ListItem Value="0">Both</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Emp Type (OD)</td>
                                        <td>:</td>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rblEmpTypeOD" RepeatDirection="Horizontal">
                                            </asp:RadioButtonList></td>
                                    </tr>
                                    <tr>
                                        <td>Authorized area (OD)</td>
                                        <td>:</td>
                                        <td>
                                            <asp:RadioButtonList runat="server" ID="rblOnlyDepartmentOD" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
                                                <asp:ListItem Value="1">Only Depertment</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>


                        </div>

                                       <div class="button_area_create_ammount">
                                          <table class="button_table">
                                              <tr>
                                                  <th>
                                                      <asp:Button ID="btnSave" ClientIDMode="Static" CssClass="Tbutton" runat="server" Text="Change" OnClick="btnSave_Click" />
                                                  </th>
                                                  <th>
                                                      <asp:Button ID="btnClear" ClientIDMode="Static" CssClass="Tbutton" runat="server" Text="Clear" OnClick="btnClear_Click" />
                                                  </th>
                                                 <%-- <th>
                                                      <asp:Button ID="btnDelete" ClientIDMode="Static" CssClass="Tbutton" runat="server" Text="Delete" />
                                                  </th>--%>

                                              </tr>
                                          </table>
                                           
                                       </div>
                            
                             </div>
                        <br />
                        <center>
                            <asp:RadioButtonList runat="server" ID="rblShowType" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblShowType_SelectedIndexChanged" > 
                                          <asp:ListItem  Selected="True" Value="0">Show All</asp:ListItem>
                                          <asp:ListItem Value="1">Show Only Leave Management Authority</asp:ListItem>
                                <asp:ListItem Value="2">Show Only Outduty Management Authority</asp:ListItem>
                                      </asp:RadioButtonList>
                        </center>
                             
                        </div>                                      
                                     
                                      
                                      <asp:GridView BackColor="White" ID="gvAccountList" DataKeyNames="UserId" runat="server"  HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#0000ff" HeaderStyle-Height="28px"  CssClass="table_access_control" Width="100%" AutoGenerateColumns="False"  OnRowCommand="gvAccountList_RowCommand" OnRowDataBound="gvAccountList_RowDataBound">
                                            <Columns>
                                              <asp:BoundField DataField="EmpName" HeaderText="Full Name" Visible="true" ItemStyle-HorizontalAlign="left" ItemStyle-Height="26px"/>
                                                <asp:BoundField DataField="LvAuthorityOrder" HeaderStyle-HorizontalAlign="Center" HeaderText="Leave Autority Position"  Visible="true" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="UserName" HeaderText="User Id"  Visible="true" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="UserPassword" HeaderText="Password"  Visible="true" ItemStyle-HorizontalAlign="Left" />                          
                                                <asp:BoundField DataField="UserType" HeaderText="User Type"  Visible="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkStatus" runat="server" Checked='<%#bool.Parse(Eval("Status").ToString())%>' CssClass="largerCheckbox" Enabled="false"  />
                                                    </ItemTemplate> 
                                                    <ItemStyle HorizontalAlign="Center" Width="45px" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button  runat="server" ID="btnAlter" CommandName="Alter" Text="Edit" Width="60px" Height="24px" ForeColor="Green" Font-Bold="true" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                                    </ItemTemplate> 
                                                </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Button  runat="server" ID="btnRemove" CommandName="Remove" Text="Delete" Width="60px" Height="24px" ForeColor="Red" Font-Bold="true" OnClientClick="return confirm('Are you sure to delete?');" CommandArgument='<%#((GridViewRow)Container).RowIndex%>'  />
                                                    </ItemTemplate> 
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>         
                                   </ContentTemplate>
                              </asp:UpdatePanel>
              </div>
</div>
    <script type="text/ecmascript">
        function editAccount(id) {
           
            $('#hfUserId').val(id);
            $('#txtFirstName').val($('#r_' + id + ' td:first').html());
            
            $('#txtLastName').val($('#r_' + id + ' td:nth-child(2)').html());

            $('#txtUserName').val($('#r_' + id + ' td:nth-child(3)').html());
            $('#txtPassword').val($('#r_' + id + ' td:nth-child(4)').html());


            $('#txtEmail').val($('#r_' + id + ' td:nth-child(5)').html());

            $('#ddlUserType').val($('#r_' + id + ' td:nth-child(6)').html());

            if ($('#r_' + id + ' td:nth-child(7)').html() == "True")
            {
                
                $('#chkStatus').prop('checked',true);
            }
            $('#chkShowUserNamePassword').attr('checked', false);
            $('#hfStatus').val('0');

            
        }
    </script>


</asp:Content>
