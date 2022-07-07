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

namespace SigmaERP.hrd
{
    public partial class Thanas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
                loadDictrict();
                loadAllThana();
            }
        }
        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "Thana.aspx", gvLateDeductionTypeList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

            }
            catch { }

        }
        private void loadDictrict()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                da = new SqlDataAdapter("SELECT distinct DstName, DstId FROM HRD_District", sqlDB.connection);
                da.Fill(dt);
                ddlDistrict.DataValueField = "DstId";
                ddlDistrict.DataTextField = "DstName";
                ddlDistrict.DataSource = dt;
                ddlDistrict.DataBind();

                ddlDistrict.Items.Insert(0, new ListItem(" ", "0000"));
            }
            catch { }
        }
        private void saveThana()
        {
            try
            {
                string[] getColumns = { "DstId", "ThaName", "ThaNameBangla" };
                string[] getValues = { ddlDistrict.SelectedValue, txtThanaName.Text.Trim(), txtThanaNameBn.Text.Trim() };
                if (SQLOperation.forSaveValue("HRDThanaInfo", getColumns, getValues, sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success-> Successfully Saved ";
                    clear();
                    loadAllThana();
                }
            }
            catch { }
        }

        private void updateThana()
        {
            try
            {
                string[] getColumns = { "DstId", "ThaName", "ThaNameBangla" };
                string[] getValues = { ddlDistrict.SelectedValue.ToString(), txtThanaName.Text.Trim(), txtThanaNameBn.Text.Trim() };

                if (SQLOperation.forUpdateValue("HRDThanaInfo", getColumns, getValues, "ThaId", ViewState["__getThaId__"].ToString(), sqlDB.connection))
                {
                    lblMessage.InnerText = "success->Successfully Updated";
                    clear();
                    loadAllThana();
                }

            }
            catch { }
        }
        private void loadAllThana()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select * from v_thanaInfo where DstId="+ddlDistrict.SelectedValue.ToString()+"", dt);

                gvLateDeductionTypeList.DataSource = dt;
                gvLateDeductionTypeList.DataBind();
            }
            catch { }

        }

        protected void gvLateDeductionTypeList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            
            int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
            ViewState["__getThaId__"] = gvLateDeductionTypeList.DataKeys[rIndex].Values[0].ToString();
            if (e.CommandName == "Alter")
            {
                ddlDistrict.SelectedValue = gvLateDeductionTypeList.DataKeys[rIndex].Values[1].ToString();
                txtThanaName.Text = gvLateDeductionTypeList.Rows[rIndex].Cells[2].Text.ToString();
                txtThanaNameBn.Text = (gvLateDeductionTypeList.Rows[rIndex].Cells[3].Text.ToString().Equals("&nbsp;")) ? "" : gvLateDeductionTypeList.Rows[rIndex].Cells[3].Text.ToString();
                if (ViewState["__UpdateAction__"].Equals("0"))
                {
                    btnSave.Enabled = false;
                    btnSave.CssClass = "";
                }
                else
                {
                    btnSave.Enabled = true;
                    btnSave.CssClass = "Rbutton";
                }
                btnSave.Text = "Update";
            }
            else if (e.CommandName == "Delete")
            {
                if (SQLOperation.forDeleteRecordByIdentifier("HRDThanaInfo", "ThaId", ViewState["__getThaId__"].ToString(),sqlDB.connection))
                {
                    lblMessage.InnerText = "success->Successfully Deleted";
                    clear();
                }
            }
        }

        private void clear()
        {
            txtThanaName.Text = "";
            txtThanaNameBn.Text = "";
            if (ViewState["__WriteAction__"].Equals("0"))
            {
                btnSave.Enabled = false;
                btnSave.CssClass = "";
            }
            else
            {
                btnSave.Enabled = true;
                btnSave.CssClass = "Rbutton";
            }
            btnSave.Text  = "Save";
          //  ddlDistrict.SelectedValue = "0000";
        }

        private bool InputValidationBascet()
        {
            try
            {
                if (ddlDistrict.SelectedValue.ToString().Equals("0000"))
                {
                    lblMessage.InnerText = "Please select the district name ";
                    return false;
                }
                else if (txtThanaName.Text.Trim() == "")
                {
                    lblMessage.InnerText = "error->Please type the thana name for selected district";
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (InputValidationBascet())
            {
                if (btnSave.Text == "Save") saveThana();
                else updateThana();
            }
        }

        protected void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadAllThana();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void gvLateDeductionTypeList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            loadAllThana();
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
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
            {
                try
                {
                    if (ViewState["__DeletAction__"].ToString().Equals("0"))
                    {
                        Button lnkDelete = (Button)e.Row.FindControl("btnDelete");
                        lnkDelete.Enabled = false;
                        lnkDelete.OnClientClick = "return false";
                        lnkDelete.ForeColor = Color.Silver;
                    }

                }
                catch { }
                try
                {
                    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        Button lnkDelete = (Button)e.Row.FindControl("btnAlter");
                        lnkDelete.Enabled = false;
                        lnkDelete.ForeColor = Color.Silver;
                    }

                }
                catch { }
            }
        }

        protected void gvLateDeductionTypeList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            loadAllThana();
            gvLateDeductionTypeList.PageIndex = e.NewPageIndex;
            gvLateDeductionTypeList.DataBind();
        }
    }
}