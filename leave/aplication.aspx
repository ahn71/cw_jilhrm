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
            <asp:PostBackTrigger ControlID="ckbIsHalfDayLeave"  />
          
        </Triggers>
        <ContentTemplate>
            <div class="main_box Lbox">
        <div class="main_box_header_leave LBoxheader">
                    <h2>Leave Application</h2>
                </div>

                <div class="main_box_body_leave Lbody">
            <div class="main_box_content_leave" >

                        <!--ST-->
                        <div class="application_box_left">
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
                                        <asp:DropDownList ID="ddlBranch" ClientIDMode="Static"   CssClass="form-control select_width"  runat="server" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" AutoPostBack="True"  >              
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
                                            <asp:DropDownList ID="ddlDepartment" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" ></asp:DropDownList>
                                            <asp:Label ID="lblSftTime" runat="server" ForeColor="Blue" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>  
                                    <tr runat="server" id="trEmployee">
                                        <td>Emp Card No 
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:DropDownList ID="ddlEmpCardNo" CssClass="form-control select_width" runat="server"   ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlEmpCardNo_SelectedIndexChanged" ></asp:DropDownList>
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
                                            <asp:TextBox ID="txtEmpName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False"></asp:TextBox>
                                            <asp:Label ID="lblDepartment" runat="server" ForeColor="Blue" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>Leave Name
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:DropDownList ID="ddlLeaveName" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLeaveName_SelectedIndexChanged" style="margin: 0;" ></asp:DropDownList>
                                           
                                        </td>                                                  
                                    </tr>
                                      <tr runat="server" id="trHalfDayLeave">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td class="tdWidth hll-select">
                                            <asp:CheckBox runat="server" ID="ckbIsHalfDayLeave" ClientIDMode="Static" Text="Half day leave?" AutoPostBack="true" OnCheckedChanged="ckbIsHalfDayLeave_CheckedChanged"/>
                                           
                                        </td>                                                  
                                    </tr>
                                     <tr>
                                        <td>Apply Date
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:TextBox ID="txtApplyDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width key-off" autocomplete="off"></asp:TextBox>
                                            
                                            <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                PopupButtonID="imgEffectDateTo" Enabled="True"
                                                TargetControlID="txtApplyDate" ID="CalendarExtender4">
                                            </asp:CalendarExtender>         

                                        </td>
                                    </tr>
                                    <tr>
                                        <td runat="server" id="tdFromDate" >From Date
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:TextBox ID="txtFromDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width key-off" autocomplete="off"></asp:TextBox>                                           
                                            <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                PopupButtonID="imgEffectDateFrom" Enabled="True"
                                                TargetControlID="txtFromDate" ID="CExtApplicationDate">
                                            </asp:CalendarExtender>                                        
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trToDate">
                                        <td>To Date
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:TextBox ID="txtToDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width key-off" autocomplete="off"></asp:TextBox>
                                            
                                            <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                PopupButtonID="imgEffectDateTo" Enabled="True"
                                                TargetControlID="txtToDate" ID="CalendarExtender1">
                                            </asp:CalendarExtender>
                                           
                                            

                                        </td>
                                    </tr>  
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td> <asp:Button ID="btnDateCalculation" runat="server" Text="Calculation"  class="Lbutton" OnClick="btnDateCalculation_Click" />
</td>
                                    </tr>

                                    <tr>
                                        <td>No. Of Days
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:TextBox ID="txtNoOfDays" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Enabled="False"></asp:TextBox>
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
                                             <asp:TextBox ID="txtTotalHolydays" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Enabled="False"></asp:TextBox>
                                           
                                        </td>
                                    </tr>
                                   

                                        <tr runat="server" id="trReplacedDate">
                                        <td>Replaced Date(s)
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:TextBox ID="txtReplacedDate" placeholder="dd-MM-yyyy, dd-MM-yyyy, dd-MM-yyyy,..."  runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" autocomplete="off"></asp:TextBox>
                                            
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
                                            <asp:DropDownList ID="ddlHandOverPerson" CssClass="form-control select_width" runat="server" ClientIDMode="Static" ></asp:DropDownList>
                                           </td>
                                    </tr>
                                    

                                        
                                      <tr >
                                        <td>Attach Document (if any)
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                            <asp:TextBox ID="txtFileName" ClientIDMode="Static" runat="server"  Style="display:none;position: absolute;width: 43%;float: left;margin-left: 3px; height:30px;margin: 5px 3px;"  Enabled="False"></asp:TextBox>                                         
                                          <asp:FileUpload ID="FileUploadDoc" runat="server"  style="width:211px;position: relative;margin-top: 10px;margin-left: 4px;margin-bottom: 7px;"/> 
                                        </td>
                                                  
                                    </tr>
                               
                                    <tr id="trpregnatn" runat="server"> 
                                        <td>Date of pregnant</td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">

                                            <asp:TextBox ID="txtPregnantDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" ></asp:TextBox>
                                           
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
                                            <asp:TextBox ID="txtPrasabaDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" ></asp:TextBox>
                                            
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
                                            <asp:CheckBox ID="chkApproved" runat="server" Text="Approved" Checked="True" Enabled="False"/> <asp:CheckBox ID="chkProcessed" runat="server" Text="Processed" Checked="True" Enabled="False"/>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td >
                                            Purpose of Leave
                                        </td>
                                        <td>:</td>
                                        <td class="tdWidth">
                                           <asp:TextBox ID="txtNotes" runat="server" Height="40px" ClientIDMode="Static" CssClass="form-control text_box_width" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                      <tr>
                                        <td >
                                            Leave Address
                                        </td>
                                        <td>:</td>
                                        <td class="tdWidth">
                                           <asp:TextBox ID="txtLvAddress" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" ></asp:TextBox>
                                   
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Contact</td>
                                        <td>:</td>
                                        <td> <asp:TextBox ID="txtLvContact" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <!--RT   -->
                        

                        <div style="border-radius: 5px; display:none; border: 2px solid #086A99;border-top:0px; font-weight:bold; width: 380px;background:#ddd;padding:5px;" id="PopupWindow" >
                            <div id="divDrag" class="boxFotter">
                                 <a ID="btnCancel" href="#"><img style="left: 373px;position: absolute;top: -1px;width: 5% !important;" src="../images/icon/cancel.png" alt="" /></a>
                           <center> 
                                
                                <h2>Leave Status</h2>
                                
                           </center>
                                
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
                                            <asp:Button ID="btnSelectAll" PostBackUrl="~/leave/all_leave_list.aspx" runat="server" Text="Leave List" CssClass="Lbutton" /></td>
                                       
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
