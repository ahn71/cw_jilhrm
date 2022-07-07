using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.payroll
{
    public partial class Punishment_OthersPay : System.Web.UI.Page
    {
        DataTable dt;
        string sqlCmd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
               txtAddMonth.Text= txtMonthMobileCell.Text = txtmonthname.Text = DateTime.Now.ToString("MM-yyyy");
                setPrivilege();
                ViewState["__IsChanged__"] = "no";
                Session["OPERATION_PROGRESS"] = 0;              
                if (!classes.commonTask.HasBranch())
                {
                    ddlCompanyList.Enabled = false;
                    ddlCompanyList2.Enabled = false;
                }
            }
        }


        private void setPrivilege()
        {
            try
            {

                ViewState["__WriteAction__"] = "1";
                ViewState["__DeletAction__"] = "1";
                ViewState["__ReadAction__"] = "1";
                ViewState["__UpdateAction__"] = "1";
                ViewState["__preRIndex__"] = "No";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();


                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForpayrollentrypanel(getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "payroll_entry_panel.aspx", gvpunishment, btnSave, ViewState["__CompanyId__"].ToString(), ddlCompanyList, ddlCompanyList2);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

                ddlCompanyListMobileCell.DataSource = ddlCompanyList.DataSource;
                ddlCompanyListMobileCell.DataValueField = ddlCompanyList.DataValueField;
                ddlCompanyListMobileCell.DataTextField = ddlCompanyList.DataTextField;
                ddlCompanyListMobileCell.DataBind();

                ddlCompanyList3.DataSource = ddlCompanyList.DataSource;
                ddlCompanyList3.DataValueField = ddlCompanyList.DataValueField;
                ddlCompanyList3.DataTextField = ddlCompanyList.DataTextField;
                ddlCompanyList3.DataBind();
                ddlCompanyList3.SelectedValue = ViewState["__CompanyId__"].ToString();
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                ddlCompanyList2.SelectedValue = ViewState["__CompanyId__"].ToString();
               
                classes.Employee.LoadEmpCardNoForPayrollwithEmpId(ddlEmpCardNo, ddlCompanyList.SelectedValue);
                classes.Employee.LoadEmpCardNoForPayrollwithEmpId(ddlEmpCardNo2, ddlCompanyList2.SelectedValue);
                classes.Employee.LoadEmpCardNoForPayrollwithEmpId(ddlEmpCardNoMobileCell, ddlCompanyListMobileCell.SelectedValue);
                classes.Employee.LoadEmpCardNoForPayrollwithEmpId(ddlEmpCardNo3, ddlCompanyList3.SelectedValue);
                classes.commonTask.loadAdjustmentType(ddlAddPurposeList);
                loadMobileCell();
                loadPunishment();
                loadotherpay();

            }
            catch { }

        }
        private void loadPunishment()
        {
             if (txtmonthname.Text.Trim() == "")
            {
                lblMessage.InnerText = "warning-> Please select Month.";
                txtmonthname.Focus();
                return;
            }
            DataTable dtPunishment = new DataTable();
            sqlDB.fillDataTable("Select * from v_Payroll_Punishment where CompanyId='"+ddlCompanyList.SelectedValue+ "' and MonthName='"+ txtmonthname.Text.Trim() + "'", dtPunishment);
            gvpunishment.DataSource = dtPunishment;
            gvpunishment.DataBind();
        }
        private void PrintPunishment()
        {
            if (txtmonthname.Text.Trim() == "")
            {
                lblMessage.InnerText = "warning-> Please select Month.";
                txtmonthname.Focus();
                return;
            }
            DataTable dtPunishment = new DataTable();
            sqlCmd = "Select Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo, PAmount, EmpName, CompanyName,  Address, MonthName from v_Payroll_Punishment where CompanyId='" + ddlCompanyList.SelectedValue + "' and MonthName='" + txtmonthname.Text.Trim() + "' order by convert(int, substring(EmpCardNo,10,6))";
            sqlDB.fillDataTable(sqlCmd, dtPunishment);
            if (dtPunishment==null || dtPunishment.Rows.Count==0)
            {
                lblMessage.InnerText = "warning-> No record found.";
                return;
            }
            Session["__PrintAdvance__"] = dtPunishment;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=PrintAdvance');", true);  //Open New Tab for Sever side code

        }
        private void loadotherpay()
        {
            DataTable dtotherpay = new DataTable();
            sqlDB.fillDataTable("Select * from v_Payroll_OthersPay where CompanyId='" + ddlCompanyList2.SelectedValue + "'", dtotherpay);
            gvotherspay.DataSource = dtotherpay;
            gvotherspay.DataBind();
        }
        private void loadMobileCell()
        {
            if (txtMonthMobileCell.Text.Trim() == "")
            {
                lblMessage.InnerText = "warning-> Please select Month.";
                txtMonthMobileCell.Focus();
                return;
            }
            DataTable dtMobileCell = new DataTable();
            sqlDB.fillDataTable("select McId, CompanyId,Payroll_DeductMobileCell.EmpId,EmpName, Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+" +
                "' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,format(Month,'MM-yyyy') Month ,MobileCell from Payroll_DeductMobileCell "+
                " inner join Personnel_EmployeeInfo on Payroll_DeductMobileCell.EmpId=Personnel_EmployeeInfo.EmpId where CompanyId='"+ddlCompanyListMobileCell.SelectedValue+ "' and format(Month,'MM-yyyy')='"+txtMonthMobileCell.Text.Trim()+"' " +
                " order by month(Month) desc,year(Month) desc", dtMobileCell);
            gvMobileCell.DataSource = dtMobileCell;
            gvMobileCell.DataBind();
        }
        private void PrintMobileCell()
        {
            if (txtMonthMobileCell.Text.Trim() == "")
            {
                lblMessage.InnerText = "warning-> Please select Month.";
                txtMonthMobileCell.Focus();
                return;
            }
            DataTable dtMobileCell = new DataTable();
            sqlCmd = " select Convert(varchar(3),left(EmpCardNo,LEN(EmpCardNo)-4))+' '+Convert(varchar(10),right(EmpCardNo,LEN(EmpCardNo)-9)) as EmpCardNo,EmpName,MobileCell as PAmount, format(Month,'MM-yyyy') as  MonthName, CompanyName,  Address " +
                " from Payroll_DeductMobileCell inner join Personnel_EmployeeInfo on Payroll_DeductMobileCell.EmpId = Personnel_EmployeeInfo.EmpId inner join HRD_CompanyInfo on Personnel_EmployeeInfo.CompanyId = HRD_CompanyInfo.CompanyId where Personnel_EmployeeInfo.CompanyId ='" + ddlCompanyListMobileCell.SelectedValue + "' and format(Month,'MM-yyyy')='" + txtMonthMobileCell.Text.Trim() + "' " +
                " order by convert(int, substring(EmpCardNo,10,6))";
            sqlDB.fillDataTable(sqlCmd, dtMobileCell);
            if (dtMobileCell == null || dtMobileCell.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning-> No record found.";
                return;
            }
            Session["__PrintMobileCell__"] = dtMobileCell;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=PrintMobileCell');", true);  //Open New Tab for Sever side code
        }
        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.Employee.LoadEmpCardNoForPayrollwithEmpId(ddlEmpCardNo, ddlCompanyList.SelectedValue);
            loadPunishment();

        }
        protected void ddlCompanyList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.Employee.LoadEmpCardNoForPayrollwithEmpId(ddlEmpCardNo2, ddlCompanyList2.SelectedValue);

        }
        static DataTable dt_AllowanceSettings = new DataTable();
       
       
        protected void btnSave_Click(object sender, EventArgs e)
        {

            if(btnSave.Text=="Update")
            {
                UpdatePunishment();
            }
            else
            {
                savePunishment();
            }
            
        }


        private void savePunishment()
        {
            try
            {
               
                string[] getColumns = { "CompanyId", "EmpId", "PName", "PAmount", "MonthName"};
                string[] getValues = { ddlCompanyList.SelectedValue,ddlEmpCardNo.SelectedValue, txtpunishment.Text, txtPAmount.Text,txtmonthname.Text};

                if (SQLOperation.forSaveValue("Payroll_Punishment", getColumns, getValues, sqlDB.connection) == true)
                {
                    if (ViewState["__WriteAction__"].Equals("0"))
                    {
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "Pbutton";
                    }
                    allClearPunishment();
                    loadPunishment();

                    // lblMessage.InnerText = "success->Successfully Submitted.";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow('success','Successfully Submitted.');", true);
                }
                else
                {

                    // lblMessage.InnerText = "error->Unable to Submit.";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow(error,'Unable to Submit.');", true);
                }

            }
            catch
            {
               
                //  lblMessage.InnerText = "error->Unable to Submit."; 
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow(error,'Unable to Submit.');", true);
            }

        }
        private void UpdatePunishment()
        {
            try
            {
                string[] getColumns = { "CompanyId", "EmpId", "PName", "PAmount", "MonthName" };
                string[] getValues = { ddlCompanyList.SelectedValue, ddlEmpCardNo.SelectedValue, txtpunishment.Text, txtPAmount.Text, txtmonthname.Text };

                if (SQLOperation.forUpdateValue("Payroll_Punishment", getColumns, getValues, "PSN", ViewState["__getSL__"].ToString(), sqlDB.connection) == true)
                {
                    if (ViewState["__WriteAction__"].Equals("0"))
                    {
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "Pbutton";
                    }
                    allClearPunishment();
                    loadPunishment();

                    // lblMessage.InnerText = "success->Successfully Submitted.";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow('success','Successfully Update.');", true);
                }
                else
                {

                    // lblMessage.InnerText = "error->Unable to Submit.";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow(error,'Unable to Update.');", true);
                }

            }
            catch
            {

                //  lblMessage.InnerText = "error->Unable to Submit."; 
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow(error,'Unable to Submit.');", true);
            }

        }
        protected void ddlEmpCardNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEmpCardNo.SelectedIndex != 0)
               // IndividualSalary();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }

      
        private void allClearPunishment()
        {
            txtpunishment.Text = "";
            txtPAmount.Text = "0";            
            btnSave.Text = "Submit";
        }

        private void loadSalaryInfo(string companyId)
        {
            try
            {
                SQLOperation.selectBySetCommandInDatatable("select distinct EmpCardNo,EmpName, max(SN) as SN,EmpType,EmpStatus,EmpTypeId,BasicSalary,MedicalAllownce,HouseRent ,EmpPresentSalary,CompanyId," +
                    " ActiveSalary,IsActive,PFAmount,case when SalaryCount='False' Then 'Cash' Else case when SalaryCount='True' then 'Bank' else 'Check' End End As SalaryCount from v_Personnel_EmpCurrentStatus group by EmpCardNo,EmpName,EmpTypeId,EmpType,BasicSalary,MedicalAllownce,HouseRent ," +
                    " EmpPresentSalary,EmpStatus,CompanyId,ActiveSalary,IsActive,PFAmount,SalaryCount having EmpStatus in('1','8') AND ActiveSalary='true' AND IsActive='1' AND CompanyId='" + companyId + "' order by SN ", dt = new DataTable(), sqlDB.connection);
                //gvSalaryList.DataSource = dt;
                //gvSalaryList.DataBind();
                ViewState["__IsChanged__"] = "no";
            }
            catch { }
        }

        protected void tc1_ActiveTabChanged(object sender, EventArgs e)
        {

            if (tc1.ActiveTabIndex == 1)
            {
               // txtFinding.Visible = true;
                //if (gvSalaryList.Rows.Count == 0 || ViewState["__IsChanged__"].ToString().Equals("yes")) loadSalaryInfo(ddlCompanyList2.SelectedValue);
            }
            //else txtFinding.Visible = false;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }

        protected void gvSalaryList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                    e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
                }
            }
            catch { }
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
            {


                try
                {
                    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        Button btn = (Button)e.Row.FindControl("btnEdit");
                        btn.Enabled = false;
                        btn.ForeColor = Color.Silver;
                    }

                }
                catch { }
            }
        }     
        protected void gvpunishment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName.Equals("Alter"))
                {
                    string a = ViewState["__preRIndex__"].ToString();
                    if (!ViewState["__preRIndex__"].ToString().Equals("No")) gvpunishment.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    gvpunishment.Rows[rIndex].BackColor = System.Drawing.Color.Yellow;
                    ViewState["__preRIndex__"] = rIndex;
                    setValueToControl(rIndex, gvpunishment.DataKeys[rIndex].Values[0].ToString(), gvpunishment.DataKeys[rIndex].Values[1].ToString(), gvpunishment.DataKeys[rIndex].Values[2].ToString());
                    btnSave.Text = "Update";
                    if (ViewState["__UpdateAction__"].Equals("0"))
                    {
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "Pbutton";
                    }

                }
                else if (e.CommandName.Equals("deleterow"))
                {
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    SQLOperation.forDeleteRecordByIdentifier("Payroll_Punishment", "PSN", gvpunishment.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "deleteSuccess()", true);
                    allClearPunishment();
                    lblMessage.InnerText = "success->Successfully  Deleted";
                    gvpunishment.Rows[rIndex].Visible = false;

                }
            }
            catch { }
        }
        private void setValueToControl(int rIndex, string getSL, string getCompanyId,string EmpId)
        {
            try
            {
                ViewState["__getSL__"] = getSL;
               ddlCompanyList.SelectedValue = getCompanyId;
               ddlEmpCardNo.SelectedValue = EmpId;
              // txtpunishment.Text = gvpunishment.Rows[rIndex].Cells[3].Text;
               txtPAmount.Text = gvpunishment.Rows[rIndex].Cells[3].Text;
               txtmonthname.Text = gvpunishment.Rows[rIndex].Cells[4].Text;
              


            }
            catch { }
        }

        protected void btnSave2_Click(object sender, EventArgs e)
        {
            if (btnSave2.Text == "Update")
            {
                Updateotherpay();
            }
            else
            {
                saveotherpay();
            }
        }
        private void saveotherpay()
        {
            try
            {
                string active = "";
                if(checkActive.Checked)
                {
                    active = "1";
                }
                else
                {
                    active = "0";
                }
                string[] getColumns = { "CompanyId", "EmpId", "OPpurpose", "OtherPay", "IsActive" };
                string[] getValues = { ddlCompanyList2.SelectedValue, ddlEmpCardNo2.SelectedValue, txtpurpose.Text, txtotherpayAmount.Text, active };

                if (SQLOperation.forSaveValue("Payroll_OthersPay", getColumns, getValues, sqlDB.connection) == true)
                {
                    if (ViewState["__WriteAction__"].Equals("0"))
                    {
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "Pbutton";
                    }
                    allClearOtherpay();
                    loadotherpay();

                    // lblMessage.InnerText = "success->Successfully Submitted.";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow('success','Successfully Submitted.');", true);
                }
                else
                {

                    // lblMessage.InnerText = "error->Unable to Submit.";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow(error,'Unable to Submit.');", true);
                }

            }
            catch
            {

                //  lblMessage.InnerText = "error->Unable to Submit."; 
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow(error,'Unable to Submit.');", true);
            }

        }
        private void Updateotherpay()
        {
            try
            {
                string active = "";
                if (checkActive.Checked)
                {
                    active = "1";
                }
                else
                {
                    active = "0";
                }
                string[] getColumns = { "CompanyId", "EmpId", "OPpurpose", "OtherPay", "IsActive" };
                string[] getValues = { ddlCompanyList2.SelectedValue, ddlEmpCardNo2.SelectedValue, txtpurpose.Text, txtotherpayAmount.Text, active };

                if (SQLOperation.forUpdateValue("Payroll_OthersPay", getColumns, getValues, "OPSN", ViewState["__getSL__"].ToString(), sqlDB.connection) == true)
                {
                    if (ViewState["__WriteAction__"].Equals("0"))
                    {
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "Pbutton";
                    }
                    allClearOtherpay();
                    loadotherpay();

                    // lblMessage.InnerText = "success->Successfully Submitted.";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow('success','Successfully Update.');", true);
                }
                else
                {

                    // lblMessage.InnerText = "error->Unable to Submit.";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow(error,'Unable to Update.');", true);
                }

            }
            catch
            {

                //  lblMessage.InnerText = "error->Unable to Submit."; 
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow(error,'Unable to Submit.');", true);
            }

        }
        private void allClearOtherpay()
        {
            txtpurpose.Text = "";
            txtotherpayAmount.Text = "";
            checkActive.Checked = true;
            btnSave2.Text = "Submit";
        }
        private void allClearMobileCell()
        {
            txtAmountMobileCell.Text = "0";
            btnSaveMobileCell.Text = "Submit";
        }


        protected void gvotherspay_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                if (e.CommandName.Equals("Alter"))
                {
                    string a = ViewState["__preRIndex__"].ToString();
                    if (!ViewState["__preRIndex__"].ToString().Equals("No")) gvotherspay.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    gvotherspay.Rows[rIndex].BackColor = System.Drawing.Color.Yellow;
                    ViewState["__preRIndex__"] = rIndex;
                    setValueToControl2(rIndex, gvotherspay.DataKeys[rIndex].Values[0].ToString(), gvotherspay.DataKeys[rIndex].Values[1].ToString(), gvpunishment.DataKeys[rIndex].Values[2].ToString());
                    btnSave2.Text = "Update";
                    if (ViewState["__UpdateAction__"].Equals("0"))
                    {
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "Pbutton";
                    }

                }
                else if (e.CommandName.Equals("deleterow"))
                {
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    SQLOperation.forDeleteRecordByIdentifier("Payroll_OthersPay", "OPSN", gvotherspay.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "deleteSuccess()", true);
                    allClearPunishment();
                    lblMessage.InnerText = "success->Successfully  Deleted";
                    gvotherspay.Rows[rIndex].Visible = false;

                }
            }
            catch { }
        }
        private void setValueToControl2(int rIndex, string getSL, string getCompanyId, string EmpId)
        {
            try
            {
                ViewState["__getSL__"] = getSL;
                ddlCompanyList2.SelectedValue = getCompanyId;
                ddlEmpCardNo2.SelectedValue = EmpId;
                txtpurpose.Text = gvotherspay.Rows[rIndex].Cells[3].Text;
                txtotherpayAmount.Text= gvotherspay.Rows[rIndex].Cells[4].Text;
                 if(gvotherspay.Rows[rIndex].Cells[5].Text=="Yes")
                 {
                     checkActive.Checked = true;
                 }
                 else
                 {
                     checkActive.Checked = false;
                 }
            }
            catch { }
        }
        
        private void setValueToControlMobileCell(int rIndex, string getMcId, string getCompanyId, string EmpId)
        {
            try
            {
                ViewState["__getMcId__"] = getMcId;
                ddlCompanyListMobileCell.SelectedValue = getCompanyId;
                ddlEmpCardNoMobileCell.SelectedValue = EmpId;
                txtAmountMobileCell.Text= gvMobileCell.Rows[rIndex].Cells[2].Text;
                txtMonthMobileCell.Text = gvMobileCell.Rows[rIndex].Cells[3].Text;
               
            }
            catch { }
        }
        protected void btnSaveMobileCell_Click(object sender, EventArgs e)
        {
                SaveMobileCell();
          
        }
        private void SaveMobileCell()
        {
            try
            {
                string[] Month = txtMonthMobileCell.Text.Trim().Split('-');
                //----------delete existing record------------
                SqlCommand cmd = new SqlCommand("delete Payroll_DeductMobileCell where EmpId='" + ddlEmpCardNoMobileCell.SelectedValue + "' and Month='" + Month[1] + "-" + Month[0] + "-01'",sqlDB.connection);
                cmd.ExecuteNonQuery();
                //--------------------------------------
                
                string[] getColumns = { "EmpId", "MobileCell", "Month" };
                string[] getValues = { ddlEmpCardNoMobileCell.SelectedValue, txtAmountMobileCell.Text.Trim(), Month[1] + "-" + Month[0] + "-01" };

                if (SQLOperation.forSaveValue("Payroll_DeductMobileCell", getColumns, getValues, sqlDB.connection) == true)
                {
                    if (ViewState["__WriteAction__"].Equals("0"))
                    {
                        btnSaveMobileCell.Enabled = false;
                        btnSaveMobileCell.CssClass = "";
                    }
                    else
                    {
                        btnSaveMobileCell.Enabled = true;
                        btnSaveMobileCell.CssClass = "Pbutton";
                    }
                    
                    loadMobileCell();                    
                    if(btnSaveMobileCell.Text=="Update")
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow('success','Successfully Updated.');", true);
                    else
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow('success','Successfully Submitted.');", true);
                    allClearMobileCell();
                }
                else
                {

                    // lblMessage.InnerText = "error->Unable to Submit.";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow(error,'Unable to Submit.');", true);
                }

            }
            catch
            {

                //  lblMessage.InnerText = "error->Unable to Submit."; 
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Messageshow(error,'Unable to Submit.');", true);
            }

        }
       

        protected void ddlCompanyListMobileCell_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.Employee.LoadEmpCardNoForPayrollwithEmpId(ddlEmpCardNoMobileCell, ddlCompanyListMobileCell.SelectedValue);
             loadMobileCell();
        }

        protected void gvMobileCell_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {

                if (e.CommandName.Equals("Alter"))
                {
                    string a = ViewState["__preRIndex__"].ToString();
                    if (!ViewState["__preRIndex__"].ToString().Equals("No")) gvMobileCell.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    gvMobileCell.Rows[rIndex].BackColor = System.Drawing.Color.Yellow;
                    ViewState["__preRIndex__"] = rIndex;
                    setValueToControlMobileCell(rIndex, gvMobileCell.DataKeys[rIndex].Values[0].ToString(), gvMobileCell.DataKeys[rIndex].Values[1].ToString(), gvMobileCell.DataKeys[rIndex].Values[2].ToString());
                    btnSaveMobileCell.Text = "Update";
                    if (ViewState["__UpdateAction__"].Equals("0"))
                    {
                        btnSaveMobileCell.Enabled = false;
                        btnSaveMobileCell.CssClass = "";
                    }
                    else
                    {
                        btnSaveMobileCell.Enabled = true;
                        btnSaveMobileCell.CssClass = "Pbutton";
                    }

                }
                else if (e.CommandName.Equals("deleterow"))
                {
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    SQLOperation.forDeleteRecordByIdentifier("Payroll_DeductMobileCell", "McId", gvMobileCell.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load()", true);
                    allClearMobileCell();
                    lblMessage.InnerText = "success->Successfully  Deleted";
                    gvMobileCell.Rows[rIndex].Visible = false;

                }
            }
            catch { }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            PrintPunishment();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            loadPunishment();
        }

        protected void btnSearchMobileCell_Click(object sender, EventArgs e)
        {
            loadMobileCell();
        }

        protected void btnPrintMobileCell_Click(object sender, EventArgs e)
        {
            PrintMobileCell();
        }

        protected void btnSave3_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load()", true);
            if (btnSave3.Text.Trim() == "Update")
                updateAdjustment();
            else
                saveAdjustment();
        }
        protected void ddlEmpCardNo3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load()", true);
            DataTable dtSalaryInfo = new DataTable();
            dtSalaryInfo = Employee.loadEmployeeSalary(ddlEmpCardNo3.SelectedValue);
            if (dtSalaryInfo != null && dtSalaryInfo.Rows.Count > 0)
            {
                txtBasic.Text =Math.Round((float.Parse(dtSalaryInfo.Rows[0]["BasicSalary"].ToString())/30),2).ToString();
            }
            else
            {
                txtBasic.Text = "0";
            }
            txtDays.Text = "";
            txtAddAmount.Text = "0";
            loadAdjustment();
        }

        protected void ddlAddPurposeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load()", true);
            if (ddlAddPurposeList.SelectedValue == "1")
            {
                txtBasic.Visible = true;
                txtDays.Visible = true;
                txtDays.Text = "";
                txtAddAmount.Text = "0";
            }
            else
            {
                txtBasic.Visible = false;
                txtDays.Visible = false;
            }
        }

        protected void txtDays_TextChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load()", true);
            if (txtDays.Text.Trim().Length == 0)
            {
                txtDays.Text = "0";
            }
            txtAddAmount.Text =Math.Round(float.Parse(txtBasic.Text.Trim()) * float.Parse(txtDays.Text.Trim()),2).ToString();
        }
        private void saveAdjustment()
        {
            try {
                sqlCmd= @"INSERT INTO [dbo].[Payroll_MonthlyAdjustment]
           ([CompanyID]
           ,[EmpID]
           ,[Month]
           ,[Amount]
           ,[AdjustmentTypeID],[Remarks])
     VALUES
           ('" + ddlCompanyList3.SelectedValue+"','"+ddlEmpCardNo3.SelectedValue+"','"+commonTask.ddMMyyyyToyyyyMMdd("01-"+txtAddMonth.Text.Trim())+"','"+txtAddAmount.Text.Trim()+"',"+ddlAddPurposeList.SelectedValue+",'"+ txtAddRemarks.Text.Trim() + "')";
                if (CRUD.Execute(sqlCmd, sqlDB.connection))
                {
                    lblMessage.InnerText = "success->Successfully  Saved.";
                    loadAdjustment();
                    allClearAdjustment();
                }
                else
                    lblMessage.InnerText = "error->Unable to Save.";
            }           
            catch(Exception ex) { }
        }
        private void updateAdjustment()
        {
            try
            {
                sqlCmd = @"Update Payroll_MonthlyAdjustment set [Month]='" + commonTask.ddMMyyyyToyyyyMMdd("01-" + txtAddMonth.Text.Trim()) + "',[Amount]='" + txtAddAmount.Text.Trim() + "',[AdjustmentTypeID]=" + ddlAddPurposeList.SelectedValue + ",[Remarks]='" + txtAddRemarks.Text.Trim() + "' Where SL="+ViewState["__SL__"].ToString();    
                if (CRUD.Execute(sqlCmd, sqlDB.connection))
                {
                    lblMessage.InnerText = "success->Successfully  Updated.";
                    loadAdjustment();
                    allClearAdjustment();
                }
                else
                    lblMessage.InnerText = "error->Unable to Update.";
            }
            catch (Exception ex) { }
        }
        private void loadAdjustment()
        {
            sqlCmd = "SELECT SL,CompanyID, EmpID, format(Month, 'MM-yyyy') as Month,Amount,pma.AdjustmentTypeID,pat.AdjustmentType,Remarks FROM Payroll_MonthlyAdjustment pma inner join  Payroll_AdjustmentType pat on pma.AdjustmentTypeID = pat.AdjustmentTypeID where EmpID='" + ddlEmpCardNo3.SelectedValue+"' order by convert(varchar, Month, 120) desc";
            dt = new DataTable();
            sqlDB.fillDataTable(sqlCmd, dt);
            gvAdjustment.DataSource = dt;
            gvAdjustment.DataBind();


        }

        protected void gvAdjustment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Alter"))
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load()", true);
                    string a = ViewState["__preRIndex__"].ToString();
                    if (!ViewState["__preRIndex__"].ToString().Equals("No")) gvAdjustment.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    gvAdjustment.Rows[rIndex].BackColor = System.Drawing.Color.Yellow;
                    ViewState["__preRIndex__"] = rIndex;
                    ViewState["__SL__"]= gvAdjustment.DataKeys[rIndex].Values[0].ToString();
                    ddlCompanyList3.SelectedValue= gvAdjustment.DataKeys[rIndex].Values[1].ToString();
                    ddlEmpCardNo3.SelectedValue = gvAdjustment.DataKeys[rIndex].Values[2].ToString();
                    ddlAddPurposeList.SelectedValue = gvAdjustment.DataKeys[rIndex].Values[3].ToString();
                    if (ddlAddPurposeList.SelectedValue == "1")
                    {
                        Employee.loadEmployeeSalary(ddlEmpCardNo3.SelectedValue);
                        txtDays.Visible = true;
                        txtBasic.Visible = true;
                    }
                    else
                    {
                        txtDays.Visible = false;
                        txtBasic.Visible = false;
                    }
                    txtAddMonth.Text = gvAdjustment.Rows[rIndex].Cells[0].Text;
                    txtAddAmount.Text = gvAdjustment.Rows[rIndex].Cells[2].Text;
                    txtAddRemarks.Text = gvAdjustment.Rows[rIndex].Cells[3].Text.Replace("&nbsp;","");
                    btnSave3.Text = "Update";
                    if (ViewState["__UpdateAction__"].Equals("0"))
                    {
                        btnSave3.Enabled = false;
                        btnSave3.CssClass = "";
                    }
                    else
                    {
                        btnSave3.Enabled = true;
                        btnSave3.CssClass = "Pbutton";
                    }

                }
                else if (e.CommandName.Equals("deleterow"))
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load()", true);
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    SQLOperation.forDeleteRecordByIdentifier("Payroll_MonthlyAdjustment", "SL", gvAdjustment.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "deleteSuccess()", true);                   
                    lblMessage.InnerText = "success->Successfully  Deleted";
                    gvAdjustment.Rows[rIndex].Visible = false;
                    allClearAdjustment();

                }
            }
            catch { }
        }
        private void allClearAdjustment()
        {
            ViewState["__SL__"] = "";
            btnSave3.Text = "Submit";
            txtAddAmount.Text = "0";
            txtAddRemarks.Text = "";
            txtDays.Text = "";
        }
    }
}