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
    public partial class allowance_calculation_settingsvat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
                //loadAllowanceCalculationSettings();
                classes.commonTask.loadEmpTypeInRadioButtonList(rblEmpType);
                classes.commonTask.loadEmpTypeInRadioButtonList(rblEmployeeType2);
                if (!classes.commonTask.HasBranch())
                {
                    ddlCompanyList.Enabled = false;
                    ddlCompanyList2.Enabled = false;
                }

                loadSalaryCalculationSettings(ddlCompanyList.SelectedValue);
                loadHRD_AllownceSetting(ddlCompanyList2.SelectedValue);
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
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "allowance_calculation_settingsvat.aspx", gvSalaryCalculationList, gvAllowanceAmountList, btnSave, btnSave2, ddlCompanyList, ViewState["__CompanyId__"].ToString(), ddlCompanyList2);
                                                           
                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                ddlCompanyList2.SelectedValue = ViewState["__CompanyId__"].ToString();

            }
            catch { }

        }
        public void loadAllowanceCalculationSettings(string AlCalId)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select * from Payroll_AllowanceCalculationSetting where AlCalId='" + AlCalId + "'", dt);
                if (dt.Rows.Count > 0)
                {
                    ViewState["__AlCalId__"] = AlCalId;
                    rblEmpType.SelectedValue = dt.Rows[0]["EmpTypeId"].ToString();
                    rblSalaryType.SelectedValue = dt.Rows[0]["SalaryType"].ToString();
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
                    btnSave.Text = "Update";

                    rblHouseRent.SelectedValue = dt.Rows[0]["HouseRent"].ToString();


                    rblMedical.SelectedValue = dt.Rows[0]["MedicalAllownce"].ToString();
                    rblFood.SelectedValue = dt.Rows[0]["FoodAllownce"].ToString();
                    rblConvence.SelectedValue = dt.Rows[0]["ConvenceAllownce"].ToString();
                    rblTechnical.SelectedValue = dt.Rows[0]["TechnicalAllowance"].ToString();
                    rblOthers.SelectedValue = dt.Rows[0]["OthersAllowance"].ToString();
                    rblProvident.SelectedValue = dt.Rows[0]["ProvidentFund"].ToString();
                    rblBasic.SelectedValue = dt.Rows[0]["BasicAllowance"].ToString();

                    ChooseSalaryType(rblSalaryType.SelectedValue, true);
                }
                else lblMessage.InnerText = "warinig->Allowance calculation settings not set";

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadSalaryCalculationSettings(ddlCompanyList.SelectedValue);

        }
        protected void ddlCompanyList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadHRD_AllownceSetting(ddlCompanyList2.SelectedValue);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (InputValidation())
                {
                    if (btnSave.Text.Trim() == "Save") saveAllowanceSettings();
                    else updateAllowanceSettings(ViewState["__AlCalId__"].ToString());
                    clearInput();
                }
            }
            catch { }
        }

        private bool InputValidation()
        {
            try
            {
                if (rblEmpType.SelectedIndex == -1)
                {
                    lblMessage.InnerText = "error->Please select employee type.";
                    rblEmpType.Focus();
                    return false;
                }
                else if (rblSalaryType.SelectedIndex == -1)
                {
                    lblMessage.InnerText = "error->Please select salary type.";
                    rblSalaryType.Focus();
                    return false;
                }
                else if (btnSave.Text.Trim() == "Save")
                {
                    foreach (GridViewRow gvr in gvSalaryCalculationList.Rows)
                    {
                        if (gvr.Cells[10].Text.Trim().Equals(rblEmpType.SelectedItem.ToString().Trim()))
                        {
                            lblMessage.InnerText = "error->Already allowance  setuped for this type of employee.";
                            return false;
                        }

                    }
                }
                return true;
            }
            catch { return false; }
        }

        private string checkHasRecord()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select * from Payroll_AllowanceCalculationSetting", dt);
                if (dt.Rows.Count > 0) return dt.Rows[0]["AlCalId"].ToString();
                else return null;
            }
            catch { return null; }
        }

        private void saveAllowanceSettings()
        {

            try
            {
                string[] getColumns = { "BasicAllowance", "MedicalAllownce", "FoodAllownce", "ConvenceAllownce", "HouseRent", "TechnicalAllowance", "OthersAllowance", "ProvidentFund", "EmpTypeId", "SalaryType", "CompanyId", "CalculationType" };
                string[] getValues = { rblBasic.SelectedValue, rblMedical.SelectedValue, rblFood.SelectedValue, rblConvence.SelectedValue, rblHouseRent.SelectedValue, rblTechnical.SelectedValue, rblOthers.SelectedValue, rblProvident.SelectedValue, rblEmpType.SelectedValue, rblSalaryType.SelectedValue, ddlCompanyList.SelectedValue, "vattax" };
                if (SQLOperation.forSaveValue("Payroll_AllowanceCalculationSetting", getColumns, getValues, sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success->Successfully Set";
                    loadSalaryCalculationSettings(ddlCompanyList.SelectedValue);
                    clearInput();
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }

        }

        private void updateAllowanceSettings(string AlCalId)
        {
            try
            {

                string[] getColumns = { "BasicAllowance", "MedicalAllownce", "FoodAllownce", "ConvenceAllownce", "HouseRent", "TechnicalAllowance", "OthersAllowance", "ProvidentFund", "EmpTypeId", "SalaryType", "CompanyId" };
                string[] getValues = { rblBasic.SelectedValue, rblMedical.SelectedValue, rblFood.SelectedValue, rblConvence.SelectedValue, rblHouseRent.SelectedValue, rblTechnical.SelectedValue, rblOthers.SelectedValue, rblProvident.SelectedValue, rblEmpType.SelectedValue, rblSalaryType.SelectedValue, ddlCompanyList.SelectedValue };
                if (SQLOperation.forUpdateValue("Payroll_AllowanceCalculationSetting", getColumns, getValues, "AlCalId", AlCalId, sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success->Successfully Set";
                    loadSalaryCalculationSettings(ddlCompanyList.SelectedValue); clearInput();
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private void loadSalaryCalculationSettings(string companyid)
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("select acs.AlCalId," +
                " case when acs.BasicAllowance=0 then '%' when acs.BasicAllowance=1 then 't' else 'x' end BasicAllowance ," +
                " Case when acs.MedicalAllownce=0 then '%' when acs.MedicalAllownce=1 then 't' else 'x' end MedicalAllownce," +
                " case when acs.FoodAllownce=0 then '%' when acs.FoodAllownce=1 then 't' else 'x'end FoodAllownce," +
                " Case when acs.ConvenceAllownce=0 then '%' when acs.ConvenceAllownce=1 then 't' else 'x' end ConvenceAllownce , " +
                " Case when acs.HouseRent=0 then '%' when acs.HouseRent=1 then 't' else 'x' end HouseRent, " +
                " Case when acs.TechnicalAllowance=0 then '%' when acs.TechnicalAllowance=1 then 't' else 'x' End TechnicalAllowance," +
                " Case when acs.OthersAllowance=0 then '%' when acs.OthersAllowance=1 then 't' else 'x' end OthersAllowance," +
                " Case when acs.ProvidentFund=0 then '%' when acs.ProvidentFund=1 then 't' else 'x' end ProvidentFund ,acs.SalaryType," +
                " FORMAT(acs.EntryDate,'dd-MM-yyyy hh:MM:ss tt') as EntryDate,et.EmpType  from Payroll_AllowanceCalculationSetting as  acs" +
                " inner join HRD_EmployeeType et on acs.empTypeId=et.EmpTypeId where acs.CalculationType='vattax' and acs.CompanyId='" + companyid + "'", dt);
            gvSalaryCalculationList.DataSource = dt;
            gvSalaryCalculationList.DataBind();
        }

        protected void rblSalaryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChooseSalaryType(rblSalaryType.SelectedValue);
        }

        private void ChooseSalaryType(string SalaryType)
        {
            try
            {
                if (SalaryType.Equals("Scale"))
                {
                    rblBasic.Enabled = false;
                    rblBasic.SelectedIndex = 2;

                    rblHouseRent.SelectedIndex = 0;
                    rblMedical.SelectedIndex = 0;
                    rblFood.SelectedIndex = 0;
                    rblConvence.SelectedIndex = 0;
                    rblTechnical.SelectedIndex = 0;
                    rblOthers.SelectedIndex = 0;
                    rblProvident.SelectedIndex = 0;

                    rblHouseRent.Enabled = true;
                    rblMedical.Enabled = true;
                    rblFood.Enabled = true;
                    rblConvence.Enabled = true;
                    rblTechnical.Enabled = true;
                    rblOthers.Enabled = true;

                }
                else if (SalaryType.Equals("Gross"))
                {
                    rblBasic.Enabled = true;
                    rblBasic.SelectedIndex = 2;

                    rblBasic.Enabled = false;
                    rblBasic.SelectedIndex = 2;

                    rblHouseRent.Enabled = false;
                    rblHouseRent.SelectedIndex = 2;

                    rblMedical.Enabled = false;
                    rblMedical.SelectedIndex = 2;

                    rblFood.Enabled = false;
                    rblFood.SelectedIndex = 2;

                    rblConvence.Enabled = false;
                    rblConvence.SelectedIndex = 2;

                    rblTechnical.Enabled = false;
                    rblTechnical.SelectedIndex = 2;

                    rblOthers.Enabled = false;
                    rblOthers.SelectedIndex = 2;

                    rblProvident.SelectedIndex = 1;
                }
                else
                {
                    rblBasic.Enabled = true;
                    rblBasic.SelectedIndex = 0;

                    rblBasic.Enabled = true;
                    rblBasic.SelectedIndex = 0;

                    rblHouseRent.Enabled = true;
                    rblHouseRent.SelectedIndex = 0;

                    rblMedical.Enabled = true;
                    rblMedical.SelectedIndex = 0;

                    rblFood.Enabled = true;
                    rblFood.SelectedIndex = 0;

                    rblConvence.Enabled = true;
                    rblConvence.SelectedIndex = 0;

                    rblTechnical.Enabled = true;
                    rblTechnical.SelectedIndex = 0;

                    rblOthers.Enabled = true;
                    rblOthers.SelectedIndex = 0;

                    rblProvident.SelectedIndex = 0;
                }
            }
            catch { }
        }
        private void ChooseSalaryType(string SalaryType, bool IsUpdateTime)
        {
            try
            {
                if (SalaryType.Equals("Scale"))
                {
                    rblBasic.Enabled = false;
                    rblBasic.SelectedValue = "1";
                }
                else if (SalaryType.Equals("Gross"))
                {
                    trBasic.Visible = true;
                    rblBasic.Enabled = false;
                    rblBasic.SelectedIndex = 2;
                    rblHouseRent.Enabled = false;
                    rblHouseRent.SelectedIndex = 2;
                    rblMedical.Enabled = false;
                    rblMedical.SelectedIndex = 2;
                    rblFood.Enabled = false;
                    rblFood.SelectedIndex = 2;
                    rblConvence.Enabled = false;
                    rblConvence.SelectedIndex = 2;
                    rblTechnical.Enabled = false;
                    rblTechnical.SelectedIndex = 2;
                    rblOthers.Enabled = false;
                    rblOthers.SelectedIndex = 2;


                }
                else
                {
                    rblBasic.Enabled = true;
                    rblBasic.Enabled = true;
                    rblHouseRent.Enabled = true;
                    rblMedical.Enabled = true;
                    rblMedical.Enabled = true;
                    rblFood.Enabled = true;
                    rblConvence.Enabled = true;
                    rblTechnical.Enabled = true;
                    rblOthers.Enabled = true;

                }
            }
            catch { }
        }



        private void clearInput()
        {
            try
            {
                rblBasic.Enabled = true;
                rblHouseRent.Enabled = true;
                rblMedical.Enabled = true;
                rblFood.Enabled = true;
                rblConvence.Enabled = true;
                rblTechnical.Enabled = true;
                rblOthers.Enabled = true;

                rblBasic.SelectedIndex = -1;
                rblHouseRent.SelectedIndex = -1;
                rblMedical.SelectedIndex = -1;
                rblFood.SelectedIndex = -1;
                rblConvence.SelectedIndex = -1;
                rblTechnical.SelectedIndex = -1;
                rblOthers.SelectedIndex = -1;

                rblEmpType.SelectedIndex = -1;
                rblSalaryType.SelectedIndex = -1;
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
            catch { }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clearInput();
        }

        protected void gvSalaryCalculationList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                    e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";

                    if (e.Row.Cells[1].Text == "t")
                    {
                        e.Row.Cells[1].Text = "৳";
                        e.Row.Cells[1].ForeColor = System.Drawing.Color.Green;
                    }
                    else if (e.Row.Cells[1].Text == "x") e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;

                    if (e.Row.Cells[2].Text == "t")
                    {
                        e.Row.Cells[2].Text = "৳";
                        e.Row.Cells[2].ForeColor = System.Drawing.Color.Green;
                    }
                    else if (e.Row.Cells[2].Text == "x") e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;

                    if (e.Row.Cells[3].Text == "t")
                    {
                        e.Row.Cells[3].Text = "৳";
                        e.Row.Cells[3].ForeColor = System.Drawing.Color.Green;
                    }
                    else if (e.Row.Cells[3].Text == "x") e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;

                    if (e.Row.Cells[4].Text == "t")
                    {
                        e.Row.Cells[4].Text = "৳";
                        e.Row.Cells[4].ForeColor = System.Drawing.Color.Green;
                    }
                    else if (e.Row.Cells[4].Text == "x") e.Row.Cells[4].ForeColor = System.Drawing.Color.Red;

                    if (e.Row.Cells[5].Text == "t")
                    {
                        e.Row.Cells[5].Text = "৳";
                        e.Row.Cells[5].ForeColor = System.Drawing.Color.Green;
                    }
                    else if (e.Row.Cells[5].Text == "x") e.Row.Cells[5].ForeColor = System.Drawing.Color.Red;

                    if (e.Row.Cells[6].Text == "t")
                    {
                        e.Row.Cells[6].Text = "৳";
                        e.Row.Cells[6].ForeColor = System.Drawing.Color.Green;
                    }
                    else if (e.Row.Cells[6].Text == "x") e.Row.Cells[6].ForeColor = System.Drawing.Color.Red;

                    if (e.Row.Cells[7].Text == "t")
                    {
                        e.Row.Cells[7].Text = "৳";
                        e.Row.Cells[7].ForeColor = System.Drawing.Color.Green;
                    }
                    else if (e.Row.Cells[7].Text == "x") e.Row.Cells[7].ForeColor = System.Drawing.Color.Red;

                    if (e.Row.Cells[8].Text == "t")
                    {
                        e.Row.Cells[8].Text = "৳";
                        e.Row.Cells[8].ForeColor = System.Drawing.Color.Green;
                    }
                    else if (e.Row.Cells[8].Text == "x") e.Row.Cells[8].ForeColor = System.Drawing.Color.Red;
                }


            }
            catch { }
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
            {

                try
                {
                    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                        btnEdit.Enabled = false;
                        btnEdit.ForeColor = Color.Silver;
                    }

                }
                catch { }
            }
        }

        protected void gvSalaryCalculationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Alter"))
                {
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    loadAllowanceCalculationSettings(gvSalaryCalculationList.DataKeys[rIndex].Value.ToString());

                }
            }
            catch { }
        }



        private void SetConstrant()
        {
            try
            {
                // here 0=Percentage 1=Amount 2=Not Count
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select * from Payroll_AllowanceCalculationSetting where CalculationType='vattax' and EmpTypeId='" + rblEmployeeType2.SelectedValue + "' and companyId='" + ddlCompanyList2.SelectedValue + "'", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    lblSalaryType.Text = dt.Rows[0]["SalaryType"].ToString();
                    ViewState["__AlCalId__"] = dt.Rows[0]["AlCalId"].ToString();
                    if (dt.Rows[0]["BasicAllowance"].ToString() == "1")
                    {
                        txtBasic.MaxLength = 5;
                        lblBasic.Text = " ( ৳ )";
                        lblBasic.ForeColor = System.Drawing.Color.DarkGreen;
                        txtBasic.Enabled = true;
                    }
                    else if (dt.Rows[0]["BasicAllowance"].ToString() == "2")
                    {
                        lblBasic.Text = " ( x )";
                        lblBasic.ForeColor = System.Drawing.Color.Red;
                        txtBasic.Enabled = false;
                    }
                    else
                    {
                        lblBasic.Text = " ( % )";
                        lblBasic.ForeColor = System.Drawing.Color.Black;
                        txtBasic.MaxLength = 2;
                        txtBasic.Enabled = true;
                    }

                    if (dt.Rows[0]["MedicalAllownce"].ToString() == "1")
                    {
                        txtMedical.MaxLength = 5;
                        lblMedical.Text = " ( ৳ )";
                        lblMedical.ForeColor = System.Drawing.Color.DarkGreen;
                        txtMedical.Enabled = true;
                    }
                    else if (dt.Rows[0]["MedicalAllownce"].ToString() == "2")
                    {

                        lblMedical.Text = " ( x )";
                        lblMedical.ForeColor = System.Drawing.Color.Red;
                        txtMedical.Enabled = false;
                    }
                    else
                    {
                        lblMedical.Text = " ( % )";
                        lblMedical.ForeColor = System.Drawing.Color.Black;
                        txtMedical.MaxLength = 2;
                        txtMedical.Enabled = true;
                    }

                    if (dt.Rows[0]["FoodAllownce"].ToString() == "1")
                    {
                        txtFood.MaxLength = 5;
                        lblFood.Text = " ( ৳ )";
                        lblFood.ForeColor = System.Drawing.Color.DarkGreen;
                        txtFood.Enabled = true;
                    }
                    else if (dt.Rows[0]["FoodAllownce"].ToString() == "2")
                    {
                        lblFood.Text = " ( x )";
                        lblFood.ForeColor = System.Drawing.Color.Red;
                        txtFood.Enabled = false;
                    }
                    else
                    {
                        lblFood.Text = " ( % )";
                        lblFood.ForeColor = System.Drawing.Color.Black;
                        txtFood.MaxLength = 2;
                        txtFood.Enabled = true;
                    }

                    if (dt.Rows[0]["ConvenceAllownce"].ToString() == "1")
                    {
                        txtConvience.MaxLength = 5;
                        lblConvience.Text = " ( ৳ )";
                        lblConvience.ForeColor = System.Drawing.Color.DarkGreen;
                        txtBasic.Enabled = true;
                    }
                    else if (dt.Rows[0]["ConvenceAllownce"].ToString() == "2")
                    {
                        lblConvience.Text = " ( x )";
                        lblConvience.ForeColor = System.Drawing.Color.Red;
                        txtConvience.Enabled = false;
                    }
                    else
                    {
                        lblConvience.Text = " ( % )";
                        lblConvience.ForeColor = System.Drawing.Color.Black;
                        txtConvience.MaxLength = 2;
                        txtConvience.Enabled = true;
                    }

                    if (dt.Rows[0]["HouseRent"].ToString() == "1")
                    {
                        txtHouse.MaxLength = 5;
                        lblHouse.Text = " ( ৳ )";
                        lblHouse.ForeColor = System.Drawing.Color.DarkGreen;
                        txtHouse.Enabled = true;
                    }
                    else if (dt.Rows[0]["HouseRent"].ToString() == "2")
                    {
                        lblHouse.Text = " ( x )";
                        lblHouse.ForeColor = System.Drawing.Color.Red;
                        txtHouse.Enabled = false;
                    }
                    else
                    {
                        lblHouse.Text = " ( % )";
                        lblHouse.ForeColor = System.Drawing.Color.Black;
                        txtHouse.MaxLength = 2;
                        txtHouse.Enabled = true;
                    }
                    if (dt.Rows[0]["TechnicalAllowance"].ToString() == "1")
                    {
                        txtTechnical.MaxLength = 5;
                        lblTechnical.Text = " ( ৳ )";
                        lblTechnical.ForeColor = System.Drawing.Color.DarkGreen;
                        txtTechnical.Enabled = true;
                    }
                    else if (dt.Rows[0]["TechnicalAllowance"].ToString() == "2")
                    {
                        lblTechnical.Text = " ( x )";
                        lblTechnical.ForeColor = System.Drawing.Color.Red;
                        txtTechnical.Enabled = false;
                    }
                    else
                    {
                        lblTechnical.Text = " ( % )";
                        lblTechnical.ForeColor = System.Drawing.Color.Black;
                        txtTechnical.MaxLength = 2;
                        txtTechnical.Enabled = true;
                    }

                    if (dt.Rows[0]["OthersAllowance"].ToString() == "1")
                    {
                        txtOthers.MaxLength = 5;
                        lblOthers.Text = " ( ৳ )";
                        lblOthers.ForeColor = System.Drawing.Color.DarkGreen;
                        txtOthers.Enabled = true;
                    }
                    else if (dt.Rows[0]["OthersAllowance"].ToString() == "2")
                    {
                        lblOthers.Text = " ( x )";
                        lblOthers.ForeColor = System.Drawing.Color.Red;
                        txtOthers.Enabled = false;
                    }
                    else
                    {
                        lblOthers.Text = " ( % )";
                        lblOthers.ForeColor = System.Drawing.Color.Black;
                        txtOthers.MaxLength = 2;
                        txtOthers.Enabled = true;
                    }

                    if (dt.Rows[0]["ProvidentFund"].ToString() == "1")
                    {
                        txtPF.MaxLength = 5;
                        lblPF.Text = " ( ৳ )";
                        lblPF.ForeColor = System.Drawing.Color.DarkGreen;
                        txtPF.Enabled = true;
                    }
                    else if (dt.Rows[0]["ProvidentFund"].ToString() == "2")
                    {
                        lblPF.Text = " ( x )";
                        lblPF.ForeColor = System.Drawing.Color.Red;
                        txtPF.Enabled = false;
                    }
                    else
                    {
                        lblPF.Text = " ( % )";
                        lblPF.ForeColor = System.Drawing.Color.Black;
                        txtPF.MaxLength = 2;
                        txtPF.Enabled = true;
                    }
                }
            }
            catch { }
        }
        protected void rblEmpType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetConstrant();
            }
            catch { }
        }

        private void SaveHRD_AllownceSetting()
        {
            try
            {
                string[] getColumns = { "AlCalId", "BasicAllowance", "MedicalAllownce", "FoodAllownce", "ConvenceAllownce", "HouseRent", "TechnicalAllowance", "PFAllowance", "OthersAllowance", "Year", "UserId" };
                string[] getValues = { ViewState["__AlCalId__"].ToString(),txtBasic.Text.Trim(),txtMedical.Text.Trim(),txtFood.Text.Trim(),txtConvience.Text.Trim(),
                                      txtHouse.Text.Trim(),txtTechnical.Text.Trim(),txtPF.Text.Trim(),txtOthers.Text.Trim(),DateTime.Now.ToString("yyyy"),Session["__GetUID__"].ToString()};
                if (SQLOperation.forSaveValue("HRD_AllownceSetting", getColumns, getValues, sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success->Successfully Set";
                    loadHRD_AllownceSetting(ddlCompanyList2.SelectedValue);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Update_HRD_AllownceSetting()
        {
            try
            {
                string[] getColumns = { "AlCalId", "BasicAllowance", "MedicalAllownce", "FoodAllownce", "ConvenceAllownce", "HouseRent", "TechnicalAllowance", "PFAllowance", "OthersAllowance", "Year", "UserId" };
                string[] getValues = { ViewState["__AlCalId__"].ToString(),txtBasic.Text.Trim(),txtMedical.Text.Trim(),txtFood.Text.Trim(),txtConvience.Text.Trim(),
                                      txtHouse.Text.Trim(),txtTechnical.Text.Trim(),txtPF.Text.Trim(),txtOthers.Text.Trim(),DateTime.Now.ToString("yyyy"),Session["__GetUID__"].ToString()};
                if (SQLOperation.forUpdateValue("HRD_AllownceSetting", getColumns, getValues, "AllownceId", ViewState["__AllownceId__"].ToString(), sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success->Successfully Set";
                    loadHRD_AllownceSetting(ddlCompanyList2.SelectedValue);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void loadHRD_AllownceSetting(string companyId)
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select has.AllownceId,has.AlCalId,has.BasicAllowance,has.MedicalAllownce,has.FoodAllownce,has.ConvenceAllownce,has.HouseRent,has.TechnicalAllowance,has.PFAllowance,has.OthersAllowance,Year,Format(has.EntryDate,'dd-MM-yyyy hh:MM:ss tt') as EntryDate,cs.SalaryType,et.EmpType,et.EmpTypeId from HRD_AllownceSetting as has inner join " +
                    "  Payroll_AllowanceCalculationSetting cs on has.AlCalId=cs.AlCalId inner join HRD_EmployeeType et on cs.EmpTypeId=et.EmpTypeId where cs.CalculationType='vattax' and cs.CompanyId='" + companyId + "'", dt);
                gvAllowanceAmountList.DataSource = dt;
                gvAllowanceAmountList.DataBind();


            }
            catch { }
        }

        private bool Allowance_Validation_Basket()
        {
            try
            {
                if (rblEmployeeType2.SelectedIndex == -1)
                {
                    lblMessage.InnerText = "error->Please select employee type";
                    rblEmployeeType2.Focus();
                    return false;
                }
                else return true;
            }
            catch { return false; }
        }

        protected void btnSave2_Click(object sender, EventArgs e)
        {
            if (Allowance_Validation_Basket())
            {
                if (btnSave2.Text == "Save") SaveHRD_AllownceSetting();
                else Update_HRD_AllownceSetting();
            }
        }


        protected void gvSalaryCalculationAmountList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Alter"))
                {
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    loadAllowanceCalculationAmountSettings(gvAllowanceAmountList.DataKeys[rIndex].Value.ToString(), rIndex);

                }
            }
            catch { }
        }

        private void loadAllowanceCalculationAmountSettings(string AllownceId, int rIndex)
        {
            try
            {
                txtBasic.Text = (gvAllowanceAmountList.Rows[rIndex].Cells[1].Text == "0") ? "" : gvAllowanceAmountList.Rows[rIndex].Cells[1].Text;
                txtMedical.Text = (gvAllowanceAmountList.Rows[rIndex].Cells[2].Text == "0") ? "" : gvAllowanceAmountList.Rows[rIndex].Cells[2].Text;
                txtFood.Text = (gvAllowanceAmountList.Rows[rIndex].Cells[3].Text == "0") ? "" : gvAllowanceAmountList.Rows[rIndex].Cells[3].Text;
                txtConvience.Text = (gvAllowanceAmountList.Rows[rIndex].Cells[4].Text == "0") ? "" : gvAllowanceAmountList.Rows[rIndex].Cells[4].Text;
                txtTechnical.Text = (gvAllowanceAmountList.Rows[rIndex].Cells[5].Text == "0") ? "" : gvAllowanceAmountList.Rows[rIndex].Cells[5].Text;
                txtHouse.Text = (gvAllowanceAmountList.Rows[rIndex].Cells[6].Text == "0") ? "" : gvAllowanceAmountList.Rows[rIndex].Cells[6].Text;
                txtOthers.Text = (gvAllowanceAmountList.Rows[rIndex].Cells[7].Text == "0") ? "" : gvAllowanceAmountList.Rows[rIndex].Cells[7].Text;
                txtPF.Text = (gvAllowanceAmountList.Rows[rIndex].Cells[8].Text == "0") ? "" : gvAllowanceAmountList.Rows[rIndex].Cells[8].Text;
                rblEmployeeType2.SelectedValue = gvAllowanceAmountList.DataKeys[rIndex].Values[2].ToString();
                lblSalaryType.Text = gvAllowanceAmountList.Rows[rIndex].Cells[11].Text;
                rblEmployeeType2.Enabled = false;
                ViewState["__AllownceId__"] = AllownceId;
                if (ViewState["__UpdateAction__"].Equals("0"))
                {
                    btnSave2.Enabled = false;
                    btnSave2.CssClass = "";
                }
                else
                {
                    btnSave2.Enabled = true;
                    btnSave2.CssClass = "Pbutton";
                }
                btnSave2.Text = "Update";
                SetConstrant();
            }
            catch (Exception ex)
            { }
        }

        private void ClearAmountBox()
        {
            try
            {
                txtBasic.Text = "";
                txtMedical.Text = "";
                txtFood.Text = "";
                txtConvience.Text = "";
                txtTechnical.Text = "";
                txtHouse.Text = "";
                txtOthers.Text = "";
                txtPF.Text = "";
                rblEmployeeType2.Enabled = true;
                lblSalaryType.Text = "";
                rblEmployeeType2.SelectedIndex = -1;
                if (ViewState["__WriteAction__"].Equals("0"))
                {
                    btnSave2.Enabled = false;
                    btnSave2.CssClass = "";
                }
                else
                {
                    btnSave2.Enabled = true;
                    btnSave2.CssClass = "Pbutton";
                }
                btnSave2.Text = "Save";
            }
            catch { }
        }

        protected void btnClear2_Click(object sender, EventArgs e)
        {
            ClearAmountBox();
        }
    }
}