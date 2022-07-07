<%@ Page Title="Appoinment Letter" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="appoinment_letter.aspx.cs" Inherits="SigmaERP.personnel.appoinment_letter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:HiddenField ID="upSuperAdmin" runat="server" ClientIDMode="Static" />
    <div class="employe_list_main_box">
        <div class="punishment_box_header">
            <h2>Appoinment Letter</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content">
        <div class="punishment_against">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <Triggers>
                   
                    <asp:AsyncPostBackTrigger ControlID="rdbstaff" />
                     <asp:AsyncPostBackTrigger ControlID="rdbworker" />
                </Triggers>
                <ContentTemplate>
        <table class="employee_table">  
             <asp:HiddenField ClientIDMode="Static" ID="hdfEmptypeId" Value="" runat="server" />                   
                  <tr>
                        <td style="width:35%">
                            <asp:RadioButton ID="rdbstaff" Class="" runat="server" Text="Staff" AutoPostBack="True" Checked="True" OnCheckedChanged="rdbstaff_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdbworker" runat="server" Text="Worker" AutoPostBack="True" OnCheckedChanged="rdbworker_CheckedChanged" />
                        </td>
                   </tr>
            </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

                <div class="punishment_against">
                      <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlDivision" />
                     <asp:AsyncPostBackTrigger ControlID="ddlDepartment" />
                </Triggers>
                <ContentTemplate>
                  <table class="employee_table">                 
                        <tbody>
                    <tr>
                            <td>
                                Division
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDivision" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                   
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                       <tr>
                            <td>
                                Department
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                 <asp:DropDownList ID="ddlDepartment" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                                   
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
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
                     </ContentTemplate>
                </asp:UpdatePanel>
                </div>
                <div class="punishment_button_area" style="text-align:center">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                 <asp:Button ID="btnPreview" Class="emp_btn"  runat="server" Text="Preview" OnClick="btnPreview_Click"  />
                                <asp:Button ID="btnClose" Class="emp_btn" runat="server" Text="Close" PostBackUrl="~/default.aspx" />
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
