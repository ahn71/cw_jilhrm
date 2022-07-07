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
    public partial class leave_register : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                LoadWorkerCardNo(ddlCardNo);
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
                    dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='resignation_letter.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
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
        private void LoadWorkerCardNo(DropDownList dl)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select MAX(SN) as SN,EmpId,EmpCardNo From v_EmployeeProfile where EmpTypeId=1 and EmpStatus in ('1','8') and ActiveSalary='True' Group by EmpId,EmpCardNo order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "SN";
                dl.DataBind();
                dl.Items.Insert(0, new ListItem("Card No", "0"));
            }
            catch { }
        }
        private void LoadStaffCardNo(DropDownList dl)
        {
            dt = new DataTable();
            sqlDB.fillDataTable("Select MAX(SN) as SN,EmpId,EmpCardNo From v_EmployeeDetails where EmpTypeId=2 and EmpStatus in ('1','8') and ActiveSalary='True' Group by EmpId,EmpCardNo order by EmpCardNo", dt);
            dl.DataSource = dt;
            dl.DataTextField = "EmpCardNo";
            dl.DataValueField = "SN";
            dl.DataBind();
            dl.Items.Insert(0, new ListItem("Card No", "0"));
        }

        protected void rdbWorker_CheckedChanged(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            rdbStaff.Checked = false;
            LoadWorkerCardNo(ddlCardNo);
        }

        protected void rdbStaff_CheckedChanged(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            rdbWorker.Checked = false;
            LoadStaffCardNo(ddlCardNo);
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                for (int k = 0; k < ddlCardNo.Items.Count; k++)
                {
                    if (ddlCardNo.Items[k].Text == txtCardNo.Text)
                    {
                        ddlCardNo.SelectedIndex = k;
                        break;
                    }
                }
                if (ddlCardNo.SelectedItem.Text != txtCardNo.Text)
                {
                    lblMessage.InnerText = "warning->Please Type Valid CardNo";
                    return;
                }
            }
            catch { }
        }

        protected void btnPrintpreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (validation() == false) return;
                dt = new DataTable();
                sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,DptName From v_Personnel_EmpCurrentStatus where SN=" + ddlCardNo.SelectedValue + "", dt);
                DataTable dtAllLeave = new DataTable();
                sqlDB.fillDataTable("Select Distinct LeaveId,FromDate,ToDate,LeaveName,WeekHolydayNo,TotalDays From v_Leave_LeaveApplication where ToYear='" + DateTime.Now.Year + "' and EmpId='" + dt.Rows[0].ItemArray[0].ToString() + "'", dtAllLeave);
                DataTable dtMain = new DataTable();
                sqlDB.fillDataTable("Select EmpId,EmpCardNo,EmpName,DptName,LeaveId,LeaveName,UseLeave,TotalLeave From v_Leave_LeaveApplication where SN=" + ddlCardNo.SelectedValue + "", dtMain);
                dtMain.Rows.Clear();
                //DataTable leaveName = new DataTable();
                //var distinctRows = (from DataRow dRow in dtAllLeave.Rows
                //                    select dRow["LeaveName"]).Distinct();
                // string[] LName=new string[3];
                //foreach (var name in distinctRows)
                //{
                //    LName[0] = name.ToString();
                //    LName[1] = name.ToString();
                //}
                DataView view = new DataView(dtAllLeave);
                DataTable distinctValues = view.ToTable(true, "LeaveId", "LeaveName");
                for (int x = 0; x < distinctValues.Rows.Count; x++)
                {
                    DataTable TLeavedays = new DataTable();
                    object sumObject="",sumweekend="",LeaveName="";
                    int totaldays=0;
                    sqlDB.fillDataTable("Select LeaveDays From tblLeaveConfig where LeaveId="+distinctValues.Rows[x]["LeaveId"].ToString()+"",TLeavedays);
                    if (distinctValues.Rows[x]["LeaveName"].ToString() == "S/L")
                    {
                      
                        sumObject = dtAllLeave.Compute("Sum(TotalDays)", "LeaveId = " + distinctValues.Rows[x]["LeaveId"].ToString() + "");
                        sumweekend = dtAllLeave.Compute("Sum(WeekHolydayNo)", "LeaveId = " + distinctValues.Rows[x]["LeaveId"].ToString() + "");
                        totaldays = int.Parse(sumObject.ToString()) + int.Parse(sumweekend.ToString());
                       
                    }
                    else if (distinctValues.Rows[x]["LeaveName"].ToString() == "C/L")
                    {
                        sumObject = dtAllLeave.Compute("Sum(TotalDays)", "LeaveId = " + distinctValues.Rows[x]["LeaveId"].ToString() + "");
                        sumweekend = dtAllLeave.Compute("Sum(WeekHolydayNo)", "LeaveId = " + distinctValues.Rows[x]["LeaveId"].ToString() + "");
                        totaldays = int.Parse(sumObject.ToString()) + int.Parse(sumweekend.ToString());
                    }
                    else if (distinctValues.Rows[x]["LeaveName"].ToString() == "E/L")
                    {
                        sumObject = dtAllLeave.Compute("Sum(TotalDays)", "LeaveId = " + distinctValues.Rows[x]["LeaveId"].ToString() + "");
                        sumweekend = dtAllLeave.Compute("Sum(WeekHolydayNo)", "LeaveId = " + distinctValues.Rows[x]["LeaveId"].ToString() + "");
                        totaldays = int.Parse(sumObject.ToString()) + int.Parse(sumweekend.ToString());
                    }
                    else if (distinctValues.Rows[x]["LeaveName"].ToString() == "M/L")
                    {
                        sumObject = dtAllLeave.Compute("Sum(TotalDays)", "LeaveId = " + distinctValues.Rows[x]["LeaveId"].ToString() + "");
                        sumweekend = dtAllLeave.Compute("Sum(WeekHolydayNo)", "LeaveId = " + distinctValues.Rows[x]["LeaveId"].ToString() + "");
                        totaldays = int.Parse(sumObject.ToString()) + int.Parse(sumweekend.ToString());
                    }

                    dtMain.Rows.Add(dt.Rows[0]["EmpId"].ToString(), dt.Rows[0]["EmpCardNo"].ToString(), dt.Rows[0]["EmpName"].ToString(), dt.Rows[0]["DptName"].ToString(), distinctValues.Rows[x]["LeaveId"].ToString(), distinctValues.Rows[x]["LeaveName"].ToString(), totaldays.ToString(), TLeavedays.Rows[0]["LeaveDays"].ToString());

                }

                    //for (int i = 0; i < dtAllLeave.Rows.Count; i++)
                    //{
                    //    dtMain.Rows.Add(dt.Rows[0]["EmpId"].ToString(), dt.Rows[0]["EmpCardNo"].ToString(), dt.Rows[0]["EmpName"].ToString(), dt.Rows[0]["DptName"].ToString(), dtAllLeave.Rows[i]["LeaveId"].ToString(), dtAllLeave.Rows[i]["LeaveName"].ToString(), dtAllLeave.Rows[i]["TotalDays"].ToString(), dtAllLeave.Rows[i]["TotalDays"].ToString());
                    //}
                if (dtMain.Rows.Count > 0)
                {
                    Session["__LeaveRegister__"] = dtMain;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LeaveRegister');", true);  //Open New Tab for Sever side code
                }
                else lblMessage.InnerText = "warning->No Leave Available";
            }
            catch { }
        }
        private Boolean validation()
        {
            try
            {
                lblMessage.InnerText = "";
                if (ddlCardNo.SelectedItem.Text == "Card No")
                {
                    lblMessage.InnerText = "warning->Please Select Valid Card No";
                    return false;
                }
                return true;
            }
            catch { return false; }
        }
    }
}