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

namespace SigmaERP.vat_tax
{
    public partial class taxfreeallowance : System.Web.UI.Page
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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "taxfreeallowance.aspx", ddlCompanyName, gvtaxfreeallowance, btnSave);
                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
                loadTaxFreeAllowance();

            }


            catch { }

        }
        private void loadTaxFreeAllowance()
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("SELECT * FROM VatTax_TaxFreeAllowance ", dt);
            gvtaxfreeallowance.DataSource = dt;
            gvtaxfreeallowance.DataBind();
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
                SqlCommand cmd = new SqlCommand("Delete From VatTax_TaxFreeAllowance", sqlDB.connection);
                cmd.ExecuteNonQuery();
                string[] getColumns = { "ConvenceAllownce", "HouseRent", "MedicalAllownce" };
                string[] getValues = { txtConvenceAllownce.Text, txtHouseRent.Text, txtMedicalAllownce.Text};

                if (SQLOperation.forSaveValue("VatTax_TaxFreeAllowance", getColumns, getValues, sqlDB.connection) == true)
                {
                    allClear();
                    loadTaxFreeAllowance();
                    lblMessage.InnerText = "success->Successfully Save";
                }
            }
            catch { }
        }
        private void Update()
        {
            try
            {
                string[] getColumns = { "ConvenceAllownce", "HouseRent", "MedicalAllownce" };
                string[] getValues = { txtConvenceAllownce.Text, txtHouseRent.Text, txtMedicalAllownce.Text };

                if (SQLOperation.forUpdateValue("VatTax_TaxFreeAllowance", getColumns, getValues, "TFSN", ViewState["__getSL__"].ToString(), sqlDB.connection) == true)
                {
                    // saveShiftConfigDateLog(true, StartTime, EndTime);
                    lblMessage.InnerText = "success->Successfully  Updated";
                    loadTaxFreeAllowance();
                    allClear();
                }
            }
            catch { }
        }
        private void allClear()
        {
            txtConvenceAllownce.Text = "";
            txtHouseRent.Text = "";
            txtMedicalAllownce.Text = "";
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

        protected void gvtaxfreeallowance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName.Equals("Alter"))
                {
                    string a = ViewState["__preRIndex__"].ToString();
                    if (!ViewState["__preRIndex__"].ToString().Equals("No")) gvtaxfreeallowance.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    gvtaxfreeallowance.Rows[rIndex].BackColor = System.Drawing.Color.Yellow;
                    ViewState["__preRIndex__"] = rIndex;
                    setValueToControl(rIndex, gvtaxfreeallowance.DataKeys[rIndex].Values[0].ToString());
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

                    SQLOperation.forDeleteRecordByIdentifier("VatTax_TaxFreeAllowance", "TFSN", gvtaxfreeallowance.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "deleteSuccess()", true);
                    allClear();
                    lblMessage.InnerText = "success->Successfully  Deleted";
                    gvtaxfreeallowance.Rows[rIndex].Visible = false;

                }
            }
            catch { }
        }
        private void setValueToControl(int rIndex, string getSL)
        {
            try
            {
                ViewState["__getSL__"] = getSL;
                txtConvenceAllownce.Text = gvtaxfreeallowance.Rows[rIndex].Cells[0].Text;
                txtHouseRent.Text = gvtaxfreeallowance.Rows[rIndex].Cells[1].Text;
                txtMedicalAllownce.Text = gvtaxfreeallowance.Rows[rIndex].Cells[2].Text;
            }
            catch { }
        }

        protected void gvtaxfreeallowance_RowDataBound(object sender, GridViewRowEventArgs e)
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