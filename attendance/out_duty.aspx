<%@ Page Title="Out Duty" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="out_duty.aspx.cs" Inherits="SigmaERP.attendance.out_duty" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
        #ContentPlaceHolder1_MainContent_gvOutDuty th, td {
            text-align: center;
        }

            #ContentPlaceHolder1_MainContent_gvOutDuty th:nth-child(3), td:nth-child(3) {
                text-align: left;
                padding-left: 3px;
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
                    <li><a href="#" class="ds_negevation_inactive Mactive">Out Duty Entry</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="main_box Mbox out-duty-box">
        <div class="main_box_header MBoxheader">
            <h2>Out Duty</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content">


                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" />
                        <asp:AsyncPostBackTrigger ControlID="gvOutDuty" />
                        <asp:AsyncPostBackTrigger ControlID="gvClient" />
                        <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="row">
                            <center>                     
                     <div id="divFindInfo" runat="server" style="font-weight:bold"></div>
                     
                       <table class="od-table">
                           <asp:Panel runat="server" ID="pnlInputUnit" ClientIDMode="Static">
                        <tr>
                                <td>Company<span class="requerd1">*</span></td>
                                <td>:</td>
                                <td colspan="3"> 
                                    <asp:DropDownList ID="ddlCompanyList" runat="server" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width">
                                    </asp:DropDownList>
                                </td>
                               
                               
                            </tr> 
              
                            <tr  >
                                <td>Date <span class="requerd1">*</span></td>
                                <td>: </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtDate" runat="server" autocomplete="off" ClientIDMode="Static" CssClass="form-control text_box_width key-off"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MM-yyyy" TargetControlID="txtDate">
                                    </asp:CalendarExtender>
                                </td>
                            </tr>                      
                                 
                        <tr runat="server" id="trEmpCardNo">
                            <td>Card No<span class="requerd1">*</span>
                            </td>
                            <td>:
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtEmpCardNo" ClientIDMode="Static"  runat="server" CssClass="form-control text_box_width cn-input" ></asp:TextBox> 
                                <asp:Button ID="btnFindEmpInfo" runat="server" Text="Find " CssClass="Mbutton" Width="60px" Height="37px" OnClientClick="return InputValidation();" OnClick="btnFindEmpInfo_Click"></asp:Button>
                             </td>
                          

                        </tr>
                                                
                            <tr>
                                <td>Type <span class="requerd1">*</span></td>
                                <td>: </td>
                               <td colspan="3">
                              <asp:RadioButtonList runat="server" ID="rblDutyType" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblDutyType_SelectedIndexChanged" >
                                  <asp:ListItem Value="0" Selected="True">Out Duty</asp:ListItem>
                                  <asp:ListItem Value="1">Training</asp:ListItem>
                              </asp:RadioButtonList>
                          </td</tr>
                                
                           </asp:Panel>
                           <asp:Panel runat="server" ID="pnlClientInfo">
                               <tr>
                                <td></td>
                                <td></td>
                               <td colspan="3">
                             <asp:CheckBox runat="server" ID="ckbStraightFromHome" Text="Straight from home" />
                                   <br />
                             <asp:CheckBox runat="server" ID="ckbStraightToHome" Text="Straight to home" />
                          </td</tr>
                                     <tr>
                                <td>Client Name</td>
                                <td>: </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtClientName" runat="server"  ClientIDMode="Static" CssClass="form-control text_box_width" Height="35px"></asp:TextBox>
                                  
                                </td>
                                 <tr>
                                <td>In Time<span class="requerd1">*</span> </td>
                                <td>:</td>
                                <td>   
                                    <asp:DropDownList ID="ddlInHur" runat="server" CssClass="attend_select_min" Width="67px">
                                      <asp:ListItem Value=" "></asp:ListItem>
                                      <asp:ListItem Value="01">01</asp:ListItem>
                                      <asp:ListItem Value="02">02</asp:ListItem>
                                      <asp:ListItem Value="03">03</asp:ListItem>
                                      <asp:ListItem Value="04">04</asp:ListItem>
                                      <asp:ListItem Value="05">05</asp:ListItem>
                                      <asp:ListItem Value="06">06</asp:ListItem>
                                      <asp:ListItem Value="07">07</asp:ListItem>
                                      <asp:ListItem Value="08">08</asp:ListItem>
                                      <asp:ListItem Value="09">09</asp:ListItem>
                                      <asp:ListItem Value="10">10</asp:ListItem>
                                      <asp:ListItem Value="11">11</asp:ListItem>
                                      <asp:ListItem Value="12">12</asp:ListItem>                              
                                    </asp:DropDownList> 
                                 </td>
                                <td> 
                                     <asp:TextBox ID="txtInMin" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00" style=" text-align: center; font-weight: bold;width:60px"></asp:TextBox>
                                </td>
                                <td> 
                                     <asp:DropDownList ID="ddlInTimeAMPM" runat="server" CssClass="attend_select_min" Width="67px" style="float:left;">
                                      <asp:ListItem Value="AM">AM</asp:ListItem>
                                      <asp:ListItem Value="PM">PM</asp:ListItem>
                                    </asp:DropDownList>                          
                                </td>                        
                                
                            </tr>
                               <tr >
                               <td>Out Time <span class="requerd1">*</span></td>
                                <td>: </td>
                                <td>
                                  <asp:DropDownList ID="ddlOutHur" runat="server" CssClass="attend_select_min" Width="67px" style="float:left;">
                                      <asp:ListItem Value=" "></asp:ListItem>
                                      <asp:ListItem Value="01">01</asp:ListItem>
                                      <asp:ListItem Value="02">02</asp:ListItem>
                                      <asp:ListItem Value="03">03</asp:ListItem>
                                      <asp:ListItem Value="04">04</asp:ListItem>
                                      <asp:ListItem Value="05">05</asp:ListItem>
                                      <asp:ListItem Value="06">06</asp:ListItem>
                                      <asp:ListItem Value="07">07</asp:ListItem>
                                      <asp:ListItem Value="08">08</asp:ListItem>
                                      <asp:ListItem Value="09">09</asp:ListItem>
                                      <asp:ListItem Value="10">10</asp:ListItem>
                                      <asp:ListItem Value="11">11</asp:ListItem>
                                      <asp:ListItem Value="12">12</asp:ListItem>                              
                                    </asp:DropDownList>
                                </td>
                                <td> 
                                    <asp:TextBox ID="txtOutMin" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00" style=" text-align: center; font-weight: bold; width:60px"></asp:TextBox>
                                    </td>
                                <td> 
                                    <asp:DropDownList ID="ddlOutTimeAMPM" runat="server" CssClass="attend_select_min" Width="67px">
                                        <asp:ListItem Value="AM" Selected="True">AM</asp:ListItem>    
                                        <asp:ListItem Value="PM" >PM</asp:ListItem>                                                                            
                                    </asp:DropDownList>
                                </td>
                            </tr>
                                        </asp:Panel>
                              
                        
                                   <tr>
                                <td>Remark</td>
                                <td>: </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" ClientIDMode="Static" CssClass="form-control text_box_width" Height="35px"></asp:TextBox>
                                  
                                </td>
                            </tr>
                                    <tr>
                                <td></td>
                                <td></td>
                                <td colspan="3">
                                     <asp:Button ID="btnAdd" CssClass="Mbutton" runat="server" Text="Add" OnClick="btnAdd_Click" />
                                  
                                </td>
                            </tr>
                        
                    </table>

                      
                        </center>
                            <br />
                            <div class="table-responsive">

                                <asp:GridView ID="gvClient" ClientIDMode="Static" runat="server" HeaderStyle-BackColor="#2B5E4E" HeaderStyle-Height="28px" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="14px" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" Height="13px" OnRowCommand="gvClient_RowCommand" OnRowDataBound="gvClient_RowDataBound">
                                    <PagerStyle CssClass="gridview" Height="20px" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>

                                        <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex+1%>
                                            </ItemTemplate>

                                            <ItemStyle HorizontalAlign="Center" ForeColor="green" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Client Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClientName" runat="server" Text='<%# Eval("ClientName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="In">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInTime" runat="server" Text='<%# Eval("InTime") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Out">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOutTime" runat="server" Text='<%# Eval("OutTime") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Purpose">
                                            <ItemTemplate>
                                                <asp:Label Font-Bold="true" ID="lblPurpose" runat="server" Text='<%# Eval("Purpose") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Edit
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="btnEditClient" runat="server" Text="Edit" Font-Bold="true" CommandName="editClientRow" ForeColor="Green"
                                                    CommandArgument="<%# Container.DataItemIndex%>" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Remove
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="btnRemoveClient" runat="server" Text="Remove" Font-Bold="true" CommandName="removeClientRow" ForeColor="Red"
                                                    OnClientClick="return confirm('Are you sure, you want to remove the record?')"
                                                    CommandArgument="<%# Container.DataItemIndex%>" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                    </Columns>

                                </asp:GridView>


                            </div>
                        </div>
                        <div class="button_area" style="margin-top: 10px;">

                            <center>

                    <table class="button_table">
                        <tr>
                          
                            <th>
                                <asp:Button ID="btnSave" CssClass="Mbutton" runat="server" Text="Save" OnClientClick="return InputValidationBasket();" OnClick="btnSave_Click" />
                            </th>
                            <th>
                                <asp:Button ID="btnClear" CssClass="Mbutton" runat="server" Text="Clear" />
                           <th> 
                               <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/attendance_default.aspx" CssClass="Mbutton" />
                           </th>
                           
                        
                        </tr>
                    </table>
                        
                    </center>

                            <%--</div>--%>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <br/>
                <div class="dataTables_wrapper">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <asp:GridView ID="gvOutDuty" runat="server" HeaderStyle-BackColor="#2B5E4E" HeaderStyle-Height="28px" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="14px" AllowPaging="true" PageSize="40" Width="100%" AutoGenerateColumns="False" DataKeyNames="SL,StraightFromHome,StraightToHome" CellPadding="4" ForeColor="#333333" Height="13px" OnRowCommand="gvOutDuty_RowCommand" OnRowDataBound="gvOutDuty_RowDataBound">
                                    <PagerStyle CssClass="gridview" Height="20px" />
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
                                        <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label  ID="lblOuttime" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Edit
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="btnEdit"  CssClass="btn-xs btn-info text-white" style="color:#ffffff" runat="server" Text="Edit" CommandName="editRow"
                                                    CommandArgument="<%# Container.DataItemIndex%>" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

            </div>


        </div>
    </div>


</asp:Content>
