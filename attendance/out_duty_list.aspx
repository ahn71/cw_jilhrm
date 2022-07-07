<%@ Page Title="Out Duty List" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="out_duty_list.aspx.cs" Inherits="SigmaERP.attendance.out_duty_list" %>
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
        .btngroup{
            padding-top:22px;
        }
        .od-src-box{
            padding-left:10px;
        }
        .list_main_content_box_header {
            overflow: inherit;
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
                    <li><a href="/attendance_default.aspx">Attendance</a></li>
                    <li>/</li>
                    <li><a href="#" class="ds_negevation_inactive Mactive">Out Duty List</a></li>
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

                <div id="divElementContainer" runat="server" class="list_main_content_box_header MBoxheader" style="width: 100%;">
                     
                
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                            <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
                            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDepartmentList" />
                            <asp:AsyncPostBackTrigger ControlID="gvOutDuty" />
                        </Triggers>
                        <ContentTemplate>
                            
                            <div class="container-fluid lla-control-box">
                                <div class="row">
                                    <div class="col-md-12 lla-title">
                                        <p>Out Duty List</p>
                                    </div>
                                    
                                </div>
                                <div class="row">
                                    <div class="col-md-6 lla-option-btn">
                                        <asp:RadioButtonList runat="server" ID="rblApprovedPending" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblApprovedPending_SelectedIndexChanged">
                                         <asp:ListItem Selected="True"   Value="1">Approved</asp:ListItem>
                                        <%-- <asp:ListItem Value="0" >Pending</asp:ListItem>--%>
                                         <asp:ListItem Value="2" >Rejected</asp:ListItem>
                                     </asp:RadioButtonList>
                                    </div>                                    
                                    <div class="col-md-6">
                                    <div class="pull-right">                            
                                    
                                    </div>
                                    </div>
                        </div>
                    </div>
                          
                                              
                 
               <div style="width:100%;">
                   <div class="row od-src-box">
                       <div class="col-md-2 hidden-xs">
                           <div class="form-group">
                                <label for="Company">Company</label>
                               <asp:DropDownList ID="ddlCompanyList" ClientIDMode="Static" CssClass="form-control " runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" >              
                                         </asp:DropDownList>
                           </div> 
                       </div>
                       <div class="col-md-2 hidden-xs">
                           <div class="form-group">
                                <label for="">Depertment</label>
                                <asp:DropDownList ID="ddlDepartmentList" CssClass="form-control " runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartmentList_SelectedIndexChanged"></asp:DropDownList>
                           </div> 
                       </div>
                       <div class="col-md-1 hidden-xs" runat="server" visible="false">
                           <div class="form-group">
                                <label for=""></label>
                                <asp:DropDownList ID="ddlGrouping" CssClass="form-control inline_form_text_box_width" runat="server" ></asp:DropDownList>
                           </div> 
                       </div>
                       <div class="col-md-1 hidden-xs">
                           <div class="form-group">
                            <label for="">Card No </label>
                           <asp:TextBox ID="txtCardNo" runat="server" CssClass="form-control "  ClientIDMode="Static" MaxLength="12"></asp:TextBox>
                       </div> 
                       </div>
                       <div class="col-md-1 hidden-xs">
                           <div class="form-group">
                                <label for="">Year </label>
                               <asp:DropDownList runat="server" ID="ddlChoseYear" CssClass="form-control "  AutoPostBack="True" OnSelectedIndexChanged="ddlChoseYear_SelectedIndexChanged"  ></asp:DropDownList>
                           </div> 
                       </div>
                       <div class="col-md-2">
                           <div class="form-group">
                                <label for="">From Date </label>
                               <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control "  ClientIDMode="Static" autocomplete="off" MaxLength="12"></asp:TextBox>
                                       <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Format="dd-MM-yyyy" TargetControlID="txtFromDate">
                                       </asp:CalendarExtender>
                           </div> 
                       </div>
                       <div class="col-md-2">
                           <div class="form-group">
                                <label for="">To Date </label>
                               <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control " ClientIDMode="Static" autocomplete="off" MaxLength="12"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server"  Format="dd-MM-yyyy" TargetControlID="txtToDate">
                                        </asp:CalendarExtender>
                           </div> 
                       </div>
                       <div class="col-md-2">
                       <div class="btngroup">
                           <div class="form-group pull-left">
                                <asp:Button runat="server" ID="btnSearch" CssClass="Mbutton" Text="Search" Width="75px" style="border:1px solid;" Height="34px" OnClick="btnSearch_Click"  />
                           </div>
                           <div class="form-group pull-left" style="display:none">
                                <asp:Button ID="btnRefresh" runat="server" CssClass="Mbutton" Height="34px" OnClick="btnRefresh_Click" style="border:1px solid;" Text="Refresh" Width="75px" />
                           </div>
                           <div class="form-group pull-left">
                               <asp:Button ID="btnClear" runat="server" CssClass="Mbutton" Height="34px" OnClick="btnClear_Click" style="border:1px solid;" Text="Clear" Width="75px" />
                           </div>
                       </div>
                       </div>
                   </div>
                   
                        
                     </div>


                            </ContentTemplate>
                        </asp:UpdatePanel>
                      </div>                   

               
                <asp:UpdatePanel runat="server" ID="up2">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                        <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
                    </Triggers>
                    <ContentTemplate>

                  <div class="table-responsive">
                  <div style="width: 100%; margin:0px auto ">
                    <asp:GridView ID="gvOutDuty" runat="server" HeaderStyle-BackColor="#2B5E4E" HeaderStyle-Height="28px" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="14px"  Width="100%" AutoGenerateColumns="False" DataKeyNames="SL" CellPadding="4" ForeColor="#333333" Height="13px" OnRowCommand="gvOutDuty_RowCommand" OnRowDataBound="gvOutDuty_RowDataBound">
                                    
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>

                                        <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1%>
                                            </ItemTemplate>

                                            <ItemStyle HorizontalAlign="Center" ForeColor="green" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Card No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("EmpCardNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("EmpName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Department">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDptName" runat="server" Text='<%# Eval("DptName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDsgName" runat="server" Text='<%# Eval("DsgName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Straight from home">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckbsfhome" runat="server" Enabled="false" Checked='<%# Eval("StraightFromHome") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Straight to home">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="ckbsthome" runat="server" Enabled="false" Checked='<%# Eval("StraightToHome") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" Font-Bold="true" runat="server" Text='<%# Eval("TypeName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" ForeColor="Blue" Font-Bold="true" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action By">
                                            <ItemTemplate>
                                                <asp:Label ID="lblActionBy"  Font-Bold="true" runat="server" Text='<%# Eval("AuthorizedByName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label  ID="lblOuttime" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                 <%--       <asp:TemplateField >
                                            <HeaderTemplate>
                                                Edit
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="btnEdit"  CssClass="btn-xs btn-info text-white" style="color:#ffffff" runat="server" Text="Edit" CommandName="editRow"
                                                    CommandArgument="<%# Container.DataItemIndex%>" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>--%>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Delete
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="btnView" runat="server" Text="Delete" CommandName="deleterow" class="btn-xs btn-danger"
                                                    OnClientClick="return confirm('Are you sure, you want to delete the record?')"
                                                    CommandArgument="<%# Container.DataItemIndex%>" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>

                                </asp:GridView>
                  </div>
                  </div>
                   </ContentTemplate>
                </asp:UpdatePanel>
        </div>
    </div>
  
 
    <script src="../scripts/jquery-1.8.2.js"></script>   
     <script type="text/javascript">
         function goToNewTabandWindow(url) {
             window.open(url);             
         }
    </script>
</asp:Content>

