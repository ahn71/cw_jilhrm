using adviitRuntimeScripting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using ComplexScriptingSystem;
using System.Data.SqlClient;
using SigmaERP.classes;

namespace SigmaERP.personnel
{
    public partial class monthly_manpower : System.Web.UI.Page
    {
        string CompanyID = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();

            if (!IsPostBack)
            {
                classes.commonTask.LoadEmpTypeWithAll(rblEmpType);
                txtDate.Text = "01-" + "01-" + DateTime.Now.Year.ToString();
                txtFromDate.Text = "31-" + "12-" + DateTime.Now.Year.ToString();                
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyy.Enabled = false;        
            }
        }
        private void setPrivilege()
        {
            try
            {
               
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                string[] AccessPermission = new string[0];
                //System.Web.UI.HtmlControls.HtmlTable a = tblGenerateType;
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "monthly_manpower.aspx", ddlCompanyy, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];

                ddlCompanyy.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.commonTask.LoadInitialShift(ddlShift, ddlCompanyy.SelectedValue);
                addAllTextInShift();
              // classes.commonTask.LoadDepartment(ddlCompanyy.SelectedValue, lstAll);
            }
            catch { }

        }
        private void addAllTextInShift()
        {
            ddlShift.Items.RemoveAt(0);
            if (ddlShift.Items.Count > 1)
                ddlShift.Items.Insert(0, new ListItem("All", "00"));
        }

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Delete From MonthlyManpower", sqlDB.connection);
                cmd.ExecuteNonQuery();
                DataTable dtTotal = new DataTable();
              
                string[] FromDate = txtDate.Text.Split('-');
                string[] ToDate = txtFromDate.Text.Split('-');
                DateTime FDate = new DateTime(int.Parse(FromDate[2]), int.Parse(FromDate[1]), int.Parse(FromDate[0]));
                DateTime TDate = new DateTime(int.Parse(ToDate[2]), int.Parse(ToDate[1]), int.Parse(ToDate[0]));
                int i = 0;
                string MonthName = "", TotalManpower = "", NMale = "", NFemale = "", SMale = "", SFemale = "", PMale = "", PFemale = "", RMale = "", RFemale = "";
                while (FDate < TDate)
                {
                    if (FDate > DateTime.Now) break;
                    if (i == 0)
                    {
                        DateTime tt = new DateTime(FDate.Year, FDate.Month, 1);
                        DateTime ts = tt.AddMonths(1);
                        DateTime tm = ts.AddDays(-1);
                        string TD = tm.ToString("yyyy-MM-dd");
                        MonthName = FDate.ToString("MMMM", CultureInfo.InvariantCulture)+"-"+FDate.Year;

                        sqlDB.fillDataTable("Select Count(EmpId) as EmpId,ATTDate From tblAttendanceRecord where ATTDate='" + FromDate[2] + "-" + FromDate[1] + "-01' group by ATTDate order by ATTDate", dtTotal);
                        if (dtTotal.Rows.Count == 0) TotalManpower = "0";
                        else
                        {
                            if (dtTotal.Rows.Count == 1)
                            {
                                if (dtTotal.Rows[0]["EmpId"].ToString() == "") TotalManpower = "0";
                                else TotalManpower = dtTotal.Rows[0]["EmpId"].ToString();
                            }
                            else
                            {
                                if (dtTotal.Rows[1]["EmpId"].ToString() == "") TotalManpower = "0";
                                else TotalManpower = dtTotal.Rows[1]["EmpId"].ToString();
                            }
                        }
                        sqlDB.fillDataTable("Select Distinct Sum(Male) as Male,Sum(Female) as Female From v_ManpowerProcess where EmpJoiningDate between '" + FromDate[2] + "-" + FromDate[1] + "-" + FromDate[0] + "' and '" + TD + "'", dtTotal = new DataTable());
                        if (dtTotal.Rows[0]["Male"].ToString() == "") NMale = "0";
                        else NMale = dtTotal.Rows[0]["Male"].ToString();
                        if (dtTotal.Rows[0]["Female"].ToString() == "") NFemale = "0";
                        else NFemale = dtTotal.Rows[0]["Female"].ToString();
                        sqlDB.fillDataTable("Select Distinct Sum(Male) as Male,Sum(Female) as Female From v_ManpowerProcess where EffectiveDateSeparation between '" + FromDate[2] + "-" + FromDate[1] + "-" + FromDate[0] + "' and '" + TD + "'", dtTotal = new DataTable());
                        if (dtTotal.Rows[0]["Female"].ToString() == "") SMale = "0";
                        else SMale = dtTotal.Rows[0]["Male"].ToString();
                        if (dtTotal.Rows[0]["Female"].ToString() == "") SFemale = "0";
                        else SFemale = dtTotal.Rows[0]["Female"].ToString();
                        sqlDB.fillDataTable("Select Distinct Sum(Male) as Male,Sum(Female) as Female From v_ManpowerProcess where EffectiveDate between '" + FromDate[2] + "-" + FromDate[1] + "-" + FromDate[0] + "' and '" + TD + "'", dtTotal = new DataTable());
                        if (dtTotal.Rows[0]["Female"].ToString() == "") RMale = "0";
                        else RMale = dtTotal.Rows[0]["Male"].ToString();
                        if (dtTotal.Rows[0]["Female"].ToString() == "") RFemale = "0";
                        else RFemale = dtTotal.Rows[0]["Female"].ToString();
                        sqlDB.fillDataTable("Select Distinct Sum(Male) as Male,Sum(Female) as Female From v_ManpowerProcess where EffectiveMonth ='" + FDate.ToString("MM")+"-"+FDate.Year+ "'", dtTotal = new DataTable());
                        if (dtTotal.Rows[0]["Female"].ToString() == "") PMale = "0";
                        else PMale = dtTotal.Rows[0]["Male"].ToString();
                        if (dtTotal.Rows[0]["Female"].ToString() == "") PFemale = "0";
                        else PFemale = dtTotal.Rows[0]["Female"].ToString();


                    }
                    else
                    {
                        DateTime tt = new DateTime(FDate.Year, FDate.Month, 1);
                        DateTime ts = tt.AddMonths(1);
                        DateTime tm = ts.AddDays(-1);
                        string FD = tt.ToString("yyyy-MM-dd");
                        string TD = tm.ToString("yyyy-MM-dd");
                        MonthName = FDate.ToString("MMMM", CultureInfo.InvariantCulture) + "-" + FDate.Year;

                        sqlDB.fillDataTable("Select Count(EmpId) as EmpId,ATTDate From tblAttendanceRecord where ATTDate='" + FD + "' group by ATTDate order by ATTDate", dtTotal);
                        if (dtTotal.Rows.Count == 0) TotalManpower = "0";
                        else
                        {
                            if (dtTotal.Rows.Count == 1)
                            {
                                if (dtTotal.Rows[0]["EmpId"].ToString() == "") TotalManpower = "0";
                                else TotalManpower = dtTotal.Rows[0]["EmpId"].ToString();
                            }
                            else
                            {
                                if (dtTotal.Rows[1]["EmpId"].ToString() == "") TotalManpower = "0";
                                else TotalManpower = dtTotal.Rows[1]["EmpId"].ToString();
                            }
                        }
                        sqlDB.fillDataTable("Select Distinct Sum(Male) as Male,Sum(Female) as Female From v_ManpowerProcess where EmpJoiningDate between '" + FD + "' and '"+TD+"'", dtTotal = new DataTable());
                        if (dtTotal.Rows[0]["Male"].ToString() == "") NMale = "0";
                        else NMale = dtTotal.Rows[0]["Male"].ToString();
                        if (dtTotal.Rows[0]["Female"].ToString() == "") NFemale = "0";
                        else NFemale = dtTotal.Rows[0]["Female"].ToString();
                        sqlDB.fillDataTable("Select Distinct Sum(Male) as Male,Sum(Female) as Female From v_ManpowerProcess where EffectiveDateSeparation between '" + FD + "' and '" + TD + "'", dtTotal = new DataTable());
                        if (dtTotal.Rows[0]["Female"].ToString() == "") SMale = "0";
                        else SMale = dtTotal.Rows[0]["Male"].ToString();
                        if (dtTotal.Rows[0]["Female"].ToString() == "") SFemale = "0";
                        else SFemale = dtTotal.Rows[0]["Female"].ToString();
                        sqlDB.fillDataTable("Select Distinct Sum(Male) as Male,Sum(Female) as Female From v_ManpowerProcess where EffectiveDate between '" + FD + "' and '" + TD + "'", dtTotal = new DataTable());
                        if (dtTotal.Rows[0]["Female"].ToString() == "") RMale = "0";
                        else RMale = dtTotal.Rows[0]["Male"].ToString();
                        if (dtTotal.Rows[0]["Female"].ToString() == "") RFemale = "0";
                        else RFemale = dtTotal.Rows[0]["Female"].ToString();
                        sqlDB.fillDataTable("Select Distinct Sum(Male) as Male,Sum(Female) as Female From v_ManpowerProcess where EffectiveMonth ='" + FDate.ToString("MM") + "-" + FDate.Year + "'", dtTotal = new DataTable());
                        if (dtTotal.Rows[0]["Female"].ToString() == "") PMale = "0";
                        else PMale = dtTotal.Rows[0]["Male"].ToString();
                        if (dtTotal.Rows[0]["Female"].ToString() == "") PFemale = "0";
                        else PFemale = dtTotal.Rows[0]["Female"].ToString();


                    }
                    try
                    {
                        string[] getColumns = { "MonthName", "TotalManpower", "NMale", "NFemale", "SMale", "SFemale", "RMale", "RFemale", "PTotalManpower", "PMale", "PFemale" };
                        string[] getValues = { MonthName,TotalManpower,NMale,NFemale,SMale,SFemale,RMale,RFemale,"0",PMale,PFemale };
                        if (SQLOperation.forSaveValue("MonthlyManpower", getColumns, getValues, sqlDB.connection) == true)
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.InnerText = ex.Message;
                    }
                    i++;
                    FDate = FDate.AddMonths(1);
                }
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select * From MonthlyManpower ", dt);
                if (dt.Rows.Count > 0)
                {
                    Session["__ManthlyManPower__"] = dt;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=ManthlyManPower');", true);  //Open New Tab for Sever side code
                }
                else
                {
                    lblMessage.InnerText = "warning->No Data Found";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
                }
            }
            catch { }
        }

        protected void ddlCompanyy_SelectedIndexChanged(object sender, EventArgs e)
        {
            CompanyID = (ddlCompanyy.SelectedValue.ToString().Equals("0000")) ? ViewState["__CompanyId__"].ToString() : ddlCompanyy.SelectedValue;
            classes.commonTask.LoadInitialShift(ddlShift, ddlCompanyy.SelectedValue);
            addAllTextInShift();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "loadcardNo();", true);
        }
    }
}