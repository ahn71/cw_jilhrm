using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.vat_tax
{
    public partial class vat_tax_report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
               
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
                            

            }
        }
        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];

                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CShortName__"] = getCookies["__CShortName__"].ToString();


                //------------load privilege setting inof from db------
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "vat_tax_report.aspx", ddlCompanyName, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                commonTask.LoadDepartmentByCompanyInListBox(ViewState["__CompanyId__"].ToString(), lstAll);
                commonTask.loadTaxYears(ddlSelectMonth, ViewState["__CompanyId__"].ToString());
                //-----------------------------------------------------


            }
            catch { }
        }
       

        protected void rblGenerateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                if (!rblGenerateType.SelectedItem.Text.Equals("All"))
                {
                    txtEmpCardNo.Enabled = true;
                    pnl1.Enabled = false;
                                     
                    pnl1.Visible = false;
                    txtEmpCardNo.Focus();

                }
                else
                {
                    txtEmpCardNo.Enabled = false;
                    pnl1.Enabled = true;
                    
                    
                    pnl1.Visible = true;
                   
                }
              
            }
            catch { }
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            if (ddlSelectMonth.SelectedValue == "0")
            { lblMessage.InnerText = "warning->Please select tax years!"; ddlSelectMonth.Focus(); return; }
            if (rblGenerateType.SelectedItem.Text.Equals("All") && lstSelected.Items.Count < 1) { lblMessage.InnerText = "warning->Please select any Department"; lstSelected.Focus(); return; }
            if (!rblGenerateType.SelectedItem.Text.Equals("All") && txtEmpCardNo.Text.Trim().Length < 4) { lblMessage.InnerText = "warning->Please type valid Card No!(Minimum last 4 digit.)"; txtEmpCardNo.Focus(); return; }
            if (rblReportType.SelectedValue == "IndividualTaxCalculation")
                IndividualTaxCalculation();
            else if (rblReportType.SelectedValue == "IndividualIncome")
                IndividualIncome();
            else if (rblReportType.SelectedValue == "IndividualInvestment")
                IndividualInvestment();
            else
                IncomeTaxSheet();

        }
        private void IndividualTaxCalculation()
        {
            try
            {
                string CompanyList = "";               
                string DepartmentList = "";
                CompanyList = (ddlCompanyName.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                string[] TaxId = ddlSelectMonth.SelectedValue.Split('|');
                string getSQLCMD;
                DataTable dt = new DataTable();
                DataTable dt1;
                DataTable dt2;
                if (rblGenerateType.SelectedItem.Text.Equals("All"))
                {
                    
                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                    getSQLCMD = "SELECT CompanyId,CompanyName,Address,Type,substring(EmpCardNo,10,6) as EmpCardNo, EmpId, BasicSalary, Bonus, PF_Amount, EL_Amount, OthersIncome, Total_Taxable_Income, OthersAllowance, ConveyanceTaxFree, " +
                          " HouseRent, HouseRentTaxFree, MadicalTaxFree, TotalTax, Rebatable, NetPayableTax, PerMonthTax, EmpEmprContribution, TaxYears, EmpName," +
                          " DsgName, TaxId" +
                          " FROM   v_VatTax_IncomeTax " +
                          " where " +
                          " CompanyId  in(" + CompanyList + ")  and TaxId=" + TaxId[0] + " and DptId " + DepartmentList + " " +
                          " ";
                        
                 


                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                    }

                }
                else
                {

                    getSQLCMD = "SELECT CompanyId,CompanyName,Address,Type,substring(EmpCardNo,10,6) as EmpCardNo, EmpId, BasicSalary, Bonus, PF_Amount, EL_Amount, OthersIncome, Total_Taxable_Income, OthersAllowance, ConveyanceTaxFree, " +
                           " HouseRent, HouseRentTaxFree, MadicalTaxFree, TotalTax, Rebatable, NetPayableTax, PerMonthTax, EmpEmprContribution, TaxYears, EmpName," +
                           " DsgName, TaxId" +
                           " FROM   v_VatTax_IncomeTax " +
                           " where " +
                           " CompanyId  in(" + CompanyList + ")  and TaxId=" + TaxId[0] + " and EmpCardNo like'%" + txtEmpCardNo.Text + "' " +
                           " ";
                   

                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                    }                   


                }
                if (rblGenerateType.SelectedItem.Text.Equals("All"))
                {
                    sqlDB.fillDataTable("select * from VatTax_TaxCalculation where TaxId=" + TaxId[0] + " and EmpId in(select EmpId FROM   v_VatTax_IncomeTax where CompanyId  in(" + CompanyList + ")  and TaxId=" + TaxId[0] + " and DptId " + DepartmentList + "  )", dt1 = new DataTable());
                    sqlDB.fillDataTable("select * from VatTax_RebateCalculation where TaxId=" + TaxId[0] + " and EmpId in(select EmpId FROM   v_VatTax_IncomeTax where CompanyId  in(" + CompanyList + ")  and TaxId=" + TaxId[0] + " and DptId " + DepartmentList + " )", dt2 = new DataTable());
                }
                else 
                {
                    sqlDB.fillDataTable("select * from VatTax_TaxCalculation where TaxId=" + TaxId[0] + " and EmpId in(" + dt.Rows[0]["EmpId"].ToString() + ")", dt1 = new DataTable());
                    sqlDB.fillDataTable("select * from VatTax_RebateCalculation where TaxId=" + TaxId[0] + " and EmpId in(" + dt.Rows[0]["EmpId"].ToString() + ")", dt2 = new DataTable());
                }
               
                Session["__IndivisualTaxInfo__"] = dt;                
                Session["__TaxCalculationInfo__"] = dt1;                
                Session["__RebateCalculationInfo__"] = dt2;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=IndivisualTaxInfo');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }

        private void IndividualIncome()
        {
            try
            {
                string CompanyList = "";
                string DepartmentList = "";
                CompanyList = (ddlCompanyName.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                string[] TaxId = ddlSelectMonth.SelectedValue.Split('|');
                string getSQLCMD;
                DataTable dt = new DataTable();          
                if (rblGenerateType.SelectedItem.Text.Equals("All"))
                {

                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                    getSQLCMD = "SELECT CompanyId,CompanyName,Address,Type,substring(EmpCardNo,10,6) as EmpCardNo, EmpId, BasicSalary, Bonus, PF_Amount, EL_Amount, OthersIncome, Total_Taxable_Income, OthersAllowance, Conveyance, ConveyanceTaxFree, " +
                           " HouseRent, HouseRentTaxFree,Madical, MadicalTaxFree, TotalTax, Rebatable, NetPayableTax, PerMonthTax, EmpEmprContribution, TaxYears, EmpName," +
                          " DsgName, TaxId,Tin" +
                          " FROM   v_VatTax_IncomeTax " +
                          " where " +
                          " CompanyId  in(" + CompanyList + ")  and TaxId=" + TaxId[0] + " and DptId " + DepartmentList + " " +
                          " ";




                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                    }

                }
                else
                {

                    getSQLCMD = "SELECT CompanyId,CompanyName,Address,Type,substring(EmpCardNo,10,6) as EmpCardNo, EmpId, BasicSalary, Bonus, PF_Amount, EL_Amount, OthersIncome, Total_Taxable_Income, OthersAllowance,Conveyance, ConveyanceTaxFree, " +
                           " HouseRent, HouseRentTaxFree,Madical, MadicalTaxFree, TotalTax, Rebatable, NetPayableTax, PerMonthTax, EmpEmprContribution, TaxYears, EmpName," +
                           " DsgName, TaxId,Tin" +
                           " FROM   v_VatTax_IncomeTax " +
                           " where " +
                           " CompanyId  in(" + CompanyList + ")  and TaxId=" + TaxId[0] + " and EmpCardNo like'%" + txtEmpCardNo.Text + "' " +
                           " ";


                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                    }


                }               
                Session["__IndivisualIncomeInfo__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=IndivisualIncomeInfo');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
        private void IndividualInvestment()
        {
            try
            {
                string CompanyList = "";
                string DepartmentList = "";
                CompanyList = (ddlCompanyName.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                string[] TaxId = ddlSelectMonth.SelectedValue.Split('|');
                string getSQLCMD;
                DataTable dt = new DataTable();
                if (rblGenerateType.SelectedItem.Text.Equals("All"))
                {

                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                    getSQLCMD = "select EmpId, CompanyId, CompanyName, Address, EmpName, Rebatable, EmpEmprContribution, Tin, MaxInvestmentAmount,TotalInvestmentAmount, RebatableInvestment, LifeInsurPremium,ContrDepositPensionScheme, InvstInApprSavingsCertificate,InvstInApprDebentureOrDebentureStock_StockOrShares, ContrPFWhichPFAct1925Applies, ContrSuperAnnuationFund, ContrBenevolentFundAndGroupInsurPremium,ContrZakatFund, Others " +                         
                          " FROM   v_VatTax_IncomeTax " +
                          " where " +
                          " CompanyId  in(" + CompanyList + ")  and TaxId=" + TaxId[0] + " and DptId " + DepartmentList + " " +
                          " ";




                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                    }

                }
                else
                {

                    getSQLCMD = "select EmpId, CompanyId, CompanyName, Address, EmpName, Rebatable, EmpEmprContribution, Tin, MaxInvestmentAmount,TotalInvestmentAmount, RebatableInvestment, LifeInsurPremium,ContrDepositPensionScheme, InvstInApprSavingsCertificate,InvstInApprDebentureOrDebentureStock_StockOrShares, ContrPFWhichPFAct1925Applies, ContrSuperAnnuationFund, ContrBenevolentFundAndGroupInsurPremium,ContrZakatFund, Others " +
                          " FROM   v_VatTax_IncomeTax " +                         
                           " where " +
                           " CompanyId  in(" + CompanyList + ")  and TaxId=" + TaxId[0] + " and EmpCardNo like'%" + txtEmpCardNo.Text + "' " +
                           " ";


                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                    }


                }
                Session["__IndividualInvestment__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=IndividualInvestment');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
        private void IncomeTaxSheet()
        {
            try
            {
                string CompanyList = "";
                string DepartmentList = "";
                CompanyList = (ddlCompanyName.SelectedValue.Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                string[] TaxId = ddlSelectMonth.SelectedValue.Split('|');
                string getSQLCMD;
                DataTable dt = new DataTable();         
                if (rblGenerateType.SelectedItem.Text.Equals("All"))
                {

                    DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                    getSQLCMD = "SELECT CompanyId, DptId, CompanyName, Address, BasicSalary, Bonus, PF_Amount, EL_Amount, OthersIncome, Total_Taxable_Income, ConveyanceTaxFree, HouseRentTaxFree, " +
                          " MadicalTaxFree, TotalTax, Rebatable, NetPayableTax, EmpName, DsgName, PayableTax, PaidTax, Tin" +                         
                          " FROM   v_VatTax_IncomeTax " +
                          " where " +
                          " CompanyId  in(" + CompanyList + ")  and TaxId=" + TaxId[0] + " and DptId " + DepartmentList + " " +
                          " ";
                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                    }

                }
                else
                {

                    getSQLCMD = "SELECT CompanyId, DptId, CompanyName, Address, BasicSalary, Bonus, PF_Amount, EL_Amount, OthersIncome, Total_Taxable_Income, ConveyanceTaxFree, HouseRentTaxFree, " +
                         " MadicalTaxFree, TotalTax, Rebatable, NetPayableTax, EmpName, DsgName, PayableTax, PaidTax, Tin" +
                         " FROM   v_VatTax_IncomeTax " +
                           " where " +
                           " CompanyId  in(" + CompanyList + ")  and TaxId=" + TaxId[0] + " and EmpCardNo like'%" + txtEmpCardNo.Text + "' " +
                           " ";


                    sqlDB.fillDataTable(getSQLCMD, dt);
                    if (dt.Rows.Count == 0)
                    {
                        lblMessage.InnerText = "warning->Sorry any record are not founded"; return;
                    }


                }  

                Session["__IncomeTaxSheet__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=IncomeTaxSheet');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                string CompanyId = (ddlCompanyName.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue.ToString();
                classes.commonTask.LoadDepartmentByCompanyInListBox(CompanyId, lstAll);               
                commonTask.loadTaxYears(ddlSelectMonth, CompanyId);
            }
            catch { }
        } 

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveItem(lstAll, lstSelected);

        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveAll(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            classes.commonTask.AddRemoveAll(lstSelected, lstAll);
        }
    }
}