using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComplexScriptingSystem;
using adviitRuntimeScripting;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Drawing;
using System.Web.SessionState;
using System.Threading;
using SigmaERP.classes;

namespace SigmaERP.payroll
{
    public partial class bonus_generation : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            lblMessage.InnerText = "";
            

            if (!IsPostBack)
            {
                Session["OPERATION_PROGRESS"] = 0;
                setPrivilege();
                if (!classes.commonTask.HasBranch())
                    ddlCompanyList.Enabled = false;
                ddlCompanyList.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.Payroll.loadBonusTypeWithRID(ddlSelectBonusMonth, ddlCompanyList.SelectedValue);
                 ViewState["__IsGerments__"] = classes.commonTask.IsGarments();
                
            }
        }


        private void setPrivilege()
        {
            try
            {
               
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                HttpContext.Current.Session["__GetUserID__"] = ViewState["__UserId__"] = getUserId;
                ViewState["__UserType__"] = getCookies["__getUserType__"].ToString();
                ViewState["__CompanyId__"] = getCookies["__CompanyId__"].ToString();

                if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin"))
                {
                    classes.commonTask.LoadBranch(ddlCompanyList);
                    return;
                }
                else
                {
                    classes.commonTask.LoadBranch(ddlCompanyList, ViewState["__CompanyId__"].ToString());
                    ddlCompanyList.Enabled = false;
                    //operation.Enabled = false;
                    //operation.CssClass = "";
                    DataTable dt = new DataTable();
                    sqlDB.fillDataTable("select * from UserPrivilege where PageName='bonus_generation.aspx' and UserId=" + getCookies["__getUserId__"].ToString() + "", dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (bool.Parse(dt.Rows[0]["GenerateAction"].ToString()).Equals(true))
                        {
                            //btnGeneration.CssClass = "css_btn";
                            //btnGeneration.Enabled = true;
                        }
                    }
                }
            }
            catch { }
        }

    
       
        protected void btnGeneration_Click(object sender, EventArgs e)
        {

            
            loadRunningEmpForBonus();
            imgLoading.Visible = false;
            lblMessage.InnerText = "success->Successfully bonus generated.";
            lblGenerationStatus.Text = "Generation Done";
        }
        DataTable dtRunningEmp = new DataTable();
        private void loadRunningEmpForBonus()
        {
            try
            {
                bonus_generation bg = new bonus_generation();
                string[] getBonusInfo =ddlSelectBonusMonth.SelectedValue.Split('_');
                bg.ViewState["__UserId__"] = Session["__GetUserID__"].ToString();
                // here getBonusInfo[0]=BId
                // and  getBonusInfo[1]=RId



                DataTable dtBonusMonthInfo = new DataTable();
                DataTable dtGetCalculationDate = new DataTable();

                string CompanyId = ddlCompanyList.SelectedValue;
                sqlDB.fillDataTable("select SlabType,Chosen,Percentage,BonusType,GenerateOn,EquivalentDays from Payroll_BonusMonthSetup where BId ='" + getBonusInfo[0] + "' and Chosen=1 ", dtBonusMonthInfo);  // get bonusn month info start 12 months then 11 months then 10 mpnths as sequqntioaly 
        
                sqlDB.fillDataTable("select convert(varchar(10),CalculationDate,120) as CalculationDate from v_Payroll_BonusSetup_DistinctRecord where BId ='" + getBonusInfo[0] + "' ", dtGetCalculationDate);

                string _empCardNo = "";
                if (txtCardNo.Text.Trim().Length > 0)
                {
                    string[] empCardno = txtCardNo.Text.Trim().Split(',');
                    foreach (string item in empCardno)
                    {
                        _empCardNo += ",'" + item + "'";
                    }
                    _empCardNo = " and SUBSTRING(EmpCardNo,10,6)  in(" + _empCardNo.Remove(0, 1) + ")";
                }

                string _empHoldCardNo = "";
                if (txtBonusHoldCardNo.Text.Trim().Length > 0)
                {
                    string[] empCardno = txtBonusHoldCardNo.Text.Trim().Split(',');
                    foreach (string item in empCardno)
                    {
                        _empHoldCardNo += ",'" + item + "'";
                    }
                    _empHoldCardNo = " and SUBSTRING(EmpCardNo,10,6) not in(" + _empHoldCardNo.Remove(0, 1) + ")";
                }

                string sqlquery = "";
                 if (getBonusInfo[1] != "0" && getBonusInfo[1] == "1")
                     sqlquery= "select EmpId,EmpCardNo,EmpName,SN,EmpType,EmpTypeId,EmpStatus,ActiveSalary,Convert(varchar(10)," +
                        "EmpJoiningDate,120) as EmpJoiningDate,BasicSalary,EmpPresentSalary,IsActive,CompanyId,RId  from v_Personnel_EmpCurrentStatus" +
                        " where  EmpStatus in('1','8')  AND ActiveSalary='true' AND IsActive='1' AND CompanyId='" + CompanyId + "' "+ _empCardNo+ _empHoldCardNo;
                else
                     sqlquery= "select  EmpId,EmpCardNo,EmpName,  SN,EmpType,EmpTypeId,EmpStatus,ActiveSalary,Convert(varchar(10)," +
                    "EmpJoiningDate,120) as EmpJoiningDate,BasicSalary,EmpPresentSalary,IsActive,CompanyId,RId  from v_Personnel_EmpCurrentStatus" +
                    " where  EmpStatus in('1','8')  AND ActiveSalary='true' AND IsActive='1' AND RId='" + getBonusInfo[1] + "' CompanyId='" + CompanyId + "'" + _empCardNo + _empHoldCardNo;

               
               sqlDB.fillDataTable(sqlquery, bg.dtRunningEmp = new DataTable());               

                

                if (bg.dtRunningEmp.Rows.Count < 1)
                {


                    return;
                }
                ClearYearlyBonusSheetByBonusType(getBonusInfo[0],_empCardNo);
                DataTable dt = new DataTable();
                DataTable dtWorkerAttInfo = new DataTable();
                for (int i = 0; i < bg.dtRunningEmp.Rows.Count; i++)
                {
                    double equivalentDays = 0;
                    double getBounus = 0;
                    double getTotalDays = 0;
                    
                   
                    

                    

                    string Percentage = "";

                    string BasicOrPresintSalary = "";
                    if (dtBonusMonthInfo.Rows[0]["GenerateOn"].ToString() == "Basic Salary") BasicOrPresintSalary = bg.dtRunningEmp.Rows[i]["BasicSalary"].ToString();
                    else BasicOrPresintSalary = bg.dtRunningEmp.Rows[i]["EmpPresentSalary"].ToString();

                  
                       

                    

                    sqlDB.fillDataTable("select DateDiff (day,'" + bg.dtRunningEmp.Rows[i]["EmpJoiningDate"].ToString() + "','" + dtGetCalculationDate.Rows[0]["CalculationDate"].ToString() + "')+1 as TotalDays", dt=new DataTable());
                    if (dt.Rows.Count > 0)
                    {

                        getTotalDays =double.Parse( dt.Rows[0]["TotalDays"].ToString());

                        for (int j = 0; j < dtBonusMonthInfo.Rows.Count; j++)
                        {
                            if (bool.Parse(dtBonusMonthInfo.Rows[j]["Chosen"].ToString()) == true && getTotalDays >= int.Parse(dtBonusMonthInfo.Rows[j]["EquivalentDays"].ToString()))
                            {

                                getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[j]["Percentage"].ToString()) / 100,2);
                                Percentage = dtBonusMonthInfo.Rows[j]["Percentage"].ToString();                                
                                break;
                            }
                        }
                        if (ckbIsProRataBasis.Checked && getBounus == 0 && getTotalDays > 0)
                        {
                            getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[dtBonusMonthInfo.Rows.Count - 1]["Percentage"].ToString()) / 100, 2);
                            Percentage = dtBonusMonthInfo.Rows[dtBonusMonthInfo.Rows.Count-1]["Percentage"].ToString();
                            equivalentDays = double.Parse(dtBonusMonthInfo.Rows[dtBonusMonthInfo.Rows.Count - 1]["EquivalentDays"].ToString());
                            if (getBounus>0)
                            getBounus = Math.Round((getBounus / equivalentDays) * getTotalDays,2);
                        }
                        saveBonusInfo(bg.dtRunningEmp.Rows[i]["SN"].ToString(), bg.dtRunningEmp.Rows[i]["EmpCardNo"].ToString(), bg.dtRunningEmp.Rows[i]["BasicSalary"].ToString(), getBounus, Percentage, bg.dtRunningEmp.Rows[i]["EmpPresentSalary"].ToString(), dtBonusMonthInfo.Rows[0]["GenerateOn"].ToString(), dt.Rows[0]["TotalDays"].ToString(), getBonusInfo[0], ddlCompanyList.SelectedValue, bg);

                    }
                }
            }
            catch { }
        
        }

        bool isGenerated;
        private static void saveBonusInfo(string setSN, string setEmpCardNo, string setBasicSalary, double setBonus, string Percentage, string PresentSalary, string generateOn, string TotalDays,string BId,string smonth,bonus_generation bg)
        {
            try
            {
                DataTable dtCertainEmp = new DataTable();
                sqlDB.fillDataTable("select EmpId,EmpTypeId,DptId,DsgId,SftId,CompanyId from v_Personnel_EmpCurrentStatus where SN ="+setSN+"",dtCertainEmp);
                try
                {
                    string[] getColumns = {"CompanyId","SftId","BID", "EmpId", "EmpCardNo", "EmpTypeId", "PresentSalary", "BasicSalary", "Percentage", "BonusAmount","DptId", "DsgId", "GenerateDate", "BonusType", "UserId", "GenerateOn", "TotalDays" };
                    string[] getValues = {dtCertainEmp.Rows[0]["CompanyId"].ToString(),dtCertainEmp.Rows[0]["SftId"].ToString(), BId,dtCertainEmp.Rows[0]["EmpId"].ToString(), setEmpCardNo, dtCertainEmp.Rows[0]["EmpTypeId"].ToString(),PresentSalary,
                                         setBasicSalary.ToString(),Percentage,setBonus.ToString(),dtCertainEmp.Rows[0]["DptId"].ToString(),dtCertainEmp.Rows[0]["DsgId"].ToString(),  
                                         DateTime.Now.ToString("yyyy-MM-dd"),smonth,
                                          bg.ViewState["__UserId__"] .ToString(),generateOn,TotalDays};
                    if (SQLOperation.forSaveValue("Payroll_YearlyBonusSheet", getColumns, getValues, sqlDB.connection) == true) bg.isGenerated = true;
                    
                }
                catch (Exception ex)
                {
                   // MessageBox.Show(ex.Message);
                }

            }
            catch { }
        
        }
        private static void ClearYearlyBonusSheetByBonusType(string BID, string CardNo)
        {
            try
            {
                string sqlCmd = "Delete Payroll_YearlyBonusSheet where BID="+ BID+ CardNo;
                CRUD.Execute(sqlCmd,sqlDB.connection);
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

        protected void ddlCompanyList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                classes.Payroll.loadBonusTypeWithRID(ddlSelectBonusMonth, ddlCompanyList.SelectedValue);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ProcessingHide();", true);
            }
            catch { }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object Operation(string smonth,string scompanyid)
        {
            HttpSessionState session = HttpContext.Current.Session;

            //Separate thread for long running operation
            
            ThreadPool.QueueUserWorkItem(delegate
            {


                try
                {
                    bonus_generation bg = new bonus_generation();
                    string[] getBonusInfo = smonth.Split('-');
                    bg.ViewState["__UserId__"] = session["__GetUserID__"].ToString();
                    // here getBonusInfo[0]=BId
                    // and  getBonusInfo[1]=RId



                    DataTable dtBonusMonthInfo = new DataTable();
                    DataTable dtGetCalculationDate = new DataTable();

                    string CompanyId = scompanyid;
                    sqlDB.fillDataTable("select SlabType,Chosen,Percentage,BonusType,GenerateOn from Payroll_BonusMonthSetup where BId ='" + getBonusInfo[0] + "'", dtBonusMonthInfo);  // get bonusn month info start 12 months then 11 months then 10 mpnths as sequqntioaly 
                    string [] GetBId = smonth.Split('-');
                    sqlDB.fillDataTable("select convert(varchar(11),CalculationDate,111) as CalculationDate from v_Payroll_BonusSetup_DistinctRecord where BId ='" +GetBId[0]+ "' ", dtGetCalculationDate);

                    if (getBonusInfo[1] != "0" && getBonusInfo[1]=="1")
                        sqlDB.fillDataTable("select distinct EmpId,EmpCardNo,EmpName, max(SN) as SN,EmpType,EmpTypeId,EmpStatus,ActiveSalary,Convert(varchar(11)," +
                            "EmpJoiningDate,111) as EmpJoiningDate,BasicSalary,EmpPresentSalary,IsActive,CompanyId,RId  from v_Personnel_EmpCurrentStatus" +
                            " group by EmpId,EmpCardNo,EmpName,SalaryType,EmpTypeId,EmpType,EmpStatus,ActiveSalary,EmpJoiningDate,BasicSalary,EmpPresentSalary,IsActive,CompanyId,RId" +
                            " having EmpStatus in('1','8')  AND ActiveSalary='true' AND IsActive='1' AND CompanyId='" + CompanyId + "' order by SN", bg.dtRunningEmp = new DataTable());
                    else
                        sqlDB.fillDataTable("select distinct EmpId,EmpCardNo,EmpName, max(SN) as SN,EmpType,EmpTypeId,EmpStatus,ActiveSalary,Convert(varchar(11)," +
                        "EmpJoiningDate,111) as EmpJoiningDate,BasicSalary,EmpPresentSalary,IsActive,CompanyId,RId  from v_Personnel_EmpCurrentStatus" +
                        " group by EmpId,EmpCardNo,EmpName,SalaryType,EmpTypeId,EmpType,EmpStatus,ActiveSalary,EmpJoiningDate,BasicSalary,EmpPresentSalary,IsActive,CompanyId,RId" +
                        " having EmpStatus in('1','8')  AND ActiveSalary='true' AND IsActive='1' AND RId='"+getBonusInfo[1]+"' CompanyId='" + CompanyId + "' order by SN", bg.dtRunningEmp = new DataTable());

                    if (bg.dtRunningEmp.Rows.Count < 1)
                    {

                        
                        return;
                    }
                    ClearYearlyBonusSheetByBonusType(getBonusInfo[0],"");
                    DataTable dt = new DataTable();
                    DataTable dtWorkerAttInfo = new DataTable();
                    for (int i = 0; i < bg.dtRunningEmp.Rows.Count; i++)
                    {

                        int getValue = 0;
                        if (i != 0) getValue = (100 * i / (bg.dtRunningEmp.Rows.Count-1));
                        //probar.Style.Add("width", getValue.ToString()+"%");


                        //probar.InnerHtml = getValue.ToString() + "%";   

                        System.Threading.Thread.Sleep(500);

                        string Percentage = "";

                        string BasicOrPresintSalary = "";
                        if (dtBonusMonthInfo.Rows[0]["GenerateOn"].ToString() == "Basic Salary") BasicOrPresintSalary = bg.dtRunningEmp.Rows[i]["BasicSalary"].ToString();
                        else BasicOrPresintSalary = bg.dtRunningEmp.Rows[i]["EmpPresentSalary"].ToString();


                        DataRow[] dr = bg.dtRunningEmp.Select("EmpCardNo='00003017'", "");

                        if (bg.dtRunningEmp.Rows[i]["EmpCardNo"].ToString() == "00003017")
                        {

                        }

                        sqlDB.fillDataTable("select DateDiff (day,'" + bg.dtRunningEmp.Rows[i]["EmpJoiningDate"].ToString() + "','" + dtGetCalculationDate.Rows[0]["CalculationDate"].ToString() + "') as TotalDays", dt);
                        if (dt.Rows.Count > 0)
                        {
                            double getBounus = 0;
                         
                                if (bool.Parse(dtBonusMonthInfo.Rows[0]["Chosen"].ToString()) == true && int.Parse(dt.Rows[0]["TotalDays"].ToString()) >= 360) //for 12 Months
                                {
                                    getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[0]["Percentage"].ToString()) / 100, 0);
                                    Percentage = dtBonusMonthInfo.Rows[0]["Percentage"].ToString();
                                }

                                else if (bool.Parse(dtBonusMonthInfo.Rows[1]["Chosen"].ToString()) == true && int.Parse(dt.Rows[0]["TotalDays"].ToString()) >= 330) //for 11 Months
                                {
                                    getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[1]["Percentage"].ToString()) / 100, 0);
                                    Percentage = dtBonusMonthInfo.Rows[1]["Percentage"].ToString();
                                }

                                else if (bool.Parse(dtBonusMonthInfo.Rows[2]["Chosen"].ToString()) == true && int.Parse(dt.Rows[0]["TotalDays"].ToString()) >= 300) //for 10 Months
                                {
                                    getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[2]["Percentage"].ToString()) / 100, 0);
                                    Percentage = dtBonusMonthInfo.Rows[2]["Percentage"].ToString();
                                }

                                else if (bool.Parse(dtBonusMonthInfo.Rows[3]["Chosen"].ToString()) == true && int.Parse(dt.Rows[0]["TotalDays"].ToString()) >= 270) //for 09 Months
                                {
                                    getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[3]["Percentage"].ToString()) / 100, 0);
                                    Percentage = dtBonusMonthInfo.Rows[3]["Percentage"].ToString();
                                }

                                else if (bool.Parse(dtBonusMonthInfo.Rows[4]["Chosen"].ToString()) == true && int.Parse(dt.Rows[0]["TotalDays"].ToString()) >= 240) //for 08 Months
                                {
                                    getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[4]["Percentage"].ToString()) / 100, 0);
                                    Percentage = dtBonusMonthInfo.Rows[4]["Percentage"].ToString();
                                }

                                else if (bool.Parse(dtBonusMonthInfo.Rows[5]["Chosen"].ToString()) == true && int.Parse(dt.Rows[0]["TotalDays"].ToString()) >= 210) //for 07 Months
                                {
                                    getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[5]["Percentage"].ToString()) / 100, 0);
                                    Percentage = dtBonusMonthInfo.Rows[5]["Percentage"].ToString();
                                }

                                else if (bool.Parse(dtBonusMonthInfo.Rows[6]["Chosen"].ToString()) == true && int.Parse(dt.Rows[0]["TotalDays"].ToString()) >= 180) //for 06 Months
                                {
                                    getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[6]["Percentage"].ToString()) / 100, 0);
                                    Percentage = dtBonusMonthInfo.Rows[6]["Percentage"].ToString();
                                }

                                else if (bool.Parse(dtBonusMonthInfo.Rows[7]["Chosen"].ToString()) == true && int.Parse(dt.Rows[0]["TotalDays"].ToString()) >= 150) //for 05 Months
                                {
                                    getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[7]["Percentage"].ToString()) / 100, 0);
                                    Percentage = dtBonusMonthInfo.Rows[7]["Percentage"].ToString();
                                }

                                else if (bool.Parse(dtBonusMonthInfo.Rows[8]["Chosen"].ToString()) == true && int.Parse(dt.Rows[0]["TotalDays"].ToString()) >= 120) //for 04 Months
                                {
                                    getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[8]["Percentage"].ToString()) / 100, 0);
                                    Percentage = dtBonusMonthInfo.Rows[8]["Percentage"].ToString();
                                }

                                else if (bool.Parse(dtBonusMonthInfo.Rows[9]["Chosen"].ToString()) == true && int.Parse(dt.Rows[0]["TotalDays"].ToString()) >= 90) //for 03 Months
                                {
                                    getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[9]["Percentage"].ToString()) / 100, 0);
                                    Percentage = dtBonusMonthInfo.Rows[9]["Percentage"].ToString();
                                }

                                else if (bool.Parse(dtBonusMonthInfo.Rows[10]["Chosen"].ToString()) == true && int.Parse(dt.Rows[0]["TotalDays"].ToString()) >= 60) //for 02 Months
                                {
                                    getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[10]["Percentage"].ToString()) / 100, 0);
                                    Percentage = dtBonusMonthInfo.Rows[10]["Percentage"].ToString();
                                }

                                else if (bool.Parse(dtBonusMonthInfo.Rows[11]["Chosen"].ToString()) == true && int.Parse(dt.Rows[0]["TotalDays"].ToString()) >= 10) //for 01 Months
                                {
                                    getBounus = Math.Round(double.Parse(BasicOrPresintSalary) * double.Parse(dtBonusMonthInfo.Rows[11]["Percentage"].ToString()) / 100, 0);
                                    Percentage = dtBonusMonthInfo.Rows[11]["Percentage"].ToString();
                                }
                                else
                                {
                                    getBounus = 0;      // if getBonus is 0 taka then not counted as get bounus
                                    Percentage = "0";  // if percentage is 0(%) then not counted as get bonus 
                                }
                            
                           
                            // if (getBounus > 0)
                            saveBonusInfo(bg.dtRunningEmp.Rows[i]["SN"].ToString(), bg.dtRunningEmp.Rows[i]["EmpCardNo"].ToString(), bg.dtRunningEmp.Rows[i]["BasicSalary"].ToString(), getBounus, Percentage, bg.dtRunningEmp.Rows[i]["EmpPresentSalary"].ToString(), dtBonusMonthInfo.Rows[0]["GenerateOn"].ToString(), dt.Rows[0]["TotalDays"].ToString(), getBonusInfo[0], scompanyid, bg);
                            //lbProcessingStatus.Items.Add("Processing completed of  " + dtRunningEmp.Rows[i]["EmpType"].ToString() + "  " +dtRunningEmp.Rows[i]["EmpName"].ToString()+"  Card No. " + dtRunningEmp.Rows[i]["EmpCardNo"].ToString() + "");
                            session["OPERATION_PROGRESS"] = getValue;
                            Thread.Sleep(1000);


                        }
                    }
                    //  System.Threading.Thread.Sleep(50);
                    //  ProgressBar1.Value = 0;
                    if (bg.isGenerated)
                    {
                        

                    }
                   
                }
                catch { }

                
            });

            return new { progress = 0 };
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static object OperationProgress()
        {
            int operationProgress = 0;

            if (HttpContext.Current.Session["OPERATION_PROGRESS"] != null)
                operationProgress = (int)HttpContext.Current.Session["OPERATION_PROGRESS"];

            return new { progress = operationProgress };
        }

    }
}