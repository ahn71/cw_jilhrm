<%@ Page Title="Monthly Salary Flow" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="monthly_salary_flow.aspx.cs" Inherits="SigmaERP.payroll.monthly_salary_flow" %>
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
                    <li><a href="#" class="ds_negevation_inactivez Pactive">Monthly Salary Flow</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>

        </Triggers>
        <ContentTemplate>
      <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Monthly Salary Flow</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">
                
              <div class="bonus_generation" style="width: 55%; margin: 0px auto;">               
                    <h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center"></h1>
                    <table runat="server" visible="true" id="tblGenerateType" class="division_table_leave1">  
                        <tr>
                            <td>
                                Company Name 
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCompanyList" ClientIDMode="Static" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Year 
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlYear" ClientIDMode="Static" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
             
                    </table>
                </div>

                <div class="job_card_button_area">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold; width:139px; float:left">
                                            <asp:Label runat="server" ID="lblProcess" text="wait processing"></asp:Label>
                                        <img style="width:26px;height:24px;cursor:pointer; margin-right:-56px" src="/images/wait.gif"  />  
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                    <asp:Button ID="btnPreview" CssClass="Pbutton" runat="server" Text="Preview" OnClick="btnPreview_Click"   /> &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="Pbutton" />
                </div>
            </div>
        </div>
    </div>            
            </span>            
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/ecmascript">

        function CloseWindowt() {
            window.close();
        }

        function goToNewTabandWindow(url) {
            window.open(url);

        }
    </script>
</asp:Content>
