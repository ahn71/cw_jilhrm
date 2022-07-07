<%@ Page Title="Roster Manage Report" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="shift_manage_report.aspx.cs" Inherits="SigmaERP.personnel.shift_manage_report" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tdWidth {
            width: 250px;
        }

        #ContentPlaceHolder1_MainContent_gvEmpList th {
            text-align: center;
        }

        #ContentPlaceHolder1_MainContent_gvEmpList th:nth-child(2), td:nth-child(2) {
            padding-left: 3px;
            text-align: left;
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
                    <li><a href="#" class="ds_negevation_inactive Ptactive">Roster Manage Report</a></li>
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
                <h2>Roster Manage Report</h2>
            </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>                     
                            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />                                                                                              
                            <asp:AsyncPostBackTrigger ControlID="chkAllAssigned" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDepartmentList" />   
                            <asp:AsyncPostBackTrigger ControlID="lnkNew" />                       
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
               <div>
                  <h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center">You Have Not Any Access Permission!</h1>
                     <table style="width:98%;margin:0px auto;">
                              <tr>
                                    <td>
                                       Company
                                      </td>
                                       <td class="tdWidth">
                                     <asp:DropDownList runat="server" ID="ddlCompanyList" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ></asp:DropDownList>
                                 </td>                                 
                                  <td>
                                      Deparment 
                                      </td>
                                       <td class="tdWidth">
                                     <asp:DropDownList runat="server" ID="ddlDepartmentList" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True"  OnSelectedIndexChanged="ddlDepartmentList_SelectedIndexChanged"></asp:DropDownList>
                                 </td>                                                            
                                    <td>
                                     Shift 
                                      </td>
                                       <td class="tdWidth">
                                     <asp:DropDownList runat="server" ID="ddlShift" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True" ForeColor="Red"></asp:DropDownList>
                                 </td>  
                                  <td>
                                      <asp:CheckBox ID="chkAllAssigned" Text="All Assigned Shift" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllAssigned_CheckedChanged" />
                                  </td>                          
                             </tr>
                            <tr>
                                 <td>By Date</td>
                                <td class="tdWidth">
                                     <asp:TextBox ID="txtDate"  ClientIDMode="Static"  runat="server" CssClass="form-control text_box_width" MaxLength="10" style="width:96%; text-align:center" ></asp:TextBox>
                                     <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDate" Format="dd-MM-yyyy">
                                     </asp:CalendarExtender>                                  
                                </td>
                                <td>
                                      <asp:LinkButton runat="server"   Text="New" style="float:left; text-decoration:underline; " ID="lnkNew" ForeColor="#ffff00" Font-Bold="true" OnClientClick="return newClndr();" BorderColor="Yellow"></asp:LinkButton>
                                </td>                                                       
                                   <td >                                    
                                     <asp:Button runat="server" ID="btnSearch" CssClass="css_btn Ptbut" style="float:left" Text="Search" Width="40%" Height="34px" OnClick="btnSearch_Click"/>  <asp:Button runat="server" ID="btnPrint" CssClass="css_btn Ptbut" Text="Print" style="float:right" Width="40%" Height="34px" OnClick="btnPrint_Click"/>                                      
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
                     <asp:GridView HeaderStyle-BackColor="#750000" ID="gvEmpList"  runat="server" AutoGenerateColumns="false"  HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true"  HeaderStyle-Height="25px" HeaderStyle-Font-Size="14px" PageSize="25" Width="100%">
                         <PagerStyle CssClass="gridview" />
                          <Columns>                           
                           <asp:BoundField DataField="EmpCardNo" HeaderText="CardNo" ItemStyle-HorizontalAlign="Center" ItemStyle-Height="21px" />
                                 <asp:BoundField DataField="EmpName" HeaderText="Name" />                                 
                                 <asp:BoundField DataField="DptName" HeaderText="Department" ItemStyle-HorizontalAlign="Center" />                                
                                 <asp:BoundField DataField="sftName" HeaderText="Shift" ItemStyle-HorizontalAlign="Center" />                                                                                                                            
                                 <asp:BoundField DataField="sftName" HeaderText="Off Day" ItemStyle-HorizontalAlign="Center" />                                              
                          </Columns>
                     </asp:GridView>
                         
                </div>
                   </ContentTemplate>
                </asp:UpdatePanel>
        </div>
    <script type="text/javascript">
        function goToNewTabandWindow(url) {
            window.open(url);
        }
        function newClndr()
        {         
            $("#txtDate").val('');
        }
    </script>
</asp:Content>
