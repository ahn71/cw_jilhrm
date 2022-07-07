<%@ Page Title="Bonus Setup" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="bonus_setup.aspx.cs" Inherits="SigmaERP.payroll.bonus_setup" %>
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
        #ContentPlaceHolder1_ContentPlaceHolder1_divBonusList th {
            text-align:center;
        }
         #ContentPlaceHolder1_ContentPlaceHolder1_divBonusList th:first-child,td:first-child {
            text-align:left;
            padding-left:3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="row">
            <div class="col-md-12">
                <div class="ds_nagevation_bar">
                   
                           <ul>
                               <li><a href="/default.aspx">Dasboard</a></li>
                               <li> <a class="seperator" href="#">/</a></li>
                               <li> <a href="/payroll_default.aspx">Payroll</a></li>
                               <li> <a class="seperator" href="#">/</a></li>
                               <li> <a href="/payroll/bonus_index.aspx" >Bouns</a></li>
                               <li> <a class="seperator" href="#">/</a></li>
                               <li> <a href="#" class="ds_negevation_inactive Pactive">Bouns Setup</a></li>
                           </ul>               
                    
                </div>
             </div>
        </div>
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>


    <asp:HiddenField ID="hfSaveStatus" runat="server" ClientIDMode="Static" Value="Save" />
    <asp:HiddenField ID="hfBId" runat="server" ClientIDMode="Static" Value="0" />
    <asp:HiddenField ID="hfUpdateAction" runat="server" ClientIDMode="Static" Value="False" />
    <asp:HiddenField ID="hfWriteAction" runat="server" ClientIDMode="Static" Value="False" />

    <div class="main_box Mbox">
    	<div class="main_box_header PBoxheader">
            <h2>Bonus Setup</h2>
        </div>
    	<div class="main_box_body Pbody">
        	<div class="main_box_content">

                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" />
                     <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" />
                        <asp:AsyncPostBackTrigger ControlID="rblGenerateByReligion" />
                    </Triggers>
                    <ContentTemplate>

                        

                        <div class="input_division_info">
                    <table class="employee_table">
                        <tr>
                            <td>
                                Company Name<span class="requerd1">*</span>
                            </td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlCompanyName" CssClass="form-control select_width" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged" AutoPostBack="True" ></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Bonus Name<span class="requerd1">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtBonusName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" autocomplete="off"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                        <td style="vertical-align:top">
                            Bonus Generate By Religion <span class="requerd1">*</span>
                        </td>
                            <td style="vertical-align:top">:</td>
                        <td>
                            <asp:RadioButtonList ID="rblGenerateByReligion" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblGenerateByReligion_SelectedIndexChanged" RepeatColumns="1" RepeatLayout="Flow" ></asp:RadioButtonList>
                        </td>
                    </tr>
                        <tr>
                            <td>
                                Payment Date<span class="requerd1">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPaymentDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" autocomplete="off"></asp:TextBox>
                                <asp:CalendarExtender ID="txtPaymentDate_CalendarExtender" Format="dd-MM-yyyy" runat="server" TargetControlID="txtPaymentDate">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Status<span class="requerd1">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                                    <asp:ListItem Value="1">Active</asp:ListItem>
                                    <asp:ListItem Value="0">Inactive</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Config Date<span class="requerd1">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>

                                <asp:TextBox ID="txtConfigDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" autocomplete="off"></asp:TextBox>
                                <asp:CalendarExtender ID="txtConfigDate_CalendarExtender" runat="server" TargetControlID="txtConfigDate" Format="dd-MM-yyyy">
                                </asp:CalendarExtender>
                            </td>
                            
                        </tr>
                        <tr>
                            <td>Calulation Date<span class="requerd1">*</span></td>
                            <td>:</td>
                            <td>

                                <asp:TextBox ID="txtCalculationDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" autocomplete="off"></asp:TextBox>
                                
                                <asp:CalendarExtender ID="txtCalculationDate_CalendarExtender" runat="server" TargetControlID="txtCalculationDate" Format="dd-MM-yyyy">
                                </asp:CalendarExtender>
                                
                            </td>
                        </tr>
                        
                    </table>
                </div>

                <div class="button_area Rbutton_area">
                   <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Pbutton" OnClientClick="return InputValidationBasket();" OnClick="btnSave_Click" ClientIDMode="Static" />
                   <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="Pbutton" OnClick="btnClear_Click" />
                   <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/payroll_default.aspx" CssClass="Pbutton" />           
                </div>
 
                    <br />
                        <div runat="server" id="divBonusList" class="show_division_info">
                  <asp:GridView ID="gvBonusSetupList" runat="server" DataKeyNames="BId,CompanyId,Status,RId" Width="100%" HeaderStyle-BackColor="#FFA500" HeaderStyle-Height="28px" HeaderStyle-ForeColor="White" AutoGenerateColumns="false" AllowPaging="true" OnRowCommand="gvBonusSetupList_RowCommand" OnRowDeleting="gvBonusSetupList_RowDeleting" OnRowDataBound="gvBonusSetupList_RowDataBound"  >
                      <Columns>
                          <asp:BoundField DataField="BonusName" HeaderText="Bonus Name"  HeaderStyle-HorizontalAlign="Left"/>
                          <asp:BoundField DataField="Religion" HeaderText="Religion"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                          <asp:BoundField DataField="PaymentDate" HeaderText="Payment Date" ItemStyle-HorizontalAlign="Center" />
                          <asp:BoundField DataField ="ConfigDate" HeaderText="Config Date" ItemStyle-HorizontalAlign="Center" />
                          <asp:BoundField DataField="CalculationDate" HeaderText="CalculationDate" ItemStyle-HorizontalAlign="Center" />

                          <asp:TemplateField HeaderText="Status">
                              <ItemTemplate>
                                  <asp:CheckBox ID="chkStatus" runat="server" Checked='<%#bool.Parse(Eval("Status").ToString())%>' />
                              </ItemTemplate>
                              <ItemStyle HorizontalAlign="Center" />
                          </asp:TemplateField>
                              
                          <asp:TemplateField HeaderText="Edit">
                              <ItemTemplate>
                                  <asp:Button runat="server" ID="btnAlter" Text="Edit" Width="60px" CommandName="Alter" CommandArgument='<%#((GridViewRow)Container).RowIndex %>' ForeColor="Green" Font-Bold="true" />
                              </ItemTemplate>
                               <ItemStyle HorizontalAlign="Center" />
                          </asp:TemplateField>

                          <asp:TemplateField HeaderText="Delete">
                              <ItemTemplate>
                                  <asp:Button runat="server" ID="btnDelete" Text="Delete" Width="60px" CommandName="Delete" CommandArgument='<%#((GridViewRow)Container).RowIndex %>' ForeColor="Red" Font-Bold="true" OnClientClick="return confirm('Do you want to delete ?')" />
                              </ItemTemplate>
                              <ItemStyle HorizontalAlign="Center" />
                          </asp:TemplateField>

                      </Columns>
                  </asp:GridView>
                            </div>
                </ContentTemplate>
            </asp:UpdatePanel> 

            </div>

          
        </div>
    </div>

    <script src="../scripts/jquery-2.0.0.min.js"></script>

    <script type="text/javascript">
        $(function () {
            $("#btnDelete").removeClass("css_btn");
            $("#btnDelete").hide();
        })
        function InputValidationBasket() {
            try {
                if ($('#txtBonusName').val().trim().length == 0) {
                    showMessage('Please type bunus name', 'error');
                    $('#txtBonusName').focus(); return false;
                }
                if ($('#txtPaymentDate').val().trim().length == 0) {
                    showMessage('Please select patment date', 'error');
                    $('#txtPaymentDate').focus(); return false;
                }

                if ($('#txtConfigDate').val().trim().length == 0) {
                    showMessage('Please select config date', 'error');
                    $('#txtConfigDate').focus(); return false;
                }

                if ($('#txtCalculationDate').val().trim().length == 0) {
                    showMessage('Please select calculation date', 'error');
                    $('#txtCalculationDate').focus(); return false;
                }
                return true;
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }

        function ClearInputBox() {
            try {
                $('#txtBonusName').val('');
                $('#txtPaymentDate').val('');
                $('#txtConfigDate').val('');
                $('#txtCalculationDate').val('');
                $('#btnSave').val('Save');
                $('#hfSaveStatus').val('Save');

                if ($('#hfWriteAction').val() == 'True') {
                    $('#btnSave').addClass("css_btn");
                    $('#btnSave').prop("disabled", false);

                   
                }
                $('#txtBonusName').prop('disabled', false);
               
                $('#btnDelete').removeClass("css_btn");
                $("#btnDelete").hide();
                $('#hfWriteAction').val('False');
                $('#hfBId').val('0');
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }

        function yesDelete()
        {
            if ($('#hfWriteAction').val() == 'True') {
                var IsDelete = confirm('Are you sure, you want to delete the record?');
                if (IsDelete) return true;
                else return false;
            }
            else return fals;
        }

        function editBonus(getId)
        {
            try
            {
                $("#btnDelete").addClass("css_btn");
                $("#btnDelete").show();

                $('#txtBonusName').prop('disabled', true);
               
               
                $('#txtBonusName').val($('#r_' + getId + ' td:first-child').html());
                $('#txtPaymentDate').val($('#r_' + getId + ' td:nth-child(2)').html());
                $('#txtConfigDate').val($('#r_' + getId + ' td:nth-child(3)').html());
                $('#txtCalculationDate').val($('#r_' + getId + ' td:nth-child(4)').html());
                $('#btnSave').val('Update');

                
               

                if ($('#hfUpdateAction').val() == 'True') {
                    $('#btnSave').addClass("css_btn");
                    $('#btnSave').prop("disabled", false);
                }
                else {
                   

                    $('#btnSave').removeClass("css_btn");
                    $('#btnSave').prop("disabled", true);
                }


                $('#hfBId').val(getId);
                $('#hfSaveStatus').val('Update');
                if ($('#r_' + getId + ' td:nth-child(5)').html() == 'True') {

                    document.getElementById('rblStatus').rows[0].cells[0].childNodes[0].checked = true; 
                    document.getElementById('rblStatus').rows[0].cells[1].childNodes[0].checked = false;
                }
                else {

                    document.getElementById('rblStatus').rows[0].cells[0].childNodes[0].checked = false; 
                    document.getElementById('rblStatus').rows[0].cells[1].childNodes[0].checked = true; 
                }

               
                
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }
    </script>
</asp:Content>
