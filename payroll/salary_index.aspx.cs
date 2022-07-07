using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.payroll
{
    public partial class salary_index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {


                if (!IsPostBack)
                {
                    HttpCookie getCookies = Request.Cookies["userInfo"];
                    if (getCookies == null || getCookies.Value == "")
                    {
                        Response.Redirect("~/ControlPanel/Login.aspx");

                    }
                    if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()) != "Master Admin" && ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()) != "Viewer")
                    {
                        pSalaryEntry.Visible = false;
                        pAllowanceCalculation.Visible = false;
                        pSalaryGenerate.Visible = false;
                        pSeperationGenerate.Visible = false;
                        pSalarySheet.Visible = false;
                        pSalarySummary.Visible = false;
                        pOverTimeSheet.Visible = false;
                        pSeperationSalarySheet.Visible = false;
                        pPaySlip.Visible = false;
                        pSeperationFinalBillSheet.Visible = false;
                        pSalaryFlow.Visible = false;
                        pSalaryStructure.Visible = false;
                        pPromotion.Visible = false;
                        pPromotionListReport.Visible = false;
                        pIncrement.Visible = false;
                        pIncrementListReport.Visible = false;
                        pPunishmentOtherPay.Visible = false;
                        pLeaveDeductionSetting.Visible = false;
                        DataTable dt = new DataTable();
                        dt = checkUserPrivilege.PanelWiseUserPrivilege(getCookies["__getUserId__"].ToString(), "6");
                        if (dt.Rows.Count > 0)
                        {
                            for (byte i = 0; i < dt.Rows.Count; i++)
                                switch (dt.Rows[i]["ModulePageName"].ToString())
                                {
                                    case "payroll_entry_panel.aspx":
                                        pSalaryEntry.Visible = true;
                                        break;
                                    case "allowance_calculation_settings.aspx":
                                        pAllowanceCalculation.Visible = true;
                                        break;
                                    case "payroll_generation.aspx":
                                        pSalaryGenerate.Visible = true;
                                        pPunishmentOtherPay.Visible = true;
                                        pLeaveDeductionSetting.Visible = true;
                                        break;
                                    case "separation_generation.aspx":
                                        pSeperationGenerate.Visible = true;
                                        break;
                                    case "salary_sheet_Report.aspx":
                                        pSalarySheet.Visible = true;
                                        break;
                                    case "summary_of_salary.aspx":
                                        pSalarySummary.Visible = true;
                                        break;
                                    case "ot_payment_sheet.aspx":
                                        pOverTimeSheet.Visible = true;
                                        break;
                                    case "pay_slip.aspx":
                                        pPaySlip.Visible = true;
                                        break;
                                    case "separation_pmt_sheet.aspx":
                                        pSeperationSalarySheet.Visible = true;
                                        break;

                                    case "final_bill_payment_sheet.aspx":
                                        pSeperationFinalBillSheet.Visible = true;
                                        break;
                                    case "monthly_salary_flow.aspx":
                                        pSalaryFlow.Visible = true;
                                        break;
                                    case "CurrentSalaryStructure.aspx":
                                        pSalaryStructure.Visible = true;
                                        break;
                                    case "promotion.aspx":
                                        pPromotion.Visible = true;
                                        break;
                                    case "promotion_sheet.aspx":
                                        pPromotionListReport.Visible = true;
                                        break;
                                    case "salary_increment.aspx":
                                        pIncrement.Visible = true;
                                        break;
                                    case "increment_sheet.aspx":
                                        pIncrementListReport.Visible = true;
                                        break;
                                    case "Punishment_OthersPay.aspx":
                                        pPunishmentOtherPay.Visible = true;
                                        break;
                                    default:
                                        break;

                                }
                        }
                    }
                }




            }
            catch (Exception ex)
            {
                Response.Redirect("~/ControlPanel/Login.aspx");
            }

        }
    }
}