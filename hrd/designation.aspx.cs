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
    public partial class designation1 : System.Web.UI.Page
    {
        string CompanyId = "";
        string sqlcmd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                sqlDB.connectionString = Glory.getConnectionString();
                sqlDB.connectDB();
                lblMessage.InnerText = "";
                if (!IsPostBack)
                {
                    
                    setPrivilege();
                    loadDesignation();
                             
                }
            }
            catch { }
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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "designation.aspx", ddlCompanyName, divDesignationList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];               
                classes.commonTask.loadDepartmentListByCompany(dlDepartment, ViewState["__CompanyId__"].ToString());
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
            catch { }

        }
        private void loadDesignation()
        {
            try
            {
                ViewState["__preRIndex__"] = "No";
                string DptId = (dlDepartment.SelectedValue == "0") ? "" : "and DptId='" + dlDepartment.SelectedValue + "'";
                 CompanyId = (ddlCompanyName.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue;
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
                {
                   sqlcmd = "SELECT SL,CompanyId,CompanyName,DptId,DsgId, DptName, DsgName,DsgNameBn, DsgShortName, DsgStatus FROM v_HRD_Designation where CompanyId='" + CompanyId + "' " + DptId + " Order by DptId";
                }
                else
                {
                    sqlcmd = "SELECT SL,CompanyId,CompanyName,DptId,DsgId, DptName,DsgName,DsgNameBn, DsgShortName, DsgStatus FROM v_HRD_Designation where CompanyId='" + ViewState["__CompanyId__"] + "' " + DptId + " Order by DptId";
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
                    btnSave.Text = "Update";
                }
                else if(e.CommandName.Equals("Delete"))
                {
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    if(deleteValidation(divDesignationList.DataKeys[rIndex].Values[3].ToString()))
                    {
                    SQLOperation.forDeleteRecordByIdentifier("HRD_Designation", "SL", divDesignationList.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "deleteSuccess()", true);
                    allClear();
                    lblMessage.InnerText = "success->Successfully Designation Deleted";
                    }
                    else
                        lblMessage.InnerText = "error->Warning! Can't delete this Designation. It is used for emplloyes.";

                }
            }
            catch { }
        }
        private void setValueToControl(int rIndex, string getSL, string getCompanyId, string getDptId)
        {
            try
            {
                ViewState["__getSL__"] = getSL;
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
                {
                    ddlCompanyName.SelectedValue = getCompanyId;
                    classes.commonTask.SearchDepartment(ddlCompanyName.SelectedValue, dlDepartment);
                }
                else
                {
                    classes.commonTask.SearchDepartment(ViewState["__CompanyId__"].ToString(),dlDepartment);
                }
                dlDepartment.SelectedValue = getDptId;
                txtDesignation.Text = divDesignationList.Rows[rIndex].Cells[3].Text;
                if (divDesignationList.Rows[rIndex].Cells[4].Text == "&nbsp;")
                    txtDesignationBn.Text = "";
                else txtDesignationBn.Text = divDesignationList.Rows[rIndex].Cells[4].Text;
                if (divDesignationList.Rows[rIndex].Cells[5].Text == "&nbsp;")
                    txtDesignationShortName.Text = "";
                else txtDesignationShortName.Text = divDesignationList.Rows[rIndex].Cells[5].Text;
                if (divDesignationList.Rows[rIndex].Cells[6].Text == "True")
                {
                    dlStatus.Text = "Active";
                }
                else
                {
                    dlStatus.Text = "InActive";
                }
            }
            catch { }
        }
        private void allClear()
        {
            txtDesignation.Text = "";
            txtDesignationBn.Text = "";
            txtDesignationShortName.Text = "";
            txtDesignationCode.Text = "";
            dlStatus.Text = "-select-";
            hdnbtnStage.Value = "";
            hdnUpdate.Value = "";
            //dlDepartment.Items.Clear();
            btnSave.Text = "Save";
        }

        private Boolean SaveDesignation()
        {
            try
            {
                int st;
                if (dlStatus.Text.Equals("Active")) st = 1;
                else st = 0;
                SqlCommand cmd = new SqlCommand("insert into HRD_Designation (DsgId,DptId,DsgName,DsgNameBn,DsgShortName,DsgStatus) values( '" + classes.commonTask.LoadSL("Select Max(SL) as SL From HRD_Designation", "Designation") + "','" + dlDepartment.SelectedValue + "' ,'" + txtDesignation.Text + "',N'" + txtDesignationBn.Text + "','" + txtDesignationShortName.Text + "'," + st.ToString() + ") ", sqlDB.connection);
                cmd.ExecuteNonQuery();
                loadDesignation();
                return true;
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
                loadDesignation();
                return false;
            }
        }

        private Boolean UpdateDesignation()
        {
            try
            {
                int st;
                if (dlStatus.Text.Equals("Active")) st = 1;
                else st = 0;
                string getIdentifierValue = ViewState["__getSL__"].ToString();
                SqlCommand cmd = new SqlCommand("Update HRD_Designation set DptId='" + dlDepartment.SelectedValue + "', DsgName='" + txtDesignation.Text + "', DsgNameBn=N'" + txtDesignationBn.Text + "', DsgShortName='" + txtDesignationShortName.Text.Trim() + "', DsgStatus=" + st.ToString() + " where SL=" + getIdentifierValue + "", sqlDB.connection);
                cmd.ExecuteNonQuery();
               // loadDesignation();
                return true;
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = ex.Message;
                loadDesignation();
                return false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dlDepartment.SelectedValue == "0") 
                {
                    lblMessage.InnerText = "warning->Please Select Any Department !"; dlDepartment.Focus(); return;
                }
                if (txtDesignation.Text == "")
                {
                    lblMessage.InnerText = "warning->Please Enter Designation !";
                    txtDesignation.Focus();
                  //  loadDesignation();
                    return;
                }
                if (dlStatus.SelectedValue == "-select-")
                {
                    lblMessage.InnerText = "warning->Please Select Status !"; dlStatus.Focus();
                   // loadDesignation();
                    return;
                }

                if (btnSave.Text == "Update")
                {
                    if (UpdateDesignation() == true)
                    {
                        loadDesignation();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "UpdateSuccess()", true);
                        allClear();
                        //dlDepartment.SelectedValue="0";
                    }
                }
                else
                {
                    
                    if (SaveDesignation() == true)
                    {
                        allClear();
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
            loadDesignation();
            divDesignationList.DataBind();
        }

        protected void divDesignationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            loadDesignation();
            divDesignationList.PageIndex = e.NewPageIndex;
            divDesignationList.DataBind();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            allClear();
            loadDesignation();
        }

        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.commonTask.SearchDepartment(ddlCompanyName.SelectedValue,dlDepartment);
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
                        LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
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
                        LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkAlter");
                        lnkDelete.Enabled = false;
                        lnkDelete.ForeColor = Color.Silver;
                    }

                }
                catch { }
            }
        }

        protected void dlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadDesignation();
        }

        private bool deleteValidation(string DsgId)
        {
            DataTable dt = new DataTable();
            sqlDB.fillDataTable("Select EmpID from Personnel_EmpCurrentStatus where DsgId=" + DsgId + "", dt);
            if (dt.Rows.Count > 0)
                return false;
            else return true;
        }
     
    }
}