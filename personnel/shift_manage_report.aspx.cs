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
    public partial class shift_manage_report : System.Web.UI.Page
    {
        DataTable dt;
        DataTable dtSetPrivilege;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";

            if (!IsPostBack)
            {
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;  
            }
        }
        private void setPrivilege()
        {
            try
            {

                ViewState["__ReadAction__"] = "1";

                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string getUserId = getCookies["__getUserId__"].ToString();
                string cmpID = ViewState["__CompanyId__"].ToString();
                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                {
                    dtSetPrivilege = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='shift_manage_report.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);
                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                        {
                            btnPrint.CssClass = "css_btn Ptbut"; btnPrint.Enabled = true;
                            btnSearch.CssClass = "css_btn Ptbut"; btnSearch.Enabled = true;
                        }
                        else
                        {
                         // tblGenerateType.Visible = false;
                            WarningMessage.Visible = true;
                            btnPrint.Enabled = false;
                            btnPrint.CssClass = "";
                            btnSearch.CssClass = ""; btnSearch.Enabled = false;
                        }

                    }
                    else
                    {
                      //tblGenerateType.Visible = false;
                        WarningMessage.Visible = true;
                        btnPrint.Enabled = false;
                        btnPrint.CssClass = "";
                        btnSearch.CssClass = ""; btnSearch.Enabled = false;
                    }
                }
                classes.commonTask.LoadBranch(ddlCompanyList);
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ddlCompanyList.SelectedValue);     
            }
            catch { }

        }
        private void loadRecentlyAssiendTopTenShift() // Load Top 10 Assined Shift By Department  
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("select distinct  top 10 STId,' ( '+Format(TFromdate,'dd-MM-yyyy')+' '+Format(TToDate,'dd-MM-yyyy')+' ) '+SftName  +' | '+ GName as Title from v_ShiftTransferInfoDetails  where  CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "' AND DptId='" + ddlDepartmentList.SelectedValue.ToString() + "' ", dt);
                ddlShift.DataTextField = "Title";
                ddlShift.DataValueField = "STId";
                ddlShift.DataSource = dt;
                ddlShift.DataBind();
                ddlShift.Items.Insert(0, new ListItem(" ", "0"));
                addAllTextInShift();
            }
            catch { }
        }
        private void loadRecentloyAssinedAllShift() // Load All Assined Shift By Department 
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("select distinct STId,' ( '+Format(TFromdate,'dd-MM-yyyy')+' '+Format(TToDate,'dd-MM-yyyy')+' ) '+SftName +' | '+ GName as Title from v_ShiftTransferInfoDetails  where  CompanyId='" + ddlCompanyList.SelectedValue.ToString() + "' AND DptId='" + ddlDepartmentList.SelectedValue.ToString() + "' ", dt);
                ddlShift.DataTextField = "Title";
                ddlShift.DataValueField = "STId";
                ddlShift.DataSource = dt;
                ddlShift.DataBind();
                ddlShift.Items.Insert(0, new ListItem(" ", "0"));
                addAllTextInShift();
            }
            catch { }
        }
        private void addAllTextInShift() // For All Inser in Shift List
        {
            if (ddlShift.Items.Count > 2)
                ddlShift.Items.Insert(1, new ListItem("All", "00"));
        }
        protected void ddlDepartmentList_SelectedIndexChanged(object sender, EventArgs e)
        {           
            loadRecentlyAssiendTopTenShift();
        }

        protected void chkAllAssigned_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllAssigned.Checked == true)
                loadRecentloyAssinedAllShift();
            else loadRecentlyAssiendTopTenShift();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //--------------------Validation-------------------
            if (ddlDepartmentList.SelectedValue == "0") { lblMessage.InnerText = "warning-> Please Select Any Shift!"; ddlDepartmentList.Focus(); return; }
            if (ddlShift.SelectedValue =="0") { lblMessage.InnerText = "warning-> Please Select Any Department!"; ddlShift.Focus(); return; }
            //----------------------------------------------------
            dt = new DataTable();
            dt=GenerateRepotData();
            Session["__ShiftTarnsferReport__"] = dt;
            if (dt==null || dt.Rows.Count < 1)
            {
                lblMessage.InnerText = "warning-> Any record are not founded";
                return;
            } 
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=ShiftTarnsferReport-"+ddlCompanyList.SelectedValue+"');", true);  //Open New Tab for Sever side code
     
          
        }
        private DataTable GenerateRepotData()
        {
            try
            {
                string ShiftList = "";
                ShiftList = (ddlShift.SelectedValue != "00") ?"in('"+ddlShift.SelectedValue+"')" : classes.commonTask.getShiftList(ddlShift);
                if (txtDate.Text != "") // for Speacific Date            
                    sqlDB.fillDataTable("select SftName+' ( '+Format(TFromdate,'dd-MM-yyyy')+' '+Format(TToDate,'dd-MM-yyyy')+' ) ' as SftName,SftId,EmpId,EmpName,substring(EmpCardNo,8,12)as EmpCardNo,FName,DptName,Format(SDate,'dd-MM-yyyy')as SDate,Day,EmpType,DsgName,IsWeekend from v_ShiftTransferInfoDetails where CompanyId='" + ddlCompanyList.SelectedValue + "' and STId " + ShiftList + " and DptId='" + ddlDepartmentList.SelectedValue + "' AND Format(SDate,'dd-MM-yyyy')='" + txtDate.Text.Trim() + "' ORDER BY SftName, DptName, EntryDate,EmpCardNo ", dt);
                else sqlDB.fillDataTable("select SftName+' ( '+Format(TFromdate,'dd-MM-yyyy')+' '+Format(TToDate,'dd-MM-yyyy')+' ) ' as SftName,SftId,EmpId,EmpName,substring(EmpCardNo,8,12)as EmpCardNo,FName,DptName,Format(SDate,'dd-MM-yyyy')as SDate,Day,EmpType,DsgName,IsWeekend from v_ShiftTransferInfoDetails where  CompanyId='" + ddlCompanyList.SelectedValue + "' and STId " + ShiftList + " and DptId='" + ddlDepartmentList.SelectedValue + "' ORDER BY EntryDate, DptName, SDate,EmpCardNo ", dt);
                   return dt;              
                   
            }
            catch { return dt = null; }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //--------------------Validation-------------------
            if (ddlDepartmentList.SelectedValue == "0") { lblMessage.InnerText = "warning-> Please Select Any Shift!"; ddlDepartmentList.Focus(); return; }
            if (ddlShift.SelectedValue == "0") { lblMessage.InnerText = "warning-> Please Select Any Department!"; ddlShift.Focus(); return; }
            //----------------------------------------------------
            dt = new DataTable();
            dt = GenerateRepotData();
            if (dt == null || dt.Rows.Count < 1)
            {
                lblMessage.InnerText = "warning-> Any record are not founded";
                return;
            }
            gvEmpList.DataSource = dt;
            gvEmpList.DataBind();
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.commonTask.loadDepartmentListByCompany(ddlDepartmentList, ddlCompanyList.SelectedValue); 
        }
      
    }
}