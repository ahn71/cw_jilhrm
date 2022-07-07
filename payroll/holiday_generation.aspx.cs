using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace SigmaERP.payroll
{
    public partial class holiday_generation : System.Web.UI.Page
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
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='holiday_generation.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
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
            //lbProcessingStatus.Items.Clear();
            //string[] getDays = txtGenerateMonth.Text.Trim().Split('-');
           
            //int days = DateTime.DaysInMonth(int.Parse(getDays[2]), int.Parse(getDays[1]));

            //Read_N_Write_WH(days, int.Parse(getDays[1]), getDays[2]);

          //  loadMonthSetup(1, int.Parse(getDays[1]), int.Parse(getDays[2]));

            //if (dtGetMonthSetup.Rows.Count == 0)
            //{
            //    lblMessage.InnerText = "error->This month is not setup";
            //    return;
            //}
           //getHoliDayAccount( getDays[1],getDays[2]);
            HolidayTaksGeneration();

        }

        private void deletePreviousRecordByDate()
        {
            try
            {
                SQLOperation.forDeleteRecordByIdentifier("Payroll_HolidayWeekendBill", "AttDate", txtGenerateMonth.Text.Trim(), sqlDB.connection);
            }
            catch { }
        }

        private void HolidayTaksGeneration()
        {
            try
            {
                deletePreviousRecordByDate();

                DataTable dt = new System.Data.DataTable();
                sqlDB.fillDataTable("select * from tblHolydayWork where HDate='"+txtGenerateMonth.Text.Trim()+"'",dt);
                if (dt.Rows.Count == 0)
                {
                    sqlDB.fillDataTable("select * from Attendance_WeekendInfo where WeekendDate='" + txtGenerateMonth.Text.Trim() + "'", dt=new System.Data.DataTable ());
                }

                if (dt.Rows.Count > 0)
                {
                    sqlDB.fillDataTable("select  distinct EmpId,EmpCardNo,EmpProximityNo,InHour,InMin,OutHour,OutMin,Did,DptId,DsgId,LnId,EmpTypeID from v_tblAttendanceRecord where ATTDate ='" + txtGenerateMonth.Text.Trim() + "' AND InHour not in ('00','0') And EmptypeId='2' AND IsActive='1'", dt = new System.Data.DataTable());

                    for (int r = 0; r < dt.Rows.Count; r++)
                    {
                        string logInTime = dt.Rows[r]["InHour"].ToString() + ":" + dt.Rows[r]["InMin"].ToString();
                        string logOutTime = dt.Rows[r]["OutHour"].ToString() + ":" + dt.Rows[r]["OutMin"].ToString();

                        string dailyOT = (TimeSpan.Parse(logOutTime) - (TimeSpan.Parse(logInTime))).ToString();

                        // double temp = double.Parse(dailyOT);
                        string[] temp = dailyOT.Split(':');
                        string takeTempOT = temp[0]+"."+temp[1];

                        dailyOT = temp[0] + ":" + temp[1];

                        try
                        {
                            string[] getColumns = { "EmpCardNo", "EmpId", "AttDate", "InHour", "InMin", "OutHour", "OutMin", "DId", "DptId", "EmpTypeId", "DsgId", "LnId", "TotalWorkHour", "TotalWorkMin", "StayTime" };
                            string[] getValues = { dt.Rows[r]["EmpCardNo"].ToString(),dt.Rows[r]["EmpId"].ToString(), convertDateTime.getCertainCulture(txtGenerateMonth.Text.Trim()).ToString(), dt.Rows[r]["InHour"].ToString(),dt.Rows[r]["InMin"].ToString(),
                                                  dt.Rows[r]["OutHour"].ToString(),dt.Rows[r]["OutMin"].ToString(),dt.Rows[r]["DId"].ToString(),dt.Rows[r]["DptId"].ToString(),dt.Rows[r]["EmpTypeId"].ToString(),dt.Rows[r]["DsgId"].ToString(),dt.Rows[r]["LnId"].ToString(),
                                                  temp[0].ToString(),temp[1].ToString(),dailyOT};
                            SQLOperation.forSaveValue("Payroll_HolidayWeekendBill", getColumns, getValues, sqlDB.connection);
                           
                        }
                        catch (Exception ex)
                        {
                           // MessageBox.Show(ex.Message);
                        }
                    
                    }

                  // 
                      
                
                }
                //if (dt.Rows.Count > 0)
                HolidayBillGeneration();
            }
            catch { }
        
        }

        private void HolidayBillGeneration()
        {
            try
            {
                string[] getDays = txtGenerateMonth.Text.Split('-');
                string date = getDays[2] + "-" + getDays[1] + "-" + getDays[0];

                sqlDB.fillDataTable("select * from HRD_OthersSetting ", dt = new DataTable());




                sqlDB.fillDataTable("select  EmpCardNo,EmpName,DsgName,DptId,DptName,HolidayAllownce,LnId,Lncode,LogIn,LogOut,EmpTypeId,EmpType,TotalWorkHour,TotalWorkMin,AttDate,StayTime,ElapsedTime,IsActive from v_Payroll_HolidayWeekendBill group by EmpCardNo,EmpName,DsgName,DptId,DptName,HolidayAllownce,LnId,Lncode,LogIn,LogOut,EmpTypeId,EmpType,TotalWorkHour,TotalWorkMin,AttDate,StayTime,ElapsedTime,IsActive having attdate='" + txtGenerateMonth.Text + "' and ElapsedTime >='" + dt.Rows[0]["StaffHolidayTotalHour"].ToString() + ':' + dt.Rows[0]["StaffHolidayTotalMin"].ToString() + "' AND EmpTypeId='2' AND IsActive='1'", dt = new DataTable());

                Session["__HolidayBill__"] = dt;

                if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->No Holiday Bill Found";
                    return;
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=HolidayBill-" + date + "');", true);  //Open New Tab for Sever side code

            }
            catch { }
        
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

        ArrayList getHoliDay = new ArrayList();
        private void Read_N_Write_WH(int days, int month, string year)    // N=And,WH=Weekly Holiday
        {
            try
            {
                getHoliDay.Clear();
                DataTable dtHoliday = new DataTable();
                sqlDB.fillDataTable("select HCode,HDate,Description from v_tblHolydayWork where HDate='" + txtGenerateMonth.Text.Trim() + "'", dtHoliday);
                for (byte b = 0; b < dtHoliday.Rows.Count; b++)
                {
                    string [] getDate=(dtHoliday.Rows[b]["HDate"].ToString()).Split('-');
                    getHoliDay.Add(getDate[2] + "-" + getDate[1] + "-" + getDate[0]);
                }

                //if (rbType.SelectedIndex != 0)
                //{
                //    DateTime begin = new DateTime(int.Parse(year), month, 1);
                //    DateTime end = new DateTime(int.Parse(year), month, days);

                //    while (begin <= end)
                //    {
                //        if (begin.DayOfWeek == DayOfWeek.Friday)
                //        {
                //            getHoliDay.Add(begin.ToString("yyyy-MM-dd"));
                //        }
                //        begin = begin.AddDays(1);
                //    }
                //}


                
            }
            catch { }
        }

        DataTable dtCertainEmp;
        private void getHoliDayAccount( string Month,string Year)
        {
            try
            {
                string predicate="";
                for (byte b = 0; b < getHoliDay.Count; b++)
                {

                    if (b==0 & b == getHoliDay.Count - 1) predicate = "('" + getHoliDay[b] + "')";
                    else
                    { 
                        if (b==0)predicate+="('"+getHoliDay[b]+"',";
                        else if (b == getHoliDay.Count - 1) predicate += "'"+getHoliDay[b] + "')";
                        else predicate += "'"+getHoliDay[b]+"',";
                    
                    }

                }

                string SQLCMD = "select * from tblAttendanceRecord where ATTDate in " + predicate + " AND ATTStatus In ('P','L') AND EmpTypeId='2'";   // 2=Worker

                sqlDB.fillDataTable(SQLCMD,dtCertainEmp=new DataTable ());

                if (dtCertainEmp.Rows.Count == 0)
                {
                    lblMessage.InnerText = "error->Please First generate payroll,Then generate holiday bill."; return;
                }

                holidayCalculation( Month, Year);

            }
            catch (Exception ex)
            { 
            
            }
        }

        private void holidayCalculation(string Month,string Year)
        {
            try
            {
                DataTable dtProximityNo = dtCertainEmp.DefaultView.ToTable(true, "EmpProximityNo");
                for (byte b = 0; b < dtProximityNo.Rows.Count; b++)
                {
                    DataTable dtCertainEmpDetails = dtCertainEmp.Select("EmpProximityNo='" + dtProximityNo.Rows[b]["EmpProximityNo"].ToString() + "'").CopyToDataTable();
                    byte count=0;
                    for (byte r = 0; r < dtCertainEmpDetails.Rows.Count; r++)
                    { 
                        string logInTime=dtCertainEmpDetails.Rows[r]["InHour"].ToString() + ":" + dtCertainEmpDetails.Rows[r]["InMin"].ToString();
                        string logOutTime=dtCertainEmpDetails.Rows[r]["OutHour"].ToString() + ":" + dtCertainEmpDetails.Rows[r]["OutMin"].ToString();

                        string getWorkTime = (TimeSpan.Parse(logOutTime) - TimeSpan.Parse(logInTime)).ToString();

                        int workHour=Convert.ToDateTime(getWorkTime).Hour;
                        int workMinute = Convert.ToDateTime(getWorkTime).Minute;
                        string HoureMinute = workHour + "." + workMinute;
                        HoureMinute = Math.Round(double.Parse(HoureMinute), 0).ToString();
                        if (int.Parse(HoureMinute) >= 4) count++;
                        
                    }

                    sqlDB.fillDataTable("select HolidayAllownce,EmpName from v_Personnel_EmpCurrentStatus where EmpProximityNo ='"+dtProximityNo.Rows[b]["EmpProximityNo"].ToString()+"' And ActiveSalary='true'",dt=new DataTable ());
                    double getHolidayBill = Math.Round(double.Parse(dt.Rows[b]["HolidayAllownce"].ToString())*count);
                    
                    //SQLOperation.cmd = new System.Data.SqlClient.SqlCommand("update Payroll_MonthlySalarySheet set HoliDayBillAmount=" + getHolidayBill + " where EmpProximityNo='" + dtProximityNo.Rows[0]["EmpProximityNo"].ToString()+ "' AND Month ='"+Month+"' AND Year='"+Year+"'",sqlDB.connection);
                    //SQLOperation.cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("insert into Payroll_HolidayAllowanceGenerate (EmpId,HDate,HolidayAllownce) values (@EmpId,@HDate,@HolidayAllownce)",sqlDB.connection);
                    cmd.Parameters.AddWithValue("@EmpId", dtProximityNo.Rows[b]["EmpId"].ToString());
                    cmd.Parameters.AddWithValue("@HDate",convertDateTime.getCertainCulture(txtGenerateMonth.Text.Trim()));
                    cmd.Parameters.AddWithValue("@HolidayAllownce", getHolidayBill);
                    cmd.ExecuteNonQuery();

                    lbProcessingStatus.Items.Add("Processing complete of a staff "+dt.Rows[0]["EmpName"].ToString()+" Proximity No. " + dtProximityNo.Rows[b]["EmpProximityNo"].ToString() + "");
                }
            }
            catch { }
        }

        


    }
}