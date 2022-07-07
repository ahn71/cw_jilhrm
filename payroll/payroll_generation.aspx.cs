using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using ComplexScriptingSystem;
using System.Drawing;
using ComplexScriptingWebControl;
using System.Web.SessionState;
using System.Threading;
using System.Web.Services;
using SigmaERP.classes;

namespace SigmaERP.payroll
{
    public partial class payroll_generation : System.Web.UI.Page
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
                classes.Employee.LoadEmpCardNoWithNameByCompanyRShift(ddlEmpCardNo,ddlCompanyList.SelectedValue);
                if (classes.Payroll.Office_IsGarments()) IsGarments = true;
                else IsGarments = false;
            }
            if (!classes.commonTask.HasBranch())
                ddlCompanyList.Enabled = false;
            ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();

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
                Session["__CShortName__"] = getCookies["__CShortName__"].ToString();
                ViewState["__CShortName__"] = getCookies["__CShortName__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForOnlyWriteAction(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "payroll_generation.aspx", ddlCompanyList, btnGenerate, btnBDTNoteGenerate);


                classes.commonTask.LoadShift(ddlShiftList, ViewState["__CompanyId__"].ToString());
                addAllTextInShift();
            }
            catch { }
        }
        
        byte totalDays=0;
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
           
            string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

           // getSelectedShiftId();

            string[] getDays = txtGenerateMonth.Text.Trim().Split('-');
            int days = DateTime.DaysInMonth(int.Parse(getDays[2]),int.Parse(getDays[1]));

           
          
            

            loadWeekendInfo(getDays[1] + "-" + getDays[2],getDays[0]);
            loadMonthSetup("1",getDays[1],getDays[2],CompanyId);

            Session["OPERATION_PROGRESS"] = 10;
            Thread.Sleep(1000);

            if (rbGenaratingType.SelectedValue.ToString() == "0") salarySheetClearByMonthYear(getDays[1], getDays[2], CompanyId);
            generateMonthlySalarySheet(getDays[1] + "-" + getDays[2], getDays[1], getDays[2], days, getDays[0]);
            
        }

        private static void GnerateButtonClick(string SDate,string CompanyId,string GType)
        {      
            
           

            // getSelectedShiftId();

            string[] getDays = SDate.Trim().Split('-');
            int days = DateTime.DaysInMonth(int.Parse(getDays[2]), int.Parse(getDays[1]));



            payroll_generation pg = new payroll_generation();

            loadWeekendInfo(getDays[1] + "-" + getDays[2], getDays[0]);
            loadMonthSetup("1", getDays[1], getDays[2], CompanyId);


            if (GType == "0") salarySheetClearByMonthYear(getDays[1], getDays[2], CompanyId);
            HttpContext.Current.Session["getMonthYear"]=getDays[1] + "-" + getDays[2];
            HttpContext.Current.Session["month"] = getDays[1];
            HttpContext.Current.Session["year"] = getDays[2];
            HttpContext.Current.Session["Days"] = days;
            HttpContext.Current.Session["SelectedDays"] = getDays[0];

            
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
                    getRequiredSftId +=ddlShiftList.Items[r].Value.ToString() +",";

                }
                ViewState["__getRequiredSftId__"] = getRequiredSftId.Remove(getRequiredSftId.LastIndexOf(','));
            }
            catch { }
        }
        DataTable dtShiftListForCheckOverTime;
        private void checkOverTimeIsActiveForSelectedShift(string CompanyId)
        {
            try
            {
                dtShiftListForCheckOverTime = new DataTable();
                if (ddlShiftList.SelectedItem.ToString().Equals("All"))
                {
                    sqlDB.fillDataTable("select SftId,SftOverTime from HRD_Shift where CompanyId='" + CompanyId + "' AND SftId in (" + ViewState["__getRequiredSftId__"].ToString() + ")", dtShiftListForCheckOverTime);
                }
                else sqlDB.fillDataTable("select SftId,SftOverTime from HRD_Shift where CompanyId='" + CompanyId + "' AND SftId ="+ddlShiftList.SelectedValue.ToString()+"", dtShiftListForCheckOverTime);
            }
            catch { }
        }

       
        public static  void loadWeekendInfo(string MonthYear,string selectDays)
        {
            try
            {
                DataTable dt=new DataTable ();
                string a = MonthYear.Substring(3, 4);
                string aa = MonthYear.Substring(0,2);
                sqlDB.fillDataTable("select distinct convert(varchar(11),WeekendDate,111) as WeekendDate from v_Attendance_WeekendInfo where MonthName='" + MonthYear + "' AND WeekendDate >='" + MonthYear.Substring(3, 4) + '-' + MonthYear.Substring(0, 2) + "-01" + "' AND WeekendDate<='"+MonthYear.Substring(3, 4) + '-' + MonthYear.Substring(0, 2)+'-' +selectDays+"'", dt);

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

               HttpSessionState session = HttpContext.Current.Session;
               session["__WeekendCount__"] = dt.Rows.Count.ToString();
               session["__setPredicate__"] = setPredicate;
               
            }
            catch { }
         
        }

        private bool alreadyCreatedPayrollSheet(string Month,string Year)
        {
            try
            {
                sqlDB.fillDataTable("select SL from Payroll_MonthlySalarySheet where Month='" + Month + "' AND Year ='" + Year + "' AND EmpTypeId ="+1+"", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    string monthName = new DateTime(int.Parse(Year), int.Parse(Month), 1).ToString("MMM", CultureInfo.InvariantCulture);
                    lblMessage.InnerText = "error->Already salary sheet created of "+monthName+"-"+Year+",Now you can create partial salary sheet .";
                    return false;
                }
                else return true;
                
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private bool certainEmployeeSalarySheetIsCreated(string Month,string Year,string CardNo)
        {
            try
            {
                sqlDB.fillDataTable("select SL from Payroll_MonthlySalarySheet where Month='" + Month + "' AND Year ='" + Year + "' AND EmpCardNo ='" + CardNo + "' AND EmpTypeId =" + 1 + "", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    string monthName = new DateTime(int.Parse(Year), int.Parse(Month), 1).ToString("MMM", CultureInfo.InvariantCulture);
                    lblMessage.InnerText = "error-> Already created of "+monthName+"-"+Year;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static DataTable dtGetMonthSetup;
        private static void loadMonthSetup(string days,string month,string year,string CompanyId)
        {
            try
            {
                string monthName = new DateTime(int.Parse(year),int.Parse(month),int.Parse(days)).ToString("MMM", CultureInfo.InvariantCulture);
                monthName += year.ToString().Substring(2, 2);
                string monthName2 = month + "-"+year;
                SQLOperation.selectBySetCommandInDatatable("select TotalDays,TotalWeekend ,FromDate,ToDate,TotalHoliday,TotalWorkingDays from tblMonthSetup where CompanyId='" + CompanyId+ "' AND ( MonthName='" + monthName + "' OR MonthName='" + monthName2 + "') ", dtGetMonthSetup = new DataTable(), sqlDB.connection);
                
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
       

        private void generateMonthlySalarySheet(string getMonthYear,string month,string year,int Days,string selectDays)
        {
            try
            {
                payroll_generation pg = new payroll_generation();
               
                double getTotalOT;
                string getJoingingDate;
               
                string MonthName=year+"-"+month;
                // get stamp card price
                sqlDB.fillDataTable("select StampDeduct from HRD_AllownceSetting where AllownceId =(select max(AllownceId) from HRD_AllownceSetting)", dtStampDeduct = new DataTable());



                // for get company id
                string CompanyId=(ddlCompanyList.SelectedValue.ToString().Equals("0000"))?ViewState["__CompanyId__"].ToString():ddlCompanyList.SelectedValue.ToString();

              

                //check overTime is active ?
                checkOverTimeIsActiveForSelectedShift(CompanyId); 

                if (rbGenaratingType.SelectedValue.ToString().Equals("0"))   // generating type for all employee
                {
                    // for delete existing salary sheet of this month
                    salarySheetClearByMonthYear(month, year, CompanyId);

                    // get all regular employee at this time
                    sqlDB.fillDataTable("select distinct EmpCardNo,EmpName, max(SN) as SN,EmpType,EmpTypeId,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId  from v_Personnel_EmpCurrentStatus group by EmpCardNo,EmpName,EmpTypeId,EmpType,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId having EmpStatus in('1','8') AND ActiveSalary='true' AND IsActive='1' AND CompanyId='" + CompanyId + "' order by SN", dtRunningEmp = new DataTable());
                }

                else    // generating type for single employee
                {
                    if (txtEmpCardNo.Text.Trim().Length>=4)  // valid card justification
                    {
                        //salarySheetClearForCertainEmployeeByMonthYear(month, year, CompanyId,ECar);

                        // get max SN of employee,whose active salary status is true 
                         sqlDB.fillDataTable("select EmpCardNo,EmpName,MAX(SN) as SN,EmpType,EmpTypeId,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId  from v_Personnel_EmpCurrentStatus group by EmpCardNo,EmpName,EmpType,EmpTypeId,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId having EmpCardNo Like'%" + txtEmpCardNo.Text + "' AND  EmpStatus in ('1','8') AND ActiveSalary='true' AND IsActive='1' AND CompanyId='" + CompanyId + "' ", dtRunningEmp = new DataTable());
                       
                    }
                    else
                    {
                        lblMessage.InnerText = "error->Please type valid card no of an employee";
                        txtEmpCardNo.Focus();
                    }
                }

                double getTime = Math.Round((double.Parse(dtRunningEmp.Rows.Count.ToString()))/10, 0);
              //  System.Threading.Thread.Sleep(TimeSpan.FromSeconds(getTime));

                for (int i = 0; i < dtRunningEmp.Rows.Count; i++)
                {
                   
                    int getValue = 0;
                    if (rbGenaratingType.SelectedValue.ToString() != "1")
                    {
                        // for get operation progress--------------------------------

                       // if (i != 0) getValue = (100 * i / (dtRunningEmp.Rows.Count-1));
                        //probar.Style.Add("width",getValue.ToString()+"%");
                        //probar.InnerHtml = getValue.ToString()+"%";
                        ProgressBar1.Value = getValue;
                        Response.Clear();
                        Response.Write(getValue.ToString()+"%");
                       // Response.End();
                      //  Response.Flush();
                        System.Threading.Thread.Sleep(1000);
                    }
                   //------------------------------------------------------------

                   
                    // get essential information of a certain employee 
                    sqlDB.fillDataTable("select EmpId,EmpCardNo,BasicSalary,MedicalAllownce,FoodAllownce,ConvenceAllownce,HouseRent,TechnicalAllownce,OthersAllownce,EmpPresentSalary,AttendanceBonus,LunchCount,LunchAllownce,DptId,GrdName,DsgId,sftId from v_Personnel_EmpCurrentStatus where SN=" + dtRunningEmp.Rows[i]["SN"].ToString() + "", dtCertainEmp = new DataTable());

                    // get Proximity number of a certain employee
                    sqlDB.fillDataTable("select convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate from Personnel_EmployeeInfo where EmpId='" +dtCertainEmp.Rows[0]["EmpId"].ToString()+ "'", dt = new DataTable());
                    ViewState["__getJoingingDate__"] = dt.Rows[0]["EmpJoiningDate"].ToString();
                        
                    // get leave information of a certain employee
                    sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId,StateStatus from v_tblAttendanceRecord where ATTStatus='lv' AND MonthName ='" + year + '-' + month + "' AND EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' And AttDate >='"+year+'-'+month+'-'+"01"+"' AND AttDate <= '"+year+'-'+month+'-'+selectDays+"'", dtLeaveInfo = new DataTable());
                    getAllLeaveInformation();

                    // get present information of a certain employee
                    sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(11),ATTDate,111) as ATTDate,InHour,InMin,OutHour,OutMin,ATTStatus from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus In ('P','L') AND MonthName='" + MonthName + "' AND AttDate Not  " + Session["__setPredicate__"].ToString() + " AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' ", dtPresent = new DataTable());

                    // get late information of a certain employee
                    sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate, EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='L' AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", dtLate = new DataTable());

                    // get absent information of a certain employee
                    sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='A' AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", dtAbsent = new DataTable());
                    
                    // check attendance bonus of a certain employee
                    checkForAttendanceBonus();
                        
                    // Call Over time Callculation for count OT taka

                    // for checking overtime is active this shift 

                    DataRow[] dr = dtShiftListForCheckOverTime.Select("SftId=" + dtCertainEmp.Rows[0]["SftId"].ToString() + " AND SftOverTime='true'");
                    
                    //if (dr.Length>0) OverTimeCalculation(dtCertainEmp.Rows[0]["EmpId"].ToString(),month,year,selectDays,double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString()));
                    //else
                    //{
                    //    ViewState["__getTotalOvertimeAmt__"] = "0";
                    //    ViewState["__getTotalOverTime__"] = "0";
                    //    getOTRate = 0; 
                    //}
                    
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

                    //saveMonthlyPayrollSheet(month, year, Days, dtRunningEmp.Rows[i]["EmpName"].ToString(), i, selectDays, int.Parse(Session["__getUserId__"].ToString()), CompanyId, pg,txtGenerateMonth.Text);

                    //if (!isActiveLoop) break;
                }

               // lblMessage.InnerText = "success->Successfully payroll generated of "+dtRunningEmp.Rows.Count+" "+rbEmpTypeList.SelectedItem.ToString()+"";
                ProgressBar1.Value = 0;
                Response.Clear();
               
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
            }
        }
        private static void salarySheetClearByMonthYear(string month,string year,string CompanyId)
        {
            try
            {
               SqlCommand  cmd = new SqlCommand("delete from Payroll_MonthlySalarySheet where CompanyId='" + CompanyId + "'  AND Year(YearMonth)='" + year + "' AND Month(YearMonth)='" + month + "' AND EmpStatus in ('1','8') AND IsSeperationGeneration='0'", sqlDB.connection);
               cmd.ExecuteNonQuery();
            }
            catch { }
        }

        private static void salarySheetClearForCertainEmployeeByMonthYear(string month, string year, string CompanyId,string EmpCardNo)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("delete from Payroll_MonthlySalarySheet where CompanyId='" + CompanyId + "' AND Year(YearMonth)='" + year + "' AND Month(YearMonth)='" + month + "' AND EmpCardNo like '%" + EmpCardNo + "' AND IsSeperationGeneration='0'", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        private void IsMaternityLeave()
        {
            try
            {
                //if (dtPresent.Rows.Count>0)
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

                    DataRow[] drWeekendAsLeave = dtLeaveInfo.Select("AttDate " + Session["__setPredicate__"].ToString());
                    ViewState["__WeekendAsLeave__"] = drWeekendAsLeave.Length;

                    
                        
                   
                }
                
            }
            catch (Exception ex)
            {
             //   lblMessage.InnerText ="error->"+ex.Message;
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
                lblMessage.InnerText = "error->"+ex.Message;
            }
        }

        double getOTRate;
       
       
        double TotalExtraOTTaka;
        int OT_Hour_ForBuyer_AsRegular;
        int Extra_OT_Hour_OfEmp;
        int GetTotalOT_Hour;
        int check_OverTime;
       static bool IsGarments=false ;
       static byte TotalWorkingDays;
        private void OverTimeCalculation(string getEmpId, string getMonth, string getYear, string SelectedDays, double getBasic,payroll_generation pg)   // over time calculation 
        {
            try
            {
                pg.TotalExtraOTTaka = 0;
                pg.OT_Hour_ForBuyer_AsRegular = 0;
                pg.GetTotalOT_Hour = 0;
                pg.Extra_OT_Hour_OfEmp = 0;
                
                string FromDate=getYear+"-"+getMonth+"-01";
                string ToDate=getYear+"-"+getMonth+"-"+SelectedDays;

                DataTable dtMonthlyOT=new DataTable ();

                pg.getOTRate = Math.Round((getBasic / 208) * 2, 2); //Here Average Day of month 30 and weekend 4 so total days 26 and daily working ours 8 so that 26*8=208 ((basic/208)*2)
                //------For Count Weekend Task of Employee-------------------
                sqlDB.fillDataTable("select sum (OverTime) as OverTime,OverTimeCheck from v_tblAttendanceRecord where IsActive='1' AND EmpId='" + getEmpId + "' AND ATTDate>='" + getYear + "-" + getMonth + "-01' AND ATTDate<='" + getYear + "-" + getMonth + "-" + SelectedDays + "' Group by OverTimeCheck", dtMonthlyOT);
                if(dtMonthlyOT.Rows.Count==0)
                {
                    pg.ViewState["__getTotalOvertimeAmt__"] = "0";
                    pg.ViewState["__getTotalOverTime__"] = "0";
                    pg.ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"] = "0";
                    pg.ViewState["__Extra_OT_Amt_OfEmp__"] = "0";
                }
                else if (dtMonthlyOT.Rows[0]["OverTimeCheck"].ToString() == "" || dtMonthlyOT.Rows[0]["OverTimeCheck"].ToString() == "False")
                {
                    pg.ViewState["__getTotalOvertimeAmt__"] = "0";
                    pg.ViewState["__getTotalOverTime__"] = "0";
                    pg.ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"] = "0";
                    pg.ViewState["__Extra_OT_Amt_OfEmp__"] = "0";
                }
                else
                {
                    if (IsGarments)
                    {
                        if (dtMonthlyOT.Rows[0]["OverTime"].ToString() == "") pg.GetTotalOT_Hour = 0;
                        else pg.GetTotalOT_Hour = int.Parse(dtMonthlyOT.Rows[0]["OverTime"].ToString());
                        pg.check_OverTime = (int.Parse(dtGetMonthSetup.Rows[0]["TotalWorkingDays"].ToString()) * 2);  // Calculate overtime for checking desired monthly overtime for buyr
                        if (GetTotalOT_Hour > check_OverTime)
                        {
                            pg.Extra_OT_Hour_OfEmp = GetTotalOT_Hour - check_OverTime;
                            pg.OT_Hour_ForBuyer_AsRegular = GetTotalOT_Hour - Extra_OT_Hour_OfEmp;
                        }
                        else
                        {
                            pg.Extra_OT_Hour_OfEmp = GetTotalOT_Hour;
                            pg.OT_Hour_ForBuyer_AsRegular = 0;
                        }
                    }
                    
                    if (dtMonthlyOT.Rows.Count > 0)
                    {
                        if (!IsGarments)
                        {
                            pg.ViewState["__getTotalOvertimeAmt__"] = Math.Round(getOTRate * double.Parse(dtMonthlyOT.Rows[0]["OverTime"].ToString()), 0);
                            pg.ViewState["__getTotalOverTime__"] = dtMonthlyOT.Rows[0]["OverTime"].ToString();
                            pg.ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"] = "0";
                            pg.ViewState["__Extra_OT_Amt_OfEmp__"] = "0";
                        }
                        else
                        {
                            pg.ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"] = Math.Round(getOTRate * double.Parse(OT_Hour_ForBuyer_AsRegular.ToString()), 0);
                            pg.ViewState["__Extra_OT_Amt_OfEmp__"] = Math.Round(getOTRate * double.Parse(Extra_OT_Hour_OfEmp.ToString()), 0);
                            pg.ViewState["__getTotalOvertimeAmt__"] = Math.Round(getOTRate * double.Parse(GetTotalOT_Hour.ToString()), 0);


                            pg.ViewState["__getTotalOverTime__"] = GetTotalOT_Hour;
                        }
                    }
                    else
                    {                        
                        pg.ViewState["__getTotalOvertimeAmt__"] = "0";
                        pg.ViewState["__getTotalOverTime__"] = "0";
                    }
                }

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
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

            if (bool.Parse(dtCertainEmp.Rows[0]["LunchCount"].ToString()).Equals(true))
            {

              //  getLunchCost = Math.Round(double.Parse(dtCertainEmp.Rows[0]["LunchAllownce"].ToString()) * double.Parse(dtGetMonthSetup.Rows[0]["TotalWorkingDays"].ToString()), 2);
                getLunchCost = Math.Round(double.Parse(dtCertainEmp.Rows[0]["LunchAllownce"].ToString()) * double.Parse(dtPresent.Rows.Count.ToString()), 2);
            }
            else getLunchCost = 0;
        }
        double getTotalSalary;
        double getTotalSalaryWithAllOT;
        double getNetPayable;
        double getAdvanceInstallment;
        double getLoanInstallment;
        double getPayable;
        double getNetPayableWithAllOTAmt;
        private static void getNetPayableCalculation(int getDays,payroll_generation pg)   // net payable calculation
        {
            try
            {

                pg.getNetPayable = 0;
                pg.getAdvanceInstallment = 0;
                pg.getLoanInstallment = 0;
                pg.getNetPayableWithAllOTAmt = 0;
                double getPresentSalary = double.Parse(pg.dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString());
                pg.ViewState["__presentSalary__"] = getPresentSalary.ToString();
                try
                {
                    if (pg.dtAdvanceInfo.Rows.Count > 0)
                        if (pg.dtCutAdvance.Rows.Count > 0) pg.getAdvanceInstallment = Math.Round(double.Parse(pg.dtCutAdvance.Rows[0]["InstallmentAmount"].ToString()), 0);

                }
                catch { }

                try
                {
                    if (pg.dtLoanInfo.Rows.Count > 0)
                        if (pg.dtCutLoan.Rows.Count > 0) pg.getLoanInstallment = Math.Round(double.Parse(pg.dtCutLoan.Rows[0]["InstallmentAmount"].ToString()), 0);
                }
                catch { }

                // find Absent deduction
                double getAbsentAmount = Math.Round((pg.dtAbsent.Rows.Count * double.Parse(pg.dtCertainEmp.Rows[0]["BasicSalary"].ToString())) / getDays, 0);

                //get one day salary 
                double onDaySalary = Math.Round((1 * double.Parse(pg.dtCertainEmp.Rows[0]["BasicSalary"].ToString())) / getDays, 0);

                pg.ViewState["__absentFine__"] = getAbsentAmount.ToString();

                pg.getPayable = 0;
              //  getPayable = Math.Round((getPresentSalary - getAbsentAmount), 0);
               // ViewState["__PayableDays__"];
                //-------------------------------------------------------------------
                pg.getPayable = getPresentSalary * float.Parse(pg.ViewState["__PayableDays__"].ToString()) / getDays;
                pg.getPayable = Math.Round((pg.getPayable - getAbsentAmount), 0);

                pg.ViewState["__TotalDeduction__"] = (getPresentSalary - pg.getPayable).ToString();  // this line for get total deduction 

                getAbsentAmount = getDays - float.Parse(pg.ViewState["__PayableDays__"].ToString());  // now absent dayes enetrd in getabsentAmount

                pg.ViewState["__absentFine__"] = (getAbsentAmount * onDaySalary) + double.Parse(pg.ViewState["__absentFine__"].ToString());

                //---------------------------------------------------------------------------
              //   getNetPayable = Math.Round(((getPresentSalary + TotalOTTaka + getAttendanceBonus + getLunchCost) - (getAbsentAmount + getAdvanceInstallment + getLoanInstallment)), 0);

                if (IsGarments)
                {
                    string a = pg.ViewState["__getTotalOvertimeAmt__"].ToString();
                    string aa = pg.ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"].ToString();
                    pg.getNetPayable = Math.Round(((pg.getPayable + float.Parse(pg.ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"].ToString()) + pg.getAttendanceBonus + pg.getLunchCost) - (pg.getAdvanceInstallment + pg.getLoanInstallment)), 0);
                    pg.getNetPayableWithAllOTAmt = Math.Round(((pg.getPayable + float.Parse(pg.ViewState["__getTotalOvertimeAmt__"].ToString()) + pg.getAttendanceBonus + pg.getLunchCost) - (pg.getAdvanceInstallment + pg.getLoanInstallment)), 0);
                }
                // to get finaly payble amount
                pg.getTotalSalary = Math.Round((pg.getNetPayable - double.Parse(pg.dtStampDeduct.Rows[0]["StampDeduct"].ToString())), 0);
                            
            }
            catch (Exception ex)
            {
               // lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private static bool joiningMonthIsEqual(string getMonth, string getYear,string CompanyId,string selectdates,payroll_generation pg)   //net payable calculation,compier joining time for generate salary sheet of month
        {
            try
            {
                string getShortname = pg.ViewState["__CShortName__"].ToString();
                string[] getJoiningMonth = pg.ViewState["__getJoingingDate__"].ToString().Split('-');

                string getJoinMonth = getJoiningMonth[1] + "-" + getJoiningMonth[2];

                string getCurrentMonth = getMonth + "-" + getYear;

                string[] selectDates = selectdates.Trim().Split('-');


                // below option for checking joining date-month-year is equal of current month 
                if (getJoinMonth.Equals(getCurrentMonth) && int.Parse(getJoiningMonth[0].ToString())!=1)
                {
                   
                    //-------------Count Payable Days------------------------------------------------------------------

                    string getYearMonth = getYear + "-" + getMonth;

                    DataTable dtHoliday = new DataTable();
                    sqlDB.fillDataTable("select convert(varchar(11),HDate,105) as HDate from tblHolydayWork where CompanyId='" + CompanyId + "' AND HDate>='" + getYearMonth + "-" + getJoiningMonth[0] + "' AND HDate <='" + selectDates[2] + "-" + selectDates[1] + "-" + selectDates[0] + "' AND HDate not in (select AttDate from tblAttendanceRecord where EmpId='" + pg.dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv') ", dtHoliday);
                    byte HDCount = 0;


                    if (getJoiningMonth[0] != "1") pg.getAttendanceBonus = 0;

                    for (byte b=0;b<dtHoliday.Rows.Count;b++)
                    {
                        string [] dates = dtHoliday.Rows[b]["HDate"].ToString().Split('-');
                       
                        DateTime HDay = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]));
                        DateTime join = new DateTime(int.Parse(getJoiningMonth[2]), int.Parse(getJoiningMonth[1]), int.Parse(getJoiningMonth[0]));

                        if (HDay>=join) HDCount += 1;
                    }


                    DataTable dtWDInfo;
                    string monthYear=getMonth+"-"+getYear;
                    sqlDB.fillDataTable("select distinct format(WeekendDate,'dd-MM-yyyy') as WeekendDate from v_Attendance_WeekendInfo where CompanyId='" + CompanyId + "' AND MonthName='" + monthYear + "' And WeekendDate>='" + getYearMonth + "-" + getJoiningMonth[0] + "' AND WeekendDate <='" + selectDates[2] + "-" + selectDates[1] + "-" + selectDates[0] + "' AND WeekendDate not in (select AttDate from tblAttendanceRecord where EmpId='" + pg.dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv')", dtWDInfo = new DataTable());

                    byte WDCount = 0;
                    for (byte b = 0; b < dtWDInfo.Rows.Count; b++)
                    {
                        string[] dates = dtWDInfo.Rows[b]["WeekendDate"].ToString().Split('-');
                        // string [] joinDates=
                        DateTime WDay = new DateTime(int.Parse(dates[2]), int.Parse(dates[1]), int.Parse(dates[0]));
                        DateTime Join = new DateTime(int.Parse(getJoiningMonth[2]), int.Parse(getJoiningMonth[1]), int.Parse(getJoiningMonth[0]));

                        if (WDay>=Join) WDCount += 1;

                    }
                    pg.ViewState["__PayableDays__"] = "0";
                    if(getShortname=="MSL")
                    {
                        float PayableDays = float.Parse(WDCount.ToString()) + float.Parse(pg.ViewState["__cl__"].ToString()) + float.Parse(pg.ViewState["__sl__"].ToString()) + float.Parse(pg.ViewState["__al__"].ToString()) + float.Parse(pg.ViewState["__ofl__"].ToString()) + float.Parse(pg.ViewState["__othl__"].ToString()) + float.Parse(pg.dtPresent.Compute("Sum(PaybleDay)", "").ToString()) + float.Parse(HDCount.ToString()) + float.Parse(pg.dtAbsent.Rows.Count.ToString());
                        pg.ViewState["__PayableDays__"] = PayableDays.ToString();
                    }
                    else
                    {
                        int PayableDays = WDCount + int.Parse(pg.ViewState["__cl__"].ToString()) + int.Parse(pg.ViewState["__sl__"].ToString()) + int.Parse(pg.ViewState["__al__"].ToString()) + int.Parse(pg.ViewState["__ofl__"].ToString()) + int.Parse(pg.ViewState["__othl__"].ToString()) + pg.dtPresent.Rows.Count + HDCount + pg.dtAbsent.Rows.Count;
                        pg.ViewState["__PayableDays__"] = PayableDays.ToString();
                    }
                    
                    //--------------------------End--------------------------------------------------------------------

                    pg.ViewState["__WeekendCount__"] = WDCount.ToString();
                    pg.ViewState["__HolidayCount__"] = HDCount.ToString();


                    int getDays = DateTime.DaysInMonth(int.Parse(getYear), int.Parse(getMonth));
                    int LateDays = getDays - int.Parse(getJoiningMonth[0]);  // this line find out active days

                    LateDays = getDays - LateDays-1;  // this line find out late days

                    /*--------------------------For something rong------------------------------------------------------------------------------
                    double NewGross = Math.Round((double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString()) / getDays) * LateDays, 0); 

                    NewGross = Math.Round((double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString())) - NewGross, 0);
                    ViewState["__presentSalary__"] = NewGross;
                    
                   ------------------------------------------------------------------------------------------------------------------------
                    */
                    pg.ViewState["__presentSalary__"] = double.Parse(pg.dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString()) + double.Parse(pg.dtCertainEmp.Rows[0]["TechnicalAllownce"].ToString());



                    // to get absent amount 
                    double getAbsentAmount = Math.Round((pg.dtAbsent.Rows.Count * double.Parse(pg.dtCertainEmp.Rows[0]["BasicSalary"].ToString())) / getDays, 0);

                    //get one day salary 
                    double onDaySalary = Math.Round((1 * double.Parse(pg.dtCertainEmp.Rows[0]["BasicSalary"].ToString())) / getDays, 0);

                    pg.ViewState["__absentFine__"] = getAbsentAmount.ToString();

                    pg.getPayable = 0;
                 //   getPayable = NewGross - getAbsentAmount;

                    //-------------------------------------------------------------
                    pg.getPayable = double.Parse(pg.ViewState["__presentSalary__"].ToString()) * double.Parse(pg.ViewState["__PayableDays__"].ToString()) / getDays;
                    pg.getPayable = Math.Round(pg.getPayable - getAbsentAmount, 0);

                    pg.ViewState["__TotalDeduction__"] = (double.Parse(pg.ViewState["__presentSalary__"].ToString()) - pg.getPayable).ToString();  // this line for get total deduction 

                    
                    //-------------------------------------------------------------

                    getAbsentAmount = getDays - byte.Parse(pg.ViewState["__PayableDays__"].ToString());   // now getAbsentAmount=Due Absent days
                    pg.ViewState["__absentFine__"] = (getAbsentAmount * onDaySalary) + double.Parse(pg.ViewState["__absentFine__"].ToString());


                    pg.getAdvanceInstallment = 0;
                    pg.getLoanInstallment = 0;
                    try
                    {
                        if (pg.dtAdvanceInfo.Rows.Count > 0)
                            if (pg.dtCutAdvance.Rows.Count > 0) pg.getAdvanceInstallment = Math.Round(double.Parse(pg.dtCutAdvance.Rows[0]["InstallmentAmount"].ToString()), 0);

                    }
                    catch { }

                    try
                    {
                        if (pg.dtLoanInfo.Rows.Count > 0)
                            if (pg.dtCutLoan.Rows.Count > 0) pg.getLoanInstallment = Math.Round(double.Parse(pg.dtCutLoan.Rows[0]["InstallmentAmount"].ToString()), 0);
                    }
                    catch { }




                   // getNetPayable = Math.Round(((NewGross + TotalOTTaka + getAttendanceBonus + getLunchCost) - (getAbsentAmount + getAdvanceInstallment + getLoanInstallment)), 0);


                    pg.getNetPayable = Math.Round(((pg.getPayable + float.Parse(pg.ViewState["__getTotalOvertimeAmt__"].ToString()) + pg.getAttendanceBonus + pg.getLunchCost) - (pg.getAdvanceInstallment + pg.getLoanInstallment)), 0);

                    // to get finaly  payble amount
                    pg.getTotalSalary = Math.Round((pg.getNetPayable - double.Parse(pg.dtStampDeduct.Rows[0]["StampDeduct"].ToString())), 0);

                  

                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private static void PayableDaysCalculation(string MonthName,string SelectedDay,string CompanyId,payroll_generation pg)    // For Runing employee
        {
            try
            {
                DataTable dt;
                string monthYear = MonthName.Substring(3, 4) + "-" + MonthName.Substring(0, 2);
                sqlDB.fillDataTable("select distinct format(WeekendDate,'yyyy-MM-dd') as WeekendDate from v_Attendance_WeekendInfo where CompanyId='" + CompanyId + "'  AND  MonthName='" + MonthName + "' And WeekendDate >='" + monthYear + "-01' AND WeekendDate <='" + monthYear + '-' + SelectedDay + "' AND WeekendDate not in (select AttDate from tblAttendanceRecord where EmpId='" + pg.dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv')", dt = new DataTable());
                pg.ViewState["__WeekendCount__"] = dt.Rows.Count.ToString();
                
                DataTable dtHoliday = new DataTable();
                sqlDB.fillDataTable("select * from tblHolydayWork where CompanyId='" + CompanyId + "' AND HDate >='" + monthYear + "-01' AND HDate <='" + monthYear + '-' + SelectedDay + "' AND HDate not in (select AttDate from tblAttendanceRecord where EmpId='" + pg.dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv') ", dtHoliday);
                pg.ViewState["__HolidayCount__"] = dtHoliday.Rows.Count.ToString();

                pg.ViewState["__PayableDays__"] = "0";
                string getShortname = pg.ViewState["__CShortName__"].ToString();
                if (getShortname == "MSL")
                {
                    float PayableDays = float.Parse(dt.Rows.Count.ToString()) + float.Parse(pg.ViewState["__cl__"].ToString()) + float.Parse(pg.ViewState["__sl__"].ToString()) + float.Parse(pg.ViewState["__al__"].ToString()) + float.Parse(pg.dtPresent.Compute("Sum(PaybleDay)", "").ToString()) + float.Parse(dtHoliday.Rows.Count.ToString()) + float.Parse(pg.dtAbsent.Rows.Count.ToString());
                    pg.ViewState["__PayableDays__"] = PayableDays.ToString();
                }
                else
                {
                    int PayableDays = dt.Rows.Count + int.Parse(pg.ViewState["__cl__"].ToString()) + int.Parse(pg.ViewState["__sl__"].ToString()) + int.Parse(pg.ViewState["__al__"].ToString()) + pg.dtPresent.Rows.Count + dtHoliday.Rows.Count + pg.dtAbsent.Rows.Count;
                    pg.ViewState["__PayableDays__"] = PayableDays.ToString();
                }
                
            }
            catch { }
        
        }

        private static void getPayableDaysCalculationForML(string MonthName, string SelectedDay,payroll_generation pg)
        {
            try
            {
                DataTable dt;
                string monthYear = MonthName.Substring(3, 4) + "-" + MonthName.Substring(0, 2);
                if (pg.dtPresent.Rows.Count == 0)
                {
                    pg.ViewState["__WeekendCount__"] = "0";
                    pg.ViewState["__HolidayCount__"] = "0";

                }
                else
                {
                    sqlDB.fillDataTable("select * from Attendance_WeekendInfo where  MonthName='" + MonthName + "' And WeekendDate >='" + pg.dtPresent.Rows[0]["ATTDate"].ToString() + "' AND WeekendDate <='" + pg.dtPresent.Rows[pg.dtPresent.Rows.Count - 1]["ATTDate"].ToString() + "'", dt = new DataTable());
                    pg.ViewState["__WeekendCount__"] = dt.Rows.Count.ToString();

                    DataTable dtHoliday = new DataTable();
                    sqlDB.fillDataTable("select * from tblHolydayWork where HDate >='" + pg.dtPresent.Rows[0]["ATTDate"].ToString() + "' AND HDate <='" + pg.dtPresent.Rows[pg.dtPresent.Rows.Count - 1]["ATTDate"].ToString() + "'", dtHoliday);
                    pg.ViewState["__HolidayCount__"] = dtHoliday.Rows.Count.ToString();
                }


                pg.ViewState["__PayableDays__"] = "0";
                string getShortname = pg.ViewState["__CShortName__"].ToString();
                if(getShortname=="MSL")
                {
                    float PayableDays = float.Parse(pg.ViewState["__WeekendCount__"].ToString()) + float.Parse(pg.ViewState["__cl__"].ToString()) + float.Parse(pg.ViewState["__sl__"].ToString()) + float.Parse(pg.ViewState["__al__"].ToString()) + float.Parse(pg.ViewState["__ofl__"].ToString()) + float.Parse(pg.ViewState["__othl__"].ToString()) + float.Parse(pg.dtPresent.Compute("Sum(PaybleDay)", "").ToString()) + float.Parse(pg.ViewState["__HolidayCount__"].ToString()) + float.Parse(pg.dtAbsent.Rows.Count.ToString());
                    pg.ViewState["__PayableDays__"] = PayableDays.ToString();
                }
                else
                {
                    int PayableDays = int.Parse(pg.ViewState["__WeekendCount__"].ToString()) + int.Parse(pg.ViewState["__cl__"].ToString()) + int.Parse(pg.ViewState["__sl__"].ToString()) + int.Parse(pg.ViewState["__al__"].ToString()) + int.Parse(pg.ViewState["__ofl__"].ToString()) + int.Parse(pg.ViewState["__othl__"].ToString()) + pg.dtPresent.Rows.Count + int.Parse(pg.ViewState["__HolidayCount__"].ToString()) + pg.dtAbsent.Rows.Count;
                    pg.ViewState["__PayableDays__"] = PayableDays.ToString();
                }                
                
            }
            catch { }
        }

        private static void saveMonthlyPayrollSheet(string getMonth,string getYear,int getDays,string empName,int i,string selectedDay,int userId,string CompanyId,payroll_generation pg,string SDate)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("insert into Payroll_MonthlySalarySheet(CompanyId,SftId,EmpId,EmpCardNo,YearMonth,DaysInMonth,Activeday,WeekendHoliday,PayableDays," +
                      "CasualLeave,SickLeave,AnnualLeave,OfficialLeave,OthersLeave,FestivalHoliday,AbsentDay,PresentDay,EmpPresentSalary,BasicSalary,HouseRent,MedicalAllownce,ConvenceAllownce,FoodAllownce,TechnicalAllowance," +
                      "OthersAllownce,LunchAllowance,AdvanceDeduction,LoanDeduction,AbsentDeduction,AttendanceBonus,Payable,TotalOTHour,OTRate,TotalOTAmount,NetPayable,Stampdeduct,TotalSalary,DptId," +
                      "DsgId,GrdName,EmpTypeId,EmpStatus,UserId,IsSeperationGeneration,GenerateDate,OTHourForBuyer,OTAmountForBuyer,ExtraOTHour,ExtraOTAmount,NetPayableWithAllOTAmt) " +

                      "values(@CompanyId,@SftId,@EmpId,@EmpCardNo,@YearMonth,@DaysInMonth,@Activeday,@WeekendHoliday,@PayableDays,@CasualLeave," +
                      "@SickLeave,@AnnualLeave,@OfficialLeave,@OthersLeave,@FestivalHoliday,@AbsentDay,@PresentDay,@EmpPresentSalary,@BasicSalary,@HouseRent,@MedicalAllownce,@ConvenceAllownce,@FoodAllownce," +
                      "@TechnicalAllowance,@OthersAllownce,@LunchAllowance,@AdvanceDeduction,@LoanDeduction,@AbsentDeduction,@AttendanceBonus,@Payable,@TotalOTHour,@OTRate,@TotalOTAmount,@NetPayable,@Stampdeduct,@TotalSalary,@DptId," +
                      "@DsgId,@GrdName,@EmpTypeId,@EmpStatus,@UserId,@IsSeperationGeneration,@GenerateDate,@OTHourForBuyer,@OTAmountForBuyer,@ExtraOTHour,@ExtraOTAmount,@NetPayableWithAllOTAmt)", sqlDB.connection);


                cmd.Parameters.AddWithValue("@CompanyId",pg.dtRunningEmp.Rows[i]["CompanyId"].ToString());
                cmd.Parameters.AddWithValue("@SftId", pg.dtRunningEmp.Rows[i]["SftId"].ToString());
                cmd.Parameters.AddWithValue("@EmpId", pg.dtCertainEmp.Rows[0]["EmpId"].ToString());
                cmd.Parameters.AddWithValue("@EmpCardNo", pg.dtCertainEmp.Rows[0]["EmpCardNo"].ToString());
                
                string getYearMonth="01-"+getMonth+"-"+getYear; 
                cmd.Parameters.AddWithValue("@YearMonth",convertDateTime.getCertainCulture(getYearMonth).ToString());
                cmd.Parameters.AddWithValue("@DaysInMonth", dtGetMonthSetup.Rows[0]["TotalDays"].ToString());
                cmd.Parameters.AddWithValue("@Activeday", dtGetMonthSetup.Rows[0]["TotalWorkingDays"].ToString());

                if (joiningMonthIsEqual(getMonth, getYear,CompanyId,SDate,pg) == false)
                {
                    if (pg.dtRunningEmp.Rows[i]["EmpStatus"].ToString().Equals("1"))
                    {
                        if (pg.ViewState["__ml__"].ToString().Equals("0"))
                        {
                            PayableDaysCalculation(getMonth + "-" + getYear, selectedDay,CompanyId,pg);
                            getNetPayableCalculation(getDays,pg);   // this function call to get net payable  amount
                        }
                        else
                        {
                            getPayableDaysCalculationForML(getMonth + "-" + getYear, selectedDay,pg);
                            getNetPayableCalculation(getDays,pg);   // this function call to get net payable  amount
                        }
                    }
                    else if (pg.dtRunningEmp.Rows[i]["EmpStatus"].ToString().Equals("8"))
                    {
                        if (pg.ViewState["__ml__"].ToString().Equals("0"))
                        {
                            PayableDaysCalculation(getMonth + "-" + getYear, selectedDay,CompanyId,pg);
                            getNetPayableCalculation(getDays,pg);   // this function call to get net payable  amount
                        }
                        else
                        {
                            getPayableDaysCalculationForML(getMonth + "-" + getYear, selectedDay,pg);
                            getNetPayableCalculation(getDays,pg);   // this function call to get net payable  amount
                        }

                    }
                }


                cmd.Parameters.AddWithValue("@WeekendHoliday", int.Parse(pg.ViewState["__WeekendCount__"].ToString()) - int.Parse(pg.ViewState["__WeekendAsLeave__"].ToString()));
                cmd.Parameters.AddWithValue("@PayableDays", float.Parse(pg.ViewState["__PayableDays__"].ToString()) - float.Parse(pg.dtAbsent.Rows.Count.ToString()));

                cmd.Parameters.AddWithValue("@CasualLeave", pg.ViewState["__cl__"].ToString());
                cmd.Parameters.AddWithValue("@SickLeave", pg.ViewState["__sl__"].ToString());
                cmd.Parameters.AddWithValue("@AnnualLeave", pg.ViewState["__al__"].ToString());
                cmd.Parameters.AddWithValue("@OfficialLeave", pg.ViewState["__ofl__"].ToString());
                cmd.Parameters.AddWithValue("@OthersLeave", pg.ViewState["__othl__"].ToString());

                cmd.Parameters.AddWithValue("@FestivalHoliday", pg.ViewState["__HolidayCount__"].ToString());
                cmd.Parameters.AddWithValue("@AbsentDay", pg.dtAbsent.Rows.Count.ToString());
                string getShortname = pg.ViewState["__CShortName__"].ToString();
                if (getShortname == "MSL")
                {
                    cmd.Parameters.AddWithValue("@PresentDay", float.Parse(pg.dtPresent.Compute("Sum(PaybleDay)", "").ToString()));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PresentDay", pg.dtPresent.Rows.Count);
                }
                
              
                cmd.Parameters.AddWithValue("@EmpPresentSalary", pg.ViewState["__presentSalary__"].ToString());
                cmd.Parameters.AddWithValue("@BasicSalary", pg.dtCertainEmp.Rows[0]["BasicSalary"].ToString());
                cmd.Parameters.AddWithValue("@HouseRent", pg.dtCertainEmp.Rows[0]["HouseRent"].ToString());
                cmd.Parameters.AddWithValue("@MedicalAllownce", pg.dtCertainEmp.Rows[0]["MedicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@ConvenceAllownce", pg.dtCertainEmp.Rows[0]["ConvenceAllownce"].ToString());
                cmd.Parameters.AddWithValue("@FoodAllownce", pg.dtCertainEmp.Rows[0]["FoodAllownce"].ToString());
                cmd.Parameters.AddWithValue("@TechnicalAllowance", pg.dtCertainEmp.Rows[0]["TechnicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@OthersAllownce", pg.dtCertainEmp.Rows[0]["OthersAllownce"].ToString());
                cmd.Parameters.AddWithValue("@LunchAllowance", pg.getLunchCost);

                cmd.Parameters.AddWithValue("@AdvanceDeduction", pg.getAdvanceInstallment);
                cmd.Parameters.AddWithValue("@LoanDeduction", pg.getLoanInstallment);

               // cmd.Parameters.AddWithValue("@AbsentDeduction", ViewState["__absentFine__"].ToString());

                cmd.Parameters.AddWithValue("@AbsentDeduction", pg.ViewState["__TotalDeduction__"].ToString());
                cmd.Parameters.AddWithValue("@AttendanceBonus", pg.getAttendanceBonus);
                
                cmd.Parameters.AddWithValue("@Payable", pg.getPayable);
                cmd.Parameters.AddWithValue("@TotalOTHour", pg.ViewState["__getTotalOverTime__"].ToString());
                
                cmd.Parameters.AddWithValue("@OTRate", pg.getOTRate);
                cmd.Parameters.AddWithValue("@TotalOTAmount", pg.ViewState["__getTotalOvertimeAmt__"].ToString());
                cmd.Parameters.AddWithValue("@NetPayable", pg.getNetPayable);
                cmd.Parameters.AddWithValue("@Stampdeduct", Math.Round(double.Parse(pg.dtStampDeduct.Rows[0]["StampDeduct"].ToString()), 0));
                cmd.Parameters.AddWithValue("@TotalSalary", pg.getTotalSalary);

                
                cmd.Parameters.AddWithValue("@DptId", pg.dtCertainEmp.Rows[0]["DptId"].ToString());
                cmd.Parameters.AddWithValue("@DsgId", pg.dtCertainEmp.Rows[0]["DsgId"].ToString());
                cmd.Parameters.AddWithValue("@GrdName", pg.dtCertainEmp.Rows[0]["GrdName"].ToString());
                cmd.Parameters.AddWithValue("@EmpTypeId",pg.dtRunningEmp.Rows[i]["EmpTypeId"].ToString());
                cmd.Parameters.AddWithValue("@EmpStatus", '1');

                cmd.Parameters.AddWithValue("@UserId",userId.ToString());
                cmd.Parameters.AddWithValue("@IsSeperationGeneration","0");
                cmd.Parameters.AddWithValue("@GenerateDate",convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy"))).ToString();
                cmd.Parameters.AddWithValue("@OTHourForBuyer", pg.OT_Hour_ForBuyer_AsRegular);
                cmd.Parameters.AddWithValue("@OTAmountForBuyer", pg.ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"].ToString());
                cmd.Parameters.AddWithValue("@ExtraOTHour", pg.Extra_OT_Hour_OfEmp);
                cmd.Parameters.AddWithValue("@ExtraOTAmount",pg.ViewState["__Extra_OT_Amt_OfEmp__"].ToString());
                cmd.Parameters.AddWithValue("@NetPayableWithAllOTAmt",pg.getNetPayableWithAllOTAmt);
                cmd.ExecuteNonQuery();

                Advance_And_Loan_StatusChange(pg);  // For advance and loan status change


             //  lbProcessingStatus.Items.Add("Processing completed of "+dtRunningEmp.Rows[i]["EmpType"].ToString() +" "+empName+" Card No. "+dtRunningEmp.Rows[i]["EmpCardNo"].ToString()+""); 

            }
            catch (Exception ex)
            {
                //lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private static void Advance_And_Loan_StatusChange(payroll_generation pg)
        {
                try
                {
                    if (pg.dtCutAdvance.Rows.Count != 0)   // for change Advance status
                    {
                        if (pg.dtAdvanceInfo.Rows[0]["InstallmentNo"].ToString().Equals(pg.dtAdvanceInfo.Rows[0]["PaidInstallmentNo"].ToString()))
                        {
                            SqlCommand cmd = new System.Data.SqlClient.SqlCommand("update Payroll_AdvanceInfo set PaidStatus ='1' where AdvanceId='" + pg.dtAdvanceInfo.Rows[0]["AdvanceId"].ToString() + "'", sqlDB.connection);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch { }

                try
                {
                    if (pg.dtCutLoan.Rows.Count != 0)   // for change Loan status
                {
                    if (pg.dtLoanInfo.Rows[0]["InstallmentNo"].ToString().Equals(pg.dtLoanInfo.Rows[0]["PaidInstallmentNo"].ToString()))
                    {
                        SqlCommand cmd = new System.Data.SqlClient.SqlCommand("update Payroll_LoanInfo set PaidStatus ='1' where LoanId='" + pg.dtLoanInfo.Rows[0]["LoanId"].ToString() + "'", sqlDB.connection);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }

        protected void rbGenaratingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbGenaratingType.SelectedValue.ToString().Equals("1")) // for certain employee
            {
               
                txtEmpCardNo.Enabled = true;
                ddlEmpCardNo.Enabled = true;
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

                getSelectedShiftId();
                classes.Employee.LoadEmpCardNoWithNameByCompanyRShift(ddlEmpCardNo,CompanyId);
            }
            else
            {
                txtEmpCardNo.Enabled = false;
                ddlEmpCardNo.Enabled = false;
               
            }
        }

        protected void rbEmpTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            txtEmpCardNo.Text = "";
           // if (rbGenaratingType.SelectedValue.ToString().Equals("1"))
          //  classes.Employee.LoadEmpCardNoWithName(ddlEmpCardNo, rbEmpTypeList.SelectedValue);
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
        private void addAllTextInShift()
        {
            try
            {
                if (ddlShiftList.Items.Count > 2)
                    ddlShiftList.Items.Insert(1, new ListItem("All", "00"));
            }
            catch { }
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                classes.commonTask.LoadShift(ddlShiftList, CompanyId); addAllTextInShift();
            }
            catch { }
        }

        protected void ddlShiftList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               
                if (rbGenaratingType.SelectedIndex == 1)
                {
                    string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

                    getSelectedShiftId();
                    classes.Employee.LoadEmpCardNoWithNameByCompanyRShift(ddlEmpCardNo, CompanyId, ViewState["__getRequiredSftId__"].ToString());
                }
            }
            catch { }
        }


        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object Operation(string SDate,string SCompanyId,string GType,string ECardNo,string EmpId)
        {
            try
            {
                // return false;
                HttpSessionState session = HttpContext.Current.Session;

                //Separate thread for long running operation
                
                 string getUserId=  HttpContext.Current.Session["__getUserId__"].ToString();
                 string getShortname = HttpContext.Current.Session["__CShortName__"].ToString();                 
                ThreadPool.QueueUserWorkItem(delegate
                {

                    payroll_generation pg = new payroll_generation();
                    pg.ViewState["__CShortName__"] = getShortname;
                    // payroll_generation page = (payroll_generation)HttpContext.Current.CurrentHandler;


                    string[] getDays = SDate.Trim().Split('-');
                    int days = DateTime.DaysInMonth(int.Parse(getDays[2]), int.Parse(getDays[1]));

                    string MonthName = getDays[1] + "-" + getDays[2];
                    string MonthName2 = getDays[2] + "-" + getDays[1];
                    string month = getDays[1];
                    string year = getDays[2];
                    int Days = days;
                    string selectDays = getDays[0];



                    #region   for weeken info of selected month----------------------------------------------------------------------------------------------

                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select distinct convert(varchar(11),WeekendDate,111) as WeekendDate from v_Attendance_WeekendInfo where MonthName='" + MonthName + "' AND WeekendDate >='" + year + '-' + month + "-01" + "' AND WeekendDate<='" + year + '-' + month + '-' + selectDays + "'", dt);

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

                    #endregion------------------------------------------------------------------------------------------------------------------------------



                    loadMonthSetup("1", getDays[1], getDays[2], SCompanyId);


                    if (GType == "0") salarySheetClearByMonthYear(getDays[1], getDays[2], SCompanyId);

                    else 
                    {
                        string[] GetEmpCardNo = ECardNo.Split(' ');
                        salarySheetClearForCertainEmployeeByMonthYear(getDays[1], getDays[2], SCompanyId, GetEmpCardNo[0]);

                    }





                    double getTotalOT;
                    string getJoingingDate;


                    // get stamp card price
                    sqlDB.fillDataTable("select StampDeduct from HRD_AllownceSetting where AllownceId =(select max(AllownceId) from HRD_AllownceSetting)", pg.dtStampDeduct = new DataTable());



                    // for get company id
                    string CompanyId = SCompanyId;



                    //check overTime is active ?
                    //pg.checkOverTimeIsActiveForSelectedShift(CompanyId);

                    if (GType.Equals("0"))   // generating type for all employee
                    {
                        // for delete existing salary sheet of this month
                        salarySheetClearByMonthYear(month, year, CompanyId);

                        // get all regular employee at this time
                        sqlDB.fillDataTable("select distinct EmpCardNo,EmpName, max(SN) as SN,EmpType,EmpTypeId,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId  from v_Personnel_EmpCurrentStatus group by EmpCardNo,EmpName,EmpTypeId,EmpType,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId having EmpStatus in('1','8') AND ActiveSalary='true' AND IsActive='1' AND CompanyId='" + CompanyId + "' order by SN", pg.dtRunningEmp = new DataTable());
                    }

                    else    // generating type for single employee
                    {
                        if (ECardNo.Length >= 4)  // valid card justification
                        {
                            //salarySheetClearForCertainEmployeeByMonthYear(month, year, CompanyId);

                            // get max SN of employee,whose active salary status is true 
                            sqlDB.fillDataTable("select EmpCardNo,EmpName,MAX(SN) as SN,EmpType,EmpTypeId,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId  from v_Personnel_EmpCurrentStatus group by EmpCardNo,EmpName,EmpType,EmpTypeId,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId having EmpCardNo Like'%" + ECardNo + "' AND  EmpStatus in ('1','8') AND ActiveSalary='true' AND IsActive='1' AND CompanyId='" + CompanyId + "' ", pg.dtRunningEmp = new DataTable());

                        }
                        else
                        {
                            //lblMessage.Text = "error->Please type valid card no of an employee";
                            //txtEmpCardNo.Focus();
                        }
                    }

                    double getTime = Math.Round((double.Parse(pg.dtRunningEmp.Rows.Count.ToString())) / 10, 0);






                    int operationProgress;






                    for (int i = 0; i < pg.dtRunningEmp.Rows.Count; i++)
                    {

                        int getValue = 0;
                        if (GType != "1")
                        {
                            // for get operation progress--------------------------------

                            if (i != 0) getValue = (100 * i / (pg.dtRunningEmp.Rows.Count - 1));

                            // Response.End();
                            //  Response.Flush();
                            //System.Threading.Thread.Sleep(1000);
                        }
                        //------------------------------------------------------------


                        // get essential information of a certain employee 
                        sqlDB.fillDataTable("select EmpId,EmpCardNo,BasicSalary,MedicalAllownce,FoodAllownce,ConvenceAllownce,HouseRent,TechnicalAllownce,OthersAllownce,EmpPresentSalary,AttendanceBonus,LunchCount,LunchAllownce,DptId,GrdName,DsgId,sftId from v_Personnel_EmpCurrentStatus where SN=" + pg.dtRunningEmp.Rows[i]["SN"].ToString() + "", pg.dtCertainEmp = new DataTable());

                        // get Proximity number of a certain employee
                        sqlDB.fillDataTable("select convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate from Personnel_EmployeeInfo where EmpId='" + pg.dtCertainEmp.Rows[0]["EmpId"].ToString() + "'", pg.dt = new DataTable());
                        pg.ViewState["__getJoingingDate__"] = pg.dt.Rows[0]["EmpJoiningDate"].ToString();

                        // get leave information of a certain employee
                        sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId,StateStatus from v_tblAttendanceRecord where IsActive='1' and ATTStatus='lv' AND MonthName ='" + year + '-' + month + "' AND EmpId='" + pg.dtCertainEmp.Rows[0]["EmpId"].ToString() + "' And AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", pg.dtLeaveInfo = new DataTable());
                        pg.getAllLeaveInformation();

                        // get present information of a certain employee
                        if(getShortname=="MSL")
                        {
                            sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(11),ATTDate,111) as ATTDate,InHour,InMin,OutHour,OutMin,ATTStatus,CASE WHEN DATEDIFF(HOUR,'00:00:00',StayTime)<8 and DATEDIFF(HOUR,'00:00:00',StayTime)>=5 THEN (CONVERT(FLOAT,1) / 2)  ELSE CASE WHEN DATEDIFF(HOUR,'00:00:00',StayTime)>=8 then 1 else 0 end END AS 'PaybleDay' from v_tblAttendanceRecord where IsActive='1' and EmpId='" + pg.dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus In ('P','L') AND MonthName='" + MonthName2 + "' AND AttDate Not  " + setPredicate + " AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' ", pg.dtPresent = new DataTable());
                        }
                        else
                        {
                            // get present information of a certain employee
                            sqlDB.fillDataTable("select distinct EmpId,Convert(varchar(11),ATTDate,111) as ATTDate,InHour,InMin,OutHour,OutMin,ATTStatus from v_tblAttendanceRecord where EmpId='" + pg.dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus In ('P','L') AND MonthName='" + MonthName2 + "' AND AttDate Not  " + setPredicate + " AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' ", pg.dtPresent = new DataTable());
                        }                       

                        // get late information of a certain employee
                        sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate, EmpId from v_tblAttendanceRecord where IsActive='1' and  EmpId='" + pg.dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='L' AND MonthName='" + MonthName2 + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", pg.dtLate = new DataTable());

                        // get absent information of a certain employee
                        sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId from v_tblAttendanceRecord where IsActive='1' and  EmpId='" + pg.dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='A' AND MonthName='" + MonthName2 + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", pg.dtAbsent = new DataTable());

                        // check attendance bonus of a certain employee
                        pg.checkForAttendanceBonus();

                        // Call Over time Callculation for count OT taka

                        // for checking overtime is active this shift 

                       //  DataRow[] dr = pg.dtShiftListForCheckOverTime.Select("SftId=" + pg.dtCertainEmp.Rows[0]["SftId"].ToString() + " AND SftOverTime='true'");

                        //if (dr.Length > 0)
                        pg.OverTimeCalculation(pg.dtCertainEmp.Rows[0]["EmpId"].ToString(),month,year,days.ToString(), double.Parse(pg.dtCertainEmp.Rows[0]["BasicSalary"].ToString()),pg);
                       
                        pg.ViewState["__getMonthYear__"] = month+"-"+year;
                        // get advance information of a certain employee 
                        sqlDB.fillDataTable("select Max(SL) as SL,AdvanceId,PaidInstallmentNo,InstallmentNo from Payroll_AdvanceInfo Where EmpCardNo='" + pg.dtRunningEmp.Rows[i]["EmpCardNo"].ToString() + "' AND EmpTypeId=" + pg.dtRunningEmp.Rows[i]["EmpTypeId"].ToString() + " AND PaidStatus='0'  group By AdvanceId,PaidInstallmentNo,InstallmentNo ", pg.dtAdvanceInfo = new DataTable());

                        if (pg.dtAdvanceInfo.Rows.Count > 0)
                        {
                            // get information employee are aggre for give advance installment ?
                            sqlDB.fillDataTable("select InstallmentAmount,PaidInstallmentNo,PaidMonth from Payroll_AdvanceSetting where AdvanceId ='" + pg.dtAdvanceInfo.Rows[0]["AdvanceId"].ToString() + "' AND PaidMonth='" + pg.ViewState["__getMonthYear__"].ToString() + "'", pg.dtCutAdvance = new DataTable());

                        }
                        else
                        {
                            pg.dtAdvanceInfo = new DataTable(); pg.dtCutAdvance = new DataTable();
                            // this extra query for when 1 times advance are executed and next time again is needed to generea the same month salary
                            sqlDB.fillDataTable("select distinct AdvanceId,InstallmentAmount,PaidInstallmentNo,PaidMonth from v_Payroll_AdvanceSetting where  PaidMonth='" + pg.ViewState["__getMonthYear__"].ToString() + "' AND EmpId='" + pg.dtCertainEmp.Rows[0]["EmpId"].ToString() + "'", pg.dtCutAdvance = new DataTable());
                            if (pg.dtCutAdvance.Rows.Count > 0)
                                sqlDB.fillDataTable("select AdvanceId,InstallmentNo,PaidInstallmentNo from Payroll_AdvanceInfo Where AdvanceId='" + pg.dtCutAdvance.Rows[0]["AdvanceId"].ToString() + "' ", pg.dtAdvanceInfo = new DataTable());
                        }

                        // get loan information of a certain employee 
                        sqlDB.fillDataTable("select Max(SL) as SL,LoanId,PaidInstallmentNo,InstallmentNo from Payroll_LoanInfo Where EmpCardNo='" + pg.dtRunningEmp.Rows[i]["EmpCardNo"].ToString() + "' AND EmpTypeId=" + pg.dtRunningEmp.Rows[i]["EmpTypeId"].ToString() + " AND PaidStatus='0' group By LoanId,PaidInstallmentNo,InstallmentNo ", pg.dtLoanInfo = new DataTable());

                        if (pg.dtLoanInfo.Rows.Count > 0)
                        {
                            // get information employee are aggre for give loan installment ?
                            sqlDB.fillDataTable("select InstallmentAmount,PaidInstallmentNo,PaidMonth from Payroll_LoanSetting where LoanId ='" + pg.dtLoanInfo.Rows[0]["LoanId"].ToString() + "' AND PaidMonth='" + 123 + "'", pg.dtCutLoan = new DataTable());
                        }

                        //if (rbEmpTypeList.SelectedItem.ToString().ToLower().Equals("staff"))checkLunchCost();
                        //else getLunchCost = 0;

                        saveMonthlyPayrollSheet(month, year, Days, pg.dtRunningEmp.Rows[i]["EmpName"].ToString(), i, selectDays, int.Parse(getUserId), CompanyId, pg, SDate);

                        //   if (!isActiveLoop) break;

                        session["OPERATION_PROGRESS"] = getValue;
                        Thread.Sleep(1000);
                    }

                    
                });

                return new { progress = 0 };
            }
            catch { return 0; };
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object OperationProgress()
        {
            int operationProgress = 0;                    
            if (HttpContext.Current.Session["OPERATION_PROGRESS"] != null)
                operationProgress = (int)HttpContext.Current.Session["OPERATION_PROGRESS"];

            return new { progress = operationProgress };
        }

        
    }
}