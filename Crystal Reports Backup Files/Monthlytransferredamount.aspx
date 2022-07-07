<%@ Page Title="Monthly Transferred Amount" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="Monthlytransferredamount.aspx.cs" Inherits="SigmaERP.payroll.Monthlytransferredamount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
           
        
        </Triggers>
        <ContentTemplate>

            <div class="main_box">
                <div class="main_box_header">
                    <h2>Monthly Transferred Amount</h2>
                </div>
                <div class="main_box_body">
                    <div class="main_box_content">
                        <div class="overtime_report_box3">
                            <table class="monthly_attend_table">
                                <tr>
                                    <td>Month ID
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMonthID" ClientIDMode="Static" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlMonthID" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                
                            </table>
                        </div>
                        
                        <div class="job_card_button_area">
                            <asp:Button ID="btnPreview" CssClass="css_btn" ValidationGroup="save" runat="server" Text="Preview" OnClick="btnPreview_Click"  />

                            <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
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
