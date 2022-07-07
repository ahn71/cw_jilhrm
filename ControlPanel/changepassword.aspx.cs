using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using ComplexScriptingSystem;
using adviitRuntimeScripting;
using System.Data;
using SigmaERP.classes;
using System.Drawing;

namespace SigmaERP.ControlPanel
{
    public partial class changepassword : System.Web.UI.Page
    {
        DataTable dt;
      static  DataTable dtprivilege;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
               
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.commonTask.LoadEmpTypeWithAll(rblEmpType, "");
                classes.commonTask.LoadEmpTypeWithAll(rblEmpTypeOD, "");
            }
        }

        private void setPrivilege()
        {
            try
            {
               

                
                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserId__"] = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();

                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(),getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()),"changepassword.aspx",ddlCompanyList,gvAccountList,btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];             
                classes.commonTask.LoadShift(ddlShift, ViewState["__CompanyId__"].ToString());
                loadUserInfo();
                
          

                

            }
            catch { }

        }
        private void loadUserInfoForSuperAdmin() 
        {
            try
            {
                SQLOperation.selectBySetCommandInDatatable("select UserId,FirstName,LastName,UserName,UserPassword,UserType,Status from UserAccount where CompanyId='" + ViewState["__CompanyId__"].ToString() + "'and UserType not in('CwNUgpy/R6vrIxIvnT1Brw==' ,'XgWQYNdKrQtMhGZAvmF98Q==')" +
                    " union Select UserId,FirstName,LastName,UserName,UserPassword,UserType,Status From UserAccount where UserId=" + ViewState["__UserId__"].ToString() + " ", dt = new DataTable(), sqlDB.connection);
                gvAccountList.DataSource = dt;
                gvAccountList.DataBind();
            }
            catch { }
        }

        private void loadUserInfo()
        {
            try
            {
                string CompanyId=(ddlCompanyList.SelectedValue.ToString().Equals("0000"))?ViewState["__CompanyId__"].ToString():ddlCompanyList.SelectedItem.Value.ToString();
                string IsLvAuthority = "";// (rblShowType.SelectedValue.Equals("1")) ? " and  isLvAuthority=1" : "";
                if (rblShowType.SelectedValue.Equals("1"))
                    IsLvAuthority = " and  isLvAuthority=1";
                else if (rblShowType.SelectedValue.Equals("2"))
                    IsLvAuthority = " and  isODAuthority=1";
                
                //-------------------------For Master Admin-------------------
                /*
                   if (ComplexLetters.getEntangledLetters(ViewState["__getUserType__"].ToString()).Equals("Master Admin"))
                   {
                       if (ddlShift.SelectedIndex == 0) SQLOperation.selectBySetCommandInDatatable("select UserId,LastName,UserName,UserPassword,UserType,Status from UserAccount where CompanyId='" + CompanyId + "' ", dt = new DataTable(), sqlDB.connection);
                       else SQLOperation.selectBySetCommandInDatatable("select UserId,LastName,UserName,UserPassword,UserType,Status from UserAccount where CompanyId='" + CompanyId + "' AND SftId=" + ddlShift.SelectedValue.ToString() + " ", dt = new DataTable(), sqlDB.connection);
                   }
                   //------------------------------------------------------------------

                       // ------------------------For Supper Admin-----------------------------
                   if (ViewState["__getUserType__"].ToString().Equals("CwNUgpy/R6vrIxIvnT1Brw=="))
                   {
                       if (ddlShift.SelectedIndex == 0) SQLOperation.selectBySetCommandInDatatable("select UserId,LastName,UserName,UserPassword,UserType,Status from UserAccount where CompanyId='" + CompanyId + "' AND (UserType !='CwNUgpy/R6vrIxIvnT1Brw==' AND UserType!='XgWQYNdKrQtMhGZAvmF98Q==') union Select UserId,LastName,UserName,UserPassword,UserType,Status From UserAccount where UserId=" + ViewState["__UserId__"].ToString() + "", dt = new DataTable(), sqlDB.connection);
                       else SQLOperation.selectBySetCommandInDatatable("select UserId,LastName,UserName,UserPassword,UserType,Status from UserAccount where CompanyId='" + CompanyId + "' AND SftId=" + ddlShift.SelectedValue.ToString() + " ", dt = new DataTable(), sqlDB.connection);
                   }
                   else 
                   {
                       if (ddlShift.SelectedIndex == 0) SQLOperation.selectBySetCommandInDatatable("select UserId,LastName,UserName,UserPassword,UserType,Status from UserAccount where CompanyId='" + CompanyId + "' AND (UserType !='CwNUgpy/R6vrIxIvnT1Brw==XgWQYNdKrQtMhGZAvmF98Q' AND UserType!='XgWQYNdKrQtMhGZAvmF98Q==')", dt = new DataTable(), sqlDB.connection);
                       else SQLOperation.selectBySetCommandInDatatable("select UserId,LastName,UserName,UserPassword,UserType,Status from UserAccount where CompanyId='" + CompanyId + "' AND SftId=" + ddlShift.SelectedValue.ToString() + "  AND (UserType !='CwNUgpy/R6vrIxIvnT1Brw==' AND UserType!='==')", dt = new DataTable(), sqlDB.connection);
                   }

                       */

                    //------------------------------End--------------------------------------


                    // ------------------------For Supper Admin-----------------------------
                if (ViewState["__UserType__"].ToString().Equals("CwNUgpy/R6vrIxIvnT1Brw=="))
                {
                    if (ddlShift.SelectedIndex == 0) SQLOperation.selectBySetCommandInDatatable("select LvAuthorityOrder,UserId,EmpName,UserName,UserPassword,UserType,Status from v_UserAccount where CompanyId='" + CompanyId + "' " + IsLvAuthority + " AND (UserType !='CwNUgpy/R6vrIxIvnT1Brw==' AND UserType!='XgWQYNdKrQtMhGZAvmF98Q==') union LvAuthorityOrder,UserId,EmpName,UserName,UserPassword,UserType,Status from v_UserAccount where UserId=" + ViewState["__UserId__"].ToString() + "", dt = new DataTable(), sqlDB.connection);
                    else SQLOperation.selectBySetCommandInDatatable("select LvAuthorityOrder,UserId,EmpName,UserName,UserPassword,UserType,Status from v_UserAccount where CompanyId='" + CompanyId + "' " + IsLvAuthority + " AND SftId=" + ddlShift.SelectedValue.ToString() + " ", dt = new DataTable(), sqlDB.connection);
                }
                else 
                {
                    if (ddlShift.SelectedIndex == 0) SQLOperation.selectBySetCommandInDatatable("select LvAuthorityOrder,UserId,EmpName,UserName,UserPassword,UserType,Status from v_UserAccount where CompanyId='" + CompanyId + "' " + IsLvAuthority + " ", dt = new DataTable(), sqlDB.connection);
                    else SQLOperation.selectBySetCommandInDatatable("select LvAuthorityOrder,UserId,EmpName,UserName,UserPassword,UserType,Status from v_UserAccount where CompanyId='" + CompanyId + "' " + IsLvAuthority + " AND SftId=" + ddlShift.SelectedValue.ToString() + " ", dt = new DataTable(), sqlDB.connection);
                }
                gvAccountList.DataSource = dt;
                gvAccountList.DataBind();
            }
            catch (Exception ex)
            { 
            
            }
        }
        
        protected void chkShowUserNamePassword_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!chkShowUserNamePassword.Checked)
                {
                    txtUserName.Text = ComplexLetters.getTangledLetters(txtUserName.Text.Trim());
                    txtPassword.Text = ComplexLetters.getTangledLetters(txtPassword.Text.Trim());
                }
                else
                {
                    
                    txtUserName.Text = ComplexLetters.getEntangledLetters(txtUserName.Text.Trim());
                    txtPassword.Text = ComplexLetters.getEntangledLetters(txtPassword.Text.Trim());
                }

                /*
                if (hfStatus.Value == "0")
                {
                    txtUserName.Text = ComplexLetters.getEntangledLetters(txtUserName.Text.Trim());
                    txtPassword.Text = ComplexLetters.getEntangledLetters(txtPassword.Text.Trim());
                    hfStatus.Value = "1";
                }
                else
                {
                    txtUserName.Text = ComplexLetters.getTangledLetters(txtUserName.Text.Trim());
                    txtPassword.Text = ComplexLetters.getTangledLetters(txtPassword.Text.Trim());
                    hfStatus.Value = "0";
                }
                 * */
                txtNewPassword.Text = "";
            }
            catch { }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            clear();
            loadUserInfo();
        }

        private void clear()
        {
            try
            {
                txtFirstName.Text = "";
               // txtLastName.Text = "";
                txtUserName.Text = "";
                txtPassword.Text = "";
                txtNewPassword.Text = "";
                txtEmail.Text = "";
                ddlUserType.SelectedIndex = 0;
                chkStatus.Checked = false;
                hfStatus.Value = "0";
                chkShowUserNamePassword.Checked = false;

            }
            catch { }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
                lblMessage.InnerText = "";
                if (ViewState["__UserId__"] == null) return;
                //if (txtNewPassword.Text.Trim() == "")
                //{
                //    lblMessage.InnerText = "warning->Please type new password";
                //    loadUserInfo();
                //    return;
                //}
                if (ckbLeaveAuthority.Checked && txtOrder.Text.Trim() == "")
                {
                    txtOrder.Focus();
                    lblMessage.InnerText = "warning->Please enter leave authority position no!";
                    return;
                }
                if (ckbODAuthority.Checked && txtOrderOD.Text.Trim() == "")
                {
                    txtOrder.Focus();
                    lblMessage.InnerText = "warning->Please enter outduty authority position no!";
                    return;
                }
                byte status = (chkStatus.Checked) ? (byte)1 : (byte)0;
                
                if (txtNewPassword.Text.Trim() == "")
                {
                    string[] getColumns ={"Email", "UserType", "Status", "isLvAuthority", "LvAuthorityOrder", "LvAuthorityAction", "LvEmpType", "LvOnlyDpt", "isOdAuthority", "OdAuthorityOrder", "OdAuthorityAction", "OdEmpType", "OdOnlyDpt" };
                    string[] getValues = {txtEmail.Text.Trim(), ComplexLetters.getTangledLetters(ddlUserType.Text.Trim()), status.ToString(), (ckbLeaveAuthority.Checked) ? "1" : "0", txtOrder.Text, rblLeaveAuthority.SelectedValue, rblEmpType.SelectedValue, rblOnlyDepartment.SelectedValue, (ckbODAuthority.Checked) ? "1" : "0", txtOrderOD.Text, rblODAuthority.SelectedValue, rblEmpTypeOD.SelectedValue, rblOnlyDepartmentOD.SelectedValue };
                    if (SQLOperation.forUpdateValue("UserAccount", getColumns, getValues, "UserId", ViewState["__UserId__"].ToString(), sqlDB.connection) == true)
                    {
                        lblMessage.InnerText = "success->Successfully Account Updated";
                        clear();
                        ViewState["__UserId__"] = null;
                        loadUserInfo();
                    }
                }
                else
                {
                    string[] getColumns = { "UserPassword", "Email", "UserType", "Status", "isLvAuthority", "LvAuthorityOrder", "LvAuthorityAction", "LvEmpType", "LvOnlyDpt", "isOdAuthority", "OdAuthorityOrder", "OdAuthorityAction", "OdEmpType", "OdOnlyDpt" };
                    string[] getValues = { ComplexLetters.getTangledLetters(txtNewPassword.Text.Trim()), txtEmail.Text.Trim(), ComplexLetters.getTangledLetters(ddlUserType.Text.Trim()), status.ToString(), (ckbLeaveAuthority.Checked) ? "1" : "0", txtOrder.Text, rblLeaveAuthority.SelectedValue, rblEmpType.SelectedValue, rblOnlyDepartment.SelectedValue, (ckbODAuthority.Checked) ? "1" : "0", txtOrderOD.Text, rblODAuthority.SelectedValue, rblEmpTypeOD.SelectedValue, rblOnlyDepartmentOD.SelectedValue };
                    if (SQLOperation.forUpdateValue("UserAccount", getColumns, getValues, "UserId", ViewState["__UserId__"].ToString(), sqlDB.connection) == true)
                    {
                        lblMessage.InnerText = "success->Successfully Account Updated";
                        clear();
                        ViewState["__UserId__"] = null;
                        loadUserInfo();
                    }
                }
                
            }
            catch (Exception ex)
            {
                
            }
        }

        protected void gvAccountList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                int rIndex = Convert.ToInt32(e.CommandArgument);
                ViewState["__UserId__"] = gvAccountList.DataKeys[rIndex].Value.ToString();
                if (e.CommandName.Equals("Alter"))
                {
                    
                    sqlDB.fillDataTable("select * from v_UserAccount where UserId=" + gvAccountList.DataKeys[rIndex].Value.ToString() + "",dt=new DataTable ());
                    
                    txtFirstName.Text = dt.Rows[0]["EmpName"].ToString();
                   // txtLastName.Text = dt.Rows[0]["LastName"].ToString();
                    if (chkShowUserNamePassword.Checked)
                    {
                        txtUserName.Text =ComplexLetters.getEntangledLetters(dt.Rows[0]["UserName"].ToString());
                        txtPassword.Text =ComplexLetters.getEntangledLetters(dt.Rows[0]["UserPassword"].ToString());
                    }
                    else
                    {
                        txtUserName.Text = dt.Rows[0]["UserName"].ToString();
                        txtPassword.Text = dt.Rows[0]["UserPassword"].ToString();
                    }
                    txtEmail.Text = dt.Rows[0]["Email"].ToString();

                    ddlUserType.SelectedValue = ComplexLetters.getEntangledLetters(dt.Rows[0]["UserType"].ToString());

                    chkStatus.Checked = bool.Parse(dt.Rows[0]["Status"].ToString());
                    try { ckbLeaveAuthority.Checked = bool.Parse(dt.Rows[0]["isLvAuthority"].ToString()); }
                    catch { ckbLeaveAuthority.Checked = false; }
                    if (ckbLeaveAuthority.Checked)
                    {
                        pLeaveAuthority.Visible = true;
                        txtOrder.Text = dt.Rows[0]["LvAuthorityOrder"].ToString();
                        rblEmpType.SelectedValue = dt.Rows[0]["LvEmpType"].ToString();
                        rblOnlyDepartment.SelectedValue = (dt.Rows[0]["LvOnlyDpt"].ToString().Equals("True")) ? "1" : "0";
                        rblLeaveAuthority.SelectedValue =  dt.Rows[0]["LvAuthorityAction"].ToString();
                    }
                    else
                    {
                        txtOrder.Text = "";
                        rblEmpType.SelectedValue = "0";
                        rblOnlyDepartment.SelectedValue = "0";
                        rblLeaveAuthority.SelectedValue = "1";
                        pLeaveAuthority.Visible = false;
                    }
                    try { ckbODAuthority.Checked = bool.Parse(dt.Rows[0]["isODAuthority"].ToString()); }
                    catch { ckbODAuthority.Checked = false; }
                    if (ckbODAuthority.Checked)
                    {
                        pODAuthority.Visible = true;
                        txtOrderOD.Text = dt.Rows[0]["ODAuthorityOrder"].ToString();
                        rblEmpTypeOD.SelectedValue = dt.Rows[0]["ODEmpType"].ToString();
                        rblOnlyDepartmentOD.SelectedValue = (dt.Rows[0]["ODOnlyDpt"].ToString().Equals("True")) ? "1" : "0";
                        rblODAuthority.SelectedValue = dt.Rows[0]["ODAuthorityAction"].ToString();
                    }
                    else
                    {
                        txtOrderOD.Text = "";
                        rblEmpTypeOD.SelectedValue = "0";
                        rblOnlyDepartmentOD.SelectedValue = "0";
                        rblODAuthority.SelectedValue = "1";
                        pODAuthority.Visible = false;
                    }
                    if (ViewState["__UpdateAction__"].Equals("0"))
                    {
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "Tbutton";
                    }
                }
                else if (e.CommandName.Equals("Remove")) 
                {
                    if (rIndex == 0) 
                    {
                        lblMessage.InnerText = "warning->This account isn't deletable."; return;
                    }
                    if (SQLOperation.forDeleteRecordByIdentifier("UserAccount", "UserId", ViewState["__UserId__"].ToString(), sqlDB.connection) == true) 
                    {
                        lblMessage.InnerText = "success->Succsessfully Account Deleted.";
                        gvAccountList.Rows[rIndex].Visible = false;
                    }
                }

                
            }
            catch { }
        }
        protected void ckbLeaveAuthority_CheckedChanged(object sender, EventArgs e)
        {
           // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            if (ckbLeaveAuthority.Checked)
                pLeaveAuthority.Visible = true;

            else
            {
                txtOrder.Text = "";
                rblEmpType.SelectedValue = "0";
                rblOnlyDepartment.SelectedValue = "0";
                rblLeaveAuthority.SelectedValue = "1";
                pLeaveAuthority.Visible = false;
            }

        }

        protected void gvAccountList_RowDataBound(object sender, GridViewRowEventArgs e)
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
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
            {
                try
                {
                    if (ViewState["__DeletAction__"].ToString().Equals("0"))
                    {
                        Button lnkDelete = (Button)e.Row.FindControl("btnRemove");
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
                        Button lnkDelete = (Button)e.Row.FindControl("btnAlter");
                        lnkDelete.Enabled = false;
                        lnkDelete.ForeColor = Color.Silver;
                    }

                }
                catch { }
            }
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedItem.Value.ToString();
            classes.commonTask.LoadShift(ddlShift,CompanyId);
            loadUserInfo();
            clear();
            
        }

        protected void ddlShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadUserInfo();
        }

        protected void rblShowType_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadUserInfo();
        }

        protected void ckbODAuthority_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbODAuthority.Checked)
                pODAuthority.Visible = true;

            else
            {
                txtOrderOD.Text = "";
                rblEmpTypeOD.SelectedValue = "0";
                rblOnlyDepartmentOD.SelectedValue = "0";
                rblODAuthority.SelectedValue = "1";
                pODAuthority.Visible = false;
            }
        }
    }
}