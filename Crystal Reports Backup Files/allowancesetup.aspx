<%@ Page Title="Stamp Deduction" Language="C#" MasterPageFile="~/hrd_nested.master" AutoEventWireup="true" CodeBehind="allowancesetup.aspx.cs" Inherits="SigmaERP.personnel.allowancesetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <%-- <link href="../style/oitlStyle.css" rel="stylesheet" />
    <link href="../style/reg_style.css" rel="stylesheet" />--%>
    <style>
        #ContentPlaceHolder1_MainContent_gvAllownceList th {
            text-align:center;
        }
        .show_division_info{
            width:35% !important;
        }
        .Rall{
            width:37% !important;
        }
        .Rbutton_area{
            width:320px!important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row Rrow">
                  <div class="col-md-12 ds_nagevation_bar">
               <div style="margin-top: 5px">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                       <li> <a href="#">/</a></li>
                       <li> <a href="/hrd_default.aspx">Settings</a></li>
                       <li> <a href="#">/</a></li>
                       <li> <a href="#" class="ds_negevation_inactive Ractive">Stamp Deduction</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hfSaveStatus" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfAllowanceId" ClientIDMode="Static" runat="server" Value="0" />
    <asp:HiddenField ID="upSave" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upupdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="updelete" runat="server" ClientIDMode="Static" />

    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="main_box RBox">
        <div class="main_box_header RBoxheader">
            <h2>Stamp Deduction Panel</h2>
        </div>
        <div class="main_box_body Rbody">

            <div class="main_box_content">
                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <%-- <asp:AsyncPostBackTrigger ControlID="btnSave" />--%>
                        <asp:AsyncPostBackTrigger ControlID="btnClear" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="input_division_info Rall">
                           
                            <table class="division_table">

                                <tr runat="server" visible="false">
                                    <td>Basic(%) </td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtBasicFind" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="320px"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr runat="server" visible="false">
                                    <td>Medical(%)</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtMedical" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="320px"></asp:TextBox>
                                    </td>
                                </tr>

                                 <tr runat="server" visible="false">
                                    <td>Conveyance(%)</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtConveyance" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="320px"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr runat="server" visible="false">
                                    <td>Food</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtFood" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="320px"></asp:TextBox>
                                    </td>
                                </tr>

                               
                                <tr runat="server" visible="false">
                                    <td>Hose Rent</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtHouseRent" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="320px" Enabled="False">0.0</asp:TextBox>
                                    </td>
                                </tr>

                            
                                 <tr>
                                    <td>Stamp Deduct <span class="requerd1">*</span></td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtStampDeduct" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Width="320px"></asp:TextBox>
                                    </td>
                                </tr>
                       
                            </table>
                            
                        </div>

                
                        <div>

                            <div style="margin: 5px; float: left; width: 232px;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>

                                        <span style="font-family: 'Times New Roman'; font-size: 20px; color: green; font-weight: bold; float: left; height: 48px;">wait processing
                                        <img style="width: 26px; height: 26px; cursor: pointer; float: left" src="/images/wait.gif" />
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>

                            <div class="button_area Rbutton_area">
                               <a href="#" onclick="window.history.back()" class="Rbutton">Back</a>
                               <asp:Button ID="btnSave" ClientIDMode="Static" CssClass="Rbutton" runat="server" Text="Save" OnClientClick="return InputValidationBasket();" OnClick="btnSave_Click" />
                               <asp:Button ID="btnClear" ClientIDMode="Static" CssClass="Rbutton" runat="server" Text="Clear" OnClick="btnClear_Click" />
                               <asp:Button ID="btnClose" runat="server" ClientIDMode="Static" CssClass="Rbutton" OnClientClick="ClearInputBox();" PostBackUrl="~/hrd_default.aspx" Text="Close" />
                            </div>
                           
                        </div>
                        <div runat="server" id="divAllowanceType" class="show_division_info">
<asp:GridView ID="gvAllownceList" runat="server" DataKeyNames="AllownceId"  HeaderStyle-ForeColor="White" AutoGenerateColumns="False"  Width="100%" OnRowCommand="gvAllownceList_RowCommand" OnRowDataBound="gvAllownceList_RowDataBound">
                             <RowStyle HorizontalAlign="Center" />                              
                             <Columns>
                                 <asp:BoundField Visible="false" DataField="BasicAllowance" HeaderText="Basic(%)"  />
                                 <asp:BoundField Visible="false" DataField="MedicalAllownce" HeaderText="Medical(%)"  />
                                 <asp:BoundField Visible="false" DataField="FoodAllownce" HeaderText="Food" />
                                 <asp:BoundField Visible="false" DataField="ConvenceAllownce" HeaderText="Convance(%)"/>
                                 <asp:BoundField Visible="false" DataField="HouseRent" HeaderText="House Rent"  />
                                 <asp:BoundField DataField="StampDeduct" HeaderText="Stamp Deduction" />
                                 <asp:BoundField Visible="false" DataField="Year" HeaderText="Year"/>
                                  <asp:TemplateField HeaderText="Edit" ItemStyle-Width="100px">
                                     <ItemTemplate>
                                         <asp:Button ID="btnAlter" runat="server" ControlStyle-CssClass="btnForAlterInGV" Text="Edit" CommandName="Alter" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                     </ItemTemplate>
                                 </asp:TemplateField>           

                             </Columns>
                             <HeaderStyle BackColor="#0057AE" Height="28px" />
                         </asp:GridView>
                       </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                </div>  
        </div>
    </div>


    <script type="text/javascript">

        function InputValidationBasket() {
            try {

                if ($('#txtMedical').val().trim().length == 0) {
                    showMessage('Please type medical allowance amount', 'error');
                    $('#txtMedical').focus(); return false;
                }
                if ($('#txtFood').val().trim().length == 0) {
                    showMessage('Please type food allowance amount', 'error');
                    $('#txtFood').focus(); return false;
                }
                if ($('#txtConveyance').val().trim().length == 0) {
                    showMessage('Please type convenyance allowance amount', 'error');
                    $('#txtConveyance').focus(); return false;
                }
                if ($('#txtHouseRent').val().trim().length == 0) {
                    showMessage('Please type house rent allowance in percentage', 'error');
                    $('#txtHouseRent').focus(); return false;
                }
                if ($('#txtBasicFind').val().trim().length == 0) {
                    showMessage('Please type basic  in percentage', 'error');
                    $('#txtHouseRent').focus(); return false;
                }
                if ($('#txtOthers').val().trim().length == 0) {
                    showMessage('Please type others allowance in percentage', 'error');
                    $('#txtHouseRent').focus(); return false;
                }
                if ($('#txtStampDeduct').val().trim().length == 0) {
                    showMessage('Please type StampDeduct Amount', 'error');
                    $('#txtStampDeduct').focus(); return false;
                }

                if ($('#txtLunchAllowance').val().trim().length == 0)
                {
                    showMessage('Please type lunch Amount', 'error');
                    $('#txtLunchAllowance').focus(); return false;
                }

            }
            catch (exception) {
                showMessage(exception, error)
            }
        }
        function ClearInputBox() {
            try {
                //if ($('#upSave').val() == '0') {

                //    $('#btnSave').removeClass('css_btn');
                //    $('#btnSave').attr('disabled', 'disabled');
                //}
                //else {
                //    $('#btnSave').addClass('css_btn');
                //    $('#btnSave').removeAttr('disabled');
                //}

                $('#txtBasicFind').val('');
                $('#txtMedical').val('');
                $('#txtFood').val('');
                $('#txtConveyance').val('');
                $('#txtHouseRent').val('0.0');
                $('#txtOthers').val('');
                
                //$('#hfSaveStatus').val('Save');
                $('#btnSave').val('Save');
                $('#hfAllowanceId').val('');
                $('#txtStampDeduct').val('');               
                
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }
        function test()
        {
            $('#btnSave').val('');
        }
        function editAllowanceType(getId)
        {
            $('#hfAllowanceId').val(getId);
            $('#txtBasicFind').val($('#r_' + getId + ' td:first-child').html());
            $('#txtMedical').val($('#r_' + getId + ' td:nth-child(2)').html());
           
            $('#txtConveyance').val($('#r_' + getId + ' td:nth-child(3)').html());
            $('#txtFood').val($('#r_' + getId + ' td:nth-child(4)').html());

            $('#txtHouseRent').val($('#r_' + getId + ' td:nth-child(5)').html());
           
            $('#txtStampDeduct').val($('#r_' + getId + ' td:nth-child(6)').html());
            

            $('#hfSaveStatus').val('Update');
            if ($('#upupdate').val() == '1') {
                $('#btnSave').val('Update');
                $('#btnSave').addClass('css_btn');
                $('#btnSave').removeAttr('disabled');
            }
            else {
                $('#btnSave').val('Update');
                $('#btnSave').removeClass('css_btn');
                $('#btnSave').attr('disabled', 'disabled');
            }
            
        }

    </script>
</asp:Content>
