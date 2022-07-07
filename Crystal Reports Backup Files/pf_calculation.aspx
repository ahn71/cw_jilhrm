<%@ Page Title="PF Calculation" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="pf_calculation.aspx.cs" Inherits="SigmaERP.pf.pf_calculation" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dasboard</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="/payroll_default.aspx">Payroll</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li>  <a href="/pf/pf_index.aspx">Provident Fund</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                     <li> <a href="#" class="ds_negevation_inactive Pactive">PF Calculation</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate>
        <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
    </ContentTemplate>
    </asp:UpdatePanel>   
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >        
        <ContentTemplate>          
        
   
   <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>PF Calculation</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">                          
                <div class="em_personal_info" id="divEmpPersonnelInfo" style="margin:0px;overflow: initial;">
                    
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <Triggers>
                           <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" /> 
                           
                        </Triggers>
                        <ContentTemplate>
                             <div class="input_division_info" style="width:480px;">
                   <div class="row">
                        <div class="col-sm-12">
                              <div class="row">
                                <div class="col-sm-3">Company Name<span class="requerd1">*</span></div>
                                <div class="col-sm-9">
                                     <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" AutoPostBack="true" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged" >
                                                                        
                                      </asp:DropDownList>
                                </div>
                              </div>
                            <div class="row">
                                <div class="col-sm-3">Employee<span class="requerd1">*</span></div>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlEmpCardNo" runat="server"  ClientIDMode="Static" CssClass="form-control select_width"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                  <div class="col-sm-3"> From Month <span class="requerd1">*</span></div>
                                  <div class="col-sm-3">
                                      <asp:TextBox ID="txtFrommonth" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:CalendarExtender ID="txtFrommonth_CalendarExtender" runat="server" Format="MMM-yyyy" TargetControlID="txtFrommonth">
                                   </asp:CalendarExtender>
                             
                              </div>
                                 <div class="col-sm-3"> <p style="text-align:right">To Month<span class="requerd1">*</span></p> </div>
                                  <div class="col-sm-3">

                             <asp:TextBox ID="txttomonth" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width"></asp:TextBox>
                                <asp:CalendarExtender ID="txttomonth_CalendarExtender" runat="server" Format="MMM-yyyy" TargetControlID="txttomonth">
                                   </asp:CalendarExtender>
                             
                               </div>                                
                              </div>
                              <div class="row">
                                  <div class="col-sm-3"></div>
                                  <div class="col-sm-9">
                                      <div class="button_area Rbutton_area">
                                        <asp:Button ID="btnpfcalculation" CssClass="Pbutton" ClientIDMode="Static" runat="server" Text="PF Calculation" OnClientClick="return InputValidationBasket();" Style="float: left" OnClick="btnpfcalculation_Click" />
                                        <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="/pf/pf_index.aspx" CssClass="Pbutton" Style="float: left" />
                                   
                                    </div>

                                  </div>
                              </div>
                          
                        </div>
                    </div>
                                  
                    
                </div>
                            

                        </ContentTemplate>
                    </asp:UpdatePanel>
                   

                </div>
            </div>
        </div>
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    <script type="text/javascript">    
        $(document).ready(function () {
        
            $("#ddlEmpCardNo").select2();
        });
        function load() {
            $("#ddlEmpCardNo").select2();
            $('#imgLoading').hide();
        }
         
                function SaveSuccess() {
                    showMessage("Successfully saved", "success");
                }
                function UnableSave() {
                    $("#ddlEmpCardNo").select2();
                    showMessage("Unable to save", "error");
                }
                function UpdateSuccess() {
                    showMessage("Successfully Updated", "success");
                }
                function UnableUpdate() {
                    showMessage("Unable to Update", "error");
                }


               
        </script>
</asp:Content>
