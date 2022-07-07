<%@ Page Title="Shift Manage Report By Date Range" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="shift_manage_reportByDateRange.aspx.cs" Inherits="SigmaERP.personnel.shift_manage_reportByDateRange" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .lbCsss {
            width:390px;
            height:250px;
        }
        .lbAllDiv {
            width:390px;
            float:left;         
            
        }
        .lbSelectedDiv {
            width:390px;
            float:right;         
            
        }
        .lbDiv {
            overflow:hidden;
            height:250px;  
                     
        }
        .divMainBx {
            width:900px;
             margin: 20px auto;   
    background:#c6c6b4;
        }
        .tblArowBx {
            margin-left: 8px;
            margin-top: 54px;
    text-align: center;
    width: 100%;
        }
        .divbtn {
            margin: 20px 0;
            
        }
        .tdWidth {
            width:110px;
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
                    <li><a href="/personnel/roster_index.aspx">Roster Configuration</a></li>
                    <li><a href="#">/</a></li>
                    <li><a href="#" class="ds_negevation_inactive Ptactive">Shift Manage Report By Date Range</a></li>
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
           <asp:AsyncPostBackTrigger ControlID="ddlDepartment" />      
            <asp:AsyncPostBackTrigger ControlID="rblReportType" />       
        </Triggers>
        <ContentTemplate>
             <div class="row Ptrow">
        <div class="employee_box_header PtBoxheader">
                    
                    <h2>Shift Manage Report By Date Range</h2>
                </div>                
                 <div class="employee_box_body">
            <div class="employee_box_content" >
        <div class="punishment_against1">
                <div class="punishment_against">                    
                  
<h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center">You Have Not Any Access Permission!</h1>  
                          <center>  <asp:RadioButtonList runat="server" ID="rblReportType" CssClass="rbl" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblReportType_SelectedIndexChanged" >
                                <asp:ListItem Value="1" Selected="True">Merge Grouping</asp:ListItem>
                                <asp:ListItem Value="0" >Individual Gourping</asp:ListItem>                                
                            </asp:RadioButtonList>     
                              </center>                    
                     <table runat="server" visible="true" id="tblGenerateType"  class="employee_table">                                 
                        <tr id="trCompanyName" runat="server" visible="true">
                            <td>                               
                                Company
                            </td>
                            <td>:</td>
                            <td>
                                 <asp:DropDownList ID="ddlCompany"  runat="server" Width="96%"  AutoPostBack="true" ClientIDMode="Static" CssClass="form-control select_width" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged" >
                                </asp:DropDownList>                           
                            </td>                            
                              </tr> 
                          <tr> 
                              <td>
                                Department
                            </td>
                            <td>:</td>
                            <td>
                                <asp:DropDownList ID="ddlDepartment" runat="server"  Width="96%" AutoPostBack="true" ClientIDMode="Static" CssClass="form-control select_width" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" >
                                </asp:DropDownList>  
                            </td>
  
                        </tr>
                                 <tr>
                           <td runat="server" id="tdDate">                                
                                From Date
                            </td>
                            <td>:</td>
                            <td>
                                  <asp:TextBox ID="txtDate" Style="float:left"  ClientIDMode="Static" Width="39%"  runat="server" CssClass="form-control text_box_width" ></asp:TextBox>
                                <asp:CalendarExtender
                                                ID="TextBoxDate_CalendarExtender" Format="dd-MM-yyyy" runat="server" Enabled="True" TargetControlID="txtDate">
                                            </asp:CalendarExtender>                         
                          <%--  </td>
                                      <td>--%>
                               <p style="float:left; margin-top:10px">To Date :</p> 
                         <%--   </td>
                            <td>:</td>
                             <td>--%>
                                <asp:TextBox ID="txtToDtae" ClientIDMode="Static" Style="float:right; margin-right:10px;width: 39%" runat="server" CssClass="form-control text_box_width" ></asp:TextBox>
                                <asp:CalendarExtender
                                                ID="CalendarExtender1" Format="dd-MM-yyyy" runat="server" Enabled="True" TargetControlID="txtToDtae">
                                            </asp:CalendarExtender>                                 
                             </td>
                                     <td class="tdWidth"> <asp:Button runat="server"  ID="btnSearch" Visible="false" Text="Search"  CssClass="css_btn Ptbut"  OnClick="btnSearch_Click" />
                                         <asp:TextBox runat="server" Font-Bold="true" Style="float:left"   PlaceHolder=" Card No" ID="txtCardNo" ClientIDMode="Static" Width="75px" CssClass="form-control text_box_width"></asp:TextBox> <asp:LinkButton ID="lnkNew" Style="text-decoration:underline;font:bold;color:red; margin-top:5px" Text="New" runat="server" OnClientClick="InputBoxNew()"></asp:LinkButton></td>                          
                        </tr>
                    </table>                            
                        </div>
                       <div  class="id_card" style="background-color:white; width:61%;">
                 <div class="id_card_left EilistL">                        
                        <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata EilistCec" SelectionMode="Multiple"></asp:ListBox>
                    </div>
                           <div class="id_card_center EilistC">
                               <table class="employee_table">
                                   <tr>
                                       <td>
                                           <asp:Button ID="btnAddItem" Class="arrow_button" runat="server" Text=">" OnClick="btnAddItem_Click" />
                                       </td>
                                   </tr>
                                   <tr>
                                       <td>
                                           <asp:Button ID="btnAddAllItem" Class="arrow_button" runat="server" Text=">>" OnClick="btnAddAllItem_Click" />
                                       </td>
                                   </tr>
                                   <tr>
                                       <td>
                                           <asp:Button ID="btnRemoveItem" Class="arrow_button" runat="server" Text="<" OnClick="btnRemoveItem_Click" />
                                       </td>
                                   </tr>
                                   <tr>
                                       <td>
                                           <asp:Button ID="btnRemoveAllItem" Class="arrow_button" runat="server" Text="<<" OnClick="btnRemoveAllItem_Click" />
                                       </td>
                                   </tr>
                               </table>
                    </div>
                   <div class="id_card_right EilistR">                        
                          <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata EilistCec"  ClientIDMode="Static" runat="server"></asp:ListBox>
                    </div>
                   </div>                        
                         <div class="import_data_footer_left" style="margin-top: 20px; height: 10px; width: 43%;">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static">
                            <ProgressTemplate>
                                <span style="font-family: 'Times New Roman'; font-size: 20px; margin-top: -14px; color: green; font-weight: bold; float: left">
                                    <p>Wait Generating&nbsp;</p>
                                </span>
                                <img style="width: 26px; height: 26px; cursor: pointer; float: left; margin-top: -13px;" src="/images/wait.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                       </div>
                       <%-- <div class="divbtn">                            
                           &nbsp; &nbsp;  
                            &nbsp; &nbsp; &nbsp;
                    
                        </div>--%>
            <table style="width:auto;margin-top:10px">
                <tr>
                    <td><asp:Button ID="btnPreview" CssClass="css_btn Ptbut" runat="server" ValidationGroup="save" OnClientClick="return InputValidation();" Text="Preview" OnClick="btnPreview_Click"/></td><td><asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn Ptbut" /></td>
                </tr>
            </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
     <script type="text/javascript">
         function goToNewTabandWindow(url) {
             window.open(url);
         }
         function InputBoxNew() {

             $('#txtCardNo').val('');
         }
    </script>
</asp:Content>
