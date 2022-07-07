using adviitRuntimeScripting;
using ComplexScriptingSystem;
using SigmaERP.classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SigmaERP.payroll
{
    public partial class separation_pmt_sheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            this.btnaddall.Click += new System.EventHandler(this.btnaddall_Click);
            this.btnadditem.Click += new System.EventHandler(this.btnadditem_Click);
            this.btnremoveitem.Click += new System.EventHandler(this.btnremoveitem_Click);
            this.btnremoveall.Click += new System.EventHandler(this.btnremoveall_Click);
            if (!IsPostBack)
            {
                classes.commonTask.loadEmpTypeInRadioButtonList(rbEmpList);
              //  classes.commonTask.loadDivision(ddlDivisionName);
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                loadSeperationType();
            }

        }
        DataTable dtSetPrivilege;
        private void setPrivilege()
        {
            try
            {

                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                Session["__getUserId__"] = ViewState["__getUserId__"] = getUserId;
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                //------------load privilege setting inof from db------
                string[] AccessPermission = new string[0];
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForReport(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "separation_pmt_sheet.aspx", ddlCompanyList, WarningMessage, tblGenerateType, btnPreview);
                ViewState["__ReadAction__"] = AccessPermission[0];
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                loadMonthId(ViewState["__CompanyId__"].ToString());
                //-----------------------------------------------------
            }
            catch { }
        }

        private void loadSeperationType()
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("select EmpStatus,EmpStatusName from HRD_EmpStatus where EmpStatus not in('0','1','8')",dt);
                ddlSeperationType.DataValueField = "EmpStatus";
                ddlSeperationType.DataTextField = "EmpStatusName";
                ddlSeperationType.DataSource = dt;
                ddlSeperationType.DataBind();
                ddlSeperationType.Items.Insert(0,new ListItem (" "," "));
            }
            catch { }
        }
        private void btnaddall_Click(object sender, System.EventArgs e)
        {
            AddRemoveAll(lstAllDepartment, lstSelectedDepartment);
        }

        private void btnadditem_Click(object sender, System.EventArgs e)
        {
            AddRemoveItem(lstAllDepartment,lstSelectedDepartment);
    
        }

        private void btnremoveitem_Click(object sender, System.EventArgs e)
        {
            AddRemoveItem(lstSelectedDepartment, lstAllDepartment);
        }

        private void btnremoveall_Click(object sender, System.EventArgs e)
        {
            AddRemoveAll(lstSelectedDepartment, lstAllDepartment);
        }


        private void AddRemoveAll(ListBox aSource, ListBox aTarget)
        {

            try
            {

                foreach (ListItem item in aSource.Items)
                {
                    aTarget.Items.Add(item);
                }
                aSource.Items.Clear();

            }
            catch (Exception expException)
            {
                Response.Write(expException.Message);
            }

        }
        private void AddRemoveItem(ListBox aSource, ListBox aTarget)
        {

            ListItemCollection licCollection;

            try
            {

                licCollection = new ListItemCollection();
                for (int intCount = 0; intCount < aSource.Items.Count; intCount++)
                {
                    if (aSource.Items[intCount].Selected == true)
                        licCollection.Add(aSource.Items[intCount]);
                }

                for (int intCount = 0; intCount < licCollection.Count; intCount++)
                {
                    aSource.Items.Remove(licCollection[intCount]);
                    aTarget.Items.Add(licCollection[intCount]);
                }

            }
            catch (Exception expException)
            {
                Response.Write(expException.Message);
            }
            finally
            {
                licCollection = null;
            }

        }
  
        private void loadMonthId(string compnayId)// Updated By Suman.
        {
            try
            {
                DataTable dt = new DataTable();
                sqlDB.fillDataTable("Select distinct MonthName,Format(FromDate,'MMM-yyyy') as YearMonth,MonthId From tblMonthSetup  where CompanyId='" + compnayId + "' order by MonthId desc", dt);
                ddlMonthId.DataTextField = "YearMonth";
                ddlMonthId.DataValueField = "MonthName";
                ddlMonthId.DataSource = dt;
                ddlMonthId.DataBind();
                ddlMonthId.Items.Insert(0, new ListItem(" ", " "));
            }
            catch { }
        }    

     

        protected void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                string setPredicate = "";
                for (byte b = 0; b < lstSelectedDepartment.Items.Count; b++)
                {
                    if (b == 0 && b == lstSelectedDepartment.Items.Count - 1)
                    {
                        setPredicate = "in('" + lstSelectedDepartment.Items[b].Text + "')";
                    }
                    else if (b == 0 && b != lstSelectedDepartment.Items.Count - 1)
                    {
                        setPredicate += "in ('" + lstSelectedDepartment.Items[b].Text + "'";
                    }
                    else if (b != 0 && b == lstSelectedDepartment.Items.Count - 1)
                    {
                        setPredicate += ",'" + lstSelectedDepartment.Items[b].Text + "')";
                    }
                    else setPredicate += ",'" + lstSelectedDepartment.Items[b].Text + "'";
                }
                DataTable dt = new DataTable();

                sqlDB.fillDataTable("select EmpName,EmpCardNo,convert(varchar(11),EmpJoiningDate,105) as EmpJoiningDate,DaysInMonth,WeekendHoliday,CasualLeave,SickLeave,"+
                    "AnnualLeave,FestivalHoliday,AbsentDay,PresentDay,PayableDays,BasicSalary,HouseRent,MedicalAllownce,ConvenceAllownce,FoodAllownce,EmpPresentSalary,"+
                    "AbsentDeduction,Payable,TotalOTHour,OTRate,TotalOTAmount,LunchAllowance,AttendanceBonus,AdvanceDeduction,LoanDeduction,NetPayable,Stampdeduct,"+
                    "TotalSalary,DsgName,LnId,LnCode,FId,FCode,GrpId,GrpName,EmpTypeId,EmpType,DId,DName,Month,Year,EmpId,EmpStatus,EmpStatusName,DptName,DptNameBn,"+
                    "GrdName,OthersAllownce,ExtraOTHour,TotalOverTime,TotalSalaryWithAllOT,DptId,EmpNameBn,DsgNameBn,GrdNameBangla,SalaryCount from v_MonthlySalarySheet where Month='" + ddlMonthId.SelectedItem.ToString().Substring(5, 2) + "' AND Year = '" + ddlMonthId.SelectedItem.ToString().Substring(0, 4) + "' AND EmpType='Worker' AND  " +
                    "  And DptName " + setPredicate + " AND EmpStatus ='" + ddlSeperationType.SelectedValue.ToString() + "' order by DptId,LnId,EmpCardNo ", dt = new DataTable());
                /*
                sqlDB.fillDataTable("SELECT   v_MonthlySalarySheet  .  EmpName  ,EmpNameBn,DsgNameBn,   v_MonthlySalarySheet  .  EmpCardNo  ,DptName,DptNameBn, "+
                "   v_MonthlySalarySheet  .  DsgName  ,   v_MonthlySalarySheet  .  LnCode  ,   v_MonthlySalarySheet  .  FCode  ,   v_MonthlySalarySheet  .  GrpName  ,"+
                "   v_MonthlySalarySheet  .  GrdName ,GrdNameBangla ,   convert(varchar(11),v_MonthlySalarySheet.EmpJoiningDate,105) as EmpJoiningDate , "+
                "  v_MonthlySalarySheet  .  DaysInMonth  ,   v_MonthlySalarySheet  .  WeekendHoliday  ,   v_MonthlySalarySheet  .  CasualLeave  ,  "+
                " v_MonthlySalarySheet  .  SickLeave  ,   v_MonthlySalarySheet  .  AnnualLeave  ,   v_MonthlySalarySheet  .  FestivalHoliday  ,  "+
                " v_MonthlySalarySheet  .  AbsentDay  ,   v_MonthlySalarySheet  .  PresentDay  ,   v_MonthlySalarySheet  .  PayableDays  ,   "+
                "v_MonthlySalarySheet  .  BasicSalary  ,   v_MonthlySalarySheet  .  HouseRent  ,   v_MonthlySalarySheet  .  MedicalAllownce  , "+
                "  v_MonthlySalarySheet  .  ConvenceAllownce  ,   v_MonthlySalarySheet  .  FoodAllownce  ,   v_MonthlySalarySheet  .  EmpPresentSalary  ,  "+
                " v_MonthlySalarySheet  .  AbsentDeduction  ,   v_MonthlySalarySheet  .  Payable  ,   v_MonthlySalarySheet  .  TotalOTHour  ,  "+
                " v_MonthlySalarySheet  .  OTRate  ,   v_MonthlySalarySheet  .  TotalOTAmount  ,   v_MonthlySalarySheet  .  AttendanceBonus  , "+
                "  v_MonthlySalarySheet  .  AdvanceDeduction  ,   v_MonthlySalarySheet  .  LoanDeduction  ,   v_MonthlySalarySheet  .  NetPayable  , "+
                "  v_MonthlySalarySheet  .  Stampdeduct  ,   v_MonthlySalarySheet  .  TotalSalary,LnId,DptId  "+
                "FROM  v_MonthlySalarySheet where Month='" + ddlMonthId.SelectedItem.ToString().Substring(5, 2) + "' AND "+
                "Year = '" + ddlMonthId.SelectedItem.ToString().Substring(0, 4) + "' AND EmpType='Worker' AND"+
                " DName='" + ddlDivisionName.SelectedItem.ToString() + "' And DptName "+setPredicate+" AND EmpStatus ='"+ddlSeperationType.SelectedValue.ToString()+"' Group By v_MonthlySalarySheet  .  EmpName  ,   v_MonthlySalarySheet  .  EmpCardNo  ,"+
                "EmpNameBn,DsgNameBn,   v_MonthlySalarySheet  .  DsgName  ,   v_MonthlySalarySheet  .  LnCode  ,   v_MonthlySalarySheet  .  FCode  ,   "+
                "v_MonthlySalarySheet  .  GrpName  ,   v_MonthlySalarySheet  .  GrdName  ,GrdNameBangla,  v_MonthlySalarySheet.EmpJoiningDate,   "+
                "v_MonthlySalarySheet  .  DaysInMonth  ,   v_MonthlySalarySheet  .  WeekendHoliday  ,   v_MonthlySalarySheet  .  CasualLeave  ,  "+
                " v_MonthlySalarySheet  .  SickLeave  ,   v_MonthlySalarySheet  .  AnnualLeave  ,   v_MonthlySalarySheet  .  FestivalHoliday  ,  "+
                " v_MonthlySalarySheet  .  AbsentDay  ,   v_MonthlySalarySheet  .  PresentDay  ,   v_MonthlySalarySheet  .  PayableDays  ,  "+
                " v_MonthlySalarySheet  .  BasicSalary  ,   v_MonthlySalarySheet  .  HouseRent  ,   v_MonthlySalarySheet  .  MedicalAllownce  ,"+
                "   v_MonthlySalarySheet  .  ConvenceAllownce  ,   v_MonthlySalarySheet  .  FoodAllownce  ,   v_MonthlySalarySheet  .  EmpPresentSalary  , "+
                "  v_MonthlySalarySheet  .  AbsentDeduction  ,   v_MonthlySalarySheet  .  Payable  ,   v_MonthlySalarySheet  .  TotalOTHour  , "+
                "  v_MonthlySalarySheet  .  OTRate  ,   v_MonthlySalarySheet  .  TotalOTAmount  ,   v_MonthlySalarySheet  .  AttendanceBonus  ,"+
                "   v_MonthlySalarySheet  .  AdvanceDeduction  ,   v_MonthlySalarySheet  .  LoanDeduction  ,   v_MonthlySalarySheet  .  NetPayable  , "+
                "  v_MonthlySalarySheet  .  Stampdeduct  ,   v_MonthlySalarySheet.TotalSalary,LnId,DptId,DptName,DptNameBn"+
                " Order By DptId,LnId,EmpCardNo ", dt = new DataTable());
                  */

                Session["__WorkerSalarySheet__"] = dt;

                string monthName = "";
                if (rbLanguage.SelectedValue.ToString() == "0") Session["__Language__"] = "Bangal";
                else Session["__Language__"] = "English";
               
                sqlDB.fillDataTable("select MonthName from HRD_MonthNameBangla where MonthId='" +ddlMonthId.SelectedItem.ToString().Substring(5,2) + "'", dt = new DataTable());
                monthName = dt.Rows[0]["MonthName"].ToString();
                monthName += "-" + ddlMonthId.SelectedItem.ToString().Substring(0,4);
               
                
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=WorkerSeperationSheet-" + monthName + "');", true);  //Open New Tab for Sever side code
            }
            catch { }
        }
    }
}