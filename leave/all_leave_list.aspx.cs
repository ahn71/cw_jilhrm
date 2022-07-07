using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System.Data.SqlClient;
using System.Drawing;
using SigmaERP.classes;

namespace SigmaERP.personnel
{
    public partial class week_end_list_all : System.Web.UI.Page
    {
        DataTable dt;
        static DataTable dtSetPrivilege;
        string sqlCmd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try { 
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                    try
                    {
                      string  query = Request.QueryString["for"].ToString();
                        rblApprovedPending.SelectedValue = query;
                    }
                    catch
                    { }
                    ViewState["__LineORGroupDependency__"] = classes.commonTask.GroupORLineDependency();
                setPrivilege();
                if (ViewState["__LineORGroupDependency__"].ToString().Equals("False"))
                    classes.commonTask.LoadGrouping(ddlGrouping, ViewState["__CompanyId__"].ToString());
                //SearchLeaveApplication();
                loadYear();
                //ddlEmpType.Items.Insert(0, new ListItem(string.Empty, "0"));
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                SearchLeaveApplication();
                //loadLeaveApplicationAtFirstTime();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
               
            }
            }
            catch { }
        }
 

        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserId__"] = getUserId;
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__EmpId__"] = getCookies["__getEmpId__"].ToString();
                ViewState["__ODOnlyDpt__"] = getCookies["__getODOnlyDpt__"].ToString();
                ViewState["__DptId__"] = getCookies["__getDptId__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForList(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "aplication.aspx", ddlCompanyList, gvLeaveList, btnSearch);
                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];


                
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin")
                {
                    classes.commonTask.loadDepartmentByAdminForLeave(ddlDepartmentList, ViewState["__CompanyId__"].ToString(), ViewState["__UserId__"].ToString());
                    if (ddlDepartmentList.Items.Count == 1)
                    {
                        if (ViewState["__LineORGroupDependency__"].ToString().Equals("True"))
                        {
                            classes.commonTask.LoadGrouping(ddlGrouping, ViewState["__CompanyId__"].ToString(), ddlDepartmentList.SelectedValue);
                        }
                        classes.commonTask.LoadInitialShiftByDepartment(ddlShift, ViewState["__CompanyId__"].ToString(), ddlDepartmentList.SelectedValue);
                    }
                }
                else
                    classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ViewState["__CompanyId__"].ToString());
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "User")
                {
                    ViewState["__ReadAction__"] = "1";
                    ddlDepartmentList.Enabled = false;
                    txtCardNo.Enabled = false;
                    txtCardNo.Text = commonTask.returnEmpCardNoByEmpId(ViewState["__EmpId__"].ToString());

                }




            }
            catch { }
        }
        private void loadYear()
        {
            try
            {
                sqlDB.fillDataTable("select distinct FromYear from v_Leave_LeaveApplication order by FromYear DESC", dt = new DataTable());
                ddlChoseYear.DataTextField = "FromYear";
                ddlChoseYear.DataValueField = "FromYear";
                ddlChoseYear.DataSource = dt;
                ddlChoseYear.DataBind();
               // ddlChoseYear.SelectedIndex = 0;
                ddlChoseYear.Items.Insert(0, new ListItem(string.Empty, "0"));
                ddlChoseYear.SelectedValue = DateTime.Now.Year.ToString();
            }
            catch { }
        }

        //private void loadLeaveApplicationAtFirstTime()
        //{
        //    try
        //    {
        //       // sqlDB.fillDataTable("select Distinct LACode,EmpCardNo,convert (varchar(24),FromDate,113) as FromDate,convert (varchar(24),ToDate,113) as ToDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where (FromYear ='" + System.DateTime.Now.ToString("yyyy") + "' OR ToYear='" + DateTime.Now.Year.ToString() + "') AND CompanyId='" + ViewState["__CompanyId__"].ToString() + "'   AND IsApproved ='true'  order by FromDate desc, EmpCardNo", dt = new DataTable());
        //        sqlDB.fillDataTable("select LACode,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate, WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and  IsApproved ='true' order by FromDate desc, EmpCardNo", dt = new DataTable());
        //        if(dt.Rows.Count<1)
        //        {
        //            gvLeaveList.DataSource = null;
        //            gvLeaveList.DataBind();
        //            divRecordMessage.InnerText = "Any Leave Application Are Not Founded !";
        //            divRecordMessage.Visible = true;
        //            return;
        //        }
        //        gvLeaveList.DataSource = dt;
        //        gvLeaveList.DataBind();
        //    }
        //    catch { }
        //}

        protected void gvLeaveList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete"))
                {
                    string a = e.CommandArgument.ToString();
                    Delete(int.Parse(a));
                   

                }

                if (e.CommandName.Equals("Alter"))
                {
                   
                    int index = Convert.ToInt32(e.CommandArgument);
                    string LACode = gvLeaveList.DataKeys[index].Value.ToString();
                    Response.Redirect("/leave/aplication.aspx?LC="+LACode+"-"+rblApprovedPending.SelectedValue+"");
                }
                if (e.CommandName.Equals("View"))
                    viewLeaveApplication(e.CommandArgument.ToString());
            }
            catch { }
        }

        //private void delete(string getLACode)
        //{
        //    try
        //    {
        //        if (SQLOperation.forDeleteRecordByIdentifier("Leave_LeaveApplication", "LACode", getLACode, sqlDB.connection) == true)
        //        {


        //            lblMessage.Text = "Successfully Deleted.";
        //            loadALlLeaveInfo();
        //        }
        //    }
        //    catch { }
        //}


        protected void gvLeaveList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                    e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";

                   
                    if (e.Row.Cells[7].Text == "Done") e.Row.Cells[7].ForeColor = Color.Blue;               
                    else e.Row.Cells[7].ForeColor = Color.Green;
                }
               
               
            }
            catch { }
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
            {
                Button btn;
                try
                {
                    if (ViewState["__DeletAction__"].ToString().Equals("0"))
                    {
                        btn = new Button();
                        btn = (Button)e.Row.FindControl("btnView");
                        btn.Enabled = false;
                        btn.OnClientClick = "return false";
                        btn.ForeColor = Color.Silver;
                    }

                }
                catch { }
                try
                {
                    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        btn = new Button();
                        btn = (Button)e.Row.FindControl("btnEdit");
                        btn.Enabled = false;
                        btn.ForeColor = Color.Silver;
                    }

                }
                catch { }
               
            }
        }
        private void viewLeaveApplication(string LaCode)
        {
            try
            {
                string getSQLCMD;
                DataTable dt = new DataTable();
                DataTable dtApprovedRejectedDate = new DataTable();
                getSQLCMD = " SELECT LACode,EmpId, format(FromDate,'dd-MM-yyyy') as FromDate, format(ToDate,'dd-MM-yyyy') as ToDate, TotalDays,"
                    + "  Remarks, LeaveName, DsgName, DptName, CompanyName,format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo, "
                    + " EmpName, Address, LvAddress, LvContact, CompanyId, DptId, format(ApplyDate,'dd-MM-yyyy') as ApplyDate,HEmpCardNo,HEmpName,HDptName,HDsgName,LastLeaveDate,LastLeaveNature,LastLeaveDays,IsHalfDayLeave "
                    + " FROM"
                    + " dbo.v_Leave_LeaveApplication"
                    + " where LACode=" + LaCode + "";
                sqlDB.fillDataTable(getSQLCMD, dt);
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->No record available."; return;
                }
                string[] FDate = dt.Rows[0]["FromDate"].ToString().Split('-');
                Session["__Language__"] = "English";
                Session["__LeaveApplication__"] = dt;
                getSQLCMD = "with dCL as(select 'c/l' as ShortName,ISNULL( sum(Days),0) as Deducted from Leave_MonthlyLeaveDeductionRecord where EmpID='" + dt.Rows[0]["EmpId"].ToString() + "' and Month>='" + FDate[2] + "-01-01' and Month<'" + FDate[2] + "-" + FDate[1] + "-" + FDate[0] + "')," +
                    " lvd as ( select CompanyId, Leaveid, ShortName,sum( case when IsHalfDayLeave=1 then 0.5 else 1 end) as Amount,case when Sex='Male' then 'm/l'else '' end Sex " +
                     " from v_Leave_LeaveApplicationDetails where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' and IsApproved=1 and LeaveDate>='" + FDate[2] + "-01-01' and LeaveDate<'" + FDate[2] + "-" + FDate[1] + "-" + FDate[0] + "'" +
                     " group by CompanyId,Leaveid, ShortName,Sex),pcs as (select case when Sex='Male' then 'm/l'else '' end Sex,CompanyId from v_EmployeeDetails where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' and IsActive=1 ) ,lc as ( select * from tblLeaveConfig where CompanyId=(select CompanyId from pcs)) ,"+
                     " la as(select  LeaveId,TotalDays from Leave_LeaveApplication where LACode=" + LaCode + ")" +
                     " select lc.ShortName,ISNULL(lvd.Amount,0) as Amount,lc.LeaveDays,lc.CompanyId,lc.LeaveName,( lc.LeaveDays-(ISNULL(lvd.Amount,0)+ISNULL(dCL.Deducted,0)) ) as Remaining,TotalDays Applied, dCL.Deducted from  lc left join lvd on lc.LeaveId=lvd.LeaveId and lc.CompanyId=lvd.CompanyId  left join la on lc.LeaveId=la.LeaveId left join dCL on lvd.ShortName = dCL.ShortName " +
                     " where  lc.ShortName not in('sr/l',(select   Sex from pcs))";
                sqlDB.fillDataTable(getSQLCMD, dt = new DataTable());
                Session["__LeaveCurrentStatus__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LeaveApplication');", true);  //Open New Tab for Sever side code

            }
            catch { }
        }

        private void Delete(int id)
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("select EmpId,Convert(varchar(11),FromDate,111) as FromDate,Convert(varchar(11),ToDate,111) as ToDate,LeaveName,EmpCardNo,EmpTypeId from v_Leave_LeaveApplication where Lacode =" + id.ToString() + "", dt);

            if (dt.Rows[0]["LeaveName"].ToString().ToLower().Equals("maternity leave") || dt.Rows[0]["LeaveName"].ToString().ToLower().Equals("m/l") || dt.Rows[0]["LeaveName"].ToString().ToLower().Equals("ml"))
            {
                changeEmpTypeOnBaseMaternityLeaveDelete(dt.Rows[0]["EmpId"].ToString());

            }

            if (SQLOperation.forDeleteRecordByIdentifier("Leave_LeaveApplication", "LACode", id.ToString(), sqlDB.connection) == true)
            {
                string getEmpTypeId = dt.Rows[0]["EmpTypeId"].ToString();
                string getEmpCardNo = dt.Rows[0]["EmpCardNo"].ToString();

                sqlDB.fillDataTable("select ATTStatus,convert(varchar(11),AttDate,111)as AttDate from tblAttendanceRecord where EmpCardNo like'%" + getEmpCardNo + "' AND EmpTypeId=" + getEmpTypeId + " AND ATTDate >='" + dt.Rows[0]["FromDate"].ToString() + "' AND AttDate <='" + dt.Rows[0]["ToDate"].ToString() + "'", dt = new DataTable());

                for (byte b = 0; b < dt.Rows.Count; b++)
                {
                    if (!dt.Rows[b]["ATTStatus"].ToString().Equals("W"))
                    {

                        SqlCommand cmd = new SqlCommand("update tblAttendanceRecord set ATTStatus='A',StateStatus=' ',DailyStartTimeALT='00:00:00:00' where EmpCardNo like'%" + getEmpCardNo + "' AND EmpTypeId=" + getEmpTypeId + " AND ATTDate ='" + dt.Rows[b]["AttDate"].ToString() + "' ", sqlDB.connection);
                        cmd.ExecuteNonQuery();
                    }

                }

            }
        }

        private void changeEmpTypeOnBaseMaternityLeaveDelete(string EmpId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("update Personnel_EmpCurrentStatus set EmpStatus='1' where SN=(select Max (SN) from Personnel_EmpCurrentStatus where EmpId='" + EmpId + "' and ActiveSalary='true' and IsActive=1)", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        protected void gvLeaveList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            if (rblApprovedPending.SelectedValue == "Rejected")
                SearchLeaveApplicationRejected();
            else
            SearchLeaveApplication();
            gvLeaveList.PageIndex = e.NewPageIndex;
            gvLeaveList.DataBind();
            //try
            //{
            //    SearchLeaveApplication();
            //}
            //catch { }
            //gvLeaveList.PageIndex = e.NewPageIndex;
            //Session["pageNumber"] = e.NewPageIndex;
            //gvLeaveList.DataBind();
        }

        protected void gvLeaveList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            System.Threading.Thread.Sleep(1000);
            if (rblApprovedPending.SelectedValue == "Rejected")
                SearchLeaveApplicationRejected();
            else
            SearchLeaveApplication();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                if (rblApprovedPending.SelectedValue == "Rejected")
                    SearchLeaveApplicationRejected();
                else
                    SearchLeaveApplication();
            }
            catch { }
        }
        private void SearchLeaveApplication()
        {
            try
            {

                string EmpId = (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("User")) ? " and EmpId='" + ViewState["__EmpId__"].ToString() + "'" : "";

                string AdminCondition = (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin")) ? "  and (EmpId='" + ViewState["__EmpId__"].ToString() + "' or EmpId in(select EmpId from  tblLeaveAuthorityAccessControl where AuthorityID=" + ViewState["__UserId__"].ToString() + ")) " : "";
                //if (ddlCompanyList.SelectedIndex == 0 && txtCardNo.Text.Trim().Length == 0)
                //{
                //    lblMessage.InnerText = "warning-> Please, Select Company Name.";
                //    return;
                //}

                if (txtCardNo.Text.Trim() != "")
                {
                    if (txtCardNo.Text.Length < 4)
                    { lblMessage.InnerText = "warning-> Please Type Employee Card No Minimum 4 Character!"; return; }
                }
                if (txtFromDate.Text.Trim().Length != 0 || txtToDate.Text.Trim().Length != 0)
                {
                    string[] dates = txtFromDate.Text.Trim().Split('-');
                    ViewState["__FDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                    dates = txtToDate.Text.Trim().Split('-');
                    ViewState["__TDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                    ddlChoseYear.SelectedIndex = 0;
                }
                //if (txtCardNo.Text.Trim() == "" && ddlCompanyList.Text.Trim() != "" && ddlShift.SelectedIndex < 1 && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && ((txtToDate.Text.Trim() != "" && txtFromDate.Text.Trim() != "")))
                //{
                //    lblMessage.InnerText = "warning-> Please, Select a Department.";
                //    return;
                //}
                if (ddlCompanyList.SelectedItem.Text.Trim() == "")
                {
                    ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                }

                //1.1 Search by Company,Year
                if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlChoseYear.SelectedIndex > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedItem.Text.Trim()=="")  && txtCardNo.Text.Trim().Length == 0)
                    sqlCmd = "select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where   CompanyId='" + ddlCompanyList.SelectedValue + "' and ( FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "') and IsApproved ='" + rblApprovedPending.SelectedValue + "' " + EmpId + AdminCondition + " order by convert(varchar,FromDate,120) desc, EmpCardNo";
                //1.2 Search by Company,FromDate,ToDate
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlChoseYear.SelectedIndex == -1 || ddlChoseYear.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length>0 && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedItem.Text.Trim() == "") && txtCardNo.Text.Trim().Length == 0)
                    sqlCmd = "select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where   CompanyId='" + ddlCompanyList.SelectedValue + "' and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "' and  IsApproved ='" + rblApprovedPending.SelectedValue + "' " + EmpId + AdminCondition + " order by convert(varchar,FromDate,120) desc, EmpCardNo";
                //2.1 Search by Company,Year,CardNo
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlChoseYear.SelectedIndex > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedItem.Text.Trim()=="")  && txtCardNo.Text.Trim().Length!= 0)
                    sqlCmd = "select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where   CompanyId='" + ddlCompanyList.SelectedValue + "' and ( FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "') and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "' and IsApproved ='" + rblApprovedPending.SelectedValue + "' " + EmpId + AdminCondition + " order by convert(varchar,FromDate,120) desc, EmpCardNo";
                //2.2 Search by Company,FromDate,ToDate,CardNo
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlChoseYear.SelectedIndex == -1 || ddlChoseYear.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length>0 && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedItem.Text.Trim() == "") && txtCardNo.Text.Trim().Length != 0)
                    sqlCmd = "select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where   CompanyId='" + ddlCompanyList.SelectedValue + "' and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "' and  IsApproved ='" + rblApprovedPending.SelectedValue + "' " + EmpId + AdminCondition + " order by convert(varchar,FromDate,120) desc, EmpCardNo";
                //2.1 Search by Company,Department,year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlChoseYear.SelectedIndex > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 &&  ddlDepartmentList.SelectedItem.Text.Trim()!= "" && txtCardNo.Text.Trim().Length == 0)
                    sqlCmd = "select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where   CompanyId='" + ddlCompanyList.SelectedValue + "' and DptID='"+ ddlDepartmentList.SelectedValue+"' and ( FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "') and  IsApproved ='" + rblApprovedPending.SelectedValue + "' " + EmpId + AdminCondition + " order by convert(varchar,FromDate,120) desc, EmpCardNo";
                //2.2 Search by Company,Department,FromDate,ToDate
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlChoseYear.SelectedIndex == -1 || ddlChoseYear.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length>0 && ddlDepartmentList.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0)
                    sqlCmd = "select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where   CompanyId='" + ddlCompanyList.SelectedValue + "'  and DptID='" + ddlDepartmentList.SelectedValue + "' and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "' and  IsApproved ='" + rblApprovedPending.SelectedValue + "' " + EmpId + AdminCondition + " order by convert(varchar,FromDate,120) desc, EmpCardNo";
                //3.1 Search by Company,Department,year,CardNo
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlChoseYear.SelectedIndex > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && ddlDepartmentList.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length != 0)
                    sqlCmd = "select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where   CompanyId='" + ddlCompanyList.SelectedValue + "' and DptID='" + ddlDepartmentList.SelectedValue + "' and ( FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "') and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "' and  IsApproved ='" + rblApprovedPending.SelectedValue + "' " + EmpId + AdminCondition + " order by convert(varchar,FromDate,120) desc, EmpCardNo";
                //3.2 Search by Company,Department,FromDate,ToDate,CardNo
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlChoseYear.SelectedIndex == -1 || ddlChoseYear.SelectedItem.Text.Trim() == "") && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlDepartmentList.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length != 0)
                    sqlCmd = "select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where   CompanyId='" + ddlCompanyList.SelectedValue + "'  and DptID='" + ddlDepartmentList.SelectedValue + "' and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "' and  IsApproved ='" + rblApprovedPending.SelectedValue + "' " + EmpId + AdminCondition + " order by convert(varchar,FromDate,120) desc, EmpCardNo";



                sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
                //------------------------------------------------------------------------------------------------
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->No record available.";
                    gvLeaveList.DataSource = null;
                    gvLeaveList.DataBind();
                    return;
                }
                gvLeaveList.DataSource = dt;
                gvLeaveList.DataBind();
                if (EmpId == "")
                {
                    gvLeaveList.Columns[9].Visible = true;
                    gvLeaveList.Columns[10].Visible = true;
                    gvLeaveList.Columns[11].Visible = true;
                }
                else if (rblApprovedPending.SelectedValue == "1")
                {
                    gvLeaveList.Columns[10].Visible = false;
                    gvLeaveList.Columns[11].Visible = false;
                }
                else
                {
                    gvLeaveList.Columns[9].Visible = true;
                    gvLeaveList.Columns[10].Visible = true;
                    gvLeaveList.Columns[11].Visible = true;
                }


            }
            catch { }
        }
        private void SearchLeaveApplication_()
        {
            try
            {

                string EmpId = (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("User")) ? " and EmpId='" + ViewState["__EmpId__"] .ToString()+ "'" : "";

                string AdminCondition = (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin")) ? "  and (EmpId='" + ViewState["__EmpId__"].ToString() + "' or EmpId in(select EmpId from  tblLeaveAuthorityAccessControl where AuthorityID=" + ViewState["__UserId__"].ToString() + ")) " : "";
                //if (ddlCompanyList.SelectedIndex == 0 && txtCardNo.Text.Trim().Length == 0)
                //{
                //    lblMessage.InnerText = "warning-> Please, Select Company Name.";
                //    return;
                //}

                if (txtCardNo.Text.Trim() != "")
                {
                    if (txtCardNo.Text.Length < 4)
                    { lblMessage.InnerText = "warning-> Please Type Employee Card No Minimum 4 Character!"; return; }
                }
                if (txtFromDate.Text.Trim().Length != 0 || txtToDate.Text.Trim().Length != 0)
                {
                    string[] dates = txtFromDate.Text.Trim().Split('-');
                    ViewState["__FDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                    dates = txtToDate.Text.Trim().Split('-');
                    ViewState["__TDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                    ddlChoseYear.SelectedIndex = 0;
                }
                if (txtCardNo.Text.Trim() == "" && ddlCompanyList.Text.Trim() != "" && ddlShift.SelectedIndex < 1 && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && ((txtToDate.Text.Trim() != "" && txtFromDate.Text.Trim() != "")))
                {
                    lblMessage.InnerText = "warning-> Please, Select a Department.";
                    return;
                }
                if (ddlCompanyList.SelectedItem.Text.Trim()=="")
                {
                    ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                }
              
                //1. Search by Company,Year
                if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlChoseYear.SelectedIndex>0  && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length== 0)
                    sqlCmd = "select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where  ( FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "') and CompanyId='" + ddlCompanyList.SelectedValue + "'and  IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition+" order by FromDate desc, EmpCardNo";
                 
                //if (txt)
                //1. Search by Company, CardNo.
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && (ddlChoseYear.SelectedIndex == -1 || ddlChoseYear.SelectedIndex == 0) && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlCmd = "select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "'  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + " order by FromDate desc, EmpCardNo";
                 //2. Search by Company,Department,Card No
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim()!="" && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && ddlChoseYear.SelectedIndex <1 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "'  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + " order by FromDate desc, EmpCardNo";
                // 3. Search by Company,Department,Shift  
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtCardNo.Text.Trim().Length == 0 && ddlChoseYear.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0)
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "'  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + " order by FromDate desc, EmpCardNo";
                // 4. Search by Company,Department,Shift,CardNo 
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "'  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "'  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                //5. Search by Company,Department,Shift,CardNo,From Date,To Date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0)
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "'  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "'and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                //6. Search by Company,Department,Shift,CardNo,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0)
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "'  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "' and ( FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                //7. Search by Company,Department,Shift,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0)
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and (FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')   AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                //8. Search by Company,Department,Shift,From date,To Date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0)
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                //9. Search by Company, Department
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and  IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + " order by FromDate desc, EmpCardNo";
                //10. Search by Company, CardNo,From date,To date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and FromDate >='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "'  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                //11. Search by Company,Department,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim()=="")
                {
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and (FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                }
                       //12.  Search by Company, Shift
                  else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && ddlDepartmentList.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "'  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                //13.  Search by Company, Shift,Card No
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex>0 && ddlDepartmentList.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "'  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                //14. Search by Company, Department, FromDate,ToDate
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and FromDate >='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
               //15. Search by Company, Department, FromDate,ToDate,Card no.
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length > 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and FromDate >='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "'  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";

                //------------------------------Search By Line or Group------------------------------------------------

                // 4. Search by Company,Department,Shift,Grouping 
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && txtCardNo.Text.Trim().Length == 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() == "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "   AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";

                            // 4. Search by Company,Department,Shift,Grouping ,CardNo
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() == "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + txtCardNo.Text.Trim() + "'   AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";


                 //5. Search by Company,Department,Shift,CardNo,From Date,To Date,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + txtCardNo.Text.Trim() + "' and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'   AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                //6. Search by Company,Department,Shift,CardNo,Year,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + txtCardNo.Text.Trim() + "' and (FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')   AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                //7. Search by Company,Department,Shift,Year,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and (FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "   order by FromDate desc, EmpCardNo";
                //8. Search by Company,Department,Shift,From date,To Date,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'   AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";


                   // 4. Search by Company,Department,Grouping 
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtCardNo.Text.Trim().Length == 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() == "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "   AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";

                          // 4. Search by Company,Department,Grouping ,CardNo
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + txtCardNo.Text.Trim() + "'  AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "   order by FromDate desc, EmpCardNo";


                 //5. Search by Company,Department,CardNo,From Date,To Date,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + txtCardNo.Text.Trim() + "'and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'   AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                //6. Search by Company,Department,CardNo,Year,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + txtCardNo.Text.Trim() + "' and (FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')   AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                //7. Search by Company,Department,Year,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and (FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')   AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";
                //8. Search by Company,Department,From date,To Date,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlCmd = "select LACode,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'   AND IsApproved ='"+rblApprovedPending.SelectedValue+"' "+EmpId + AdminCondition + "  order by FromDate desc, EmpCardNo";

                sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
                //------------------------------------------------------------------------------------------------

                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->No record available.";
                    gvLeaveList.DataSource = null;
                    gvLeaveList.DataBind();
                    return;
                }
                gvLeaveList.DataSource = dt;
                gvLeaveList.DataBind();
                if (EmpId == "")
                {
                    gvLeaveList.Columns[9].Visible = true;
                    gvLeaveList.Columns[10].Visible = true;
                    gvLeaveList.Columns[11].Visible = true;
                }
                else if (rblApprovedPending.SelectedValue == "1")
                {
                    gvLeaveList.Columns[10].Visible = false;
                    gvLeaveList.Columns[11].Visible = false;
                }
                else 
                {
                    gvLeaveList.Columns[9].Visible = true;
                    gvLeaveList.Columns[10].Visible = true;
                    gvLeaveList.Columns[11].Visible = true;
                }
               

            }
            catch { }
        }

        private void SearchLeaveApplicationRejected()
        {
            try
            {

                string EmpId = (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("User")) ? " and EmpId='" + ViewState["__EmpId__"].ToString() + "'" : "";
                //if (ddlCompanyList.SelectedIndex == 0 && txtCardNo.Text.Trim().Length == 0)
                //{
                //    lblMessage.InnerText = "warning-> Please, Select Company Name.";
                //    return;
                //}

                if (txtCardNo.Text.Trim() != "")
                {
                    if (txtCardNo.Text.Length < 4)
                    { lblMessage.InnerText = "warning-> Please Type Employee Card No Minimum 4 Character!"; return; }
                }
                if (txtFromDate.Text.Trim().Length != 0 || txtToDate.Text.Trim().Length != 0)
                {
                    string[] dates = txtFromDate.Text.Trim().Split('-');
                    ViewState["__FDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                    dates = txtToDate.Text.Trim().Split('-');
                    ViewState["__TDate__"] = dates[2] + "-" + dates[1] + "-" + dates[0];
                    ddlChoseYear.SelectedIndex = 0;
                }
                if (txtCardNo.Text.Trim() == "" && ddlCompanyList.Text.Trim() != "" && ddlShift.SelectedIndex < 1 && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && ((txtToDate.Text.Trim() != "" && txtFromDate.Text.Trim() != "") ))
                {
                    lblMessage.InnerText = "warning-> Please, Select a Department.";
                    return;
                }
                if (ddlCompanyList.SelectedItem.Text.Trim() == "")
                {
                    ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                }
                //1. Search by Company and year
                if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlChoseYear.SelectedIndex>0 && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0)
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where  ( FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "') and CompanyId='" + ddlCompanyList.SelectedValue + "'  " + EmpId + " order by FromDate desc, EmpCardNo", dt = new DataTable());

                //if (txt)
                //1. Search by Company, CardNo.
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "'   " + EmpId + " order by FromDate desc, EmpCardNo", dt = new DataTable());
                //2. Search by Company,Department,Card No
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "'   " + EmpId + " order by FromDate desc, EmpCardNo", dt = new DataTable());
                // 3. Search by Company,Department,Shift  
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && txtCardNo.Text.Trim().Length == 0 && ddlChoseYear.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0)
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "'   " + EmpId + " order by FromDate desc, EmpCardNo", dt = new DataTable());
                // 4. Search by Company,Department,Shift,CardNo 
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "'  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "'   " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //5. Search by Company,Department,Shift,CardNo,From Date,To Date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "'  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "'and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'   " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //6. Search by Company,Department,Shift,CardNo,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "'  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "' and ( FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')   " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //7. Search by Company,Department,Shift,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0)
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and (FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')    " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //8. Search by Company,Department,Shift,From date,To Date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'   " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //9. Search by Company, Department
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' " + EmpId + " order by FromDate desc, EmpCardNo", dt = new DataTable());
                //10. Search by Company, CardNo,From date,To date
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && (ddlDepartmentList.SelectedIndex == -1 || ddlDepartmentList.SelectedIndex == 0) && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length > 0)
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "' and FromDate >='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "'   " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //11. Search by Company,Department,Year
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && (ddlShift.SelectedIndex == -1 || ddlShift.SelectedIndex == 0) && ddlChoseYear.SelectedItem.Text.Trim() != "")
                {
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and (FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')   " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                }
                //12.  Search by Company, Shift
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && ddlDepartmentList.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "'   " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //13.  Search by Company, Shift,Card No
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && ddlDepartmentList.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && txtCardNo.Text.Trim().Length > 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "'   " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //14. Search by Company, Department, FromDate,ToDate
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and FromDate >='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'   " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //15. Search by Company, Department, FromDate,ToDate,Card no.
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && txtCardNo.Text.Trim().Length > 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedIndex == -1))
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and FromDate >='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "'   " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());

                //------------------------------Search By Line or Group------------------------------------------------

                // 4. Search by Company,Department,Shift,Grouping 
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && txtCardNo.Text.Trim().Length == 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() == "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "    " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());

                            // 4. Search by Company,Department,Shift,Grouping ,CardNo
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() == "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + txtCardNo.Text.Trim() + "'    " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());


                 //5. Search by Company,Department,Shift,CardNo,From Date,To Date,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + txtCardNo.Text.Trim() + "' and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'    " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //6. Search by Company,Department,Shift,CardNo,Year,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + txtCardNo.Text.Trim() + "' and (FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')    " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //7. Search by Company,Department,Shift,Year,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and (FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')   " + EmpId + "   order by FromDate desc, EmpCardNo", dt = new DataTable());
                //8. Search by Company,Department,Shift,From date,To Date,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedIndex > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "'and SftId='" + ddlShift.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'    " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());


                   // 4. Search by Company,Department,Grouping 
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtCardNo.Text.Trim().Length == 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() == "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "    " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());

                          // 4. Search by Company,Department,Grouping ,CardNo
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length == 0 && txtToDate.Text.Trim().Length == 0 && (ddlChoseYear.SelectedIndex == 0 || ddlChoseYear.SelectedItem.Text.Trim() != "") && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + txtCardNo.Text.Trim() + "'   " + EmpId + "   order by FromDate desc, EmpCardNo", dt = new DataTable());


                 //5. Search by Company,Department,CardNo,From Date,To Date,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtCardNo.Text.Trim().Length > 0 && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + txtCardNo.Text.Trim() + "'and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'    " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //6. Search by Company,Department,CardNo,Year,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + "  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like '%" + txtCardNo.Text.Trim() + "' and (FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')    " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //7. Search by Company,Department,Year,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && ddlChoseYear.SelectedItem.Text.Trim() != "" && txtCardNo.Text.Trim().Length == 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and (FromYear='" + ddlChoseYear.SelectedValue + "' OR ToYear='" + ddlChoseYear.SelectedValue + "')    " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());
                //8. Search by Company,Department,From date,To Date,Grouping
                else if (ddlCompanyList.SelectedItem.Text.Trim() != "" && ddlDepartmentList.SelectedItem.Text.Trim() != "" && ddlShift.SelectedItem.Text.Trim() == "" && txtFromDate.Text.Trim().Length > 0 && txtToDate.Text.Trim().Length > 0 && ddlGrouping.SelectedItem.Text.Trim() != "")
                    sqlDB.fillDataTable("select LACode,EmpName,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(FromDate,'dd-MM-yyyy') as FromDate,format(ToDate,'dd-MM-yyyy') as ToDate,format(ApplyDate,'dd-MM-yyyy') as ApplyDate,WeekHolydayNo,TotalDays,IsApproved,'Rejected' CurrentProcessStatus,LeaveName from v_Leave_LeaveApplication_Log where EmpStatus in ('1','8')  and CompanyId='" + ddlCompanyList.SelectedValue + "'and DptId='" + ddlDepartmentList.SelectedValue + "' and GId=" + ddlGrouping.SelectedValue + " and FromDate>='" + ViewState["__FDate__"].ToString() + "' and ToDate<='" + ViewState["__TDate__"].ToString() + "'    " + EmpId + "  order by FromDate desc, EmpCardNo", dt = new DataTable());

                //------------------------------------------------------------------------------------------------

                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->No record available.";
                    gvLeaveList.DataSource = null;
                    gvLeaveList.DataBind();
                    return;
                }
                gvLeaveList.DataSource = dt;
                gvLeaveList.DataBind();
                gvLeaveList.Columns[9].Visible=false;
                gvLeaveList.Columns[10].Visible = false;
                gvLeaveList.Columns[11].Visible = false;
            }
            catch { }
        }
       

        bool AtFirstTimeLoadLeaveList = true;
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Response.Redirect("/leave/all_leave_list.aspx");

        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               
                //classes.commonTask.loadDivision(ddlDepartmentList, ddlCompanyList.SelectedValue.ToString(), "Admin");
                //classes.commonTask.LoadShift(ddlShift, ddlCompanyList.SelectedValue.ToString(), "Admin");
                if (ddlCompanyList.SelectedValue == "0000")
                {
                    ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                }
                // classes.commonTask.LoadShift(ddlShift, ddlCompanyList.SelectedValue.ToString());
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin")
                {
                    classes.commonTask.loadDepartmentByAdminForLeave(ddlDepartmentList, ViewState["__CompanyId__"].ToString(), ViewState["__UserId__"].ToString());
                    if (ddlDepartmentList.Items.Count == 1)
                    {
                        if (ViewState["__LineORGroupDependency__"].ToString().Equals("True"))
                        {
                            classes.commonTask.LoadGrouping(ddlGrouping, ViewState["__CompanyId__"].ToString(), ddlDepartmentList.SelectedValue);
                        }
                        classes.commonTask.LoadInitialShiftByDepartment(ddlShift, ViewState["__CompanyId__"].ToString(), ddlDepartmentList.SelectedValue);
                    }
                }
                else
                classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ddlCompanyList.SelectedValue.ToString());
                gvLeaveList.DataSource = null;
                gvLeaveList.DataBind();
                if (rblApprovedPending.SelectedValue == "Rejected")
                    SearchLeaveApplicationRejected();
                else
                SearchLeaveApplication();
            }
            catch { }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            allClear();
           
        }
        private void allClear() 
        {
            
            ddlChoseYear.SelectedIndex = 0;
            //ddlCompanyList.SelectedIndex = 0;
            ddlDepartmentList.SelectedIndex = -1;
            ddlShift.SelectedIndex = -1;
            txtToDate.Text = "";
            txtFromDate.Text = "";
            txtCardNo.Text = "";
        }

        protected void ddlShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblApprovedPending.SelectedValue == "Rejected")
                SearchLeaveApplicationRejected();
            else
            SearchLeaveApplication();
           
        }

        protected void ddlDivisionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["__LineORGroupDependency__"].ToString().Equals("True"))
            {
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue;
                classes.commonTask.LoadGrouping(ddlGrouping, CompanyId, ddlDepartmentList.SelectedValue);
            }
            classes.commonTask.LoadInitialShiftByDepartment(ddlShift, ddlCompanyList.SelectedValue, ddlDepartmentList.SelectedValue);
            if (rblApprovedPending.SelectedValue == "Rejected")
                SearchLeaveApplicationRejected();
            else
            SearchLeaveApplication();
           
        }

        protected void ddlChoseYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFromDate.Text = "";
            txtToDate.Text = "";
            if (rblApprovedPending.SelectedValue == "Rejected")
                SearchLeaveApplicationRejected();
            else
            SearchLeaveApplication();
           
        }

        protected void ddlGrouping_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblApprovedPending.SelectedValue == "Rejected")
                SearchLeaveApplicationRejected();
            else
            SearchLeaveApplication();
        }

        protected void rblApprovedPending_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblApprovedPending.SelectedValue == "Rejected")
                SearchLeaveApplicationRejected();
            else
            SearchLeaveApplication();
        }

    }
}