using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace SigmaERP.personnel
{
    public partial class appoinment_letter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                classes.commonTask.loadDivision(ddlDivision);
                LoadEmptypeId("Staff");
            }
        }
        private void setPrivilege()
        {
            try
            {
                upSuperAdmin.Value = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    upSuperAdmin.Value = "0";
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='appoinment_letter.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                        {
                            btnPreview.CssClass = "";

                            btnPreview.Enabled = false;


                        }
                    }
                }

            }
            catch { }

        }
        private void LoadEmptypeId(string EmpTypeName)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select EmpTypeId From HRD_EmployeeType where EmpType='" + EmpTypeName + "'", dt);
                if (dt.Rows.Count == 0) return;
                hdfEmptypeId.Value = dt.Rows[0]["EmpTypeId"].ToString();
            }
            catch { }
        }

        protected void rdbstaff_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdbstaff.Checked)
                {
                    LoadEmptypeId("Staff");
                    ddlDivision.SelectedIndex = 0;
                    ddlDepartment.Items.Clear();
                    ddlCardNo.Items.Clear();
                    rdbworker.Checked = false;
                }
                else
                {
                    rdbworker.Checked = true;
                }
            }
            catch { }
        }

        protected void rdbworker_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbworker.Checked)
            {
                LoadEmptypeId("Worker");
                ddlDivision.SelectedIndex = 0;
                ddlDepartment.Items.Clear();
                ddlCardNo.Items.Clear();
                rdbstaff.Checked = false;
            }
            else
            {
                rdbstaff.Checked = true;
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                classes.commonTask.SearchDepartment(ddlDivision.SelectedValue,ddlDepartment);
            }
            catch { }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.commonTask.LoadEmpCardNoAppoinmentLetter(hdfEmptypeId.Value,ddlDivision.SelectedValue,ddlDepartment.SelectedValue,ddlCardNo);
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            if (rdbstaff.Checked == true)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=AppoinmentLetterStaff-" + ddlCardNo.SelectedValue + "-"+upSuperAdmin.Value+"');", true);  //Open New Tab for Sever side code
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=AppoinmentLetter-" + ddlCardNo.SelectedValue + "-"+upSuperAdmin.Value+"');", true);  //Open New Tab for Sever side code
            }
        }
    }
}