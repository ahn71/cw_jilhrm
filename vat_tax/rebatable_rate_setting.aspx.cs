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

namespace SigmaERP.vat_tax
{
    public partial class rebatable_rate_setting : System.Web.UI.Page
    {
        string CompanyId = "";
        string sqlcmd = "";
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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "rebatable_rate_setting.aspx", ddlCompanyName, gvvatraxrateSettings, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
                loadVatTaxRateSettings();

            }


            catch { }

        }
        private void loadVatTaxRateSettings()
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("SELECT * FROM v_VatTax_Rebatable_Rate where CompanyId='" + ddlCompanyName.SelectedValue + "'", dt);
            gvvatraxrateSettings.DataSource = dt;
            gvvatraxrateSettings.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text == "Update")
                {
                    Update();
                }
                else
                {
                    Save();
                }
            }
            catch { }
        }
        private void Save()
        {
            try
            {
                string[] getColumns = { "CompanyId", "SlabName", "FromTaka", "ToTaka", "IncomeTaxRate", "RateOrder" };
                string[] getValues = { ddlCompanyName.SelectedValue, txtSlabName.Text, txtFromTaka.Text, txttoTaka.Text, txtincometaxrate.Text, txtOrder.Text };

                if (SQLOperation.forSaveValue("VatTax_Rebatable_Rate", getColumns, getValues, sqlDB.connection) == true)
                {
                    allClear();
                    loadVatTaxRateSettings();
                    lblMessage.InnerText = "success->Successfully Save";
                }
            }
            catch { }
        }
        private void Update()
        {
            try
            {
                string[] getColumns = { "CompanyId", "SlabName", "FromTaka", "ToTaka", "IncomeTaxRate", "RateOrder" };
                string[] getValues = { ddlCompanyName.SelectedValue, txtSlabName.Text, txtFromTaka.Text, txttoTaka.Text, txtincometaxrate.Text, txtOrder.Text };

                if (SQLOperation.forUpdateValue("VatTax_Rebatable_Rate", getColumns, getValues, "RSN", ViewState["__getSL__"].ToString(), sqlDB.connection) == true)
                {
                    // saveShiftConfigDateLog(true, StartTime, EndTime);
                    lblMessage.InnerText = "success->Successfully  Updated";
                    loadVatTaxRateSettings();
                    allClear();
                }
            }
            catch { }
        }

        protected void gvvatraxrateSettings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName.Equals("Alter"))
                {
                    string a = ViewState["__preRIndex__"].ToString();
                    if (!ViewState["__preRIndex__"].ToString().Equals("No")) gvvatraxrateSettings.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    gvvatraxrateSettings.Rows[rIndex].BackColor = System.Drawing.Color.Yellow;
                    ViewState["__preRIndex__"] = rIndex;
                    setValueToControl(rIndex, gvvatraxrateSettings.DataKeys[rIndex].Values[0].ToString(), gvvatraxrateSettings.DataKeys[rIndex].Values[1].ToString());
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

                    SQLOperation.forDeleteRecordByIdentifier("VatTax_Rebatable_Rate", "RSN", gvvatraxrateSettings.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "deleteSuccess()", true);
                    allClear();
                    lblMessage.InnerText = "success->Successfully  Deleted";
                    gvvatraxrateSettings.Rows[rIndex].Visible = false;

                }
            }
            catch { }
        }
        private void allClear()
        {
            txtFromTaka.Text = "";
            txtincometaxrate.Text = "";
            txtOrder.Text = "";
            txtSlabName.Text = "";
            txttoTaka.Text = "";
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
            btnSave.Text = "Save";

        }
        private void setValueToControl(int rIndex, string getSL, string getCompanyId)
        {
            try
            {
                ViewState["__getSL__"] = getSL;
                ddlCompanyName.SelectedValue = getCompanyId;
                txtSlabName.Text = gvvatraxrateSettings.Rows[rIndex].Cells[1].Text;
                txtFromTaka.Text = gvvatraxrateSettings.Rows[rIndex].Cells[2].Text;
                txttoTaka.Text = gvvatraxrateSettings.Rows[rIndex].Cells[3].Text;
                txtincometaxrate.Text = gvvatraxrateSettings.Rows[rIndex].Cells[4].Text;
                txtOrder.Text = gvvatraxrateSettings.Rows[rIndex].Cells[5].Text;


            }
            catch { }
        }

        protected void gvvatraxrateSettings_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
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
                        LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkAlter");
                        lnkDelete.Enabled = false;
                        lnkDelete.ForeColor = Color.Silver;
                    }

                }
                catch { }
            }
        }
    }
}