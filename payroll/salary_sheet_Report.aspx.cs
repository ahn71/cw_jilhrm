using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.payroll
{
    public partial class salary_sheet_Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                classes.commonTask.loadEmpTye(rblEmployeeType);
                rblEmployeeType.SelectedValue = "1";
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
                ViewState["__IsGerments__"]=classes.commonTask.IsGarments();
                if (!bool.Parse(ViewState["__IsGerments__"].ToString()))
                    trHideForIndividual.Visible = false;
                    
            }
        }
        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];

                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CShortName__"] = getCookies["__CShortName__"].ToString();


                //------------load privilege setting inof from db------
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "salary_sheet_Report.aspx", ddlCompanyName, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                commonTask.LoadDepartmentByCompanyInListBox(ViewState["__CompanyId__"].ToString(), lstAll);
                classes.Payroll.loadMonthIdByCompany(ddlSelectMonth, ViewState["__CompanyId__"].ToString());
                //-----------------------------------------------------


            }
            catch { }
        }
        private void addAllTextInShift()
        {
            if (ddlShiftName.Items.Count > 2)
                ddlShiftName.Items.Insert(1, new ListItem("All", "00"));
        }

        protected void rblGenerateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
              

                if (!rblGenerateType.SelectedItem.Text.Equals("All"))
                {
                    txtEmpCardNo.Enabled = true;
                    pnl1.Enabled = false;
                    ddlShiftName.Enabled = false;
                    trHideForIndividual.Visible = false;
                    pnl1.Visible = false;
                    txtEmpCardNo.Focus();

                }
                else
                {
                    txtEmpCardNo.Enabled = false;
                    pnl1.Enabled = true;
                    ddlShiftName.Enabled = true;
                    trHideForIndividual.Visible = true;
                    pnl1.Visible = true;
                    rblEmployeeType.SelectedValue = "1";
                    rblPaymentType.SelectedValue = "Cash";
                }
                if (!bool.Parse(ViewState["__IsGerments__"].ToString()))
                    trHideForIndividual.Visible = false;
            }
            catch { }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            if (ddlSelectMonth.SelectedValue == "0") 
            { lblMessage.InnerText = "warning->Please select any Month!"; ddlSelectMonth.Focus(); return; }
            if (rblGenerateType.SelectedItem.Text.Equals("All") && lstSelected.Items.Count < 1) { lblMessage.InnerText = "warning->Please select any Department"; lstSelected.Focus(); return; }
            if (!rblGenerateType.SelectedItem.Text.Equals("All") && txtEmpCardNo.Text.Trim().Length<4) { lblMessage.InnerText = "warning->Please type valid Card No!(Minimum last 4 digit.)";txtEmpCardNo.Focus(); return; }
            if(ckbBankForwardingReport.Checked)
                generateSalarySheetForBankForwarding();
            else
            generateSalarySheet();
        }
        private void generateSalarySheet()
        {
            try
            {
                string CompanyList = "";
                string ShiftList = "";
                string DepartmentList = "";

                if (!Page.IsValid)   // If Java script are desible then 
                {
                    lblMessage.InnerText = "erroe->Please Select From Date And To Date"; return;
                }
                if (chkForAllCompany.Checked)
                {
                    CompanyList = classes.Payroll.getCompanyList(ddlCompanyName);
                    ShiftList = classes.Payroll.getSftIdList(ddlShiftName);
                    DepartmentList = classes.commonTask.getDepartmentList();
                }
                else
                {
                    CompanyList = (ddlCompanyName.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();

                    //if (ddlShiftName.SelectedItem.ToString().Equals("All"))
                    //{

                    //    ShiftList = classes.Payroll.getSftIdList(ddlShiftName);
                    //    DepartmentList = classes.commonTask.getDepartmentList();
                    //}
                    //else
                    //{
                    //    ShiftList = ddlShiftName.SelectedValue.ToString();
                    //    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                    //}
                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                }

                string Condition = (bool.Parse(ViewState["__IsGerments__"].ToString())) ? "And EmpTypeId=" + rblEmployeeType.SelectedValue + " And SalaryCount='" + rblPaymentType.SelectedValue + "'" : "";
                string getSQLCMD;
                DataTable dt = new DataTable();
                if (rblGenerateType.SelectedItem.Text.Equals("All"))
                {
                    if (rblSheet.SelectedValue == "0")
                    {
                        getSQLCMD = "SELECT EmpName,EmptypeId, Substring(EmpCardNo,10,6) as EmpCardNo , AbsentDay, BasicSalary, HouseRent, MedicalAllownce, AbsentDeduction, " +
                            " TotalOTHour, OTRate, round(TotalOTAmount,0) as TotalOTAmount, AttendanceBonus, DptName, CompanyName, SftName, EmpPresentSalary, Address,HolidayWorkingDays,HolidayTaka,HoliDayBillAmount," +
                            " DptId, CompanyId, DsgName, TotalSalary, GrdName, GId, GName, PresentDay,WeekendHoliday,FestivalHoliday, PayableDays, Payable, OthersAllownce, ProvidentFund, ProfitTax, LateFine, TiffinDays, TiffinTaka, TiffinBillAmount,CasualLeave,SickLeave,AnnualLeave,OfficialLeave,DormitoryRent,TotalOverTime,TotalOtherOverTime,DaysInMonth,OthersPay,OthersDeduction,ShortLeave,AdvanceDeduction,LateDays,ConvenceAllownce,convert(varchar(10), EmpJoiningDate,105) EmpJoiningDate, MobileCell,BankName,FoodAllownce,LateFineDays,Adjustment,isnull(NetEmpPresentSalary,EmpPresentSalary) as NetEmpPresentSalary,LWP,isnull(LWPDeduction,0) as LWPDeduction  " +
                            " FROM   v_MonthlySalarySheet " +
                            " where "+
                            " IsActive='1' and CompanyId  in(" + CompanyList + ") and DptId " + DepartmentList + "  AND YearMonth='" + ddlSelectMonth.SelectedItem.Value.ToString() + "' "+Condition+"  AND IsSeperationGeneration='0' "+
                            " ORDER BY CONVERT(int,DptId),convert(int,Gid), CustomOrdering";                          
                        Session["__ReportTitle__"] = "";
                    }
                    else
                    {
                        getSQLCMD = "SELECT EmpName,EmptypeId, Substring(EmpCardNo,10,6) as EmpCardNo , AbsentDay, BasicSalary, HouseRent, MedicalAllownce, AbsentDeduction, " +
                             " TotalOTHour, OTRate, round(TotalOTAmount,0) as TotalOTAmount, AttendanceBonus, DptName, CompanyName, SftName, EmpPresentSalary, Address,HolidayWorkingDays,HolidayTaka,HoliDayBillAmount," +
                             " DptId, CompanyId, DsgName, TotalSalary, GrdName, GId, GName, PresentDay,WeekendHoliday,FestivalHoliday, PayableDays, Payable, OthersAllownce, ProvidentFund, ProfitTax, LateFine, TiffinDays, TiffinTaka, TiffinBillAmount,CasualLeave,SickLeave,AnnualLeave,OfficialLeave,DormitoryRent,TotalOverTime,TotalOtherOverTime,DaysInMonth,OthersPay,OthersDeduction,ShortLeave,AdvanceDeduction,LateDays,ConvenceAllownce,convert(varchar(10), EmpJoiningDate,105) EmpJoiningDate, MobileCell,BankName,FoodAllownce,LateFineDays,Adjustment,isnull(NetEmpPresentSalary,EmpPresentSalary) as NetEmpPresentSalary,LWP,isnull(LWPDeduction,0) as LWPDeduction  " +
                             " FROM   v_MonthlySalarySheet " +
                             " where " +
                             " IsActive='1' and CompanyId  in(" + CompanyList + ") and DptId " + DepartmentList + "  AND YearMonth='" + ddlSelectMonth.SelectedItem.Value.ToString() + "' " + Condition + " AND IsSeperationGeneration='1' " +
                             " ORDER BY CONVERT(int,DptId),convert(int,Gid), CustomOrdering";
                        Session["__ReportTitle__"] = "[Separation]";
                    }


                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->No record found."; return;
                    }                   

                }
                else
                {
                    if (rblSheet.SelectedValue == "0")
                    {
                        getSQLCMD = "SELECT EmpName,EmptypeId, Substring(EmpCardNo,10,6) as EmpCardNo, AbsentDay, BasicSalary, HouseRent, MedicalAllownce, AbsentDeduction, " +
                           " TotalOTHour, OTRate, round(TotalOTAmount,0) as TotalOTAmount, AttendanceBonus, DptName, CompanyName, SftName, EmpPresentSalary, Address,HolidayWorkingDays,HolidayTaka,HoliDayBillAmount," +
                           " DptId, CompanyId, DsgName, TotalSalary, GrdName, GId, GName, PresentDay,WeekendHoliday,FestivalHoliday, PayableDays, Payable, OthersAllownce, ProvidentFund, ProfitTax, LateFine, TiffinDays, TiffinTaka, TiffinBillAmount,CasualLeave,SickLeave,AnnualLeave,OfficialLeave,SalaryCount,DormitoryRent,TotalOverTime,TotalOtherOverTime,DaysInMonth,OthersPay,OthersDeduction,ShortLeave,AdvanceDeduction,LateDays,ConvenceAllownce,convert(varchar(10), EmpJoiningDate,105) EmpJoiningDate, MobileCell,BankName,FoodAllownce,LateFineDays,Adjustment,isnull(NetEmpPresentSalary,EmpPresentSalary) as NetEmpPresentSalary,LWP,isnull(LWPDeduction,0) as LWPDeduction  " +
                           " FROM   v_MonthlySalarySheet " +
                           " where " +
                           " IsActive='1' AND CompanyId in(" + CompanyList + ") AND YearMonth='" + ddlSelectMonth.SelectedItem.Value.ToString() + "' AND EmpCardNo Like '%" + txtEmpCardNo.Text.Trim() + "' AND IsSeperationGeneration='0' " +
                           " ORDER BY CONVERT(int,DptId),convert(int,Gid), CustomOrdering";                
                        Session["__ReportTitle__"] = "";
                    }
                    else
                    {
                        getSQLCMD = "SELECT EmpName,EmptypeId, Substring(EmpCardNo,10,6) as EmpCardNo, AbsentDay, BasicSalary, HouseRent, MedicalAllownce, AbsentDeduction, " +
                            " TotalOTHour, OTRate, round(TotalOTAmount,0) as TotalOTAmount, AttendanceBonus, DptName, CompanyName, SftName, EmpPresentSalary, Address,HolidayWorkingDays,HolidayTaka,HoliDayBillAmount," +
                            " DptId, CompanyId, DsgName, TotalSalary, GrdName, GId, GName, PresentDay,WeekendHoliday,FestivalHoliday, PayableDays, Payable, OthersAllownce, ProvidentFund, ProfitTax, LateFine, TiffinDays, TiffinTaka, TiffinBillAmount,CasualLeave,SickLeave,AnnualLeave,OfficialLeave,SalaryCount,DormitoryRent,TotalOverTime,TotalOtherOverTime,DaysInMonth,OthersPay,OthersDeduction,ShortLeave,AdvanceDeduction,LateDays,ConvenceAllownce,convert(varchar(10), EmpJoiningDate,105) EmpJoiningDate, MobileCell,BankName,FoodAllownce,LateFineDays,Adjustment,isnull(NetEmpPresentSalary,EmpPresentSalary) as NetEmpPresentSalary,LWP,isnull(LWPDeduction,0) as LWPDeduction  " +
                            " FROM   v_MonthlySalarySheet " +
                            " where " +
                            " IsActive='1' AND CompanyId in(" + CompanyList + ") AND YearMonth='" + ddlSelectMonth.SelectedItem.Value.ToString() + "' AND EmpCardNo Like '%" + txtEmpCardNo.Text.Trim() + "' AND IsSeperationGeneration='1' " +
                            " ORDER BY CONVERT(int,DptId),convert(int,Gid), CustomOrdering";
                        Session["__ReportTitle__"] = "[Separation]";
                    }

                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->No record found."; return;
                    }
                    rblEmployeeType.SelectedValue = dt.Rows[0]["EmptypeId"].ToString();
                    rblPaymentType.SelectedValue = dt.Rows[0]["SalaryCount"].ToString();
                   
                       
                }

                Session["__Language__"] = "English";
                Session["__SalarySheet__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=SalarySheet-" + ddlSelectMonth.SelectedItem.Text + "-" + ViewState["__IsGerments__"].ToString() + "-" + rblEmployeeType.SelectedValue + "-" + rblPaymentType.SelectedValue + "');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
        private void generateSalarySheetForBankForwarding()
        {
            try
            {
                string CompanyList = "";
                string ShiftList = "";
                string DepartmentList = "";

                if (!Page.IsValid)   // If Java script are desible then 
                {
                    lblMessage.InnerText = "erroe->Please Select From Date And To Date"; return;
                }
                if (chkForAllCompany.Checked)
                {
                    CompanyList = classes.Payroll.getCompanyList(ddlCompanyName);
                    ShiftList = classes.Payroll.getSftIdList(ddlShiftName);
                    DepartmentList = classes.commonTask.getDepartmentList();
                }
                else
                {
                    CompanyList = (ddlCompanyName.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();

                    //if (ddlShiftName.SelectedItem.ToString().Equals("All"))
                    //{

                    //    ShiftList = classes.Payroll.getSftIdList(ddlShiftName);
                    //    DepartmentList = classes.commonTask.getDepartmentList();
                    //}
                    //else
                    //{
                    //    ShiftList = ddlShiftName.SelectedValue.ToString();
                    //    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                    //}
                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                }

                string Condition = (bool.Parse(ViewState["__IsGerments__"].ToString())) ? "And EmpTypeId=" + rblEmployeeType.SelectedValue + " And SalaryCount='" + rblPaymentType.SelectedValue + "'" : "";
                string getSQLCMD;
                DataTable dt = new DataTable();
                if (rblGenerateType.SelectedItem.Text.Equals("All"))
                {
                    if (rblSheet.SelectedValue == "0")
                    {
                        getSQLCMD = "SELECT EmpName,EmptypeId, Substring(EmpCardNo,10,6) as EmpCardNo , AbsentDay, BasicSalary, HouseRent, MedicalAllownce, AbsentDeduction, " +
                            " TotalOTHour, OTRate, round(TotalOTAmount,0) as TotalOTAmount, AttendanceBonus, DptName, CompanyName, SftName, EmpPresentSalary, Address,HolidayWorkingDays,HolidayTaka,HoliDayBillAmount," +
                            " DptId, CompanyId, DsgName, TotalSalary, GrdName, GId, GName, PresentDay,WeekendHoliday,FestivalHoliday, PayableDays, Payable, OthersAllownce, ProvidentFund, ProfitTax, LateFine, TiffinDays, TiffinTaka, TiffinBillAmount,CasualLeave,SickLeave,AnnualLeave,OfficialLeave,DormitoryRent,TotalOverTime,TotalOtherOverTime,DaysInMonth,OthersPay,OthersDeduction,ShortLeave,AdvanceDeduction,LateDays,ConvenceAllownce,convert(varchar(10), EmpJoiningDate,105) EmpJoiningDate, MobileCell,BankName,format(YearMonth,'MMMM-yyyy') as YearMonth ,EmpAccountNo " +
                            " FROM   v_MonthlySalarySheet " +
                            " where " +
                            " IsActive='1' and CompanyId  in(" + CompanyList + ") and DptId " + DepartmentList + "  AND YearMonth='" + ddlSelectMonth.SelectedItem.Value.ToString() + "' " + Condition + "  AND IsSeperationGeneration='0' " +
                            " ORDER BY CONVERT(int,DptId),convert(int,Gid), CustomOrdering";
                        Session["__ReportTitle__"] = "";
                    }
                    else
                    {
                        getSQLCMD = "SELECT EmpName,EmptypeId, Substring(EmpCardNo,10,6) as EmpCardNo , AbsentDay, BasicSalary, HouseRent, MedicalAllownce, AbsentDeduction, " +
                             " TotalOTHour, OTRate, round(TotalOTAmount,0) as TotalOTAmount, AttendanceBonus, DptName, CompanyName, SftName, EmpPresentSalary, Address,HolidayWorkingDays,HolidayTaka,HoliDayBillAmount," +
                             " DptId, CompanyId, DsgName, TotalSalary, GrdName, GId, GName, PresentDay,WeekendHoliday,FestivalHoliday, PayableDays, Payable, OthersAllownce, ProvidentFund, ProfitTax, LateFine, TiffinDays, TiffinTaka, TiffinBillAmount,CasualLeave,SickLeave,AnnualLeave,OfficialLeave,DormitoryRent,TotalOverTime,TotalOtherOverTime,DaysInMonth,OthersPay,OthersDeduction,ShortLeave,AdvanceDeduction,LateDays,ConvenceAllownce,convert(varchar(10), EmpJoiningDate,105) EmpJoiningDate, MobileCell,BankName,format(YearMonth,'MMMM-yyyy') as YearMonth ,EmpAccountNo " +
                             " FROM   v_MonthlySalarySheet " +
                             " where " +
                             " IsActive='1' and CompanyId  in(" + CompanyList + ") and DptId " + DepartmentList + "  AND YearMonth='" + ddlSelectMonth.SelectedItem.Value.ToString() + "' " + Condition + " AND IsSeperationGeneration='1' " +
                             " ORDER BY CONVERT(int,DptId),convert(int,Gid), CustomOrdering";
                        Session["__ReportTitle__"] = "[Separation]";
                    }


                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->No record found."; return;
                    }

                }
                else
                {
                    if (rblSheet.SelectedValue == "0")
                    {
                        getSQLCMD = "SELECT EmpName,EmptypeId, Substring(EmpCardNo,10,6) as EmpCardNo, AbsentDay, BasicSalary, HouseRent, MedicalAllownce, AbsentDeduction, " +
                           " TotalOTHour, OTRate, round(TotalOTAmount,0) as TotalOTAmount, AttendanceBonus, DptName, CompanyName, SftName, EmpPresentSalary, Address,HolidayWorkingDays,HolidayTaka,HoliDayBillAmount," +
                           " DptId, CompanyId, DsgName, TotalSalary, GrdName, GId, GName, PresentDay,WeekendHoliday,FestivalHoliday, PayableDays, Payable, OthersAllownce, ProvidentFund, ProfitTax, LateFine, TiffinDays, TiffinTaka, TiffinBillAmount,CasualLeave,SickLeave,AnnualLeave,OfficialLeave,SalaryCount,DormitoryRent,TotalOverTime,TotalOtherOverTime,DaysInMonth,OthersPay,OthersDeduction,ShortLeave,AdvanceDeduction,LateDays,ConvenceAllownce,convert(varchar(10), EmpJoiningDate,105) EmpJoiningDate, MobileCell,BankName " +
                           " FROM   v_MonthlySalarySheet " +
                           " where " +
                           " IsActive='1' AND CompanyId in(" + CompanyList + ") AND YearMonth='" + ddlSelectMonth.SelectedItem.Value.ToString() + "' AND EmpCardNo Like '%" + txtEmpCardNo.Text.Trim() + "' AND IsSeperationGeneration='0' " +
                           " ORDER BY CONVERT(int,DptId),convert(int,Gid), CustomOrdering";
                        Session["__ReportTitle__"] = "";
                    }
                    else
                    {
                        getSQLCMD = "SELECT EmpName,EmptypeId, Substring(EmpCardNo,10,6) as EmpCardNo, AbsentDay, BasicSalary, HouseRent, MedicalAllownce, AbsentDeduction, " +
                            " TotalOTHour, OTRate, round(TotalOTAmount,0) as TotalOTAmount, AttendanceBonus, DptName, CompanyName, SftName, EmpPresentSalary, Address,HolidayWorkingDays,HolidayTaka,HoliDayBillAmount," +
                            " DptId, CompanyId, DsgName, TotalSalary, GrdName, GId, GName, PresentDay,WeekendHoliday,FestivalHoliday, PayableDays, Payable, OthersAllownce, ProvidentFund, ProfitTax, LateFine, TiffinDays, TiffinTaka, TiffinBillAmount,CasualLeave,SickLeave,AnnualLeave,OfficialLeave,SalaryCount,DormitoryRent,TotalOverTime,TotalOtherOverTime,DaysInMonth,OthersPay,OthersDeduction,ShortLeave,AdvanceDeduction,LateDays,ConvenceAllownce,convert(varchar(10), EmpJoiningDate,105) EmpJoiningDate, MobileCell,BankName " +
                            " FROM   v_MonthlySalarySheet " +
                            " where " +
                            " IsActive='1' AND CompanyId in(" + CompanyList + ") AND YearMonth='" + ddlSelectMonth.SelectedItem.Value.ToString() + "' AND EmpCardNo Like '%" + txtEmpCardNo.Text.Trim() + "' AND IsSeperationGeneration='1' " +
                            " ORDER BY CONVERT(int,DptId),convert(int,Gid), CustomOrdering";
                        Session["__ReportTitle__"] = "[Separation]";
                    }

                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->No record found."; return;
                    }
                    rblEmployeeType.SelectedValue = dt.Rows[0]["EmptypeId"].ToString();
                    rblPaymentType.SelectedValue = dt.Rows[0]["SalaryCount"].ToString();


                }

                
                Session["__SalarySheetBankForwardingLetter__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=SalarySheetBankForwardingLetter');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                classes.commonTask.LoadDepartmentByCompanyInListBox(CompanyId, lstAll);
                //classes.commonTask.LoadShift(ddlShiftName, CompanyId);
                //addAllTextInShift();
                classes.Payroll.loadMonthIdByCompany(ddlSelectMonth, CompanyId);
            }
            catch { }
        }

        protected void ddlShiftName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                lstAll.Items.Clear();
                lstSelected.Items.Clear();
                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();

                if (ddlShiftName.SelectedItem.ToString().Equals("All"))
                {

                    string ShiftList = classes.commonTask.getShiftList(ddlShiftName);
                    classes.commonTask.LoadDepartmentByCompanyAndShiftInListBox(CompanyId, ShiftList, lstAll);
                    return;
                }
                classes.commonTask.LoadDepartmentByCompanyAndShiftInListBox(CompanyId, "in (" + ddlShiftName.SelectedValue.ToString() + ")", lstAll);
            }
            catch { }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {           
            classes.commonTask.AddRemoveItem(lstAll, lstSelected);

        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {                     
            classes.commonTask.AddRemoveAll(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {          
            classes.commonTask.AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveAll(lstSelected, lstAll);
        }
    }
}