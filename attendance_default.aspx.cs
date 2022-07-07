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
    public partial class attendance_default : System.Web.UI.Page
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
                        pMonthSetup.Visible = false;
                        pShrinkData.Visible = false;
                        pManuallyCount.Visible = false;
                        pAttendanceList.Visible = false;
                        pAttSummary.Visible = false;
                        pAttInOutReport.Visible = false;
                        pAttManualReport.Visible = false;
                        pAttMonthlyStatus.Visible = false;
                        pAttManpowerWise.Visible = false;
                        pOverTimeReport.Visible = false;
                        pOverTimeReport.Visible = false;

                        pOutDuty.Visible = false;
                        pOutDutyReport.Visible = false;
                        DataTable dt = new DataTable();
                        if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()) == "User")
                        {
                            // dt = checkUserPrivilege.PanelWiseUserPrivilegeByUserType(getCookies["__getUserType__"].ToString(), "4");
                            pAttInOutReport.Visible = true;
                            pAttMonthlyStatus.Visible = true;
                            pOutDuty.Visible = true;
                            pOutDutyList.Visible = true;
                            pOutDutyReport.Visible = true;
                        }
                        else
                        {

                        
                        dt = checkUserPrivilege.PanelWiseUserPrivilege(getCookies["__getUserId__"].ToString(), "4");
                        if (dt.Rows.Count > 0)
                        {
                            for (byte i = 0; i < dt.Rows.Count; i++)
                                switch (dt.Rows[i]["ModulePageName"].ToString())
                                {
                                    case "monthly_setup.aspx":
                                        pMonthSetup.Visible = true;
                                        break;
                                    case "import_data_ahg.aspx":
                                        pShrinkData.Visible = true;
                                        break;
                                    case "attendance.aspx":
                                        pManuallyCount.Visible = true;
                                        break;
                                    case "attendance_list.aspx":
                                        pAttendanceList.Visible = true;
                                        break;
                                    case "attendance_summary.aspx":
                                        pAttSummary.Visible = true;                                      
                                        break;
                                    case "daily_movement.aspx":
                                        pAttInOutReport.Visible = true;
                                        break;
                                    case "daily_manualAttendance_report.aspx":
                                        pAttManualReport.Visible = true;
                                        break;
                                    case "monthly_in_out_report.aspx":
                                        pAttMonthlyStatus.Visible = true;
                                        break;

                                    case "attendance_summary_manpower.aspx":
                                        pAttManpowerWise.Visible = true;
                                        break;
                                    case "overtime_report.aspx":
                                        pOverTimeReport.Visible = true;
                                        break;
                                    case "aplication.aspx":
                                        pOutDuty.Visible = true;
                                        pOutDutyReport.Visible = true;
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