<%@ Page Title="" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="tiffin_generation.aspx.cs" Inherits="SigmaERP.payroll.tiffin_generation" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>   
     <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="main_box">
    	<div class="main_box_header">
            <h2>Daily Tiffin Bill Generation</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">

                <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                    <Triggers>

                    </Triggers>
                    <ContentTemplate>
                <div class="bonus_generation">
                     <table  class="bonus_generation_table">
                         <tr>
                             <td>
                                 Select EmpType
                             </td>
                             <td>
                                  <asp:RadioButtonList ID="rbEmpTypeList" runat="server" RepeatDirection="Horizontal" AutoPostBack="True"></asp:RadioButtonList> 
                             </td>
                         </tr>
                            <tr>
                                
                                <td>
                                    Select Date</td>
                                <td>
                                    <asp:TextBox CssClass="form-control text_box_width" ID="txtGenerateMonth" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtGenerateMonth_CalendarExtender" Format="dd-MM-yyyy" runat="server" TargetControlID="txtGenerateMonth">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>
                     </table>
                </div>
                <div class="payroll_generation_box3">
                  
                </div>
                <div class="payroll_generation_button">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold; width:139px; float:left">
                                            <asp:Label runat="server" ID="lblProcess" text="wait processing"></asp:Label>
                                        <img style="width:26px;height:24px;cursor:pointer; margin-right:-56px" src="/images/wait.gif"  />  
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                     <asp:Button ID="btnGeneration" runat="server" CssClass="css_btn" ClientIDMode="Static" Text="Generation" OnClientClick="return InputValidationBasket();" OnClick="btnGeneration_Click" />
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                </div>
                <div class="payroll_generation_bottom">
                    <asp:ListBox ID="lbProcessingStatus" Width="600" Height="120" runat="server"></asp:ListBox>
                </div>
 </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
   
        </div>

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
