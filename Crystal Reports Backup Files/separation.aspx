<%@ Page Title="Separation"  Language="C#" MasterPageFile="~/personnel_NestedMaster.Master" AutoEventWireup="true" CodeBehind="separation.aspx.cs" Inherits="SigmaERP.personnel.separation" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    
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
        /*#ContentPlaceHolder1_MainContent_gvSeparationList th, td {
            text-align:center;
        }*/

        #td1{
        min-width:400px;
        }
        #ContentPlaceHolder1_MainContent_tabSeperationPanel_tabSeperationActivation_gvCurrentSeperationListForActivation th {
            padding-left:3px;
        }
         #ContentPlaceHolder1_MainContent_tabSeperationPanel_tabSeperationActivation_gvCurrentSeperationListForActivation th:nth-child(8),th:nth-child(10)
          {
            text-align:center;
          }
  
         .ajax__calendar table tr {
    height:auto !important;
    font-size:10px !important;
    width:auto !important;
}
         .ajax__calendar_days table tr td{
             padding:0;
             font-size:11px;
         }
         .ajax__calendar_days table tr td:first-child{
             padding:0!important;
             font-size:11px;
         }
        .wd {
            max-width:171px;
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
                    <li><a href="#" class="ds_negevation_inactive">Employees Seperation</a></li>
                </ul>
            </div>

        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hfSaveStatus" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfSeparationId" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="upSave" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upupdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="updelete" runat="server" ClientIDMode="Static" />

<%--    <div class="punishment_main_box_2">
        <div class="punishment_box_header">--%>
     <div class="row Ptrow">
   
            <div class="employee_box_header PtBoxheader" style="border-bottom: 1px solid white">
            <h2>Separation Entry Panel</h2>
        </div>
       
         <asp:TabContainer ID="tabSeperationPanel" runat="server"  CssClass="fancy fancy-green">
             <asp:TabPanel ID="tabPanel1" runat="server">
                 <HeaderTemplate>
                     Seperation Entry & Pending List
                 </HeaderTemplate>
                 <ContentTemplate >
                     <div class="employee_box_body">
            <div class="employee_box_content">
                <div class="punishment_against_Separation" style="width:45%">
                    <asp:UpdatePanel ID="up1" runat="server">
                        <Triggers>
                           
                            <asp:AsyncPostBackTrigger ControlID="gvSeparationList" />
                        </Triggers>
                        <ContentTemplate>
                            <table class="employee_table">
                                <tr id="trCompany" runat="server">
                                    <td>Company<span class="requerd1">*</span>
                                    </td>
                                    <td>:
                                    </td>
                                    <td id="td1" runat="server" clientidmode="Static" style="font-size: 16px">
                                        <asp:DropDownList ID="ddlCompany" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True">
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                                
                                <tr>

                                    <td> Card No <span class="requerd1">*</span>
                                    </td>
                                    <td>:
                                    </td>
                                    <td id="tdCardNo" runat="server" clientidmode="Static" style="font-size: 16px">                                    
                                       <asp:DropDownList ID="ddlEmpCardNo" runat="server" ClientIDMode="Static" CssClass="form-control select_width fontSize" onChange="getCardNo()"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Effective Date <span class="requerd1">*</span>
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEffectiveDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>

                                        <asp:CalendarExtender ID="txtEffectiveDate_CalendarExtender" Format="dd-MM-yyyy" runat="server" TargetControlID="txtEffectiveDate">
                                        </asp:CalendarExtender>

                                    </td>
                                </tr>
                                <tr>
                                    <td>Separation Type <span class="requerd1">*</span>
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSeparationType" runat="server" ClientIDMode="Static" CssClass="form-control select_width">
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <td>Remarks
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemarks" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Height="49px" TextMode="MultiLine"  ></asp:TextBox>

                                    </td>
                                </tr>
                            </table>
                            <div class="punishment_button_area">
                                <table class="emp_button_table">
                                    <tbody>
                                        <tr>
                                            <th>
                                                <asp:Button ID="btnSave" ClientIDMode="Static" CssClass="css_btn Ptbut" runat="server" Text="Save" OnClientClick="return InputValidation();" OnClick="btnSave_Click" /></th>
                                            <th>
                                                <asp:Button ID="btnClear" CssClass="css_btn Ptbut" runat="server" Text="Clear" OnClientClick="ClearInputBox()" OnClick="btnClear_Click" /></th>
                                            <th>
                                                <asp:Button ID="btnClose" CssClass="css_btn Ptbut" runat="server" PostBackUrl="~/default.aspx" Text="Close" /></th>
                                            </th>
                                            <th>
                                                <asp:Button ID="btnDelete" CssClass="css_btn Ptbut" runat="server" Text="Delete" OnClick="btnDelete_Click" /></th>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
               <div class="punishment_against_3" style="width:54% ;float:right;">
                    <asp:UpdatePanel ID="up2" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnClear" />
                            <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                            <asp:AsyncPostBackTrigger ControlID="ddlSearchCompany" />
                        </Triggers>
                        <ContentTemplate>
                            
                                <table style="width:100%">
                                    <tr>
                                         <td visible="false" id="tdCompany" runat="server">Company
                                    </td>
                                    <td>:
                                    </td>
                                    <td id="tddlcompany" runat="server" clientidmode="Static" style="font-size: 16px">
                                        <asp:DropDownList ID="ddlSearchCompany" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True">
                                        </asp:DropDownList>

                                    </td>
                                        <td>
                                            <asp:TextBox ClientIDMode="Static" ID="txtSearchEmp" autocomplete='off'  CssClass="form-control text_box_width"  Width="95%" runat="server" Font-Bold="True" MaxLength="16"></asp:TextBox>
                                            
                                        </td>
                                        <td>
                                            <asp:Button ID="btnFind" runat="server" Text="Find"  CssClass="css_btn Ptbut"  OnClick="btnFind_Click" />
                                            </td>
                                        <td>
                                            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="css_btn Ptbut"   OnClick="btnRefresh_Click" />

                                        </td>
                                    </tr>
                                </table>


                                <%--<div id="divSeparationList" runat="server" style="width: 508px; height: 599px;"></div>--%>
                                <asp:GridView ID="gvSeparationList" Width="100%" CssClass="display" HeaderStyle-BackColor="#750000" HeaderStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="11px" runat="server" AutoGenerateColumns="false" HeaderStyle-Height="28px"  DataKeyNames="EmpSeparationId,EmpTypeId,Remarks,EmpId" AllowPaging="true" PageSize="20" OnRowCommand="gvSeparationList_RowCommand" OnPageIndexChanging="gvSeparationList_PageIndexChanging" OnRowDataBound="gvSeparationList_RowDataBound">
                                    <PagerStyle CssClass="gridview" />                                
                                    <Columns>
                                        <asp:BoundField DataField="EmpSeparationId" HeaderText="EmpSeparationId" Visible="false" />
                                        <asp:BoundField DataField="EmpTypeId" HeaderText="EmpTypeId" Visible="false" />

                                        <asp:BoundField DataField="EmpCardNo" HeaderText="Code" Visible="true" />
                                        <asp:BoundField DataField="EmpName" HeaderText="Name" Visible="true" ItemStyle-Width="90%" />
                                        <asp:BoundField DataField="EmpType" HeaderText="Type" Visible="true" ItemStyle-Width="20%" />

                                        <asp:BoundField DataField="EffectiveDate" HeaderText="Effective Date" Visible="true" />
                                        <asp:BoundField DataField="EmpStatusName" HeaderText="Sep.Type" Visible="true" />
                                        <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" Visible="true" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" Visible="false" />
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:Button ID="btnAlter" runat="server" CommandName="Alter" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Edit" Font-Bold="true" ForeColor="Green" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:Button ID="btnRemove" runat="server" CommandName="Remove" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" OnClientClick="return confirm('Do you want to delete this record ?')" Text="Delete" Font-Bold="true" ForeColor="Red" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--<asp:ButtonField ButtonType="Button" CommandName="Alter" Text="Edit" HeaderText="Edit" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="Green" />--%>        

                                    </Columns>
                                </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
                 </ContentTemplate>             

              </asp:TabPanel>
             <asp:TabPanel ID="tabSeperationList" runat="server" TabIndex="1" >
                 <HeaderTemplate>Current Seperation List</HeaderTemplate>
                 <ContentTemplate>
                     <asp:UpdatePanel runat="server">
                          <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="ddlCompanyCurrentList" />
                        </Triggers>
                         <ContentTemplate>  
                                         <table class="employee_table" style="width:50%">
                                <tr id="tr2" runat="server">
                                    <td>Company
                                    </td>
                                    <td>:
                                    </td>
                                    <td  style="font-size: 16px">
                                        <asp:DropDownList ID="ddlCompanyCurrentList" ClientIDMode="Static" CssClass="form-control" runat="server" AutoPostBack="True"  OnSelectedIndexChanged="ddlCompanyCurrentList_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>                                 
                                </tr>
                               </table>
                                 <br />                          
                             <div style="width:100%">
                                   <asp:GridView ID="gvCurrentSeperationList" Width="100%" CssClass="display" HeaderStyle-BackColor="#750000" HeaderStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="11px" runat="server" AutoGenerateColumns="false" HeaderStyle-Height="28px"  AllowPaging="true" PageSize="20" OnPageIndexChanging="gvCurrentSeperationList_PageIndexChanging" OnRowCommand="gvSeparationList_RowCommand" OnRowDataBound="gvCurrentSeperationList_RowDataBound" >
                                    <PagerStyle CssClass="gridview" />                                
                                    <Columns>
                                        <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" Visible="true" />
                                         <asp:BoundField DataField="EmpName" HeaderText="Name" Visible="true" />
                                        <asp:BoundField DataField="EmpType" HeaderText="Emp Type" Visible="true" />
                                        <asp:BoundField DataField="EffectiveDate" HeaderText="Effected" Visible="true" />
                                        <asp:BoundField DataField="EmpStatusName" HeaderText="Seperation Type" Visible="true" />
                                        <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" Visible="true" />
                                        <asp:BoundField DataField="UserName" HeaderText="User" Visible="true" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" Visible="false" />
                                       </Columns>
                                </asp:GridView>
                             </div>
                         </ContentTemplate>
                     </asp:UpdatePanel>
                 </ContentTemplate>
             </asp:TabPanel>
                     <asp:TabPanel ID="tabSeperationActivation" runat="server" TabIndex="2" >
                 <HeaderTemplate>Activation Panel</HeaderTemplate>
                 <ContentTemplate>
                     <asp:UpdatePanel runat="server">
                          <Triggers>
                              <asp:AsyncPostBackTrigger ControlID="ddlCompanyListActive" />
                              <asp:AsyncPostBackTrigger ControlID="btnSearch" /> 
                        </Triggers>
                         <ContentTemplate>                            
                             <div style="width:100%">
                                  <table class="employee_table" style="width:60%">
                                <tr id="tr1" runat="server">
                                    <td>Company
                                    </td>
                                    <td>:
                                    </td>
                                    <td  style="font-size: 16px">
                                        <asp:DropDownList ID="ddlCompanyListActive" ClientIDMode="Static" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyListActive_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td> Card No
                                    </td>
                                    <td>:
                                    </td>
                                    <td>                                    
                                      <asp:TextBox runat="server" ID="txtEmpCardNo" CssClass="form-control">
                                      </asp:TextBox>
                                    </td>
                                    <td>
                                         <asp:Button runat="server" ID="btnSearch" ClientIDMode="Static" Text="Search" CssClass="css_btn Ptbut" OnClick="btnSearch_Click" />
                                    </td>
                                     <td>
                                         <asp:Button runat="server" ID="btnCleartxt" ClientIDMode="Static" Text="Clear" CssClass="css_btn Ptbut" OnClientClick="return Clear()" />
                                    </td>
                                </tr>
                               </table>
                                 <br />
                                   <asp:GridView ID="gvCurrentSeperationListForActivation" Width="100%" DataKeyNames="EmpSeparationId,EmpId" CssClass="display" HeaderStyle-BackColor="#750000" HeaderStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="11px" runat="server" AutoGenerateColumns="false" HeaderStyle-Height="28px"  AllowPaging="true" PageSize="20"  OnPageIndexChanging="gvCurrentSeperationListForActivation_PageIndexChanging" OnRowCommand="gvCurrentSeperationListForActivation_RowCommand" OnRowDataBound="gvCurrentSeperationListForActivation_RowDataBound">
                                    <PagerStyle CssClass="gridview" />                                
                                    <Columns>
                                        <asp:BoundField DataField="EmpCardNo" HeaderText="Card No"  />
                                       <asp:BoundField DataField="EmpName" HeaderText="Name"  />
                                       <asp:BoundField DataField="EmpType" HeaderText="Emp Type" />     
                                          <asp:BoundField DataField="DptName" HeaderText="Department"  />
                                        <asp:BoundField DataField="DsgName" HeaderText="Designation"  />
                                         <asp:BoundField DataField="EmpStatusName" HeaderText="Seperation Type"  />  
                                        <asp:BoundField DataField="EffectiveDate" HeaderText="Seperation Date" />                              
                                            <asp:TemplateField  HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                Active Date
                                            </HeaderTemplate>
                                            <ItemTemplate  >
                                   <asp:TextBox  ID="txtActiveDate" runat="server" Enabled="true" Style="text-align:center"  CssClass="form-control wd" Text='<%#Bind("CurrentDate") %>'>'></asp:TextBox> 
                                   <asp:CalendarExtender ID="txtActiveDate_CalendarExtender" runat="server" Format="dd-MM-yyyy"  TargetControlID="txtActiveDate">
                                   </asp:CalendarExtender>
                                  </ItemTemplate>
                                        </asp:TemplateField>   
                                            <asp:TemplateField>
                                            <HeaderTemplate>
                                               Remarks
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRemarks" runat="server" ClientIDMode="Static" CssClass="form-control" Text=''></asp:TextBox>                                             
                                            </ItemTemplate>
                                        </asp:TemplateField>                  
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center"  ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                Active
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btnActive" CommandName="Active" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Font-Bold="true" OnClientClick="return confirm('Do you want to active this employee ?')"  BackColor="Green" ClientIDMode="Static" Text="Active" CssClass="btn btn-primary" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       </Columns>
                                </asp:GridView>
                             </div>
                         </ContentTemplate>
                     </asp:UpdatePanel>
                 </ContentTemplate>
             </asp:TabPanel>
              <asp:TabPanel ID="TabPanel2" runat="server" TabIndex="3" >
                 <HeaderTemplate>Activation Log</HeaderTemplate>
                 <ContentTemplate>
                     <asp:UpdatePanel runat="server">
                          <Triggers>
                              <asp:AsyncPostBackTrigger ControlID="ddlCompanyListActiveLog" />
                              <asp:AsyncPostBackTrigger ControlID="btnSearchLog" /> 
                        </Triggers>
                         <ContentTemplate>                            
                             <div style="width:100%">
                                  <table class="employee_table" style="width:60%">
                                <tr id="tr3" runat="server">
                                    <td>Company
                                    </td>
                                    <td>:
                                    </td>
                                    <td  style="font-size: 16px">
                                        <asp:DropDownList ID="ddlCompanyListActiveLog" ClientIDMode="Static" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyListActiveLog_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td> Card No
                                    </td>
                                    <td>:
                                    </td>
                                    <td>                                    
                                      <asp:TextBox runat="server" ID="txtCardnoActive" CssClass="form-control">
                                      </asp:TextBox>
                                    </td>
                                    <td>
                                         <asp:Button runat="server" ID="btnSearchLog" ClientIDMode="Static" Text="Search" CssClass="css_btn Ptbut" OnClick="btnSearchLog_Click" />
                                    </td>
                                  
                                </tr>
                               </table>
                                 <br />
                                   <asp:GridView ID="gvSeparationActivitionLog" Width="100%"  CssClass="display" HeaderStyle-BackColor="#750000" HeaderStyle-HorizontalAlign="Center" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="11px" runat="server" AutoGenerateColumns="false" HeaderStyle-Height="28px"  AllowPaging="true" PageSize="20"  OnPageIndexChanging="gvSeparationActivitionLog_PageIndexChanging"  >
                                    <PagerStyle CssClass="gridview" />                                
                                    <Columns>
                                        <asp:BoundField DataField="EmpCardNo" HeaderText="Card No"  />
                                       <asp:BoundField DataField="EmpName" HeaderText="Name"  />
                                       <asp:BoundField DataField="EmpType" HeaderText="Emp Type" />     
                                          <asp:BoundField DataField="DptName" HeaderText="Department"  />
                                        <asp:BoundField DataField="DsgName" HeaderText="Designation"  />
                                        <asp:BoundField DataField="ActiveDate" HeaderText="Actived Date"  />
                                        <asp:BoundField DataField="Remark" HeaderText="Remark"  />
                                        <asp:BoundField DataField="UName" HeaderText="Actived By"  />                         
                                       </Columns>
                                </asp:GridView>
                             </div>
                         </ContentTemplate>
                     </asp:UpdatePanel>
                 </ContentTemplate>
             </asp:TabPanel>
         </asp:TabContainer>
    </div>   
    <script type="text/javascript">
        $(document).ready(function () {
            load();

        });
        function load() {
            $("#ddlEmpCardNo").select2();
           
        }
        function getCardNo() {

            var getId = document.getElementById('ddlEmpCardNo');
            var val = getId.options[getId.selectedIndex].text;
            //  alert(val);
            var getCardNo = val.split(' ');
            //document.getElementById('txtEmpCardNo').value = getCardNo[0];

        }
        function Clear()
        {
            $('#txtEmpCardNo').val("");
            
        }

        function editSeparationType(getId) {
            try {
                $('#hfSaveStatus').val("Update");
                $('#hfSeparationId').val(getId);
                $('#btnSave').val("Update");

                $('#ddlEmpCardNo').hide();
                $('#txtEmpCardNo').hide();
                $('#tdCardNo').text($('#r_' + getId + ' td:first-child').html());
                $('#txtEffectiveDate').val($('#r_' + getId + ' td:nth-child(2)').html());
                $('#ddlSeparationType').val($('#r_' + getId + ' td:nth-child(3)').html());
                $('#btnDelete').show();
                jx.load('/ajax.aspx?id=' + getId + '&todo=getSeparationType' + '&amount= ' + 0 + '&status=' + status + ' ', function (data) {
                    $('#txtRemarks').html(data);
                });
            }
            catch (exception) {
                lblMessage.innerText = exception;
            }
        }

        function ClearInputBox() {            
            $('#txtEffectiveDate').val(' ');
            $('#txtSearchEmp').val(' ');
            $('#txtRemarks').val(' ');
            $('#btnSave').val("Save");
            $('#hfSaveStatus').val("Save");
            $('#btnSave').val("Save");
            load();
        }

        function InputValidation() {
            try {
                if ($('#txtEmpCardNo').val().length < 4) {
                    lblMessage.innerText = "Please Type a Card Number or Select a Card Number";
                    $('#txtEmpCardNo').focus();
                    return false;

                }
                else if ($('#txtEffectiveDate').val().length < 8) {
                    lblMessage.innerText = "Please Type a Card Number or Select a Card Number";
                    $('#txtEffectiveDate').focus();
                    return false;

                }
                else if ($('#ddlSeparationType').val() == "-select-") {
                    lblMessage.innerText = "Please Type a Card Number or Select a Card Number";
                    $('#ddlSeparationType').focus();
                    return false;
                    Boolean
                }
                return true;
            }
            catch (exception) {
                return true;
            }
        }
    </script>
 
  
</asp:Content>
