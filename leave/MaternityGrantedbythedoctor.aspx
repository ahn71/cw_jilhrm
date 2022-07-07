<%@ Page Title="" Language="C#" MasterPageFile="~/leave_nested.Master" AutoEventWireup="true" CodeBehind="MaternityGrantedbythedoctor.aspx.cs" Inherits="SigmaERP.personnel.MaternityGrantedbythedoctor" %>
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
                       
                            <tr id="trIndivisualCardNo" runat="server">
                                <td>
                                    Card No:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEmpCardNo" runat="server" ClientIDMode="Static" CssClass="form-control select_width" Height="26px" ></asp:DropDownList>
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
                                 <th><asp:Button ID="btnpreview" CssClass="back_button" runat="server" ClientIDMode="Static" Text="Preview" OnClick="btnpreview_Click"   /></th>
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
