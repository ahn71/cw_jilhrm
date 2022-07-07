<%@ Page Title="Earn Leave Generation" Language="C#" MasterPageFile="~/leave_nested.master" AutoEventWireup="true" CodeBehind="earnleavegeneration.aspx.cs" Inherits="SigmaERP.leave.earnleavegeneration" %>
<%@ Register Assembly="ComplexScriptingWebControl" Namespace="ComplexScriptingWebControl" TagPrefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                       <li> <a href="#" class="ds_negevation_inactive Lactive">Earn Leave Generation</a></li>
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
            <h2>Earn Leave Generation Panel</h2>
        </div>
        <div class="main_box_body_leave Lbody">
            <div class="main_box_content_leave" id="divElementContainer" runat="server">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers> 
                        <asp:AsyncPostBackTrigger ControlID="btnGenerate" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="input_division_info">
                            <table class="employee_table">                                
                                 <tr>
                                         <td>
                                            Company
                                        </td>                                      
                                        <td>
                                            <asp:DropDownList ID="ddlCompanyList" runat="server"  CssClass="form-control select_width" ClientIDMode="Static"></asp:DropDownList>
                                        </td>                                       
                                    </tr>
                                    <tr>
                                        <td>Year
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="form-control text_box_width"  ClientIDMode="Static" ID="txtStartDate" runat="server" autocomplete="off"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtGenerateMonth_CalendarExtender" Format="yyyy" runat="server" TargetControlID="txtStartDate">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr runat="server" visible="false">
                                        <td>End Date
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="form-control text_box_width"  ClientIDMode="Static" ID="txtEndDate" runat="server" autocomplete="off"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" Format="dd-MM-yyyy" runat="server" TargetControlID="txtEndDate">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                     <tr runat="server" visible="false">
                                        <td>
                                            Excepted Card No
                                        </td>
                                        <td>
                                           <asp:TextBox ID="txtExceptedEmpCardNo"  ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" PLaceHolder="990001,990002,990003,......n" autocomplete="off"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            
                                        </td>
                                        <td>
                                           <asp:TextBox ID="txtEmpCardNo"  ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" PLaceHolder="Type card no like 0001 to individual generation" autocomplete="off"></asp:TextBox>
                                        </td>
                                    </tr> 
                                <tr>
                                        <td>
                                           
                                        </td>
                                        <td>
                                            <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" ClientIDMode="Static"  />
                                        </td>
                                    </tr>
                                <tr>
                                        <td>
                                           
                                        </td>
                                        <td>
                                            <br />
                                            <br />
                                            <asp:Button ID="btnGenerate"  CssClass="Lbutton" runat="server" Text="Generate" OnClientClick="return processing();" OnClick="btnGenerate_Click" /> 
                                        </td>
                                    </tr>
                                    
                            </table>
                        </div>
                       
                        <div >
                         
                               
                           <div class="show_division_info">
                           <asp:GridView runat="server" ID="gvEarnLeaveGenerationList" CssClass="gvdisplay1" DataKeyNames="CompanyId,StartDate1,EndDate1,IsSeparated" AutoGenerateColumns="false" HeaderStyle-BackColor="#ffa500" HeaderStyle-Height="28px" HeaderStyle-ForeColor="White" Width="100%">
                                            <Columns>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>SL</HeaderTemplate>
                                                    <ItemTemplate >
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Month" HeaderText="Generate Month" />                                                                                              
                                                <asp:BoundField DataField="StartDate" HeaderText="Start Date" />                                                                                              
                                                <asp:BoundField DataField="EndDate" HeaderText="End Date" />                                                                                              
                                                <asp:BoundField DataField="IsSeparatedText" HeaderText="Is Separated" />                                                                                              
                                                <asp:TemplateField HeaderText="Delete"  HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                  <ItemTemplate >
                                      <asp:Button ID="btnRemove" runat="server" CommandName="Remove" Width="55px" Height="30px" Font-Bold="true" ForeColor="red" Text="Delete" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                  </ItemTemplate>
                              </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                </div>
                          </div>  
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>

   <script type="text/javascript">
       $('#imgLoading').hide();
       function processing()
       {              
        $('#imgLoading').show();
        return true;
       }
        function msg(type,msg) {
        showMessage(type+"->"+msg);
        $('#imgLoading').hide();        
    }
   </script>
</asp:Content>