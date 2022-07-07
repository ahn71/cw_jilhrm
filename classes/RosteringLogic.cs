using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using adviitRuntimeScripting;
using System.Web.UI.WebControls;

namespace SigmaERP.classes
{
    public class RosteringLogic
    {
        SqlCommand cmd;
        

        public static void LoadShiftList_ForNotRegular(string CompanyId,DropDownList ddl)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select SftId,SftName from HRD_Shift where ForRegular='False' AND CompanyId='" + CompanyId + "' ", dt);
                ddl.DataTextField = "SftName";
                ddl.DataValueField = "SftId";
                ddl.DataSource = dt;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("","0"));
            }
            catch { }
        }

        public static void loadDepartmentListByCompany(DropDownList ddl, string CompanyId)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select distinct dptName, DptId,convert(int,DptCode) from v_Personnel_EmpCurrentStatus where DutyType='Roster' AND CompanyId='" + CompanyId + "' order by convert(int,DptCode)", dt);         
                ddl.DataValueField = "DptId";
                ddl.DataTextField = "DptName";
                ddl.DataSource = dt;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("", "0"));
            }
            catch { }
        }

        public static void loadSubDepartment_ByDeparmemt(string DepartmentId,DropDownList ddl)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select distinct GId,GName,GroupOrdering From v_Personnel_EmpCurrentStatus where DptId='" + DepartmentId + "' order by GroupOrdering", dt);
                ddl.DataTextField = "GName";
                ddl.DataValueField = "GId";
                ddl.DataSource = dt;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("", "0"));

            }
            catch { }
        }

        public static void LoadAssignedShiftList(string DepartmentId,string SubdepartmentId,string CompanyId,DropDownList ddl)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable(" select sti.STId,hs.SftName+'-----'+CONVERT(varchar, Format(sti.TFromDate,'MMM-dd-yyyy')) +' '+CONVERT(varchar,Format(sti.TToDate,'MMM-dd-yyyy')) as ShiftTitle from  " +
                    " HRD_Shift as hs inner join ShiftTransferInfo as sti on hs.SftId=sti.SftId  AND sti.DptId='" + DepartmentId + "' AND sti.GId='" + SubdepartmentId + "' AND sti.CompanyId='" + CompanyId + "' order by sti.STId desc", dt);
                ddl.DataTextField = "ShiftTitle";
                ddl.DataValueField = "STId";
                ddl.DataSource = dt;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("", "0"));
            }
            catch { }
        }
        public static void LoadAssignedShiftList(string CompanyId, DropDownList ddl)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable(" select sti.STId,hs.SftName+'-----'+CONVERT(varchar, Format(sti.TFromDate,'MMM-dd-yyyy')) +' '+CONVERT(varchar,Format(sti.TToDate,'MMM-dd-yyyy')) as ShiftTitle from  " +
                    " HRD_Shift as hs inner join ShiftTransferInfo as sti on hs.SftId=sti.SftId   AND sti.CompanyId='" + CompanyId + "' order by sti.STId desc", dt);
                ddl.DataTextField = "ShiftTitle";
                ddl.DataValueField = "STId";
                ddl.DataSource = dt;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("", "0"));
            }
            catch { }
        }

        public static void LoadAssignedShiftList_BySearchTToDate(string DepartmentId, string SubdepartmentId, string CompanyId, DropDownList ddl,string TToDate)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable(" select sti.STId,hs.SftName+'     '+CONVERT(varchar, Format(sti.TFromDate,'MMM-dd-yyyy')) +' '+CONVERT(varchar,Format(sti.TToDate,'MMM-dd-yyyy')) as ShiftTitle from  " +
                    " HRD_Shift as hs inner join ShiftTransferInfo as sti on hs.SftId=sti.SftId  AND sti.DptId='" + DepartmentId + "' AND sti.GId='" + SubdepartmentId + "' AND sti.CompanyId='" + CompanyId + "' AND TToDate >= '"+TToDate+"'", dt);
                ddl.DataTextField = "ShiftTitle";
                ddl.DataValueField = "STId";
                ddl.DataSource = dt;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("", "0"));
            }
            catch { }
        }

        public static void loadDepartmentListByCompany(DropDownList ddl, string CompanyId, string DptId) // For Admin
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select distinct dptName, DptId,convert(int,DptCode) from v_Personnel_EmpCurrentStatus where DutyType='Roster' AND CompanyId='" + CompanyId + "' and DptId=" + DptId + " ", dt);
                ddl.DataValueField = "DptId";
                ddl.DataTextField = "DptName";
                ddl.DataSource = dt;
                ddl.DataBind();
            }
            catch { }
        }
        public static void LoadRosterShift(string DepartmentId, string CompanyId, DropDownList ddl)
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable(" select Distinct SftId,SftName from v_tblAttendanceRecord where ForRegular=0 and CompanyId='" + CompanyId + "' and DptId='" + DepartmentId + "'", dt);
            ddl.DataTextField = "SftName";
            ddl.DataValueField = "SftId";
            ddl.DataSource = dt;
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("All", "0"));
        }
    }
}