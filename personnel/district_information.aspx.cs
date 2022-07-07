using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class district_information : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
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
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='district_information.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
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
            divindivisual.Visible = false;
            rdbindividual.Checked = false;
           
        }

        protected void rdbindividual_CheckedChanged(object sender, EventArgs e)
        {
            rdball.Checked = false;
            divindivisual.Visible = true;
            classes.commonTask.LoadDistrict(ddlDistrictName);        
        }

        protected void btnPrintpreview_Click(object sender, EventArgs e)
        {
            if (rdball.Checked == true)
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeProfile where ActiveSalary='True' Group by EmpId", dt);
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
                sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpAccountNo,EmpName,SalaryCount,EmpPresentSalary,PermanentDistrict,DptName,DsgName,GrdName From v_EmployeeProfile where SN " + setSn + " order by EmpCardNo", dt);
                Session["__EmployeeDistrictwiseReport__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EmployeeDistrictwiseReport-" + upSuperAdmin.Value + "');", true);  //Open New Tab for Sever side code
            }
            else if (rdbindividual.Checked == true)
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeProfile where ActiveSalary='True' and PermanentDistrict='" + ddlDistrictName.SelectedItem.Text + "' Group by EmpId", dt);
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
                dt = new DataTable();
                sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpAccountNo,EmpName,SalaryCount,EmpPresentSalary,PermanentDistrict,DptName,DsgName,GrdName From v_EmployeeProfile where SN " + setSn + " and ActiveSalary='True' order by EmpCardNo", dt);
                Session["__EmployeeDistrictwiseReport__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EmployeeDistrictwiseReport-" + upSuperAdmin.Value + "');", true);  //Open New Tab for Sever side code
            }
        }
    }
}