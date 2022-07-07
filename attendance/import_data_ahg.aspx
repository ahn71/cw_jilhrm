<%@ Page Title="" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="import_data_ahg.aspx.cs" Inherits="SigmaERP.attendance.import_data_ahg" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
</script>


     <style type="text/css">
        .alignment {
            text-align: center
        }
        .AlgRgh {
            text-align: right;
            font-family: Verdana, Arial, Helvetica, sans-serif;
        }
        .fileUpload {
        background-color:yellow;
        /*float: left;*/
        height: 36px;
     
        margin-top: 0;
        width: 188px;
        }
        .selectionBox {
            margin: 15px auto;
            font-size:13px;
            width:100%;
        }
        .selectionBox td:nth-child(2),td:nth-child(4),td:nth-child(6) {
            
            width:25%;
        }
        .tbl1 {
            margin:0px auto;
            font-size:13px;
            /*width:80%;*/
        }
         .tbl1 tr {
             height:50px;
           
        }
        .ajax__calendar_days table tr{
            height:auto !important;
        }
        .ajax__calendar_days table tr td:nth-child(2), td:nth-child(4), td:nth-child(6) {
          width: auto;
        }
         .table tr th:first-child, tr th:nth-child(2) {
            align-content:center;
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
                    <li><a href="#" class="ds_negevation_inactive Mactive">Processing Daily Attendance</a></li>
                </ul>
            </div>
        </div>
    </div>

    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>



    <div class="main_box Mbox">
         <div class="main_box_header MBoxheader">
            <h2>Processing Daily Attendance </h2>
        </div>
        <%--<div class="main_box_body">
            <div class="main_box_content">--%>
        <div class="employee_box_body">
                    <div class="employee_box_content">

                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                    <Triggers>

                       <%-- <asp:AsyncPostBackTrigger ControlID="btnImport" />--%>
                        <asp:AsyncPostBackTrigger ControlID="gvAttendance" />
                        <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                        <asp:AsyncPostBackTrigger ControlID="ddlDepartmentList" />
                        <asp:AsyncPostBackTrigger ControlID="rblDateType" />
                      
                    </Triggers>
                    <ContentTemplate>

                        <p>
                       
                        <p>
                        </p>

                        <div>
                            <center>
                                    <table>
                                        <tr>
                                            
                                            <td>
                                                <asp:RadioButtonList ForeColor="Red" Style="font-size:13px" Font-Bold="true" ID="rblImportType" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True" Value="FullImport">Full Process</asp:ListItem>
                                        <asp:ListItem Value="PartialImport">Partial Process</asp:ListItem>
                                    </asp:RadioButtonList>  
                                            </td>
                                            <td>&nbsp;|&nbsp;</td>
                                            <td>
                                                 <asp:RadioButtonList  ForeColor="blue" Style="font-size:13px" Font-Bold="true" ID="rblDateType" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal" AutoPostBack="true" >
                                        <asp:ListItem Selected="True" Value="SingleDate">Single Date</asp:ListItem>
                                        <%--<asp:ListItem Value="MultipleDate">Multiple Date</asp:ListItem>--%>
                                    </asp:RadioButtonList>  
                                            </td>
                                         
                                        </tr>
                                    </table>
                                     
                                    
                                    
                                                            
                                </center>
                        </div>
                        <br />
                        <div class="import_data_header">
                            <div style="width:80%;margin:0px auto"  >
                                <table  class="selectionBox">
                                    <tr>
                                        <td>Company <span class="requerd1">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCompanyList" runat="server" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" Width="96%" >
                                            </asp:DropDownList>
                                        </td>
                                        <td>Department<span class="requerd1">*</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDepartmentList" runat="server" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" Width="96%" >
                                            </asp:DropDownList>
                                        </td>
                                        <td id="tdSelectFile" runat="server" visible="true"  >
                                            Select File<span class="requerd1">*</span>
                                        </td>
                                      <td id="tdFileUpload" runat="server" visible="true"  >
                                          <asp:FileUpload ID="FileUpload1" runat="server" CssClass="fileUpload" />
                                      </td>
                                    </tr>
                                </table>
                              
                            </div>
                        </div>
                        <div class="import_data_content">
                            <div class="import_data_content_left">
                                <p style="text-align: center; font-weight: bold; margin: 0px 0px 15px;">
                                    Full Section
                                </p>
                             <%--   <p style="margin-left: 150px;">
                                    <span id="spnFullFromDate" runat="server">Date :</span>
                                    <asp:TextBox ID="txtFullAttDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" PlaceHolder="Click For Calendar" Width="174px" Style="margin-left: 10px"></asp:TextBox>
                                </p>--%>
                                <table  class="tbl1">
                                    <tr>
                                        <td id="spnFullFromDate" runat="server">Date<span class="requerd1">*</span></td>
                                        <td>:</td>
                                        <td><asp:TextBox ID="txtFullAttDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" PlaceHolder="Click For Calendar" Width="174px" Style="margin-left: 10px" autocomplete="off"></asp:TextBox>
                                             <asp:CalendarExtender ID="CExtApplicationDate" runat="server" Enabled="True" Format="dd-MM-yyyy" PopupButtonID="imgAttendanceDate" TargetControlID="txtFullAttDate">
                                </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    </table>
                               
                               
                                <p style="margin-left: 150px;">
                                    <span id="spnFullToDate" runat="server" visible="false">To Date </span>
                                    <asp:TextBox ID="txtFullToDate" Visible="false" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" PlaceHolder="Click For Calendar" Width="174px" Style="margin-left: 26px" autocomplete="off"></asp:TextBox>
                                </p>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" Format="dd-MM-yyyy" PopupButtonID="imgAttendanceDate" TargetControlID="txtFullToDate">
                                </asp:CalendarExtender>
                               
                            </div>
                            <div class="import_data_content_right">
                                <p style="text-align: center; font-weight: bold; margin: 0px 0px 15px;">
                                    Partial Section
                                </p>
                                <table  class="tbl1">
                                    <tr>
                                        <td>Card :&nbsp; <span class="requerd1">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtCardNo" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_import" PLaceHolder="Type Card No" autocomplete="off" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdPartialFromDate" runat="server">Date : &nbsp;<span class="requerd1">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtPartialAttDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_import" PLaceHolder="Click For Calendar" autocomplete="off"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtPartialAttDate_CalendarExtender" runat="server" Format="dd-MM-yyyy" TargetControlID="txtPartialAttDate">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>

                                    <tr id="trPartialToDate" runat="server" visible="false">
                                        <td>To Date : </td>
                                        <td>
                                            <asp:TextBox ID="txtPartialToDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_import" PLaceHolder="Click For Calendar" autocomplete="off"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd-MM-yyyy" TargetControlID="txtPartialToDate">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>



                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="import_data_footer" style="border-bottom:0;">
           
                    <div class="import_data_footer_right"  style="width: 185px; margin: 0px auto; float: none;">     
                              
                        <asp:Button ID="Button3" runat="server" CssClass="Mbutton" PostBackUrl="~/attendance_default.aspx" Text="Close" />
                        <asp:Button ID="btnImport" runat="server" Style="float: left;" CssClass="Mbutton"  Text="Process" OnClick="btnImport_Click" />

                    </div>

                </div>
                <div class="import_data_footer">
           
                    <div class="import_data_footer_right" style="height:30px;">     
                        <asp:Label ID="lblErrorMessage" runat="server" ClientIDMode="Static" ForeColor="Red" Text=""></asp:Label>
                    </div>

                </div>


                <div class="dataTables_wrapper">
                    <asp:GridView ID="gvAttendance" runat="server" AllowPaging="True" style="font-size:13px" AutoGenerateColumns="False" DataKeyNames="EmpCardNo" CellPadding="4" ForeColor="#333333" Height="13px"  PageSize="1500" Width="100%" OnPageIndexChanging="gvAttendance_PageIndexChanging" OnRowDataBound="gvAttendance_RowDataBound">
                        <PagerStyle CssClass="gridview" Height="20px" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>

                             <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hideSubId" runat="server" 
                                            Value='<%# DataBinder.Eval(Container.DataItem, "EmpCardNo")%>' />
                                        <%# Container.DataItemIndex+1%>
                                    </ItemTemplate>

                                    <ItemStyle HorizontalAlign="Center" ForeColor="green" />
                                </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Card No">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("EmpCardNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("EmpName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Department">
                                <ItemTemplate>
                                    <asp:Label ID="lblDptName" runat="server" Text='<%# Eval("DptName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Designation">
                                <ItemTemplate>
                                    <asp:Label ID="lblDsgName" runat="server" Text='<%# Eval("DsgName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblATTDate" runat="server" Text='<%# Eval("AttDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="In Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblIntime" ForeColor="Green" Font-Bold="true" runat="server" Text='<%# Eval("Intime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Out Time" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label Font-Bold="true" ID="lblOuttime" ForeColor="Red" runat="server" Text='<%# Eval("Outtime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblATTStatus" runat="server" Text='<%# Eval("ATTStatus") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                          
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#27235C" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F8FAFA" />
                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                        <SortedDescendingCellStyle BackColor="#D4DFE1" />
                        <SortedDescendingHeaderStyle BackColor="#15524A" />
                    </asp:GridView>
                    <ul runat="server" id="ulAttMissingLog" visible="false" >
                        <li><a target="_blank" href="attendance_missing_log.aspx">attendance missing log</a></li>
                    </ul>
                </div>



            </div>
        </div>
    </div>


    <%--<div class="import_middle">
                            <p>
                                <asp:RadioButton ID="RadioButton1" runat="server"></asp:RadioButton>
                                Full Import
                            </p>
                            <p>
                                <asp:Button ID="Button2" runat="server" Text="Button"/>
                                <asp:RadioButton ID="RadioButton2" runat="server"></asp:RadioButton>
                                Partial Import.
                            </p>
                        </div>--%>
    <%--<div class="import_bottom">
                            
                            <asp:ListBox ID="ListBox1" Width="546" Height="120" runat="server"></asp:ListBox>
                        </div>--%>
    <%--                 </div>
                    
                </div>
            </div>--%>





    <script type="text/javascript">
        function InputValidationBasket() {
            try {
                             
                if ($('#FileUpload1').val().trim().length==0)
                {
                    alert('Please select access database file !');
                    $('#FileUpload1').focus();
                    return true;
                }

                if ($('#rblImportType input:checked').val().trim() == "FullImport") {
                    if ($('#txtFullAttDate').val().trim().length == 0) {
                        showMessage('Please select date for full attendance import', 'error');
                        $('#txtFullAttDate').focus(); return false;
                    }
                }
                else {
                    if ($('#txtCardNo').val().trim().length < 4) {
                        showMessage('Please type valid card no', 'error');
                        $('#txtCardNo').focus(); return false;
                    }

                    if ($('#txtPartialAttDate').val().trim().length == 0) {
                        showMessage('Please select date for partial attendance import', 'error');
                        $('#txtPartialAttDate').focus(); return false;
                    }
                }

                
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }

        function ClearInputBox()
        {
            try
            {
                $('#txtCardNo').val('');
                $('#txtFullAttDate').val('');
                $('#txtPartialAttDate').val('');
                
                $('#ddlDivisionName').val('0');
                $('#ddlShiftName').val('0');
                
            }
            catch (exception)
            {
                showMessage(exception, error)
            }
        }

        function alertMessage() {

            setTimeout(function () {
                $('#lblErrorMessage').fadeOut("slow", function () {
                    $('#lblErrorMessage').remove();
                    $('#lblErrorMessage').val('');
                });

            }, 3000);
        }

    </script>
</asp:Content>
