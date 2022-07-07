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
    public partial class summary_of_salary : System.Web.UI.Page
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
                ViewState["__IsGerments__"] = classes.commonTask.IsGarments();
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

                //------------load privilege setting inof from db------
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "summary_of_salary.aspx", ddlCompanyName, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                classes.commonTask.LoadDepartmentByCompanyInListBox(ViewState["__CompanyId__"].ToString(), lstAll);
                classes.Payroll.loadMonthIdByCompany(ddlMonth, ViewState["__CompanyId__"].ToString());
                //-----------------------------------------------------
            }
            catch { }
        }
        private void addAllTextInShift()
        {
            if (ddlShiftName.Items.Count > 2)
                ddlShiftName.Items.Insert(1, new ListItem("All", "00"));
        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
           
            if (ddlMonth.SelectedValue == "0") { lblMessage.InnerText = "warning->Please select any Month!"; ddlMonth.Focus(); return; }
            if (lstSelected.Items.Count < 1) { lblMessage.InnerText = "warning->Please select any Department"; lstSelected.Focus(); return; }
            generateSalarySummary();
        }

        private void generateSalarySummary()
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
                   
                    //if (ddlShiftName.SelectedItem.ToString().Equals("All"))
                    //{

                    ////    ShiftList = classes.Payroll.getSftIdList(ddlShiftName);
                    //    DepartmentList = classes.commonTask.getDepartmentList();
                    //}
                    //else
                    //{
                    //    ShiftList = ddlShiftName.SelectedValue.ToString();
                    //    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                    //}
                   
                }
                CompanyList = (ddlCompanyName.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                string getSQLCMD;
                DataTable dt = new DataTable();
                string EmpType = rblEmployeeType.SelectedValue;
           
                    if (!bool.Parse(ViewState["__IsGerments__"].ToString()))
                {
                    EmpType = ViewState["__IsGerments__"].ToString();
                    getSQLCMD = "SELECT  DptName, CompanyId, CompanyName, Address,sum(round(EmpPresentSalary,0)) as EmpPresentSalary, Address,"+
                        " sum(round(TotalSalary,0)) as TotalSalary,  sum(round(TiffinBillAmount,0)) as TiffinBillAmount,sum(round(HoliDayBillAmount,0)) as HoliDayBillAmount, sum(round(ProvidentFund,0)) as ProvidentFund," +
                        " sum(round(ProfitTax,0)) as ProfitTax,sum(round(AbsentDeduction,0)) as AbsentDeduction,sum(round(AdvanceDeduction,0)) as AdvanceDeduction,"+
                        " sum(round(LateFine,0)) as LateFine,sum(round(OthersPay,0)) as OthersPay, sum(round(OthersDeduction,0)) as OthersDeduction "+
                        " FROM  v_MonthlySalarySheet " +
                        " where IsActive='1' and  CompanyId='" + CompanyList + "' and DptId " + DepartmentList + "  AND YearMonth='" + ddlMonth.SelectedValue + "' AND IsSeperationGeneration='" + rblSheet.SelectedValue + "' " +
                        " group By DptId ,DptName, CompanyId, CompanyName, Address "+
                        " ORDER BY convert(int,DptId)";
                }
                   
               else if(rblReportType.SelectedValue=="0")
                        getSQLCMD = "SELECT Count(EmpId) as EmpID, sum(round(TotalSalary,0)) as TotalSalary, sum(round(Payable,0)) as Payable, sum(round(TotalOTAmount,0)) as TotalOTAmount, sum(EmpPresentSalary) as EmpPresentSalary," +
                        " sum(AttendanceBonus) as AttendanceBonus,sum(TiffinBillAmount) as TiffinBillAmount,sum(DormitoryRent) as DormitoryRent,sum(round(HoliDayBillAmount,0)) as HoliDayBillAmount,sum(round(ProvidentFund,0)) as ProvidentFund,sum(round(ProfitTax,0)) as ProfitTax," +
                        "DptName, DptId,CompanyId,GName,CompanyName, Address " +
                        " FROM  v_MonthlySalarySheet "+
                        " where IsActive='1' and CompanyId='" + CompanyList + "' and DptId " + DepartmentList + " and  EmpTypeId=" + rblEmployeeType.SelectedValue + " And SalaryCount='" + rblPaymentType.SelectedValue + "' AND YearMonth='" + ddlMonth.SelectedValue + "' AND IsSeperationGeneration='" + rblSheet.SelectedValue + "' " +
                        " group by GId,DptId,CompanyId,DptName,GName,CompanyName, Address "+
                        " ORder by convert(int,DptId),convert(int,Gid) ";
               else
                    getSQLCMD = "select CompanyId,CompanyName,Address,DptId,DptCode,DptName,sum(TotalSalary)as TotalSalary from v_MonthlySalarySheet " +
                 "where IsActive='1' and CompanyId='" + CompanyList + "' AND YearMonth='" + ddlMonth.SelectedValue + "' and DptId " + DepartmentList + " " +
                 "group by  CompanyId,CompanyName,Address,DptId,DptCode,DptName " +
                 "order by convert(int,DptCode).convert(int,Gid) ";
                sqlDB.fillDataTable(getSQLCMD, dt);
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                }
                string seperation = (rblSheet.SelectedValue == "0") ? "" : "[ Seperation ]";
                     if (!bool.Parse(ViewState["__IsGerments__"].ToString()))
                         Session["__SummaryReportTitle__"] = "Salary Top Sheet For  " + ddlMonth.SelectedItem.Text + "   " + seperation + "";
                else if (rblEmployeeType.SelectedValue == "1")
                         Session["__SummaryReportTitle__"] = "Summary of Wages,O.T,Tiffin Allowance & Attendance Bonus Sheet for the Month of " + ddlMonth.SelectedItem.Text + "   " + seperation + " ";
                else if (rblPaymentType.SelectedValue == "Cash")
                         Session["__SummaryReportTitle__"] = "Summary of Salary,Holiday Allowance, Tiffin Allowance & Attendance Bonus Sheet for the Month of " + ddlMonth.SelectedItem.Text + " (Cash Portion)   " + seperation + "";
                else if (rblPaymentType.SelectedValue == "Bank")
                         Session["__SummaryReportTitle__"] = "Summary of Salary,Holiday Allowance, Tiffin Allowance & Attendance Bonus Sheet for the Month of " + ddlMonth.SelectedItem.Text + " (Fund Transfer Portion)   " + seperation + "";
                else
                         Session["__SummaryReportTitle__"] = "Summary of Salary,Holiday Allowance, Tiffin Allowance & Attendance Bonus Sheet for the Month of " + ddlMonth.SelectedItem.Text + " (Check Portion)   " + seperation + "";
                Session["__Language__"] = "English";
                Session["__SummaryOfSalary__"] = dt;
               
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=SummaryOfSalary-" + rblReportType.SelectedValue + "-" + EmpType + "');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }

        protected void ddlShiftName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {              
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

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lstSelected.Items.Clear();
                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                classes.commonTask.LoadDepartmentByCompanyInListBox(CompanyId, lstAll);

                //classes.Payroll.loadMonthIdByCompany(ddlSelectMonth, CompanyId);
                //classes.commonTask.LoadShift(ddlShiftName, CompanyId);
                //addAllTextInShift();
                classes.Payroll.loadMonthIdByCompany(ddlMonth, CompanyId);
            }
            catch { }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            classes.commonTask.AddRemoveItem(lstAll, lstSelected);
        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {

            lblMessage.InnerText = "";
            classes.commonTask.AddRemoveAll(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            classes.commonTask.AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {

            lblMessage.InnerText = "";
            classes.commonTask.AddRemoveAll(lstSelected, lstAll);
        }

    }
}