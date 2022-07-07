<%@ Page Title="Place Assign Panel" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="FloorAssigne.aspx.cs" Inherits="SigmaERP.personnel.FloorAssigne" %>
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

        $(document).ready(function () {
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

    </script>
    <style>
        .table tr td:nth-child(2){
            padding-left:10px;
        }
        .stick{
    position:fixed;top:0px;left:126px;
}
        #ContentPlaceHolder1_MainContent_gvEmpList tbody tr td{
            padding-left:5px;        
        }
        .tdwidth {
            width:270px;
        }
        #ContentPlaceHolder1_MainContent_gvEmpList th, td {
            text-align:center;
        }
         #ContentPlaceHolder1_MainContent_gvEmpList th:nth-child(2), td:nth-child(2) {
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
                    <li><a href="#" class="ds_negevation_inactive Ptactive">Place Assign Panel</a></li>
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
                <h2>Place Assign Panel</h2>
            </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>                     
                            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />                                                                                              
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                            <asp:AsyncPostBackTrigger ControlID="ddlDepartmentList" />    
                            <asp:AsyncPostBackTrigger ControlID="chkLoadAllShiftList" />                     
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
                        <table style="width:98%;margin:0px auto;">
                              <tr>
                                    <td>
                                       Company
                                      </td>
                                       <td class="tdwidth">
                                     <asp:DropDownList runat="server" ID="ddlCompanyList" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ></asp:DropDownList>
                                 </td>                                  
                                  <td>
                                      Deparment 
                                      </td>
                                       <td class="tdwidth">
                                     <asp:DropDownList runat="server" ID="ddlDepartmentList" CssClass="form-control text_box_width style" Width="96%" Height="30px" AutoPostBack="True" ForeColor="Red" OnSelectedIndexChanged="ddlDepartmentList_SelectedIndexChanged" ></asp:DropDownList>
                                 </td> 
                                  <td>Date</td>
                                <td style="width:190px">
                                     <asp:TextBox ID="txtDate" Width="90%" ClientIDMode="Static"  runat="server" CssClass="form-control text_box_width" MaxLength="10" style="text-align:center" ></asp:TextBox>
                                     <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDate" Format="dd-MM-yyyy">
                                     </asp:CalendarExtender>
                                </td>      
                                                                                                                
                                       <td>  <asp:Button runat="server" ID="btnSearch" CssClass="css_btn Ptbut" Text="Search" Width="72px" Height="34px" OnClick="btnSearch_Click"/></td>                                                       
                                   <td>                                    
                                     <asp:Button runat="server" ID="btnPrint" CssClass="css_btn Ptbut" Text="Print" Width="65px" Height="34px" OnClick="btnPrint_Click"  />                                      
                                </td>   
                                       <td>
                                      <asp:CheckBox ID="chkLoadAllShiftList" runat="server" AutoPostBack="true" Text="All Assigned Shift" OnCheckedChanged="chkLoadAllShiftList_CheckedChanged"  />
                                  </td>  
                                  <td >
                                      <asp:Label ID="lblTotalEmployee" runat="server" Text="" ForeColor="Red" style="float:right;font-weight:bold;"></asp:Label>
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
                     <div  style="width:100%; background-color:#fff;margin: auto;">
                         <div id="divRecordMessage" runat="server" visible="false" style="color: red; font-weight: bold; text-align: center; padding-top: 75px; font-size: 32px; height: 118px">
                           
                         </div>
                     <asp:GridView HeaderStyle-BackColor="#750000" ID="gvEmpList"  runat="server" AutoGenerateColumns="false"  HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true"  HeaderStyle-Height="25px" HeaderStyle-Font-Size="14px" PageSize="25" Width="100%" DataKeyNames="SL" OnRowDataBound="gvEmpList_RowDataBound" >
                         <PagerStyle CssClass="gridview" />
                          <Columns>             
                                  <asp:TemplateField HeaderText="CardNo"  ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                  <ItemTemplate >
                                 <asp:Label ID="lblEmpCard" runat="server" Text='<%#Eval("EmpCardNo") %>'    AppendDataBoundItems="true" AutoPostBack="true" Width="140px" />
                                  </ItemTemplate>
                              </asp:TemplateField>    
                                      
                          
                                 <asp:BoundField DataField="EmpName" HeaderText="Name" />                                 
                                 <asp:BoundField DataField="DptName" HeaderText="Department" ItemStyle-HorizontalAlign="Center" />                                
                                 <asp:BoundField DataField="sftName" HeaderText="Shift" ItemStyle-HorizontalAlign="Center" />  
                              
                              <asp:TemplateField HeaderText="Comments"  ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                  <ItemTemplate >
                                   <asp:TextBox ID="txtRemaks" runat="server" onkeyup ="OnKeyPress(this)"  Text='<%#Eval("Notes") %>'   AppendDataBoundItems="true" CssClass="form-control text_box_width style" AutoPostBack="true" Width="140px" ></asp:TextBox> 
                                  </ItemTemplate>
                              </asp:TemplateField>            
                                                                                                                                               
                              <asp:TemplateField HeaderText="Floor"  ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                  <ItemTemplate >
                                   <asp:DropDownList ID="ddlFloorList" runat="server"  AppendDataBoundItems="true" CssClass="form-control text_box_width style" AutoPostBack="true" OnSelectedIndexChanged="ddlFloorList_SelectedIndexChanged" ForeColor="Red" Width="100px" ></asp:DropDownList> 
                                  </ItemTemplate>
                              </asp:TemplateField>                                                
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

        function OnKeyPress(e)
        {
           
            var id = e.id;
            var val = $('#'+id).val();
           
            var index=id.split('_');
            
            var lblVal = $('#ContentPlaceHolder1_MainContent_gvEmpList_lblEmpCard_'+index[4]).text();
            var ddlVal = $('#ContentPlaceHolder1_MainContent_ddlDepartmentList').val();
            var txtDate = $('#txtDate').val();
            jx.load('/ajax.aspx?id=' + id + '&todo=getDutyComments' + '&ECardNo= ' + lblVal + '&STID=' + ddlVal + '&SDate=' + txtDate + '&Values=' + val+ ' ', function (data) {
                
            });

        }
    </script>
</asp:Content>
