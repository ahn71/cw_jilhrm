using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class EmpInfo_Index : System.Web.UI.Page
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
                        pEmployee.Visible = false;
                        pEmployeeList.Visible = false;
                        pEmployeeProfile.Visible = false;
                        pEmployeeListReport.Visible = false;
                        pSeperation.Visible = false;
                        pSeperationListReport.Visible = false;
                        pManPowerStatusReport.Visible = false;
                        pMonthlyManPowerReport.Visible = false;
                        pEmpContactListReport.Visible = false;
                        pEmpIDCardReport.Visible = false;
                        pBloodGroupReport.Visible = false;


                        DataTable dt = new DataTable();
                        dt = checkUserPrivilege.PanelWiseUserPrivilege(getCookies["__getUserId__"].ToString(), "2");
                        if (dt.Rows.Count > 0)
                        {
                            for (byte i = 0; i < dt.Rows.Count; i++)
                                switch (dt.Rows[i]["ModulePageName"].ToString())
                                {
                                    case "employee.aspx":
                                        pEmployee.Visible = true;
                                        pEmployeeList.Visible = true;
                                        break;
                                    case "employee_profile.aspx":
                                        pEmployeeProfile.Visible = true;
                                        break;
                                    case "employee_information.aspx":
                                        pEmployeeListReport.Visible = true;
                                        break;
                                    case "separation.aspx":
                                        pSeperation.Visible = true;
                                        break;
                                    case "seperation_sheet.aspx":
                                        pSeperationListReport.Visible = true;
                                        break;
                                    case "man_power_status.aspx":
                                        pManPowerStatusReport.Visible = true;
                                        break;
                                    case "monthly_manpower.aspx":
                                        pMonthlyManPowerReport.Visible = true;
                                        break;
                                    case "EmpContactReport.aspx":
                                        pEmpContactListReport.Visible = true;
                                        break;

                                    case "staff_id_card.aspx":
                                        pEmpIDCardReport.Visible = true;
                                        break;
                                    case "blood_group.aspx":
                                        pBloodGroupReport.Visible = true;
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