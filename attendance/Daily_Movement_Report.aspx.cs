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

namespace SigmaERP.attendance
{
    public partial class Daily_Movement_Report : System.Web.UI.Page
    {
        DataTable dtSetPrivilege;
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
                txtToDate.Text = txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Enabled = false;
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

                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "Daily_Movement_Report.aspx", ddlCompany, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                classes.commonTask.LoadDepartment(ViewState["__CompanyId__"].ToString(), lstAll);

                //-----------------------------------------------------

            
               
            }
            catch { }
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
            if (rblReportType.SelectedValue == "1" && txtToDate.Text == "")
            {
                lblMessage.InnerText = "warning-> Please Select To Date ";
                txtToDate.Focus(); return;
            }
            if (txtCardNo.Text.Trim() == "")
            {

             
                if (lstSelected.Items.Count == 0)
                {
                    lblMessage.InnerText = "warning-> Please Select Any Shift!";
                    lstSelected.Focus();

                    return;
                }
            }
            else 
            {
                if (txtCardNo.Text.Trim().Length < 4)
                {
                    lblMessage.InnerText = "warning-> Please Type Valid Card Number!";
                    return;
                }
            }
            LoadDailyMovementData();
        }
        private void LoadDailyMovementData()
        {
            DataTable dt;
            string CmdSql = "";
            string[] dmy = txtDate.Text.Split('-');
            string d = dmy[0]; string m = dmy[1]; string y = dmy[2];

            string DepartmentList = "";

            if (!Page.IsValid)   // If Java script are desible then 
            {
                lblMessage.InnerText = "erroe->Please Select From Date And To Date"; return;
            }
            DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
            string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and CS.EmpTypeId=" + rblEmpType.SelectedValue + " ";
            string DateOrRange = "";
            if (rblReportType.SelectedValue == "1")  // for Date Range
            {

                //string[] Tdmy = txtToDate.Text.Split('-');
                //string Td = Tdmy[0]; string Tm = Tdmy[1]; string Ty = Tdmy[2];
                //if (txtCardNo.Text.Trim().Length == 0) 
                //{
                //    CmdSql = "select distinct cs.CompanyId,  cs.DptId, cs.SftId, cs.EmpNameBn EmpName,cs.DptNameBn DptName,cs.DsgNameBn DsgName," +
                //                            "cs.CompanyNameBangla CompanyName,cs.AddressBangla Address,cio.Badgenumber,cio.CHECKTIME,FORMAT(cio.CHECKTIME,'dd-MM-yyy') as Sex,cs.SftNameBangla SftName,cs.MillNoBn EmpAttCard," +
                //                            " Month(cio.CHECKTIME) as DsgId from v_CHECKINOUT cio  inner join v_Personnel_EmpCurrentStatusForMovement cs on cio.Badgenumber=cs.EmpCardNo" +
                //                            " where cs.EmpStatus in (1,8) " + EmpTypeID + " and cs.SftId " + DepartmentList + " " + Gender + "  and  cs.DptId ='" + ddlShiftList.SelectedValue + "' " +
                //                            " and cs.Emp_Mill_No='" + ddlMillNo.SelectedValue + "' AND cio.CHECKTIME >='" + y + "-" + m + "-" + d + "' and cio.CHECKTIME<=(select dateadd(day,1,'" + Ty + "-" + Tm + "-" + Td + "')) " +
                //                            " ";                    
                //}

                //else
                //{
                //    CmdSql = "select distinct cs.Sex as Gender, cs.CompanyId,  cs.DptId, cs.SftId, cs.EmpNameBn EmpName,cs.DptNameBn DptName,cs.DsgNameBn DsgName," +
                //        "cs.CompanyNameBangla CompanyName,cs.AddressBangla Address,cio.Badgenumber,cio.CHECKTIME,FORMAT(cio.CHECKTIME,'dd-MM-yyy') as Sex,cs.SftNameBangla SftName,cs.MillNoBn EmpAttCard," +
                //        " Month(cio.CHECKTIME) as DsgId from v_CHECKINOUT cio  inner join v_Personnel_EmpCurrentStatusForMovement cs on cio.Badgenumber=cs.EmpCardNo" +
                //        " where cs.EmpStatus in (1,8) and cs.EmpCardNo ='" + txtCardNo.Text.Trim() + "' AND cio.CHECKTIME >='" + y + "-" + m + "-" + d + "' and cio.CHECKTIME<=(select dateadd(day,1,'" + Ty + "-" + Tm + "-" + Td + "')) " +
                //        " ";  


                //}
                //DateOrRange =classes.commonTask.GenerateBanglaMonthDMY(txtDate.Text) + " †_‡K " +classes.commonTask.GenerateBanglaMonthDMY(txtToDate.Text);
            }
            else // for single Date 
            {
                if (txtCardNo.Text.Trim().Length == 0)
                    CmdSql = "select distinct cs.CompanyId,  cs.DptId, cs.SftId, cs.EmpName,cs.DptName,cs.DsgName," +
                        "cs.CompanyName,cs.Address,cio.Badgenumber,cio.CHECKTIME,FORMAT(cio.CHECKTIME,'dd-MM-yyy') as Sex,cs.SftName," +
                        " Month(cio.CHECKTIME) as DsgId from v_CHECKINOUT cio  inner join v_Personnel_EmpCurrentStatus cs on cio.Badgenumber=cs.EmpCardNo" +
                        " where cs.EmpStatus in (1,8) " + EmpTypeID + " and cs.DptId " + DepartmentList + " " +
                        "  AND cio.CHECKTIME >='" + y + "-" + m + "-" + d + "' and cio.CHECKTIME<=(select dateadd(day,1,'" + y + "-" + m + "-" + d + "')) " +
                        " ";
                else
                {
                    CmdSql = "select distinct cs.Sex as Gender, cs.CompanyId,  cs.DptId, cs.SftId, cs.EmpName,cs.DptName,cs.DsgName," +
                      "cs.CompanyName,cs.Address,cio.Badgenumber,cio.CHECKTIME,FORMAT(cio.CHECKTIME,'dd-MM-yyy') as Sex,cs.SftName," +
                      " Month(cio.CHECKTIME) as DsgId from v_CHECKINOUT cio  inner join v_Personnel_EmpCurrentStatus cs on cio.Badgenumber=cs.EmpCardNo" +
                      " where cs.EmpStatus in (1,8) and cs.EmpCardNo ='" + txtCardNo.Text.Trim() + "' AND cio.CHECKTIME >='" + y + "-" + m + "-" + d + "' and cio.CHECKTIME<=(select dateadd(day,1,'" + y + "-" + m + "-" + d + "')) " +
                      " ";


                }
                DateOrRange = classes.commonTask.GenerateBanglaMonthDMY(txtDate.Text);
            }
            sqlDB.fillDataTable(CmdSql, dt = new DataTable());
            if (dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->No Attendance Available";
                return;
            }
            if (txtCardNo.Text.Trim().Length != 0)

                Session["__DailyMovementReport__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=DailyMovementReport-" + DateOrRange.Replace('-', '/') + "');", true);  //Open New Tab for Sever side code
        }
        private void LoadDailyMovementDataBangla() 
        {
            DataTable dt;
            string CmdSql = "";
            string[] dmy = txtDate.Text.Split('-');
            string d = dmy[0]; string m = dmy[1]; string y = dmy[2];
          
            string DepartmentList = "";

            if (!Page.IsValid)   // If Java script are desible then 
            {
                lblMessage.InnerText = "erroe->Please Select From Date And To Date"; return;
            }
           DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
           string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and CS.EmpTypeId=" + rblEmpType.SelectedValue + " ";
            string DateOrRange = "";
            if (rblReportType.SelectedValue == "1")  // for Date Range
            {

                //string[] Tdmy = txtToDate.Text.Split('-');
                //string Td = Tdmy[0]; string Tm = Tdmy[1]; string Ty = Tdmy[2];
                //if (txtCardNo.Text.Trim().Length == 0) 
                //{
                //    CmdSql = "select distinct cs.CompanyId,  cs.DptId, cs.SftId, cs.EmpNameBn EmpName,cs.DptNameBn DptName,cs.DsgNameBn DsgName," +
                //                            "cs.CompanyNameBangla CompanyName,cs.AddressBangla Address,cio.Badgenumber,cio.CHECKTIME,FORMAT(cio.CHECKTIME,'dd-MM-yyy') as Sex,cs.SftNameBangla SftName,cs.MillNoBn EmpAttCard," +
                //                            " Month(cio.CHECKTIME) as DsgId from v_CHECKINOUT cio  inner join v_Personnel_EmpCurrentStatusForMovement cs on cio.Badgenumber=cs.EmpCardNo" +
                //                            " where cs.EmpStatus in (1,8) " + EmpTypeID + " and cs.SftId " + DepartmentList + " " + Gender + "  and  cs.DptId ='" + ddlShiftList.SelectedValue + "' " +
                //                            " and cs.Emp_Mill_No='" + ddlMillNo.SelectedValue + "' AND cio.CHECKTIME >='" + y + "-" + m + "-" + d + "' and cio.CHECKTIME<=(select dateadd(day,1,'" + Ty + "-" + Tm + "-" + Td + "')) " +
                //                            " ";                    
                //}
                                       
                //else
                //{
                //    CmdSql = "select distinct cs.Sex as Gender, cs.CompanyId,  cs.DptId, cs.SftId, cs.EmpNameBn EmpName,cs.DptNameBn DptName,cs.DsgNameBn DsgName," +
                //        "cs.CompanyNameBangla CompanyName,cs.AddressBangla Address,cio.Badgenumber,cio.CHECKTIME,FORMAT(cio.CHECKTIME,'dd-MM-yyy') as Sex,cs.SftNameBangla SftName,cs.MillNoBn EmpAttCard," +
                //        " Month(cio.CHECKTIME) as DsgId from v_CHECKINOUT cio  inner join v_Personnel_EmpCurrentStatusForMovement cs on cio.Badgenumber=cs.EmpCardNo" +
                //        " where cs.EmpStatus in (1,8) and cs.EmpCardNo ='" + txtCardNo.Text.Trim() + "' AND cio.CHECKTIME >='" + y + "-" + m + "-" + d + "' and cio.CHECKTIME<=(select dateadd(day,1,'" + Ty + "-" + Tm + "-" + Td + "')) " +
                //        " ";  
                   
                   
                //}
                //DateOrRange =classes.commonTask.GenerateBanglaMonthDMY(txtDate.Text) + " †_‡K " +classes.commonTask.GenerateBanglaMonthDMY(txtToDate.Text);
            }
            else // for single Date 
            {
                if (txtCardNo.Text.Trim().Length == 0)
                    CmdSql = "select distinct cs.CompanyId,  cs.DptId, cs.SftId, cs.EmpNameBn EmpName,cs.DptNameBn DptName,cs.DsgNameBn DsgName,"+
                        "cs.CompanyNameBangla CompanyName,cs.AddressBangla Address,cio.Badgenumber,cio.CHECKTIME,FORMAT(cio.CHECKTIME,'dd-MM-yyy') as Sex,cs.SftNameBangla SftName,cs.MillNoBn EmpAttCard," +
                        " Month(cio.CHECKTIME) as DsgId from v_CHECKINOUT cio  inner join v_Personnel_EmpCurrentStatus cs on cio.Badgenumber=cs.EmpCardNo"+
                        " where cs.EmpStatus in (1,8) " + EmpTypeID + " and cs.DptId " + DepartmentList + " " +
                        "  AND cio.CHECKTIME >='" + y + "-" + m + "-" + d + "' and cio.CHECKTIME<=(select dateadd(day,1,'" + y + "-" + m + "-" + d + "')) " +
                        " ";
                else
                {
                    CmdSql = "select distinct cs.Sex as Gender, cs.CompanyId,  cs.DptId, cs.SftId, cs.EmpNameBn EmpName,cs.DptNameBn DptName,cs.DsgNameBn DsgName," +
                      "cs.CompanyNameBangla CompanyName,cs.AddressBangla Address,cio.Badgenumber,cio.CHECKTIME,FORMAT(cio.CHECKTIME,'dd-MM-yyy') as Sex,cs.SftNameBangla SftName," +
                      " Month(cio.CHECKTIME) as DsgId from v_CHECKINOUT cio  inner join v_Personnel_EmpCurrentStatus cs on cio.Badgenumber=cs.EmpCardNo" +
                      " where cs.EmpStatus in (1,8) and cs.EmpCardNo ='" + txtCardNo.Text.Trim() + "' AND cio.CHECKTIME >='" + y + "-" + m + "-" + d + "' and cio.CHECKTIME<=(select dateadd(day,1,'" + y + "-" + m + "-" + d + "')) " +
                      " ";
                   
                  
                }
                DateOrRange = classes.commonTask.GenerateBanglaMonthDMY(txtDate.Text);
            }
            sqlDB.fillDataTable(CmdSql, dt = new DataTable());
            if (dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->No Attendance Available";
                return;
            }
            if (txtCardNo.Text.Trim().Length != 0)
              
            Session["__DailyMovementReportBangla__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=DailyMovementReportBangla-" + DateOrRange.Replace('-', '/')+"');",true);  //Open New Tab for Sever side code
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.commonTask.LoadDepartment(ddlCompany.SelectedValue, lstAll);
        }

        protected void rblReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblReportType.SelectedValue == "1")
            {
                tdDate.InnerText = "From Date";
                txtToDate.Enabled = true;
            }
            else
            {
                tdDate.InnerText = "Date";
                txtToDate.Enabled = false;
            }
        }
        protected void ddlShiftList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               
                lstAll.Items.Clear();
                lstSelected.Items.Clear();
                string CompanyId = (ddlCompany.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue.ToString();                
               
            }
            catch { }
        }
    }
}