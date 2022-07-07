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
using SigmaERP.classes;
using System.Drawing;

namespace SigmaERP.personnel
{
    public partial class LeaveConfig : System.Web.UI.Page
    {
        string strSQL = "";
       
        SqlCommand cmd;
        static DataTable dtSetPrivilege;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {

                setPrivilege();
                commonTask.loadLeaveType(ddlLeaveTypes);
                LoadGrid();
               // loadPunismentInfo();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
            
        }

        private void setPrivilege()
        {
            try
            {


                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "LeaveConfig.aspx", ddlCompanyList, gvLeaveConfig, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
                if (ViewState["__ReadAction__"].ToString().Equals("0")) 
                {
                    btnPreview.Enabled = false;
                    btnPreview.CssClass = "";
                }

                
            }
            catch { }

        }
        
        

        void LoadGrid()
        {
            string Vt = ViewState["__CompanyId__"].ToString();
            string CompanyId=(ddlCompanyList.SelectedValue.ToString().Equals("0000"))?ViewState["__CompanyId__"].ToString():ddlCompanyList.SelectedValue.ToString();
            string strSQL = "select * from tblLeaveConfig where CompanyId='" +CompanyId+ "'";
            DataTable DTLocal = new DataTable();
            sqlDB.fillDataTable(strSQL, DTLocal);

            gvLeaveConfig.DataSource = DTLocal;
            gvLeaveConfig.DataBind();
        }

        private void saveLeaveConfig()
        {
            try
            {
                byte isChecked = (IsDeductionAllowed.Checked) ? (byte)1 :(byte)0;
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

                string[] getColumns = { "LeaveName", "ShortName", "LeaveDays", "LeaveNature", "IsDeductionAllowed", "CompanyId"};
                string[] getValues = { ddlLeaveTypes.SelectedItem.ToString(), ddlLeaveTypes.SelectedValue.ToString(), txtLeaveDays.Text.Trim(), txtLeaveNature.Text.Trim(), isChecked.ToString(), CompanyId};
              
                if (SQLOperation.forSaveValue("tblLeaveConfig", getColumns, getValues,sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success->Successfully Leaveconfig saved";
                }
            }
            catch (Exception ex)
            {
            //    MessageBox.Show(ex.Message);
            }
        }


        private void updateLeaveConfig()
        {
            try
            {
                byte isChecked = (IsDeductionAllowed.Checked) ? (byte)1 : (byte)0;
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

                string[] getColumns = { "LeaveName", "ShortName", "LeaveDays", "LeaveNature", "IsDeductionAllowed", "CompanyId" };
                string[] getValues = { ddlLeaveTypes.SelectedItem.ToString(), ddlLeaveTypes.SelectedValue.ToString(), txtLeaveDays.Text.Trim(), txtLeaveNature.Text.Trim(), isChecked.ToString(),CompanyId};
                if (SQLOperation.forUpdateValue("tblLeaveConfig", getColumns, getValues, "LeaveId", hdLeaveId.Value.ToString(),sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success->Successfully updated";
                }
            }
            catch (Exception ex)
            {
                
            }
        }


        void Clear()
        {
            lblMessage.InnerText = "";
            if (ViewState["__WriteAction__"].ToString() == "0")
            {
                btnSave.Enabled = false;
                btnSave.CssClass = "";
            }
            ddlLeaveTypes.SelectedIndex = 0;

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
            
           
            txtLeaveNature.Text = "";
           
            txtLeaveDays.Text = "";
            IsDeductionAllowed.Checked = false;
           // ddlCompanyList.SelectedIndex = 0;

        }

       

        void Delete(int id)
        {
            string strSql = "delete from [dbo].[tblLeaveConfig] where [LeaveId]='" + id + "'";
            cmd = new SqlCommand(strSql,sqlDB.connection);
            cmd.ExecuteNonQuery();
            lblMessage.InnerText = "success->Successfully Deleted";
            hdLeaveId.Value = "";
            btnSave.Text = "Save";

        }

        private void SetValueToControl(string leaveid,string CompanyId)
        {
            string strSQL = @"select LeaveId, [LeaveName],ShortName, [LeaveDays], [LeaveNature], IsDeductionAllowed   from [tblLeaveConfig]
                                where LeaveId='" + leaveid + "'";
            DataTable DTLocal = new DataTable();
            sqlDB.fillDataTable(strSQL, DTLocal);


            hdLeaveId.Value = DTLocal.Rows[0]["LeaveId"].ToString();
            
            txtLeaveDays.Text = DTLocal.Rows[0]["LeaveDays"].ToString();
            txtLeaveNature.Text = DTLocal.Rows[0]["LeaveNature"].ToString();
            IsDeductionAllowed.Checked = (bool.Parse(DTLocal.Rows[0]["IsDeductionAllowed"].ToString()).Equals(true)) ? true : false;
            for (byte b = 0; b < ddlLeaveTypes.Items.Count;b++ )
            {
                if (ddlLeaveTypes.Items[b].Value.ToString().Equals(DTLocal.Rows[0]["ShortName"].ToString()))
                {
                    ddlLeaveTypes.SelectedIndex = b; break;
                }
            }

            ddlCompanyList.SelectedValue = CompanyId;
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
            btnSave.Text = "Update";
        }

        protected void gvLfeaveConfig_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    if (ViewState["__UpdateAction__"].ToString() == "1")
                    {
                        btnSave.CssClass = "Lbutton";
                        btnSave.Enabled = true;
                    }

                    string leaveid = e.CommandArgument.ToString();

                    //SetValueToControl(leaveid);


                }
                else if (e.CommandName == "Delete")
                {
                    int id = Convert.ToInt16(e.CommandArgument.ToString());
                    Delete(id);
                   
                    Clear();
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = ex.ToString();
            }
        }

