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
    public partial class pf_FDR : System.Web.UI.Page
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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "pf_FDR.aspx", ddlCompanyName, gvPFSettings, btnSave);

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

                sqlcmd = "select FdrID,CompanyID,FdrNo,FdrAmount,InterestRate,InterestAmount,CONVERT(VARCHAR(10),FromDate, 105) as FromDate,"+
                    "convert(varchar(10),ToDate,105) as ToDate,Period,Bank,Branch,FdrAmount+InterestAmount as TotalWithInterest from PF_FDR where CompanyId='" + CompanyId + "' order by convert(int,CompanyId)";

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
                SqlCommand cmd = new SqlCommand("insert into PF_FDR values('" + ddlCompanyName.SelectedValue + "','" + txtFDRNo.Text.Trim() + "','"  + 
                    txtFDRAmount.Text.Trim() + "','" + txtInterestRate.Text.Trim() + "'," + txtInterestAmount.Text.Trim() + ",'"
                    + convertDateTime.getCertainCulture(txtFromDate.Text).ToString() + "','" + convertDateTime.getCertainCulture(txtToDate.Text).ToString() + 
                    "',"+txtPeriod.Text.Trim() + ",'" + txtBank.Text.Trim() + "','" + txtBranch.Text + "') ", sqlDB.connection);
              //  SQLOperation.forDeleteRecordByIdentifier("PF_CalculationSetting", "CompanyId", ddlCompanyName.SelectedValue, sqlDB.connection);
                if (int.Parse(cmd.ExecuteNonQuery().ToString()) == 1)
                {
                    loadPFSettings();
                    lblMessage.InnerText = "success-> Successfully Saved.";
                    allClear();
                }
                else
                    lblMessage.InnerText = "error-> Unable to Save !";
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error>" + ex.Message;
            }
        }

        private void UpdatePFSettings()
        {
            try
            {

                SqlCommand cmd = new SqlCommand("Update  PF_FDR set FdrNo='" + txtFDRNo.Text.Trim() + "',FdrAmount='" +txtFDRAmount.Text.Trim()+
                    "',InterestRate='" + txtInterestRate.Text.Trim() + "',InterestAmount='" + txtInterestAmount.Text.Trim() + "',FromDate='" + convertDateTime.getCertainCulture(txtFromDate.Text).ToString() +
                    "',ToDate='" + convertDateTime.getCertainCulture(txtToDate.Text).ToString() + "',Period=" + txtPeriod.Text.Trim() + ",Bank='" + txtBank.Text.Trim() + "',Branch='" + txtBranch.Text.Trim() +
                    "' where FdrID='" + ViewState["__FdrID__"].ToString() + "'", sqlDB.connection);
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
                ViewState["__FdrID__"] = gvPFSettings.DataKeys[rIndex].Values[1].ToString();
                txtFDRNo.Text = gvPFSettings.Rows[rIndex].Cells[0].Text.Trim();
                txtFDRAmount.Text = gvPFSettings.Rows[rIndex].Cells[1].Text.Trim();
                //  txtRateOfInterest.Text = gvPFSettings.Rows[rIndex].Cells[2].Text.Trim();
                txtInterestRate.Text = gvPFSettings.Rows[rIndex].Cells[2].Text.Trim();              
                txtFromDate.Text = gvPFSettings.Rows[rIndex].Cells[3].Text.Trim();
                txtToDate.Text = gvPFSettings.Rows[rIndex].Cells[4].Text.Trim();
                txtPeriod.Text = gvPFSettings.Rows[rIndex].Cells[5].Text.Trim();
                txtInterestAmount.Text = gvPFSettings.Rows[rIndex].Cells[6].Text.Trim();
                txtTotalWithInterest.Text = gvPFSettings.Rows[rIndex].Cells[7].Text.Trim();
                txtBank.Text = (gvPFSettings.Rows[rIndex].Cells[8].Text.Trim().Equals("&nbsp;")) ? "" : gvPFSettings.Rows[rIndex].Cells[8].Text.Trim();
                txtBranch.Text = (gvPFSettings.Rows[rIndex].Cells[9].Text.Trim().Equals("&nbsp;")) ? "" : gvPFSettings.Rows[rIndex].Cells[9].Text.Trim();
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
                SQLOperation.forDeleteRecordByIdentifier("PF_FDR", "FdrID", gvPFSettings.DataKeys[rIndex].Values[1].ToString(), sqlDB.connection);
                allClear();
                lblMessage.InnerText = "success->Successfully  Deleted";
                gvPFSettings.Rows[rIndex].Visible = false;
            }
        }
        private void allClear()
        {

            txtFDRNo.Text = "";
            txtFDRAmount.Text = "";
            // txtRateOfInterest.Text = "";
            txtInterestRate.Text = "";
            txtInterestAmount.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtPeriod.Text = "";
            txtBank.Text = "";
            txtBranch.Text = "";
            txtTotalWithInterest.Text = "";
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
         private void PeriodDays() 
        {
            if (txtFromDate.Text.Trim().Length == 10 && txtToDate.Text.Trim().Length == 10)
             {
                 DateTime FDate = DateTime.Parse(txtFromDate.Text.Trim());
                 DateTime TDate = DateTime.Parse(txtToDate.Text.Trim());
                 txtPeriod.Text = (TDate - FDate).Days.ToString();
             }
            
        }

         private void Interest()
         {
             if (txtFDRAmount.Text.Trim().Length>0 && txtInterestRate.Text.Trim().Length>0 && txtFromDate.Text.Trim().Length == 10 && txtToDate.Text.Trim().Length == 10)
             {
                 float P = float.Parse(txtFDRAmount.Text.Trim());
                 float r = float.Parse(txtInterestRate.Text.Trim())/100;
                 float t = int.Parse(txtPeriod.Text.Trim())/365;
                // float A = P*(1 + (r*t));
                 float A = (r * P) * t;
                 txtInterestAmount.Text = A.ToString();
                 txtTotalWithInterest.Text = (P + A).ToString();
             }

         }
         protected void txtFromDate_TextChanged(object sender, EventArgs e)
         {
             try 
             { 
                 PeriodDays();
                 Interest();
             }
             catch { }
             
         }

         protected void txtFDRAmount_TextChanged(object sender, EventArgs e)
         {
             try
             {                 
                 Interest();
             }
             catch { }
         }


    }
}