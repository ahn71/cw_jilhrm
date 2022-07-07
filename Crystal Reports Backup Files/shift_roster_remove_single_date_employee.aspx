<%@ Page Title="Roster Add & Remove" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="shift_roster_remove_single_date_employee.aspx.cs" Inherits="SigmaERP.personnel.shift_roster_remove_single_date_employee" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

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
        .stick {
            position: fixed;
            top: 0px;
            left: 126px;
        }

        .tblstyle {
        }

        #ContentPlaceHolder1_MainContent_gvEmpList tbody tr td {
            padding-left: 5px;
        }

        .auto-style1 {
            width: 328px;
        }

        .tdWidth {
            width: 250px;
        }

        .dis {
            display: inline;
        }
        #ContentPlaceHolder1_MainContent_gvEmpList td,th {
            text-align:center;
        }
         #ContentPlaceHolder1_MainContent_gvEmpList td:nth-child(2),th:nth-child(2) {
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
                    <li><a href="#" class="ds_negevation_inactive Ptactive">Roster Add & Remove</a></li>
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
                <h2>Roster Add & Remove Panel</h2>

            </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>                     
                            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />                                                                                              
                            <%--<asp:AsyncPostBackTrigger ControlID="btnSave" />--%>
                            
                            <asp:AsyncPostBackTrigger ControlID="ddlDepartmentList" />      
                            <asp:AsyncPostBackTrigger ControlID="chkLoadAllShiftList" /> 
                            <asp:AsyncPostBackTrigger ControlID="ddlAssignShift" />                  
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
               <div style="margin-left:15px">
                        <table style="border-collapse: collapse;width:100%; padding: 5px 0px;">
                              <tr>
                                    <td style="color:#fff;font-size:13px;">
                                       Company
                                      </td>
                                       <td class="tdWidth">
                                     <asp:DropDownList runat="server" ID="ddlCompanyList" CssClass="form-control text_box_width style" Width="96%" AutoPostBack="True"  OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ></asp:DropDownList>
                                 </td>

                                  <td style="color:#fff;font-size:13px;">
                                      Deparment 
                                      </td>
                                       <td class="tdWidth">
                                     <asp:DropDownList runat="server" ID="ddlDepartmentList" CssClass="form-control text_box_width style" Width="96%" AutoPostBack="True" ClientIDMode="Static" OnSelectedIndexChanged="ddlDepartmentList_SelectedIndexChanged" ></asp:DropDownList>
                                 </td>   
                                  <td style="color:#fff;font-size:13px;">
                                      Shift 
                                      </td>
                                       <td class="tdWidth">
                                     <asp:DropDownList runat="server" ID="ddlAssignShift" CssClass="form-control text_box_width style" Width="96%"  AutoPostBack="True" ClientIDMode="Static" OnSelectedIndexChanged="ddlAssignShift_SelectedIndexChanged" ></asp:DropDownList>
                                 </td>     
                                    
                                  <td style="color:#fff;font-size:13px;">
                                      <asp:CheckBox ID="chkLoadAllShiftList" runat="server" AutoPostBack="true" Text="All Assigned Shift" OnCheckedChanged="chkLoadAllShiftList_CheckedChanged"   />
                                  </td> 
                              </tr>

                           <tr>
                               <td>Date</td>
                                <td>
                                       <asp:TextBox ID="txtDate" runat="server" Width="96%"  Style=" text-align:center "  ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                     
                                     <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                PopupButtonID="imgEffectDateFrom" Enabled="True"
                                                TargetControlID="txtDate" ID="CExtApplicationDate">
                                            </asp:CalendarExtender>
                                  </td> 
                                  
                                  <td>  <asp:Button runat="server" ID="btnSearch" CssClass="css_btn Ptbut" Text="Search" Width="70px" Height="34px" OnClick="btnSearch_Click" /></td>

                               <td style="border-top: 1px solid #fff; border-left: 1px solid #fff; border-bottom: 1px solid #fff">     
                                   <asp:TextBox ID="txtEmpCardNo" runat="server" Width="90px" placeHolder="Card No" Style="text-align: center" ClientIDMode="Static" AutoComplete="off" CssClass="form-control text_box_width dis" Font-Bold="True" ForeColor="#FF3300" MaxLength="6"></asp:TextBox>                              
                                    <asp:Button runat="server" ID="btnVerify" CssClass="css_btn Ptbut" Text="Verify" Width="65px" Height="34px" OnClick="btnVerify_Click" />
                                   <asp:Button runat="server" ID="btnSave" CssClass="css_btn Ptbut" Text="Add" OnClientClick="return confirm('Do you want to save ?')" Width="65px" Height="34px" OnClick="btnSave_Click" />
                                   
                                  
                               </td>
                                <td style="color:#fff;font-size:13px;border-top: 1px solid #fff; border-bottom: 1px solid #fff">
                                      Floor 
                                 </td>
                                 <td class="tdWidth" style="border-top: 1px solid #fff;border-bottom: 1px solid #fff">
                                     <asp:DropDownList runat="server" ID="ddlFloorList" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True" ClientIDMode="Static" ></asp:DropDownList>
                                 </td>   
                                     
                                 <td style="border-top: 1px solid #fff; border-right: 1px solid #fff; border-bottom: 1px solid #fff">
                                     <asp:TextBox ID="txtNotes" placeHolder="Coments" runat="server" Width="96%"  Style=" text-align:center "  ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
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
                     <asp:GridView HeaderStyle-BackColor="#750000" ID="gvEmpList"  runat="server" AutoGenerateColumns="false"  HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true"  HeaderStyle-Height="25px" HeaderStyle-Font-Size="14px" PageSize="25" Width="100%" DataKeyNames="STId,EmpId,DsgId,EmpTypeId,GId,FId" OnRowCommand="gvEmpList_RowCommand" OnRowDataBound="gvEmpList_RowDataBound"   >
                         <PagerStyle CssClass="gridview" />
                          <Columns>                           
                           <asp:BoundField DataField="EmpCardNo" HeaderText="CardNo" ItemStyle-HorizontalAlign="Center" ItemStyle-Height="21px" />
                                 <asp:BoundField DataField="EmpName" HeaderText="Name" />                                 
                                 <asp:BoundField DataField="DsgName" HeaderText="Designation" ItemStyle-HorizontalAlign="Center" />                                
                                 <asp:BoundField DataField="EmpType" HeaderText="Employee Type" ItemStyle-HorizontalAlign="Center" />  
                                 <asp:BoundField DataField="FName" HeaderText="Duty Place" ItemStyle-HorizontalAlign="Center" />  
                                  <asp:BoundField DataField="Notes" HeaderText="Notes" ItemStyle-HorizontalAlign="Center" />  
                                                                                   
                                 <asp:TemplateField HeaderText="Remove" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" Visible="true">
                                 
                                  <ItemTemplate >
                                     
                                   
                                      
                                      <asp:ImageButton ID="ibRemove" runat="server" CommandName="Remove" Width="55px" Height="28px" ImageUrl="~/images/action/Delete_Icon.png"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' OnClientClick="return confirm('Do you want to remove ?')" />
                                       </ItemTemplate>
                              </asp:TemplateField>
                                                                             
                          </Columns>
                     </asp:GridView>
                         
                </div>
                   </ContentTemplate>
                </asp:UpdatePanel>
        </div>

</asp:Content>
