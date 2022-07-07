using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ComplexScriptingSystem;
using System.Drawing;
using System.Data.SqlClient;

namespace SigmaERP.payroll
{
    public partial class advancsettinge : System.Web.UI.Page
    {
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("MM-yyyy");
                ViewState["__EditSuccessStatus__"] = "True";
                ddlCompanyList.SelectedIndex = 0;
                lblTtile.Text = "Advanced List For Accept Advance Installment Of "+DateTime.Now.ToString("MM-yyyy");
                setPrivilege();
                loadExistsAdvance();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
            }
        }

        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {
                ViewState["__WriteAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin"))
                {
                    classes.commonTask.LoadBranch(ddlCompanyList);
                   
                    return;
                }
                else
                {
                    classes.commonTask.LoadBranch(ddlCompanyList, ViewState["__CompanyId__"].ToString());
                   
                    if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Admin"))
                    {

                        gvAdvaceList.Visible = false;
                    }
                    else
                    {
                        btnSet.CssClass = "";
                        btnSet.Enabled = false;
                    }

                    dtSetPrivilege = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='advancsetting.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dtSetPrivilege.Rows.Count > 0)
                    {
                        if (bool.Parse(dtSetPrivilege.Rows[0]["ReadAction"].ToString()).Equals(true))
                        {
                           
                            gvAdvaceList.Visible = true;
                        }

                        if (bool.Parse(dtSetPrivilege.Rows[0]["WriteAction"].ToString()).Equals(true))
                        {
                            btnSet.CssClass = "";
                            btnSet.Enabled = false;


                        }
                    }
                }
            }
            catch { }
        }

        private void loadExistsAdvance()
        {
            try
            {
                string[] YM = txtDate.Text.Trim().Split('-');   // YM=Year and Month
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
                sqlDB.fillDataTable("select AdvanceId,EmpName,EmpCardNo,AdvanceAmount,InstallmentNo,InstallmentAmount,Format(StartMonth,'MM-yyyy') as StartMonth,convert(varchar(11),EntryDate,105) as EntryDate ,EmpType,PaidInstallmentNo from v_Payroll_AdvanceInfo where CompanyId='" + CompanyId + "' AND PaidStatus='false' AND CONVERT(VARCHAR(7), StartMonth, 120)<='"+YM[1]+"-"+YM[0]+"' AND PaidStatus='false' ", dt = new DataTable());
                if (dt.Rows.Count > 0) gvAdvaceList.DataSource = dt;
                else gvAdvaceList.DataSource = null;
                gvAdvaceList.DataBind();
            }
            catch { }
        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
        
            saveAdvanceSetting();
        }

        private void saveAdvanceSetting()
        {
            try
            {
                bool status = false;

                if (ViewState["__EditSuccessStatus__"].ToString() == "False")
                {
                    lblMessage.InnerText = "warning->Please must be complete edit status of installment number !";
                    return;
                }
                bool YesAlterd = false;  // check condition for testing advancedSetting is altered
                DataTable dtAssignedList = new DataTable();
                if (checkedAlreadyAssigned(out dtAssignedList))
                {
                    for (int i = 0; i < dtAssignedList.Rows.Count; i++)
                    {
                        sqlDB.fillDataTable("select PaidInstallmentNo from Payroll_AdvanceInfo where AdvanceId='" + dtAssignedList.Rows[i]["AdvanceId"].ToString() + "'", dt = new DataTable());

                        int PaidInstallmentNo = int.Parse(dt.Rows[0]["PaidInstallmentNo"].ToString()) - int.Parse(dtAssignedList.Rows[i]["PaidInstallmentNo"].ToString());
                        SQLOperation.cmd = new System.Data.SqlClient.SqlCommand("update Payroll_AdvanceInfo set PaidInstallmentNo =" + PaidInstallmentNo + " where AdvanceId='" + gvAdvaceList.DataKeys[i].Value.ToString() + "'", sqlDB.connection);
                        SQLOperation.cmd.ExecuteNonQuery();
                    }
                    if (dtAssignedList.Rows.Count > 0)
                    {
                        SqlCommand cmd = new SqlCommand("delete  from Payroll_AdvanceSetting where PaidMonth='" + txtDate.Text + "'", sqlDB.connection);
                        cmd.ExecuteNonQuery();
                        YesAlterd = true;
                    }
                }

                for (int i = 0; i < gvAdvaceList.Rows.Count; i++)
                   {
                    CheckBox chk = new CheckBox();
                    
                    chk = (CheckBox)gvAdvaceList.Rows[i].Cells[9].FindControl("SelectCheckBox");
                    if (chk.Checked)
                    {
                        TextBox txtInstallmentNO = (TextBox)gvAdvaceList.Rows[i].Cells[9].FindControl("txtInstallmentNo");

                        double getInstallmentAmount = Math.Round(double.Parse(gvAdvaceList.Rows[i].Cells[5].Text) * int.Parse(txtInstallmentNO.Text.Trim()),0);
                        string[] getColumns = { "AdvanceId", "InstallmentAmount", "PaidInstallmentNo", "PaidMonth" };
                        string[] getValues = { gvAdvaceList.DataKeys[i].Value.ToString(), getInstallmentAmount.ToString(),int.Parse(txtInstallmentNO.Text.Trim()).ToString(), txtDate.Text };
                        SQLOperation.forSaveValue("Payroll_AdvanceSetting", getColumns, getValues, sqlDB.connection); status = true;

                        #region-----------for aleter operation-----------------------------------------
                        if (YesAlterd)
                        {
                            sqlDB.fillDataTable("select PaidInstallmentNo from Payroll_AdvanceInfo where AdvanceId='" + gvAdvaceList.DataKeys[i].Value.ToString() + "'", dt = new DataTable());
                            int PaidInstallmentNo = int.Parse(dt.Rows[0]["PaidInstallmentNo"].ToString()) + int.Parse(txtInstallmentNO.Text);
                            SQLOperation.cmd = new System.Data.SqlClient.SqlCommand("update Payroll_AdvanceInfo set PaidInstallmentNo =" + PaidInstallmentNo + " where AdvanceId='" + gvAdvaceList.DataKeys[i].Value.ToString() + "'", sqlDB.connection);
                            SQLOperation.cmd.ExecuteNonQuery();
                        }
                        #endregion--------------alter operation is closed------------------------------
                        #region -----------------for regular operation---------------------------------
                        else
                        {
                            SQLOperation.cmd = new System.Data.SqlClient.SqlCommand("update Payroll_AdvanceInfo set PaidInstallmentNo =" + (int.Parse(gvAdvaceList.Rows[i].Cells[9].Text) + int.Parse(txtInstallmentNO.Text.Trim())) + " where AdvanceId='" + gvAdvaceList.DataKeys[i].Value.ToString() + "'", sqlDB.connection);
                            SQLOperation.cmd.ExecuteNonQuery();
                        }
                        #endregion ----------------regular operarion is closed--------------------------

                    }

                }

                if (status)
                {
                    lblMessage.InnerText = "success-> Successfully Advance Setting Saved";
                    loadExistsAdvance();
                }
                
                
                
                
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private bool checkedAlreadyAssigned(out DataTable dtAssigneList)
        {

            sqlDB.fillDataTable("select  *  from Payroll_AdvanceSetting inner join Payroll_AdvanceInfo on Payroll_AdvanceSetting.AdvanceId=Payroll_AdvanceInfo.AdvanceId where  PaidStatus='False' and PaidMonth='" + txtDate.Text + "' and CompanyId='" + ddlCompanyList.SelectedValue + "'", dtAssigneList = new DataTable());
                if (dtAssigneList.Rows.Count > 0) return true;
                else return false;
           
        }

        private bool checkAlreadySetForThisMonth()  // test already exists ?
        {
            try
            {
                sqlDB.fillDataTable("select PaidMonth from Payroll_AdvanceSetting where PaidMonth='"+DateTime.Now.ToString("MM-yyyy")+"'",dt=new DataTable ());
                if (dt.Rows.Count > 0)
                {
                    lblMessage.InnerText = "warning->Sorry,Already set advance installment for this month !";
                    return true;
                }
                else return false;
            }
            catch { return false; }
        }
        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";

            string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
          
            loadExistsAdvance();
        }

        protected void ddlShiftList_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadExistsAdvance();
        }

        protected void gvAdvaceList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";

                

                int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                Button btnEdit = (Button)gvAdvaceList.Rows[rIndex].Cells[10].FindControl("btnEdit");
                TextBox txtInstallmentNO = (TextBox)gvAdvaceList.Rows[rIndex].Cells[9].FindControl("txtInstallmentNo");

                if (e.CommandName.Equals("Alter"))
                {

                    if (btnEdit.Text.Equals("Edit"))
                    {
                        if (ViewState["__EditSuccessStatus__"].ToString() == "False")
                        {
                            lblMessage.InnerText = "warning->Please must be complete previous installment number !";
                            return;
                        }


                        txtInstallmentNO.Enabled = true;

                        btnEdit.Text = "Ok";
                        btnEdit.ForeColor = Color.Red;
                        txtInstallmentNO.Style.Add("border-style", "solid");
                        txtInstallmentNO.Style.Add("border-color", "#0000ff");
                        ViewState["__EditSuccessStatus__"] ="False";
                    }
                    else
                    {
                        if ((int.Parse(gvAdvaceList.Rows[rIndex].Cells[9].Text) + int.Parse(txtInstallmentNO.Text.Trim())) > int.Parse(gvAdvaceList.Rows[rIndex].Cells[4].Text))
                        {
                            lblMessage.InnerText = "error->Please type right installment no.";
                            return;
                        }
                        btnEdit.Text = "Edit";
                        btnEdit.ForeColor = Color.Green;
                        txtInstallmentNO.Enabled = false;
                        txtInstallmentNO.Style.Add("border-style", "solid");
                        txtInstallmentNO.Style.Add("border-color", "gray");

                        ViewState["__EditSuccessStatus__"] = "True";
                    }
                }                                            
            }
            catch { }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            loadExistsAdvance();
            lblTtile.Text = "Advanced List For Accept Advance Installment Of " + txtDate.Text.Trim();
        }

    }
}