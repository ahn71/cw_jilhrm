<%@ Page Title="Others Setup" Language="C#" MasterPageFile="~/hrd_nested.master" AutoEventWireup="true" CodeBehind="others_settings.aspx.cs" Inherits="SigmaERP.hrd.others_settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <style>
      #ContentPlaceHolder1_MainContent_gvOthersList th {
          text-align:center;
      }
      .division_table {
          width:450px;
          margin:0px auto;
      }
      .division_table td:nth-child(3) {
          width:250px;
      }
      .Width_ddl {
          width:121px;
      }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row Rrow">
                  <div class="col-md-12 ds_nagevation_bar">
               <div style="margin-top: 5px">
                   <ul>
                       <li><a href="/default.aspx">Dashboard</a></li>
                       <li> <a href="#">/</a></li>
                       <li> <a href="/hrd_default.aspx">Settings</a></li>
                       <li> <a href="#">/</a></li>
                       <li> <a href="#" class="ds_negevation_inactive Ractive">Others</a></li>
                   </ul>               
             </div>
          
             </div>
       </div>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:HiddenField ID="hfSaveStatus" ClientIDMode="Static" runat="server" Value="Save" />
    <asp:HiddenField ID="hfAllowanceId" ClientIDMode="Static" runat="server" Value="0" />
      <asp:HiddenField ID="upSave" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="upupdate" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="updelete" runat="server" ClientIDMode="Static" />

    <asp:UpdatePanel ID="uplMessage" runat="server">
        <ContentTemplate>
            <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="main_box RBox">
        <div class="main_box_header RBoxheader">
            <h2>Others Setup Panel</h2>
        </div>
        <div class="main_box_body Rbody" >

            <div class="main_box_content">
                <asp:UpdatePanel ID="up1" runat="server" UpdateMode="Conditional">
                    <Triggers>
                       <asp:AsyncPostBackTrigger ControlID="ddlCompanyName" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="input_division_info">

                            <table class="division_table">
                                <tr>
                                    <td>Company Name 
                                    </td>
                                    <td>:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlCompanyName" runat="server" ClientIDMode="Static" CssClass="form-control select_width" AutoPostBack="true" OnSelectedIndexChanged="ddlCompanyName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                        <tr>
                        <td>
                          Worker Tiffin Time
                      </td>
                       <td>
                           :
                       </td>
                        <td>
                            <asp:DropDownList ID="ddlWTiffinHour" runat="server" ClientIDMode="Static" CssClass="form-control select_width Width_ddl" style="  float:left;">
                               <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                            </asp:DropDownList>   <asp:DropDownList ID="ddlWTiffinMin" ClientIDMode="Static" runat="server" CssClass="form-control select_width Width_ddl">
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                                <asp:ListItem>18</asp:ListItem>
                                <asp:ListItem>19</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>21</asp:ListItem>
                                <asp:ListItem>22</asp:ListItem>
                                <asp:ListItem>23</asp:ListItem>
                                <asp:ListItem>24</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>26</asp:ListItem>
                                <asp:ListItem>27</asp:ListItem>
                                <asp:ListItem>28</asp:ListItem>
                                <asp:ListItem>29</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>35</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>45</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>55</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        </tr>
                      

                   <tr>
                             <td>
                          Staff Tiffin Time
                      </td>
                       <td>
                           :
                       </td>
                        <td>
                          <asp:DropDownList ID="ddlStaffTiffinHour" runat="server" ClientIDMode="Static" CssClass="form-control select_width Width_ddl" style="   float:left;">
                              <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                            </asp:DropDownList>   <asp:DropDownList ID="ddlStaffTiffinMin" ClientIDMode="Static" runat="server" CssClass="form-control select_width Width_ddl" >
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                                <asp:ListItem>18</asp:ListItem>
                                <asp:ListItem>19</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>21</asp:ListItem>
                                <asp:ListItem>22</asp:ListItem>
                                <asp:ListItem>23</asp:ListItem>
                                <asp:ListItem>24</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>26</asp:ListItem>
                                <asp:ListItem>27</asp:ListItem>
                                <asp:ListItem>28</asp:ListItem>
                                <asp:ListItem>29</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>35</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>45</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>55</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                   </tr>
                                                     <tr>
                          <td>
                              Staff Holiday Time
                          </td>
                          <td>
                              :
                          </td>
                          <td>
                              <asp:DropDownList ID="ddlStaffHolidayHour" ClientIDMode="Static" runat="server" CssClass="form-control select_width Width_ddl" style="  float:left;">
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                  </asp:DropDownList>

                               <asp:DropDownList ID="ddlStaffHolidayMin" ClientIDMode="Static" runat="server" CssClass="form-control select_width Width_ddl" >
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                                <asp:ListItem>18</asp:ListItem>
                                <asp:ListItem>19</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>21</asp:ListItem>
                                <asp:ListItem>22</asp:ListItem>
                                <asp:ListItem>23</asp:ListItem>
                                <asp:ListItem>24</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>26</asp:ListItem>
                                <asp:ListItem>27</asp:ListItem>
                                <asp:ListItem>28</asp:ListItem>
                                <asp:ListItem>29</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>35</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>45</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>55</asp:ListItem>
                            </asp:DropDownList>
                          </td>


                      </tr>
                                <tr>
                                    <td>Worker Tiffin Taka
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtWorkerTiffinTaka" CssClass="form-control select_width">

                                        </asp:TextBox>
                                    </td>
                                </tr>

                                  <tr>
                                    <td>Staff Tiffin Taka
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtStaffTiffinTaka" CssClass="form-control select_width">

                                        </asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>Staff Holiday Count
                                    </td>
                                    <td>:
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="ckbStaffHolidayCout" />
                                    </td>
                                </tr>
