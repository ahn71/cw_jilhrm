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
    public partial class monthly_salary_flow : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();

            if (!IsPostBack)
            {
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                loadMonthId();
            }
        }

        DataTable dtSetPrivilege = new DataTable();
        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__getUserId__"] = getUserId;


                //------------load privilege setting inof from db------
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "monthly_salary_flow.aspx", ddlCompanyList, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
               
                //-----------------------------------------------------


            }
            catch { }
        }

        private void loadMonthId()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select  Distinct format(yearmonth,'yyyy') as Year from v_MonthlySalarySheet order by Year ", dt);
                ddlYear.DataTextField = "Year";
                ddlYear.DataValueField = "Year";
                ddlYear.DataSource = dt;
                ddlYear.DataBind();
                ddlYear.Items.Insert(0, new ListItem(" ", " "));
            }
            catch { }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();   
            sqlDB.fillDataTable("SELECT        CompanyId, CompanyName, Format(YearMonth, 'MM-yyyy') AS Month, Format(YearMonth, 'yyyy') AS Year, Format(YearMonth, 'MMMM') AS MonthName, SUM(TotalSalary) AS TotalSalary " +
                               "FROM dbo.v_MonthlySalarySheet where CompanyId='" + ddlCompanyList.SelectedItem.Value + "' AND Year(YearMonth)='"+ddlYear.SelectedItem.Text+"'" +
                              "GROUP BY YearMonth, CompanyId, CompanyName",dt=new DataTable ());

            Session["__SalaryFlow__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=SalaryFlow-" + ddlYear.SelectedItem.Text + "');", true);  //Open New Tab for Sever side code
           
        }
    }
}