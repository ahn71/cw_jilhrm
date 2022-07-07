<%@ Page Title="Rejoin of Employee" Language="C#" MasterPageFile="~/personnel_NestedMaster.Master" AutoEventWireup="true" CodeBehind="rejoin_of_employee.aspx.cs" Inherits="SigmaERP.personnel.rejoin_of_employee" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:HiddenField ID="hdnUpdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnbtnStage" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdfCardNo" runat="server" ClientIDMode="Static" />
    <div class="punishment_main_box">
        <div class="punishment_box_header">
            <h2>Rejoin of Employee</h2>
        </div>
        <div class="punishment_bottom_header">
            <h3>Rejoin of Employee</h3>
            <p>
                <asp:Label ID="lblHeader" runat="server" ClientIDMode="Static" Text="Add Mode"></asp:Label></p>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content">
                <div class="punishment_against">

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                            <asp:AsyncPostBackTrigger ControlID="rdoEmpType" />
                        </Triggers>
                        <ContentTemplate>                     

        <table class="employee_table">

                   <tr>
                       <td>
                           Employee Type
                       </td>
                       <td>
                           :
                       </td>
                       <td>
                       <asp:RadioButtonList ID="rdoEmpType" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" Width="50%" OnSelectedIndexChanged="rdoEmpType_SelectedIndexChanged">
                        <asp:ListItem Value="0">Worker</asp:ListItem>
                        <asp:ListItem Value="1">Staff</asp:ListItem>
                        </asp:RadioButtonList>
                           </td>
                   </tr>
                  <tr>
                            <td>
                                Employee Card No
                            </td>
                            <td>
                                :
                            </td>
                            <td id="tdCardNo" runat="server" ClientIDMode="Static" style="font-size:16px" >
                               <asp:DropDownList ID="ddlEmployeeCardNo" ClientIDMode="Static" CssClass="form-control select_width" runat="server">
                                   
                                </asp:DropDownList>
                                
                            </td>
                    </tr>
                    <tr>
                            <td>
                                Effective Date
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtEffectiveDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" Format=dd-MM-yyyy runat="server" TargetControlID="txtEffectiveDate"></asp:CalendarExtender>
                                
                            </td>
                    </tr>
                <tr>
                            <td>
                               Remarks
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                
                            </td>
                    </tr>
               
            </table>
                            
                    <div class="punishment_button_area">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                <th><asp:Button ID="btnSave" CssClass="emp_btn" runat="server" ClientIDMode="Static" Text="Save" OnClientClick="return validateInputs();" OnClick="btnSave_Click" /></th>
                                 <th><asp:Button ID="btnClear" ClientIDMode="Static" CssClass="emp_btn" runat="server"  Text="Clear" /></th>
                                <th><a class="css_btn" href="../default.aspx" >Close</a></th>
                                <th><asp:Button ID="btnDelete" CssClass="emp_btn" ClientIDMode="Static" runat="server" Text="Delete" OnClick="btnDelete_Click" /></th> 
                         </tr>
                            
                    </tbody>
                  </table>
                </div>

                            </ContentTemplate>
                    </asp:UpdatePanel>
  
            </div >
                <div>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                        </Triggers>
                        <ContentTemplate>

                       
                    
                 <div id="divrejoinEmployee" class="datatables_wrapper" runat="server" style="width:100%; height:auto; max-height:600px;overflow:auto;overflow-x:hidden;"></div>
                             </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>
              
            </div>
           
        </div>
               
     </div>
   
    <script type="text/javascript">
        function validateInputs() {
            if (validateText('txtEffectiveDate', 1, 60, 'Please Select Effective Date') == false) return false;
            return true;
        }
        function editRejoin(id) {
            $('#btnSave').val('Alter');
            $('#ddlEmployeeCardNo').hide();
            $('#tdCardNo').text($('#r_' + id + ' td:first-child').html());
            $('#hdfCardNo').val($('#r_' + id + ' td:first-child').html());
            var emptype = $('#r_' + id + ' td:nth-child(2)').html();
            
            if (emptype == 'Worker') {
                //alert(document.getElementsByName('rdoEmpType')[0].checked);
                //var v = 1;
                //$('#rdoEmpType[type=radio][value=' + v + ']').prop('checked', true);
            }
            else {
              //  $(':rdoEmpType[value="1"]').attr('checked', 'checked');
            }
            var effectiveDate = $('#r_' + id + ' td:nth-child(3)').html();
            $('#txtEffectiveDate').val(effectiveDate);
            var remarks = $('#r_' + id + ' td:nth-child(4)').html();
            $('#txtRemarks').val(remarks);
            $('#btnDelete').addClass('emp_btn');
            $('#btnDelete').removeAttr('disabled');
            $('#hdnUpdate').val(id);
            $('#hdnbtnStage').val(1);
            $('#lblHeader').val('Edit Mode');
           
            $('#btnClear').click(function () {
                $('#btnSave').val('Save');
                $('#hdnUpdate').val('');
                $('#hdnbtnStage').val('');
                $('#lblHeader').val('Add Mode');
                $('#txtEffectiveDate').val('');
                $('#txtRemarks').val('');
               
            });
        }

    </script>
</asp:Content>
