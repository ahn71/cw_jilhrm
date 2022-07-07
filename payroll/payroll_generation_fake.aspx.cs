using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.payroll
{
    public partial class payroll_generation_fake : System.Web.UI.Page
    {
        DataTable dt;
        SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["OPERATION_PROGRESS"] = 0;

            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();

            ProgressBar1.Minimum = 0;
            ProgressBar1.Maximum = 100;
            ProgressBar1.BackColor = System.Drawing.Color.Blue;
            ProgressBar1.ForeColor = Color.White;
            ProgressBar1.Height = new Unit(20);
            ProgressBar1.Width = new Unit(100);

            if (!IsPostBack)
            {
                setPrivilege();
                // classes.commonTask.loadEmpTypeInRadioButtonList(rbEmpTypeList);
                Session["__CID__"] = ddlCompanyList.SelectedValue;
                classes.Employee.LoadEmpCardNoWithNameByCompanyRShift(ddlEmpCardNo, ddlCompanyList.SelectedValue);
                //if (classes.Payroll.Office_IsGarments()) IsGarments = true;
                //else IsGarments = false;
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.Employee.LoadEmpCardNoWithNameByCompanyRShift(ddlEmpCardNo, ddlCompanyList.SelectedValue);
            }

            lblMessage.InnerText = "";
        }
        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {
                payroll_generation pg = new payroll_generation();
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                Session["__getUserId__"] = getUserId;
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForOnlyWriteAction(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "payroll_generation.aspx", ddlCompanyList, btnGenerate, btnBDTNoteGenerate);
                classes.commonTask.LoadShift(ddlShiftList, ViewState["__CompanyId__"].ToString());




            }
            catch { }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {

            string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

            // getSelectedShiftId();

            string[] getDays = txtGenerateMonth.Text.Trim().Split('-');
            int DaysInMonth = DateTime.DaysInMonth(int.Parse(getDays[2]), int.Parse(getDays[1]));





            //     loadWeekendInfo(getDays[1] + "-" + getDays[2], getDays[0]);
            loadMonthSetup("1", getDays[1], getDays[2], CompanyId);

            //Session["OPERATION_PROGRESS"] = 10;
            //  Thread.Sleep(1000);

            //if (rbGenaratingType.SelectedValue.ToString() == "0") salarySheetClearByMonthYear(getDays[1], getDays[2], CompanyId);
            //else salarySheetClearByMonthYear(getDays[1], getDays[2], CompanyId,ddlEmpCardNo.SelectedValue);
            generateMonthlySalarySheet(getDays[1] + "-" + getDays[2], getDays[1], getDays[2], DaysInMonth, getDays[0]);
            //Response.Clear();
            //imgLoading.Visible = false;
        }

        protected void btnBDTNoteGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

                // getSelectedShiftId();                
                string[] getDays = txtGenerateMonth.Text.Trim().Split('-');
                BDTNoteGeneration(getDays[1], getDays[2]);


                DataTable dt = new DataTable();
                string getMonthName = getDays[1] + "-" + getDays[2];
                sqlDB.fillDataTable("select NoteName,Amount,DptName,MonthName,DptId,CompanyId,CompanyName,SftId,SftName from v_Payroll_MonthlyNoteAmount group by NoteName,Amount,DptName,MonthName,DptId,CompanyId,CompanyName,SftId,SftName having CompanyId='" + CompanyId + "'  AND MonthName='" + getMonthName + "' order by CompanyId,SftId,DptId ", dt);
                Session["__NoteAmount__"] = dt;

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=NoteGenerate-" + getMonthName + "');", true);  //Open New Tab for Sever side code

            }
            catch { }
        }
        private void BDTNoteGeneration(string month, string year)
        {
            try
            {
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();


                deleteBDTNotes(CompanyId);
                DataTable dtBDTNotes = new DataTable();
                sqlDB.fillDataTable("select SL,Note from HRD_BDTNote where Chosen='true'", dtBDTNotes);



                DataTable dtGetSalarySheet = new DataTable();
                sqlDB.fillDataTable("select CompanyId,SftId,DptId,DptName,Format(YearMonth,'MM-yyyy') as YearMonth,sum(TotalSalary) AS TotalSalary from "
                + "v_MonthlySalarySheet where Month(YearMonth) ='" + month + "' AND Year(YearMonth)='" + year + "' AND CompanyId='" + CompanyId + "'  group by CompanyId,SftId,DptId,DptName,YearMonth order  by "
                + "DptId,SftId ", dtGetSalarySheet);

                int[] noteAmount = new int[dtBDTNotes.Rows.Count];

                double getTime = Math.Round((double.Parse(dtGetSalarySheet.Rows.Count.ToString())) / 100, 0);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(getTime));

                for (int i = 0; i < dtGetSalarySheet.Rows.Count; i++)
                {
                    double getSalaryAmount = double.Parse(dtGetSalarySheet.Rows[i]["TotalSalary"].ToString());
                    double temp;
                    for (byte j = (byte)(dtBDTNotes.Rows.Count - 1); j >= 0; j--)
                    {
                        if (getSalaryAmount >= int.Parse(dtBDTNotes.Rows[j]["Note"].ToString()))
                        {
                            temp = getSalaryAmount % int.Parse(dtBDTNotes.Rows[j]["Note"].ToString());
                            getSalaryAmount -= temp;
                            noteAmount[j] += (int)getSalaryAmount / int.Parse(dtBDTNotes.Rows[j]["Note"].ToString());
                            getSalaryAmount = temp;
                            if ((int)getSalaryAmount == 0 || j == 0) break;
                        }

                        if (j == 0) break;
                    }


                    if (i == 1088)
                    {

                    }

                    //string a = dtGetSalarySheet.Rows[i]["DptId"].ToString();
                    //string ab = dtGetSalarySheet.Rows[i + 1]["DptId"].ToString();

                    //string aa = dtGetSalarySheet.Rows[i]["LnId"].ToString();

                    //string aaa = dtGetSalarySheet.Rows[i + 1]["LnId"].ToString();

                    //if (a == "24" && aa == "43")
                    //{ 

                    //}

                    saveGenerateNotes(noteAmount, dtBDTNotes, dtGetSalarySheet, i);
                    noteAmount = new int[dtBDTNotes.Rows.Count];
                    /*
                    if (i == dtGetSalarySheet.Rows.Count - 1)    // for last value
                    {
                        saveGenerateNotes(noteAmount, dtBDTNotes, dtGetSalarySheet, i);
                        noteAmount = new int[dtBDTNotes.Rows.Count];
                    }
                    else if (dtGetSalarySheet.Rows[i]["DptId"].ToString().Equals(dtGetSalarySheet.Rows[i + 1]["DptId"].ToString()))
                    {
                       
                        saveGenerateNotes(noteAmount, dtBDTNotes, dtGetSalarySheet, i);
                        noteAmount = new int[dtBDTNotes.Rows.Count];

                    }
                    else if (dtGetSalarySheet.Rows[i]["DptId"].ToString() != dtGetSalarySheet.Rows[i + 1]["DptId"].ToString())
                    {
                        saveGenerateNotes(noteAmount, dtBDTNotes, dtGetSalarySheet, i);
                        noteAmount = new int[dtBDTNotes.Rows.Count];
                    }
                     */
                }

            }
            catch { }
        }
        private void saveGenerateNotes(int[] noteAmount, DataTable dtBDTNotes, DataTable dtGetSalarySheet, int i)
        {
            try
            {
                for (byte b = (byte)(dtBDTNotes.Rows.Count - 1); b >= 0; b--)
                {
                    try
                    {
                        string[] getColumns = { "CompanyId", "SftId", "DptId", "NoteName", "Amount", "MonthName" };
                        string[] getValues = { dtGetSalarySheet.Rows[i]["CompanyId"].ToString(), dtGetSalarySheet.Rows[i]["SftId"].ToString(), dtGetSalarySheet.Rows[i]["DptId"].ToString(), dtBDTNotes.Rows[b]["Note"].ToString(), noteAmount[b].ToString(), dtGetSalarySheet.Rows[i]["YearMonth"].ToString() };
                        SQLOperation.forSaveValue("Payroll_MonthlyNoteAmount", getColumns, getValues, sqlDB.connection);
                        if (b == 0)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show(ex.Message);
                    }
                }
            }
            catch { }
        }
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
        private void deleteBDTNotes(string CompanyId)
        {
            try
            {
                string[] getMonthName = txtGenerateMonth.Text.Split('-');
                SQLOperation.forDeleteRecordByIdentifier("Payroll_MonthlyNoteAmount", "MonthName", getMonthName[1] + "-" + getMonthName[2], sqlDB.connection);
                SqlCommand cmd = new SqlCommand("delete from Payroll_MonthlyNoteAmount where MonthName ='" + getMonthName[1] + "-" + getMonthName[2] + "' AND CompanyId='" + CompanyId + "' ");
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
        DataTable dtOverTime_stayTime;
        DataTable dtTiffin_Staff_WorkerTaka;
        DataTable dtShortleave;
        DataTable dtOtherspay;
        DataTable dtOthersDeduction;

        private void generateMonthlySalarySheet(string getMonthYear, string month, string year, int DaysInMonth, string selectDays)
        {
            try
            {
                payroll_generation pg = new payroll_generation();

                double getTotalOT;
                string getJoingingDate;

                string MonthName = year + "-" + month;
                // for get company id
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                // get stamp card price
                sqlDB.fillDataTable("select StampDeduct from HRD_AllownceSetting where AllownceId =(select max(AllownceId) from HRD_AllownceSetting)", dtStampDeduct = new DataTable());
                sqlDB.fillDataTable("select WorkerTiffinTaka,StaffTiffinTaka from HRD_OthersSetting where CompanyId='" + CompanyId + "'", dtTiffin_Staff_WorkerTaka = new DataTable());








                //check overTime is active ?


                if (rbGenaratingType.SelectedValue.ToString().Equals("0"))   // generating type for all employee
                {
                    // for delete existing salary sheet of this month
                    salarySheetClearByMonthYear(month, year, CompanyId);

                    // get all regular employee at this time
                    sqlDB.fillDataTable("select  EmpCardNo,EmpName, SN,EmpType,EmpTypeId,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId,OverTime  from v_MonthlySalarySheet where CompanyId='" + CompanyId + "' and YearMonth='" + year + "-" + month + "-01' and IsActive='1' order by SN", dtRunningEmp = new DataTable());
                }

                else    // generating type for single employee
                {
                    if (txtEmpCardNo.Text.Trim().Length >= 4)  // valid card justification
                    {
                        salarySheetClearByMonthYear(month, year, CompanyId, txtEmpCardNo.Text);
                        // get max SN of employee,whose active salary status is true 
                        sqlDB.fillDataTable("select  EmpCardNo,EmpName, SN,EmpType,EmpTypeId,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId,OverTime  from v_MonthlySalarySheet where CompanyId='" + CompanyId + "' and YearMonth='" + year + "-" + month + "-01' and IsActive='1' and EmpCardNo like '%" + txtEmpCardNo.Text + "' order by SN", dtRunningEmp = new DataTable());

                    }
                    else
                    {
                        lblMessage.InnerText = "error->Please type valid card no of an employee";
                        txtEmpCardNo.Focus();
                    }
                }

                double getTime = Math.Round((double.Parse(dtRunningEmp.Rows.Count.ToString())) / 10, 0);
                bool isgarments = classes.Payroll.Office_IsGarments();
                //  System.Threading.Thread.Sleep(TimeSpan.FromSeconds(getTime));
                //imgLoading.Visible = true;

                for (int i = 0; i < dtRunningEmp.Rows.Count; i++)
                {

                    int getValue = 0;
                    if (rbGenaratingType.SelectedValue.ToString() != "1")
                    {
                        // for get operation progress--------------------------------

                        if (i != 0) getValue = (100 * i / (dtRunningEmp.Rows.Count - 1));
                        //probar.Style.Add("width",getValue.ToString()+"%");
                        //probar.InnerHtml = getValue.ToString()+"%";
                        //  ProgressBar1.Value = getValue;

                        // Response.Write(getValue.ToString() + "%");

                        //  System.Threading.Thread.Sleep(1000);
                    }
                    //------------------------------------------------------------


                    // get essential information of a certain employee 
                    sqlDB.fillDataTable("select EmpId,EmpCardNo,round(((case  when EmpPresentSalary<3505 then 3505 else EmpPresentSalary end)-400)/1.35,0) BasicSalary,300 as MedicalAllownce,FoodAllownce,ConvenceAllownce,round(((round(((case  when EmpPresentSalary<3505 then 3505 else EmpPresentSalary end)-400)/1.35,0))*35)/100,0) HouseRent,TechnicalAllownce,100 as OthersAllownce,case  when EmpPresentSalary<3505 then 3505 else EmpPresentSalary end EmpPresentSalary,AttendanceBonus,LunchCount,LunchAllownce,DptId,GrdName,DsgId,sftId,DormitoryRent,PFAmount,IncomeTax from v_Personnel_EmpCurrentStatus where SN=" + dtRunningEmp.Rows[i]["SN"].ToString() + "", dtCertainEmp = new DataTable());

                    // get Proximity number of a certain employee
                    sqlDB.fillDataTable("select convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate from Personnel_EmployeeInfo where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "'", dt = new DataTable());
                    ViewState["__getJoingingDate__"] = dt.Rows[0]["EmpJoiningDate"].ToString();

                    // get leave information of a certain employee
                    sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId,StateStatus from v_tblAttendanceRecord where ATTStatus='lv' AND MonthName ='" + year + '-' + month + "' AND EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' And AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", dtLeaveInfo = new DataTable());
                    getAllLeaveInformation();

                    // get present information of a certain employee
                    if (isgarments)
                    {
                        sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(11),ATTDate,111) as ATTDate,InHour,InMin,OutHour,OutMin,ATTStatus from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus In ('P','L') AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' AND PaybleDays='1' ", dtPresent = new DataTable());
                    }
                    else
                    {
                        sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(11),ATTDate,111) as ATTDate,InHour,InMin,OutHour,OutMin,ATTStatus from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus In ('P','L') AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' ", dtPresent = new DataTable());
                    }

                    // get late information of a certain employee
                    if (isgarments)
                    {
                        sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate, EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='L' AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' AND PaybleDays='1' ", dtLate = new DataTable());
                    }
                    else
                    {
                        sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate, EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='L' AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' ", dtLate = new DataTable());
                    }

                    // get absent information of a certain employee
                    if (isgarments)
                    {
                        sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='A' AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' Union select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus In ('P','L') AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' AND PaybleDays='0' ", dtAbsent = new DataTable());
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
                    //  checkForAttendanceBonus(month, year, dtCertainEmp.Rows[0]["EmpId"].ToString());


                    getHourlyAmount(DaysInMonth, double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString()));

                    // Call Over time Callculation for count OT taka

                    dtOverTime_stayTime = new DataTable();
                    // sqlDB.fillDataTable("select Sum(OverTime) as OverTime,Sum(Convert(int,Substring(Convert(varchar,StayTime),0,3)))  as StayTime from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'  and IsOverTime='1'", dtOverTime_stayTime);
                    sqlDB.fillDataTable("Select  isnull(CAST(SUM(DATEDIFF(second, 0, OverTime)) / 3600 AS varchar(12)) + ':' + RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OverTime)) / 60 % 60 AS varchar(2)), 2) + ':' +RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OverTime)) % 60 AS varchar(2)), 2),'00:00:00') AS OverTime,isnull(CAST(SUM(DATEDIFF(second, 0, OtherOverTime)) / 3600 AS varchar(12)) + ':' +RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OtherOverTime)) / 60 % 60 AS varchar(2)), 2) + ':' + RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OtherOverTime)) % 60 AS varchar(2)), 2),'00:00:00') AS OtherOverTime,isnull(CAST(SUM(DATEDIFF(second, 0, OverTime)) / 3600 AS varchar(12)) + ':' + RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OverTime)) / 60 % 60 AS varchar(2)), 2) + ':' +RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OverTime)) % 60 AS varchar(2)), 2),'00:00:00')+isnull(CAST(SUM(DATEDIFF(second, 0, OtherOverTime)) / 3600 AS varchar(12)) + ':' +RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OtherOverTime)) / 60 % 60 AS varchar(2)), 2) + ':' + RIGHT('0' + CAST(SUM(DATEDIFF(second, 0, OtherOverTime)) % 60 AS varchar(2)), 2),'00:00:00') as TotalOverTime from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'  and IsOverTime='1' and IsActive='1'", dtOverTime_stayTime);
                    // if (dtOverTime_stayTime.Rows[0]["TotalOverTime"].ToString().Trim() == "00:00:00")
                    // {

                    //     ViewState["__getTotalOvertimeAmt__"] = "0";
                    //     ViewState["__getTotalOverTime__"] = "0";
                    //     ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"] = "0";
                    //     ViewState["__Extra_OT_Amt_OfEmp__"] = "0";
                    // }
                    //else
                    // {

                    string time = dtOverTime_stayTime.Rows[0]["OverTime"].ToString();
                    string[] spltTime = time.Split(':');


                    string time2 = dtOverTime_stayTime.Rows[0]["OtherOverTime"].ToString();

                    string[] spltTime2 = time2.Split(':');


                    double hours = double.Parse(spltTime[0]) + double.Parse(spltTime2[0]);
                    double min = double.Parse(spltTime[1]) + double.Parse(spltTime2[1]);
                    double secods = double.Parse(spltTime[2]) + double.Parse(spltTime2[2]);

                    if (secods >= 60)
                    {
                        secods = secods - 60;
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
                            min = min - 60;
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
                            ViewState["__Holidays__"] = "0";
                            ViewState["__HolidayBillAmount__"] = "0";
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
                    sqlDB.fillDataTable("select Max(SL) as SL,Payroll_AdvanceInfo.AdvanceId,Payroll_AdvanceInfo.PaidInstallmentNo,InstallmentNo from Payroll_AdvanceInfo inner join Payroll_AdvanceSetting on Payroll_AdvanceInfo.AdvanceId=Payroll_AdvanceSetting.AdvanceId Where EmpCardNo='" + dtRunningEmp.Rows[i]["EmpCardNo"].ToString() + "' AND EmpTypeId=" + dtRunningEmp.Rows[i]["EmpTypeId"].ToString() + " AND Payroll_AdvanceSetting.PaidMonth='" + getMonthYear + "'  group By Payroll_AdvanceInfo.AdvanceId,Payroll_AdvanceInfo.PaidInstallmentNo,InstallmentNo ", dtAdvanceInfo = new DataTable());

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

                    saveMonthlyPayrollSheet(month, year, DaysInMonth, dtRunningEmp.Rows[i]["EmpName"].ToString(), i, selectDays, int.Parse(Session["__getUserId__"].ToString()), CompanyId, pg, txtGenerateMonth.Text);

                    //if (!isActiveLoop) break;
                }

                // lblMessage.InnerText = "success->Successfully payroll generated of "+dtRunningEmp.Rows.Count+" "+rbEmpTypeList.SelectedItem.ToString()+"";
                //  ProgressBar1.Value = 0;
                //  Response.Clear();
                //lblMessage.InnerText = "success->Successfully payroll generated of " + dtRunningEmp.Rows.Count;
                rbGenaratingType.SelectedValue = "0";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ProcessingEnd(" + dtRunningEmp.Rows.Count.ToString() + ");", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ProcessingEror(" + ex.Message + ");", true);
            }
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
        private void saveMonthlyPayrollSheet(string getMonth, string getYear, int DaysInMonth, string empName, int i, string selectedDay, int userId, string CompanyId, payroll_generation pg, string SDate)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("insert into Payroll_MonthlySalarySheet_fake(CompanyId,SftId,EmpId,EmpCardNo,YearMonth,DaysInMonth,Activeday,WeekendHoliday,PayableDays," +
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

                if (joiningMonthIsEqual(getMonth, getYear, CompanyId, SDate, pg, DaysInMonth) == false)
                {
                    //    if (dtRunningEmp.Rows[i]["EmpStatus"].ToString().Equals("1"))
                    //    {
                    //        if (ViewState["__ml__"].ToString().Equals("0"))
                    //        {
                    PayableDaysCalculation(getMonth + "-" + getYear, selectedDay, CompanyId, pg);
                    checkForAttendanceBonus(getMonth, getYear, dtCertainEmp.Rows[0]["EmpId"].ToString());
                    getNetPayableCalculation(DaysInMonth, pg);   // this function call to get net payable  amount
                    //        }
                    //        else
                    //        {
                    //            getPayableDaysCalculationForML(getMonth + "-" + getYear, selectedDay, pg);
                    //            getNetPayableCalculation(DaysInMonth, pg);   // this function call to get net payable  amount
                    //        }
                    //    }
                    //    else if (dtRunningEmp.Rows[i]["EmpStatus"].ToString().Equals("8"))
                    //    {
                    //        if (ViewState["__ml__"].ToString().Equals("0"))
                    //        {
                    //            PayableDaysCalculation(getMonth + "-" + getYear, selectedDay, CompanyId, pg);
                    //            getNetPayableCalculation(DaysInMonth, pg);   // this function call to get net payable  amount
                    //        }
                    //        else
                    //        {
                    //            getPayableDaysCalculationForML(getMonth + "-" + getYear, selectedDay, pg);
                    //            getNetPayableCalculation(DaysInMonth, pg);   // this function call to get net payable  amount
                    //        }

                    //}
                }


                cmd.Parameters.AddWithValue("@WeekendHoliday", int.Parse(ViewState["__WeekendCount__"].ToString()) - int.Parse(ViewState["__WeekendAsLeave__"].ToString()));
                cmd.Parameters.AddWithValue("@PayableDays", ViewState["__PayableDays__"].ToString());

                cmd.Parameters.AddWithValue("@CasualLeave", ViewState["__cl__"].ToString());
                cmd.Parameters.AddWithValue("@SickLeave", ViewState["__sl__"].ToString());
                cmd.Parameters.AddWithValue("@AnnualLeave", ViewState["__al__"].ToString());
                cmd.Parameters.AddWithValue("@OfficialLeave", ViewState["__ofl__"].ToString());
                cmd.Parameters.AddWithValue("@OthersLeave", ViewState["__othl__"].ToString());

                cmd.Parameters.AddWithValue("@FestivalHoliday", ViewState["__HolidayCount__"].ToString());
                cmd.Parameters.AddWithValue("@AbsentDay", ViewState["__TotalAbsentDays__"].ToString());
                cmd.Parameters.AddWithValue("@PresentDay", dtPresent.Rows.Count.ToString());

                cmd.Parameters.AddWithValue("@EmpPresentSalary", ViewState["__presentSalary__"].ToString());
                cmd.Parameters.AddWithValue("@BasicSalary", dtCertainEmp.Rows[0]["BasicSalary"].ToString());
                cmd.Parameters.AddWithValue("@HouseRent", dtCertainEmp.Rows[0]["HouseRent"].ToString());
                cmd.Parameters.AddWithValue("@MedicalAllownce", dtCertainEmp.Rows[0]["MedicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@ConvenceAllownce", dtCertainEmp.Rows[0]["ConvenceAllownce"].ToString());
                cmd.Parameters.AddWithValue("@FoodAllownce", dtCertainEmp.Rows[0]["FoodAllownce"].ToString());
                cmd.Parameters.AddWithValue("@TechnicalAllowance", dtCertainEmp.Rows[0]["TechnicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@OthersAllownce", dtCertainEmp.Rows[0]["OthersAllownce"].ToString());
                cmd.Parameters.AddWithValue("@LunchAllowance", 0);

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
                cmd.Parameters.AddWithValue("@EmpStatus", '1');

                cmd.Parameters.AddWithValue("@UserId", userId.ToString());
                cmd.Parameters.AddWithValue("@IsSeperationGeneration", "0");
                cmd.Parameters.AddWithValue("@GenerateDate", convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy"))).ToString();
                cmd.Parameters.AddWithValue("@OTHourForBuyer", 0);
                cmd.Parameters.AddWithValue("@OTAmountForBuyer", 0);
                cmd.Parameters.AddWithValue("@ExtraOTHour", 0);
                cmd.Parameters.AddWithValue("@ExtraOTAmount", 0);
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

                Advance_And_Loan_StatusChange(pg);  // For advance and loan status change


                //  lbProcessingStatus.Items.Add("Processing completed of "+dtRunningEmp.Rows[i]["EmpType"].ToString() +" "+empName+" Card No. "+dtRunningEmp.Rows[i]["EmpCardNo"].ToString()+""); 

            }
            catch (Exception ex)
            {
                //lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        double getTotalSalary;
        double getTotalSalaryWithAllOT;
        double getNetPayable;
        double getAdvanceInstallment;
        double getLoanInstallment;
        double getPayable;
        double getNetPayableWithAllOTAmt;
        private void getNetPayableCalculation(int DaysInMonth, payroll_generation pg)   // net payable calculation
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

                //int getAbsendDaysFromLate = (dtLate.Rows.Count >= 3) ? (int)dtLate.Rows.Count / 3 : 0;
                int getAbsendDaysFromLate = 0;
                //find late days
                ViewState["__LateDays__"] = getAbsendDaysFromLate;

                //ViewState["__PayableDays__"] = (int.Parse(ViewState["__PayableDays__"].ToString()) - getAbsendDaysFromLate);

                // getAbsendDaysFromLate += dtAbsent.Rows.Count;
                //find absent days
                ViewState["__TotalAbsentDays__"] = dtAbsent.Rows.Count;

                double lateDeduction = Math.Round(double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString()) / DaysInMonth * getAbsendDaysFromLate, 2);
                ViewState["__LateFine__"] = lateDeduction;
                // find Absent deduction
                double getAbsentAmount = Math.Round(((double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString()) / DaysInMonth) * (DaysInMonth-double.Parse(ViewState["__PayableDays__"].ToString()))), 2);

                //get one day salary 
                double onDaySalary = (1 * double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString())) / DaysInMonth;

                ViewState["__TotalDeduction__"] = getAbsentAmount.ToString();

                ViewState["__PFDeduction"] = dtCertainEmp.Rows[0]["PFAmount"].ToString();

                //getPresentSalary = onDaySalary * double.Parse(ViewState["__PayableDays__"].ToString());
                getPresentSalary = double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString()) - getAbsentAmount;

                getPayable = 0;
                getPayable = Math.Round(((getPresentSalary + double.Parse(dtOtherspay.Rows[0]["OtherPay"].ToString())) - (lateDeduction + double.Parse(dtCertainEmp.Rows[0]["IncomeTax"].ToString()) + double.Parse(dtCertainEmp.Rows[0]["PFAmount"].ToString()) + getAdvanceInstallment + getLoanInstallment + double.Parse(dtOthersDeduction.Rows[0]["PAmount"].ToString()) + double.Parse(dtCertainEmp.Rows[0]["DormitoryRent"].ToString()))), 0);

                // ViewState["__PayableDays__"];
                //-------------------------------------------------------------------
                // get total salary 


                //ViewState[""] = (getPresentSalary -  getPayable).ToString();  // this line for get total deduction 

                // getAbsentAmount = getDays - float.Parse( ViewState["__PayableDays__"].ToString());  // now absent dayes enetrd in getabsentAmount

                // ViewState["__absentFine__"] = (getAbsentAmount * onDaySalary) + double.Parse( ViewState["__absentFine__"].ToString());

                //---------------------------------------------------------------------------
                double totalovertimeamt = double.Parse(ViewState["__getTotalOvertimeAmt__"].ToString());
                getNetPayable = getPayable + totalovertimeamt + getAttendanceBonus + double.Parse(ViewState["__TiffinBillAmount__"].ToString()) + double.Parse(ViewState["__HolidayBillAmount__"].ToString());
                getNetPayable = Math.Round(getNetPayable, 0, MidpointRounding.AwayFromZero);


                // to get finaly payble amount
                getTotalSalary = Math.Round((getNetPayable - double.Parse(dtStampDeduct.Rows[0]["StampDeduct"].ToString())), 0);

            }
            catch (Exception ex)
            {
                // lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private void PayableDaysCalculation(string MonthName, string SelectedDay, string CompanyId, payroll_generation pg)    // For Runing employee
        {
            try
            {
                DataTable dt;
                string monthYear = MonthName.Substring(3, 4) + "-" + MonthName.Substring(0, 2);
                sqlDB.fillDataTable("select distinct format(WeekendDate,'yyyy-MM-dd') as WeekendDate from v_Attendance_WeekendInfo where CompanyId='" + CompanyId + "'  AND  MonthName='" + MonthName + "' And WeekendDate >='" + monthYear + "-01' AND WeekendDate <='" + monthYear + '-' + SelectedDay + "' AND WeekendDate not in (select AttDate from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv')", dt = new DataTable());
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
                        if (dtWC.Rows.Count == 0)
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
                float PayableDays = float.Parse(dt.Rows.Count.ToString()) + float.Parse(ViewState["__cl__"].ToString()) + float.Parse(ViewState["__sl__"].ToString()) + float.Parse(ViewState["__al__"].ToString()) + dtPresent.Rows.Count + float.Parse(dtHoliday.Rows.Count.ToString());
                ViewState["__PayableDays__"] = PayableDays.ToString();
            }
            catch { }

        }

        private void getPayableDaysCalculationForML(string MonthName, string SelectedDay, payroll_generation pg)
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
                //int PayableDays = int.Parse(  ViewState["__WeekendCount__"].ToString()) + int.Parse(  ViewState["__cl__"].ToString()) + int.Parse(  ViewState["__sl__"].ToString()) + int.Parse(  ViewState["__al__"].ToString()) + int.Parse(  ViewState["__ofl__"].ToString()) + int.Parse(  ViewState["__othl__"].ToString()) +   dtPresent.Rows.Count + int.Parse(  ViewState["__HolidayCount__"].ToString()) +   dtAbsent.Rows.Count;
                float PayableDays = float.Parse(ViewState["__WeekendCount__"].ToString()) + float.Parse(ViewState["__cl__"].ToString()) + float.Parse(ViewState["__sl__"].ToString()) + float.Parse(ViewState["__al__"].ToString()) + float.Parse(ViewState["__ofl__"].ToString()) + float.Parse(ViewState["__othl__"].ToString()) + float.Parse(ViewState["__ml__"].ToString()) + float.Parse(dtPresent.Rows.Count.ToString()) + float.Parse(ViewState["__HolidayCount__"].ToString()) + float.Parse(dtAbsent.Rows.Count.ToString());
                ViewState["__PayableDays__"] = PayableDays.ToString();
            }
            catch { }
        }

        private bool joiningMonthIsEqual(string getMonth, string getYear, string CompanyId, string selectdates, payroll_generation pg, int DaysinMonth)   //net payable calculation,compier joining time for generate salary sheet of month
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
                    sqlDB.fillDataTable("select convert(varchar(11),HDate,105) as HDate from tblHolydayWork where CompanyId='" + CompanyId + "' AND HDate>='" + getYearMonth + "-" + getJoiningMonth[0] + "' AND HDate <='" + selectDates[2] + "-" + selectDates[1] + "-" + selectDates[0] + "' AND HDate not in (select AttDate from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv') ", dtHoliday);
                    byte HDCount = 0;


                    if (getJoiningMonth[0] != "1") getAttendanceBonus = 0;


                    DataTable dtTemp = new DataTable();
                    dtTemp.Columns.Add("AttDate", typeof(string));
                    DataTable dtWC = new DataTable();
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
                            if (dtWC.Rows.Count == 0)
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

                    //for (byte b = 0; b < dtHoliday.Rows.Count; b++)
                    //{
                    //    string[] dates = dtHoliday.Rows[b]["HDate"].ToString().Split('-');

                    //    DateTime HDay = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]));
                    //    DateTime join = new DateTime(int.Parse(getJoiningMonth[2]), int.Parse(getJoiningMonth[1]), int.Parse(getJoiningMonth[0]));

                    //    if (HDay >= join) HDCount += 1;
                    //}


                    DataTable dtWDInfo;
                    string monthYear = getMonth + "-" + getYear;

                    sqlDB.fillDataTable("select distinct format(WeekendDate,'dd-MM-yyyy') as WeekendDate from v_Attendance_WeekendInfo where CompanyId='" + CompanyId + "' AND MonthName='" + monthYear + "' And WeekendDate>='" + getYearMonth + "-" + getJoiningMonth[0] + "' AND WeekendDate <='" + selectDates[2] + "-" + selectDates[1] + "-" + selectDates[0] + "' AND WeekendDate not in (select AttDate from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv')", dt = new DataTable());

                    dtTemp = new DataTable();
                    dtWC = new DataTable();


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
                    //byte WDCount = 0;
                    //for (byte b = 0; b < dtWDInfo.Rows.Count; b++)
                    //{
                    //    string[] dates = dtWDInfo.Rows[b]["WeekendDate"].ToString().Split('-');
                    //    // string [] joinDates=
                    //    DateTime WDay = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]));
                    //    DateTime Join = new DateTime(int.Parse(getJoiningMonth[2]), int.Parse(getJoiningMonth[1]), int.Parse(getJoiningMonth[0]));

                    //    if (WDay >= Join) WDCount += 1;

                    //}
                    ViewState["__PayableDays__"] = "0";
                    float PayableDays = float.Parse(ViewState["__WeekendCount__"].ToString()) + float.Parse(ViewState["__cl__"].ToString()) + float.Parse(ViewState["__sl__"].ToString()) + float.Parse(ViewState["__al__"].ToString()) + float.Parse(ViewState["__ofl__"].ToString()) + float.Parse(ViewState["__othl__"].ToString()) + float.Parse(dtPresent.Rows.Count.ToString()) + float.Parse(ViewState["__HolidayCount__"].ToString());
                    ViewState["__PayableDays__"] = PayableDays.ToString();
                    //--------------------------End--------------------------------------------------------------------                    
                    //ViewState["__HolidayCount__"] = HDCount.ToString();


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

                    getNetPayableCalculation(DaysinMonth, pg);

                    //   // to get absent amount 
                    //   double getAbsentAmount = Math.Round((dtAbsent.Rows.Count * double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString())) / getDays, 0);

                    //   //get one day salary 
                    //   double onDaySalary = Math.Round((1 * double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString())) / getDays, 0);

                    //   ViewState["__absentFine__"] = getAbsentAmount.ToString();

                    //   getPayable = 0;
                    //   //   getPayable = NewGross - getAbsentAmount;

                    //   //-------------------------------------------------------------
                    //   getPayable = Math.Round(double.Parse(dtOverTime_stayTime.Rows[0]["StayTime"].ToString()) * NormalHourlySalary, 0);
                    //   getPayable = Math.Round(getPayable - getAbsentAmount, 0);

                    //   ViewState["__TotalDeduction__"] = (double.Parse(  ViewState["__presentSalary__"].ToString()) -   getPayable).ToString();  // this line for get total deduction 


                    //   //-------------------------------------------------------------

                    ////   getAbsentAmount = getDays - byte.Parse(  ViewState["__PayableDays__"].ToString());   // now getAbsentAmount=Due Absent days
                    //  //   ViewState["__absentFine__"] = (getAbsentAmount * onDaySalary) + double.Parse(  ViewState["__absentFine__"].ToString());


                    //     getAdvanceInstallment = 0;
                    //     getLoanInstallment = 0;
                    //   try
                    //   {
                    //       if (  dtAdvanceInfo.Rows.Count > 0)
                    //           if (  dtCutAdvance.Rows.Count > 0)   getAdvanceInstallment = Math.Round(double.Parse(  dtCutAdvance.Rows[0]["InstallmentAmount"].ToString()), 0);

                    //   }
                    //   catch { }

                    //   try
                    //   {
                    //       if (  dtLoanInfo.Rows.Count > 0)
                    //           if (  dtCutLoan.Rows.Count > 0)   getLoanInstallment = Math.Round(double.Parse(  dtCutLoan.Rows[0]["InstallmentAmount"].ToString()), 0);
                    //   }
                    //   catch { }




                    //   // getNetPayable = Math.Round(((NewGross + TotalOTTaka + getAttendanceBonus + getLunchCost) - (getAbsentAmount + getAdvanceInstallment + getLoanInstallment)), 0);


                    //     getNetPayable = Math.Round(((  getPayable + float.Parse(  ViewState["__getTotalOvertimeAmt__"].ToString()) +   getAttendanceBonus) - (  getAdvanceInstallment +   getLoanInstallment)), 0);

                    //   // to get finaly  payble amount
                    //     getTotalSalary = Math.Round((  getNetPayable - double.Parse(  dtStampDeduct.Rows[0]["StampDeduct"].ToString())), 0);



                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void Advance_And_Loan_StatusChange(payroll_generation pg)
        {
            try
            {
                if (dtCutAdvance.Rows.Count != 0)   // for change Advance status
                {
                    if (dtAdvanceInfo.Rows[0]["InstallmentNo"].ToString().Equals(dtAdvanceInfo.Rows[0]["PaidInstallmentNo"].ToString()))
                    {
                        SqlCommand cmd = new System.Data.SqlClient.SqlCommand("update Payroll_AdvanceInfo set PaidStatus ='1' where AdvanceId='" + dtAdvanceInfo.Rows[0]["AdvanceId"].ToString() + "'", sqlDB.connection);
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
                        SqlCommand cmd = new System.Data.SqlClient.SqlCommand("update Payroll_LoanInfo set PaidStatus ='1' where LoanId='" + dtLoanInfo.Rows[0]["LoanId"].ToString() + "'", sqlDB.connection);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
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

        public DataTable dtGetMonthSetup;
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
                if (dtLate.Rows.Count >= 1 || dtApplydate.Rows.Count > 0 || dtAbsent.Rows.Count >= 1 || int.Parse(dtGetMonthSetup.Rows[0]["TotalDays"].ToString()) != int.Parse(ViewState["__PayableDays__"].ToString())) getAttendanceBonus = 0;
                else getAttendanceBonus = int.Parse(dtCertainEmp.Rows[0]["AttendanceBonus"].ToString());
                //}
                //else getAttendanceBonus = 0;
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private void salarySheetClearByMonthYear(string month, string year, string CompanyId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("delete from Payroll_MonthlySalarySheet_fake where CompanyId='" + CompanyId + "'  AND Year(YearMonth)='" + year + "' AND Month(YearMonth)='" + month + "' AND EmpStatus in ('1','8') AND IsSeperationGeneration='0'", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }
        private void salarySheetClearByMonthYear(string month, string year, string CompanyId, string empcardno)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("delete from Payroll_MonthlySalarySheet_fake where CompanyId='" + CompanyId + "'  AND Year(YearMonth)='" + year + "' AND Month(YearMonth)='" + month + "' AND EmpStatus in ('1','8') AND EmpCardNo LIKE '%" + empcardno + "' AND IsSeperationGeneration='0'", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

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

                    DataRow[] drWeekendAsLeave = dtLeaveInfo.Select("AttDate " + Session["__setPredicate__"].ToString());
                    ViewState["__WeekendAsLeave__"] = drWeekendAsLeave.Length;




                }

            }
            catch (Exception ex)
            {
                //   lblMessage.InnerText ="error->"+ex.Message;
            }
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.Employee.LoadEmpCardNoWithNameByCompanyRShift(ddlEmpCardNo, ddlCompanyList.SelectedValue);
            if (rbGenaratingType.SelectedValue == "0")
            {
                ddlEmpCardNo.Enabled = false;
                txtEmpCardNo.Enabled = false;
            }
            else
            {
                ddlEmpCardNo.Enabled = true;
                txtEmpCardNo.Enabled = true;
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }


    }


}