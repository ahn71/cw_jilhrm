<%@ Page Title="Thana" Language="C#" MasterPageFile="~/hrd_nested.master" AutoEventWireup="true" CodeBehind="Thana.aspx.cs" Inherits="SigmaERP.hrd.Thanas" %>
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
        #ContentPlaceHolder1_MainContent_gvLateDeductionTypeList th, td {
            padding-left: 3px;
        }

        #ContentPlaceHolder1_MainContent_gvLateDeductionTypeList th:nth-child(5),th:nth-child(6) {
            text-align: center;
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
                       <li> <a href="#" class="ds_negevation_inactive Ractive">Thana Configuration</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
     <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>

    <div class="main_box RBox">
        <div class="main_box_header RBoxheader">
            <h2>Thana Configuration Panel</h2>
        </div>
        <div class="main_box_body Rbody">
            <div class="main_box_content" id="divElementContainer" runat="server">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                       
                        <asp:AsyncPostBackTrigger ControlID="btnSave" />
                        <asp:AsyncPostBackTrigger ControlID="ddlDistrict" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="input_division_info Tinfo">
                            <table class="division_table">
                               
                                <tbody>

                                     <tr id="trCompanyName" runat="server">
                                        <td>District <span class="requerd1">*</span></td>
                                        <td>:</td>
                                        <td>
                                        <asp:DropDownList ID="ddlDistrict" ClientIDMode="Static"   CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged"  >              
                                         </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td>Leave Id
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLeaveId" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False"></asp:TextBox>

                                        </td>
                                    </tr>--%>
                                    
                                    <tr>
                                        <td>Thana <span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="txtThanaName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>

                                            

                                            <%--<asp:RequiredFieldValidator  Type="Integer" ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator2" runat="server"  ControlToValidate="txtLeaveDays" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>বাংলায়
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="txtThanaNameBn" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width fontF"></asp:TextBox>

                                            

                                            <%--<asp:RequiredFieldValidator  Type="Integer" ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator2" runat="server"  ControlToValidate="txtLeaveDays" ErrorMessage="*"></asp:RequiredFieldValidator>--%>

                                        </td>
                                    </tr>
                                    
                                    
                                    
                

                                </tbody>
                            </table>
                            
                            <div class="button_area Rbutton_area" style="width:100%">
                              <a href="#" onclick="window.history.back()" class="Rbutton">Back</a>        
                               <asp:Button ID="btnSave" ValidationGroup="save" CssClass="Rbutton" runat="server" Text="Save" OnClick="btnSave_Click"   />      
                               <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="Rbutton" OnClick="btnClear_Click" />      
                               <asp:Button ID="Button3" runat="server" Text="Close" CssClass="Rbutton" PostBackUrl="~/attendance_default.aspx" OnClick="Page_Load" />
                            </div>  

                        </div>
                       
                        <div class="show_division_info" >
                            

                            <asp:GridView ID="gvLateDeductionTypeList" runat="server" DataKeyNames="ThaId,DstId" AllowPaging="True" AutoGenerateColumns="False" Width="100%" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" OnRowCommand="gvLateDeductionTypeList_RowCommand" OnRowDeleting="gvLateDeductionTypeList_RowDeleting" OnRowDataBound="gvLateDeductionTypeList_RowDataBound" OnPageIndexChanging="gvLateDeductionTypeList_PageIndexChanging"  >
                                <RowStyle HorizontalAlign="Center" />
                                <PagerStyle CssClass="gridview Sgridview" Height="40px" />
                                <Columns>
                                    

                                    <asp:BoundField DataField="DstName" HeaderText="District"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                    <asp:BoundField DataField="DstBangla" HeaderText="জেলা"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="fontF"/>
                                    <asp:BoundField DataField="ThaName" HeaderText="Thana"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"  />
                                    <asp:BoundField DataField="ThaNameBangla" HeaderText="থানা"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="fontF"  />
                                   
                                    
                                    

                                     <asp:TemplateField HeaderText="Edit" ItemStyle-Width="100px">
                                       <ItemTemplate  >
                                            <asp:Button ID="btnAlter" runat="server" ControlStyle-CssClass="btnForAlterInGV"  Text="Edit" CommandName="Alter" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                       </ItemTemplate>
                                   </asp:TemplateField>
                                    <%--<asp:ButtonField CommandName="Alter"   ControlStyle-CssClass="btnForAlterInGV"  HeaderText="Alter" ButtonType="Button" Text="Alter" ItemStyle-Width="80px"/>--%>
                                   
                                    <asp:TemplateField HeaderText="Delete" ItemStyle-Width="100px">
                                       <ItemTemplate  >
                                            <asp:Button ID="btnDelete" runat="server" ControlStyle-CssClass="btnForDeleteInGV"  Text="Delete" CommandName="Delete" CommandArgument='<%#((GridViewRow)Container).RowIndex%>'  OnClientClick="return confirm('Are you sure to delete ?')" />
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
</asp:Content>
