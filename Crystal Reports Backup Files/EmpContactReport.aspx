<%@ Page Title="Contact Info" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="EmpContactReport.aspx.cs" Inherits="SigmaERP.personnel.EmpContactReport" %>
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
                  <div class="col-md-12 ">
               <div class="ds_nagevation_bar">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                       <li><a href="#">/</a></li>
                       <li><a href="/personnel_defult.aspx">Personnel</a></li>
                       <li><a href="#">/</a></li>
                       <li><a href="/personnel/employee_index.aspx">Employee Information</a></li>
                       <li><a href="#">/</a></li>
                       <li><a href="#" class="ds_negevation_inactive Ptactive">Contact List Report</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
           <asp:AsyncPostBackTrigger ControlID="ddlCompany" />
           <asp:AsyncPostBackTrigger ControlID="ddlShiftList" />  
            <asp:AsyncPostBackTrigger ControlID="rblReportType" />         
        </Triggers>
        <ContentTemplate>
            <div class="row Ptrow">

                <div class="employee_box_header PtBoxheader">
                    <h2>Contact List Report</h2>
                </div>
                <div class="employee_box_body">
                    <div class="employee_box_content">

                        <div class="punishment_against">
<h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center">You Have Not Any Access Permission!</h1>
                     <table runat="server" visible="true" id="tblGenerateType"  class="employee_table">
                        <tr id="trCompanyName" runat="server" visible="true">
                            <td>                               
                                Company
                            </td>
                            <td>:</td>
                            <td class="tdWidth">
                                 <asp:DropDownList ID="ddlCompany" runat="server"  AutoPostBack="true" ClientIDMode="Static" CssClass="form-control select_width" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"  >
                                </asp:DropDownList>                           
                            </td>
                           </tr>
                         <tr>
                            <td>
                                Shift
                            </td>
                            <td>:</td>
                            <td class="tdWidth">
                                <asp:DropDownList ID="ddlShiftList" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
                                </asp:DropDownList>  
                            </td>  
                        </tr>   
                         <tr>
                            <td>Employee Type</td>
                            <td>:</td>
                            <td class="tdWidth">
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

                            <td>                               
                                Report Type
                            </td>
                            <td>:</td>
                        <td class="tdWidth">
                            <asp:RadioButtonList  runat="server" ID="rblReportType"  ClientIDMode="Static" RepeatDirection="Horizontal" Font-Bold="true">
                                <asp:ListItem Selected="True" Value="0">Contact List</asp:ListItem>
                                <asp:ListItem Value="1">Emergency Contact List</asp:ListItem>                                 
                            </asp:RadioButtonList>
                        </td>
                          </tr>
                         <tr>
                              <td>Card No / Name
                            </td>
                            <td>:</td>
                            <td class="tdWidth">
                                <asp:DropDownList ID="ddlCardNo" ClientIDMode="Static" CssClass="form-control select_width"  runat="server"></asp:DropDownList>
                            </td>
                         </tr>
                         <tr visible="false" runat="server">
                        <td>Card No</td>
                             <td>:</td>
                             <td>
                                  <asp:TextBox ID="txtCardNo" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>  
                                 <asp:LinkButton ID="lnkNew" Text="New" runat="server" OnClientClick="InputBoxNew()"></asp:LinkButton>                              
                            </td>
                   </tr>       
                  
                  </table>
                                     
                        </div>               
                         <div id="divDepartmentList" runat="server" class="id_card" style="background-color:white; width:61%;">
                            <div class="id_card_left EilistL">
                                <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata EilistCec" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div class="id_card_center EilistC">
                                <table style="margin-top:0px;" class="employee_table">                  
                              <tr>
                                    <td >
                                        <asp:Button ID="btnAddItem" Class="arrow_button" runat="server" Text=">" OnClick="btnAddItem_Click" />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnAddAllItem" Class="arrow_button" runat="server" Text=">>" OnClick="btnAddAllItem_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveItem" Class="arrow_button" runat="server" Text="<" OnClick="btnRemoveItem_Click"   />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveAllItem" Class="arrow_button" runat="server" Text="<<" OnClick="btnRemoveAllItem_Click"  />
                                    </td>
                               </tr>
                        </table>
                    </div>
                     <div class="id_card_right EilistR">
                                <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata EilistCec"  ClientIDMode="Static" runat="server"></asp:ListBox>
                            </div>
                </div>
                        <div class="job_card_button_area">
                            
                            <asp:Button ID="btnPreview" CssClass="css_btn Ptbut" runat="server" ValidationGroup="save"  Text="Preview" OnClick="btnPreview_Click"  />
                            &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/personnel_defult.aspx" CssClass="css_btn Ptbut" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
     <script type="text/javascript">
         $(document).ready(function () {
             $("#ddlCardNo").select2();
             $("#ddlShiftList").select2();

         });
         function loadcardNo() {
             $("#ddlCardNo").select2();
             $("#ddlShiftList").select2();
         }
         function goToNewTabandWindow(url) {
             window.open(url);
             loadcardNo();
         }       
         function InputBoxNew() {

             $('#txtCardNo').val('');
         }
         //function InputBoxNew()
         //{
           
         //    $('#txtCardNo').val('');
         //}
         //function InputValidation()
         //{
         //    if (validateText('txtDate', 1, 100, "Please Select Attendance Date ") == false) return false;
         //    return true;             
         //}
    </script>
</asp:Content>
