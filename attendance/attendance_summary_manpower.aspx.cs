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
    public partial class attendance_summary_manpower : System.Web.UI.Page
    {
        DataTable dt;
        DataTable dtSetPrivilege;
        string CompanyId = "";
        string SqlCmd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                //classes.commonTask.LoadEmpTypeWithAll(rblEmpType);
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompany.Enabled = false;
                ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
                Session["__MinDigits__"] = "6";
            }

        }
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                //------------load privilege setting inof from db------
                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "attendance_summary_manpower.aspx", ddlCompany, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.commonTask.LoadDepartment(ViewState["__CompanyId__"].ToString(), lstAll);
                //-----------------------------------------------------

              
         


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
            generateReport();
        }
        private void generateReport() 
        {
         
           
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue.ToString();
            //string[] dmy = txtDate.Text.Split('-');
            //string d = dmy[0]; string m = dmy[1]; string y = dmy[2];

            string CompanyList = "";
     
            string DepartmentList = "";

        


            CompanyList = "in ('" + CompanyId + "')";      

            DepartmentList = classes.commonTask.getDepartmentList(lstSelected);

             SqlCmd="WITH NewEmployee AS(SELECT COUNT(EmpId) as NewEmp,DptName,GName,DsgName FROM v_tblAttendanceRecord "+
                " WHERE CompanyId " + CompanyList + " and DptId " + DepartmentList + " and CONVERT(varchar(11),ATTDate,105)='" + txtDate.Text + "' AND ATTDate=EmpJoiningDate GROUP BY DptName,GName,DsgName) " +
                "SELECT SUM(CASE WHEN ATTStatus='P' OR ATTStatus='L' THEN 1 ELSE 0 END) AS Present,SUM(CASE WHEN ATTStatus='A' THEN 1 ELSE 0 END) AS Absent,"+
                "SUM(CASE WHEN ATTStatus='Lv' THEN 1 ELSE 0 END) AS Leave,n.NewEmp,t.DptName,t.GName,t.DsgName,t.CompanyName,t.Address,CONVERT(varchar(11),"+
                "ATTDate,106) AS ATTDate FROM v_tblAttendanceRecord t LEFT OUTER JOIN NewEmployee n ON t.DptName=n.DptName AND t.GName=n.GName AND "+
                "t.DsgName=n.DsgName WHERE t.CompanyId " + CompanyList + " and t.DptId " + DepartmentList + " and CONVERT(varchar(11),ATTDate,105)='" + txtDate.Text + "' " +
                "GROUP BY t.DptName,t.GName,t.DsgName,t.CompanyName, NewEmp,t.Address,CONVERT(varchar(11),ATTDate,106)";
            sqlDB.fillDataTable(SqlCmd, dt = new DataTable());      
          
            if (dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->No Attendance Available";               
                return;
            }
            Session["__ManpowerWiseAttReport__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=ManpowerWiseAttReport');", true);  //Open New Tab for Sever side code

        }
    }
}