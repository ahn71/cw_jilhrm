<%@ Page Title="District Configuration" Language="C#" MasterPageFile="~/hrd_nested.master" AutoEventWireup="true" CodeBehind="district_Config.aspx.cs" Inherits="SigmaERP.hrd.district_Config" %>
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
        #ContentPlaceHolder1_MainContent_gvDistrictList th:nth-child(4), th:nth-child(5) {
            text-align:center;
        }
        #ContentPlaceHolder1_MainContent_gvDistrictList th:nth-child(1),td:nth-child(1),th:nth-child(2),td:nth-child(2),th:nth-child(3),td:nth-child(3){
            padding-left:3px;
            
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
                       <li> <a href="#" class="ds_negevation_inactive Ractive">District Configuration</a></li>
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
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">

                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" />
                                <asp:AsyncPostBackTrigger ControlID="gvDistrictList" />
                                <asp:AsyncPostBackTrigger ControlID="dlDivision" />
                                <asp:AsyncPostBackTrigger ControlID="btnNew" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:HiddenField ID="hdnbtnStage" runat="server" ClientIDMode="Static" />
    <div class="main_box RBox">
    	<div class="main_box_header RBoxheader">
            <h2>District Configuration Panel</h2>
        </div>
    	<div class="main_box_body Rbody">
        	<div class="main_box_content">
                <div class="input_division_info">
                    <table class="division_table">
                        <tr>
                            <td>
                               Division Name <span class="requerd1">*</span>
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                               <asp:DropDownList ID="dlDivision" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" AutoPostBack="true" OnSelectedIndexChanged="dlDivision_SelectedIndexChanged">
                                   <asp:ListItem>Dhaka</asp:ListItem>
                                   <asp:ListItem>Mymensingh</asp:ListItem> 
                                   <asp:ListItem>Chittagong</asp:ListItem> 
                                   <asp:ListItem>Sylhet</asp:ListItem> 
                                   <asp:ListItem>Rangpur</asp:ListItem> 
                                   <asp:ListItem>Rajshahi</asp:ListItem> 
                                   <asp:ListItem>Khulna</asp:ListItem> 
                                   <asp:ListItem>Barisal</asp:ListItem>                               
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               District Name <span class="requerd1">*</span>  
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistrictName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               বাংলায়
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistrictNameBn" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width fontF"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>

                <div class="button_area Rbutton_area">
                    <a href="#" onclick="window.history.back()" class="Rbutton">Back</a>
                    <asp:Button ID="btnNew" ClientIDMode="Static" CssClass="Rbutton"  runat="server" Text="New"  OnClick="btnNew_Click"/>
                    <asp:Button ID="btnSave" ClientIDMode="Static" CssClass="Rbutton"  runat="server" Text="Save" OnClientClick="return validateInputs();" OnClick="btnSave_Click"  />
                    <asp:Button ID="btnClose" ClientIDMode="Static" CssClass="Rbutton" PostBackUrl="~/hrd_default.aspx"  runat="server" Text="Close" />
                </div>

                <div class="show_division_info">
                    <asp:GridView ID="gvDistrictList" runat="server" DataKeyNames="DstId" AllowPaging="True" PageSize="10"  AutoGenerateColumns="False" Width="100%" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" OnRowCommand="gvDistrictList_RowCommand" OnRowDataBound="gvDistrictList_RowDataBound" OnPageIndexChanging="gvDistrictList_PageIndexChanging" OnRowDeleting="gvDistrictList_RowDeleting"  >
                             <RowStyle HorizontalAlign="Center" />
                              <PagerStyle CssClass="gridview Sgridview" Height="40px"/>
                             <Columns>
                                 <asp:BoundField DataField="Division" HeaderText="Division" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                 <asp:BoundField DataField="DstName" HeaderText="District" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                  <asp:BoundField DataField="DstBangla" HeaderText="জেলা" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="fontF" />



                                 <asp:TemplateField HeaderText="Edit" ItemStyle-Width="100px">
                                     <ItemTemplate>
                                         <asp:Button ID="btnAlter" runat="server" ControlStyle-CssClass="btnForAlterInGV" Text="Edit" CommandName="Alter" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                 <%--<asp:ButtonField CommandName="Alter"   ControlStyle-CssClass="btnForAlterInGV"  HeaderText="Alter" ButtonType="Button" Text="Alter" ItemStyle-Width="80px"/>--%>

                                 <asp:TemplateField HeaderText="Delete" ItemStyle-Width="100px">
                                     <ItemTemplate>
                                         <asp:Button ID="btnDelete" runat="server" ControlStyle-CssClass="btnForDeleteInGV" Text="Delete" CommandName="Delete" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' OnClientClick="return confirm('Are you sure to delete ?')" />
                                     </ItemTemplate>
                                 </asp:TemplateField>

                             </Columns>
                             <HeaderStyle BackColor="#0057AE" Height="28px" />
                         </asp:GridView>

                </div>
                      
            
            </div>
        </div>
                          </ContentTemplate>
                </asp:UpdatePanel>
      <script type="text/javascript">

          //$('#dlDivision').change(function () {



          $('#btnNew').click(function () {
              clear();
          });
          function validateInputs() {
              if (validateText('txtDistrictName', 1, 60, 'Enter District Name !') == false) return false;
              return true;
          }

          function editLineConfig(id) {
              var divsn = $('#r_' + id + ' td:first').html();
              $('#dlDivision option:selected').text(divsn);
              var DistrictName = $('#r_' + id + ' td:nth-child(2)').html();
              $('#txtDistrictName').val(DistrictName);

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

              $('#txtDistrictName').val('');
              $('#btnSave').val('Save');
              $('#hdnbtnStage').val("");
              $('#hdnUpdate').val("");
              $('#btnDelete').removeClass('css_btn');
              $('#btnDelete').attr('disabled', 'disabled');
          }

    </script>
   
</asp:Content>
