using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using ComplexScriptingSystem;

namespace SigmaERP.classes
{
    public class mManually_Attendance_Count
    {
       
        public static string []  Find_IsRunningEmployee(string CompanyId,string EmpCardNo )
        {
            try
            {
                DataTable dt;                          
                dt = new DataTable();
                string [] EmployeeInfo = new string[1];
                SqlDataAdapter da = new SqlDataAdapter("select EmpId,EmpName,DptName from v_Personnel_EmpCurrentStatus where Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like '%" + EmpCardNo + "'" +
                         " AND IsActive='1' AND  EmpStatus in ('1','8')  " +
                         " AND CompanyId='" + CompanyId + "'", sqlDB.connection);
                da.Fill(dt);
                //sqlDB.fillDataTable("select EmpId,EmpName,DptName from v_Personnel_EmpCurrentStatus where EmpCardNo like '%" + EmpCardNo + "'" +
                //         " AND IsActive='1' AND  EmpStatus in ('1','8')  " +
                //         " AND CompanyId='" + CompanyId + "'", dt);

                if (dt.Rows.Count > 0)
                {
                    //divFindInfo.Style.Add("Color", "Green");                
                     EmployeeInfo[0] = "Name:" + dt.Rows[0]["EmpName"].ToString() + ",Department: " + dt.Rows[0]["DptName"].ToString();
                    
                    return EmployeeInfo;                
                }
                else
                {
                    return null;
                }
            }
            catch { return null; }
        }


