using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using adviitRuntimeScripting;

namespace SigmaERP.attendance
{
    public partial class late_report : System.Web.UI.Page
    {
      
       
        string strParam = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sqlDB.connectionString = Glory.getConnectionString();
                sqlDB.connectDB();

                this.btnaddall.Click += new System.EventHandler(this.btnaddall_Click);
                this.btnadditem.Click += new System.EventHandler(this.btnadditem_Click);
                this.btnremoveitem.Click += new System.EventHandler(this.btnremoveitem_Click);
                this.btnremoveall.Click += new System.EventHandler(this.btnremoveall_Click);

                this.Load += new System.EventHandler(this.Page_Load);

                if (!IsPostBack)
                {
                    setPrivilege();
                    loadDepartment();
                }
            }
            catch { }
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
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='late_report.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                        {
                            btnPreviewLate.CssClass = " ";
                            btnPreviewLate.Enabled = false;
                        }
                    }
                }
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

        protected void rdoDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdoDept.SelectedIndex == 0)
                {
                    loadDepartment();
                    ddlDivision.Items.Clear();
                }
                else if (rdoDept.SelectedIndex == 1)
                {
                    loadDevision();
                    lstEmployees.Items.Clear();
                    lstSelectedEmployees.Items.Clear();
                }
            }
            catch { }
        }

        private void loadDepartment() //Load Department for select List All
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select distinct DptName From v_tblAttendanceRecord", dt);
                lstEmployees.DataTextField = "DptName";
                lstEmployees.DataSource = dt;
                lstEmployees.DataBind();
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

        protected void btnPreviewLate_Click(object sender, EventArgs e)
        {
            try
            {
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
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select MAX(SN) as SN, EmpId From v_tblAttendanceRecord Where ATTStatus='L' and ATTDate='" + dptDate.Text.Trim() + "'  and ActiveSalary='True' and IsActive=1  and DptName " + setPredicate + " Group By EmpId ", dt);
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
                sqlDB.fillDataTable("Select  EmpCardNo, EmpName,DsgName,LnCode,ATTStatus,DptName,ATTDate,InHour,InMin,OutHour,OutMin,InSec,OutSec From v_tblAttendanceRecord Where ATTStatus='L' and ATTDate='" + dptDate.Text.Trim() + "'  and ActiveSalary='True' and DptName " + setPredicate + " and SN "+setSn+" Order By LnCode ", dt);
                Session["__dtLetReport__"] = dt;
                if(dt.Rows.Count>0) ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTab('/All Report/Report.aspx?for=DailyLateReportByLine');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }

    }
}