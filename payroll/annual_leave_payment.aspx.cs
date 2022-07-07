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
    public partial class annual_leave_payment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();

            this.btnaddall.Click += new System.EventHandler(this.btnaddall_Click);
            this.btnadditem.Click += new System.EventHandler(this.btnadditem_Click);
            this.btnremoveitem.Click += new System.EventHandler(this.btnremoveitem_Click);
            this.btnremoveall.Click += new System.EventHandler(this.btnremoveall_Click);
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);

            if (!IsPostBack)
            {
                setPrivilege();
                loadMonthName(); loadDevision();
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


                        }
                    }
                }

            }
            catch { }

        }
        private void loadMonthName()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select distinct generateMonth from v_Leave_YearlyEarnLeaveGeneration  ", dt);
                ddlMonthID.DataValueField = "generateMonth";
                ddlMonthID.DataTextField = "generateMonth";
                ddlMonthID.DataSource = dt;
                ddlMonthID.DataBind();
                ddlMonthID.Items.Insert(0, new ListItem(" ", "00"));
            }
            catch { }

        }
        private void loadDevision() //Load Devision 
        {
            try
            {
                ddlDivision.Items.Clear();
                ddlDivision.Items.Add(" ");
                sqlDB.bindDropDownList("Select distinct DName From v_tblAttendanceRecord", "DName", ddlDivision);


            }
            catch { }
        }

        private void AddRemoveAll(ListBox aSource, ListBox aTarget)
        {
            try
            {
                foreach (ListItem item in aSource.Items)
                {
                    aTarget.Items.Add(item);
                }
                aSource.Items.Clear();
            }
            catch (Exception expException)
            {
                Response.Write(expException.Message);
            }
        }

        private void AddRemoveItem(ListBox aSource, ListBox aTarget)
        {
            ListItemCollection licCollection;
            try
            {
                licCollection = new ListItemCollection();
                for (int intCount = 0; intCount < aSource.Items.Count; intCount++)
                {
                    if (aSource.Items[intCount].Selected == true)
                        licCollection.Add(aSource.Items[intCount]);
                }

                for (int intCount = 0; intCount < licCollection.Count; intCount++)
                {
                    aSource.Items.Remove(licCollection[intCount]);
                    aTarget.Items.Add(licCollection[intCount]);
                }
            }
            catch (Exception expException)
            {
                Response.Write(expException.Message);
            }
            finally
            {
                licCollection = null;
            }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select distinct DptName From v_tblAttendanceRecord Where DName='" + ddlDivision.SelectedItem.Text + "' ", dt);
                lstEmployees.DataTextField = "DptName";
                lstEmployees.DataSource = dt;
                lstEmployees.DataBind();
            }
            catch { }
        }
        private void btnaddall_Click(object sender, System.EventArgs e)
        {
            AddRemoveAll(lstEmployees, lstSelectedEmployees);
        }

        private void btnadditem_Click(object sender, System.EventArgs e)
        {
            AddRemoveItem(lstEmployees, lstSelectedEmployees);
        }

        private void btnremoveitem_Click(object sender, System.EventArgs e)
        {
            AddRemoveItem(lstSelectedEmployees, lstEmployees);
        }

        private void btnremoveall_Click(object sender, System.EventArgs e)
        {
            AddRemoveAll(lstSelectedEmployees, lstEmployees);
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                loadingImg.Visible = true;
                string setPredicate = "";
                for (byte b = 0; b < lstSelectedEmployees.Items.Count; b++)
                {
                    if (b == 0 && b == lstSelectedEmployees.Items.Count - 1)
                    {
                        setPredicate = "in('" + lstSelectedEmployees.Items[b].Text + "')";
                    }
                    else if (b == 0 && b != lstSelectedEmployees.Items.Count - 1)
                    {
                        setPredicate += "in ('" + lstSelectedEmployees.Items[b].Text + "'";
                    }
                    else if (b != 0 && b == lstSelectedEmployees.Items.Count - 1)
                    {
                        setPredicate += ",'" + lstSelectedEmployees.Items[b].Text + "')";
                    }
                    else setPredicate += ",'" + lstSelectedEmployees.Items[b].Text + "'";
                }

                string getMonthName = ddlMonthID.SelectedItem.ToString().Substring(5, 2) + "-" + ddlMonthID.SelectedItem.ToString().Substring(0, 4);




                DataTable dt = new DataTable();
                if (rdoEmpType.SelectedItem.Text == "Worker")
                {
                    dt = new DataTable();
                    sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_Leave_YearlyEarnLeaveGeneration where  generateMonth='" + ddlMonthID.SelectedItem.Text.ToString() + "' And EmpType='Worker' AND DptName " + setPredicate + " Group by EmpId", dt);
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

                    sqlDB.fillDataTable("select EmpCardNo,EmpName,DsgName,Convert(varchar(11),EmpJoiningDate,105)as EmpJoiningDate,Convert(varchar(11),EarnLeavePerviousStartYear,105) as EarnLeavePerviousStartYear,Convert(varchar(11),EarnLeaveEndYear,105) as EarnLeaveEndYear,BasicSalary,NetTotal,LnCode,DptName,generateMonth,EmpType,Convert(varchar(11),GenerateDate,105) as GenerateDate,dptId,LnId,PayableDays,WorkingDays from v_Leave_YearlyEarnLeaveGeneration " +
                    "Where generateMonth='" + ddlMonthID.SelectedItem.Text.ToString() + "' And SN " + setSn + " order by DptName,LnCode", dt = new DataTable());

                }

                else
                {
                    dt = new DataTable();
                    sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_Leave_YearlyEarnLeaveGeneration where  generateMonth='" + ddlMonthID.SelectedItem.Text.ToString() + "' And EmpType='Staff' AND DptName " + setPredicate + " Group by EmpId", dt);
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

                    sqlDB.fillDataTable("select EmpCardNo,EmpName,DsgName,Convert(varchar(11),EmpJoiningDate,105)as EmpJoiningDate,Convert(varchar(11),EarnLeavePerviousStartYear,105) as EarnLeavePerviousStartYear,Convert(varchar(11),EarnLeaveEndYear,105) as EarnLeaveEndYear,BasicSalary,NetTotal,LnCode,DptName,generateMonth,EmpType,Convert(varchar(11),GenerateDate,105) as GenerateDate,dptId,LnId,PayableDays,WorkingDays from v_Leave_YearlyEarnLeaveGeneration " +
                    "Where generateMonth='" + ddlMonthID.SelectedItem.Text.ToString() + "' And SN " + setSn + " order by DptName,LnCode", dt = new DataTable());


                }


                Session["__dtEarnLeave__"] = dt;
                if (dt.Rows.Count > 0) ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTab('/All Report/Report.aspx?for=EarnLeaveInfo');", true);  //Open New Tab for Sever side code     
                else lblMessage.InnerText = "warning->No Data Available";
            }
            catch { }
        }
    }
}