using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP
{
    public partial class leave_default : System.Web.UI.Page
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
                        pLeaveConfig.Visible = false;
                        pHolidaySetup.Visible = false;
                        pLeaveApplication.Visible = false;
                        pShortLeave.Visible = false;
                        pLeaveApporval.Visible = false;
                        pShortLeaveApproval.Visible = false;
                        pLeaveList.Visible = false;
                        pLeaveBalanceReport.Visible = false;
                        pOfficialPurposeLeave.Visible = false;
                        pLeaveYearlyReport.Visible = false;
                        pELGenerate.Visible = false;
                        pELReport.Visible = false;

                        DataTable dt = new DataTable();
                        if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()) == "User")
                        {
                            pLeaveApplication.Visible = true;
                            pLeaveList.Visible = true;
                            pLeaveYearlyReport.Visible = true;
                            //  dt = checkUserPrivilege.PanelWiseUserPrivilegeByUserType(getCookies["__getUserType__"].ToString(), "3");
                        }
                        else { 
                            dt = checkUserPrivilege.PanelWiseUserPrivilege(getCookies["__getUserId__"].ToString(), "3");
                        if (dt.Rows.Count > 0)
                        {
                            for (byte i = 0; i < dt.Rows.Count; i++)
                                switch (dt.Rows[i]["ModulePageName"].ToString())
                                {
                                    case "LeaveConfig.aspx":
                                        pLeaveConfig.Visible = true;
                                        break;
                                    case "holyday.aspx":
                                        pHolidaySetup.Visible = true;
                                        break;
                                    case "aplication.aspx":
                                        pLeaveApplication.Visible = true;
                                        pLeaveList.Visible = true;
                                        break;
                                    case "short_leave.aspx":
                                        pShortLeave.Visible = true;
                                        break;
                                    case "for_approve_leave_list.aspx":
                                        pLeaveApporval.Visible = true;
                                        break;
                                    case "for_approve_shortleave_list.aspx":
                                        pShortLeaveApproval.Visible = true;
                                        break;                                        
                                    case "all_leave_list.aspx":
                                        pLeaveList.Visible = true;
                                        break;
                                    case "leave_balance_report.aspx":
                                        pLeaveBalanceReport.Visible = true;
                                        break;
                                    case "company_purpose_leave_report.aspx":
                                        pOfficialPurposeLeave.Visible = true;
                                        break;
                                    case "yearly_leaveStatus_report.aspx":
                                        pLeaveYearlyReport.Visible = true;
                                        break;
                                    case "generation.aspx":
                                        pELGenerate.Visible = true;
                                        break;
                                    case "earn_leave_Report.aspx":
                                        pELReport.Visible = true;
                                        break;                     
                                    default:
                                        break;

                                }
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