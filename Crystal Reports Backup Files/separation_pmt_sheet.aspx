<%@ Page Title="Separation Salary Sheet" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="separation_pmt_sheet.aspx.cs" Inherits="SigmaERP.payroll.separation_pmt_sheet" %>
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
                    <li><a href="#" class="ds_negevation_inactivez Pactive">Separation Salary Sheet</a></li>
                </ul>
            </div>
        </div>
    </div>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">       
        <ContentTemplate>

     <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Separation Salary Sheet</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">  
                 <div class="bonus_generation" style="width: 61%; margin: 0px auto;">               
                    <h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center"></h1>
                     <center> <asp:RadioButtonList ID="rbEmpList" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList></center>
                     <table runat="server" visible="true" id="tblGenerateType" class="division_table_leave1">
                         <tr>
                             <td>Company : 
                             </td>
                             <td>
                                 <asp:DropDownList ID="ddlCompanyList" runat="server" AutoPostBack="true" CssClass="form-control select_width" ClientIDMode="Static" ></asp:DropDownList>
                             </td>
                         </tr>
                         <tr>
                             <td>Month ID : 
                             </td>
                             <td>
                                 <asp:DropDownList ID="ddlMonthId" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 <b>Emp Status : </b>
                             </td>
                             <td>
                                 <asp:DropDownList ID="ddlSeperationType" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                             </td>

                         </tr>
                         <tr>
                             <td>
                                 <b>Language: </b>
                             </td>
                             <td>
                                 <asp:RadioButtonList ID="rbLanguage" runat="server" AutoPostBack="True" RepeatDirection="Horizontal">
                                     <asp:ListItem Value="0">Bangla</asp:ListItem>
                                     <asp:ListItem Value="1" Selected="True">English</asp:ListItem>
                                 </asp:RadioButtonList>
                             </td>
                         </tr>
                     </table>
                </div>
               <div id="divDepartmentList" runat="server" class="id_card" style="background-color: white; width: 61%;">
                     <div class="id_card_left EilistL">                        
                        <asp:ListBox ID="lstAllDepartment" CssClass="lstdata EilistCec" Style="height: 270px !important"  runat="server" SelectionMode="Multiple" AutoPostBack="True"></asp:ListBox>
                    </div>
                     <div class="id_card_center EilistC">
                                    <div style="margin-top: 60px;" class="employee_table">

                        <asp:Button ID="btnadditem" CssClass="arrow_button" runat="server" Text=">" />
                        <br />
                        <asp:Button ID="btnaddall" CssClass="arrow_button" runat="server" Text=">>"/>
                        <br />
                        <asp:Button ID="btnremoveitem" CssClass="arrow_button" runat="server" Text="<"/>
                        <br />
                        <asp:Button ID="btnremoveall" CssClass="arrow_button" runat="server" Text="<<"/>
                    </div>
                         </div>
                      <div class="id_card_right EilistR">                        
                        <asp:ListBox ID="lstSelectedDepartment"  CssClass="lstdata EilistCec" Style="height: 270px !important" runat="server" SelectionMode="Multiple"></asp:ListBox>

                    </div>
                    <br />

                    <div style="margin:5px; float:left; width: 232px;">
                          <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold;float:left; height: 48px;"> wait processing
                                        <img style="width:26px;height:26px;cursor:pointer; float:left" src="/images/wait.gif"  />
                                          
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                    </div>

                </div>
               <div class="job_card_button_area">
                    <asp:Button ID="btnPreview" CssClass="Pbutton"  ValidationGroup="save" runat="server" Text="Preview" OnClick="btnPreview_Click" />
                                
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="Pbutton" />
                </div>
            </div>
        </div>
    </div>
             </ContentTemplate>
    </asp:UpdatePanel>

        <script type="text/javascript">
           
            function CloseWindowt() {
                window.close();
            }

            function goToNewTabandWindow(url) {
                window.open(url);

            }

    </script>
</asp:Content>
