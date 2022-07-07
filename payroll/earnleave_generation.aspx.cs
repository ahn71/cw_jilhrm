using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;





namespace SigmaERP.payroll
{
    public partial class earnleave_generation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            if (!IsPostBack)
            {
                setPrivilege();
                loadGenerationMonths();
                ddlEarnLeaveYear.Items.Clear();
                ListItem[] itens = new ListItem[4] {
    new ListItem("-- Year --"),
    new ListItem(DateTime.Today.Year.ToString()),
    new ListItem(DateTime.Today.AddYears(-1).Year.ToString()),
    new ListItem(DateTime.Today.AddYears(-2).Year.ToString())
};
                ddlEarnLeaveYear.Items.AddRange(itens);
                classes.Employee.LoadEmpCardNoWithNameByCompany(ddlEmpCardNo, ddlBranch.SelectedValue, "2", "Select For Individual Generation");
            }
        }
        static DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__UserId__"] = getUserId;
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();



                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "generation.aspx", ddlBranch, btnGenerate);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];
                ddlBranch.SelectedValue = ViewState["__CompanyId__"].ToString();

                if (ViewState["__ReadAction__"].ToString().Equals("0"))
                {
                    gvEmpEarnLeaveList.Visible = false;

                }





            }


            catch { }

        }

        private void loadGenerationMonths()
        {
            try
            {
                int b = 0;
                byte i = 0;
                while (b <= 12)
                {
                    if (b == 0) ddlGenerateMonth.Items.Insert(i, new ListItem(" ", b.ToString()));
                    else
                    {
                        string getMonthName = new DateTime(int.Parse(DateTime.Now.ToString("yyyy")), b, 1).ToString("MMM", CultureInfo.InvariantCulture);

                        ddlGenerateMonth.Items.Insert(i, new ListItem(getMonthName + "-" + DateTime.Now.ToString("yyyy"), b.ToString()));
                    }
                    b = b + 1; i++;
                }
            }
            catch { }

        }

        protected void ddlGenerateMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               

                
                if (ddlGenerateMonth.SelectedValue.ToString().Equals("0")) return;
                else
                {
                    byte getMonth = byte.Parse(DateTime.Now.ToString("MM"));

                    if (ddlGenerateMonth.SelectedValue.ToString().Equals(getMonth.ToString()))
                    {
                        string getYear = ddlGenerateMonth.SelectedItem.ToString().Substring(4, 4);

                        if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Jan")) getYear = getYear + "-01";
                        else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Feb")) getYear = getYear + "-02";
                        else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Mar")) getYear = getYear + "-03";
                        else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Apr")) getYear = getYear + "-04";
                        else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("May")) getYear = getYear + "-05";
                        else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Jun")) getYear = getYear + "-06";
                        else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Jul")) getYear = getYear + "-07";
                        else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Aug")) getYear = getYear + "-08";
                        else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Sep")) getYear = getYear + "-09";
                        else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Oct")) getYear = getYear + "-10";
                        else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Nov")) getYear = getYear + "-11";
                        else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Dec")) getYear = getYear + "-12";

                        string DaysRange = DateTime.DaysInMonth(int.Parse(getYear.Substring(0, 4)), int.Parse(getYear.Substring(5, 2))).ToString();
                        ViewState["__DaysOfMonth__"] = DaysRange;
                        DaysRange = int.Parse(getYear.Substring(0, 4)) - 1 + "-" + getYear.Substring(5, 2) + "-" + int.Parse(DaysRange);
                        if (getYear.Substring(5, 2) == "02" && ((int.Parse(getYear.Substring(0, 4)) - 1) % 4 == 0) && ((int.Parse(getYear.Substring(0, 4)) - 1) % 100 != 0) || ((int.Parse(getYear.Substring(0, 4)) - 1) % 400 == 0))
                        {
                            DaysRange = DaysRange;
                        }
                        else if (getYear.Substring(5, 2) == "02")
                        {
                            if (ViewState["__DaysOfMonth__"].ToString() == "29")
                                DaysRange = int.Parse(getYear.Substring(0, 4)) - 1 + "-" + getYear.Substring(5, 2) + "-" + (int.Parse(ViewState["__DaysOfMonth__"].ToString()) - 1);
                        }
                        string FromDate = "2011-" + (int.Parse(getYear.Substring(5, 2))) + "-01";

                        if (checkAlreadyGenerated(getYear) == false) return;
                        else
                        {
                            DataTable dt = new DataTable();
                            string sqlcmd = ("SELECT SN, EmpId, EmpName, EmpCardNo, EmpTypeId, EmpType, DATEDIFF(day, EarnLeaveDate,'" + DateTime.Now.ToString("yyyy-MM-dd") + "') AS TotalDays," +
                                " ActiveSalary FROM dbo.v_Personnel_EmpCurrentStatus WHERE DATEDIFF(day, EarnLeaveDate,'" + DateTime.Now.ToString("yyyy-MM-dd") + "') >= 360 " +
                                "  AND CompanyId='" + ddlBranch.SelectedValue + "' AND EmpStatus in ('1','8') and IsActive=1 and EarnLeaveDate>='" + FromDate + "' and EarnLeaveDate<='" + DaysRange + "' AND Convert(varchar(3),EarnLeaveDate,100)='" + ddlGenerateMonth.SelectedItem.Text.Trim().Substring(0, 3) + "' ");
                            txtquery.Text = sqlcmd;
                            sqlDB.fillDataTable(sqlcmd, dt);
                            gvEmpEarnLeaveList.DataSource = dt;
                            gvEmpEarnLeaveList.DataBind();

                            hHeader.InnerText = "Earn leave List ->" + dt.Rows.Count;
                            ModalPopupExtender1.Show();
                        }
                    }
                    else
                    {
                        lblGeneratedMessage.Text = "Your selected month is not same of current system month !";
                        ModalPopupExtender2.Show();
                    }
                }
            }
            catch { }
        }

        private void loadEmployeeForThisMonth()
        {
            try
            {

                DataTable dt = new DataTable();
                string sqlcmd = "SELECT SN, EmpId, EmpName, EmpCardNo, EmpTypeId, EmpType, DATEDIFF(day, EarnLeaveDate,'" + DateTime.Now.ToString("yyyy-MM-dd") + "') AS TotalDays," +
                    " ActiveSalary FROM dbo.v_Personnel_EmpCurrentStatus WHERE DATEDIFF(day, EarnLeaveDate,'" + DateTime.Now.ToString("yyyy-MM-dd") + "') >= 360 " +
                    " AND CompanyId='" + ddlBranch.SelectedValue + "' AND EmpStatus in ('1','8') and IsActive=1 and EarnLeaveDate>='2013-06-01' and EarnLeaveDate<='2013-06-30'";
                sqlDB.fillDataTable(sqlcmd, dt);
                gvEmpEarnLeaveList.DataSource = dt;
                gvEmpEarnLeaveList.DataBind();

                hHeader.InnerText = "Earn leave List ->" + dt.Rows.Count + " " + sqlcmd;

                ModalPopupExtender1.Show();
            }
            catch { }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                ModalPopupExtender1.Hide();

            }
            catch { }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                //if (ddlBranch.SelectedIndex < 1)
                //{
                //    lblMessage.InnerText = "warning-> Please, Select Company!"; ddlBranch.Focus();
                //    return;
                //}
                //if (ddlEarnLeaveYear.SelectedIndex < 1)
                //{
                //    lblMessage.InnerText = "warning-> Please, Select Earn Leave Year!"; ddlEarnLeaveYear.Focus();
                //    return;
                //}
                //if (txtGenerateMonth.Text.Length < 8)
                //{
                //    lblMessage.InnerText = "warning-> Please, Select Earn Leave To Date !"; txtGenerateMonth.Focus();
                //    return;
                //}

                DataTable dtLD = new DataTable();
                sqlDB.fillDataTable("select LeaveDays from tblLeaveConfig where ShortName='a/l' and  CompanyId='"+ddlBranch.SelectedValue+"'", dtLD);
                string Year = (int.Parse(ddlEarnLeaveYear.SelectedValue) - 1).ToString();
                lblMessage.InnerText = "";
                //SQLOperation.forDelete("Leave_LastEarnLeaveDateLog", sqlDB.connection);            
               // ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "Processing();", true);
                int TotalGenerated = 0;
                  for (int i = 0; i < gvEmpEarnLeaveList.Rows.Count; i++)
                    {
                        CheckBox chk = (CheckBox)gvEmpEarnLeaveList.Rows[i].Cells[3].FindControl("SelectCheckBox");

                        if (chk.Checked)
                        {
                            try
                            {


                                DateTime EL_SatrtDate;
                                DateTime EL_EndDate;
                                DataTable dt = new DataTable();
                                sqlDB.fillDataTable("select EmpId,EmpTypeId,CompanyId,SftId,DptId,DsgId,BasicSalary,FORMAT(EarnLeaveDate,'dd-MM')+ '-" + Year + "' as EarnLeaveDate,'" + Year + "-'+ FORMAT(EarnLeaveDate,'MM-dd') as EarnLeaveDate2 from v_Personnel_EmpCurrentStatus where SN=" + gvEmpEarnLeaveList.DataKeys[i].Value.ToString() + "", dt);

                                DataTable dtEx = new DataTable();
                                sqlDB.fillDataTable("select EarnLeavePerviousStartYear from Leave_YearlyEarnLeaveGeneration where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' and YEAR(EarnLeavePerviousStartYear)='"+ddlEarnLeaveYear.SelectedItem.Text+"'", dtEx);
                                if (dtEx.Rows.Count > 0) 
                                {
                                    EL_SatrtDate = DateTime.Parse(dtEx.Rows[0]["EarnLeavePerviousStartYear"].ToString());
                                    SqlCommand cmd = new SqlCommand("delete from Leave_YearlyEarnLeaveGeneration where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' and YEAR(EarnLeavePerviousStartYear)='" + ddlEarnLeaveYear.SelectedItem.Text + "'",sqlDB.connection);
                                    cmd.ExecuteNonQuery();
                                }
                                else
                                 EL_SatrtDate = DateTime.Parse(dt.Rows[0]["EarnLeaveDate2"].ToString()).AddDays(1);
                                 EL_EndDate = EL_SatrtDate.AddYears(1).AddDays(-1);
                                string TotalDays = (EL_EndDate - EL_SatrtDate).TotalDays.ToString();

                      
                       
                                DataTable dtGetWorkDays = new DataTable();
                                DataTable dtEL;

                                sqlDB.fillDataTable("select ATTStatus from tblAttendanceRecord where ATTStatus='Lv' and StateStatus='Annual Leave' and ATTDate>='" + EL_SatrtDate.ToString("yyyy-MM-dd") + "' and ATTDate<='" + EL_EndDate.ToString("yyyy-MM-dd") + "'  and EmpId='"+dt.Rows[0]["EmpId"].ToString()+"'", dtEL = new DataTable());
                                int getEarnLeaveDays = int.Parse(dtLD.Rows[0]["LeaveDays"].ToString()) - dtEL.Rows.Count; 
                                DataTable dtPresentDays = new DataTable();                
                                    sqlDB.fillDataTable("select ATTStatus from tblAttendanceRecord where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' AND  ATTDate >='" + EL_SatrtDate.ToString("yyyy-MM-dd") + "' AND ATTDate <= '" + EL_EndDate.ToString("yyyy-MM-dd") + "' AND AttStatus in ('p','l')", dtPresentDays);
                                    if (dtPresentDays.Rows.Count > 0)
                                    { }
                             
                                double getTotalAmount = 0;
                                if (rblGeneratedOn.SelectedValue.ToString().Equals("Basic"))
                                    getTotalAmount = Math.Round((double.Parse(dt.Rows[0]["BasicSalary"].ToString())/30) * getEarnLeaveDays);    // this calculation for yearly earn leave generation and whose are currently regular
                                else getTotalAmount = Math.Round((double.Parse(dt.Rows[0]["EmpPresentSalary"].ToString())/30 )* getEarnLeaveDays); // this calculation for yearly earn leave generation and whose are currently regular

                                //---------------------------------------------------------------------------------------------------







                                string[] getColumns = { "EmpId", "EmpTypeId", "BasicSalary", "NetTotal", "CompanyId", "DptId", "DsgId", "GenerateDate", "EarnLeavePerviousStartYear", "EarnLeaveEndYear", "PayableDays", "WorkingDays", "IsSeperate", "SpendDays", "ELDateForReport" };
                                string[] getValues = { dt.Rows[0]["EmpId"].ToString(),dt.Rows[0]["EmpTypeId"].ToString(),dt.Rows[0]["BasicSalary"].ToString(),getTotalAmount.ToString(), dt.Rows[0]["CompanyId"].ToString(), dt.Rows[0]["DptId"].ToString(),
                                                     dt.Rows[0]["DsgId"].ToString(),convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy")).ToString(),
                                                     convertDateTime.getCertainCulture(EL_SatrtDate.ToString("dd-MM-yyyy")).ToString(),convertDateTime.getCertainCulture(EL_EndDate.ToString("dd-MM-yyyy")).ToString(),getEarnLeaveDays.ToString(),dtPresentDays.Rows.Count.ToString(),
                                                     "0",dtEL.Rows.Count.ToString(),convertDateTime.getCertainCulture(EL_SatrtDate.AddYears(1).ToString("dd-MM-yyyy")).ToString()};
                                SQLOperation.forSaveValue("Leave_YearlyEarnLeaveGeneration", getColumns, getValues, sqlDB.connection);
                                
                                // below part for update earn leave date in emp current status
                                string[] getFiled = { "EarnLeaveDate" };  // start date as next earn leave generate 
                                string[] getValue = { convertDateTime.getCertainCulture(EL_EndDate.ToString("dd-MM-yyyy")).ToString() };
                                SQLOperation.forUpdateValue("Personnel_EmpCurrentStatus", getFiled, getValue, "SN", gvEmpEarnLeaveList.DataKeys[i].Value.ToString(), sqlDB.connection);

                                //--------------------below code for keep Earnleave Date which is used for delete operation-----------------------

                                SqlCommand cmd1 = new SqlCommand("delete from Leave_LastEarnLeaveDateLog where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' and EarnLeaveDate='" + convertDateTime.getCertainCulture(EL_EndDate.ToString("dd-MM-yyyy")).ToString() + "'", sqlDB.connection);
                                cmd1.ExecuteNonQuery();
                                string[] getCells = { "EmpId", "EarnLeaveDate" };  // start date as next earn leave generate 
                                string[] getCellValus = { dt.Rows[0]["EmpId"].ToString(), convertDateTime.getCertainCulture(EL_EndDate.ToString("dd-MM-yyyy")).ToString() };
                                SQLOperation.forSaveValue("Leave_LastEarnLeaveDateLog", getCells, getCellValus, sqlDB.connection);
                               TotalGenerated++;
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(ex.Message);
                            }

                        }

                    }
                  ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ProcessingEnd(" + TotalGenerated + ");", true);
                  txtGenerateMonth.Text = "";
                  ddlEarnLeaveYear.SelectedIndex = 0;
                
                //}

            }
            catch { }
        }

        private void oldGenerateCode() 
        {
            try
            {
                DataTable dtLD = new DataTable();
                sqlDB.fillDataTable("select LeaveDays from tblLeaveConfig where ShortName='a/l'", dtLD);

                lblMessage.InnerText = "";
                SQLOperation.forDelete("Leave_LastEarnLeaveDateLog", sqlDB.connection);
                if (ddlGenerateMonth.SelectedValue.ToString() == "0")
                {
                    lblMessage.InnerText = "warning->Select Generation Month";
                    return;
                }
                string getYear = ddlGenerateMonth.SelectedItem.ToString().Substring(4, 4);

                if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Jan")) getYear = getYear + "-01";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Feb")) getYear = getYear + "-02";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Mar")) getYear = getYear + "-03";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Apr")) getYear = getYear + "-04";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("May")) getYear = getYear + "-05";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Jun")) getYear = getYear + "-06";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Jul")) getYear = getYear + "-07";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Aug")) getYear = getYear + "-08";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Sep")) getYear = getYear + "-09";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Oct")) getYear = getYear + "-10";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Nov")) getYear = getYear + "-11";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Dec")) getYear = getYear + "-12";

               // if (!checkGetLastDateOfMonth()) return;

                int TotalGenerated = 0;
                if (checkAlreadyGenerated(getYear) == false) return;

                else
                {
                    for (int i = 0; i < gvEmpEarnLeaveList.Rows.Count; i++)
                    {
                        CheckBox chk = (CheckBox)gvEmpEarnLeaveList.Rows[i].Cells[3].FindControl("SelectCheckBox");

                        if (chk.Checked)
                        {
                            try
                            {
                                DataTable dt = new DataTable();
                                sqlDB.fillDataTable("select EmpId,EmpTypeId,CompanyId,SftId,DptId,DsgId,BasicSalary,convert(varchar(11),EarnLeaveDate,105) as EarnLeaveDate,Format(EarnLeaveDate,'yyyy-MM-dd') as EarnLeaveDate2 from v_Personnel_EmpCurrentStatus where SN=" + gvEmpEarnLeaveList.DataKeys[i].Value.ToString() + "", dt);
                                string PreviousYear = "";

                                string[] getEarnleaveDate = dt.Rows[0]["EarnLeaveDate"].ToString().Split('-');
                                string getDay = (getEarnleaveDate[0].Length == 1) ? "0" + getEarnleaveDate[0] : getEarnleaveDate[0];
                                string getMonth = (getEarnleaveDate[1].Length == 1) ? "0" + getEarnleaveDate[1] : getEarnleaveDate[1];
                                int getStartYear = int.Parse(getEarnleaveDate[2]) + 1;
                                int tempYear = getStartYear;



                                string generateDay = ViewState["__DaysOfMonth__"].ToString();
                                string generateMonth = getMonth;


                                DateTime getEndDate = new DateTime(int.Parse(getEarnleaveDate[2]), int.Parse(getEarnleaveDate[1]), int.Parse(getEarnleaveDate[0]));
                                getEndDate = getEndDate.AddDays(-1);



                                string[] getEndEarnLeaveDate = getEndDate.ToString().Split('/');

                                string getEndDay = (getEndEarnLeaveDate[1].Length == 1) ? "0" + getEndEarnLeaveDate[1] : getEndEarnLeaveDate[1];
                                string getEndMonth = (getEndEarnLeaveDate[0].Length == 1) ? "0" + getEndEarnLeaveDate[0] : getEndEarnLeaveDate[0];

                                tempYear = (int.Parse(getEndEarnLeaveDate[2].Substring(0, 4)) < tempYear - 1) ? tempYear - 1 : int.Parse(getEndEarnLeaveDate[2].Substring(0, 4)) + 1;

                                string FromDateForWD = getEarnleaveDate[2] + "-" + generateMonth + "-01";
                                string ToDateForWD = getStartYear + "-" + generateMonth + "-01";

                                DataTable dtGetWorkDays = new DataTable();

                                //core calculation----------------------------------------------

                                if (DateTime.ParseExact(dt.Rows[0]["EarnLeaveDate2"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy") != (int.Parse(DateTime.Now.Year.ToString()) - 1).ToString())
                                {
                                    PreviousYear = (int.Parse(DateTime.Now.Year.ToString()) - 1).ToString() + "-" + DateTime.ParseExact(dt.Rows[0]["EarnLeaveDate2"].ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("MM-dd");
                                }
                                else PreviousYear = dt.Rows[0]["EarnLeaveDate2"].ToString();


                                string getEarnLeaveDays = "0"; DataTable dtPresentDays = new DataTable();
                                // for just static 360 days
                                if (rblDaysOptions.SelectedValue.ToString().Equals("On360Days"))
                                    getEarnLeaveDays = (360 / int.Parse(dtLD.Rows[0]["LeaveDays"].ToString())).ToString();
                                else  // for just on present or late days amount in 360 days
                                {
                                    dtPresentDays = new DataTable();

                                    DateTime ToDate = DateTime.ParseExact(PreviousYear, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                    ToDate = ToDate.AddYears(1);
                                    ToDateForWD = ToDate.AddDays(-1).ToString("yyyy-MM-dd");
                                    sqlDB.fillDataTable("select ATTStatus from tblAttendanceRecord where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' AND  ATTDate >='" + PreviousYear + "' AND ATTDate < '" + ToDateForWD + "' AND AttStatus in ('p','l')", dtPresentDays);
                                    if (dtPresentDays.Rows.Count > 0)
                                        getEarnLeaveDays = Math.Round(Double.Parse(dtPresentDays.Rows.Count.ToString()) / double.Parse(dtLD.Rows[0]["LeaveDays"].ToString()), 0).ToString();
                                }
                                getEarnLeaveDays = Math.Round(double.Parse(getEarnLeaveDays), 0).ToString();

                                // now get acutual days,which is used for earn leave amount
                                DataTable dtALAmount = new DataTable();
                                sqlDB.fillDataTable("select ATTStatus from tblAttendanceRecord where EmpId='" + dt.Rows[0]["EmpId"].ToString() + "' AND  ATTDate >='" + PreviousYear + "' AND ATTDate < '" + ToDateForWD + "' AND StateStatus='E/L'", dtALAmount);

                                getEarnLeaveDays = (int.Parse(getEarnLeaveDays) - dtALAmount.Rows.Count).ToString();

                                if (dt.Rows[0]["EmpId"].ToString().Equals("00001430") || dt.Rows[0]["EmpId"].ToString().Equals("2606"))
                                {

                                }
                                double getTotalAmount = 0;
                                if (rblGeneratedOn.SelectedValue.ToString().Equals("Basic"))
                                    getTotalAmount = Math.Round(double.Parse(dt.Rows[0]["BasicSalary"].ToString()) * (int.Parse(getEarnLeaveDays)) / 30, 0);    // this calculation for yearly earn leave generation and whose are currently regular
                                else getTotalAmount = Math.Round(double.Parse(dt.Rows[0]["EmpPresentSalary"].ToString()) * (int.Parse(getEarnLeaveDays)) / 30, 0);    // this calculation for yearly earn leave generation and whose are currently regular

                                //---------------------------------------------------------------------------------------------------

                                // find total working days of the employee



                                // sqlDB.fillDataTable("select SUM(PayableDays) as PD from v_MonthlySalarySheet where CompareDate>='" + FromDateForWD + "' and CompareDate < '" + ToDateForWD + "' and EmpId ='" + dt.Rows[0]["EmpId"].ToString() + "'", dtGetWorkDays);
                                int getWorkDays = 0;
                                if (rblDaysOptions.SelectedValue.ToString().Equals("On360Days"))
                                {
                                    DateTime ToDate = DateTime.ParseExact(PreviousYear, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                    ToDate = ToDate.AddYears(1);
                                    ToDateForWD = ToDate.AddDays(-1).ToString("yyyy-MM-dd");
                                    sqlDB.fillDataTable("select Count(ATTDate) as PD from tblAttendanceRecord where ATTDate>='" + FromDateForWD + "' and ATTDate < '" + ToDateForWD + "' and EmpId ='" + dt.Rows[0]["EmpId"].ToString() + "' and ATTStatus In ('P','L')", dtGetWorkDays);
                                    getWorkDays = (int.Parse(dtGetWorkDays.Rows[0]["PD"].ToString()) > 0) ? int.Parse(dtGetWorkDays.Rows[0]["PD"].ToString()) : 0;

                                }
                                else getWorkDays = dtPresentDays.Rows.Count;




                                string[] getColumns = { "EmpId", "EmpTypeId", "BasicSalary", "NetTotal", "CompanyId", "SftId", "DptId", "DsgId", "GenerateDate", "EarnLeavePerviousStartYear", "EarnLeaveEndYear", "PayableDays", "WorkingDays", "IsSeperate", "SpendDays", "ELDateForReport" };
                                string[] getValues = { dt.Rows[0]["EmpId"].ToString(),dt.Rows[0]["EmpTypeId"].ToString(),dt.Rows[0]["BasicSalary"].ToString(),getTotalAmount.ToString(), dt.Rows[0]["CompanyId"].ToString(),dt.Rows[0]["SftId"].ToString(), dt.Rows[0]["DptId"].ToString(),
                                                     dt.Rows[0]["DsgId"].ToString(),convertDateTime.getCertainCulture(generateDay+"-"+generateMonth+"-"+getStartYear).ToString(),
                                                     convertDateTime.getCertainCulture(dt.Rows[0]["EarnLeaveDate"].ToString()).ToString(),convertDateTime.getCertainCulture(getEndDay+"-"+getEndMonth+"-"+tempYear).ToString(),dtLD.Rows[0]["LeaveDays"].ToString(),getWorkDays.ToString(),
                                                     "0",dtALAmount.Rows.Count.ToString(),convertDateTime.getCertainCulture(getDay + "-" + getMonth + "-" + getStartYear).ToString()};
                                SQLOperation.forSaveValue("Leave_YearlyEarnLeaveGeneration", getColumns, getValues, sqlDB.connection);

                                // below part for update earn leave date in emp current status
                                string[] getFiled = { "EarnLeaveDate" };  // start date as next earn leave generate 
                                string[] getValue = { convertDateTime.getCertainCulture(getDay + "-" + getMonth + "-" + getStartYear).ToString() };
                                SQLOperation.forUpdateValue("Personnel_EmpCurrentStatus", getFiled, getValue, "SN", gvEmpEarnLeaveList.DataKeys[i].Value.ToString(), sqlDB.connection);

                                //--------------------below code for keep Earnleave Date which is used for delete operation-----------------------

                                string[] getCells = { "EmpId", "EarnLeaveDate" };  // start date as next earn leave generate 
                                string[] getCellValus = { dt.Rows[0]["EmpId"].ToString(), convertDateTime.getCertainCulture(dt.Rows[0]["EarnLeaveDate"].ToString()).ToString() };
                                SQLOperation.forSaveValue("Leave_LastEarnLeaveDateLog", getCells, getCellValus, sqlDB.connection);
                                TotalGenerated++;
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(ex.Message);
                            }

                        }

                    }
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ProcessingEnd(" + TotalGenerated + ");", true);
                   

                }

            }
            catch { }
        }

        private bool checkGetLastDateOfMonth()
        {
            if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Jan") && DateTime.Now.ToString("dd").Equals("31")) return true;

            else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Feb"))
            {
                string getYear = ddlGenerateMonth.SelectedItem.Text.Substring(4, 4);

                if (int.Parse(getYear) % 4 == 0 && int.Parse(getYear) % 100 != 0 || int.Parse(getYear) % 400 == 0)
                {
                    if (DateTime.Now.ToString("dd").Equals("29")) return true;
                }
                else if (DateTime.Now.ToString("dd").Equals("28")) return true;
                return false;
            }

            else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Mar") && DateTime.Now.ToString("dd").Equals("31")) return true;
            else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Apr") && DateTime.Now.ToString("dd").Equals("30")) return true;
            else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("May") && DateTime.Now.ToString("dd").Equals("31")) return true;
            else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Jun") && DateTime.Now.ToString("dd").Equals("30")) return true;
            else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Jul") && DateTime.Now.ToString("dd").Equals("31")) return true;
            else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Aug") && DateTime.Now.ToString("dd").Equals("31")) return true;
            else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Sep") && DateTime.Now.ToString("dd").Equals("30")) return true;
            else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Oct") && DateTime.Now.ToString("dd").Equals("31")) return true;
            else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Nov") && DateTime.Now.ToString("dd").Equals("30")) return true;
            else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Dec") && DateTime.Now.ToString("dd").Equals("31")) return true;



            else
            {
                lblGeneratedMessage.Text = "Your Selected Date Must be Last Date Of This Month!";
                ModalPopupExtender2.Show();
                return false;
            }

        }
        private bool checkAlreadyGenerated(string getGenerateMonth)
        {

            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select GenerateMonth from v_Leave_YearlyEarnLeaveGeneration where GenerateMonth='" + getGenerateMonth + "' AND IsSeperate='false' ", dt);
                if (dt.Rows.Count > 0)
                {
                    lblGeneratedMessage.Text = "Already " + ddlGenerateMonth.SelectedItem.Text + " is generated !";
                    ModalPopupExtender2.Show();

                    sqlDB.fillDataTable("select * from Leave_LastEarnLeaveDateLog ", dt = new DataTable());
                    // if (dt.Rows.Count > 0) btnDelete.Visible = true;
                    return false;
                }

                else return true;
            }
            catch { return false; }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
           
        }
        private void OldDeleteCode()
        {
            try
            {
                string getYear = ddlGenerateMonth.SelectedItem.ToString().Substring(4, 4);

                if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Jan")) getYear = getYear + "-01-31";

                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Feb"))
                {
                    if (int.Parse(getYear) % 100 == 0 && int.Parse(getYear) % 400 == 0) getYear = getYear + "-02-29";
                    else getYear = getYear + "-02-28";
                }
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Mar")) getYear = getYear + "-03-31";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Apr")) getYear = getYear + "-04-30";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("May")) getYear = getYear + "-05-31";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Jun")) getYear = getYear + "-06-30";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Jul")) getYear = getYear + "-07-31";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Aug")) getYear = getYear + "-08-31";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Sep")) getYear = getYear + "-09-30";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Oct")) getYear = getYear + "-10-31";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Nov")) getYear = getYear + "-11-30";
                else if (ddlGenerateMonth.SelectedItem.Text.Substring(0, 3).Equals("Dec")) getYear = getYear + "-12-31";

                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("delete from Leave_YearlyEarnLeaveGeneration where GenerateDate ='" + getYear + "' AND IsSeperate='false'", sqlDB.connection);
                int i = cmd.ExecuteNonQuery();
                updateEarnLeaveDateAsPreviousEarnLeaveDateFromLeave_YearlyEarnLeaveGenerationInEmpCurrentStaus();
            }
            catch { }
        }

        private void updateEarnLeaveDateAsPreviousEarnLeaveDateFromLeave_YearlyEarnLeaveGenerationInEmpCurrentStaus()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select EmpId,Format(EarnLeaveDate,'dd-MM-yyyy') as EarnLeaveDate from Leave_LastEarnLeaveDateLog", dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[] getFiled = { "EarnLeaveDate" };  // start date as next earn leave generate 
                    string[] getValue = { convertDateTime.getCertainCulture(dt.Rows[i]["EarnLeaveDate"].ToString()).ToString() };
                    SQLOperation.forUpdateValue("Personnel_EmpCurrentStatus", getFiled, getValue, "EmpId", dt.Rows[i]["EmpId"].ToString(), sqlDB.connection);

                }
                lblMessage.InnerText = "success->Successfully Deleted";
                btnDelete.Visible = false;
            }
            catch { }
        }

        protected void txtGenerateMonth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlEarnLeaveYear.SelectedIndex < 1)
                {
                    lblMessage.InnerText = "warning-> Select Earn Leave Year.";
                    ddlEarnLeaveYear.Focus();
                    return;
                }
                string[] GDate = txtGenerateMonth.Text.Split('-');
                GDate[0]=GDate[2]+"-"+GDate[1]+"-"+GDate[0];

                if (checkAlreadyGenerated(ddlEarnLeaveYear.SelectedItem.Text) == false) return;
                        else
                        {
                            DataTable dt = new DataTable();
                            string sqlcmd = "";
                    if(ckbGenerateOn.Checked)
                        sqlcmd = ("SELECT SN, EmpId, EmpName, EmpCardNo, EmpTypeId, EmpType, DATEDIFF(day, DATEADD(DAY, 1, EarnLeaveDate),'" + GDate[0] + "')+1 AS TotalDays," +
                                " ActiveSalary FROM dbo.v_Personnel_EmpCurrentStatus WHERE DATEDIFF(day, DATEADD(DAY, 1, EarnLeaveDate),'" + GDate[0] + "')+1 >= 365 " +
                                "  AND CompanyId='" + ddlBranch.SelectedValue + "' AND EmpStatus in ('1','8') and IsActive=1  ");
                    else
                        sqlcmd = ("SELECT SN, EmpId, EmpName, EmpCardNo, EmpTypeId, EmpType, DATEDIFF(day, DATEADD(DAY, 1, EarnLeaveDate),'" + GDate[0] + "')+1 AS TotalDays," +
                               " ActiveSalary FROM dbo.v_Personnel_EmpCurrentStatus WHERE  CompanyId='" + ddlBranch.SelectedValue + "' and DATEDIFF(day, DATEADD(DAY, 1, EarnLeaveDate),'" + GDate[0] + "')+1 >= 1 AND EmpStatus in ('1','8') and IsActive=1  ");
                            txtquery.Text = sqlcmd;
                            sqlDB.fillDataTable(sqlcmd, dt);
                            gvEmpEarnLeaveList.DataSource = dt;
                            gvEmpEarnLeaveList.DataBind();

                            if (gvEmpEarnLeaveList.Rows.Count > 0)
                            {
                                hHeader.InnerText = "Earn leave List ->" + dt.Rows.Count;
                                ModalPopupExtender1.Show();
                                btnSubmit.Visible = true;
                                btnYes.Visible = false;
                                btnNo.Visible = false;
                                Panel1.Visible = true;
                            }
                            else 
                            {
                                hHeader.InnerText = "Earn Leave Already Generated! Do you want to Re-genearate?";
                                ModalPopupExtender1.Show();
                                btnSubmit.Visible = false;
                                btnYes.Visible = true;
                                btnNo.Visible = true;
                                Panel1.Visible = false;
                            }
                        }
                    //}
                    //else
                    //{
                    //    lblGeneratedMessage.Text = "Your selected month is not same of current system month !";
                    //    ModalPopupExtender2.Show();
                    //}
                
            }
            catch { }
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            try
            {

                string[] GDate = txtGenerateMonth.Text.Split('-');
                GDate[0] = GDate[2] + "-" + GDate[1] + "-" + GDate[0];
                string  Year = (int.Parse(ddlEarnLeaveYear.SelectedValue)-1).ToString();
                    DataTable dt = new DataTable();
                    string sqlcmd = "";

                    sqlcmd = ("SELECT SN, EmpId, EmpName, EmpCardNo, EmpTypeId, EmpType, DATEDIFF(day, DATEADD(DAY, 1,convert(date, '" + Year + "-'+ FORMAT(EarnLeaveDate,'MM-dd')) ),'" + GDate[0] + "')+1 AS TotalDays," +
                           " ActiveSalary FROM dbo.v_Personnel_EmpCurrentStatus WHERE DATEDIFF(day, DATEADD(DAY, 1,convert(date, '" + Year + "-'+ FORMAT(EarnLeaveDate,'MM-dd')) ),'" + GDate[0] + "')+1 >= 365 " +
                           "  AND CompanyId='" + ddlBranch.SelectedValue + "' AND EmpStatus in ('1','8') and IsActive=1  ");                    
                         txtquery.Text = sqlcmd;
                    sqlDB.fillDataTable(sqlcmd, dt);
                    gvEmpEarnLeaveList.DataSource = dt;
                    gvEmpEarnLeaveList.DataBind();

                    if (gvEmpEarnLeaveList.Rows.Count > 0)
                    {
                        hHeader.InnerText = "Earn leave List ->" + dt.Rows.Count;
                        ModalPopupExtender1.Show();
                        btnSubmit.Visible = true;
                        btnYes.Visible = false;
                        btnNo.Visible = false;
                        Panel1.Visible = true;
                    }                   
                
            

            }
            catch { }
        }

      
    }
}