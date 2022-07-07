<%@ Page Title="Employee" Language="C#" MasterPageFile="~/personnel_NestedMaster.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="employee.aspx.cs" Inherits="SigmaERP.personnel.Employee" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">     
    <link href="/style/dataTables.css" rel="stylesheet" />
    <style>
        #ContentPlaceHolder1_MainContent_rblPunchType {
             margin-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>   
    
    <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdfSalaryType" Value="Gross" runat="server" ClientIDMode="Static" />
     <asp:HiddenField ID="hdfsaveupdatestatus" Value="Save" runat="server" ClientIDMode="Static"  />
     <asp:HiddenField ID="hdfBasic" Value="0" runat="server" ClientIDMode="Static"  />
    <asp:HiddenField ID="hdfMedical" Value="0" runat="server" ClientIDMode="Static"  />
    <asp:HiddenField ID="hdfhouserent" Value="0" runat="server" ClientIDMode="Static"  />
    <asp:HiddenField ID="hdfConveyance" Value="0" runat="server" ClientIDMode="Static"  />
     <asp:HiddenField ID="hdfpresentsalryKeypress" Value="0"  runat="server" ClientIDMode="Static"  />
    <asp:HiddenField ID="upSave" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upupdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="updelete" runat="server" ClientIDMode="Static" />
     <asp:HiddenField ID="upSuperAdmin" runat="server" ClientIDMode="Static" />    
    <asp:HiddenField ID="hdfEmpId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdfUserType" Value="1" runat="server" ClientIDMode="Static" />

    <div class="row Rrow">
        <div class="ds_nagevation_bar">
            <ul>
                <li><a href="/default.aspx">Dashboard</a></li>
                <li> <a href="#">/</a></li>
                <li> <a href="/personnel_defult.aspx">Personnel</a></li>
                <li> <a href="#">/</a></li>
                 <li> <a href="/personnel/employee_index.aspx">Employee Information</a></li>
                <li> <a href="#">/</a></li>
                <li> <a href="#" class="ds_negevation_inactive Ptactive">Employees Entry</a></li>
            </ul>               
        </div>
       </div>
   
    <div class="row Ptrow">
        
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                <asp:AsyncPostBackTrigger ControlID="btnNew" />
            </Triggers>
            <ContentTemplate>
                <div class="employee_box_header PtBoxheader">
                    <h2>
                        <asp:Label ID="Label1" Text="Official Details" runat="server" ClientIDMode="Static"></asp:Label>
                    </h2>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        

        <div class="employee_box_body Ptbody">
            <div class="employee_box_content" style="overflow:initial;">
                <div id="divEmpInfo">
                    <div class="row">
                    <div class="col-lg-5">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlBranch" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDesingnation" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDepartment" />
                            <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                            <%-- <asp:AsyncPostBackTrigger ControlID="ddlEmpType" />--%>
                            <asp:AsyncPostBackTrigger ControlID="btnFindEmployee" />
                           <%-- <asp:AsyncPostBackTrigger ControlID="btnSave" />--%>
                            <asp:AsyncPostBackTrigger ControlID="btnNew" />
                            <asp:AsyncPostBackTrigger ControlID="chkAlternativeEmpCard" />
                            <asp:AsyncPostBackTrigger ControlID="rblPunchType" />
                        </Triggers>
                        <ContentTemplate>
                             <asp:HiddenField ID="hdfCardnoDigits" Value="0" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdfCardnoDigitsSet" Value="0" runat="server" ClientIDMode="Static" />
                            <div class="employee_box_left">
                                <table class="employee_table">
                                    <tr id="trBranch" runat="server">
                                        <td>Branch <span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBranch" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Emp Type <span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblEmpType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rbLanguage_SelectedIndexChanged">
                                               
                                            </asp:RadioButtonList>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                           <asp:CheckBox  Text="Is Authorized?" runat="server" ID="ckbAuthorized"  />

                                        </td>
                                    </tr>
                                    <tr>

                                        <td>Salary Type <span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblSalaryType" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="Scale" Text="Scale" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="Gross" Text="Gross"></asp:ListItem>
                                                <asp:ListItem Value="Gross Scale" Text="Gross Scale"></asp:ListItem>
                                            </asp:RadioButtonList>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Full Name <span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Nick  Name <span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNickName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>নাম <span class="requerd1">*</span></td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="txtNameBangla" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Font-Names="SutonnyMJ"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Department <span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDepartment" runat="server" ClientIDMode="Static" CssClass="form-control select_width" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td>Designation <span class="requerd1">*</span>
                                        </td> 
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDesingnation" runat="server" ClientIDMode="Static" CssClass="form-control select_width" OnSelectedIndexChanged="ddlDesingnation_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Group <span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGrouping" runat="server" ClientIDMode="Static" CssClass="form-control select_width" ></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Emp Card No <span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox   ID="txtEmpCardNo" Style="float: left; " CssClass="form-control text_box_width" Visible="false" runat="server" MaxLength="15"></asp:TextBox><asp:DropDownList ID="ddlEmpCardNo" runat="server" ClientIDMode="Static" Style="margin-right: 0px; float: left" CssClass="form-control select_width" AutoPostBack="True" OnSelectedIndexChanged="ddlEmpCardNo_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td style="white-space: nowrap;">Alternate Card
                                        </td>
                                        <td>:
                                        </td>
                                        <td style="white-space: nowrap;">
                                            <asp:TextBox ID="txtAlternativeCard" Style="float: left; width: 110px;" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width_2" MaxLength="5" Enabled="False"></asp:TextBox>&nbsp;
                                            <asp:FilteredTextBoxExtender ID="F1" runat="server" FilterType="Numbers" 
                                TargetControlID="txtAlternativeCard" ValidChars=""></asp:FilteredTextBoxExtender>
                                            <asp:CheckBox ID="chkAlternativeEmpCard" Style="line-height: 3;" ClientIDMode="Static" CssClass="empchkLunch" Text="Active" Checked="false" runat="server" OnCheckedChanged="chkAlternativeEmpCard_CheckedChanged" AutoPostBack="True" />
                                        </td>
                                    </tr>
                                   
                                       <tr>
                                        <td>Proximity No
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                         <asp:TextBox   ID="txtProximityNo" ClientIDMode="Static"   CssClass="form-control text_box_width"  style="float:left; width:110px" runat="server" MaxLength="10"></asp:TextBox>
                                             <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" 
                                TargetControlID="txtProximityNo" ValidChars=""></asp:FilteredTextBoxExtender>   
                                             <asp:CheckBox runat="server" ID="ckbProximityChange" style="float:left;padding:9px" ToolTip="Click this box to change proximity no." Enabled="false"  AutoPostBack="true" OnCheckedChanged="ckbProximityChange_CheckedChanged" />  
                                                                                
                                            <asp:RadioButtonList ID="rblPunchType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblPunchType_SelectedIndexChanged" >
                                                 <asp:ListItem Value="0" Selected="True">Finger Or Face</asp:ListItem>
                                               <asp:ListItem Value="1" >Proximity</asp:ListItem>                                                
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                       <tr runat="server" visible="false" id="trProximityChangeDate">
                                        <td>
                                            Change Date
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                             <asp:TextBox ID="txtProximityChangeDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" Format="dd-MM-yyyy" OnClientDateSelectionChanged="dateSelectionChanged"  runat="server" TargetControlID="txtProximityChangeDate"></asp:CalendarExtender>
                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Emp Status
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlEmpStatus" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Emp Shift
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:DropDownList ID="ddlShift" runat="server" ClientIDMode="Static" CssClass="form-control select_width"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Type
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:DropDownList ID="ddlType" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
                                                <asp:ListItem>Probationary</asp:ListItem>
                                                <asp:ListItem>Permanent</asp:ListItem>
                                                
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Duty Type
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblDutyType" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="Regular" Text="Regular" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="Roster" Text="Roster"></asp:ListItem>                                               
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Ordering
                                        </td>
                                        <td>:
                                        </td>
                                        <td>                                            
                                             <asp:TextBox ID="txtDptWise"  Font-Bold="true" runat="server" ClientIDMode="Static" style="width:39%;float:left;color:red;text-align:center" MaxLength="4" ToolTip="Dpertment Wise Custom Ordering"  placeholder="Dpt Wise" CssClass="form-control text_box_width"></asp:TextBox>
                                            <asp:CheckBox runat="server" ID="cskDptWise" AutoPostBack="true" style="float:left;padding-left: 2px;padding-right: 2px;padding-top: 8px" ToolTip="Dpertment Wise Custom Ordering"  OnCheckedChanged="cskDptWise_CheckedChanged" />
                                            <asp:TextBox ID="txtFlatOrder" Font-Bold="true" runat="server" ClientIDMode="Static" style="width:39%;float:left; color:red; text-align:center" MaxLength="4" ToolTip="Flat Custom Ordering" placeholder="Flat" CssClass="form-control text_box_width"></asp:TextBox>
                                            <asp:CheckBox runat="server"  ID="cskFlatOrder" AutoPostBack="true" ToolTip="Flat Custom Ordering"  style="float:left;  padding-left: 2px;padding-top: 8px" OnCheckedChanged="cskFlatOrder_CheckedChanged" />
                                        </td>
                                    </tr>

                                       <tr>
                                        <td>TIN
                                        </td>
                                        <td>:
                                        </td>
                                        <td>                                            
                                             <asp:TextBox ID="txtTIN"  Font-Bold="true" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                            
                                        </td>
                                    </tr>
                                    
                                   
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>
                    <div class="col-lg-5">
                    <div class="employee_box_center">
                       
                                <div>
                                     <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <Triggers>
                               <%-- <asp:AsyncPostBackTrigger ControlID="btnSave"/>--%>
                                <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                                <%--<asp:AsyncPostBackTrigger ControlID="ddlEmpType" />--%>

                                <asp:AsyncPostBackTrigger ControlID="btnNew" />
                                <asp:AsyncPostBackTrigger ControlID="btnFindEmployee" />
                            </Triggers>
                            <ContentTemplate>
                                <table class="employee_table" style="height: 578px;">
                                     <tr>
                                        <td>Joining Date <span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtJoiningDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender4" Format="d-M-yyyy" OnClientDateSelectionChanged="dateSelectionChanged"  runat="server" TargetControlID="txtJoiningDate"></asp:CalendarExtender>
                                            
                                        </td>
                                    </tr>
                                     <tr id="trtxtExpireDate" runat="server">
                                        <td>Expire Date
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtExpireDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender6" Format="d-M-yyyy" runat="server" TargetControlID="txtExpireDate"></asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:175px;">Father's/ Husband's Name
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="dsFatherName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mother's Name
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="dsMotherName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Marital Status
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:DropDownList ID="dsMaritialStatus" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
                                                <asp:ListItem>Single</asp:ListItem>
                                                <asp:ListItem>Married</asp:ListItem>
                                                <asp:ListItem>Widow</asp:ListItem>
                                                <asp:ListItem>Divorced</asp:ListItem>
                                            </asp:DropDownList>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Date of Birth
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="dsDateOfBirth" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender5" Format="d-M-yyyy" runat="server" TargetControlID="dsDateOfBirth"></asp:CalendarExtender>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Place of Birth
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="dsPlaceOfBirth" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Height
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="dsHeight" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Weight
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="dsWeight" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Blood Group
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:DropDownList ID="dsBloodGroup" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
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
                                        <td>Gender
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:DropDownList ID="dsSex" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
                                                <asp:ListItem>Male</asp:ListItem>
                                                <asp:ListItem>Female</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Religion
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:DropDownList ID="dsReligion" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
                                            </asp:DropDownList>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Last Educational Qualifi
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlLastEdQualification" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Total No of Experience
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="dsNoOfExperience" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Nationality
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="dsNationality" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width">Bangladeshi</asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>National ID/Birth ID No
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="dsNationIDCardNo" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox>
                                        </td>
                                    </tr>
                                   
                                    <tr id="trel" runat="server" visible="false">
                                        <td>Earned Leave
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="txtEarnedLeave" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width">0</asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr id="tresd" runat="server" visible="false">
                                        <td>El Start Date
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="txtElStart" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender3" Format="d-M-yyyy" runat="server" TargetControlID="txtElStart"></asp:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                                </ContentTemplate>
                        </asp:UpdatePanel> 
                                    
                                </div>                                                   
                    </div>
                    </div>
                    <div class="col-lg-2">
                    <div class="employee_box_right">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <%--<asp:AsyncPostBackTrigger ControlID="btnSave" />--%>
                                <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                                <asp:AsyncPostBackTrigger ControlID="btnNew" />
                                <asp:AsyncPostBackTrigger ControlID="btnFindEmployee" />
                            </Triggers>
                            <ContentTemplate>
                                <div>
                                    <asp:Image ID="imgProfile" class="profileImage" ClientIDMode="Static" runat="server" ImageUrl="~/images/profileImages/noProfileImage.jpg" />
                                    <asp:FileUpload ID="FileUpload1" Style="margin-top: 20px;" runat="server" onchange="previewFile()" ClientIDMode="Static" />
                                </div>


                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <%--<asp:AsyncPostBackTrigger ControlID="btnSave" />--%>
                                <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                                <asp:AsyncPostBackTrigger ControlID="btnNew" />
                            </Triggers>
                            <ContentTemplate>
                                <div style="width: 100%;">
                                    <asp:Image ID="imgSignature" class="profileImage_2" ClientIDMode="Static" runat="server" ImageUrl="~/images/profileImages/Signature.jpg" />
                                    <asp:FileUpload ID="FileUpload2" Style="margin-top: 20px;" runat="server" onchange="previewFilesignature()" ClientIDMode="Static" />
                                </div>
                                 <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSave" />
                                <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                                <asp:AsyncPostBackTrigger ControlID="btnNew" />
                                <asp:AsyncPostBackTrigger ControlID="rblEmpType" />
                            </Triggers>

                            <ContentTemplate>
                                    <div class="emp_right_area" style="padding-top: 10px; padding-left: 0px">
                                        <table class="emp_right_table">
                                            <tbody>
                                                <tr>
                                                    <asp:Button ID="btnNew" Class="css_btn Ptbut" runat="server" Text="New" OnClick="btnNew_Click" />
                                                    <asp:Button Visible="false" ID="btnListAll" Class="css_btn Ptbut" runat="server" Text="List All"  PostBackUrl="~/personnel/employee_list.aspx" />
                                                    <asp:Button ID="btnSave" CssClass="css_btn Ptbut" ClientIDMode="Static" runat="server" OnClientClick="return InputValidation();" Text="Save" OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnClose" Class="css_btn Ptbut" runat="server" Text="Close" PostBackUrl="~/personnel_defult.aspx" />
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                <div style="margin-top: 20px;display:none;" id="divUserType" runat="server">
                                    <fieldset id="fs" runat="server" visible="true" style="margin-right: 55px">
                                        <legend>User Type</legend>
                                        <table class="employee_table">
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList Style="height: 110px" Width="110px" ID="rblusertype" runat="server" RepeatColumns="1">
                                                        <asp:ListItem Value="5">User</asp:ListItem>
                                                        <asp:ListItem Value="1">Viewer</asp:ListItem>
                                                        <asp:ListItem Value="2">Admin</asp:ListItem>
                                                        <asp:ListItem Value="3">Super Admin</asp:ListItem>
                                                        <asp:ListItem Value="4">Master Admin</asp:ListItem>                                                        
                                                    </asp:RadioButtonList>

                                                </td>

                                            </tr>

                                        </table>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblUserName" ForeColor="Black" Text="User name" Font-Bold="true"></asp:Label>
                                                </td>
                                                <td style="font: bold; font-size: 18px">:</td>
                                                <td>
                                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblUser" ForeColor="Red"   font-size="16px" Font-Bold="True" Font-Names="Times New Roman"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblPassword" ForeColor="Black" Text="Password" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td style="font: bold; font-size: 18px">:</td>
                                                <td>
                                                    <asp:Label runat="server" ClientIDMode="Static" ID="lblPass" ForeColor="Green" Font-Bold="True" Font-Size="18px" Font-Names="Times New Roman"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                                </ContentTemplate>
                        </asp:UpdatePanel>
                            <div style="margin-top: 14px" class="em_right_button_area">
                            <asp:Button Visible="false" ID="btnPersonalInfo" runat="server" ClientIDMode="Static" CssClass="em_right_button" Text="Personal Details" OnClientClick="return divEmpInfoHide();" OnClick="btnPersonalInfo_Click" /><br />
                            <asp:Button ID="btnEmployeeAd" runat="server" CssClass="em_right_button" OnClientClick="return divEmpAddress();" Text="Employee's Address" OnClick="btnEmployeeAd_Click" /><br />
                            <asp:Button ID="btnExperience" runat="server" CssClass="em_right_button" OnClientClick="return divEmpExperienceList();" Text="Experience" OnClick="btnExperience_Click" /><br />
                            <asp:Button ID="btnEducation" runat="server" CssClass="em_right_button" OnClientClick="return divEducationList();" Text="Education" OnClick="btnEducation_Click" /><br />
                            <asp:Button Visible="false" ID="btnFindEmployee" runat="server" CssClass="em_right_button" Text="Find Employee" OnClick="btnFindEmployee_Click" />
                            <asp:TextBox Visible="false" ID="txtCardNo" PLaceHolder="Type Card No" runat="server" style="color: Green; font-weight: bold; text-align: center; width: 181px; border-radius: 0px;" ClientIDMode="Static" CssClass="form-control text_box_width_import"></asp:TextBox>

                        </div>
                                 </ContentTemplate>
                        </asp:UpdatePanel>                           
                       
                    </div>
                    </div>
                    </div>

                 </div>
                </div>
            </div>

            
        </div>
            
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ddlEmpCardNo").select2();
            var expiredate = $('#txtExpireDate').val();
            if (expiredate == "") {
                AddYear();
            }
            $(document).on("keypress", "body", function (e) {
                if (e.keyCode == 13) e.preventDefault();
                // alert('deafault prevented');

            });
        });

        function load() {
            $("#ddlEmpCardNo").select2();
        }
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
        function previewFilesignature() {
            try {
                var preview = document.querySelector('#imgSignature');
                var file = document.querySelector('#FileUpload2').files[0];

                var reader = new FileReader();

                reader.onloadend = function () {
                    preview.src = reader.result;
                }

                if (file) {
                    reader.readAsDataURL(file);
                } else {
                    preview.src = "";
                }
               
            }
            catch (exception) {
                lblMessage.innerText = exception;
            }

        }
        //function EmpType() {
            
        //    if ($('#rblEmpType').select().text == "Worker") {
        //        $("#divUserType").hide();
        //        alert('ok');
        //    }
        //    else {
        //        $("#divUserType").hide();
        //        alert('ok');
        //    }
        //}
        function totalSalaryCalculation(e) {
            try {
               
            }
            catch (exception) {

            }
        }
        function InputValidation() {
            try {                
                if ($('#txtName').val().trim().length==0) {
                    showMessage("warning->Please Type Employee Name ");
                    $('#txtName').focus();
                    return false;
                }
                if ($('#txtNickName').val().trim().length == 0) {
                    showMessage("warning->Please Type Nick Name ");
                    $('#txtNickName').focus();
                    return false;
                }
               if ($('#ddlDepartment option:selected').text().length == 0) {
                    showMessage("warning->Please Select Department ");
                    $('#ddlDepartment').focus();
                    return false;
               }
              
               
                if ($('#ddlDesingnation option:selected').text().length == 0) {
                    
                    showMessage("warning->Please Select Desingnation ");
                    $('#ddlDesingnation').focus();
                    return false;
                }
                if ($('#ddlGrouping option:selected').text().length == 0) {                   
                    showMessage("warning->Please Select Line/Group ");
                    $('#ddlGrouping').focus();
                    return false;
                }
                if ($('#chkAlternativeEmpCard').attr('checked')) {
                    var value = $('#hdfCardnoDigitsSet').val();
                  
                    if ($('#txtAlternativeCard').val().trim().length < value) {
                        showMessage("warning->Please Type Alternative Card No (Minimum" + value + " Digits) ");
                        $('#txtAlternativeCard').focus();
                        return false;
                    }
                }
                //alert($('#rblPunchType input:checked').val());
                if ($('#ContentPlaceHolder1_MainContent_rblPunchType input:checked').val() == '1' && $('#txtProximityNo').val().trim().length != 10) {
                    showMessage("warning->Please Type Proximity No Must be 10 Digits ");
                    $('#txtProximityNo').focus();
                    return false;
                }

                if ($('#ContentPlaceHolder1_MainContent_ckbProximityChange').attr('checked')) {
                   
                    if ($('#txtProximityChangeDate').val().trim().length < 8) {
                        showMessage("warning->Please Type Valid Proximity Change Date ! ");
                        $('#txtProximityChangeDate').focus();
                        return false;
                    }
                }
                //if ($('#ContentPlaceHolder1_MainContent_txtEmpCardNo').val().trim().length < 8) {
                //    showMessage("warning->Employee Card Number Must be 8 Number ");
                //    $('#ContentPlaceHolder1_MainContent_txtEmpCardNo').focus();
                //    return false;
                //}
               
                if ($('#ddlEmpStatus option:selected').text().length == 0) {

                    showMessage("warning->Please Select Employee Status ");
                    $('#ddlEmpStatus').focus();
                    return false;
                }
                if ($('#ddlShift option:selected').text().length == 0) {

                    showMessage("warning->Please Select Shift ");
                    $('#ddlShift').focus();
                    return false;
                }      
               
                if ($('#txtJoiningDate').val().trim().length <8) {
                    showMessage("warning->Please Select Joining Date");
                    $('#txtJoiningDate').focus();
                    return false;
                }
                if ($('#txtExpireDate').val().trim().length < 8) {
                    showMessage("warning->Please Select Expire Date");
                    $('#txtExpireDate').focus();
                    return false;
                }

                           
               
                return true;
              

            }
            catch (exception) {

            }
        }

        function divEmpInfoHide() {
           
            if ($('#ddlEmpCardNo option:selected').text().length == 0) {
                showMessage('Please Select Employee Card No', 'warning');
                //lblMessage.innerText = 'Please Select Employee Card No';
                return false;
            }
            var EmpId = $('#ddlEmpCardNo option:selected').val();
            jx.load('/ajax.aspx?id=' + EmpId + '&todo=Religion', function (data) {

                $('#ddlRligion').html(data);
            });
            jx.load('/ajax.aspx?id=' + EmpId + '&todo=PersonalInfo', function (data) {
                if (data == "0") return;
                var dataSplited = data.split("#*");
                dataSplited.forEach(function (ds) {
                    var dst = ds.split("_")
                    $('#ds' + dst[0]).val(dst[1]);
                });

            });
           
            return true;
        }
        function divEmpAddress() {
            if ($('#ddlEmpCardNo option:selected').text().length == 0) {
                showMessage('Please Select Employee Card No', 'warning');
                //lblMessage.innerText = 'Please Select Employee Card No';
                return false;
            }
           
            return true;

        }
        function ShowdivEmpAddress() {
            $('#divEmpInfo').hide();
            $('#div_emp_save').hide();
            $('#divEmpAddress').show();
            $('#divEmpPersonnelInfo').hide();
        }
        function AllHidewithoutEmployeediv() {
            $('#divEmpInfo').show();
            $('#div_emp_save').show();
            $('#divEmpAddress').hide();
            $('#divEmpPersonnelInfo').hide();
            $('#divEmpExperience').hide();
            $('#divEmpEducation').hide();
        }
        function divEmpExperienceList() {
            if ($('#ddlEmpCardNo option:selected').text().length == 0) {
                showMessage('Please Select Employee Card No','warning');
                return false;
            }
           
            return true;
        }
      
        $('#ContentPlaceHolder1_btnFindEmployee').click(function () {
            
            if ($('#ddlEmpType option:selected').text().length == 0) {
                showMessage('Please Select Employee Type', 'warning');
                return;
            }
        });
        function ShowdivEmpExperience() {
            $('#divEmpInfo').hide();
            $('#div_emp_save').hide();
            $('#divEmpAddress').hide();
            $('#divEmpExperience').show();
        }
        function divEducationList() {
            if ($('#ddlEmpCardNo option:selected').text().length == 0) {
                showMessage('Please Select Employee Card No', 'warning');
                return false;
            }
           
            return true;
        }
        function ShowDivEducation(){
            $('#divEmpInfo').hide();
            $('#div_emp_save').hide();
            $('#divEmpExperience').hide();
            $('#divEmpEducation').show();
        }
        function LoadEmpExperienceList() {
            var EmpId = $('#ddlEmpCardNo option:selected').val();
            jx.load('/ajax.aspx?id=' + EmpId + '&todo=LoadEmployeeExperience', function (data) {
                $('#divExperienceList').html(data);
            });
        }
            function getSlabStatus(checkbox) {
              
                // var a = ($('#rdbSlabA').is(':checked')) ? true : false;
                var id = checkbox.id;

                var famount = checkbox.value;
                var status = checkbox.checked;
                alert(status);
                if (status) {

                }
            }
          
            function divClose() {
               
                $('#divEmpInfo').show();
                $('#div_emp_save').show();
                $('#').hide();
            }
                function EmpAddressdivClose() {
                    $('#divEmpPersonnelInfo').hide();
                    $('#divEmpAddress').show();
                    $('#divEmpExperience').hide();
                    $('#divEmpEducation').hide();
                    $('#divEmpInfo').hide();
                    $('#div_emp_save').hide();

                }
                function EmpExperienceListClose() {
                    $('#divEmpPersonnelInfo').hide();
                    $('#divEmpExperience').hide();
                    $('#divEmpEducation').hide();
                    $('#divEmpInfo').show();
                    $('#div_emp_save').show();
                    $('#btnSaveEducation').val('Save');
                    $('#hdfeducation').val('');
                    $('#btnSaveExperience').val('Save');
                    $('#hdfexperience').val('');
                    $('#txtCompanyName').val('');
                    $('#txtDesignation').val('');
                    $('#txtResponsibility').val('');
                    $('#txtYearOfExp').val('');
                    $('#txtJoiningDateExperience').val('');
                    $('#txtResignDate').val('');
                    $('#txtSpecialQualification').val('');
                    $('#txtDegree').val('');
                    $('#txtYear').val('');
                    $('#txtInstitute').val('');
                    $('#txtResult').val('');

                }
       
                function editEmpExperience(id) {
                    $('#txtCompanyName').val($('#r_' + id + ' td:first').html());
                    $('#txtDesignation').val($('#r_' + id + ' td:nth-child(2)').html());
                    $('#txtResponsibility').val($('#r_' + id + ' td:nth-child(3)').html());
                    $('#txtYearOfExp').val($('#r_' + id + ' td:nth-child(4)').html());
                    $('#txtJoiningDateExperience').val($('#r_' + id + ' td:nth-child(5)').html());
                    $('#txtResignDate').val($('#r_' + id + ' td:nth-child(6)').html());
                    $('#txtSpecialQualification').val($('#r_' + id + ' td:nth-child(7)').html());
                    $('#btnSaveExperience').val('Update');
                    $('#hdfexperience').val(id);

                }
                function editEducation(id) {
                    $('#txtDegree').val($('#r_' + id + ' td:first').html());
                    $('#txtYear').val($('#r_' + id + ' td:nth-child(2)').html());
                    $('#txtInstitute').val($('#r_' + id + ' td:nth-child(3)').html());
                    $('#txtResult').val($('#r_' + id + ' td:nth-child(4)').html());
                    $('#btnSaveEducation').val('Update');
                    $('#hdfeducation').val(id);
                }
                function ShowPersonalInfo() {
                    $('#divEmpInfo').hide();
                    $('#div_emp_save').hide();
                    $('#divEmpPersonnelInfo').show();
            

                }
                function ShowAllEmployeeList() {
                    $('#divEmpInfo').hide();
                    $('#div_emp_save').hide();
                    $('#divEmpAllList').show();

                }
                function CardValidate() {
                    showMessage("This Employee Card is used by another person", "warning");
                    load();
                }
                function ProximityNoValidate() {
                    showMessage("This Proximity No is used by another person", "warning");
                    load();
                }
                function SaveSuccess() {
                    showMessage("Successfully saved", "success");
                    load();
                }
                function UnableSave() {
                    showMessage("Unable to save", "error");
                    load();
                }
                function UpdateSuccess() {
                    showMessage("Successfully Updated", "success");
                    load();
                }
                function UnableUpdate() {
                    showMessage("Unable to Update", "error");
                    load();
                }
                function usertype() {
                    $('#hdfUserType').val('0');
                }
                //function SuperAdminPrivilige() {
                //    showMessage('Super Admin No Permission', 'warning');
                //}
                function AddYear()
                {
                   
                    var JoinDate = $('#txtJoiningDate').val();
                    if (JoinDate == "") return;
                    var parts = JoinDate.split('-');
                    var day = parts[0];
                    var month = parts[1];
                    var year = parts[2];
                    if (day < 10) day = "0" + day;
                    if (month < 10) month = "0" + month;
                    var mydate = new Date(month+"/"+day+"/"+year);
                    var date = formatDate(mydate)
                    $('#txtExpireDate').val(date);                    
                }
                function formatDate(date) {
                    var day = date.getDate();
                    var month = date.getMonth() + 1;
                    var year = date.getFullYear() + 1;

                    if (day < 10) day = "0" + day;
                    if (month < 10) month = "0" + month;

                    return day + "-" + month + "-" + year;
                }
                function dateSelectionChanged(sender, args) {
                    selectedDate = sender.get_selectedDate();

                    var date = formatDate(selectedDate)
                    $('#txtExpireDate').val(date);
                   
                }               
        </script>
                
          
</asp:Content>              