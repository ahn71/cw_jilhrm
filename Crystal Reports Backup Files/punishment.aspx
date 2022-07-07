<%@ Page Title="Punishment" Language="C#" MasterPageFile="~/personnel_NestedMaster.Master" AutoEventWireup="true" CodeBehind="punishment.aspx.cs" Inherits="SigmaERP.personnel.punishment" Debug="true" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:HiddenField ID="hfSaveStatus" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfPunismentId" ClientIDMode="Static" runat="server" Value="Save" />
    <div class="punishment_main_box_2">
        <div class="punishment_box_header">
            <h2>Punishment</h2>
        </div>
        <div class="punishment_bottom_header">
            <h3>Add Punishment</h3>
           <p style="font-weight: bold; font-size: 16px;"> Punisment List</p>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content_punishment">
                <div class="punishment_against_2">
                    
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger  ControlID="ddlEmpCardNo"/>
                        </Triggers>
                        <ContentTemplate>
                            <table class="employee_table">
                                <tr>
                                    <td>Employee Card No
                                    </td>
                                    <td>:
                                    </td>
                                    <td id="tdCardNo" runat="server" ClientIDMode="Static" style="font-size:16px">
                                        <asp:TextBox ClientIDMode="Static" ID="txtEmpCardNo" autocomplete='off'   Style="width: 66px" CssClass="form-control text_box_width" runat="server"></asp:TextBox>
                                       <asp:DropDownList ID="ddlEmpCardNo" runat="server" ClientIDMode="Static" Width="116px" CssClass="form-control select_width" onChange="getCardNo()"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Punisment Order Ref
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPunismetnOrderRef" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>Punisment Order Ref Date
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPunismetnOrderRefDate" runat="server" autocomplete="off" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>


                                        <asp:CalendarExtender ID="txtPunismetnOrderRefDate_CalendarExtender" Format="d-M-yyyy"  runat="server" TargetControlID="txtPunismetnOrderRefDate">
                                        </asp:CalendarExtender>


                                    </td>
                                </tr>
                                <tr>
                                    <td>Punisment Type
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPunismentType" runat="server" ClientIDMode="Static" Width="195px" CssClass="form-control select_width"></asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <td>Amount
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAmount" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>Remarks
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemarks" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Height="54px" TextMode="MultiLine" Width="190px"></asp:TextBox>

                                    </td>
                                </tr>
                            </table>
                            <div class="punishment_button_area">
                                <table class="emp_button_table_fix">
                                    <tbody>
                                        <tr>
                                            <th>
                                                <asp:Button ID="btnSave" CssClass="emp_btn" runat="server" OnClientClick="return validateInputs();" ClientIDMode="Static" Text="Save" OnClick="btnSave_Click" />
                                            </th>
                                            <th>
                                                <asp:Button ID="btnClear" CssClass="emp_btn" runat="server" ClientIDMode="Static" Text="Clear" OnClientClick="ClearInputBox()" />
                                            </th>
                                            <th>
                                               <th><a class="css_btn" href="../default.aspx" >Close</a></th>
                                            </th>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
            </div>
                <asp:UpdatePanel runat="server" ID="up3" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSave"  />
                    </Triggers>
                    <ContentTemplate>
                        <div class="punishment_against_3">
                        <div runat="server" id="divPunismentList" style="width:508px;height: 599px;"></div>
            </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                    


            </div>
        </div>
     </div>
    <script src="../scripts/jquery-1.8.2.js"></script>
    <script type="text/javascript">
        function getCardNo()
        {
            
           var val = document.getElementById('ddlEmpCardNo').value;
           document.getElementById('txtEmpCardNo').value = val;

        }
        function validateInputs() {
            if (validateText('txtEmpCardNo', 1, 60, 'Select Employee Card No') == false) return false;
            if (validateText('txtPunismetnOrderRef', 1, 60, 'Enter Punishment Order Ref ') == false) return false;
            if (validateText('txtPunismetnOrderRefDate', 1, 60, 'Select Punishment Order Ref Date ') == false) return false;
            if (validateText('txtAmount', 1, 60, 'Enter Punishment Amount ') == false) return false;
            return true;
        }

        function ClearInputBox()
        {
            document.getElementById('txtEmpCardNo').value = "";
            document.getElementById('txtPunismetnOrderRef').value = "";
            document.getElementById('txtPunismetnOrderRefDate').value = "";
            document.getElementById('ddlPunismentType').value = "";
            document.getElementById('txtAmount').value = "";
            document.getElementById('txtRemarks').value = "";
            document.getElementById('btnSave').value = "Save";
            document.getElementById('hfSaveStatus').value="Save";
        }

        function editFeesType(getId)
        {
            $('#hfPunismentId').val(getId);
          
            $('#hfSaveStatus').val("Update");
            $('#btnSave').val("Update");

            $('#txtEmpCardNo').val($('#r_' + getId + ' td:first-child').html());
            $('#ddlEmpCardNo').val($('#r_' + getId + ' td:first-child').html());
            $('#txtEmpCardNo').hide();
            $('#ddlEmpCardNo').hide();
            $('#tdCardNo').text($('#r_' + getId + ' td:first-child').html());
           
            $('#txtPunismetnOrderRef').val($('#r_' + getId + ' td:nth-child(2)').html());
            
            $('#txtPunismetnOrderRefDate').val($('#r_' + getId + ' td:nth-child(3)').html());
            
            $('#ddlPunismentType').val($('#r_' + getId + ' td:nth-child(3)').html());
            
            jx.load('/ajax.aspx?id=' + getId + '&todo=getPunismentAmount' + '&amount= ' + 0+ '&status=' + status + ' ', function (data) {
                var getVale = data;
                var getData = getVale.split("_");
                
                $('#txtAmount').val(getData[0]);
                $('#txtRemarks').html(getData[1]);

            });
        }
    </script>
</asp:Content>
