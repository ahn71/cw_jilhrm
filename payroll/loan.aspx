<%@ Page Title="Loan" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="loan.aspx.cs" Inherits="SigmaERP.payroll.loan" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <div class="salary_increment_main_box">
    	<div class="main_box_header">
            <h2>Loan</h2>
        </div>
    	<div class="main_box_body">
        	<div class="advance_main_box_content">
                <div class="advance_2_left_area">
                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="rbEmpType" />
                        <asp:AsyncPostBackTrigger ControlID="btnAdd" />
                        <asp:AsyncPostBackTrigger ControlID="btnClear" />
                        <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                     </Triggers>
                    <ContentTemplate>
                <div class="loan_top" style="border: medium none;">
                    <div class="loan_top_top">
                       
                       <center>
                        <asp:RadioButtonList ID="rbEmpType" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbEmpType_SelectedIndexChanged" AutoPostBack="True" >
                                        
                                    </asp:RadioButtonList></center>
                         <br /> <br />
                        <table class="loan_top_top_table">
                           
                            <tr>
                                <td colspan="2">Employee ID</td>
                                
                            </tr>
                            
                            <tr>
                                <td colspan="2"><asp:DropDownList ID="ddlEmpCardNo" runat="server" ClientIDMode="Static" Width="395px"  CssClass="form-control select_width" AutoPostBack="True" OnSelectedIndexChanged="ddlEmpCardNo_SelectedIndexChanged"></asp:DropDownList> </td>
                            </tr>
                        </table>

                          <table>
                             <tr>
                                <td> <asp:TextBox ID="txtFindEmployeeCardNo" runat="server" ClientIDMode="Static" PlaceHolder="Please Type Card Number For Find an Employee"  Width="310"  CssClass="form-control text_box_width" style=" text-align:center; margin-left: 33px;"></asp:TextBox></td>
                                <td>
                                    <asp:Button runat="server" ID="btnFind" Text="Find" Width="69px"  CssClass="next_button" OnClick="btnFind_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="loan_top_middle">
                        <table class="loan_top_middle_table">
                            <tr>
                                <td>Entry Date</td>
                                <td>Start Month(MM-yy)</td>
                                <td></td>
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="txtEntryDate" runat="server" ClientIDMode="Static" Width="160px"  CssClass="form-control text_box_width" ></asp:TextBox>
                                   
                                    <asp:CalendarExtender ID="txtEntryDate_CalendarExtender" Format="dd-MM-yyyy" runat="server" TargetControlID="txtEntryDate">
                                    </asp:CalendarExtender>
                                   
                                </td>
                                
                                <td><asp:TextBox ID="txtStartMonth" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_loan"></asp:TextBox>
                                   
                                    <asp:CalendarExtender ID="txtStartMonth_CalendarExtender" Format="MM-yyyy" runat="server" TargetControlID="txtStartMonth">
                                    </asp:CalendarExtender>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td>Advance Amount</td>
                                <td>No. of Installment</td>
                                
                            </tr>
                            <tr>
                                <td><asp:TextBox ID="txtAdvanceAmount" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_loan"></asp:TextBox></td>
                                <td><asp:TextBox ID="txtNoOfInstallment" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_loan"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td> <asp:Button ID="btnAdd" runat="server" Text="Add" ClientIDMode="Static" CssClass="next_button" Width="67px" OnClientClick="return InputValidationBasket();" OnClick="btnAdd_Click" Style="width: 67px; float: left; margin-left: 27px; margin-top: 5px;"/></td>
                            </tr>
                         </table>
                    </div>
                    <div class="loan_top_bottom">
                        <table class="show_division_table">
                        <tr>
                            <asp:GridView ID="gvInstallmentDetails" runat="server" AutoGenerateColumns="false" Width="400px" CssClass="loan_table" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" HeaderStyle-Height="22px" >
                                <Columns>
                                    <asp:BoundField DataField="Month" HeaderStyle-HorizontalAlign="Left" HeaderText="Pay Month" ItemStyle-Width="308px" ItemStyle-HorizontalAlign="left"/>
                                    <asp:BoundField DataField="Amount" HeaderStyle-HorizontalAlign=" right" HeaderText="Pay Amount" ItemStyle-Width="200px"  ItemStyle-HorizontalAlign="right"/>
                                </Columns>
                            </asp:GridView>
                        </tr>    
                    </table>
                        <br />
                        <div>
                            <table class="text_area_advance">
                                 <tr>
                                    <td>
                                        Remarks :
                                    </td>
                                     <td>
                                         <asp:TextBox ID="txtNotes" runat="server" Height="56px" Width="328px" TextMode="MultiLine"></asp:TextBox>
                                     </td>
                              
                                     </tr>
                            </table>
                        </div>
                    </div>
                     </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel runat="server" ID="up3" UpdateMode="Conditional">
                    <Triggers>

                    </Triggers>
                    <ContentTemplate>

                    
                    <div class="late_report_button">
                    <asp:Button ID="btnSave" runat="server" Text="Save" ClientIDMode="Static" CssClass="css_btn" OnClick="btnSave_Click"/>
                    <asp:Button ID="btnClear" runat="server" Text="Clear" ClientIDMode="Static" CssClass="css_btn" OnClick="btnClear_Click" />
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete"  ClientIDMode="Static" CssClass="css_btn" OnClick="btnDelete_Click"   />
                        </ContentTemplate>
                </asp:UpdatePanel>
                    </div>
                    
                </div>
            

            <div class="advance_2_right_area">
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSave" />
                    <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                </Triggers>
                <ContentTemplate >
                <asp:GridView ID="gvLoanInfo" runat="server" DataKeyNames="LoanId" HeaderStyle-BackColor="Black" AutoGenerateColumns="false" HeaderStyle-Height="30px" HeaderStyle-Font-Bold="false" HeaderStyle-ForeColor="White" OnRowCommand="gvLoanInfo_RowCommand">
                    <Columns>
                      <%--  <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Bind("LoanId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                         <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" Visible="true" itemstyle-horizontalalign="center" ItemStyle-Height="26px"    />
                        <asp:BoundField DataField="LoanAmount" HeaderText="Loan" Visible="true" itemstyle-horizontalalign="center" ItemStyle-Height="26px"   />
                         <asp:BoundField DataField="InstallmentNo" HeaderText="Ins.No." Visible="true" itemstyle-horizontalalign="center" ItemStyle-Height="26px"    />
                        <asp:BoundField DataField="InstallmentAmount" HeaderText="Ins.Amount" Visible="true"  itemstyle-horizontalalign="center" ItemStyle-Height="26px"    />
                         <asp:BoundField DataField="StartMonth" HeaderText="St Month" Visible="true" itemstyle-horizontalalign="center" ItemStyle-Height="26px"   />
                        <asp:BoundField DataField="EntryDate" HeaderText="Date" Visible="true" itemstyle-horizontalalign="center" ItemStyle-Height="26px"  />

                        <asp:BoundField DataField="EmpType" HeaderText="EmpType" Visible="true" itemstyle-horizontalalign="center" ItemStyle-Height="26px"  />

                         <asp:CommandField ShowSelectButton="true"/>

                    </Columns>
                     <SelectedRowStyle BackColor="Yellow" />
                </asp:GridView>
                 </ContentTemplate>
            </asp:UpdatePanel>

            </div>
                   
                </div>
                

                

              
           
        </div>
    
    <script src="../scripts/jquery-2.0.0.min.js"></script>
    <script type="text/javascript">
        function InputValidationBasket() {
            try {

                if ($('#txtEntryDate').val().trim().length == 0) {
                    showMessage('Please select entry date', 'error');
                    $('#txtEntryDate').focus(); return false;
                }

                if ($('#txtAdvanceAmount').val().trim().length == 0) {
                    showMessage('Please type advance amount', 'error');
                    $('#txtAdvanceAmount').focus(); return false;
                }
                if ($('#txtStartMonth').val().trim().length == 0) {
                    showMessage('Please select start month', 'error');
                    $('#txtStartMonth').focus(); return false;
                }

                if ($('#txtNoOfInstallment').val().trim().length == 0) {
                    showMessage('Please type number of installment', 'error');
                    $('#txtNoOfInstallment').focus(); return false;
                }

                return true;
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }
    </script>
</asp:Content>
