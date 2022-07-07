using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.leave
{
    public partial class yearly_leaveStatus_report : System.Web.UI.Page
    {
        DataTable dt;
        string companyId = "";
        string sqlCmd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
               
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
            }

        }
        private void loadYear()
        {
            try
            {
                DataTable dtYear = new DataTable();
                companyId = (ddlCompanyName.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                sqlDB.fillDataTable(" select distinct Year  from v_v_v_Leave_Yearly_Status where CompanyId='" + companyId + "' order by Year desc", dtYear);
                ddlYear.DataTextField = "Year";
                ddlYear.DataValueField = "Year";
                ddlYear.DataSource = dtYear;
                ddlYear.DataBind();
            }
            catch { }
        }
        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];

                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserId__"] = getUserId;
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__EmpId__"] = getCookies["__getEmpId__"].ToString();
                ViewState["__ODOnlyDpt__"] = getCookies["__getODOnlyDpt__"].ToString();
                ViewState["__DptId__"] = getCookies["__getDptId__"].ToString();
                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "yearly_leaveStatus_report.aspx", ddlCompanyName, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                classes.Payroll.loadMonthIdByCompany(ddlYear, ViewState["__CompanyId__"].ToString());
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "User")
                {                   
                                       
                    ddlShiftName.Enabled = false;
                    workerlist.Visible = false;
                    txtCardNo.Text = commonTask.returnEmpCardNoByEmpId(ViewState["__EmpId__"].ToString());
                    txtCardNo.Enabled = false;
                }
                else 
                {
                    //if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin" && ViewState["__ODOnlyDpt__"].ToString().Equals("True"))
                    //    classes.commonTask.LoadDepartment(ViewState["__CompanyId__"].ToString(), ViewState["__DptId__"].ToString(), lstAll);

                    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin")
                        classes.commonTask.LoadDepartmentListByAdminForLeave(lstAll,ViewState["__CompanyId__"].ToString(),ViewState["__UserId__"].ToString());
                    else
                        classes.commonTask.LoadDepartment(ViewState["__CompanyId__"].ToString(), lstAll);
                                   
                }
                

            }
            catch { }

        }
        private void GenerateYearlyLeaveStarus() 
        {
            try
            {
                string CompanyList = "";
                string ShiftList = "";
                string DepartmentList = "";                      

                if (chkForAllCompany.Checked)
                {
                    CompanyList = classes.commonTask.getCompaniesList(ddlCompanyName);
                    ShiftList = classes.commonTask.getShiftList(ddlShiftName);
                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                }
                else
                {
                    string Cid = (ddlCompanyName.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                    CompanyList = "in ('" + Cid + "')";
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
                }
                string sqlCmd = "";
                string IsIndividual = "";
                if (txtCardNo.Text.Trim().Length > 0)
                {
                    if(txtCardNo.Text.Trim().Length<4)
                    {
                        lblMessage.InnerText = "warning-> Please Type Mininmum 4 Character of Card Number!";
                        return;
                    }
                    IsIndividual = "Yes";
                    sqlCmd = "SELECT  v_v_v_Leave_Yearly_Status.Year, v_v_v_Leave_Yearly_Status.EmpName,v_v_v_Leave_Yearly_Status.EmpCardNo,Sex, v_v_v_Leave_Yearly_Status.DptName," +
                   "v_v_v_Leave_Yearly_Status.SftName,v_v_v_Leave_Yearly_Status.CompanyName,v_v_v_Leave_Yearly_Status.Address,v_v_v_Leave_Yearly_Status.DsgName," +                
                   "v_v_v_Leave_Yearly_Status.CL_Spend, CASE  WHEN v_v_v_Leave_Yearly_Status.CL_Remaining IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.CL_Remaining END As CL_Remaining,CASE  WHEN v_v_v_Leave_Yearly_Status.CL_Total IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.CL_Total END AS CL_Total," +
                   "v_v_v_Leave_Yearly_Status.SL_Spend, CASE  WHEN v_v_v_Leave_Yearly_Status.SL_Remaining IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.SL_Remaining END As SL_Remaining,CASE  WHEN v_v_v_Leave_Yearly_Status.SL_Total IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.SL_Total END AS SL_Total," +
                    "v_v_v_Leave_Yearly_Status.AL_Spend, CASE  WHEN v_v_v_Leave_Yearly_Status.AL_Remaining IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.AL_Remaining END As AL_Remaining,CASE  WHEN v_v_v_Leave_Yearly_Status.AL_Total IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.AL_Total END AS AL_Total," +
                   "v_v_v_Leave_Yearly_Status.ML_Spend, CASE  WHEN v_v_v_Leave_Yearly_Status.ML_Remaining IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.ML_Remaining END As ML_Remaining,CASE  WHEN v_v_v_Leave_Yearly_Status.ML_Total IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.ML_Total END AS ML_Total," +
                   "v_v_v_Leave_Yearly_Status.OPL_Spend, CASE  WHEN v_v_v_Leave_Yearly_Status.OPL_Remaining IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.OPL_Remaining END As OPL_Remaining,CASE  WHEN v_v_v_Leave_Yearly_Status.OPL_Total IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.OPL_Total END AS OPL_Total," +
                   "v_v_v_Leave_Yearly_Status.OL_Spend, CASE  WHEN v_v_v_Leave_Yearly_Status.OL_Remaining IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.OL_Remaining END As OL_Remaining,CASE  WHEN v_v_v_Leave_Yearly_Status.OL_Total IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.OL_Total END AS OL_Total" +                   
                   " FROM  dbo.v_v_v_Leave_Yearly_Status" +
                   " where Year ='" + ddlYear.SelectedValue + "'  AND CompanyId " + CompanyList + " and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like '%" + txtCardNo.Text.Trim() + "'" +
                   "ORDER BY v_v_v_Leave_Yearly_Status.CompanyName,v_v_v_Leave_Yearly_Status.SftName,v_v_v_Leave_Yearly_Status.DptName";
                }
                else
                {
                    IsIndividual = "No";
                    sqlCmd = "SELECT  v_v_v_Leave_Yearly_Status.Year, v_v_v_Leave_Yearly_Status.EmpName,substring(v_v_v_Leave_Yearly_Status.EmpCardNo,8,15) as EmpCardNo, v_v_v_Leave_Yearly_Status.DptName," +
                   "v_v_v_Leave_Yearly_Status.SftName,v_v_v_Leave_Yearly_Status.CompanyName,v_v_v_Leave_Yearly_Status.Address,v_v_v_Leave_Yearly_Status.DsgName," +
                   "v_v_v_Leave_Yearly_Status.CL_Spend, CASE  WHEN v_v_v_Leave_Yearly_Status.CL_Remaining IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.CL_Remaining END As CL_Remaining,CASE  WHEN v_v_v_Leave_Yearly_Status.CL_Total IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.CL_Total END AS CL_Total," +
                   "v_v_v_Leave_Yearly_Status.SL_Spend, CASE  WHEN v_v_v_Leave_Yearly_Status.SL_Remaining IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.SL_Remaining END As SL_Remaining,CASE  WHEN v_v_v_Leave_Yearly_Status.SL_Total IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.SL_Total END AS SL_Total," +
                    "v_v_v_Leave_Yearly_Status.AL_Spend, CASE  WHEN v_v_v_Leave_Yearly_Status.AL_Remaining IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.AL_Remaining END As AL_Remaining,CASE  WHEN v_v_v_Leave_Yearly_Status.AL_Total IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.AL_Total END AS AL_Total," +
                   "v_v_v_Leave_Yearly_Status.ML_Spend, CASE  WHEN v_v_v_Leave_Yearly_Status.ML_Remaining IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.ML_Remaining END As ML_Remaining,CASE  WHEN v_v_v_Leave_Yearly_Status.ML_Total IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.ML_Total END AS ML_Total," +
                   "v_v_v_Leave_Yearly_Status.OPL_Spend, CASE  WHEN v_v_v_Leave_Yearly_Status.OPL_Remaining IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.OPL_Remaining END As OPL_Remaining,CASE  WHEN v_v_v_Leave_Yearly_Status.OPL_Total IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.OPL_Total END AS OPL_Total," +
                   "v_v_v_Leave_Yearly_Status.OL_Spend, CASE  WHEN v_v_v_Leave_Yearly_Status.OL_Remaining IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.OL_Remaining END As OL_Remaining,CASE  WHEN v_v_v_Leave_Yearly_Status.OL_Total IS NULL THEN 0 ELSE v_v_v_Leave_Yearly_Status.OL_Total END AS OL_Total" +  
                " FROM  dbo.v_v_v_Leave_Yearly_Status" +
                " where Year ='" + ddlYear.SelectedValue + "'  AND CompanyId " + CompanyList + " AND " +
                " SftId " + ShiftList + " AND DptId " + DepartmentList + "" +
                "ORDER BY v_v_v_Leave_Yearly_Status.CompanyName,v_v_v_Leave_Yearly_Status.SftName,v_v_v_Leave_Yearly_Status.DptName";
 
                }
                sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->No record found."; return;         
                }               
                Session["__YearlyLeaveStatus__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=YearlyLeaveStatus-"+IsIndividual+"');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
        private void GenerateMonthlyLeaveStatus()
        {

            if (ddlYear.SelectedIndex<1)
            { lblMessage.InnerText = "warning-> Please select any month!"; ddlYear.Focus(); return; }
            if (lstSelected.Items.Count < 1 && txtCardNo.Text.Trim().Length == 0)
            { lblMessage.InnerText = "warning-> Please select any department!"; lstSelected.Focus(); return; }
            string[] month = ddlYear.SelectedValue.Split('-');
            
            string yStartDate = month[0] + "-01-01";
            string yEndDate = (DateTime.Parse(ddlYear.SelectedValue).AddDays(-1)).ToString("yyyy-MM-dd") ;
            string mStartDate = month[0] + "-" + month[1] + "-01";
            string mEndDate = month[0] + "-"+month[1]+"-"+ DateTime.DaysInMonth(int.Parse(month[0]),int.Parse( month[1]));


            if (txtCardNo.Text.Trim() == "")
            {
                string DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                //sqlCmd = @"with
                //        eLv as(
                //        select sum( case when ShortName='c/l' then LeaveDays else 0 end) as eCL,sum( case when ShortName='s/l' then LeaveDays else 0 end) as eSL,
                //        sum( case when ShortName='a/l' then LeaveDays else 0 end) as eEL from tblLeaveConfig where CompanyId='" + ddlCompanyName.SelectedValue + "'" +
                //        ")," +
                //        "aYTD as (" +
                //        "select EmpId, sum(case when  ShortName='c/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end) as aYTDCL ,sum( case when  ShortName='s/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end ) as aYTDSL,sum( case when  ShortName='a/l' then 1 else 0 end )  as aYTDEL from v_Leave_LeaveApplicationDetails " +
                //        "where CompanyId='" + ddlCompanyName.SelectedValue + "' and IsApproved=1 and DptId " + DepartmentList + " and LeaveDate>='" + yStartDate + "' and LeaveDate<='" + yEndDate + "' " +
                //        "group by EmpId" +
                //        ")," +
                //        "aMTD as (" +
                //        "select EmpId, sum(case when  ShortName='c/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end ) as aTDCL ,sum( case when  ShortName='s/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end ) as aTDSL,sum( case when  ShortName='a/l' then 1 else 0 end )  as aTDEL from v_Leave_LeaveApplicationDetails " +
                //        "where CompanyId='" + ddlCompanyName.SelectedValue + "' and IsApproved=1 and DptId " + DepartmentList + "  and LeaveDate>='" + mStartDate + "' and LeaveDate<='" + mEndDate + "' " +
                //        "group by EmpId" +
                //        "), " +
                //        "dYCL as (select EmpID, Sum(Days) as dYCL from Leave_MonthlyLeaveDeductionRecord where Month >= '" + yStartDate + "' and Month<= '" + yEndDate + "' group by EmpID" +
                //        ")," +
                //        "dMCL as (select EmpID, Sum(Days) as dMCL from Leave_MonthlyLeaveDeductionRecord where Month >= '" + mStartDate + "' and Month<= '" + mEndDate + "' group by EmpID) " +
                //        "select mss.EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,mss.CompanyId,mss.CompanyName,mss.Address,LateDays,LateFineDays as absentDueToLate,eLv.eCL,eLv.eSL,elv.eEL," +
                //        " isnull(aYTD.aYTDCL, 0) as aYTDCL,isnull(aYTD.aYTDEL, 0) as aYTDEL,isnull(aYTD.aYTDSL, 0) as aYTDSL,ISNULL(aMTD.aTDCL, 0) as aTDCL," +
                //        "ISNULL(aMTD.aTDEL, 0) as aTDEL ,ISNULL(aMTD.aTDSL, 0) as aTDSL,ISNULL(dYCL.dYCL, 0) as dYCL, ISNULL(dMCL.dMCL, 0) as dMCL,AbsentDay, DaysInMonth,PayableDays " +
                //        "from v_MonthlySalarySheet mss left join aYTD on mss.EmpId = aYTD.EmpId left join aMTD on mss.EmpId = aMTD.EmpId left join dYCL on mss.EmpId = dYCL.EmpID left  join dMCL on mss.EmpId = dMCL.EmpId cross join eLv " +
                //        "where  YearMonth='" + ddlYear.SelectedValue + "' and DptId " + DepartmentList + " ";
                string AdminCondition = (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin")) ? "  and (mss.EmpId='" + ViewState["__EmpId__"].ToString() + "' or mss.EmpId in(select EmpId from  tblLeaveAuthorityAccessControl where AuthorityID=" + ViewState["__UserId__"].ToString() + ")) " : "";
                sqlCmd = @" with eLv as(
                       select sum( case when ShortName='c/l' then LeaveDays else 0 end) as eCL,sum( case when ShortName='s/l' then LeaveDays else 0 end) as eSL,
                        sum( case when ShortName='a/l' then LeaveDays else 0 end) as eEL from tblLeaveConfig where CompanyId='" + ddlCompanyName.SelectedValue + "'),aYTD as (select EmpId, sum(case when  ShortName='c/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end) as aYTDCL ,sum( case when  ShortName='s/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end ) as aYTDSL,sum( case when  ShortName='a/l' then 1 else 0 end )  as aYTDEL from v_Leave_LeaveApplicationDetails where CompanyId='" + ddlCompanyName.SelectedValue + "' and IsApproved=1 and DptId " + DepartmentList + " and LeaveDate>='" + yStartDate + "' and LeaveDate<='" + yEndDate + "' group by EmpId),aMTD as (select EmpId, sum(case when  ShortName='c/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end ) as aTDCL ,sum( case when  ShortName='s/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end ) as aTDSL,sum( case when  ShortName='a/l' then 1 else 0 end )  as aTDEL from v_Leave_LeaveApplicationDetails where CompanyId='" + ddlCompanyName.SelectedValue + "' and IsApproved=1   and DptId " + DepartmentList + "  and LeaveDate>='" + mStartDate + "' and LeaveDate<='" + mEndDate + "' group by EmpId), dYCL as (select EmpID, Sum(Days) as dYCL from Leave_MonthlyLeaveDeductionRecord where Month >= '" + yStartDate + "' and Month<= '" + yEndDate + "' group by EmpID),dMCL as (select EmpID, Sum(Days) as dMCL from Leave_MonthlyLeaveDeductionRecord where Month >= '"+mStartDate+"' and Month<= '" + mEndDate + "' group by EmpID) select mss.EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,mss.CompanyId,mss.CompanyName,mss.Address,LateDays,LateFineDays as absentDueToLate,case when year(mss.EmpJoiningDate)= '"+month[0]+"' then nEb.CL else   eLv.eCL end as eCL,case when year(mss.EmpJoiningDate)= '"+month[0]+"' then nEb.SL else eLv.eSL end as eSL,case when isnull(ela.IsActive,0)=1 and ela.ActiveFrom<= '"+month[0]+ "-01-01' then elv.eEL+ISNULL(ecf.ELDays,0) else 0 end as eEL, isnull(aYTD.aYTDCL, 0) as aYTDCL,isnull(aYTD.aYTDEL, 0) as aYTDEL,isnull(aYTD.aYTDSL, 0) as aYTDSL,ISNULL(aMTD.aTDCL, 0) as aTDCL,ISNULL(aMTD.aTDEL, 0) as aTDEL ,ISNULL(aMTD.aTDSL, 0) as aTDSL,ISNULL(dYCL.dYCL, 0) as dYCL, ISNULL(dMCL.dMCL, 0) as dMCL,AbsentDay, DaysInMonth,PayableDays from v_MonthlySalarySheet mss left join aYTD on mss.EmpId = aYTD.EmpId left join aMTD on mss.EmpId = aMTD.EmpId left join dYCL on mss.EmpId = dYCL.EmpID left  join dMCL on mss.EmpId = dMCL.EmpId left join Leave_EarnLeaveCarriedForward ecf on mss.EmpId=ecf.EmpId  and ecf.Year='" + month[0] + @"-01-01' cross join eLv left join Leave_LeaveBalanceForNewEmp nEb on month(mss.EmpJoiningDate)=nEb.MonthID left join EarnLeave_Activationlog ela on mss.EmpId=ela.EmpID  where  YearMonth='" + ddlYear.SelectedValue + "' and DptId " + DepartmentList + " "+AdminCondition;
            }
            else
            {
                string AdminCondition = (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin")) ? "  and (EmpId='" + ViewState["__EmpId__"].ToString() + "' or EmpId in(select EmpId from  tblLeaveAuthorityAccessControl where AuthorityID=" + ViewState["__UserId__"].ToString() + ")) " : "";
                string EmpId = "";
                sqlCmd = @"select EmpId from Personnel_EmployeeInfo  where   Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like '%" + txtCardNo.Text.Trim() + "' "+ AdminCondition;
                sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
                if (dt==null || dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->Invalid Card No !"; return;
                }
                EmpId = dt.Rows[0]["EmpId"].ToString();
                //sqlCmd = @"with
                //        eLv as(
                //        select sum( case when ShortName='c/l' then LeaveDays else 0 end) as eCL,sum( case when ShortName='s/l' then LeaveDays else 0 end) as eSL,
                //        sum( case when ShortName='a/l' then LeaveDays else 0 end) as eEL from tblLeaveConfig where CompanyId='" + ddlCompanyName.SelectedValue + "'" +
                //        ")," +
                //        "aYTD as (" +
                //        "select EmpId, sum(case when  ShortName='c/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end) as aYTDCL ,sum( case when  ShortName='s/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end ) as aYTDSL,sum( case when  ShortName='a/l' then 1 else 0 end )  as aYTDEL from v_Leave_LeaveApplicationDetails " +
                //        "where CompanyId='" + ddlCompanyName.SelectedValue + "' and IsApproved=1 and Empid='" + EmpId + "' and LeaveDate>='" + yStartDate + "' and LeaveDate<='" + yEndDate + "' " +
                //        "group by EmpId" +
                //        ")," +
                //        "aMTD as (" +
                //        "select EmpId, sum(case when  ShortName='c/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end ) as aTDCL ,sum( case when  ShortName='s/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end ) as aTDSL,sum( case when  ShortName='a/l' then 1 else 0 end )  as aTDEL from v_Leave_LeaveApplicationDetails " +
                //        "where CompanyId='" + ddlCompanyName.SelectedValue + "' and IsApproved=1 and Empid='" + EmpId + "'  and LeaveDate>='" + mStartDate + "' and LeaveDate<='" + mEndDate + "' " +
                //        "group by EmpId" +
                //        "), " +
                //        "dYCL as (select EmpID, Sum(Days) as dYCL from Leave_MonthlyLeaveDeductionRecord where Month >= '" + yStartDate + "' and Month<= '" + yEndDate + "' and Empid='"+EmpId+"' group by EmpID" +
                //        ")," +
                //        "dMCL as (select EmpID, Sum(Days) as dMCL from Leave_MonthlyLeaveDeductionRecord where Month >= '" + mStartDate + "' and Month<= '" + mEndDate + "' and Empid='" + EmpId+"' group by EmpID) " +
                //        "select mss.EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,mss.CompanyId,mss.CompanyName,mss.Address,LateDays,LateFineDays as absentDueToLate,eLv.eCL,eLv.eSL,elv.eEL," +
                //        " isnull(aYTD.aYTDCL, 0) as aYTDCL,isnull(aYTD.aYTDEL, 0) as aYTDEL,isnull(aYTD.aYTDSL, 0) as aYTDSL,ISNULL(aMTD.aTDCL, 0) as aTDCL," +
                //        "ISNULL(aMTD.aTDEL, 0) as aTDEL ,ISNULL(aMTD.aTDSL, 0) as aTDSL,ISNULL(dYCL.dYCL, 0) as dYCL, ISNULL(dMCL.dMCL, 0) as dMCL,AbsentDay, DaysInMonth,PayableDays " +
                //        "from v_MonthlySalarySheet mss left join aYTD on mss.EmpId = aYTD.EmpId left join aMTD on mss.EmpId = aMTD.EmpId left join dYCL on mss.EmpId = dYCL.EmpID left  join dMCL on mss.EmpId = dMCL.EmpId cross join eLv " +
                //        "where  YearMonth='" + ddlYear.SelectedValue + "' and mss.EmpId='"+EmpId+"'";
                sqlCmd = @" with eLv as(
                       select sum( case when ShortName='c/l' then LeaveDays else 0 end) as eCL,sum( case when ShortName='s/l' then LeaveDays else 0 end) as eSL,
                        sum( case when ShortName='a/l' then LeaveDays else 0 end) as eEL from tblLeaveConfig where CompanyId='" + ddlCompanyName.SelectedValue + "'),aYTD as (select EmpId, sum(case when  ShortName='c/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end) as aYTDCL ,sum( case when  ShortName='s/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end ) as aYTDSL,sum( case when  ShortName='a/l' then 1 else 0 end )  as aYTDEL from v_Leave_LeaveApplicationDetails where CompanyId='" + ddlCompanyName.SelectedValue + "' and IsApproved=1  and Empid='" + EmpId + "' and LeaveDate>='" + yStartDate + "' and LeaveDate<='" + yEndDate + "' group by EmpId),aMTD as (select EmpId, sum(case when  ShortName='c/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end ) as aTDCL ,sum( case when  ShortName='s/l' then (case when ISHalfDayLeave=1 then 0.5  else 1 end) else 0 end ) as aTDSL,sum( case when  ShortName='a/l' then 1 else 0 end )  as aTDEL from v_Leave_LeaveApplicationDetails where CompanyId='" + ddlCompanyName.SelectedValue + "' and IsApproved=1    and Empid='" + EmpId + "' and LeaveDate>='" + mStartDate + "' and LeaveDate<='" + mEndDate + "' group by EmpId), dYCL as (select EmpID, Sum(Days) as dYCL from Leave_MonthlyLeaveDeductionRecord where Month >= '" + yStartDate + "' and Month<= '" + yEndDate + "' group by EmpID),dMCL as (select EmpID, Sum(Days) as dMCL from Leave_MonthlyLeaveDeductionRecord where Month >= '" + mStartDate + "' and Month<= '" + mEndDate + "' group by EmpID) select mss.EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,mss.CompanyId,mss.CompanyName,mss.Address,LateDays,LateFineDays as absentDueToLate,case when year(mss.EmpJoiningDate)= '" + month[0] + "' then nEb.CL else   eLv.eCL end as eCL,case when year(mss.EmpJoiningDate)= '" + month[0] + "' then nEb.SL else eLv.eSL end as eSL,case when isnull(ela.IsActive,0)=1 and ela.ActiveFrom<= '" + month[0] + "-01-01' then elv.eEL+ISNULL(ecf.ELDays,0) else 0 end as eEL, isnull(aYTD.aYTDCL, 0) as aYTDCL,isnull(aYTD.aYTDEL, 0) as aYTDEL,isnull(aYTD.aYTDSL, 0) as aYTDSL,ISNULL(aMTD.aTDCL, 0) as aTDCL,ISNULL(aMTD.aTDEL, 0) as aTDEL ,ISNULL(aMTD.aTDSL, 0) as aTDSL,ISNULL(dYCL.dYCL, 0) as dYCL, ISNULL(dMCL.dMCL, 0) as dMCL,AbsentDay, DaysInMonth,PayableDays from v_MonthlySalarySheet mss left join aYTD on mss.EmpId = aYTD.EmpId left join aMTD on mss.EmpId = aMTD.EmpId left join dYCL on mss.EmpId = dYCL.EmpID left  join dMCL on mss.EmpId = dMCL.EmpId left join Leave_EarnLeaveCarriedForward ecf on mss.EmpId=ecf.EmpId  and ecf.Year='" + month[0] + @"-01-01'  cross join eLv left join Leave_LeaveBalanceForNewEmp nEb on month(mss.EmpJoiningDate)=nEb.MonthID left join EarnLeave_Activationlog ela on mss.EmpId=ela.EmpID  where  YearMonth='" + ddlYear.SelectedValue + "' and mss.Empid='" + EmpId + "' ";
            }
            sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
            if (dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->No record found."; return;
            }

            Session["__MonthlyLeaveStatus__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=MonthlyLeaveStatus-"+ddlYear.SelectedItem.Text.Replace('-', '/') + "');", true);  //Open New Tab for Sever side code

        }

        private void GenerateYearlyLeaveStarus_SG()
        {
            try
            {

                
                if (lstSelected.Items.Count < 1 && txtCardNo.Text.Trim().Length == 0)
                { lblMessage.InnerText = "warning-> Please Select Department!"; lstSelected.Focus(); return; }
                string CompanyList = "";
                string ShiftList = "";
                string DepartmentList = "";
            
                if (!Page.IsValid)   // If Java script are desible then 
                {
                    lblMessage.InnerText = "erroe->Please Select From Date And To Date"; return;
                }
               
                ViewState["__FromDate__"] = ddlYear.SelectedValue + "-01-01";
                ViewState["__ToDate__"] = ddlYear.SelectedValue + "-12-31"; 

                string Cid = (ddlCompanyName.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                CompanyList = "in ('" + Cid + "')";                
                if (!ckbIndividualDetails.Checked)
                {
                    if (txtCardNo.Text.Trim().Length == 0)
                    {

                        DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                        //sqlCmd= @"with dCL as(select EmpID, Sum(Days)as dCL from Leave_MonthlyLeaveDeductionRecord  where Month >= '" + ViewState["__FromDate__"].ToString() + "' AND Month<= '" + ViewState["__ToDate__"].ToString() + "'  group by EmpID) "+
                        //    "SELECT ld.EmpId, EmpName,substring(EmpCardNo, 10, 6) EmpCardNo, Sex,ISNULL(dcl.dCL, 0) as dCL, SUM(CL) AS CL, SUM(SL) AS SL, SUM(ML) AS ML, SUM(AL) AS AL, SUM(OPL) AS OPL, SUM(OL) AS OL, "+
                        //    "ISNULL( dcl.dCL, 0)+SUM(CL) + SUM(SL) + SUM(ML) + SUM(AL) + SUM(OPL) + SUM(OL) AS Total, SUM(DISTINCT LeaveDays) -(ISNULL(dcl.dCL, 0) + SUM(CL) + SUM(SL) + SUM(ML) + SUM(AL) + SUM(OPL) + SUM(OL)) AS Remaining,"+
                        //    "DptId, DptName, DsgId, DsgName, CompanyId, SftId, SftName, CompanyName, Address FROM dbo.v_v_Leave_LeaveApplicationDetails AS ld left join dCL on ld.EmpId = dcl.EmpID "+
                        //    "where IsApproved=1 and ld.LeaveDate >= '" + ViewState["__FromDate__"].ToString() + "' AND ld.LeaveDate <= '" + ViewState["__ToDate__"].ToString() + "' AND CompanyId " + CompanyList + " AND " +
                        //    " DptId " + DepartmentList + " " +
                        //    "GROUP BY ld.EmpId, EmpName, EmpCardNo, Sex, DptId, DptName, DsgId, DsgName, CompanyId, SftId, SftName,CompanyName,Address,dcl.dCL " +
                        //    "order by substring(EmpCardNo,10,6)";
                        string AdminCondition = (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin")) ? "  and (EmpId='" + ViewState["__EmpId__"].ToString() + "' or EmpId in(select EmpId from  tblLeaveAuthorityAccessControl where AuthorityID=" + ViewState["__UserId__"].ToString() + ")) " : "";
                        sqlCmd = @"with dCL as(select EmpID, Sum(Days)as dCL from Leave_MonthlyLeaveDeductionRecord  where Month >= '" + ViewState["__FromDate__"].ToString() + @"' AND Month<= '" + ViewState["__ToDate__"].ToString() + @"'  group by EmpID) ,
bLv as (select sum(case when ShortName='c/l' then LeaveDays else 0 end ) as bCL,sum(case when ShortName='a/l' then LeaveDays else 0 end ) as bAL,sum(case when ShortName='s/l' then LeaveDays else 0 end ) as bSL,sum(case when ShortName='m/l' then LeaveDays else 0 end ) as bML from tblLeaveConfig where CompanyId " + CompanyList + @"),
lv as (
select EmpId, sum(case when ShortName='c/l' then TotalDays else 0 end ) as CL, sum(case when ShortName='s/l' then TotalDays else 0 end ) as SL, sum(case when ShortName='a/l' then TotalDays else 0 end ) as AL, sum(case when ShortName='m/l' then TotalDays else 0 end ) as ML from v_Leave_LeaveApplication where IsApproved=1 and FromDate >= '" + ViewState["__FromDate__"].ToString() + @"' AND FromDate <= '" + ViewState["__ToDate__"].ToString() + @"' group by EmpId),
lv1 as(
select ed.EmpId,SUBSTRING(ed.EmpCardNo,10,6) as EmpCardNo,ed.EmpName,ed.EmpJoiningDate, ed.DsgName,ed.DptName,ISNULL(dCL.dCL,0) as dCL, ISNULL(lv.CL,0) as CL,ISNULL(lv.SL,0) as SL,ISNULL(lv.AL,0) as AL,isnull(lv.ML,0) as ML,  case when ed.EmpJoiningDate  >= '" + ViewState["__FromDate__"].ToString() + @"' AND ed.EmpJoiningDate <= '" + ViewState["__ToDate__"].ToString() + @"' then nEb.CL else blv.bCL end as bCL,case when isnull(ela.IsActive,0)=1 and ela.ActiveFrom<= '" + ViewState["__ToDate__"].ToString() + @"' then bLv.bAL+isnull(ecf.ELDays,0) else 0 end as bAL,bLv.bML , case when ed.EmpJoiningDate  >= '" + ViewState["__FromDate__"].ToString() + @"' AND ed.EmpJoiningDate <= '" + ViewState["__ToDate__"].ToString() + @"' then nEb.SL else blv.bSL end as bSL,CompanyId,CompanyName,Address,Sex,DptId  from lv right join  v_EmployeeDetails ed on lv.EmpId=ed.EmpId and ed.IsActive=1  left join dCL on ed.EmpId=dCL.EmpID inner join Leave_LeaveBalanceForNewEmp nEb on month(ed.EmpJoiningDate)=nEb.MonthID left join EarnLeave_Activationlog ela on ed.EmpId=ela.EmpID left join Leave_EarnLeaveCarriedForward ecf on ed.EmpId=ecf.EmpId  and ecf.Year='" + ViewState["__FromDate__"].ToString() + @"' cross join blv where lv.EmpId is not null or   ( lv.EmpId  is null  and ed.EmpStatus in(1,8)) and ed.EmpJoiningDate < '" + ViewState["__ToDate__"].ToString() + @"' AND CompanyId " + CompanyList + " )" +
" select * from lv1 where   DptId " + DepartmentList + AdminCondition + " order by substring(EmpCardNo,10,6)";

                    }

                    else
                    {

                        //sqlCmd = @"with dCL as(select EmpID, Sum(Days)as dCL from Leave_MonthlyLeaveDeductionRecord  where Month >= '" + ViewState["__FromDate__"].ToString() + "' AND Month<= '" + ViewState["__ToDate__"].ToString() + "' and EmpID in(select EmpID from Personnel_EmployeeInfo where Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text + "')  group by EmpID) " +
                        //      "SELECT ld.EmpId, EmpName,substring(EmpCardNo, 10, 6) EmpCardNo, Sex,ISNULL(dcl.dCL, 0) as dCL, SUM(CL) AS CL, SUM(SL) AS SL, SUM(ML) AS ML, SUM(AL) AS AL, SUM(OPL) AS OPL, SUM(OL) AS OL, " +
                        //      "ISNULL( dcl.dCL, 0)+SUM(CL) + SUM(SL) + SUM(ML) + SUM(AL) + SUM(OPL) + SUM(OL) AS Total, SUM(DISTINCT LeaveDays) -(ISNULL(dcl.dCL, 0) + SUM(CL) + SUM(SL) + SUM(ML) + SUM(AL) + SUM(OPL) + SUM(OL)) AS Remaining," +
                        //      "DptId, DptName, DsgId, DsgName, CompanyId, SftId, SftName, CompanyName, Address FROM dbo.v_v_Leave_LeaveApplicationDetails AS ld left join dCL on ld.EmpId = dcl.EmpID " +
                        //      "where IsApproved=1 and ld.LeaveDate>='" + ViewState["__FromDate__"].ToString() + "' AND ld.LeaveDate<='" + ViewState["__ToDate__"].ToString() + "' AND CompanyId " + CompanyList + " AND " +
                        //                       " Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text + "'" +
                        //      "GROUP BY ld.EmpId, EmpName, EmpCardNo, Sex, DptId, DptName, DsgId, DsgName, CompanyId, SftId, SftName,CompanyName,Address,dcl.dCL " +
                        //      "order by substring(EmpCardNo,10,6)";

                        string AdminCondition = (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin")) ? "  where (EmpId='" + ViewState["__EmpId__"].ToString() + "' or EmpId in(select EmpId from  tblLeaveAuthorityAccessControl where AuthorityID=" + ViewState["__UserId__"].ToString() + ")) " : "";
                        sqlCmd = @"with dCL as(select EmpID, Sum(Days)as dCL from Leave_MonthlyLeaveDeductionRecord  where Month >= '" + ViewState["__FromDate__"].ToString() + @"' AND Month<= '" + ViewState["__ToDate__"].ToString() + @"' AND  EmpID in(select EmpID from Personnel_EmployeeInfo where Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + @"')  group by EmpID) ,
bLv as (select sum(case when ShortName='c/l' then LeaveDays else 0 end ) as bCL,sum(case when ShortName='a/l' then LeaveDays else 0 end ) as bAL,sum(case when ShortName='s/l' then LeaveDays else 0 end ) as bSL,sum(case when ShortName='m/l' then LeaveDays else 0 end ) as bML from tblLeaveConfig where CompanyId " + CompanyList + @"),
lv as (
select EmpId, sum(case when ShortName='c/l' then TotalDays else 0 end ) as CL, sum(case when ShortName='s/l' then TotalDays else 0 end ) as SL, sum(case when ShortName='a/l' then TotalDays else 0 end ) as AL, sum(case when ShortName='m/l' then TotalDays else 0 end ) as ML from v_Leave_LeaveApplication where IsApproved=1 and FromDate >= '" + ViewState["__FromDate__"].ToString() + @"' AND FromDate <= '" + ViewState["__ToDate__"].ToString() + @"' AND  EmpID in(select EmpID from Personnel_EmployeeInfo where Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + @"') group by EmpId),lv1 as(
select ed.EmpId,SUBSTRING(ed.EmpCardNo,10,6) as EmpCardNo,ed.EmpName,ed.EmpJoiningDate, ed.DsgName,ed.DptName,ISNULL(dCL.dCL,0) as dCL, ISNULL(lv.CL,0) as CL,ISNULL(lv.SL,0) as SL,ISNULL(lv.AL,0) as AL,isnull(lv.ML,0) as ML,  case when ed.EmpJoiningDate  >= '" + ViewState["__FromDate__"].ToString() + @"' AND ed.EmpJoiningDate <= '" + ViewState["__ToDate__"].ToString() + @"' then nEb.CL else blv.bCL end as bCL,case when isnull(ela.IsActive,0)=1 and ela.ActiveFrom<= '" + ViewState["__ToDate__"].ToString() + @"' then bLv.bAL+isnull(ecf.ELDays,0) else 0 end as bAL,bLv.bML , case when ed.EmpJoiningDate  >= '" + ViewState["__FromDate__"].ToString() + @"' AND ed.EmpJoiningDate <= '" + ViewState["__ToDate__"].ToString() + @"' then nEb.SL else blv.bSL end as bSL,CompanyId,CompanyName,Address,Sex,DptId  from lv right join  v_EmployeeDetails ed on lv.EmpId=ed.EmpId and ed.IsActive=1  left join dCL on ed.EmpId=dCL.EmpID inner join Leave_LeaveBalanceForNewEmp nEb on month(ed.EmpJoiningDate)=nEb.MonthID left join EarnLeave_Activationlog ela on ed.EmpId=ela.EmpID left join Leave_EarnLeaveCarriedForward ecf on ed.EmpId=ecf.EmpId  and ecf.Year='" + ViewState["__FromDate__"].ToString() + @"' cross join blv where lv.EmpId is not null or   ( lv.EmpId  is null  and ed.EmpStatus in(1,8)) and ed.EmpJoiningDate < '" + ViewState["__ToDate__"].ToString() + @"' AND CompanyId " + CompanyList + "  AND  ed.EmpID in(select EmpID from Personnel_EmployeeInfo where Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + "'))" +
" select * from lv1 "+AdminCondition+" order by substring(EmpCardNo,10,6)";
                    }

                   
                    sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->No record available."; return;
                    }
                    DataTable dtLeave;
                    sqlDB.fillDataTable("select ShortName, LeaveDays from tblLeaveConfig where CompanyId " + CompanyList + "", dtLeave = new DataTable());
                    Session["__dtLeave__"] = dtLeave;
                    Session["__LeaveYearlySummary__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LeaveYearlySummary-" + ddlYear.SelectedValue + "');", true);  //Open New Tab for Sever side code
                }
                else 
                {
                    string AdminCondition = (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin")) ? "  and (lv.EmpId='" + ViewState["__EmpId__"].ToString() + "' or lv.EmpId in(select EmpId from  tblLeaveAuthorityAccessControl where AuthorityID=" + ViewState["__UserId__"].ToString() + ")) " : "";
                    if (txtCardNo.Text.Trim().Length == 0)
                    {

                        DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                        //sqlCmd = "select CompanyId,v_Leave_LeaveApplication.EmpId,CompanyName,Address,TotalDays, LACode, ShortName,LeaveName,SUBSTRING(EmpCardNo,10,6) EmpCardNo,EmpName,DptId,DptName,DsgId,DsgName," +
                        //    " convert(varchar, ApplyDate,105) ApplyDate,convert(varchar, FromDate,105) FromDate,convert(varchar, ToDate,105) ToDate,case when ShortName='s/l' " +
                        //    " then TotalDays else '' end SL,case when ShortName='c/l' then TotalDays else '' end CL,case when ShortName='a/l' then TotalDays else '' end EL ,case when " +
                        //    " ShortName='m/l' then TotalDays else '' end ML  ,Sex as EmpType " +
                        //    "  from v_Leave_LeaveApplication left join Personnel_EmpPersonnal on v_Leave_LeaveApplication.EmpId=Personnel_EmpPersonnal.EmpId where IsApproved=1 and year(FromDate)='" + ddlYear.SelectedValue + "' and CompanyId " + CompanyList + "  AND DptId " + DepartmentList +
                        //    " order by convert(int, DptId),convert(int,SUBSTRING(EmpCardNo,10,6)), FromDate";

                        sqlCmd = @"with dCL as(select EmpID, Sum(Days)as dCL from Leave_MonthlyLeaveDeductionRecord  where Month >= '" + ddlYear.SelectedValue + @"-01-01' AND Month<= '" + ddlYear.SelectedValue + @"-12-31'  group by EmpID) ,bLv as (select sum(case when ShortName='c/l' then LeaveDays else 0 end ) as bCL,sum(case when ShortName='a/l' then LeaveDays else 0 end ) as bAL,sum(case when ShortName='s/l' then LeaveDays else 0 end ) as bSL,sum(case when ShortName='m/l' then LeaveDays else 0 end ) as bML from tblLeaveConfig where CompanyId " + CompanyList + ") select CompanyId,lv.EmpId,CompanyName,Address,TotalDays, LACode, ShortName,LeaveName,SUBSTRING(EmpCardNo,10,6) EmpCardNo,EmpName,DptId,DptName,DsgId,DsgName, convert(varchar, ApplyDate,105) ApplyDate,convert(varchar, FromDate,105) FromDate,convert(varchar, ToDate,105) ToDate,case when ShortName='s/l'  then TotalDays else '' end SL,case when ShortName='c/l' then TotalDays else '' end CL,case when ShortName='a/l' then TotalDays else '' end AL ,case when  ShortName='m/l' then TotalDays else '' end ML,  case when lv.EmpJoiningDate  >= '" + ddlYear.SelectedValue + @"-01-01' AND lv.EmpJoiningDate <= '" + ddlYear.SelectedValue + "-12-31' then nEb.CL else blv.bCL end as bCL,case when isnull(ela.IsActive,0)=1 and ela.ActiveFrom<= '" + ddlYear.SelectedValue + "-12-31' then bLv.bAL+isnull(ecf.ELDays,0) else 0 end as bAL,case when EmpType='Female' then bLv.bML else 0 end as bML  , case when lv.EmpJoiningDate  >= '" + ddlYear.SelectedValue + "-01-01' AND lv.EmpJoiningDate <= '" + ddlYear.SelectedValue + "-12-31' then nEb.SL else blv.bSL end as bSL  ,Sex as EmpType,isnull(dCL.dCL,0) as dCL from v_Leave_LeaveApplication lv left join Personnel_EmpPersonnal on lv.EmpId=Personnel_EmpPersonnal.EmpId left join Leave_LeaveBalanceForNewEmp nEb on month(lv.EmpJoiningDate)=nEb.MonthID left join EarnLeave_Activationlog ela on lv.EmpId=ela.EmpID left join dCL on lv.EmpId=dCL.EmpID left join Leave_EarnLeaveCarriedForward ecf on lv.EmpId=ecf.EmpId  and ecf.Year='" + ddlYear.SelectedValue + @"-01-01' cross join blv where IsApproved=1 and year(FromDate)='" + ddlYear.SelectedValue + "' and CompanyId " + CompanyList + "  AND DptId " + DepartmentList+ AdminCondition + " order by convert(int, DptId),convert(int,SUBSTRING(EmpCardNo,10,6)), FromDate";
                        
                    }
                    else
                        //sqlCmd = "select CompanyId,v_Leave_LeaveApplication.EmpId,CompanyName,Address,TotalDays, LACode, ShortName,LeaveName,SUBSTRING(EmpCardNo,10,6) EmpCardNo,EmpName,DptId,DptName,DsgId,DsgName," +
                        //  " convert(varchar, ApplyDate,105) ApplyDate,convert(varchar, FromDate,105) FromDate,convert(varchar, ToDate,105) ToDate,case when ShortName='s/l' " +
                        //  " then TotalDays else '' end SL,case when ShortName='c/l' then TotalDays else '' end CL,case when ShortName='a/l' then TotalDays else '' end EL ,case when " +
                        //  " ShortName='m/l' then TotalDays else '' end ML  ,Sex as EmpType " +
                        //  "  from v_Leave_LeaveApplication left join Personnel_EmpPersonnal on v_Leave_LeaveApplication.EmpId=Personnel_EmpPersonnal.EmpId where IsApproved=1 and year(FromDate)='" + ddlYear.SelectedValue + "' and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text + "'  order by  FromDate";
                        sqlCmd = @"with dCL as(select EmpID, Sum(Days)as dCL from Leave_MonthlyLeaveDeductionRecord  where Month >= '" + ddlYear.SelectedValue + @"-01-01' AND Month<= '" + ddlYear.SelectedValue + @"-12-31' AND  EmpID in(select EmpID from Personnel_EmployeeInfo where Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + @"')  group by EmpID) ,bLv as (select sum(case when ShortName='c/l' then LeaveDays else 0 end ) as bCL,sum(case when ShortName='a/l' then LeaveDays else 0 end ) as bAL,sum(case when ShortName='s/l' then LeaveDays else 0 end ) as bSL,sum(case when ShortName='m/l' then LeaveDays else 0 end ) as bML from tblLeaveConfig where CompanyId " + CompanyList + ") select CompanyId,lv.EmpId,CompanyName,Address,TotalDays, LACode, ShortName,LeaveName,SUBSTRING(EmpCardNo,10,6) EmpCardNo,EmpName,DptId,DptName,DsgId,DsgName, convert(varchar, ApplyDate,105) ApplyDate,convert(varchar, FromDate,105) FromDate,convert(varchar, ToDate,105) ToDate,case when ShortName='s/l'  then TotalDays else '' end SL,case when ShortName='c/l' then TotalDays else '' end CL,case when ShortName='a/l' then TotalDays else '' end AL ,case when  ShortName='m/l' then TotalDays else '' end ML,  case when lv.EmpJoiningDate  >= '" + ddlYear.SelectedValue + @"-01-01' AND lv.EmpJoiningDate <= '" + ddlYear.SelectedValue + "-12-31' then nEb.CL else blv.bCL end as bCL,case when isnull(ela.IsActive,0)=1 and ela.ActiveFrom<= '" + ddlYear.SelectedValue + "-12-31' then bLv.bAL+isnull(ecf.ELDays,0) else 0 end as bAL,case when EmpType='Female' then bLv.bML else 0 end as bML  , case when lv.EmpJoiningDate  >= '" + ddlYear.SelectedValue + "-01-01' AND lv.EmpJoiningDate <= '" + ddlYear.SelectedValue + "-12-31' then nEb.SL else blv.bSL end as bSL  ,Sex as EmpType,isnull(dCL.dCL,0) as dCL from v_Leave_LeaveApplication lv left join Personnel_EmpPersonnal on lv.EmpId=Personnel_EmpPersonnal.EmpId left join Leave_LeaveBalanceForNewEmp nEb on month(lv.EmpJoiningDate)=nEb.MonthID left join EarnLeave_Activationlog ela on lv.EmpId=ela.EmpID left join dCL on lv.EmpId=dCL.EmpID left join Leave_EarnLeaveCarriedForward ecf on lv.EmpId=ecf.EmpId  and ecf.Year='" + ddlYear.SelectedValue + @"-01-01' cross join blv where IsApproved=1 and year(FromDate)='" + ddlYear.SelectedValue + "' and CompanyId " + CompanyList + "  AND  lv.EmpID in(select EmpID from Personnel_EmployeeInfo where Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) like'%" + txtCardNo.Text.Trim() + @"') "+ AdminCondition + " order by convert(int, DptId),convert(int,SUBSTRING(EmpCardNo,10,6)), FromDate";
                    sqlDB.fillDataTable(sqlCmd, dt = new DataTable());                  
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Data not found."; return;
                    }
                    DataTable dtLeave;
                    sqlDB.fillDataTable("select ShortName, LeaveDays from tblLeaveConfig where CompanyId " + CompanyList + "", dtLeave = new DataTable());
                    Session["__dtLeave__"] = dtLeave;
                    Session["__LeaveYearlySummaryIndividualDetails__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LeaveYearlySummaryIndividualDetails-" + ddlYear.SelectedValue + "');", true);  //Open New Tab for Sever side code
               
                  
                }
            }
            catch { }
        }
        private void addAllTextInShift()
        {
            if (ddlShiftName.Items.Count > 2)
                ddlShiftName.Items.Insert(1, new ListItem("All", "00"));
        }

        protected void ddlShiftName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lstAll.Items.Clear();
                lstSelected.Items.Clear();
                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();

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

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {


            companyId= (ddlCompanyName.SelectedValue=="0000")?ViewState["__CompanyId__"].ToString():ddlCompanyName.SelectedValue.ToString();
            if (!chkForAllCompany.Checked)
            {

                classes.commonTask.LoadShift(ddlShiftName, companyId, "Admin");
            }
            else
            {

                classes.commonTask.LoadShift(ddlShiftName, companyId, ViewState["__UserType__"].ToString());
            }
            addAllTextInShift();
            loadYear();
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveItem(lstAll, lstSelected);
        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveAll(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveAll(lstSelected, lstAll);
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            //GenerateYearlyLeaveStarus();

            
            if (rblReportType.SelectedValue == "Yearly")
                GenerateYearlyLeaveStarus_SG();
            else
                GenerateMonthlyLeaveStatus();
        }

        protected void rblReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblReportType.SelectedValue == "Yearly")
            {
                ckbIndividualDetails.Visible = true;
                tdMonthYear.InnerText = "Year     ";
                loadYear();
            }
            else
            {
                ckbIndividualDetails.Visible = false;
                tdMonthYear.InnerText = "Month     ";
                classes.Payroll.loadMonthIdByCompany(ddlYear, ViewState["__CompanyId__"].ToString());
            }
               
        }
    }
}