using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.leave
{
    public partial class Earn_leave_Report : System.Web.UI.Page
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
                    classes.commonTask.LoadShift(ddlShiftName, ViewState["__CompanyId__"].ToString());

                }
                else
                {
                    chkForAllCompany.Visible = false;
                    dtSetPrivilege = new DataTable();
                    chkForAllCompany.Enabled = true;
                    classes.commonTask.LoadBranch(ddlCompanyName, ViewState["__CompanyId__"].ToString());
                    classes.commonTask.LoadShift(ddlShiftName, ViewState["__CompanyId__"].ToString());

                    //if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                    //{
                    //    btnPreview.CssClass = ""; btnPreview.Enabled = false;
                    //}

                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='Earn_leave_Report.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);

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
                loadMonthName(CompanyId);
                addAllTextInShift();

            }
            catch { }
        }
        private void addAllTextInShift()
        {
            if (ddlShiftName.Items.Count > 2)
                ddlShiftName.Items.Insert(1, new ListItem("All", "00"));
        }
        private void loadMonthName(string CompanyId)
        {
            try
            { 
                DataTable dt = new DataTable();
                 //string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                sqlDB.fillDataTable("select distinct  format(ElDateForReport,'MMM-yyyy') as DateText,format(ElDateForReport,'yyyy-MM') as DateVale from v_Leave_YearlyEarnLeaveGeneration  where CompanyId='" + CompanyId + "' order by DateVale desc ", dt);
                ddlSelectMonth.DataValueField = "DateVale";
                ddlSelectMonth.DataTextField = "DateText";
                ddlSelectMonth.DataSource = dt;
                ddlSelectMonth.DataBind();
                ddlSelectMonth.Items.Insert(0, new ListItem(" ", "00"));
            }
            catch { }
        }
        protected void rblGenerateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
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
                loadMonthName(CompanyId);
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
            Validation();
            EarnLeaveGeneration();
        }
        private void EarnLeaveGeneration()
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
                    CompanyList = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();

                    if (ddlShiftName.SelectedItem.ToString().Equals("All"))
                    {
                        ShiftList = classes.Payroll.getSftIdList(ddlShiftName);                        
                    }
                    else
                    {
                        ShiftList = ddlShiftName.SelectedValue.ToString();                       
                    }
                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                }
                string [] YearMonth = ddlSelectMonth.SelectedValue.Split('-');
                string[] MonthNeme = ddlSelectMonth.SelectedItem.Text.Split('-');
                string MonthRange="";
                String GeneratOrELmonthDate = "ELDateForReport";
                if (ckAllMonth.Checked == false) MonthRange = MonthNeme[0].ToString();
                else 
                {
                    GeneratOrELmonthDate = "GenerateDate";                    
                    if (YearMonth[1] == "03") MonthRange = "Jan~Feb~Mar";
                    else if (YearMonth[1] == "06") MonthRange = "Apr~May~Jun";
                    else if (YearMonth[1] == "09") MonthRange = "Jul~Aug~Sep";
                    else  MonthRange = "Oct~Nov~Dec";
                } 
                string getSQLCMD;
                DataTable dt = new DataTable();
                if (rblGenerateType.SelectedItem.Text.Equals("All"))
                {
                    getSQLCMD = "SELECT Format(GenerateDate,'MMM-yyyy') as GenerateDate, SUBSTRING(v_Leave_YearlyEarnLeaveGeneration.EmpCardNo,8,15) as EmpCardNo, v_Leave_YearlyEarnLeaveGeneration.EmpName,v_Leave_YearlyEarnLeaveGeneration.DsgName," +
                        " FORMAT( v_Leave_YearlyEarnLeaveGeneration.EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,FORMAT( v_Leave_YearlyEarnLeaveGeneration.EarnLeavePerviousStartYear,'dd-MM-yyyy') as EarnLeavePerviousStartYear,FORMAT( v_Leave_YearlyEarnLeaveGeneration.EarnLeaveEndYear,'dd-MM-yyyy') as EarnLeaveEndYear," +
                        " v_Leave_YearlyEarnLeaveGeneration.BasicSalary,v_Leave_YearlyEarnLeaveGeneration.NetTotal,v_Leave_YearlyEarnLeaveGeneration.DptName, v_Leave_YearlyEarnLeaveGeneration.CompanyName," +
                        "v_Leave_YearlyEarnLeaveGeneration.Address,v_Leave_YearlyEarnLeaveGeneration.SftName, v_Leave_YearlyEarnLeaveGeneration.PayableDays,v_Leave_YearlyEarnLeaveGeneration.WorkingDays," +
                        "v_Leave_YearlyEarnLeaveGeneration.SpendDays" +
                        " FROM   dbo.v_Leave_YearlyEarnLeaveGeneration " +
                         " where "
                        + " CompanyId in(" + CompanyList + ") AND SftId in (" + ShiftList + ") AND Year(" + GeneratOrELmonthDate + ")='" + YearMonth[0] + "' AND Month(" + GeneratOrELmonthDate + ")='" + YearMonth[1] + "' AND dptId  " + DepartmentList + " "
                        + " ORDER BY v_Leave_YearlyEarnLeaveGeneration.CompanyName,v_Leave_YearlyEarnLeaveGeneration.SftName,v_Leave_YearlyEarnLeaveGeneration.DptName ";

                }
                else
                {
                    getSQLCMD = "SELECT Format(GenerateDate,'MMM-yyyy') as GenerateDate, SUBSTRING(v_Leave_YearlyEarnLeaveGeneration.EmpCardNo,8,15) as EmpCardNo, v_Leave_YearlyEarnLeaveGeneration.EmpName,v_Leave_YearlyEarnLeaveGeneration.DsgName," +
                        " FORMAT( v_Leave_YearlyEarnLeaveGeneration.EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate,FORMAT( v_Leave_YearlyEarnLeaveGeneration.EarnLeavePerviousStartYear,'dd-MM-yyyy') as EarnLeavePerviousStartYear,FORMAT( v_Leave_YearlyEarnLeaveGeneration.EarnLeaveEndYear,'dd-MM-yyyy') as EarnLeaveEndYear," +
                        " v_Leave_YearlyEarnLeaveGeneration.BasicSalary,v_Leave_YearlyEarnLeaveGeneration.NetTotal,v_Leave_YearlyEarnLeaveGeneration.DptName, v_Leave_YearlyEarnLeaveGeneration.CompanyName," +
                        "v_Leave_YearlyEarnLeaveGeneration.Address,v_Leave_YearlyEarnLeaveGeneration.SftName, v_Leave_YearlyEarnLeaveGeneration.PayableDays,v_Leave_YearlyEarnLeaveGeneration.WorkingDays," +
                        "v_Leave_YearlyEarnLeaveGeneration.SpendDays,FORMAT(ELDateForReport,'MMM')as ElMonthName" +
                        " FROM   dbo.v_Leave_YearlyEarnLeaveGeneration "
                        + " where"
                        + " CompanyId in(" + CompanyList + ") AND Year(" + GeneratOrELmonthDate + ")='" + YearMonth[0] + "' AND Month(" + GeneratOrELmonthDate + ")='" + YearMonth[1] + "' AND EmpCardNo Like '%" + txtEmpCardNo.Text.Trim() + "' "
                      + " ORDER BY v_Leave_YearlyEarnLeaveGeneration.CompanyName,v_Leave_YearlyEarnLeaveGeneration.SftName,v_Leave_YearlyEarnLeaveGeneration.DptName ";  
                }
                sqlDB.fillDataTable(getSQLCMD, dt);                
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                } 
                if (rblGenerateType.SelectedItem.Text != "All") MonthRange = dt.Rows[0]["ElMonthName"].ToString();               
                Session["__EarnLeave__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EarnLeave-" + MonthRange + "');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
        private void Validation() 
        {
            if (rblGenerateType.SelectedItem.Text == "All")
            {
                if (ddlShiftName.SelectedItem.Text == "")
                { 
                    lblMessage.InnerText = "warning-> Please select any Shift!";
                    return;
                }
                if (lstSelected.Items.Count==0)
                {
                  lblMessage.InnerText = "warning-> Please select any Department!";
                    return;
                }
            }
            else 
            {
                if (txtEmpCardNo.Text.Trim().Length<4) 
                {
                    lblMessage.InnerText = "warning-> Please Type a Card Number. (Minimum 4 character)";
                    txtEmpCardNo.Focus();
                    return;
                }
            }
            if (ddlSelectMonth.SelectedIndex==0)
            {
              lblMessage.InnerText = "warning-> Please select any Month !";
                return; 
            }           
        }

        protected void ddlSelectMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string[] Month = ddlSelectMonth.SelectedValue.Split('-');
                if (Month[1] == "03" || Month[1] == "06" || Month[1] == "09" || Month[1] == "12") ckAllMonth.Visible = true;
                else
                {
                    ckAllMonth.Checked = false;
                    ckAllMonth.Visible = false;   
                }
            }
            catch { }
          
        }
    }
}