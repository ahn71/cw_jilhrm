<%@ Page Title="Employee Personal" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="EmployeePersonal.aspx.cs" Inherits="SigmaERP.personnel.EmployeePersonal" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/style/jquery-ui-datepekar.css" rel="stylesheet" />
    <link href="/style/dataTables.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
<div class="container" style="padding:0;margin-top:25px;max-width:980px;">
    <div class="row">
   <div class="col-lg-12">
       <div class="employee_box_header personal_color_header">
         <h2 style="float:none"><label id="lblEmpFormType" for="Employee">Personal Details</label></h2>
     </div>
     <div class="employee_box_body personal_color_body">
         
         <div class="employee_box_content">
     <div class="em_personal_info" id="divEmpPersonnelInfo" >
                   <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                 <Triggers>
                     
                 </Triggers>
                 <ContentTemplate>
                    <div class="row">
                    <div class="col-lg-2"></div>
                    <div class="col-lg-8">
                    <table class="em_personal_info_table">
                        <tr>
                            <td>
                                Father's/Husband's Name
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="dsFatherName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Mother's Name
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="dsMotherName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                পিতার নাম
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="dsFatherNameBn" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Font-Names="SutonnyMJ"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                মাতার নাম
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="dsMotherNameBN" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Font-Names="SutonnyMJ"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Marital Status
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                
                                <asp:DropDownList ID="dsMaritialStatus" runat="server" ClientIDMode="Static"  CssClass="form-control select_width">
                                      <asp:ListItem>Single</asp:ListItem> 
                                    <asp:ListItem>Married</asp:ListItem> 
                                    <asp:ListItem>Widow</asp:ListItem> 
                                    <asp:ListItem>Divorced</asp:ListItem>                                  
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Data of Birth
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                
                                <asp:TextBox ID="dsDateOfBirth" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender5" Format=d-M-yyyy runat="server" TargetControlID="dsDateOfBirth"></asp:CalendarExtender>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Place of Birth
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="dsPlaceOfBirth" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Height
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="dsHeight" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Weight
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="dsWeight" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Blood Group
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                
                                <asp:DropDownList ID="dsBloodGroup" runat="server" ClientIDMode="Static"  CssClass="form-control select_width">
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
                        <tr>
                            <td>
                                Sex
                            </td>
                            <td>
                                :
                            </td>
                            <td>

                                <asp:DropDownList ID="dsSex" runat="server" ClientIDMode="Static"  CssClass="form-control select_width">
                                        <asp:ListItem>Male</asp:ListItem> 
                                    <asp:ListItem>Female</asp:ListItem>                                
                                </asp:DropDownList>
                            </td>
                        </tr>
                         <tr>
                            <td>
                              Number of Child
                            </td>
                            <td>
                                :
                            </td>
                            <td>

                                <asp:TextBox ID="txtNumberofchild" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width">0</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Religion
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                
                                <asp:DropDownList ID="dsReligion" runat="server" ClientIDMode="Static"  CssClass="form-control select_width">
                                                                        
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Last Educational Qualifi
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLastEdQualification" runat="server" ClientIDMode="Static"  CssClass="form-control select_width">                                                                        
                                </asp:DropDownList>                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Total No of Experience
                            </td>
                            <td>
                                :
                            </td>
                            <td>

                                <asp:TextBox ID="dsNoOfExperience" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Nationality
                            </td>
                            <td>
                                :
                            </td>
                            <td>

                                <asp:TextBox ID="dsNationality" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width">Bangladeshi</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                National ID Card No
                            </td>
                            <td>
                                :
                            </td>
                            <td>

                                <asp:TextBox ID="dsNationIDCardNo" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                 </ContentTemplate>
             </asp:UpdatePanel>
                     <table class="em_button_table">
                        <tr>
                            
       
                           
                            <th>
                                <asp:Button ID="btnSavePersonal" ClientIDMode="Static" class="emp_btn" runat="server" Text="Save" OnClick="btnSavePersonal_Click"  />
                            </th>
                           
                            <th><asp:Button ID="btndivClose" ClientIDMode="Static" class="emp_btn" runat="server" Text="Close" OnClick="btndivClose_Click"  /></th>
                            
                        </tr>
                    </table>
                <div class="col-lg-2">
               </div> 
               </div>
               
         </div>
         </div>
            </div>
          </div>
        </div>
    </div>
    <script type="text/javascript">
        function CloseWindowt() {
            window.close();
        }
        function goToNewTabandWindowsClose(url) {
            window.open(url);
            window.close();
        }
    </script>
</asp:Content>
