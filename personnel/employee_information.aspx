<%@ Page Title="Employee List" Language="C#" MasterPageFile="~/personnel_NestedMaster.Master" AutoEventWireup="true" CodeBehind="employee_information.aspx.cs" Inherits="SigmaERP.personnel.employee_information" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .punishment_against1 {
  overflow: hidden;
  
  margin-right:15px;
  margin:0 auto;
  margin-bottom:10px;
}
        .auto-style1 {
            width: 214px;
        }
        .auto-style2 {
            width: 108px;
        }
         .tdWidth{
            width:400px;
            height:45px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
                  <div class="col-md-12 ">
               <div class="ds_nagevation_bar">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                       <li><a href="#">/</a></li>
                       <li><a href="/personnel_defult.aspx">Personnel</a></li>
                       <li><a href="#">/</a></li>
                       <li><a href="/personnel/employee_index.aspx">Employee Information</a></li>
                       <li><a href="#">/</a></li>
                       <li><a href="#" class="ds_negevation_inactive Ptactive">Employees List</a></li>
                   </ul>
               </div>
          
             </div>
       </div>
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlDepartment" />
            <asp:AsyncPostBackTrigger ControlID="ddlCompany" />
            
        </Triggers>
        <ContentTemplate>
    <div class="row Ptrow">
        <div class="employee_box_header PtBoxheader">
            <h2>Employee List Report</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content" style="height:495px">
        <div class="punishment_against1">
                <div class="punishment_against">  
                    <h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center"></h1>                  
                  <table runat="server" id="tblGenerateType" class="employee_table">                 
                        <tbody>
                             <tr id="trCompany" visible="false" runat="server">
                            <td >
                                Company
                            </td>
                            <td>
                                :
                            </td>
                            <td class="tdWidth">
                               <asp:DropDownList ID="ddlCompany" ClientIDMode="Static" CssClass="form-control select_width" runat="server" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" AutoPostBack="True">
                                   
                                </asp:DropDownList>                                
                            </td>
                            
                        </tr>
                             <tr >
                                <td>
                                Department
                            </td>
                            <td>
                                :
                            </td>
                            <td class="tdWidth">
                               <asp:DropDownList ID="ddlDepartment" AutoPostBack="true" ClientIDMode="Static" CssClass="form-control select_width" runat="server" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged">
                                   
                                </asp:DropDownList>
                                
                            </td>
                            </tr>  
                        <tr runat="server" visible="false">
                            <td>Card No</td>
                             <td>:</td>
                             <td  runat="server"  class="tdWidth">
                                  <asp:DropDownList ID="ddlCardNo" ClientIDMode="Static" CssClass="form-control select_width"  runat="server"></asp:DropDownList>
                                  <asp:TextBox ID="txtCardNo" Visible="false" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>                                
                            </td>
                             <td></td>
                        </tr>    
                            <tr>
                                <td>Employee Type</td>
                                <td>:</td>
                                <td colspan="6">
                                    <asp:RadioButtonList runat="server" ID="rblEmpType" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblEmpType_SelectedIndexChanged" >
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                           <tr>
                                <td>Language</td>
                                <td>:</td>
                                <td class="tdWidth"><asp:RadioButtonList ID="rblLanguage"  RepeatDirection="Horizontal"  runat="server"  > 
                                     <asp:ListItem Value="EN" Selected="True" >English</asp:ListItem>   
                                    <asp:ListItem Value="BN" >Bangla</asp:ListItem>                                  
                                                                                                                                                          
                                     </asp:RadioButtonList>
                                </td>
                            </tr> 
                            <tr>
                                <td>Report Type</td>
                                <td>:</td>
                                <td class="tdWidth"><asp:RadioButtonList ID="rdblSearchEmployee"  RepeatColumns="5" runat="server"  AutoPostBack="True" OnSelectedIndexChanged="rdblSearchEmployee_SelectedIndexChanged">                                 
                                         <asp:ListItem Selected="True">Basic</asp:ListItem>
                                         <asp:ListItem >Designation</asp:ListItem>
                                         <asp:ListItem>District</asp:ListItem>
                                         <asp:ListItem>Joining</asp:ListItem>
                                         <asp:ListItem>Religion</asp:ListItem>                                      
                                     </asp:RadioButtonList>
                                </td>
                            </tr>
                     <tr id="trStatus" runat="server">
                            <td>
                                Status
                            </td>
                            <td>
                                :
                            </td>
                            <td >
                               <asp:DropDownList ID="ddlEmpStatus" ClientIDMode="Static" CssClass="form-control select_width" runat="server">
                                   
                                </asp:DropDownList>
                                
                            </td>
                        </tr> 
                            <tr id="trFdate" runat="server">
                                <td>
                                    From Date
                                </td>
                                <td>
                                :
                            </td>
                                <td>
                                    <asp:TextBox ID="dtpFromDate" CssClass="form-control text_box_width" Width="42%" runat="server"></asp:TextBox>
                                     <asp:CalendarExtender ID="CalendarExtender2" Format=dd-MM-yyyy runat="server" TargetControlID="dtpFromDate"></asp:CalendarExtender>
                                </td>
                            </tr>   
                            <tr id="trTdate" runat="server">
                                <td>
                                    To Date
                                </td>
                                <td>
                                :
                            </td>
                                <td >
                                    <asp:TextBox ID="dtpTodate" CssClass="form-control text_box_width" Width="42%"  runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" Format=dd-MM-yyyy runat="server" TargetControlID="dtpTodate"></asp:CalendarExtender>
                                </td>
                            </tr> 
                           
                            <tr id="trDistrict" runat="server">
                                <td>
                                District
                            </td>
                            <td>
                                :
                            </td>
                            <td >
                               <asp:DropDownList ID="ddlDistrict" ClientIDMode="Static" CssClass="form-control select_width" runat="server" >
                                   
                                </asp:DropDownList>
                                
                            </td>
                            </tr>  
                             <tr id="trReligion" runat="server">
                                <td>
                                Religion
                            </td>
                            <td>
                                :
                            </td>
                            <td >
                               <asp:DropDownList ID="ddlReligion"  ClientIDMode="Static" CssClass="form-control select_width" runat="server" >
                                   
                                </asp:DropDownList>
                                
                            </td>
                            </tr>  
                               
                      
                    </tbody>
                  </table>
                </div>
                   <div id="divdesignation" visible="false" runat="server" class="id_card" style="background-color:white; width:61%;">
                    <div class="id_card_left EilistL">
                        <p>Available Designation</p>
                        <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata EilistCec" SelectionMode="Multiple"></asp:ListBox>
                    </div>
                    <div class="id_card_center EilistC">
                        <table style="margin-top:38px;" class="employee_table">                     
                              <tr>
                                    <td>
                                        <asp:Button ID="btnAddItem" Class="arrow_button" runat="server" Text=">" OnClick="btnAddItem_Click"   />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnAddAllItem" Class="arrow_button" runat="server" Text=">>" OnClick="btnAddAllItem_Click"   />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveItem" Class="arrow_button" runat="server" Text="<" OnClick="btnRemoveItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveAllItem" Class="arrow_button" runat="server" Text="<<" OnClick="btnRemoveAllItem_Click"   />
                                    </td>
                               </tr>
                        </table>
                    </div>
                    <div class="id_card_right EilistR">
                        <p>Selected Designation</p>
                          <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata EilistCec"  ClientIDMode="Static" runat="server"></asp:ListBox>
                    </div>
                </div>
                <div class="button_area Rbutton_area" style="text-align: center; width: auto;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/lock.png.jpg" Height="60px" Width="60px" Visible="false" />                              
                    <asp:Button ID="btnPreview" ClientIDMode="Static" CssClass="css_btn Ptbut" runat="server"  Text="Preview" OnClick="btnPreview_Click" />
                    <asp:Button ID="btnClose" CssClass="css_btn Ptbut" runat="server" Text="Close" PostBackUrl="~/personnel_defult.aspx" />
                    <asp:Button ID="Button1" CssClass="css_btn Ptbut" runat="server" Text="Clear" />
                </div>

        </div>
      </div>
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
     <script type="text/javascript">
         $(document).ready(function () {
             $("#ddlCardNo").select2();
             $("#ddlDepartment").select2();
             $("#ddlDistrict").select2();


         });
         function loadcardNo() {
             $("#ddlCardNo").select2();
             $("#ddlDepartment").select2();
             $("#ddlDistrict").select2();
            
         }
         function goToNewTabandWindow(url) {

             window.open(url);
             loadcardNo();

         }
         function InputBoxNew() {             
            
             $("#ddlCardNo").val('0');
             loadcardNo();
            // $('#txtCardNo').val('');
         }
    </script>
</asp:Content>
