using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using adviitRuntimeScripting;
using ComplexScriptingSystem;

namespace SigmaERP.classes
{
    public class Employee
    {
      public static DataTable dt;
        
        public static void LoadEmployeeType(DropDownList dl)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select EmpTypeId,EmpType From HRD_EmployeeType", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpType";
                dl.DataValueField = "EmpTypeId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }
        public static void LoadEmpCardNoWithName(DropDownList dl,string EmpType)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,EmpId From v_Personnel_EmpCurrentStatus where EmpTypeId=" + EmpType + " and EmpStatus in ('1','8') Group by EmpCardNo,EmpId,EmpName order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }
      
        public static void LoadEmpCardNoWithNameByCompany(DropDownList dl, string CompanyId, string EmpType,string txt)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,EmpId From v_Personnel_EmpCurrentStatus where CompanyId='" + CompanyId + "' and EmpTypeId in(" + EmpType + ") and EmpStatus in ('1','8')  Group by EmpCardNo,EmpId,EmpName order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(txt, "0"));
            }
            catch { }
        }
        public static void LoadEmpCardNoWithNameByCompanyForPf(DropDownList dl, string CompanyId)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,EmpId From v_Personnel_EmpCurrentStatus where CompanyId='" + CompanyId + "'  and EmpStatus in ('1','8') and PfMember=1  Group by EmpCardNo,EmpId,EmpName order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem("All", "0"));
            }
            catch { }
        }
        public static void LoadEmpCardNoWithNameByCompanyRShift(DropDownList dl, string CompanyId)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,EmpId From v_Personnel_EmpCurrentStatus where CompanyId='" + CompanyId + "' AND EmpStatus in ('1','8') Group by EmpCardNo,EmpId,EmpName order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }

        public static void LoadEmpCardNoWithNameByCompanyRShift(DropDownList dl, string CompanyId,string ShiftId)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,EmpId From v_Personnel_EmpCurrentStatus where CompanyId='" +CompanyId+ "' AND SftId in ("+ShiftId+") AND EmpStatus in ('1','8') Group by EmpCardNo,EmpId,EmpName order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }

        public static void LoadEmpCardNoWithNameByCompanyRShiftForSeperationEmp(DropDownList dl, string CompanyId,string SeperationMonthYear)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,EmpId From v_Personnel_EmpSeparation where CompanyId='" + CompanyId + "'  AND EmpStatus not in ('1','8') AND YearMonth='"+SeperationMonthYear+"' Group by EmpCardNo,EmpId,EmpName order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }

        public static void LoadEmpCardNo(DropDownList dl, string EmpType, string CompanyId)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,EmpId From v_Personnel_EmpCurrentStatus where EmpTypeId=" + EmpType + " and EmpStatus in ('1','8') and CompanyId='"+CompanyId+"' Group by EmpCardNo,EmpId,EmpName order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }
        public static void LoadEmpCardNo(DropDownList dl, string EmpType)// For_DeltaAtt
        {
            try
            {
                dt = new DataTable();
               sqlDB.fillDataTable("Select Max(SN) as SN, (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,EmpId From v_Personnel_EmpCurrentStatus where EmpTypeId=" + EmpType + " and EmpStatus in ('1','8')  Group by EmpCardNo,EmpId,EmpName order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }
        public static void LoadEmpCardNo(DropDownList dl, string EmpType, string CompanyId, string NewCardNo)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,EmpId From v_Personnel_EmpCurrentStatus where EmpTypeId=" + EmpType + " and EmpStatus in ('1','8') and CompanyId='" + CompanyId + "' Group by EmpCardNo,EmpId,EmpName order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(NewCardNo, "0"));
            }
            catch { }
        }

        public static void LoadEmpCardNoForPayroll(DropDownList dl, string CompanyId)// For payroll Entry
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,EmpId From v_Personnel_EmpCurrentStatus where CompanyId=" + CompanyId + " and EmpStatus in ('1','8')  Group by EmpCardNo,EmpId,EmpName order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "SN";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }
        public static void LoadEmpCardNoForPayrollwithEmpId(DropDownList dl, string CompanyId)// For payroll Entry
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select EmpId, (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo From Personnel_EmployeeInfo where CompanyId=" + CompanyId + " and EmpStatus in ('1','8')   order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }
        public static DataTable loadEmployeeSalary(string EmpID)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("select EmpPresentSalary,BasicSalary from Personnel_EmpCurrentStatus where IsActive=1 and EmpId='"+ EmpID + "'", dt);
                return dt;
            }
            catch { return null; }
        }
        public static void LoadEmpCardNoWithName(DropDownList dl, string EmpType, string InstallmentType)
        {
            try
            {
                dt = new DataTable();
                if (InstallmentType.Equals("1st Installment")) sqlDB.fillDataTable("Select Distinct (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,EmpId From v_Leave_LeaveApplication where ShortName='M/L' AND IsProcessessed='1' AND EmpTypeId=" + EmpType + " AND (FirstInstallmentSignature='false' OR FirstInstallmentSignature is null)  order by EmpCardNo ", dt);
                else sqlDB.fillDataTable("Select Distinct (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,EmpId From v_Leave_LeaveApplication where ShortName='M/L' AND IsProcessessed='1' AND EmpTypeId=" + EmpType + " AND FirstInstallmentSignature='true' AND (SecondInstallmentSignature='false' OR SecondInstallmentSignature is null)  order by EmpCardNo ", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }

        public static void LoadEmpCardNoWithNameAll(DropDownList dl, string EmpType)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,EmpId From v_Personnel_EmpCurrentStatus where EmpTypeId=" + EmpType + "  Group by EmpCardNo,EmpId,EmpName order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }

       

        public static void loadEmpPunismentType(DropDownList dl)
        {
            try
            {
                SQLOperation.selectBySetCommandInDatatable("select PtId,PtName from HRD_PunishmentType ",dt=new DataTable (),sqlDB.connection);
                dl.DataSource = dt;
                dl.DataValueField = "PtId";
                dl.DataTextField = "PtName";
                dl.DataBind();
              
            }
            catch { }
        }
        public static void LoadResignedEmpCardNo(DropDownList dl,string EmpType)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, EmpId,EmpCardNo From Personnel_EmpCurrentStatus where EmpTypeId="+EmpType+" and EmpStatus in(4,7) Group by EmpId,EmpCardNo Order by EmpCardNo ", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }
        public static void LoadEmpStatus(DropDownList dl)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select EmpStatus,EmpStatusName From HRD_EmpStatus order by EmpStatus", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpStatusName";
                dl.DataValueField = "EmpStatus";
                dl.DataBind();
               
            }
            catch { }
        }
        public static void LoadEmpCardIncPro(DropDownList dl, string TypeOfChange, string CompanyId)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, (Convert(nvarchar(50),SubString(EmpCardNo,8,16))+' '+EmpName) as EmpCardNo,EmpId From v_Promotion_Increment where TypeOfChange='" + TypeOfChange + "' and CompanyId='"+CompanyId+"'  Group by EmpCardNo,EmpId,EmpName order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                if (dt.Rows.Count > 1)
                {
                    dl.Items.Insert(0, new ListItem("All", "0"));
                }
            }
            catch { }
        }
        public static void LoadEmpCardNo_ForSeperation(DropDownList dl, string CompanyId)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, (Convert(nvarchar(50),EmpCardNo)+' '+EmpName) as EmpCardNo,(EmpId+'|'+(Convert(nvarchar(50),EmpCardNo))+'|'+convert(varchar(2),EmpTypeId)) as EmpId From v_Personnel_EmpCurrentStatus where EmpStatus in ('1','8') and CompanyId='" + CompanyId + "' Group by (Convert(nvarchar(50),EmpCardNo)+' '+EmpName),(EmpId+'|'+(Convert(nvarchar(50),EmpCardNo))+'|'+convert(varchar(2),EmpTypeId)) order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "EmpId";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }
        public static void CheckToPermanent(string CompanyId) 
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("select ei.EmpId, Type, EmpJoiningDate, convert(date, DateADD(month,6,EmpJoiningDate)) as ParmanentDate from Personnel_EmployeeInfo ei inner join Personnel_EmpCurrentStatus cs on ei.EmpId=cs.EmpId and cs.IsActive=1 where ei.CompanyId='" + CompanyId + "'and Type<>'Permanent' and cs.EmpID not in(select distinct EmpID from nf_PermanentNotification) and DateADD(month,5,EmpJoiningDate)<=getdate()", dt);
                if (dt.Rows.Count > 0)
                {
                    DataTable dtAdminList = new DataTable();
                    dtAdminList = commonTask.getAdminListForNotification("pn");

                    //SqlCommand cmd = new SqlCommand("update Personnel_EmployeeInfo set Type='Permanent' where CompanyId='" + CompanyId + "' and EmpId in (select EmpId from Personnel_EmployeeInfo where CompanyId='" + CompanyId + "'and Type<>'Permanent' and DateADD(month,6,EmpJoiningDate)<=getdate())", sqlDB.connection);
                    //cmd.ExecuteNonQuery();
                    SqlCommand cmd;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtAdminList.Rows.Count; i++)
                        {
                            try
                            {
                             cmd = new SqlCommand("insert into nf_PermanentNotification (EmpID,ActivedDate,ActivedDatetime,EmpSeen,AdminID,AdminSeen) values('" + dt.Rows[i]["EmpId"].ToString()
                            + "','" + dt.Rows[i]["ParmanentDate"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',0,'" + dtAdminList.Rows[j]["AdminID"].ToString() + "',0)", sqlDB.connection);
                                cmd.ExecuteNonQuery();
                            } catch (Exception ex) { }
                            
                        }                        
                    }
                }
            }
            catch (Exception ex) { }
        }
        public static void CheckBirthDayForNotification(string CompanyId)
        {
            try
            {
                dt = new DataTable();
                string sqlCmd= @"select Empid, convert(varchar(4), YEAR(getdate()))+'-'+convert(varchar, format( DateOfBirth,'MM-dd')) as BirthDay from v_EmployeeDetails  Where CompanyID='"+CompanyId
                    +"' and  DateOfBirth is not null and EmpStatus in (1, 8) and convert(varchar(4), YEAR(getdate())) +'-' + convert(varchar, format(DateOfBirth, 'MM-dd')) <= getdate() and not Empid+'_'+ convert(varchar(4), YEAR(getdate())) in(select EmpID+'_'+ convert(varchar(4), YEAR(getdate())) from nf_BirthdayNotification where Year(BirthDay)=YEAR(getdate()))";
                sqlDB.fillDataTable(sqlCmd, dt);
                if (dt.Rows.Count > 0)
                {
                    DataTable dtAdminList = new DataTable();
                    dtAdminList = commonTask.getAdminListForNotification("pn");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtAdminList.Rows.Count; i++)
                        {

                            CRUD.Execute("insert into nf_BirthdayNotification (EmpID,BirthDay,EmpSeen,AdminID,AdminSeen) values('" + dt.Rows[i]["EmpId"].ToString()
                            + "','" + dt.Rows[i]["BirthDay"].ToString() + "',0,'" + dtAdminList.Rows[j]["AdminID"].ToString() + "',0)", sqlDB.connection);
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
        private static string getAdminId(string EmpId,string CompanyId) 
        {
            try { 
                  dt = new DataTable();
                sqlDB.fillDataTable("select EmpId, Type, EmpJoiningDate, DateADD(month,6,EmpJoiningDate) as ParmanentDate from Personnel_EmployeeInfo where CompanyId='" + CompanyId + "'and Type<>'Permanent' and DateADD(month,6,EmpJoiningDate)<=getdate()", dt);
                if (dt.Rows.Count > 0)
                    return dt.Rows[0]["EmpId"].ToString();
                else
                    return ""; 
            }
            catch { return ""; }
        }
        public static void earnLeaveActivation(string EmpId, string CompanyId)
        {
            try
            {
                if (EmpId != "")
                    EmpId = " and EmpID='" + EmpId + "'";
                dt = new DataTable();
                sqlDB.fillDataTable("select EmpId,convert(varchar(10),EmpJoiningDate,120) as EmpJoiningDate from v_EmployeeDetails where CompanyId='" + CompanyId + "' " + EmpId + " and IsActive=1  and EmpId not in(select EmpId from EarnLeave_Activationlog Where IsActive=1)", dt);

                DateTime CurrentDate =DateTime.Parse(ServerTimeZone.GetBangladeshNowDate("yyyy-MM-dd"));
                //DateTime CurrentDate =DateTime.Parse("2023-01-01");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DateTime empJoiningDate = DateTime.Parse(dt.Rows[i]["EmpJoiningDate"].ToString());
                    DateTime earnLeaveStandardDate = DateTime.Parse(empJoiningDate.Year.ToString() + "-01-01");// 1st January
                    if (earnLeaveStandardDate < empJoiningDate)
                    {
                        empJoiningDate = earnLeaveStandardDate.AddYears(1);
                    }
                    int count = 0;
                    for (DateTime Date = empJoiningDate; Date <= CurrentDate; Date = Date.AddMonths(1))
                    {
                        if (Date.ToString("MM").Equals("01"))
                            count++;
                        if (count > 1)
                        {
                            saveEarnLeaveActivation(dt.Rows[i]["EmpID"].ToString(),Date.ToString("yyyy-MM")+"-01");
                            break;
                        }
                    }
                }
            }
            catch { }
        }
        public static void saveEarnLeaveActivation(string EmpID,string ActiveFrom)
        {
            try {
                CRUD.Execute(@"INSERT INTO [dbo].[EarnLeave_Activationlog]
           ([EmpID]
           ,[ActiveFrom]
           ,[ActionTime]
           ,[IsActive]) VALUES
           ('"+ EmpID + @"','"+ ActiveFrom + "','"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"',1)", sqlDB.connection);
            } catch(Exception ex) { }
        }
        //public static void getValidEmpCardNo(string )
    }
}