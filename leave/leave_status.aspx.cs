using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using adviitRuntimeScripting;

namespace SigmaERP.personnel
{
    public partial class leave_status : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                classes.commonTask.LoadMonthName(ddlMonth);
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
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='leave_status.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                        {
                            btnPreview.CssClass = "";
                            btnPreview.Enabled = false;
                            btnMaternityLeave.CssClass = "";
                            btnMaternityLeave.Enabled = false;

                        }
                    }
                }

            }
            catch { }

        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                string getMonth = ddlMonth.SelectedItem.ToString().Substring(3, 4) + "-" + ddlMonth.SelectedItem.ToString().Substring(0, 2);
                DataTable dt = new DataTable();
                if (rdoDept.SelectedIndex == 0)
                {
                    sqlDB.fillDataTable("select EmpId, LACode,EmpCardNo,DptName,DptId,TotalDays,Remarks,LeaveName,FromMonth,ToMonth,Convert(varchar(11),FromDate,111)as FromDate,Convert(varchar(11),ToDate,111)As ToDate  from v_Leave_LeaveApplication group by LACode,EmpCardNo,DptName,DptId,TotalDays,Remarks,LeaveName,FromMonth,ToMonth,FromDate,ToDate,EmpId having ToMonth='" + getMonth + "' AND LeaveName Not In ('M/L') order by dptId", dt);
                }
                else
                {
                    sqlDB.fillDataTable("select EmpId, LACode,EmpCardNo,DptName,DptId,TotalDays,Remarks,LeaveName,FromMonth,ToMonth,Convert(varchar(11),FromDate,111)as FromDate,Convert(varchar(11),ToDate,111)As ToDate  from v_Leave_LeaveApplication group by LACode,EmpCardNo,DptName,DptId,TotalDays,Remarks,LeaveName,FromMonth,ToMonth,FromDate,ToDate,EmpId having ToMonth='" + getMonth + "' AND LeaveName Not In ('M/L') and dptId='" + ddlDepartment.SelectedValue + "' order by dptId", dt);
                }
                
                DataTable dtCurrentMonthInfo = dt.Select(" FromMonth='"+getMonth+"' AND ToMonth='"+getMonth+"' ").CopyToDataTable();
                DataTable dtNotCurrentMonthInfo = new DataTable();
                try
                {
                    dtNotCurrentMonthInfo = dt.Select(" fromMonth not in ('" + getMonth + "')").CopyToDataTable();
                }
                catch { }
                for (int i = 0; i < dtNotCurrentMonthInfo.Rows.Count; i++)
                {
                    DataTable dtLeaveInfo = new DataTable();

                    sqlDB.fillDataTable("select datediff (day,'"+getMonth+"-01','" + dtNotCurrentMonthInfo.Rows[i]["ToDate"].ToString() + "') as TotalDays from v_Leave_LeaveApplication where LACode=" + dtNotCurrentMonthInfo.Rows[i]["LACode"].ToString() + "", dtLeaveInfo);

                    int getLeaveDays = int.Parse(dtLeaveInfo.Rows[0]["TotalDays"].ToString());
                    getLeaveDays ++;
                    dtCurrentMonthInfo.Rows.Add(dtNotCurrentMonthInfo.Rows[i]["EmpId"].ToString(), dtNotCurrentMonthInfo.Rows[i]["LACode"].ToString(), dtNotCurrentMonthInfo.Rows[i]["EmpCardNo"].ToString(), dtNotCurrentMonthInfo.Rows[i]["DptName"].ToString(),
                                                dtNotCurrentMonthInfo.Rows[i]["DptId"].ToString(),getLeaveDays, dtNotCurrentMonthInfo.Rows[i]["Remarks"].ToString(),
                                                dtNotCurrentMonthInfo.Rows[i]["LeaveName"].ToString(), getMonth, dtNotCurrentMonthInfo.Rows[i]["ToMonth"].ToString(), getMonth + "01", dtNotCurrentMonthInfo.Rows[i]["ToDate"].ToString());

                }

                dtNotCurrentMonthInfo = new DataTable();

                sqlDB.fillDataTable("select EmpId, LACode,EmpCardNo,DptName,DptId,TotalDays,Remarks,LeaveName,FromMonth,ToMonth,Convert(varchar(11),FromDate,111)as FromDate,Convert(varchar(11),ToDate,111)As ToDate  from v_Leave_LeaveApplication group by LACode,EmpCardNo,DptName,DptId,TotalDays,Remarks,LeaveName,FromMonth,ToMonth,FromDate,ToDate,EmpId having ToMonth >'" + getMonth + "' AND FromMonth='" + getMonth + "' AND LeaveName Not In ('M/L') order by dptId", dtNotCurrentMonthInfo);

                int getDaysInMonth = DateTime.DaysInMonth(int.Parse(ddlMonth.SelectedItem.ToString().Substring(3, 4)), int.Parse(ddlMonth.SelectedItem.ToString().Substring(0, 2)));
               
                for (int i = 0; i < dtNotCurrentMonthInfo.Rows.Count; i++)
                {
                    DataTable dtLeaveInfo = new DataTable();

                    sqlDB.fillDataTable("select datediff (day,'" + dtNotCurrentMonthInfo.Rows[i]["FromDate"].ToString() + "','" + getMonth + "-" + getDaysInMonth + "') as TotalDays from v_Leave_LeaveApplication where LACode=" + dtNotCurrentMonthInfo.Rows[i]["LACode"].ToString() + "", dtLeaveInfo);

                    int getLeaveDays = int.Parse(dtLeaveInfo.Rows[0]["TotalDays"].ToString());
                    getLeaveDays++;
                    dtCurrentMonthInfo.Rows.Add(dtNotCurrentMonthInfo.Rows[i]["EmpId"].ToString(), dtNotCurrentMonthInfo.Rows[i]["LACode"].ToString(), dtNotCurrentMonthInfo.Rows[i]["EmpCardNo"].ToString(), dtNotCurrentMonthInfo.Rows[i]["DptName"].ToString(),
                                                dtNotCurrentMonthInfo.Rows[i]["DptId"].ToString(), getLeaveDays, dtNotCurrentMonthInfo.Rows[i]["Remarks"].ToString(),
                                                dtNotCurrentMonthInfo.Rows[i]["LeaveName"].ToString(), getMonth, dtNotCurrentMonthInfo.Rows[i]["ToMonth"].ToString(), getMonth + "01", dtNotCurrentMonthInfo.Rows[i]["ToDate"].ToString());

                }
                string  MonthName = "";
                string[] getmonth = ddlMonth.SelectedItem.Text.Split('-');
                if (getmonth[0].Equals("01"))
                {
                    MonthName = "JAN" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("02"))
                {
                    MonthName = "FEB" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("03"))
                {
                    MonthName = "MAR" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("04"))
                {
                    MonthName = "APR" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("05"))
                {
                    MonthName = "MAY" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("06"))
                {
                    MonthName = "JUN" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("07"))
                {
                    MonthName = "JUL" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("08"))
                {
                    MonthName = "AUG" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("09"))
                {
                    MonthName = "SEP" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("10"))
                {
                    MonthName = "OCT" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("11"))
                {
                    MonthName = "NOV" + getmonth[1].Substring(3, 4);
                }
                else if (getmonth[0].Equals("12"))
                {
                    MonthName = "DEC" + getmonth[1].Substring(2, 2);
                }
                Session["__LeaveStatusSummary__"] = dtCurrentMonthInfo;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LeaveStatusSummary-" + MonthName+ "');", true);  //Open New Tab for Sever side code 

            }
            catch { }
        }

        protected void rdoDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoDept.SelectedIndex == 1)
                {
                    classes.commonTask.loadDepartment(ddlDepartment);
                    ddlDepartment.Enabled = true;
                }
                else
                {
                    ddlDepartment.Items.Clear();
                    ddlDepartment.Enabled = false;
                }
            }
            catch { }
        }


        protected void btnMaternityLeave_Click(object sender, EventArgs e)
        {
            try
            {
                string getMonth = ddlMonth.SelectedItem.ToString().Substring(3, 4) + "-" + ddlMonth.SelectedItem.ToString().Substring(0, 2);

                int getDaysInMonth = DateTime.DaysInMonth(int.Parse(ddlMonth.SelectedItem.ToString().Substring(3, 4)), int.Parse(ddlMonth.SelectedItem.ToString().Substring(0, 2)));

                DataTable dt = new DataTable();
                if (rdoDept.SelectedIndex == 0)
                {
                    sqlDB.fillDataTable("select EmpId,LACode,EmpCardNo,DptName,DptId,TotalDays,Remarks,LeaveName,FromMonth,ToMonth,Convert(varchar(11),FromDate,111)as FromDate,Convert(varchar(11),ToDate,111)As ToDate  from v_Leave_LeaveApplication group by EmpId,LACode,EmpCardNo,DptName,DptId,TotalDays,Remarks,LeaveName,FromMonth,ToMonth,FromDate,ToDate having  FromMonth <='" + getMonth + "' AND ToMonth >='" + getMonth + "' AND LeaveName In ('M/L') order by dptId", dt);
                }
                else
                {
                    sqlDB.fillDataTable("select EmpId,LACode,EmpCardNo,DptName,DptId,TotalDays,Remarks,LeaveName,FromMonth,ToMonth,Convert(varchar(11),FromDate,111)as FromDate,Convert(varchar(11),ToDate,111)As ToDate  from v_Leave_LeaveApplication group by EmpId,LACode,EmpCardNo,DptName,DptId,TotalDays,Remarks,LeaveName,FromMonth,ToMonth,FromDate,ToDate having  FromMonth <='" + getMonth + "' AND ToMonth >='" + getMonth + "' AND LeaveName In ('M/L') and dptId='" + ddlDepartment.SelectedValue + "' order by dptId", dt);
                }

                DataTable dtCurrentMonthInfo = new DataTable();
                DataTable dtMLLeaveInfo = new DataTable();
                try
                {

                    dtMLLeaveInfo = dt.Copy(); dtMLLeaveInfo.Rows.Clear();
                    dtCurrentMonthInfo = dt.Select(" FromMonth='" + getMonth + "'").CopyToDataTable();

                }
                catch { }

                for (int i = 0; i < dtCurrentMonthInfo.Rows.Count; i++)
                {
                    DataTable dtLeaveInfo = new DataTable();

                    sqlDB.fillDataTable("select datediff (day,'" + dtCurrentMonthInfo.Rows[i]["FromDate"].ToString() + "','" + getMonth + "-" + getDaysInMonth + "') as TotalDays from v_Leave_LeaveApplication where LACode=" + dtCurrentMonthInfo.Rows[i]["LACode"].ToString() + "", dtLeaveInfo);

                    int getLeaveDays = int.Parse(dtLeaveInfo.Rows[0]["TotalDays"].ToString());
                    getLeaveDays++;
                    dtMLLeaveInfo.Rows.Add(dtCurrentMonthInfo.Rows[i]["EmpId"].ToString(), dtCurrentMonthInfo.Rows[i]["LACode"].ToString(), dtCurrentMonthInfo.Rows[i]["EmpCardNo"].ToString(), dtCurrentMonthInfo.Rows[i]["DptName"].ToString(),
                                                dtCurrentMonthInfo.Rows[i]["DptId"].ToString(), getLeaveDays, dtCurrentMonthInfo.Rows[i]["Remarks"].ToString(),
                                                dtCurrentMonthInfo.Rows[i]["LeaveName"].ToString(), getMonth, dtCurrentMonthInfo.Rows[i]["ToMonth"].ToString(), dtCurrentMonthInfo.Rows[i]["FromDate"].ToString(), getMonth + "-" + getDaysInMonth);

                }

                DataTable dtNotCurrentMonthInfo = new DataTable();

                try
                {
                    dtNotCurrentMonthInfo = dt.Select(" fromMonth not in ('" + getMonth + "') AND ToMonth not in ('" + getMonth + "')").CopyToDataTable();
                }
                catch { }

                for (int i = 0; i < dtNotCurrentMonthInfo.Rows.Count; i++)
                {

                    dtMLLeaveInfo.Rows.Add(dtNotCurrentMonthInfo.Rows[i]["EmpId"].ToString(), dtNotCurrentMonthInfo.Rows[i]["LACode"].ToString(), dtNotCurrentMonthInfo.Rows[i]["EmpCardNo"].ToString(), dtNotCurrentMonthInfo.Rows[i]["DptName"].ToString(),
                                                dtNotCurrentMonthInfo.Rows[i]["DptId"].ToString(), getDaysInMonth, dtNotCurrentMonthInfo.Rows[i]["Remarks"].ToString(),
                                                dtNotCurrentMonthInfo.Rows[i]["LeaveName"].ToString(), getMonth, dtNotCurrentMonthInfo.Rows[i]["ToMonth"].ToString(), getMonth + "-01", getMonth + "-" + getDaysInMonth);

                }

                try
                {
                    dtCurrentMonthInfo = dt.Select(" ToMonth='" + getMonth + "'").CopyToDataTable();
                }
                catch { }

                for (int i = 0; i < dtCurrentMonthInfo.Rows.Count; i++)
                {
                    DataTable dtLeaveInfo = new DataTable();

                    sqlDB.fillDataTable("select datediff (day,'" + getMonth + "-01','" + dtCurrentMonthInfo.Rows[i]["ToDate"].ToString() + "') as TotalDays from v_Leave_LeaveApplication where LACode=" + dtNotCurrentMonthInfo.Rows[i]["LACode"].ToString() + "", dtLeaveInfo);

                    int getLeaveDays = int.Parse(dtLeaveInfo.Rows[0]["TotalDays"].ToString());
                    getLeaveDays++;
                    dtMLLeaveInfo.Rows.Add(dtCurrentMonthInfo.Rows[i]["EmpId"].ToString(), dtCurrentMonthInfo.Rows[i]["LACode"].ToString(), dtCurrentMonthInfo.Rows[i]["EmpCardNo"].ToString(), dtCurrentMonthInfo.Rows[i]["DptName"].ToString(),
                                                dtCurrentMonthInfo.Rows[i]["DptId"].ToString(), getLeaveDays, dtCurrentMonthInfo.Rows[i]["Remarks"].ToString(),
                                                dtCurrentMonthInfo.Rows[i]["LeaveName"].ToString(), getMonth, dtCurrentMonthInfo.Rows[i]["ToMonth"].ToString(), getMonth + "-01", dtCurrentMonthInfo.Rows[i]["ToDate"].ToString());

                }
                string MonthName = "";
                string[] getmonth = ddlMonth.SelectedItem.Text.Split('-');
                if (getmonth[0].Equals("01"))
                {
                    MonthName = "JAN" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("02"))
                {
                    MonthName = "FEB" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("03"))
                {
                    MonthName = "MAR" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("04"))
                {
                    MonthName = "APR" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("05"))
                {
                    MonthName = "MAY" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("06"))
                {
                    MonthName = "JUN" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("07"))
                {
                    MonthName = "JUL" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("08"))
                {
                    MonthName = "AUG" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("09"))
                {
                    MonthName = "SEP" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("10"))
                {
                    MonthName = "OCT" + getmonth[1].Substring(2, 2);
                }
                else if (getmonth[0].Equals("11"))
                {
                    MonthName = "NOV" + getmonth[1].Substring(3, 4);
                }
                else if (getmonth[0].Equals("12"))
                {
                    MonthName = "DEC" + getmonth[1].Substring(2, 2);
                }
                Session["__LeaveStatusSummary__"] = dtMLLeaveInfo;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LeaveStatusSummary-" + MonthName + "');", true);  //Open New Tab for Sever side code 
            }
            catch { }
        }
    }
}