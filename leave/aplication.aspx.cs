using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using ComplexScriptingSystem;
using adviitRuntimeScripting;
using System.Data.SqlClient;
using System.Drawing;
using SigmaERP.classes;
using System.IO;

namespace SigmaERP.personnel
{
    public partial class aplication : System.Web.UI.Page
    {
        string sql = "";
        
        DataTable DTLocal = null;
        string[] getColumns;
        string[] getValues;
        string[] getReplaceDate; 
        SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                trReplacedDate.Visible = false; 
                ViewState["__FindName__"] = "No";
                classes.commonTask.LoadBranch(ddlBranch);
                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                trpregnatn.Visible = false;
                trprasabera.Visible = false;
                ViewState["yesAlater"] = "False";
                string[] query={" "};
                try
                {
                    query = Request.QueryString["LC"].ToString().Split('-');
                    ViewState["__lvListType__"] = query[1];
                }
                catch
                { }

                setPrivilege();
                
                
             //   classes.commonTask.loadLeaveTypes(ddlLeaveName);
              
                //loadLeaveApplication();
               
                
                ddlLeaveName.SelectedIndex = 0;
                // loadPunismentInfo();
                //TabContainer1.ActiveTabIndex = 0;
                if (query[0] != " ")
                {
                    chkProcessed.Enabled = true;
                    SetValueToControl(query[0]);               
                    ViewState["yesAlater"] = "True";
                    ViewState["__LACode__"] = query[0];
                }
                

                if (!classes.commonTask.HasBranch())
                    ddlBranch.Enabled = false;
                ddlBranch.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
           
        }

        
        private void loadLeaveName()
        {
            try
            {
                string CompanyId = (ddlBranch.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue.ToString();
                sqlDB.fillDataTable("Select LeaveId, LeaveName+' '+shortName as LeaveName  from tblLeaveConfig where ShortName <>'sr/l' and CompanyId='" + CompanyId + "'", dt = new DataTable());
                ddlLeaveName.DataValueField = "LeaveId";
                ddlLeaveName.DataTextField = "LeaveName";
                ddlLeaveName.DataSource = dt;
                ddlLeaveName.DataBind();
                ddlLeaveName.Items.Insert(0, new ListItem("", "0"));
               
            }
            catch { }
        }

        static DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserId__"] = getUserId;
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__EmpId__"] = getCookies["__getEmpId__"].ToString();
                ViewState["__ODOnlyDpt__"] = getCookies["__getODOnlyDpt__"].ToString();
                ViewState["__DptId__"] = getCookies["__getDptId__"].ToString();

                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "aplication.aspx", ddlBranch, btnSaveLeave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

                ViewState["__IsLeaveAuthority__"] = commonTask.IsLeaveAuthority(ViewState["__CompanyId__"].ToString());
          

                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "User")
                {
                   
                    findEmpName();
                    trDepartment.Visible = false;
                    trEmployee.Visible = false;
                    classes.commonTask.LoadChargeHandedToOverPerson(ddlHandOverPerson, ViewState["__CompanyId__"].ToString(), ViewState["__EmpId__"].ToString());
                }
                else if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin")
                {
                    classes.commonTask.loadDepartmentByAdminForLeave(ddlDepartment, ViewState["__CompanyId__"].ToString(), ViewState["__UserId__"].ToString());
                    if(ddlDepartment.Items.Count==1)
                    classes.commonTask.LoadEmpCardNoByAdminAndDepartment(ddlEmpCardNo, ViewState["__UserId__"].ToString(), ddlDepartment.SelectedValue);
                  
                    classes.commonTask.LoadChargeHandedToOverPerson(ddlHandOverPerson, ViewState["__CompanyId__"].ToString(), "");
                }
                else
                {
                    classes.commonTask.loadDepartmentListByCompany(ddlDepartment, ViewState["__CompanyId__"].ToString());
                    classes.commonTask.LoadChargeHandedToOverPerson(ddlHandOverPerson, ViewState["__CompanyId__"].ToString(), "");
                }                   
                classes.commonTask.loadLeaveNameByCompany(ddlLeaveName, ViewState["__CompanyId__"].ToString());

