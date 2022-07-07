<%@ Page Title="Blood Group" Language="C#" MasterPageFile="~/personnel_NestedMaster.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="blood_group.aspx.cs" Inherits="SigmaERP.personnel.blood_group" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>   
      
      .tdWidth{
            width:400px;
            height:40px;
        }
    
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
                <div class="row">
                  <div class="col-md-12 ds_nagevation_bar">
               <div style="margin-top: 5px">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                       <li><a href="#">/</a></li>
                       <li><a href="/personnel_defult.aspx">Personnel</a></li>
                       <li><a href="#">/</a></li>
                       <li><a href="/personnel/employee_index.aspx">Employee Information</a></li>
                       <li><a href="#">/</a></li>
                       <li><a href="#" class="ds_negevation_inactive">Blood Group Report</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>
   <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:HiddenField ID="upSuperAdmin" runat="server" ClientIDMode="Static" />
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
          <Triggers>
              <asp:AsyncPostBackTrigger ControlID="rdball" />
              <asp:AsyncPostBackTrigger ControlID="rdbindividual" />
              <asp:AsyncPostBackTrigger ControlID="rblEmpType" />
              <asp:AsyncPostBackTrigger ControlID="dsBloodGroup" />           
          </Triggers>
          <ContentTemplate>
    <div class="row Ptrow">
        
        <div class="employee_box_header PtBoxheader">
            <h2>Blood Group Report</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content" style="height:255px;">
                <div class="punishment_against">
                        <h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center"></h1>
                     <table runat="server" id="tblGenerateType" class="employee_table">
                        <tr>
                            <td>
                                Company
                            </td>
                            <td>:</td>
                            <td class="tdWidth">
                                <asp:DropDownList ID="ddlBranch"   CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                    
                                </asp:DropDownList>
                            </td>
                        </tr>
                           <tr>
                            <td>Employee Type</td>
                            <td>:</td>
                            <td class="tdWidth">
                                <asp:RadioButtonList runat="server" ID="rblEmpType" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblEmpType_SelectedIndexChanged">
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>Report Type</td>
                            <td>:</td>
                            <td class="tdWidth">
                                 <asp:RadioButton ID="rdball" Class=""  ClientIDMode="Static" AutoPostBack="true" Checked="true" runat="server" Text="All" OnCheckedChanged="rdball_CheckedChanged"  />
                                <asp:RadioButton ID="rdbindividual" ClientIDMode="Static" AutoPostBack="true" runat="server" Text="Individual" OnCheckedChanged="rdbindividual_CheckedChanged" />
                                <asp:RadioButton ID="rdbGroup" AutoPostBack="true" runat="server" Text="Blood Group" OnCheckedChanged="rdbGroup_CheckedChanged"  />
                            </td>
                        </tr>
                        <tr runat="server" id="divindivisual">
                            <td>Card No / Name
                            </td>
                            <td>:</td>
                            <td class="tdWidth">
                                <asp:DropDownList ID="ddlCardNo" ClientIDMode="Static" CssClass="form-control select_width"  runat="server"></asp:DropDownList>                                
                            </td>
                        </tr> 
                               <tr runat="server" visible="false" id="trBloodGroup"> 
                           <td> Blood Group</td> 
                                   <td>:</td>                     
                           <td class="tdWidth">                                
                                <asp:DropDownList ID="dsBloodGroup" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" >
                                     <asp:ListItem></asp:ListItem> 
                                     <asp:ListItem>A+</asp:ListItem>  
                                     <asp:ListItem>A-</asp:ListItem> 
                                     <asp:ListItem>B+</asp:ListItem> 
                                     <asp:ListItem>B-</asp:ListItem> 
                                     <asp:ListItem>AB+</asp:ListItem> 
                                     <asp:ListItem>AB-</asp:ListItem> 
                                     <asp:ListItem>O+</asp:ListItem> 
                                     <asp:ListItem>O-</asp:ListItem>                                  
                                </asp:DropDownList>                                
                            </td>
                       </tr>                           
                        </table>                   
                </div>           
                <div class="punishment_button_area">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                <th><asp:Button ID="btnPrintpreview" runat="server" CssClass="back_button Ptbut" ClientIDMode="Static" Text="Preview" OnClick="btnPrintpreview_Click" /></th>
                                <th><asp:Button ID="btnClose" PostBackUrl="~/personnel_defult.aspx" Text="Close" runat="server" CssClass="css_btn Ptbut" /></th>   
                         </tr>
                    </tbody>
                  </table>
                </div>

        </div>
      </div>
    </div>
              </ContentTemplate>
        </asp:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ddlCardNo").select2();

        });
        function loadcardNo() {
            $("#ddlCardNo").select2();
        }
        function goToNewTabandWindow(url) {
            window.open(url);
            loadcardNo();
        }

    </script>
</asp:Content>
