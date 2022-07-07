using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using ComplexScriptingSystem;
using System.Globalization;
using SigmaERP.classes;

namespace SigmaERP.payroll
{
    public partial class final_bill_payment_sheet : System.Web.UI.Page
    {
        DataTable dt;
        SqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();

            if (!IsPostBack)
            {
                setPrivilege();
                classes.commonTask.loadEmpTypeInRadioButtonList(rbEmpTypeList);
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                loadMonthId();
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
                ViewState["__getUserId__"] = getUserId;

                //------------load privilege setting inof from db------
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "final_bill_payment_sheet.aspx", ddlCompanyList, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
              
                //-----------------------------------------------------

           
            }
            catch { }
        }

        private void loadAcceptableMinuteAsOT()
        {
            try
            {
                sqlDB.fillDataTable("select AcceptableMinuteasOT from HRD_OthersSetting", dt = new DataTable());
                ViewState["__AcceptableMinuteasOT__"] = dt.Rows[0]["AcceptableMinuteasOT"].ToString();
            }
            catch { }

        }

        private void loadMonthId()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select  Distinct format(yearmonth,'MMM-yyyy') as MonthYearText,format(yearmonth,'yyyy-MM') as YearMonthValue from v_MonthlySalarySheet order by YearMonthValue ", dt);
                ddlMonthID.DataTextField = "MonthYearText";
                ddlMonthID.DataValueField = "YearMonthValue";
                ddlMonthID.DataSource = dt;
                ddlMonthID.DataBind();
                ddlMonthID.Items.Insert(0, new ListItem(" ", " "));
            }
            catch { }
        }


        private bool validationBasket()
        {
            try
            {
                if (txtEmpCardNo.Text.Trim().Length <3)
                {
                    lblMessage.InnerText = "warning->Please Type Valid CardNo";
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

       

        private void loadWeekendInfo(string MonthYear, string selectDays,string CompanyId)
        {
            try
            {
                DataTable dt = new DataTable();
                string a = MonthYear.Substring(3, 4);
                string aa = MonthYear.Substring(0, 2);

                //sqlDB.fillDataTable("select convert(varchar(11),WeekendDate,111) as WeekendDate from Attendance_WeekendInfo where MonthName='" + MonthYear + "' AND WeekendDate >='" + MonthYear.Substring(3, 4)+ '-' + MonthYear.Substring(0, 2) + "-01" + "' AND WeekendDate<='" + MonthYear.Substring(3, 4) + '-' + MonthYear.Substring(0, 2) + '-' + selectDays + "'", dt);

                sqlDB.fillDataTable("select distinct format(WeekendDate,'yyyy/MM/dd') as WeekendDate from v_Attendance_WeekendInfo where CompanyId='" + CompanyId + "' AND MonthName='" + MonthYear + "' And WeekendDate>='" + MonthYear.Substring(3, 4) + '-' + MonthYear.Substring(0, 2) + "-01" + "' AND WeekendDate<='" + MonthYear.Substring(3, 4) + '-' + MonthYear.Substring(0, 2) + '-' + selectDays + "'", dt);
                  
                string setPredicate = "";

                for (byte b = 0; b < dt.Rows.Count; b++)
                {
                    if (b == 0 && b == dt.Rows.Count - 1)
                    {
                        setPredicate = "in('" + dt.Rows[b]["WeekendDate"].ToString() + "')";

                    }
                    else if (b == 0 && b != dt.Rows.Count - 1)
                    {
                        setPredicate += "in ('" + dt.Rows[b]["WeekendDate"].ToString() + "'";
                    }
                    else if (b != 0 && b == dt.Rows.Count - 1)
                    {
                        setPredicate += ",'" + dt.Rows[b]["WeekendDate"].ToString() + "')";
                    }
                    else
                    {
                        setPredicate += ",'" + dt.Rows[b]["WeekendDate"].ToString() + "'";
                    }
                }

                ViewState["__setPredicate__"] = setPredicate;
            }
            catch { }

        }


        DataTable dtGetMonthSetup;
        private void loadMonthSetup(string days, string month, string year, string CompanyId)
        {
            try
            {
                string monthName = new DateTime(int.Parse(year), int.Parse(month), int.Parse(days)).ToString("MMM", CultureInfo.InvariantCulture);
                monthName += year.ToString().Substring(2, 2);
                string monthName2 = month + "-" + year;
                SQLOperation.selectBySetCommandInDatatable("select TotalDays,TotalWeekend ,FromDate,ToDate,TotalHoliday,TotalWorkingDays from tblMonthSetup where CompanyId='" + CompanyId + "' AND ( MonthName='" + monthName + "' OR MonthName='" + monthName2 + "') ", dtGetMonthSetup = new DataTable(), sqlDB.connection);

            }
            catch (Exception ex)
            {
            }
        }

        byte totalDays = 0;
        DataTable dtSeperationInfo;
        protected void btnPreview_Click(object sender, EventArgs e)
        {

            lblMessage.InnerText = "";
            dt = new DataTable();

            string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

            try
            {
               
            
                if (validationBasket() == false) return;
                else
                {
                    sqlDB.fillDataTable("select max(EmpSeparationId) as EmpSeparationId,convert(varchar(11),EffectiveDate,105) as EffectiveDate,convert(varchar(2),EffectiveDate,105) as EffectiveDay,EmpId,EmpStatus as SeparationType from v_Personnel_EmpSeparation where companyId='" + CompanyId + "' AND EmpCardNo like '%" + txtEmpCardNo.Text.Trim() + "' and EmpTypeID=" + rbEmpTypeList.SelectedValue.ToString() + " group by EffectiveDate,EmpId, EmpStatus ", dtSeperationInfo = new DataTable());

                    sqlDB.fillDataTable("select * from v_Personnel_EmpSeparation where EmpSeparationId=" + dtSeperationInfo.Rows[0]["EmpSeparationId"].ToString() + " AND YearMonth='"+ddlMonthID.SelectedItem.Value.ToString()+"'",dt);

                }
            }
            catch { }

            if (dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->Please Select Correct Employee Type.";
                return;
            }
            string[] getDays = dtSeperationInfo.Rows[0]["EffectiveDate"].ToString().Trim().Split('-');

            int days = DateTime.DaysInMonth(int.Parse(getDays[2]), int.Parse(getDays[1]));

           
            

            loadWeekendInfo(getDays[1] + "-" + getDays[2], getDays[0],CompanyId);

            loadMonthSetup("1", getDays[1], getDays[2], CompanyId);   // days ,month ,year

            generateMonthlySalarySheet(getDays[1] + "-" + getDays[2], getDays[1], getDays[2], days, dtSeperationInfo.Rows[0]["EffectiveDay"].ToString());


            
        }

        DataTable dtShiftListForCheckOverTime;
        private void checkOverTimeIsActiveForSelectedShift(string CompanyId)
        {
            try
            {
                dtShiftListForCheckOverTime = new DataTable();
                
                sqlDB.fillDataTable("select SftId,SftOverTime from HRD_Shift where CompanyId='" + CompanyId + "' ", dtShiftListForCheckOverTime);
                
            }
            catch { }
        }

        DataTable dtRunningEmp;
        DataTable dtCertainEmp;
        DataTable dtLeaveInfo;
        DataTable dtPresent;
        DataTable dtAbsent;
        DataTable dtLate;
        DataTable dtAdvanceInfo;
        DataTable dtCutAdvance;
        DataTable dtLoanInfo;
        DataTable dtCutLoan;
        DataTable dtStampDeduct;


        private void generateMonthlySalarySheet(string getMonthYear, string month, string year, int Days,string selectDays)
        {
            try
            {
                string getEmpProximityNo;
                double getTotalOT;
                string getJoingingDate;
                bool isActiveLoop = true;
                string MonthName = year + "-" + month;

                // get stamp card price
                sqlDB.fillDataTable("select StampDeduct from HRD_AllownceSetting where AllownceId =(select max(AllownceId) from HRD_AllownceSetting)", dtStampDeduct = new DataTable());

              
                
                salarySheetClearForCertainEmployeeByMonthYearAndEmpId(month, year);

                // for get company id
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

                //check overTime is active ?
                checkOverTimeIsActiveForSelectedShift(CompanyId);

                // get max SN of employee,whose active salary status is true 

                sqlDB.fillDataTable("select EmpCardNo,EmpId,EmpName,MAX(SN) as SN,EmpType,EmpTypeId,EmpStatus,CompanyId,convert(varchar(11),EffectiveDate,105) as EffectiveDate,convert(varchar(2),EffectiveDate,105) as EffectiveDay,SftId  from v_Personnel_EmpSeparation group by SftId,EmpCardNo,EmpName,EmpType,EmpTypeId,EmpStatus,EmpId,CompanyId,EffectiveDate having EmpId='" + dtSeperationInfo.Rows[0]["EmpId"].ToString() + "' AND  EmpStatus in ('" + dtSeperationInfo.Rows[0]["SeparationType"].ToString() + "')  AND CompanyId='" + CompanyId + "'", dtRunningEmp = new DataTable());
               

                for (int i = 0; i < dtRunningEmp.Rows.Count; i++)
                {
                   
                    
                    //------------------------------------------------------------

                    // get essential information of a certain employee 
                    sqlDB.fillDataTable("select EmpId,EmpCardNo,BasicSalary,MedicalAllownce,FoodAllownce,ConvenceAllownce,HouseRent,TechnicalAllownce,OthersAllownce,EmpPresentSalary,AttendanceBonus,LunchCount,LunchAllownce,DptId,GrdName,DsgId,sftId,Convert(varchar(11),EarnLeaveDate,111 )as EarnLeaveDate,EmpType from v_Personnel_EmpCurrentStatus where SN=" + dtRunningEmp.Rows[i]["SN"].ToString() + "", dtCertainEmp = new DataTable());

                    // get joining date and earnleave date 
                    sqlDB.fillDataTable("select convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,convert(varchar(11),EmpJoiningDate,111) as EmpJoiningDateForEarnLeaveCalculate from Personnel_EmployeeInfo where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "'", dt = new DataTable());
                    ViewState["__getJoingingDate__"] = dt.Rows[0]["EmpJoiningDate"].ToString();
                    ViewState["__EmpJoiningDateForEarnLeaveCalculate__"] = dt.Rows[0]["EmpJoiningDateForEarnLeaveCalculate"].ToString();

                    // get leave information of a certain employee
                    sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId,StateStatus from v_tblAttendanceRecord where ATTStatus='lv' AND MonthName ='" + year + '-' + month + "' AND EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' And AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", dtLeaveInfo = new DataTable());
                    getAllLeaveInformation();

                    
                    // get present information of a certain employee
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(11),ATTDate,111) as ATTDate,InHour,InMin,OutHour,OutMin,ATTStatus from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus In ('P','L') AND MonthName='" + MonthName + "' AND AttDate Not  " + ViewState["__setPredicate__"].ToString() + " AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' ", dtPresent = new DataTable());


                    // get late information of a certain employee
                    sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate, EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='L' AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", dtLate = new DataTable());

                    // get absent information of a certain employee
                    sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='A' AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", dtAbsent = new DataTable());

                    // check attendance bonus of a certain employee
                    checkForAttendanceBonus();


                    // check shift overtime by company shift
                    DataRow[] dr = dtShiftListForCheckOverTime.Select("SftId=" + dtCertainEmp.Rows[0]["SftId"].ToString() + " AND SftOverTime='true'");


                    if (dr.Length > 0) OverTimeCalculation(dtCertainEmp.Rows[0]["EmpId"].ToString(), month, year, selectDays, double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString()));
                    else
                    {
                        ViewState["__getTotalOvertimeAmt__"] = "0";
                        ViewState["__getTotalOverTime__"] = "0";
                        getOTRate = 0;
                    }

                    // get advance information of a certain employee 
                    sqlDB.fillDataTable("select Max(SL) as SL,AdvanceId,PaidInstallmentNo,InstallmentNo from Payroll_AdvanceInfo Where EmpCardNo='" + dtRunningEmp.Rows[i]["EmpCardNo"].ToString() + "' AND EmpTypeId=" + dtRunningEmp.Rows[i]["EmpTypeId"].ToString() + " AND PaidStatus='0'  group By AdvanceId,PaidInstallmentNo,InstallmentNo ", dtAdvanceInfo = new DataTable());

                    if (dtAdvanceInfo.Rows.Count > 0)
                    {
                        // get information employee are aggre for give advance installment ?
                        sqlDB.fillDataTable("select InstallmentAmount,PaidInstallmentNo,PaidMonth from Payroll_AdvanceSetting where AdvanceId ='" + dtAdvanceInfo.Rows[0]["AdvanceId"].ToString() + "' AND PaidMonth='" + getMonthYear + "'", dtCutAdvance = new DataTable());

                    }

                    // get loan information of a certain employee 
                    sqlDB.fillDataTable("select Max(SL) as SL,LoanId,PaidInstallmentNo,InstallmentNo from Payroll_LoanInfo Where EmpCardNo='" + dtRunningEmp.Rows[i]["EmpCardNo"].ToString() + "' AND EmpTypeId=" + dtRunningEmp.Rows[i]["EmpTypeId"].ToString() + " AND PaidStatus='0' group By LoanId,PaidInstallmentNo,InstallmentNo ", dtLoanInfo = new DataTable());

                    if (dtLoanInfo.Rows.Count > 0)
                    {
                        // get information employee are aggre for give loan installment ?
                        sqlDB.fillDataTable("select InstallmentAmount,PaidInstallmentNo,PaidMonth from Payroll_LoanSetting where LoanId ='" + dtLoanInfo.Rows[0]["LoanId"].ToString() + "' AND PaidMonth='" + getMonthYear + "'", dtCutLoan = new DataTable());

                    }

                    //if (rbEmpTypeList.SelectedItem.ToString().ToLower().Equals("staff"))checkLunchCost();
                    //else getLunchCost = 0;

                    saveMonthlyPayrollSheet(month, year, Days, dtRunningEmp.Rows[i]["EmpName"].ToString(), i, selectDays, int.Parse(ViewState["__getUserId__"].ToString()), ViewState["__EmpJoiningDateForEarnLeaveCalculate__"].ToString(), dtCertainEmp.Rows[0]["EarnLeaveDate"].ToString(), dtRunningEmp.Rows[i]["EmpTypeId"].ToString(), dtCertainEmp.Rows[0]["EmpType"].ToString(), dtRunningEmp.Rows[i]["EffectiveDate"].ToString(), dtRunningEmp.Rows[i]["EmpStatus"].ToString(), CompanyId);
                    
                    
                }

                //lblMessage.InnerText = "success->Successfully payroll generated of " + dtRunningEmp.Rows.Count + " " + rbEmpTypeList.SelectedItem.ToString() + "";
               
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
            }
        }


        double getOTRate;
        double TotalOTTaka;
        private void OverTimeCalculation(string getEmpId, string getMonth, string getYear, string SelectedDays, double getBasic)   // over time calculation 
        {
            try
            {



                DataTable dtMonthlyOT = new DataTable();

                getOTRate = 0;
                //------For Count Weekend Task of Employee-------------------
                sqlDB.fillDataTable("select sum (OverTime) as OverTime from tblAttendanceRecord where EmpId='" + getEmpId + "' AND ATTDate>='" + getYear + "-" + getMonth + "-01' AND ATTDate<='" + getYear + "-" + getMonth + "-" + SelectedDays + "'", dtMonthlyOT);

                getOTRate = Math.Round(getBasic / 104, 2);

                if (dtMonthlyOT.Rows.Count > 0)
                {
                    ViewState["__getTotalOvertimeAmt__"] = Math.Round(getOTRate * double.Parse(dtMonthlyOT.Rows[0]["OverTime"].ToString()), 0);
                    ViewState["__getTotalOverTime__"] = dtMonthlyOT.Rows[0]["OverTime"].ToString();
                }
                else
                {
                    ViewState["__getTotalOvertimeAmt__"] = "0";
                    ViewState["__getTotalOverTime__"]="0";
                }
            }
            catch (Exception ex)
            {
                ViewState["__getTotalOvertimeAmt__"] = "0";
                ViewState["__getTotalOverTime__"] = "0";

                //lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        DataTable dtWeekendAsLeave;
        private void getAllLeaveInformation()   // all leave information
        {
            try
            {
                ViewState["__cl__"] = 0; ViewState["__sl__"] = 0; ViewState["__al__"] = 0; ViewState["__WeekendAsLeave__"] = "0"; ViewState["__ml__"] = "0";
                ViewState["__ofl__"] = 0; ViewState["__othl__"] = 0;
                if (dtLeaveInfo.Rows.Count > 0)
                {

                    DataRow[] dr = dtLeaveInfo.Select("StateStatus ='Casula Leave'");
                    ViewState["__cl__"] = dr.Length;

                    DataRow[] dr1 = dtLeaveInfo.Select("StateStatus ='Sick Leave'");
                    ViewState["__sl__"] = dr1.Length;


                    DataRow[] dr2 = dtLeaveInfo.Select("StateStatus ='Annual Leave'");
                    ViewState["__al__"] = dr2.Length;

                    DataRow[] dr3 = dtLeaveInfo.Select("StateStatus ='Maternity Leave'");
                    ViewState["__ml__"] = dr3.Length;

                    DataRow[] dr4 = dtLeaveInfo.Select("StateStatus ='Official Purpose Leave'");
                    ViewState["__ofl__"] = dr4.Length;

                    DataRow[] dr5 = dtLeaveInfo.Select("StateStatus ='Others Leave'");
                    ViewState["__othl__"] = dr5.Length;

                    DataRow[] drWeekendAsLeave = dtLeaveInfo.Select("AttDate " + ViewState["__setPredicate__"].ToString());
                    ViewState["__WeekendAsLeave__"] = drWeekendAsLeave.Length;
                }

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        int getAttendanceBonus;

        private void checkForAttendanceBonus()   // check attendance bonus
        {
            try
            {
                if (int.Parse(dtGetMonthSetup.Rows[0]["TotalWorkingDays"].ToString()) == dtPresent.Rows.Count)
                {
                    if (dtLate.Rows.Count >= 3) getAttendanceBonus = 0;
                    else getAttendanceBonus = int.Parse(dtCertainEmp.Rows[0]["AttendanceBonus"].ToString());
                }
                else getAttendanceBonus = 0;
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private void salarySheetClearForCertainEmployeeByMonthYearAndEmpId(string month, string year)
        {
            try
            {
                cmd = new SqlCommand("delete from Payroll_MonthlySalarySheet where  Month(YearMonth)='" + month + "' AND Year(YearMonth)='" + year + "' AND EmpId='" + dtSeperationInfo.Rows[0]["EmpId"].ToString() + "' AND IsSeperationGeneration='1'", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        

        private void CalculateHolidayAndWeekendOvertime(string EmpId)
        {
            try
            {
                //  sqlDB.fillDataTable("select ");
            }
            catch { }
        }

        double getLunchCost;
        private void checkLunchCost()   // check lunch account
        {

            if (bool.Parse(dtCertainEmp.Rows[0]["LunchCount"].ToString()).Equals(false))
            {

                getLunchCost = Math.Round(double.Parse(dtCertainEmp.Rows[0]["LunchAllownce"].ToString()) * double.Parse(dtGetMonthSetup.Rows[0]["TotalWorkingDays"].ToString()), 2);
            }
        }
        double getTotalSalary;
        double getTotalSalaryWithAllOT;
        double getNetPayable;
        double getAdvanceInstallment;
        double getLoanInstallment;
        double getPayable;

        private void getNetPayableCalculation(int getDays)   // net payable calculation
        {
            try
            {

                getNetPayable = 0;
                getAdvanceInstallment = 0;
                getLoanInstallment = 0;

                double getPresentSalary = double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString()) + double.Parse(dtCertainEmp.Rows[0]["TechnicalAllownce"].ToString());
                ViewState["__presentSalary__"] = getPresentSalary.ToString();

                try
                {
                    if (dtAdvanceInfo.Rows.Count > 0)
                        if (dtCutAdvance.Rows.Count > 0) getAdvanceInstallment = Math.Round(double.Parse(dtCutAdvance.Rows[0]["InstallmentAmount"].ToString()), 0);

                }
                catch { }

                try
                {
                    if (dtLoanInfo.Rows.Count > 0)
                        if (dtCutLoan.Rows.Count > 0) getLoanInstallment = Math.Round(double.Parse(dtCutLoan.Rows[0]["InstallmentAmount"].ToString()), 0);
                }
                catch { }

                // find Absent deduction
                double getAbsentAmount = Math.Round((dtAbsent.Rows.Count * double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString())) / getDays, 0);

                //get one day salary 
                double onDaySalary = Math.Round((1 * double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString())) / getDays, 0);

                ViewState["__absentFine__"] = getAbsentAmount.ToString();

                getPayable = 0;
                //  getPayable = Math.Round((getPresentSalary - getAbsentAmount), 0);
                // ViewState["__PayableDays__"];
                //-------------------------------------------------------------------
                getPayable = getPresentSalary * byte.Parse(ViewState["__PayableDays__"].ToString()) / getDays;
                getPayable = Math.Round((getPayable - getAbsentAmount), 0);

                ViewState["__TotalDeduction__"] = (getPresentSalary - getPayable).ToString();  // this line for get total deduction 

                getAbsentAmount = getDays - byte.Parse(ViewState["__PayableDays__"].ToString());  // now absent dayes enetrd in getabsentAmount

                ViewState["__absentFine__"] = (getAbsentAmount * onDaySalary) + double.Parse(ViewState["__absentFine__"].ToString());

                //---------------------------------------------------------------------------
                //   getNetPayable = Math.Round(((getPresentSalary + TotalOTTaka + getAttendanceBonus + getLunchCost) - (getAbsentAmount + getAdvanceInstallment + getLoanInstallment)), 0);

                getNetPayable = Math.Round(((getPayable + double.Parse(ViewState["__getTotalOvertimeAmt__"].ToString()) + getAttendanceBonus + getLunchCost) - (getAdvanceInstallment + getLoanInstallment)), 0);

                // to get finaly payble amount
                getTotalSalary = Math.Round((getNetPayable - double.Parse(dtStampDeduct.Rows[0]["StampDeduct"].ToString())), 0);



            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private bool joiningMonthIsEqual(string getMonth, string getYear, string EffectiveDate, string CompanyId)   //net payable calculation,compier joining time for generate salary sheet of month
        {
            try
            {
                string[] getJoiningMonth = ViewState["__getJoingingDate__"].ToString().Split('-');
                string getJoinMonth = getJoiningMonth[1] + "-" + getJoiningMonth[2];

                //  string getJoinDate=getJoinMonth
                string getCurrentMonth = getMonth + "-" + getYear;

                string[] getDismisDate = EffectiveDate.Split('-');

                int daysInMonth = DateTime.DaysInMonth(int.Parse(getDismisDate[0]), int.Parse(getDismisDate[1]));

                bool isProcess = false;

                //-------------------this predicate is check dismissdate is less form days in month----then check joni month year is equal with sepereation month year--------

                if (getJoinMonth.Equals(getCurrentMonth))
                {
                    if (int.Parse(getDismisDate[0]) < daysInMonth)
                    {
                        // if (getJoinMonth.Equals(getCurrentMonth))
                        isProcess = true;
                    }
                    //------------------------------------------------------------------------------------------------------------------------------------------------------------

                    //-------------------- test seperation  date and days in month is equal----and joining month year and seperation month year is equals--------------------- 
                    else if (getDismisDate[0].Equals(daysInMonth.ToString()))
                    {
                        if (int.Parse(getJoiningMonth[0]) == 1)
                            return false;
                        else isProcess = true;
                    }
                }
                //--------------------------------------------------------------------------------------------------------------------------------------------------------



                // Entered below option for calculate seperateion net payment

                if (isProcess)
                {
                    ViewState["__TechnicalAllowance__"] = dtCertainEmp.Rows[0]["TechnicalAllownce"].ToString();

                    string[] selectDates = EffectiveDate.Split('-');

                    //-------------Count Payable Days------------------------------------------------------------------

                    string getYearMonth = getYear + "-" + getMonth;

                    DataTable dtHoliday = new DataTable();
                    sqlDB.fillDataTable("select convert(varchar(11),HDate,105) as HDate from tblHolydayWork where CompanyId='" + CompanyId + "' AND HDate>='" + getYearMonth + "-" + getJoiningMonth[0] + "' AND HDate <='" + selectDates[2] + "-" + selectDates[1] + "-" + selectDates[0] + "' AND HDate not in (select AttDate from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv') ", dtHoliday);
                    byte HDCount = 0;


                    if (getJoiningMonth[0] != "1") getAttendanceBonus = 0;

                    for (byte b = 0; b < dtHoliday.Rows.Count; b++)
                    {
                        string[] dates = dtHoliday.Rows[b]["HDate"].ToString().Split('-');
                        // string [] joinDates=
                        DateTime HDay = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]));
                        DateTime join = new DateTime(int.Parse(getJoiningMonth[2]), int.Parse(getJoiningMonth[1]), int.Parse(getJoiningMonth[0]));

                        if (HDay >= join) HDCount += 1;
                    }

                    DataTable dtWDInfo;
                    string monthYear = getMonth + "-" + getYear;
                    sqlDB.fillDataTable("select distinct format(WeekendDate,'dd-MM-yyyy') as WeekendDate from v_Attendance_WeekendInfo where CompanyId='" + CompanyId + "' AND MonthName='" + monthYear + "' And WeekendDate>='" + getYearMonth + "-" + getJoiningMonth[0] + "' AND WeekendDate <='" + selectDates[2] + "-" + selectDates[1] + "-" + selectDates[0] + "' AND WeekendDate not in (select AttDate from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv')", dtWDInfo = new DataTable());

                    byte WDCount = 0;
                    for (byte b = 0; b < dtWDInfo.Rows.Count; b++)
                    {
                        string[] dates = dtWDInfo.Rows[b]["WeekendDate"].ToString().Split('-');
                        // string [] joinDates=
                        DateTime WDay = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]));
                        DateTime Join = new DateTime(int.Parse(getJoiningMonth[2]), int.Parse(getJoiningMonth[1]), int.Parse(getJoiningMonth[0]));

                        if (WDay >= Join) WDCount += 1;

                    }
                    ViewState["__PayableDays__"] = "0";

                    int PayableDays = WDCount + int.Parse(ViewState["__cl__"].ToString()) + int.Parse(ViewState["__sl__"].ToString()) + int.Parse(ViewState["__al__"].ToString()) + int.Parse(ViewState["__ofl__"].ToString()) + int.Parse(ViewState["__othl__"].ToString()) + dtPresent.Rows.Count + HDCount + dtAbsent.Rows.Count;
                    ViewState["__PayableDays__"] = PayableDays.ToString();
                    //--------------------------End--------------------------------------------------------------------

                    ViewState["__WeekendCount__"] = WDCount.ToString();
                    ViewState["__HolidayCount__"] = HDCount.ToString();


                    //int getDays = DateTime.DaysInMonth(int.Parse(getYear), int.Parse(getMonth));
                    //int LateDays = 0;

                    int getDays = DateTime.DaysInMonth(int.Parse(getYear), int.Parse(getMonth));
                    int LateDays = getDays - int.Parse(getJoiningMonth[0]);  // this line find out active days

                    LateDays = getDays - LateDays - 1;  // this line find out late days


                    if (getJoinMonth.Equals(getCurrentMonth)) LateDays = int.Parse(getDismisDate[0]) - int.Parse(getJoiningMonth[0]);  // this line find out active days

                    else LateDays = int.Parse(getDismisDate[0]) - 1;  // this line find out active days

                    LateDays = LateDays + 1;  // this line find out late days

                    /*
                    double NewGross = Math.Round((double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString()) / getDays) * LateDays, 0);

                    //NewGross = Math.Round((double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString())) - NewGross, 0);
                    ViewState["__presentSalary__"] = NewGross;
                    */

                    // to get absent amount 
                    // to get absent amount 
                    double getAbsentAmount = Math.Round((dtAbsent.Rows.Count * double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString())) / getDays, 0);

                    //get one day salary 
                    double onDaySalary = Math.Round((1 * double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString())) / getDays, 0);

                    ViewState["__absentFine__"] = getAbsentAmount.ToString();

                    getPayable = 0;
                    //-------------------------------------------------------------
                    getPayable = double.Parse(ViewState["__presentSalary__"].ToString()) * PayableDays / getDays;
                    getPayable = Math.Round(getPayable - getAbsentAmount, 0);

                    double getTechincalAllowance = 0;

                    if (double.Parse(ViewState["__TechnicalAllowance__"].ToString()) > 0)
                    {
                        getTechincalAllowance = double.Parse(ViewState["__TechnicalAllowance__"].ToString()) * PayableDays / getDays;
                    }
                    else
                    {
                        ViewState["__TechnicalAllowance__"] = "0";
                    }
                    ViewState["__TotalDeduction__"] = ((double.Parse(ViewState["__presentSalary__"].ToString()) + double.Parse(ViewState["__TechnicalAllowance__"].ToString())) - (getPayable + getTechincalAllowance)).ToString();  // this line for get total deduction 

                    getPayable += getTechincalAllowance;

                    //-------------------------------------------------------------

                    getAbsentAmount = getDays - byte.Parse(ViewState["__PayableDays__"].ToString());   // now getAbsentAmount=Due Absent days
                    ViewState["__absentFine__"] = (getAbsentAmount * onDaySalary) + double.Parse(ViewState["__absentFine__"].ToString());


                    getAdvanceInstallment = 0;
                    getLoanInstallment = 0;
                    try
                    {
                        if (dtAdvanceInfo.Rows.Count > 0)
                            if (dtCutAdvance.Rows.Count > 0) getAdvanceInstallment = Math.Round(double.Parse(dtCutAdvance.Rows[0]["InstallmentAmount"].ToString()), 0);

                    }
                    catch { }

                    try
                    {
                        if (dtLoanInfo.Rows.Count > 0)
                            if (dtCutLoan.Rows.Count > 0) getLoanInstallment = Math.Round(double.Parse(dtCutLoan.Rows[0]["InstallmentAmount"].ToString()), 0);
                    }
                    catch { }




                    // getNetPayable = Math.Round(((NewGross + TotalOTTaka + getAttendanceBonus + getLunchCost) - (getAbsentAmount + getAdvanceInstallment + getLoanInstallment)), 0);


                    getNetPayable = Math.Round(((getPayable + int.Parse(ViewState["__getTotalOvertimeAmt__"].ToString()) + getAttendanceBonus + getLunchCost) - (getAdvanceInstallment + getLoanInstallment)), 0);

                    // to get finaly  payble amount
                    getTotalSalary = Math.Round((getNetPayable - double.Parse(dtStampDeduct.Rows[0]["StampDeduct"].ToString())), 0);

                    ViewState["__presentSalary__"] = Math.Round(double.Parse(ViewState["__presentSalary__"].ToString()) + double.Parse(ViewState["__TechnicalAllowance__"].ToString()), 0);
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private void PayableDaysCalculation(string MonthName, string SelectedDay, string CompanyId)    // For Runing employee
        {
            try
            {
                DataTable dt;
                string monthYear = MonthName.Substring(3, 4) + "-" + MonthName.Substring(0, 2);

                sqlDB.fillDataTable("select distinct format(WeekendDate,'dd-MM-yyyy') as WeekendDate from v_Attendance_WeekendInfo where CompanyId='" + CompanyId + "' AND MonthName='" + MonthName + "' And WeekendDate>='" + monthYear + "-01' AND WeekendDate <='" + monthYear + "-" + SelectedDay + "' AND WeekendDate not in (select AttDate from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv')", dt = new DataTable());

                ViewState["__WeekendCount__"] = dt.Rows.Count.ToString();

                DataTable dtHoliday = new DataTable();

                sqlDB.fillDataTable("select convert(varchar(11),HDate,105) as HDate from tblHolydayWork where CompanyId='" + CompanyId + "' AND HDate>='" + monthYear + "- 01' AND HDate <='" + monthYear + "-" + SelectedDay + "' AND HDate not in (select AttDate from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv') ", dtHoliday);
                ViewState["__HolidayCount__"] = dtHoliday.Rows.Count.ToString();

                ViewState["__PayableDays__"] = "0";

                int PayableDays = dt.Rows.Count + int.Parse(ViewState["__cl__"].ToString()) + int.Parse(ViewState["__sl__"].ToString()) + int.Parse(ViewState["__al__"].ToString()) + int.Parse(ViewState["__ofl__"].ToString()) + int.Parse(ViewState["__othl__"].ToString()) + dtPresent.Rows.Count + dtHoliday.Rows.Count + dtAbsent.Rows.Count;

                ViewState["__PayableDays__"] = PayableDays.ToString();
            }
            catch { }

        }

        private void saveMonthlyPayrollSheet(string getMonth, string getYear, int getDays, string empName, int i, string selectedDay, int userId,string EmpJoiningDate,string EarnLeaveDate,string empTypeId, string empType, string EffectiveDate, string EmpStatusId,string CompanyId)
        {
            try
            {
                cmd = new SqlCommand("insert into Payroll_MonthlySalarySheet(CompanyId,SftId,EmpId,EmpCardNo,YearMonth,DaysInMonth,Activeday,WeekendHoliday,PayableDays," +
                      "CasualLeave,SickLeave,AnnualLeave,OfficialLeave,OthersLeave,FestivalHoliday,AbsentDay,PresentDay,EmpPresentSalary,BasicSalary,HouseRent,MedicalAllownce,ConvenceAllownce,FoodAllownce,TechnicalAllowance," +
                      "OthersAllownce,LunchAllowance,AdvanceDeduction,LoanDeduction,AbsentDeduction,AttendanceBonus,Payable,TotalOTHour,OTRate,TotalOTAmount,NetPayable,Stampdeduct,TotalSalary,DptId," +
                      "DsgId,GrdName,EmpTypeId,EmpStatus,UserId,IsSeperationGeneration,GenerateDate) " +

                      "values(@CompanyId,@SftId,@EmpId,@EmpCardNo,@YearMonth,@DaysInMonth,@Activeday,@WeekendHoliday,@PayableDays,@CasualLeave," +
                      "@SickLeave,@AnnualLeave,@OfficialLeave,@OthersLeave,@FestivalHoliday,@AbsentDay,@PresentDay,@EmpPresentSalary,@BasicSalary,@HouseRent,@MedicalAllownce,@ConvenceAllownce,@FoodAllownce," +
                      "@TechnicalAllowance,@OthersAllownce,@LunchAllowance,@AdvanceDeduction,@LoanDeduction,@AbsentDeduction,@AttendanceBonus,@Payable,@TotalOTHour,@OTRate,@TotalOTAmount,@NetPayable,@Stampdeduct,@TotalSalary,@DptId," +
                      "@DsgId,@GrdName,@EmpTypeId,@EmpStatus,@UserId,@IsSeperationGeneration,@GenerateDate)", sqlDB.connection);

                cmd.Parameters.AddWithValue("@CompanyId", dtRunningEmp.Rows[i]["CompanyId"].ToString());
                cmd.Parameters.AddWithValue("@SftId", dtRunningEmp.Rows[i]["SftId"].ToString());
                cmd.Parameters.AddWithValue("@EmpId", dtCertainEmp.Rows[0]["EmpId"].ToString());
                cmd.Parameters.AddWithValue("@EmpCardNo", dtCertainEmp.Rows[0]["EmpCardNo"].ToString());

                string getYearMonth = "01-" + getMonth + "-" + getYear;
                cmd.Parameters.AddWithValue("@YearMonth", convertDateTime.getCertainCulture(getYearMonth).ToString());
                cmd.Parameters.AddWithValue("@DaysInMonth", dtGetMonthSetup.Rows[0]["TotalDays"].ToString());
                cmd.Parameters.AddWithValue("@Activeday", dtGetMonthSetup.Rows[0]["TotalWorkingDays"].ToString());

                if (joiningMonthIsEqual(getMonth, getYear, EffectiveDate, CompanyId) == false)
                {
                    PayableDaysCalculation(getMonth + "-" + getYear, selectedDay, CompanyId);
                    getNetPayableCalculation(getDays);   // this function call to get net payable  amount
                }

                cmd.Parameters.AddWithValue("@WeekendHoliday", int.Parse(ViewState["__WeekendCount__"].ToString()) - int.Parse(ViewState["__WeekendAsLeave__"].ToString()));
                cmd.Parameters.AddWithValue("@PayableDays", int.Parse(ViewState["__PayableDays__"].ToString()) - dtAbsent.Rows.Count);

                cmd.Parameters.AddWithValue("@CasualLeave", ViewState["__cl__"].ToString());
                cmd.Parameters.AddWithValue("@SickLeave", ViewState["__sl__"].ToString());
                cmd.Parameters.AddWithValue("@AnnualLeave", ViewState["__al__"].ToString());
                cmd.Parameters.AddWithValue("@OfficialLeave", ViewState["__ofl__"].ToString());
                cmd.Parameters.AddWithValue("@OthersLeave", ViewState["__othl__"].ToString());

                cmd.Parameters.AddWithValue("@FestivalHoliday", ViewState["__HolidayCount__"].ToString());
                cmd.Parameters.AddWithValue("@AbsentDay", dtAbsent.Rows.Count.ToString());
                cmd.Parameters.AddWithValue("@PresentDay", dtPresent.Rows.Count);

                cmd.Parameters.AddWithValue("@EmpPresentSalary", ViewState["__presentSalary__"].ToString());
                cmd.Parameters.AddWithValue("@BasicSalary", dtCertainEmp.Rows[0]["BasicSalary"].ToString());
                cmd.Parameters.AddWithValue("@HouseRent", dtCertainEmp.Rows[0]["HouseRent"].ToString());
                cmd.Parameters.AddWithValue("@MedicalAllownce", dtCertainEmp.Rows[0]["MedicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@ConvenceAllownce", dtCertainEmp.Rows[0]["ConvenceAllownce"].ToString());
                cmd.Parameters.AddWithValue("@FoodAllownce", dtCertainEmp.Rows[0]["FoodAllownce"].ToString());
                cmd.Parameters.AddWithValue("@TechnicalAllowance", dtCertainEmp.Rows[0]["TechnicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@OthersAllownce", dtCertainEmp.Rows[0]["OthersAllownce"].ToString());
                cmd.Parameters.AddWithValue("@LunchAllowance", getLunchCost);

                cmd.Parameters.AddWithValue("@AdvanceDeduction", getAdvanceInstallment);
                cmd.Parameters.AddWithValue("@LoanDeduction", getLoanInstallment);

                // cmd.Parameters.AddWithValue("@AbsentDeduction", ViewState["__absentFine__"].ToString());

                cmd.Parameters.AddWithValue("@AbsentDeduction", ViewState["__TotalDeduction__"].ToString());
                cmd.Parameters.AddWithValue("@AttendanceBonus", getAttendanceBonus);

                cmd.Parameters.AddWithValue("@Payable", getPayable);
                cmd.Parameters.AddWithValue("@TotalOTHour", ViewState["__getTotalOverTime__"].ToString());

                cmd.Parameters.AddWithValue("@OTRate", getOTRate);
                cmd.Parameters.AddWithValue("@TotalOTAmount", ViewState["__getTotalOvertimeAmt__"].ToString());
                cmd.Parameters.AddWithValue("@NetPayable", getNetPayable);
                cmd.Parameters.AddWithValue("@Stampdeduct", Math.Round(double.Parse(dtStampDeduct.Rows[0]["StampDeduct"].ToString()), 0));
                cmd.Parameters.AddWithValue("@TotalSalary", getTotalSalary);


                cmd.Parameters.AddWithValue("@DptId", dtCertainEmp.Rows[0]["DptId"].ToString());
                cmd.Parameters.AddWithValue("@DsgId", dtCertainEmp.Rows[0]["DsgId"].ToString());
                cmd.Parameters.AddWithValue("@GrdName", dtCertainEmp.Rows[0]["GrdName"].ToString());
                cmd.Parameters.AddWithValue("@EmpTypeId", dtRunningEmp.Rows[i]["EmpTypeId"].ToString());
                cmd.Parameters.AddWithValue("@EmpStatus", dtRunningEmp.Rows[i]["EmpStatus"].ToString());

                cmd.Parameters.AddWithValue("@UserId", userId.ToString());
                cmd.Parameters.AddWithValue("@IsSeperationGeneration", "1");
                cmd.Parameters.AddWithValue("@GenerateDate", convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy"))).ToString();
                cmd.ExecuteNonQuery();

                Advance_And_Loan_StatusChange();  // For advance and loan status change


                


                lblMessage.InnerText="success->Processing completed of " + rbEmpTypeList.SelectedItem.ToString() + " " + empName + "";



                loadFinalBillInfo(getYear, getMonth, dtCertainEmp.Rows[0]["EmpId"].ToString(), EmpJoiningDate, EarnLeaveDate, dtStampDeduct.Rows[0]["StampDeduct"].ToString(), dtCertainEmp.Rows[0]["BasicSalary"].ToString());



            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private void loadFinalBillInfo( string year,string month,string EmpId,string EmpJoiningDate,string EarnLeaveDate,string stampCardDeduction,string basicSalary)
        {
            try
            {


                sqlDB.fillDataTable("select * from v_MonthlySalarySheet where Year(YearMonth)='" + year + "' And Month(YearMonth)='" + month + "' And EmpId='" + EmpId + "'", dt = new DataTable());
                Session["__FinalBillForm__"] = dt;

                sqlDB.fillDataTable("select MonthName from HRD_MonthNameBangla where MonthId='" + ddlMonthID.SelectedItem.Value.ToString().Substring(5, 2) + "'", dt = new DataTable());


                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=FinalBillForm-" + ddlMonthID.SelectedItem.ToString() + "-" + rbEmpTypeList.SelectedItem.ToString() + "-" + EmpJoiningDate + "-" + EarnLeaveDate + "-" + dt.Rows[0]["MonthName"].ToString() +"-"+stampCardDeduction+"-"+basicSalary+"-"+ddlCompanyList.SelectedItem.Value.ToString()+"');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }

    

        private void Advance_And_Loan_StatusChange()
        {
            try
            {
                if (dtCutAdvance.Rows.Count != 0)   // for change Advance status
                {
                    if (dtAdvanceInfo.Rows[0]["InstallmentNo"].ToString().Equals(dtAdvanceInfo.Rows[0]["PaidInstallmentNo"].ToString()))
                    {
                        cmd = new System.Data.SqlClient.SqlCommand("update Payroll_AdvanceInfo set PaidStatus ='1' where AdvanceId='" + dtAdvanceInfo.Rows[0]["AdvanceId"].ToString() + "'", sqlDB.connection);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }

            try
            {
                if (dtCutAdvance.Rows.Count != 0)   // for change Loan status
                {
                    if (dtLoanInfo.Rows[0]["InstallmentNo"].ToString().Equals(dtLoanInfo.Rows[0]["PaidInstallmentNo"].ToString()))
                    {
                        cmd = new System.Data.SqlClient.SqlCommand("update Payroll_LoanInfo set PaidStatus ='1' where LoanId='" + dtLoanInfo.Rows[0]["LoanId"].ToString() + "'", sqlDB.connection);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

    }
}