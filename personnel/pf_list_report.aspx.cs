using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class pf_list_report : System.Web.UI.Page
    {
        DataTable dt;
        DataTable dtSetPrivilege;
        string CompanyId = "";
        string Cmd = "";
        string EmpTypeID = "";
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

                //-----------------------------------------------------

                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
                {
                    trCompanyName.Visible = true;
                    classes.commonTask.LoadBranch(ddlCompany);
                    //ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
                    classes.commonTask.LoadInitialShift(ddlShiftList, ViewState["__CompanyId__"].ToString());

                }
                else
                {
                    classes.commonTask.LoadBranch(ddlCompany, ViewState["__CompanyId__"].ToString());
                    classes.commonTask.LoadShift(ddlShiftList, ViewState["__CompanyId__"].ToString());
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='daily_movement.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege = new DataTable());
                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                        {
                            btnPreview.CssClass = "css_btn Ptbut"; btnPreview.Enabled = true;
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

                addAllTextInShift();
                ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
            catch { }
        }
        private void addAllTextInShift()
        {
            if (ddlShiftList.Items.Count > 2)
                ddlShiftList.Items.Insert(1, new ListItem("All", "00"));
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
            classes.commonTask.LoadInitialShift(ddlShiftList, CompanyId);
            addAllTextInShift();
        }

        protected void ddlShiftList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lstAll.Items.Clear();
                lstSelected.Items.Clear();
                string CompanyId = (ddlCompany.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue.ToString();

                if (ddlShiftList.SelectedItem.ToString().Equals("All"))
                {

                    string ShiftList = classes.commonTask.getShiftList(ddlShiftList);
                    classes.commonTask.LoadDepartmentByCompanyAndShiftInListBox(CompanyId, ShiftList, lstAll);
                    return;
                }
                classes.commonTask.LoadDepartmentByCompanyAndShiftInListBox(CompanyId, "in (" + ddlShiftList.SelectedValue.ToString() + ")", lstAll);
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
        private void generatePF_List_Report() 
        {

            if (ddlShiftList.SelectedItem.Text.Trim() == "")
            {
                lblMessage.InnerText = "warning-> Please Select Any Shift!";
                ddlShiftList.Focus(); return;
            }
            if (lstSelected.Items.Count == 0)
            {
                lblMessage.InnerText = "warning-> Please Select Any Department!";
                lstSelected.Focus(); return;
            }          
          //  string CompanyList = "";
            string ShiftList = "";
            string DepartmentList = "";
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString(): ddlCompany.SelectedValue;
             EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId=" + rblEmpType.SelectedValue + "";
            if (ddlShiftList.SelectedItem.ToString().Equals("All"))
            ShiftList = classes.commonTask.getShiftList(ddlShiftList);
             else
               ShiftList = "in ('" + ddlShiftList.SelectedValue.ToString() + "')";
           
            DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
            Cmd = "select CompanyId,CompanyName,Address,SftId,SftName,DptName,DsgName,substring(EmpCardNo,8,15) as EmpCardNo,EmpName,FORMAT(PfDate,'dd-MM-yyyy') as PfDate,"+
                "PFAmount from v_EmployeeDetails "+
                "where CompanyId='" + CompanyId + "' and SftId " + ShiftList + " and DptId " + DepartmentList + " and PfMember=1 and EmpStatus in(1,8) " + EmpTypeID + " Order By DptCode,CustomOrdering";
          sqlDB.fillDataTable(Cmd, dt = new DataTable());
            if (dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->PF List Not Available";
                return;
            }
            Session["__PFList__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=PFList');", true);  //Open New Tab for Sever side code
        }
        private void generateBank_Cash_Report()
        {

            if (ddlShiftList.SelectedItem.Text.Trim() == "")
            {
                lblMessage.InnerText = "warning-> Please Select Any Shift!";
                ddlShiftList.Focus(); return;
            }
            if (lstSelected.Items.Count == 0)
            {
                lblMessage.InnerText = "warning-> Please Select Any Department!";
                lstSelected.Focus(); return;
            }
            //  string CompanyList = "";
            string ShiftList = "";
            string DepartmentList = "";
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
             EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId=" + rblEmpType.SelectedValue + "";
            if (ddlShiftList.SelectedItem.ToString().Equals("All"))
                ShiftList = classes.commonTask.getShiftList(ddlShiftList);
            else
                ShiftList = "in ('" + ddlShiftList.SelectedValue.ToString() + "')";

            DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
            Cmd = "select CompanyId,CompanyName,Address,SftId,SftName,DptName,DsgName,substring(EmpCardNo,8,15) as EmpCardNo,EmpName,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate," +
                "EmpAccountNo from v_EmployeeDetails " +
                "where CompanyId='" + CompanyId + "' and SftId " + ShiftList + " and DptId " + DepartmentList + " and SalaryCount='" + rblReportType.SelectedValue + "' and EmpStatus in(1,8) " + EmpTypeID + " Order by DptCode,CustomOrdering";
            sqlDB.fillDataTable(Cmd, dt = new DataTable());
            if (dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->"+rblReportType.SelectedItem.Text+" Not Available";
                return;
            }
            Session["__BankOrCash__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=BankOrCash-" + rblReportType.SelectedItem.Text.Trim() + "');", true);  //Open New Tab for Sever side code

        }
        protected void btnPreview_Click(object sender, EventArgs e)
        {
            if (rblReportType.SelectedValue == "PF")
                generatePF_List_Report();
            else generateBank_Cash_Report();
        }
    }
}