using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

using System.Data.OleDb;
using adviitRuntimeScripting;

using ComplexScriptingSystem;
using System.IO;
using System.Drawing;

namespace SigmaERP.attendance
{
    public partial class import_data : System.Web.UI.Page
    {
       
        DataTable DTHoliday;

        SqlDataAdapter da;
        SqlCommand cmd;
        int k;

        bool ImportProcess = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            ProgressBar1.Minimum = 0;
            ProgressBar1.Maximum = 5000;
            ProgressBar1.BackColor = System.Drawing.Color.Blue;
            ProgressBar1.ForeColor = Color.White;
            ProgressBar1.Height = new Unit(20);
            ProgressBar1.Width = new Unit(500);
            lblErrorMessage.Text = "";
            if (!IsPostBack)
            {
              
                //txtFullAttDate.Text = "2014-01-01";
                //this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
                ViewState["__OT__"] = "0";
                setPrivilege();              
            }
            if (!classes.commonTask.HasBranch())
                ddlCompanyList.Enabled = false;
        }

        private void setPrivilege()
        {
            try
            {
                
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();

               if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='import_data.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                        {
                            btnImport.CssClass = "";
                            btnImport.Enabled = false;
                            return;
                        }
                        else
                        {
                            btnImport.CssClass = "Mbutton";
                            btnImport.Enabled = true;
                         
                        }
                    }
                    else 
                    {
                        btnImport.CssClass = "";
                        btnImport.Enabled = false;
                        return;
                    }
                }
               classes.commonTask.LoadBranch(ddlCompanyList);
               ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
               classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ddlCompanyList.SelectedValue);              
            }
            catch { }
        }

        DataTable dtWeekendInfo = new DataTable();
        private void loadWeekendInfo(string MonthYear)
        {
            try
            {

                sqlDB.fillDataTable("select convert(varchar(11),WeekendDate,111) as WeekendDate from Attendance_WeekendInfo where MonthName='" + MonthYear + "'", dtWeekendInfo);
                
            }
            catch { }

        }
        
        void LoadGrid()
        {
            try
            {

                
                string strSql ="";
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedItem.Value.ToString();
                if (ViewState["__ImportType__"].ToString().Equals("FullImport"))

                    if (ddlShiftName.SelectedItem.Text=="All")strSql = "select ar.EmpCardNo,ar.EmpName,FORMAT(ATTDate,'dd-MM-yyyy') as Date,InHour+':'+ InMin As Time,ar.ATTStatus,ar.StateStatus  from v_tblAttendanceRecord ar where ar.AttDate>='" + ViewState["__FromAttDate__"].ToString() + "' AND ar.AttDate<='" + ViewState["__AttDate__"].ToString() + "' AND ar.CompanyId='" + CompanyId + "' AND ar.DptId='"+ddlDepartmentList.SelectedValue.ToString()+"' ";
                    else strSql = "select ar.EmpCardNo,ar.EmpName,FORMAT(ATTDate,'dd-MM-yyyy') as Date,InHour+':'+ InMin As Time,ar.ATTStatus,ar.StateStatus  from v_tblAttendanceRecord ar where ar.AttDate>='" + ViewState["__FromAttDate__"].ToString() + "' AND ar.AttDate<='" + ViewState["__AttDate__"].ToString() + "' AND ar.CompanyId='" + CompanyId + "' AND ar.sftId=" + ddlShiftName.SelectedValue.ToString() + " AND ar.DptId='" + ddlDepartmentList.SelectedValue.ToString() + "' ";

                else strSql = "select ar.EmpCardNo,ar.EmpName,FORMAT(ATTDate,'dd-MM-yyyy') as Date,InHour+':'+ InMin As Time,ar.ATTStatus,ar.StateStatus  from v_tblAttendanceRecord ar where ar.AttDate='" + ViewState["__FromAttDate__"].ToString() + "' AND ar.AttDate<='" + ViewState["__AttDate__"].ToString() + "' AND EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' AND ar.CompanyId='" + CompanyId + "'";

                DataTable DTGrid = new DataTable();
                sqlDB.fillDataTable(strSql, DTGrid);
                if (DTGrid.Rows.Count > 0) gvAttendance.DataSource = DTGrid;
                else gvAttendance.DataSource = null;
                gvAttendance.DataBind();

                ProgressBar1.Value = 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
               
        }

        private void deleteExistingRecordByDate_AttManual(bool ForAllEmployee,string ShiftId)
        {
            try
            {
                if (ForAllEmployes) cmd = new System.Data.SqlClient.SqlCommand("delete from tblAttendanceRecord where Format(attDate,'yyyy-MM-dd')='" + ViewState["__AttDate__"].ToString() + "' AND DptId='" + ddlDepartmentList.SelectedItem.Value + "' AND SftId=" + ShiftId + " AND AttManual is null ", sqlDB.connection);
                else cmd = new System.Data.SqlClient.SqlCommand("delete from tblAttendanceRecord where attDate='" + ViewState["__AttDate__"].ToString() + "' AND EmpId='" + dtEmpInfo.Rows[0]["EmpId"].ToString() + "' AND DptId='" + ddlDepartmentList.SelectedItem.Value + "'  AND AttManual is null ", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }
        


        private bool validationBasket()
        {
            try
            {
                if (ddlDepartmentList.SelectedValue=="0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select a department";
                    ddlDepartmentList.Focus();
                    return false;
                }
                if (ddlShiftName.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select a shift";
                    ddlShiftName.Focus();
                    return false;
                }

                if (rblImportType.SelectedValue == "FullImport" && rblDateType.SelectedValue == "SingleDate" && txtFullAttDate .Text.Trim().Length<10)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select attendance date";
                    txtFullAttDate.Focus();
                    return false;
                }

                if (rblImportType.SelectedValue !="FullImport" && txtCardNo.Text.Trim().Length<4)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please type valid card no";
                    txtCardNo.Focus();
                    return false;
                }
                if (rblImportType.SelectedValue != "FullImport" && txtPartialAttDate.Text.Trim().Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select partial attendance date";
                    txtPartialAttDate.Focus();
                    return false;
                }

                if (txtFullToDate.Visible==true && txtFullToDate.Text.Trim().Length<10)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select To date";
                    txtFullToDate.Focus();
                    return false;
                }
                else if (txtPartialToDate.Visible==true && txtPartialToDate.Text.Trim().Length<10)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select To date";
                    txtPartialToDate.Focus();
                    return false;
                }
                if (rblImportType.SelectedValue != "FullImport" && ddlShiftName.SelectedItem.Text=="All")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Sorry you can't select all shift for partial process.";
                    ddlShiftName.Focus();
                    return false;
                }

                return true;
            }
            catch { return false; }
        }

        int i;
        int j = 0;
        DataTable dtEmpInfo;
        string attDate = "";
        DataTable dt;
        bool ForAllEmployes;
        string selectedDate;

        protected void btnImport_Click(object sender, EventArgs e)
        {
            loadAAllShiftInfo();

            lblMessage.InnerText = "";
            ProgressBar1.Value = 0;
            //ViewState["__ShiftList__"] = (ddlShiftName.SelectedItem.Text == "All") ? classes.commonTask.GetAllShiftIdFromDroudownList(ddlShiftName) : ddlShiftName.SelectedItem.Value.ToString();
           // return;
            if (!validationBasket()) return;
            string [] FDate;
            string [] TDate;
            if (rblImportType.SelectedValue.ToString()=="FullImport")
            {
                FDate = txtFullAttDate.Text.Trim().Split('-');
                ViewState["__FromAttDate__"] = FDate[2] + "-" + FDate[1] + "-" + FDate[0];
                TDate = (txtFullToDate.Text.Trim().Length == 0) ? txtFullAttDate.Text.Split('-') : txtFullToDate.Text.Split('-');
                ViewState["__ToDate__"] = (txtFullToDate.Text.Trim().Length == 0) ? txtFullAttDate.Text: txtFullToDate.Text;
            }
            else
            {
                FDate = txtPartialAttDate.Text.Trim().Split('-');
                ViewState["__FromAttDate__"] = FDate[2] + "-" + FDate[1] + "-" + FDate[0];
                TDate = (txtPartialToDate.Text.Trim().Length == 0) ? txtPartialAttDate.Text.Trim().Split('-') : txtPartialToDate.Text.Split('-');
                ViewState["__ToDate__"]=(txtPartialToDate.Text.Trim().Length == 0) ? txtPartialAttDate.Text.Trim(): txtPartialToDate.Text;
            }

           
            DateTime FromDate = new DateTime(int.Parse(FDate[2]),int.Parse(FDate[1]),int.Parse(FDate[0]));
            DateTime ToDate = new DateTime(int.Parse(TDate[2]), int.Parse(TDate[1]), int.Parse(TDate[0]));

            ForAllEmployes = (rblImportType.SelectedValue.ToString().Equals("FullImport")) ? true : false;
            ViewState["__ImportType__"] = (rblImportType.SelectedValue.ToString().Equals("FullImport")) ? "FullImport" :"PartialImport";
            while (FromDate <= ToDate)
            {
                string [] AttDate = FromDate.ToString().Split('/');

                AttDate[0]=(AttDate[0].Trim().Length==1)?"0"+AttDate[0]:AttDate[0]; // month
                AttDate[1]=(AttDate[1].Trim().Length==1)?"0"+AttDate[1]:AttDate[1]; // day
                AttDate[2]=AttDate[2].Substring(0,4); // year

                ViewState["__AttDate__"] = AttDate[2] + "-" + AttDate[0] + "-" + AttDate[1]; // format yyyy-MM-dd






                selectedDate = AttDate[1] + "-" + AttDate[0] + "-" + AttDate[2];   // fromat dd-MM-yyyy
                
                


                // load all Employee EmpId according to shift and division
                dtEmpInfo = new DataTable();

                string ShiftId;
                ViewState["__TotalNoShift__"] = (ddlShiftName.SelectedItem.Text == "All") ? ddlShiftName.Items.Count-1:0;

                if (ddlShiftName.SelectedItem.Text == "All")
                {
                    
                    for (byte b=1; b < ddlShiftName.Items.Count; b++)
                    {
                        dtEmpInfo = new DataTable();
                        if (rblImportType.SelectedValue.ToString().Equals("FullImport")) sqlDB.fillDataTable("select EmpId,convert(int,Right(EmpCardNo,LEN(EmpCardNo)-7)) as EmpCardNo,EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate,SftId,IsWeekend,GId from v_ShiftTransferInfoDetails where SDate='" + ViewState["__AttDate__"].ToString() + "' AND DptId='" + ddlDepartmentList.SelectedItem.Value + "'  AND SftId=" + ddlShiftName.Items[b].Value.ToString() + "", dtEmpInfo);

                        else sqlDB.fillDataTable("select EmpId,convert(int,Right(EmpCardNo,LEN(EmpCardNo)-7)) as EmpCardNo,EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate,SftId,IsWeekend,GId from v_ShiftTransferInfoDetails where SDate='" + ViewState["__AttDate__"].ToString() + "' AND EmpCardNo Like'%" + txtCardNo.Text.Trim() + "' AND DptId='" + ddlDepartmentList.SelectedItem.Value + "' AND SftId=" + ddlShiftName.Items[b].Value.ToString() + "", dtEmpInfo);
                        if(b==18)
                        {

                        }
                        ShiftId=ddlShiftName.Items[b].Value;

                       
                        AttendanceImportProcessing(ShiftId, ForAllEmployes,b);
                       
                    }
                    
                }
                else
                {
                    dtEmpInfo = new DataTable();
                    if (rblImportType.SelectedValue.ToString().Equals("FullImport")) sqlDB.fillDataTable("select EmpId,Convert(int,Right(EmpCardNo,LEN(EmpCardNo)-7)) as EmpCardNo,EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate,SftId,IsWeekend,GId from v_ShiftTransferInfoDetails where SDate='" + ViewState["__AttDate__"] .ToString()+ "' AND DptId='" + ddlDepartmentList.SelectedItem.Value + "'  AND SftId=" + ddlShiftName.SelectedValue.ToString() + "", dtEmpInfo);

                    else sqlDB.fillDataTable("select EmpId,Convert(int,Right(EmpCardNo,LEN(EmpCardNo)-7)) as EmpCardNo,EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate,SftId,IsWeekend,GId from v_ShiftTransferInfoDetails where SDate='" + ViewState["__AttDate__"].ToString() + "' AND EmpCardNo Like'%" + txtCardNo.Text.Trim() + "' AND DptId='" + ddlDepartmentList.SelectedItem.Value + "' AND SftId=" + ddlShiftName.SelectedValue.ToString() + "", dtEmpInfo);
                    
                    ShiftId = ddlShiftName.SelectedValue.ToString();
                    AttendanceImportProcessing(ShiftId, ForAllEmployes,0);
                    
                }

                FromDate = FromDate.AddDays(1);
            }

            if (ImportProcess)
            {
                ImportProcess = false;
                ProgressBar1.Value = 0;
                rblImportType.SelectedIndex = 0;
                LoadGrid();
            }
        }

        private void AttendanceImportProcessing(string ShiftId, bool ForAllEmployes,byte SelectedShift_Index)
        {
            try
            {
                // check employees is exists
                if (dtEmpInfo.Rows.Count == 0)
                {
                    if (ViewState["__ToDate__"].ToString().Equals(selectedDate))  
                        if (SelectedShift_Index.ToString()==ViewState["__TotalNoShift__"].ToString())  // for find out number ofrows of departments Shift
                        {
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                            lblErrorMessage.Text = "Any employees are not exists of this date of the shift";
                            return;
                        }
                }
                // for delete existing attendance record
               deleteExistingRecordByDate_AttManual(ForAllEmployes,ShiftId);

                // find weekend,holydays or others offdays
                sqlDB.fillDataTable("select Convert(varchar(11),Offdate,105) as OffDate,Reason from v_AllOffDays where OffDate='" + ViewState["__AttDate__"].ToString() + "' AND CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "'", dt = new DataTable());

                if (dt.Rows.Count > 0)
                {
                    //transfer all Employee info for set weekend or others holydays
                    setWeekend_Others_Holyday(ViewState["__AttDate__"].ToString(), dt.Rows[0]["Reason"].ToString(),ShiftId);
                    return;
                }

                // Method calling for set daily attendance as record
                if(dtEmpInfo.Rows.Count>0)
                settblAttendanceRecord(ViewState["__AttDate__"].ToString(), ForAllEmployes, ShiftId,SelectedShift_Index);


                ////////////////////

                //  loadWeekendInfo(txtFullAttDate.Text.Substring(5, 2) + '-' + txtFullAttDate.Text.Substring(0, 4));
               
            }
            catch { }
        }

        private void settblAttendanceRecord(string attDate, bool ForAllStudents, string ShiftId, byte SelectedShift_Index)
        {
            try
            {
                

                // for get shift information

                string CompanyId=(ddlCompanyList.SelectedValue.ToString().Equals("0000"))?ViewState["__CompanyId__"].ToString():ddlCompanyList.SelectedItem.Value.ToString();

                DataTable dtShift = new DataTable();
                sqlDB.fillDataTable("select SftStartTime,SftEndTime,StartPunchCountTime,SftAcceptableLate,SftOverTime,AcceptableTimeAsOT,SftStartTimeIndicator, SftEndTimeIndicator from HRD_Shift where CompanyId='" + CompanyId + "' AND SftId=" + ShiftId + "", dtShift);
                ViewState["__OTM__"] = dtShift.Rows[0]["AcceptableTimeAsOT"].ToString();




                //if (bool.Parse(dtShift.Rows[0]["SftOverTime"].ToString()).Equals(true)) FindOverTimeAcceptalbeMinutes();

                var CHMS = TimeSpan.Parse(Convert.ToDateTime(dtShift.Rows[0]["SftStartTime"].ToString()).Hour.ToString()+":"+Convert.ToDateTime(dtShift.Rows[0]["SftStartTime"].ToString()).Minute+":"+Convert.ToDateTime(dtShift.Rows[0]["SftStartTime"].ToString()).Second);
                CHMS = CHMS + TimeSpan.Parse("00:" + Convert.ToInt32(dtShift.Rows[0]["SftAcceptableLate"].ToString())+":00");
               

                int COutH = Convert.ToDateTime(dtShift.Rows[0]["SftEndTime"].ToString()).Hour;  // for get office start hour
                int COutM = Convert.ToDateTime(dtShift.Rows[0]["SftEndTime"].ToString()).Minute;  // for get office start Minute

                string DailyStartTimeALT_CloseTime = Convert.ToDateTime(dtShift.Rows[0]["SftStartTime"].ToString()).Hour.ToString() + ":" + Convert.ToDateTime(dtShift.Rows[0]["SftStartTime"].ToString()).Minute.ToString() + ":" + Convert.ToDateTime(dtShift.Rows[0]["SftStartTime"].ToString()).Second.ToString() + ":" + dtShift.Rows[0]["SftAcceptableLate"].ToString() + ":" + COutH + ":" + COutM;
                
                for (i = 0; i < dtEmpInfo.Rows.Count; i++)
                {

                    if (dtEmpInfo.Rows[i]["EmpCardNo"].ToString() == "1365")
                    {

                    }
                    if (!CompareJoiningDateAndLogindate(i)) // check joining date and attendance date
                    {
                        if (!ForAllStudents)  // this part will be active, when accendance count for single employee
                        {
                            lblMessage.InnerText = "error->This employee is not joined in this date.";
                            return;
                        }
                    }
                    else i = j;
                    sqlDB.fillDataTable("select distinct Badgenumber,Format(CHECKTIME,'yyyy/MM/dd HH:mm:ss') as CHECKTIME from v_CHECKINOUT where FORMAT(CheckTime,'yyyy-MM-dd')='" + attDate + "' AND Badgenumber ='" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "'  order by CHECKTIME", dt = new DataTable());

                    if (dt.Rows.Count > 0)  // if employe is punched or present of this day
                    {

                        AttendanceCount(dtShift, DailyStartTimeALT_CloseTime, COutH, COutM, CHMS.ToString(), bool.Parse(dtEmpInfo.Rows[i]["IsWeekend"].ToString()));                                          
                    }

                    else  // if any punched are not founded then he/she either (absent-Leave),Weekend
                    {
                        string AttStatus = "a";
                        string StateStatus = "Absent";


                        if (bool.Parse(dtEmpInfo.Rows[i]["IsWeekend"].ToString()))
                        {
                            AttStatus = "w";
                            StateStatus = "Weekly Holiday";
                        }
                        

                        // Here first time check ayn leave is exists for this employee

                        DataTable dtLeaveInfo = new DataTable();
                        sqlDB.fillDataTable("select la.LACode from Leave_LeaveApplication la where la.IsApproved='true' AND la.LACode=(select LACode from Leave_LeaveApplicationDetails  where EmpId='" + dtEmpInfo.Rows[i]["EmpId"].ToString() + "' AND LeaveDate='" + ViewState["__AttDate__"].ToString() + "')", dtLeaveInfo);

                        if (dtLeaveInfo.Rows.Count > 0)
                        {
                            System.Data.SqlClient.SqlCommand cmd;

                            // find Todate of this leave
                            sqlDB.fillDataTable("select FORMAT(ToDate,'dd-MM-yyyy') as ToDate,LeaveId,LeaveName,LACode from v_Leave_LeaveApplication where LACode=" + dtLeaveInfo.Rows[0]["LACode"].ToString() + "", dt = new DataTable());

                            // if Todate is equal of current select days then below code is execute
                            if (selectedDate.Equals(dt.Rows[0]["ToDate"].ToString()))
                            {

                                cmd = new System.Data.SqlClient.SqlCommand("Update Leave_LeaveApplication set IsProcessessed='0' where LACode= " + dt.Rows[0]["LACode"].ToString() + "", sqlDB.connection);
                                cmd.ExecuteNonQuery();

                            }

                            // for changed used status for leave 
                            cmd = new System.Data.SqlClient.SqlCommand("Update Leave_LeaveApplicationDetails set used='1' where LeaveDate='" + attDate + "' AND LACode=" + dt.Rows[0]["LACode"].ToString() + "", sqlDB.connection);
                            cmd.ExecuteNonQuery();

                            AttStatus = "lv"; StateStatus = dt.Rows[0]["LeaveName"].ToString();
                        }

                        // below part is same for absent and leave attendance count 

                        try
                        {
                            string[] getColumns = { "EmpId", "AttDate", "EmpTypeId", "InHour", "InMin", "InSec", "OutHour", "OutMin", "OutSec", "BreakStartHour", "BreakStartMin", "BreakEndHour", "BreakEndMin", "AttStatus", "StateStatus", "DailyStartTimeALT_CloseTime", "OverTime", "SftId", "DptId", "CompanyId","GId" };
                            string[] getValues = { dtEmpInfo.Rows[i]["EmpId"].ToString(), convertDateTime.getCertainCulture(selectedDate).ToString(),
                                                 dtEmpInfo.Rows[i]["EmpTypeId"].ToString(), "00", "00", "00", "00", "00", "00","00","00","00","00", AttStatus,
                                                 StateStatus, "00:00:00:00:00:00","0",dtEmpInfo.Rows[i]["SftId"].ToString(),ddlDepartmentList.SelectedValue.ToString(),ddlCompanyList.SelectedValue,dtEmpInfo.Rows[i]["GId"].ToString()};
                            SQLOperation.forSaveValue("tblAttendanceRecord", getColumns, getValues, sqlDB.connection);

                            ProgressBar1.Value = val++;
                            System.Threading.Thread.Sleep(2);
                            ImportProcess = true;
                            
                            
                        }
                        catch { }
                    }

                }
               
                
                if (dtEmpInfo.Rows.Count > 0 && ViewState["__ToDate__"].ToString().Equals(selectedDate) && SelectedShift_Index.ToString()==ViewState["__TotalNoShift__"].ToString())
                {
                    ImportProcess = false;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Successfully attendance counted of "+ddlShiftName.SelectedItem.Text+" shift of "+ddlDepartmentList.SelectedItem.Text+" department ";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    rblImportType.SelectedIndex = 0;
                    LoadGrid();
                }

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        
        

        private void setEmpIdAndCardNo(OleDbConnection cont)
        {
            try
            {
                OleDbCommand cmd;
                cmd = new OleDbCommand("delete from TempEmpCardNo", cont);
                cmd.ExecuteNonQuery();

                for(int i=0;i<dtEmpInfo.Rows.Count;i++)
                {

                    cmd = new OleDbCommand("insert into TempEmpCardNo (EmpId,EmpCardNo) values ('" + dtEmpInfo.Rows[i]["EmpId"].ToString() + "','" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "')", cont);
                    cmd.ExecuteNonQuery();
                     

                }
            }
            catch { }
        }

        private void setWeekend_Others_Holyday(string attDate, string Reason,string ShiftId)
        {
            try
            {

                if (dtEmpInfo.Rows.Count == 1) deleteExistingRecordByDate_AttManual(false,ShiftId);
                else deleteExistingRecordByDate_AttManual(true,ShiftId);

                string attStatus = (Reason.Trim().Equals("Weekly Holiday")) ? "w" : "h";
                string temAttstatus = attStatus;
                Reason = (attStatus == "h") ? "Holiday" : "Weekly Holiday";
               
                DataTable dtShift = new DataTable();
                sqlDB.fillDataTable("select SftStartTime,SftEndTime,StartPunchCountTime,SftAcceptableLate,SftOverTime,AcceptableTimeAsOT,SftStartTimeIndicator, SftEndTimeIndicator from HRD_Shift where CompanyId='" + ddlCompanyList.SelectedItem.Value + "' AND SftId=" + ShiftId + "", dtShift);
                ViewState["__OTM__"] = dtShift.Rows[0]["AcceptableTimeAsOT"].ToString();

                var CHMS = TimeSpan.Parse(Convert.ToDateTime(dtShift.Rows[0]["SftStartTime"].ToString()).Hour.ToString() + ":" + Convert.ToDateTime(dtShift.Rows[0]["SftStartTime"].ToString()).Minute + ":" + Convert.ToDateTime(dtShift.Rows[0]["SftStartTime"].ToString()).Second);
                CHMS = CHMS + TimeSpan.Parse("00:" + Convert.ToInt32(dtShift.Rows[0]["SftAcceptableLate"].ToString()) + ":00");


                int COutH = Convert.ToDateTime(dtShift.Rows[0]["SftEndTime"].ToString()).Hour;  // for get office start hour
                int COutM = Convert.ToDateTime(dtShift.Rows[0]["SftEndTime"].ToString()).Minute;  // for get office start Minute

                string DailyStartTimeALT_CloseTime = Convert.ToDateTime(dtShift.Rows[0]["SftStartTime"].ToString()).Hour.ToString() + ":" + Convert.ToDateTime(dtShift.Rows[0]["SftStartTime"].ToString()).Minute.ToString() + ":" + Convert.ToDateTime(dtShift.Rows[0]["SftStartTime"].ToString()).Second.ToString() + ":" + dtShift.Rows[0]["SftAcceptableLate"].ToString() + ":" + COutH + ":" + COutM;


                for (i = 0; i < dtEmpInfo.Rows.Count; i++)
                {

                    if (CompareJoiningDateAndLogindate(i)) // check joining date and attendance date
                    {
                        dt = new DataTable();

                        if (!bool.Parse(dtEmpInfo.Rows[i]["IsWeekend"].ToString()))  // for test individually employee wise weekend.that is for all normally weekend
                        {
                            sqlDB.fillDataTable("select distinct Badgenumber,CHECKTIME from v_CHECKINOUT where FORMAT(CheckTime,'yyyy-MM-dd')='" + attDate + "' AND Badgenumber ='" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "'  order by CHECKTIME", dt);
                            bool result = false;


                            if (dt.Rows.Count > 0) result = AttendanceCount(dtShift, DailyStartTimeALT_CloseTime, COutH, COutM, CHMS.ToString(), true);  // if return false then it will be counted as holyday or weekend or others   



                            if (!result)
                            {
                                string[] getColumns = { "EmpId", "EmpTypeId", "AttDate", "InHour", "InMin", "InSec", "OutHour", "OutMin", "OutSec", "BreakStartHour", "BreakStartMin", "BreakEndHour", "BreakEndMin", "AttStatus", "StateStatus", "DailyStartTimeALT_CloseTime", "OverTime", "SftId", "DptId", "CompanyId","GId" };
                                string[] getValues = { dtEmpInfo.Rows[i]["EmpId"].ToString(), dtEmpInfo.Rows[0]["EmpTypeId"].ToString(), convertDateTime.getCertainCulture(attDate).ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", attStatus, Reason, "00:00:00:00:00:00", "0", 
                                                         dtEmpInfo.Rows[i]["SftId"].ToString(),ddlDepartmentList.SelectedValue,ddlCompanyList.SelectedValue,dtEmpInfo.Rows[i]["GId"].ToString() };
                                SQLOperation.forSaveValue("tblAttendanceRecord", getColumns, getValues, sqlDB.connection);
                                ProgressBar1.Value = val++;
                                System.Threading.Thread.Sleep(2);
                                ImportProcess = true;
                            }
                        }

                        else  // if individually weekend are founded then
                        {
                            sqlDB.fillDataTable("select Badgenumber,CHECKTIME from v_CHECKINOUT where FORMAT(CheckTime,'yyyy-MM-dd')='" + attDate + "' AND Badgenumber ='" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "'  order by CHECKTIME", dt);
                            bool result = false;


                            if (dt.Rows.Count > 0) result = AttendanceCount(dtShift, DailyStartTimeALT_CloseTime, COutH, COutM, CHMS.ToString(), true);  // if return false then it will be counted as holyday or weekend or others   
                            if (!result)
                            {
                                string[] getColumns = { "EmpId", "EmpTypeId", "AttDate", "InHour", "InMin", "InSec", "OutHour", "OutMin", "OutSec", "BreakStartHour", "BreakStartMin", "BreakEndHour", "BreakEndMin", "AttStatus", "StateStatus", "DailyStartTimeALT_CloseTime", "OverTime", "SftId", "DptId", "CompanyId","GId" };
                                string[] getValues = { dtEmpInfo.Rows[i]["EmpId"].ToString(), dtEmpInfo.Rows[0]["EmpTypeId"].ToString(), convertDateTime.getCertainCulture(attDate).ToString(), "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "a", "Absent", DailyStartTimeALT_CloseTime, "0", dtEmpInfo.Rows[i]["SftId"].ToString(),
                                                         ddlDepartmentList.SelectedValue,ddlCompanyList.SelectedValue,dtEmpInfo.Rows[i]["GId"].ToString() };
                                SQLOperation.forSaveValue("tblAttendanceRecord", getColumns, getValues, sqlDB.connection);
                                System.Threading.Thread.Sleep(2);
                                ImportProcess = true;
                                
                            }
                        }
                       
                    }
                        
                }
                
                if (dtEmpInfo.Rows.Count > 0 && ViewState["__ToDate__"].ToString().Equals(selectedDate))
                {
                    ImportProcess = false;
                    lblMessage.InnerText = "success->Successfully attendance counted";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    rblImportType.SelectedIndex = 0;
                    LoadGrid();
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "warning->" + ex.Message;
            }
        }
        DataTable dtAllShiftList = new DataTable();
        private void loadAAllShiftInfo()
        {
            try
            {
                sqlDB.fillDataTable("select SftId,SftStartTime,SftEndTime,StartPunchCountTime,SftAcceptableLate,SftOverTime,AcceptableTimeAsOT,SftStartTimeIndicator, SftEndTimeIndicator from HRD_Shift where CompanyId='" + ddlCompanyList.SelectedItem.Value + "'", dtAllShiftList);
            }
            catch { }
        }
        private bool AttendanceCount(DataTable dtShift, string DailyStartTimeALT_CloseTime, int COutH, int COutM, string CHM_S,bool IsWeekendHolydayOthers)
        {
            try
            {
                string InHur = "00";
                string InMin = "00";
                string InSec = "00";
                string OutHur = "00";
                string OutMin = "00";
                string OutSec = "00";
                string StateStatus = "Present";
                var CHMS = TimeSpan.Parse(CHM_S);

                byte k = 0; bool CountPresent = false;
   
                if(IsWeekendHolydayOthers)   // if Weekend or holyday setup but employe is present to do tasks
                {
                   
                    var PunchTime = TimeSpan.Parse(DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim());
                    ViewState["__FirstPunchOfDay__"] = PunchTime; // its first punch of day .that is used for count over time.
                   
                    PunchTime = TimeSpan.Parse(DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim());

                       
                    InHur = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                    InMin = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                    InSec = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim();

                    OutHur = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                    OutMin = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                    OutSec = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim();

                    CountPresent = true;
                    ViewState["__LastPunchOfDay__"] = OutHur + ":" + OutMin + ":" + OutSec;      
                    getTotalOverTimeBy_DutyHoure(dtShift,true);
                    CountPresent = true; ;

                    
                }
                //if shift time is PM to AM then that means (Night Shift)----------------------------------------------------------------------- 
                else if (dtShift.Rows[0]["SftStartTimeIndicator"].ToString().Equals("PM") && dtShift.Rows[0]["SftEndTimeIndicator"].ToString().Equals("AM"))
                {
                    var PunchCountTime = TimeSpan.Parse(dtShift.Rows[0]["StartPunchCountTime"].ToString());
                    var PunchTime = TimeSpan.Parse(DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim());
                    ViewState["__FirstPunchOfDay__"] = PunchTime; // its first punch of day .that is used for count over time.
                    for (k = 0; k < dt.Rows.Count; k++)
                    {
                        PunchTime = TimeSpan.Parse(DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim());
                        
                        if (PunchTime >= PunchCountTime)
                        {
                            InHur = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                            InMin = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                            InSec = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim();

                            string[] Dates = ViewState["__AttDate__"].ToString().Split('-');
                            DateTime AttDateForLogoutTime = new DateTime(int.Parse(Dates[0]),int.Parse(Dates[1]), int.Parse(Dates[2]));
                            AttDateForLogoutTime = AttDateForLogoutTime.AddDays(1);
                            Dates = AttDateForLogoutTime.ToString().Split('/');

                            /* Here occured two events.
                             
                            1: He/She is assigned for morning shift as normal duty.So, 
                            2:He/She can continue duty in morning shift for overtime. */

                            string Month = (Dates[0].ToString().Length == 1) ? "0" + Dates[0] : Dates[0];
                            string Day = (Dates[1].ToString().Length == 1) ? "0" + Dates[1] : Dates[1];

                            string temDate = Dates[2].Substring(0, 4) + "-" + Month + "-" + Day;

                            //-----------------------------------------------------------------------------
                            DataRow[] dr = null;
                            try
                            {
                                DataTable dtEShiftInfo = new DataTable(); // EShift=Exists Shift Info
                                sqlDB.fillDataTable("select SftId from v_ShiftTransferInfoDetails where EmpId='" + dtEmpInfo.Rows[i]["EmpId"].ToString() + "' AND SDate='" + temDate + "'", dtEShiftInfo);

                                dr = dtAllShiftList.Select("SftId=" + dtEShiftInfo.Rows[0]["SftId"].ToString() + "");
                            }
                            catch { dr = null; }
                            //-----------------------------------------------------------------------------

                            //------------------------------ 1:) Events are occured -----------------------------------------------
                            DataTable dtLogoutTime = new DataTable();
                            if (dr != null)
                            {
                                if (dr[0]["SftStartTimeIndicator"].ToString().Trim().Equals("AM") && dr[0]["SftEndTimeIndicator"].ToString().Trim().Equals("PM") || dr[0]["SftStartTimeIndicator"].ToString().Trim().Equals("AM") && dr[0]["SftEndTimeIndicator"].ToString().Trim().Equals("AM"))
                                {
                                    sqlDB.fillDataTable("select distinct Badgenumber,CHECKTIME from v_CHECKINOUT where FORMAT(CheckTime,'yyyy-MM-dd')='" + temDate + "' AND Format(CheckTime,'HH:MM')<='12:00' AND Badgenumber ='" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "'  order by CHECKTIME", dtLogoutTime);
                                    if (dtLogoutTime.Rows.Count == 1 || dtLogoutTime.Rows.Count == 2)
                                    {
                                        OutHur = ((DateTime.Parse(dtLogoutTime.Rows[0]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dtLogoutTime.Rows[0]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dtLogoutTime.Rows[0]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                        OutMin = ((DateTime.Parse(dtLogoutTime.Rows[0]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dtLogoutTime.Rows[0]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dtLogoutTime.Rows[0]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                        OutSec = ((DateTime.Parse(dtLogoutTime.Rows[0]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dtLogoutTime.Rows[0]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dtLogoutTime.Rows[0]["CHECKTIME"].ToString()).Second.ToString().Trim();
                                    }

                                    else if (dtLogoutTime.Rows.Count > 2)
                                    {
                                        OutHur = ((DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 2]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 2]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 2]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                        OutMin = ((DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 2]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 2]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 2]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                        OutSec = ((DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 2]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 2]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 2]["CHECKTIME"].ToString()).Second.ToString().Trim();
                                    }
                                    else
                                    {
                                        OutHur = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                        OutMin = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                        OutSec = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim();
                                    }

                                }

                            //-------------------------------------------1:) Events are Colosed--------------------------------------------

                            //--------------------------------------2:) Events are occured-----------------------------------------------                        
                                else
                                {
                                    sqlDB.fillDataTable("select distinct Badgenumber,CHECKTIME from v_CHECKINOUT where FORMAT(CheckTime,'yyyy-MM-dd')='" + temDate + "' AND Format(CheckTime,'HH:MM')>='8:00' AND AND Format(CheckTime,'HH:MM')<='2:45' AND Badgenumber ='" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "'  order by CHECKTIME", dtLogoutTime);
                                    if (dtLogoutTime.Rows.Count > 0)
                                    {
                                        OutHur = ((DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                        OutMin = ((DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                        OutSec = ((DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dtLogoutTime.Rows[dtLogoutTime.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim();
                                    }
                                    else
                                    {
                                        OutHur = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                        OutMin = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                        OutSec = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim();
                                    }
                                }
                            }
                            else
                            {
                                sqlDB.fillDataTable("select distinct Badgenumber,CHECKTIME from v_CHECKINOUT where FORMAT(CheckTime,'yyyy-MM-dd')='" + temDate + "' AND Format(CheckTime,'HH:MM')>='8:00' AND AND Format(CheckTime,'HH:MM')<='2:45' AND Badgenumber ='" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "'  order by CHECKTIME", dtLogoutTime);
                                if (dtLogoutTime.Rows.Count > 0)
                                {
                                    OutHur = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                    OutMin = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                    OutSec = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim();
                                }
                                else
                                {
                                    OutHur = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                    OutMin = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                    OutSec = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim();
                                }
                            }
                            //---------------------------------------2:) Events are Closed------------------------------------------------
                            CountPresent = true;
                            ViewState["__LastPunchOfDay__"] = OutHur + ":" + OutMin + ":" + OutSec;
                            //if (bool.Parse(dtShift.Rows[0]["SftOverTime"].ToString()).Equals(true)) getTotalOverTime(dtShift,int.Parse(OutHur), int.Parse(OutMin), COutH, COutM, true);
                            if (bool.Parse(dtShift.Rows[0]["SftOverTime"].ToString()).Equals(true)) getTotalOverTimeBy_DutyHoure(dtShift,false);
                            break;
                        }

                    }

                }
                //---------------------------------------------------------------------------------------------------------------------
                //  if shift time is (AM to PM) OR (AM to AM) OR (PM to PM) then------------------------------------------------------- 
                else
                {
                    string[] Dates = ViewState["__AttDate__"].ToString().Split('-');
                    DateTime AttDateForLogoutTime = new DateTime(int.Parse(Dates[0]), int.Parse(Dates[1]), int.Parse(Dates[2]));
                    AttDateForLogoutTime = AttDateForLogoutTime.AddDays(-1);
                    Dates = AttDateForLogoutTime.ToString().Split('/');

                    string Month = (Dates[0].ToString().Length == 1) ? "0" + Dates[0] : Dates[0];
                    string Day = (Dates[1].ToString().Length == 1) ? "0" + Dates[1] : Dates[1];

                    string temDate = Dates[2].Substring(0, 4) + "-" + Month + "-" + Day;
                    bool HasPrevoiusRoster = false;
                    DataRow[] dr = null;
                    try
                    {
                        //-----------------------------------------------------------------------------
                        DataTable dtEShiftInfo = new DataTable(); // EShift=Exists Shift Info
                        sqlDB.fillDataTable("select SftId from v_ShiftTransferInfoDetails where EmpId='" + dtEmpInfo.Rows[i]["EmpId"].ToString() + "' AND SDate='" + temDate + "'", dtEShiftInfo);

                        dr = dtAllShiftList.Select("SftId=" + dtEShiftInfo.Rows[0]["SftId"].ToString() + "");

                        //-----------------------------------------------------------------------------    
                        
                    }
                    catch { dr = null; }

                    var PunchCountTime = TimeSpan.Parse(dtShift.Rows[0]["StartPunchCountTime"].ToString());
                    DataTable dtLogoutTime = new DataTable();
                    
                    // if previous date duty are night shift and Todays duty are morning shift then this section executed
                    if (dr!=null)
                    {
                        if (dr[0]["SftStartTimeIndicator"].ToString().Trim().Equals("PM") && dr[0]["SftEndTimeIndicator"].ToString().Trim().Equals("AM"))
                        {

                            if (dtShift.Rows[0]["SftStartTimeIndicator"].ToString().Equals("AM") && dtShift.Rows[0]["SftEndTimeIndicator"].ToString().Equals("PM"))
                            {
                                sqlDB.fillDataTable("select distinct Badgenumber,CHECKTIME from v_CHECKINOUT where FORMAT(CheckTime,'yyyy-MM-dd')='" + ViewState["__AttDate__"].ToString() + "' AND Format(CheckTime,'HH:MM')<='12:00' AND Badgenumber ='" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "'  order by CHECKTIME", dtLogoutTime);


                                if (dtLogoutTime.Rows.Count > 0)
                                {

                                    string bb = (int.Parse(dtLogoutTime.Rows.Count.ToString()) - 1).ToString();
                                    for (k = byte.Parse(bb); k >= 0; k--)
                                    {
                                        var PunchTime = TimeSpan.Parse(DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim());
                                        ViewState["__FirstPunchOfDay__"] = PunchTime;
                                        if (PunchTime >= PunchCountTime)
                                        {
                                            InHur = ((DateTime.Parse(dtLogoutTime.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dtLogoutTime.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dtLogoutTime.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                            InMin = ((DateTime.Parse(dtLogoutTime.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dtLogoutTime.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dtLogoutTime.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                            InSec = ((DateTime.Parse(dtLogoutTime.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dtLogoutTime.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dtLogoutTime.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim();

                                            OutHur = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                            OutMin = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                            OutSec = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim();
                                            ViewState["__LastPunchOfDay__"] = OutHur + ":" + OutMin + ":" + OutSec;

                                            CountPresent = true;
                                            if (bool.Parse(dtShift.Rows[0]["SftOverTime"].ToString()).Equals(true)) getTotalOverTimeBy_DutyHoure(dtShift, false);
                                            break;
                                        }
                                    }
                                }

                            }

                            else
                            {
                                var PunchTime = TimeSpan.Parse(DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim());
                                ViewState["__FirstPunchOfDay__"] = PunchTime; // its first punch of day .that is used for count over time.
                                for (k = 0; k < dt.Rows.Count; k++)
                                {
                                    PunchTime = TimeSpan.Parse(DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim());

                                    if (PunchTime >= PunchCountTime)
                                    {
                                        InHur = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                        InMin = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                        InSec = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim();

                                        OutHur = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                        OutMin = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                        OutSec = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim();

                                        CountPresent = true;
                                        ViewState["__LastPunchOfDay__"] = OutHur + ":" + OutMin + ":" + OutSec;
                                        //if (bool.Parse(dtShift.Rows[0]["SftOverTime"].ToString()).Equals(true)) getTotalOverTime(int.Parse(OutHur), int.Parse(OutMin), COutH, COutM, false);
                                        if (bool.Parse(dtShift.Rows[0]["SftOverTime"].ToString()).Equals(true)) getTotalOverTimeBy_DutyHoure(dtShift, false);
                                        break;
                                    }

                                }
                            }


                        }

                        else
                        {
                            var PunchTime = TimeSpan.Parse(DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim());
                            ViewState["__FirstPunchOfDay__"] = PunchTime; // its first punch of day .that is used for count over time.
                            for (k = 0; k < dt.Rows.Count; k++)
                            {
                                PunchTime = TimeSpan.Parse(DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim());

                                if (PunchTime >= PunchCountTime)
                                {
                                    InHur = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                    InMin = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                    InSec = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim();

                                    OutHur = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                    OutMin = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                    OutSec = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim();

                                    CountPresent = true;
                                    ViewState["__LastPunchOfDay__"] = OutHur + ":" + OutMin + ":" + OutSec;
                                    //if (bool.Parse(dtShift.Rows[0]["SftOverTime"].ToString()).Equals(true)) getTotalOverTime(int.Parse(OutHur), int.Parse(OutMin), COutH, COutM, false);
                                    if (bool.Parse(dtShift.Rows[0]["SftOverTime"].ToString()).Equals(true)) getTotalOverTimeBy_DutyHoure(dtShift, false);
                                    break;
                                }

                            }
                        }
                    }
                    else
                    {
                        var PunchTime = TimeSpan.Parse(DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim());
                        ViewState["__FirstPunchOfDay__"] = PunchTime; // its first punch of day .that is used for count over time.
                        for (k = 0; k < dt.Rows.Count; k++)
                        {
                            PunchTime = TimeSpan.Parse(DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim());

                            if (PunchTime >= PunchCountTime)
                            {
                                InHur = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                InMin = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                InSec = ((DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString().Trim();

                                OutHur = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Hour.ToString().Trim();
                                OutMin = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Minute.ToString().Trim();
                                OutSec = ((DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim()).Length == 1) ? "0" + DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim() : DateTime.Parse(dt.Rows[dt.Rows.Count - 1]["CHECKTIME"].ToString()).Second.ToString().Trim();

                                CountPresent = true;
                                ViewState["__LastPunchOfDay__"] = OutHur + ":" + OutMin + ":" + OutSec;
                                //if (bool.Parse(dtShift.Rows[0]["SftOverTime"].ToString()).Equals(true)) getTotalOverTime(int.Parse(OutHur), int.Parse(OutMin), COutH, COutM, false);
                                if (bool.Parse(dtShift.Rows[0]["SftOverTime"].ToString()).Equals(true)) getTotalOverTimeBy_DutyHoure(dtShift, false);
                                break;
                            }

                        }
                    } 


                   
                    
                }
                //---------------------------------------------------------------------------------------------------------------------

                // compare punch has many times for count logout time-------------------------------
                if (int.Parse(InHur) == int.Parse(OutHur) && int.Parse(InMin) == int.Parse(OutMin) && int.Parse(InSec) == int.Parse(OutSec))
                {
                    OutHur = "00"; OutMin = "00"; OutSec = "00";
                }
                //------------------------------------------------------------------------------------

                char attStatus = 'p';

                if (CountPresent)
                {
                    if (TimeSpan.Parse(DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Hour.ToString() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Minute.ToString() + ":" + DateTime.Parse(dt.Rows[k]["CHECKTIME"].ToString()).Second.ToString()) <= CHMS)
                        attStatus = 'p';
                    else attStatus = 'l';

                }
                else
                {
                    if (IsWeekendHolydayOthers || !IsWeekendHolydayOthers)
                    {
                        attStatus = 'a';
                        StateStatus = "Absent";
                        InHur = "00"; InMin = "00"; InSec = "00"; OutHur = "00"; OutMin = "00"; OutSec = "00";
                        ViewState["__OT__"] = "0";
                    }
                   

                    else return false;
                }

                
                       
                try
                {
                    string[] getColumns = { "EmpId", "AttDate", "EmpTypeId", "InHour", "InMin", "InSec", "OutHour", "OutMin", "OutSec", "BreakStartHour", "BreakStartMin", "BreakEndHour", "BreakEndMin", "AttStatus", "StateStatus", "DailyStartTimeALT_CloseTime", "OverTime", "SftId","DptId","CompanyId","GId" };
                    string[] getValues = { dtEmpInfo.Rows[i]["EmpId"].ToString(), convertDateTime.getCertainCulture(selectedDate).ToString(),dtEmpInfo.Rows[i]["EmpTypeId"].ToString(), InHur, 
                                                    InMin, InSec, OutHur, OutMin, OutSec,"00","00","00","00",attStatus.ToString(), 
                                                    StateStatus, DailyStartTimeALT_CloseTime,ViewState["__OT__"].ToString(),dtEmpInfo.Rows[i]["SftId"].ToString(),
                                                    ddlDepartmentList.SelectedValue.ToString(),ddlCompanyList.SelectedValue,dtEmpInfo.Rows[i]["GId"].ToString()};
                    SQLOperation.forSaveValue("tblAttendanceRecord", getColumns, getValues, sqlDB.connection);
                    ProgressBar1.Value =val++;
                    System.Threading.Thread.Sleep(2);
                    ImportProcess = true;
                    return  true;
                }
                catch { return false; }

                
            }
            catch {return false ; }
        }

        int val = 0;
        bool Datestatus = false;
        private bool CompareJoiningDateAndLogindate(int i)
        {
            try
            {
                Datestatus = false;
                j = i;

                DateTime LoginDate = (rblImportType.SelectedValue.ToString().Equals("FullImport")) ? DateTime.ParseExact(txtFullAttDate.Text.Trim(), "dd-MM-yyyy", null) : DateTime.ParseExact(txtPartialAttDate.Text.Trim(), "dd-MM-yyyy", null);
                DateTime JoiningDate = DateTime.ParseExact(dtEmpInfo.Rows[i]["EmpJoiningDate"].ToString(),"dd-MM-yyyy",null);

                if (LoginDate >= JoiningDate)
                {
                    Datestatus = true;
                    return true;

                }
                else
                {
                    i++;
                    if (i < dtEmpInfo.Rows.Count)
                    {

                        CompareJoiningDateAndLogindate(i);
                    }
                    if (Datestatus) return true;
                    else return false;
                }
            }
            catch { return false; }
        }

        private void FindOverTimeAcceptalbeMinutes()
        {
            try
            {
                sqlDB.fillDataTable("select AcceptableMinuteasOT from HRD_OthersSetting ",dt=new DataTable ());
                ViewState["__OTM__"] = dt.Rows[0]["AcceptableMinuteasOT"].ToString();
            }
            catch { }
        }

        
        private void getTotalOverTimeBy_DutyHoure(DataTable dtShift,bool IsHolyDayTasks)
        {
            try
            {
                TimeSpan LogInTime;
                TimeSpan LogOutTime;
                string[] GetTimeDuration = "".Split(' ');

                ViewState["__OT__"] = "0";

                TimeSpan SStartTime = TimeSpan.Parse(DateTime.Parse(dtShift.Rows[0]["SftStartTime"].ToString()).Hour + ":" + DateTime.Parse(dtShift.Rows[0]["SftStartTime"].ToString()).Minute);
                TimeSpan SEndTime = TimeSpan.Parse(DateTime.Parse(dtShift.Rows[0]["SftEndTime"].ToString()).Hour + ":" + DateTime.Parse(dtShift.Rows[0]["SftEndTime"].ToString()).Minute);
                
                if (IsHolyDayTasks)
                {
                    LogInTime = TimeSpan.Parse(ViewState["__FirstPunchOfDay__"].ToString());
                    LogOutTime = TimeSpan.Parse(ViewState["__LastPunchOfDay__"].ToString());
                    GetTimeDuration = (LogInTime - LogOutTime).ToString().Split(':');
                    GetTimeDuration[0] = ((int.Parse(GetTimeDuration[0])) * 60 + int.Parse(GetTimeDuration[1])).ToString();  

                    if (int.Parse(GetTimeDuration[0])>=360)
                    {
                        ViewState["__OT__"] = Math.Floor(double.Parse(GetTimeDuration[0]) / 60).ToString();
                        return;
                    }
                    return;
                }
                
                string[] TotalOfficeTime = "".Split(' '); 

                if (!dtShift.Rows[0]["SftEndTimeIndicator"].ToString().Equals("PM"))
                {
                    TotalOfficeTime = (SStartTime - SEndTime).ToString().Split(':');
                }
                else
                {
                    TotalOfficeTime = (SEndTime-SStartTime).ToString().Split(':');
                }
                TotalOfficeTime[0] = ((int.Parse(TotalOfficeTime[0]) * 60) + int.Parse(TotalOfficeTime[1])).ToString();


               // working time--------------------------------------------------------------------
                LogInTime = TimeSpan.Parse(ViewState["__FirstPunchOfDay__"].ToString());
                LogOutTime = TimeSpan.Parse(ViewState["__LastPunchOfDay__"].ToString());
               
                if (!dtShift.Rows[0]["SftEndTimeIndicator"].ToString().Equals("PM"))
                {
                    GetTimeDuration = (LogInTime - LogOutTime).ToString().Split(':');
                }
                else
                {
                    GetTimeDuration = (LogOutTime-LogInTime).ToString().Split(':');
                }

                GetTimeDuration[0] = ((int.Parse(GetTimeDuration[0])) * 60 + int.Parse(GetTimeDuration[1])).ToString();  
                //-----------------------------------------------------------------------------------


                string getExtraStayTime = (int.Parse(GetTimeDuration[0]) - int.Parse(TotalOfficeTime[0])).ToString();

                if (int.Parse(getExtraStayTime)>=int.Parse(ViewState["__OTM__"].ToString()))
                {
                    ViewState["__OT__"] = Math.Floor(double.Parse(getExtraStayTime) / 60).ToString();

                }


            }
            catch { }
        }


        // getTotalOverTime Method is used for count overtime by difference between office endtime-logout time;
        private void getTotalOverTime(int OutHur, int OutMin, int sftOutHur, int sftOutMin,bool Yes_PMtoAM)
        {
            try
            {
                ViewState["__OT__"]="0";
                TimeSpan ShiftEndTime =TimeSpan.Parse(sftOutHur + ":" + sftOutMin);
                TimeSpan LogOutTime =TimeSpan.Parse(OutHur + ":"+OutMin);
                if (!Yes_PMtoAM)
                {
                    if (LogOutTime > ShiftEndTime)
                    {

                        string OverTime = (LogOutTime - ShiftEndTime).ToString();
                        string[] getHoursAndMinuts = OverTime.Split(':');

                        int Hour = int.Parse(getHoursAndMinuts[0]);
                        int minute = int.Parse(getHoursAndMinuts[1]);

                        if (minute >= int.Parse(ViewState["__OTM__"].ToString())) Hour += 1;
                        ViewState["__OT__"] = Hour;
                    }
                }
                else
                {
                    if (LogOutTime < TimeSpan.Parse("24:00:00"))
                    {
                        if (LogOutTime > ShiftEndTime)
                        {
                            string OverTime = (LogOutTime - ShiftEndTime).ToString();
                            string[] getHoursAndMinuts = OverTime.Split(':');

                            int Hour = int.Parse(getHoursAndMinuts[0]);
                            int minute = int.Parse(getHoursAndMinuts[1]);

                            if (minute >= int.Parse(ViewState["__OTM__"].ToString())) Hour += 1;
                            ViewState["__OT__"] = Hour;
                        }
                    }
                }            
            }
            catch { }
        }

        DataTable dtHasPreviousAttendance;
        private bool checkHasAttendanceRecord()
        {
            try
            {
                dtHasPreviousAttendance = new DataTable();
                sqlDB.fillDataTable("select EmpId,EmpTypeId,AttManual,ATTStatus,StateStatus from tblAttendanceRecord where ATTDate='" + txtFullAttDate.Text.Trim() + "' AND  AttManual is null", dtHasPreviousAttendance);
                
                if (dtHasPreviousAttendance.Rows.Count > 0)
                {
                    cmd = new System.Data.SqlClient.SqlCommand("delete from tblAttendanceRecord where ATTDate='"+txtFullAttDate.Text.Trim()+"' AND ATTStatus not in ('LV') AND AttManual is null",sqlDB.connection);
                    cmd.ExecuteNonQuery();
                    return true;
                }   
                else return false;
            }
            catch { return false; }
        }

        
        protected void gvAttendance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadGrid();
            gvAttendance.PageIndex = e.NewPageIndex;
            gvAttendance.DataBind();
            //UpdatePanel1.Update();
        }


       

        protected void gvAttendance_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void partialImportClear()
        {
            txtCardNo.Text = "";
            txtPartialAttDate.Text = "";
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ddlCompanyList.SelectedValue);
               
                gvAttendance.DataSource = null;
                gvAttendance.DataBind();
            }
            catch { }
        }

        protected void ddlDepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                classes.commonTask.LoadShiftByDepartmentWithAll(ddlShiftName, ddlDepartmentList.SelectedValue.ToString());

            }
            catch { }
        }

        protected void rblDateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rblImportType.SelectedValue == "FullImport")
                {
                    if (rblDateType.SelectedValue == "MultipleDate")
                    {
                        spnFullFromDate.InnerText = "From Date";
                        spnFullToDate.Visible = true;
                        txtFullToDate.Visible = true;
                       

                        tdPartialFromDate.InnerText = "Date";
                        trPartialToDate.Visible = false;
                        
                    }
                    else
                    {
                        spnFullFromDate.InnerText = "Date";
                        spnFullToDate.Visible = false;
                        txtFullToDate.Visible = false;
                        txtFullToDate.Text = "";

                        tdPartialFromDate.InnerText = "Date";
                        trPartialToDate.Visible = false;
                       
                    }
                }
                else
                {
                    if (rblDateType.SelectedValue == "MultipleDate")
                    {
                        spnFullFromDate.InnerText = "Date";
                        spnFullToDate.Visible = false;
                        

                        tdPartialFromDate.InnerText = "From Date";
                        trPartialToDate.Visible = true;
                        txtFullToDate.Visible = false;
                    }
                    else
                    {
                        spnFullFromDate.InnerText = "Date";
                        spnFullToDate.Visible = false;


                        tdPartialFromDate.InnerText = "Date";
                        trPartialToDate.Visible = false ;
                        txtFullToDate.Visible = false;
                    }
                }

                txtPartialToDate.Text = "";
                txtFullToDate.Text = "";
            }
            catch { }
        }

        protected void ddlShiftName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rblImportType.SelectedValue!="FullImport" && ddlShiftName.SelectedItem.Text=="All")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Sorry you can't select all shift for partial process.";
                    ddlShiftName.Focus(); return;
                }
            }
            catch { }
        }

       


      
    }
}