<%@ Page Title="Joining Shift Date" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="joining_shift_date.aspx.cs" Inherits="SigmaERP.attendance.joining_shift_date" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>


    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <Triggers>
        </Triggers>
        <ContentTemplate>

            <div class="main_box">
                <div class="main_box_header">
                    <h2>Joining Shift Date</h2>
                </div>
                <div class="main_box_body">
                    <div class="main_box_content">
                        <div class="job_card_button_area">
                            <asp:Button ID="btnPreview" CssClass="css_btn" runat="server" Text="Preview"/>
                            
                            &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="Button3" runat="server" Text="Close" PostBackUrl="~/default.aspx" CssClass="css_btn" />
                        </div>
                    </div>
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