        public static string[] Get_Needed_EmployeeeInfo(string CompanyId, string EmpCardNo)
        {
            try
            {
                DataTable dt;
                dt = new DataTable();
                string[] EmployeeInfo = new string[8];
                sqlDB.fillDataTable("select EmpId,DptId,DsgId,GID,CompanyId,EmpDutyType,SftId,EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate from v_Personnel_EmpCurrentStatus where Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9))  like '%" + EmpCardNo + "'" +
                         " AND IsActive='1' AND  EmpStatus in ('1','8')  " +
                         " AND CompanyId='" + CompanyId + "'", dt);

                if (dt.Rows.Count > 0)
                {
                    //divFindInfo.Style.Add("Color", "Green"); 
                    EmployeeInfo[0] = dt.Rows[0]["EmpId"].ToString();
                    EmployeeInfo[1] = dt.Rows[0]["DptId"].ToString();
                    EmployeeInfo[2] = dt.Rows[0]["DsgId"].ToString();
                    EmployeeInfo[3] = dt.Rows[0]["GID"].ToString();
                    EmployeeInfo[4] = dt.Rows[0]["EmpDutyType"].ToString();
                    EmployeeInfo[5] = dt.Rows[0]["SftId"].ToString();
                    EmployeeInfo[6] = dt.Rows[0]["EmpTypeId"].ToString();
                    EmployeeInfo[7] = dt.Rows[0]["EmpJoiningDate"].ToString();
                    return EmployeeInfo;
                }
                else
                {
                    return null;
                }
            }
            catch { return null; }
        }
        public static void deleteExistingAttendanceByDate_EmpId(string AttDate, string EmpId)
        {
            try
            {
                string[] date = AttDate.Split('-');
                SqlCommand cmd;
                cmd = new System.Data.SqlClient.SqlCommand("delete from tblAttendanceRecord where attDate='" + date[2] + "-" + date[1] + "-" + date[0] + "' AND EmpId='" + EmpId + "' ", sqlDB.connection);
                cmd.ExecuteNonQuery();
                //if (result > 0) ChangeLeaveStatusByeEmpIdAndDate(dtEmpInfo.Rows[0]["EmpId"].ToString(), AttDate);

            }
            catch { }
        }
        public static string[] getTotalOverTime(TimeSpan LogInTime, TimeSpan LogOutTime, TimeSpan ShiftStartTime, TimeSpan ShiftEndTime, string AcceptableMinAsOT, string AcceptableMinAsLate, bool IsWHO_DaysTask, TimeSpan TiffinTime, TimeSpan MinWorkingHours,TimeSpan MinOverTime, string OverLateTime, string AcceptableHalfDay)
        {
            try
            {
                string[] DayStatus = new string[10];
                DayStatus[9] = "0"; //It's Half Day Deduct
                DateTime _LogInTime =DateTime.Parse("2019-01-01 " + LogInTime.ToString());
                DateTime _LogOutTime = DateTime.Parse("2019-01-01 " + LogOutTime.ToString());
                DateTime _ShiftStartTime = DateTime.Parse("2019-01-01 " + ShiftStartTime.ToString());
                DateTime _ShiftEndTime = DateTime.Parse("2019-01-01 " + ShiftEndTime.ToString());


                DayStatus[3] = "00:00:00";
                string OverTime = (!IsWHO_DaysTask)?(LogOutTime - ShiftEndTime).ToString(): (LogOutTime - LogInTime).ToString();

                if (!IsWHO_DaysTask)
                {
                    //if (LogInTime <= ShiftStartTime + TimeSpan.Parse("00:" + AcceptableMinAsLate + ":00")) DayStatus[0] = "P";
                    //else if (LogInTime > ShiftStartTime + TimeSpan.Parse("00:" + AcceptableMinAsLate.ToString() + ":00"))
                    DateTime _ShiftEndTimeTemp = _ShiftStartTime.AddMinutes(int.Parse(AcceptableMinAsLate));
                    DateTime _ShiftEndTimeTem = _ShiftEndTime.AddMinutes(-int.Parse(AcceptableMinAsLate));
                    if (_LogInTime <= _ShiftEndTimeTemp && (LogOutTime.ToString().Equals("00:00:00")? true: _ShiftEndTimeTem<= _LogOutTime))
                    {
                        DayStatus[0] = "P";
                        DayStatus[1] = "Present";
                        DayStatus[6] = "1";
                    }
                    else
                    {
                        DayStatus[3] = (LogInTime - ShiftStartTime).ToString(); // to get late time
                        _ShiftEndTimeTemp = _ShiftStartTime.AddMinutes(int.Parse(AcceptableHalfDay));
                        _ShiftEndTimeTem = _ShiftEndTime.AddMinutes(-int.Parse(AcceptableHalfDay));
                        if (_LogInTime > _ShiftEndTimeTemp || (LogOutTime.ToString().Equals("00:00:00") ? false : _ShiftEndTimeTem >_LogOutTime))// It's Absent
                        {
                            DayStatus[0] = "A";
                            DayStatus[1] = "Absent";
                            DayStatus[6] = "0";
                        }
                        else // Late
                        {
                           
                            DayStatus[0] = "L";
                            DayStatus[1] = "Present";
                            DayStatus[6] = "1";
                            _ShiftEndTimeTemp = _ShiftStartTime.AddMinutes(int.Parse(OverLateTime));
                            _ShiftEndTimeTem = _ShiftEndTime.AddMinutes(-int.Parse(OverLateTime));
                            if (_LogInTime > _ShiftEndTimeTemp || (LogOutTime.ToString().Equals("00:00:00") ? false : _ShiftEndTimeTem > _LogOutTime))// It's Half Day Deduct
                            {
                                DayStatus[9] = "1"; //It's Half Day Deduct
                            }
                        }
                    }
                    //if (_LogInTime <= _ShiftStartTime.AddMinutes(int.Parse(AcceptableMinAsLate))) DayStatus[0] = "P";
                    //else if (_LogInTime > _ShiftStartTime.AddMinutes(int.Parse(AcceptableMinAsLate)))
                    //{
                    //    DayStatus[0] = "L";
                    //    DayStatus[3] = (LogInTime - ShiftStartTime).ToString(); // to get late time           
                    //    if(_LogInTime > _ShiftStartTime.AddMinutes(int.Parse(AcceptableMinAsLate)) )

                    //}
                    //DayStatus[1] = "Present";
                }
                string Get_OTHour;
                string ExtraTime;
                TimeSpan totalTime = (LogOutTime - ShiftEndTime);
                if (totalTime > MinOverTime)
                {
                    Get_OTHour = MinOverTime.ToString();
                    ExtraTime = (totalTime - MinOverTime).ToString();
                }
                else
                {
                    if(totalTime<TimeSpan.Parse("00:00:00"))
                    {
                        totalTime = TimeSpan.Parse("00:00:00");
                        Get_OTHour = "00:00:00";
                    }
                    else Get_OTHour = totalTime.ToString();
                    ExtraTime = "00:00:00";
                }

                //------ to get stay time-------------------------------
                DateTime time = DateTime.Today + LogInTime;
                    String result = time.ToString("tt");
                TimeSpan k;
                if ((DateTime.Today + LogInTime).ToString("tt") == "PM" && (DateTime.Today + LogOutTime).ToString("tt") == "AM")
                {
                    TimeSpan z = LogOutTime - TimeSpan.Parse("00:00:00");
                    TimeSpan i = (TimeSpan.Parse("23:59:59") - LogInTime) + TimeSpan.Parse("00:00:01");
                    k = z + i;
                }
                else
                {
                    k = (LogOutTime - LogInTime);
                }

                string StayTime="00:00:00";
                if(LogOutTime.ToString()=="00:00:00")
                {
                    StayTime = StayTime;
                }
                else
                StayTime = k.ToString();
               // string StayTime = ((LogOutTime - LogInTime) < TimeSpan.Parse("00:00:00")) ? "00:00:00" : (LogOutTime - LogInTime).ToString();

                //----------------- end --------------------------------

                DayStatus[2] = Get_OTHour;
                DayStatus[4] = StayTime;
               
                DayStatus[7] = ExtraTime;
                DayStatus[8] = totalTime.ToString();
                //---------------------to get TiffineCount------------------
                if (StayTime == "00:00:00"||TimeSpan.Parse(StayTime)<MinWorkingHours)
                {
                    
                    DayStatus[2] = "00:00:00";
                    DayStatus[5] = "0";
                    DayStatus[6] = "0";
                    DayStatus[7] = "00:00:00";
                    DayStatus[8] = "00:00:00";
                }
                else
                {
                    TimeSpan tiffinstaytime;
                    if (LogInTime < ShiftStartTime)
                    {
                        time = DateTime.Today + ShiftStartTime;
                        result = time.ToString("tt");
                        if ((DateTime.Today + ShiftStartTime).ToString("tt") == "PM" && (DateTime.Today + LogOutTime).ToString("tt") == "AM")
                        {
                            TimeSpan z = LogOutTime - TimeSpan.Parse("00:00:00");
                            TimeSpan i = (TimeSpan.Parse("23:59:59") - ShiftStartTime) + TimeSpan.Parse("00:00:01");
                            k = z + i;
                        }
                        else
                        {
                            k = (LogOutTime - ShiftStartTime);
                        }
                       // tiffinstaytime = LogOutTime - ShiftStartTime;
                        tiffinstaytime = k;
                    }
                    else
                    {
                        tiffinstaytime = TimeSpan.Parse(StayTime);
                        //tiffinstaytime = LogOutTime - LogInTime;
                    }
                    if (tiffinstaytime >= TiffinTime)
                    {
                        DayStatus[5] = "1";
                    }
                    else
                    {
                        DayStatus[5] = "0";
                    }
                }
                //--  this below part comment by nayem at 17-03-2019--
                //if (TimeSpan.Parse(DayStatus[3]) > TimeSpan.Parse("04:00:00"))
                //{
                //    DayStatus[0] = "A";
                //    DayStatus[1] = "Absent";
                //    DayStatus[2] = "00:00:00";
                //    DayStatus[3] = DayStatus[3];
                //    DayStatus[4] = StayTime;
                //    DayStatus[5] = "0";
                //    DayStatus[6] = "0";
                //    DayStatus[7] = "00:00:00";
                //    DayStatus[8] = "00:00:00";
                //}

                return DayStatus;
                
            }
            catch { return null; }
        }
        public static string[] OverTime_Calculation_ForWeekend_Holiday(TimeSpan LogInTime, TimeSpan LogOutTime, int AcceptableOTMin, TimeSpan RosterStartTime, TimeSpan TiffinTime, string attstatus, string StateStatus,TimeSpan MinOverTime)
        {
            try
            {

                string[] WHO_DayStatus = new string[9];
                string ExtraTime;
                string Get_OTHour;

                int Get_OTMinute;

                //Get_OTHour = (Get_OTMinute > AcceptableOTMin) ? (byte)(int.Parse(Get_OTHour.ToString()) + 1) : Get_OTHour;

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

                //string StayTime = ((LogOutTime - LogInTime) < TimeSpan.Parse("00:00:00")) ? "00:00:00" : (LogOutTime - LogInTime).ToString();
                WHO_DayStatus[0] = attstatus;
                WHO_DayStatus[1] = StateStatus;
                WHO_DayStatus[2] = Get_OTHour;
                WHO_DayStatus[3] = "00:00:00";
                WHO_DayStatus[4] = StayTime;
                WHO_DayStatus[6] = "0";
                WHO_DayStatus[7] = ExtraTime;
                WHO_DayStatus[8] = StayTime;
                //---------------------to get TiffineCount------------------
                if (StayTime == "00:00:00")
                {
                    WHO_DayStatus[5] = "0";
                    WHO_DayStatus[2] = "00:00:00";
                    WHO_DayStatus[7] = "00:00:00";
                    WHO_DayStatus[8] = "00:00:00";
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
                       // tiffinstaytime = LogOutTime - LogInTime;
                    }
                    if (tiffinstaytime >= TiffinTime)
                    {
                        WHO_DayStatus[5] = "1";
                    }
                    else
                    {
                        WHO_DayStatus[5] = "0";
                    }
                }

                return WHO_DayStatus;
            }
            catch { return null; }
        }

