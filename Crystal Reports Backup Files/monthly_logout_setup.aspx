<%@ Page Title="" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="monthly_logout_setup.aspx.cs" Inherits="SigmaERP.attendance.monthly_logout_setup" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <%--<style type="text/css">
        .leftBox {
        margin-top: -25px;
        border-color: #808080;
        border-style: solid;
        border-width: 1px;
        float: left;
        height:67px;
        width: 681px;
        }
        .rightBox {
        border-color: #808080;
        border-style: solid;
        border-width: 1px;
        float: left;
        height: 462px;
        width: 681px;
        }
    </style>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="row">
         <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li>/</li>
                    <li><a href="/attendance_default.aspx">Attendance</a></li>
                    <li>/</li>
                    <li><a href="#" class="ds_negevation_inactive Mactive">Daily Logout Setup</a></li>
                </ul>
            </div>
        </div>
    </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>

    <div class="main_box Mbox">
                <div class="main_box_header MBoxheader">
                    <h2>Monthly Logout Time And OT Setting</h2>
                </div>
                <div class="punishment_bottom_header" style="width: 722px;">
                    
                    
                </div>
                <div class="employee_box_body">

                    <div class="employee_box_content">
                        
        <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="dlSelectMonth" />
           <%-- <asp:AsyncPostBackTrigger ControlID="dlExistsUser" />--%>
        </Triggers>
        <ContentTemplate>
             <div class="bonus_generation" style="width:55%; margin:0px auto;">
           
            <div class="leftBox">
                    
                     <div style="margin-left: 87px; margin-top: 9px;">
                         <asp:Button ID="btnPopup" runat="server" style="display:none" Text="Close"   CssClass="css_btn" />
                     </div>        

                     <table class="division_table_leave1">
                         <tr>
                             <td>
                                 Select Month :
                             </td>
                             <td>
                                 <asp:DropDownList ID="dlSelectMonth" Width="131px" runat="server" ClientIDMode="Static"  CssClass="form-control select_width"  AutoPostBack="True" OnSelectedIndexChanged="dlSelectMonth_SelectedIndexChanged">
                               </asp:DropDownList>
                             </td>
                             
                             <td>
                                 <asp:CheckBox runat="server" ID="chkDecreace"/>
                                 <asp:Label runat="server" ID="lblText" for="chkDecreace" Text="Decrease overtime for overall of this month : " style="cursor: default;" ></asp:Label>
                             </td>
                             <td>
                                  <asp:TextBox runat="server" ID="txtDecreaseAmount" Text="00" Width="44px"  style="width: 44px; text-align: center; font-weight: bold; font-size: 15px;" MaxLength="2"></asp:TextBox>
                             </td>
                         </tr>
                     </table>
                <br />
                <asp:Label ID="lblStatus" runat="server" Font-Bold="True"></asp:Label>
                 </div>
              
   
            <div class="rightBox">       
                       
                <div runat="server" id="divPagelist" visible="true" style ="height:461px; width:681px; overflow:scroll"> 
                <asp:GridView runat="server" ID="gvDateList"  AutoGenerateColumns="false" DataKeyNames="SL"  HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" >
                    <AlternatingRowStyle BackColor="#DCDCDC" />
                  <Columns>
                        <asp:BoundField DataField="MonthDate" HeaderText="Date" ItemStyle-Width="469px" />

                      <asp:TemplateField HeaderText="Chosen" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="55px"  >
                            <ItemTemplate>
                                <asp:CheckBox ID="chkChosen" runat="server" HeaderText="Chosen"
                                    Checked='<%#bool.Parse(Eval("Chosen").ToString())%>' AutoPostBack="true"  />
                            </ItemTemplate>
                               <ItemStyle HorizontalAlign="Center" Width="55px" />
                        </asp:TemplateField>

                        
                     <asp:TemplateField HeaderText="OutHour" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                            <ItemTemplate>
                               <asp:TextBox ID="txtLogoutHour" runat="server" TextMode="SingleLine" Text='<%#(Eval("LogoutHour").ToString())%>' style="text-align:center; width:56px"   ></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                        </asp:TemplateField>

                       <asp:TemplateField HeaderText="OutMin" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="45px">
                            <ItemTemplate>
                               <asp:TextBox ID="txtLogoutMin" runat="server" TextMode="SingleLine" Text='<%#(Eval("LogoutMin").ToString())%>' style="text-align:center;width:57px" ></asp:TextBox>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="45px" />
                        </asp:TemplateField>

                           <asp:TemplateField HeaderText="N.OT.H." ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" >
                            <ItemTemplate>
                               <asp:TextBox ID="txtNormallyOTHour" runat="server" TextMode="SingleLine" Text='<%#(Eval("NormallyOTHour").ToString())%>' style="text-align:center;width:57px" ></asp:TextBox>
                            </ItemTemplate>
                               <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>
                       
                          
                       
                           
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#0000A9" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#000065" />
                </asp:GridView>
                </div>

                <asp:Button runat="server" ID="btnSet" Text="Set" Width="95px" style=" margin-top: 5px;" OnClick="btnSet_Click" />
                 <asp:Button runat="server" ID="btnDelete" Text="Delete" Width="95px" style=" margin-top: 5px;" OnClick="btnDelete_Click" />
            </div>
                  
           



                    </div>


      </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </div>
    </div>

</asp:Content>
