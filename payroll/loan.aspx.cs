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

namespace SigmaERP.payroll
{
    public partial class loan : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setDefaultColulmns();
                classes.commonTask.loadEmpTypeInRadioButtonList(rbEmpType);
                loadExistsLoan();
                ViewState["LoanId"] = "0";
                ViewState["__EmpId__"] = "0";
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
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='bdt_note_set.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false))
                        {
                            ViewState["__ReadAction__"] = "0";
                            gvLoanInfo.Visible = false;
                        }

                        if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                        {
                            btnSave.CssClass = "";
                            btnSave.Enabled = false;
                            

                        }
                    }
                }
            }
            catch { }
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            double getInstallmentMonthANDPayment = Math.Round((double.Parse(txtAdvanceAmount.Text.Trim())) / int.Parse(txtNoOfInstallment.Text.Trim()), 0);
            byte getMonth = byte.Parse(txtStartMonth.Text.Substring(0, 2));
            dt = new DataTable();
            dt.Columns.Add("Month", typeof(string));
            dt.Columns.Add("Amount", typeof(string));
            int getYear = int.Parse(txtStartMonth.Text.Substring(3, 4));
            bool status = false;
            for (byte b = 0; b < byte.Parse(txtNoOfInstallment.Text.Trim()); b++)
            {
                if ((getMonth + b) == 13)
                {
                    getYear = getYear + 1;
                    getMonth = 0;
                    status = true;
                }

                if (status) getMonth += 1;
                else getMonth += b;
                dt.Rows.Add(getMonth.ToString() + "-" + getYear.ToString(), getInstallmentMonthANDPayment.ToString());
            }

            gvInstallmentDetails.DataSource = dt;
            gvInstallmentDetails.DataBind();
        }


        private void setDefaultColulmns()
        {
            try
            {
                dt = new DataTable();
                dt.Columns.Add("Month", typeof(string));
                dt.Columns.Add("Amount", typeof(string));
                dt.Rows.Add(" ", " ");
                gvInstallmentDetails.DataSource = dt;
                gvInstallmentDetails.DataBind();
                gvInstallmentDetails.Columns[0].HeaderStyle.Width = 200;
                gvInstallmentDetails.Columns[0].ItemStyle.Width = 200;
                gvInstallmentDetails.Columns[1].HeaderStyle.Width = 120;
            }
            catch { }
        }

        private void loadExistsLoan()
        {
            try
            {
                sqlDB.fillDataTable("select LoanId,EmpCardNo,LoanAmount,InstallmentNo,InstallmentAmount,StartMonth,convert(varchar(11),EntryDate,105) as EntryDate ,EmpType from v_Payroll_LoanInfo where PaidStatus='false'", dt = new DataTable());
                gvLoanInfo.DataSource = dt;
                gvLoanInfo.DataBind();
            }
            catch { }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            bool status = false;
            for (byte b = 0; b < rbEmpType.Items.Count; b++)
            {
                if (rbEmpType.Items[b].Selected == true)
                {
                    status = true;
                    break;
                }
            }

            if (!status) { lblMessage.InnerText = "error->Please select a type"; return; }
            if (ddlEmpCardNo.SelectedItem.ToString().Trim() == "" && txtFindEmployeeCardNo.Text.Trim().Length < 8)    
            {
                lblMessage.InnerText = "error->Please select an employee card no";
                return;
            }
            if (gvInstallmentDetails.Rows.Count == 0)
            {
                lblMessage.InnerText = "error->Please add installment month.";
                return;
            }
            btnFind_Click(sender, e);
            savePayroll_LoanInfo();
        }

        private void savePayroll_LoanInfo()
        {
            try
            {

                if (!checkHasExistsAnyLoan()) return;
                
                string LoanId = "L-"+txtFindEmployeeCardNo.Text.Trim()+ "_" + rbEmpType.SelectedValue.ToString() + "_" + txtStartMonth.Text.Trim() + "_" + txtEntryDate.Text;

                try
                {
                    string[] getColumns = { "LoanId", "EmpId", "EmpCardNo", "EmpTypeId", "EntryDate", "LoanAmount", "InstallmentNo","InstallmentAmount","StartMonth", "PaidStatus", "Notes",
                                          "PaidInstallmentNo"};
                    string[] getValues = {LoanId,ViewState["__EmpId__"].ToString(),txtFindEmployeeCardNo.Text.Trim(),rbEmpType.SelectedValue.ToString(), convertDateTime.getCertainCulture(txtEntryDate.Text.Trim()).ToString(),
                                         txtAdvanceAmount.Text.Trim(),txtNoOfInstallment.Text.Trim(),gvInstallmentDetails.Rows[0].Cells[1].Text.Trim(),txtStartMonth.Text.Trim().ToString(),"0",txtNotes.Text.Trim(),"0"};
                    if (SQLOperation.forSaveValue("Payroll_LoanInfo", getColumns, getValues, sqlDB.connection) == true)
                    {
                        //savePayroll_AdvanceDetails(AdvanceId);
                        lblMessage.InnerText = "success->Successfully Loan Saved";
                        clearInputBox(); loadExistsLoan();

                    }
                }
                catch (Exception ex)
                {
                    lblMessage.InnerText = "error->" + ex.Message;
                }

            }
            catch (Exception ex)
            { }
        }


        private bool checkHasExistsAnyLoan()
        {
            try
            {
                sqlDB.fillDataTable("select * from Payroll_LoanInfo where EmpCardNo ='" + txtFindEmployeeCardNo.Text.Trim() + "' AND EmpTypeId=" + rbEmpType.SelectedValue.ToString() + " AND PaidStatus='False'",dt=new DataTable ());
                if (dt.Rows.Count > 0)
                {
                    lblMessage.InnerText = "warning->Sorry,This "+rbEmpType.SelectedItem.ToString()+" already taken a loan";
                    return false;
                }
                return true;
            }
            catch { return false; }
        }
        protected void rbEmpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            classes.Employee.LoadEmpCardNoWithName(ddlEmpCardNo, rbEmpType.SelectedValue);
        }

        private void clearInputBox()
        {
            try
            {
                
                txtEntryDate.Text = "";
                txtFindEmployeeCardNo.Text = "";
                txtAdvanceAmount.Text = "";
                txtNoOfInstallment.Text = "";
                txtStartMonth.Text = "";
                txtNotes.Text = "";
                setDefaultColulmns();
                ViewState["LoanId"] = "0";
                ViewState["__EmpId__"] = "0";
            }
            catch { }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            clearInputBox();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            if (!ViewState["LoanId"].ToString().Equals("0"))
            {
                Delete(ViewState["LoanId"].ToString());

                loadExistsLoan();
            }
            else
            {
                lblMessage.InnerText = "error->Please select a record";
            }
        }

        private void Delete(string LoanId)
        {
            try
            {
                sqlDB.fillDataTable("select * from Payroll_LoanSetting where LoanId='" + LoanId + "'", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    lblMessage.InnerText = "warning->Sorry,This Loan info record is not deleted.Causes " + dt.Rows.Count + " installment is Payed or set";
                    return;
                }
                else
                {
                    SQLOperation.forDeleteRecordByIdentifier("Payroll_LoanInfo", "LoanId", LoanId, sqlDB.connection);

                    loadExistsLoan();
                    lblMessage.InnerText = "success->Loan info record is successfully deleted";

                }
                ViewState["LoanId"] = "0";
            }
            catch { }
        }

        protected void gvLoanInfo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                lblMessage.InnerText = "";
                int index = int.Parse(e.CommandArgument.ToString());
                ViewState["LoanId"] = gvLoanInfo.DataKeys[index].Value.ToString();
            }
            catch { }
        }

        protected void ddlEmpCardNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            string[] getEmpCard = ddlEmpCardNo.SelectedItem.Text.Split(' ');
            txtFindEmployeeCardNo.Text = getEmpCard[0];
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            if (txtFindEmployeeCardNo.Text.Trim().Length > 8 && txtFindEmployeeCardNo.Text.Trim().Length < 8)
            {
                lblMessage.InnerText = "warning->Please type a valid card number";
                return;
            }
            else
            {
                sqlDB.fillDataTable("Select Max(SN) as SN, EmpName,EmpId,EmpStatus,EmpStatusName From v_Personnel_EmpCurrentStatus where EmpTypeId=" + rbEmpType.SelectedValue.ToString() + " AND EmpCardNo='" + txtFindEmployeeCardNo.Text.Trim() + "' Group by EmpCardNo,EmpId,EmpName,EmpStatus,EmpStatusName ", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["EmpStatus"].ToString().Equals("1") || dt.Rows[0]["EmpStatus"].ToString().Equals("8"))
                    {
                        ViewState["__EmpId__"] = dt.Rows[0]["EmpId"].ToString();
                        lblMessage.InnerText = "warning->Employee name is " + dt.Rows[0]["EmpName"].ToString();
                    }
                    else lblMessage.InnerText = "warning->" + dt.Rows[0]["EmpName"].ToString() + " is " + dt.Rows[0]["EmpStatusName"].ToString();
                }
                else lblMessage.InnerText = "Sorry,This card number is not valid";
            }
        }
       
    }
}