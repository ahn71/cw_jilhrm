using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.payroll
{
    public partial class tiffin_generation : System.Web.UI.Page
    {
        DataTable dt;
        SqlCommand cmd;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                classes.commonTask.loadEmpTypeInRadioButtonList(rbEmpTypeList);
            }
        }

        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='tiffin_generation.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(false))
                        {
                            btnGeneration.CssClass = "";
                            btnGeneration.Enabled = false;
                        }
                    }
                }
            }
            catch { }
        }


        protected void btnGeneration_Click(object sender, EventArgs e)
        {
            /*
            lbProcessingStatus.Items.Clear();
            string[] getDays = txtGenerateMonth.Text.Trim().Split('-');
            int days = DateTime.DaysInMonth(int.Parse(getDays[2]), int.Parse(getDays[1]));

            //  Read_N_Write_WH(days, int.Parse(getDays[1]), getDays[2]);

            loadMonthSetup(1, int.Parse(getDays[1]), int.Parse(getDays[2]));

            if (dtGetMonthSetup.Rows.Count == 0)
            {
                lblMessage.InnerText = "error->This month is not setup";
                return;
            }
            getNightBillAccount(getDays[1], getDays[2]);
             * */

            getDailyTiffinBill();
        }

        DataTable dtGetMonthSetup;
        private void loadMonthSetup(int days, int month, int year)
        {
            try
            {
                string monthName = new DateTime(year, month, days).ToString("MMM", CultureInfo.InvariantCulture);
                monthName += year.ToString().Substring(2, 2);
                SQLOperation.selectBySetCommandInDatatable("select TotalDays,TotalWeekend ,FromDate,ToDate,TotalHoliday,TotalWorkingDays from tblMonthSetup where MonthName='" + monthName + "'", dtGetMonthSetup = new DataTable(), sqlDB.connection);
            }
            catch (Exception ex)
            {

            }
        }

        DataTable dtRunningEmp;
        DataTable dtPresent;
        DataTable dtAbsent;
        private void getNightBillAccount(string month, string year)
        {
            try
            {
                sqlDB.fillDataTable("select EmpProximityNo  from Payroll_MonthlySalarySheet where Month='" + month + "' AND Year='" + year + "'", dt = new DataTable());
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "error->Please First generate payroll,Then generate holiday bill."; return;
                }

                sqlDB.fillDataTable("select distinct EmpCardNo,EmpProximityNo, max(SN) as SN,EmpTypeId,EmpType,EmpStatus,ActiveSalary  from v_Personnel_EmpCurrentStatus group by EmpCardNo,EmpProximityNo,SalaryType,EmpTypeId,EmpType,EmpStatus,ActiveSalary having EmpStatus in ('1','4','5'  )  AND ActiveSalary='true' order by SN", dtRunningEmp = new DataTable());


                for (int r = 0; r < dtRunningEmp.Rows.Count; r++)
                {
                    // get present information of a certain employee
                    sqlDB.fillDataTable("select * from tblAttendanceRecord where EmpProximityNo='" + dtRunningEmp.Rows[r]["EmpProximityNo"].ToString() + "' AND ATTStatus In ('P','L')", dtPresent = new DataTable());

                    sqlDB.fillDataTable("select TiffinAllownce,EmpType,EmpName from v_Personnel_EmpCurrentStatus where  SN=" + dtRunningEmp.Rows[r]["SN"].ToString() + "", dt = new DataTable());

                    tiffinBillCalculation(month, year, dtRunningEmp.Rows[r]["EmpProximityNo"].ToString());
                }



            }
            catch (Exception ex)
            {

            }
        }

        private void tiffinBillCalculation(string Month, string Year, string getProximityNo)
        {
            try
            {

                byte count = 0;
                for (byte r = 0; r < dtPresent.Rows.Count; r++)
                {
                    string logInTime = dtPresent.Rows[r]["InHour"].ToString() + ":" + dtPresent.Rows[r]["InMin"].ToString();
                    string logOutTime = dtPresent.Rows[r]["OutHour"].ToString() + ":" + dtPresent.Rows[r]["OutMin"].ToString();
                    logOutTime = logOutTime.Replace(':', '.');
                    //  string getWorkTime = (TimeSpan.Parse(logOutTime) - TimeSpan.Parse(logInTime)).ToString();

                    if (dt.Rows[0]["EmpType"].ToString().Equals("Worker")) if (double.Parse(logOutTime) >= 9 ) count++;   
                    
                    else if (double.Parse(logOutTime) >= 7) count++;  
                   
                }


                double getTiffinBill = Math.Round(double.Parse(dt.Rows[0]["TiffinAllownce"].ToString()) * count);

                SQLOperation.cmd = new System.Data.SqlClient.SqlCommand("update Payroll_MonthlySalarySheet set TiffinBillAmount=" + getTiffinBill + " where EmpProximityNo='" + getProximityNo + "' AND Month ='" + Month + "' AND Year='" + Year + "'", sqlDB.connection);
                SQLOperation.cmd.ExecuteNonQuery();
                lbProcessingStatus.Items.Add("Processing complete of a " + dt.Rows[0]["EmpType"].ToString() + " " + dt.Rows[0]["EmpName"].ToString() + " Proximity No. " + getProximityNo + "");

            }
            catch { }
        }

        private void getDailyTiffinBill()
        {
            try
            {
                string[] getDays = txtGenerateMonth.Text.Split('-');
                string date = getDays[2] + "-" + getDays[1] + "-" + getDays[0];

                sqlDB.fillDataTable("select * from HRD_OthersSetting ", dt=new DataTable ());
              
                
                if (rbEmpTypeList.SelectedValue.ToString()=="1")
                    sqlDB.fillDataTable("select  EmpCardNo,EmpName,DsgName,DptId,DptName,OutTime,TiffinAllownce,LnId,Lncode,AttDate,OutHour,EmpTypeId,EmpType,OutMin,LogoutTime,IsActive from v_DailyTiffinBillGenerate group by EmpCardNo,EmpName,DsgName,DptId,DptName,OutTime,TiffinAllownce,LnId,Lncode,AttDate,OutHour,EmpTypeId,EmpType,OutMin,LogoutTime,IsActive having attdate='" + date + "' and LogoutTime >='" + dt.Rows[0]["WorkerTiffinHour"].ToString() + ":" + dt.Rows[0]["WorkerTiffinMin"].ToString() + "' AND EmpTypeId=" + rbEmpTypeList.SelectedValue.ToString() + " And IsActive='1'", dt = new DataTable());
                else sqlDB.fillDataTable("select  EmpCardNo,EmpName,DsgName,DptId,DptName,OutTime,TiffinAllownce,LnId,Lncode,AttDate,OutHour,EmpTypeId,EmpType,OutMin,LogoutTime,IsActive from v_DailyTiffinBillGenerate group by EmpCardNo,EmpName,DsgName,DptId,DptName,OutTime,TiffinAllownce,LnId,Lncode,AttDate,OutHour,EmpTypeId,EmpType,OutMin,LogoutTime,IsActive having attdate='" + date + "' and LogoutTime >='" + dt.Rows[0]["StaffTiffinHour"].ToString() + ":" + dt.Rows[0]["StaffTiffinMin"].ToString() + "' AND EmpTypeId=" + rbEmpTypeList.SelectedValue.ToString() + " And IsActive='1'", dt = new DataTable());

                Session["__TiffinBill__"] = dt;
                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->No Tiffin Found";
                    return;
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=TiffinBill-" + date + "');", true);  //Open New Tab for Sever side code
            
            }
            catch { }
        
        }

        
    }
}