<%@ Page Title="Leave Approval Panel" Language="C#" MasterPageFile="~/leave_nested.master" AutoEventWireup="true" CodeBehind="for_approve_leave_list.aspx.cs" Inherits="SigmaERP.personnel.for_approve_list" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../scripts/jquery-1.8.2.js"></script>
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
        #ContentPlaceHolder1_MainContent_gvForApprovedList td, th {
            text-align: center;
        }

            #ContentPlaceHolder1_MainContent_gvForApprovedList td:first-child, th:first-child {
                text-align: left;
                padding-left: 3px;
            }

        .tblSelection {
            width: 95%;
            margin: 0px auto;
        }

        .tdWidth {
           width:25%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <div class="row">
                  <div class="col-md-12 ds_nagevation_bar">
               <div style="margin-top: 5px">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                       <li>/</li>
                       <li> <a href="/leave_default.aspx">Leave</a></li>
                       <li>/</li>
                       <li> <a href="#" class="ds_negevation_inactive Lactive">Leave Approval Panel</a></li>
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
    
    <%--<div class="list_main_box">

                <div style="width: 80%; background-color:#27235C" class="list_main_content_box_header"  id="divElementContainer"  runat="server"> --%>
     <div class="main_box Lbox">
        <div runat="server" id="divElementContainer" class="main_box_header_leave LBoxheader">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                     
                            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                            <asp:AsyncPostBackTrigger ControlID="ddlShiftName" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDepartmentList" />
                            <asp:AsyncPostBackTrigger ControlID="ddlFindingType" />
                            
                        </Triggers>
                        <ContentTemplate>
                      <h2 style="border-bottom:1px solid white">Leave Approval Panel</h2>
                            <%--<h2 style="float: right; margin-top: -42px;">
                          <asp:LinkButton runat="server" ID="lnkRefresh" Text="Refresh" ForeColor="gray" OnClick="lnkRefresh_Click"></asp:LinkButton> |
                          <a style="color:gray" href="../leave_default.aspx">Close</a></h2>--%>
                    
                 
               <div hidden="hidden" >
                         <table class="tblSelection">
                              <tr>
                                  
                                        <td>Company</td>
                                        <td>:</td>
                                        <td class="tdWidth">
                                        <asp:DropDownList ID="ddlCompanyList" ClientIDMode="Static" style="margin-left: 10px; width: 96%;"   CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged"  >              
                                         </asp:DropDownList>
                                        </td>         
                          <asp:Panel runat="server" Visible="false">
                                  <td>Shift</td>
                                        <td>:</td>
                                        <td class="tdWidth">
                                            <asp:DropDownList ID="ddlShiftName" CssClass="form-control select_width" runat="server" Width="96%" AutoPostBack="True" OnSelectedIndexChanged="ddlShiftName_SelectedIndexChanged" ></asp:DropDownList>
                                        </td>
                                  </asp:Panel>
                                 <td>
                                     Department
                                 </td>
                                 <td>:</td>
                                 <td class="tdWidth">
                                     <asp:DropDownList runat="server" ID="ddlDepartmentList" CssClass="form-control text_box_width style" Width="96%" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartmentList_SelectedIndexChanged" ></asp:DropDownList>
                                 </td>
                                                    
                                 
                                  <td>
                                     Type
                                 </td>
                                 <td>:</td>
                                 <td>
                                     <asp:DropDownList runat="server" ID="ddlFindingType" CssClass="form-control text_box_width style" Width="95px" AutoPostBack="True" OnSelectedIndexChanged="ddlFindingType_SelectedIndexChanged" >
                                        <%-- <asp:ListItem Value="Today">Today</asp:ListItem>--%>
                                         <asp:ListItem Value="All">All</asp:ListItem>
                                     </asp:DropDownList>
                                 </td>
                                  <td><h2 style="float: right;"><asp:LinkButton runat="server" ID="lnkRefresh" Text="Refresh" ForeColor="white" OnClick="lnkRefresh_Click"></asp:LinkButton></h2></td>
                                  <td style="color:gray;font-size:medium; ">|</td>
                                  <td><h2 style="float: right; "><a style="color:white" href="../leave_default.aspx">Close</a></h2></td>
                                 
                             </tr>
                         </table>
                  
                     </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
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
                        
                     <asp:GridView HeaderStyle-BackColor="#27235C" ID="gvForApprovedList" runat="server" AutoGenerateColumns="false"  HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="14px" AllowPaging="true" PageSize="40" Width="100%" DataKeyNames="LACode,EmpID" OnRowCommand="gvForApprovedList_RowCommand" OnRowDataBound="gvForApprovedList_RowDataBound">
                         <PagerStyle CssClass="gridview" />
                          <Columns>
                             <asp:BoundField DataField="LACode" HeaderText="LACode" Visible="false" />    
                              <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" Visible="true" ItemStyle-Width="200px" ItemStyle-Height="25px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Height="28px"  />
                             <asp:BoundField DataField="EmpName" HeaderText="Name" Visible="true" ItemStyle-Width="200px" ItemStyle-Height="25px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Height="28px"  />
                             <asp:BoundField DataField="LeaveName" HeaderText="Leave" Visible="true" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="188px"  />
                             
                              <asp:TemplateField HeaderText="From Date" >
                                <ItemTemplate >
                                    <center>
                                    <asp:TextBox ID="txtFromDate"  runat="server" CssClass="form-control text_box_width" Enabled="false"   Width="83px" Text='<%#(Eval("FromDate").ToString()) %>' ></asp:TextBox>
                                    <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                 Enabled="True"
                                                TargetControlID="txtFromDate" ID="CExtApplicationDate">
                              </asp:CalendarExtender>
                                        </center>
                                </ItemTemplate>
                              </asp:TemplateField>
                              
                              <%--<asp:BoundField DataField="FromDate" HeaderText="From" Visible="true" ItemStyle-HorizontalAlign="Center"  />--%>
                              
                              
                              <%-- 
                              <asp:BoundField DataField="ToDate" HeaderText="To" Visible="true" ItemStyle-HorizontalAlign="Center" />
                              --%>
                              
                           <asp:TemplateField HeaderText="To Date" >
                               <ItemTemplate>
                                   <center>
                                   <asp:TextBox ID="txtToDate"  runat="server" CssClass="form-control text_box_width"   Enabled="false"  Width="83px" Text='<%#(Eval("ToDate").ToString()) %>'  ></asp:TextBox>

                                   <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate" Format="dd-MM-yyyy"  >

                                   </asp:CalendarExtender>
                                   </center>
                               </ItemTemplate>
                           </asp:TemplateField>

                              <asp:TemplateField  HeaderText="Total Days">
                                  <ItemTemplate>
                                      <center>
                                      <asp:Table runat="server">
                                          <asp:TableRow>
                                              <asp:TableCell>
                                                  <asp:Button runat="server" ID="btnEqual" Height="38px" Text="=" Font-Bold="true" ForeColor="Red" Width="30px"  CommandName="Calculation" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                                  
                                               </asp:TableCell>
                                              <asp:TableCell>
                                                   <asp:TextBox ID="txtTotalDays" runat="server" Width="36px" style="text-align:center" CssClass="form-control text_box_width" Enabled="false" Font-Bold="true" ForeColor="Red" Text='<%#(Eval("TotalDays").ToString()) %>'></asp:TextBox>
                                              </asp:TableCell>
                                          </asp:TableRow>
                                      </asp:Table>
                                          </center>
                                  </ItemTemplate>
                              </asp:TemplateField>
                             
                             
                               <asp:BoundField DataField="EntryDate" HeaderText="Requsted" Visible="true" ItemStyle-HorizontalAlign="Center"  />
                            
                                <asp:TemplateField HeaderText="Change" HeaderStyle-Width="35px">
                                  <ItemTemplate >
                                      <asp:Button ID="btnEdit" runat="server" Width="55px" Height="30px" Font-Bold="true" ForeColor="blue" Text="Edit" CommandName="Alter" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                  </ItemTemplate>
                              </asp:TemplateField>

                               <asp:TemplateField HeaderText="Details" HeaderStyle-Width="35px">
                                  <ItemTemplate>
                                      <asp:Button ID="btnView" runat="server" CommandName="View" Font-Bold="true" Text="View" Width="55px" Height="30px" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                  </ItemTemplate>
                              </asp:TemplateField>  

                              <asp:TemplateField HeaderText="Approval" HeaderStyle-Width="80px">
                                  <ItemTemplate>
                                      <asp:Table runat="server" >
                                          <asp:TableRow>
                                                 <asp:TableCell>
                                                   <asp:Button ID="btnForword"  Text="Forword" CommandName="Forword" runat="server" Width="80px" Height="30px" Font-Bold="true" ForeColor="Blue" OnClientClick="return confirm('Do you want to Forword ?')" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' CssClass="hand" />
                                              </asp:TableCell>
                                              <asp:TableCell>
                                                  |
                                              </asp:TableCell>
                                              <asp:TableCell>
                                                   <asp:Button ID="btnYes"  Text="Approve" CommandName="Yes" runat="server" Width="80px" Height="30px" Font-Bold="true" ForeColor="Green" OnClientClick="return confirm('Do you want to approved ?')" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' CssClass="hand" />
                                              </asp:TableCell>
                                              <asp:TableCell>
                                                  |
                                              </asp:TableCell>
                                              <asp:TableCell>
                                                  <asp:Button ID="btnNot"  Text="Reject" CommandName="No" Font-Bold="true" Height="30px" ForeColor="red" runat="server" OnClientClick="return confirm('Do you want to not approved ?')" Width="80px" CommandArgument='<%#((GridViewRow)Container).RowIndex%>'  />
                                              </asp:TableCell>
                                          </asp:TableRow>
                                      </asp:Table>
                                     <%--  --%>
                                     <%-- <asp:Label ID="btnSeperator"  Text="|" CommandName="No" runat="server"  />--%>
                                       

                                  </ItemTemplate>
                              </asp:TemplateField>
                                                                   
                          </Columns>
                         <HeaderStyle BackColor="#5EC1FF" ForeColor="White" Height="28px" />
                     </asp:GridView>
                         <div id="divRecordMessage" runat="server" visible="false" style="color: red; font-weight: bold; text-align: center; padding-top: 75px; font-size: 32px; height: 118px">
                         </div>
                </div>
                   </ContentTemplate>
                </asp:UpdatePanel>
        </div>
    
    <style type=”text/css”>
    .hand { cursor: pointer; cursor: hand; } /* cross browser hand */
</style>
    <script type="text/javascript">
        function LeaveDaysCaclulate(ob) {
          
            
            var start = new Date("2014-06-01");
            var td = ob.value;
            var end = new Date(td);
            var diff = new Date(end - start);
            var days = diff / 1000 / 60 / 60 / 24;
            alert(days);
            var par = $(ob).closest('tr');
            var d = par[0];
            var r = d.find('td');
        }
        function goToNewTabandWindow(url) {
            window.open(url);
        }
    </script>

</asp:Content>
