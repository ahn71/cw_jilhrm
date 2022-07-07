using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComplexScriptingWebControl;
using System.Drawing;
using System.Threading;
using System.Web.SessionState;
using SigmaERP.classes;

namespace SigmaERP.payroll
{
    public partial class separation_generation : System.Web.UI.Page
    {
        DataTable dt; SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            Session["OPERATION_PROGRESS"] = 0;

            if (!IsPostBack)
            {
                setPrivilege();
                classes.Employee.LoadEmpCardNoWithNameByCompanyRShiftForSeperationEmp(ddlEmpCardNo, ddlCompanyList.SelectedValue,ddlMonthID.SelectedValue==" "?"": Convert.ToDateTime(ddlMonthID.SelectedValue).ToString("yyyy-MM"));
                if (classes.Payroll.Office_IsGarments()) IsGarments = true;
                else IsGarments = false;
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
            }
        }


        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                Session["__getUserId__"]=ViewState["__getUserId__"] = getUserId;
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForOnlyWriteAction(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "separation_generation.aspx", ddlCompanyList,btnGeneration);

                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                loadMonthId(ViewState["__CompanyId__"].ToString());
              

               

            }
            catch { }
        }
        DataTable dtSeperationInfo = new DataTable();
        protected void btnGeneration_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
        //    probar.Style.Add("width", "0%");
           // probar.InnerHtml = "0%";

            separation_generation sg = new separation_generation();


            string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

           // getSelectedShiftId();

           
            lblMessage.InnerText = "";
            dt = new DataTable();

            string [] getDays = ddlMonthID.SelectedValue.ToString().Split('-');
            int days = DateTime.DaysInMonth(int.Parse(getDays[1]), int.Parse(getDays[0]));
          
            //Read_N_Write_WH(days,int.Parse(getDays[1]), getDays[2]);

            
           loadMonthSetup("1", getDays[0], getDays[1],CompanyId,sg);   // days ,month ,year

            generateMonthlySalarySheet(getDays[0] + "-" + getDays[1], getDays[0], getDays[1], days,sg);


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

        private void loadMonthId( string compnayId)// Updated By Nayem.
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select distinct MonthName,Format(FromDate,'MMM-yyyy') as YearMonth,MonthId From tblMonthSetup  where CompanyId='" + compnayId + "' order by MonthId desc", dt);
                ddlMonthID.DataTextField = "YearMonth";
                ddlMonthID.DataValueField = "MonthName";
                ddlMonthID.DataSource = dt;
                ddlMonthID.DataBind();
                ddlMonthID.Items.Insert(0, new ListItem(" ", " "));
            }
            catch { }
        }


        

        private  void loadWeekendInfo(string MonthYear, string selectDays,string CompanyId,separation_generation sg)
        {
            try
            {
                DataTable dt = new DataTable();
                string[] month = MonthYear.Split('-');
                MonthYear = month[1] + "-" + month[0];

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
        private  void loadMonthSetup(string days, string month, string year,string CompanyId,separation_generation sg)
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
        DataTable dtOverTime_stayTime;
        DataTable dtTiffin_Staff_WorkerTaka;
        DataTable dtShortleave;
        DataTable dtOtherspay;
        DataTable dtOthersDeduction;

        private void generateMonthlySalarySheet(string getMonthYear, string month, string year, int Days,separation_generation sg)
        {
            try
            {


                string MonthName = year + "-" + month;

               


                // for get company id
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                // get stamp card price
                sqlDB.fillDataTable("select StampDeduct from HRD_AllownceSetting where AllownceId =(select max(AllownceId) from HRD_AllownceSetting)", dtStampDeduct = new DataTable());
                sqlDB.fillDataTable("select WorkerTiffinTaka,StaffTiffinTaka from HRD_OthersSetting where CompanyId='" + CompanyId + "'", dtTiffin_Staff_WorkerTaka = new DataTable());



                //check overTime is active ?
               // checkOverTimeIsActiveForSelectedShift(CompanyId);

                if (rblGenerateType.SelectedValue.ToString().Equals("0"))   // generating type for all employee
                {
                    salarySheetClearByMonthYear(month, year, CompanyId);  // delete for all employees 

                    // get all regular employee at this time                  
                    sqlDB.fillDataTable("select  EmpCardNo,EmpName, SN,EmpType,EmpTypeId,EmpStatus,convert(varchar(11),EffectiveDate,105) as EffectiveDate,convert(varchar(2),EffectiveDate,105) as EffectiveDay,EmpStatus,CompanyId,SftId from v_Personnel_EmpSeparation where CompanyId ='" + CompanyId + "' AND YearMonth='" + MonthName + "' AND IsActive='True' order by EmpCardNo", dtRunningEmp = new DataTable());
                }

                else    // generating type for single employee
                {
                    if (txtEmpCardNo.Text.Trim().Length>=4)  // valid card justification
                    {
                        salarySheetClearByMonthYear(month, year, CompanyId, txtEmpCardNo.Text);

                        // get max SN of employee,whose active salary status is true 
                        sqlDB.fillDataTable("select  EmpCardNo,EmpName, SN,EmpType,EmpTypeId,EmpStatus,convert(varchar(11),EffectiveDate,105) as EffectiveDate,convert(varchar(2),EffectiveDate,105) as EffectiveDay,EmpStatus,CompanyId,SftId from v_Personnel_EmpSeparation where CompanyId ='" + CompanyId + "' AND YearMonth='" + MonthName + "' AND IsActive='True' AND EmpCardNo like '%"+txtEmpCardNo.Text.Trim()+"'  order by EmpCardNo", dtRunningEmp = new DataTable());


                    }
                    else
                    {
                        lblMessage.InnerText = "error->Please type valid card no of an employee";
                        txtEmpCardNo.Focus();
                    }
                }

                

                double getTime = Math.Round((double.Parse(dtRunningEmp.Rows.Count.ToString())) / 10, 0);
                //  System.Threading.Thread.Sleep(TimeSpan.FromSeconds(getTime));
                bool isgarments = classes.Payroll.Office_IsGarments();

                for (int i = 0; i < dtRunningEmp.Rows.Count; i++)
                {

                    int getValue = 0;
                    if (rblGenerateType.SelectedValue.ToString() != "1")
                    {
                        // for get operation progress--------------------------------

                        if (i != 0) getValue = (100 * i / (dtRunningEmp.Rows.Count - 1));
                       // probar.Style.Add("width", getValue.ToString() + "%");
                       // probar.InnerHtml = getValue.ToString() + "%";
                        System.Threading.Thread.Sleep(1000);
                    }
                    //------------------------------------------------------------

                    string selectDays = dtRunningEmp.Rows[i]["EffectiveDay"].ToString();
                   // string MonthName = getDays[1] + "-" + getDays[0];  

                    loadWeekendInfo(MonthName, selectDays,CompanyId,sg); // load weekend info for indivudal employee

                    // get essential information of a certain employee 
                    sqlDB.fillDataTable("select EmpId,EmpCardNo,BasicSalary,MedicalAllownce,FoodAllownce,ConvenceAllownce,HouseRent,TechnicalAllownce,OthersAllownce,EmpPresentSalary,AttendanceBonus,LunchCount,LunchAllownce,DptId,GrdName,DsgId,sftId,Convert(varchar(11),EarnLeaveDate,111 )as EarnLeaveDate,EmpType,DormitoryRent,PFAmount from v_Personnel_EmpCurrentStatus where SN=" + dtRunningEmp.Rows[i]["SN"].ToString() + "", dtCertainEmp = new DataTable());

                    // get Proximity number of a certain employee
                    
                    sqlDB.fillDataTable("select convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,convert(varchar(11),EmpJoiningDate,111) as EmpJoiningDateForEarnLeaveCalculate from Personnel_EmployeeInfo where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "'", dt = new DataTable());
                    ViewState["__getJoingingDate__"] = dt.Rows[0]["EmpJoiningDate"].ToString();
                    ViewState["__EmpJoiningDateForEarnLeaveCalculate__"] = dt.Rows[0]["EmpJoiningDateForEarnLeaveCalculate"].ToString();

                    // get leave information of a certain employee
                    sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId,StateStatus from v_tblAttendanceRecord where ATTStatus='lv' AND MonthName ='" + year + '-' + month + "' AND EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' And AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", dtLeaveInfo = new DataTable());
                    getAllLeaveInformation();

                    // get present information of a certain employee
                    if (isgarments)
                    {
                        sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(11),ATTDate,111) as ATTDate,InHour,InMin,OutHour,OutMin,ATTStatus from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus In ('P','L') AND MonthName='" + MonthName + "' AND AttDate Not  " + ViewState["__setPredicate__"].ToString() + " AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' AND PaybleDays='1' ", dtPresent = new DataTable());
                    }
                    else
                    {
                        sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(11),ATTDate,111) as ATTDate,InHour,InMin,OutHour,OutMin,ATTStatus from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus In ('P','L') AND MonthName='" + MonthName + "' AND AttDate Not  " + ViewState["__setPredicate__"].ToString() + " AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' ", dtPresent = new DataTable());
                    }

                    // get late information of a certain employee
                    if (isgarments)
                    {
                        sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate, EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='L' AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' AND PaybleDays='1'", dtLate = new DataTable());
                    }
                    else
                    {
                        sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate, EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='L' AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", dtLate = new DataTable());
                    }

                    // get absent information of a certain employee
                    if (isgarments)
                    {
                        sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='A' AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' Union select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus in('P','L') AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' AND PaybleDays='0' ", dtAbsent = new DataTable());
                    }
                    else
                    {
                        sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='A' AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' ", dtAbsent = new DataTable());
                    }

                    //get short leave

                    sqlDB.fillDataTable("select distinct convert(varchar(11),LvDate,111) as LvDate,EmpId from Leave_ShortLeave where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND LvDate >='" + year + '-' + month + '-' + "01" + "' AND LvDate <= '" + year + '-' + month + '-' + selectDays + "' ", dtShortleave = new DataTable());

                    //get Other's Pay

                    sqlDB.fillDataTable("select  ISNULL(Sum(OtherPay),0) OtherPay from Payroll_OthersPay where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND IsActive='1' ", dtOtherspay = new DataTable());

                    //get Other's Deduction

                    sqlDB.fillDataTable("select ISNULL(Sum(PAmount),0) PAmount from Payroll_Punishment where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND MonthName ='" + month + '-' + year + "' ", dtOthersDeduction = new DataTable());

                    // check attendance bonus of a certain employee
                    


                    getHourlyAmount(Days, double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString()));

                    // Call Over time Callculation for count OT taka

                    //dtOverTime_stayTime = new DataTable();
                    //sqlDB.fillDataTable("select Sum(OverTime) as OverTime,Sum(Convert(int,Substring(Convert(varchar,StayTime),0,3)))  as StayTime from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' and IsOverTime='1'", dtOverTime_stayTime);

                     dtOverTime_stayTime = new DataTable();
                   // sqlDB.fillDataTable("select Sum(OverTime) as OverTime,Sum(Convert(int,Substring(Convert(varchar,StayTime),0,3)))  as StayTime from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'  and IsOverTime='1'", dtOverTime_stayTime);
                     sqlDB.fillDataTable("Select  isnull(CAST(SUM(DATEDIFF(second, 0, OverTime)) / 3600 AS varchar(12)) + ':' + RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OverTime)) / 60 % 60 AS varchar(2)), 2) + ':' +RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OverTime)) % 60 AS varchar(2)), 2),'00:00:00') AS OverTime,isnull(CAST(SUM(DATEDIFF(second, 0, OtherOverTime)) / 3600 AS varchar(12)) + ':' +RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OtherOverTime)) / 60 % 60 AS varchar(2)), 2) + ':' + RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OtherOverTime)) % 60 AS varchar(2)), 2),'00:00:00') AS OtherOverTime,isnull(CAST(SUM(DATEDIFF(second, 0, OverTime)) / 3600 AS varchar(12)) + ':' + RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OverTime)) / 60 % 60 AS varchar(2)), 2) + ':' +RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OverTime)) % 60 AS varchar(2)), 2),'00:00:00')+isnull(CAST(SUM(DATEDIFF(second, 0, OtherOverTime)) / 3600 AS varchar(12)) + ':' +RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OtherOverTime)) / 60 % 60 AS varchar(2)), 2) + ':' + RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OtherOverTime)) % 60 AS varchar(2)), 2),'00:00:00') as TotalOverTime from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' and IsOverTime='1'", dtOverTime_stayTime);
                    //if (dtOverTime_stayTime.Rows[0]["OverTime"].ToString().Trim() == "")
                    //{

                    //    ViewState["__getTotalOvertimeAmt__"] = "0";
                    //    ViewState["__getTotalOverTime__"] = "0";
                    //    ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"] = "0";
                    //    ViewState["__Extra_OT_Amt_OfEmp__"] = "0";
                    //}
                    //else
                    //{
                    //    ViewState["__getTotalOvertimeAmt__"] = OverTimeHourlySalary * int.Parse(dtOverTime_stayTime.Rows[0]["OverTime"].ToString());
                    //    ViewState["__getTotalOverTime__"] = dtOverTime_stayTime.Rows[0]["OverTime"].ToString();
                    //    ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"] = "0";
                    //    ViewState["__Extra_OT_Amt_OfEmp__"] = "0";
                    //}

                     string time = dtOverTime_stayTime.Rows[0]["OverTime"].ToString();
                     string[] spltTime = time.Split(':');


                     string time2 = dtOverTime_stayTime.Rows[0]["OtherOverTime"].ToString();

                     string[] spltTime2 = time2.Split(':');


                     double hours = double.Parse(spltTime[0]) + double.Parse(spltTime2[0]);
                     double min = double.Parse(spltTime[1]) + double.Parse(spltTime2[1]);
                     double secods = double.Parse(spltTime[2]) + double.Parse(spltTime2[2]);

                     if (secods >= 60)
                     {
                         secods = secods / 60;
                         min = min + 1;
                         if (secods > 45)
                         {
                             min = min + 1;
                             secods = 0;
                         }

                     }
                     else if (secods > 30)
                     {
                         min = min + 1;
                         secods = 0;
                     }
                     if (min >= 60)
                     {
                         hours = hours + 1;
                         if (min == 60)
                         {
                             min = 0;
                         }
                         else
                             min = min / 60;
                     }
                     string h = hours.ToString().Length == 1 ? "0" + hours.ToString() : hours.ToString();
                     string m = min.ToString().Length == 1 ? "0" + min.ToString() : min.ToString();
                     string s = secods.ToString().Length == 1 ? "0" + secods.ToString() : secods.ToString();

                     string totalOverTime = h + ":" + m + ":" + s;
                     double minOttk = (OverTimeHourlySalary / 60) * min;
                     double hourlyot = OverTimeHourlySalary * hours;

                     ViewState["__getTotalOvertimeAmt__"] = minOttk + hourlyot;
                     ViewState["__getTotalOverTime__"] = totalOverTime;
                     ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"] = "0";
                     ViewState["__Extra_OT_Amt_OfEmp__"] = "0";
                     ViewState["__getOverTime__"] = dtOverTime_stayTime.Rows[0]["OverTime"].ToString();
                     ViewState["__getOtherOverTime__"] = dtOverTime_stayTime.Rows[0]["OtherOverTime"].ToString();
                     //}
                    string tiffincount = dtRunningEmp.Rows[i]["EmpTypeId"].ToString() == "1" ? dtTiffin_Staff_WorkerTaka.Rows[0]["WorkerTiffinTaka"].ToString() : dtTiffin_Staff_WorkerTaka.Rows[0]["StaffTiffinTaka"].ToString();
                    ViewState["__TiffinTaka__"] = tiffincount;
                    DataTable dtTiffin_Holidays = new DataTable();
                    sqlDB.fillDataTable("select ISNULL(Sum(TiffinCount),0) as TiffinCount,ISNULL(Sum(HolidayCount),0)  as HolidayCount from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", dtTiffin_Holidays);
                    ViewState["__HolidayTaka__"] = (float.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString()) / 30).ToString();
                    if (dtTiffin_Holidays.Rows.Count > 0)
                    {
                        ViewState["__Tiffindays__"] = dtTiffin_Holidays.Rows[0]["TiffinCount"].ToString();

                        ViewState["__TiffinBillAmount__"] = (float.Parse(dtTiffin_Holidays.Rows[0]["TiffinCount"].ToString()) * float.Parse(tiffincount)).ToString();
                        if (isgarments)
                        {
                            ViewState["__Holidays__"] = dtTiffin_Holidays.Rows[0]["HolidayCount"].ToString();
                            ViewState["__HolidayBillAmount__"] = ((float.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString()) / 30) * (float.Parse(dtTiffin_Holidays.Rows[0]["HolidayCount"].ToString()))).ToString();
                        }
                        else
                        {
                            ViewState["__Holidays__"] = dtTiffin_Holidays.Rows[0]["HolidayCount"].ToString();
                            ViewState["__HolidayBillAmount__"] = ((float.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString()) / 26) * (float.Parse(dtTiffin_Holidays.Rows[0]["HolidayCount"].ToString()))).ToString();
                        }
                    }
                    else
                    {
                        ViewState["__Tiffindays__"] = "0";
                        ViewState["__TiffinBillAmount__"] = "0";
                        ViewState["__Holidays__"] = "0";
                        ViewState["__HolidayBillAmount__"] = "0";
                    }
                   

                    // get advance information of a certain employee 
                    sqlDB.fillDataTable("select Max(SL) as SL,Payroll_AdvanceInfo.AdvanceId,Payroll_AdvanceInfo.PaidInstallmentNo,InstallmentNo from Payroll_AdvanceInfo inner join Payroll_AdvanceSetting on Payroll_AdvanceInfo.AdvanceId=Payroll_AdvanceSetting.AdvanceId Where EmpCardNo='" + dtRunningEmp.Rows[i]["EmpCardNo"].ToString() + "' AND EmpTypeId=" + dtRunningEmp.Rows[i]["EmpTypeId"].ToString() + " AND AND Payroll_AdvanceSetting.PaidMonth='" + getMonthYear + "'  group By Payroll_AdvanceInfo.AdvanceId,Payroll_AdvanceInfo.PaidInstallmentNo,InstallmentNo ", dtAdvanceInfo = new DataTable());

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
                    saveMonthlyPayrollSheet(month, year, Days, dtRunningEmp.Rows[i]["EmpName"].ToString(), i, selectDays, int.Parse(ViewState["__getUserId__"].ToString()), CompanyId, sg, getMonthYear);

                   // saveMonthlyPayrollSheet(month, year, Days, dtRunningEmp.Rows[i]["EmpName"].ToString(), i, selectDays, int.Parse(ViewState["__getUserId__"].ToString()), ViewState["__EmpJoiningDateForEarnLeaveCalculate__"].ToString(), dtCertainEmp.Rows[0]["EarnLeaveDate"].ToString(), dtRunningEmp.Rows[i]["EmpTypeId"].ToString(), dtCertainEmp.Rows[0]["EmpType"].ToString(), dtRunningEmp.Rows[i]["EffectiveDate"].ToString(), dtRunningEmp.Rows[i]["EmpStatus"].ToString(), CompanyId);
                    //   if (!isActiveLoop) break;
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ProcessingEnd(" + dtRunningEmp.Rows.Count.ToString() + ");", true);

                // lblMessage.InnerText = "success->Successfully payroll generated of "+dtRunningEmp.Rows.Count+" "+rbEmpTypeList.SelectedItem.ToString()+"";

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ProcessingEror(" + ex.Message + ");", true);
            }
        }
        private void salarySheetClearByMonthYear(string month, string year, string CompanyId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("delete from Payroll_MonthlySalarySheet where CompanyId='" + CompanyId + "'  AND Year(YearMonth)='" + year + "' AND Month(YearMonth)='" + month + "' AND IsSeperationGeneration='1'", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }
        private void salarySheetClearByMonthYear(string month, string year, string CompanyId, string empcardno)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("delete from Payroll_MonthlySalarySheet where CompanyId='" + CompanyId + "'  AND Year(YearMonth)='" + year + "' AND Month(YearMonth)='" + month + "'  AND EmpCardNo LIKE '%" + empcardno + "' AND IsSeperationGeneration='1'", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }
        private static string gettotalovertime(string time)
        {
            char[] delimiters = new char[] { ':', ' ' };
            string[] spltTime = time.Split(delimiters);

            string[] splthour = spltTime[0].Split('.');
            if (splthour.Count() == 1)
            {
                int hour = int.Parse(spltTime[0]);
                int minute = int.Parse(spltTime[1]);
                int seconds = int.Parse(spltTime[2].Substring(0, 2));
                return hour + ":" + minute + ":" + seconds;
            }
            else
            {
                int hour = int.Parse(splthour[0]);
                int minute = int.Parse(splthour[1]);
                int seconds = int.Parse(spltTime[1]);
                return hour + ":" + minute + ":" + seconds;
            }

        }
        double getOndaySalary;
        double NormalHourlySalary;
        double OverTimeHourlySalary;

        private void getHourlyAmount(int DaysInMonth, double Salary)
        {
            try
            {
                // getOndaySalary = Math.Round(GrossSalary / DaysInMonth, 0);
                // NormalHourlySalary = Math.Round(getOndaySalary/8);
                OverTimeHourlySalary = Math.Round(Salary / 208 * 2, 2); // here 208 is static.
            }
            catch { }
        }

        private  void salarySheetClearForCertainEmployeeByMonthYear(string month, string year, string CompanyId,separation_generation sg,string EmpCardNo)
        {
            try
            {
                cmd = new SqlCommand("delete from Payroll_MonthlySalarySheet where CompanyId='" + CompanyId + "' AND  Year(YearMonth)='" + year + "' AND Month(YearMonth)='" + month + "' AND EmpCardNo='" + EmpCardNo + "' AND IsSeperationGeneration='1'", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        private  void salarySheetClearForAllEmployeesByMonthYear(string month, string year, string CompanyId,separation_generation sg)
        {
            try
            {
                cmd = new SqlCommand("delete from Payroll_MonthlySalarySheet where CompanyId='" + CompanyId + "' AND Year(YearMonth)='" + year + "' AND Month(YearMonth)='" + month + "' AND IsSeperationGeneration='1'", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        DataTable dtShiftListForCheckOverTime;
        private  void checkOverTimeIsActiveForSelectedShift(string CompanyId,separation_generation sg)
        {
            try
            {
                dtShiftListForCheckOverTime = new DataTable();
                
                sqlDB.fillDataTable("select SftId,SftOverTime from HRD_Shift where CompanyId='" + CompanyId + "' AND SftId in (" + ViewState["__getRequiredSftId__"].ToString() + ")", dtShiftListForCheckOverTime);
                
            }
            catch { }
        }


        private  void saveMonthlyPayrollSheet(string getMonth,string getYear,int getDays,string empName,int i,string selectedDay,int userId,string CompanyId,separation_generation sg,string SDate)
        {
            try
            {  
                //SqlCommand cmd = new SqlCommand("insert into Payroll_MonthlySalarySheet(CompanyId,SftId,EmpId,EmpCardNo,YearMonth,DaysInMonth,Activeday,WeekendHoliday,PayableDays," +
                //      "CasualLeave,SickLeave,AnnualLeave,OfficialLeave,OthersLeave,FestivalHoliday,AbsentDay,PresentDay,EmpPresentSalary,BasicSalary,HouseRent,MedicalAllownce,ConvenceAllownce,FoodAllownce,TechnicalAllowance," +
                //      "OthersAllownce,LunchAllowance,AdvanceDeduction,LoanDeduction,AbsentDeduction,AttendanceBonus,Payable,TotalOTHour,OTRate,TotalOTAmount,NetPayable,Stampdeduct,TotalSalary,DptId," +
                //      "DsgId,GrdName,EmpTypeId,EmpStatus,UserId,IsSeperationGeneration,GenerateDate,OTHourForBuyer,OTAmountForBuyer,ExtraOTHour,ExtraOTAmount,NetPayableWithAllOTAmt) " +

                //      "values(@CompanyId,@SftId,@EmpId,@EmpCardNo,@YearMonth,@DaysInMonth,@Activeday,@WeekendHoliday,@PayableDays,@CasualLeave," +
                //      "@SickLeave,@AnnualLeave,@OfficialLeave,@OthersLeave,@FestivalHoliday,@AbsentDay,@PresentDay,@EmpPresentSalary,@BasicSalary,@HouseRent,@MedicalAllownce,@ConvenceAllownce,@FoodAllownce," +
                //      "@TechnicalAllowance,@OthersAllownce,@LunchAllowance,@AdvanceDeduction,@LoanDeduction,@AbsentDeduction,@AttendanceBonus,@Payable,@TotalOTHour,@OTRate,@TotalOTAmount,@NetPayable,@Stampdeduct,@TotalSalary,@DptId," +
                //      "@DsgId,@GrdName,@EmpTypeId,@EmpStatus,@UserId,@IsSeperationGeneration,@GenerateDate,@OTHourForBuyer,@OTAmountForBuyer,@ExtraOTHour,@ExtraOTAmount,@NetPayableWithAllOTAmt)", sqlDB.connection);
                SqlCommand cmd = new SqlCommand("insert into Payroll_MonthlySalarySheet(CompanyId,SftId,EmpId,EmpCardNo,YearMonth,DaysInMonth,Activeday,WeekendHoliday,PayableDays," +
                      "CasualLeave,SickLeave,AnnualLeave,OfficialLeave,OthersLeave,FestivalHoliday,AbsentDay,PresentDay,EmpPresentSalary,BasicSalary,HouseRent,MedicalAllownce,ConvenceAllownce,FoodAllownce,TechnicalAllowance," +
                      "OthersAllownce,LunchAllowance,AdvanceDeduction,LoanDeduction,AbsentDeduction,AttendanceBonus,Payable,TotalOTHour,OTRate,TotalOTAmount,NetPayable,Stampdeduct,TotalSalary,DptId," +
                      "DsgId,GrdName,EmpTypeId,EmpStatus,UserId,IsSeperationGeneration,GenerateDate,OTHourForBuyer,OTAmountForBuyer,ExtraOTHour,ExtraOTAmount,NetPayableWithAllOTAmt,LateDays,LateFine,TiffinDays,TiffinTaka,TiffinBillAmount,HolidayWorkingDays,HolidayTaka,HoliDayBillAmount,DormitoryRent,ProvidentFund,TotalOverTime,TotalOtherOverTime,OthersPay,OthersDeduction,ShortLeave,ProfitTax) " +

                      "values(@CompanyId,@SftId,@EmpId,@EmpCardNo,@YearMonth,@DaysInMonth,@Activeday,@WeekendHoliday,@PayableDays,@CasualLeave," +
                      "@SickLeave,@AnnualLeave,@OfficialLeave,@OthersLeave,@FestivalHoliday,@AbsentDay,@PresentDay,@EmpPresentSalary,@BasicSalary,@HouseRent,@MedicalAllownce,@ConvenceAllownce,@FoodAllownce," +
                      "@TechnicalAllowance,@OthersAllownce,@LunchAllowance,@AdvanceDeduction,@LoanDeduction,@AbsentDeduction,@AttendanceBonus,@Payable,@TotalOTHour,@OTRate,@TotalOTAmount,@NetPayable,@Stampdeduct,@TotalSalary,@DptId," +
                      "@DsgId,@GrdName,@EmpTypeId,@EmpStatus,@UserId,@IsSeperationGeneration,@GenerateDate,@OTHourForBuyer,@OTAmountForBuyer,@ExtraOTHour,@ExtraOTAmount,@NetPayableWithAllOTAmt,@LateDays,@LateFine,@TiffinDays,@TiffinTaka,@TiffinBillAmount,@HolidayWorkingDays,@HolidayTaka,@HoliDayBillAmount,@DormitoryRent,@ProvidentFund,@TotalOverTime,@TotalOtherOverTime,@OthersPay,@OthersDeduction,@ShortLeave,@ProfitTax)", sqlDB.connection);


                cmd.Parameters.AddWithValue("@CompanyId", dtRunningEmp.Rows[i]["CompanyId"].ToString());
                cmd.Parameters.AddWithValue("@SftId", dtRunningEmp.Rows[i]["SftId"].ToString());
                cmd.Parameters.AddWithValue("@EmpId", dtCertainEmp.Rows[0]["EmpId"].ToString());
                cmd.Parameters.AddWithValue("@EmpCardNo", dtCertainEmp.Rows[0]["EmpCardNo"].ToString());

                string getYearMonth = "01-" + getMonth + "-" + getYear;
                cmd.Parameters.AddWithValue("@YearMonth", convertDateTime.getCertainCulture(getYearMonth).ToString());
                cmd.Parameters.AddWithValue("@DaysInMonth", dtGetMonthSetup.Rows[0]["TotalDays"].ToString());
                cmd.Parameters.AddWithValue("@Activeday", dtGetMonthSetup.Rows[0]["TotalWorkingDays"].ToString());

                if (joiningMonthIsEqual(getMonth, getYear, CompanyId, SDate, sg) == false)
                {
                    //if (dtRunningEmp.Rows[i]["EmpStatus"].ToString().Equals("1"))
                    //{
                    //    if (ViewState["__ml__"].ToString().Equals("0"))
                    //    {
                            PayableDaysCalculation(getMonth + "-" + getYear, selectedDay, CompanyId, sg);

                            checkForAttendanceBonus(getMonth, getYear, dtCertainEmp.Rows[0]["EmpId"].ToString());
                            getNetPayableCalculation(getDays, sg);   // this function call to get net payable  amount
                    //    }
                    //    else
                    //    {
                    //        getPayableDaysCalculationForML(getMonth + "-" + getYear, selectedDay, sg);
                    //        getNetPayableCalculation(getDays, sg);   // this function call to get net payable  amount
                    //    }
                    //}
                    //else if (dtRunningEmp.Rows[i]["EmpStatus"].ToString().Equals("8"))
                    //{
                    //    if (ViewState["__ml__"].ToString().Equals("0"))
                    //    {
                    //        PayableDaysCalculation(getMonth + "-" + getYear, selectedDay, CompanyId, sg);
                    //        getNetPayableCalculation(getDays, sg);   // this function call to get net payable  amount
                    //    }
                    //    else
                    //    {
                    //        getPayableDaysCalculationForML(getMonth + "-" + getYear, selectedDay, sg);
                    //        getNetPayableCalculation(getDays, sg);   // this function call to get net payable  amount
                    //    }

                    //}
                }


                cmd.Parameters.AddWithValue("@WeekendHoliday", int.Parse(ViewState["__WeekendCount__"].ToString()) - int.Parse(ViewState["__WeekendAsLeave__"].ToString()));
                cmd.Parameters.AddWithValue("@PayableDays", float.Parse(ViewState["__PayableDays__"].ToString()));

                cmd.Parameters.AddWithValue("@CasualLeave", ViewState["__cl__"].ToString());
                cmd.Parameters.AddWithValue("@SickLeave", ViewState["__sl__"].ToString());
                cmd.Parameters.AddWithValue("@AnnualLeave", ViewState["__al__"].ToString());
                cmd.Parameters.AddWithValue("@OfficialLeave", ViewState["__ofl__"].ToString());
                cmd.Parameters.AddWithValue("@OthersLeave", ViewState["__othl__"].ToString());

                cmd.Parameters.AddWithValue("@FestivalHoliday", ViewState["__HolidayCount__"].ToString());
                cmd.Parameters.AddWithValue("@AbsentDay", dtAbsent.Rows.Count.ToString());
                cmd.Parameters.AddWithValue("@PresentDay", float.Parse(dtPresent.Rows.Count.ToString()));

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

                cmd.Parameters.AddWithValue("@OTRate", OverTimeHourlySalary);
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
                cmd.Parameters.AddWithValue("@OTHourForBuyer", OT_Hour_ForBuyer_AsRegular);
                cmd.Parameters.AddWithValue("@OTAmountForBuyer", ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"].ToString());
                cmd.Parameters.AddWithValue("@ExtraOTHour", Extra_OT_Hour_OfEmp);
                cmd.Parameters.AddWithValue("@ExtraOTAmount", ViewState["__Extra_OT_Amt_OfEmp__"].ToString());
                cmd.Parameters.AddWithValue("@NetPayableWithAllOTAmt", getNetPayableWithAllOTAmt);

                cmd.Parameters.AddWithValue("@LateDays", ViewState["__LateDays__"].ToString());
                cmd.Parameters.AddWithValue("@LateFine", ViewState["__LateFine__"].ToString());
                cmd.Parameters.AddWithValue("@TiffinDays", ViewState["__Tiffindays__"].ToString());
                cmd.Parameters.AddWithValue("@TiffinTaka", ViewState["__TiffinTaka__"].ToString());
                cmd.Parameters.AddWithValue("@TiffinBillAmount", ViewState["__TiffinBillAmount__"].ToString());
                cmd.Parameters.AddWithValue("@HolidayWorkingDays", ViewState["__Holidays__"].ToString());
                cmd.Parameters.AddWithValue("@HolidayTaka", ViewState["__HolidayTaka__"].ToString());
                cmd.Parameters.AddWithValue("@HoliDayBillAmount", ViewState["__HolidayBillAmount__"].ToString());
                cmd.Parameters.AddWithValue("@DormitoryRent", dtCertainEmp.Rows[0]["DormitoryRent"].ToString());
                cmd.Parameters.AddWithValue("@ProvidentFund", dtCertainEmp.Rows[0]["PFAmount"].ToString());
                cmd.Parameters.AddWithValue("@TotalOverTime", ViewState["__getOverTime__"].ToString());
                cmd.Parameters.AddWithValue("@TotalOtherOverTime", ViewState["__getOtherOverTime__"].ToString());
                cmd.Parameters.AddWithValue("@OthersPay", dtOtherspay.Rows[0]["OtherPay"].ToString());
                cmd.Parameters.AddWithValue("@OthersDeduction", dtOthersDeduction.Rows[0]["PAmount"].ToString());
                cmd.Parameters.AddWithValue("@ShortLeave", dtShortleave.Rows.Count);
                cmd.Parameters.AddWithValue("@ProfitTax", dtCertainEmp.Rows[0]["IncomeTax"].ToString());
                cmd.ExecuteNonQuery();

                Advance_And_Loan_StatusChange(sg);  // For advance and loan status change


             //  lbProcessingStatus.Items.Add("Processing completed of "+dtRunningEmp.Rows[i]["EmpType"].ToString() +" "+empName+" Card No. "+dtRunningEmp.Rows[i]["EmpCardNo"].ToString()+""); 

            }
            catch (Exception ex)
            {
                //lblMessage.InnerText = "error->" + ex.Message;
            }
        }


        private  void Advance_And_Loan_StatusChange(separation_generation sg)
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
                if (dtCutLoan.Rows.Count != 0)   // for change Loan status
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

        private void PayableDaysCalculation(string MonthName, string SelectedDay,string CompanyId)    // For Runing employee
        {
            try
            {
                DataTable dt;
                string monthYear = MonthName.Substring(3, 4) + "-" + MonthName.Substring(0, 2);

                sqlDB.fillDataTable("select distinct format(WeekendDate,'dd-MM-yyyy') as WeekendDate from v_Attendance_WeekendInfo where CompanyId='" + CompanyId + "' AND MonthName='" + MonthName + "' And WeekendDate>='" + monthYear + "-01' AND WeekendDate <='" + monthYear + "-" + SelectedDay + "' AND WeekendDate not in (select AttDate from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv')", dt = new DataTable());
                    
                ViewState["__WeekendCount__"] = dt.Rows.Count.ToString();

                DataTable dtHoliday = new DataTable();

                sqlDB.fillDataTable("select convert(varchar(11),HDate,105) as HDate from tblHolydayWork where CompanyId='" + CompanyId + "' AND HDate>='" + monthYear +"- 01' AND HDate <='" +monthYear+ "-" + SelectedDay +"' AND HDate not in (select AttDate from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv') ", dtHoliday);
                ViewState["__HolidayCount__"] = dtHoliday.Rows.Count.ToString();

                ViewState["__PayableDays__"] = "0";
               
                int PayableDays = dt.Rows.Count + int.Parse(ViewState["__cl__"].ToString()) + int.Parse(ViewState["__sl__"].ToString()) + int.Parse(ViewState["__al__"].ToString()) + int.Parse(ViewState["__ofl__"].ToString()) + int.Parse(ViewState["__othl__"].ToString()) + dtPresent.Rows.Count + dtHoliday.Rows.Count;
               
                ViewState["__PayableDays__"] = PayableDays.ToString();
            }
            catch { }

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

                    DataRow[] dr = dtLeaveInfo.Select("StateStatus ='Casual Leave'");
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
        private void checkForAttendanceBonus(string month, string year, string EmpId)   // check attendance bonus
        {
            try
            {
                DataTable dtApplydate = new DataTable();
                // get applydate information of a certain employee
                sqlDB.fillDataTable("SELECT EmpId,Convert(varchar(7),Lv_date,126) FROM Leave_ApplyDate where EmpId='" + EmpId + "' and Convert(varchar(7),Lv_date,126)='" + year + "-" + month + "'", dtApplydate);

                //if (int.Parse(dtGetMonthSetup.Rows[0]["TotalWorkingDays"].ToString()) == dtPresent.Rows.Count)
                //{
                if (dtLate.Rows.Count >= 1 || dtApplydate.Rows.Count > 0 || dtAbsent.Rows.Count >= 1 || int.Parse(dtGetMonthSetup.Rows[0]["TotalWorkingDays"].ToString()) != int.Parse(ViewState["__PayableDays__"].ToString())) getAttendanceBonus = 0;
                else getAttendanceBonus = int.Parse(dtCertainEmp.Rows[0]["AttendanceBonus"].ToString());
                //}
                //else getAttendanceBonus = 0;
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private void salarySheetClearOfSeperationGeneration(string month, string year)
        {
            try
            {
                cmd = new SqlCommand("delete from Payroll_MonthlySalarySheet where Month='" + month + "' AND Year='" + year + "' AND IsSeperationGeneration='1'", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        double getOTRate;
        
        double TotalOTTaka;
        double TotalExtraOTTaka;
        int OT_Hour_ForBuyer_AsRegular;
        int Extra_OT_Hour_OfEmp;
        int GetTotalOT_Hour;
        int check_OverTime;
        static byte TotalWorkingDays;


        private void OverTimeCalculation(string getEmpId, string getMonth, string getYear, string SelectedDays, double getBasic, separation_generation sg)   // over time calculation 
        {
            try
            {
                TotalExtraOTTaka = 0;
                OT_Hour_ForBuyer_AsRegular = 0;
                GetTotalOT_Hour = 0;
                Extra_OT_Hour_OfEmp = 0;

                string FromDate = getYear + "-" + getMonth + "-01";
                string ToDate = getYear + "-" + getMonth + "-" + SelectedDays;

                DataTable dtMonthlyOT = new DataTable();

                getOTRate = 0;
                getOTRate = Math.Round((getBasic / 208) * 2, 2); //Here Average Day of month 30 and weekend 4 so total days 26 and daily working ours 8 so that 26*8=208 ((basic/208)*2)
                //------For Count Weekend Task of Employee-------------------
                sqlDB.fillDataTable("select sum (OverTime) as OverTime,OverTimeCheck from v_tblAttendanceRecord where IsActive='1' AND EmpId='" + getEmpId + "' AND ATTDate>='" + getYear + "-" + getMonth + "-01' AND ATTDate<='" + getYear + "-" + getMonth + "-" + SelectedDays + "' Group by OverTimeCheck", dtMonthlyOT);
                if (dtMonthlyOT.Rows[0]["OverTimeCheck"].ToString() == "" || dtMonthlyOT.Rows[0]["OverTimeCheck"].ToString() == "False")
                {
                    ViewState["__getTotalOvertimeAmt__"] = "0";
                    ViewState["__getTotalOverTime__"] = "0";
                    ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"] = "0";
                    ViewState["__Extra_OT_Amt_OfEmp__"] = "0";
                }
                else
                {
                    if (IsGarments)
                    {
                        if (dtMonthlyOT.Rows[0]["OverTime"].ToString() == "") GetTotalOT_Hour = 0;
                        else GetTotalOT_Hour = int.Parse(dtMonthlyOT.Rows[0]["OverTime"].ToString());
                        check_OverTime = (int.Parse(dtGetMonthSetup.Rows[0]["TotalWorkingDays"].ToString()) * 2);  // Calculate overtime for checking desired monthly overtime for buyr
                        if (GetTotalOT_Hour > check_OverTime)
                        {
                            Extra_OT_Hour_OfEmp = GetTotalOT_Hour - check_OverTime;
                            OT_Hour_ForBuyer_AsRegular = GetTotalOT_Hour - Extra_OT_Hour_OfEmp;
                        }
                        else
                        {
                            Extra_OT_Hour_OfEmp = GetTotalOT_Hour;
                            OT_Hour_ForBuyer_AsRegular = 0;
                        }
                    }                    
                    if (dtMonthlyOT.Rows.Count > 0)
                    {
                        if (!IsGarments)
                        {
                            ViewState["__getTotalOvertimeAmt__"] = Math.Round(getOTRate * double.Parse(dtMonthlyOT.Rows[0]["OverTime"].ToString()), 0);
                            ViewState["__getTotalOverTime__"] = dtMonthlyOT.Rows[0]["OverTime"].ToString();
                            ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"] = "0";
                            ViewState["__Extra_OT_Amt_OfEmp__"] = "0";
                        }
                        else
                        {
                            ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"] = Math.Round(getOTRate * double.Parse(OT_Hour_ForBuyer_AsRegular.ToString()), 0);
                            ViewState["__Extra_OT_Amt_OfEmp__"] = Math.Round(getOTRate * double.Parse(Extra_OT_Hour_OfEmp.ToString()), 0);
                            ViewState["__getTotalOvertimeAmt__"] = Math.Round(getOTRate * double.Parse(GetTotalOT_Hour.ToString()), 0);


                            ViewState["__getTotalOverTime__"] = GetTotalOT_Hour;
                        }
                    }
                    else
                    {                        
                        ViewState["__getTotalOvertimeAmt__"] = "0";
                        ViewState["__getTotalOverTime__"] = "0";
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

      
        double getLunchCost;
        private void checkLunchCost()   // check lunch account
        {

            if (bool.Parse(dtCertainEmp.Rows[0]["LunchCount"].ToString()).Equals(true))
            {

                getLunchCost = Math.Round(double.Parse(dtCertainEmp.Rows[0]["LunchAllownce"].ToString()) * double.Parse(dtGetMonthSetup.Rows[0]["TotalWorkingDays"].ToString()), 2);
            }
            else getLunchCost = 0;
        }
        double getNetPayableWithAllOTAmt;
        double getTotalSalary;
        double getTotalSalaryWithAllOT;
        double getNetPayable;
        double getAdvanceInstallment;
        double getLoanInstallment;
        double getPayable;
        static bool IsGarments = false;

        //private void getNetPayableCalculation(int getDays)   // net payable calculation
        //{
        //    try
        //    {

        //        getNetPayable = 0;
        //        getAdvanceInstallment = 0;
        //        getLoanInstallment = 0;

        //        double getPresentSalary = double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString()) + double.Parse(dtCertainEmp.Rows[0]["TechnicalAllownce"].ToString());
        //        ViewState["__presentSalary__"] = getPresentSalary.ToString();
               
        //        try
        //        {
        //            if (dtAdvanceInfo.Rows.Count > 0)
        //                if (dtCutAdvance.Rows.Count > 0) getAdvanceInstallment = Math.Round(double.Parse(dtCutAdvance.Rows[0]["InstallmentAmount"].ToString()), 0);

        //        }
        //        catch { }

        //        try
        //        {
        //            if (dtLoanInfo.Rows.Count > 0)
        //                if (dtCutLoan.Rows.Count > 0) getLoanInstallment = Math.Round(double.Parse(dtCutLoan.Rows[0]["InstallmentAmount"].ToString()), 0);
        //        }
        //        catch { }

        //        // find Absent deduction
        //        double getAbsentAmount = Math.Round((dtAbsent.Rows.Count * double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString())) / getDays, 0);

        //        //get one day salary 
        //        double onDaySalary = Math.Round((1 * double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString())) / getDays, 0);

        //        ViewState["__absentFine__"] = getAbsentAmount.ToString();

        //        getPayable = 0;
        //        //  getPayable = Math.Round((getPresentSalary - getAbsentAmount), 0);
        //        // ViewState["__PayableDays__"];
        //        //-------------------------------------------------------------------
        //        getPayable = getPresentSalary * byte.Parse(ViewState["__PayableDays__"].ToString()) / getDays;
        //        getPayable = Math.Round((getPayable - getAbsentAmount), 0);

        //        ViewState["__TotalDeduction__"] = (getPresentSalary - getPayable).ToString();  // this line for get total deduction 

        //        getAbsentAmount = getDays - byte.Parse(ViewState["__PayableDays__"].ToString());  // now absent dayes enetrd in getabsentAmount

        //        ViewState["__absentFine__"] = (getAbsentAmount * onDaySalary) + double.Parse(ViewState["__absentFine__"].ToString());

        //        //---------------------------------------------------------------------------
        //        //   getNetPayable = Math.Round(((getPresentSalary + TotalOTTaka + getAttendanceBonus + getLunchCost) - (getAbsentAmount + getAdvanceInstallment + getLoanInstallment)), 0);

        //        getNetPayable = Math.Round(((getPayable + double.Parse(ViewState["__getTotalOvertimeAmt__"].ToString()) + getAttendanceBonus + getLunchCost) - (getAdvanceInstallment + getLoanInstallment)), 0);

        //        // to get finaly payble amount
        //        getTotalSalary = Math.Round((getNetPayable - double.Parse(dtStampDeduct.Rows[0]["StampDeduct"].ToString())), 0);

                 

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.InnerText = "error->" + ex.Message;
        //    }
        //}



        private void getNetPayableCalculation(int DaysInMonth, separation_generation pg)   // net payable calculation
        {
            try
            {

                getNetPayable = 0;
                getAdvanceInstallment = 0;
                getLoanInstallment = 0;
                getNetPayableWithAllOTAmt = 0;
                double getPresentSalary = double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString());
                ViewState["__presentSalary__"] = getPresentSalary.ToString();
                // for advance deduction
                try
                {
                    if (dtAdvanceInfo.Rows.Count > 0)
                        if (dtCutAdvance.Rows.Count > 0) getAdvanceInstallment = Math.Round(double.Parse(dtCutAdvance.Rows[0]["InstallmentAmount"].ToString()), 0);

                }
                catch { }

                // for loan deduction
                try
                {
                    if (dtLoanInfo.Rows.Count > 0)
                        if (dtCutLoan.Rows.Count > 0) getLoanInstallment = Math.Round(double.Parse(dtCutLoan.Rows[0]["InstallmentAmount"].ToString()), 0);
                }
                catch { }

                int getAbsendDaysFromLate = (dtLate.Rows.Count >= 3) ? (int)dtLate.Rows.Count / 3 : 0;
                //find late days
                ViewState["__LateDays__"] = getAbsendDaysFromLate;

                //ViewState["__PayableDays__"] = (int.Parse(ViewState["__PayableDays__"].ToString()) - getAbsendDaysFromLate);

                // getAbsendDaysFromLate += dtAbsent.Rows.Count;
                //find absent days
                ViewState["__TotalAbsentDays__"] = dtAbsent.Rows.Count;

                double lateDeduction = Math.Round(double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString()) / DaysInMonth * getAbsendDaysFromLate, 2);
                ViewState["__LateFine__"] = lateDeduction;
                // find Absent deduction
                double getAbsentAmount = Math.Round(double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString()) / DaysInMonth * dtAbsent.Rows.Count, 2);

                //get one day salary 
                double onDaySalary = (1 * double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString())) / DaysInMonth;

                ViewState["__TotalDeduction__"] = getAbsentAmount.ToString();

                ViewState["__PFDeduction"] = dtCertainEmp.Rows[0]["PFAmount"].ToString();

                getPresentSalary = onDaySalary * double.Parse(ViewState["__PayableDays__"].ToString());

                getPayable = 0;
                getPayable = Math.Round(((getPresentSalary + double.Parse(dtOtherspay.Rows[0]["OtherPay"].ToString())) - (lateDeduction + double.Parse(dtCertainEmp.Rows[0]["IncomeTax"].ToString()) + double.Parse(dtCertainEmp.Rows[0]["PFAmount"].ToString()) + getAdvanceInstallment + getLoanInstallment + double.Parse(dtOthersDeduction.Rows[0]["PAmount"].ToString()) + double.Parse(dtCertainEmp.Rows[0]["DormitoryRent"].ToString()))), 0);

                // ViewState["__PayableDays__"];
                //-------------------------------------------------------------------
                // get total salary 


                //ViewState[""] = (getPresentSalary -  getPayable).ToString();  // this line for get total deduction 

                // getAbsentAmount = getDays - float.Parse( ViewState["__PayableDays__"].ToString());  // now absent dayes enetrd in getabsentAmount

                // ViewState["__absentFine__"] = (getAbsentAmount * onDaySalary) + double.Parse( ViewState["__absentFine__"].ToString());

                //---------------------------------------------------------------------------
                getNetPayable = Math.Round(((getPayable + float.Parse(ViewState["__getTotalOvertimeAmt__"].ToString()) + getAttendanceBonus + float.Parse(ViewState["__TiffinBillAmount__"].ToString()) + float.Parse(ViewState["__HolidayBillAmount__"].ToString()))), 0);


                // to get finaly payble amount
                getTotalSalary = Math.Round((getNetPayable - double.Parse(dtStampDeduct.Rows[0]["StampDeduct"].ToString())), 0);

            }
            catch (Exception ex)
            {
                // lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private  bool joiningMonthIsEqual(string getMonth, string getYear, string CompanyId, string selectdates, separation_generation sg)   //net payable calculation,compier joining time for generate salary sheet of month
        {
            try
            {
                string[] getJoiningMonth = ViewState["__getJoingingDate__"].ToString().Split('-');

                string getJoinMonth = getJoiningMonth[1] + "-" + getJoiningMonth[2];

                string getCurrentMonth = getMonth + "-" + getYear;

                string[] selectDates = selectdates.Trim().Split('-');


                // below option for checking joining date-month-year is equal of current month 
                if (getJoinMonth.Equals(getCurrentMonth) && int.Parse(getJoiningMonth[0].ToString()) != 1)
                {

                    //-------------Count Payable Days------------------------------------------------------------------

                    string getYearMonth = getYear + "-" + getMonth;

                    DataTable dtHoliday = new DataTable();
                    sqlDB.fillDataTable("select convert(varchar(11),HDate,105) as HDate from tblHolydayWork where CompanyId='" + CompanyId + "' AND HDate>='" + getYearMonth + "-" + getJoiningMonth[0] + "' AND HDate <='" + selectDates[2] + "-" + selectDates[1] + "-" + selectDates[0] + "' AND HDate not in (select AttDate from tblAttendanceRecord where EmpId='" +dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv') ", dtHoliday);
                    byte HDCount = 0;


                    if (getJoiningMonth[0] != "1") getAttendanceBonus = 0;

                    for (byte b = 0; b < dtHoliday.Rows.Count; b++)
                    {
                        string[] dates = dtHoliday.Rows[b]["HDate"].ToString().Split('-');

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
                    int PayableDays = WDCount + int.Parse(ViewState["__cl__"].ToString()) + int.Parse(ViewState["__sl__"].ToString()) + int.Parse(ViewState["__al__"].ToString()) + int.Parse(ViewState["__ofl__"].ToString()) + int.Parse(ViewState["__othl__"].ToString()) + dtPresent.Rows.Count + HDCount;
                    ViewState["__PayableDays__"] = PayableDays.ToString();
                    //--------------------------End--------------------------------------------------------------------

                    ViewState["__WeekendCount__"] = WDCount.ToString();
                    ViewState["__HolidayCount__"] = HDCount.ToString();


                    int getDays = DateTime.DaysInMonth(int.Parse(getYear), int.Parse(getMonth));
                    int LateDays = getDays - int.Parse(getJoiningMonth[0]);  // this line find out active days

                    LateDays = getDays - LateDays - 1;  // this line find out late days

                    /*--------------------------For something rong------------------------------------------------------------------------------
                    double NewGross = Math.Round((double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString()) / getDays) * LateDays, 0); 

                    NewGross = Math.Round((double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString())) - NewGross, 0);
                    ViewState["__presentSalary__"] = NewGross;
                    
                   ------------------------------------------------------------------------------------------------------------------------
                    */
                    ViewState["__presentSalary__"] = double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString()) + double.Parse(dtCertainEmp.Rows[0]["TechnicalAllownce"].ToString());

                    getNetPayableCalculation(getDays, sg);

                    //// to get absent amount 
                    //double getAbsentAmount = Math.Round((dtAbsent.Rows.Count * double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString())) / getDays, 0);

                    ////get one day salary 
                    //double onDaySalary = Math.Round((1 * double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString())) / getDays, 0);

                    //ViewState["__absentFine__"] = getAbsentAmount.ToString();

                    //getPayable = 0;
                    ////   getPayable = NewGross - getAbsentAmount;

                    ////-------------------------------------------------------------
                    //getPayable = double.Parse(ViewState["__presentSalary__"].ToString()) * PayableDays / getDays;
                    //getPayable = Math.Round(getPayable - getAbsentAmount, 0);

                    //ViewState["__TotalDeduction__"] = (double.Parse(ViewState["__presentSalary__"].ToString()) - getPayable).ToString();  // this line for get total deduction 


                    ////-------------------------------------------------------------

                    //getAbsentAmount = getDays - byte.Parse(ViewState["__PayableDays__"].ToString());   // now getAbsentAmount=Due Absent days
                    //ViewState["__absentFine__"] = (getAbsentAmount * onDaySalary) + double.Parse(ViewState["__absentFine__"].ToString());


                    //getAdvanceInstallment = 0;
                    //getLoanInstallment = 0;
                    //try
                    //{
                    //    if (dtAdvanceInfo.Rows.Count > 0)
                    //        if (dtCutAdvance.Rows.Count > 0) getAdvanceInstallment = Math.Round(double.Parse(dtCutAdvance.Rows[0]["InstallmentAmount"].ToString()), 0);

                    //}
                    //catch { }

                    //try
                    //{
                    //    if (dtLoanInfo.Rows.Count > 0)
                    //        if (dtCutLoan.Rows.Count > 0) getLoanInstallment = Math.Round(double.Parse(dtCutLoan.Rows[0]["InstallmentAmount"].ToString()), 0);
                    //}
                    //catch { }




                    //// getNetPayable = Math.Round(((NewGross + TotalOTTaka + getAttendanceBonus + getLunchCost) - (getAbsentAmount + getAdvanceInstallment + getLoanInstallment)), 0);


                    //getNetPayable = Math.Round(((getPayable + float.Parse(ViewState["__getTotalOvertimeAmt__"].ToString()) + getAttendanceBonus + getLunchCost) - (getAdvanceInstallment + getLoanInstallment)), 0);

                    //// to get finaly  payble amount
                    //getTotalSalary = Math.Round((getNetPayable - double.Parse(dtStampDeduct.Rows[0]["StampDeduct"].ToString())), 0);



                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private  void PayableDaysCalculation(string MonthName, string SelectedDay, string CompanyId, separation_generation sg)    // For Runing employee
        {
            try
            {
                DataTable dt;
                string monthYear = MonthName.Substring(3, 4) + "-" + MonthName.Substring(0, 2);
           //   sqlDB.fillDataTable("select distinct format(WeekendDate,'yyyy-MM-dd') as WeekendDate from v_Attendance_WeekendInfo where CompanyId='" + CompanyId + "'  AND  MonthName='" + MonthName + "' And WeekendDate >='" + monthYear + "-01' AND WeekendDate <='" + monthYear + '-' + SelectedDay + "' AND WeekendDate not in (select AttDate from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv')", dt = new DataTable());
                sqlDB.fillDataTable("select distinct format(ATTDate,'yyyy-MM-dd') as WeekendDate from tblAttendanceRecord where CompanyId='" + CompanyId + "'  AND  RIGHT(CONVERT(VARCHAR(10),ATTDate, 105), 7)='" + MonthName + "' and EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTStatus='W' ", dt = new DataTable());
                DataTable dtTemp = new DataTable();
                dtTemp.Columns.Add("AttDate", typeof(string));
                DataTable dtWC = new DataTable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int count = 0;
                    string date1 = Convert.ToDateTime(dt.Rows[i]["WeekendDate"].ToString()).AddDays(-1).ToString("yyyy/MM/dd");
                    string date2 = Convert.ToDateTime(dt.Rows[i]["WeekendDate"].ToString()).AddDays(1).ToString("yyyy/MM/dd");
                    DataRow[] result = dtAbsent.Select("AttDate='" + date1 + "'");
                    if (result.Count() > 0)
                    {
                        count++;
                    }
                    DataRow[] result2 = dtAbsent.Select("AttDate='" + date2 + "'");
                    if (result2.Count() > 0)
                    {
                        count++;
                    }
                    if (count > 1)
                    {
                        sqlDB.fillDataTable("SELECT EmpId FROM tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTDate='" + Convert.ToDateTime(dt.Rows[i]["WeekendDate"].ToString()).ToString("yyyy-MM-dd") + "' AND StayTime>'04:00:00' ", dtWC);
                        if (dtWC.Rows.Count == 0)
                        dtTemp.Rows.Add(Convert.ToDateTime(dt.Rows[i]["WeekendDate"].ToString()).ToString("yyyy/MM/dd"));
                    }
                }
                if (dtTemp.Rows.Count > 0)
                {
                    for (int k = 0; k < dtTemp.Rows.Count; k++)
                    {
                        dtAbsent.Rows.Add(dtTemp.Rows[k]["AttDate"].ToString(), "");
                        dt.Rows.RemoveAt(0);
                    }
                }
                ViewState["__WeekendCount__"] = dt.Rows.Count.ToString();


                DataTable dtHoliday = new DataTable();
                sqlDB.fillDataTable("select * from tblHolydayWork where CompanyId='" + CompanyId + "' AND HDate >='" + monthYear + "-01' AND HDate <='" + monthYear + '-' + SelectedDay + "' AND HDate not in (select AttDate from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv') ", dtHoliday);
                dtTemp = new DataTable();
                dtTemp.Columns.Add("AttDate", typeof(string));
                for (int i = 0; i < dtHoliday.Rows.Count; i++)
                {
                    int count = 0;
                    string date1 = Convert.ToDateTime(dtHoliday.Rows[i]["HDate"].ToString()).AddDays(-1).ToString("yyyy/MM/dd");
                    string date2 = Convert.ToDateTime(dtHoliday.Rows[i]["HDate"].ToString()).AddDays(1).ToString("yyyy/MM/dd");
                    DataRow[] result = dtAbsent.Select("AttDate='" + date1 + "'");
                    if (result.Count() > 0)
                    {
                        count++;
                    }
                    DataRow[] result2 = dtAbsent.Select("AttDate='" + date2 + "'");
                    if (result2.Count() > 0)
                    {
                        count++;
                    }
                    if (count > 1)
                    {
                        dtWC = new DataTable();
                        sqlDB.fillDataTable("SELECT EmpId FROM tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTDate='" + Convert.ToDateTime(dtHoliday.Rows[i]["HDate"].ToString()).ToString("yyyy-MM-dd") + "' AND StayTime>'04:00:00' ", dtWC);
                        dtTemp.Rows.Add(Convert.ToDateTime(dtHoliday.Rows[i]["HDate"].ToString()).ToString("yyyy/MM/dd"));
                    }
                }
                if (dtTemp.Rows.Count > 0)
                {
                    for (int k = 0; k < dtTemp.Rows.Count; k++)
                    {
                        dtAbsent.Rows.Add(dtTemp.Rows[k]["AttDate"].ToString(), "");
                        dtHoliday.Rows.RemoveAt(0);
                    }
                }

                ViewState["__HolidayCount__"] = dtHoliday.Rows.Count.ToString();

                ViewState["__PayableDays__"] = "0";
                //int PayableDays = dt.Rows.Count + int.Parse(ViewState["__cl__"].ToString()) + int.Parse(ViewState["__sl__"].ToString()) + int.Parse(ViewState["__al__"].ToString()) + dtPresent.Rows.Count + dtHoliday.Rows.Count + dtAbsent.Rows.Count;
                float PayableDays = float.Parse(dt.Rows.Count.ToString()) + float.Parse(ViewState["__cl__"].ToString()) + float.Parse(ViewState["__sl__"].ToString()) + float.Parse(ViewState["__al__"].ToString()) + float.Parse(dtPresent.Rows.Count.ToString()) + float.Parse(dtHoliday.Rows.Count.ToString()) + float.Parse(dtAbsent.Rows.Count.ToString()); // Sum for half day salary
                ViewState["__PayableDays__"] = PayableDays.ToString();
            }
            catch { }

        }

        private  void getPayableDaysCalculationForML(string MonthName, string SelectedDay, separation_generation sg)
        {
            try
            {
                DataTable dt;
                string monthYear = MonthName.Substring(3, 4) + "-" + MonthName.Substring(0, 2);
                if (dtPresent.Rows.Count == 0)
                {
                    ViewState["__WeekendCount__"] = "0";
                    ViewState["__HolidayCount__"] = "0";

                }
                else
                {
                    sqlDB.fillDataTable("select * from Attendance_WeekendInfo where  MonthName='" + MonthName + "' And WeekendDate >='" + dtPresent.Rows[0]["ATTDate"].ToString() + "' AND WeekendDate <='" + dtPresent.Rows[dtPresent.Rows.Count - 1]["ATTDate"].ToString() + "'", dt = new DataTable());
                    ViewState["__WeekendCount__"] = dt.Rows.Count.ToString();

                    DataTable dtHoliday = new DataTable();
                    sqlDB.fillDataTable("select * from tblHolydayWork where HDate >='" + dtPresent.Rows[0]["ATTDate"].ToString() + "' AND HDate <='" + dtPresent.Rows[dtPresent.Rows.Count - 1]["ATTDate"].ToString() + "'", dtHoliday);
                    ViewState["__HolidayCount__"] = dtHoliday.Rows.Count.ToString();
                }


                ViewState["__PayableDays__"] = "0";
                //int PayableDays = int.Parse(ViewState["__WeekendCount__"].ToString()) + int.Parse(ViewState["__cl__"].ToString()) + int.Parse(ViewState["__sl__"].ToString()) + int.Parse(ViewState["__al__"].ToString()) + int.Parse(ViewState["__ofl__"].ToString()) + int.Parse(ViewState["__othl__"].ToString()) + dtPresent.Rows.Count + int.Parse(ViewState["__HolidayCount__"].ToString()) + dtAbsent.Rows.Count;
                float PayableDays = float.Parse(ViewState["__WeekendCount__"].ToString()) + float.Parse(ViewState["__cl__"].ToString()) + float.Parse(ViewState["__sl__"].ToString()) + float.Parse(ViewState["__al__"].ToString()) + float.Parse(ViewState["__ofl__"].ToString()) + float.Parse(ViewState["__othl__"].ToString()) + float.Parse(dtPresent.Compute("Sum(PaybleDay)", "").ToString()) + int.Parse(ViewState["__HolidayCount__"].ToString()) +float.Parse(dtAbsent.Rows.Count.ToString());
                ViewState["__PayableDays__"] = PayableDays.ToString();
            }
            catch { }
        }

        private bool joiningMonthIsEqual(string getMonth, string getYear, string EffectiveDate,string CompanyId)   //net payable calculation,compier joining time for generate salary sheet of month
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
                    sqlDB.fillDataTable("select convert(varchar(11),HDate,105) as HDate from tblHolydayWork where CompanyId='"+CompanyId+"' AND HDate>='" + getYearMonth + "-" + getJoiningMonth[0] + "' AND HDate <='" + selectDates[2] + "-" + selectDates[1] + "-" + selectDates[0] + "' AND HDate not in (select AttDate from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv') ", dtHoliday);
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

                    LateDays = getDays - LateDays - 1;  // this line find out late daysdtRunningEmp


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

                    getPayable+=getTechincalAllowance;

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

                    ViewState["__presentSalary__"]=Math.Round(double.Parse(ViewState["__presentSalary__"].ToString())+double.Parse(ViewState["__TechnicalAllowance__"].ToString()),0);
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void rblGenerateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rblGenerateType.SelectedItem.Text.Equals("Partial"))
                {
                    ddlEmpCardNo.Enabled = true;
                    txtEmpCardNo.Enabled = true;
                   
                    string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

                  //  getSelectedShiftId();
                    classes.Employee.LoadEmpCardNoWithNameByCompanyRShiftForSeperationEmp(ddlEmpCardNo, CompanyId, ddlMonthID.SelectedValue == " " ? "" : Convert.ToDateTime(ddlMonthID.SelectedValue).ToString("yyyy-MM"));
                    
                }
                else
                {
                    ddlEmpCardNo.Enabled = false;
                    txtEmpCardNo.Enabled = false;

                }
                imgLoading.Visible = false;
            }
            catch { }
        }


        /*
        private void getSelectedShiftId()
        {
            try
            {
                string getRequiredSftId = "";
                if (!ddlShiftList.SelectedItem.Text.Trim().Equals("All")) getRequiredSftId = ddlShiftList.SelectedItem.Value.ToString() + ",";
                else
                    for (byte r = 0; r < ddlShiftList.Items.Count; r++)
                    {
                        getRequiredSftId += ddlShiftList.Items[r].Value.ToString() + ",";

                    }
                ViewState["__getRequiredSftId__"] = getRequiredSftId.Remove(getRequiredSftId.LastIndexOf(','));
            }
            catch { }
        }

        private void addAllTextInShift()
        {
            try
            {
                if (ddlShiftList.Items.Count > 2)
                    ddlShiftList.Items.Insert(1, new ListItem("All", "00"));
            }
            catch { }
        }
        */
        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                //classes.commonTask.LoadShift(ddlShiftList, CompanyId); addAllTextInShift();
                classes.Employee.LoadEmpCardNoWithNameByCompanyRShiftForSeperationEmp(ddlEmpCardNo, ddlCompanyList.SelectedValue, ddlMonthID.SelectedValue == " " ? "" : Convert.ToDateTime(ddlMonthID.SelectedValue).ToString("yyyy-MM"));
                loadMonthId(CompanyId);
                imgLoading.Visible = false;
                if(rblGenerateType.SelectedValue=="0")
                {
                    ddlEmpCardNo.Enabled = false;
                    txtEmpCardNo.Enabled = false;
                }
                else
                {
                    ddlEmpCardNo.Enabled = true;
                    txtEmpCardNo.Enabled = true;
                }
            }
            catch { }
        }

        protected void ddlShiftList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                rblGenerateType.SelectedIndex = 0;
            }
            catch { }
        }

        protected void ddlMonthID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                rblGenerateType.SelectedIndex = 0;
                classes.Employee.LoadEmpCardNoWithNameByCompanyRShiftForSeperationEmp(ddlEmpCardNo, ddlCompanyList.SelectedValue, ddlMonthID.SelectedValue == " " ? "" : Convert.ToDateTime(ddlMonthID.SelectedValue).ToString("yyyy-MM"));
                imgLoading.Visible = false;
            }
            catch { }
        }

        protected void ddlEmpCardNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                string[] getEmpCardNo = ddlEmpCardNo.SelectedItem.Text.Trim().Split(' ');
                txtEmpCardNo.Text = getEmpCardNo[0];
                imgLoading.Visible = false;
            }
            catch { }
        }

        //public static separation_generation sg;
        //[System.Web.Services.WebMethod(EnableSession = true)]

        //public static object Operation(string SDate, string SCompanyId, string GType, string ECardNo, string EmpId)
        //{
        //    try
        //    {
        //        // return false;
        //        HttpSessionState session = HttpContext.Current.Session;

        //        //Separate thread for long running operation

        //        string getUserId = session["__getUserId__"].ToString();
        //        sg = new separation_generation();
        //        string sdatesOrEfectiveDates = "";
        //        string setPredicate = "";
        //        ThreadPool.QueueUserWorkItem(delegate
        //        {
        //            dt = new DataTable();

        //            string[] getDays = SDate.Split('-');
        //            int days = DateTime.DaysInMonth(int.Parse(getDays[1]), int.Parse(getDays[0]));
        //            string MonthName = getDays[1] + "-" + getDays[0];
        //            //Read_N_Write_WH(days,int.Parse(getDays[1]), getDays[2]);


        //            loadMonthSetup("1", getDays[0], getDays[1], SCompanyId, sg);   // days ,month ,year




        //            // get stamp card price
        //            sqlDB.fillDataTable("select StampDeduct from HRD_AllownceSetting where AllownceId =(select max(AllownceId) from HRD_AllownceSetting)", dtStampDeduct = new DataTable());



        //            // for get company id
        //            string CompanyId = SCompanyId;



        //            //check overTime is active ?
        //            checkOverTimeIsActiveForSelectedShift(CompanyId, sg);

        //            if (GType.Equals("0"))   // generating type for all employee
        //            {
        //                salarySheetClearForAllEmployeesByMonthYear(getDays[0], getDays[1], CompanyId, sg);  // delete for all employees getDays[0]=month and getDays[1]=year

        //                // get all regular employee at this time                  
        //                sqlDB.fillDataTable("select  EmpCardNo,EmpName, SN,EmpType,EmpTypeId,EmpStatus,convert(varchar(11),EffectiveDate,105) as EffectiveDate,convert(varchar(2),EffectiveDate,105) as EffectiveDay,EmpStatus,CompanyId,SftId from v_Personnel_EmpSeparation where CompanyId ='" + CompanyId + "' AND YearMonth='" + MonthName + "' AND IsActive='True' order by EmpCardNo", dtRunningEmp = new DataTable());
        //            }

        //            else    // generating type for single employee
        //            {
        //                if (ECardNo.Trim().Length >= 4)  // valid card justification
        //                {
        //                    salarySheetClearForCertainEmployeeByMonthYear(getDays[0], getDays[1], CompanyId, sg, ECardNo); //getDays[0]=Month and getDays[1]=Year

        //                    // get max SN of employee,whose active salary status is true 
        //                    sqlDB.fillDataTable("select  EmpCardNo,EmpName, SN,EmpType,EmpTypeId,EmpStatus,convert(varchar(11),EffectiveDate,105) as EffectiveDate,convert(varchar(2),EffectiveDate,105) as EffectiveDay,EmpStatus,CompanyId,SftId from v_Personnel_EmpSeparation where CompanyId ='" + CompanyId + "' AND YearMonth='" + MonthName + "' AND IsActive='True' AND EmpCardNo like '%" + ECardNo + "'  order by EmpCardNo", dtRunningEmp = new DataTable());
        //                }

        //            }

        //            for (int i = 0; i < dtRunningEmp.Rows.Count; i++)
        //            {
        //                sdatesOrEfectiveDates = dtRunningEmp.Rows[i]["EffectiveDay"].ToString();
        //                loadWeekendInfo(MonthName, sdatesOrEfectiveDates, SCompanyId, sg);
        //                setPredicate = ViewState["__setPredicate__"].ToString();
        //                #region--for get operation progress value--------
        //                int getValue = 0;
        //                if (GType != "1") if (i != 0) getValue = (100 * i / (dtRunningEmp.Rows.Count - 1));
        //                #endregion-------------End get value-------------


        //                // get essential information of a certain employee 
        //                sqlDB.fillDataTable("select EmpId,EmpCardNo,BasicSalary,MedicalAllownce,FoodAllownce,ConvenceAllownce,HouseRent,TechnicalAllownce,OthersAllownce,EmpPresentSalary,AttendanceBonus,LunchCount,LunchAllownce,DptId,GrdName,DsgId,sftId from v_Personnel_EmpCurrentStatus where SN=" + dtRunningEmp.Rows[i]["SN"].ToString() + "", dtCertainEmp = new DataTable());

        //                // get Proximity number of a certain employee
        //                sqlDB.fillDataTable("select convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate from Personnel_EmployeeInfo where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "'", dt = new DataTable());
        //                ViewState["__getJoingingDate__"] = dt.Rows[0]["EmpJoiningDate"].ToString();

        //                // get leave information of a certain employee
        //                sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId,StateStatus from v_tblAttendanceRecord where ATTStatus='lv' AND MonthName ='" + getDays[1] + '-' + getDays[0] + "' AND EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' And AttDate >='" + getDays[1] + '-' + getDays[0] + '-' + "01" + "' AND AttDate <= '" + getDays[1] + '-' + getDays[0] + '-' + sdatesOrEfectiveDates + "'", dtLeaveInfo = new DataTable());
        //                getAllLeaveInformation();

        //                // get present information of a certain employee
        //                sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(11),ATTDate,111) as ATTDate,InHour,InMin,OutHour,OutMin,ATTStatus,CASE WHEN DATEDIFF(HOUR,'00:00:00',StayTime)<8 and DATEDIFF(HOUR,'00:00:00',StayTime)>=5 THEN (CONVERT(FLOAT,1) / 2)  ELSE CASE WHEN DATEDIFF(HOUR,'00:00:00',StayTime)>=8 then 1 else 0 end END AS 'PaybleDay' from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus In ('P','L') AND MonthName='" + MonthName + "' AND AttDate Not  " + setPredicate + " AND AttDate >='" + getDays[1] + '-' + getDays[0] + '-' + "01" + "' AND AttDate <= '" + getDays[1] + '-' + getDays[0] + '-' + sdatesOrEfectiveDates + "' ", dtPresent = new DataTable());

        //                // get late information of a certain employee
        //                sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate, EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='L' AND MonthName='" + MonthName + "' AND AttDate >='" + getDays[1] + '-' + getDays[0] + '-' + "01" + "' AND AttDate <= '" + getDays[1] + '-' + getDays[0] + '-' + sdatesOrEfectiveDates + "'", dtLate = new DataTable());

        //                // get absent information of a certain employee
        //                sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='A' AND MonthName='" + MonthName + "' AND AttDate >='" + getDays[1] + '-' + getDays[0] + '-' + "01" + "' AND AttDate <= '" + getDays[1] + '-' + getDays[0] + '-' + sdatesOrEfectiveDates + "'", dtAbsent = new DataTable());

        //                // check attendance bonus of a certain employee
        //                // checkForAttendanceBonus();

        //                // Call Over time Callculation for count OT taka

        //                // for checking overtime is active this shift 

        //                //  DataRow[] dr = dtShiftListForCheckOverTime.Select("SftId=" + dtCertainEmp.Rows[0]["SftId"].ToString() + " AND SftOverTime='true'");

        //                //if (dr.Length > 0) OverTimeCalculation(dtCertainEmp.Rows[0]["EmpId"].ToString(), "", "", "", double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString()));
        //                // else
        //                // {
        //                OverTimeCalculation(dtCertainEmp.Rows[0]["EmpId"].ToString(), getDays[0], getDays[1], sdatesOrEfectiveDates, double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString()), sg);

        //                //   }
        //                ViewState["__getMonthYear__"] = SDate;
        //                // get advance information of a certain employee 
        //                sqlDB.fillDataTable("select Max(SL) as SL,AdvanceId,PaidInstallmentNo,InstallmentNo from Payroll_AdvanceInfo Where EmpCardNo='" + dtRunningEmp.Rows[i]["EmpCardNo"].ToString() + "' AND EmpTypeId=" + dtRunningEmp.Rows[i]["EmpTypeId"].ToString() + " AND PaidStatus='0'  group By AdvanceId,PaidInstallmentNo,InstallmentNo ", dtAdvanceInfo = new DataTable());

        //                if (dtAdvanceInfo.Rows.Count > 0)
        //                {
        //                    // get information employee are aggre for give advance installment ?
        //                    sqlDB.fillDataTable("select InstallmentAmount,PaidInstallmentNo,PaidMonth from Payroll_AdvanceSetting where AdvanceId ='" + dtAdvanceInfo.Rows[0]["AdvanceId"].ToString() + "' AND PaidMonth='" + ViewState["__getMonthYear__"].ToString() + "'", dtCutAdvance = new DataTable());

        //                }
        //                else
        //                {
        //                    dtAdvanceInfo = new DataTable(); dtCutAdvance = new DataTable();
        //                    // this extra query for when 1 times advance are executed and next time again is needed to generea the same month salary
        //                    sqlDB.fillDataTable("select distinct AdvanceId,InstallmentAmount,PaidInstallmentNo,PaidMonth from v_Payroll_AdvanceSetting where  PaidMonth='" + ViewState["__getMonthYear__"].ToString() + "' AND EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "'", dtCutAdvance = new DataTable());
        //                    if (dtCutAdvance.Rows.Count > 0)
        //                        sqlDB.fillDataTable("select AdvanceId,InstallmentNo,PaidInstallmentNo from Payroll_AdvanceInfo Where AdvanceId='" + dtCutAdvance.Rows[0]["AdvanceId"].ToString() + "' ", dtAdvanceInfo = new DataTable());
        //                }

        //                // get loan information of a certain employee 
        //                sqlDB.fillDataTable("select Max(SL) as SL,LoanId,PaidInstallmentNo,InstallmentNo from Payroll_LoanInfo Where EmpCardNo='" + dtRunningEmp.Rows[i]["EmpCardNo"].ToString() + "' AND EmpTypeId=" + dtRunningEmp.Rows[i]["EmpTypeId"].ToString() + " AND PaidStatus='0' group By LoanId,PaidInstallmentNo,InstallmentNo ", dtLoanInfo = new DataTable());

        //                if (dtLoanInfo.Rows.Count > 0)
        //                {
        //                    // get information employee are aggre for give loan installment ?
        //                    sqlDB.fillDataTable("select InstallmentAmount,PaidInstallmentNo,PaidMonth from Payroll_LoanSetting where LoanId ='" + dtLoanInfo.Rows[0]["LoanId"].ToString() + "' AND PaidMonth='" + 123 + "'", dtCutLoan = new DataTable());
        //                }

        //                //if (rbEmpTypeList.SelectedItem.ToString().ToLower().Equals("staff"))checkLunchCost();
        //                //else getLunchCost = 0;

        //                saveMonthlyPayrollSheet(getDays[0], getDays[1], days, dtRunningEmp.Rows[i]["EmpName"].ToString(), i, sdatesOrEfectiveDates, int.Parse(getUserId), CompanyId, sg, SDate);

        //                //   if (!isActiveLoop) break;

        //                session["OPERATION_PROGRESS"] = getValue;
        //                Thread.Sleep(1000);
        //            }

        //            if (dtRunningEmp.Rows.Count == 0)
        //            {
        //                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "call me", "alertMessage();", true);
        //            }

        //        });

        //        return new { progress = 0 };

        //    }
        //    catch { return 0; }
        //}

        //[System.Web.Services.WebMethod(EnableSession = true)]
        //public static object OperationProgress()
        //{
        //    int operationProgress = 0;
        //    if (HttpContext.Current.Session["OPERATION_PROGRESS"] != null)
        //        operationProgress = (int)HttpContext.Current.Session["OPERATION_PROGRESS"];
        //    if (dtRunningEmp.Rows.Count == 0) return new { progress = 200 };
        //    else return new { progress = operationProgress };
        //}

       
    }
}