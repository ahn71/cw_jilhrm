<%@ Page Title="Tax Generation" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="vat_tax_generation.aspx.cs" Inherits="SigmaERP.vat_tax.vat_tax_generation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="false" ></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
                  <div class="col-md-12 ds_nagevation_bar">
               <div style="margin-top: 5px">
                   <ul>
                    <li><a href="/default.aspx">Dasboard</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="/payroll_default.aspx">Payroll</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li>  <a href="/vat_tax/vat_tax_index.aspx">Vat&Tax</a></li>
                    <li><a class="seperator" href="#">/</a></li>                       
                    <li> <a href="#" class="ds_negevation_inactive Pactive">Tax Generation</a></li>
                   </ul>               
             </div>          
             </div>
       </div>
    <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2 >Tax Generation</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">
                <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                        <Triggers>                           
                            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />                           
                            <asp:AsyncPostBackTrigger ControlID="btnGenerate" />
                            <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />                           
                        </Triggers>
                        <ContentTemplate>
                           <div class="bonus_generation" style="width: 66%; margin: 0px auto;">   
                              <center>
                                 <asp:RadioButtonList ID="rbGenaratingType" runat="server"  AutoPostBack="false" ClientIDMode="Static" onChange="EnableControl(this);" RepeatDirection="Horizontal" Width="274px">
                                                <asp:ListItem Selected="True" Value="0">Full Generate</asp:ListItem>
                                                <asp:ListItem Value="1">Partial Generate</asp:ListItem>
                                            </asp:RadioButtonList>
                               </center>
                                <table runat="server" visible="true" id="tblGenerateType"  style="width:60%; margin:0px auto">                                    
                                    <tr>
                                         <td>
                                            Company<span class="requerd1">*</span>
                                        </td>                                      
                                        <td>
                                            <asp:DropDownList ID="ddlCompanyList" runat="server" AutoPostBack="True"  CssClass="form-control select_width" ClientIDMode="Static" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged"  ></asp:DropDownList>
                                        </td>                                       
                                    </tr>   
                                     <tr>
                                         <td>
                                             Tax Years<span class="requerd1">*</span>
                                        </td>                                       
                                        <td>
                                            <asp:DropDownList ID="ddlTaxYears" runat="server"  ClientIDMode="Static"  CssClass="form-control select_width"></asp:DropDownList>
                                        </td>
                                    </tr>                              
                                    <tr>
                                         <td>
                                             Employee<span class="requerd1">*</span>
                                        </td>                                       
                                        <td>
                                            <asp:DropDownList ID="ddlEmpCardNo" runat="server" Enabled="false" ClientIDMode="Static"   CssClass="form-control select_width" onChange="getCardNo()"></asp:DropDownList>
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
                            <br />
                            <div  style="width: 61%; margin: 0px auto; overflow: hidden">
                                <div style="width: 50%; margin: 0px auto; overflow: hidden">
                                    <asp:Button ID="btnGenerate" CssClass="Pbutton" ClientIDMode="Static" runat="server" Text="Generate" OnClientClick="return processing();" Style="float: left" OnClick="btnGenerate_Click"  />                                    
                                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/payroll_default.aspx" CssClass="Pbutton" Style="float: left" />                                   
                                </div>
                            </div>
                            <br />
                        <hr />
                            <br />
                     <div id="progressbar" style="width:100%;position:relative;"></div>
                     <asp:Label ID="lblM" runat="server" ClientIDMode="Static" Text="" style=" font-weight:bold; position: absolute; margin-top: -23px; margin-left: 22%;"  ></asp:Label>

                        <%-- <cc1:ProgressBar runat="server" ID="ProgressBar1" />--%>
                     </ContentTemplate>
                    </asp:UpdatePanel>                


            </div>
        </div>
    </div>
   <script type="text/javascript">
       $(document).ready(function () {
           $(document).on("keypress", "body", function (e) {
               if (e.keyCode == 13) e.preventDefault();
               // alert('deafault prevented');
           });
           $("#ddlEmpCardNo").select2();
       });
       function load() {
           $("#ddlEmpCardNo").select2();
           $('#imgLoading').hide();
       }
       $('#imgLoading').hide();
       function getCardNo() {

           var e = document.getElementById('ddlEmpCardNo');
           var text = e.options[e.selectedIndex].text;
           var splitValue = text.split(' ');
           document.getElementById('txtEmpCardNo').value = splitValue[0];
       }
       function indivisualvalidation() {
           if (validation() == false) return false;
           if ($('#ddlEmpCardNo option:selected').text().length == 0) {
               showMessage("warning->Please Select Card No ");
               $('#ddlEmpCardNo').focus();
               return false;
           }
           return true;
       }
       function validation() {
           if ($('#txtGenerateMonth').val().trim().length == 0) {
               showMessage("warning->Please Select Date ");
               $('#txtGenerateMonth').focus();
               return false;
           }
       }
       function EnableControl(e) {
           var checked_radio = $("[id*=rbGenaratingType] input:checked");

           if (checked_radio.val() == 0) {
               $("#txtEmpCardNo").prop("disabled", true);
               $("#ddlEmpCardNo").prop("disabled", true);

           }
           else {
               $("#txtEmpCardNo").prop("disabled", false);
               $("#ddlEmpCardNo").prop("disabled", false);

           }
       }
       function processing() {
          
           $('#imgLoading').show();
           return true;
       }
       function ProcessingEnd(total) {
           showMessage("success->Successfully vat tax generated of " + total + "");
           $('#imgLoading').hide();
           load();
       }
       function ProcessingEror(total) {
           showMessage("error->" + total + "");
           $('#imgLoading').hide();
           load();
       }
       function goToNewTabandWindow(url) {
           window.open(url);
           ProcessingEnd();
       }
</script>
</asp:Content>
