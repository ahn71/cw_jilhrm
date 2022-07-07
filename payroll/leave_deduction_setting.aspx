<%@ Page Title="Leave Deduction Setting" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="leave_deduction_setting.aspx.cs" Inherits="SigmaERP.payroll.leave_deduction_setting" %>
<%@ Register Assembly="AjaxControlToolkit"  Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
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
    .lds-src-row {
        width: 22%;
        margin: 0px auto 10px;
        overflow: hidden;
    }
    @media only screen and (max-width: 900px) {
      .lds-src-row {
            width: 100%;
            margin: 0px auto 10px;
            overflow: hidden;
        }
    }
    .gvdisplay1 tr th, .gvdisplay1 tr td{
        padding:5px 5px 5px 5px
    }
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
                    <li>  <a href="/payroll/salary_index.aspx">Salary</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                     <li> <a href="#" class="ds_negevation_inactive Pactive">Leave Deduction Setting</a></li>
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
            <h2>Leave Deduction Setting</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">                            
                <div class="em_personal_info" id="divEmpPersonnelInfo" style="margin:0px">                    
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <Triggers>                           
                           <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                           <asp:AsyncPostBackTrigger ControlID="gvEmplyeeList" />
                        </Triggers>
                        <ContentTemplate>
                            <div class="lds-src-row">
                                <div class="">
                                    <table>
                                        <tr>
                                            <td>Month &nbsp;</td>
                                            <td>
                                                <asp:TextBox runat="server" ClientIDMode="Static" ID="txtMonth" CssClass="form-control" autocomplete="off" placeholder="MM-yyyy" ></asp:TextBox>
                                                    <asp:CalendarExtender ID="txtPartialAttDate_CalendarExtender" runat="server" Format="MM-yyyy"  TargetControlID="txtMonth">
                                                    </asp:CalendarExtender>   
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ClientIDMode="Static" Text="Search" CssClass="Pbutton" ID="btnSearch" OnClick="btnSearch_Click" />
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ClientIDMode="Static" Text="Submit" CssClass="Pbutton" ID="btnSubmit" OnClick="btnSubmit_Click"/>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                </div>
                            </div>
                             
                                                  
                            
                           
                        
                            <asp:GridView runat="server" ID="gvEmplyeeList"  HeaderStyle-HorizontalAlign="Center" CssClass="gvdisplay1"  AutoGenerateColumns="false" HeaderStyle-BackColor="#ffa500" HeaderStyle-Height="28px" HeaderStyle-ForeColor="White"  Width="100%" OnRowDataBound="gvEmplyeeList_RowDataBound">
                                            <Columns>                                       
                                                 <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" />
                                                 <asp:BoundField DataField="EmpName" HeaderText="Name"/>                                                 
                                                 <asp:BoundField DataField="DptName" HeaderText="Department"/>
                                                 <asp:BoundField DataField="DsgName" HeaderText="Designation"/>                                                 
                                                 <asp:TemplateField HeaderText="Chosen">
                                                <ItemTemplate>
                                                    <asp:Label Visible="false" runat="server" ID="lblEmpId" ClientIDMode="Static" Text='<%# Eval("EmpID").ToString()%>'></asp:Label>                                                    
                                                <asp:CheckBox runat="server" ID="ckbChosen" Checked='<%# bool.Parse(Eval("Status").ToString()) %>' />
                                                </ItemTemplate>
                                                </asp:TemplateField>                                               
                                            </Columns>
                          </asp:GridView>
                         </ContentTemplate>
                    </asp:UpdatePanel>
                   

                </div>
            </div>
        </div>
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
