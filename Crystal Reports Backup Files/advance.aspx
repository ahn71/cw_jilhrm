<%@ Page Title="Advance" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="advance.aspx.cs" Inherits="SigmaERP.payroll.advance" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../scripts/jquery-1.8.2.js"></script>
        <script type="text/javascript">

            var oldgridcolor;
            function SetMouseOver(element) {
                oldgridcolor = element.style.backgroundColor;
                element.style.backgroundColor = '#ffeb95';
                element.style.cursor = 'pointer';
                // element.style.textDecoration = 'underline';
            }
            function SetMouseOut(element) {
                element.style.backgroundColor = oldgridcolor;
                // element.style.textDecoration = 'none';
            }
     </script>
    <style>
        #ContentPlaceHolder1_ContentPlaceHolder1_gvAdvanceInfo th,td {
            text-align:center;
        }
         #ContentPlaceHolder1_ContentPlaceHolder1_gvAdvanceInfo th:nth-child(3),td:nth-child(3) {
            text-align:left;
            padding-left:3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dasboard</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="/payroll_default.aspx">Payroll</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="/payroll/advance_index.aspx">Advance</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="#" class="ds_negevation_inactive Pactive">Advance Entry Panel</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="uplMessage" runat="server">
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
     <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Advance Info Reports</h2>
        </div>
        <div class="main_box_body Pbody">
            <div class="main_box_content" style="overflow:hidden">         
                  
                             <div style="float: left; width: 35%; border-right: 1px solid #ddd; margin-right: 1%; padding-right: 1%;">    
                                   <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                        <Triggers>                           
                            <asp:AsyncPostBackTrigger ControlID="ddlShiftList" />
                            <asp:AsyncPostBackTrigger ControlID="btnAdd" />
                            <asp:AsyncPostBackTrigger ControlID="btnClear" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                            <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                            <%--<asp:AsyncPostBackTrigger ControlID="btnComplain" />--%>
                        </Triggers>
                        <ContentTemplate>
                                               
                                    <table style="width:100%;">
                                       <tr>
                                            <td style="text-align: left;">Company <span class="requerd1">*</span></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlCompanyList" runat="server" AutoPostBack="True" ClientIDMode="Static" CssClass="form-control select_width" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:DropDownList Visible="false" ID="ddlShiftList" runat="server" AutoPostBack="True" ClientIDMode="Static" CssClass="form-control select_width" OnSelectedIndexChanged="ddlShiftList_SelectedIndexChanged" Width="160px">
                                                </asp:DropDownList>
                                            </td>
                                            <tr>
                                                <td style="text-align: left;">Employee ID <span class="requerd1">*</span></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlEmpCardNo" runat="server" AutoPostBack="True" ClientIDMode="Static" CssClass="form-control select_width" OnSelectedIndexChanged="ddlEmpCardNo_SelectedIndexChanged" Width="395px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </tr>
                                    </table>
                                    <table style="width:98%;">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtFindEmployeeCardNo" runat="server" ClientIDMode="Static" PlaceHolder="Please Type Card Number For Find an Employee" Width="315" CssClass="form-control text_box_width" Style="text-align: center; margin-left: 4px;"></asp:TextBox></td>
                                            <td>
                                                <asp:Button runat="server" ID="btnFind" Text="Find" Width="69px"  CssClass="Pbutton" OnClick="btnFind_Click" />
                                            </td>
                                        </tr>
                                    </table>                  
                                    <table  style="width:98%;">
                                        <tr>
                                            <td>Take Date<span class="requerd1">*</span></td>
                                            <td>Start Month(MM-yyyy)<span class="requerd1">*</span></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtEntryDate" runat="server" ClientIDMode="Static" Width="160px" CssClass="form-control text_box_width" style=" text-align:center;color:green"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtEntryDate_CalendarExtender" Format="dd-MM-yyyy" runat="server" TargetControlID="txtEntryDate">
                                                </asp:CalendarExtender>
                                            </td>

                                            <td>
                                                <asp:TextBox ID="txtStartMonth" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_loan" style=" text-align:center;color:red" ></asp:TextBox>
                                                <asp:CalendarExtender ID="txtStartMonth_CalendarExtender" Format="MM-yyyy" runat="server" TargetControlID="txtStartMonth">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Advance Amount<span class="requerd1">*</span></td>
                                            <td>No. of Installment<span class="requerd1">*</span></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtAdvanceAmount" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_loan"></asp:TextBox></td>
                                            <td>
                                                <asp:TextBox ID="txtNoOfInstallment" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_loan" style="width:66%;float:left" ></asp:TextBox>
                                                <asp:Button ID="btnAdd" runat="server" Text="Add" ClientIDMode="Static" CssClass="Pbutton"  OnClientClick="return InputValidationBasket();" OnClick="btnAdd_Click" />
                                                </td>
                                        </tr>                                    
                                    </table>                               
                                <div class="loan_top_bottom">
                                    <table class="show_division_table">
                                        <tr>
                                            <asp:GridView ID="gvInstallmentDetails" HeaderStyle-BackColor="#ffa500" runat="server" AutoGenerateColumns="false" Width="98%" CssClass="loan_table" HeaderStyle-ForeColor="White" HeaderStyle-Height="22px">
                                                <Columns>
                                                    <%--<asp:TemplateField ItemStyle-CssClass="center"  HeaderText="No">
                                                        <ItemTemplate>
                                                            <label><%# Container.DataItemIndex + 1 %></label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>--%>
                                                    <asp:BoundField DataField="Month" HeaderStyle-HorizontalAlign="Left" HeaderText="Pay Month" ItemStyle-Width="308px" ItemStyle-HorizontalAlign="left" />
                                                    <asp:BoundField DataField="Amount" HeaderStyle-HorizontalAlign=" right" HeaderText="Pay Amount" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" />
                                                </Columns>
                                            </asp:GridView>
                                        </tr>
                                    </table>
                                    <br />
                                    <div>
                                        <table class="text_area_advance">
                                            <tr>
                                                <td>Remarks :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotes" runat="server" Height="56px" Width="328px" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                       <br />
                                        <div style="width:313px;margin:0 auto;">
                                         <asp:Button ID="btnSave" runat="server" Text="Save" ClientIDMode="Static" CssClass="Pbutton" OnClick="btnSave_Click" />
                                          <asp:Button ID="btnClear" runat="server" Text="Clear" ClientIDMode="Static" CssClass="Pbutton" OnClick="btnClear_Click" />
                                        <asp:Button ID="btnClose" runat="server" Text="Close" PostBackUrl="~/payroll_default.aspx" ClientIDMode="Static" CssClass="Pbutton" />
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" ClientIDMode="Static" CssClass="Pbutton" OnClick="btnDelete_Click" />
                                        <asp:Button ID="btnComplain" Visible="false" runat="server" Text="Complain" ClientIDMode="Static" CssClass="Pbutton" OnClick="btnComplain_Click" />
                                        </div>
                                    </div>
                                </div>


                              </ContentTemplate>
                    </asp:UpdatePanel>        

                <div style="float:right;width:63%;">

                <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Always" >
                    <ContentTemplate>

                   
                          
                            
                                <asp:GridView ID="gvAdvanceInfo" runat="server" DataKeyNames="AdvanceId" Width="100%" HeaderStyle-BackColor="#ffa500" AutoGenerateColumns="false" HeaderStyle-Height="30px" HeaderStyle-Font-Bold="false" HeaderStyle-ForeColor="White" AllowPaging="True" OnPageIndexChanging="gvAdvanceInfo_PageIndexChanging" PageSize="25" OnRowCommand="gvAdvanceInfo_RowCommand" OnRowDataBound="gvAdvanceInfo_RowDataBound">
                            <PagerStyle CssClass="gridview" Height="20px" />
                            <Columns>
                                <%-- <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Bind("AdvanceId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                                 <asp:TemplateField ItemStyle-CssClass="center"  HeaderText="SL">
                                                        <ItemTemplate>
                                                            <label><%# Container.DataItemIndex + 1 %></label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                <asp:BoundField DataField="AdvanceId" HeaderText="Advance Id" Visible="false" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="center" ItemStyle-Height="15px" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" Visible="true" ItemStyle-HorizontalAlign="center" ItemStyle-Height="26px" />
                                 <asp:BoundField DataField="EmpName" HeaderText="Name" Visible="true" ItemStyle-HorizontalAlign="center" ItemStyle-Height="26px" />
                                <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance" Visible="true" ItemStyle-HorizontalAlign="center" ItemStyle-Height="26px" />
                                <asp:BoundField DataField="InstallmentNo" HeaderText="Ins.No." Visible="true" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="Red" ItemStyle-HorizontalAlign="center" ItemStyle-Height="26px" />
                                <asp:BoundField DataField="InstallmentAmount" HeaderText="Ins.Amount" Visible="true" ItemStyle-HorizontalAlign="center" ItemStyle-Height="26px" />
                                <asp:BoundField DataField="StartMonth" HeaderText="St Month" Visible="true" ItemStyle-HorizontalAlign="center" ItemStyle-Height="26px" />
                                <asp:BoundField DataField="EntryDate" HeaderText="Date" Visible="true" ItemStyle-HorizontalAlign="center" ItemStyle-Height="26px" />
                                <asp:BoundField DataField="EmpType" HeaderText="EmpType" Visible="true" ItemStyle-HorizontalAlign="center" ItemStyle-Height="26px" />

                                <%-- <asp:ButtonField CommandName="View" HeaderText="Select" ButtonType="Button" Text="    Select   " itemstyle-horizontalalign="center" ItemStyle-Width="50px" ItemStyle-ForeColor="Green" ItemStyle-Font-Bold="true" />--%>
                                <asp:CommandField ShowSelectButton="true" />

                                <%-- <asp:TemplateField HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            Delete
                                        </HeaderTemplate>
                                        <ItemTemplate>

                                            <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete" ClientIDMode="Static"
                                                OnClientClick="return confirm('Are you sure, you want to delete the record?')"
                                                CommandArgument='<%# Eval("AdvanceId") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                            </Columns>
                            <SelectedRowStyle BackColor="Yellow" />
                        </asp:GridView>
                            
                                   
                 </ContentTemplate>
                </asp:UpdatePanel>
                     </div>                
               </div>

        </div>
    </div>
    
    <script src="../scripts/jquery-2.0.0.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
     
            $("#ddlEmpCardNo").select2();
        });

        function load() {
            $("#ddlEmpCardNo").select2();
        }
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
