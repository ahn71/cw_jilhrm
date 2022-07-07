<%@ Page Title="Roster Create Panel" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="ShiftManagement.aspx.cs" Inherits="SigmaERP.personnel.ShiftManagement" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register Assembly="ComplexScriptingWebControl" Namespace="ComplexScriptingWebControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="../style/list_checkbox_style.css" rel="stylesheet" />
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
               $(document).ready(function () {

                  // jQuery('#ddlNewShift').select2();
                   var s = $("#ContentPlaceHolder1_MainContent_divElementContainer");
                   var pos = s.position();
                   $(window).scroll(function () {
                       var windowpos = $(window).scrollTop();
                       if (windowpos >= pos.top) {
                           s.addClass("stick");
                       } else {
                           s.removeClass("stick");
                       }
                   });
               });

               //function load() {
               //    jQuery('#ddlNewShift').select2();
               //}
</script>
<style>
.stick{
    position:fixed;top:0px;left:126px;
}
    
    #ContentPlaceHolder1_MainContent_gvEmpList th,td {
        text-align:center;
        font-size:13px;
    }
    .employee_table tr td{
        font-size:14px;
        color:#fff;
    }
    .ajax__calendar_container TD {
      color: #222 !important;
      font-size: 11px !important;
    }
        #ContentPlaceHolder1_MainContent_gvEmpList th:nth-child(2),td:nth-child(2) {
        text-align:left;
        padding-left:3px;
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
                       <li><a href="#" class="ds_negevation_inactive Ptactive">Roster Create Panel</a></li>
                   </ul>
               </div>
          
             </div>
       </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <script src="../scripts/jquery-1.8.2.js"></script>
<asp:UpdatePanel ID="uplMessage" runat="server" >
   
    <ContentTemplate>
        <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
    </ContentTemplate>
