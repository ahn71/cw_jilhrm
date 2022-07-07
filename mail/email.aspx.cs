using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ComplexScriptingSystem;
using HtmlAgilityPack;
using System.Data.SqlClient;


namespace SigmaERP.mail
{
    public partial class email : System.Web.UI.Page
    {
        SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();

            if (!IsPostBack)
            {
                
                setPrivilege();
                loadComplainByModuleType("Personnel");
                loadLoginUserList();
                TabContainer1.ActiveTabIndex = 0;
                getInitialMailStatus(10);  // 10 that is unkown and 10 is initial value.
            }
        }



        static DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__getUserId__"] = getUserId;
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                //------------load privilege setting inof from db------
                dtSetPrivilege = new DataTable();
               // sqlDB.fillDataTable("select * from UserPrivilege where PageName='aplication.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);
                //-----------------------------------------------------

               



                   

            }
            catch { }

        }


        private void getInitialMailStatus(byte TabIndex)   // for all mail status
        {
            try
            {
                
                DataTable dtMailStatus=new DataTable ();
                if (TabIndex == 10)  // 10 that is unkown,that is initial value
                {
                    sqlDB.fillDataTable("select ComId from Mail_Complain_Info where ModuleType='Personnel' AND IsRead='False' AND RxUserId=" + ViewState["__getUserId__"].ToString() + "  ", dtMailStatus);
                    if (dtMailStatus.Rows.Count > 0) tab1Personnel.InnerText = " (" + dtMailStatus.Rows.Count.ToString() + ")";

                    sqlDB.fillDataTable("select ComId from Mail_Complain_Info where ModuleType='Leave' AND IsRead='False' AND RxUserId=" + ViewState["__getUserId__"].ToString() + "  ", dtMailStatus = new DataTable());
                    if (dtMailStatus.Rows.Count > 0) tab2Leave.InnerText = " (" + dtMailStatus.Rows.Count.ToString() + ")";

                    sqlDB.fillDataTable("select ComId from Mail_Complain_Info where ModuleType='Attendance' AND IsRead='False' AND RxUserId=" + ViewState["__getUserId__"].ToString() + "  ", dtMailStatus = new DataTable());
                    if (dtMailStatus.Rows.Count > 0) tab3Attendance.InnerText = " (" + dtMailStatus.Rows.Count.ToString() + ")";

                    sqlDB.fillDataTable("select ComId from Mail_Complain_Info where ModuleType='Payroll' AND IsRead='False' AND RxUserId=" + ViewState["__getUserId__"].ToString() + "  ", dtMailStatus = new DataTable());
                    if (dtMailStatus.Rows.Count > 0) tab4Payroll.InnerText = " (" + dtMailStatus.Rows.Count.ToString() + ")";

                    sqlDB.fillDataTable("select ComposeMail_Id from v_Mail_ComposeMail_Info where IsRead='False' AND RxUserId=" + ViewState["__getUserId__"].ToString() + " AND CompanyId='" + ViewState["__CompanyId__"].ToString() + "'  ", dtMailStatus = new DataTable());
                    if (dtMailStatus.Rows.Count > 0) tab6Compose.InnerText = " (" + dtMailStatus.Rows.Count.ToString() + ")";
                }
                else
                {
                    if (TabContainer1.ActiveTabIndex == 0)
                    {
                        sqlDB.fillDataTable("select ComId from Mail_Complain_Info where ModuleType='Personnel' AND IsRead='False' AND RxUserId=" + ViewState["__getUserId__"].ToString() + "  ", dtMailStatus);
                        if (dtMailStatus.Rows.Count > 0) tab1Personnel.InnerText = " (" + dtMailStatus.Rows.Count.ToString() + ")";
                        else tab1Personnel.InnerText = "";
                    }
                    else if (TabContainer1.ActiveTabIndex == 1)
                    {
                        sqlDB.fillDataTable("select ComId from Mail_Complain_Info where ModuleType='Leave' AND IsRead='False' AND RxUserId=" + ViewState["__getUserId__"].ToString() + "  ", dtMailStatus = new DataTable());
                        if (dtMailStatus.Rows.Count > 0) tab2Leave.InnerText = " (" + dtMailStatus.Rows.Count.ToString() + ")";
                        else tab2Leave.InnerText = "";
                    }
                    else if (TabContainer1.ActiveTabIndex == 2)
                    {
                        sqlDB.fillDataTable("select ComId from Mail_Complain_Info where ModuleType='Attendance' AND IsRead='False' AND RxUserId=" + ViewState["__getUserId__"].ToString() + "  ", dtMailStatus = new DataTable());
                        if (dtMailStatus.Rows.Count > 0) tab3Attendance.InnerText = " (" + dtMailStatus.Rows.Count.ToString() + ")";
                        else tab3Attendance.InnerText = "";
                    }
                    else if (TabContainer1.ActiveTabIndex == 3)
                    {
                        sqlDB.fillDataTable("select ComId from Mail_Complain_Info where ModuleType='Payroll' AND IsRead='False' AND RxUserId=" + ViewState["__getUserId__"].ToString() + "  ", dtMailStatus = new DataTable());
                        if (dtMailStatus.Rows.Count > 0) tab4Payroll.InnerText = " (" + dtMailStatus.Rows.Count.ToString() + ")";
                        else tab4Payroll.InnerText = "";
                    }
                    else if (TabContainer1.ActiveTabIndex == 5)
                    {
                        sqlDB.fillDataTable("select ComposeMail_Id from v_Mail_ComposeMail_Info where IsRead='False' AND RxUserId=" + ViewState["__getUserId__"].ToString() + " AND CompanyId='" + ViewState["__CompanyId__"].ToString() + "'  ", dtMailStatus = new DataTable());
                        if (dtMailStatus.Rows.Count > 0) tab6Compose.InnerText = " (" + dtMailStatus.Rows.Count.ToString() + ")";
                        else tab6Compose.InnerText = "";
                    }
                }
            }
            catch { }
        }

        DataTable dt;
        private void loadComplainByModuleType(string ModuleType)
        {
            try
            {
                dt = new DataTable();
                if (!ModuleType.Equals("Chat") || !ModuleType.Equals("ComposeMail")) sqlDB.fillDataTable("select ComId,EmpId,NickName,Subject,Format(CDate,'MMM dd') as CDate,IsRead from v_Mail_Complain_Info where ModuleType='" + ModuleType + "' AND RxUserId=" + ViewState["__getUserId__"].ToString() + " ", dt = new DataTable());

                if (TabContainer1.ActiveTabIndex == 0 || ModuleType == "Personnel")
                {
                    gvPersonalInbox.DataSource = dt;
                    gvPersonalInbox.DataBind();
                }
                else if (TabContainer1.ActiveTabIndex == 1)
                {
                    gvLeave.DataSource = dt;
                    gvLeave.DataBind();


                }
                else if (TabContainer1.ActiveTabIndex == 2)
                {
                    gvAttendance.DataSource = dt;
                    gvAttendance.DataBind();
                }

                else if (TabContainer1.ActiveTabIndex == 3) 
                {
                    gvPayroll.DataSource = dt;
                    gvPayroll.DataBind();
                }

                else if (TabContainer1.ActiveTabIndex == 4) loadChatList();

                else if (TabContainer1.ActiveTabIndex == 5)
                {
                    sqlDB.fillDataTable("select ComposeMail_Id,EmpId,NickName,Subject,Format(CDate,'MMM dd') as CDate,IsRead from v_Mail_ComposeMail_Info where rxUserId=" + ViewState["__getUserId__"].ToString() + " ", dt = new DataTable());
                    gvComposeMail.DataSource = dt;
                    gvComposeMail.DataBind();
                }
            }
            catch { }
        }

        private void loadChatList()
        {
            try
            {
                sqlDB.fillDataTable("select top (20) CId,FirstName,Text,CDateTime from v_Mail_ChatInfo  where RxUserId =" + ViewState["__getUserId__"].ToString() + " order by CId Desc", dt = new DataTable());
                gvChat.DataSource = dt;
                gvChat.DataBind();

                SqlCommand cmd = new SqlCommand("Update Mail_ChatInfo Set Status='1' where RxUserId=" + ViewState["__getUserId__"].ToString() + " AND Status='false'",sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            try
            {
                if (TabContainer1.ActiveTabIndex == 0)
                {
                    loadComplainByModuleType("Personnel");

                    txtPersonnelDetails.Visible = false;
                    gvPersonalInbox.Visible = true;

                    txtLeaveDetails.Visible = false;
                    gvLeave.Visible = false;

                    txtAttendanceDetails.Visible = false;
                    gvAttendance.Visible = false;

                    txtPayrollDetails.Visible = false;
                    gvPayroll.Visible = false;

                    txtChatDetails.Visible = false;
                    gvChat.Visible = false;

                    txtComposeMailDetails.Visible = false;
                    gvComposeMail.Visible = false;

                   
                }
                else if (TabContainer1.ActiveTabIndex == 1)
                {
                    loadComplainByModuleType("Leave");

                    txtPersonnelDetails.Visible = false;
                    gvPersonalInbox.Visible = false;

                    txtLeaveDetails.Visible = false;
                    gvLeave.Visible = true;

                    txtAttendanceDetails.Visible = false;
                    gvAttendance.Visible = false;

                    txtPayrollDetails.Visible = false;
                    gvPayroll.Visible = false;

                    txtChatDetails.Visible = false;
                    gvChat.Visible = false;

                    txtComposeMailDetails.Visible = false;
                    gvComposeMail.Visible = false;
                }
                else if (TabContainer1.ActiveTabIndex == 2)
                {
                    loadComplainByModuleType("Attendance");

                    txtPersonnelDetails.Visible = false;
                    gvPersonalInbox.Visible = false;

                    txtLeaveDetails.Visible = false;
                    gvLeave.Visible = false;

                    txtAttendanceDetails.Visible = false;
                    gvAttendance.Visible = true;

                    txtPayrollDetails.Visible = false;
                    gvPayroll.Visible = false;

                    txtChatDetails.Visible = false;
                    gvChat.Visible = false;

                    txtComposeMailDetails.Visible = false;
                    gvComposeMail.Visible = false;
                }
                else if (TabContainer1.ActiveTabIndex == 3)
                {
                    loadComplainByModuleType("Payroll");
                    txtPersonnelDetails.Visible = false;
                    gvPersonalInbox.Visible = false;

                    txtLeaveDetails.Visible = false;
                    gvLeave.Visible = false;

                    txtAttendanceDetails.Visible = false;
                    gvAttendance.Visible = false;

                    txtPayrollDetails.Visible = false;
                    gvPayroll.Visible = true;

                    txtChatDetails.Visible = false;
                    gvChat.Visible = false;

                    txtComposeMailDetails.Visible = false;
                    gvComposeMail.Visible = false;
                }

                else if (TabContainer1.ActiveTabIndex == 4)
                {
                    loadComplainByModuleType("Chat");
                    txtPersonnelDetails.Visible = false;
                    gvPersonalInbox.Visible = false;

                    txtLeaveDetails.Visible = false;
                    gvLeave.Visible = false;

                    txtAttendanceDetails.Visible = false;
                    gvAttendance.Visible = false;

                    txtPayrollDetails.Visible = false;
                    gvPayroll.Visible = false;

                    txtChatDetails.Visible = false;
                    gvChat.Visible = true;

                    txtComposeMailDetails.Visible = false;
                    gvComposeMail.Visible = false;
                }
                else
                {
                    loadComplainByModuleType("ComposeMail");
                    txtPersonnelDetails.Visible = false;
                    gvPersonalInbox.Visible = false;

                    txtLeaveDetails.Visible = false;
                    gvLeave.Visible = false;

                    txtAttendanceDetails.Visible = false;
                    gvAttendance.Visible = false;

                    txtPayrollDetails.Visible = false;
                    gvPayroll.Visible = false;

                    txtChatDetails.Visible = false;
                    gvChat.Visible = false;

                    txtComposeMailDetails.Visible = false;
                    gvComposeMail.Visible = true;
                }


               
               
            }
            catch { ViewState["__getCurrentTabIndex__"] = TabContainer1.TabIndex; }
        }

        protected void gvPersonalInbox_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                if (e.CommandName.Equals("ForPersonnelDetails"))
                {
                    gvPersonalInbox.Visible = false;
                    sqlDB.fillDataTable("select Details from v_Mail_Complain_Info where ComID="+gvPersonalInbox.DataKeys[rIndex].Value.ToString()+"",dt=new DataTable ());
                    
                    HtmlDocument mainDoc = new HtmlDocument();

                    txtPersonnelDetails.Text = dt.Rows[0]["Details"].ToString();
                    
                    txtPersonnelDetails.Visible = true;

                    cmd = new SqlCommand("Update Mail_Complain_Info set IsRead ='1' where ComID=" + gvPersonalInbox.DataKeys[rIndex].Value.ToString() + "", sqlDB.connection);
                    cmd.ExecuteNonQuery();

                    getInitialMailStatus((byte)TabContainer1.ActiveTabIndex);
                    

                }

                else if (e.CommandName.Equals("ForLeaveDetails"))
                {
                    gvLeave.Visible = false;
                    sqlDB.fillDataTable("select Details from v_Mail_Complain_Info where ComID=" + gvLeave.DataKeys[rIndex].Value.ToString() + "", dt = new DataTable());



                    txtLeaveDetails.Text = dt.Rows[0]["Details"].ToString();
                    txtLeaveDetails.Visible = true;

                    cmd = new SqlCommand("Update Mail_Complain_Info set IsRead ='1' where ComID=" + gvLeave.DataKeys[rIndex].Value.ToString() + "", sqlDB.connection);
                    cmd.ExecuteNonQuery();

                    getInitialMailStatus((byte)TabContainer1.ActiveTabIndex);
                  
                }

                else if (e.CommandName.Equals("ForAttendanceDetails"))
                {
                    gvAttendance.Visible = false;
                    sqlDB.fillDataTable("select Details from v_Mail_Complain_Info where ComID=" + gvAttendance.DataKeys[rIndex].Value.ToString() + "", dt = new DataTable());
                    HtmlDocument mainDoc = new HtmlDocument();

                    txtAttendanceDetails.Text = dt.Rows[0]["Details"].ToString();
                    txtAttendanceDetails.Visible = true;

                    cmd = new SqlCommand("Update Mail_Complain_Info set IsRead ='1' where ComID=" + gvAttendance.DataKeys[rIndex].Value.ToString() + "", sqlDB.connection);
                    cmd.ExecuteNonQuery();

                    getInitialMailStatus((byte)TabContainer1.ActiveTabIndex);
                }

                else if (e.CommandName.Equals("ForPayrollDetails"))
                {
                    gvPayroll.Visible = false;
                    sqlDB.fillDataTable("select Details from v_Mail_Complain_Info where ComID=" + gvPayroll.DataKeys[rIndex].Value.ToString() + "", dt = new DataTable());

                    txtPayrollDetails.Text = dt.Rows[0]["Details"].ToString();
                    txtPayrollDetails.Visible = true;

                    cmd = new SqlCommand("Update Mail_Complain_Info set IsRead ='1' where ComID=" + gvPayroll.DataKeys[rIndex].Value.ToString() + "", sqlDB.connection);
                    cmd.ExecuteNonQuery();

                    getInitialMailStatus((byte)TabContainer1.ActiveTabIndex);
                }
                else if (e.CommandName.Equals("ForComposeMailDetails"))
                {
                    gvComposeMail.Visible = false;
                    sqlDB.fillDataTable("select Details from v_Mail_ComposeMail_Info where ComposeMail_Id=" + gvComposeMail.DataKeys[rIndex].Value.ToString() + "", dt = new DataTable());

                    txtComposeMailDetails.Text = dt.Rows[0]["Details"].ToString();
                    txtComposeMailDetails.Visible = true;

                    cmd = new SqlCommand("Update Mail_ComposeMail_Info set IsRead ='1' where ComposeMail_Id=" + gvComposeMail.DataKeys[rIndex].Value.ToString() + "", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                    getInitialMailStatus((byte)TabContainer1.ActiveTabIndex);
                }
            }
            catch { }
        }

        protected void btnInbox_Click(object sender, EventArgs e)
        {
            try
            {
                if (TabContainer1.ActiveTabIndex == 0)
                {
                    gvPersonalInbox.Visible = true;
                    txtPersonnelDetails.Visible = false;
                    txtPersonnelDetails.Text = "";

                }
                else if (TabContainer1.ActiveTabIndex == 1)
                {
                    gvLeave.Visible = true;
                    txtLeaveDetails.Visible = false;
                    txtLeaveDetails.Text = "";
                }
                else if (TabContainer1.ActiveTabIndex == 2)
                {
                    gvAttendance.Visible = true;
                    txtAttendanceDetails.Visible = false;
                    txtAttendanceDetails.Text = "";
                }
                else if (TabContainer1.ActiveTabIndex == 3)
                {
                    gvPayroll.Visible = true;
                    txtPayrollDetails.Visible = false;
                    txtPayrollDetails.Text = "";
                }
                else if (TabContainer1.ActiveTabIndex == 5)
                {
                    gvComposeMail.Visible = true;
                    txtComposeMailDetails.Visible = false;
                    txtComposeMailDetails.Text = "";
                }
            }
            catch { }
        }

        private void loadLoginUserList()
        {
            try
            {
                sqlDB.fillDataTable("select UserId,FirstName from UserAccount where CompanyId='" + ViewState["__CompanyId__"].ToString() + "' AND IsLogin='true' AND UserId !=" + ViewState["__getUserId__"] .ToString()+ "", dt = new DataTable());
                chkLoginUseList.DataValueField = "UserId";
                chkLoginUseList.DataTextField = "FirstName";
                chkLoginUseList.DataSource = dt;
                chkLoginUseList.DataBind();
            }
            catch { }
        }

        protected void chkLoginUseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Session["__SelectedUser__"] = chkLoginUseList.SelectedItem.Text.ToString();
            }
            catch { }
        }

        protected void btnCompose_Click(object sender, EventArgs e)
        {
            try
            {
                Session["__forCompose__"] = "Yes";
                Session["__PreviousPage__"] = Request.ServerVariables["HTTP_REFERER"].ToString();
                Response.Redirect("~/mail/complain.aspx");
            }
            catch { }
        }
    }
}