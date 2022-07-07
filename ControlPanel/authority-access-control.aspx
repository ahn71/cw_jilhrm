<%@ Page Title="Authority Access Control" Language="C#" MasterPageFile="~/Tools_Nested.master" AutoEventWireup="true" CodeBehind="authority-access-control.aspx.cs" Inherits="SigmaERP.ControlPanel.authority_access_control" %>
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

        .txt{
            margin:3px;
            text-align:center;
            width:58px;
        }
        .btnn {
             margin:3px;
            text-align:center;
        }
        table tr td, table tr th{
            padding: 4px;
        }
        .id_card_left select option{
            padding:5px;
        }
        .id_card_right select option{
             padding:5px;
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
                        <li><a href="#" class="ds_negevation_inactive Tactive">Authority Access Control</a></li>
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
                <div class="employee_box_body" style="background-color:white; min-height:500px;overflow:hidden">                    
        <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <Triggers>
       
           <asp:AsyncPostBackTrigger ControlID="ddlCompany" />
           <asp:AsyncPostBackTrigger ControlID="btnAddItem" />
           <asp:AsyncPostBackTrigger ControlID="btnAddAllItem" />
           <asp:AsyncPostBackTrigger ControlID="btnRemoveItem" />
           <asp:AsyncPostBackTrigger ControlID="btnRemoveAllItem" />
           <asp:AsyncPostBackTrigger ControlID="gvAllAuthorityList" />
           <asp:AsyncPostBackTrigger ControlID="gvSelectedAuthorityList" />
           <asp:AsyncPostBackTrigger ControlID="rblAutoritySetupType" />
           
        </Triggers>
        <ContentTemplate>
           
                 <div>                    
                                    
                     
                     <div id="workerlist" runat="server" class="id_card" style="background-color:white; width:70%;">
                         <table style="width:100%;" >
                              <tr>
                             <td>
                                  
                             </td>
                             <td>
                                 <asp:RadioButtonList runat="server" ID="rblAutoritySetupType" ClientIDMode="Static" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblAutoritySetupType_SelectedIndexChanged" AutoPostBack="true" >
                                     <asp:ListItem Selected="True" Value="Lv">Leave Authoriy Setup</asp:ListItem>
                                     <asp:ListItem Value="OD">Out Duty Authoriy Setup</asp:ListItem>
                                 </asp:RadioButtonList>
                           </td>                           
                         </tr>
                         <tr>
                             <td>
                                 Company 
                             </td>
                             <td>
                                  <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control select_width" Width="80%" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" >
                               </asp:DropDownList>
                           </td>                           
                         </tr>
                     </table>
                     <hr /> 
                            <div class="id_card_left EilistL">
                                <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata EilistCec" style="height:270px !important" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div class="id_card_center EilistC" >
                                <table style="margin-top:60px;" class="employee_table">                    
                              <tr>
                                    <td >
                                        <asp:Button ID="btnAddItem" Class="arrow_button" runat="server" Text=">" OnClick="btnAddItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnAddAllItem" Class="arrow_button" runat="server" Text=">>" OnClick="btnAddAllItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveItem" Class="arrow_button" runat="server" Text="<" OnClick="btnRemoveItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveAllItem" Class="arrow_button" runat="server" Text="<<" OnClick="btnRemoveAllItem_Click"  />
                                    </td>
                               </tr>
                        </table>
                    </div>
                     <div class="id_card_right EilistR">
                                <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata EilistCec"  style="height:270px !important"  ClientIDMode="Static" runat="server"></asp:ListBox>
                            </div>
                         <br />
                         <hr /> 
                         <br />
                         <div>
                             <h4>AllAuthority</h4>
                              <asp:GridView ID="gvAllAuthorityList" runat="server"  Width="100%" AutoGenerateColumns="False" DataKeyNames="UserId" OnRowCommand="gvAllAuthorityList_RowCommand">
<HeaderStyle BackColor="#0057AE" Font-Bold="True" Font-Size="14px" ForeColor="White" Height="28px"></HeaderStyle>  
                         <Columns>
                            <asp:BoundField DataField="EmpCardNo"  HeaderText="Card No" ItemStyle-Height="28px" >
                             </asp:BoundField>  
                             <asp:BoundField DataField="EmpName"  HeaderText="Name" ItemStyle-Height="28px" >
                             </asp:BoundField>  
                             <asp:BoundField DataField="DsgName"  HeaderText="Designation" ItemStyle-Height="28px" >
                             </asp:BoundField>  
                             <asp:BoundField DataField="DptName"  HeaderText="Department" ItemStyle-Height="28px" >
                             </asp:BoundField> 
                             <asp:TemplateField HeaderText="Possition" >
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtLvAuthorityOrder" CssClass="txt form-control"  Text='<%#Eval("LvAuthorityOrder")%>'></asp:TextBox>   
                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Action" >
                                <ItemTemplate>
                                    <asp:RadioButtonList runat="server" ID="rblAuthorityAction" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="true" Value="1">Forword</asp:ListItem>
                                                <asp:ListItem Value="2">Approved</asp:ListItem>
                                                <asp:ListItem Value="0">Both</asp:ListItem>
                                            </asp:RadioButtonList>                                  
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Add" >
                                <ItemTemplate>
                                    <asp:Button ID="btnAdd" runat="server" CommandName="Add" CssClass="btn btn-primary btnn"  Text="Add" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />                    
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                         </div>
                         <br />
                         <hr />
                         <h4>Selected Authority</h4>
                           <div>
                    <asp:GridView ID="gvSelectedAuthorityList" runat="server"  Width="100%" AutoGenerateColumns="False" DataKeyNames="rIndexAll,UserId,LvAuthorityAction" OnRowCommand="gvSelectedAuthorityList_RowCommand" OnRowDataBound="gvSelectedAuthorityList_RowDataBound">
<HeaderStyle BackColor="#0057AE" Font-Bold="True" Font-Size="14px" ForeColor="White" Height="28px"></HeaderStyle>  
                         <Columns>
                            <asp:BoundField DataField="EmpCardNo"  HeaderText="Card No" ItemStyle-Height="28px" >
                             </asp:BoundField>  
                             <asp:BoundField DataField="EmpName"  HeaderText="Name" ItemStyle-Height="28px" >
                             </asp:BoundField>  
                             <asp:BoundField DataField="DsgName"  HeaderText="Designation" ItemStyle-Height="28px" >
                             </asp:BoundField>  
                             <asp:BoundField DataField="DptName"  HeaderText="Department" ItemStyle-Height="28px" >
                             </asp:BoundField> 
                          <asp:TemplateField HeaderText="Possition" >
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtLvAuthorityOrder" CssClass="txt form-control" Text='<%#Eval("LvAuthorityOrder") %>'></asp:TextBox>                                     
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Action" >
                                <ItemTemplate>
                                    <asp:RadioButtonList runat="server" ID="rblAuthorityAction" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="true" Value="1">Forword</asp:ListItem>
                                                <asp:ListItem Value="2">Approved</asp:ListItem>
                                                <asp:ListItem Value="0">Both</asp:ListItem>
                                            </asp:RadioButtonList>                                  
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Remove" >
                                <ItemTemplate>
                                    <asp:Button ID="btnRemove" runat="server" CommandName="Remove" CssClass="btn btn-danger btnn"  Text="Remove" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />                    
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                </div>
                 </div>     
            <br />
                <div>
                    <asp:GridView ID="gvEmployeeList" runat="server"  Width="100%" AutoGenerateColumns="False" DataKeyNames="EmpId,Authority" OnRowCommand="gvEmployeeList_RowCommand" OnRowDataBound="gvEmployeeList_RowDataBound">
<HeaderStyle BackColor="#0057AE" Font-Bold="True" Font-Size="14px" ForeColor="White" Height="28px"></HeaderStyle>  
                         <Columns>
                              <asp:TemplateField HeaderText="SL">
                                <ItemTemplate>
                                     <%# Container.DataItemIndex + 1 %>                                  
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="EmpCardNo"  HeaderText="Card No" ItemStyle-Height="28px" >
                             </asp:BoundField>  
                             <asp:BoundField DataField="EmpName"  HeaderText="Name" ItemStyle-Height="28px" >
                             </asp:BoundField>  
                             <asp:BoundField DataField="DsgName"  HeaderText="Designation" ItemStyle-Height="28px" >
                             </asp:BoundField>  
                             <asp:BoundField DataField="DptName"  HeaderText="Department" ItemStyle-Height="28px" >
                             </asp:BoundField>
                             <asp:BoundField DataField="Authority"  HeaderText="Authority" ItemStyle-Height="28px" >
                             </asp:BoundField>            
                               <asp:TemplateField>
                                   <HeaderTemplate>
                                       <asp:CheckBox runat="server" ID="ckbEmpAll" ClientIDMode="Static" Checked="true" Text="All" AutoPostBack="true" OnCheckedChanged="ckbEmpAll_CheckedChanged" />
                                   </HeaderTemplate>
                                <ItemTemplate>
                                     <asp:CheckBox runat="server" ID="ckbEmp" ClientIDMode="Static" Checked="true" />
                                </ItemTemplate>
                            </asp:TemplateField>
                               <asp:TemplateField HeaderText="" >
                                <ItemTemplate>
                                     <asp:Button ID="btnDirectApproved" runat="server" CommandName="directApproval" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Direct Approval" CssClass="btn btn-success btnn" OnClientClick="return confirm('Are you sure, This employee is allowed for Direct approval?');" ></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" >
                                <ItemTemplate>
                                     <asp:Button ID="lnkDeleteEmp" runat="server" CommandName="remove" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Delete" CssClass="btn btn-danger btnn" OnClientClick="return confirm('Are you sure to delete?');" ></asp:Button>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <div class="pull-right" style="padding:10px 0">
                       <asp:Button runat="server" ID="btnSubmit" Text="Submit" ClientIDMode="Static" CssClass="btn btn-success"  OnClick="btnSubmit_Click" />
                   </div>
                </div> 
  <br />
       

      
                  
                   </ContentTemplate>
    </asp:UpdatePanel>    
    </div>
    </div>
</asp:Content>
