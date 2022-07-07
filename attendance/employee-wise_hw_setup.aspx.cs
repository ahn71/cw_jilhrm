using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComplexScriptingSystem;
using System.Drawing;

namespace SigmaERP.attendance
{
    public partial class employee_wise_hw_setup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
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
                ViewState["__preRIndex__"] = "No";
                string getUserId = getCookies["__getUserId__"].ToString();

                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__UserId__"] = getCookies["__getUserId__"].ToString();
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where ModulePageName='employee-wise_hw_setup.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["ReadAction"].ToString()).Equals(false) &&
                           bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false) &&
                           bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false) &&
                           bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                        {
                            ViewState["__WriteAction__"] = "0";
                            btnSave.CssClass = "";
                            btnSave.Enabled = false;
                            ViewState["__UpdateAction__"] = "0";
                            ViewState["__DeletAction__"] = "0";
                            return;
                        }
                        if (bool.Parse(dt.Rows[0]["WriteAction"].ToString()).Equals(false))
                        {
                            ViewState["__WriteAction__"] = "0";
                            btnSave.CssClass = "";
                            btnSave.Enabled = false;
                        }
                        if (bool.Parse(dt.Rows[0]["UpdateAction"].ToString()).Equals(false))
                        {
                            ViewState["__UpdateAction__"] = "0";
                        }
                        if (bool.Parse(dt.Rows[0]["DeleteAction"].ToString()).Equals(false))
                        {
                            ViewState["__DeletAction__"] = "0";
                        }
                    }
                    else
                    {
                        ViewState["__WriteAction__"] = "0";
                        btnSave.CssClass = "";
                        btnSave.Enabled = false;
                        ViewState["__UpdateAction__"] = "0";
                        ViewState["__DeletAction__"] = "0";
                        return;
                    }


                }
                classes.commonTask.LoadBranch(ddlCompanyList);
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                ViewState["__preRIndex__"] = "No";
                //setPrivilege();
                ViewState["__EmpId__"] = "0";
                //LoadHolidaysByCompany();
                lblEmpName.Text = "";
                loadLeaveInfo();
                //  dlExistsUser.Enabled = false;
            }


            catch { }

        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            if (txtCardNo.Text.Trim().Length<4)
            {
                lblMessage.InnerText = "warning->Please type valid card no .";
                return;
            }

            FindEmployee();
        }

        private void FindEmployee()
        { 
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select Max(SN) as SN, EmpId,EmpName, DptName,DptId,DsgId from v_Personnel_EmpCurrentStatus where EmpCardNo Like '%" + txtCardNo.Text.Trim() + "' AND EmpStatus in ('1','8')  group by EmpId,EmpName, DptName,DptId,DsgId ", dt);
                if (dt.Rows.Count>0)
                {
                    lblEmpName.Text = dt.Rows[0]["EmpName"].ToString() + " | " + dt.Rows[0]["DptName"].ToString();
                    ViewState["__EmpId__"] = dt.Rows[0]["EmpId"].ToString();
                    ViewState["__DptId__"] = dt.Rows[0]["DptId"].ToString();
                    ViewState["__DsgId__"] = dt.Rows[0]["DsgId"].ToString();
                }
            }
            catch { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidationBasket()) return;

            if (btnSave.Text == "Save") saveHolyDayWeekendOfEmp();
            else updateHolyDayWeekendOfEmp();
        }

        private bool ValidationBasket()
        {
            try
            {
                if (txtCardNo.Text.Trim().Length<4)
                {
                    lblMessage.InnerText = "warning->Please type valid card no";
                    txtCardNo.Focus();
                    return false;
                }
                else if (ViewState["__EmpId__"].ToString()=="0")
                {
                    lblMessage.InnerText = "warning->Please find employee by card no ";
                    txtCardNo.Focus();
                    return false;
                }
                else if (txtFromDate.Text.Trim().Length<10)
                {
                    lblMessage.InnerText = "Please select or type valid date ";
                    txtFromDate.Focus();
                    return false;
                }
                return true;
            }
            catch { return false; }
        }

        private void saveHolyDayWeekendOfEmp()
        {
            try
            {
                bool result = false;
                string HWCode="";
                

                string[] getColumns = { "EmpId", "FromDate", "ToDate", "Remarks", "HWType", "CompanyId", "UserId", "DptId", "DsgId", "HWDayName" };
                string[] getValues = { ViewState["__EmpId__"].ToString(), convertDateTime.getCertainCulture(txtFromDate.Text.Trim()).ToString(),convertDateTime.getCertainCulture(txtToDate.Text.Trim()).ToString(),txtDescription.Text.Trim(), ddlType.SelectedItem.Text,
                                         ddlCompanyList.SelectedItem.Value, ViewState["__UserId__"].ToString(),ViewState["__DptId__"].ToString(),
                                     ViewState["__DsgId__"].ToString(),ddlWeekend.SelectedItem.Value };
                if (SQLOperation.forSaveValue("tblHolydayWeekentEmployeeWise", getColumns, getValues, sqlDB.connection) == true)
                {
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select Max(HWCode) as HWCode from tblHolydayWeekentEmployeeWise ",dt);
                    HWCode = dt.Rows[0]["HWCode"].ToString();
                    result =true;
                }
                
                if (SaveHolyDayWeekendDetails(HWCode))
                {
                    lblMessage.InnerText = "success->Successfully Saved";
                    loadLeaveInfo();
                    ClearAll();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private bool SaveHolyDayWeekendDetails(string HWCode)
        {
            try
            {
                string[] FDates = txtFromDate.Text.Split('-');
                string[] TDates = txtToDate.Text.Split('-');
                DateTime FromDate = new DateTime(int.Parse(FDates[2]), int.Parse(FDates[1]), int.Parse(FDates[0]));
                DateTime ToDate = new DateTime(int.Parse(TDates[2]), int.Parse(TDates[1]), int.Parse(TDates[0]));

                while (FromDate <= ToDate)
                {
                    if (FromDate.DayOfWeek.ToString().Equals(ddlWeekend.SelectedItem.Text))
                    {
                        string[] FromDates = FromDate.ToString().Split('/');

                        FromDates[1] = (FromDates[1].Trim().Length == 1) ? "0" + FromDates[1] : FromDates[1];
                        FromDates[0] = (FromDates[0].Trim().Length == 1) ? "0" + FromDates[0] : FromDates[0];

                        string HWDate = FromDates[1] + "-" + FromDates[0] + "-" + FromDates[2].Substring(0, 4);

                        string[] getCells = { "HWCode", "EmpId", "HWDate" };
                        string[] setValues = { HWCode, ViewState["__EmpId__"].ToString(), convertDateTime.getCertainCulture(HWDate).ToString() };

                        SQLOperation.forSaveValue("tblHolydayWeekentEmployeeWiseDetails", getCells, setValues, sqlDB.connection);
                    }

                    FromDate = FromDate.AddDays(1);

                }
                return true;
            }
            catch { return false; }
        }

        private void updateHolyDayWeekendOfEmp()
        {
            try
            {
                string[] getColumns = { "FromDate", "ToDate", "Remarks", "CompanyId", "UserId", "DptId", "DsgId", "HWDayName" };
                string[] getValues = { convertDateTime.getCertainCulture(txtFromDate.Text.Trim()).ToString(), convertDateTime.getCertainCulture(txtToDate.Text.Trim()).ToString(), txtDescription.Text.Trim(),
                                     ddlCompanyList.SelectedItem.Value, ViewState["__UserId__"].ToString(),ViewState["__DptId__"].ToString(),ViewState["__DsgId__"].ToString(),ddlWeekend.SelectedItem.Text};

                if (SQLOperation.forUpdateValue("tblHolydayWeekentEmployeeWise", getColumns, getValues, "HWCode", ViewState["__HWCode__"].ToString(), sqlDB.connection) == true)
                {
                    SQLOperation.forDeleteRecordByIdentifier("tblHolydayWeekentEmployeeWiseDetails", "HWCode", ViewState["__HWCode__"].ToString().ToString(),sqlDB.connection);
                   
                    if (SaveHolyDayWeekendDetails(ViewState["__HWCode__"].ToString()))
                    {
                        lblMessage.InnerText = "success->Successfully Updated";
                        loadLeaveInfo();
                        ClearAll();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ClearAll()
        {
            txtCardNo.Text = "";
            txtDescription.Text = "";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            ViewState["__EmpId__"] = "0";
            lblEmpName.Text = "";
            if (ViewState["__WriteAction__"].ToString().Equals("0"))
            {
                btnSave.CssClass = "";
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.CssClass = "Mbutton";
                btnSave.Enabled = true;
            }
            btnSave.Text = "Save";
        }

        private void loadLeaveInfo()
        {
            try
            {
                DataTable dt=new DataTable ();
                sqlDB.fillDataTable("select HWCode,EmpCardNo,EmpName,DptName,Format(FromDate,'dd-MM-yyyy') as FromDate,Format(ToDate,'dd-MM-yyyy') as ToDate,HWType,Remarks,HWDayName from v_tblHolydayWeekentEmployeeWise where IsUsed='False' order by HWCode Desc", dt);
                gvHoliday.DataSource = dt;
                gvHoliday.DataBind();
            }
            catch { }
        }

        protected void gvHoliday_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                if (e.CommandName=="Alter")
                {
                    DataTable dt=new DataTable ();
                    sqlDB.fillDataTable("select EmpId,Format(FromDate,'dd-MM-yyyy') as FromDate,Format(ToDate,'dd-MM-yyyy') as ToDate,Remarks,HWType,CompanyId,DptId,DsgId,HWDayName from tblHolydayWeekentEmployeeWise where HWCode=" + gvHoliday.DataKeys[rIndex].Value.ToString() + "", dt);
                    ViewState["__HWCode__"] = gvHoliday.DataKeys[rIndex].Value.ToString();
                    ViewState["__EmpId__"] = dt.Rows[0]["EmpId"].ToString();
                    ddlCompanyList.SelectedValue = dt.Rows[0]["CompanyId"].ToString();
                    txtCardNo.Text = gvHoliday.Rows[rIndex].Cells[0].Text.Trim();
                    txtFromDate.Text = dt.Rows[0]["FromDate"].ToString();
                    txtToDate.Text = dt.Rows[0]["ToDate"].ToString();
                    ddlType.SelectedValue = dt.Rows[0]["HWType"].ToString();
                    txtDescription.Text = dt.Rows[0]["Remarks"].ToString();
                    ViewState["__DptId__"] = dt.Rows[0]["DptId"].ToString();
                    ViewState["__DsgId__"] = dt.Rows[0]["DsgId"].ToString();
                    lblEmpName.Text = gvHoliday.Rows[rIndex].Cells[1].Text + " " + gvHoliday.Rows[rIndex].Cells[2].Text;
                    ddlWeekend.SelectedValue = dt.Rows[0]["HWDayName"].ToString();
                    if (!ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        btnSave.Enabled = true;
                        btnSave.CssClass = "Mbutton";
                    }
                    btnSave.Text = "Update";
                }
                else if (e.CommandName=="Delete")
                {
                   if (SQLOperation.forDeleteRecordByIdentifier("tblHolydayWeekentEmployeeWise","HWCode",gvHoliday.DataKeys[rIndex].Value.ToString(),sqlDB.connection))
                   {
                       lblMessage.InnerText = "success->Successfully Record Deleted";
                   }
                }
            }
            catch { }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        protected void gvHoliday_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            loadLeaveInfo();
        }

        protected void gvHoliday_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvHoliday.PageIndex = e.NewPageIndex;
                loadLeaveInfo();
            }
            catch { }
        }

        protected void gvHoliday_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Admin"))
            {
                LinkButton lnk = new LinkButton();
                try
                {
                    if (ViewState["__DeletAction__"].ToString().Equals("0"))
                    {
                        lnk = (LinkButton)e.Row.FindControl("lnkDelete");
                        lnk.Enabled = false;
                        lnk.OnClientClick = "return false";
                        lnk.ForeColor = Color.Black;
                    }

                }
                catch { }
                try
                {
                    if (ViewState["__UpdateAction__"].ToString().Equals("0"))
                    {
                        lnk = (LinkButton)e.Row.FindControl("lnkAlter");
                        lnk.Enabled = false;
                        lnk.ForeColor = Color.Black;
                    }

                }
                catch { }
            }
        }
    }
}