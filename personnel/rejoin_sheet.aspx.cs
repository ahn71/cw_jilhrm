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
    public partial class rejoin_sheet : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                classes.commonTask.loadDivision(ddlDivision);
                classes.commonTask.LoadMonthName(ddlMonthName);
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
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='rejoin_sheet.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
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

        protected void rdbStaff_CheckedChanged(object sender, EventArgs e)
        {
            rdbWorker.Checked = false;
        }

        protected void rdbWorker_CheckedChanged(object sender, EventArgs e)
        {
            rdbStaff.Checked = false;
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDepartment(ddlDivision.SelectedValue, lstAll);
        }
        private void LoadDepartment(string divisionId, ListBox lst)
        {
            try
            {
                dt = new DataTable();

                sqlDB.fillDataTable("SELECT DptId, DptName FROM HRD_Department where DId=" + divisionId + "", dt);

                lst.DataValueField = "DptId";
                lst.DataTextField = "DptName";
                lst.DataSource = dt;
                lst.DataBind();
            }
            catch { }
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

        protected void btnpreview_Click(object sender, EventArgs e)
        {
            try
            {
                string setPredicate = "", MonthName = "";
                string[] getmonth = ddlMonthName.SelectedItem.Text.Split('-');
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
                string[] getMonthID = ddlMonthName.SelectedItem.Text.Split('-');
                if (rdbStaff.Checked == true)
                {
                    dt = new DataTable();
                    sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmpRejoinSheet where  EmpTypeId=2 and DptId " + setPredicate + " and EffectiveMonth='" + getMonthID[1] + "-" + getMonthID[0] + "' and ActiveSalary='True' and IsActive=1 Group by EmpId", dt);
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
                    sqlDB.fillDataTable("Select  EmpCardNo,EmpName,DsgName,DptName,LnCode, convert(varchar(11),EmpJoiningDate,106)as EmpJoiningDate,GrdName,convert(varchar(11),EffectiveDate,106)as EffectiveDate,EmpPresentSalary From v_EmpRejoinSheet where SN " + setSn + " order by EmpCardNo", dt = new DataTable());

                }
                else
                {
                    dt = new DataTable();
                   
                    string setSn = "", setEmpId = "";


                    sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmpRejoinSheet where  EmpTypeId=1 and DptId " + setPredicate + " and EffectiveMonth='" + getMonthID[1] + "-" + getMonthID[0] + "' and ActiveSalary='True' and IsActive=1 Group by EmpId", dt);
                    setSn = ""; setEmpId = "";
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
                    sqlDB.fillDataTable("Select  EmpCardNo,EmpName,DsgName,DptName,LnCode, convert(varchar(11),EmpJoiningDate,106)as EmpJoiningDate,GrdName,convert(varchar(11),EffectiveDate,106)as EffectiveDate,EmpPresentSalary From v_EmpRejoinSheet where SN " + setSn + " order by EmpCardNo", dt = new DataTable());
                }
                if (dt.Rows.Count > 0)
                {
                    Session["__RejoinSheet__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=RejoinSheet-" + MonthName + "');", true);  //Open New Tab for Sever side code
                }
                else
                    lblMessage.InnerText = "warning->No Data Available";
            }
            catch { }
        }
    }
}