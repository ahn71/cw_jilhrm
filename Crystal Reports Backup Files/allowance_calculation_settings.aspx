<%@ Page Title="Allowance Calculation Settings" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="allowance_calculation_settings.aspx.cs" Inherits="SigmaERP.payroll.allowance_calculation_settings" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
        .rbl leabel {
            margin-left:5px;
        padding-right:10px;
        }
        .gv1 th {
            text-align:center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    <li><a href="#" class="ds_negevation_inactive Pactive">Allowance Calculation Settings</a></li>
                </ul>
            </div>
        </div>
    </div>
     <asp:ScriptManager ID="ScriptManager1" runat="server"  ></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
   <%-- <div class="employee_main_box">
        
        <div class="employee_box_header">
            <h2 style="float: none">
                <label id="lblEmpFormType" for="Employee">Salary Allowance Calculation Settings&nbsp; Panel</label></h2>
        </div>
        <div class="employee_box_body">
         
            <div class="employee_box_content">--%>
     <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2 style="float: none">
                <label id="lblEmpFormType" for="Employee">Salary Allowance Calculation Settings&nbsp; Panel</label></h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">
                
                <div class="em_personal_info" id="divEmpPersonnelInfo" style="margin:0px">
                    
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <Triggers>
                                        
                        </Triggers>
                        <ContentTemplate>

                               <asp:TabContainer runat="server" ID="tcContainer" CssClass="fancy fancy-green" ActiveTabIndex="0" >
                <asp:TabPanel runat="server" HeaderText="Allowance Calculation Plan Settings" TabIndex="0"  ID="tab1" >
                    <ContentTemplate>
                        <center>
                                <div>
                                    <br />
                                   <table>
                                       <td>Company 
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlCompanyList" runat="server" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" Width="96%" >
                                            </asp:DropDownList>
                                        </td>
                                       <tr>
                                           <td>
                                              Type
                                           </td>
                                           <td>:</td>
                                           <td>
                                               <asp:RadioButtonList Font-Bold="True" runat="server" RepeatDirection="Horizontal" ID="rblEmpType" >

                                        </asp:RadioButtonList>
                                           </td>
                                       </tr>
                                          <tr id="salarytype" runat="server">

                                                <td runat="server">Salary Type
                                                </td>
                                                <td runat="server">:
                                                </td>
                                                <td runat="server">
                                                  <asp:RadioButtonList ID="rblSalaryType" runat="server" Font-Bold="True" AutoPostBack="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblSalaryType_SelectedIndexChanged">
                                                      <asp:ListItem Value="Scale" Text="Scale" ></asp:ListItem>
                                                      <asp:ListItem Value="Gross" Text="Gross"></asp:ListItem>
                                                      <asp:ListItem Value="Gross Scale" Text="Gross Scale"></asp:ListItem>
                                                  </asp:RadioButtonList> 
                                                  
                                                </td>
                                            </tr>
                                   </table>
                                       
                                        <br />
                                   
                               <table border="1">
                                     <tr id="trBasic" runat="server">
                                       <td runat="server">
                                           Basic
                                       </td>
                                    
                                       <td runat="server">
                                           <asp:RadioButtonList runat="server" ID="rblBasic" CssClass="rbl" >
                                               <asp:ListItem Text="Percentage (%)" Value="0"></asp:ListItem>
                                             
                                               <asp:ListItem Text="Amount (৳)" Value="1" ></asp:ListItem>
                                               <asp:ListItem Text="Not Count" Value="2" ></asp:ListItem>
                                           </asp:RadioButtonList>                                           
                                       </td>
                                         
                                   </tr>
                                   <tr>
                                       <td>
                                           House Rent
                                       </td>
                                    
                                       <td>
                                           <asp:RadioButtonList runat="server" ID="rblHouseRent" CssClass="rbl" >
                                              <asp:ListItem Text="Percentage of Gross (%)" Value="0" ></asp:ListItem>
                                               <asp:ListItem Text="Percentage of Basic(%)" Value="3" ></asp:ListItem>
                                              <asp:ListItem Text="Amount (৳)" Value="1" ></asp:ListItem>
                                               <asp:ListItem Text="Not Count" Value="2" ></asp:ListItem>
                                           </asp:RadioButtonList>
                                       </td>
                                      <%-- <td>
                                             <asp:CheckBox runat="server" ID="ckbHRperOfBasic" Text="of Basic" />
                                         </td>--%>
                                   </tr>
                                    <tr>
                                       <td>
                                           Medical
                                       </td>
                                      
                                       <td>
                                           <asp:RadioButtonList runat="server" ID="rblMedical" CssClass="rbl" >
                                              <asp:ListItem Text="Percentage of Gross (%)" Value="0" ></asp:ListItem>
                                               <asp:ListItem Text="Percentage of Basic(%)" Value="3" ></asp:ListItem>
                                               <asp:ListItem Text="Amount (৳)" Value="1" ></asp:ListItem>
                                               <asp:ListItem Text="Not Count" Value="2" ></asp:ListItem>
                                           </asp:RadioButtonList>
                                       </td>
                                       <%-- <td>
                                             <asp:CheckBox runat="server" ID="CheckBox1" Text="of Basic" />
                                         </td>--%>
                                   </tr>
                                    <tr>
                                       <td>
                                          Food
                                       </td>
                                        
                                       <td>
                                           <asp:RadioButtonList runat="server" ID="rblFood" CssClass="rbl" >
                                              <asp:ListItem Text="Percentage of Gross (%)" Value="0" ></asp:ListItem>
                                               <asp:ListItem Text="Percentage of Basic(%)" Value="3" ></asp:ListItem>
                                               <asp:ListItem Text="Amount (৳)" Value="1" ></asp:ListItem>
                                               <asp:ListItem Text="Not Count" Value="2" ></asp:ListItem>
                                           </asp:RadioButtonList>
                                       </td>
                                      <%--  <td>
                                             <asp:CheckBox runat="server" ID="CheckBox2" Text="of Basic" />
                                         </td>--%>
                                   </tr>
                                    <tr>
                                       <td>
                                           Convence
                                       </td>
                                     
                                       <td>
                                           <asp:RadioButtonList runat="server" ID="rblConvence" CssClass="rbl" >
                                               <asp:ListItem Text="Percentage of Gross (%)" Value="0" ></asp:ListItem>
                                               <asp:ListItem Text="Percentage of Basic(%)" Value="3" ></asp:ListItem>
                                              <asp:ListItem Text="Amount (৳)" Value="1" ></asp:ListItem>
                                               <asp:ListItem Text="Not Count" Value="2" ></asp:ListItem>
                                           </asp:RadioButtonList>
                                       </td>
                                     <%--   <td>
                                             <asp:CheckBox runat="server" ID="CheckBox3" Text="of Basic" />
                                         </td>--%>
                                   </tr>
                                    <tr>
                                       <td>
                                          Technical
                                       </td>
                                      
                                       <td>
                                           <asp:RadioButtonList runat="server" ID="rblTechnical" CssClass="rbl" >
                                             <asp:ListItem Text="Percentage of Gross (%)" Value="0" ></asp:ListItem>
                                               <asp:ListItem Text="Percentage of Basic(%)" Value="3" ></asp:ListItem>
                                               <asp:ListItem Text="Amount (৳)" Value="1" ></asp:ListItem>
                                               <asp:ListItem Text="Not Count" Value="2" ></asp:ListItem>
                                           </asp:RadioButtonList>
                                       </td>
                                      <%--  <td>
                                             <asp:CheckBox runat="server" ID="CheckBox4" Text="of Basic" />
                                         </td>--%>
                                   </tr>
                                    <tr>
                                       <td>
                                           Others
                                       </td>
                                   
                                       <td>
                                           <asp:RadioButtonList runat="server" ID="rblOthers" CssClass="rbl" >
                                               <asp:ListItem Text="Percentage of Gross (%)" Value="0" ></asp:ListItem>
                                               <asp:ListItem Text="Percentage of Basic(%)" Value="3" ></asp:ListItem>
                                               <asp:ListItem Text="Amount (৳)" Value="1" ></asp:ListItem>
                                               <asp:ListItem Text="Not Count" Value="2" ></asp:ListItem>
                                           </asp:RadioButtonList>
                                       </td>
                                      <%--  <td>
                                             <asp:CheckBox runat="server" ID="CheckBox5" Text="of Basic" />
                                         </td>--%>
                                   </tr>
                                   <tr>
                                       <td>
                                           Provident Fund
                                       </td>
                                   
                                       <td>
                                           <asp:RadioButtonList runat="server" ID="rblProvident" CssClass="rbl" >
                                              <asp:ListItem Text="Percentage of Gross (%)" Value="0" ></asp:ListItem>
                                               <asp:ListItem Text="Percentage of Basic(%)" Value="3" ></asp:ListItem>
                                               <asp:ListItem Text="Amount (৳)" Value="1" ></asp:ListItem>
                                               <asp:ListItem Text="Not Count" Value="2" ></asp:ListItem>
                                           </asp:RadioButtonList>
                                       </td>
                                       <%--<td>
                                             <asp:CheckBox runat="server" ID="CheckBox6" Text="of Basic" />
                                         </td>--%>
                                   </tr>

                               </table>
                           </div>
                                <br />
                               
                                <div>
                                   
                                    <asp:Button runat="server" ID="btnSave" CssClass="Pbutton" Text="Save" Width="100px" OnClick="btnSave_Click" />
                                     <asp:Button runat="server" ID="btnClear" CssClass="Pbutton" Text="Clear" Width="98px" OnClick="btnClear_Click" />
                                     
                                </div>
                                </center>
                        <br />
                             <asp:GridView  runat="server" ID="gvSalaryCalculationList" CssClass="gv1" DataKeyNames="AlCalId" AutoGenerateColumns="False" PageSize="25" Width="100%" OnRowCommand="gvSalaryCalculationList_RowCommand" OnRowDataBound="gvSalaryCalculationList_RowDataBound" >
                                            <Columns>
                                              
                                                <asp:TemplateField>
                                                    <HeaderTemplate>SL</HeaderTemplate>
                                                    <ItemTemplate >
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="BasicAllowance" HeaderText="Basic">
                                                 <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="MedicalAllownce" HeaderText="Medical">   

                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="FoodAllownce" HeaderText="Food">
                                                 <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="ConvenceAllownce" HeaderText="Convence">   

                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="TechnicalAllowance" HeaderText="Technical">
                                                 <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="HouseRent" HeaderText="House">   
                                                   
                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                   
                                                <asp:BoundField DataField="OthersAllowance" HeaderText="Others">
                                                <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ProvidentFund" HeaderText="ProvidentFund">

                                                <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="EntryDate" HeaderText="Entry Date Time">
                                                 <ItemStyle Font-Bold="True" ForeColor="Blue" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="EmpType" HeaderText="Type">   
                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SalaryType" HeaderText="Salary Type" >  
                                                                                                
                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                                                                
                                                <asp:TemplateField HeaderText="Edit">
                                               
                                  <ItemTemplate >
                                      <asp:Button ID="btnEdit" runat="server" CommandName="Alter" Width="55px" Height="30px" Font-Bold="true" ForeColor="green" Text="Edit" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                  </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="30px" />
                              </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="#ffa500" ForeColor="White" Height="28px" />
                                        </asp:GridView>
                    </ContentTemplate>
                </asp:TabPanel>
                                   <asp:TabPanel ID="tab2" runat="server" HeaderText="Allowance Calculation Amount Settings" TabIndex="1" >
                                       <ContentTemplate>
                                           <br />
                                           <center>
                                   <table>
                                        <td>Company 
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlCompanyList2" runat="server" OnSelectedIndexChanged="ddlCompanyList2_SelectedIndexChanged" ClientIDMode="Static" AutoPostBack="true" CssClass="form-control select_width" Width="96%" >
                                            </asp:DropDownList>
                                        </td>
                                       <tr>
                                           <td>
                                              Emp Type
                                           </td>
                                           <td>:</td>
                                           <td>
                                               <asp:RadioButtonList AutoPostBack="True" Font-Bold="True" runat="server" OnSelectedIndexChanged="rblEmpType2_SelectedIndexChanged" RepeatDirection="Horizontal" ID="rblEmployeeType2">

                                        </asp:RadioButtonList>
                                           </td>
                                       </tr>
                                       <tr>
                                           <td>Salary Type </td>
                                           <td>:</td>
                                           <td>
                                               <asp:Label  ID="lblSalaryType" runat="server" Font-Bold="True"></asp:Label>
                                           </td>
                                       </tr>
                                         
                                   </table>
                                               <br />
                                               <table border="1">
                                                   <tr>
                                                       <td>Basic<asp:Label runat="server" ID="lblBasic" Font-Bold="True"></asp:Label></td>
                                                      
                                                       <td>
                                                           <asp:TextBox ID="txtBasic" runat="server"  ClientIDMode="Static" CssClass="form-control text_box_width_2" style="text-align:center;font-weight:bold;"></asp:TextBox>
                                                       </td>
                                                   </tr>
                                                    <tr>
                                                       <td>Medical <asp:Label runat="server" ID="lblMedical" Font-Bold="True"></asp:Label> </td>
                                                      
                                                       <td>
                                                           <asp:TextBox ID="txtMedical" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" style="text-align:center;font-weight:bold;"></asp:TextBox>
                                                       </td>
                                                   </tr>
                                                    <tr>
                                                       <td>Food<asp:Label runat="server" ID="lblFood" Font-Bold="True"></asp:Label></td>
                                                      
                                                       <td>
                                                           <asp:TextBox ID="txtFood" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" style="text-align:center;font-weight:bold;"></asp:TextBox>
                                                       </td>
                                                         <tr>
                                                       <td>Convience <asp:Label runat="server" ID="lblConvience" Font-Bold="True"></asp:Label></td>
                                                      
                                                       <td>
                                                           <asp:TextBox ID="txtConvience" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" style="text-align:center;font-weight:bold;"></asp:TextBox>
                                                       </td>
                                                              <tr>
                                                       <td>Techincal <asp:Label runat="server" ID="lblTechnical" Font-Bold="True"></asp:Label></td>
                                                       
                                                       <td>
                                                           <asp:TextBox ID="txtTechnical" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" style="text-align:center;font-weight:bold;"></asp:TextBox>
                                                       </td>
                                                   </tr>
                                                              <tr>
                                                       <td>House <asp:Label runat="server" ID="lblHouse" Font-Bold="True"></asp:Label></td>
                                                       
                                                       <td>
                                                           <asp:TextBox ID="txtHouse" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" style="text-align:center;font-weight:bold;"></asp:TextBox>
                                                       </td>
                                                   </tr>
                                                              <tr>
                                                       <td>Others <asp:Label runat="server" ID="lblOthers" Font-Bold="True"></asp:Label></td>
                                                       
                                                       <td>
                                                           <asp:TextBox ID="txtOthers" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" style="text-align:center;font-weight:bold;"></asp:TextBox>
                                                       </td>
                                                   </tr>
                                                              <tr>
                                                       <td>Provident Fund <asp:Label runat="server" ID="lblPF" Font-Bold="True"></asp:Label> </td>
                                                       
                                                       <td>
                                                           <asp:TextBox ID="txtPF" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width_2" style="text-align:center;font-weight:bold;"></asp:TextBox>
                                                       </td>
                                                   </tr>
                                                              
                                                  
                                               </table>

                                               <br />
                                               <div>
                                   
                                    <asp:Button runat="server" ID="btnSave2" CssClass="Pbutton" Text="Save" Width="100px" OnClick="btnSave2_Click"  />
                                     <asp:Button runat="server" ID="btnClear2" CssClass="Pbutton" Text="Clear" Width="98px" OnClick="btnClear2_Click"  />
                                     
                                </div>

                                               

                                       </center>
                                           <br />
                                            <asp:GridView  runat="server" ID="gvAllowanceAmountList" CssClass="gv1" DataKeyNames="AllownceId,AlCalId,EmpTypeId" AutoGenerateColumns="False" PageSize="25" Width="100%" OnRowCommand="gvSalaryCalculationAmountList_RowCommand" OnRowDataBound="gvSalaryCalculationList_RowDataBound" >
                                            <Columns>
                                              
                                                <asp:TemplateField>
                                                    <HeaderTemplate>SL</HeaderTemplate>
                                                    <ItemTemplate >
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="BasicAllowance" HeaderText="Basic">
                                                 <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="MedicalAllownce" HeaderText="Medical">   

                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="FoodAllownce" HeaderText="Food">
                                                 <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="ConvenceAllownce" HeaderText="Convence">   

                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="TechnicalAllowance" HeaderText="Technical">
                                                 <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="HouseRent" HeaderText="House">   
                                                   
                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                   
                                                <asp:BoundField DataField="OthersAllowance" HeaderText="Others">
                                                <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PFAllowance" HeaderText="PFAllowance">

                                                <ItemStyle Font-Bold="True" HorizontalAlign="Center" />
                                                </asp:BoundField>

                                                <asp:BoundField DataField="EntryDate" HeaderText="Entry Date Time">
                                                 <ItemStyle Font-Bold="True" ForeColor="Blue" HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                 <asp:BoundField DataField="EmpType" HeaderText="Type">   
                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SalaryType" HeaderText="Salary Type" >  
                                                                                                
                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                                                                
                                                <asp:TemplateField HeaderText="Edit">
                                               
                                  <ItemTemplate >
                                      <asp:Button ID="btnEdit" runat="server" CommandName="Alter" Width="55px" Height="30px" Font-Bold="true" ForeColor="green" Text="Edit" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                  </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" Width="30px" />
                              </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="#ffa500" ForeColor="White" Height="28px" />
                                        </asp:GridView>
                                        <br />
                                       </ContentTemplate>
                                   </asp:TabPanel>
            </asp:TabContainer>

                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                   

                </div>
            </div>


        </div>
    </div>
</asp:Content>
