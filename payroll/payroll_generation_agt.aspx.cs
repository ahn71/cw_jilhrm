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
    public partial class payroll_generaion_agt : System.Web.UI.Page
    {
        DataTable dt;
        SqlCommand cmd;
        string Org = "JIL";
        string sqlCmd = "";
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
                ViewState["___IsGerments__"] = classes.Payroll.Office_IsGarments();
                if (ViewState["___IsGerments__"].ToString().Equals("False"))
                {
                    if (ddlCompanyList.SelectedValue == "0001")
                        txtNotTiffinCardno.Text = "";
                    else
                        txtNotTiffinCardno.Text = "0069,0037";
                }
                           
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

            string[] getDays = txtGenerateMonth.Text.Trim().Split('-');
            int DaysInMonth = DateTime.DaysInMonth(int.Parse(getDays[2]), int.Parse(getDays[1]));
            ViewState["__year__"] = getDays[2];
            ViewState["__month__"] = getDays[2]+"-"+getDays[1]+"-"+getDays[0];    
            loadMonthSetup("1", getDays[1], getDays[2], CompanyId);
            generateMonthlySalarySheet(getDays[1] + "-" + getDays[2], getDays[1], getDays[2], DaysInMonth, getDays[0]);
          
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
        DataTable dtLeaveWithoutPay;
        DataTable dtPresent;
        DataTable dtAbsent;
        DataTable dtLate;
        DataTable dtHalfDayDeduct;
        DataTable dtAdvanceInfo;
        DataTable dtCutAdvance;
        DataTable dtLoanInfo;
        DataTable dtMobileCell;
        DataTable dtAdjustment;
        DataTable dtCutLoan;
        DataTable dtStampDeduct;
        DataTable dtOverTime_stayTime;
        DataTable dtTiffin_Staff_WorkerTaka;
        DataTable dtShortleave;
        DataTable dtOtherspay;
        DataTable dtOthersDeduction;
       // DataTable dtPFDeduction;
        DataTable dtTaxDeduction;

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


                string _empCardNo = "";
                if (txtNotTiffinCardno.Text.Trim().Length > 0)
                {
                    string[] empCardno = txtNotTiffinCardno.Text.Trim().Split(',');                    
                    foreach (string item in empCardno)
                    {
                        _empCardNo += ",'" + item + "'";
                    }                    
                    _empCardNo = " and SUBSTRING(EmpCardNo,10,6) not in(" + _empCardNo.Remove(0, 1) + ")";
                }

                //check overTime is active ?             

                if (rbGenaratingType.SelectedValue.ToString().Equals("0"))   // generating type for all employee
                {
                    // for delete existing salary sheet of this month
                    salarySheetClearByMonthYear(month, year, CompanyId);

                    // get all regular employee at this time
                    sqlDB.fillDataTable("select distinct EmpCardNo,EmpName, max(SN) as SN,EmpType,EmpTypeId,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId,OverTime  from v_Personnel_EmpCurrentStatus group by EmpCardNo,EmpName,EmpTypeId,EmpType,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId,OverTime having EmpStatus in('1','8') AND ActiveSalary='true' AND IsActive='1' AND CompanyId='" + CompanyId + "' "+ _empCardNo + "  order by SN", dtRunningEmp = new DataTable());
                }

                else    // generating type for single employee
                {
                    if (txtEmpCardNo.Text.Trim().Length >= 4)  // valid card justification
                    {
                        salarySheetClearByMonthYear(month, year, CompanyId, txtEmpCardNo.Text);
                        // get max SN of employee,whose active salary status is true 
                        sqlDB.fillDataTable("select EmpCardNo,EmpName,MAX(SN) as SN,EmpType,EmpTypeId,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId,OverTime  from v_Personnel_EmpCurrentStatus group by EmpCardNo,EmpName,EmpType,EmpTypeId,EmpStatus,ActiveSalary,IsActive,CompanyId,SftId,OverTime having EmpCardNo Like'%" + txtEmpCardNo.Text + "' AND  EmpStatus in ('1','8') AND ActiveSalary='true' AND IsActive='1' AND CompanyId='" + CompanyId + "' ", dtRunningEmp = new DataTable());

                    }
                    else
                    {
                        lblMessage.InnerText = "error->Please type valid card no of an employee";
                        txtEmpCardNo.Focus();
                    }
                }

                double getTime = Math.Round((double.Parse(dtRunningEmp.Rows.Count.ToString())) / 10, 0);
                bool isgarments = bool.Parse(ViewState["___IsGerments__"].ToString());
                //  System.Threading.Thread.Sleep(TimeSpan.FromSeconds(getTime));
                //imgLoading.Visible = true;

                for (int i = 0; i < dtRunningEmp.Rows.Count; i++)
                {

                    int getValue = 0;
                    if (rbGenaratingType.SelectedValue.ToString() != "1")
                    {
                        // for get operation progress--------------------------------

                        if (i != 0) getValue = (100 * i / (dtRunningEmp.Rows.Count-1));
                        //probar.Style.Add("width",getValue.ToString()+"%");
                        //probar.InnerHtml = getValue.ToString()+"%";
                      //  ProgressBar1.Value = getValue;
                       
                       // Response.Write(getValue.ToString() + "%");
                        
                      //  System.Threading.Thread.Sleep(1000);
                    }
                    //------------------------------------------------------------
                    

                    // get essential information of a certain employee 
                    sqlDB.fillDataTable("select EmpId,EmpCardNo,BasicSalary,MedicalAllownce,FoodAllownce,ConvenceAllownce,HouseRent,TechnicalAllownce,OthersAllownce,EmpPresentSalary,AttendanceBonus,LunchCount,LunchAllownce,DptId,GrdName,DsgId,sftId,DormitoryRent,PFAmount,IncomeTax,ISNULL( PfMember,0) as PfMember from v_Personnel_EmpCurrentStatus where SN=" + dtRunningEmp.Rows[i]["SN"].ToString() + "", dtCertainEmp = new DataTable());

                    // delete existing record of Leave deduction 
                    CRUD.Execute("delete Leave_MonthlyLeaveDeductionRecord where EmpID='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and  format(Month,'yyyy-MM')='" +year+"-"+month+"'", sqlDB.connection);


                    // get Proximity number of a certain employee
                    sqlDB.fillDataTable("select convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,convert(varchar(10),EmpJoiningDate,120) as EmpJoiningDate1 from Personnel_EmployeeInfo where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "'", dt = new DataTable());
                    ViewState["__getJoingingDate__"] = dt.Rows[0]["EmpJoiningDate"].ToString();
                    ViewState["__EmpJoiningDate__"] = dt.Rows[0]["EmpJoiningDate1"].ToString();

                    // get leave information of a certain employee
                    sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId,StateStatus from v_tblAttendanceRecord where ATTStatus='lv' and StateStatus<>'Leave Without Pay (LWP)' AND MonthName ='" + year + '-' + month + "' AND EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' And AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", dtLeaveInfo = new DataTable());
                    // get leave without pay of a certain employee
                    sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId,StateStatus from v_tblAttendanceRecord where ATTStatus='lv' and StateStatus='Leave Without Pay (LWP)' AND MonthName ='" + year + '-' + month + "' AND EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' And AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "'", dtLeaveWithoutPay = new DataTable());
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
                    //if (isgarments)
                    //{
                    //    sqlDB.fillDataTable("select  convert(varchar(11),AttDate,111) as AttDate, EmpId from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='L' and IsHalfDayDeduct<>1  AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' AND PaybleDays='1' ", dtLate = new DataTable());
                    //}
                    //else
                    //{
                        sqlDB.fillDataTable("select  convert(varchar(11),AttDate,111) as AttDate, EmpId from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='L'  AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' ", dtLate = new DataTable());
                    //}

                    // get half day leave deduct  information of a certain employee
                   // sqlDB.fillDataTable("select convert(varchar(11),AttDate,111) as AttDate, EmpId from tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "'  AND ATTStatus In ('P','L') and IsHalfDayDeduct=1 AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' ", dtHalfDayDeduct = new DataTable());
                  
                    // get absent information of a certain employee
                    if (isgarments)
                    {
                        sqlDB.fillDataTable("select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='A' AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' Union select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus In ('P','L') AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' AND PaybleDays='0' ", dtAbsent = new DataTable());
                    }
                    else
                    {
                        string sql = "select distinct convert(varchar(11),AttDate,111) as AttDate,EmpId from v_tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND ATTStatus='A' AND MonthName='" + MonthName + "' AND AttDate >='" + year + '-' + month + '-' + "01" + "' AND AttDate <= '" + year + '-' + month + '-' + selectDays + "' ";
                        sqlDB.fillDataTable(sql, dtAbsent = new DataTable()); 
                    }
                    //get short leave

                    sqlDB.fillDataTable("select distinct convert(varchar(11),LvDate,111) as LvDate,EmpId from Leave_ShortLeave where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND LvDate >='" + year + '-' + month + '-' + "01" + "' AND LvDate <= '" + year + '-' + month + '-' + selectDays + "' ", dtShortleave = new DataTable());

                    //get Other's Pay

                    sqlDB.fillDataTable("select  ISNULL(Sum(OtherPay),0) OtherPay from Payroll_OthersPay where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND IsActive='1' ", dtOtherspay = new DataTable());

                    //get Other's Deduction

                    sqlDB.fillDataTable("select ISNULL(Sum(PAmount),0) PAmount from Payroll_Punishment where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND MonthName ='" + month + '-' + year + "' ", dtOthersDeduction = new DataTable());
                    
                    //get PF Deduction
                    if(dtCertainEmp.Rows[0]["PfMember"].ToString()=="True")
                        ViewState["__PFAmount__"] = dtCertainEmp.Rows[0]["PFAmount"].ToString();
                    else
                        ViewState["__PFAmount__"] = "0";




                    //   sqlDB.fillDataTable("select ISNULL(Sum(EmpContributionAmount),0) PFAmount from PF_CalculationDetails where  EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND CONVERT(VARCHAR(7), convert(date,'01-'+MonthName),120) ='" + year + '-' + month + "' ", dtPFDeduction = new DataTable());

                    //get Tax Deduction

                    // if(isgarments==false)
                    //   sqlDB.fillDataTable("select TaxAmount from VatTax_IncomeTaxDetailsLog where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and Month='" + year + "-" + month + "-01'", dtTaxDeduction = new DataTable());
                    // else
                    sqlDB.fillDataTable("select isnull(IncomeTax,0) as TaxAmount from Personnel_EmpCurrentStatus where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and IsActive=1", dtTaxDeduction = new DataTable());                
                        ViewState["__TaxAmount__"] = (dtTaxDeduction.Rows.Count > 0) ? dtTaxDeduction.Rows[0]["TaxAmount"].ToString() : "0";
                   
                  
                    
                       // ViewState["__TaxAmount__"] = dtCertainEmp.Rows[0]["IncomeTax"].ToString() ;

                    // check attendance bonus of a certain employee
                  //  checkForAttendanceBonus(month, year, dtCertainEmp.Rows[0]["EmpId"].ToString());

                        if (Org != "JIL") // this block is not need to execute for Jaksion Internation Limited.This block contain Overtime,Tiffin, Holiday etc..
                        {
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
                                try
                                {
                                    string[] NotTiffinCardno = txtNotTiffinCardno.Text.Split(',');
                                    if (NotTiffinCardno.Contains(dtCertainEmp.Rows[0]["EmpCardNo"].ToString().Substring(9, dtCertainEmp.Rows[0]["EmpCardNo"].ToString().Length - 9)))
                                        ViewState["__Tiffindays__"] = "0";
                                    else
                                        ViewState["__Tiffindays__"] = dtTiffin_Holidays.Rows[0]["TiffinCount"].ToString();
                                }
                                catch { }
                                ViewState["__TiffinBillAmount__"] = (float.Parse(ViewState["__Tiffindays__"].ToString()) * float.Parse(tiffincount)).ToString();
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
                        }
                        else 
                        {
                            ViewState["__getTotalOvertimeAmt__"] = "0"; 
                            ViewState["__getTotalOverTime__"] = "0";
                            ViewState["__OT_Amt_Hour_ForBuyer_AsRegular__"] = "0";
                            ViewState["__Extra_OT_Amt_OfEmp__"] = "0";
                            ViewState["__getOverTime__"] = "0";
                            ViewState["__getOtherOverTime__"] = "0";

                            ViewState["__TiffinTaka__"] = "0";
                            ViewState["__HolidayTaka__"] = "0";
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

                    // get Mobile Cell information of a certain employee 
                    sqlDB.fillDataTable("select ISNULL(sum(MobileCell),0) MobileCell  from Payroll_DeductMobileCell where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and Month='"+year+"-"+month+"-01'", dtMobileCell = new DataTable());

                    // get Adjustment information of a certain employee 
                    sqlDB.fillDataTable("select AdjustmentTypeID,Amount,Remarks from Payroll_MonthlyAdjustment where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and Month='" + year + "-" + month + "-01'", dtAdjustment = new DataTable());

                    saveMonthlyPayrollSheet(month, year,DaysInMonth, dtRunningEmp.Rows[i]["EmpName"].ToString(), i, selectDays, int.Parse(Session["__getUserId__"].ToString()), CompanyId, pg,txtGenerateMonth.Text);                 
                }               
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
        private  void saveMonthlyPayrollSheet(string getMonth, string getYear, int DaysInMonth, string empName, int i, string selectedDay, int userId, string CompanyId, payroll_generation pg, string SDate)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("insert into Payroll_MonthlySalarySheet(CompanyId,SftId,EmpId,EmpCardNo,YearMonth,DaysInMonth,Activeday,WeekendHoliday,PayableDays," +
                      "CasualLeave,SickLeave,AnnualLeave,OfficialLeave,OthersLeave,FestivalHoliday,AbsentDay,PresentDay,EmpPresentSalary,BasicSalary,HouseRent,MedicalAllownce,ConvenceAllownce,FoodAllownce,TechnicalAllowance," +
                      "OthersAllownce,LunchAllowance,AdvanceDeduction,LoanDeduction,AbsentDeduction,AttendanceBonus,Payable,TotalOTHour,OTRate,TotalOTAmount,NetPayable,Stampdeduct,TotalSalary,DptId," +
                      "DsgId,GrdName,EmpTypeId,EmpStatus,UserId,IsSeperationGeneration,GenerateDate,OTHourForBuyer,OTAmountForBuyer,ExtraOTHour,ExtraOTAmount,NetPayableWithAllOTAmt,LateDays,LateFine,LateFineDays,TiffinDays,TiffinTaka,TiffinBillAmount,HolidayWorkingDays,HolidayTaka,HoliDayBillAmount,DormitoryRent,ProvidentFund,TotalOverTime,TotalOtherOverTime,OthersPay,OthersDeduction,ShortLeave,ProfitTax,MobileCell,LWP,Adjustment,NetEmpPresentSalary,LWPDeduction) " +

                      "values(@CompanyId,@SftId,@EmpId,@EmpCardNo,@YearMonth,@DaysInMonth,@Activeday,@WeekendHoliday,@PayableDays,@CasualLeave," +
                      "@SickLeave,@AnnualLeave,@OfficialLeave,@OthersLeave,@FestivalHoliday,@AbsentDay,@PresentDay,@EmpPresentSalary,@BasicSalary,@HouseRent,@MedicalAllownce,@ConvenceAllownce,@FoodAllownce," +
                      "@TechnicalAllowance,@OthersAllownce,@LunchAllowance,@AdvanceDeduction,@LoanDeduction,@AbsentDeduction,@AttendanceBonus,@Payable,@TotalOTHour,@OTRate,@TotalOTAmount,@NetPayable,@Stampdeduct,@TotalSalary,@DptId," +
                      "@DsgId,@GrdName,@EmpTypeId,@EmpStatus,@UserId,@IsSeperationGeneration,@GenerateDate,@OTHourForBuyer,@OTAmountForBuyer,@ExtraOTHour,@ExtraOTAmount,@NetPayableWithAllOTAmt,@LateDays,@LateFine,@LateFineDays,@TiffinDays,@TiffinTaka,@TiffinBillAmount,@HolidayWorkingDays,@HolidayTaka,@HoliDayBillAmount,@DormitoryRent,@ProvidentFund,@TotalOverTime,@TotalOtherOverTime,@OthersPay,@OthersDeduction,@ShortLeave,@ProfitTax,@MobileCell,@LWP,@Adjustment,@NetEmpPresentSalary,@LWPDeduction); SELECT SCOPE_IDENTITY()", sqlDB.connection);


                cmd.Parameters.AddWithValue("@CompanyId", dtRunningEmp.Rows[i]["CompanyId"].ToString());
                cmd.Parameters.AddWithValue("@SftId", dtRunningEmp.Rows[i]["SftId"].ToString());
                cmd.Parameters.AddWithValue("@EmpId", dtCertainEmp.Rows[0]["EmpId"].ToString());
                cmd.Parameters.AddWithValue("@EmpCardNo", dtCertainEmp.Rows[0]["EmpCardNo"].ToString());

                string getYearMonth = getYear + "-" + getMonth +"-01";
               // getYearMonth = convertDateTime.getCertainCulture(getYearMonth).ToString();
                cmd.Parameters.AddWithValue("@YearMonth", getYearMonth);
                cmd.Parameters.AddWithValue("@DaysInMonth", dtGetMonthSetup.Rows[0]["TotalDays"].ToString());
                cmd.Parameters.AddWithValue("@Activeday", dtGetMonthSetup.Rows[0]["TotalWorkingDays"].ToString());

                if (joiningMonthIsEqual(getMonth, getYear, CompanyId, SDate, pg,DaysInMonth) == false)
                {
                
                            PayableDaysCalculation(getMonth + "-" + getYear, selectedDay, CompanyId, pg);
                            checkForAttendanceBonus(getMonth, getYear, dtCertainEmp.Rows[0]["EmpId"].ToString());
                            getNetPayableCalculation(DaysInMonth, pg);   // this function call to get net payable  amount                
                }
                cmd.Parameters.AddWithValue("@WeekendHoliday", int.Parse(ViewState["__WeekendCount__"].ToString()) - int.Parse(ViewState["__WeekendAsLeave__"].ToString()));
                cmd.Parameters.AddWithValue("@PayableDays", ViewState["__PayableDays__"].ToString());

                cmd.Parameters.AddWithValue("@CasualLeave", ViewState["__cl__"].ToString());
                cmd.Parameters.AddWithValue("@SickLeave", ViewState["__sl__"].ToString());
                cmd.Parameters.AddWithValue("@AnnualLeave", ViewState["__al__"].ToString());
                cmd.Parameters.AddWithValue("@OfficialLeave",ViewState["__ofl__"].ToString());
                cmd.Parameters.AddWithValue("@OthersLeave", ViewState["__othl__"].ToString());

                cmd.Parameters.AddWithValue("@FestivalHoliday", ViewState["__HolidayCount__"].ToString());
                cmd.Parameters.AddWithValue("@AbsentDay", ViewState["__TotalAbsentDays__"].ToString());
                cmd.Parameters.AddWithValue("@PresentDay",dtPresent.Rows.Count.ToString());

                cmd.Parameters.AddWithValue("@EmpPresentSalary", ViewState["__presentSalary__"].ToString());
                cmd.Parameters.AddWithValue("@NetEmpPresentSalary", ViewState["__NetEmpPresentSalary__"].ToString());                
                cmd.Parameters.AddWithValue("@BasicSalary", ViewState["__BasicSalary__"].ToString());
                cmd.Parameters.AddWithValue("@HouseRent", ViewState["__HouseRent__"].ToString());
                cmd.Parameters.AddWithValue("@MedicalAllownce", ViewState["__MedicalAllownce__"].ToString());
                cmd.Parameters.AddWithValue("@ConvenceAllownce", ViewState["__ConvenceAllownce__"].ToString());
                cmd.Parameters.AddWithValue("@FoodAllownce", ViewState["__FoodAllownce__"].ToString());
                cmd.Parameters.AddWithValue("@TechnicalAllowance", dtCertainEmp.Rows[0]["TechnicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@OthersAllownce", dtCertainEmp.Rows[0]["OthersAllownce"].ToString());
                cmd.Parameters.AddWithValue("@LunchAllowance", 0);

                cmd.Parameters.AddWithValue("@AdvanceDeduction",   getAdvanceInstallment);
                cmd.Parameters.AddWithValue("@LoanDeduction",   getLoanInstallment);

                // cmd.Parameters.AddWithValue("@AbsentDeduction", ViewState["__absentFine__"].ToString());

                cmd.Parameters.AddWithValue("@AbsentDeduction", ViewState["__AbsentDeduction__"].ToString());
                cmd.Parameters.AddWithValue("@AttendanceBonus",   getAttendanceBonus);

                cmd.Parameters.AddWithValue("@Payable", getPayable);
                cmd.Parameters.AddWithValue("@TotalOTHour",   ViewState["__getTotalOverTime__"].ToString());

                cmd.Parameters.AddWithValue("@OTRate", OverTimeHourlySalary);
                cmd.Parameters.AddWithValue("@TotalOTAmount", ViewState["__getTotalOvertimeAmt__"].ToString());
                cmd.Parameters.AddWithValue("@NetPayable", getNetPayable);
                cmd.Parameters.AddWithValue("@Stampdeduct", Math.Round(double.Parse(dtStampDeduct.Rows[0]["StampDeduct"].ToString()), 0));
                cmd.Parameters.AddWithValue("@TotalSalary",   getTotalSalary);


                cmd.Parameters.AddWithValue("@DptId", dtCertainEmp.Rows[0]["DptId"].ToString());
                cmd.Parameters.AddWithValue("@DsgId", dtCertainEmp.Rows[0]["DsgId"].ToString());
                cmd.Parameters.AddWithValue("@GrdName", dtCertainEmp.Rows[0]["GrdName"].ToString());
                cmd.Parameters.AddWithValue("@EmpTypeId", dtRunningEmp.Rows[i]["EmpTypeId"].ToString());
                cmd.Parameters.AddWithValue("@EmpStatus", '1');

                cmd.Parameters.AddWithValue("@UserId", userId.ToString());
                cmd.Parameters.AddWithValue("@IsSeperationGeneration", "0");
                cmd.Parameters.AddWithValue("@GenerateDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@OTHourForBuyer", 0);
                cmd.Parameters.AddWithValue("@OTAmountForBuyer",0);
                cmd.Parameters.AddWithValue("@ExtraOTHour", 0);
                cmd.Parameters.AddWithValue("@ExtraOTAmount",0);
                cmd.Parameters.AddWithValue("@NetPayableWithAllOTAmt", getNetPayableWithAllOTAmt);

                cmd.Parameters.AddWithValue("@LateDays", ViewState["__LateDays__"].ToString());
                cmd.Parameters.AddWithValue("@LateFine", ViewState["__LateFine__"].ToString());
                cmd.Parameters.AddWithValue("@LateFineDays", ViewState["__LateFineDays__"].ToString());
                cmd.Parameters.AddWithValue("@TiffinDays", ViewState["__Tiffindays__"].ToString());
                cmd.Parameters.AddWithValue("@TiffinTaka",ViewState["__TiffinTaka__"].ToString());
                cmd.Parameters.AddWithValue("@TiffinBillAmount",ViewState["__TiffinBillAmount__"].ToString());
                cmd.Parameters.AddWithValue("@HolidayWorkingDays", ViewState["__Holidays__"].ToString());
                cmd.Parameters.AddWithValue("@HolidayTaka", ViewState["__HolidayTaka__"].ToString());
                cmd.Parameters.AddWithValue("@HoliDayBillAmount",ViewState["__HolidayBillAmount__"].ToString());
                cmd.Parameters.AddWithValue("@DormitoryRent",dtCertainEmp.Rows[0]["DormitoryRent"].ToString());
                cmd.Parameters.AddWithValue("@ProvidentFund", ViewState["__PFAmount__"].ToString());
                cmd.Parameters.AddWithValue("@TotalOverTime", ViewState["__getOverTime__"].ToString());
                cmd.Parameters.AddWithValue("@TotalOtherOverTime", ViewState["__getOtherOverTime__"].ToString());
                cmd.Parameters.AddWithValue("@OthersPay", dtOtherspay.Rows[0]["OtherPay"].ToString());
                cmd.Parameters.AddWithValue("@OthersDeduction",dtOthersDeduction.Rows[0]["PAmount"].ToString());
                cmd.Parameters.AddWithValue("@ShortLeave",dtShortleave.Rows.Count);
                cmd.Parameters.AddWithValue("@ProfitTax", ViewState["__TaxAmount__"].ToString());
                cmd.Parameters.AddWithValue("@MobileCell", dtMobileCell.Rows[0]["MobileCell"].ToString());
                cmd.Parameters.AddWithValue("@LWP",dtLeaveWithoutPay.Rows.Count);
                cmd.Parameters.AddWithValue("@Adjustment", getTotalAdjustment);
                cmd.Parameters.AddWithValue("@LWPDeduction", getLWPDeduction);
               string sl =cmd.ExecuteScalar().ToString();
              
                   // if (int.Parse(cmd.ExecuteNonQuery().ToString()) > 0)
                    if (int.Parse(sl)> 0)
                    {
                    if (dtAdjustment != null && dtAdjustment.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtAdjustment.Rows)
                        {
                            CRUD.Execute("INSERT INTO [dbo].[Payroll_MonthlySalaryAdjustmentDetails]([SalaryID],[AdjustmentID],[Amount],[Remarks])VALUES("+ sl + ","+ dr["AdjustmentTypeID"] + ",'"+ dr["Amount"] + "','"+ dr["Remarks"] + "')", sqlDB.connection);
                        }
                    }
                    try {
                        SqlCommand cmd1 = new SqlCommand("update VatTax_IncomeTaxDetailsLog set isPaid=1 where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and Month='" + getYear + "-" + getMonth + "-01'", sqlDB.connection);
                        cmd1.ExecuteNonQuery();
                          }
                          catch { }

                        //  if (dtCertainEmp.Rows[0]["PfMember"].ToString().Equals("True"))
                           //   SavePFDetails(getYearMonth);
                         
                    }
              
           
                Advance_And_Loan_StatusChange(pg);  // For advance and loan status change


                //  lbProcessingStatus.Items.Add("Processing completed of "+dtRunningEmp.Rows[i]["EmpType"].ToString() +" "+empName+" Card No. "+dtRunningEmp.Rows[i]["EmpCardNo"].ToString()+""); 

            }
            catch (Exception ex)
            {
                //lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private void SavePFDetails(string YearMonth) 
        {
            try
            {
                
                
                   
                    SqlCommand cmd = new SqlCommand("Delete From PF_CalculationDetails where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and YearMonth='" + YearMonth + "'", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                    DataTable dtPf;
                    sqlDB.fillDataTable("select ClosingBalance from  PF_CalculationDetails where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() +
                        "' and YearMonth=(select max(YearMonth) from  PF_CalculationDetails where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "')", dtPf = new DataTable());
                if(dtPf==null|| dtPf.Rows.Count==0)
                    sqlDB.fillDataTable("Select PfOpeningBalance ClosingBalance from v_Personnel_EmpCurrentStatus "
                        + "where PfMember='1' and EmpStatus in('1','8') and IsActive='1'  and EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' ", dtPf = new DataTable());

                float openingbalance = float.Parse(dtPf.Rows[0]["ClosingBalance"].ToString());
                float closingbalance = openingbalance +( float.Parse(ViewState["__PFAmount__"].ToString())*2);

              

                    
                    string[] getColumns = { "YearMonth", "EmpId", "OpeningBalance", "EmpContributionPer", "EmpContributionAmount", "EmprContributionPer", "EmprContributionAmount", "ClosingBalance" };
                    string[] getValues = { YearMonth, dtCertainEmp.Rows[0]["EmpId"].ToString(), openingbalance.ToString(), dtCertainEmp.Rows[0]["PfEmpContribution"].ToString(), ViewState["__PFAmount__"].ToString(), dtCertainEmp.Rows[0]["PfEmpContribution"].ToString(), ViewState["__PFAmount__"].ToString(), closingbalance.ToString()};

                    SQLOperation.forSaveValue("PF_CalculationDetails", getColumns, getValues, sqlDB.connection);


            }
            catch { }
        }
        double getTotalSalary;
        double getTotalSalaryWithAllOT;
        double getNetPayable;
        double getAdvanceInstallment;
        double getLoanInstallment;
        double getPayable;
        double getNetPayableWithAllOTAmt;
        double getTotalAdjustment;
        double getLWPDeduction;
        private void getNetPayableCalculation(int DaysInMonth, payroll_generation pg)   // net payable calculation
        {
            try
            {

                 getNetPayable = 0;
                 getAdvanceInstallment = 0;
                 getLoanInstallment = 0;
                 getNetPayableWithAllOTAmt = 0;
                 getTotalAdjustment = 0;
                 getLWPDeduction = 0;
                double getPresentSalary = double.Parse( dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString());
                 ViewState["__presentSalary__"] = getPresentSalary.ToString();
                // for advance deduction
                try   
                {
                    if ( dtAdvanceInfo.Rows.Count > 0)
                        if ( dtCutAdvance.Rows.Count > 0)  getAdvanceInstallment = Math.Round(double.Parse( dtCutAdvance.Rows[0]["InstallmentAmount"].ToString()), 0);

                }
                catch { }

                // for loan deduction
                try
                {
                    if ( dtLoanInfo.Rows.Count > 0)
                        if ( dtCutLoan.Rows.Count > 0)  getLoanInstallment = Math.Round(double.Parse( dtCutLoan.Rows[0]["InstallmentAmount"].ToString()), 0);
                }
                catch { }
                if (dtLate.Rows.Count % 2 != 0)
                {

                }
            // float getAbsendDaysFromLate = (dtLate.Rows.Count >= 2) ? dtLate.Rows.Count / 2 : 0;
             float getAbsendDaysFromLate = (float)dtLate.Rows.Count/2 ;
                // -----Start cl/sl and/or salary deduct bellow by nayem at 24-03-2019------
               //   float getHalfDayDeduction = (dtHalfDayDeduct.Rows.Count > 0) ? dtHalfDayDeduct.Rows.Count / 2 : 0;
              //  getAbsendDaysFromLate += getHalfDayDeduction;

                if (getAbsendDaysFromLate >0)// 2 days late for deduct 1 cl/sl . 
                {                  
                    float restAbsendDaysFromLate = 0;
                    if (getAbsendDaysFromLate > 2)// maximum 2 days leave deduct in one month 
                    {
                        restAbsendDaysFromLate = getAbsendDaysFromLate - 2;
                        restAbsendDaysFromLate += deductLeaveAndReturnRestDays(2);
                    }
                    else
                    {
                        restAbsendDaysFromLate= deductLeaveAndReturnRestDays(getAbsendDaysFromLate);
                    }
                    getAbsendDaysFromLate = restAbsendDaysFromLate;
                }               
               
                    

                //find late days
                ViewState["__LateDays__"] = dtLate.Rows.Count;
                ViewState["__LateFineDays__"] = getAbsendDaysFromLate;

                //ViewState["__PayableDays__"] = (int.Parse(ViewState["__PayableDays__"].ToString()) - getAbsendDaysFromLate);

               // getAbsendDaysFromLate += dtAbsent.Rows.Count;
                //find absent days
                ViewState["__TotalAbsentDays__"] = dtAbsent.Rows.Count;

                double lateDeduction = Math.Round(double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString()) / 30 * getAbsendDaysFromLate, 2);
                ViewState["__LateFine__"] = lateDeduction;
               // ViewState["__LateFine__"] = "0";
                // find Absent deduction
                double getAbsentAmount = Math.Round(double.Parse(dtCertainEmp.Rows[0]["BasicSalary"].ToString()) / 30 * dtAbsent.Rows.Count, 2);

                // getLWPDeduction
                if(dtLeaveWithoutPay.Rows.Count>0)
                getLWPDeduction = double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString()) / 30 * dtLeaveWithoutPay.Rows.Count;
                


                //get one day salary 
                double onDaySalary = (1 * double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString())) / DaysInMonth;

                ViewState["__AbsentDeduction__"] = getAbsentAmount.ToString();

                //  ViewState["__PFDeduction"] = dtPFDeduction.Rows[0]["PFAmount"].ToString();

                getPresentSalary = onDaySalary * int.Parse(ViewState["__TotalActiveDays__"].ToString()); //double.Parse(dtCertainEmp.Rows[0]["EmpPresentSalary"].ToString()); //onDaySalary * double.Parse(ViewState["__PayableDays__"].ToString());
                if (getPresentSalary < double.Parse(ViewState["__presentSalary__"].ToString()))// for new employee allowance calculation
                {
                   DataTable dt_AllowanceSettings = new DataTable();
                    sqlDB.fillDataTable("Select acs.SalaryType ,has.BasicAllowance,has.MedicalAllownce,has.FoodAllownce,has.ConvenceAllownce,has.TechnicalAllowance, " +
                        " has.HouseRent,has.OthersAllowance,has.PFAllowance,has.AlCalId,acs.EmpTypeId," +
                        "acs.BasicAllowance as BasicStatus,acs.MedicalAllownce as MedicalStatus,acs.FoodAllownce as FoodStatus,acs.ConvenceAllownce as ConStatus," +
                        " acs.TechnicalAllowance as TecStatus,acs.HouseRent as HouseStatus,acs.OthersAllowance as OthStatus, acs.ProvidentFund as PFStatus " +
                        " from HRD_AllownceSetting as has inner join Payroll_AllowanceCalculationSetting acs on has.AlCalId=acs.AlCalId where acs.CalculationType='salary'", dt_AllowanceSettings);
                    ViewState["__NetEmpPresentSalary__"] = getPresentSalary;

                    double basic = getPresentSalary * (double.Parse(dt_AllowanceSettings.Rows[0]["BasicAllowance"].ToString()) / 100);
                    double houseRent = basic * (double.Parse(dt_AllowanceSettings.Rows[0]["HouseRent"].ToString()) / 100);
                    double medical = basic * (double.Parse(dt_AllowanceSettings.Rows[0]["MedicalAllownce"].ToString()) / 100);
                    double convence = basic * (double.Parse(dt_AllowanceSettings.Rows[0]["ConvenceAllownce"].ToString()) / 100);
                    double food = basic * (double.Parse(dt_AllowanceSettings.Rows[0]["FoodAllownce"].ToString()) / 100);


                    ViewState["__BasicSalary__"] = basic.ToString();
                    ViewState["__HouseRent__"] = houseRent.ToString();
                    ViewState["__MedicalAllownce__"] = medical.ToString();
                    ViewState["__ConvenceAllownce__"] = convence.ToString();
                    ViewState["__FoodAllownce__"] = food.ToString();
                }
                else
                {
                    ViewState["__NetEmpPresentSalary__"] = ViewState["__presentSalary__"].ToString();
                    ViewState["__BasicSalary__"] = dtCertainEmp.Rows[0]["BasicSalary"].ToString();
                    ViewState["__HouseRent__"] = dtCertainEmp.Rows[0]["HouseRent"].ToString();
                    ViewState["__MedicalAllownce__"] = dtCertainEmp.Rows[0]["MedicalAllownce"].ToString();
                    ViewState["__ConvenceAllownce__"] = dtCertainEmp.Rows[0]["ConvenceAllownce"].ToString();
                    ViewState["__FoodAllownce__"] = dtCertainEmp.Rows[0]["FoodAllownce"].ToString();
                }
                
                if (dtAdjustment != null && dtAdjustment.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAdjustment.Rows)
                    {
                        getTotalAdjustment += float.Parse(dr["Amount"].ToString());
                    }
                    getTotalAdjustment = Math.Round(getTotalAdjustment,2);
                }
                 getPayable = 0;
                 getPayable = Math.Round(((Math.Round( getPresentSalary+ double.Parse(ViewState["__PFAmount__"].ToString()),2) + double.Parse(dtOtherspay.Rows[0]["OtherPay"].ToString())) - (getAbsentAmount + lateDeduction + double.Parse(ViewState["__TaxAmount__"].ToString()) + Math.Round( double.Parse(ViewState["__PFAmount__"].ToString())+ double.Parse(ViewState["__PFAmount__"].ToString()),2) + getAdvanceInstallment + getLoanInstallment + double.Parse(dtOthersDeduction.Rows[0]["PAmount"].ToString()) + double.Parse(dtCertainEmp.Rows[0]["DormitoryRent"].ToString())+ getLWPDeduction)),2);
               
                // ViewState["__PayableDays__"];
                //-------------------------------------------------------------------
               // get total salary 
               

                 //ViewState[""] = (getPresentSalary -  getPayable).ToString();  // this line for get total deduction 

               // getAbsentAmount = getDays - float.Parse( ViewState["__PayableDays__"].ToString());  // now absent dayes enetrd in getabsentAmount

                // ViewState["__absentFine__"] = (getAbsentAmount * onDaySalary) + double.Parse( ViewState["__absentFine__"].ToString());

                //---------------------------------------------------------------------------
                 
                     double totalovertimeamt = double.Parse(ViewState["__getTotalOvertimeAmt__"].ToString());
                     getNetPayable = getPayable + totalovertimeamt + getAttendanceBonus + double.Parse(ViewState["__TiffinBillAmount__"].ToString()) + double.Parse(ViewState["__HolidayBillAmount__"].ToString());
             
                 getNetPayable = Math.Round(getNetPayable, 2);

               
                // to get finaly payble amount
                 getTotalSalary = Math.Round(( getNetPayable+getTotalAdjustment -(int.Parse(dtMobileCell.Rows[0]["MobileCell"].ToString())+ double.Parse( dtStampDeduct.Rows[0]["StampDeduct"].ToString()))),2);

            }
            catch (Exception ex)
            {
                // lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        private float getLeaveBalance(string LeaveName)
        {
            try
            {
                float balance = 0;
                DataTable dtLv;
                DateTime empJoiningDate = DateTime.Parse(ViewState["__EmpJoiningDate__"].ToString());
                string year = ViewState["__year__"].ToString();
                if (empJoiningDate.Year == int.Parse(year))
                    sqlCmd = "select CL as LeaveDays,'c/l' as  ShortName from Leave_LeaveBalanceForNewEmp where MOnthID='" + empJoiningDate.ToString("MM") + "'";
                else
                    sqlCmd = "select LeaveDays,ShortName from tblLeaveConfig where ShortName='" + LeaveName + "'";
                    sqlDB.fillDataTable(sqlCmd, dtLv = new DataTable());
                byte getLeaveDays = byte.Parse(dtLv.Rows[0]["LeaveDays"].ToString());
                sqlCmd = "select ISNULL( sum( case when isnull(IsHalfDayLeave,0)=0 then 1 else 0.5  end ),0) as Day from v_Leave_LeaveApplicationDetails where EmpId='" + dtCertainEmp.Rows[0]["EmpID"].ToString() + "' And ShortName='" + LeaveName + "' AND FromYear='" + ViewState["__year__"].ToString() + "' ";
                sqlDB.fillDataTable(sqlCmd, dtLv = new DataTable());
                float UsedLeaveDays=float.Parse(dtLv.Rows[0]["Day"].ToString());
                sqlCmd = "select ISNULL( sum(ISNULL(Days,0)),0) Days from Leave_MonthlyLeaveDeductionRecord where  EmpID='" + dtCertainEmp.Rows[0]["EmpID"].ToString() + "' and LeaveType='" + LeaveName + "' and year(Month)='" + ViewState["__year__"].ToString() + "'";
                sqlDB.fillDataTable(sqlCmd, dtLv = new DataTable());
                float DeductedLeaveDays = float.Parse( dtLv.Rows[0]["Days"].ToString());
                balance = getLeaveDays - (UsedLeaveDays + DeductedLeaveDays);
                if (balance < 0)
                    balance = 0;
                return balance;

            }
            catch { return 0; }
        }
        private bool leaveDeductionPermission()
        {
            try {
                DataTable dt = new DataTable();
                string[] Month = ViewState["__month__"].ToString().Split('-');
                string SqlCmd = "select EmpID from Leave_MonthlyLeaveDeductionSettings Where Month='" + Month[0]+"-"+ Month[1]+"-01' and EmpID='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "'";
                sqlDB.fillDataTable(SqlCmd, dt);
                if (dt == null || dt.Rows.Count == 0)
                    return true;
                return false;
            } catch { return true; }
            
        }
        private float deductLeaveAndReturnRestDays(float LateDays)
        {
            try
            {
                if (!leaveDeductionPermission())
                    return LateDays;
                float restLate = 0;
                //string[] LeaveType = { "c/l"};
                // foreach (string item in LeaveType)
                // {
               
                float balance= getLeaveBalance("c/l");
                //if (balance == 0)
                //    continue;
                if (balance > 0)
                {
                    while (true)
                    {
                        if (LateDays <= balance)
                        {
                            // deduct Leave

                            string sqlcmd = @"INSERT INTO [dbo].[Leave_MonthlyLeaveDeductionRecord]([EmpID],[Month],[LeaveType],[Days],[Note])" +
                                " VALUES('" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "','" + ViewState["__month__"].ToString() + "','c/l'," + LateDays + ",'for Late')";
                            CRUD.Execute(sqlcmd, sqlDB.connection);
                            break;
                        }
                        else
                        {
                            LateDays -= 0.5f;
                            restLate += 0.5f;
                        }
                    }
                }
                else
                    restLate = LateDays;

                //    LateDays = restLate;
                //    restLate = 0;
                //}
                return restLate;
            }
            catch { return 0; }
        }
      
        
        private  void PayableDaysCalculation(string MonthName, string SelectedDay, string CompanyId, payroll_generation pg)    // For Runing employee
        {
            try
            {
                DataTable dt;              
                string[] m = MonthName.Split('-');
                MonthName = m[1] + "-" + m[0];
                sqlDB.fillDataTable("select distinct format(ATTDate,'yyyy-MM-dd') as WeekendDate from tblAttendanceRecord where CompanyId='" + CompanyId + "' and EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTStatus='W' and format(ATTDate,'yyyy-MM-dd')>='"+ MonthName + "-01' and format(ATTDate,'yyyy-MM-dd')<='"+ MonthName + "-"+ SelectedDay + "'", dt = new DataTable());
                DataTable dtTemp = new DataTable();
                dtTemp.Columns.Add("AttDate", typeof(string));
                DataTable dtWC = new DataTable();
                DataTable dtcheck;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int count = 0;
                    DateTime d1 = Convert.ToDateTime(dt.Rows[i]["WeekendDate"].ToString()).AddDays(-1);
                    DateTime d2 = Convert.ToDateTime(dt.Rows[i]["WeekendDate"].ToString()).AddDays(1);
                    
                    while (true)
                    {
                        sqlDB.fillDataTable("select distinct format(ATTDate,'yyyy-MM-dd') as HDate from tblAttendanceRecord where CompanyId='" + CompanyId + "' and EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTStatus in('H','W') and format(ATTDate,'yyyy-MM-dd')='" + d1.ToString("yyyy-MM-dd") + "'", dtcheck = new DataTable());
                        if (dtcheck.Rows.Count == 0)
                            break;
                        d1 = d1.AddDays(-1);
                    }
                    //  string date1 = Convert.ToDateTime(dtHoliday.Rows[i]["HDate"].ToString()).AddDays(-1).ToString("yyyy/MM/dd");                    
                    string date1 = d1.ToString("yyyy/MM/dd");
                    while (true)
                    {
                        sqlDB.fillDataTable("select distinct format(ATTDate,'yyyy-MM-dd') as HDate from tblAttendanceRecord where CompanyId='" + CompanyId + "' and EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTStatus in('H','W') and format(ATTDate,'yyyy-MM-dd')='" + d2.ToString("yyyy-MM-dd") + "'", dtcheck = new DataTable());
                        if (dtcheck.Rows.Count == 0)
                            break;
                        d2 = d2.AddDays(1);
                    }
                    string date2 = d2.ToString("yyyy/MM/dd");
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

                        //sqlDB.fillDataTable("SELECT EmpId FROM tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTDate='" + Convert.ToDateTime(dt.Rows[i]["WeekendDate"].ToString()).ToString("yyyy-MM-dd") + "' and AttStatus='A' ", dtWC);
                        //if (dtWC.Rows.Count == 0)
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
              //  sqlDB.fillDataTable("select * from tblHolydayWork where CompanyId='" + CompanyId + "' AND HDate >='" + monthYear + "-01' AND HDate <='" + monthYear + '-' + SelectedDay + "' AND HDate not in (select AttDate from tblAttendanceRecord where EmpId='" +   dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv') ", dtHoliday);
               // sqlDB.fillDataTable("select * from tblHolydayWork where CompanyId='" + CompanyId + "' AND HDate >='" + monthYear + "-01' AND HDate <='" + monthYear + '-' + SelectedDay + "' AND HDate not in (select AttDate from tblAttendanceRecord where EmpId='" +   dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv') ", dtHoliday);
                sqlDB.fillDataTable("select distinct format(ATTDate,'yyyy-MM-dd') as HDate from tblAttendanceRecord where CompanyId='" + CompanyId + "' and EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTStatus='H' and format(ATTDate,'yyyy-MM-dd')>='" + MonthName + "-01' and format(ATTDate,'yyyy-MM-dd')<='" + MonthName + "-" + SelectedDay + "'", dtHoliday);
                dtTemp = new DataTable();
                dtTemp.Columns.Add("AttDate", typeof(string));               
                for (int i = 0; i < dtHoliday.Rows.Count; i++)
                {
                    int count = 0;
                    DateTime d1 = Convert.ToDateTime(dtHoliday.Rows[i]["HDate"].ToString()).AddDays(-1);
                    DateTime d2 = Convert.ToDateTime(dtHoliday.Rows[i]["HDate"].ToString()).AddDays(1);
                    while (true)
                    {
                        sqlDB.fillDataTable("select distinct format(ATTDate,'yyyy-MM-dd') as HDate from tblAttendanceRecord where CompanyId='" + CompanyId + "' and EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTStatus in('H','W') and format(ATTDate,'yyyy-MM-dd')='" + d1.ToString("yyyy-MM-dd") + "'", dtcheck=new DataTable());
                        if (dtcheck.Rows.Count== 0)
                            break;
                        d1 = d1.AddDays(-1);                       
                    }
                  //  string date1 = Convert.ToDateTime(dtHoliday.Rows[i]["HDate"].ToString()).AddDays(-1).ToString("yyyy/MM/dd");                    
                    string date1 = d1.ToString("yyyy/MM/dd");
                    while (true)
                    {
                        sqlDB.fillDataTable("select distinct format(ATTDate,'yyyy-MM-dd') as HDate from tblAttendanceRecord where CompanyId='" + CompanyId + "' and EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTStatus in('H','W') and format(ATTDate,'yyyy-MM-dd')='" + d2.ToString("yyyy-MM-dd") + "'", dtcheck = new DataTable());
                        if (dtcheck.Rows.Count == 0)
                            break;
                        d2 = d2.AddDays(1);
                    }
                    string date2 = d2.ToString("yyyy/MM/dd");
                   // string date2 = Convert.ToDateTime(dtHoliday.Rows[i]["HDate"].ToString()).AddDays(1).ToString("yyyy/MM/dd");
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
                        //dtWC = new DataTable();
                        //sqlDB.fillDataTable("SELECT EmpId FROM tblAttendanceRecord where EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTDate='" + Convert.ToDateTime(dtHoliday.Rows[i]["HDate"].ToString()).ToString("yyyy-MM-dd") + "' and AttStatus='A' ", dtWC);
                        //if (dtWC.Rows.Count == 0)
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
              //  float PayableDays = float.Parse(dt.Rows.Count.ToString()) + float.Parse(ViewState["__cl__"].ToString()) + float.Parse(  ViewState["__sl__"].ToString()) + float.Parse(  ViewState["__al__"].ToString()) +dtPresent.Rows.Count+ float.Parse(dtHoliday.Rows.Count.ToString());
                float PayableDays = float.Parse(dt.Rows.Count.ToString()) + dtLeaveInfo.Rows.Count + dtPresent.Rows.Count+ float.Parse(dtHoliday.Rows.Count.ToString());
               ViewState["__PayableDays__"] = PayableDays.ToString();
                ViewState["__TotalActiveDays__"] = SelectedDay;
            }
            catch { }

        }

        private  void getPayableDaysCalculationForML(string MonthName, string SelectedDay, payroll_generation pg)
        {
            try
            {
                DataTable dt;
                string monthYear = MonthName.Substring(3, 4) + "-" + MonthName.Substring(0, 2);
                if (  dtPresent.Rows.Count == 0)
                {
                      ViewState["__WeekendCount__"] = "0";
                      ViewState["__HolidayCount__"] = "0";

                }
                else
                {
                    sqlDB.fillDataTable("select * from Attendance_WeekendInfo where  MonthName='" + MonthName + "' And WeekendDate >='" +   dtPresent.Rows[0]["ATTDate"].ToString() + "' AND WeekendDate <='" +   dtPresent.Rows[  dtPresent.Rows.Count - 1]["ATTDate"].ToString() + "'", dt = new DataTable());
                      ViewState["__WeekendCount__"] = dt.Rows.Count.ToString();

                    DataTable dtHoliday = new DataTable();
                    sqlDB.fillDataTable("select * from tblHolydayWork where HDate >='" +   dtPresent.Rows[0]["ATTDate"].ToString() + "' AND HDate <='" +   dtPresent.Rows[  dtPresent.Rows.Count - 1]["ATTDate"].ToString() + "'", dtHoliday);
                      ViewState["__HolidayCount__"] = dtHoliday.Rows.Count.ToString();
                }


                  ViewState["__PayableDays__"] = "0";
                //int PayableDays = int.Parse(  ViewState["__WeekendCount__"].ToString()) + int.Parse(  ViewState["__cl__"].ToString()) + int.Parse(  ViewState["__sl__"].ToString()) + int.Parse(  ViewState["__al__"].ToString()) + int.Parse(  ViewState["__ofl__"].ToString()) + int.Parse(  ViewState["__othl__"].ToString()) +   dtPresent.Rows.Count + int.Parse(  ViewState["__HolidayCount__"].ToString()) +   dtAbsent.Rows.Count;
                  float PayableDays = float.Parse(ViewState["__WeekendCount__"].ToString()) + float.Parse(ViewState["__cl__"].ToString()) + float.Parse(ViewState["__sl__"].ToString()) + float.Parse(ViewState["__al__"].ToString()) + float.Parse(ViewState["__ofl__"].ToString()) + float.Parse(ViewState["__othl__"].ToString()) + float.Parse(ViewState["__ml__"].ToString()) + float.Parse(dtPresent.Rows.Count.ToString()) + float.Parse(ViewState["__HolidayCount__"].ToString()) + float.Parse(dtAbsent.Rows.Count.ToString());
                  ViewState["__PayableDays__"] = PayableDays.ToString();
            }
            catch { }
        }

        private  bool joiningMonthIsEqual(string getMonth, string getYear, string CompanyId, string selectdates, payroll_generation pg,int DaysinMonth)   //net payable calculation,compier joining time for generate salary sheet of month
        {
            try
            {
                string[] getJoiningMonth = ViewState["__getJoingingDate__"].ToString().Split('-');

                string getJoinMonth = getJoiningMonth[1] + "-" + getJoiningMonth[2];

                string getCurrentMonth = getMonth + "-" + getYear;

                string[] selectDates = selectdates.Trim().Split('-');

                string selectedDay = selectDates[0];
                string joiningDay = getJoiningMonth[0];
                // below option for checking joining date-month-year is equal of current month 
                if (getJoinMonth.Equals(getCurrentMonth) && int.Parse(getJoiningMonth[0].ToString()) != 1)
                {
                    DataTable dt;
                string MonthName = getYear + "-" + getMonth;
                    //sqlDB.fillDataTable("select distinct format(WeekendDate,'yyyy-MM-dd') as WeekendDate from v_Attendance_WeekendInfo where CompanyId='" + CompanyId + "'  AND  MonthName='" + MonthName + "' And WeekendDate >='" + monthYear + "-01' AND WeekendDate <='" + monthYear + '-' + SelectedDay + "' AND WeekendDate not in (select AttDate from tblAttendanceRecord where EmpId='" +   dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv')", dt = new DataTable());
                    // sqlDB.fillDataTable("select distinct format(ATTDate,'yyyy-MM-dd') as WeekendDate from tblAttendanceRecord where CompanyId='" + CompanyId + "'  AND  RIGHT(CONVERT(VARCHAR(10),ATTDate, 105), 7)='" + MonthName + "' and EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTStatus='W' ", dt = new DataTable());
                    sqlDB.fillDataTable("select distinct format(ATTDate,'yyyy-MM-dd') as WeekendDate from tblAttendanceRecord where CompanyId='" + CompanyId + "' and EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTStatus='W' and format(ATTDate,'yyyy-MM-dd')>='" + MonthName + "-"+ joiningDay + "' and format(ATTDate,'yyyy-MM-dd')<='" + MonthName + "-" + selectedDay + "'", dt = new DataTable());
                    ViewState["__WeekendCount__"] = dt.Rows.Count.ToString();
                    DataTable dtHoliday = new DataTable();
                    //  sqlDB.fillDataTable("select * from tblHolydayWork where CompanyId='" + CompanyId + "' AND HDate >='" + monthYear + "-01' AND HDate <='" + monthYear + '-' + SelectedDay + "' AND HDate not in (select AttDate from tblAttendanceRecord where EmpId='" +   dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv') ", dtHoliday);
                    // sqlDB.fillDataTable("select * from tblHolydayWork where CompanyId='" + CompanyId + "' AND HDate >='" + monthYear + "-01' AND HDate <='" + monthYear + '-' + SelectedDay + "' AND HDate not in (select AttDate from tblAttendanceRecord where EmpId='" +   dtCertainEmp.Rows[0]["EmpId"].ToString() + "' AND AttStatus='lv') ", dtHoliday);
                    sqlDB.fillDataTable("select distinct format(ATTDate,'yyyy-MM-dd') as WeekendDate from tblAttendanceRecord where CompanyId='" + CompanyId + "' and EmpId='" + dtCertainEmp.Rows[0]["EmpId"].ToString() + "' and ATTStatus='H' and format(ATTDate,'yyyy-MM-dd')>='" + MonthName + "-"+ joiningDay + "' and format(ATTDate,'yyyy-MM-dd')<='" + MonthName + "-" + selectedDay + "'", dtHoliday);
                    ViewState["__HolidayCount__"] = dtHoliday.Rows.Count.ToString();
                    ViewState["__PayableDays__"] = "0";
                  //  float PayableDays = float.Parse(dt.Rows.Count.ToString()) + float.Parse(ViewState["__cl__"].ToString()) + float.Parse(ViewState["__sl__"].ToString()) + float.Parse(ViewState["__al__"].ToString()) + dtPresent.Rows.Count + float.Parse(dtHoliday.Rows.Count.ToString());
                    float PayableDays = float.Parse(dt.Rows.Count.ToString()) + dtLeaveInfo.Rows.Count + dtPresent.Rows.Count + float.Parse(dtHoliday.Rows.Count.ToString());
                    ViewState["__PayableDays__"] = PayableDays.ToString();
                    ViewState["__TotalActiveDays__"] = (int.Parse(selectedDay) - int.Parse(joiningDay)) + 1;
                    getNetPayableCalculation(DaysinMonth,pg);
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private  void Advance_And_Loan_StatusChange(payroll_generation pg)
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

        private void getHourlyAmount(int DaysInMonth,double Salary)
        {
            try
            {
               // getOndaySalary = Math.Round(GrossSalary / DaysInMonth, 0);
               // NormalHourlySalary = Math.Round(getOndaySalary/8);
                OverTimeHourlySalary = Math.Round(Salary / 208*2, 2); // here 208 is static.
            }
            catch { }
        }

        public  DataTable dtGetMonthSetup;
        private  void loadMonthSetup(string days, string month, string year, string CompanyId)
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
        private void checkForAttendanceBonus(string month,string year,string EmpId)   // check attendance bonus
        {
            try
            {
                DataTable dtApplydate = new DataTable();
                // get applydate information of a certain employee
                sqlDB.fillDataTable("SELECT EmpId,Convert(varchar(7),Lv_date,126) FROM Leave_ApplyDate where EmpId='" + EmpId + "' and Convert(varchar(7),Lv_date,126)='" + year + "-" + month + "'", dtApplydate);

                //if (int.Parse(dtGetMonthSetup.Rows[0]["TotalWorkingDays"].ToString()) == dtPresent.Rows.Count)
                //{
                if (dtLate.Rows.Count >= 1 || dtApplydate.Rows.Count > 0 || dtAbsent.Rows.Count >= 1 || int.Parse(dtGetMonthSetup.Rows[0]["TotalDays"].ToString()) != int.Parse(ViewState["__PayableDays__"].ToString()) || dtLeaveWithoutPay.Rows.Count>0) getAttendanceBonus = 0;
                else getAttendanceBonus = int.Parse(dtCertainEmp.Rows[0]["AttendanceBonus"].ToString());
                //}
                //else getAttendanceBonus = 0;
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private  void salarySheetClearByMonthYear(string month, string year, string CompanyId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("delete from Payroll_MonthlySalarySheet where CompanyId='" + CompanyId + "'  AND Year(YearMonth)='" + year + "' AND Month(YearMonth)='" + month + "' AND EmpStatus in ('1','8') AND IsSeperationGeneration='0'", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }
        private void salarySheetClearByMonthYear(string month, string year, string CompanyId,string empcardno)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("delete from Payroll_MonthlySalarySheet where CompanyId='" + CompanyId + "'  AND Year(YearMonth)='" + year + "' AND Month(YearMonth)='" + month + "' AND EmpStatus in ('1','8') AND EmpCardNo LIKE '%" + empcardno + "' AND IsSeperationGeneration='0'", sqlDB.connection);
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
                    DataRow[] dr6 = dtLeaveInfo.Select("StateStatus ='Compensation/Replace Leave'");
                    DataRow[] dr7 = dtLeaveInfo.Select("StateStatus ='Foreign Employees Leave'");
                    ViewState["__othl__"] = dr5.Length+ dr6.Length + dr7.Length;

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
            if(rbGenaratingType.SelectedValue=="0")
            {
                ddlEmpCardNo.Enabled = false;
                txtEmpCardNo.Enabled = false;
            }
            else
            {
                ddlEmpCardNo.Enabled = true;
                txtEmpCardNo.Enabled = true;
            }


            ViewState["___IsGerments__"] = classes.Payroll.Office_IsGarments();
            if (ViewState["___IsGerments__"].ToString().Equals("False"))
            {
                if (ddlCompanyList.SelectedValue == "0001")
                   txtNotTiffinCardno.Text = "";
                else
                    txtNotTiffinCardno.Text = "0069,0037";
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }
        private void removeTiffinCount(string Month)
        {
            try 
            {
               
                if (txtNotTiffinCardno.Text.Trim() != "")
                {
                    SqlCommand cmd = new SqlCommand("update tblAttendanceRecord set TiffinCount=0  where EmpId in(select EmpId from v_EmployeeDetails where substring(EmpCardNo,10,10) in(" + txtNotTiffinCardno.Text + ") and CompanyId='" + ddlCompanyList.SelectedValue + "') and format(ATTDate,'MM-yyyy')='" + Month + "' ", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        private void SaveProvidentFundDetails(string Month)
        {
            try
            {

                if (txtNotTiffinCardno.Text.Trim() != "")
                {
                    SqlCommand cmd = new SqlCommand("update tblAttendanceRecord set TiffinCount=0  where EmpId in(select EmpId from v_EmployeeDetails where substring(EmpCardNo,10,10) in(" + txtNotTiffinCardno.Text + ") and CompanyId='" + ddlCompanyList.SelectedValue + "') and format(ATTDate,'MM-yyyy')='" + Month + "' ", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }


       
    }


}