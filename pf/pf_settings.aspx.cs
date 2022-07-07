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

namespace SigmaERP.pf
{
    public partial class pf_settings : System.Web.UI.Page
    {
        string CompanyId = "";
        string sqlcmd = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();

            }

        }
        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                ViewState["__preRIndex__"] = "No";
                string getUserId = getCookies["__getUserId__"].ToString();

                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "pf_settings.aspx", ddlCompanyName, gvPFSettings, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
                loadPFSettings();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyName.Enabled = false;
                ddlCompanyName.SelectedValue = ViewState["__CompanyId__"].ToString();
                
            }


            catch { }

        }
        private void loadPFSettings()
        {
            try
            {
                CompanyId = (ddlCompanyName.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompanyName.SelectedValue;

                sqlcmd = "select * from v_PF_CalculationSetting where CompanyId='" + CompanyId + "' order by convert(int,CompanyId)";

                DataTable dt = new DataTable();
                sqlDB.fillDataTable(sqlcmd, dt);
                gvPFSettings.DataSource = dt;
                gvPFSettings.DataBind();
               
            }
            catch { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "Save")
                SavePFSettings();
            else
                UpdatePFSettings();
        }
        private void SavePFSettings()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("insert into PF_CalculationSetting values('" + ddlCompanyName.SelectedValue + "'," + txtEmpContribution.Text.Trim() + ","+
                    "'" + txtEmprContribution.Text.Trim() + "','0','" +txtPFStartYear.Text.Trim() +"',"+
                    "" +txtEmpPartStartYear.Text.Trim() +","+txtEmpPartEndYear.Text.Trim()+","+txtEmpEmprStartYear.Text.Trim()+","+txtEmpEmprEndYear.Text.Trim()+","+
                    ""+txtEmpEmprIntrStartYear.Text.Trim()+","+txtEmpEmprIntrEndYear.Text.Trim()+") ", sqlDB.connection);
                SQLOperation.forDeleteRecordByIdentifier("PF_CalculationSetting", "CompanyId", ddlCompanyName.SelectedValue, sqlDB.connection);  
                if (int.Parse(cmd.ExecuteNonQuery().ToString())==1)
                {
                    loadPFSettings();
                    lblMessage.InnerText="success-> Successfully Saved.";
                    allClear();
                }   
                else
                    lblMessage.InnerText = "error-> Unable to Save !";
            }
            catch (Exception ex)
            {
                lblMessage.InnerText ="error>"+ ex.Message;              
            }
        }

        private void UpdatePFSettings()
        {
            try
            {

                SqlCommand cmd = new SqlCommand("Update  PF_CalculationSetting set EmpContribution=" + txtEmpContribution.Text.Trim() + ",EmprContribution=" +
                    "'" + txtEmprContribution.Text.Trim() + "',RateofInterest='0',PFStartYear='" + txtPFStartYear.Text.Trim() + "',PEmpPartStartyear=" +
                    "" + txtEmpPartStartYear.Text.Trim() + ",PEmpPartEndyear=" + txtEmpPartEndYear.Text.Trim() + ",PEmpEmprStartyear=" + txtEmpEmprStartYear.Text.Trim() + ",PEmpEmprEndyear=" + txtEmpEmprEndYear.Text.Trim() + ",PEmpEmprIrstStartyear=" +
                    "" + txtEmpEmprIntrStartYear.Text.Trim() + ",PEmpEmprIrstEndyear=" + txtEmpEmprIntrEndYear.Text.Trim() + " where CompanyId='"+ddlCompanyName.SelectedValue+"'", sqlDB.connection);
                if (int.Parse(cmd.ExecuteNonQuery().ToString()) == 1)
                {
                    loadPFSettings();
                    lblMessage.InnerText = "success-> Successfully Update.";
                    allClear();
                }
                else
                    lblMessage.InnerText = "error-> Unable to Update !";

            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error>" + ex.Message;

            }
        }

        protected void gvPFSettings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName.Equals("Alter"))
            {
                string a = ViewState["__preRIndex__"].ToString();
                if (!ViewState["__preRIndex__"].ToString().Equals("No")) gvPFSettings.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                

                gvPFSettings.Rows[rIndex].BackColor = System.Drawing.Color.Yellow;
                ViewState["__preRIndex__"] = rIndex;

                ddlCompanyName.SelectedValue = gvPFSettings.DataKeys[rIndex].Values[0].ToString();

                txtEmpContribution.Text = gvPFSettings.Rows[rIndex].Cells[0].Text.Trim();
                txtEmprContribution.Text = gvPFSettings.Rows[rIndex].Cells[1].Text.Trim();
            //  txtRateOfInterest.Text = gvPFSettings.Rows[rIndex].Cells[2].Text.Trim();
                txtPFStartYear.Text = gvPFSettings.Rows[rIndex].Cells[2].Text.Trim();
                txtEmpPartStartYear.Text = gvPFSettings.Rows[rIndex].Cells[3].Text.Trim();
                txtEmpPartEndYear.Text = gvPFSettings.Rows[rIndex].Cells[4].Text.Trim();
                txtEmpEmprStartYear.Text = gvPFSettings.Rows[rIndex].Cells[5].Text.Trim();
                txtEmpEmprEndYear.Text = gvPFSettings.Rows[rIndex].Cells[6].Text.Trim();
                txtEmpEmprIntrStartYear.Text = gvPFSettings.Rows[rIndex].Cells[7].Text.Trim();
                txtEmpEmprIntrEndYear.Text = gvPFSettings.Rows[rIndex].Cells[8].Text.Trim();
                btnSave.Text = "Update";
                if (ViewState["__UpdateAction__"].Equals("0"))
                {
                    btnSave.Enabled = false;
                    btnSave.CssClass = "";
                }
                else
                {
                    btnSave.Enabled = true;
                    btnSave.CssClass = "Pbutton";
                }             
            }
            else if (e.CommandName.Equals("deleterow"))
            {
                SQLOperation.forDeleteRecordByIdentifier("PF_CalculationSetting", "CompanyId", gvPFSettings.DataKeys[rIndex].Values[0].ToString(), sqlDB.connection);               
                allClear();
                lblMessage.InnerText = "success->Successfully  Deleted";
                gvPFSettings.Rows[rIndex].Visible = false;
            }
        }
        private void allClear()
        {
            
            txtEmpContribution.Text = "";
            txtEmprContribution.Text = "";
         // txtRateOfInterest.Text = "";
            txtPFStartYear.Text = "";
            txtEmpPartStartYear.Text = "";
            txtEmpPartEndYear.Text = "";
            txtEmpEmprStartYear.Text = "";
            txtEmpEmprEndYear.Text = "";
            txtEmpEmprIntrStartYear.Text = "";
            txtEmpEmprIntrEndYear.Text = "";
            if (ViewState["__WriteAction__"].Equals("0"))
            {
                btnSave.Enabled = false;
                btnSave.CssClass = "";
            }
            else
            {
                btnSave.Enabled = true;
                btnSave.CssClass = "Pbutton";
            }
            btnSave.Text = "Save";
        }
        protected void ddlCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadPFSettings();
        }

        protected void gvPFSettings_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void btnNew_Click(object sender, EventArgs e)
        {
            allClear();
        }



    }
}