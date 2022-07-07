<%@ Page Title="Promotions" Language="C#" MasterPageFile="~/personnel_NestedMaster.Master" AutoEventWireup="true" CodeBehind="promotion.aspx.cs" Inherits="SigmaERP.personnel.promotion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <%--  <script src="../scripts/jquery-1.8.2.js"></script>--%>
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
        .auto-style1 {
            width: 2px;
        }

        .hide {
            display: none;
        }
        .gvRight th {
            text-align:center;
        }
        #ContentPlaceHolder1_MainContent_divpromotionList th:nth-child(2),td:nth-child(2) {
            text-align:left;
            padding-left:3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
                  <div class="col-md-12 ds_nagevation_bar">
               <div style="margin-top: 5px">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                       <li> <a class="seperator" href="#"></a>/</li>
                       <li> <a href="/payroll_default.aspx">Payroll</a></li>   
                       <li> <a class="seperator" href="#"></a>/</li>                  
                        <li> <a href="/payroll/salary_index.aspx">Salary</a></li>
                        <li> <a class="seperator" href="#"></a>/</li>
                       <li> <a href="#" class="ds_negevation_inactive Pactive">Employee Promotion</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:HiddenField ID="hfSaveStatus" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfEmpSN" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfEmpTypeId" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfDeleteStatus" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfBasicPercentage" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfConvenceAllownce" ClientIDMode="Static" runat="server" Value="0" />
    <asp:HiddenField ID="hfMedicalAllownce" ClientIDMode="Static" runat="server" Value="0" />

    <asp:HiddenField ID="hfDepartmentId" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfDesignationId" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfGradeId" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="upSave" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upupdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="updelete" runat="server" ClientIDMode="Static" />
  <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Promotions</h2>
        </div>        
    	<div class="main_box_body Pbody">
            <div class="main_box_content">
                <div class="salary_increment_left_area">
                    <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddlCompany" />
                            <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                            <asp:AsyncPostBackTrigger ControlID="ddlEmpType" />
                            <asp:AsyncPostBackTrigger ControlID="ddlNewDepartment" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                            <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:HiddenField ID="hfSalaryType" ClientIDMode="Static" runat="server" Value="Save" />
                            <asp:HiddenField ID="hfEditPromotion" ClientIDMode="Static" runat="server" Value="noEdit" />
                            <div class="punishment_against">
                                <table class="employee_table">
                                    <tbody>
                                        <tr id="trCompany" runat="server" >
                                            <td>Company
                                            </td>
                                            <td>:
                                            </td>
                                            <td id="td1" runat="server" clientidmode="Static" style="font-size: 16px">
                                                <asp:DropDownList ID="ddlCompany" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                                </asp:DropDownList>

                                            </td>
                                        </tr>
                                        <tr runat="server" visible="false">
                                            <td>Emp type
                                            </td>
                                            <td>:
                                            </td>
                                            <td id="tdEmpType" runat="server" clientidmode="Static" style="font-size: 16px">
                                                <asp:DropDownList ID="ddlEmpType" ClientIDMode="Static" CssClass="form-control select_width" runat="server" OnSelectedIndexChanged="ddlEmpType_SelectedIndexChanged" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Employee Card No
                                            </td>
                                            <td>:
                                            </td>
                                            <td id="tdCardNo" runat="server" clientidmode="Static" style="font-size: 16px">
                                                <table class="employee_table">
                                                    <tr>
                                                        <td runat="server" visible="false"><asp:TextBox ClientIDMode="Static" ID="txtEmpCardNo" autocomplete='off'  CssClass="form-control text_box_width" runat="server"></asp:TextBox></td>
                                                        <td colspan="2"><asp:DropDownList ID="ddlEmpCardNo" runat="server"  ClientIDMode="Static"  CssClass="form-control select_width" AutoPostBack="true"  OnSelectedIndexChanged="ddlEmpCardNo_SelectedIndexChanged"></asp:DropDownList></td>
                                                        <td runat="server" visible="false"><asp:Button ID="btnSearch" ClientIDMode="Static" CssClass="Pbutton" runat="server" Text="Find" Width="60px" OnClientClick="return ForFindInputValidationBasket();" OnClick="btnSearch_Click"  /></td>
                                                        <td><asp:Button ID="btnPromotionInfo" runat="server"  CssClass="Pbutton" Text="Promotion Status" OnClick="btnPromotionInfo_Click" /></td>
                                                    </tr>
                                                </table>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Employee Name
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmpName" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False"></asp:TextBox>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Last Increment Date
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLastIncrementDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False"></asp:TextBox>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Last Increment Amount
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLastIncrementAmount" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False"></asp:TextBox>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Previous Gross
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPreGross" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Increment Amount
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIncrementAmount" runat="server"  ClientIDMode="Static" onkeypress="return totalSalaryCalculation(event)"  CssClass="form-control text_box_width"></asp:TextBox>
                                               <%--  New Gross:  <asp:TextBox ID="txtNewGross" CssClass="form-control text_box_width_2" runat="server" ClientIDMode="Static" ></asp:TextBox>--%>

                                            </td>
                                        </tr>
                                         <tr>
                                            <td>New Gross
                                            </td>
                                            <td>:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNewGross" runat="server"  ClientIDMode="Static" Enabled="false"  CssClass="form-control text_box_width"></asp:TextBox>
                                               <%--  New Gross:  <asp:TextBox ID="txtNewGross" CssClass="form-control text_box_width_2" runat="server" ClientIDMode="Static" ></asp:TextBox>--%>

                                            </td>
                                        </tr>

                                    </tbody>
                                </table>

                            </div>
                            <div class="promo_middle_box">
                                <div class="promo_box_left">
                                    &nbsp;
                                </div>
                                <!--RT   -->
                            </div>


                            <!--ST-->
                            <div class="promo_middle_box">
                                <div class="promo_box_left">
                                    <fieldset style="min-height: 127px">
                                        <legend>
                                            <b>Present Job Description</b>
                                        </legend>
                                        <table class="employee_table">

                                            <tr>
                                                <td>Department
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtPresentDepartment" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" EnableTheming="True" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Designation
                                                </td>
                                                <td>:
                                                </td>
                                                <td>

                                                    <asp:TextBox ID="txtPresentDesignation" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Grade
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPresentGrade" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>



                                        </table>
                                    </fieldset>
                                </div>
                                <!--RT   -->
                                <div class="promo_box_right">
                                    <fieldset style="padding: 0px">
                                        <legend>
                                            <b>New Job Description</b>
                                        </legend>
                                        <table class="employee_table">
                                            <tr>
                                                <td>Department
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlNewDepartment" runat="server" Height="30px" ClientIDMode="Static" Width="197px" CssClass="form-control select_width" OnSelectedIndexChanged="ddlNewDepartment_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Designation
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlNewDesignation" runat="server" Height="30px" ClientIDMode="Static" Width="197px" CssClass="form-control select_width"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Grade
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlNewGrade" runat="server" Height="30px" ClientIDMode="Static" Width="197px" CssClass="form-control select_width"></asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </div>
                            </div>
                            <div class="punishment_against">
                                <table class="employee_table">
                                    <tbody>
                                        <tr>
                                          <td>New Emp Type
                                            </td>
                                            <td class="auto-style1">:
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlNewEmpType" runat="server" ClientIDMode="Static" Width="197px" CssClass="form-control select_width"></asp:DropDownList>
                                            </td>
                                        </tr>                                       
                                        <tr>
                                            <td>Emp Promotion Order Ref Number
                                            </td>
                                            <td class="auto-style1">:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIncrementOrderRefNumber" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Emp Promotion Order Date
                                            </td>
                                            <td class="auto-style1">:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIncrementOrderRefDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtIncrementOrderRefDate_CalendarExtender" Format="d-M-yyyy" runat="server" TargetControlID="txtIncrementOrderRefDate">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Effective From</td>
                                            <td class="auto-style1">:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEffectiveFrom" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtEffectiveFrom_CalendarExtender" Format="MM-yyyy" runat="server" TargetControlID="txtEffectiveFrom">
                                                </asp:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Remarks
                                            </td>
                                            <td class="auto-style1">:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRemarks" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="emp_button_area" style="width: 426px">
                                <table class="promo_button_table">
                                    <tbody>
                                        <tr>
                                            <th>
                                                <asp:Button ID="btnSave" ClientIDMode="Static" CssClass="Pbutton" runat="server" Text="Save" OnClientClick="return InputValidationBasket();" OnClick="btnSave_Click" /></th>
                                            <th>
                                                <asp:Button ID="btnClear" ClientIDMode="Static" CssClass="Pbutton" runat="server" Text="Clear" OnClientClick="ClearInputBox()"  /></th>
                                            <th>
                                                <asp:Button ID="btnClose" ClientIDMode="Static" Width="80px" CssClass="Pbutton" runat="server" Text="Close" PostBackUrl="~/default.aspx" /></th>
                                            <th>
                                                <asp:Button ID="btnDelete" ClientIDMode="Static" CssClass="Pbutton" runat="server" Text="Delete" OnClick="btnDelete_Click" /></th>
                                            <th>
                                                <asp:Button ID="btnComplain" runat="server" Text="Complain" CssClass="css_btn" Visible="false" OnClick="btnComplain_Click" /></th>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                 <div runat="server" id="divPagelist" visible="true" style="height: 841px; width: auto; padding: 0 0 0 15px; overflow: scroll">
                <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave" />
                        <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                         <asp:AsyncPostBackTrigger ControlID="ddlCompany" />
                        <%-- <asp:AsyncPostBackTrigger ControlID="btnClear" />--%>
                    </Triggers>
                    <ContentTemplate>
                       <%-- <div runat="server" id="divPagelist" visible="true" style="height: 841px; width: 40%; padding: 0 0 0 15px; overflow: scroll">--%>
                            <%-- <div id="divpromotionList" runat="server" class="salary_increment_right_area"></div>--%>
                            <asp:GridView ID="divpromotionList" runat="server" CssClass="gvRight" Width="99%" style="margin-left:1px;" AutoGenerateColumns="False" DataKeyNames="SN,EmpId" HeaderStyle-BackColor="#ffa500" HeaderStyle-Height="28px" HeaderStyle-ForeColor="white" HeaderStyle-Font-Size="11px" HeaderStyle-Font-Bold="true" AllowPaging="True" OnRowCommand="divpromotionList_RowCommand" OnRowDeleting="divpromotionList_RowDeleting" OnRowDataBound="divpromotionList_RowDataBound">

                                <HeaderStyle BackColor="#ffa500 " Font-Bold="True" Font-Size="11px" ForeColor="White" Height="28px"></HeaderStyle>
                                <PagerStyle CssClass="gridview" Height="20px" />

                                <RowStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="SN" HeaderText="SN" Visible="false" ItemStyle-Height="28px">
                                        <ItemStyle Height="28px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmpId" HeaderText="EmpId" Visible="false" ItemStyle-Height="28px">
                                        <ItemStyle Height="28px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CompanyName" HeaderStyle-HorizontalAlign="Left" HeaderText="Company Name" Visible="false" ItemStyle-Width="100px" ItemStyle-Height="28px" ItemStyle-HorizontalAlign="Left">
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                                        <ItemStyle HorizontalAlign="Left" Height="28px" Width="100px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" Visible="true">
                                        <%--<ItemStyle Height="28px" Width="120px"></ItemStyle>--%>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmpName" HeaderText="Name" Visible="true" ItemStyle-Width="120px" ItemStyle-Height="28px">
                                        <ItemStyle Height="28px" Width="120px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DptName" HeaderText="Dpt.Name" Visible="true" >
                                        <%--<ItemStyle Height="28px" Width="120px"></ItemStyle>--%>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DsgName" HeaderText="Dsg Name" Visible="true" >
                                        <%--<ItemStyle Height="28px" Width="150px"></ItemStyle>--%>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmpType" HeaderText="EmpType" Visible="false" ItemStyle-Width="84px" ItemStyle-Height="28px">
                                        <ItemStyle Height="28px" Width="84px"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateofUpdate" HeaderText="DateofUpdate" Visible="false" ItemStyle-Width="84px" ItemStyle-Height="28px">
                                        <ItemStyle Height="28px" Width="84px"></ItemStyle>
                                    </asp:BoundField>
                                    <%-- <asp:TemplateField HeaderText="Alter">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkAlter" runat="server" CommandName="Alter" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Alter" Font-Bold="true" ForeColor="Green" ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="deleterow" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Delete" Font-Bold="true" ForeColor="Red" OnClientClick="return confirm('Are you sure to delete?');"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        
                    </ContentTemplate>
                </asp:UpdatePanel>
                     </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {           
            $(document).on("keypress", "body", function (e) {
                if (e.keyCode == 13) e.preventDefault();
                // alert('deafault prevented');

            });

           
        });
        $(document).ready(function () {
            load();
        });
        function totalSalaryCalculation(e) {
            if (e.keyCode == 13) {
                $('#txtNewGross').val(Math.round((parseFloat($('#txtIncrementAmount').val()) + parseFloat($('#txtPreGross').val())), 0));
            }
        }
        //$(document).ready(function () {
        //    $("#txtIncrementAmount").keydown(function (e) {
        //        // Allow: backspace, delete, tab, escape, enter and .
        //        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        //            // Allow: Ctrl+A
        //            (e.keyCode == 65 && e.ctrlKey === true) ||
        //            // Allow: home, end, left, right, down, up
        //            (e.keyCode >= 35 && e.keyCode <= 40)) {
        //            // let it happen, don't do anything
        //            showMessage("Please Type (0-9) Number","error");
        //            return;
        //        }
        //        // Ensure that it is a number and stop the keypress
        //        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        //            e.preventDefault();
        //        }
        //    });
        //});
        function getCardNo() {

            var val = document.getElementById('ddlEmpCardNo').value;
            document.getElementById('txtEmpCardNo').value = val;
        }
            function ClearInputBox() {
                try {
                    $('#ddlEmpType').show();
                    $('#hfDeleteStatus').val('0');

                    $('#txtEmpCardNo').val('Card No');
                    $('#txtEmpName').val('');
                    $('#ddlEmpCardNo').val('Card No');
                    $('#txtLastIncrementDate').val('');
                    $('#txtLastIncrementAmount').val('');
                    $('#txtIncrementAmount').val('');
                    $('#txtPresentDivision').val('');
                    $('#txtPresentDepartment').val('');
                    $('#txtPresentDesignation').val('');
                    $('#txtPresentGrade').val('');
                    $('#txtPresentLine').val('');
                    $('#txtPresentFloor').val('');
                    $('#txtPresentGroup').val('');
                    $('#ddlNewDivision').val('');
                    $('#ddlNewDepartment').val('');
                    $('#ddlNewDesignation').val('');
                    $('#ddlNewGrade').val('');
                    $('#ddlNewLine').val('');
                    $('#ddlNewFloor').val('');
                    $('#ddlNewGroup').val('');
                    $('#ddlNewEmpType').val('Select Type');
                    $('#txtNewEmpCardNo').val('');
                    $('#txtIncrementOrderRefNumber').val('');
                    $('#txtIncrementOrderRefDate').val('');
                    $('#txtEffectiveFrom').val('');
                    $('#txtRemarks').val('');
                    $('#hfSaveStatus').val('Save');
                    $('#btnSave').val("Save");
                    $('#ddlEmpCardNo').show();
                    $('#txtEmpCardNo').show();
                    $('#btnSearch').show();
                    load();
                }
                catch (exception) {

                }
            }

            function InputValidationBasket() {
                try {
                    if ($('#txtEmpCardNo').val().trim().length < 8) {
                        showMessage("Please type or select valid card no", "error");
                        $('#txtEmpCardNo').focus();
                        return false;
                    }
                    else if ($('#txtIncrementOrderRefNumber').val().trim().length == 0) {
                        showMessage("Please type the  increment order reference number", "error");
                        $('#txtIncrementOrderRefNumber').focus();
                        return false;
                    }
                    else if ($('#txtIncrementOrderRefDate').val().trim().length == 0) {
                        showMessage("Please select the  increment order reference date", "error");
                        $('#txtIncrementOrderRefDate').focus();
                        return false;
                    }
                    else if ($('#txtEffectiveFrom').val().trim().length == 0) {
                        showMessage("Please select the  effective date", "error");
                        $('#txtEffectiveFrom').focus();
                        return false;
                    }

                    else if ($('#txtNewEmpCardNo').val().trim().length == 0) {
                        return true;
                    }
                    else if (($('#txtNewEmpCardNo').val().trim().length != 0) && ($('#txtNewEmpCardNo').val().trim().length < 8)) {
                        showMessage("Card no must be 8 degits", "error");
                        $('#txtNewEmpCardNo').focus();
                        return false;
                    }

                    if (($('#ddlNewDivision').val().trim() != "0") && ($('#ddlNewDepartment').val().trim() == "0")) {

                        showMessage("Please select the new department", "error");
                        $('#ddlNewDepartment').focus();
                        return false;
                    }

                    else if (($('#ddlNewDepartment').val().trim() != "0") && ($('#ddlNewDesignation').val().trim() == "0")) {
                        showMessage("Please select the new designation", "error");
                        $('#ddlNewDesignation').focus();
                        return false;
                    }

                    else if ($('#ddlNewGrade').val().trim() == 0) {
                        showMessage("Please select the  new grade", "error");
                        $('#ddlNewGrade').focus();
                        return false;
                    }

                    else if (($('#ddlNewDivision').val().trim() != "0") && ($('#ddlNewDepartment').val().trim() != "0") && ($('#ddlNewLine').val().trim() == "0") && ($('#ddlNewFloor').val().trim() == "0") && ($('#ddlNewGroup').val().trim() == "0")) {
                        showMessage("Please select a type from line or floor or gropu", "error");
                        $('#ddlNewLine').focus();
                        return false;
                    }
                }
                catch (exception) {

                }
            }

            function ForFindInputValidationBasket() {
                try {
                    if ($('#txtEmpCardNo').val().trim().length < 8) {
                        showMessage("Please type or select valid card no", "error");
                        $('#txtEmpCardNo').focus();
                        return false;
                    }
                }
                catch (exception) {
                }
            }

            function editPromotion(getId) {
                try {
                    $('#ddlEmpType').hide();
                    $('#tdEmpType').text($('#r_' + getId + ' td:nth-child(4)').html());
                    jx.load('/ajax.aspx?esn=' + getId + '&todo=getPromotion' + '&amount= ' + 0 + '&status=' + status + ' ', function (data) {
                        var getInfo = data.split("_");
                        $('#hfEmpSN').val(getId);
                        $('#hfEmpTypeId').val(getInfo[30]);
                        $('#ddlEmpType').val(getInfo[29]);
                        $('#txtEmpCardNo').val($('#r_' + getId + ' td:first-child').html());
                        $('#txtEmpName').val(getInfo[1]);
                        $('#ddlEmpCardNo').val($('#r_' + getId + ' td:first-child').html());
                        $('#txtLastIncrementDate').val(getInfo[5]);
                        $('#txtLastIncrementAmount').val(getInfo[4]);
                        $('#hfDepartmentId').val(getInfo[14]);
                        $('#txtPresentDepartment').val(getInfo[15]);
                        $('#hfDesignationId').val(getInfo[16]);
                        $('#txtPresentDesignation').val(getInfo[17]);
                        $('#hfGradeId').val(getInfo[18]);
                        $('#txtPresentGrade').val(getInfo[19]);
                        $('#ddlNewEmpType').val(getInfo[29]);
                        $('#txtNewEmpCardNo').val($('#r_' + getId + ' td:first-child').html());
                        $('#txtIncrementOrderRefNumber').val(getInfo[10]);
                        $('#txtIncrementOrderRefDate').val(getInfo[11]);
                        $('#txtEffectiveFrom').val(getInfo[12]);
                        $('#txtRemarks').val(getInfo[28]);
                        $('#ddlNewDepartment').val('');
                        $('#ddlNewDesignation').val('');
                        $('#ddlNewGrade').val('');
                        $('#hfSaveStatus').val('View');
                        $('#btnSave').val("View");
                        $('#ddlEmpCardNo').show();
                        $('#txtEmpCardNo').show();
                        $('#btnSearch').show();
                        if ($('#updelete').val() == '1') {
                            $('#btnDelete').removeClass("hide");
                            $('#btnDelete').addClass('emp_btn');
                            $('#btnDelete').removeAttr('disabled');
                        }
                        else {
                            $('#btnDelete').removeClass('emp_btn');
                            $('#btnDelete').attr('disabled', 'disabled');
                        }
                    });
                }
                catch (exception) {
                    showMessage(exception);
                }
            }


            function getDeleteMessage() {
                try {
                    $('#hfDeleteStatus').val('0')

                    var isdelete = confirm("Do you want to delete ?");
                    alert(isdelete);
                    if (isdelete == true) {
                        $('#hfDeleteStatus').val('1');
                        alert($('#hfDeleteStatus').val());
                    }
                    else $('#hfDeleteStatus').val('0');
                    alert($('#hfDeleteStatus').val());
                }
                catch (exception) {
                    showMessage(exception);
                }
            }

            function CloseWindowt() {
                window.close();
            }

            function goToNewTabandWindow(url) {
                window.open(url);
                load();
            }
            function load() {
                $("#ddlEmpCardNo").select2();
            }
    </script>
</asp:Content>
