<%@ Page Title="" Language="C#" MasterPageFile="~/payroll_nested.master" AutoEventWireup="true" CodeBehind="AdvanceDeductionMonthList.aspx.cs" Inherits="SigmaERP.payroll.AdvanceDeductionMonthList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <style>
        #ContentPlaceHolder1_ContentPlaceHolder1_gvAdvaceList th, td {
            text-align:center;
        }
           #ContentPlaceHolder1_ContentPlaceHolder1_gvAdvaceList th:nth-child(2), td:nth-child(2) {
            text-align:left;
            padding-left:3px;
        }
            .modalBackground
    {
        background-color: Black;
        filter: alpha(opacity=90);
        opacity: 0.8;
    }
    .modalPopup
    {
        background-color: #FFFFFF;
        border-width: 3px;
        border-style: solid;
        border-color: black;
        width: 300px;
        height: 140px;
    }

    .modalPopup .header {
      background-color: #0090cb;
      color: White;
      font-size: 16px;
      margin: -3px -3px 10px;
      padding: 5px 5px;
      text-align: left;
    }
    .modalPopup .popupClose{
      position: absolute;
      top: 0;
      right: 0;
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
                    <li><a href="/payroll/advance_index.aspx">Advance</a></li>
                    <li><a class="seperator" href="#">/</a></li>
                    <li><a href="#" class="ds_negevation_inactive Pactive">Advance Settings</a></li>
                </ul>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="main_box Mbox">

        <div class="main_box_header PBoxheader">
            <h2>
                <asp:Label runat="server" ID="lblTtile">Advance Deduction Month List</asp:Label>
                &nbsp;View</h2>
        </div>

        <div class="main_box_body Pbody">
            <div class="main_box_content">

                <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                    <Triggers></Triggers>
                    <ContentTemplate>

                        <br />



                        <asp:GridView ID="gvAdvaceList" runat="server" AutoGenerateColumns="False" DataKeyNames="AdvanceId" Width="100%" OnRowCommand="gvAdvaceList_RowCommand">
                            <Columns>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Bind("AdvanceId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" Text="Deduction Months" runat="server" CommandName="ADML" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" Height="27px" Width="124px" Font-Bold="false" ForeColor="Green" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="125px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="EmpCardNo" HeaderText="EmpCardNo">
                                    <ItemStyle Height="26px" HorizontalAlign="Center" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EmpName" HeaderText="Name">
                                    <ItemStyle Height="26px" HorizontalAlign="Left" Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AdvanceAmount" HeaderText="Advance">
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="InstallmentNo" HeaderText="Ins.No.">
                                    <ItemStyle Font-Bold="True" ForeColor="Red" HorizontalAlign="Center" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="InstallmentAmount" HeaderText="Ins.Amount">

                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="StartMonth" HeaderText="StartM.">
                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EntryDate" HeaderText="Date">

                                    <ItemStyle HorizontalAlign="Center" Width="100px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="EmpType" HeaderText="EmpType">

                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>

                                <asp:BoundField DataField="PaidInstallmentNo" HeaderText="P.Ins.No">

                                    <ItemStyle Font-Bold="True" ForeColor="Green" HorizontalAlign="Center" />
                                </asp:BoundField>

                            </Columns>
                            <HeaderStyle BackColor="Orange" Font-Bold="False" ForeColor="White" Height="26px" />
                        </asp:GridView>

                        <input type="button" id="btnShow" runat="server" value="ClickME" style="display:none" />

                        <asp:ModalPopupExtender runat="server" ID="mpe1" PopupControlID="Panel1" TargetControlID="btnShow"
                            CancelControlID="btnClose" BackgroundCssClass="modalBackground">
                        </asp:ModalPopupExtender>

                        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" Style="display: none">
                            <div class="header">
                                Installment Paid Month List
                            </div>
                            <asp:GridView ID="gvSettingsMonths" AutoGenerateColumns="false" HeaderStyle-BackColor="#FFA500" HeaderStyle-Height="28px" runat="server">
                               <RowStyle HorizontalAlign="Center" />
                                 <Columns>
                                    <asp:BoundField DataField="InstallmentAmount" HeaderText="Ins. Amount"></asp:BoundField>
                                    <asp:BoundField DataField="PaidInstallmentNo" HeaderText="Paid Ins.Amount" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ></asp:BoundField>
                                    <asp:BoundField DataField="PaidMonth" HeaderText="Paid Month"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                          
                            <asp:ImageButton ID="btnClose" runat="server" CssClass="popupClose" ImageUrl="~/images/icon/close03.png" />
                        </asp:Panel>
                        <asp:Label runat="server" ID="Label1"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>


    </div>

</asp:Content>
