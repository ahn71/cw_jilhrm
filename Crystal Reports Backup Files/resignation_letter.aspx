<%@ Page Title="Resignation Letter" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="resignation_letter.aspx.cs" Inherits="SigmaERP.personnel.resignation_letter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <div class="worker_id__main_box">
        <div class="punishment_box_header">
            <h2>Resignation Letter</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content">
                <div  id="divindivisual" class="punishment_against" runat="server">
                    <fieldset>
                    <legend>
                        <b>Option</b>
                    </legend>
                   <table class="employee_table">
                        <tr runat="server" id="trEmpType">
                        <td width="27%">
                            <asp:RadioButton ID="rdbWorker" ClientIDMode="Static" Class="" Checked="true" runat="server" Text="Worker"  AutoPostBack="True" OnCheckedChanged="rdbWorker_CheckedChanged"  />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdbStaff" ClientIDMode="Static" runat="server" Text="Staff" AutoPostBack="True" OnCheckedChanged="rdbStaff_CheckedChanged"  />
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
                               <th><asp:Button ID="btnFind" runat="server" CssClass="back_button" ClientIDMode="Static" Text="Find" OnClick="btnFind_Click"   /></th>
                           </td>
                           </tr>
                   </table>
                        </fieldset>
                </div>
                
                <div class="punishment_button_area">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                <th><asp:Button ID="btnPrintpreview" runat="server" CssClass="back_button" ClientIDMode="Static" Text="Preview" OnClick="btnPrintpreview_Click" /></th>
                                <th><asp:Button ID="btnClose" PostBackUrl="~/default.aspx" Text="Close" runat="server" CssClass="css_btn" /></th>    
                         </tr>
                    </tbody>
                  </table>
                </div>

        </div>
      </div>
    </div>
    <script type="text/javascript">
        function goToNewTabandWindow(url) {
            window.open(url);

        }

    </script>
</asp:Content>
