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
using System.Drawing;
using SigmaERP.classes;

namespace SigmaERP.attendance
{
    public partial class monthly_setup : System.Web.UI.Page
    {
        string strSQL = "";

        SqlCommand cmd;
       

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();

        
            if (!IsPostBack)
            {
                ViewState["__preRIndex__"] = "No";
                ViewState["__IsCalculated__"] = "No";
                setPrivilege();
                LoadGrid();
                if (!classes.commonTask.HasBranch())
                ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
            }
        }

        static DataTable  dtSetprivilege;
        private void setPrivilege()
        {
            try
            {
               
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();

                ViewState["__getUserId__"] = getUserId;
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();

         
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "monthly_setup.aspx", ddlCompanyList, gvMonthSetup, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
               

            }
            catch { }

        }

        private void saveMonthSetup()
        {
            try
            {
                string[] fd = txtFromDate.Text.Trim().Split('-');
                string[] td = txtToDate.Text.Trim().Split('-');
                string[] epd = txtExpectedPaymetnDate.Text.Trim().Split('-');
                byte isActive = (ddlMonthStatus.SelectedItem.Text.ToString().Equals("Active")) ? (byte)1 : (byte)0;
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

                string[] getColumns = { "MonthName", "FromDate", "ToDate", "TotalDays", "TotalWeekend", "TotalHoliday", "TotalWorkingDays", "ExpectedPaymentDate", "MonthStatus", "CompanyId", "UserId" };
                string[] getValues = { txtMonthName.Text.Trim(), fd[2]+"-"+fd[1]+"-"+fd[0],
                                       td[2]+"-"+td[1]+"-"+td[0], txtTotalNOofDay.Text.Trim(), 
                                       txtTotalWeekend.Text.Trim(), txtTotalHoliday.Text.Trim(), txtTotalWorkingDays.Text.Trim(),
                                       epd[2]+"-"+epd[1]+"-"+epd[0], isActive.ToString(), 
                                       CompanyId,ViewState["__getUserId__"].ToString()};
                if (SQLOperation.forSaveValue("tblMonthSetup", getColumns, getValues, sqlDB.connection) == true)
                {
                    saveAttendance_WeekendInfo();
                    lblMessage.InnerText = "success->Successfully month setup saved";
                    LoadGrid(); Clear();
                }
            }
            catch (SqlException sex)
            {
                lblMessage.InnerText = "error->"+txtMonthName.Text+" month alredy setuped ";
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;

            }
        }

        private void updateMonthSetup()
        {
            try
            {
                string[] fd = txtFromDate.Text.Trim().Split('-');
                string[] td = txtToDate.Text.Trim().Split('-');
                string[] epd = txtExpectedPaymetnDate.Text.Trim().Split('-');
                byte isActive = (ddlMonthStatus.SelectedItem.Text.ToString().Equals("Active")) ? (byte)1 : (byte)0;
                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

                string[] getColumns = { "FromDate", "ToDate", "TotalDays", "TotalWeekend", "TotalHoliday", "TotalWorkingDays", "ExpectedPaymentDate", "MonthStatus", "CompanyId","UserId" };
                string[] getValues = { fd[2]+"-"+fd[1]+"-"+fd[0],
                                         td[2]+"-"+td[1]+"-"+td[0], txtTotalNOofDay.Text.Trim(),
                                         txtTotalWeekend.Text.Trim(), txtTotalHoliday.Text.Trim(), txtTotalWorkingDays.Text.Trim(),
                                         epd[2]+"-"+epd[1]+"-"+epd[0], isActive.ToString(),
                                         CompanyId,ViewState["__getUserId__"].ToString() };

                if (SQLOperation.forUpdateValue("tblMonthSetup", getColumns, getValues, "MonthId", ViewState["__hid__"].ToString(), sqlDB.connection) == true)
                {
                    saveAttendance_WeekendInfo();
                    lblMessage.InnerText = "success->Successfully month setup updated";
                    LoadGrid(); Clear();
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message);
            }
        }

