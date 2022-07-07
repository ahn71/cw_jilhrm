<%@ Page Title="" Language="C#" MasterPageFile="~/Glory.Master" AutoEventWireup="true" CodeBehind="email.aspx.cs" Inherits="SigmaERP.mail.email" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <script src="http://tinymce.cachefly.net/4.1/tinymce.min.js"></script>
    <script>tinymce.init({ selector: 'textarea' });</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="main_box_content">
        <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="chkLoginUseList" />
                  <asp:AsyncPostBackTrigger ControlID="TabContainer1" />
            </Triggers>
            <ContentTemplate>
                <div class="personnel_Category_Group">
                     <asp:Button ID="btnCompose" runat="server" Text="Compose" CssClass="btnForMailPart" OnClick="btnCompose_Click" />
                    <asp:Button ID="btnInbox" runat="server" Text="Inbox" CssClass="btnForMailPart" OnClick="btnInbox_Click" />
                   
                    <asp:Button ID="btnChat" runat="server" Text="Chat" CssClass="btnForMailPart" Visible="false" />
                      
                     <fieldset style="margin-top: 15px; display:none">
                         <legend style="font-weight:bold">Longin User List</legend> 
                            <asp:RadioButtonList runat="server" ID="chkLoginUseList" AutoPostBack="true"  style="margin-top:10px" OnSelectedIndexChanged="chkLoginUseList_SelectedIndexChanged" ></asp:RadioButtonList>
                         
                     </fieldset>         
                </div>

           

    <asp:TabContainer ID="TabContainer1" AutoPostBack="true"  runat="server" ClientIDMode="Static" CssClass="MyTabStyle"  UseVerticalStripPlacement="false" Font-Bold="true" Font-Size="20px" Height="600px" ActiveTabIndex="1" OnActiveTabChanged="TabContainer1_ActiveTabChanged" >
        <asp:TabPanel runat="server" ID="tab1" TabIndex="0" ClientIDMode="Static"   > 
            <HeaderTemplate >Personnel<span id="tab1Personnel" runat= "server" style=" font-weight:bold; color:red; font-family:Arial"></span></HeaderTemplate>
            <ContentTemplate>
                <asp:UpdatePanel ID="upPersonnel" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnInbox" />
                        <asp:AsyncPostBackTrigger ControlID="TabContainer1" />
                        <asp:AsyncPostBackTrigger ControlID="gvPersonalInbox" />
                    </Triggers>
                    <ContentTemplate>

                    
                
                <div class="Mail_Partial_Info">
                    <asp:GridView runat="server" ID="gvPersonalInbox" DataKeyNames="ComId" HeaderStyle-BackColor="#27235C" HeaderStyle-ForeColor="White" AutoGenerateColumns="false" GridLines="None" AllowPaging="true" AllowSorting="true" PageSize="20" Width="80%" OnRowCommand="gvPersonalInbox_RowCommand" >
                        <Columns> 
                           
                            <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" Checked='<%#bool.Parse(Eval("IsRead").ToString())%>' Enabled="false"  BackColor="White" Width="94%" Style="border:2px Solid gray; border-left:1px solid gray"   />

                            </ItemTemplate> 
                            <ItemStyle HorizontalAlign="Center" Width="45px" />
                           </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sender" HeaderStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Button ID="btnNickName" runat="server"  Text='<%#(Eval("NickName")) %>' CommandName="ForPersonnelDetails"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' BackColor="White" Width="100%" Style="text-align: left"></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Subject" HeaderStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Button ID="btnSubject" runat="server"  Text='<%#(Eval("Subject")) %>' CommandName="ForPersonnelDetails" BackColor="White" Width="100%"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Style="text-align: left" ></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                             

                             <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Button ID="btnDateTime" runat="server"  Text='<%#(Eval("CDate")) %>' Width="100%" BackColor="White" CommandName="ForPersonnelDetails"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Style="text-align: left" ></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                            
                        </Columns>
                    </asp:GridView>

                    
                    <asp:TextBox ID="txtPersonnelDetails" runat="server" Width="80%" Height="560px" TextMode="MultiLine" Visible="false" ></asp:TextBox>
                    <asp:HtmlEditorExtender  ID="txtBody_HtmlEditorExtender" runat="server" TargetControlID="txtPersonnelDetails" EnableSanitization="false" >
                                </asp:HtmlEditorExtender>
                   <%-- <div id="divPersonalMessage" runat="server" style="width:80%; height:560px" >

                    </div>--%>
                       
                    </div>
                        <div runat="server" id="divPersonalMail"></div>
                
                        </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>


        <!--Now Start Leave Module -->

        <asp:TabPanel runat="server" ID="tab2"  TabIndex="1" ClientIDMode="Static"  >
            <HeaderTemplate>Leave<span id="tab2Leave" runat= "server" style=" font-weight:bold; color:red; font-family:Arial"></span></HeaderTemplate>
            <ContentTemplate>
                <asp:UpdatePanel ID="upLeave" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnInbox" />
                        <asp:AsyncPostBackTrigger ControlID="TabContainer1" />
                    </Triggers>
                    <ContentTemplate>

                    
                
                <div class="Mail_Partial_Info">
                    <asp:GridView runat="server" ID="gvLeave" DataKeyNames="ComId" HeaderStyle-BackColor="#27235C" HeaderStyle-ForeColor="White" AutoGenerateColumns="False" GridLines="None" AllowPaging="True" AllowSorting="True" PageSize="20" Width="80%" OnRowCommand="gvPersonalInbox_RowCommand" >
                        <Columns> 
                           
                             <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" Checked='<%#bool.Parse(Eval("IsRead").ToString())%>' Enabled="false"  BackColor="White" Width="94%" Style="border:2px Solid gray; border-left:1px solid gray"   />

                            </ItemTemplate> 
                            <ItemStyle HorizontalAlign="Center" Width="45px" />
                           </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sender" HeaderStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Button ID="btnNickName" runat="server"  Text='<%#(Eval("NickName")) %>' CommandName="ForLeaveDetails"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' BackColor="White" Width="100%" Style="text-align: left"></asp:Button> 
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Subject" HeaderStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Button ID="btnSubject" runat="server"  Text='<%#(Eval("Subject")) %>' CommandName="ForLeaveDetails" BackColor="White" Width="100%"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Style="text-align: left" ></asp:Button> 
                                </ItemTemplate>
                                 <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>

                          

                             <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Button ID="btnDateTime" runat="server"  Text='<%#(Eval("CDate")) %>' Width="100%" BackColor="White" CommandName="ForLeaveDetails"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Style="text-align: left" ></asp:Button> 
                                </ItemTemplate>
                                 <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>

                        </Columns>
                        <HeaderStyle BackColor="#27235C" ForeColor="White" />
                    </asp:GridView>

                    
                    <asp:TextBox ID="txtLeaveDetails" runat="server" Width="80%" Height="560px" TextMode="MultiLine" Visible="false" ></asp:TextBox>
                    <asp:HtmlEditorExtender  ID="txtBodytxtLeaveDetails" runat="server" TargetControlID="txtLeaveDetails" EnableSanitization="false" >
                                </asp:HtmlEditorExtender>
                       
                    </div>
                
                        </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel runat="server" ID="tab3" TabIndex="2">
            <HeaderTemplate >Attendance<span id="tab3Attendance" runat= "server" style=" font-weight:bold; color:red; font-family:Arial"></span></HeaderTemplate>
            <ContentTemplate>
               <asp:UpdatePanel ID="upAttendance" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnInbox" />
                         <asp:AsyncPostBackTrigger ControlID="TabContainer1" />
                    </Triggers>
                    <ContentTemplate>

                    
                
                <div class="Mail_Partial_Info">
                    <asp:GridView runat="server" ID="gvAttendance" DataKeyNames="ComId" HeaderStyle-BackColor="#27235C" HeaderStyle-ForeColor="White" AutoGenerateColumns="false" GridLines="None" AllowPaging="true" AllowSorting="true" PageSize="20" Width="80%" OnRowCommand="gvPersonalInbox_RowCommand" >
                        <Columns> 
                           
                             <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" Checked='<%#bool.Parse(Eval("IsRead").ToString())%>' Enabled="false"  BackColor="White" Width="94%" Style="border:2px Solid gray; border-left:1px solid gray"   />

                            </ItemTemplate> 
                            <ItemStyle HorizontalAlign="Center" Width="45px" />
                           </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sender" HeaderStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Button ID="btnNickName" runat="server"  Text='<%#(Eval("NickName")) %>' CommandName="ForAttendanceDetails"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' BackColor="White" Width="100%" Style="text-align: left"></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Subject" HeaderStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Button ID="btnSubject" runat="server"  Text='<%#(Eval("Subject")) %>' CommandName="ForAttendanceDetails" BackColor="White" Width="100%"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Style="text-align: left" ></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Button ID="btnDateTime" runat="server"  Text='<%#(Eval("CDate")) %>' Width="100%" BackColor="White" CommandName="ForAttendanceDetails"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Style="text-align: left" ></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>

                    
                    <asp:TextBox ID="txtAttendanceDetails" runat="server" Width="80%" Height="560px" TextMode="MultiLine" Visible="false" ></asp:TextBox>
                    <asp:HtmlEditorExtender  ID="attendanceDetails" runat="server" TargetControlID="txtAttendanceDetails" EnableSanitization="false" >
                    </asp:HtmlEditorExtender>
                     
                    </div>
                
                        </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel runat="server" ID="tab4" TabIndex="3">
            <HeaderTemplate>Payroll<span id="tab4Payroll" runat= "server" style=" font-weight:bold; color:red; font-family:Arial"></span></HeaderTemplate>
            <ContentTemplate>
                 <asp:UpdatePanel ID="upPayroll" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnInbox" />
                         <asp:AsyncPostBackTrigger ControlID="TabContainer1" />
                    </Triggers>
                    <ContentTemplate>

                    
                
                <div class="Mail_Partial_Info">
                    <asp:GridView runat="server" ID="gvPayroll" DataKeyNames="ComId" HeaderStyle-BackColor="#27235C" HeaderStyle-ForeColor="White" AutoGenerateColumns="false" GridLines="None" AllowPaging="true" AllowSorting="true" PageSize="20" Width="80%" OnRowCommand="gvPersonalInbox_RowCommand" >
                        <Columns> 
                           
                             <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" Checked='<%#bool.Parse(Eval("IsRead").ToString())%>' Enabled="false"  BackColor="White" Width="94%" Style="border:2px Solid gray; border-left:1px solid gray"   />

                            </ItemTemplate> 
                            <ItemStyle HorizontalAlign="Center" Width="45px" />
                           </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sender" HeaderStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Button ID="btnNickName" runat="server"  Text='<%#(Eval("NickName")) %>' CommandName="ForPayrollDetails"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' BackColor="White" Width="100%" Style="text-align: left"></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Subject" HeaderStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Button ID="btnSubject" runat="server"  Text='<%#(Eval("Subject")) %>' CommandName="ForPayrollDetails" BackColor="White" Width="100%"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Style="text-align: left" ></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Button ID="btnDateTime" runat="server"  Text='<%#(Eval("CDate")) %>' Width="100%" BackColor="White" CommandName="ForPayrollDetails"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Style="text-align: left" ></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>

                    
                    <asp:TextBox ID="txtPayrollDetails" runat="server" Width="80%" Height="560px" TextMode="MultiLine" Visible="false" ></asp:TextBox>

                      <asp:HtmlEditorExtender  ID="payrollDetails" runat="server" TargetControlID="txtPayrollDetails" EnableSanitization="false" >
                    </asp:HtmlEditorExtender>
                    </div>
                
                        </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="tab5" runat="server" TabIndex="4">
            <HeaderTemplate>Chat </HeaderTemplate>
            <ContentTemplate>
                 <asp:UpdatePanel ID="upChat" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnInbox" />
                         <asp:AsyncPostBackTrigger ControlID="TabContainer1" />
                    </Triggers>
                    <ContentTemplate>

                    
                
                <div class="Mail_Partial_Info">
                    <asp:GridView runat="server" ID="gvChat" DataKeyNames="CId" HeaderStyle-BackColor="#27235C" HeaderStyle-ForeColor="White" AutoGenerateColumns="false" GridLines="None" AllowPaging="true" AllowSorting="true" PageSize="20" Width="80%" >
                        <Columns > 
                           
                            <asp:TemplateField HeaderText="Sender" HeaderStyle-HorizontalAlign="Left"  HeaderStyle-Width="15%" >
                                <ItemTemplate  >
                                    <asp:Button ID="btnNickName" runat="server"  Text='<%#(Eval("FirstName")) %>' CommandName="ForDetailsPersonnel"  Width="100%" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' BackColor="White"  Style="float:left; text-align: left"></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Text" HeaderStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Button ID="btnSubject" runat="server"  Text='<%#(Eval("Text")) %>' CommandName="ForDetailsPersonnel" BackColor="White" Width="100%"    CommandArgument='<%#((GridViewRow)Container).RowIndex%>'   Style=" text-align: left" ></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12%" >
                                <ItemTemplate>
                                    <asp:Button ID="btnDateTime" runat="server"  Text='<%#(Eval("CDateTime")) %>' Width="100%"  BackColor="White" CommandName="ForDetailsPersonnel"   CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Style="float:right; text-align: left" ></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>

                    
                    <asp:TextBox ID="txtChatDetails" runat="server" Width="80%" Height="560px" TextMode="MultiLine" Visible="false" ></asp:TextBox>

                     
                    </div>
                
                   </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>


       <asp:TabPanel runat="server" ID="tab6" TabIndex="3">
            <HeaderTemplate>Compose<span id="tab6Compose" runat= "server" style=" font-weight:bold; color:red; font-family:Arial"></span></HeaderTemplate>
            <ContentTemplate>
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnInbox" />
                         <asp:AsyncPostBackTrigger ControlID="TabContainer1" />
                    </Triggers>
                    <ContentTemplate>

                    
                
                <div class="Mail_Partial_Info">
                    <asp:GridView runat="server" ID="gvComposeMail" DataKeyNames="ComposeMail_Id" HeaderStyle-BackColor="#27235C" HeaderStyle-ForeColor="White" AutoGenerateColumns="false" GridLines="None" AllowPaging="true" AllowSorting="true" PageSize="20" Width="80%" OnRowCommand="gvPersonalInbox_RowCommand" >
                        <Columns> 
                           
                             <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkStatus" runat="server" Checked='<%#bool.Parse(Eval("IsRead").ToString())%>' Enabled="false"  BackColor="White" Width="94%" Style="border:2px Solid gray; border-left:1px solid gray"   />

                            </ItemTemplate> 
                            <ItemStyle HorizontalAlign="Center" Width="45px" />
                           </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sender" HeaderStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Button ID="btnNickName" runat="server"  Text='<%#(Eval("NickName")) %>' CommandName="ForComposeMailDetails"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' BackColor="White" Width="100%" Style="text-align: left"></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Subject" HeaderStyle-HorizontalAlign="Left" >
                                <ItemTemplate>
                                    <asp:Button ID="btnSubject" runat="server"  Text='<%#(Eval("Subject")) %>' CommandName="ForComposeMailDetails" BackColor="White" Width="100%"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Style="text-align: left" ></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Button ID="btnDateTime" runat="server"  Text='<%#(Eval("CDate")) %>' Width="100%" BackColor="White" CommandName="ForComposeMailDetails"  CommandArgument='<%#((GridViewRow)Container).RowIndex%>' Style="text-align: left" ></asp:Button> 
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>

                    
                    <asp:TextBox ID="txtComposeMailDetails" runat="server" Width="80%" Height="560px" TextMode="MultiLine" Visible="false" ></asp:TextBox>

                      <asp:HtmlEditorExtender  ID="ComposeMail" runat="server" TargetControlID="txtComposeMailDetails" EnableSanitization="false" >
                    </asp:HtmlEditorExtender>
                    </div>
                
                        </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:TabPanel>

    </asp:TabContainer>
 </ContentTemplate>
        </asp:UpdatePanel>
   </div>


</asp:Content>
