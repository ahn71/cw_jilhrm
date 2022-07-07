<%@ Page Title="Employee Education" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="EmployeeEducation.aspx.cs" Inherits="SigmaERP.personnel.EmployeeEducation" %>
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
     <asp:HiddenField ID="hdfeducation" runat="server" ClientIDMode="Static"  />
<div  style="padding:0;margin-top:25px;">
    <div class="row">
   <div class="col-lg-12">
         <div class="employee_box_header PtBoxheader">
         <h2>Education Panel</h2>
     </div>
     <div class="employee_box_body Ptbody">
         
         <div class="employee_box_content">
    <div class="personal_employee_education_main" runat="server" id="divEmpEducation">
                 <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                 <Triggers>
                    
                 </Triggers>
                 <ContentTemplate>
                   <div class="row">
                     <div class="col-lg-1"></div>
                     <div class="col-lg-10">
             <div class="personal_employee_education">
                 <div class="row">
                     <div class="col-lg-1"></div>
                     <div class="col-lg-5">
                            <div class="form-horizontal form_horizontal_custom">
                            <div class="form-group">
                            <label for="inputEmail3" class="col-sm-4 control-label">Degree</label>
                            <div class="col-sm-7 padding_right">
                                <asp:TextBox ID="txtDegree" CssClass="form-control"  ClientIDMode="Static" runat="server"></asp:TextBox>
                            </div>
                            </div>
                            <div class="form-group">
                            <label for="inputPassword3" class="col-sm-4 control-label">Institute</label>
                            <div class="col-sm-7 padding_right">
                                <asp:TextBox ID="txtInstitute" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                            </div>
                            </div>
                        </div>
                     </div>
                     <div class="col-lg-5">
                          <div class="form-horizontal form_horizontal_custom">
                            <div class="form-group">
                            <label for="inputEmail3" class="col-sm-4 control-label">Year</label>
                            <div class="col-sm-7 padding_right">
                                <asp:TextBox ID="txtYear" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                            </div>
                            </div>
                            <div class="form-group">
                            <label for="inputPassword3" class="col-sm-4 control-label">Result</label>
                            <div class="col-sm-7 padding_right">
                                <asp:TextBox ID="txtResult" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                            </div>
                            </div>
                        </div>
                     </div>
                     <div class="col-lg-1"></div>
                 </div>
              
          </div>
        </div>
        <div class="col-lg-1"></div>
        </div>
                     </ContentTemplate>
                      </asp:UpdatePanel>
              <table class="em_button_table">
                        <tr>
                             <th>
                                 <asp:Button ID="btnPrevious" ClientIDMode="Static" class="emp_btn Ptbut" runat="server" Text="<<" ToolTip="Previous Page" OnClick="btnPrevious_Click"  />
                                
                            </th>
                            <th>
                                <asp:Button ID="btnSaveEducation"  ClientIDMode="Static" class="emp_btn Ptbut" runat="server" Text="Save" OnClientClick="return InputValidation();" OnClick="btnSaveEducation_Click" />
                            </th>
                           <th><asp:Button ID="btnCloseEmpEducation" ClientIDMode="Static"  class="emp_btn Ptbut" runat="server" Text="Close" OnClick="btnCloseEmpEducation_Click"  /></th>
                            
                            
                        </tr>
              </table>
                  <div style="width:100%; margin-top:10px; text-align:center"><h1>Education</h1></div>
                 <div id="divEducationList" class="datatables_wrapper" style="width:100%; height:auto; max-height:500px;overflow:auto;overflow-x:hidden;"></div>
                     
         </div>
             
             </div>
             </div>
        </div>
        </div>
        </div>
    <script type="text/javascript">
        function editEducation(id) {
            $('#txtDegree').val($('#r_' + id + ' td:first').html());
            $('#txtYear').val($('#r_' + id + ' td:nth-child(2)').html());
            $('#txtInstitute').val($('#r_' + id + ' td:nth-child(3)').html());
            $('#txtResult').val($('#r_' + id + ' td:nth-child(4)').html());
            $('#btnSaveEducation').val('Update');
            $('#hdfeducation').val(id);
        }
        function InputValidation() {
            try {
                if ($('#txtDegree').val().trim().length == 0) {
                    showMessage("warning->Please Type Degree Name !");
                    $('#txtDegree').focus();
                    return false;
                }
                if ($('#txtInstitute').val().trim().length == 0) {
                    showMessage("warning->Please Type Institute Name !");
                    $('#txtInstitute').focus();
                    return false;
                }
                if ($('#txtYear').val().trim().length == 0) {
                    showMessage("warning->Please Type Passing Year ! ");
                    $('#txtYear').focus();
                    return false;
                }
                return true;
            }
            catch (exception)
            {

            }
        }
    </script>
</asp:Content>
