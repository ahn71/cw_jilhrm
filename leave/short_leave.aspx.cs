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

namespace SigmaERP.leave
{
    public partial class short_leave : System.Web.UI.Page
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
                    ddlBranch.Enabled = false;
                ddlBranch.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.commonTask.loadEmpCardNoByCompany(ddlEmpCardNo, ddlBranch.SelectedValue);
                txtLeaveDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                loadLeaveName();
                loadLeaveApplication();

            }
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



                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "aplication.aspx", ddlBranch, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

                if (ViewState["__ReadAction__"].ToString().Equals("0"))
                {
                    gvLeaveList.Visible = false;
                }
            }


            catch { }

        }
        private void loadLeaveName()
        {
            try
            {
                string CompanyId = (ddlBranch.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue.ToString();
                DataTable dt;
                sqlDB.fillDataTable("Select LeaveId, LeaveName+' '+shortName as LeaveName from tblLeaveConfig where CompanyId='" + CompanyId + "' and shortName='sr/l'", dt = new DataTable());
                ddlLeaveName.DataSource = dt;
                 ddlLeaveName.DataTextField = "LeaveName";
                 ddlLeaveName.DataValueField = "LeaveId";
                 ddlLeaveName.DataBind();
            }
            catch { }
        }

        private void saveLeaveApplication()
        {
            try
            {
                string CompanyId = (ddlBranch.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue.ToString();
               string FromTime  =( (ddlInTimeAMPM.SelectedValue.Equals("AM")) ? txtInHur.Text : (int.Parse(txtInHur.Text) + 12).ToString())+":"+ txtInMin.Text.Trim()+":00";
               string ToTime = (ddlOutTimeAMPM.SelectedValue.Equals("AM")) ? txtOutHur.Text : (int.Parse(txtOutHur.Text) + 12).ToString()+":"+ txtOutMin.Text.Trim()+":00";

               TimeSpan LvTime = TimeSpan.Parse(ToTime) - TimeSpan.Parse(FromTime);
               if (LvTime > TimeSpan.Parse("02:00:00"))
               {
                   lblMessage.InnerText = "warning-> Leave time cann't more then 2 Hours";
                   return;
               }
               string[] getColumns = { "LeaveID", "EmpId", "CompanyId", "LvDate", "FromTime", "ToTime", "LvTime", "Remarks", "LeaveProcessingOrder", "LvStatus"};

               string[] getValues = { ddlLeaveName.SelectedValue.ToString(), ddlEmpCardNo.SelectedValue, CompanyId, convertDateTime.getCertainCulture(txtLeaveDate.Text.Trim()).ToString(), FromTime, ToTime, LvTime.ToString(), txtRemark.Text.Trim(), getLeaveAuthority(), "0" };

                if (SQLOperation.forSaveValue("Leave_ShortLeave", getColumns, getValues, sqlDB.connection) == true)
                {
                    loadLeaveApplication();
                    lblMessage.InnerText = "success->Successfully Short Leave  Saved";
                    clear();
                }
            }

            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        private void UpdateLeaveApplication()
        {
            try
            {
                string FromTime = ((ddlInTimeAMPM.SelectedValue.Equals("AM")) ? txtInHur.Text : (int.Parse(txtInHur.Text) + 12).ToString()) + ":" + txtInMin.Text.Trim() + ":00";
                string ToTime = (ddlOutTimeAMPM.SelectedValue.Equals("AM")) ? txtOutHur.Text : (int.Parse(txtOutHur.Text) + 12).ToString() + ":" + txtOutMin.Text.Trim() + ":00";

                TimeSpan LvTime = TimeSpan.Parse(ToTime) - TimeSpan.Parse(FromTime);
                if (LvTime > TimeSpan.Parse("02:00:00"))
                {
                    lblMessage.InnerText = "warning-> Leave time cann't more then 2 Hours";
                    return;
                }
                string[] getColumns = { "LeaveID", "LvDate", "FromTime", "ToTime", "LvTime", "Remarks", "LeaveProcessingOrder", "LvStatus" };

                string[] getValues = { ddlLeaveName.SelectedValue.ToString(), convertDateTime.getCertainCulture(txtLeaveDate.Text.Trim()).ToString(), FromTime, ToTime, LvTime.ToString(), txtRemark.Text.Trim(), getLeaveAuthority(), "0" };

                if (SQLOperation.forUpdateValue("Leave_ShortLeave", getColumns, getValues,"SrLvID", ViewState["__SrLvId__"].ToString(), sqlDB.connection) == true)
                {
                    loadLeaveApplication();
                    lblMessage.InnerText = "success->Successfully Short Leave Updated.";
                    clear();
                }
            }

            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private void clear() 
        {
            ddlEmpCardNo.SelectedIndex = 0;
            txtLeaveDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            ddlInTimeAMPM.SelectedValue = "PM";
            ddlOutTimeAMPM.SelectedValue = "PM";
            txtInHur.Text = "00"; txtInMin.Text = "00";
            txtOutHur.Text = "00"; txtOutMin.Text = "00";
            txtRemark.Text = "";
            if (ViewState["__WriteAction__"].Equals("0"))
            {
                btnSave.Enabled = false;
                btnSave.CssClass = "";
            }
            else
            {
                btnSave.Enabled = true;
                btnSave.CssClass = "Lbutton";
            }
            btnSave.Text = "Save";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);

            if (ddlEmpCardNo.SelectedIndex < 1) 
            {
                lblMessage.InnerText = "warning-> Please, select any employe !";
                ddlEmpCardNo.Focus();
                return;
            }
            if (txtLeaveDate.Text.Trim().Length < 8)
            {
                lblMessage.InnerText = "warning-> Please, select leave date !";
                txtLeaveDate.Focus();
                return;
            }
            if (txtInHur.Text == "" || txtInHur.Text == "0" || txtInHur.Text == "00")
            {
                lblMessage.InnerText = "warning-> Please, enter from time  !";
                txtInHur.Focus();
                return;
            }
            if (txtOutHur.Text == "" || txtOutHur.Text == "0" || txtOutHur.Text == "00") 
            {
                lblMessage.InnerText = "warning-> Please, enter to time  !";
                txtOutHur.Focus();
                return;
            }
            if (!checkLeaveDaysValidation()) return;
            if (btnSave.Text.Trim() == "Save")
                saveLeaveApplication();
            else
                UpdateLeaveApplication();
        }
        private string getLeaveAuthority() 
        {
            try 
            {
                DataTable dtLvOrder ;
                sqlDB.fillDataTable("select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where  isLvAuthority=1 and DptId in(select DptId from Personnel_EmpCurrentStatus where EmpId='" + ddlEmpCardNo.SelectedValue + "')", dtLvOrder = new DataTable());
                if (!dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString().Equals("0"))
                    return dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString();
                else 
                {
                    sqlDB.fillDataTable("select ISNULL(max(LvAuthorityOrder),0) LvAuthorityOrder from v_UserAccountforLeave where isLvAuthority=1 and  LvOnlyDpt=0", dtLvOrder = new DataTable());                    
                        return dtLvOrder.Rows[0]["LvAuthorityOrder"].ToString();
                }
            }
            catch { return "0"; }
        }
        void loadLeaveApplication()  
        {
            DataTable dtLeaveInfo = new DataTable();
            string CompanyId = (ddlBranch.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue.ToString();


            sqlDB.fillDataTable("select SrLvID,EmpId,EmpCardNo,CompanyId,EmpName,FORMAT(LvDate,'dd-MM-yyyy') as LvDate,FromTime,ToTime,LvTime,Remarks,case  when LvStatus=0 then 'Pending' else case  when LvStatus=1 then 'Approved' else  case  when LvStatus=2 then 'Rejected' end end  end  as LvStatus "
                +"  from v_Leave_ShortLeave where CompanyId='" + CompanyId + "' Order by year(LvDate)desc, Month(LvDate)desc,LvDate desc", dtLeaveInfo);
            gvLeaveList.DataSource = dtLeaveInfo;
            gvLeaveList.DataBind();
        }

        protected void gvLeaveList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                
                int index = Convert.ToInt32(e.CommandArgument.ToString());
                string LvId = gvLeaveList.DataKeys[index].Value.ToString();
                ViewState["__SrLvId__"] = LvId;
                if (e.CommandName == "Alter")
                {                
                   
                    ViewState["yesAlater"] = "True";                    
                    SetValueToControl(LvId);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);

                }
                else if (e.CommandName == "remove")
                {
                    if (SQLOperation.forDeleteRecordByIdentifier("Leave_ShortLeave", "SrLvID", LvId.ToString(), sqlDB.connection) == true)
                    {
                        loadLeaveApplication();  
                      
                    }
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);

                   
                   
                }
                else if (e.CommandName.Equals("View"))
                {

                    viewLeaveApplication(LvId);
                }
            }
            catch { }
        }


        private void SetValueToControl(string LAId)
        {
            try
            {
                
                DataTable dtLocal = new DataTable();
                sqlDB.fillDataTable("select EmpId,CompanyId,FORMAT(LvDate,'dd-MM-yyyy') as LvDate,FromTime,ToTime,LvTime,Remarks  from Leave_ShortLeave where SrLvID='" + LAId + "'", dtLocal);
                 ViewState["EmpId"]= ddlEmpCardNo.SelectedValue = dtLocal.Rows[0]["EmpId"].ToString();
                 ViewState["__LvDate__"]= txtLeaveDate.Text = dtLocal.Rows[0]["LvDate"].ToString();
                TimeSpan FromTime =TimeSpan.Parse(dtLocal.Rows[0]["FromTime"].ToString());
                TimeSpan ToTime =TimeSpan.Parse( dtLocal.Rows[0]["ToTime"].ToString());
                if (FromTime.Hours >12)
                {
                    ddlInTimeAMPM.SelectedValue = "PM";
                    txtInHur.Text = (FromTime.Hours - 12).ToString();
                }
                else if (FromTime.Hours == 12)
                {
                    ddlInTimeAMPM.SelectedValue = "PM";
                    txtInHur.Text = FromTime.Hours .ToString();
                }
                else
                {
                    ddlInTimeAMPM.SelectedValue = "AM";
                    txtInHur.Text = FromTime.Hours.ToString();
                }

                if (ToTime.Hours > 12)
                {
                    ddlOutTimeAMPM.SelectedValue = "PM";
                    txtOutHur.Text = (ToTime.Hours - 12).ToString();
                }
                else if (ToTime.Hours == 12)
                {
                    ddlOutTimeAMPM.SelectedValue = "PM";
                    txtOutHur.Text = ToTime.Hours.ToString();
                }
                else
                {
                    ddlOutTimeAMPM.SelectedValue = "AM";
                    txtOutHur.Text = ToTime.Hours.ToString();
                }
                txtInMin.Text = FromTime.Minutes.ToString();
                txtOutMin.Text = ToTime.Minutes.ToString();
                txtRemark.Text = dtLocal.Rows[0]["Remarks"].ToString();
                btnSave.Text = "Update";
                if (ViewState["__UpdateAction__"].Equals("0"))
                {
                    btnSave.Enabled = false;
                    btnSave.CssClass = "";
                }
                else
                {
                    btnSave.Enabled = true;
                    btnSave.CssClass = "Lbutton";
                }

            }
            catch { }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            clear();
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            string CompanyId = (ddlBranch.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlBranch.SelectedValue.ToString();
            classes.commonTask.loadEmpCardNoByCompany(ddlEmpCardNo, CompanyId);           
            loadLeaveName();
            loadLeaveApplication();
        }
        private bool checkLeaveDaysValidation()
        {
            try
            {
                DataTable dt;
                sqlDB.fillDataTable("select LeaveDays,ShortName from tblLeaveConfig where LeaveId=" + ddlLeaveName.SelectedValue.ToString() + "", dt = new DataTable());
                byte getLeaveDays = byte.Parse(dt.Rows[0]["LeaveDays"].ToString());
                string EmpID = "";
                byte day;
                if(btnSave.Text == "Save")
                {                  
                   
                    sqlDB.fillDataTable("select SrLvID from Leave_ShortLeave where EmpId='" + ddlEmpCardNo.SelectedValue + "'  AND FORMAT(LvDate,'MM-yyyy')= '" + txtLeaveDate.Text.Remove(0, 3) + "' ", dt = new DataTable());
                }
                else
                {
                    sqlDB.fillDataTable("select SrLvID from Leave_ShortLeave where EmpId='" + ViewState["EmpId"].ToString() + "'  AND FORMAT(LvDate,'MM-yyyy')= '" + txtLeaveDate.Text.Remove(0, 3) + "' AND FORMAT(LvDate,'dd-MM-yyyy')<> '"+ViewState["__LvDate__"].ToString()+"'", dt = new DataTable());
                  
                }
           
              
                
                if ((dt.Rows.Count + 1) > getLeaveDays)
                    {
                        lblMessage.InnerText = "error->Already you are spanted " + dt.Rows.Count + " days of  " + ddlLeaveName.SelectedItem.ToString().Substring(0, ddlLeaveName.SelectedItem.ToString().Length - 3) + " of this Month Total allocated days for " + ddlLeaveName.SelectedItem.ToString().Substring(0, ddlLeaveName.SelectedItem.ToString().Length - 3) + " is " + getLeaveDays + " !";
                        return false;
                    }
                    else return true;
                
             



            }
            catch { return false; }
        }
        protected void gvLeaveList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ////try
            ////{
            ////    if (e.Row.RowType == DataControlRowType.DataRow)
            ////    {
            ////        e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
            ////        e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
            ////    }
            ////}
            ////catch { }
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
            {
                try
                {
                    if (ViewState["__DeletAction__"].ToString().Equals("0"))
                    {
                        Button lnkDelete = (Button)e.Row.FindControl("btnView");
                        lnkDelete.Enabled = false;
                        lnkDelete.OnClientClick = "return false";
                        lnkDelete.ForeColor = Color.Silver;
                    }

                }
                catch { }
                try
                {
                    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        Button lnkDelete = (Button)e.Row.FindControl("btnEdit");
                        lnkDelete.Enabled = false;
                        lnkDelete.ForeColor = Color.Silver;
                    }

                }
                catch { }
            }
        }
        private void viewLeaveApplication(string LaCode)
        {
            try
            {

                string getSQLCMD;
                DataTable dt = new DataTable();
                DataTable dtApprovedRejectedDate = new DataTable();
                getSQLCMD = " SELECT SrLvID,EmpId, EmpName, format(LvDate,'dd-MM-yyyy') as LvDate, FromTime, ToTime, LvTime, CompanyName, Address, DsgName, Remarks"
                    + " FROM"
                    + " v_Leave_ShortLeave"
                    + " where SrLvID=" + LaCode + "";
                sqlDB.fillDataTable(getSQLCMD, dt);
                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    lblMessage.InnerText = "warning->Sorry any payslip are not founded"; return;
                }
                Session["__Language__"] = "English";
                Session["__ShortLeaveApplication__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=ShortLeaveApplication');", true);  //Open New Tab for Sever side code

            }
            catch { }
        }
    }
}