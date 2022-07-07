<%@ Page Title="" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="loansetting.aspx.cs" Inherits="SigmaERP.payroll.loansetting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>


    <div class="create_account_main_box" style="width: 658px;">
                <div class="employee_box_header">
                    <h2><asp:Label runat="server"  ID="lblTtile"></asp:Label> </h2>
                </div>
                
                <div class="employee_box_body" style="width: 634px;">

                    <div class="create_account_content">

        <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
        <Triggers>
           <%-- <asp:AsyncPostBackTrigger ControlID="btnSearch" />--%>
            <asp:AsyncPostBackTrigger ControlID="btnSet" />
        </Triggers>
        <ContentTemplate>
            <div style="float:left">
             <asp:UpdateProgress ID="UpdateProgress1" runat="server" style="margin-top:-23px;" >
                                    <ProgressTemplate>
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold;float:left"><p>Wait&nbsp; processing</p> </span> 
                                        <img style="width:26px;height:26px;cursor:pointer; float:left;margin-top: 21px;margin-left: 7px;" src="/images/wait.gif"  />  
                                    </ProgressTemplate>
                                </asp:UpdateProgress> <br>
            </div>
            <br />
              <asp:Button id="btnSet" runat="server" Text="Set" CssClass="css_btn" style=" float:right; width:80px;margin-top:-25px" OnClick="btnSet_Click"/><br />
              <asp:GridView ID="gvLoanList" runat="server" AutoGenerateColumns="false" HeaderStyle-Height="26px" HeaderStyle-Font-Bold="false" HeaderStyle-ForeColor="White" HeaderStyle-BackColor="Black" DataKeyNames="LoanId">
            <Columns>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Bind("LoanId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="EmpCardNo" HeaderText="EmpCardNo" Visible="true" ItemStyle-Width="100px" itemstyle-horizontalalign="center" ItemStyle-Height="26px" />
                <asp:BoundField DataField="LoanAmount" HeaderText="Loan" Visible="true" ItemStyle-Width="100px" itemstyle-horizontalalign="center" />
                <asp:BoundField DataField="InstallmentNo" HeaderText="Ins.No." Visible="true" ItemStyle-Width="100px"  itemstyle-horizontalalign="center" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="red"/>
                 <asp:BoundField DataField="InstallmentAmount" HeaderText="Ins.Amount" Visible="true" ItemStyle-Width="100px" itemstyle-horizontalalign="center" />
               
                <asp:BoundField DataField="StartMonth" HeaderText="StartM." Visible="true" ItemStyle-Width="100px" itemstyle-horizontalalign="center" />
                <asp:BoundField DataField="EntryDate" HeaderText="Date" Visible="true" ItemStyle-Width="100px" itemstyle-horizontalalign="center"/>

                <asp:BoundField DataField="EmpType" HeaderText="EmpType" Visible="true" itemstyle-horizontalalign="center" />
                <asp:BoundField DataField="PaidInstallmentNo" HeaderText="P.Ins.No" Visible="true" ItemStyle-Width="100px" itemstyle-horizontalalign="center" ItemStyle-Font-Bold="true" ItemStyle-ForeColor="Green" />
                <asp:TemplateField AccessibleHeaderText="Choose" ItemStyle-Width="100px" itemstyle-horizontalalign="center">
                    <ItemTemplate   >
                        <asp:CheckBox ID="SelectCheckBox" runat="server" ItemStyle-Width="100px" Checked="true"  />
                        
                        
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>     

             </ContentTemplate>
    </asp:UpdatePanel>

                    </div>
                </div>
            </div>

</asp:Content>
