<%@ Page Title="" Language="C#" MasterPageFile="~/payroll_nested.Master" AutoEventWireup="true" CodeBehind="bdt_note_set.aspx.cs" Inherits="SigmaERP.hrd.bdt_note_set" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

     <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
    </asp:UpdatePanel>

    <div class="create_account_main_box" style="width: 658px;">
                <div class="employee_box_header">
                    <h2>Bangladeshi Notes List</h2>
                </div>
                
                <div class="employee_box_body" style="width: 634px;">

                    <div class="create_account_content">

        <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSet" />
             <asp:AsyncPostBackTrigger ControlID="btnReset" />
        </Triggers>
        <ContentTemplate>

                <asp:GridView runat="server" Width="200px" DataKeyNames="SL" CssClass="display" ID="gvNoteList" HeaderStyle-BackColor="Black" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="15 px" HeaderStyle-ForeColor="White" AutoGenerateColumns="false" OnRowDataBound="gvNoteList_RowDataBound" >
            <Columns>
                <asp:BoundField  HeaderStyle-HorizontalAlign="Center" DataField="Note" HeaderText="BDT Note" ItemStyle-Width="100px" ItemStyle-Font-Bold="true" ItemStyle-HorizontalAlign="Center" />

                 <asp:TemplateField  HeaderStyle-HorizontalAlign="Center" AccessibleHeaderText="Choose" HeaderText="Chosen" ItemStyle-Width="30px" itemstyle-horizontalalign="center">
                    <ItemTemplate>
                        <asp:CheckBox ID="SelectCheckBox" runat="server" ItemStyle-Width="50px" Checked='<%#bool.Parse(Eval("Chosen").ToString())%>'   />
                        
                        
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
            <div Style="float: left; margin-left: 224px; margin-top: 26px;">
                <asp:Button id="btnReset" runat="server" Text="Reset" CssClass="css_btn"  OnClick="btnReset_Click" />
             <asp:Button id="btnSet" runat="server" Text="Set" CssClass="css_btn"  OnClick="btnSet_Click" /><br />
            </div>

             </ContentTemplate>
    </asp:UpdatePanel>

                    </div>
                </div>
            </div>
  
</asp:Content>
