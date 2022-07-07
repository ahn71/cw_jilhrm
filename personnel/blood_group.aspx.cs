using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class blood_group : System.Web.UI.Page
    {
        DataTable dt;
        string CompanyId = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
                divindivisual.Visible = false;
                classes.commonTask.LoadEmpTypeWithAll(rblEmpType);
                if (!classes.commonTask.HasBranch())
                ddlBranch.Enabled = false;
                ddlBranch.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
        }
        private void setPrivilege()
        {
            try
            {
                upSuperAdmin.Value = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();

                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "blood_group.aspx", ddlBranch, WarningMessage, tblGenerateType, btnPrintpreview);
                ViewState["__ReadAction__"] = AccessPermission[0];

            }               
            
            catch { }

        }
        
        protected void rdball_CheckedChanged(object sender, EventArgs e)
        {
            divindivisual.Visible = false;
            rdbindividual.Checked = false;
            rdbGroup.Checked = false;
            trBloodGroup.Visible = false;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true); 
           
        }

        protected void rdbindividual_CheckedChanged(object sender, EventArgs e)
        {
            rdball.Checked = false;
            rdbGroup.Checked = false;
            divindivisual.Visible = true;
            trBloodGroup.Visible = false;
            CompanyId = (ddlBranch.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue;
            classes.commonTask.LoadEmpCardNoByEmpType(ddlCardNo, CompanyId, rblEmpType.SelectedValue);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }
        protected void rdbGroup_CheckedChanged(object sender, EventArgs e)
        {
            rdball.Checked = false;
            rdbindividual.Checked = false;
            divindivisual.Visible = true;          
            divindivisual.Visible = false;
            trBloodGroup.Visible = true;
        }
        protected void rdbWorker_CheckedChanged(object sender, EventArgs e)
        {
            
            CompanyId = (ddlBranch.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue;
            LoadWorkerCardNo(ddlCardNo, CompanyId);
        }

        protected void rdbStaff_CheckedChanged(object sender, EventArgs e)
        {
            CompanyId = (ddlBranch.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue;
           
            LoadStaffCardNo(ddlCardNo, CompanyId);
        }
        private void LoadWorkerCardNo(DropDownList dl,string CompanyId)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select MAX(SN) as SN,EmpId,EmpCardNo From v_EmployeeProfile where EmpTypeId=1 and EmpStatus in ('1','8') and ActiveSalary='True' and CompanyId='" + CompanyId + "' Group by EmpId,EmpCardNo order by EmpCardNo", dt);
                dl.DataSource = dt;
                dl.DataTextField = "EmpCardNo";
                dl.DataValueField = "SN";
                dl.DataBind();
            }
            catch { }
        }
        private void LoadStaffCardNo(DropDownList dl, string CompanyId)
        {
            dt = new DataTable();
            sqlDB.fillDataTable("Select MAX(SN) as SN,EmpId,EmpCardNo From v_EmployeeDetails where EmpTypeId=2 and EmpStatus in ('1','8') and ActiveSalary='True'and CompanyId='" + CompanyId + "' Group by EmpId,EmpCardNo order by EmpCardNo", dt);
            dl.DataSource = dt;
            dl.DataTextField = "EmpCardNo";
            dl.DataValueField = "SN";
            dl.DataBind();
        }

        protected void btnPrintpreview_Click(object sender, EventArgs e)
        {
            CompanyId = (ddlBranch.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue;
            string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId=" + rblEmpType.SelectedValue + "";
                
            if (rdball.Checked == true)
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where ActiveSalary='True' and CompanyId='" + CompanyId + "' " + EmpTypeID + " Group by EmpId", dt);
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning-> Any Employees Are Not Available In This Company!"; return;
                }
                string setSn = "", setEmpId = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0 && i == dt.Rows.Count - 1)
                    {
                        setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        setEmpId = "in('" + dt.Rows[i].ItemArray[1].ToString() + "')";
                    }
                    else if (i == 0 && i != dt.Rows.Count - 1)
                    {
                        setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        setEmpId += "in ('" + dt.Rows[i].ItemArray[1].ToString() + "'";
                    }
                    else if (i != 0 && i == dt.Rows.Count - 1)
                    {
                        setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        setEmpId += ",'" + dt.Rows[i].ItemArray[1].ToString() + "')";
                    }
                    else
                    {
                        setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        setEmpId += ",'" + dt.Rows[i].ItemArray[1].ToString() + "'";
                    }
                }
                dt = new DataTable();
                sqlDB.fillDataTable("Select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DsgName,DptName,BloodGroup,CompanyName,Address From v_EmployeeDetails where SN " + setSn + " order by DptCode,CustomOrdering", dt);              
                Session["__EmployeeBloodGroup__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EmployeeBloodGroup');", true);  //Open New Tab for Sever side code
            }
            else if (rdbindividual.Checked == true)
            {
                if (ddlCardNo.SelectedIndex == -1)
                {
                    lblMessage.InnerText = "warning->"+rblEmpType.SelectedItem.Text+" Employees Are Not Available!";
                    ddlCardNo.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                    return;
                }
                if (ddlCardNo.SelectedItem.Text.Trim() == "")
                {
                    lblMessage.InnerText = "warning->Please Select Any Employee!";
                    ddlCardNo.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                    return;
                }
                dt = new DataTable();
                sqlDB.fillDataTable("Select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DsgName,DptName,BloodGroup,CompanyName,Address From v_EmployeeDetails where SN=" + ddlCardNo.SelectedValue + " and ActiveSalary='True'", dt);
                Session["__EmployeeBloodGroup__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EmployeeBloodGroup');", true);  //Open New Tab for Sever side code
            }
            else if (rdbGroup.Checked == true)
            {
                if ( dsBloodGroup.SelectedItem.Text.Trim() == "")
                {
                    lblMessage.InnerText = "warning->Please Select Blood Group!";
                    dsBloodGroup.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                    return;
                }
                dt = new DataTable();
                sqlDB.fillDataTable("Select EmpId,Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,DsgName,DptName,BloodGroup,CompanyName,Address From v_EmployeeDetails where CompanyId='" + CompanyId + "' and BloodGroup='" + dsBloodGroup.SelectedItem.Text.Trim().ToString() + "' " + EmpTypeID + " and ActiveSalary='True' order by DptCode,CustomOrdering", dt);
                if (dt.Rows.Count < 1)
                {
                    lblMessage.InnerText = "warning-> Any Employees Are Not Founded of (" + dsBloodGroup.SelectedItem.Text + ") Blood Group."; return;
                }
                Session["__EmployeeBloodGroup__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=EmployeeBloodGroup');", true);  //Open New Tab for Sever side code
            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdball.Checked == true) return;
            CompanyId = (ddlBranch.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue;
            classes.commonTask.LoadEmpCardNoByEmpType(ddlCardNo, CompanyId, rblEmpType.SelectedValue);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
          
        }
        protected void rblEmpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompanyId = (ddlBranch.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue;
            classes.commonTask.LoadEmpCardNoByEmpType(ddlCardNo, CompanyId, rblEmpType.SelectedValue);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }
    }
}