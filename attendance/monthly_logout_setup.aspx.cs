using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.attendance
{
    public partial class monthly_logout_setup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            if (!IsPostBack)
            {
                setPrivilege();
                loadMonthName();
            }
        }

        private void setPrivilege()
        {
            try
            {
                ViewState["__WriteAction__"] = "1";
                ViewState["__DeletAction__"] = "1";
                ViewState["__ReadAction__"] = "1";
                ViewState["__UpdateAction__"] = "1";

                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                if (getCookies["__getUserType__"].ToString().Equals("Super Admin")) return;
                else
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='monthly_logout_setup.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false))
                        {
                            ViewState["__ReadAction__"] = "0";
                            dlSelectMonth.Enabled = false;
                        }
                        if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                        {
                            ViewState["__WriteAction__"] = "0";
                           
                            btnSet.Enabled = false;
                        }
                        if (bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false))
                        {
                            ViewState["__UpdateAction__"] = "0";
                        }
                        if (bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                        {
                            ViewState["__DeletAction__"] = "0";
                            btnDelete.Enabled = false;
                        }
                    }

                }

            }
            catch { }

        }

        private void loadMonthName()
        {
            try
            {

                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select MonthName from tblMonthSetup ", dt);

                dlSelectMonth.DataValueField = "MonthName";
                dlSelectMonth.DataValueField = "MonthName";
                dlSelectMonth.DataSource = dt;
                dlSelectMonth.DataBind();
                dlSelectMonth.Items.Insert(0, new ListItem(" ", "00"));

            }
            catch { }

        }


        protected void dlSelectMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMessage.InnerText = "";
                if (hasEntryRecord() == true) return;

                string getMonth = dlSelectMonth.SelectedItem.ToString().Substring(3, 4) + "-" + dlSelectMonth.SelectedItem.ToString().Substring(0, 2);
                string getMonth2 = dlSelectMonth.SelectedItem.ToString().Substring(3, 4) + "/" + dlSelectMonth.SelectedItem.ToString().Substring(0, 2);

                DataTable dt=new DataTable ();
                sqlDB.fillDataTable("select convert(varchar(11),WeekendDate,111) as WeekendDate from Attendance_WeekendInfo where MonthName='" + dlSelectMonth.SelectedItem.ToString() + "'", dt);
                DataTable dtHD = new DataTable();
                sqlDB.fillDataTable("select convert(varchar(11),HDate,111) as HDate from tblHolydayWork where HDate='" + getMonth + "'", dtHD);
                
                
                
                int getDaysInMonth = DateTime.DaysInMonth(int.Parse(dlSelectMonth.SelectedItem.ToString().Substring(3, 4)), int.Parse(dlSelectMonth.SelectedItem.ToString().Substring(0, 2)));

                DataTable dtDays = new DataTable();

                dtDays.Columns.Add("MonthDate", typeof(string));
                dtDays.Columns.Add("LogoutHour", typeof(string));
                dtDays.Columns.Add("LogoutMin", typeof(string));
                dtDays.Columns.Add("NormallyOTHour", typeof(string));
                dtDays.Columns.Add("SL", typeof(string));
                dtDays.Columns.Add("Chosen",typeof(bool));
                for (byte b = 1; b <getDaysInMonth+1; b++)
                {
                 if(b.ToString().Length==1)   dtDays.Rows.Add("0"+b+"-"+dlSelectMonth.SelectedItem.ToString(),"17","00","2",b,true);
                 else  dtDays.Rows.Add(b + "-" + dlSelectMonth.SelectedItem.ToString(),"17","00","2",b,true);
                
                }
                chkDecreace.Checked = false;
                txtDecreaseAmount.Text = "00";
                lblStatus.Text = "Unsetuped Month Info";
                gvDateList.DataSource = dtDays;
                gvDateList.DataBind();
            }
            catch { }

        }

        private bool hasEntryRecord()
        {
            try
            {
                DataTable dt=new DataTable ();
                sqlDB.fillDataTable("select SL,Convert(varchar(11),MonthDate,105)as MonthDate,Chosen,LogoutHour,LogoutMin,NormallyOTHour,MonthName,DecreaseOvertime from Attendance_MonthlyLogoutTimeAndOTSetting where MonthName='" + dlSelectMonth.SelectedItem.ToString() + "'", dt);
                if (dt.Rows.Count > 0)
                {
                    lblStatus.Text = "Setuped Month Info";
                    gvDateList.DataSource = dt;
                    gvDateList.DataBind();
                    txtDecreaseAmount.Text = dt.Rows[0]["DecreaseOvertime"].ToString();
                    if (txtDecreaseAmount.Text != "0") chkDecreace.Checked = true;
                    return true;
                }
                else return false;
            }
            catch { return false; }
        
        }

        protected void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                DeletePreviousSetup();
                string getDecreaceOvertime="0";
                byte getIsDecreaseOvertime =(byte) 0;
                if (chkDecreace.Checked)
                {
                    if (txtDecreaseAmount.Text.Trim().Length == 0) getDecreaceOvertime = "0";
                    else getDecreaceOvertime = txtDecreaseAmount.Text;
                    getIsDecreaseOvertime = (byte)1;

                }
                else getDecreaceOvertime = "0";
                for (byte b = 0; b < gvDateList.Rows.Count; b++)
                {

                    TextBox txtLogoutHour = (TextBox)gvDateList.Rows[b].Cells[2].FindControl("txtLogoutHour");
                    TextBox txtLogoutMin = (TextBox)gvDateList.Rows[b].Cells[3].FindControl("txtLogoutMin");
                    TextBox txtNormallyOTHour = (TextBox)gvDateList.Rows[b].Cells[4].FindControl("txtNormallyOTHour");

                    CheckBox chk = new CheckBox();
                    chk = (CheckBox)gvDateList.Rows[b].Cells[1].FindControl("chkChosen");
                    try
                    {

                        byte Chosen = (chk.Checked) ? (byte)1 : (byte)0;
                        string[] getColumns = { "MonthDate", "Chosen", "LogoutHour", "LogoutMin", "NormallyOTHour", "MonthName", "DecreaseOvertime", "IsDecreaseOvertime" };
                        string[] getValues = { convertDateTime.getCertainCulture(gvDateList.Rows[b].Cells[0].Text).ToString(), Chosen.ToString(), txtLogoutHour.Text, txtLogoutMin.Text, txtNormallyOTHour.Text, dlSelectMonth.SelectedItem.ToString(), getDecreaceOvertime,getIsDecreaseOvertime.ToString()};
                        SQLOperation.forSaveValue("Attendance_MonthlyLogoutTimeAndOTSetting", getColumns, getValues, sqlDB.connection);
                      
                    }
                    catch (Exception ex)
                    {
                       
                    }
                }

                txtDecreaseAmount.Text = "00";
                chkDecreace.Checked =false;
                lblMessage.InnerText = "success->Successfully Monthly Logout Time And OT Setting Setup";
            }
            catch { }
        }

        private void DeletePreviousSetup()
        {
            try
            {
                SQLOperation.forDeleteRecordByIdentifier("Attendance_MonthlyLogoutTimeAndOTSetting", "MonthName", dlSelectMonth.SelectedItem.ToString(), sqlDB.connection);
            }
            catch { }
        
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeletePreviousSetup();
            gvDateList.DataSource = null;
            gvDateList.DataBind();
            lblStatus.Text = "";
        }
    }


}