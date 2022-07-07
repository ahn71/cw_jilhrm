<%@ Page Title="Payroll Generation" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="payroll_generation.aspx.cs" Inherits="SigmaERP.payroll.payroll_generation" %>

<%@ Register Assembly="ComplexScriptingWebControl" Namespace="ComplexScriptingWebControl" TagPrefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
         $(document).ready(function () {
             $(".meter > span").each(function () {
                 $(this)
                     .data("origWidth", $(this).width())
                     .width(0)
                     .animate({
                         width: $(this).data("origWidth")
                     }, 1200);
             });
         });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" ></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
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
                       <li> <a href="#" class="ds_negevation_inactive Pactive">Payroll Generation</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>

    <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2 >Salary Generation</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">

                <asp:UpdatePanel ID="up2" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="rbGenaratingType" />
                            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                            <asp:AsyncPostBackTrigger ControlID="ddlShiftList" />
                            <asp:PostBackTrigger ControlID="btnGenerate" />
                            <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                           
                        </Triggers>
                        <ContentTemplate>
                           <div class="bonus_generation" style="width: 61%; margin: 0px auto;">   
                               <center>
                                    <asp:RadioButtonList ID="rbGenaratingType" runat="server" OnSelectedIndexChanged="rbGenaratingType_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static" RepeatDirection="Horizontal" Width="274px">
                                                <asp:ListItem Selected="True" Value="0">Full Generate</asp:ListItem>
                                                <asp:ListItem Value="1">Partial Generate</asp:ListItem>
                                            </asp:RadioButtonList>
                               </center>
                                <table runat="server" visible="true" id="tblGenerateType"  style="width:60%; margin:0px auto"> 
                                    <tr>                          
                                        <td>
                                            <asp:DropDownList Visible="false" ID="ddlShiftList" runat="server" AutoPostBack="true" CssClass="form-control select_width" ClientIDMode="Static" OnSelectedIndexChanged="ddlShiftList_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>

                                    </tr>
                                    <tr>
                                         <td>
                                            Company
                                        </td>
                                      
                                        <td  >
                                            <asp:DropDownList ID="ddlCompanyList" runat="server" AutoPostBack="true" CssClass="form-control select_width" ClientIDMode="Static" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ></asp:DropDownList>
                                        </td>
                                       
                                    </tr>
                                    <tr>
                                        <td>Wages Generate Month
                                        </td>
                                        <td>
                                            <asp:TextBox CssClass="form-control text_box_width"  ClientIDMode="Static" ID="txtGenerateMonth" runat="server"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtGenerateMonth_CalendarExtender" Format="dd-MM-yyyy" runat="server" TargetControlID="txtGenerateMonth">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtEmpCardNo" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" PLaceHolder="Type or Select Card No" Enabled="False" EnableTheming="True"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlEmpCardNo" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" onChange="getCardNo()"></asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
  
                            </div>
                            <br />
                            <div  style="width: 61%; margin: 0px auto; overflow: hidden">
                                <div style="width: 50%; margin: 0px auto; overflow: hidden">
                                    <asp:Button ID="btnGenerate" CssClass="Pbutton" ClientIDMode="Static" runat="server" Text="Generate" OnClientClick="return InputValidationBasket();" Style="float: left" OnClick="btnGenerate_Click" />
                                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/payroll_default.aspx" CssClass="Pbutton" Style="float: left" />
                                    <asp:Button ID="btnBDTNoteGenerate" runat="server" Text="BDT Note Generate" CssClass="Pbutton" Style="float: left; width: 166px;" OnClick="btnBDTNoteGenerate_Click" /><br />
                                </div>
                            </div>
                            <br />
                        <hr />
                            <br />
                     <div id="progressbar" style="width:100%;position:relative;"></div>
                     <asp:Label ID="lblM" runat="server" ClientIDMode="Static" Text="" style=" font-weight:bold; position: absolute; margin-top: -23px; margin-left: 22%;"  ></asp:Label>

                         <cc1:ProgressBar runat="server" ID="ProgressBar1" />
                     </ContentTemplate>
                    </asp:UpdatePanel>


                       


            </div>
        </div>
    </div>
            
                    
    <script type="text/javascript">


        $.updateProgressbar = function () {
            //Calling PageMethod for current progress
            PageMethods.OperationProgress(function (result) {
                //Updating progress
                $("#progressbar").progressbar('value', result.progress)
                var pval = ('value', result.progress);            
                $('#<%=lblM.ClientID%>').html(pval + "%");            
                //If operation is complete
                if (result.progress == 100) {
                    //Enable button
                    $("#operation").attr('disabled', '');
                }
                    //If not
                else {
                    //Reset timer
                    setTimeout($.updateProgressbar, 500);
                }
            });
        };

        $(document).ready(function () {
            //Progressbar initialization
            $("#progressbar").progressbar({ value: 0 });
            //Button click event
            $("#btnGenerate").click(function (e) {
                e.preventDefault();
                //Disabling button
                //$("#operation").attr('disabled', 'disabled');
                //Making sure that progress indicate 0
                $("#progressbar").progressbar('value', 0);

                var sdate = $('#txtGenerateMonth').val();                
                var companyId = $('#ddlCompanyList').val();               
                
                //  var checked_radio = $("[id*=rbGenaratingType] input:checked");
                var checked_radio = $("#rbGenaratingType input:checked");
                var gType = checked_radio.val();

                
                var cardNo = $('#txtEmpCardNo').val();
                var EmpId = $('#ddlEmpCardNo').val();
                if (!InputValidationBasket()) return;
               
                if (gType == 1) if (!InputValidationBasket2()) return;
                    
                       
                
               // return;
                //Call PageMethod which triggers long running operation
                PageMethods.Operation(sdate,companyId,gType,cardNo,EmpId,function (result) {
                    if (result) {
                        //Updating progress
                        $("#progressbar").progressbar('value', result.progress)
                        //Setting the timer
                        setTimeout($.updateProgressbar, 500);
                    }
                });
            });
        });



        function getCardNo() {

            var e = document.getElementById('ddlEmpCardNo');
            var text = e.options[e.selectedIndex].text;
            var splitValue=text.split(' ');

            document.getElementById('txtEmpCardNo').value = splitValue[0];

        }

        function InputValidationBasket() {
            try {
                if ($('#txtGenerateMonth').val().trim().length <=4 ) {
                    showMessage('Please select salary month', 'error');
                    $('#txtGenerateMonth').focus(); return false;
                }
                return true;
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }

        function InputValidationBasket2() {
            try {              
                if ($('#ddlEmpCardNo').val().trim().length <= 4) {
                    showMessage('Please select an emp card no', 'error');
                    $('#ddlEmpCardNo').focus(); return false;
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
    </script>
  
</asp:Content>
