<%@ Page Title="Separation Generation" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="separation_generation.aspx.cs" Inherits="SigmaERP.payroll.separation_generation" %>

<%@ Register Assembly="ComplexScriptingWebControl" Namespace="ComplexScriptingWebControl" TagPrefix="cc1" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                       <li> <a href="#" class="ds_negevation_inactive Pactive">Separation Generation</a></li>
                   </ul>               
             </div>          
             </div>
       </div>
      <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Separation Generation</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">
                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="rblGenerateType" />
                        <asp:AsyncPostBackTrigger ControlID="btnGeneration" />
                        <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
                        <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
                        <asp:AsyncPostBackTrigger ControlID="ddlMonthID" />
                    </Triggers>
                    <ContentTemplate>
                          <div class="bonus_generation" style="width: 61%; margin: 0px auto;">     
                           <table runat="server" visible="true" id="tblGenerateType" class="division_table_leave1"> 
                                <tr>
                                      <td>
                                          Company
                                      </td>
                                        <td collspan="2">
                                            <asp:DropDownList ID="ddlCompanyList" runat="server" AutoPostBack="true" CssClass="form-control select_width" ClientIDMode="Static" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ></asp:DropDownList>
                                        </td>                         
                                    </tr>
                                <tr>
                                    <td>Select Month </td>                                       
                                    <td>
                                        <asp:DropDownList ID="ddlMonthID" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMonthID_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>Type</td>
                                    <td>
                                    <asp:RadioButtonList ID="rblGenerateType" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal" RepeatLayout="Flow" Onchange="radio_button_indaxChanged();" >
                                        <asp:ListItem Selected="True" Text="Full" Value="0"></asp:ListItem>
                                        <asp:ListItem Selected="False" Text="Partial" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Card No
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEmpCardNo" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" Enabled="False" onChange="ddlEmpCardNo_ChangedItem();" ></asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:TextBox ID="txtEmpCardNo" ClientIDMode="Static" runat="server" CssClass="form-control text_box_width" PLaceHolder="Type or Select Card No" Enabled="False" EnableTheming="True"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                        <td>
                                           
                                        </td>
                                        <td>
                                            <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loading.gif" ClientIDMode="Static"  />
                                        </td>
                                    </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="payroll_generation_button">                  
                    <asp:Button ID="btnGeneration" CssClass="Pbutton" runat="server"  OnClick="btnGeneration_Click"  Text="Generation" />
               <%--      <input  type="button" class="Pbutton" id="generate" value="Generation" Visible="false"/>--%>
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/payroll_default.aspx" CssClass="Pbutton" />                   
                 </div>
                <div id="progressbar" style="width:100%;position:relative;"></div>
                <asp:Label ID="lblM" runat="server" ClientIDMode="Static" Text="" style=" font-weight:bold; position: absolute; margin-top: -23px; margin-left: 22%;"  ></asp:Label>
                <p ID="lblErrorMessage" style="color:red;display:none">iiii</p>
            </div>
        </div>
    </div>
      <script type="text/javascript">
          $('#imgLoading').hide();
          $.updateProgressbar = function () {
              //Calling PageMethod for current progress
              PageMethods.OperationProgress(function (result) {
                  //Updating progress
                  var pval = ('value', result.progress);
                  if (pval != 200) {
                      $("#progressbar").progressbar('value', result.progress)
                      //var pval = ('value', result.progress);
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
                  }
                  else {
                      $('#<%=lblM.ClientID%>').css('color','red');
                      $('#<%=lblM.ClientID%>').html("sorry any record not founded");
                  }
             
            });
        };

          $(document).ready(function () {
              load();
            
            //Progressbar initialization
            $("#progressbar").progressbar({ value: 0 });
            //Button click event
            $("#generate").click(function (e) {

                e.preventDefault();

                //Disabling button
                //$("#operation").attr('disabled', 'disabled');
                //Making sure that progress indicate 0
                $("#progressbar").progressbar('value', 0);


                var companyId = $('#ddlCompanyList').val();
                var smonth = $('#ddlMonthID').val();
                var sradio = $('#rblGenerateType input:checked');                
                var gtype = sradio.val();
                var EmpCarNo = $('#txtEmpCardNo').val();
                var EmpId = $('#ddlEmpCardNo').val();
                if (gtype == 0) { if (!InputValidationBasket()) return; }            
                else { if (!InputValidationBasket2()) return; }
                // return;
                //Call PageMethod which triggers long running operation
                PageMethods.Operation(smonth, companyId,gtype,EmpCarNo,EmpId, function (result) {
                    if (result) {
                        //Updating progress
                        $("#progressbar").progressbar('value', result.progress)
                        //Setting the timer
                        setTimeout($.updateProgressbar, 500);
                    }
                });
            });
          });
          function load() {
              $("#ddlEmpCardNo").select2();              
          }


        function InputValidationBasket() {
            try {
                if ($('#ddlMonthID').val().trim().length <= 1) {
                    showMessage('Please select seperation generation month', 'error');
                    $('#ddlMonthID').focus(); return false;
                }
                return true;
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }
        function InputValidationBasket2() {
            try {
                if ($('#ddlMonthID').val().trim().length <= 1) {
                    showMessage('Please select seperation generation month', 'error');
                    $('#ddlMonthID').focus(); return false;
                }
                else if ($('#txtEmpCardNo').val().trim().length < 4)
                {
                    showMessage('Please select or type valid card no', 'error');
                    $('#txtEmpCardNo').focus(); return false;
                }
                return true;
            }
            catch (exception) {
                showMessage(exception, error)
            }
        }
        function radio_button_indaxChanged() {
          
            var sradio = $('#rblGenerateType input:checked');
            if (sradio.val() == "0") {
                $('#ddlEmpCardNo').attr('disabled', true);
                $('#txtEmpCardNo').attr('disabled',true);
            }
            else {
                $('#ddlEmpCardNo').attr('disabled', false);
                $('#txtEmpCardNo').attr('disabled',false);
            }
            $('#imgLoading').hide();
        }

        function ddlEmpCardNo_ChangedItem()
        {
            try
            {
                var getEmpCardNo = $('#ddlEmpCardNo option:selected').text().split(' ');
                $('#txtEmpCardNo').val(getEmpCardNo[0]);
            }
            catch (exception)
            { }
        }
        function processing() {
            var checked_radio = $("[id*=rbGenaratingType] input:checked");

            if (checked_radio.val() == 0) {
                if (validation() == false) return false;
            }
            else {
                if (indivisualvalidation() == false) return false;
            }
            $('#imgLoading').show();
            return true;
        }

        function alertMessage() {
           
            setTimeout(function () {
                $('#lblErrorMessage').css('display', 'block');
                $('#lblErrorMessage').fadeOut("slow", function () {
                    $('#lblErrorMessage').remove();
                    $('#lblErrorMessage').val('');
                });

            }, 10000);
        }
        function ProcessingEnd(total) {
            showMessage("success->Successfully separation generated of " + total + "");
            $('#imgLoading').hide();
        }
        function ProcessingEror(total) {
            showMessage("error->" + total + "");
            $('#imgLoading').hide();
        }


    </script>

</asp:Content>
