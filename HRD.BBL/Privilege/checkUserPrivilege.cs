using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//using HRD.BBL.Privilege;


namespace SigmaERP.classes
{
    public static  class checkUserPrivilege
    {
        //department.aspx,designation.aspx,shift_config.aspx,line_config.aspx,CompanyInfo.aspx,separation.aspx,LeaveConfig.aspx,holyday.aspx
        // ShiftManagment.aspx,shift_roster_extend.aspx
        public static string[] checkUserPrivilegeForSettigs(string CompanyId, string UserID, string UserType, string PageName, DropDownList ddlCompany, GridView gv, Button btnSave) 
        {
            string [] AccessPermission=new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch  (UserType)
            {
                case "Super Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                    }
                    break;
                case "Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='"+PageName+"' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false)
                                && bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";
                                AccessPermission[1] = "0";
                                AccessPermission[2] = "0";
                                AccessPermission[3] = "0";
                                gv.Visible = false;
                                btnSave.Enabled = false;
                                btnSave.CssClass = "";
                                break;
                            }                   
                            if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[1] = "0";
                                btnSave.CssClass = "";
                                btnSave.Enabled = false;                          
                            }
                            if (bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false))                         
                                AccessPermission[2] = "0";
                             
                            if (bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))                          
                                AccessPermission[3] = "0";                              
                            
                            
                        }
                        else
                        {
                            AccessPermission[0] = "0";
                            AccessPermission[1] = "0";
                            AccessPermission[2] = "0";
                            AccessPermission[3] = "0";
                            gv.Visible = false;
                            btnSave.Enabled = false;
                            btnSave.CssClass = "";                            
                        }

                    }
                    break;
                case "Master Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                    }
                    break;
                case "Viewer":
                    {
                      classes.commonTask.LoadBranch(ddlCompany);                       
                        AccessPermission[1] = "0";
                        AccessPermission[2] = "0";
                        AccessPermission[3] = "0";
                        btnSave.CssClass = "";
                        btnSave.Enabled = false;
                               
                    }
                    break;
                case "User":
                    {
                        AccessPermission[0] = "0";
                        AccessPermission[1] = "0";
                        AccessPermission[2] = "0";
                        AccessPermission[3] = "0";
                        gv.Visible = false;
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                        break;
                    }
                default:
                    break;              
            }
            return AccessPermission;
        }
        //grade_config.aspx,qualification.aspx,religion.aspx,district_Config.aspx,Thana.aspx,allowancesetup.aspx,floorConfig.aspx,business_type.aspx
        
        public static string[] checkUserPrivilegeForSettigs(string UserID, string UserType, string PageName, GridView gv, Button btnSave)
        {
            string[] AccessPermission = new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch (UserType)
            {               
                case "Admin":
                    {
                       
                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='" + PageName + "' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false)
                                && bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";
                                AccessPermission[1] = "0";
                                AccessPermission[2] = "0";
                                AccessPermission[3] = "0";
                                gv.Visible = false;
                                btnSave.Enabled = false;
                                btnSave.CssClass = "";
                                break;
                            }
                            if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[1] = "0";
                                btnSave.CssClass = "";
                                btnSave.Enabled = false;
                            }
                            if (bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false))
                                AccessPermission[2] = "0";

                            if (bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                                AccessPermission[3] = "0";


                        }
                        else
                        {
                            AccessPermission[0] = "0";
                            AccessPermission[1] = "0";
                            AccessPermission[2] = "0";
                            AccessPermission[3] = "0";
                            gv.Visible = false;
                            btnSave.Enabled = false;
                            btnSave.CssClass = "";
                        }

                    }
                    break;              
                case "Viewer":
                    {                      
                        AccessPermission[1] = "0";
                        AccessPermission[2] = "0";
                        AccessPermission[3] = "0";
                        btnSave.CssClass = "";
                        btnSave.Enabled = false;

                    }
                    break;
                case "User": 
                    {
                        AccessPermission[0] = "0";
                        AccessPermission[1] = "0";
                        AccessPermission[2] = "0";
                        AccessPermission[3] = "0";
                        gv.Visible = false;
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                        break;
                    }
                   
                default:
                    break;
            }
            return AccessPermission;
        }
        //payroll_entry_panel.aspx
        public static string[] checkUserPrivilegeForpayrollentrypanel(string UserID, string UserType, string PageName, GridView gv, Button btnSave, string CompanyId, DropDownList ddlCompany, DropDownList ddlCompany2)
        {
            string[] AccessPermission = new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch (UserType)
            {
                case "Super Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        classes.commonTask.LoadBranch(ddlCompany2, CompanyId);
                    }
                    break;
                case "Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        classes.commonTask.LoadBranch(ddlCompany2, CompanyId);

                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='" + PageName + "' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false)
                                && bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";
                                AccessPermission[1] = "0";
                                AccessPermission[2] = "0";
                                AccessPermission[3] = "0";
                                gv.Visible = false;
                                btnSave.Enabled = false;
                                btnSave.CssClass = "";
                                break;
                            }
                            if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[1] = "0";
                                btnSave.CssClass = "";
                                btnSave.Enabled = false;
                            }
                            if (bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false))
                                AccessPermission[2] = "0";

                            if (bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                                AccessPermission[3] = "0";


                        }
                        else
                        {
                            AccessPermission[0] = "0";
                            AccessPermission[1] = "0";
                            AccessPermission[2] = "0";
                            AccessPermission[3] = "0";
                            gv.Visible = false;
                            btnSave.Enabled = false;
                            btnSave.CssClass = "";
                        }

                    }
                    break;
                case "Master Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                        classes.commonTask.LoadBranch(ddlCompany2);
                    }
                    break;    
                case "Viewer":
                    {
                        AccessPermission[1] = "0";
                        AccessPermission[2] = "0";
                        AccessPermission[3] = "0";
                        btnSave.CssClass = "";
                        btnSave.Enabled = false;
                        classes.commonTask.LoadBranch(ddlCompany);
                        classes.commonTask.LoadBranch(ddlCompany2);
                    }
                    break;
                default:
                    break;
            }
            return AccessPermission;
        }
        //pfentrypanel.aspx
        public static string[] checkUserPrivilegeForpfentrypanel(string UserID, string UserType, string PageName, GridView gv, GridView gv2, Button btnSave, string CompanyId, DropDownList ddlCompany, DropDownList ddlCompany2)
        {
            string[] AccessPermission = new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch (UserType)
            {
                case "Super Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        classes.commonTask.LoadBranch(ddlCompany2, CompanyId);
                    }
                    break;
                case "Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        classes.commonTask.LoadBranch(ddlCompany2, CompanyId);

                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='" + PageName + "' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false)
                                && bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";
                                AccessPermission[1] = "0";
                                AccessPermission[2] = "0";
                                AccessPermission[3] = "0";
                                gv.Visible = false;
                                gv2.Visible = false;
                                btnSave.Enabled = false;
                                btnSave.CssClass = "";
                                break;
                            }
                            if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[1] = "0";
                                btnSave.CssClass = "";
                                btnSave.Enabled = false;
                            }
                            if (bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false))
                                AccessPermission[2] = "0";

                            if (bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                                AccessPermission[3] = "0";


                        }
                        else
                        {
                            AccessPermission[0] = "0";
                            AccessPermission[1] = "0";
                            AccessPermission[2] = "0";
                            AccessPermission[3] = "0";
                            gv.Visible = false;
                            gv2.Visible = false;
                            btnSave.Enabled = false;
                            btnSave.CssClass = "";
                        }

                    }
                    break;
                case "Master Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                        classes.commonTask.LoadBranch(ddlCompany2);
                    }
                    break;
                case "Viewer":
                    {
                        AccessPermission[1] = "0";
                        AccessPermission[2] = "0";
                        AccessPermission[3] = "0";
                        btnSave.CssClass = "";
                        btnSave.Enabled = false;
                        classes.commonTask.LoadBranch(ddlCompany);
                        classes.commonTask.LoadBranch(ddlCompany2);
                    }
                    break;
                default:
                    break;
            }
            return AccessPermission;
        }
        //allowance_calculation_settings.aspx,
        public static string[] checkUserPrivilegeForSettigs(string UserID, string UserType, string PageName, GridView gv, GridView gv2, Button btnSave, Button btnSave2, DropDownList ddlCompany, string CompanyId,DropDownList ddlCompany2)
        {
            string[] AccessPermission = new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch (UserType)
            {
                case "Super Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        classes.commonTask.LoadBranch(ddlCompany2, CompanyId);
                    }
                    break;
                case "Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        classes.commonTask.LoadBranch(ddlCompany2, CompanyId);
                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='" + PageName + "' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false)
                                && bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";
                                AccessPermission[1] = "0";
                                AccessPermission[2] = "0";
                                AccessPermission[3] = "0";
                                gv.Visible = false;
                                btnSave.Enabled = false;
                                btnSave.CssClass = "";

                                gv2.Visible = false;
                                btnSave2.Enabled = false;
                                btnSave2.CssClass = "";
                                break;
                            }
                            if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[1] = "0";
                                btnSave.CssClass = "";
                                btnSave.Enabled = false;
                                btnSave2.Enabled = false;
                                btnSave2.CssClass = "";
                            }
                            if (bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false))
                                AccessPermission[2] = "0";

                            if (bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                                AccessPermission[3] = "0";


                        }
                        else
                        {
                            AccessPermission[0] = "0";
                            AccessPermission[1] = "0";
                            AccessPermission[2] = "0";
                            AccessPermission[3] = "0";
                            gv.Visible = false;
                            btnSave.Enabled = false;
                            btnSave.CssClass = "";


                            gv2.Visible = false;
                            btnSave2.Enabled = false;
                            btnSave2.CssClass = "";
                        }

                    }
                    break;
                case "Viewer":
                    {
                        AccessPermission[1] = "0";
                        AccessPermission[2] = "0";
                        AccessPermission[3] = "0";
                        btnSave.CssClass = "";
                        btnSave.Enabled = false;                    
                        btnSave2.Enabled = false;
                        btnSave2.CssClass = "";
                        classes.commonTask.LoadBranch(ddlCompany);
                        classes.commonTask.LoadBranch(ddlCompany2);
                    }
                    break;
                case "Master Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                        classes.commonTask.LoadBranch(ddlCompany2);
                    }
                    break;
                case "User":
                    {
                        AccessPermission[0] = "0";
                        AccessPermission[1] = "0";
                        AccessPermission[2] = "0";
                        AccessPermission[3] = "0";
                        gv.Visible = false;
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                        break;
                    }
                default:
                    break;
            }
            return AccessPermission;
        }
        // Employee.aspx,aplication.aspx
        public static string[] checkUserPrivilegeForSettigs(string CompanyId, string UserID, string UserType, string PageName, DropDownList ddlCompany, Button btnSave) 
        {
            string[] AccessPermission = new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch (UserType)
            {
                
                case "User":
                    classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                    break;
                case "Super Admin":
                   classes.commonTask.LoadBranch(ddlCompany, CompanyId);                    
                    break;               
                case "Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='" + PageName + "' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false)
                                && bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";
                                AccessPermission[1] = "0";
                                AccessPermission[2] = "0";
                                AccessPermission[3] = "0";
                            
                                btnSave.Enabled = false;
                                btnSave.CssClass = "";
                                break;
                            }
                            if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[1] = "0";
                                btnSave.CssClass = "";
                                btnSave.Enabled = false;
                            }
                            if (bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false))
                                AccessPermission[2] = "0";

                            if (bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                                AccessPermission[3] = "0";


                        }
                        else
                        {
                            AccessPermission[0] = "0";
                            AccessPermission[1] = "0";
                            AccessPermission[2] = "0";
                            AccessPermission[3] = "0";
                           
                            btnSave.Enabled = false;
                            btnSave.CssClass = "";
                        }

                    }
                    break;
                
                case "Master Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                    }
                    break;
                case "Viewer":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                        AccessPermission[1] = "0";
                        AccessPermission[2] = "0";
                        AccessPermission[3] = "0";
                        btnSave.CssClass = "";
                        btnSave.Enabled = false;

                    }
                    break;
                //case "User":
                //    {
                //        AccessPermission[0] = "0";
                //        AccessPermission[1] = "0";
                //        AccessPermission[2] = "0";
                //        AccessPermission[3] = "0";                      
                //        btnSave.Enabled = false;
                //        btnSave.CssClass = "";
                //        break;
                //    }
                default:
                    break;
            }
            return AccessPermission;
        }
        //employee_list.aspx,all_leave_list.aspx
        public static string[] checkUserPrivilegeForList(string CompanyId, string UserID, string UserType, string PageName, DropDownList ddlCompany,GridView gv, Button btnSearch) //employee_list.aspx
        {
            string[] AccessPermission = new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch (UserType)
            {
                case "User":
                    classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                    break;
                case "Super Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                    }
                    break;              
                case "Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='" + PageName + "' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false) 
                                && bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";                               
                                AccessPermission[2] = "0";
                                AccessPermission[3] = "0";
                                gv.Visible = false;
                                btnSearch.Enabled = false;
                                btnSearch.CssClass = "";
                               
                                break;
                            }                          
                            if (bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false))
                                AccessPermission[2] = "0";

                            if (bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                                AccessPermission[3] = "0";


                        }
                        else
                        {
                            AccessPermission[0] = "0";                          
                            AccessPermission[2] = "0";
                            AccessPermission[3] = "0";
                        }

                    }
                    break;
                case "Master Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                    }
                    break;
                case "Viewer":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);                       
                        AccessPermission[2] = "0";
                        AccessPermission[3] = "0";
                        gv.Visible = false;

                    }
                    break;
            
                default:
                    break;
            }
            return AccessPermission;
        }


        //employee_profile.aspx,employee_information.aspx,seperation_sheet.aspx,yearly_leaveStatus_report.aspx,company_purpose_leave_report,
        //leave_balance_report
        public static string[] checkUserPrivilegeForReport(string CompanyId, string UserID, string UserType, string PageName, DropDownList ddlCompany, HtmlGenericControl WarningMessage, HtmlTable tblGenerateType, Button btnPrint)
        {
            string[] AccessPermission = new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch (UserType)
            {
                case "User":
                    classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                    break;
                case "Super Admin":
                 classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                 break;
                case "Admin":               
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='" + PageName + "' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";
                                WarningMessage.Visible = true;
                                tblGenerateType.Visible = false;
                                WarningMessage.InnerText = "You Have Not Any Access Permission!";
                                btnPrint.Enabled = false;
                                btnPrint.CssClass = "";
                                break;
                            }


                        }
                        else
                        {
                            AccessPermission[0] = "0";
                            WarningMessage.Visible = true;
                            tblGenerateType.Visible = false;
                            WarningMessage.InnerText = "You Have Not Any Access Permission!";
                            btnPrint.Enabled = false;
                            btnPrint.CssClass = "";

                        }

                    }
                    break;
                case "Master Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                    }
                    break;
                case "Viewer":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);

                    }
                    break;
                default:
                    break;
            }
            return AccessPermission;
        }
        //increment_sheet.aspx
        public static string[] checkUserPrivilegeForReport(string CompanyId, string UserID, string UserType, string PageName, DropDownList ddlCompany, HtmlGenericControl WarningMessage, HtmlTable tblGenerateType, Button btnPrint, Button btnPrint2)
        {
            string[] AccessPermission = new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch (UserType)
            {
                case "Super Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                    }
                    break;
                case "Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='" + PageName + "' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";
                                WarningMessage.Visible = true;
                                tblGenerateType.Visible = false;
                                WarningMessage.InnerText = "You Have Not Any Access Permission!";
                                btnPrint.Enabled = false;
                                btnPrint.CssClass = "";
                                btnPrint2.Enabled = false;
                                btnPrint2.CssClass = "";
                                break;
                            }


                        }
                        else
                        {
                            AccessPermission[0] = "0";
                            WarningMessage.Visible = true;
                            tblGenerateType.Visible = false;
                            WarningMessage.InnerText = "You Have Not Any Access Permission!";
                            btnPrint.Enabled = false;
                            btnPrint.CssClass = "";
                            btnPrint2.Enabled = false;
                            btnPrint2.CssClass = "";

                        }

                    }
                    break;
                case "Master Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                    }
                    break;
                case "Viewer":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);

                    }
                    break;
                default:
                    break;
            }
            return AccessPermission;
        }
        //attendance_summary
        public static string[] checkUserPrivilegeForReport(string CompanyId, string UserID, string UserType, string PageName, DropDownList ddlCompany, HtmlGenericControl WarningMessage, Button btnPrint)
        {
            string[] AccessPermission = new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch (UserType)
            {
                case "Super Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                    }
                    break;
                case "Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='" + PageName + "' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";
                                WarningMessage.Visible = true;                                
                                WarningMessage.InnerText = "You Have Not Any Access Permission!";
                                btnPrint.Enabled = false;
                                btnPrint.CssClass = "";
                                break;
                            }


                        }
                        else
                        {
                            AccessPermission[0] = "0";
                            WarningMessage.Visible = true;                           
                            WarningMessage.InnerText = "You Have Not Any Access Permission!";
                            btnPrint.Enabled = false;
                            btnPrint.CssClass = "";

                        }

                    }
                    break;
                case "Master Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                    }
                    break;
                case "Viewer":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);

                    }
                    break;
                default:
                    break;
            }
            return AccessPermission;
        }
        //import_data_ahg,separation_generation,pf_calculation
        public static string[] checkUserPrivilegeForOnlyWriteAction(string CompanyId, string UserID, string UserType, string PageName, DropDownList ddlCompany,  Button btnSave)
        {
            string[] AccessPermission = new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch (UserType)
            {
                case "Super Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                    }
                    break;
                case "Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='" + PageName + "' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";                                
                                btnSave.Enabled = false;
                                btnSave.CssClass = "";
                                break;
                            }


                        }
                        else
                        {
                            AccessPermission[0] = "0";
                            btnSave.Enabled = false;
                            btnSave.CssClass = "";

                        }

                    }
                    break;
                case "Master Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                    }
                    break;
                case "Viewer":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);

                    }
                    break;
                default:
                    break;
            }
            return AccessPermission;
        }

        //payroll_generation.aspx,payroll_generation_aht.aspx
        public static string[] checkUserPrivilegeForOnlyWriteAction(string CompanyId, string UserID, string UserType, string PageName, DropDownList ddlCompany, Button btnSave, Button btnSave2)
        {
            string[] AccessPermission = new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch (UserType)
            {
                case "Super Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                    }
                    break;

                case "Admin":
                case "User":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='" + PageName + "' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";
                                btnSave.Enabled = false;
                                btnSave.CssClass = "";
                                btnSave2.Enabled = false;
                                btnSave2.CssClass = "";
                                break;
                            }


                        }
                        else
                        {
                            AccessPermission[0] = "0";
                            btnSave.Enabled = false;
                            btnSave.CssClass = "";
                            btnSave2.Enabled = false;
                            btnSave2.CssClass = "";

                        }

                    }
                    break;
                case "Master Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                    }
                    break;
                case "Viewer":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);

                    }
                    break;
                default:
                    break;
            }
            return AccessPermission;
        }

        public static string[] checkUserPrivilegeForOnlyWriteAction(string CompanyId, string UserID, string UserType, string PageName, DropDownList ddlCompany, GridView gv)
        {
            string[] AccessPermission = new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch (UserType)
            {
                case "Super Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                    }
                    break;
                case "Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='" + PageName + "' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";
                                AccessPermission[1] = "0";
                                gv.Visible = false;                               
                                break;
                            }
                            if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                            {                                
                                AccessPermission[1] = "0";                               
                                break;
                            }


                        }
                        else
                        {
                            AccessPermission[0] = "0";
                            AccessPermission[1] = "0";
                            gv.Visible = false; 
                        }

                    }
                    break;
                case "Master Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                    }
                    break;
                case "Viewer":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);

                    }
                    break;
                default:
                    break;
            }
            return AccessPermission;
        }

        //ShiftManageRemove.aspx
        public static string[] checkUserPrivilegeForOnlyDeleteAction(string CompanyId, string UserID, string UserType, string PageName, DropDownList ddlCompany,GridView gv, Button btnDelete)
        {
            string[] AccessPermission = new string[4];
            AccessPermission[0] = "1";//Read
            AccessPermission[1] = "1";//Write
            AccessPermission[2] = "1";//Upadte
            AccessPermission[3] = "1";//Delete
            switch (UserType)
            {
                case "Super Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                    }
                    break;
                case "Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany, CompanyId);
                        DataTable dt = new DataTable();
                        sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='" + PageName + "' and UserId=" + UserID + "", dt);
                        if (dt.Rows.Count > 0)
                        {
                            if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false) && bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                            {
                                AccessPermission[0] = "0";
                                AccessPermission[3] = "0";
                                gv.Visible = false;
                                btnDelete.Enabled = false;
                                btnDelete.CssClass = "";
                                break;
                            }                                
                            if (bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                            {                                
                                AccessPermission[3] = "0";
                                btnDelete.Enabled = false;
                                btnDelete.CssClass = "";
                                break;
                            }


                        }
                        else
                        {
                            AccessPermission[0] = "0";
                            AccessPermission[3] = "0";
                            gv.Visible = false;
                            btnDelete.Enabled = false;
                            btnDelete.CssClass = "";
                        }

                    }
                    break;
                case "Master Admin":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);
                    }
                    break;
                case "Viewer":
                    {
                        classes.commonTask.LoadBranch(ddlCompany);

                    }
                    break;
                default:
                    break;
            }
            return AccessPermission;
        }
        public static void PrivilegeByModule(string UserType, string UserId, Panel Settings, Panel Personnel, Panel Leave, Panel Attendance, Panel Payroll, Panel Tools)
        {
            DataTable dt;
            if (UserType == "Lf/wQdbBDMw=")// for user 
            {
                Settings.Visible = false;
                Personnel.Visible = false;                
                Payroll.Visible = false;
                Tools.Visible = false;

                Leave.Visible = true;
                Attendance.Visible = true;
            }
            else 
            {           
            sqlDB.fillDataTable("select Settings,Personnel,Leave,Attendance,Payroll,Tools from UserPrivilegeByModule where  UserId=" + UserId + "", dt=new DataTable());
            if (dt.Rows.Count > 0) 
            {
                if(bool.Parse(dt.Rows[0]["Settings"].ToString()))
                    Settings.Visible = true;
                else
                    Settings.Visible = false;

                if (bool.Parse(dt.Rows[0]["Personnel"].ToString()))
                    Personnel.Visible = true;
                else
                    Personnel.Visible = false;

                if (bool.Parse(dt.Rows[0]["Leave"].ToString()))
                    Leave.Visible = true;
                else
                    Leave.Visible = false;

                if (bool.Parse(dt.Rows[0]["Attendance"].ToString()))
                    Attendance.Visible = true;
                else
                    Attendance.Visible = false;

                if (bool.Parse(dt.Rows[0]["Payroll"].ToString()))
                    Payroll.Visible = true;
                else
                    Payroll.Visible = false;

                if (bool.Parse(dt.Rows[0]["Tools"].ToString()))
                    Tools.Visible = true;
                else
                    Tools.Visible = false;
            }
            else
            {
                Settings.Visible = false;
                Personnel.Visible = false;
                Leave.Visible = false;
                Attendance.Visible = false;
                Payroll.Visible = false;
                Tools.Visible = false;                
            }
            }
        }
        public static DataTable PanelWiseUserPrivilege(string UserId, string ModuleID)
        {

            DataTable dt = new DataTable();
            string sqlCmd = "select distinct ModulePageName from UserPrivilege where UserID=" + UserId + " and ( ModuleId=" + ModuleID + " or ModulePageName='aplication.aspx') and (ReadAction=1 or WriteAction=1 or UpdateAction=1 or DeleteAction=1)";
            sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
            return dt;
        }
        public static DataTable PanelWiseUserPrivilegeByUserType(string UserType, string ModuleID)
        {

            DataTable dt = new DataTable();
            string sqlCmd = "select distinct ModulePageName from ModulePageInfoByUserType where UserType='" + UserType + "' and ModuleId=" + ModuleID + "";
            sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
            return dt;
        }
        public static DataTable PanelWiseUserPrivilegeByUserTypeAndPage(string UserType, string ModulePageName)
        {

            DataTable dt = new DataTable();
            string sqlCmd = "select distinct ModulePageName from ModulePageInfoByUserType where UserType='" + UserType + "' and ModulePageName='" + ModulePageName + "'";
           sqlDB.fillDataTable(sqlCmd, dt = new DataTable());
            return dt;
        }
        public static DataTable PanelWiseUserPrivilege(string UserId, string ModuleID, string ModulePageName)
        {

            DataTable dt = new DataTable();
            sqlDB.fillDataTable("select distinct ModulePageName from UserPrivilege where UserID=" + UserId + " and ModulePageName='" + ModulePageName + "'  and (ReadAction=1 or WriteAction=1 or UpdateAction=1 or DeleteAction=1)", dt = new DataTable());
            return dt;
        }


      
      
    }
}