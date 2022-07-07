<%@ Page Title="Maternity Voucher" Language="C#" MasterPageFile="~/leave_nested.Master" AutoEventWireup="true" CodeBehind="maternity.aspx.cs" Inherits="SigmaERP.personnel.maternity" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 49px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row">
        <div class="col-md-12 ds_nagevation_bar">
            <div style="margin-top: 5px">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li>/</li>
                    <li><a href="/leave_default.aspx">Leave</a></li>
                    <li>/</li>
                    <li><a href="#" class="ds_negevation_inactive">Maternity Voucher</a></li>
                </ul>
            </div>
        </div>
    </div>
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   <asp:UpdatePanel ID="uplMessage" runat="server">
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="rblOptions" />
            <asp:AsyncPostBackTrigger ControlID="rblEmpType" />
            <asp:AsyncPostBackTrigger ControlID="ddlCardNo" />
        </Triggers>
        <ContentTemplate>
              <div class="main_box1">
                  <div class="main_content_box_header">
                         <h2>Maternity Voucher</h2>
                     </div>
     
                     <div class="main_content_box_body">
                        <div class="box_main_content"> 
                             <div class="payment_form">         
                                  <div class="content_left">
                                       <fieldset><legend>Selection</legend>
                                            <asp:RadioButtonList ID="rblOptions" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblOptions_SelectedIndexChanged">
                                                <asp:ListItem Selected="True">1st Installment</asp:ListItem>
                                                <asp:ListItem>2nd Installment</asp:ListItem>
                                                <asp:ListItem>Payment Details</asp:ListItem>

                                            </asp:RadioButtonList>
                                            
                                    </fieldset><br />
                                  </div>

                                 <div class="content_left">
                                       <fieldset><legend>Type</legend>
                                            <asp:RadioButtonList ID="rblEmpType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblEmpType_SelectedIndexChanged" >
                                                
                                            </asp:RadioButtonList>
                                            
                                    </fieldset><br />
                                  </div>

                                  <div class ="content_right" >
                                       <%--<fieldset>
                                             <table>
                                                   <tr>
                                                       <td class="auto-style1">Month :</td>
                                                    <td><asp:DropDownList ID="ddlMonthID" ClientIDMode="Static" CssClass="form-control select_width_leave" runat="server">
                                                        <asp:ListItem>-select-</asp:ListItem>
                                                        <asp:ListItem>Active</asp:ListItem>
                                                        <asp:ListItem>InActive</asp:ListItem>
                                                        </asp:DropDownList>

                                                    </td>
                                                       <td></td>
                                                   </tr>
                                             </table> <br />
                                    </fieldset><br />--%>

                                      <fieldset>
                                             <table>
                                                   <tr>
                                                       <td>Card No :</td>
                                                    <td><asp:DropDownList ID="ddlCardNo" ClientIDMode="Static" CssClass="form-control select_width_leave" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCardNo_SelectedIndexChanged">
                                                      
                                                        </asp:DropDownList>

                                                    </td>
                                                       <td></td>
                                                   </tr>
                                                 <tr>
                                                     <td></td>
                                                     <td>
                                                         <asp:TextBox ID="txtCardNo" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" placeHolder="Type card no for find employee" Font-Bold="true" Enabled="False" ></asp:TextBox>
                                                     </td>
                                                 </tr>
                                             </table> <br />
                                    </fieldset><br />

                                      
                                  </div>

                                            <div class="border">
                                             </div>  
                                              <div class="list_small_button" style="width:280px">
                                                   <table>
                                            <tbody>
                                    
                                                <tr>
                                                <td>
                                              <asp:Button ID="btnPreview" runat="server" Text="Prewiew" 
                                              CssClass="back_button" OnClick="btnPreview_Click"  /></td>
                                            <td>
                                              <asp:Button ID="Button3" PostBackUrl="~/leave_default.aspx" runat="server" Text="Close" Cssclass="css_btn" /> </td>  
                                                    <td>
                                              <asp:Button ID="btnDelete" runat="server" Text="Delete" Cssclass="css_btn"  style="background-color: gray;" OnClick="btnDelete_Click" /> </td>                           
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
        function CloseWindowt() {
            window.close();
        }

        function goToNewTabandWindow(url) {
            window.open(url);

        }

    </script>

</asp:Content>
