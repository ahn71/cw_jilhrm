<%@ Page Title="Tools" Language="C#" MasterPageFile="~/Tools_Nested.master" AutoEventWireup="true" CodeBehind="tools_default.aspx.cs" Inherits="SigmaERP.tools_defaults" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar" style="border-bottom:none;">
                <div style="margin-top: 5px">
                    <ul>
                        <li><a href="/default.aspx">Dasboard</a></li>
                        <li>/</li>
                        <li><a href="#" class="ds_negevation_inactive Tactive">Tools</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>

    <%--  <img alt="" class="main_images" src="images/hrd.png">--%>
    <div class="main-bg">
    <div class="col-lg-12" style="margin-top: 10%">
        <div class="row">

            <div class=" col-md-2"></div>

            <div class="col-md-2">
                <a class="ds_Settings_Basic_Text Tbox" href="/ControlPanel/CreateAccount.aspx">
                    <img class="image_width_for_module" src="images/common/department.ico" /><br />
                    User Account</a>

            </div>
            <div class=" col-md-2">
                <a class="ds_Settings_Basic_Text Tbox" href="/ControlPanel/changepassword.aspx">
                    <img class="image_width_for_module" src="images/common/designation.ico" /><br />
                    C. Password</a>
            </div>
            <div class=" col-md-2">
                <a class="ds_Settings_Basic_Text Tbox" href="/ControlPanel/user_privilege.aspx">
                    <img class="image_width_for_module" src="images/common/grade.ico" /><br />
                    User Privilege</a>
            </div>
            <div class=" col-md-2">
                <a class="ds_Settings_Basic_Text Tbox" href="/ControlPanel/authority-access-control.aspx">
                    <img class="image_width_for_module" src="images/common/Class Schedule.ico" /><br />
                    Authority Access Controle</a>
            </div>
          <%--  <div class=" col-md-2">
                <a class="ds_Settings_Basic_Text Tbox" href="/hrd/shift_config.aspx">
                    <img class="image_width_for_module" src="images/common/Class Schedule.ico" /><br />
                    Db Backup</a>
            </div>--%>
             <%--<div class=" col-md-2">
                <a class="ds_Settings_Basic_Text Tbox" href="/ModifyPage.aspx">
                    <img class="image_width_for_module" src="images/common/Class Schedule.ico" /><br />
                   Modify</a>
            </div>--%>
            <div class=" col-md-2"></div>
        </div>
    </div></div>
</asp:Content>
