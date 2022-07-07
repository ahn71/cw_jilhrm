<%@ Page Title="Company Setup" Language="C#" MasterPageFile="~/hrd_nested.master" AutoEventWireup="true" CodeBehind="CompanyInfo.aspx.cs" Inherits="SigmaERP.hrd.CompanyInfo" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .rblC {
            padding-left:3px;
        }
        #ContentPlaceHolder1_MainContent_gvCompanyInfo th,td {
            padding-left:3px;
        }
        #ContentPlaceHolder1_MainContent_gvCompanyInfo th:nth-child(3),th:nth-child(4),th:nth-child(5),th:nth-child(6),th:nth-child(7),th:nth-child(8),th:nth-child(9),th:nth-child(10),th:nth-child(11) {
           text-align:center;
        }
        .deleteButton {
                background-color: #a20019;
    border: 2px solid gray;
    color: white;
    font-weight: bold;
    height: 27px;
    width: 66px;
        }
        .company_radio_btn tr td table{margin-left:5px;}
      
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
                       <li> <a href="#" class="ds_negevation_inactive Ractive">Company</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>    
    <asp:HiddenField ID="hdfID" ClientIDMode="Static" runat="server" Value="0" />
     <asp:HiddenField ID="upSave" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upupdate" runat="server" ClientIDMode="Static" />   
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="main_box RBox">
        <div class="main_box_header RBoxheader" >
            <h2>Company Setup Panel</h2>
        </div>
        <div class="main_box_body Rbody">
            <div class="main_box_content">
                 <div runat="server" id="divMsg" style="font-size: 12px;height: 24px;  padding-left: 8px;padding-top: 12px; width: 428px; "></div>
                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>                        
                       <asp:AsyncPostBackTrigger ControlID="rblOfficeType"/>
                        <asp:AsyncPostBackTrigger ControlID="rblCardNoType"/>
                    </Triggers>
                    <ContentTemplate>
                <div style="margin: 0px auto; width: 868px; overflow: hidden;">
                <div style=float:left;>
                
                        <div class="input_division_info_2" style="float:left; width:450px;">                           
                            <table class="division_table company_radio_btn">
                                <tr>
                                    <td>Company ID <span class="requerd1">*</span></td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtCompanyId" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Enabled="False" ></asp:TextBox>
                                    </td>                                   
                                </tr>
                                <tr><td>Company Type <span class="requerd1">*</span></td>
                                    <td>:</td>
                                    <td><asp:RadioButtonList ID="rblOfficeType"  runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblOfficeType_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="1">Head Office</asp:ListItem>
                                        <asp:ListItem Value="0">Branch Office</asp:ListItem>
                                        </asp:RadioButtonList></td>
                                </tr>
                                <tr runat="server" visible="false" id="trHeadOffice">
                                    <td>Head Office <span class="requerd1">*</span></td>
                                    <td>:</td>
                                    <td>                                        
                                    <asp:DropDownList ID="ddlHeadOffice" runat="server" ClientIDMode="Static" CssClass="form-control select_width" ></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Company Name <span class="requerd1">*</span></td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtCompanyName" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Short Name <span class="requerd1">*</span></td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtShortName" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" style="text-transform:uppercase" MaxLength="3" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>কোম্পানীর নাম <span class="requerd1">*</span></td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtCompanyNameBangla" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Font-Names="SutonnyMJ"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right;margin-top:0px">Address</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtAddress" ClientIDMode="Static" Height="60px" runat="server" CssClass="form-control text_box_width" TextMode="MultiLine" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>ঠিকানা <span class="requerd1">*</span></td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtAddressBangla" ClientIDMode="Static" Height="60px" TextMode="MultiLine" runat="server" CssClass="form-control text_box_width" Font-Names="SutonnyMJ" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Country</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtCountry" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" ></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>Telephone</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtTelephone" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Fax</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtFax" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" ></asp:TextBox>
                                    </td>
                                </tr> 
                                 <tr>
                                    <td>Default Currency</td>
                                    <td>:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlDefaultCurrency" ClientIDMode="Static" CssClass="form-control select_width"  runat="server">                                            
                                            <asp:ListItem Value="Taka">Taka</asp:ListItem>
                                            <asp:ListItem Value="Ruppe">Ruppe</asp:ListItem>                                            
                                            <asp:ListItem Value="Riyal">Riyal</asp:ListItem>
                                            <asp:ListItem Value="Dollar">Dollar</asp:ListItem>                                            
                                            <asp:ListItem Value="Pound">Pound</asp:ListItem>
                                 </asp:DropDownList>
                                        <%--<asp:TextBox ID="txtDefaultCurrency" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" ></asp:TextBox>--%>
                                    </td>
                                </tr>                               
                            </table>                            
                        </div>
                             
                                       
                </div>
                       <div style="float: right;">                     
                   <div style="text-align:center;">
                        <asp:Image ID="imgProfile" class="BImg" ClientIDMode="Static"  runat="server" ImageUrl="~/images/profileImages/Logo.png" />  
                        <asp:FileUpload ID="FileUpload1" style="margin-left:108px" runat="server"  onchange="previewFile()" ClientIDMode="Static" />
                    </div>
                    <table class="company_radio_btn">                        
                                                              
                        <tr>
                            <td>Business Type</td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList ID="ddlBusinessType" ClientIDMode="Static" CssClass="form-control select_width"  runat="server">                                            
                                 </asp:DropDownList>
                            </td>
                        </tr>
                         <tr>
                                <td>Multiple Branch</td>
                                 <td>:</td>
                                 <td>
                                    <asp:DropDownList ID="ddlMultipleBranch" ClientIDMode="Static" CssClass="form-control select_width"  runat="server">
                                      <asp:ListItem>No</asp:ListItem>
                                       <asp:ListItem>Yes</asp:ListItem>                                              
                                          </asp:DropDownList>
                                    </td>
                                </tr>  
                        <tr>
                                <td>Card No Digits</td>
                                 <td>:</td>
                                 <td>
                                    <asp:DropDownList ID="ddlCardNoDigit" ClientIDMode="Static" CssClass="form-control select_width"  runat="server">
                                      <asp:ListItem Value="3">3</asp:ListItem>
                                       <asp:ListItem Value="4">4</asp:ListItem>  
                                         <asp:ListItem Value="5">5</asp:ListItem>                                               
                                          </asp:DropDownList>
                                    </td>
                                </tr>  
                        <tr>
                                <td>Card No Type</td>
                                 <td>:</td>
                                 <td>
                                    <asp:RadioButtonList ID="rblCardNoType"   runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblCardNoType_SelectedIndexChanged">
                                        <asp:ListItem  Selected="True" Value="0">Flat</asp:ListItem>
                                        <asp:ListItem Value="1">Department Wise</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>                                                                               
                                 <tr>
                                    <td runat="server" id="tdFladCode">Flat Code</td>
                                    <td>:</td>
                                    <td>
                                       <asp:TextBox ID="txtFladCode" Font-Bold="true" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" style="width:25%;float:left;color:red;"  MaxLength="2" >99</asp:TextBox>
                                        <asp:TextBox ID="txtStartCardNo" ClientIDMode="Static" Font-Bold="true" placeholder="Start Card No" runat="server" CssClass="form-control text_box_width"  Width="71%" MaxLength="5" ></asp:TextBox>
                                        </td>
                                </tr>
                                  <tr>
                                    <td>Weekend</td>
                                    <td>:</td>
                                    <td>
                                         <asp:DropDownList ID="ddlWeekend" ClientIDMode="Static" CssClass="form-control select_width"  runat="server">
                                             <asp:ListItem>Friday</asp:ListItem>
                                              <asp:ListItem>Saturday</asp:ListItem>
                                              <asp:ListItem>Sunday</asp:ListItem>
                                             <asp:ListItem>Monday</asp:ListItem>
                                              <asp:ListItem>Tuesday</asp:ListItem>
                                             <asp:ListItem>Wednesday</asp:ListItem>
                                              <asp:ListItem>Thursday</asp:ListItem>
                                         </asp:DropDownList>
                                    </td>
                                </tr>
                         <tr>
                                    <td>Att Machine</td>
                                    <td>:</td>
                                    <td>
                                         <asp:DropDownList ID="ddlMachine" ClientIDMode="Static" CssClass="form-control select_width"  runat="server">
                                             <asp:ListItem Value="ZK">ZK</asp:ListItem>
                                             <asp:ListItem Value="RMS">RMS</asp:ListItem>                                            
                                         </asp:DropDownList>
                                    </td>
                                </tr>
                            <tr>
                                    <td>Comments</td>
                                    <td>:</td>
                                    <td>
                                        <asp:TextBox ID="txtComments" ClientIDMode="Static" Height="60px" runat="server" CssClass="form-control text_box_width" TextMode="MultiLine" ></asp:TextBox>
                                    </td>
                                </tr>
                    </table>                  
                        </div>
                    </div>
            </ContentTemplate>
                </asp:UpdatePanel>
                    
                    <div class="button_area_company_setup">
                        <a href="#" onclick="window.history.back()" class="Rbutton">Back</a>
                        <asp:Button ID="btnSave" ClientIDMode="Static" CssClass="Rbutton" OnClientClick="return InputValidation();" runat="server" Text="Save"  OnClick="btnSave_Click" />
                        <asp:Button ID="btnClear" Class="Rbutton" runat="server" Text="Clear" OnClick="btnClear_Click" />
                        <asp:Button ID="btnClose" Class="Rbutton" runat="server" Text="Close" PostBackUrl="~/hrd_default.aspx" />
                    </div>

                <div  class="show_division_info">
                    <asp:GridView ID="gvCompanyInfo" runat="server" AutoGenerateColumns="False" Width="100%"  AllowPaging="True" PageSize="4"  DataKeyNames="ID,CompanyId" OnPageIndexChanging="gvCompanyInfo_PageIndexChanging" OnRowCommand="gvCompanyInfo_RowCommand" OnRowDataBound="gvCompanyInfo_RowDataBound" OnRowDeleting="gvCompanyInfo_RowDeleting"  >
                <HeaderStyle BackColor="#0057AE" Height="28px" HorizontalAlign="Center" Font-Bold="True" Font-Size="14px" ForeColor="White"></HeaderStyle>
                         <PagerStyle CssClass="gridview Sgridview" Height="40px" />
                          <Columns>
                             <asp:BoundField DataField="CompanyId" HeaderText="CompanyId"  Visible="false" />
                              <asp:BoundField DataField="CompanyName" HeaderStyle-HorizontalAlign="Left" HeaderText="Name" Visible="true" />
                             <asp:BoundField DataField="Address" HeaderStyle-HorizontalAlign="Left" HeaderText="Address" Visible="true" >
                              <ItemStyle Width="100px" />
                              </asp:BoundField>
                             <asp:BoundField DataField="Country" ItemStyle-HorizontalAlign="Center" HeaderText="Country" Visible="true" />
                             <asp:BoundField DataField="Telephone" ItemStyle-HorizontalAlign="Center" HeaderText="Telephone" Visible="true" />
                             <asp:BoundField DataField="Fax" ItemStyle-HorizontalAlign="Center" HeaderText="Fax" Visible="true" />
                              <asp:BoundField DataField="ComType" ItemStyle-HorizontalAlign="Center" HeaderText="Company Type" Visible="true" />
                             <asp:BoundField DataField="BTypeName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="BusinessType" Visible="true" />
                              <asp:BoundField DataField="StartCardNo" HeaderText="S.Card No" ItemStyle-HorizontalAlign="Center" Visible="true" />
                             <%--<asp:ButtonField ButtonType="Button" ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="btnForAlterInGV"    HeaderText="Edit" Text="Edit" CommandName="Alter" />--%>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            Delete
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="btnAlter" runat="server"  ControlStyle-CssClass="btnForAlterInGV"   Text="Edit" CommandName="Alter"
                                               
                                                CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            Delete
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="btnDelete" runat="server"  ControlStyle-CssClass="btnForDeleteInGV"   Text="Delete" CommandName="Delete"
                                                OnClientClick="return confirm('Are you sure, you want to delete the record?')"
                                                CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                          </Columns>                         
                     </asp:GridView>
                </div>               
                </div>        

        </div>
    </div>
       <%-- </div>--%>
    <script type="text/javascript">
        function previewFile() {
            try {
                var preview = document.querySelector('#imgProfile');
                var file = document.querySelector('#FileUpload1').files[0];

                var reader = new FileReader();

                reader.onloadend = function () {
                    preview.src = reader.result;
                }

                if (file) {
                    reader.readAsDataURL(file);
                } else {
                    preview.src = "";
                }
                var imagename = $('#FileUpload1').val();
                $('#HiddenField1').val(imagename);
            }
            catch (exception) {
                lblMessage.innerText = exception;
            }

        }
        function InputValidation() {
            var value = $('select#ddlCardNoDigit option:selected').val();
            if (validateText('txtCompanyName', 1, 100, "Please Enter Company Name") == false) return false;
            if (validateText('txtShortName', 3, 10, "Please Enter Sort Name of Company (Must be 3 Character)") == false) return false;
            if (validateText('txtCompanyNameBangla', 1, 100, "Please Enter Company Name in Bangla") == false) return false;
            if (validateText('txtAddress', 1, 300, "Please Enter Company Address") == false) return false;
            if (validateText('txtAddressBangla', 1, 100, "Please Enter Company Addrerss in Bangla") == false) return false;
            if (validateText('txtStartCardNo', value, value, "Enter Start Card Number (Must be " + value + " Character)") == false) return false;
            
            return true;
        }
        function Success()
        {
            showMessage('Successfully Saved','success');
        }
        function updSuccess()
        {
            showMessage('Successfully Update','success');
        }
        function delSuccess() {
            showMessage('Successfully Deleted','success');
        }
        function Warning() {
            showMessage('Frist Set a Head Office','warning');
        }
        //function AllClear()
        //{
        //    $("#txtCompanyName").val("");
        //    $("#txtShortName").val("");
        //    $("#txtAddress").val("");
        //    $("#txtAddressBangla").val("");
        //    $("#txtComments").val("");
        //    $("#txtCompanyId").val("");
        //    $("#txtCompanyNameBangla").val("");
        //    $("#txtCountry").val("");
        //    $("#txtFax").val("");
        //    $("#txtTelephone").val("");
        //    $("#txtStartCardNo").val("");
        //    $("#rblOfficeType").val('1');
        //    $("#txtCompanyName").val("");
        //}
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
</asp:Content>
