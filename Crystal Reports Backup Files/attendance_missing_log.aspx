<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="attendance_missing_log.aspx.cs" Inherits="SigmaERP.attendance.attendance_missing_log" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>

<body>
    <form id="form1" runat="server">
  
        <h2>Attendance Missing Log</h2>
        <div class="dataTables_wrapper">
                    <asp:GridView ID="gvAttMissingLog" runat="server" style="font-size:13px" AutoGenerateColumns="False" DataKeyNames="EmpCardNo" CellPadding="4" ForeColor="#333333" Height="13px"  PageSize="1500" Width="100%">
                        <PagerStyle CssClass="gridview" Height="20px" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>

                             <asp:TemplateField HeaderText="S.No" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hideSubId" runat="server" 
                                            Value='<%# DataBinder.Eval(Container.DataItem, "EmpCardNo")%>' />
                                        <%# Container.DataItemIndex+1%>
                                    </ItemTemplate>

                                    <ItemStyle HorizontalAlign="Center" ForeColor="green" />
                                </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderText="Card No">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpCode" runat="server" Text='<%# Eval("EmpCardNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmpName" runat="server" Text='<%# Eval("EmpName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Department">
                                <ItemTemplate>
                                    <asp:Label ID="lblDptName" runat="server" Text='<%# Eval("DptName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Designation">
                                <ItemTemplate>
                                    <asp:Label ID="lblDsgName" runat="server" Text='<%# Eval("DsgName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblATTDate" runat="server" Text='<%# Eval("AttDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>    
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Reason">
                                <ItemTemplate>
                                    <asp:Label ID="lblReason" runat="server" Text='<%# Eval("Reason") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>                                                                         
                        </Columns>
                        <EditRowStyle BackColor="#7C6F57" />
                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#27235C" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#E3EAEB" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F8FAFA" />
                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                        <SortedDescendingCellStyle BackColor="#D4DFE1" />
                        <SortedDescendingHeaderStyle BackColor="#15524A" />
                    </asp:GridView>
    </div>
    </form>
</body>
</html>
