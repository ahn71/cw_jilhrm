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

namespace SigmaERP.personnel
{
    public partial class leave_balance_report : System.Web.UI.Page
    {
        DataTable dt;

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


        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];

                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();

                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "leave_balance_report.aspx", ddlCompanyName, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];             
                    classes.commonTask.LoadShift(ddlShiftName, ViewState["__CompanyId__"].ToString());                      

                addAllTextInShift();

            }
            catch { }

        }
        private void addAllTextInShift()
        {
            if (ddlShiftName.Items.Count > 2)
                ddlShiftName.Items.Insert(1, new ListItem("All", "00"));
        }
        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!chkForAllCompany.Checked)
            {
                
                classes.commonTask.LoadShift(ddlShiftName, ddlCompanyName.SelectedValue.ToString(),"Admin");
            }
            else
            {
                
                classes.commonTask.LoadShift(ddlShiftName, ddlCompanyName.SelectedValue.ToString(), ViewState["__UserType__"].ToString());
            }
            addAllTextInShift();
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            generateReprotElementByUsrType();
        }

        private void generateReprotElementByUsrType()
        {
            try
            {   string CompanyList="";
                string ShiftList="";
                string DepartmentList="";

                if (!Page.IsValid)   // If Java script are desible then 
                {
                    lblMessage.InnerText = "erroe->Please Select From Date And To Date"; return;
                }
                string[] DatesFormat = txtFromDate.Text.Trim().Split('-');
                ViewState["__FromDate__"] = DatesFormat[2] + "-" + DatesFormat[1] + "-" + DatesFormat[0];
                DatesFormat = txtToDate.Text.Trim().Split('-');
                ViewState["__ToDate__"] = DatesFormat[2] + "-" + DatesFormat[1] + "-" + DatesFormat[0];

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
                sqlDB.fillDataTable("SELECT EmpId, EmpName, EmpCardNo, Sex, SUM(CL) AS CL, SUM(SL) AS SL, SUM(ML) AS ML, SUM(AL) AS AL, SUM(OPL) AS OPL, SUM(OL) AS OL," +
                    "SUM(CL)+SUM(SL) + SUM(ML) + SUM(AL) + SUM(OPL) + SUM(OL) AS Total, SUM(DISTINCT LeaveDays) - (SUM(CL) + SUM(SL) + SUM(ML) + SUM(AL) + SUM(OPL)+SUM(OL)) AS Remaining," +
                    "DptId, DptName, DsgId, DsgName, CompanyId, SftId, SftName,CompanyName,Address " +
                    "FROM dbo.v_v_Leave_LeaveApplicationDetails AS ld where ld.LeaveDate>='" + ViewState["__FromDate__"].ToString() + "' AND ld.LeaveDate<='" + ViewState["__ToDate__"].ToString() + "' AND CompanyId " + CompanyList + " AND " +
                    " SftId " + ShiftList + " AND DptId " + DepartmentList + "" +
                    "GROUP BY EmpId, EmpName, EmpCardNo, Sex, DptId, DptName, DsgId, DsgName, CompanyId, SftId, SftName,CompanyName,Address", dt = new DataTable());

                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                }
                string DateRange=(txtFromDate.Text.Trim().Equals(txtToDate.Text))?"Date "+txtFromDate.Text.Trim():" Date From "+txtFromDate.Text+" To "+txtToDate.Text.Trim();
               
                Session["__LeaveBalanceReport__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LeaveBalanceReport-" +DateRange+ "');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }

        protected void chkForAllCompany_CheckedChanged(object sender, EventArgs e)
        {
            if (chkForAllCompany.Checked)
            {
                ddlCompanyName.Enabled = false;
                ddlShiftName.Enabled = false;
                divDepartmentList.Visible = false;
            }
            else
            {
                ddlCompanyName.Enabled = true;
                
                ddlShiftName.Enabled = true;
                divDepartmentList.Visible = true;
            }
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

    }
}