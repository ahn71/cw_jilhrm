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

namespace SigmaERP.personnel
{
    public partial class holyday : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                
                ViewState["__preRIndex__"] = "No";
                setPrivilege();
                LoadHolidaysByCompany();
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
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString().ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();

                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "holyday.aspx", ddlCompanyList, gvHoliday, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];

            }
            catch { }

        }
        

        private void LoadHolidaysByCompany()
        {
            string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
            string strSQL = @"select [HCode], CONVERT(varchar(10), [HDate], 105) as [HDate], [Description] from tblHolydayWork where CompanyId='"+CompanyId+"' order by year(HDate) desc, MONTH(HDate) desc, HDate desc";
            DataTable DTLocal = new DataTable();
            sqlDB.fillDataTable(strSQL,DTLocal);
            gvHoliday.DataSource = DTLocal;
            gvHoliday.DataBind();
            
        }
        private void saveHolidayWork()
        {
            try
            {
                string[] d = txtDate.Text.Trim().Split('-');
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                string[] getColumns = { "CompanyId", "HDate", "Description" };
                string[] getValues = { CompanyId, d[2]+"-"+d[1]+"-"+d[0], txtDescription.Text.Trim() };
                if (SQLOperation.forSaveValue("tblHolydayWork", getColumns, getValues,sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success->Successfully saved";
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" +ex.Message;
            }
        }

        private void updateHolidayWork(string HCode)
        {
            try
            {
                string[] d = txtDate.Text.Trim().Split('-');
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                string[] getColumns = { "CompanyId", "HDate", "Description" };
                string[] getValues = { CompanyId, d[2] + "-" + d[1] + "-" + d[0], txtDescription.Text.Trim() };
                if (SQLOperation.forUpdateValue("tblHolydayWork", getColumns, getValues, "HCode", HCode, sqlDB.connection) == true)
                {
                    lblMessage.InnerText = "success->Successfully updated";
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        void Clear()
        {
            try
            {
                lblMessage.InnerText = "";
                if (ViewState["__WriteAction__"].ToString() == "0")
                {
                    btnSave.Enabled = false;
                    btnSave.CssClass = "";
                }
                txtDate.Text = "";
                txtDescription.Text = "";
                btnSave.Text = "Save";
                gvHoliday.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            }
            catch { }
        }

        protected void gvHoliday_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Alter")
                {

                    if (!ViewState["__preRIndex__"].ToString().Equals("No")) gvHoliday.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                   
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());

                    gvHoliday.Rows[rIndex].BackColor = System.Drawing.Color.Yellow;
                    ViewState["__preRIndex__"] = rIndex;

                    if (ViewState["__UpdateAction__"].ToString() == "1")
                    {
                        btnSave.CssClass = "Lbutton";
                        btnSave.Enabled = true;
                    }
                    ViewState["__HCode__"] = gvHoliday.DataKeys[rIndex].Value.ToString();

                    SetValueToControl(ViewState["__HCode__"].ToString());

                }
                else if (e.CommandName == "Delete")
                {
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    Delete(gvHoliday.DataKeys[rIndex].Value.ToString());
                    LoadHolidaysByCompany();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = ex.ToString();
            }
        }
        void Delete(string id)
        {
           // string strSql = "delete from [dbo].[tblHolydayWork] where [HCode]=" + id + "";
            SqlCommand cmd = new SqlCommand("delete from [dbo].[tblHolydayWork] where [HCode]=" + id + "", sqlDB.connection);
            cmd.ExecuteNonQuery();
           
            btnSave.Text = "Save";

        }

        private void SetValueToControl(string hid)
        {
            string strSQL = @"select HCode, CONVERT(varchar(10), [HDate],105) as HDate, [Description]  from [tblHolydayWork]
                                where HCode='" + hid + "'";
            DataTable DTLocal = new DataTable();

            sqlDB.fillDataTable(strSQL, DTLocal);
            

            txtDate.Text = DTLocal.Rows[0]["HDate"].ToString();
            txtDescription.Text = DTLocal.Rows[0]["Description"].ToString();
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

        protected void gvHoliday_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
        }

       

        protected void gvHoliday_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                LoadHolidaysByCompany();
                gvHoliday.PageIndex = e.NewPageIndex;
                gvHoliday.DataBind();
            }
            catch { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text.Trim().Equals("Save")) saveHolidayWork();
            else updateHolidayWork(ViewState["__HCode__"].ToString());
            Clear();
            LoadHolidaysByCompany();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

   
      

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadHolidaysByCompany();
            }
            catch { }
        }

        protected void gvHoliday_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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
      

       
    }
    
}