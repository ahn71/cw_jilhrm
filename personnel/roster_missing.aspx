<%@ Page Title="" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="roster_missing.aspx.cs" Inherits="SigmaERP.personnel.roster_missing" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

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
                       <li><a href="#" class="ds_negevation_inactive Ptactive">Roster Missing Panel</a></li>
                   </ul>
               </div>
          
             </div>
       </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate>
        <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
    </ContentTemplate>
</asp:UpdatePanel>    


     <div class="row Ptrow">
        <div class="employee_box_header PtBoxheader">
            <div class="employee_box_header PtBoxheader" style="border-bottom: 1px solid white">
                <h2>Roster Missing &amp; Add Panel</h2>

            </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>                     
                            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />                                                                                              
                            <asp:AsyncPostBackTrigger ControlID="ddlExistsShiftList" />
                            
                            <asp:AsyncPostBackTrigger ControlID="ddlDepartmentList" />      
                           
                            <asp:AsyncPostBackTrigger ControlID="ddlAssignShift" />                  
                        </Triggers>
                        <ContentTemplate>                      
                          
               <div style="margin-left:15px">
                        <table style="border-collapse: collapse;width:100%; padding: 5px 0px;">
                              <tr>
                                    <td>
                                       Company
                                      </td>
                                       <td >
                                     <asp:DropDownList runat="server" ID="ddlCompanyList" CssClass="form-control text_box_width style" Width="200px" AutoPostBack="True" Enabled="False"   ></asp:DropDownList>
                                 </td>
                                  <td>
                                       Group
                                      </td>
                                       <td >
                                     <asp:DropDownList runat="server" ID="ddlGroupList" CssClass="form-control text_box_width style" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlGroupList_SelectedIndexChanged"   ></asp:DropDownList>
                                 </td>
                                  <td >
                                      Deparment 
                                      </td>
                                       <td >
                                     <asp:DropDownList runat="server" ID="ddlDepartmentList" CssClass="form-control text_box_width style" Width="200px" AutoPostBack="True" ClientIDMode="Static" OnSelectedIndexChanged="ddlDepartmentList_SelectedIndexChanged"  ></asp:DropDownList>
                                 </td>   
                                  <td >
                                      Shift 
                                      </td>
                                       <td>
                                     <asp:DropDownList runat="server" ID="ddlAssignShift" CssClass="form-control text_box_width style" Width="200px"  AutoPostBack="True" ClientIDMode="Static"  ></asp:DropDownList>
                                 </td>     
                                 
                                   <td>Date</td>
                                <td>
                                       <asp:TextBox ID="txtDate" runat="server"   Style=" text-align:center "  ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                     
                                     <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                PopupButtonID="imgEffectDateFrom" Enabled="True"
                                                TargetControlID="txtDate" ID="CExtApplicationDate">
                                            </asp:CalendarExtender>
                                  </td> 
                                  
                                  <td>  <asp:Button runat="server" ID="btnSearch" CssClass="css_btn Ptbut" Text="Search" Width="70px" Height="34px" OnClick="btnSearch_Click"  /></td>


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
                 <div style="width: 100%; background-color: #fff; margin: auto;">
                     <div id="divRecordMessage" runat="server" visible="false" style="color: red; font-weight: bold; text-align: center; padding-top: 75px; font-size: 32px; height: 118px">
                     </div>
                     <asp:GridView HeaderStyle-BackColor="#750000" ID="gvEmpList" runat="server" AutoGenerateColumns="false" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Height="25px" HeaderStyle-Font-Size="14px" PageSize="25" Width="100%" DataKeyNames="EmpId,DptId,DsgId,EmpTypeId,GId" OnRowDataBound="gvEmpList_RowDataBound" OnRowCommand="gvEmpList_RowCommand">
                         <PagerStyle CssClass="gridview" />
                         <Columns>
                             <asp:TemplateField HeaderText="SL">
                                 <ItemTemplate>
                                     <%# Container.DataItemIndex+1 %>
                                 </ItemTemplate>
                                 <ItemStyle HorizontalAlign="Center" Font-Bold="true" ForeColor="Red" />
                             </asp:TemplateField>
                             <asp:BoundField DataField="EmpCardNo" HeaderText="CardNo" ItemStyle-HorizontalAlign="Center" ItemStyle-Height="21px" />
                             <asp:BoundField DataField="EmpName" HeaderText="Name" />
                             <asp:BoundField DataField="DsgName" HeaderText="Designation" ItemStyle-HorizontalAlign="Center" />
                             <asp:BoundField DataField="EmpType" HeaderText="Employee Type" ItemStyle-HorizontalAlign="Center" />
                             <asp:TemplateField HeaderText="Add" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" Visible="true">

                                 <ItemTemplate>
                                     <asp:ImageButton ID="iAdd" runat="server" CommandName="RosterAdd" Width="55px" Height="28px" ImageUrl="~/images/action/add.jpg" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                 </ItemTemplate>
                             </asp:TemplateField>

                         </Columns>
                     </asp:GridView>

                 </div>

                 <div style="border-radius: 5px; border: 2px solid #086A99; border-top: 0px; font-weight: bold; width: 380px; background: #ddd; padding: 5px;" id="PopupWindow">
                     <center><p style="font-size: 17px;color:green;font-weight:bold; cursor:move" id="drag"> Roster Setting Panel</p></center>
                     <hr />

                     <table>
                         <tr>
                             <td>
                                 <asp:CheckBox Checked="true" ForeColor="Green" runat="server" Text=" For Full Roster" ID="chkForFullRoster" AutoPostBack="True" OnCheckedChanged="chkForFullRoster_CheckedChanged"  />
                             </td>
                             <td>
                                 <asp:CheckBox runat="server" AutoPostBack="true" ForeColor="Blue" Text="" ID="chkForDate" OnCheckedChanged="chkForDate_CheckedChanged"  />
                             </td>

                         </tr>
                     </table>
                     <hr />
                     Select Roster 
                
               
                    <asp:DropDownList runat="server" ID="ddlExistsShiftList" CssClass="form-control text_box_width style" ForeColor="Red" Width="92%" Height="30px" AutoPostBack="True" ClientIDMode="Static" Font-Size="14px" Font-Names="Arial" OnSelectedIndexChanged="ddlExistsShiftList_SelectedIndexChanged" ></asp:DropDownList>
                     <asp:ImageButton runat="server" ImageUrl="~/images/action/Error.png" ID="btnClosePopup" Style="width: 7%; float: right; margin-top: -30px; margin-right: -2px;" />

                 </div>

                 <asp:Button runat="server" ID="btnclick" Text="Popup Click" Width="75px" style="display:none" />
                 <asp:ModalPopupExtender CacheDynamicResults="false" PopupDragHandleControlID="drag" Drag="true" Enabled="true" CancelControlID="btnClosePopup" ID="ModelPopupExtender" PopupControlID="PopupWindow" TargetControlID="btnclick" runat="server"></asp:ModalPopupExtender>
             </ContentTemplate>
         </asp:UpdatePanel>
     </div>


</asp:Content>
