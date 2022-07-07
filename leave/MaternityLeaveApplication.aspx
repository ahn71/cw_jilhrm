<%@ Page Title="Maternity Application" Language="C#" MasterPageFile="~/leave_nested.Master" AutoEventWireup="true" CodeBehind="MaternityLeaveApplication.aspx.cs" Inherits="SigmaERP.personnel.MaternityLeaveApplication" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row">
        <div class="col-md-12 ds_nagevation_bar">
            <div style="margin-top: 5px">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li>/</li>
                    <li><a href="/leave_default.aspx">Leave</a></li>
                    <li>/</li>
                    <li><a href="#" class="ds_negevation_inactive">Maternity Application</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    <asp:HiddenField ID="upSuperAdmin" runat="server" ClientIDMode="Static" />
    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
          <Triggers>
              
              <asp:AsyncPostBackTrigger ControlID="rdbWorker" />
              <asp:AsyncPostBackTrigger ControlID="rdbStaff" />
          </Triggers>
          <ContentTemplate>
    <div style="width:691px" class="worker_id__main_box">
        
        <div class="punishment_box_header">
            <h2>Maternity Application</h2>
        </div>
        <div class="employee_box_body">
            <div class="employee_box_content">

                
                <div  id="divindivisual" class="punishment_against" runat="server">
                    <fieldset>
                    <legend>
                        <b>Option</b>
                    </legend>
                   <table class="employee_table">
                        <tr runat="server" id="trEmpType">
                        <td width="27%">
                            <asp:RadioButton ID="rdbWorker" ClientIDMode="Static" Class="" Checked="true" runat="server" Text="Worker"  AutoPostBack="True" OnCheckedChanged="rdbWorker_CheckedChanged" />
                        </td>
                        <td>
                            <asp:RadioButton ID="rdbStaff" ClientIDMode="Static" runat="server" Text="Staff" AutoPostBack="True" OnCheckedChanged="rdbStaff_CheckedChanged" />
                        </td>
                   </tr> 
                       <tr runat="server" id="trCardNo">
                       <td>
                         Individual Card
                        </td>
                         <td>
                           <asp:DropDownList ID="ddlCardNo" CssClass="form-control select_width" width="129px" runat="server"></asp:DropDownList>
                        </td>
                           <td>
                          <asp:TextBox ID="txtCardNo" PLaceHolder="Type Card No" Width="92px" runat="server"   ClientIDMode="Static" CssClass="form-control text_box_width_import"></asp:TextBox>              
                           </td>
                           <td>
                               <th><asp:Button ID="btnFind" runat="server" CssClass="back_button" ClientIDMode="Static" Text="Find" OnClick="btnFind_Click"   /></th>
                           </td>
                           </tr>
                   </table>
                        </fieldset>
                </div>
                <div  class="punishment_button_area">
                     <fieldset>
                    <legend>
                        <b>Preview</b>
                    </legend>
                    <table class="emp_button_table">
                        <tbody>
                            <tr>
                                <th><asp:Button ID="btnMaternityApp" runat="server" Width="246px" CssClass="back_button" ClientIDMode="Static" Text="Maternity Application" OnClick="btnMaternityApp_Click" /></th>
                                <th><asp:Button ID="btnDocCerLetter" runat="server" Width="246px" CssClass="back_button" ClientIDMode="Static" Text="Doctor Certification Letter" OnClick="btnDocCerLetter_Click"   /></th>
                                  
                                
                         </tr>
                            <tr>
                                <th><asp:Button ID="btnGrantedDoctor" Width="246px" runat="server" CssClass="back_button" ClientIDMode="Static" Text="Granted by the doctor" OnClick="btnGrantedDoctor_Click" /></th>
                                <th><asp:Button ID="btnLetterofAuthority" runat="server" Width="155px" CssClass="back_button" ClientIDMode="Static" Text="Letter of Authority" OnClick="btnLetterofAuthority_Click"   />   <asp:Button ID="btnClose" Width="80px" PostBackUrl="~/leave_default.aspx" Text="Close" runat="server" CssClass="css_btn" /></th>
                                
                            </tr>
                    </tbody>
                  </table>
                         </fieldset>
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
        var ddlText, ddlValue, ddl, lblMesg;
        function CacheItems() {
            ddlText = new Array();
            ddlValue = new Array();
            //ddl = document.getElementById("<%=ddlCardNo.ClientID %>");
            //lblMesg = document.getElementById("<%=lblMessage.ClientID%>");
            for (var i = 0; i < ddl.options.length; i++) {
                ddlText[ddlText.length] = ddl.options[i].text;
                ddlValue[ddlValue.length] = ddl.options[i].value;
            }
        }
        window.onload = CacheItems;

        function FilterItems(value) {
            ddl.options.length = 0;
            for (var i = 0; i < ddlText.length; i++) {
                if (ddlText[i].toLowerCase().indexOf(value) != -1) {
                    AddItem(ddlText[i], ddlValue[i]);
                }
            }
            //lblMesg.innerHTML = ddl.options.length + " items found.";
            if (ddl.options.length == 0) {
                AddItem("No items found.", "");
            }
        }

        //function AddItem(text, value) {
        //    var opt = document.createElement("option");
        //    opt.text = text;
        //    opt.value = value;
        //    ddl.options.add(opt);
        //}

    </script>
</asp:Content>
