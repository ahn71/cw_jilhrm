<%@ Page Title="" Language="C#" MasterPageFile="~/leave_nested.Master" AutoEventWireup="true" CodeBehind="MaternityLetterofAuthority.aspx.cs" Inherits="SigmaERP.personnel.MaternityLetterofAuthority" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
          <Triggers>
            
          </Triggers>
          <ContentTemplate>
    <div class="worker_id__main_box">
        <div class="punishment_box_header">
            <h2>Maternity Granted by the doctor</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content">

               <div class="punishment_against">
                    <fieldset>
                    <legend>
                        <b>Option</b>
                    </legend>
                  <table class="employee_table">                 
                        <tbody>
                      <tr>
                        <td width="27%">
                            <asp:RadioButton ID="rdball" Class=""  ClientIDMode="Static" AutoPostBack="true" Checked="true" runat="server" Text="All" OnCheckedChanged="rdball_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdbindividual" ClientIDMode="Static" AutoPostBack="true" runat="server" Text="Individual" OnCheckedChanged="rdbindividual_CheckedChanged" />
                        </td>
                   </tr>       
                      
                    </tbody>
                  </table>
                  </fieldset>
                </div>
                <div  id="divindivisual" class="punishment_against" runat="server">
                   <table class="employee_table">
                        <tr runat="server" id="trEmpType">
                        <td width="27%">
                            <asp:RadioButton ID="rdbWorker" ClientIDMode="Static" Class="" Checked="true" runat="server" Text="Worker" OnCheckedChanged="rdbWorker_CheckedChanged" AutoPostBack="True" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdbStaff" ClientIDMode="Static" runat="server" Text="Staff" AutoPostBack="True" OnCheckedChanged="rdbStaff_CheckedChanged" />
                        </td>
                   </tr> 
                       <tr runat="server" id="trCardNo">
                       <td>
                         Individual Card
                        </td>
                         <td>
                           <asp:DropDownList ID="ddlCardNo" CssClass="form-control select_width" width="129px" runat="server"></asp:DropDownList>
                        </td>
                           <td>
                          <asp:TextBox ID="txtCardNo" PLaceHolder="Type Card No" Width="92px" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_import"></asp:TextBox>              
                           </td>
                           <td>
                               <th><asp:Button ID="btnFind" runat="server" CssClass="back_button" ClientIDMode="Static" Text="Find" OnClick="btnFind_Click"  /></th>
                           </td>
                           </tr>
                   </table>
                </div>
                <div class="punishment_button_area">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                 <th><asp:Button ID="btnpreview" CssClass="back_button" runat="server" ClientIDMode="Static" Text="Preview" OnClick="btnpreview_Click"    /></th>
                                <th><asp:Button ID="btnClose" CssClass="css_btn" Text="Close" PostBackUrl="~/default.aspx" runat="server" /></th> 
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
