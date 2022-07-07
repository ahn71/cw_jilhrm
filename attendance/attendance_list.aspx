<%@ Page Title="Attendance List" Language="C#" MasterPageFile="~/attendance_nested.Master" AutoEventWireup="true" CodeBehind="attendance_list.aspx.cs" Inherits="SigmaERP.attendance.attendance_list" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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

    <style type="text/css">
        .style {
            font-size: 17px;
            font-weight: bold;
            width: 95px;
        }

        #ContentPlaceHolder1_MainContent_gvAttendanceList th {
            text-align: center;
        }

        #ContentPlaceHolder1_MainContent_divElementContainer h2 {
            font-size: 16px;
            padding: 0px;
            text-align: center;
        }

        #ContentPlaceHolder1_MainContent_gvAttendanceList th:nth-child(2),td:nth-child(2) {
           text-align: left;
           padding-left: 3px;
        }
     
        .emp_header_left {
            font-size: 16px;
            padding-left: 10px;
            text-align: left;
            font: bold;
            margin-top:10px;
        }
           .emp_header_right {
            float: right;
            margin-top: -37px;
            font: bold;
        }
          
    </style>
  <script type="text/javascript">
      //$(function () {
      //    alert();
      //    $("#btnForGet").click(function(){

      //        alert("Yes U Pressed");
      //    })
      //})
    </script>
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
                    <li><a href="#" class="ds_negevation_inactive Mactive">Attendance List</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>

    <div style="padding:0;margin-top:25px;max-width:100%;">
    <div class="row Rrow">
                <div id="divElementContainer" runat="server" class="list_main_content_box_header MBoxheader" style="width: 100%;">
                     
                 <%--<div style="overflow: hidden;margin-bottom: 5px; border-bottom: 1px solid #ddd;">--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
           <asp:AsyncPostBackTrigger  ControlID="ddlDepartmentName" />
            <asp:AsyncPostBackTrigger  ControlID="ddlShift" />
            <asp:AsyncPostBackTrigger  ControlID="ddlGrouping" />

        </Triggers>
    <ContentTemplate>
        <div style="overflow: hidden; margin-bottom: 5px; border-bottom: 1px solid #ddd;">
           
            <%--<h3 class="emp_header_left">--%>
             <h2 class="emp_header_left" style="float: left; width:78%;">
                <p style="text-shadow: 5px 5px 5px rgb(0, 0, 0); font-size: 20px; font-weight: 500; text-align: center;width:1142px; padding-top:5px">Attendance List</p>
                 <h2></h2>
                 <h2 class="emp_header_right">
                     <!--<a href="/leave/aplication.aspx">Close</a>-->
                     <asp:Button ID="Button1" runat="server" CssClass="Mbutton" Height="34px"  PostBackUrl="~/attendance_default.aspx" Style="border: 1px solid;" Text="Close" Width="75px" />
                     <asp:Button ID="btnRefresh" runat="server" CssClass="Mbutton" Height="34px" OnClick="btnRefresh_Click" Text="Refresh" Width="75px" />
                     <asp:Button ID="btnClear" runat="server" CssClass="Mbutton" Height="34px" OnClick="btnClear_Click" Text="Clear" Width="75px" />
                 </h2>
           </h2>
        </div>
        <%--</div>--%>
        <div style="width: 100%;">
             <table style="margin: 0 0 5px 6px; border-collapse: collapse;" width="99%">
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
                             <asp:DropDownList ID="ddlCompanyList" ClientIDMode="Static" CssClass="form-control inline_form_text_box_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged">
                             </asp:DropDownList>
                         </td>  

                         <td>
                             <asp:DropDownList ID="ddlDepartmentName" CssClass="form-control inline_form_text_box_width" runat="server"  AutoPostBack="True" OnSelectedIndexChanged="ddlDivisionName_SelectedIndexChanged"></asp:DropDownList>
                         </td>
                           <td>
                             <asp:DropDownList runat="server" ID="ddlShift" CssClass="form-control inline_form_text_box_width" AutoPostBack="True" OnSelectedIndexChanged="ddlShift_SelectedIndexChanged"></asp:DropDownList>
                         </td>
                         <td>
                             <asp:DropDownList ID="ddlGrouping" CssClass="form-control inline_form_text_box_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlGrouping_SelectedIndexChanged"></asp:DropDownList>
                         </td>
                       
                         <td>
                             <asp:TextBox ID="txtCardNo" runat="server" CssClass="form-control inline_form_text_box_width" style="width: 100px !important;"  ClientIDMode="Static" MaxLength="12"></asp:TextBox>
                         </td>
                         <td>
                             <asp:DropDownList runat="server" ID="ddlChoseYear" CssClass="form-control inline_form_text_box_width" style="width: 100px !important;" AutoPostBack="True" OnSelectedIndexChanged="ddlChoseYear_SelectedIndexChanged"></asp:DropDownList>
                         </td>

                         <td>
                             <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control inline_form_text_box_width" style="width: 100px !important;"  autocomplete="off" ClientIDMode="Static" MaxLength="12"></asp:TextBox>
                             <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Format="dd-MM-yyyy" TargetControlID="txtFromDate">
                             </asp:CalendarExtender>
                         </td>

                         <td>
                             <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control inline_form_text_box_width" style="width: 100px !important;"  autocomplete="off"  ClientIDMode="Static" MaxLength="12"></asp:TextBox>
                             <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Format="dd-MM-yyyy" TargetControlID="txtToDate">
                             </asp:CalendarExtender>
                         </td>
                         <td><asp:Button runat="server" ID="btnSearch" CssClass="Mbutton" Text="Search" Width="75px" Height="34px" OnClick="btnSearch_Click" /></td>                     


                     </tr>
                 
                 </table>
            <div>
                <asp:RadioButtonList runat="server" ID="rblAttListType" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblAttListType_SelectedIndexChanged">
                <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                <asp:ListItem Value="1" >Out Duty</asp:ListItem>
                <asp:ListItem Value="2" >Training</asp:ListItem>
                 </asp:RadioButtonList></div>
        </div>
                     <%--</h2>--%>
                            </ContentTemplate>
