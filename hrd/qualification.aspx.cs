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
    public partial class qualification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
            
                LoadQualification("");
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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "qualification.aspx", gvQualificationList, btnSave);

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
                    if (UpdatePunishmentType() == true)
                    {
                        LoadQualification("");
                        AllClear();
                        lblMessage.InnerText = "success->Successfully Updated";
                    }
                }
                else
                {
                    if (SavepunishmentType() == true)
                    {
                        AllClear();
                        LoadQualification("");
                        lblMessage.InnerText = "success->Successfully Saved";
                    }
                }
            }
            catch
            {
                LoadQualification("");
            }
        }
        private Boolean SavepunishmentType()
        {
            try
            {
                string[] getColumns = { "QName", "QNameBn" };
                string[] getValues = { txtQualification.Text.Trim(), txtQualificationBn.Text.Trim() };
                if (SQLOperation.forSaveValue("HRD_Qualification", getColumns, getValues, sqlDB.connection) == true)
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
        private Boolean UpdatePunishmentType()
        {
            try
            {
                string[] getColumns = { "QName", "QNameBn" };
                string[] getValues = { txtQualification.Text.Trim(), txtQualificationBn.Text.Trim() };
                string getIdentifierValue = ViewState["__QId__"].ToString();
                if (SQLOperation.forUpdateValue("HRD_Qualification", getColumns, getValues, "QId", getIdentifierValue, sqlDB.connection) == true)
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
            hdnbtnStage.Value = "0";
            hdnUpdate.Value = "";
            txtQualification.Text = "";
            txtQualificationBn.Text = "";
            btnSave.Text = "Save";
            txtQualification.Focus();
        }


        private void LoadQualification(string sqlcmd)
        {
            sqlcmd = "SELECT * FROM HRD_Qualification";
            DataTable dt = new DataTable();
            sqlDB.fillDataTable(sqlcmd, dt);
            gvQualificationList.DataSource = dt;
            gvQualificationList.DataBind();
          
        }

        protected void gvQualificationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            LoadQualification("");
            gvQualificationList.PageIndex = e.NewPageIndex;
            gvQualificationList.DataBind();
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

        protected void gvQualificationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
            ViewState["__QId__"] = gvQualificationList.DataKeys[rIndex].Values[0].ToString();
            if (e.CommandName == "Alter")
            {
                txtQualification.Text = gvQualificationList.Rows[rIndex].Cells[0].Text.ToString();
                txtQualificationBn.Text = (gvQualificationList.Rows[rIndex].Cells[1].Text.ToString().Equals("&nbsp;")) ? "" : gvQualificationList.Rows[rIndex].Cells[1].Text.ToString();              
                btnSave.Text = "Update";
            }
            else if (e.CommandName == "Delete")
            {
                if (deleteValidation(ViewState["__QId__"].ToString()))
                {
                    if (SQLOperation.forDeleteRecordByIdentifier("HRD_Qualification", "QId", ViewState["__QId__"].ToString(), sqlDB.connection))
                    {
                        lblMessage.InnerText = "success->Successfully Deleted";
                        AllClear();
                        // clear();
                    }
                }
                else
                    lblMessage.InnerText = "error->Warning! Can't delete this Qualification. It is used for emplloyes.";
            }
        }

        protected void gvQualificationList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            LoadQualification("");
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            AllClear();
        }

        private bool deleteValidation(string QID)
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("select LastEdQualification from Personnel_EmpPersonnal where LastEdQualification=" + QID + "", dt);
            if (dt.Rows.Count > 0)
                return false;
            else return true;
        }
    }
}