using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using adviitRuntimeScripting;
using ComplexScriptingSystem;

namespace SigmaERP.attendance
{
    public partial class daily_atten_summary : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                if (!setPrivilege())
                {
                    Response.Redirect("/attendance/attendance_summary.aspx");
                    return;
                }          
               
            }
        }

        private Boolean setPrivilege()
        {
            try
            {

                ViewState["__WriteAction__"] = "1";
                ViewState["__DeletAction__"] = "1";
                ViewState["__ReadAction__"] = "1";
                ViewState["__UpdateAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];

                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Viewer"))
                {
                    return false;
                }
                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin"))
                {
                    chkForAllCompany.Checked = true;
                    ddlCompanyName.Enabled = false;
                    ddlShiftName.Enabled = false;
                    divDepartmentList.Visible = false;
                    classes.commonTask.LoadBranch(ddlCompanyName);
                    return true;
                }
               
                else
                {
                    trForCompanyList.Visible = false;
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='aplication.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);

                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false))
                        {
                            // btnSelectAll.CssClass = "";
                            //  ViewState["__ReadAction__"] = "0";
                            // btnSelectAll.Enabled = false;

                        }
                        if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                        {
                            ViewState["__WriteAction__"] = "0";
                            //  btnSaveLeave.CssClass = "";
                            //  btnSaveLeave.Enabled = false;
                        }
                        if (bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false))
                        {
                            ViewState["__UpdateAction__"] = "0";
                        }
                        if (bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                        {
                            ViewState["__DeletAction__"] = "0";
                        }
                    }
                    return true;

                }

            }
            catch { return true; }

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

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstAll, lstSelected);
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

        protected void chkForAllCompany_CheckedChanged(object sender, EventArgs e)
        {
            if (chkForAllCompany.Checked)
            {
                ddlCompanyName.Enabled = false;
                ddlShiftName.Enabled = false;
                divDepartmentList.Visible = false;
            }
            else
            {
                ddlCompanyName.Enabled = true;
                ddlShiftName.Enabled = true;
                divDepartmentList.Visible = true;
                ddlShiftName.Items.Clear();
                lstAll.DataSource = "";
                lstAll.DataBind();
                lstSelected.DataSource = "";
                lstSelected.DataBind();
            }
        }

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.commonTask.LoadShift(ddlShiftName,ddlCompanyName.SelectedValue);
            ddlShiftName.Items.RemoveAt(0);
            ddlShiftName.Items.Insert(0, new ListItem(string.Empty, "00"));
            ddlShiftName.Items.Insert(1,new ListItem("All","0000"));
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Date = dptDate.Text.Split('-');
                if (ViewState["__UserType__"].ToString().Equals("Super Admin"))
                {
                    if (chkForAllCompany.Checked == false)
                    {
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
                        if (ddlShiftName.SelectedItem.Text == "All")
                        {
                          //  sqlDB.fillDataTable("Select * From v_v_DailyAttendanceSummary where DId=" + ddlDivisionName.SelectedValue + " and DptId " + setPredicate + " and ATTDate='" + Date[2] + "-" + Date[1] + "-" + Date[0] + "'", dt = new DataTable());
                        }
                        else
                        {
                          //  sqlDB.fillDataTable("Select * From v_v_DailyAttendanceSummary where DId=" + ddlDivisionName.SelectedValue + " and DptId " + setPredicate + " and SftId=" + ddlShiftName.SelectedValue + " and ATTDate='" + Date[2] + "-" + Date[1] + "-" + Date[0] + "'", dt = new DataTable());
                        }
                        if (dt.Rows.Count > 0)
                        {
                            Session["__DailyAttSummary__"] = dt;
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=DailyAttSummary-" + dptDate.Text + "');", true);  //Open New Tab for Sever side code
                        }
                        else
                        {
                            lblMessage.InnerText = "warning-> Attendance Not available";
                        }
                    }
                    else
                    {
                        sqlDB.fillDataTable("Select * From v_v_DailyAttendanceSummary where  ATTDate='" + Date[2] + "-" + Date[1] + "-" + Date[0] + "'", dt = new DataTable());
                        if (dt.Rows.Count > 0)
                        {
                            Session["__DailyAttSummary__"] = dt;
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=DailyAttSummary-" + dptDate.Text + "');", true);  //Open New Tab for Sever side code
                        }
                        else
                        {
                            lblMessage.InnerText = "warning-> Attendance Not available";
                        }
                    }
                }
                else
                {
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
                    if (ddlShiftName.SelectedItem.Text == "All")
                    {
                       // sqlDB.fillDataTable("Select * From v_v_DailyAttendanceSummary where DId=" + ddlDivisionName.SelectedValue + " and DptId " + setPredicate + " and ATTDate='" +Date[2]+"-"+Date[1]+"-"+Date[0]+"'", dt = new DataTable());
                    }
                    else
                    {
                       // sqlDB.fillDataTable("Select * From v_v_DailyAttendanceSummary where DId=" + ddlDivisionName.SelectedValue + " and DptId " + setPredicate + " and SftId=" + ddlShiftName.SelectedValue + " and ATTDate='" + Date[2] + "-" + Date[1] + "-" + Date[0] + "'", dt = new DataTable());
                    }
                    if (dt.Rows.Count > 0)
                    {
                        Session["__DailyAttSummary__"] = dt;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=DailyAttSummary-"+dptDate.Text+"');", true);  //Open New Tab for Sever side code
                    }
                    else
                    {
                        lblMessage.InnerText = "warning-> Attendance Not available";
                    }
                }
            }
            catch { }
        }

        protected void ddlShiftName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlShiftName.SelectedValue == "0000"&&ddlCompanyName.SelectedValue!="0000")
            {
                classes.commonTask.LoadDepartmentByCompanyInListBox(ddlCompanyName.SelectedValue,lstAll);
            }
        }
    }
}