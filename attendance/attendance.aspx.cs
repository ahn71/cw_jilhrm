using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;
using adviitRuntimeScripting;
using System.Data.SqlClient;
using ComplexScriptingSystem;
using System.Globalization;
using SigmaERP.classes;


namespace SigmaERP.attendance
{
    public partial class attendance : System.Web.UI.Page
    {

        string strSQL = "";
      
        SqlDataAdapter da;
        SqlCommand cmd;
        DataTable dt; DataTable dtEmpInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();


            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                string[] ForAlter = {" "};
                try
                {
                    ForAlter = Request.QueryString["eid_cn_at"].Split('_');
                }
                catch { }
                setPrivilege();          
                tdFromDate.InnerText = "Date";
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");               
                DataTable dt = classes.mCommon_Module_For_AttendanceProcessing.Load_Process_AttendanceData(ddlCompanyList.SelectedValue,"0", DateTime.Now.ToString("yyyy-MM-dd"), true, "");
                gvAttendance.DataSource = dt;
                gvAttendance.DataBind();
                ViewState["__OT__"] = "0"; ;
                if (ForAlter[0] != " ") setValueForAlter(ForAlter[0],ForAlter[1],ForAlter[2],ForAlter[3],ForAlter[4],ForAlter[5],ForAlter[6],ForAlter[7],ForAlter[8]);
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
        }

        static DataTable dtSetPrivilege; 
        private void setPrivilege()
        {
            try
            {
               
                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__getUserId__"] = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();



                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), ViewState["__getUserId__"].ToString(), ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "attendance.aspx", ddlCompanyList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];