</asp:UpdatePanel>
                </div>
                    <%--</div>--%>
                 <%-- <div class="dataTables_wrapper" style="width:944px; margin:0px auto; background-color:white">--%>

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
                     <asp:GridView HeaderStyle-BackColor="#2B5E4E" HeaderStyle-Height="28px" ID="gvAttendanceList" runat="server" AutoGenerateColumns="false"  HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="14px" AllowPaging="true" PageSize="40" Width="100%" DataKeyNames="EmpId,EmpCardNo,AttDate,AttStatus,InTime,OutTime,EmpType,EmpName,StateStatus" OnPageIndexChanging="gvAttendanceList_PageIndexChanging" OnRowCommand="gvAttendanceList_RowCommand" OnRowDeleting="gvAttendanceList_RowDeleting" OnRowDataBound="gvAttendanceList_RowDataBound"  >
                         <PagerStyle CssClass="gridview" />
                          <Columns>
                             <asp:BoundField DataField="EmpId" HeaderText="LACode" Visible="false" />                              
                              <asp:BoundField DataField="StateStatus" HeaderText="StateStatus" Visible="false" />
                              <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" Visible="true"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                              <asp:BoundField DataField="EmpName" HeaderText="Name" />
                             <asp:BoundField DataField="MonthId" HeaderText="Month" Visible="true"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>
                             <asp:BoundField DataField="AttDate" HeaderText="Date" Visible="true"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>
                             <asp:BoundField DataField="AttStatus" HeaderText="Status" Visible="true"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true"/>
                             <asp:BoundField DataField="AttManual" HeaderText="Count" Visible="true"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>
                             <asp:BoundField DataField="InTime" HeaderText="In Time" Visible="true" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="Green" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="15px"/>
                             <asp:BoundField DataField="OutTime" HeaderText="Out Time" Visible="true" ItemStyle-Font-Bold="true"  ItemStyle-ForeColor="red" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Font-Size="15px" />
                             <asp:BoundField DataField="EmpType" HeaderText="Type" Visible="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            <%-- <asp:ButtonField ButtonType="Button" HeaderText="Alter" Text="Alter" ControlStyle-Width="60px" CommandName="Alter" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>--%>
                            
                              
                            
                              <asp:TemplateField>
                                  <HeaderTemplate>
                                      Edit
                                  </HeaderTemplate>
                                 <ItemTemplate  >
                                     <asp:Button ID="btnAlter" runat="server" Text="Edit" ForeColor="Green" Font-Bold="true" CommandName="Alter" Width="60px" CommandArgument='<%#((GridViewRow)Container).RowIndex%>'/>
                                 </ItemTemplate>
                                  <HeaderStyle HorizontalAlign="Center" />
                                  <ItemStyle HorizontalAlign="Center" />
                              </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-VerticalAlign="Middle" >
                                        <HeaderTemplate>
                                            Delete
                                        </HeaderTemplate>
                                        <ItemTemplate >
                                            <asp:Button ID="btnRemove" Font-Bold="true" ForeColor="Red" runat="server" Text="Delete" CommandName="Delete" 
                                                OnClientClick="return confirm('Are you sure, you want to delete the record?')"
                                                CommandArgument='<%# Eval("EmpId")+","+Eval("EmpCardNo")+","+Eval("AttDate")+","+Eval("AttStatus") %>' ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
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
</asp:Content>
