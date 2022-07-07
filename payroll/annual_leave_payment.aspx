<%@ Page Title="Annual Leave Payment" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="annual_leave_payment.aspx.cs" Inherits="SigmaERP.payroll.annual_leave_payment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>

    <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlDivision" />
        </Triggers>
        <ContentTemplate>
              <div class="main_box1">
                        <div class="main_content_box_header">
                         <h2>Annual Leave Payment</h2>
                     </div>
     
                     <div class="main_content_box_body">
                <div class="main_box_content">
                <div class="job_card_box1">
                    <table class="job_card_table">
                        <tr>
                            <td>
                                <asp:RadioButton ID="RadioButton1" runat="server" Text="Month ID" Checked="True"></asp:RadioButton>
                                
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rdoCardOrDivision" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" RepeatLayout="Flow" Width="100%" >
                                    
                                    <asp:ListItem Selected="True">Division</asp:ListItem>
                                </asp:RadioButtonList>                            
                            </td>
                        </tr>
                       
                    </table>

                    

                </div>
                <div class="job_card_box2">
                    <table>
                        <tr>
                            <td>Month ID :</td>
                            <td> 

                                <asp:DropDownList ID="ddlMonthID" runat="server" CssClass="form-control select_width">
                                    
                                </asp:DropDownList>
                                                   <asp:RequiredFieldValidator InitialValue="" ID="RequiredFieldValidator13" Display="Dynamic" 
    ValidationGroup="save" runat="server" ControlToValidate="ddlMonthID"
    Text="*" ErrorMessage="ErrorMessage"></asp:RequiredFieldValidator>

                            </td>
                            <td>
                                <asp:Label ID="lblTitle" runat="server">Division</asp:Label>
                                &nbsp;</td>
                            <td> 
                                
                                <asp:DropDownList ID="ddlDivision" runat="server" AppendDataBoundItems="True" AutoPostBack="true" ClientIDMode="Static" CssClass="form-control select_width" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged" >
                                </asp:DropDownList>
                            </td>
<%--                            <asp:RequiredFieldValidator InitialValue="" ID="RequiredFieldValidator1" Display="Dynamic" 
    ValidationGroup="save" runat="server" ControlToValidate="txtCardNo"
    Text="*" ErrorMessage="ErrorMessage"></asp:RequiredFieldValidator>--%>
                        </tr>
                        
                    </table>
                    <br />
                    <table>
                         <tr>
                            <td>
                                Type
                            </td>
                             <td>:</td> 
                             <td>
                                 <asp:RadioButtonList ID="rdoEmpType" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"  >
                        <asp:ListItem>Worker</asp:ListItem>
                        <asp:ListItem>Staff</asp:ListItem>
                        </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="daily_absence_report_box3">
                    <p runat="server" id="loadingImg" style="height:2px; margin:0px">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                            <ProgressTemplate>
                                <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold;float:left"><p>Please Wait</p> </span> 
                                <img style="width:26px;height:26px;cursor:pointer; float:left; margin-left:15px" src="/images/wait.gif"  /> 
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </p>
                </div>
                <div class="daily_absence_report_box4">
                    <div class="daily_absence_report_left">
                        <p>Available Departments</p>
                        <asp:ListBox ID="lstEmployees" Width="260" Height="146" runat="server" SelectionMode="Multiple" AutoPostBack="True"></asp:ListBox>
                    </div>
                    <div class="daily_absence_report_middle">

                        <asp:Button ID="btnadditem" CssClass="next_button" runat="server" Text=">" />
                        <br />
                        <asp:Button ID="btnaddall" CssClass="next_button" runat="server" Text=">>"/>
                        <br />
                        <asp:Button ID="btnremoveitem" CssClass="next_button" runat="server" Text="<"/>
                        <br />
                        <asp:Button ID="btnremoveall" CssClass="next_button" runat="server" Text="<<"/>
                    </div>
                    <div class="daily_absence_report_right">
                        <p>Selected Department/s</p>
                        <asp:ListBox ID="lstSelectedEmployees" Width="260" Height="146" runat="server" SelectionMode="Multiple"></asp:ListBox>

                    </div>
                </div>

                <div class="job_card_button_area">
                    <asp:Button ID="btnPreview" CssClass="css_btn"  ValidationGroup="save" runat="server" Text="Preview" />
                    &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                </div>
            </div>
                                     
                  </div>
                  
        </div>
             </ContentTemplate>
    </asp:UpdatePanel>

     <script type="text/javascript">
         function InputValidationBasket() {
             try {
                 if ($('#txtGenerateMonth').val().trim().length <= 4) {
                     showMessage('Please select salary month', 'error');
                     $('#txtGenerateMonth').focus(); return false;
                 }
                 return true;
             }
             catch (exception) {
                 showMessage(exception, error)
             }
         }

         function CloseWindowt() {
             window.close();
         }

         function goToNewTabandWindow(url) {
             window.open(url);

         }
    </script>
</asp:Content>