        public static void SaveAttendance_Status(string EmpId, string selectedDate, string EmpTypeId, string InHour, string InMin, string InSec, string OutHour, string OutMin, string OutSec,
                                           string AttStatus, string StateStatus, string OverTime, string SftId, string DptId, string DsgId, string CompanyId, string GId, string LateTime, string StayTime, string DailyStartTimeALT_CloseTime, string AttManual, string TiffinCount, string HolidayCount, string PaybleDays, string OtherOverTime,string TotalOverTime,string OutDuty, string ReferenceID,string UserId,string Remark, string IsHalfDayLeave, string IsHalfDayDeduct)
        {
            try
            {
                deleteExistingAttendanceByDate_EmpId(selectedDate, EmpId); // for delete existing attendance record
                
                DateTime dtTimeConvert;
                LateTime = (LateTime == null) ? "00:00:00" : LateTime;
                if (DateTime.TryParse(LateTime, out dtTimeConvert)) LateTime = dtTimeConvert.ToString("HH:mm:ss");
                dtTimeConvert = new DateTime();
                StayTime = (StayTime == null) ? "00:00:00" : StayTime;
                if (OutDuty == "1")
                {
                    TiffinCount = "1";
                    PaybleDays = "1";
                }

                if (DateTime.TryParse(StayTime, out dtTimeConvert)) StayTime = dtTimeConvert.ToString("HH:mm:ss");
                string[] getColumns = { "EmpId", "AttDate", "EmpTypeId", "InHour", "InMin", "InSec", "OutHour", "OutMin", "OutSec",
                                        "AttStatus", "StateStatus",
                                        "DailyStartTimeALT_CloseTime", "OverTime", "SftId", "DptId","DsgId", "CompanyId", "GId","LateTime","StayTime","AttManual","TiffinCount","HolidayCount","PaybleDays","OtherOverTime","TotalOverTime","OutDuty" ,"ReferenceID","UserId","Remark","IsHalfDayLeave","IsHalfDayDeduct"};

                string[] getValues = { EmpId, commonTask.ddMMyyyyToyyyyMMdd(selectedDate).ToString(),
                                                 EmpTypeId,InHour,InMin,InSec,OutHour,OutMin,OutSec,AttStatus,
                                                 StateStatus,DailyStartTimeALT_CloseTime,OverTime,SftId,DptId,DsgId,CompanyId,GId,LateTime,StayTime,AttManual,TiffinCount,HolidayCount,PaybleDays,OtherOverTime,TotalOverTime,OutDuty, ReferenceID,UserId,Remark,IsHalfDayLeave,IsHalfDayDeduct};
                SQLOperation.forSaveValue("tblAttendanceRecord", getColumns, getValues, sqlDB.connection);
            }
            catch { }
        }
        public static void SaveAttendance_Status_OD(string EmpId, string selectedDate, string EmpTypeId, string InHour, string InMin, string InSec, string OutHour, string OutMin, string OutSec,
                                           string AttStatus, string StateStatus, string OverTime, string SftId, string DptId, string DsgId, string CompanyId, string GId, string LateTime, string StayTime, string DailyStartTimeALT_CloseTime, string AttManual, string TiffinCount, string HolidayCount, string PaybleDays, string OtherOverTime, string TotalOverTime, string OutDuty, string ReferenceID, string UserId, string Remark,string ODID)
        {
            try
            {
                deleteExistingAttendanceByDate_EmpId(selectedDate, EmpId); // for delete existing attendance record

                DateTime dtTimeConvert;
                LateTime = (LateTime == null) ? "00:00:00" : LateTime;
                if (DateTime.TryParse(LateTime, out dtTimeConvert)) LateTime = dtTimeConvert.ToString("HH:mm:ss");
                dtTimeConvert = new DateTime();
                StayTime = (StayTime == null) ? "00:00:00" : StayTime;
                if (OutDuty == "1")
                {
                    TiffinCount = "1";
                    PaybleDays = "1";
                }

                if (DateTime.TryParse(StayTime, out dtTimeConvert)) StayTime = dtTimeConvert.ToString("HH:mm:ss");
                string[] getColumns = { "EmpId", "AttDate", "EmpTypeId", "InHour", "InMin", "InSec", "OutHour", "OutMin", "OutSec",
                                        "AttStatus", "StateStatus",
                                        "DailyStartTimeALT_CloseTime", "OverTime", "SftId", "DptId","DsgId", "CompanyId", "GId","LateTime","StayTime","AttManual","TiffinCount","HolidayCount","PaybleDays","OtherOverTime","TotalOverTime","OutDuty" ,"ReferenceID","UserId","Remark","ODID"};

                string[] getValues = { EmpId, commonTask.ddMMyyyyToyyyyMMdd(selectedDate).ToString(),
                                                 EmpTypeId,InHour,InMin,InSec,OutHour,OutMin,OutSec,AttStatus,
                                                 StateStatus,DailyStartTimeALT_CloseTime,OverTime,SftId,DptId,DsgId,CompanyId,GId,LateTime,StayTime,AttManual,TiffinCount,HolidayCount,PaybleDays,OtherOverTime,TotalOverTime,OutDuty, ReferenceID,UserId,Remark,ODID};
                SQLOperation.forSaveValue("tblAttendanceRecord", getColumns, getValues, sqlDB.connection);
            }
            catch { }
        }
        public static string [] Roster_Operation_TimeChecking(TimeSpan ShiftPunchStartTime,TimeSpan ShiftEndTime,TimeSpan LogInTime)
        {
            try
            {
                string[] Roster_Operation_Status = new string[2];
                if (LogInTime< ShiftPunchStartTime)
                {
                    Roster_Operation_Status[0] = "False";
                    Roster_Operation_Status[1] = "Please type valid shift start time .{0} is shift start punch count time " +ShiftPunchStartTime;
                    return Roster_Operation_Status;
                }
                else
                {
                    Roster_Operation_Status[0] = "True";
                    return Roster_Operation_Status;
                }
                    
                
            }
            catch { return null; }
        }

    }
}