<%@ Page Title="" Language="C#" MasterPageFile="~/personnel_NestedMaster.Master" AutoEventWireup="true" CodeBehind="common_increment.aspx.cs" Inherits="SigmaERP.personnel.common_increment" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ClientIDMode="Static" ID="hfBasicPercentage" runat="server" />
     <asp:HiddenField ClientIDMode="Static" ID="hfOthersAllowance" runat="server" />
    <asp:HiddenField ClientIDMode="Static" ID="hfHouseRentPercentage" runat="server" />
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="com_incre_main_box">
        <div class="employee_box_header">
             <h2>Common Increment</h2>
         </div>
        <div class="employee_box_body">
            <div class="employee_box_content">
                <asp:UpdatePanel runat="server" id="up1" UpdateMode="Conditional">
                    <Triggers>

                    </Triggers>
                    <ContentTemplate>
               <div class="com_incre_left">
                   <table class="com_incre_table">
                        <tbody>
                    <tr>
                            <td>
                                <asp:CheckBox ID="chkGeneration" Class="" runat="server" Text="Generation" />
                            </td>
                     </tr>
                    <tr>
                        <td>
                        <asp:RadioButtonList ID="rbEmployeeType" runat="server">
                            <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
                            <asp:ListItem Value="1">Worker</asp:ListItem>
                            <asp:ListItem Value="2">Staff</asp:ListItem>
                            </asp:RadioButtonList>
                            </td>
                    </tr>
                 </table>
               </div> 
                <div class="com_incre_right">
                    <table class="com_incre_table">
                        <tbody>
                    <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                               
                     </tr>
                    <tr>
                            <td>
                                Month ID (mmmyy)
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtMonth" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" ></asp:TextBox>
                                
                                <asp:CalendarExtender ID="txtMonth_CalendarExtender" Format="MM-yyyy" runat="server" TargetControlID="txtMonth">
                                </asp:CalendarExtender>
                                
                            </td>
                    </tr>
                    <tr>
                            <td>
                                Percentage
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPercentage" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                
                            </td>
                    </tr>
                   <tr>
                            <td>Remarks :</td>
                            <td>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" ClientIDMode="Static" runat="server" Height="51px" Width="194px" CssClass="form-control text_box_width" TextMode="MultiLine"></asp:TextBox>
                            </td>
                    </tr>
                 </table>
               </div> 
                    <div class="com_incre_button">
                    <table class="promo_button_table">
                        <tbody>
                            <tr>
                                <th><asp:Button ID="btnUndo" CssClass="back_button" runat="server" Text="Undo" OnClick="btnUndo_Click" /></th>
                                <th><asp:Button ID="btnGenerate" CssClass="emp_btn" runat="server" Text="Generate" OnClientClick="return InputValidationBasket();" OnClick="btnGenerate_Click"/></th>
                               <th><a class="css_btn" href="../default.aspx" >Close</a></th>         
                         </tr>
                    </tbody>
                  </table>
                </div>
</ContentTemplate>
                </asp:UpdatePanel>

                </div>
        </div>
    </div>
    <script src="../scripts/jquery-2.0.0.min.js"></script>
    <script type="text/javascript">
        function InputValidationBasket()
        {
            try
            {
                if ($('#txtMonth').val().trim().length <5) {
                    showMessage("Please select or type the valid date of month","error");
                    $('#txtMonth').focus();
                    return false;
                }
                else if ($('#txtPercentage').val().trim().length == 0)
                {
                    showMessage("Please type percentage","error");
                    $('#txtPercentage').focus();
                    return false;
                }
                return true;
            }
            catch (exception)
            {
                showMessage(exception);
            }
        }

        function ClearInputBox()
        {
            try
            {
                $('#txtMonth').val('');
                $('#txtPercentage').val('');
                $('#txtRemarks').val('');
            }
            catch (exception)
            {
                showMessage(exception);
            }
        }
        
    </script>
</asp:Content>
