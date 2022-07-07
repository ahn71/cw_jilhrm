using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//using BG.Common;

using System.Data;
using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System.Data.SqlClient;

namespace SigmaERP.attendance
{
    public partial class dep_wise_attendance : System.Web.UI.Page
    {


        DataTable dt; SqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {

            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            txtToDate.Enabled = false;
           
            if (!IsPostBack)
            {
               // setPrivilege();
                classes.commonTask.loadEmpTypeInRadioButtonList(rbEmpType);
                classes.commonTask.loadDivision(ddlDivision);
               // LoadGrid();
                rbEmpType.SelectedIndex = 0;
                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            }
        }

        void LoadGrid()
        {
            try
            {
                string strSQL = @"select AttId, MonthId, EmpCardNo, EmpName, AttenStatus,
                                    DateIn, DateOut, TimeInHr, TimeInMin, TimeOutForLunchHr,
                                    TimeOutForLunchMin, TimeInAfterLunchHr, TimeInAfterLunchMin,
                                    TimeOutHr, TimeOutMin  from dbo.tblAttendance";
                DataTable DTLocal = new DataTable();

                sqlDB.fillDataTable(strSQL, DTLocal);

                gvAttendance.DataSource = DTLocal;
                gvAttendance.DataBind();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        protected void rbAttendanceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbAttendanceList.SelectedItem.ToString().Equals("Current Date"))
                {
                    txtToDate.Text = "";
                    txtFromDate.Enabled = false;
                    lblMessage.InnerText = "";
                }
                else
                {

                    txtToDate.Text = "";
                    txtToDate.Enabled = true;
                    lblMessage.InnerText = "";
                }
            }
            catch { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (inputValidationBasket())
            {
                saveManualAttendance();
                if (ViewState["__status__"].ToString().Equals("1"))
                {
                    if (rbAttendanceList.SelectedValue.ToString() == "1") lblMessage.InnerText = "success->Successfully department wise  attendance count from " + txtFromDate.Text.Trim() + "to " + txtToDate.Text.Trim();
                    else lblMessage.InnerText = "success->Successfully department wise  attendance count of " + txtFromDate.Text.Trim();
                }
            }
        }

