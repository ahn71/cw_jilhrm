<%@ Page Title="" Language="C#" MasterPageFile="~/Tools_Nested.master" AutoEventWireup="true" CodeBehind="ModifyPage.aspx.cs" Inherits="SigmaERP.ModifyPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:Button runat="server"  ID="btnModify" Text="Modify" OnClick="btnModify_Click"/>
</asp:Content>
