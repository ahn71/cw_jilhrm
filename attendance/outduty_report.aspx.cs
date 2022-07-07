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
    public partial class outduty_report : System.Web.UI.Page
    {
        string CompanyId = "";
        DataTable dt;
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
                txtFromDate.Text = "01-" + DateTime.Now.ToString("MM-yyyy");
                txtToDate.Text = DateTime.DaysInMonth(DateTime.Now.Year,DateTime.Now.Month)+"-" + DateTime.Now.ToString("MM-yyyy");
            }
        }
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserId__"] = getUserId;
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__EmpId__"] = getCookies["__getEmpId__"].ToString();
                ViewState["__ODOnlyDpt__"] = getCookies["__getODOnlyDpt__"].ToString();
                ViewState["__DptId__"] = getCookies["__getDptId__"].ToString();
                //------------load privilege setting inof from db------
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "daily_movement.aspx", ddlCompany, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                //-----------------------------------------------------
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "User")
                {
                    ViewState["__ReadAction__"] = "1";
                    workerlist.Visible = false;
                    txtCardNo.Text = commonTask.returnEmpCardNoByEmpId(ViewState["__EmpId__"].ToString());
                    txtCardNo.Enabled = false;
                    lnkNew.Visible = false;
                }
                else
                {
                    //if(ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin" && ViewState["__ODOnlyDpt__"].ToString().Equals("True"))
                    //classes.commonTask.LoadDepartment(ViewState["__CompanyId__"].ToString(), ViewState["__DptId__"].ToString(), lstAll);
                    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin")
                    classes.commonTask.LoadDepartmentListByAdminForOutDuty(lstAll,ViewState["__CompanyId__"].ToString(),ViewState["__UserId__"].ToString());
                    else
                    classes.commonTask.LoadDepartment(ViewState["__CompanyId__"].ToString(), lstAll);
                }
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
        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
           

              CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin")
                classes.commonTask.LoadDepartmentListByAdminForOutDuty(lstAll,CompanyId, ViewState["__UserId__"].ToString());
            else
                classes.commonTask.LoadDepartment(CompanyId, lstAll);
            lstSelected.Items.Clear();
            

        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            generateOutdutyReport();
        }
        private void generateOutdutyReport()
        {
            if (txtFromDate.Text.Trim().Length != 10)
            {
                lblMessage.InnerText = "warning-> Please Select From Date !";
                txtFromDate.Focus();
                return;
            }
            if (txtToDate.Text.Trim().Length != 10)
            {
                lblMessage.InnerText = "warning-> Please Select To Date !";
                txtToDate.Focus();
                return;
            }
            if (lstSelected.Items.Count == 0 && txtCardNo.Text.Trim().Length == 0)
            {
                lblMessage.InnerText = "warning-> Please Select Any Department!";
                lstSelected.Focus();                
                return;
            }
           
            string[] Fdmy = txtFromDate.Text.Split('-');
            string[] Tdmy = txtToDate.Text.Split('-');
            string FDate = commonTask.ddMMyyyyToyyyyMMdd(txtFromDate.Text);
            string TDate = commonTask.ddMMyyyyToyyyyMMdd(txtToDate.Text);      

            string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId=" + rblEmpType.SelectedValue + " ";
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue.ToString();


            string CompanyList = "";
            string sqlCmd = "";
            string DepartmentList = "";
            CompanyList = "in ('" + CompanyId + "')";
            
            if (txtCardNo.Text.Trim().Length == 0)
            {
                DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                sqlCmd = "SELECT SL,ODID, EmpId,AuthorizedByName as AuthorizedBy, EmpName, DptName, CompanyName, InTime, OutTime, Address, case when Type=1 then '[Training]' else   ClientName end as ClientName,case when Type=1 then Remark else  Purpose end as Purpose,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo, convert(varchar(10), Date, 105) Date FROM   v_tblOutDutyDetails where Status=1 and Date>='" + FDate + "' and Date<='" + TDate + "' and IsActive=1 and CompanyId " + CompanyList + "   AND DptId " + DepartmentList + " " + EmpTypeID + "  and(EmpId='" + ViewState["__EmpId__"].ToString() + "' or EmpId in(select EmpId from  tblOutDutyAuthorityAccessControl where AuthorityID="+ ViewState["__UserId__"].ToString() + "))   order by convert(int,DptCode),convert(int,GId),CustomOrdering  ";
                
            }
            else
            {
                if (txtCardNo.Text.Trim().Length < 4)
                {
                    lblMessage.InnerText = "warning-> Please Type Valid Card Number!(Minimum " + Session["__MinDigits__"].ToString() + " Digits)";
                    txtCardNo.Focus();                  
                    return;
                }            
                    sqlCmd = "SELECT SL,ODID, EmpId,AuthorizedByName as AuthorizedBy, EmpName, DptName, CompanyName, InTime, OutTime, Address, case when Type=1 then '[Training]' else   ClientName end as ClientName,case when Type=1 then Remark else  Purpose end as Purpose,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo, convert(varchar(10), Date, 105) Date FROM   v_tblOutDutyDetails where Status=1 and Date>='" + FDate + "' and Date<='" + TDate + "' and IsActive=1 and CompanyId " + CompanyList + "  and Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) Like'%" + txtCardNo.Text.Trim() + "' and(EmpId='" + ViewState["__EmpId__"].ToString() + "' or EmpId in(select EmpId from  tblOutDutyAuthorityAccessControl where AuthorityID=" + ViewState["__UserId__"].ToString() + ")) ";
            
            }
            sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
            if (dt==null || dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->No Data Available";              
                return;
            }
            string dateRange = txtFromDate.Text.Trim() + " to " + txtToDate.Text.Trim();
            Session["__OutDutyByDateRange__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=OutDutyByDateRange-" + dateRange.Replace('-','/') + "');", true);  //Open New Tab for Sever side code


        }
    }
}