                if(ViewState["__ReadAction__"].ToString().Equals("0"))
                {
                    gvAttendance.Visible = false;
                    btnShow.Enabled = false;
                    btnShow.CssClass = "";
                }             
            
            }
            catch { }
        }

        private void setValueForAlter(string empId, string empCardNo, string AttDate, string AttStatus, string InTime,string OutTime,string EmpType,string EmpName,string StateStaus)
        {
            try
            {
                // controls are disable when altered an attendance -------------
               
                btnFindEmpInfo.Enabled = false;
                txtFromDate.Enabled = false;
                txtEmpCardNo.Enabled = false;
                //--------------------------------------------------------------

                txtFromDate.Enabled = false;
                btnSave.Text = "Update";

                if (ViewState["__UpdateAction__"].Equals("0"))
                {
                    btnSave.Enabled = false;
                    btnSave.CssClass = "";
                }
                else
                {
                    btnSave.Enabled = true;
                    btnSave.CssClass = "Mbutton";
                }

                sqlDB.fillDataTable("select SftId,DptName,CompanyId from v_Personnel_EmpCurrentStatus where EmpId='"+empId+"'",dt=new DataTable ());

                divFindInfo.InnerText = "Name:" + EmpName + ",Department:" + dt.Rows[0]["DptName"].ToString();
                divFindInfo.Style.Add("color","green");
                ddlCompanyList.SelectedValue = dt.Rows[0]["CompanyId"].ToString();
                //ddlShiftName.SelectedValue = dt.Rows[0]["SftId"].ToString();
                ddlAttendanceTemplate.SelectedValue = "";
                txtEmpCardNo.Text = empCardNo;
               

                ViewState["__EmpId__"] = empId;
                txtFromDate.Text = AttDate;
                string[] InTimes = InTime.Split(':');
                txtInHur.Text = InTimes[0];
                txtInMin.Text = InTimes[1];

                sqlDB.fillDataTable("select InTime12Format,OutTime12Format  from v_v_tblAttendanceRecord where attDate='" + AttDate.Substring(6, 4) + "-" + AttDate.Substring(3, 2) + "-" + AttDate.Substring(0, 2) + "' AND EmpId='" + empId + "'", dt = new DataTable());

                InTimes = dt.Rows[0]["InTime12Format"].ToString().Split(' '); // to get am or pm 
                ddlInTimeAMPM.SelectedValue = InTimes[1];

                string[] OutTimes = OutTime.Split(':');
                txtOutHur.Text = OutTimes[0];
                txtOutMin.Text = OutTimes[1];

                InTimes = dt.Rows[0]["OutTime12Format"].ToString().Split(' '); // to get am or pm 
                ddlOutTimeAMPM.SelectedValue = InTimes[1];

                if (AttStatus.ToLower().Equals("p")) ddlAttendanceTemplate.SelectedValue = "p";
                else if (AttStatus.ToLower().Equals("a")) ddlAttendanceTemplate.SelectedValue="a";
                else if (AttStatus.ToLower().Equals("l")) ddlAttendanceTemplate.SelectedValue = "l";
                else if (AttStatus.ToLower().Equals("lv") && StateStaus.Equals("Casula Leave")) ddlAttendanceTemplate.SelectedValue = "c/l";
                else if (AttStatus.ToLower().Equals("lv") && StateStaus.Equals("Sick Leave")) ddlAttendanceTemplate.SelectedValue = "s/l";
                else if (AttStatus.ToLower().Equals("lv") && StateStaus.Equals("Annual Leave")) ddlAttendanceTemplate.SelectedValue = "a/l";
                else if (AttStatus.ToLower().Equals("lv") && StateStaus.Equals("Maternity Leave")) ddlAttendanceTemplate.SelectedValue = "m/l";
                else if (AttStatus.ToLower().Equals("lv") && StateStaus.Equals("Official Purpose Leave")) ddlAttendanceTemplate.SelectedValue = "op/l";
                else if (AttStatus.ToLower().Equals("lv") && StateStaus.Equals("Others Leave")) ddlAttendanceTemplate.SelectedValue = "o/l";
                else if (AttStatus.ToLower().Equals("w")) ddlAttendanceTemplate.SelectedValue = "w";
                else if (AttStatus.ToLower().Equals("h")) ddlAttendanceTemplate.SelectedValue = "h";

            }
            catch { }
        }
        
        void Clear()
        {

        
            btnFindEmpInfo.Enabled = true;
            txtFromDate.Enabled = true;
            txtEmpCardNo.Enabled = true;
            divFindInfo.InnerText = "";
            txtEmpCardNo.Text = "";
            if (ViewState["__WriteAction__"].Equals("0"))
            {
                btnSave.Enabled = false;
                btnSave.CssClass = "";
            }
            else
            {
                btnSave.Enabled = true;
                btnSave.CssClass = "Mbutton";
            }
            ddlAttendanceTemplate.SelectedIndex = -1;
            txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txtToDate.Text = "";
            txtInHur.Text = "0";
            txtInMin.Text = "0";
            txtOutForLunchHr.Text = "0";
            txtOutForLunchMin.Text = "0";
            txtInAfterLunchHr.Text = "0";
            txtInAfterLunchMin.Text = "0";
            txtOutHur.Text = "0";
            txtOutMin.Text = "0";
            btnFindEmpInfo.Enabled = true;
            txtRemark.Text = "";
        }

        void LoadAttendanceForPageLoadingTime()
        {
            try
            {
                string strSql = "select * from v_v_tblAttendanceRecord where Date='" + DateTime.Now.ToString("dd-MM-yyyy") + "' AND CompanyId='" + ViewState["__CompanyId__"].ToString() + "' AND AttManual is not null";
                DataTable DTGrid = new DataTable();
                sqlDB.fillDataTable(strSql, DTGrid);
                gvAttendance.DataSource = DTGrid;
                gvAttendance.DataBind();
            }
            catch (Exception ex)
            {
                //throw ex;
            }

        }

        void LoadAttendanceByDivisionAndShift()
        {
            try
            {
                string strSql = "select * from v_v_tblAttendanceRecord where Date='" + DateTime.Now.ToString("dd-MM-yyyy") + "' AND SftId="+5+" AND AttManual is not null";
                DataTable DTGrid = new DataTable();
                sqlDB.fillDataTable(strSql, DTGrid);
                gvAttendance.DataSource = DTGrid;
                gvAttendance.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Delete(int id)
        {
            string strSql = "delete from [dbo].[tblAttendance] where [AttId]='" + id + "'";

            cmd = new SqlCommand(strSQL,sqlDB.connection);

            hdAttendance.Value = "";
            btnSave.Text = "Save";

        }

        private void SetValueToControl(string hid)
        {
            try
            {
                string strSQL = @"select AttId, MonthId, EmpCardNo, EmpName, AttenStatus,
                                    DateIn, DateOut, TimeInHr, TimeInMin, TimeOutForLunchHr,
                                    TimeOutForLunchMin, TimeInAfterLunchHr, TimeInAfterLunchMin,
                                    TimeOutHr, TimeOutMin  from dbo.tblAttendance where AttId='" + hid + "'";
                DataTable DTLocal = new DataTable();
               

                hdAttendance.Value = DTLocal.Rows[0]["MonthID"].ToString();


                txtEmpCardNo.Text = DTLocal.Rows[0]["EmpCardNo"].ToString();
               
                //ddlMonthID.SelectedValue = DTLocal.Rows[0]["MonthId"].ToString();
                ddlAttendanceTemplate.SelectedValue = DTLocal.Rows[0]["AttenStatus"].ToString();
                txtFromDate.Text = Convert.ToDateTime(DTLocal.Rows[0]["DateIn"]).ToString("dd-MMM-yyyy");
                txtToDate.Text = Convert.ToDateTime(DTLocal.Rows[0]["DateOut"]).ToString("dd-MMM-yyyy");
                txtInHur.Text = DTLocal.Rows[0]["TimeInHr"].ToString();
                txtInMin.Text = DTLocal.Rows[0]["TimeInMin"].ToString();
                txtOutForLunchHr.Text = DTLocal.Rows[0]["TimeOutForLunchHr"].ToString();
                txtOutForLunchMin.Text = DTLocal.Rows[0]["TimeOutForLunchMin"].ToString();
                txtInAfterLunchHr.Text = DTLocal.Rows[0]["TimeInAfterLunchHr"].ToString();
                txtInAfterLunchMin.Text = DTLocal.Rows[0]["TimeInAfterLunchMin"].ToString();
                txtOutHur.Text = DTLocal.Rows[0]["TimeOutHr"].ToString();
                txtOutMin.Text = DTLocal.Rows[0]["TimeOutMin"].ToString();
                btnSave.Text = "Update";
                if (ViewState["__UpdateAction__"].Equals("0"))
                {
                    btnSave.Enabled = false;
                    btnSave.CssClass = "";
                }
                else
                {
                    btnSave.Enabled = true;
                    btnSave.CssClass = "Mbutton";
                }
                //Convert.ToDateTime(DTLocal.Rows[0]["HDate"]).ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //DTLocal = null;
            }
        }
        protected void gvAttendance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    string hid = e.CommandArgument.ToString();
                    SetValueToControl(hid);

                }
                else if (e.CommandName == "Delete")
                {
                    int id = Convert.ToInt16(e.CommandArgument.ToString());
                    Delete(id);
                   
                    Clear();
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = ex.ToString();
            }
        }

       
        private void ChangeLeaveStatusByeEmpIdAndDate(string EmpId,string AttDate)
        {
            try
            {
               
                sqlDB.fillDataTable("select LACode from Leave_LeaveApplicationDetails where LeaveDate='" + AttDate + "' AND EmpId='" + EmpId + "'", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    SqlCommand cmd = new SqlCommand("Update Leave_LeaveApplicationDetails set Used='0' where EmpId='" + EmpId + "' AND LeaveDate='" + AttDate + "'", sqlDB.connection);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("Update Leave_LeaveApplication set IsProcessessed='0' where LACode=" + dt.Rows[0]["LACode"].ToString() + " AND EmpId='" + EmpId + "' AND ToDate='" + AttDate + "'", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                }
                
            }
            catch { }
        }


        private void FindOverTimeAcceptalbeMinutes()
        {
            try
            {
                sqlDB.fillDataTable("select AcceptableMinuteasOT from HRD_OthersSetting ", dt = new DataTable());
                ViewState["__OTM__"] = dt.Rows[0]["AcceptableMinuteasOT"].ToString();
            }
            catch { }
        }
              
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                  
                    // for checking essential validation ----------------------------
                    if (!InputValidationBasket()) return;
                    string[] DayStatus = new string[9];
                    string InHur = "00"; string OutHur = "00";
                    DateTime AttDate = DateTime.ParseExact(txtFromDate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    // to get needed employee information for count employee attendance 
                    string [] Get_Needed_EmployeeInfo = classes.mManually_Attendance_Count.Get_Needed_EmployeeeInfo(ddlCompanyList.SelectedValue, txtEmpCardNo.Text.Trim());
                    DateTime joindate = DateTime.ParseExact(Get_Needed_EmployeeInfo[7], "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    if (joindate > AttDate)
                    {
                        lblMessage.InnerText = "error->Attendace date must be largest or equal to Joining Date (" + Get_Needed_EmployeeInfo[7] + ") ";
                        return;
                    }
                    string[] Shift_Roster_InfoList = new string[9];
                    DataTable dtOtherSettings = mZK_Shrink_Data_SqlServer.LoadOTherSettings(ddlCompanyList.SelectedValue);
                    string[] othersetting = new string[8];
                    if (dtOtherSettings.Rows.Count > 0)
                    {
                        othersetting[0] = dtOtherSettings.Rows[0]["WorkerTiffinHour"].ToString();
                        othersetting[1] = dtOtherSettings.Rows[0]["WorkerTiffinMin"].ToString();
                        othersetting[2] = dtOtherSettings.Rows[0]["StaffTiffinHour"].ToString();
                        othersetting[3] = dtOtherSettings.Rows[0]["StaffTiffinMin"].ToString();
                        othersetting[4] = dtOtherSettings.Rows[0]["StaffHolidayCount"].ToString();
                        othersetting[5] = dtOtherSettings.Rows[0]["MinWorkingHour"].ToString() + ":" + dtOtherSettings.Rows[0]["MinWorkingMin"].ToString() + ":00"; //Minimum Working Hours
                        othersetting[6] = dtOtherSettings.Rows[0]["StaffHolidayTotalHour"].ToString() + ":" + dtOtherSettings.Rows[0]["StaffHolidayTotalMin"].ToString() + ":00"; //Minimum Staff Working Hours For Holiday Allowance
                        othersetting[7] = dtOtherSettings.Rows[0]["MinOverTimeHour"].ToString() + ":" + dtOtherSettings.Rows[0]["MinOverTimeMin"].ToString() + ":00"; //Minimum OverTimeHour
                    }
                    string worker = othersetting[0] + ":" + othersetting[1] + ":00";
                    string staff = othersetting[2] + ":" + othersetting[3] + ":00";
                    string tiffin = Get_Needed_EmployeeInfo[6] == "1" ? worker : staff;
                    string holidaycount = "0";
                    string IsHalfDayLeave = "0";
                    
                    if (Get_Needed_EmployeeInfo[4].Equals("Roster"))  // if employee duty type is roster then execute if block
                    {
                        // to get all roster information 
                        Shift_Roster_InfoList = classes.mCommon_Module_For_AttendanceProcessing.LoadShift_Information(Get_Needed_EmployeeInfo[0].ToString(), false, "", ViewState["__AttDates__"].ToString());
                       
                        //-- for leave information and leave count-----------------------------
                        string[] Leave_Info = classes.mZK_Shrink_Data_SqlServer.Check_Any_Leave_Are_Exist(AttDate.ToString("yyyy-MM-dd"), Get_Needed_EmployeeInfo[0]);
                        if (Leave_Info[0].ToString() != "0")  // check any type of leave. if are leave exists then execute this if block
                        {
                            DayStatus[0] = "Lv";
                            DayStatus[1] = Leave_Info[1];
                            DayStatus[2] = "00:00:00";
                            DayStatus[3] = "00:00:00";
                            DayStatus[4] = "00:00:00";
                            DayStatus[6] = "0";
                            DayStatus[7] = "00:00:00";
                            DayStatus[8] = "00:00:00";
                            classes.LeaveLibrary.LeaveCount(AttDate.ToString("yyyy-MM-dd"), Leave_Info[0]);
                            classes.
                           mManually_Attendance_Count.
                           SaveAttendance_Status(Get_Needed_EmployeeInfo[0], AttDate.ToString("dd-MM-yyyy"), Get_Needed_EmployeeInfo[6],
                                                 (InHur.Length == 1) ? "0" + InHur : InHur,
                                                 (txtInMin.Text.Trim().Length == 1) ? "0" + txtInMin.Text.Trim() : txtInMin.Text.Trim(), "00",
                                                 (OutHur.Length == 1) ? "0" + OutHur : OutHur,
                                                 (txtOutMin.Text.Trim().Length == 1) ? "0" + txtOutMin.Text.Trim() : txtOutMin.Text.Trim(), "00",
                                                 DayStatus[0], DayStatus[1], DayStatus[2], Shift_Roster_InfoList[0], Get_Needed_EmployeeInfo[1],
                                                 Get_Needed_EmployeeInfo[2], ddlCompanyList.SelectedValue, Get_Needed_EmployeeInfo[3],
                                                 DayStatus[3], DayStatus[4], Shift_Roster_InfoList[3] + ":" + Shift_Roster_InfoList[6] + ":" + Shift_Roster_InfoList[4], "MC", "0", "0", DayStatus[6], DayStatus[7], DayStatus[8], (ckbOutPunch.Checked) ? "1" : (ckbTraining.Checked) ? "2" : "0", txtReferencId.Text.Trim(), ViewState["__getUserId__"].ToString(), txtRemark.Text.Trim(), "0", DayStatus[9]);
                            lblMessage.InnerText = "success-> Successfully Manualy Attendance Counted";
                            DataTable dt = classes.mCommon_Module_For_AttendanceProcessing.Load_Process_AttendanceData(ddlCompanyList.SelectedValue, "0", AttDate.ToString("yyyy-MM-dd"), false, txtEmpCardNo.Text);
                            gvAttendance.DataSource = dt;
                            gvAttendance.DataBind();
                        }
                        else {
                        //-------------------------End-----------------------------------------------
                        //---------------------------------------------------------------------------
                        // to checking selected date is weekend or holiday ?
                        DayStatus = classes.mZK_Shrink_Data_SqlServer.Check_Todays_Is_HolidayOrWeekend(AttDate.ToString("yyyy-MM-dd"));
                        if (bool.Parse(DayStatus[0]).Equals(true)) // if selected date is weekend or holiday then execute thi block
                        {
                           
                            
                            if (!int.Parse(txtInHur.Text.Trim()).Equals(0))
                            {
                                InHur = (ddlInTimeAMPM.SelectedValue.Equals("AM")) ? txtInHur.Text : (int.Parse(txtInHur.Text).Equals(12)) ? txtInHur.Text : (int.Parse(txtInHur.Text) + 12).ToString();
                                OutHur = (ddlOutTimeAMPM.SelectedValue.Equals("AM")) ? txtOutHur.Text : (int.Parse(txtOutHur.Text).Equals(12) || int.Parse(txtOutHur.Text).Equals(0)) ? txtOutHur.Text : (int.Parse(txtOutHur.Text) + 12).ToString(); 
                                DayStatus = classes.mManually_Attendance_Count.OverTime_Calculation_ForWeekend_Holiday(TimeSpan.Parse(InHur + ":" + txtInMin.Text + ":00"), TimeSpan.Parse(OutHur + ":" + txtOutMin.Text + ":00"), int.Parse(Shift_Roster_InfoList[5]), TimeSpan.Parse(Shift_Roster_InfoList[3]), TimeSpan.Parse(tiffin), (DayStatus[1].Equals("W")) ? "W" : "H", (DayStatus[1].Equals("W")) ? "Weekend" : "Holiday",TimeSpan.Parse(othersetting[7]));
                                if (Get_Needed_EmployeeInfo[6] == "2" && TimeSpan.Parse(DayStatus[4]) >= TimeSpan.Parse(othersetting[6]))
                                {
                                    holidaycount = "1";
                                }
                            }
                            else
                            {
                                string wh = DayStatus[1];
                                DayStatus = new string[7];
                                DayStatus[0] = (wh.Equals("W")) ? "W" : "H";
                                DayStatus[1] = (wh.Equals("W")) ? "Weekend" : "Holiday";
                                DayStatus[2] = "00:00:00";
                                DayStatus[3] = "00:00:00";
                                DayStatus[4] = "00:00:00";
                                DayStatus[5] = "0";
                                DayStatus[6] = "0";
                                DayStatus[7] = "00:00:00";
                                DayStatus[8] = "00:00:00";
                            }
                                

                           classes.
                           mManually_Attendance_Count.
                           SaveAttendance_Status(Get_Needed_EmployeeInfo[0], AttDate.ToString("dd-MM-yyyy"), Get_Needed_EmployeeInfo[6],
                                                 (InHur.Length == 1) ? "0" + InHur : InHur,
                                                 (txtInMin.Text.Trim().Length == 1) ? "0" + txtInMin.Text.Trim() : txtInMin.Text.Trim(), "00",
                                                 (OutHur.Length == 1) ? "0" + OutHur : OutHur,
                                                 (txtOutMin.Text.Trim().Length == 1) ? "0" + txtOutMin.Text.Trim() : txtOutMin.Text.Trim(), "00",
                                                 DayStatus[0], DayStatus[1], DayStatus[2], Shift_Roster_InfoList[0], Get_Needed_EmployeeInfo[1],
                                                 Get_Needed_EmployeeInfo[2], ddlCompanyList.SelectedValue, Get_Needed_EmployeeInfo[3],
                                                 DayStatus[3], DayStatus[4], Shift_Roster_InfoList[3] + ":" + Shift_Roster_InfoList[6] + ":" + Shift_Roster_InfoList[4], "MC", DayStatus[5], holidaycount, "0", DayStatus[7], DayStatus[8], (ckbOutPunch.Checked) ? "1" : (ckbTraining.Checked) ? "2" : "0", txtReferencId.Text.Trim(), ViewState["__getUserId__"].ToString(), txtRemark.Text.Trim(), "0", DayStatus[9]);
                           lblMessage.InnerText = "success-> Successfully Manualy Attendance Counted";
                           DataTable dt = classes.mCommon_Module_For_AttendanceProcessing.Load_Process_AttendanceData(ddlCompanyList.SelectedValue, "0", AttDate.ToString("yyyy-MM-dd"), false, txtEmpCardNo.Text);
                           gvAttendance.DataSource = dt;
                           gvAttendance.DataBind();
                        }
                         //---------------------------------------------------------------------------------------
                        else
                        {
                            InHur = (ddlInTimeAMPM.SelectedValue.Equals("AM")) ? txtInHur.Text : (int.Parse(txtInHur.Text).Equals(12)) ? txtInHur.Text : (int.Parse(txtInHur.Text) + 12).ToString();
                            OutHur = (ddlOutTimeAMPM.SelectedValue.Equals("AM")) ? txtOutHur.Text : (int.Parse(txtOutHur.Text).Equals(12) || int.Parse(txtOutHur.Text).Equals(0)) ? txtOutHur.Text : (int.Parse(txtOutHur.Text) + 12).ToString(); 
                            string[] Roster_Operation_Status = classes.mManually_Attendance_Count.Roster_Operation_TimeChecking(TimeSpan.Parse(Shift_Roster_InfoList[7]), TimeSpan.Parse(Shift_Roster_InfoList[4]), TimeSpan.Parse(InHur + ":" + txtInMin.Text.Trim() + ":00"));

                            if (!bool.Parse(Roster_Operation_Status[0]))
                            {
                                lblMessage.InnerText = "error->" + Roster_Operation_Status[1];
                                return;
                            }

                            DayStatus = classes.mManually_Attendance_Count.getTotalOverTime(TimeSpan.Parse(InHur + ":" + txtInMin.Text + ":00"), TimeSpan.Parse(OutHur + ":" + txtOutMin.Text + ":00"), TimeSpan.Parse(Shift_Roster_InfoList[3]), TimeSpan.Parse(Shift_Roster_InfoList[4]), Shift_Roster_InfoList[5], Shift_Roster_InfoList[6], false, TimeSpan.Parse(tiffin), TimeSpan.Parse(othersetting[5]),TimeSpan.Parse(othersetting[7]), Shift_Roster_InfoList[9], Shift_Roster_InfoList[10]);

                            classes.
                           mManually_Attendance_Count.
                           SaveAttendance_Status(Get_Needed_EmployeeInfo[0], AttDate.ToString("dd-MM-yyyy"), Get_Needed_EmployeeInfo[6],
                                                 (InHur.Length == 1) ? "0" + InHur : InHur,
                                                 (txtInMin.Text.Trim().Length == 1) ? "0" + txtInMin.Text.Trim() : txtInMin.Text.Trim(), "00",
                                                 (OutHur.Length == 1) ? "0" + OutHur : OutHur,
                                                 (txtOutMin.Text.Trim().Length == 1) ? "0" + txtOutMin.Text.Trim() : txtOutMin.Text.Trim(), "00",
                                                 DayStatus[0], DayStatus[1], DayStatus[2], Shift_Roster_InfoList[0], Get_Needed_EmployeeInfo[1],
                                                 Get_Needed_EmployeeInfo[2], ddlCompanyList.SelectedValue, Get_Needed_EmployeeInfo[3],
                                                 DayStatus[3], DayStatus[4], Shift_Roster_InfoList[3] + ":" + Shift_Roster_InfoList[6] + ":" + Shift_Roster_InfoList[4], "MC", DayStatus[5], holidaycount, DayStatus[6], DayStatus[7], DayStatus[8], (ckbOutPunch.Checked) ? "1" : (ckbTraining.Checked) ? "2" : "0", txtReferencId.Text.Trim(), ViewState["__getUserId__"].ToString(), txtRemark.Text.Trim(),"0", DayStatus[9]);
                            lblMessage.InnerText = "success-> Successfully Manualy Attendance Counted";
                            DataTable dt = classes.mCommon_Module_For_AttendanceProcessing.Load_Process_AttendanceData(ddlCompanyList.SelectedValue, "0", AttDate.ToString("yyyy-MM-dd"), false, txtEmpCardNo.Text);
                            gvAttendance.DataSource = dt;
                            gvAttendance.DataBind();
                        }
                        }


                    }
                    else // if employee duty type is regular then execute this bolock
                    {

                        // to get all roster information 
                        Shift_Roster_InfoList = classes.mCommon_Module_For_AttendanceProcessing.LoadShift_Information(Get_Needed_EmployeeInfo[0], true, Get_Needed_EmployeeInfo[5], AttDate.ToString("yyyy-MM-dd"));
                        
                        //-- for leave information and leave count-----------------------------
                        string[] Leave_Info = classes.mZK_Shrink_Data_SqlServer.Check_Any_Leave_Are_Exist(AttDate.ToString("yyyy-MM-dd"), Get_Needed_EmployeeInfo[0]);
                        if (Leave_Info[0].ToString() != "0" && Leave_Info[2].ToString().Equals("False"))  // check any type of leave. if are leave exists then execute this if block
                        {
                            DayStatus[0] = "Lv";
                            DayStatus[1] = Leave_Info[1];
                            DayStatus[2] = "00:00:00";
                            DayStatus[3] = "00:00:00";
                            DayStatus[4] = "00:00:00";
                            DayStatus[5] = "0";
                            DayStatus[6] = "0";
                            DayStatus[7] = "00:00:00";
                            DayStatus[8] = "00:00:00";
                            DayStatus[9] = "0";
                            classes.LeaveLibrary.LeaveCount(AttDate.ToString("yyyy-MM-dd"), Leave_Info[0]);
                        }
                        //-------------------------End-----------------------------------------------
                        else { 
                        // to checking selected date is weekend or holiday ?
                        DayStatus = classes.mZK_Shrink_Data_SqlServer.Check_Todays_Is_HolidayOrWeekend(AttDate.ToString("yyyy-MM-dd"));
                        if (bool.Parse(DayStatus[0]).Equals(true)) // if selected date is weekend or holiday then execute thi block
                        {
                           
                            if (!int.Parse(txtInHur.Text.Trim()).Equals(0))
                            {
                                InHur = (ddlInTimeAMPM.SelectedValue.Equals("AM")) ? txtInHur.Text : (int.Parse(txtInHur.Text).Equals(12)) ? txtInHur.Text : (int.Parse(txtInHur.Text) + 12).ToString();
                                OutHur = (ddlOutTimeAMPM.SelectedValue.Equals("AM")) ? txtOutHur.Text : (int.Parse(txtOutHur.Text).Equals(12) || int.Parse(txtOutHur.Text).Equals(0)) ? txtOutHur.Text : (int.Parse(txtOutHur.Text) + 12).ToString(); 
                                DayStatus = classes.mManually_Attendance_Count.OverTime_Calculation_ForWeekend_Holiday(TimeSpan.Parse(InHur + ":" + txtInMin.Text + ":00"), TimeSpan.Parse(OutHur + ":" + txtOutMin.Text + ":00"), int.Parse(Shift_Roster_InfoList[5]), TimeSpan.Parse(Shift_Roster_InfoList[3]), TimeSpan.Parse(tiffin), (DayStatus[1].Equals("W")) ? "W" : "H", (DayStatus[1].Equals("W")) ? "Weekend" : "Holiday",TimeSpan.Parse(othersetting[7]));
                                if (Get_Needed_EmployeeInfo[6] == "2" && TimeSpan.Parse(DayStatus[4]) >= TimeSpan.Parse(othersetting[6]))
                                {
                                    holidaycount = "1";
                                }
                            }
                            else
                            {
                                string wh = DayStatus[1];
                                DayStatus = new string[10];
                                DayStatus[0] = (wh.Equals("W")) ? "W" : "H";
                                DayStatus[1] = (wh.Equals("W")) ? "Weekend" : "Holiday";
                                DayStatus[2] = "00:00:00";
                                DayStatus[3] = "00:00:00";
                                DayStatus[4] = "00:00:00";
                                DayStatus[5] = "0";
                                DayStatus[6] = "0";
                                DayStatus[7] = "00:00:00";
                                DayStatus[8] = "00:00:00";
                                DayStatus[9] = "0";
                                }                          
                        }
                        else
                        {
                            InHur = (ddlInTimeAMPM.SelectedValue.Equals("AM")) ? txtInHur.Text : (int.Parse(txtInHur.Text).Equals(12)) ? txtInHur.Text : (int.Parse(txtInHur.Text) + 12).ToString();
                            OutHur = (ddlOutTimeAMPM.SelectedValue.Equals("AM")) ? txtOutHur.Text : (int.Parse(txtOutHur.Text).Equals(12) || int.Parse(txtOutHur.Text).Equals(0)) ? txtOutHur.Text : (int.Parse(txtOutHur.Text) + 12).ToString(); 
                   
                             DayStatus = classes.mManually_Attendance_Count.getTotalOverTime(TimeSpan.Parse(InHur + ":" + txtInMin.Text + ":00"), TimeSpan.Parse(OutHur + ":" + txtOutMin.Text + ":00"), TimeSpan.Parse(Shift_Roster_InfoList[3]), TimeSpan.Parse(Shift_Roster_InfoList[4]), Shift_Roster_InfoList[5], Shift_Roster_InfoList[6], false, TimeSpan.Parse(tiffin), TimeSpan.Parse(othersetting[5]),TimeSpan.Parse(othersetting[7]), Shift_Roster_InfoList[9], Shift_Roster_InfoList[10]);
                                if (Leave_Info[0].ToString() != "0")
                                {
                                    DayStatus[0] = "P";
                                    DayStatus[1] = "Present";
                                    DayStatus[6] = "1";
                                    IsHalfDayLeave = "1";
                                }
                        }
                        }
                        classes.
                            mManually_Attendance_Count.
                        SaveAttendance_Status(Get_Needed_EmployeeInfo[0], AttDate.ToString("dd-MM-yyyy"), Get_Needed_EmployeeInfo[6],
                                                  (InHur.Length == 1) ? "0" + InHur : InHur,
                                                  (txtInMin.Text.Trim().Length == 1) ? "0" + txtInMin.Text.Trim() : txtInMin.Text.Trim(), "00",
                                                  (OutHur.Length == 1) ? "0" + OutHur : OutHur,
                                                  (txtOutMin.Text.Trim().Length == 1) ? "0" + txtOutMin.Text.Trim() : txtOutMin.Text.Trim(), "00",
                                                  DayStatus[0], DayStatus[1], DayStatus[2], Shift_Roster_InfoList[0], Get_Needed_EmployeeInfo[1],
                                                  Get_Needed_EmployeeInfo[2], ddlCompanyList.SelectedValue, Get_Needed_EmployeeInfo[3],
                                                  DayStatus[3], DayStatus[4], Shift_Roster_InfoList[3] + ":" + Shift_Roster_InfoList[6] + ":" + Shift_Roster_InfoList[4], "MC", DayStatus[5], holidaycount, DayStatus[6], DayStatus[7], DayStatus[8], (ckbOutPunch.Checked) ? "1" : (ckbTraining.Checked) ? "2" : "0", txtReferencId.Text.Trim(), ViewState["__getUserId__"].ToString(), txtRemark.Text.Trim(), IsHalfDayLeave, DayStatus[9]);
                        lblMessage.InnerText = "success-> Successfully Manualy Attendance Counted";
                       DataTable dt= classes.mCommon_Module_For_AttendanceProcessing.Load_Process_AttendanceData(ddlCompanyList.SelectedValue,"0", AttDate.ToString("yyyy-MM-dd"), false, txtEmpCardNo.Text);
                        gvAttendance.DataSource = dt;
                        gvAttendance.DataBind();
                    }
                        

                   

                }
                catch { }
            }
            catch { }
        }

        

        protected void rblAttendanceCountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                if (rblAttendanceCountType.SelectedItem.ToString().Equals("Single"))
                {
                    trToDate.Visible = false;
                    tdFromDate.InnerText = "Date";
                }
                else
                {
                    trToDate.Visible = true;
                    tdFromDate.InnerText = "From Date";


                }
            }
            catch { }
        }

        private bool CompareJoiningDateAndLogindate(byte i, string AttDate)
        {
            try
            {
                DateTime InDate = new DateTime(int.Parse(AttDate.Trim().Substring(6, 4)), int.Parse(AttDate.Trim().Substring(3, 2)), int.Parse(AttDate.Trim().Substring(0, 2)));
                DateTime AdmissionDate = new DateTime(int.Parse(dtEmpInfo.Rows[i]["EmpJoiningDate"].ToString().Substring(6, 4)), int.Parse(dtEmpInfo.Rows[i]["EmpJoiningDate"].ToString().Substring(3, 2)), int.Parse(dtEmpInfo.Rows[i]["EmpJoiningDate"].ToString().Substring(0, 2)));

                if (InDate >= AdmissionDate) return true;
                else
                {
                    lblMessage.InnerText = "error->This employee is not join on this date.The join date is " + dtEmpInfo.Rows[i]["EmpJoiningDate"].ToString();
                    return false;
                }
            }
            catch { return false; }
        }

        

       
        protected void gvAttendance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                LoadAttendanceByDivisionAndShift();
                gvAttendance.PageIndex = e.NewPageIndex;
                gvAttendance.DataBind();
            }
            catch { }
        }


        private bool InputValidationBasket()
        {
            try
            {

            
                if (txtEmpCardNo.Text.Trim().Length <4)
                {
                    lblMessage.InnerText = "error->Please type the emp cardno";
                    txtEmpCardNo.Focus();
                    return false;
                }
                else if (ddlAttendanceTemplate.SelectedValue.ToString().Equals("s"))
                {
                    lblMessage.InnerText = "error->Please select attendance sttus";
                    ddlAttendanceTemplate.Focus();
                    return false;
                }

                else if (rblAttendanceCountType.SelectedValue.ToString().Equals("Single") && txtFromDate.Text.Trim().Length < 10)
                {
                    lblMessage.InnerText = "error->Please select attendance date";
                    txtFromDate.Focus();
                    return false;
                }
                else if (!rblAttendanceCountType.SelectedValue.ToString().Equals("Single") && txtFromDate.Text.Trim().Length < 10)
                {
                   
                    lblMessage.InnerText = "error->Please select attendance from date";
                    txtFromDate.Focus();
                    return false;
                }

                else if (!rblAttendanceCountType.SelectedValue.ToString().Equals("Single") && txtToDate.Text.Trim().Length < 10)
                {
                    lblMessage.InnerText = "error->Please select attendance to date";
                    txtToDate.Focus();
                    return false;
                }
                
                else if ( ddlAttendanceTemplate.SelectedValue=="AC" && int.Parse(txtInHur.Text.Trim()) == 0 &&!ckbOutPunch.Checked &&!ckbTraining.Checked)
                {
                    lblMessage.InnerText = "error->Please type valid shift start time";
                    txtInHur.Focus();
                    return false;
                }

                //else if (ckbOutPunch.Checked && txtReferencId.Text.Trim().Equals(""))
                //{
                //    lblMessage.InnerText = "error->Please type Reference ID";
                //    txtReferencId.Focus();
                //    return false;
                //}

                //else if (ckbOutPunch.Checked && (txtOutHur.Text.Trim().Equals("") || txtOutHur.Text.Trim().Equals("00") || txtOutHur.Text.Trim().Equals("0")))
                //{
                //    lblMessage.InnerText = "error->Please type out time";
                //    txtOutHur.Focus();
                //    return false;
                //}


                else if (ddlAttendanceTemplate.SelectedValue.ToString().Equals("m/l"))
                {
                    sqlDB.fillDataTable("select Sex from Personnel_EmpPersonnal where EmpId=(select EmpId from Personnel_EmpCurrentStatus where EmpCardNo Like '%"+txtEmpCardNo.Text.Trim()+"')", dt = new DataTable());

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Sex"].ToString().Equals("Male") && ddlAttendanceTemplate.SelectedValue.ToString().Equals("m/l"))
                        {
                            lblMessage.InnerText = "error->Sorry this employee is male";
                            return false;
                        }
                    }

                    else
                    {
                        lblMessage.InnerText = "error->Please set sex of this employee";
                        return false;
                    }
                        
                }

                return true;
            }
            catch { return false; }
        }

        
        
        protected void btnFindEmpInfo_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtEmpCardNo.Text.Trim().Length<4) 
                {
                    lblMessage.InnerText = "warning-> Please type valid Card No !";
                    txtEmpCardNo.Focus();
                    return;
                }
                if (txtFromDate.Text.Trim().Length < 8)
                {
                    lblMessage.InnerText = "warning-> Please type valid Date !";
                    txtFromDate.Focus();
                    return;
                }
                   
                string[] EmployeeInfos = classes.mManually_Attendance_Count.Find_IsRunningEmployee(ddlCompanyList.SelectedValue.ToString(), txtEmpCardNo.Text.Trim());
                if (EmployeeInfos == null) lblMessage.InnerText = "error->Please type valid employee card no";
                else
                {
                    divFindInfo.Style.Add("Color", "Green");
                    divFindInfo.InnerText = EmployeeInfos[0];
                    string[] Date = txtFromDate.Text.Split('-');
                    ViewState["__AttDates__"] = Date[2] + "-" + Date[1] + "-" + Date[0];
                    sqlDB.fillDataTable("select  InHour,InMin,OutHour,OutMin,AttStatus,OutDuty,ReferenceID,Remark from v_tblAttendanceRecord  where attdate='" + ViewState["__AttDates__"].ToString() + "' AND EmpCardNo like'%" + txtEmpCardNo.Text.Trim() + "'", dt = new DataTable());
                    if (dt.Rows.Count > 0)
                    {
                        //if (dt.Rows[0]["ReferenceID"].ToString().Equals(""))
                        //{
                        //    ckbOutPunch.Checked = false;
                        //    trReferencId.Visible = false;
                        //}
                        //else
                        //{
                        //    ckbOutPunch.Checked = true;
                        //    trReferencId.Visible = true;
                        //}
                        ckbOutPunch.Checked = bool.Parse(dt.Rows[0]["OutDuty"].ToString());
                        txtReferencId.Text = dt.Rows[0]["ReferenceID"].ToString();

                        int InHur = int.Parse(dt.Rows[0]["InHour"].ToString());
                        int OutHur = int.Parse(dt.Rows[0]["OutHour"].ToString());
                        if (InHur > 12)
                        {
                            txtInHur.Text = (InHur - 12).ToString();
                            ddlInTimeAMPM.SelectedValue = "PM";
                        }
                        else
                        {
                            txtInHur.Text = InHur.ToString();
                            ddlInTimeAMPM.SelectedValue = "AM";
                        }
                        if (OutHur > 12)
                        {
                            txtOutHur.Text = (OutHur - 12).ToString();
                            ddlOutTimeAMPM.SelectedValue = "PM";
                        }
                        else
                        {
                            txtOutHur.Text = OutHur.ToString();
                            ddlOutTimeAMPM.SelectedValue = "AM";
                        }
                        txtInMin.Text = dt.Rows[0]["InMin"].ToString();
                        txtOutMin.Text = dt.Rows[0]["OutMin"].ToString();
                        if (dt.Rows[0]["AttStatus"].ToString().Equals("P") || dt.Rows[0]["AttStatus"].ToString().Equals("L") || dt.Rows[0]["AttStatus"].ToString().Equals("A"))
                            ddlAttendanceTemplate.SelectedValue = "AC";
                        else if (dt.Rows[0]["AttStatus"].ToString().Equals("W") || dt.Rows[0]["AttStatus"].ToString().Equals("H"))
                            ddlAttendanceTemplate.SelectedValue = "WH";
                        else
                            ddlAttendanceTemplate.SelectedValue = "Lv";
                        txtRemark.Text = dt.Rows[0]["Remark"].ToString();
                    }
                    else 
                    {
                        ckbOutPunch.Checked = false;
                        trReferencId.Visible = false;
                        txtReferencId.Text = "";
                        txtInHur.Text = "00";   txtInMin.Text = "00";
                        txtOutHur.Text = "00";  txtOutMin.Text = "00";
                        ddlInTimeAMPM.SelectedValue = "AM";
                        ddlOutTimeAMPM.SelectedValue = "PM";
                        txtRemark.Text = "";
                    }
                }
            }
            catch { }            
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void gvAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                    e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
                }
            }
            catch { }
        }

        protected void btnComplain_Click(object sender, EventArgs e)
        {
            Session["__ModuleType__"] = "Attendance";
            Session["__forCompose__"] = "No";
            Session["__PreviousPage__"] = Request.ServerVariables["HTTP_REFERER"].ToString();
            Response.Redirect("/mail/complain.aspx");
        }

        protected void ckbOutPunch_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbOutPunch.Checked)
            {
                ckbTraining.Checked = false;
                //trReferencId.Visible = true;
                //txtReferencId.Focus();
            } 
              
              
              
}

        protected void ckbTraining_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbTraining.Checked)         
                ckbOutPunch.Checked = false;
            
              
        }

       
    }
}