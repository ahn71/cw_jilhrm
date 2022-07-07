using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComplexScriptingSystem;
using adviitRuntimeScripting;
using System.Data;
using System.Data.SqlClient;

namespace SigmaERP.personnel
{
    public partial class common_increment : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            btnUndo.CssClass = "";
           // btnUndo.Enabled = false;
            if (!IsPostBack)
            {
                loadEmpType();
                loadBasictAndOtheresPercentage();
            }
        }

        private void loadEmpType()
        {
            try
            {
                rbEmployeeType.Items.Clear();
                SQLOperation.selectBySetCommandInDatatable("select EmpTypeId,EmpType from HRD_EmployeeType",dt=new DataTable (),sqlDB.connection);
               
                rbEmployeeType.Items.Insert(0, new ListItem("All","50"));
                for (byte b = 0; b < dt.Rows.Count; b++)
                {
                   // string getType = (dt.Rows[b]["EmpType"].ToString().ToLower().Equals("others")) ? "All" : dt.Rows[b]["EmpType"].ToString();
                    rbEmployeeType.Items.Insert(b+1, new ListItem(dt.Rows[b]["EmpType"].ToString(), dt.Rows[b]["EmpTypeId"].ToString()));
                }
                
            }
            catch { }
        }

        private void loadBasictAndOtheresPercentage()
        {
            try
            {
                sqlDB.fillDataTable("select HouseRent,BasicAllowance,OthersAllowance from HRD_AllownceSetting where AllownceId=(select max(AllownceId) from HRD_AllownceSetting)", dt = new DataTable());
                hfBasicPercentage.Value = dt.Rows[0]["BasicAllowance"].ToString();
                hfOthersAllowance.Value = dt.Rows[0]["OthersAllowance"].ToString();
                hfHouseRentPercentage.Value = dt.Rows[0]["HouseRent"].ToString();
            }
            catch { }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            generateSalaryCommonIncrement();
        }

        private void generateSalaryCommonIncrement()
        {
            try
            {
                byte empTypeId = byte.Parse(rbEmployeeType.SelectedValue.ToString());
                if (checkIsSelect() == false)
                {
                    lblMessage.InnerText = "warning->Please Select Employee Type.";
                    return;
                }
                DataTable dtDetails;
                if (rbEmployeeType.SelectedItem.Text.Equals("All"))
                {
                    sqlDB.fillDataTable("select EmpCardNo,MAX(SN) As SN,EmpType,IsActive,EmpStatus   from v_Personnel_EmpCurrentStatus group by EmpCardNo,EmpType,IsActive,EmpStatus having IsActive='1' AND EmpStatus in('1','8') order by SN", dt = new DataTable());
                }
                else sqlDB.fillDataTable("select EmpCardNo,MAX(SN) As SN,EmpTypeId,IsActive,EmpStatus  from v_Personnel_EmpCurrentStatus group by EmpCardNo,EmpTypeId,IsActive,EmpStatus having EmpTypeId =" + empTypeId + " AND IsActive='1' AND EmpStatus in('1','8') order by SN", dt = new DataTable());
                if (dt.Rows.Count > 0) enteredDataInCommonIncrement();

                for (int r = 0; r < dt.Rows.Count; r++)
                {

                    sqlDB.fillDataTable("select EmpId,EmpCardNo,EmpTypeId,SalaryType,EmpPresentSalary,IncrementAmount,BasicSalary,MedicalAllownce,FoodAllownce," +
                    "ConvenceAllownce,HouseRent,DId,DptId,LnId,FId,GrpId,DsgId,EmpStatus,GrdName,OthersAllownce,HolidayAllownce,TiffinAllownce,NightAllownce,AttendanceBonus,"+
                    "LunchAllownce,LunchCount,Format(DateofUpdate,'dd-MM-yyyy') as DateofUpdate,TypeOfChange,EffectiveMonth,"+
                    "OrderRefNo, convert(varchar(11),OrderRefDate,105)as OrderRefDate, Remarks,ActiveSalary, Format (EarnLeaveDate,'dd-MM-yyyy') as EarnLeaveDate, IsActive, "+
                    "OrderRefDate,EffectiveMonth,DsgName "+
                    "from v_Personnel_EmpCurrentStatus where SN=" + dt.Rows[r]["SN"].ToString() + "", dtDetails = new DataTable());
                 

                    double getPresentGross;
                    double getIncrementAmount;
                    double getNewBasic = 0;
                    double getOthersAllowance = 0;

                    if (dtDetails.Rows[0]["SalaryType"].Equals("Gross"))
                    {
                        getIncrementAmount = Math.Round((double.Parse(dtDetails.Rows[0]["EmpPresentSalary"].ToString()) * (double.Parse(txtPercentage.Text.Trim()))) / 100,0);
                        
                        getPresentGross = Math.Round((double.Parse(dtDetails.Rows[0]["EmpPresentSalary"].ToString()) + getIncrementAmount), 0);
                       
                        getOthersAllowance = Math.Round((getPresentGross * double.Parse (hfOthersAllowance.Value.ToString()))/100,0);

                        getNewBasic = Math.Round(getPresentGross -getOthersAllowance,2);
                        
                    }
                    else
                    {

                        getIncrementAmount = Math.Round((double.Parse(dtDetails.Rows[0]["EmpPresentSalary"].ToString()) * (double.Parse(txtPercentage.Text.Trim()))) / 100, 0);

                        getPresentGross = Math.Round((double.Parse(dtDetails.Rows[0]["EmpPresentSalary"].ToString()) + getIncrementAmount), 0);
                        
                        double getMedicalAllownce = Math.Round(double.Parse(dtDetails.Rows[0]["MedicalAllownce"].ToString()), 0);
                        double getConvenceAllownce = Math.Round(double.Parse(dtDetails.Rows[0]["ConvenceAllownce"].ToString()), 0);
                        double getFoods = Math.Round(double.Parse(dtDetails.Rows[0]["FoodAllownce"].ToString()), 0);
                        

                        getNewBasic = Math.Round(getPresentGross - (getMedicalAllownce + getFoods + getConvenceAllownce),2);

                        getNewBasic = Math.Round(getNewBasic / (double.Parse(hfBasicPercentage.Value.ToString())),0);

                        double getHouseRent = Math.Round(getNewBasic*(double.Parse(hfHouseRentPercentage.Value.ToString())) / 100, 0);

                        getPresentGross = Math.Round(getMedicalAllownce + getFoods + getHouseRent + getConvenceAllownce + getNewBasic, 0);


                    }


                    //__________________________________Entered Data in emp current status table______________________________________

                    try
                    {
                        string EarnLeaveDate=(dtDetails.Rows[0]["EarnLeaveDate"].ToString().Trim().Length >=8) ?dtDetails.Rows[0]["EarnLeaveDate"].ToString():"01-01-2050";
                        string[] getColumns = { "EmpId", "EmpCardNo", "EmpTypeId", "SalaryType", "EmpPresentSalary", "IncrementAmount", "BasicSalary",
                                                  "MedicalAllownce", "FoodAllownce", "ConvenceAllownce", "HouseRent", "DId", "DptId", "LnId", "FId", 
                                                  "GrpId", "DsgId", "EmpStatus", "GrdName", "OthersAllownce", "HolidayAllownce", "TiffinAllownce",
                                                  "NightAllownce", "AttendanceBonus", "LunchAllownce", "LunchCount", "DateofUpdate", "TypeOfChange",
                                                  "EffectiveMonth", "OrderRefNo", "Remarks", "ActiveSalary", "EarnLeaveDate", "IsActive" };

                        string[] getValues = { dtDetails.Rows[0]["EmpId"].ToString(), dtDetails.Rows[0]["EmpCardNo"].ToString(), dtDetails.Rows[0]["EmpTypeId"].ToString(),
                                               dtDetails.Rows[0]["SalaryType"].ToString(),getPresentGross.ToString(), getIncrementAmount.ToString(),getNewBasic.ToString(),
                                               dtDetails.Rows[0]["MedicalAllownce"].ToString(),dtDetails.Rows[0]["FoodAllownce"].ToString(),dtDetails.Rows[0]["ConvenceAllownce"].ToString(),
                                               dtDetails.Rows[0]["HouseRent"].ToString(),dtDetails.Rows[0]["DId"].ToString(),dtDetails.Rows[0]["DptId"].ToString(),dtDetails.Rows[0]["LnId"].ToString(),
                                               dtDetails.Rows[0]["FId"].ToString(), dtDetails.Rows[0]["GrpId"].ToString(),dtDetails.Rows[0]["DsgId"].ToString(),dtDetails.Rows[0]["EmpStatus"].ToString(),
                                               dtDetails.Rows[0]["GrdName"].ToString(),dtDetails.Rows[0]["OthersAllownce"].ToString(),dtDetails.Rows[0]["HolidayAllownce"].ToString(),dtDetails.Rows[0]["TiffinAllownce"].ToString(),
                                               dtDetails.Rows[0]["NightAllownce"].ToString(),dtDetails.Rows[0]["AttendanceBonus"].ToString(),dtDetails.Rows[0]["LunchAllownce"].ToString(),dtDetails.Rows[0]["LunchCount"].ToString(),
                                               convertDateTime.getCertainCulture(DateTime.Now.ToString("dd-MM-yyyy")).ToString(),"i",txtMonth.Text," ",txtRemarks.Text,"0", convertDateTime.getCertainCulture(EarnLeaveDate).ToString(),
                                               "0"};
                        if (SQLOperation.forSaveValue("Personnel_EmpCurrentStatus", getColumns, getValues,sqlDB.connection) == true)
                        {
                            findLastSalaryIncrement(dt.Rows[r]["SN"].ToString());
                            savePersonalCurrentStatusAsSalaryIncrementLog(dt.Rows[r]["SN"].ToString(), dt.Rows[r]["EmpCardNo"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.InnerText = "error->" + ex.Message;
                    }

                    //________________________________________________________________________________________________________________
                }
                if (dt.Rows.Count > 0)
                {
                    btnUndo.Enabled = true;
                    btnUndo.CssClass = "back_button";
                    lblMessage.InnerText = "success->Successfully Incremented";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        private bool checkIsSelect()
        {
            for (byte b = 0; b < rbEmployeeType.Items.Count; b++)
            {
                if (rbEmployeeType.Items[b].Selected==true) return true;
            }
            return false;
        }

        DataTable dtCertainEmpInfo;
        private void findLastSalaryIncrement(string SN)   // for get previous record that is any type such as p,i,s
        {
            try
            {
                lblMessage.InnerText = "";
                sqlDB.fillDataTable("select EmpId,EmpTypeId,EmpName,SalaryType,EmpPresentSalary,TypeOfChange,IncrementAmount,convert(varchar(11),DateofUpdate,105)as DateofUpdate,BasicSalary,MedicalAllownce,FoodAllownce,HouseRent,ConvenceAllownce,OthersAllownce,OrderRefNo,OrderRefDate,EffectiveMonth,DId,DptId,DsgId,GrdName,LnId,FId,GrpId,LunchCount,convert(varchar(11),EarnLeaveDate,105) as EarnLeaveDate,HolidayAllownce,TiffinAllownce,NightAllownce,AttendanceBonus,LunchAllownce from v_Personnel_EmpCurrentStatus where SN=" + SN + "", dtCertainEmpInfo = new DataTable());
            }
            catch { }
        }

        private void savePersonalCurrentStatusAsSalaryIncrementLog(string SN,string EmpCardNo)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("insert into Personnel_EmpCurrentStatusSalaryIncrementLog (SN,EmpId, EmpTypeId, SalaryType, EmpPresentSalary, IncrementAmount, BasicSalary, MedicalAllownce, HouseRent, ConvenceAllownce, HolidayAllownce, TiffinAllownce, NightAllownce, AttendanceBonus, DateofUpdate, DId, DptId, LnId, FId, GrpId, DsgId, EmpCardNo, GrdName, TypeOfChange, EffectiveMonth, OrderRefNo, OrderRefDate, EmpStatus, Remarks,ActiveSalary,FoodAllownce,OthersAllownce,LunchCount,EarnLeaveDate,LunchAllownce) values " +
                    "(@SN, @EmpId, @EmpTypeId, @SalaryType, @EmpPresentSalary, @IncrementAmount, @BasicSalary, @MedicalAllownce, @HouseRent, @ConvenceAllownce, @HolidayAllownce, @TiffinAllownce, @NightAllownce, @AttendanceBonus, @DateofUpdate, @DId, @DptId, @LnId, @FId, @GrpId," +
                    "@DsgId, @EmpCardNo, @GrdName, @TypeOfChange, @EffectiveMonth, @OrderRefNo, @OrderRefDate,@EmpStatus, @Remarks,@ActiveSalary,@FoodAllownce,@OthersAllownce,@LunchCount,@EarnLeaveDate,@LunchAllownce)", sqlDB.connection);

                cmd.Parameters.AddWithValue("@SN",SN);
                cmd.Parameters.AddWithValue("@EmpId",dtCertainEmpInfo.Rows[0]["EmpId"].ToString());

                cmd.Parameters.AddWithValue("@EmpTypeId", dtCertainEmpInfo.Rows[0]["EmpTypeId"].ToString());

                cmd.Parameters.AddWithValue("@SalaryType", dtCertainEmpInfo.Rows[0]["SalaryType"].ToString());
                cmd.Parameters.AddWithValue("@EmpPresentSalary", dtCertainEmpInfo.Rows[0]["EmpPresentSalary"].ToString());

                cmd.Parameters.AddWithValue("@IncrementAmount", dtCertainEmpInfo.Rows[0]["IncrementAmount"].ToString());




                cmd.Parameters.AddWithValue("@BasicSalary", dtCertainEmpInfo.Rows[0]["BasicSalary"].ToString());
                cmd.Parameters.AddWithValue("@MedicalAllownce", dtCertainEmpInfo.Rows[0]["MedicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@HouseRent", dtCertainEmpInfo.Rows[0]["HouseRent"].ToString());

                cmd.Parameters.AddWithValue("@ConvenceAllownce", dtCertainEmpInfo.Rows[0]["ConvenceAllownce"].ToString());
                cmd.Parameters.AddWithValue("@HolidayAllownce", dtCertainEmpInfo.Rows[0]["HolidayAllownce"].ToString());
                cmd.Parameters.AddWithValue("@TiffinAllownce", dtCertainEmpInfo.Rows[0]["TiffinAllownce"].ToString());
                cmd.Parameters.AddWithValue("@NightAllownce", dtCertainEmpInfo.Rows[0]["NightAllownce"].ToString());
                cmd.Parameters.AddWithValue("@AttendanceBonus", dtCertainEmpInfo.Rows[0]["AttendanceBonus"].ToString());


                cmd.Parameters.AddWithValue("@DateofUpdate", convertDateTime.getCertainCulture(dtCertainEmpInfo.Rows[0]["DateofUpdate"].ToString()).ToString());

                cmd.Parameters.AddWithValue("@DId", dtCertainEmpInfo.Rows[0]["DId"].ToString());
                cmd.Parameters.AddWithValue("@DptId", dtCertainEmpInfo.Rows[0]["DptId"].ToString());
                cmd.Parameters.AddWithValue("@LnId", dtCertainEmpInfo.Rows[0]["LnId"].ToString());


                cmd.Parameters.AddWithValue("@FId", dtCertainEmpInfo.Rows[0]["FId"].ToString());

                cmd.Parameters.AddWithValue("@GrpId", dtCertainEmpInfo.Rows[0]["GrpId"].ToString());

                cmd.Parameters.AddWithValue("@DsgId", dtCertainEmpInfo.Rows[0]["DsgId"].ToString());

                cmd.Parameters.AddWithValue("@EmpCardNo",EmpCardNo);

                cmd.Parameters.AddWithValue("@GrdName", dtCertainEmpInfo.Rows[0]["GrdName"].ToString());
                cmd.Parameters.AddWithValue("@TypeOfChange", dtCertainEmpInfo.Rows[0]["TypeOfChange"].ToString());

                DataTable dtGetPartialEmpInfo = new DataTable();
                sqlDB.fillDataTable("select EffectiveMonth,OrderRefNo,convert(varchar(11),OrderRefDate,105) as OrderRefDate,Remarks from Personnel_EmpCurrentStatus where SN=" +SN + "", dtGetPartialEmpInfo);

                cmd.Parameters.AddWithValue("@EffectiveMonth", dtGetPartialEmpInfo.Rows[0]["EffectiveMonth"].ToString());

                cmd.Parameters.AddWithValue("@OrderRefNo", dtGetPartialEmpInfo.Rows[0]["OrderRefNo"].ToString());
                if (dtGetPartialEmpInfo.Rows[0]["OrderRefDate"].ToString().Length <= 5)
                {
                    cmd.Parameters.AddWithValue("@OrderRefDate", convertDateTime.getCertainCulture("01-01-1950"));
                }
                else cmd.Parameters.AddWithValue("@OrderRefDate", convertDateTime.getCertainCulture(dtGetPartialEmpInfo.Rows[0]["OrderRefDate"].ToString()));
                cmd.Parameters.AddWithValue("@EmpStatus", '1');
                cmd.Parameters.AddWithValue("@Remarks", dtGetPartialEmpInfo.Rows[0]["Remarks"].ToString());
                cmd.Parameters.AddWithValue("@ActiveSalary", '1');   // remember that previous active salary all time 1 may be

                cmd.Parameters.AddWithValue("@FoodAllownce", dtCertainEmpInfo.Rows[0]["FoodAllownce"].ToString());

                cmd.Parameters.AddWithValue("@OthersAllownce", dtCertainEmpInfo.Rows[0]["OthersAllownce"].ToString());

                byte getLounchCount = (bool.Parse(dtCertainEmpInfo.Rows[0]["LunchCount"].ToString()) == true) ? (byte)1 : (byte)0;
                cmd.Parameters.AddWithValue("@LunchCount", getLounchCount.ToString());
                ViewState["__EarnLeaveDate__"] =  dtCertainEmpInfo.Rows[0]["EarnLeaveDate"].ToString();
                if (dtCertainEmpInfo.Rows[0]["EarnLeaveDate"].ToString() == "" || dtCertainEmpInfo.Rows[0]["EarnLeaveDate"].ToString() == " ") ViewState["__EarnLeaveDate__"] = "01-01-2050";
                cmd.Parameters.AddWithValue("@EarnLeaveDate", convertDateTime.getCertainCulture(ViewState["__EarnLeaveDate__"].ToString()).ToString());

                cmd.Parameters.AddWithValue("@LunchAllownce", dtCertainEmpInfo.Rows[0]["LunchAllownce"].ToString());
                int i = (int)cmd.ExecuteNonQuery();
            }
            catch { }
        }

        private void enteredDataInCommonIncrement()
        {
            try
            {
                HttpCookie getCookies = Request.Cookies["userInfo"];
                string getUserId = getCookies["__getUserId__"].ToString();
                string[] getColumns = { "EmpTypeId", "AmountPercentage", "EffectiveMonth", "Remarks", "EntryDate", "userId", "IsActivated" };
                string[] getValues = { rbEmployeeType.SelectedValue.ToString(), txtPercentage.Text.Trim(), txtMonth.Text.Trim(), txtRemarks.Text.Trim(), convertDateTime.getCertainCulture(DateTime.Now.ToString("d-M-yyyy")).ToString(), getUserId, "0" };
                SQLOperation.forSaveValue("Personnel_EmpCommonIncrement", getColumns, getValues, sqlDB.connection);
                
            }
            catch (Exception ex)
            {
              lblMessage.InnerText=ex.Message;
            }
        }

        protected void btnUndo_Click(object sender, EventArgs e)
        {
            DeleteRecentlyIncrementData();
        }

        private void DeleteRecentlyIncrementData(string sn)
        {
            try
            {
                sqlDB.fillDataTable("select EmpCardNo,MAX(SN) As SN,ActiveSalary,EffectiveMonth   from Personnel_EmpCurrentStatus group by EmpCardNo,ActiveSalary,EffectiveMonth having ActiveSalary ='false' order by SN", dt = new DataTable());

                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    SQLOperation.forDeleteRecordByIdentifier("Personnel_EmpCurrentStatus", "SN", dt.Rows[r]["SN"].ToString(), sqlDB.connection);
                }
                if (dt.Rows.Count > 0)
                {
                    btnUndo.Enabled = false;
                    btnUndo.CssClass = "";
                    SQLOperation.forDeleteRecordByIdentifier("Personnel_EmpCommonIncrement", "EffectiveMonth", dt.Rows[0]["EffectiveMonth"].ToString(), sqlDB.connection);
                    lblMessage.InnerText = "success->Successfully Increment List Deleted";
                }
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void DeleteRecentlyIncrementData()
        {
            try
            {
                DataTable dtSN = new DataTable();
                sqlDB.fillDataTable("select EmpCardNo,MAX(SN) As SN,ActiveSalary,EffectiveMonth,EmpId,IsActive   from Personnel_EmpCurrentStatus group by EmpCardNo,ActiveSalary,EffectiveMonth,EmpId,IsActive having IsActive='0' And ActiveSalary ='false' order by SN", dtSN);
                bool isSuccess=false;
                for (int i = 0; i < dtSN.Rows.Count; i++)
                {
                    if (SQLOperation.forDeleteRecordByIdentifier("Personnel_EmpCurrentStatus", "SN", dtSN.Rows[i]["SN"].ToString(), sqlDB.connection) == true)
                    {
                        dt = new DataTable();
                        sqlDB.fillDataTable("select Max(SN) as SN from v_Personnel_EmpCurrentStatus where  EmpId='" +dtSN.Rows[i]["EmpId"].ToString()+ "'", dt);
                        SqlCommand upIsActive = new SqlCommand("Update Personnel_EmpCurrentStatus set IsActive=1 where SN=" + dt.Rows[0]["SN"].ToString() + "", sqlDB.connection);
                        upIsActive.ExecuteNonQuery();
                        
                       SQLOperation.forDeleteRecordByIdentifier("Personnel_EmpCurrentStatusSalaryIncrementLog", "SN", dt.Rows[0]["SN"].ToString(), sqlDB.connection);
                       isSuccess = true;
                    }                      
                }

                if (isSuccess)
                {
                    lblMessage.InnerText = "success->Successfully Undo";
                    btnUndo.Enabled = false;
                }

            }
            catch { }
        }

       
    }
}