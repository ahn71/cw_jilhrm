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
    public partial class leave_deduction_setting : System.Web.UI.Page
    {
        DataTable dt;
        string sqlCmd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";            
            if (!IsPostBack)
            {
                txtMonth.Text = DateTime.Now.ToString("MM-yyyy");
                setPrivilege();
                loadEmployeeList();
            }
        }
        private void setPrivilege()
        {
            try
            {                
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                Session["__getUserId__"] = getUserId;
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                DropDownList ddlCompanyList = new DropDownList();
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForOnlyWriteAction(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "payroll_generation.aspx", ddlCompanyList, btnSearch);
                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
                if (ViewState["__WriteAction__"].ToString().Equals("0"))
                {
                    gvEmplyeeList.Enabled = false;
                    btnSearch.Enabled = false;
                }
            }
            catch { }
        }
        private void loadEmployeeList()
        {
            try
            {
                string date = commonTask.ddMMyyyyToyyyyMMdd("01-" + txtMonth.Text.Trim());
                ViewState["__Month__"] = date;
                sqlCmd = "select  ed.EmpId, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo ,ed.EmpName,ed.DsgName,ed.DptName, case when lmds.EmpID is null then 'True' else 'False' End  as Status from v_EmployeeDetails ed left join Leave_MonthlyLeaveDeductionSettings lmds on ed.EmpId=lmds.EmpID and  lmds.Month='"+ ViewState["__Month__"].ToString() + "'";
                sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
                gvEmplyeeList.DataSource = dt;
                gvEmplyeeList.DataBind();
            }
            catch(Exception ex)
            { }
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            loadEmployeeList();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            deleteData();
            foreach (GridViewRow row in gvEmplyeeList.Rows)
            {
                CheckBox cbChosen = new CheckBox();
                cbChosen =(CheckBox)row.FindControl("ckbChosen");
                if (!cbChosen.Checked)
                {
                    string EmpID = ((Label)row.FindControl("lblEmpId")).Text;                   
                    saveData(EmpID);
                }                    
            }
            lblMessage.InnerText = "success-> Successfully Submitted.";
        }
        private void saveData(string EmpID)
        {            
            sqlCmd = "INSERT INTO [dbo].[Leave_MonthlyLeaveDeductionSettings]([EmpID],[Month])VALUES ('"+ EmpID + "','"+ ViewState["__Month__"].ToString() + "')";
            CRUD.Execute(sqlCmd, sqlDB.connection);
        }
        private void deleteData()
        {
            sqlCmd = "DELETE [dbo].[Leave_MonthlyLeaveDeductionSettings] WHERE [Month]='"+ ViewState["__Month__"].ToString() + "'";
            CRUD.Execute(sqlCmd, sqlDB.connection);
        }

        protected void gvEmplyeeList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                    e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
                }
            }
            catch { }
        }
    }
}