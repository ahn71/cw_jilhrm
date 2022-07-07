<%@ Page Title="" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="salary_BankCash_report.aspx.cs" Inherits="SigmaERP.personnel.salary_BankCash_report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:HiddenField ID="upSuperAdmin" runat="server" ClientIDMode="Static" />
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
          <Triggers>
              <asp:AsyncPostBackTrigger ControlID="rblSalaryCount" />                        
          </Triggers>
          <ContentTemplate>
    <div class="worker_id__main_box">
        <div class="punishment_box_header">
            <h2>Salary Count</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content">

                <div class="punishment_against">
                        <h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center">You Have Not Any Access Permission!</h1>
                     <table runat="server" visible="false" id="tblGenerateType" class="employee_table">
                        <tr>
                            <td>
                                Company
                            </td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList ID="ddlBranch" ClientIDMode="Static"   CssClass="form-control select_width" runat="server" AutoPostBack="True">
                                    
                                </asp:DropDownList>
                            </td>
                        </tr>   
                        </table>
                    <fieldset runat="server" id="fsRadioBtn">
                    <legend>
                        <b>Options</b>
                    </legend>
                  <table class="employee_table" style="margin:0px auto; width:122px">                 
                        <tbody>
                      <tr>
                        <td>
                            <asp:RadioButtonList runat="server" ID="rblSalaryCount" ClientIDMode="Static" RepeatDirection="Horizontal" AutoPostBack="true">
                                <asp:ListItem Selected="True" Value="Bank">Bank
                                </asp:ListItem>
                                 <asp:ListItem Value="Cash">Cash
                                </asp:ListItem>
                            </asp:RadioButtonList>
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
                                <th><asp:Button ID="btnPrintpreview" runat="server" CssClass="css_btn" ClientIDMode="Static" Text="Preview" OnClick="btnPrintpreview_Click" /></th>
                                <th><asp:Button ID="btnClose" PostBackUrl="~/personnel_defult.aspx" Text="Close" runat="server" CssClass="css_btn" /></th>   
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
