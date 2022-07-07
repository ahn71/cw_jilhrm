<%@ Page Title="ID Card" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="staff_id_card.aspx.cs" Inherits="SigmaERP.personnel.staff_id_card" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
               .punishment_against1 {
  overflow: hidden;
  
  margin-right:15px;
  margin:0 auto;
  margin-bottom:10px;
}
        .auto-style1 {
            width: 214px;
        }
        .auto-style2 {
            width: 108px;
        }
       
      .tdWidth{
            width:400px;
            height:40px;
        }
    
    </style>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="row">
        <div class="col-md-12 ds_nagevation_bar">
            <div style="margin-top: 5px">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li><a href="#">/</a></li>
                    <li><a href="/personnel_defult.aspx">Personnel</a></li>
                    <li><a href="#">/</a></li>
                    <li><a href="/personnel/employee_index.aspx">Employee Information</a></li>
                    <li><a href="#">/</a></li>
                    <li><a href="#" class="ds_negevation_inactive">ID Card Report</a></li>
                </ul>
            </div>

        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">

    </asp:ScriptManager>   

      <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
     <asp:UpdatePanel runat="server" ID="up1">
         <Triggers>
             <asp:AsyncPostBackTrigger ControlID="ddlBranch" />
             <asp:AsyncPostBackTrigger ControlID="rdbAll" />
             <asp:AsyncPostBackTrigger ControlID="rdbDeptWise" />
             <asp:AsyncPostBackTrigger ControlID="rdbIndividual" />
             <asp:AsyncPostBackTrigger ControlID="ddlDepName" />
             <asp:AsyncPostBackTrigger ControlID="rblEmpType" />

         </Triggers>
        <ContentTemplate>
            <div class="row Ptrow">
                <div class="employee_box_header PtBoxheader">
                    <h2>ID Card Report</h2>
                </div>
                <div class="employee_box_body">
                    <div class="employee_box_content" style="height:460px">
                        <div class="punishment_against1">
                             <div class="punishment_against">
                    <h1 runat="server" visible="false" id="WarningMessage" style="color: red; text-align: center"></h1>
                            <table runat="server" visible="true" id="tblGenerateType" class="employee_table">
                                <tr>
                                    <td>Company
                                    </td>
                                    <td>:</td>
                                    <td class="tdWidth">
                                        <asp:DropDownList ID="ddlBranch" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>Emp. Type
                                    </td>
                                    <td>:
                                    </td>
                                    <td class="tdWidth">
                                        <asp:RadioButtonList ID="rblEmpType" Class="" ClientIDMode="Static" runat="server" RepeatColumns="3" AutoPostBack="True" OnSelectedIndexChanged="rblEmpType_SelectedIndexChanged" />
                                       
                                    </td>
                                </tr>
                                <tr>
                                    <td>Report Type
                                    </td>
                                    <td>:
                                    </td>
                                    <td class="tdWidth">
                                        <asp:RadioButton ID="rdbAll" Class="" ClientIDMode="Static" runat="server" Text="All" AutoPostBack="True" Checked="True" OnCheckedChanged="rdbAll_CheckedChanged" />
                                        <asp:RadioButton ID="rdbDeptWise" ClientIDMode="Static" runat="server" Text="Department" AutoPostBack="True" OnCheckedChanged="rdbDeptWise_CheckedChanged" />
                                        <asp:RadioButton ID="rdbIndividual" ClientIDMode="Static" runat="server" Text="Individual" AutoPostBack="True" OnCheckedChanged="rdbIndividual_CheckedChanged" />
                                    </td>
                                </tr>                                
                                <tr id="trddldepname" runat="server">
                                    <td>Dept Name</td>
                                    <td>:</td>
                                    <td class="tdWidth">
                                        <asp:DropDownList runat="server" ID="ddlDepName" CssClass="form-control select_width" ClientIDMode="Static" OnSelectedIndexChanged="ddlDepName_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList></td>
                                </tr>
                                <tr id="trddlempcardno" runat="server">
                                    <td>Card No / Name</td>
                                    <td>:</td>
                                    <td class="tdWidth">
                                        <asp:DropDownList runat="server" ID="ddlEmpCardNo" CssClass="form-control select_width" ClientIDMode="Static"></asp:DropDownList></td>
                                </tr>
                                 <tr>
                                        <td>Report View
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth">
                                          <asp:RadioButtonList runat="server" RepeatColumns="2" ID="rblreportview">
                                              <asp:ListItem Selected="True" Value="0">Front Part</asp:ListItem>
                                              <asp:ListItem Selected="False" Value="1">Back Part</asp:ListItem>
                                          </asp:RadioButtonList>
                                        </td>
                                    </tr>
                            </table>
                        </div>
                            
                        <div id="workerlist" runat="server" class="id_card" style="background-color:white; width:61%;">
                            <div class="id_card_left EilistL">
                                <asp:ListBox ID="lstAll" runat="server" CssClass="lstdata EilistCec" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div class="id_card_center EilistC">
                                <table style="margin-top:0px;" class="employee_table">
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
                                <asp:ListBox ID="lstSelected" SelectionMode="Multiple" CssClass="lstdata EilistCec"  ClientIDMode="Static" runat="server"></asp:ListBox>
                            </div>
                        </div>
                        <div class="punishment_button_area">
                            <table class="emp_button_table">
                                <tbody>
                                    <tr>
                                        <th>
                                            <asp:Button ID="btnpreview" CssClass="css_btn Ptbut" runat="server" ClientIDMode="Static" Text="Preview" OnClick="btnpreview_Click" /></th>
                                        <th>
                                            <asp:Button ID="btnClose" CssClass="css_btn Ptbut" Text="Close" PostBackUrl="~/default.aspx" runat="server" /></th>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                    </div>
                    </div>
                </div>
            </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ddlEmpCardNo").select2();
            $("#ddlDepName").select2();

        });
        function loadcardNo() {
            $("#ddlEmpCardNo").select2();
            $("#ddlDepName").select2();
        }
        function goToNewTabandWindow(url) {
            window.open(url);
            loadcardNo();
        }

    </script>
</asp:Content>
