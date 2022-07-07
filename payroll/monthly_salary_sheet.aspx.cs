using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComplexScriptingSystem;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SigmaERP.payroll
{
    public partial class monthly_salary_sheet : System.Web.UI.Page
    {
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                classes.commonTask.loadEmpTypeInRadioButtonList(rbEmpTypeList,"hasMnu");
                classes.commonTask.loadEmpTypeInRadioButtonList(rblSelectEmployeeType);
                classes.commonTask.loadDivision(ddlDivisionName);
                loadMonthName();
            }
        }

        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='monthly_salary_sheet.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                        {
                            btnPreview.CssClass = "";
                            btnPreview.Enabled = false;
                        }
                    }
                    btnBankSalaryList.Visible = false;
                }
            }
            catch { }
        }

        private void loadMonthName()
        {
            try
            {
                SQLOperation.selectBySetCommandInDatatable("select distinct (Convert(nvarchar(50),v_MonthlySalarySheet.Month)+'-'+v_MonthlySalarySheet.Year) as Month,MonthYear,Year from v_MonthlySalarySheet order by Year desc, MonthYear ", dt = new DataTable(), sqlDB.connection);
                ddlMonthID.DataValueField = "Month";
                ddlMonthID.DataTextField = "Month";
                ddlMonthID.DataSource = dt;
                ddlMonthID.DataBind();
                ddlMonthID.Items.Insert(0, new ListItem(" ", " "));

            }
            catch (Exception ex)
            {

            }
        }



        protected void rbEmpTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!rbEmpTypeList.SelectedValue.ToString().Equals("50"))
            {
                ddlCardNo.Enabled = false;
                rblSelectEmployeeType.Visible = false;
            }
            else
            {
                ddlCardNo.Enabled = true;
                rblSelectEmployeeType.Visible = true;
            }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            if (rbEmpTypeList.SelectedItem.ToString().Equals("Staff")) StaffSalarySheet("CSL");  // CSL=Company Salary List 
            else if (rbEmpTypeList.SelectedValue.ToString().Equals("50")) { }
            else WorkerSalarySheet();
        }

        private void WorkerSalarySheet()
        {
            try
            {

                string setPredicate = "";
             //   string[] getMonth = ddlMonthID.SelectedValue.Split('-');
                for (byte b = 0; b < lstSelected.Items.Count; b++)
                {

                    if (b == 0 && b == lstSelected.Items.Count - 1)
                    {
                        setPredicate = "in('" + lstSelected.Items[b].Value + "')";
                    }
                    else if (b == 0 && b != lstSelected.Items.Count - 1)
                    {
                        setPredicate += "in ('" + lstSelected.Items[b].Value + "'";
                    }
                    else if (b != 0 && b == lstSelected.Items.Count - 1)
                    {
                        setPredicate += ",'" + lstSelected.Items[b].Value + "')";
                    }
                    else setPredicate += ",'" + lstSelected.Items[b].Value + "'";
                }

                if (!ddlCardNo.Enabled)
                    sqlDB.fillDataTable("SELECT   v_MonthlySalarySheet  .  EmpName  ,EmpNameBn,DsgNameBn,DName,   v_MonthlySalarySheet  .  EmpCardNo  ,DptName,DptNameBn,   v_MonthlySalarySheet  .  DsgName  ,   v_MonthlySalarySheet  .  LnCode  ,   v_MonthlySalarySheet  .  FCode  ,   v_MonthlySalarySheet  .  GrpName  ,   v_MonthlySalarySheet  .  GrdName ,GrdNameBangla ,   convert(varchar(11),v_MonthlySalarySheet.EmpJoiningDate,105) as EmpJoiningDate ,   v_MonthlySalarySheet  .  DaysInMonth  ,   v_MonthlySalarySheet  .  WeekendHoliday  ,   v_MonthlySalarySheet  .  CasualLeave  ,   v_MonthlySalarySheet  .  SickLeave  ,   v_MonthlySalarySheet  .  AnnualLeave  ,   v_MonthlySalarySheet  .  FestivalHoliday  ,   v_MonthlySalarySheet  .  AbsentDay  ,   v_MonthlySalarySheet  .  PresentDay  ,   v_MonthlySalarySheet  .  PayableDays  ,   v_MonthlySalarySheet  .  BasicSalary  ,   v_MonthlySalarySheet  .  HouseRent  ,   v_MonthlySalarySheet  .  MedicalAllownce  ,   v_MonthlySalarySheet  .  ConvenceAllownce  ,   v_MonthlySalarySheet  .  FoodAllownce  ,   v_MonthlySalarySheet  .  EmpPresentSalary  ,   v_MonthlySalarySheet  .  AbsentDeduction  ,   v_MonthlySalarySheet  .  Payable  ,   v_MonthlySalarySheet  .  TotalOTHour  ,   v_MonthlySalarySheet  .  OTRate  ,   v_MonthlySalarySheet  .  TotalOTAmount  ,   v_MonthlySalarySheet  .  AttendanceBonus  ,   v_MonthlySalarySheet  .  AdvanceDeduction  ,   v_MonthlySalarySheet  .  LoanDeduction  ,   v_MonthlySalarySheet  .  NetPayable  ,   v_MonthlySalarySheet  .  Stampdeduct  ,   v_MonthlySalarySheet  .  TotalSalary,LnId,DptId,IsSeperationGeneration  FROM  v_MonthlySalarySheet where Month='" + txtMonthId.Text.Substring(0, 2) + "' AND Year = '" + txtMonthId.Text.Substring(3, 4) + "' AND EmpType='Worker' AND DName='" + ddlDivisionName.SelectedItem.ToString() + "' AND IsSeperationGeneration='0' AND DptId " + setPredicate + "  Group By v_MonthlySalarySheet  .  EmpName  ,   v_MonthlySalarySheet  .  EmpCardNo  ,EmpNameBn,DsgNameBn,   v_MonthlySalarySheet  .  DsgName  ,   v_MonthlySalarySheet  .  LnCode  ,   v_MonthlySalarySheet  .  FCode  ,   v_MonthlySalarySheet  .  GrpName  ,   v_MonthlySalarySheet  .  GrdName  ,GrdNameBangla,  v_MonthlySalarySheet.EmpJoiningDate,   v_MonthlySalarySheet  .  DaysInMonth  ,   v_MonthlySalarySheet  .  WeekendHoliday  ,   v_MonthlySalarySheet  .  CasualLeave  ,   v_MonthlySalarySheet  .  SickLeave  ,   v_MonthlySalarySheet  .  AnnualLeave  ,   v_MonthlySalarySheet  .  FestivalHoliday  ,   v_MonthlySalarySheet  .  AbsentDay  ,   v_MonthlySalarySheet  .  PresentDay  ,   v_MonthlySalarySheet  .  PayableDays  ,   v_MonthlySalarySheet  .  BasicSalary  ,   v_MonthlySalarySheet  .  HouseRent  ,   v_MonthlySalarySheet  .  MedicalAllownce  ,   v_MonthlySalarySheet  .  ConvenceAllownce  ,   v_MonthlySalarySheet  .  FoodAllownce  ,   v_MonthlySalarySheet  .  EmpPresentSalary  ,   v_MonthlySalarySheet  .  AbsentDeduction  ,   v_MonthlySalarySheet  .  Payable  ,   v_MonthlySalarySheet  .  TotalOTHour  ,   v_MonthlySalarySheet  .  OTRate  ,   v_MonthlySalarySheet  .  TotalOTAmount  ,   v_MonthlySalarySheet  .  AttendanceBonus  ,   v_MonthlySalarySheet  .  AdvanceDeduction  ,   v_MonthlySalarySheet  .  LoanDeduction  ,   v_MonthlySalarySheet  .  NetPayable  ,   v_MonthlySalarySheet  .  Stampdeduct  ,   v_MonthlySalarySheet.TotalSalary,LnId,DptId,DptName,DptNameBn,DName,IsSeperationGeneration Order By DptId,LnId,EmpCardNo ", dt = new DataTable());
                else sqlDB.fillDataTable("SELECT   v_MonthlySalarySheet  .  EmpName  ,EmpNameBn,DsgNameBn,DName,   v_MonthlySalarySheet  .  EmpCardNo  ,DptName,DptNameBn ,  v_MonthlySalarySheet  .  DsgName  ,   v_MonthlySalarySheet  .  LnCode  ,   v_MonthlySalarySheet  .  FCode  ,   v_MonthlySalarySheet  .  GrpName  ,   v_MonthlySalarySheet  .  GrdName  ,GrdNameBangla, convert(varchar(11),v_MonthlySalarySheet.EmpJoiningDate,105) as EmpJoiningDate,   v_MonthlySalarySheet  .  DaysInMonth  ,   v_MonthlySalarySheet  .  WeekendHoliday  ,   v_MonthlySalarySheet  .  CasualLeave  ,   v_MonthlySalarySheet  .  SickLeave  ,   v_MonthlySalarySheet  .  AnnualLeave  ,   v_MonthlySalarySheet  .  FestivalHoliday  ,   v_MonthlySalarySheet  .  AbsentDay  ,   v_MonthlySalarySheet  .  PresentDay  ,   v_MonthlySalarySheet  .  PayableDays  ,   v_MonthlySalarySheet  .  BasicSalary  ,   v_MonthlySalarySheet  .  HouseRent  ,   v_MonthlySalarySheet  .  MedicalAllownce  ,   v_MonthlySalarySheet  .  ConvenceAllownce  ,   v_MonthlySalarySheet  .  FoodAllownce  ,   v_MonthlySalarySheet  .  EmpPresentSalary  ,   v_MonthlySalarySheet  .  AbsentDeduction  ,   v_MonthlySalarySheet  .  Payable  ,   v_MonthlySalarySheet  .  TotalOTHour  ,   v_MonthlySalarySheet  .  OTRate  ,   v_MonthlySalarySheet  .  TotalOTAmount  ,   v_MonthlySalarySheet  .  AttendanceBonus  ,   v_MonthlySalarySheet  .  AdvanceDeduction  ,   v_MonthlySalarySheet  .  LoanDeduction  ,   v_MonthlySalarySheet  .  NetPayable  ,   v_MonthlySalarySheet  .  Stampdeduct  ,   v_MonthlySalarySheet  .  TotalSalary,LnId,DptId,IsSeperationGeneration  FROM  v_MonthlySalarySheet where Month='" + txtMonthId.Text.Substring(0, 2) + "' AND Year = '" + txtMonthId.Text.Substring(3, 4) + "' AND EmpID='" + ddlCardNo.SelectedValue.ToString() + "' AND DName='" + ddlDivisionName.SelectedItem.ToString() + "' AND IsSeperationGeneration='0' And DptId "+setPredicate+" Group By  v_MonthlySalarySheet  .  EmpName  ,   v_MonthlySalarySheet  .  EmpCardNo  ,EmpNameBn,DsgNameBn,   v_MonthlySalarySheet  .  DsgName  ,   v_MonthlySalarySheet  .  LnCode  ,   v_MonthlySalarySheet  .  FCode  ,   v_MonthlySalarySheet  .  GrpName ,v_MonthlySalarySheet  .  GrdName  ,GrdNameBangla,v_MonthlySalarySheet.EmpJoiningDate,   v_MonthlySalarySheet  .  DaysInMonth  ,   v_MonthlySalarySheet  .  WeekendHoliday  ,   v_MonthlySalarySheet  .  CasualLeave  ,   v_MonthlySalarySheet  .  SickLeave  ,   v_MonthlySalarySheet  .  AnnualLeave  ,   v_MonthlySalarySheet  .  FestivalHoliday  ,   v_MonthlySalarySheet  .  AbsentDay  ,   v_MonthlySalarySheet  .  PresentDay  ,   v_MonthlySalarySheet  .  PayableDays  ,   v_MonthlySalarySheet  .  BasicSalary  ,   v_MonthlySalarySheet  .  HouseRent  ,   v_MonthlySalarySheet  .  MedicalAllownce  ,   v_MonthlySalarySheet  .  ConvenceAllownce  ,   v_MonthlySalarySheet  .  FoodAllownce  ,   v_MonthlySalarySheet  .  EmpPresentSalary  ,   v_MonthlySalarySheet  .  AbsentDeduction  ,   v_MonthlySalarySheet  .  Payable  ,   v_MonthlySalarySheet  .  TotalOTHour  ,   v_MonthlySalarySheet  .  OTRate  ,   v_MonthlySalarySheet  .  TotalOTAmount  ,   v_MonthlySalarySheet  .  AttendanceBonus  ,   v_MonthlySalarySheet  .  AdvanceDeduction  ,   v_MonthlySalarySheet  .  LoanDeduction  ,   v_MonthlySalarySheet  .  NetPayable  ,   v_MonthlySalarySheet  .  Stampdeduct  ,   v_MonthlySalarySheet.TotalSalary,LnId,DptId,DptName,DptNameBn,DName,IsSeperationGeneration Order By DptId,LnId,EmpCardNo ", dt = new DataTable());
                Session["__WorkerSalarySheet__"] = dt;
                string monthName ="";
                if (rbLanguage.SelectedValue.ToString().Equals("0"))
                {
                    Session["__Language__"] = "Bangal";
                    sqlDB.fillDataTable("select MonthName from HRD_MonthNameBangla where MonthId='" + txtMonthId.Text.Substring(0, 2) + "'", dt = new DataTable());
                    monthName = dt.Rows[0]["MonthName"].ToString();
                    monthName += "-" + txtMonthId.Text.Substring(3, 4);

                }
                else if (rbLanguage.SelectedValue.ToString().Equals("1"))
                {
                    Session["__Language__"] = "English";
                    monthName = new DateTime(int.Parse(txtMonthId.Text.Substring(3, 4)), int.Parse(txtMonthId.Text.Substring(0, 2)), 1).ToString("MMMM", CultureInfo.InvariantCulture);
                    monthName += "-" + txtMonthId.Text.Substring(3, 4);
                }


                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=WorkerSalarySheet-"+monthName+"');", true);  //Open New Tab for Sever side code
            }
            catch (Exception ex)
            { 
            
            }
        }


        private void StaffSalarySheet(string SalaryType)
        {
            try
            {
                string setPredicate = "";
                //   string[] getMonth = ddlMonthID.SelectedValue.Split('-');
                for (byte b = 0; b < lstSelected.Items.Count; b++)
                {

                    if (b == 0 && b == lstSelected.Items.Count - 1)
                    {
                        setPredicate = "in('" + lstSelected.Items[b].Value + "')";
                    }
                    else if (b == 0 && b != lstSelected.Items.Count - 1)
                    {
                        setPredicate += "in ('" + lstSelected.Items[b].Value + "'";
                    }
                    else if (b != 0 && b == lstSelected.Items.Count - 1)
                    {
                        setPredicate += ",'" + lstSelected.Items[b].Value + "')";
                    }
                    else setPredicate += ",'" + lstSelected.Items[b].Value + "'";
                }


                if (SalaryType.Equals("CSL"))
                {
                    if (!ddlCardNo.Enabled) sqlDB.fillDataTable(" SELECT DName, v_MonthlySalarySheet.EmpName,DptName,DptNameBn,v_MonthlySalarySheet.EmpCardNo,v_MonthlySalarySheet.DsgName, convert(varchar(11),v_MonthlySalarySheet.EmpJoiningDate,105) as EmpJoiningDate,v_MonthlySalarySheet.DaysInMonth,v_MonthlySalarySheet.WeekendHoliday,v_MonthlySalarySheet.FestivalHoliday,v_MonthlySalarySheet.CasualLeave,v_MonthlySalarySheet.SickLeave,v_MonthlySalarySheet.AnnualLeave,v_MonthlySalarySheet.AbsentDay,v_MonthlySalarySheet.PresentDay,v_MonthlySalarySheet.PayableDays,v_MonthlySalarySheet.BasicSalary,v_MonthlySalarySheet.OthersAllownce,v_MonthlySalarySheet.EmpPresentSalary,v_MonthlySalarySheet.AbsentDeduction, v_MonthlySalarySheet.Payable,v_MonthlySalarySheet.LunchAllowance,v_MonthlySalarySheet.AttendanceBonus,v_MonthlySalarySheet.AdvanceDeduction,v_MonthlySalarySheet.LoanDeduction,v_MonthlySalarySheet.NetPayable,v_MonthlySalarySheet.Stampdeduct,v_MonthlySalarySheet.TotalSalary,v_MonthlySalarySheet.LnCode,DptId,LnId,SalaryCount FROM v_MonthlySalarySheet where Month='" + txtMonthId.Text.Substring(0, 2) + "' AND Year = '" + txtMonthId.Text.Substring(3, 4) + "' AND EmpType='Staff' AND DName='" + ddlDivisionName.SelectedItem.ToString() + "' AND SalaryCount not in ('Bank') AND DptId " + setPredicate + " Group By v_MonthlySalarySheet.EmpName,v_MonthlySalarySheet.EmpCardNo,v_MonthlySalarySheet.DsgName,v_MonthlySalarySheet.EmpJoiningDate,v_MonthlySalarySheet.DaysInMonth,v_MonthlySalarySheet.WeekendHoliday,v_MonthlySalarySheet.FestivalHoliday,v_MonthlySalarySheet.CasualLeave,v_MonthlySalarySheet.SickLeave,v_MonthlySalarySheet.AnnualLeave,v_MonthlySalarySheet.AbsentDay,v_MonthlySalarySheet.PresentDay,v_MonthlySalarySheet.PayableDays,v_MonthlySalarySheet.BasicSalary,v_MonthlySalarySheet.OthersAllownce,v_MonthlySalarySheet.EmpPresentSalary,v_MonthlySalarySheet.AbsentDeduction, v_MonthlySalarySheet.Payable,v_MonthlySalarySheet.LunchAllowance,v_MonthlySalarySheet.AttendanceBonus,v_MonthlySalarySheet.AdvanceDeduction,v_MonthlySalarySheet.LoanDeduction,v_MonthlySalarySheet.NetPayable,DptName,DptNameBn,v_MonthlySalarySheet.Stampdeduct,v_MonthlySalarySheet.TotalSalary,v_MonthlySalarySheet.LnCode,DptId,LnId,SalaryCount,DName   Order By DptId,LnId,EmpCardNo", dt = new DataTable());
                    else sqlDB.fillDataTable(" SELECT DName,v_MonthlySalarySheet.EmpName,DptName,DptNameBn,v_MonthlySalarySheet.EmpCardNo,v_MonthlySalarySheet.DsgName,convert(varchar(11),v_MonthlySalarySheet.EmpJoiningDate,105) as EmpJoiningDate ,v_MonthlySalarySheet.DaysInMonth,v_MonthlySalarySheet.WeekendHoliday,v_MonthlySalarySheet.FestivalHoliday,v_MonthlySalarySheet.CasualLeave,v_MonthlySalarySheet.SickLeave,v_MonthlySalarySheet.AnnualLeave,v_MonthlySalarySheet.AbsentDay,v_MonthlySalarySheet.PresentDay,v_MonthlySalarySheet.PayableDays,v_MonthlySalarySheet.BasicSalary,v_MonthlySalarySheet.OthersAllownce,v_MonthlySalarySheet.EmpPresentSalary,v_MonthlySalarySheet.AbsentDeduction, v_MonthlySalarySheet.Payable,v_MonthlySalarySheet.LunchAllowance,v_MonthlySalarySheet.AttendanceBonus,v_MonthlySalarySheet.AdvanceDeduction,v_MonthlySalarySheet.LoanDeduction,v_MonthlySalarySheet.NetPayable,v_MonthlySalarySheet.Stampdeduct,v_MonthlySalarySheet.TotalSalary,v_MonthlySalarySheet.LnCode,DptId,LnId,SalaryCount FROM v_MonthlySalarySheet where Month='" + txtMonthId.Text.Substring(0, 2) + "' AND Year = '" + txtMonthId.Text.Substring(3, 4) + "' AND EmpType='Staff' AND EmpID='" + ddlCardNo.SelectedValue.ToString() + "' AND DName='" + ddlDivisionName.SelectedItem.ToString() + "' AND SalaryCount not in ('Bank') Group By v_MonthlySalarySheet.EmpName,v_MonthlySalarySheet.EmpCardNo,v_MonthlySalarySheet.DsgName,v_MonthlySalarySheet.EmpJoiningDate,v_MonthlySalarySheet.DaysInMonth,v_MonthlySalarySheet.WeekendHoliday,v_MonthlySalarySheet.FestivalHoliday,v_MonthlySalarySheet.CasualLeave,v_MonthlySalarySheet.SickLeave,v_MonthlySalarySheet.AnnualLeave,v_MonthlySalarySheet.AbsentDay,v_MonthlySalarySheet.PresentDay,v_MonthlySalarySheet.PayableDays,v_MonthlySalarySheet.BasicSalary,v_MonthlySalarySheet.OthersAllownce,v_MonthlySalarySheet.EmpPresentSalary,v_MonthlySalarySheet.AbsentDeduction, v_MonthlySalarySheet.Payable,v_MonthlySalarySheet.LunchAllowance,v_MonthlySalarySheet.AttendanceBonus,v_MonthlySalarySheet.AdvanceDeduction,v_MonthlySalarySheet.LoanDeduction,DptName,DptNameBn,v_MonthlySalarySheet.NetPayable,v_MonthlySalarySheet.Stampdeduct,v_MonthlySalarySheet.TotalSalary,v_MonthlySalarySheet.LnCode,DptId,LnId,SalaryCount,DName Order By DptId,LnId,EmpCardNo ", dt = new DataTable());
                }
                else
                {
                    if (!ddlCardNo.Enabled) sqlDB.fillDataTable(" SELECT v_MonthlySalarySheet.EmpName,DptName,DptNameBn,v_MonthlySalarySheet.EmpCardNo,v_MonthlySalarySheet.DsgName, convert(varchar(11),v_MonthlySalarySheet.EmpJoiningDate,105) as EmpJoiningDate,v_MonthlySalarySheet.DaysInMonth,v_MonthlySalarySheet.WeekendHoliday,v_MonthlySalarySheet.FestivalHoliday,v_MonthlySalarySheet.CasualLeave,v_MonthlySalarySheet.SickLeave,v_MonthlySalarySheet.AnnualLeave,v_MonthlySalarySheet.AbsentDay,v_MonthlySalarySheet.PresentDay,v_MonthlySalarySheet.PayableDays,v_MonthlySalarySheet.BasicSalary,v_MonthlySalarySheet.OthersAllownce,v_MonthlySalarySheet.EmpPresentSalary,v_MonthlySalarySheet.AbsentDeduction, v_MonthlySalarySheet.Payable,v_MonthlySalarySheet.LunchAllowance,v_MonthlySalarySheet.AttendanceBonus,v_MonthlySalarySheet.AdvanceDeduction,v_MonthlySalarySheet.LoanDeduction,v_MonthlySalarySheet.NetPayable,v_MonthlySalarySheet.Stampdeduct,v_MonthlySalarySheet.TotalSalary,v_MonthlySalarySheet.LnCode,DptId,LnId,SalaryCount FROM v_MonthlySalarySheet where Month='" + txtMonthId.Text.Substring(0, 2) + "' AND Year = '" + txtMonthId.Text.Substring(3, 4) + "' AND EmpType='Staff' AND DName='" + ddlDivisionName.SelectedItem.ToString() + "' AND SalaryCount in ('Bank') AND DptId " + setPredicate + " Group By v_MonthlySalarySheet.EmpName,v_MonthlySalarySheet.EmpCardNo,v_MonthlySalarySheet.DsgName,v_MonthlySalarySheet.EmpJoiningDate,v_MonthlySalarySheet.DaysInMonth,v_MonthlySalarySheet.WeekendHoliday,v_MonthlySalarySheet.FestivalHoliday,v_MonthlySalarySheet.CasualLeave,v_MonthlySalarySheet.SickLeave,v_MonthlySalarySheet.AnnualLeave,v_MonthlySalarySheet.AbsentDay,v_MonthlySalarySheet.PresentDay,v_MonthlySalarySheet.PayableDays,v_MonthlySalarySheet.BasicSalary,v_MonthlySalarySheet.OthersAllownce,v_MonthlySalarySheet.EmpPresentSalary,v_MonthlySalarySheet.AbsentDeduction, v_MonthlySalarySheet.Payable,v_MonthlySalarySheet.LunchAllowance,v_MonthlySalarySheet.AttendanceBonus,v_MonthlySalarySheet.AdvanceDeduction,v_MonthlySalarySheet.LoanDeduction,v_MonthlySalarySheet.NetPayable,DptName,DptNameBn,v_MonthlySalarySheet.Stampdeduct,v_MonthlySalarySheet.TotalSalary,v_MonthlySalarySheet.LnCode,DptId,LnId,SalaryCount   Order By DptId,LnId,EmpCardNo", dt = new DataTable());
                    else sqlDB.fillDataTable(" SELECT v_MonthlySalarySheet.EmpName,DptName,DptNameBn,v_MonthlySalarySheet.EmpCardNo,v_MonthlySalarySheet.DsgName,convert(varchar(11),v_MonthlySalarySheet.EmpJoiningDate,105) as EmpJoiningDate ,v_MonthlySalarySheet.DaysInMonth,v_MonthlySalarySheet.WeekendHoliday,v_MonthlySalarySheet.FestivalHoliday,v_MonthlySalarySheet.CasualLeave,v_MonthlySalarySheet.SickLeave,v_MonthlySalarySheet.AnnualLeave,v_MonthlySalarySheet.AbsentDay,v_MonthlySalarySheet.PresentDay,v_MonthlySalarySheet.PayableDays,v_MonthlySalarySheet.BasicSalary,v_MonthlySalarySheet.OthersAllownce,v_MonthlySalarySheet.EmpPresentSalary,v_MonthlySalarySheet.AbsentDeduction, v_MonthlySalarySheet.Payable,v_MonthlySalarySheet.LunchAllowance,v_MonthlySalarySheet.AttendanceBonus,v_MonthlySalarySheet.AdvanceDeduction,v_MonthlySalarySheet.LoanDeduction,v_MonthlySalarySheet.NetPayable,v_MonthlySalarySheet.Stampdeduct,v_MonthlySalarySheet.TotalSalary,v_MonthlySalarySheet.LnCode,DptId,LnId,SalaryCount FROM v_MonthlySalarySheet where Month='" + txtMonthId.Text.Substring(0, 2) + "' AND Year = '" + txtMonthId.Text.Substring(3, 4) + "' AND EmpType='Staff' AND EmpID='" + ddlCardNo.SelectedValue.ToString() + "' AND DName='" + ddlDivisionName.SelectedItem.ToString() + "' AND SalaryCount in ('Bank') Group By v_MonthlySalarySheet.EmpName,v_MonthlySalarySheet.EmpCardNo,v_MonthlySalarySheet.DsgName,v_MonthlySalarySheet.EmpJoiningDate,v_MonthlySalarySheet.DaysInMonth,v_MonthlySalarySheet.WeekendHoliday,v_MonthlySalarySheet.FestivalHoliday,v_MonthlySalarySheet.CasualLeave,v_MonthlySalarySheet.SickLeave,v_MonthlySalarySheet.AnnualLeave,v_MonthlySalarySheet.AbsentDay,v_MonthlySalarySheet.PresentDay,v_MonthlySalarySheet.PayableDays,v_MonthlySalarySheet.BasicSalary,v_MonthlySalarySheet.OthersAllownce,v_MonthlySalarySheet.EmpPresentSalary,v_MonthlySalarySheet.AbsentDeduction, v_MonthlySalarySheet.Payable,v_MonthlySalarySheet.LunchAllowance,v_MonthlySalarySheet.AttendanceBonus,v_MonthlySalarySheet.AdvanceDeduction,v_MonthlySalarySheet.LoanDeduction,DptName,DptNameBn,v_MonthlySalarySheet.NetPayable,v_MonthlySalarySheet.Stampdeduct,v_MonthlySalarySheet.TotalSalary,v_MonthlySalarySheet.LnCode,DptId,LnId,SalaryCount Order By DptId,LnId,EmpCardNo ", dt = new DataTable());
                }
                Session["__StaffSalarySheet__"] = dt;
                string monthName="";
                if (rbLanguage.SelectedValue.ToString().Equals("0"))
                {
                    Session["__Language__"] = "Bangal";
                    sqlDB.fillDataTable("select MonthName from HRD_MonthNameBangla where MonthId='" + txtMonthId.Text.Substring(0, 2) + "'", dt = new DataTable());
                    monthName = dt.Rows[0]["MonthName"].ToString();
                    monthName += "-" + txtMonthId.Text.Substring(3, 4);
                }

                else if (rbLanguage.SelectedValue.ToString().Equals("1"))              
                {
                    Session["__Language__"] = "English";
                    monthName = new DateTime(int.Parse(txtMonthId.Text.Substring(3, 4)), int.Parse(txtMonthId.Text.Substring(0, 2)), 1).ToString("MMMM", CultureInfo.InvariantCulture);
                    monthName += "-" + txtMonthId.Text.Substring(3, 4);
                }

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=StaffSalarySheet-" + monthName + "');", true);  //Open New Tab for Sever side code
            }
            catch (Exception ex)
            { 
            
            }
        }


        protected void rblSelectEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.Employee.LoadEmpCardNoWithName(ddlCardNo, rblSelectEmployeeType.SelectedValue);
        }

        protected void btnBankSalaryList_Click(object sender, EventArgs e)
        {
            try
            {
                StaffSalarySheet("BSL");  // BSL = Bank Salary List;
        
            }
            catch { }
        }
        private void AddRemoveItem(ListBox aSource, ListBox aTarget)
        {
           
            ListItemCollection licCollection;

            try
            {

                licCollection = new ListItemCollection();
                for (int intCount = 0; intCount < aSource.Items.Count; intCount++)
                {
                    if (aSource.Items[intCount].Selected == true)
                        licCollection.Add(aSource.Items[intCount]);
                }

                for (int intCount = 0; intCount < licCollection.Count; intCount++)
                {
                    aSource.Items.Remove(licCollection[intCount]);
                    aTarget.Items.Add(licCollection[intCount]);
                }
               

            }
            catch (Exception expException)
            {
                Response.Write(expException.Message);
            }
            finally
            {
                licCollection = null;
            }

        }

        private void AddRemoveAll(ListBox aSource, ListBox aTarget)
        {

            try
            {

                foreach (ListItem item in aSource.Items)
                {
                    aTarget.Items.Add(item);
                }
                aSource.Items.Clear();

            }
            catch (Exception expException)
            {
                Response.Write(expException.Message);
            }

        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstAll, lstSelected);
        }
      
        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            AddRemoveAll(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            AddRemoveAll(lstSelected, lstAll);
        }

        protected void ddlDivisionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDepartment(ddlDivisionName.SelectedValue, lstAll);
        }

        private void LoadDepartment(string divisionId, ListBox lst)
        {
            try
            {
                dt = new DataTable();

                sqlDB.fillDataTable("SELECT DptId, DptName FROM HRD_Department where DId=" + divisionId + "", dt);

                lst.DataValueField = "DptId";
                lst.DataTextField = "DptName";
                lst.DataSource = dt;
                lst.DataBind();
            }
            catch { }
        }
    }
}