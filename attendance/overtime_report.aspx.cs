using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.attendance
{
    public partial class overtime_report1 : System.Web.UI.Page
    {
        DataTable dt;
        string CompanyId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                classes.commonTask.LoadEmpTypeWithAll(rblEmpType);
                rblEmpType.SelectedValue = "1";
                if (!classes.commonTask.HasBranch())
                    ddlCompany.Enabled = false;
                ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();

                Session["__MinDigits__"] = "4";
            }
           
        }
        private void setPrivilege()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                //------------load privilege setting inof from db------
                //------------load privilege setting inof from db------
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "daily_movement.aspx", ddlCompany, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];

                classes.commonTask.LoadShiftNameByCompany(ViewState["__CompanyId__"].ToString(),ddlShift);
                classes.commonTask.LoadMonthName(ViewState["__CompanyId__"].ToString(), ddlMonthName);
                trMonth.Visible = false;
                classes.commonTask.LoadDepartment(ViewState["__CompanyId__"].ToString(), lstAll);
                //-----------------------------------------------------
            }
            catch { }
        }
        protected void rdbDailyOT_CheckedChanged(object sender, EventArgs e)
        {
            trMonth.Visible = false;
            rdbMonthlyOT.Checked = false;
            trDate.Visible = true;
        }

        protected void rdbMonthlyOT_CheckedChanged(object sender, EventArgs e)
        {
            trDate.Visible = false;
            trMonth.Visible = true;
            rdbDailyOT.Checked = false;
        }

 
        private void LoadDepartment(string divisionId, ListBox lst)
        {
            try
            {
                dt = new DataTable();

                sqlDB.fillDataTable("SELECT DptId, DptName FROM HRD_Department where DId=" + divisionId + "", dt);

                lst.DataValueField = "DptId";
                lst.DataTextField = "DptName";
                lst.DataSource = dt;
                lst.DataBind();
            }
            catch { }
        }
        

     

      

        protected void btnPreview_Click(object sender, EventArgs e)
        {


            if (rdbDailyOT.Checked)
                OTReport();
            else
                MonthlyOverTimeReport();
        }
        private void MonthlyOverTimeReport()
        {
            try
            {
                if (ddlMonthName.SelectedIndex<1)
                {
                    lblMessage.InnerText = "warning-> Please Select Month Name !";
                    ddlMonthName.Focus();
                    return;
                }
                if (lstSelected.Items.Count == 0 && txtCardNo.Text.Trim() == "")
                {
                    lblMessage.InnerText = "warning-> Please Select Any Department !";
                    lstSelected.Focus();
                    return;
                }

                string EmpTypeID = (rblEmpType.SelectedValue == "0") ? "" : " and EmpTypeId= " + rblEmpType.SelectedValue + "";
                string CompanyList = "";
                string ShiftList = (ddlShift.SelectedItem.Text == "All") ? "" : " and SftId= " + ddlShift.SelectedValue + "";
                string DepartmentList = "";
                string ReportTitle = "";
                string ReportDate = "";
                if (!Page.IsValid)   // If Java script are desible then 
                {
                    lblMessage.InnerText = "erroe->Please Select From Date And To Date"; return;
                }

                CompanyList = (ddlCompany.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue.ToString();
                CompanyList = "in ('" + CompanyList + "')";
                DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
                DataTable dt = new DataTable();
                string[] MY =  ddlMonthName.SelectedItem.Text.Split('-');
                dt = classes.BusinessLogic.get_MonthlyOvertimeReprot(CompanyList, DepartmentList, ShiftList, MY[0], MY[1], txtCardNo.Text, EmpTypeID);
                 if (dt.Rows.Count == 0)
                {
                    lblMessage.InnerText = "warning->No Attendance Available";                  
                    return;
                }
                Session["__MonthlyOverTimeReport__"] = dt;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=MonthlyOverTimeReport-" + ddlMonthName.SelectedItem.Text +"');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
        private void OTReport() 
        {
            if (dptDate.Text.Trim().Length < 8)
            {
                lblMessage.InnerText = "warning-> Please Select Valid Date !";
                dptDate.Focus();
                return;
            }
            if (lstSelected.Items.Count == 0 && txtCardNo.Text.Trim() == "")
            {
                lblMessage.InnerText = "warning-> Please Select Any Department !";
                lstSelected.Focus();
                return;
            }

            string ShiftName = (ddlShift.SelectedValue == "0") ? "" : " and SftName='" + ddlShift.SelectedValue + "' ";
            string EmpTypeID = (rblEmpType.SelectedValue == "All") ? "" : " and EmpTypeId=" + rblEmpType.SelectedValue + " ";
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue.ToString();
            string  DepartmentList = classes.commonTask.getDepartmentList(lstSelected);
            string[] dmy = dptDate.Text.Split('-');
            string d = dmy[0]; string m = dmy[1]; string y = dmy[2];
            
         
            if (txtCardNo.Text.Trim().Length == 0) sqlDB.fillDataTable("Select Format(ATTDate,'dd-MM-yyyy') as ATTDate,SubString(EmpCardNo,10,15) as EmpCardNo,EmpName,DsgName,InHour,InMin,OutHour,OutMin,InSec,OutSec,CompanyName,DptName,SftName,Address,ATTStatus,CompanyId,DptId,SftId,Overtime,TotalOverTime,OtherOverTime,GId,GName From v_tblAttendanceRecord where ATTDate='" + y + "-" + m + "-" + d + "' and ActiveSalary='True' and IsActive=1 and CompanyId ='" + CompanyId + "' " + ShiftName + "  AND DptId " + DepartmentList + " " + EmpTypeID + " And TotalOverTime>'00:00:00' order by convert(int,DptCode),convert(int,GId), convert(int,SftId),CustomOrdering ", dt = new DataTable());
            else
            {
                if (txtCardNo.Text.Trim().Length < int.Parse(Session["__MinDigits__"].ToString()))
                {
                    lblMessage.InnerText = "warning-> Please Type Valid Card Number!(Minimum " + Session["__MinDigits__"].ToString() + " Digits)";
                    txtCardNo.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    return;
                }
                sqlDB.fillDataTable("Select Format(ATTDate,'dd-MM-yyyy') as ATTDate,SubString(EmpCardNo,10,15) as EmpCardNo,EmpName,DsgName,InHour,InMin,OutHour,OutMin,InSec,OutSec,CompanyName,DptName,SftName,Address,ATTStatus,CompanyId,DptId,SftId,Overtime,TotalOverTime,OtherOverTime,GId,GName From v_tblAttendanceRecord where ATTDate='" + y + "-" + m + "-" + d + "' and ActiveSalary='True' and IsActive=1 and EmpCardNo Like'%" + txtCardNo.Text.Trim() + "' and CompanyId= '" + CompanyId + "' And TotalOverTime>'00:00:00' ", dt = new DataTable());
            }

            if (dt.Rows.Count == 0)
            {
                lblMessage.InnerText = "warning-> Overtime Not Available";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                return;
            }          
            Session["__DailyOTReport__"] = dt;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=DailyOTReport-" + dptDate.Text + "');", true);  //Open New Tab for Sever side code
        
        }
        private void Daily_OverTimeGenerate(string dptName)
        {
            try
            {
                
                DataTable dtRunningEmp = new DataTable();

                SqlCommand cmd1 = new SqlCommand("delete from Attendance_OverTime_Counter", sqlDB.connection);
                cmd1.ExecuteNonQuery();
                string[] getDate = dptDate.Text.Split('-');
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select MAX(SN) as SN, EmpId From v_tblAttendanceRecord Where EmpTypeId=" + 1 + " AND ActiveSalary='true' and IsActive=1  and ATTDate='" + getDate[2] + "-" + getDate[1] + "-" + getDate[0] + "' and DptName " + dptName + " Group By EmpId ", dt);
                string setSn = "", setEmpId = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0 && i == dt.Rows.Count - 1)
                    {
                        setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        setEmpId = "in('" + dt.Rows[i].ItemArray[1].ToString() + "')";
                    }
                    else if (i == 0 && i != dt.Rows.Count - 1)
                    {
                        setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        setEmpId += "in ('" + dt.Rows[i].ItemArray[1].ToString() + "'";
                    }
                    else if (i != 0 && i == dt.Rows.Count - 1)
                    {
                        setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        setEmpId += ",'" + dt.Rows[i].ItemArray[1].ToString() + "')";
                    }
                    else
                    {
                        setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        setEmpId += ",'" + dt.Rows[i].ItemArray[1].ToString() + "'";
                    }
                }

                sqlDB.fillDataTable("select EmpCardNo,SN,EmpId,EmpName,EmpType,EmpTypeId,ActiveSalary  from v_tblAttendanceRecord  where   EmpTypeId=" + 1 + " AND ActiveSalary='true' and IsActive=1  and SN " + setSn + "  AND ATTDate='" + getDate[2] + "-" + getDate[1] + "-" + getDate[0] + "'", dtRunningEmp = new DataTable());

                // sqlDB.fillDataTable("select EmpCardNo,EmpId,EmpName,SN,EmpType,EmpTypeId,EmpStatus,ActiveSalary  from v_Personnel_EmpCurrentStatus where EmpCardNo ='00001166'",dtRunningEmp=new DataTable());
                for (int i = 0; i < dtRunningEmp.Rows.Count; i++)
                {

                    string getTotaOTHM = "0";
                    DataTable dtPresent = new DataTable();

                    string a = dtRunningEmp.Rows[i]["EmpId"].ToString();

                    // get present information of a certain employee
                    sqlDB.fillDataTable("select * from v_tblAttendanceRecord where SN='" + dtRunningEmp.Rows[i]["SN"].ToString() + "' AND ATTStatus In ('P','L') AND ATTDate='" + getDate[2] + "-" + getDate[1] + "-" + getDate[0] + "'", dtPresent = new DataTable());
                    DataTable dtOTStart = new DataTable();
                    sqlDB.fillDataTable("Select LogoutHour,LogoutMin From Attendance_MonthlyLogoutTimeAndOTSetting where MonthDate='" + getDate[2] + "-" + getDate[1] + "-" + getDate[0] + "'", dtOTStart);

                    if (dtPresent.Rows.Count > 0)
                    {
                        double getOutHour = double.Parse(dtPresent.Rows[0]["OutHour"].ToString());
                        string getOutMinuts;
                        if (dtPresent.Rows[0]["OutMin"].ToString().Length == 1) getOutMinuts = ".0" + double.Parse(dtPresent.Rows[0]["OutMin"].ToString());
                        else getOutMinuts = "." + double.Parse(dtPresent.Rows[0]["OutMin"].ToString());

                        getOutHour = getOutHour + double.Parse(getOutMinuts);

                        if (getOutHour >= double.Parse(dtOTStart.Rows[0]["LogoutHour"].ToString() + "." + dtOTStart.Rows[0]["LogoutMin"].ToString()))
                        {
                            string logOutTime = dtPresent.Rows[0]["OutHour"].ToString() + ":" + dtPresent.Rows[0]["OutMin"].ToString();

                            string dailyOT = (TimeSpan.Parse(logOutTime) - (TimeSpan.Parse(dtOTStart.Rows[0]["LogoutHour"].ToString() + ":" + dtOTStart.Rows[0]["LogoutMin"].ToString()))).ToString();

                            getTotaOTHM = (TimeSpan.Parse(dailyOT) + TimeSpan.Parse(getTotaOTHM)).ToString();

                            // getTotaOTHM=Math.Round(double.Parse(getTotaOTHM),0).ToString();


                            string[] getOTHoureMinuts = getTotaOTHM.Split(':');
                            if (!getTotaOTHM.Length.Equals(1) && !getTotaOTHM.Equals("0"))
                            {
                                double OTH = double.Parse(getOTHoureMinuts[0]);
                                string OTM = "." + getOTHoureMinuts[1];

                                getTotaOTHM = OTH.ToString() + OTM;
                                getTotaOTHM = Math.Round(double.Parse(getTotaOTHM), 0).ToString();
                                if (getTotaOTHM != "0")
                                {

                                    SqlCommand cmd = new SqlCommand("insert into Attendance_OverTime_Counter (EmpId,AttDate,OverTime) values (@EmpId,@AttDate,@OverTime)", sqlDB.connection);
                                    cmd.Parameters.AddWithValue("@EmpId", dtRunningEmp.Rows[i]["EmpId"].ToString());
                                    cmd.Parameters.AddWithValue("@AttDate", convertDateTime.getCertainCulture(dptDate.Text.Trim()));
                                    cmd.Parameters.AddWithValue("@OverTime", getTotaOTHM);
                                    cmd.ExecuteNonQuery();
                                }

                            }
                        }
                        //getOTRate = Math.Round(getBasic / 104, 2);
                        //TotalOTTaka = Math.Round(getOTRate * double.Parse(getTotaOTHM), 0);
                        //ViewState["__getTotaOTHM__"] = getTotaOTHM;
                    }
                }
                sqlDB.fillDataTable("Select MAX(SN) as SN, EmpId From v_Attendance_OverTime_Counter Where ATTDate='" + getDate[2] + "-" + getDate[1] + "-" + getDate[0] + "' and IsActive=1  Group By EmpId ", dt);
                setSn = ""; setEmpId = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == 0 && i == dt.Rows.Count - 1)
                    {
                        setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        setEmpId = "in('" + dt.Rows[i].ItemArray[1].ToString() + "')";
                    }
                    else if (i == 0 && i != dt.Rows.Count - 1)
                    {
                        setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        setEmpId += "in ('" + dt.Rows[i].ItemArray[1].ToString() + "'";
                    }
                    else if (i != 0 && i == dt.Rows.Count - 1)
                    {
                        setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                        setEmpId += ",'" + dt.Rows[i].ItemArray[1].ToString() + "')";
                    }
                    else
                    {
                        setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                        setEmpId += ",'" + dt.Rows[i].ItemArray[1].ToString() + "'";
                    }
                }

                DataTable dtoverTime = new DataTable();
                sqlDB.fillDataTable("Select  EmpCardNo, EmpName,DsgName,DName,LnCode,DptName,convert(varchar(11),AttDate,106) as AttDate,OverTime From v_Attendance_OverTime_Counter Where  ATTDate='" + getDate[2] + "-" + getDate[1] + "-" + getDate[0] + "' and ActiveSalary='True' and IsActive=1  and OverTime>0  Order By LnCode,EmpCardNo ", dtoverTime);

                Session["__OverTime__"] = dtoverTime;
                if (dtoverTime.Rows.Count > 0) ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTab('/All Report/Report.aspx?for=DailyOverTimeReportByLine');", true);  //Open New Tab for Sever side code
                else
                {
                    lblMessage.InnerText = "warning->Overtime Not Available ";
                }

            }
            catch (Exception ex)
            {
                // lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private void Monthly_OverTimeGenerate(string dptName)
        {
            try
            {
                DataTable dtRunningEmp = new DataTable();

                SqlCommand cmd1 = new SqlCommand("delete from MonthlyOT", sqlDB.connection);
                cmd1.ExecuteNonQuery();
                
            try
            {
                DataTable AOT = new DataTable();
                sqlDB.fillDataTable("select AcceptableMinuteasOT from HRD_OthersSetting", AOT);
                ViewState["__AcceptableMinuteasOT__"] = AOT.Rows[0]["AcceptableMinuteasOT"].ToString();
            }
            catch { }
        
        
                string[] getMonthYear = ddlMonthName.SelectedItem.Text.Split('-');
                loadMonthlyLogoutTimeAndNormallyOTMin(getMonthYear[0], getMonthYear[1]);
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select Distinct MAX(SN) as SN, EmpId From v_tblAttendanceRecord Where EmpTypeId=" + 1 + " AND ActiveSalary='true' and IsActive=1  and MonthName='" +getMonthYear[1]+"-"+getMonthYear[0]+ "' and DptName " + dptName + " Group By EmpId ", dt);
               
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    OverTimeCalculation(dt.Rows[i]["EmpId"].ToString());
                }
               
            }
            catch { }
        }
        private void loadMonthlyLogoutTimeAndNormallyOTMin(string getMonth, string getYear)
        {
            try
            {

                sqlDB.fillDataTable("select convert(varchar(11),MonthDate,111) as MonthDate,LogoutHour,LogoutMin,NormallyOTHour,DecreaseOvertime,IsDecreaseOvertime from Attendance_MonthlyLogoutTimeAndOTSetting where MonthName='" + getMonth + '-' + getYear + "' AND Chosen='true'", dtMonthlyLogoutTimeAndNormallyOTMin);
            }
            catch { }

        }
        DataTable dtRunningEmp;
        DataTable dtCertainEmp;
        DataTable dtLeaveInfo;
        DataTable dtPresent;
        DataTable dtAbsent;
        DataTable dtLate;
        DataTable dtAdvanceInfo;
        DataTable dtCutAdvance;
        DataTable dtLoanInfo;
        DataTable dtCutLoan;
        DataTable dtStampDeduct;
        DataTable dtMonthlyLogoutTimeAndNormallyOTMin = new DataTable();
        double getOTRate;
        string getTotaOTHM;
        double TotalOTTaka;
        double TotalExtraOTTaka;
        string getTotalExtraOTHM;
        string getTotalOvertimeOfMonth;
        private void OverTimeCalculation(string EmpId)   // over time calculation 
        {
            try
            {
                TotalExtraOTTaka = 0;
                string getTotaOTHM = "0";
                getTotalExtraOTHM = "0";
                getTotalOvertimeOfMonth = "0";
                DataTable dtWeekindOverTime = new DataTable();
                double weekendOvertime = 0;
                getOTRate = 0;
                string[] getMonthYear = ddlMonthName.SelectedItem.Text.Split('-');
                DataTable dtdate = new DataTable();
                sqlDB.fillDataTable("Select Distinct ATTDate From v_tblAttendanceRecord where MonthName='" + getMonthYear[1] + "-" + getMonthYear[0] + "' and EmpId='" + EmpId + "'", dtdate);
               string  setDate = "";
               for (int i = 0; i < dtdate.Rows.Count; i++)
                {
                    if (i == 0 && i == dtdate.Rows.Count - 1)
                    {
                        setDate = "in('" + dtdate.Rows[i].ItemArray[0].ToString() + "')";
                        
                    }
                    else if (i == 0 && i != dtdate.Rows.Count - 1)
                    {
                        setDate += "in ('" + dtdate.Rows[i].ItemArray[0].ToString() + "'";
                        
                    }
                    else if (i != 0 && i == dtdate.Rows.Count - 1)
                    {
                        setDate += ",'" + dtdate.Rows[i].ItemArray[0].ToString() + "')";
                        
                    }
                    else
                    {
                        setDate += ",'" + dtdate.Rows[i].ItemArray[0].ToString() + "'";
                        
                    }
                }
                //------For Count Weekend Task of Employee-------------------
               sqlDB.fillDataTable("select InHour,InMin,OutHour,OutMin from tblAttendanceRecord where AttDate " + setDate + " And EmpId='" + EmpId + "' AND ATTStatus in ('W','H')", dtWeekindOverTime);

                for (byte r = 0; r < dtWeekindOverTime.Rows.Count; r++)
                {

                    string logInTime = dtWeekindOverTime.Rows[r]["InHour"].ToString() + ":" + dtWeekindOverTime.Rows[r]["InMin"].ToString();
                    string logOutTime = dtWeekindOverTime.Rows[r]["OutHour"].ToString() + ":" + dtWeekindOverTime.Rows[r]["OutMin"].ToString();

                    string dailyOT = (TimeSpan.Parse(logOutTime) - (TimeSpan.Parse(logInTime))).ToString();

                    // double temp = double.Parse(dailyOT);
                    string[] temp = dailyOT.Split(':');
                    if (int.Parse(temp[1]) >= int.Parse(ViewState["__AcceptableMinuteasOT__"].ToString()))
                    {
                        temp[0] = (int.Parse(temp[0]) + 1).ToString();
                    }
                    dailyOT = temp[0];


                    double takeTempOT = double.Parse(temp[0]);
                    weekendOvertime += Math.Round(takeTempOT, 0);
                }

                //----------------------End-----------------------------------
                sqlDB.fillDataTable("select InHour,InMin,OutHour,OutMin,Convert(varchar(11),ATTDate,111) as ATTDate from tblAttendanceRecord where AttDate " + setDate + " And EmpId='" + EmpId + "' AND ATTStatus in ('P','L')", dtPresent = new DataTable());

                for (byte b = 0; b < dtPresent.Rows.Count; b++)
                {
                    double getOutHour = double.Parse(dtPresent.Rows[b]["OutHour"].ToString());
                    string getOutMinuts;
                    if (dtPresent.Rows[b]["OutMin"].ToString().Length == 1) getOutMinuts = ".0" + double.Parse(dtPresent.Rows[b]["OutMin"].ToString());
                    else getOutMinuts = "." + double.Parse(dtPresent.Rows[b]["OutMin"].ToString());

                    if (b == 18)
                    {

                    }

                    getOutHour = getOutHour + double.Parse(getOutMinuts);


                    DataTable dtGetDailyLogOutTimeAndNOT = dtMonthlyLogoutTimeAndNormallyOTMin.Select("MonthDate='" + dtPresent.Rows[b]["ATTDate"].ToString() + "'").CopyToDataTable();

                    string DailyLogoutHour = (dtGetDailyLogOutTimeAndNOT.Rows[0]["LogoutHour"].ToString().Length == 1) ? "0" + dtGetDailyLogOutTimeAndNOT.Rows[0]["LogoutHour"].ToString() : dtGetDailyLogOutTimeAndNOT.Rows[0]["LogoutHour"].ToString();

                    string DailyLogoutMin = (dtGetDailyLogOutTimeAndNOT.Rows[0]["LogoutMin"].ToString().Length == 1) ? "0" + dtGetDailyLogOutTimeAndNOT.Rows[0]["LogoutMin"].ToString() : dtGetDailyLogOutTimeAndNOT.Rows[0]["LogoutMin"].ToString();

                    int DailyNormallyOTHour = int.Parse(dtGetDailyLogOutTimeAndNOT.Rows[0]["NormallyOTHour"].ToString());

                    string a = ViewState["__AcceptableMinuteasOT__"].ToString();
                    string forOTCountTime = (TimeSpan.Parse(DailyLogoutHour + ":" + DailyLogoutMin) + (TimeSpan.Parse("00" + ":" + ViewState["__AcceptableMinuteasOT__"].ToString()))).ToString();

                    string[] tempTimes = forOTCountTime.Split(':');

                    forOTCountTime = tempTimes[0] + "." + tempTimes[1];

                    double getDailyFixedOutTimeForOT = Math.Round(double.Parse(forOTCountTime), 2);

                    if (getOutHour >= getDailyFixedOutTimeForOT)
                    {
                        string logOutTime = dtPresent.Rows[b]["OutHour"].ToString() + ":" + dtPresent.Rows[b]["OutMin"].ToString();

                        string dailyOT = (TimeSpan.Parse(logOutTime) - (TimeSpan.Parse(DailyLogoutHour + ":" + DailyLogoutMin))).ToString();



                        string[] getDailyOT = dailyOT.Split(':');

                        if (int.Parse(getDailyOT[1]) >= int.Parse(ViewState["__AcceptableMinuteasOT__"].ToString()))
                        {
                            getDailyOT[0] = (int.Parse(getDailyOT[0]) + 1).ToString();
                        }
                        dailyOT = getDailyOT[0] + ":00";

                        if (int.Parse(getDailyOT[0]) >= DailyNormallyOTHour)
                        {
                            string tempOT = dailyOT;
                            dailyOT = (TimeSpan.Parse(dailyOT) - (TimeSpan.Parse("" + DailyNormallyOTHour + ":00"))).ToString();

                            if (DateTime.Parse(dailyOT) >= DateTime.Parse("00:" + ViewState["__AcceptableMinuteasOT__"].ToString() + ":00"))
                            {
                                getTotalExtraOTHM = (TimeSpan.Parse(dailyOT) + TimeSpan.Parse(getTotalExtraOTHM)).ToString();   // line for to get extra ot.extra ot means upper then 2
                            }

                            tempOT = (TimeSpan.Parse(tempOT) - TimeSpan.Parse(dailyOT)).ToString();
                            getTotaOTHM = (TimeSpan.Parse(tempOT) + TimeSpan.Parse(getTotaOTHM)).ToString();               // line for to get normaly ot. lower then 2 hours
                        }
                        else
                        {
                            getTotaOTHM = (TimeSpan.Parse(dailyOT) + TimeSpan.Parse(getTotaOTHM)).ToString();
                        }

                    }
                }

                string[] getOTHoureMinuts = getTotaOTHM.Split(':');

                //  bool isTotalDecreaseOvertimeFromNormalOT = false;   // status for konwn already cut decrease total overtime  ?
                bool isPartialDecreaseOvertimeCutFromNormalOT = false;  // status for  cut partial decrease overtime 
                //  bool isTotalDecreaseOvertimeFromWeekendOT = false;
                bool isPartialDecreaseOvertimeFromWeekendOT = false;
                string getDueOT = "0";

                if (!getTotaOTHM.Length.Equals(1) && !getTotaOTHM.Equals("0"))
                {
                    string OTH = getOTHoureMinuts[0];

                    if (OTH.ToString().Contains("."))
                    {
                        string[] getHours = OTH.Split('.');
                        OTH = TimeSpan.FromDays(int.Parse(getHours[0])).TotalHours.ToString();
                        OTH = (int.Parse(OTH) + int.Parse(getHours[1])).ToString();

                    }
                    string OTM = "." + getOTHoureMinuts[1];
                    //if (OTM != ".00" || OTM != ".0") 
                    //    if (double.Parse(OTM)>.59)OTM = "1";
                    getTotaOTHM = (double.Parse(OTH) + double.Parse(OTM)).ToString();


                    getTotaOTHM = Math.Round(double.Parse(getTotaOTHM), 0).ToString();

                    //-----------------------------For Checking and cutting overall overtime from everyone and from weedendOvertime and Normal overtime ------------------------------------------------------

                    if (bool.Parse(dtMonthlyLogoutTimeAndNormallyOTMin.Rows[0]["IsDecreaseOvertime"].ToString()).Equals(true))
                    {
                        if (weekendOvertime >= float.Parse(dtMonthlyLogoutTimeAndNormallyOTMin.Rows[0]["DecreaseOvertime"].ToString()))
                        {
                            weekendOvertime = weekendOvertime - float.Parse(dtMonthlyLogoutTimeAndNormallyOTMin.Rows[0]["DecreaseOvertime"].ToString());
                            // isTotalDecreaseOvertimeFromWeekendOT = true;
                        }
                        else
                        {
                            getDueOT = (double.Parse(dtMonthlyLogoutTimeAndNormallyOTMin.Rows[0]["DecreaseOvertime"].ToString()) - weekendOvertime).ToString();
                            isPartialDecreaseOvertimeFromWeekendOT = true;
                        }

                        if (isPartialDecreaseOvertimeFromWeekendOT)    // test after 
                        {
                            if (float.Parse(getTotaOTHM) >= float.Parse(getDueOT))
                            {
                                getTotaOTHM = (float.Parse(getTotaOTHM) - float.Parse(getDueOT)).ToString();
                                // isTotalDecreaseOvertimeFromNormalOT = true;
                            }
                            else
                            {
                                getDueOT = (float.Parse(getDueOT) - float.Parse(getTotaOTHM)).ToString();
                                getTotaOTHM = "0";
                                isPartialDecreaseOvertimeCutFromNormalOT = true;
                            }
                        }

                    }
                }

                //-------------------------------------------End-----------------------------------------------------------------------------------

               
               
                ViewState["__getTotaOTHM__"] = getTotaOTHM;


                string[] getTotalExtraOTHMContainDays = getTotalExtraOTHM.Split(':');
                if (getTotalExtraOTHMContainDays[0].ToString().Contains('.'))
                {
                    string[] getHours = getTotalExtraOTHMContainDays[0].ToString().Split('.');
                    double Hours = TimeSpan.FromDays(int.Parse(getHours[0])).TotalHours;
                    Hours = Hours + int.Parse(getHours[1]);

                    getTotalExtraOTHM = Hours.ToString();

                    getTotalExtraOTHM = Hours + "." + getTotalExtraOTHMContainDays[1];

                }
                else
                {
                    string[] getExtraOverTime = getTotalExtraOTHM.Split(':');

                    if (getExtraOverTime.Length > 1)
                        getTotalExtraOTHM = getExtraOverTime[0] + "." + getExtraOverTime[1];

                    else getTotalExtraOTHM = getExtraOverTime[0];
                }

                string[] tempValue = getTotalExtraOTHM.Split('.');
                // if (tempValue[1] != ".00" || tempValue[1] != ".0") tempValue[1] = "1";

                getTotalExtraOTHM = tempValue[0];
                getTotalExtraOTHM = Math.Round(double.Parse(getTotalExtraOTHM), 0).ToString();


                //----------------------------For checking and cutting decrease overtime from extra overtime -----------------------------

                if (bool.Parse(dtMonthlyLogoutTimeAndNormallyOTMin.Rows[0]["IsDecreaseOvertime"].ToString()).Equals(true))
                {
                    if (isPartialDecreaseOvertimeCutFromNormalOT)
                    {
                        getTotalExtraOTHM = (float.Parse(getTotalExtraOTHM) - float.Parse(getDueOT)).ToString();
                        if (float.Parse(getTotalExtraOTHM) <= 0) getTotalExtraOTHM = "0";
                    }
                }

                //------------------------------------End Checking And Cutting--------------------------------------------------------------



                getTotalExtraOTHM = (double.Parse(getTotalExtraOTHM) + weekendOvertime).ToString();

                

                getTotalOvertimeOfMonth = (int.Parse(getTotalExtraOTHM) + int.Parse(getTotaOTHM)).ToString();


                SqlCommand cmd = new SqlCommand("Insert Into MonthlyOT values(@EmpId,@OT1,@OT2)", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpId",EmpId);
                cmd.Parameters.AddWithValue("@OT1", getTotaOTHM);
                cmd.Parameters.AddWithValue("@OT2", getTotalExtraOTHM);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveItem(lstAll, lstSelected);
        }

        protected void btnAddAllItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveAll(lstAll, lstSelected);
        }

        protected void btnRemoveItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveItem(lstSelected, lstAll);
        }

        protected void btnRemoveAllItem_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.commonTask.AddRemoveAll(lstSelected, lstAll);
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            CompanyId = (ddlCompany.SelectedValue == "0000") ? ViewState["__CompanyId__"].ToString() : ddlCompany.SelectedValue;
            classes.commonTask.LoadShiftNameByCompany(CompanyId, ddlShift);
            classes.commonTask.LoadMonthName(CompanyId, ddlMonthName);
            classes.commonTask.LoadDepartment(CompanyId, lstAll);
            lstSelected.Items.Clear();
        }

    }
}