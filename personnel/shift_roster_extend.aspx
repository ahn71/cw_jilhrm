<%@ Page Title="Roster Extend Panel" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="shift_roster_extend.aspx.cs" Inherits="SigmaERP.personnel.shift_roster_extend" %>
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
              //jQuery('#ddlDepartmentList').select2();
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
          //    jQuery('#ddlDepartmentList').select2();
          //}
</script>
<style>
.stick{
    position:fixed;top:0px;left:126px;
}
.tblstyle{

}
#ContentPlaceHolder1_MainContent_gvEmpList tbody tr td{
            padding-left:5px; 
            font-size:13px;       
        }
    .auto-style1 {
        width: 328px;
    }
    .tdWidth
    {
     width:180px;
    }
    #ContentPlaceHolder1_MainContent_gvEmpList th, td {
        text-align:center;
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
                       <li><a href="#" class="ds_negevation_inactive Ptactive">Roster Extend Panel</a></li>
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>                     
                            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />                                                                                              
                            <asp:PostBackTrigger ControlID="btnExtend" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDepartmentList" />      
                            <asp:AsyncPostBackTrigger ControlID="chkLoadAllShiftList" /> 
                            <asp:AsyncPostBackTrigger ControlID="ddlAssignShift" /> 
                            <asp:AsyncPostBackTrigger ControlID="ddlGrouopList" />                 
                        </Triggers>
                        <ContentTemplate>                      
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
                             <div class="employee_box_header PtBoxheader" style="border-bottom:1px solid white;position:relative">           
                                  <h2>Roster Extend Panel</h2>
                                 <asp:Label ID="lblErrorMessage" runat="server" Text=""  style="position: absolute; top: 0px; padding: 13px;" ClientIDMode="Static"></asp:Label>
                   <div style="overflow:hidden;margin-top:-32px; float:right;">
                                               
                       <asp:RadioButtonList style="color:#fff;font-size:13px;font-weight:bold;position:relative;float:right;margin-right:5%;"  ID="rbRosterType" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal" Font-Bold="True">
                           <asp:ListItem Value="Create" Text="Create Roster"></asp:ListItem>
                           <asp:ListItem Value="Extend" Text="Extend Roster"></asp:ListItem>
                       </asp:RadioButtonList>               
                   </div>
                                 </div>
               <div style="border:1px solid white">                  
                        <table style="width:100%" >
                              <tr>
                                    <td style="color:#fff;font-size:13px;">
                                       Company
                                      </td>
                                       <td class="tdWidth">
                                     <asp:DropDownList runat="server" ID="ddlCompanyList" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged"  ></asp:DropDownList>
                                 </td>

                                  <td style="color:#fff;font-size:13px;">
                                      Deparment 
                                      </td>
                                       <td class="tdWidth">
                                     <asp:DropDownList runat="server" ID="ddlDepartmentList" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True" ClientIDMode="Static" OnSelectedIndexChanged="ddlDepartmentList_SelectedIndexChanged" ></asp:DropDownList>
                                 </td>   
                                 <td style="color:#fff;font-size:13px;">
                                      Group 
                                      </td>
                                       <td class="tdWidth">
                                     <asp:DropDownList runat="server" ID="ddlGrouopList" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True" ClientIDMode="Static" OnSelectedIndexChanged="ddlGrouopList_SelectedIndexChanged"  ></asp:DropDownList>
                                 </td>   
                                  <td style="color:#fff;font-size:13px;">
                                      Shift 
                                      </td>
                                       <td class="tdWidth">
                                     <asp:DropDownList runat="server" ID="ddlAssignShift" CssClass="form-control text_box_width style" style="min-width:96%" Height="30px" AutoPostBack="True" ClientIDMode="Static" OnSelectedIndexChanged="ddlAssignShift_SelectedIndexChanged" ></asp:DropDownList>
                                 </td>     
                                    
                                  <td style="color:#fff;font-size:13px;">
                                      <asp:CheckBox ID="chkLoadAllShiftList" runat="server" AutoPostBack="true" Text="All A.Sh." OnCheckedChanged="chkLoadAllShiftList_CheckedChanged"  />
                                  </td> 
                                  <td >
                                       <asp:TextBox ID="txtDate" runat="server" Width="100px" placeHolder="Extend Date" Style=" text-align:center "  ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                      <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                PopupButtonID="imgEffectDateFrom" Enabled="True"
                                                TargetControlID="txtDate" ID="CExtApplicationDate">
                                            </asp:CalendarExtender>
                                  </td>                                                                                   
                                  <td>  <asp:Button runat="server" ID="btnExtend" CssClass="css_btn Ptbut" Text="Submit" OnClientClick="return confirm('Do you want to complete the action ?')" Width="70px" Height="34px" OnClick="btnExtend_Click"   /></td>
                                
                                     <td>
                                         <asp:Label runat="server" ID="lblTotal" Text="Total" Font-Bold="true" ForeColor="Red" style="text-align:right; float:right;width:40px; margin-right:5px"  ></asp:Label>
                                     </td>                 
                             </tr>
                           
                         </table>                 
                  
                     </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div >
                   <div class="loding_img">
                       <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold;float:left"><p>&nbsp;</p> </span> <br />
                                        <img cursor:pointer; float:left" src="/images/loader-2.gif"/>  
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                  </div>
                <asp:UpdatePanel runat="server" ID="up2">
                    <Triggers>
                        

                    </Triggers>
                    <ContentTemplate>
                     <div  style="width: 100%; background-color:#fff;margin: auto;">
                         <div id="divRecordMessage" runat="server" visible="false" style="color: red; font-weight: bold; text-align: center; padding-top: 75px; font-size: 32px; height: 118px">
                           
                         </div>
                     <asp:GridView HeaderStyle-BackColor="#750000" ID="gvEmpList"  runat="server" AutoGenerateColumns="false"  HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true"  HeaderStyle-Height="25px" HeaderStyle-Font-Size="14px" PageSize="25" Width="100%" DataKeyNames="STId,EmpId,DsgId,EmpTypeId,GId,FId" OnRowDataBound="gvEmpList_RowDataBound"  >
                         <PagerStyle CssClass="gridview" />
                          <Columns>                           
                           <asp:BoundField DataField="EmpCardNo" HeaderText="CardNo" ItemStyle-Height="21px" />
                                 <asp:BoundField DataField="EmpName" HeaderText="Name" />                                 
                                 <asp:BoundField DataField="DsgName" HeaderText="Designation"  />                                
                                 <asp:BoundField DataField="EmpType" HeaderText="Employee Type"  />  
                                 <asp:BoundField DataField="FName" HeaderText="Duty Place"  />  
                                  <asp:BoundField DataField="Notes" HeaderText="Notes"  />  
                                                                                   
                              <%--   <asp:TemplateField HeaderText="Remove" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" Visible="true">
                                 
                                  <ItemTemplate >
                                     
                                   
                                      <%--<asp:Button ID="btnRemove" runat="server" CommandName="Edit" Width="59px" Height="30px" Font-Bold="true" ForeColor="green" Text="Remove" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />--%>
                                   <%--   <asp:ImageButton ID="ibRemove" runat="server" CommandName="Remove" Width="55px" Height="28px" ImageUrl="~/images/action/Delete_Icon.png"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' OnClientClick="return confirm('Do you want to remove ?')" />
                                       </ItemTemplate>
                              </asp:TemplateField>--%>
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
        </div>
     <div id="divProgressPanel" runat="server">
          <cc1:ProgressBar ID="ProgressBar1" runat="server" />
           </div>
    <script type="text/javascript">
        function alertMessage() {

            setTimeout(function () {
                $('#lblErrorMessage').fadeOut("slow", function () {
                    $('#lblErrorMessage').remove();
                    $('#lblErrorMessage').val('&npsp;');
                });

            }, 3000);
        }
    </script>
</asp:Content>
