<%@ Page Title="Home" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="SigmaERP._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  <%--  <img class="main_images" src="images/welcome.png" alt="" />--%>
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar" style="border-bottom:none;">
                 <div style="margin-top: 5px">
                     <a href="/default.aspx">Dashboard</a>
                 </div>
            </div>
         </div>
    </div>
    <div style="background: transparent url('../../images/glossy-2.jpg') repeat scroll 0% 0%; position: absolute; width: 100%; left: 0px; height: 1000%;">
    <div class="col-lg-12" style="margin-top: 5%; "> 
    <div class="row">

        <div class=" col-md-3"></div>

        <asp:Panel runat="server" ID="mSettings">
            <div class=" col-md-2">

                <a style="border-bottom: 10px solid #0057AE" class="ds_Settings_Basic_default" href="/hrd_default.aspx">
                    <img class="image_width" src="images/common/settings.ico" />Settings</a>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="mPersonnel">
            <div class=" col-md-2">
                <a style="border-bottom: 10px solid #9E1313" class="ds_Settings_Basic_default" href="/personnel_defult.aspx">
                    <img class="image_width" src="images/common/personnel.ico" />Personnel</a>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" id="mLeave">
            <div class=" col-md-2" >
          <a style="border-bottom:10px solid #008B8B" class="ds_Settings_Basic_default" href="/leave_default.aspx"> <img class="image_width" src="images/common/leave.ico" />Leave</a>
        </div>
        </asp:Panel>
        
        <div class=" col-md-3"></div>
        </div>
    
        <div class="row">
         <div class=" col-md-3"></div>
            <asp:Panel  runat="server" id="mAttendance">
                 <div class=" col-md-2">
            <a style="border-bottom:10px solid #2B5E4E " class="ds_Settings_Basic_default" href="/attendance_default.aspx"> <img class="image_width" src="images/common/attendance.ico" />Attendance</a>
        </div>
            </asp:Panel>
       <asp:Panel  runat="server" id="mPayroll">
      
        <div class=" col-md-2" >
           <a style="border-bottom:10px solid green " class="ds_Settings_Basic_default" href="/payroll_default.aspx"> <img class="image_width" src="images/common/payroll.ico" />Payroll</a>
        </div>
            </asp:Panel>
            <asp:Panel runat="server" id="mTools">
                <div class=" col-md-2" >
          <a style="border-bottom:10px solid blue " class="ds_Settings_Basic_default" href="/tools_default.aspx"> <img class="image_width" src="images/common/tools.png" />Tools</a>
        </div>
            </asp:Panel>
        
        <div class=" col-md-3"></div>
    </div>
        <h3 style="text-align: center; margin-top: 200px; font-weight: bold; color: green; text-shadow: 3px 5px 1px rgb(126, 171, 52); font-size: 75px; font-family: initial;">OHRM</h3>
        </div>
       
        </div>
    <script type="text/javascript" >
        function c()
        {
            location.href = '/hrd_default.aspx';
        }
    </script>

</asp:Content>
