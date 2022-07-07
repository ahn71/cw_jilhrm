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
    public partial class vat_tax_investment : System.Web.UI.Page
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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "vat_rate_settings.aspx", ddlCompanyName, gvvatraxrateSettings, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();

                commonTask.loadTaxYearsOnlyYears(ddlInvstYear, ddlCompanyName.SelectedValue);
                classes.Employee.LoadEmpCardNoWithNameByCompany(ddlEmployeeList, ddlCompanyName.SelectedValue, "2","");
                
            }
            catch { }

        }
        private void loadVatTaxRateSettings()
        {
            try
            {
                DataTable dt;
                sqlDB.fillDataTable("SELECT * FROM VatTax_EmpInvestment where EmpId='" + ddlEmployeeList.SelectedValue + "' ", dt = new DataTable());
                gvvatraxrateSettings.DataSource = dt;
                gvvatraxrateSettings.DataBind();
            }
            catch { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                if (ddlInvstYear.SelectedIndex < 1) { lblMessage.InnerText = "warning-> Please, Select Investment Year !"; ddlInvstYear.Focus(); return; }    
                if (ddlEmployeeList.SelectedIndex < 1) { lblMessage.InnerText = "warning-> Please, Select Employee !"; ddlEmployeeList.Focus(); return; }
                            
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
                string[] getColumns = { "EmpId", "InvstYear", "LifeInsurPremium", "ContrDepositPensionScheme", "InvstInApprSavingsCertificate", "InvstInApprDebentureOrDebentureStock_StockOrShares", "ContrPFWhichPFAct1925Applies", "ContrSuperAnnuationFund", "ContrBenevolentFundAndGroupInsurPremium", "ContrZakatFund", "Others" };
                string[] getValues = { ddlEmployeeList.SelectedValue, ddlInvstYear.SelectedValue, txtLifeInsurPremium.Text, txtContrDepositPensionScheme.Text, txtInvstInApprSavingsCertificate.Text, txtInvstInApprDebentureOrDebentureStock_StockOrShares.Text, txtContrPFWhichPFAct1925Applies.Text, txtContrSuperAnnuationFund.Text, txtContrBenevolentFundAndGroupInsurPremium.Text, txtContrZakatFund.Text, txtOthers.Text};

                if (SQLOperation.forSaveValue("VatTax_EmpInvestment", getColumns, getValues, sqlDB.connection) == true)
                {
                    AllClear();
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
                string[] getColumns = {   "LifeInsurPremium", "ContrDepositPensionScheme", "InvstInApprSavingsCertificate", "InvstInApprDebentureOrDebentureStock_StockOrShares", "ContrPFWhichPFAct1925Applies", "ContrSuperAnnuationFund", "ContrBenevolentFundAndGroupInsurPremium", "ContrZakatFund", "Others" };
                string[] getValues = {  txtLifeInsurPremium.Text, txtContrDepositPensionScheme.Text, txtInvstInApprSavingsCertificate.Text, txtInvstInApprDebentureOrDebentureStock_StockOrShares.Text, txtContrPFWhichPFAct1925Applies.Text, txtContrSuperAnnuationFund.Text, txtContrBenevolentFundAndGroupInsurPremium.Text, txtContrZakatFund.Text, txtOthers.Text };
                if (SQLOperation.forUpdateValue("VatTax_EmpInvestment", getColumns, getValues, "SL", ViewState["__getSL__"].ToString(), sqlDB.connection) == true)
                {
                    // saveShiftConfigDateLog(true, StartTime, EndTime);
                    lblMessage.InnerText = "success->Successfully  Updated";
                    loadVatTaxRateSettings();
                    AllClear();
                }
            }
            catch { }
        }

        protected void gvvatraxrateSettings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                if (e.CommandName.Equals("Alter"))
                {
                    string a = ViewState["__preRIndex__"].ToString();
                    if (!ViewState["__preRIndex__"].ToString().Equals("No")) gvvatraxrateSettings.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    gvvatraxrateSettings.Rows[rIndex].BackColor = System.Drawing.Color.Yellow;
                    ViewState["__preRIndex__"] = rIndex;
                    setValueToControl(gvvatraxrateSettings.DataKeys[rIndex].Values[0].ToString());                   
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

                    SQLOperation.forDeleteRecordByIdentifier("VatTax_EmpInvestment", "SL", gvvatraxrateSettings.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);                    
                    AllClear();
                    lblMessage.InnerText = "success->Successfully  Deleted";
                    gvvatraxrateSettings.Rows[rIndex].Visible = false;

                }
            }
            catch { }
        }
   
        private void setValueToControl( string getSL)
        {
            try
            {
                
                DataTable dt;
                sqlDB.fillDataTable("SELECT * FROM VatTax_EmpInvestment where SL=" + getSL + "", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    ddlInvstYear.SelectedValue = dt.Rows[0]["InvstYear"].ToString();
                    txtLifeInsurPremium.Text = dt.Rows[0]["LifeInsurPremium"].ToString();
                    txtContrDepositPensionScheme.Text = dt.Rows[0]["ContrDepositPensionScheme"].ToString();
                    txtInvstInApprSavingsCertificate.Text = dt.Rows[0]["InvstInApprSavingsCertificate"].ToString();
                    txtInvstInApprDebentureOrDebentureStock_StockOrShares.Text = dt.Rows[0]["InvstInApprDebentureOrDebentureStock_StockOrShares"].ToString();
                    txtContrPFWhichPFAct1925Applies.Text = dt.Rows[0]["ContrPFWhichPFAct1925Applies"].ToString();
                    txtContrBenevolentFundAndGroupInsurPremium.Text = dt.Rows[0]["ContrBenevolentFundAndGroupInsurPremium"].ToString();
                    txtContrZakatFund.Text = dt.Rows[0]["ContrZakatFund"].ToString();
                    txtOthers.Text = dt.Rows[0]["Others"].ToString();
                    ViewState["__getSL__"] = getSL;
                    btnSave.Text = "Update";
                }
              


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

        

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            commonTask.loadTaxYearsOnlyYears(ddlInvstYear, ddlCompanyName.SelectedValue);
            classes.Employee.LoadEmpCardNoWithNameByCompany(ddlEmployeeList, ddlCompanyName.SelectedValue, "2","");
        }

        protected void ddlEmployeeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            if (ddlInvstYear.SelectedIndex < 1) { lblMessage.InnerText = "warning-> Please, Select Investment Year !"; ddlInvstYear.Focus(); return; }    
            loadVatTaxRateSettings();
            alter();
           
        }
        protected void ddlInvstYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            alter();
        }
        private void alter() 
        {
            try
            {

                DataTable dt;
                sqlDB.fillDataTable("SELECT * FROM VatTax_EmpInvestment where EmpId='" + ddlEmployeeList.SelectedValue + "' and InvstYear='" + ddlInvstYear.SelectedValue + "' ", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    txtLifeInsurPremium.Text = dt.Rows[0]["LifeInsurPremium"].ToString();
                    txtContrDepositPensionScheme.Text = dt.Rows[0]["ContrDepositPensionScheme"].ToString();
                    txtInvstInApprSavingsCertificate.Text = dt.Rows[0]["InvstInApprSavingsCertificate"].ToString();
                    txtInvstInApprDebentureOrDebentureStock_StockOrShares.Text = dt.Rows[0]["InvstInApprDebentureOrDebentureStock_StockOrShares"].ToString();
                    txtContrPFWhichPFAct1925Applies.Text = dt.Rows[0]["ContrPFWhichPFAct1925Applies"].ToString();
                    txtContrBenevolentFundAndGroupInsurPremium.Text = dt.Rows[0]["ContrBenevolentFundAndGroupInsurPremium"].ToString();
                    txtContrZakatFund.Text = dt.Rows[0]["ContrZakatFund"].ToString();
                    txtOthers.Text = dt.Rows[0]["Others"].ToString();
                    ViewState["__getSL__"] = dt.Rows[0]["SL"].ToString();
                    btnSave.Text = "Update";
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

                }
                else
                {
                    AllClear();
                }

            }
            catch { }
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            ddlInvstYear.SelectedValue = "0";
            AllClear();
        }
        private void AllClear() 
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
            btnSave.Text = "Save";
            txtLifeInsurPremium.Text = "0";
            txtContrDepositPensionScheme.Text = "0";
            txtInvstInApprSavingsCertificate.Text = "0";
            txtInvstInApprDebentureOrDebentureStock_StockOrShares.Text = "0";
            txtContrPFWhichPFAct1925Applies.Text = "0";
            txtContrBenevolentFundAndGroupInsurPremium.Text = "0";
            txtContrZakatFund.Text = "0";
            txtOthers.Text = "0";
            txtContrSuperAnnuationFund.Text = "0";
            btnSave.Text = "Save";
        }
       
    }
}