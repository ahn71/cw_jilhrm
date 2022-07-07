using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ComplexScriptingSystem;
using SigmaERP.classes;

namespace SigmaERP.attendance
{
    public partial class daily_manualAttendance_report : System.Web.UI.Page
    {
        DataTable dt;
        DataTable dtSetPrivilege;
        string CompanyId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                classes.commonTask.LoadEmpTypeWithAll(rblEmpType);
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompany.Enabled = false;
                ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
                Session["__MinDigits__"] = "4";
                txtFDate.Text = txtTDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
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
                //------------load privilege setting inof from db------
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "daily_movement.aspx", ddlCompany, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                classes.commonTask.LoadShiftNameByCompany(ViewState["__CompanyId__"].ToString(), ddlShift);
                classes.commonTask.LoadDepartment(ViewState["__CompanyId__"].ToString(), lstAll);
                //-----------------------------------------------------





            }
            catch { }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {

            GenerateReport();                  

        }

    
        private void GenerateReport()
        {
            if (txtFDate.Text.Trim().Length < 8) 
            {
                lblMessage.InnerText = "warning-> select From Date !"; txtFDate.Focus(); return;
            }
            if (txtTDate.Text.Trim().Length < 8)
            {
                lblMessage.InnerText = "warning-> select To Date !"; txtTDate.Focus(); return;
            }
            if (lstSelected.Items.Count == 0 && txtCardNo.Text.Trim().Length == 0)
            {
                lblMessage.InnerText = "warning-> Please Select Any Department!";
                lstSelected.Focus();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                return;
            }
            string ShiftName = (ddlShift.SelectedValue == "0") ? "" : " and SftName='" + ddlShift.SelectedValue + "' ";
            string[] Fdmy = txtFDate.Text.Split('-');
            string[] Tdmy = txtTDate.Text.Split('-');
            string F_YMD = Fdmy[2] + "-" + Fdmy[1]+"-"+ Fdmy[0];
            string T_YMD = Tdmy[2] + "-" + Tdmy[1] + "-" + Tdmy[0];
            string AttStatus = (rblAttStatus.SelectedValue == "All") ? "" : " and AttStatus='" + rblAttStatus.SelectedValue + "' ";
            //if (classes.commonTask.IsWeekendORHoliday(y + "-" + m + "-" + d))
            //{
            //    if (rblAttStatus.SelectedValue == "P")

            //        AttStatus = " and InHour<>'00' ";
            //    else if (rblAttStatus.SelectedValue == "A")
            //        AttStatus = " and InHour='00'  ";
            //    else if (rblAttStatus.SelectedValue == "Lv")
            //        AttStatus = " and AttStatus='Lv'  ";
            //    else
            //        AttStatus = "";
            //}

            string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId=" + rblEmpType.SelectedValue + " ";
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue.ToString();


            string CompanyList = "";           
            string DepartmentList = "";

            if (!Page.IsValid)   // If Java script are desible then 
            {
                lblMessage.InnerText = "erroe->Please Select From Date And To Date"; return;
            }
            CompanyList = "in ('" + CompanyId + "')";
            DepartmentList = classes.commonTask.getDepartmentList(lstSelected);



            if (txtCardNo.Text.Trim().Length == 0) sqlDB.fillDataTable("select EmpId,SUBSTRING(EmpCardNo,10,6) as EmpCardNo, EmpName,DptId,DptName,DsgId,DsgName,SftId,SftName,GId,GName,CompanyId,CompanyName,Address, CONVERT(VARCHAR(10), AttDate, 105) as AttDate,PInHour,PInMin,PInSec,POutHour,POutMin,POutSec  ,InHour,InMin,InSec,OutHour,OutMin,OutSec,OutDuty,FirstName,LastName  from v_tblAttendanceRecordPunchLog where AttManual='MC'  and  ATTDate>='" + F_YMD + "' and ATTDate<='" + T_YMD + "' and CompanyId " + CompanyList + "   AND DptId " + DepartmentList + " " + EmpTypeID + " " + AttStatus + " " + ShiftName + "  order by convert(int,DptCode),convert(int,GId), convert(int,SftId),CustomOrdering ", dt = new DataTable());
            else
            {
                if (txtCardNo.Text.Trim().Length < int.Parse(Session["__MinDigits__"].ToString()))
                {
                    lblMessage.InnerText = "warning-> Please Type Valid Card Number!(Minimum " + Session["__MinDigits__"].ToString() + " Digits)";
                    txtCardNo.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    return;
                }
                sqlDB.fillDataTable("select EmpId,SUBSTRING(EmpCardNo,10,6) as EmpCardNo, EmpName,DptId,DptName,DsgId,DsgName,SftId,SftName,GId,GName,CompanyId,CompanyName,Address, CONVERT(VARCHAR(10), AttDate, 105) as AttDate,PInHour,PInMin,PInSec,POutHour,POutMin,POutSec  ,InHour,InMin,InSec,OutHour,OutMin,OutSec,OutDuty,FirstName,LastName  from v_tblAttendanceRecordPunchLog where AttManual='MC'  and  ATTDate>='" + F_YMD + "' and ATTDate<='" + T_YMD + "'  and EmpCardNo Like'%" + txtCardNo.Text.Trim() + "' and CompanyId " + CompanyList + " " + AttStatus + " ", dt = new DataTable());
            }

            if (dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->No Manual Attendance Available";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                return;
            }
            string dateRange = (txtFDate.Text + " to " + txtTDate.Text).Replace('-', '/');
            Session["__ManualAttReprot__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=ManualAttReprot-" + dateRange + "-" + rblPrintType.SelectedValue + "');", true);  //Open New Tab for Sever side code

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

   

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);

            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
            classes.commonTask.LoadShiftNameByCompany(CompanyId, ddlShift);
            classes.commonTask.LoadDepartment(CompanyId, lstAll);
            lstSelected.Items.Clear();
            //classes.commonTask.LoadShift(ddlShiftList, CompanyId , ViewState["__UserType__"].ToString()); 
            // classes.commonTask.LoadShiftByNumber(ddlShiftList, CompanyId, rblShiftNumber.SelectedValue);
            // addAllTextInShift();

        }





    }
}