        private bool inputValidationBasket()
        {
            try
            {
                if (txtInTimeHr.Text.Trim().Equals("0") || txtInTimeHr.Text.Trim().Equals("00"))
                {
                    lblMessage.InnerText = "warning->Please set login hour";
                    txtInTimeHr.Focus();
                    return false;
                }

                else if (txtTimeOutHr.Text.Trim().Equals("0") || txtTimeOutHr.Text.Trim().Equals("00"))
                {
                    lblMessage.InnerText = "warning->Please set logout hour";
                    txtTimeOutHr.Focus();
                    return false;
                }
                return true;
            }
            catch { return false; }
        }
        DataTable dtLoadEmpId;
        private void saveManualAttendance()
        {
            try
            {
                DateTime end; 
                DateTime begin=Convert.ToDateTime(DateTime.Now.ToString());
                dtLoadEmpId=new DataTable ();
                string getToDateDBFormat="";
                string getFromDateDBFormat = "";

                string [] getFromDate = txtFromDate.Text.Trim().Split('-');
                string[] getToDate = txtToDate.Text.Trim().Split('-');

                begin = new DateTime(int.Parse(getFromDate[2]), int.Parse(getFromDate[1]), int.Parse(getFromDate[0]));

                if (rbAttendanceList.SelectedValue.ToString().Equals("0"))     //0=Current Date
                {
                    
                    end = new DateTime(int.Parse(getFromDate[2]), int.Parse(getFromDate[1]), int.Parse(getFromDate[0]));
                    getToDateDBFormat = getFromDate[2] + "-" + getFromDate[1] + "-" + getFromDate[0];
                }
                else
                {
                    end = new DateTime(int.Parse(getToDate[2]), int.Parse(getToDate[1]), int.Parse(getToDate[0]));
                    getToDateDBFormat = getToDate[2] + "-" + getToDate[1] + "-" + getToDate[0];
                }

                sqlDB.fillDataTable("select EmpId,EmpCardNo,Format(EmpJoiningDate,'dd-MM-yyyy') as EmpJoiningDate from v_Personnel_EmpCurrentStatus where EmpStatus in ('1','8') and IsActive=1 AND DptId='" + ddlDepartment.SelectedValue.ToString() + "' AND EmpTypeId=" + rbEmpType.SelectedValue.ToString() + " group by EMpId,EmpCardNo,EmpJoiningDate", dtLoadEmpId);

                while (begin <= end)
                {
                    string[] setFormDate = begin.ToString().Split('/');

                    setFormDate[0] = (setFormDate[0].Length == 1) ? "0" + setFormDate[0] : setFormDate[0];
                    setFormDate[1] = (setFormDate[1].Length == 1) ? "0" + setFormDate[1] : setFormDate[1];

                    getFromDateDBFormat = setFormDate[2].Substring(0, 4) + "-" + setFormDate[0] + "-" + setFormDate[1];

                    string getNormalFormat = setFormDate[1] + "-" + setFormDate[0] + "-" + setFormDate[2].Substring(0, 4);


                    for (int i = 0; i < dtLoadEmpId.Rows.Count; i++)
                    {
                        if (!CompareJoinDateAndIndate(i)) break;  // check joindate and attendance date
                        else i = k;

                        string stateStatus = "";
                        string attStatus = "";
                        string DailyStartTimeALT = "00-00-00-00";
                        sqlDB.fillDataTable("select * from v_Leave_LeaveApplication where FromDate <='" + getFromDateDBFormat + "' AND ToDate >='" + getToDateDBFormat + "' And EmpId='" +dtLoadEmpId.Rows[0]["EmpId"].ToString() + "'", dt = new DataTable());
                        string strTimeCompare = @"select SftStartTime,SftAcceptableLate from [dbo].[HRD_Shift] ";


                        DataTable DTTimeCompare = new DataTable();
                        sqlDB.fillDataTable(strTimeCompare, DTTimeCompare);

                        int CH = Convert.ToDateTime(DTTimeCompare.Rows[0]["SftStartTime"].ToString()).Hour; // for get real hour

                        int CMin = Convert.ToDateTime(DTTimeCompare.Rows[0]["SftStartTime"].ToString()).Minute;  // for get real munite
                        int CM = Convert.ToInt32(DTTimeCompare.Rows[0]["SftAcceptableLate"].ToString());    // for get acceptable minute
                        int CS = Convert.ToDateTime(DTTimeCompare.Rows[0]["SftStartTime"].ToString()).Second;  // for get real second

                        if (dt.Rows.Count > 0)
                        {

                            attStatus = "LV";
                            stateStatus = dt.Rows[0]["LeaveName"].ToString();
                        }
                        else if (dt.Rows.Count == 0)
                        {
                            sqlDB.fillDataTable("select * from tblHolydayWork where HDate='" + getFromDateDBFormat + "'", dt = new DataTable());
                            if (dt.Rows.Count > 0)
                            {

                                attStatus = "H";
                                stateStatus = "";
                            }
                            else
                            {
                                sqlDB.fillDataTable("select * from Attendance_WeekendInfo where WeekendDate='" + getFromDateDBFormat + "'", dt = new DataTable());
                                if (dt.Rows.Count > 0)
                                {
                                    attStatus = "W";
                                    stateStatus = "";
                                }

                                else
                                {
                                    if (ddlAttenStatus.SelectedItem.Text.Contains("Leave"))
                                    {
                                        if (ddlAttenStatus.SelectedItem.Text.Equals("Casula Leave")) stateStatus = "C/L";
                                        else if (ddlAttenStatus.SelectedItem.Text.Equals("Sick Leave")) stateStatus = "S/L";
                                        else if (ddlAttenStatus.SelectedItem.Text.Equals("Maternity Leave")) stateStatus = "M/L";
                                        attStatus = "LV";

                                    }
                                    else if (ddlAttenStatus.SelectedItem.Text.Contains("Present"))
                                    {
                                        stateStatus = "";
                                        attStatus = "P";
                                        DailyStartTimeALT = CH + "-" + CMin + "-" + CS + "-" + CM;
                                    }
                                    else if (ddlAttenStatus.SelectedItem.Text.Contains("Absent"))
                                    {
                                        stateStatus = "";
                                        attStatus = "A";

                                    }
                                    else if (ddlAttenStatus.SelectedItem.Text.Contains("Late"))
                                    {
                                        stateStatus = "";
                                        attStatus = "L";
                                        DailyStartTimeALT = CH + "-" + CMin + "-" + CS + "-" + CM;
                                    }

                                    else if (ddlAttenStatus.SelectedItem.Text.Contains("Weekend"))
                                    {
                                        stateStatus = "";
                                        attStatus = "W";
                                    }

                                    else if (ddlAttenStatus.SelectedItem.Text.Contains("Holiday"))
                                    {
                                        stateStatus = "";
                                        attStatus = "H";
                                    }

                                }

                            }
                        }



                        DataTable dtGetProximityNo;
                        string proximityNo = "";
                        sqlDB.fillDataTable("select EmpProximityNo from Personnel_EmployeeInfo where EmpId=(select Distinct EmpId from Personnel_EmpCurrentStatus where EmpCardNo='" + dtLoadEmpId.Rows[i]["EmpCardNo"].ToString() + "' AND EmpTypeId=" + rbEmpType.SelectedValue.ToString() + ")", dtGetProximityNo = new DataTable());
                        if (dtGetProximityNo.Rows.Count > 0) proximityNo = dtGetProximityNo.Rows[0]["EmpProximityNo"].ToString();

                        try
                        {
                            cmd = new SqlCommand("delete from tblAttendanceRecord where ATTDate='" + getFromDateDBFormat + "' AND EmpId=(select Distinct EmpId from Personnel_EmpCurrentStatus where EmpCardNo='" + dtLoadEmpId.Rows[i]["EmpCardNo"].ToString() + "' AND EmpTypeId=" + rbEmpType.SelectedValue.ToString() + ")", sqlDB.connection);
                            cmd.ExecuteNonQuery();
                        }
                        catch { }

                        txtInTimeHr.Text = (txtInTimeHr.Text.Trim().Length == 1) ? "0" + txtInTimeHr.Text : txtInTimeHr.Text;
                        txtInTimeMin.Text = (txtInTimeMin.Text.Trim().Length == 1) ? "0" + txtInTimeMin.Text : txtInTimeMin.Text;

                        txtOutForLunchHr.Text = (txtOutForLunchHr.Text.Trim().Length == 1) ? "0" + txtOutForLunchHr.Text : txtOutForLunchHr.Text;
                        txtOutForLunchMin.Text = (txtOutForLunchMin.Text.Trim().Length == 1) ? "0" + txtOutForLunchMin.Text : txtOutForLunchMin.Text;

                        txtInAfterLunchHr.Text = (txtInAfterLunchHr.Text.Trim().Length == 1) ? "0" + txtInAfterLunchHr.Text : txtInAfterLunchHr.Text;
                        txtInAfterLunchMin.Text = (txtInAfterLunchMin.Text.Trim().Length == 1) ? "0" + txtInAfterLunchMin.Text : txtInAfterLunchMin.Text;


                        txtTimeOutHr.Text = (txtTimeOutHr.Text.Trim().Length == 1) ? "0" + txtTimeOutHr.Text : txtTimeOutHr.Text;
                        txtTimeOutMin.Text = (txtTimeOutMin.Text.Trim().Length == 1) ? "0" + txtTimeOutMin.Text : txtTimeOutMin.Text;

                        string InSec = "00";
                        string OutSec = "00";


                        string[] getColumns = { "EmpProximityNo", "EmpCardNo", "ATTDate", "InHour", "InMin", "InSec", "OutHour", "OutMin", "OutSec", "BreakStartHour", "BreakStartMin", "BreakEndHour", "BreakEndMin", "ATTStatus", "StateStatus", "Remarks", "EmpTypeId", "AttManual", "EmpId", "DailyStartTimeALT" };
                        string[] getValues = { proximityNo, dtLoadEmpId.Rows[i]["EmpCardNo"].ToString(), convertDateTime.getCertainCulture(getNormalFormat).ToString(), txtInTimeHr.Text.Trim(), txtInTimeMin.Text.Trim(), InSec, txtTimeOutHr.Text.Trim(), txtTimeOutMin.Text.Trim(), OutSec, txtOutForLunchHr.Text.Trim(), txtOutForLunchMin.Text.Trim(), txtInAfterLunchHr.Text.Trim(), txtInAfterLunchMin.Text.Trim(), attStatus, stateStatus, " ", rbEmpType.SelectedValue.ToString(), "Manual Attendance",dtLoadEmpId.Rows[i]["EmpId"].ToString(), DailyStartTimeALT };
                        if (SQLOperation.forSaveValue("tblAttendanceRecord", getColumns, getValues, sqlDB.connection) == true)
                        {
                            ViewState["__status__"] = "1";
                        }
                    }

                    begin=begin.AddDays(1);
                }

               

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
        
        }

        int k;
        bool Datestatus = false;
        private bool CompareJoinDateAndIndate(int i)
        {
            try
            {
                Datestatus = false;
                k = i;
                DateTime InDate = new DateTime(int.Parse(txtFromDate.Text.Trim().Substring(6, 4)), int.Parse(txtFromDate.Text.Trim().Substring(3, 2)), int.Parse(txtFromDate.Text.Trim().Substring(0,2)));
                DateTime JoinDate = new DateTime(int.Parse(dtLoadEmpId.Rows[i]["EmpJoiningDate"].ToString().Substring(6, 4)), int.Parse(dtLoadEmpId.Rows[i]["EmpJoiningDate"].ToString().Substring(3, 2)), int.Parse(dtLoadEmpId.Rows[i]["EmpJoiningDate"].ToString().Substring(0, 2)));
                if (InDate >= JoinDate)
                {
                    return true;
                }
                else 
                {
                    i++;
                    if (i < dtLoadEmpId.Rows.Count)
                    {
                        CompareJoinDateAndIndate(i);
                    }
                    if (Datestatus) return true;
                    else return false;
                }
            }
            catch { return false; }
        }

        protected void ddlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlDepartment.Items.Clear();
            LoadDepartment(ddlDivision.SelectedValue.ToString());
        }

        private void LoadDepartment(string divisionId)
        {
            try
            {
                dt = new DataTable();
                
                sqlDB.fillDataTable("SELECT DptId, DptName FROM HRD_Department where DId=" + divisionId + "", dt);

                ddlDepartment.DataValueField = "DptId";
                ddlDepartment.DataTextField = "DptName";
                ddlDepartment.DataSource = dt;
                ddlDepartment.DataBind();
                ddlDepartment.Items.Insert(0, new ListItem(" ", " "));
            }
            catch { }
        }
    }
}