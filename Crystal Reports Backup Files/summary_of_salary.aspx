<%@ Page Title="Summary Of Salary" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="summary_of_salary.aspx.cs" Inherits="SigmaERP.payroll.summary_of_salary" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    <li><a href="#" class="ds_negevation_inactivez Pactive">Summary Of Salary</a></li>
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
  <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Summary of Salary</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">
                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" />
                        <asp:AsyncPostBackTrigger ControlID="chkForAllCompany" />
                        <asp:AsyncPostBackTrigger ControlID="ddlShiftName" />
                        <asp:AsyncPostBackTrigger ControlID="btnPreview" />
                    </Triggers>
                    <ContentTemplate>
               <div class="bonus_generation" style="width: 61%; margin: 0px auto;">               
                    <h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center"></h1>
                    <table runat="server" visible="true" id="tblGenerateType" class="division_table_leave1">                                                                       
                             <tr>                                    
                                    <td>
                                        <asp:CheckBox ID="chkForAllCompany" runat="server" Text="For All Companies" AutoPostBack="True" Visible="False" />
                                    </td>                         
                                   <td></td>
                                    <td></td>
                                    <td>
                                        <asp:RadioButtonList ID="rblSheet" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"   >
                                        <asp:ListItem Selected="True" Text="Regular" Value="0"></asp:ListItem>
                                        <asp:ListItem Selected="False" Text="Separation" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    </td>
                            </tr>
                                <tr id="trForCompanyList" runat="server">
                                <td>Company</td>
                                <td>
                                    <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static" CssClass="form-control select_width" style="max-width:80%"  AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged"   >
                                    </asp:DropDownList>
                                </td>
                           <td>Month</td>
                             <td><asp:DropDownList ID="ddlMonth" runat="server"  ClientIDMode="Static" CssClass="form-control select_width"  AutoPostBack="false" ></asp:DropDownList></td>
                           </tr>
                         <tr style="display:none" >
                             
                               <td runat="server" id="tdSftTxt">Shift</td>
                           <td runat="server" id="tdSft">
                               <asp:DropDownList ID="ddlShiftName" runat="server" ClientIDMode="Static" CssClass="form-control select_width" AutoPostBack="True" OnSelectedIndexChanged="ddlShiftName_SelectedIndexChanged"  >
                               </asp:DropDownList>
                           </td>
                         </tr>
                        <tr runat="server" id="trHideForIndividual" >
                                <td>Employee Type</td>                                
                                <td>                                   
                                    <asp:RadioButtonList ID="rblEmployeeType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"   >
                                        
                                    </asp:RadioButtonList>
                                </td>
                             <td>Payment Type</td>                                
                                <td>                                   
                                    <asp:RadioButtonList ID="rblPaymentType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"   >
                                                        <asp:ListItem Value="Cash" Selected="True">Cash</asp:ListItem>
                                                        <asp:ListItem Value="Bank">Bank</asp:ListItem>
                                                        <asp:ListItem Value="Check">Check</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                     </table>
                   
                   <asp:RadioButtonList  ID="rblReportType" Visible="false" runat="server" RepeatDirection="Horizontal">
                       <asp:ListItem Value="0" Selected="True">By Section</asp:ListItem>
                        <asp:ListItem Value="1" >By Department</asp:ListItem>
                   </asp:RadioButtonList>

                    
                </div>

                        <div id="divDepartmentList" runat="server" class="id_card" style="background-color: white; width: 61%;">
                            <asp:Panel ID="pnl1" runat="server">
                                <div class="id_card_left EilistL">
                                    <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata EilistCec" Style="height: 270px !important" SelectionMode="Multiple"></asp:ListBox>
                                </div>
                                <div class="id_card_center EilistC">
                                    <table style="margin-top: 60px;" class="employee_table">
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAddItem" Class="arrow_button" runat="server" Text=">" OnClick="btnAddItem_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnAddAllItem" Class="arrow_button" runat="server" Text=">>" OnClick="btnAddAllItem_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnRemoveItem" Class="arrow_button" runat="server" Text="<" OnClick="btnRemoveItem_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btnRemoveAllItem" Class="arrow_button" runat="server" Text="<<" OnClick="btnRemoveAllItem_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="id_card_right EilistR">
                                    <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata EilistCec" Style="height: 270px !important" ClientIDMode="Static" runat="server"></asp:ListBox>
                                </div>                      
                        </asp:Panel>
                </div>
                <div class="payroll_generation_button">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold; width:139px; float:left">
                                            <asp:Label runat="server" ID="lblProcess" text="wait processing"></asp:Label>
                                        <img style="width:26px;height:24px;cursor:pointer; margin-right:-56px" src="/images/wait.gif"  />  
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                    <asp:Button ID="btnPreview" runat="server" CssClass="Pbutton" Text="Preview" OnClientClick="return InputValidationBasket();" OnClick="btnPreview_Click"  />
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/payroll_default.aspx" CssClass="Pbutton" />
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
                if ($('#txtGenerateMonth').val().trim().length <= 4) {
                    showMessage('Please select salary month', 'error');
                    $('#txtGenerateMonth').focus(); return false;
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
