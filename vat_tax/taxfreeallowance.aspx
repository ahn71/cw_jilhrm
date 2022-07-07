<%@ Page Title="Tax Free Allowance" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="taxfreeallowance.aspx.cs" Inherits="SigmaERP.vat_tax.taxfreeallowance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row Rrow">
                  <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dasboard</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="/payroll_default.aspx">Payroll</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li>  <a href="/vat_tax/vat_tax_index.aspx">Vat and Tax</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                     <li> <a href="#" class="ds_negevation_inactive Pactive">Tax Free Allowance</a></li>
                </ul>
            </div>
        </div>
       </div>
  
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>   
    <div class="main_box Mbox">
    	<div class="main_box_header PBoxheader">
            <h2>Tax Free Allowance</h2>
        </div>
    	<div class="main_box_body Pbody">
        	<div class="main_box_content">
                  <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave"/>  
               
        </Triggers>
        <ContentTemplate>
                <div class="input_division_info">
                   <div class="row">
                        <div class="col-sm-12">
                              <div  runat="server" Visible="false" class="row tbl-controlPanel">
                                <div class="col-sm-4">Company Name<span class="requerd1">*</span></div>
                                <div class="col-sm-8">
                                     <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" AutoPostBack="true" >
                                                                        
                                      </asp:DropDownList>
                                </div>
                              </div>
                             <div class="row tbl-controlPanel">
                                <div class="col-sm-4">Convence Allownce<span class="requerd1">*</span></div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtConvenceAllownce" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                
                                </div>
                              </div>
                              <div class="row tbl-controlPanel">
                                <div class="col-sm-4">House Rent<span class="requerd1">*</span></div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtHouseRent" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>                                
                                </div>
                              </div>
                              <div class="row tbl-controlPanel">
                                <div class="col-sm-4">Medical Allownce<span class="requerd1">*</span></div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtMedicalAllownce" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>                                
                                </div>
                              </div>                              
                              </div>
                            
                          
                        </div>
                    </div>                

                
              
                <div class="button_area Rbutton_area">
                    <a href="#" onclick="window.history.back()" class="Pbutton">Back</a>                    
                    <asp:Button ID="btnSave" ClientIDMode="Static" class="Pbutton" runat="server" Text="Save" OnClientClick="return validateInputs();" OnClick="btnSave_Click"/>
                    <asp:Button ID="btnClose" ClientIDMode="Static" CssClass="Pbutton" PostBackUrl="~/vat_tax/vat_tax_index.aspx"  runat="server" Text="Close" />
                </div>
            </ContentTemplate>
                      </asp:UpdatePanel>

                  <div class="show_division_info">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">    
        <ContentTemplate>
                     <%--<div id="ShiftConfig" class="datatables_wrapper" runat="server" style="width:100%; height:auto; max-height:500px;overflow:auto;overflow-x:hidden;"></div>--%>
                    <asp:GridView ID="gvtaxfreeallowance" runat="server" CssClass="gvdisplay1"  Width="100%" AutoGenerateColumns="False" DataKeyNames="TFSN"  OnRowCommand="gvtaxfreeallowance_RowCommand" OnRowDataBound="gvtaxfreeallowance_RowDataBound"  AllowPaging="True"  >

<HeaderStyle BackColor="#FFA500" Font-Bold="True" Font-Size="14px" ForeColor="White" Height="28px"></HeaderStyle>
                        <PagerStyle CssClass="gridview Sgridview" Height="40px" />
                       <RowStyle HorizontalAlign="Center" />
                         <Columns>                     
                            <%--  <asp:BoundField DataField="CompanyName" HeaderStyle-HorizontalAlign="Left" HeaderText="Company Name" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="Left"  >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>

<ItemStyle HorizontalAlign="Left" Height="28px" ></ItemStyle>
                             </asp:BoundField>--%>
                            <asp:BoundField DataField="ConvenceAllownce" HeaderText="Convence Allownce" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center"   >
<ItemStyle Height="28px" ></ItemStyle>
                             </asp:BoundField>
                            <asp:BoundField DataField="HouseRent" HeaderText="House Rent" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center" >
<ItemStyle Height="28px" ></ItemStyle>
                             </asp:BoundField>  
                              <asp:BoundField DataField="MedicalAllownce" HeaderText="Medical Allownce" Visible="true"  ItemStyle-Height="28px" ItemStyle-HorizontalAlign="center"  >
<ItemStyle Height="28px" ></ItemStyle>
                              </asp:BoundField>                             
                             
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAlter" runat="server" CommandName="Alter" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Edit" Font-Bold="true" ForeColor="Green" ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Delete" >
                                <ItemTemplate>
                                     <asp:LinkButton ID="lnkDelete" runat="server" CommandName="deleterow" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Delete" Font-Bold="true" ForeColor="Red" OnClientClick="return confirm('Are you sure to delete?');" ></asp:LinkButton>
                                </ItemTemplate>

                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
         </ContentTemplate>
                           </asp:UpdatePanel>
                </div>
				
            </div>
        </div>
    </div>   
    <script type="text/javascript">
        
       function validateInputs() {
           if (validateText('txtConvenceAllownce', 1, 60, 'Enter Tax Free Conveyance Allowance') == false) return false;
           if (validateText('txtHouseRent', 1, 60, 'Enter Tax Free House rent Allowance') == false) return false;
           if (validateText('txtMedicalAllownce', 1, 60, 'Enter Tax Free Medical Allowance') == false) return false;
             return true;
       }
          </script>
</asp:Content>