<tr>
                          <td>
                              Minimum Working Time
                          </td>
                          <td>
                              :
                          </td>
                          <td>
                              <asp:DropDownList ID="ddlMinimumWorkingHour" ClientIDMode="Static" runat="server" CssClass="form-control select_width Width_ddl" style="  float:left;">
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                  </asp:DropDownList>

                               <asp:DropDownList ID="ddlMinimumWorkingMin" ClientIDMode="Static" runat="server" CssClass="form-control select_width Width_ddl" >
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                                <asp:ListItem>18</asp:ListItem>
                                <asp:ListItem>19</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>21</asp:ListItem>
                                <asp:ListItem>22</asp:ListItem>
                                <asp:ListItem>23</asp:ListItem>
                                <asp:ListItem>24</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>26</asp:ListItem>
                                <asp:ListItem>27</asp:ListItem>
                                <asp:ListItem>28</asp:ListItem>
                                <asp:ListItem>29</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>35</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>45</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>55</asp:ListItem>
                            </asp:DropDownList>
                          </td>


                      </tr>
                                                           
                                <tr>
                          <td>
                             Minimum Over Time
                          </td>
                          <td>
                              :
                          </td>
                          <td>
                              <asp:DropDownList ID="ddlMinimumOverTimeHour" ClientIDMode="Static" runat="server" CssClass="form-control select_width Width_ddl" style="  float:left;">
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                  </asp:DropDownList>

                               <asp:DropDownList ID="ddlMinimumOverTimeMin" ClientIDMode="Static" runat="server" CssClass="form-control select_width Width_ddl" >
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                                <asp:ListItem>18</asp:ListItem>
                                <asp:ListItem>19</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>21</asp:ListItem>
                                <asp:ListItem>22</asp:ListItem>
                                <asp:ListItem>23</asp:ListItem>
                                <asp:ListItem>24</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>26</asp:ListItem>
                                <asp:ListItem>27</asp:ListItem>
                                <asp:ListItem>28</asp:ListItem>
                                <asp:ListItem>29</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>35</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>45</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>55</asp:ListItem>
                            </asp:DropDownList>
                          </td>


                      </tr>
                          

                      <tr runat="server" visible="false">
                          <td>
                              Worker Night Bill&nbsp; Time
                          </td>
                          <td>
                              :
                          </td>
                          <td>
                              <asp:DropDownList ID="ddlWNightBillHour" runat="server" ClientIDMode="Static" CssClass="form-control select_width Width_ddl" style="   float:left;">
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                                <asp:ListItem>18</asp:ListItem>
                                <asp:ListItem>19</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>21</asp:ListItem>
                                <asp:ListItem>22</asp:ListItem>
                                <asp:ListItem>23</asp:ListItem>
                                <asp:ListItem>24</asp:ListItem>
                                  </asp:DropDownList>

                               <asp:DropDownList ID="ddlWorkerNightBillMin" ClientIDMode="Static" runat="server" CssClass="form-control select_width Width_ddl" >
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                                <asp:ListItem>18</asp:ListItem>
                                <asp:ListItem>19</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>21</asp:ListItem>
                                <asp:ListItem>22</asp:ListItem>
                                <asp:ListItem>23</asp:ListItem>
                                <asp:ListItem>24</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>26</asp:ListItem>
                                <asp:ListItem>27</asp:ListItem>
                                <asp:ListItem>28</asp:ListItem>
                                <asp:ListItem>29</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>35</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>45</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>55</asp:ListItem>
                            </asp:DropDownList>
                          </td>


                      </tr>

                         <tr  runat="server" visible="false">
                          <td>
                              Staff Night Bill  Time
                          </td>
                          <td>
                              :
                          </td>
                          <td>
                              <asp:DropDownList ID="ddlStaffNightBillHour" ClientIDMode="Static" runat="server" CssClass="form-control select_width Width_ddl" style="    float:left;">
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                                <asp:ListItem>18</asp:ListItem>
                                <asp:ListItem>19</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>21</asp:ListItem>
                                <asp:ListItem>22</asp:ListItem>
                                <asp:ListItem>23</asp:ListItem>
                                <asp:ListItem>24</asp:ListItem>
                                  </asp:DropDownList>

                               <asp:DropDownList ID="ddlStaffNightBillMin" ClientIDMode="Static" runat="server" CssClass="form-control select_width Width_ddl">
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>01</asp:ListItem>
                                <asp:ListItem>02</asp:ListItem>
                                <asp:ListItem>03</asp:ListItem>
                                <asp:ListItem>04</asp:ListItem>
                                <asp:ListItem>05</asp:ListItem>
                                <asp:ListItem>06</asp:ListItem>
                                <asp:ListItem>07</asp:ListItem>
                                <asp:ListItem>08</asp:ListItem>
                                <asp:ListItem>09</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                                <asp:ListItem>13</asp:ListItem>
                                <asp:ListItem>14</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>16</asp:ListItem>
                                <asp:ListItem>17</asp:ListItem>
                                <asp:ListItem>18</asp:ListItem>
                                <asp:ListItem>19</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>21</asp:ListItem>
                                <asp:ListItem>22</asp:ListItem>
                                <asp:ListItem>23</asp:ListItem>
                                <asp:ListItem>24</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>26</asp:ListItem>
                                <asp:ListItem>27</asp:ListItem>
                                <asp:ListItem>28</asp:ListItem>
                                <asp:ListItem>29</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>35</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>45</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>55</asp:ListItem>
                            </asp:DropDownList>
                          </td>


                      </tr>

                          

                        <tr  runat="server" visible="false">
                            <td>
                                Acceptable Minute as OT
                            </td>
                            <td>
                                :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlOTList" runat="server" ClientIDMode="Static" CssClass="form-control select_width " >
                                <asp:ListItem>00</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>20</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>40</asp:ListItem>
                                <asp:ListItem>45</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>55</asp:ListItem>
                  
                            </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                        </div>

                
                        <div class="button_area Rbutton_area">
                            <a href="#" onclick="window.history.back()" class="Rbutton">Back</a>
                           <asp:Button ID="btnSave" ClientIDMode="Static" CssClass="Rbutton" runat="server" Text="Save" OnClientClick="return InputValidationBasket();" OnClick="btnSave_Click" />
                           <asp:Button ID="btnClear" ClientIDMode="Static" CssClass="Rbutton" runat="server" Text="Clear" OnClick="btnClear_Click"/>
                           <asp:Button ID="btnClose" runat="server" ClientIDMode="Static" CssClass="Rbutton" OnClientClick="ClearInputBox();" PostBackUrl="~/hrd_default.aspx" Text="Close" />
                           <asp:Button ID="Button1" ClientIDMode="Static" CssClass="Rbutton" runat="server" Text="UpEmp" OnClick="Button1_Click" Visible="False"  />
                        </div>
                        <div runat="server" id="divOthersSettings" class="show_division_info">
