using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.vat_tax
{
    public partial class vat_tax_generation : System.Web.UI.Page
    {
        DataTable dt;
        DataTable dtRunningEmp;
        DataTable dtTaxSlapInfo;
        DataTable dtRebateSlapInfo;
        DataTable dtTaxFreeInfo;
        DataTable dtAllowanceInfo; 
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["OPERATION_PROGRESS"] = 0;

            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();               
                Session["__CID__"] = ddlCompanyList.SelectedValue;                
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                commonTask.loadTaxYears(ddlTaxYears,ddlCompanyList.SelectedValue);
                classes.Employee.LoadEmpCardNoWithNameByCompany(ddlEmpCardNo, ddlCompanyList.SelectedValue, "1,2", "Select For Individual Generation");
            }

            lblMessage.InnerText = "";
        }
        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {                
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__getUserId__"] = getUserId;
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForOnlyWriteAction(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "vat_tax_generation.aspx", ddlCompanyList, btnGenerate);  
            }
            catch { }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                //--------------this connection for old data-----------------------
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["local2"].ConnectionString);
                con.Open();
                //---------------------------------------
                string sqlquery = "";
                if (rbGenaratingType.SelectedValue == "0")
                    sqlquery = "select max(SN) as SN, EmpId,EmpPresentSalary,IncomeTax,Sex,DateOfBirth,PfMember    from v_Personnel_EmpCurrentStatus   where  EmpStatus in('1','8') AND ActiveSalary='true' AND IsActive='1' and EmpTypeId=2 AND CompanyId='" + ddlCompanyList.SelectedValue + "' group by EmpId,EmpPresentSalary,IncomeTax,Sex,DateOfBirth,PfMember  order by SN";                    
                else
                    sqlquery = "select max(SN) as SN, EmpId,EmpPresentSalary,IncomeTax,Sex,DateOfBirth ,PfMember   from v_Personnel_EmpCurrentStatus   where EmpStatus in('1','8') AND ActiveSalary='true' AND IsActive='1' and EmpTypeId=2 AND CompanyId='" + ddlCompanyList.SelectedValue + "' and EmpId='" + ddlEmpCardNo.SelectedValue + "' group by EmpId,EmpPresentSalary,IncomeTax,Sex,DateOfBirth,PfMember  order by SN";
                   // sqlDB.fillDataTable(sqlquery, dtRunningEmp = new DataTable());
                SqlDataAdapter da = new SqlDataAdapter(sqlquery, con);
                da.Fill(dtRunningEmp = new DataTable());
                con.Close();
                string taxpayer;
                if (dtRunningEmp.Rows.Count > 0)
                {
                    DataTable dtMinTax;
                    sqlDB.fillDataTable("select MinimumTax from VatTax_MinimumTax where IsActive=1", dtMinTax = new DataTable());
                    string[] TaxID = ddlTaxYears.SelectedValue.Split('|');
                    sqlDB.fillDataTable("select cs.CalculationType, has.BasicAllowance,has.MedicalAllownce,has.ConvenceAllownce,has.HouseRent,has.PFAllowance from HRD_AllownceSetting as has inner join " +
                    "  Payroll_AllowanceCalculationSetting cs on has.AlCalId=cs.AlCalId inner join HRD_EmployeeType et on cs.EmpTypeId=et.EmpTypeId where  cs.EmpTypeId=2 and cs.CompanyId='" + ddlCompanyList.SelectedValue + "'", dtAllowanceInfo = new DataTable());
                    sqlDB.fillDataTable("select ConvenceAllownce,HouseRent,MedicalAllownce from VatTax_TaxFreeAllowance ", dtTaxFreeInfo = new DataTable());
                    
                    sqlDB.fillDataTable("select ToTaka,IncomeTaxRate from VatTax_Rebatable_Rate where CompanyId='" + ddlCompanyList.SelectedValue + "'  order by RateOrder   ", dtRebateSlapInfo = new DataTable());

                    DataRow[] drVatTax = dtAllowanceInfo.Select("CalculationType='vattax'");
                    DataRow[] drSalary = dtAllowanceInfo.Select("CalculationType='salary'");
                    for (int i = 0; i < dtRunningEmp.Rows.Count; i++)
                    {
                        if (dtRunningEmp.Rows[i]["Sex"].ToString().Equals("Female"))
                            taxpayer = "1";
                        else
                            taxpayer = "0";
                        sqlDB.fillDataTable("select ToTaka,IncomeTaxRate from VatTax_Rate where CompanyId='" + ddlCompanyList.SelectedValue + "' and Taxpayer=" + taxpayer + "  order by RateOrder   ", dtTaxSlapInfo = new DataTable());
                        classes.Tax_Calculation.generateTaxCalculation(ViewState["__getUserId__"].ToString(), TaxID[0], TaxID[1], TaxID[2], TaxID[3], TaxID[4], ddlCompanyList.SelectedValue, dtRunningEmp.Rows[i]["EmpId"].ToString(), float.Parse(dtRunningEmp.Rows[i]["EmpPresentSalary"].ToString()), bool.Parse(dtRunningEmp.Rows[i]["PfMember"].ToString())
                             , int.Parse(drVatTax[0][1].ToString()), int.Parse(drSalary[0][1].ToString()), int.Parse(drVatTax[0][5].ToString()), int.Parse(drVatTax[0][2].ToString()), dtTaxSlapInfo, dtRebateSlapInfo, dtTaxFreeInfo, float.Parse(dtRunningEmp.Rows[i]["IncomeTax"].ToString()), int.Parse(dtMinTax.Rows[0]["MinimumTax"].ToString()));
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ProcessingEnd(" + dtRunningEmp.Rows.Count.ToString() + ");", true);
                    }
                        
                   
                }
            }
            catch { }
             
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            commonTask.loadTaxYears(ddlTaxYears, ddlCompanyList.SelectedValue);
            classes.Employee.LoadEmpCardNoWithNameByCompany(ddlEmpCardNo, ddlCompanyList.SelectedValue, "2", "Select For Individual Generation");
        }
 
    }
}