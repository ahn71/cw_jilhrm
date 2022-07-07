<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_profile.aspx.cs" Inherits="SigmaERP.others.user_profile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
       <center>
    <div >
      <div style="font-family:Arial ; margin-top:110px; font-weight:bold">
          <label id="lblCompanyTitle" runat="server" ></label><br />
          <label id="lblAddress" runat="server" ></label>
      </div>
        <br />
        <div>
            <asp:Image ID="imgUserImage" runat="server"  Width="199px" Height="190px"></asp:Image>
        </div>
        <br />

        <div>
            <table>
                <tr>
                    <td>
                        Name :
                    </td>
                    <td>:</td>
                    <td>
                        <label id="lblName" runat="server"></label>
                    </td>
                </tr>

                <tr>
                    <td>
                        Card No.
                    </td>
                    <td>:</td>
                    <td>
                        <label id="lblCardNo" runat="server"></label>
                    </td>
                </tr>
                <tr>
                    <td>
                        User Type 
                    </td>
                    <td>:</td>
                    <td>
                        <label id="lblUserType" runat="server"></label>
                    </td>
                </tr>

                <tr>
                    <td>
                        Shift 
                    </td>
                    <td>:</td>
                    <td>
                        <label id="lblShift" runat="server"></label>
                    </td>
                </tr>
                  <tr>
                    <td>
                        Department 
                    </td>
                    <td>:</td>
                    <td>
                        <label id="lblDepartment" runat="server"></label>
                    </td>
                </tr>

                  <tr>
                    <td>
                        Designation 
                    </td>
                    <td>:</td>
                    <td>
                        <label id="lblDesignation" runat="server"></label>
                    </td>
                </tr>
                
                  <tr>
                    <td>
                        Joining Date 
                    </td>
                    <td>:</td>
                    <td>
                        <label id="lblJoiningDate" runat="server"></label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
           </center>
    </form>
</body>
</html>
