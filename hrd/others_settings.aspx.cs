using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using SigmaERP.classes;

namespace SigmaERP.hrd
{
    public partial class others_settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
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
                ViewState["__preRIndex__"] = "No";
                string getUserId = getCookies["__getUserId__"].ToString();

                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "others_settings.aspx", ddlCompanyName, gvOthersList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

                loadAllowanceType();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
                //  dlExistsUser.Enabled = false;
            }


            catch { }

        }
        

        private void loadHRD_OthersSetting()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select * from HRD_OthersSetting ", dt);
                //gvOthersSettings.DataSource = dt;
                //gvOthersSettings.DataBind();
            }
            catch { }

        }

        private void saveOthersSettings()
        {
            try
            {
                string[] getColumns = { "WorkerTiffinHour", "WorkerTiffinMin", "StaffTiffinHour", "StaffTiffinMin", "StaffHolidayTotalHour", "StaffHolidayTotalMin", "WorkerTiffinTaka", "StaffTiffinTaka", "StaffHolidayCount", "CompanyId", "MinWorkingHour", "MinWorkingMin", "MinOverTimeHour", "MinOverTimeMin" };
                string[] getValues = { ddlWTiffinHour.SelectedValue, ddlWTiffinMin.SelectedValue, ddlStaffTiffinHour.SelectedValue, ddlStaffTiffinMin.SelectedValue, ddlStaffHolidayHour.SelectedValue, ddlStaffHolidayMin.SelectedValue,txtWorkerTiffinTaka.Text, txtStaffTiffinTaka.Text,bool.Parse(ckbStaffHolidayCout.Checked.ToString()).ToString(), ddlCompanyName.SelectedValue,ddlMinimumWorkingHour.SelectedValue ,ddlMinimumWorkingMin.SelectedValue,ddlMinimumOverTimeHour.SelectedValue,ddlMinimumOverTimeMin.SelectedValue };
                if (SQLOperation.forSaveValue("HRD_OthersSetting", getColumns, getValues, sqlDB.connection) == true)
                {
                    AllClear();
                    lblMessage.InnerText = "success->Successfully Saved";
                    loadAllowanceType();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateOthersSettings()
        {
            try
            {
                string[] getColumns = { "WorkerTiffinHour", "WorkerTiffinMin", "StaffTiffinHour", "StaffTiffinMin", "StaffHolidayTotalHour", "StaffHolidayTotalMin", "WorkerTiffinTaka", "StaffTiffinTaka", "StaffHolidayCount", "MinWorkingHour", "MinWorkingMin", "MinOverTimeHour", "MinOverTimeMin" };
                string[] getValues = { ddlWTiffinHour.SelectedValue, ddlWTiffinMin.SelectedValue, ddlStaffTiffinHour.SelectedValue, ddlStaffTiffinMin.SelectedValue, ddlStaffHolidayHour.SelectedValue, ddlStaffHolidayMin.SelectedValue, txtWorkerTiffinTaka.Text, txtStaffTiffinTaka.Text, bool.Parse(ckbStaffHolidayCout.Checked.ToString()).ToString(),  ddlMinimumWorkingHour.SelectedValue, ddlMinimumWorkingMin.SelectedValue, ddlMinimumOverTimeHour.SelectedValue, ddlMinimumOverTimeMin.SelectedValue };
                if (SQLOperation.forUpdateValue("HRD_OthersSetting", getColumns, getValues, "SL", ViewState["__SL__"].ToString(), sqlDB.connection) == true)
                {
                    AllClear();
                    lblMessage.InnerText = "success->Successfully Update";
                    loadAllowanceType();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlCompanyName.SelectedIndex < 1)
            {
                lblMessage.InnerText = "warning-> Please,select any company!"; ddlCompanyName.Focus(); return;
            }
            if(btnSave.Text.Trim().Equals("Save"))
            {
                deleteSettings();
                saveOthersSettings();
            }
           else
            UpdateOthersSettings();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
        }

        private void deleteSettings()
        {
            try
            {
                
                SQLOperation.forDeleteRecordByIdentifier("HRD_OthersSetting", "CompanyId",ddlCompanyName.SelectedValue, sqlDB.connection);
            }
            catch { }
        
        }
        private void loadAllowanceType()
        {
            try
            {
                string CompanyId = (ddlCompanyName.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue;
                DataTable dt = new DataTable();
                SQLOperation.selectBySetCommandInDatatable(" select * from HRD_OthersSetting where SL= (select MAX(SL) from HRD_OthersSetting where CompanyId='" + CompanyId + "') ", dt = new DataTable(), sqlDB.connection);
                //int totalRows = dt.Rows.Count;
                //string divInfo = "";  
                //if (totalRows == 0)
                //{
                //    divInfo = "<div class='noData'></div>";
                //    divInfo += "<div class='dataTables_wrapper'>Any Allowance Not assigned<div class='head'></div></div>";
                //    divOthersSettings.Controls.Add(new LiteralControl(divInfo));
                //    return;
                //}
                gvOthersList.DataSource = dt;
                gvOthersList.DataBind();
             
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
            }


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("Select EmpId,EmpStatus from Personnel_EmpCurrentStatus", dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SqlCommand cmd = new SqlCommand("Update Personnel_EmployeeInfo set EmpStatus=" + dt.Rows[i]["EmpStatus"].ToString() + " where EmpId='" + dt.Rows[i]["EmpId"].ToString() + "' ", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
        }

        protected void gvOthersList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
            ViewState["__SL__"] = gvOthersList.DataKeys[rIndex].Values[0].ToString();
            if (e.CommandName == "Alter")
            { 
            //   ddlWTiffinHour.SelectedValue= gvOthersList.Rows[rIndex].Cells[0].Text.ToString();
            //   ddlWTiffinMin.SelectedValue= gvOthersList.Rows[rIndex].Cells[1].Text.ToString();
            //   ddlStaffTiffinHour.SelectedValue= gvOthersList.Rows[rIndex].Cells[2].Text.ToString();
            //   ddlStaffTiffinMin.SelectedValue= gvOthersList.Rows[rIndex].Cells[3].Text.ToString();
            //   ddlWNightBillHour.SelectedValue= gvOthersList.Rows[rIndex].Cells[4].Text.ToString();
            //   ddlWorkerNightBillMin.SelectedValue= gvOthersList.Rows[rIndex].Cells[5].Text.ToString();
            //   ddlStaffNightBillHour.SelectedValue= gvOthersList.Rows[rIndex].Cells[6].Text.ToString();
            //   ddlStaffNightBillMin.SelectedValue= gvOthersList.Rows[rIndex].Cells[7].Text.ToString();
            //   ddlStaffHolidayHour.SelectedValue= gvOthersList.Rows[rIndex].Cells[8].Text.ToString();
            //   ddlStaffHolidayMin.SelectedValue= gvOthersList.Rows[rIndex].Cells[9].Text.ToString();
            //   ddlOTList.SelectedValue= gvOthersList.Rows[rIndex].Cells[10].Text.ToString();
                Alter();
               btnSave.Text = "Update";
            }          
        }

        private void Alter()
        {
            DataTable dt = new DataTable();
            SQLOperation.selectBySetCommandInDatatable(" select * from HRD_OthersSetting where SL=" + ViewState["__SL__"].ToString()+ " ", dt = new DataTable(), sqlDB.connection);
            ddlCompanyName.SelectedValue = dt.Rows[0]["CompanyId"].ToString();
            ddlWTiffinHour.SelectedValue = dt.Rows[0]["WorkerTiffinHour"].ToString();
            ddlWTiffinMin.SelectedValue = dt.Rows[0]["WorkerTiffinMin"].ToString();
            ddlStaffTiffinHour.SelectedValue = dt.Rows[0]["StaffTiffinHour"].ToString();
            ddlStaffTiffinMin.SelectedValue = dt.Rows[0]["StaffTiffinMin"].ToString();
            ddlStaffHolidayHour.SelectedValue = dt.Rows[0]["StaffHolidayTotalHour"].ToString();
            ddlStaffHolidayMin.SelectedValue = dt.Rows[0]["StaffHolidayTotalMin"].ToString();
            txtWorkerTiffinTaka.Text = dt.Rows[0]["WorkerTiffinTaka"].ToString();
            txtStaffTiffinTaka.Text = dt.Rows[0]["StaffTiffinTaka"].ToString();
            ckbStaffHolidayCout.Checked = bool.Parse(dt.Rows[0]["StaffHolidayCount"].ToString());
            ddlMinimumWorkingHour.SelectedValue = dt.Rows[0]["MinWorkingHour"].ToString();
            ddlMinimumWorkingMin.SelectedValue = dt.Rows[0]["MinWorkingMin"].ToString();
            ddlMinimumOverTimeHour.SelectedValue = dt.Rows[0]["MinOverTimeHour"].ToString();
            ddlMinimumOverTimeMin.SelectedValue = dt.Rows[0]["MinOverTimeMin"].ToString();

        }
        private void AllClear()
        {
            ddlWTiffinHour.SelectedValue ="00";
            ddlWTiffinMin.SelectedValue = "00";
            ddlStaffTiffinHour.SelectedValue = "00";
            ddlStaffTiffinMin.SelectedValue = "00";
            ddlWNightBillHour.SelectedValue = "00";
            ddlWorkerNightBillMin.SelectedValue = "00";
            ddlStaffNightBillHour.SelectedValue = "00";
            ddlStaffNightBillMin.SelectedValue = "00";
            ddlStaffHolidayHour.SelectedValue = "00";
            ddlStaffHolidayMin.SelectedValue = "00";
            ddlOTList.SelectedValue = "00";
            btnSave.Text = "Save";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            AllClear();
        }

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadAllowanceType();
        }
    }
}