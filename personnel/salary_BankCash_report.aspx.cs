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
    public partial class salary_BankCash_report : System.Web.UI.Page
    {
        DataTable dt;
        string CompanyId = "";
        string Cmd = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)          
                setPrivilege();
            
               
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
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
                {
                    //For supper admin & Master admin
                    tblGenerateType.Visible = true;
                    classes.commonTask.LoadBranch(ddlBranch);
                    //ddlBranch.SelectedValue = ViewState["__CompanyId__"].ToString();
                    ViewState["__IsUserType__"] = "1";
                    return;
                }
                else
                {
                    ViewState["__IsUserType__"] = "0";
                    upSuperAdmin.Value = "0";
                    classes.commonTask.LoadBranch(ddlBranch, ViewState["__CompanyId__"].ToString());
                    DataTable dtSetPrivilege = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='salary_BankCash_report.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                    //    {
                    //        btnPrintpreview.CssClass = "";
                    //        btnPrintpreview.Enabled = false;
                    //    }
                    //}
                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                        {
                            btnPrintpreview.CssClass = "css_btn"; btnPrintpreview.Enabled = true; fsRadioBtn.Disabled = false;
                        }
                        else
                        {
                            tblGenerateType.Visible = false;
                            WarningMessage.Visible = true;
                            btnPrintpreview.CssClass = ""; btnPrintpreview.Enabled = false;
                            fsRadioBtn.Disabled = true;
                            // mainDiv.Style.Add("Pointer-event", "none");
                        }

                    }
                    else
                    {
                        tblGenerateType.Visible = false;
                        WarningMessage.Visible = true;
                        btnPrintpreview.CssClass = ""; btnPrintpreview.Enabled = false;
                        fsRadioBtn.Disabled = true;
                        // mainDiv.Style.Add("Pointer-event", "none");
                    }
                    //if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                    //else
                    //{
                    //    upSuperAdmin.Value = "0";
                    //    DataTable dt = new DataTable();
                    //    sqlDB.fillDataTable("select * from UserPrivilege where PageName='blood_group.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                    //        {
                    //            btnPrintpreview.CssClass = "";

                    //            btnPrintpreview.Enabled = false;


                    //        }
                    //    }
                    //}
                }
            }
            catch { }

        }
        protected void btnPrintpreview_Click(object sender, EventArgs e)
        {
            generatePF_List_Report();
        }
        private void generatePF_List_Report()
        {           
            //  string CompanyList = "";
            string ShiftList = "";       
            CompanyId = (ddlBranch.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue;
            Cmd = "select CompanyId,CompanyName,Address,SftId,SftName,DptName,DsgName,substring(EmpCardNo,8,15) as EmpCardNo,EmpName,FORMAT(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate," +
                "EmpAccountNo from v_EmployeeDetails " +
                "where CompanyId='" + CompanyId + "' and SalaryCount='"+rblSalaryCount.SelectedValue+"'";
            sqlDB.fillDataTable(Cmd, dt = new DataTable());
            if (dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->PF List Not Available";
                return;
            }
            Session["__BankOrCash__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=BankOrCash-"+rblSalaryCount.SelectedValue+"');", true);  //Open New Tab for Sever side code

        }


    }
}