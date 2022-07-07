using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.payroll
{
    public partial class advance_info : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
                
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

                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Viewer"))
                {

                    classes.commonTask.LoadBranch(ddlCompanyName);
                    classes.commonTask.LoadDepartment(ViewState["__CompanyId__"].ToString(), lstAll);
                  //  classes.commonTask.LoadShift(ddlShiftName, ViewState["__CompanyId__"].ToString());


                }
                else
                {
                    chkForAllCompany.Visible = false;
                    dtSetPrivilege = new DataTable();
                    chkForAllCompany.Enabled = true;
                    classes.commonTask.LoadBranch(ddlCompanyName, ViewState["__CompanyId__"].ToString());
                    classes.commonTask.LoadDepartment(ViewState["__CompanyId__"].ToString(), lstAll);
                  //  classes.commonTask.LoadShift(ddlShiftName, ViewState["__CompanyId__"].ToString());

                    //if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                    //{
                    //    btnPreview.CssClass = ""; btnPreview.Enabled = false;
                    //}

                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='advance_info.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);

                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                        {
                            btnPreview.CssClass = "css_btn"; btnPreview.Enabled = true;
                        }
                        else
                        {
                            tblGenerateType.Visible = false;
                            WarningMessage.Visible = true;
                            btnPreview.CssClass = ""; btnPreview.Enabled = false;
                        }

                    }
                    else
                    {
                        tblGenerateType.Visible = false;
                        WarningMessage.Visible = true;
                        btnPreview.CssClass = ""; btnPreview.Enabled = false;
                    }

                }

                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();

                classes.Payroll.loadMonthIdByCompany(ddlSelectMonth, CompanyId);
               // addAllTextInShift();

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
                lblMessage.InnerText = "";

                if (!rblGenerateType.SelectedItem.Text.Equals("All"))
                {
                    txtEmpCardNo.Enabled = true;
                    txtEmpCardNo.Focus();
                    pnl1.Enabled = false;
                    ddlShiftName.Enabled = false;
                }
                else
                {
                    txtEmpCardNo.Enabled = false;
                    pnl1.Enabled = true;
                    ddlShiftName.Enabled = true;
                }
            }
            catch { }
        }

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                classes.commonTask.LoadShift(ddlShiftName, CompanyId);
                addAllTextInShift();
                classes.Payroll.loadMonthIdByCompany(ddlSelectMonth, CompanyId);
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

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            //if (ddlSelectMonth.SelectedIndex < 1) 
            //{
            //    lblMessage.InnerText = "warning-> Please select any month !"; ddlSelectMonth.Focus(); return;
            //}
            if (rblGenerateType.SelectedValue == "0" && lstSelected.Items.Count == 0) 
            {
                lblMessage.InnerText = "warning-> Please select any department !"; lstSelected.Focus(); return;
            }
            if (rblGenerateType.SelectedValue != "0" && txtEmpCardNo.Text.Trim().Length==0)
            {
                lblMessage.InnerText = "warning-> Please type valid card no !"; txtEmpCardNo.Focus(); return;
            }
            generateOTPaymentSheet();
        }

        private void generateOTPaymentSheet()
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
                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
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
                }

                string getSQLCMD="";

                // for find out last advance info for all employee
                if (rblReportType.SelectedValue.ToString().Trim().Equals("Summary") && rblGenerateType.SelectedItem.Text.ToString().Equals("All") && rbAllAdvanceLastAdvance.SelectedValue.ToString().Equals("Last Advance Info"))
                {
                    Session["__rptType__"] = "Last Advance Info Summary For All Emp";

                    getSQLCMD = "SELECT Max(SL) as SL,substring(v_Payroll_AdvanceInfo.EmpCardNo,8,15) as EmpCardNo,Format(v_Payroll_AdvanceInfo.EntryDate,'MM-yyyy') as EntryDate,v_Payroll_AdvanceInfo.InstallmentNo,v_Payroll_AdvanceInfo.AdvanceAmount,"
                              + "Format(v_Payroll_AdvanceInfo.StartMonth,'MM-yyyy') as StartMonth,v_Payroll_AdvanceInfo.PaidInstallmentNo,v_Payroll_AdvanceInfo.InstallmentAmount,"
                              + "v_Payroll_AdvanceInfo.EmpName,v_Payroll_AdvanceInfo.DptName,"
                              + "v_Payroll_AdvanceInfo.DsgName,v_Payroll_AdvanceInfo.CompanyName,v_Payroll_AdvanceInfo.PaidStatus,"
                              + "v_Payroll_AdvanceInfo.Address,CompanyId,sftId,DptId"
                              + " FROM dbo.v_Payroll_AdvanceInfo"
                              +" Where "
                              + " CompanyId in(" + CompanyList + ") AND dptId  " + DepartmentList + " "
                              +" Group By "
                              + " substring(v_Payroll_AdvanceInfo.EmpCardNo,8,15),v_Payroll_AdvanceInfo.EntryDate,v_Payroll_AdvanceInfo.InstallmentNo,v_Payroll_AdvanceInfo.AdvanceAmount,"
                              +" v_Payroll_AdvanceInfo.StartMonth,v_Payroll_AdvanceInfo.PaidInstallmentNo,v_Payroll_AdvanceInfo.InstallmentAmount,"
                              +" v_Payroll_AdvanceInfo.EmpName,v_Payroll_AdvanceInfo.DptName,"
                              +" v_Payroll_AdvanceInfo.DsgName,v_Payroll_AdvanceInfo.CompanyName,v_Payroll_AdvanceInfo.PaidStatus,"
                              +" v_Payroll_AdvanceInfo.Address,CompanyId,sftId,DptId"
                              +" ORDER BY CompanyId,sftId,DptId";
                }
                else if (rblReportType.SelectedValue.ToString().Trim().Equals("Summary") && rblGenerateType.SelectedItem.Text.ToString().Equals("Individual") && rbAllAdvanceLastAdvance.SelectedValue.ToString().Equals("Last Advance Info"))
                {
                    Session["__rptType__"] = "Last Advance Info Summary For Certain Emp";

                    getSQLCMD = "SELECT Max(SL) as SL,substring(v_Payroll_AdvanceInfo.EmpCardNo,8,15) as EmpCardNo,Format(v_Payroll_AdvanceInfo.EntryDate,'MM-yyyy') as EntryDate,v_Payroll_AdvanceInfo.InstallmentNo,v_Payroll_AdvanceInfo.AdvanceAmount,"
                              + "Format(v_Payroll_AdvanceInfo.StartMonth,'MM-yyyy') as StartMonth,v_Payroll_AdvanceInfo.PaidInstallmentNo,v_Payroll_AdvanceInfo.InstallmentAmount,"
                              + "v_Payroll_AdvanceInfo.EmpName,v_Payroll_AdvanceInfo.DptName,"
                              + "v_Payroll_AdvanceInfo.DsgName,v_Payroll_AdvanceInfo.CompanyName,v_Payroll_AdvanceInfo.PaidStatus,"
                              + "v_Payroll_AdvanceInfo.Address,CompanyId,sftId,DptId"
                              + " FROM dbo.v_Payroll_AdvanceInfo"
                              + " Where "
                              + " CompanyId in(" + CompanyList + ") AND EmpCardNo Like '%" + txtEmpCardNo.Text.Trim() + "' "
                              + " Group By "
                              + " substring(v_Payroll_AdvanceInfo.EmpCardNo,8,15),v_Payroll_AdvanceInfo.EntryDate,v_Payroll_AdvanceInfo.InstallmentNo,v_Payroll_AdvanceInfo.AdvanceAmount,"
                              + " v_Payroll_AdvanceInfo.StartMonth,v_Payroll_AdvanceInfo.PaidInstallmentNo,v_Payroll_AdvanceInfo.InstallmentAmount,"
                              + " v_Payroll_AdvanceInfo.EmpName,v_Payroll_AdvanceInfo.DptName,"
                              + " v_Payroll_AdvanceInfo.DsgName,v_Payroll_AdvanceInfo.CompanyName,v_Payroll_AdvanceInfo.PaidStatus,"
                              + " v_Payroll_AdvanceInfo.Address,CompanyId,sftId,DptId"
                              + " ORDER BY CompanyId,sftId,DptId";
                }

                else if (rblReportType.SelectedValue.ToString().Trim().Equals("Summary") && rblGenerateType.SelectedItem.Text.ToString().Equals("All") && rbAllAdvanceLastAdvance.SelectedValue.ToString().Equals("All Advance Info"))
                {
                    Session["__rptType__"] = "All Advance Info Summary For All Emp";

                    getSQLCMD = "SELECT substring(v_Payroll_AdvanceInfo.EmpCardNo,8,15) as EmpCardNo,Format(v_Payroll_AdvanceInfo.EntryDate,'MM-yyyy') as EntryDate,v_Payroll_AdvanceInfo.InstallmentNo,"
                              + "Format(v_Payroll_AdvanceInfo.StartMonth,'MM-yyyy') as StartMonth,v_Payroll_AdvanceInfo.PaidInstallmentNo,v_Payroll_AdvanceInfo.InstallmentAmount,v_Payroll_AdvanceInfo.AdvanceAmount,"
                              + "v_Payroll_AdvanceInfo.EmpName,v_Payroll_AdvanceInfo.DptName,"
                              + "v_Payroll_AdvanceInfo.DsgName,v_Payroll_AdvanceInfo.CompanyName,v_Payroll_AdvanceInfo.PaidStatus,"
                              + "v_Payroll_AdvanceInfo.Address,CompanyId,sftId,DptId,EmpId"
                              + " FROM dbo.v_Payroll_AdvanceInfo"
                              + " Where "
                              + " CompanyId in(" + CompanyList + ")  AND dptId  " + DepartmentList + " "
                              + " Group By "
                              + " substring(v_Payroll_AdvanceInfo.EmpCardNo,8,15),v_Payroll_AdvanceInfo.EntryDate,v_Payroll_AdvanceInfo.InstallmentNo,"
                              + " v_Payroll_AdvanceInfo.StartMonth,v_Payroll_AdvanceInfo.PaidInstallmentNo,v_Payroll_AdvanceInfo.InstallmentAmount,v_Payroll_AdvanceInfo.AdvanceAmount,"
                              + " v_Payroll_AdvanceInfo.EmpName,v_Payroll_AdvanceInfo.DptName,"
                              + " v_Payroll_AdvanceInfo.DsgName,v_Payroll_AdvanceInfo.CompanyName,v_Payroll_AdvanceInfo.PaidStatus,"
                              + " v_Payroll_AdvanceInfo.Address,CompanyId,sftId,DptId,EmpId"
                              + " ORDER BY v_Payroll_AdvanceInfo.CompanyId,v_Payroll_AdvanceInfo.SftId,DptId,EmpCardNo";
                }

                else if (rblReportType.SelectedValue.ToString().Trim().Equals("Summary") && rblGenerateType.SelectedItem.Text.ToString().Equals("Individual") && rbAllAdvanceLastAdvance.SelectedValue.ToString().Equals("All Advance Info"))
                {
                    Session["__rptType__"] = "All Advance Info Summary For Certain Emp";

                    getSQLCMD = "SELECT substring(v_Payroll_AdvanceInfo.EmpCardNo,8,15) as EmpCardNo,Format(v_Payroll_AdvanceInfo.EntryDate,'MM-yyyy') as EntryDate,v_Payroll_AdvanceInfo.InstallmentNo,"
                              + "Format(v_Payroll_AdvanceInfo.StartMonth,'MM-yyyy') as StartMonth,v_Payroll_AdvanceInfo.PaidInstallmentNo,v_Payroll_AdvanceInfo.InstallmentAmount,v_Payroll_AdvanceInfo.AdvanceAmount,"
                              + "v_Payroll_AdvanceInfo.EmpName,v_Payroll_AdvanceInfo.DptName,"
                              + "v_Payroll_AdvanceInfo.DsgName,v_Payroll_AdvanceInfo.CompanyName,v_Payroll_AdvanceInfo.PaidStatus,"
                              + "v_Payroll_AdvanceInfo.Address,CompanyId,sftId,DptId,EmpId"
                              + " FROM dbo.v_Payroll_AdvanceInfo"
                              + " Where "
                              + " CompanyId in(" + CompanyList + ") AND EmpCardNo Like '%" + txtEmpCardNo.Text.Trim() + "' "
                              + " Group By "
                              + " substring(v_Payroll_AdvanceInfo.EmpCardNo,8,15),v_Payroll_AdvanceInfo.EntryDate,v_Payroll_AdvanceInfo.InstallmentNo,"
                              + " v_Payroll_AdvanceInfo.StartMonth,v_Payroll_AdvanceInfo.PaidInstallmentNo,v_Payroll_AdvanceInfo.InstallmentAmount,v_Payroll_AdvanceInfo.AdvanceAmount,"
                              + " v_Payroll_AdvanceInfo.EmpName,v_Payroll_AdvanceInfo.DptName,"
                              + " v_Payroll_AdvanceInfo.DsgName,v_Payroll_AdvanceInfo.CompanyName,v_Payroll_AdvanceInfo.PaidStatus,"
                              + " v_Payroll_AdvanceInfo.Address,CompanyId,sftId,DptId,EmpId"
                              + " ORDER BY v_Payroll_AdvanceInfo.CompanyId,v_Payroll_AdvanceInfo.SftId,DptId,EmpCardNo";
                }

                DataTable dt = new DataTable();

                /*
                if (rblGenerateType.SelectedItem.Text.Equals("All"))
                {
                    getSQLCMD = " SELECT v_MonthlySalarySheet.EmpName,v_MonthlySalarySheet.EmpCardNo,v_MonthlySalarySheet.BasicSalary,"
                                + "v_MonthlySalarySheet.TotalOTHour,v_MonthlySalarySheet.OTRate,v_MonthlySalarySheet.TotalOTAmount,v_MonthlySalarySheet.DsgName,"
                                + "v_MonthlySalarySheet.DptName,v_MonthlySalarySheet.CompanyName,v_MonthlySalarySheet.SftName,v_MonthlySalarySheet.Address,FORMAT(YearMonth,'MMMM-yyyy') as YearMonth"
                                + " FROM"
                                + " v_MonthlySalarySheet"
                                +" where "
                                + " CompanyId in('" + CompanyList + "') AND SftId in (" + ShiftList + ") AND YearMonth='" + ddlSelectMonth.SelectedItem.Value.ToString() + "' AND dptId  " + DepartmentList + " AND  TotalOTAmount>0"
                                + "group by "
                                + " v_MonthlySalarySheet.EmpName,v_MonthlySalarySheet.EmpCardNo,v_MonthlySalarySheet.BasicSalary,"
                                + "v_MonthlySalarySheet.TotalOTHour,v_MonthlySalarySheet.OTRate,v_MonthlySalarySheet.TotalOTAmount,v_MonthlySalarySheet.DsgName,"
                                + "v_MonthlySalarySheet.DptName,v_MonthlySalarySheet.CompanyName,v_MonthlySalarySheet.SftName,v_MonthlySalarySheet.Address,YearMonth"
                                + " ORDER BY "
                                +" v_MonthlySalarySheet.CompanyName, v_MonthlySalarySheet.SftName, v_MonthlySalarySheet.DptName";
                }
                else
                {
                    getSQLCMD = "SELECT v_MonthlySalarySheet.EmpName, v_MonthlySalarySheet.EmpCardNo,v_MonthlySalarySheet.EmpJoiningDate,v_MonthlySalarySheet.DaysInMonth,v_MonthlySalarySheet.CasualLeave,v_MonthlySalarySheet.SickLeave, v_MonthlySalarySheet.AnnualLeave,v_MonthlySalarySheet.FestivalHoliday,v_MonthlySalarySheet.AbsentDay,v_MonthlySalarySheet.PresentDay,v_MonthlySalarySheet.PayableDays,v_MonthlySalarySheet.BasicSalary,v_MonthlySalarySheet.HouseRent,v_MonthlySalarySheet.MedicalAllownce,v_MonthlySalarySheet.ConvenceAllownce,v_MonthlySalarySheet.DsgName,v_MonthlySalarySheet.DptName,v_MonthlySalarySheet.CompanyName,v_MonthlySalarySheet.SftName,v_MonthlySalarySheet.WeekendHoliday,v_MonthlySalarySheet.OfficialLeave,v_MonthlySalarySheet.OthersLeave,v_MonthlySalarySheet.FoodAllownce,v_MonthlySalarySheet.TotalOTAmount,v_MonthlySalarySheet.Stampdeduct,v_MonthlySalarySheet.TechnicalAllowance,v_MonthlySalarySheet.NetPayable,v_MonthlySalarySheet.TotalSalary,v_MonthlySalarySheet.ProvidentFund,v_MonthlySalarySheet.ProfitTax,CompanyId,SftId,DptId"
                            + " FROM   v_MonthlySalarySheet"
                            + " where "
                            + " CompanyId ='" + CompanyList + "' AND YearMonth='" + ddlSelectMonth.SelectedItem.Value.ToString() + "' AND EmpCardNo Like '%" + txtEmpCardNo.Text.Trim() + "' "
                            + "group by "
                            + "v_MonthlySalarySheet.EmpName, v_MonthlySalarySheet.EmpCardNo,v_MonthlySalarySheet.EmpJoiningDate,v_MonthlySalarySheet.DaysInMonth,v_MonthlySalarySheet.CasualLeave,v_MonthlySalarySheet.SickLeave, v_MonthlySalarySheet.AnnualLeave,v_MonthlySalarySheet.FestivalHoliday,v_MonthlySalarySheet.AbsentDay,v_MonthlySalarySheet.PresentDay,v_MonthlySalarySheet.PayableDays,v_MonthlySalarySheet.BasicSalary,v_MonthlySalarySheet.HouseRent,v_MonthlySalarySheet.MedicalAllownce,v_MonthlySalarySheet.ConvenceAllownce,v_MonthlySalarySheet.DsgName,v_MonthlySalarySheet.DptName,v_MonthlySalarySheet.CompanyName,v_MonthlySalarySheet.SftName,v_MonthlySalarySheet.WeekendHoliday,v_MonthlySalarySheet.OfficialLeave,v_MonthlySalarySheet.OthersLeave,v_MonthlySalarySheet.FoodAllownce,v_MonthlySalarySheet.TotalOTAmount,v_MonthlySalarySheet.Stampdeduct,v_MonthlySalarySheet.TechnicalAllowance,v_MonthlySalarySheet.NetPayable,v_MonthlySalarySheet.TotalSalary,v_MonthlySalarySheet.ProvidentFund,v_MonthlySalarySheet.ProfitTax,CompanyId,SftId,DptId"
                            + " order by CompanyId,SftId,DptId";
                }
                */
                

                /*
                SqlCommand cmd = new SqlCommand("Payroll_MonthlySalary_Payslip",sqlDB.connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@YearMonth",ddlSelectMonth.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@CompanyId",CompanyList);
                cmd.Parameters.AddWithValue("@shiftId", ShiftList);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt=new DataTable();
                da.Fill(dt);
                */
                sqlDB.fillDataTable(getSQLCMD, dt);
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->Sorry any advance record are not founded"; return;
                }

                Session["__Language__"] = "English";
                Session["__AdvanceInfo__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=AdvanceInfo-" + ddlSelectMonth.SelectedItem.Text + "');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
        

    }
}