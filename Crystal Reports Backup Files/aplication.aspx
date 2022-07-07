<%@ Page Title="Leave Application" Language="C#" MasterPageFile="~/leave_nested.Master" AutoEventWireup="true" CodeBehind="aplication.aspx.cs" Inherits="SigmaERP.personnel.aplication" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 81px;

        }
        .tdWidth {
            width:80%;
        }
        #ContentPlaceHolder1_MainContent_TabContainer1_tab1_gvLeaveApplication td, th {
            text-align:center;
        }
          #ContentPlaceHolder1_MainContent_TabContainer1_tab1_gvLeaveApplication td:nth-child(7),td:nth-child(8),td:nth-child(9),td:nth-child(10) {
            width:50px;
        }
          .form-control.text_box_width {
              float: left;
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
                       <li> <a href="#" class="ds_negevation_inactive Lactive">Leave Application</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <link href="../style/Design.css" rel="stylesheet" />
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="hdLeaveApplicationId" />
            <%--<asp:AsyncPostBackTrigger ControlID="btnSaveLeave" />--%>            
            <asp:AsyncPostBackTrigger ControlID="btnDateCalculation" />
            <asp:AsyncPostBackTrigger ControlID="ddlLeaveName" />
            <asp:AsyncPostBackTrigger ControlID="ddlDepartment" />
            <asp:AsyncPostBackTrigger ControlID="ddlBranch" />
            <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
            <asp:PostBackTrigger ControlID="btnSaveLeave"  />
          
        </Triggers>
        <ContentTemplate>
            <div class="main_box Lbox">
        <div class="main_box_header_leave LBoxheader">
                    <h2>Leave Application</h2>
                </div>

                <div class="main_box_body_leave Lbody">
            <div class="main_box_content_leave" >

                        <!--ST-->
                        <div class="application_box_left" style="width:61%">
                            <fieldset>
                                <legend>
                                    <b>Leave Transaction</b>
                                    <asp:HiddenField ID="hdLeaveApplicationId" ClientIDMode="Static" runat="server" Value="" />
                                </legend>
                                <table class="employee_table">
                                    <tr id="trCompanyName" runat="server" >
                                        <td>Company Name</td>
                                        <td>:</td>
                                        <td class="tdWidth">
                                        <asp:DropDownList ID="ddlBranch" ClientIDMode="Static"   CssClass="form-control select_width" Width="96%" runat="server" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" AutoPostBack="True"  >              
                                         </asp:DropDownList>
                                        </td>
                                    </tr>
                                 <%--   <tr>
                                        <td>Shift</td>
                                        <td>:</td>
                                        <td class="tdWidth">
                                            <asp:DropDownList ID="ddlShiftName" CssClass="form-control select_width" Width="96%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlShiftName_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:Label ID="lblSftTime" runat="server" ForeColor="Blue" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr> --%>                                    
                                            <tr runat="server" id="trDepartment">
                                        <td>Department</td>
                                        <td>:</td>
                                        <td class="tdWidth">
                                            <asp:DropDownList ID="ddlDepartment" CssClass="form-control select_width" Width="96%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" ></asp:DropDownList>
                                            <asp:Label ID="lblSftTime" runat="server" ForeColor="Blue" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>  
                                    <tr runat="server" id="trEmployee">
                                        <td>Emp Card No 
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:DropDownList ID="ddlEmpCardNo" CssClass="form-control select_width" runat="server"  Width="96%" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlEmpCardNo_SelectedIndexChanged" ></asp:DropDownList>
                                            <asp:TextBox ID="txtEmpCardNo" runat="server" Visible="false"  ClientIDMode="Static" CssClass="form-control text_box_width" ></asp:TextBox><asp:Button Visible="false" ID="btnFind" runat="server" Text="Find" Width="76px" OnClick="btnFind_Click" />
                                           <%-- <asp:RequiredFieldValidator ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtEmpCardNo" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Emp Name
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:TextBox ID="txtEmpName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Width="96%" Enabled="False"></asp:TextBox>
                                            <asp:Label ID="lblDepartment" runat="server" ForeColor="Blue" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>Apply Date
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:TextBox ID="txtApplyDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Width="96%"></asp:TextBox>
                                            
                                            <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                PopupButtonID="imgEffectDateTo" Enabled="True"
                                                TargetControlID="txtApplyDate" ID="CalendarExtender4">
                                            </asp:CalendarExtender>         

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>From Date
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">

                                            <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Width="96%"></asp:TextBox>
                                           
                                            <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                PopupButtonID="imgEffectDateFrom" Enabled="True"
                                                TargetControlID="txtFromDate" ID="CExtApplicationDate">
                                            </asp:CalendarExtender>
                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>To Date
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Width="96%"></asp:TextBox>
                                            
                                            <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                PopupButtonID="imgEffectDateTo" Enabled="True"
                                                TargetControlID="txtToDate" ID="CalendarExtender1">
                                            </asp:CalendarExtender>
                                            <asp:Button ID="btnDateCalculation" runat="server" Text="Calculation"  class="Lbutton" OnClick="btnDateCalculation_Click" />

                                            

                                        </td>
                                    </tr>                      

                                    <tr>
                                        <td>No. Of Days
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:TextBox ID="txtNoOfDays" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Enabled="False" Width="96%"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtNoOfDays"
                                                ErrorMessage="Please Enter Only Numbers" ValidationExpression="^\d+$" ValidationGroup="save"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>No. Of Weekend
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                             <asp:TextBox ID="txtTotalHolydays" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Enabled="False" Width="96%"></asp:TextBox>
                                           
                                        </td>
                                    </tr>
                                    <tr >
                                        <td>Leave Name
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:DropDownList ID="ddlLeaveName" ClientIDMode="Static" CssClass="form-control select_width" runat="server"  Width="96%" AutoPostBack="True" OnSelectedIndexChanged="ddlLeaveName_SelectedIndexChanged" ></asp:DropDownList>
                                           
                                        </td>                                                  
                                    </tr>

                                        <tr runat="server" id="trReplacedDate">
                                        <td>Replaced Date(s)
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:TextBox ID="txtReplacedDate" placeholder="dd-MM-yyyy, dd-MM-yyyy, dd-MM-yyyy,..."  runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Width="96%"></asp:TextBox>
                                            
                                            <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                PopupButtonID="imgEffectDateTo" Enabled="True"
                                                TargetControlID="txtReplacedDate" ID="CalendarExtender5">
                                            </asp:CalendarExtender>         

                                        </td>
                                    </tr>
                                     <tr runat="server" id="tr1">
                                        <td>Charge Handed Over To 
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:DropDownList ID="ddlHandOverPerson" CssClass="form-control select_width" runat="server"  Width="96%" ClientIDMode="Static" ></asp:DropDownList>
                                           </td>
                                    </tr>
                                    

                                        
                                      <tr >
                                        <td>Attach Document (if any)
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:TextBox ID="txtFileName" ClientIDMode="Static" runat="server"  Style="width: 100px;float: left;margin-left: 3px; height:22px"  Enabled="False" Width="50px"></asp:TextBox>                                         
                                          <asp:FileUpload ID="FileUploadDoc" runat="server" Width="211px" /> 
                                        </td>
                                                  
                                    </tr>
                               
                                    <tr id="trpregnatn" runat="server"> 
                                        <td>Date of pregnant</td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">

                                            <asp:TextBox ID="txtPregnantDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Width="96%"></asp:TextBox>
                                           
                                            <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                PopupButtonID="imgEffectDateFrom" Enabled="True"
                                                TargetControlID="txtPregnantDate" ID="CalendarExtender2">
                                            </asp:CalendarExtender>

                                            <asp:RequiredFieldValidator ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPregnantDate" ErrorMessage="*"></asp:RequiredFieldValidator>
                                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="txtPregnantDate" ValidationGroup="save"
                                                ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$"
                                                runat="server" ErrorMessage="Invalid Farmat.">
                                            </asp:RegularExpressionValidator>--%>

                                        </td>
                                    </tr>
                                    <tr id="trprasabera" runat="server" visible="false" >
                                        <td>Date of Child prasabera
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:TextBox ID="txtPrasabaDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Width="96%"></asp:TextBox>
                                            
                                            <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                PopupButtonID="txtPrasabaDate" Enabled="True"
                                                TargetControlID="txtPrasabaDate" ID="CalendarExtender3">
                                            </asp:CalendarExtender>
                                           

                                            

                                        </td>
                                    </tr>

                                    <tr id="trStatusBar" runat="server" visible="false">
                                        <td>
                                            Status
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:CheckBox ID="chkApproved" runat="server" Text="Approved" Checked="True" Enabled="False"/> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="chkProcessed" runat="server" Text="Processed" Checked="True" Enabled="False"/>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td >
                                            Purpose of Leave
                                        </td>
                                        <td>:</td>
                                        <td class="tdWidth">
                                           <asp:TextBox ID="txtNotes" runat="server" Height="40px" ClientIDMode="Static" CssClass="form-control text_box_width" Width="96%" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td >
                                            Leave Address
                                        </td>
                                        <td>:</td>
                                        <td class="tdWidth">
                                           <asp:TextBox ID="txtLvAddress" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"  Width="50%"></asp:TextBox>
                                   <label style="float: left; margin-top: 14px;">Contact</label>
                                             <asp:TextBox ID="txtLvContact" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"  Width="32%"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <!--RT   -->
                        

                        <div style="border-radius: 5px; display:none; border: 2px solid #086A99;border-top:0px; font-weight:bold; width: 380px;background:#ddd;padding:5px;" id="PopupWindow" >
                            <div id="divDrag" class="boxFotter">
                                 <a ID="btnCancel" href="#"><img style="left: 373px;position: absolute;top: -1px;width: 5% !important;" src="../images/icon/cancel.png" alt="" /></a>
                           <cnter> 
                                
                                <h2>Leave Status</h2>
                                
                           </cnter>
                                
                             </div>
                            <asp:Panel ID="Panel1" runat="server" BackColor="WhiteSmoke">

                            <fieldset>
                                <legend>
                                    <b>Leave Count</b>
                                </legend>
                                <table class="employee_table">
                                    <tr>
                                        <td>Total Leave
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTotalLeave" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Font-Bold="True" Font-Size="12pt"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Used
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                            <asp:TextBox ID="txtUsed" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Font-Bold="True" Font-Size="12pt" ForeColor="Red"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Unused
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUnused" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Font-Bold="True" Font-Size="12pt" ForeColor="Green"></asp:TextBox>
                                        </td>
                                    </tr>

                                     

                                </table>
                                </asp:Panel>
                            </fieldset>

                            <asp:Panel ID="Panel2" runat="server" BackColor="WhiteSmoke">
                            <fieldset>
                                <legend>
                                    <b>Partial Information</b>
                                </legend>
                                <table class="employee_table">
                                    <tr>
                                        <td class="auto-style1">Type
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmployeeType" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Font-Bold="True" Font-Size="10pt"></asp:TextBox>

                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1">Card No
                                        </td>
                                        <td>:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCardNo" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Font-Bold="True" Font-Size="10pt"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style1">Name
                                        </td>
                                        <td>:
                                        </td>
                                        <td>

                                           <asp:TextBox ID="txtName" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Font-Bold="True" Font-Size="10pt"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>                                
                            </fieldset>
                            </asp:Panel>
                           
                        </div>


                        <div class="border" style="width:35%">
                        </div>


                        <div class="list_button" >
                            <table >
                                <tbody>

                                    <tr>
                                        <td style="width: 17px;">
                                            <asp:Button ID="btnSelectAll" PostBackUrl="~/leave/all_leave_list.aspx" runat="server" Text="Select All" CssClass="Lbutton" /></td>
                                       
                                        <td style="width: 17px;">
                                            <asp:Button ID="btnSaveLeave" runat="server" Text="Save" CssClass="Lbutton" OnClick="btnSaveLeave_Click" />
                                        </td>
                                        <td style="width: 17px;">
                                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="Lbutton" OnClick="btnClear_Click" />
                                        </td>
                                        <td style="width: 17px;">
                                            <asp:Button ID="btnClose" runat="server" Text="Close" PostBackUrl="~/leave_default.aspx" CssClass="Lbutton"/>
                                        </td>
                                          
                                       <%-- <td style="width: 17px;">
                                            <asp:Button ID="btnComplain" Visible="false" runat="server" Text="Complain" CssClass="css_btn" OnClick="btnComplain_Click"/>
                                        </td>--%>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <asp:TabContainer ID="TabContainer1" runat="server" CssClass="fancy fancy-green" AutoPostBack="true" OnActiveTabChanged="TabContainer1_ActiveTabChanged" Font-Bold="true" ActiveTabIndex="0"  >

                            <asp:TabPanel ID="tab1" TabIndex="0" runat="server" Font-Bold="false">
                                <HeaderTemplate>Leave Going List</HeaderTemplate>
                                <ContentTemplate>
                                     <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" Drag="True" 
                            DropShadow="True" PopupControlID="PopupWindow" TargetControlID="btnClick" CancelControlID="btnCancel" PopupDragHandleControlID="divDrag" CacheDynamicResults="False" Enabled="True" >
                        </asp:ModalPopupExtender>                                
                        <div class="dataTables_wrapper" style="margin-top: -134px;">
                             <asp:Button ID="btnClick" runat="server" Text="Popup click" Width="75px" Style="display:none"/>        
                            <asp:GridView ID="gvLeaveApplication" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" CssClass="display" DataKeyNames="LACode" ForeColor="#333333" Height="8px" OnDataBinding="gvLeaveApplication_DataBinding" OnPageIndexChanging="gvLeaveApplication_PageIndexChanging" OnRowCommand="gvLeaveApplication_RowCommand" OnRowDataBound="gvLeaveApplication_RowDataBound" OnRowDeleting="gvLeaveApplication_RowDeleting" OnRowEditing="gvLeaveApplication_RowEditing" Width="100%">
                                         <AlternatingRowStyle BackColor="White" />
                                         <Columns>
                                             <asp:BoundField DataField="LACode" HeaderText="LACode" Visible="False">
                                             <HeaderStyle HorizontalAlign="Center" />
                                             <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="EmpCardNo" HeaderText="Card No">
                                             <HeaderStyle HorizontalAlign="Center" />
                                             <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="FromDate" HeaderText="From">
                                             <HeaderStyle HorizontalAlign="Center" />
                                             <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="ToDate" HeaderText="To">
                                             <HeaderStyle HorizontalAlign="Center" />
                                             <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="WeekHolydayNo" HeaderText="W.H.">
                                             <HeaderStyle HorizontalAlign="Center" />
                                             <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="TotalDays" HeaderText="T.Days">
                                             <HeaderStyle HorizontalAlign="Center" />
                                             <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                             </asp:BoundField>
                                             <asp:BoundField DataField="ShortName" HeaderText="Leave ">
                                             <HeaderStyle HorizontalAlign="Center" />
                                             <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                             </asp:BoundField>
                                             <asp:ButtonField ButtonType="Button" CommandName="Status" HeaderText="Status" Text=" Status ">
                                             <ControlStyle CssClass="btnForStatusInGV" />
                                             <HeaderStyle HorizontalAlign="Center" />
                                             <ItemStyle Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" Width="50px" />
                                             </asp:ButtonField>
                                             <asp:TemplateField>
                                                 <HeaderTemplate>
                                                     Edit
                                                 </HeaderTemplate>
                                                 <ItemTemplate>
                                                     <asp:Button ID="btnAlter" runat="server" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" CommandName="Alter" CssClass="btnForAlterInGV" Text="Edit" Width="60px" />
                                                 </ItemTemplate>
                                                 <HeaderStyle HorizontalAlign="Center" />
                                                 <ItemStyle HorizontalAlign="Center" />
                                             </asp:TemplateField>
                                             <asp:TemplateField>
                                                 <HeaderTemplate>
                                                     Delete
                                                 </HeaderTemplate>
                                                 <ItemTemplate>
                                                     <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# Eval("LACode") %>' CommandName="Delete" ForeColor="Red" OnClientClick="return confirm('Are you sure, you want to delete the record?')" Text="Delete"></asp:LinkButton>
                                                 </ItemTemplate>
                                                 <HeaderStyle HorizontalAlign="Center" />
                                             </asp:TemplateField>
                                             <asp:TemplateField>
                                                 <HeaderTemplate>
                                                     View
                                                 </HeaderTemplate>
                                                 <ItemTemplate>
                                                     <asp:Button ID="btnView" runat="server" ClientIDMode="Static" CommandArgument='<%#Eval("LACode")%>' CommandName="View" Text="View" />
                                                 </ItemTemplate>
                                                 <ControlStyle CssClass="btnForStatusInGV" />
                                                 <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                                 <ItemStyle HorizontalAlign="Center" />
                                             </asp:TemplateField>
                                         </Columns>
                                         <EditRowStyle BackColor="#7C6F57" />
                                         <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                         <HeaderStyle BackColor="#5EC1FF" ForeColor="White" Height="28px" />
                                         <PagerStyle BackColor="#666666" CssClass="gridview" HorizontalAlign="Center" />
                                         <RowStyle BackColor="#E3EAEB" />
                                         <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="White" />
                                         <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                         <SortedAscendingHeaderStyle BackColor="#246B61" />
                                         <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                         <SortedDescendingHeaderStyle BackColor="#15524A" />
                                     </asp:GridView>
                        </div>

                                     

</ContentTemplate>
                            </asp:TabPanel>

                            <asp:TabPanel ID="tab2" runat="server" TabIndex="1" Font-Bold="false">
                                <HeaderTemplate>Leave Pending List</HeaderTemplate>
                                <ContentTemplate>
                                    <asp:GridView ID="gvRejectedList" runat="server" AutoGenerateColumns="False"
                                    CellPadding="4" Height="8px" AllowPaging="True" ForeColor="#333333" Width="100%" CssClass="display" DataKeyNames="LACode" OnRowCommand="gvRejectedList_RowCommand" OnRowDataBound="gvRejectedList_RowDataBound" >
                                <AlternatingRowStyle BackColor="White" />
                                <PagerStyle CssClass="gridview" BackColor="#666666" HorizontalAlign="Center" />
                                <Columns>
                                    
                                    <asp:BoundField DataField="LACode" HeaderText="LACode" Visible="False" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                   <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                   <asp:BoundField DataField="FromDate" HeaderText="From" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                   <asp:BoundField DataField="ToDate" HeaderText="To" >
                                    
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                    
                                    <asp:BoundField DataField="WeekHolydayNo" HeaderText="W.H." >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                   <asp:BoundField DataField="TotalDays" HeaderText="T.Days" >
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShortName" HeaderText="Leave ">                                                                                                
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle Height="12px" HorizontalAlign="Center" Width="100px" />
                                    </asp:BoundField>
                                     <asp:TemplateField>
                                        <HeaderTemplate >
                                            View
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Button runat="server" CommandName="View" ClientIDMode="Static" ID="btnView" Text="View" CommandArgument='<%#Eval("LACode")%>' />
                                        </ItemTemplate>
                                         <ControlStyle CssClass="btnForStatusInGV" />
                                         <HeaderStyle HorizontalAlign="Center" Width="8%" />
                                         <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    
                                </Columns>
                                <EditRowStyle BackColor="#7C6F57" />
                                <FooterStyle BackColor="#1C5E55" ForeColor="White" Font-Bold="True" />
                               <HeaderStyle BackColor="#5EC1FF" ForeColor="White" Height="28px" />
                                <RowStyle BackColor="#E3EAEB" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                <SortedDescendingHeaderStyle BackColor="#15524A" />
                            </asp:GridView>
                                </ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>

                    </div>
                </div>
       
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


 
    

       

            

     <script type="text/javascript">
       
         $(document).ready(function () {
             loadcardNo();

         });
         function loadcardNo() {
             $("#ddlEmpCardNo").select2();
             $("#ddlHandOverPerson").select2();
         }
        function goToNewTabandWindow(url) {
            window.open(url);
            loadcardNo();
        }
        function WarningMsg(msg)
        {
            showMessage(msg,'warning');
        }
        function SuccessMsg(msg) {
            showMessage(msg, 'success');
        }
        function ErrorMsg(msg) {
            showMessage(msg, 'error');
        }
    </script>

</asp:Content>
