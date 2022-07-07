using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.hrd
{
    public partial class religion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();                
                LoadReligion("");
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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "religion.aspx",  gvQualificationList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

            }
            catch { }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (btnSave.Text=="Update")
                {
                    if (UpdateLoadReligion() == true)
                    {
                        LoadReligion("");
                        AllClear();
                        lblMessage.InnerText = "success->Successfully Updated";
                    }
                }
                else
                {
                    if (SaveReligion() == true)
                    {
                        AllClear();
                        LoadReligion("");
                        lblMessage.InnerText = "success->Successfully Saved";
                    }
                }
            }
            catch
            {
                LoadReligion("");
            }
        }
        private Boolean SaveReligion()
        {
            try
            {
                string[] getColumns = { "RName", "RNameBn" };
                string[] getValues = { txtReligion.Text.Trim(), txtReligionBn.Text.Trim() };
                if (SQLOperation.forSaveValue("HRD_Religion", getColumns, getValues, sqlDB.connection) == true)
                {

                }
                return true;
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
                return false;
            }
        }
        private Boolean UpdateLoadReligion()
        {
            try
            {
                string[] getColumns = { "RName", "RNameBn" };
                string[] getValues = { txtReligion.Text.Trim(), txtReligionBn.Text.Trim() };
                string getIdentifierValue = ViewState["__RId__"].ToString();
                if (SQLOperation.forUpdateValue("HRD_Religion", getColumns, getValues, "RId", getIdentifierValue, sqlDB.connection) == true)
                {

                }
                return true;
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
                return false;
            }
        }
        private void AllClear()
        {
            hdnbtnStage.Value = "";
            hdnUpdate.Value = "";
            txtReligion.Text = "";
            txtReligionBn.Text = "";
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
            btnSave.Text = "Save";
            txtReligion.Focus();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string getIdentifierValue = hdnUpdate.Value.ToString();
                if (SQLOperation.forDeleteRecordByIdentifier("HRD_Religion", "RId", getIdentifierValue, sqlDB.connection) == true)
                {
                    LoadReligion("");
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
            }
        }
        private void LoadReligion(string sqlcmd)
        {
            if (string.IsNullOrEmpty(sqlcmd)) sqlcmd = "SELECT * FROM HRD_Religion where RId<>1";
            DataTable dt = new DataTable();
            sqlDB.fillDataTable(sqlcmd, dt);

            gvQualificationList.DataSource = dt;
            gvQualificationList.DataBind();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            AllClear();
        }

        protected void gvQualificationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
             int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
            ViewState["__RId__"] = gvQualificationList.DataKeys[rIndex].Values[0].ToString();
            if (e.CommandName == "Alter")
            {
                txtReligion.Text = gvQualificationList.Rows[rIndex].Cells[0].Text.ToString();
                txtReligionBn.Text = (gvQualificationList.Rows[rIndex].Cells[1].Text.ToString().Equals("&nbsp;")) ? "" : gvQualificationList.Rows[rIndex].Cells[1].Text.ToString();
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
                if (deleteValidation(ViewState["__RId__"].ToString()))
                {
                if (SQLOperation.forDeleteRecordByIdentifier("HRD_Religion", "RId", ViewState["__RId__"].ToString(), sqlDB.connection))
                {
                    lblMessage.InnerText = "success->Successfully Deleted";
                    AllClear();
                    // clear();
                }
                }
                 else
                     lblMessage.InnerText = "error->Warning! Can't delete this Religion. It is used for emplloyes.";
            }
        }

        protected void btnNew_Click1(object sender, EventArgs e)
        {
            AllClear();
        }

        protected void gvQualificationList_RowDataBound(object sender, GridViewRowEventArgs e)
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
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
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

        protected void gvQualificationList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            LoadReligion("");
        }
        private bool deleteValidation(string RId)
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("select RId from Personnel_EmpPersonnal where RId=" + RId + "", dt);
            if (dt.Rows.Count > 0)
                return false;
            else return true;
        }
    }
}