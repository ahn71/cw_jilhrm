<%@ Page Title="Districtwise Employee Report" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="district_information.aspx.cs" Inherits="SigmaERP.personnel.district_information" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:HiddenField ID="upSuperAdmin" runat="server" ClientIDMode="Static" />
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
          <Triggers>
              <asp:AsyncPostBackTrigger ControlID="rdball" />
              <asp:AsyncPostBackTrigger ControlID="rdbindividual" />
             
          </Triggers>
          <ContentTemplate>
    <div class="worker_id__main_box">
        <div class="punishment_box_header">
            <h2>Districtwise Employee Report</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content">

                <div class="punishment_against">
                    <fieldset>
                    <legend>
                        <b>Option</b>
                    </legend>
                  <table class="employee_table">                 
                        <tbody>
                      <tr>
                        <td width="27%">
                            <asp:RadioButton ID="rdball" Class=""  ClientIDMode="Static" AutoPostBack="true" Checked="true" runat="server" Text="All" OnCheckedChanged="rdball_CheckedChanged"  />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdbindividual" ClientIDMode="Static" AutoPostBack="true" runat="server" Text="Individual" OnCheckedChanged="rdbindividual_CheckedChanged"  />
                        </td>
                   </tr>       
                      
                    </tbody>
                  </table>
                  </fieldset>
                </div>
                <div  id="divindivisual" class="punishment_against" runat="server">
                   <table class="employee_table">
                       
                       <tr runat="server" id="trDistrictName">
                       <td>
                         Individual District
                        </td>
                         <td>
                           <asp:DropDownList ID="ddlDistrictName" CssClass="form-control select_width" runat="server"></asp:DropDownList>
                        </td>
                           </tr>
                   </table>
                </div>
                <div class="punishment_button_area">
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                <th><asp:Button ID="btnPrintpreview" runat="server" CssClass="back_button" ClientIDMode="Static" Text="Preview" OnClick="btnPrintpreview_Click"  /></th>
                                <th><asp:Button ID="btnClose" PostBackUrl="~/default.aspx" Text="Close" runat="server" CssClass="css_btn" /></th>   
                         </tr>
                    </tbody>
                  </table>
                </div>

        </div>
      </div>
    </div>
              </ContentTemplate>
        </asp:UpdatePanel>
    <script type="text/javascript">
        function goToNewTabandWindow(url) {
            window.open(url);

        }

    </script>
</asp:Content>
