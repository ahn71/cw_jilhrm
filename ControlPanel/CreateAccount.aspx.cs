using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using adviitRuntimeScripting;
using System.Data;
using SigmaERP.classes;

namespace SigmaERP.ControlPanel
{
    public partial class CreateAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
               // loadCompany();
                setPrivilege();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();                
                if (!classes.commonTask.HasBranch())
                    ddlCompany.Enabled = false;
                classes.commonTask.LoadEmpTypeWithAll(rblEmpType,"");
                classes.commonTask.loadEmpCardNoByCompanyAndEmpType(ddlEmpList, ddlCompany.SelectedValue,"1,2");
            }
        }

        private void setPrivilege()
        {
            try
            {
                ddlUserType.Items.Clear();
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForOnlyWriteAction(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "CreateAccount.aspx", ddlCompany, btnSave);



              
               

                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                {
                    ddlUserType.Items.Insert(0, new ListItem("User", "User"));
                    ddlUserType.Items.Insert(1, new ListItem("Admin", "Admin"));
                    ddlUserType.Items.Insert(2, new ListItem("Viewer", "Viewer"));
                    
                }
                else if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Developer"))
                {

                    ddlCompany.Enabled = true;
                    classes.commonTask.LoadBranch(ddlCompany);
                    ddlUserType.Items.Insert(0, new ListItem("User", "User"));                    
                    ddlUserType.Items.Insert(1, new ListItem("Admin", "Admin"));
                    //ddlUserType.Items.Insert(2, new ListItem("Super Admin", "Super Admin"));
                    ddlUserType.Items.Insert(2, new ListItem("Master Admin", "Master Admin"));
                    ddlUserType.Items.Insert(3, new ListItem("Viewer", "Viewer"));
                }
                else if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin"))
                {
                    ddlUserType.Items.Insert(0, new ListItem("User", "User"));
                    ddlUserType.Items.Insert(1, new ListItem("Admin", "Admin"));
                    ddlUserType.Items.Insert(2, new ListItem("Super Admin", "Super Admin"));
                    ddlUserType.Items.Insert(3, new ListItem("Viewer", "Viewer"));
                }

             

            }
            catch (Exception ex) { }

        }

        private void loadCompany()
        {
            try
            {

                string sqlCmd;
                HttpCookie getCookies = Request.Cookies["userInfo"];
                if (getCookies["__getUserType__"].ToString().Equals("Developer"))
                    sqlCmd = "select CompanyId,CompanyName from HRD_CompanyInfo";
                else
                {
                    ddlCompany.Enabled = false;
                    sqlCmd = "select CompanyId,CompanyName from HRD_CompanyInfo where CompanyId='" + getCookies["__CompanyId__"].ToString() + "'";
                }
                DataTable dt;
                sqlDB.fillDataTable(sqlCmd,dt=new DataTable () );
                ddlCompany.DataTextField = "CompanyName";
                ddlCompany.DataValueField = "CompanyId";
                ddlCompany.DataSource = dt;
                ddlCompany.DataBind();
               
            }
            catch { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (validationBasket() == true)
            {
                createUserAccount();
            }
        }
        private void createUserAccount()
        {
            try
            {
                string[] getColumns = { "UserName", "UserPassword", "Email", "UserType", "CreatedOn", "CreatedBy", "Status", "CoockieInfo", "CompanyId", "isLvAuthority", "LvAuthorityOrder", "LvAuthorityAction", "LvEmpType", "LvOnlyDpt" ,"EmpId"};
               string[] getValues = {ComplexLetters.getTangledLetters(txtUsername.Text.Trim()),ComplexLetters.getTangledLetters(txtPassword.Text.Trim()),txtEmail.Text.Trim(),ComplexLetters.getTangledLetters(ddlUserType.SelectedItem.ToString()),
                                     DateTime.Now.ToString("yyyy-MM-dd"),"1","1","",ddlCompany.SelectedValue.ToString(),(ckbLeaveAuthority.Checked)?"1":"0",txtOrder.Text,rblLeaveAuthority.SelectedValue,rblEmpType.SelectedValue,rblOnlyDepartment.SelectedValue,ddlEmpList.SelectedValue};
                if (SQLOperation.forSaveValue("UserAccount", getColumns, getValues,sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success->successfully new account created";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();",true);
                    ckbLeaveAuthority.Checked = false;
                    pLeaveAuthority.Visible = false;
                    txtOrder.Text = "";
                    rblEmpType.SelectedValue = "0";
                    rblOnlyDepartment.SelectedValue = "0";
                    rblLeaveAuthority.SelectedValue = "1";
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                lblMessage.InnerText="error->"+ex.Message;
            }
        }

        private bool validationBasket()
        {
            try
            {
               
                //if (txtFirstName.Text.Trim().Length <= 2)
                //{
                //    txtFirstName.Focus();
                //    lblMessage.InnerText = "warning->First name required minimum 3 characters !";
                //    return false;
                //}
                //if (txtLastName.Text.Trim().Length <= 2)
                //{
                //    txtLastName.Focus();
                //    lblMessage.InnerText = "warning->Last name required minimum 3 characters !";
                //    return false;
                //}
                if (ddlEmpList.SelectedIndex <1)               
                    {
                        ddlEmpList.Focus();
                        lblMessage.InnerText = "warning->Please select employee !";
                        return false;
                    }
                
                if (txtUsername.Text.Trim().Length <= 3)
                {
                    txtUsername.Focus();
                    lblMessage.InnerText = "warning->User name required minimum 4 characters !";
                    return false;
                }
                if (txtPassword.Text.Trim().Length <= 2)
                {
                    txtPassword.Focus();
                    lblMessage.InnerText = "warning->User password required minimum 4 characters !";
                    return false;
                }
                if (txtPassword.Text.Trim().CompareTo(txtConfirmPassword.Text.Trim()) == 1)
                {
                    txtConfirmPassword.Focus();
                    lblMessage.InnerText = "warning->Confirm password miss match with main password !";
                    return false;
                }
                if (ckbLeaveAuthority.Checked && txtOrder.Text.Trim()=="" )
                {
                    txtOrder.Focus();
                    lblMessage.InnerText = "warning->Please enter authority position no!";
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        protected void ckbLeaveAuthority_CheckedChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
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

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.loadEmpCardNoByCompanyAndEmpType(ddlEmpList, ddlCompany.SelectedValue, "1,2");
        }
    }
}