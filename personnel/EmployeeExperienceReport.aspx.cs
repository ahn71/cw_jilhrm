using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace SigmaERP.personnel
{
    public partial class EmployeeExperienceReport : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                trIndivisualCardNo.Visible = false;
            }
        }
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='EmployeeExperienceReport.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                        {
                            btnpreview.CssClass = "";

                            btnpreview.Enabled = false;


                        }
                    }
                }

            }
            catch { }

        }

        protected void rdbAll_CheckedChanged(object sender, EventArgs e)
        {
            trIndivisualCardNo.Visible = false;
            rdbIndividual.Checked = false;
        }

        protected void rdbIndividual_CheckedChanged(object sender, EventArgs e)
        {
            rdbAll.Checked = false;
            trIndivisualCardNo.Visible = true;
            EmpCardNo(ddlEmpCardNo);
        }
        private void EmpCardNo(DropDownList dl)
        {
            dt = new DataTable();
            sqlDB.fillDataTable("Select MAX(SN) as SN,EmpId,EmpCardNo From v_EmployeeDetails where EmpStatus in ('1','8') and ActiveSalary='True' Group by EmpId,EmpCardNo order by EmpCardNo", dt);
            dl.DataSource = dt;
            dl.DataTextField = "EmpCardNo";
            dl.DataValueField = "SN";
            dl.DataBind();
        }

        protected void btnpreview_Click(object sender, EventArgs e)
        {
            if (rdbAll.Checked)
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_ContactInfo where  EmpStatus in ('1','8') and ActiveSalary='True' Group by EmpId", dt);
                string setSn = "", setEmpId = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0 && i == dt.Rows.Count - 1)
                    {
                        setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        setEmpId = "in('" + dt.Rows[i].ItemArray[1].ToString() + "')";
                    }
                    else if (i == 0 && i != dt.Rows.Count - 1)
                    {
                        setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        setEmpId += "in ('" + dt.Rows[i].ItemArray[1].ToString() + "'";
                    }
                    else if (i != 0 && i == dt.Rows.Count - 1)
                    {
                        setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        setEmpId += ",'" + dt.Rows[i].ItemArray[1].ToString() + "')";
                    }
                    else
                    {
                        setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        setEmpId += ",'" + dt.Rows[i].ItemArray[1].ToString() + "'";
                    }
                }
                dt = new DataTable();
                sqlDB.fillDataTable("Select EmpName,EmpCardNo,EmpTypeId,DptName,DsgName,CompanyName,Designation,YearOfExp,Convert(varchar(10),JoiningDate) as JoiningDate,Convert(varchar(10),ResignDate) as ResignDate From v_EmployeeExperience where ActiveSalary='True' and SN "+setSn+"  order by EmpCardNo ", dt = new DataTable());

            }
            else
            {
                sqlDB.fillDataTable("Select Max(SN) as SN,EmpName,EmpCardNo,EmpTypeId,DptName,DsgName,CompanyName,Designation,YearOfExp,Convert(varchar(10),JoiningDate) as JoiningDate,Convert(varchar(10),ResignDate) as ResignDate From v_EmployeeExperience where SN=" + ddlEmpCardNo.SelectedValue + " and ActiveSalary='True' group by EmpName,EmpCardNo,EmpTypeId,DptName,DsgName,CompanyName,Designation,YearOfExp, JoiningDate,ResignDate order by EmpCardNo ", dt = new DataTable());
            }
            Session["__EmployeeExperience__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EmployeeExperience');", true);  //Open New Tab for Sever side code
        }
    }
}