                if (ViewState["__ReadAction__"].ToString().Equals("0"))
                {
                    //    gvLeaveApplication.Visible = false;
                    //    gvRejectedList.Visible = false;
                    btnSelectAll.Enabled = false;
                    btnSelectAll.CssClass = "";
                }
                

            }


            catch { }

        }

     
        //void loadLeaveApplication()   // all approved leave application
        //{
        //    DataTable dtLeaveInfo = new DataTable();
        //    string CompanyId = (ddlBranch.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue.ToString();
        //   string strSQL;
        //    //if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "User")
        //    //    strSQL = "select LACode,LeaveId,convert(varchar(11),FromDate,105) as FromDate,convert (varchar(11),ToDate,105) as ToDate,WeekHolydayNo,TotalDays,EmpId,ShortName,EmpType,EmpCardNo from v_Leave_LeaveApplication where IsProcessessed='0' AND IsApproved ='0' AND CompanyId='" + CompanyId + "' and EmpId='" + ViewState["__EmpId__"].ToString() + "' Order by EmpCardNo,FromDate desc";
        //    //else {  
        //   if (ddlDepartment.SelectedIndex == 0) 
        //       strSQL = "select LACode,LeaveId,convert(varchar(11),FromDate,105) as FromDate,convert (varchar(11),ToDate,105) as ToDate,WeekHolydayNo,TotalDays,EmpId,ShortName,EmpType,EmpCardNo from v_Leave_LeaveApplication where IsProcessessed='true' AND IsApproved ='true' AND CompanyId='" + CompanyId + "' Order by EmpCardNo,FromDate desc";
        //   else strSQL = "select LACode,LeaveId,convert(varchar(11),FromDate,105) as FromDate,convert (varchar(11),ToDate,105) as ToDate,WeekHolydayNo,TotalDays,EmpId,ShortName,EmpType,EmpCardNo from v_Leave_LeaveApplication where IsProcessessed='true' AND IsApproved ='true' AND CompanyId='" + CompanyId + "' AND DptId='" + ddlDepartment.SelectedValue.ToString() + "' Order by EmpCardNo,FromDate desc";
        //    //}
        //        sqlDB.fillDataTable(strSQL, dtLeaveInfo );
       
        //    gvLeaveApplication.DataSource =dtLeaveInfo;
        //    gvLeaveApplication.DataBind();
        //}

        //void loadRejected_LeaveApplication()  // for display all rejected leave application
        //{
        //    DataTable dtLeaveInfo = new DataTable();

        //    string strSQL = "select LACode,LeaveId,convert(varchar(11),FromDate,105) as FromDate,convert (varchar(11),ToDate,105) as ToDate,WeekHolydayNo,TotalDays,EmpId,ShortName,EmpType,EmpCardNo from v_Leave_LeaveApplication where IsProcessessed='false' AND IsApproved ='false' AND CompanyId='" + ddlBranch.SelectedValue + "' Order by EmpCardNo,FromDate desc";
        //    SQLOperation.selectBySetCommandInDatatable(strSQL, dtLeaveInfo, sqlDB.connection);

        //    gvRejectedList.DataSource = dtLeaveInfo;
        //    gvRejectedList.DataBind();
        //}

        //private string getLeaveAuthority()
        //{
        //    try
        //    {
        //        DataTable dtLvOrder;
        //        sqlDB.fillDataTable("select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where isLvAuthority=1 and DptId in(" + ViewState["__DptId__"].ToString() + ")", dtLvOrder = new DataTable());
        //        if (!dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString().Equals("0"))
        //            return dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString();
        //        else
        //        {
        //            sqlDB.fillDataTable("select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where isLvAuthority=1 and  LvOnlyDpt=0", dtLvOrder = new DataTable());
        //            return dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString();
        //        }
        //    }
        //    catch { return "0"; }
        //}
        private string getLeaveAuthority()
        {
            try
            {
                DataTable dtLvOrder;                
                sql = "select case when IsDirectApprove=1 then -1 else   max(AuthorityPosition) end as LvAuthorityOrder from tblLeaveAuthorityAccessControl where EmpID='" + ViewState["EmpId"].ToString() + "' group by IsDirectApprove";
                sqlDB.fillDataTable(sql, dtLvOrder = new DataTable());
                if (dtLvOrder == null || dtLvOrder.Rows.Count == 0)
                    return "";
                return dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString();
                
                //--- old code bellow---
                sql = "select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where isLvAuthority=1 and DptId in(" + ViewState["__DptId__"].ToString() + ") and EmpId ='" + ViewState["__EmpId__"].ToString() + "'";
                sqlDB.fillDataTable(sql, dtLvOrder = new DataTable());
                if (!dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString().Equals("0"))
                    sql = "select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where isLvAuthority=1 and DptId in(" + ViewState["__DptId__"].ToString() + ") and LvAuthorityOrder<" + dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString() + "";
                else
                    sql = "select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where isLvAuthority=1 and DptId in(" + ViewState["__DptId__"].ToString() + ") ";
                sqlDB.fillDataTable(sql, dtLvOrder = new DataTable());
                if (!dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString().Equals("0"))
                    return dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString();
                else
                {
                    sql = "select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where isLvAuthority=1 and  LvOnlyDpt=0 and EmpId ='" + ViewState["__EmpId__"].ToString() + "'";
                    sqlDB.fillDataTable(sql, dtLvOrder = new DataTable());
                    if (!dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString().Equals("0"))
                        sql = "select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where isLvAuthority=1 and  LvOnlyDpt=0 and LvAuthorityOrder<" + dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString() + "";
                    else
                        sql = "select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where isLvAuthority=1 and  LvOnlyDpt=0";
                    sqlDB.fillDataTable(sql, dtLvOrder = new DataTable());
                    return dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString();
                }
                
            }
            catch { return ""; }
        }
        private void saveLeaveApplication()
        {
            try
            {
                string LeaveProcessingOrder = "0";
                string IsApprove = "0";
                if (bool.Parse(ViewState["__IsLeaveAuthority__"].ToString()))// if this company use leave appoval system
                {
                     LeaveProcessingOrder   = getLeaveAuthority();
                    if (LeaveProcessingOrder.Trim() == "") // Leave Authority is not found. plz setup for this employee.
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('You are not able to apply for any leave. Please contact with Authority.');", true);
                        return;
                        // 
                    }
                    else if (LeaveProcessingOrder.Trim() == "-1")// This Employee is allowed for direct approval
                    {
                        IsApprove = "1";
                      
                    }
                        
                }
                else
                    IsApprove = "1";


                
                    

              //  byte process = (DateTime.ParseExact(txtFromDate.Text.Trim(),"dd-MM-yyyy",null) >= DateTime.ParseExact(DateTime.Now.ToString("dd-MM-yyyy"),"dd-MM-yyyy",null)) ? (byte)1 : (byte)0;
                byte process = 0;
                string CompanyId = (ddlBranch.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue.ToString();

                if (!ddlLeaveName.SelectedItem.ToString().Contains("m/l"))
                {

                    //string[] getColumns = { "LeaveId", "EmpId", "FromDate", "ToDate", "WeekHolydayNo", "TotalDays", "IsApproved", "IsProcessessed", "EmpTypeId", "EntryDate", "CompanyId", "SftId", "Remarks", "DptId", "DsgId", "EmpStatus", "LeaveFormSLNo", "ApplyDate", "LeaveProcessingOrder", "LvAddress", "LvContact", "HandedOverEmpId", "IsHalfDayLeave" };
                    //// string ReplacedDate = (ddlLeaveName.SelectedItem.ToString().Contains("cr/l")) ? commonTask.ddMMyyyyToyyyyMMdd(txtReplacedDate.Text.Trim()).ToString() : "";

                    //string[] getValues = { ddlLeaveName.SelectedValue.ToString(), ViewState["EmpId"].ToString(),commonTask.ddMMyyyyToyyyyMMdd(txtFromDate.Text.Trim()).ToString(),
                    //                         commonTask.ddMMyyyyToyyyyMMdd(txtToDate.Text.Trim()).ToString(), txtTotalHolydays.Text.Trim(),
                    //                         txtNoOfDays.Text.Trim(),
                    //                         IsApprove, process.ToString(),ViewState["__EmpTypeId__"].ToString(),
                    //                         DateTime.Now.ToString("yyyy-MM-dd"),CompanyId,ViewState["__SftId__"].ToString(),txtNotes.Text.Trim(),
                    //                     ViewState["__DptId__"].ToString(),ViewState["__DsgId__"].ToString(),"1",classes.LeaveLibrary.GetLeaveFormSerialNo().ToString(), commonTask.ddMMyyyyToyyyyMMdd(txtApplyDate.Text.Trim()).ToString()
                    //                     ,LeaveProcessingOrder,txtLvAddress.Text.Trim(),txtLvContact.Text.Trim(),ddlHandOverPerson.SelectedValue,ckbIsHalfDayLeave.Checked.ToString()};

                    string sqlCmd = "Insert into Leave_LeaveApplication "+
                        "(LeaveId, EmpId, FromDate, ToDate, WeekHolydayNo, TotalDays, IsApproved, IsProcessessed, EmpTypeId, EntryDate, CompanyId, SftId, Remarks, DptId, DsgId, EmpStatus, LeaveFormSLNo, ApplyDate, LeaveProcessingOrder, LvAddress, LvContact, HandedOverEmpId, IsHalfDayLeave) "+
                        "values('"+ ddlLeaveName.SelectedValue.ToString() + "','"+ ViewState["EmpId"].ToString() + "','"+ commonTask.ddMMyyyyToyyyyMMdd(txtFromDate.Text.Trim()).ToString() + 
                        "','"+ commonTask.ddMMyyyyToyyyyMMdd(txtToDate.Text.Trim()).ToString() + "','"+ txtTotalHolydays.Text.Trim() + "','"+ txtNoOfDays.Text.Trim() + 
                        "','"+ IsApprove + "','"+ process.ToString() + "','"+ ViewState["__EmpTypeId__"].ToString() + "','"+ DateTime.Now.ToString("yyyy-MM-dd") + 
                        "','"+ CompanyId + "','"+ ViewState["__SftId__"].ToString() + "','"+ txtNotes.Text.Trim() + "','"+ ViewState["__DptId__"].ToString() + 
                        "','"+ ViewState["__DsgId__"].ToString() + "','1','"+ classes.LeaveLibrary.GetLeaveFormSerialNo().ToString() + "','"+ commonTask.ddMMyyyyToyyyyMMdd(txtApplyDate.Text.Trim()).ToString() + 
                        "','"+ LeaveProcessingOrder + "','"+ txtLvAddress.Text.Trim() + "','"+ txtLvContact.Text.Trim() + "','"+ ddlHandOverPerson.SelectedValue + "','"+ ckbIsHalfDayLeave.Checked.ToString() + "');SELECT SCOPE_IDENTITY()";
                    int LAcode = CRUD.ExecuteReturnID(sqlCmd, sqlDB.connection);
                    //if (SQLOperation.forSaveValue("Leave_LeaveApplication", getColumns, getValues, sqlDB.connection) == true)
                    if (LAcode>0)                    
                    {
                        PeocessLeaveDetails(LAcode.ToString());
                        if (ddlLeaveName.SelectedItem.Text.Trim() == "Compensation/Replace Leave cr/l")
                            SaveLeaveReplacedDate(LAcode.ToString());
                        if (ViewState["yesAlater"].Equals("True"))
                        {
                            ViewState["yesAlater"] = "False";
                            Response.Redirect("/leave/all_leave_list.aspx?for=" + ViewState["__lvListType__"].ToString());
                        }
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "SuccessMsg('Successfully Leave Application Saved.');", true);                       
                        Clear();
                    }

                }
                else
                {
                    if (!checkSex()) return;

                    //string[] getColumns = { "LeaveId", "EmpId", "FromDate", "ToDate", "WeekHolydayNo", "TotalDays", "IsApproved", "IsProcessessed", "EmpTypeId", "PregnantDate", "PrasaberaDate", "EntryDate", "CompanyId", "SftId", "Remarks", "DptId", "DsgId", "EmpStatus", "LeaveFormSLNo", "ApplyDate", "LeaveProcessingOrder", "LvAddress", "LvContact", "HandedOverEmpId" };

                    //string[] getValues = { ddlLeaveName.SelectedValue.ToString(), ViewState["EmpId"].ToString(), commonTask.ddMMyyyyToyyyyMMdd(txtFromDate.Text.Trim()).ToString(),
                    //                         commonTask.ddMMyyyyToyyyyMMdd(txtToDate.Text.Trim()).ToString(), txtTotalHolydays.Text.Trim(), txtNoOfDays.Text.Trim(), 
                    //                         IsApprove, process.ToString(),
                    //                         ViewState["__EmpTypeId__"].ToString(),commonTask.ddMMyyyyToyyyyMMdd(txtPregnantDate.Text.Trim()).ToString(),
                    //                        commonTask.ddMMyyyyToyyyyMMdd(txtPrasabaDate.Text.Trim()).ToString(),DateTime.Now.ToString("yyyy-MM-dd"),CompanyId,ViewState["__SftId__"].ToString(),txtNotes.Text.Trim(),
                    //                        ViewState["__DptId__"].ToString(),ViewState["__DsgId__"].ToString(),"8",classes.LeaveLibrary.GetLeaveFormSerialNo().ToString(), commonTask.ddMMyyyyToyyyyMMdd(txtApplyDate.Text.Trim()).ToString(),getLeaveAuthority(),txtLvAddress.Text.Trim(),txtLvContact.Text.Trim(),ddlHandOverPerson.SelectedValue};
                    string sqlCmd = "Insert into Leave_LeaveApplication " +
                                            "(LeaveId, EmpId, FromDate, ToDate, WeekHolydayNo, TotalDays, IsApproved, IsProcessessed, EmpTypeId, PregnantDate, PrasaberaDate, EntryDate, CompanyId, SftId, Remarks, DptId, DsgId, EmpStatus, LeaveFormSLNo, ApplyDate, LeaveProcessingOrder, LvAddress, LvContact, HandedOverEmpId) " +
                                            "values('"+ddlLeaveName.SelectedValue.ToString()+ "','" + ViewState["EmpId"].ToString() + "','" + commonTask.ddMMyyyyToyyyyMMdd(txtFromDate.Text.Trim()).ToString() + "','" + commonTask.ddMMyyyyToyyyyMMdd(txtToDate.Text.Trim()).ToString() + 
                                            "','" + txtTotalHolydays.Text.Trim() + "','" + txtNoOfDays.Text.Trim() + "','" + IsApprove + "','" + process.ToString() + "','" + ViewState["__EmpTypeId__"].ToString() + "','" + commonTask.ddMMyyyyToyyyyMMdd(txtPregnantDate.Text.Trim()).ToString() + 
                                            "','" + commonTask.ddMMyyyyToyyyyMMdd(txtPrasabaDate.Text.Trim()).ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + CompanyId + "','" + ViewState["__SftId__"].ToString() + "','" + txtNotes.Text.Trim()+ 
                                            "','" + ViewState["__DptId__"].ToString() + "','" + ViewState["__DsgId__"].ToString() + "','8','" + classes.LeaveLibrary.GetLeaveFormSerialNo().ToString() + "','" + commonTask.ddMMyyyyToyyyyMMdd(txtApplyDate.Text.Trim()).ToString() + "','" + getLeaveAuthority() + 
                                            "','" + txtLvAddress.Text.Trim() + "','" + txtLvContact.Text.Trim() + "','" + ddlHandOverPerson.SelectedValue + "');SELECT SCOPE_IDENTITY()";
                    int LAcode = CRUD.ExecuteReturnID(sqlCmd, sqlDB.connection);
                    //if (SQLOperation.forSaveValue("Leave_LeaveApplication", getColumns, getValues, sqlDB.connection) == true)
                    if (LAcode > 0)
                    { 

                        // ChangeEmpTypeForML();
                        PeocessLeaveDetails(LAcode.ToString());
                        if (ViewState["yesAlater"].Equals("True"))
                        {
                            ViewState["yesAlater"] = "False";
                            Response.Redirect("/leave/all_leave_list.aspx?for=" + ViewState["__lvListType__"].ToString());
                        }
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "SuccessMsg('Successfully Leave Application Saved.');", true);
                       // lblMessage.InnerText = "success->Successfully Leave Application Saved";
                       // loadLeaveApplication();
                        Clear();
                    }
                }

                

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ErrorMsg('" + ex.Message + "');", true);
               // lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private void PeocessLeaveDetails(string LACode)
        {
            try
            {
              //  sqlDB.fillDataTable("select MAX(LACode) as LACode from Leave_LeaveApplication where EmpId=" + ViewState["EmpId"].ToString() + "", dt = new DataTable());
                DateTime FromDate = DateTime.Parse(commonTask.ddMMyyyyToyyyyMMdd(txtFromDate.Text.Trim()));
                DateTime ToDate = DateTime.Parse(commonTask.ddMMyyyyToyyyyMMdd(txtToDate.Text.Trim()));

                DateTime ApplyDate = DateTime.Parse(commonTask.ddMMyyyyToyyyyMMdd(txtApplyDate.Text.Trim())); //new DateTime(int.Parse(txtApplyDate.Text.Trim().Substring(6, 4)), int.Parse(txtApplyDate.Text.Trim().Substring(3, 2)), int.Parse(txtApplyDate.Text.Trim().Substring(0, 2)));

                DateTime FromDate1 = FromDate;

                while (FromDate <= ToDate)
                {
                    
                    if (ApplyDate > FromDate1)             
                        saveApplyDate(LACode, FromDate.ToString("yyyy-MM-dd"));
                    
                    
                    saveLeaveDetails(LACode, FromDate.ToString("yyyy-MM-dd"));
                    FromDate = FromDate.AddDays(1);
                }


                SaveAttachDocument(LACode);      
            }
            catch { }

        }
        private void SaveLeaveReplacedDate(string LACode)
        {
            try
            {
               // sqlDB.fillDataTable("select MAX(LACode) as LACode from Leave_LeaveApplication where EmpId=" + ViewState["EmpId"].ToString() + "", dt = new DataTable());

                for (byte i = 0; i < getReplaceDate.Length; i++)
                {
                    string[] getColumns = { "LACode", "EmpId", "RDate"};
                    string[] getValues = { LACode, ViewState["EmpId"].ToString(), commonTask.ddMMyyyyToyyyyMMdd(getReplaceDate[i]).ToString() };
                    SQLOperation.forSaveValue("Leave_LeaveReplacedDateLog", getColumns, getValues, sqlDB.connection);
                }
               
            }
            catch { }

        }
        private void saveLeaveDetails(string LACode, string Date)
        {
            try
            {
                string[] getColumns = { "LACode", "EmpId", "LeaveDate", "Used" };
                string[] getValues = { LACode, ViewState["EmpId"].ToString(), Date, "0" };
                SQLOperation.forSaveValue("Leave_LeaveApplicationDetails", getColumns, getValues, sqlDB.connection);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ErrorMsg('" + ex.Message + "');", true);
              //  lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private void saveApplyDate(string LACode, string FromDate) // While Apply Date < From Date then execute this function().
        {
            try
            {                
                string[] getColumns = { "EmpId", "LACode", "Lv_date"};
                string[] getValues = { ViewState["EmpId"].ToString(), LACode, FromDate };
                SQLOperation.forSaveValue("Leave_ApplyDate", getColumns, getValues, sqlDB.connection);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ErrorMsg('" + ex.Message + "');", true);
                //lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private bool checkSex()
        {
            try
            {
                sqlDB.fillDataTable("select Sex from Personnel_EmpPersonnal where EmpId='"+ViewState["EmpId"].ToString()+"'",dt=new DataTable ());
                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ErrorMsg('Please set sex or gender of this employee !');", true);
                    //lblMessage.InnerText = "error->Please set sex or gender of this employee !";
                    return false;
                }
                bool sex = (dt.Rows[0]["Sex"].ToString().Equals("Female")) ? true : false;
                if (!sex)
                { 
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ErrorMsg('This Employee Is Male !');", true);
                    //lblMessage.InnerText = "error->This Employee Is Male !";
                    return false;
                }
                else return true;
            }
            catch { return false;}
        }

        private void ChangeEmpTypeForML()   // change EmpType when any female get any Maternity Leave
        {
            try
            {
                cmd = new SqlCommand("update Personnel_EmpCurrentStatus set EmpStatus='8' where SN=(select Max(SN) from  Personnel_EmpCurrentStatus Where EmpId='" + ViewState["EmpId"].ToString() + "' AND IsActive=1)", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }
       
        private void deleteExistsAttendance(string DateFrom)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("delete from tblAttendanceRecord where EmpId ='" + ViewState["EmpId"].ToString() + "' and ATTDate='" + DateFrom + "'",sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        
        }

        private void saveLeavedayAndToadyIsEquql()
        {
            try
            {
                sqlDB.fillDataTable("select EmpProximityNo from Personnel_EmployeeInfo where EmpId='" + ViewState["EmpId"].ToString() + "'",dt=new DataTable ());



                string[] getColumns = { "EmpProximityNo", "EmpCardNo", "ATTDate", "InHour", "InMin", "InSec", "OutHour", "OutMin", "OutSec", "BreakStartHour", "BreakStartMin", "BreakEndHour", "BreakEndMin", "ATTStatus", "StateStatus", "Remarks", "EmpTypeId", "AttManual", "EmpId", "DailyStartTimeALT" };
                string[] getValues = { dt.Rows[0]["EmpProximityNo"].ToString(), txtEmpCardNo.Text.Trim(), commonTask.ddMMyyyyToyyyyMMdd(txtFromDate.Text.Trim()).ToString(), 
                                         "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "LV", ddlLeaveName.SelectedItem.Text," ",ViewState["__EmpTypeId__"].ToString(), "Manual Attendance", ViewState["EmpId"].ToString(), "00-00-00-00" };
                SQLOperation.forSaveValue("tblAttendanceRecord", getColumns, getValues, sqlDB.connection);
              
            }
            catch { }
        }

        private void changeEmpTypeOnBaseMaternityLeaveDelete(string EmpId,string EmpTypeId)
        {
            try
            {
                cmd = new SqlCommand("update Personnel_EmpCurrentStatus set EmpStatus='1' where SN=(select Max (SN) from Personnel_EmpCurrentStatus where EmpId='" + EmpId + "' and IsActive=1)", sqlDB.connection);
                cmd.ExecuteNonQuery();
            }
            catch { }
        }
       
        void Clear()
        {
            ViewState["__FindName__"] = "No";
            lblDepartment.Text = "";
            lblSftTime.Text = "";
            ViewState["yesAlater"] = "False";
            txtEmpCardNo.Text = "";
            txtEmpName.Text = "";
            txtApplyDate.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtTotalHolydays.Text = "";
            txtNoOfDays.Text = "";
            txtNotes.Text = "";
            chkApproved.Checked = true;
            chkProcessed.Checked = true;
            txtCardNo.Enabled = true;
            txtPregnantDate.Text = "";
            txtPrasabaDate.Text = "";
            trprasabera.Visible = false;
            trpregnatn.Visible = false;
            trStatusBar.Visible = false;

            if (ViewState["__WriteAction__"].Equals("0"))
            {
                btnSaveLeave.Enabled = false;
                btnSaveLeave.CssClass = "";
            }
            else
            {
                btnSaveLeave.Enabled = true;
                btnSaveLeave.CssClass = "Rbutton";
            }
            btnSaveLeave.Text = "Save";
            ddlLeaveName.SelectedIndex = 0;
            chkProcessed.Enabled = false;            
            ddlEmpCardNo.SelectedIndex = 0;
            txtFileName.Text = "";
            txtReplacedDate.Text = "";
            ckbIsHalfDayLeave.Checked = false;
         
            
        }

        private void SetValueToControl(string LAId)
        {
            try
            {
                string strSQL = @"select la.EmpCardNo,la.LACode, la.EmpId, la.FromDate,la.IsApproved,la.IsProcessessed,la.ToDate,la.ApplyDate,la.ReplacedDate,
                                la.WeekHolydayNo,la.EmpTypeId, la.TotalDays, lc.LeaveId,et.EmpType,la.PregnantDate,la.PrasaberaDate,la.CompanyId,la.SftId,
                                la.Remarks,la.DptId,la.IsHalfDayLeave,la.HandedOverEmpId,la.LvAddress,la.LvContact
                                from   dbo.v_Leave_LeaveApplication la
                                join dbo.tblLeaveConfig lc on lc.LeaveId=la.LeaveId inner join HRD_EmployeeType et on et.EmpTypeId=la.EmpTypeId
                                where la.LACode='" + LAId + "'";
                DataTable DTLocal = new DataTable();
                

                sqlDB.fillDataTable(strSQL, DTLocal);

                ddlBranch.SelectedValue = DTLocal.Rows[0]["CompanyId"].ToString();
               // ddlShiftName.SelectedValue = DTLocal.Rows[0]["SftId"].ToString();
                ddlDepartment.SelectedValue = DTLocal.Rows[0]["DptId"].ToString();
                classes.commonTask.LoadEmpCardNoByDepartment(ddlEmpCardNo, ddlBranch.SelectedValue, ddlDepartment.SelectedValue);
                hdLeaveApplicationId.Value = DTLocal.Rows[0]["LACode"].ToString();
                ddlEmpCardNo.SelectedValue = DTLocal.Rows[0]["EmpCardNo"].ToString();
                txtEmpCardNo.Text = DTLocal.Rows[0]["EmpCardNo"].ToString();
                ViewState["EmpId"] = DTLocal.Rows[0]["EmpId"].ToString();
                //txtEmpName.Text = DTLocal.Rows[0]["EmpName"].ToString();
                //txtStartDate.Text = Convert.ToDateTime(retutnObject.StartDate).ToString("dd-MMM-yyyy");
                ViewState["OldApplyDate"] = txtApplyDate.Text = Convert.ToDateTime(DTLocal.Rows[0]["ApplyDate"]).ToString("dd-MM-yyyy");
                ViewState["OldFromDate"]=txtFromDate.Text = Convert.ToDateTime(DTLocal.Rows[0]["FromDate"]).ToString("dd-MM-yyyy");
                ViewState["OldToDate"]=txtToDate.Text = Convert.ToDateTime(DTLocal.Rows[0]["ToDate"]).ToString("dd-MM-yyyy");   
    
                txtTotalHolydays.Text = DTLocal.Rows[0]["WeekHolydayNo"].ToString();
                txtNoOfDays.Text = DTLocal.Rows[0]["TotalDays"].ToString();
                ddlLeaveName.SelectedValue = DTLocal.Rows[0]["LeaveId"].ToString();
                chkApproved.Checked = ((DTLocal.Rows[0]["IsApproved"].ToString()).Equals("True")) ? true : false;
                chkProcessed.Checked = ((DTLocal.Rows[0]["IsProcessessed"].ToString()).Equals("True")) ? true : false;
                ddlHandOverPerson.SelectedValue= DTLocal.Rows[0]["HandedOverEmpId"].ToString();
                txtLvAddress.Text= DTLocal.Rows[0]["LvAddress"].ToString();
                txtLvContact.Text= DTLocal.Rows[0]["LvContact"].ToString();
                if (DTLocal.Rows[0]["IsHalfDayLeave"].ToString().Equals("True"))
                {
                    ckbIsHalfDayLeave.Checked = true;
                    tdFromDate.InnerText = "Leave Date";
                    trToDate.Visible = false;

                }
                //trStatusBar.Visible = true;             
                btnSaveLeave.Text = "Update";
                if (File.Exists(Server.MapPath("/EmployeeImages/LeaveDocument/" +LAId.ToString()+".jpg"))) 
                {
                    txtFileName.Text = LAId.ToString();
                }
                if (ViewState["__UpdateAction__"].Equals("0"))
                {
                    btnSaveLeave.Enabled = false;
                    btnSaveLeave.CssClass = "";
                }
                else
                {
                    btnSaveLeave.Enabled = true;
                    btnSaveLeave.CssClass = "Lbutton";
                }
                txtCardNo.Enabled = false;
                txtNotes.Text = DTLocal.Rows[0]["Remarks"].ToString();
                if (ddlLeaveName.SelectedItem.Text == "M/L")
                {
                    if (DTLocal.Rows[0]["PregnantDate"].ToString() == "")
                        txtPregnantDate.Text = DTLocal.Rows[0]["PregnantDate"].ToString();
                    else txtPregnantDate.Text = DateTime.Parse(DTLocal.Rows[0]["PregnantDate"].ToString()).ToString("dd-MM-yyyy");
                    if (DTLocal.Rows[0]["PrasaberaDate"].ToString()=="")
                        txtPrasabaDate.Text = DTLocal.Rows[0]["PrasaberaDate"].ToString();
                    else txtPrasabaDate.Text = DateTime.Parse(DTLocal.Rows[0]["PrasaberaDate"].ToString()).ToString("dd-MM-yyyy");
                    trprasabera.Visible = true;
                    trpregnatn.Visible = true;
                }
                if (ddlLeaveName.SelectedItem.ToString().Contains("cr/l"))
                {
                    trReplacedDate.Visible = true;

                    //txtReplacedDate.Text = Convert.ToDateTime(DTLocal.Rows[0]["ReplacedDate"]).ToString("dd-MM-yyyy");
                    txtReplacedDate.Text = returnReplaceDate(LAId); ;
                }

                findOutShiftTimeAndDptName();
                findEmpName();
            }
            catch { }
        }
        private string returnReplaceDate(string LaCode)
        {
            try
            {
                string ReplacedDate = "";
                sqlDB.fillDataTable(" SELECT LACode, convert(varchar(10),RDate,105) RDate FROM Leave_LeaveReplacedDateLog where LACode =" + LaCode, dt = new DataTable());
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i=0;i<dt.Rows.Count;i++)
                    {
                        ReplacedDate +=","+dt.Rows[i]["RDate"].ToString();
                    }
                    return ReplacedDate.Remove(0,1);
                }
                else return "";
            }
            catch { return ""; }
            
        }
        private void findOutShiftTimeAndDptName()
        {
            try
            {
                //sqlDB.fillDataTable("select  StartTime12Fromat,EndTime12Fomat From v_HRD_Shift where CompanyId='" + ddlBranch.SelectedValue.ToString() + "' AND SftId=" + ddlShiftName.SelectedValue.ToString() + "", dt = new DataTable());
                //if (dt.Rows.Count > 0) lblSftTime.Text = dt.Rows[0]["StartTime12Fromat"].ToString() + "-" + dt.Rows[0]["EndTime12Fomat"].ToString();
            }
            catch { }
        }
        //protected void gvLeaveApplication_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //       // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
               
        //        if (e.CommandName == "Alter")
        //        {
        //            int index = Convert.ToInt32(e.CommandArgument.ToString());
        //            string LACode = gvLeaveApplication.DataKeys[index].Value.ToString();
        //            ViewState["__LACode__"] = LACode;
                   
        //            ViewState["yesAlater"] = "True";
        //          //  ViewState["OldFromDate"] = txtFromDate.Text = gvLeaveApplication.Rows[index].Cells[2].Text;
        //           // ViewState["OldToDate"] = txtToDate.Text = gvLeaveApplication.Rows[index].Cells[3].Text;
        //            SetValueToControl(LACode);

        //        }
        //        else if (e.CommandName == "Delete")
        //        {
        //            Delete(Convert.ToInt32(e.CommandArgument)); // delete leave and change essential record status

        //            loadLeaveApplication();
        //            Clear();
        //        }

        //        else if (e.CommandName == "Status")
        //        {
        //            string LACode = gvLeaveApplication.DataKeys[Convert.ToInt32(e.CommandArgument)].Value.ToString();
        //            sqlDB.fillDataTable("select EmpId from Leave_LeaveApplication where LACode="+LACode+"",dt=new DataTable ());
        //            sqlDB.fillDataTable("select EmpName,EmpType,EmpCardNo from v_Personnel_EmpCurrentStatus where SN=(select Max(SN) from Personnel_EmpCurrentStatus where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' AND IsActive=1)",dt=new DataTable ());
                    
        //            txtEmployeeType.Text = dt.Rows[0]["EmpType"].ToString();
        //            txtCardNo.Text = dt.Rows[0]["EmpCardNo"].ToString();
        //            txtName.Text = dt.Rows[0]["EmpName"].ToString();

        //            sqlDB.fillDataTable("select Used from Leave_LeaveApplicationDetails where LACode="+LACode+"",dt=new DataTable ());

        //            DataRow[] dr={};
                  
        //            try
        //            {
        //                dr = dt.Select("Used='true'",null);
        //            }
        //            catch { }

        //            txtTotalLeave.Text = dt.Rows.Count.ToString();
                      
        //            txtUsed.Text = dr.Length.ToString();

        //            txtUnused.Text = (Convert.ToInt16(txtTotalLeave.Text.Trim()) - Convert.ToInt16(txtUsed.Text.Trim())).ToString();
                    
                    
        //            //ModalPopupExtender1.Show();
        //        }

        //        else if (e.CommandName.Equals("View"))
        //        { 
        //        viewLeaveApplication(e.CommandArgument.ToString());
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //lblMessage.Text = ex.ToString();
        //    }
        //}


        void Delete(int id)  // delete leave and change essential record status
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("select EmpId,Convert(varchar(11),FromDate,111) as FromDate,Convert(varchar(11),ToDate,111) as ToDate,LeaveName,EmpCardNo,EmpTypeId from v_Leave_LeaveApplication where Lacode =" + id.ToString() + "", dt);

            if (dt.Rows[0]["LeaveName"].ToString().ToLower().Equals("maternity leave") || dt.Rows[0]["LeaveName"].ToString().ToLower().Equals("m/l") || dt.Rows[0]["LeaveName"].ToString().ToLower().Equals("ml"))
            {
                changeEmpTypeOnBaseMaternityLeaveDelete(dt.Rows[0]["EmpId"].ToString(), dt.Rows[0]["EmpTypeId"].ToString());

            }

            if (SQLOperation.forDeleteRecordByIdentifier("Leave_LeaveApplication", "LACode", id.ToString(), sqlDB.connection) == true)
            {
                
                string getEmpTypeId = dt.Rows[0]["EmpTypeId"].ToString();
                string getEmpCardNo = dt.Rows[0]["EmpCardNo"].ToString();

                sqlDB.fillDataTable("select ATTStatus,convert(varchar(11),AttDate,111)as AttDate from tblAttendanceRecord where EmpCardNo='" + getEmpCardNo + "' AND EmpTypeId=" + getEmpTypeId + " AND ATTDate in (select leaveDate from Leave_LeaveApplicationDetails where LACode="+id.ToString()+"AND used='true')", dt = new DataTable());

                for (byte b = 0; b < dt.Rows.Count; b++)
                {
                    if (!dt.Rows[b]["ATTStatus"].ToString().Equals("W"))
                    {

                        SqlCommand cmd = new SqlCommand("update tblAttendanceRecord set ATTStatus='A',StateStatus=' ',DailyStartTimeALT='00:00:00:00' where EmpCardNo='" + getEmpCardNo + "' AND EmpTypeId=" + getEmpTypeId + " AND ATTDate ='" + dt.Rows[b]["AttDate"].ToString() + "' ", sqlDB.connection);
                        cmd.ExecuteNonQuery();
                    }

                }

                SQLOperation.forDeleteRecordByIdentifier("Leave_LeaveApplicationDetails", "LACode", id.ToString(), sqlDB.connection);

            
            }           

            btnSaveLeave.Text = "Save";
        }
        //protected void gvLeaveApplication_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    loadLeaveApplication();
        //}

        //protected void gvLeaveApplication_RowEditing(object sender, GridViewEditEventArgs e)
        //{

        //}


        private void Read_N_Write_WH(int Fdays, int Fmonth, string Fyear, int Tdays, int Tmonth, string Tyear)    // N=And,WH=Weekly Holiday
        {
            try
            {
                string month;
                if (Fmonth.ToString().Length == 1) month = "0" + Fmonth.ToString();
                else month = Fmonth.ToString();
                DataTable dt=new DataTable ();
               // sqlDB.fillDataTable("select convert(varchar(11),WeekendDate,105) as WeekendDate from Attendance_WeekendInfo where MonthName='" + month+'-'+Fyear + "'",dt);

                sqlDB.fillDataTable("select convert(varchar(11),WeekendDate,105) as WeekendDate from Attendance_WeekendInfo where WeekendDate >='" + Fyear + '-' + Fmonth + '-' + Fdays + "'AND WeekendDate <='" + Tyear + '-' + Tmonth + '-' + Tdays + "'", dt);

                DateTime begin = new DateTime(int.Parse(Fyear), Fmonth,Fdays);
                DateTime end = new DateTime(int.Parse(Tyear), Tmonth, Tdays);
                int totalWeekendDays=0;
                while (begin <= end)
                {
                    if (begin.DayOfWeek == DayOfWeek.Friday)
                    {
                        string[] getDates = begin.ToString().Split('/');
                        string getDate=(getDates[1].Length == 1)? "0" + getDates[1]:getDates[1];
                        string getMonth = (getDates[0].Length == 1) ?"0" + getDates[0] : getDates[0];
                        bool yesCount = false;
                        for (byte b = 0; b < dt.Rows.Count;b++ )
                        {
                            if (getDate + "-" + getMonth + "-" + Fyear == dt.Rows[b]["WeekendDate"].ToString())
                            {
                                yesCount = true;
                                break;
                            }
                        }

                        if (yesCount) totalWeekendDays += 1;
                    }
                     begin=begin.AddDays(1);
                }
                txtTotalHolydays.Text = totalWeekendDays.ToString();
            }
            catch { }
        }

        DataTable dt;
        protected void btnFind_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            if (txtEmpCardNo.Text.Trim().Length < 4)
            {
                lblMessage.InnerText = "error->Please type valid card no"; 
                return;
            }
            findEmpName();
        }

        private void findEmpName()
        {
            try
            {
             
                lblDepartment.Text = "";
                txtEmpName.Text = "";
                string CompanyId = (ddlBranch.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue.ToString();
                dt = new DataTable();
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("User"))
                    sqlDB.fillDataTable("select EmpId,EmpName,CompanyName,DptName,EmpTypeId,DptId,DsgId,SftId,convert(varchar(10),EmpJoiningDate,120) as  EmpJoiningDate from v_Personnel_EmpCurrentStatus where EmpId='" + ViewState["__EmpId__"].ToString()+ "'", dt);
                    else
                sqlDB.fillDataTable("select EmpId,EmpName,CompanyName,DptName,EmpTypeId,DptId,DsgId,SftId,convert(varchar(10),EmpJoiningDate,120) as  EmpJoiningDate from v_Personnel_EmpCurrentStatus where EmpCardNo like '%" + ddlEmpCardNo.SelectedValue+ "'" +
                         " AND IsActive='1' AND  EmpStatus in ('1','8')  " +
                         " AND CompanyId='" + CompanyId + "' AND DptId=" + ddlDepartment.SelectedValue + "", dt);

                if (dt.Rows.Count==0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ErrorMsg('Please check your selected information or type valid card no !');", true);                  
                    return;
                }

                //------------------------------------------------------------------------------------------------
                // this validation for user type employee.that will be give only access own data access
                //if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("User"))
                //{
                //    if (!classes.UserPrivilege.UserPrivilegeForUserType(txtEmpCardNo.Text, CompanyId, ViewState["__UserId__"].ToString()))
                //    {
                //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ErrorMsg('Please type your correct cardno !');", true);
                //      //  lblMessage.InnerText = "Please type your correct cardno !";
                //        return;
                    
                //    }
                //}
                //--------------------------------------------------------------------------------------------------

                txtEmpName.Text = dt.Rows[0]["empName"].ToString();
                lblDepartment.Text = "Department : " + dt.Rows[0]["DptName"].ToString();
                ViewState["EmpId"] = dt.Rows[0]["EmpId"].ToString();
                ViewState["__EmpTypeId__"] = dt.Rows[0]["EmpTypeId"].ToString();
                ViewState["__SftId__"] = dt.Rows[0]["SftId"].ToString();
                ViewState["__DptId__"] = dt.Rows[0]["DptId"].ToString();
                ViewState["__DsgId__"] = dt.Rows[0]["DsgId"].ToString();
                ViewState["__EmpJoiningDate__"] = dt.Rows[0]["EmpJoiningDate"].ToString();

                ViewState["__FindName__"] = "Yes";
            }
            catch { }
        }
       
        private bool validationExistsRecord()
        {
            try
            {
                if (txtEmpName.Text.Trim().Length == 0) return false;
                if (ddlLeaveName.SelectedValue.ToString().Equals("50"))  // 50 is default value which is denoted empty string 
                {
                    return false ;
                }
                if (ddlLeaveName.SelectedItem.Text == "M/L")
                {
                    if (txtPregnantDate.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ErrorMsg('Please Select Pregnant Date !');", true);
                       // lblMessage.InnerText = "warning->Please Select Pregnant Date";
                        txtPregnantDate.Focus();
                        return false;
                    }
                    else if (txtPregnantDate.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ErrorMsg('Please Select Prasabera Date !');", true);
                       // lblMessage.InnerText = "warning->Please Select Prasabera Date";
                        txtPregnantDate.Focus();
                        return false;
                    }
                }
                return true;
            }
            catch { return false; }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            Clear();
        }

        string[] fromDates;
        string[] toDates;
        protected void btnSaveLeave_Click(object sender, EventArgs e)
        {
           
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "User")
            {

                if (txtEmpName.Text.Trim().Length==0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('you are not exist here !');", true);                    
                    return;
                }
            }
            else 
            {
                if (ddlDepartment.SelectedValue=="0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('Please select any Department !');", true);
                    //lblMessage.InnerText = "warning-> Please select any Department !";
                    ddlDepartment.Focus();
                    return;
                }
                if (ddlEmpCardNo.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('Please select any Employee !');", true);
                    /// lblMessage.InnerText = "warning-> Please select any Employee !";
                    ddlEmpCardNo.Focus();
                    return;
                }
            }
            if (txtApplyDate.Text.Trim().Length < 8) 
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('Please select Valid Apply Date !');", true);
               // lblMessage.InnerText = "warning-> Please select Valid Apply Date !";
                txtApplyDate.Focus();
                return;
            }
            if (txtFromDate.Text.Trim().Length < 8)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('Please select Valid From Date !');", true);
               // lblMessage.InnerText = "warning-> Please select Valid From Date !";
                txtFromDate.Focus();
                return;
            }
            if (ckbIsHalfDayLeave.Checked)
                txtToDate.Text = txtFromDate.Text;
            if (txtToDate.Text.Trim().Length < 8)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('Please select Valid To Date !');", true);
               // lblMessage.InnerText = "warning-> Please select Valid To Date !";
                txtToDate.Focus();
                return;
            }
            if (ddlLeaveName.SelectedIndex < 1) 
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('Please select Leave Name !');", true);
                //lblMessage.InnerText = "warning-> Please select Leave Name !";
                ddlLeaveName.Focus();
                return;
            }
            //if (ddlHandOverPerson.SelectedIndex < 1)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('Please select Charge Handed Over Person!');", true);
            //    //lblMessage.InnerText = "warning-> Please select Leave Name !";
            //    ddlHandOverPerson.Focus();
            //    return;
            //}
            if (ddlLeaveName.SelectedItem.Text == "Earn Leave a/l")
            {
                if (!checkJoiningDate())
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('You are not capable to enjoy Earn leave facility.!');", true);
                    return;
                }
            }
            
            if (!checkReplacedDateValidation()) return;
            if (!checkLeaveDaysValidation()) return;
            //if (ViewState["__FindName__"].Equals("No"))

            fromDates = txtFromDate.Text.Trim().Split('-');
            toDates = txtToDate.Text.Trim().Split('-');

            if (ViewState["yesAlater"].Equals("True"))
            {
                if (!checkIsPending(ViewState["__LACode__"].ToString()))
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('The approved/rejected leave record can not be updated.');", true);
                    return;
                }

                DataTable dtLeaveDetails = new DataTable();

               
 
                string DailyStartTimeALT_CloseTime = "00:00:00:00:00:00";
                btnDateCalculation_Click(sender, e);

                if (!txtFromDate.Text.Trim().Equals(ViewState["OldFromDate"].ToString()) || !txtToDate.Text.Trim().Equals(ViewState["OldToDate"].ToString()))
                {
                    calculationForAlterOperation();

                    checkNotSameDate();
                    for (int i = 0; i < dtNotSameDate.Rows.Count; i++)
                    {
                        updateDailyAttendance(dtNotSameDate.Rows[i]["LeaveDate"].ToString(), "00","00", "00","00","00","00", DailyStartTimeALT_CloseTime, dtNotSameDate.Rows[i]["Status"].ToString());
                    }

                    SQLOperation.forDelete("temp_NewLeaveDateForAlter", sqlDB.connection);
                    SQLOperation.forDelete("temp_OldLeaveDateForAlter", sqlDB.connection);
                }
               

                SQLOperation.forDeleteRecordByIdentifier("Leave_LeaveApplication", "LACode", ViewState["__LACode__"].ToString(), sqlDB.connection);
                SQLOperation.forDeleteRecordByIdentifier("Leave_LeaveApplicationDetails", "LACode", ViewState["__LACode__"].ToString(), sqlDB.connection);

            }

            if (!checkLeaveDateExist()) return;
            saveLeaveApplication();
            ViewState["yesAlater"] = "False";
        }
        private bool checkJoiningDate()
        {
            try
            {
                DateTime empJoiningDate = DateTime.Parse(ViewState["__EmpJoiningDate__"].ToString());
                int count =0;
                for (DateTime Date = empJoiningDate; Date <= DateTime.Now; Date = Date.AddMonths(1))
                {
                    if (Date.ToString("MM").Equals("01"))
                        count++;
                    if (count > 1)
                        return true;                   
                }         
                return false;
            }
            catch (Exception ex)
            { return false; }
        }
        private bool checkIsPending(string LACode)
        {
            try
            {

                sqlDB.fillDataTable("select LACode from Leave_LeaveApplication where  IsApproved=0 and LACode=" + LACode, dt = new DataTable());
                if (dt != null && dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool checkLeaveDateExist() 
        {
            try
            {
                 
                sqlDB.fillDataTable("with temp as ("+
                    "SELECT  distinct LeaveDate = STUFF(( "+
                    " SELECT ',' + CONVERT(VARCHAR(10), LeaveDate, 105) "+
                    " FROM Leave_LeaveApplicationDetails where  LeaveDate >='" + fromDates[2] + "-" + fromDates[1] + "-" + fromDates[0] + "' and  LeaveDate <='" + toDates[2] + "-" + toDates[1] + "-" + toDates[0] + "' and EmpId='" + ViewState["EmpId"].ToString() + "'" +
                    " FOR XML PATH('') ), 1, 1, '') "+
                    " FROM Leave_LeaveApplicationDetails ) select LeaveDate from temp where LeaveDate is not null", dt = new DataTable());

                if (dt.Rows.Count == 0)
                    return true;
                else 
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('Already applied for " + dt.Rows[0]["LeaveDate"].ToString() + " !');", true);
                   // lblMessage.InnerText = "warning-> Already applied for " + dt.Rows[0]["LeaveDate"].ToString() + "  "; 
                    return false;
                }


            }
            catch { return false; }
        }
        private bool checkLeaveDaysValidation()
        {
            try
            {
                sqlDB.fillDataTable("select LeaveDays,ShortName from tblLeaveConfig where LeaveId="+ddlLeaveName.SelectedValue.ToString()+"",dt=new DataTable ());
               
                if (dt.Rows[0]["ShortName"].ToString().Equals("op/l") || dt.Rows[0]["ShortName"].ToString().Equals("o/l")) return true;  // because officeal purpose leave or others leave has not needed any validation

                byte getLeaveDays = byte.Parse(dt.Rows[0]["LeaveDays"].ToString());
                if (dt.Rows[0]["ShortName"].ToString().Equals("sr/l"))
                {
                    sqlDB.fillDataTable("select Used from v_Leave_LeaveApplicationDetails where EmpId='" + ViewState["EmpId"].ToString() + "' And ShortName=(select ShortName from tblLeaveConfig where LeaveId=" + ddlLeaveName.SelectedValue.ToString() + ") AND FORMAT(LeaveDate,'MM-yyyy')= '" + txtFromDate.Text.Remove(0, 3) + "' ", dt = new DataTable());
                    if (dt.Rows.Count + float.Parse(txtNoOfDays.Text.Trim()) > getLeaveDays)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ErrorMsg('Already you are spanted " + dt.Rows.Count + " days of  " + ddlLeaveName.SelectedItem.ToString().Substring(0, ddlLeaveName.SelectedItem.ToString().Length - 3) + " of this Month Total allocated days for " + ddlLeaveName.SelectedItem.ToString().Substring(0, ddlLeaveName.SelectedItem.ToString().Length - 3) + " is " + getLeaveDays + " !');", true);
                     //   lblMessage.InnerText = "error->Already you are spanted " + dt.Rows.Count + " days of  " + ddlLeaveName.SelectedItem.ToString().Substring(0, ddlLeaveName.SelectedItem.ToString().Length - 3) + " of this Month Total allocated days for " + ddlLeaveName.SelectedItem.ToString().Substring(0, ddlLeaveName.SelectedItem.ToString().Length - 3) + " is " + getLeaveDays + " !";
                        return false;
                    }
                    else return true;
                }
                else
                {
                    string[] y = txtFromDate.Text.Split('-');
                    //-------Leave balance for new joining employee------
                    if (dt.Rows[0]["ShortName"].ToString().Equals("c/l") || dt.Rows[0]["ShortName"].ToString().Equals("s/l"))
                    {
                        DateTime empJoiningDate = DateTime.Parse(ViewState["__EmpJoiningDate__"].ToString());
                        if (empJoiningDate.Year== int.Parse(y[2]))
                        {
                            if (dt.Rows[0]["ShortName"].ToString().Equals("c/l"))
                            sqlDB.fillDataTable("select CL as LeaveDays,'c/l' as  ShortName from Leave_LeaveBalanceForNewEmp where MOnthID='"+ empJoiningDate.ToString("MM")+ "'", dt = new DataTable());
                            else
                                sqlDB.fillDataTable("select SL as LeaveDays,'s/l' as  ShortName from Leave_LeaveBalanceForNewEmp where MOnthID='" + empJoiningDate.ToString("MM") + "'", dt = new DataTable());
                        }
                    }
                    //-------for new joining employee end------

                    sqlDB.fillDataTable("select  ISNULL(sum(ISNULL(Days,0)),0) as Days from Leave_MonthlyLeaveDeductionRecord where LeaveType=(select ShortName from tblLeaveConfig where LeaveId=" + ddlLeaveName.SelectedValue.ToString() + ") and Year(Month)='" + y[2]+"' and EmpID='" + ViewState["EmpId"].ToString() + "'", dt = new DataTable());
                    double deductedLv = 0;
                    if (dt!=null && dt.Rows.Count > 0)
                        deductedLv = double.Parse(dt.Rows[0]["Days"].ToString());
                    sqlDB.fillDataTable("select isnull( sum( case when  IsHalfDayLeave=1  then 0.5 else 1  end),0) as  spantedDays from v_Leave_LeaveApplicationDetails where EmpId='" + ViewState["EmpId"].ToString() + "' And ShortName=(select ShortName from tblLeaveConfig where LeaveId=" + ddlLeaveName.SelectedValue.ToString() + ") AND FromYear='" + y[2] + "' ", dt = new DataTable());
                    double spantedDays = double.Parse( dt.Rows[0]["spantedDays"].ToString());                    
                    if (spantedDays + deductedLv + float.Parse(txtNoOfDays.Text.Trim()) > getLeaveDays)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ErrorMsg('Already you are spanted " + dt.Rows.Count + " days of  " + ddlLeaveName.SelectedItem.ToString().Substring(0, ddlLeaveName.SelectedItem.ToString().Length - 3) + " of this Year Total allocated days for " + ddlLeaveName.SelectedItem.ToString().Substring(0, ddlLeaveName.SelectedItem.ToString().Length - 3) + " is " + getLeaveDays + " !');", true);                    
                        //lblMessage.InnerText = "error->Already you are spanted " + dt.Rows.Count + " days of  " + ddlLeaveName.SelectedItem.ToString().Substring(0, ddlLeaveName.SelectedItem.ToString().Length - 3) + " of this Year Total allocated days for " + ddlLeaveName.SelectedItem.ToString().Substring(0, ddlLeaveName.SelectedItem.ToString().Length - 3) + " is " + getLeaveDays + " !";
                        return false;
                    }
                    else return true;
                }
                

               
            }
            catch { return false; }
        }
        private bool checkReplacedDateValidation()
        {
            try
            {
                if (ddlLeaveName.SelectedItem.Text.Trim() == "Compensation/Replace Leave cr/l")
                {
                    if (txtReplacedDate.Text == "")
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('Please select or type Replaced Date(s)!');", true);
                        txtReplacedDate.Focus();
                        return false;
                    }
                    else 
                    {
                        getReplaceDate = txtReplacedDate.Text.Trim().Split(',');
                        
                        getReplaceDate = getReplaceDate.Distinct().ToArray();
                        if (txtNoOfDays.Text.Trim() != getReplaceDate.Length.ToString())
                        {
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('Please check Replaced Date(s), might be incorrect or Leave Day(s) and Replaced Date(s) is/are not equal!');", true);
                            txtReplacedDate.Focus();
                            return false;
                        }
                        sqlDB.fillDataTable("with temp as (" +
                    "SELECT  distinct RDate = STUFF(( " +
                    " SELECT ',' + CONVERT(VARCHAR(10), RDate, 105) " +
                    " FROM Leave_leaveReplacedDateLog where  EmpId='" + ViewState["EmpId"].ToString() + "' and  convert(varchar(10),RDate,105) in('" + txtReplacedDate.Text.Trim() + "')" +
                    " FOR XML PATH('') ), 1, 1, '') " +
                    " FROM Leave_leaveReplacedDateLog ) select RDate from temp where RDate is not null", dt = new DataTable());
                        if (dt.Rows.Count > 0) 
                        {
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('This/These date(s) ("+dt.Rows[0]["RDate"].ToString()+") has been used');", true);
                            txtReplacedDate.Focus();
                            return false;
                        }
                        return true;
                    }
                    
                }
                else { return true; }


            }
            catch
            { 
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('Please select or type Replaced Date(s)!');", true);
                   
                return false;
            }
        }

        private void updateDailyAttendance(string attDate, string InHour, string InMin, string InSec, string OutHur, string OutMin, string OutSec, string DailyStartTimeALT_CloseTime, string Status)
        {
            try
            {
                sqlDB.fillDataTable("select * from DailyAttendanceRecord where attDate='" + attDate + "'", dt = new DataTable());

                if (dt.Rows.Count > 0)
                {


                    string attStatus;
                    if (Status.Equals("Old"))
                    {
                        sqlDB.fillDataTable("select Convert(varchar(11),OffDate,105) as OffDate,Purpose from OffdaySettings where OffDate='" + attDate + "'", dt = new DataTable());
                        if (dt.Rows.Count > 0) attStatus = (dt.Rows[0]["Purpose"].ToString().Trim().Equals("Weekly Holiday")) ? "w" : "h";
                        else attStatus = "a";
                    }
                    else attStatus = "lv";

                    string stateStatus = (Status.Equals("Old")) ? "Absent" : ddlLeaveName.SelectedValue.ToString().Substring(0, ddlLeaveName.SelectedItem.ToString().Length - 3);


                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("update DailyAttendanceRecord set InHur='" + InHour + "',InMin='" + InMin + "', " +
                    "InSec='" + InSec + "',OutHur='" + OutHur + "',OutMin='" + OutMin + "',OutSec='" + OutSec + "',AttStatus='" + attStatus + "',StateStatus='" + stateStatus + "', " +
                    "AttManual='Manual Attendance',DailyStartTimeALT_CloseTime='" + DailyStartTimeALT_CloseTime + "',DId='" + ViewState["__SftId__"].ToString() + "' where EmpId=" + ViewState["EmpId"].ToString() + " and AttDate='" + attDate + "'", sqlDB.connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }

        DataTable dtNotSameDate;
        private void checkNotSameDate()
        {
            try
            {
                sqlDB.fillDataTable("select FORMAT(LeaveDate,'yyyy-MM-dd') as LeaveDate,Status from temp_NewLeaveDateForAlter where LeaveDate  not in (select LeaveDate from temp_OldLeaveDateForAlter)", dtNotSameDate = new DataTable());
                sqlDB.fillDataTable("select FORMAT(LeaveDate,'yyyy-MM-dd') as LeaveDate,Status from temp_OldLeaveDateForAlter where LeaveDate  not in (select LeaveDate from temp_NewLeaveDateForAlter)", dtNotSameDate);
            }
            catch { }
        }
        private void calculationForAlterOperation()
        {
            try
            {
                SQLOperation.forDelete("temp_NewLeaveDateForAlter", sqlDB.connection);
                SQLOperation.forDelete("temp_OldLeaveDateForAlter", sqlDB.connection);

                DateTime NewFromDate = new DateTime(int.Parse(txtFromDate.Text.Substring(6, 4)), int.Parse(txtFromDate.Text.Substring(3, 2)), int.Parse(txtFromDate.Text.Substring(0, 2)));
                DateTime NewToDate = new DateTime(int.Parse(txtToDate.Text.Substring(6, 4)), int.Parse(txtToDate.Text.Substring(3, 2)), int.Parse(txtToDate.Text.Substring(0, 2)));



                while (NewFromDate <= NewToDate)
                {
                    string[] newFromDates = NewFromDate.ToString().Split('/');
                    string getDay = (newFromDates[1].Length == 0) ? "0" + newFromDates[1] : newFromDates[1];
                    string getMonth = (newFromDates[0].Length == 0) ? "0" + newFromDates[0] : newFromDates[0];

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("insert into temp_NewLeaveDateForAlter (LeaveDate,Status) values(@LeaveDate,@Status)", sqlDB.connection);
                    cmd.Parameters.AddWithValue("@LeaveDate", commonTask.ddMMyyyyToyyyyMMdd(getDay + "-" + getMonth + "-" + newFromDates[2].Substring(0, 4)));
                    cmd.Parameters.AddWithValue("@Status", "New");
                    cmd.ExecuteNonQuery();

                    NewFromDate = NewFromDate.AddDays(1);
                }


                fromDates = ViewState["OldFromDate"].ToString().Split('-');
                toDates = ViewState["OldToDate"].ToString().Split('-');

                DateTime oldFromDate = new DateTime(int.Parse(fromDates[2]), int.Parse(fromDates[1]), int.Parse(fromDates[0]));
                DateTime oldToDate = new DateTime(int.Parse(toDates[2]), int.Parse(toDates[1]), int.Parse(toDates[0]));

                while (oldFromDate <= oldToDate)
                {
                    fromDates = oldFromDate.ToString().Split('/');
                    string getDay = (fromDates[1].Length == 0) ? "0" + fromDates[1] : fromDates[1];
                    string getMonth = (fromDates[0].Length == 0) ? "0" + fromDates[0] : fromDates[0];

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("insert into temp_OldLeaveDateForAlter (LeaveDate,Status) values(@LeaveDate,@Status)", sqlDB.connection);
                    cmd.Parameters.AddWithValue("@LeaveDate", commonTask.ddMMyyyyToyyyyMMdd(getDay + "-" + getMonth + "-" + fromDates[2].Substring(0, 4)));
                    cmd.Parameters.AddWithValue("@Status", "Old");
                    cmd.ExecuteNonQuery();
                    oldFromDate = oldFromDate.AddDays(1);
                }
            }
            catch { }
        }
        //protected void gvLeaveApplication_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    try
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        //        loadLeaveApplication();
        //        gvLeaveApplication.PageIndex = e.NewPageIndex;
        //        gvLeaveApplication.DataBind();
        //    }
        //    catch { }
        //}

        //protected void gvLeaveApplication_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (dtSetPrivilege.Rows.Count > 0)
        //        {
        //            if (bool.Parse(dtSetPrivilege.Rows[0]["__DeletAction__"].ToString()).Equals(false))
        //            {
        //                LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
        //                lnkDelete.Enabled = false;
        //                lnkDelete.OnClientClick = "return false";
        //                lnkDelete.ForeColor = Color.Black;
        //            }

        //            if (bool.Parse(dtSetPrivilege.Rows[0]["__UpdateAction__"].ToString()).Equals(false))
        //            {
        //                Button btn = (Button)e.Row.FindControl("btnAlter");
        //                btn.Enabled = false;
        //                btn.CssClass = "";
        //            }

        //        }
        //        else
        //        {
        //            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
        //            {
        //                LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
        //                lnkDelete.Enabled = false;
        //                lnkDelete.OnClientClick = "return false";
        //                lnkDelete.ForeColor = Color.Black;

        //                Button btn = (Button)e.Row.FindControl("btnAlter");
        //                btn.Enabled = false;
        //                btn.CssClass = "";
        //            }
        //        }
                
        //    }
        //    catch { }

        //    try
        //    {

        //    }
        //    catch { }
        //}

        //protected void gvLeaveApplication_DataBinding(object sender, EventArgs e)
        //{
            
        //}

        protected void btnDateCalculation_Click(object sender, EventArgs e)
        {
            try
            {
                if (ckbIsHalfDayLeave.Checked)
                    txtToDate.Text = txtFromDate.Text;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                string FromDate = txtFromDate.Text.Substring(6, 4) + "-" + txtFromDate.Text.Trim().Substring(3, 2) + "-" + txtFromDate.Text.Trim().Substring(0, 2);
                string ToDate = txtToDate.Text.Substring(6, 4) + "-" + txtToDate.Text.Trim().Substring(3, 2) + "-" + txtToDate.Text.Trim().Substring(0, 2);

                string CompanyId = (ddlBranch.SelectedIndex == 0) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue.ToString();
                sqlDB.fillDataTable("select distinct Convert(varchar(11),Offdate,105) as OffDate,Reason from v_AllOffDays where OffDate >='" + FromDate + "' AND OffDate<='"+ToDate+"'  AND CompanyId='" + CompanyId + "' ", dt = new DataTable());
               
                txtTotalHolydays.Text = dt.Rows.Count.ToString();

                sqlDB.fillDataTable("select DateDiff(Day,'" + FromDate + "','" + ToDate + "') as TotalDays", dt = new DataTable());
                txtNoOfDays.Text = (int.Parse(dt.Rows[0]["TotalDays"].ToString()) + 1).ToString();
                if (ckbIsHalfDayLeave.Checked)
                    txtNoOfDays.Text = "0.5";


            }
            catch { }
        }

        protected void ddlLeaveName_SelectedIndexChanged(object sender, EventArgs e)
        {
           ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            if (ddlLeaveName.SelectedItem.ToString().Contains("m/l"))
            {
                trpregnatn.Visible = true;
                trprasabera.Visible = true;
            }
            else
            {
                trpregnatn.Visible = false;
                trprasabera.Visible = false;
            }
            if (ddlLeaveName.SelectedItem.ToString().Contains("cr/l"))
            {
                trReplacedDate.Visible = true; 
            }
            else
                trReplacedDate.Visible = false;
            if (commonTask.IsAllowHalfDayLeave(ddlLeaveName.SelectedValue))
            {
                ckbIsHalfDayLeave.Checked = false;
                trHalfDayLeave.Visible = true;
            }
            else
            {
                ckbIsHalfDayLeave.Checked = false;
                trHalfDayLeave.Visible = false;
            }
        }
 
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);      
               // classes.commonTask.LoadShift(ddlShiftName,CompanyId);
                classes.commonTask.loadDepartmentListByCompany(ddlDepartment, ddlBranch.SelectedValue);
                classes.commonTask.loadLeaveNameByCompany(ddlLeaveName, ddlBranch.SelectedValue);
                ViewState["__IsLeaveAuthority__"] = commonTask.IsLeaveAuthority(ViewState["__CompanyId__"].ToString());
                //   loadLeaveApplication();
            }
            catch { }
        }

        //protected void ddlShiftName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        //        lblMessage.InnerText = "";
        //        lblSftTime.Text = "";
        //        string CompanyId = (ddlBranch.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue.ToString();
        //        sqlDB.fillDataTable("select  StartTime12Fromat,EndTime12Fomat From v_HRD_Shift where CompanyId='" + CompanyId + "' AND SftId=" + ddlShiftName.SelectedValue.ToString() + "", dt = new DataTable());
        //        if (dt.Rows.Count > 0) lblSftTime.Text = dt.Rows[0]["StartTime12Fromat"].ToString() + "-" + dt.Rows[0]["EndTime12Fomat"].ToString();

        //        loadLeaveApplication();  // load leave application
        //        classes.commonTask.LoadEmpCardNoByShift(ddlEmpCardNo, CompanyId, ddlShiftName.SelectedValue);
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.InnerText = "error->"+ex.Message;
        //    }
        //}

        protected void btnComplain_Click(object sender, EventArgs e)
        {
            Session["__forCompose__"] = "No";
            Session["__ModuleType__"] = "Leave";
            Session["__PreviousPage__"] = Request.ServerVariables["HTTP_REFERER"].ToString();       
            Response.Redirect("/mail/complain.aspx");
        }


        protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            //if (TabContainer1.ActiveTabIndex == 1) loadRejected_LeaveApplication();
            //else loadLeaveApplication();
        }

        //Md.Nayem 
        //Email=xxx@gmail.com
        //purpose : To set value for leave applicaiton report

        private void viewLeaveApplication(string LaCode)
        {
            try
            {
                string getSQLCMD;
                DataTable dt = new DataTable();
                DataTable dtApprovedRejectedDate = new DataTable();
                getSQLCMD = " SELECT LACode,CompanyId,ShortName,EmpId, Format(v_Leave_LeaveApplication.FromDate,'dd-MM-yyyy') as FromDate , Format(v_Leave_LeaveApplication.ToDate,'dd-MM-yyyy') as ToDate ,v_Leave_LeaveApplication.TotalDays,v_Leave_LeaveApplication.Remarks,"
                    + " v_Leave_LeaveApplication.EmpName, v_Leave_LeaveApplication.DptName, v_Leave_LeaveApplication.DsgName, v_Leave_LeaveApplication.CompanyName, "
                    + " v_Leave_LeaveApplication.SftName, Format(v_Leave_LeaveApplication.EntryDate,'dd-MM-yyyy') as EntryDate"
                    + " FROM"
                    + " dbo.v_Leave_LeaveApplication"
                    + " where LACode=" + LaCode + "";
                sqlDB.fillDataTable(getSQLCMD, dt);
               
                

                if (dt.Rows.Count == 0)
                {
                    getSQLCMD = "SELECT LACode ,CompanyId,ShortName,EmpId, Format(FromDate,'dd-MM-yyyy') as FromDate , Format(ToDate,'dd-MM-yyyy') as ToDate ,TotalDays,Remarks,EmpName,DptName,DsgName,CompanyName, SftName, Format(EntryDate,'dd-MM-yyyy') as EntryDate From dbo.v_Leave_LeaveApplication_Log where LACode="+LaCode+"";
                    sqlDB.fillDataTable(getSQLCMD, dt);
                
                }

                if (dt.Rows.Count == 0)
                {
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "WarningMsg('Sorry any record are not founded !');", true);
                   // lblMessage.InnerText = "warning->Sorry any record are not founded"; 
                    return;

                }

                Session["__Language__"] = "English";
                Session["__LeaveApplication__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LeaveApplication');", true);  //Open New Tab for Sever side code

            }
            catch { }
        }

        

        protected void ddlEmpCardNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
            txtCardNo.Text = ddlEmpCardNo.SelectedValue;
            findEmpName();
            classes.commonTask.LoadChargeHandedToOverPerson(ddlHandOverPerson, ddlBranch.SelectedValue, ddlEmpCardNo.SelectedValue);
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()) == "Admin")
                    classes.commonTask.LoadEmpCardNoByAdminAndDepartment(ddlEmpCardNo, ViewState["__UserId__"].ToString(), ddlDepartment.SelectedValue);
                else
                {
                    string CompanyId = (ddlBranch.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue.ToString();
                    // loadLeaveApplication();  // load leave application
                    classes.commonTask.LoadEmpCardNoByDepartment(ddlEmpCardNo, CompanyId, ddlDepartment.SelectedValue);
                }
               
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ErrorMsg('" + ex.Message + "');", true);
              
                //lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        //protected void gvRejectedList_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
               
        //    }
        //    catch { }
        //}

        //protected void gvRejectedList_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
                
        //        if (e.CommandName.Equals("View"))
        //        {
        //            string aa = e.CommandArgument.ToString();
        //            viewLeaveApplication(e.CommandArgument.ToString());
        //        }
        //    }
        //    catch { }
        //}


        private void SaveAttachDocument(string filename) 
        {
            
            if (FileUploadDoc.HasFile)
            {
                if (txtFileName.Text.Trim().Length > 0) 
                {
                    System.IO.File.Delete(Request.PhysicalApplicationPath + "/EmployeeImages/LeaveDocument/" + txtFileName.Text.Trim()+".jpg");
                }
                FileUploadDoc.SaveAs(Server.MapPath("/EmployeeImages/LeaveDocument/" + filename+".jpg"));
                //try
                //{                   

                //    System.Drawing.Image image = System.Drawing.Image.FromStream(FileUploadDoc.PostedFile.InputStream);
                //    int width = 595;
                //    int height = 842;
                //    using (System.Drawing.Image thumbnail = image.GetThumbnailImage(width, height, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero))
                //    {
                //        using (MemoryStream memoryStream = new MemoryStream())
                //        {
                //            thumbnail.Save(Server.MapPath("/EmployeeImages/LeaveDocument/" + filename+".jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                //        }
                //    }
                //}
                //catch { }
            }
        }

        protected void ckbIsHalfDayLeave_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbIsHalfDayLeave.Checked)
            {
                tdFromDate.InnerText = "Leave Date";
                trToDate.Visible = false;
            }
            else
            {
                tdFromDate.InnerText = "From Date";
                trToDate.Visible = true;

                txtToDate.Text = "";
            }
        }



       




    }
}