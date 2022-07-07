using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.attendance
{
    public partial class import_data_ahg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();

            if (!IsPostBack)
            {
               // FileCoppy();
                ViewState["__OT__"] = "0";
                setPrivilege();              
            }
            if (!classes.commonTask.HasBranch())
                ddlCompanyList.Enabled = false;
        }

        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__getUserId__"] = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CShortName__"] = getCookies["__CShortName__"].ToString();
              

                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForOnlyWriteAction(ViewState["__CompanyId__"].ToString(), ViewState["__getUserId__"].ToString(), ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "import_data.aspx", ddlCompanyList, btnImport);  

                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.commonTask.loadDepartmentListByCompany_ForShrink(ddlDepartmentList, ddlCompanyList.SelectedValue);
                ViewState["__AttMachineName__"] = classes.commonTask.loadAttMachineName(ddlCompanyList.SelectedValue);
                if (File.Exists(HttpContext.Current.Server.MapPath("~/IsOnline.txt")) || ViewState["__AttMachineName__"].ToString().Equals("RMS"))
                {
                    tdFileUpload.Visible = true;
                    tdSelectFile.Visible = true;
                }
                else
                {
                    tdFileUpload.Visible = false;
                    tdSelectFile.Visible = false;
                }
            }
            catch { }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                lblErrorMessage.Text += ",70";
                if (validationBasket())
                {
                    lblErrorMessage.Text += ",73";
                    DataTable DtEmpAttList=null;
                 //   DateTime AttendanceDate = (rblImportType.SelectedItem.Value.Equals("FullImport")) ?  DateTime.ParseExact(txtFullAttDate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture) : DateTime.ParseExact(txtPartialAttDate.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    DateTime AttendanceDate = (rblImportType.SelectedItem.Value.Equals("FullImport")) ?  DateTime.Parse(commonTask.ddMMyyyyToyyyyMMdd( txtFullAttDate.Text)) : DateTime.Parse(commonTask.ddMMyyyyToyyyyMMdd( txtPartialAttDate.Text));
                    bool ShrinkType = (rblImportType.SelectedItem.Value.Equals("FullImport")) ? true : false;
                    lblErrorMessage.Text += ",77" + AttendanceDate.ToString() ;
                    switch (ViewState["__AttMachineName__"].ToString())
                    {
                        //case "OIT":  // for optimal it limited
                        //case "MSL":  // for Manvil Styles Limited , Gabtoli Hamayetpur
                        //    classes.mZK_Shrink_Data_SqlServer mzk = new classes.mZK_Shrink_Data_SqlServer();
                        //    mzk.Store_In_Attendance_Log(ddlCompanyList.SelectedValue, AttendanceDate, ShrinkType, ddlDepartmentList.SelectedValue, txtCardNo.Text);                            
                        //    break;
                        case "RMS": // for Tania Taxtile Limited , Narayongong
                      //  case "SAL": // for Mirzapur Stark Apparels Limited , Tangail Mirzapur
                            classes.mRMS_Shrink_data_MSAccess sda = new classes.mRMS_Shrink_data_MSAccess();
                            sda.Store_In_Attendance_Log(ddlCompanyList.SelectedValue, AttendanceDate, FileUpload1, ShrinkType, ddlDepartmentList.SelectedValue, txtCardNo.Text, ViewState["__getUserId__"].ToString());                          
                            break;
                      
                        default: // for optimal it limited // for Manvil Styles Limited , Gabtoli Hamayetpur                            
                              classes.mZK_Shrink_Data_SqlServer mzk = new classes.mZK_Shrink_Data_SqlServer();
                              mzk.Store_In_Attendance_Log(ddlCompanyList.SelectedValue, AttendanceDate, ShrinkType, ddlDepartmentList.SelectedValue, txtCardNo.Text, ViewState["__getUserId__"].ToString(), FileUpload1, lblErrorMessage);                            
                            break;
                    }
                    DtEmpAttList = classes.mCommon_Module_For_AttendanceProcessing.Load_Process_AttendanceData(ddlCompanyList.SelectedValue, ddlDepartmentList.SelectedValue, AttendanceDate.ToString("yyyy-MM-dd"), ShrinkType, txtCardNo.Text.Trim());
                    gvAttendance.DataSource = DtEmpAttList;
                    gvAttendance.DataBind();
                    ulAttMissingLog.Visible = true;
                    lblErrorMessage.Text += "eroro msg";
                }
            }
            catch (Exception ex) { lblErrorMessage.Text +=ex.Message; }           
        }
    
        private bool validationBasket()
        {
            try
            {
                
                //if (!FileUpload1.HasFile && FileUpload1.Visible)
                //{

                //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                //    lblErrorMessage.Text = "Please select access database file";
                //    FileUpload1.Focus();
                //    return false;
                //}
                if (!File.Exists(HttpContext.Current.Server.MapPath("~/AccessFile/"+ddlCompanyList.SelectedValue+"att2000.mdb")))
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select  access database file (att2000)";
                    FileUpload1.Focus();
                    return false;
                }

                if (rblImportType.SelectedValue == "FullImport" && rblDateType.SelectedValue == "SingleDate" && txtFullAttDate.Text.Trim().Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select attendance date";
                    txtFullAttDate.Focus();
                    return false;
                }

                if (rblImportType.SelectedValue != "FullImport" && txtCardNo.Text.Trim().Length < 4)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please type valid card no";
                    txtCardNo.Focus();
                    return false;
                }
                if (rblImportType.SelectedValue != "FullImport" && txtPartialAttDate.Text.Trim().Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select partial attendance date";
                    txtPartialAttDate.Focus();
                    return false;
                }

                if (txtFullToDate.Visible == true && txtFullToDate.Text.Trim().Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select To date";
                    txtFullToDate.Focus();
                    return false;
                }
                else if (txtPartialToDate.Visible == true && txtPartialToDate.Text.Trim().Length < 10)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "alertMessage();", true);
                    lblErrorMessage.Text = "Please select To date";
                    txtPartialToDate.Focus();
                    return false;
                }
                

                return true;
            }
            catch { return false; }
        }

        protected void gvAttendance_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvAttendance.PageIndex = e.NewPageIndex;
                gvAttendance.DataBind();
            }
            catch { }
        }

        protected void gvAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
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
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["__AttMachineName__"] = classes.commonTask.loadAttMachineName(ddlCompanyList.SelectedValue);
            classes.commonTask.loadDepartmentListByCompany_ForShrink(ddlDepartmentList, ddlCompanyList.SelectedValue);
            if (ViewState["__AttMachineName__"].ToString().Equals("RMS"))
            {
                tdFileUpload.Visible = true;
                tdSelectFile.Visible = true;
            }
            else
            {
                tdFileUpload.Visible = false;
                tdSelectFile.Visible = false;
            }
        }
        private void FileCoppy() 
        {
            string fileName = "att2000.mdb";
            string sourcePath = @"D:\Running Project";
            string targetPath = @"D:\Running Project";

            // Use Path class to manipulate file and directory paths.
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, "1000"+fileName);
            if (File.Exists(destFile))
            {
                File.Delete(destFile);
            }
            System.IO.File.Copy(sourceFile, destFile, true);
        }
    }
}