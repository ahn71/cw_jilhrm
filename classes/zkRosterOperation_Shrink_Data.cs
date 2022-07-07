using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SigmaERP.classes
{
    public class zkRosterOperation_Shrink_Data
    {
        public static void RoserOperationProcessing(DateTime AttDate, string EmpId, byte EmpTypeId, string EmpCardNo_BadgeNo, bool IsWeekendHolydayOthers, string Roster_StartTimeIndicator, string Roster_EndTimeIndicator, bool Has_AssingnedShift, TimeSpan ShiftStartTime, TimeSpan ShiftEndTime, TimeSpan ShiftPunchCountStartTime, string AcceptableLate, int AcceptableMinAsOT, string ShiftId, string DepartmentId, string DesignationId, string CompanyId, string GId, string BreakStartTime, string BreakEndTime, bool IsOT, TimeSpan TiffinTime, bool holidaycount, bool isgerments, string UserId,string  tableName )
        {
            try
            {
                // Zk Att Execute here
                DataTable dtPunchList = new DataTable();
                string[] WHO_DayStatus = { "0", "00:00:00", "00:00:00", "0", "0" };
                string[] DayStatus = { "", "", "00:00:00", "00:00:00", "00:00:00", "0", "0", "00:00:00", "00:00:00" };
                DataTable dt;
                string[] BreakTime_Counting = { "00:00:00", "00:00:00" };
                DataTable dtOtherSettings = mZK_Shrink_Data_SqlServer.LoadOTherSettings(CompanyId);
                DataTable dtshortleave = mZK_Shrink_Data_SqlServer.LoadShorLeave(EmpId, AttDate.ToString("yyyy-MM-dd"));
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
                string tiffin = EmpTypeId.ToString() == "1" ? worker : staff;

                if (IsWeekendHolydayOthers)     // if Weekend or holyday setup but employe is present to do tasks
                {
                    sqlDB.fillDataTable("select distinct Badgenumber,FORMAT(CHECKTIME,'HH') as Hour,FORMAT(CHECKTIME,'mm') as Minute,FORMAT(CHECKTIME,'ss') as Second, FORMAT(CHECKTIME,'HH:mm:ss') as PunchTime from  " + tableName + "  where CONVERT(date,CHECKTIME)='" + AttDate.ToString("yyyy-MM-dd") + "' AND Format(CheckTime,'HH:mm:ss')>='" + ShiftPunchCountStartTime + "' AND Badgenumber Like '%" + EmpCardNo_BadgeNo + "'  order by PunchTime", dtPunchList);
                   // sqlDB.fillDataTable("select distinct ProximityNo,Hour,Minute,Second,Convert(varchar(2),Hour)+':'+CONVERT(varchar(2),Minute)+':'+Convert(varchar(2),Second) as PunchTime from tblAttendance where PunchDate='" + AttDate.ToString("yyyy-MM-dd") + "'  and convert(time(7), Convert(varchar(2),Hour)+':'+CONVERT(varchar(2),Minute)+':'+Convert(varchar(2),Second))>='" + ShiftPunchCountStartTime + "' AND ProximityNo='" + ProximityNo + "' order by Hour,Minute,Second ", dtPunchList);
                    string[] Leave_Info = mZK_Shrink_Data_SqlServer.Check_Any_Leave_Are_Exist(AttDate.ToString("yyyy-MM-dd"), EmpId);

                    if (dtPunchList.Rows.Count > 0)
                    {
                        WHO_DayStatus = mZK_Shrink_Data_SqlServer.OverTime_Calculation_ForWeekend_Holiday(TimeSpan.Parse(dtPunchList.Rows[0]["PunchTime"].ToString()), TimeSpan.Parse(dtPunchList.Rows[0]["PunchTime"].ToString()),AcceptableMinAsOT, ShiftStartTime, TiffinTime, TimeSpan.Parse(othersetting[7]), dtshortleave.Rows.Count);
                        DayStatus[0] = (Leave_Info[0].ToString() != "0") ? "Lv" : "W";
                        DayStatus[1] = (Leave_Info[0].ToString() != "0") ? Leave_Info[1] : "Weekend";
                        DayStatus[2] = WHO_DayStatus[0];
                        if (EmpTypeId.ToString() != "1" && TimeSpan.Parse(WHO_DayStatus[2]) >= TimeSpan.Parse(othersetting[6]))
                        {
                            WHO_DayStatus[4] = "1";
                        }
                        else
                        {
                            WHO_DayStatus[4] = "0";
                        }
                        if (!isgerments)
                        {
                            WHO_DayStatus[3] = "0";
                        }
                    }
                    else
                    {
                        DayStatus[0] = (DayStatus[1].Equals("Weekend")) ? "W" : "H";
                        DayStatus[1] = (DayStatus[1].Equals("Weekend")) ? "Weekend" : "Holiday";
                        DayStatus[2] = "00:00:00";
                        DayStatus[7] = "00:00:00";
                        DayStatus[8] = "00:00:00";
                    }
                    if (Leave_Info[0].ToString() != "0") classes.LeaveLibrary.LeaveCount(AttDate.ToString("yyyy-MM-dd"), Leave_Info[0]);
                }

                //  if shift time is (AM to PM) OR (AM to AM) OR (PM to PM) then------------------------------------------------------- 
                else if ((Roster_StartTimeIndicator.Trim().Equals("AM") && Roster_EndTimeIndicator.Trim().Equals("PM")) ||
                    (Roster_StartTimeIndicator.Trim().Equals("AM") && Roster_EndTimeIndicator.Trim().Equals("AM")) ||
                    (Roster_StartTimeIndicator.Trim().Equals("PM") && Roster_EndTimeIndicator.Trim().Equals("PM")))
                {
                    string[] Leave_Info = mZK_Shrink_Data_SqlServer.Check_Any_Leave_Are_Exist(AttDate.ToString("yyyy-MM-dd"), EmpId);
                    if (Leave_Info[0].ToString() != "0")  // check any type of leave. if are leave exists then execute this if block
                    {
                        DayStatus[0] = "Lv";
                        DayStatus[1] = Leave_Info[1];
                    } //End-----------------------------------------------------------------------------------------------------------------------
                    else // without leave---------------------------------------------------------------------------------------------------------
                    {

                        sqlDB.fillDataTable("select distinct Badgenumber,FORMAT(CHECKTIME,'HH') as Hour,FORMAT(CHECKTIME,'mm') as Minute,FORMAT(CHECKTIME,'ss') as Second, FORMAT(CHECKTIME,'HH:mm:ss') as PunchTime from  " + tableName + "  where CONVERT(date,CHECKTIME)='" + AttDate.ToString("yyyy-MM-dd") + "' AND Format(CheckTime,'HH:mm:ss')>='" + ShiftPunchCountStartTime + "' AND Badgenumber Like '%" + EmpCardNo_BadgeNo + "%'  order by PunchTime", dtPunchList);                       

                        if (dtPunchList.Rows.Count == 0)  // any punched are not exists of selectd day
                        {
                            DayStatus[0] = "A";
                            DayStatus[1] = "Absent";
                            DayStatus[2] = "00:00:00";
                            DayStatus[7] = "00:00:00";
                            DayStatus[8] = "00:00:00";
                        }
                        else
                        {
                            DayStatus = OverTime_Calculation_ForRegularDuty(TimeSpan.Parse(dtPunchList.Rows[0]["PunchTime"].ToString()), TimeSpan.Parse(dtPunchList.Rows[dtPunchList.Rows.Count - 1]["PunchTime"].ToString()), ShiftStartTime, ShiftEndTime, byte.Parse(AcceptableLate), (byte)AcceptableMinAsOT, "", IsOT, TiffinTime, TimeSpan.Parse(othersetting[5]), TimeSpan.Parse(othersetting[7]), dtshortleave.Rows.Count);
                           
                            //DayStatus[0] = DayStatus[0];
                            //DayStatus[1] = DayStatus[1];
                            //DayStatus[2] = DayStatus[2];
                            BreakTime_Counting = classes.mCommon_Module_For_AttendanceProcessing.Counting_BreakTime(AttDate.ToString("yyyy-MM-dd"), EmpCardNo_BadgeNo, BreakStartTime, BreakEndTime);  // counting breaktime
                        }
                    }
                }
                else  // PM-AM
                {
                    string[] Leave_Info = mZK_Shrink_Data_SqlServer.Check_Any_Leave_Are_Exist(AttDate.ToString("yyyy-MM-dd"), EmpId);
                    if (Leave_Info[0].ToString() != "0")  // check any type of leave. if are leave exists then execute this if block
                    {
                        DayStatus[0] = "Lv";
                        DayStatus[1] = Leave_Info[1];
                        classes.LeaveLibrary.LeaveCount(AttDate.ToString("yyyy-MM-dd"), Leave_Info[0]);
                    } //End-----------------------------------------------------------------------------------------------------------------------
                    else // without leave---------------------------------------------------------------------------------------------------------
                    {


                        sqlDB.fillDataTable("select distinct Badgenumber,FORMAT(CHECKTIME,'HH') as Hour,FORMAT(CHECKTIME,'mm') as Minute,FORMAT(CHECKTIME,'ss') as Second, FORMAT(CHECKTIME,'HH:mm:ss') as PunchTime from  " + tableName + "  where CONVERT(date,CHECKTIME)='" + AttDate.ToString("yyyy-MM-dd") + "' AND Format(CheckTime,'HH:mm:ss')>='" + ShiftPunchCountStartTime + "'  AND Badgenumber Like '%" + EmpCardNo_BadgeNo + "'  order by PunchTime", dtPunchList);
                        
                        if (dtPunchList.Rows.Count == 0)  // any punched are not exists of selectd day
                        {
                            DayStatus[0] = "A";
                            DayStatus[1] = "Absent";
                            DayStatus[2] = "00:00:00";
                            DayStatus[7] = "00:00:00";
                            DayStatus[8] = "00:00:00";
                        }
                        else
                        {

                            DateTime tempDate = AttDate.AddDays(1);
                            TimeSpan FirstPunch = TimeSpan.Parse(dtPunchList.Rows[0]["PunchTime"].ToString());
                            //  byte TotalRows = (byte)dtPunchList.Rows.Count;
                            TimeSpan LastPunch = TimeSpan.Parse("00:00:00");
                            if (dtPunchList.Rows.Count > 1)
                                LastPunch = TimeSpan.Parse(dtPunchList.Rows[dtPunchList.Rows.Count - 1]["PunchTime"].ToString());
                            DataTable dttemp;
                            sqlDB.fillDataTable("select distinct Badgenumber,FORMAT(CHECKTIME,'HH') as Hour,FORMAT(CHECKTIME,'mm') as Minute,FORMAT(CHECKTIME,'ss') as Second, FORMAT(CHECKTIME,'HH:mm:ss') as PunchTime from  " + tableName + "  where CONVERT(date,CHECKTIME)='" + tempDate.ToString("yyyy-MM-dd") + "'  AND Format(CheckTime,'HH:mm:ss')<='11:59:59' AND Badgenumber Like '%" + EmpCardNo_BadgeNo + "'  order by PunchTime", dttemp = new DataTable());
                           // sqlDB.fillDataTable("select distinct ProximityNo,Hour,Minute,Second,Convert(varchar(2),Hour)+':'+CONVERT(varchar(2),Minute)+':'+Convert(varchar(2),Second) as PunchTime from tblAttendance where PunchDate='" + tempDate.ToString("yyyy-MM-dd") + "' AND ProximityNo='" + ProximityNo + "' and convert(time(7), Convert(varchar(2),Hour)+':'+CONVERT(varchar(2),Minute)+':'+Convert(varchar(2),Second))>='00:00:00' and Hour<=11 and Minute<=59 and Second<= 59 order by Hour,Minute,Second ", dttemp = new DataTable());
                            if (dttemp.Rows.Count > 0)
                            {
                                for (byte b = 0; b < dttemp.Rows.Count; b++)
                                {
                                    dtPunchList.Rows.Add(dttemp.Rows[b]["Badgenumber"].ToString(), dttemp.Rows[b]["Hour"].ToString(), dttemp.Rows[b]["Minute"].ToString(),
                                     dttemp.Rows[b]["Second"].ToString(), dttemp.Rows[b]["PunchTime"].ToString());
                                }
                                LastPunch = TimeSpan.Parse(dtPunchList.Rows[dtPunchList.Rows.Count - 1]["PunchTime"].ToString()); // to get out time of duty .so last punch are counted 
                                BreakTime_Counting = classes.mCommon_Module_For_AttendanceProcessing.Counting_BreakTime(AttDate.ToString("yyyy-MM-dd"), EmpCardNo_BadgeNo, BreakStartTime, BreakEndTime);  // counting breaktime

                            }
                            else
                                IsOT = false;
                            DayStatus = OverTime_Calculation_ForRegularDuty(FirstPunch, LastPunch, ShiftStartTime, ShiftEndTime, byte.Parse(AcceptableLate), (byte)AcceptableMinAsOT, "", IsOT, TiffinTime, TimeSpan.Parse(othersetting[5]), "PMtoAM", TimeSpan.Parse(othersetting[7]), dtshortleave.Rows.Count);
                         
                        }
                    }
                }


                mZK_Shrink_Data_SqlServer.SaveAttendance_Status(EmpId,AttDate.ToString("dd-MM-yyyy"),EmpTypeId.ToString(),
                    (dtPunchList.Rows.Count==0)?"00":(dtPunchList.Rows[0]["Hour"].ToString().Length==1)?"0"+ dtPunchList.Rows[0]["Hour"].ToString(): dtPunchList.Rows[0]["Hour"].ToString(),
                                                               (dtPunchList.Rows.Count == 0) ? "00" : (dtPunchList.Rows[0]["Minute"].ToString().Length == 1) ? "0" + dtPunchList.Rows[0]["Minute"].ToString() : dtPunchList.Rows[0]["Minute"].ToString(),
                                                               (dtPunchList.Rows.Count == 0) ? "00" : (dtPunchList.Rows[0]["Second"].ToString().Length == 1) ? "0" + dtPunchList.Rows[0]["Second"].ToString() : dtPunchList.Rows[0]["Second"].ToString(),

                                                               (dtPunchList.Rows.Count == 0) ? "00" : ((dtPunchList.Rows[0]["Hour"].ToString().Trim()== dtPunchList.Rows[dtPunchList.Rows.Count-1]["Hour"].ToString().Trim()) && (dtPunchList.Rows[0]["Minute"].ToString().Trim() == dtPunchList.Rows[dtPunchList.Rows.Count - 1]["Minute"].ToString().Trim())) ?"00":(dtPunchList.Rows[dtPunchList.Rows.Count-1]["Hour"].ToString().Length == 1) ? "0" + dtPunchList.Rows[dtPunchList.Rows.Count-1]["Hour"].ToString() : dtPunchList.Rows[dtPunchList.Rows.Count-1]["Hour"].ToString(),
                                                               (dtPunchList.Rows.Count == 0) ? "00" : ((dtPunchList.Rows[0]["Hour"].ToString().Trim() == dtPunchList.Rows[dtPunchList.Rows.Count - 1]["Hour"].ToString().Trim()) && (dtPunchList.Rows[0]["Minute"].ToString().Trim() == dtPunchList.Rows[dtPunchList.Rows.Count - 1]["Minute"].ToString().Trim())) ? "00" : (dtPunchList.Rows[dtPunchList.Rows.Count - 1]["Minute"].ToString().Length == 1) ? "0" + dtPunchList.Rows[dtPunchList.Rows.Count - 1]["Minute"].ToString() : dtPunchList.Rows[dtPunchList.Rows.Count - 1]["Minute"].ToString(),
                                                               (dtPunchList.Rows.Count == 0) ? "00" : ((dtPunchList.Rows[0]["Hour"].ToString().Trim() == dtPunchList.Rows[dtPunchList.Rows.Count - 1]["Hour"].ToString().Trim()) && (dtPunchList.Rows[0]["Minute"].ToString().Trim() == dtPunchList.Rows[dtPunchList.Rows.Count - 1]["Minute"].ToString().Trim())) ? "00" : (dtPunchList.Rows[dtPunchList.Rows.Count - 1]["Second"].ToString().Length == 1) ? "0" + dtPunchList.Rows[dtPunchList.Rows.Count - 1]["Second"].ToString() : dtPunchList.Rows[dtPunchList.Rows.Count - 1]["Second"].ToString(),
                                                                DayStatus[0], DayStatus[1], DayStatus[2], ShiftId, DepartmentId, DesignationId, CompanyId, GId, DayStatus[3], DayStatus[4], ShiftStartTime + ":" + AcceptableLate + ":" + ShiftEndTime, DayStatus[5], WHO_DayStatus[4], DayStatus[6], DayStatus[7], DayStatus[8], UserId,"0", "0");

                //---------------------------------------------------------------------------------------------------------------------
            }
            catch { }
        }


        public static string[] OverTime_Calculation_ForRegularDuty(TimeSpan LogInTime, TimeSpan LogOutTime, TimeSpan RosterStartTime, TimeSpan RosterEndTime, byte AcceptableLate, byte OverTimeMin, string TimeIndicator, bool IsOT, TimeSpan TiffinTime, TimeSpan MinWorkingHours, TimeSpan MinOverTime, int shortleave)
        {
            try
            {
                string[] DayStatus = new string[9];  // DayaStatus [0]= Status P/L,DayStatus[1]=State Staus,DayStatus[2]=OverTime,DayStatus[3]=LateTime,DayStatus[4]=StayTime;
                string LateTime = "00:00:00";

                if (LogInTime <= RosterStartTime + TimeSpan.Parse("00:" + AcceptableLate.ToString() + ":00")) DayStatus[0] = "P";
                else if (LogInTime > RosterStartTime + TimeSpan.Parse("00:" + AcceptableLate.ToString() + ":00"))
                {
                    LateTime = (RosterStartTime - LogInTime).ToString(); // to get late time
                    DayStatus[0] = "L";
                }
                DayStatus[1] = "Present";

                string ExtraTime = "";
                string Get_OTHour = "";
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
                    else Get_OTHour = totalTime.ToString();
                    ExtraTime = "00:00:00";
                }
                //int Get_OTHour = (((int)TimeSpan.Parse(ExtraTime).Hours) > 0) ? (int)TimeSpan.Parse(ExtraTime).Hours : 0;

                //// int Get_OTMinutea = (int)TimeSpan.Parse(ExtraTime).Minutes;
                //int Get_OTMinute = (((int)TimeSpan.Parse(ExtraTime).Minutes) > 0) ? (int)TimeSpan.Parse(ExtraTime).Minutes : 0;

                // Get_OTHour = (Get_OTMinute > OverTimeMin) ? (byte)(int.Parse(Get_OTHour.ToString()) + 1) : Get_OTHour;

                //-------------- to get stay time---------------------------

                string StayTime = (LogOutTime - LogInTime).ToString();
                StayTime = (StayTime.Contains('-')) ? "00:00:00" : StayTime;
                LateTime = (LateTime.Contains('-')) ? "00:00:00" : LateTime;

                //----------------- end ------------------------------------
                DayStatus[2] = Get_OTHour;
                DayStatus[3] = LateTime;
                DayStatus[4] = StayTime;
                DayStatus[6] = "1";
                DayStatus[7] = ExtraTime;
                DayStatus[8] = totalTime.ToString();
                //---------------------to get TiffineCount------------------
                if (StayTime == "00:00:00" || TimeSpan.Parse(StayTime) < MinWorkingHours)
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
                    if (LogInTime < RosterStartTime)
                    {
                        tiffinstaytime = LogOutTime - RosterStartTime;
                    }
                    else
                    {
                        tiffinstaytime = LogOutTime - LogInTime;
                    }
                    if (tiffinstaytime >= TiffinTime && shortleave == 0)
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
                    DayStatus[8] = "00:00:00";
                }

                return DayStatus;
            }
            catch { return null; }
        }
        public static string[] OverTime_Calculation_ForRegularDuty(TimeSpan LogInTime, TimeSpan LogOutTime, TimeSpan RosterStartTime, TimeSpan RosterEndTime, byte AcceptableLate, byte OverTimeMin, string TimeIndicator, bool IsOT, TimeSpan TiffinTime, TimeSpan MinWorkingHours, string PMtoAM, TimeSpan MinOverTime, int Shortleave)
        {
            try
            {
                string[] DayStatus = new string[9];  // DayaStatus [0]= Status P/L,DayStatus[1]=State Staus,DayStatus[2]=OverTime,DayStatus[3]=LateTime,DayStatus[4]=StayTime;
                string LateTime = "00:00:00";

                if (LogInTime <= RosterStartTime + TimeSpan.Parse("00:" + AcceptableLate.ToString() + ":00")) DayStatus[0] = "P";
                else if (LogInTime > RosterStartTime + TimeSpan.Parse("00:" + AcceptableLate.ToString() + ":00"))
                {
                    LateTime = (RosterStartTime - LogInTime).ToString(); // to get late time
                    DayStatus[0] = "L";
                }
                DayStatus[1] = "Present";

                string ExtraTime = "";
                string Get_OTHour = "";
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
                    else Get_OTHour = totalTime.ToString();
                    ExtraTime = "00:00:00";
                }

                //-------------- to get stay time---------------------------

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



                string StayTime = k.ToString();
                StayTime = (StayTime.Contains('-')) ? "00:00:00" : StayTime;
                LateTime = (LateTime.Contains('-')) ? "00:00:00" : LateTime;

                //----------------- end ------------------------------------
                DayStatus[2] = Get_OTHour;
                DayStatus[3] = LateTime;
                DayStatus[4] = StayTime;
                DayStatus[6] = "1";
                DayStatus[7] = ExtraTime;
                DayStatus[8] = totalTime.ToString();
                //---------------------to get TiffineCount------------------
                if (StayTime == "00:00:00" || TimeSpan.Parse(StayTime) < MinWorkingHours)
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
                    if (LogInTime < RosterStartTime)
                    {
                        if ((DateTime.Today + RosterStartTime).ToString("tt") == "PM" && (DateTime.Today + LogOutTime).ToString("tt") == "AM")
                        {

                            TimeSpan z = LogOutTime - TimeSpan.Parse("00:00:00");
                            TimeSpan i = (TimeSpan.Parse("23:59:59") - RosterStartTime) + TimeSpan.Parse("00:00:01");
                            tiffinstaytime = z + i;
                        }
                        else
                        {
                            tiffinstaytime = LogOutTime - RosterStartTime;
                        }
                    }
                    else
                    {
                        tiffinstaytime = TimeSpan.Parse(StayTime);
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
                    DayStatus[8] = "00:00:00";
                }

                return DayStatus;
            }
            catch { return null; }
        }

    }
}