<asp:GridView ID="gvOthersList" runat="server" DataKeyNames="SL"  HeaderStyle-ForeColor="White" AutoGenerateColumns="False"  Width="100%" OnRowCommand="gvOthersList_RowCommand" >
                             <RowStyle HorizontalAlign="Center" />                              
                             <Columns>
                                  <asp:BoundField DataField="CompanyId" HeaderText="CompanyId"  />                                  
                                 <asp:BoundField DataField="WorkerTiffinHour" HeaderText="WTIffinH"  />
                                 <asp:BoundField DataField="WorkerTiffinMin" HeaderText="WTIffinM"  />
                                 <asp:BoundField DataField="StaffTiffinHour" HeaderText="STiffinH" />
                                 <asp:BoundField DataField="StaffTiffinMin" HeaderText="STiffinM"/>
                                 <asp:BoundField DataField="StaffHolidayTotalHour" HeaderText="SHolidayH"  />
                                 <asp:BoundField DataField="StaffHolidayTotalMin" HeaderText="SHolidayM" />

                                 
                                 <asp:BoundField DataField="WorkerTiffinTaka" HeaderText="W.TiffinTk"  />
                                 <asp:BoundField DataField="StaffTiffinTaka" HeaderText="S.TiffinTk" />
                                 <asp:BoundField DataField="StaffHolidayCount" HeaderText="S.H.Count"/>
                                 
                                 <asp:BoundField DataField="MinWorkingHour" HeaderText="MinWorkingHour"  />
                                 <asp:BoundField DataField="MinWorkingMin" HeaderText="MinWorkingMin"  />
                                 <asp:BoundField DataField="MinOverTimeHour" HeaderText="Min.OT Hour" />
                                 <asp:BoundField DataField="MinOverTimeMin" HeaderText="Min. OT Min"/>
                                  <asp:TemplateField HeaderText="Edit" ItemStyle-Width="100px">
                                     <ItemTemplate>
                                         <asp:Button ID="btnAlter" runat="server" ControlStyle-CssClass="btnForAlterInGV" Text="Edit" CommandName="Alter" CommandArgument='<%#((GridViewRow)Container).RowIndex%>' />
                                     </ItemTemplate>
                                 </asp:TemplateField>           

                             </Columns>
                             <HeaderStyle BackColor="#0057AE" Height="28px" />
                         </asp:GridView>
                       </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                </div>
                

            
        </div>
    </div>


    <script type="text/javascript">

        function InputValidationBasket() {
            try {

                if ($('#txtMedical').val().trim().length == 0) {
                    showMessage('Please type medical allowance amount', 'error');
                    $('#txtMedical').focus(); return false;
                }
                if ($('#txtFood').val().trim().length == 0) {
                    showMessage('Please type food allowance amount', 'error');
                    $('#txtFood').focus(); return false;
                }
                if ($('#txtConveyance').val().trim().length == 0) {
                    showMessage('Please type convenyance allowance amount', 'error');
                    $('#txtConveyance').focus(); return false;
                }
                if ($('#txtHouseRent').val().trim().length == 0) {
                    showMessage('Please type house rent allowance in percentage', 'error');
                    $('#txtHouseRent').focus(); return false;
                }
                if ($('#txtBasicFind').val().trim().length == 0) {
                    showMessage('Please type basic  in percentage', 'error');
                    $('#txtHouseRent').focus(); return false;
                }
                if ($('#txtOthers').val().trim().length == 0) {
                    showMessage('Please type others allowance in percentage', 'error');
                    $('#txtHouseRent').focus(); return false;
                }
                if ($('#txtStampDeduct').val().trim().length == 0) {
                    showMessage('Please type StampDeduct Amount', 'error');
                    $('#txtStampDeduct').focus(); return false;
                }

            }
            catch (exception) {
                showMessage(exception, error)
            }
        }
        function ClearInputBox() {
            try {
                if ($('#upSave').val() == '0') {

                    $('#btnSave').removeClass('css_btn');
                    $('#btnSave').attr('disabled', 'disabled');
                }
                else {
                    $('#btnSave').addClass('css_btn');
                    $('#btnSave').removeAttr('disabled');
                }
                $('#ddlWHour').val('00');
                $('#ddlWMin').val('00');
                $('#ddlStaffHour').val('00');
                $('#ddlStaffMin').val('00');
                $('#ddlWNightBillHour').val('00');
                $('#ddlWorkerNightBillMin').val('00');
                $('#ddlStaffNightBillHour').val('00');
                $('#ddlStaffNightBillMin').val('00');
                $('#ddlStaffHolidayHour').val('00');
                $('#ddlStaffHolidayMin').val('00');
                $('#ddlOTList').val('00');
                $('#hfSaveStatus').val('Save');
                $('#btnSave').val('Save');

            }
            catch (exception) {
                showMessage(exception, error)
            }
        }

        function editAllowanceType(getId) {

           
            $('#hfAllowanceId').val(getId);
            var hur = $('#r_' + getId + ' td:first').html()
            //alert(val);
            $('#ddlWHour').val(hur);
           
            $('#ddlWMin').val($('#r_' + getId + ' td:nth-child(2)').html());
           // alert($('#r_' + getId + ' td:nth-child(2)').html());
            $('#ddlStaffHour').val($('#r_' + getId + ' td:nth-child(3)').html());
            $('#ddlStaffMin').val($('#r_' + getId + ' td:nth-child(4)').html());
            $('#ddlWNightBillHour').val($('#r_' + getId + ' td:nth-child(5)').html());
            $('#ddlWorkerNightBillMin').val($('#r_' + getId + ' td:nth-child(6)').html());
            $('#ddlStaffNightBillHour').val($('#r_' + getId + ' td:nth-child(7)').html());
            $('#ddlStaffNightBillMin').val($('#r_' + getId + ' td:nth-child(8)').html());
            $('#ddlStaffHolidayHour').val($('#r_' + getId + ' td:nth-child(9)').html());
            $('#ddlStaffHolidayMin').val($('#r_' + getId + ' td:nth-child(10)').html());
            $('#ddlOTList').val($('#r_' + getId + ' td:nth-child(11)').html());
            $('#hfSaveStatus').val('Update');
            if ($('#upupdate').val() == '1') {
                $('#btnSave').val('Update');
                $('#btnSave').addClass('css_btn');
                $('#btnSave').removeAttr('disabled');
            }
            else {
                $('#btnSave').val('Update');
                $('#btnSave').removeClass('css_btn');
                $('#btnSave').attr('disabled', 'disabled');
            }

        }

    </script>

</asp:Content>
