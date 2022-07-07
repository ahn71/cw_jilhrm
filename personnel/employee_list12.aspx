<%@ Page Title="Employee List" Language="C#" MasterPageFile="~/personnel_NestedMaster.Master" AutoEventWireup="true" CodeBehind="employee_list12.aspx.cs" Inherits="SigmaERP.personnel.employee_list" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
   
     <div class="employe_list_main_box">   
          <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlShift" />
            <asp:AsyncPostBackTrigger ControlID="ddlDepartmentList" />
            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
            <asp:AsyncPostBackTrigger ControlID="ddlChoseYear" />
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
            <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
            <asp:AsyncPostBackTrigger ControlID="btnClear" />
        </Triggers>
    <ContentTemplate>
    <asp:HiddenField ID="hdfdeleteconfirm" Value="false" runat="server" ClientIDMode="Static" />            
                 <div class="punishment_box_header">
                    <h2>Employee List</h2>
                     <div style="float:right;">
                         <table>
                              <tr>
                                 <td>
                                     Shift
                                 </td>
                                 
                                 <td>
                                     <asp:DropDownList runat="server" ID="ddlShift" CssClass="form-control text_box_width style" Width="100px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlShift_SelectedIndexChanged"   ></asp:DropDownList>
                                 </td>
                                               
                                  <td>Department</td>
                                        
                                        <td>
                                            <asp:DropDownList ID="ddlDepartmentList" CssClass="form-control select_width" runat="server" Width="190px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartmentList_SelectedIndexChanged" ></asp:DropDownList>
                                        </td>
                                 <td>Company</td>
                                        
                                        <td>
                                        <asp:DropDownList ID="ddlCompanyList" ClientIDMode="Static" style="margin-left: 10px" Width="190px" Height="30px"  CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged"   >              
                                         </asp:DropDownList>
                                        </td>                     
                                 
                                 <td>Card No</td>
                                
                                 <td>
                                     <asp:TextBox ID="txtCardNo" runat="server" CssClass="form-control text_box_width style" Width="175px" Height="20px"  ClientIDMode="Static" MaxLength="12"></asp:TextBox>
                                 </td>
                                 
                             </tr>
                         </table>
                   <table>
                       <tr>
                           <td>
                                  Year
                                 </td>
                                
                                 <td>
                                     <asp:DropDownList runat="server" ID="ddlChoseYear" CssClass="form-control text_box_width style" Width="100px" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlChoseYear_SelectedIndexChanged"   ></asp:DropDownList>
                                 </td>
                           <td>
                               From Date
                           </td>
                           
                           <td>
                               <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control text_box_width style" style=" width: 180px; margin-left:9px" Height="20px" ClientIDMode="Static" MaxLength="12"></asp:TextBox>
                               <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Format="dd-MM-yyyy" TargetControlID="txtFromDate">
                               </asp:CalendarExtender>
                           </td>
                           <td>
                               To Date
                           </td>
                       
                            <td>
                               <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control text_box_width style" style=" width: 180px; margin-left:21px" Height="20px" ClientIDMode="Static" MaxLength="12"></asp:TextBox>
                                <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server"  Format="dd-MM-yyyy" TargetControlID="txtToDate">
                                </asp:CalendarExtender>
                           </td>
                           
                           <td>
                                     <asp:Button runat="server" ID="btnSearch" CssClass="css_btn" Text="Search" Width="75px" Height="34px" OnClick="btnSearch_Click"   />
                                     
                                 </td>
                                  <td>
                                     <asp:Button runat="server" ID="btnRefresh" CssClass="css_btn" Text="Refresh" Width="75px" Height="34px" OnClick="btnRefresh_Click"    />  
                                 </td>
                           <td>
                                     <asp:Button runat="server" ID="btnClear" CssClass="css_btn" Text="Clear" Width="75px" Height="34px" OnClick="btnClear_Click"  />  
                                 </td>
                       </tr>
                   </table>
                     </div>
                </div>
         </ContentTemplate>
