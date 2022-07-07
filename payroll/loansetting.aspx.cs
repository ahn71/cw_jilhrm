using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.payroll
{
    public partial class loansetting : System.Web.UI.Page
    {
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                lblTtile.Text = "Loan List For Accept Loan Installment Of " + DateTime.Now.ToString("MM-yyyy");
                loadExistsLoan();
            }
        }

        private void setPrivilege()
        {
            try
            {
                ViewState["__WriteAction__"] = "1";
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='loansetting.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false))
                        {
                            ViewState["__ReadAction__"] = "0";
                            gvLoanList.Visible = false;
                        }

                        if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                        {
                            btnSet.CssClass = "";
                            btnSet.Enabled = false;


                        }
                    }
                }
            }
            catch { }
        }

        private void loadExistsLoan()
        {
            try
            {
                sqlDB.fillDataTable("select LoanId,EmpCardNo,LoanAmount,InstallmentNo,InstallmentAmount,StartMonth,convert(varchar(11),EntryDate,105) as EntryDate ,EmpType,PaidInstallmentNo from v_Payroll_LoanInfo where PaidStatus='false'", dt = new DataTable());
                gvLoanList.DataSource = dt;
                gvLoanList.DataBind();
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
                for (int i = 0; i < gvLoanList.Rows.Count; i++)
                {
                    CheckBox chk = new CheckBox();
                    chk = (CheckBox)gvLoanList.Rows[i].Cells[9].FindControl("SelectCheckBox");
                    if (chk.Checked)
                    {
                        string[] getColumns = { "LoanId", "InstallmentAmount", "PaidInstallmentNo", "PaidMonth" };
                        string[] getValues = { gvLoanList.DataKeys[i].Value.ToString(), gvLoanList.Rows[i].Cells[4].Text, (int.Parse(gvLoanList.Rows[i].Cells[8].Text) + 1).ToString(), DateTime.Now.ToString("MM-yyyy") };
                        SQLOperation.forSaveValue("Payroll_LoanSetting", getColumns, getValues, sqlDB.connection); status = true;

                        SQLOperation.cmd = new System.Data.SqlClient.SqlCommand("update Payroll_LoanInfo set PaidInstallmentNo =" + (int.Parse(gvLoanList.Rows[i].Cells[8].Text) + 1) + " where LoanId='" + gvLoanList.DataKeys[i].Value.ToString() + "'", sqlDB.connection);
                        SQLOperation.cmd.ExecuteNonQuery();

                    }

                }

                if (status)
                {
                    lblMessage.InnerText = "success-> Successfully Loan Setting Saved";
                    loadExistsLoan();

                }




            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

    }
}