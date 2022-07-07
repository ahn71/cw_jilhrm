<%@ Page Title="Roster View & Remove Panel" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="ShiftManageRemove.aspx.cs" Inherits="SigmaERP.personnel.ShiftManageRemove" %>
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

        /*#ContentPlaceHolder1_MainContent_gvEmpList tbody tr td {
            padding-left: 5px;
        }*/

        .auto-style1 {
            width: 270px;
        }

        .tdWidth {
            width: 250px;
        }

        #ContentPlaceHolder1_MainContent_gvEmpList td,th {
            text-align: center;
        }
         #ContentPlaceHolder1_MainContent_gvEmpList td:nth-child(3),th:nth-child(3) {
            text-align: left;
            padding-left:3px;
        }
        /*#ContentPlaceHolder1_MainContent_gvEmpList th:nth-child(2), th:nth-child(3), td:nth-child(2), td:nth-child(3) {
            margin-left: 3px;
            text-align: left;
        }*/
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
                    <li><a href="#" class="ds_negevation_inactive Ptactive">Roster View & Remove</a></li>
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
                <h2>Roster View & Remove Panel</h2>
            </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>                     
                            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />                                                                                              
                            <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDepartmentList" />      
                            <asp:AsyncPostBackTrigger ControlID="chkLoadAllShiftList" />                   
                        </Triggers>
                        <ContentTemplate>                      
                        
               <div>
                        <table style="width:88%;margin:0px auto">
                              <tr>
                                    <%--<td runat="server" >
                                       Company
                                      </td>--%>
                                       <td class="auto-style1" runat="server" visible="false">
                                     <asp:DropDownList runat="server" ID="ddlCompanyList" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ></asp:DropDownList>
                                 </td>

                                  <td>
                                      Deparment 
                                      </td>
                                       <td class="auto-style1">
                                     <asp:DropDownList runat="server" ID="ddlDepartmentList" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartmentList_SelectedIndexChanged" ClientIDMode="Static" ></asp:DropDownList>
                                 </td>     
                                  <td>
                                      <asp:CheckBox ID="chkLoadAllShiftList" runat="server" AutoPostBack="true" Text="All Assigned Shift" OnCheckedChanged="chkLoadAllShiftList_CheckedChanged" />
                                  </td>                                                                                    
                                       <td>  <asp:Button runat="server" ID="btnDelete" CssClass="css_btn Ptbut" Font-Bold="true" Text="Delete full roster" OnClientClick="return confirm('Do you want to delete ?')" Width="125px" Height="34px" OnClick="btnDelete_Click"  /></td>
                                  <td>
                                       <asp:TextBox Height="28px" MaxLength="10" AutoComplete="Off" PlaceHolder="Select for single date " Style=" border:1px solid; text-align:center;" ID="txtRosterDate" runat="server" Enabled="true" ForeColor="Blue"   ></asp:TextBox> 
                                   <asp:CalendarExtender ID="txtRosterDate_CalendarExtender" runat="server" Format="dd-MM-yyyy" TargetControlID="txtRosterDate">
                                   </asp:CalendarExtender>
                             <asp:Button runat="server" ID="btnSearch" CssClass="css_btn Ptbut" Text="Search" Width="125px" Height="34px" OnClick="btnSearch_Click"  /> 
                                  </td>
                                <td>                                    
                                     <asp:Button runat="server" ID="btnClose" CssClass="css_btn Ptbut" Text="Close" Width="65px" Height="34px"  />
                                      
                                </td> 
                                  <td>
                                      <asp:Label runat="server" ID="lblTotal" Text="T" Font-Bold="true" ForeColor="Yellow" style="text-align:center; border-radius:16px ; float:right;width:46px; font:bold; font-family:'Times New Roman'; border:solid; font-size:large"  ></asp:Label>
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
                     <asp:GridView HeaderStyle-BackColor="#750000" ID="gvEmpList"  runat="server" AutoGenerateColumns="false"  HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true"  HeaderStyle-Height="25px" HeaderStyle-Font-Size="14px" PageSize="25" Width="100%" DataKeyNames="EmpId" OnRowDataBound="gvEmpList_RowDataBound" OnRowCommand="gvEmpList_RowCommand" OnRowDeleting="gvEmpList_RowDeleting" >
                         <PagerStyle CssClass="gridview" />
                          <Columns>      
                                     <asp:TemplateField HeaderText="SL">
                                <ItemTemplate>
                                     
                                    <%#Container.DataItemIndex+1 %>                              
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" ForeColor="Red" />
                            </asp:TemplateField>                      
                           <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" ItemStyle-HorizontalAlign="Center" ItemStyle-Height="21px" />
                                 <asp:BoundField DataField="EmpName" HeaderText="Name" />                                 
                                 <asp:BoundField DataField="DptName" HeaderText="Department" ItemStyle-HorizontalAlign="Center" />                                
                                 <%--<asp:BoundField DataField="EmpType" HeaderText="Employee Type" ItemStyle-HorizontalAlign="Center" />--%>


                                <asp:TemplateField>
                                  <HeaderTemplate>
                                      Delete
                                  </HeaderTemplate>
                                  <ItemTemplate>
                                      <asp:Button  Height="30px" ForeColor="red"  ID="btnDelete" runat="server" CommandName="Remove" Text="Delete Full roseter just this employee"  OnClientClick=" return confirm('Do you want to delete full roster of this employee ?')"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                  </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Center" />
                              </asp:TemplateField>  
                              
                              <asp:TemplateField HeaderText="Date"  ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                  <ItemTemplate >
                                   <asp:TextBox Height="28px" Style=" border:1px solid; text-align:center;" ID="txtAttDate" runat="server" Enabled="true" ForeColor="Blue"   ></asp:TextBox> 
                                   <asp:CalendarExtender ID="txtAttDate_CalendarExtender" runat="server" Format="dd-MM-yyyy" TargetControlID="txtAttDate">
                                   </asp:CalendarExtender>
                                  </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                               </asp:TemplateField>

                                  <asp:TemplateField>
                                  <HeaderTemplate>
                                      Delete By Single Date
                                  </HeaderTemplate>
                                  <ItemTemplate >
                                      <asp:Button  Height="30px" ForeColor="DarkRed" ID="btnDeleteBySingleDate" runat="server" CommandName="RemoveBySingleDate" Text="Delete single date roster of this employee "  OnClientClick=" return confirm('Do you want to delete single date roster of this employee ?')"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />                                 
                                   </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Center" />
                              </asp:TemplateField> 


                                                                                                     
                                 <%--<asp:TemplateField HeaderText="Remove" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" Visible="true">
                                 
                                  <ItemTemplate >                           
                                      <asp:ImageButton ID="ibRemove" runat="server" CommandName="Remove" Width="55px" Height="28px" ImageUrl="~/images/action/Delete_Icon.png"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' OnClientClick="return confirm('Do you want to remove ?')" />
                                       </ItemTemplate>
                              </asp:TemplateField>   --%>                                                                       
                          </Columns>
                     </asp:GridView>
                         
                </div>
                   </ContentTemplate>
                </asp:UpdatePanel>
        </div>
     </div>
</asp:Content>