</asp:UpdatePanel>
     
                <%-- <div class="emp_list_box_body">    --%>
                 
                 <asp:UpdatePanel ID="UpdatePanel10"  runat="server">
                 <Triggers>
                     
                 </Triggers>
                 <ContentTemplate>
            
                 <%--<div id="divEmployeeList" runat="server" class="datatables_wrapper" style="width:auto; height:600px; overflow:auto;overflow-x:hidden;"></div>--%>
                     <div class="display" style="width:auto;height:600px;overflow-x:hidden;" >
                         <asp:GridView ClientIDMode="Static" ID="gvEmployeeList" HeaderStyle-BackColor="#27235c" Width="100%" HeaderStyle-Height="28px" runat="server" AllowPaging="True" BackColor="White" HeaderStyle-ForeColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" PageSize="100" AutoGenerateColumns="False" OnPageIndexChanging="gvEmployeeList_PageIndexChanging" OnRowCommand="gvEmployeeList_RowCommand"  DataKeyNames="EmpId" OnRowDataBound="gvEmployeeList_RowDataBound" >
                             <PagerStyle CssClass="gridview" Height="20px" />
                             <Columns>
                                 <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfempid" runat="server" Value='<%# Bind("EmpId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                                  <asp:TemplateField Visible="false">
                            <ItemTemplate>
                              <%--  <asp:HiddenField ID="hdfSN" runat="server" Value='<%# Bind("SN") %>' />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                                 <asp:BoundField DataField="EmpCardNo" HeaderText="CardNo"  />
                                 <asp:BoundField DataField="EmpName" HeaderText="Name" />
                                 <asp:BoundField DataField="EmpJoiningDate" HeaderText="Join Date" />
                                 <asp:BoundField DataField="DptName" HeaderText="Department" />
                                 <asp:BoundField DataField="DsgName" HeaderText="Designation" />
                                 <asp:BoundField DataField="SftName" HeaderText="Shift" />
                                 <asp:BoundField DataField="EmpShiftStartDate" HeaderText="Shift Start" />
                                 <asp:BoundField DataField="EmpStatusName" HeaderText="Status" />                                 
                                 <asp:ButtonField CommandName="Alter" HeaderText="Edit"  ButtonType="Button" Text="Edit"  itemstyle-horizontalalign="center" ItemStyle-Width="50px" ItemStyle-ForeColor="Black" ItemStyle-Font-Bold="true" >
                                 <ItemStyle Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" Width="50px" />
                                 </asp:ButtonField>
                                 <asp:TemplateField>
                                        <HeaderTemplate>
                                            Delete
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="btnNot"  Text="No" CommandName="Delete" Font-Bold="true" Height="30px" ForeColor="red" runat="server" OnClientClick="return confirm('Do you want to not approved ?')" Width="80px" CommandArgument='<%#((GridViewRow)Container).RowIndex%>'  />
                                            <%--<asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete" 
                                                OnClientClick="return confirm('Are you sure, you want to delete the record?')"
                                                CommandArgument='<%#((GridViewRow)Container).RowIndex%>'></asp:LinkButton>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                             </Columns>
                             
                         </asp:GridView>
                     </div>
             
                  
                     </ContentTemplate>
                     </asp:UpdatePanel>
               
                     
                    

         </div>
    <script type="text/javascript">
        function deleteRow(id) {
            var answer = confirm("Are you sure you want to Delete");
            if (answer == true) {
                jx.load('/ajax.aspx?id=' + id + '&todo=DeleteEmployee', function (data) {
                    document.location = '/personnel/employee_list.aspx';
                    // $('#divEmployeeList').html(data);
                });
                return true;
            }
            else {
                return false;
            }
            
            
        }
        function editEmployee(id) {
            goURL('/personnel/employee.aspx?EmpId=' + id + "&Edit=True");
        }
        function confirmDelete() {
            alert("confirmComplete");
            var answer = confirm("Are you sure you want to Delete");
            if (answer == true) {
                $('#hdfdeleteconfirm').val('true');
                //return true;
            }
            else {
                $('#hdfdeleteconfirm').val('false');
               // return false;
            }
        }
    </script>
</asp:Content>
