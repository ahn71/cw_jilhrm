<%@ Page Title="Bonus Month Setup" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="bonus_monyh_setup.aspx.cs" Inherits="SigmaERP.payroll.bonud_monyh_setup" %>

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
    <style type="text/css">
        .leftBox {
            margin-top: -25px;
            border-color: #808080;
            border-style: solid;
            border-width: 1px;
            float: left;
            height: 115px;
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

        #ContentPlaceHolder1_ContentPlaceHolder1_tc1_tab1_gvBonusMonthList {
            width: 90%;
            margin: 0px auto;
        }
        #ContentPlaceHolder1_ContentPlaceHolder1_tc1_tab1_gvBonusMonthList th:first-child,th:nth-child(2), td:first-child,td:nth-child(2) {
            padding-left:3px;
        }
        #ContentPlaceHolder1_ContentPlaceHolder1_tc1_tab1_gvBonusMonthList th:nth-child(3),th:nth-child(4), td:nth-child(3),td:nth-child(4) {
            text-align:center;
        }
        #ContentPlaceHolder1_ContentPlaceHolder1_tc1_tab2_gvSetupedList th:first-child,th:nth-child(3) {
            text-align:center;
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
                    <li><a href="/payroll/bonus_index.aspx">Bouns</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="#" class="ds_negevation_inactive Pactive">Bouns Month Setup</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="main_box Mbox">
        <div class="main_box_header PBoxheader">
            <h2>Bonus Month Setup</h2>
        </div>
        <div class="punishment_bottom_header">
        </div>
        <div class="main_box_body Pbody">
            <div class="main_box_content">

                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <%--<asp:AsyncPostBackTrigger ControlID="dlSelectBonusYearAndType" />--%>
                        <%--<asp:AsyncPostBackTrigger ControlID="ddlComapnyList" />--%>
                    </Triggers>
                    <ContentTemplate>

                        <asp:TabContainer ID="tc1" runat="server" CssClass="fancy fancy-green" AutoPostBack="true" OnActiveTabChanged="tc1_ActiveTabChanged" ActiveTabIndex="0">
                            <asp:TabPanel ID="tab1" runat="server" TabIndex="0">
                                <HeaderTemplate>Bonus Month Setup</HeaderTemplate>
                                <ContentTemplate>
                                    <div class="leftBox" style="width: 100%">
                                        <div style="margin-left: 87px; margin-top: 9px;">
                                            <asp:Button ID="btnPopup" runat="server" Style="display: none" Text="Close" CssClass="css_btn" />
                                        </div>
                                        <br />
                                        <table style="margin: 0px auto">
                                            <tr>
                                                <td>Select Company<span class="requerd1">*</span> :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlComapnyList" runat="server" ClientIDMode="Static" CssClass="form-control select_width" AutoPostBack="True" Style="width: 265px;" OnSelectedIndexChanged="ddlComapnyList_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td>Select Bonus Name<span class="requerd1">*</span> :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="dlSelectBonusYearAndType" runat="server" ClientIDMode="Static" CssClass="form-control select_width" AutoPostBack="True" OnSelectedIndexChanged="dlSelectBonusYearAndType_SelectedIndexChanged" Style="width: 265px;">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>

                                        
                                    </div>
                                    <br />
                                    <div class="rightBox" style="width: 100%">
                                        <asp:Label ID="lblStatus" Style="margin-left: 131px;" runat="server" Font-Bold="True"></asp:Label>
                                        <asp:GridView runat="server" ID="gvBonusMonthList" AutoGenerateColumns="False" DataKeyNames="SL" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" OnRowDataBound="gvSetupedList_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="SlabType" HeaderText="Slab Type">
                                                    <ItemStyle Width="469px" Font-Bold="True" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="EquivalentMonth" HeaderText="Equivalent Month">
                                                    <ItemStyle Width="200px" Font-Bold="True" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Chosen">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkChosen" runat="server" HeaderText="Chosen"
                                                            Checked='<%#bool.Parse(Eval("Chosen").ToString())%>' AutoPostBack="true" ItemStyle-Font-Bold="true" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="55px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Per.(%)">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPercentage" runat="server" TextMode="SingleLine" Text='<%#(Eval("Percentage").ToString())%>' Style="text-align: center; width: 56px; font-weight: bold; color: red;" AutoComplete="off" MaxLength="3"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                </asp:TemplateField>




                                            </Columns>
                                            <FooterStyle BackColor="#CCCCCC" />
                                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                                            <RowStyle BackColor="White" />
                                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                            <SortedAscendingHeaderStyle BackColor="Gray" />
                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                            <SortedDescendingHeaderStyle BackColor="#383838" />
                                        </asp:GridView>
                                        <table id="tblGenreateType" runat="server" visible="False" style="margin: 0px auto; font-weight: bold;">
                                            <tr runat="server">
                                                <td runat="server">Bonus Generate On :
                                                </td>
                                                <td runat="server">
                                                    <asp:RadioButtonList ID="rblGenerateType" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="Gross Salary">Gross Salary</asp:ListItem>
                                                        <asp:ListItem Selected="True">Basic Salary</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr runat="server">
                                                <td style="border-bottom: 1px solid #ccc;" runat="server"></td>
                                                <td style="border-bottom: 1px solid #ccc;" runat="server"></td>
                                            </tr>

                                        </table>


                                        <div>
                                            <div style="width: 195px; margin: 0px auto; margin-top: 12px;">
                                                <asp:Button runat="server" ID="btnSet" Text="Set" Width="95px" Style="margin-top: 5px;" OnClick="btnSet_Click" />
                                                <asp:Button runat="server" ID="btnDelete" Text="Delete" Width="95px" Style="margin-top: 5px;" OnClientClick="return confirm('Are you sure ? ');" OnClick="btnDelete_Click" />
                                            </div>
                                        </div>





                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tab2" TabIndex="1" Height="550px">
                                <HeaderTemplate>Bonus Month Setup List</HeaderTemplate>
                                <ContentTemplate>
                                    <div>
                                        <asp:GridView RowStyle-Height="28px" runat="server" Width="100%" ID="gvSetupedList" AutoGenerateColumns="False" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellPadding="4" Style="margin-top: 3px; padding-top: 15px;" CellSpacing="2" ForeColor="Black" OnRowDataBound="gvSetupedList_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="50px">
                                                    <HeaderTemplate>SL</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                    <ItemStyle ForeColor="Red" Font-Bold="true" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="BonusType" HeaderText="Bonus Title" ItemStyle-Width="200px" ItemStyle-Font-Bold="true">
                                                    <ItemStyle Width="469px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="SetupedDate" HeaderText="Setup Date" ItemStyle-Width="100px" ItemStyle-Font-Bold="true">
                                                    <ItemStyle Width="200px" HorizontalAlign="Center" />
                                                </asp:BoundField>

                                            </Columns>
                                            <FooterStyle BackColor="#CCCCCC" />
                                            <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                                            <RowStyle BackColor="White" />
                                            <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                            <SortedAscendingHeaderStyle BackColor="#808080" />
                                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                            <SortedDescendingHeaderStyle BackColor="#383838" />
                                        </asp:GridView>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>
