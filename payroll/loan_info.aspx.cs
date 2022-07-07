using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.payroll
{
    public partial class loan_info : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();

            if (!IsPostBack)
            {
                classes.commonTask.loadEmpTypeInRadioButtonList(rbEmpTypeList);
                classes.commonTask.loadDivision(ddlDivisionName);
            }
        }

        protected void rblGenerateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblGenerateType.SelectedValue == "All")
            {
                trddlDividison.Visible = true;
                divJobCardpart.Visible = true;
                trddlIndividualCardno.Visible = false;
                trtxtIndividualCardno.Visible = false;

            }
            else
            {
                trddlIndividualCardno.Visible = true;
                trtxtIndividualCardno.Visible = true;

                trddlDividison.Visible = false;
                divJobCardpart.Visible = false;
            }
        }

        protected void ddlDivisionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDepartment(ddlDivisionName.SelectedValue, lstAll);
        }

        private void LoadDepartment(string divisionId, ListBox lst)
        {
            try
            {
                DataTable dt = new DataTable();

                sqlDB.fillDataTable("SELECT DptId, DptName FROM HRD_Department where DId=" + divisionId + "", dt);

                lst.DataValueField = "DptId";
                lst.DataTextField = "DptName";
                lst.DataSource = dt;
                lst.DataBind();
            }
            catch { }
        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            AddRemoveAll(lstAll, lstSelected);
        }
        private void AddRemoveAll(ListBox aSource, ListBox aTarget)
        {

            try
            {

                foreach (ListItem item in aSource.Items)
                {
                    aTarget.Items.Add(item);
                }
                aSource.Items.Clear();

            }
            catch (Exception expException)
            {
                Response.Write(expException.Message);
            }

        }

        private void AddRemoveItem(ListBox aSource, ListBox aTarget)
        {

            ListItemCollection licCollection;

            try
            {

                licCollection = new ListItemCollection();
                for (int intCount = 0; intCount < aSource.Items.Count; intCount++)
                {
                    if (aSource.Items[intCount].Selected == true)
                        licCollection.Add(aSource.Items[intCount]);
                }

                for (int intCount = 0; intCount < licCollection.Count; intCount++)
                {
                    aSource.Items.Remove(licCollection[intCount]);
                    aTarget.Items.Add(licCollection[intCount]);
                }

            }
            catch (Exception expException)
            {
                Response.Write(expException.Message);
            }
            finally
            {
                licCollection = null;
            }

        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            AddRemoveAll(lstSelected, lstAll);
        }

        protected void rbEmpTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!rblGenerateType.SelectedValue.ToString().Equals("All")) classes.Employee.LoadEmpCardNoWithName(ddlCardNo, rbEmpTypeList.SelectedValue);
        }

        protected void ddlCardNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] getCardNo = ddlCardNo.SelectedItem.ToString().Split(' ');
            txtCardNo.Text = getCardNo[0];
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                string setPredicate = "";
                //   string[] getMonth = ddlMonthID.SelectedValue.Split('-');
                for (byte b = 0; b < lstSelected.Items.Count; b++)
                {

                    if (b == 0 && b == lstSelected.Items.Count - 1)
                    {
                        setPredicate = "in('" + lstSelected.Items[b].Value + "')";
                    }
                    else if (b == 0 && b != lstSelected.Items.Count - 1)
                    {
                        setPredicate += "in ('" + lstSelected.Items[b].Value + "'";
                    }
                    else if (b != 0 && b == lstSelected.Items.Count - 1)
                    {
                        setPredicate += ",'" + lstSelected.Items[b].Value + "')";
                    }
                    else setPredicate += ",'" + lstSelected.Items[b].Value + "'";
                }


                DataTable dt = new DataTable();
                if (rblReportType.SelectedValue.ToString().Equals("Summary") && rblGenerateType.SelectedValue.ToString().Equals("Certain") && rbAllAdvanceLastAdvance.SelectedValue.ToString().Equals("Last Loan Info"))
                {

                    sqlDB.fillDataTable("SELECT Distinct EmpCardNo,EmpName,EmpType,LoanId,InstallmentNo,LoanAmount,EmpTypeId,InstallmentAmount,PaidInstallmentNo,StartMonth,Convert(Varchar(11),EntryDate,105)as EntryDate,Notes,DName,DptName,DsgName from v_Payroll_LoanInfo where SL =(select Max(SL) from Payroll_LoanInfo where EmpCardNo='" + txtCardNo.Text.Trim() + "' And EmpTypeId=" + rbEmpTypeList.SelectedValue.ToString() + ")", dt = new DataTable());

                    Session["__rptType__"] = "Certain Last Loan Info Summary";

                }

                else if (rblReportType.SelectedValue.ToString().Equals("Summary") && rblGenerateType.SelectedValue.ToString().Equals("Certain") && rbAllAdvanceLastAdvance.SelectedValue.ToString().Equals("All Loan Info"))
                {
                    sqlDB.fillDataTable(" select  EmpCardNo,EmpName,EmpType,LoanId,InstallmentNo,LoanAmount,InstallmentAmount,PaidInstallmentNo,StartMonth,Convert(Varchar(11),EntryDate,105)as EntryDate,Notes,DName,DptName,DsgName,PaidStatus,LnId,EmpTypeId from v_Payroll_LoanInfo where EmpCardNo='" + txtCardNo.Text.Trim() + "' And EmpTypeId=" + rbEmpTypeList.SelectedValue.ToString() + " order by DptId,LnId,EmpCardno", dt = new DataTable());
                    Session["__rptType__"] = "Certain Emp All Loan Info Summary";
                }

                else if (rblReportType.SelectedValue.ToString().Equals("Summary") && rblGenerateType.SelectedValue.ToString().Equals("All") && rbAllAdvanceLastAdvance.SelectedValue.ToString().Equals("Last Loan Info"))
                {

                    sqlDB.fillDataTable(" select Max(SL) as SL,EmpCardNo,EmpName,EmpType,LoanId,InstallmentNo,LoanAmount,InstallmentAmount,PaidInstallmentNo,StartMonth,Convert(Varchar(11),EntryDate,105)as EntryDate,Notes,DName,DptName,DsgName,PaidStatus,DptId,LnId,EmpTypeId from v_Payroll_LoanInfo group by EmpCardNo,EmpName,EmpType,LoanId,InstallmentNo,LoanAmount,InstallmentAmount,PaidInstallmentNo,StartMonth,EntryDate,Notes,DName,DptName,DsgName,PaidStatus,DptId,LnId,EmpTypeId Having DptId " + setPredicate + " And EmpTypeId=" + rbEmpTypeList.SelectedValue.ToString() + " order by DptId,LnId,EmpCardno ", dt = new DataTable());
                    Session["__rptType__"] = "All Last Loan Info Summary"; // All Emp Last Loan Info Summary ,is same of Certain Emp All Loan Info Summary 
                }
                else if (rblReportType.SelectedValue.ToString().Equals("Summary") && rblGenerateType.SelectedValue.ToString().Equals("All") && rbAllAdvanceLastAdvance.SelectedValue.ToString().Equals("All Loan Info"))
                {

                    sqlDB.fillDataTable(" select EmpTypeId,EmpCardNo,EmpName,EmpType,LoanId,InstallmentNo,LoanAmount,InstallmentAmount,PaidInstallmentNo,StartMonth,Convert(Varchar(11),EntryDate,105)as EntryDate,Notes,DName,DptName,DsgName,PaidStatus,DptId,LnId,LnCode from v_Payroll_LoanInfo group by EmpCardNo,EmpName,EmpType,LoanId,InstallmentNo,LoanAmount,InstallmentAmount,PaidInstallmentNo,StartMonth,EntryDate,Notes,DName,DptName,DsgName,PaidStatus,DptId,LnId,LnCode,EmpTypeId Having DptId " + setPredicate + " AND EmpTypeId=" + rbEmpTypeList.SelectedValue.ToString() + "  order by DptId,LnId,EmpCardno", dt = new DataTable());
                    Session["__rptType__"] = "OverAll Loan Info Summary";
                }

                else if (rblReportType.SelectedValue.ToString().Equals("Details") && rblGenerateType.SelectedValue.ToString().Equals("Certain") && rbAllAdvanceLastAdvance.SelectedValue.ToString().Equals("Last Loan Info"))
                {
                    sqlDB.fillDataTable(" SELECT Distinct Sl,EmpCardNo, EmpName, DptName,LoanId,InstallmentNo, LoanAmount,EmpTypeId from v_Payroll_LoanInfo where SL=(Select Max(SL) From v_Payroll_LoanInfo where EmpCardNo='" + txtCardNo.Text.Trim() + "' AND EmpTypeId=" + rbEmpTypeList.SelectedValue.ToString() + ")", dt = new DataTable());

                    DataTable dtSubRpt = new DataTable();
                    sqlDB.fillDataTable("select Distinct * from v_Payroll_LoanSetting  where Sl=" + dt.Rows[0]["SL"].ToString() + "", dtSubRpt);

                    Session["__ForSubReport__"] = dtSubRpt;
                    Session["__rptType__"] = "Certain Loan Info Details";
                }

                else if (rblReportType.SelectedValue.ToString().Equals("Details") && rblGenerateType.SelectedValue.ToString().Equals("Certain") && rbAllAdvanceLastAdvance.SelectedValue.ToString().Equals("All Loan Info"))
                {
                    sqlDB.fillDataTable(" SELECT Distinct Sl,LoanId,EmpCardNo, EmpName, DptName, InstallmentNo, LoanAmount,EmpTypeId from v_Payroll_LoanInfo where EmpCardNo='" + txtCardNo.Text.Trim() + "' AND EmpTypeId=" + rbEmpTypeList.SelectedValue.ToString() + " order by SL Desc ", dt = new DataTable());

                    DataTable dtSubRpt = new DataTable();
                    sqlDB.fillDataTable("select Distinct * from v_Payroll_LoanSetting where EmpCardNo='" + txtCardNo.Text.Trim() + "' AND EmpTypeId=" + rbEmpTypeList.SelectedValue.ToString() + " order by SL Desc ", dtSubRpt);

                    Session["__ForSubReport__"] = dtSubRpt;
                    Session["__rptType__"] = "Certain All Loan Info Details";
                }

                else if (rblReportType.SelectedValue.ToString().Equals("Details") && rblGenerateType.SelectedValue.ToString().Equals("All") && rbAllAdvanceLastAdvance.SelectedValue.ToString().Equals("Last Loan Info"))
                {
                    sqlDB.fillDataTable(" SELECT Distinct Max(SL) as SL,LoanId,EmpCardNo, EmpName,LnId,DptId, DptName,LnCode, InstallmentNo, LoanAmount ,EmpTypeId from v_Payroll_LoanInfo group by LoanId,EmpCardNo, EmpName, DptName, InstallmentNo, LoanAmount ,EmpTypeId,LnCode,LnId,DptId having EmpTypeId=" + rbEmpTypeList.SelectedValue.ToString() + " order by DptId,LnId,EmpCardNo ", dt = new DataTable());

                    DataTable dtSubRpt = new DataTable();
                    sqlDB.fillDataTable("select Distinct * from v_Payroll_LoanSetting where EmpTypeId=" + rbEmpTypeList.SelectedValue.ToString() + " order by SL Desc ", dtSubRpt);

                    Session["__ForSubReport__"] = dtSubRpt;
                    Session["__rptType__"] = "All Emp Last Loan Details";
                }

                else if (rblReportType.SelectedValue.ToString().Equals("Details") && rblGenerateType.SelectedValue.ToString().Equals("All") && rbAllAdvanceLastAdvance.SelectedValue.ToString().Equals("All Loan Info"))
                {
                    sqlDB.fillDataTable(" SELECT Distinct SL,LoanId,EmpCardNo, EmpName,LnId,DptId, DptName,LnCode, InstallmentNo, LoanAmount ,EmpTypeId from v_Payroll_LoanInfo where EmpTypeId=" + rbEmpTypeList.SelectedValue.ToString() + " order by DptId,LnId,EmpCardNo ", dt = new DataTable());

                    DataTable dtSubRpt = new DataTable();
                    sqlDB.fillDataTable("select Distinct * from v_Payroll_LoanSetting where EmpTypeId=" + rbEmpTypeList.SelectedValue.ToString() + " order by SL Desc ", dtSubRpt);

                    Session["__ForSubReport__"] = dtSubRpt;
                    Session["__rptType__"] = "All Emp All Loan Details";
                }

                Session["__LoanInfo__"] = dt;

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=LoanInfo');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
    }
}