</asp:UpdatePanel>    
  <div class="row Ptrow">
        <div class="employee_box_header PtBoxheader">
            <div class="employee_box_header PtBoxheader" style="border-bottom: 1px solid white">
                <h2>Roster Create Panel</h2>

            </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>                     
                            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                            <asp:AsyncPostBackTrigger ControlID="ddlCurrentShift" />
                            <asp:AsyncPostBackTrigger ControlID="ddlNewShift" />                                              
                            <asp:PostBackTrigger ControlID="btnSubmit" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDepartmentList" />   
                            <asp:AsyncPostBackTrigger ControlID="ddlGroupList" />                      
                        </Triggers>
                        <ContentTemplate>    
                               <div style="float: right; margin-top: -32px;">
                       <asp:Label ID="lblTotalRow" runat="server" Text="" Font-Bold="true" ForeColor="Yellow"></asp:Label>
                       <asp:Label ID="lblSelectedRow" runat="server" Text="" Font-Bold="true" ForeColor="#74BF43"></asp:Label>
                   </div>                 
                            <%--<h2 style="float: right; margin-top: -42px;">
                          <asp:LinkButton runat="server" ID="lnkRefresh" Text="Refresh" ForeColor="gray" OnClick="lnkRefresh_Click"></asp:LinkButton> |
                          <a style="color:gray" href="../leave_default.aspx">Close</a></h2>--%>                   
                            <%--<table style="width: 100%;">
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>--%>
                      
               <div>
                 
            <%--       <div class="row">
                       <div class="col-md-1">                      
                           Company
                       </div>
                       <div class="col-md-2">
                           <asp:DropDownList runat="server" ID="DropDownList1" CssClass="form-control text_box_width style" Width="98%" Height="30px" AutoPostBack="True" ></asp:DropDownList>
                       </div>
                       <div class="col-md-1">                      
                           Company
                       </div>
                       <div class="col-md-2">
                           <asp:DropDownList runat="server" ID="DropDownList2" CssClass="form-control text_box_width style" Width="98%" Height="30px" AutoPostBack="True" ></asp:DropDownList>
                       </div>
                       <div class="col-md-1">                      
                           Company
                       </div>
                       <div class="col-md-2">
                           <asp:DropDownList runat="server" ID="DropDownList3" CssClass="form-control text_box_width style" Width="98%" Height="30px" AutoPostBack="True" ></asp:DropDownList>
                       </div>
                       <div class="col-md-1">                      
                           Company
                       </div>
                       <div class="col-md-2">
                           <asp:DropDownList runat="server" ID="DropDownList4" CssClass="form-control text_box_width style" Width="98%" Height="30px" AutoPostBack="True" ></asp:DropDownList>
                       </div>
                   </div>--%>
                   
                      <table class="employee_table">               
                          <tr>
                              <td class="shift_manage_headar_label_color">
                                       Company
                                      </td>
                                       <td>
                                           <asp:DropDownList runat="server" ID="ddlCompanyList" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ></asp:DropDownList>
                                     
                                 </td>
                              <td class="shift_manage_headar_label_color">Group
                              </td>
                              <td>
                                  <asp:DropDownList runat="server" ID="ddlGroupList" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlGroupList_SelectedIndexChanged"></asp:DropDownList>
                              </td>

                              <td class="shift_manage_headar_label_color">Deparment
                              </td>
                              <td>
                                  <asp:DropDownList runat="server" ID="ddlDepartmentList" CssClass="form-control text_box_width style" Width="98%" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartmentList_SelectedIndexChanged"></asp:DropDownList>
                              </td>

                              <td class="shift_manage_headar_label_color" style="display:none">Current Shift
                              </td>

                              <td style="display:none">
                                  <asp:DropDownList runat="server" ID="ddlCurrentShift" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlCurrentShift_SelectedIndexChanged"></asp:DropDownList>
                              </td>                  

                          </tr>
                            <tr>
                                 <td class="shift_manage_headar_label_color">From Date</td>
                                <td>
                                     <asp:TextBox ID="txtFromDate" Width="96%" ClientIDMode="Static" AutoComplete="off"  runat="server" CssClass="form-control text_box_width" MaxLength="10" style="text-align:center" ></asp:TextBox>
                                     <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtFromDate" Format="dd-MM-yyyy">
                                     </asp:CalendarExtender>
                                </td>

                                <td class="shift_manage_headar_label_color">To Date</td>
                                <td>
                                     <asp:TextBox ID="txtToDate" Width="96%" ClientIDMode="Static" AutoComplete="off"  runat="server" CssClass="form-control text_box_width" MaxLength="10" style="text-align:center" ></asp:TextBox>
                                     <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" TargetControlID="txtToDate" Format="dd-MM-yyyy">
                                     </asp:CalendarExtender>
                                </td>
                                <td class="shift_manage_headar_label_color">New Shift
                              </td>

                              <td>
                                  <asp:DropDownList ID="ddlNewShift" CssClass="form-control text_box_width style" runat="server" Width="96%" Height="30px" AutoPostBack="True" ClientIDMode="Static" OnSelectedIndexChanged="ddlNewShift_SelectedIndexChanged"></asp:DropDownList>
                              </td>
                               
                                <td>  </td>
                                <td class="shift_manage_headar_label_color"> 
                                    <asp:Button runat="server" ID="btnSubmit" CssClass="css_btn Ptbut" Text="Submit"  Width="65px" Height="34px" OnClick="btnSubmit_Click" />                                   
                                     <asp:Button runat="server" ID="btnClose" CssClass="css_btn Ptbut" Text="Close" Width="65px" Height="34px"  />                                      
                                    <asp:CheckBox runat="server" ID="chkUpdate" AutoPostBack="true" OnCheckedChanged="chkUpdate_CheckedChanged" />
                                    <asp:Label ID="lblCheck" runat="server" for="chkUpdate" Text="Shift Modify"></asp:Label>
                                </td>
                                <td></td>                      
                              </tr>                          
                         </table>                 
                  
                     </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div >
                   <div style="position: fixed;text-align: center;top: 50%;width: 100%;">
                       <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold;float:left"><p>&nbsp;</p> </span> <br />
                                        <img cursor:pointer; float:left" src="/images/loader-2.gif"/>  
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                       <div style="color: red; font-size: 30px; font-family: 'Segoe UI'; font-weight: bold;">
                           <asp:Label ID="lblErrorMessage" runat="server" Text="Message" ClientIDMode="Static"></asp:Label>
                       </div>
                  </div>
                <asp:UpdatePanel runat="server" ID="up2">
                    <Triggers>
                        
                        
                    </Triggers>
                    <ContentTemplate>
                     <div  style="width: 100%; background-color:#fff;margin: auto;">
                         <div id="divRecordMessage" runat="server" visible="false" style="color: red; font-weight: bold; text-align: center; padding-top: 75px; font-size: 32px; height: 118px">
                           
                         </div>
                     <asp:GridView HeaderStyle-BackColor="#750000" ID="gvEmpList"  runat="server" AutoGenerateColumns="false"  HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true"  HeaderStyle-Height="25px" HeaderStyle-Font-Size="14px" PageSize="25" Width="100%" DataKeyNames="EmpId,SftId,Assigned" OnRowDataBound="gvEmpList_RowDataBound">
                         <PagerStyle CssClass="gridview" />
                          <Columns>   
                              <asp:TemplateField>
                                  <HeaderTemplate>SL</HeaderTemplate>
                                  <ItemTemplate>
                                      <%# Container.DataItemIndex+1 %>
                                  </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Center" ForeColor="Red" Font-Bold="true" />
                              </asp:TemplateField>                        
                           <asp:BoundField DataField="EmpCardNo" HeaderText="CardNo" ItemStyle-HorizontalAlign="Center" ItemStyle-Height="21px" />
                                 <asp:BoundField DataField="EmpName" HeaderText="Name" />   
                               <asp:TemplateField HeaderText="Chosen" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chkHeaderChosen" Checked="false" AutoPostBack="true" Text="Chosen" OnCheckedChanged="chkHeaderChosen_CheckedChanged" />
                                    </HeaderTemplate>
                                  <ItemTemplate >                
                                      <div class="SingleCheckbox">  
                                      <asp:CheckBox runat="server" Text="&nbsp;" ID="chkChosen" Checked='<%#bool.Parse(Eval("Status").ToString()) %>' AutoPostBack="true" OnCheckedChanged="chkChosen_CheckedChanged" />
                                      </div>
                                     <%-- <asp:Button ID="btnEdit" runat="server" CommandName="Edit" Width="55px" Height="30px" Font-Bold="true" ForeColor="green" Text="Edit" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />--%>
                                  </ItemTemplate>
                              </asp:TemplateField> 
                               <asp:TemplateField HeaderText="Duty Place"  ItemStyle-Width="100px">
                                  <ItemTemplate >
                                   <asp:DropDownList ID="ddlFloorList" runat="server"  AppendDataBoundItems="true" CssClass="form-control text_box_width style"  ForeColor="Red" ></asp:DropDownList> 
                                  </ItemTemplate>
                              </asp:TemplateField>     
                               <asp:TemplateField HeaderText="Comments"  ItemStyle-Width="70px">
                                  <ItemTemplate >
                                   <asp:TextBox ID="txtRemaks" runat="server"   Text='<%#Eval("Notes") %>'   AppendDataBoundItems="true" CssClass="form-control text_box_width style" AutoPostBack="true" Width="140px" ></asp:TextBox> 
                                  </ItemTemplate>
                              </asp:TemplateField>     
                                                           
                                 <asp:BoundField DataField="DsgName" HeaderText="Designation"  /> 
                                <asp:BoundField DataField="SftName" HeaderText="Roster"  />                                       
                                                                                            
                               
                             <%--  <asp:TemplateField HeaderText="Delete" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                  <ItemTemplate>
                                      <asp:Button ID="btnView" runat="server" CommandName="Remove" Font-Bold="true" ForeColor="red" Text="Delete" Width="55px" Height="30px" OnClientClick="return confirm('Do you want to delete this record ?')" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                  </ItemTemplate>
                              </asp:TemplateField>  --%>                                                  
                          </Columns>
                     </asp:GridView>
                         
                </div>
                   </ContentTemplate>
                </asp:UpdatePanel>
          <div id="divProgressPanel" runat="server">
          <cc1:ProgressBar ID="ProgressBar1" runat="server" />
           </div>
        </div>
    <style type=”text/css”>
    .hand { cursor: pointer; cursor: hand; } /* cross browser hand */
</style>
       <script type="text/javascript">
           function deleteRow(id) {
               var answer = confirm("Are you sure you want to Delete");
               if (answer == true) {
                   jx.load('/ajax.aspx?id=' + id + '&todo=DeleteEmployee', function (data) {
                       document.location = '/personnel/employee_list.aspx';
                       // $('#divEmployeeList').html(data);
                   });
                   return true;
               }
               else {
                   return false;
               }


           }
           function editEmployee(id) {
               goURL('/personnel/employee.aspx?EmpId=' + id + "&Edit=True");
           }
           function confirmDelete() {
               alert("confirmComplete");
               var answer = confirm("Are you sure you want to Delete");
               if (answer == true) {
                   $('#hdfdeleteconfirm').val('true');
                   //return true;
               }
               else {
                   $('#hdfdeleteconfirm').val('false');
                   // return false;
               }
           }

           function alertMessage()
           {
              
               setTimeout(function () {
                   $('#lblErrorMessage').fadeOut("slow", function () {
                       $('#lblErrorMessage').remove();
                       $('#lblErrorMessage').val('');
                   });

               }, 3000);
           }
    </script>
</asp:Content>
