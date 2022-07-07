<%@ Page Title="" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="roster_remove_panel.aspx.cs" Inherits="SigmaERP.attendance.roster_remove_panel" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
        function allclear()
        {
           
            $("#txtFromDate").val('');
        }

        function CloseWindowt() {
            window.close();
        }

        function goToNewTabandWindow(url) {
            window.open(url);

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnDeleteFullRoster" />
            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
            <asp:AsyncPostBackTrigger ControlID="ddlShiftList" />
        </Triggers>
    <ContentTemplate>
        <div class="list_main_box" style="margin-bottom:200px;">
               <div class="list_main_content_box_header list_main_content_box_header_width" id="divElementContainer"  runat="server"> 
                   <h2>Roster Remove Panel</h2><h2 style="float: right; margin-top: -42px;"><a style="color:red" href="/attendance/attendance.aspx">Close</a></h2>
                   <div style="padding:10px; border-top:1px solid #ccc;overflow:hidden;">
                   <div class="form-inline">                      
                      
                      <div class="form-group rost_form_group">
                        <label>Department</label>
                        <asp:DropDownList runat="server" ID="ddlDepartmentName" CssClass="form-control select_width rost_select_box" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartmentName_SelectedIndexChanged"></asp:DropDownList>
                      </div>
                      <div class="form-group rost_form_group">
                        <label>Sub Depart.</label><asp:DropDownList ID="ddlSubDepartmentName" CssClass="form-control select_width rost_select_box2" runat="server" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlSubDepartmentName_SelectedIndexChanged" ></asp:DropDownList>
                      <asp:Label ID="lblRowCount" runat="server" ForeColor="Yellow" Font-Bold="true"></asp:Label>
                      </div>
                      <div class="form-group rost_form_group">
                        <label>Roster  List</label><asp:DropDownList ID="ddlShiftList" CssClass="form-control select_width rost_select_box3" runat="server" Height="30px" AutoPostBack="True" OnSelectedIndexChanged="ddlShiftList_SelectedIndexChanged" ></asp:DropDownList>
                            
                      </div>
                      <div class="form-group rost_form_group">
                           <label>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp</label>
                           
                      </div>
                      <div class="form-grou rost_form_groupp">
                           
                             <asp:Button runat="server" ID="btnDeleteFullRoster" CssClass="css_btn" Text="Delete Full Roster" Width="122px" Height="34px" OnClientClick="return confirm('Do you want to delete full roster ?')" OnClick="btnDeleteFullRoster_Click" /> 
                             <asp:TextBox Height="28px" MaxLength="10" AutoComplete="Off" PlaceHolder="Select date for test " Style=" border:1px solid; text-align:center;" ID="txtRosterDate" runat="server" Enabled="true" ForeColor="Blue"   ></asp:TextBox> 
                                   <asp:CalendarExtender ID="txtRosterDate_CalendarExtender" runat="server" Format="dd-MM-yyyy" TargetControlID="txtRosterDate">
                                   </asp:CalendarExtender>
                             <asp:Button runat="server" ID="btnSearch" CssClass="css_btn" Text="Search" Width="70px" Height="34px" OnClick="btnSearch_Click"  /> 
                          <asp:LinkButton runat="server" ForeColor="White" Font-Bold="true" ID="lnkRefresh" Text="Refresh" OnClick="lnkRefresh_Click"></asp:LinkButton>
                      </div>

                       <div class="form-group rost_form_group att_shift">
                        <label>Company</label>
                        <asp:DropDownList  ID="ddlCompanyList" Enabled="false" ClientIDMode="Static" Width="188px" Height="30px" CssClass="form-control select_width" runat="server" AutoPostBack="True"  >              
                        </asp:DropDownList>
                      </div>
                    </div>
                 </div>
                   
                  
                   </div>
              
         
                  <div class="dataTables_wrapper list_main_content_box_body_width">

                 
                     <asp:GridView HeaderStyle-BackColor="#27235C"   HeaderStyle-Height="28px" ID="gvEmpList" CssClass="gvAttListCss" runat="server" AutoGenerateColumns="false"  HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="14px" AllowPaging="false" PageSize="40" Width="100%" DataKeyNames="EmpId" OnRowDataBound="gvEmpList_RowDataBound" OnRowCommand="gvEmpList_RowCommand" >
                         <PagerStyle CssClass="gridview" />
                          <Columns>
                             
                                <asp:TemplateField HeaderText="SL">
                                <ItemTemplate>
                                     
                                    <%#Container.DataItemIndex+1 %>                              
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Font-Bold="true" ForeColor="Red" />
                            </asp:TemplateField>       
                           
                              <asp:BoundField DataField="EmpName" HeaderText="EmpName" Visible="true" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"  />
                              <asp:BoundField ItemStyle-Height="28px" DataField="DsgName" HeaderText="Designation" Visible="true" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                
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
                                      <asp:Button  Height="30px" ForeColor="DarkRed" ID="btnDeleteBySingleDate" runat="server" CommandName="RemoveBySingleDate" Text="Delete single date roster of this employee "  OnClientClick=" return confirm('Do you want to delete full roster of this employee ?')"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                 
                                   </ItemTemplate>
                                      <ItemStyle HorizontalAlign="Center" />
                              </asp:TemplateField> 
                                             
                          </Columns>                         
                     </asp:GridView>
                       <div id="divRecordMessage" runat="server" visible="false" style="color: red; background-color:white; font-weight: bold; text-align: center; padding-top: 75px; font-size: 32px; height: 118px">
                         </div>
                </div>            
     
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
