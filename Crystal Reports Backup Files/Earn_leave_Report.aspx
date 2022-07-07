<%@ Page Title="Earn Leave Report" Language="C#" MasterPageFile="~/leave_nested.master" AutoEventWireup="true" CodeBehind="Earn_leave_Report.aspx.cs" Inherits="SigmaERP.leave.Earn_leave_Report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                       <li> <a href="#" class="ds_negevation_inactive">Earn Leave Report</a></li>
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
    <div class="main_box1">
    	<div class="main_box_header">
            <h2>Earn Leave Report</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">

                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" />
                        <asp:AsyncPostBackTrigger ControlID="chkForAllCompany" />
                        <asp:AsyncPostBackTrigger ControlID="ddlShiftName" />
                        <asp:AsyncPostBackTrigger ControlID="rblGenerateType" />
                        <asp:AsyncPostBackTrigger ControlID="ddlSelectMonth" />
                    </Triggers>
                    <ContentTemplate>
                <div class="bonus_generation">
                 <h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center">You Have Not Any Access Permission!</h1>
                     <table runat="server" visible="true" id="tblGenerateType" class="bonus_generation_table super_admin_option">                                                                      
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:CheckBox ID="chkForAllCompany" runat="server" Text="For All Companies" AutoPostBack="True" Visible="False" />
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:RadioButtonList ID="rblGenerateType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="True" OnSelectedIndexChanged="rblGenerateType_SelectedIndexChanged"    >
                                        <asp:ListItem Selected="True" Text="All" Value="0"></asp:ListItem>
                                        <asp:ListItem Selected="False" Text="Individual" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    </td>
                            </tr>
                                <tr id="trForCompanyList" runat="server">
                                <td>Company</td>
                                <td>
                                    <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static" CssClass="form-control select_width" Width="185px" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged"    >
                                    </asp:DropDownList>
                                </td>
                             <td>Shift</td>
                           <td>
                               <asp:DropDownList ID="ddlShiftName" runat="server" ClientIDMode="Static" CssClass="form-control select_width" Width="185px" AutoPostBack="True" OnSelectedIndexChanged="ddlShiftName_SelectedIndexChanged"  >
                               </asp:DropDownList>
                           </td>
                           </tr>
                     </table>                    
                </div>
                <div class="payroll_generation_box3">
                   <table>
                       <tr><td></td><td><asp:CheckBox runat="server" ID="ckAllMonth" Text="With All Month" ForeColor="Green" Font-Bold="true" Visible="false" /></td><td></td></tr>
                       <tr>
                          <td>Select Month &nbsp;</td>                      
                           <td>
                                <asp:DropDownList ID="ddlSelectMonth" runat="server" ClientIDMode="Static" CssClass="form-control select_width" Width="185px" AutoPostBack="True" OnSelectedIndexChanged="ddlSelectMonth_SelectedIndexChanged">
                               </asp:DropDownList>
                           </td>                  
                           <td>&nbsp;Card No &nbsp;</td>                           
                           <td>
                               <asp:TextBox ID="txtEmpCardNo" runat="server" Width="174" ClientIDMode="Static" PlaceHolder="For Certain" CssClass="form-control text_box_width_import" Enabled="False" ></asp:TextBox>                               
                           </td>
                       </tr>
                   </table>
                </div>
                        <asp:Panel id="pnl1" runat="server" >
                  <div class="job_card_box3" id="divDepartmentList" runat="server" >
                    <div class="job_card_left">
                        <asp:ListBox ID="lstAll" Width="270" SelectionMode="Multiple"  Height="146" runat="server"></asp:ListBox>
                    </div>
                                <div class="id_card_center">
                        <table class="arowBnt_table">                     
                              <tr>
                                    <td>
                                        <asp:Button ID="btnAddItem" Class="arrow_button" runat="server" Text=">" OnClick="btnAddItem_Click"    />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnAddAllItem" Class="arrow_button" runat="server" Text=">>" OnClick="btnAddAllItem_Click"   />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveItem" Class="arrow_button" runat="server" Text="<" OnClick="btnRemoveItem_Click"    />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnRemoveAllItem" Class="arrow_button" runat="server" Text="<<" OnClick="btnRemoveAllItem_Click"    />
                                    </td>
                               </tr>
                        </table>
                    </div>
                    <div class="job_card_right">
                        <asp:ListBox ID="lstSelected" Width="270" SelectionMode="Multiple" Height="146" runat="server"></asp:ListBox>
                    </div>
                </div>
                            </asp:Panel>
                <div class="payroll_generation_button">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold; width:139px; float:left">
                                            <asp:Label runat="server" ID="lblProcess" text="wait processing"></asp:Label>
                                        <img style="width:26px;height:24px;cursor:pointer; margin-right:-56px" src="/images/wait.gif"  />  
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                    <asp:Button ID="btnPreview" runat="server" CssClass="css_btn" Text="Preview" OnClick="btnPreview_Click"  />
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/payroll_default.aspx" CssClass="css_btn" />
                </div>
                
            </div>
                        </ContentTemplate>
                </asp:UpdatePanel>
        </div>
    </div>
     </div>

    <script type="text/javascript">
        function InputValidationBasket() {
            try {

                if ($('#txtEmpCardNo').val().trim().length < 4) {
                    showMessage('Please select To Date', 'error');
                    $('#txtToDate').focus(); return true;
                }
                return true;
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }

        function CloseWindowt() {
            window.close();
        }

        function goToNewTabandWindow(url) {
            window.open(url);

        }

        function getSalaryMonth() {

            var val = document.getElementById('ddlMonthID').value;
            document.getElementById('txtMonthId').value = val;

        }

        function CloseWindowt() {
            window.close();
        }

        function goToNewTabandWindow(url) {
            window.open(url);

        }



    </script>
</asp:Content>
