<%@ Page Title="Holiday Setting" Language="C#" MasterPageFile="~/leave_nested.Master" AutoEventWireup="true" CodeBehind="holyday.aspx.cs" Inherits="SigmaERP.personnel.holyday" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #ContentPlaceHolder1_MainContent_gvHoliday th, td {
            text-align:center;
        }
         #ContentPlaceHolder1_MainContent_gvHoliday th:nth-child(2), td:nth-child(2) {
            text-align:left;
            padding-left:3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <div class="row">
                  <div class="col-md-12 ds_nagevation_bar">
               <div style="margin-top: 5px">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                       <li> <a href="#">/</a></li>
                       <li> <a href="/leave_default.aspx">Leave</a></li>
                       <li> <a href="#">/</a></li>
                       <li> <a href="#" class="ds_negevation_inactive Lactive">Holiday Setting</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>
         <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">

    <Triggers>
    <asp:AsyncPostBackTrigger ControlID="btnSave" />
                            
    <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
    </Triggers>
    <ContentTemplate>

    <div class="main_box Lbox">
    <div class="main_box_header_leave LBoxheader">
    <h2>Holiday Setting</h2>
    </div>
    <div class="main_box_body_leave Lbody">
        <div class="main_box_content_leave" id="divElementContainer" runat="server">
    <div class="input_division_info">
          <table class="employee_table">
                <tr>
                    <td>Company Name<span class="requerd1">*</span></td>
                    <td>:</td>
                    <td>
                        <asp:DropDownList ID="ddlCompanyList" ClientIDMode="Static" CssClass="form-control select_width" Width="96%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>


                <tr>
                    <td>Date<span class="requerd1">*</span>
                    </td>
                    <td>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" ClientIDMode="Static" runat="server" Width="96%" CssClass="form-control text_box_width"></asp:TextBox>
                        <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                            PopupButtonID="imgDate" Enabled="True"
                            TargetControlID="txtDate" ID="CExtApplicationDate">
                        </asp:CalendarExtender>



                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValida" runat="server"
    ControlToValidate="txtDate" ValidationExpression="^(([1-9])|(0[1-9])|(1[0-2]))\/((0[1-9])|([1-31]))\/((19|20)\d\d)$" Display="Dynamic" ValidationGroup="save" SetFocusOnError="true" ErrorMessage="invalid date">*</asp:RegularExpressionValidator>--%>
                                    
                    </td>
                </tr>
                <tr>
                    <td>Discription<span class="requerd1">*</span>
                    </td>
                    <td>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescription" ClientIDMode="Static" runat="server" Width="96%" CssClass="form-control text_box_width"></asp:TextBox>
                        <asp:RequiredFieldValidator ForeColor="Red" ValidationGroup="save" ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDescription" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>


            </table>
        </div>

        <div class="button_area">
            <table>

                <tr>
                    <td>
                        <asp:Button ID="btnSave" CssClass="Lbutton" ValidationGroup="save" runat="server" Text="Save" OnClick="btnSave_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="Lbutton" OnClick="btnClear_Click" />
                    </td>
                    <td>
                        <asp:Button ID="Button3" runat="server" PostBackUrl="~/leave_default.aspx" Text="Close" CssClass="Lbutton" />
                    </td>

                </tr>

            </table>
        </div>

        <div class="show_division_info">
            <%--<div id="ShiftConfig" class="datatables_wrapper" runat="server" style="width:100%; height:auto; max-height:500px;overflow:auto;overflow-x:hidden;"></div>--%>
            <asp:GridView ID="gvHoliday" runat="server" Width="100%" AutoGenerateColumns="false" DataKeyNames="HCode"  HeaderStyle-Height="28px" HeaderStyle-ForeColor="white" HeaderStyle-Font-Size="14px" HeaderStyle-Font-Bold="true" OnPageIndexChanging="gvHoliday_PageIndexChanging" OnRowCommand="gvHoliday_RowCommand" OnRowDeleting="gvHoliday_RowDeleting" AllowPaging="True" PageSize="20" OnRowDataBound="gvHoliday_RowDataBound">
                <RowStyle HorizontalAlign="Center" />
                <PagerStyle CssClass="gridview" />
                <Columns>

                    <asp:BoundField DataField="HDate" HeaderText="Date" ItemStyle-Height="28px" />

                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />

                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:Button ID="btnAlter" runat="server" CommandName="Alter" ControlStyle-CssClass="btnForAlterInGV"  CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Edit"  />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:Button ID="btnDelete" runat="server" CommandName="Delete" ControlStyle-CssClass="btnForDeleteInGV" CommandArgument="<%#((GridViewRow)Container).RowIndex%>" Text="Delete"  OnClientClick="return confirm('Are you sure to delete?');" />
                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>
                <HeaderStyle BackColor="#5EC1FF" Height="28px" />
            </asp:GridView>

        </div>


    </div>
    </div>
    </div>
     </ContentTemplate>
      </asp:UpdatePanel>
    <script type="text/javascript">
 

    </script>
</asp:Content>