        private void saveAttendance_WeekendInfo()
        {
            DataTable dt;
            string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
            sqlDB.fillDataTable("select MonthId from tblMonthSetup where MonthName='" + txtMonthName.Text.Trim() + "' And CompanyId='" + CompanyId + "'", dt = new DataTable());
            try
            {
                SQLOperation.forDeleteRecordByIdentifier("Attendance_WeekendInfo", "MonthID", dt.Rows[0]["MonthId"].ToString(), sqlDB.connection);
            }
            catch { }

            for (byte b = 0; b < gvWeekendDate.Rows.Count; b++)
            {
                CheckBox chk = new CheckBox();
                chk = (CheckBox)gvWeekendDate.Rows[b].Cells[1].FindControl("SelectCheckBox");
                try
                {
                    if (chk.Checked)
                    {
                        string[] wd = gvWeekendDate.Rows[b].Cells[0].Text.ToString().Split('-');
                        string[] getColumns = { "Weekend", "WeekendDate", "Reason", "CompanyId", "MonthId" };
                        string[] getValues = {ViewState["__Weekend__"].ToString(),
                                              wd[2]+"-"+wd[1]+"-"+wd[0],"Weekly Holiday",
                                              CompanyId,dt.Rows[0]["MonthId"].ToString()};

                        SQLOperation.forSaveValue("Attendance_WeekendInfo", getColumns, getValues, sqlDB.connection);
                        
                    }
                }
                catch (Exception ex)
                {
                    //  MessageBox.Show(ex.Message);
                }
            }
        }

       

        
        void Clear()
        {
            try
            {
                lblMessage.InnerText = "";
                txtMonthName.Enabled = true;
                txtMonthName.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtTotalNOofDay.Text = "";
                txtTotalWeekend.Text = "";
                txtTotalHoliday.Text = "";
                txtTotalWorkingDays.Text = "";
                txtExpectedPaymetnDate.Text = "";

                if (ViewState["__WriteAction__"].Equals("0"))
                {
                    btnSave.Enabled = false;
                    btnSave.CssClass = "";
                }
                else
                {
                    btnSave.Enabled = true;
                    btnSave.CssClass = "Mbutton";
                }
                ddlMonthStatus.SelectedIndex = -1;
                btnSave.Text = "Save";
                ViewState["__IsCalculated__"] = "No";
                //ddlCompanyList.SelectedIndex = 0;
                gvMonthSetup.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
            }
            catch { }
        }

        void LoadGrid()
        {
            string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();
            string strSQL = @"select MonthID, MonthName, convert(varchar(11), [FromDate],106) as FromDate,convert(varchar(11), [ToDate],106) as ToDate,[TotalDays],
                                [TotalWeekend],[TotalHoliday],[TotalWorkingDays], convert(varchar(11), [ExpectedPaymentDate],106) 
                                as[ExpectedPaymentDate] from [dbo].[tblMonthSetup] where CompanyId='" + CompanyId + "' order by MonthID desc";
            DataTable DTLocal = new DataTable();

            sqlDB.fillDataTable(strSQL, DTLocal);

            gvMonthSetup.DataSource = DTLocal;
            gvMonthSetup.DataBind();
        }

        void Delete(int id)
        {
            string strSql = "delete from [dbo].[tblMonthSetup] where [MonthID]=" + id + "";
            cmd = new SqlCommand(strSql,sqlDB.connection);
            cmd.ExecuteNonQuery();
            hdMonthSetup.Value = "";
            btnSave.Text = "Save";


        }

        private void SetValueToControl(string hid)
        {
            try
            {
                string strSQL = @"select MonthID, MonthName, FromDate, ToDate, TotalDays, TotalWeekend, 
                                TotalHoliday, TotalWorkingDays, ExpectedPaymentDate, 
                                MonthStatus,CompanyId from dbo.tblMonthSetup where MonthID='" + hid + "'";
                DataTable DTLocal = new DataTable();
               // ConManager.DBConnection("local");
               // ConManager.OpenDataTableThroughAdapter(strSQL, out DTLocal, true);
                sqlDB.fillDataTable(strSQL,DTLocal);
                hdMonthSetup.Value = DTLocal.Rows[0]["MonthID"].ToString();

                txtMonthName.Text = DTLocal.Rows[0]["MonthName"].ToString();
                txtFromDate.Text = Convert.ToDateTime(DTLocal.Rows[0]["FromDate"]).ToString("dd-MM-yyyy");
                txtToDate.Text = Convert.ToDateTime(DTLocal.Rows[0]["ToDate"]).ToString("dd-MM-yyyy");
                txtTotalNOofDay.Text = DTLocal.Rows[0]["TotalDays"].ToString();
                txtTotalWeekend.Text = DTLocal.Rows[0]["TotalWeekend"].ToString();
                txtTotalHoliday.Text = DTLocal.Rows[0]["TotalHoliday"].ToString();
                txtTotalWorkingDays.Text = DTLocal.Rows[0]["TotalWorkingDays"].ToString();
                txtExpectedPaymetnDate.Text = Convert.ToDateTime(DTLocal.Rows[0]["ExpectedPaymentDate"]).ToString("dd-MM-yyyy");
                ddlMonthStatus.SelectedValue=(bool.Parse(DTLocal.Rows[0]["MonthStatus"].ToString()).Equals(true))?"1":"2";
                ddlMonthStatus.SelectedValue = DTLocal.Rows[0]["MonthStatus"].ToString();
               
                btnSave.Text = "Update";
                if (ViewState["__UpdateAction__"].Equals("0"))
                {
                    btnSave.Enabled = false;
                    btnSave.CssClass = "";
                }
                else
                {
                    btnSave.Enabled = true;
                    btnSave.CssClass = "Mbutton";
                }
                ddlCompanyList.SelectedValue = DTLocal.Rows[0]["CompanyId"].ToString();
                   
                //Convert.ToDateTime(DTLocal.Rows[0]["HDate"]).ToString("dd-MMM-yyyy");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //DTLocal = null;
            }
        }
        
