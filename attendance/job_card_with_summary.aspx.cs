using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//using BG.Common;

using System.Data;
using adviitRuntimeScripting;
using System.Data.SqlClient;
using ComplexScriptingSystem;

namespace SigmaERP.attendance
{
    public partial class job_card_with_summary : System.Web.UI.Page
    {
        string CompanyId = "";
        DataTable dt;
        DataTable dtSetPrivilege;
        string empId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sqlDB.connectionString = Glory.getConnectionString();
                sqlDB.connectDB();


                this.Load += new System.EventHandler(this.Page_Load);

                if (!IsPostBack)
                {
                    //classes.commonTask.LoadEmpTypeWithAll(rblEmpType);
                    setPrivilege();
                    if (!classes.commonTask.HasBranch())
                        ddlCompany.Enabled = false;
                    ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
                    loadMonthId();
                }
            }
            catch { }
        }

        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                //------------load privilege setting inof from db------

                //-----------------------------------------------------

                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
                {
                    trCompanyName.Visible = true;
                    classes.commonTask.LoadBranch(ddlCompany);
                    //ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
                    // classes.commonTask.LoadShift(ddlShiftList, ViewState["__CompanyId__"].ToString());
                    classes.commonTask.LoadDepartmentByCompanyInListBox(ViewState["__CompanyId__"].ToString(), lstEmployees);

                }
                else
                {
                    classes.commonTask.LoadBranch(ddlCompany, ViewState["__CompanyId__"].ToString());
                    classes.commonTask.LoadDepartmentByCompanyInListBox(ViewState["__CompanyId__"].ToString(), lstEmployees);
                    //classes.commonTask.LoadShift(ddlShiftList, ViewState["__CompanyId__"].ToString());
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='daily_movement.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege = new DataTable());
                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                        {
                            btnPreview.CssClass = "Mbutton"; btnPreview.Enabled = true;
                        }
                        else
                        {
                            tblGenerateType.Visible = false;
                            WarningMessage.Visible = true;
                            btnPreview.CssClass = ""; btnPreview.Enabled = false;
                            // mainDiv.Style.Add("Pointer-event", "none");
                        }

                    }
                    else
                    {
                        tblGenerateType.Visible = false;
                        WarningMessage.Visible = true;
                        btnPreview.CssClass = ""; btnPreview.Enabled = false;
                        // mainDiv.Style.Add("Pointer-event", "none");
                    }
                    //if (dt.Rows.Count > 0)
                    //{
                    //    if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                    //    {
                    //        btnPreview.CssClass = "";
                    //        btnPreview.Enabled = false;
                    //    }
                    //}
                }
                
            }
            catch { }
        }

        private void loadMonthId()
        {
            try
            {
                sqlDB.bindDropDownList("Select distinct MonthName From v_tblAttendanceRecord order by MonthName DESC", "MonthName", ddlMonthID);
                ddlMonthID.Items.Add(" ");
            }
            catch { }
        }      
       
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveItem(lstEmployees, lstSelectedEmployees);
        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveAll(lstEmployees, lstSelectedEmployees);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveItem(lstSelectedEmployees, lstEmployees);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveAll(lstSelectedEmployees, lstEmployees);
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
      
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
            //classes.commonTask.LoadShift(ddlShiftList, CompanyId , ViewState["__UserType__"].ToString()); 
            classes.commonTask.LoadDepartmentByCompanyInListBox(CompanyId,lstEmployees);

        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
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

                DataTable dt = new DataTable();
                if (txtCardNo.Text.Length > 0) sqlDB.fillDataTable("Select EmpId,SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,SftName,ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,Convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime From v_tblAttendanceRecord Where EmpCardNo Like'%" + txtCardNo.Text.Trim() + "' and MonthName='" + ddlMonthID.SelectedItem.Text + "'  Group By EmpId,EmpCardNo,EmpName,SftName,ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime  Order By DptId ", dt);
                else sqlDB.fillDataTable("Select EmpId,SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,SftName,ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,Convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime From v_tblAttendanceRecord Where MonthName='" + ddlMonthID.SelectedItem.Text + "' and DptName " + setPredicate + " Group By EmpId,EmpCardNo,EmpName,SftName,ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime  Order By DptId ", dt);
                Session["__dtJobCard__"] = dt;
                if (dt.Rows.Count > 0)
                {
                    DataTable dtSummary=new DataTable();
                    if (txtCardNo.Text.Length > 0) sqlDB.fillDataTable("Select EmpId,SUM(CASE WHEN StateStatus = 'Absent' THEN 1 ELSE 0 END) AS 'Absent',SUM(CASE WHEN StateStatus = 'C/L' THEN 1 ELSE 0 END) AS 'CL',SUM(CASE WHEN StateStatus = 'S/L' THEN 1 ELSE 0 END) AS 'SL',SUM(CASE WHEN StateStatus = 'M/L' THEN 1 ELSE 0 END) AS 'ML',SUM(CASE WHEN StateStatus = 'E/L' THEN 1 ELSE 0 END) AS 'EL',SUM(CASE WHEN StateStatus = 'Holiday' THEN 1 ELSE 0 END) AS 'Holiday',SUM(CASE WHEN StateStatus = 'Present' THEN 1 ELSE 0 END) AS 'Present',SUM(CASE WHEN StateStatus = 'Weekend' THEN 1 ELSE 0 END) AS 'Weekend',SUM(CASE WHEN StayTime! = '00:00:00.0000000'  AND AttStatus Not in('W','H') THEN case when StayTime>='05:00:00.0000000' and StayTime<'08:00:00.0000000' Then 1/2 else 1 end ELSE 0 END) AS 'APday' From v_tblAttendanceRecord Where EmpCardNo Like'%" + txtCardNo.Text.Trim() + "' and MonthName='" + ddlMonthID.SelectedItem.Text + "' group by EmpId", dtSummary);
                    else sqlDB.fillDataTable("Select EmpId,SUM(CASE WHEN StateStatus = 'Absent' THEN 1 ELSE 0 END) AS 'Absent',SUM(CASE WHEN StateStatus = 'C/L' THEN 1 ELSE 0 END) AS 'CL',SUM(CASE WHEN StateStatus = 'S/L' THEN 1 ELSE 0 END) AS 'SL',SUM(CASE WHEN StateStatus = 'M/L' THEN 1 ELSE 0 END) AS 'ML',SUM(CASE WHEN StateStatus = 'E/L' THEN 1 ELSE 0 END) AS 'EL',SUM(CASE WHEN StateStatus = 'Holiday' THEN 1 ELSE 0 END) AS 'Holiday',SUM(CASE WHEN StateStatus = 'Present' THEN 1 ELSE 0 END) AS 'Present',SUM(CASE WHEN StateStatus = 'Weekend' THEN 1 ELSE 0 END) AS 'Weekend',SUM(CASE WHEN StayTime! = '00:00:00.0000000'  AND AttStatus Not in('W','H') THEN case when StayTime>='05:00:00.0000000' and StayTime<'08:00:00.0000000' Then 1/2 else 1 end ELSE 0 END) AS 'APday' From v_tblAttendanceRecord Where MonthName='" + ddlMonthID.SelectedItem.Text + "' and DptName " + setPredicate + " group by EmpId", dtSummary);
                    Session["__dtSummary__"] = dtSummary;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=JobCardReport');", true);  //Open New Tab for Sever side code         
                }
                else
                {
                    lblMessage.InnerText = "warning->No Attendance Available";
                }
            }
            catch { }
        }
    }
}