using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.hrd
{
    public partial class punishment_type : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                btnDelete.CssClass = "";
                btnDelete.Enabled = false;
                LoadPunishmentType("");
            }
        }
        private void setPrivilege()
        {
            try
            {
                upupdate.Value = "1";
                updelete.Value = "1";
                upSave.Value = "1";
                ViewState["__WriteAction__"] = "1";
                ViewState["__DeletAction__"] = "1";
                ViewState["__ReadAction__"] = "1";
                ViewState["__UpdateAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='punishment_type.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false))
                        {
                            ViewState["__ReadAction__"] = "0";
                            divpunishmenttype.Visible = false;
                        }
                        if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                        {
                            ViewState["__WriteAction__"] = "0";
                            btnSave.CssClass = "";
                            btnSave.Enabled = false;
                            upSave.Value = "0";
                        }
                        if (bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false))
                        {
                            ViewState["__UpdateAction__"] = "0";
                            upupdate.Value = "0";
                        }
                        if (bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                        {
                            ViewState["__DeletAction__"] = "0";
                            updelete.Value = "0";
                        }


                    }
                    //  dlExistsUser.Enabled = false;
                }

            }
            catch { }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (hdnbtnStage.Value.ToString().Equals("1"))
                {
                    if (UpdatePunishmentType() == true)
                    {
                        LoadPunishmentType("");
                        AllClear();
                        lblMessage.InnerText = "success->Successfully Updated";
                    }
                }
                else
                {
                    if (SavepunishmentType() == true)
                    {
                        AllClear();
                        LoadPunishmentType("");
                        lblMessage.InnerText = "success->Successfully Saved";
                    }
                }
            }
            catch
            {
                LoadPunishmentType("");
            }
        }
        private Boolean SavepunishmentType()
        {
            try
            {
                string[] getColumns = { "PtName" };
                string[] getValues = {txtPunishmentType.Text.Trim() };
                if (SQLOperation.forSaveValue("HRD_PunishmentType", getColumns, getValues, sqlDB.connection) == true)
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
                string[] getColumns = { "PtName" };
                string[] getValues = {txtPunishmentType.Text.Trim() };
                string getIdentifierValue = hdnUpdate.Value.ToString();
                if (SQLOperation.forUpdateValue("HRD_PunishmentType", getColumns, getValues, "PtId", getIdentifierValue, sqlDB.connection) == true)
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
            txtPunishmentType.Text = "";          
            btnDelete.CssClass = "";
            btnDelete.Enabled = false;
            btnSave.Text = "Save";
            txtPunishmentType.Focus();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string getIdentifierValue = hdnUpdate.Value.ToString(); 
                if (SQLOperation.forDeleteRecordByIdentifier("HRD_PunishmentType", "PtId", getIdentifierValue, sqlDB.connection) == true)
                {
                    LoadPunishmentType("");
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
            }
        }
        private void LoadPunishmentType(string sqlcmd)
        {
            if (string.IsNullOrEmpty(sqlcmd)) sqlcmd = "SELECT * FROM HRD_PunishmentType";
            DataTable dt = new DataTable();
            sqlDB.fillDataTable(sqlcmd, dt);

            int totalRows = dt.Rows.Count;
            string divInfo = "";
            divpunishmenttype.Controls.Clear();

            if (totalRows == 0)
            {
                divInfo = "<div class='noData'>No Punishment Type available</div>";
                divInfo += "<div class='dataTables_wrapper'><div class='head'></div></div>";
                divpunishmenttype.Controls.Add(new LiteralControl(divInfo));
                return;
            }

            divInfo = " <table id='tblClassList' class='display'  > ";
            divInfo += "<thead>";
            divInfo += "<tr>";
            divInfo += "<th>Punishment Type</th>";
            divInfo += "<th style='width:70px'>Edit</th>";
            divInfo += "</tr>";

            divInfo += "</thead>";

            divInfo += "<tbody>";
            string id = "";

            for (int x = 0; x < dt.Rows.Count; x++)
            {
                id = dt.Rows[x]["PtId"].ToString();
                divInfo += "<tr id='r_" + id + "'>";
                divInfo += "<td >" + dt.Rows[x]["PtName"].ToString() + "</td>";

                divInfo += "<td class='numeric_control' >" + "<img src='/Images/datatable/edit.png' class='editImg'   onclick='editPunishmenttype(" + id + ");'  />";
            }

            divInfo += "</tbody>";
            divInfo += "<tfoot>";

            divInfo += "</table>";

            divpunishmenttype.Controls.Add(new LiteralControl(divInfo));
        }
    }
}