        protected void gvMonthSetup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
              
                if (e.CommandName == "Edit")
                {
                    if (!ViewState["__preRIndex__"].ToString().Equals("No")) gvMonthSetup.Rows[int.Parse(ViewState["__preRIndex__"].ToString())].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");

                    int rIndex = int.Parse((e.CommandArgument).ToString());
                    gvMonthSetup.Rows[rIndex].BackColor = System.Drawing.Color.Yellow;
                    ViewState["__preRIndex__"] = rIndex.ToString();
                    // gvMonthSetup.Rows[rIndex].BackColor = System.Drawing.ColorTranslator.FromHtml("#3B5998");

                    if (ViewState["__UpdateAction__"].Equals("0"))
                    {
                        btnSave.Enabled = false;
                        btnSave.CssClass = "";
                    }
                    else
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "Mbutton";
                    }

                   ViewState["__hid__"] = gvMonthSetup.DataKeys[rIndex].Value.ToString();
                   SetValueToControl(ViewState["__hid__"].ToString());

                    txtMonthName.Enabled = false;



                }
                else if (e.CommandName == "Delete")
                {
                    int id = int.Parse(e.CommandArgument.ToString());                   
                    Delete(id);
                    LoadGrid();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                //lblMessage.Text = ex.ToString();
            }
        }

        protected void gvMonthSetup_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvMonthSetup_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!InputValidationBasket()) return;
            if (btnSave.Text.Equals("Save"))
            {
                saveMonthSetup();
               
            }
            else
            {
                updateMonthSetup();
                
            }

            
        }

        protected void gvMonthSetup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadGrid();
            gvMonthSetup.PageIndex = e.NewPageIndex;
            gvMonthSetup.DataBind();
        }

        private bool InputValidationBasket()
        {
            try
            {   
                if (txtMonthName.Text.Trim().Length < 7)
                {
                    lblMessage.InnerText = "warning->Please select month.";
                    txtMonthName.Focus(); return false;
                }
           
                if (txtFromDate.Text.Trim().Length < 10)
                {
                    lblMessage.InnerText = "warning->Please from month.";
                    txtFromDate.Focus(); return false;
                }
                if (txtToDate.Text.Trim().Length < 10)
                {
                    lblMessage.InnerText = "warning->Please select to date.";
                    txtToDate.Focus(); return false;
                }
                if (byte.Parse(txtTotalNOofDay.Text.Trim()) < 28)
                {
                    lblMessage.InnerText = "warning->Please type total no of days.";
                    txtTotalNOofDay.Focus(); return false;
                }
                if (txtTotalWeekend.Text.Trim()=="") 
                {
                    lblMessage.InnerText = "warning->Please type the total weekend no.";
                    txtTotalWeekend.Focus(); return false;
                }
                if (txtTotalWorkingDays.Text.Trim()=="")
                {
                    lblMessage.InnerText = "warning->Please type total working days.";
                    txtTotalWorkingDays.Focus(); return false;
                }
                if (txtExpectedPaymetnDate.Text.Trim().Length < 10)
                {
                    lblMessage.InnerText = "warning->Please select Expected Paymetn Date.";
                    txtExpectedPaymetnDate.Focus(); return false;
                }

                if (ddlMonthStatus.SelectedValue.ToString().Equals("0"))
                {
                    lblMessage.InnerText = "warning->Please select month satus.";
                    ddlMonthStatus.Focus(); return false;

                }
                if (ViewState["__IsCalculated__"].ToString().Equals("No")) 
                {
                    lblMessage.InnerText = "warning->Please click calculation button."; return false;
                }
                return true;
            }
            catch { return false; }
        }

        private void MonthSetupCalculation()
        {
            try
            {


                string CompanyId = (ddlCompanyList.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyList.SelectedValue.ToString();

                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select Weekend from HRD_CompanyInfo where CompanyId='" + CompanyId + "'", dt);

                string Weekend = dt.Rows[0]["Weekend"].ToString();
                ViewState["__Weekend__"] = Weekend;
                string[] FromDate = txtFromDate.Text.Split('-');
                string[] ToDate = txtToDate.Text.Split('-');
                DateTime begin = new DateTime(int.Parse(FromDate[2]), int.Parse(FromDate[1]), int.Parse(FromDate[0]));
                DateTime end = new DateTime(int.Parse(ToDate[2]), int.Parse(ToDate[1]), int.Parse(ToDate[0]));

                dt = new DataTable();
                dt.Columns.Add("WDate", typeof(string));
                byte totalDays = 0;
                while (begin <= end)
                {
                    if (begin.DayOfWeek.ToString() == Weekend)
                    {
                        dt.Rows.Add(begin.ToString("dd-MM-yyyy"));
                    }
                    begin = begin.AddDays(1); totalDays++;
                }

                gvWeekendDate.DataSource = dt;
                gvWeekendDate.DataBind();

                txtTotalNOofDay.Text = totalDays.ToString();

                sqlDB.fillDataTable("select * from v_tblHolydayWork where CompanyId='"+CompanyId+"' and  MonthYear ='" + txtMonthName.Text.Substring(3, 4) + "-" + txtMonthName.Text.Substring(0, 2) + "'", dt = new DataTable());

                txtTotalHoliday.Text = dt.Rows.Count.ToString();

                txtTotalWeekend.Text = gvWeekendDate.Rows.Count.ToString();

                txtTotalWorkingDays.Text = (int.Parse(txtTotalNOofDay.Text) - int.Parse(txtTotalHoliday.Text) - int.Parse(txtTotalWeekend.Text)).ToString();
            }
            catch { }
        }

        protected void btnCalculation_Click(object sender, EventArgs e)
        {
            lblMessage.InnerText = "";
            MonthSetupCalculation();
            ModalPopupExtender1.Show();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                ViewState["__IsCalculated__"] = "Yes";
                byte CheckedCount = 0;
                for (byte b = 0; b < gvWeekendDate.Rows.Count;b++ )
                {
                    CheckBox chk = new CheckBox();
                    chk = (CheckBox)gvWeekendDate.Rows[b].Cells[1].FindControl("SelectCheckBox");

                    if (chk.Checked)
                    {
                        CheckedCount++;
                    }
                

                }

                txtTotalWeekend.Text = CheckedCount.ToString();

                txtTotalWorkingDays.Text = (int.Parse(txtTotalNOofDay.Text) - int.Parse(txtTotalHoliday.Text) - int.Parse(txtTotalWeekend.Text)).ToString();

                ModalPopupExtender1.Hide();
                
            }
            catch { }
        }

        protected void gvMonthSetup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
           {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["onmouseover"] = "javascript:SetMouseOver(this)";
                    e.Row.Attributes["onmouseout"] = "javascript:SetMouseOut(this)";
                }
            }
            catch { }


            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Viewer"))
            {
                try
                {
                    if (ViewState["__DeletAction__"].ToString().Equals("0"))
                    {
                        Button lnkDelete = (Button)e.Row.FindControl("lnkDelete");
                        lnkDelete.Enabled = false;
                        lnkDelete.OnClientClick = "return false";
                        lnkDelete.ForeColor = Color.Silver;
                    }

                }
                catch { }
                try
                {
                    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        Button lnkDelete = (Button)e.Row.FindControl("lnkEdit");
                        lnkDelete.Enabled = false;
                        lnkDelete.ForeColor = Color.Silver;
                    }

                }
                catch { }
            }
        }

        protected void dlDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
           //     classes.commonTask.loadDivision(dlDivision, ddlCompanyList.SelectedValue.ToString(), "Admin");
                LoadGrid();
            }
            catch { }
        }
    }
}