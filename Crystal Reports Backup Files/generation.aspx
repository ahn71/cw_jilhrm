<%@ Page Title="Earn Leave Generation" Language="C#" MasterPageFile="~/leave_nested.master" AutoEventWireup="true" CodeBehind="generation.aspx.cs" Inherits="SigmaERP.leave.generation" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
     <asp:UpdatePanel ID="uplMessage" runat="server" >
    <ContentTemplate><p class="message"  id="lblMessage" clientidmode="Static" runat="server"></p></ContentTemplate>
</asp:UpdatePanel>

    <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlGenerateMonth" />
        </Triggers>
        <ContentTemplate>
              <div class="row">
                  <div class="col-md-12 ds_nagevation_bar">
               <div style="margin-top: 5px">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                       <li>/</li>
                       <li> <a href="/leave_default.aspx">Leave</a></li>
                       <li>/</li>
                       <li> <a href="#" class="ds_negevation_inactive Lactive">Earn Leave Generation</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>

    <div class="main_box_leave">
       
    	<div class="main_box_header_leave">
            <h2>Earn Leave Generation</h2>
        </div>
    	<div class="main_box_body_leave">
        	<div class="main_box_content_leave">
                <div class="input_division_info_leave">

                    <div class="generation_box">
                     <table class="division_table-ge">
                    
                    <tbody>
                          <tr>
                           
                             <td><label>Company Name</label></td>
                             <td>
                                 <asp:DropDownList ID="ddlBranch" ClientIDMode="Static"   CssClass="form-control select_width"  runat="server"  AutoPostBack="false"  style="width:217px;" >              
                                         </asp:DropDownList>
                             </td>
                         </tr>
                        <tr>
                            <td>
                                <label>Generate On </label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblDaysOptions" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ForeColor="Blue" Font-Bold="true">
                                    <asp:ListItem Value="On360Days" Text="On 360 Days" ></asp:ListItem>
                                    <asp:ListItem Value="PresentDays" Selected="True" Text="Present Days in 360 Days" ></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <label>Generate On </label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblGeneratedOn" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ForeColor="Blue" Font-Bold="true">
                                    <asp:ListItem Value="Basic" Text="Basic Salary" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="Gross" Text="Gross Salary" ></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                         <tr>
                           
                             <td><label>Generation Month </label></td>
                             <td>
                                 <asp:DropDownList ID="ddlGenerateMonth" runat="server" ClientIDMode="Static"  CssClass="form-control select_width" AutoPostBack="True" OnSelectedIndexChanged="ddlGenerateMonth_SelectedIndexChanged" style="width:217px;"></asp:DropDownList>
                             </td>
                         </tr>

                        

                        
                       
                    </tbody></table></div>
                            <div class="long_box">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <span style=" font-family:'Times New Roman'; font-size:20px; color:green;font-weight:bold;float:left"><p>Wait&nbsp; processing</p> </span> 
                                        <img style="width:26px;height:26px;cursor:pointer; float:left;margin-left:5px;margin-top:22px;" src="/images/wait.gif"  />  
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                         

                        
                </div>
        
                
                  <div class="border">
                                </div>    
                                <div class="list_small_button" style="width:285px">
                                                   <table>
                                                        <tbody>
                                    
                                                        <tr>
                                                          <td><asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="back_button" OnClick="btnGenerate_Click"/></td>
                                                          <td>
                                                              <asp:Button ID="Button2" runat="server" PostBackUrl="~/default.aspx" Text="Close" Cssclass="css_btn" />
                                                         </td>  
                                                            <td>
                                                                 <asp:Button ID="btnPopup" runat="server" style="display:none" Text="Close"   CssClass="css_btn" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnDelete" runat="server"  Text="Delete" Cssclass="css_btn" OnClick="btnDelete_Click" />
                                                            </td> 
                                                            <td>
                                                                 <asp:Button ID="btnGeneratePopup" runat="server" style="display:none" Text="Close"   CssClass="css_btn" />
                                                            </td>                       
                                                      </tr>
                                                </tbody>
                                         </table>
                                    </div>

                            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" Drag="True" 
                            DropShadow="True" PopupControlID="PopupWindow" TargetControlID="btnPopup" CancelControlID="btnCancel" PopupDragHandleControlID="divDrag" CacheDynamicResults="False" Enabled="True" >
                        </asp:ModalPopupExtender>

                       <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" Drag="True" 
                            DropShadow="True" PopupControlID="popupGenerate" TargetControlID="btnGeneratePopup" CancelControlID="btnCancel2" PopupDragHandleControlID="divDrag" CacheDynamicResults="False" Enabled="True" >
                        </asp:ModalPopupExtender>

                        <div style="border-left: 2px solid #086A99; border-right: 2px solid #086A99; border-bottom: 2px solid #086A99; border-radius: 5px;  font-weight:bold; width: 662px; background:#ddd; padding:5px; border-top-style: none; height:450px; border-top-color: inherit; border-top-width: 0px;" id="PopupWindow" >
                            <div id="divDrag" class="boxFotter">
                                 <a ID="btnCancel" href="#"><img class="popup_close" src="../images/icon/cancel.png" alt="" style="margin-left: 280px;" /></a>
                           <cnter> 
                                <h2 runat="server" id="hHeader" style="margin-top: -3px;">Earn leave List</h2>
                           </cnter>
                                
                             </div>

                            <asp:Panel ID="Panel1" runat="server" BackColor="WhiteSmoke" Height="428px">
                                 
                                 <div style ="height:370px; width:657px; overflow:scroll"> 
                                <asp:GridView runat="server"  ID="gvEmpEarnLeaveList" AutoGenerateColumns="false" AllowPaging="false"  HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" PageSize="10" Width="604px" DataKeyNames="SN">
                                 <PagerStyle CssClass="gridview" />
                                     <Columns>
                                     <asp:BoundField HeaderText="Card No" DataField="EmpCardNo" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" ItemStyle-Width="25px" />
                                         <asp:BoundField HeaderText="Name" DataField="EmpName" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" ItemStyle-Width="100px" />
                                         <asp:BoundField HeaderText="Type" DataField="EmpType" HeaderStyle-BackColor="Black" HeaderStyle-ForeColor="White" ItemStyle-Width="25px" />
                                     <asp:TemplateField AccessibleHeaderText="Choose" HeaderText="Chosen" ItemStyle-Width="5px"  ItemStyle-HorizontalAlign="center">
                                         <ItemTemplate  >
                                             <asp:CheckBox ID="SelectCheckBox" runat="server" ItemStyle-Width="5px" Checked="true" />
                                         </ItemTemplate>
                                     </asp:TemplateField>
                                 </Columns>
                             </asp:GridView>
                                     </div>
                                <br />
                               
                                 

                             

                                <asp:Button ID="btnSubmit" Width="80px" Text="Set" runat="server" OnClick="btnSubmit_Click"  /><br />
                                <asp:TextBox runat="server" ID="txtquery" ></asp:TextBox>
                           </asp:Panel>
                          

                          
                           
                        </div>
               
				
                 <div style="border-left: 2px solid #086A99; border-right: 2px solid #086A99; border-bottom: 2px solid #086A99; border-radius: 5px;  font-weight:bold; width: 447px; background:#ddd; padding:5px; border-top-style: none; height:198px; border-top-color: inherit; border-top-width: 0px;" id="popupGenerate" >
                            

                            <asp:Panel ID="Panel2" runat="server" BackColor="WhiteSmoke" Height="201px" Width="455px" style="margin-left: -4px;margin-top: -2px;">
                             
                                <br />
                                <br />
                                <asp:Label runat="server" Text="Generated Message" ID="lblGeneratedMessage" Font-Bold="True" Font-Size="26px" ForeColor="#CC0000" style="margin-left: 25px;display:block;"></asp:Label>
               
                                <br />
                                <asp:Button ID="btnCancel2" runat="server" Text="Ok" Height="30px" Width="113px" style="margin-left: 169px;margin-top: 50px;" />
                           </asp:Panel>
                        </div>


            </div>
        </div>
    </div>
             </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
