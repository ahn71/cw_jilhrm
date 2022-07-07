<%@ Page Title="Promotion Letter" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="promotion_letter.aspx.cs" Inherits="SigmaERP.personnel.promotion_letter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
      <asp:HiddenField ID="upSuperAdmin" runat="server" ClientIDMode="Static" />
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="rdbStaff" />
             <asp:AsyncPostBackTrigger ControlID="rdbWorker" />
            <asp:AsyncPostBackTrigger ControlID="btnPreview" />

        </Triggers>
        <ContentTemplate>
            
    <div class="worker_id__main_box">
        <div class="punishment_box_header">
            <h2>Promotion Letter</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content">
        <div class="punishment_against">
        <table class="employee_table">                     
                  <tr>
                        <td width="27%">
                            <asp:RadioButton ID="rdbStaff" Class=""  runat="server" Text="Staff" AutoPostBack="True" OnCheckedChanged="rdbStaff_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdbWorker" runat="server" Text="Worker" OnCheckedChanged="rdbWorker_CheckedChanged" AutoPostBack="True" />
                        </td>
                   </tr>
            </table>
            </div>

                <div class="punishment_against">
                    <fieldset>
                    <legend>
                        <b>Employee</b>
                    </legend>
                  <table class="employee_table">                 
                        <tbody>
                      <tr>
                            <td>
                                Card No
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                               <asp:DropDownList ID="ddlCardNo" ClientIDMode="Static" CssClass="form-control select_width" runat="server">
                                   
                                </asp:DropDownList>
                                
                            </td>
                        </tr>            
                      
                    </tbody>
                  </table>
                  </fieldset>
                </div>
                <div class="punishment_button_area">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                <th>  <asp:Button ID="btnPreview" ClientIDMode="Static" Width="80px" CssClass="emp_btn" runat="server" Text="Preview" OnClick="btnPreview_Click" /></th>
                               <th>  <asp:Button ID="btnClose" ClientIDMode="Static" Width="80px" CssClass="emp_btn" runat="server" Text="Close" PostBackUrl="~/default.aspx"  /></th>
                         </tr>
                    </tbody>
                  </table>
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
