<%@ Page Title="Current Salary Structure" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="CurrentSalaryStructure.aspx.cs" Inherits="SigmaERP.payroll.CurrentSalaryStructure" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li><a class="seperator" href="#"></a>/</li>
                    <li><a href="/payroll_default.aspx">Payroll</a></li>
                    <li><a class="seperator" href="#"></a>/</li>
                    <li><a href="/payroll/salary_index.aspx">Salary</a></li>
                    <li><a class="seperator" href="#"></a>/</li>
                    <li><a href="#" class="ds_negevation_inactivez Pactive">Current Salary Structure</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:HiddenField ID="upSuperAdmin" runat="server" ClientIDMode="Static" />
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
          <Triggers>
              <asp:AsyncPostBackTrigger ControlID="rdball" />
              <asp:AsyncPostBackTrigger ControlID="rdbindividual" />
              <asp:AsyncPostBackTrigger ControlID="rdbWorker" />
              <asp:AsyncPostBackTrigger ControlID="rdbStaff" />
              <asp:AsyncPostBackTrigger ControlID="rdbEmpType" />
              <asp:AsyncPostBackTrigger ControlID="rdbTypeWorker" />
              <asp:AsyncPostBackTrigger ControlID="rdbTypeStaff" />

          </Triggers>
          <ContentTemplate>
 <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Current Salary Structure</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">

                <div class="punishment_against">
                    <fieldset>
                    <legend>
                        <b>Option</b>
                    </legend>
                  <table class="employee_table">                 
                        <tbody>
                      <tr>
                        <td width="27%">
                            <asp:RadioButton ID="rdball" Class=""  ClientIDMode="Static" AutoPostBack="true" Checked="true" runat="server" Text="All" OnCheckedChanged="rdball_CheckedChanged"  />
                        </td>
                          <td>
                            <asp:RadioButton ID="rdbEmpType" ClientIDMode="Static" runat="server" Text="Employee Type" AutoPostBack="True" OnCheckedChanged="rdbEmpType_CheckedChanged"  />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdbindividual" ClientIDMode="Static" AutoPostBack="true" runat="server" Text="Individual" OnCheckedChanged="rdbindividual_CheckedChanged"  />
                        </td>
                   </tr>       
                      
                    </tbody>
                  </table>
                  </fieldset>
                </div>
                <div id="divEmptype" class="punishment_against" runat="server">
                    <table class="employee_table">
                        <tr runat="server" id="tr1">
                        <td width="27%">
                            <asp:RadioButton ID="rdbTypeWorker" ClientIDMode="Static" Class="" Checked="true" runat="server" Text="Worker"  AutoPostBack="True" OnCheckedChanged="rdbTypeWorker_CheckedChanged"  />
                        </td>
                            
                        <td>
                            <asp:RadioButton ID="rdbTypeStaff" ClientIDMode="Static" runat="server" Text="Staff" AutoPostBack="True" OnCheckedChanged="rdbTypeStaff_CheckedChanged"  />
                        </td>
                   </tr> 
                      
                   </table>
                </div>
                <div  id="divindivisual" class="punishment_against" runat="server">
                   <table class="employee_table">
                        <tr runat="server" id="trEmpType">
                        <td width="27%">
                            <asp:RadioButton ID="rdbWorker" ClientIDMode="Static" Class="" Checked="true" runat="server" Text="Worker"  AutoPostBack="True" OnCheckedChanged="rdbWorker_CheckedChanged" />
                        </td>
                            
                        <td>
                            <asp:RadioButton ID="rdbStaff" ClientIDMode="Static" runat="server" Text="Staff" AutoPostBack="True" OnCheckedChanged="rdbStaff_CheckedChanged"  />
                        </td>
                   </tr> 
                       <tr runat="server" id="trCardNo">
                       <td>
                         Individual Card
                        </td>
                         <td>
                           <asp:DropDownList ID="ddlCardNo" ClientIDMode="Static" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                        </td>
                           </tr>
                   </table>
                </div>
                <div class="punishment_button_area">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                <th><asp:Button ID="btnPrintpreview" runat="server" CssClass="Pbutton" ClientIDMode="Static" Text="Preview" OnClick="btnPrintpreview_Click" /></th>
                                <th><asp:Button ID="btnClose" PostBackUrl="~/payroll_default.aspx" Text="Close" runat="server" CssClass="Pbutton" /></th>   
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
            $("#ddlCardNo").select2();

          
        });

        function load() {
            $("#ddlCardNo").select2();
           
        }
        function goToNewTabandWindow(url) {
            window.open(url);
            load();
        }

    </script>
</asp:Content>
