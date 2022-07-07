using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.hrd
{
    public partial class line_config : System.Web.UI.Page
    {
        string compnayId = "";
        string sqlcmd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if(!IsPostBack)
            {
                
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                
            }
        }
        private void GroupORLineDependency() 
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("select * from HRD_SubDepartmentInfo", dt);
            if (dt.Rows[0]["HasSubDepartment"].ToString().Equals("False"))
            {
                ViewState["__Dependency__"] = "False";
                trDepartment.Visible = false;
                loadDesignation();
            }
            else { 
                trDepartment.Visible = true;
                ViewState["__Dependency__"] = "True";
                classes.commonTask.loadDepartmentListByCompany(dlDepartment, ViewState["__CompanyId__"].ToString());
            }
        }
        private void setPrivilege()
        {
            try
            {
                
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__preRIndex__"] = "No";
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "line_config.aspx", ddlCompanyName, divDesignationList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
              
                GroupORLineDependency();      
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();

            }
            catch { }

        }
        private void loadDesignation()
        {
            try
            {
                compnayId = (ddlCompanyName.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue;
          
                    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
                    {
                        sqlcmd = "SELECT GId,CompanyId,DptId,CompanyName, DptNameBn,DptName, GName,GNameBn, IsActive FROM V_HRD_Group where CompanyId='" + compnayId + "' ";
                    }
                    else
                    {
                        sqlcmd = "SELECT GId,CompanyId,DptId,CompanyName, DptNameBn,DptName, GName,GNameBn, IsActive FROM V_HRD_Group where CompanyId='" + ViewState["__CompanyId__"] + "'";
                    }
              
                DataTable dt = new DataTable();
                sqlDB.fillDataTable(sqlcmd, dt);
                divDesignationList.DataSource = dt;
                divDesignationList.DataBind();
                
               

            }
            catch { }
        }
        private void loadLineOrGroupByDepartment()
        {
            try
            {
                compnayId = (ddlCompanyName.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue;
             
                    if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
                    {
                        sqlcmd = "SELECT GId,CompanyId,DptId,CompanyName, DptName, GName,GNameBn, IsActive FROM V_HRD_Group where CompanyId='" + compnayId + "' and DptId='"+dlDepartment.SelectedValue+"' ";
                    }
                    else
                    {
                        sqlcmd = "SELECT GId,CompanyId,DptId,CompanyName, DptName, GName,GNameBn, IsActive FROM V_HRD_Group where CompanyId='" + ViewState["__CompanyId__"] + "' and DptId='" + dlDepartment.SelectedValue + "'";
                    }                
                DataTable dt = new DataTable();
                sqlDB.fillDataTable(sqlcmd, dt);
                divDesignationList.DataSource = dt;
                divDesignationList.DataBind();
                

            }
            catch { }
        }

        protected void divDesignationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                if (e.CommandName.Equals("Alter"))
                {
                    if (!ViewState["__preRIndex__"].ToString().Equals("No")) divDesignationList.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    divDesignationList.Rows[rIndex].BackColor = System.Drawing.Color.Yellow;
                    ViewState["__preRIndex__"] = rIndex;
                    setValueToControl(rIndex, divDesignationList.DataKeys[rIndex].Values[0].ToString(), divDesignationList.DataKeys[rIndex].Values[1].ToString(), divDesignationList.DataKeys[rIndex].Values[2].ToString());
                    if (ViewState["__UpdateAction__"].Equals("0"))
                    {
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "Rbutton";
                    }
                    btnSave.Text = "Update";
                }
                else if (e.CommandName.Equals("Delete"))
                {
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    if (deleteValidation(divDesignationList.DataKeys[rIndex].Values[0].ToString()))
                    {
                        SQLOperation.forDeleteRecordByIdentifier("HRD_Group", "GId", divDesignationList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "deleteSuccess()", true);
                        allClear();
                        lblMessage.InnerText = "success->Successfully Line/Group Deleted";
                    }
                    else
                        lblMessage.InnerText = "error->Warning! Can't delete this Line/Group. It is used for emplloyes.";


                }
            }
            catch { }
        }
        private void setValueToControl(int rIndex, string getSL,string CompanyId,string DptId)
        {
            try
            {
                    ViewState["__getSL__"] = getSL;              
                    ddlCompanyName.SelectedValue = CompanyId;
                    if (ViewState["__Dependency__"].ToString().Equals("True"))
                    {
                        classes.commonTask.SearchDepartment(ddlCompanyName.SelectedValue, dlDepartment);
                        dlDepartment.SelectedValue = DptId;
                    }
                txtLine.Text = divDesignationList.Rows[rIndex].Cells[2].Text;
                if (divDesignationList.Rows[rIndex].Cells[3].Text == "&nbsp;")
                    txtLineBn.Text = "";
                else txtLineBn.Text = divDesignationList.Rows[rIndex].Cells[3].Text;               
                if (divDesignationList.Rows[rIndex].Cells[4].Text == "True")
                {
                    dlStatus.SelectedValue = "1";
                }
                else
                {
                    ddlCompanyName.SelectedValue = "0";
                }
            }
            catch { }
        }
        private void allClear()
        {
            txtLine.Text = "";
            txtLineBn.Text = "";
            dlStatus.SelectedIndex = 0;
            hdnbtnStage.Value = "";
            hdnUpdate.Value = "";
            if (ViewState["__WriteAction__"].Equals("0"))
            {
                btnSave.Enabled = false;
                btnSave.CssClass = "";
            }
            else
            {
                btnSave.Enabled = true;
                btnSave.CssClass = "Rbutton";
            }
           
            //dlDepartment.Items.Clear();
            btnSave.Text = "Save";
        }

        private Boolean SaveDesignation()
        {
            try
            {    
                SqlCommand cmd = new SqlCommand("insert into HRD_Group  values('"+ddlCompanyName.SelectedValue+"', '" + dlDepartment.SelectedValue + "' ,'" + txtLine.Text + "',N'" + txtLineBn.Text + "'," + dlStatus.SelectedValue + ") ", sqlDB.connection);
                cmd.ExecuteNonQuery();                
                return true;
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
                if (ViewState["__Dependency__"].ToString().Equals("True"))
                    loadLineOrGroupByDepartment();
                else
                loadDesignation();
                return false;
            }
        }

        private Boolean UpdateDesignation()
        {
            try
            {
                int st;
                if (
                    dlStatus.Text.Equals("Active")) st = 1;
                else st = 0;
                string getIdentifierValue = ViewState["__getSL__"].ToString();
                SqlCommand cmd = new SqlCommand("Update HRD_Group set DptId='" + dlDepartment.SelectedValue + "', GName='" + txtLine.Text + "', GNameBn=N'" + txtLineBn.Text + "',IsActive=" +dlStatus.SelectedValue + " where GId=" + getIdentifierValue + "", sqlDB.connection);
                cmd.ExecuteNonQuery();                
                return true;
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
                if (ViewState["__Dependency__"].ToString().Equals("True"))
                    loadLineOrGroupByDepartment();
                else
                loadDesignation();
                return false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (!InputValidation()) return;

                if (btnSave.Text == "Update")
                {
                    if (UpdateDesignation() == true)
                    {
                        if (ViewState["__Dependency__"].ToString().Equals("True"))
                            loadLineOrGroupByDepartment();
                        else
                        loadDesignation();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "UpdateSuccess()", true);
                        allClear();
                    }
                }
                else
                {                   
                    if (SaveDesignation() == true)
                    {
                        allClear();
                        if (ViewState["__Dependency__"].ToString().Equals("True"))
                            loadLineOrGroupByDepartment();
                        else
                        loadDesignation();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "SaveSuccess()", true);
                    }
                }
            }
            catch
            {
                loadDesignation();
            }
        }

        protected void divDesignationList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ViewState["__Dependency__"].ToString().Equals("True"))
                loadLineOrGroupByDepartment();
            else
                loadDesignation();
        }

        protected void divDesignationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (ViewState["__Dependency__"].ToString().Equals("True"))
                loadLineOrGroupByDepartment();
            else
                loadDesignation();
            divDesignationList.PageIndex = e.NewPageIndex;
            divDesignationList.DataBind();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            allClear();
            if (ViewState["__Dependency__"].ToString().Equals("True"))
                loadLineOrGroupByDepartment();
            else
            loadDesignation();
        }

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewState["__Dependency__"].ToString().Equals("True")) { 
            classes.commonTask.SearchDepartment(ddlCompanyName.SelectedValue, dlDepartment);
            }
            else
            loadDesignation();
        }

        protected void divDesignationList_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        Button lnkDelete = (Button)e.Row.FindControl("btnDelete");
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
        private bool InputValidation() 
        {
            if ( trDepartment.Visible==true && dlDepartment.SelectedValue == "0" )
            {
                lblMessage.InnerText = "warning->Please select any Departmnet!"; dlDepartment.Focus(); return false;
            }
            if (txtLine.Text == "")
            {
                lblMessage.InnerText = "warning->Please enter Line/Group !"; txtLine.Focus(); return false;
            }
            if (dlStatus.SelectedIndex==0)
            {
                lblMessage.InnerText = "warning->Please select any Status!"; dlStatus.Focus(); return false;
            }            
            return true;
        }

        protected void dlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadLineOrGroupByDepartment();
        }
        private bool deleteValidation(string GId)
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("Select EmpID from Personnel_EmpCurrentStatus where GId=" + GId + "", dt);
            if (dt.Rows.Count > 0)
                return false;
            else return true;
        }
    }
}