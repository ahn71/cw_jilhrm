<%@ Page Title="Bonus Decrease/Increase" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="bonus_increase_decrease.aspx.cs" Inherits="SigmaERP.payroll.bonus_decrease" %>
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
        .gv th {
            text-align:center;
        }
          .gv th:nth-child(3),td:nth-child(3){
              padding-left:3px;
            text-align:left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dasboard</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="/payroll_default.aspx">Payroll</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li> <a href="/payroll/bonus_index.aspx" >Bouns</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="#" class="ds_negevation_inactive Pactive">Bonus Increase and Decrease</a></li>
                </ul>
            </div>
        </div>
    </div> 
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>


    <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Bonus Increase and Decrease</h2>
        </div>
    	<div class="main_box_body Pbody">
            <div class="main_box_content">

        <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" />
            <asp:AsyncPostBackTrigger ControlID="ddlCompanyList" />
          
            <asp:AsyncPostBackTrigger ControlID="ddlBonusType" />
        </Triggers>
        <ContentTemplate>
             <div class="bonus_generation" style="width: 98%; margin: 0px auto;">               
                    <h1  runat="server" visible="false" id="WarningMessage"  style="color:red; text-align:center">You Have Not Any Access Permission!</h1>
                    <table runat="server" visible="true" id="tblGenerateType" class="division_table_leave1"> 
                <tr>

                    <td>Company :</td>
                    <td>
                        <asp:DropDownList ID="ddlCompanyList" runat="server" AutoPostBack="true" CssClass="form-control select_width" OnSelectedIndexChanged="ddlCompanyList_SelectedIndexChanged" ></asp:DropDownList>
                    </td>
                     <td>Bonus Type</td>

                    <td>
                        <asp:DropDownList ID="ddlBonusType" runat="server" ClientIDMode="Static" CssClass="form-control select_width" AutoPostBack="True" OnSelectedIndexChanged="ddlBonusType_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="Pbutton" Style="float: right; width: 80px;" OnClick="btnSearch_Click" /><br />
                    </td>
                    <td>
                      <asp:TextBox ID="txtEmpCardNo" runat="server"  ClientIDMode="Static" PlaceHolder="For Certain Employee" CssClass="form-control text_box_width_import"  ></asp:TextBox>
                    </td>
                </tr>
            </table>
          
             </div>
             <hr />
            <br />
              <asp:GridView ID="gvBonusList" CssClass="gv" runat="server"  AutoGenerateColumns="false" HeaderStyle-Height="26px" HeaderStyle-Font-Bold="false" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="#ffa500" DataKeyNames="SL" Width="100%" OnRowCommand="gvBonusList_RowCommand" OnRowDataBound="gvBonusList_RowDataBound"  >
            <Columns>  
                   <asp:TemplateField HeaderText="SL">
                                <ItemTemplate>
                                     <%# Container.DataItemIndex + 1 %>                                  
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>               
                <asp:BoundField DataField="EmpCardNo" HeaderText="EmpCardNo" Visible="true" ItemStyle-Width="100px" itemstyle-horizontalalign="center" ItemStyle-Height="26px" />
                <asp:BoundField DataField="EmpName" HeaderText="Name" Visible="true"  ItemStyle-Height="26px" />
                <asp:BoundField DataField="DsgName" HeaderText="Designation" Visible="true" itemstyle-horizontalalign="center" />
                <asp:BoundField DataField="EmpJoiningDate" HeaderText="JOD" Visible="true" ItemStyle-Width="100px"  itemstyle-horizontalalign="center" ItemStyle-Font-Bold="false" ItemStyle-ForeColor="black"/>
                 <asp:BoundField DataField="PresentSalary" HeaderText="Gross" Visible="true" ItemStyle-Width="100px" itemstyle-horizontalalign="center" />
               
                <asp:BoundField DataField="BasicSalary" HeaderText="Basic" Visible="true"  itemstyle-horizontalalign="center" />
                <asp:BoundField DataField="BonusAmount" HeaderText="Bonus" Visible="true"  itemstyle-horizontalalign="center" ItemStyle-ForeColor="green" ItemStyle-Font-Bold="true" />

                <asp:BoundField DataField="Percentage" HeaderText="Bonus(%)" Visible="true" itemstyle-horizontalalign="center" />
 
                 <asp:TemplateField HeaderText="Amount">
                    <ItemTemplate>
                        <asp:TextBox ID="txtInDe_Crease" runat="server" Enabled="false" Text="0" Font-Bold="true" ForeColor="black" Height="20px" Width="65px" style="text-align: center; margin-left: 2px; autocomplete:off"  ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                      <asp:TemplateField ItemStyle-Width="100px" itemstyle-horizontalalign="center" HeaderText="Alter" >
                   <ItemTemplate>
                       <asp:Table runat="server">
                           <asp:TableRow>
                               <asp:TableCell>
                                   <asp:Button ID="btnIncrease" Text="Increase" runat="server" CommandName="Increase" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" Height="27px" Width="71px" Font-Bold="true" ForeColor="Green"  /> 
                               </asp:TableCell>

                               <asp:TableCell>|</asp:TableCell>

                               <asp:TableCell >
                                   <asp:Button ID="btnDecrease" Text="Decrease"  runat="server" CommandName="Decrease" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" Height="27px" Width="77px" Font-Bold="true" ForeColor="red"  /> 
                               </asp:TableCell>
                           </asp:TableRow>

                         
                       </asp:Table>
                       
                   </ItemTemplate>
                </asp:TemplateField>


            </Columns>
        </asp:GridView>     

             </ContentTemplate>
    </asp:UpdatePanel>

                    </div>
                </div>
            </div>
</asp:Content>
