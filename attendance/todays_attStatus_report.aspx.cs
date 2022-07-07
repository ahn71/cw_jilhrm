using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ComplexScriptingSystem;
namespace SigmaERP.attendance
{
    public partial class todays_attStatus_report : System.Web.UI.Page
    {
        DataTable dt;
        DataTable dtSetPrivilege;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
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
                ViewState["__UserId__"] = getCookies["__getUserId__"].ToString();
             //   ViewState["__DptId__"] = getCookies["__DptId__"].ToString();
                //------------load privilege setting inof from db------

                //-----------------------------------------------------

                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))// Admin  
                {
                    sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='todays_attStatus_report.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege = new DataTable());

                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(false))
                        {
                            WarningMessage.Visible = true;
                            btnStatus.Enabled = false;
                            btnStatus.CssClass = "";
                            btnAbsentStatus.Enabled = false;
                            btnAbsentStatus.CssClass = "";
                            btnPresentStatus.Enabled = false;
                            btnPresentStatus.CssClass = "";
                            return;
                        }
                    }
                    else
                    {
                        WarningMessage.Visible = true;
                        btnStatus.Enabled = false;
                        btnStatus.CssClass = "";
                        btnAbsentStatus.Enabled = false;
                        btnAbsentStatus.CssClass = "";
                        btnPresentStatus.Enabled = false;
                        btnPresentStatus.CssClass = "";
                        return;
                    }
                } 
            }
            catch { }
        }
        protected void btnStatus_Click(object sender, EventArgs e)
        {
            //-------------------- Data processing-----------------------------
           // classes.DailyAttendanceProcessing DAP = new classes.DailyAttendanceProcessing();
           // DAP.GetRequiredInfo(ViewState["__CompanyId__"].ToString(), "1", classes.ServerTimeZone.GetBangladeshNowDate(), ViewState["__UserId__"].ToString());
            //-------------------------------------------------
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                sqlDB.fillDataTable("Select AttStatus,EmpName,DptName,DptCode,DsgName,GId, GName,EmpCardNo,CompanyId,CompanyName,Address,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where CompanyId='" + ViewState["__CompanyId__"].ToString() + "' and DptId='" + ViewState["__DptId__"].ToString() + "' AND  EmpStatus in (1,8)  and ATTDate ='" + classes.ServerTimeZone.GetBangladeshNowDate("yyyy-MM-dd") + "' order by  Gid,CustomOrdering", dt = new DataTable());
           else
                sqlDB.fillDataTable("Select AttStatus,EmpName,DptName,DptCode,DsgName,GId, GName,EmpCardNo,CompanyId,CompanyName,Address,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where CompanyId='" + ViewState["__CompanyId__"].ToString() + "' AND  EmpStatus in (1,8)  and ATTDate ='" + classes.ServerTimeZone.GetBangladeshNowDate("yyyy-MM-dd") + "' order by  Gid,CustomOrdering", dt = new DataTable());
            if (dt == null || dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->Todays attendance status not founded !"; return;
            }
            Session["__TodaysAttStatus__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=TodaysAttStatus-all');", true);  //Open New Tab for Sever side code
           
        }

        protected void btnPresentStatus_Click(object sender, EventArgs e)
        {
            //-------------------- Data processing-----------------------------
           // classes.DailyAttendanceProcessing DAP = new classes.DailyAttendanceProcessing();
           // DAP.GetRequiredInfo(ViewState["__CompanyId__"].ToString(), "1", classes.ServerTimeZone.GetBangladeshNowDate(), ViewState["__UserId__"].ToString());
            //-------------------------------------------------
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                sqlDB.fillDataTable("Select AttStatus,EmpName,DptName,DptCode,DsgName,GId, GName,EmpCardNo,CompanyId,CompanyName,Address,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where CompanyId='" + ViewState["__CompanyId__"].ToString() + "' and DptId='" + ViewState["__DptId__"].ToString() + "' and AttStatus='p' AND  EmpStatus in (1,8) and ATTDate ='" + classes.ServerTimeZone.GetBangladeshNowDate("yyyy-MM-dd") + "' order by  Gid,CustomOrdering", dt = new DataTable());
            else
                sqlDB.fillDataTable("Select AttStatus,EmpName,DptName,DptCode,DsgName,GId, GName,EmpCardNo,CompanyId,CompanyName,Address,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where CompanyId='" + ViewState["__CompanyId__"].ToString() + "' and AttStatus='p' AND  EmpStatus in (1,8) and ATTDate ='" + classes.ServerTimeZone.GetBangladeshNowDate("yyyy-MM-dd") + "' order by  Gid,CustomOrdering", dt = new DataTable());
            if (dt == null || dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->Todays present status not founded !"; return;
            }
            Session["__TodaysAttStatus__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=TodaysAttStatus-p');", true);  //Open New Tab for Sever side code
           
        }

        protected void btnAbsentStatus_Click(object sender, EventArgs e)
        {
            //-------------------- Data processing-----------------------------
          //  classes.DailyAttendanceProcessing DAP = new classes.DailyAttendanceProcessing();
           // DAP.GetRequiredInfo(ViewState["__CompanyId__"].ToString(), "1", classes.ServerTimeZone.GetBangladeshNowDate(), ViewState["__UserId__"].ToString());
            //-------------------------------------------------
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                sqlDB.fillDataTable("Select AttStatus,EmpName,DptName,DptCode,DsgName,GId, GName,EmpCardNo,CompanyId,CompanyName,Address,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where CompanyId='" + ViewState["__CompanyId__"].ToString() + "' and DptId='" + ViewState["__DptId__"].ToString() + "' and AttStatus='a' AND  EmpStatus in (1,8)  and ATTDate ='" + classes.ServerTimeZone.GetBangladeshNowDate("yyyy-MM-dd") + "' order by  Gid,CustomOrdering", dt = new DataTable());
           else
                sqlDB.fillDataTable("Select AttStatus,EmpName,DptName,DptCode,DsgName,GId, GName,EmpCardNo,CompanyId,CompanyName,Address,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where CompanyId='" + ViewState["__CompanyId__"].ToString() + "' and AttStatus='a' AND  EmpStatus in (1,8)  and ATTDate ='" + classes.ServerTimeZone.GetBangladeshNowDate("yyyy-MM-dd") + "' order by  Gid,CustomOrdering", dt = new DataTable());
            if (dt == null || dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->Todays absent status not founded !"; return;
            }
            Session["__TodaysAttStatus__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=TodaysAttStatus-a');", true);  //Open New Tab for Sever side code
           
        }

        protected void btnLateStatus_Click(object sender, EventArgs e)
        {
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                sqlDB.fillDataTable("Select AttStatus,EmpName,DptName,DptCode,DsgName,GId, GName,EmpCardNo,CompanyId,CompanyName,Address,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where CompanyId='" + ViewState["__CompanyId__"].ToString() + "' and DptId='" + ViewState["__DptId__"].ToString() + "' and AttStatus='l' AND  EmpStatus in (1,8)  and ATTDate ='" + classes.ServerTimeZone.GetBangladeshNowDate("yyyy-MM-dd") + "' order by  Gid,CustomOrdering", dt = new DataTable());
            else
                sqlDB.fillDataTable("Select AttStatus,EmpName,DptName,DptCode,DsgName,GId, GName,EmpCardNo,CompanyId,CompanyName,Address,format(ATTDate,'dd-MM-yyyy') as ATTDate From v_tblAttendanceRecord where CompanyId='" + ViewState["__CompanyId__"].ToString() + "' and AttStatus='l' AND  EmpStatus in (1,8)  and ATTDate ='" + classes.ServerTimeZone.GetBangladeshNowDate("yyyy-MM-dd") + "' order by  Gid,CustomOrdering", dt = new DataTable());
            if (dt == null || dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->Todays Late status not founded !"; return;
            }
            Session["__TodaysAttStatus__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=TodaysAttStatus-l');", true);  //Open New Tab for Sever side code
           
        }
    }
}