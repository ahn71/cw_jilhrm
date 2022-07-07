using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using ComplexScriptingSystem;
using adviitRuntimeScripting;
using CrystalDecisions.Shared;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;
using CrystalDecisions.Web;

namespace SigmaERP.All_Report
{
    public partial class Repor : System.Web.UI.Page
    {
        DataTable dt;
        ReportDocument rpd;
        protected void Page_Init(object sender, EventArgs e)
        {
            this.Init += new System.EventHandler(this.Page_Init);
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                string[] query = Request.QueryString["for"].ToString().Split('-');

                //----------------For payroll reports------------------------

                if (query[0].Equals("WorkerSalarySheet")) loadWorkerSalarySheet(query[1], query[2]);
                else if (query[0].Equals("StaffSalarySheet")) loadStaffSalasySheet(query[1], query[2]);
                else if (query[0].Equals("EmployeeProfile")) LoadEmployeeProfile(query[1]);
                else if (query[0].Equals("AppoinmentLetter")) LoadAppoinmentLetter(query[1]);
                else if (query[0].Equals("ResignationLetter")) LoadResignationLetter();
                else if (query[0].Equals("AppoinmentLetterStaff")) LoadAppoinmentLetterStaff(query[1], query[2]);
                else if (query[0].Equals("PromotionLetterStaff")) PromotionLetterStaff(query[1], query[2] + "-" + query[3]);
                else if (query[0].Equals("PromotionLetterWorker")) PromotionLetterWorker(query[1], query[2] + "-" + query[3]);
                else if (query[0].Equals("PromotionSheet")) PromotionSheet(query[1]);
                else if (query[0].Equals("PreviousPromotionSheet")) PreviousPromotionSheet(query[1]);
                else if (query[0].Equals("IncrementSheet")) IncrementSheet(query[1]);
                else if (query[0].Equals("IndivisualIncrementSheet")) IndivisualIncrementSheet();
                else if (query[0].Equals("IncrementSheetDetails")) IncrementSheetDetails(query[1]);
                else if (query[0].Equals("IndIncrementSheetDetails")) IndIncrementSheetDetails();

                else if (query[0].Equals("PriviousIncrementSheet")) PriviousIncrementSheet(query[1]);
                else if (query[0].Equals("PaySlip")) loadPaySlip(query[1], query[2]);

                else if (query[0].Equals("BonusSheet")) loadBonusSheet(query[1]);
                else if (query[0].Equals("BonusMissSheet")) loadBonusMissSheet(query[1] + "-" + query[2] + "-" + query[3]);
                else if (query[0].Equals("SalarySheet")) loadSalarySheet(query[1], query[2], query[3], query[4], query[5]);                
                else if (query[0].Equals("SalarySheetBankForwardingLetter"))loadSalarySheetBankForwardingLetter();
                else if (query[0].Equals("BonusSheetBankForwardingLetter"))loadBonusSheetBankForwardingLetter();
                else if (query[0].Equals("PrintAdvance")) PrintAdvance();
                else if (query[0].Equals("PrintMobileCell")) PrintMobileCell();
               

                else if (query[0].Equals("SummaryOfBonus")) loadSummaryOfBonus(query[1] + "-" + query[2] + "-" + query[3]);
                else if (query[0].Equals("SummaryOfSalary")) loadSummaryOfSalary(query[1], query[2]);

                else if (query[0].Equals("PaySlip2")) loadPaySlip2();
                else if (query[0].Equals("IncrementLetterStaff"))
                {
                    if (query[4] == "")
                    {
                        IncrementLetterStaff(query[1], query[2], query[3], query[4], query[5]);
                    }
                    else
                    {
                        IncrementLetterStaff(query[1], query[2], query[3], query[4] + "-" + query[5], query[6]);
                    }
                }
                else if (query[0].Equals("RejoinSheet")) ShowRejoinSheet(query[1]);
                else if (query[0].Equals("IncrementLetterWorker")) IncrementLetterWorker(query[1]);
                else if (query[0].Equals("WorkerIDCard")) PrintWorkerIDCard();
                else if (query[0].Equals("StaffIDCard")) PrintStaffIDCard();
                else if (query[0].Equals("StaffOfficerIDCard")) PrintStaffOfficerIDCard();
                else if (query[0].Equals("DailyAbsentReportByLine")) loadDailyAbsentReport();
                else if (query[0].Equals("DailyAttendanceReportByLine")) loadDailyAttendanceReport();
                else if (query[0].Equals("DailyLateReportByLine")) loadDailyLateReportByLine();
                else if (query[0].Equals("DailyOutTimeMissingByLine")) loadDailyOutTimeMissReportByLine();
                else if (query[0].Equals("DailyOverTimeReportByLine")) loadOverTimeReport();
                else if (query[0].Equals("MonthlyOverTime")) loadMonthlyOverTimeReport(query[1]);

                else if (query[0].Equals("FinalBillForm")) ShowFinalBillForm(query[4], query[5], query[6], float.Parse(query[7]), query[8], query[9]);

                else if (query[0].Equals("WorkerSeperationSheet")) loadWorkerSeperationSheet(query[1], query[2]);

                else if (query[0].Equals("AdvanceInfo")) loadAdvanceInfo();
                else if (query[0].Equals("LoanInfo")) loadLoanInfo();
                else if (query[0].Equals("CurrentSalaryStructure")) ShowCurrentSalaryStructure(query[1]);
                else if (query[0].Equals("WorkerBonusSheet")) loadWorkerBonusSheet(query[2], query[1], query[3], query[4]);

                else if (query[0].Equals("NoteGenerate")) ShowNoteGenerate(query[1] + "-" + query[2]);
                else if (query[0].Equals("SalaryFlow")) SalaryFlow(query[1]);
                //---------------------- Vat Tax reports-----------------------------
                else if (query[0].Equals("IndivisualTaxInfo")) loadIndividualTaxReport();
                else if (query[0].Equals("IndivisualIncomeInfo")) loadIndividualIncomeReport();
                else if (query[0].Equals("IndividualInvestment")) loadIndividualInvestment();
                else if (query[0].Equals("IncomeTaxSheet")) loadTaxInfoSheet();
                //----------------------PF Report--------------------------------------
                else if (query[0].Equals("MonthlyPFSheet")) loadMonthlyPFSheet();
                //--------------------For leave reports------------------------------
                else if (query[0].Equals("LeaveConfig")) loadLeaveConfig();
              //  else if (query[0].Equals("LeaveApplication")) loadLeaveApplicationReport();
                else if (query[0].Equals("LeaveApplication")) loadLeaveApplicationReportForSG();
                else if (query[0].Equals("ShortLeaveApplication")) loadshortLeaveApplicationReportForSG();

                else if (query[0].Equals("JobCardReportActual")) loadJobCardReportActual();
                else if (query[0].Equals("HolidayAndWeekendStatus")) MonthlyHolidayAndWeekendStatus();
                else if (query[0].Equals("JobCardReport")) loadJobCardReport();
                else if (query[0].Equals("LeaveBalanceReport")) loadLeaveBalanceReport();
                else if (query[0].Equals("LeaveYearlySummary")) LeaveYearlySummary(query[1]);
                else if (query[0].Equals("LeaveYearlySummaryIndividualDetails")) LeaveYearlySummaryIndividualDetails(query[1]);
                else if (query[0].Equals("CompanyPurposeLeaveReport")) loadComapanyPurposeaLeaveReport();
                else if (query[0].Equals("YearlyLeaveStatus")) loadYearlyLeaveStatus(query[1]);
                else if (query[0].Equals("MonthlyLeaveStatus")) loadMonthlyLeaveStatus(query[1]);
                else if (query[0].Equals("LeaveRegister")) ShowLeaveRegister();
                else if (query[0].Equals("MaternityLeaveApplication")) ShowMaternityLeaveApplication();

                else if (query[0].Equals("mlvoucher")) loadVoucherInfo();
                else if (query[0].Equals("mlvoucherdetails")) loadVoucherDetails();

                else if (query[0].Equals("LeaveStatusSummary")) ShowLeaveStatusSummary(query[1]);

                else if (query[0].Equals("EarnLeave")) ShowEarnLeaveGeneration(query[1]);

                else if (query[0].Equals("DoctorCertificationLetter")) ShowDoctorCertificationLetter();
                else if (query[0].Equals("Grantedbythedoctor")) ShowGrantedbythedoctor(query[1]);
                else if (query[0].Equals("LetterofAuthority")) ShowLetterofAuthority();

                //-------------------For HR Reports----------------------------------

                else if (query[0].Equals("ManPowerStatus")) ShowManpowerStatus();
                else if (query[0].Equals("ManthlyManPower")) ShowMonthlyManpowerStatus();
                else if (query[0].Equals("DailyOverTimeReportByLine")) loadOverTimeReport();
                else if (query[0].Equals("EmpInformation")) ShowEmpInformation(query[1]);
                else if (query[0].Equals("EmpInformationBangla")) ShowEmpInformationBangla(query[1]);
                else if (query[0].Equals("RecrutmentPanelList")) ShowRecrutmentPanelList(query[1]);
                else if (query[0].Equals("SeparationSheet")) ShowSeparationSheet(query[1]);
                else if (query[0].Equals("SeparationSheetDetails")) ShowSeparationSheetDetails(query[1]);


                else if (query[0].Equals("EmployeeExperience")) ShowEmpExperience();
                else if (query[0].Equals("EmployeeBloodGroup")) LoadEmployeeBloodGroup();
                else if (query[0].Equals("EmployeeDistrictwiseReport")) LoadDistrictWiseEmployeeReport(query[1]);

                else if (query[0].Equals("TiffinBill")) ShowTiffinBill(query[0] + "-" + query[1] + "-" + query[2] + "-" + query[3]);
                else if (query[0].Equals("NightBill")) ShowDailyNightBill(query[0] + "-" + query[1] + "-" + query[2] + "-" + query[3]);
                else if (query[0].Equals("HolidayBill")) ShowHoliDayBill(query[0] + "-" + query[1] + "-" + query[2] + "-" + query[3]);
                else if (query[0].Equals("Monthlytransferredamount")) ShowMonthlytransferredamount(query[1], query[2]);

                else if (query[0].Equals("ContactInfo")) ShowContactInfo();






                else if (query[0].Equals("DailyAttStatus")) ShowDailyAttStatus(query[1], query[2] + "-" + query[3] + "-" + query[4], query[5] + "-" + query[6] + "-" + query[7]);
                else if (query[0].Equals("PromotionInfo")) ShowPromotionInfo();
                else if (query[0].Equals("IncrementInfo")) ShowIncrementInfo();
                else if (query[0].Equals("PFList")) ShowPFList();
                else if (query[0].Equals("BankOrCash")) ShowBankOrCashReport(query[1]);
                else if (query[0].Equals("ContactList")) ShowContactList(query[1]);
                else if (query[0].Equals("ContactListBangla")) ShowContactListBangla(query[1]);


                else if (query[0].Equals("ShiftTarnsferReport")) LoadShiftTransferReport(query[1]);//For Shift Transfer Report
                else if (query[0].Equals("ShiftScheduleDetails")) LoadShiftScheduleDetails();


                //------------------------------For Attendance reports----------------------------------------------
                else if (query[0].Equals("TodaysAttStatus")) TodaysAttendanceStatus(query[1]);
                else if (query[0].Equals("DailyMovement")) ShowDailyMovement(query[1] + "-" + query[2] + "-" + query[3], query[4], query[5]);
                else if (query[0].Equals("OutDutyByDateRange")) OutDutyByDateRange(query[1]);
                else if (query[0].Equals("ManualAttReprot")) ShowManualAttendanceReport(query[1], query[2]);
                else if (query[0].Equals("DailyOTReport")) Daily_OT_Report(query[1] + "-" + query[2] + "-" + query[3]);
                else if (query[0].Equals("MonthlyOverTimeReport")) MonthlyOverTimeReport(query[1] + "-" + query[2]);



                else if (query[0].Equals("ManpowerWiseAttReport")) ManpowerWiseAttReport();
                else if (query[0].Equals("DailyMovementBangla")) ShowDailyMovementBangla(query[1] + "-" + query[2] + "-" + query[3], query[4]);
                else if (query[0].Equals("DailyMovementReportBangla")) DailyMovementBangla(query[1]);
                else if (query[0].Equals("DailyMovementReport")) DailyMovement(query[1]);
                else if (query[0].Equals("EarlyOutLateOut"))
                {
                    if (query.Length == 7)
                        EarlyOutLateOut(query[1] + "-" + query[2] + "-" + query[3] + "-" + query[4] + "-" + query[5], query[6]);
                    else EarlyOutLateOut(query[1] + "-" + query[2] + "-" + query[3], query[4]);
                }
                else if (query[0].Equals("DailyAttSummary")) ShowDailyAttSummaryReport(query[1] + "-" + query[2] + "-" + query[3]);
                else if (query[0].Equals("AttSummary")) LoadAttSummaryDetails(query[1], query[2], query[3], query[4], query[5], query[6], query[7], query[8], query[9], query[10], query[11]);
                else if (query[0].Equals("AllAttSummary")) LoadAllAttSummary(query[1], query[2], query[3], query[4]);
                else if (query[0].Equals("Monthlyattendance")) ShowMonthlyattendance(query[1]);
                else if (query[0].Equals("OvertimeSheet")) ShowOvertimeSheet(query[1] + "-" + query[2]);
                else if (query[0].Equals("MonthlyLoginLogoutReport"))
                {
                    if (query[3] == "Log InOut")
                        MonthlyLoginLogoutReport(query[1] + "-" + query[2]);
                    else if (query[3] == "Att Status") MonthlyAttendanceStatusReport(query[1] + "-" + query[2]);
                    else MonthlyAttendanceSummaryReport(query[1] + "-" + query[2]);
                }
                else if (query[0].Equals("MonthlyLoginLogoutReportBangla"))
                {
                    if (query[3] == "Log InOut")
                        MonthlyLoginLogoutReportBangla(query[1] + "-" + query[2]);
                    else if (query[3] == "Att Status") MonthlyAttendanceStatusReportBangla(query[1] + "-" + query[2]);
                    else MonthlyAttendanceSummaryReportBangla(query[1] + "-" + query[2]);
                }
                else if (query[0].Equals("SpeacificEmpAttSummary")) SpeacificEmpDailyAttReport();
                else if (query[0].Equals("SpeacificAttSummaryMultiDate")) SpeacificEmpDailyAttReport_MultiDate();


                //------------------------------------------------------------------------------------------------------
            }
            catch (Exception ex) { }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            try
            {
                rpd.Refresh();
                CrystalReportViewer1.ReportSource = null;
                rpd.Dispose();

                rpd.Close();

                GC.Collect();
            }
            catch
            {
                rpd.Refresh();

                CrystalReportViewer1.ReportSource = null; ;
                GC.Collect();
            }
        }
        private void loadIndividualTaxReport()
        {

            dt = new DataTable();            
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            dt  = (DataTable)Session["__IndivisualTaxInfo__"];
            dt1 = (DataTable)Session["__TaxCalculationInfo__"];
            dt2 = (DataTable)Session["__RebateCalculationInfo__"];
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//VatTax//IndividualVatTaxReport.rpt"));
            rpd.SetDataSource(dt);
            rpd.Subreports[0].SetDataSource(dt2);
            rpd.Subreports[1].SetDataSource(dt1);
            sqlDB.fillDataTable("select ConvenceAllownce,HouseRent,MedicalAllownce from VatTax_TaxFreeAllowance", dt = new DataTable());
            rpd.SetParameterValue(0, dt.Rows[0]["HouseRent"].ToString());
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
        private void loadIndividualIncomeReport()
        {

            dt = new DataTable();          
            dt = (DataTable)Session["__IndivisualIncomeInfo__"];           
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//VatTax//IndividualIncomeReport.rpt"));
            rpd.SetDataSource(dt);         
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
        private void loadIndividualInvestment()
        {

            dt = new DataTable();
            dt = (DataTable)Session["__IndividualInvestment__"];           
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//VatTax//IndividualInvestmentReport.rpt"));
            rpd.SetDataSource(dt);         
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }

        private void loadTaxInfoSheet()
        {

            dt = new DataTable();
            dt = (DataTable)Session["__IncomeTaxSheet__"];
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//VatTax//VatTaxSummaryReport.rpt"));
            rpd.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }

        private void loadMonthlyPFSheet()
        {

            dt = new DataTable();
            dt = (DataTable)Session["__MonthlyPFSheet__"];
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//PF//MonthlyPFSheet.rpt"));
            rpd.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
        
        private DataTable LoadCompanyInfo(string CompanyId)
        {
            try
            {
                sqlDB.fillDataTable("SELECT CompanyId, CompanyName,Address FROM HRD_CompanyInfo where CompanyId ='" + CompanyId + "' ", dt = new DataTable());
                return dt;
            }
            catch { return dt = null; }
        }
        private void LoadShiftTransferReport(string CompanyId) //For Shift Transfer Report
        {
            try
            {
rpd = new ReportDocument();
                dt = (DataTable)Session["__ShiftTarnsferReport__"];
                rpd.Load(Server.MapPath("//All Report//Personnel//ShiftSedule.rpt"));
                rpd.SetDataSource(dt);
                DataTable dtComInfo = new DataTable();
                dtComInfo = LoadCompanyInfo(CompanyId);
                rpd.SetParameterValue(0, dtComInfo.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtComInfo.Rows[0]["Address"].ToString());
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;


            }
            catch { }
        }
        private void LoadShiftScheduleDetails() //For Shift Schedule Details Report Report
        {
            try
            {

           rpd = new ReportDocument();
                dt = (DataTable)Session["__ShiftScheduleDetails__"];
                rpd.Load(Server.MapPath("//All Report//Personnel//ShiftSeduleDetails.rpt"));
                rpd.SetDataSource(dt);
                DataTable dtComInfo = new DataTable();
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void LoadAttSummaryDetails(string sid, string cid, string did, string title, string fd, string fm, string fy, string d, string m, string y, string PrintType)
        {
            try
            {
                DataTable dt = new DataTable();
                if (title == "Department" || title == "Shift")
                {
                    dt = classes.commonTask.AttSummaryDetails(sid, cid, did, title, d, m, y, "", fd, fm, fy);
                }

                else if (title == "Absent")
                {
                    dt = classes.commonTask.AttSummaryDetails(sid, cid, did, title, d, m, y, "a", fd, fm, fy);
                }
                else if (title == "Present")
                {
                    dt = classes.commonTask.AttSummaryDetails(sid, cid, did, title, d, m, y, "p", fd, fm, fy);
                }
                else if (title == "Leave")
                {
                    dt = classes.commonTask.AttSummaryDetails(sid, cid, did, title, d, m, y, "lv", fd, fm, fy);
                }
                else if (title == "Late")
                {
                    dt = classes.commonTask.AttSummaryDetails(sid, cid, did, title, d, m, y, "l", fd, fm, fy);
                }
                else if (title == "Total")
                {
                    dt = classes.commonTask.AttSummaryDetails(sid, cid, did, title, d, m, y, "Total", fd, fm, fy);
                }
                rpd = new ReportDocument();
                if (PrintType == "View")
                    rpd.Load(Server.MapPath("//All Report//Attendance//DailyMovementReport_ForSummary.rpt"));
                else
                    rpd.Load(Server.MapPath("//All Report//Attendance//DailyMovementReport_ForSummary_print.rpt"));
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0, "Date: " + fd + "-" + fm + "-" + fy);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;

            }
            catch { }
        }

        private void DailyMovementBangla(string Date)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//Daily_Movement.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__DailyMovementReportBangla__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0, Date.Replace('/', '-'));
             

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void DailyMovement(string Date)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//Daily_MovementEnglish.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__DailyMovementReport__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0, Date.Replace('/', '-'));
              

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void LoadAllAttSummary(string fd, string fm, string fy,string rpt)
        {
            try
            {

                rpd = new ReportDocument();
                dt = (DataTable)Session["__AllAttSummary__"];
                if(rpt=="Dpt")
                rpd.Load(Server.MapPath("//All Report//Attendance//AttSummary.rpt"));
                else
                    rpd.Load(Server.MapPath("//All Report//Attendance//AttSummaryByGroup.rpt"));
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0, "Date: " + fd + "-" + fm + "-" + fy);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void loadPaySlip(string month,string year) // Job Card Report By Line
        {
            try
            {
                rpd = new ReportDocument();
                DataTable dtcompany = new DataTable();

                dt = (DataTable)Session["__PaySlip__"];              
                    rpd.Load(Server.MapPath("//All Report//Payroll//PaySlip_JIL.rpt"));
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0, month+"-"+year);
                

                            
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void loadPaySlip2() // Job Card Report By Line Pay Slip 2
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Payroll//PaySlip2.rpt"));
                dt = (DataTable)Session["__dtJobCard__"];
                rpd.SetDataSource(dt);

                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void loadJobCardReport() // Job Card Report By Line
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//JobCardReport.rpt"));
                dt = (DataTable)Session["__dtJobCard__"];
                rpd.SetDataSource(dt);

                DataTable dtSub = new DataTable();
                dtSub = (DataTable)Session["__dtSummary__"];
                ReportDocument subReport = rpd.Subreports[0];
                rpd.Subreports[0].SetDataSource(dtSub);

                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void loadJobCardReportActual() // Job Card Report By Line
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//JobCardReportActual.rpt"));
                dt = (DataTable)Session["__dtJobCard__"];
                rpd.SetDataSource(dt);

                DataTable dtSub = new DataTable();
                dtSub = (DataTable)Session["__dtSummary__"];
                ReportDocument subReport = rpd.Subreports[0];
                rpd.Subreports[0].SetDataSource(dtSub);

                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void MonthlyHolidayAndWeekendStatus() 
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//MonthlyHolidayAndWeekendStatus.rpt"));
                dt = (DataTable)Session["__dtWHStatus__"];
                rpd.SetDataSource(dt);            
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void loadOverTimeReport() // Over Time Report By Line
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//DailyOverTimeReportByLine.rpt"));
                dt = (DataTable)Session["__OverTime__"];
                rpd.SetDataSource(dt);

                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void loadMonthlyOverTimeReport(string MonthName) // Over Time Report By Line
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//MonthlyOverTime.rpt"));
                dt = (DataTable)Session["__MonthlyOverTime__"];
                rpd.SetDataSource(dt);

                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, MonthName);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void loadDailyAbsentReport()  // Daily Absent Report By Line
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//DailyAbsentReportByLine.rpt"));
                dt = (DataTable)Session["__dtAbsent__"];
                rpd.SetDataSource(dt);

                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void loadDailyAttendanceReport()  // Daily Attendance Report By Line
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//DailyAttendanceReportByLine.rpt"));
                dt = (DataTable)Session["__dtAttendance__"];
                rpd.SetDataSource(dt);

                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void loadDailyLateReportByLine()  // Daily Late Report By Line
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//DailyLateReportByLine.rpt"));
                dt = (DataTable)Session["__dtLetReport__"];
                rpd.SetDataSource(dt);

                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void loadDailyOutTimeMissReportByLine()  // Daily Out Time Missing Report By Line
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//OutTimeMissingReportByLine.rpt"));
                dt = (DataTable)Session["__dtOutTimeMiss__"];
                rpd.SetDataSource(dt);

                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }


        private void loadWorkerSalarySheet(string month, string year)
        {
            try
            {
                //ParameterFields paramFields = new ParameterFields();

                //ParameterField paramField = new ParameterField();

                //ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();

                //paramField.ParameterFieldName = "GarmentsTitle";// This is how you can send Parameter Value to the Crystal Report

                //discreteVal.Value = "OK Man";
                //paramField.CurrentValues.Add(discreteVal);

                //paramFields.Add(paramField);
                //CrystalReportViewer1.ParameterFieldInfo = paramFields;


                dt = new DataTable();
                dt = (DataTable)Session["__WorkerSalarySheet__"];

                rpd = new ReportDocument();
                if (Session["__Language__"].ToString().Equals("Bangal")) rpd.Load(Server.MapPath("//All Report//Payroll//WorkerMonthlySalarySheetBangla.rpt"));

                else rpd.Load(Server.MapPath("//All Report//Payroll//WorkerMonthlySalarySheet.rpt"));
                //rpd.FileName=Server.MapPath("//All Report//Payroll//WorkerMonthlySalarySheet.rpt");
                rpd.SetDataSource(dt);
                loadGarmentsInfo();   // for get company info
                if (Session["__Language__"].ToString().Equals("Bangal"))
                {
                    rpd.SetParameterValue(0, dtGramentsInfo.Rows[0]["CompanyNameBangla"].ToString());
                    rpd.SetParameterValue(1, dtGramentsInfo.Rows[0]["AddressBangla"].ToString());

                    rpd.SetParameterValue(2, "gvwmK †eZb eB-" + month);
                    rpd.SetParameterValue(3, year);
                }
                else
                {
                    rpd.SetParameterValue(0, dtGramentsInfo.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, dtGramentsInfo.Rows[0]["Address"].ToString());

                    rpd.SetParameterValue(2, "Monthly Salary Sheet-" + month);
                    rpd.SetParameterValue(3, year);
                }
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;




                //CrystalReportViewer1.GroupTreeStyle = false;

                //gvAdvaceList.DataSource = dt;
                //gvAdvaceList.DataBind();
            }
            catch { }
        }

        private void loadStaffSalasySheet(string month, string year)
        {
            try
            {

                dt = new DataTable();
                dt = (DataTable)Session["__StaffSalarySheet__"];

                rpd = new ReportDocument();
                if (Session["__Language__"].ToString().Equals("Bangal")) rpd.Load(Server.MapPath("//All Report//Payroll//StaffMonthlySalarySheetBangla.rpt"));

                else rpd.Load(Server.MapPath("//All Report//Payroll//StaffMonthlySalarySheet.rpt"));
                //rpd.FileName=Server.MapPath("//All Report//Payroll//WorkerMonthlySalarySheet.rpt");
                rpd.SetDataSource(dt);
                loadGarmentsInfo();   // for get company info
                if (Session["__Language__"].ToString().Equals("Bangal"))
                {
                    rpd.SetParameterValue(0, dtGramentsInfo.Rows[0]["CompanyNameBangla"].ToString());
                    rpd.SetParameterValue(1, dtGramentsInfo.Rows[0]["AddressBangla"].ToString());

                    rpd.SetParameterValue(2, "মাসিক বেতন বই-" + month);
                    // rpd.SetParameterValue(3, year);
                }
                else
                {
                    rpd.SetParameterValue(0, dtGramentsInfo.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, dtGramentsInfo.Rows[0]["Address"].ToString());

                    rpd.SetParameterValue(2, "Monthly Salary Sheet-" + month + "-" + year);
                    // rpd.SetParameterValue(3, year);
                }
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;

                // rpd.PrintToPrinter(1,true, 0, 0);
                //  rpd.ReportOptions.EnableSavePreviewPicture = true;
            }
            catch (Exception ex)
            {
            }
        }

        DataTable dtGramentsInfo;
        private void loadGarmentsInfo()
        {
            try
            {
                sqlDB.fillDataTable("select CompanyName,CompanyNameBangla,Address,AddressBangla from HRD_CompanyInfo", dtGramentsInfo = new DataTable());
            }
            catch { }
        }
        private void LoadEmployeeProfile(string IsDefault)
        {
            try
            {
                rpd = new ReportDocument();
                if (IsDefault=="0")
                {
                    rpd.Load(Server.MapPath("//All Report//Personnel//EmployeeBioData.rpt"));
                    dt = new DataTable();
                    dt = (DataTable)Session["__EmployeeProfile__"];
                    rpd.SetDataSource(dt);
                    DataTable dtcompany = new DataTable();
                    sqlDB.fillDataTable("Select CompanyName,Address,Telephone From HRD_CompanyInfo", dtcompany);
                    rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                    rpd.SetParameterValue(2, dtcompany.Rows[0]["Telephone"].ToString());
                    rpd.SetParameterValue(3, Server.MapPath("//EmployeeImages//Images//"));
                }
                else
                {
                   
                    rpd.Load(Server.MapPath("//All Report//Personnel//EmpProfileClient.rpt"));
                    dt = new DataTable();
                    dt = (DataTable)Session["__EmployeeProfile__"];
                    rpd.SetDataSource(dt);
                    rpd.SetParameterValue(0, Server.MapPath("//EmployeeImages//Images//"));
                }
                    
                
                //DataTable dt1 = new DataTable();
                //sqlDB.fillDataTable("Select * From v_Promotion_Increment", dt1);
                //rpd.Subreports[0].SetDataSource(dt1);
                //DataTable dt2 = new DataTable();
                //sqlDB.fillDataTable("Select * From Personnel_EmpExperience", dt2);
                //rpd.Subreports[1].SetDataSource(dt2);
                
                //rpd.SetParameterValue(2,SuperAdmin);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void LoadDistrictWiseEmployeeReport(string SuperAdmin)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//EmployeeDistrictReport.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__EmployeeDistrictwiseReport__"];
                rpd.SetDataSource(dt);

                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(1, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(2, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(0, SuperAdmin);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void LoadEmployeeBloodGroup()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//BloodGroup.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__EmployeeBloodGroup__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }


        private void LoadAppoinmentLetter(string EmpId)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//AppointmentLetter.rpt"));
                dt = new DataTable();
                DataTable dtSn = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, EmpId from v_AppoinmentLetter where EmpId='" + EmpId + "' and  ActiveSalary='True' group by EmpId", dtSn);
                sqlDB.fillDataTable("Select  EmpId,EmpCardNo,EmpProximityNo,EmpNameBn,FatherNameBn,PresentAdBangla,PermanentVillage,PermanentPostOffice,PermanentThana,PermanentZilla,Convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,105) as DateOfBirth,DsgNameBn,BasicSalary,HouseRent,MedicalAllownce,Age,DptNameBn,GrdName,OthersAllownce,FoodAllownce,ConvenceAllownce,EmpTypeId,GrdNameBangla From v_AppoinmentLetter where SN=" + dtSn.Rows[0]["SN"].ToString() + " and ActiveSalary='True' ", dt);
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyNameBangla,AddressBangla From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyNameBangla"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["AddressBangla"].ToString());
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void LoadResignationLetter()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//Resignation_Letter.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__ResignationLetter__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyNameBangla,AddressBangla From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyNameBangla"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["AddressBangla"].ToString());
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void PromotionLetterStaff(string DsgName, string EDate)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//Promotion_Letter_Staff.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__PromotionLetterStaff__"];
                rpd.SetDataSource(dt);
                //DataTable dtcompany = new DataTable();
                //sqlDB.fillDataTable("Select CompanyNameBangla,AddressBangla From HRD_CompanyInfo", dtcompany);
                //rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyNameBangla"].ToString());
                //rpd.SetParameterValue(1, dtcompany.Rows[0]["AddressBangla"].ToString());
                CrystalReportViewer1.ReportSource = rpd;
                rpd.SetParameterValue(0, DsgName);
                rpd.SetParameterValue(1, EDate);
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void PromotionLetterWorker(string DsgName, string EDate)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//Promotion_Letter_Worker.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__PromotionLetterWorker__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyNameBangla,AddressBangla From HRD_CompanyInfo", dtcompany);
                CrystalReportViewer1.ReportSource = rpd;
                rpd.SetParameterValue(0, DsgName);
                rpd.SetParameterValue(1, EDate);
                rpd.SetParameterValue(2, dtcompany.Rows[0]["CompanyNameBangla"].ToString());
                rpd.SetParameterValue(3, dtcompany.Rows[0]["AddressBangla"].ToString());
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void PromotionSheet(string MonthName)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//Promotion_Sheet.rpt"));

                dt = new DataTable();
                dt = (DataTable)Session["__PromotionSheet__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                CrystalReportViewer1.ReportSource = rpd;
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, MonthName);

                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void PreviousPromotionSheet(string MonthName)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//Previous_Promotion_Sheet.rpt"));

                dt = new DataTable();
                dt = (DataTable)Session["__PreviousPromotionSheet__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                CrystalReportViewer1.ReportSource = rpd;
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, MonthName);

                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void IncrementSheet(string MonthName)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//IncrementSheet.rpt"));

                dt = new DataTable();
                dt = (DataTable)Session["__IncrementSheet__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                CrystalReportViewer1.ReportSource = rpd;
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, MonthName);

                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void IndivisualIncrementSheet()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//IndividualIncrementSheet.rpt"));

                dt = new DataTable();
                dt = (DataTable)Session["__IndivisualIncrementSheet__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                CrystalReportViewer1.ReportSource = rpd;
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
               // rpd.SetParameterValue(2, MonthName);

                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void IncrementSheetDetails(string MonthName)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//IncrementSheetDetails.rpt"));

                dt = new DataTable();
                dt = (DataTable)Session["__IncrementSheetDetails__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                CrystalReportViewer1.ReportSource = rpd;
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, MonthName);

                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void IndIncrementSheetDetails()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//IndIncrementSheetDetails.rpt"));

                dt = new DataTable();
                dt = (DataTable)Session["__IndIncrementSheetDetails__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                CrystalReportViewer1.ReportSource = rpd;
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());                

                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void PriviousIncrementSheet(string MonthName)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//Privious_Salary_Increment_Sheet.rpt"));

                dt = new DataTable();
                dt = (DataTable)Session["__PriviousIncrementSheet__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                CrystalReportViewer1.ReportSource = rpd;
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, MonthName);

                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void IncrementLetterStaff(string BasicSalary, string OthersAllow, string presentSalary, string EDate, string UserType)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//Increment_Letter_Staff.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__IncrementLetterStaff__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                CrystalReportViewer1.ReportSource = rpd;
                rpd.SetParameterValue(0, presentSalary);
                rpd.SetParameterValue(1, EDate);
                rpd.SetParameterValue(2, BasicSalary);
                rpd.SetParameterValue(3, OthersAllow);
                rpd.SetParameterValue(4, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(5, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(6, UserType);
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void IncrementLetterWorker(string SN)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//IncrementLetter_Worker.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__IncrementLetterWorker__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyNameBangla,AddressBangla From HRD_CompanyInfo", dtcompany);
                CrystalReportViewer1.ReportSource = rpd;
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyNameBangla"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["AddressBangla"].ToString());
                DataTable dtRunning = new DataTable();
                sqlDB.fillDataTable("Select GrdNameBangla,DsgNameBn,BasicSalary,HouseRent,MedicalAllownce,ConvenceAllownce,EmpPresentSalary,EffectiveMonth,FoodAllownce From v_Personnel_EmpCurrentStatus where SN=" + SN + "", dtRunning);
                rpd.SetParameterValue(2, dtRunning.Rows[0]["GrdNameBangla"].ToString());
                rpd.SetParameterValue(3, dtRunning.Rows[0]["DsgNameBn"].ToString());
                rpd.SetParameterValue(4, dtRunning.Rows[0]["BasicSalary"].ToString());
                rpd.SetParameterValue(5, dtRunning.Rows[0]["HouseRent"].ToString());
                dt = new DataTable();
                sqlDB.fillDataTable("Select Distinct HouseRent from HRD_AllownceSetting", dt);
                rpd.SetParameterValue(6, dt.Rows[0]["HouseRent"].ToString());
                rpd.SetParameterValue(7, dtRunning.Rows[0]["MedicalAllownce"].ToString());
                rpd.SetParameterValue(8, dtRunning.Rows[0]["ConvenceAllownce"].ToString());
                rpd.SetParameterValue(9, dtRunning.Rows[0]["EmpPresentSalary"].ToString());
                rpd.SetParameterValue(10, dtRunning.Rows[0]["EffectiveMonth"].ToString());
                rpd.SetParameterValue(11, dtRunning.Rows[0]["FoodAllownce"].ToString());
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void LoadAppoinmentLetterStaff(string EmpId, string upsuperadmin)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//StaffAPPLetter.rpt"));
                dt = new DataTable();
                DataTable dtSn = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN, EmpId from v_AppoinmentLetter where EmpId='" + EmpId + "' and  ActiveSalary='True' group by EmpId", dtSn);
                sqlDB.fillDataTable("Select  EmpId,PresentAd,PermanentAd,EmpName,Convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,Convert(varchar(11),DateOfBirth,105) as DateOfBirth,DsgNameBn,BasicSalary,Age,DptNameBn,GrdName,OthersAllownce,FoodAllownce,ConvenceAllownce,EmpTypeId,GrdNameBangla,Sex,DsgName,SalaryCount From v_AppoinmentLetter where SN=" + dtSn.Rows[0]["SN"].ToString() + " and ActiveSalary='True' ", dt);
                rpd.SetDataSource(dt);
                //DataTable dtcompany = new DataTable();
                //sqlDB.fillDataTable("Select CompanyNameBangla,AddressBangla From HRD_CompanyInfo", dtcompany);
                //rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyNameBangla"].ToString());
                //rpd.SetParameterValue(1, dtcompany.Rows[0]["AddressBangla"].ToString());
                CrystalReportViewer1.ReportSource = rpd;
                rpd.SetParameterValue(0, upsuperadmin);
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void PrintWorkerIDCard()
        {
            try
            {
                rpd = new ReportDocument();
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyLogo,CompanyName,ShortName From HRD_CompanyInfo", dtcompany);

                if (Session["__ReportView__"].ToString() == "0")
                {
                    if (dtcompany.Rows[0]["ShortName"].ToString() == "MSL")
                    {
                        rpd.Load(Server.MapPath("//All Report//Personnel//FrontPartWorkerID.rpt"));
                    }
                    else rpd.Load(Server.MapPath("//All Report//Personnel//AHGWorker.rpt"));
                }
                else
                {
                    if (dtcompany.Rows[0]["ShortName"].ToString() == "MSL")
                    {
                        rpd.Load(Server.MapPath("//All Report//Personnel//BackPartWorkerID.rpt"));
                    }
                    else rpd.Load(Server.MapPath("//All Report//Personnel//AHGBackPartWorkerID.rpt"));
                }
                dt = new DataTable();
                dt = (DataTable)Session["__WorkerID__"];
                rpd.SetDataSource(dt);
                
                //rpd.SetParameterValue(0, Server.MapPath("//EmployeeImages//CompanyLogo//" + Path.GetFileName(dtcompany.Rows[0]["CompanyLogo"].ToString())));
                if (dtcompany.Rows[0]["ShortName"].ToString() == "MSL")
                {
                    rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, Server.MapPath("//EmployeeImages//Images//"));
                    rpd.SetParameterValue(2, Server.MapPath("//EmployeeImages//CompanyLogo//"));
                    rpd.SetParameterValue(3, Server.MapPath("//EmployeeImages//Signature//MSL2015020001authorizedsignature.jpg"));
                }
                else
                {
                    rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, Server.MapPath("//EmployeeImages//Images//"));
                    rpd.SetParameterValue(2, Server.MapPath("//EmployeeImages//Signature//"));
                    dt = new DataTable();
                    sqlDB.fillDataTable("Select SignatureImage From Personnel_EmployeeInfo WHERE AuthorizedPerson='1'", dt);
                    rpd.SetParameterValue(3, dt.Rows[0]["SignatureImage"].ToString());
                }
                
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void PrintStaffIDCard()
        {
            try
            {
                rpd = new ReportDocument();
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyLogo,CompanyName,ShortName From HRD_CompanyInfo", dtcompany);

                if (Session["__ReportView__"].ToString() == "0")
                {
                    if (dtcompany.Rows[0]["ShortName"].ToString() == "MSL")
                    {
                        rpd.Load(Server.MapPath("//All Report//Personnel//FrontPartWorkerID.rpt"));
                    }
                    else rpd.Load(Server.MapPath("//All Report//Personnel//AHGStaff.rpt"));
                }
                else
                {
                    if (dtcompany.Rows[0]["ShortName"].ToString() == "MSL")
                    {
                        rpd.Load(Server.MapPath("//All Report//Personnel//BackPartWorkerID.rpt"));
                    }
                    else rpd.Load(Server.MapPath("//All Report//Personnel//AHGBackPartWorkerID.rpt"));
                }
                dt = new DataTable();
                dt = (DataTable)Session["__WorkerID__"];
                rpd.SetDataSource(dt);

                //rpd.SetParameterValue(0, Server.MapPath("//EmployeeImages//CompanyLogo//" + Path.GetFileName(dtcompany.Rows[0]["CompanyLogo"].ToString())));
                if (dtcompany.Rows[0]["ShortName"].ToString() == "MSL")
                {
                    rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, Server.MapPath("//EmployeeImages//Images//"));
                    rpd.SetParameterValue(2, Server.MapPath("//EmployeeImages//CompanyLogo//"));
                    rpd.SetParameterValue(3, Server.MapPath("//EmployeeImages//Signature//MSL2015020001authorizedsignature.jpg"));
                }
                else
                {
                    rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, Server.MapPath("//EmployeeImages//Images//"));
                    rpd.SetParameterValue(2, Server.MapPath("//EmployeeImages//Signature//"));
                    dt = new DataTable();
                    sqlDB.fillDataTable("Select SignatureImage From Personnel_EmployeeInfo WHERE AuthorizedPerson='1'", dt);
                    rpd.SetParameterValue(3, dt.Rows[0]["SignatureImage"].ToString());
                }

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void PrintStaffOfficerIDCard()
        {
            try
            {
                rpd = new ReportDocument();
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyLogo,CompanyName,ShortName From HRD_CompanyInfo", dtcompany);

                if (Session["__ReportView__"].ToString() == "0")
                {
                    if (dtcompany.Rows[0]["ShortName"].ToString() == "MSL")
                    {
                        rpd.Load(Server.MapPath("//All Report//Personnel//FrontPartWorkerID.rpt"));
                    }
                    else rpd.Load(Server.MapPath("//All Report//Personnel//AHGOfficer.rpt"));
                }
                else
                {
                    if (dtcompany.Rows[0]["ShortName"].ToString() == "MSL")
                    {
                        rpd.Load(Server.MapPath("//All Report//Personnel//BackPartWorkerID.rpt"));
                    }
                    else rpd.Load(Server.MapPath("//All Report//Personnel//AHGBackPartWorkerID.rpt"));
                }
                dt = new DataTable();
                dt = (DataTable)Session["__WorkerID__"];
                rpd.SetDataSource(dt);

                //rpd.SetParameterValue(0, Server.MapPath("//EmployeeImages//CompanyLogo//" + Path.GetFileName(dtcompany.Rows[0]["CompanyLogo"].ToString())));
                if (dtcompany.Rows[0]["ShortName"].ToString() == "MSL")
                {
                    rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, Server.MapPath("//EmployeeImages//Images//"));
                    rpd.SetParameterValue(2, Server.MapPath("//EmployeeImages//CompanyLogo//"));
                    rpd.SetParameterValue(3, Server.MapPath("//EmployeeImages//Signature//MSL2015020001authorizedsignature.jpg"));
                }
                else
                {
                    rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, Server.MapPath("//EmployeeImages//Images//"));
                    rpd.SetParameterValue(2, Server.MapPath("//EmployeeImages//Signature//"));
                    dt = new DataTable();
                    sqlDB.fillDataTable("Select SignatureImage From Personnel_EmployeeInfo WHERE AuthorizedPerson='1'", dt);
                    rpd.SetParameterValue(3, dt.Rows[0]["SignatureImage"].ToString());
                }

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        //private void PrintStaffIDCard()
        //{
        //    try
        //    {
        //        rpd = new ReportDocument();
        //        rpd.Load(Server.MapPath("//All Report//Personnel//StaffIdCard.rpt"));
        //        dt = new DataTable();
        //        dt = (DataTable)Session["__StaffID__"];
        //        rpd.SetDataSource(dt);
        //        DataTable dtcompany = new DataTable();
        //        sqlDB.fillDataTable("Select CompanyLogo,CompanyName From HRD_CompanyInfo", dtcompany);
        //        rpd.SetParameterValue(0, Server.MapPath("//EmployeeImages//CompanyLogo//" + Path.GetFileName(dtcompany.Rows[0]["CompanyLogo"].ToString())));
        //        rpd.SetParameterValue(1, dtcompany.Rows[0]["CompanyName"].ToString());
        //        rpd.SetParameterValue(2, Server.MapPath("//EmployeeImages//Images//"));
        //        CrystalReportViewer1.ReportSource = rpd;
        //        CrystalReportViewer1.HasToggleGroupTreeButton = false;

        //        //string strpath = base.Server.MapPath("~/All Report/PdfFile");
        //        //string[] files = Directory.GetFiles(strpath);
        //        //foreach (string file in files)
        //        //{
        //        //    FileInfo fi = new FileInfo(file);
        //        //    if (fi.LastAccessTime < DateTime.Now.AddDays(-1))
        //        //        fi.Delete();
        //        //}
        //        //string filename = "StaffID" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + adviitScripting.getRandomString(1);
        //        //string strfullpath = string.Concat(strpath, "\\", filename, ".pdf");
        //        //rpd.ExportToDisk(ExportFormatType.PortableDocFormat, strfullpath);
                
                
        //    }
        //    catch { }
        //}
        private void ShowCurrentSalaryStructure(string upsuperadmin)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Payroll//CurrentSalaryStructure.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__CurrentSalaryStructure__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                CrystalReportViewer1.ReportSource = rpd;
                rpd.SetParameterValue(0, upsuperadmin);
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowManpowerStatus()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//ManPowerStatus.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__ManPowerStatus__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;

            }
            catch { }
        }
        private void ShowMonthlyManpowerStatus()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//MonthlyManpower.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__ManthlyManPower__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;

            }
            catch { }
        }
        private void ShowEmpInformation(string ReportType)
        {
            try
            {
                rpd = new ReportDocument();
                if (ReportType == "BasicInfo")
                {
                    rpd.Load(Server.MapPath("//All Report//Personnel//EmpBasicInfoList.rpt"));
                }
                else if (ReportType == "Designation")
                {
                    rpd.Load(Server.MapPath("//All Report//Personnel//DesignationwiseEmpList.rpt"));
                    DataTable dtcompany = new DataTable();
                    sqlDB.fillDataTable("Select CompanyName,Address,Telephone From HRD_CompanyInfo where CompanyId='" + ViewState["__CompanyId__"] + "'", dtcompany);
                    rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                    rpd.SetParameterValue(2, dtcompany.Rows[0]["Telephone"].ToString());
                }
                else if (ReportType == "District")
                {
                    rpd.Load(Server.MapPath("//All Report//Personnel//DistrictwiseEmpList.rpt"));
                    DataTable dtcompany = new DataTable();
                    sqlDB.fillDataTable("Select CompanyName,Address,Telephone From HRD_CompanyInfo where CompanyId='" + ViewState["__CompanyId__"] + "'", dtcompany);
                    rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                    rpd.SetParameterValue(2, dtcompany.Rows[0]["Telephone"].ToString());
                }
                else if (ReportType == "Religion")
                {
                    rpd.Load(Server.MapPath("//All Report//Personnel//ReligionwiseEmpList.rpt"));
                    DataTable dtcompany = new DataTable();
                    sqlDB.fillDataTable("Select CompanyName,Address,Telephone From HRD_CompanyInfo where CompanyId='" + ViewState["__CompanyId__"] + "'", dtcompany);
                    rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                    rpd.SetParameterValue(2, dtcompany.Rows[0]["Telephone"].ToString());
                }
                dt = new DataTable();
                dt = (DataTable)Session["__EmpInformation__"];
                rpd.SetDataSource(dt);
               
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowEmpInformationBangla(string ReportType)
        {
            try
            {
                rpd = new ReportDocument();
                if (ReportType == "BasicInfo")
                {
                    rpd.Load(Server.MapPath("//All Report//Personnel//EmpBasicInfoListBangla.rpt"));
                    //rpd.SetParameterValue(0, Gender);
                }
                else if (ReportType == "Designation")
                {
                    rpd.Load(Server.MapPath("//All Report//Personnel//DesignationwiseEmpListBangla.rpt"));
                }
                else if (ReportType == "District")
                {
                    rpd.Load(Server.MapPath("//All Report//Personnel//DistrictwiseEmpListBangla.rpt"));
                }
                else if (ReportType == "Religion")
                {
                    rpd.Load(Server.MapPath("//All Report//Personnel//ReligionwiseEmpListBangla.rpt"));
                }
                dt = new DataTable();
                dt = (DataTable)Session["__EmpInformationBangla__"];
                rpd.SetDataSource(dt);                
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowRecrutmentPanelList(string MonthName)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//RecruitmentPannelList.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__RecrutmentPanelList__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, MonthName);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;


            }
            catch { }
        }
        private void ShowRejoinSheet(string MonthName)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//Rejoin_Sheet.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__RejoinSheet__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, MonthName);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;


            }
            catch { }
        }
        private void ShowSeparationSheet(string MonthName)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//SeparationSheet.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__SeparationSheet__"];
                rpd.SetDataSource(dt);               
                rpd.SetParameterValue(0, MonthName);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void ShowSeparationSheetDetails(string MonthName)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//SeparationDetailsReport.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__SeparationSheetDetails__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, MonthName);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowNoteGenerate(string monthName)
        {
            try
            {
                rpd = new ReportDocument();

                string[] getMonth = monthName.Split('-');
                string MonthName = new DateTime(int.Parse(getMonth[1]), int.Parse(getMonth[0]), 1).ToString("MMMM", CultureInfo.InvariantCulture);

                rpd.Load(Server.MapPath("//All Report//Payroll//note_amount_generate.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__NoteAmount__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                //rpd.SetParameterValue(2, dtcompany.Rows[0]["Address"].ToString());

                rpd.SetParameterValue(2, MonthName + "-" + getMonth[1]);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void SalaryFlow(string monthName)
        {
            try
            {
                rpd = new ReportDocument();

              //  string[] getMonth = monthName.Split('-');
             //   string MonthName = new DateTime(int.Parse(getMonth[1]), int.Parse(getMonth[0]), 1).ToString("MMMM", CultureInfo.InvariantCulture);

                rpd.Load(Server.MapPath("//All Report//Payroll//Graphically_MonthlySalaryFlow.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__SalaryFlow__"];
                rpd.SetDataSource(dt);
               
                //rpd.SetParameterValue(2, dtcompany.Rows[0]["Address"].ToString());

               
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void ShowOvertimeSheet(string monthName)
        {
            try
            {
                rpd = new ReportDocument();

                string[] getMonth = monthName.Split('-');
                // string MonthName = new DateTime(int.Parse(getMonth[1]), int.Parse(getMonth[0]), 1).ToString("MMMM", CultureInfo.InvariantCulture);

                rpd.Load(Server.MapPath("//All Report//Payroll//OvertimeSheet_SF.rpt"));   // pesot=Pay Slip Extra OverTime
                dt = new DataTable();
                dt = (DataTable)Session["__OvertimeSheet__"];
                rpd.SetDataSource(dt);
                //  DataTable dtcompany = new DataTable();
                //sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                // rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                // rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());

                //  rpd.SetParameterValue(2, MonthName + "-" + getMonth[1]);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void MonthlyLoginLogoutReport(string monthName)
        {
            try
            {
                string [] getMonth=monthName.Split('-');
                string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(getMonth[0]));
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//Monthly_LogIn_LogOut_Report.rpt"));   // pesot=Pay Slip Extra OverTime
                dt = new DataTable();
                dt = (DataTable)Session["__MonthlyLoginLogoutReport__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(1, "Log in-out report of " + strMonthName +"-"+getMonth[1]);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

    
        private void MonthlyAttendanceStatusReport(string monthName)
        {
            try
            {
                string[] getMonth = monthName.Split('-');
                string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(getMonth[0]));
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//MonthlyAttendanceStatusSheet.rpt"));   // pesot=Pay Slip Extra OverTime
                dt = new DataTable();
                dt = (DataTable)Session["__MonthlyLoginLogoutReport__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(1, "Attendance status report of " + strMonthName + "-" + getMonth[1]);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        /*..............................Start............................................
         Md.Abid Hasan (Nayem)
         nayem.optimal@gmail.com
         Purpose: Monthly Attendance summary Report preview
         */
        private void MonthlyAttendanceSummaryReport(string monthName)
        {
            try
            {
                string[] getMonth = monthName.Split('-');
                string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(getMonth[0]));
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//MonthlyAttendanceStatusSummary3For_SG.rpt"));   // pesot=Pay Slip Extra OverTime
                dt = new DataTable();
                dt = (DataTable)Session["__MonthlyLoginLogoutReport__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(1, "Attendance status with summary report of " + strMonthName + "-" + getMonth[1]);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void MonthlyLoginLogoutReportBangla(string monthName)
        {
            try
            {
                string[] getMonth = monthName.Split('-');
               // string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(getMonth[0]));
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//Monthly_LogIn_LogOut_ReportBangla.rpt"));   // pesot=Pay Slip Extra OverTime
                dt = new DataTable();
                dt = (DataTable)Session["__MonthlyLoginLogoutReportBangla__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(1, "gvwmK cÖ‡ek Ges evwni mxU," + monthName);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void MonthlyAttendanceStatusReportBangla(string monthName)
        {
            try
            {
                string[] getMonth = monthName.Split('-');
               // string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(getMonth[0]));
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//MonthlyAttendanceStatusSheetBanglaColor.rpt"));   // pesot=Pay Slip Extra OverTime
                dt = new DataTable();
                dt = (DataTable)Session["__MonthlyLoginLogoutReportBangla__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(1, "gvwmK nvwRiv mxU, " + monthName);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void MonthlyAttendanceSummaryReportBangla(string monthName)
        {
            try
            {
                string[] getMonth = monthName.Split('-');
               // string strMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(int.Parse(getMonth[0]));
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//MonthlyAttendanceStatusSummary3BanglaColor.rpt"));   // pesot=Pay Slip Extra OverTime
                dt = new DataTable();
                dt = (DataTable)Session["__MonthlyLoginLogoutReportBangla__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(1, "gvwmK nvwRiv mxU, " + monthName );
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        //...........................................
        private void ShowEmpExperience()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//EmployeeExperience.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__EmployeeExperience__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }


        private void ShowTiffinBill(string getDate)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Payroll//DailyTiffinBill.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__TiffinBill__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, getDate);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void ShowDailyNightBill(string getDate)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Payroll//DailyNightBill.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__NightBill__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, getDate);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void ShowHoliDayBill(string getDate)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Payroll//DailyHolidayBill.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__HolidayBill__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, getDate);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void ShowContactInfo()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//EmpContactInfo.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__ContactInfo__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyNameBangla,AddressBangla From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyNameBangla"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["AddressBangla"].ToString());
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowDailyAttSummaryReport(string Month)
        {
            try
            {
                // string[] getMonth = Month.Split('-');
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//DailyAttSummary.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__DailyAttSummary__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0, Month);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowLeaveStatusSummary(string MonthName)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Leave//LeaveStatusSummary.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__LeaveStatusSummary__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, MonthName);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowMonthlyattendance(string MonthName)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//MonthlyAttendanceReport.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__Monthlyattendance__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, MonthName);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowMonthlytransferredamount(string MonthName, string Year)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Payroll//Monthlytransferredamount.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__Monthlytransferredamount__"];
                rpd.SetDataSource(dt);
                //DataTable dtcompany = new DataTable();
                //sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                //rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                //rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(0, MonthName + "-" + Year);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void ShowEarnLeaveGeneration(string MonthName)  // For Earn Leave Report . By Nayem.
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Leave//earn_leave_generation.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__EarnLeave__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0, MonthName);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        /*........................................................
         Md.Abid Hasan (Nayem)
         nayem.optimal@gmail.com
         .........................................................*/
        private void ShowMaternityLeaveApplication() // For Maternity Leave Application
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Leave//MaternityLeaveApplicationDynamic.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__LeaveApplication__"];
                rpd.SetDataSource(dt);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowDoctorCertificationLetter()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Leave//Maternity_Doctor_Certification_Letter.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__DoctorCertificationLetter__"];
                rpd.SetDataSource(dt);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowGrantedbythedoctor(string EmpId)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Leave//MaternityGrantedbythedoctor.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__Grantedbythedoctor__"];
                rpd.SetDataSource(dt);
                if (dt.Rows[0]["FromDate"].ToString() != "")
                {
                    string[] Fdate = dt.Rows[0]["FromDate"].ToString().Split('-');
                    string[] TDate = dt.Rows[0]["ToDate"].ToString().Split('-');
                    DateTime FFdate = DateTime.Parse(Fdate[1] + "/" + Fdate[0] + "/" + Fdate[2]);
                    DateTime ToDate = DateTime.Parse(TDate[1] + "/" + TDate[0] + "/" + TDate[2]);
                    TimeSpan difference = (ToDate.AddDays(1) - FFdate);

                    string totaldays = difference.TotalDays.ToString();
                    int x = int.Parse(totaldays) / 2;
                    int i = 1;
                    while (i < x)
                    {
                        FFdate = FFdate.AddDays(1);
                        i++;
                    }
                    string format = "dd-MM-yyyy";
                    string FirstEnddate = FFdate.ToString(format);
                    string SecondFirstdate = FFdate.AddDays(1).ToString(format);
                    rpd.SetParameterValue(0, dt.Rows[0]["FromDate"].ToString());
                    rpd.SetParameterValue(1, FirstEnddate);
                    rpd.SetParameterValue(2, SecondFirstdate);
                    rpd.SetParameterValue(3, ToDate.ToString(format));
                    dt = new DataTable();
                    sqlDB.fillDataTable("Select Max(SL) as SL, InstallmentAmount,FirstInstallmentSignature,FORMAT(FirstAcceptDate,'dd-MM-yyyy') as FirstAcceptDate,SecondInstallmentSignature,FORMAT(SecondAcceptDate,'dd-MM-yyyy') as SecondAcceptDate From Leave_MaterintyVoucher where EmpId='" + EmpId + "' group by InstallmentAmount,FirstInstallmentSignature,SecondInstallmentSignature,FirstAcceptDate,SecondAcceptDate", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FirstInstallmentSignature"].ToString() == "True")
                        {
                            rpd.SetParameterValue(4, dt.Rows[0]["InstallmentAmount"].ToString());
                            rpd.SetParameterValue(6, dt.Rows[0]["FirstAcceptDate"].ToString());
                        }
                        else
                        {
                            rpd.SetParameterValue(4, "");
                            rpd.SetParameterValue(6, "");
                        }
                            
                        if (dt.Rows[0]["SecondInstallmentSignature"].ToString() == "True")
                        {
                            rpd.SetParameterValue(5, dt.Rows[0]["InstallmentAmount"].ToString());
                            rpd.SetParameterValue(7, dt.Rows[0]["SecondAcceptDate"].ToString());
                        }
                        else 
                        {
                            rpd.SetParameterValue(5, "");
                            rpd.SetParameterValue(7, "");
                        }
                    }
                    else
                    {
                        rpd.SetParameterValue(4, "");
                        rpd.SetParameterValue(5, "");
                        rpd.SetParameterValue(6, "");
                        rpd.SetParameterValue(7, "");
                    }

                }
                else
                {
                    rpd.SetParameterValue(0, "");
                    rpd.SetParameterValue(1, "");
                    rpd.SetParameterValue(2, "");
                    rpd.SetParameterValue(3, "");
                    rpd.SetParameterValue(4, "");
                    rpd.SetParameterValue(5, "");
                    rpd.SetParameterValue(6, "");
                    rpd.SetParameterValue(7, "");
                }
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowLetterofAuthority()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Leave//MaternityLetterofAuthority.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__LetterofAuthority__"];
                rpd.SetDataSource(dt);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowLeaveRegister()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Leave//Leave_Register.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__LeaveRegister__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void TodaysAttendanceStatus(string title)
        {
            try
            {
                ReportDocument rpd;
                DataTable dt = new DataTable();
                dt = (DataTable)Session["__TodaysAttStatus__"];
                rpd = new ReportDocument();
                if (!classes.machineType.fBrowserIsMobile())
                    rpd.Load(Server.MapPath("//All Report//Attendance//AttTodaysStatus.rpt"));
                else
                    rpd.Load(Server.MapPath("//All Report//Attendance//AttTodaysStatusM.rpt"));
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0, "Date : " + DateTime.Now.ToString("dd-MM-yyyy"));
                if (title == "a")
                    title = "Todays Absent Status";
                else if (title == "p")
                    title = "Todays Present Status";
                else if (title == "l")
                    title = "Todays Late Status";
                else title = "Todays Attendance Status";

                rpd.SetParameterValue(1, title);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;

            }
            catch { }
        }
        private void ShowDailyMovement(string Date,string IsColor,string Title)
        {
            try
            {
                rpd = new ReportDocument();
                if(IsColor=="0")
                rpd.Load(Server.MapPath("//All Report//Attendance//DailyMovementReportColor.rpt"));
                else rpd.Load(Server.MapPath("//All Report//Attendance//DailyMovementReport.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__DailyMovement__"];
                rpd.SetDataSource(dt);                
                rpd.SetParameterValue(0, Date);
                rpd.SetParameterValue(1, Title);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void OutDutyByDateRange(string Date)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//OutDutyByDateRange.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__OutDutyByDateRange__"];
                rpd.SetDataSource(dt);                
                rpd.SetParameterValue(0, Date.Replace('/','-'));
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void ShowManualAttendanceReport(string DateRange, string IsColor)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//DailyManualAttendanceReport.rpt"));               
                dt = new DataTable();
                dt = (DataTable)Session["__ManualAttReprot__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0, DateRange.Replace('/','-'));

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void Daily_OT_Report(string Date)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//DailyOTReport.rpt"));                
                dt = new DataTable();
                dt = (DataTable)Session["__DailyOTReport__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0, Date);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void MonthlyOverTimeReport(string monthName)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//Monthly_OverTime_Report_SFG.rpt"));   // pesot=Pay Slip Extra OverTime
                dt = new DataTable();
                dt = (DataTable)Session["__MonthlyOverTimeReport__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(1, monthName);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ManpowerWiseAttReport()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//ManPowerWiseAttReport.rpt"));             
                dt = new DataTable();
                dt = (DataTable)Session["__ManpowerWiseAttReport__"];
                rpd.SetDataSource(dt);  
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void ShowDailyMovementBangla(string Date, string IsColor)
        {
            try
            {
                rpd = new ReportDocument();
                if (IsColor == "0")
                    rpd.Load(Server.MapPath("//All Report//Attendance//DailyMovementReportBanglaColor.rpt"));
                else rpd.Load(Server.MapPath("//All Report//Attendance//DailyMovementReportBangla.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__DailyMovementBangla__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0, Date);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void EarlyOutLateOut(string Date,string ReportTitle)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//EarlyOut_LateOut_Report.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__DailyMovement__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0, ReportTitle);
                rpd.SetParameterValue(1,Date);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void ShowDailyAttStatus(string status, string Date, string FromDate)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Attendance//DailyAttendanceStatus.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__DailyAttStatus__"];
                rpd.SetDataSource(dt);
                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, Date);
                rpd.SetParameterValue(3, status);
                rpd.SetParameterValue(4, FromDate);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowPromotionInfo()//  Updated by Nayem.  Purpose: Individual Promotion Status Report.
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//PromotionReport.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__PromotionInfo__"];
                rpd.SetDataSource(dt);
                //DataTable dtcompany = new DataTable();
                //sqlDB.fillDataTable("Select CompanyName,Address From HRD_CompanyInfo", dtcompany);
                //rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyName"].ToString());
                //rpd.SetParameterValue(1, dtcompany.Rows[0]["Address"].ToString());

                //DataTable dtPersonalInfo = new DataTable();
                //sqlDB.fillDataTable("select DName,DptName,DsgName,EmpType,LnCode from v_Personnel_EmpCurrentStatus where SN=" + SN + "", dtPersonalInfo = new DataTable());
                //rpd.SetParameterValue(2, dtPersonalInfo.Rows[0]["Dname"].ToString());
                //rpd.SetParameterValue(3, dtPersonalInfo.Rows[0]["DptName"].ToString());
                //rpd.SetParameterValue(4, dtPersonalInfo.Rows[0]["DsgName"].ToString());
                //rpd.SetParameterValue(5, dtPersonalInfo.Rows[0]["LnCode"].ToString());
                //rpd.SetParameterValue(6, dtPersonalInfo.Rows[0]["EmpType"].ToString());
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowIncrementInfo()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//SalaryIncermentReport.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__IncrementInfo__"];
                rpd.SetDataSource(dt);                
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }

        private void ShowPFList()
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//PF_List.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__PFList__"];
                rpd.SetDataSource(dt);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowContactList(string Type)
        {
            try
            {
                rpd = new ReportDocument();
                if (Type == "0")
                    rpd.Load(Server.MapPath("//All Report//Personnel//EmpContact_List.rpt"));
                else
                    rpd.Load(Server.MapPath("//All Report//Personnel//EmpEmergency Contact_List.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__ContacList__"];
                rpd.SetDataSource(dt);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowContactListBangla(string Type)
        {
            try
            {
                rpd = new ReportDocument();
                if (Type == "0")
                    rpd.Load(Server.MapPath("//All Report//Personnel//EmpContact_ListBangla.rpt"));
                else
                    rpd.Load(Server.MapPath("//All Report//Personnel//EmpEmergency Contact_ListBangla.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__ContacListBangla__"];
                rpd.SetDataSource(dt);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void ShowBankOrCashReport(string Type)
        {
            try
            {
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Personnel//SalaryBankOrCash.rpt"));
                dt = new DataTable();
                dt = (DataTable)Session["__BankOrCash__"];
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0,Type);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void loadWorkerBonusSheet(string month, string year, string type, string empType)
        {
            try
            {

                dt = new DataTable();
                dt = (DataTable)Session["__WorkerBonusSheet__"];

                rpd = new ReportDocument();
                if (Session["__Language__"].ToString().Equals("Bangal")) rpd.Load(Server.MapPath("//All Report//Payroll//worker_bonus_sheet.rpt"));

                else rpd.Load(Server.MapPath("//All Report//Payroll//worker_bonus_sheet.rpt"));
                //rpd.FileName=Server.MapPath("//All Report//Payroll//WorkerMonthlySalarySheet.rpt");
                rpd.SetDataSource(dt);
                loadGarmentsInfo();   // for get company info




                string getMonthName = new DateTime(int.Parse(year), int.Parse(month), 1).ToString("MMM", CultureInfo.InvariantCulture);
                string monthName = " ";
                monthName = getMonthName + "-" + year + "(" + type + ")";

                if (Session["__Language__"].ToString().Equals("Bangal"))
                {
                    rpd.SetParameterValue(0, dtGramentsInfo.Rows[0]["CompanyNameBangla"].ToString());
                    rpd.SetParameterValue(1, dtGramentsInfo.Rows[0]["AddressBangla"].ToString());

                    rpd.SetParameterValue(2, monthName);
                    rpd.SetParameterValue(3, "Bonus Sheet Of " + empType);
                }
                else
                {
                    rpd.SetParameterValue(0, dtGramentsInfo.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, dtGramentsInfo.Rows[0]["Address"].ToString());

                    rpd.SetParameterValue(2, monthName);
                    rpd.SetParameterValue(3, "Bonus Sheet Of " + empType);
                }
                Session["__rpd__"] = rpd;
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;

            }
            catch (Exception ex)
            {
            }
        }

        private void ShowFinalBillForm(string EmpJoiningDate, string EarnLeaveDate, string BanMonthName, float stampCardDeduction, string basicSalary,string CompanyId)
        {
            try
            {
                dt = new DataTable();
                dt = (DataTable)Session["__FinalBillForm__"];
                string getEarnLeaveDays = "0";
                double getTotalAmount = 0;
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Payroll//FinalSatelmentBill.rpt"));
                rpd.SetDataSource(dt);

                double getTotalOverTimeAmount = (double.Parse(dt.Rows[0]["OTRate"].ToString()) * int.Parse(dt.Rows[0]["TotalOTHour"].ToString()));
                double getTotalPayableSalary = double.Parse(dt.Rows[0]["Payable"].ToString()) + getTotalOverTimeAmount;

                DataTable dtElapsedDays = new DataTable();
                sqlDB.fillDataTable("select Datediff (Day,'" + EarnLeaveDate+ "','" +DateTime.Now.ToString("yyyy-MM-dd")+ "') as ElapsedDays", dtElapsedDays);

                if (int.Parse(dtElapsedDays.Rows[0]["ElapsedDays"].ToString()) >=360)
                {
                    DataTable dtLD = new DataTable();
                    sqlDB.fillDataTable("select LeaveDays from tblLeaveConfig where ShortName='a/l' AND CompanyId='" + CompanyId + "'", dtLD);

                    DataTable dtELDate = new DataTable();
                    sqlDB.fillDataTable("select EmpId,convert(varchar(11),EarnLeaveDate,105) as EarnLeaveDate,Format(EarnLeaveDate,'yyyy-MM-dd') as EarnLeaveDate2 from Personnel_EmpCurrentStatus where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "'", dtELDate);

                    DataTable dtALAmount = new DataTable();
                    sqlDB.fillDataTable("select StateStatus from tblAttendanceRecord where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' AND  ATTDate >='" + dtELDate.Rows[0]["EarnLeaveDate2"].ToString() + "' AND ATTDate<='" + DateTime.Now.ToString("yyyy-MM-dd") + "' AND StateStatus='Annual Leave'", dtALAmount);
                                


                    //sqlDB.fillDataTable("select TotalDays from v_EarnLeaveGenerateAccount where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "'", dt=new DataTable ());

                   // sqlDB.fillDataTable("select MAX(Sn),TotalDays,ActiveSalary from v_EarnLeaveGenerateAccount where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' And ActiveSalary='true'group by TotalDays,ActiveSalary", dt = new DataTable());

                    int getDaysForPayment = int.Parse(dtLD.Rows[0]["LeaveDays"].ToString()) - dtALAmount.Rows.Count;

                  //  getEarnLeaveDays = (getDaysForPayment / 360).ToString();
                  //  getEarnLeaveDays = Math.Round(double.Parse(getEarnLeaveDays) * int.Parse(dt.Rows[0]["TotalDays"].ToString()), 0).ToString();
                  //  getEarnLeaveDays = Math.Round(double.Parse(getEarnLeaveDays), 0).ToString();
                    getTotalAmount = Math.Round(double.Parse(basicSalary) * int.Parse(getDaysForPayment.ToString()) / 30, 0);    // this calculation for yearly earn leave generation and whose are currently Iregular
                }

                getTotalPayableSalary = getTotalPayableSalary + getTotalAmount - stampCardDeduction;

                DataTable dtcompany = new DataTable();
                sqlDB.fillDataTable("Select CompanyNameBangla,AddressBangla From HRD_CompanyInfo", dtcompany);
                rpd.SetParameterValue(0, dtcompany.Rows[0]["CompanyNameBangla"].ToString());
                rpd.SetParameterValue(1, dtcompany.Rows[0]["AddressBangla"].ToString());
                rpd.SetParameterValue(2, BanMonthName);
                rpd.SetParameterValue(3, Math.Round(double.Parse(getEarnLeaveDays), 0));
                rpd.SetParameterValue(4, Math.Round(getTotalAmount, 0));
                rpd.SetParameterValue(5, stampCardDeduction);

                rpd.SetParameterValue(6, Math.Round(getTotalPayableSalary, 0));
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }

        }

        private void loadWorkerSeperationSheet(string month, string year)
        {
            try
            {


                dt = new DataTable();
                dt = (DataTable)Session["__WorkerSalarySheet__"];

                rpd = new ReportDocument();
                if (Session["__Language__"].ToString().Equals("Bangal")) rpd.Load(Server.MapPath("//All Report//Payroll//WorkerSeperationSheetBangla.rpt"));

                else rpd.Load(Server.MapPath("//All Report//Payroll//WorkerMonthlySalarySheet.rpt"));
                //rpd.FileName=Server.MapPath("//All Report//Payroll//WorkerMonthlySalarySheet.rpt");
                rpd.SetDataSource(dt);
                loadGarmentsInfo();   // for get company info
                if (Session["__Language__"].ToString().Equals("Bangal"))
                {
                    rpd.SetParameterValue(0, dtGramentsInfo.Rows[0]["CompanyNameBangla"].ToString());
                    rpd.SetParameterValue(1, dtGramentsInfo.Rows[0]["AddressBangla"].ToString());

                    rpd.SetParameterValue(2, "gvwmK ‡mcv‡ikb eB-" + month);
                    rpd.SetParameterValue(3, year);
                }
                else
                {
                    rpd.SetParameterValue(0, dtGramentsInfo.Rows[0]["CompanyName"].ToString());
                    rpd.SetParameterValue(1, dtGramentsInfo.Rows[0]["Address"].ToString());

                    rpd.SetParameterValue(2, "Monthly Seperation Sheet-" + month);
                    rpd.SetParameterValue(3, year);
                }
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;




                //CrystalReportViewer1.GroupTreeStyle = false;

                //gvAdvaceList.DataSource = dt;
                //gvAdvaceList.DataBind();
            }
            catch { }
        }

        private void loadAdvanceInfo()
        {
            try
            {


                dt = new DataTable();
                dt = (DataTable)Session["__AdvanceInfo__"];

                string advanceTitle = "";
                //string EmpType= (dt.Rows[0]["EmpTypeId"].ToString()=="1")?"Workers":"Staffs";

                rpd = new ReportDocument();




                if (Session["__rptType__"].ToString() == "Last Advance Info Summary For All Emp" || Session["__rptType__"].ToString() == "Last Advance Info Summary For Certain Emp")
                {
                    rpd.Load(Server.MapPath("//All Report//Payroll//LastAdvanceSummary.rpt"));
                }
                else if (Session["__rptType__"].ToString() == "All Advance Info Summary For All Emp" || Session["__rptType__"].ToString() == "All Advance Info Summary For Certain Emp")
                {
                    rpd.Load(Server.MapPath("//All Report//Payroll//AllAdvanceSumaaryGrpCardNo.rpt"));
                }
                rpd.SetDataSource(dt);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;





            }
            catch { }
        }

        private void loadLoanInfo()
        {
            try
            {


                dt = new DataTable();
                dt = (DataTable)Session["__LoanInfo__"];

                string advanceTitle = "";
                string EmpType = (dt.Rows[0]["EmpTypeId"].ToString() == "1") ? "Workers" : "Staffs";

                rpd = new ReportDocument();


                if (Session["__rptType__"].ToString() == "Certain Last Loan Info Summary")
                {
                    rpd.Load(Server.MapPath("//All Report//Payroll//IndividualLoanSummary.rpt"));
                    advanceTitle = "Last advance summary of this " + EmpType;
                }
                else if (Session["__rptType__"].ToString() == "Certain Emp All Loan Info Summary" || Session["__rptType__"].ToString().Equals("All Last Loan Info Summary"))
                {
                    // All Emp Last Loan Info Summary ,is same of Certain Emp All Loan Info Summary 

                    rpd.Load(Server.MapPath("//All Report//Payroll//CertainEmpAllLoanSummary.rpt"));
                    if (Session["__rptType__"].ToString() == "Certain Emp All Loan Info Summary")
                    {

                        advanceTitle = "All Loan summary of this " + EmpType;
                    }
                    else
                    {
                        advanceTitle = "Last Loan summary of  all " + EmpType;
                    }

                }
                else if (Session["__rptType__"].ToString().Equals("OverAll Loan Info Summary"))
                {
                    rpd.Load(Server.MapPath("//All Report//Payroll//AllLoanSumaaryGrpCardNo.rpt"));
                    advanceTitle = "All Loan summary of all " + EmpType;
                }

                else if (Session["__rptType__"].ToString() == "Certain Loan Info Details")
                {
                    rpd.Load(Server.MapPath("//All Report//Payroll//IndividualLoanDetails.rpt"));

                    DataTable dtSubRpt = new DataTable();
                    dtSubRpt = (DataTable)Session["__ForSubReport__"];
                    ReportDocument subReport = rpd.Subreports[0];
                    rpd.Subreports[0].SetDataSource(dtSubRpt);

                    advanceTitle = "Last Loan details of this " + EmpType;
                }

                else if (Session["__rptType__"].ToString() == "Certain All Loan Info Details")
                {
                    rpd.Load(Server.MapPath("//All Report//Payroll//IndividualAllLoanDetails.rpt"));

                    DataTable dtSubRpt = new DataTable();
                    dtSubRpt = (DataTable)Session["__ForSubReport__"];
                    ReportDocument subReport = rpd.Subreports[0];
                    rpd.Subreports[0].SetDataSource(dtSubRpt);

                    advanceTitle = "All Loan details of this " + EmpType;
                }

                else if (Session["__rptType__"].ToString() == "All Emp Last Loan Details")
                {
                    rpd.Load(Server.MapPath("//All Report//Payroll//AllLastLoanDetails.rpt"));

                    DataTable dtSubRpt = new DataTable();
                    dtSubRpt = (DataTable)Session["__ForSubReport__"];
                    ReportDocument subReport = rpd.Subreports[0];
                    rpd.Subreports[0].SetDataSource(dtSubRpt);

                    advanceTitle = "Last loan details of all " + EmpType;
                }

                else if (Session["__rptType__"].ToString() == "All Emp All Loan Details")
                {
                    rpd.Load(Server.MapPath("//All Report//Payroll//AllEmpAllLoanDetails.rpt"));

                    DataTable dtSubRpt = new DataTable();
                    dtSubRpt = (DataTable)Session["__ForSubReport__"];
                    ReportDocument subReport = rpd.Subreports[0];
                    rpd.Subreports[0].SetDataSource(dtSubRpt);


                    advanceTitle = "All loan details of all " + EmpType;
                }

                //rpd.FileName=Server.MapPath("//All Report//Payroll//WorkerMonthlySalarySheet.rpt");

                rpd.SetDataSource(dt);

                loadGarmentsInfo();   // for get company info


                rpd.SetParameterValue(0, dtGramentsInfo.Rows[0]["CompanyName"].ToString());
                rpd.SetParameterValue(1, dtGramentsInfo.Rows[0]["Address"].ToString());
                rpd.SetParameterValue(2, advanceTitle);

                // rpd.SetParameterValue(2, "Monthly Seperation Sheet-" + month);
                //rpd.SetParameterValue(3, year);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;




                //CrystalReportViewer1.GroupTreeStyle = false;

                //gvAdvaceList.DataSource = dt;
                //gvAdvaceList.DataBind();
            }
            catch { }
        }

        private void loadVoucherInfo()
        {
            try
            {


                dt = new DataTable();
                dt = (DataTable)Session["__voucherInfo__"];

                rpd = new ReportDocument();


                rpd.Load(Server.MapPath("//All Report//Leave//MaternityVoucher.rpt"));

                //rpd.SetDataSource(dt);
                loadGarmentsInfo();   // for get company info

                rpd.SetParameterValue(0, dt.Rows[0]["EmpNameBn"].ToString());
                rpd.SetParameterValue(1, dt.Rows[0]["DsgNameBn"].ToString());
                rpd.SetParameterValue(2, dt.Rows[0]["EmpCardNo"].ToString());
                rpd.SetParameterValue(3, dt.Rows[0]["DptNameBn"].ToString());
                rpd.SetParameterValue(4, dt.Rows[0]["ThreeMonthsTotalPaymentWithBonus"].ToString());
                rpd.SetParameterValue(5, dt.Rows[0]["TotalPresentDays"].ToString());

                // rpd.SetParameterValue(4,5000);
                // rpd.SetParameterValue(5,10);

                rpd.SetParameterValue(6, 0);
                sqlDB.fillDataTable("select * from v_Leave_MaterintyVoucher_Details where MLVoucherNo='" + dt.Rows[0]["MLVoucherNo"].ToString() + "'", dt = new DataTable());
                if (dt.Rows.Count > 2)
                {


                    rpd.SetParameterValue(7, dt.Rows[2]["MonthId"].ToString());
                    rpd.SetParameterValue(8, dt.Rows[1]["MonthId"].ToString());
                    rpd.SetParameterValue(9, dt.Rows[0]["MonthId"].ToString());

                    rpd.SetParameterValue(10, dt.Rows[2]["PresentDays"].ToString());
                    rpd.SetParameterValue(11, dt.Rows[1]["PresentDays"].ToString());
                    rpd.SetParameterValue(12, dt.Rows[0]["PresentDays"].ToString());

                    rpd.SetParameterValue(13, dt.Rows[2]["TakenWages"].ToString());
                    rpd.SetParameterValue(14, dt.Rows[1]["TakenWages"].ToString());
                    rpd.SetParameterValue(15, dt.Rows[0]["TakenWages"].ToString());


                    

                }
                else
                {
                    rpd.SetParameterValue(7, 0);
                    rpd.SetParameterValue(8, 0);
                    rpd.SetParameterValue(9, 0);

                    rpd.SetParameterValue(10, 0);
                    rpd.SetParameterValue(11, 0);
                    rpd.SetParameterValue(12, 0);

                    rpd.SetParameterValue(11, 0);
                    rpd.SetParameterValue(12, 0);
                    rpd.SetParameterValue(13, 0);
                }
                rpd.SetParameterValue(16, dt.Rows[0]["CompanyNameBangla"]);
                rpd.SetParameterValue(17, dt.Rows[0]["AddressBangla"]);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;




                //CrystalReportViewer1.GroupTreeStyle = false;


            }
            catch { }
        }

        private void loadVoucherDetails()
        {
            try
            {


                //dt = new DataTable();
                //dt = (DataTable)Session["__voucherInfo__"];
                DataTable dtDetails = (DataTable)Session["__VoucherDetails__"];
                rpd = new ReportDocument();


                rpd.Load(Server.MapPath("//All Report//Leave//MaternityVoucherDetails.rpt"));

                rpd.SetDataSource(dtDetails);
                //  loadGarmentsInfo();   // for get company info               
                rpd.SetParameterValue(0, dtDetails.Rows[0]["MLVoucherNo"]);
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;                
                //CrystalReportViewer1.GroupTreeStyle = false;
            }
            catch { }
        }
        private void LeaveYearlySummary(string Year)
        {
            try
            {
                dt = new DataTable();
                dt = (DataTable)Session["__LeaveYearlySummary__"];
                DataTable dtLeave = new DataTable();
                dtLeave = (DataTable)Session["__dtLeave__"];
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Leave//LeaveBalanceReport_SG.rpt"));
                rpd.SetDataSource(dt);



                rpd.SetParameterValue(0, Year);


                //rpd.SetParameterValue(1, dtLeave.Rows[0]["LeaveDays"].ToString());//CL
                //rpd.SetParameterValue(2, dtLeave.Rows[1]["LeaveDays"].ToString());//SL  
                //rpd.SetParameterValue(3, dtLeave.Rows[2]["LeaveDays"].ToString());//EL
                //rpd.SetParameterValue(4, dtLeave.Rows[3]["LeaveDays"].ToString());//ML
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;






            }
            catch { }
        }
        private void LeaveYearlySummaryIndividualDetails(string Year)
        {
            try
            {
                dt = new DataTable();
                dt = (DataTable)Session["__LeaveYearlySummaryIndividualDetails__"];
                DataTable dtLeave = new DataTable();
                dtLeave = (DataTable)Session["__dtLeave__"];
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Leave//LeaveBalanceReportIndividualDetails_SG.rpt"));
                rpd.SetDataSource(dt);



                rpd.SetParameterValue(0, Year);


                rpd.SetParameterValue(1, dtLeave.Rows[0]["LeaveDays"].ToString());//CL
                rpd.SetParameterValue(2, dtLeave.Rows[1]["LeaveDays"].ToString());//SL  
                rpd.SetParameterValue(3, dtLeave.Rows[2]["LeaveDays"].ToString());//EL
                rpd.SetParameterValue(4, dtLeave.Rows[3]["LeaveDays"].ToString());//ML
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;






            }
            catch { }
        }
        private void loadLeaveBalanceReport()
        {
            try
            {
                dt = new DataTable();
                dt = (DataTable)Session["__LeaveBalanceReport__"];
                DataTable dtLeaveBalance = (DataTable)Session["__VoucherDetails__"];
                rpd = new ReportDocument();


                rpd.Load(Server.MapPath("//All Report//Leave//LeaveBalanceReport.rpt"));

                rpd.SetDataSource(dt);
                // loadGarmentsInfo();   // for get company info

                rpd.SetParameterValue(0, " ");
                rpd.SetParameterValue(1, "Motijil Dhaka");
                rpd.SetParameterValue(2, "01589");

                rpd.SetParameterValue(3, Request.QueryString["for"].ToString().Substring(19, Request.QueryString["for"].ToString().Length - 19));
                // rpd.SetParameterValue(4, dt.Rows.Count);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;




                //CrystalReportViewer1.GroupTreeStyle = false;


            }
            catch { }
        }

        private void loadComapanyPurposeaLeaveReport()
        {
            try
            {
                dt = new DataTable();
                dt = (DataTable)Session["__LeaveBalanceReport__"];
                DataTable dtLeaveBalance = (DataTable)Session["__VoucherDetails__"];
                rpd = new ReportDocument();


                rpd.Load(Server.MapPath("//All Report//Leave//CompanyPurposeLeave.rpt"));

                rpd.SetDataSource(dt);
                // loadGarmentsInfo();   // for get company info

                rpd.SetParameterValue(0, Request.QueryString["for"].ToString().Substring(26, Request.QueryString["for"].ToString().Length - 26));
                // rpd.SetParameterValue(1, "Motijil Dhaka");
                //rpd.SetParameterValue(2, "01589");

                //   rpd.SetParameterValue(3, "From 01-01-2014 To 31-01-2014");
                // rpd.SetParameterValue(4, dt.Rows.Count);

                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;




                //CrystalReportViewer1.GroupTreeStyle = false;


            }
            catch { }
        }
        private void loadYearlyLeaveStatus(string IsIndividual)
        {
            try
            {
                dt = new DataTable();
                dt = (DataTable)Session["__YearlyLeaveStatus__"];

                rpd = new ReportDocument();
                if (IsIndividual == "Yes") rpd.Load(Server.MapPath("//All Report//Leave//LeaveYearlyStatusForIndividual.rpt"));
              else  rpd.Load(Server.MapPath("//All Report//Leave//LeaveYearlyStatus.rpt"));

                rpd.SetDataSource(dt);  
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch { }
        }
        private void loadMonthlyLeaveStatus(string Month)
        {
            try
            {
                dt = new DataTable();
                dt = (DataTable)Session["__MonthlyLeaveStatus__"];
                rpd = new ReportDocument();
                rpd.Load(Server.MapPath("//All Report//Leave//LeaveMonthlyStatusReport1.rpt"));
                rpd.SetDataSource(dt);
                rpd.SetParameterValue(0, Month.Replace('/','-'));
                CrystalReportViewer1.ReportSource = rpd;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
            }
            catch (Exception ex) { }
        }
        //---------------------------------------Start------------------------------------------
        private void loadSummaryOfBonus(string BonusType)
        {
            dt = new DataTable();
            dt = (DataTable)Session["__SummaryOfBonus__"];
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//Payroll//Summary_of_Bonus.rpt"));
            rpd.SetDataSource(dt);
            rpd.SetParameterValue(0, BonusType);
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
        private void loadBonusSheet(string BonusType)
        {

            dt = new DataTable();
            dt = (DataTable)Session["__BonusSheet__"];
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//Payroll//Bonus_Sheet_JIL.rpt"));
            rpd.SetDataSource(dt);
            rpd.SetParameterValue(0, BonusType.Replace('/','-'));
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
        private void loadBonusMissSheet(string BonusType)
        {

            dt = new DataTable();
            dt = (DataTable)Session["__BonusMissSheet__"];
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//Payroll//Bonus_Miss_Sheet.rpt"));
            rpd.SetDataSource(dt);
            rpd.SetParameterValue(0, BonusType);
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
        private void loadSummaryOfSalary(string ReportType,string EmpTypeId)
        {
            dt = new DataTable();
            dt = (DataTable)Session["__SummaryOfSalary__"];
            rpd = new ReportDocument();
           // if (ReportType=="0")
            ///{
                if (EmpTypeId=="1")
                rpd.Load(Server.MapPath("//All Report//Payroll//Summary_of_Salary_SF_Worker.rpt"));
                else if (EmpTypeId == "2")
                    rpd.Load(Server.MapPath("//All Report//Payroll//Summary_of_Salary_SF_Staff.rpt"));
            else
                    rpd.Load(Server.MapPath("//All Report//Payroll//Summary_of_Salary_HO.rpt"));
            //}
           
            //else
            //    rpd.Load(Server.MapPath("//All Report//Payroll//Summary_of_Salary.rpt"));
            rpd.SetDataSource(dt);
            rpd.SetParameterValue(0, Session["__SummaryReportTitle__"].ToString());
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
        private void loadSalarySheet(string SelectMonth, string Year, string IsGarments, string EmpTypeId, string PaymentType)
        {

            dt = new DataTable();
            dt = (DataTable)Session["__SalarySheet__"];
            rpd = new ReportDocument();
           
                if (Session["__Language__"].ToString() == "English")
                {
                    if (IsGarments=="False") // This is for Head Office
                        rpd.Load(Server.MapPath("//All Report//Payroll//monthly_salary_sheet_JIL.rpt"));
                    else
                    {
                        if(EmpTypeId=="1")
                            rpd.Load(Server.MapPath("//All Report//Payroll//monthly_salary_sheet_SG_Worker.rpt"));
                        else if(PaymentType=="Cash")
                            rpd.Load(Server.MapPath("//All Report//Payroll//monthly_salary_sheet_SG_Staff_Cash.rpt"));
                        else if(PaymentType=="Bank")
                            rpd.Load(Server.MapPath("//All Report//Payroll//monthly_salary_sheet_SG_Staff_Bank.rpt"));
                        else
                            rpd.Load(Server.MapPath("//All Report//Payroll//monthly_salary_sheet_SG_Staff_Check.rpt"));

                    }         
                    rpd.SetDataSource(dt);
                    rpd.SetParameterValue(0, SelectMonth + "-" + Year);
                    rpd.SetParameterValue(1,Session["__ReportTitle__"].ToString());
                }
                else
                {
                    rpd.Load(Server.MapPath("//All Report//Payroll//monthly_salary_sheet_new_Bangla.rpt"));
                    rpd.SetDataSource(dt);
                    rpd.SetParameterValue(0, SelectMonth);
                    rpd.SetParameterValue(1, Year);
                   // rpd.SetParameterValue(1, Session["__ReportTitle__"].ToString());
                }
            //}
           
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
        private void loadSalarySheetBankForwardingLetter()
        {

            dt = new DataTable();
            dt = (DataTable)Session["__SalarySheetBankForwardingLetter__"];
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//Payroll//monthly_salary_bank_forwarding.rpt"));
            rpd.SetDataSource(dt);            
           
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
        private void loadBonusSheetBankForwardingLetter()
        {

            dt = new DataTable();
            dt = (DataTable)Session["__BonusSheetBankForwardingLetter__"];
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//Payroll//bonus_sheet_bank_forwarding.rpt"));
            rpd.SetDataSource(dt);

            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
        private void PrintMobileCell()
        {

            dt = new DataTable();
            dt = (DataTable)Session["__PrintMobileCell__"];
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//Payroll//MonthlyDeduction.rpt"));
            rpd.SetDataSource(dt);
            rpd.SetParameterValue(0,"Mobile Cell Report");
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
        private void PrintAdvance()
        {

            dt = new DataTable();
            dt = (DataTable)Session["__PrintAdvance__"];
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//Payroll//MonthlyDeduction.rpt"));
            rpd.SetDataSource(dt);
            rpd.SetParameterValue(0, "Advance Report");
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
        private void loadLeaveConfig()
        {
           
            dt = new DataTable();
            dt = (DataTable)Session["__LeaveConfig__"];
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//Leave//leave_configReport.rpt"));
            rpd.SetDataSource(dt);
            rpd.SetParameterValue(0, DateTime.Now.Year);
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }

        private void loadLeaveApplicationReport()
        {

            rpd = new ReportDocument();

            rpd.Load(Server.MapPath("//All Report//Leave//LeaveApplicationReport.rpt"));

            dt = new DataTable();
            dt = (DataTable)Session["__LeaveApplication__"];

            string getSQLCMD = "select Format(FromDate,'dd-MM-yyyy') as FromDate,Format(ToDate,'dd-MM-yyyy') as ToDate ,TotalDays,ApprovedRejected "
                + " from"
                + " v_Leave_LeaveApplication_Log"
                + " where LACode='" + dt.Rows[0]["LACode"].ToString() + "'";

            DataTable dtRequestedDate = new DataTable();
            sqlDB.fillDataTable(getSQLCMD, dtRequestedDate);
            DataTable dtLeaveStatus = new DataTable();
            string getSQLCMD1 = " select  vld.ShortName,COUNT(vld.ShortName) as Amount,tbc.LeaveDays,tbc.LeaveDays-COUNT(vld.ShortName) as Remaining,tbc.CompanyId "
                + " from v_Leave_LeaveApplicationDetails as vld Inner join tblLeaveConfig as tbc on vld.LeaveId=tbc.LeaveId AND vld.EmpId='" + dt.Rows[0]["EmpId"].ToString() + "'"
                + " AND vld.FromYear='" + DateTime.Now.Year.ToString() + "' AND tbc.CompanyId='" + dt.Rows[0]["CompanyId"].ToString() + "' group by vld.ShortName,tbc.LeaveDays,tbc.CompanyId ";
            sqlDB.fillDataTable(getSQLCMD1, dtLeaveStatus);
          
            rpd.SetDataSource(dt);

            if (dtRequestedDate.Rows.Count == 0)
            {
                rpd.SetParameterValue(11, dt.Rows[0]["FromDate"].ToString());
                rpd.SetParameterValue(12, dt.Rows[0]["ToDate"].ToString());
                rpd.SetParameterValue(13, dt.Rows[0]["TotalDays"].ToString());

                rpd.SetParameterValue(18, " ");
                rpd.SetParameterValue(19," ");
                rpd.SetParameterValue(20, " ");
                rpd.SetParameterValue(21,"Leave Application : Pending");


            }
            else
            {
                if (dtRequestedDate.Rows[0]["ApprovedRejected"].ToString().Equals("Rejected"))
                {
                    rpd.SetParameterValue(11, dtRequestedDate.Rows[0]["FromDate"].ToString());
                    rpd.SetParameterValue(12, dtRequestedDate.Rows[0]["ToDate"].ToString());
                    rpd.SetParameterValue(13, dtRequestedDate.Rows[0]["TotalDays"].ToString());

                    rpd.SetParameterValue(18, " ");
                    rpd.SetParameterValue(19, " ");
                    rpd.SetParameterValue(20, " ");
                    rpd.SetParameterValue(21, "Leave Application : Rejected");
                }
                else
                {
                    rpd.SetParameterValue(11, dtRequestedDate.Rows[0]["FromDate"].ToString());
                    rpd.SetParameterValue(12, dtRequestedDate.Rows[0]["ToDate"].ToString());
                    rpd.SetParameterValue(13, dtRequestedDate.Rows[0]["TotalDays"].ToString());

                    rpd.SetParameterValue(18,dt.Rows[0]["FromDate"].ToString());
                    rpd.SetParameterValue(19,dt.Rows[0]["ToDate"].ToString());
                    rpd.SetParameterValue(20, dt.Rows[0]["TotalDays"].ToString());
                    rpd.SetParameterValue(21, "Leave Application : Approved");
                }

            }

            rpd.SetParameterValue(0, "HR Department");
            rpd.SetParameterValue(1, "Dhaka,Bangladesh");

            DataTable dtLeaveConfig = new DataTable();

            sqlDB.fillDataTable("select * from tblLeaveConfig where CompanyId='"+dt.Rows[0]["CompanyId"].ToString()+"'",dtLeaveConfig);

            try
            {
                DataRow [] dr = dtLeaveStatus.Select("ShortName='c/l'",null);

                rpd.SetParameterValue(2, dr[0]["LeaveDays"]);    // for initial all c/l  of this year
                rpd.SetParameterValue(3, dr[0]["Amount"]);      // for used all c/l of this year
                rpd.SetParameterValue(4, dr[0]["Remaining"]);   // for remaining  c/l   of this year
            }
            catch
            {
                rpd.SetParameterValue(2, dtLeaveConfig.Rows[0]["LeaveDays"]);    // for initial all c/l  of this year
                rpd.SetParameterValue(3,"0");      // for used all c/l of this year
                rpd.SetParameterValue(4,"0");   // for remaining  c/l   of this year          
            }

            try
            {
                DataRow[] dr = dtLeaveStatus.Select("ShortName='s/l'", null);
                rpd.SetParameterValue(5, dr[0]["LeaveDays"]);    // for initial all s/l  of this year
                rpd.SetParameterValue(6, dr[0]["Amount"]);      // for used all s/l of this year
                rpd.SetParameterValue(7, dr[0]["Remaining"]);   // for remaining  s/l   of this year
            }
            catch 
            {
                rpd.SetParameterValue(5, dtLeaveConfig.Rows[0]["LeaveDays"]);    // for initial all s/l  of this year
                rpd.SetParameterValue(6, "0");      // for used all s/l of this year
                rpd.SetParameterValue(7, "0");   // for remaining  s/l   of this year       
            }

            try
            {
                DataRow[] dr = dtLeaveStatus.Select("ShortName='a/l'", null);
                rpd.SetParameterValue(8, dr[0]["LeaveDays"]);    // for initial all a/l  of this year
                rpd.SetParameterValue(9, dr[0]["Amount"]);      // for used all a/l of this year
                rpd.SetParameterValue(10, dr[0]["Remaining"]);   // for remaining  a/l   of this year
            }
            catch
            {
                rpd.SetParameterValue(8, dtLeaveConfig.Rows[0]["LeaveDays"]);    // for initial all a/l  of this year
                rpd.SetParameterValue(9, "0");      // for used all a/l of this year
                rpd.SetParameterValue(10, "0");   // for remaining  a/l   of this year       
            }

            if (dt.Rows[0]["ShortName"].ToString().Equals("c/l")) rpd.SetParameterValue(14, "✔");
                else rpd.SetParameterValue(14, " ");

            if (dt.Rows[0]["ShortName"].ToString().Equals("s/l")) rpd.SetParameterValue(15, "✔");
            else rpd.SetParameterValue(15, " ");

            if (dt.Rows[0]["ShortName"].ToString().Equals("a/l")) rpd.SetParameterValue(16, "✔");
            else rpd.SetParameterValue(16, " ");

            if (dt.Rows[0]["ShortName"].ToString().Equals("m/l")) rpd.SetParameterValue(17, "✔");
            else rpd.SetParameterValue(17, " ");

            if (dt.Rows[0]["ShortName"].ToString().Equals("op/l")) rpd.SetParameterValue(22, "✔");
            else rpd.SetParameterValue(22, " ");

            if (dt.Rows[0]["ShortName"].ToString().Equals("o/l")) rpd.SetParameterValue(23, "✔");
            else rpd.SetParameterValue(23, " ");

            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }

        private void loadLeaveApplicationReportForSG()
        {

            string DocLocation="";
            rpd = new ReportDocument();

            rpd.Load(Server.MapPath("//All Report//Leave//Leave Application for Jakson.rpt"));
            dt = new DataTable();
            dt = (DataTable)Session["__LeaveApplication__"];
            rpd.SetDataSource(dt);
            DataTable dtSub = new DataTable();           
            dtSub = (DataTable)Session["__LeaveCurrentStatus__"];
            rpd.Subreports[1].SetDataSource(dtSub);
            //rpd.Subreports[1].SetDataSource(dtSub);
            DataTable dtSignature = new DataTable();
      
          
            if (File.Exists(Server.MapPath("/EmployeeImages/LeaveDocument/" + dt.Rows[0]["LACode"].ToString()+".jpg")))  
                DocLocation=Server.MapPath("//EmployeeImages//LeaveDocument//" + dt.Rows[0]["LACode"].ToString());        
            rpd.SetParameterValue(0, DocLocation);


            sqlDB.fillDataTable("select Leave_SignatureOrder.SL,DateTime,case when Approval=0 then 'Forword' when Approval=1 then 'Approved' when Approval=2 then 'Rejected' end Approval  from Leave_ApprovalLog inner join Leave_SignatureOrder on Leave_ApprovalLog.UserID= Leave_SignatureOrder.UserId and Leave_SignatureOrder.DptId is null where LACode=" + dt.Rows[0]["LACode"].ToString() + " order by Leave_SignatureOrder.SL", dtSignature);
            if (dtSignature.Rows.Count > 0)
            {
                int j = 0;
                for (int i = 1; i < 5; i++)
                {

                    try
                    {
                        if (int.Parse(dtSignature.Rows[j]["SL"].ToString()) > i)
                        {
                            rpd.SetParameterValue(i, "");
                            j--;
                        }
                        else
                            rpd.SetParameterValue(i, dtSignature.Rows[j]["DateTime"].ToString() + "[" + dtSignature.Rows[j]["Approval"].ToString() + "]");

                    }
                    catch { rpd.SetParameterValue(i, ""); }
                    j++;
                }
            }
            else
            {                
                rpd.SetParameterValue(1, "");
                rpd.SetParameterValue(2, "");
                rpd.SetParameterValue(3, "");
                rpd.SetParameterValue(4, "");
            }           
            rpd.SetParameterValue(5, "✔");
            rpd.SetParameterValue(6, "✔");
            rpd.SetParameterValue(7, "✔");
            sqlDB.fillDataTable("select convert(varchar(10), RDate,105) as RDate from Leave_leaveReplacedDateLog where LACode = " + dt.Rows[0]["LACode"].ToString(), dtSignature=new DataTable());
            if (dtSignature.Rows.Count > 0)
            {
                if (dtSignature.Rows.Count > 1)
                {
                    string rDate = "";
                    for (byte i = 0; i < dtSignature.Rows.Count; i++)
                        rDate += "," + dtSignature.Rows[i]["RDate"].ToString();
                    rpd.SetParameterValue(8, rDate.Remove(0,1));
                }
                else
                    rpd.SetParameterValue(8, dtSignature.Rows[0]["RDate"].ToString());

            }
            else
            {
                rpd.SetParameterValue(8, "");
            }
            rpd.SetParameterValue(9, DocLocation);

            // rpd.Subreports[1].SetDataSource("");



            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }

        private void loadshortLeaveApplicationReportForSG()
        {

            rpd = new ReportDocument();

            rpd.Load(Server.MapPath("//All Report//Leave//Short Leave Application for SG.rpt"));

            dt = new DataTable();
            dt = (DataTable)Session["__ShortLeaveApplication__"];
            rpd.SetDataSource(dt);

            DataTable dtSignature = new DataTable();

            sqlDB.fillDataTable("with lso as (select * from Leave_SignatureOrder where DptId = (select distinct DptId from Leave_ApprovalLog where LACode=" + dt.Rows[0]["SrLvID"].ToString() + " and DptId is not null ) or DptId is null ) " +
                "select lso.SL,DateTime,case when Approval=0 then 'Forword' when Approval=1 then 'Approved' when Approval=2 then 'Rejected' end Approval  from Leave_ApprovalLog inner join lso on Leave_ApprovalLog.UserID= lso.UserId  where LACode=" + dt.Rows[0]["SrLvID"].ToString() + " order by lso.SL", dtSignature);
            if (dtSignature.Rows.Count > 0)
            {
                int j = 0;
                for (int i = 0; i < 5; i++)
                {
                    
                    try
                    {
                        if (int.Parse(dtSignature.Rows[j]["SL"].ToString())>i)
                        {
                            rpd.SetParameterValue(i, "");
                            j--;
                        }                            
                        else
                            rpd.SetParameterValue(i,"["+dtSignature.Rows[j]["Approval"].ToString() + "]\r\n" + dtSignature.Rows[j]["DateTime"].ToString());                
                       
                    }
                    catch { rpd.SetParameterValue(i, ""); }
                    j++;
                }
            }
            else
            {
                rpd.SetParameterValue(0, "");
                rpd.SetParameterValue(1, "");
                rpd.SetParameterValue(2, "");
                rpd.SetParameterValue(3, "");
                rpd.SetParameterValue(4, "");
            }
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }

        private void SpeacificEmpDailyAttReport() // For single Date.
        {
            dt = new DataTable();
            dt = (DataTable)Session["__SpeacificEmpAttSummary__"];
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//Attendance//SpeacificEmpAttStatus.rpt"));
            rpd.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
        private void SpeacificEmpDailyAttReport_MultiDate() // For multiple Date.
        {
            dt = new DataTable();
            dt = (DataTable)Session["__SpeacificAttSummaryMultiDate__"];
            rpd = new ReportDocument();
            rpd.Load(Server.MapPath("//All Report//Attendance//SpeacificEmpAttStatusMultipleDate.rpt"));
            rpd.SetDataSource(dt);
            CrystalReportViewer1.ReportSource = rpd;
            CrystalReportViewer1.HasToggleGroupTreeButton = false;
        }
    }
}