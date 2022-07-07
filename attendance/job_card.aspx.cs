using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using adviitRuntimeScripting;
using System.Data.SqlClient;
using ComplexScriptingSystem;
using SigmaERP.classes;

namespace SigmaERP.attendance
{
    public partial class job_card : System.Web.UI.Page
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
                    classes.commonTask.LoadEmpTypeWithAll(rblEmpType);                   
                    setPrivilege();
                    if (!classes.commonTask.HasBranch())
                        ddlCompany.Enabled = false;
                    ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();                  
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
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "monthly_in_out_report.aspx", ddlCompany, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                 classes.commonTask.loadMonthIdByCompany(ddlMonthID, ViewState["__CompanyId__"].ToString());
                classes.commonTask.LoadDepartment(ViewState["__CompanyId__"].ToString(),lstEmployees);
                //-----------------------------------------------------        
              

            }
            catch { }
        }

        ////private void loadMonthId()
        ////{
        ////    try
        ////    {
        ////        sqlDB.bindDropDownList("Select distinct MonthName From v_tblAttendanceRecord order by MonthName DESC", "MonthName", ddlMonthID);
        ////        ddlMonthID.Items.Add(" ");
        ////    }
        ////    catch { }
        ////}

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
            lstSelectedEmployees.Items.Clear();
            classes.commonTask.LoadDepartmentByCompanyInListBox(CompanyId, lstEmployees);

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
                string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId= " + rblEmpType.SelectedValue + "";

                DataTable dt = new DataTable();
                if (txtCardNo.Text.Length > 0) sqlDB.fillDataTable("Select EmpId,SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,SftName,format(ATTDate,'dd-MM-yyyy') as ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,Convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime,TotalOverTime,TotalDays From v_tblAttendanceRecord Where CompanyId='" + ddlCompany.SelectedValue + "' and EmpCardNo Like'%" + txtCardNo.Text.Trim() + "' and MonthId='" + ddlMonthID.SelectedItem.Text + "'  Group By EmpId,EmpCardNo,EmpName,SftName,ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime,TotalOverTime,TotalDays order by  ATTDate", dt);
                else sqlDB.fillDataTable("Select EmpId,SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,SftName,format(ATTDate,'dd-MM-yyyy') as ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,Convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime,TotalOverTime,TotalDays From v_tblAttendanceRecord Where CompanyId='" + ddlCompany.SelectedValue + "' " + EmpTypeID + " and MonthId='" + ddlMonthID.SelectedItem.Text + "' and DptName " + setPredicate + " Group By EmpId,EmpCardNo,EmpName,SftName,ATTDate,DptName,DsgName,MonthName,InHour,InMin,OutHour,OutMin,ATTStatus,StayTime,OverTime,DptId,StateStatus,EmpJoiningDate,GrdName,EmpType,InSec,OutSec,LateTime,OverTimeCheck,CompanyName,Address,GName,MonthId,BreakStartTime,BreakEndTime,TotalOverTime,TotalDays,GId,CustomOrdering  Order By convert(int,DptId), CustomOrdering,Empid, ATTDate ", dt);
                Session["__dtJobCard__"] = dt;
                if (dt.Rows.Count > 0)
                {
                    DataTable dtSummary = new DataTable();
                    if (txtCardNo.Text.Length > 0) sqlDB.fillDataTable("Select EmpId,SUM(CASE WHEN StateStatus = 'Absent' THEN 1 ELSE 0 END) AS 'Absent',SUM(CASE WHEN StateStatus = 'Casual Leave' THEN 1 ELSE 0 END) AS 'CL',SUM(CASE WHEN StateStatus = 'Sick Leave' THEN 1 ELSE 0 END) AS 'SL',SUM(CASE WHEN StateStatus = 'Maternity Leave' THEN 1 ELSE 0 END) AS 'ML',SUM(CASE WHEN StateStatus = 'Annual Leave' THEN 1 ELSE 0 END) AS 'EL',SUM(CASE WHEN StateStatus = 'Holiday' THEN 1 ELSE 0 END) AS 'Holiday',SUM(CASE WHEN StateStatus = 'Present' THEN 1 ELSE 0 END) AS 'Present',SUM(CASE WHEN StateStatus = 'Weekend' THEN 1 ELSE 0 END) AS 'Weekend',Sum(PaybleDays) AS 'APday' From v_tblAttendanceRecord Where CompanyId='" + ddlCompany.SelectedValue + "' and EmpCardNo Like'%" + txtCardNo.Text.Trim() + "' and MonthId='" + ddlMonthID.SelectedItem.Text + "' group by EmpId", dtSummary);
                    else sqlDB.fillDataTable("Select EmpId,SUM(CASE WHEN StateStatus = 'Absent' THEN 1 ELSE 0 END) AS 'Absent',SUM(CASE WHEN StateStatus = 'Casual Leave' THEN 1 ELSE 0 END) AS 'CL',SUM(CASE WHEN StateStatus = 'Sick Leave' THEN 1 ELSE 0 END) AS 'SL',SUM(CASE WHEN StateStatus = 'Maternity Leave' THEN 1 ELSE 0 END) AS 'ML',SUM(CASE WHEN StateStatus = 'Annual Leave' THEN 1 ELSE 0 END) AS 'EL',SUM(CASE WHEN StateStatus = 'Holiday' THEN 1 ELSE 0 END) AS 'Holiday',SUM(CASE WHEN StateStatus = 'Present' THEN 1 ELSE 0 END) AS 'Present',SUM(CASE WHEN StateStatus = 'Weekend' THEN 1 ELSE 0 END) AS 'Weekend',Sum(PaybleDays) AS 'APday' From v_tblAttendanceRecord Where CompanyId='" + ddlCompany.SelectedValue + "' " + EmpTypeID + " and MonthId='" + ddlMonthID.SelectedItem.Text + "' and DptName " + setPredicate + " group by EmpId", dtSummary);
                    Session["__dtSummary__"] = dtSummary;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=JobCardReportActual');", true);  //Open New Tab for Sever side code         
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