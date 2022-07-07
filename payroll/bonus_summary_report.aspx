<%@ Page Title="Bonus Summary" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="bonus_summary_report.aspx.cs" Inherits="SigmaERP.payroll.bonus_summary_report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dasboard</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="/payroll_default.aspx">Payroll</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li> <a href="/payroll/bonus_index.aspx" >Bouns</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="#" class="ds_negevation_inactive Pactive">Bonus Summary Report</a></li>
                </ul>
            </div>
        </div>
    </div>   
     <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
  <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Bonus Summary Report</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">
                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" />
                        <asp:AsyncPostBackTrigger ControlID="chkForAllCompany" />
                        <asp:AsyncPostBackTrigger ControlID="ddlShiftName" />
                         <asp:AsyncPostBackTrigger ControlID="ddlBonusType" />
                        <asp:AsyncPostBackTrigger ControlID="btnPreview" />
                    </Triggers>
                    <ContentTemplate>
                <div class="bonus_generation" style="width: 61%; margin: 0px auto;">   
                   <h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center">You Have Not Any Access Permission!</h1>
                    <table runat="server" visible="true" id="tblGenerateType" class="division_table_leave1">                                                              
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:CheckBox visible="false" ID="chkForAllCompany" runat="server" Text="For All Companies" />
                                    </td>
                            </tr>
                                <tr id="trForCompanyList" runat="server">
                                <td>Company</td>
                                <td>
                                    <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static" CssClass="form-control select_width"  AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged"  >
                                    </asp:DropDownList>
                                </td>
                                    <td>Bonus Type</td>
                             <td><asp:DropDownList ID="ddlBonusType" runat="server"  ClientIDMode="Static" CssClass="form-control select_width"  AutoPostBack="True" ></asp:DropDownList></td>
                           
                           </tr>
                         <tr style="display:none">  
                             
                             <td runat="server" id="tdSftTxt">Shift</td>
                           <td runat="server" id="tdSft">
                               <asp:DropDownList ID="ddlShiftName" runat="server" ClientIDMode="Static" CssClass="form-control select_width"  AutoPostBack="True" OnSelectedIndexChanged="ddlShiftName_SelectedIndexChanged" >
                               </asp:DropDownList>
                           </td>
                         </tr>
                     </table>
                    
                </div>
                        <asp:Panel ID="pnl1" runat="server">
                            <div id="divDepartmentList" runat="server" class="id_card" style="background-color: white; width: 61%;">
                                <div class="id_card_left EilistL">
                                    <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata EilistCec" style="height:270px !important" SelectionMode="Multiple"></asp:ListBox>
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
                                    <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata EilistCec" style="height:270px !important" ClientIDMode="Static" runat="server"></asp:ListBox>
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
