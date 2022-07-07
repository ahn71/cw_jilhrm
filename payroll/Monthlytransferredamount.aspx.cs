using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.payroll
{
    public partial class Monthlytransferredamount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                loadMonthId();
                
            }
        }
        private void setPrivilege()
        {
            try
            {
                ViewState["__WriteAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='monthly_attend.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
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
        private void loadMonthId()
        {
            try
            {
                sqlDB.bindDropDownList("Select distinct MonthName From v_tblAttendanceRecord order by MonthName DESC", "MonthName", ddlMonthID);
                ddlMonthID.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlMonthID.SelectedItem.Text == "")
                {
                    lblMessage.InnerText = "warning->Please Select MonthId";
                    return;
                }
                string[] MonthID = ddlMonthID.SelectedItem.Text.Split('-');
                string Year = MonthID[0];
                string Month = MonthID[1];
                DataTable dt = new DataTable();

                sqlDB.fillDataTable("Select EmpName,DsgName,EmpAccountNo,TotalSalary from v_MonthlySalarySheet where Month='" + Month + "' and Year='" + Year + "' and SalaryCount='Bank'", dt);
                if (dt.Rows.Count > 0)
                {
                    string MonthName = "";
                    string[] getmonth = ddlMonthID.SelectedItem.Text.Split('-');
                    if (getmonth[1].Equals("01"))
                    {
                        MonthName = "January-" + getmonth[0];
                    }
                    else if (getmonth[1].Equals("02"))
                    {
                        MonthName = "February-" + getmonth[0];
                    }
                    else if (getmonth[1].Equals("03"))
                    {
                        MonthName = "March-" + getmonth[0];
                    }
                    else if (getmonth[1].Equals("04"))
                    {
                        MonthName = "April-" + getmonth[0];
                    }
                    else if (getmonth[1].Equals("05"))
                    {
                        MonthName = "May-" + getmonth[0];
                    }
                    else if (getmonth[1].Equals("06"))
                    {
                        MonthName = "June-" + getmonth[0];
                    }
                    else if (getmonth[1].Equals("07"))
                    {
                        MonthName = "July-" + getmonth[0];
                    }
                    else if (getmonth[1].Equals("08"))
                    {
                        MonthName = "August-" + getmonth[0];
                    }
                    else if (getmonth[1].Equals("09"))
                    {
                        MonthName = "September-" + getmonth[0];
                    }
                    else if (getmonth[1].Equals("10"))
                    {
                        MonthName = "October" + getmonth[0];
                    }
                    else if (getmonth[1].Equals("11"))
                    {
                        MonthName = "November-" + getmonth[0];
                    }
                    else if (getmonth[1].Equals("12"))
                    {
                        MonthName = "December-" + getmonth[0];
                    }
                    Session["__Monthlytransferredamount__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=Monthlytransferredamount-" + MonthName + "');", true);  //Open New Tab for Sever side code
                }
                else lblMessage.InnerText = "warning->No Data Available";

            }
            catch { }
        }
    }
}