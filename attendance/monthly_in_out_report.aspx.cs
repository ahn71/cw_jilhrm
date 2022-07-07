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



namespace SigmaERP.attendance
{
    public partial class monthly_in_out_report : System.Web.UI.Page
    {
        string CompanyId="";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";

            if (!IsPostBack)
            {
                classes.commonTask.LoadEmpTypeWithAll(rblEmpType);
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
                Session["__MinDigits__"] = "6";
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
                ViewState["__EmpId__"] = getCookies["__getEmpId__"].ToString();
                ViewState["__ODOnlyDpt__"] = getCookies["__getODOnlyDpt__"].ToString();
                ViewState["__DptId__"] = getCookies["__getDptId__"].ToString();
                ViewState["__onlyDptId__"] = "";
                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "monthly_in_out_report.aspx", ddlCompanyName, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                classes.commonTask.loadMonthIdByCompany(ddlMonthList, ViewState["__CompanyId__"].ToString());
               
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "User")
                {
                    
                    rblGenerateType.SelectedValue = "1";
                    rblGenerateType.Enabled = false;
                    workerlist.Visible = false;
                    txtCardNo.Text = commonTask.returnEmpCardNoByEmpId(ViewState["__EmpId__"].ToString());
                    txtCardNo.Enabled = false;
                    trEmpType.Visible = false;
                    rblEmpType.Enabled = false;
                    lnkNew.Visible = false;
                }
                else
                {
                    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin" && ViewState["__ODOnlyDpt__"].ToString().Equals("True"))
                    {
                        ViewState["__onlyDptId__"] = " and DptID='" + ViewState["__DptId__"].ToString() + "'";
                        classes.commonTask.LoadDepartment(ViewState["__CompanyId__"].ToString(), ViewState["__DptId__"].ToString(), lstAll);
                    }                        
                    else
                        classes.commonTask.LoadDepartment(ViewState["__CompanyId__"].ToString(), lstAll);
                }
               
            }
            catch { }

        }

  
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveItem(lstAll, lstSelected);
        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveAll(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveAll(lstSelected, lstAll);
        }      

        protected void ddlShiftName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                lstAll.Items.Clear();
                lstSelected.Items.Clear();
                 CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();         
                }
            catch { }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {//------------------------Validation--------------------------------
                if (rblGenerateType.SelectedIndex == 0)
                {
                    if (ddlMonthList.SelectedValue == "0")
                    {
                        lblMessage.InnerText = "warning-> Please Select Any Month!"; ddlMonthList.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                        return;
                    }
                    if (lstSelected.Items.Count == 0)
                    {
                        lblMessage.InnerText = "warning-> Please Select Any Department!"; lstSelected.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                        return;
                    }
                }
                else
                {
                    if (ddlMonthList.SelectedValue == "0")
                    {
                        lblMessage.InnerText = "warning-> Please Select Any Month!"; ddlMonthList.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                        return;
                    }
                    if (txtCardNo.Text.Trim().Length ==0)
                    {
                        lblMessage.InnerText = "warning-> Please Type Valid Card Number!";
                        txtCardNo.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                        return;
                    }
                }
                //----------------------------End-------------------------------
                if (rblReportType.SelectedValue == "3")
                {
                    GenerateJobCardReport();
                }
                else if (rblReportType.SelectedValue == "4")
                {
                    GenerateHolidayAndWeekendReport();
                }
                else
                {
                    if (rblLanguage.SelectedValue == "EN")
                        GenerateReportEnglish();
                    else
                        GenerateReportBangla();
                }
            }
            catch { }
        }
        private void GenerateReportEnglish()
        {
            try
            {
                string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId= " + rblEmpType.SelectedValue + "";
                string CompanyList = "";
                string ShiftList = "";
                string DepartmentList = "";
                string ReportTitle = "";
                string ReportDate = "";

                if (!Page.IsValid)   // If Java script are desible then 
                {
                    lblMessage.InnerText = "erroe->Please Select From Date And To Date"; return;
                }

                CompanyList = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                CompanyList = "in ('" + CompanyList + "')";
                DepartmentList = classes.commonTask.getDepartmentList(lstSelected);




                DataTable dt = new DataTable();

                string[] MY = ddlMonthList.SelectedItem.Value.ToString().Split('-');
                string type = "";

                if (rblReportType.SelectedIndex == 0)
                {
                    dt = classes.BusinessLogic.get_MonthlyLoginLogOutTime(CompanyList, DepartmentList, MY[0], MY[1], rblGenerateType.SelectedIndex, txtCardNo.Text, EmpTypeID, ViewState["__onlyDptId__"].ToString());
                    type = "Log InOut";
                }
                else if (rblReportType.SelectedIndex == 1)
                {
                    dt = classes.BusinessLogic.get_Moanthly_Attendance_Sheet(CompanyList, DepartmentList, MY[0], MY[1], rblGenerateType.SelectedIndex, txtCardNo.Text, EmpTypeID, ViewState["__onlyDptId__"].ToString());
                    type = "Att Status";
                }
                else
                {
                    dt = classes.BusinessLogic.get_Moanthly_Attendance_Sheet_Summary(CompanyList, DepartmentList, MY[0], MY[1], rblGenerateType.SelectedIndex, txtCardNo.Text, EmpTypeID, ViewState["__onlyDptId__"].ToString());
                    type = "Att Summary";
                }

                //  sqlDB.fillDataTable("select SftEndTime from HRD_Shift where SftId=" + ddlShiftName.SelectedItem.Value.ToString() + "", dt);







                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->No Attendance Available";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    return;
                }
                Session["__MonthlyLoginLogoutReport__"] = dt;

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=MonthlyLoginLogoutReport-" + ddlMonthList.SelectedItem.Value.ToString() + "-" + type + "');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
    
        private void GenerateReportBangla()
        {
            try
            {
                string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId= " + rblEmpType.SelectedValue + "";
                string CompanyList = "";
                string ShiftList = "";
                string DepartmentList = "";
                string ReportTitle = "";
                string ReportDate = "";

                if (!Page.IsValid)   // If Java script are desible then 
                {
                    lblMessage.InnerText = "erroe->Please Select From Date And To Date"; return;
                }

                CompanyList = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                CompanyList = "in ('" + CompanyList + "')";
                DepartmentList = classes.commonTask.getDepartmentList(lstSelected);




                DataTable dt = new DataTable();

                string[] MY = ddlMonthList.SelectedItem.Value.ToString().Split('-');
                string type = "";

                if (rblReportType.SelectedIndex == 0)
                {
                    dt = classes.BusinessLogic.get_MonthlyLoginLogOutTimeBangla(CompanyList, DepartmentList, MY[0], MY[1], rblGenerateType.SelectedIndex, txtCardNo.Text, EmpTypeID);
                    type = "Log InOut";
                }
                else if (rblReportType.SelectedIndex == 1)
                {
                    dt = classes.BusinessLogic.get_Moanthly_Attendance_SheetBangla(CompanyList, DepartmentList, MY[0], MY[1], rblGenerateType.SelectedIndex, txtCardNo.Text, EmpTypeID);
                    type = "Att Status";
                }
                else
                {
                    dt = classes.BusinessLogic.get_Moanthly_Attendance_Sheet_SummaryBangla(CompanyList, DepartmentList, MY[0], MY[1], rblGenerateType.SelectedIndex, txtCardNo.Text, EmpTypeID);
                    type = "Att Summary";
                }

                //  sqlDB.fillDataTable("select SftEndTime from HRD_Shift where SftId=" + ddlShiftName.SelectedItem.Value.ToString() + "", dt);







                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->No Attendance Available";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    return;
                }
                Session["__MonthlyLoginLogoutReportBangla__"] = dt;

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=MonthlyLoginLogoutReportBangla-" + classes.commonTask.GenerateBanglaMonthNameMY(ddlMonthList.SelectedValue) + "-" + type + "');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
    

        protected void rblGenerateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                if (rblGenerateType.SelectedIndex == 0) txtCardNo.Enabled = false;
                else { txtCardNo.Enabled = true; txtCardNo.Focus(); }
            }
            catch { }
        }

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
            classes.commonTask.loadMonthIdByCompany(ddlMonthList,CompanyId) ;
            classes.commonTask.LoadDepartment(CompanyId, lstAll);
            lstSelected.Items.Clear();
          //  classes.commonTask.LoadShift(ddlShiftName, CompanyId, ViewState["__UserType__"].ToString());
          
        }
        private void GenerateJobCardReport() 
        {

          
            try
            {

                string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId= " + rblEmpType.SelectedValue + "";
                string DepartmentList = "";
                if(rblGenerateType.SelectedValue=="0")
                DepartmentList = classes.commonTask.getDepartmentList(lstSelected);

                string[] Month = ddlMonthList.SelectedValue.Split('-');

                DataTable dt = new DataTable();
                if (rblGenerateType.SelectedValue == "1") sqlDB.fillDataTable("Select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,SftName,format(ATTDate,'dd-MM-yyyy') as ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,Convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime,OverTime as TotalOverTime,TotalDays From v_tblAttendanceRecord Where CompanyId='" + ddlCompanyName.SelectedValue + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like'%" + txtCardNo.Text.Trim() + "' and MonthName='" + Month[1] + "-" + Month[0] + "' "+ ViewState["__onlyDptId__"].ToString() + " Group By EmpId,EmpCardNo,EmpName,SftName,ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime,OverTime,TotalDays order by  ATTDate  ", dt);
                else sqlDB.fillDataTable("Select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,SftName,format(ATTDate,'dd-MM-yyyy') as ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,Convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime,OverTime as TotalOverTime,TotalDays From v_tblAttendanceRecord Where CompanyId='" + ddlCompanyName.SelectedValue + "' and MonthName='" + Month[1] + "-" + Month[0] + "' and DptId " + DepartmentList + " " + EmpTypeID + " Group By EmpId,EmpCardNo,EmpName,SftName,ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime,OverTime,TotalDays,GId,CustomOrdering  Order By convert(int,DptId), CustomOrdering,Empid, ATTDate   ", dt);
                Session["__dtJobCard__"] = dt;
                if (dt.Rows.Count > 0)
                {
                    DataTable dtSummary = new DataTable();
                    if (rblGenerateType.SelectedValue == "1") sqlDB.fillDataTable("Select EmpId,SUM(CASE WHEN StateStatus = 'Absent' THEN 1 ELSE 0 END) AS 'Absent',SUM(CASE WHEN StateStatus = 'C/L' THEN 1 ELSE 0 END) AS 'CL',SUM(CASE WHEN StateStatus = 'S/L' THEN 1 ELSE 0 END) AS 'SL',SUM(CASE WHEN StateStatus = 'M/L' THEN 1 ELSE 0 END) AS 'ML',SUM(CASE WHEN StateStatus = 'E/L' THEN 1 ELSE 0 END) AS 'EL',SUM(CASE WHEN StateStatus = 'Holiday' THEN 1 ELSE 0 END) AS 'Holiday',SUM(CASE WHEN StateStatus = 'Present' THEN 1 ELSE 0 END) AS 'Present',SUM(CASE WHEN StateStatus = 'Weekend' THEN 1 ELSE 0 END) AS 'Weekend',Sum(PaybleDays) AS 'APday' From v_tblAttendanceRecord Where CompanyId='" + ddlCompanyName.SelectedValue + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like'%" + txtCardNo.Text.Trim() + "' and MonthName='" + Month[1] + "-" + Month[0] + "' " + ViewState["__onlyDptId__"].ToString() + "  group by EmpId", dtSummary);
                    else sqlDB.fillDataTable("Select EmpId,SUM(CASE WHEN StateStatus = 'Absent' THEN 1 ELSE 0 END) AS 'Absent',SUM(CASE WHEN StateStatus = 'C/L' THEN 1 ELSE 0 END) AS 'CL',SUM(CASE WHEN StateStatus = 'S/L' THEN 1 ELSE 0 END) AS 'SL',SUM(CASE WHEN StateStatus = 'M/L' THEN 1 ELSE 0 END) AS 'ML',SUM(CASE WHEN StateStatus = 'E/L' THEN 1 ELSE 0 END) AS 'EL',SUM(CASE WHEN StateStatus = 'Holiday' THEN 1 ELSE 0 END) AS 'Holiday',SUM(CASE WHEN StateStatus = 'Present' THEN 1 ELSE 0 END) AS 'Present',SUM(CASE WHEN StateStatus = 'Weekend' THEN 1 ELSE 0 END) AS 'Weekend',Sum(PaybleDays) AS 'APday' From  v_tblAttendanceRecord Where CompanyId='" + ddlCompanyName.SelectedValue + "' " + EmpTypeID + " and MonthName='" + Month[1] + "-" + Month[0] + "' and DptId " + DepartmentList + " group by EmpId", dtSummary);
                    Session["__dtSummary__"] = dtSummary;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=JobCardReport');", true);  //Open New Tab for Sever side code         
                }
                else
                {
                    lblMessage.InnerText = "warning->No Attendance Available";
                }
            }
            catch { }
        }

        private void GenerateHolidayAndWeekendReport()
        {


            try
            {

                string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId= " + rblEmpType.SelectedValue + "";
                string DepartmentList = "";
                if (rblGenerateType.SelectedValue == "0")
                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);

                string[] Month = ddlMonthList.SelectedValue.Split('-');

                DataTable dt = new DataTable();
                if (rblGenerateType.SelectedValue == "1") sqlDB.fillDataTable("Select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,SftName,format(ATTDate,'dd-MM-yyyy') as ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,Convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime, TotalOverTime,TotalDays From v_tblAttendanceRecord Where CompanyId='" + ddlCompanyName.SelectedValue + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like'%" + txtCardNo.Text.Trim() + "' and MonthName='" + Month[1] + "-" + Month[0] + "' and (ATTStatus ='W' or ATTStatus='H') " + ViewState["__onlyDptId__"].ToString() + " Group By EmpId,EmpCardNo,EmpName,SftName,ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,TotalOverTime,DptId,StateStatus,EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime,OverTime,TotalDays order by  ATTDate  ", dt);
                else sqlDB.fillDataTable("Select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,SftName,format(ATTDate,'dd-MM-yyyy') as ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,Convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime,TotalOverTime,TotalDays From v_tblAttendanceRecord Where CompanyId='" + ddlCompanyName.SelectedValue + "' and MonthName='" + Month[1] + "-" + Month[0] + "' and DptId " + DepartmentList + " " + EmpTypeID + "  and (ATTStatus ='W' or ATTStatus='H') Group By EmpId,EmpCardNo,EmpName,SftName,ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime,TotalOverTime,TotalDays,GId,CustomOrdering  Order By convert(int,DptId),CustomOrdering,Empid, ATTDate  ", dt);
                Session["__dtWHStatus__"] = dt;
                if (dt.Rows.Count > 0)
                {                   
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=HolidayAndWeekendStatus');", true);  //Open New Tab for Sever side code         
                }
                else
                {
                    lblMessage.InnerText = "warning->No Attendance Available";
                }
            }
            catch { }
        }

      
       

    

  

        

    }
}