        protected void gvLfeaveConfig_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

       

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text.Trim().Equals("Save")) saveLeaveConfig();
            else updateLeaveConfig();
            LoadGrid();
            Clear();
           
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void gvLfeaveConfig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //try
            //{
            //    if (ViewState["__DeletAction__"].ToString().Equals("0"))
            //    {
            //        LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
            //        lnkDelete.Enabled = false;
            //        lnkDelete.OnClientClick = "return false";
            //    }

            //    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
            //    {
            //        LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
            //        lnkEdit.Enabled = false;
            //        lnkEdit.OnClientClick = "return false";
            //    }
            //}
            //catch { }
        }

        protected void gvLfeaveConfig_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gvLeaveConfig_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                if (e.CommandName == "Alter")
                {
                   


                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    string leaveid = gvLeaveConfig.DataKeys[rIndex].Values[0].ToString();
                    string CompanyId = gvLeaveConfig.DataKeys[rIndex].Values[1].ToString();
                    SetValueToControl(leaveid, CompanyId);

                }
                else if (e.CommandName == "Delete")
                {
                    int id = Convert.ToInt16(e.CommandArgument.ToString());
                    Delete(id);
                    LoadGrid();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = ex.ToString();
            }
        }

        protected void gvLeaveConfig_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            LoadGrid();
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
        
        protected void gvLeaveConfig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //try
            //{
            //    if (bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false))
            //    {
            //        Button lnkDelete = (Button)e.Row.FindControl("btnDelete");
            //        lnkDelete.Enabled = false;
            //        lnkDelete.OnClientClick = "return false";

            //        Button btn = (Button)e.Row.FindControl("btnAlter"); // 1 is Bind button index
            //        btn.Enabled = false; //
            //    }

            //}
            //catch { }

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
        private void priviewCode() 
        {
            string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
            string getSQLCMD;
            DataTable dt = new DataTable();
            getSQLCMD = " SELECT v_tblLeaveConfig.LeaveName, v_tblLeaveConfig.ShortName, v_tblLeaveConfig.LeaveDays, v_tblLeaveConfig.LeaveNature, v_tblLeaveConfig.CompanyName, v_tblLeaveConfig.Address"
                +" FROM "
                +" dbo.v_tblLeaveConfig "
                + " where CompanyId ='" + CompanyId + "' "
                +" ORDER BY v_tblLeaveConfig.CompanyName";
            sqlDB.fillDataTable(getSQLCMD, dt);
            if (dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning->Sorry any payslip are not founded"; return;
            }
            Session["__LeaveConfig__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LeaveConfig');", true);  //Open New Tab for Sever side code
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            priviewCode();
        }

       

       
    }
}