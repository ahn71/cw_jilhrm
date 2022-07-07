using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data;
using ComplexScriptingSystem;
using adviitRuntimeScripting;
using System.IO;
using System.Web.UI.WebControls;

namespace SigmaERP.classes
{
    public class Payroll
    {
        public static void loadMonthIdByCompany(DropDownList ddlMonthList, string CompanyId)
        {
            try
            {
                DataTable dt=new DataTable ();
                sqlDB.fillDataTable("select  Format(FromDate,'MMM-yyyy') as YearMonth,format(FromDate,'yyyy-MM')+'-01' as MonthYear from tblMonthSetup where CompanyId='"+ CompanyId + "' order by format(FromDate,'yyyy-MM')+'-01' desc", dt);
                ddlMonthList.DataSource = dt;
                ddlMonthList.DataValueField = "MonthYear";
                ddlMonthList.DataTextField = "YearMonth";
                ddlMonthList.DataBind();
                ddlMonthList.Items.Insert(0,new ListItem (" ","0"));
            }
            catch { }
        }
        public static void loadBonusType(DropDownList ddlBonusType, string CompanyId)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select BID, case when b.RId=1 then BonusName else BonusName+'('+ r.RName+')' end  as BonusName from Payroll_BonusSetup b left join HRD_Religion r on b.RId=r.RId where CompanyId='"+ CompanyId + "' order by convert(varchar(10),CalculationDate,120) desc", dt);
                ddlBonusType.DataSource = dt;
                ddlBonusType.DataTextField = "BonusName";
                ddlBonusType.DataValueField = "BID";
                ddlBonusType.DataBind();
                ddlBonusType.Items.Insert(0, new ListItem("", "0"));
            }
            catch{ }
        }
        public static void loadBonusTypeWithRID(DropDownList ddlBonusType, string CompanyId)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select convert(varchar, BID)+'_'+convert(varchar,b.RID) as BID, case when b.RId=1 then BonusName else BonusName+'('+ r.RName+')' end  as BonusName from Payroll_BonusSetup b left join HRD_Religion r on b.RId=r.RId where CompanyId='" + CompanyId + "' order by convert(varchar(10),CalculationDate,120) desc", dt);
                ddlBonusType.DataSource = dt;
                ddlBonusType.DataTextField = "BonusName";
                ddlBonusType.DataValueField = "BID";
                ddlBonusType.DataBind();
                ddlBonusType.Items.Insert(0, new ListItem("", "0"));
            }
            catch { }
        }

        public static void loadBonusTypeByCompany(DropDownList ddlBonusType, string CompanyId)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select Distinct BonusType,BId,GenerateDate  from v_Payroll_YearlyBonusSheet where CompanyId='" + CompanyId + "' order by GenerateDate", dt);
                ddlBonusType.DataSource = dt;
                ddlBonusType.DataTextField = "BonusType";
                ddlBonusType.DataValueField = "BId";
                ddlBonusType.DataBind();
                ddlBonusType.Items.Insert(0, new ListItem("", "0"));
            }
            catch { }
        }

        public static string getSftIdList(DropDownList ddlSftList)
        {
            try
            {
                string setPredicate = "";
                for (byte b = 0; b < ddlSftList.Items.Count; b++)
                {
                    setPredicate +=ddlSftList.Items[b].Value.ToString() +",";
                }

                setPredicate = setPredicate.Remove(setPredicate.LastIndexOf(','));
                return setPredicate;
            }
            catch { return " "; }

        }

        public static string getCompanyList(DropDownList ddlCompanyList)
        {
            try
            {
                string setPredicate = "";
                for (byte b = 0; b < ddlCompanyList.Items.Count; b++)
                {
                    setPredicate += ddlCompanyList.Items[b].Value.ToString() + ",";
                }

                setPredicate = setPredicate.Remove(setPredicate.LastIndexOf(','));
                return setPredicate;
            }
            catch { return " "; }

        }

        public static DataTable Load_Payroll_AllowanceCalculationSetting()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select * from Payroll_AllowanceCalculationSetting",dt);
                return dt;
            }
            catch { return null; }
        }

        public static DataTable Load_HRD_AllownceSetting()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select * from HRD_AllownceSetting", dt);
                return dt;
            }
            catch { return null; }
        }

        public static DataTable SalaryInfo(string SN)
        {
            try
            {
                DataTable dt = new DataTable();
                dt= CRUD.ExecuteReturnDataTable("select IncrementAmount,EmpName,DateofUpdate,SalaryCount, EmpAccountNo,BankId,GrdName,EmpJoinigSalary,EmpPresentSalary,BasicSalary,MedicalAllownce,HouseRent,EmpTypeId,EmpType," +
                    "ConvenceAllownce,FoodAllownce,AttendanceBonus,PfMember,PfDate,PFAmount,HouseRent_Persent,Medical,PF_Persent,SalaryType,NightAllownce,OverTime,OthersAllownce,DormitoryRent,IncomeTax,isnull(AllowToEdit,0) as AllowToEdit from v_EmployeeDetails where SN=" + SN + "", sqlDB.connection);
                return dt;
             
            }
            catch { return null; }
        }
        public static bool Office_IsGarments()
        {
            try
            {
                DataTable dt = new DataTable();
                SQLOperation.selectBySetCommandInDatatable("select * from Payroll_Office_IsGarments", dt, sqlDB.connection);
                
                if (bool.Parse(dt.Rows[0]["IsGarments"].ToString().Trim()) == true)return true;
                
                else return false;
             
            }
            catch { return false; }
        }
    }
}