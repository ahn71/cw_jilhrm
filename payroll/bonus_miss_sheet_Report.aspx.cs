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
    public partial class bonus_miss_sheet_Report : System.Web.UI.Page
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
                    classes.commonTask.LoadDepartmentByCompanyInListBox(ViewState["__CompanyId__"].ToString(), lstAll);
                   // classes.commonTask.LoadShift(ddlShiftName, ViewState["__CompanyId__"].ToString());
                }
                else
                {
                    chkForAllCompany.Visible = false;
                    dtSetPrivilege = new DataTable();
                    chkForAllCompany.Enabled = true;
                    classes.commonTask.LoadBranch(ddlCompanyName, ViewState["__CompanyId__"].ToString());
                    classes.commonTask.LoadDepartmentByCompanyInListBox(ViewState["__CompanyId__"].ToString(), lstAll);
                  //  classes.commonTask.LoadShift(ddlShiftName, ViewState["__CompanyId__"].ToString());

                    //if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                    //{
                    //    btnPreview.CssClass = ""; btnPreview.Enabled = false;
                    //}

                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='bonus_miss_sheet_Report.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);

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
                    else {
                        tblGenerateType.Visible = false;
                        WarningMessage.Visible = true;
                        btnPreview.CssClass = ""; btnPreview.Enabled = false;
                    }

                }

                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();

                classes.Payroll.loadBonusType(ddlBonusType, CompanyId);

               // addAllTextInShift();

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
            if (ddlBonusType.SelectedValue == "0") { lblMessage.InnerText = "warning->Please select any Bonus Type!"; ddlBonusType.Focus(); return; }
            if (rblGenerateType.SelectedValue != "0" && txtEmpCardNo.Text.Trim().Length == 0) { lblMessage.InnerText = "warning->Please enter valid employee code no!"; txtEmpCardNo.Focus(); return; }
            if (rblGenerateType.SelectedValue == "0" && lstSelected.Items.Count == 0) { lblMessage.InnerText = "warning->Please select any Department!"; lstSelected.Focus(); return; }
            generateBonusSheet();
        }
        private void generateBonusSheet()
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
                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                }

                string getSQLCMD;
                DataTable dt = new DataTable();
                if (rblGenerateType.SelectedItem.Text.Equals("All"))
                {
                    getSQLCMD = "SELECT   substring(v_Payroll_YearlyBonusSheet.EmpCardNo,8,15) as EmpCardNo, v_Payroll_YearlyBonusSheet.PresentSalary,v_Payroll_YearlyBonusSheet.BasicSalary,v_Payroll_YearlyBonusSheet.BonusAmount," +
                              "v_Payroll_YearlyBonusSheet.TotalDays, v_Payroll_YearlyBonusSheet.EmpName,v_Payroll_YearlyBonusSheet.DptId,v_Payroll_YearlyBonusSheet.DptName,v_Payroll_YearlyBonusSheet.DsgName, v_Payroll_YearlyBonusSheet.Address," +
                              "FORMAT(v_Payroll_YearlyBonusSheet.EmpJoiningDate ,'dd-MM-yyyy') as EmpJoiningDate, v_Payroll_YearlyBonusSheet.CompanyName, v_Payroll_YearlyBonusSheet.SftName,v_Payroll_YearlyBonusSheet.CompanyId,v_Payroll_YearlyBonusSheet.SftId,v_Payroll_YearlyBonusSheet.DptId " +
                              " FROM   v_Payroll_YearlyBonusSheet"
                             + " where"
                             + " CompanyId in(" + CompanyList + ")  AND BonusType='" + ddlBonusType.SelectedItem.Value.ToString() + "' AND dptId  " + DepartmentList + " and BonusAmount ='0' "

                             + " Order By convert(int,DptCode),convert(int,SftId),CustomOrdering";

                }
                else
                {
                    getSQLCMD = "SELECT   substring(v_Payroll_YearlyBonusSheet.EmpCardNo,8,15) as EmpCardNo, v_Payroll_YearlyBonusSheet.PresentSalary,v_Payroll_YearlyBonusSheet.BasicSalary,v_Payroll_YearlyBonusSheet.BonusAmount," +
                              "v_Payroll_YearlyBonusSheet.TotalDays, v_Payroll_YearlyBonusSheet.EmpName,v_Payroll_YearlyBonusSheet.DptId,v_Payroll_YearlyBonusSheet.DptName,v_Payroll_YearlyBonusSheet.DsgName, v_Payroll_YearlyBonusSheet.Address," +
                              " FORMAT(v_Payroll_YearlyBonusSheet.EmpJoiningDate ,'dd-MM-yyyy') as EmpJoiningDate, v_Payroll_YearlyBonusSheet.CompanyName, v_Payroll_YearlyBonusSheet.SftName,v_Payroll_YearlyBonusSheet.CompanyId,v_Payroll_YearlyBonusSheet.SftId,v_Payroll_YearlyBonusSheet.DptId " +
                              " FROM   v_Payroll_YearlyBonusSheet"
                             + " where"
                             + " CompanyId in(" + CompanyList + ")  AND BonusType='" + ddlBonusType.SelectedItem.Value.ToString() + "' AND EmpCardNo='"+txtEmpCardNo.Text.Trim()+"' and BonusAmount ='0' ";

                }

                sqlDB.fillDataTable(getSQLCMD, dt);
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                }

                Session["__Language__"] = "English";
                Session["__BonusMissSheet__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=BonusMissSheet-" + ddlBonusType.SelectedItem.Text + "');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                classes.commonTask.LoadDepartmentByCompanyInListBox(CompanyId, lstAll);
                ////classes.Payroll.loadMonthIdByCompany(ddlSelectMonth, CompanyId);
                //classes.commonTask.LoadShift(ddlShiftName, CompanyId);
                //addAllTextInShift();
                classes.Payroll.loadBonusType(ddlBonusType, CompanyId);
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

        protected void rblGenerateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!rblGenerateType.SelectedItem.Text.Equals("All"))
                {
                    txtEmpCardNo.Enabled = true;
                    pnl1.Enabled = false;
                    ddlShiftName.SelectedValue = "0";
                    ddlShiftName.Enabled = false;
                    txtEmpCardNo.Focus();
                }
                else
                {
                    txtEmpCardNo.Text = "";
                    txtEmpCardNo.Enabled = false;
                    pnl1.Enabled = true;
                    ddlShiftName.Enabled = true;
                }
            }
            catch { }
        }
    }
}