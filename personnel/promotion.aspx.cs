using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using adviitRuntimeScripting;
using ComplexScriptingSystem;
using System.Data.SqlClient;
using System.Data;
using SigmaERP.classes;
using System.Drawing;

namespace SigmaERP.personnel
{
    public partial class promotion : System.Web.UI.Page
    {
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            sqlDB.connectionString = Glory.getConnectionString();
            sqlDB.connectDB();
            setBtnDelete();
            byte isUpdateMode =0;

            try
            {
                isUpdateMode = byte.Parse(Request.QueryString["status"].ToString()); 
                
            }
            catch {}

            if (isUpdateMode == 1)
            {
                findPromotionInfo(long.Parse(Request.QueryString["esn"].ToString())); return;
            }
            if (!IsPostBack)
            {
                setPrivilege();               
                classes.commonTask.loadEmpTye(ddlEmpType);
                classes.commonTask.loadEmpTye(ddlNewEmpType);
                classes.commonTask.LoadGrade(ddlNewGrade);               
                if (!classes.commonTask.HasBranch())
                    ddlCompany.Enabled = false;
            }
            lblMessage.InnerText = "";
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
                AccessPermission = checkUserPrivilege.checkUserPrivilegeForSettigs(ViewState["__CompanyId__"].ToString(), getUserId, ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()), "promotion.aspx", ddlCompany, divpromotionList, btnSave);

                ViewState["__ReadAction__"] = AccessPermission[0];
                ViewState["__WriteAction__"] = AccessPermission[1];
                ViewState["__UpdateAction__"] = AccessPermission[2];
                ViewState["__DeletAction__"] = AccessPermission[3];


