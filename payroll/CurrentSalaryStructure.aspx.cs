using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using adviitRuntimeScripting;

namespace SigmaERP.payroll
{
    public partial class CurrentSalaryStructure : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
                divEmptype.Visible = false;
                divindivisual.Visible = false;
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
                    sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='CurrentSalaryStructure.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false))
                        {
                            btnPrintpreview.CssClass = "";
                            btnPrintpreview.Enabled = false;
                        }
                    }
                }
            }
            catch { }
        }

        protected void rdball_CheckedChanged(object sender, EventArgs e)
        {
            divEmptype.Visible = false;
            divindivisual.Visible = false;
            rdbEmpType.Checked = false;
            rdbindividual.Checked = false;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }

        protected void rdbEmpType_CheckedChanged(object sender, EventArgs e)
        {
            rdball.Checked = false;
            rdbindividual.Checked = false;
            divindivisual.Visible = false;
            divEmptype.Visible = true;
            rdbTypeWorker.Checked = true;
            rdbTypeStaff.Checked = false;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }

        protected void rdbindividual_CheckedChanged(object sender, EventArgs e)
        {
            rdball.Checked = false;
            rdbEmpType.Checked = false;
            divEmptype.Visible = false;
            divindivisual.Visible = true;
            rdbStaff.Checked = false;
            rdbWorker.Checked = true;
            LoadWorkerCardNo(ddlCardNo);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }

        protected void rdbTypeWorker_CheckedChanged(object sender, EventArgs e)
        {
            rdbTypeStaff.Checked = false;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }

        protected void rdbTypeStaff_CheckedChanged(object sender, EventArgs e)
        {
            rdbTypeWorker.Checked = false;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }

        protected void rdbWorker_CheckedChanged(object sender, EventArgs e)
        {
            rdbStaff.Checked = false;
            LoadWorkerCardNo(ddlCardNo);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }

        protected void rdbStaff_CheckedChanged(object sender, EventArgs e)
        {
            rdbWorker.Checked = false;
            LoadStaffCardNo(ddlCardNo);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }
        private void LoadWorkerCardNo(DropDownList dl)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select MAX(SN) as SN,EmpId,EmpCardNo From v_EmployeeProfile where EmpTypeId=1 and EmpStatus in ('1','8') Group by EmpId,EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "SN";
                dl.DataBind();
            }
            catch { }
        }
        private void LoadStaffCardNo(DropDownList dl)
        {
            dt = new DataTable();
            sqlDB.fillDataTable("Select MAX(SN) as SN,EmpId,EmpCardNo From v_EmployeeDetails where EmpTypeId=2 and EmpStatus in ('1','8') Group by EmpId,EmpCardNo", dt);
            dl.DataSource = dt;
            dl.DataTextField = "EmpCardNo";
            dl.DataValueField = "SN";
            dl.DataBind();
        }

        protected void btnPrintpreview_Click(object sender, EventArgs e)
        {
            try
            {
                string EmpType = "";
                if (rdball.Checked == true)
                {
                    dt = new DataTable();
                    sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where  EmpStatus in ('1','8') and ActiveSalary='True' Group by EmpId", dt);
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
                    sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,EmpJoinigSalary,EmpPresentSalary,BasicSalary,MedicalAllownce,ConvenceAllownce,FoodAllownce,HouseRent,HolidayAllownce,TiffinAllownce,NightAllownce,AttendanceBonus,OthersAllownce,LunchAllownce,CompanyId,DptId,CompanyName,DptName,PFAmount,Address From v_EmployeeDetails where SN " + setSn + " order by EmpCardNo", dt);
                    Session["__CurrentSalaryStructure__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=CurrentSalaryStructure-"+upSuperAdmin.Value+"');", true);  //Open New Tab for Sever side code
                }
                else if (rdbEmpType.Checked == true)
                {
                   
                    dt = new DataTable();
                    if (rdbTypeWorker.Checked == true)
                    {
                         EmpType="Worker";
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where  EmpStatus in('1','8') and EmpTypeId=1 and ActiveSalary='True' Group by EmpId", dt);
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
                        sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,EmpJoinigSalary,EmpPresentSalary,BasicSalary,MedicalAllownce,ConvenceAllownce,FoodAllownce,HouseRent,HolidayAllownce,TiffinAllownce,NightAllownce,AttendanceBonus,OthersAllownce,LunchAllownce,CompanyId,DptId,CompanyName,DptName,PFAmount,Address From v_EmployeeDetails where SN " + setSn + " order by EmpCardNo", dt = new DataTable());
                    }
                    else
                    {
                        EmpType = "Staff";
                        dt = new DataTable();
                        sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where  EmpStatus in('1','8') and EmpTypeId=2 and ActiveSalary='True' Group by EmpId", dt);
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
                        sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,EmpJoinigSalary,EmpPresentSalary,BasicSalary,MedicalAllownce,ConvenceAllownce,FoodAllownce,HouseRent,HolidayAllownce,TiffinAllownce,NightAllownce,AttendanceBonus,OthersAllownce,LunchAllownce,CompanyId,DptId,CompanyName,DptName,PFAmount,Address From v_EmployeeDetails where SN " + setSn + " order by EmpCardNo", dt = new DataTable());
                       
                    }
                    if (dt == null || dt.Rows.Count < 1) 
                    {
                        lblMessage.InnerText = "warning-> Any "+EmpType+" Are Not Available";
                        return;
                    }
                    Session["__CurrentSalaryStructure__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=CurrentSalaryStructure-"+upSuperAdmin.Value+"');", true);  //Open New Tab for Sever side code
                }
                else if (rdbindividual.Checked == true)
                {
                    dt = new DataTable();
                    sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,EmpJoinigSalary,EmpPresentSalary,BasicSalary,MedicalAllownce,ConvenceAllownce,FoodAllownce,HouseRent,HolidayAllownce,TiffinAllownce,NightAllownce,AttendanceBonus,OthersAllownce,LunchAllownce,CompanyId,DptId,CompanyName,DptName,PFAmount,Address From v_EmployeeDetails where SN=" + ddlCardNo.SelectedValue + " order by EmpCardNo", dt);
                      if (dt == null || dt.Rows.Count < 1) 
                    {
                        EmpType = (rdbWorker.Checked == true) ? "Worker" : "Staff";
                        lblMessage.InnerText = "warning-> Any "+EmpType+" Are Not Available";
                        return;
                    }
                    Session["__CurrentSalaryStructure__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=CurrentSalaryStructure-"+upSuperAdmin.Value+"');", true);  //Open New Tab for Sever side code
                }
            }
            catch { }
        }
    }
}