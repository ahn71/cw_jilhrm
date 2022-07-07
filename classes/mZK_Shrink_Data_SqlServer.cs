using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SigmaERP.classes
{
    public class mZK_Shrink_Data_SqlServer
    {
        public SqlCommand cmd;
        // this method return an arry that is contain some data.DayStatus[0] = "True", DayStatus[1] = "W\H";
        public static string[] Check_Todays_Is_HolidayOrWeekend(string SelectedDate)
        {
            try
            {
               string[] DayStatus = new string[2];
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select SL from Attendance_WeekendInfo where WeekendDate='" + SelectedDate + "'", dt);
                if (dt.Rows.Count > 0)
                {
                    DayStatus[0] = "True";
                    DayStatus[1] = "W";
                    return DayStatus;
                }
                else
                {
                    dt = new DataTable();
                    sqlDB.fillDataTable("select HCode from tblHolydayWork where HDate='" + SelectedDate + "'", dt);
                    if (dt.Rows.Count > 0)
                    {
                        DayStatus[0] = "True";
                        DayStatus[1] = "H";
                    }
                    else
                        DayStatus[0] = "False";
                    return DayStatus;
                }

            }
            catch { return null; }
        }
        public static DataTable LoadOTherSettings(string CompanyId)
        {
            DataTable dt=new DataTable();
            try
            {               
                sqlDB.fillDataTable("select * from HRD_OthersSetting where CompanyId='" + CompanyId + "'", dt);
                return dt;
            }
            catch { return dt; }
        }
        public static DataTable LoadShorLeave(string Empid,string date)
        {
            DataTable dt = new DataTable();
            try
            {
                sqlDB.fillDataTable("select * from Leave_ShortLeave where EmpId='" + Empid + "' and LvDate='" + date + "' and LvStatus='1'", dt);
                return dt;
            }
            catch { return dt; }
        }
        public static string CheckOutDuty(string Empid, string date)
        {
            DataTable dt = new DataTable();
            try
            {
                sqlDB.fillDataTable("select SL from tblOutDuty where Status=1 and EmpId='"+ Empid + "' and FORMAT(Date,'yyyy-MM-dd')='"+ date + "'", dt);
                if (dt != null && dt.Rows.Count > 0)
                   return dt.Rows[0]["SL"].ToString();
                else
                return "";
            }
            catch { return ""; }
        }
        public static DataTable getOutDutyInfo(string ODID)
        {
            DataTable dt = new DataTable();
            try
            {
                sqlDB.fillDataTable("select SL,ISNULL(StraightFromHome,0) as StraightFromHome,ISNULL(StraightToHome,0) as StraightToHome,case when type=1 then 'True' else 'False' end as type from  tblOutDuty where SL=" + ODID, dt);

                return dt;
            }
            catch { return dt; }
        }
        public static void SaveAttendance_Status(string EmpId, string selectedDate, string EmpTypeId, string InHour, string InMin, string InSec, string OutHour, string OutMin, string OutSec,
                                            string AttStatus, string StateStatus, string OverTime, string SftId, string DptId, string DsgId, string CompanyId, string GId, string LateTime, string StayTime, string DailyStartTimeALT_CloseTime, string TiffinCount, string HolidayCount, string PaybleDays, string OtherOverTime, string TotalOverTime, string UserId,string IsHalfDayLeave,string IsHalfDayDeduct)
        {
            try
            {
                if (IsHalfDayDeduct == null)
                    IsHalfDayDeduct = "0";
                // RMS Attendance Saved Here
                DateTime dtTimeConvert;
                LateTime = (LateTime == null) ? "00:00:00" : LateTime;
                if (DateTime.TryParse(LateTime, out dtTimeConvert)) LateTime = dtTimeConvert.ToString("HH:mm:ss");
                dtTimeConvert = new DateTime();
                StayTime = (StayTime == null) ? "00:00:00" : StayTime;

                if (DateTime.TryParse(StayTime, out dtTimeConvert)) StayTime = dtTimeConvert.ToString("HH:mm:ss");
                string[] getColumns = { "EmpId", "AttDate", "EmpTypeId", "InHour", "InMin", "InSec", "OutHour", "OutMin", "OutSec",
                                        "AttStatus", "StateStatus",
                                        "DailyStartTimeALT_CloseTime", "OverTime", "SftId", "DptId","DsgId", "CompanyId", "GId","LateTime","StayTime","TiffinCount","HolidayCount","PaybleDays","OtherOverTime","TotalOverTime","UserId","IsHalfDayLeave","IsHalfDayDeduct"};

                string[] getValues = { EmpId, commonTask.ddMMyyyyToyyyyMMdd(selectedDate).ToString(),
                                                 EmpTypeId,InHour,InMin,InSec,OutHour,OutMin,OutSec,AttStatus,
                                                 StateStatus,DailyStartTimeALT_CloseTime,OverTime,SftId,DptId,DsgId,CompanyId,GId,LateTime,StayTime,TiffinCount,HolidayCount,PaybleDays,OtherOverTime,TotalOverTime,UserId, IsHalfDayLeave,IsHalfDayDeduct};
                try
                {
                    SQLOperation.forSaveValue("tblAttendanceRecord", getColumns, getValues, sqlDB.connection);
                }
                catch (Exception ex) { }
                if (AttStatus == "L" || AttStatus == "l")
                {
                    DataTable dtAdminlist = new DataTable();
                    
                    try
                    {
                        dtAdminlist = (DataTable)(HttpContext.Current.Session["__lnAdminList__"]);
                        for (int i = 0; i < dtAdminlist.Rows.Count; i++)
                        {
                            string[] getColumns_L = { "EmpId", "Date", "LateTime", "EmpSeen", "AdminID", "AdminSeen" };
                            string[] getValues_L = { EmpId, commonTask.ddMMyyyyToyyyyMMdd(selectedDate).ToString(), LateTime, "0", dtAdminlist.Rows[i]["AdminID"].ToString(), "0" };
                            SQLOperation.forSaveValue("nf_LateNotification", getColumns_L, getValues_L, sqlDB.connection);
                        }                
                    }
                    catch (Exception ex) { }
                }
               

                //------------------Attendance Punch time save in Punch log for manual att report by Nayem-----------
                SqlCommand cmd = new SqlCommand("delete tblAttendanceRecordPunchLog where EmpId='" + EmpId + "' and AttDate='" + commonTask.ddMMyyyyToyyyyMMdd(selectedDate).ToString() + "' ", sqlDB.connection);
                cmd.ExecuteNonQuery();
                string[] getColumns1 = { "EmpId", "AttDate", "PInHour", "PInMin", "PInSec", "POutHour", "POutMin", "POutSec" };

                string[] getValues1 = { EmpId, commonTask.ddMMyyyyToyyyyMMdd(selectedDate).ToString()
                                                 ,InHour,InMin,InSec,OutHour,OutMin,OutSec};
                SQLOperation.forSaveValue("tblAttendanceRecordPunchLog", getColumns1, getValues1, sqlDB.connection);
                //----------------------------------------------------------------------------------------------
            }
            catch { }
        }
        public static void SaveAttendance_Status(string EmpId, string selectedDate, string EmpTypeId, string InHour, string InMin, string InSec, string OutHour, string OutMin, string OutSec,
                                                string AttStatus, string StateStatus, string OverTime, string SftId, string DptId, string DsgId, string CompanyId, string GId, string LateTime, string StayTime, string DailyStartTimeALT_CloseTime, string TiffinCount, string HolidayCount, string PaybleDays, string OtherOverTime, string TotalOverTime, string UserId, string IsHalfDayLeave, string IsHalfDayDeduct,string ODID)
        {
            try
            {
                if (IsHalfDayDeduct == null)
                    IsHalfDayDeduct = "0";
                // RMS Attendance Saved Here
                DateTime dtTimeConvert;
                LateTime = (LateTime == null) ? "00:00:00" : LateTime;
                if (DateTime.TryParse(LateTime, out dtTimeConvert)) LateTime = dtTimeConvert.ToString("HH:mm:ss");
                dtTimeConvert = new DateTime();
                StayTime = (StayTime == null) ? "00:00:00" : StayTime;

                if (DateTime.TryParse(StayTime, out dtTimeConvert)) StayTime = dtTimeConvert.ToString("HH:mm:ss");
                string[] getColumns = { "EmpId", "AttDate", "EmpTypeId", "InHour", "InMin", "InSec", "OutHour", "OutMin", "OutSec",
                                        "AttStatus", "StateStatus",
                                        "DailyStartTimeALT_CloseTime", "OverTime", "SftId", "DptId","DsgId", "CompanyId", "GId","LateTime","StayTime","TiffinCount","HolidayCount","PaybleDays","OtherOverTime","TotalOverTime","UserId","IsHalfDayLeave","IsHalfDayDeduct","ODID"};

                string[] getValues = { EmpId, commonTask.ddMMyyyyToyyyyMMdd(selectedDate).ToString(),
                                                 EmpTypeId,InHour,InMin,InSec,OutHour,OutMin,OutSec,AttStatus,
                                                 StateStatus,DailyStartTimeALT_CloseTime,OverTime,SftId,DptId,DsgId,CompanyId,GId,LateTime,StayTime,TiffinCount,HolidayCount,PaybleDays,OtherOverTime,TotalOverTime,UserId, IsHalfDayLeave,IsHalfDayDeduct,ODID};
                try
                {
                    SQLOperation.forSaveValue("tblAttendanceRecord", getColumns, getValues, sqlDB.connection);
                }
                catch (Exception ex) { }
                if (AttStatus == "L" || AttStatus == "l")
                {
                    DataTable dtAdminlist = new DataTable();

                    try
                    {
                        dtAdminlist = (DataTable)(HttpContext.Current.Session["__lnAdminList__"]);
                        for (int i = 0; i < dtAdminlist.Rows.Count; i++)
                        {
                            string[] getColumns_L = { "EmpId", "Date", "LateTime", "EmpSeen", "AdminID", "AdminSeen" };
                            string[] getValues_L = { EmpId, commonTask.ddMMyyyyToyyyyMMdd(selectedDate).ToString(), LateTime, "0", dtAdminlist.Rows[i]["AdminID"].ToString(), "0" };
                            SQLOperation.forSaveValue("nf_LateNotification", getColumns_L, getValues_L, sqlDB.connection);
                        }
                    }
                    catch (Exception ex) { }
                }


                //------------------Attendance Punch time save in Punch log for manual att report by Nayem-----------
                SqlCommand cmd = new SqlCommand("delete tblAttendanceRecordPunchLog where EmpId='" + EmpId + "' and AttDate='" + commonTask.ddMMyyyyToyyyyMMdd(selectedDate).ToString() + "' ", sqlDB.connection);
                cmd.ExecuteNonQuery();
                string[] getColumns1 = { "EmpId", "AttDate", "PInHour", "PInMin", "PInSec", "POutHour", "POutMin", "POutSec" };

                string[] getValues1 = { EmpId, commonTask.ddMMyyyyToyyyyMMdd(selectedDate).ToString()
                                                 ,InHour,InMin,InSec,OutHour,OutMin,OutSec};
                SQLOperation.forSaveValue("tblAttendanceRecordPunchLog", getColumns1, getValues1, sqlDB.connection);
                //----------------------------------------------------------------------------------------------
            }
            catch { }
        }
        public static void SaveAttendance_Status(string EmpId, string selectedDate, string EmpTypeId, string InHour, string InMin, string InSec, string OutHour, string OutMin, string OutSec,
                                          string BreakStartTime,string BreakEndTime,string AttStatus, string StateStatus,string DailyStartTimeALT_CloseTime, string OverTime, string SftId, string DptId, string DsgId, string CompanyId,
                                          string GId, string LateTime, string StayTime, string TiffinCount, string HolidayCount,string PaybleDays,string OtherOverTime,string TotalOverTime,string  UserId, string IsHalfDayLeave, string IsHalfDayDeduct)
        {
            try
            {

                LateTime = (LateTime == null) ? "00:00:00" : LateTime;
                DateTime dtTimeConvert;
                if (DateTime.TryParse(LateTime, out dtTimeConvert)) LateTime = dtTimeConvert.ToString("HH:mm:ss");
                dtTimeConvert = new DateTime();

                StayTime = (StayTime == null) ? "00:00:00" : StayTime;

                string BreakStay_Time= (TimeSpan.Parse(BreakEndTime) - TimeSpan.Parse(BreakStartTime)).ToString();
                
                StayTime = (BreakStartTime.Contains("-"))?StayTime:(TimeSpan.Parse(StayTime) - TimeSpan.Parse(BreakEndTime)).ToString();

                if (DateTime.TryParse(StayTime, out dtTimeConvert)) StayTime = dtTimeConvert.ToString("HH:mm:ss");

                if (DateTime.TryParse(BreakStartTime, out dtTimeConvert)) BreakStartTime = dtTimeConvert.ToString("HH:mm:ss");
                if (DateTime.TryParse(BreakEndTime, out dtTimeConvert)) BreakEndTime = dtTimeConvert.ToString("HH:mm:ss");



                string[] getColumns = { "EmpId", "AttDate", "EmpTypeId", "InHour", "InMin", "InSec", "OutHour", "OutMin", "OutSec",
                                        "BreakStartTime","BreakEndTime","AttStatus", "StateStatus",
                                        "DailyStartTimeALT_CloseTime", "OverTime", "SftId", "DptId","DsgId", "CompanyId", "GId","LateTime","StayTime","TiffinCount","HolidayCount","PaybleDays","OtherOverTime","TotalOverTime","UserId" };

                string[] getValues = { EmpId, commonTask.ddMMyyyyToyyyyMMdd(selectedDate).ToString(),
                                                 EmpTypeId,InHour,InMin,InSec,OutHour,OutMin,OutSec,BreakStartTime,BreakEndTime,AttStatus,
                                                 StateStatus,DailyStartTimeALT_CloseTime,OverTime,SftId,DptId,DsgId,CompanyId,GId,LateTime,StayTime,TiffinCount,HolidayCount,PaybleDays,OtherOverTime,TotalOverTime, UserId};
                SQLOperation.forSaveValue("tblAttendanceRecord", getColumns, getValues, sqlDB.connection);
           
            }
            catch { }
        }
        private void ImportAttendance(string CompanyId, string attDate, bool ForAllStudents, FileUpload fileupload,string EmpCardNo,Label lblErrorMsg ) // This function is use for import att punch form ms access file.
        {
            string filename = "att2000.mdb";
            if (fileupload.HasFile == true)
            {
                filename = Path.GetFileName(fileupload.FileName);
                File.Delete(HttpContext.Current.Server.MapPath("~/AccessFile/") + CompanyId + filename);
                fileupload.SaveAs(HttpContext.Current.Server.MapPath("~/AccessFile/") + CompanyId + filename);
            }
            


            
            
            OleDbConnection cont = new OleDbConnection();
            string getFilePaht = HttpContext.Current.Server.MapPath("//AccessFile//" + CompanyId + filename);
            string connection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + getFilePaht + "";
            cont.ConnectionString = connection;
            cont.Open();
            OleDbDataAdapter da;
            string sqlcmd1 = "";
            string sqlcmd2 = "";
            if (ForAllStudents)
            {
                sqlcmd1 = "select USERINFO.UserID,Format(CheckTime,'yyyy-MM-dd HH:mm:ss') as CHECKTIME,CHECKTYPE,USERINFO.VERIFYCODE,SENSORID,Memoinfo,WorkCode,sn,UserExtFmt,USERINFO.BADGENUMBER from CHECKINOUT inner join USERINFO  on CHECKINOUT.USERID=USERINFO.USERID where   Format(CHECKTIME,'yyyy-MM-dd')='" + attDate + "'";
                sqlcmd2 = "delete CHECKINOUTOnline where Format(CHECKTIME,'yyyy-MM-dd')='" + attDate + "'";
            }
            else
            {
                sqlcmd1 = "select USERINFO.UserID,Format(CheckTime,'yyyy-MM-dd HH:mm:ss') as CHECKTIME,CHECKTYPE,USERINFO.VERIFYCODE,SENSORID,Memoinfo,WorkCode,sn,UserExtFmt,USERINFO.BADGENUMBER from CHECKINOUT inner join USERINFO  on CHECKINOUT.USERID=USERINFO.USERID where  Badgenumber='" + EmpCardNo + "' and Format(CHECKTIME,'yyyy-MM-dd')='" + attDate + "'";
                sqlcmd2 = "delete CHECKINOUTOnline where  Badgenumber='" + EmpCardNo + "' and Format(CHECKTIME,'yyyy-MM-dd')='" + attDate + "'";
            }
            da = new OleDbDataAdapter(sqlcmd1, cont);  // here selecteddate format =yyyyMMdd            
            DataTable dtPunch = new DataTable();
            da.Fill(dtPunch);
            cont.Close();
            SqlCommand cmd1;
            cmd1 = new SqlCommand(sqlcmd2,sqlDB.connection);
            cmd1.ExecuteNonQuery();
            SqlCommand cmd;
            //----------------------------------------------- entered punch data into CHECKINOUTOnline table------------------------------------------------
            lblErrorMsg.Text += ",199,>" + dtPunch.Rows;
            foreach (DataRow dr in dtPunch.Rows)
            {
                try { 

                cmd = new SqlCommand("insert into CHECKINOUTOnline(UserID,CHECKTIME,CHECKTYPE,VERIFYCODE,SENSORID,Memoinfo,WorkCode,sn,UserExtFmt,BADGENUMBER) " +
                    " values " +
                    "(" + dr["UserID"].ToString() + ",'" + dr["CHECKTIME"].ToString() + "','" + dr["CHECKTYPE"].ToString() + "'," + dr["VERIFYCODE"].ToString()
                    + ",'" + dr["SENSORID"].ToString() + "','" + dr["Memoinfo"].ToString() + "','" + dr["WorkCode"].ToString() + "','" + dr["sn"].ToString() + "'," + dr["UserExtFmt"].ToString() + ",'" + dr["BADGENUMBER"].ToString() + "')", sqlDB.connection);
                cmd.ExecuteNonQuery();
                }
                catch { }
            }
            lblErrorMsg.Text += ",210,>";
        }
        public void Store_In_Attendance_Log(string CompanyId, DateTime SelectedDate, bool ForAllEmployee, string DepartmentId, string EmpCardNo, string UserId, FileUpload FileUploader,Label lblErrorMsg)
        {
            try
            {
                HttpContext.Current.Session["__lnAdminList__"] = commonTask.getAdminListForNotification("ln");
                lblErrorMsg.Text += ",214";
                string tableName = "v_CHECKINOUT";
                //-----------Import File------------ 
                if (File.Exists(HttpContext.Current.Server.MapPath("~/IsOnline.txt")))
                {
                    lblErrorMsg.Text += ",219," + SelectedDate.ToString();
                    string sd = SelectedDate.ToString("yyyy-MM-dd");
                    lblErrorMsg.Text += ",221,"+sd;
                    ImportAttendance(CompanyId, sd, ForAllEmployee, FileUploader, EmpCardNo, lblErrorMsg);
                    lblErrorMsg.Text += ",222";

                    tableName = "CHECKINOUTOnline";
                }
                //----------------------------------
                lblErrorMsg.Text += ",227";
                DataTable dtEmpInfo =mCommon_Module_For_AttendanceProcessing.loadRunningEmployee(SelectedDate.ToString("yyyy-MM-dd"), ForAllEmployee,CompanyId, DepartmentId, EmpCardNo);  // for load all running employee
                DataTable dt = new DataTable();

               

                //--------------------------------------------- To konwing selected date is weekend or holyday ?---------------------------------------------
              string[] DayStatus =mZK_Shrink_Data_SqlServer.Check_Todays_Is_HolidayOrWeekend(SelectedDate.ToString("yyyy-MM-dd"));
                string TempDayStatus = DayStatus[1];
                DataTable dtOtherSettings = mZK_Shrink_Data_SqlServer.LoadOTherSettings(CompanyId);
               
                string[] othersetting=new string[8];
                if(dtOtherSettings.Rows.Count>0)
                {
                    othersetting[0]=dtOtherSettings.Rows[0]["WorkerTiffinHour"].ToString();
                    othersetting[1]=dtOtherSettings.Rows[0]["WorkerTiffinMin"].ToString();
                    othersetting[2]=dtOtherSettings.Rows[0]["StaffTiffinHour"].ToString();
                    othersetting[3]=dtOtherSettings.Rows[0]["StaffTiffinMin"].ToString();
                    othersetting[4] = dtOtherSettings.Rows[0]["StaffHolidayCount"].ToString();
                    othersetting[5] = dtOtherSettings.Rows[0]["MinWorkingHour"].ToString() + ":" + dtOtherSettings.Rows[0]["MinWorkingMin"].ToString() + ":00"; //Minimum Working Hours
                    othersetting[6] = dtOtherSettings.Rows[0]["StaffHolidayTotalHour"].ToString() + ":" + dtOtherSettings.Rows[0]["StaffHolidayTotalMin"].ToString() + ":00"; //Minimum Staff Working Hours For Holiday Allowance
                    othersetting[7] = dtOtherSettings.Rows[0]["MinOverTimeHour"].ToString() + ":" + dtOtherSettings.Rows[0]["MinOverTimeMin"].ToString() + ":00"; //Minimum OverTime
                }
                bool isgarments = classes.Payroll.Office_IsGarments();

                if (bool.Parse(DayStatus[0]))  // checking date is holiday or weekend ?. If date is weekend or holi day then execute this block
                {
                    for (int i = 0; i < dtEmpInfo.Rows.Count; i++)
                    {
                        DateTime joindate = DateTime.ParseExact(dtEmpInfo.Rows[i]["EmpJoiningDate"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        if (joindate > SelectedDate)
                        {
                            continue;
                        }
                        else
                        {
                            classes.mCommon_Module_For_AttendanceProcessing.delete_Attendance(CompanyId, DepartmentId, SelectedDate.ToString("yyyy-MM-dd"), false, dtEmpInfo.Rows[i]["EmpId"].ToString()); // delete existing attendance record by att date
                            DataTable dtshortleave = mZK_Shrink_Data_SqlServer.LoadShorLeave(dtEmpInfo.Rows[i]["EmpId"].ToString(), SelectedDate.ToString("yyyy-MM-dd"));
                            dt = new DataTable();
                            string[] WHO_DayStatus = { "00:00:00", "00:00:00", "00:00:00", "0", "0", "00:00:00", "00:00:00" };
                            string worker = othersetting[0] + ":" + othersetting[1] + ":00";
                            string staff = othersetting[2] + ":" + othersetting[3] + ":00";
                            string tiffin = dtEmpInfo.Rows[i]["EmpTypeId"].ToString() == "1" ? worker : staff;
                            // if day is holiday or weekend and worker are punched then calculate overtime and find roster ..........................                         
                            string[] Roster_Info;                        
                       
                        if (dtEmpInfo.Rows[i]["EmpDutyType"].ToString().Equals("Roster"))
                        {

                            Roster_Info = GetRosterId(SelectedDate.ToString("yyyy-MM-dd"), dtEmpInfo.Rows[i]["EmpId"].ToString(), false, "");
                            if (Roster_Info == null) mCommon_Module_For_AttendanceProcessing.NotCountableAttendanceLog(dtEmpInfo.Rows[i]["EmpId"].ToString(), "Rostering Problem", SelectedDate.ToString("MMM-dd-yyyy")); // This employee does not asigned in any roster.
                            else
                            {
                                if (!bool.Parse(Roster_Info[8])) // if this date is not set weekend or holyday for this roster duty type emplyee .then its counted as weekend or holiday  
                                {
                                    TimeSpan ShiftPunchCountStartTime = TimeSpan.Parse(Roster_Info[7]);
                                    string[] Leave_Info = Check_Any_Leave_Are_Exist(SelectedDate.ToString("yyyy-MM-dd"), dtEmpInfo.Rows[i]["EmpId"].ToString());
                             //   sqlDB.fillDataTable("select distinct Badgenumber, FORMAT(CHECKTIME,'HH') as Hour,FORMAT(CHECKTIME,'mm') as Minute,FORMAT(CHECKTIME,'ss') as Second,FORMAT(CHECKTIME,'HH:mm:ss') as PunchTime from v_CHECKINOUT where CONVERT(date,CHECKTIME)='" + SelectedDate.ToString("yyyy-MM-dd") + "' AND Badgenumber Like '%" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "%'  order by CHECKTIME", dt);
                                sqlDB.fillDataTable("select distinct Badgenumber,FORMAT(CHECKTIME,'HH') as Hour,FORMAT(CHECKTIME,'mm') as Minute,FORMAT(CHECKTIME,'ss') as Second, FORMAT(CHECKTIME,'HH:mm:ss') as PunchTime from  "+tableName+" where CONVERT(date,CHECKTIME)='" + SelectedDate.ToString("yyyy-MM-dd") + "' and convert(time(7),CHECKTIME ) >='" + ShiftPunchCountStartTime + "' AND Badgenumber ='" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "'  order by PunchTime", dt);
                                if (dt.Rows.Count > 0)
                                {
                                    DataTable dtPunchList = new DataTable();

                                    DateTime tempDate = SelectedDate.AddDays(1);
                                    TimeSpan FirstPunch = TimeSpan.Parse(dt.Rows[0]["PunchTime"].ToString());
                                    //  byte TotalRows = (byte)dtPunchList.Rows.Count;
                                    TimeSpan LastPunch = TimeSpan.Parse("00:00:00");
                                    if (dt.Rows.Count > 1)
                                        LastPunch = TimeSpan.Parse(dt.Rows[dt.Rows.Count - 1]["PunchTime"].ToString());
                                    DataTable dttemp;
                                    if ((Roster_Info[1].Equals("PM") && Roster_Info[2].Equals("AM")))
                                    {

                                        //sqlDB.fillDataTable("select distinct ProximityNo,Hour,Minute,Second,Convert(varchar(2),Hour)+':'+CONVERT(varchar(2),Minute)+':'+Convert(varchar(2),Second) as PunchTime from tblAttendance where PunchDate='" + tempDate.ToString("yyyy-MM-dd") + "' AND ProximityNo='" + _ProxymityNo + "' and convert(time(7), Convert(varchar(2),Hour)+':'+CONVERT(varchar(2),Minute)+':'+Convert(varchar(2),Second))>='00:00:00' and Hour<=11 and Minute<=59 and Second<= 59 order by Hour,Minute,Second ", dttemp = new DataTable());
                                        sqlDB.fillDataTable("select distinct Badgenumber,FORMAT(CHECKTIME,'HH') as Hour,FORMAT(CHECKTIME,'mm') as Minute,FORMAT(CHECKTIME,'ss') as Second, FORMAT(CHECKTIME,'HH:mm:ss') as PunchTime from  "+tableName+"  where CONVERT(date,CHECKTIME)='" + tempDate.ToString("yyyy-MM-dd") + "' and convert(time(7),CHECKTIME ) >='00:00:00' and convert(time(7),CHECKTIME ) <='11:59:59' AND Badgenumber ='" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "'  order by PunchTime", dttemp = new DataTable());
                                        if (dttemp.Rows.Count > 0)
                                        {
                                            for (byte b = 0; b < dttemp.Rows.Count; b++)
                                            {
                                                dt.Rows.Add(dttemp.Rows[b]["Badgenumber"].ToString(), dttemp.Rows[b]["Hour"].ToString(), dttemp.Rows[b]["Minute"].ToString(),
                                                 dttemp.Rows[b]["Second"].ToString(), dttemp.Rows[b]["PunchTime"].ToString());
                                            }
                                            LastPunch = TimeSpan.Parse(dt.Rows[dt.Rows.Count - 1]["PunchTime"].ToString()); // to get out time of duty .so last punch are counted 


                                        }
                                    }

                                    WHO_DayStatus = OverTime_Calculation_ForWeekend_Holiday(FirstPunch, LastPunch, int.Parse(Roster_Info[5]), TimeSpan.Parse(Roster_Info[3]), TimeSpan.Parse(tiffin), TimeSpan.Parse(othersetting[7]), dtshortleave.Rows.Count);
                                    if (dtEmpInfo.Rows[i]["EmpTypeId"].ToString() == "2" && TimeSpan.Parse(WHO_DayStatus[2]) >= TimeSpan.Parse(othersetting[6]))
                                    {
                                        WHO_DayStatus[4] = "1";
                                    }
                                    else
                                    {
                                        WHO_DayStatus[4] = "0";
                                    }
                                    if (!isgarments)
                                    {
                                        WHO_DayStatus[3] = "0";
                                    }
                                }
                                if (Leave_Info[0].ToString() != "0")
                                {
                                    DayStatus[0] = "Lv";
                                    DayStatus[1] = Leave_Info[1];
                                    classes.LeaveLibrary.LeaveCount(SelectedDate.ToString("yyyy-MM-dd"), Leave_Info[0]);
                                }
                                else
                                {
                                    DayStatus[0] = TempDayStatus;
                                    DayStatus[1] = (TempDayStatus == "W") ? "Weekend" : "Holiday";

                                }
                                // send parameter for count attendance 
                                SaveAttendance_Status(dtEmpInfo.Rows[i]["EmpId"].ToString(), SelectedDate.ToString("dd-MM-yyyy"), dtEmpInfo.Rows[i]["EmpTypeId"].ToString(),
                                    (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Hour"].ToString().Length == 1) ? "0" + dt.Rows[0]["Hour"].ToString() : dt.Rows[0]["Hour"].ToString(),
                                    (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Minute"].ToString().Length == 1) ? "0" + dt.Rows[0]["Minute"].ToString() : dt.Rows[0]["Minute"].ToString(),
                                    (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Second"].ToString().Length == 1) ? "0" + dt.Rows[0]["Second"].ToString() : dt.Rows[0]["Second"].ToString(),
                                    (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Hour"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Hour"].ToString().Trim() && dt.Rows[0]["Minute"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Minute"].ToString().Trim()) ? "00" : (dt.Rows[dt.Rows.Count - 1]["Hour"].ToString().Length == 1) ? "0" + dt.Rows[dt.Rows.Count - 1]["Hour"].ToString() : dt.Rows[dt.Rows.Count - 1]["Hour"].ToString(),
                                    (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Hour"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Hour"].ToString().Trim() && dt.Rows[0]["Minute"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Minute"].ToString().Trim()) ? "00" : (dt.Rows[dt.Rows.Count - 1]["Minute"].ToString().Length == 1) ? "0" + dt.Rows[dt.Rows.Count - 1]["Minute"].ToString() : dt.Rows[dt.Rows.Count - 1]["Minute"].ToString(),
                                    (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Hour"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Hour"].ToString().Trim() && dt.Rows[0]["Minute"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Minute"].ToString().Trim()) ? "00" : (dt.Rows[dt.Rows.Count - 1]["Second"].ToString().Length == 1) ? "0" + dt.Rows[dt.Rows.Count - 1]["Second"].ToString() : dt.Rows[dt.Rows.Count - 1]["Second"].ToString(),
                                    DayStatus[0], DayStatus[1], WHO_DayStatus[0],
                                    Roster_Info[0].ToString(),
                                    dtEmpInfo.Rows[i]["DptId"].ToString(), dtEmpInfo.Rows[i]["DsgId"].ToString(),
                                    CompanyId, dtEmpInfo.Rows[i]["GId"].ToString(), WHO_DayStatus[1], WHO_DayStatus[2], Roster_Info[3] + ":" + Roster_Info[6] + ":" + Roster_Info[4], WHO_DayStatus[3], WHO_DayStatus[4], "0", WHO_DayStatus[5], WHO_DayStatus[6], UserId,"0", "0");


                            }
                            else
                            {
                                zkRosterOperation_Shrink_Data.RoserOperationProcessing(SelectedDate, dtEmpInfo.Rows[i]["EmpId"].ToString(), byte.Parse(dtEmpInfo.Rows[i]["EmpTypeId"].ToString()), dtEmpInfo.Rows[i]["EmpCardNo"].ToString(),
                                    bool.Parse(Roster_Info[8]), Roster_Info[1], Roster_Info[2], true, TimeSpan.Parse(Roster_Info[3]), TimeSpan.Parse(Roster_Info[4]), TimeSpan.Parse(Roster_Info[7]), Roster_Info[6], int.Parse(Roster_Info[5]), Roster_Info[0], dtEmpInfo.Rows[i]["DptId"].ToString(), dtEmpInfo.Rows[i]["DsgId"].ToString(), CompanyId, dtEmpInfo.Rows[i]["GId"].ToString(), Roster_Info[9], Roster_Info[10], bool.Parse(Roster_Info[11]), TimeSpan.Parse(tiffin), bool.Parse(othersetting[4]), isgarments,UserId,tableName);
                            }

                            }
                        }
                        else   // if employee type is regular then successfully executed this block
                        {
                            Roster_Info = GetRosterId(SelectedDate.ToString("yyyy-MM-dd"), dtEmpInfo.Rows[i]["EmpId"].ToString(), true, dtEmpInfo.Rows[i]["SftId"].ToString());
                            string[] Leave_Info = Check_Any_Leave_Are_Exist(SelectedDate.ToString("yyyy-MM-dd"), dtEmpInfo.Rows[i]["EmpId"].ToString());
                            TimeSpan ShiftPunchCountStartTime = TimeSpan.Parse(Roster_Info[7]);                         
                            sqlDB.fillDataTable("select distinct Badgenumber,FORMAT(CHECKTIME,'HH') as Hour,FORMAT(CHECKTIME,'mm') as Minute,FORMAT(CHECKTIME,'ss') as Second, FORMAT(CHECKTIME,'HH:mm:ss') as PunchTime from  "+tableName+"  where CONVERT(date,CHECKTIME)='" + SelectedDate.ToString("yyyy-MM-dd") + "' and convert(time(7),CHECKTIME ) >='" + ShiftPunchCountStartTime + "' and Badgenumber ='" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "'  order by PunchTime", dt);
                            if (dt.Rows.Count > 0)
                            {
                                WHO_DayStatus = OverTime_Calculation_ForWeekend_Holiday(TimeSpan.Parse(dt.Rows[0]["PunchTime"].ToString()), TimeSpan.Parse(dt.Rows[dt.Rows.Count - 1]["PunchTime"].ToString()), int.Parse(Roster_Info[5]), TimeSpan.Parse(Roster_Info[3]), TimeSpan.Parse(tiffin), TimeSpan.Parse(othersetting[7]), dtshortleave.Rows.Count);
                                if (dtEmpInfo.Rows[i]["EmpTypeId"].ToString() == "2" && TimeSpan.Parse(WHO_DayStatus[2]) >= TimeSpan.Parse(othersetting[6]))
                                {
                                    WHO_DayStatus[4] = "1";
                                }
                                else
                                {
                                    WHO_DayStatus[4] = "0";
                                }
                                if (!isgarments)
                                {
                                    WHO_DayStatus[3] = "0";
                                }
                            }
                            if (Leave_Info[0].ToString() != "0")
                            {
                                DayStatus[0] = "Lv";
                                DayStatus[1] = Leave_Info[1];
                                classes.LeaveLibrary.LeaveCount(SelectedDate.ToString("yyyy-MM-dd"), Leave_Info[0]);
                            }
                            else
                            {
                                DayStatus[0] = TempDayStatus;
                                DayStatus[1] = (TempDayStatus == "W") ? "Weekend" : "Holiday";

                            }
                        }
                        //------------------------------ End ------------------------------------------------------------------------------------

                        // send parameter for count attendance 
                        SaveAttendance_Status(dtEmpInfo.Rows[i]["EmpId"].ToString(), SelectedDate.ToString("dd-MM-yyyy"), dtEmpInfo.Rows[i]["EmpTypeId"].ToString(),
                            (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Hour"].ToString().Length == 1) ? "0" + dt.Rows[0]["Hour"].ToString() : dt.Rows[0]["Hour"].ToString(),
                            (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Minute"].ToString().Length == 1) ? "0" + dt.Rows[0]["Minute"].ToString() : dt.Rows[0]["Minute"].ToString(),
                            (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Second"].ToString().Length == 1) ? "0" + dt.Rows[0]["Second"].ToString() : dt.Rows[0]["Second"].ToString(),
                            (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Hour"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Hour"].ToString().Trim() && dt.Rows[0]["Minute"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Minute"].ToString().Trim()) ? "00" : (dt.Rows[dt.Rows.Count - 1]["Hour"].ToString().Length == 1) ? "0" + dt.Rows[dt.Rows.Count - 1]["Hour"].ToString() : dt.Rows[dt.Rows.Count - 1]["Hour"].ToString(),
                            (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Hour"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Hour"].ToString().Trim() && dt.Rows[0]["Minute"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Minute"].ToString().Trim()) ? "00" : (dt.Rows[dt.Rows.Count - 1]["Minute"].ToString().Length == 1) ? "0" + dt.Rows[dt.Rows.Count - 1]["Minute"].ToString() : dt.Rows[dt.Rows.Count - 1]["Minute"].ToString(),
                            (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Hour"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Hour"].ToString().Trim() && dt.Rows[0]["Minute"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Minute"].ToString().Trim()) ? "00" : (dt.Rows[dt.Rows.Count - 1]["Second"].ToString().Length == 1) ? "0" + dt.Rows[dt.Rows.Count - 1]["Second"].ToString() : dt.Rows[dt.Rows.Count - 1]["Second"].ToString(),
                            DayStatus[0], DayStatus[1], WHO_DayStatus[0],
                            Roster_Info[0].ToString(),
                            dtEmpInfo.Rows[i]["DptId"].ToString(), dtEmpInfo.Rows[i]["DsgId"].ToString(),
                            CompanyId, dtEmpInfo.Rows[i]["GId"].ToString(), WHO_DayStatus[1], WHO_DayStatus[2], Roster_Info[3] + ":" + Roster_Info[6] + ":" + Roster_Info[4], WHO_DayStatus[3], WHO_DayStatus[4], "0", WHO_DayStatus[5], WHO_DayStatus[6], UserId,"0", "0");

                         
                        } 

                }
                }//------------------------------------------------ End all weekend or holiday transaction--------------------------------------------
                else  // date is not holiday or weekend.that's mean working date then 
                {
                    lblErrorMsg.Text += ",418";
                    for (int i = 0; i < dtEmpInfo.Rows.Count; i++)
                    {
                        string ODID= CheckOutDuty(dtEmpInfo.Rows[i]["EmpID"].ToString(), SelectedDate.ToString("yyyy-MM-dd")); // Check out duty 
                      //  DateTime joindate = DateTime.ParseExact(dtEmpInfo.Rows[i]["EmpJoiningDate"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        DateTime joindate = DateTime.Parse(commonTask.ddMMyyyyToyyyyMMdd( dtEmpInfo.Rows[i]["EmpJoiningDate"].ToString()));
                        if (joindate > SelectedDate)
                        {
                            continue;
                        }
                        else
                        {
                            classes.mCommon_Module_For_AttendanceProcessing.delete_Attendance(CompanyId, DepartmentId, SelectedDate.ToString("yyyy-MM-dd"), false, dtEmpInfo.Rows[i]["EmpId"].ToString()); // delete existing attendance record by att date
                            DataTable dtshortleave = mZK_Shrink_Data_SqlServer.LoadShorLeave(dtEmpInfo.Rows[i]["EmpId"].ToString(), SelectedDate.ToString("yyyy-MM-dd"));
                            DayStatus = new string[10];
                        string[] Roster_Info;
                      
                        if (dtEmpInfo.Rows[i]["EmpDutyType"].ToString().Equals("Roster"))   // if employee duty type is Roster then executed this block
                        {
                         
                            string worker = othersetting[0] + ":" + othersetting[1] + ":00";
                            string staff = othersetting[2] + ":" + othersetting[3] + ":00";
                            string tiffin = dtEmpInfo.Rows[i]["EmpTypeId"].ToString() == "1" ? worker : staff;
                            Roster_Info = GetRosterId(SelectedDate.ToString("yyyy-MM-dd"), dtEmpInfo.Rows[i]["EmpId"].ToString(), false, "");

                            if (Roster_Info == null) mCommon_Module_For_AttendanceProcessing.NotCountableAttendanceLog(dtEmpInfo.Rows[i]["EmpId"].ToString(), "Rostering Problem", SelectedDate.ToString("MMM-dd-yyyy"));
                            else
                                zkRosterOperation_Shrink_Data.RoserOperationProcessing(SelectedDate, dtEmpInfo.Rows[i]["EmpId"].ToString(), byte.Parse(dtEmpInfo.Rows[i]["EmpTypeId"].ToString()), dtEmpInfo.Rows[i]["EmpCardNo"].ToString(),
                                    bool.Parse(Roster_Info[8]), Roster_Info[1], Roster_Info[2], true, TimeSpan.Parse(Roster_Info[3]), TimeSpan.Parse(Roster_Info[4]), TimeSpan.Parse(Roster_Info[7]), Roster_Info[6], int.Parse(Roster_Info[5]), Roster_Info[0], dtEmpInfo.Rows[i]["DptId"].ToString(), dtEmpInfo.Rows[i]["DsgId"].ToString(), CompanyId, dtEmpInfo.Rows[i]["GId"].ToString(), Roster_Info[9], Roster_Info[10], bool.Parse(Roster_Info[11]), TimeSpan.Parse(tiffin), bool.Parse(othersetting[4]), isgarments,UserId,tableName);
                
                        }
                        else   // for regular duty type employee
                        {
                            Roster_Info = GetRosterId(SelectedDate.ToString("yyyy-MM-dd"), dtEmpInfo.Rows[i]["EmpId"].ToString(), true, dtEmpInfo.Rows[i]["SftId"].ToString());
                            TimeSpan ShiftPunchCountStartTime = TimeSpan.Parse(Roster_Info[7]);
                         // for leave------------------------------------------------------------------------------------------------------------------
                             string[] Leave_Info = Check_Any_Leave_Are_Exist(SelectedDate.ToString("yyyy-MM-dd"), dtEmpInfo.Rows[i]["EmpId"].ToString());
                             string IsHalfDayLeave = "0";
                            if (Leave_Info[0].ToString() != "0" && Leave_Info[2].ToString().Equals("False"))  // check any type of leave. if are leave exists then execute this if block
                                {
                                        DayStatus[0] = "Lv";
                                        DayStatus[1] = Leave_Info[1];
                                        DayStatus[2] = "00:00:00";// OT
                                        DayStatus[3] = "00:00:00"; // Late Time
                                        DayStatus[4] = "00:00:00"; // Stay Time
                                        DayStatus[5] = "0"; //Tiffin Count
                                        DayStatus[6] = "0"; //PaybleDays
                                        DayStatus[7] = "00:00:00"; //OtherOverTime
                                        DayStatus[8] = "00:00:00"; //TotaOverTime

                                        classes.LeaveLibrary.LeaveCount(SelectedDate.ToString("yyyy-MM-dd"), Leave_Info[0]);
                                        dt = new DataTable();
                                   
                                   
                                } //End-----------------------------------------------------------------------------------------------------------------------
                            else // without leave------------------------------------------------------------------------------
                            {
                                dt = new DataTable();
                               // sqlDB.fillDataTable("select distinct Badgenumber,FORMAT(CHECKTIME,'HH') as Hour,FORMAT(CHECKTIME,'mm') as Minute,FORMAT(CHECKTIME,'ss') as Second, FORMAT(CHECKTIME,'HH:mm:ss') as PunchTime from  "+tableName+"  where CONVERT(date,CHECKTIME)='" + SelectedDate.ToString("yyyy-MM-dd") + "' AND Badgenumber Like '%" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "'  order by PunchTime", dt);
                                sqlDB.fillDataTable("select distinct Badgenumber,FORMAT(CHECKTIME,'HH') as Hour,FORMAT(CHECKTIME,'mm') as Minute,FORMAT(CHECKTIME,'ss') as Second, FORMAT(CHECKTIME,'HH:mm:ss') as PunchTime from  "+tableName+"  where CONVERT(date,CHECKTIME)='" + SelectedDate.ToString("yyyy-MM-dd") + "' and convert(time(7),CHECKTIME ) >='" + ShiftPunchCountStartTime + "' and Badgenumber ='" + dtEmpInfo.Rows[i]["EmpCardNo"].ToString() + "'  order by PunchTime", dt);
                                if (dt.Rows.Count == 0)  // any punched are not exists of selectd day
                                {
                                    DayStatus[0] = "A";
                                    DayStatus[1] = "Absent";
                                    DayStatus[2] = "00:00:00";
                                    DayStatus[3] = "00:00:00"; // Late Time
                                    DayStatus[4] = "00:00:00"; // Stay Time
                                    DayStatus[5] = "0"; //Tiffin Count
                                    DayStatus[6] = "0"; //PaybleDays
                                    DayStatus[7] = "00:00:00"; //OtherOverTime
                                    DayStatus[8] = "00:00:00"; //TotaOverTime
                                    DayStatus[9] = "0"; //TotaOverTime
                                        bool StraightFromHome = false;
                                        bool StraightToHome = false;
                                        bool IsTraining = false; 
                                        if (ODID != "")
                                        {
                                            DataTable dtOutduty = new DataTable();
                                            dtOutduty = getOutDutyInfo(ODID);
                                            if (dtOutduty != null && dtOutduty.Rows.Count > 0)
                                            {
                                                StraightFromHome = bool.Parse(dtOutduty.Rows[0]["StraightFromHome"].ToString());
                                                StraightToHome = bool.Parse(dtOutduty.Rows[0]["StraightToHome"].ToString());
                                                IsTraining = bool.Parse(dtOutduty.Rows[0]["type"].ToString());
                                                
                                            }
                                            if ((StraightFromHome && StraightToHome)||IsTraining)
                                            {
                                                DayStatus[0] = "P";//Att Status
                                                DayStatus[1] = "Present";//State Status
                                                DayStatus[6] = "1";//PaybleDays
                                            }
                                            
                                        }
                                }
                                else
                                {

                                       
                                    string worker = othersetting[0] + ":" + othersetting[1] + ":00";
                                    string staff = othersetting[2] + ":" + othersetting[3] + ":00";
                                    string tiffin = dtEmpInfo.Rows[i]["EmpTypeId"].ToString() == "1" ? worker : staff;
                                    string[] PresentDays_Status = OverTime_Calculation_ForRegularDuty(ODID, TimeSpan.Parse(dt.Rows[0]["PunchTime"].ToString()), TimeSpan.Parse(dt.Rows[dt.Rows.Count - 1]["PunchTime"].ToString()), TimeSpan.Parse(Roster_Info[3]), TimeSpan.Parse(Roster_Info[4]), byte.Parse(Roster_Info[6]), byte.Parse(Roster_Info[5]), TimeSpan.Parse(tiffin), TimeSpan.Parse(othersetting[5]), TimeSpan.Parse(othersetting[7]), dtshortleave.Rows.Count, isgarments, Roster_Info[12], Roster_Info[13]);
                                    DayStatus[0] = PresentDays_Status[0];//Att Status
                                    DayStatus[1] = PresentDays_Status[1];//State Status
                                    DayStatus[2] = PresentDays_Status[2]; //OT
                                    DayStatus[3] = PresentDays_Status[3]; // Late Time
                                    DayStatus[4] = PresentDays_Status[4]; // Stay Time
                                    DayStatus[5] = PresentDays_Status[5];//Tiffin Count
                                    DayStatus[6] = PresentDays_Status[6]; //PaybleDays
                                    DayStatus[7] = PresentDays_Status[7]; //OtherOverTime
                                    DayStatus[8] = PresentDays_Status[8]; //TotaOverTime
                                    DayStatus[9] = PresentDays_Status[9]; //It's half day deduct
                                    if (Leave_Info[0].ToString() != "0")// this block for half day leave
                                    {
                                            DayStatus[0] = "P";//Att Status
                                            DayStatus[1] = "Present";//State Status
                                            DayStatus[6] = "1";//PaybleDays
                                            IsHalfDayLeave = "1";
                                    }
                                        //if (ODID != "") // this block for out duty
                                        //{
                                        //    DayStatus[0] = "P";//Att Status
                                        //    DayStatus[1] = "Present";//State Status
                                        //    DayStatus[6] = "1";//PaybleDays
                                        //}

                                    }
                            }
                            string inHour = (dt.Rows.Count == 0) ? "00" : dt.Rows[0]["Hour"].ToString();
                            inHour = (inHour.Length == 1) ? "0" + dt.Rows[0]["Hour"].ToString() : inHour;
                            string inMinute = (dt.Rows.Count == 0) ? "00" : dt.Rows[0]["Minute"].ToString();
                            inMinute = (inMinute.Length == 0) ? "0" + dt.Rows[0]["Minute"].ToString() : inMinute;
                            string inSecond = (dt.Rows.Count == 0) ? "00" : dt.Rows[0]["Second"].ToString();
                            inSecond = (inSecond.Length == 0) ? "0" + dt.Rows[0]["Second"].ToString() : inSecond;
                            string outHour = (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Hour"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Hour"].ToString().Trim() && dt.Rows[0]["Minute"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Minute"].ToString().Trim()) ? "00" : dt.Rows[dt.Rows.Count - 1]["Hour"].ToString();
                            outHour = (outHour.Length == 1) ? "0" + dt.Rows[dt.Rows.Count - 1]["Hour"].ToString() : outHour;
                            string outMinute = (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Hour"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Hour"].ToString().Trim() && dt.Rows[0]["Minute"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Minute"].ToString().Trim()) ? "00" : dt.Rows[dt.Rows.Count - 1]["Minute"].ToString();
                            outMinute = (outMinute.Length == 1) ? "0" + dt.Rows[dt.Rows.Count - 1]["Minute"].ToString() : outMinute;
                            string outSecond = (dt.Rows.Count == 0) ? "00" : (dt.Rows[0]["Hour"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Hour"].ToString().Trim() && dt.Rows[0]["Minute"].ToString().Trim() == dt.Rows[dt.Rows.Count - 1]["Minute"].ToString().Trim()) ? "00" : dt.Rows[dt.Rows.Count - 1]["Second"].ToString();
                            outSecond = (outSecond.Length == 1) ? "0" + dt.Rows[dt.Rows.Count - 1]["Second"].ToString() : outSecond;
                            // send parameter for count attendance 
                            SaveAttendance_Status(dtEmpInfo.Rows[i]["EmpId"].ToString(), SelectedDate.ToString("dd-MM-yyyy"), dtEmpInfo.Rows[i]["EmpTypeId"].ToString(), inHour, inMinute, inSecond,
                                outHour, outMinute, outSecond, DayStatus[0], DayStatus[1].ToString(), DayStatus[2].ToString(), Roster_Info[0].ToString(), dtEmpInfo.Rows[i]["DptId"].ToString(),
                                dtEmpInfo.Rows[i]["DsgId"].ToString(), CompanyId, dtEmpInfo.Rows[i]["GId"].ToString(), DayStatus[3], DayStatus[4], Roster_Info[3] + ":" + Roster_Info[6] + ":" + Roster_Info[4], DayStatus[5], "0", DayStatus[6], DayStatus[7], DayStatus[8], UserId, IsHalfDayLeave,DayStatus[9],ODID);

                             }

                        }
                    }
                    lblErrorMsg.Text += ",522";

                }
                //---------------------------------------------------End--------------------------------------------------------------------------------------
            }
            catch (Exception ex) { lblErrorMsg.Text += ex.Message; }
        }

        public static string[] Check_Any_Leave_Are_Exist(string SelectedDate, string EmpId)
        {
            try
            {
                DataTable dt = new DataTable();
                string[] Leave_Info = new string[3];
                sqlDB.fillDataTable("select LACode,LeaveName, ISNULL(IsHalfDayLeave, 0) as IsHalfDayLeave from v_Leave_LeaveApplicationDetails where IsApproved=1 and LeaveDate='" + SelectedDate + "' AND EmpId='" + EmpId + "'", dt);
                if (dt.Rows.Count > 0)
                {
                    Leave_Info[0] = dt.Rows[0]["LACode"].ToString();
                    Leave_Info[1] = dt.Rows[0]["LeaveName"].ToString();
                    Leave_Info[2] = dt.Rows[0]["IsHalfDayLeave"].ToString();
                    
                }
                else Leave_Info[0] = "0";
                return Leave_Info;
            }
            catch { return null; }
        }

        private static string[] GetRosterId(string SelectedDate, string EmpId, bool IsRegularDuty_Type, string ShiftId)
        {
            try
            {
                DataTable dt = new DataTable();
                string[] Gt_RosterInfo = new string[14];
                if (IsRegularDuty_Type)
                {
                    sqlDB.fillDataTable("select SftOverTime,"+ShiftId+" as SftId,SftStartTimeIndicator,SftEndTimeIndicator,SftStartTime,SftEndTime,SftAcceptableLate,AcceptableTimeAsOT,StartPunchCountTime,Format(Cast(BreakStartTime as datetime),'HH:mm:ss') as BreakStartTime,Format(Cast(BreakEndTime as datetime),'HH:mm:ss') as BreakEndTime,OverLateTime,AcceptableHalfDay from HRD_SpecialTimetable where StartDate <= '" + SelectedDate + "' and EndDate >= '" + SelectedDate + "'", dt=new DataTable());
                    if(dt==null|| dt.Rows.Count==0)
                    sqlDB.fillDataTable("select SftOverTime,SftId,SftStartTimeIndicator,SftEndTimeIndicator,SftStartTime,SftEndTime,SftAcceptableLate,AcceptableTimeAsOT,StartPunchCountTime,Format(Cast(BreakStartTime as datetime),'HH:mm:ss') as BreakStartTime,Format(Cast(BreakEndTime as datetime),'HH:mm:ss') as BreakEndTime,OverLateTime,AcceptableHalfDay  from HRD_Shift where SftId ='" + ShiftId + "'", dt = new DataTable());
                }
                else
                    sqlDB.fillDataTable("select SftOverTime,SftId,SftStartTimeIndicator,SftEndTimeIndicator,SftStartTime,SftEndTime,SftAcceptableLate,AcceptableTimeAsOT,StartPunchCountTime,IsWeekend,Format(Cast(BreakStartTime as datetime),'HH:mm:ss') as BreakStartTime,Format(Cast(BreakEndTime as datetime),'HH:mm:ss') as BreakEndTime ,OverLateTime,AcceptableHalfDay from v_ShiftTransferInfoDetails where SDate ='" + SelectedDate + "' AND EmpId='" + EmpId + "'", dt);

                Gt_RosterInfo[0] = (dt.Rows.Count > 0) ? dt.Rows[0]["SftId"].ToString() : "0";
                Gt_RosterInfo[1] = dt.Rows[0]["SftStartTimeIndicator"].ToString();
                Gt_RosterInfo[2] = dt.Rows[0]["SftEndTimeIndicator"].ToString();
                Gt_RosterInfo[3] = dt.Rows[0]["SftStartTime"].ToString();
                Gt_RosterInfo[4] = dt.Rows[0]["SftEndTime"].ToString();
                Gt_RosterInfo[5] = dt.Rows[0]["AcceptableTimeAsOT"].ToString();
                Gt_RosterInfo[6] = dt.Rows[0]["SftAcceptableLate"].ToString();
                Gt_RosterInfo[7] = dt.Rows[0]["StartPunchCountTime"].ToString();
                Gt_RosterInfo[8] = (IsRegularDuty_Type) ? "False" : dt.Rows[0]["IsWeekend"].ToString();
                Gt_RosterInfo[9] = dt.Rows[0]["BreakStartTime"].ToString();
                Gt_RosterInfo[10] = dt.Rows[0]["BreakEndTime"].ToString();
                Gt_RosterInfo[11] = dt.Rows[0]["SftOverTime"].ToString();
                Gt_RosterInfo[12] = dt.Rows[0]["OverLateTime"].ToString();
                Gt_RosterInfo[13] = dt.Rows[0]["AcceptableHalfDay"].ToString();

                return Gt_RosterInfo;
            }
            catch { return null; }
        }
       
         public static string[] OverTime_Calculation_ForWeekend_Holiday(TimeSpan LogInTime, TimeSpan LogOutTime, int AcceptableOTMin, TimeSpan RosterStartTime, TimeSpan TiffinTime, TimeSpan MinOverTime, int shortleave)
        {
            try
            {

                string[] WHO_DayStatus = new string[7];
                string ExtraTime;
                string Get_OTHour;

                // int Get_OTMinute;

                // Get_OTHour = (Get_OTMinute > AcceptableOTMin) ? (byte)(int.Parse(Get_OTHour.ToString()) + 1) : Get_OTHour;

                DateTime time = DateTime.Today + LogInTime;
                String result = time.ToString("tt");
                TimeSpan k;
                if ((DateTime.Today + LogInTime).ToString("tt") == "PM" && (DateTime.Today + LogOutTime).ToString("tt") == "AM")
                {

                    TimeSpan z = LogOutTime - TimeSpan.Parse("00:00:00");
                    TimeSpan i = (TimeSpan.Parse("23:59:59") - LogInTime) + TimeSpan.Parse("00:00:01");
                    k = z + i;
                    if (k > MinOverTime)
                    {
                        Get_OTHour = MinOverTime.ToString();
                        ExtraTime = (k - MinOverTime).ToString();
                    }

                    else
                    {
                        Get_OTHour = k.ToString();
                        ExtraTime = "00:00:00";
                    }
                }
                else
                {
                    k = (LogOutTime - LogInTime);
                    if (k > MinOverTime)
                    {
                        Get_OTHour = MinOverTime.ToString();
                        ExtraTime = (k - MinOverTime).ToString();
                    }

                    else
                    {
                        Get_OTHour = k.ToString();
                        ExtraTime = "00:00:00";
                    }
                }

                string StayTime = "00:00:00";
                if (LogOutTime.ToString() == "00:00:00")
                {
                    StayTime = StayTime;
                }
                else
                    StayTime = k.ToString();

                // string StayTime = ((LogOutTime - LogInTime) < TimeSpan.Parse("00:00:00")) ? "00:00:00" : (LogOutTime - LogInTime).ToString();

                WHO_DayStatus[0] = Get_OTHour;
                WHO_DayStatus[1] = "00:00:00";
                WHO_DayStatus[2] = StayTime;
                WHO_DayStatus[5] = ExtraTime;
                WHO_DayStatus[6] = StayTime;
                //---------------------to get TiffineCount------------------
                if (StayTime == "00:00:00")
                {
                    WHO_DayStatus[0] = "00:00:00";
                    WHO_DayStatus[3] = "0";
                    WHO_DayStatus[5] = "00:00:00";
                    WHO_DayStatus[6] = "00:00:00";
                }
                else
                {
                    TimeSpan tiffinstaytime;
                    if (LogInTime < RosterStartTime)
                    {
                        time = DateTime.Today + RosterStartTime;
                        result = time.ToString("tt");
                        if ((DateTime.Today + RosterStartTime).ToString("tt") == "PM" && (DateTime.Today + LogOutTime).ToString("tt") == "AM")
                        {
                            TimeSpan z = LogOutTime - TimeSpan.Parse("00:00:00");
                            TimeSpan i = (TimeSpan.Parse("23:59:59") - RosterStartTime) + TimeSpan.Parse("00:00:01");
                            k = z + i;
                        }
                        else
                        {
                            k = (LogOutTime - RosterStartTime);
                        }
                        tiffinstaytime = k;
                        // tiffinstaytime = LogOutTime - RosterStartTime;
                    }
                    else
                    {
                        tiffinstaytime = TimeSpan.Parse(StayTime);
                        //tiffinstaytime = LogOutTime - LogInTime;
                    }
                    if (tiffinstaytime >= TiffinTime && shortleave == 0)
                    {
                        WHO_DayStatus[3] = "1";
                    }
                    else
                    {
                        WHO_DayStatus[3] = "0";
                    }
                }

                return WHO_DayStatus;
            }
            catch { return null; }
        }
        
        public static string[] OverTime_Calculation_ForRegularDuty(string ODID, TimeSpan LogInTime, TimeSpan LogOutTime, TimeSpan RosterStartTime, TimeSpan RosterEndTime, byte AcceptableLate, byte OverTimeMin, TimeSpan TiffinTime, TimeSpan MinWorkingHours, TimeSpan MinOverTime, int Shortleave, bool isgerments, string OverLateTime, string AcceptableHalfDay)
        {
            try
            {
                bool StraightFromHome=false;
                bool StraightToHome=false;
                bool IsTraining = false;
                if (ODID != "")
                {
                    DataTable dtOutduty = new DataTable();
                    dtOutduty = getOutDutyInfo(ODID);
                    if (dtOutduty != null && dtOutduty.Rows.Count > 0)
                    {
                        StraightFromHome=bool.Parse(dtOutduty.Rows[0]["StraightFromHome"].ToString());
                        StraightToHome=bool.Parse(dtOutduty.Rows[0]["StraightToHome"].ToString());
                        IsTraining = bool.Parse(dtOutduty.Rows[0]["type"].ToString());
                    }
                }
                if (DateTime.Parse("2019-01-01 " + LogInTime.ToString()).ToString("yyyy-MM-dd HH:mm") == DateTime.Parse("2019-01-01 " + LogOutTime.ToString()).ToString("yyyy-MM-dd HH:mm"))
                    LogOutTime = LogInTime;
                string[] DayStatus = new string[10];
                DayStatus[9] = "0"; //It's Half Day Deduct
                DateTime _LogInTime = DateTime.Parse("2019-01-01 " + LogInTime.ToString());
                DateTime _LogOutTime = DateTime.Parse("2019-01-01 " + LogOutTime.ToString());
              
                DateTime _ShiftStartTime = DateTime.Parse("2019-01-01 " + RosterStartTime.ToString());
                DateTime _ShiftEndTime = DateTime.Parse("2019-01-01 " + RosterEndTime.ToString());
                string LateTime = "00:00:00";
                string ExtraTime = "";
                string Get_OTHour = "";
                //if (LogInTime <= RosterStartTime + TimeSpan.Parse("00:" + AcceptableLate.ToString() + ":00")) DayStatus[0] = "P";
                //else if (LogInTime > RosterStartTime + TimeSpan.Parse("00:" + AcceptableLate.ToString() + ":00"))
                //{
                //    DayStatus[0] = "L";
                //    LateTime = (LogInTime - RosterStartTime).ToString(); // to get late time                   
                //}

                //DayStatus[1] = "Present";

                DateTime _ShiftEndTimeTemp = _ShiftStartTime.AddMinutes(AcceptableLate);
                DateTime _ShiftEndTimeTem = _ShiftEndTime.AddMinutes(-AcceptableLate);
              //  if (_LogInTime <= _ShiftEndTimeTemp && (LogOutTime==LogInTime ? true : _ShiftEndTimeTem <= _LogOutTime))
                    if (IsTraining ||((StraightFromHome || _LogInTime <= _ShiftEndTimeTemp) && (StraightToHome || (LogOutTime == LogInTime ? true : _ShiftEndTimeTem <= _LogOutTime))))
                {
                    DayStatus[0] = "P";
                    DayStatus[1] = "Present";
                    DayStatus[6] = "1";//Payble Day
                }
                else
                {
                    if(LogInTime> RosterStartTime)
                    LateTime = (LogInTime - RosterStartTime).ToString(); // to get late time
                    else if(RosterEndTime> LogOutTime)
                        LateTime = (RosterEndTime - LogOutTime).ToString(); // to get late time
                    _ShiftEndTimeTemp = _ShiftStartTime.AddMinutes(int.Parse(AcceptableHalfDay));
                    _ShiftEndTimeTem = _ShiftEndTime.AddMinutes(-int.Parse(AcceptableHalfDay));
                    if (( !StraightFromHome &&_LogInTime > _ShiftEndTimeTemp) || (LogOutTime == LogInTime ? false : (!StraightToHome && _ShiftEndTimeTem > _LogOutTime)))// It's Absent
                    {
                        DayStatus[0] = "A";
                        DayStatus[1] = "Absent";
                        DayStatus[6] = "0";
                    }
                    else // Late
                    {

                        DayStatus[0] = "L";
                        DayStatus[1] = "Present";
                        DayStatus[6] = "1";//Payble Day
                        _ShiftEndTimeTemp = _ShiftStartTime.AddMinutes(int.Parse(OverLateTime));
                        _ShiftEndTimeTem = _ShiftEndTime.AddMinutes(-int.Parse(OverLateTime));
                        if ((!StraightFromHome && _LogInTime > _ShiftEndTimeTemp) || (LogOutTime == LogInTime ? false : (!StraightToHome && _ShiftEndTimeTem > _LogOutTime)))// It's Half Day Deduct
                        {
                            DayStatus[9] = "1"; //It's Half Day Deduct
                        }
                    }
                }


                TimeSpan totalTime = (LogOutTime - RosterEndTime);
                if (totalTime > MinOverTime)
                {
                    Get_OTHour = MinOverTime.ToString();
                    ExtraTime = (totalTime - MinOverTime).ToString();
                }
                else
                {
                    if (totalTime < TimeSpan.Parse("00:00:00"))
                    {
                        totalTime = TimeSpan.Parse("00:00:00");
                        Get_OTHour = "00:00:00";
                    }
                    else
                        Get_OTHour = totalTime.ToString();
                    ExtraTime = "00:00:00";
                }               

                //-------------- to get stay time---------------------------

                string StayTime = (LogOutTime - LogInTime).ToString();

                //----------------- end ------------------------------------

                DayStatus[2] = Get_OTHour;
                DayStatus[3] = LateTime;
                DayStatus[4] = StayTime;
               
                DayStatus[7] = ExtraTime;//otherovertime
                DayStatus[8] = totalTime.ToString();//otherovertime

                //---------------------to get TiffineCount------------------
                if (isgerments)
                {
                    if (StayTime == "00:00:00" || TimeSpan.Parse(StayTime) < MinWorkingHours)
                    {
                        DayStatus[2] = "00:00:00";//OT
                        DayStatus[5] = "0";//TiffinCount
                        DayStatus[6] = "0";//Payble Day
                        DayStatus[7] = "00:00:00";//otherovertime
                        DayStatus[8] = "00:00:00";//otherovertime
                    }
                    else
                    {
                        TimeSpan tiffinstaytime;
                        if (LogInTime < RosterStartTime)
                        {
                            tiffinstaytime = LogOutTime - RosterStartTime;
                        }
                        else
                        {
                            tiffinstaytime = LogOutTime - LogInTime;
                        }
                        if (tiffinstaytime >= TiffinTime && Shortleave == 0)
                        {
                            DayStatus[5] = "1";
                        }
                        else
                        {
                            DayStatus[5] = "0";
                        }
                    }
                    //-------------------end---------------------------------------


                    if (TimeSpan.Parse(LateTime) > TimeSpan.Parse("04:00:00"))
                    {
                        DayStatus[0] = "A";
                        DayStatus[1] = "Absent";
                        DayStatus[2] = "00:00:00";
                        DayStatus[3] = LateTime;
                        DayStatus[4] = StayTime;
                        DayStatus[5] = "0";
                        DayStatus[6] = "0";
                        DayStatus[7] = "00:00:00";
                        DayStatus[8] = "00:00:00";//otherovertime
                    }
                }
                else
                {
                    if (StayTime == "00:00:00")
                    {
                        DayStatus[2] = "00:00:00";//OT
                        DayStatus[5] = "0";//TiffinCount
                        DayStatus[6] = "0";//Payble Day
                        DayStatus[7] = "00:00:00";//otherovertime
                        DayStatus[8] = "00:00:00";//otherovertime
                    }
                    else
                    {

                        if (LogOutTime >= TiffinTime && Shortleave == 0)
                        {
                            DayStatus[5] = "1";
                        }
                        else
                        {
                            DayStatus[5] = "0";
                        }
                    }
                    //-------------------end---------------------------------------

                }

                return DayStatus;
            }
            catch { return null; }
        }
     
    }
}