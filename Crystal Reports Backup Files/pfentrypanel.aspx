<%@ Page Title="PF Entry Panel" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="pfentrypanel.aspx.cs" Inherits="SigmaERP.pf.pfentrypanel" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <%--<script src="../scripts/jquery-1.8.2.js"></script>--%>

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


</script>
    <style>
        
/*.gvdisplay1 th:nth-child(1) {
    text-align: center;
}*/
       
    </style>
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
                     <li> <a href="#" class="ds_negevation_inactive Pactive">PF Entry Panel</a></li>
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
            <h2>PF Entry Panel</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">
                <input type="text" class="form-control" visible="true" id="txtFinding" runat="server" style="margin-left: 0px; width: 99%; text-align:center"  placeholder="Search by anythings" />               
                <div class="em_personal_info" id="divEmpPersonnelInfo" style="margin:0px">
                    
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <Triggers>
                            <%--  <asp:AsyncPostBackTrigger ControlID="ddlEmpCardNo" />--%>

                            <%--<asp:AsyncPostBackTrigger ControlID="chkSalaryCount" />

                     <asp:AsyncPostBackTrigger ControlID="chkPFMember" />
                      <asp:AsyncPostBackTrigger ControlID="btnSave" />         --%>
                           
                        </Triggers>
                        <ContentTemplate>

                            <asp:TabContainer ID="tc1" runat="server" CssClass="fancy fancy-green" AutoPostBack="true" OnActiveTabChanged="tc1_ActiveTabChanged"  ActiveTabIndex="0">
                                <asp:TabPanel runat="server"  TabIndex="0" ID="tab1" HeaderText="PF Member Pending">
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                                            <Triggers>
                                               
                                                <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />    
                                                <asp:AsyncPostBackTrigger ControlID="btnSubmit" />                                            
                                            </Triggers>
                                            <ContentTemplate>
                                                 <table class="em_personal_info_table">
                                              <tr>
                                                 <td>Company
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCompanyList" runat="server" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged"  ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" >
                                            </asp:DropDownList>
                                                </td>
                                            </tr>
                                                          <tr>
                                                 <td>
                                                     Employee Maturity 
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblEmpMaturity" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblEmpMaturity_SelectedIndexChanged">
                                                        
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>

                                          </table>
                                                <asp:GridView ID="gvpfpendinglist" runat="server" DataKeyNames="EmpId" 
                       OnRowDataBound="gvpfpendinglist_RowDataBound" Width="100%"  AutoGenerateColumns="false">
                                                     <HeaderStyle BackColor="#FFA500" Font-Bold="True" Font-Size="14px" ForeColor="White" Height="28px"></HeaderStyle>
                        <PagerStyle CssClass="gridview Sgridview" Height="40px" />
                        <Columns>
                            <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="left">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hideEmpId" runat="server"
                                        Value='<%# DataBinder.Eval(Container.DataItem, "EmpId")%>' />
                                    <%# Container.DataItemIndex+1%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="left" /> 
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Card No">
                                <ItemTemplate>
                                    <asp:Label ID="lblCardNo" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "EmpCardNo")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                           
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblempName" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "EmpName")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Joining Date">
                                <ItemTemplate>
                                    <asp:Label ID="lbljoinDate" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "EmpJoiningDate","{0:dd-MM-yyyy}")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="O.Balance" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtBalance" runat="server" ClientIDMode="Static"
                                        Height="28px" Style="  text-align:center;" CssClass="input"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "PfOpeningBalance")%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Emp.Contribution" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox Height="28px" Style="  text-align:center;" ID="txtEmpContribution" runat="server" ClientIDMode="Static"
                                        Width="80px" CssClass="input" OnTextChanged="txtEmpContribution_TextChanged" AutoPostBack="true"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "EmpContribution")%>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Basic" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:label Height="28px" Style="  text-align:center;" ID="lblbasic" runat="server" ClientIDMode="Static"
                                        Width="80px" CssClass="input"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "BasicSalary")%>'></asp:label>
                                </ItemTemplate>
                            </asp:TemplateField>  
                            <asp:TemplateField HeaderText="Pf Amount" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:label Height="28px" Style="  text-align:center;" ID="lblpfamount" runat="server" ClientIDMode="Static"
                                        Width="80px" CssClass="input"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "PFAmount")%>'></asp:label>
                                </ItemTemplate>
                            </asp:TemplateField>                             
                            
                             <asp:TemplateField HeaderText="PF Start Date"  ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                  <ItemTemplate >
                                   <asp:TextBox Height="28px" Style="  text-align:center;" ID="txtpfstartDate" runat="server" Enabled="true" ForeColor="Blue"  Text='<%# DataBinder.Eval(Container.DataItem, "PfDate","{0:dd-MM-yyyy}")%>' ></asp:TextBox> 
                                   <asp:CalendarExtender ID="txtpfstartDate_CalendarExtender" runat="server" Format="dd-MM-yyyy" TargetControlID="txtpfstartDate">
                                   </asp:CalendarExtender>
                                  </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                               </asp:TemplateField>
                            <asp:TemplateField HeaderText="Select"  ItemStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>                                                                                        
                                            <asp:CheckBox runat="server"  ItemStyle-HorizontalAlign="Center" ID="hdChk" Text="All" Checked="true" AutoPostBack="True" OnCheckedChanged="hdChk_CheckedChanged" /><br />                                                     
                                        </HeaderTemplate>
                                        <ItemTemplate>                               
                                                    <asp:CheckBox ID="chkStatus" ItemStyle-HorizontalAlign="Center" runat="server" AutoPostBack="true" Checked="true" OnCheckedChanged="chkStatus_CheckedChanged" />                                                
                                        </ItemTemplate>
                                    </asp:TemplateField>
                        </Columns>
                    </asp:GridView>                                               
                                           <table class="em_personal_info_table">
                                              <tr>
                                                 <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                   <%-- <asp:DropDownList ID="DropDownList1" runat="server"  ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" >
                                            </asp:DropDownList>--%>
                                                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" ClientIDMode="Static" CssClass="btn btn-default" OnClick="btnSubmit_Click" />
                                                </td>
                                            </tr>
                                          </table>        
                                       
                                                </ContentTemplate>
                             </asp:UpdatePanel>
                                    </ContentTemplate>

                                </asp:TabPanel>
                                <asp:TabPanel runat="server" ID="tab2" TabIndex="1" HeaderText="PF Member List">
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                                            <ContentTemplate>

                                          <table class="em_personal_info_table">
                                              <tr>
                                                 <td>Company
                                                </td>
                                                <td>:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCompanyList2" runat="server"  ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" OnSelectedIndexChanged="ddlCompanyList2_SelectedIndexChanged" >
                                            </asp:DropDownList>
                                                </td>
                                            </tr>
                                           
                                          </table>
                                        <asp:GridView runat="server" ID="gvpflist" CssClass="gvdisplay1" DataKeyNames="EmpId" AutoGenerateColumns="false" HeaderStyle-BackColor="#ffa500" HeaderStyle-Height="28px" HeaderStyle-ForeColor="White" PageSize="25" Width="100%"   >
                                            <Columns>
                                                <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="left">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hideEmpId" runat="server"
                                                            Value='<%# DataBinder.Eval(Container.DataItem, "EmpId")%>' />
                                                        <%# Container.DataItemIndex+1%>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" ItemStyle-HorizontalAlign="Center" />
                                                 <asp:BoundField DataField="EmpName" HeaderStyle-HorizontalAlign="Left" HeaderText="Name" />
                                                 <asp:BoundField DataField="EmpJoiningDate" HeaderText="Joining Date" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="PfOpeningBalance" HeaderText="O.Balance" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="PfEmpContribution" HeaderText="Contribution" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="PFAmount" HeaderText="PF.Amount" ItemStyle-HorizontalAlign="Center"/>
                                                 <asp:BoundField DataField="PfDate" HeaderText="Pf.Date" ItemStyle-HorizontalAlign="Center"/>                                                
                                                 <%--<asp:TemplateField HeaderText="Change"  HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                  <ItemTemplate >
                                      <asp:Button ID="btnEdit" runat="server" CommandName="Alter" Width="55px" Height="30px" Font-Bold="true" ForeColor="green" Text="Edit" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                  </ItemTemplate>
                              </asp:TemplateField>--%>
                                            </Columns>
                                        </asp:GridView>
                                                  </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>

                                </asp:TabPanel>
                            </asp:TabContainer>

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
            //$("#ddlBankList").select2();
            $(document).on("keyup", '.form-control', function () {
                searchTable($(this).val(), 'ContentPlaceHolder1_ContentPlaceHolder1_tc1_tab2_gvpflist', '');
                searchTable($(this).val(), 'ContentPlaceHolder1_ContentPlaceHolder1_tc1_tab1_gvpfpendinglist', '');
            });
            $(document).on("keypress", "body", function (e) {
                if (e.keyCode == 13) e.preventDefault();
                // alert('deafault prevented');

            });
            $("#ddlEmpCardNo").select2();
        });   
        
        function load() {
            $("#ddlEmpCardNo").select2();
            $("#ddlBankList").select2();            
        }
        function Messageshow(messagetype,message)
        {
            showMessage(message, messagetype);
            load();
        }
        function goToNewTab(url)
        {
            $("#ddlEmpCardNo").select2();
            $("#ddlBankList").select2();
            window.open(url);
        }        

      



        function InputValidation() {
            try {       
               
                if ($('#ddlEmpCardNo option:selected').text().length == 0) {

                    showMessage("warning->Please Select Any Employee");
                    $('#ddlEmpCardNo').focus();
                    return false;
                }
              
              
             
                return true;
              

            }
            catch (exception) {

            }
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
