<%@ Page Title="Leave List" Language="C#" MasterPageFile="~/leave_nested.Master" AutoEventWireup="true" CodeBehind="all_leave_list.aspx.cs" Inherits="SigmaERP.personnel.week_end_list_all" %>
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
        #ContentPlaceHolder1_MainContent_gvLeaveList th {
            text-align:center;
        }
        .lla-control-box {
            margin: 0 0 18px 0;
        }
        .lla-control-box .lla-title p{
            text-align: center;
            padding-bottom: 0px;
            font-size: 20px;
            text-shadow: 5px 5px 5px #000;
        }
        .lla-control-box .row{
            border-bottom: 1px solid #fff;
            padding:10px 0;
        }
        .lla-control-box .row:nth-child(1){
                padding: 5px 0px 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li>/</li>
                    <li><a href="/leave_default.aspx">Leave</a></li>
                    <li>/</li>
                    <li><a href="#" class="ds_negevation_inactive Lactive">Leave List</a></li>
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
   
    
    
     

<div style="padding:0;margin-top:25px;max-width:100%;">
    <div class="row Rrow">

                <div id="divElementContainer" runat="server" class="list_main_content_box_header LBoxheader" style="width: 100%;">
                     
                
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                            <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
                            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                            <asp:AsyncPostBackTrigger ControlID="ddlShift" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDepartmentList" />
                            <asp:AsyncPostBackTrigger ControlID="gvLeaveList" />
                        </Triggers>
                        <ContentTemplate>
                            
                            <div class="container-fluid lla-control-box">
                                <div class="row">
                                    <div class="col-md-12 lla-title">
                                        <p>Leave List</p>
                                    </div>
                                    
                                </div>
                                <div class="row">
                                    <div class="col-md-6 lla-option-btn">
                                        <asp:RadioButtonList runat="server" ID="rblApprovedPending" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblApprovedPending_SelectedIndexChanged">
                                         <asp:ListItem Selected="True"   Value="1">Approved</asp:ListItem>
                                         <asp:ListItem Value="0" >Pending</asp:ListItem>
                                         <asp:ListItem Value="Rejected" >Rejected</asp:ListItem>
                                     </asp:RadioButtonList>
                                    </div>
                                    
                                    <div class="col-md-6">
                                        <div class="pull-right">
                                         <!--<a href="/leave/aplication.aspx">Close</a>-->
                                     <asp:Button ID="Button1" runat="server" CssClass="Lbutton" Height="34px" PostBackUrl="~/leave_default.aspx" style="border:1px solid;" Text="Close" Width="75px" />
                                     <asp:Button ID="btnRefresh" runat="server" CssClass="Lbutton" Height="34px" OnClick="btnRefresh_Click" style="border:1px solid;" Text="Refresh" Width="75px" />
                                     <asp:Button ID="btnClear" runat="server" CssClass="Lbutton" Height="34px" OnClick="btnClear_Click" style="border:1px solid;" Text="Clear" Width="75px" />
                                    </div>
                            </div>
                        </div>
                    </div>
                          
                                              
                 
               <div style="width:100%;">
                    
                  <table width="99%" style="margin:0 0 5px 6px; border-collapse: collapse;">
                       <tr>
                            <td>Company</td>
                           <td>Depertment</td>
                            <td>Shift</td>
                            
                            <td>Line / Grp</td>
                            <td>Card No</td>
                            <td>Year</td>
                            <td>From Date</td>
                            <td>To Date</td>
                            <td></td>
                        </tr>
                       <tr>
                            <td>
                                 <asp:DropDownList ID="ddlCompanyList" ClientIDMode="Static" CssClass="form-control inline_form_text_box_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged"  >              
                                 </asp:DropDownList>
                            </td>
                           <td>
                                <asp:DropDownList ID="ddlDepartmentList" CssClass="form-control inline_form_text_box_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDivisionName_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                 <asp:DropDownList runat="server" ID="ddlShift" CssClass="form-control inline_form_text_box_width" AutoPostBack="True" OnSelectedIndexChanged="ddlShift_SelectedIndexChanged"  ></asp:DropDownList>
                            </td>
                            
                            <td>
                                <asp:DropDownList ID="ddlGrouping" CssClass="form-control inline_form_text_box_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlGrouping_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCardNo" runat="server" CssClass="form-control inline_form_text_box_width" style="width:100px !important;" ClientIDMode="Static" MaxLength="12"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlChoseYear" CssClass="form-control inline_form_text_box_width" style="width:100px !important;" AutoPostBack="True" OnSelectedIndexChanged="ddlChoseYear_SelectedIndexChanged"  ></asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control inline_form_text_box_width" style="width: 100px !important;" ClientIDMode="Static" MaxLength="12"></asp:TextBox>
                               <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Format="dd-MM-yyyy" TargetControlID="txtFromDate">
                               </asp:CalendarExtender>
                            </td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control inline_form_text_box_width" style="width: 100px !important;" ClientIDMode="Static" MaxLength="12"></asp:TextBox>
                                <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server"  Format="dd-MM-yyyy" TargetControlID="txtToDate">
                                </asp:CalendarExtender>
                            </td>
                            <td><asp:Button runat="server" ID="btnSearch" CssClass="Lbutton" Text="Search" Width="75px" style="border:1px solid;" Height="34px" OnClick="btnSearch_Click"  /></td>
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
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                        <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
                    </Triggers>
                    <ContentTemplate>

                  <div style="width: 100%; margin:0px auto ">
                       <asp:GridView HeaderStyle-BackColor="#5EC1FF" ID="gvLeaveList" HeaderStyle-Height="28px" runat="server" AutoGenerateColumns="false" BackColor="White" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="14px" AllowPaging="true" PageSize="40" Width="100%" DataKeyNames="LACode" OnRowCommand="gvLeaveList_RowCommand" OnRowDataBound="gvLeaveList_RowDataBound" OnPageIndexChanging="gvLeaveList_PageIndexChanging" OnRowDeleting="gvLeaveList_RowDeleting">
                          <PagerStyle CssClass="gridview" />
                          <Columns>
                              <asp:BoundField DataField="LACode" HeaderText="LACode" Visible="false" />
                              <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" Visible="true"  ItemStyle-HorizontalAlign="Center" />
                              <asp:BoundField DataField="FromDate" HeaderText="From Date" Visible="true"  ItemStyle-HorizontalAlign="Center" />
                              <asp:BoundField DataField="ToDate" HeaderText="To Date" Visible="true"   ItemStyle-HorizontalAlign="Center"/>
                              <%--<asp:BoundField DataField="WeekHolydayNo" HeaderText="Total Week." Visible="true" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />--%>
                              <asp:BoundField DataField="ApplyDate" HeaderText="Apply Date" Visible="true" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                              <asp:BoundField DataField="TotalDays" HeaderText="Total Days" Visible="true" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="Red" />
                              <asp:BoundField DataField="IsApproved" HeaderText="Approved" Visible="false" ItemStyle-HorizontalAlign="Center"  />
                              <asp:BoundField DataField="CurrentProcessStatus" HeaderText="Processed" Visible="true" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center"  />
                              <asp:BoundField DataField="LeaveName" HeaderText="LeaveName" Visible="true"   ItemStyle-HorizontalAlign="Center" />
                              <%--  <asp:ButtonField ButtonType="Button" HeaderText="Alter" Text="Alter" CommandName="Alter" />--%>
                                 <asp:TemplateField>
                                  <HeaderTemplate>
                                      View
                                  </HeaderTemplate>
                                  <ItemTemplate>
                                      <asp:Button runat="server" ID="btnReport" Text="View" Font-Bold="true" CommandName="View" ForeColor="Green" CommandArgument='<%# Eval("LACode") %>' />
                                  </ItemTemplate>
                                   <ItemStyle HorizontalAlign="Center" />
                              </asp:TemplateField>
                              <asp:TemplateField>
                                  <HeaderTemplate>
                                      Edit
                                  </HeaderTemplate>
                                  <ItemTemplate>
                                      <asp:Button runat="server" ID="btnEdit" Text="Edit" Font-Bold="true" CommandName="Alter" ForeColor="Green" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                  </ItemTemplate>
                                   <ItemStyle HorizontalAlign="Center" />
                              </asp:TemplateField>
                              <asp:TemplateField>
                                  <HeaderTemplate>
                                      Delete
                                  </HeaderTemplate>
                                  <ItemTemplate>
                                      <asp:Button ID="btnView" runat="server" Text="Delete" Font-Bold="true" CommandName="Delete" ForeColor="Red"
                                          OnClientClick="return confirm('Are you sure, you want to delete the record?')"
                                          CommandArgument='<%# Eval("LACode") %>' />
                                  </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Center" />
                              </asp:TemplateField>
                          </Columns>
                      </asp:GridView>
                      <div id="divRecordMessage" runat="server" visible="false" style="color: red; background-color:white; font-weight: bold; text-align: center; padding-top: 75px; font-size: 32px; height: 118px">                           
                         </div>
                  </div>
                   </ContentTemplate>
                </asp:UpdatePanel>
        </div>
    </div>
  
 
    <script src="../scripts/jquery-1.8.2.js"></script>
   <%-- <script type="text/javascript">

        var oldgridcolor;
        function SetMouseOver(element) {
            oldgridcolor = element.style.backgroundColor;
            element.style.backgroundColor = '#ffeb95';
            element.style.cursor = 'pointer';
            element.style.textDecoration = 'underline';
        }
        function SetMouseOut(element) {
            element.style.backgroundColor = oldgridcolor;
            element.style.textDecoration = 'none';

        }

</script>--%>
    
     <script type="text/javascript">
         function goToNewTabandWindow(url) {
             window.open(url);             
         }
    </script>
</asp:Content>
