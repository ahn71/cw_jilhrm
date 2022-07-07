using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using adviitRuntimeScripting;
using System.Data.SqlClient;

namespace SigmaERP.attendance
{
    public partial class monthly_attend : System.Web.UI.Page
    {
               
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                loadMonthId();
                classes.commonTask.loadDivision(ddlDivision);
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
                            btnpreview.CssClass = "";
                            btnpreview.Enabled = false;
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
                sqlDB.bindDropDownList("Select distinct MonthName From v_tblAttendanceRecord order by MonthName", "MonthName", ddlMonthName);
                ddlMonthName.Items.Insert(0, new ListItem(string.Empty, "0"));
            }
            catch { }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDepartment(ddlDivision.SelectedValue, lstAll);
        }
        private void LoadDepartment(string divisionId, ListBox lst)
        {
            try
            {
               DataTable dt = new DataTable();

                sqlDB.fillDataTable("SELECT DptId, DptName FROM HRD_Department where DId=" + divisionId + "", dt);

                lst.DataValueField = "DptId";
                lst.DataTextField = "DptName";
                lst.DataSource = dt;
                lst.DataBind();
            }
            catch { }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstAll, lstSelected);
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

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            AddRemoveAll(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            AddRemoveAll(lstSelected, lstAll);
        }

        protected void rdbStaff_CheckedChanged(object sender, EventArgs e)
        {
            rdbWorker.Checked = false;
        }

        protected void rdbWorker_CheckedChanged(object sender, EventArgs e)
        {
            rdbStaff.Checked = false;
        }

        protected void btnpreview_Click(object sender, EventArgs e)
        {

            try
            {
                loadingImg.Visible = true;
                SqlCommand cmd = new SqlCommand("Delete From MonthlyAttendance", sqlDB.connection);
                cmd.ExecuteNonQuery();
                string[] getMonth = ddlMonthName.SelectedItem.Text.Split('-');
                DataTable dtTD = new DataTable();
                sqlDB.fillDataTable("Select TotalDays From tblMonthSetup where MonthName='" + getMonth[1] + "-" + getMonth[0] + "' ", dtTD);
                string setPredicate = "";
                for (byte b = 0; b < lstSelected.Items.Count; b++)
                {

                    if (b == 0 && b == lstSelected.Items.Count - 1)
                    {
                        setPredicate = "in('" + lstSelected.Items[b].Value + "')";
                    }
                    else if (b == 0 && b != lstSelected.Items.Count - 1)
                    {
                        setPredicate += "in ('" + lstSelected.Items[b].Value + "'";
                    }
                    else if (b != 0 && b == lstSelected.Items.Count - 1)
                    {
                        setPredicate += ",'" + lstSelected.Items[b].Value + "')";
                    }
                    else setPredicate += ",'" + lstSelected.Items[b].Value + "'";
                }
                DataTable dtempid = new DataTable();
                sqlDB.fillDataTable("Select Distinct EmpId From v_tblAttendanceRecord where MonthName='" + ddlMonthName.SelectedItem.Text + "'  and DptId " + setPredicate + "", dtempid);
                for (int i = 0; i < dtempid.Rows.Count; i++)
                {

                    int w = 0, h = 0, cl = 0, sl = 0, el = 0, ad = 0, pd = 0;
                    DataTable dtatt = new DataTable();
                    sqlDB.fillDataTable("Select Distinct ATTStatus,StateStatus,Convert(varchar(11),ATTDate,105) as ATTDate From v_tblAttendanceRecord where MonthName='" + ddlMonthName.SelectedItem.Text + "'  and DptId " + setPredicate + " and EmpId='" + dtempid.Rows[i]["EmpId"].ToString() + "' order by ATTDate", dtatt);
                    for (int j = 0; j < dtatt.Rows.Count; j++)
                    {

                        string lvstatus = "", status = "";

                        if (dtatt.Rows[j]["ATTStatus"].ToString() == "LV")
                        {
                            lvstatus = dtatt.Rows[j]["StateStatus"].ToString();
                        }
                        else
                        {
                            lvstatus = dtatt.Rows[j]["ATTStatus"].ToString();
                        }
                        status = dtatt.Rows[j]["ATTStatus"].ToString();

                        switch (status)
                        {
                            case "A":
                                ad++;
                                break;
                            case "P":
                                pd++;
                                break;
                            case "W":
                                w++;
                                break;
                            case "H":
                                h++;
                                break;
                            case "LV":
                                if (lvstatus == "C/L")
                                    cl++;
                                else if (lvstatus == "S/L")
                                    sl++;
                                else if (lvstatus == "E/L")
                                    el++;
                                break;
                            case "L":
                                lvstatus = "P";
                                pd++;
                                break;
                        }
                        string[] getday = dtatt.Rows[j]["ATTDate"].ToString().Split('-');
                        int day = int.Parse(getday[0]);

                        if (j == 0)
                        {

                            SqlCommand cmdInsert = new SqlCommand("Insert Into MonthlyAttendance(EmpId,day" + day + ") values('" + dtempid.Rows[i]["EmpId"].ToString() + "','" + lvstatus + "')", sqlDB.connection);
                            cmdInsert.ExecuteNonQuery();
                        }
                        else
                        {
                            SqlCommand cmdupdate = new SqlCommand("Update MonthlyAttendance set day" + day + "='" + lvstatus + "' where EmpId='" + dtempid.Rows[i]["EmpId"].ToString() + "'", sqlDB.connection);

                            cmdupdate.ExecuteNonQuery();
                        }

                    }

                    SqlCommand cmdstatus = new SqlCommand("Update MonthlyAttendance set Totaldays=" + dtTD.Rows[0]["TotalDays"].ToString() + ", Weckend=" + w + ",Holiday=" + h + ",CL=" + cl + ",SL=" + sl + ",EL=" + el + ",AD=" + ad + ",PD=" + pd + " where EmpId='" + dtempid.Rows[i]["EmpId"].ToString() + "'", sqlDB.connection);
                    cmdstatus.ExecuteNonQuery();
                }
                string MonthName = "";
                string[] getmonth = ddlMonthName.SelectedItem.Text.Split('-');
                if (getmonth[1].Equals("01"))
                {
                    MonthName = "JAN" + getmonth[0].Substring(2, 2);
                }
                else if (getmonth[1].Equals("02"))
                {
                    MonthName = "FEB" + getmonth[0].Substring(2, 2);
                }
                else if (getmonth[1].Equals("03"))
                {
                    MonthName = "MAR" + getmonth[0].Substring(2, 2);
                }
                else if (getmonth[1].Equals("04"))
                {
                    MonthName = "APR" + getmonth[0].Substring(2, 2);
                }
                else if (getmonth[1].Equals("05"))
                {
                    MonthName = "MAY" + getmonth[0].Substring(2, 2);
                }
                else if (getmonth[1].Equals("06"))
                {
                    MonthName = "JUN" + getmonth[0].Substring(2, 2);
                }
                else if (getmonth[1].Equals("07"))
                {
                    MonthName = "JUL" + getmonth[0].Substring(2, 2);
                }
                else if (getmonth[1].Equals("08"))
                {
                    MonthName = "AUG" + getmonth[0].Substring(2, 2);
                }
                else if (getmonth[1].Equals("09"))
                {
                    MonthName = "SEP" + getmonth[0].Substring(2, 2);
                }
                else if (getmonth[1].Equals("10"))
                {
                    MonthName = "OCT" + getmonth[0].Substring(2, 2);
                }
                else if (getmonth[1].Equals("11"))
                {
                    MonthName = "NOV" + getmonth[0].Substring(3, 4);
                }
                else if (getmonth[1].Equals("12"))
                {
                    MonthName = "DEC" + getmonth[0].Substring(2, 2);
                }
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_MonthlyAttendance where IsActive=1  Group by EmpId", dt);
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
                sqlDB.fillDataTable("Select * from v_MonthlyAttendance where SN " + setSn + "", dt);
                Session["__Monthlyattendance__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=Monthlyattendance-" + MonthName + "');", true);  //Open New Tab for Sever side code

            }
            catch { }
        }
       
    }
}