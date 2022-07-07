using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using ComplexScriptingSystem;
using adviitRuntimeScripting;
using SigmaERP.classes;

namespace SigmaERP.payroll
{
    public partial class ot_payment_sheet : System.Web.UI.Page
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


                //------------load privilege setting inof from db------
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "ot_payment_sheet.aspx", ddlCompanyName, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                commonTask.LoadDepartmentByCompanyInListBox(ViewState["__CompanyId__"].ToString(), lstAll);
                classes.Payroll.loadMonthIdByCompany(ddlSelectMonth, ViewState["__CompanyId__"].ToString());
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
                }
                else
                {
                    txtEmpCardNo.Enabled = false;
                    pnl1.Enabled = true;
                    ddlShiftName.Enabled = true;
                    txtEmpCardNo.Focus();
                }
            }
            catch { }
        }

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                //classes.commonTask.LoadShift(ddlShiftName, CompanyId);
                //addAllTextInShift();
                classes.commonTask.LoadDepartmentByCompanyInListBox(CompanyId, lstAll);
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

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            if (ddlSelectMonth.SelectedValue == "0") { lblMessage.InnerText = "warning->Please select any Month!"; ddlSelectMonth.Focus(); return; }
            generateOTPaymentSheet();
        }

        private void generateOTPaymentSheet()
        {
            try
            {
                string CompanyList = "";
               
                string DepartmentList = "";

                if (!Page.IsValid)   // If Java script are desible then 
                {
                    lblMessage.InnerText = "erroe->Please Select From Date And To Date"; return;
                }


                CompanyList = (ddlCompanyName.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                string getSQLCMD;
                DataTable dt = new DataTable();
                if (rblGenerateType.SelectedItem.Text.Equals("All"))
                {
                    getSQLCMD = " SELECT v_MonthlySalarySheet.EmpName,substring(v_MonthlySalarySheet.EmpCardNo,8,15) as EmpCardNo,v_MonthlySalarySheet.BasicSalary,"
                                + "v_MonthlySalarySheet.TotalOTHour,v_MonthlySalarySheet.OTRate,v_MonthlySalarySheet.TotalOTAmount,v_MonthlySalarySheet.DsgName,"
                                + "v_MonthlySalarySheet.DptId,v_MonthlySalarySheet.DptName,v_MonthlySalarySheet.CompanyName,v_MonthlySalarySheet.SftName,v_MonthlySalarySheet.Address,FORMAT(YearMonth,'MMMM-yyyy') as YearMonth,CompanyId,GId,GName ,TotalOverTime,TotalOtherOverTime,TotalOTHour "
                                + " FROM"
                                + " v_MonthlySalarySheet"
                                + " where "
                                + " CompanyId in('" + CompanyList + "') AND  YearMonth='" + ddlSelectMonth.SelectedItem.Value.ToString() + "' AND dptId  " + DepartmentList + " AND  TotalOTAmount>0"
                                + "group by "
                                + " v_MonthlySalarySheet.EmpName,substring(v_MonthlySalarySheet.EmpCardNo,8,15),v_MonthlySalarySheet.BasicSalary,"
                                + "v_MonthlySalarySheet.TotalOTHour,v_MonthlySalarySheet.OTRate,v_MonthlySalarySheet.TotalOTAmount,v_MonthlySalarySheet.DsgName,"
                                + "v_MonthlySalarySheet.DptId,v_MonthlySalarySheet.DptName,v_MonthlySalarySheet.CompanyName,v_MonthlySalarySheet.SftName,v_MonthlySalarySheet.Address,YearMonth,CompanyId,GId,GName ,convert(int,DptCode),convert(int,SftId),CustomOrdering,TotalOverTime,TotalOtherOverTime,TotalOTHour "
                                + "ORDER BY "
                                +" convert(int,DptCode),convert(int,SftId),CustomOrdering";
                }
                else
                {

                    getSQLCMD = " SELECT v_MonthlySalarySheet.EmpName,substring(v_MonthlySalarySheet.EmpCardNo,8,15) as EmpCardNo,v_MonthlySalarySheet.BasicSalary,"
                                + "v_MonthlySalarySheet.TotalOTHour,v_MonthlySalarySheet.OTRate,v_MonthlySalarySheet.TotalOTAmount,v_MonthlySalarySheet.DsgName,"
                                + "v_MonthlySalarySheet.DptId,v_MonthlySalarySheet.DptName,v_MonthlySalarySheet.CompanyName,v_MonthlySalarySheet.SftName,v_MonthlySalarySheet.Address,FORMAT(YearMonth,'MMMM-yyyy') as YearMonth,CompanyId,GId ,GName,TotalOverTime,TotalOtherOverTime,TotalOTHour "
                                + " FROM"
                                + " v_MonthlySalarySheet"
                                + " where "
                                + " CompanyId ='" + CompanyList + "' AND YearMonth='" + ddlSelectMonth.SelectedItem.Value.ToString() + "' AND EmpCardNo Like '%" + txtEmpCardNo.Text.Trim() + "' "
                                + "group by "
                                + " v_MonthlySalarySheet.EmpName,substring(v_MonthlySalarySheet.EmpCardNo,8,15),v_MonthlySalarySheet.BasicSalary,"
                                + "v_MonthlySalarySheet.TotalOTHour,v_MonthlySalarySheet.OTRate,v_MonthlySalarySheet.TotalOTAmount,v_MonthlySalarySheet.DsgName,"
                                + "v_MonthlySalarySheet.DptId,v_MonthlySalarySheet.DptName,v_MonthlySalarySheet.CompanyName,v_MonthlySalarySheet.SftName,v_MonthlySalarySheet.Address,YearMonth,CompanyId,GId,GName,TotalOverTime,TotalOtherOverTime,TotalOTHour ";
                              
                }

                sqlDB.fillDataTable(getSQLCMD, dt);

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

                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->Sorry Any Record Are Not Founded"; return;
                }

                Session["__Language__"] = "English";
                Session["__OvertimeSheet__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=OvertimeSheet-" + ddlSelectMonth.SelectedItem.Text + "');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
    }
}


