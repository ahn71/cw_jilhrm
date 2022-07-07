<%@ Page Title="Todays Attendance Report" Language="C#" MasterPageFile="~/attendance_nested.master" AutoEventWireup="true" CodeBehind="todays_attStatus_report.aspx.cs" Inherits="SigmaERP.attendance.todays_attStatus_report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .buttonArea {
            border: 1px solid silver;
            margin: 20px auto;
            width: 217px;
        }
        .Bnt_table {
            padding-top:25px;
            width: 100%;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-md-12">
            <div class="ds_nagevation_bar">
                <ul>
                    <li><a href="/default.aspx">Dashboard</a></li>
                    <li>/</li>
                    <li><a href="/attendance_default.aspx">Attendance</a></li>
                    <li>/</li>
                    <li><a href="#" class="ds_negevation_inactive Mactive">Todays Attendance Status</a></li>
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
    <div>
        <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnStatus" />
                        <asp:AsyncPostBackTrigger ControlID="btnPresentStatus" />
                          <asp:AsyncPostBackTrigger ControlID="btnAbsentStatus" />                        
                    </Triggers>
                    <ContentTemplate>
        <div class="buttonArea" >
            <h2  runat="server"  id="WarningMessage" visible="false"  style="color:red; text-align:center">You Have Not Any Access Permission!</h2>
                        <table class="Bnt_table">                     
                              <tr>
                                    <td >
                                        <asp:Button ID="btnStatus" CssClass="Mbutton" runat="server" Width="200px" Text="Todays All Status" OnClick="btnStatus_Click"  />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnPresentStatus" Class="Mbutton" runat="server" Width="200px"  Text="Todays Present Status" OnClick="btnPresentStatus_Click" />
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnAbsentStatus" Class="Mbutton" runat="server" Width="200px"  Text="Todays Absent Status" OnClick="btnAbsentStatus_Click"/>
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnLateStatus" Class="Mbutton" runat="server" Width="200px"  Text="Todays Late Status" OnClick="btnLateStatus_Click"/>
                                    </td>
                               </tr>
                            <tr>
                                    <td>
                                        <asp:Button ID="btnClose" Class="Mbutton" runat="server" Width="200px"  Text="Close" PostBackUrl="/attendance_default.aspx" />
                                    </td>
                               </tr>
                        </table>
           <div  style="padding:10px">
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" ClientIDMode="Static" DisplayAfter="100">
                            <ProgressTemplate>
                                <span style="font-family: 'Times New Roman'; font-size: 20px; margin-top: -14px; color: green; font-weight: bold; float: left">
                                    <p>Wait Generating&nbsp;</p>
                                </span>
                                <img style="width: 26px; height: 26px; cursor: pointer; float: left; margin-top: -13px;" src="/images/wait.gif" />
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                       </div>
                    </div>
                        
                        </ContentTemplate>
                        </asp:UpdatePanel>

   </div>
     <script type="text/javascript">
        

        function CloseWindowt() {
            window.close();
        }

        function goToNewTabandWindow(url) {
            window.open(url);

        }

     </script>

</asp:Content>
