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
    public partial class late_deduction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                loadLateDeduction();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
        }
        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {


                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                // below part for supper admin and master admin.there must be controll everythings.remember that super admin not seen another super admin information
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
                {

                    classes.commonTask.LoadBranch(ddlCompanyList);
                    return;
                }
                else    // below part for admin and viewer.while admin just write info and viewer just see information.its for by default settings
                {

                    classes.commonTask.LoadBranch(ddlCompanyList, ViewState["__UserType__"].ToString());

                   

                    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                    {
                       
                        divElementContainer.EnableViewState = false;
                    }

                    //  here set privilege by setting master admin or supper admin 
                    dtSetPrivilege = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='for_approve_leave_list.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dtSetPrivilege);

                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                        {
                           
                            divElementContainer.EnableViewState = true;
                        }


                    }


                }

            }
            catch { }
        }
        private void loadLateDeduction()
        {
            try
            {
                string CompanyId = (ddlCompanyList.SelectedValue.ToString()=="0000") ? ViewState["__CompanyId__"].ToString(): ddlCompanyList.SelectedValue.ToString();
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select distinct * from v_Attendance_LateDeduction where CompanyId='"+CompanyId+"' order by LDId",dt);
                gvLateDeductionTypeList.DataSource = dt;
                gvLateDeductionTypeList.DataBind();
            }
            catch { }
        }
        private void saveLateDeduction()
        {
            try
            {
                string CompanyId = (ddlCompanyList.SelectedValue.ToString()=="0000") ? ViewState["__CompanyId__"].ToString(): ddlCompanyList.SelectedValue.ToString();
                string[] getColumns = { "CompanyId", "LeaveName", "LateDays", "IsDeduction", "NoDeductionDays", "Entrydate", "Notes" };
                string[] getValues = {CompanyId,ddlLeaveTypes.SelectedItem.ToString(),txtLateDays.Text,"1",txtNoOfDeductionDays.Text,convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy")).ToString(),txtNotes.Text.Trim()};
                if (SQLOperation.forSaveValue("Attendance_LateDeduction", getColumns, getValues,sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success->Successfully Saved";
                    clear();
                    loadLateDeduction();
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private void updateLateDeduction()
        {
            try
            {
                string[] getColumns = { "CompanyId", "LeaveName", "LateDays", "IsDeduction", "NoDeductionDays", "Entrydate", "Notes" };
                string[] getValues =  {ddlCompanyList.SelectedValue.ToString(), ddlLeaveTypes.SelectedItem.ToString(), txtLateDays.Text, "1", txtNoOfDeductionDays.Text, convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy")).ToString(), txtNotes.Text.Trim() };

                if (SQLOperation.forUpdateValue("Attendance_LateDeduction", getColumns, getValues, "LDId", gvLateDeductionTypeList.DataKeys[Convert.ToInt32(ViewState["__rIndex__"].ToString())].Values[0].ToString(), sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success->Successfully Updated";
                    clear();
                    loadLateDeduction();
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->"+ex.Message;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (InputValidationBascat())
            {
                if (btnSave.Text.Equals("Save")) saveLateDeduction();
                else updateLateDeduction();
            }
        }

        private bool InputValidationBascat()
        {
            try
            {
                if (ddlLeaveTypes.SelectedValue.ToString() == "s")
                {
                    lblMessage.InnerText = "error->Please select the a leave type";
                    ddlLeaveTypes.Focus();
                    return false;
                }
                else if (txtLateDays.Text.Trim() == "")
                {

                    lblMessage.InnerText = "error->Please type the late days ";
                    txtLateDays.Focus();
                    return false;
                }
                else if (txtNoOfDeductionDays.Text.Trim() == "")
                {
                    lblMessage.InnerText = "error->Please type the deduction days";
                    txtNoOfDeductionDays.Focus();
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        protected void gvLateDeductionTypeList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                if (e.CommandName == "Alter")
                {
                    ViewState["__rIndex__"] = rIndex.ToString();
                    setValueInControl(rIndex);
                }
                else if (e.CommandName == "Delete")
                {
                    if (SQLOperation.forDeleteRecordByIdentifier("", "LDId", gvLateDeductionTypeList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection))
                    {
                        lblMessage.InnerText = "success->Successfully Deleted";
                        clear();

                    }
                }
                
            }
            catch { }
        }

        private void setValueInControl(int rIndex)
        {
            try
            {
                ddlCompanyList.SelectedValue = gvLateDeductionTypeList.DataKeys[rIndex].Values[1].ToString();
                ddlLeaveTypes.SelectedValue= gvLateDeductionTypeList.DataKeys[rIndex].Values[3].ToString();
                txtLateDays.Text = gvLateDeductionTypeList.Rows[rIndex].Cells[1].Text.ToString();
                txtNoOfDeductionDays.Text = gvLateDeductionTypeList.Rows[rIndex].Cells[3].Text.ToString();
                txtNotes.Text = gvLateDeductionTypeList.DataKeys[rIndex].Values[2].ToString();

                btnSave.Text = "Update";
            }
            catch { }
        }

        private void clear()
        {
           
            ddlCompanyList.SelectedIndex = 0;
            ddlLeaveTypes.SelectedIndex = 0;
            txtLateDays.Text = "";
            txtNoOfDeductionDays.Text = "";
            txtNotes.Text = "";
            btnSave.Text = "Save";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            clear();
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadLateDeduction();
        }

        protected void gvLateDeductionTypeList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            lblMessage.InnerText = "";
            loadLateDeduction();
        }

        protected void gvLateDeductionTypeList_RowDataBound(object sender, GridViewRowEventArgs e)
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
        }
    }
}