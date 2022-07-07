using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using adviitRuntimeScripting;

namespace SigmaERP.attendance
{
    public partial class DailyAttStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                //LoadAttStatus(ddlAttStatus);
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
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='DailyAttStatus.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
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

        private void LoadAttStatus(DropDownList dl)
        {
            try
            {
                DataTable dt=new DataTable();
                sqlDB.fillDataTable("Select Distinct LeaveName From tblLeaveConfig ", dt = new DataTable());
                dl.DataSource = dt;
                dl.DataTextField = "LeaveName";
                dl.DataValueField = "";
                dl.DataBind();
                sqlDB.fillDataTable("Select Distinct ATTStatus From tblAttendanceRecord", dt = new DataTable());
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["ATTStatus"].ToString() != "LV")
                    dl.Items.Add(dt.Rows[i]["ATTStatus"].ToString());
                }
                dl.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
           
            try
            {
                lblMessage.InnerText = "";
                if (ddlAttStatus.SelectedItem.Text == "-Select-")
                {
                    lblMessage.InnerText = "warning->Please Select Attendance Status";
                    return;
                }
               else if (txtDate.Text == "")
                {
                    lblMessage.InnerText = "warning->Please Select To Date";
                    return;
                }
               else if (txtFromDate.Text == "")
                {
                    lblMessage.InnerText = "warning->Please Select From Date";
                    return;
                }
                else  if (Convert.ToDateTime(txtDate.Text) > Convert.ToDateTime(txtFromDate.Text))
                {
                    lblMessage.InnerText = "warning->Must be Maximum To Date to From Date";
                    return;
                }
                string[] getMonth = txtDate.Text.Split('-');
                string[] getFromDate = txtFromDate.Text.Split('-');
                DataTable dt = new DataTable();
                if (ddlAttStatus.SelectedItem.Text == "Casula Leave" || ddlAttStatus.SelectedItem.Text == "Sick Leave" || ddlAttStatus.SelectedItem.Text == "Earned Leave" || ddlAttStatus.SelectedItem.Text == "Maternity Leave")
                {
                    string LeaveName = ddlAttStatus.SelectedItem.Text;
                    string Leave = "";
                    switch (LeaveName)
                    {
                        case "Casula Leave":
                            Leave = "C/L";
                            break;
                        case "Sick Leave":
                            Leave = "S/L";
                            break;
                        case "Earned Leave":
                            Leave = "E/L";
                            break;
                        case "Maternity Leave":
                            Leave = "M/L";
                            break;
                    }
                    dt = new DataTable();
                    sqlDB.fillDataTable("Select Max(SN) as SN,EmpId  From v_tblAttendanceRecord where ATTDate between'" + getMonth[2] + "-" + getMonth[1] + "-" + getMonth[0] + "' and '" + getFromDate[2] + "-" + getFromDate[1] + "-" + getFromDate[0] + "'  and StateStatus='" + Leave + "' and ActiveSalary='True' Group by EmpId", dt);
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

                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,DptName,DsgName,LnCode,EmpType From v_tblAttendanceRecord where ATTDate between'" + getMonth[2] + "-" + getMonth[1] + "-" + getMonth[0] + "' and '" + getFromDate[2] + "-" + getFromDate[1] + "-" + getFromDate[0] + "'  and StateStatus='" + Leave + "' and ActiveSalary='True' and SN " + setSn + "  order by LnCode,EmpCardNo", dt);
                }
                else
                {
                    string AttStatus = ddlAttStatus.SelectedItem.Text;
                    string Status = "";
                    switch (AttStatus)
                    {
                        case "Present":
                            Status = "P";
                            break;
                        case "Absent":
                            Status = "A";
                            break;
                        case "Late":
                            Status = "L";
                            break;
                        case "Weekend":
                            Status = "W";
                            break;
                        case "Holiday":
                            Status = "H";
                            break;
                    }

                    dt = new DataTable();
                    sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From v_tblAttendanceRecord where ATTDate between'" + getMonth[2] + "-" + getMonth[1] + "-" + getMonth[0] + "' and '" + getFromDate[2] + "-" + getFromDate[1] + "-" + getFromDate[0] + "' and ATTStatus='" + Status + "'and ActiveSalary='True' Group by EmpId", dt);
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
                    sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,DptName,DsgName,LnCode,EmpType From v_tblAttendanceRecord where ATTDate between'" + getMonth[2] + "-" + getMonth[1] + "-" + getMonth[0] + "' and '" + getFromDate[2] + "-" + getFromDate[1] + "-" + getFromDate[0] + "' and ATTStatus='" + Status + "'and ActiveSalary='True' and SN " + setSn + "  order by LnCode,EmpCardNo", dt);
                }
                Session["__DailyAttStatus__"] = dt;
                if (dt.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=DailyAttStatus-" + ddlAttStatus.SelectedItem.Text + "-" + txtDate.Text + "-" + txtFromDate.Text + "');", true);  //Open New Tab for Sever side code
                }
                else
                {
                    lblMessage.InnerText = "warning->No Data Available";
                }
            }
            catch { }
    }
    }
}