using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.All_Report.Attendance
{
    public partial class early_out_late_out : System.Web.UI.Page
    {
        string CompanyId="";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
           
            if (!IsPostBack)
            {
                classes.commonTask.LoadEmpTypeWithAll(rblEmpType);
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
            }

        }

        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];

                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();

                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Viewer"))
                {

                    classes.commonTask.LoadBranch(ddlCompanyName);
                    classes.commonTask.LoadShiftByNumber(ddlShiftName, ViewState["__CompanyId__"].ToString(), rblShiftNumber.SelectedValue);
                   
                }
                else
                {
                   
                    dtSetPrivilege = new DataTable();
                  
                    //classes.commonTask.LoadBranch(ddlCompanyName, ViewState["__CompanyId__"].ToString());
                    //classes.commonTask.LoadShift(ddlShiftName, ViewState["__CompanyId__"].ToString());

                    //if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                    //{
                    //    btnPreview.CssClass = ""; btnPreview.Enabled = false;
                    //} 
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='early_out_late_out.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);

                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                        {
                            btnPreview.CssClass = "css_btn"; btnPreview.Enabled = true;
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

                }
         //  addAllTextInShift();

            }
            catch { }

        }

        private void addAllTextInShift()
        {
            if (ddlShiftName.Items.Count > 2)
                ddlShiftName.Items.Insert(1, new ListItem("All", "00"));
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveItem(lstAll, lstSelected);
        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveAll(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveAll(lstSelected, lstAll);
        }

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
            if (ddlShiftName.SelectedItem.ToString().Equals("All"))
            {

                string ShiftList = classes.commonTask.getShiftList(ddlShiftName);
                classes.commonTask.LoadDepartmentByCompanyAndShiftInListBox(CompanyId, ShiftList, lstAll);
                return;
            }
            classes.commonTask.LoadDepartmentByCompanyAndShiftInListBox(CompanyId, "in (" + ddlShiftName.SelectedValue.ToString() + ")", lstAll);         
           addAllTextInShift();
        }

        protected void ddlShiftName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                lstAll.Items.Clear();
                lstSelected.Items.Clear();
                CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();

                if (ddlShiftName.SelectedItem.ToString().Equals("All"))
                {
                    string ShiftList = classes.commonTask.getShiftList(ddlShiftName);
                    classes.commonTask.LoadDepartmentByCompanyAndShiftInListBox(CompanyId, ShiftList, lstAll);
                    return;
                }
                classes.commonTask.LoadDepartmentByCompanyAndShiftInListBox(CompanyId, "in (" + ddlShiftName.SelectedValue.ToString() + ")", lstAll);
            }
            catch { }
        }
        protected void rblShiftNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            CompanyId = (ddlCompanyName.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue;
            classes.commonTask.LoadShiftByNumber(ddlShiftName, CompanyId, rblShiftNumber.SelectedValue);
            //addAllTextInShift();
        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //------------------------Validation--------------------------------
                    if (txtCardNo.Text.Trim().Length == 0)
                    {
                        if (ddlShiftName.SelectedValue == "0")
                        {
                            lblMessage.InnerText = "warning-> Please Select Any Shift!"; ddlShiftName.Focus();
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                            return;
                        }                     
                        if (lstSelected.Items.Count == 0)
                        {
                            lblMessage.InnerText = "warning-> Please Select Any Department!"; lstSelected.Focus();
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                            return;
                        }
                    }
                    else
                    {
                        if (txtCardNo.Text.Trim().Length < 4)
                        {
                            lblMessage.InnerText = "warning-> Please Enter Minimum 4 Character of Card No.";
                            txtCardNo.Focus();
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                            return;
                        }
                    }
                //----------------------------End-------------------------------

                string CompanyList = "";
                string ShiftList = "";
                string DepartmentList = "";
                string ReportTitle = "";
                string ReportDate = "";
                string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId=" + rblEmpType.SelectedValue + " ";
                if (!Page.IsValid)   // If Java script are desible then 
                {
                    lblMessage.InnerText = "erroe->Please Select From Date And To Date";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    return;
                }
                CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();

                CompanyList = "in ('" + CompanyId + "')";

                if (ddlShiftName.SelectedItem.ToString().Equals("All"))
                {

                    ShiftList = classes.commonTask.getShiftList(ddlShiftName);
                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                }
                else
                {
                    ShiftList = "in ('" + ddlShiftName.SelectedValue.ToString() + "')";
                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                }

                string [] FDMY = txtFromDate.Text.Trim().Split('-');
                FDMY[0] = FDMY[2] + "-" + FDMY[1] + "-" + FDMY[0];

                string [] TDMY = txtToDate.Text.Trim().Split('-');
                TDMY[0] = TDMY[2] + "-" + TDMY[1] + "-" + TDMY[0];

                DataTable dt = new DataTable();

                sqlDB.fillDataTable("select SftEndTime from HRD_Shift where SftId="+ddlShiftName.SelectedItem.Value.ToString()+"",dt);


                if (rblReportType.SelectedItem.Value.ToString().Equals("0"))  // when early out is selected then this part are activated.0=Early Out
                {
                    if (FDMY[0] == TDMY[0])
                    {
                        if (txtCardNo.Text.Trim().Length == 0) // for all employees
                            sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,DsgName,OutHour,OutMin,OutSec,CompanyName,DptName,SftName,Address,CompanyId,DptId,SftId,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where  ATTDate='" + FDMY[0] + "' AND CONVERT(time,OutTime) <'" + dt.Rows[0]["SftEndTime"].ToString() + "' AND ActiveSalary='True' and IsActive=1 and CompanyId " + CompanyList + " AND SftId " + ShiftList + " AND DptId " + DepartmentList + " AND StateStatus='Present' " + EmpTypeID + " order by DptCode,CustomOrdering ", dt = new DataTable());
                        else // for single employee
                            sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,DsgName,OutHour,OutMin,OutSec,CompanyName,DptName,SftName,Address,CompanyId,DptId,SftId,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' AND  ATTDate='" + FDMY[0] + "' AND CONVERT(time,OutTime) <'" + dt.Rows[0]["SftEndTime"].ToString() + "' AND ActiveSalary='True' and IsActive=1 and CompanyId " + CompanyList + " AND SftId " + ShiftList + " AND DptId " + DepartmentList + " AND StateStatus='Present' ", dt = new DataTable());
                        ReportDate = "On "+txtFromDate.Text;
                    }
                    else
                    {
                        if (txtCardNo.Text.Trim().Length == 0) // for all employees
                            sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,DsgName,OutHour,OutMin,OutSec,CompanyName,DptName,SftName,Address,CompanyId,DptId,SftId,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where ATTDate >='" + FDMY[0] + "' AND AttDate<='" + TDMY[0] + "' AND CONVERT(time,OutTime) <'" + dt.Rows[0]["SftEndTime"].ToString() + "' AND ActiveSalary='True' and IsActive=1 and CompanyId " + CompanyList + " AND SftId " + ShiftList + " AND DptId " + DepartmentList + " AND StateStatus='Present' " + EmpTypeID + " order by DptCode,CustomOrdering  ", dt = new DataTable());
                         else   // for single employee
                            sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,DsgName,OutHour,OutMin,OutSec,CompanyName,DptName,SftName,Address,CompanyId,DptId,SftId,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' AND ATTDate >='" + FDMY[0] + "' AND AttDate<='" + TDMY[0] + "' AND CONVERT(time,OutTime) <'" + dt.Rows[0]["SftEndTime"].ToString() + "' AND ActiveSalary='True' and IsActive=1 and CompanyId " + CompanyList + " AND SftId " + ShiftList + " AND DptId " + DepartmentList + " AND StateStatus='Present'", dt = new DataTable());
                        ReportDate = "On " + txtFromDate.Text+" To "+txtToDate.Text;
                    }

                    ReportTitle = "Early Out Report";
                }

                else  // when late out is selected then this part are activated
                {
                    DataTable dtOT = new DataTable();
                    sqlDB.fillDataTable("select AcceptableMinuteasOT from HRD_OthersSetting", dtOT = new DataTable());

                    string ShiftEndTime = (TimeSpan.Parse(dt.Rows[0]["SftEndTime"].ToString()) + TimeSpan.Parse("00:" + dtOT.Rows[0]["AcceptableMinuteasOT"].ToString())).ToString();
                
                    if (FDMY[0] == TDMY[0])
                    {
                        if (txtCardNo.Text.Trim().Length == 0) // for all employees
                            sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,DsgName,OutHour,OutMin,OutSec,CompanyName,DptName,SftName,Address,CompanyId,DptId,SftId,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where ATTDate='" + FDMY[0] + "' AND CONVERT(time,OutTime) >='" + ShiftEndTime + "' AND ActiveSalary='True' and IsActive=1 and CompanyId " + CompanyList + " AND SftId " + ShiftList + " AND DptId " + DepartmentList + " AND StateStatus='Present' " + EmpTypeID + " order by DptCode,CustomOrdering  ", dt = new DataTable());
                        else  // for single employee
                            sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,DsgName,OutHour,OutMin,OutSec,CompanyName,DptName,SftName,Address,CompanyId,DptId,SftId,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' AND ATTDate='" + FDMY[0] + "' AND CONVERT(time,OutTime) >='" + ShiftEndTime + "' AND ActiveSalary='True' and IsActive=1 and CompanyId " + CompanyList + " AND SftId " + ShiftList + " AND DptId " + DepartmentList + " AND StateStatus='Present'  ", dt = new DataTable());
                        ReportDate = "On " + txtFromDate.Text;
                    }
                    else
                    {
                        if (txtCardNo.Text.Trim().Length == 0) // for all employees
                            sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,DsgName,OutHour,OutMin,OutSec,CompanyName,DptName,SftName,Address,CompanyId,DptId,SftId,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where ATTDate >='" + FDMY[0] + "' AND AttDate<='" + TDMY[0] + "' AND CONVERT(time,OutTime) >='" + ShiftEndTime + "' AND ActiveSalary='True' and IsActive=1 and CompanyId " + CompanyList + " AND SftId " + ShiftList + " AND DptId " + DepartmentList + " AND StateStatus='Present' " + EmpTypeID + " order by DptCode,CustomOrdering ", dt = new DataTable());
                        else  // for single employee
                            sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,DsgName,OutHour,OutMin,OutSec,CompanyName,DptName,SftName,Address,CompanyId,DptId,SftId,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' AND ATTDate >='" + FDMY[0] + "' AND AttDate<='" + TDMY[0] + "' AND CONVERT(time,OutTime) >='" + ShiftEndTime + "' AND ActiveSalary='True' and IsActive=1 and CompanyId " + CompanyList + " AND SftId " + ShiftList + " AND DptId " + DepartmentList + " AND StateStatus='Present'  ", dt = new DataTable());
                        ReportDate = "On " + txtFromDate.Text + " To " + txtToDate.Text;
                    }

                    ReportTitle = "Late Out Report";
                }

               


                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->No Attendance Available";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    return;
                }
                Session["__DailyMovement__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EarlyOutLateOut-" +ReportDate+ "-"+ReportTitle+"');", true);  //Open New Tab for Sever side code
        
            }
            catch { }
        }

       
    }
}