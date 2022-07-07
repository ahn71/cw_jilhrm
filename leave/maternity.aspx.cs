using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.personnel
{
    public partial class maternity : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            
            if (!IsPostBack)
            {
                classes.commonTask.loadEmpTypeInRadioButtonList(rblEmpType);
                //loadMonthName();
                btnDelete.Visible = false;
            }

        }
        /*
        private void loadMonthName()
        {
            try
            {
                SQLOperation.selectBySetCommandInDatatable("select distinct (Convert(nvarchar(50),Payroll_MonthlySalarySheet.Month)+'-'+Payroll_MonthlySalarySheet.Year) as Month from Payroll_MonthlySalarySheet", dt = new DataTable(), sqlDB.connection);
                ddlMonthID.DataValueField = "Month";
                ddlMonthID.DataTextField = "Month";
                ddlMonthID.DataSource = dt;
                ddlMonthID.DataBind();
                ddlMonthID.Items.Insert(0, new ListItem(" ", " "));

            }
            catch (Exception ex)
            {

            }
        }
        */
        protected void rblOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                if (rblOptions.SelectedIndex == 2)
                {
                    ddlCardNo.Enabled = false;
                    txtCardNo.Enabled = true;
                    btnDelete.Visible = false;
                    
                }
                else
                {
                    txtCardNo.Enabled = false;
                    ddlCardNo.Enabled = true;
                    
                    classes.Employee.LoadEmpCardNoWithName(ddlCardNo, rblEmpType.SelectedValue, rblOptions.SelectedItem.ToString());
                    if (rblOptions.SelectedIndex == 1) btnDelete.Visible = true;
                    else if (rblOptions.SelectedIndex == 2) btnPreview.Text = "Details";
                    else btnDelete.Visible = false;
                }
                txtCardNo.Text = "";
            }
            catch { }
        }

        protected void rblEmpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            txtCardNo.Text = "";
            if (rblOptions.SelectedIndex == 0 || rblOptions.SelectedIndex==1) classes.Employee.LoadEmpCardNoWithName(ddlCardNo, rblEmpType.SelectedValue, rblOptions.SelectedItem.ToString());
           
        }

        protected void ddlCardNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                string[] getCardNo = ddlCardNo.SelectedItem.ToString().Split(' ');
                txtCardNo.Text = getCardNo[0];
            }
            catch { }

        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            
            if (rblOptions.SelectedIndex == 0)
            {
                if (!validationBasket(rblOptions.SelectedIndex)) return;
                MaternityLeaveCalculation();
            }
            else if (rblOptions.SelectedIndex == 1)
            {
                if (!validationBasket(rblOptions.SelectedIndex)) return;
                justUpdateSecondInstallmentSignature();
            }

            else
            {
                if (!validationBasket(rblOptions.SelectedIndex)) return;
                MaternityPayment_Details();
            }
        }

        

        private bool validationBasket(int index)
        {
            try
            {
                if (index == 0)
                {
                    //if (ddlMonthID.SelectedItem.ToString().Trim().Length < 3)
                    //{
                    //    lblMessage.InnerText = "warning->Please Select Month Id"; ddlMonthID.Focus(); return false;
                    //}
                    if (txtCardNo.Text.Trim().Length < 4)
                    {
                        lblMessage.InnerText = "warning->Please Type CardNo"; ddlCardNo.Focus(); return false;
                    }
                    return true;
                }
                else if (index == 1 || index == 2)
                {
                    if (txtCardNo.Text.Trim().Length < 4)
                    {
                        if (index == 1) { lblMessage.InnerText = "warning->Please Type CardNo"; ddlCardNo.Focus(); return false; }
                        else { lblMessage.InnerText = "warning->Please Type CardNo"; txtCardNo.Focus(); return false; }
                    }
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        private void MaternityLeaveCalculation()
        {
            try
            {
                float getBouns = 0;
                float getTotalSalary = 0;
                double getAverageWages = 0;
                string getInstallmetnAmount = "0";
                int getTotalPresentDays = 0;

                sqlDB.fillDataTable("select EmpName,convert(varchar(11),EmpJoiningDate,126) as EmpJoiningDate,EmpId from Personnel_EmployeeInfo where EmpId =(select EmpId from Personnel_EmployeeInfo where EmpCardNo ='"+txtCardNo.Text.Trim()+"' AND EmpTypeId=" + rblEmpType.SelectedValue.ToString() + ")", dt = new DataTable());
                ViewState["__EmpId__"] = dt.Rows[0]["EmpId"].ToString(); ViewState["__EmpJoiningDate__"] = dt.Rows[0]["EmpJoiningDate"].ToString(); ViewState["__EmpName__"] = dt.Rows[0]["EmpName"].ToString();
              
                if (!checkHowManyTimesGetML()) return;  
                
                sqlDB.fillDataTable("select dateDiff(day,'" + ViewState["__EmpJoiningDate__"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "') as TotalDays", dt = new DataTable());
                ViewState["__TotalDays__"] = dt.Rows[0]["TotalDays"].ToString();


                if (int.Parse(ViewState["__TotalDays__"].ToString()) >= 180)   //180 Means 6 Months
                {
                    sqlDB.fillDataTable(" select *,Format(YearMonth,'yyyy-MM') as  bonusMonth from v_MonthlySalarySheet where EmpId ='" + ViewState["__EmpId__"].ToString() + "' AND TotalSalary <>'0' order by  YearMonth desc  ", dt = new DataTable());
                   
                        Session["__dtSalaryMonthInfo__"] = dt;
                        ArrayList getBonusList = new ArrayList(3);
                        for (byte b = 0; b <= 2; b++)   // use 3 months
                        {
                            DataTable dtGetBonus = new DataTable();
                            sqlDB.fillDataTable(" select BonusAmount from v_Payroll_YearlyBonusSheet where bonusMonth='" + dt.Rows[0]["bonusMonth"].ToString()+ "' And EmpId='" + ViewState["__EmpId__"].ToString() + "'", dtGetBonus);
                            if (dtGetBonus.Rows.Count > 0)
                            {
                                getBouns += float.Parse(dtGetBonus.Rows[0]["BonusAmount"].ToString());
                                getBonusList.Add(dtGetBonus.Rows[0]["BonusAmount"].ToString());
                                getTotalSalary += float.Parse(dt.Rows[b]["TotalSalary"].ToString());
                            }
                            else
                            {
                                getBonusList.Add(0);
                                getTotalSalary += float.Parse(dt.Rows[b]["TotalSalary"].ToString());
                            }
                            getTotalPresentDays += int.Parse(dt.Rows[b]["PresentDay"].ToString());
                        }
                        getTotalSalary += getBouns;
                        getAverageWages = getTotalSalary / getTotalPresentDays;
                       // getAverageWages = 0;
                        getAverageWages = Math.Round(getAverageWages, 2);
                        getInstallmetnAmount = Math.Round(getAverageWages * 112 / 2, 0).ToString();
                       // getInstallmetnAmount = "0";
                        string getVoucherNo = "MLV-" + txtCardNo.Text.Trim() + "-" + ViewState["__EmpId__"].ToString() + "-" + rblEmpType.SelectedValue.ToString() + "-" + DateTime.Now.ToString("dd-MM-yyyy");
                        saveMLVoucher(getVoucherNo, getTotalSalary.ToString(), getAverageWages.ToString(), getInstallmetnAmount.ToString(), getBonusList,getTotalPresentDays.ToString());

                }
                else
                {
                    lblMessage.InnerText = "warning->Sorry, Employee " + ViewState["__EmpName__"].ToString() + " has due to " + ViewState["__TotalDays__"].ToString() + " days for fillup(SIX) months";
                }

                
            }
            catch { }
        }

        private bool checkHowManyTimesGetML()
        {
            try
            {
                sqlDB.fillDataTable("select * from Leave_MaterintyVoucher where EmpId='" + ViewState["__EmpId__"].ToString() + "'", dt = new DataTable());
                if (dt.Rows.Count >= 2)
                {
                    lblMessage.InnerText = "warning->" + ViewState["__EmpName__"].ToString() +"already spented 2 times maternity leave !";
                    return false;
                }
                else return true;
            }
            catch { return false; }
        }
        private void saveMLVoucher(string voucherNo, string TotalPayment, string AverageWages, string InstallmentAmount, ArrayList getBonusList, string getTotalPresentDays)
        {
           
            sqlDB.fillDataTable("select DptId,DsgId from Personnel_EmpCurrentStatus where EmpId='" + ViewState["__EmpId__"].ToString() + "' AND IsActive='1' ", dt = new DataTable());

            try
            {
                string[] getColumns = { "MLVoucherNo","EmpId", "EmpTypeId", "DptId", "DsgId", "ThreeMonthsTotalPaymentWithBonus", "AverageWages", 
                                          "InstallmentAmount", "FirstInstallmentSignature","FirstAcceptDate","SecondInstallmentSignature","TotalPresentDays"  };

                string[] getValues = {voucherNo,ViewState["__EmpId__"].ToString(),rblEmpType.SelectedValue.ToString(),
                                         dt.Rows[0]["DptId"].ToString(),dt.Rows[0]["DsgId"].ToString(),TotalPayment.ToString(),AverageWages.ToString(),
                                         InstallmentAmount.ToString(),"1",
                                         convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy")).ToString(),"0",getTotalPresentDays};

                if (SQLOperation.forSaveValue("Leave_MaterintyVoucher", getColumns, getValues,sqlDB.connection) == true)
                {
                    saveMLVoucherDetails(voucherNo,getBonusList);
                    lblMessage.InnerText = "success->Successfully Maternity Voucher Processed";

                    sqlDB.fillDataTable("select MLVoucherNo,EmpNameBn,DsgNameBn,SUBSTRING( EmpCardNo,8,15) as EmpCardNo, DptNameBn,ThreeMonthsTotalPaymentWithBonus,TotalPresentDays from v_Leave_MaterintyVoucher where MLVoucherNo='" + voucherNo + "'", dt = new DataTable());
                    Session["__voucherInfo__"] = dt;

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTab('/All Report/Report.aspx?for=mlvoucher-');", true);  //Open New Tab for Sever side code
                }
            }
            catch (Exception ex)
            {
                
            }
           
        }

        private void saveMLVoucherDetails(string voucherNo,ArrayList getBonusList)
        {
            try
            {
                dt = (DataTable)Session["__dtSalaryMonthInfo__"];
                for (byte b = 0; b < 3; b++)
                {
                    string[] getColumns = { "MLVoucherNo", "MonthId", "PresentDays", "TakenWages", "TakenBonus" };
                    string[] getValues = { voucherNo, dt.Rows[b]["bonusMonth"].ToString(), dt.Rows[b]["PresentDay"].ToString(), dt.Rows[b]["TotalSalary"].ToString(),getBonusList[b].ToString() };
                    SQLOperation.forSaveValue("Leave_MaterintyVoucher_Details", getColumns, getValues, sqlDB.connection);
                    
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        private void justUpdateSecondInstallmentSignature()
        {
            try
            {
                sqlDB.fillDataTable("Select Max(LACode) as LACode, ToDate From  Leave_LeaveApplication where LeaveId=5 and EmpId=(select EmpId from v_Leave_MaterintyVoucher where EmpTypeId=" + rblEmpType.SelectedValue.ToString() + " AND EmpCardNo ='" + txtCardNo.Text.Trim() + "') group by ToDate  ", dt = new DataTable());
                if (Convert.ToDateTime(dt.Rows[0]["ToDate"].ToString()) > DateTime.Now)
                {
                    lblMessage.InnerText = "warning->Not appropriate Date For Second Installment ";
                    return;
                }

                sqlDB.fillDataTable("select distinct MLVoucherNo from Leave_MaterintyVoucher where FirstInstallmentSignature='true' AND  SecondInstallmentSignature='false' AND EmpId =(select EmpId from v_Leave_MaterintyVoucher where EmpTypeId="+rblEmpType.SelectedValue.ToString()+" AND EmpCardNo ='"+txtCardNo.Text.Trim()+"')", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    SqlCommand cmd = new SqlCommand("Update Leave_MaterintyVoucher set SecondInstallmentSignature='1',SecondAcceptDate='"+convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy")).ToString()+"' where MLVoucherNo='" + dt.Rows[0]["MLVoucherNo"].ToString() + "'",sqlDB.connection);
                    cmd.ExecuteNonQuery();
                    lblMessage.InnerText = "success->Successfully Second Installment Paid";
                }
            }
            catch { }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCardNo.Text.Trim().Length < 4)
                {
                    lblMessage.InnerText = "warning->Please select a card number."; return;
                }
                else
                {
                    sqlDB.fillDataTable("select distinct MLVoucherNo from Leave_MaterintyVoucher where FirstInstallmentSignature='true' AND  SecondInstallmentSignature='false' AND EmpId =(select EmpId from v_Leave_MaterintyVoucher where EmpTypeId=" + rblEmpType.SelectedValue.ToString() + " AND EmpCardNo like '%" + txtCardNo.Text.Trim() + "')", dt = new DataTable());
                    if (SQLOperation.forDeleteRecordByIdentifier("Leave_MaterintyVoucher", "MLVoucherNo", dt.Rows[0]["MLVoucherNo"].ToString(), sqlDB.connection) == true) ;
                    { 
                    lblMessage.InnerText="success-Successfully first installment deleted";
                    }
                }
            }
            catch { }
        }

        private void MaternityPayment_Details()
        {
            try
            {
                //sqlDB.fillDataTable("select *   from v_Leave_MaterintyVoucher where EmpCardNo like'%" + txtCardNo.Text.Trim() + "' AND EmpTypeId=" + rblEmpType.SelectedValue.ToString() + "", dt = new DataTable());
                //Session["__voucherInfo__"] = dt;

                //if (dt.Rows.Count > 0)
                //{
                //    sqlDB.fillDataTable("select * from v_Leave_MaterintyVoucher_Details where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "'", dt = new DataTable());
                //    Session["__VoucherDetails__"] = dt;
                //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTab('/All Report/Report.aspx?for=mlvoucherdetails-');", true);  //Open New Tab for Sever side code
                //}
                string SqlCmd = "  SELECT Count(Distinct v_Leave_MaterintyVoucher_Details.MLVoucherNo) as MLVoucherNo,v_Leave_MaterintyVoucher_Details.MonthId, v_Leave_MaterintyVoucher_Details.PresentDays," +
                    " v_Leave_MaterintyVoucher_Details.TakenWages,v_Leave_MaterintyVoucher_Details.TakenBonus,v_Leave_MaterintyVoucher_Details.ThreeMonthsTotalPaymentWithBonus, v_Leave_MaterintyVoucher_Details.AverageWages,"+
                    "v_Leave_MaterintyVoucher_Details.InstallmentAmount,v_Leave_MaterintyVoucher_Details.CompanyNameBangla, v_Leave_MaterintyVoucher_Details.AddressBangla,v_Leave_MaterintyVoucher_Details.EmpNameBn,"+
                    "v_Leave_MaterintyVoucher_Details.DsgNameBn, v_Leave_MaterintyVoucher_Details.DptNameBn,SUBSTRING( v_Leave_MaterintyVoucher_Details.EmpCardNo,8,15) as EmpCardNo" +

                    " FROM Glory.dbo.v_Leave_MaterintyVoucher_Details"+

                    " where EmpCardNo like'%" + txtCardNo.Text.Trim() + "'AND EmpTypeId=" + rblEmpType.SelectedValue.ToString() + "" +

                    " Group By v_Leave_MaterintyVoucher_Details.MonthId, v_Leave_MaterintyVoucher_Details.PresentDays, v_Leave_MaterintyVoucher_Details.TakenWages,v_Leave_MaterintyVoucher_Details.TakenBonus,"+
                    "v_Leave_MaterintyVoucher_Details.ThreeMonthsTotalPaymentWithBonus, v_Leave_MaterintyVoucher_Details.AverageWages,v_Leave_MaterintyVoucher_Details.InstallmentAmount,v_Leave_MaterintyVoucher_Details.CompanyNameBangla,"+
                    " v_Leave_MaterintyVoucher_Details.AddressBangla,v_Leave_MaterintyVoucher_Details.EmpNameBn,v_Leave_MaterintyVoucher_Details.DsgNameBn, v_Leave_MaterintyVoucher_Details.DptNameBn,v_Leave_MaterintyVoucher_Details.EmpCardNo";


                sqlDB.fillDataTable(SqlCmd, dt = new DataTable());                
                if (dt.Rows.Count > 0)
                {
                    Session["__VoucherDetails__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTab('/All Report/Report.aspx?for=mlvoucherdetails-');", true);  //Open New Tab for Sever side code
                }
                else

                {
                    lblMessage.InnerText = "warning->Sorry,Any voucher details are not founded";
                    return;
                }
            }
            catch { }
        }

       
    }
}