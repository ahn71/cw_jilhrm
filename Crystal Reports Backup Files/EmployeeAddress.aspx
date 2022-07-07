<%@ Page Title="Employee Address" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="EmployeeAddress.aspx.cs" Inherits="SigmaERP.personnel.EmployeeAddress" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="/style/jquery-ui-datepekar.css" rel="stylesheet" />
    <link href="/style/dataTables.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--     <div class="row">
                  <div class="col-md-12 ">
               <div class="ds_nagevation_bar">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                        <li> <a href="#">/</a></li>
                        <li> <a href="/personnel_defult.aspx">Personnel</a></li>
                        <li> <a href="#">/</a></li>
                       <li> <a href="#" class="ds_negevation_inactive">Employee address</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>--%>
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
      <div class="employee_main_box Ptmain">
         <div class="employee_box_header PtBoxheader">
         <h2>Employee Address Panel</h2>
     </div>
     <div class="employee_box_body">
         
         <div class="employee_box_content">
     <div class="personal_info_employee" runat="server" id="divEmpAddress">
         <div class="EaddBox">
                 <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                 <Triggers>
                     <asp:AsyncPostBackTrigger ControlID="ddlPreCity" />
                    <asp:AsyncPostBackTrigger ControlID="ddlPerCity" />
                 </Triggers>
                 <ContentTemplate>
                 <div class="em_present_address" style="width: 389px; float: left;">
                     <fieldset class="custom_fieldset_empl" style="float:left; width:387px">
                        <legend>Present Address</legend>
                         <table class="em_present_address_table">
                        <tr >
                            <td>
                                Village
                            </td>
                            <td>
                                :
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtPreVillage" runat="server"  ClientIDMode="Static" CssClass="form-control text_box_width" ></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                গ্রাম
                            </td>
                            <td>
                                :
                            </td>
                            <td  colspan="5">
                                <asp:TextBox ID="txtPreVillageBangla" runat="server"  ClientIDMode="Static" CssClass="form-control text_box_width" Font-Names="SutonnyMJ"></asp:TextBox>
                            </td>
                        </tr>
                              <tr>
                            <td>
                                Post Office
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                               
                               <asp:TextBox ID="txtPrePO" runat="server"  ClientIDMode="Static" CssClass="form-control text_box_width" ></asp:TextBox>
                            </td>
                        </tr>
                             <tr>
                            <td>
                                ডাকঘর 
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                               
                                <asp:TextBox ID="txtPrePOBangla" runat="server"  ClientIDMode="Static" CssClass="form-control text_box_width" Font-Names="SutonnyMJ"></asp:TextBox>
                            </td>
                        </tr>
                             <tr>
                            <td>
                                Post Box
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrePostBox" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>                        
                             <tr>
                            <td>
                                District 
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                               
                                 <asp:DropDownList ID="ddlPreCity" runat="server" ClientIDMode="Static" AutoPostBack="true"  OnSelectedIndexChanged="ddlPreCity_SelectedIndexChanged" CssClass="form-control select_width">
                                                                        
                                </asp:DropDownList>
                            </td>
                        </tr> 
                             <tr>
                            <td>
                                Thana
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                               
                                 <asp:DropDownList ID="ddlPreThana" runat="server" ClientIDMode="Static"  CssClass="form-control select_width"  >
                                                                        
                                </asp:DropDownList>
                            </td>
                        </tr> 
                                                 
                        
                    </table>

                     </fieldset>
                 </div>
                    <%-- <div>
                         <asp:Button  runat="server" ClientIDMode="Static" Text=" >> " ID="btnArow" />
                     </div>--%>
                   
                 <div class="em_permanent_address">
                     <asp:ImageButton  runat="server" ClientIDMode="Static"  ID="ebntSameAddress"  CssClass="image_btn" ImageUrl="~/images/icon/forward.ico" OnClick="ebntSameAddress_Click" />
                     <fieldset class="custom_fieldset_empl" style="float:right; width:380px">
                        <legend>Permanent Address</legend>
                         <table class="em_present_address_table">
                        <tr >
                            <td>
                                Village
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPerVillage" runat="server"  ClientIDMode="Static" CssClass="form-control text_box_width" ></asp:TextBox>
                            </td>
                        </tr>
                        
                        <tr>
                            <td>
                                গ্রাম
                            </td>
                            <td>
                                :
                            </td>
                            <td  colspan="5">
                                <asp:TextBox ID="txtPerVillageBangla" runat="server"  ClientIDMode="Static" CssClass="form-control text_box_width" Font-Names="SutonnyMJ"></asp:TextBox>
                            </td>
                        </tr>
                              <tr>
                            <td>
                                Post Office
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                               
                               <asp:TextBox ID="txtPerPO" runat="server"  ClientIDMode="Static" CssClass="form-control text_box_width" ></asp:TextBox>
                            </td>
                        </tr>
                             <tr>
                            <td>
                                ডাকঘর 
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                               
                                <asp:TextBox ID="txtPerPOBangla" runat="server"  ClientIDMode="Static" CssClass="form-control text_box_width" Font-Names="SutonnyMJ"></asp:TextBox>
                            </td>
                        </tr>
                             <tr>
                            <td>
                                Post Box
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPerPostBox" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>                        
                             <tr>
                            <td>
                                District 
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                               
                                 <asp:DropDownList ID="ddlPerCity" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlPerCity_SelectedIndexChanged"  CssClass="form-control select_width">
                                                                        
                                </asp:DropDownList>
                            </td>
                        </tr> 
                             <tr>
                            <td>
                                Thana
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                               
                                 <asp:DropDownList ID="ddlPerThana" runat="server" ClientIDMode="Static"  CssClass="form-control select_width"  >
                                                                        
                                </asp:DropDownList>
                            </td>
                        </tr> 
                             
                        
                    </table>
                     </fieldset>
                 </div>
                   <table style="margin: 15px auto 0px 226px; width: 352px;">
                   <tr>
                            <td>
                              Mobile No
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobileNo" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                             <tr>
                            <td>
                                Email Address
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmailAddress" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                 </table>
                    <div class="em_emergency_contact">
                     <fieldset>
                        <legend>Emergency Contact</legend>
                         <div class="emergency_contact_left">
                            <table style="margin:15px auto;">
                                 <tr>
                                    <td>
                                        Contact Name
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtContactName" runat="server" ClientIDMode="Static" MaxLength="100" CssClass="form-control text_box_width"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Emp. Relation
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmpRelation" runat="server" ClientIDMode="Static" MaxLength="15" CssClass="form-control text_box_width"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        Job Description
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtJobDescription" runat="server" ClientIDMode="Static" MaxLength="100" CssClass="form-control text_box_width" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                         </table>
                         </div>
                         <div class="emergency_contact_right">
                             <table style="margin:15px auto;">
                                 <tr>
                                    <td>
                                        Address
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmergencyAddress" runat="server" ClientIDMode="Static" MaxLength="100" CssClass="form-control text_box_width"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Mobile No
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmergencyPhoneNo" runat="server" ClientIDMode="Static" MaxLength="15" CssClass="form-control text_box_width"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        Gender
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                       <asp:DropDownList ID="ddlGender" runat="server" ClientIDMode="Static"  CssClass="form-control select_width">
                                           <asp:ListItem >Male</asp:ListItem>
                                           <asp:ListItem >Female</asp:ListItem>
                                           </asp:DropDownList>
                                    </td>
                                </tr>
                                  <tr>
                                    <td>
                                        Age
                                    </td>
                                    <td>
                                        :
                                    </td>
                                    <td>
                                      <asp:TextBox ID="txtAge" runat="server" ClientIDMode="Static" MaxLength="8" CssClass="form-control text_box_width"  ></asp:TextBox>
                                    </td>
                                </tr>
                         </table>
                         </div>
                     </fieldset>
                 </div>
                      </ContentTemplate>
             </asp:UpdatePanel>
         </div>



                 <br />
                 <table class="em_button_table">
                        <tr>                          
                            <th>
                                <asp:Button ID="btnSaveAddress" ClientIDMode="Static" class="emp_btn Ptbut" runat="server" Text="Save" OnClick="btnSaveAddress_Click"  />
                            </th>
                           
                            <th><asp:Button ID="btnEmpAddressclose" ClientIDMode="Static"  class="emp_btn Ptbut" runat="server" Text="Close" OnClick="btnEmpAddressclose_Click"  /></th>
                            <th>
                               
                            </th>
                            <th>
                                 <asp:Button ID="btnNext" ClientIDMode="Static" class="emp_btn Ptbut" runat="server" Text=">>" OnClick="btnNext_Click" ToolTip="Next Page" />
                                
                            </th>
                        </tr>
                    </table>
             <table style="float:right">
                        <tr>
                            
                           
                            
                            
                        </tr>
                    </table>
                     
             </div>
             </div>
         </div>
          </div>
    <script type="text/javascript">
        function goToNewTabandWindowsClose(url) {
            window.open(url);
            window.close();
        }
    </script>
</asp:Content>
