<%@ Page Title="User Privilege" Language="C#" MasterPageFile="~/Tools_Nested.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="user_privilege.aspx.cs" Inherits="SigmaERP.ControlPanel.user_privilege" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../style/list_checkbox_style.css" rel="stylesheet" />
    <script src="../scripts/jquery-1.8.2.js"></script>
    <script type="text/javascript">

        var oldgridcolor;
        function SetMouseOver(element) {
            oldgridcolor = element.style.backgroundColor;
            element.style.backgroundColor = '#ffeb95';
            element.style.cursor = 'pointer';
            element.style.textDecoration = 'underline';
        }
        function SetMouseOut(element) {
            element.style.backgroundColor = oldgridcolor;
            element.style.textDecoration = 'none';

        }

</script>

    <style type="text/css">
        #ContentPlaceHolder1_MainContent_gvPageList th {
            text-align:center;
        }
     
          #ContentPlaceHolder1_MainContent_gvPageList th:first-child,#ContentPlaceHolder1_MainContent_gvPageList th:nth-child(2) {
            text-align:left;
        }
        .leftBox {
        
        border-color: #808080;
        border-style: solid;
        border-width: 1px;
        float: left;
        height:75px;
        width: 100%;
        }
        .rightBox {
        border-color: #808080;
        border-style: solid;
        border-width: 1px;
        float: left;
        height: 420px;
        width: 100%;
        padding-top:5px;
        }

        .input.largerCheckbox
        {
        width: 30px;
        height: 30px;
         }

    </style>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
          <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <div style="margin-top: 5px">
                    <ul>
                        <li><a href="/default.aspx">Dasboard</a></li>
                        <li>/</li>
                        <li><a href="/tools_default.aspx">Tools</a></li>
                        <li>/</li>
                        <li><a href="#" class="ds_negevation_inactive Tactive">Change Password</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
  <div id="divChangePasswordMainBox" runat="server" class="create_account_main_box Mbox">
                <div class="employee_box_header TBoxheader">
                    <h2>User Privilege Panel</h2>
                </div>
              <%--  <div class="punishment_bottom_header" style="width: 900px;">
                    
                    
                </div>--%>
                <div class="employee_box_body" style="background-color:white; min-height:500px">                    
        <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="rbProjectModule" />
            <asp:AsyncPostBackTrigger ControlID="dlExistsUser" />
           <asp:AsyncPostBackTrigger ControlID="ddlCompany" />
            <%--<asp:AsyncPostBackTrigger ControlID="ddlDepartment" />--%>
        </Triggers>
        <ContentTemplate>
           
                 <div >
                     <div style="margin-left: 87px; margin-top: 9px;">
                         <asp:Button ID="btnPopup" runat="server" style="display:none" Text="Close"   CssClass="css_btn" />
                     </div> 
                                    
                     <table style="width:100%;">
                         <tr>
                             <td>
                                 Company 
                             </td>
                             <td>
                                  <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control select_width" Width="80%" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                               </asp:DropDownList>
                             </td>
                          <%--    <td>
                                 Department 
                             </td>
                             <td>
                                  <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control select_width" Width="198px" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" >
                                           
                                        </asp:DropDownList>
                             </td>--%>

                             <td style="text-align:right;">
                                 Select User :
                             </td>
                             <td>
                                 <asp:DropDownList ID="dlExistsUser" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" AutoPostBack="True" OnSelectedIndexChanged="dlExistsUser_SelectedIndexChanged">
                               </asp:DropDownList><asp:Label ID="lblUserType" runat="server" Font-Bold="True" ForeColor="#CC0000" ></asp:Label>
                             </td>
                             <td style="text-align:right;">
                                 <asp:Button ID="btnDelete" runat="server" Text="Delete" style="margin-top: 5px; width: 80px; margin-left: -2px;" OnClick="btnDelete_Click" Visible="False" />
                             </td>
                         </tr>
                     </table>
                     <hr />                    
                 </div>     
                 <center>
                     <div>
                        <table runat="server" id="tblMo">
                            <tr>
                                <td>Module Privilege </td>
                                <td> : </td>
                                <td>
                                    <asp:CheckBoxList runat="server" ID="ckblModuleList" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="ckblModuleList_SelectedIndexChanged">
                                         <asp:ListItem Value="1">Settings</asp:ListItem>
                                        <asp:ListItem Value="2">Personnel</asp:ListItem>
                                        <asp:ListItem Value="3">Leave</asp:ListItem>
                                        <asp:ListItem Value="4">Attendance</asp:ListItem>  
                                         <asp:ListItem Value="6">Payroll</asp:ListItem>                                     
                                        <asp:ListItem Value="5">Tools</asp:ListItem>
                                    </asp:CheckBoxList>                                  
                                </td>
                            </tr>
                        </table>
                     </div>
                         </center>  
              <center>
                     <div>
                        <table>
                            <tr>
                                <td>Module </td>
                                <td> : </td>
                                <td>
                                    <asp:RadioButtonList ID="rbProjectModule" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbProjectModule_SelectedIndexChanged" RepeatDirection="Horizontal" Font-Bold="true" ForeColor="Green">
                                        <asp:ListItem Value="0" Selected="true">None</asp:ListItem>
                                        <asp:ListItem Value="1">Settings</asp:ListItem>
                                        <asp:ListItem Value="2">Personnel</asp:ListItem>
                                        <asp:ListItem Value="3">Leave</asp:ListItem>
                                        <asp:ListItem Value="4">Attendance</asp:ListItem>  
                                         <asp:ListItem Value="6">Payroll</asp:ListItem>                                     
                                        <asp:ListItem Value="5">Tools</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                     </div>
                         </center>    
   
            <div  runat="server" id="divRightBox"> 
                <asp:GridView Width="100%" runat="server" ID="gvPageList" AutoGenerateColumns="False" HeaderStyle-Height="28px"  RowStyle-Height="24px"  HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" DataKeyNames="ModulePageName" BackColor="#0000FF" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" OnRowDataBound="gvPageList_RowDataBound" CellSpacing="2" ForeColor="Black">
                    <Columns>

                       


                        <asp:BoundField DataField="PageTitle" HeaderText="Page Title" ItemStyle-Font-Bold="true" >

                    <ItemStyle Width="300px" Font-Underline="false" />
                            
                        </asp:BoundField>
                        <asp:BoundField DataField="ModulePageName" HeaderText="Page Name" ItemStyle-Font-Bold="true" >
                            <ItemStyle Width="300px" Font-Underline="false" />
                      
                            
                        </asp:BoundField>

                     <asp:TemplateField HeaderText="Read" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="45px">
                            <ItemTemplate>
                                   <div class="SingleCheckbox">  
                                <asp:CheckBox ID="chkReadAction" runat="server" HeaderText="ReadAction" Text="&nbsp;"
                                    Checked='<%#bool.Parse(Eval("ReadAction").ToString())%>' OnCheckedChanged="chkReadAction_CheckedChanged" AutoPostBack="true" />
                           </div>
                                        </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="45px" />
                        </asp:TemplateField>
                       
                           <asp:TemplateField HeaderText="Write" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" >
                            <ItemTemplate>
                                 <div class="SingleCheckbox">  
                                <asp:CheckBox ID="chkWriteAction" runat="server" HeaderText="WriteAction" Text="&nbsp;"
                                    Checked='<%#bool.Parse(Eval("WriteAction").ToString())%>' OnCheckedChanged="chkWriteAction_CheckedChanged" AutoPostBack="true"  />
                          </div>
                                       </ItemTemplate>
                               <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                       
                           <asp:TemplateField HeaderText="Update" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="55px"  >
                            <ItemTemplate>
                                 <div class="SingleCheckbox">  
                                <asp:CheckBox ID="chkUpdateAction" runat="server" HeaderText="UpdateAction" Text="&nbsp;"
                                    Checked='<%#bool.Parse(Eval("UpdateAction").ToString())%>'  OnCheckedChanged="chkUpdateAction_CheckedChanged" AutoPostBack="true"  />
                            </div>
                                     </ItemTemplate>
                               <ItemStyle HorizontalAlign="Center" Width="55px" />
                        </asp:TemplateField>


                           <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="55px" >
                            <ItemTemplate>
                                 <div class="SingleCheckbox">  
                                <asp:CheckBox ID="chkDeleteAction" runat="server" HeaderText="DeleteAction" Text="&nbsp;"
                                    Checked='<%#bool.Parse(Eval("DeleteAction").ToString())%>' OnCheckedChanged="chkDeleteAction_CheckedChanged" AutoPostBack="true"  />
                         </div>
                                        </ItemTemplate>
                               <ItemStyle HorizontalAlign="Center" Width="55px" />
                        </asp:TemplateField>

                           <asp:TemplateField HeaderText="All" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="45px">
                            <ItemTemplate>
                                 <div class="SingleCheckbox">
                                <asp:CheckBox ID="chkAllAction" runat="server" HeaderText="All Action" Text="&nbsp;"
                                    Checked='<%#bool.Parse(Eval("AllAction").ToString())%>' OnCheckedChanged="chkAllAction_CheckedChanged" AutoPostBack="true" />
                           </div>
                                      </ItemTemplate>
                               <ItemStyle HorizontalAlign="Center" Width="45px" />
                        </asp:TemplateField>

                      
                       
                           
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" />
                    <HeaderStyle BackColor="#0000FF" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                    <RowStyle BackColor="White" />
                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#808080" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#383838" />
                </asp:GridView>
                       </div>
                  
                   </ContentTemplate>
    </asp:UpdatePanel>    
    </div>
    </div>
</asp:Content>
