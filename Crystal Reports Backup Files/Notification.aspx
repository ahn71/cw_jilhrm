<%@ Page Title="" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="Notification.aspx.cs" Inherits="SigmaERP.Notification" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #ContentPlaceHolder1_gvPermanentNotification th:first-child,th:nth-child(4) {
            text-align:center;
        }
        #ContentPlaceHolder1_gvPermanentNotification th:first-child,th:nth-child(4) {
            padding-left:3px;
        }
        #ContentPlaceHolder1_gvLateNotification th:first-child,th:nth-child(4) ,th:nth-child(5)  {
           text-align:center;
        }
         #ContentPlaceHolder1_gvLateNotification td:first-child,td:nth-child(4),td:nth-child(5)  {
           text-align:center;
        }
         #ContentPlaceHolder1_gvLateNotification th:first-child,th:nth-child(4) ,th:nth-child(5)  {
          padding-left:3px;
        }
         #ContentPlaceHolder1_gvLateNotification td:first-child,td:nth-child(4),td:nth-child(5)  {
           padding-left:3px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <br />
    <div style="max-width:800px; margin:0px auto">
          <div class="main_box_header_leave LBoxheader">
                    <h2>Notification</h2>
                </div>
        <div style="width: 100%; margin:0px auto">
                       <asp:GridView HeaderStyle-BackColor="#5EC1FF" ID="gvLateNotification" HeaderStyle-Height="28px" runat="server" AutoGenerateColumns="false" BackColor="White" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="14px" AllowPaging="true" PageSize="40" Width="100%">
                          <PagerStyle CssClass="gridview" />
                          <Columns>
                              
                              <asp:BoundField DataField="EmpCardNo" HeaderText="Card No"   />
                               <asp:BoundField DataField="EmpName" HeaderText="Name"  ItemStyle-HorizontalAlign="left" />
                               <asp:BoundField DataField="DsgName" HeaderText="Designation"  ItemStyle-HorizontalAlign="Left" />
                              <asp:BoundField DataField="Date" HeaderText="Late Date"   />
                              <asp:BoundField DataField="LateTime" HeaderText="Late Time"   />
                             
                          </Columns>
                      </asp:GridView>                   
                  </div>
            <div style="width: 100%; margin:0px auto">
                       <asp:GridView HeaderStyle-BackColor="#5EC1FF" ID="gvPermanentNotification" HeaderStyle-Height="28px" runat="server" AutoGenerateColumns="false" BackColor="White" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="14px" AllowPaging="true" PageSize="40" Width="100%" OnPageIndexChanging="gvPermanentNotification_PageIndexChanging">
                          <PagerStyle CssClass="gridview" />
                          <Columns>
                              
                              <asp:BoundField DataField="EmpCardNo" HeaderText="Card No"  ItemStyle-HorizontalAlign="Center" />
                               <asp:BoundField DataField="EmpName" HeaderText="Name"  ItemStyle-HorizontalAlign="left" />
                               <asp:BoundField DataField="DsgName" HeaderText="Designation"  ItemStyle-HorizontalAlign="Left" />
                              <asp:BoundField DataField="Date" HeaderText="Permanent Date"   ItemStyle-HorizontalAlign="Center" />                             
                             
                          </Columns>
                      </asp:GridView>                   
                  </div>
    </div>
</asp:Content>
