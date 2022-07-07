using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SigmaERP.classes
{
    public class mCommon_Module_For_AttendanceProcessing
    {
        // this method for retrive current employee list by applied conditions
        public static DataTable loadRunningEmployee(string attDate, bool ForAllEmployee,string CompanyId, string DepartmentId, string EmpCardNo)
        {
            try
            {
                //DataTable dtEmpInfo = new DataTable();
                //if (ForAllEmployee)
                //{
                //    if (DepartmentId == "0")
                //        sqlDB.fillDataTable("select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,convert(varchar(11),EmpJoiningDate,105)as EmpJoiningDate,SftId,RealProximityNo,GId,DptId,DsgId,EmpDutyType from v_Personnel_EmpCurrentStatus where CompanyId='" + CompanyId + "' and EmpStatus in ('1','8') ", dtEmpInfo);
                //    else
                //        sqlDB.fillDataTable("select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,convert(varchar(11),EmpJoiningDate,105)as EmpJoiningDate,SftId,RealProximityNo,GId,DptId,DsgId,EmpDutyType from v_Personnel_EmpCurrentStatus where CompanyId='" + CompanyId + "' and DptId='" + DepartmentId + "' AND EmpStatus in ('1','8') ", dtEmpInfo);
                //}
                //else
                //    sqlDB.fillDataTable("select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpTypeId,convert(varchar(11),EmpJoiningDate,105)as EmpJoiningDate,SftId,RealProximityNo,GId,DptId,DsgId,EmpDutyType from v_Personnel_EmpCurrentStatus where CompanyId='" + CompanyId + "' and EmpCardNo Like '%" + EmpCardNo + "%' AND EmpStatus in ('1','8') ", dtEmpInfo);
                //return dtEmpInfo;
                string sqlCmd = "";
                DataTable dtEmpInfo = new DataTable();
                if (ForAllEmployee)
                {
                    if (DepartmentId == "0")
                        sqlCmd = "select cs.EmpId,Convert(varchar(3),left(cs.EmpCardNo,LEN(cs.EmpCardNo)-4))+' '+Convert(varchar(10),right(cs.EmpCardNo,LEN(cs.EmpCardNo)-9)) as EmpCardNo,cs.EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate,SftId,EmpAttCard as RealProximityNo,GId,DptId,DsgId,EmpDutyType,EmpAttCard from v_Personnel_EmpCurrentStatus cs left join Personnel_EmpSeparation sp on cs.EmpId=sp.EmpId and cs.EmpStatus=sp.SeparationType where  cs.IsActive=1 and  CompanyId='" + CompanyId + "' and ( EmpStatus in ('1','8') or sp.EffectiveDate>='" + attDate + "' ) ";
                    else
                        sqlCmd = "select cs.EmpId,Convert(varchar(3),left(cs.EmpCardNo,LEN(cs.EmpCardNo)-4))+' '+Convert(varchar(10),right(cs.EmpCardNo,LEN(cs.EmpCardNo)-9)) as EmpCardNo,cs.EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate,SftId,EmpAttCard as RealProximityNo,GId,DptId,DsgId,EmpDutyType,EmpAttCard from v_Personnel_EmpCurrentStatus cs left join Personnel_EmpSeparation sp on cs.EmpId=sp.EmpId and cs.EmpStatus=sp.SeparationType where  cs.IsActive=1 and CompanyId='" + CompanyId + "' and DptId='" + DepartmentId + "' AND ( EmpStatus in ('1','8') or sp.EffectiveDate>='" + attDate + "' ) ";
                }
                else
                    sqlCmd = "select cs.EmpId,Convert(varchar(3),left(cs.EmpCardNo,LEN(cs.EmpCardNo)-4))+' '+Convert(varchar(10),right(cs.EmpCardNo,LEN(cs.EmpCardNo)-9)) as EmpCardNo,cs.EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate,SftId,EmpAttCard as RealProximityNo,GId,DptId,DsgId,EmpDutyType,EmpAttCard from v_Personnel_EmpCurrentStatus cs left join Personnel_EmpSeparation sp on cs.EmpId=sp.EmpId and cs.EmpStatus=sp.SeparationType  where  cs.IsActive=1 and CompanyId='" + CompanyId + "' AND ( cs.EmpCardNo  Like '%" + EmpCardNo + "' or Convert(varchar(3),left(cs.EmpCardNo,LEN(cs.EmpCardNo)-4))+' '+Convert(varchar(10),right(cs.EmpCardNo,LEN(cs.EmpCardNo)-9)) Like '%" + EmpCardNo + "') AND (EmpStatus in ('1', '8') or sp.EffectiveDate >= '" + attDate + "') ";
               sqlDB.fillDataTable(sqlCmd, dtEmpInfo);
                return dtEmpInfo;
            }
            catch { return null; }
        }

        public static DataTable RMSA_loadRunningEmployee(string attDate, bool ForAllEmployee, string CompnayId, string DepartmentId, string EmpCardNo)
        {
            try
            {
                DataTable dtEmpInfo = new DataTable();
                if (ForAllEmployee)
                {
                    if (DepartmentId == "0")
                        sqlDB.fillDataTable("select cs.EmpId,Convert(int,Right(cs.EmpCardNo,LEN(cs.EmpCardNo)-7)) as EmpCardNo,cs.EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate,SftId,EmpAttCard as RealProximityNo,GId,DptId,DsgId,EmpDutyType,EmpAttCard,isnull(IsDelivery,0) IsDelivery from v_Personnel_EmpCurrentStatus cs left join Personnel_EmpSeparation sp on cs.EmpId=sp.EmpId and cs.EmpStatus=sp.SeparationType where  cs.IsActive=1 and  CompanyId='" + CompnayId + "' and ( EmpStatus in ('1','8') or sp.EffectiveDate>='" + attDate + "' ) ", dtEmpInfo);
                    // sqlDB.fillDataTable("select EmpId,Convert(int,Right(EmpCardNo,LEN(EmpCardNo)-7)) as EmpCardNo,EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate,SftId,EmpAttCard as RealProximityNo,GId,DptId,DsgId,EmpDutyType,EmpAttCard,isnull(IsDelivery,0) IsDelivery from v_Personnel_EmpCurrentStatus where  IsActive=1 and  CompanyId='" + CompnayId + "' and EmpStatus in ('1','8') AND EmpAttCard !='' ", dtEmpInfo);
                    else
                        sqlDB.fillDataTable("select cs.EmpId,Convert(int,Right(cs.EmpCardNo,LEN(cs.EmpCardNo)-7)) as EmpCardNo,cs.EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate,SftId,EmpAttCard as RealProximityNo,GId,DptId,DsgId,EmpDutyType,EmpAttCard,isnull(IsDelivery,0) IsDelivery from v_Personnel_EmpCurrentStatus cs left join Personnel_EmpSeparation sp on cs.EmpId=sp.EmpId and cs.EmpStatus=sp.SeparationType where  cs.IsActive=1 and CompanyId='" + CompnayId + "' and DptId='" + DepartmentId + "' AND ( EmpStatus in ('1','8') or sp.EffectiveDate>='" + attDate + "' ) ", dtEmpInfo);
                    // sqlDB.fillDataTable("select EmpId,Convert(int,Right(EmpCardNo,LEN(EmpCardNo)-7)) as EmpCardNo,EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate,SftId,EmpAttCard as RealProximityNo,GId,DptId,DsgId,EmpDutyType,EmpAttCard,isnull(IsDelivery,0) IsDelivery from v_Personnel_EmpCurrentStatus where  IsActive=1 and CompanyId='" + CompnayId + "' and DptId='" + DepartmentId + "' AND EmpStatus in ('1','8') AND EmpAttCard !=''", dtEmpInfo);
                }
                else
                    sqlDB.fillDataTable("select cs.EmpId,Convert(int,Right(cs.EmpCardNo,LEN(cs.EmpCardNo)-7)) as EmpCardNo,cs.EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate,SftId,EmpAttCard as RealProximityNo,GId,DptId,DsgId,EmpDutyType,EmpAttCard,isnull(IsDelivery,0) IsDelivery from v_Personnel_EmpCurrentStatus cs left join Personnel_EmpSeparation sp on cs.EmpId=sp.EmpId and cs.EmpStatus=sp.SeparationType  where  cs.IsActive=1 and CompanyId='" + CompnayId + "' and cs.EmpCardNo Like '%" + EmpCardNo + "%' AND(EmpStatus in ('1', '8') or sp.EffectiveDate >= '" + attDate + "') ", dtEmpInfo);
                //sqlDB.fillDataTable("select EmpId,Convert(int,Right(EmpCardNo,LEN(EmpCardNo)-7)) as EmpCardNo,EmpTypeId,Format(EmpJoiningDate,'dd-MM-yyyy')as EmpJoiningDate,SftId,EmpAttCard as RealProximityNo,GId,DptId,DsgId,EmpDutyType,EmpAttCard,isnull(IsDelivery,0) IsDelivery from v_Personnel_EmpCurrentStatus where  IsActive=1 and CompanyId='" + CompnayId + "' and EmpCardNo Like '%" + EmpCardNo + "%' AND EmpStatus in ('1','8') AND EmpAttCard !=''", dtEmpInfo);
                return dtEmpInfo;
            }
            catch { return null; }
        }

        // this method for delete current or exeisting attendance of selected date 
        public static void delete_Attendance(string CompanyId, string DepartmentId, string AttDate, bool ForAllEmployee, string EmpId)
        {
            try
            {
                SqlCommand cmd;
                if (!ForAllEmployee) // for all employee.that can be for all employee or just one selected employee
                    cmd = new SqlCommand("delete from tblAttendanceRecord where CompanyId='" + CompanyId + "' and AttDate='" + AttDate + "' AND EmpId='" + EmpId + "' AND  (AttManual is null  or (ODID is not null or ODID='')) ", sqlDB.connection);
                else if (DepartmentId.Equals("0"))
                    cmd = new SqlCommand("delete from tblAttendanceRecord where CompanyId='" + CompanyId + "' and AttDate='" + AttDate + "' AND (AttManual is null  or (ODID is not null or ODID='')) ", sqlDB.connection);
                else  // for specific one employee
                    cmd = new SqlCommand("delete from tblAttendanceRecord where CompanyId='" + CompanyId + "' and AttDate='" + AttDate + "' AND DptId='" + DepartmentId + "' AND (AttManual is null  or (ODID is not null or ODID='')) ", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        // for load all attendance data after attendance processing 
        public static DataTable Load_Process_AttendanceData(string CompanyId, string DepartmentId, string AttDate, bool ForAllEmployee, string EmpId)
        {
            try
            {
                DataTable dt = new DataTable();
                if (!ForAllEmployee)
                {
                    sqlDB.fillDataTable("select Convert(varchar(3),left(ecs.EmpCardNo,LEN(ecs.EmpCardNo)-4))+' '+Convert(varchar(10),right(ecs.EmpCardNo,LEN(ecs.EmpCardNo)-9)) as EmpCardNo,ecs.EmpName,ecs.DptName,ecs.DsgName, case when ODID is not null and ODID<>0 then  atd.ATTStatus+' (OD)' else  atd.ATTStatus end as  ATTStatus, " +
                   "   atd.InHour+':'+atd.InMin+':'+atd.InSec as Intime,atd.OutHour+':'+atd.OutMin+':'+atd.OutSec as Outtime,Format(ATTDate,'dd-MMM-yyyy') as ATTDate " +
                   " from " +
                   " tblAttendanceRecord as atd inner join " +
                   " v_Personnel_EmpCurrentStatus ecs on " +
                   " atd.EmpId=ecs.EmpId and ecs.IsActive=1 AND atd.ATTDate='" + AttDate + "' and atd.CompanyId='" + CompanyId + "' AND ( ecs.EmpCardNo  Like '%"+ EmpId + "%' or Convert(varchar(3),left(ecs.EmpCardNo,LEN(ecs.EmpCardNo)-4))+' '+Convert(varchar(10),right(ecs.EmpCardNo,LEN(ecs.EmpCardNo)-9)) Like '%" + EmpId + "%')", dt);
                }
                else if (DepartmentId.Equals("0"))
                    sqlDB.fillDataTable("select Convert(varchar(3),left(ecs.EmpCardNo,LEN(ecs.EmpCardNo)-4))+' '+Convert(varchar(10),right(ecs.EmpCardNo,LEN(ecs.EmpCardNo)-9)) as EmpCardNo,ecs.EmpName,ecs.DptName,ecs.DsgName, case when ODID is not null and ODID<>0 then  atd.ATTStatus+' (OD)' else  atd.ATTStatus end as  ATTStatus, " +
                        "  atd.InHour+':'+atd.InMin+':'+atd.InSec as Intime,atd.OutHour+':'+atd.OutMin+':'+atd.OutSec as Outtime,Format(ATTDate,'dd-MMM-yyyy') as ATTDate " +
                        " from " +
                        " tblAttendanceRecord as atd inner join " +
                        " v_Personnel_EmpCurrentStatus ecs on " +
                        " atd.EmpId=ecs.EmpId and ecs.IsActive=1 and atd.CompanyId='" + CompanyId + "' AND atd.ATTDate='" + AttDate + "' order by ecs.DptId,ecs.CustomOrdering", dt);
                else
                    sqlDB.fillDataTable("select Convert(varchar(3),left(ecs.EmpCardNo,LEN(ecs.EmpCardNo)-4))+' '+Convert(varchar(10),right(ecs.EmpCardNo,LEN(ecs.EmpCardNo)-9)) as EmpCardNo,ecs.EmpName,ecs.DptName,ecs.DsgName, case when ODID is not null and ODID<>0 then  atd.ATTStatus+' (OD)' else  atd.ATTStatus end as  ATTStatus, " +
                    "  atd.InHour+':'+atd.InMin+':'+atd.InSec as Intime,atd.OutHour+':'+atd.OutMin+':'+atd.OutSec as Outtime,Format(ATTDate,'dd-MMM-yyyy') as ATTDate " +
                    " from " +
                    " tblAttendanceRecord as atd inner join " +
                    " v_Personnel_EmpCurrentStatus ecs on " +
                    " atd.EmpId=ecs.EmpId and ecs.IsActive=1 AND atd.ATTDate='" + AttDate + "' and atd.CompanyId='" + CompanyId + "' AND atd.DptId='" + DepartmentId + "' order by ecs.CustomOrdering", dt);
                return dt;

            }
            catch { return null; }
        }

        public static DataTable Load_Process_AttendanceData(string AttDate)
        {
            try
            {
                DataTable dt = new DataTable();

                sqlDB.fillDataTable("select Convert(varchar(3),left(ecs.EmpCardNo,LEN(ecs.EmpCardNo)-4))+' '+Convert(varchar(10),right(ecs.EmpCardNo,LEN(ecs.EmpCardNo)-9)) as EmpCardNo,ecs.EmpName,ecs.DptName,ecs.DsgName, atd.ATTStatus, " +
                "   atd.InHour+':'+atd.InMin+':'+atd.InSec as Intime,atd.OutHour+':'+atd.OutMin+':'+atd.OutSec as Outtime,Format(ATTDate,'dd-MMM-yyyy') as ATTDate " +
                " from " +
                " tblAttendanceRecord as atd inner join " +
                " v_Personnel_EmpCurrentStatus ecs on " +
                " atd.EmpId=ecs.EmpId AND atd.ATTDate='" + AttDate + "' AND AttManual ='mc'", dt);                            
                return dt;

            }
            catch { return null; }
        }

        public static void NotCountableAttendanceLog(string EmpId,string Reason,string AttDate)
        {
            

            SqlCommand cmd = new SqlCommand("insert into tblAttendance_NotCountableLogRecord values('"+EmpId+"','"+Reason+"','"+AttDate+"')",sqlDB.connection);
            cmd.ExecuteNonQuery();
        }

        public static void LoadAttendanceMissingLog(GridView GridView)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select pecs.EmpCardNo,pecs.EmpName,pecs.DptName,pecs.DsgName,alog.AttDate,alog.Reason  from tblAttendance_NotCountableLogRecord alog " +
                                    " inner join "+
                                    " v_Personnel_EmpCurrentStatus as pecs on "+
                                    " alog.EmpId=pecs.EmpId",dt);
                GridView.DataSource = dt;
                GridView.DataBind();
            }
            catch { }
        }

        public static string [] LoadShift_Information(string EmpId,bool IsRegularDuty_Type,string ShiftId,string AttDate)
        {
            try
            {
                DataTable dt = new DataTable();
                string[] Gt_RosterInfo = new string[11];

                if (IsRegularDuty_Type)
                {
                    sqlDB.fillDataTable("select "+ ShiftId + " as SftId, SftStartTimeIndicator, SftEndTimeIndicator, SftStartTime, SftEndTime, SftAcceptableLate, AcceptableTimeAsOT, StartPunchCountTime, OverLateTime, AcceptableHalfDay  from HRD_SpecialTimetable where StartDate <= '"+ AttDate + "' and EndDate >= '"+ AttDate + "'", dt = new DataTable());
                    if(dt==null || dt.Rows.Count==0)
                    sqlDB.fillDataTable("select SftId,SftStartTimeIndicator,SftEndTimeIndicator,SftStartTime,SftEndTime,SftAcceptableLate,AcceptableTimeAsOT,StartPunchCountTime,OverLateTime,AcceptableHalfDay  from HRD_Shift where SftId ='" + ShiftId + "'", dt = new DataTable());
                }
                else
                    sqlDB.fillDataTable("select SftId,SftStartTimeIndicator,SftEndTimeIndicator,SftStartTime,SftEndTime,SftAcceptableLate,AcceptableTimeAsOT,StartPunchCountTime,IsWeekend,OverLateTime,AcceptableHalfDay  from v_ShiftTransferInfoDetails where SDate ='" + AttDate + "' AND EmpId='" + EmpId + "'", dt);
                Gt_RosterInfo[0] = (dt.Rows.Count > 0) ? dt.Rows[0]["SftId"].ToString() : "0";
                Gt_RosterInfo[1] = dt.Rows[0]["SftStartTimeIndicator"].ToString();
                Gt_RosterInfo[2] = dt.Rows[0]["SftEndTimeIndicator"].ToString();
                Gt_RosterInfo[3] = dt.Rows[0]["SftStartTime"].ToString();
                Gt_RosterInfo[4] = dt.Rows[0]["SftEndTime"].ToString();
                Gt_RosterInfo[5] = dt.Rows[0]["AcceptableTimeAsOT"].ToString();
                Gt_RosterInfo[6] = dt.Rows[0]["SftAcceptableLate"].ToString();
                Gt_RosterInfo[7] = dt.Rows[0]["StartPunchCountTime"].ToString();
                Gt_RosterInfo[8] = (IsRegularDuty_Type) ? "False" : dt.Rows[0]["IsWeekend"].ToString();
                Gt_RosterInfo[9] =  dt.Rows[0]["OverLateTime"].ToString();
                Gt_RosterInfo[10] = dt.Rows[0]["AcceptableHalfDay"].ToString();
                return Gt_RosterInfo;
            }
            catch { return null; }
        }

        public static string [] Counting_BreakTime(string AttDate, string EmpCardNo, string BreakStratTime,string BreakEndTime)
        {
            string[] CountBreakTime = { "00:00:00", "00:00:00" };
            try
            {               
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select distinct Badgenumber, FORMAT(CHECKTIME,'HH:mm:ss') as PunchTime from v_CHECKINOUT where CONVERT(date,CHECKTIME)='" + AttDate + "' AND FORMAT(CHECKTIME,'HH:mm:ss')>='"+BreakStratTime+ "' AND FORMAT(CHECKTIME,'HH:mm:ss')<='" + BreakEndTime + "' AND Badgenumber Like '%" + EmpCardNo + "'  order by PunchTime", dt);
                if (dt.Rows.Count>0)
                {
                    if ((Convert.ToDateTime(dt.Rows[0]["PunchTime"].ToString()).Hour== Convert.ToDateTime(dt.Rows[dt.Rows.Count-1]["PunchTime"].ToString()).Hour) &&
                        (Convert.ToDateTime(dt.Rows[0]["PunchTime"].ToString()).Minute == Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1]["PunchTime"].ToString()).Minute))
                    {
                        CountBreakTime[0] = dt.Rows[0]["PunchTime"].ToString();
                        CountBreakTime[1] = "00:00:00";
                    }
                    else
                    {
                        CountBreakTime[0] = dt.Rows[0]["PunchTime"].ToString();
                        CountBreakTime[1] = dt.Rows[dt.Rows.Count-1]["PunchTime"].ToString();
                    }                 
                }
                return CountBreakTime;
            }
            catch { return CountBreakTime ; }
        }
    }
}