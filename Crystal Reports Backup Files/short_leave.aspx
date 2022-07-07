<%@ Page Title="Short Leave" Language="C#" MasterPageFile="~/leave_nested.master" AutoEventWireup="true" CodeBehind="short_leave.aspx.cs" Inherits="SigmaERP.leave.short_leave" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <%--      <script src="../scripts/jquery-1.8.2.js"></script>
        <script type="text/javascript">

            var oldgridcolor;
            function SetMouseOver(element) {
                oldgridcolor = element.style.backgroundColor;
                element.style.backgroundColor = '#ffeb95';
                element.style.cursor = 'pointer';
                // element.style.textDecoration = 'underline';
            }
            function SetMouseOut(element) {
                element.style.backgroundColor = oldgridcolor;
                // element.style.textDecoration = 'none';

            }

</script>--%>
    <style>
        #ContentPlaceHolder1_MainContent_gvLeaveList th {
            text-align:center;
        }
           #ContentPlaceHolder1_MainContent_gvLeaveList th:nth-child(2) {
            text-align:left;
            padding-left:3px;
        }
                 #ContentPlaceHolder1_MainContent_gvLeaveList td:nth-child(2) {
          
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
                       <li>/</li>
                       <li> <a href="/leave_default.aspx">Leave</a></li>
                       <li>/</li>
                       <li> <a href="#" class="ds_negevation_inactive Lactive">Short Leave</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <link href="../style/Design.css" rel="stylesheet" />
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>


    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSave" />
            <asp:AsyncPostBackTrigger ControlID="ddlLeaveName" />            
            <asp:AsyncPostBackTrigger ControlID="ddlBranch" />
            <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />
        </Triggers>
        <ContentTemplate>
            <div class="main_box Lbox">
        <div class="main_box_header_leave LBoxheader">
                    <h2>Short Leave Entry Panel</h2>
                </div>

                <div class="main_box_body_leave Lbody">
            <div class="main_box_content_leave" >

                        <!--ST-->
                        <div class="application_box_left" style="width:61%">
                            <fieldset>
                                <legend>
                                    <b>Leave Transaction</b>                                   
                                </legend>
                                <table class="employee_table">
                                    <tr id="trCompanyName" runat="server" >
                                        <td>Company Name <span class="requerd1">*</span></td>
                                        <td>:</td>
                                        <td class="tdWidth" colspan="8">
                                        <asp:DropDownList ID="ddlBranch" ClientIDMode="Static"   CssClass="form-control select_width" Width="96%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged"  >              
                                         </asp:DropDownList>
                                        </td>
                                    </tr>                           
                                    <tr>
                                        <td>Emp Card No <span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth" colspan="8">
                                            <asp:DropDownList ID="ddlEmpCardNo" CssClass="form-control select_width" runat="server"  Width="96%" ClientIDMode="Static"   ></asp:DropDownList>
                                                                                    
                                        </td>
                                    </tr>                                    
                                      <tr runat="server" visible="false" >
                                        <td>Leave Name<span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth" colspan="8">
                                            <asp:DropDownList ID="ddlLeaveName" ClientIDMode="Static" CssClass="form-control select_width" runat="server" AppendDataBoundItems="True" Width="96%"></asp:DropDownList>                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td> Date<span class="requerd1">*</span>
                                        </td>
                                        <td>:
                                        </td>
                                        <td class="tdWidth" colspan="8">
                                            <asp:TextBox ID="txtLeaveDate" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Width="96%"></asp:TextBox>
                                            
                                            <asp:CalendarExtender runat="server" Format="dd-MM-yyyy"
                                                PopupButtonID="imgEffectDateTo" Enabled="True"
                                                TargetControlID="txtLeaveDate" ID="CalendarExtender4">
                                            </asp:CalendarExtender>                            

                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Time <span class="requerd1">*</span></td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="txtInHur" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00" Style="text-align: center; font-weight: bold; width: 60px; float: left;"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtInMin" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00" Style="text-align: center; font-weight: bold; width: 60px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlInTimeAMPM" runat="server" CssClass="attend_select_min" Width="67px" Style="float: left;">                                                
                                                <asp:ListItem Value="PM">PM</asp:ListItem>
                                                <asp:ListItem Value="AM">AM</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>                                
                                        <td style="font:bold">To</td>                                       
                                        <td>
                                            <asp:TextBox ID="txtOutHur" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00" Style="text-align: center; font-weight: bold; width: 60px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOutMin" runat="server" ClientIDMode="Static" CssClass="form-control attend_text_box_width" MaxLength="2" Text="00" Style="text-align: center; font-weight: bold; width: 60px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlOutTimeAMPM" runat="server" CssClass="attend_select_min" Width="67px">
                                                <asp:ListItem Value="PM" Selected="True">PM</asp:ListItem>
                                                <asp:ListItem Value="AM">AM</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>                                                              
                                     <tr>
                                        <td >
                                            Remark
                                        </td>
                                        <td>:</td>
                                        <td class="tdWidth" colspan="8">
                                           <asp:TextBox ID="txtRemark" runat="server" Height="40px" ClientIDMode="Static" CssClass="form-control text_box_width" Width="96%" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                              <div class="border" style="width:35%">
                        </div>
                        <div class="list_button" >
                            <table >
                                <tbody>

                                    <tr>
                                       <td style="width: 17px;">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Lbutton" OnClick="btnSave_Click"/>
                                        </td>
                                        <td style="width: 17px;">
                                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="Lbutton" OnClick="btnClear_Click" />
                                        </td>
                                        <td style="width: 17px;">
                                            <asp:Button ID="btnClose" runat="server" Text="Close" PostBackUrl="~/leave_default.aspx" CssClass="Lbutton"/>
                                        </td>                                       
                                    </tr>
                                </tbody>
                            </table>
                        </div>  
                                
                
                
               <div style="width: 100%; margin:0px auto ">
                       <asp:GridView HeaderStyle-BackColor="#5EC1FF" ID="gvLeaveList" HeaderStyle-Height="28px" runat="server" AutoGenerateColumns="false"  BackColor="White" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="14px" AllowPaging="true" PageSize="40" Width="100%" DataKeyNames="SrLvID" OnRowCommand="gvLeaveList_RowCommand" OnRowDataBound="gvLeaveList_RowDataBound" >
                          <PagerStyle CssClass="gridview" />
                          <Columns>                             
                              <asp:BoundField DataField="EmpCardNo" HeaderText="Card No"   ItemStyle-HorizontalAlign="Center" />
                              <asp:BoundField DataField="EmpName" HeaderText="EmpName"    />
                              <asp:BoundField DataField="LvDate" HeaderText="Date"   ItemStyle-HorizontalAlign="Center"/>                           
                              <asp:BoundField DataField="FromTime" HeaderText="From Time"  ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                              <asp:BoundField DataField="ToTime" HeaderText="To Time"  ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="Red" />
                              <asp:BoundField DataField="LvTime" HeaderText="Leave Time" ItemStyle-HorizontalAlign="Center"  />   
                              <asp:BoundField DataField="LvStatus" HeaderText="Leave Status" ItemStyle-HorizontalAlign="Center"  />                              
                              <asp:BoundField DataField="Remarks" HeaderText="Remarks"    ItemStyle-HorizontalAlign="Center" />  
                              <asp:TemplateField HeaderText="Details" HeaderStyle-Width="35px">
                                  <ItemTemplate>
                                      <asp:Button ID="btnShow" runat="server" CommandName="View" Font-Bold="true" Text="View" Width="55px" Height="30px" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                  </ItemTemplate>
                              </asp:TemplateField>                            
                              <asp:TemplateField>
                                  <HeaderTemplate>
                                      Edit
                                  </HeaderTemplate>
                                  <ItemTemplate>
                                      <asp:Button runat="server" ID="btnEdit" Text="Edit" Font-Bold="true" CommandName="Alter" ForeColor="Green" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                  </ItemTemplate>
                                   <ItemStyle HorizontalAlign="Center" />
                              </asp:TemplateField>
                              <asp:TemplateField>
                                  <HeaderTemplate>
                                      Delete
                                  </HeaderTemplate>
                                  <ItemTemplate>
                                      <asp:Button ID="btnView" runat="server" Text="Delete" Font-Bold="true" CommandName="remove" ForeColor="Red" CommandArgument='<%#((GridViewRow)Container).RowIndex%>'
                                          OnClientClick="return confirm('Are you sure, you want to delete the record?')"
                                          />
                                  </ItemTemplate>
                                  <ItemStyle HorizontalAlign="Center" />
                              </asp:TemplateField>
                          </Columns>
                      </asp:GridView>
                      <div id="divRecordMessage" runat="server" visible="false" style="color: red; background-color:white; font-weight: bold; text-align: center; padding-top: 75px; font-size: 32px; height: 118px">                           
                         </div>
                  </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

     <script type="text/javascript">
       
         $(document).ready(function () {
             load();

         });
         function load() {
             $("#ddlEmpCardNo").select2();
         }
         function goToNewTabandWindow(url) {
             window.open(url);
             load();
         }
    </script>
</asp:Content>
