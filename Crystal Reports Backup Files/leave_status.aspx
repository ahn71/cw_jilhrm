<%@ Page Title="Leave Status" Language="C#" MasterPageFile="~/leave_nested.Master" AutoEventWireup="true" CodeBehind="leave_status.aspx.cs" Inherits="SigmaERP.personnel.leave_status" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <Triggers>
                   
                    <asp:AsyncPostBackTrigger ControlID="rdoDept" />
                     <%--<asp:AsyncPostBackTrigger ControlID="rdbworker" />--%>
                </Triggers>
                <ContentTemplate>

           <div class="main_box">
                <div class="main_content_box_header">
                     <h2>Leave Status Summary</h2>
                 </div>
     
                 <div class="main_content_box_body">
                
                       <!--ST-->
                    <div class="box_main_content"> 
                          <div class="status_form">
                               <fieldset><legend>Month</legend>
                                         <table>
                                               <tr>
                                                   <td>Month Name :</td>
                                                    <td><asp:DropDownList ID="ddlMonth" ClientIDMode="Static" CssClass="form-control select_width_leave" runat="server">
                                                        <asp:ListItem Value="0" Text="Select One"></asp:ListItem>
                                                                
                                                        </asp:DropDownList>

                                                    </td>
                                                   <td></td>
                                                   <td></td>
                                               </tr>
                                         </table> <br />
                                </fieldset><br />
                               
                              <table>
                                     <tr>
                                         <td align="right">
                                             <asp:RadioButtonList ID="rdoDept" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdoDept_SelectedIndexChanged" AutoPostBack="True">
                                                 <asp:ListItem Text="List All" Value="0" Selected="True"></asp:ListItem>
                                                 <asp:ListItem Text="Department" Value="1"></asp:ListItem>
                                             </asp:RadioButtonList>
                                             </td>
                                         <td></td>
                                         <td></td>
                                     </tr>
                                  </table> <br /> <br />

                                  <table>
                                     <tr>
                                         <td>Depart Name :</td>
                                         <td>
                                                 <asp:DropDownList ID="ddlDepartment"  ClientIDMode="Static" CssClass="form-control select_width_leave" runat="server">
                                                
                                                </asp:DropDownList>

                                         </td>
                                         <td></td>
                                         <td></td>
                                     </tr>
                              </table>
                          </div> 
                        
                        <div class="border">
                         </div>  
                                  <div class="button_middle_status" style="width:420px">
                                       <table>
                                <tbody>
                                    
                                    <tr>
                                    <td>
                                  <asp:Button ID="btnPreview" runat="server" Text="Prewiew" 
                                  CssClass="back_button" OnClick="btnPreview_Click" /></td>
                                <td>
                                    <td>
                               <asp:Button ID="btnMaternityLeave" runat="server" Text="Maternity Leave Preview" 
                                  CssClass="back_button" OnClick="btnMaternityLeave_Click" /></td>
                                    <td>
                              <asp:Button ID="Button3" runat="server" PostBackUrl="~/default.aspx" Text="Close" Cssclass="css_btn" /> </td>                          
                                </tr>

                    </tbody></table>
                             </div>
                        </div>        
                
                   </div>               
             </div>
                  
        </div>
                    </ContentTemplate>
        </asp:UpdatePanel>
     <script type="text/javascript">
         function goToNewTabandWindow(url) {
             window.open(url);


         }
    </script>
</asp:Content>