                ddlCompany.SelectedValue = ViewState["__CompanyId__"].ToString();
                classes.Employee.LoadEmpCardNoForPayroll(ddlEmpCardNo, ViewState["__CompanyId__"].ToString());
                classes.commonTask.SearchDepartment(ViewState["__CompanyId__"].ToString(), ddlNewDepartment);
                classes.commonTask.LoadGrade(ddlNewGrade);
                loadSalaryInfo();           

            }
            catch { }

        }

        private void loadBasictAndOtheresPercentage()
        {
            try
            {
                sqlDB.fillDataTable("select BasicAllowance,MedicalAllownce,ConvenceAllownce from HRD_AllownceSetting where AllownceId=(select max(AllownceId) from HRD_AllownceSetting)", dt = new DataTable());
                hfBasicPercentage.Value = dt.Rows[0]["BasicAllowance"].ToString();
                hfConvenceAllownce.Value = dt.Rows[0]["ConvenceAllownce"].ToString();
                hfMedicalAllownce.Value = dt.Rows[0]["MedicalAllownce"].ToString();
            }
            catch { }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            if (ddlEmpType.SelectedItem.Text.Equals("Staff"))
            {
                
                HttpCookie getCookies = Request.Cookies["userInfo"];
                if (IsBankSalary() == true)
                {
                    if (ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(getCookies["__getUserType__"].ToString()).Equals("Master Admin"))
                    {
                        findPromotionInfo(0);
                    }
                    else
                    {
                        lblMessage.InnerText = "warning->Sorry,Supper admin is required for complete this query";
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    }

                }
                else findPromotionInfo(0);
            }
            else
            {
                findPromotionInfo(0);
            }
            txtNewGross.Text = "";
            txtIncrementAmount.Text = "";
        }
        private bool IsBankSalary()
        {
            try
            {
                DataTable dtSalaryCount = new DataTable();
                sqlDB.fillDataTable(" select SalaryCount from Personnel_EmployeeInfo where EmpCardNo='" + ddlEmpCardNo.SelectedValue.ToString() + "' AND EmpTypeId=" + ddlEmpType.SelectedValue.ToString() + "", dtSalaryCount);
                if (dtSalaryCount.Rows[0]["SalaryCount"].ToString() == "Bank") return true;
                else return false;
            }
            catch { return false; }

        }

        private void setBtnDelete()
        {
            try
            {
                btnDelete.CssClass = "hide";
              //  btnDelete.Style["Display"] = "None";
                //btnDelete.CssClass = "";
                //btnDelete.Enabled = false;
            }
            catch { }
        }
        private void findPromotionInfo(long esn)  // sn= employee serila number
        {
            try
            {
                if (esn == 0) sqlDB.fillDataTable("select SN,EmpId,EmpCardNo,EmpTypeId,EmpName,SalaryType,EmpPresentSalary,IncrementAmount,convert(varchar(11),DateofUpdate,105)as DateofUpdate,BasicSalary,MedicalAllownce,FoodAllownce,HouseRent,ConvenceAllownce,OrderRefNo,OrderRefDate,EffectiveMonth,DptId,DptName,DsgId,DsgName,GrdName,TypeOfChange,HolidayAllownce,TiffinAllownce,NightAllownce,AttendanceBonus,LunchAllownce,LunchCount,Convert(varchar(11),EarnLeaveDate,105) as EarnLeaveDate,EmpTypeId from v_Personnel_EmpCurrentStatus where SN='" + ddlEmpCardNo.SelectedValue + "'", dt = new DataTable());
                else sqlDB.fillDataTable("select SN,EmpId,EmpTypeId,EmpCardNo,EmpName,SalaryType,EmpPresentSalary,IncrementAmount,convert(varchar(11),DateofUpdate,105)as DateofUpdate,BasicSalary,MedicalAllownce,FoodAllownce,HouseRent,ConvenceAllownce,OrderRefNo,convert(varchar(11),OrderRefDate,105) as OrderRefDate,EffectiveMonth,DId,DptId,DptName,DsgId,DsgName,GrdName,TypeOfChange,LunchAllownce,LunchCount from v_Personnel_EmpCurrentStatus where SN=" + esn + "", dt = new DataTable());
                if (dt.Rows.Count > 0)
                {
                    ddlEmpType.SelectedValue = dt.Rows[0]["EmpTypeId"].ToString();
                    ViewState["__getSN__"] = dt.Rows[0]["SN"].ToString();
                    ViewState["__getEmpId__"] = dt.Rows[0]["EmpId"].ToString();
                    ViewState["__getEmpCardNo__"] = dt.Rows[0]["EmpCardNo"].ToString();
                    txtEmpName.Text = dt.Rows[0]["EmpName"].ToString();
                    hfSalaryType.Value = dt.Rows[0]["SalaryType"].ToString();
                    ViewState["__SalaryType__"] = dt.Rows[0]["SalaryType"].ToString();
                    ViewState["__EmpTypeId__"] = dt.Rows[0]["EmpTypeId"].ToString();
                    txtLastIncrementDate.Text = dt.Rows[0]["DateofUpdate"].ToString();
                    txtLastIncrementAmount.Text = dt.Rows[0]["IncrementAmount"].ToString();
                    ViewState["__EPS__"] = dt.Rows[0]["EmpPresentSalary"].ToString();  // EPS=Employee Present Salary 
                    txtPreGross.Text = dt.Rows[0]["EmpPresentSalary"].ToString(); 
                    ViewState["__TypeOfChange__"] = dt.Rows[0]["TypeOfChange"].ToString();

                    ViewState["__BasicSalary__"] = dt.Rows[0]["BasicSalary"].ToString();
                    
                    ViewState["__HouseRent__"] = dt.Rows[0]["HouseRent"].ToString();
                    LoadAllownceSetting(dt.Rows[0]["SalaryType"].ToString(), ddlEmpType.SelectedValue);

                    ViewState["__HolidayAllownce__"] = dt.Rows[0]["HolidayAllownce"].ToString();
                    ViewState["__TiffinAllownce__"] = dt.Rows[0]["TiffinAllownce"].ToString();
                    ViewState["__NightAllownce__"] = dt.Rows[0]["NightAllownce"].ToString();
                    ViewState["__AttendanceBonus__"] = dt.Rows[0]["AttendanceBonus"].ToString();
                    ViewState["__getLunchCount__"] = dt.Rows[0]["LunchCount"].ToString();
                    ViewState["__LunchAllownce__"] = dt.Rows[0]["LunchAllownce"].ToString();
                    ViewState["__EarnLeaveDate__"] = dt.Rows[0]["EarnLeaveDate"].ToString();
                  
                    txtPresentDepartment.Text = dt.Rows[0]["DptName"].ToString();
                    ViewState["__DptId__"] = dt.Rows[0]["DptId"].ToString();
                    txtPresentDesignation.Text = dt.Rows[0]["DsgName"].ToString();
                    ViewState["__DsgId__"] = dt.Rows[0]["DsgId"].ToString();

                    txtPresentGrade.Text = dt.Rows[0]["GrdName"].ToString();
                    ViewState["__GrdName__"] = dt.Rows[0]["GrdName"].ToString();
                    
                    
                }
                else lblMessage.InnerText = "Warning->This employee is not exists in select employee type.";
            }
            catch { }
        }
        protected void ddlNewDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            classes.commonTask.SearchDesignation(ddlNewDepartment.SelectedValue, ddlNewDesignation);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);

        }

     

        protected void btnSave_Click(object sender, EventArgs e)
        {           
            if (hfSaveStatus.Value.ToString().Equals("Save")&&Validation()==true) savePromotionPortion();
            else
            {
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);                
                //loadSalaryInfo("");
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }
        private Boolean Validation()
        {
            try
            {
                if (ddlNewDepartment.SelectedItem.Text != "")
                {
                    if (ddlNewDepartment.SelectedItem.Text != txtPresentDepartment.Text)
                    {
                        if (ddlNewDesignation.SelectedItem.Text == "")
                        {
                            lblMessage.InnerText = "warning->Select Designation";
                            ddlNewDesignation.Focus();
                            return false;
                        }
                    }
                }
                return true;
            }
            catch { return false; }
        }
        private void savePromotionPortion()
        {
            try
            {
                    double getIncrementAmount = 0, getMedicalAllownce = 0, getConvence = 0, getBasic = 0, getPresentSalary = 0, getHouseRent=0,getfood=0,getOthers=0;
                     
                    DataTable dtPreStatus = new DataTable();
                    sqlDB.fillDataTable("Select CompanyId,EmpTypeId,SalaryType,EmpPresentSalary,IncrementAmount,BasicSalary,MedicalAllownce,ConvenceAllownce,HouseRent,PreTechnicalAllownce,TechnicalAllownce,DptId,DsgId,EmpStatus,GrdName,OthersAllownce,EarnLeaveDate,PreShiftTransferDate,ShiftTransferDate,ShiftTransferToDate,SftId,GId,PreGId,SalaryCount,BankId,EmpAccountNo,PfMember,PfDate,PFAmount,CustomOrdering,OverTime,PreEmpDutyType,EmpDutyType,DormitoryRent,PreIncomeTax,IncomeTax From Personnel_EmpCurrentStatus where SN='" + ddlEmpCardNo.SelectedValue + "'", dtPreStatus);
                    LoadAllownceSetting(dtPreStatus.Rows[0]["SalaryType"].ToString(), ddlNewEmpType.SelectedValue);
                    getIncrementAmount = (txtIncrementAmount.Text.Trim().Length == 0) ? 0 : double.Parse(txtIncrementAmount.Text.Trim());
                    if (getIncrementAmount == 0)
                    {
                        getMedicalAllownce = Math.Round(double.Parse(ViewState["__MedicalAllownce__"].ToString()), 0);
                        getConvence = Math.Round(double.Parse(ViewState["__ConvenceAllownce__"].ToString()), 0);
                        getfood = Math.Round(double.Parse(ViewState["__FoodAllownce__"].ToString()), 0);
                        getBasic = double.Parse(ViewState["__BasicSalary__"].ToString());
                        getPresentSalary =double.Parse(dtPreStatus.Rows[0]["EmpPresentSalary"].ToString());
                        getHouseRent = double.Parse(dtPreStatus.Rows[0]["HouseRent"].ToString());
                        getOthers = double.Parse(dtPreStatus.Rows[0]["OthersAllownce"].ToString());
                        
                    }
                    else
                    {
                        if (classes.Payroll.Office_IsGarments())
                        {
                            getPresentSalary = double.Parse(dtPreStatus.Rows[0]["EmpPresentSalary"].ToString()) + getIncrementAmount;
                            getBasic = Math.Round(getPresentSalary * double.Parse(ViewState["__Basicpercent__"].ToString()) / 100, 0);
                            getfood = Math.Round(double.Parse(ViewState["__FoodAllownce__"].ToString()), 0);
                            getMedicalAllownce = Math.Round(double.Parse(ViewState["__MedicalAllownce__"].ToString()), 0);
                            getConvence = Math.Round(double.Parse(ViewState["__ConvenceAllownce__"].ToString()), 0);                         
                            getHouseRent = Math.Round(getPresentSalary * double.Parse(ViewState["__Houserentpercent__"].ToString()) / 100, 0);
                            getOthers = getPresentSalary - (getBasic + getHouseRent + getMedicalAllownce);
                            if(getOthers<0)
                            {
                                getHouseRent = getHouseRent + getOthers;
                            }
                            if(getPresentSalary<=5500)
                            {
                                ViewState["__AttendanceBonus__"] = "200";
                            }
                            else
                            {
                                ViewState["__AttendanceBonus__"] = "0";
                            }
                        }
                        else
                        {
                            getPresentSalary = double.Parse(dtPreStatus.Rows[0]["EmpPresentSalary"].ToString()) + getIncrementAmount;
                            getBasic = Math.Round(getPresentSalary * double.Parse(ViewState["__Basicpercent__"].ToString()) / 100, 0);
                            getfood = Math.Round(double.Parse(ViewState["__FoodAllownce__"].ToString()), 0);
                            getMedicalAllownce = Math.Round(double.Parse(ViewState["__MedicalAllownce__"].ToString()), 0);
                            getConvence = Math.Round(getPresentSalary * double.Parse(ViewState["__ConvenceAllownce__"].ToString()) / 100, 0);
                            getHouseRent = Math.Round(getPresentSalary * double.Parse(ViewState["__Houserentpercent__"].ToString()) / 100, 0);
                           // getOthers = getPresentSalary - (getBasic + getHouseRent + getMedicalAllownce);
                        }
                        
                    }                                                                                           
                string getEmpType = (!ddlNewEmpType.SelectedItem.Text.Trim().Equals("Select Type")) ? ddlNewEmpType.SelectedValue : ddlEmpType.SelectedValue;
                string d = ViewState["__DptId__"].ToString();
                string getDepartmentId = (ddlNewDepartment.Text.ToString().Equals("")) ? ViewState["__DptId__"].ToString() : ddlNewDepartment.SelectedValue.ToString();
                string getDesignationId = (ddlNewDesignation.Text.ToString().Equals("")) ?ViewState["__DsgId__"].ToString(): ddlNewDesignation.SelectedValue.ToString() ;
                //  string get = ViewState["__GrdId__"].ToString();
                string getGradeName = (!ddlNewGrade.Text.ToString().Equals("0")) ? ddlNewGrade.SelectedItem.ToString() : ViewState["__GrdName__"].ToString();

              

                


                //string getCardNo = (!txtNewEmpCardNo.Text.Trim().Length.Equals(0)) ? txtNewEmpCardNo.Text.Trim() : txtEmpCardNo.Text.Trim();
                string getChangeOfType = "p";
                string date = DateTime.Now.ToString("dd-MM-yyyy");

                SqlCommand cmd = new SqlCommand("Insert into  Personnel_EmpCurrentStatus (EmpId, PreCompanyId, CompanyId, EmpCardNo, PreEmpTypeId, EmpTypeId, PreSalaryType, SalaryType, PreEmpSalary, EmpPresentSalary, PreIncrementAmount, IncrementAmount, PreBasicSalary, BasicSalary,PreMedicalAllownce, MedicalAllownce,PreFoodAllownce, FoodAllownce,PreConvenceAllownce, ConvenceAllownce, PreHouseRent, HouseRent,PreTechnicalAllownce,TechnicalAllownce, PreDptId, DptId, PreDsgId, DsgId, PreEmpStatus, EmpStatus, PreGrdName, GrdName, PreOthersAllownce, OthersAllownce, HolidayAllownce, TiffinAllownce, NightAllownce, AttendanceBonus, LunchAllownce, LunchCount, DateofUpdate, TypeOfChange, EffectiveMonth, OrderRefNo, OrderRefDate, Remarks, ActiveSalary, EarnLeaveDate, IsActive,PreShiftTransferDate,ShiftTransferDate,ShiftTransferToDate,SftId,GId,PreGId,SalaryCount,BankId,EmpAccountNo,PfMember,PfDate,PFAmount,CustomOrdering,OverTime,PreEmpDutyType,EmpDutyType,DormitoryRent,PreIncomeTax,IncomeTax)  values (@EmpId, @PreCompanyId, @CompanyId, @EmpCardNo, @PreEmpTypeId, @EmpTypeId, @PreSalaryType, @SalaryType, @PreEmpSalary, @EmpPresentSalary, @PreIncrementAmount, @IncrementAmount, @PreBasicSalary, @BasicSalary,@PreMedicalAllownce, @MedicalAllownce,@PreFoodAllownce,@FoodAllownce,@PreConvenceAllownce, @ConvenceAllownce, @PreHouseRent, @HouseRent,@PreTechnicalAllownce,@TechnicalAllownce, @PreDptId, @DptId, @PreDsgId, @DsgId, @PreEmpStatus, @EmpStatus, @PreGrdName, @GrdName, @PreOthersAllownce, @OthersAllownce, @HolidayAllownce, @TiffinAllownce, @NightAllownce, @AttendanceBonus, @LunchAllownce, @LunchCount, @DateofUpdate, @TypeOfChange, @EffectiveMonth, @OrderRefNo, @OrderRefDate, @Remarks, @ActiveSalary, @EarnLeaveDate, @IsActive,@PreShiftTransferDate,@ShiftTransferDate,@ShiftTransferToDate,@SftId,@GId,@PreGId,@SalaryCount,@BankId,@EmpAccountNo,@PfMember,@PfDate,@PFAmount,@CustomOrdering,@OverTime,@PreEmpDutyType,@EmpDutyType,@DormitoryRent,@PreIncomeTax,@IncomeTax) ", sqlDB.connection);
                cmd.Parameters.AddWithValue("@EmpId", ViewState["__getEmpId__"].ToString());
                cmd.Parameters.AddWithValue("@PreCompanyId", dtPreStatus.Rows[0]["CompanyId"].ToString());
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
                cmd.Parameters.AddWithValue("@CompanyId", ddlCompany.SelectedValue);
                else cmd.Parameters.AddWithValue("@CompanyId",ViewState["__CompanyId__"].ToString());
                cmd.Parameters.AddWithValue("@EmpCardNo", ViewState["__getEmpCardNo__"].ToString());
                cmd.Parameters.AddWithValue("@PreEmpTypeId", dtPreStatus.Rows[0]["EmpTypeId"].ToString());
                cmd.Parameters.AddWithValue("@EmpTypeId",ddlNewEmpType.SelectedValue);
                cmd.Parameters.AddWithValue("@PreSalaryType", dtPreStatus.Rows[0]["SalaryType"].ToString());
                cmd.Parameters.AddWithValue("@SalaryType", dtPreStatus.Rows[0]["SalaryType"].ToString());
                cmd.Parameters.AddWithValue("@PreEmpSalary", dtPreStatus.Rows[0]["EmpPresentSalary"].ToString());
                cmd.Parameters.AddWithValue("@EmpPresentSalary", getPresentSalary);
                cmd.Parameters.AddWithValue("@PreIncrementAmount", dtPreStatus.Rows[0]["IncrementAmount"].ToString());
                cmd.Parameters.AddWithValue("@IncrementAmount", getIncrementAmount.ToString());
                cmd.Parameters.AddWithValue("@PreBasicSalary", dtPreStatus.Rows[0]["BasicSalary"].ToString());
                cmd.Parameters.AddWithValue("@BasicSalary", getBasic);
                cmd.Parameters.AddWithValue("@PreMedicalAllownce", dtPreStatus.Rows[0]["MedicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@MedicalAllownce",getMedicalAllownce);
                cmd.Parameters.AddWithValue("@PreFoodAllownce",ViewState["__FoodAllownce__"].ToString());
                cmd.Parameters.AddWithValue("@FoodAllownce", getfood);
                cmd.Parameters.AddWithValue("@PreConvenceAllownce", dtPreStatus.Rows[0]["ConvenceAllownce"].ToString()); 
                cmd.Parameters.AddWithValue("@ConvenceAllownce", getConvence);                 
                cmd.Parameters.AddWithValue("@PreHouseRent", dtPreStatus.Rows[0]["HouseRent"].ToString());
                cmd.Parameters.AddWithValue("@HouseRent", getHouseRent.ToString());               
                cmd.Parameters.AddWithValue("@PreTechnicalAllownce", dtPreStatus.Rows[0]["PreTechnicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@TechnicalAllownce", dtPreStatus.Rows[0]["TechnicalAllownce"].ToString());
                cmd.Parameters.AddWithValue("@PreDptId", dtPreStatus.Rows[0]["DptId"].ToString());
                cmd.Parameters.AddWithValue("@DptId", getDepartmentId);
                cmd.Parameters.AddWithValue("@PreDsgId", dtPreStatus.Rows[0]["DsgId"].ToString());
                cmd.Parameters.AddWithValue("@DsgId", getDesignationId);
                cmd.Parameters.AddWithValue("@PreEmpStatus", dtPreStatus.Rows[0]["EmpStatus"].ToString());
                cmd.Parameters.AddWithValue("@EmpStatus", "1");
                cmd.Parameters.AddWithValue("@PreGrdName", dtPreStatus.Rows[0]["GrdName"].ToString());
                cmd.Parameters.AddWithValue("@GrdName", getGradeName);
                cmd.Parameters.AddWithValue("@PreOthersAllownce", dtPreStatus.Rows[0]["OthersAllownce"].ToString());
                cmd.Parameters.AddWithValue("@OthersAllownce","0");
                cmd.Parameters.AddWithValue("@HolidayAllownce","0");
                cmd.Parameters.AddWithValue("@TiffinAllownce", ViewState["__TiffinAllownce__"].ToString());              
                cmd.Parameters.AddWithValue("@NightAllownce",ViewState["__NightAllownce__"].ToString());
                cmd.Parameters.AddWithValue("@AttendanceBonus",ViewState["__AttendanceBonus__"].ToString());
                cmd.Parameters.AddWithValue("@LunchAllownce", ViewState["__LunchAllownce__"].ToString());
                byte getLounchCount = (bool.Parse(ViewState["__getLunchCount__"].ToString()) == true) ? (byte)1 : (byte)0;
                cmd.Parameters.AddWithValue("@LunchCount",getLounchCount.ToString());
                cmd.Parameters.AddWithValue("@DateofUpdate", convertDateTime.getCertainCulture(date));
                cmd.Parameters.AddWithValue("@TypeOfChange", getChangeOfType);
                cmd.Parameters.AddWithValue("@EffectiveMonth", txtEffectiveFrom.Text);
                cmd.Parameters.AddWithValue("@OrderRefNo", txtIncrementOrderRefNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@OrderRefDate", convertDateTime.getCertainCulture(txtIncrementOrderRefDate.Text));
                cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim());
                cmd.Parameters.AddWithValue("@ActiveSalary", 0);
                if (ViewState["__EarnLeaveDate__"].ToString() == "" || ViewState["__EarnLeaveDate__"].ToString() == " ") ViewState["__EarnLeaveDate__"] = "01-01-2050";
                cmd.Parameters.AddWithValue("@EarnLeaveDate",convertDateTime.getCertainCulture( ViewState["__EarnLeaveDate__"].ToString()).ToString());
                cmd.Parameters.AddWithValue("@IsActive", 0);
                cmd.Parameters.AddWithValue("@PreShiftTransferDate", dtPreStatus.Rows[0]["PreShiftTransferDate"].ToString());
                cmd.Parameters.AddWithValue("@ShiftTransferDate", dtPreStatus.Rows[0]["ShiftTransferDate"].ToString());
                cmd.Parameters.AddWithValue("@ShiftTransferToDate", dtPreStatus.Rows[0]["ShiftTransferToDate"].ToString());
                cmd.Parameters.AddWithValue("@SftId", dtPreStatus.Rows[0]["SftId"].ToString());
                cmd.Parameters.AddWithValue("@GId", dtPreStatus.Rows[0]["GId"].ToString());
                cmd.Parameters.AddWithValue("@PreGId", dtPreStatus.Rows[0]["PreGId"].ToString());
                cmd.Parameters.AddWithValue("@SalaryCount", dtPreStatus.Rows[0]["SalaryCount"].ToString());
                cmd.Parameters.AddWithValue("@BankId", dtPreStatus.Rows[0]["BankId"].ToString());
                cmd.Parameters.AddWithValue("@EmpAccountNo", dtPreStatus.Rows[0]["EmpAccountNo"].ToString());
                cmd.Parameters.AddWithValue("@PfMember", dtPreStatus.Rows[0]["PfMember"].ToString());
                cmd.Parameters.AddWithValue("@PfDate", dtPreStatus.Rows[0]["PfDate"].ToString());
                cmd.Parameters.AddWithValue("@PFAmount", dtPreStatus.Rows[0]["PFAmount"].ToString());
                cmd.Parameters.AddWithValue("@CustomOrdering", dtPreStatus.Rows[0]["CustomOrdering"].ToString());
                cmd.Parameters.AddWithValue("@OverTime", dtPreStatus.Rows[0]["OverTime"].ToString());
                cmd.Parameters.AddWithValue("@PreEmpDutyType", dtPreStatus.Rows[0]["PreEmpDutyType"].ToString());
                cmd.Parameters.AddWithValue("@EmpDutyType", dtPreStatus.Rows[0]["EmpDutyType"].ToString());
                cmd.Parameters.AddWithValue("@DormitoryRent", dtPreStatus.Rows[0]["DormitoryRent"].ToString());
                cmd.Parameters.AddWithValue("@PreIncomeTax", dtPreStatus.Rows[0]["PreIncomeTax"].ToString());
                cmd.Parameters.AddWithValue("@IncomeTax", dtPreStatus.Rows[0]["IncomeTax"].ToString());
                int i=(int)cmd.ExecuteNonQuery();
                if (i>0)
                {
                    lblMessage.InnerText = "success->Successfull Pormotioned ";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    loadSalaryInfo();
                }

               
            }
            catch (Exception ex)
            {
                lblMessage.InnerText = "error->" + ex.Message;
            }
        }
        private void loadSalaryInfo()
        {
            try
            {
               // DataTable dtSI = new DataTable();
                
                //sqlDB.fillDataTable("Select Max(SN) as SN,EmpId From  v_EmployeeDetails where TypeOfChange='p' and CompanyId='" + ViewState["__CompanyId__"] + "'  Group by EmpId", dt);
                //string setSn = "";
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    if (i == 0 && i == dt.Rows.Count - 1)
                //    {
                //        setSn = "in('" + dt.Rows[i].ItemArray[0].ToString() + "')";
                //    }
                //    else if (i == 0 && i != dt.Rows.Count - 1)
                //    {
                //        setSn += "in ('" + dt.Rows[i].ItemArray[0].ToString() + "'";
                //    }
                //    else if (i != 0 && i == dt.Rows.Count - 1)
                //    {
                //        setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "')";
                //    }
                //    else
                //    {
                //        setSn += ",'" + dt.Rows[i].ItemArray[0].ToString() + "'";
                //    }
                //}
                dt = new DataTable();
             //    sqlDB.fillDataTable("select SN,EmpId,CompanyName,EmpCardNo, DptName,DsgName,Convert(varchar(11),DateofUpdate,105) as DateofUpdate,EmpType,EmpName  from v_Personnel_EmpCurrentStatus where SN " + setSn + "", dt = new DataTable());    
                sqlDB.fillDataTable("select SN,EmpId,CompanyName,EmpCardNo, DptName,DsgName,Convert(varchar(11),DateofUpdate,105) as DateofUpdate,EmpType,EmpName  from v_Personnel_EmpCurrentStatus where TypeOfChange='p' and ActiveSalary=0 and CompanyId='" + ddlCompany.SelectedValue + "' order by EffectiveMonth desc", dt = new DataTable());
                divpromotionList.DataSource = dt;
                divpromotionList.DataBind();

            }
            catch { }
        }
        private DataTable dt_AllowanceSettings = new DataTable();
        private void LoadAllownceSetting(string SalaryType, string EmpTypeId)
        {
            try
            {
                dt_AllowanceSettings = new DataTable();
                sqlDB.fillDataTable("Select acs.SalaryType ,has.BasicAllowance,has.MedicalAllownce,has.FoodAllownce,has.ConvenceAllownce,has.TechnicalAllowance, " +
                    " has.HouseRent,has.OthersAllowance,has.PFAllowance,has.AlCalId,acs.EmpTypeId," +
                    "acs.BasicAllowance as BasicStatus,acs.MedicalAllownce as MedicalStatus,acs.FoodAllownce as FoodStatus,acs.ConvenceAllownce as ConStatus," +
                    " acs.TechnicalAllowance as TecStatus,acs.HouseRent as HouseStatus,acs.OthersAllowance as OthStatus, acs.ProvidentFund as PFStatus " +
                    " from HRD_AllownceSetting as has inner join Payroll_AllowanceCalculationSetting acs on has.AlCalId=acs.AlCalId where SalaryType='" + SalaryType + "' and EmpTypeId='" + EmpTypeId + "' and CalculationType='salary'", dt_AllowanceSettings);
                if (dt_AllowanceSettings.Rows.Count > 0)
                {
                    ViewState["__MedicalAllownce__"] = dt_AllowanceSettings.Rows[0]["MedicalAllownce"].ToString();
                    ViewState["__FoodAllownce__"] = dt_AllowanceSettings.Rows[0]["FoodAllownce"].ToString();
                    ViewState["__ConvenceAllownce__"] = dt_AllowanceSettings.Rows[0]["ConvenceAllownce"].ToString();
                    ViewState["__Houserentpercent__"] = dt_AllowanceSettings.Rows[0]["HouseRent"].ToString();
                    ViewState["__Basicpercent__"] = dt_AllowanceSettings.Rows[0]["BasicAllowance"].ToString();
                }




            }
            catch { }
        }

        protected void ddlEmpType_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
            {
                classes.commonTask.loadEmpCardNo(ddlEmpCardNo, ddlEmpType.SelectedValue.ToString(),ddlCompany.SelectedValue);
            }
            else
            {
                classes.commonTask.loadEmpCardNo(ddlEmpCardNo, ddlEmpType.SelectedValue.ToString(), ViewState["__CompanyId__"].ToString());
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {               
              //  Response.Write("<script language=javascript>var a=confirm('The system not allow negative inventory,continue?');</script>");

                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "getDeleteMessage();", true);
                //if (hfDeleteStatus.Value.ToString().Equals("1"))
                //{
                   deletePromotion("");
                //}
                //else loadSalaryInfo();
                 ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);

            }
            catch { }
        }

        private void deletePromotion(string SN)
        {
            try
            {
                dt = new DataTable();
                sqlDB.fillDataTable("Select Distinct EmpId From Personnel_EmpCurrentStatus where SN=" + SN + "", dt);  
                if (SQLOperation.forDeleteRecordByIdentifier("Personnel_EmpCurrentStatus", "SN",SN, sqlDB.connection) == true)
                {
                  
                    SqlCommand upIsActive = new SqlCommand("Update Personnel_EmpCurrentStatus set IsActive=1 where SN=(Select Max(SN) as SN From Personnel_EmpCurrentStatus where EmpId='"+dt.Rows[0]["EmpId"].ToString()+"')", sqlDB.connection);
                    upIsActive.ExecuteNonQuery();               
                    lblMessage.InnerText = "success->Successfully Deleted";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "ClearInputBox();", true);
                    loadSalaryInfo();
                }
            }
            catch { }
        }

        protected void btnPromotionInfo_Click(object sender, EventArgs e)
        {
            try
            {
                string CompanyId = "";
                if (ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Super Admin") || ComplexLetters.getEntangledLetters(ViewState["__UserType__"].ToString()).Equals("Master Admin"))
                {
                    CompanyId = ddlCompany.SelectedValue;
                }
                else
                {
                    CompanyId=ViewState["__CompanyId__"].ToString();
                }
                DataTable dtPromotionInfo = new DataTable();
                sqlDB.fillDataTable("Select SubString(EmpCardNo,8,15) as EmpCardNo,EmpName,PreIncrementAmount,IncrementAmount,PreGrdName,GrdName,DptName,PreDptName,DsgName,PreDsgName,Address,CompanyName From v_Promotion_Increment where CompanyId='" + CompanyId + "'  and EmpId in(select EmpId from Personnel_EmpCurrentStatus where SN='" + ddlEmpCardNo.SelectedValue + "') and TypeOfChange='p'", dtPromotionInfo);
                Session["__PromotionInfo__"] = dtPromotionInfo;
                if (dtPromotionInfo.Rows.Count > 0)
                {

                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "goToNewTabandWindow('/All Report/Report.aspx?for=PromotionInfo');", true);  //Open New Tab for Sever side code
                }
                else
                {
                    lblMessage.InnerText = "warning->No Promotion ";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
                    return;
                }

            }
            catch { }
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            classes.Employee.LoadEmpCardNoForPayroll(ddlEmpCardNo, ddlCompany.SelectedValue);
            classes.commonTask.SearchDepartment(ddlCompany.SelectedValue,ddlNewDepartment);
            loadSalaryInfo();  
        }

        protected void divpromotionList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "deleterow")
                {
                    int rIndex = Convert.ToInt32(e.CommandArgument.ToString());
                    deletePromotion(divpromotionList.DataKeys[rIndex].Values[0].ToString());
                }
            }
            catch { }
        }

        protected void divpromotionList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);
            loadSalaryInfo();  
        }

        protected void btnComplain_Click(object sender, EventArgs e)
        {
            Session["__ModuleType__"] = "Personnel";
            Session["__forCompose__"] = "No";
            Session["__PreviousPage__"] = Request.ServerVariables["HTTP_REFERER"].ToString();
            Response.Redirect("/mail/complain.aspx");
        }

        protected void divpromotionList_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
                        lnkDelete.Enabled = false;
                        lnkDelete.OnClientClick = "return false";
                        lnkDelete.ForeColor = Color.Silver;
                    }

                }
                catch { }
             
            }
        }

        protected void ddlEmpCardNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "call me", "load();", true);      
            findPromotionInfo(0);           
            txtNewGross.Text = "";
            txtIncrementAmount.Text = "";
        }

     

         
    }
}