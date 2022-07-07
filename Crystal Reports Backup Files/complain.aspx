<%@ Page Title="" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="complain.aspx.cs" Inherits="SigmaERP.mail.complain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../style/jquery-ui.min.css" rel="stylesheet" />
    <script src="../scripts/jquery-ui.js"></script>
    <script src="../scripts/jquery-ui.min.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>
    
    <script type="text/javascript">
        $(function () {
            $('#<%=txtTo.ClientID%>').autocomplete({
                source: function (request, response)
                {
                    var serverURL = window.location.protocol + "//" + window.location.host + "/";
                   
                    $.ajax({
                        url: serverURL + "others/user_info.asmx/getUserInfo",
                        data: "{'getFirstName':'" + request.term + "'}",
                        type:"POST",
                        dataType: 'json',
                        contentType: "application/json;charset=utf-8",
                        success: function (data)
                        {
                            response(data.d);
                        },
                        error: function (result)
                        {
                            alert("sorry"+result);
                        }
                    });                   
                }
                
            });
        });
    </script>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">

                            <Triggers>
                                
                             

                            </Triggers>
                            <ContentTemplate>

              <div class="main_box">
    	<div class="main_box_header">
            <h2 id="h2Title" runat="server">Complain</h2>
        </div>
    	<div class="main_box_body">
        	<div class="main_box_content">
                <div class="input_division_info"  style="margin-left:5px" >
                    <table  >
                        <tr id="trCompany" runat="server">
                            
                             <td>Module</td>
                                        <td>:</td>
                                        <td>
                                        <asp:DropDownList ID="ddlModuleType" ClientIDMode="Static"   CssClass="form-control select_width" runat="server" Width="400px"  >              
                                            <asp:ListItem>Personnel</asp:ListItem>
                                            <asp:ListItem>Leave</asp:ListItem>
                                            <asp:ListItem>Attendance</asp:ListItem>
                                            <asp:ListItem>Payroll</asp:ListItem>
                                         </asp:DropDownList>
                                        </td>

                        </tr>
                        <tr>
                        <td>To</td>
                                        <td>:</td>
                                        <td>
                                        <asp:DropDownList ID="ddlTo" ClientIDMode="Static"   CssClass="form-control select_width" runat="server" Width="400px"  >              
                                            <asp:ListItem>Personnel</asp:ListItem>
                                            <asp:ListItem>Leave</asp:ListItem>
                                            <asp:ListItem>Attendance</asp:ListItem>
                                            <asp:ListItem>Payroll</asp:ListItem>
                                         </asp:DropDownList>
                                         <asp:TextBox ID="txtTo" Visible="false"  runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Width="704px" Font-Bold="True"></asp:TextBox>
                                        </td>

                        </tr>

                        <tr>
                            <td>
                                Subject</td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubject" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Width="704px" Font-Bold="True"></asp:TextBox>
                            </td>

                        </tr>
                          
                         <tr>
                            <td  style="vertical-align: top;">
                                Details 
                            </td>
                             <td>:</td>
                            <td>
                                <asp:TextBox ID="txtBody" runat="server" ClientIDMode="Static" CssClass="form-control text_box_width" Height="463px" TextMode="MultiLine" Width="783px"  style="height: 463px; margin-left: 11px; width: 707px;"></asp:TextBox>
                               
                 
                                <asp:HtmlEditorExtender  ID="txtBody_HtmlEditorExtender" runat="server" TargetControlID="txtBody" EnableSanitization="false" >
                                </asp:HtmlEditorExtender>
                            </td>
                        </tr>
                            
                        
                    </table>

                </div>

               <br />
                <div  style="margin: 10px 0px 0px 57px;">
                    <table >
                        <tr>
                           
                            <td>
                                <asp:Button ID="btnSave" ClientIDMode="Static" CssClass="css_btn"  runat="server" Text="Submit" OnClick="btnSave_Click" />
                            </td>
                             <td>
                                <asp:Button ID="btnClear" ClientIDMode="Static" CssClass="css_btn"  runat="server" Text="Clear" OnClick="btnClear_Click"/>
                            </td>
                            
                             <td> <asp:Button ID="btnClose" ClientIDMode="Static" CssClass="css_btn" PostBackUrl="~/hrd_default.aspx"  runat="server" Text="Close" /></td>
                           
                            <td>
                                <asp:Button ID="btnBack" ClientIDMode="Static" CssClass="css_btn"  runat="server" Text="Back" OnClick="btnBack_Click" />
                            </td>
                        </tr>
                            
                    </table>
                </div>
              

				
            </div>
        </div>
    </div>
                                </ContentTemplate>
                </asp:UpdatePanel>
</asp:Content>
