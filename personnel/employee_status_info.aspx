<%@ Page Title="" Language="C#" MasterPageFile="~/personnel_NestedMaster.master" AutoEventWireup="true" CodeBehind="employee_status_info.aspx.cs" Inherits="SigmaERP.personnel.employee_status_info" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
           <script type="text/javascript">

               var oldgridcolor;
               function SetMouseOver(element) {
                   oldgridcolor = element.style.backgroundColor;
                   element.style.backgroundColor = '#ffeb95';
                   element.style.cursor = 'pointer';
                   // element.style.textDecoration = 'underline';
               }
               function SetMouseOut(element) {
                   element.style.backgroundColor = oldgridcolor;
                   // element.style.textDecoration = 'none';

               }

</script>
    <style>
        /*#gvSeperationListLog td{
           text-align:center;
        }
          #gvSeperationListLog td:nth-child(2){
           text-align:left;
           padding-left:3px;
        }   
             #gvSeperationListLog th:nth-child(2){
           text-align:left;
           padding-left:3px;
        } 
                #gvSeperationListLog td:nth-child(1){
           text-align:left;
           padding-left:3px;
           width:200px;
        }*/       
       
        .table tr th:nth-child(2),tr th:nth-child(3),tr th:nth-child(4),tr th:nth-child(5), table tr th:nth-child(6),
        table tr th:nth-child(7), table tr th:nth-child(8),
        tr td:nth-child(5), table tr td:nth-child(6),
        table tr td:nth-child(7), table tr td:nth-child(8) {
            text-align:center;
        }
         .table tr td:nth-child(2),tr td:nth-child(3),tr td:nth-child(4),tr td:nth-child(5), table tr td:nth-child(6),
        table tr td:nth-child(7), table tr td:nth-child(8),
        tr td:nth-child(5), table tr td:nth-child(6),
        table tr td:nth-child(7), table tr td:nth-child(8) {
            text-align:center;
        }
         .table tr td:nth-child(2),tr th:nth-child(2)
         {
             text-align:left !important;                      
         }
         .table tr td:nth-child(1)
         {
             text-align:left !important;            
             padding-left:10px;
         }
          .table tr td:first-child, th:first-child {        
           text-align:left !important;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="employee_box_body">
               <div class="application_box_header">
                    <h2>Master Search Panel For Employee</h2>
                </div>
        <div class="employee_box_content">

           <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Always">
               <ContentTemplate>

              
            <div style="width: 100%;" id="divElementContainer" runat="server">
           
             <asp:TextBox ClientIDMode="Static" ID="txtSearchLog" autocomplete='off' Style="width: 100%; text-align: center" CssClass="form-control text_box_width" placeholder="search by anything" runat="server" Font-Bold="True"></asp:TextBox>
                <asp:GridView ClientIDMode="Static" GridLines="Horizontal"  ID="gvSeperationListLog" Width="100%" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvSeperationListLog_RowDataBound">
                    <HeaderStyle BackColor="#27235C" Font-Bold="True" Font-Size="11px" ForeColor="White" Height="28px"  />
                    <RowStyle Height="27px" />
                    <PagerStyle CssClass="gridview" />
                    <Columns>

                        <asp:BoundField DataField="EmpCardNo" HeaderText="Card No" />
                        <asp:BoundField DataField="EmpName" HeaderText="Name">
                          
                        </asp:BoundField>
                        <asp:BoundField DataField="EmpType" HeaderText="Type">
                      
                        </asp:BoundField>
                        <asp:BoundField DataField="MobileNo" HeaderText="Mobile">
                        
                        </asp:BoundField>
                        <asp:BoundField DataField="Email" HeaderText="Email">
                      
                        </asp:BoundField>
                        <asp:BoundField DataField="NationIDCardNo" HeaderText="N.I.N">

                      
                        </asp:BoundField>

                        <asp:BoundField DataField="EmpJoiningDate" HeaderText="J.O.B" />

                        <asp:BoundField DataField="EmpStatusName" HeaderText="Sep.Type" />



                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" />


                    </Columns>
                </asp:GridView>
           
            </div>

                    </ContentTemplate>
           </asp:UpdatePanel>
        </div>
    </div>

    <script src="../scripts/jquery-1.8.2.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
           
            $(document).on("keyup", '#txtSearchLog', function () {
                searchTable($(this).val(), 'gvSeperationListLog', '');
            });

            $(document).on("keypress", "body", function (e) {
                if (e.keyCode == 13) e.preventDefault();
                // alert('deafault prevented');

            });

        });
    </script>
</asp:Content>
