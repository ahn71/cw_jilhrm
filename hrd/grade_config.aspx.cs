using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ComplexScriptingSystem;
using System.Drawing;
using SigmaERP.classes;

namespace SigmaERP.hrd
{
    public partial class grade_config : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            {
                if (!IsPostBack)
                {
                    setPrivilege();
                    loadGrade("");
                    
                }
            }
        }
        private void setPrivilege()
        {
            try
            {
            
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "grade_config.aspx", gvGradeList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

            }
            catch { }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text=="Update")
            {
                if (UpdateGradeConfig() == true)
                {
                    loadGrade("");
                    lblMessage.InnerText = "success->Successfully Updated";
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "UpdateSuccess()", true);
                    allClear();
                }
            }
            else
            {
                if (SaveGradeConfig() == true)
                {
                    loadGrade("");
                    allClear();
                    lblMessage.InnerText = "success->Successfully Saved";
                }
            }
        }
        private Boolean SaveGradeConfig()
        {
            try
            {
                int st;
                if (dlStatus.Text.Equals("Active")) st = 1;
                else st = 0;
                SqlCommand cmd = new SqlCommand("Insert into HRD_Grade values('" + classes.commonTask.LoadSL("Select Max(SL)as SL From HRD_Grade", "Grade") + "','" + txtGradeName.Text + "',N'" + txtGradeBangla.Text + "'," + st.ToString() + ")", sqlDB.connection);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch { return false; }
        }
        private void allClear()
        {
            txtGradeBangla.Text = "";
            txtGradeName.Text = "";
            dlStatus.Text = "-select-";
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
            btnSave.Text = "Save";
           
        }
        private Boolean UpdateGradeConfig()
        {
            try
            {
                 int st;
                if (dlStatus.Text.Equals("Active")) st = 1;
                else st = 0;
                SqlCommand cmd = new SqlCommand("Update HRD_Grade set GrdName='" + txtGradeName.Text + "',GrdNameBangla=N'" + txtGradeBangla.Text + "',GrdStatus=" + st.ToString() + " where SL=" + ViewState["__SL__"].ToString() + "", sqlDB.connection);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch { return false; }
        }
        private void loadGrade(string sqlcmd)
        {
            if (string.IsNullOrEmpty(sqlcmd)) sqlcmd = "select SL,GrdId,GrdName,GrdNameBangla, case GrdStatus when 0  then 'InActive' else 'Active' end as GrdStatus from HRD_Grade";
            DataTable dt = new DataTable();
            sqlDB.fillDataTable(sqlcmd, dt);
            gvGradeList.DataSource = dt;
            gvGradeList.DataBind();            
        }

      

        protected void btnNew_Click(object sender, EventArgs e)
        {

            allClear();
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "clear()", true);
        }
   

        protected void gvGradeList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
            ViewState["__SL__"] = gvGradeList.DataKeys[rIndex].Values[0].ToString();
            if (e.CommandName == "Alter")
            {
                txtGradeName.Text = gvGradeList.Rows[rIndex].Cells[0].Text.ToString();
                txtGradeBangla.Text = gvGradeList.Rows[rIndex].Cells[1].Text.ToString();
                //if (gvGradeList.Rows[rIndex].Cells[2].Text == "True")
                //{
                //    dlStatus.Text = "Active";
                //}
                //else
                //{
                //    dlStatus.Text = "InActive";
                //}
                dlStatus.Text = gvGradeList.Rows[rIndex].Cells[2].Text.Trim();
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
            else if (e.CommandName == "Delete")
            {
                if (SQLOperation.forDeleteRecordByIdentifier("HRD_Grade", "SL", ViewState["__SL__"].ToString(), sqlDB.connection))
                {
                    lblMessage.InnerText = "success->Successfully Deleted";
                    allClear();
                   // clear();
                }
            }
        }

        protected void gvGradeList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            loadGrade("");
        }

        protected void gvGradeList_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gvGradeList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            loadGrade("");
            gvGradeList.PageIndex = e.NewPageIndex;
            gvGradeList.DataBind();


        }
    }
}