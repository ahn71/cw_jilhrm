<%@ Page Title="Employee Experience" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="employee_experience.aspx.cs" Inherits="SigmaERP.personnel.employee_experience" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/style/jquery-ui-datepekar.css" rel="stylesheet" />
    <link href="/style/dataTables.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
     <asp:HiddenField ID="hdfexperience" runat="server" ClientIDMode="Static"  />
<div style="padding:0;margin-top:25px;">
    <div class="row">
   <div class="col-lg-12">
         <div class="employee_box_header PtBoxheader">
         <h2>Experience Panel</h2>
     </div>
     <div class="employee_box_body personal_color_body Ptbody">
         
         <div class="employee_box_content">
     <div class="personal_employee_experience_main" runat="server" id="divEmpExperience"  >
                  <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                 <Triggers>
                     
                 </Triggers>
                 <ContentTemplate>
                     <div class="row">
                     <div class="col-lg-1"></div>
                     <div class="col-lg-10">
             <div class="personal_employee_experience">
                 <div class="row">
                     <div class="col-lg-6">
                            <div class="form-horizontal form_horizontal_custom">
                            <div class="form-group">
                            <label for="inputEmail3" class="col-sm-4 control-label">Name of Company</label>
                            <div class="col-sm-7 padding_right">
                                <asp:TextBox ID="txtCompanyName" CssClass="form-control"  ClientIDMode="Static" runat="server"></asp:TextBox>
                            </div>
                            </div>
                            <div class="form-group">
                            <label for="inputPassword3" class="col-sm-4 control-label">Responsibility</label>
                            <div class="col-sm-7 padding_right">
                                <asp:TextBox ID="txtResponsibility" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                            </div>
                            </div>
                            <div class="form-group">
                            <label for="inputPassword3" class="col-sm-4 control-label">Joining Date</label>
                            <div class="col-sm-7 padding_right">
                                <asp:TextBox ID="txtJoiningDateExperience" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender6" Format=d-M-yyyy runat="server" TargetControlID="txtJoiningDateExperience"></asp:CalendarExtender>
                            </div>
                            </div>
                            <div class="form-group">
                                <label for="inputPassword3" class="col-sm-4 control-label">Special Qualification</label>
                                <div class="col-sm-7 padding_right">
                                    <asp:TextBox ID="txtSpecialQualification" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                     </div>
                     <div class="col-lg-6">
                          <div class="form-horizontal form_horizontal_custom">
                            <div class="form-group">
                            <label for="inputEmail3" class="col-sm-4 control-label">Designation</label>
                            <div class="col-sm-7 padding_right">
                                <asp:TextBox ID="txtDesignation" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                            </div>
                            </div>
                            <div class="form-group">
                            <label for="inputPassword3" class="col-sm-4 control-label">Year of Experience</label>
                            <div class="col-sm-7 padding_right">
                                <asp:TextBox ID="txtYearOfExp" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                            </div>
                            </div>
                            <div class="form-group">
                            <label for="inputPassword3" class="col-sm-4 control-label">Resign Date</label>
                            <div class="col-sm-7 padding_right">
                                <asp:TextBox ID="txtResignDate" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender7" Format=d-M-yyyy runat="server" TargetControlID="txtResignDate"></asp:CalendarExtender>
                            </div>
                            </div>
                        </div>
                     </div>
                 </div>
                    
                     </div>
                 </div>
          </div>
                      </ContentTemplate>
                      </asp:UpdatePanel>
             <table class="em_button_table">
                        <tr>
                            <th>
                                 <asp:Button ID="btnPrevious" ClientIDMode="Static" class="emp_btn Ptbut" runat="server" Text="<<" ToolTip="Previous Page" OnClick="btnPrevious_Click"  />
                                
                            </th>
                            <th>
                                <asp:Button ID="btnSaveExperience" OnClientClick="return InputValidation();"  ClientIDMode="Static" class="emp_btn Ptbut" runat="server" Text="Save" OnClick="btnSaveExperience_Click" />
                            </th>
                           
                            <th><asp:Button ID="btnExperienceClose" ClientIDMode="Static"  class="emp_btn Ptbut" runat="server" Text="Close" OnClick="btnExperienceClose_Click"  /></th>
                               <th>
                                 <asp:Button ID="btnNext" ClientIDMode="Static" class="emp_btn Ptbut" runat="server" Text=">>"  ToolTip="Next Page" OnClick="btnNext_Click" />
                                
                            </th>
                        </tr>
                </table>
                 <div style="width:100%; margin-top:10px; text-align:center"><h1>Experience</h1></div>
                 <div id="divExperienceList" class="datatables_wrapper" style="width:100%; height:auto; max-height:500px;overflow:auto;overflow-x:hidden;"></div>
                    
            </div>
             </div>
         </div>
          </div>
        </div>
    </div>
    <script type="text/javascript">
        function editEmpExperience(id) {
            $('#txtCompanyName').val($('#r_' + id + ' td:first').html());
            $('#txtDesignation').val($('#r_' + id + ' td:nth-child(2)').html());
            $('#txtResponsibility').val($('#r_' + id + ' td:nth-child(3)').html());
            $('#txtYearOfExp').val($('#r_' + id + ' td:nth-child(4)').html());
            $('#txtJoiningDateExperience').val($('#r_' + id + ' td:nth-child(5)').html());
            $('#txtResignDate').val($('#r_' + id + ' td:nth-child(6)').html());
            $('#txtSpecialQualification').val($('#r_' + id + ' td:nth-child(7)').html());
            $('#btnSaveExperience').val('Update');
            $('#hdfexperience').val(id);
           
        }
        function InputValidation() {
            try {
                if ($('#txtCompanyName').val().trim().length == 0) {
                    showMessage("warning->Please Type Company Name !");
                    $('#txtCompanyName').focus();
                    return false;
                }
                if ($('#txtDesignation').val().trim().length == 0) {
                    showMessage("warning->Please Type Designation !");
                    $('#txtDesignation').focus();
                    return false;
                }
                if ($('#txtResponsibility').val().trim().length == 0) {
                    showMessage("warning->Please Type Responsibility !");
                    $('#txtResponsibility').focus();
                    return false;
                }
                if ($('#txtYearOfExp').val().trim().length == 0) {
                    showMessage("warning->Please Type Year of Experience !");
                    $('#txtYearOfExp').focus();
                    return false;
                }
                if ($('#txtJoiningDateExperience').val().trim().length == 0) {
                    showMessage("warning->Please Type Joining Date !");
                    $('#txtJoiningDateExperience').focus();
                    return false;
                }
                if ($('#txtResignDate').val().trim().length == 0) {
                    showMessage("warning->Please Type Resign Date !");
                    $('#txtResignDate').focus();
                    return false;
                }
                return true;
            }
            catch (exception) {

            }
        }
        function goToNewTabandWindowsClose(url) {
            window.open(url);
            window.close();
        }
    </script>
</asp:Content>
