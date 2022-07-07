<%@ Page Title="Floor Configuraion" Language="C#" MasterPageFile="~/hrd_nested.master" AutoEventWireup="true" CodeBehind="floorConfig.aspx.cs" Inherits="SigmaERP.hrd.floorConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #ContentPlaceHolder1_MainContent_gvFloorList th, td {
            text-align:center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row Rrow">
        <div class="col-md-12 ds_nagevation_bar">
            <div style="margin-top: 5px">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li><a href="#">/</a></li>
                    <li><a href="/hrd_default.aspx">Settings</a></li>
                    <li><a href="#">/</a></li>
                    <li><a href="#" class="ds_negevation_inactive Ractive">Floor Configuraion</a></li>
                </ul>
            </div>

        </div>
    </div>
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:HiddenField ID="hdnUpdate" runat="server" ClientIDMode="Static" />    
     <asp:HiddenField ID="upSave" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upupdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="updelete" runat="server" ClientIDMode="Static" />
    <div class="main_box RBox">
    	<div class="main_box_header RBoxheader">
            <h2>Floor Configuration</h2>
        </div>
    	<div class="main_box_body Rbody">  
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" />                               
                            </Triggers>
                            <ContentTemplate>
        	<div class="main_box_content">
                <div class="input_division_info" style="width:40%">
                    <table class="division_table">
                        <tr>
                            <td>Floor Name <span class="requerd1">*</span></td>
                            <td>&nbsp; :</td>
                            <td>
                                <asp:TextBox ID="txtFloor" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>                                                                                        
                            </td>
                        </tr>
                        <tr>
                            <td>বাংলায়</td>
                            <td>&nbsp; :</td>
                            <td>
                                <asp:TextBox ID="txtFloorBn" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width fontF"></asp:TextBox>                                                                                        
                            </td>
                        </tr>
                        <tr>
                            <td>Remarks</td>
                            <td>&nbsp; :</td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Height="50px" TextMode="MultiLine"></asp:TextBox>                                                                                        
                            </td>
                        </tr>
                        <tr><td><td></td></td><td><asp:CheckBox ID="chkIsActive" runat="server" ClientIDMode="Static" Text="Is Active" style="float:left;" /></td></tr>
                    </table>
                </div>
                <div class="button_area">
                    <table class="button_table">
                        <tr>
                            <th>
                                <asp:Button ID="btnNew" runat="server" ClientIDMode="Static" CssClass="Rbutton" Text="New" OnClick="btnNew_Click"  /></th>
                            <th>
                                <asp:Button ID="btnSave" ClientIDMode="Static" CssClass="Rbutton"  runat="server" Text="Save" OnClientClick="return validateInputs();" OnClick="btnSave_Click"    />
                            </th>
                            <%--<th>
                                <asp:Button ID="btnDelete" ClientIDMode="Static" CssClass="css_btn"  runat="server" Text="Delete"   />
                            </th>--%>
                            <th> <asp:Button ID="btnClose" ClientIDMode="Static" CssClass="Rbutton" PostBackUrl="~/hrd_default.aspx"  runat="server" Text="Close" /></th>
                        </tr>
                    </table>
                </div>
            <div class="show_division_info" >                          
                         <asp:GridView ID="gvFloorList"  runat="server"  DataKeyNames="FId" AllowPaging="True" AutoGenerateColumns="False" Width="100%" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" CellPadding="10" OnRowCommand="gvFloorList_RowCommand" OnRowDeleting="gvFloorList_RowDeleting" OnPageIndexChanging="gvFloorList_PageIndexChanging" OnRowDataBound="gvFloorList_RowDataBound"  >
                              <PagerStyle CssClass="gridview" Height="20px" />
                                <RowStyle HorizontalAlign="Center" />                            
                                <Columns >                                    
                                    <asp:BoundField DataField="FName"  HeaderText="Floor Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="FNameBn"  HeaderText="বাংলায়" HeaderStyle-HorizontalAlign="Center" ItemStyle-CssClass="fontF" ItemStyle-HorizontalAlign="Center"/>
                                    <asp:BoundField DataField="IsActive" HeaderText="Active"  HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="center"  />  
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"/>                                                                     
                                     <asp:TemplateField HeaderText="Edit" ItemStyle-Width="100px">
                                       <ItemTemplate  >
                                            <asp:Button ID="btnAlter" runat="server" ControlStyle-CssClass="btnForAlterInGV"  Text="Edit" CommandName="Alter" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                       </ItemTemplate>
                                   </asp:TemplateField>                                     
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="100px">
                                       <ItemTemplate  >
                                            <asp:Button ID="btnDelete" runat="server" ControlStyle-CssClass="btnForDeleteInGV"  Text="Delete" CommandName="Delete" CommandArgument='<%#((GridViewRow)Container).RowIndex%>'  OnClientClick="return confirm('Are you sure to delete ?')" />
                                       </ItemTemplate>
                                   </asp:TemplateField>                                     
                                </Columns>
                                 <HeaderStyle BackColor="#0057AE" ForeColor="white" Height="28px" />
                            </asp:GridView>                            
                        </div>           			
                </div>
                                  </ContentTemplate>
                        </asp:UpdatePanel>	
        </div>
    <%--</div>--%>
   <%-- </div>--%>
     <script type="text/javascript">
         //$('#dlDivision').change(function () {

         $('#btnNew').click(function () {
             clear();
         });
         function validateInputs() {
             if (validateText('txtFloor', 1, 60, 'Enter Floor Name') == false) return false;
             return true;
         }

         function editQualification(id) {
             var divsn = $('#r_' + id + ' td:first').html();

             $('#txtReligion').val(divsn);
             if ($('#updelete').val() == '1') {
                 $('#btnDelete').addClass('css_btn');
                 $('#btnDelete').removeAttr('disabled');
             }
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
             $('#hdnbtnStage').val(1);
             $('#hdnUpdate').val(id);
         }        
         function clear() {
             if ($('#upSave').val() == '0') {

                 $('#btnSave').removeClass('css_btn');
                 $('#btnSave').attr('disabled', 'disabled');
             }
             else {
                 $('#btnSave').addClass('css_btn');
                 $('#btnSave').removeAttr('disabled');
             }
             $('#txtReligion').val('');
             $('#btnSave').val('Save');
             $('#hdnbtnStage').val("");
             $('#hdnUpdate').val("");
             $('#btnDelete').removeClass('css_btn');
             $('#btnDelete').attr('disabled', 'disabled');
         }
    </script>
     </div>
</asp:Content>
